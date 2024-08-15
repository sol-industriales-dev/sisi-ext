(function () {

    $.namespace('principal.home.estadisticas');
    estadisticas = function () {
        function init() {
            cargarGrafica();
        }

        function cargarGrafica() {
            var data1 = [];
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Home/getNotificacionesDisponibilidad",
                data: {},
                asyn: false,
                success: function (response) {
                    var notificaciones = response.disponibilidad;
                    data1 = [0, 0, 0, 0, 0, 0, 0, 0, 0];
                    for (var i = 0; i < notificaciones.length; i++){
                        if (notificaciones[i].decimalDisponibilidad > 9){
                            if (notificaciones[i].decimalDisponibilidad > 19){
                                if (notificaciones[i].decimalDisponibilidad > 29){
                                    if (notificaciones[i].decimalDisponibilidad > 39){
                                        if (notificaciones[i].decimalDisponibilidad > 49) {
                                            if (notificaciones[i].decimalDisponibilidad > 59) {
                                                if (notificaciones[i].decimalDisponibilidad > 69) {
                                                    if (notificaciones[i].decimalDisponibilidad > 79) { data1[8]++; }
                                                    else{ data1[7]++; }
                                                }
                                                else { data1[6]++; }
                                            }
                                            else { data1[5]++; }
                                        }
                                        else { data1[4]++; }
                                    }
                                    else { data1[3]++; }
                                }
                                else { data1[2]++; }
                            }
                            else { data1[1]++; }
                        }
                        else{ data1[0]++; }
                    }
                    var max = Math.max.apply(Math, data1);
                    var ctx = document.getElementById("recent-rep-chart");
                    if (ctx) {
                        ctx.height = 300;
                        ctx.clientWidth = 1000;
                        var myChart = new Chart(ctx, {
                            type: 'bar',
                            data: {
                                labels: ['0-9%', '10-19%', '20-29%', '30-39%', '40-49%', '50-59%', '60-69%', '70-79%', '80-85%'],
                                datasets: [
                                  {
                                      label: 'Disponibilidad',
                                      backgroundColor: [
                                        '#0074D9',
                                        '#FFDC00',
                                        '#FF4136',
                                        '#7FDBFF',
                                        '#FF851B',
                                        '#85144b',
                                        '#2ECC40',
                                        '#001f3f',
                                        '#ff6600'
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
                                        '#ff6600'
                                      ],
                                      borderWidth: [
                                        0, 0, 0
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
                                        'transparent'
                                      ],
                                      borderColor: 'rgba(0,173,95,0.9)',
                                      pointHoverBackgroundColor: '#fff',
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
                                            labelString: 'Rango de porcentaje',
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