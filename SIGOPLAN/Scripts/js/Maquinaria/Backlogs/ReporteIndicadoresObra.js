(function () {
    $.namespace('Maquinaria.Backlogs.ReporteIndicadoresObra');

    //#region CONST FILTROS
    const divTbl1 = $('#divTbl1');
    // const cboFiltroMesTbl1 = $('#cboFiltroMesTbl1');
    const txtFiltroFechaInicioTbl1 = $('#txtFiltroFechaInicioTbl1');
    const txtFiltroFechaFinTbl1 = $('#txtFiltroFechaFinTbl1');
    const cboFiltroResponsableTbl1 = $('#cboFiltroResponsableTbl1');
    const btnFiltroBuscarTbl1 = $('#btnFiltroBuscarTbl1');
    const btnFiltroLimpiarTbl1 = $('#btnFiltroLimpiarTbl1');

    const divTbl2 = $('#divTbl2');
    const txtFiltroFechaInicioTbl2 = $('#txtFiltroFechaInicioTbl2');
    const txtFiltroFechaFinTbl2 = $('#txtFiltroFechaFinTbl2');
    const cboFiltroTipoMaquinaTbl2 = $('#cboFiltroTipoMaquinaTbl2');
    const cboFiltroEstatusTbl2 = $('#cboFiltroEstatusTbl2');
    const btnFiltroBuscarTbl2 = $('#btnFiltroBuscarTbl2');
    const btnFiltroLimpiarTbl2 = $('#btnFiltroLimpiarTbl2');

    const divTbl3 = $('#divTbl3');
    const cboFiltroMesTbl3 = $('#cboFiltroMesTbl3');
    const cboFiltroAnioTbl3 = $('#cboFiltroAnioTbl3');
    const cboFiltroTipoMaquinaTbl3 = $('#cboFiltroTipoMaquinaTbl3');
    const cboFiltroMotivoTbl3 = $('#cboFiltroMotivoTbl3');
    const cboFiltroEstatusTbl3 = $('#cboFiltroEstatusTbl3');
    const btnFiltroBuscarTbl3 = $('#btnFiltroBuscarTbl3');
    const btnFiltroLimpiarTbl3 = $('#btnFiltroLimpiarTbl3');

    const cboProyecto = $('#cboProyecto');
    //#endregion

    //#region CONST TAB MENU
    const tabMenuTbl1 = $('#tabMenuTbl1');
    const tabMenuTbl2 = $('#tabMenuTbl2');
    const tabMenuTbl3 = $('#tabMenuTbl3');
    const tblPorResponsable = $('#tblPorResponsable');
    const tblPorEquipo = $('#tblPorEquipo');
    const tblPorFolio = $('#tblPorFolio');
    const tabTbl1 = $('#tabTbl1');
    const tabTbl2 = $('#tabTbl2');
    const tabTbl3 = $('#tabTbl3');
    let dtPorResponsable;
    let dtPorEquipo;
    let dtPorFolio;
    let dtPorEquipoDet;
    const GraficaReportes = $('#GraficaReportes');

    // const txtFiltroFechaInicioTbl1 = $('#txtFiltroFechaInicioTbl1');
    // const txtFiltroFechaFinTbl1 = $('#txtFiltroFechaFinTbl1');
    //#endregion

    //#region CONST GRAFICAS TBL3
    const btnGraficaResponsables = $('#btnGraficaResponsables');
    const btnGraficaConjuntos = $('#btnGraficaConjuntos');
    const btnGraficaDias = $('#btnGraficaDias');
    //#endregion

    //#region CONST DESCRIPCIÓN BL
    const mdlDescripcionBL = $('#mdlDescripcionBL');
    const txtDescripcionBL = $('#txtDescripcionBL');
    //#endregion

    const añoActual = new Date().getFullYear();
    const mesActual = new Date().getMonth();
    const ultimoDiaDelMes = moment(new Date(añoActual, mesActual, 1)).endOf('month').format('DD');

    const divGraficaEquipoCostoHora = $('#divGraficaEquipoCostoHora');
    const divGraficaEquipoCostoMes = $('#divGraficaEquipoCostoMes');
    const chbTipoGraficaPorEquipo = $('#chbTipoGraficaPorEquipo');
    const modalDetallePorEquipo = $('#modalDetallePorEquipo');
    const tblPorEquipoDet = $('#tblPorEquipoDet');

    //#region CONST OCULTOS
    const inputEmpresaActual = $('#inputEmpresaActual');
    _empresaActual = +inputEmpresaActual.val();
    //#endregion

    BackLogs = function () {
        (function init() {
            GraficaReportes.show();
            fncListeners();
            fncInicializarDatePickers();
            obtenerUrlPArams();
        })();

        function fncListeners() {
            initTblResponsable();
            initTblEquipo();
            initTblFolio();
            initTblEquipoDet();

            fncHabilitarDeshabilitarCtrls(true);

            $("#divGrReponsable").css("display", "none");
            $("#divGrConjuntos").css("display", "none");
            $("#divGrDias").css("display", "none");

            cboProyecto.change(function () {
                cboFiltroResponsableTbl1.attr("multiple", true);
                cboFiltroResponsableTbl1.fillCombo("/BackLogs/FillCboResponsablesAnalisisBLResponsable",
                    { areaCuenta: $(this).val() }, true, "TODOS", () => convertToMultiselectSelectAll(cboFiltroResponsableTbl1));
            });

            btnGraficaResponsables.click(function (e) {
                fncGraficaReporteResposables();
                $("#divGrReponsable").css("display", "block");
                $("#divGrConjuntos").css("display", "none");
                $("#divGrDias").css("display", "none");
            });
            btnGraficaConjuntos.click(function (e) {
                fncGetGraficaConjuntos();
                $("#divGrReponsable").css("display", "none");
                $("#divGrConjuntos").css("display", "block");
                $("#divGrDias").css("display", "none");
            });
            btnGraficaDias.click(function (e) {
                fncGetGraficaBLDias();
                $("#divGrReponsable").css("display", "none");
                $("#divGrConjuntos").css("display", "none");
                $("#divGrDias").css("display", "block");
            });

            $("#btnInicioObra").click(function (e) {
                document.location.href = '/BackLogs/Index?areaCuenta=' + cboProyecto.val();
            });
            $("#btnProgramaInspeccion").click(function (e) {
                document.location.href = '/BackLogs/ProgramaInspeccionBackLogs?areaCuenta=' + cboProyecto.val();
            })
            $("#btnRegistroBackLogs").click(function (e) {
                document.location.href = '/BackLogs/RegistroBackLogsObra?areaCuenta=' + cboProyecto.val();
            });
            $("#btnInformeRehabilitacion").click(function (e) {
                document.location.href = '/BackLogs/InformeBackLogsRehabilitacion?areaCuenta=' + cboProyecto.val();
            });

            //#region TAB CLICK MENUS
            tabMenuTbl1.click(function (e) {
                divTbl1.parent().css("display", "block");
                divTbl2.parent().css("display", "none");
                divTbl3.parent().css("display", "none");

                tabTbl1.css("display", "block");
                tabTbl2.css("display", "none");
                tabTbl3.css("display", "none");
            });

            tabMenuTbl2.click(function (e) {
                divTbl1.parent().css("display", "none");
                divTbl2.parent().css("display", "block");
                divTbl3.parent().css("display", "none");

                tabTbl1.css("display", "none");
                tabTbl2.css("display", "block");
                tabTbl3.css("display", "none");
            });

            tabMenuTbl3.click(function (e) {
                divTbl1.parent().css("display", "none");
                divTbl2.parent().css("display", "none");
                divTbl3.parent().css("display", "block");

                tabTbl1.css("display", "none");
                tabTbl2.css("display", "none");
                tabTbl3.css("display", "block");
            });
            tabMenuTbl1.trigger("click");
            //#endregion

            //#region FILL CBOS
            fncFillCbos();
            //#endregion

            //#region SE OBTIENE LOS RESULTADOS DE LOS REPORTES E INDICADORES
            btnFiltroBuscarTbl1.click(function (e) {
                fncGetReporteIndicadoresPorReponsables();
                ObtenerGraficas();
            });
            btnFiltroBuscarTbl2.click(function (e) {
                fncGetReporteIndicadorPorEquipo();
            });
            btnFiltroBuscarTbl3.click(function (e) { // TODO
                fncGetReporteIndicadores();
                // fncGraficaReporteResposables();
                // fncGetGraficaConjuntos();
                // fncGetGraficaBLDias();
            });
            //#endregion

            //#region SE LIMPIA LOS FILTROS
            btnFiltroLimpiarTbl1.click(function (e) {
                txtFiltroFechaInicioTbl1.datepicker('setDate', new Date(añoActual, mesActual, 1));
                txtFiltroFechaFinTbl1.datepicker('setDate', new Date(añoActual, mesActual, ultimoDiaDelMes));
                // cboFiltroMesTbl1[0].selectedIndex = 0;
                cboFiltroResponsableTbl1[0].selectedIndex = 0;
            });

            btnFiltroLimpiarTbl2.click(function (e) {
                txtFiltroFechaInicioTbl2.datepicker('setDate', new Date(añoActual, mesActual, 1));
                txtFiltroFechaFinTbl2.datepicker('setDate', new Date(añoActual, mesActual, ultimoDiaDelMes));
                cboFiltroTipoMaquinaTbl2[0].selectedIndex = 0;
                cboFiltroEstatusTbl2[0].selectedIndex = 0;
            });

            btnFiltroLimpiarTbl3.click(function (e) {
                // cboFiltroAnioTbl3[0].selectedIndex = 0;
                // cboFiltroMesTbl3[0].selectedIndex = 0;
                // cboFiltroTipoMaquinaTbl3[0].selectedIndex = 0;
                // cboFiltroMotivoTbl3[0].selectedIndex = 0;
                // cboFiltroEstatusTbl3[0].selectedIndex = 0;
            });
            //#endregion
            chbTipoGraficaPorEquipo.change(function (e) {
                var tipoGraficaPorEquipo = $(this).prop('checked');
                if (tipoGraficaPorEquipo) {
                    divGraficaEquipoCostoHora.css('display', 'block');
                    divGraficaEquipoCostoMes.css('display', 'none');
                }
                else {
                    divGraficaEquipoCostoHora.css('display', 'none');
                    divGraficaEquipoCostoMes.css('display', 'block');
                }
            });

            cboProyecto.on("change", function () {
                if ($(this).val() != "") {
                    fncHabilitarDeshabilitarCtrls(false);
                } else {
                    fncHabilitarDeshabilitarCtrls(true);
                }
            });
        }

        function fncHabilitarDeshabilitarCtrls(disabled) {
            txtFiltroFechaInicioTbl1.attr("disabled", disabled);
            txtFiltroFechaFinTbl1.attr("disabled", disabled);
            // cboFiltroResponsableTbl1.attr("disabled", disabled);
            btnFiltroBuscarTbl1.attr("disabled", disabled);
            btnFiltroLimpiarTbl1.attr("disabled", disabled);
            txtFiltroFechaInicioTbl2.attr("disabled", disabled);
            txtFiltroFechaFinTbl2.attr("disabled", disabled);
            cboFiltroTipoMaquinaTbl2.attr("disabled", disabled);
            cboFiltroEstatusTbl2.attr("disabled", disabled);
            btnFiltroBuscarTbl2.attr("disabled", disabled);
            // cboFiltroMesTbl3.attr("disabled", disabled);
            cboFiltroTipoMaquinaTbl3.attr("disabled", disabled);
            cboFiltroMotivoTbl3.attr("disabled", disabled);
            // cboFiltroEstatusTbl3.attr("disabled", disabled);
            btnFiltroBuscarTbl3.attr("disabled", disabled);
            btnFiltroLimpiarTbl3.attr("disabled", disabled);
            // chbTipoGraficaPorEquipo.attr("disabled", disabled);
            btnGraficaResponsables.attr("disabled", disabled);
            btnGraficaConjuntos.attr("disabled", disabled);
            btnGraficaDias.attr("disabled", disabled);
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

        function fncFillCbos() {
            if (_empresaActual == 6) {
                cboProyecto.fillCombo("FillCboAC", {}, false);
            } else {
                cboProyecto.fillCombo("/CatObra/cboCentroCostosUsuarios", {}, false);
            }
            cboProyecto.select2({ width: "resolve" });

            //#region FILTROS TBL1
            // cboFiltroResponsableTbl1.select2();
            //#endregion

            //#region FILTROS TBL2
            cboFiltroTipoMaquinaTbl2.fillCombo("/BackLogs/FillTipoMaquinariaTMC", {}, false);
            cboFiltroTipoMaquinaTbl2.select2();
            cboFiltroTipoMaquinaTbl2.select2({ width: "100%" });
            cboFiltroEstatusTbl2.select2();
            cboFiltroEstatusTbl2.select2({ width: "100%" });
            //#endregion

            //#region FILTROS tbl3
            cboFiltroTipoMaquinaTbl3.fillCombo("/BackLogs/FillTipoMaquinariaTMC", {}, false);
            cboFiltroTipoMaquinaTbl3.select2();
            cboFiltroTipoMaquinaTbl3.select2({ width: "100%" });
            // cboFiltroMesTbl3.select2();
            // cboFiltroMesTbl3.select2({ width: "100%" });
            cboFiltroMesTbl3.attr("multiple", true);
            convertToMultiselectSelectAll(cboFiltroMesTbl3);
            cboFiltroMotivoTbl3.select2();
            cboFiltroMotivoTbl3.select2({ width: "100%" });
            // cboFiltroEstatusTbl3.select2();
            // cboFiltroEstatusTbl3.select2({ width: "100%" });
            cboFiltroEstatusTbl3.attr("multiple", true);
            convertToMultiselectSelectAll(cboFiltroEstatusTbl3);
            //#endregion
        }

        function fncInicializarDatePickers() {
            txtFiltroFechaInicioTbl2.datepicker({
                dateFormat: "dd/mm/yy",
            }).datepicker('setDate', new Date(añoActual, mesActual, 1));
            txtFiltroFechaFinTbl2.datepicker({
                dateFormat: "dd/mm/yy",
            }).datepicker('setDate', new Date(añoActual, mesActual, ultimoDiaDelMes));

            txtFiltroFechaInicioTbl1.datepicker({
                dateFormat: "dd/mm/yy",
            }).datepicker('setDate', new Date(añoActual, mesActual, 1));
            txtFiltroFechaFinTbl1.datepicker({
                dateFormat: "dd/mm/yy",
            }).datepicker('setDate', new Date(añoActual, mesActual, ultimoDiaDelMes));
        }

        function initTblResponsable() {
            dtPorResponsable = tblPorResponsable.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'responsable', title: 'Responsable' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'top1Conjunto', title: 'Top 1' },
                    { data: 'top2Conjunto', title: 'Top 2' },
                    { data: 'top3Conjunto', title: 'Top 3' },
                    { data: 'cantBL50OMenos', title: '50%<br>o<br>menos' },
                    { data: 'cantBL70', title: '60%<br>a<br>90%' },
                    { data: 'cantBL100', title: '100%' },
                    { data: 'cantBLTotal', title: 'Total' },
                    { data: 'tiempoProm100', title: 'Tiempo promedio para<br>llegar a 100% (Días)' }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTblEquipo() {
            dtPorEquipo = tblPorEquipo.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: "noEconomico", title: 'Económico' },
                    { data: "descripcion", title: 'Descripción' },
                    { data: "modelo", title: 'Modelo' },
                    { data: "horas", title: 'Horas' },
                    { data: "estatus", title: 'Estatus' },
                    {
                        title: 'Fecha creación<br>de BL más<br>antiguo < 100%',
                        render: function (data, type, row) {
                            if (row.fechaUltimoBL == null) return '--';
                            else return moment(row.fechaUltimoBL).format('DD/MM/YYYY');
                        }
                    },
                    { data: "cantidadBL", title: 'Cant. BL' },
                    {
                        data: "strPresupuestoMes", title: 'Costo Total<br>del BL (MXN)',
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: "strPresupuestoAcumulado", title: 'Costo Total<br>Acumulado de<br>Backlogs (M.N.)',
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: "costoHora", title: 'Costo/Hora',
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        title: 'Detalle',
                        render: function (data, type, row) {
                            return '<button class="btn btn-warning btn-xs botonDetalleTblEquipo"><i class="fa fa-info-circle" aria-hidden="true"></i></button>';
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblPorEquipo.on('click', '.botonDetalleTblEquipo', function () {
                        let rowData = dtPorEquipo.row($(this).closest('tr')).data();
                        // dtPorEquipoDet.clear();
                        // dtPorEquipoDet.rows.add(rowData.backlogs);
                        // dtPorEquipoDet.draw();
                        fncGetPorEquipoDet(rowData.noEconomico);
                        modalDetallePorEquipo.modal('show');
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': [0, 1, 2, 3, 4, 5, 6] },
                    { className: 'dt-right', 'targets': [7, 8, 9] }
                ],
            });
        }

        function initTblFolio() {
            dtPorFolio = tblPorFolio.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: "folioBL", title: 'Folio' },
                    {
                        title: 'Descripción',
                        render: function (data, type, row, meta) {
                            return `<button title="${row.descripcionBL}" class="btn btn-xs btn-primary botonDescripcion"><i class="fa fa-align-justify"></i></button>`;
                        }
                    },
                    { data: "noEconomico", title: 'Económico' },
                    {
                        title: "Conjunto", render: (data, type, row, meta) => {
                            return `<p title="${row.conjuntoAbreviacion}">${row.conjunto}</p>`
                        }
                    },
                    { data: "estatus", title: 'Estatus' },
                    {
                        data: "fechaCreacionBL", title: 'Fecha<br>creación de BL',
                        render: function (data, type, row) {
                            let fechaCreacionBL = moment(data).format("DD/MM/YYYY");
                            if (fechaCreacionBL == "01/01/2000") {
                                return "-";
                            } else {
                                return fechaCreacionBL;
                            }
                        }
                    },
                    {
                        data: "fechaInstaladoBL", title: 'Fecha<br>instalado BL',
                        render: function (data, type, row) {
                            if (row.idEstatus != 7) {
                                return "--";
                            } else {
                                return moment(data).format("DD/MM/YYYY");
                            }
                        }
                    },
                    { data: "diasTotales", title: 'Tiempo<br>transcurrido' },
                    { data: "strCostoTotalBL", title: 'Costo total<br>del BL' },
                    { data: "responsable", title: 'Responsable' }
                ],
                initComplete: function (settings, json) {
                    tblPorFolio.on('click', '.botonDescripcion', function () {
                        let rowData = dtPorFolio.row($(this).closest('tr')).data();
                        txtDescripcionBL.val(rowData.descripcionBL);
                        mdlDescripcionBL.modal('show');
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetReporteIndicadores() {
            let arrMeses = new Array();
            cboFiltroMesTbl3.val().forEach(element => {
                arrMeses.push(element);
            });
            let arrEstatus = new Array();
            cboFiltroEstatusTbl3.val().forEach(element => {
                arrEstatus.push(element);
            });
            axios.post("GetReporteIndicadores", { anio: cboFiltroAnioTbl3.val(), areaCuenta: cboProyecto.val(), lstMeses: arrMeses, lstEstatus: arrEstatus }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL FOLIOS
                    dtPorFolio.clear();
                    dtPorFolio.rows.add(items);
                    dtPorFolio.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetReporteIndicadoresPorReponsables() {
            let arrUsuarios = new Array();
            cboFiltroResponsableTbl1.val().forEach(element => {
                arrUsuarios.push(element);
            });
            let objFiltro = new Object();
            objFiltro = {
                areaCuenta: cboProyecto.val(),
                lstUsuarios: arrUsuarios
            }
            let fechaInicio = txtFiltroFechaInicioTbl1.val();
            let fechaFin = txtFiltroFechaFinTbl1.val();
            axios.post("GetReportePorResponsables", { objFiltro, fechaInicio, fechaFin }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region 
                    dtPorResponsable.clear();
                    dtPorResponsable.rows.add(response.data.lstResponsables);
                    dtPorResponsable.draw();
                    //#endregion
                } else {
                    dtPorResponsable.clear();
                    dtPorResponsable.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetReporteIndicadorPorEquipo() {
            let AreaCuenta = cboProyecto.val();
            let fechaInicio = txtFiltroFechaInicioTbl2.val();
            let fechaFin = txtFiltroFechaFinTbl2.val();
            let tipoEquipo = cboFiltroTipoMaquinaTbl2.val();
            let estatus = cboFiltroEstatusTbl2.val();
            axios.post("GetIndicadorBacklogPorEquipo", { AreaCuenta, fechaInicio, fechaFin, tipoEquipo, estatus }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL EQUIPOS
                    dtPorEquipo.clear();
                    dtPorEquipo.rows.add(response.data.lstEquipos);
                    dtPorEquipo.draw();

                    fncGraficaPorEquipoCostoHora(response.data.catGraficaPorEquipo, response.data.dataGraficaCostoHora);
                    fncGraficaPorEquipoCostoMes(response.data.catGraficaPorEquipo, response.data.dataGraficaCostoMes);

                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGraficaPorEquipoCostoHora(categorias, data) {
            Highcharts.chart('graficaEquipoCostoHora', {
                chart: {
                    type: 'column'
                },
                title: { text: '' },
                xAxis: { categories: categorias },
                yAxis: { title: { text: '' } },

                plotOptions: {
                    series: { label: { connectorAllowed: false } }
                },
                series: [{
                    showInLegend: false,
                    name: 'Costo Hora',
                    data: data
                },],
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

        function fncGraficaPorEquipoCostoMes(categorias, data) {
            Highcharts.chart('graficaEquipoCostoMes', {
                chart: {
                    type: 'column'
                },
                title: { text: '' },
                xAxis: { categories: categorias },
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
                    showInLegend: false,
                    name: 'Costo Mensual',
                    data: data
                },],
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

        function fncGraficaResposable(categories, data, meta) {
            Highcharts.chart('GrReponsable', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Cant. de BL por responsable'
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                xAxis: {
                    categories: categories == null ? "" : categories,
                    // categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                    crosshair: true
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                series: [{
                    type: 'column',
                    name: "Cant. BL",
                    data: data == null ? "" : data
                }
                ],
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                align: 'center',
                                verticalAlign: 'bottom',
                                layout: 'horizontal'
                            },
                            yAxis: {
                                labels: {
                                    align: 'left',
                                    x: 0,
                                    y: -5
                                },
                                title: {
                                    text: null
                                }
                            },
                            subtitle: {
                                text: null
                            },
                            credits: {
                                enabled: false
                            }
                        }
                    }]
                },
                credits: {
                    enabled: false
                }
            });
        }

        function fncGraficaReporteResposables() {
            let objParamsDTO = fncGetOBJGraficaResponsables();
            if (objParamsDTO != "") {
                axios.post("GetInfoGraficaResponsables", objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGraficaResposable(response.data.lstResponsables, response.data.lstCantBL);
                    }
                    else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetOBJGraficaResponsables() {
            let arrMeses = [];
            cboFiltroMesTbl3.val().forEach(element => {
                arrMeses.push(element);
            });

            let arrEstatus = [];
            cboFiltroEstatusTbl3.val().forEach(element => {
                arrEstatus.push(element);
            });

            let obj = {};
            obj.areaCuenta = cboProyecto.val();
            obj.anio = cboFiltroAnioTbl3.val();
            obj.lstMeses = arrMeses;
            obj.lstEstatus = arrEstatus;
            return obj;
        }

        function ObtenerGraficas() {
            let fechaInicio = txtFiltroFechaInicioTbl1.val();
            let fechaFin = txtFiltroFechaFinTbl1.val();
            $.post('/BackLogs/GetBackLogsGraficaresponsable', {
                inicioMes: fechaInicio.split('/')[1],
                finMes: fechaFin.split('/')[1],
                areaCuenta: cboProyecto.val(),
                _lstResponsables: cboFiltroResponsableTbl1.val(),
                inicioAnio: fechaInicio.split('/')[2],
                finAnio: fechaFin.split('/')[2]
            }).then(response => {
                if (response) {
                    initGraficaReportes('GraficaReportes', response, null, null);
                } else {
                    AlertaGeneral(`Alerta`, 'Ocurrió un error');
                }
            });

            // let obj = new Object();
            // obj = {
            //     inicioMes: fechaInicio.split('/')[1],
            //     finMes: fechaFin.split('/')[1],
            //     areaCuenta: cboProyecto.val(),
            //     _lstResponsables: cboFiltroResponsableTbl1.length > 0 ? cboFiltroResponsableTbl1.val() : 0
            // }
            // console.log(cboFiltroResponsableTbl1.val());
            // axios.post("GetBackLogsGraficaresponsable", obj).then(response => {
            //     let { success, items, message } = response.data;
            //     if (success) {
            //         console.log(response);
            //         initGraficaReportes('GraficaReportes', response.data.gpxBarra[0], null, null);
            //     } else {
            //         Alert2Error(message);
            //     }
            // }).catch(error => Alert2Error(error.message));
        }

        function initGraficaReportes(grafica, datos, callback, items) {
            Highcharts.chart(grafica, {
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                accessibility: {
                    announceNewData: {
                        enabled: true
                    }
                },
                xAxis: {
                    type: 'category'
                },
                yAxis: {
                    title: {
                        text: 'Total'
                    }
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            // format: '{point.y:.1f}'
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
                },
                series: datos.gpxBarra,
                credits: {
                    enabled: false
                },
            });
        }

        function initTblEquipoDet() {
            dtPorEquipoDet = tblPorEquipoDet.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: "noEconomico", title: "Económico" },
                    { data: "folioBL", title: "Folio" },
                    {
                        data: "descripcion", title: "Descripción",
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary btn-xs verDescripcionBL" title="Descripción del BL.">Descripción BL</button>`;
                        }
                    },
                    {
                        title: "Fecha<br>inspección",
                        render: function (data, type, row) {
                            return moment(row.fechaInspeccion).format('DD/MM/YYYY');
                        }
                    },
                    { data: "conjunto", title: "Conjunto" },
                    { data: "strCostoTotalBL", title: 'Total MX' },
                    { data: "estatus", title: "Estatus" },
                    {
                        data: "fechaInstaladoBL", title: "Fecha<br>instalado",
                        render: function (data, type, row) {
                            let fechaCierre = "";
                            if (row.idEstatus == 7) {
                                fechaCierre = moment(row.fechaInstaladoBL).format("DD/MM/YYYY");
                            } else {
                                fechaCierre = "--";
                            }
                            return fechaCierre
                        }
                    },
                    { data: "diasTotales", title: "Días<br>totales" }
                ],
                initComplete: function (settings, json) {
                    // tblPorEquipoDet.on('draw.dt', function () {
                    //     fncColorearCeldaEstatus();
                    // });

                    tblPorEquipoDet.on("click", ".verDescripcionBL", function () {
                        const rowData = dtPorEquipoDet.row($(this).closest("tr")).data();
                        txtDescripcionBL.val(rowData.descripcionBL);
                        mdlDescripcionBL.modal("show");
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                createdRow: function (row, data, index) {
                    if (data[6] == "100%") {
                        $("td", row).eq(6).addClass("boton-estatus-20");
                    }
                }
            });
        }

        function fncGetPorEquipoDet(noEconomico) {
            if (cboProyecto.val() != "" && noEconomico != "") {
                let obj = {};
                obj.noEconomico = noEconomico;
                obj.areaCuenta = cboProyecto.val();
                axios.post("GetTotalInfoEconomicoBL", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtPorEquipoDet.clear();
                        dtPorEquipoDet.rows.add(items);
                        dtPorEquipoDet.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (cboProyecto.val() == "" || cboProyecto.val() == undefined) {
                    Alert2Warning("Es necesario seleccionar un área cuenta.");
                }
            }
        }

        function fncColorearCeldaEstatus() {
            tblPorEquipoDet.find("tbody tr").each(function (index) {
                $(this).addClass("rowHover");
                $(this).children("td").each(function (index2) {
                    if (index2 == 6) {
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

        function fncGraficaConjuntos(lstConjuntos) {
            Highcharts.chart('graficaConjuntos', {
                chart: {
                    type: 'pie'
                },
                title: { text: 'Conjuntos utilizados' },
                yAxis: { title: { text: '' } },
                plotOptions: {
                    series: { label: { connectorAllowed: false } }
                },
                series: [{
                    showInLegend: false,
                    name: '',
                    data: lstConjuntos
                },],
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

        function fncGraficaBLDias(data) {
            Highcharts.chart('graficaBLDias', {
                chart: {
                    type: 'pie'
                },
                title: { text: 'BL duración en días' },
                yAxis: { title: { text: '' } },
                plotOptions: {
                    series: { label: { connectorAllowed: false } }
                },
                series: [{
                    showInLegend: false,
                    name: '',
                    data: data
                },],
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

        function fncGetGraficaConjuntos() {
            let objParamsDTO = fncGetOBJGraficaResponsables();
            if (objParamsDTO != "") {
                axios.post("GetGraficaConjuntos", objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGraficaConjuntos(response.data.lstConjuntos);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetGraficaBLDias() {
            let objParamsDTO = fncGetOBJGraficaResponsables();
            axios.post("GetGraficaBLDias", objParamsDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let arrData = [];
                    for (let i = 0; i < response.data.lstGraficaDias.length; i++) {
                        let obj = new Object();
                        switch (i) {
                            case 0:
                                obj = {
                                    name: "0 a 40",
                                    y: parseFloat(response.data.lstGraficaDias[i]),
                                    color: "green"
                                }
                                break;
                            case 1:
                                obj = {
                                    name: "41 a 60",
                                    y: parseFloat(response.data.lstGraficaDias[i]),
                                    color: "yellow"
                                }
                                break;
                            case 2:
                                obj = {
                                    name: "61 a 80",
                                    y: parseFloat(response.data.lstGraficaDias[i]),
                                    color: "orange"
                                }
                                break;
                            case 3:
                                obj = {
                                    name: "81 o mayor",
                                    y: parseFloat(response.data.lstGraficaDias[i]),
                                    color: "red"
                                }
                                break;
                        }
                        arrData.push(obj);
                    }
                    fncGraficaBLDias(arrData);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
    }

    $(document).ready(() => BackLogs = new BackLogs())
    // { $.blockUI({ message: $('#domMessage') }); }
    // { $.unblockUI(); }
})();