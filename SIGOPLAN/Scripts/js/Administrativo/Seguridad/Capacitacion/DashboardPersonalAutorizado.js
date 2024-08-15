(() => {
    $.namespace('Adminstrativo.Seguridad.Capacitacion.DashboardPersonalAutorizado');
    DashboardPersonalAutorizado = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const selectArea = $('#selectArea');
        const selectActividadAutorizada = $('#selectActividadAutorizada');
        const botonBuscar = $('#botonBuscar');
        const spanAutorizados = $('#spanAutorizados');
        const spanPersonalPuesto = $('#spanPersonalPuesto');
        const checkboxTodasActividades = $('#checkboxTodasActividades');
        const spanPersonalDC3 = $('#spanPersonalDC3');
        const tblTablaAutorizante = $('#tblTablaAutorizante');
        //#endregion

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);

        let chartPersonalAutorizado = null;
        let chartDC3 = null;

        (function init() {
            $('.select2').select2();
            TablaAutorizados();
            llenarCombos();
            botonBuscar.click(cargarDatosDashboard);
            selectCentroCosto.change(cargarAreasCC);
        })();

        checkboxTodasActividades.on('click', function () {
            if (checkboxTodasActividades.is(':checked')) {
                selectActividadAutorizada.find('option').prop("selected", true);
                selectActividadAutorizada.trigger("change");
            } else {
                selectActividadAutorizada.find('option').prop("selected", false);
                selectActividadAutorizada.trigger("change");
            }
        });

        $(document).ready(function () {
            selectActividadAutorizada.select2();
            selectActividadAutorizada.val();
            selectActividadAutorizada.trigger("change");
        });

        function llenarCombos() {
            axios.get('ObtenerComboCCAmbasEmpresas')
                .then(response => {
                    let { success, items, message } = response.data;

                    if (success) {
                        items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;

                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                            });

                            selectCentroCosto.append('<option value="Todos">Todos</option>');
                            selectCentroCosto.append(groupOption);
                        });
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }

                    convertToMultiselect('#selectCentroCosto');
                }).catch(error => AlertaGeneral(`Alerta`, error.message));

            convertToMultiselect('#selectArea');

            selectActividadAutorizada.fillCombo('ObtenerComboCursos', null, false, '');
            selectActividadAutorizada.find('option[value=""]').remove();
            // convertToMultiselect('#selectActividadAutorizada');
        }

        function cargarDatosDashboard() {
            let listaCentrosCosto = [];

            getValoresMultiplesCustom('#selectCentroCosto').forEach(x => {
                listaCentrosCosto.push({
                    cc: x.value,
                    empresa: +(x.empresa)
                });
            });

            let listaAreas = [];

            getValoresMultiplesCustom('#selectArea').forEach(x => {
                listaAreas.push({
                    cc: x.cc,
                    departamento: x.value,
                    empresa: +(x.empresa)
                });
            });

            let filtros = {
                listaCCConstruplan: listaCentrosCosto.filter((x) => { return x.empresa == 1; }).map(function (x) { return x.cc; }),
                listaCCArrendadora: listaCentrosCosto.filter((x) => { return x.empresa == 2; }).map(function (x) { return x.cc; }),
                listaAreas: listaAreas,
                listaCursosID: getValoresMultiples('#selectActividadAutorizada')
            }

            axios.post('CargarDashboardPersonalAutorizado', { filtros })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        initChartPersonalAutorizado(response.data.graficaPersonalAutorizado);
                        initChartDC3(response.data.graficaDC3);

                        AddRows(tblTablaAutorizante, response.data.tablaListaAsistentes);
                        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initChartPersonalAutorizado(data) {
            const { labels, datasets } = data;

            if (chartPersonalAutorizado) {
                chartPersonalAutorizado.destroy();
            }

            let ctx = document.getElementById('chartPersonalAutorizado').getContext('2d');

            $('#rowInfo').css('display', 'block');

            spanAutorizados.text(datasets[0].data[0]);
            spanPersonalPuesto.text(datasets[0].data[1]);

            chartPersonalAutorizado = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels, datasets: [{
                        data: datasets[0].data,
                        backgroundColor: datasets[0].backgroundColor,
                        hoverBackgroundColor: ['#404040', '#404040']
                    }]
                },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: "Personal Autorizado"
                    },
                    legend: {
                        display: false,
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

        function initChartDC3(data) {
            const { labels, datasets } = data;

            if (chartDC3) {
                chartDC3.destroy();
            }

            let ctx = document.getElementById('chartDC3').getContext('2d');

            $('#rowInfo2').css('display', 'block');

            spanPersonalDC3.text(datasets[0].data[1]);

            chartDC3 = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels, datasets: [{
                        data: datasets[0].data,
                        backgroundColor: datasets[0].backgroundColor
                        // hoverBackgroundColor: ['#404040', '#404040']
                    }]
                },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: "PERSONAL CON DC-3"
                    },
                    legend: {
                        display: false,
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

        function cargarAreasCC() {
            let listaCentrosCosto = [];

            getValoresMultiplesCustom('#selectCentroCosto').forEach(x => {
                listaCentrosCosto.push({
                    cc: x.value,
                    empresa: +(x.empresa)
                });
            });

            if (listaCentrosCosto.length == 0) {
                selectArea.empty();
                convertToMultiselect('#selectArea');
                return;
            }

            let ccsCplan = listaCentrosCosto.filter((x) => { return x.empresa == 1; }).map(function (x) { return x.cc; });
            let ccsArr = listaCentrosCosto.filter((x) => { return x.empresa == 2; }).map(function (x) { return x.cc; });

            $.post('/Administrativo/Capacitacion/ObtenerAreasPorCC', { ccsCplan, ccsArr })
                .then(response => {
                    selectArea.empty();
                    if (response.success) {
                        // Operación exitosa.
                        // const todosOption = `<option value="Todos">Todos</option>`;
                        const option = `<option value="Todos">Todos</option>`;
                        selectArea.append(option);

                        response.items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" cc="${y.Id}" empresa="${y.Prefijo}">${y.Text}</option>`;
                            });
                            selectArea.append(groupOption);
                        });

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }

                    convertToMultiselect('#selectArea');
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getValoresMultiplesCustom(selector) {
            var _tempObj = $(selector + ' option:selected').map(function (a, item) {
                return { value: item.value, empresa: $(item).attr('empresa'), cc: $(item).attr('cc') };
            });
            var _tempArrObj = new Array();
            $.each(_tempObj, function (i, e) {
                _tempArrObj.push(e);
            });
            return _tempArrObj;
        }

        function TablaAutorizados() {
            dtTablaAutorizantes = tblTablaAutorizante.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                order: [[1, 'asc']],
                searching: false,
                bFilter: false,
                info: false,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    { data: "claveEmpleado", title: "# Empleado" },
                    { data: "nombreEmpleado", title: "Nombre" },
                    { data: "puestoDesc", title: "Puesto" },
                    { data: "ccDesc", title: "Centro de costo" }
                ],
                initComplete: function (settings, json) {
                    tblTablaAutorizante.on('click', '.classBtn', function () {
                        let rowData = dtTablaAutorizantes.row($(this).closest('tr')).data();
                    });
                    tblTablaAutorizante.on('click', '.classBtn', function () {
                        let rowData = dtTablaAutorizantes.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Adminstrativo.Seguridad.Capacitacion.DashboardPersonalAutorizado = new DashboardPersonalAutorizado())
})();