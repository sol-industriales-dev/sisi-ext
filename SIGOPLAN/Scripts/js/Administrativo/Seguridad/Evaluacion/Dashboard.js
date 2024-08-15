(() => {
    $.namespace('Administrativo.Evaluacion.Dashboard');
    Dashboard = function () {
        //#region Selectores
        const tablaDashboard = $('#tablaDashboard');
        const multiSelectCategoria = $('#multiSelectCategoria');
        // const multiSelectActividad = $('#multiSelectActividad');
        const selectEvaluador = $('#selectEvaluador');
        const inputMes = $('#inputMes');
        const selectCentroCosto = $('#selectCentroCosto');
        const botonResultados = $('#botonResultados');
        const botonReporte = $('#botonReporte');
        const report = $("#report");
        // const inputFechaInicio = $('#inputFechaInicio');
        // const inputFechaFin = $('#inputFechaFin');
        //#endregion

        let dtDashboard;

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioMesAnterior = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        fechaInicioMesAnterior.setMonth(fechaInicioMesAnterior.getMonth() - 1);
        const fechaFinMesAnterior = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        fechaFinMesAnterior.setDate(fechaFinMesAnterior.getDate() - 1);

        // Gráficas
        let chartPromedioGeneral = null;
        let chartPromedioPorAreas = null;
        let chartPromedioPorActividades = null;

        (function init() {
            agregarListeners();
            initMonthPicker(inputMes);

            // initMonthPicker(inputFechaInicio);
            // initMonthPicker(inputFechaFin);

            $('.select2').select2();

            selectCentroCosto.fillComboSeguridad(false);
            multiSelectCategoria.fillCombo('/Administrativo/Evaluacion/GetCategoriasCombo', null, false, 'Todos');
            selectEvaluador.fillCombo('/Administrativo/Evaluacion/GetEvaluadoresCombo', null, false, null);
            convertToMultiselect('#multiSelectCategoria');

            seleccionarTodosMultiselect('#multiSelectCategoria');
        })();

        function agregarListeners() {
            botonResultados.click(cargarResultados);
            botonReporte.click(verReporte);
        }

        function cargarResultados() {
            let idEmpresa = $(selectCentroCosto).getEmpresa();
            let strAgrupacion = $(selectCentroCosto).getAgrupador();
            let idAgrupacion;
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }
            console.log(idEmpresa);
            console.log(idAgrupacion);

            let mes = '01/' + inputMes.val();
            let cc = selectCentroCosto.val();
            let categorias = getValoresMultiples('#multiSelectCategoria');
            let evaluadorID = +(selectEvaluador.val());
            let idAgrupador = idAgrupacion;
            if (cc == '') {
                AlertaGeneral(`Alerta`, `Seleccione un Centro de Costos.`);
                return;
            }

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/CargarDashboard', { mes, idEmpresa, idAgrupador, categorias, evaluadorID })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        initChartPromedioGeneral(response.chartPromedioGeneral);
                        initChartPromedioPorAreas(response.chartPromedioPorAreas);
                        initChartPromedioPorActividades(response.chartPromedioPorActividades);

                        botonReporte.attr('disabled', !(response.data.length > 0));
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function initChartPromedioGeneral(data) {
            const { labels, datasets } = data;

            if (chartPromedioGeneral) {
                chartPromedioGeneral.destroy();
            }

            var ctx = document.getElementById('chartPromedioGeneral').getContext('2d');

            chartPromedioGeneral = new Chart(ctx, {
                type: 'pie',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: "% Cumplimiento General"
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
                    },
                    tooltips: {
                        mode: 'label',
                        callbacks: {
                            label: function (tooltipItem, data) {
                                return data['datasets'][0]['data'][tooltipItem['index']] + '%';
                            }
                        }
                    }
                }
            });
        }

        function initChartPromedioPorAreas(data) {
            const { labels, datasets } = data;

            if (chartPromedioPorAreas) {
                chartPromedioPorAreas.destroy();
            }

            let ctx = document.getElementById('chartPromedioPorAreas').getContext('2d');

            chartPromedioPorAreas = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: "% Cumplimiento por Áreas"
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
                                beginAtZero: true,
                                suggestedMax: 100,
                                suggestedMin: 0,
                                callback: function (value, index, values) {
                                    return value + '%';
                                }
                            }
                        }]
                    },
                    tooltips: {
                        mode: 'label',
                        callbacks: {
                            label: function (tooltipItem, data) {
                                return data['datasets'][0]['data'][tooltipItem['index']] + '%';
                            }
                        }
                    }
                }
            });
        }

        function initChartPromedioPorActividades(data) {
            const { labels, datasets } = data;

            if (chartPromedioPorActividades) {
                chartPromedioPorActividades.destroy();
            }

            let ctx = document.getElementById('chartPromedioPorActividades').getContext('2d');

            chartPromedioPorActividades = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: "% Cumplimiento por Actividades"
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
                                beginAtZero: true,
                                suggestedMax: 100,
                                suggestedMin: 0,
                                callback: function (value, index, values) {
                                    return value + '%';
                                }
                            }
                        }]
                    },
                    tooltips: {
                        mode: 'label',
                        callbacks: {
                            label: function (tooltipItem, data) {
                                return data['datasets'][0]['data'][tooltipItem['index']] + '%';
                            }
                        }
                    }
                }
            });
        }

        function verReporte() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", '/Reportes/Vista.aspx?idReporte=127');
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

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
            }).datepicker("setDate", fechaInicioMesAnterior);
        }
    }
    $(document).ready(() => Administrativo.Evaluacion.Dashboard = new Dashboard())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();