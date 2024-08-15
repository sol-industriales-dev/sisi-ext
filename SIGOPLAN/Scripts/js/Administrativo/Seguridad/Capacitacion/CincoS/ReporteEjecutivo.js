(() => {
    $.namespace('Administrativo.ReporteEjecutivo');
    ReporteEjecutivo = function () {
        const cboFiltroCC = $('#cboFiltroCC');
        const cboFiltroArea = $('#cboFiltroArea');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFinal = $('#inputFechaFinal');
        const btnBuscar = $('#btnBuscar');
        const btnImprimir = $('#btnImprimir');
        const report = $("#report");

        const tabla5sProyecto = $("#tabla5sProyecto");
        let dtTabla5sProyecto = null;
        const graficaBarra5sProyecto = $("#graficaBarra5sProyecto");

        (function init() {
            fncCargarCombos();
            fncSelect2();
            fncDatePicker();

            initTabla5sProyecto();
            fncListeners();
        })();

        function fncCargarCombos() {
            cboFiltroCC.fillCombo('GetCCs', { consulta: 5 }, false);
            cboFiltroArea.fillCombo('GetAreas', { consulta: 0 }, false);
        }

        function fncSelect2() {
            cboFiltroCC.select2();
            cboFiltroArea.select2();
        }
        function fncDatePicker() {
            inputFechaInicio.datepicker().datepicker("setDate", new Date());
            inputFechaFinal.datepicker().datepicker("setDate", new Date());
        }

        function fncListeners() {
            btnBuscar.click(cargarReporteEjecutivo);
            btnImprimir.click(GenerarReporte);
        }

        function initTabla5sProyecto()
        {
            dtTabla5sProyecto = tabla5sProyecto.DataTable({
                destroy: true,
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                ordering: false,
                columns: [
                    { title: "AUDITORIAS 5'S", data: 'cc' }
                    , { title: "S1 / Clasificar (Seiri)", data: 'clasificar', render: function (data, type, row) { return data.toString() + " %"; } }
                    , { title: "S2 / Ordenar (Seiton)", data: 'ordenar', render: function (data, type, row) { return data.toString() + " %"; } }
                    , { title: "S3 / Limpiar (Seiso)", data: 'limpiar', render: function (data, type, row) { return data.toString() + " %"; } }
                    , { title: "S4 / Estandarizar (Seiketsu)", data: 'estandarizar', render: function (data, type, row) { return data.toString() + " %"; } }
                    , { title: "S5 / Disciplina (Shitsuke)", data: 'disciplina', render: function (data, type, row) { return data.toString() + " %"; } }
                    , { title: "PROMEDIO TOTAL", data: 'total', render: function (data, type, row) { return data.toString() + " %"; } }
                ],
            });
        }

        function cargarReporteEjecutivo() {
            $.post("GetReporteEjecutivo", { CCs: cboFiltroCC.val(), areas: cboFiltroArea.val(), fechaInicio: inputFechaInicio.val(), fechaFin: inputFechaFinal.val() })
                .then(function (response) {
                    if (response.success) {
                        let datosTabla = response.tablaReporteEjecutivo;
                        let obras = response.obras;
                        let porcentajesObras = response.porcentajesObras;
                        let datosPorMes = response.datosPorMes;
                        if(datosTabla.length > 0)
                        {
                            dtTabla5sProyecto.clear().rows.add(datosTabla).draw();
                        }
                        if(obras.length > 0 && porcentajesObras.length > 0)
                        {
                            cargarGraficaBarras(obras, porcentajesObras);
                            CargarGraficaPentagono1(datosTabla);
                            CargarGraficaTendencia(datosPorMes[0], 'graficaTendenciaProyecto1', obras[0]);
                            if(obras.length > 1) {
                                CargarGraficaPentagono2(datosTabla);
                                CargarGraficaTendencia(datosPorMes[1], 'graficaTendenciaProyecto2', obras[1]);
                            }
                            if(obras.length > 2) {
                                CargarGraficaPentagono3(datosTabla);
                                CargarGraficaTendencia(datosPorMes[2], 'graficaTendenciaProyecto3', obras[2]);
                            }
                            if(obras.length > 3) {
                                CargarGraficaPentagono4(datosTabla);
                                CargarGraficaTendencia(datosPorMes[3], 'graficaTendenciaProyecto4', obras[3]);
                            }
                            if(obras.length > 4) {
                                CargarGraficaPentagono5(datosTabla);
                                CargarGraficaTendencia(datosPorMes[4], 'graficaTendenciaProyecto5', obras[4]);
                            }
                            if(obras.length > 5) {
                                CargarGraficaPentagono6(datosTabla);
                                CargarGraficaTendencia(datosPorMes[5], 'graficaTendenciaProyecto6', obras[5]);
                            }
                        }

                    } else {
                        // Operación no completada.
                        var a = 1;
                    }
                }, function(error) {
                    // Error al lanzar la petición.
                    var b = 1;
                }
            );
        }

        function cargarGraficaBarras(obras, porcentajes)
        {
            Highcharts.chart('graficaBarra5sProyecto', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                xAxis: {
                    categories: obras
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: '',
                    data: porcentajes
                }],
                plotOptions: {
                    column: {
                        colorByPoint: true
                    }
                },
                colors: [
                    '#ff0000',
                    '#00ff00',
                    '#0000ff'
                ]
            });           
        }

        function CargarGraficaPentagono1(datos)
        {            
            Highcharts.chart('graficaPentagono5sProyecto1', {
                chart: {
                    polar: true,
                    type: 'area'
                },
                title: {
                    text: datos[0].cc,
                },
                xAxis: {
                    categories: ['Clasificar', 'Limpieza', 'Orden', 'Estandarización', 'Disciplina' ],
                    tickmarkPlacement: 'on',
                    lineWidth: 0
                },
                yAxis: {
                    gridLineInterpolation: 'polygon',
                    lineWidth: 0,
                    min: 0,
                    max: 100
                },
                tooltip: {
                    shared: true,
                    pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.0f}</b><br/>'
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                series: [
                    {
                        name: 'Auditoria',
                        data: [datos[0].clasificar, datos[0].limpiar, datos[0].ordenar, datos[0].estandarizar, datos[0].disciplina ],
                        color: "#f06203",
                        fillOpacity: 0.5,
                        pointPlacement: 'on'
                    },
                    {
                        name: 'Objetivo',
                        data: [100, 100, 100, 100, 100],
                        color: "#2E8BC0",
                        fillOpacity: 0.2,
                        pointPlacement: 'on'
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
                        }
                    }]
                },
                credits: { enabled: false }
            });
        }

        function CargarGraficaPentagono2(datos)
        {            
            Highcharts.chart('graficaPentagono5sProyecto2', {
                chart: {
                    polar: true,
                    type: 'area'
                },
                title: {
                    text: datos[1].cc,
                },
                xAxis: {
                    categories: ['Clasificar', 'Limpieza', 'Orden', 'Estandarización', 'Disciplina' ],
                    tickmarkPlacement: 'on',
                    lineWidth: 0
                },
                yAxis: {
                    gridLineInterpolation: 'polygon',
                    lineWidth: 0,
                    min: 0,
                    max: 100
                },
                tooltip: {
                    shared: true,
                    pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.0f}</b><br/>'
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                series: [
                    {
                        name: 'Auditoria',
                        data: [datos[1].clasificar, datos[1].limpiar, datos[1].ordenar, datos[1].estandarizar, datos[1].disciplina ],
                        color: "#f06203",
                        fillOpacity: 0.5,
                        pointPlacement: 'on'
                    },
                    {
                        name: 'Objetivo',
                        data: [100, 100, 100, 100, 100],
                        color: "#2E8BC0",
                        fillOpacity: 0.2,
                        pointPlacement: 'on'
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
                        }
                    }]
                },
                credits: { enabled: false }
            });
        }

        function CargarGraficaPentagono3(datos)
        {            
            Highcharts.chart('graficaPentagono5sProyecto3', {
                chart: {
                    polar: true,
                    type: 'area'
                },
                title: {
                    text: datos[2].cc,
                },
                xAxis: {
                    categories: ['Clasificar', 'Limpieza', 'Orden', 'Estandarización', 'Disciplina' ],
                    tickmarkPlacement: 'on',
                    lineWidth: 0
                },
                yAxis: {
                    gridLineInterpolation: 'polygon',
                    lineWidth: 0,
                    min: 0,
                    max: 100
                },
                tooltip: {
                    shared: true,
                    pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.0f}</b><br/>'
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                series: [
                    {
                        name: 'Auditoria',
                        data: [datos[2].clasificar, datos[2].limpiar, datos[2].ordenar, datos[2].estandarizar, datos[2].disciplina ],
                        color: "#f06203",
                        fillOpacity: 0.5,
                        pointPlacement: 'on'
                    },
                    {
                        name: 'Objetivo',
                        data: [100, 100, 100, 100, 100],
                        color: "#2E8BC0",
                        fillOpacity: 0.2,
                        pointPlacement: 'on'
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
                        }
                    }]
                },
                credits: { enabled: false }
            });
        }

        function CargarGraficaPentagono4(datos)
        {            
            Highcharts.chart('graficaPentagono5sProyecto4', {
                chart: {
                    polar: true,
                    type: 'area'
                },
                title: {
                    text: datos[3].cc,
                },
                xAxis: {
                    categories: ['Clasificar', 'Limpieza', 'Orden', 'Estandarización', 'Disciplina' ],
                    tickmarkPlacement: 'on',
                    lineWidth: 0
                },
                yAxis: {
                    gridLineInterpolation: 'polygon',
                    lineWidth: 0,
                    min: 0,
                    max: 100
                },
                tooltip: {
                    shared: true,
                    pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.0f}</b><br/>'
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                series: [
                    {
                        name: 'Auditoria',
                        data: [datos[3].clasificar, datos[3].limpiar, datos[3].ordenar, datos[3].estandarizar, datos[3].disciplina ],
                        color: "#f06203",
                        fillOpacity: 0.5,
                        pointPlacement: 'on'
                    },
                    {
                        name: 'Objetivo',
                        data: [100, 100, 100, 100, 100],
                        color: "#2E8BC0",
                        fillOpacity: 0.2,
                        pointPlacement: 'on'
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
                        }
                    }]
                },
                credits: { enabled: false }
            });
        }

        function CargarGraficaPentagono5(datos)
        {            
            Highcharts.chart('graficaPentagono5sProyecto5', {
                chart: {
                    polar: true,
                    type: 'area'
                },
                title: {
                    text: datos[4].cc,
                },
                xAxis: {
                    categories: ['Clasificar', 'Limpieza', 'Orden', 'Estandarización', 'Disciplina' ],
                    tickmarkPlacement: 'on',
                    lineWidth: 0
                },
                yAxis: {
                    gridLineInterpolation: 'polygon',
                    lineWidth: 0,
                    min: 0,
                    max: 100
                },
                tooltip: {
                    shared: true,
                    pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.0f}</b><br/>'
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                series: [
                    {
                        name: 'Auditoria',
                        data: [datos[4].clasificar, datos[4].limpiar, datos[4].ordenar, datos[4].estandarizar, datos[4].disciplina ],
                        color: "#f06203",
                        fillOpacity: 0.5,
                        pointPlacement: 'on'
                    },
                    {
                        name: 'Objetivo',
                        data: [100, 100, 100, 100, 100],
                        color: "#2E8BC0",
                        fillOpacity: 0.2,
                        pointPlacement: 'on'
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
                        }
                    }]
                },
                credits: { enabled: false }
            });
        }

        function CargarGraficaPentagono6(datos)
        {            
            Highcharts.chart('graficaPentagono5sProyecto6', {
                chart: {
                    polar: true,
                    type: 'area'
                },
                title: {
                    text: datos[5].cc,
                },
                xAxis: {
                    categories: ['Clasificar', 'Limpieza', 'Orden', 'Estandarización', 'Disciplina' ],
                    tickmarkPlacement: 'on',
                    lineWidth: 0
                },
                yAxis: {
                    gridLineInterpolation: 'polygon',
                    lineWidth: 0,
                    min: 0,
                    max: 100
                },
                tooltip: {
                    shared: true,
                    pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.0f}</b><br/>'
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                series: [
                    {
                        name: 'Auditoria',
                        data: [datos[5].clasificar, datos[5].limpiar, datos[5].ordenar, datos[5].estandarizar, datos[5].disciplina ],
                        color: "#f06203",
                        fillOpacity: 0.5,
                        pointPlacement: 'on'
                    },
                    {
                        name: 'Objetivo',
                        data: [100, 100, 100, 100, 100],
                        color: "#2E8BC0",
                        fillOpacity: 0.2,
                        pointPlacement: 'on'
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
                        }
                    }]
                },
                credits: { enabled: false }
            });
        }

        function CargarGraficaTendencia(datos, elemento, obra)
        {
            Highcharts.chart(elemento, {
                title: {
                    text: obra
                },
                yAxis: {
                    title: {
                        text: '% Cumplimiento'
                    },
                    max: 100,
                    crosshair: true,
                    gridLineWidth: 0,
                },

                xAxis: {
                    accessibility: {
                        rangeDescription: 'Enero a Diciembre'
                    },
                    categories: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                    crosshair: true,
                    gridLineWidth: 1,
                },

                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },

                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        },
                        //pointStart: 2010
                    }
                },

                series: [{
                    name: '',
                    color: '#EF6202',
                    marker: {
                        fillColor: '#000000',
                        lineWidth: 1,
                        lineColor: '#000000',
                        symbol: 'diamond'
                    },
                    data: datos
                }],

                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
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

        function GenerarReporte () {
            // var svg = chart.getSVG({
            //     exporting: {
            //         sourceWidth: chart.chartWidth,
            //         sourceHeight: chart.chartHeight
            //     }
            // });
            let domImgSrc;

            var node = document.getElementById('divReporteEjecutivo');
            // node.style.display = "block";

            domtoimage
                .toPng(node)
                .then(function (dataUrl) {
                    var img = new Image();
                    domImgSrc = dataUrl;
                    img.src = dataUrl;
                    // document.body.appendChild(img);
                    // console.log(imgCalendario);

                    img.onload = function () {
                        img5sReporteEjecutivo = dataUrl;
                        fncSaveImgSession();
                        // Alert2AccionConfirmar('', '¿Desea generar el reporte?', 'Confirmar', 'Cancelar', () => fncSaveImgSession());
                        // node.style.display = "none";

                    };
                })
                .catch(function (error) {
                    console.error("oops, something went wrong!", error);
                });
        };

        function fncSaveImgSession() {
            axios.post("SaveImg5sReporteEjecutivo", { img: img5sReporteEjecutivo }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    const report = $("#report");
                    report.attr("src", '/Reportes/Vista.aspx?idReporte=283');
                    document.getElementById('report').onload = function () {
                        openCRModal();
                        $.unblockUI();
                    };
                }
            }).catch(error => Alert2Error(error.message));
        }

        //function verReporte() {
        //    report.attr("src", `/Reportes/Vista.aspx?idReporte=${283}`);
        //    report.on('load', function () {
        //        $.unblockUI();
        //        openCRModal();
        //    });
        //}

    }

    $(document).ready(() => {
        Administrativo.ReporteEjecutivo = new ReporteEjecutivo();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();