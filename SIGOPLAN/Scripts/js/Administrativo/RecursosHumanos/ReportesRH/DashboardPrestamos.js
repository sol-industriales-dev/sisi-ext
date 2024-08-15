(() => {
    $.namespace('CH.DashboardPrestamos');

    //#region CONST FILTROS
    const txtFiltro_FechaInicio = $("#txtFiltro_FechaInicio")
    const txtFiltro_FechaFin = $("#txtFiltro_FechaFin")
    const cboFiltro_CC = $("#cboFiltro_CC")
    const btnFiltroBuscar = $('#btnFiltroBuscar')
    //#endregion

    //#region CONST GRAFICAS
    const graficaCantidadPrestamos = $("#graficaCantidadPrestamos")
    const graficaCantindadTipoPrestamos = $("#graficaCantindadTipoPrestamos")
    //#endregion

    DashboardPrestamos = function () {
        (function init() {
            fncListeners();

            // FILL COMBOS
            cboFiltro_CC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos")
            convertToMultiselect("#cboFiltro_CC")

            //#region FILTROS
            btnFiltroBuscar.click(function () {
                fncGetDashboardPrestamos()
            })
            //#endregion
        })();

        function fncListeners() {
            //#region INIT GRAFICAS
            initGraficaCantidadPrestamos()
            initGraficaCantindadTipoPrestamos()
            //#endregion
        }

        //#region FUNCIONES DASHBOARD | GRAFICAS
        function fncGetDashboardPrestamos() {
            let obj = new Object()
            obj.lstCC = getValoresMultiples("#cboFiltro_CC")
            obj.fechaInicio = txtFiltro_FechaInicio.val()
            obj.fechaFin = txtFiltro_FechaFin.val()
            axios.post('GetDashboardPrestamos', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    initGraficaCantidadPrestamos(response.data.lstGraficaCantidadPrestamosPorCC)
                    initGraficaCantindadTipoPrestamos(response.data.lstGraficaCantidadPrestamosPorTipoPrestamos)
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initGraficaCantidadPrestamos(datos) {
            if (datos != undefined && datos != null) {
                let obj = {}
                let arr = []
                datos.forEach(element => {
                    obj = {}
                    obj.name = element.name
                    obj.y = element.y
                    obj.drilldown = element.drilldown
                    arr.push(obj)
                });

                Highcharts.chart("graficaCantidadPrestamos", {
                    chart: {
                        type: 'column'
                    },
                    title: {
                        align: 'center',
                        text: 'Cantidad prestamos'
                    },
                    accessibility: {
                        announceNewData: {
                            enabled: true
                        }
                    },
                    xAxis: {
                        type: 'category'
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                format: '{point.y:.1f}'
                            }
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> of total<br/>'
                    },
                    series: [
                        {
                            name: "Prestamos",
                            colorByPoint: true,
                            data: arr
                        }
                    ]
                });
            }
        }

        function initGraficaCantindadTipoPrestamos(datos) {
            if (datos != undefined && datos != null) {
                let obj = {}
                let arr = []
                datos.forEach(element => {
                    obj = {}
                    obj.name = element.name
                    obj.y = element.y
                    obj.drilldown = element.drilldown
                    arr.push(obj)
                });

                Highcharts.chart("graficaCantindadTipoPrestamos", {
                    chart: {
                        type: 'column'
                    },
                    title: {
                        align: 'center',
                        text: 'Cantidad tipo de prestamos'
                    },
                    accessibility: {
                        announceNewData: {
                            enabled: true
                        }
                    },
                    xAxis: {
                        type: 'category'
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                format: '{point.y:.1f}'
                            }
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> of total<br/>'
                    },
                    series: [
                        {
                            name: "Prestamos",
                            colorByPoint: true,
                            data: arr
                        }
                    ]
                });
            }
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.DashboardPrestamos = new DashboardPrestamos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();