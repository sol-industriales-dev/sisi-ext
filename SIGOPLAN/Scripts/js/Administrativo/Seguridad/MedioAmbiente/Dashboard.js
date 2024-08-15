(() => {
    $.namespace('CH.Dashboard');

    //#region CONST DASHBOARD
    const cboFiltroAgrupacion = $('#cboFiltroAgrupacion');
    const txtMesInicio = $('#txtMesInicio');
    const txtMesFinal = $('#txtMesFinal');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnGraficaIndice = $('#btnGraficaIndice');
    const btnGraficaAcumulado = $('#btnGraficaAcumulado');
    const divGraficaBarraIndice = $('#divGraficaBarraIndice');
    const divGraficaBarraAcumulado = $('#divGraficaBarraAcumulado');

    const dateFormat = "dd/mm/yy";
    const showAnim = "slide";
    const fechaActual = new Date();
    const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);
    var txtAnoIndice;
    //#endregion

    Dashboard = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            $('#selectDivision').fillCombo('/Administrativo/Requerimientos/GetDivisionesCombo', null, false, 'Todos');
            convertToMultiselect('#selectDivision');
            $('#selectLineaNegocio').fillCombo('/Administrativo/Requerimientos/GetLineaNegocioCombo', { division: 0 }, false, 'Todos');
            convertToMultiselect('#selectLineaNegocio');

            cboFiltroAgrupacion.fillComboSeguridadDivisionLineaNegocio(false, false, $('#selectDivision').val(), $('#selectLineaNegocio').val());
            cboFiltroAgrupacion.select2({ width: "100%" });

            initMonthPicker(txtMesInicio);
            txtMesInicio.datepicker({ dateFormat, showAnim }).datepicker("setDate", fechaInicioAnio);
            initMonthPicker(txtMesFinal);

            btnFiltroBuscar.on("click", function () {
                fncGetGraficas();
            });

            $("#divGraficaBarrasIndice").css("display", "block");
            $("#divGraficaBarrasAcumulado").css("display", "none");

            btnGraficaIndice.on("click", function () {
                $("#divGraficaBarrasIndice").css("display", "block");
                $("#divGraficaBarrasAcumulado").css("display", "none");
            });

            btnGraficaAcumulado.on("click", function () {
                $("#divGraficaBarrasIndice").css("display", "none");
                $("#divGraficaBarrasAcumulado").css("display", "block");
            });
        }

        $('#selectDivision').on('change', function () {
            cboFiltroAgrupacion.fillComboSeguridadDivisionLineaNegocio(false, false, $('#selectDivision').val(), $('#selectLineaNegocio').val());
        });

        $('#selectLineaNegocio').on('change', function () {
            cboFiltroAgrupacion.fillComboSeguridadDivisionLineaNegocio(false, false, $('#selectDivision').val(), $('#selectLineaNegocio').val());
        });

        function fncGetGraficas() {
            let obj = {
                listaDivisiones: $('#selectDivision').val(),
                listaLineasNegocio: $('#selectLineaNegocio').val(),
                idAgrupacion: cboFiltroAgrupacion.val(),
                fechaInicio: txtMesInicio.val(),
                fechaFin: txtMesFinal.val()
            }
            axios.post("GetGraficas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region INIT GRAFICAS
                    initGraficaBarrasIndice(response.data.objGraficaBarrasDTO);
                    initGraficaBarrasAcumulado(response.data.objGraficaBarrasDTO);
                    initGraficaConsumoEnergiaElectrica(response.data.graficaConsumoEnergiaElectricaData, response.data.graficaConsumoEnergiaElectricaMeta);
                    initGraficaGeneracionEnergiaElectrica(response.data.graficaGeneracionEnergiaElectricaData, response.data.graficaGeneracionEnergiaElectricaMeta);
                    initGraficaConsumoCombustible(response.data.graficaConsumoCombustibleData, response.data.graficaConsumoCombustibleMeta);
                    // initGraficaGeneracionGEI(response.data.graficaGeneracionGEIData, response.data.graficaGeneracionGEIMeta);
                    initGraficaConsumoAgua(response.data.graficaConsumoAguaData, response.data.graficaConsumoAguaMeta);
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#region GRAFICA BARRAS INDICE
        function initGraficaBarrasIndice(datos) {
            Highcharts.chart('divGraficaBarrasIndice', {
                lang: highChartsDicEsp,
                title: {
                    text: ''
                },
                xAxis: {
                    categories: datos.lstCategorias,
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
                    labels: { format: '{value}' }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: `
                        <tr>
                            <td style="color:{series.color};padding:0">{series.name}: </td>
                            <td><b>{point.y}</b></td>
                        </tr>`,
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                labels: {
                    items: [{
                        // html: 'Total fruit consumption',
                        style: {
                            left: '50px',
                            top: '18px',
                            color: ( // theme
                                Highcharts.defaultOptions.title.style &&
                                Highcharts.defaultOptions.title.style.color
                            ) || 'black'
                        }
                    }]
                },
                series: [{
                    type: 'column',
                    name: 'GENERACIÓN DE RESIDUOS BIOLOGICO INFECCIOSOS',
                    data: datos.lstDataRBI,
                    color: "#92d050"
                }, {
                    type: 'column',
                    name: 'GENERACIÓN DE RESIDUOS DE MANEJO ESPECIAL',
                    data: datos.lstDataRME,
                    color: "#ff0000"
                }, {
                    type: 'column',
                    name: 'GENERACIÓN DE RESIDUOS PELIGROSOS',
                    data: datos.lstDataRP,
                    color: "#c00000"
                }, {
                    type: 'column',
                    name: 'GENERACIÓN DE RESIDUOS SOLIDOS URBANOS',
                    data: datos.lstDataRSU,
                    color: "#2f5597"
                }, {
                    type: 'spline',
                    name: 'INDICE ' + txtMesInicio.val() + ' al ' + txtMesFinal.val(),
                    data: datos.lstIndice2023,
                    marker: {
                        lineWidth: 2,
                        lineColor: Highcharts.getOptions().colors[3],
                        fillColor: 'white',
                        color: "#70ad47"
                    }
                }, {
                    type: 'spline',
                    name: 'INDICE 2022',
                    data: datos.lstIndice2022,
                    marker: {
                        lineWidth: 2,
                        lineColor: Highcharts.getOptions().colors[3],
                        fillColor: 'white',
                        color: "#4472c4"
                    }
                }]
            });
        }
        //#endregion

        //#region GRAFICA BARRAS ACUMULADOS
        function initGraficaBarrasAcumulado(datos) {
            Highcharts.chart('divGraficaBarrasAcumulado', {
                lang: highChartsDicEsp,
                title: {
                    text: ''
                },
                xAxis: {
                    categories: datos.lstCategorias,
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
                    labels: { format: '{value}' }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: `
                        <tr>
                            <td style="color:{series.color};padding:0">{series.name}: </td>
                            <td><b>{point.y}</b></td>
                        </tr>`,
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                labels: {
                    items: [{
                        style: {
                            left: '50px',
                            top: '18px',
                            color: (
                                Highcharts.defaultOptions.title.style &&
                                Highcharts.defaultOptions.title.style.color
                            ) || 'black'
                        }
                    }]
                },
                series: [{
                    type: 'column',
                    name: 'GENERACIÓN DE RESIDUOS BIOLOGICO INFECCIOSOS',
                    data: datos.lstDataRBI,
                    color: "#92d050"
                }, {
                    type: 'column',
                    name: 'GENERACIÓN DE RESIDUOS DE MANEJO ESPECIAL',
                    data: datos.lstDataRME,
                    color: "#ff0000"
                }, {
                    type: 'column',
                    name: 'GENERACIÓN DE RESIDUOS PELIGROSOS',
                    data: datos.lstDataRP,
                    color: "#c00000"
                }, {
                    type: 'column',
                    name: 'GENERACIÓN DE RESIDUOS SOLIDOS URBANOS',
                    data: datos.lstDataRSU,
                    color: "#2f5597"
                }, {
                    type: 'spline',
                    name: 'ACUMULADO ' + txtMesInicio.val() + ' al ' + txtMesFinal.val(),
                    data: datos.lstAcumulado2023,
                    marker: {
                        lineWidth: 2,
                        lineColor: Highcharts.getOptions().colors[3],
                        fillColor: 'white',
                        color: "#4472c4"
                    },
                }, {
                    type: 'spline',
                    name: 'ACUMULADO 2022',
                    data: datos.lstAcumulado2022,
                    marker: {
                        lineWidth: 2,
                        lineColor: Highcharts.getOptions().colors[3],
                        fillColor: 'white',
                        color: "#4472c4"
                    }
                }]
            });
        }
        //#endregion

        //#region GRAFICA CONSUMO DE ENERGIA ELECTRICA
        function initGraficaConsumoEnergiaElectrica(consumo, meta) {
            Highcharts.chart('divGraficaConsumoEnergiaElectrica', Highcharts.merge(gaugeOptions, {
                yAxis: {
                    min: 0,
                    max: meta,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: '',
                    data: [consumo],
                    dataLabels: {
                        format:
                            '<div style="text-align:center">' +
                            '<span style="font-size:25px">{y}%</span><br/>' +
                            '<span style="font-size:12px;opacity:0.4">%</span>' +
                            '</div>'
                    },
                    tooltip: {
                        valueSuffix: ' %'
                    }
                }]
            }));
        }
        //#endregion

        //#region GENERACIÓN DE ENERGIA ELECTRICA
        function initGraficaGeneracionEnergiaElectrica(consumo, meta) {
            Highcharts.chart('divGraficaGeneracionEnergiaElectrica', Highcharts.merge(gaugeOptions, {
                yAxis: {
                    min: 0,
                    max: meta,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: '',
                    data: [consumo],
                    dataLabels: {
                        format:
                            '<div style="text-align:center">' +
                            '<span style="font-size:25px">{y}%</span><br/>' +
                            '<span style="font-size:12px;opacity:0.4">%</span>' +
                            '</div>'
                    },
                    tooltip: {
                        valueSuffix: ' %'
                    }
                }]
            }));
        }
        //#endregion

        //#region CONSUMO DE COMBUSTIBLE
        function initGraficaConsumoCombustible(consumo, meta) {
            Highcharts.chart('divGraficaConsumoCombustible', Highcharts.merge(gaugeOptions, {
                yAxis: {
                    min: 0,
                    max: meta,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: '',
                    data: [consumo],
                    dataLabels: {
                        format:
                            '<div style="text-align:center">' +
                            '<span style="font-size:25px">{y}%</span><br/>' +
                            '<span style="font-size:12px;opacity:0.4">%</span>' +
                            '</div>'
                    },
                    tooltip: {
                        valueSuffix: ' %'
                    }
                }]
            }));
        }
        //#endregion

        //#region GENERACIÓN GEI
        function initGraficaGeneracionGEI(consumo, meta) {
            Highcharts.chart('divGraficaGeneracionGEI', Highcharts.merge(gaugeOptions, {
                yAxis: {
                    min: 0,
                    max: meta,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: '',
                    data: [consumo],
                    dataLabels: {
                        format:
                            '<div style="text-align:center">' +
                            '<span style="font-size:25px">{y}%</span><br/>' +
                            '<span style="font-size:12px;opacity:0.4">%</span>' +
                            '</div>'
                    },
                    tooltip: {
                        valueSuffix: ' %'
                    }
                }]
            }));
        }
        //#endregion

        //#region CONSUMO AGUA
        function initGraficaConsumoAgua(consumo, meta) {
            Highcharts.chart('divGraficaConsumoAgua', Highcharts.merge(gaugeOptions, {
                yAxis: {
                    min: 0,
                    max: meta,
                    title: {
                        text: ''
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: '',
                    data: [consumo],
                    dataLabels: {
                        format:
                            '<div style="text-align:center">' +
                            '<span style="font-size:25px">{y}%</span><br/>' +
                            '<span style="font-size:12px;opacity:0.4">%</span>' +
                            '</div>'
                    },
                    tooltip: {
                        valueSuffix: ' %'
                    }
                }]
            }));
        }
        //#endregion

        //#region GENERAL
        var gaugeOptions = {
            chart: {
                type: 'solidgauge'
            },
            title: null,
            pane: {
                center: ['50%', '85%'],
                size: '140%',
                startAngle: -90,
                endAngle: 90,
                background: {
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || '#EEE',
                    innerRadius: '60%',
                    outerRadius: '100%',
                    shape: 'arc'
                }
            },
            exporting: {
                enabled: false
            },
            tooltip: {
                enabled: false
            },
            yAxis: {
                stops: [
                    [0.1, '#55BF3B'],
                    [0.5, '#DDDF0D'],
                    [0.9, '#DF5353']
                ],
                lineWidth: 0,
                tickWidth: 0,
                minorTickInterval: null,
                tickAmount: 2,
                title: {
                    y: -70
                },
                labels: {
                    y: 16
                }
            },
            plotOptions: {
                solidgauge: {
                    dataLabels: {
                        y: 5,
                        borderWidth: 0,
                        useHTML: true
                    }
                }
            }
        };
        //#endregion

        //#region DATETIME PICKER
        function initMonthPicker(input) {
            $(input).datepicker({
                dateFormat: "mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                maxDate: fechaActual,
                showAnim: showAnim,
                closeText: "Aceptar",
                onClose: function (dateText, inst) {
                    function isDonePressed() {
                        return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                    }

                    if (isDonePressed()) {
                        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                        $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');

                        $('.date-picker').focusout()//Added to remove focus from datepicker input box on selecting date
                    }
                },
                beforeShow: function (input, inst) {
                    inst.dpDiv.addClass('month_year_datepicker')

                    if ((datestr = $(this).val()).length > 0) {
                        year = datestr.substring(datestr.length - 4, datestr.length);
                        month = datestr.substring(0, 2);
                        $(this).datepicker('option', 'defaultDate', new Date(year, month - 1, 1));
                        $(this).datepicker('setDate', new Date(year, month - 1, 1));
                        $(".ui-datepicker-calendar").hide();
                    }
                }
            }).datepicker("setDate", fechaActual);
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Dashboard = new Dashboard();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();