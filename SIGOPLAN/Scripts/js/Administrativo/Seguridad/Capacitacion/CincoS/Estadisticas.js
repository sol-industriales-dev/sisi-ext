(() => {
    $.namespace('Administrativo.Estadisticas');
    Estadisticas = function () {

        const cboFiltroCC = $('#cboFiltroCC');
        const cboFiltroArea = $('#cboFiltroArea');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFinal = $('#inputFechaFinal');
        const btnBuscar = $('#btnBuscar');

        const tblTendenciasImplementadas = $("#tblTendenciasImplementadas");
        let dtTendenciasImplementadas = null;
        const grTendenciasImplementadas = $("#grTendenciasImplementadas");

        const inputTotalObservaciones = $("#inputTotalObservaciones");
        const inputTotalEnRevision = $("#inputTotalEnRevision");
        const inputTotalEnProceso = $("#inputTotalEnProceso");
        const inputTotalConcluidas = $("#inputTotalConcluidas");

        const inputClasificar = $("#inputClasificar");
        const inputLimpieza = $("#inputLimpieza");
        const inputOrden = $("#inputOrden");
        const inputEstandarizacion = $("#inputEstandarizacion");
        const inputDisciplina = $("#inputDisciplina");

        const tblProduccion = $("#tblProduccion");
        let dtProduccion = null;
        const graficaProduccion = $("#graficaProduccion");

        const tblMantenimiento = $("#tblMantenimiento");
        let dtMantenimiento = null;
        const graficaMantenimiento = $("#graficaMantenimiento");

        const tblOverhaul = $("#tblOverhaul");
        let dtOverhaul = null;
        const graficaOverhauln = $("#graficaOverhauln");

        const tblAlmacen = $("#tblAlmacen");
        let dtAlmacen = null;
        const graficaAlmacen = $("#graficaAlmacen");

        const tblAreaAdmin = $("#tblAreaAdmin");
        let dtAreaAdmin = null;
        const graficaAreaAdmin = $("#graficaAreaAdmin");

        (function init() {
            fncListeners();
            fncCargarCombos();
            fncSelect2();
            fncDatePicker();
            initTblTendenciasImplementadas();
            initTblProduccion();
            initTblMantenimiento();
            initTblOverhaul();
            initTblAlmacen();
            initTblAreaAdmin();
        })();

        function fncListeners() {
            btnBuscar.click(cargarInformacion);
        }

        function fncCargarCombos() {
            cboFiltroCC.fillCombo('GetCCs', { consulta: 0 }, false);
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

        function cargarInformacion() {
            $.post("GetEstadisticasTendencias", { CCs: cboFiltroCC.val(), areas: cboFiltroArea.val(), fechaInicio: inputFechaInicio.val(), fechaFin: inputFechaFinal.val() })
                .then(function (response) {
                    if (response.success) {
                        var datosTablaTendencias = response.tablaTendencias;
                        var datosGraficaTendencias = response.datosGraficaTendencias;
                        var datosGraficaRadar = response.datosGraficaRadar;
                        var datosTablaProduccion = response.tablaProduccion;
                        var datosTablaMantenimiento = response.tablaMantenimiento;
                        var datosTablaOverhaul = response.tablaOverhaul;
                        var datosTablaAlmacen = response.tablaAlmacen;
                        var datosTablaAdministrativas = response.tablaAdministrativas;
                        if (datosTablaTendencias.length > 0) dtTendenciasImplementadas.clear().rows.add(datosTablaTendencias).draw();
                        if (datosGraficaTendencias.length > 0) CargarGraficaTendencias(datosGraficaTendencias);
                        if (datosGraficaRadar.length > 0) {
                            cargarGraficaRadar(datosGraficaRadar);
                            inputClasificar.val(datosGraficaRadar[0] + ' %');
                            inputLimpieza.val(datosGraficaRadar[1] + ' %');
                            inputOrden.val(datosGraficaRadar[2] + ' %');
                            inputEstandarizacion.val(datosGraficaRadar[3] + ' %');
                            inputDisciplina.val(datosGraficaRadar[4] + ' %');
                        }
                        inputTotalObservaciones.val(response.totalObservaciones);
                        inputTotalEnRevision.val(response.totalEnRevision);
                        inputTotalEnProceso.val(response.totalEnProceso);
                        inputTotalConcluidas.val(response.totalConcluidas);
                        if (datosTablaProduccion.length > 0) {
                            dtProduccion.clear().rows.add(datosTablaProduccion).draw();
                            var pendientes = $.map(datosTablaProduccion, function (val) { return val.pendientes; });
                            var concluidas = $.map(datosTablaProduccion, function (val) { return val.concluidas; });
                            var total = $.map(datosTablaProduccion, function (val) { return val.total; });

                            var sumPendientes = pendientes.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputPendientesProduccion").val(sumPendientes);
                            var sumConcluidas = concluidas.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputConcluidasProduccion").val(sumConcluidas);
                            var sumTotal = total.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputAccionesProduccion").val(sumTotal);
                            var cumplimiento = (sumConcluidas / (sumTotal == 0 ? 1 : sumTotal) * 100);
                            $("#inputCumplimientoProduccion").val(cumplimiento.toFixed(2) + ' %');

                            cargarGraficaProduccion(pendientes, concluidas, total);
                        }
                        if (datosTablaMantenimiento.length > 0) {
                            dtMantenimiento.clear().rows.add(datosTablaMantenimiento).draw();
                            var pendientes = $.map(datosTablaMantenimiento, function (val) { return val.pendientes; });
                            var concluidas = $.map(datosTablaMantenimiento, function (val) { return val.concluidas; });
                            var total = $.map(datosTablaMantenimiento, function (val) { return val.total; });

                            var sumPendientes = pendientes.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputPendientesMantenimiento").val(sumPendientes);
                            var sumConcluidas = concluidas.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputConcluidasMantenimiento").val(sumConcluidas);
                            var sumTotal = total.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputAccionesMantenimiento").val(sumTotal);
                            var cumplimiento = (sumConcluidas / (sumTotal == 0 ? 1 : sumTotal)) * 100;
                            $("#inputCumplimientoMantenimiento").val(cumplimiento.toFixed(2) + ' %');

                            cargarGraficaMantenimiento(pendientes, concluidas, total);
                        }
                        if (datosTablaOverhaul.length > 0) {
                            dtOverhaul.clear().rows.add(datosTablaOverhaul).draw();
                            var pendientes = $.map(datosTablaOverhaul, function (val) { return val.pendientes; });
                            var concluidas = $.map(datosTablaOverhaul, function (val) { return val.concluidas; });
                            var total = $.map(datosTablaOverhaul, function (val) { return val.total; });

                            var sumPendientes = pendientes.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputPendientesOverhaul").val(sumPendientes);
                            var sumConcluidas = concluidas.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputConcluidasOverhaul").val(sumConcluidas);
                            var sumTotal = total.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputAccionesOverhaul").val(sumTotal);
                            var cumplimiento = (sumConcluidas / (sumTotal == 0 ? 1 : sumTotal)) * 100;
                            $("#inputCumplimientoOverhaul").val(cumplimiento.toFixed(2) + ' %');

                            cargarGraficaOverhaul(pendientes, concluidas, total);
                        }
                        if (datosTablaAlmacen.length > 0) {
                            dtAlmacen.clear().rows.add(datosTablaAlmacen).draw();
                            var pendientes = $.map(datosTablaAlmacen, function (val) { return val.pendientes; });
                            var concluidas = $.map(datosTablaAlmacen, function (val) { return val.concluidas; });
                            var total = $.map(datosTablaAlmacen, function (val) { return val.total; });

                            var sumPendientes = pendientes.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputPendientesAlmacen").val(sumPendientes);
                            var sumConcluidas = concluidas.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputConcluidasAlmacen").val(sumConcluidas);
                            var sumTotal = total.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputAccionesAlmacen").val(sumTotal);
                            var cumplimiento = (sumConcluidas / (sumTotal == 0 ? 1 : sumTotal)) * 100;
                            $("#inputCumplimientoAlmacen").val(cumplimiento.toFixed(2) + ' %');

                            cargarGraficaAlmacen(pendientes, concluidas, total);
                        }
                        if (datosTablaAdministrativas.length > 0) {
                            dtAreaAdmin.clear().rows.add(datosTablaAdministrativas).draw();
                            var pendientes = $.map(datosTablaAdministrativas, function (val) { return val.pendientes; });
                            var concluidas = $.map(datosTablaAdministrativas, function (val) { return val.concluidas; });
                            var total = $.map(datosTablaAdministrativas, function (val) { return val.total; });

                            var sumPendientes = pendientes.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputPendientesAreaAdmin").val(sumPendientes);
                            var sumConcluidas = concluidas.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputConcluidasAreaAdmin").val(sumConcluidas);
                            var sumTotal = total.reduce(function (a, b) { return parseInt(a, 10) + parseInt(b, 10); })
                            $("#inputAccionesAreaAdmin").val(sumTotal);
                            var cumplimiento = (sumConcluidas / (sumTotal == 0 ? 1 : sumTotal)) * 100;
                            $("#inputCumplimientoAreaAdmin").val(cumplimiento.toFixed(2) + ' %');

                            cargarGraficaAreaAdmin(pendientes, concluidas, total);
                        }

                    } else {
                        // Operación no completada.
                        var a = 1;
                    }
                }, function (error) {
                    // Error al lanzar la petición.
                    var b = 1;
                }
                );
        }

        //--> SECCION GENERAL
        function initTblTendenciasImplementadas() {
            dtTendenciasImplementadas = tblTendenciasImplementadas.DataTable({
                destroy: true,
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                ordering: false,
                columns: [
                    { title: " ", data: 'cincoSDescripcion', width: "16%" }
                    , { title: "Enero", data: 'enero', width: "7%" }
                    , { title: "Febrero", data: 'febrero', width: "7%" }
                    , { title: "Marzo", data: 'marzo', width: "7%" }
                    , { title: "Abril", data: 'abril', width: "7%" }
                    , { title: "Mayo", data: 'mayo', width: "7%" }
                    , { title: "Junio", data: 'junio', width: "7%" }
                    , { title: "Julio", data: 'julio', width: "7%" }
                    , { title: "Agosto", data: 'agosto', width: "7%" }
                    , { title: "Septiembre", data: 'septiembre', width: "7%" }
                    , { title: "Octubre", data: 'octubre', width: "7%" }
                    , { title: "Noviembre", data: 'noviembre', width: "7%" }
                    , { title: "Diciembre", data: 'diciembre', width: "7%" }
                ],
                footerCallback: function (tfoot, data, start, end, display) {
                    var api = this.api();
                    var lastRow = api.rows().count();
                    for (i = 1; i < api.columns().count(); i++) {
                        let ultimo = api.cell(lastRow - 1, i).data();
                        let total = 0;
                        for (j = 0; j < api.rows().count() - 1; j++) {
                            total += api.cell(j, i).data();
                        }
                        let porcentaje = 0;
                        if (ultimo > 0) porcentaje = (total / ultimo) * 100;

                        $(tfoot).find('th').eq(i).html(porcentaje.toFixed(2) + ' %');
                    }
                },
            });
        }

        function CargarGraficaTendencias(datos) {
            Highcharts.chart('grTendenciasImplementadas', {
                title: {
                    text: ''
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
        //<--

        //-->SECCION ACCIONES SEGUIMIENTO
        function initTblProduccion() {
            dtProduccion = tblProduccion.DataTable({
                destroy: true,
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                ordering: false,
                columns: [
                    { title: "MES AUDITORIA", data: 'mes', width: "25%" }
                    , { title: "TOTAL DE \"S 's\" EVALUADAS", data: 'total', width: "25%" }
                    , { title: "TOTAL DE \"S 's\" NO ESTANDARIZADAS", data: 'pendientes', width: "25%" }
                    , { title: "\"S 's\" ESTANDARIZADAS", data: 'concluidas', width: "25%" }
                ],
            });
        }

        function initTblMantenimiento() {
            dtMantenimiento = tblMantenimiento.DataTable({
                destroy: true,
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                ordering: false,
                columns: [
                    { title: "MES AUDITORIA", data: 'mes', width: "25%" }
                    , { title: "TOTAL DE \"S 's\" EVALUADAS", data: 'total', width: "25%" }
                    , { title: "TOTAL DE \"S 's\" NO ESTANDARIZADAS", data: 'pendientes', width: "25%" }
                    , { title: "\"S 's\" ESTANDARIZADAS", data: 'concluidas', width: "25%" }
                ],
            });
        }

        function initTblOverhaul() {
            dtOverhaul = tblOverhaul.DataTable({
                destroy: true,
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                ordering: false,
                columns: [
                    { title: "MES AUDITORIA", data: 'mes', width: "25%" }
                    , { title: "TOTAL DE \"S 's\" EVALUADAS", data: 'total', width: "25%" }
                    , { title: "TOTAL DE \"S 's\" NO ESTANDARIZADAS", data: 'pendientes', width: "25%" }
                    , { title: "\"S 's\" ESTANDARIZADAS", data: 'concluidas', width: "25%" }
                ],
            });
        }

        function initTblAlmacen() {
            dtAlmacen = tblAlmacen.DataTable({
                destroy: true,
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                ordering: false,
                columns: [
                    { title: "MES AUDITORIA", data: 'mes', width: "25%" }
                    , { title: "TOTAL DE \"S 's\" EVALUADAS", data: 'total', width: "25%" }
                    , { title: "TOTAL DE \"S 's\" NO ESTANDARIZADAS", data: 'pendientes', width: "25%" }
                    , { title: "\"S 's\" ESTANDARIZADAS", data: 'concluidas', width: "25%" }
                ],
            });
        }

        function initTblAreaAdmin() {
            dtAreaAdmin = tblAreaAdmin.DataTable({
                destroy: true,
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                ordering: false,
                columns: [
                    { title: "MES AUDITORIA", data: 'mes', width: "25%" }
                    , { title: "TOTAL DE \"S 's\" EVALUADAS", data: 'total', width: "25%" }
                    , { title: "TOTAL DE \"S 's\" NO ESTANDARIZADAS", data: 'pendientes', width: "25%" }
                    , { title: "\"S 's\" ESTANDARIZADAS", data: 'concluidas', width: "25%" }
                ],
            });
        }

        function cargarGraficaProduccion(pendientes, concluidas, total) {
            Highcharts.chart('graficaProduccion', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text: '',
                },
                xAxis: [{
                    categories: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    crosshair: true
                }],
                yAxis: {
                    title: {
                        text: '',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    labels: {
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                },
                tooltip: {
                    shared: true
                },
                legend: {
                    align: 'left',
                    x: 80,
                    verticalAlign: 'top',
                    y: 80,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                },
                series: [{
                    name: 'Pendientes',
                    type: 'column',
                    color: "#f06203",
                    data: pendientes,

                }, {
                    name: 'Concluidas',
                    type: 'column',
                    color: "#808080",
                    data: concluidas,
                }, {
                    name: 'Total',
                    type: 'spline',
                    color: "#000000",
                    data: total,
                }],
                credits: { enabled: false }
            });
        }

        function cargarGraficaMantenimiento(pendientes, concluidas, total) {
            Highcharts.chart('graficaMantenimiento', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text: '',
                },
                xAxis: [{
                    categories: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    crosshair: true
                }],
                yAxis: {
                    title: {
                        text: '',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    labels: {
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                },
                tooltip: {
                    shared: true
                },
                legend: {
                    align: 'left',
                    x: 80,
                    verticalAlign: 'top',
                    y: 80,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                },
                series: [{
                    name: 'Pendientes',
                    type: 'column',
                    color: "#f06203",
                    data: pendientes,

                }, {
                    name: 'Concluidas',
                    type: 'column',
                    color: "#808080",
                    data: concluidas,
                }, {
                    name: 'Total',
                    type: 'spline',
                    color: "#000000",
                    data: total,
                }],
                credits: { enabled: false }
            });
        }

        function cargarGraficaOverhaul(pendientes, concluidas, total) {
            Highcharts.chart('graficaOverhaul', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text: '',
                },
                xAxis: [{
                    categories: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    crosshair: true
                }],
                yAxis: {
                    title: {
                        text: '',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    labels: {
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                },
                tooltip: {
                    shared: true
                },
                legend: {
                    align: 'left',
                    x: 80,
                    verticalAlign: 'top',
                    y: 80,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                },
                series: [{
                    name: 'Pendientes',
                    type: 'column',
                    color: "#f06203",
                    data: pendientes,

                }, {
                    name: 'Concluidas',
                    type: 'column',
                    color: "#808080",
                    data: concluidas,
                }, {
                    name: 'Total',
                    type: 'spline',
                    color: "#000000",
                    data: total,
                }],
                credits: { enabled: false }
            });
        }

        function cargarGraficaAlmacen(pendientes, concluidas, total) {
            Highcharts.chart('graficaAlmacen', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text: '',
                },
                xAxis: [{
                    categories: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    crosshair: true
                }],
                yAxis: {
                    title: {
                        text: '',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    labels: {
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                },
                tooltip: {
                    shared: true
                },
                legend: {
                    align: 'left',
                    x: 80,
                    verticalAlign: 'top',
                    y: 80,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                },
                series: [{
                    name: 'Pendientes',
                    type: 'column',
                    color: "#f06203",
                    data: pendientes,

                }, {
                    name: 'Concluidas',
                    type: 'column',
                    color: "#808080",
                    data: concluidas,
                }, {
                    name: 'Total',
                    type: 'spline',
                    color: "#000000",
                    data: total,
                }],
                credits: { enabled: false }
            });
        }

        function cargarGraficaAreaAdmin(pendientes, concluidas, total) {
            Highcharts.chart('graficaAreaAdmin', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text: '',
                },
                xAxis: [{
                    categories: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    crosshair: true
                }],
                yAxis: {
                    title: {
                        text: '',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    labels: {
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                },
                tooltip: {
                    shared: true
                },
                legend: {
                    align: 'left',
                    x: 80,
                    verticalAlign: 'top',
                    y: 80,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                },
                series: [{
                    name: 'Pendientes',
                    type: 'column',
                    color: "#f06203",
                    data: pendientes,

                }, {
                    name: 'Concluidas',
                    type: 'column',
                    color: "#808080",
                    data: concluidas,
                }, {
                    name: 'Total',
                    type: 'spline',
                    color: "#000000",
                    data: total,
                }],
                credits: { enabled: false }
            });
        }


        //<--

        //--> SECCION RADAR 5'S
        function cargarGraficaRadar(datos) {
            Highcharts.chart('grRadar', {
                chart: {
                    polar: true,
                    type: 'area'
                },
                title: {
                    text: ' ',
                },
                xAxis: {
                    categories: ['Clasificar', 'Limpieza', 'Orden', 'Estandarización', 'Disciplina'],
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
                        data: datos,
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
        //<--        
    }

    $(document).ready(() => {
        Administrativo.Estadisticas = new Estadisticas();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();