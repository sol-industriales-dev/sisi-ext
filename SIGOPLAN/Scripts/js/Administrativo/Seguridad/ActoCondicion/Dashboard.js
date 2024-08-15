(() => {
    $.namespace('Administrativo.ActoCondicion.Dashboard');

    Dashboard = function () {

        // Variables.

        //Filtros.
        const comboCC = $('#comboCC');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const comboSupervisor = $('#comboSupervisor');
        const comboDepartamento = $('#comboDepartamento');
        const botonBuscar = $('#botonBuscar');

        const gpxTotalDep = $('#gpxTotalDep');
        let gpxTotalDepl;

        // Gráficas
        let chartSucesosPorMes = null;
        let chartSucesosPorDepartamento = null;
        let chartActosClasificacion = null;
        let chartCondicionesClasificacion = null;
        let chartComportamiento = null;




        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);

        (function init() {
            // Lógica de inicialización.
            llenarCombos();
            initDatepickers();
            agregarListeners();
        })();

        $('#selectDivision').on('change', function () {
            comboCC.fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivision').val(), $('#selectLineaNegocio').val());
            convertToMultiselect('#comboCC');
        });

        $('#selectLineaNegocio').on('change', function () {
            comboCC.fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivision').val(), $('#selectLineaNegocio').val());
            convertToMultiselect('#comboCC');
        });

        // Métodos.
        function llenarCombos() {
            $('#selectDivision').fillCombo('/Administrativo/Requerimientos/GetDivisionesCombo', null, false, 'Todos');
            convertToMultiselect('#selectDivision');
            $('#selectLineaNegocio').fillCombo('/Administrativo/Requerimientos/GetLineaNegocioCombo', { division: 0 }, false, 'Todos');
            convertToMultiselect('#selectLineaNegocio');

            comboCC.fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivision').val(), $('#selectLineaNegocio').val());
            convertToMultiselect('#comboCC');
            comboSupervisor.fillCombo('/Administrativo/ActoCondicion/ObtenerSupervisores', null, false, 'Todos');
            comboDepartamento.fillCombo('/Administrativo/ActoCondicion/ObtenerDepartamentos', null, false, 'Todos');

            $('.select2').select2();
        }

        function initDatepickers() {
            inputFechaInicio.datepicker({ dateFormat, showAnim }).datepicker("setDate", fechaInicioAnio);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
        }

        function agregarListeners() {
            botonBuscar.click(function () {
                cargarDatosDashboard()
                obtenerGraficaTotalDep();
            });
        }

        function cargarDatosDashboard() {

            const filtro = obtenerFiltroBusqueda();

            $.post('/Administrativo/ActoCondicion/CargarDatosDashboard', { filtro })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        initChartSucesosMes(response.chartSucesosPorMes)
                        initChartSucesosDepartamento(response.chartSucesosPorDepartamento);
                        initChartActosClasificacion(response.chartActosClasificacion);
                        initChartCondicionesClasificacion(response.chartCondicionesClasificacion);
                        initChartComportamiento(response.chartComportamiento);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );

        }

        function obtenerFiltroBusqueda() {
            const claveSupervisor = comboSupervisor.val();
            const departamentoID = comboDepartamento.val();
            // const arrGrupos = $("#comboCC").getMultiSeg();

            //#region SE ELIMINA LOS PARAMETROS ADICIONALES DEL VALUE EN CASO DE SER CONTRATISTA O AGRUPACION DE CONTRATISTAS
            let objGrupos = new Object();
            let arrGrupos = [];
            for (let i = 0; i < $("#comboCC").getMultiSeg().length; i++) {
                let str = $("#comboCC").getMultiSeg()[i].idAgrupacion;
                let idEmpresa = $("#comboCC").getMultiSeg()[i].idEmpresa;
                if (parseFloat(idEmpresa) == 1000) {
                    let idAgrupacion = str.replace("c_", "");
                    objGrupos = {
                        idEmpresa: idEmpresa,
                        idAgrupacion: parseFloat(idAgrupacion)
                    };
                    arrGrupos.push(objGrupos);
                } else if (parseFloat(idEmpresa) == 2000) {
                    let idAgrupacion = str.replace("a_", "");
                    objGrupos = {
                        idEmpresa: idEmpresa,
                        idAgrupacion: parseFloat(idAgrupacion)
                    };
                    arrGrupos.push(objGrupos);
                } else {
                    objGrupos = {
                        idEmpresa: $("#comboCC").getMultiSeg()[i].idEmpresa,
                        idAgrupacion: $("#comboCC").getMultiSeg()[i].idAgrupacion
                    };
                    arrGrupos.push(objGrupos);
                }
            }
            //#endregion

            let obj = new Object();
            obj = {
                arrGrupos: arrGrupos,
                fechaInicial: inputFechaInicio.val(),
                fechaFinal: inputFechaFin.val(),
                claveSupervisor: claveSupervisor == "Todos" ? 0 : claveSupervisor,
                departamentoID: departamentoID == "Todos" ? 0 : departamentoID,
                listaDivisiones: $('#selectDivision').val(),
                listaLineasNegocio: $('#selectLineaNegocio').val()
            };
            return obj;
        }

        function initChartSucesosMes(data) {

            const { labels, datasets } = data;

            if (chartSucesosPorMes) {
                chartSucesosPorMes.destroy();
            }

            let ctx = document.getElementById('chartSucesosPorMes').getContext('2d');

            chartSucesosPorMes = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: "Cantidad de Sucesos por Mes "
                    },
                    legend: {
                        display: true,
                        position: 'bottom',
                    },
                    plugins: {
                        labels: {
                            render: 'value'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }

        function initChartSucesosDepartamento(data) {

            const { labels, datasets } = data;

            if (chartSucesosPorDepartamento) {
                chartSucesosPorDepartamento.destroy();
            }

            var ctx = document.getElementById('chartSucesosPorDepartamento').getContext('2d');

            chartSucesosPorDepartamento = new Chart(ctx, {
                type: 'pie',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: "Actos y Condiciones Inseguras por Departamento"
                    },
                    legend: {
                        display: true,
                        position: 'bottom',
                    },
                    plugins: {
                        labels: {
                            render: 'percentage',
                            precision: 2
                        }
                    }
                }
            });
        }

        function initChartActosClasificacion(data) {

            const { labels, datasets } = data;

            if (chartActosClasificacion) {
                chartActosClasificacion.destroy();
            }

            let ctx = document.getElementById('chartActosClasificacion').getContext('2d');

            chartActosClasificacion = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: "Cantidad de Actos Inseguros por Clasificación"
                    },
                    legend: {
                        display: false
                    },
                    plugins: {
                        labels: {
                            render: 'value'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }

        function initChartCondicionesClasificacion(data) {

            const { labels, datasets } = data;

            if (chartCondicionesClasificacion) {
                chartCondicionesClasificacion.destroy();
            }

            let ctx = document.getElementById('chartCondicionesClasificacion').getContext('2d');

            chartCondicionesClasificacion = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: "Cantidad de Condiciones Inseguras por Clasificación"
                    },
                    legend: {
                        display: false
                    },
                    plugins: {
                        labels: {
                            render: 'value'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }

        function initChartComportamiento(data) {

            const { labels, datasets } = data;

            if (chartComportamiento) {
                chartComportamiento.destroy();
            }

            let ctx = document.getElementById('chartComportamiento').getContext('2d');

            chartComportamiento = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: "Cantidad de Accidentes, Comportamientos y Acciones por Mes"
                    },
                    // tooltips: {
                    //     mode: 'point',
                    //     intersect: true
                    // },
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {

                                const labels = [];

                                const dataset = data.datasets[tooltipItem.datasetIndex];
                                const index = tooltipItem.index;
                                const totalGrupo = data.datasets.filter(x => x.stack == dataset.stack).map(x => x.data[index]).reduce((a, b) => a + (+b));

                                labels.push(`${dataset.label}: ${tooltipItem.yLabel} `)
                                labels.push(`Total ${dataset.stack}: ${totalGrupo}`.toUpperCase());

                                return labels;
                            }
                        }
                    },
                    legend: {
                        display: true,
                        position: 'bottom',
                    },
                    plugins: {
                        labels: {
                            render: () => '',
                            fontStyle: 'bold',
                        }
                    },
                    scales: {
                        xAxes: [{
                            stacked: true,
                        }],
                        yAxes: [{
                            stacked: true
                        }]
                    }
                }
            });
        }



        function obtenerGraficaTotalDep() {
            let parametros = obtenerFiltroBusqueda();
            axios.post('obtenerGraficaTotalDep', { filtro: parametros })
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    if (response) {
                        gxpGraficaTotal('gpxTotalDep', response.data, null, null);
                        gpxCumplimientoActo(response.data.gpxActos);
                        gpxVacunacionDepartamentos(response.data.barraVacunacion);
                        gpxVacunacionTotal(response.data.gpxVacunacionTotal);
                        gpxVacunacionAgrupacion(response.data.barraVacunacionAgrupacion);
                    } else {
                        AlertaGeneral(`Alerta`, 'Ocurrió un error');
                    }
                });
        }

        function gxpGraficaTotal(grafica, datos, callback, items) {
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
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
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
                            format: '{point.y %}'
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b><br/>'
                },
                series: datos.gpxGrafica,
                credits: {
                    enabled: false
                }
            });
        }

        function gpxCumplimientoActo(datos) {
            Highcharts.chart('gpxCumplimientoActos', {
                chart: {
                    type: 'pie'
                },

                title: {
                    text: ''
                },

                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
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

                series: datos,

                credits: {
                    enabled: false
                }
            });
        }

        function gpxVacunacionDepartamentos(data) {
            Highcharts.chart('gpxVacunacionDepartamento', {
                chart: {
                    type: 'column'
                },

                title: {
                    text: ''
                },

                xAxis: {
                    categories: data.categorias
                },

                yAxis: {
                    allowDecimals: false,
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },

                legend: {
                    enabled: true
                },

                plotOptions: {
                    column: {
                        stacking: 'normal'
                    }
                },

                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + '</b><br/>' +
                            this.series.name + ': ' + this.y + '<br/>' +
                            'Total: ' + this.point.stackTotal;
                    }
                },

                series: data.series,

                credits: {
                    enabled: false
                }
            });
        }

        function gpxVacunacionAgrupacion(data) {
            Highcharts.chart('gpxVacunacionAgrupacion', {
                chart: {
                    type: 'column'
                },

                title: {
                    text: ''
                },

                xAxis: {
                    categories: data.categorias
                },

                yAxis: {
                    allowDecimals: false,
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    },
                    stackLabels: {
                        enabled: true,
                        style: {
                            fontWeight: 'bold',
                            color: ( // theme
                                Highcharts.defaultOptions.title.style &&
                                Highcharts.defaultOptions.title.style.color
                            ) || 'gray'
                        }
                    }
                },

                legend: {
                    enabled: true
                },

                plotOptions: {
                    column: {
                        stacking: 'normal'
                    }
                },

                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + '</b><br/>' +
                            this.series.name + ': ' + this.y + '<br/>' +
                            'Total: ' + this.point.stackTotal;
                    }
                },

                series: data.series,

                credits: {
                    enabled: false
                }
            });
        }

        function gpxVacunacionTotal(datos) {
            Highcharts.chart('gpxVacunacionTotal', {
                chart: {
                    type: 'pie'
                },

                title: {
                    text: ''
                },

                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
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

                series: datos,

                credits: {
                    enabled: false
                }
            });
        }
    }
    $(() => Administrativo.ActoCondicion.Dashboard = new Dashboard())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();