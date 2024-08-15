(() => {
    $.namespace('Administrativo.Requerimientos.Condicionantes');
    Condicionantes = function () {
        //#region Selectores
        const tablaCondicionantes = $('#tablaCondicionantes');
        const botonAgregar = $('#botonAgregar');
        const modalCondicionante = $('#modalCondicionante');
        const inputDescripcion = $('#inputDescripcion');
        const botonGuardar = $('#botonGuardar');
        //#endregion

        let dtCondicionantes;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            initTablaCondicionantes();
            agregarListeners();
            cargarCondicionantes();
        })();

        modalCondicionante.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function initTablaCondicionantes() {
            dtCondicionantes = tablaCondicionantes.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaCondicionantes.on('click', '.btn-editar', function () {
                        let rowData = dtCondicionantes.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        modalCondicionante.modal('show');
                    });

                    tablaCondicionantes.on('click', '.btn-eliminar', function () {
                        let rowData = dtCondicionantes.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar la condicionante "${rowData.descripcion}"?`,
                            () => eliminarCondicionante(rowData.id))
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

        function cargarCondicionantes() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GetCondicionantes')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaCondicionantes, response.data);
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
                modalCondicionante.modal('show');
            });
            botonGuardar.click(guardarCondicionante);
        }

        function limpiarModal() {
            inputDescripcion.val('');
        }

        function llenarCamposModal(data) {
            inputDescripcion.val(data.descripcion);
        }

        function guardarCondicionante() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevaCondicionante();
                    break;
                case ESTATUS.EDITAR:
                    editarCondicionante();
                    break;
            }
        }

        function nuevaCondicionante() {
            let condicionante = getInformacionCondicionante();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GuardarNuevaCondicionante', { condicionante })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalCondicionante.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarCondicionantes();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function editarCondicionante() {
            let condicionante = getInformacionCondicionante();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/EditarCondicionante', { condicionante })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalCondicionante.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarCondicionantes();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function eliminarCondicionante(id) {
            let condicionante = { id };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/EliminarCondicionante', { condicionante })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarCondicionantes();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getInformacionCondicionante() {
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
    $(document).ready(() => Administrativo.Requerimientos.Condicionantes = new Condicionantes())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();