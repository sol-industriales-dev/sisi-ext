(() => {
    $.namespace('Administrativo.Requerimientos.Dashboard');

    Dashboard = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const selectCentroCostoClasificacion = $('#selectCentroCostoClasificacion');
        const botonResultados = $('#botonResultados');
        const botonResultadosClasificacion = $('#botonResultadosClasificacion');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const inputFechaInicioClasificacion = $('#inputFechaInicioClasificacion');
        const inputFechaFinClasificacion = $('#inputFechaFinClasificacion');
        const selectDivision = $('#selectDivision');
        const selectLineaNegocio = $('#selectLineaNegocio');
        const selectDivisionClasificacion = $('#selectDivisionClasificacion');
        const selectLineaNegocioClasificacion = $('#selectLineaNegocioClasificacion');
        const selectRequerimiento = $('#selectRequerimiento');
        const selectClasificacion = $('#selectClasificacion');
        //#endregion

        // Gráficas
        let chartGeneral = null;
        let chartRequerimientos = null;
        let chartSecciones = null;
        let chartClasificaciones = null;
        let ultimoCentroCostoSeleccionado = null;
        let ultimoCentroCostoSeleccionadoClasificacion = null;

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioMesAnterior = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        fechaInicioMesAnterior.setMonth(fechaInicioMesAnterior.getMonth() - 1);
        const fechaFinMesAnterior = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        fechaFinMesAnterior.setDate(fechaFinMesAnterior.getDate() - 1);

        (function init() {
            agregarListeners();

            $('.select2').select2();

            $('#selectDivision').fillCombo('/Administrativo/Requerimientos/GetDivisionesCombo', null, false, 'Todos');
            convertToMultiselect('#selectDivision');
            $('#selectLineaNegocio').fillCombo('/Administrativo/Requerimientos/GetLineaNegocioCombo', { division: 0 }, false, 'Todos');
            convertToMultiselect('#selectLineaNegocio');

            selectDivisionClasificacion.fillCombo('/Administrativo/Requerimientos/GetDivisionesCombo', null, false, 'Todos');
            convertToMultiselect('#selectDivisionClasificacion');
            $('#selectLineaNegocioClasificacion').fillCombo('/Administrativo/Requerimientos/GetLineaNegocioCombo', { division: 0 }, false, 'Todos');
            convertToMultiselect('#selectLineaNegocioClasificacion');
            selectCentroCosto.fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivision').val(), $('#selectLineaNegocio').val());
            convertToMultiselect('#selectCentroCosto');

            selectCentroCostoClasificacion.fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivisionClasificacion').val(), $('#selectLineaNegocioClasificacion').val());
            convertToMultiselect('#selectCentroCostoClasificacion');
            selectRequerimiento.fillCombo('/Administrativo/Requerimientos/FillComboRequerimientosDashboard', { division: 0, listaCC: null }, false, 'Todos');
            convertToMultiselect('#selectRequerimiento');
            selectClasificacion.fillCombo('/Administrativo/Requerimientos/GetClasificacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectClasificacion');

            inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioMesAnterior);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaFinMesAnterior);
            inputFechaInicioClasificacion.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioMesAnterior);
            inputFechaFinClasificacion.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaFinMesAnterior);
        })();

        $('#selectDivision').on('change', function () {
            selectCentroCosto.fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivision').val(), $('#selectLineaNegocio').val());
            convertToMultiselect('#selectCentroCosto');
        });

        $('#selectLineaNegocio').on('change', function () {
            selectCentroCosto.fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivision').val(), $('#selectLineaNegocio').val());
            convertToMultiselect('#selectCentroCosto');
        });

        $('#selectDivisionClasificacion').on('change', function () {
            selectCentroCostoClasificacion.fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivisionClasificacion').val(), $('#selectLineaNegocioClasificacion').val());
            convertToMultiselect('#selectCentroCostoClasificacion');
        });

        $('#selectLineaNegocioClasificacion').on('change', function () {
            selectCentroCostoClasificacion.fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivisionClasificacion').val(), $('#selectLineaNegocioClasificacion').val());
            convertToMultiselect('#selectCentroCostoClasificacion');
        });

        function agregarListeners() {
            botonResultados.click(cargarResultados);
            botonResultadosClasificacion.click(cargarResultadosClasificacion)
            // botonReporte.click(verReporte);
        }

        selectDivision.on('change', function () {
            let division = +(selectDivision.val());

            if (division > 0) {
                selectCentroCosto.fillComboSeguridad(false, division, false);
                convertToMultiselect('#selectCentroCosto');
            } else {
                selectCentroCosto.fillComboSeguridad(false, 0, false);
                convertToMultiselect('#selectCentroCosto');
            }
        });

        selectDivisionClasificacion.on('change', function () {
            let division = +(selectDivisionClasificacion.val());

            if (division > 0) {
                selectCentroCostoClasificacion.fillComboSeguridad(false, division, false);
                convertToMultiselect('#selectCentroCostoClasificacion');
            } else {
                selectCentroCostoClasificacion.fillComboSeguridad(false, 0, false);
                convertToMultiselect('#selectCentroCostoClasificacion');
            }
        });

        selectCentroCosto.on('change', function () {
            if ($(this).val().length > 5) {
                $(this).val(ultimoCentroCostoSeleccionado);
                $('#selectCentroCosto').multiselect('deselect', [$(this).val()]).multiselect("refresh");
            } else {
                ultimoCentroCostoSeleccionado = $(this).val();
            }
        });

        selectCentroCostoClasificacion.on('change', function () {
            if ($(this).val().length > 5) {
                $(this).val(ultimoCentroCostoSeleccionadoClasificacion);
                $('#selectCentroCostoClasificacion').multiselect('deselect', [$(this).val()]).multiselect("refresh");
            } else {
                ultimoCentroCostoSeleccionadoClasificacion = $(this).val();
            }
        });

        function cargarResultados() {
            //#region SE ELIMINA LOS PARAMETROS ADICIONALES DEL VALUE EN CASO DE SER CONTRATISTA O AGRUPACION DE CONTRATISTAS
            let objGrupos = new Object();
            let arrGrupos = [];
            for (let i = 0; i < $("#selectCentroCosto").getMultiSeg().length; i++) {
                let str = $("#selectCentroCosto").getMultiSeg()[i].idAgrupacion;
                let idEmpresa = $("#selectCentroCosto").getMultiSeg()[i].idEmpresa;
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
                        idEmpresa: $("#selectCentroCosto").getMultiSeg()[i].idEmpresa,
                        idAgrupacion: $("#selectCentroCosto").getMultiSeg()[i].idAgrupacion
                    };
                    arrGrupos.push(objGrupos);
                }
            }
            //#endregion

            // let arrGrupos = $(selectCentroCosto).getMultiSeg();
            let listaRequerimientos = getValoresMultiples('#selectRequerimiento');
            let fechaInicio = inputFechaInicio.val();
            let fechaFin = inputFechaFin.val();

            if (arrGrupos.length == 0) {
                AlertaGeneral(`Alerta`, `Seleccione un Centro de Costos.`);
                return;
            }

            if (listaRequerimientos.length == 0) {
                AlertaGeneral(`Alerta`, `Seleccione un requerimiento.`);
                return;
            }

            let listaDivisiones = selectDivision.val();
            let listaLineasNegocio = selectLineaNegocio.val();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/CargarDashboard', { listaDivisiones, listaLineasNegocio, arrGrupos, listaRequerimientos, fechaInicio, fechaFin })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        initChartGeneral(response.chartGeneral);
                        initChartRequerimientos(response.chartRequerimientos);
                        initChartSecciones(response.chartSecciones);

                        // botonReporte.attr('disabled', !(response.data.length > 0));
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cargarResultadosClasificacion() {

            //#region SE ELIMINA LOS PARAMETROS ADICIONALES DEL VALUE EN CASO DE SER CONTRATISTA O AGRUPACION DE CONTRATISTAS
            let objGrupos = new Object();
            let arrGrupos = [];
            for (let i = 0; i < $("#selectCentroCostoClasificacion").getMultiSeg().length; i++) {
                let str = $("#selectCentroCostoClasificacion").getMultiSeg()[i].idAgrupacion;
                let idEmpresa = $("#selectCentroCostoClasificacion").getMultiSeg()[i].idEmpresa;
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
                        idEmpresa: $("#selectCentroCostoClasificacion").getMultiSeg()[i].idEmpresa,
                        idAgrupacion: $("#selectCentroCostoClasificacion").getMultiSeg()[i].idAgrupacion
                    };
                    arrGrupos.push(objGrupos);
                }
            }
            //#endregion

            // let arrGrupos = $(selectCentroCostoClasificacion).getMultiSeg();
            let fechaInicio = inputFechaInicioClasificacion.val();
            let fechaFin = inputFechaFinClasificacion.val();

            if (arrGrupos.length == 0) {
                AlertaGeneral(`Alerta`, `Seleccione un Centro de Costos.`);
                return;
            }

            let listaDivisiones = selectDivisionClasificacion.val();
            let listaLineasNegocio = selectLineaNegocioClasificacion.val();
            let listaClasificaciones = getValoresMultiples('#selectClasificacion');

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/CargarDashboardClasificacion', { listaDivisiones, listaLineasNegocio, arrGrupos, listaClasificaciones, fechaInicio, fechaFin })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        initChartClasificaciones(response.chartClasificacion);

                        // botonReporte.attr('disabled', !(response.data.length > 0));
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function initChartGeneral(data) {
            const { labels, datasets } = data;

            if (chartGeneral) {
                chartGeneral.destroy();
            }

            var ctx = document.getElementById('chartGeneral').getContext('2d');

            chartGeneral = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: false,
                        text: "% Cumplimiento General por Centro de Costo"
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
                        xAxes: [{
                            ticks: {
                                // maxRotation: 90,
                                // minRotation: 90,
                                callback: function (value, index, values) {
                                    return value;
                                }
                            }
                        }],
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

        function initChartRequerimientos(data) {
            const { labels, datasets } = data;

            if (chartRequerimientos) {
                chartRequerimientos.destroy();
            }

            let ctx = document.getElementById('chartRequerimientos').getContext('2d');

            chartRequerimientos = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: false,
                        text: "% Cumplimiento por Requerimientos"
                    },
                    legend: {
                        display: true,
                        position: 'bottom'
                    },
                    plugins: {
                        labels: {
                            render: 'value'
                        }
                    },
                    scales: {
                        xAxes: [{
                            // ticks: {
                            //     maxRotation: 0,
                            //     minRotation: 0
                            // }
                        }],
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
                                var label = data.datasets[tooltipItem.datasetIndex].label || '';
                                var porcentaje = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index] || '';

                                if (label) {
                                    label += `: ${+(porcentaje) >= 0 ? +(porcentaje) : 0}%`;
                                }

                                return label;
                            }
                        }
                    }
                }
            });
        }

        function initChartSecciones(data) {
            const { labels, datasets } = data;

            if (chartSecciones) {
                chartSecciones.destroy();
            }

            let ctx = document.getElementById('chartSecciones').getContext('2d');

            chartSecciones = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: false,
                        text: "% Cumplimiento por Secciones"
                    },
                    legend: {
                        display: true,
                        position: 'bottom'
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
                                var label = data.datasets[tooltipItem.datasetIndex].label || '';
                                var porcentaje = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index] || '';

                                if (label) {
                                    label += `: ${+(porcentaje) >= 0 ? +(porcentaje) : 0}%`;
                                }

                                return label;
                            }
                        }
                    }
                }
            });
        }

        function initChartClasificaciones(data) {
            const { labels, datasets } = data;

            if (chartClasificaciones) {
                chartClasificaciones.destroy();
            }

            let ctx = document.getElementById('chartClasificaciones').getContext('2d');

            chartClasificaciones = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: false,
                        text: "% Cumplimiento por Clasificaciones"
                    },
                    legend: {
                        display: true,
                        position: 'bottom'
                    },
                    plugins: {
                        labels: {
                            render: 'value'
                        }
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                maxRotation: 0,
                                minRotation: 0
                            }
                        }],
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
                                var label = data.datasets[tooltipItem.datasetIndex].label || '';
                                var porcentaje = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index] || '';

                                if (label) {
                                    label += `: ${+(porcentaje) >= 0 ? +(porcentaje) : 0}%`;
                                }

                                return label;
                            }
                        }
                    }
                }
            });
        }

        // function verReporte() {
        //     $.blockUI({ message: 'Procesando...', baseZ: 2000 });

        //     report.attr("src", '/Reportes/Vista.aspx?idReporte=');
        //     report.on('load', function () {
        //         $.unblockUI();
        //         openCRModal();
        //     });
        // }
    }

    $(document).ready(() => Administrativo.Requerimientos.Dashboard = new Dashboard())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();