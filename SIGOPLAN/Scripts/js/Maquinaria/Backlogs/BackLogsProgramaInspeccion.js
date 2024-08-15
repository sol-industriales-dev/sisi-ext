(function () {

    $.namespace("Maquinaria.Backlogs.BackLogsProgramaInspeccion");

    //#region CONST FILTROS
    const cboFiltroAnio = $('#cboFiltroAnio');
    const cboFiltroTipoEquipo = $('#cboFiltroTipoEquipo');
    const btnFiltroGuardar = $("#btnFiltroGuardar");
    const btnFiltroExportar = $("#btnFiltroExportar");
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroMaquinasEnPrograma = $('#btnFiltroMaquinasEnPrograma');
    //#endregion

    //#region CONST MENU
    const btnMenu_InicioObra = $('#btnMenu_InicioObra');
    const btnMenu_ProgramaInspeccion = $("#btnMenu_ProgramaInspeccion");
    const btnMenu_RegistroBackLogs = $("#btnMenu_RegistroBackLogs");
    const btnMenu_InformeRehabilitacion = $("#btnMenu_InformeRehabilitacion");
    const btnMenu_ReporteIndicadores = $("#btnMenu_ReporteIndicadores");
    //#endregion

    //#region CONST
    const cboFiltroAreaCuenta = $("#cboFiltroAreaCuenta");
    const cboFiltroGrupo = $("#cboFiltroGrupo");
    const cboFiltroPeriodo = $("#cboFiltroPeriodo");
    const cboFiltroNoEconomico = $('#cboFiltroNoEconomico');
    const lblFechaInicio = $("#lblFechaInicio");
    const lblAl = $("#lblAl");
    const lblFechaFinal = $("#lblFechaFinal");
    const tblBL_Inspecciones = $('#tblBL_Inspecciones');

    let prioridad = 1;
    let dtInspecciones;
    //#endregion

    //#region MODAL ACTUALIZAR INSPECCION
    const mdlActualizarInspeccion = $('#mdlActualizarInspeccion');
    const txtActualizarPrioridad = $('#txtActualizarPrioridad');
    const txtActualizarNoEconomico = $('#txtActualizarNoEconomico');
    const txtActualizarDescripcion = $('#txtActualizarDescripcion');
    const txtActualizarHorasRestantes = $('#txtActualizarHorasRestantes');
    const txtActualizarPeriodoInspeccion = $('#txtActualizarPeriodoInspeccion');
    const txtActualizarInspRealizada = $('#txtActualizarInspRealizada');
    const txtActualizarCantBackLogs = $('#txtActualizarCantBackLogs');
    const btnActualizarInspeccion = $('#btnActualizarInspeccion');
    //#endregion

    //#region CONST OCULTOS
    const inputEmpresaActual = $('#inputEmpresaActual');
    _empresaActual = +inputEmpresaActual.val();
    //#endregion

    BackLogs = function () {
        (function init() {
            fncListeners();
            initTablaInspecciones();
            $(".buttons-excel").attr("hidden", "hidden");
            obtenerUrlPArams();
        })();

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            if (variables != undefined) {
                cboFiltroAreaCuenta.val(variables.areaCuenta);
                cboFiltroAreaCuenta.trigger('change');
            }
        }

        function getUrlParams(url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            return params;
        }

        function fncListeners() {
            //#region INIT
            lblAl.hide();
            fncFillCboProyectosObra();
            fncHabilitarDeshabilitarControles();
            convertToMultiselectSelectAll(cboFiltroNoEconomico);
            convertToMultiselectSelectAll(cboFiltroGrupo);
            //#endregion

            //#region FILTROS
            cboFiltroAreaCuenta.change(function (e) {
                if ($(this).val() != "") {
                    fncFillCboProyectosObra();
                    fncFillCboPeriodos();
                    fncGetPeriodoActual();
                }
                fncHabilitarDeshabilitarControles();
            });

            cboFiltroAnio.change(function () {
                fncFillCboPeriodos();
                fncGetPeriodoActual();
                fncHabilitarDeshabilitarControles();
            });

            cboFiltroPeriodo.change(function (e) {
                if ($(this).val() > 0) {
                    let dataPrefijo = $('select[id="cboFiltroPeriodo"] option:selected').attr("data-prefijo");
                    let dataFechas = dataPrefijo.split("|");
                    for (let i = 0; i < dataFechas.length; i++) {
                        if (i == 0) {
                            lblFechaInicio.text(dataFechas[0]);
                        } else {
                            lblFechaFinal.text(dataFechas[1]);
                        }
                        lblFechaInicio.addClass("lblFechaInicioFinal");
                        lblFechaFinal.addClass("lblFechaInicioFinal");
                        lblFechaInicio.show();
                        lblFechaFinal.show();
                        lblAl.show();
                    }
                } else {
                    lblFechaInicio.hide();
                    lblAl.hide();
                    lblFechaFinal.hide();
                }
            });

            btnFiltroBuscar.click(function (e) {
                prioridad = 1;
                fncGetInspecciones(false);
            });

            btnFiltroMaquinasEnPrograma.click(function (e) {
                prioridad = 1;
                fncGetInspecciones(true);
            });

            btnFiltroGuardar.click(function (e) {
                fncGuardarInspecciones();
            });

            btnFiltroExportar.click(function (e) {
                Alert2AccionConfirmar("Generar excel", "¿Desea exportar la tabla a formato excel?", "Confirmar", "Cancelar", () => fncExportarTabla());
            });

            cboFiltroGrupo.change(function (e) {
                let arrGrupos = [];
                cboFiltroGrupo.val().forEach(element => {
                    arrGrupos.push(element);
                });

                cboFiltroNoEconomico.fillCombo("/BackLogs/FillCboNoEconomico", { areaCuenta: cboFiltroAreaCuenta.val(), lstGrupos: arrGrupos }, true, null);
                convertToMultiselectSelectAll(cboFiltroNoEconomico);
                cboFiltroNoEconomico.multiselect('enable');
            });

            cboFiltroTipoEquipo.fillCombo("/BackLogs/FillCboTipoEquipo", {}, false);
            cboFiltroTipoEquipo.select2();
            cboFiltroTipoEquipo.change(function () {
                fncFillCboGrupos();
            });
            //#endregion

            //#region MENU
            btnMenu_InicioObra.click(function (e) {
                document.location.href = '/BackLogs/Index?areaCuenta=' + cboFiltroAreaCuenta.val();
            });
            btnMenu_ProgramaInspeccion.click(function (e) {
                document.location.href = '/BackLogs/ProgramaInspeccionBackLogs?areaCuenta=' + cboFiltroAreaCuenta.val();
            })
            btnMenu_RegistroBackLogs.click(function (e) {
                document.location.href = "/BackLogs/RegistroBackLogsObra?areaCuenta=" + cboFiltroAreaCuenta.val();
            });
            btnMenu_InformeRehabilitacion.click(function (e) {
                document.location.href = "/BackLogs/InformeBackLogsRehabilitacion?areaCuenta=" + cboFiltroAreaCuenta.val();
            });
            btnMenu_ReporteIndicadores.click(function (e) {
                document.location.href = "/BackLogs/ReporteIndicadoresObra?areaCuenta=" + cboFiltroAreaCuenta.val();
            });
            //#endregion

            txtActualizarCantBackLogs.click(function (e) {
                $(this).select();
            });

            btnActualizarInspeccion.click(function (e) {
                fncActualizarInsp();
            });
        }

        function fncFillCboGrupos() {
            // if (cboFiltroTipoEquipo.val() != "") {
            let objParamsDTO = {};
            objParamsDTO.areaCuenta = cboFiltroAreaCuenta.val();
            objParamsDTO.tipoEquipoID = cboFiltroTipoEquipo.val();
            cboFiltroGrupo.attr("multiple", true);
            cboFiltroGrupo.fillCombo("/BackLogs/FillCboGrupo", { objParamsDTO }, true, "TODOS", () => { convertToMultiselectSelectAll(cboFiltroGrupo) });
            convertToMultiselectSelectAll(cboFiltroGrupo);
            cboFiltroGrupo.multiselect('enable');
            cboFiltroGrupo.trigger("change");
            // }
        }

        function fncExportarTabla() {
            $(".buttons-excel").trigger("click");
        }

        function fncActualizarInsp() {
            let obj = fncGetDatosActualizarInsp();
            if (obj != null) {
                axios.post("ActualizarInspeccion", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito("Éxito al actualizar la inspección.");
                        fncGetInspecciones(false);
                        mdlActualizarInspeccion.modal("hide");
                        btnActualizarInspeccion.attr("data-id", 0);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetDatosActualizarInsp() {
            let strMensajeError = "";
            let fechaInspRealizada = "";
            let cantBackLogs = 0;

            txtActualizarInspRealizada.val() != "" ? fechaInspRealizada = txtActualizarInspRealizada.val() : strMensajeError += `Es necesario indicar la "Insp. Realizada".<br>`;
            txtActualizarCantBackLogs.val() != "" ? cantBackLogs = txtActualizarCantBackLogs.val() : strMensajeError += `Es necesario indicar la "Cant. BackLogs".`;
            if (strMensajeError != "") {
                Alert2Warning(strMensajeError)
            } else {
                let obj = new Object();
                obj = {
                    id: btnActualizarInspeccion.attr("data-id"),
                    fechaInspRealizada: fechaInspRealizada,
                    cantBackLogs: cantBackLogs
                };
                return obj;
            }
        }

        function fncHabilitarDeshabilitarControles() {
            if (cboFiltroAreaCuenta.val() != "" && cboFiltroAnio.val() > 0) {
                cboFiltroPeriodo.attr("disabled", false);
                cboFiltroTipoEquipo.attr("disabled", false);
                btnFiltroGuardar.attr("disabled", false);
                btnFiltroExportar.attr("disabled", false);
                btnFiltroBuscar.attr("disabled", false);
                btnFiltroMaquinasEnPrograma.attr("disabled", false);
            } else {
                cboFiltroPeriodo.attr("disabled", true);
                cboFiltroTipoEquipo.attr("disabled", true);
                cboFiltroGrupo.attr("disabled", true);
                cboFiltroNoEconomico.attr("disabled", true);
                btnFiltroGuardar.attr("disabled", true);
                btnFiltroExportar.attr("disabled", true);
                btnFiltroBuscar.attr("disabled", true);
                btnFiltroMaquinasEnPrograma.attr("disabled", true);
            }
        }

        function fncFillCboProyectosObra() {
            let idProyecto = cboFiltroAreaCuenta.val();
            if (idProyecto == null) {
                if (_empresaActual == 6) {
                    cboFiltroAreaCuenta.fillCombo("FillCboAC", {}, false);
                } else {
                    cboFiltroAreaCuenta.fillCombo("/CatObra/cboCentroCostosUsuarios", {}, false);
                }
                cboFiltroAreaCuenta.select2({ width: "resolve" });
            }
            cboFiltroAnio.select2({ width: "resolve" });
        }

        function fncFillCboPeriodos() {
            if (cboFiltroAreaCuenta.val() != "" && cboFiltroAnio.val() > 0) {
                cboFiltroPeriodo.fillCombo("/BackLogs/FillPeriodos", { anio: cboFiltroAnio.val() }, false);
                cboFiltroPeriodo.select2();
            }
        }

        function fncGetPeriodoActual() {
            axios.post("GetPeriodoActual").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let periodoActual = response.data.periodoActual;
                    cboFiltroPeriodo.val(periodoActual);
                    cboFiltroPeriodo.trigger("change");
                    cboFiltroPeriodo.attr("periodo-actual", periodoActual);

                    // cboFiltroGrupo.multiselect('selectAll', false);
                    // cboFiltroGrupo.multiselect('refresh');
                    // cboFiltroGrupo.multiselect('deselect', 'Todos');
                    // cboFiltroGrupo.trigger("change");

                    // $("#cboFiltroNoEconomico").multiselect('selectAll', false);
                    // $("#cboFiltroNoEconomico").multiselect('refresh');
                    // $("#cboFiltroNoEconomico").multiselect('deselect', 'Todos');

                    // btnFiltroMaquinasEnPrograma.trigger("click");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaInspecciones() {
            dtInspecciones = tblBL_Inspecciones.DataTable({
                language: dtDicEsp,
                destroy: true,
                ordering: true,
                paging: false,
                searching: false,
                bFilter: true,
                info: false,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true, visible: false,
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6]
                        }
                    }
                ],
                columns: [
                    { data: "prioridad", title: 'Prioridad' },
                    { data: 'noEconomico', title: 'Económico' },
                    { data: 'descripcion', title: 'Descripción', visible: false },
                    { data: 'horometro', title: 'Horas restantes' },
                    {
                        title: 'Periodo de Inspección',
                        render: function (data, type, row) {
                            let periodoInspeccion = lblFechaInicio.text() + " - " + lblFechaFinal.text();
                            return periodoInspeccion;
                        }
                    },
                    {
                        data: 'fechaInspRealizada', title: "Insp. Realizada", visible: false,
                        render: function (data, type, row) {
                            if (row.id > 0) {
                                return moment(data).format("DD/MM/YYYY");
                            } else {
                                return "";
                            }
                        }
                    },
                    { data: 'cantBackLogs', title: "Cant. BackLogs", visible: false },
                    {
                        data: "fechaInspRealizada", title: 'Insp. realizada',
                        render: function (data, type, row) {
                            if (row.id > 0) {
                                return `<input type="date" disabled="true" value="${moment(data).format("YYYY-MM-DD")}" class="form-control fechaInspRealizada">`;
                            } else {
                                return `<input type="date" class="form-control fechaInspRealizada">`;
                            }
                        }
                    },
                    {
                        data: "cantBackLogs", title: 'Cant. BackLogs',
                        render: function (data, type, row) {
                            if (row.id > 0) {
                                return `<input class="form-control cantBackLogs" onclick="this.select()" disabled="true" value="${data}">`;
                            } else {
                                return `<input class="form-control cantBackLogs" onclick="this.select()" value="0">`;
                            }
                        }
                    },
                    {
                        render: function (data, type, row) {
                            let btns = "";
                            if (row.id > 0) {
                                let btnActualizar = `<button class="btn btn-xs btn-warning editarInspeccion" title="Actualizar inspección."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                                let btnEliminar = `<button class="btn btn-xs btn-danger eliminarInspeccion" title="Eliminar inspección."><i class="fas fa-trash"></i></button>`;
                                btns = btnActualizar + btnEliminar
                            }
                            return btns;
                        }
                    },
                    { data: 'id', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblBL_Inspecciones.on('click', '.editarInspeccion', function () {
                        let rowData = dtInspecciones.row($(this).closest('tr')).data();
                        txtActualizarPrioridad.val(rowData.prioridad);
                        txtActualizarNoEconomico.val(rowData.noEconomico);
                        txtActualizarDescripcion.val(rowData.descripcion);
                        txtActualizarHorasRestantes.val(rowData.horometro);
                        txtActualizarPeriodoInspeccion.val(moment(rowData.fechaInicioInsp).format("DD/MM/YYYY") + " - " + moment(rowData.fechaFinalInsp).format("DD/MM/YYYY"));
                        txtActualizarInspRealizada.val(moment(rowData.fechaInspRealizada).format("YYYY-MM-DD"));
                        txtActualizarCantBackLogs.val(rowData.cantBackLogs);
                        btnActualizarInspeccion.attr("data-id", rowData.id);

                        txtActualizarPrioridad.attr("disabled", true);
                        txtActualizarNoEconomico.attr("disabled", true);
                        txtActualizarDescripcion.attr("disabled", true);
                        txtActualizarHorasRestantes.attr("disabled", true);
                        txtActualizarPeriodoInspeccion.attr("disabled", true);

                        mdlActualizarInspeccion.modal("show");
                    });
                    tblBL_Inspecciones.on('click', '.eliminarInspeccion', function () {
                        let rowData = dtInspecciones.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarInspeccion(rowData.id));
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { "order": [[0, "desc"]] }
                ],
            });
        }

        function fncGetDatosInspeccionesGuardar() {
            let strMensajeError = "";
            let objInsp = new Array();
            var datosDT = $('#tblBL_Inspecciones').DataTable().rows().data();
            $('#tblBL_Inspecciones').find('tbody').find('tr').each(function (index, value) {
                let idInsp = 0;
                idInsp = datosDT[index].id;
                if (idInsp <= 0) {
                    let fechaInspRealizada = $(value).find('.fechaInspRealizada').val();
                    let cantBackLogs = $(value).find('.cantBackLogs').val();
                    if (fechaInspRealizada && cantBackLogs != "") {
                        obj = {
                            areaCuenta: cboFiltroAreaCuenta.val(),
                            periodo: cboFiltroPeriodo.val(),
                            noEconomico: datosDT[index].noEconomico,
                            horometro: datosDT[index].horometro,
                            fechaInicioInsp: lblFechaInicio.text(),
                            fechaFinalInsp: lblFechaFinal.text(),
                            fechaInspRealizada: $(value).find('.fechaInspRealizada').val(),
                            cantBackLogs: $(value).find('.cantBackLogs').val()
                        };
                        objInsp.push(obj);
                    }

                    if (fechaInspRealizada != "" && cantBackLogs <= 0) {
                        $(value).find('.cantBackLogs').closest("td").css("background-color", "#ff0000");
                        strMensajeError += `En caso de asignar la "Inspección realizada", es necesario indicar la cantidad de BackLogs.<br>`;
                    } else {
                        $(value).find('.cantBackLogs').closest("td").css("background-color", "");
                    }

                    if (fechaInspRealizada == "" && cantBackLogs > 0) {
                        $(value).find('.fechaInspRealizada').closest("td").css("background-color", "#ff0000");
                        strMensajeError += `En caso de asignar la cantidad de BackLogs, es necesario indicar la "Inspección realizada".<br>`;
                    } else {
                        $(value).find('.fechaInspRealizada').closest("td").css("background-color", "");
                    }

                    if (strMensajeError != "") {
                        Alert2Warning(strMensajeError);
                    }
                }
            });
            return objInsp;
        }

        function fncEliminarInspeccion(id) {
            let obj = new Object();
            obj = {
                id: id
            };
            axios.post("EliminarInspeccionObra", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Éxito al eliminar el registro.");
                    fncGetInspecciones(false);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetInspecciones(esPuroPrograma) {
            if (cboFiltroAreaCuenta.val() != "" && cboFiltroAnio.val() > 0) {
                let objData = fncGetObjFiltro(esPuroPrograma);
                if (objData != "" && objData != undefined) {
                    axios.post("GetInspecciones", objData).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            if (response.data.lstInspecciones != null) {
                                dtInspecciones.clear();
                                dtInspecciones.rows.add(response.data.lstInspecciones);
                                dtInspecciones.draw();
                            } else {
                                dtInspecciones.clear();
                                dtInspecciones.draw();
                            }
                        }
                    }).catch(error => Alert2Error(error.message));
                }
            }
        }

        function fncGetObjFiltro(esPuroPrograma) {
            let periodo = 0;
            let lstNoEconomicosID = [];
            let strMensajeError = "";

            fncDefaultCtrls("cboFiltroPeriodo");
            fncDefaultCtrls("cboFiltroGrupo");
            fncDefaultCtrls("cboFiltroNoEconomico");

            areaCuenta = cboFiltroAreaCuenta.val();
            cboFiltroPeriodo.val() != "" ? periodo = cboFiltroPeriodo.val() : fncValidacionCtrl("#select2-cboFiltroPeriodo-container", true, "Es necesario seleccionar un periodo.");
            // cboFiltroGrupo.val() != "" ? idGrupo = cboFiltroGrupo.val() : fncValidacionCtrl("#select2-cboFiltroGrupo-container", true, "Es necesario seleccionar un grupo.");

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
            } else {
                let objData = new Object();
                objData = {
                    periodo: periodo,
                    areaCuenta: cboFiltroAreaCuenta.val(),
                    idGrupo: cboFiltroGrupo.val(),
                    lstNoEconomicosID: cboFiltroNoEconomico.val(),
                    esPuroPrograma: esPuroPrograma,
                    anio: cboFiltroAnio.val(),
                    tipoEquipoID: cboFiltroTipoEquipo.val()
                };
                return objData;
            }
        }

        function fncGuardarInspecciones() {
            let objData = fncGetDatosInspeccionesGuardar();
            if (objData != null && objData.length > 0) {
                axios.post("GuardarInspecciones", objData).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetInspecciones(false);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }
    };

    $(document).ready(() => BackLogs = new BackLogs())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();