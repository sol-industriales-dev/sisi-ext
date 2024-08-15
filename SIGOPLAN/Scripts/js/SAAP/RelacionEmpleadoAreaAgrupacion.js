(() => {
    $.namespace('SAAP.RelacionEmpleadoAreaAgrupacion');

    RelacionEmpleadoAreaAgrupacion = function () {
        //#region Selectores
        const tablaRelacion = $('#tablaRelacion');
        const botonAgregar = $('#botonAgregar');
        const modalRelacion = $('#modalRelacion');
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const inputNombre = $('#inputNombre');
        const inputApellidoPaterno = $('#inputApellidoPaterno');
        const inputApellidoMaterno = $('#inputApellidoMaterno');
        const labelArea = $('#labelArea');
        const selectArea = $('#selectArea');
        const selectCentroCosto = $('#selectCentroCosto');
        const selectTipoUsuario = $('#selectTipoUsuario');
        const botonGuardar = $('#botonGuardar');
        //#endregion

        let dtRelacion;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            $('.select2').select2();
            initTablaRelacion();
            agregarListeners();
            cargarRelaciones();

            selectArea.fillCombo('/SAAP/SAAP/GetAreaCombo', null, false, null);
            selectCentroCosto.fillCombo('/SAAP/SAAP/GetAgrupacionCombo', null, false, null);
        })();

        selectTipoUsuario.on('change', function () {
            if (selectTipoUsuario.val() == 2) {
                labelArea.text('Área Evaluadora:');
            } else {
                labelArea.text('Área:');
            }
        });

        function initTablaRelacion() {
            dtRelacion = tablaRelacion.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '50vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaRelacion.on('click', '.btn-editar', function () {
                        let rowData = dtRelacion.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;

                        modalRelacion.modal('show');
                    });

                    tablaRelacion.on('click', '.btn-eliminar', function () {
                        let rowData = dtRelacion.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el registro?`,
                            () => eliminarRelacion(rowData.id))
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'nombreEmpleado', title: 'Empleado' },
                    { data: 'areaDesc', title: 'Área' },
                    { data: 'agrupacionDesc', title: 'Agrupación' },
                    { data: 'tipoUsuarioDesc', title: 'Tipo Usuario' },
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
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarRelaciones() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/SAAP/SAAP/GetRelacionesEmpleadoAreaAgrupacion')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaRelacion, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function agregarListeners() {
            inputClaveEmpleado.click(function (e) {
                $(this).select();
            });
            botonAgregar.click(() => {
                limpiarModal();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                modalRelacion.modal('show');
            });
            botonGuardar.click(guardarRelacion);
            inputClaveEmpleado.change(cargarEmpleadoPorClave);
        }

        function limpiarModal() {
            inputClaveEmpleado.val('');
            inputNombre.val('');
            inputApellidoPaterno.val('');
            inputApellidoMaterno.val('');
            selectArea.val('');
            selectCentroCosto.val('');
            selectCentroCosto.select2().trigger('change');
            selectTipoUsuario.val('');
            selectTipoUsuario.change();
        }

        function llenarCamposModal(data) {
            inputClaveEmpleado.val(data.claveEmpleado);
            inputClaveEmpleado.change();
            inputNombre.val(data.nombre);
            inputApellidoPaterno.val(data.apellidoPaterno);
            inputApellidoMaterno.val(data.apellidoMaterno);
            selectArea.val(data.area);
            selectCentroCosto.val(data.agrupacion_id).trigger('change.select2');
            selectTipoUsuario.val(data.tipoUsuario);
            selectTipoUsuario.change();
        }

        function guardarRelacion() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevaRelacion();
                    break;
                case ESTATUS.EDITAR:
                    editarRelacion();
                    break;
            }
        }

        function nuevaRelacion() {
            let relacion = getInformacionRelacion();

            if (relacion.tipoUsuario == '' || relacion.tipoUsuario == null) {
                Alert2Warning('Debe seleccionar el tipo de usuario.');
                return;
            }

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/SAAP/SAAP/GuardarNuevaRelacion', { relacion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalRelacion.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarRelaciones();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function editarRelacion() {
            let relacion = getInformacionRelacion();

            if (relacion.tipoUsuario == '' || relacion.tipoUsuario == null) {
                Alert2Warning('Debe seleccionar el tipo de usuario.');
                return;
            }

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/SAAP/SAAP/EditarRelacion', { relacion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalRelacion.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarRelaciones();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function eliminarRelacion(id) {
            let relacion = { id };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/SAAP/SAAP/EliminarRelacion', { relacion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarRelaciones();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getInformacionRelacion() {
            return {
                id: botonGuardar.data().id,
                usuario_id: +inputClaveEmpleado.attr('usuario-id'),
                area: +(selectArea.val()),
                empresa_id: 0,
                agrupacion_id: selectCentroCosto.getAgrupador(),
                tipoUsuario: selectTipoUsuario.val(),
                estatus: true
            };
        }

        function cargarEmpleadoPorClave() {
            let claveEmpleado = +(inputClaveEmpleado.val());

            if (claveEmpleado > 0) {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/SAAP/SAAP/GetUsuarioPorClave', { claveEmpleado })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            inputClaveEmpleado.attr('usuario-id', response.data.usuario_id);
                            inputNombre.val(response.data.nombre);
                            inputApellidoPaterno.val(response.data.apellidoPaterno);
                            inputApellidoMaterno.val(response.data.apellidoMaterno);
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    });
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }

    $(document).ready(() => SAAP.RelacionEmpleadoAreaAgrupacion = new RelacionEmpleadoAreaAgrupacion())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();