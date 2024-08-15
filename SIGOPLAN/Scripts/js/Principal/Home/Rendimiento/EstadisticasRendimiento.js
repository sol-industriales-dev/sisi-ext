(function () {

    $.namespace('principal.home.estadisticas');
    colores = ['#0074D9', '#FFDC00', '#FF4136', '#7FDBFF', '#FF851B', '#85144b', '#2ECC40', '#001f3f', '#ff6600', '#F012BE', '#01FF70', '#39CCCC'];
    estadisticas = function () {
        function init() {
            cargarGrafica();
        }

        function cargarGrafica() {
            var cc = [];
            var cc2 = [];
            var data = [];
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Home/getNotificacionesRendimiento",
                data: {},
                asyn: false,
                success: function (response) {
                    var notificaciones = response.rendimiento;
                    for (var i = 0; i < notificaciones.length; i++) {
                        data.push(notificaciones[i].rendimientofinal);
                        cc.push(notificaciones[i].economico + " - " + notificaciones[i].cc);
                    }
                    var max = Math.max.apply(Math, data);
                    var ctx = document.getElementById("recent-rep-chart");
                    if (ctx) {
                        ctx.height = 350;
                        var myChart = new Chart(ctx, {
                            type: 'bar',
                            data: {
                                labels: cc,
                                datasets: [
                                  {
                                      label: 'Rendimiento',
                                      backgroundColor: colores,
                                      hoverBackgroundColor: colores,
                                      borderColor: 'transparent',
                                      pointHoverBackgroundColor: '#fff',
                                      borderWidth: 0,
                                      data: data
                                  }
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
                                            labelString: 'CC',
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
                                            display: true,
                                            color: '#f2f2f2'

                                        },
                                        scaleLabel: {
                                            display: true,
                                            labelString: 'Rendimiento',
                                            fontSize: 14
                                        }
                                    }]
                                },
                                elements: {
                                    point: {
                                        radius: 0,
                                        hitRadius: 10,
                                        hoverRadius: 4,
                                        hoverBorderWidth: 3
                                    }
                                }
                            }
                        });
                    }

                    ctx = document.getElementById("percent-chart");
                    if (ctx) {
                        ctx.height = 250;
                        var myChart = new Chart(ctx, {
                            type: 'doughnut',
                            data: {
                                datasets: [
                                  {
                                      label: "Alertas",
                                      data: data,
                                      backgroundColor: colores,
                                      hoverBackgroundColor: colores,
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
                                        'transparent',
                                        'transparent'
                                      ]
                                  }
                                ],
                                labels: cc
                            },
                            options: {
                                maintainAspectRatio: false,
                                responsive: true,
                                cutoutPercentage: 55,
                                animation: {
                                    animateScale: true,
                                    animateRotate: true
                                },
                                legend: {
                                    display: false
                                },
                                tooltips: {
                                    //titleFontFamily: "Poppins",
                                    xPadding: 15,
                                    yPadding: 10,
                                    caretPadding: 0,
                                    bodyFontSize: 16
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