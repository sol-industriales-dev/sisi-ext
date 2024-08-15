(() => {
    $.namespace('SAAP.SAAP.Asignacion');
    Asignacion = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const botonBuscar = $('#botonBuscar');
        const botonAgregar = $('#botonAgregar');
        const tablaActividades = $('#tablaActividades');
        const modalAgregarActividad = $('#modalAgregarActividad');
        const selectCentroCostoAgregar = $('#selectCentroCostoAgregar');
        const selectAreaAgregar = $('#selectAreaAgregar');
        const selectClasificacionAgregar = $('#selectClasificacionAgregar');
        const inputFechaInicioEvaluacion = $('#inputFechaInicioEvaluacion');
        const tablaActividadesAgregar = $('#tablaActividadesAgregar');
        const botonGuardar = $('#botonGuardar');
        const botonQuitar = $('#botonQuitar');
        //#endregion

        let dtActividades;
        let dtActividadesAgregar;

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();

        let _contadorQuitar = 0;

        (function init() {
            agregarListeners();
            initTablaActividades();
            initTablaActividadesAgregar();
            $('.select2').select2();

            selectCentroCosto.fillCombo('/SAAP/SAAP/GetAgrupacionCombo', null, false, null);
            selectCentroCostoAgregar.fillCombo('/SAAP/SAAP/GetAgrupacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectCentroCostoAgregar');
            selectAreaAgregar.fillCombo('/SAAP/SAAP/GetAreaCombo', null, false, 'Todos');
            convertToMultiselect('#selectAreaAgregar');
            selectClasificacionAgregar.fillCombo('/SAAP/SAAP/GetClasificacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectClasificacionAgregar');
        })();

        selectAreaAgregar.on('change', function () {
            let areas = getValoresMultiples('#selectAreaAgregar');

            selectClasificacionAgregar.fillCombo('/SAAP/SAAP/GetClasificacionCombo', { areas }, false, 'Todos');
            convertToMultiselect('#selectClasificacionAgregar');

            cargarActividadesAplicables();
        });

        selectClasificacionAgregar.on('change', function () {
            cargarActividadesAplicables();
        });

        modalAgregarActividad.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function agregarListeners() {
            botonBuscar.click(cargarAsignacion);
            botonGuardar.click(guardarAsignacion);
            botonAgregar.click(() => {
                limpiarModal();
                modalAgregarActividad.modal('show');
            });
            botonQuitar.click(() => {
                AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de quitar las asignaciones seleccionadas?`, () => eliminarAsignacionesActividades());
            });
        }

        function limpiarModal() {
            selectCentroCostoAgregar.fillCombo('/SAAP/SAAP/GetAgrupacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectCentroCostoAgregar');
            selectAreaAgregar.fillCombo('/SAAP/SAAP/GetAreaCombo', null, false, 'Todos');
            convertToMultiselect('#selectAreaAgregar');
            selectClasificacionAgregar.fillCombo('/SAAP/SAAP/GetClasificacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectClasificacionAgregar');

            inputFechaInicioEvaluacion.datepicker({ dateFormat, minDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);

            dtActividadesAgregar.clear().draw();
        }

        function initTablaActividades() {
            dtActividades = tablaActividades.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    // tablaActividades.on('click', '.btn-eliminar', function () {
                    //     let rowData = dtActividades.row($(this).closest('tr')).data();

                    //     AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de quitar la asignación de la actividad "${rowData.indice} - ${rowData.descripcion}"?`,
                    //         () => eliminarAsignacionActividad(rowData.id))
                    // });

                    tablaActividades.on('click', 'input[type="checkbox"]', function () {
                        if ($(this).prop('checked')) {
                            _contadorQuitar++;
                        } else {
                            _contadorQuitar--;
                        }

                        if (_contadorQuitar > 0) {
                            botonQuitar.attr('disabled', false);
                        } else {
                            botonQuitar.attr('disabled', true);
                        }
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'areaDesc', title: 'Área' },
                    { data: 'clasificacionDesc', title: 'Clasificación' },
                    { data: 'porcentaje', title: 'Porcentaje' },
                    { data: 'diasCompromiso', title: 'Días Compromiso' },
                    { data: 'fechaInicioEvaluacionString', title: 'Fecha Inicio Evaluación' },
                    {
                        title: 'Quitar', render: function (data, type, row, meta) {
                            return `
                                <div>
                                    <input id="checkboxQuitar_${meta.row}" type="checkbox" class="form-control regular-checkboxdanger checkboxQuitar" style="height: 25px;">
                                    <label for="checkboxQuitar_${meta.row}"></label>
                                </div>
                            `;

                            // return `<button class="btn-eliminar btn btn-xs btn-danger"><i class="fas fa-times"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaActividadesAgregar() {
            dtActividadesAgregar = tablaActividadesAgregar.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaActividadesAgregar.on('click', '.btn-quitar', function () {
                        dtActividadesAgregar.row($(this).closest('tr')).remove().draw();
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'areaDesc', title: 'Área' },
                    { data: 'clasificacionDesc', title: 'Clasificación' },
                    { data: 'porcentaje', title: 'Porcentaje' },
                    { data: 'diasCompromiso', title: 'Días Compromiso' },
                    {
                        title: 'Quitar', render: function (data, type, row, meta) {
                            return `<button class="btn-quitar btn btn-xs btn-danger"><i class="fas fa-times"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '30%', targets: [1] }
                ]
            });
        }

        function cargarAsignacion() {
            if (selectCentroCosto.getAgrupador() != '') {
                axios.post('/SAAP/SAAP/GetAsignacionActividades', { agrupacion_id: selectCentroCosto.getAgrupador() })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            if (response.data.data.length > 0) {
                                AddRows(tablaActividades, response.data.data);
                                _contadorQuitar = 0;
                                botonQuitar.attr('disabled', true);
                            } else {
                                AlertaGeneral(`Alerta`, `La agrupación no tiene actividades asignadas.`);
                                dtActividades.clear().draw();
                            }
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function guardarAsignacion() {
            let agrupaciones = getValoresMultiples('#selectCentroCostoAgregar');
            let actividades = [];

            tablaActividadesAgregar.find('tbody tr').each(function (index, row) {
                let rowData = dtActividadesAgregar.row(row).data();

                if (rowData != undefined) {
                    actividades.push(+rowData.id);
                }
            });

            if (actividades.length == 0) {
                Alert2Warning('No hay actividades seleccionadas.');
                return;
            }

            axios.post('/SAAP/SAAP/GuardarAsignacionActividades', { agrupaciones, fechaInicioEvaluacion: inputFechaInicioEvaluacion.val(), actividades })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalAgregarActividad.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarAsignacion();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarAsignacionesActividades() {
            let listaAsignaciones_id = [];

            tablaActividades.find('tbody tr').each((index, row) => {
                let checkbox = $(row).find('.checkboxQuitar');

                if ($(checkbox).prop('checked')) {
                    listaAsignaciones_id.push(dtActividades.row(row).data().id);
                }
            });

            axios.post('/SAAP/SAAP/EliminarAsignacionesActividades', { listaAsignaciones_id }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                    cargarAsignacion();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarActividadesAplicables() {
            let areas = getValoresMultiples('#selectAreaAgregar');
            let clasificaciones = getValoresMultiples('#selectClasificacionAgregar');

            axios.post('/SAAP/SAAP/GetActividadesAplicables', { areas, clasificaciones })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaActividadesAgregar, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => SAAP.SAAP.Asignacion = new Asignacion())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...', baseZ: 2000 }))
        .ajaxStop($.unblockUI);
})();