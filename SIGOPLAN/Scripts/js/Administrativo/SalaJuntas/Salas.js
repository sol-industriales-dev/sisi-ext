(() => {
    $.namespace('Administrativo.SalaJuntas.Salas');
    Salas = function () {
        //#region Selectores
        const tablaSalas = $('#tablaSalas');
        const botonAgregar = $('#botonAgregar');
        const modalSala = $('#modalSala');
        const inputDescripcion = $('#inputDescripcion');
        const inputCapacidad = $('#inputCapacidad');
        const inputCorreo = $('#inputCorreo');
        //solo para ejemplo datetime


        const botonGuardar = $('#botonGuardar');
        //#endregion

        let dtSalas;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            initTablaSalas();
            agregarListeners();
            cargarSalas();
            //Generar datatime en el campo de fecha




        })();

        modalSala.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function initTablaSalas() {
            dtSalas = tablaSalas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaSalas.on('click', '.btn-editar', function () {
                        let rowData = dtSalas.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        modalSala.modal('show');
                    });

                    tablaSalas.on('click', '.btn-eliminar', function () {
                        let rowData = dtSalas.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar la sala "${rowData.descripcion}"?`,
                            () => eliminarSala(rowData.id))
                    });
                },
                columns: [

                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'capacidad', title: 'Capacidad' },
                    { data: 'correo', title: 'Correo' },
                    //ejemplo para ver formato de fecha en datatable

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



        function agregarListeners() {
            botonAgregar.click(() => {
                limpiarModal();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                modalSala.modal('show');
            });
            botonGuardar.click(guardarSala);
        }

        function limpiarModal() {
            inputDescripcion.val('');
            inputCapacidad.val('');
            inputCorreo.val('');
            //ejemplo


        }

        function llenarCamposModal(data) {
            inputDescripcion.val(data.descripcion);
            inputCapacidad.val(data.capacidad);
            inputCorreo.val(data.correo);
            //ejemplo datetime modal de campo fecha

        }

        function guardarSala() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevaSala();
                    break;
                case ESTATUS.EDITAR:
                    editarSala();
                    break;
            }
        }

        function cargarSalas() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/SalaJuntas/GetSalas')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaSalas, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }


        function nuevaSala() {
            let sala = getInformacionSala();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/SalaJuntas/GuardarNuevaSala', { sala })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalSala.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarSalas();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function editarSala() {
            let sala = getInformacionSala();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/SalaJuntas/EditarSala', { sala })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalSala.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarSalas();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function eliminarSala(id) {
            let sala = { id };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/SalaJuntas/EliminarSala', { sala })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarSalas();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getInformacionSala() {
            return {

                id: botonGuardar.data().id,
                descripcion: inputDescripcion.val(),
                capacidad: inputCapacidad.val(),
                correo: inputCorreo.val(),

                estatus: true,

            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.SalaJuntas.Salas = new Salas())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();