(() => {
    $.namespace('CH.Modificacion');

    //#region CONST FILTROS
    const cboFiltroTipoModificacion = $("#cboFiltroTipoModificacion");
    const cboFiltroLineaNegocio = $("#cboFiltroLineaNegocio");
    const cboFiltroAreaDepartamento = $("#cboFiltroAreaDepartamento");
    const cboFiltroCC = $("#cboFiltroCC");
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroGuardar = $('#btnFiltroGuardar');
    const cboFiltroPuesto = $('#cboFiltroPuesto');
    const btnFiltroCalcular = $('#btnFiltroCalcular');
    const txtFiltroFechaAplicaCambio = $('#txtFiltroFechaAplicaCambio');
    const txtFiltroFechaRegistro = $('#txtFiltroFechaRegistro');
    //#endregion

    //#region CONST MODIFICACION TABULADORES
    let dtTabEmpleadosActivos;
    const tblTabEmpleadosActivos = $('#tblTabEmpleadosActivos');
    let dtTabPuestos;
    const tblTabPuestos = $('#tblTabPuestos');
    const txtPorcentajeGlobal = $('#txtPorcentajeGlobal');

    let contadorIndex = 0;
    let contadorIndexCategorias = 0;
    //#endregion

    //#region AUTORIZANTES
    const cboCE_Autorizante_CapitalHumano1 = $("#cboCE_Autorizante_CapitalHumano1");
    const cboCE_Autorizante_CapitalHumano2 = $("#cboCE_Autorizante_CapitalHumano2");
    const cboCE_Autorizante_GerenteSubdirectorDirector1 = $("#cboCE_Autorizante_GerenteSubdirectorDirector1");
    const cboCE_Autorizante_GerenteSubdirectorDirector2 = $("#cboCE_Autorizante_GerenteSubdirectorDirector2");
    const cboCE_Autorizante_LineaNegocio1 = $("#cboCE_Autorizante_LineaNegocio1");
    const cboCE_Autorizante_LineaNegocio2 = $("#cboCE_Autorizante_LineaNegocio2");
    const cboCE_Autorizante_AltaDireccion1 = $("#cboCE_Autorizante_AltaDireccion1");
    const cboCE_Autorizante_AltaDireccion2 = $("#cboCE_Autorizante_AltaDireccion2");
    //#endregion

    Modificacion = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblTabEmpleadosActivos();
            initTblTabPuestos();
            fncGetAccesosMenu();
            $("#menuModificacion").addClass("opcionSeleccionada");
            tblTabEmpleadosActivos.css("display", "none");
            tblTabPuestos.css("display", "none");
            //#endregion

            //#region FILTROS
            cboFiltroLineaNegocio.change(function () {
                if ($(this).val != "") {
                    cboFiltroCC.fillCombo('FillCboCC', { lstFK_LineaNegocio: getValoresMultiples('#cboFiltroLineaNegocio') }, false, 'Todos');
                    convertToMultiselect('#cboFiltroCC');
                }
                fncFillComboPuestos();
            });

            cboFiltroAreaDepartamento.change(function () {
                fncFillComboPuestos();
            });

            btnFiltroBuscar.click(function () {
                fncBuscarResultados();
            });

            btnFiltroGuardar.click(function () {
                fncCrearModificacion();
            });

            btnFiltroCalcular.click(function () {
                for (let i = 1; i < contadorIndex; i++) {
                    let porcModificacion = $(`#txtCE_Modificacion_Porc_${i}`).val();
                    let tipoNomina = $(`#txtCE_Modificacion_Porc_${i}`).attr("tipoNomina");
                    let esquemaPago = "70/30" //$(`#txtCE_Modificacion_Porc_${i}`).attr("esquemaPago");

                    // SE REALIZA CALCULO SI EL PORCENTAJE ES MAYOR A 0
                    if (porcModificacion > 0 && porcModificacion < 100) {
                        let nominal = unmaskNumero6DCompras($(`#txtCE_Actual_TotalNominal_${i}`).val());
                        let totalMensual = unmaskNumero6DCompras($(`#txtCE_Actual_SueldoMensual_${i}`).val());

                        let splitEsquemaPago = esquemaPago;
                        let tipoEsquemaPago = splitEsquemaPago.split('/');

                        if (tipoEsquemaPago.length == 2) {
                            totalMensual = totalMensual + ((porcModificacion / 100) * totalMensual);

                            if (tipoNomina == 1) {
                                nominal = (totalMensual / 30.4) * 7;
                            } else if (tipoNomina == 4) {
                                nominal = totalMensual / 2;
                            }

                            sueldoBase = nominal * ((tipoEsquemaPago[0] / 100));
                            complemento = nominal * ((tipoEsquemaPago[1] / 100));
                            nominal = sueldoBase + complemento;
                        } if (tipoEsquemaPago.length == 1) {
                            sueldoBase = nominal;
                            complemento = 0;
                        }

                        $(`#txtCE_Modificacion_SueldoBase_${i}`).val(maskNumero2DCompras(sueldoBase));
                        $(`#txtCE_Modificacion_Complemento_${i}`).val(maskNumero2DCompras(complemento));
                        $(`#txtCE_Modificacion_TotalNominal_${i}`).val(maskNumero2DCompras(nominal));
                        $(`#txtCE_Modificacion_SueldoMensual_${i}`).val(maskNumero2DCompras(totalMensual));
                    } else {
                        let nominal = unmaskNumero6DCompras($(`#txtCE_Modificacion_TotalNominal_${i}`).val());
                        let totalMensual = unmaskNumero6DCompras($(`#txtCE_Modificacion_SueldoMensual_${i}`).val());

                        let splitEsquemaPago = esquemaPago;
                        let tipoEsquemaPago = splitEsquemaPago.split('/');
                        let sueldoBase = 0;
                        let complemento = 0;

                        if (tipoEsquemaPago.length == 2) {
                            sueldoBase = nominal * ((tipoEsquemaPago[0] / 100));
                            complemento = nominal * ((tipoEsquemaPago[1] / 100));
                            nominal = sueldoBase + complemento;
                            if (tipoNomina == 4) {
                                totalMensual = nominal * 2;
                            }
                            else if (tipoNomina == 1) {
                                totalMensual = (nominal / 30.4) * 7;
                            }
                        } else if (tipoEsquemaPago.length == 1) {
                            sueldoBase = nominal;
                            complemento = 0;
                            nominal = sueldoBase;
                            totalMensual = (nominal / 30.4) * 7;
                        }

                        $(`#txtCE_Modificacion_SueldoBase_${i}`).val(maskNumero2DCompras(sueldoBase));
                        $(`#txtCE_Modificacion_Complemento_${i}`).val(maskNumero2DCompras(complemento));
                        $(`#txtCE_Modificacion_TotalNominal_${i}`).val(maskNumero2DCompras(nominal));
                        $(`#txtCE_Modificacion_SueldoMensual_${i}`).val(maskNumero2DCompras(totalMensual));
                    }
                }
            });

            cboFiltroCC.change(function () {
                fncFillComboPuestos();
            });
            //#endregion

            //#region FILL COMBO
            cboFiltroTipoModificacion.fillCombo('FillCboTipoModificaciones', null, false, null);
            cboFiltroLineaNegocio.fillCombo('FillCboLineaNegocios', {}, false, 'Todos');
            convertToMultiselect('#cboFiltroLineaNegocio');
            cboFiltroAreaDepartamento.fillCombo('FillCboAreasDepartamentos', {}, false, 'Todos');
            convertToMultiselect('#cboFiltroAreaDepartamento');

            cboCE_Autorizante_CapitalHumano1.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_CapitalHumano2.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_GerenteSubdirectorDirector1.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_GerenteSubdirectorDirector2.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_LineaNegocio1.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_LineaNegocio2.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_AltaDireccion1.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_AltaDireccion2.fillCombo('FillCboUsuarios', null, false, null);
            convertToMultiselect('#cboFiltroPuesto');
            convertToMultiselect('#cboFiltroCC');
            $(".select2").select2();

            // SE OBTIENE LA FECHA ACTUAL
            fncGetFechaActual();
            //#endregion

            //#region MODIFICACION TABULADORES
            txtPorcentajeGlobal.on("keypress", function () {
                $(".aumentoPorc").val($(this).val());
            });
            //#endregion
        }

        //#region FUNCIONES
        function fncBuscarResultados() {
            if (cboFiltroTipoModificacion.val() > 0) {
                fncDefaultCtrls("select2-cboFiltroTipoModificacion-container");
                switch (cboFiltroTipoModificacion.val()) {
                    case "1": // MODIFICACION A EMPLEADOS ACTIVOS
                        fncGetTabuladoresEmpleadosActivos();
                        tblTabEmpleadosActivos.css("display", "block");
                        tblTabPuestos.css("display", "none");
                        break;
                    case "2": // MODIFICACION_A_PUESTOS
                        fncGetTabuladoresPuestos();
                        tblTabEmpleadosActivos.css("display", "none");
                        tblTabPuestos.css("display", "block");
                        break;
                }
            } else {
                $("#select2-cboFiltroTipoModificacion-container").css('border', '2px solid red');
                Alert2Warning("Es necesario seleccionar el tipo de modificación.");
            }
        }

        function fncFillComboPuestos() {
            cboFiltroPuesto.fillCombo('FillCboFiltroPuestos_Reportes', { lstCC: getValoresMultiples('#cboFiltroCC'), lstFK_LineaNegocio: getValoresMultiples('#cboFiltroLineaNegocio') }, false, 'Todos');
            convertToMultiselect('#cboFiltroPuesto');
        }

        function fncSetCategorias() {
            for (let i = 1; i < contadorIndexCategorias + 1; i++) {
                let FK_Categoria = $(`#cboCE_Actual_Categoria_${i}`).val();
                $(`#cboCE_Actual_Categoria_${i}`).val(FK_Categoria);
                $(`#cboCE_Actual_Categoria_${i}`).trigger("change");
            }
        }

        function fncCrearModificacion() {
            // #region AUTORIZANTES
            let objGestion = {}
            let lstGestionAutorizantesDTO = []
            //#region CAPITAL HUMANO
            if (cboCE_Autorizante_CapitalHumano1.val() > 0) {
                objGestion = {}
                objGestion.nivelAutorizante = 0
                objGestion.FK_UsuarioAutorizacion = cboCE_Autorizante_CapitalHumano1.val()
                lstGestionAutorizantesDTO.push(objGestion)
            }

            if (cboCE_Autorizante_CapitalHumano2.val() > 0) {
                objGestion = {}
                objGestion.nivelAutorizante = 0
                objGestion.FK_UsuarioAutorizacion = cboCE_Autorizante_CapitalHumano2.val()
                lstGestionAutorizantesDTO.push(objGestion)
            }
            //#endregion

            //#region GERENTE / SUBDIRECTOR / DIRECTOR DE ÁREA
            if (cboCE_Autorizante_GerenteSubdirectorDirector1.val() > 0) {
                objGestion = {}
                objGestion.nivelAutorizante = 1
                objGestion.FK_UsuarioAutorizacion = cboCE_Autorizante_GerenteSubdirectorDirector1.val()
                lstGestionAutorizantesDTO.push(objGestion)
            }

            if (cboCE_Autorizante_GerenteSubdirectorDirector2.val() > 0) {
                objGestion = {}
                objGestion.nivelAutorizante = 1
                objGestion.FK_UsuarioAutorizacion = cboCE_Autorizante_GerenteSubdirectorDirector2.val()
                lstGestionAutorizantesDTO.push(objGestion)
            }
            //#endregion

            //#region DIRECTOR LINEA DE NEGOCIO
            if (cboCE_Autorizante_LineaNegocio1.val() > 0) {
                objGestion = {}
                objGestion.nivelAutorizante = 2
                objGestion.FK_UsuarioAutorizacion = cboCE_Autorizante_LineaNegocio1.val()
                lstGestionAutorizantesDTO.push(objGestion)
            }

            if (cboCE_Autorizante_LineaNegocio2.val() > 0) {
                objGestion = {}
                objGestion.nivelAutorizante = 2
                objGestion.FK_UsuarioAutorizacion = cboCE_Autorizante_LineaNegocio2.val()
                lstGestionAutorizantesDTO.push(objGestion)
            }
            //#endregion

            //#region ALTA DIRECCIÓN
            if (cboCE_Autorizante_AltaDireccion1.val() > 0) {
                objGestion = {}
                objGestion.nivelAutorizante = 3
                objGestion.FK_UsuarioAutorizacion = cboCE_Autorizante_AltaDireccion1.val()
                lstGestionAutorizantesDTO.push(objGestion)
            }

            if (cboCE_Autorizante_AltaDireccion2.val() > 0) {
                objGestion = {}
                objGestion.nivelAutorizante = 3
                objGestion.FK_UsuarioAutorizacion = cboCE_Autorizante_AltaDireccion2.val()
                lstGestionAutorizantesDTO.push(objGestion)
            }
            //#endregion
            //#endregion

            //#region TABULADORES MODIFICACION

            // REGISTRO PRINCIPAL
            let objParamDTO = {}
            objParamDTO.tipoModificacion = cboFiltroTipoModificacion.val()
            objParamDTO.lstFK_LineaNegocio = getValoresMultiples("#cboFiltroLineaNegocio")
            objParamDTO.lstFK_AreaDepartamento = getValoresMultiples("#cboFiltroAreaDepartamento")
            objParamDTO.lstCC = getValoresMultiples("#cboFiltroCC")
            objParamDTO.lstFK_Puestos = getValoresMultiples('#cboFiltroPuesto')
            objParamDTO.fechaAplicaCambio = txtFiltroFechaAplicaCambio.val()

            // REGISTRO DETALLE
            let obj = {}
            let lstParamDTO = []
            for (let i = 1; i < contadorIndex; i++) {
                let FK_Tabulador = $(`#txtCE_Modificacion_SueldoBase_${i}`).attr("FK_Tabulador");
                let FK_TabuladorDet = $(`#txtCE_Modificacion_SueldoBase_${i}`).attr("FK_TabuladorDet");
                let FK_Puesto = $(`#txtCE_Modificacion_SueldoBase_${i}`).attr("FK_Puesto");
                let sueldoBase = $(`#txtCE_Modificacion_SueldoBase_${i}`).val();
                let complemento = $(`#txtCE_Modificacion_Complemento_${i}`).val();
                let totalMensual = $(`#txtCE_Modificacion_SueldoMensual_${i}`).val();
                let porcentajeIncremento = $(`#txtCE_Modificacion_Porc_${i}`).val();
                let clave_empleado = $(`#txtCE_Modificacion_Porc_${i}`).attr("clave_empleado");
                let FK_Categoria = $(`#cboCE_Actual_Categoria_${i}`).val();
                let FK_LineaNegocio = $(`#txtCE_Modificacion_Porc_${i}`).attr("FK_LineaNegocio");
                let FK_EsquemaPago = $(`#cboCE_Actual_EsquemPago_${i}`).val();

                obj = {};
                obj.clave_empleado = clave_empleado;
                obj.FK_Puesto = FK_Puesto;
                obj.FK_Tabulador = FK_Tabulador;
                obj.FK_TabuladorDet = FK_TabuladorDet;
                obj.FK_LineaNegocio = FK_LineaNegocio;
                obj.FK_Categoria = FK_Categoria == undefined ? categoria : FK_Categoria;
                obj.FK_EsquemaPago = FK_EsquemaPago;
                obj.porcentajeIncremento = porcentajeIncremento;
                obj.fechaAplicaCambio = txtFiltroFechaAplicaCambio.val();
                obj.salario_base = unmaskNumero6DCompras(sueldoBase);
                obj.complemento = unmaskNumero6DCompras(complemento);
                obj.suma = unmaskNumero6DCompras(sueldoBase) + unmaskNumero6DCompras(complemento);
                obj.totalMensual = unmaskNumero6DCompras(totalMensual);
                lstParamDTO.push(obj);
            }
            //#endregion

            fncDefaultCtrls("txtFiltroFechaAplicaCambio")
            if (txtFiltroFechaAplicaCambio.val() != "") {
                obj = {};
                obj.objParamDTO = objParamDTO;
                obj.lstParamDTO = lstParamDTO;
                obj.lstGestionAutorizantesDTO = lstGestionAutorizantesDTO;
                axios.post('CrearModificacion', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncLimpiarCaptura();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                txtFiltroFechaAplicaCambio.css('border', '2px solid red');
                Alert2Warning("Es necesario indicar la fecha que aplica el cambio.");
            }
        }

        function fncLimpiarCaptura() {
            cboFiltroTipoModificacion[0].selectedIndex = 0;
            cboFiltroTipoModificacion.trigger("change");
            cboFiltroLineaNegocio[0].selectedIndex = 0;
            cboFiltroLineaNegocio.trigger("change");
            cboFiltroAreaDepartamento[0].selectedIndex = 0;
            cboFiltroAreaDepartamento.trigger("change");
            cboFiltroCC[0].selectedIndex = 0;
            cboFiltroCC.trigger("change");
            cboFiltroPuesto[0].selectedIndex = 0;
            cboFiltroPuesto.trigger("change");
            txtFiltroFechaAplicaCambio.val("");
            cboCE_Autorizante_CapitalHumano1[0].selectedIndex = 0;
            cboCE_Autorizante_CapitalHumano1.trigger("change");
            cboCE_Autorizante_CapitalHumano2[0].selectedIndex = 0;
            cboCE_Autorizante_CapitalHumano2.trigger("change");
            cboCE_Autorizante_GerenteSubdirectorDirector1[0].selectedIndex = 0;
            cboCE_Autorizante_GerenteSubdirectorDirector1.trigger("change");
            cboCE_Autorizante_GerenteSubdirectorDirector2[0].selectedIndex = 0;
            cboCE_Autorizante_GerenteSubdirectorDirector2.trigger("change");
            cboCE_Autorizante_LineaNegocio1[0].selectedIndex = 0;
            cboCE_Autorizante_LineaNegocio1.trigger("change");
            cboCE_Autorizante_LineaNegocio2[0].selectedIndex = 0;
            cboCE_Autorizante_LineaNegocio2.trigger("change");
            cboCE_Autorizante_AltaDireccion1[0].selectedIndex = 0;
            cboCE_Autorizante_AltaDireccion1.trigger("change");
            cboCE_Autorizante_AltaDireccion2[0].selectedIndex = 0;
            cboCE_Autorizante_AltaDireccion2.trigger("change");
        }
        //#endregion

        //#region MODIFICACIÓN A TABULADORES DE EMPLEADOS ACTIVOS
        function initTblTabEmpleadosActivos() {
            dtTabEmpleadosActivos = tblTabEmpleadosActivos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreEmpleado', title: 'Empleado' },
                    { data: 'puestoDesc', title: 'Puesto' },
                    { data: 'categoriaDesc', title: 'Cat' },
                    { data: 'tipoNominaDesc', title: 'Tipo nómina' },
                    { data: 'sueldoBaseStringActual', title: 'Sueldo base' },
                    { data: 'complementoStringActual', title: 'Complemento' },
                    { data: 'totalNominalStringActual', title: 'Total nominal' },
                    { data: 'sueldoMensualStringActual', title: 'Total mensual' },
                    { render: (data, type, row, meta) => { return `` } },
                    { data: 'categoriaDescModificacion', title: 'Cat' },
                    { data: 'tipoNominaDesc', title: 'Tipo nómina' },
                    { data: 'sueldoBaseStringModificacion', title: 'Sueldo base' },
                    { data: 'complementoStringModificacion', title: 'Complemento' },
                    { data: 'totalNominalStringModificacion', title: 'Total nominal' },
                    { data: 'sueldoMensualStringModificacion', title: 'Total mensual' },
                    { data: 'aumentoPorc', title: '%' },
                    {
                        visible: false,
                        render: (data, type, row) => {
                            contadorIndexCategorias = row.contadorIndex
                            return ""
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: '8%', targets: [15] }
                ],
                drawCallback: function (settings) {
                    $(".categoria").select2()
                    fncSetCategorias()
                }
            });
        }

        function fncGetTabuladoresEmpleadosActivos() {
            if (cboFiltroTipoModificacion.val() > 0) {
                let objParamDTO = {}
                objParamDTO.tipoModificacion = cboFiltroTipoModificacion.val()
                objParamDTO.lstFK_LineaNegocio = getValoresMultiples('#cboFiltroLineaNegocio')
                objParamDTO.lstFK_AreaDepartamento = getValoresMultiples('#cboFiltroAreaDepartamento')
                objParamDTO.lstCC = getValoresMultiples('#cboFiltroCC')
                objParamDTO.lstFK_Puestos = getValoresMultiples('#cboFiltroPuesto')
                axios.post('GetTabuladoresEmpleadosActivos', objParamDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtTabEmpleadosActivos.clear();
                        dtTabEmpleadosActivos.rows.add(response.data.lstEmpleadosActivosTabHistorial);
                        dtTabEmpleadosActivos.draw();
                        contadorIndex = response.data.contadorIndex;
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (cboFiltroTipoModificacion.val() <= 0) {
                    fncValidacionCtrl("cboFiltroModificacion", true, "Es necesario seleccionar el tipo de modificación.")
                    return false;
                }
            }
        }
        //#endregion

        //#region MODIFICACIÓN A PUESTOS
        function initTblTabPuestos() {
            dtTabPuestos = tblTabPuestos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'puestoDesc', title: 'Puesto' },
                    { data: 'lineaNegocioDesc', title: 'LN' },
                    { data: 'categoriaDesc', title: 'Cat' },
                    { data: 'esquemaPagoDesc', title: 'Esquema<br>pago' },
                    { data: 'tipoNominaDesc', title: 'Tipo nómina' },
                    { data: 'sueldoBaseStringActual', title: 'Sueldo base' },
                    { data: 'complementoStringActual', title: 'Complemento' },
                    { data: 'totalNominalStringActual', title: 'Total nominal' },
                    { data: 'sueldoMensualStringActual', title: 'Total mensual' },
                    { data: 'categoriaDescModificacion', title: 'Cat' },
                    { data: 'esquemaPagoDescModificacion', title: 'Esquema<br>pago' },
                    // { data: 'tipoNominaDesc', title: 'Tipo nómina' },
                    { data: 'sueldoBaseStringModificacion', title: 'Sueldo base' },
                    { data: 'complementoStringModificacion', title: 'Complemento' },
                    { data: 'totalNominalStringModificacion', title: 'Total nominal' },
                    { data: 'sueldoMensualStringModificacion', title: 'Total mensual' },
                    { data: 'aumentoPorc', title: '%' },
                    {
                        visible: false,
                        render: (data, type, row) => {
                            contadorIndexCategorias = row.contadorIndex
                            return ""
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: '8%', targets: [14] }
                ],
                drawCallback: function (settings) {
                    $(".categoria").select2()
                    fncSetCategorias()
                }
            });
        }

        function fncGetTabuladoresPuestos() {
            if (cboFiltroTipoModificacion.val() > 0) {
                let objParamDTO = {};
                objParamDTO.tipoModificacion = cboFiltroTipoModificacion.val();
                objParamDTO.lstFK_LineaNegocio = getValoresMultiples('#cboFiltroLineaNegocio');
                objParamDTO.lstFK_AreaDepartamento = getValoresMultiples('#cboFiltroAreaDepartamento');
                objParamDTO.lstCC = getValoresMultiples('#cboFiltroCC');
                objParamDTO.lstFK_Puestos = getValoresMultiples('#cboFiltroPuesto');
                axios.post('GetTabuladoresPuestos', objParamDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtTabPuestos.clear();
                        dtTabPuestos.rows.add(response.data.lstTabPuestos);
                        dtTabPuestos.draw();
                        contadorIndex = response.data.contadorIndex;
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (cboFiltroTipoModificacion.val() <= 0) {
                    fncValidacionCtrl("cboFiltroModificacion", true, "Es necesario seleccionar el tipo de modificación.");
                    return false;
                }
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncGetAccesosMenu() {
            axios.post('GetAccesosMenu').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    for (let i = 0; i <= 9; i++) {
                        switch (response.data.lstAccesosDTO[i]) {
                            case 0:
                                $("#menuLineaNegocios").css("display", "inline");
                                break;
                            case 1:
                                $("#menuPuestos").css("display", "inline");
                                break;
                            case 2:
                                $("#menuTabuladores").css("display", "inline");
                                break;
                            case 3:
                                $("#menuPlantillasPersonal").css("display", "inline");
                                break;
                            case 4:
                                $("#menuGestionTabuladores").css("display", "inline");
                                break;
                            case 5:
                                $("#menuGestionPlantillasPersonal").css("display", "inline");
                                break;
                            case 6:
                                $("#menuModificacion").css("display", "inline");
                                break;
                            case 7:
                                $("#menuGestionModificacion").css("display", "inline");
                                break;
                            case 8:
                                $("#menuReportes").css("display", "inline");
                                break;
                            case 9:
                                $("#menuGestionReportes").css("display", "inline");
                                break;
                        }
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetFechaActual() {
            axios.post('GetFechaActual').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    txtFiltroFechaRegistro.val(moment(response.data.fechaActual).format('DD/MM/YYYY'));
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Modificacion = new Modificacion();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();