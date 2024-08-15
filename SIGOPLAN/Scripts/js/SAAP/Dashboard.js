(() => {
    $.namespace('SAAP.Dashboard');
    Dashboard = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const selectAreas = $('#selectAreas');
        const botonResultados = $('#botonResultados');
        const checkboxTodosCentroCosto = $('#checkboxTodosCentroCosto');
        const tablaActividadesVencer = $('#tablaActividadesVencer');
        const tablaActividadesCumplidas = $('#tablaActividadesCumplidas');
        //#endregion

        let dtActividadesVencer;
        let dtActividadesCumplidas;

        (function init() {
            initTablaActividadesVencer();
            initTablaActividadesCumplidas();

            agregarListeners();

            selectCentroCosto.fillCombo('/SAAP/SAAP/GetAgrupacionCombo', null, false, '');
            selectCentroCosto.find('option[value=""]').remove();
            selectAreas.fillCombo("/SAAP/SAAP/GetAreaCombo", null, false, '--Todos--');

            $('.select2').select2();
        })();

        checkboxTodosCentroCosto.on('click', function () {
            if (checkboxTodosCentroCosto.is(':checked')) {
                selectCentroCosto.find('option').prop("selected", true);
                selectCentroCosto.trigger("change");
            } else {
                selectCentroCosto.find('option').prop("selected", false);
                selectCentroCosto.trigger("change");
            }
        });

        function agregarListeners() {
            botonResultados.click(cargarResultados);
        }

        function cargarResultados() {
            let listaAgrupaciones = getValoresMultiples('#selectCentroCosto');
            let filtroArea = selectAreas.val() == '--Todos--' ? 0 : selectAreas.val();

            axios.post('/SAAP/SAAP/CargarDashboard', { listaAgrupaciones, filtroArea })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        initGraficaProgresoArranque(response.data.graficaProgresoArranque);
                        initGraficaAvanceCategoria(response.data.graficaAvanceCategoria);
                        initGraficaDesempenoArea(response.data.graficaDesempenoArea);

                        AddRows(tablaActividadesVencer, response.data.tablaActividadesVencer);
                        AddRows(tablaActividadesCumplidas, response.data.tablaActividadesCumplidas);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initGraficaProgresoArranque(datos) {
            Highcharts.chart('chartProgresoArranque', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                lang: highChartsDicEsp,
                title: { text: 'Gráfica Progreso de Arranque' },

                tooltip: {
                    pointFormat: '<b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                        }
                    }
                },
                series: [{
                    name: 'Desempeño Global',
                    colorByPoint: true,
                    data: [{ name: 'Cumplidos', y: datos.serie1[0], color: 'green' }, { name: 'No Cumplidos', y: datos.serie2[0], color: 'red' }]
                }],
                credits: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function initGraficaAvanceCategoria(datos) {
            Highcharts.chart('chartAvanceCategoria', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: '' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: { text: '' },
                    labels: {
                        formatter: function () {
                            return this.axis.defaultLabelFormatter.call(this) + '%';
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: `
                        <tr>
                            <td style="color:{series.color};padding:0">{series.name}: </td><td style="padding:0"><b>{point.y:.1f}%</b></td>
                        </tr>`,
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: { column: { pointPadding: 0.2, borderWidth: 0 } },
                series: [{
                    name: 'Avance',
                    data: datos.serie1,
                    color: 'rgb(68, 114, 196)'
                }],
                credits: { enabled: false },
                legend: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function initGraficaDesempenoArea(datos) {
            Highcharts.chart('chartDesempenoArea', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: '' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: { text: '' },
                    labels: {
                        formatter: function () {
                            return this.axis.defaultLabelFormatter.call(this) + '%';
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: `
                        <tr>
                            <td style="color:{series.color};padding:0">{series.name}: </td><td style="padding:0"><b>{point.y:.1f}%</b></td>
                        </tr>`,
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: { column: { pointPadding: 0.2, borderWidth: 0 } },
                series: [{
                    name: 'Desempeño',
                    data: datos.serie1,
                    color: 'rgb(68, 114, 196)'
                }],
                credits: { enabled: false },
                legend: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function initTablaActividadesVencer() {
            dtActividadesVencer = tablaActividadesVencer.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                ordering: false,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'actividadDesc', title: 'Actividad' },
                    { data: 'diasRestantes', title: 'Días Restantes' },
                    {
                        data: 'avance', title: 'Avance', render: function (data, type, row, meta) {
                            return data + '%';
                        }
                    },
                    { data: 'responsableDesc', title: 'Responsable' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaActividadesCumplidas() {
            dtActividadesCumplidas = tablaActividadesCumplidas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                ordering: false,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'actividadDesc', title: 'Actividad' },
                    { data: 'diasRestantes', title: 'Días Restantes' },
                    {
                        data: 'avance', title: 'Avance', render: function (data, type, row, meta) {
                            return data + '%';
                        }
                    },
                    { data: 'responsableDesc', title: 'Responsable' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => SAAP.Dashboard = new Dashboard())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();