(() => {
    $.namespace('DatosDiarios.DashboardEstatusDiario');

    DashboardEstatusDiario = function () {
        //#region Selectores
        const selectAreaCuenta = $('#selectAreaCuenta');
        const botonBuscar = $('#botonBuscar');
        const divGraficas = $('#divGraficas');
        //#endregion

        let _contadorGraficas = 1;

        (function init() {
            selectAreaCuenta.select2();
            selectAreaCuenta.fillCombo('/CapturaDatos/ObtenerAreaCuenta', null, false, 'Todos');
            // convertToMultiselect('#selectAreaCuenta');

            botonBuscar.click(cargarGraficas);
        })();

        function cargarGraficas() {
            if (selectAreaCuenta.val() == '') {
                Alert2Warning('Debe seleccionar por lo menos una área-cuenta.');
                return;
            }

            axios.post('/CapturaDatos/CargarGraficasDashboard', { listaAreaCuenta: selectAreaCuenta.val() }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    _contadorGraficas = 1;
                    divGraficas.empty();

                    response.data.listaGraficas.forEach((x) => {
                        renderizarHTMLGrafica();
                        initGrafica(x.tituloAreaCuenta, x.fechaUltimaActualizacionString, x.datosGrafica);

                        _contadorGraficas++;
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function renderizarHTMLGrafica() {
            divGraficas.append(`
                <div class="col-md-6">
                    <div class="panel-group">
                        <div class="panel panel-primary">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" href="#panelGrafica_${_contadorGraficas}">
                                    </a>
                                </h4>
                            </div>
                            <div id="panelGrafica_${_contadorGraficas}" class="panel-collapse collapse in">
                                <div class="panel-body">
                                    <div class="row" style="overflow-y: auto;">
                                        <div id="grafica_${_contadorGraficas}"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `);
        }

        function initGrafica(tituloAreaCuenta, fechaUltimaActualizacionString, datos) {
            let grafica = Highcharts.chart(`grafica_${_contadorGraficas}`, {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: tituloAreaCuenta },
                subtitle: { text: `ACTUALIZADO AL: ${fechaUltimaActualizacionString}` },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
                    labels: { format: '{value}' }
                },
                tooltip: {
                    formatter: function () {
                        let index = this.points[0].point.index;
                        let causa = datos.listaCausa[index];
                        let fechaInicial = datos.listaFechaInicial[index];
                        let fechaProyectada = datos.listaFechaProyectada[index];
                        let acciones = datos.listaAcciones[index];

                        return `
                            <span style="font-size:16px"><b>${this.x}</b></span>
                            <table>
                                <tr>
                                    <td style="padding: 3px; border: 1px solid black;"><b>${this.points[0].series.name}: </b></td>
                                    <td style="padding: 3px; border: 1px solid black;">${this.y}</td>
                                </tr>
                                <tr>
                                    <td style="padding: 3px; border: 1px solid black;"><b>Causa: </b></td>
                                    <td style="padding: 3px; border: 1px solid black;">${causa}</td>
                                </tr>
                                <tr>
                                    <td style="padding: 3px; border: 1px solid black;"><b>Fecha Inicial: </b></td>
                                    <td style="padding: 3px; border: 1px solid black;">${fechaInicial}</td>
                                </tr>
                                <tr>
                                    <td style="padding: 3px; border: 1px solid black;"><b>Fecha Proyectada: </b></td>
                                    <td style="padding: 3px; border: 1px solid black;">${fechaProyectada}</td>
                                </tr>
                                <tr>
                                    <td style="padding: 3px; border: 1px solid black;"><b>Acciones: </b></td>
                                    <td style="padding: 3px; border: 1px solid black;">${acciones}</td>
                                </tr>
                            </table>
                        `;
                    },
                    // headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    // pointFormat: `
                    //     <tr>
                    //         <td style="color:{series.color};padding:0">{series.name}: </td>
                    //         <td><b>{point.y} {series.data.indexOf( this.point )}</b></td>
                    //     </tr>`,
                    // footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    },
                    series: {
                        dataLabels: {
                            enabled: true,
                        }
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1, color: 'rgb(246, 142, 55)' }
                ],
                credits: { enabled: false },
                legend: { enabled: false }
            });

            // $('.highcharts-title').css("display", "none");

            grafica.reflow();
        }
    }

    $(document).ready(() => DatosDiarios.DashboardEstatusDiario = new DashboardEstatusDiario()).ajaxStart(() => $.blockUI({ message: 'Procesando...' })).ajaxStop($.unblockUI);
})();