(() => {
    $.namespace('Administrativo.Evaluacion.Actividades');
    Actividades = function () {
        //#region Selectores
        const tablaActividades = $('#tablaActividades');
        const botonAgregar = $('#botonAgregar');
        const modalActividad = $('#modalActividad');
        const inputDescripcion = $('#inputDescripcion');
        const inputPonderacion = $('#inputPonderacion');
        const botonGuardar = $('#botonGuardar');
        //#endregion

        let dtActividades;
        const ESTATUS = {
            NUEVO: 0,
            EDITAR: 1
        };

        (function init() {
            initTablaActividades();
            agregarListeners();
            cargarActividades();
        })();

        function initTablaActividades() {
            dtActividades = tablaActividades.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaActividades.on('click', '.btn-editar', function () {
                        let rowData = dtActividades.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        modalActividad.modal('show');
                    });

                    tablaActividades.on('click', '.btn-eliminar', function () {
                        let rowData = dtActividades.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar la actividad "${rowData.descripcion}"?`,
                            () => eliminarActividad(rowData.id))
                    });
                },
                columns: [
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'ponderacion', title: 'Ponderación' },
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

        function cargarActividades() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/GetActividades')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaActividades, response.data);
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
                modalActividad.modal('show');
            });
            botonGuardar.click(guardarActividad);
        }

        function limpiarModal() {
            inputDescripcion.val('');
            inputPonderacion.val('');
        }

        function llenarCamposModal(data) {
            inputDescripcion.val(data.descripcion);
            inputPonderacion.val(data.ponderacion);
        }

        function guardarActividad() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevaActividad();
                    break;
                case ESTATUS.EDITAR:
                    editarActividad();
                    break;
            }
        }

        function nuevaActividad() {
            let actividad = getInformacionActividad();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/GuardarNuevaActividad', { actividad })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalActividad.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarActividades();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function editarActividad() {
            let actividad = getInformacionActividad();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/EditarActividad', { actividad })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalActividad.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarActividades();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function eliminarActividad(id) {
            let actividad = { id };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/EliminarActividad', { actividad })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarActividades();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getInformacionActividad() {
            return {
                id: botonGuardar.data().id,
                descripcion: inputDescripcion.val(),
                ponderacion: inputPonderacion.val(),
                estatus: true
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Evaluacion.Actividades = new Actividades())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();