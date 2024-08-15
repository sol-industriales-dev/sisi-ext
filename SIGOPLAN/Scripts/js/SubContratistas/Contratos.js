(() => {
    $.namespace('SubContratistas.Contratos');
    Contratos = function () {
        //#region Selectores
        const botonAgregar = $('#botonAgregar');
        const tablaContratos = $('#tablaContratos');
        const modalContratos = $('#modalContratos');
        const botonGuardar = $('#botonGuardar');
        const inputNombre = $('#inputNombre');
        //#endregion

        let dtContratos;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            $('.select2').select2();
            initTablaContratos();
            agregarListeners();
            cargarContratos();
        })();

        function initTablaContratos() {
            dtContratos = tablaContratos.DataTable({
                retrieve: true,
                paging: true,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaContratos.on('click', '.btn-editar', function () {
                        let rowData = dtContratos.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        modalContratos.modal('show');
                    });

                    tablaContratos.on('click', '.btn-eliminar', function () {
                        let rowData = dtContratos.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el contrato?`,
                            () => eliminarContrato(rowData.id))
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

        function cargarContratos() {
            axios.post('GetContratos')
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaContratos, response.data.data);
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
                modalContratos.modal('show');
            });
            botonGuardar.click(guardarContrato);
        }

        function limpiarModal() {
            inputNombre.val('');
        }

        function llenarCamposModal(data) {
            inputNombre.val(data.nombre);
        }

        function guardarContrato() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevoContrato();
                    break;
                case ESTATUS.EDITAR:
                    editarContrato();
                    break;
            }
        }

        function nuevoContrato() {
            let contrato = getInformacionContrato();

            axios.post('GuardarNuevoContrato', { contrato })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalContratos.modal('hide');
                        Alert2Exito('Se ha guardado la información.');
                        cargarContratos();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarContrato() {
            let contrato = getInformacionContrato();

            axios.post('EditarContrato', { contrato })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalContratos.modal('hide')
                        Alert2Exito('Se ha guardado la información');
                        cargarContratos();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarContrato(id) {
            let contrato = { id };

            axios.post('EliminarContrato', { contrato })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado la información.');
                        cargarContratos();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function getInformacionContrato() {
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
    $(document).ready(() => SubContratistas.SubContratistas = new Contratos())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();