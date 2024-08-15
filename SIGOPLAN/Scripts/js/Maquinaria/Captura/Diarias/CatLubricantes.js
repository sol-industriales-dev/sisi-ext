(() => {
    $.namespace('AceitesLubricantes.CatLubricantes');
    CatLubricantes = function () {
        //#region Selectores
        const botonAgregar = $('#botonAgregar');
        const tablaLubricantes = $('#tablaLubricantes');
        const modalLubricante = $('#modalLubricante');
        const botonGuardar = $('#botonGuardar');
        const inputDescripcion = $('#inputDescripcion');
        const selectModelo = $('#selectModelo');
        const selectSubconjunto = $('#selectSubconjunto');
        //#endregion

        let dtLubricantes;
        let _subConjuntoID_Anterior = 0;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            $('.select2').select2();
            initTablaLubricantes();
            agregarListeners();
            cargarLubricantes();

            selectModelo.fillCombo('/AceitesLubricantes/GetComboModelos', null, false, null);
            selectSubconjunto.fillCombo('/AceitesLubricantes/GetComboSubconjuntos', null, false, null);
        })();

        function initTablaLubricantes() {
            dtLubricantes = tablaLubricantes.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '50vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaLubricantes.on('click', '.btn-editar', function () {
                        let rowData = dtLubricantes.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        _subConjuntoID_Anterior = rowData.subConjuntoID;
                        modalLubricante.modal('show');
                    });

                    tablaLubricantes.on('click', '.btn-eliminar', function () {
                        let rowData = dtLubricantes.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el registro?`,
                            () => eliminarLubricante({ id: rowData.id, subConjuntoID: rowData.subConjuntoID }))
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'Descripcion', title: 'Descripción' },
                    { data: 'modeloDesc', title: 'Modelo' },
                    { data: 'subconjuntoDesc', title: 'Subconjunto' },
                    {
                        render: function (data, type, row, meta) {
                            return `
                                <button title="Editar" class="btn-editar btn btn-xs btn-warning"><i class="fas fa-pencil-alt"></i></button>
                                <button title="Eliminar" class="btn-eliminar btn btn-xs btn-danger"><i class="fas fa-trash"></i></button>
                            `;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarLubricantes() {
            axios.post('/AceitesLubricantes/CargarCatalogoLubricantes').then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    AddRows(tablaLubricantes, response.data.data);
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
                modalLubricante.modal('show');
            });
            botonGuardar.click(guardarLubricante);
        }

        function limpiarModal() {
            inputDescripcion.val('');
            selectModelo.val('');
            selectModelo.select2().change();
            selectSubconjunto.val('');
            selectSubconjunto.select2().change();
        }

        function llenarCamposModal(data) {
            inputDescripcion.val(data.Descripcion);
            selectModelo.val(data.modeloID);
            selectModelo.select2().change();
            selectSubconjunto.val(data.subConjuntoID);
            selectSubconjunto.select2().change();
        }

        function guardarLubricante() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevoLubricante();
                    break;
                case ESTATUS.EDITAR:
                    editarLubricante();
                    break;
            }
        }

        function nuevoLubricante() {
            let lubricante = getInformacionLubricante();

            axios.post('/AceitesLubricantes/GuardarNuevoLubricante', { lubricante }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    modalLubricante.modal('hide');
                    AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                    cargarLubricantes();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarLubricante() {
            let lubricante = getInformacionLubricante();
            let subConjuntoID_Anterior = _subConjuntoID_Anterior;

            axios.post('/AceitesLubricantes/EditarLubricante', { lubricante, subConjuntoID_Anterior }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    modalLubricante.modal('hide');
                    AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                    cargarLubricantes();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarLubricante(lubricante) {
            axios.post('/AceitesLubricantes/EliminarLubricante', { lubricante }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                    cargarLubricantes();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function getInformacionLubricante() {
            return {
                id: botonGuardar.data().id,
                Descripcion: inputDescripcion.val().trim(),
                modeloID: +selectModelo.val(),
                subConjuntoID: +selectSubconjunto.val(),
                registroActivo: true
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => AceitesLubricantes.CatLubricantes = new CatLubricantes())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();