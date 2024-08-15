(() => {
    $.namespace('CH.Agrupaciones');
    Agrupaciones = function () {
        //#region Selectores
        const tablaAgrupaciones = $('#tablaAgrupaciones');
        const botonAgregar = $('#botonAgregar');
        const modalAgrupacion = $('#modalAgrupacion');
        const inputNombre = $('#inputNombre');
        const botonGuardar = $('#botonGuardar');
        const cboFiltroAnio = $('#cboFiltroAnio');
        const cboFiltroCC = $('#cboFiltroCC');
        const cboAnio = $('#cboAnio');
        const cboCC = $('#cboCC');
        const botonBuscar = $('#botonBuscar');
        //#endregion

        let dtAgrupaciones;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            initTablaAgrupaciones();
            agregarListeners();
        })();

        modalAgrupacion.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        botonBuscar.on("click", function(){
            cargarAgrupaciones();
        });

        cboFiltroAnio.fillCombo("FillAnios", {}, false);
        cboFiltroAnio.select2();
        cboFiltroAnio.select2({ width: "100%" });
        cboFiltroCC.select2();
        cboFiltroCC.select2({ width: "100%" });
        cboFiltroAnio.on("change", function () {
            if ($(this).val() > 0) {
                cboFiltroCC.fillCombo('FillUsuarioRelCC', { anio: $(this).val() }, false);
            }
        });
        cboAnio.fillCombo("FillAnios", {}, false);
        cboAnio.select2();
        cboAnio.select2({ width: "100%" });
        cboCC.fillCombo('FillCCAutorizantes', null, false, null);
        cboCC.select2();
        cboCC.select2({ width: "100%" });

        function initTablaAgrupaciones() {
            dtAgrupaciones = tablaAgrupaciones.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaAgrupaciones.on('click', '.btn-editar', function () {
                        let rowData = dtAgrupaciones.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        
                        modalAgrupacion.modal('show');
                    });

                    tablaAgrupaciones.on('click', '.btn-eliminar', function () {
                        let rowData = dtAgrupaciones.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar la agrupacion "${rowData.nombre}"?`,
                            () => eliminarAgrupacion(rowData.id))
                    });
                },
                drawCallback: function () {
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                },
                columns: [
                    { data: 'nombre', title: 'AGRUPACIÓN' },
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
                    },
                    { data: 'anio', title: 'anio', visible: false},
                    { data: 'idCC', title: 'cc', visible: false},
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarAgrupaciones() {
            let obj = {
                anio: cboFiltroAnio.val(),
                idCC: cboFiltroCC.val()
            }
            
            axios.post('GetAgrupaciones',obj)
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaAgrupaciones, response.data.data);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
        }

        function agregarListeners() {
            botonAgregar.click(() => {
                if (cboFiltroCC.val() == "" || cboFiltroAnio.val() == "") {
                    let strMensajeError = "";
                    cboFiltroCC.val() <= 0 ? strMensajeError += "Es necesario indicar un CC." : "";
                    cboFiltroAnio.val() <= 0 ? strMensajeError += "<br>Es necesario indicar un año." : "";
                    Alert2Warning(strMensajeError);
                } else {
                    limpiarModal();
                    cboCC.val(cboFiltroCC.val());
                    cboCC.trigger("change");
                    cboAnio.val(cboFiltroAnio.val());
                    cboAnio.trigger("change");
                    botonGuardar.data().estatus = ESTATUS.NUEVO;
                    botonGuardar.data().id = 0;
                    modalAgrupacion.modal('show');
                }
                

            });
            botonGuardar.click(guardarAgrupacion);
        }

        function limpiarModal() {
            inputNombre.val('');
            cboAnio.val(0);
            cboAnio.trigger("change");
            cboCC.val(0);
            cboCC.trigger("change");
        }

        function llenarCamposModal(data) {
            inputNombre.val(data.nombre);
            cboAnio.val(data.anio);
            cboAnio.trigger("change");
            cboCC.val(data.idCC);
            cboCC.trigger("change");
        }

        function guardarAgrupacion() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevaAgrupacion();
                    break;
                case ESTATUS.EDITAR:
                    editarAgrupacion();
                    break;
            }
        }

        function nuevaAgrupacion() {
            let agrupacion = getInformacionAgrupacion();

            axios.post('GuardarNuevaAgrupacion', agrupacion)
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalAgrupacion.modal('hide');
                        Alert2Exito("Se ha registrado con éxito.");
                        cargarAgrupaciones();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
        }

        function editarAgrupacion() {
            let agrupacion = getInformacionAgrupacion();

            axios.post('EditarAgrupacion', { agrupacion })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalAgrupacion.modal('hide');
                        Alert2Exito("Se ha registrado con éxito.");
                        cargarAgrupaciones();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
        }

        function eliminarAgrupacion(id) {
            let agrupacion = { id };

            axios.post('EliminarAgrupacion', { agrupacion })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito("Se ha registrado con éxito.");
                        cargarAgrupaciones();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
        }

        function getInformacionAgrupacion() {
            return {
                id: botonGuardar.data().id,
                nombre: inputNombre.val(),
                estatus: true,
                anio: cboAnio.val(),
                idCC: cboCC.val(),
                
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => CH.Agrupaciones = new Agrupaciones())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();