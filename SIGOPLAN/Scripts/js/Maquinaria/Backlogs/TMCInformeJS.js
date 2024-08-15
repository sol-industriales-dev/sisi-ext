(() => {
    $.namespace("Maquinaria.BackLogs.BackLogsInformeRehabilitacion");

    BackLogs = function () {
        //#region CONST FILTROS
        const cboProyecto = $("#cboProyecto");
        const cboCC = $("#cboCC");
        const cboModelo = $("#cboModelo");
        const cboGrupo = $("#cboGrupo");
        const cboConjunto = $("#cboConjunto");
        const cboSubconjunto = $("#cboSubconjunto");
        const cboMes = $("#cboMes");
        const chk20 = $("#chk20");
        const chk40 = $("#chk40");
        const chk50 = $("#chk50");
        const chk60 = $("#chk60");
        const chk80 = $("#chk80");
        const chk90 = $("#chk90");
        const chk100 = $("#chk100");
        const btnGuardarModalArchivos = $('#btnGuardarModalArchivos');
        const btnLimpiarFiltro = $("#btnLimpiarFiltro");
        const btnBuscar = $('#btnBuscar');
        const tblBL_CatBackLogsTMC = $("#tblBL_CatBackLogsTMC");
        const tblBL_CatBackLogsTMC_tbody = $("#tblBL_CatBackLogsTMC tbody");
        const tblBL_BackLogs_Archivos = $('#tblBL_BackLogs_Archivos');
        const modalVerOC = $('#modalVerOC');
        const tblBL_OrdenesCompra = $('#tblBL_OrdenesCompra');
        const tblBL_Requisiciones = $('#tblBL_Requisiciones');
        const modalVerRequisicion = $('#modalVerRequisicion');
        const btnExportar = $('#btnExportar');
        const mdlDescripcionBL = $('#mdlDescripcionBL');
        const txtDescripcionBL = $('#txtDescripcionBL');
        //#endregion

        //#region BOTONES MENU
        const btnInformeRehabilitacion = $("#btnInformeRehabilitacion");
        const btnProgramaInspeccion = $("#btnProgramaInspeccion");
        const btnPresupuestoRehabilitacion = $('#btnPresupuestoRehabilitacion');
        const btnSeguimientoPresupuestos = $('#btnSeguimientoPresupuestos');
        const btnFrenteBackLogs = $('#btnFrenteBackLogs');
        const btnIndicadoresRehabilitacionTMC = $('#btnIndicadoresRehabilitacionTMC');
        const btnInicioTMC = $('#btnInicioTMC');
        const btnLiberar = $('#btnLiberar');
        //#endregion

        //#region REPORTE GENERAL
        const divReporteGeneral = $('#divReporteGeneral');
        const rptGeneralFechaInicio = $('#rptGeneralFechaInicio');
        const rptGeneralFechaFin = $('#rptGeneralFechaFin');
        const rptGeneral = $('#rptGeneral');
        //#endregion

        //#region EXCEL CARGO
        const selectCentroCosto = $('#selectCentroCosto');
        const inputFechaInicioCargo = $('#inputFechaInicioCargo');
        const inputFechaFinCargo = $('#inputFechaFinCargo');
        const botonGenerarExcelCargo = $('#botonGenerarExcelCargo');
        //#endregion

        //#region EVIDENCIAS
        const mdlEvidencias = $('#mdlEvidencias');
        const inputExamen = $('#inputExamen');
        const lblTexto1 = $('#lblTexto1');
        const rowDataId = $('#rowDataId');
        const tblEvidencias = $('#tblEvidencias');
        const btnGuardarEvidencia = $('#btnGuardarEvidencia');
        const cboMdlEvidenciasTipoEvidencia = $('#cboMdlEvidenciasTipoEvidencia');
        //#endregion

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);
        //#endregion

        let dtBackLogs;
        let dtArchivos;
        let arrFiltros = [];

        (function init() {
            fncListeners();
            initTablaBackLogs();
            fncFillCombos();
            fncDeshabilitarHabilitarFiltros();
            initTablaOC();
            initTablaArchivos();
            initTablaRequisiciones();
            obtenerUrlPArams();
        })();

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            if (variables != undefined) {
                cboProyecto.val(variables.areaCuenta);
                cboProyecto.trigger('change');
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
            btnInicioTMC.click(function (e) {
                document.location.href = '/BackLogs/IndexTMC?areaCuenta=' + cboProyecto.val();
            });
            btnProgramaInspeccion.click(function (e) {
                document.location.href = '/BackLogs/ProgramaInspTMC?areaCuenta=' + cboProyecto.val();
            })
            btnPresupuestoRehabilitacion.click(function (e) {
                document.location.href = '/BackLogs/PresupuestoRehabilitacionTMC?areaCuenta=' + cboProyecto.val();
            });
            btnSeguimientoPresupuestos.click(function (e) {
                document.location.href = '/BackLogs/SeguimientoDePresupuestoTMC?areaCuenta=' + cboProyecto.val();
            });

            btnFrenteBackLogs.click(function (e) {
                document.location.href = '/BackLogs/FrenteTMC?areaCuenta=' + cboProyecto.val();
            });

            btnInformeRehabilitacion.click(function (e) {
                document.location.href = '/BackLogs/InformeTMC?areaCuenta=' + cboProyecto.val();
            });

            btnIndicadoresRehabilitacionTMC.click(function (e) {
                document.location.href = '/BackLogs/IndicadoresRehabilitacionTMC?areaCuenta=' + cboProyecto.val();
            });

            cboProyecto.change(function (e) {
                if (cboProyecto.val() != "") {
                    fncLimpiarControles();
                } else {
                    fncLimpiarControles();
                    GetBackLogsFiltros();
                }
                fncDeshabilitarHabilitarFiltros();
            });

            cboCC.change(function (e) {
                if ($(this).val() != "") {
                    btnExportar.attr("disabled", false);
                    btnLiberar.attr("disabled", false);

                    fncGetModeloGrupoCCSeleccionado();
                } else {
                    btnExportar.attr("disabled", true);
                    btnLiberar.attr("disabled", true);
                }
            });

            cboConjunto.change(function (e) {
                if (cboConjunto.val() != "") {
                    fncFillCboSubconjuntos();
                } else {
                    cboSubconjunto.empty().append("");
                }
                GetBackLogsFiltros();
            });

            btnLimpiarFiltro.click(function (e) {
                fncLimpiarControles();
                GetBackLogsFiltros();
            });

            tblBL_CatBackLogsTMC.on('draw.dt', function () {
                fncColorearCeldaEstatus();
            });

            btnBuscar.click(function (e) {
                GetBackLogsFiltros();
            });

            btnGuardarModalArchivos.click(function () {
                subiendoArchivo();
            });

            rptGeneral.on('click', function () {
                //int tipoBL, DateTime fechaInicio, DateTime fechaFin, string ac
                location.href = '/BackLogs/GetReporteGeneral?tipoBL=2' + '&fechaInicio=' + moment(rptGeneralFechaInicio.val(), 'DD/MM/YYYY').toISOString(true) + '&fechaFin=' + moment(rptGeneralFechaFin.val(), 'DD/MM/YYYY').toISOString(true) + '&ac=' + cboProyecto.val();
            });

            btnExportar.click(function (e) {
                fncExportarExcel();
            });

            btnLiberar.click(function (e) {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea liberar el/los backlogs?', 'Confirmar', 'Cancelar', () => fncLiberacion());
            });

            btnGuardarEvidencia.click(function (e) {
                subiendoArchivo();
            });

            botonGenerarExcelCargo.click(function () {
                let listaEconomicos = [];

                selectCentroCosto.find('option:selected').each(function (idx, element) {
                    listaEconomicos.push($(element).text());
                });

                if (listaEconomicos.length == 0) {
                    Alert2Warning('Debe seleccionar un económico.');
                    return;
                }

                axios.post('GetGraficaCargoNomina', { listaEconomicos, fechaInicio: inputFechaInicioCargo.val(), fechaFin: inputFechaFinCargo.val() }).then(response => {
                    let { success, data, message } = response.data;

                    if (success) {
                        initGraficaCargoNomina(data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
            });
        }

        function fncGetModeloGrupoCCSeleccionado() {
            if (cboCC.val() != "") {
                let arrCC = cboCC.val();
                let cantCC = arrCC.length;
                if (cantCC == 1) {
                    let obj = new Object();
                    let objCC = cboCC.find(`option[value="${cboCC.val()}"]`);
                    let prefijoCC = objCC.attr("data-prefijo");
                    obj.noEconomico = prefijoCC;
                    axios.post('GetModeloGrupoCCSeleccionado', obj).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            cboModelo.val(response.data.modelo);
                            cboGrupo.val(response.data.grupo);
                        } else {
                            Alert2Error(message);
                        }
                    }).catch(error => Alert2Error(error.message));
                }
            } else {
                Alert2Warning("Ocurrió un error al obtener el modelo y grupo del CC.");
            }
        }

        function fncFillCombos() {
            rptGeneralFechaInicio.datepicker({}).datepicker();
            rptGeneralFechaFin.datepicker({}).datepicker();
            inputFechaInicioCargo.datepicker({ dateFormat, showAnim }).datepicker("setDate", fechaInicioAnio);
            inputFechaFinCargo.datepicker({ dateFormat, showAnim }).datepicker("setDate", fechaActual);

            fncFillCboProyectosObra();
            fncFillCboCC();
            // fncFillCboModelo();
            fncFillCboConjuntos();
            // fncFillCboGrupo();
            fncFillCboMes();
            cboSubconjunto.select2();
        }

        function fncFillCboProyectosObra() {
            let idProyecto = cboProyecto.val();
            if (idProyecto == null) {
                cboProyecto.fillCombo("/BackLogs/FillAreasCuentasTMC", {}, false);
                cboProyecto.select2({
                    width: "resolve"
                });
            }
        }

        function fncFillCboCC() {
            cboCC.fillCombo("/BackLogs/FillCboCC", { areaCuenta: cboProyecto.val(), esObra: false }, false, "Todos");
            convertToMultiselect('#cboCC');
            cboCC.multiselect('deselectAll', false);
            $("#cboCC").multiselect('refresh');

            selectCentroCosto.fillCombo("/BackLogs/FillComboCentroCostoBackLogs", null, false, "Todos");
            convertToMultiselect('#selectCentroCosto');
        }

        function fncFillCboModelo() {
            cboModelo.fillCombo("/BackLogs/FillCboModelo", {}, false);
            cboModelo.select2({
                width: "resolve"
            });
        }

        function fncFillCboGrupo() {
            cboGrupo.fillCombo("/BackLogs/FillCboGrupo", {}, false);
            cboGrupo.select2({
                width: "resolve"
            });
        }

        function fncFillCboConjuntos() {
            cboConjunto.fillCombo("/BackLogs/FillCboConjunto", {}, false);
            cboConjunto.select2({
                width: "resolve"
            });
        }

        function fncFillCboSubconjuntos() {
            cboSubconjunto.fillCombo("/BackLogs/FillCboSubconjunto", { idConjunto: cboConjunto.val() }, false);
            cboSubconjunto.select2({
                width: "resolve"
            });
        }

        function fncFillCboMes() {
            let Seleccione = $("<option />", { text: "--Seleccione--", value: 0, });
            let Enero = $("<option />", { text: "Enero", value: 1, });
            let Febrero = $("<option />", { text: "Febrero", value: 2, });
            let Marzo = $("<option />", { text: "Marzo", value: 3, });
            let Abril = $("<option />", { text: "Abril", value: 4, });
            let Mayo = $("<option />", { text: "Mayo", value: 5, });
            let Junio = $("<option />", { text: "Junio", value: 6, });
            let Julio = $("<option />", { text: "Julio", value: 7, });
            let Agosto = $("<option />", { text: "Agosto", value: 8, });
            let Septiembre = $("<option />", { text: "Septiembre", value: 9, });
            let Octubre = $("<option />", { text: "Octubre", value: 10, });
            let Noviembre = $("<option />", { text: "Noviembre", value: 11, });
            let Diciembre = $("<option />", { text: "Diciembre", value: 12, });

            cboMes.prepend(Diciembre);
            cboMes.prepend(Noviembre);
            cboMes.prepend(Octubre);
            cboMes.prepend(Septiembre);
            cboMes.prepend(Agosto);
            cboMes.prepend(Julio);
            cboMes.prepend(Junio);
            cboMes.prepend(Mayo);
            cboMes.prepend(Abril);
            cboMes.prepend(Marzo);
            cboMes.prepend(Febrero);
            cboMes.prepend(Enero);
            cboMes.prepend(Seleccione);
            cboMes.select2();
            cboMes.val(0);
            cboMes.trigger("change");
        }

        function fncLimpiarControles() {
            fncFillCombos();
            tblBL_CatBackLogsTMC_tbody.empty();
            dtBackLogs.clear();
            cboSubconjunto.empty().append("");
            chk20.prop("checked", false);
            chk40.prop("checked", false);
            chk50.prop("checked", false);
            chk60.prop("checked", false);
            chk80.prop("checked", false);
            chk90.prop("checked", false);
            chk100.prop("checked", false);
        }

        function fncDeshabilitarHabilitarFiltros() {
            let idProyecto = cboProyecto.val();

            if (idProyecto != "") {
                cboCC.attr("disabled", false);
                cboConjunto.attr("disabled", false);
                cboSubconjunto.attr("disabled", false);
                cboMes.attr("disabled", false);
                chk20.prop("disabled", false);
                chk40.prop("disabled", false);
                chk50.prop("disabled", false);
                chk60.prop("disabled", false);
                chk80.prop("disabled", false);
                chk90.prop("disabled", false);
                chk100.prop("disabled", false);
                btnLimpiarFiltro.attr("disabled", false);
                btnBuscar.attr("disabled", false);
                divReporteGeneral.show();
            } else {
                cboCC.attr("disabled", true);
                cboConjunto.attr("disabled", true);
                cboSubconjunto.attr("disabled", true);
                cboMes.attr("disabled", true);
                chk20.attr("disabled", true);
                chk40.attr("disabled", true);
                chk50.attr("disabled", true);
                chk60.attr("disabled", true);
                chk80.attr("disabled", true);
                chk90.attr("disabled", true);
                chk100.attr("disabled", true);
                btnLimpiarFiltro.attr("disabled", true);
                btnBuscar.attr("disabled", true);
                btnExportar.attr("disabled", true);
                btnLiberar.attr("disabled", true);
                divReporteGeneral.hide();
            }
            // cboModelo.attr("disabled", true);
            // cboGrupo.attr("disabled", true);
        }

        function fncColorearCeldaEstatus() {
            tblBL_CatBackLogsTMC.find("tbody tr").each(function (index) {
                $(this).addClass("rowHover");
                $(this).children("td").each(function (index2) {
                    switch (index2) {
                        case 9:
                            let Estatus = $(this).html();
                            switch (Estatus) {
                                case "20%":
                                    $(this).addClass("boton-estatus-20 text-color");
                                    break;
                                case "40%":
                                    $(this).addClass("boton-estatus-40 text-color");
                                    break;
                                case "50%":
                                    $(this).addClass("boton-estatus-50 text-color");
                                    break;
                                case "60%":
                                    $(this).addClass("boton-estatus-60 text-color");
                                    break;
                                case "80%":
                                    $(this).addClass("boton-estatus-80 text-color");
                                    break;
                                case "90%":
                                    $(this).addClass("boton-estatus-90 text-color");
                                    break;
                                default:
                                    break;
                            }
                    }
                })
            });
        }

        function fncGetDatosMaquina() {
            let optionCC = cboCC.find(`option[value="${cboCC.val()}"]`);
            let prefijoCC = optionCC.attr("data-prefijo");
            let objNoEconomico = new Object();
            objNoEconomico = {
                areaCuenta: cboProyecto.val(),
                noEconomico: prefijoCC
            };
            // axios.post('GetMaquina', objNoEconomico).then(response => {
            //     let { success, items, message } = response.data;
            //     if (success) {
            //         if (response.data.lstMaquina != null) {
            //             // cboModelo.val(response.data.lstMaquina[0].modeloEquipoID);
            //             // cboModelo.trigger("change");
            //             // cboGrupo.val(response.data.lstMaquina[0].grupoMaquinariaID);
            //             // cboGrupo.trigger("change");
            //         } else {
            //             cboModelo.val("");
            //             cboGrupo.val("");
            //         }
            //     }else{
            //         Alert2Error("Ocurrió un error al obtener modelo y grupo del equipo.")
            //     }
            // }).catch(error => Alert2Error(error.message));
        }

        function initTablaBackLogs() {
            dtBackLogs = tblBL_CatBackLogsTMC.DataTable({
                paging: true,
                searching: true,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                columns: [
                    { data: "id", title: "id", visible: false },
                    { data: "noEconomico", title: "C.C." },
                    { data: "folioBL", title: "Folio" },
                    {
                        data: "descripcion", title: "Descripción",
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary btn-xs verDescripcionBL" title="Descripción del BL.">Descripción BL</button>`;
                        }
                    },
                    {
                        title: "Fecha<br>programación",
                        render: function (data, type, row) {
                            return moment(row.fechaInspeccion).format('DD/MM/YYYY');
                        }
                    },
                    { data: "conjunto", title: "Conjunto" },
                    { data: "subconjunto", title: "Subconjunto", visible: false },
                    {
                        title: 'Requi.',
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary verRequisicion"><i class="fas fa-list-ul"></i></button>`;
                        }
                    },
                    {
                        title: 'O.C',
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary verOC"><i class="fas fa-shopping-cart"></i></button>`;
                        }
                    },
                    {
                        data: 'idEstatus', title: `Estatus OC`, createdCell: function (td, data) {
                            let estatus = "";
                            switch (data) {
                                case 1: estatus = "20%";
                                    break;
                                case 2: estatus = "40%";
                                    break;
                                case 3: estatus = "50%";
                                    break;
                                case 4: estatus = "60%";
                                    break;
                                case 5: estatus = "80%";
                                    break;
                                case 6: estatus = "90%";
                                    break;
                                case 7: estatus = "100%";
                                    break;
                                default:
                                    break;
                            }
                            $(td).html(`<span>${estatus}</span>`);
                        }

                    },
                    {
                        title: 'Total MX',
                        render: function (data, type, row) {
                            return "$" + row.totalMX.toFixed(2);
                        }
                    },
                    {
                        data: 'idEstatus', title: `Estatus`, createdCell: function (td, data, row) {
                            let estatus = "";
                            switch (data) {
                                case 1: estatus = `20%`;
                                    $(td).addClass("boton-estatus-20 text-color");
                                    break;
                                case 2: estatus = `40%`;
                                    $(td).addClass("boton-estatus-40 text-color");
                                    break;
                                case 3: estatus = `50%`;
                                    $(td).addClass("boton-estatus-50 text-color");
                                    break;
                                case 4: estatus = `60%`;
                                    $(td).addClass("boton-estatus-60 text-color");
                                    break;
                                case 5: estatus = `80%`;
                                    $(td).addClass("boton-estatus-80 text-color");
                                    break;
                                case 6: estatus = `90%`;
                                    $(td).addClass("boton-estatus-90 text-color");
                                    break;
                                case 7: estatus = `100%`;
                                    $(td).addClass("boton-estatus-100 text-color");
                                    break;
                                default:
                                    break;

                            }
                            $(td).html(`<span>${estatus}</span>`);
                        },
                    },
                    {
                        title: "Fecha<br>promesa",
                        render: function (data, type, row) {
                            return moment(row.fechaInspeccion).format('DD/MM/YYYY');
                        }
                    },
                    {
                        title: "Fecha<br>cierre",
                        render: function (data, type, row) {
                            let fechaCierre = "";
                            switch (row.estatus) {
                                case "BackLogs instalado (100%)":
                                    fechaCierre = moment(row.fechaModificacionBL).format("DD/MM/YYYY");
                                    break;
                                default:
                                    fechaCierre = "N/A";
                                    break;
                            }
                            return fechaCierre
                        }
                    },
                    { data: "diasTotales", title: "Días<br>totales" },
                    {
                        title: "Terminación BL", render: (data, type, row) => {
                            let botones = ``;

                            botones += `<button type="button" class="btn btn-primary cargarEvidencia"><i></i>Evidencia</button>`;
                            return botones;
                        }

                    },
                    {
                        data: "esLiberado", title: "Liberado", render: (data) => {
                            if (data) {
                                return `<input type="checkbox" checked="checked" onclick="return false" />`;
                            }
                            else {
                                return `<input type="checkbox" onclick="return false" />`;
                            }
                        }
                    },
                    {
                        data: 'tienePartes', title: 'Tiene partes', render: (data) => {
                            if (data) {
                                return `<input type="checkbox" checked="checked" title="Este BL no cuenta con costo promedio." onclick="return false" />`;
                            }
                            else {
                                return `<input type="checkbox" title="Este BL no cuenta con partes registradas." onclick="return false" />`;
                            }
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblBL_CatBackLogsTMC.on('click', '.cargarEvidencia', function () {
                        let rowData = dtBackLogs.row($(this).closest("tr")).data();
                        $('#mdlEvidencias').modal('show');
                        $("#rowDataId").val(rowData.id);
                        btnGuardarModalArchivos.attr('data-id', $('.subirArchivos').attr("data-id"));
                        $("#titleCurso").empty();
                        $('#inputExamen').val('');
                        $('#lblTexto1').text('Ningún archivo seleccionado');
                        $('#inputExamen').change(function () {
                            $('#lblTexto1').text($(this)[0].files[0].name);
                        });
                        fncGetArchivos(rowData.id);
                    });

                    tblBL_CatBackLogsTMC.on("click", ".verOC", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        fncVerificarOrdenesCompra(rowData.id);
                        modalVerOC.modal("show");
                    });

                    tblBL_CatBackLogsTMC.on("click", ".verRequisicion", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        fncGetRequisiciones(rowData.id);
                        modalVerRequisicion.modal("show");
                    });

                    tblBL_CatBackLogsTMC.on("click", ".cargarEvidenciaSeguridad", function () {
                        let rowData = dtBackLogs.row($(this).closest("tr")).data();
                        $('#mdlEvidencias').modal('show');
                        $("#rowDataId").val(rowData.id);
                        btnGuardarEvidencia.attr('data-id', $('.subirArchivos').attr("data-id"));
                        $("#titleCurso").empty();
                        $('#inputExamen').val('');
                        $('#lblTexto1').text('Ningún archivo seleccionado');
                        $('#inputExamen').change(function () {
                            $('#lblTexto1').text($(this)[0].files[0].name);
                        });
                        fncGetArchivos(rowData.id);
                    });

                    tblBL_CatBackLogsTMC.on("click", ".verDescripcionBL", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        txtDescripcionBL.val(rowData.descripcion);
                        mdlDescripcionBL.modal("show");
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                "createdRow": function (row, data, index) {
                    if (data[6] == "100%") {
                        $("td", row).eq(6).addClass("boton-estatus-20");
                    }
                }
            });
        }

        const subiendoArchivo = function () {
            if (cboMdlEvidenciasTipoEvidencia.val() >= 0) {
                var data = getFormData();
                let objInspeccion = new Object();
                objInspeccion = {
                    tipoEvidencia: cboMdlEvidenciasTipoEvidencia.val(),
                };
                axios.post('/BackLogs/postSubirArchivos', data, { params: tipoEvidencia = objInspeccion }, { headers: { 'Content-Type': 'multipart/form-data' } })
                    .then(response => {
                        let { success, datos, message } = response.data;
                        if (success) {
                            Alert2Exito('Se ha registrado con éxito.');
                            $('#lblTexto1').text('Ningún archivo seleccionado');
                            $('#mdlEvidencias').modal('hide');
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
            else {
                Alert2Warning("Es necesario indicar el tipo de evidencia.");
            }
        }

        function getFormData() {
            let data = new FormData();
            data.append("id", $("#rowDataId").val());
            $.each(document.getElementById("inputExamen").files, function (i, file) {
                data.append("archivo", file);
            });
            return data;
        }

        function initTablaOC() {
            dtOC = tblBL_OrdenesCompra.DataTable({
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                columns: [
                    { data: "numRequisicion", title: "Núm. Requisición" },
                    { data: "numOC", title: "O.C." }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function fncVerificarOrdenesCompra(idBL) {
            let objNumOC = new Object();
            objNumOC = {
                idBackLog: idBL
            };
            axios.post("GetOrdenesCompra", objNumOC).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    // let objInputs = fncCrearInputsOC(response.data.lstOC.length, response.data.lstOC);
                    // divOrdenesCompra.html(objInputs);
                    dtOC.clear();
                    dtOC.rows.add(response.data.lstOC);
                    dtOC.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        // function fncCrearInputsOC(cantOC, lstOC) {
        //     let divInputs = "";
        //     for (let i = 0; i < cantOC; i++) {
        //         divInputs += `<div class='col-sm-4'><label>Número OC: </label>&nbsp;<input value='${lstOC[i].numOC}' id='txtOC${i}' class='form-control' /></div>`;
        //     }
        //     return divInputs;
        // }

        function GetBackLogsFiltros() {
            let objFiltros = fncFiltros();
            axios.post('GetBackLogsFiltrosTMC', objFiltros).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    tblBL_CatBackLogsTMC_tbody.empty();
                    dtBackLogs.clear();
                    dtBackLogs.rows.add(response.data.lstBackLogs);
                    dtBackLogs.draw();
                    fncColorearCeldaEstatus();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncFiltros() {
            let objJS = {};
            let arrEstatus = [];
            if (cboProyecto.val() != "") { objJS.areaCuenta = cboProyecto.val(); }

            let arrNoEconomicos = Array();
            cboCC.val().forEach(element => {
                let objCC = cboCC.find(`option[value="${element}"]`);
                let noEconomico = objCC.attr("data-prefijo");
                arrNoEconomicos.push(noEconomico);
            });

            if (arrNoEconomicos.length > 0) { objJS.arrNoEconomicos = arrNoEconomicos; }
            if (cboConjunto.val() != "") { objJS.idConjunto = cboConjunto.val(); }
            if (cboSubconjunto.val() != null && cboSubconjunto.val() != "") { objJS.idSubconjunto = cboSubconjunto.val(); }
            if (cboMes.val() > 0) { objJS.Mes = cboMes.val(); }
            if (chk20.prop("checked")) { arrEstatus.push(1); }
            if (chk40.prop("checked")) { arrEstatus.push(2); }
            if (chk50.prop("checked")) { arrEstatus.push(3); }
            if (chk60.prop("checked")) { arrEstatus.push(4); }
            if (chk80.prop("checked")) { arrEstatus.push(5); }
            if (chk90.prop("checked")) { arrEstatus.push(6); }
            if (chk100.prop("checked")) { arrEstatus.push(7); }
            objJS.lstEstatus = arrEstatus;
            return objJS;
        }

        function initTablaRequisiciones() {
            dtRequisiciones = tblBL_Requisiciones.DataTable({
                language: dtDicEsp,
                destroy: true,
                ordering: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: true,
                info: false,
                columns: [
                    { data: 'numRequisicion', title: 'Núm. Requisición' }
                ],
                columnDefs: [
                    // { className: "dt-center", "targets": "_all" },
                ],
            });
        }

        function fncGetRequisiciones(idBL) {
            let objRequisicion = new Object();
            objRequisicion = {
                idBackLog: idBL,
            };
            axios.post('GetRequisiciones', objRequisicion).catch(o_O => Alert2Error(o_O.message)).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtRequisiciones.clear();
                    dtRequisiciones.rows.add(response.data.lstRequisiciones);
                    dtRequisiciones.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaArchivos() {
            dtArchivos = tblEvidencias.DataTable({
                language: dtDicEsp,
                destroy: false,
                ordering: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: "nombreArchivo", title: "Nombre del archivo" },
                    { data: 'tipoEvidencia', title: 'Tipo evidencia' },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary verEvidencia"><i class="far fa-eye"></i></button>`;
                        }
                    },
                    {
                        render: (data, type, row) => {
                            let botones = ``;
                            botones += `<button type="button" class="btn btn-danger eliminarArchivo"  data-id="${row.id}"><i class="fa fa-trash"></i></button>`;
                            return botones;

                        }
                    }
                    //render: function (data, type, row) { }
                ],
                initComplete: function (settings, json) {
                    tblEvidencias.on("click", ".verEvidencia", function () {
                        let rowData = dtArchivos.row($(this).closest("tr")).data();
                        fncMostrarEvidencia(rowData.id);
                    });
                    tblEvidencias.on('click', '.classBtn', function () {
                        let rowData = dtArchivos.row($(this).closest('tr')).data();
                    });
                    tblEvidencias.on('click', '.eliminarArchivo', function () {
                        let rowData = dtArchivos.row($(this).closest("tr")).data();
                        let id = parseFloat(rowData.id);
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarArchivo(id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncMostrarEvidencia(idEvidencia) {
            let obj = new Object();
            obj = {
                idEvidencia: idEvidencia
            }
            axios.post("MostrarEvidencia", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    $('#myModal').data().ruta = null;
                    $('#myModal').modal('show');
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetArchivos(id) {
            let obj = new Object();
            obj = {
                id: id
            }
            axios.post("GetArchivos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtArchivos.clear().draw();
                    dtArchivos.rows.add(response.data.lstArchivos).draw();
                }
                else {
                    Alert2Error(message)
                }

            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarArchivo(id) {
            axios.post('EliminarArchivos', { id: id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha eliminado con éxito.");
                    fncGetArchivos(id);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncExportarExcel() {

            let arrNoEconomicos = Array();
            cboCC.val().forEach(element => {
                let objCC = cboCC.find(`option[value="${element}"]`);
                let noEconomico = objCC.text();
                arrNoEconomicos.push(noEconomico);

                axios.post('creaExcelito', { arrNoEconomicos: noEconomico })
                    .catch(o_O => AlertaGeneral(o_O.message))
                    .then(response => {
                        location.href = `crearExcelInspeccionesTMC`;
                    });
                $.unblockUI();

                setTimeout(function () {

                }, 10000);
            });

            // if (arrNoEconomicos.length == 1) {

            // } else {
            //     Alert2Warning("Es necesario seleccionar solamente un CC");
            // }
        }

        function fncLiberacion() {

            axios.post('ActualizarLiberacion', { areaCuenta: $('#cboCC option:selected').text() }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha liberado con exito.");
                    GetBackLogsFiltros();
                }
                else {
                    Alert2Error("No se pudo liberar el backlog, por qué no se encuentra instalado(100%), o no tiene evidencias registradas.");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initGraficaCargoNomina(datos) {
            Highcharts.chart('graficaCargoNomina', {
                chart: { plotBackgroundColor: null, plotBorderWidth: null, pltoShadow: false, type: 'pie' },
                title: { text: 'DISTRIBUCIÓN DE CARGOS POR CC' },
                tooltip: {
                    pointFormat: '{series.nombre}: <b>{point.y}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            formatter: function () {
                                return this.point.name + ': ' + this.point.y + '%';
                            }
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    name: '',
                    colorByPoint: true,
                    data: datos,
                    animation: {
                        complete: function () {
                            generarImagenGraficaHighcharts('graficaCargoNomina', DescargarExcelCargoNomina)
                        }
                    }
                }],
                credits: {
                    enabled: false
                }
            });
        }

        function DescargarExcelCargoNomina(params) {
            let form = document.createElement('form');

            form.method = 'POST';
            form.action = '/BackLogs/DescargarExcelCargoNomina';

            for (index in params) {
                var input = document.createElement('input');

                input.type = 'hidden';
                input.name = 'imagenString';
                input.value = params[index];

                form.appendChild(input);
            }

            document.body.appendChild(form);

            form.submit();
        }
    }

    $(document).ready(() => BackLogs = new BackLogs())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();