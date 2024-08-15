(() => {
    $.namespace('Capacitacion.FactorCapacitacion');
    FactorCapacitacion = function () {

        //#region Filtros
        const cboFiltroDivision = $('#cboFiltroDivision');
        const cboFiltroCC = $('#cboFiltroCC');
        const btnBuscar = $('#btnBuscar');
        //#endregion

        //#region Panel Factor Capacitacion
        const inputPeriodoInicial = $('#inputPeriodoInicial');
        const inputPeriodoFinal = $('#inputPeriodoFinal');
        const tablaFactorCapacitacion = $('#tablaFactorCapacitacion');
        let dttablaFactorCapacitacion = null;
        const tablaGlobalAvanceFactor = $('#tablaGlobalAvanceFactor');
        let dttablaGlobalAvanceFactor = null;
        //#endregion

        //#region Panel Estadisticas
        const inputAño = $('#inputAño');
        const btnSeguimientosAreasRequeridas = $('#btnSeguimientosAreasRequeridas');
        const modalSeguimientosAreasRequeridas = $('#modalSeguimientosAreasRequeridas');

        const botonConsultarPorcentajeCapacitacionesOperativas = $('#botonConsultarPorcentajeCapacitacionesOperativas');
        const tablaPorcentajeCapacitacionesOperativas = $('#tablaPorcentajeCapacitacionesOperativas');
        let dttablaPorcentajeCapacitacionesOperativas = null;
        const graficaPorcentajeCapacitacionesOperativas = $('#graficaPorcentajeCapacitacionesOperativas');

        const botonConsultarCiclosOperativos = $('#botonConsultarCiclosOperativos');
        const tablaEfectividadCiclosOperativos = $('#tablaEfectividadCiclosOperativos');
        let dttablaEfectividadCiclosOperativos = null;
        const graficaEfectividadCiclosOperativos = $('#graficaEfectividadCiclosOperativos');
        const tablaPorcentajeCiclosOperativos = $('#tablaPorcentajeCiclosOperativos');
        let dttablaPorcentajeCiclosOperativos = null;
        const graficaPorcentajeCiclosOperativos = $('#graficaPorcentajeCiclosOperativos');

        const botonConsultarRecorridosOperativos = $('#botonConsultarRecorridosOperativos');
        const tablaPorcentajeRecorridosOperativos = $('#tablaPorcentajeRecorridosOperativos');
        let dttablaPorcentajeRecorridosOperativos = null;
        const graficaPorcentajeRecorridosOperativos = $('#graficaPorcentajeRecorridosOperativos');

        const botonConsultarImplementacion5s = $('#botonConsultarImplementacion5s');
        const tablaPorcentajeImplementacion5s = $('#tablaPorcentajeImplementacion5s');
        let dttablaPorcentajeImplementacion5s = null;
        const graficaPorcentajeImplementacion5s = $('#graficaPorcentajeImplementacion5s');

        //#endregion

        //#region Tablas dentro del modalEstadisticasFactorCap

        const btnEstadisticasFactorCap = $('#btnEstadisticasFactorCap');
        const modalEstadisticasFactorCap = $('#modalEstadisticasFactorCap');
        const tablaEstadisticasFactorCapacitacion = $('#tablaEstadisticasFactorCapacitacion');
        let dttablaEstadisticasFactorCapacitacion = null;
        //#endregion


        //#region Tablas dentro del modalSeguimientosAreasRequeridas
        const tablaSeguimientoAcciones = $('#tablaSeguimientoAcciones');
        let dttablaSeguimientoAcciones = null;
        const tablaSeguimientoPropuestas = $('#tablaSeguimientoPropuestas');
        let dttablaSeguimientoPropuestas = null;
        const tablaCumplimientoCiclosMensuales = $('#tablaCumplimientoCiclosMensuales');
        let dttablaCumplimientoCiclosMensuales = null;
        const tablaPromedioConocimiento = $('#tablaPromedioConocimiento');
        let dttablaPromedioConocimiento = null;
        //#endregion

        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            $(".select2").select2()

            cboFiltroDivision.fillCombo('fillComboDivision', null, false, null);
            cboFiltroDivision.on("change", function () {

                cboFiltroCC.fillCombo('GetCentrosCostoDivision', { division: cboFiltroDivision.val() }, false, null);
                getValoresMultiples('#cboFiltroCC');

            });
            // cboFiltroCC.fillCombo('/Administrativo/Capacitacion/ObtenerComboCCEnKontrol', { empresa: 1 }, false, 'Todos');
            // getValoresMultiples('#cboFiltroCC');
            select = document.getElementById("inputAño");
            var añoActual = new Date();
            var año = añoActual.getFullYear();
            for (i = año; i > (año - 4); i--) {
                option = document.createElement("option");
                option.value = i;
                option.text = i;
                select.appendChild(option);
            }


            inputPeriodoInicial.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker('option', 'showAnim', 'slide')
                .datepicker('setDate', new Date());
            inputPeriodoFinal.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker('option', 'showAnim', 'slide')
                .datepicker('setDate', new Date());

            btnEstadisticasFactorCap.on('click', function () {
                modalEstadisticasFactorCap.modal('show');
            });

            btnSeguimientosAreasRequeridas.on('click', function () {
                modalSeguimientosAreasRequeridas.modal('show');
            });


            //#region Botones Consultar Estadisticas

            botonConsultarPorcentajeCapacitacionesOperativas.click(() => {
                if ($('#cboFiltroDivision').val() == "") {
                    Alert2Error('Debe seleccionar una división.');
                    return;
                }
                if ($('#cboFiltroCC').val() == "") {
                    Alert2Error('Debe seleccionar por lo menos un centro de costo.');
                    return;
                }
                CargarCapacitacionesOperativas();
            });

            botonConsultarCiclosOperativos.click(() => {
                if ($('#cboFiltroDivision').val() == "") {
                    Alert2Error('Debe seleccionar una división.');
                    return;
                }
                if ($('#cboFiltroCC').val() == "") {
                    Alert2Error('Debe seleccionar por lo menos un centro de costo.');
                    return;
                }
                CargarEfectividadCiclos();
            });

            botonConsultarRecorridosOperativos.click(() => {
                if ($('#cboFiltroDivision').val() == "") {
                    Alert2Error('Debe seleccionar una división.');
                    return;
                }
                if ($('#cboFiltroCC').val() == "") {
                    Alert2Error('Debe seleccionar por lo menos un centro de costo.');
                    return;
                }
                CargarRecorridosOperativos();
            });

            botonConsultarImplementacion5s.click(() => {
                if ($('#cboFiltroDivision').val() == "") {
                    Alert2Error('Debe seleccionar una división.');
                    return;
                }
                if ($('#cboFiltroCC').val() == "") {
                    Alert2Error('Debe seleccionar por lo menos un centro de costo.');
                    return;
                }
                CargarImplementacion5s();
            });

            //#endregion
        }
        btnBuscar.on("click", function () {
            if ($('#cboFiltroDivision').val() == "") {
                Alert2Error('Debe seleccionar una división.');
                return;
            }
            if ($('#cboFiltroCC').val() == "") {
                Alert2Error('Debe seleccionar por lo menos un centro de costo.');
                return;
            }

            CargarFactorCapacitacion();
        });
        //#region Factor Capacitacion

        function CargarFactorCapacitacion() {
            let obj = {
                division: cboFiltroDivision.val(),
                listaCentroCosto: getValoresMultiples("#cboFiltroCC"),
                fechaInicial: inputPeriodoInicial.val(),
                fechaFinal: inputPeriodoFinal.val()
            }

            axios.post("CargarFactorCapacitacion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    var DataGraficaFactorCapacitacion = response.data.graficaFactorCapacitacion;
                    var DataGraficaAvanceGlobal = response.data.graficaAvanceGlobal;
                    var listaTablaFactorCap = response.data.listaTablaFactorCapacitacion;
                    var listaGlobalAvance = response.data.listaTablaGlobalAvancesFactorCapacitacion;
                    var listaEstadisticasFC = response.data.lstFC;


                    if (listaTablaFactorCap.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tablaFactorCapacitacion')) {
                            dttablaFactorCapacitacion.clear().destroy();
                            tablaFactorCapacitacion.empty();
                        }
                        inittablaFactorCapacitacion(DataGraficaFactorCapacitacion, listaTablaFactorCap);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tablaFactorCapacitacion')) {
                            dttablaFactorCapacitacion.clear();
                            dttablaFactorCapacitacion.draw();
                        }
                    }

                    if (listaGlobalAvance.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tablaGlobalAvanceFactor')) {
                            dttablaGlobalAvanceFactor.clear().destroy();
                            tablaGlobalAvanceFactor.empty();
                        }
                        inittablaGlobalAvanceFactor(DataGraficaAvanceGlobal, listaGlobalAvance);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tablaGlobalAvanceFactor')) {
                            dttablaGlobalAvanceFactor.clear();
                            dttablaGlobalAvanceFactor.draw();
                        }
                    }

                    if (listaEstadisticasFC.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tablaEstadisticasFactorCapacitacion')) {
                            dttablaEstadisticasFactorCapacitacion.clear().destroy();
                            tablaEstadisticasFactorCapacitacion.empty();
                        }
                        SoloTablatablaEstadisticasFactorCapacitacion(listaEstadisticasFC);
                        inittablaEstadisticasFactorCapacitacion(listaEstadisticasFC);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tablaEstadisticasFactorCapacitacion')) {
                            dttablaEstadisticasFactorCapacitacion.clear();
                            dttablaEstadisticasFactorCapacitacion.draw();
                        }
                    }
                }
            }).catch(error => Alert2Error(error.message));
        };

        //#region grafica y tabla Factor Capacitacion

        function cargarGraficaLinealFactorCapacitacion() {
            Highcharts.chart('graficaFactorCapacitacion', {
                title: {
                    text: 'Factor Capacitacion',
                    align: 'center'
                },
                yAxis: {
                    title: {
                        text: '%'
                    },
                    max: 100,
                    crosshair: true,
                    gridLineWidth: 0,
                },

                xAxis: {
                    accessibility: {
                        rangeDescription: 'Factor Capacitacion'
                    },
                    categories: [],
                    crosshair: true,
                    gridLineWidth: 1,
                },

                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'top'
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td></br>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
                plotOptions: {
                    series: {
                        name: "",
                        label: {
                            connectorAllowed: false
                        },
                        // pointStart: 2010
                    }
                },
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

        function inittablaFactorCapacitacion(grafica, tabla) {
            // let mapColumns = new Map();
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let resultChart = [];

            if (tabla.length > 0) {

                let tempData = new Map();

                tabla.forEach(el => {
                    let indexExists = dynColumns.map(item => item.title).indexOf(el.proyecto);

                    if (indexExists == -1) {
                        dynColumns.push({ title: el.proyecto });
                    }

                    tempData.set(el.proyecto, el.porcentaje);
                });

                dynData.push(tempData);

            }

            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length - 1));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;

                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));
                        }
                    }
                    resultData.push(tempResult);

                    tempDataChart = [];
                    for (let i = 1; i < tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i], "%");

                    }
                    tempChart = { name: tempResult[0], data: tempDataChart }
                    resultChart.push(tempChart);
                }
            }
            dttablaFactorCapacitacion = tablaFactorCapacitacion.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: dynColumns,
                data: resultData,
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                ],
                createdRow: function (row, data, index) {
                    for (let i = 0; i < dynColumns.length; i++) {
                        if (typeof (data[i]) == "number") {
                            $('td', row).eq(i).html(data[i] + " %");
                        }
                    }
                },
            });

            Highcharts.chart('graficaFactorCapacitacion', {
                title: {
                    text: 'Factor Capacitacion',
                    align: 'center'
                },
                yAxis: {
                    title: {
                        text: '%'
                    },
                    max: 100,
                    crosshair: true,
                    gridLineWidth: 0,
                },

                xAxis: {
                    accessibility: {
                        rangeDescription: 'Factor Capacitacion'
                    },
                    categories: grafica.categorias,
                    crosshair: true,
                    gridLineWidth: 1,
                },

                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'top'
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td></br>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
                plotOptions: {
                    series: {
                        name: grafica.serie3, data: grafica.serie3Descripcion,
                        label: {
                            connectorAllowed: false
                        },
                        // pointStart: 2010
                    }
                },
                series: [{
                    name: grafica.serie1Descripcion,
                    color: "#f06203",
                    data: grafica.serie1,
                }, {
                    name: grafica.serie2Descripcion,
                    color: "#040404",
                    data: grafica.serie2
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
        };

        function cargarGraficaLinealGlobalAvanceFactor() {
            Highcharts.chart('graficaGlobalAvanceFactor', {
                title: {
                    text: 'Global Avance Factor Capacitacion Operativa Mineria'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: [],
                    crosshair: true,
                    gridLineWidth: 1,
                    title: {
                        text: null
                    },
                    labels: {
                        step: 1
                    }
                },
                yAxis: {
                    max: 100,
                    min: 0,
                    crosshair: true,
                    gridLineWidth: 0,
                    title: {
                        text: '%',
                        align: 'center'
                    },
                    labels: {
                        overflow: 'justify'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td></br>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true,
                            connectorAllowed: false
                            // format: '{point.y:.2f}%'
                        }
                    }
                },

                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'bottom',
                },
                credits: {
                    enabled: false
                },

            });

        }

        function inittablaGlobalAvanceFactor(grafica, tabla) {
            // let mapColumns = new Map();
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];

            if (tabla.length > 0) {
                dynColumns.push({ title: "" });
                tabla.forEach(el => {

                    let indexExists = dynColumns.map(item => item.title).indexOf(el.mes);

                    if (indexExists == -1) { // NO LO ENCONTRO
                        dynColumns.push({ title: el.mes });
                    }
                    // console.log(dynData.map(e => e.get("")));
                    let indexCC = dynData.map(e => e.get("")).indexOf(el.proyectos);

                    if (indexCC == -1) {
                        let tempData = new Map();
                        tempData.set("", el.proyectos);
                        tempData.set(el.mes, el.porcentajes);
                        dynData.push(tempData);
                    } else {
                        let asdf = dynData[indexCC];
                        asdf.set("", el.proyectos);
                        asdf.set(el.mes, el.porcentajes);

                    }
                });

            };
            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;
                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));
                        }
                    }
                    resultData.push(tempResult);

                    let tempDataChart = [];
                    for (let i = 1; i < tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i]);
                    }

                    tempChart = { name: tempResult[0], data: tempDataChart }
                    resultChart.push(tempChart);

                }

                dttablaGlobalAvanceFactor = tablaGlobalAvanceFactor.DataTable({
                    language: dtDicEsp,
                    destroy: false,
                    paging: false,
                    ordering: false,
                    searching: false,
                    bFilter: false,
                    info: false,
                    columns: dynColumns,
                    data: resultData,
                    columnDefs: [
                        { className: 'dt-center', 'targets': '_all' },
                    ],
                    createdRow: function (row, data, index) {
                        for (let i = 0; i < dynColumns.length; i++) {
                            if (typeof (data[i]) == "number") {
                                $('td', row).eq(i).html(data[i] + " %");
                            }
                        }
                    },
                });

                let serieOptima = [];

                for (let i = 0; i < dynColumns.length - 1; i++) {
                    serieOptima.push(90);
                }

                var chart = Highcharts.chart('graficaGlobalAvanceFactor', {
                    title: {
                        text: 'Global Avance Factor Capacitacion Operativa Mineria',
                        align: 'center'
                    },
                    yAxis: {
                        title: {
                            text: '%'
                        },
                        max: 100,
                        min: 0,
                        crosshair: true,
                        gridLineWidth: 0,
                    },

                    xAxis: {
                        accessibility: {
                            rangeDescription: 'Global Avances'
                        },
                        categories: grafica.categorias,
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
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td></br>' +
                            '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                        footerFormat: '</table>',
                    },
                    series: [{
                        name: 'Objetivo Minimo',
                        color: '#040404',
                        data: serieOptima,
                        type: 'spline',
                        dashStyle: 'Dash',

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

                for (var item of resultChart) {
                    if (item.name != "PROMEDIO MENSUAL") {
                        chart.addSeries(item);

                    }
                }
            }
        }

        //#endregion

        //#region modal Factor Capacitacion


        function SoloTablatablaEstadisticasFactorCapacitacion(tabla) {
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];

            if (tabla.length > 0) {
                dynColumns.push({ title: "" });
                tabla.forEach(el => {

                    let indexExists = dynColumns.map(item => item.title).indexOf(el.proyecto);

                    if (indexExists == -1) { // NO LO ENCONTRO
                        dynColumns.push({ title: el.proyecto });
                    }

                    let index1 = dynData.map(e => e.get("")).indexOf("% Capacitación");
                    let index2 = dynData.map(e => e.get("")).indexOf("Efectividad ciclos");
                    let index3 = dynData.map(e => e.get("")).indexOf("Recorrido");
                    let index4 = dynData.map(e => e.get("")).indexOf("Metodologías 5's");
                    let index5 = dynData.map(e => e.get("")).indexOf("Factor de capacitación operativa");


                    if (index1 == -1) {
                        let tempDataCap = new Map();
                        tempDataCap.set("", "% Capacitación");
                        tempDataCap.set(el.proyecto, el.porcentajeCapacitacion);
                        dynData.push(tempDataCap);
                    } else {
                        let masCC1 = dynData[index1];
                        masCC1.set(el.proyecto, el.porcentajeCapacitacion);
                    }
                    if (index2 == -1) {
                        let tempDataEfe = new Map();
                        tempDataEfe.set("", "Efectividad ciclos");
                        tempDataEfe.set(el.proyecto, el.porcentajeEfectividadCiclos);
                        dynData.push(tempDataEfe);
                    }
                    else {
                        let masCC2 = dynData[index2];
                        masCC2.set(el.proyecto, el.porcentajeEfectividadCiclos);
                    }
                    if (index3 == -1) {
                        let tempDataRec = new Map();
                        tempDataRec.set("", "Recorrido");
                        tempDataRec.set(el.proyecto, el.porcentajeRecorridos);
                        dynData.push(tempDataRec);
                    }
                    else {
                        let masCC3 = dynData[index2];
                        masCC3.set(el.proyecto, el.porcentajeRecorridos);
                    }
                    if (index4 == -1) {
                        let tempData5s = new Map();
                        tempData5s.set("", "Metodologías 5's");
                        tempData5s.set(el.proyecto, el.porcentajeCincoS)
                        dynData.push(tempData5s);
                    }
                    else {
                        let masCC4 = dynData[index4];
                        masCC4.set(el.proyecto, el.porcentajeCincoS);
                    }
                    if (index5 == -1) {
                        let tempDatafac = new Map();
                        tempDatafac.set("", "Factor de capacitación operativa");
                        tempDatafac.set(el.proyecto, el.factorCapacitacionCentroCosto);
                        dynData.push(tempDatafac);
                    }
                    else {
                        let masCC5 = dynData[index5];
                        masCC5.set(el.proyecto, el.factorCapacitacionCentroCosto);
                    }
                });
            };
            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;
                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));
                        }
                    }
                    resultData.push(tempResult);

                    let tempDataChart = [];
                    for (let i = 1; i <= tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i]);
                    }
                    tempChart = { name: tempResult[0], data: tempDataChart }
                    resultChart.push(tempChart);

                }
            }
            dttablaEstadisticasFactorCapacitacion = tablaEstadisticasFactorCapacitacion.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                autoAjust: false,
                columns: dynColumns,
                data: resultData,
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },

                    // { width: '15%', targets: [1] },
                    // { width: '8%', targets: [2, 5, 6] },
                    // { width: '10%', targets: [7] },
                ],
                fixedColumns: true,
                createdRow: function (row, data, index) {
                    for (let i = 0; i < dynColumns.length; i++) {
                        if (typeof (data[i]) == "number") {
                            $('td', row).eq(i).html(data[i] + " %");
                        }
                    }
                },
            });

        }

        function inittablaEstadisticasFactorCapacitacion(tabla) {

            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];

            if (tabla.length > 0) {
                dynColumns.push({ title: "PROYECTOS" });
                tabla.forEach(el => {

                    let indexExists = dynColumns.map(item => item.title).indexOf(el.proyecto);

                    if (indexExists == -1) { // NO LO ENCONTRO
                        dynColumns.push({ title: el.proyecto });
                    }

                    let index1 = dynData.map(e => e.get("")).indexOf("% Capacitación");
                    let index2 = dynData.map(e => e.get("")).indexOf("Efectividad ciclos");
                    let index3 = dynData.map(e => e.get("")).indexOf("Recorrido");
                    let index4 = dynData.map(e => e.get("")).indexOf("Metodologías 5's");
                    // let index5 = dynData.map(e => e.get("")).indexOf("Factor de capacitación operativa");


                    if (index1 == -1) {
                        let tempDataCap = new Map();
                        tempDataCap.set("", "% Capacitación");
                        tempDataCap.set(el.proyecto, el.porcentajeCapacitacion);
                        dynData.push(tempDataCap);
                    } else {
                        let masCC1 = dynData[index1];
                        masCC1.set(el.proyecto, el.porcentajeCapacitacion);
                    }
                    if (index2 == -1) {
                        let tempDataEfe = new Map();
                        tempDataEfe.set("", "Efectividad ciclos");
                        tempDataEfe.set(el.proyecto, el.porcentajeEfectividadCiclos);
                        dynData.push(tempDataEfe);
                    }
                    else {
                        let masCC2 = dynData[index2];
                        masCC2.set(el.proyecto, el.porcentajeEfectividadCiclos);
                    }
                    if (index3 == -1) {
                        let tempDataRec = new Map();
                        tempDataRec.set("", "Recorrido");
                        tempDataRec.set(el.proyecto, el.porcentajeRecorridos);
                        dynData.push(tempDataRec);
                    }
                    else {
                        let masCC3 = dynData[index2];
                        masCC3.set(el.proyecto, el.porcentajeRecorridos);
                    }
                    if (index4 == -1) {
                        let tempData5s = new Map();
                        tempData5s.set("", "Metodologías 5's");
                        tempData5s.set(el.proyecto, el.porcentajeCincoS)
                        dynData.push(tempData5s);
                    }
                    else {
                        let masCC4 = dynData[index4];
                        masCC4.set(el.proyecto, el.porcentajeCincoS);
                    }
                    // if (index5 == -1) {
                    //     let tempDatafac = new Map();
                    //     tempDatafac.set("", "Factor de capacitación operativa");
                    //     tempDatafac.set(el.proyecto, el.factorCapacitacionCentroCosto);
                    //     dynData.push(tempDatafac);
                    // }
                    // else {
                    //     let masCC5 = dynData[index5];
                    //     masCC5.set(el.proyecto, el.factorCapacitacionCentroCosto);
                    // }
                });
            };
            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;
                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));
                        }
                    }
                    resultData.push(tempResult);

                    let tempDataChart = [];
                    for (let i = 1; i <= tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i]);
                    }
                    tempChart = { name: item.get(''), data: tempDataChart }
                    resultChart.push(tempChart);

                }
            }
            // dttablaEstadisticasFactorCapacitacion = tablaEstadisticasFactorCapacitacion.DataTable({
            //     language: dtDicEsp,
            //     destroy: false,
            //     paging: false,
            //     ordering: false,
            //     searching: false,
            //     bFilter: false,
            //     info: false,
            //     columns: dynColumns,
            //     data: resultData,
            //     columnDefs: [
            //         { className: 'dt-center', 'targets': '_all' },

            //         // { width: '15%', targets: [1] },
            //         // { width: '8%', targets: [2, 5, 6] },
            //         // { width: '10%', targets: [7] },
            //     ],
            //     fixedColumns: true,
            //     createdRow: function (row, data, index) {
            //         for (let i = 0; i < dynColumns.length; i++) {
            //             if (typeof (data[i]) == "number") {
            //                 $('td', row).eq(i).html(data[i] + " %");
            //             }
            //         }
            //     },
            // });

            let serieOptima = [];

            for (let i = 0; i < dynColumns.length - 1; i++) {
                serieOptima.push(90);
            }

            var chart = Highcharts.chart('graficaEstadisticaFactorCapacitacion', {
                chart: {
                    type: 'column'
                },
                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'top'
                },
                title: {
                    text: '% Capacitaciones Operativas',
                    align: 'center'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: columnsChart,
                    crosshair: true
                },
                yAxis: {
                    max: 100,
                    title: {
                        text: '%'
                    },
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
                series: [{
                    name: 'Objetivo Minimo',
                    color: '#040404',
                    data: serieOptima,
                    type: 'spline'
                    //AQUI NO DASH
                }],
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },

            });
            for (var item of resultChart) {
                if (item.name != "PROMEDIO MENSUAL") {
                    chart.addSeries(item);

                }
            }
        }

        function cargarGraficaEstadisticaFactorCapacitacion() {
            Highcharts.chart('graficaEstadisticaFactorCapacitacion', {
                title: {
                    text: 'Factor Capacitación',
                    align: 'center'
                },
                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'top'
                },
                yAxis: {
                    title: {
                        text: '%'
                    },
                    max: 100,
                    crosshair: true,
                    gridLineWidth: 0,
                },

                xAxis: {
                    accessibility: {
                        rangeDescription: 'Factor Capacitacion'
                    },
                    categories: datos.categorias,
                    crosshair: true,
                    gridLineWidth: 1,
                },

                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'top'
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td></br>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
                plotOptions: {
                    series: {
                        name: datos.serie3, data: datos.serie3Descripcion,
                        label: {
                            connectorAllowed: false
                        },
                        // pointStart: 2010
                    }
                },
                // series: [{
                //     name: '% Capacitacion',
                //     type: 'column',
                //     color: "#ed7d31",
                //     data: datos.serie1,
                // },
                // {
                //     name: 'Efectividad Ciclos',
                //     type: 'column',
                //     data: datos.serie1,
                // },
                // {
                //     name: 'Recorridos',
                //     type: 'column',
                //     color: "#2e75b6",
                //     data: datos.serie1,
                // },
                // {
                //     name: 'Metodologias 5s',
                //     type: 'column',
                //     color: "#00b050",
                //     data: datos.serie1,
                // }, {
                //     name: datos.serie2Descripcion,
                //     color: "#040404",
                //     data: datos.serie2
                // }, {
                //     name: 'Promedio minimo',
                //     type: 'column',
                //     color: "#ffc000",
                //     data: [0],
                // },],
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

        //#endregion

        //#endregion

        //#region Estadisticas Carga de Informacion

        function CargarCapacitacionesOperativas() {
            let obj = {
                division: cboFiltroDivision.val(),
                listaCentroCosto: getValoresMultiples("#cboFiltroCC"),
                anio: inputAño.val(),
                seccion: 1
            }

            axios.post("CargarEstadisticas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    var dataGraficaCapOperativas = response.data.graficaSeccionEstadisticasCapacitacion;
                    var listaTablaCapOperativas = response.data.listaTablasEstadisticasFactorCapacitacion;
                    if (listaTablaCapOperativas.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tablaPorcentajeCapacitacionesOperativas')) {
                            dttablaPorcentajeCapacitacionesOperativas.clear().destroy();
                            tablaPorcentajeCapacitacionesOperativas.empty();
                        }
                        inittablaPorcentajeCapacitacionesOperativas(dataGraficaCapOperativas, listaTablaCapOperativas);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tablaPorcentajeCapacitacionesOperativas')) {
                            dttablaPorcentajeCapacitacionesOperativas.clear();
                            dttablaPorcentajeCapacitacionesOperativas.draw();
                        }
                    }
                }
            }).catch(error => Alert2Error(error.message));
        }

        function CargarEfectividadCiclos() {
            let obj = {
                division: cboFiltroDivision.val(),
                listaCentroCosto: getValoresMultiples("#cboFiltroCC"),
                anio: inputAño.val(),
                seccion: 2
            }

            axios.post("CargarEstadisticas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    var dataGraficaEfectividadCiclos = response.data.graficaSeccionEstadisticasEfectividadCiclos;
                    var listaTablaEfectividadCiclos = response.data.listaTablaEstadisticasEfectividadCiclos;
                    if (listaTablaEfectividadCiclos.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tablaEfectividadCiclosOperativos')) {
                            dttablaEfectividadCiclosOperativos.clear().destroy();
                            tablaEfectividadCiclosOperativos.empty();
                        }
                        inittablaEfectividadCiclosOperativos(listaTablaEfectividadCiclos, dataGraficaEfectividadCiclos);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tablaEfectividadCiclosOperativos')) {
                            dttablaEfectividadCiclosOperativos.clear();
                            dttablaEfectividadCiclosOperativos.draw();
                        }
                    }

                    var dataGraficaPorcentajeEfectividadCiclos = response.data.graficaSeccionEstadisticasEfectividadCiclos;
                    var listaTablaPorcentajeEfectividadCiclos = response.data.listaTablaEstadisticasEfectividadCiclos;
                    if (listaTablaPorcentajeEfectividadCiclos.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tablaPorcentajeCiclosOperativos')) {
                            dttablaPorcentajeCiclosOperativos.clear().destroy();
                            tablaPorcentajeCiclosOperativos.empty();
                        }
                        inittablaPorcentajeCiclosOperativos(listaTablaPorcentajeEfectividadCiclos, dataGraficaPorcentajeEfectividadCiclos);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tablaPorcentajeCiclosOperativos')) {
                            dttablaPorcentajeCiclosOperativos.clear();
                            dttablaPorcentajeCiclosOperativos.draw();
                        }
                    }


                    var listaSeguimientoAcciones = response.data.listaTablasSeguimientosAcciones;
                    var listaDatosSeguimientoPropuestas = response.data.listaTablasSeguimientosPropuestas;
                    var listaDatosCumplimientoMensual = response.data.listaTablasCumplimientoMensual;
                    var listaDatosConocimientoCiclos = response.data.listaTablasConocimientoCicloTrabajo;

                    if (listaSeguimientoAcciones.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tablaSeguimientoAcciones')) {
                            dttablaSeguimientoAcciones.clear().destroy();
                        }
                        inittablaSeguimientoAcciones(listaSeguimientoAcciones);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tablaSeguimientoAcciones')) {
                            dttablaSeguimientoAcciones.clear();
                            dttablaSeguimientoAcciones.draw();
                        }
                    }

                    if (listaDatosSeguimientoPropuestas.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tablaSeguimientoPropuestas')) {
                            dttablaSeguimientoPropuestas.clear().destroy();

                        }
                        inittablaSeguimientoPropuestas(listaDatosSeguimientoPropuestas);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tablaSeguimientoPropuestas')) {
                            dttablaSeguimientoPropuestas.clear();
                            dttablaSeguimientoPropuestas.draw();
                        }
                    }

                    if (listaDatosCumplimientoMensual.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tablaCumplimientoCiclosMensuales')) {
                            dttablaCumplimientoCiclosMensuales.clear().destroy();

                        }
                        inittablaCumplimientoCiclosMensuales(listaDatosCumplimientoMensual);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tablaCumplimientoCiclosMensuales')) {
                            dttablaCumplimientoCiclosMensuales.clear();
                            dttablaCumplimientoCiclosMensuales.draw();
                        }
                    }

                    if (listaDatosConocimientoCiclos.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tablaPromedioConocimiento')) {
                            dttablaPromedioConocimiento.clear().destroy();

                        }
                        inittablaPromedioConocimiento(listaDatosConocimientoCiclos);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tablaPromedioConocimiento')) {
                            dttablaPromedioConocimiento.clear();
                            dttablaPromedioConocimiento.draw();
                        }
                    }

                }
            }).catch(error => Alert2Error(error.message));
        }

        function CargarRecorridosOperativos() {
            let obj = {
                division: cboFiltroDivision.val(),
                listaCentroCosto: getValoresMultiples("#cboFiltroCC"),
                anio: inputAño.val(),
                seccion: 3
            }
            axios.post("CargarEstadisticas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    var dataGraficaPorcentajeRecorridosOp = response.data.graficaSeccionEstadisticasRecorridos;
                    var listaTablaPorcentajeRecorridosOp = response.data.listaTablaEstadisticasRecorridos;
                    if (listaTablaPorcentajeRecorridosOp.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tablaPorcentajeRecorridosOperativos')) {
                            dttablaPorcentajeRecorridosOperativos.clear().destroy();
                            tablaPorcentajeRecorridosOperativos.empty();
                        }
                        inittablaPorcentajeRecorridosOperativos(listaTablaPorcentajeRecorridosOp, dataGraficaPorcentajeRecorridosOp);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tablaPorcentajeRecorridosOperativos')) {
                            dttablaPorcentajeRecorridosOperativos.clear();
                            dttablaPorcentajeRecorridosOperativos.draw();
                            cargarGraficaBarraPorcentajeRecorridosOperativos();
                        }
                    }
                    // cargarGraficaBarraPorcentajeRecorridosOperativos(response.data.graficaSeccionEstadisticas);
                    // inittablaPorcentajeRecorridosOperativos(response.data.graficaSeccionEstadisticas, response.data.listaTablasEstadisticasFactorCapacitacion);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function CargarImplementacion5s() {
            let obj = {
                division: cboFiltroDivision.val(),
                listaCentroCosto: getValoresMultiples("#cboFiltroCC"),
                anio: inputAño.val(),
                seccion: 4
            }

            axios.post("CargarEstadisticas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    var dataGraficaImplementacion5s = response.data.graficaSeccionEstadisticasImplementacion5s;
                    var listaTablaImplementacion5s = response.data.listaTablaEstadisticasImplementacion5s;
                    if (listaTablaImplementacion5s.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tablaPorcentajeImplementacion5s')) {
                            dttablaPorcentajeImplementacion5s.clear().destroy();
                            tablaPorcentajeImplementacion5s.empty();
                        }
                        inittablaPorcentajeImplementacion5s(listaTablaImplementacion5s, dataGraficaImplementacion5s);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tablaPorcentajeImplementacion5s')) {
                            dttablaPorcentajeImplementacion5s.clear();
                            dttablaPorcentajeImplementacion5s.draw();
                            cargarGraficaBarraPorcentajeImplementacion5s();
                        }
                    }
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#region Capacitaciones Operativas

        function cargarGraficaLinealPorcentajeCapacitacionesOperativas() {
            var chart = Highcharts.chart('graficaPorcentajeCapacitacionesOperativas', {
                title: {
                    text: 'Factor Capacitacion',
                    align: 'center'
                },
                yAxis: {
                    title: {
                        text: '%'
                    },
                    max: 100,
                    crosshair: true,
                    gridLineWidth: 0,
                },
                xAxis: {
                    accessibility: {
                        rangeDescription: 'Factor Capacitacion'
                    },
                    crosshair: true,
                    gridLineWidth: 1,
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td></br>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'bottom'
                },
                plotOptions: {
                    series: {
                        name: "",
                        label: {
                            connectorAllowed: false
                        },
                        // pointStart: 2010
                    }
                },
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

        function inittablaPorcentajeCapacitacionesOperativas(grafica, tabla) {
            // let mapColumns = new Map();
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];
            let ObjetivoMinimo = [];


            if (tabla.length > 0) {
                dynColumns.push({ title: "PROYECTOS" });
                tabla.forEach(el => {

                    let indexExists = dynColumns.map(item => item.title).indexOf(el.mes);

                    if (indexExists == -1) { // NO LO ENCONTRO
                        dynColumns.push({ title: el.mes });
                    }
                    // console.log(dynData.map(e => e.get("")));
                    let indexCC = dynData.map(e => e.get("")).indexOf(el.proyectos);

                    if (indexCC == -1) {
                        let tempData = new Map();
                        tempData.set("", el.proyectos);
                        tempData.set(el.mes, el.porcentajes);
                        dynData.push(tempData);
                    } else {
                        let asdf = dynData[indexCC];
                        asdf.set("", el.proyectos);
                        asdf.set(el.mes, el.porcentajes);

                    }
                });

            };
            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length - 1));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;
                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));
                        }
                    }
                    resultData.push(tempResult);

                    let tempDataChart = [];
                    for (let i = 1; i < tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i]);
                    }

                    tempChart = { name: tempResult[0], data: tempDataChart }
                    resultChart.push(tempChart);

                }

                dttablaPorcentajeCapacitacionesOperativas = tablaPorcentajeCapacitacionesOperativas.DataTable({
                    language: dtDicEsp,
                    destroy: false,
                    paging: false,
                    ordering: false,
                    searching: false,
                    bFilter: false,
                    info: false,
                    scrollX: '200px',
                    scrollCollapse: false,
                    columns: dynColumns,
                    data: resultData,
                    columnDefs: [
                        { className: 'dt-center', 'targets': '_all' },
                    ],
                    createdRow: function (row, data, index) {
                        for (let i = 0; i < dynColumns.length; i++) {
                            if (typeof (data[i]) == "number") {
                                $('td', row).eq(i).html(data[i] + " %");
                            }
                        }
                    },
                });

                let serieOptima = [];

                for (let i = 0; i < dynColumns.length - 2; i++) {
                    serieOptima.push(90);
                }

                var chart = Highcharts.chart('graficaPorcentajeCapacitacionesOperativas', {
                    title: {
                        text: 'Global Avance Factor Capacitacion Operativa Mineria',
                        align: 'center'
                    },
                    yAxis: {
                        title: {
                            text: '%'
                        },
                        min: 0,
                        crosshair: true,
                        gridLineWidth: 0,
                    },
                    xAxis: {
                        accessibility: {
                            rangeDescription: 'Global avances'
                        },
                        categories: grafica.categorias,
                        crosshair: true,
                        gridLineWidth: 1,
                    },
                    //legend: {
                    //    layout: 'vertical',
                    //    align: 'right',
                    //    verticalAlign: 'middle'
                    //},
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'top'
                    },
                    plotOptions: {
                        series: {
                            label: {
                                connectorAllowed: false
                            },
                        }
                    },
                    series: [{
                        name: 'Objetivo Minimo',
                        color: '#040404',
                        data: serieOptima,
                        type: 'spline',
                        dashStyle: 'Dash',
                    }],
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td></br>' +
                            '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                        footerFormat: '</table>',
                    },
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

                for (var item of resultChart) {
                    if (item.name != "PROMEDIO MENSUAL") {
                        chart.addSeries(item);

                    }
                }
            }
            // // let mapColumns = new Map();
            // let dynColumns = [];
            // let dynData = [];
            // let resultData = [];
            // let columnsChart = [];
            // let resultChart = [];


            // if (tabla.length > 0) {
            //     dynColumns.push({ title: "PROYECTOS" });
            //     tabla.forEach(el => {

            //         let indexExists = dynColumns.map(item => item.title).indexOf(el.mes);

            //         if (indexExists == -1) { // NO LO ENCONTRO
            //             dynColumns.push({ title: el.mes });
            //         }
            //         // console.log(dynData.map(e => e.get("")));
            //         let indexCC = dynData.map(e => e.get("PROYECTOS")).indexOf(el.proyectos);

            //         if (indexCC == -1) {
            //             let tempData = new Map();
            //             tempData.set("PROYECTOS", el.proyectos);
            //             tempData.set(el.mes, el.porcentajes);
            //             dynData.push(tempData);
            //         } else {
            //             let asdf = dynData[indexCC];
            //             asdf.set("PROYECTOS", el.proyectos);
            //             asdf.set(el.mes, el.porcentajes);

            //         }
            //     });

            // };
            // if (tabla.length > 0) {
            //     let colData = dynColumns.map(e => e.title);
            //     columnsChart = colData.slice(1, (dynColumns.length - 1));

            //     for (var item of dynData) {
            //         let tempResult = [];
            //         let tempChart;
            //         for (var col of colData) {
            //             if (!item.has(col)) {
            //                 tempResult.push(0);

            //             } else {
            //                 tempResult.push(item.get(col));
            //             }
            //         }
            //         resultData.push(tempResult);

            //         let tempDataChart = [];
            //         for (let i = 1; i < tempResult.length - 1; i++) {
            //             tempDataChart.push(tempResult[i]);
            //         }

            //         tempChart = { name: tempResult[0], data: tempDataChart }
            //         resultChart.push(tempChart);

            //     }

            //     dttablaPorcentajeCapacitacionesOperativas = tablaPorcentajeCapacitacionesOperativas.DataTable({
            //         language: dtDicEsp,
            //         destroy: false,
            //         paging: false,
            //         ordering: false,
            //         searching: false,
            //         bFilter: false,
            //         info: false,
            //         columns: dynColumns,
            //         data: resultData,
            //         columnDefs: [
            //             { className: 'dt-center', 'targets': '_all' },
            //         ],
            //     });

            //     let serieOptima = [];

            //     for (let i = 0; i < dynColumns.length - 1; i++) {
            //         serieOptima.push(90);
            //     }

            //     var chart = Highcharts.chart('graficaPorcentajeCapacitacionesOperativas', {
            //         title: {
            //             text: 'Global Avance Factor Capacitacion Operativa Mineria',
            //             align: 'center'
            //         },
            //         yAxis: {
            //             title: {
            //                 text: '%'
            //             },
            //             min: 0,
            //             crosshair: true,
            //             gridLineWidth: 0,
            //         },
            //         xAxis: {
            //             accessibility: {
            //                 rangeDescription: 'Global avances'
            //             },
            //             categories: columnsChart,
            //             crosshair: true,
            //             gridLineWidth: 1,
            //         },
            //         // legend: {
            //         //     layout: 'horizontal',
            //         //     align: 'right',
            //         //     verticalAlign: 'bottom'
            //         // },
            //         plotOptions: {
            //             series: {
            //                 label: {
            //                     connectorAllowed: false
            //                 },
            //             }
            //         },
            //         series: [{
            //             name: 'Objetivo Minimo',
            //             color: '#040404',
            //             data: serieOptima
            //         }],
            //         responsive: {
            //             rules: [{
            //                 condition: {
            //                     maxWidth: 500
            //                 },
            //                 chartOptions: {
            //                     legend: {
            //                         layout: 'horizontal',
            //                         align: 'center',
            //                         verticalAlign: 'bottom'
            //                     }
            //                 }
            //             }]
            //         },
            //         credits: { enabled: false }
            //     });

            //     for (var item of resultChart) {
            //         chart.addSeries(item);
            //     }
            // }
        }

        //#endregion

        //#region Efectividad Ciclos de Trabajo

        function inittablaEfectividadCiclosOperativos(tabla, grafica) {

            // let mapColumns = new Map();
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];

            if (tabla.length > 0) {
                dynColumns.push({ title: "PROYECTOS" });
                tabla.forEach(el => {
                    // if (el.proyectos == "PROMEDIO MENSUAL" && el.mes != "PROMEDIO MENSUAL") {
                    let indexExists = dynColumns.map(item => item.title).indexOf(el.mes);

                    if (indexExists == -1) { // NO LO ENCONTRO
                        dynColumns.push({ title: el.mes });
                    }

                    console.log(dynData.map(e => e.get("")));
                    let indexCC = dynData.map(e => e.get("")).indexOf("EFECTIVIDAD DE CICLOS");

                    if (indexCC == -1) {
                        let tempData = new Map();
                        tempData.set("", "EFECTIVIDAD DE CICLOS");

                        tempData.set(el.mes, el.porcentajes);
                        dynData.push(tempData);
                    } else {
                        let asdf = dynData[indexCC];
                        asdf.set("", "EFECTIVIDAD DE CICLOS");
                        asdf.set(el.mes, el.porcentajes);

                    }
                    // }

                });

            };
            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length - 1));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;

                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));

                        }
                    }
                    resultData.push(tempResult);

                    tempDataChart = [];
                    for (let i = 1; i < tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i]);

                    }
                    tempChart = { name: 'Efectividad de Ciclos', data: tempDataChart }
                    resultChart.push(tempChart);
                }
            }

            dttablaEfectividadCiclosOperativos = tablaEfectividadCiclosOperativos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                // scrollX: '200px',
                // scrollCollapse: false,
                columns: dynColumns,
                data: resultData,
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                ],
                createdRow: function (row, data, index) {
                    for (let i = 0; i < dynColumns.length; i++) {
                        if (typeof (data[i]) == "number") {
                            $('td', row).eq(i).html(data[i] + " %");
                        }
                    }
                },
            });

            let serieOptima = [];

            for (let i = 0; i < dynColumns.length - 2; i++) {
                serieOptima.push(90);
            }

            var chart = Highcharts.chart('graficaEfectividadCiclosOperativos', {
                title: {
                    text: ' ',
                    align: 'center'
                },
                yAxis: {
                    title: {
                        text: '%'
                    },
                    // max: 100,
                    min: 0,
                    crosshair: true,
                    gridLineWidth: 0,
                },
                xAxis: {
                    accessibility: {
                        rangeDescription: 'Factor Capacitacion'
                    },
                    categories: columnsChart,
                    crosshair: true,
                    gridLineWidth: 1,
                },
                //legend: {
                //    layout: 'vertical',
                //    align: 'right',
                //    verticalAlign: 'middle'
                //},
                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'top'
                },
                plotOptions: {
                    series: {
                        name: "",
                        label: {
                            connectorAllowed: false
                        },
                        // pointStart: 2010
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Objetivo Minimo',
                    color: '#040404',
                    data: serieOptima,
                    type: 'spline',
                    dashStyle: 'Dash',
                }],
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td></br>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
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
            });
            for (var item of resultChart) {
                if (item.name != "PROMEDIO MENSUAL") {
                    chart.addSeries(item);

                }
            }
        }

        function cargarGraficaLinealEfectividadCiclosOperativos() {
            Highcharts.chart('graficaEfectividadCiclosOperativos', {
                title: {
                    text: '',
                    align: 'center'
                },
                yAxis: {
                    title: {
                        text: '%'
                    },
                    // max: 100,
                    min: 0,
                    crosshair: true,
                    gridLineWidth: 0,
                },

                xAxis: {
                    accessibility: {
                        rangeDescription: ''
                    },
                    categories: [],
                    crosshair: true,
                    gridLineWidth: 1,
                },

                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'top'
                },

                plotOptions: {
                    series: {
                        name: [],
                        label: {
                            connectorAllowed: false
                        },
                        // pointStart: 2010
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td></br>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
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

        function inittablaPorcentajeCiclosOperativos(tabla) {

            // let mapColumns = new Map();
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];

            if (tabla.length > 0) {
                dynColumns.push({ title: "PROYECTOS" });
                tabla.forEach(el => {

                    let indexExists = dynColumns.map(item => item.title).indexOf(el.mes);

                    if (indexExists == -1) { // NO LO ENCONTRO
                        dynColumns.push({ title: el.mes });
                    }

                    console.log(dynData.map(e => e.get("PROYECTOS")));
                    let indexCC = dynData.map(e => e.get("PROYECTOS")).indexOf(el.proyectos);

                    if (indexCC == -1) {
                        let tempData = new Map();
                        tempData.set("PROYECTOS", el.proyectos);

                        tempData.set(el.mes, el.porcentajes);
                        dynData.push(tempData);
                    } else {
                        let asdf = dynData[indexCC];
                        asdf.set("PROYECTOS", el.proyectos);
                        asdf.set(el.mes, el.porcentajes);

                    }
                });

            };
            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length - 1));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;

                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));

                        }
                    }
                    resultData.push(tempResult);

                    tempDataChart = [];
                    for (let i = 1; i < tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i]);

                    }
                    tempChart = { name: tempResult[0], data: tempDataChart }
                    resultChart.push(tempChart);
                }
            }

            dttablaPorcentajeCiclosOperativos = tablaPorcentajeCiclosOperativos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                // scrollX: '200px',
                // scrollCollapse: false,
                columns: dynColumns,
                data: resultData,
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
                createdRow: function (row, data, index) {
                    for (let i = 0; i < dynColumns.length; i++) {
                        if (typeof (data[i]) == "number") {
                            $('td', row).eq(i).html(data[i] + " %");
                        }
                    }
                },

            });

            let serieOptima = [];

            for (let i = 0; i < dynColumns.length - 2; i++) {
                serieOptima.push(90);
            }

            var chart = Highcharts.chart('graficaPorcentajeCiclosOperativos', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Efectividad de Ciclos',
                    align: 'center'
                },
                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'top'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: columnsChart,
                    crosshair: true
                },
                yAxis: {
                    max: 100,
                    title: {
                        text: '%'
                    },
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
                series: [{
                    name: 'Objetivo Minimo',
                    color: '#040404',
                    data: serieOptima,
                    type: 'spline',
                    dashStyle: 'Dash',
                }],
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
            });
            for (var item of resultChart) {
                if (item.name != "PROMEDIO MENSUAL") {
                    chart.addSeries(item);

                }
            }
        }

        function cargarGraficaBarraPorcentajeCiclosOperativos() {
            Highcharts.chart('graficaPorcentajeCiclosOperativos', {
                title: {
                    text: ''
                },
                yAxis: {
                    title: {
                        text: '%'
                    },
                    max: 100,
                    crosshair: true,
                    gridLineWidth: 0,
                },

                xAxis: {
                    accessibility: {
                        rangeDescription: ''
                    },
                    categories: [],
                    crosshair: true,
                    gridLineWidth: 1,
                },

                //legend: {
                //    layout: 'vertical',
                //    align: 'right',
                //    verticalAlign: 'middle'
                //},
                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'top'
                },

                plotOptions: {
                    series: {
                        name: [],
                        label: {
                            connectorAllowed: false
                        },
                        //pointStart: 2010
                    }
                },

                series: [{
                    name: 'HERRADURA',
                    type: 'column',
                    color: "#ed7d31",
                    data: datos.serie1,
                },
                {
                    name: 'NOCHE BUENA',
                    type: 'column',
                    color: "#7030a0",
                    data: datos.serie1,
                },
                {
                    name: 'COLORADA',
                    type: 'column',
                    color: "#a5a5a5",
                    data: datos.serie1,
                },
                {
                    name: 'SAN AGUSTIN',
                    type: 'column',
                    color: "#ffc000",
                    data: datos.serie1,
                },
                {
                    name: 'MULATOS',
                    type: 'column',
                    color: "#5b9bd5",
                    data: datos.serie1,
                },
                {
                    name: 'JUANICIPIO',
                    type: 'column',
                    color: "#70ad47",
                    data: datos.serie1,
                }, {
                    name: 'OBJETIVO MENSUAL',
                    color: "#040404",
                    data: datos.serie2
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
        //#endregion

        //#region  Modal Efectividad Ciclos de Trabajo 

        function inittablaSeguimientoAcciones(tabla) {
            // let mapColumns = new Map();
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];

            if (tabla.length > 0) {
                dynColumns.push({ title: "PROYECTOS" });
                tabla.forEach(el => {
                    let indexExists = dynColumns.map(item => item.title).indexOf(el.mes);

                    if (indexExists == -1) { // NO LO ENCONTRO
                        dynColumns.push({ title: el.mes });
                    }

                    console.log(dynData.map(e => e.get("PROYECTOS")));
                    let indexCC = dynData.map(e => e.get("PROYECTOS")).indexOf(el.proyectos);

                    if (indexCC == -1) {
                        let tempData = new Map();
                        tempData.set("PROYECTOS", el.proyectos);
                        tempData.set(el.mes, el.porcentajes);
                        dynData.push(tempData);
                    } else {
                        let asdf = dynData[indexCC];
                        asdf.set("PROYECTOS", el.proyectos);
                        asdf.set(el.mes, el.porcentajes);
                    }
                });

            };
            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length - 1));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;

                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));

                        }
                    }
                    resultData.push(tempResult);

                    tempDataChart = [];
                    for (let i = 1; i < tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i]);

                    }
                    tempChart = { name: tempResult[0], data: tempDataChart }
                    resultChart.push(tempChart);
                }
            }

            dttablaSeguimientoAcciones = tablaSeguimientoAcciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                // scrollX: '200px',
                // scrollCollapse: false,
                columns: dynColumns,
                data: resultData,
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
                createdRow: function (row, data, index) {
                    for (let i = 0; i < dynColumns.length; i++) {
                        if (typeof (data[i]) == "number") {
                            $('td', row).eq(i).html(data[i] + " %");
                        }
                    }
                },
            });

        }

        function inittablaSeguimientoPropuestas(tabla) {
            // let mapColumns = new Map();
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];

            if (tabla.length > 0) {
                dynColumns.push({ title: "PROYECTOS" });
                tabla.forEach(el => {

                    let indexExists = dynColumns.map(item => item.title).indexOf(el.mes);

                    if (indexExists == -1) { // NO LO ENCONTRO
                        dynColumns.push({ title: el.mes });
                    }

                    console.log(dynData.map(e => e.get("PROYECTOS")));
                    let indexCC = dynData.map(e => e.get("PROYECTOS")).indexOf(el.proyectos);

                    if (indexCC == -1) {
                        let tempData = new Map();
                        tempData.set("PROYECTOS", el.proyectos);

                        tempData.set(el.mes, el.porcentajes);
                        dynData.push(tempData);
                    } else {
                        let asdf = dynData[indexCC];
                        asdf.set("PROYECTOS", el.proyectos);
                        asdf.set(el.mes, el.porcentajes);

                    }
                });

            };
            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length - 1));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;

                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));

                        }
                    }
                    resultData.push(tempResult);

                    tempDataChart = [];
                    for (let i = 1; i < tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i]);

                    }
                    tempChart = { name: tempResult[0], data: tempDataChart }
                    resultChart.push(tempChart);
                }
            }

            dttablaSeguimientoPropuestas = tablaSeguimientoPropuestas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                // scrollX: '200px',
                // scrollCollapse: false,
                columns: dynColumns,
                data: resultData,
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
                createdRow: function (row, data, index) {
                    for (let i = 0; i < dynColumns.length; i++) {
                        if (typeof (data[i]) == "number") {
                            $('td', row).eq(i).html(data[i] + " %");
                        }
                    }
                },
            });

        }

        function inittablaCumplimientoCiclosMensuales(tabla) {
            // let mapColumns = new Map();
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];

            if (tabla.length > 0) {
                dynColumns.push({ title: "PROYECTOS" });
                tabla.forEach(el => {

                    let indexExists = dynColumns.map(item => item.title).indexOf(el.mes);

                    if (indexExists == -1) { // NO LO ENCONTRO
                        dynColumns.push({ title: el.mes });
                    }

                    console.log(dynData.map(e => e.get("PROYECTOS")));
                    let indexCC = dynData.map(e => e.get("PROYECTOS")).indexOf(el.proyectos);

                    if (indexCC == -1) {
                        let tempData = new Map();
                        tempData.set("PROYECTOS", el.proyectos);

                        tempData.set(el.mes, el.porcentajes);
                        dynData.push(tempData);
                    } else {
                        let asdf = dynData[indexCC];
                        asdf.set("PROYECTOS", el.proyectos);
                        asdf.set(el.mes, el.porcentajes);

                    }
                });

            };
            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length - 1));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;

                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));

                        }
                    }
                    resultData.push(tempResult);

                    tempDataChart = [];
                    for (let i = 1; i < tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i]);

                    }
                    tempChart = { name: tempResult[0], data: tempDataChart }
                    resultChart.push(tempChart);
                }
            }

            dttablaCumplimientoCiclosMensuales = tablaCumplimientoCiclosMensuales.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                // scrollX: '200px',
                // scrollCollapse: false,
                columns: dynColumns,
                data: resultData,
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
                createdRow: function (row, data, index) {
                    for (let i = 0; i < dynColumns.length; i++) {
                        if (typeof (data[i]) == "number") {
                            $('td', row).eq(i).html(data[i] + " %");
                        }
                    }
                },
            });

        }

        function inittablaPromedioConocimiento(tabla) {
            // let mapColumns = new Map();
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];

            if (tabla.length > 0) {
                dynColumns.push({ title: "PROYECTOS" });
                tabla.forEach(el => {

                    let indexExists = dynColumns.map(item => item.title).indexOf(el.mes);

                    if (indexExists == -1) { // NO LO ENCONTRO
                        dynColumns.push({ title: el.mes });
                    }

                    console.log(dynData.map(e => e.get("PROYECTOS")));
                    let indexCC = dynData.map(e => e.get("PROYECTOS")).indexOf(el.proyectos);

                    if (indexCC == -1) {
                        let tempData = new Map();
                        tempData.set("PROYECTOS", el.proyectos);

                        tempData.set(el.mes, el.porcentajes);
                        dynData.push(tempData);
                    } else {
                        let asdf = dynData[indexCC];
                        asdf.set("PROYECTOS", el.proyectos);
                        asdf.set(el.mes, el.porcentajes);

                    }
                });

            };
            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length - 1));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;

                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));

                        }
                    }
                    resultData.push(tempResult);

                    tempDataChart = [];
                    for (let i = 1; i < tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i]);

                    }
                    tempChart = { name: tempResult[0], data: tempDataChart }
                    resultChart.push(tempChart);
                }
            }

            dttablaPromedioConocimiento = tablaPromedioConocimiento.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                // scrollX: '200px',
                // scrollCollapse: false,
                columns: dynColumns,
                data: resultData,
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
                createdRow: function (row, data, index) {
                    for (let i = 0; i < dynColumns.length; i++) {
                        if (typeof (data[i]) == "number") {
                            $('td', row).eq(i).html(data[i] + " %");
                        }
                    }
                },
            });

        }

        //#endregion

        //#region Recorridos Operativos

        function inittablaPorcentajeRecorridosOperativos(tabla) {

            // let mapColumns = new Map();
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];

            if (tabla.length > 0) {
                dynColumns.push({ title: "PROYECTOS" });
                tabla.forEach(el => {

                    let indexExists = dynColumns.map(item => item.title).indexOf(el.mes);

                    if (indexExists == -1) { // NO LO ENCONTRO
                        dynColumns.push({ title: el.mes });
                    }

                    console.log(dynData.map(e => e.get("PROYECTOS")));
                    let indexCC = dynData.map(e => e.get("PROYECTOS")).indexOf(el.proyectos);

                    if (indexCC == -1) {
                        let tempData = new Map();
                        tempData.set("PROYECTOS", el.proyectos);

                        tempData.set(el.mes, el.porcentajes);
                        dynData.push(tempData);
                    } else {
                        let asdf = dynData[indexCC];
                        asdf.set("PROYECTOS", el.proyectos);
                        asdf.set(el.mes, el.porcentajes);

                    }
                });

            };
            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length - 1));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;

                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));

                        }
                    }
                    resultData.push(tempResult);

                    tempDataChart = [];
                    for (let i = 1; i < tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i]);

                    }
                    tempChart = { name: tempResult[0], data: tempDataChart }
                    resultChart.push(tempChart);
                }
            }

            dttablaPorcentajeRecorridosOperativos = tablaPorcentajeRecorridosOperativos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                // scrollX: '200px',
                // scrollCollapse: false,
                columns: dynColumns,
                data: resultData,
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
                createdRow: function (row, data, index) {
                    for (let i = 0; i < dynColumns.length; i++) {
                        if (typeof (data[i]) == "number") {
                            $('td', row).eq(i).html(data[i] + " %");
                        }
                    }
                },
            });

            let serieOptima = [];

            for (let i = 0; i < dynColumns.length - 2; i++) {
                serieOptima.push(90);
            }

            var chart = Highcharts.chart('graficaPorcentajeRecorridosOperativos', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Recorridos Operativos',
                    align: 'center'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: columnsChart,
                    crosshair: true
                },
                yAxis: {
                    max: 100,
                    title: {
                        text: '%'
                    },
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td></br>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
                series: [{
                    name: 'Objetivo Minimo',
                    color: '#040404',
                    data: serieOptima,
                    type: 'spline',
                    dashStyle: 'Dash',
                }],
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
            });
            for (var item of resultChart) {
                if (item.name != "PROMEDIO MENSUAL") {
                    chart.addSeries(item);

                }
            }

        }

        function cargarGraficaBarraPorcentajeRecorridosOperativos() {
            Highcharts.chart('graficaPorcentajeRecorridosOperativos', {
                title: {
                    text: ''
                },
                yAxis: {
                    title: {
                        text: '%'
                    },
                    max: 100,
                    crosshair: true,
                    gridLineWidth: 0,
                },
                xAxis: [{
                    categories: [],
                    crosshair: true
                }],
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td></br>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        },
                        //pointStart: 2010
                    }
                },
                // series: [{
                //     name: ['HERRADURA'],
                //     type: 'column',
                //     color: "#ed7d31",
                //     data: datos.serie1,
                // },
                // {
                //     name: 'NOCHE BUENA',
                //     type: 'column',
                //     color: "#7030a0",
                //     data: datos.serie1,
                // },
                // {
                //     name: 'COLORADA',
                //     type: 'column',
                //     color: "#a5a5a5",
                //     data: datos.serie1,
                // },
                // {
                //     name: 'SAN AGUSTIN',
                //     type: 'column',
                //     color: "#ffc000",
                //     data: datos.serie1,
                // },
                // {
                //     name: 'MULATOS',
                //     type: 'column',
                //     color: "#5b9bd5",
                //     data: datos.serie1,
                // },
                // {
                //     name: 'JUANICIPIO',
                //     type: 'column',
                //     color: "#70ad47",
                //     data: datos.serie1,
                // }, {
                //     name: 'OBJETIVO MENSUAL',
                //     color: "#040404",
                //     data: datos.serie2
                // }],
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

        //#endregion

        //#region Implementacion 5's
        function inittablaPorcentajeImplementacion5s(tabla) {

            // let mapColumns = new Map();
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];

            if (tabla.length > 0) {
                dynColumns.push({ title: "PROYECTOS" });
                tabla.forEach(el => {

                    let indexExists = dynColumns.map(item => item.title).indexOf(el.mes);

                    if (indexExists == -1) { // NO LO ENCONTRO
                        dynColumns.push({ title: el.mes });
                    }

                    console.log(dynData.map(e => e.get("PROYECTOS")));
                    let indexCC = dynData.map(e => e.get("PROYECTOS")).indexOf(el.proyectos);

                    if (indexCC == -1) {
                        let tempData = new Map();

                        tempData.set("PROYECTOS", el.proyectos);

                        tempData.set(el.mes, el.porcentajes);
                        dynData.push(tempData);
                    } else {
                        let asdf = dynData[indexCC];
                        asdf.set("PROYECTOS", el.proyectos);
                        asdf.set(el.mes, el.porcentajes);

                    }
                });

            };
            if (tabla.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length - 1));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;

                    for (var col of colData) {
                        if (!item.has(col)) {
                            tempResult.push(0);

                        } else {
                            tempResult.push(item.get(col));
                        }
                    }
                    resultData.push(tempResult);

                    tempDataChart = [];
                    for (let i = 1; i < tempResult.length - 1; i++) {
                        tempDataChart.push(tempResult[i]);

                    }
                    tempChart = { name: tempResult[0], data: tempDataChart }
                    resultChart.push(tempChart);
                }
            }

            dttablaPorcentajeImplementacion5s = tablaPorcentajeImplementacion5s.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                // scrollX: '200px',
                // scrollCollapse: false,
                columns: dynColumns,
                data: resultData,
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                ],
                createdRow: function (row, data, index) {
                    for (let i = 0; i < dynColumns.length; i++) {
                        if (typeof (data[i]) == "number") {
                            $('td', row).eq(i).html(data[i] + " %");
                        }
                    }
                },
            });

            let serieOptima = [];

            for (let i = 0; i < dynColumns.length - 2; i++) {
                serieOptima.push(90);
            }

            var chart = Highcharts.chart('graficaPorcentajeImplementacion5s', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Implementacion 5s',
                    align: 'center'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: columnsChart,
                    crosshair: true
                },
                yAxis: {
                    max: 100,
                    title: {
                        text: '%'
                    },
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
                series: [{
                    name: 'Objetivo Minimo',
                    color: '#040404',
                    data: serieOptima,
                    type: 'spline',
                    dashStyle: 'Dash',
                }],
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
            });
            for (var item of resultChart) {
                if (item.name != "PROMEDIO MENSUAL") {
                    chart.addSeries(item);

                }
            }
        }

        function cargarGraficaBarraPorcentajeImplementacion5s() {
            Highcharts.chart('graficaPorcentajeImplementacion5s', {
                title: {
                    text: ''
                },
                yAxis: {
                    title: {
                        text: '%'
                    },
                    max: 100,
                    crosshair: true,
                    gridLineWidth: 0,
                },

                xAxis: {
                    accessibility: {
                        rangeDescription: ''
                    },
                    categories: [],
                    crosshair: true,
                    gridLineWidth: 1,
                },

                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'bottom'
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td></br>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        },
                        //pointStart: 2010
                    }
                },
                // series: [{
                //     name: 'ENERO',
                //     type: 'column',
                //     color: "#4472c4"

                // },
                // {
                //     name: 'FEBRERO',
                //     type: 'column',
                //     color: "#ed7d31"

                // },
                // {
                //     name: 'MARZO',
                //     type: 'column',
                //     color: "#a5a5a5"

                // },
                // {
                //     name: 'ABRIL',
                //     type: 'column',
                //     color: "#ffc000"

                // },
                // {
                //     name: 'MAYO',
                //     type: 'column',
                //     color: "#5b9bd5"

                // },
                // {
                //     name: 'JUNIO',
                //     type: 'column',
                //     color: "#70ad47"

                // },
                // {
                //     name: 'JULIO',
                //     type: 'column',
                //     color: "#264478"

                // },
                // {
                //     name: 'AGOSTO',
                //     type: 'column',
                //     color: "#9e480e"
                // },
                // {
                //     name: 'SEPTIEMBRE',
                //     type: 'column',
                //     color: "#636363"

                // },
                // {
                //     name: 'OCTUBRE',
                //     type: 'column',
                //     color: "#997300"

                // },
                // {
                //     name: 'NOVIEMBRE',
                //     type: 'column',
                //     color: "#255e91"

                // },
                // {
                //     name: 'DICIEMBRE',
                //     type: 'column',
                //     color: "#43682b"

                // }, {
                //     name: 'OBJETIVO MENSUAL',
                //     color: "#040404",
                //     data: datos.serie2
                // }],
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

        //#endregion

        //#endregion

    }

    $(document).ready(() => Capacitacion.FactorCapacitacion = new FactorCapacitacion()).ajaxStart(() => $.blockUI({ message: 'Procesando...' })).ajaxStop($.unblockUI);
})();