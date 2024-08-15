(function () {

    $.namespace('Maquinaria.Backlogs.BackLogsObra');

    //#region CONST
    const tblBacklogsEstatusObra = $("#tblBacklogsEstatusObra");
    const tblBacklogsEstatusObra_tbody = $("#tblBacklogsEstatusObra tbody");
    const tblGraficaLineasBL = $("#tblGraficaLineasBL");
    const tblGraficaLineasBL_tbody = $("#tblGraficaLineasBL tbody");
    const btnProgramaInspeccion = $("#btnProgramaInspeccion");
    const btnPresupuestoRehabilitacion = $("#btnPresupuestoRehabilitacion");
    const btnFrenteBackLogs = $('#btnFrenteBackLogs');
    const btnSeguimientoPresupuestos = $('#btnSeguimientoPresupuestos');
    const btnInicio = $("#btnInicio");
    const btnInformeRehabilitacion = $("#btnInformeRehabilitacion");
    const cboProyecto = $("#cboProyecto");
    const tblTablaDatos = $('#tblTablaDatos');
    const btnIndicadoresRehabilitacionTMC = $('#btnIndicadoresRehabilitacionTMC');
    let dtTablaDatos;

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

    let arrTendenciaBLRegistrados = [];
    let arrTendenciaBLCerrados = [];
    let arrTendenciaBLAcumulados = [];

    const divEstatusBackLogs = $("#divEstatusBackLogs");
    const divTendenciaBackLogs = $("#divTendenciaBackLogs");
    const divTablaDatos = $("#divTablaDatos");
    const btnInicioTMC = $('#btnInicioTMC');
    let rt;

    const mdlLstBackLogs = $('#mdlLstBackLogs');
    const tblListadoBL = $('#tblListadoBL');
    let dtBackLogs;
    //#endregion

    let MesActual;
    //#endregion

    BackLogs = function () {
        (function init() {
            fncListeners();
            fncLimpiarBackLogsEstatusObra();
            fncMostrarOcultar();
            initTablaDatos();
            cboProyecto.val("1010");
            cboProyecto.trigger("change");
        })();

        function fncListeners() {
            fncFillCboProyectosObra();

            tblGraficaLineasBL.hide();
            tblBacklogsEstatusObra.hide();

            //#region MENU
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
            btnInformeRehabilitacion.click(function (e) {
                document.location.href = '/BackLogs/InformeTMC?areaCuenta=' + cboProyecto.val();
            });
            btnFrenteBackLogs.click(function (e) {
                document.location.href = '/BackLogs/FrenteTMC?areaCuenta=' + cboProyecto.val();
            });
            btnIndicadoresRehabilitacionTMC.click(function (e) {
                document.location.href = '/BackLogs/IndicadoresRehabilitacionTMC?areaCuenta=' + cboProyecto.val();
            });
            //#endregion

            cboProyecto.change(function (e) {
                if (cboProyecto.val() != "") {
                    tblGraficaLineasBL.show();
                    tblBacklogsEstatusObra.show();
                    fncGetDate();
                    fncGetNumBackLogs();
                } else {
                    tblGraficaLineasBL.hide();
                    tblBacklogsEstatusObra.hide();
                }
                fncMostrarOcultar();
            });

            //#region SE OBTIENE LISTADO DE BACKLOGS EN BASE AL MES SELECCIONADO
            tdEneroBLAcumulados.on("click", function () {
                fncGetBackLogsGraficaIndex(1);
            });

            tdFebreroBLAcumulados.on("click", function () {
                fncGetBackLogsGraficaIndex(2);
            });

            tdMarzoBLAcumulados.on("click", function () {
                fncGetBackLogsGraficaIndex(3);
            });

            tdAbrilBLAcumulados.on("click", function () {
                fncGetBackLogsGraficaIndex(4);
            });

            tdMayoBLAcumulados.on("click", function () {
                fncGetBackLogsGraficaIndex(5);
            });

            tdJunioBLAcumulados.on("click", function () {
                fncGetBackLogsGraficaIndex(6);
            });

            tdJulioBLAcumulados.on("click", function () {
                fncGetBackLogsGraficaIndex(7);
            });

            tdAgostoBLAcumulados.on("click", function () {
                fncGetBackLogsGraficaIndex(8);
            });

            tdSeptiembreBLAcumulados.on("click", function () {
                fncGetBackLogsGraficaIndex(9);
            });

            tdOctubreBLAcumulados.on("click", function () {
                fncGetBackLogsGraficaIndex(10);
            });

            tdNoviembreBLAcumulados.on("click", function () {
                fncGetBackLogsGraficaIndex(11);
            });

            tdDiciembreBLAcumulados.on("click", function () {
                fncGetBackLogsGraficaIndex(12);
            });
            //#endregion
        }

        function fncGetBackLogsGraficaIndex(mes) {
            let obj = new Object();
            obj = {
                Mes: mes,
                areaCuenta: cboProyecto.val()
            }
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
            MesActual = mm;
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
            if (cboProyecto.val() != "") {
                let objFiltro = new Object();
                objFiltro = {
                    areaCuenta: cboProyecto.val()
                };
                axios.post("GetDatosGraficasBLTMC", objFiltro).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncLimpiarBackLogsEstatusObra();

                        //#region TABLA DE DATOS
                        tdNumBackLogs20.append(response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[0] > 0 ? response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[0] : parseFloat(0));
                        tdNumBackLogs40.append(response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[1] > 0 ? response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[1] : parseFloat(0));
                        tdNumBackLogs50.append(response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[2] > 0 ? response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[2] : parseFloat(0));
                        tdNumBackLogs60.append(response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[3] > 0 ? response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[3] : parseFloat(0));
                        tdNumBackLogs80.append(response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[4] > 0 ? response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[4] : parseFloat(0));
                        tdNumBackLogs90.append(response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[5] > 0 ? response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[5] : parseFloat(0));
                        tdNumBackLogs100.append(response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[6] > 0 ? response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[6] : parseFloat(0));
                        tdNumBackLogsTotal.append(response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[7] > 0 ? response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[7] : parseFloat(0));

                        estatus20 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[0] > 0) ?
                            ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[0] / response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[7]) * 100) : parseFloat(0);
                        tdPorBackLogs20.append(parseFloat(estatus20).toFixed(2) + "%");

                        estatus40 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[1] > 0) ?
                            ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[1] / response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[7]) * 100) : parseFloat(0);
                        tdPorBackLogs40.append(parseFloat(estatus40).toFixed(2) + "%");

                        estatus50 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[2] > 0) ?
                            ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[2] / response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[7]) * 100) : parseFloat(0);
                        tdPorBackLogs50.append(parseFloat(estatus50).toFixed(2) + "%");

                        estatus60 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[3] > 0) ?
                            ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[3] / response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[7]) * 100) : parseFloat(0);
                        tdPorBackLogs60.append(parseFloat(estatus60).toFixed(2) + "%");

                        estatus80 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[4] > 0) ?
                            ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[4] / response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[7]) * 100) : parseFloat(0);
                        tdPorBackLogs80.append(parseFloat(estatus80).toFixed(2) + "%");

                        estatus90 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[5] > 0) ?
                            ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[5] / response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[7]) * 100) : parseFloat(0);
                        tdPorBackLogs90.append(parseFloat(estatus90).toFixed(2) + "%");

                        estatus100 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[6] > 0) ?
                            ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[6] / response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[7]) * 100) : parseFloat(0);
                        tdPorBackLogs100.append(parseFloat(estatus100).toFixed(2) + "%");


                        // r1 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[0] > 0) ?
                        //     ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[0] + response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[0])) / 2 : parseFloat(0);
                        // diasPromBacklogs1.append(parseFloat(r1));

                        // r2 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[1] > 0) ?
                        //     ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[1] + response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[1])) / 2 : parseFloat(0);
                        // diasPromBacklogs2.append(parseFloat(r2));

                        // r3 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[2] > 0) ?
                        //     ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[2] + response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[2])) / 2 : parseFloat(0);
                        // diasPromBacklogs3.append(parseFloat(r3));

                        // r4 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[3] > 0) ?
                        //     ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[3] + response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[3])) / 2 : parseFloat(0);
                        // diasPromBacklogs4.append(parseFloat(r4));

                        // r5 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[4] > 0) ?
                        //     ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[4] + response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[4])) / 2 : parseFloat(0);
                        // diasPromBacklogs5.append(parseFloat(r5));

                        // r6 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[5] > 0) ?
                        //     ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[5] + response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[5])) / 2 : parseFloat(0);
                        // diasPromBacklogs6.append(parseFloat(r6));

                        // r7 = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[6] > 0) ?
                        //     ((response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[6] + response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[6])) / 2 : parseFloat(0);
                        // diasPromBacklogs7.append(parseFloat(r7));

                        // rt = (response.data.resultadosCantEstatusTMC.lstCantEstatusTMC[7] > 0) ?
                        //     (r1 + r2 + r3 + r4 + r5 + r6 + r7) : parseFloat(0);

                        r1 = diasPromBacklogs1.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[0]));
                        r2 = diasPromBacklogs2.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[1]));
                        r3 = diasPromBacklogs3.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[2]));
                        r4 = diasPromBacklogs4.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[3]));
                        r5 = diasPromBacklogs5.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[4]));
                        r6 = diasPromBacklogs6.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[5]));
                        r7 = diasPromBacklogs7.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[6]));
                        rt = tddiasPromTotal.append(parseFloat(response.data.resultadosTiempoPromedio.lstTiempoPromedio[7]));
                        tddiasPromTotal.append(parseFloat(rt));
                        //#endregion

                        //#region TABLA INFERIOR DE GRAFICA DE LINEAS
                        //#region BL REGISTRADOS
                        tdEneroBLRegistrados.append(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[0] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[0] : parseFloat(0));
                        tdFebreroBLRegistrados.append(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[1] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[1] : parseFloat(0));
                        tdMarzoBLRegistrados.append(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[2] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[2] : parseFloat(0));
                        tdAbrilBLRegistrados.append(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[3] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[3] : parseFloat(0));
                        tdMayoBLRegistrados.append(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[4] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[4] : parseFloat(0));
                        tdJunioBLRegistrados.append(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[5] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[5] : parseFloat(0));
                        tdJulioBLRegistrados.append(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[6] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[6] : parseFloat(0));
                        tdAgostoBLRegistrados.append(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[7] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[7] : parseFloat(0));
                        tdSeptiembreBLRegistrados.append(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[8] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[8] : parseFloat(0));
                        tdOctubreBLRegistrados.append(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[9] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[9] : parseFloat(0));
                        tdNoviembreBLRegistrados.append(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[10] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[10] : parseFloat(0));
                        tdDiciembreBLRegistrados.append(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[11] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[11] : parseFloat(0));

                        //#region SE CONSTRUYE ARREGLO CON INFORMACIÓN DE: TENDENCIA BL REGISTRADOS
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[0] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[0] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[1] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[1] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[2] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[2] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[3] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[3] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[4] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[4] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[5] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[5] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[6] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[6] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[7] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[7] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[8] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[8] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[9] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[9] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[10] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[10] : parseFloat(0));
                        arrTendenciaBLRegistrados.push(response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[11] > 0 ? response.data.resultadosCantEstatusLineasTMC.lstcontadorTMC[11] : parseFloat(0));
                        //#endregion

                        //#endregion

                        //#region BL CERRADOS
                        tdEneroBLCerrados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[0] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[0] : parseFloat(0));
                        tdFebreroBLCerrados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[1] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[1] : parseFloat(0));
                        tdMarzoBLCerrados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[2] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[2] : parseFloat(0));
                        tdAbrilBLCerrados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[3] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[3] : parseFloat(0));
                        tdMayoBLCerrados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[4] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[4] : parseFloat(0));
                        tdJunioBLCerrados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[5] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[5] : parseFloat(0));
                        tdJulioBLCerrados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[6] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[6] : parseFloat(0));
                        tdAgostoBLCerrados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[7] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[7] : parseFloat(0));
                        tdSeptiembreBLCerrados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[8] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[8] : parseFloat(0));
                        tdOctubreBLCerrados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[9] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[9] : parseFloat(0));
                        tdNoviembreBLCerrados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[10] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[10] : parseFloat(0));
                        tdDiciembreBLCerrados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[11] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[11] : parseFloat(0));

                        //#region SE CONSTRUYE ARREGLO CON INFORMACIÓN DE: TENDENCIA BL CERRADOS
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[0] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[0] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[1] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[1] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[2] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[2] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[3] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[3] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[4] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[4] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[5] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[5] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[6] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[6] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[7] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[7] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[8] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[8] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[9] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[9] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[10] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[10] : parseFloat(0));
                        arrTendenciaBLCerrados.push(response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[11] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorInstaladosTMC[11] : parseFloat(0));
                        //#endregion

                        //#endregion

                        //#region BL ACUMULADOS
                        tdEneroBLAcumulados.append(response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[0] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[0] : parseFloat(0));
                        tdFebreroBLAcumulados.append(MesActual >= 2 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[1] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[1] : parseFloat(0) : 0);
                        tdMarzoBLAcumulados.append(MesActual >= 3 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[2] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[2] : parseFloat(0) : 0);
                        tdAbrilBLAcumulados.append(MesActual >= 4 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[3] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[3] : parseFloat(0) : 0);
                        tdMayoBLAcumulados.append(MesActual >= 5 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[4] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[4] : parseFloat(0) : 0);
                        tdJunioBLAcumulados.append(MesActual >= 6 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[5] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[5] : parseFloat(0) : 0);
                        tdJulioBLAcumulados.append(MesActual >= 7 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[6] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[6] : parseFloat(0) : 0);
                        tdAgostoBLAcumulados.append(MesActual >= 8 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[7] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[7] : parseFloat(0) : 0);
                        tdSeptiembreBLAcumulados.append(MesActual >= 9 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[8] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[8] : parseFloat(0) : 0);
                        tdOctubreBLAcumulados.append(MesActual >= 10 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[9] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[9] : parseFloat(0) : 0);
                        tdNoviembreBLAcumulados.append(MesActual >= 11 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[10] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[10] : parseFloat(0) : 0);
                        tdDiciembreBLAcumulados.append(MesActual >= 12 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[11] > 0 ? response.data.resultadosCantEstatusLineasTMC.IstContadorAcumuladosTMC[11] : parseFloat(0) : 0);

                        fncCargarGraficaPastel();
                        fncCargarGraficaLineas();
                        tblBacklogsEstatusObra.show();
                    } else {
                        Alert2Error(response.message);
                    }
                }).catch(error => Alert2Error(error.message));
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
                            name: 'Elaboración de presupuesto (20%)',
                            color: "#ff3838",
                            y: estatus20,
                            sliced: true,
                            selected: true
                        }, {
                            name: 'Autorización de Presupuesto (40%)',
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
                            name: 'Rehabilitación Programada (80%)',
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
    }

    $(document).ready(() => BackLogs = new BackLogs())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();