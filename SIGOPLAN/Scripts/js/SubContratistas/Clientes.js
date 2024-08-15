(() => {
    $.namespace('SubContratistas.Clientes');
    Clientes = function () {
        //#region Selectores
        const botonAgregar = $('#botonAgregar');
        const tablaClientes = $('#tablaClientes');
        const modalClientes = $('#modalClientes');
        const botonGuardar = $('#botonGuardar');
        const inputNombre = $('#inputNombre');
        //#endregion

        let dtClientes;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            $('.select2').select2();
            initTablaClientes();
            agregarListeners();
            cargarClientes();
        })();

        function initTablaClientes() {
            dtClientes = tablaClientes.DataTable({
                retrieve: true,
                paging: true,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaClientes.on('click', '.btn-editar', function () {
                        let rowData = dtClientes.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        modalClientes.modal('show');
                    });

                    tablaClientes.on('click', '.btn-eliminar', function () {
                        let rowData = dtClientes.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el cliente?`,
                            () => eliminarCliente(rowData.id))
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

        function cargarClientes() {
            axios.post('GetClientes')
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaClientes, response.data.data);
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
                modalClientes.modal('show');
            });
            botonGuardar.click(guardarCliente);
        }

        function limpiarModal() {
            inputNombre.val('');
        }

        function llenarCamposModal(data) {
            inputNombre.val(data.nombre);
        }

        function guardarCliente() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevoCliente();
                    break;
                case ESTATUS.EDITAR:
                    editarCliente();
                    break;
            }
        }

        function nuevoCliente() {
            let cliente = getInformacionCliente();

            axios.post('GuardarNuevoCliente', { cliente })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalClientes.modal('hide');
                        Alert2Exito('Se ha guardado la información.');
                        cargarClientes();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarCliente() {
            let cliente = getInformacionCliente();

            axios.post('EditarCliente', { cliente })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalClientes.modal('hide')
                        Alert2Exito('Se ha guardado la información');
                        cargarClientes();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarCliente(id) {
            let cliente = { id };

            axios.post('EliminarCliente', { cliente })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado la información.');
                        cargarClientes();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function getInformacionCliente() {
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
    $(document).ready(() => SubContratistas.SubContratistas = new Clientes())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();