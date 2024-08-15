(() => {
    $.namespace('Almacen.Ubicaciones');

    Ubicaciones = function () {
        //#region Selectores
        const tablaUbicaciones = $('#tablaUbicaciones');
        const botonAgregar = $('#botonAgregar');
        const modalUbicacion = $('#modalUbicacion');
        const selectAlmacen = $('#selectAlmacen');
        const inputArea = $('#inputArea');
        const inputLado = $('#inputLado');
        const inputEstante = $('#inputEstante');
        const inputNivel = $('#inputNivel');
        const botonGuardar = $('#botonGuardar');
        //#endregion

        let dtUbicaciones;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            initTablaUbicaciones();
            agregarListeners();
            cargarUbicaciones();

            selectAlmacen.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, null);
        })();

        $('#inputArea, #inputLado, #inputEstante, #inputNivel').on('keyup', function () {
            $(this).val($(this).val().toUpperCase());
        });

        $('#inputArea, #inputLado, #inputEstante, #inputNivel').on('focus', function () {
            $(this).select();
        });

        function initTablaUbicaciones() {
            dtUbicaciones = tablaUbicaciones.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '50vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaUbicaciones.on('click', '.btn-editar', function () {
                        let rowData = dtUbicaciones.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        modalUbicacion.modal('show');
                    });

                    tablaUbicaciones.on('click', '.btn-eliminar', function () {
                        let rowData = dtUbicaciones.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el registro?`,
                            () => eliminarUbicacion(rowData.id))
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    {
                        title: 'Almacén', render: function (data, type, row, meta) {
                            return `[${row.almacen >= 10 ? row.almacen : '0' + row.almacen}] ${row.almacenDesc}`;
                        }
                    },
                    { data: 'area_alm', title: 'Área' },
                    { data: 'lado_alm', title: 'Lado' },
                    { data: 'estante_alm', title: 'Estante' },
                    { data: 'nivel_alm', title: 'Nivel' },
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

        function cargarUbicaciones() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Almacen/GetUbicaciones')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaUbicaciones, response.data);
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
                modalUbicacion.modal('show');
            });
            botonGuardar.click(guardarUbicacion);
        }

        function limpiarModal() {
            selectAlmacen.val('');
            inputArea.val('');
            inputLado.val('');
            inputEstante.val('');
            inputNivel.val('');
        }

        function llenarCamposModal(data) {
            selectAlmacen.val(data.almacen >= 10 ? data.almacen : '0' + data.almacen);
            inputArea.val(data.area_alm);
            inputLado.val(data.lado_alm);
            inputEstante.val(data.estante_alm);
            inputNivel.val(data.nivel_alm);
        }

        function guardarUbicacion() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevaUbicacion();
                    break;
                case ESTATUS.EDITAR:
                    editarUbicacion();
                    break;
            }
        }

        function nuevaUbicacion() {
            let ubicacion = getInformacionUbicacion();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Almacen/GuardarNuevaUbicacion', { ubicacion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalUbicacion.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarUbicaciones();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function editarUbicacion() {
            let ubicacion = getInformacionUbicacion();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Almacen/EditarUbicacion', { ubicacion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalUbicacion.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarUbicaciones();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function eliminarUbicacion(id) {
            let ubicacion = { id };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Almacen/EliminarUbicacion', { ubicacion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha eliminado la información.`);
                        cargarUbicaciones();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getInformacionUbicacion() {
            return {
                id: botonGuardar.data().id,
                almacen: +selectAlmacen.val(),
                area_alm: inputArea.val(),
                lado_alm: inputLado.val(),
                estante_alm: inputEstante.val(),
                nivel_alm: inputNivel.val(),
                registroActivo: true
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }

    $(document).ready(() => Almacen.Ubicaciones = new Ubicaciones()).ajaxStart(() => $.blockUI({ message: 'Procesando...' })).ajaxStop($.unblockUI);
})();