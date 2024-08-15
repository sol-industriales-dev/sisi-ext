(() => {
    $.namespace('recursoshumanos.reportesrh.Dashboard');

    //#region CONSTS
    const fechaIni = $('#fechaIni');
    const fechaFin = $('#fechaFin');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const cboFiltroCC = $('#cboFiltroCC');

    const tblAltas = $('#tblAltas');
    let dtAltas;

    const tblBajas = $('#tblBajas');
    let dtBajas;

    const tblCambios = $('#tblCambios');
    let dtCambios;

    const tblTipoCambios = $('#tblTipoCambios');
    let dtTipoCambios

    const spanFechaInicial = $('#spanFechaInicial');
    const spanFechaFinal = $('#spanFechaFinal');
    const spanTotalAltas = $('#spanTotalAltas');
    const spanTotalBajas = $('#spanTotalBajas');
    const spanTotalActivosInicio = $('#spanTotalActivosInicio');
    const spanTotalActivosFin = $('#spanTotalActivosFin');
    const spanTotalRotacion = $('#spanTotalRotacion');
    const spanTotalCambios = $('#spanTotalCambios');
    //#endregion

    Dashboard = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            fechaIni.val(moment().format("YYYY-MM-DD"));
            fechaFin.val(moment().format("YYYY-MM-DD"));
            // initTblAltasBajas();
            initTblAltas();
            initTblBajas();
            initTblCambios();
            initTblTipoCambios();

            btnFiltroBuscar.on("click", function () {
                fncGetDashboard();
            });


            cboFiltroCC.fillCombo("/Administrativo/ReportesRH/FillComboCC", {}, false, 'Todos');
            // cboFiltroCC.select2({ width: "100%" });
            convertToMultiselect('#cboFiltroCC');
        }

        //#region TABLES
        function initTblAltas() {
            dtAltas = tblAltas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                dom: 'Bfrtip',
                // buttons: parametrosImpresion("Reporte detalle Adeudos", "<center><h3>Reporte Detalle Adeudos </h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            // columns: [':visible', 21]
                        }
                    }
                ],
                columns: [
                    //render: function (data, type, row) { }
                    {
                        data: 'ccDesc', title: 'CC',
                        render: function (data, type, row) {
                            return data ?? row.cc;
                        }
                    },
                    { data: 'cantAltas', title: 'ALTAS' },
                    {
                        data: 'porcAltas', title: '%',
                        render: function (data, type, row) {
                            return data.toFixed(2) + "%";
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblAltas.on('click', '.classBtn', function () {
                        let rowData = dtAltas.row($(this).closest('tr')).data();
                    });
                    tblAltas.on('click', '.classBtn', function () {
                        let rowData = dtAltas.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                createdRow: function (row, data, start, end, display) {
                    total = dtAltas
                        .column(1)
                        .data()
                        .reduce(function (a, b) {
                            return Number(a) + Number(b);
                        }, 0);
                    $(dtAltas.column(1).footer()).html(total);
                },
            });
        }

        function initTblBajas() {
            dtBajas = tblBajas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                dom: 'Bfrtip',
                // buttons: parametrosImpresion("Reporte detalle Adeudos", "<center><h3>Reporte Detalle Adeudos </h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            // columns: [':visible', 21]
                        }
                    }
                ],
                columns: [
                    //render: function (data, type, row) { }
                    {
                        data: 'ccDesc', title: 'CC',
                        render: function (data, type, row) {
                            return data ?? row.cc;
                        }
                    },
                    { data: 'cantBajas', title: 'BAJAS' },
                    {
                        data: 'porcBajas', title: '%',
                        render: function (data, type, row) {
                            return data.toFixed(2) + "%";
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblBajas.on('click', '.classBtn', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                    });
                    tblBajas.on('click', '.classBtn', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                createdRow: function (row, data, start, end, display) {
                    total = dtBajas
                        .column(1)
                        .data()
                        .reduce(function (a, b) {
                            return Number(a) + Number(b);
                        }, 0);
                    $(dtBajas.column(1).footer()).html(total);
                },
            });
        }

        function initTblCambios() {
            dtCambios = tblCambios.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                dom: 'Bfrtip',
                // buttons: parametrosImpresion("Reporte detalle Adeudos", "<center><h3>Reporte Detalle Adeudos </h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            // columns: [':visible', 21]
                        }
                    }
                ],
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'ccDesc', title: 'CC' },
                    { data: 'cantidad', title: 'CAMBIOS' },
                    {
                        data: 'porcAltas', title: '%',
                        render: function (data, type, row) {
                            return data.toFixed(2) + "%";
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblCambios.on('click', '.classBtn', function () {
                        let rowData = dtCambios.row($(this).closest('tr')).data();
                    });
                    tblCambios.on('click', '.classBtn', function () {
                        let rowData = dtCambios.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                createdRow: function (row, data, start, end, display) {
                    total = dtCambios
                        .column(1)
                        .data()
                        .reduce(function (a, b) {
                            return Number(a) + Number(b);
                        }, 0);
                    $(dtCambios.column(1).footer()).html(total);
                },
            });
        }

        function initTblTipoCambios() {
            dtTipoCambios = tblTipoCambios.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                dom: 'Bfrtip',
                // buttons: parametrosImpresion("Reporte detalle Adeudos", "<center><h3>Reporte Detalle Adeudos </h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            // columns: [':visible', 21]
                        }
                    }
                ],
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'CamposCambiados', title: 'TIPO' },
                    { data: 'cantidad', title: 'CANTIDAD' },
                    {
                        data: 'porcCambios', title: '%',
                        render: function (data, type, row) {
                            return data.toFixed(2) + "%";
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblTipoCambios.on('click', '.classBtn', function () {
                        let rowData = dtTipoCambios.row($(this).closest('tr')).data();
                    });
                    tblTipoCambios.on('click', '.classBtn', function () {
                        let rowData = dtTipoCambios.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                createdRow: function (row, data, start, end, display) {
                    total = dtTipoCambios
                        .column(1)
                        .data()
                        .reduce(function (a, b) {
                            return Number(a) + Number(b);
                        }, 0);
                    $(dtTipoCambios.column(1).footer()).html(total);
                },
            });
        }
        //#endregion

        //#region BACKEND
        function fncGetDashboard() {
            axios.post("GetDashboard", { ccs: getValoresMultiples('#cboFiltroCC'), fechaInicio: fechaIni.val(), fechaFin: fechaFin.val() }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    // dtAltasBajas.clear();
                    // dtAltasBajas.rows.add(response.data.dataAltasBajas);
                    // dtAltasBajas.draw();

                    dtBajas.clear();
                    dtBajas.rows.add(response.data.dataBajas);
                    dtBajas.draw();

                    dtAltas.clear();
                    dtAltas.rows.add(response.data.dataAltas);
                    dtAltas.draw();

                    dtCambios.clear();
                    dtCambios.rows.add(response.data.dataCambios);
                    dtCambios.draw();

                    dtTipoCambios.clear();
                    dtTipoCambios.rows.add(response.data.dataTipoCambios);
                    dtTipoCambios.draw();

                    let altasData = [];
                    let bajasData = [];
                    let ccsAltas = [];
                    let ccsBajas = [];

                    let motivosData = [];

                    let cambiosData = [];
                    let ccsCambios = [];

                    let tipoCambiosData = [];
                    let tiposCambios = [];

                    // response.data.dataAltasBajas.forEach(e => {
                    //     ccs.push(e.ccDesc);
                    //     altasData.push(Number(e.porcAltas.toFixed(2)));
                    //     bajasData.push(Number(e.porcBajas.toFixed(2)));
                    // });

                    response.data.dataAltas.forEach(e => {
                        ccsAltas.push(e.ccDesc ?? e.cc);
                        altasData.push(Number(e.porcAltas.toFixed(2)));
                    });

                    response.data.dataBajas.forEach(e => {
                        ccsBajas.push(e.ccDesc ?? e.cc);
                        bajasData.push(Number(e.porcBajas.toFixed(2)));
                    });

                    response.data.dataMotivos.forEach(e => {
                        motivosData.push({ name: e.descMotivo, y: Number(e.porcMotivo.toFixed(2)) });
                    });

                    response.data.dataCambios.forEach(e => {
                        ccsCambios.push(e.ccDesc);
                        cambiosData.push(Number(e.porcAltas.toFixed(2)));
                    });

                    response.data.dataTipoCambios.forEach(e => {
                        tiposCambios.push(e.CamposCambiados);
                        tipoCambiosData.push(Number(e.porcCambios.toFixed(2)));
                    });

                    // fncInitChartAltasBajas(ccs, altasData, bajasData);
                    fncInitChartAltas(ccsAltas, altasData);
                    fncInitChartBajas(ccsBajas, bajasData);
                    fncInitChartMotivoBajas(motivosData);
                    fncInitChartCambios(ccsCambios, cambiosData);
                    fncInitChartTipoCambios(tiposCambios, tipoCambiosData);

                    spanFechaInicial.text(moment(fechaIni.val()).format("DD/MM/YYYY"));
                    spanFechaFinal.text(moment(fechaFin.val()).format("DD/MM/YYYY"));
                    spanTotalAltas.text(response.data.totalAltas);
                    spanTotalBajas.text(response.data.totalBajas);
                    spanTotalActivosInicio.text(response.data.totalInicioAltas);//ACTIVOS
                    spanTotalActivosFin.text(response.data.totalFinAltas);//ACTIVOS
                    spanTotalRotacion.text(response.data.totalCambios.toFixed(2));
                    spanTotalCambios.text(response.data.totalNumCambios);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region CHARTS
        function fncInitChartAltas(ccs, altasData) {
            Highcharts.chart('chartAltas', {
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Cantidad Altas por CC'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: ccs,
                    title: {
                        text: null
                    },
                    labels: {
                        step: 1
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '%',
                        align: 'high'
                    },
                    labels: {
                        overflow: 'justify'
                    }
                },
                tooltip: {
                    valueSuffix: '%'
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:.2f}%'
                        }
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    x: -40,
                    y: 80,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
                    shadow: true
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Altas',
                    data: altasData
                }]
            });
        }

        function fncInitChartBajas(ccs, bajasData) {
            Highcharts.chart('chartBajas', {
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Cantidad Bajas por CC'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: ccs,
                    title: {
                        text: null
                    },
                    labels: {
                        step: 1
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '%',
                        align: 'high'
                    },
                    labels: {
                        overflow: 'justify'
                    }
                },
                tooltip: {
                    valueSuffix: '%'
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:.2f}%'
                        }
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    x: -40,
                    y: 80,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
                    shadow: true
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Bajas',
                    data: bajasData
                },]
            });
        }

        function fncInitChartMotivoBajas(motivosData) {
            Highcharts.chart('chartMotivoBajas', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Motivos de Baja por CC'
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
                series: [{
                    name: 'Motivos',
                    colorByPoint: true,
                    data: motivosData
                }]
            });
        }

        function fncInitChartCambios(ccs, cambiosData) {
            Highcharts.chart('chartCambios', {
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Cambios de Salario'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: ccs,
                    title: {
                        text: null
                    },
                    labels: {
                        step: 1
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '%',
                        align: 'high'
                    },
                    labels: {
                        overflow: 'justify'
                    }
                },
                tooltip: {
                    valueSuffix: '%'
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    x: -40,
                    y: 80,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
                    shadow: true
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Cambios',
                    data: cambiosData
                }]
            });
        }

        function fncInitChartTipoCambios(tiposCambios, cambiosData) {
            Highcharts.chart('chartTipoCambios', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Tipos de Cambios'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: tiposCambios,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    },
                    column: {
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:.2f}%'
                        }
                    }

                },
                series: [{
                    name: 'Cambios',
                    data: cambiosData
                }]
            });
        }
        //#endregion
    }

    $(document).ready(() => {
        recursoshumanos.reportesrh.Dashboard = new Dashboard();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();