(() => {
    $.namespace('CH.AsignarTabulador');

    //#region CONST FILTROS
    const btnFiltroBuscar = $("#btnFiltroBuscar")
    const btnFiltroNuevo = $("#btnFiltroNuevo")
    //#endregion

    //#region CONST CREAR/EDITAR ASIGNAR TABULADOR
    let dtAsignacionTabulador
    const tblAsignacionTabulador = $("#tblAsignacionTabulador")
    const mdlCEAsignacionTabuladores = $("#mdlCEAsignacionTabuladores")
    const btnCEAsignacionTabuladores = $("#btnCEAsignacionTabuladores")
    const txtCE_ID = $("#txtCE_ID")
    const cboCE_Puesto = $("#cboCE_Puesto")
    const txtCE_Nivel = $("#txtCE_Nivel")
    const cboCE_AreaDepartamento = $("#cboCE_AreaDepartamento")
    const cboCE_TipoNomina = $("#cboCE_TipoNomina")
    const cboCE_Sindicato = $("#cboCE_Sindicato")
    const cboCE_NivelMando = $("#cboCE_NivelMando")
    const divLineaNegocios = $('#divLineaNegocios')
    const divLineaNegocioDet = $('#divLineaNegocioDet')
    const cboCE_Autorizante_CapitalHumano1 = $("#cboCE_Autorizante_CapitalHumano1")
    const cboCE_Autorizante_CapitalHumano2 = $("#cboCE_Autorizante_CapitalHumano2")
    const cboCE_Autorizante_GerenteSubdirectorDirector1 = $("#cboCE_Autorizante_GerenteSubdirectorDirector1")
    const cboCE_Autorizante_GerenteSubdirectorDirector2 = $("#cboCE_Autorizante_GerenteSubdirectorDirector2")
    const cboCE_Autorizante_LineaNegocio1 = $("#cboCE_Autorizante_LineaNegocio1")
    const cboCE_Autorizante_LineaNegocio2 = $("#cboCE_Autorizante_LineaNegocio2")
    const cboCE_Autorizante_AltaDireccion1 = $("#cboCE_Autorizante_AltaDireccion1")
    const cboCE_Autorizante_AltaDireccion2 = $("#cboCE_Autorizante_AltaDireccion2")
    //#endregion

    //#region CONST LISTADO/ACTUALIZAR TABULADOR DET
    let dtTabuladorDet
    const tblTabuladorDet = $("#tblTabuladorDet")
    const mdlListadoActualizar_TabuladorDet = $("#mdlListadoActualizar_TabuladorDet")
    const divListadoTabuladores = $("#divListadoTabuladores")
    const divActualizarTabulador = $('#divActualizarTabulador')
    const txtActualizar_SueldoBase = $("#txtActualizar_SueldoBase")
    const txtActualizar_Complemento = $("#txtActualizar_Complemento")
    const txtActualizar_TotalNominal = $('#txtActualizar_TotalNominal')
    const txtActualizar_SueldoMensual = $("#txtActualizar_SueldoMensual")
    const cboActualizar_LineaNegocio = $("#cboActualizar_LineaNegocio")
    const cboActualizar_Categoria = $("#cboActualizar_Categoria")
    const cboActualizar_EsquemaPago = $("#cboActualizar_EsquemaPago")
    const btnActualizar_Cancelar = $('#btnActualizar_Cancelar')
    const btnActualizar_TabuladorDet = $("#btnActualizar_TabuladorDet")
    //#endregion

    //#region CONST MDL
    const mdlTabuladorDetalle = $('#mdlTabuladorDetalle');
    const tblTabuladoresDet = $('#tblTabuladoresDet');

    let dtTabuladoresDet;
    //#endregion

    //#region GLOBALES
    let _empresaActual = 0;
    _empresaActual = +$("#inputEmpresaActual").val();
    //#endregion

    AsignarTabulador = function () {
        (function init() {
            fncVerificarEmpresa();
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblAsignacionTabulador();
            fncGetAsignacionTabuladores();
            fncDisabledCampos();
            initTblTabuladorDet();
            initTblTabuladorDetalle();
            fncGetAccesosMenu();
            $("#menuTabuladores").addClass("opcionSeleccionada");
            //#endregion

            //#region FILL COMBOS
            cboCE_AreaDepartamento.fillCombo('FillCboAreasDepartamentos', null, false, null);
            cboCE_TipoNomina.fillCombo('FillCboTipoNomina', null, false, null);
            cboCE_Sindicato.fillCombo('FillCboSindicatos', null, false, null);
            cboCE_NivelMando.fillCombo('FillCboNivelMando', null, false, null);

            cboActualizar_LineaNegocio.fillCombo('FillCboLineaNegocios', null, false, null);
            cboActualizar_Categoria.fillCombo('FillCboCategorias', null, false, null);
            cboActualizar_EsquemaPago.fillCombo('FillCboEsquemaPagos', null, false, null);

            cboCE_Autorizante_CapitalHumano1.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_CapitalHumano2.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_GerenteSubdirectorDirector1.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_GerenteSubdirectorDirector2.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_LineaNegocio1.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_LineaNegocio2.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_AltaDireccion1.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_AltaDireccion2.fillCombo('FillCboUsuarios', null, false, null);

            cboCE_Autorizante_CapitalHumano1.val(1019);
            cboCE_Autorizante_CapitalHumano1.trigger("change");

            $(".select2").select2()
            //#endregion

            //#region FUNCIONES FILTROS
            btnFiltroBuscar.click(function () {
                fncGetAsignacionTabuladores()
            })

            btnFiltroNuevo.click(function () {
                cboCE_Puesto.fillCombo('FillCboPuestos', null, false, null);
                mdlCEAsignacionTabuladores.data().contador = 0
                btnCEAsignacionTabuladores.data().id = 0
                btnCEAsignacionTabuladores.html(`<i class='fas fa-save'></i>&nbsp;Guardar`)
                fncLimpiarCEAsignacionTabulador()
                btnCEAsignacionTabuladores.data().NUEVO_TABULADOR = true
                cboCE_Autorizante_CapitalHumano1.val(1019);
                cboCE_Autorizante_CapitalHumano1.trigger("change");
                fncInitCEAsignacionTabulador()
            })
            //#endregion

            //#region FUNCIONES CREAR/EDITAR ASIGNACIÓN TABULADORES
            cboCE_Puesto.change(function () {
                fncLimpiarCEAsignacionTabulador();
                fncInitCEAsignacionTabulador();
                if ($(this).val() > 0) {
                    fncGetInformacionPuesto()
                }
                fncDefaultCtrls("select2-cboCE_Puesto-container")
            })

            btnCEAsignacionTabuladores.click(function () {
                fncCEAsignacionTabuladores()
            })
            //#endregion

            //#region FUNCIONES LISTADO / ACTUALIZAR TABULADOR DET
            btnActualizar_Cancelar.click(function () {
                fncMostrarOcultarListadoTabuladoresDet(true)
            })

            btnActualizar_TabuladorDet.click(function () {
                fncActualizarTabuladorDet()
            })
            //#endregion
        }

        //#region FUNCIONES ASIGNACIÓN TABULADORES
        function fncInitCEAsignacionTabulador() {
            axios.post('InitCEAsignacionTabulador').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    divLineaNegocios.html(response.data.objHTML)

                    divLineaNegocios.find(".chkCE_LineaNegocio").each((id, element) => {
                        $(element).click(function () {
                            fncDefaultCtrls("select2-cboCE_Puesto-container")
                            if (cboCE_Puesto.val() <= 0) {
                                $("#select2-cboCE_Puesto-container").css('border', '2px solid red');
                                Alert2Warning("Es necesario seleccionar un puesto.")
                                $(this).prop("checked", false)
                            } else {
                                //#region PANEL LINEA NEGOCIO DETALLE
                                let idClassName = $(this).attr("id")
                                let option = cboCE_Puesto.find(`option[value="${cboCE_Puesto.val()}"]`);
                                let puesto = option.attr("data-prefijo");

                                if ($(this).prop("checked") == true) {
                                    let objHTML_PanelLineaNegocioDet =
                                        `<div id='divLineaNegocioDet_${$(this).attr("id")}'>
                                            <div id='panelGraficas' class='panel panel-default panel-principal'>
                                            <div class='panel-heading text-center'>ASIGNACIÓN DE TABULADOR</div>
                                            <div class='panel-body'>
                                                <div class="row">
                                                    <div class="col-lg-10">
                                                        <label for="" style="color: red">${$(this).closest("label").text()}</label>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <button class="btn btn-primary btn-block" id="btnNuevoRenglon_${$(this).attr("id")}"><i class='fas fa-plus'></i></button>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-12">
                                                        <label for="">[${cboCE_Puesto.val()}] ${puesto}</label>
                                                    </div>
                                                </div>
                                                <div id="seccion_${$(this).attr("id")}"></div>
                                            </div>
                                            </div>
                                        </div>`
                                    divLineaNegocioDet.append(objHTML_PanelLineaNegocioDet);
                                    let splitID_LineaNegocios = $(this).attr("id");
                                    let idLineaNegocio = splitID_LineaNegocios.split('_');

                                    let btnNuevoRenglon = `btnNuevoRenglon_${$(this).attr("id")}`;
                                    fncGetTabuladoresExistentes(cboCE_Puesto.val(), idLineaNegocio[2], idClassName, btnNuevoRenglon);

                                    //#region SE AGREGA NUEVO RENGLON A LA LINEA DE NEGOCIO
                                    $(`#btnNuevoRenglon_${idClassName}`).click(function () {
                                        let contador = mdlCEAsignacionTabuladores.data().contador
                                        contador = contador + 1
                                        mdlCEAsignacionTabuladores.data().contador = contador
                                        divLineaNegocioDet.data().contador = contador

                                        $(`#seccion_${idClassName}`).append(`
                                        <div style="font-size: 12px !important" id="row_${contador}">
                                            <div class='col-lg-2' style="text-align: center">
                                                <label for='id' title="Categoría">Categoría</label>
                                                <div class='input-group'>
                                                    <span class='input-group-addon' style="padding: 3px;"><i id="btnEliminarRow_${contador}" class="fas fa-times-circle"></i></span>
                                                    <select id='divLineaNegocioDet_cboCECategoria_${contador}' FK_LineaNegocio="${idLineaNegocio[2]}" class='form-control select2 cboCategorias' title="Categoría"></select>
                                                </div>
                                            </div>
                                            <div class='col-lg-2' style="text-align: center">
                                                <label for='id' title="Sueldo base">Sueldo base</label>
                                                <input type='text' id='divLineaNegocioDet_txtCESueldoBase_${contador}' class='form-control' title="Sueldo base">
                                            </div>
                                            <div class='col-lg-2' style="text-align: center">
                                                <label for='id' title="Complemento">Complemento</label>
                                                <input type='text' id='divLineaNegocioDet_txtCEComplemento_${contador}' class='form-control' title="Complemento">
                                            </div>
                                            <div class='col-lg-2' style="text-align: center">
                                                <label for='id' title="Total nominal">Total nominal</label>
                                                <input type='text' id='divLineaNegocioDet_txtCETotalNominal_${contador}' class='form-control' title="Total nominal">
                                            </div>
                                            <div class='col-lg-2' style="text-align: center">
                                                <label for='id' title="Sueldo mensual">Total Mensual</label>
                                                <input type='text' id='divLineaNegocioDet_txtCESueldoMensual_${contador}' onclick='$(this).select()' class='form-control' title="Sueldo mensual">
                                            </div>
                                            <div class='col-lg-2' style="text-align: center">
                                                <label for='id' title="Esquema de pago">Esquema pago</label>
                                                <select id='divLineaNegocioDet_txtCEEsquemaPago_${contador}' class='form-control select2' title="Esquema de pago"></select>
                                            </div>
                                        </div>`)

                                        //#region FILL COMBOS
                                        $(`#divLineaNegocioDet_cboCECategoria_${contador}`).fillCombo('FillCboCategorias', "--", false, "--");
                                        $(`#divLineaNegocioDet_cboCECategoria_${contador}`).find('option').get(0).remove();
                                        $(`#divLineaNegocioDet_txtCEEsquemaPago_${contador}`).fillCombo('FillCboEsquemaPagos', "--", false, "--");

                                        if (contador > 1) {
                                            let tipoEsquemaPago = $(`#divLineaNegocioDet_txtCEEsquemaPago_${1}`).val()
                                            if (+tipoEsquemaPago > 0) {
                                                $(`#divLineaNegocioDet_txtCEEsquemaPago_${contador}`).val(+tipoEsquemaPago)
                                                $(`#divLineaNegocioDet_txtCEEsquemaPago_${contador}`).trigger("change")
                                            }
                                        }
                                        //#endregion

                                        //#region SE OBTIENE EL SUELDO BASE, COMPLEMENTO Y TOTAL NOMINAL
                                        $(`#divLineaNegocioDet_txtCEEsquemaPago_${contador}`).change(function () {
                                            if (unmaskNumero6DCompras($(`#divLineaNegocioDet_txtCESueldoMensual_${contador}`).val())) {
                                                $(`#divLineaNegocioDet_txtCESueldoMensual_${contador}`).trigger('keyup');
                                            }
                                        });

                                        $(`#divLineaNegocioDet_txtCESueldoMensual_${contador}`).keyup(function () {
                                            if ($(`#divLineaNegocioDet_txtCEEsquemaPago_${contador}`).val() > 0) {
                                                let option = $(`#divLineaNegocioDet_txtCEEsquemaPago_${contador}`).find(`option[value="${$(`#divLineaNegocioDet_txtCEEsquemaPago_${contador}`).val()}"]`);
                                                let prefijo = option.attr("data-prefijo");
                                                let splitEsquemaPago = prefijo;
                                                let tipoEsquemaPago = splitEsquemaPago.split('/');
                                                let sueldoMensual = unmaskNumero($(`#divLineaNegocioDet_txtCESueldoMensual_${contador}`).val());

                                                let tipoNomina = cboCE_TipoNomina.val()
                                                let nomina = 0
                                                if (tipoNomina == 1) {
                                                    nomina = 7
                                                } else if (tipoNomina == 4) {
                                                    nomina = 15
                                                }

                                                if (tipoEsquemaPago.length == 2) { // EL 2 SE REFIERE A QUE EL TIPO DE PAGO ESTA DIVIDO, POR EJEMPLO: 70/30, PUEDE SER 20/80, ENTRE OTROS.
                                                    //#region SE INDICA EL SUELDO BASE, COMPLEMENTO Y SUELDO

                                                    if (tipoNomina == 1) {
                                                        sueldoBase = ((sueldoMensual * 0.7) / 30.4) * 7;
                                                        complemento = ((sueldoMensual * 0.3) / 30.4) * 7;
                                                        totalNominal = sueldoBase + complemento;
                                                    } else if (tipoNomina == 4) {
                                                        sueldoBase = (sueldoMensual * 0.7) / 2;
                                                        complemento = (sueldoMensual * 0.3) / 2;
                                                        totalNominal = sueldoBase + complemento;
                                                    }

                                                    if (_empresaActual == 6) {
                                                        $(`#divLineaNegocioDet_txtCESueldoBase_${contador}`).val(maskNumero2DCompras_PERU(sueldoBase.toFixed(2)))
                                                        $(`#divLineaNegocioDet_txtCEComplemento_${contador}`).val(maskNumero2DCompras_PERU(complemento.toFixed(2)))
                                                        $(`#divLineaNegocioDet_txtCETotalNominal_${contador}`).val(maskNumero2DCompras_PERU(totalNominal.toFixed(2)))
                                                    } else {
                                                        $(`#divLineaNegocioDet_txtCESueldoBase_${contador}`).val(maskNumero2DCompras(sueldoBase.toFixed(2)))
                                                        $(`#divLineaNegocioDet_txtCEComplemento_${contador}`).val(maskNumero2DCompras(complemento.toFixed(2)))
                                                        $(`#divLineaNegocioDet_txtCETotalNominal_${contador}`).val(maskNumero2DCompras(totalNominal.toFixed(2)))
                                                    }
                                                    //#endregion
                                                } else if (tipoEsquemaPago.length == 1) {
                                                    //#region TIPO DE PAGOS HARDCODEADOS
                                                    switch ($(`#divLineaNegocioDet_txtCEEsquemaPago_${contador}`).val()) {
                                                        case "2": // TIPO PAGO: 100
                                                            {
                                                                let sueldoBase = 0
                                                                let complemento = 0
                                                                let totalNominal = 0

                                                                if (tipoNomina == 1) {
                                                                    sueldoBase = sueldoMensual / 2;
                                                                    complemento = 0;
                                                                    totalNominal = sueldoBase + complemento;

                                                                    sueldoBase = ((sueldoMensual * 1) / 30.4) * 7;
                                                                    complemento = 0;
                                                                    totalNominal = sueldoBase + complemento;
                                                                } else if (tipoNomina == 4) {
                                                                    sueldoBase = (sueldoMensual * 1) / 2;
                                                                    complemento = 0;
                                                                    totalNominal = sueldoBase + complemento;
                                                                }

                                                                if (_empresaActual == 6) {
                                                                    $(`#divLineaNegocioDet_txtCESueldoBase_${contador}`).val(maskNumero2DCompras_PERU(sueldoBase.toFixed(2)))
                                                                    $(`#divLineaNegocioDet_txtCEComplemento_${contador}`).val(maskNumero2DCompras_PERU(complemento.toFixed(2)))
                                                                    $(`#divLineaNegocioDet_txtCETotalNominal_${contador}`).val(maskNumero2DCompras_PERU(totalNominal.toFixed(2)))
                                                                } else {
                                                                    $(`#divLineaNegocioDet_txtCESueldoBase_${contador}`).val(maskNumero2DCompras(sueldoBase.toFixed(2)))
                                                                    $(`#divLineaNegocioDet_txtCEComplemento_${contador}`).val(maskNumero2DCompras(complemento.toFixed(2)))
                                                                    $(`#divLineaNegocioDet_txtCETotalNominal_${contador}`).val(maskNumero2DCompras(totalNominal.toFixed(2)))
                                                                }
                                                                break;
                                                            }
                                                        case "3": // TIPO PAGO: LIBRE
                                                            {
                                                                break;
                                                            }
                                                    }
                                                    //#endregion
                                                }
                                            } else {
                                                $(this).val("")
                                                Alert2Warning("Es necesario indicar el esquema de pago.")
                                            }
                                        })

                                        //#region TIPO ESQUEMA: 100%, SE VALIDA QUE EL COMPLEMENTO SIEMPRE SEA $0,00.
                                        $(`#divLineaNegocioDet_txtCEComplemento_${contador}`).keyup(function () {
                                            let option = $(`#divLineaNegocioDet_txtCEEsquemaPago_${contador}`).find(`option[value="${$(`#divLineaNegocioDet_txtCEEsquemaPago_${contador}`).val()}"]`);
                                            let prefijo = option.attr("data-prefijo");
                                            let splitEsquemaPago = prefijo
                                            let tipoEsquemaPago = splitEsquemaPago.split('/')
                                            if (tipoEsquemaPago.length == 1 && $(`#divLineaNegocioDet_txtCEEsquemaPago_${contador}`).val() == 2) {
                                                let complemento = 0
                                                if (_empresaActual == 6) {
                                                    $(`#divLineaNegocioDet_txtCEComplemento_${contador}`).val(maskNumero2DCompras_PERU(complemento.toFixed(2)))
                                                } else {
                                                    $(`#divLineaNegocioDet_txtCEComplemento_${contador}`).val(maskNumero2DCompras(complemento.toFixed(2)))
                                                }
                                            }
                                        })
                                        //#endregion

                                        $(`#divLineaNegocioDet_txtCESueldoMensual_${contador}`).change(function () {
                                            let sueldoMensual = unmaskNumero($(`#divLineaNegocioDet_txtCESueldoMensual_${contador}`).val())
                                            if (_empresaActual == 6) {
                                                $(`#divLineaNegocioDet_txtCESueldoMensual_${contador}`).val(maskNumero2DCompras_PERU(sueldoMensual))
                                            } else {
                                                $(`#divLineaNegocioDet_txtCESueldoMensual_${contador}`).val(maskNumero2DCompras(sueldoMensual))
                                            }
                                        })
                                        //#endregion

                                        //#region SE ELIMINA EL ROW DE LA ASIGNACIÓN DEL TABULADOR
                                        $(`#btnEliminarRow_${contador}`).click(function () {
                                            $(`#row_${contador}`).remove()

                                            let i = mdlCEAsignacionTabuladores.data().contador
                                            i = i - 1
                                            mdlCEAsignacionTabuladores.data().contador = i
                                            divLineaNegocioDet.data().contador = i
                                        })
                                        //#endregion
                                    })
                                    //#endregion
                                } else {
                                    // SE ELIMINA LA ASIGNACIÓN DE TABULADOR SELECCIONADA
                                    $(`#divLineaNegocioDet_${$(this).attr("id")}`).remove()
                                }
                                //#endregion
                            }
                        })
                    });

                    mdlCEAsignacionTabuladores.modal("show")
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetInformacionPuesto() {
            if (cboCE_Puesto.val() > 0) {
                let obj = {}
                obj.puesto = cboCE_Puesto.val()
                axios.post('GetInformacionPuesto', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        txtCE_ID.val(response.data.objPuestoDTO.puesto)
                        cboCE_AreaDepartamento.val(response.data.objPuestoDTO.FK_AreaDepartamento)
                        cboCE_AreaDepartamento.trigger("change")
                        cboCE_TipoNomina.val(response.data.objPuestoDTO.FK_TipoNomina)
                        cboCE_TipoNomina.trigger("change")
                        cboCE_Sindicato.val(response.data.objPuestoDTO.FK_Sindicato)
                        cboCE_Sindicato.trigger("change")
                        cboCE_NivelMando.val(response.data.objPuestoDTO.FK_NivelMando)
                        cboCE_NivelMando.trigger("change")
                        txtCE_Nivel.val(response.data.objPuestoDTO.nivelMando)
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener la información del puesto.")
            }
        }

        function initTblAsignacionTabulador() {
            dtAsignacionTabulador = tblAsignacionTabulador.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'puestoDesc', title: 'Puesto' },
                    {
                        render: (data, type, row, meta) => {
                            let btnNuevo = `<button class="btn btn-xs btn-success nuevoTabuladorDetalle"><i class='fas fa-plus'></i></button>`
                            let btnDetalle = `<button class="btn btn-xs btn-primary tabuladorDetalle"><i class="fas fa-stream"></i></button>`
                            return `${btnNuevo} ${btnDetalle}`
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblAsignacionTabulador.on('click', '.editarRegistro', function () {
                        let rowData = dtAsignacionTabulador.row($(this).closest('tr')).data();
                        fncGetListadoTabuladoresDet(rowData.id);
                    });

                    tblAsignacionTabulador.on('click', '.eliminarRegistro', function () {
                        let rowData = dtAsignacionTabulador.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarTabuladoresPuesto(rowData.id));
                    });

                    tblAsignacionTabulador.on("click", ".tabuladorDetalle", function () {
                        let rowData = dtAsignacionTabulador.row($(this).closest("tr")).data();
                        mdlListadoActualizar_TabuladorDet.data().FK_Tabulador = rowData.id
                        fncGetTabuladorDetalle(rowData.id)
                    });

                    tblAsignacionTabulador.on("click", ".nuevoTabuladorDetalle", function () {
                        let rowData = dtAsignacionTabulador.row($(this).closest("tr")).data()
                        cboCE_Puesto.fillCombo('FillCboPuestos', null, false, null);
                        mdlCEAsignacionTabuladores.data().contador = 0
                        btnCEAsignacionTabuladores.data().id = 0
                        btnCEAsignacionTabuladores.html(`<i class='fas fa-save'></i>&nbsp;Guardar`)
                        fncLimpiarCEAsignacionTabulador()
                        btnCEAsignacionTabuladores.data().NUEVO_TABULADOR = false
                        fncInitCEAsignacionTabulador()
                        cboCE_Puesto.val(rowData.FK_Puesto)
                        cboCE_Puesto.trigger("change")
                        btnCEAsignacionTabuladores.data().FK_Tabulador = rowData.id
                    })
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { className: 'dt-body-center', 'targets': '_all' },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '12%', targets: [1] },
                    // { width: '7%', targets: [2] }
                ],
            });
        }

        function fncGetAsignacionTabuladores() {
            let obj = {}
            axios.post('GetAsignacionTabuladores', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtAsignacionTabulador.clear();
                    dtAsignacionTabulador.rows.add(response.data.lstTabuladores);
                    dtAsignacionTabulador.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEAsignacionTabuladores() {
            let lstTabuladoresDTO = fncCEOBJAsignacionTabuladores_Detalle()
            let lstGestionAutorizantesDTO = fncCEOBJAsignacionTabuladores_Gestion()
            if (cboCE_Puesto.val() > 0 && lstTabuladoresDTO != "" && lstGestionAutorizantesDTO != "") {
                axios.post('CrearAsignacionTabulador', {
                    FK_Puesto: cboCE_Puesto.val(),
                    FK_Tabulador: btnCEAsignacionTabuladores.data().FK_Tabulador,
                    lstTabuladoresDTO: lstTabuladoresDTO,
                    lstGestionAutorizantesDTO: lstGestionAutorizantesDTO
                }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        mdlCEAsignacionTabuladores.modal("hide")
                        fncGetAsignacionTabuladores()
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message))
            } else {
                if (cboCE_Puesto.val() <= 0) {
                    Alert2Warning("Es necesario seleccionar un puesto.")
                    $("#select2-cboCE_Puesto-container").css('border', '2px solid red')
                    return false
                }

                if (lstTabuladoresDTO == "") {
                    Alert2Warning("Es necesario indicar al menos un tabulador.")
                    return false
                }

                if (lstGestionAutorizantesDTO == "") {
                    Alert2Warning("Es necesario indicar al menos un autorizante.")
                }
            }
        }

        function fncCEOBJAsignacionTabuladores_Detalle() {
            let mensajeError = ""
            // if (txtCE_LineaNegocio.val() == "") { txtCE_LineaNegocio.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_LineaNegocio") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return ""
            } else {
                let contador = divLineaNegocioDet.data().contador
                let obj = {}
                let arr = []
                for (let i = 1; i < contador + 1; i++) {
                    obj = {}
                    obj.FK_LineaNegocio = $(`#divLineaNegocioDet_cboCECategoria_${i}`).attr("FK_LineaNegocio")
                    obj.FK_Categoria = $(`#divLineaNegocioDet_cboCECategoria_${i}`).val()
                    obj.sueldoBase = unmaskNumero($(`#divLineaNegocioDet_txtCESueldoBase_${i}`).val())
                    obj.complemento = unmaskNumero($(`#divLineaNegocioDet_txtCEComplemento_${i}`).val())
                    obj.totalNominal = unmaskNumero($(`#divLineaNegocioDet_txtCETotalNominal_${i}`).val())
                    obj.sueldoMensual = unmaskNumero($(`#divLineaNegocioDet_txtCESueldoMensual_${i}`).val())
                    obj.FK_EsquemaPago = $(`#divLineaNegocioDet_txtCEEsquemaPago_${i}`).val()
                    arr.push(obj)
                }
                return arr
            }
        }

        function fncCEOBJAsignacionTabuladores_Gestion() {
            let mensajeError = ""
            // if (txtCE_LineaNegocio.val() == "") { txtCE_LineaNegocio.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_LineaNegocio") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return ""
            } else {
                let obj = {}
                let arr = []

                //#region CAPITAL HUMANO
                if (cboCE_Autorizante_CapitalHumano1.val() > 0) {
                    obj = {}
                    obj.nivelAutorizante = 0
                    obj.FK_UsuarioAutorizacion = cboCE_Autorizante_CapitalHumano1.val()
                    arr.push(obj)
                }

                if (cboCE_Autorizante_CapitalHumano2.val() > 0) {
                    obj = {}
                    obj.nivelAutorizante = 0
                    obj.FK_UsuarioAutorizacion = cboCE_Autorizante_CapitalHumano2.val()
                    arr.push(obj)
                }
                //#endregion

                //#region GERENTE / SUBDIRECTOR / DIRECTOR DE ÁREA
                if (cboCE_Autorizante_GerenteSubdirectorDirector1.val() > 0) {
                    obj = {}
                    obj.nivelAutorizante = 1
                    obj.FK_UsuarioAutorizacion = cboCE_Autorizante_GerenteSubdirectorDirector1.val()
                    arr.push(obj)
                }

                if (cboCE_Autorizante_GerenteSubdirectorDirector2.val() > 0) {
                    obj = {}
                    obj.nivelAutorizante = 1
                    obj.FK_UsuarioAutorizacion = cboCE_Autorizante_GerenteSubdirectorDirector2.val()
                    arr.push(obj)
                }
                //#endregion

                //#region DIRECTOR DE LÍNEA DE NEGOCIO
                if (cboCE_Autorizante_LineaNegocio1.val() > 0) {
                    obj = {}
                    obj.nivelAutorizante = 2
                    obj.FK_UsuarioAutorizacion = cboCE_Autorizante_LineaNegocio1.val()
                    arr.push(obj)
                }

                if (cboCE_Autorizante_LineaNegocio2.val() > 0) {
                    obj = {}
                    obj.nivelAutorizante = 2
                    obj.FK_UsuarioAutorizacion = cboCE_Autorizante_LineaNegocio2.val()
                    arr.push(obj)
                }
                //#endregion

                //#region ALTA DIRECCIÓN
                if (cboCE_Autorizante_AltaDireccion1.val() > 0) {
                    obj = {}
                    obj.nivelAutorizante = 3
                    obj.FK_UsuarioAutorizacion = cboCE_Autorizante_AltaDireccion1.val()
                    arr.push(obj)
                }

                if (cboCE_Autorizante_AltaDireccion2.val() > 0) {
                    obj = {}
                    obj.nivelAutorizante = 3
                    obj.FK_UsuarioAutorizacion = cboCE_Autorizante_AltaDireccion2.val()
                    arr.push(obj)
                }
                //#endregion

                return arr
            }
        }

        function fncEliminarTabuladoresPuesto(idTabulador) {
            if (idTabulador > 0) {
                let obj = {}
                obj.id = idTabulador
                axios.post('EliminarTabuladoresPuesto', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetAsignacionTabuladores()
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncDisabledCampos() {
            txtCE_ID.attr("disabled", true)
            cboCE_AreaDepartamento.attr("disabled", true)
            cboCE_TipoNomina.attr("disabled", true)
            cboCE_Sindicato.attr("disabled", true)
            cboCE_NivelMando.attr("disabled", true)
            txtCE_Nivel.attr("disabled", true)
        }

        function fncLimpiarCEAsignacionTabulador() {
            $("input[type='text']").val("");
            cboCE_AreaDepartamento[0].selectedIndex = 0
            cboCE_AreaDepartamento.trigger("change")
            cboCE_TipoNomina[0].selectedIndex = 0
            cboCE_TipoNomina.trigger("change")
            cboCE_Sindicato[0].selectedIndex = 0
            cboCE_Sindicato.trigger("change")
            cboCE_NivelMando[0].selectedIndex = 0
            cboCE_NivelMando.trigger("change")
            $("#divLineaNegocioDet").html("")

            cboCE_Autorizante_CapitalHumano1[0].selectedIndex = 0
            cboCE_Autorizante_CapitalHumano1.trigger("change")
            cboCE_Autorizante_CapitalHumano2[0].selectedIndex = 0
            cboCE_Autorizante_CapitalHumano2.trigger("change")
            cboCE_Autorizante_GerenteSubdirectorDirector1[0].selectedIndex = 0
            cboCE_Autorizante_GerenteSubdirectorDirector1.trigger("change")
            cboCE_Autorizante_GerenteSubdirectorDirector2[0].selectedIndex = 0
            cboCE_Autorizante_GerenteSubdirectorDirector2.trigger("change")
            cboCE_Autorizante_LineaNegocio1[0].selectedIndex = 0
            cboCE_Autorizante_LineaNegocio1.trigger("change")
            cboCE_Autorizante_LineaNegocio2[0].selectedIndex = 0
            cboCE_Autorizante_LineaNegocio2.trigger("change")
            cboCE_Autorizante_AltaDireccion1[0].selectedIndex = 0
            cboCE_Autorizante_AltaDireccion1.trigger("change")
            cboCE_Autorizante_AltaDireccion2[0].selectedIndex = 0
            cboCE_Autorizante_AltaDireccion2.trigger("change")
        }

        function fncGetTabuladoresExistentes(FK_Puesto, FK_LineaNegocio, idClassName, btnNuevoRenglon) {
            if (FK_Puesto > 0 && FK_LineaNegocio > 0) {
                let obj = {};
                obj.FK_Puesto = FK_Puesto;
                obj.FK_LineaNegocio = FK_LineaNegocio;
                axios.post('GetTabuladoresExistentes', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        if (btnCEAsignacionTabuladores.data().NUEVO_TABULADOR) {
                            if (response.data.tabuladorExistente) {
                                $(`#${btnNuevoRenglon}`).css("display", "none");
                            }
                        }

                        for (let i = 0; i < response.data.lstDTO.length; i++) {
                            let sueldoBase = 0;
                            let complemento = 0;
                            let totalNominal = 0;
                            let sueldoMensual = 0;

                            if (_empresaActual == 6) {
                                sueldoBase = maskNumero2DCompras_PERU(response.data.lstDTO[i].sueldoBase)
                                complemento = maskNumero2DCompras_PERU(response.data.lstDTO[i].complemento)
                                totalNominal = maskNumero2DCompras_PERU(response.data.lstDTO[i].totalNominal)
                                sueldoMensual = maskNumero2DCompras_PERU(response.data.lstDTO[i].sueldoMensual)
                            } else {
                                sueldoBase = maskNumero2DCompras(response.data.lstDTO[i].sueldoBase)
                                complemento = maskNumero2DCompras(response.data.lstDTO[i].complemento)
                                totalNominal = maskNumero2DCompras(response.data.lstDTO[i].totalNominal)
                                sueldoMensual = maskNumero2DCompras(response.data.lstDTO[i].sueldoMensual)
                            }

                            $(`#seccion_${idClassName}`).append(`
                            <div style="font-size: 12px !important">
                                <div class='col-lg-2' style="text-align: center">
                                    <label for='id' title="Categoría">Categoría</label>
                                    <div class='input-group'>
                                        <span class='input-group-addon' style="padding: 3px;"></span>
                                        <input type='text' class='form-control' title="Categoría" disabled value="${response.data.lstDTO[i].categoriaDesc}">
                                    </div>
                                </div>
                                <div class='col-lg-2' style="text-align: center">
                                    <label for='id' title="Sueldo base">Sueldo base</label>
                                    <input type='text' class='form-control' title="Sueldo base" disabled value="${sueldoBase}">
                                </div>
                                <div class='col-lg-2' style="text-align: center">
                                    <label for='id' title="Complemento">Complemento</label>
                                    <input type='text' class='form-control' title="Complemento" disabled value="${complemento}">
                                </div>
                                <div class='col-lg-2' style="text-align: center">
                                    <label for='id' title="Total nominal">Total nominal</label>
                                    <input type='text' class='form-control' title="Total nominal" disabled value="${totalNominal}">
                                </div>
                                <div class='col-lg-2' style="text-align: center">
                                    <label for='id' title="Sueldo mensual">Total Mensual</label>
                                    <input type='text' class='form-control' title="Sueldo mensual" disabled value="${sueldoMensual}">
                                </div>
                                <div class='col-lg-2' style="text-align: center">
                                    <label for='id' title="Esquema de pago">Esquema pago</label>
                                    <input type='text' class='form-control' title="Sueldo mensual" disabled value="${response.data.lstDTO[i].esquemaPagoDesc}">
                                </div>
                            </div>`)
                        }
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (FK_Puesto <= 0) { Alert2Warning("Ocurrió un error al verificar la existencia de tabuladores."); }
                if (FK_LineaNegocio <= 0) { Alert2Warning("Ocurrió un error al verificar la existencia de tabuladores."); }
            }
        }
        //#endregion

        //#region LISTADO / ACTUALIZAR TABULADOR DET
        function initTblTabuladorDet() {
            dtTabuladorDet = tblTabuladorDet.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'lineaNegocioDesc', title: 'Linea negocio' },
                    { data: 'categoriaDesc', title: 'Categoría' },
                    {
                        data: 'sueldoBase', title: 'Sueldo base',
                        render: function (data, type, row) {
                            if (_empresaActual == 6) {
                                return maskNumero2DCompras_PERU(row.sueldoBase)
                            }
                            else {
                                return maskNumero2DCompras(row.sueldoBase)
                            }
                        }
                    },
                    {
                        data: 'complemento', title: 'Complemento',
                        render: function (data, type, row) {
                            if (_empresaActual == 6) {
                                return maskNumero2DCompras_PERU(row.complemento)
                            } else {
                                return maskNumero2DCompras(row.complemento)
                            }
                        }
                    },
                    {
                        data: 'totalNominal', title: 'Total nominal',
                        render: function (data, type, row) {
                            if (_empresaActual == 6) {
                                return maskNumero2DCompras_PERU(row.totalNominal)
                            } else {
                                return maskNumero2DCompras(row.totalNominal)
                            }
                        }
                    },
                    {
                        data: 'sueldoMensual', title: 'Sueldo mensual',
                        render: function (data, type, row) {
                            if (_empresaActual == 6) {
                                return maskNumero2DCompras_PERU(row.sueldoMensual)
                            } else {
                                return maskNumero2DCompras(row.sueldoMensual)
                            }
                        }
                    },
                    { data: 'esquemaPagoDesc', title: 'Esquema pago' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return `${btnEditar} ${btnEliminar}`
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblTabuladorDet.on('click', '.editarRegistro', function () {
                        let rowData = dtTabuladorDet.row($(this).closest('tr')).data();
                        fncGetDatosActualizarTabuladorDet(rowData.id)
                    });

                    tblTabuladorDet.on('click', '.eliminarRegistro', function () {
                        let rowData = dtTabuladorDet.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarTabuladorDet(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { className: 'dt-body-center', 'targets': '_all' },
                    { className: 'dt-body-right', targets: [2, 3, 4, 5] },
                    { width: '50%', targets: [0] }
                ]
                // ,
                // drawCallback: function (settings) {
                //     // dtTabuladorDet.columns.adjust().draw()
                // }
            });
        }

        function fncGetListadoTabuladoresDet(FK_Tabulador) {
            if (FK_Tabulador > 0) {
                let obj = {}
                obj.FK_Tabulador = FK_Tabulador
                axios.post('GetListadoTabuladoresDet', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        fncMostrarOcultarListadoTabuladoresDet(true)
                        dtTabuladorDet.clear();
                        dtTabuladorDet.rows.add(response.data.lstTabuladoresDet);
                        dtTabuladorDet.draw();
                        mdlListadoActualizar_TabuladorDet.modal("show")
                        mdlListadoActualizar_TabuladorDet.data().FK_Tabulador = FK_Tabulador
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener los tabuladores.")
            }
        }

        function fncGetDatosActualizarTabuladorDet(idTabuladorDet) {
            if (idTabuladorDet > 0) {
                let obj = {}
                obj.id = idTabuladorDet
                axios.post('GetDatosActualizarTabuladorDet', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncDefaultCtrls("txtActualizar_SueldoBase")
                        fncDefaultCtrls("txtActualizar_Complemento")
                        fncDefaultCtrls("txtActualizar_TotalNominal")
                        fncDefaultCtrls("txtActualizar_SueldoMensual")
                        fncDefaultCtrls("select2-cboActualizar_LineaNegocio-container")
                        fncDefaultCtrls("select2-cboActualizar_Categoria-container")
                        fncDefaultCtrls("select2-cboActualizar_EsquemaPago-container")

                        fncMostrarOcultarListadoTabuladoresDet(false)
                        btnActualizar_TabuladorDet.data().idTabuladorDet = idTabuladorDet
                        txtActualizar_SueldoBase.val(response.data.objTabuladorDet.sueldoBase)
                        txtActualizar_Complemento.val(response.data.objTabuladorDet.complemento)
                        txtActualizar_TotalNominal.val(response.data.objTabuladorDet.totalNominal)
                        txtActualizar_SueldoMensual.val(response.data.objTabuladorDet.sueldoMensual)
                        cboActualizar_LineaNegocio.val(response.data.objTabuladorDet.FK_LineaNegocio)
                        cboActualizar_LineaNegocio.trigger("change")
                        cboActualizar_Categoria.val(response.data.objTabuladorDet.FK_Categoria)
                        cboActualizar_Categoria.trigger("change")
                        cboActualizar_EsquemaPago.val(response.data.objTabuladorDet.FK_EsquemaPago)
                        cboActualizar_EsquemaPago.trigger("change")
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
            else {
                Alert2Warning("Ocurrió un error al obtener la información del tabulador.")
            }
        }

        function fncActualizarTabuladorDet() {
            let obj = fncActualizarOBJTabuladorDet()
            if (btnActualizar_TabuladorDet.data().idTabuladorDet > 0 && obj != "") {
                axios.post('ActualizarTabuladorDet', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetListadoTabuladoresDet(mdlListadoActualizar_TabuladorDet.data().FK_Tabulador)
                        Alert2Exito(message)
                        btnActualizar_Cancelar.trigger("click")
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (btnActualizar_TabuladorDet.data().idTabuladorDet <= 0) {
                    Alert2Warning("Ocurrió un error al actualizar la información")
                }
            }
        }

        function fncActualizarOBJTabuladorDet() {
            let mensajeError = ""
            if (txtActualizar_SueldoBase.val() == "") { txtActualizar_SueldoBase.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtActualizar_SueldoBase") }
            if (txtActualizar_Complemento.val() == "") { txtActualizar_Complemento.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtActualizar_Complemento") }
            if (txtActualizar_TotalNominal.val() == "") { txtActualizar_TotalNominal.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtActualizar_TotalNominal") }
            if (txtActualizar_SueldoMensual.val() == "") { txtActualizar_SueldoMensual.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtActualizar_SueldoMensual") }
            if (cboActualizar_LineaNegocio.val() <= 0) { $("#select2-cboActualizar_LineaNegocio-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboActualizar_LineaNegocio-container") }
            if (cboActualizar_Categoria.val() <= 0) { $("#select2-cboActualizar_Categoria-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboActualizar_Categoria-container") }
            if (cboActualizar_EsquemaPago.val() <= 0) { $("#select2-cboActualizar_EsquemaPago-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboActualizar_EsquemaPago-container") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return ""
            } else {
                let obj = new Object()
                obj.id = btnActualizar_TabuladorDet.data().idTabuladorDet
                obj.FK_LineaNegocio = cboActualizar_LineaNegocio.val()
                obj.FK_Categoria = cboActualizar_Categoria.val()
                obj.sueldoBase = txtActualizar_SueldoBase.val()
                obj.complemento = txtActualizar_Complemento.val()
                obj.totalNominal = txtActualizar_TotalNominal.val()
                obj.sueldoMensual = txtActualizar_SueldoMensual.val()
                obj.FK_EsquemaPago = cboActualizar_EsquemaPago.val()
                return obj
            }
        }

        function fncEliminarTabuladorDet(idTabuladorDet) {
            if (idTabuladorDet > 0) {
                let obj = {}
                obj.id = idTabuladorDet
                axios.post('EliminarTabuladorDet', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetTabuladorDetalle(mdlListadoActualizar_TabuladorDet.data().FK_Tabulador)
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el registro.")
            }
        }

        function fncMostrarOcultarListadoTabuladoresDet(display) {
            if (display) {
                divListadoTabuladores.css("display", "inline")
                divActualizarTabulador.css("display", "none")
            } else {
                divListadoTabuladores.css("display", "none")
                divActualizarTabulador.css("display", "inline")
            }
        }

        function initTblTabuladorDetalle() {
            dtTabuladoresDet = tblTabuladoresDet.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: true,
                columns: [
                    { data: 'lineaNegocioDesc', title: 'Línea de negocio' },
                    { data: 'categoriaDesc', title: 'Categoría' },
                    {
                        title: "Sueldo base",
                        render: (data, type, row, meta) => {
                            if (_empresaActual == 6) {
                                return maskNumero2DCompras_PERU(row.sueldoBase);
                            } else {
                                return maskNumero2DCompras(row.sueldoBase);
                            }
                        }
                    },
                    {
                        title: "Complemento",
                        render: (data, type, row, meta) => {
                            if (_empresaActual == 6) {
                                return maskNumero2DCompras_PERU(row.complemento);
                            } else {
                                return maskNumero2DCompras(row.complemento);
                            }
                        }
                    },
                    {
                        title: "Total nominal",
                        render: (data, type, row, meta) => {
                            if (_empresaActual == 6) {
                                return maskNumero2DCompras_PERU(row.totalNominal);
                            } else {
                                return maskNumero2DCompras(row.totalNominal);
                            }
                        }
                    },
                    {
                        title: "Sueldo mensual",
                        render: (data, type, row, meta) => {
                            if (_empresaActual == 6) {
                                return maskNumero2DCompras_PERU(row.sueldoMensual);
                            } else {
                                return maskNumero2DCompras(row.sueldoMensual);
                            }
                        }
                    },
                    { data: 'esquemaPagoDesc', title: "Esquema de pago" },
                    {
                        render: (data, type, row, meta) => {
                            return `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblTabuladoresDet.on('click', '.eliminarRegistro', function () {
                        let rowData = dtTabuladoresDet.row($(this).closest('tr')).data()
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarTabuladorDet(rowData.id, rowData.FK_Tabulador))
                    })
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', 'targets': '_all' },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function fncGetTabuladorDetalle(FK_Tabulador) {
            if (FK_Tabulador > 0) {
                let objParamDTO = {}
                objParamDTO.FK_Tabulador = FK_Tabulador
                objParamDTO.tabuladorDetAutorizado = 1;
                axios.post('GetDetalleRelTabulador', objParamDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtTabuladoresDet.clear();
                        dtTabuladoresDet.rows.add(response.data.lstTabuladorDet);
                        dtTabuladoresDet.draw();
                        mdlTabuladorDetalle.modal("show")
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener el detalle del tabulador.")
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

        function fncVerificarEmpresa() {
            if (_empresaActual == 6) {
                // CODE ...
            } else {
                // CODE ...
            }
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.AsignarTabulador = new AsignarTabulador();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();