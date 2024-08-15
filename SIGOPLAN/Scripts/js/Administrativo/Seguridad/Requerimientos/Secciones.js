(() => {
    $.namespace('Administrativo.Requerimientos.Secciones');
    Secciones = function () {
        //#region Selectores
        const tablaSecciones = $('#tablaSecciones');
        const botonAgregar = $('#botonAgregar');
        const modalSeccion = $('#modalSeccion');
        const inputDescripcion = $('#inputDescripcion');
        const botonGuardar = $('#botonGuardar');
        //#endregion

        let dtSecciones;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            initTablaSecciones();
            agregarListeners();
            cargarSecciones();
        })();

        modalSeccion.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function initTablaSecciones() {
            dtSecciones = tablaSecciones.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaSecciones.on('click', '.btn-editar', function () {
                        let rowData = dtSecciones.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        modalSeccion.modal('show');
                    });

                    tablaSecciones.on('click', '.btn-eliminar', function () {
                        let rowData = dtSecciones.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar la sección "${rowData.descripcion}"?`,
                            () => eliminarSeccion(rowData.id))
                    });
                },
                columns: [
                    { data: 'descripcion', title: 'Descripción' },
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

        function cargarSecciones() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GetSecciones')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaSecciones, response.data);
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
                modalSeccion.modal('show');
            });
            botonGuardar.click(guardarSeccion);
        }

        function limpiarModal() {
            inputDescripcion.val('');
        }

        function llenarCamposModal(data) {
            inputDescripcion.val(data.descripcion);
        }

        function guardarSeccion() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevaSeccion();
                    break;
                case ESTATUS.EDITAR:
                    editarSeccion();
                    break;
            }
        }

        function nuevaSeccion() {
            let seccion = getInformacionSeccion();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GuardarNuevaSeccion', { seccion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalSeccion.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarSecciones();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function editarSeccion() {
            let seccion = getInformacionSeccion();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/EditarSeccion', { seccion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalSeccion.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarSecciones();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function eliminarSeccion(id) {
            let seccion = { id };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/EliminarSeccion', { seccion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarSecciones();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getInformacionSeccion() {
            return {
                id: botonGuardar.data().id,
                descripcion: inputDescripcion.val(),
                estatus: true
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Requerimientos.Secciones = new Secciones())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();