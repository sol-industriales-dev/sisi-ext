(() => {
    $.namespace('Subcontratistas.Dashboard');
    Dashboard = function () {

        const cboCC = $('#cboCC');
        const cboSubContratista = $('#cboSubContratista');
        const selectEstado = $('#selectEstado');
        const selectMunicipio = $('#selectMunicipio');
        const cboEspecialidad = $('#cboEspecialidad');
        const fechaFiltroInicio = $('#fechaFiltroInicio');
        const fechaFiltroFin = $('#fechaFiltroFin');

        const btnBuscar = $('#btnBuscar');
        const tblReporteEjecutivo = $('#tblReporteEjecutivo');
        let dtReporteEjecutivo = null;

        const botonExportarExcel = $('#botonExportarExcel');
        const btnFiltroExportar = $('#btnFiltroExportar');

        //General
        const botonConsultarCumplimientoGlobalSubcontratista = $('#botonConsultarCumplimientoGlobalSubcontratista');
        const botonConsultarCumplimientoGlobalElementos = $('#botonConsultarCumplimientoGlobalElementos');
        const botonConsultaCumplimientoGlobalEvaluacion = $('#botonConsultaCumplimientoGlobalEvaluacion');
        //Reporte
        const botonConsultarReporteEjecutivo = $('#botonConsultarReporteEjecutivo');
        // var elemento = 1;
        let graficaSubContratistasDashboard;//
        let graficaElementosDashboard;//
        let graficaEvaluacionDashboard;
        let countGraficas = 0;

        (function init() {
            fncListeners();
            llenarCombos();
            initTablaReporteEjecutivo();
            cargarDatosSeccionDetalle();

            botonExportarExcel.hide();

            btnFiltroExportar.attr('disabled', true);

            btnBuscar.click((x) => { btnFiltroExportar.attr('disabled', false); });
        })();

        selectEstado.on('change', function () {
            let estado_id = +selectEstado.val();

            selectMunicipio.fillCombo('FillComboMunicipios', { estado_id }, false, null);
        });

        $('#cboCC, #cboSubContratista, #cboEspecialidad, #fechaFiltroInicio, #fechaFiltroFin, #selectEstado, #selectMunicipio').on('change', function () {
            btnFiltroExportar.attr('disabled', true);
        });

        function fncListeners() {
            // botonConsultarCumplimientoGlobalSubcontratista.click(() => {

            fechaFiltroInicio.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker('option', 'showAnim', 'slide')
                .datepicker('setDate', new Date());
            fechaFiltroFin.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker('option', 'showAnim', 'slide')
                .datepicker('setDate', new Date());

            // botonConsultarCumplimientoGlobalSubcontratista.click(() => {
            btnBuscar.click(() => {
                if (getValoresMultiples("#cboSubContratista").length >= 1 && fechaFiltroInicio.val() != "" && fechaFiltroFin.val() != "") {
                    fncGetGraficaCumplimientoPorSubcontratista();
                    fncGetGraficaCumplimientoPorElementos();
                    fncGetGraficaCumplimientoPorEvaluacion();
                    // fncGetGraficaCumplimientosDetalles(elemento);

                } else if (getValoresMultiples("#cboCC").length == 0) {
                    Alert2Warning("Seleccione al menos un centro de costo.");
                    // }
                    // else if (getValoresMultiples("#cboEspecialidad").length == 0) {
                    //     Alert2Warning("Seleccione al menos una especialidad.");
                } else if (fechaFiltroInicio.val() == "") {
                    Alert2Warning("Seleccione la fecha inicial para filtrar.");
                } else if (fechaFiltroFin.val() == "") {
                    Alert2Warning("Seleccione la fecha inicial para filtrar.");
                }

                // });

                // // botonConsultarCumplimientoGlobalElementos.click(() => {
                // if (getValoresMultiples("#cboSubContratista").length >= 1 && fechaFiltroInicio.val() != "" && fechaFiltroFin.val() != "") {
                //     fncGetGraficaCumplimientoPorElementos();
                // } else if (getValoresMultiples("#cboCC").length == 0) {
                //     Alert2Warning("Seleccione al menos un centro de costo.");
                // }
                // else if (getValoresMultiples("#cboEspecialidad").length == 0) {
                //     Alert2Warning("Seleccione al menos una especialidad.");
                // } else if (fechaFiltroInicio.val() == "") {
                //     Alert2Warning("Seleccione la fecha inicial para filtrar.");
                // } else if (fechaFiltroFin.val() == "") {
                //     Alert2Warning("Seleccione la fecha inicial para filtrar.");
                // }
                // // });

                // // botonConsultaCumplimientoGlobalEvaluacion.click(() => {
                // if (getValoresMultiples("#cboSubContratista").length >= 1 && fechaFiltroInicio.val() != "" && fechaFiltroFin.val() != "") {
                //     fncGetGraficaCumplimientoPorEvaluacion();
                // } else if (getValoresMultiples("#cboCC").length == 0) {
                //     Alert2Warning("Seleccione al menos un centro de costo.");
                // }
                // else if (getValoresMultiples("#cboEspecialidad").length == 0) {
                //     Alert2Warning("Seleccione al menos una especialidad.");
                // } else if (fechaFiltroInicio.val() == "") {
                //     Alert2Warning("Seleccione la fecha inicial para filtrar.");
                // } else if (fechaFiltroFin.val() == "") {
                //     Alert2Warning("Seleccione la fecha inicial para filtrar.");
                // }
                // // });
            }),
                botonConsultarCumplimientoGlobalSubcontratista.click(() => {
                    if (getValoresMultiples("#cboSubContratista").length >= 1 && fechaFiltroInicio.val() != "" && fechaFiltroFin.val() != "") {
                        fncGetGraficaCumplimientoPorSubcontratista();
                    } else if (getValoresMultiples("#cboCC").length == 0) {
                        Alert2Warning("Seleccione al menos un centro de costo.");
                        // }
                        // else if (getValoresMultiples("#cboEspecialidad").length == 0) {
                        //     Alert2Warning("Seleccione al menos una especialidad.");
                    } else if (fechaFiltroInicio.val() == "") {
                        Alert2Warning("Seleccione la fecha inicial para filtrar.");
                    } else if (fechaFiltroFin.val() == "") {
                        Alert2Warning("Seleccione la fecha inicial para filtrar.");
                    }

                });

            botonConsultarCumplimientoGlobalElementos.click(() => {
                if (getValoresMultiples("#cboSubContratista").length >= 1 && fechaFiltroInicio.val() != "" && fechaFiltroFin.val() != "") {
                    fncGetGraficaCumplimientoPorElementos();
                } else if (getValoresMultiples("#cboCC").length == 0) {
                    Alert2Warning("Seleccione al menos un centro de costo.");
                    // }
                    // else if (getValoresMultiples("#cboEspecialidad").length == 0) {
                    //     Alert2Warning("Seleccione al menos una especialidad.");
                } else if (fechaFiltroInicio.val() == "") {
                    Alert2Warning("Seleccione la fecha inicial para filtrar.");
                } else if (fechaFiltroFin.val() == "") {
                    Alert2Warning("Seleccione la fecha inicial para filtrar.");
                }
            });

            botonConsultaCumplimientoGlobalEvaluacion.click(() => {
                if (getValoresMultiples("#cboSubContratista").length >= 1 && fechaFiltroInicio.val() != "" && fechaFiltroFin.val() != "") {
                    fncGetGraficaCumplimientoPorEvaluacion();
                } else if (getValoresMultiples("#cboCC").length == 0) {
                    Alert2Warning("Seleccione al menos un centro de costo.");
                    // }
                    // else if (getValoresMultiples("#cboEspecialidad").length == 0) {
                    //     Alert2Warning("Seleccione al menos una especialidad.");
                } else if (fechaFiltroInicio.val() == "") {
                    Alert2Warning("Seleccione la fecha inicial para filtrar.");
                } else if (fechaFiltroFin.val() == "") {
                    Alert2Warning("Seleccione la fecha inicial para filtrar.");
                }
            });

            botonConsultarReporteEjecutivo.click(() => {
                if (getValoresMultiples("#cboSubContratista").length >= 1 && getValoresMultiples("#cboCC").length >= 1 && fechaFiltroInicio.val() != "" && fechaFiltroFin.val() != "") {
                    let obj = {
                        lstFiltroCC: getValoresMultiples("#cboCC"),
                        lstFiltroSubC: getValoresMultiples("#cboSubContratista"),
                        lstFiltroEspecialidad: getValoresMultiples("#cboEspecialidad"),
                        fechaFiltroInicio: fechaFiltroInicio.val(),
                        fechaFiltroFin: fechaFiltroFin.val(),
                        fechaFiltroFin: fechaFiltroFin.val(),
                        estado_id: +selectEstado.val(),
                        municipio_id: +selectMunicipio.val(),
                        listaEspecialidades: cboEspecialidad.val().map((x) => { return +x })

                    }

                    moment(fechaFiltroFin.val(), "DD/MM/YYYY").toISOString(true)
                    axios.post('GetReporteEjecutivo', obj).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            dtReporteEjecutivo.clear();
                            dtReporteEjecutivo.rows.add(items);
                            dtReporteEjecutivo.draw();
                            $('#panelReporteEjecutivo').addClass('show');
                            // dtReporteEjecutivo.DataTable().columns.adjust().draw();
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
                }
                else if (getValoresMultiples("#cboCC").length == 0) {
                    Alert2Warning("Seleccione al menos un centro de costo.");
                    // }
                    // else if (getValoresMultiples("#cboEspecialidad").length == 0) {
                    //     Alert2Warning("Seleccione al menos una especialidad.");
                } else if (fechaFiltroInicio.val() == "") {
                    Alert2Warning("Seleccione la fecha inicial para filtrar.");
                } else if (fechaFiltroFin.val() == "") {
                    Alert2Warning("Seleccione la fecha inicial para filtrar.");
                }
            });

            btnFiltroExportar.on("click", function () {

                countGraficas = 0;


                fncGenerarReporteCrystalReport($("#chartPorcentajeCumplimientoGlobalSubcontratista").highcharts(), 'chart', 1, download);
                fncGenerarReporteCrystalReport($("#chartPorcentajeCumplimientoGlobalElementos").highcharts(), 'chart', 2, download);
                fncGenerarReporteCrystalReport($("#chartPorcentajeCumplimientoGlobalEvaluacion").highcharts(), 'chart', 3, download);
                // fncSaveImgSession();

                // let graficaSubContratistasDashboard;//
                // let graficaElementosDashboard;//
                // let graficaEvaluacionDashboard;


                if (graficaSubContratistasDashboard != undefined && graficaElementosDashboard != undefined && graficaEvaluacionDashboard != undefined) {
                    // Alert2AccionConfirmar('', '¿Desea generar el reporte?', 'Confirmar', 'Cancelar', () => fncSaveImgSession());
                }
            });
        }

        // Métodos.
        function llenarCombos() {

            cboCC.fillCombo('FillComboProyectos', null, false, 'Todos');
            convertToMultiselect('#cboCC');
            // cboSubContratista.fillCombo('FillComboSubcontratistas', null, false, 'Todos');
            cboSubContratista.fillCombo('FillComboSubcontratistasSorted', null, false, 'Todos');
            convertToMultiselect('#cboSubContratista');
            cboEspecialidad.fillCombo('FillComboEspecialidad', null, false, 'Todos');
            convertToMultiselect('#cboEspecialidad');

            selectEstado.fillCombo('FillComboEstados', null, false, null);
            selectMunicipio.fillCombo('FillComboMunicipios', { estado_id: 0 }, false, null);
        }

        //#region 
        function cargarDatosSeccionDetalle() {
            axios.post('FillComboElementos').then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    $.each(items, function (i, descripcionElemento) {
                        $(`<div class="row margin-top" style="display: none;">               
              <div class="col-xs-12 col-md-12">
              <div class="panel-group">
              <div class="panel panel-primary">
              <div class="panel-heading">
              <h4 class="panel-title">
                                             <a data-toggle="collapse" href="#panelCumplimientos ${descripcionElemento.Value}">
                                             <i class="fas fa-globe-americas"></i> Cumplimiento por ${descripcionElemento.Text}
                                             </a>
              </h4>
              </div>
              </div>
              </div>
              </div>
              </div>

        <div id="divGraficasDetalles">
        <div class="row margin-top">
            <div class="col-xs-12 col-md-12">
                <div class="panel-group">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                                  <a data-toggle="collapse" href="#panelCumplimientos${descripcionElemento.Value}">
                                    <i class="fas fa-globe-americas"></i>
                                    Cumplimiento por ${descripcionElemento.Text}
                                </a>
                                                  <button id="botonConsultarCumplimientos${descripcionElemento.Value}" class="btn btn-xs btn-default pull-right" data-elemento=${descripcionElemento.Value}>Consultar</button>
                            </h4>
                        </div>
                                             <div id="chartPorcentajeCumplimientos${descripcionElemento.Value}"></div>
                    </div>
                </div>
            </div>
        </div>
        </div>`).appendTo('#tabDetalle');

                        $('#botonConsultarCumplimientos' + descripcionElemento.Value + '').on('click', function () {
                            // console.log("Maicraaa");
                            if (getValoresMultiples("#cboCC").length >= 1 && getValoresMultiples("#cboSubContratista").length >= 1 && fechaFiltroInicio.val() != "" && fechaFiltroFin.val() != "") {
                                fncGetGraficaCumplimientoPorSubcontratista();
                                fncGetGraficaCumplimientosDetalles($(this).data("elemento"));
                            } else if (getValoresMultiples("#cboCC").length == 0) {
                                Alert2Warning("Seleccione al menos un centro de costo.");
                                // }
                                // else if (getValoresMultiples("#cboEspecialidad").length == 0) {
                                //     Alert2Warning("Seleccione al menos una especialidad.");
                            } else if (fechaFiltroInicio.val() == "") {
                                Alert2Warning("Seleccione la fecha inicial para filtrar.");
                            } else if (fechaFiltroFin.val() == "") {
                                Alert2Warning("Seleccione la fecha inicial para filtrar.");
                            }

                            // console.log("Maicraaa");  Solo para revisar si entra la funcion
                        });

                    });
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region GRAFICAS GENERAL

        function fncGetGraficaCumplimientoPorSubcontratista() {

            let obj = {
                lstFiltroCC: getValoresMultiples("#cboCC"),
                lstFiltroSubC: getValoresMultiples("#cboSubContratista"),
                lstFiltroEspecialidad: getValoresMultiples("#cboEspecialidad"),
                fechaFiltroInicio: fechaFiltroInicio.val(),
                fechaFiltroFin: fechaFiltroFin.val(),
                estado_id: +selectEstado.val(),
                municipio_id: +selectMunicipio.val(),
                listaEspecialidades: cboEspecialidad.val().map((x) => { return +x })
            }

            axios.post("GetGraficaCumplimientoPorSubcontratista", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    initChartCumplimientoGlobalSubcontratista(response.data.lstConcepts, response.data.lstData);
                    // $('#panelCumplimientoGlobalSubcontratista').addClass('show');
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initChartCumplimientoGlobalSubcontratista(concepts, data) {
            let cumplimientoOptimo = [];

            for (var item of data) {
                cumplimientoOptimo.push(90);
            }

            Highcharts.chart('chartPorcentajeCumplimientoGlobalSubcontratista', {
                chart: {
                    type: 'column'
                },
                lang: highChartsDicEsp,
                credits: { enabled: false },
                title: {
                    text: 'CUMPLIMIENTO GLOBAL DE SUBCONTRATISTA'
                },
                xAxis: {
                    categories: concepts,
                    crosshair: true
                },
                yAxis: {
                    max: 100,
                    title: {
                        useHTML: true,
                        text: '% de Cumplimiento'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            max: 100,
                            beginAtZero: true
                        }
                    }]
                },
                series: [{
                    name: 'Cumplimiento',
                    data: data
                },
                {
                    type: 'spline',
                    name: 'Cumplimiento optimo',
                    data: cumplimientoOptimo,
                    color: '#E12121'
                },]
            });

            // fncGenerarReporteCrystalReport($("#chartPorcentajeCumplimientoGlobalSubcontratista").highcharts(), 'chart', 1);
            // fncGenerarReporteCrystalReport($("#chartPorcentajeCumplimientoGlobalSubcontratista").highcharts(), 'chart', 1);
            // fncGenerarReporteCrystalReport($("#chartPorcentajeCumplimientoGlobalSubcontratista").highcharts(), 'chart', 1);
        }

        function fncGetGraficaCumplimientoPorElementos() {
            let obj = {
                lstFiltroCC: getValoresMultiples("#cboCC"),
                lstFiltroSubC: getValoresMultiples("#cboSubContratista"),
                lstFiltroEspecialidad: getValoresMultiples("#cboEspecialidad"),
                fechaFiltroInicio: fechaFiltroInicio.val(),
                fechaFiltroFin: fechaFiltroFin.val(),
                estado_id: +selectEstado.val(),
                municipio_id: +selectMunicipio.val(),
                listaEspecialidades: cboEspecialidad.val().map((x) => { return +x })
            }
            axios.post("GetGraficaCumplimientoPorElementos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    initChartCumplimientoGlobalElementos(response.data.lstConcepts, response.data.lstData);
                    // $('#panelCumplimientoGlobalSubcontratista').addClass('show');

                }
            }).catch(error => Alert2Error(error.message));
        }

        function initChartCumplimientoGlobalElementos(concepts, data) {
            let cumplimientoOptimo = [];

            for (var item of data) {
                cumplimientoOptimo.push(90);

            }

            Highcharts.chart('chartPorcentajeCumplimientoGlobalElementos', {
                chart: {
                    type: 'column'
                },
                lang: highChartsDicEsp,
                credits: { enabled: false },
                title: {
                    text: 'CUMPLIMIENTO GLOBAL POR ELEMENTOS'
                },
                xAxis: {
                    categories: concepts,
                    crosshair: true
                },
                yAxis: {
                    max: 100,
                    title: {
                        useHTML: true,
                        text: '% de Cumplimiento'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            max: 100,
                            beginAtZero: true
                        }
                    }]
                },
                series: [{
                    name: 'Cumplimiento',
                    data: data
                },
                {
                    type: 'spline',
                    name: 'Cumplimiento optimo',
                    color: '#E12121'
                },]
            });

            // fncGenerarReporteCrystalReport($("#chartPorcentajeCumplimientoGlobalElementos").highcharts(), 'chart', 2);
            // fncGenerarReporteCrystalReport($("#chartPorcentajeCumplimientoGlobalElementos").highcharts(), 'chart', 2);
            // fncGenerarReporteCrystalReport($("#chartPorcentajeCumplimientoGlobalElementos").highcharts(), 'chart', 2);
        }

        function fncGetGraficaCumplimientoPorEvaluacion() {
            let obj = {
                lstFiltroCC: getValoresMultiples("#cboCC"),
                lstFiltroSubC: getValoresMultiples("#cboSubContratista"),
                lstFiltroEspecialidad: getValoresMultiples("#cboEspecialidad"),
                fechaFiltroInicio: fechaFiltroInicio.val(),
                fechaFiltroFin: fechaFiltroFin.val(),
                estado_id: +selectEstado.val(),
                municipio_id: +selectMunicipio.val(),
                listaEspecialidades: cboEspecialidad.val().map((x) => { return +x })
            }

            axios.post("GetGraficaCumplimientoPorEvaluacion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...

                    initChartCumplimientoGlobalEvaluacion(response.data.lstConcepts, response.data.lstData);
                    // $('#panelCumplimientoGlobalSubcontratista').addClass('show');

                }
            }).catch(error => Alert2Error(error.message));
        }

        function initChartCumplimientoGlobalEvaluacion(concepts, data) {
            let cumplimientoOptimo = [];

            for (var item of data) {
                cumplimientoOptimo.push(90);
            }

            Highcharts.chart('chartPorcentajeCumplimientoGlobalEvaluacion', {
                chart: {
                    type: 'column'
                },
                lang: highChartsDicEsp,
                credits: { enabled: false },
                title: {
                    text: 'CUMPLIMIENTO GLOBAL POR EVALUACIÓN'
                },
                xAxis: {
                    categories: concepts,
                    crosshair: true
                },
                yAxis: {
                    max: 100,
                    title: {
                        useHTML: true,
                        text: '% de cumplimiento'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            max: 100,
                            beginAtZero: true
                        }
                    }]
                },
                series: [{
                    name: 'Cumplimiento',
                    data: data

                },
                {
                    type: 'spline',
                    name: 'Cumplimiento optimo',
                    data: cumplimientoOptimo,
                    color: '#E12121'
                },]
            });

            // fncGenerarReporteCrystalReport($("#chartPorcentajeCumplimientoGlobalEvaluacion").highcharts(), 'chart', 3);
            // fncGenerarReporteCrystalReport($("#chartPorcentajeCumplimientoGlobalEvaluacion").highcharts(), 'chart', 3);
            // fncGenerarReporteCrystalReport($("#chartPorcentajeCumplimientoGlobalEvaluacion").highcharts(), 'chart', 3);
        }

        function fncCrearReporte() {
            let obj = {
                lstFiltroCC: getValoresMultiples("#cboCC"),
                lstFiltroSubC: getValoresMultiples("#cboSubContratista"),
                lstFiltroEspecialidad: getValoresMultiples("#cboEspecialidad"),
                fechaFiltroInicio: fechaFiltroInicio.val(),
                fechaFiltroFin: fechaFiltroFin.val(),
                estado_id: +selectEstado.val(),
                municipio_id: +selectMunicipio.val(),
                listaEspecialidades: cboEspecialidad.val().map((x) => { return +x })
            }

            axios.post("creaVariableDeSesion", obj).then(response => {
                location.href = '/SubContratistas/EvaluacionSubcontratista/crearReporte';
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                }


            }).catch(error => Alert2Error(error.message));
        }

        EXPORT_WIDTH = 1000;
        function fncGenerarReporteCrystalReport(chart, filename, numGrafica, downloadCallback) {
            if (chart == undefined) {
                return "";
            }

            var render_width = EXPORT_WIDTH;
            var render_height = render_width * chart.chartHeight / chart.chartWidth

            var svg = chart.getSVG({
                exporting: {
                    sourceWidth: chart.chartWidth,
                    sourceHeight: chart.chartHeight
                }
            });

            var canvas = document.createElement('canvas');
            canvas.height = render_height;
            canvas.width = render_width;

            var image = new Image;
            image.onload = function () {
                canvas.getContext('2d').drawImage(this, 0, 0, render_width, render_height);
                var data = canvas.toDataURL("image/png");
                downloadCallback(data, null, numGrafica);
                // download(data, filename + '.png', numGrafica);
            };
            // image.src = 'data:image/svg+xml;base64,' + window.btoa(svg);

            var imgsrc = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(svg)));
            // // var img = new Image(1, 1); // width, height values are optional params 
            image.src = imgsrc;

        }

        function download(data, filename, numGrafica) {
            var a = document.createElement('a');
            document.body.appendChild(a);
            switch (numGrafica) {
                case 1:
                    graficaSubContratistasDashboard = data;
                    break;
                case 2:
                    graficaElementosDashboard = data;
                    break;
                case 3:
                    graficaEvaluacionDashboard = data;
                    break;
                default:
                    break;
            }
            countGraficas++;

            console.log(graficaSubContratistasDashboard != undefined,
                graficaElementosDashboard != undefined,
                graficaEvaluacionDashboard != undefined);


            if (countGraficas === 3) {
                fncSaveImgSession();
            }
        }

        function fncSaveImgSession() {
            console.log(graficaSubContratistasDashboard != undefined,
                graficaElementosDashboard != undefined,
                graficaEvaluacionDashboard != undefined);

            axios.post("SaveImgSessionDashboard", { graficaSubcontratista: graficaSubContratistasDashboard, graficaElementos: graficaElementosDashboard, graficaEvaluacion: graficaEvaluacionDashboard }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncCrearReporte();

                    graficaSubContratistasDashboard = undefined;
                    graficaElementosDashboard = undefined;
                    graficaEvaluacionDashboard = undefined;
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
        // , $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        //#region GRAFICAS DETALLES

        function fncGetGraficaCumplimientosDetalles(elemento) {
            let obj = {
                lstFiltroCC: getValoresMultiples("#cboCC"),
                lstFiltroSubC: getValoresMultiples("#cboSubContratista"),
                lstFiltroEspecialidad: getValoresMultiples("#cboEspecialidad"),
                fechaFiltroInicio: fechaFiltroInicio.val(),
                fechaFiltroFin: fechaFiltroFin.val(),
                idElemento: elemento,
                estado_id: +selectEstado.val(),
                municipio_id: +selectMunicipio.val(),
                listaEspecialidades: cboEspecialidad.val().map((x) => { return +x })
            }
            axios.post("GetCumplimientosElementos", obj,).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...

                    initChartCumplimientoDetalles(response.data.lstConcepts, response.data.lstData, elemento);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initChartCumplimientoDetalles(concepts, data, elemento) {
            axios.post('FillComboElementos').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let cumplimientoOptimo = [];

                    for (var item of data) {
                        cumplimientoOptimo.push(90);
                    }
                    var nombreChartGrafica = "chartPorcentajeCumplimientos" + elemento;
                    Highcharts.chart(nombreChartGrafica, {
                        chart: {
                            type: 'column'
                        },
                        lang: highChartsDicEsp,
                        credits: { enabled: false },
                        title: {
                            text: 'CUMPLIMIENTOS'
                        },
                        xAxis: {
                            categories: concepts,
                            crosshair: true
                        },
                        yAxis: {
                            max: 100,
                            title: {
                                useHTML: true,
                                text: '% de cumplimiento'
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                            footerFormat: '</table>',
                            shared: true,
                            useHTML: true
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.2,
                                borderWidth: 0
                            }
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    max: 100,
                                    beginAtZero: true
                                }
                            }]
                        },
                        series: [{
                            name: 'Cumplimiento',
                            data: data

                        },
                        {
                            type: 'spline',
                            name: 'Cumplimiento optimo',
                            data: cumplimientoOptimo,
                            color: '#E12121'
                        },]
                    });
                }
            })
        }

        //#endregion

        // $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        function initTablaReporteEjecutivo() {

            dtReporteEjecutivo = tblReporteEjecutivo.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                dom: 'Bfrtip',

                // scrollCollapse: true,
                columns:
                    [
                        { data: 'cc', title: 'CC' },
                        { data: 'numeroContrato', title: 'Contrato' },
                        { data: 'subContratista', title: 'Subcontratista' },
                        {
                            data: 'confiabilidad', title: '% Confiabilidad',

                            createdCell: function (td, cellData, rowData, row, col) {
                                if (rowData.confiabilidad >= 80) {
                                    $(td).css('color', '#17BB1F');
                                } else if (rowData.confiabilidad > 60 && rowData.confiabilidad < 85) {
                                    $(td).css('color', '#E6ED0B');
                                }
                                else if (rowData.confiabilidad <= 60) {
                                    $(td).css('color', '#DE1A11');
                                }
                            }
                        },
                        { data: 'calificacionEval', title: '% Calificacion Global de evaluaciones' },
                        { data: 'cumplimientoSoporte', title: '% Cumplimiento carga de soportes(Tiempo)' },
                        { data: 'cumplimientoCompromisos', title: '% Cumplimiento compromisos' }
                    ],
                buttons: [
                    'excel'
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }

                ],
            }).columns.adjust();

        }

    }



    $(document).ready(() => {
        Subcontratistas.Dashboard = new Dashboard();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();