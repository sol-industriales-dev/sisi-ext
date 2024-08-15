(() => {
    $.namespace("Maquinaria.BackLogs.BackLogsInformeRehabilitacion");

    BackLogs = function () {
        //#region CONST
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
        const btnLimpiarFiltro = $("#btnLimpiarFiltro");
        const btnBuscar = $('#btnBuscar');
        const tblBL_CatBackLogs = $("#tblBL_CatBackLogs");
        const tblBL_CatBackLogs_tbody = $("#tblBL_CatBackLogs tbody");
        const modalVerOC = $('#modalVerOC');
        const tblBL_OrdenesCompra = $('#tblBL_OrdenesCompra');
        const tblBL_Requisiciones = $('#tblBL_Requisiciones');
        const modalVerRequisicion = $('#modalVerRequisicion');
        const btnCerrarMdlRequisiciones = $('#btnCerrarMdlRequisiciones');
        const btnCerrarMdlOC = $('#btnCerrarMdlOC');
        const cboFiltroAnio = $('#cboFiltroAnio');
        const btnFiltroBuscarBL = $('#btnFiltroBuscarBL');

        //BOTONES MENU
        const btnRegistroBackLogs = $("#btnRegistroBackLogs");
        const btnInformeRehabilitacion = $("#btnInformeRehabilitacion");
        const btnProgramaInspeccion = $("#btnProgramaInspeccion");
        const btnInicioObra = $('#btnInicioObra');
        const btnExportar = $('#btnExportar');

        //REPORTE GENERAL
        const divReporteGeneral = $('#divReporteGeneral');
        const rptGeneralFechaInicio = $('#rptGeneralFechaInicio');
        const rptGeneralFechaFin = $('#rptGeneralFechaFin');
        const rptGeneral = $('#rptGeneral');

        //CONST DETALLES REQUISICION
        const mdlLstDetReqCC = $('#mdlLstDetReqCC');
        const tblLstDetReqCC = $('#tblLstDetReqCC');

        //CONST DETALLES OC
        const mdlLstDetOcReq = $('#mdlLstDetOcReq');
        const tblLstDetOcReq = $('#tblLstDetOcReq');
        //#endregion

        //#region TOTAL MX
        const mdlTotalMX = $('#mdlTotalMX');
        const txtTotalMX = $('#txtTotalMX');
        //#endregion

        //#region MODAL EVIDENCIAS
        const mdlEvidencias = $('#mdlEvidencias');
        const tblEvidencias = $('#tblEvidencias');
        let dtEvidencias;
        //#endregion

        //#region CONST OCULTOS
        const inputEmpresaActual = $('#inputEmpresaActual');
        _empresaActual = +inputEmpresaActual.val();
        //#endregion

        let dtBackLogs;
        let arrFiltros = [];

        (function init() {
            rptGeneralFechaInicio.datepicker({}).datepicker();
            rptGeneralFechaFin.datepicker({}).datepicker();

            fncListeners();
            fncElementosPeru();
            initTablaBackLogs();
            fncFillCombos();
            fncDeshabilitarHabilitarFiltros();
            initTablaOC();
            initTablaRequisiciones();
            initTablaLstDetReqCC();
            initTablaLstDetOcReq();
            initTablaEvidencias();
            obtenerUrlPArams();
        })();

        function fncListeners() {
            btnInicioObra.click(function (e) {
                document.location.href = '/BackLogs/Index?areaCuenta=' + cboProyecto.val();
            });
            btnRegistroBackLogs.click(function (e) {
                document.location.href = '/BackLogs/RegistroBackLogsObra?areaCuenta=' + cboProyecto.val();
            });

            btnProgramaInspeccion.click(function (e) {
                document.location.href = '/BackLogs/ProgramaInspeccionBackLogs?areaCuenta=' + cboProyecto.val();
            })

            btnInformeRehabilitacion.click(function (e) {
                document.location.href = '/BackLogs/InformeBackLogsRehabilitacion?areaCuenta=' + cboProyecto.val();
            });

            $("#btnReporteIndicadores").click(function (e) {
                document.location.href = "/BackLogs/ReporteIndicadoresObra?areaCuenta=" + cboProyecto.val();
            });

            cboFiltroAnio.select2();

            btnFiltroBuscarBL.on("click", function () {
                if (cboProyecto.val() != "") {
                    fncLimpiarControles();
                } else {
                    fncLimpiarControles();
                    GetBackLogsFiltros();
                }
                fncDeshabilitarHabilitarFiltros();
            });

            cboCC.change(function (e) {
                if (cboCC.val() != "") {
                    fncGetDatosMaquina();
                    btnExportar.attr("disabled", false);
                } else {
                    fncFillCboModelo();
                    fncFillCboGrupo();
                    btnExportar.attr("disabled", false);
                }
                GetBackLogsFiltros();
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

            tblBL_CatBackLogs.on('draw.dt', function () {
                fncColorearCeldaEstatus();
            });

            btnBuscar.click(function (e) {
                GetBackLogsFiltros();
            });

            rptGeneral.on('click', function () {
                let strMensajeError = "";
                let fechaInicio = "";
                let fechaFin = "";

                rptGeneralFechaInicio.val() != "" ? fechaInicio == rptGeneralFechaInicio.val() : strMensajeError += "Es necesario indicar la fecha inicio.";
                rptGeneralFechaFin.val() != "" ? fechaFin == rptGeneralFechaFin.val() : strMensajeError += "<br>Es necesario indicar la fecha fin.";

                if (strMensajeError != "") {
                    Alert2Warning(strMensajeError);
                } else {
                    Alert2AccionConfirmar("Generar reporte", "¿Desea generar el reporte general?", "Confirmar", "Cancelar", () => fncGenerarReporteGeneral());
                }
            });

            btnExportar.click(function (e) {
                Alert2AccionConfirmar("Generar reporte", "¿Desea generar el reporte del CC seleccionado?", "Confirmar", "Cancelar", () => fncCrearReportePorMaquina());
            });
        }

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            // console.log('hola soy obtener url params')
            // console.log(variables)
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

        function fncGenerarReporteGeneral() {
            location.href = '/BackLogs/GetReporteGeneral?tipoBL=1' + '&fechaInicio=' +
                moment(rptGeneralFechaInicio.val(), 'DD/MM/YYYY').toISOString(true) + '&fechaFin=' +
                moment(rptGeneralFechaFin.val(), 'DD/MM/YYYY').toISOString(true) + '&ac=' + cboProyecto.val();
        }

        function fncFillCombos() {
            fncFillCboProyectosObra();
            fncFillCboCC();
            fncFillCboModelo();
            fncFillCboConjuntos();
            fncFillCboGrupo();
            fncFillCboMes();
            cboSubconjunto.select2();
        }

        function fncFillCboProyectosObra() {
            let idProyecto = cboProyecto.val();
            if (idProyecto == null) {
                if (_empresaActual == 6) {
                    cboProyecto.fillCombo("FillCboAC", {}, false);
                } else {
                    cboProyecto.fillCombo("/CatObra/cboCentroCostosUsuarios", {}, false);
                }
                cboProyecto.select2({ width: "resolve" });
            }
        }

        function fncFillCboCC() {
            cboCC.fillCombo("/BackLogs/FillCboCC", { areaCuenta: cboProyecto.val(), esObra: true }, false);
            cboCC.select2({
                width: "resolve"
            });
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
            tblBL_CatBackLogs_tbody.empty();
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
                btnExportar.attr("disabled", true);
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

                divReporteGeneral.hide();
            }
            cboModelo.attr("disabled", true);
            cboGrupo.attr("disabled", true);
        }

        function fncColorearCeldaEstatus() {
            tblBL_CatBackLogs.find("tbody tr").each(function (index) {
                $(this).addClass("rowHover");
                $(this).children("td").each(function (index2) {
                    switch (index2) {
                        case 8:
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
                                    $(this).addClass("boton-estatus-100 text-color");
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
            axios.post('GetMaquina', objNoEconomico).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (response.data.lstMaquina != null) {
                        cboModelo.val(response.data.lstMaquina[0].modeloEquipoID);
                        cboModelo.trigger("change");
                        cboGrupo.val(response.data.lstMaquina[0].grupoMaquinariaID);
                        cboGrupo.trigger("change");
                    } else {
                        cboModelo.val("");
                        cboGrupo.val("");
                    }
                } else {
                    Alert2Error("Ocurrió un error al obtener modelo y grupo del equipo.")
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaBackLogs() {
            dtBackLogs = tblBL_CatBackLogs.DataTable({
                paging: true,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: "noEconomico", title: "C.C." },
                    { data: "folioBL", title: "Folio" },
                    { data: "descripcion", title: "Descripción", visible: false },
                    {
                        title: "Fecha Insp.",
                        render: function (data, type, row) {
                            return moment(row.fechaInspeccion).format('DD/MM/YYYY');
                        }
                    },
                    { data: "conjunto", title: "Conjunto" },
                    {
                        title: 'Evidencias',
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary verEvidencias"><i class="fas fa-folder-open"></i></button>`;
                        }
                    },
                    { data: "subconjunto", title: "Subconjunto", visible: false },
                    {
                        title: 'Requi.',
                        render: function (data, type, row) {
                            let btn = "";
                            if (row.estatus == "Elaboración de Inspección (20%)") {
                                btn = "";
                            } else {
                                btn = `<button class="btn btn-primary verRequisicion"><i class="fas fa-list-ul"></i></button>`;
                            }
                            return btn;
                        }
                    },
                    {
                        title: 'O.C',
                        render: function (data, type, row) {
                            let btn = "";
                            if (row.estatus == "Elaboración de Inspección (20%)") {
                                btn = "";
                            } else {
                                btn = `<button class="btn btn-primary verOC"><i class="fas fa-shopping-cart"></i></button>`;
                            }
                            return btn;
                        }
                    },
                    {
                        title: 'Total',
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary verTotalMX"><i class="fas fa-money-bill-wave"></i></button>`;
                            return "$" + row.totalMX.toFixed(2);
                        }
                    },
                    {
                        data: "estatus", title: "Estatus",
                        render: function (data, type, row, cliente) {
                            switch (row.estatus) {
                                case "Elaboración de Inspección (20%)":
                                    return "20%";
                                case "Elaboración de Requisición (40%)":
                                    return "40%";
                                case "Elaboración de OC (50%)":
                                    return "50%";
                                case "Suministro de Refacciones (60%)":
                                    return "60%";
                                case "Rehabilitación Programada (80%)":
                                    return "80%";
                                case "Proceso de Instalación (90%)":
                                    return "90%";
                                case "BackLogs instalado (100%)":
                                    return "<i class='fas fa-check'></i>";
                            }
                        }
                    },
                    {
                        title: "Fecha cierre",
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
                    { data: "diasTotales", title: "Días totales" }
                ],
                initComplete: function (settings, json) {
                    tblBL_CatBackLogs.on("click", ".verOC", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        fncVerificarOrdenesCompra(rowData.id);
                        btnCerrarMdlOC.attr("data-id", rowData.id);
                        modalVerOC.modal("show");
                    });

                    tblBL_CatBackLogs.on("click", ".verRequisicion", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        fncGetRequisiciones(rowData.id);
                        btnCerrarMdlRequisiciones.attr("data-id", rowData.id);
                        modalVerRequisicion.modal("show");
                    });

                    tblBL_CatBackLogs.on("click", ".verEvidencias", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        fncGetEvidencias(rowData.id);
                        mdlEvidencias.modal("show");
                    });

                    tblBL_CatBackLogs.on("click", ".verTotalMX", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        fncGetTotalMX(rowData.id, rowData.noEconomico);
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

        function fncGetTotalMX(id, noEconomico) {
            let obj = new Object();
            obj = {
                idBL: id,
                noEconomico: noEconomico
            }
            axios.post("GetTotalOCRehabilitacion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    txtTotalMX.val(response.data.totalMX.toFixed(2));
                    mdlTotalMX.modal("show");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaEvidencias() {
            dtEvidencias = tblEvidencias.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'nombreArchivo', title: 'Archivo' },
                    {
                        data: 'tipoEvidencia', title: 'Tipo evidencia',
                        render: function (type, data, row) {
                            let esOTVacia = row.tipoEvidencia;
                            return esOTVacia;
                        }
                    },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary verEvidencia"><i class="far fa-eye"></i></button>`;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblEvidencias.on('click', '.eliminarEvidencia', function () {
                        let rowData = dtEvidencias.row($(this).closest('tr')).data();
                        let id = parseFloat(rowData.id);
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarArchivo(id));
                    });
                    tblEvidencias.on("click", ".verEvidencia", function () {
                        let rowData = dtEvidencias.row($(this).closest("tr")).data();
                        fncMostrarEvidencia(rowData.id);
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

        function mostrarArchivoEvidenciaAccion(id) { //OMAR
            if (id > 0) {
                $.post('CargarDatosArchivoEvidenciaSeguimientoAcciones', { id })
                    .then(response => {
                        if (response.success) {
                            $('#myModal').data().ruta = null;
                            $('#myModal').modal('show');
                        } else {
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        }

        function fncGetEvidencias(idBL) {
            let obj = new Object();
            obj = {
                id: idBL
            }
            axios.post("GetArchivos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtEvidencias.clear();
                    dtEvidencias.rows.add(response.data.lstArchivos);
                    dtEvidencias.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
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
                    { data: "numOC", title: "O.C." },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary verDetOc"><i class="fas fa-list"></i></button>&nbsp;`;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblBL_OrdenesCompra.on('click', '.verDetOc', function () {
                        fncGetLstDetOcReq();
                        mdlLstDetOcReq.modal("show");
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaLstDetOcReq() {
            dtLstDetOcReq = tblLstDetOcReq.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'numero', title: 'Núm. O.C' },
                    { data: 'partida', title: 'Partida' },
                    { data: 'num_requisicion', title: 'Núm. requisición' },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'precio', title: 'Precio' },
                    { data: 'importe', title: 'Importe' }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetLstDetOcReq() {
            let obj = new Object();
            obj = {
                idBackLog: btnCerrarMdlOC.attr("data-id")
            };
            axios.post("GetLstDetOcReq", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtLstDetOcReq.clear();
                    dtLstDetOcReq.rows.add(response.data.lstDetOCEK);
                    dtLstDetOcReq.draw();
                    mdlLstDetOcReq.modal("show");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearReportePorMaquina() {
            if ($('#cboCC option:selected').text() != "" && cboFiltroAnio.val() > 0 && cboProyecto.val() != "") {
                let objParamsDTO = {}
                objParamsDTO.noEconomico = $('#cboCC option:selected').text();
                objParamsDTO.anio = cboFiltroAnio.val();
                objParamsDTO.areaCuenta = cboProyecto.val();
                axios.post('SetVariablesSesion', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        location.href = "/BackLogs/GenerarExcelInspeccionesObras";
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if ($('#cboCC option:selected').text() == "") {
                    Alert2Warning("Es necesario seleccionar un CC.");
                }
                if (cboFiltroAnio.val() <= 0) {
                    Alert2Warning("Es necesario seleccionar un año.");
                }
                if (cboProyecto.val() <= 0) {
                    Alert2Warning("Es necesario seleccionar un área cuenta.");
                }
            }
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
            if (cboFiltroAnio.val() > 0) {
                let objFiltros = fncFiltros();
                axios.post('GetBackLogsFiltros', objFiltros).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        tblBL_CatBackLogs_tbody.empty();
                        dtBackLogs.clear();
                        dtBackLogs.rows.add(response.data.lstBackLogs);
                        dtBackLogs.draw();
                        fncColorearCeldaEstatus();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = "";
                strMensajeError += cboProyecto.val() <= 0 ? "Es necesario indicar el área cuenta." : "";
                strMensajeError += cboFiltroAnio.val() <= 0 ? "<br>Es necesario indicar el año." : "";
                Alert2Warning(strMensajeError);
            }
        }

        function fncFiltros() {
            let objJS = {};
            let arrEstatus = [];
            if (cboProyecto.val() != "") { objJS.areaCuenta = cboProyecto.val(); }
            let optionCC = cboCC.find(`option[value="${cboCC.val()}"]`);
            let prefijoCC = optionCC.attr("data-prefijo");
            if (cboCC.val() != "") { objJS.noEconomico = prefijoCC; }
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
            objJS.esObra = true;
            objJS.lstEstatus = arrEstatus;
            objJS.anio = cboFiltroAnio.val();
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
                    { data: 'numRequisicion', title: 'Núm. Requisición' },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary verDetReq"><i class='fas fa-list'></i></button>&nbsp;`;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblBL_Requisiciones.on("click", ".verDetReq", function () {
                        let rowData = dtRequisiciones.row($(this).closest('tr')).data();
                        fncGetLstDetReqCC(rowData.numRequisicion);
                        mdlLstDetReqCC.modal("show");
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
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

        function initTablaLstDetReqCC() {
            dtLstDetReqCC = tblLstDetReqCC.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'numero', title: 'Núm. requisición' },
                    { data: 'partida', title: 'Partida' },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'comentarios', title: 'Descripción' },
                    {
                        data: 'fecha_requerido', title: 'Fecha requerido',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'cantidad', title: 'Cantidad' },
                    {
                        data: 'fecha_ordenada', title: 'Fecha ordenada',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    // CODE ...
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetLstDetReqCC(numero) {
            let obj = new Object();
            obj = {
                idBackLog: btnCerrarMdlRequisiciones.attr("data-id"),
                numero: numero
            };
            axios.post("GetAllDetRequisiciones", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtLstDetReqCC.clear();
                    dtLstDetReqCC.rows.add(response.data.lstDetRequisicionesEK);
                    dtLstDetReqCC.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncElementosPeru() {
            if (_empresaActual == 6) {
                $(".lblPERU_totalMX").html("Total SOL");
            } else {
                $(".lblPERU_totalMX").html("Total MX");
            }
        }
    }

    $(document).ready(() => BackLogs = new BackLogs())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();