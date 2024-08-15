(() => {
    $.namespace('Administrativo.Capacitacion.RelacionCCAutorizante');
    RelacionCCAutorizante = function () {
        //#region Selectores
        const tablaRelacion = $('#tablaRelacion');
        const botonAgregar = $('#botonAgregar');
        const modalRelacion = $('#modalRelacion');
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const inputNombre = $('#inputNombre');
        const inputApellidoPaterno = $('#inputApellidoPaterno');
        const inputApellidoMaterno = $('#inputApellidoMaterno');
        const selectTipoPuesto = $('#selectTipoPuesto');
        // const inputOrden = $('#inputOrden');
        const selectCentroCosto = $('#selectCentroCosto');
        const botonGuardar = $('#botonGuardar');
        //#endregion

        let dtRelacion;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            $('.select2').select2();
            initTablaRelacion();
            agregarListeners();
            cargarRelaciones();

            selectTipoPuesto.fillCombo('/Administrativo/Capacitacion/GetTipoPuestoCombo', null, false, null);
            selectCentroCosto.fillCombo('/Enkontrol/Requisicion/FillComboCcTodos', null, false, null);
        })();

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
                    { data: 'ccDesc', title: 'Centro de Costo' },
                    { data: 'nombreCompleto', title: 'Usuario' },
                    { data: 'tipoPuestoDesc', title: 'Tipo Puesto' },
                    // { data: 'orden', title: 'Orden' },
                    {
                        render: function (data, type, row, meta) {
                            return `
                            <button title="Editar" class="btn-editar btn btn-warning">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            &nbsp;
                            <button title="Eliminar" class="btn-eliminar btn btn-danger">
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
            $.post('/Administrativo/Capacitacion/GetRelacionesCCAutorizantes')
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
            botonAgregar.click(() => {
                limpiarModal();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                modalRelacion.modal('show');
            });
            botonGuardar.click(guardarRelacion);
            inputClaveEmpleado.change(cargarUsuarioPorClave);
        }

        function limpiarModal() {
            selectCentroCosto.val('');
            selectCentroCosto.select2().trigger('change');
            inputClaveEmpleado.val('');
            inputNombre.val('');
            inputApellidoPaterno.val('');
            inputApellidoMaterno.val('');
            selectTipoPuesto.val('');
            // inputOrden.val('');
        }

        function llenarCamposModal(data) {
            selectCentroCosto.val(data.cc);
            selectCentroCosto.select2().trigger('change');
            inputClaveEmpleado.val(data.clave_empleado);
            inputNombre.val(data.nombre);
            inputApellidoPaterno.val(data.apellidoPaterno);
            inputApellidoMaterno.val(data.apellidoMaterno);
            selectTipoPuesto.val(data.tipoPuesto);
            // inputOrden.val(data.orden);
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

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Capacitacion/GuardarNuevaRelacionCCAutorizante', { relacion })
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

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Capacitacion/EditarRelacionCCAutorizante', { relacion })
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
            $.post('/Administrativo/Capacitacion/EliminarRelacionCCAutorizante', { relacion })
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
                cc: selectCentroCosto.val(),
                clave_empleado: +(inputClaveEmpleado.val()),
                tipoPuesto: +(selectTipoPuesto.val()),
                orden: 0,
                estatus: true
            };
        }

        function cargarUsuarioPorClave() {
            let claveEmpleado = +(inputClaveEmpleado.val());

            if (claveEmpleado > 0) {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Administrativo/Capacitacion/GetUsuarioPorClave', { claveEmpleado })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            inputNombre.val(response.data.nombre);
                            inputApellidoPaterno.val(response.data.apellidoPaterno);
                            inputApellidoMaterno.val(response.data.apellidoMaterno);
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Capacitacion.RelacionCCAutorizante = new RelacionCCAutorizante())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();