(() => {
    $.namespace('CH.RelPuestoFases');

    //#region CONST FILTROS
    const cboFiltroLineaNegocio = $("#cboFiltroLineaNegocio");
    const cboFiltroAreaDepartamento = $("#cboFiltroAreaDepartamento");
    const btnFiltroBuscar = $("#btnFiltroBuscar");
    const btnFiltroPDF = $("#btnFiltroPDF");
    const btnFiltroExcel = $("#btnFiltroExcel");
    const btnFiltroEnviar = $("#btnFiltroEnviar");
    const cboFiltroCC = $("#cboFiltroCC");
    const cboFiltroPuesto = $("#cboFiltroPuesto");
    const btnFiltroNotificar = $('#btnFiltroNotificar');
    const cboFiltroAño = $('#cboFiltroAño');
    //#endregion

    //#region CONST AUTORIZANTES
    const cboCE_Autorizante_CapitalHumano1 = $("#cboCE_Autorizante_CapitalHumano1");
    const cboCE_Autorizante_CapitalHumano2 = $("#cboCE_Autorizante_CapitalHumano2");
    const cboCE_Autorizante_GerenteSubdirectorDirector1 = $("#cboCE_Autorizante_GerenteSubdirectorDirector1");
    const cboCE_Autorizante_GerenteSubdirectorDirector2 = $("#cboCE_Autorizante_GerenteSubdirectorDirector2");
    const cboCE_Autorizante_LineaNegocio1 = $("#cboCE_Autorizante_LineaNegocio1");
    const cboCE_Autorizante_LineaNegocio2 = $("#cboCE_Autorizante_LineaNegocio2");
    const cboCE_Autorizante_AltaDireccion1 = $("#cboCE_Autorizante_AltaDireccion1");
    const cboCE_Autorizante_AltaDireccion2 = $("#cboCE_Autorizante_AltaDireccion2");
    //#endregion

    //#region CONST REPORTE
    const tblTabuladores = $('#tblTabuladores');
    let dtTabuladores;
    //#endregion

    RelPuestoFases = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            fncGetAccesosMenu();
            $("#menuReportes").addClass("opcionSeleccionada");
            initTblTabuladores();
            tblTabuladores.DataTable().buttons('.buttonsToHide').nodes().css("display", "none");
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

            cboFiltroCC.on("change", function () {
                var lstFK_LineaNegocio = getValoresMultiples('#cboFiltroLineaNegocio');
                var lstFK_Deptos = getValoresMultiples('#cboFiltroAreaDepartamento');

                if (lstFK_LineaNegocio != null && lstFK_LineaNegocio.length > 0) {
                    fncFillComboPuestos();
                }
            });

            btnFiltroBuscar.click(function () {
                fncGetTabuladoresReporte();
            });

            btnFiltroExcel.click(function () {
                Alert2AccionConfirmar('Descargar excel', '¿Desea descargar un archivo excel?', 'Confirmar', 'Cancelar', () => fncGetParamsDTO_EXCEL());
            });

            btnFiltroPDF.click(function () {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea imprimir la plantilla seleccionada?', 'Confirmar', 'Cancelar', () => fncGetParamsDTO_PDF());
            });

            btnFiltroNotificar.on("click", function () {
                var lstLN = getValoresMultiples('#cboFiltroLineaNegocio');

                if (lstLN.length > 0) {
                    Alert2AccionConfirmar('¡Cuidado!', '¿Desea guardar y notificar la plantilla seleccionada?', 'Confirmar', 'Cancelar', () => fncSendReporteCorreo());

                } else {
                    Alert2Warning("Favor de seleccionar almenos una Linea de Negocios");
                }

            });
            //#endregion

            //#region FILL COMBO
            cboFiltroLineaNegocio.fillCombo('FillCboLineaNegocios', {}, false, 'Todos');
            cboFiltroAreaDepartamento.fillCombo('FillCboAreasDepartamentos', {}, false, 'Todos');
            convertToMultiselect('#cboFiltroLineaNegocio');
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

            cboCE_Autorizante_CapitalHumano1.val(1019);
            cboCE_Autorizante_CapitalHumano1.trigger("change");

            $(".select2").select2();
            //#endregion
        }

        //#region FUNCIONES
        function initTblTabuladores() {
            dtTabuladores = tblTabuladores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'idPuesto', title: 'Id' },
                    { data: 'puestoDesc', title: 'Puesto' },
                    { data: 'lineaNegocioDesc', title: 'LN' },
                    { data: 'categoriaDesc', title: 'Cat' },
                    { data: 'descAreaDepartamento', title: 'Area/Departamento' },
                    { data: 'descSindicato', title: 'Sindicato' },
                    { data: 'tipoNominaDesc', title: 'Tipo nómina' },
                    { data: 'sueldoBaseStringActual', title: 'Sueldo base' },
                    { data: 'complementoStringActual', title: 'Complemento' },
                    { data: 'totalNominalStringActual', title: 'Total nominal' },
                    { data: 'sueldoMensualStringActual', title: 'Total mensual' },

                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ]
            });
        }

        function fncGetTabuladoresReporte() {
            let objParamDTO = {}
            objParamDTO.lstFK_LineaNegocio = getValoresMultiples('#cboFiltroLineaNegocio')
            objParamDTO.lstFK_AreaDepartamento = getValoresMultiples('#cboFiltroAreaDepartamento')
            objParamDTO.lstCC = getValoresMultiples('#cboFiltroCC')
            objParamDTO.lstFK_Puestos = getValoresMultiples('#cboFiltroPuesto')
            axios.post('GetTabuladoresReporte', objParamDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtTabuladores.clear();
                    dtTabuladores.rows.add(response.data.lstTabPuestos);
                    dtTabuladores.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncFillComboPuestos() {
            cboFiltroPuesto.fillCombo('FillCboFiltroPuestos_Reportes', { lstCC: getValoresMultiples('#cboFiltroCC'), lstFK_LineaNegocio: getValoresMultiples('#cboFiltroLineaNegocio') }, false, 'Todos')
            convertToMultiselect('#cboFiltroPuesto')
        }

        function fncGetParamsDTO_EXCEL() {
            let objParamDTO = {}
            objParamDTO.lstFK_LineaNegocio = getValoresMultiples('#cboFiltroLineaNegocio')
            objParamDTO.lstFK_AreaDepartamento = getValoresMultiples('#cboFiltroAreaDepartamento')
            objParamDTO.lstCC = getValoresMultiples('#cboFiltroCC')
            objParamDTO.lstFK_Puestos = getValoresMultiples('#cboFiltroPuesto')
            objParamDTO.añoReporte = cboFiltroAño.val();
            axios.post('GetParamsDTO', objParamDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    location.href = 'GenerarExcelTabuladores';
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetParamsDTO_PDF() {
            let objParamDTO = {}
            objParamDTO.lstFK_LineaNegocio = getValoresMultiples('#cboFiltroLineaNegocio')
            objParamDTO.lstFK_AreaDepartamento = getValoresMultiples('#cboFiltroAreaDepartamento')
            objParamDTO.lstCC = getValoresMultiples('#cboFiltroCC')
            objParamDTO.lstFK_Puestos = getValoresMultiples('#cboFiltroPuesto')
            objParamDTO.añoReporte = cboFiltroAño.val();
            objParamDTO.lstGestionAutorizantesDTO = fncGetNotificantes();
            axios.post('GetParamsDTOPdf', objParamDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    $.blockUI({ message: "Cargando información..." });
                    var path = `/Reportes/Vista.aspx?idReporte=289`;
                    $("#report").attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncSendReporteCorreo() {
            let objParamDTO = {}
            objParamDTO.lstGestionAutorizantesDTO = fncGetNotificantes();
            objParamDTO.lstFK_LineaNegocio = getValoresMultiples('#cboFiltroLineaNegocio')
            objParamDTO.lstFK_AreaDepartamento = getValoresMultiples('#cboFiltroAreaDepartamento')
            objParamDTO.lstCC = getValoresMultiples('#cboFiltroCC')
            objParamDTO.lstFK_Puestos = getValoresMultiples('#cboFiltroPuesto')
            objParamDTO.añoReporte = cboFiltroAño.val();
            axios.post('SendReporteCorreo', objParamDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Plantilla notificada con exito.");

                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
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

        function fncGetNotificantes() {
            let notificantes = [];

            if (cboCE_Autorizante_CapitalHumano1.val() != "" && cboCE_Autorizante_CapitalHumano1.val() != "--Seleccione--") {
                notificantes.push({
                    FK_UsuarioAutorizacion: cboCE_Autorizante_CapitalHumano1.val(),
                    nivelAutorizante: 0
                });
            }
            if (cboCE_Autorizante_CapitalHumano2.val() != "" && cboCE_Autorizante_CapitalHumano2.val() != "--Seleccione--") {
                notificantes.push({
                    FK_UsuarioAutorizacion: cboCE_Autorizante_CapitalHumano2.val(),
                    nivelAutorizante: 0
                });
            }
            if (cboCE_Autorizante_GerenteSubdirectorDirector1.val() != "" && cboCE_Autorizante_GerenteSubdirectorDirector1.val() != "--Seleccione--") {
                notificantes.push({
                    FK_UsuarioAutorizacion: cboCE_Autorizante_GerenteSubdirectorDirector1.val(),
                    nivelAutorizante: 1
                });
            }
            if (cboCE_Autorizante_GerenteSubdirectorDirector2.val() != "" && cboCE_Autorizante_GerenteSubdirectorDirector2.val() != "--Seleccione--") {
                notificantes.push({
                    FK_UsuarioAutorizacion: cboCE_Autorizante_GerenteSubdirectorDirector2.val(),
                    nivelAutorizante: 1
                });
            }
            if (cboCE_Autorizante_LineaNegocio1.val() != "" && cboCE_Autorizante_LineaNegocio1.val() != "--Seleccione--") {
                notificantes.push({
                    FK_UsuarioAutorizacion: cboCE_Autorizante_LineaNegocio1.val(),
                    nivelAutorizante: 2
                });
            }
            if (cboCE_Autorizante_LineaNegocio2.val() != "" && cboCE_Autorizante_LineaNegocio2.val() != "--Seleccione--") {
                notificantes.push({
                    FK_UsuarioAutorizacion: cboCE_Autorizante_LineaNegocio2.val(),
                    nivelAutorizante: 2
                });
            }
            if (cboCE_Autorizante_AltaDireccion1.val() != "" && cboCE_Autorizante_AltaDireccion1.val() != "--Seleccione--") {
                notificantes.push({
                    FK_UsuarioAutorizacion: cboCE_Autorizante_AltaDireccion1.val(),
                    nivelAutorizante: 3
                });
            }
            if (cboCE_Autorizante_AltaDireccion2.val() != "" && cboCE_Autorizante_AltaDireccion2.val() != "--Seleccione--") {
                notificantes.push({
                    FK_UsuarioAutorizacion: cboCE_Autorizante_AltaDireccion2.val(),
                    nivelAutorizante: 3
                });
            }

            return notificantes;
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.RelPuestoFases = new RelPuestoFases();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();