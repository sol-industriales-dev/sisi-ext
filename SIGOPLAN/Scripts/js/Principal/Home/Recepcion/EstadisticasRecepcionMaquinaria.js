(function () {

    $.namespace('principal.home.estadisticas');
    estadisticas = function () {
        function init() {
            cargarGrafica();
        }

        function cargarGrafica() {
            var mesesconst = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
            var meses = [];
            var data1 = [];
            var mesAux = new Date().getMonth();
            var year;
            if (mesAux == 11) { mesAux = 1; }
            else { mesAux = mesAux + 2; }
            year = new Date().getFullYear();
            year--;
            for (var i = 0; i < 12; i++) {
                if (mesAux == 12) {
                    meses[i] = mesesconst[mesAux - 1] + " " + year;
                    mesAux = 1;
                    year++;
                }
                else {
                    meses[i] = mesesconst[mesAux - 1] + " " + year;
                    mesAux++;
                }
            }
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/ControlCalidad/GetMaquinariasPendientesEnvios",
                data: { obj: 2, tipoFiltro: 1 },
                asyn: false,
                success: function (response) {
                    data1 = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
                    for (var i = 0; i < response.EquiposPendientes.length; i++) {
                        var ames = parseInt(response.EquiposPendientes[i].Fecha.substring(3, 5)) - 1;
                        var aanio = parseInt(response.EquiposPendientes[i].Fecha.substring(6, 10))
                        var aux = mesesconst[ames] + " " + aanio;
                        var aux2 = meses.indexOf(aux);
                        data1[aux2]++;
                    }

                    for (var i = 0; i < meses.length; i++) {
                        $(".mes" + (i + 1).toString()).text(meses[i]);
                        meses[i] = meses[i].split(' ');
                    }
                    var max = Math.max.apply(Math, data1);
                    var ctx = document.getElementById("recent-rep-chart");
                    if (ctx) {
                        ctx.height = 300;
                        var myChart = new Chart(ctx, {
                            type: 'bar',
                            data: {
                                labels: meses,
                                datasets: [
                                  {
                                      label: 'Alertas',
                                      backgroundColor: [
                                        '#0074D9',
                                        '#FFDC00',
                                        '#FF4136',
                                        '#7FDBFF',
                                        '#FF851B',
                                        '#85144b',
                                        '#2ECC40',
                                        '#001f3f',
                                        '#ff6600',
                                        '#F012BE',
                                        '#01FF70',
                                        '#39CCCC'
                                      ],
                                      hoverBackgroundColor: [
                                       '#0074D9',
                                        '#FFDC00',
                                        '#FF4136',
                                        '#7FDBFF',
                                        '#FF851B',
                                        '#85144b',
                                        '#2ECC40',
                                        '#001f3f',
                                        '#ff6600',
                                        '#F012BE',
                                        '#01FF70',
                                        '#39CCCC'
                                      ],
                                      borderWidth: [
                                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                                      ],
                                      hoverBorderColor: [
                                        'transparent',
                                        'transparent',
                                        'transparent',
                                        'transparent',
                                        'transparent',
                                        'transparent',
                                        'transparent',
                                        'transparent',
                                        'transparent',
                                        'transparent',
                                        'transparent',
                                        'transparent'
                                      ],
                                      borderColor: 'rgba(0,173,95,0.9)',
                                      pointHoverBackgroundColor: '#fff',
                                      borderWidth: 0,
                                      data: data1,
                                      pointBackgroundColor: 'rgba(0,173,95,0.9)'
                                  },
                                ]
                            },
                            options: {
                                maintainAspectRatio: false,
                                legend: {
                                    display: false
                                },
                                responsive: true,
                                scales: {
                                    xAxes: [{
                                        gridLines: {
                                            drawOnChartArea: true,
                                            color: '#f2f2f2'
                                        },
                                        ticks: {
                                            fontSize: 12
                                        },
                                        scaleLabel: {
                                            display: true,
                                            labelString: 'Mes',
                                            fontSize: 14
                                        }
                                    }],
                                    yAxes: [{
                                        ticks: {
                                            beginAtZero: true,
                                            maxTicksLimit: 5,
                                            stepSize: max > 10 ? (max + (10 - max % 10)) / 5 : 2,
                                            max: max > 10 ? max + (10 - max % 10) : max + 1,
                                            fontSize: 12
                                        },
                                        gridLines: {
                                            display: false,
                                            color: '#f2f2f2'
                                        },
                                        scaleLabel: {
                                            display: true,
                                            labelString: 'Alertas',
                                            fontSize: 14
                                        }
                                    }]
                                },
                                elements: {
                                    point: {
                                        radius: 3,
                                        hoverRadius: 4,
                                        hoverBorderWidth: 3,
                                        backgroundColor: '#333'
                                    }
                                }


                            }
                        });
                    }                                      
                },
                error: function () {
                    AlertaGeneral("Alerta", "Error en la consulta");
                }
            });
        }

        init();
    };


    $(document).ready(function () {
        principal.home.estadisticas = new estadisticas();
    });
})();