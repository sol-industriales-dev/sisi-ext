(() => {
    $.namespace('CH.Dashboard');

    //#region CONSTS
    const fechaIni = $('#fechaIni');
    const fechaFin = $('#fechaFin');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const cboFiltroCC = $('#cboFiltroCC');

    const tblIncapacidades = $('#tblIncapacidades');
    let dtIncapacidades;


    //#endregion

    Dashboard = function () {
        (function init() {
            fncListeners();
            fncGetDashboard();
        })();

        function fncListeners() {
            cboFiltroCC.fillCombo("/Administrativo/ReportesRH/FillComboCC", {}, false, 'Todos');
            // cboFiltroCC.select2({ width: "100%" });
            convertToMultiselect('#cboFiltroCC');

            btnFiltroBuscar.on("click", function () {
                fncGetDashboard();
            });
        }

        //#region BACK

        function fncGetDashboard() {
            axios.post("GetDashboard", { ccs: getValoresMultiples('#cboFiltroCC'), fechaInicio: fechaIni.val(), fechaFin: fechaFin.val() }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    if (items.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tblIncapacidades')) {
                            dtIncapacidades.clear().destroy();
                            tblIncapacidades.empty();

                            // dtIncapacidades = null;

                        }
                        initTblIncapacidades(items);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tblIncapacidades')) {
                            dtIncapacidades.clear();
                            dtIncapacidades.draw();
                            fncInitChartIncaps();
                        }
                    }


                    // fncInitChartIncaps();
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#endregion

        //#region TBL

        function initTblIncapacidades(items) {

            //#region TABLA DINAMICA
            // let mapColumns = new Map();
            let dynColumns = [];
            let dynData = [];
            let resultData = [];
            let columnsChart = [];
            let resultChart = [];

            dynColumns.push({ title: "Concepto" });

            if (items.length > 0) {
                items.forEach(e => {
                    let tempData = new Map();
                    tempData.set("concepto", e.concepto);
                    e.lstIncapsDet.forEach(el => {
                        // if (!dynColumns.has(el.conceptoDet)) {
                        //     dynColumns.set(el.conceptoDet)
                        // }
                        let indexExists = dynColumns.map(item => item.title).indexOf(el.conceptoDet);

                        if (indexExists == -1) { // NO LO ENCONTRO
                            // dynColumns.push({ data: el.conceptoDet, title: el.conceptoDet });
                            dynColumns.push({ title: el.conceptoDet });
                        }

                        tempData.set(el.conceptoDet, el.cantidadDet);
                    });
                    tempData.set("total", e.total);
                    // console.log(tempData);
                    dynData.push(tempData);
                });

                // dynColumns.push({ data: "total", title: "TOTAL" });
                dynColumns.push({ title: "total" });

            }

            if (items.length > 0) {
                let colData = dynColumns.map(e => e.title);
                columnsChart = colData.slice(1, (dynColumns.length - 1));

                for (var item of dynData) {
                    let tempResult = [];
                    let tempChart;

                    for (var col of colData) {
                        if (col.toLowerCase() == 'concepto') {
                            tempResult.push(item.get(col.toLowerCase()));
                        } else {
                            if (!item.has(col)) {
                                tempResult.push(0);
                            } else {
                                tempResult.push(item.get(col));
                            }
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

            //#endregion

            // console.log(resultData);
            // console.log(resultChart);


            // console.log(dynData);
            // console.log(dynColumns);


            dtIncapacidades = tblIncapacidades.DataTable({
                language: dtDicEsp,
                // destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: dynColumns,
                dom: 'Bfrtip',
                data: resultData,
                buttons: [
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            // columns: [':visible', 21]
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblIncapacidades.on('click', '.classBtn', function () {
                        let rowData = dtIncapacidades.row($(this).closest('tr')).data();
                    });
                    tblIncapacidades.on('click', '.classBtn', function () {
                        let rowData = dtIncapacidades.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });


            //#region INIT GRAFICA
            chart = new Highcharts.chart('chartIncapacidades', {
                chart: {
                    renderTo: 'chartIncapacidades',

                    type: 'bar'
                },
                title: {
                    text: 'INCAPACIDADES'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: columnsChart,
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
                            // format: '{point.y:.2f}%'
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
                // series: [{
                //     name: "asdf",
                //     data: [1, 2]
                // }],
            });

            for (var item of resultChart) {
                chart.addSeries(item);
            }

            //#endregion

        }

        //#endregion

        //#region CHARTS

        function fncInitChartIncaps(rps) {
            Highcharts.chart('chartIncapacidades', {
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'INCAPACIDADES'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: [],
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
                            // format: '{point.y:.2f}%'
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
            });
        }
        //#endregion

        //#region FNC GRALES
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Dashboard = new Dashboard();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();