(() => {
    $.namespace('Administrativo.SeguimientoCompromisos.Asignacion');
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
        //#endregion

        let dtActividades;
        let dtActividadesAgregar;

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();

        (function init() {
            agregarListeners();
            initTablaActividades();
            initTablaActividadesAgregar();
            $('.select2').select2();

            selectCentroCosto.fillCombo('/Administrativo/SeguimientoCompromisos/GetAgrupacionCombo', null, false, null);
            selectCentroCostoAgregar.fillCombo('/Administrativo/SeguimientoCompromisos/GetAgrupacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectCentroCostoAgregar');
            selectAreaAgregar.fillCombo('/Administrativo/SeguimientoCompromisos/GetAreaCombo', null, false, 'Todos');
            convertToMultiselect('#selectAreaAgregar');
            selectClasificacionAgregar.fillCombo('/Administrativo/SeguimientoCompromisos/GetClasificacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectClasificacionAgregar');
        })();

        selectAreaAgregar.on('change', function () {
            let areas = getValoresMultiples('#selectAreaAgregar');

            selectClasificacionAgregar.fillCombo('/Administrativo/SeguimientoCompromisos/GetClasificacionCombo', { areas }, false, 'Todos');
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
        }

        function limpiarModal() {
            selectCentroCostoAgregar.fillCombo('/Administrativo/SeguimientoCompromisos/GetAgrupacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectCentroCostoAgregar');
            selectAreaAgregar.fillCombo('/Administrativo/SeguimientoCompromisos/GetAreaCombo', null, false, 'Todos');
            convertToMultiselect('#selectAreaAgregar');
            selectClasificacionAgregar.fillCombo('/Administrativo/SeguimientoCompromisos/GetClasificacionCombo', null, false, 'Todos');
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
                    tablaActividades.on('click', '.btn-eliminar', function () {
                        let rowData = dtActividades.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de quitar la asignación de la actividad "${rowData.indice} - ${rowData.descripcion}"?`,
                            () => eliminarAsignacionActividad(rowData.id))
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
                            return `<button class="btn-eliminar btn btn-xs btn-danger"><i class="fas fa-times"></i></button>`;
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
                axios.post('/Administrativo/SeguimientoCompromisos/GetAsignacionActividades', { agrupacion_id: selectCentroCosto.getAgrupador() })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            if (response.data.data.length > 0) {
                                AddRows(tablaActividades, response.data.data);
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

            axios.post('/Administrativo/SeguimientoCompromisos/GuardarAsignacionActividades', { agrupaciones, fechaInicioEvaluacion: inputFechaInicioEvaluacion.val(), actividades })
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

        function eliminarAsignacionActividad(id) {
            axios.post('/Administrativo/SeguimientoCompromisos/EliminarAsignacionActividad', { asignacion_id: id })
                .then(response => {
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

            axios.post('/Administrativo/SeguimientoCompromisos/GetActividadesAplicables', { areas, clasificaciones })
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
    $(document).ready(() => Administrativo.SeguimientoCompromisos.Asignacion = new Asignacion())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...', baseZ: 2000 }))
        .ajaxStop($.unblockUI);
})();