(() => {
    $.namespace('CH.PlantillaPersonal');

    PlantillaPersonal = function () {

        //#region CONST FILTROS
        const cboFiltroCC = $("#cboFiltroCC");
        const cboFiltroLineaNegocio = $("#cboFiltroLineaNegocio");
        const cboFiltroPuesto = $('#cboFiltroPuesto');
        const btnFiltroBuscar = $("#btnFiltroBuscar");
        const btnFiltroNuevo = $("#btnFiltroNuevo");
        const lblCantPuestosSeleccionados = $('#lblCantPuestosSeleccionados');
        _PRIMERA_BUSQUEDA = true;
        //#endregion

        //#region REGISTRAR PLANTILLA / AUTORIZANTES
        let dtPuestosTabuladores;
        const tblPuestosTabuladores = $('#tblPuestosTabuladores');

        const cboCE_Autorizante_ResponsableCC = $("#cboCE_Autorizante_ResponsableCC");
        const cboCE_Autorizante_CapitalHumano1 = $("#cboCE_Autorizante_CapitalHumano1");
        const cboCE_Autorizante_LineaNegocio1 = $("#cboCE_Autorizante_LineaNegocio1");
        const cboCE_Autorizante_LineaNegocio2 = $("#cboCE_Autorizante_LineaNegocio2");
        const cboCE_Autorizante_AltaDireccion1 = $("#cboCE_Autorizante_AltaDireccion1");
        const cboCE_Autorizante_AltaDireccion2 = $("#cboCE_Autorizante_AltaDireccion2");

        const txtCEFechaInicio = $("#txtCEFechaInicio");
        const txtCEFechaFin = $("#txtCEFechaFin");
        const mdlCargando = $('#mdlCargando');
        //#endregion

        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblPlantillaPersonal();
            btnFiltroNuevo.attr("disabled", true);
            fncGetAccesosMenu();
            $("#menuPlantillasPersonal").addClass("opcionSeleccionada");

            txtCEFechaInicio.datepicker({
                dateFormat: 'dd-mm-yy',
                defaultDate: new Date()
            });

            txtCEFechaFin.datepicker({
                dateFormat: 'dd-mm-yy',
                defaultDate: new Date()
            });

            btnFiltroBuscar.click();
            //#endregion

            //#region FILTROS
            btnFiltroBuscar.click(function () {
                fncGetTabuladoresAutorizados();
            });

            btnFiltroNuevo.click(function () {
                fncCrearSolicitudPlantilla();
            });

            cboFiltroCC.change(function () {
                if ($(this).val() != "") {
                    cboFiltroLineaNegocio.fillCombo('FillCboFiltroLineaNegocios_PlantillaPersonal', { cc: $(this).val() }, false, null);
                    cboFiltroLineaNegocio[0].selectedIndex = 1;
                    cboFiltroLineaNegocio.trigger("change");
                }
            });

            cboFiltroLineaNegocio.change(function () {
                if ($(this).val() != "") {
                    cboFiltroPuesto.fillCombo('FillCboFiltroPuestos_PlantillaPersonal', { FK_LineaNegocio: $(this).val() }, false, null);
                    cboFiltroPuesto.find('option').get(0).remove();
                }
            });

            cboFiltroPuesto.change(function () {
                lblCantPuestosSeleccionados.html(`| seleccionado: ${$("#cboFiltroPuesto :selected").length}`);
            });
            //#endregion

            //#region FILL COMBOS
            cboFiltroCC.fillCombo('FillCboFiltroCC_PlantillaPersonal', null, false, null);
            cboCE_Autorizante_ResponsableCC.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_CapitalHumano1.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_LineaNegocio1.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_LineaNegocio2.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_AltaDireccion1.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Autorizante_AltaDireccion2.fillCombo('FillCboUsuarios', null, false, null);
            $(".select2").select2();
            cboFiltroPuesto.select2({
                closeOnSelect: false,
                selectOnClose: false,
                allowClear: false,
                dropdownAutoWidth: false
            });
            //#endregion
        }

        //#region FUNCIONES PLANTILLA DE PERSONAL
        function initTblPlantillaPersonal() {
            dtPuestosTabuladores = tblPuestosTabuladores.DataTable({
                language: dtDicEsp,
                paging: true,
                columns: [
                    { data: 'personalNecesario', title: 'Personal necesario' },
                    { data: 'idPuesto', title: 'ID', visible: false },
                    {
                        data: 'puestoDesc', title: 'Puesto',
                        render: (data, type, row, meta) => {
                            return `[${row.idPuesto}] ${row.puestoDesc}`;
                        }
                    },
                    { data: 'tipoNominaDesc', title: 'Tipo nómina' },
                    { data: 'areaDepartamentoDesc', title: 'Área / Departamento' },
                    { data: 'categoriaDesc', title: 'Cat' },
                    { data: 'sueldoBaseString', title: 'Sueldo base' },
                    { data: 'complementoString', title: 'Complemento' },
                    { data: 'totalNominalString', title: 'Total nominal' },
                    { data: 'sueldoMensualString', title: 'Sueldo mensual' },
                    { data: 'esquemaPagoDescString', title: 'Esquema' },
                    {
                        title: "Acciones",
                        render: (data, type, row, meta) => {
                            return `<button class="btn btn-danger eliminarRenglon" title="Eliminar renglón."><i class='fas fa-trash'></i></button>`;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblPuestosTabuladores.on('change', '.personalNecesario', function (row) {
                        let cantNecesaria = 0;
                        let inputs = dtPuestosTabuladores.column(0).nodes().toArray();

                        for (const item of inputs) {
                            let valor = $(item.firstChild).val();
                            if (valor == "") {
                                $(item.firstChild).val(0)
                            }
                            cantNecesaria += (valor == "" || valor == undefined || isNaN(valor) ? 0 : +Number(valor))
                        }
                        tblPuestosTabuladores.find('tfoot tr th:eq(0)').html(`Personal necesario: ${cantNecesaria}`);
                    });

                    tblPuestosTabuladores.on("click", ".eliminarRenglon", function (row) {
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => {
                            dtPuestosTabuladores.row($(this).closest('tr')).remove().draw();
                        });
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', 'targets': '_all' },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
                footerCallback: function (row, data, start, end, display) {
                    var api = this.api();

                    let cantPuestos = 0;
                    let arrPuestos = cboFiltroPuesto.val();

                    let cantNecesaria = 0;

                    let inputs = api.column(0).nodes().toArray();

                    for (const item of inputs) {
                        let valor = $(item.firstChild).val();

                        if (valor == "") {
                            $(item.firstChild).val(0)
                        }

                        cantNecesaria += (valor == "" || valor == undefined || isNaN(valor) ? 0 : +Number(valor))
                    }


                    $(api.column(0).footer()).html(`Personal necesario: ${0}`);
                    $(api.column(3).footer()).html(`Cant. puestos: ${cantPuestos}`);
                    tblPuestosTabuladores.find('tfoot tr th:eq(0)').html(`Personal necesario: ${cantNecesaria}`);
                }
            });
        }

        function fncGetTabuladoresAutorizados() {
            fncDefaultCtrls("select2-cboFiltroCC-container")
            fncDefaultCtrls("select2-cboFiltroLineaNegocio-container")

            let lstPuestosDT = new Array();
            var rows = tblPuestosTabuladores.DataTable().rows().data().toArray();
            let objPersonalNecesario = {}
            let lstPersonalNecesario = []
            rows.forEach(function (row) {
                objPersonalNecesario = {}
                objPersonalNecesario.cantPersonalNecesario = $(`#txtCEPersonalNecesario${row.idPuesto}`).val();
                objPersonalNecesario.FK_Puesto = row.idPuesto;
                lstPersonalNecesario.push(objPersonalNecesario);
            });

            if (cboFiltroCC.val() != "" && cboFiltroLineaNegocio.val() > 0 && getValoresMultiples('#cboFiltroPuesto').length > 0) {
                let obj = {}
                obj.cc = cboFiltroCC.val();
                obj.FK_LineaNegocio = cboFiltroLineaNegocio.val();
                obj.lstPuestos = getValoresMultiples('#cboFiltroPuesto');
                obj.lstPersonalNecesarioDTO = lstPersonalNecesario;
                axios.post('GetTabuladoresAutorizados', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtPuestosTabuladores.clear();
                        dtPuestosTabuladores.rows.add(response.data.lstTabuladoresDTO);
                        dtPuestosTabuladores.draw();
                        btnFiltroNuevo.attr("disabled", false);
                        //#endregion
                    } else {
                        Alert2Warning(message);
                        btnFiltroNuevo.attr("disabled", true)
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (cboFiltroCC.val() == "") {
                    Alert2Warning("Es necesario seleccionar un CC.")
                    $("#select2-cboFiltroCC-container").css('border', '2px solid red')
                    return false
                }

                if (cboFiltroLineaNegocio.val() <= 0) {
                    Alert2Warning("Es necesario seleccionar una linea de negocio.")
                    $("#select2-cboFiltroLineaNegocio-container").css('border', '2px solid red')
                    return false
                }

                if (getValoresMultiples('#cboFiltroPuesto').length <= 0) {
                    Alert2Warning("Es necesario seleccionar al menos un puesto.")
                    // $("#select2-cboFiltroPuesto-container").css('border', '2px solid red')
                    return false
                }
            }
        }

        function fncCrearSolicitudPlantilla() {
            fncDefaultCtrls("select2-cboFiltroCC-container")
            fncDefaultCtrls("select2-cboFiltroLineaNegocio-container")
            fncDefaultCtrls("txtCEFechaInicio")
            fncDefaultCtrls("txtCEFechaFin")
            if (cboFiltroCC.val() != "" && cboFiltroLineaNegocio.val() > 0 && txtCEFechaInicio.val() != "" && txtCEFechaFin.val() != "") {
                //#region SE OBTIENE LISTADO DE PERSONAL NECESARIO
                let lstPersonalNecesario = []
                let lstPuestosID = []

                let inputsCol1 = dtPuestosTabuladores.column(0).nodes().toArray();
                let inputsCol2 = dtPuestosTabuladores.column(1).data().toArray();

                for (const item of inputsCol1) {
                    let valor = $(item.firstChild).val();

                    lstPersonalNecesario.push(valor)
                }

                for (const item of inputsCol2) {
                    lstPuestosID.push(item)
                }
                //#endregion

                //#region AUTORIZANTES
                let objGestion = {}
                let lstGestionAutorizantesDTO = []
                //#region RESPONSABLE CC
                if (cboCE_Autorizante_ResponsableCC.val() > 0) {
                    objGestion = {}
                    objGestion.nivelAutorizante = 5
                    objGestion.FK_UsuarioAutorizacion = cboCE_Autorizante_ResponsableCC.val()
                    lstGestionAutorizantesDTO.push(objGestion)
                }
                //#endregion

                //#region CAPITAL HUMANO
                if (cboCE_Autorizante_CapitalHumano1.val() > 0) {
                    objGestion = {}
                    objGestion.nivelAutorizante = 0
                    objGestion.FK_UsuarioAutorizacion = cboCE_Autorizante_CapitalHumano1.val()
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

                let objParamDTO = {}
                objParamDTO.cc = cboFiltroCC.val();
                objParamDTO.FK_LineaNegocio = cboFiltroLineaNegocio.val();
                objParamDTO.lstPersonalNecesario = lstPersonalNecesario;
                objParamDTO.lstPuestosID = lstPuestosID;
                objParamDTO.lstGestionAutorizantesDTO = lstGestionAutorizantesDTO;
                objParamDTO.fechaInicio = txtCEFechaInicio.val();
                objParamDTO.fechaFin = txtCEFechaFin.val();
                mdlCargando.modal("show");
                if (cboFiltroCC.val() != "" && cboFiltroLineaNegocio.val() > 0 && getValoresMultiples('#cboFiltroPuesto').length > 0 && lstPersonalNecesario.length > 0 && lstGestionAutorizantesDTO.length > 0) {
                    var path = `/Reportes/Vista.aspx?idReporte=104&plantillaID=0&plantillaCC=${objParamDTO.cc}&inMemory=1&esTabulador=1`;
                    axios.post('CrearSolicitudPlantilla', objParamDTO).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            //#region SE NOTIFICA A LOS AUTORIZANTES
                            let messageGuardar = message;
                            $("#report").attr("src", path);
                            document.getElementById('report').onload = function () {//
                                axios.post("NotificarPlantilla", { ccPlantilla: objParamDTO.cc, esAuthCompleta: false }).then(response => {
                                    let { success, items, message } = response.data;
                                    if (success) {
                                        Alert2Exito(messageGuardar)
                                        mdlCargando.modal("hide");
                                    }
                                }).catch(error => Alert2Error(error.message));
                            };

                            cboFiltroCC.fillCombo('FillCboFiltroCC_PlantillaPersonal', null, false, null);
                            fncLimpiarFiltros();
                            dtPuestosTabuladores.clear();
                            dtPuestosTabuladores.draw();
                            cboFiltroLineaNegocio.empty();
                            cboFiltroPuesto.empty();
                            $("#spanCboFiltroPuesto").text("--Seleccione--");
                            //#endregion
                        } else {
                            Alert2Warning(message);
                        }
                    }).catch(error => Alert2Error(error.message));
                } else {
                    //#region ALERTA ERRORES
                    for (let i = 0; i < lstPersonalNecesario.length; i++) {
                        if (lstPersonalNecesario[i] == "") {
                            Alert2Warning("Es necesario indicar el personal necesario.")
                            return false
                        }
                    }

                    if (lstGestionAutorizantesDTO.length <= 0) {
                        Alert2Warning("Es necesario indicar al menos un autorizante.")
                        return false
                    }
                    //#endregion
                }
            } else {
                //#region ALERTA ERRORES
                if (cboFiltroCC.val() == "") {
                    Alert2Warning("Es necesario seleccionar un CC.");
                    $("#select2-cboFiltroCC-container").css('border', '2px solid red');
                    return false;
                }

                if (cboFiltroLineaNegocio.val() <= 0) {
                    Alert2Warning("Es necesario seleccionar una linea de negocio.");
                    $("#select2-cboFiltroLineaNegocio-container").css('border', '2px solid red');
                    return false;
                }

                if (getValoresMultiples('#cboFiltroPuesto').length <= 0) {
                    Alert2Warning("Es necesario seleccionar al menos un puesto.");
                    return false;
                }

                if (txtCEFechaInicio.val() == "") {
                    Alert2Warning("Es necesario indicar la fecha inicio.");
                    txtCEFechaInicio.css('border', '2px solid red');
                    return false;
                }

                if (txtCEFechaFin.val() == "") {
                    Alert2Warning("Es necesario indicar la fecha fin.");
                    txtCEFechaFin.css('border', '2px solid red');
                    return false;
                }
                //#endregion
            }
        }

        function fncLimpiarFiltros() {
            cboFiltroCC[0].selectedIndex = 0
            cboFiltroCC.trigger("change")
            cboFiltroLineaNegocio[0].selectedIndex = 0
            cboFiltroLineaNegocio.trigger("change")
            cboFiltroPuesto[0].selectedIndex = 0
            cboFiltroPuesto.trigger("change")

            cboCE_Autorizante_ResponsableCC[0].selectedIndex = 0
            cboCE_Autorizante_ResponsableCC.trigger("change")
            cboCE_Autorizante_CapitalHumano1[0].selectedIndex = 0
            cboCE_Autorizante_CapitalHumano1.trigger("change")
            cboCE_Autorizante_LineaNegocio1[0].selectedIndex = 0
            cboCE_Autorizante_LineaNegocio1.trigger("change")
            cboCE_Autorizante_LineaNegocio2[0].selectedIndex = 0
            cboCE_Autorizante_LineaNegocio2.trigger("change")
            cboCE_Autorizante_AltaDireccion1[0].selectedIndex = 0
            cboCE_Autorizante_AltaDireccion1.trigger("change")
            cboCE_Autorizante_AltaDireccion2[0].selectedIndex = 0
            cboCE_Autorizante_AltaDireccion2.trigger("change")
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
        //#endregion
    }

    $(document).ready(() => {
        CH.PlantillaPersonal = new PlantillaPersonal();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();