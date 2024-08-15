(() => {
    $.namespace('SubContratistas.SubContratistas');
    SubContratistas = function () {
        //#region Selectores
        const botonAgregar = $('#botonAgregar');
        const tablaSubcontratistas = $('#tablaSubcontratistas');
        const modalSubcontratista = $('#modalSubcontratista');
        const botonGuardar = $('#botonGuardar');
        const inputNumeroProveedor = $('#inputNumeroProveedor');
        const inputNombre = $('#inputNombre');
        const inputDireccion = $('#inputDireccion');
        const inputNombreCorto = $('#inputNombreCorto');
        const inputCodigoPostal = $('#inputCodigoPostal');
        const inputCorreo = $('#inputCorreo');
        const inputRFC = $('#inputRFC');
        const radioFisica = $('#radioFisica');
        const radioMoral = $('#radioMoral');
        //#endregion

        let dtSubcontratistas;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            $('.select2').select2();
            initTablaSubcontratistas();
            agregarListeners();
            cargarSubcontratistas();
        })();

        inputNumeroProveedor.on('change', function () {
            let numeroProveedor = +inputNumeroProveedor.val();

            if (numeroProveedor > 0) {
                axios.post('GetProveedor', { numeroProveedor })
                    .then(response => {
                        let { success, message } = response.data;

                        if (success) {
                            let datos = response.data.data[0];

                            llenarCamposModal({
                                numeroProveedor: numeroProveedor,
                                nombre: datos.nombre,
                                direccion: datos.direccion,
                                nombreCorto: datos.nombreCorto,
                                codigoPostal: datos.codigoPostal,
                                correo: datos.correo,
                                rfc: datos.rfc,
                                fisica: false
                            });
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                limpiarModal();
            }
        });

        function initTablaSubcontratistas() {
            dtSubcontratistas = tablaSubcontratistas.DataTable({
                retrieve: true,
                paging: true,
                language: dtDicEsp,
                bInfo: false,
                // scrollY: '50vh',
                // scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaSubcontratistas.on('click', '.btn-editar', function () {
                        let rowData = dtSubcontratistas.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        modalSubcontratista.modal('show');
                    });

                    tablaSubcontratistas.on('click', '.btn-eliminar', function () {
                        let rowData = dtSubcontratistas.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el subcontratista?`,
                            () => eliminarSubcontratista(rowData.id))
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'numeroProveedor', title: '# Proveedor' },
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
                    { width: '70%', targets: [1] }
                ]
            });
        }

        function cargarSubcontratistas() {
            axios.post('GetSubcontratistas')
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaSubcontratistas, response.data.data);
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
                modalSubcontratista.modal('show');
            });
            botonGuardar.click(guardarSubcontratista);
        }

        function limpiarModal() {
            inputNumeroProveedor.val('');
            inputNombre.val('');
            inputDireccion.val('');
            inputNombreCorto.val('');
            inputCodigoPostal.val('');
            inputCorreo.val('');
            inputRFC.val('');
            radioMoral.click();
        }

        function llenarCamposModal(data) {
            inputNumeroProveedor.val(data.numeroProveedor);
            inputNombre.val(data.nombre);
            inputDireccion.val(data.direccion);
            inputNombreCorto.val(data.nombreCorto);
            inputCodigoPostal.val(data.codigoPostal);
            inputCorreo.val(data.correo);
            inputRFC.val(data.rfc);

            if (data.fisica) {
                radioFisica.click();
            } else {
                radioMoral.click();
            }
        }

        function guardarSubcontratista() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevoSubcontratista();
                    break;
                case ESTATUS.EDITAR:
                    editarSubcontratista();
                    break;
            }
        }

        function nuevoSubcontratista() {
            let subcontratista = getInformacionSubcontratista();

            axios.post('GuardarNuevoSubcontratista', { subcontratista })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalSubcontratista.modal('hide');
                        Alert2Exito('Se ha guardado la información.');
                        cargarSubcontratistas();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarSubcontratista() {
            let subcontratista = getInformacionSubcontratista();

            axios.post('EditarSubcontratista', { subcontratista })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalSubcontratista.modal('hide')
                        Alert2Exito('Se ha guardado la información');
                        cargarSubcontratistas();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarSubcontratista(id) {
            let subcontratista = { id };

            axios.post('EliminarSubcontratista', { subcontratista })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado la información.');
                        cargarSubcontratistas();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function getInformacionSubcontratista() {
            return {
                id: botonGuardar.data().id,
                numeroProveedor: inputNumeroProveedor.val(),
                nombre: inputNombre.val(),
                direccion: inputDireccion.val(),
                nombreCorto: inputNombreCorto.val(),
                codigoPostal: inputCodigoPostal.val(),
                rfc: inputRFC.val(),
                correo: inputCorreo.val(),
                fisica: radioFisica.prop('checked'),
                estatus: true
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => SubContratistas.SubContratistas = new SubContratistas())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();