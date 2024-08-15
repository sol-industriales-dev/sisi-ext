(function () {

    $.namespace('Maquinaria.Backlogs.BackLogsObra');

    //#region CONST
    const tblBacklogsEstatusObra = $("#tblBacklogsEstatusObra");
    const tblBacklogsEstatusObra_tbody = $("#tblBacklogsEstatusObra tbody");
    const tblGraficaLineasBL = $("#tblGraficaLineasBL");
    const tblGraficaLineasBL_tbody = $("#tblGraficaLineasBL tbody");
    const btnProgramaInspeccion = $("#btnProgramaInspeccion");
    const btnInicioObra = $('#btnInicioObra');
    const btnRegistroBackLogs = $("#btnRegistroBackLogs");
    const btnInicio = $("#btnInicio");
    const btnInformeRehabilitacion = $("#btnInformeRehabilitacion");
    const cboProyecto = $("#cboProyecto");
    const cboFiltroAnio = $('#cboFiltroAnio')
    const btnFiltroBuscar = $('#btnFiltroBuscar')
    const tblTablaDatos = $('#tblTablaDatos');
    const btnReporte = $('#btnReporte');
    let dtTablaDatos;

    let arrTablaDatosBLActuales = [];
    let arrTablaDatosBLPorc = [];
    let arrTablaDatosBLTiempoPromedio = [];

    let arrTendenciaBLRegistrados = [];
    let arrTendenciaBLCerrados = [];
    let arrTendenciaBLAcumulados = [];

    let imgGrafica;

    const mdlLstBackLogs = $('#mdlLstBackLogs');
    const tblListadoBL = $('#tblListadoBL');
    let dtBackLogs;

    //#region  TABLA DATOS
    const tdNumBackLogs20 = $("#tdNumBackLogs20");
    const tdNumBackLogs40 = $("#tdNumBackLogs40");
    const tdNumBackLogs50 = $("#tdNumBackLogs50");
    const tdNumBackLogs60 = $("#tdNumBackLogs60");
    const tdNumBackLogs80 = $("#tdNumBackLogs80");
    const tdNumBackLogs90 = $("#tdNumBackLogs90");
    const tdNumBackLogs100 = $("#tdNumBackLogs100");
    const tdNumBackLogsTotal = $("#tdNumBackLogsTotal");

    const tdPorBackLogs20 = $("#tdPorBackLogs20");
    const tdPorBackLogs40 = $("#tdPorBackLogs40");
    const tdPorBackLogs50 = $("#tdPorBackLogs50");
    const tdPorBackLogs60 = $("#tdPorBackLogs60");
    const tdPorBackLogs80 = $("#tdPorBackLogs80");
    const tdPorBackLogs90 = $("#tdPorBackLogs90");
    const tdPorBackLogs100 = $("#tdPorBackLogs100");

    const diasPromBacklogs1 = $('#diasPromBacklogs1');
    const diasPromBacklogs2 = $('#diasPromBacklogs2');
    const diasPromBacklogs3 = $('#diasPromBacklogs3');
    const diasPromBacklogs4 = $('#diasPromBacklogs4');
    const diasPromBacklogs5 = $('#diasPromBacklogs5');
    const diasPromBacklogs6 = $('#diasPromBacklogs6');
    const diasPromBacklogs7 = $('#diasPromBacklogs7');
    const tddiasPromTotal = $('#tddiasPromTotal');

    const tdEneroBLRegistrados = $("#tdEneroBLRegistrados");
    const tdFebreroBLRegistrados = $("#tdFebreroBLRegistrados");
    const tdMarzoBLRegistrados = $("#tdMarzoBLRegistrados");
    const tdAbrilBLRegistrados = $("#tdAbrilBLRegistrados");
    const tdMayoBLRegistrados = $("#tdMayoBLRegistrados");
    const tdJunioBLRegistrados = $("#tdJunioBLRegistrados");
    const tdJulioBLRegistrados = $("#tdJulioBLRegistrados");
    const tdAgostoBLRegistrados = $("#tdAgostoBLRegistrados");
    const tdSeptiembreBLRegistrados = $("#tdSeptiembreBLRegistrados");
    const tdOctubreBLRegistrados = $("#tdOctubreBLRegistrados");
    const tdNoviembreBLRegistrados = $("#tdNoviembreBLRegistrados");
    const tdDiciembreBLRegistrados = $("#tdDiciembreBLRegistrados");

    const tdEneroBLCerrados = $("#tdEneroBLCerrados");
    const tdFebreroBLCerrados = $("#tdFebreroBLCerrados");
    const tdMarzoBLCerrados = $("#tdMarzoBLCerrados");
    const tdAbrilBLCerrados = $("#tdAbrilBLCerrados");
    const tdMayoBLCerrados = $("#tdMayoBLCerrados");
    const tdJunioBLCerrados = $("#tdJunioBLCerrados");
    const tdJulioBLCerrados = $("#tdJulioBLCerrados");
    const tdAgostoBLCerrados = $("#tdAgostoBLCerrados");
    const tdSeptiembreBLCerrados = $("#tdSeptiembreBLCerrados");
    const tdOctubreBLCerrados = $("#tdOctubreBLCerrados");
    const tdNoviembreBLCerrados = $("#tdNoviembreBLCerrados");
    const tdDiciembreBLCerrados = $("#tdDiciembreBLCerrados");

    const tdEneroBLAcumulados = $("#tdEneroBLAcumulados");
    const tdFebreroBLAcumulados = $("#tdFebreroBLAcumulados");
    const tdMarzoBLAcumulados = $("#tdMarzoBLAcumulados");
    const tdAbrilBLAcumulados = $("#tdAbrilBLAcumulados");
    const tdMayoBLAcumulados = $("#tdMayoBLAcumulados");
    const tdJunioBLAcumulados = $("#tdJunioBLAcumulados");
    const tdJulioBLAcumulados = $("#tdJulioBLAcumulados");
    const tdAgostoBLAcumulados = $("#tdAgostoBLAcumulados");
    const tdSeptiembreBLAcumulados = $("#tdSeptiembreBLAcumulados");
    const tdOctubreBLAcumulados = $("#tdOctubreBLAcumulados");
    const tdNoviembreBLAcumulados = $("#tdNoviembreBLAcumulados");
    const tdDiciembreBLAcumulados = $("#tdDiciembreBLAcumulados");

    const divEstatusBackLogs = $("#divEstatusBackLogs");
    const divTendenciaBackLogs = $("#divTendenciaBackLogs");
    const divTablaDatos = $("#divTablaDatos");
    const btnReporteIndicadores = $('#btnReporteIndicadores');

    let r1;
    let r2;
    let r3;
    let r4;
    let r5;
    let r6;
    let r7;
    let rt;
    //#endregion

    let _MES_ACTUAL;
    let _ANIO_ACTUAL;
    //#endregion

    //#region CONST OCULTOS
    const inputEmpresaActual = $('#inputEmpresaActual');
    _empresaActual = +inputEmpresaActual.val();
    //#endregion

    BackLogs = function () {
        (function init() {

            fncListeners();
            fncLimpiarBackLogsEstatusObra();
            fncMostrarOcultar();
            initTablaDatos();
            obtenerUrlPArams();
        })();

        function fncListeners() {
            fncFillCboProyectosObra();

            btnReporte.css("display", "none");

            tblGraficaLineasBL.hide();
            tblBacklogsEstatusObra.hide();

            btnInicioObra.click(function () {
                location.reload();
            });

            btnProgramaInspeccion.click(function (e) {
                document.location.href = '/BackLogs/ProgramaInspeccionBackLogs?areaCuenta=' + cboProyecto.val();
            })

            btnRegistroBackLogs.click(function () {
                document.location.href = '/BackLogs/RegistroBackLogsObra?areaCuenta=' + cboProyecto.val();
            });

            btnInformeRehabilitacion.click(function (e) {
                document.location.href = '/BackLogs/InformeBackLogsRehabilitacion?areCuenta=' + cboProyecto.val();
            });

            btnReporteIndicadores.click(function (e) {
                document.location.href = '/BackLogs/ReporteIndicadoresObra?areCuenta=' + cboProyecto.val();

            });

            // cboProyecto.change(function (e) {
            //     fncGetGraficas()
            // });

            btnReporte.on("click", function (e) {
                fncGenerarReporteCrystalReport($('#graficaLineas').highcharts(), 'chart');
                Alert2AccionConfirmar('Generar reporte', '¿Desea generar y descargar el reporte?', 'Confirmar', 'Cancelar', () => fncGenerarReporte());
            });

            $("#btnSandBox").on("click", function (e) {
                fncGenerarReporteCrystalReport($('#graficaLineas').highcharts(), 'chart');
            });

            btnFiltroBuscar.click(function () {
                fncGetGraficas()
            })

            //#region SE OBTIENE LISTADO DE BACKLOGS EN BASE AL MES SELECCIONADO
            tdEneroBLAcumulados.on("click", function () {
                // initTblBackLogs();
                fncGetBackLogsGraficaIndex(1);
            });

            tdFebreroBLAcumulados.on("click", function () {
                // initTblBackLogs();
                fncGetBackLogsGraficaIndex(2);
            });

            tdMarzoBLAcumulados.on("click", function () {
                // initTblBackLogs();
                fncGetBackLogsGraficaIndex(3);
            });

            tdAbrilBLAcumulados.on("click", function () {
                // initTblBackLogs();
                fncGetBackLogsGraficaIndex(4);
            });

            tdMayoBLAcumulados.on("click", function () {
                // initTblBackLogs();
                fncGetBackLogsGraficaIndex(5);
            });

            tdJunioBLAcumulados.on("click", function () {
                // initTblBackLogs();
                fncGetBackLogsGraficaIndex(6);
            });

            tdJulioBLAcumulados.on("click", function () {
                // initTblBackLogs();
                fncGetBackLogsGraficaIndex(7);
            });

            tdAgostoBLAcumulados.on("click", function () {
                // initTblBackLogs();
                fncGetBackLogsGraficaIndex(8);
            });

            tdSeptiembreBLAcumulados.on("click", function () {
                // initTblBackLogs();
                fncGetBackLogsGraficaIndex(9);
            });

            tdOctubreBLAcumulados.on("click", function () {
                // initTblBackLogs();
                fncGetBackLogsGraficaIndex(10);
            });

            tdNoviembreBLAcumulados.on("click", function () {
                // initTblBackLogs();
                fncGetBackLogsGraficaIndex(11);
            });

            tdDiciembreBLAcumulados.on("click", function () {
                // initTblBackLogs();
                fncGetBackLogsGraficaIndex(12);
            });
            //#endregion
        }

        function fncGetGraficas() {
            if (cboProyecto.val() != "" && cboFiltroAnio.val() > 0) {
                tblGraficaLineasBL.show();
                tblBacklogsEstatusObra.show();
                fncGetDate();
                fncGetNumBackLogs();
                btnReporte.css("display", "inline");
            } else {
                tblGraficaLineasBL.hide();
                tblBacklogsEstatusObra.hide();
            }
            fncMostrarOcultar()
        }

        function fncGetBackLogsGraficaIndex(mes) {
            let obj = new Object();
            obj.anio = cboFiltroAnio.val()
            obj.Mes = mes
            obj.areaCuenta = cboProyecto.val()
            axios.post("GetBackLogsGraficaIndex", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    if (dtBackLogs != undefined) {
                        tblListadoBL.DataTable().clear().destroy();
                    }
                    initTblBackLogs();
                    dtBackLogs.clear();
                    dtBackLogs.rows.add(response.data.lstBL);
                    dtBackLogs.draw();
                    mdlLstBackLogs.modal("show");
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblBackLogs() {
            dtBackLogs = tblListadoBL.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'folioBL', title: 'Folio' },
                    { data: 'cc', title: 'CC' },
                    {
                        data: 'fechaInspeccion', title: 'Fecha elaboración',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    {
                        data: 'fechaModificacionBL', title: 'Fecha instalado', visible: false,
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'estatus', title: 'Estatus' }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', targets: '_all' },
                    { className: 'dt-body-center', targets: "_all" },
                    { width: '5%', targets: [0] }
                ],
            });
        }

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

        function fncMostrarOcultar() {
            if (cboProyecto.val() != "") {
                divEstatusBackLogs.show();
                divTendenciaBackLogs.show();
                divTablaDatos.show();
            } else {
                divEstatusBackLogs.hide();
                divTendenciaBackLogs.hide();
                divTablaDatos.hide();
            }
        }

        function fncGetDate() {
            var d = new Date();
            var mm = d.getMonth() + 1;
            var yyyy = d.getFullYear();
            _MES_ACTUAL = mm;
            _ANIO_ACTUAL = yyyy;
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

        function fncLimpiarBackLogsEstatusObra() {
            tdNumBackLogs20.html("");
            tdNumBackLogs40.html("");
            tdNumBackLogs50.html("");
            tdNumBackLogs60.html("");
            tdNumBackLogs80.html("");
            tdNumBackLogs90.html("");
            tdNumBackLogs100.html("");
            tdPorBackLogs20.html("");
            tdPorBackLogs40.html("");
            tdPorBackLogs50.html("");
            tdPorBackLogs60.html("");
            tdPorBackLogs80.html("");
            tdPorBackLogs90.html("");
            tdPorBackLogs100.html("");
            tdNumBackLogsTotal.html("");

            tdEneroBLRegistrados.html("");
            tdFebreroBLRegistrados.html("");
            tdMarzoBLRegistrados.html("");
            tdAbrilBLRegistrados.html("");
            tdMayoBLRegistrados.html("");
            tdJunioBLRegistrados.html("");
            tdJulioBLRegistrados.html("");
            tdAgostoBLRegistrados.html("");
            tdSeptiembreBLRegistrados.html("");
            tdOctubreBLRegistrados.html("");
            tdNoviembreBLRegistrados.html("");
            tdDiciembreBLRegistrados.html("");

            tdEneroBLCerrados.html("");
            tdFebreroBLCerrados.html("");
            tdMarzoBLCerrados.html("");
            tdAbrilBLCerrados.html("");
            tdMayoBLCerrados.html("");
            tdJunioBLCerrados.html("");
            tdJulioBLCerrados.html("");
            tdAgostoBLCerrados.html("");
            tdSeptiembreBLCerrados.html("");
            tdOctubreBLCerrados.html("");
            tdNoviembreBLCerrados.html("");
            tdDiciembreBLCerrados.html("");

            tdEneroBLAcumulados.html("");
            tdFebreroBLAcumulados.html("");
            tdMarzoBLAcumulados.html("");
            tdAbrilBLAcumulados.html("");
            tdMayoBLAcumulados.html("");
            tdJunioBLAcumulados.html("");
            tdJulioBLAcumulados.html("");
            tdAgostoBLAcumulados.html("");
            tdSeptiembreBLAcumulados.html("");
            tdOctubreBLAcumulados.html("");
            tdNoviembreBLAcumulados.html("");
            tdDiciembreBLAcumulados.html("");

            diasPromBacklogs1.html("");
            diasPromBacklogs2.html("");
            diasPromBacklogs3.html("");
            diasPromBacklogs4.html("");
            diasPromBacklogs5.html("");
            diasPromBacklogs6.html("");
            diasPromBacklogs7.html("");
            tddiasPromTotal.html("");
        }

        function fncGetNumBackLogs() {
            if (cboProyecto.val() != "" && cboFiltroAnio.val() > 0) {
                let objParamDTO = {}
                objParamDTO = {
                    areaCuenta: cboProyecto.val(),
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetDatosGraficasBLObra", objParamDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncLimpiarBackLogsEstatusObra();

                        //#region TABLA DE DATOS
                        tdNumBackLogs20.append(response.data.resultadosCantEstatus.lstCantEstatus[0] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[0] : parseFloat(0));
                        tdNumBackLogs40.append(response.data.resultadosCantEstatus.lstCantEstatus[1] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[1] : parseFloat(0));
                        tdNumBackLogs50.append(response.data.resultadosCantEstatus.lstCantEstatus[2] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[2] : parseFloat(0));
                        tdNumBackLogs60.append(response.data.resultadosCantEstatus.lstCantEstatus[3] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[3] : parseFloat(0));
                        tdNumBackLogs80.append(response.data.resultadosCantEstatus.lstCantEstatus[4] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[4] : parseFloat(0));
                        tdNumBackLogs90.append(response.data.resultadosCantEstatus.lstCantEstatus[5] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[5] : parseFloat(0));
                        tdNumBackLogs100.append(response.data.resultadosCantEstatus.lstCantEstatus[6] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[6] : parseFloat(0));
                        tdNumBackLogsTotal.append(response.data.resultadosCantEstatus.lstCantEstatus[7] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[7] : parseFloat(0));

                        estatus20 = (response.data.resultadosCantEstatus.lstCantEstatus[0] > 0) ?
                            ((response.data.resultadosCantEstatus.lstCantEstatus[0] / response.data.resultadosCantEstatus.lstCantEstatus[7]) * 100) : parseFloat(0);
                        tdPorBackLogs20.append(parseFloat(estatus20).toFixed(2) + "%");

                        estatus40 = (response.data.resultadosCantEstatus.lstCantEstatus[1] > 0) ?
                            ((response.data.resultadosCantEstatus.lstCantEstatus[1] / response.data.resultadosCantEstatus.lstCantEstatus[7]) * 100) : parseFloat(0);
                        tdPorBackLogs40.append(parseFloat(estatus40).toFixed(2) + "%");

                        estatus50 = (response.data.resultadosCantEstatus.lstCantEstatus[2] > 0) ?
                            ((response.data.resultadosCantEstatus.lstCantEstatus[2] / response.data.resultadosCantEstatus.lstCantEstatus[7]) * 100) : parseFloat(0);
                        tdPorBackLogs50.append(parseFloat(estatus50).toFixed(2) + "%");

                        estatus60 = (response.data.resultadosCantEstatus.lstCantEstatus[3] > 0) ?
                            ((response.data.resultadosCantEstatus.lstCantEstatus[3] / response.data.resultadosCantEstatus.lstCantEstatus[7]) * 100) : parseFloat(0);
                        tdPorBackLogs60.append(parseFloat(estatus60).toFixed(2) + "%");

                        estatus80 = (response.data.resultadosCantEstatus.lstCantEstatus[4] > 0) ?
                            ((response.data.resultadosCantEstatus.lstCantEstatus[4] / response.data.resultadosCantEstatus.lstCantEstatus[7]) * 100) : parseFloat(0);
                        tdPorBackLogs80.append(parseFloat(estatus80).toFixed(2) + "%");

                        estatus90 = (response.data.resultadosCantEstatus.lstCantEstatus[5] > 0) ?
                            ((response.data.resultadosCantEstatus.lstCantEstatus[5] / response.data.resultadosCantEstatus.lstCantEstatus[7]) * 100) : parseFloat(0);
                        tdPorBackLogs90.append(parseFloat(estatus90).toFixed(2) + "%");

                        estatus100 = (response.data.resultadosCantEstatus.lstCantEstatus[6] > 0) ?
                            ((response.data.resultadosCantEstatus.lstCantEstatus[6] / response.data.resultadosCantEstatus.lstCantEstatus[7]) * 100) : parseFloat(0);
                        tdPorBackLogs100.append(parseFloat(estatus100).toFixed(2) + "%");

                        //#region SE CONSTRUYE ARREGLO CON LA INFORMACIÓN DE: TABLA DE DATOS
                        arrTablaDatosBLActuales.push(response.data.resultadosCantEstatus.lstCantEstatus[0] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[0] : parseFloat(0));
                        arrTablaDatosBLActuales.push(response.data.resultadosCantEstatus.lstCantEstatus[1] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[1] : parseFloat(0));
                        arrTablaDatosBLActuales.push(response.data.resultadosCantEstatus.lstCantEstatus[2] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[2] : parseFloat(0));
                        arrTablaDatosBLActuales.push(response.data.resultadosCantEstatus.lstCantEstatus[3] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[3] : parseFloat(0));
                        arrTablaDatosBLActuales.push(response.data.resultadosCantEstatus.lstCantEstatus[4] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[4] : parseFloat(0));
                        arrTablaDatosBLActuales.push(response.data.resultadosCantEstatus.lstCantEstatus[5] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[5] : parseFloat(0));
                        arrTablaDatosBLActuales.push(response.data.resultadosCantEstatus.lstCantEstatus[6] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[6] : parseFloat(0));
                        arrTablaDatosBLActuales.push(response.data.resultadosCantEstatus.lstCantEstatus[7] > 0 ? response.data.resultadosCantEstatus.lstCantEstatus[7] : parseFloat(0));

                        arrTablaDatosBLPorc.push(parseFloat(estatus20).toFixed(2) + "%");
                        arrTablaDatosBLPorc.push(parseFloat(estatus40).toFixed(2) + "%");
                        arrTablaDatosBLPorc.push(parseFloat(estatus50).toFixed(2) + "%");
                        arrTablaDatosBLPorc.push(parseFloat(estatus60).toFixed(2) + "%");
                        arrTablaDatosBLPorc.push(parseFloat(estatus80).toFixed(2) + "%");
                        arrTablaDatosBLPorc.push(parseFloat(estatus90).toFixed(2) + "%");
                        arrTablaDatosBLPorc.push(parseFloat(estatus100).toFixed(2) + "%");
                        //#endregion

                        //#region TIEMPO PROMEDIO TABLA INFERIOR
                        if (response.data.resultadosTiempoPromedio.lstTiempoPromedio != null) {
                            r1 = diasPromBacklogs1.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[0].toFixed(2)) + " días");
                            r2 = diasPromBacklogs2.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[1].toFixed(2)) + " días");
                            r3 = diasPromBacklogs3.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[2].toFixed(2)) + " días");
                            r4 = diasPromBacklogs4.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[3].toFixed(2)) + " días");
                            r5 = diasPromBacklogs5.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[4].toFixed(2)) + " días");
                            r6 = diasPromBacklogs6.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[5].toFixed(2)) + " días");
                            r7 = diasPromBacklogs7.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[6].toFixed(2)));
                            rt = tddiasPromTotal.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[7].toFixed(2)) + " días");

                            arrTablaDatosBLTiempoPromedio.push(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[0].toFixed(2)) + " días");
                            arrTablaDatosBLTiempoPromedio.push(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[1].toFixed(2)) + " días");
                            arrTablaDatosBLTiempoPromedio.push(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[2].toFixed(2)) + " días");
                            arrTablaDatosBLTiempoPromedio.push(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[3].toFixed(2)) + " días");
                            arrTablaDatosBLTiempoPromedio.push(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[4].toFixed(2)) + " días");
                            arrTablaDatosBLTiempoPromedio.push(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[5].toFixed(2)) + " días");
                            arrTablaDatosBLTiempoPromedio.push(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[6].toFixed(2)));
                            arrTablaDatosBLTiempoPromedio.push(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[7].toFixed(2)) + " días");
                        }
                        //#endregion

                        //#endregion

                        //#region TABLA INFERIOR DE GRAFICA DE LINEAS
                        //#region BL REGISTRADOS
                        tdEneroBLRegistrados.append(response.data.resultadosCantEstatusLineas.lstcontador[0] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[0] : parseFloat(0));
                        tdFebreroBLRegistrados.append(response.data.resultadosCantEstatusLineas.lstcontador[1] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[1] : parseFloat(0));
                        tdMarzoBLRegistrados.append(response.data.resultadosCantEstatusLineas.lstcontador[2] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[2] : parseFloat(0));
                        tdAbrilBLRegistrados.append(response.data.resultadosCantEstatusLineas.lstcontador[3] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[3] : parseFloat(0));
                        tdMayoBLRegistrados.append(response.data.resultadosCantEstatusLineas.lstcontador[4] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[4] : parseFloat(0));
                        tdJunioBLRegistrados.append(response.data.resultadosCantEstatusLineas.lstcontador[5] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[5] : parseFloat(0));
                        tdJulioBLRegistrados.append(response.data.resultadosCantEstatusLineas.lstcontador[6] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[6] : parseFloat(0));
                        tdAgostoBLRegistrados.append(response.data.resultadosCantEstatusLineas.lstcontador[7] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[7] : parseFloat(0));
                        tdSeptiembreBLRegistrados.append(response.data.resultadosCantEstatusLineas.lstcontador[8] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[8] : parseFloat(0));
                        tdOctubreBLRegistrados.append(response.data.resultadosCantEstatusLineas.lstcontador[9] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[9] : parseFloat(0));
                        tdNoviembreBLRegistrados.append(response.data.resultadosCantEstatusLineas.lstcontador[10] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[10] : parseFloat(0));
                        tdDiciembreBLRegistrados.append(response.data.resultadosCantEstatusLineas.lstcontador[11] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[11] : parseFloat(0));

                        //#region SE CONSTRUYE ARREGLO CON INFORMACIÓN DE: TENDENCIA BL REGISTRADOS
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineas.lstcontador[0] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[0] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineas.lstcontador[1] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[1] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineas.lstcontador[2] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[2] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineas.lstcontador[3] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[3] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineas.lstcontador[4] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[4] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineas.lstcontador[5] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[5] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineas.lstcontador[6] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[6] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineas.lstcontador[7] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[7] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineas.lstcontador[8] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[8] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineas.lstcontador[9] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[9] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineas.lstcontador[10] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[10] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineas.lstcontador[11] > 0 ? response.data.resultadosCantEstatusLineas.lstcontador[11] : parseFloat(0));
                        //#endregion

                        //#endregion

                        //#region BL CERRADOS
                        tdEneroBLCerrados.append(response.data.resultadosCantEstatusLineas.IstContadorInstalados[0] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[0] : parseFloat(0));
                        tdFebreroBLCerrados.append(response.data.resultadosCantEstatusLineas.IstContadorInstalados[1] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[1] : parseFloat(0));
                        tdMarzoBLCerrados.append(response.data.resultadosCantEstatusLineas.IstContadorInstalados[2] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[2] : parseFloat(0));
                        tdAbrilBLCerrados.append(response.data.resultadosCantEstatusLineas.IstContadorInstalados[3] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[3] : parseFloat(0));
                        tdMayoBLCerrados.append(response.data.resultadosCantEstatusLineas.IstContadorInstalados[4] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[4] : parseFloat(0));
                        tdJunioBLCerrados.append(response.data.resultadosCantEstatusLineas.IstContadorInstalados[5] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[5] : parseFloat(0));
                        tdJulioBLCerrados.append(response.data.resultadosCantEstatusLineas.IstContadorInstalados[6] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[6] : parseFloat(0));
                        tdAgostoBLCerrados.append(response.data.resultadosCantEstatusLineas.IstContadorInstalados[7] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[7] : parseFloat(0));
                        tdSeptiembreBLCerrados.append(response.data.resultadosCantEstatusLineas.IstContadorInstalados[8] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[8] : parseFloat(0));
                        tdOctubreBLCerrados.append(response.data.resultadosCantEstatusLineas.IstContadorInstalados[9] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[9] : parseFloat(0));
                        tdNoviembreBLCerrados.append(response.data.resultadosCantEstatusLineas.IstContadorInstalados[10] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[10] : parseFloat(0));
                        tdDiciembreBLCerrados.append(response.data.resultadosCantEstatusLineas.IstContadorInstalados[11] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[11] : parseFloat(0));

                        //#region SE CONSTRUYE ARREGLO CON INFORMACIÓN DE: TENDENCIA BL CERRADOS
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineas.IstContadorInstalados[0] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[0] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineas.IstContadorInstalados[1] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[1] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineas.IstContadorInstalados[2] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[2] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineas.IstContadorInstalados[3] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[3] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineas.IstContadorInstalados[4] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[4] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineas.IstContadorInstalados[5] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[5] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineas.IstContadorInstalados[6] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[6] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineas.IstContadorInstalados[7] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[7] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineas.IstContadorInstalados[8] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[8] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineas.IstContadorInstalados[9] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[9] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineas.IstContadorInstalados[10] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[10] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineas.IstContadorInstalados[11] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorInstalados[11] : parseFloat(0));
                        //#endregion

                        //#endregion

                        //#region BL ACUMULADOS
                        if (_ANIO_ACTUAL == cboFiltroAnio.val()) {
                            tdEneroBLAcumulados.append(_MES_ACTUAL >= 1 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[0] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[0] : parseFloat(0) : 0);
                            tdFebreroBLAcumulados.append(_MES_ACTUAL >= 2 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[1] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[1] : parseFloat(0) : 0);
                            tdMarzoBLAcumulados.append(_MES_ACTUAL >= 3 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[2] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[2] : parseFloat(0) : 0);
                            tdAbrilBLAcumulados.append(_MES_ACTUAL >= 4 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[3] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[3] : parseFloat(0) : 0);
                            tdMayoBLAcumulados.append(_MES_ACTUAL >= 5 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[4] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[4] : parseFloat(0) : 0);
                            tdJunioBLAcumulados.append(_MES_ACTUAL >= 6 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[5] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[5] : parseFloat(0) : 0);
                            tdJulioBLAcumulados.append(_MES_ACTUAL >= 7 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[6] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[6] : parseFloat(0) : 0);
                            tdAgostoBLAcumulados.append(_MES_ACTUAL >= 8 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[7] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[7] : parseFloat(0) : 0);
                            tdSeptiembreBLAcumulados.append(_MES_ACTUAL >= 9 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[8] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[8] : parseFloat(0) : 0);
                            tdOctubreBLAcumulados.append(_MES_ACTUAL >= 10 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[9] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[9] : parseFloat(0) : 0);
                            tdNoviembreBLAcumulados.append(_MES_ACTUAL >= 11 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[10] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[10] : parseFloat(0) : 0);
                            tdDiciembreBLAcumulados.append(_MES_ACTUAL >= 12 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[11] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[11] : parseFloat(0) : 0);

                            arrTendenciaBLAcumulados.push(_MES_ACTUAL >= 1 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[0] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[0] : parseFloat(0) : 0);
                            arrTendenciaBLAcumulados.push(_MES_ACTUAL >= 2 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[1] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[1] : parseFloat(0) : 0);
                            arrTendenciaBLAcumulados.push(_MES_ACTUAL >= 3 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[2] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[2] : parseFloat(0) : 0);
                            arrTendenciaBLAcumulados.push(_MES_ACTUAL >= 4 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[3] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[3] : parseFloat(0) : 0);
                            arrTendenciaBLAcumulados.push(_MES_ACTUAL >= 5 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[4] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[4] : parseFloat(0) : 0);
                            arrTendenciaBLAcumulados.push(_MES_ACTUAL >= 6 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[5] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[5] : parseFloat(0) : 0);
                            arrTendenciaBLAcumulados.push(_MES_ACTUAL >= 7 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[6] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[6] : parseFloat(0) : 0);
                            arrTendenciaBLAcumulados.push(_MES_ACTUAL >= 8 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[7] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[7] : parseFloat(0) : 0);
                            arrTendenciaBLAcumulados.push(_MES_ACTUAL >= 9 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[8] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[8] : parseFloat(0) : 0);
                            arrTendenciaBLAcumulados.push(_MES_ACTUAL >= 10 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[9] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[9] : parseFloat(0) : 0);
                            arrTendenciaBLAcumulados.push(_MES_ACTUAL >= 11 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[10] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[10] : parseFloat(0) : 0);
                            arrTendenciaBLAcumulados.push(_MES_ACTUAL >= 12 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[11] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[11] : parseFloat(0) : 0);
                        } else {
                            tdEneroBLAcumulados.append(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[0] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[0] : parseFloat(0));
                            tdFebreroBLAcumulados.append(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[1] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[1] : parseFloat(0));
                            tdMarzoBLAcumulados.append(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[2] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[2] : parseFloat(0));
                            tdAbrilBLAcumulados.append(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[3] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[3] : parseFloat(0));
                            tdMayoBLAcumulados.append(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[4] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[4] : parseFloat(0));
                            tdJunioBLAcumulados.append(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[5] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[5] : parseFloat(0));
                            tdJulioBLAcumulados.append(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[6] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[6] : parseFloat(0));
                            tdAgostoBLAcumulados.append(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[7] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[7] : parseFloat(0));
                            tdSeptiembreBLAcumulados.append(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[8] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[8] : parseFloat(0));
                            tdOctubreBLAcumulados.append(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[9] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[9] : parseFloat(0));
                            tdNoviembreBLAcumulados.append(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[10] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[10] : parseFloat(0));
                            tdDiciembreBLAcumulados.append(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[11] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[11] : parseFloat(0));

                            arrTendenciaBLAcumulados.push(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[0] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[0] : parseFloat(0));
                            arrTendenciaBLAcumulados.push(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[1] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[1] : parseFloat(0));
                            arrTendenciaBLAcumulados.push(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[2] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[2] : parseFloat(0));
                            arrTendenciaBLAcumulados.push(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[3] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[3] : parseFloat(0));
                            arrTendenciaBLAcumulados.push(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[4] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[4] : parseFloat(0));
                            arrTendenciaBLAcumulados.push(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[5] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[5] : parseFloat(0));
                            arrTendenciaBLAcumulados.push(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[6] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[6] : parseFloat(0));
                            arrTendenciaBLAcumulados.push(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[7] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[7] : parseFloat(0));
                            arrTendenciaBLAcumulados.push(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[8] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[8] : parseFloat(0));
                            arrTendenciaBLAcumulados.push(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[9] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[9] : parseFloat(0));
                            arrTendenciaBLAcumulados.push(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[10] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[10] : parseFloat(0));
                            arrTendenciaBLAcumulados.push(response.data.resultadosCantEstatusLineas.IstContadorAcumulados[11] > 0 ? response.data.resultadosCantEstatusLineas.IstContadorAcumulados[11] : parseFloat(0));
                        }

                        fncCargarGraficaPastel()
                        fncCargarGraficaLineas()
                        tblBacklogsEstatusObra.show()
                    } else {
                        Alert2Error(response.message)
                    }
                }).catch(error => Alert2Error(error.message))
            }
        }

        function initTablaDatos() {
            dtTablaDatos = tblTablaDatos.DataTable({
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                columns: [
                    { title: 'Estatus' },
                    { title: 'BackLogs (Actual)' },
                    { title: '%' },
                    { title: 'Tiempo promedio' }
                ]
            });
        }

        function fncCargarGraficaPastel() {
            if (cboProyecto.val() != "") {
                Highcharts.chart('graficaPastel', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: { text: '' },
                    tooltip: { pointFormat: '{series.name}: <b>{point.percentage:.2f}%</b>' },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: { enabled: false },
                            showInLegend: true
                        }
                    },
                    series: [{
                        name: 'Estatus',
                        colorByPoint: true,
                        data: [{
                            name: 'Elaboración de Inspección (20%)',
                            color: "#ff3838",
                            y: estatus20,
                            sliced: true,
                            selected: true
                        }, {
                            name: 'Elaboración de Requisición (40%)',
                            color: "#F4B084",
                            y: estatus40
                        }, {
                            name: 'Elaboración de OC (50%)',
                            color: "#ffc000",
                            y: estatus50
                        }, {
                            name: 'Suministro de Refacciones (60%)',
                            color: "#ffff00",
                            y: estatus60
                        }, {
                            name: 'Programación de BackLogs (80%)',
                            color: "#9bc2e6",
                            y: estatus80
                        }, {
                            name: 'Proceso de Instalación (90%)',
                            color: "#4472c4",
                            y: estatus90
                        }, {
                            name: 'Backlogs instalado (100%)',
                            color: "#92d050",
                            y: estatus100
                        }]

                    }],
                    credits: { enabled: false }
                });
            }
        }

        function fncCargarGraficaLineas() {
            if (cboProyecto.val() != "") {
                Highcharts.chart('graficaLineas', {
                    title: { text: '' },
                    xAxis: { categories: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'] },
                    yAxis: { title: { text: '' } },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle'
                    },
                    plotOptions: {
                        series: { label: { connectorAllowed: false } }
                    },
                    series: [{
                        name: 'Backlogs registrados',
                        data: [
                            parseFloat(tdEneroBLRegistrados.text()),
                            parseFloat(tdFebreroBLRegistrados.text()),
                            parseFloat(tdMarzoBLRegistrados.text()),
                            parseFloat(tdAbrilBLRegistrados.text()),
                            parseFloat(tdMayoBLRegistrados.text()),
                            parseFloat(tdJunioBLRegistrados.text()),
                            parseFloat(tdJulioBLRegistrados.text()),
                            parseFloat(tdAgostoBLRegistrados.text()),
                            parseFloat(tdSeptiembreBLRegistrados.text()),
                            parseFloat(tdOctubreBLRegistrados.text()),
                            parseFloat(tdNoviembreBLRegistrados.text()),
                            parseFloat(tdDiciembreBLRegistrados.text())
                        ]
                    }, {
                        name: 'Backlogs cerrados',
                        data: [
                            parseFloat(tdEneroBLCerrados.text()),
                            parseFloat(tdFebreroBLCerrados.text()),
                            parseFloat(tdMarzoBLCerrados.text()),
                            parseFloat(tdAbrilBLCerrados.text()),
                            parseFloat(tdMayoBLCerrados.text()),
                            parseFloat(tdJunioBLCerrados.text()),
                            parseFloat(tdJulioBLCerrados.text()),
                            parseFloat(tdAgostoBLCerrados.text()),
                            parseFloat(tdSeptiembreBLCerrados.text()),
                            parseFloat(tdOctubreBLCerrados.text()),
                            parseFloat(tdNoviembreBLCerrados.text()),
                            parseFloat(tdDiciembreBLCerrados.text())
                        ]
                    }, {
                        name: 'Backlogs acumulados',
                        data: [
                            parseFloat(tdEneroBLAcumulados.text()),
                            parseFloat(tdFebreroBLAcumulados.text()),
                            parseFloat(tdMarzoBLAcumulados.text()),
                            parseFloat(tdAbrilBLAcumulados.text()),
                            parseFloat(tdMayoBLAcumulados.text()),
                            parseFloat(tdJunioBLAcumulados.text()),
                            parseFloat(tdJulioBLAcumulados.text()),
                            parseFloat(tdAgostoBLAcumulados.text()),
                            parseFloat(tdSeptiembreBLAcumulados.text()),
                            parseFloat(tdOctubreBLAcumulados.text()),
                            parseFloat(tdNoviembreBLAcumulados.text()),
                            parseFloat(tdDiciembreBLAcumulados.text())
                        ]
                    }],
                    responsive: {
                        rules: [{
                            condition: { maxWidth: 500 },
                            chartOptions: {
                                legend: {
                                    layout: 'horizontal',
                                    align: 'center',
                                    verticalAlign: 'bottom'
                                }
                            }
                        }]
                    },
                    credits: { enabled: false }
                });
            }
        }

        function fncGetNumBacklogsAniosAnteriores() {
            if (cboProyecto.val() != "") {
                $.ajax({
                    datatype: "json",
                    type: "GET",
                    url: "/BackLogs/GetNumBacklogsAniosAnteriores",
                    data: {
                        idProyecto: cboProyecto.val()
                    },
                    success: function (response) {
                        if (response.success) {
                            fncGetNumBackLogs(response.BackLogsAniosAnteriores);
                        }
                    },
                    error: function () {
                        AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                    },
                    complete: function () {
                        $.unblockUI();
                    }
                });
                tblBacklogsEstatusObra.show();
            }
        }

        function fncGenerarReporte() {
            let obj = new Object();
            obj = {
                lstTablaDatosBLActuales: arrTablaDatosBLActuales,
                lstTablaDatosBLPorc: arrTablaDatosBLPorc,
                lstTablaDatosBLTiempoPromedio: arrTablaDatosBLTiempoPromedio,
                lstTendenciaBLRegistrados: arrTendenciaBLRegistrados,
                lstTendenciaBLCerrados: arrTendenciaBLCerrados,
                lstTendenciaBLAcumulados: arrTendenciaBLAcumulados,
                grafica: imgGrafica
            }
            axios.post("GenerarReporte", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    var path = `/Reportes/Vista.aspx?idReporte=232`;
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

        //#region CREACIÓN DE REPORTE CON GRAFICA TOMADA COMO IMAGEN
        EXPORT_WIDTH = 1000;
        function fncGenerarReporteCrystalReport(chart, filename) {
            var render_width = EXPORT_WIDTH;
            var render_height = render_width * chart.chartHeight / chart.chartWidth

            var svg = chart.getSVG({
                exporting: {
                    sourceWidth: chart.chartWidth,
                    sourceHeight: chart.chartHeight
                }
            });

            var canvas = document.createElement('canvas');
            canvas.height = render_height;
            canvas.width = render_width;

            var image = new Image;
            image.onload = function () {
                canvas.getContext('2d').drawImage(this, 0, 0, render_width, render_height);
                var data = canvas.toDataURL("image/png")
                download(data, filename + '.png');
            };
            image.src = 'data:image/svg+xml;base64,' + window.btoa(svg);
        }

        function download(data, filename) {
            var a = document.createElement('a');
            document.body.appendChild(a);
            imgGrafica = data
        }
        //#endregion
    };

    $(document).ready(() => BackLogs = new BackLogs())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();