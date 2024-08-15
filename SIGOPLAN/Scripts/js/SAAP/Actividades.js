(() => {
    $.namespace('SAAP.Actividades');
    Actividades = function () {
        //#region Selectores
        const tablaActividades = $('#tablaActividades');
        const botonAgregar = $('#botonAgregar');
        const modalActividad = $('#modalActividad');
        const inputNombre = $('#inputNombre');
        const inputDescripcion = $('#inputDescripcion');
        const selectArea = $('#selectArea');
        const selectAreaEvaluadora = $('#selectAreaEvaluadora');
        const selectClasificacion = $('#selectClasificacion');
        const inputPorcentaje = $('#inputPorcentaje');
        const inputDiasCompromiso = $('#inputDiasCompromiso');
        const botonGuardar = $('#botonGuardar');
        //#endregion

        let dtActividades;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            initTablaActividades();
            agregarListeners();
            cargarActividades();

            selectArea.fillCombo('/SAAP/SAAP/GetAreaCombo', null, false, null);
            selectAreaEvaluadora.fillCombo('/SAAP/SAAP/GetAreaCombo', null, false, null);
            selectClasificacion.fillCombo('/SAAP/SAAP/GetClasificacionCombo', null, false, null);
        })();

        modalActividad.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

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
                drawCallback: function () {
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                },
                columns: [
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'areaDesc', title: 'Área' },
                    { data: 'areaEvaluadoraDesc', title: 'Área Evaluadora' },
                    { data: 'clasificacionDesc', title: 'Clasificación' },
                    {
                        data: 'porcentaje', title: 'Porcentaje', render: function (data, type, row, meta) {
                            return data + '%';
                        }
                    },
                    { data: 'diasCompromiso', title: 'Días Compromiso' },
                    {
                        render: function (data, type, row, meta) {
                            return `
                            <button title="Editar" class="btn-editar btn btn-xs btn-warning">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            <button title="Eliminar" class="btn-eliminar btn btn-xs btn-danger">
                                <i class="fas fa-trash"></i>
                            </button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '10%', targets: 7 }
                ]
            });
        }

        function cargarActividades() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/SAAP/SAAP/GetActividades')
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
            inputNombre.val('');
            inputDescripcion.val('');
            selectArea.val('');
            selectAreaEvaluadora.val('');
            selectClasificacion.val('');
            inputPorcentaje.val('');
            inputDiasCompromiso.val('');
        }

        function llenarCamposModal(data) {
            inputNombre.val(data.nombre);
            inputDescripcion.val(data.descripcion);
            selectArea.val(data.area);
            selectAreaEvaluadora.val(data.areaEvaluadora != 0 ? data.areaEvaluadora : '');
            selectClasificacion.val(data.clasificacion);
            inputPorcentaje.val(data.porcentaje);
            inputDiasCompromiso.val(data.diasCompromiso);
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

            if (+actividad.porcentaje < 1 || +actividad.porcentaje > 5) {
                Alert2Warning('Debe ingresar un valor válido para el porcentaje (de 1 a 5).');
                return;
            }

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/SAAP/SAAP/GuardarNuevaActividad', { actividad })
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

            if (+actividad.porcentaje < 1 || +actividad.porcentaje > 5) {
                Alert2Warning('Debe ingresar un valor válido para el porcentaje (de 1 a 5).');
                return;
            }

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/SAAP/SAAP/EditarActividad', { actividad })
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
            $.post('/SAAP/SAAP/EliminarActividad', { actividad })
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
                nombre: inputNombre.val(),
                descripcion: inputDescripcion.val(),
                area: selectArea.val(),
                areaEvaluadora: selectAreaEvaluadora.val(),
                clasificacion: selectClasificacion.val(),
                porcentaje: inputPorcentaje.val(),
                diasCompromiso: inputDiasCompromiso.val(),
                estatus: true
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => SAAP.Actividades = new Actividades())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();