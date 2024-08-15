(() => {
    $.namespace('Administrativo.Evaluacion.Puestos');
    Puestos = function () {
        //#region Selectores
        const tablaPuestos = $('#tablaPuestos');
        const botonAgregar = $('#botonAgregar');
        const modalPuesto = $('#modalPuesto');
        const inputDescripcion = $('#inputDescripcion');
        const selectCategoria = $('#selectCategoria');
        const tablaActividades = $('#tablaActividades');
        const botonGuardar = $('#botonGuardar');
        //#endregion

        let dtPuestos;
        let dtActividades;
        const ESTATUS = {
            NUEVO: 0,
            EDITAR: 1
        };

        (function init() {
            initTablaPuestos();
            initTablaActividades();
            agregarListeners();
            cargarPuestos();

            selectCategoria.fillCombo('/Administrativo/Evaluacion/GetCategoriasCombo', null, false);
        })();

        modalPuesto.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function initTablaPuestos() {
            dtPuestos = tablaPuestos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaPuestos.on('click', '.btn-editar', function () {
                        let rowData = dtPuestos.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        cargarActividades(true, rowData.id);
                        modalPuesto.modal('show');
                    });

                    tablaPuestos.on('click', '.btn-eliminar', function () {
                        let rowData = dtPuestos.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el puesto "${rowData.descripcion}"?`,
                            () => eliminarPuesto(rowData.id))
                    });
                },
                columns: [
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'categoriaDesc', title: 'Categoría' },
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

        function initTablaActividades() {
            dtActividades = tablaActividades.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                order: [],
                initComplete: function (settings, json) {
                    tablaActividades.on('click', 'input[type="checkbox"]', function () {
                        let row = $(this).closest('tr');
                        let checked = $(this).prop('checked');

                        row.find('.selectPeriodicidad').attr('disabled', !checked);

                        if (!checked) {
                            row.find('.selectPeriodicidad').val('');
                        }
                    });
                },
                createdRow: function (row, rowData) {
                    $(row).find('.checkBoxAplica').prop('checked', rowData.aplica);

                    let selectPeriodicidad = $(row).find('.selectPeriodicidad');

                    selectPeriodicidad.fillCombo('/Administrativo/Evaluacion/GetPeriodicidadCombo', null, false, '');
                    selectPeriodicidad.find('option[value=' + rowData.periodicidad + ']').attr('selected', true);
                    selectPeriodicidad.attr('disabled', !rowData.aplica);
                },
                columns: [
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'ponderacion', title: 'Ponderación' },
                    {
                        title: 'Aplica', render: function (data, type, row, meta) {
                            let div = document.createElement('div');
                            let checkbox = document.createElement('input');

                            checkbox.id = 'checkboxAplica_' + meta.row;
                            checkbox.setAttribute('type', 'checkbox');
                            checkbox.classList.add('form-control');
                            checkbox.classList.add('regular-checkbox');
                            checkbox.classList.add('checkBoxAplica');
                            checkbox.style.height = '25px';

                            let label = document.createElement('label');
                            label.setAttribute('for', checkbox.id);

                            $(div).append(checkbox);
                            $(div).append(label);

                            return div.outerHTML;
                        }
                    },
                    {
                        title: 'Periodicidad', render: function (data, type, row, meta) {
                            return `<select class="form-control selectPeriodicidad" disabled></select>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarPuestos() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/GetPuestos')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaPuestos, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cargarActividades(editar, puestoID) {
            if (editar) {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Administrativo/Evaluacion/GetActividadesPuesto', { puestoID })
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
            } else {
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
        }

        function agregarListeners() {
            botonAgregar.click(() => {
                limpiarModal();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                cargarActividades(false, 0);
                modalPuesto.modal('show');
            });
            botonGuardar.click(guardarPuesto);
        }

        function limpiarModal() {
            inputDescripcion.val('');
            selectCategoria.val('');

            dtActividades.clear().draw();
        }

        function llenarCamposModal(data) {
            inputDescripcion.val(data.descripcion);
            selectCategoria.val(data.categoria);
        }

        function guardarPuesto() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevoPuesto();
                    break;
                case ESTATUS.EDITAR:
                    editarPuesto();
                    break;
            }
        }

        function nuevoPuesto() {
            let puesto = getInformacionPuesto();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/GuardarNuevoPuesto', { puesto, actividades: puesto.actividades })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalPuesto.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarPuestos();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function editarPuesto() {
            let puesto = getInformacionPuesto();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/EditarPuesto', { puesto, actividades: puesto.actividades })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalPuesto.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarPuestos();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function eliminarPuesto(id) {
            let puesto = { id };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/EliminarPuesto', { puesto })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarPuestos();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getInformacionPuesto() {
            let actividades = getActividadesAplica();

            return {
                id: botonGuardar.data().id,
                descripcion: inputDescripcion.val(),
                categoria: selectCategoria.val(),
                estatus: true,
                actividades: actividades
            };
        }

        function getActividadesAplica() {
            let actividades = [];

            tablaActividades.find('tbody tr').each(function (index, row) {
                let checkbox = $(row).find('.checkBoxAplica');

                if (checkbox.prop('checked')) {
                    let rowData = dtActividades.row(row).data();
                    let periodicidad = +($(row).find('.selectPeriodicidad').val());

                    actividades.push({
                        id: rowData.id,
                        descripcion: rowData.descripcion,
                        ponderacion: rowData.ponderacion,
                        estatus: rowData.estatus,
                        aplica: true,
                        periodicidad: periodicidad
                    });
                }
            });

            return actividades;
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Evaluacion.Puestos = new Puestos())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();