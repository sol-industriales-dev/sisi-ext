(() => {
    $.namespace('SubContratistas.Proyectos');
    Proyectos = function () {
        //#region Selectores
        const botonAgregar = $('#botonAgregar');
        const tablaProyectos = $('#tablaProyectos');
        const modalProyectos = $('#modalProyectos');
        const botonGuardar = $('#botonGuardar');
        const inputNombre = $('#inputNombre');
        //#endregion

        let dtProyectos;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            $('.select2').select2();
            initTablaProyectos();
            agregarListeners();
            cargarProyectos();
        })();

        function initTablaProyectos() {
            dtProyectos = tablaProyectos.DataTable({
                retrieve: true,
                paging: true,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaProyectos.on('click', '.btn-editar', function () {
                        let rowData = dtProyectos.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        modalProyectos.modal('show');
                    });

                    tablaProyectos.on('click', '.btn-eliminar', function () {
                        let rowData = dtProyectos.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el proyecto?`,
                            () => eliminarProyecto(rowData.id))
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'nombre', title: 'Nombre' },
                    {
                        render: function (data, type, row, meta) {
                            return `
                            <button title="Editar" class="btn-editar btn btn-xs btn-warning">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            &nbsp;
                            <button title="Eliminar" class="btn-eliminar btn btn-xs btn-danger">
                                <i class="fas fa-trash"></i>
                            </button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '70%', targets: [0] }
                ]
            });
        }

        function cargarProyectos() {
            axios.post('GetProyectos')
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaProyectos, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function agregarListeners() {
            botonAgregar.click(() => {
                limpiarModal();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                modalProyectos.modal('show');
            });
            botonGuardar.click(guardarProyecto);
        }

        function limpiarModal() {
            inputNombre.val('');
        }

        function llenarCamposModal(data) {
            inputNombre.val(data.nombre);
        }

        function guardarProyecto() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevoProyecto();
                    break;
                case ESTATUS.EDITAR:
                    editarProyecto();
                    break;
            }
        }

        function nuevoProyecto() {
            let proyecto = getInformacionProyecto();

            axios.post('GuardarNuevoProyecto', { proyecto })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalProyectos.modal('hide');
                        Alert2Exito('Se ha guardado la información.');
                        cargarProyectos();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarProyecto() {
            let proyecto = getInformacionProyecto();

            axios.post('EditarProyecto', { proyecto })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalProyectos.modal('hide')
                        Alert2Exito('Se ha guardado la información');
                        cargarProyectos();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarProyecto(id) {
            let proyecto = { id };

            axios.post('EliminarProyecto', { proyecto })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado la información.');
                        cargarProyectos();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function getInformacionProyecto() {
            return {
                id: botonGuardar.data().id,
                nombre: inputNombre.val(),
                estatus: true
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => SubContratistas.SubContratistas = new Proyectos())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();