(() => {
    $.namespace('Reclutamientos.ExpedienteDigital');
    ExpedienteDigital = function () {
        //#region Selectores
        const tablaExpedientes = $('#tablaExpedientes');
        const botonAgregar = $('#botonAgregar');
        const modalCaptura = $('#modalCaptura');
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const inputNombreEmpleado = $('#inputNombreEmpleado');
        const selectArchivoAplicable = $('#selectArchivoAplicable');
        const botonGuardar = $('#botonGuardar');
        const cboFiltroEstatus = $('#cboFiltroEstatus');
        const botonBuscar = $('#botonBuscar');
        const botonDescargar = $('#botonDescargar');
        const txtBotonGuardar = $('#txtBotonGuardar');
        const selectCC = $('#selectCC');
        const modalHistorial = $('#modalHistorial');
        const tablaHistorial = $('#tablaHistorial');
        const chkFiltroEsPendiente = $('#chkFiltroEsPendiente');
        //#endregion

        let dtExpedientes;
        let dtHistorial;

        (function init() {
            initTablaHistorial();

            cargarExpedientes([0, 1, 2]);

            botonAgregar.click(() => {
                limpiarModal();
                modalCaptura.modal('show');
                txtBotonGuardar.text("Guardar");
            });
            inputClaveEmpleado.change(cargarInformacionEmpleado);
            // botonGuardar.click(guardarExpediente);

            botonGuardar.on("click", function () {
                if (txtBotonGuardar.text() == "Guardar") {
                    guardarExpediente();
                } else {
                    fncEditarExpediente();
                }
            })

            selectArchivoAplicable.fillCombo('/Administrativo/Reclutamientos/GetArchivosCombo', null, false, 'Todos');
            convertToMultiselect('#selectArchivoAplicable');

            selectCC.fillCombo('/Reclutamientos/FillFiltroCboCC', null, false, null);

            $(".select2").select2({ "width": "100%" });
            selectCC.select2({ "width": "100%" });

            convertToMultiselect('#cboFiltroEstatus');
            cboFiltroEstatus.multiselect('selectAll', false);
            cboFiltroEstatus.multiselect('refresh');
            cboFiltroEstatus.multiselect('deselect', 'Todos');

            chkFiltroEsPendiente.fillCombo("FillEstatusFiltro", {}, false, null);

            botonBuscar.on("click", function () {
                let numEstado = cboFiltroEstatus.val();
                if (numEstado == "Todos") {
                    numEstado = [0, 1, 2];
                }
                cargarExpedientes(numEstado);
            });

            botonDescargar.on('click', function () {
                location.href = '/Reclutamientos/DescargarAvanceExcel?estatus_emp=' + chkFiltroEsPendiente.val() + '&cc=' + selectCC.val();
            });

        })();

        function cargarExpedientes(numEstado) {
            axios.post('/Administrativo/Reclutamientos/CargarExpedientesDigitales', { estatus_emp: chkFiltroEsPendiente.val(), cc: selectCC.val(), estado: numEstado })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        initTablaExpedientes(response.data.columnas, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initTablaExpedientes(columnas, data) {
            let columns = columnas.map(x => {
                return {
                    data: x.Item1,
                    title: x.Item2,
                    render: function (data, type, row, meta) {
                        switch (data.estatus) {
                            case 0:
                                return `<button class="btn btn-xs btn-default botonRedondo botonArchivoNoAplica" archivo="${data.archivo_id}" id="botonArchivo_${row.id}_${data.archivo_id}" disabled><i class="fa fa-times"></i></button>`;
                            case 1:
                                return `<button class="btn btn-xs btn-success botonRedondo botonArchivoCargado" archivo="${data.archivo_id}" id="botonArchivo_${row.id}_${data.archivo_id}"><i class="fa fa-check"></i></button>`;
                            case 2:
                                return `
                                    <label for="inputFileArchivo_${row.id}_${data.archivo_id}" class="inputs pointer btn btn-xs btn-warning"><i class="fa fa-file"></i></label>
                                    <input id="inputFileArchivo_${row.id}_${data.archivo_id}" class="botonCargarArchivo" type="file" archivo="${data.archivo_id}" style="display:none;">
                                `;
                        }
                    }
                }
            });

            var columnasFinal = [{ data: 'claveEmpleado', title: 'Cve', width: '30px' }, { data: 'empleadoDesc', title: 'Empleado', width: '200px' }, { data: 'descPuesto', title: 'Puesto  ', width: '200px' }];

            columnasFinal.push(...columns);
            columnasFinal.push({
                render: function (data, type, row) {
                    return `<button class="btn btn-warning actualizarExpediente btn-xs" ><i class="far fa-edit"></i></button>&nbsp`;
                }
            });

            dtExpedientes = tablaExpedientes.DataTable({
                retrieve: true,
                paging: false,
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": true,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                fixedColumns: {
                    leftColumns: 3
                },
                initComplete: function (settings, json) {
                    tablaExpedientes.on('change', '.botonCargarArchivo', function () {
                        let rowData = dtExpedientes.row($(this).closest('tr')).data();
                        let archivo_id = +$(this).attr('archivo');
                        let listaArchivos = $(this)[0].files;

                        guardarArchivo(rowData.id, archivo_id, listaArchivos);
                    });

                    tablaExpedientes.on('click', '.botonArchivoCargado', function () {
                        // let rowData = dtExpedientes.row($(this).closest('tr')).data();
                        // let archivo_id = +$(this).attr('archivo');
                        // let link = document.createElement("button");

                        // link.download = '/Administrativo/Reclutamientos/DescargarArchivoExpediente?expediente_id=' + rowData.id + '&archivo_id=' + archivo_id;
                        // link.href = '/Administrativo/Reclutamientos/DescargarArchivoExpediente?expediente_id=' + rowData.id + '&archivo_id=' + archivo_id;
                        // link.click();
                        // location.href = '/Administrativo/Reclutamientos/DescargarArchivoExpediente?expediente_id=' + rowData.id + '&archivo_id=' + archivo_id;


                        let rowData = dtExpedientes.row($(this).closest('tr')).data();
                        let archivo_id = +$(this).attr('archivo');
                        let archivoCargado_id = rowData['columnaArchivo' + archivo_id].archivoCargado_id

                        fncAbrirMenu(rowData.id, archivo_id, archivoCargado_id);

                        $('#txtArchivo').on('change', function () {
                            let listaArchivos = $(this)[0].files;
                            guardarArchivo(rowData.id, archivo_id, listaArchivos);
                        });
                    });

                    tablaExpedientes.on('click', '.actualizarExpediente', function () {
                        let rowData = dtExpedientes.row($(this).closest('tr')).data();
                        inputClaveEmpleado.val(rowData.claveEmpleado);
                        inputClaveEmpleado.trigger("change");

                        let rowArch = Object.values(rowData).filter(x => x.estatus !== undefined);
                        for (let i = 0; i < rowArch.length; i++) {

                            if (rowArch[i].estatus == 1 || rowArch[i].estatus == 2) {
                                selectArchivoAplicable.multiselect('select', rowArch[i].archivo_id);
                            } else {
                                selectArchivoAplicable.multiselect('deselect', rowArch[i].archivo_id);

                            }

                        }
                        console.log(rowArch);
                        modalCaptura.modal('show');
                        txtBotonGuardar.text("Actualizar");
                    });
                },
                columns: columnasFinal,
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '20%', targets: [0] }
                ],
                colReorder: {
                    order: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 27, 25, 26, 28, 24, 29, 30]
                }
            });

            AddRows(tablaExpedientes, data);


        }

        function initTablaHistorial() {
            dtHistorial = tablaHistorial.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                columns: [
                    {
                        data: 'rutaArchivo', title: 'Nombre',
                        render: function (data, type, row, meta) {
                            return getFilename(data ?? "N/A");
                        }
                    },
                    { data: 'fechaCreacionString', title: 'Fecha Captura' },
                    {
                        title: 'Descargar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-primary botonDescargarArchivo"><i class="fas fa-file-download"></i></button>`;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaHistorial.on('click', '.botonDescargarArchivo', function () {
                        let rowData = dtHistorial.row($(this).closest('tr')).data();
                        fncDescargarArchivo(rowData.id, rowData.archivo_id);
                    });
                },
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        function limpiarModal() {
            inputClaveEmpleado.val('');
            inputNombreEmpleado.val('');
            selectArchivoAplicable.fillCombo('/Administrativo/Reclutamientos/GetArchivosCombo', null, false, 'Todos');
            selectArchivoAplicable.multiselect('selectAll', false);
            selectArchivoAplicable.multiselect('refresh');
            selectArchivoAplicable.multiselect('deselect', 'Todos');
        }

        function cargarInformacionEmpleado() {
            let claveEmpleado = +inputClaveEmpleado.val();

            if (claveEmpleado <= 0 || isNaN(claveEmpleado)) {
                Alert2Warning('Debe seleccionar una clave de empleado válida.');
                return;
            }

            axios.post('/Administrativo/Reclutamientos/CargarInformacionEmpleado', { claveEmpleado })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        inputNombreEmpleado.val(response.data.data.nombre);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarExpediente() {
            let claveEmpleado = +inputClaveEmpleado.val();
            let listaArchivosAplicables = getValoresMultiples('#selectArchivoAplicable');

            axios.post('/Administrativo/Reclutamientos/GuardarNuevoExpediente', { claveEmpleado, listaArchivosAplicables })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        cargarExpedientes([0, 1, 2]);
                        cboFiltroEstatus.multiselect('selectAll', false);
                        cboFiltroEstatus.multiselect('refresh');
                        cboFiltroEstatus.multiselect('deselect', 'Todos');
                        Alert2Exito('Se ha guardado la información.');
                        modalCaptura.modal('hide');

                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarArchivo(expediente_id, archivo_id, listaArchivos) {
            if (listaArchivos.length > 0) {
                const data = new FormData();

                data.append('archivo', listaArchivos[0]);
                data.append('expediente_id', expediente_id);
                data.append('archivo_id', archivo_id);

                axios.post('GuardarArchivoExpediente', data, { headers: { 'Content-Type': 'multipart/form-data' } })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            modalCaptura.modal('hide');
                            Alert2Exito('Se ha guardado la información.');
                            cargarExpedientes([0, 1, 2]);
                            cboFiltroEstatus.multiselect('selectAll', false);
                            cboFiltroEstatus.multiselect('refresh');
                            cboFiltroEstatus.multiselect('deselect', 'Todos');
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function fncEliminarArchivoExpediente(expediente_id, archivo_id) {
            axios.post("EliminarArchivoExpediente", { expediente_id, archivo_id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    cargarExpedientes([0, 1, 2]);
                    cboFiltroEstatus.multiselect('selectAll', false);
                    cboFiltroEstatus.multiselect('refresh');
                    cboFiltroEstatus.multiselect('deselect', 'Todos');
                    Alert2Exito('Se ha eliminado el archivo.');
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEditarExpediente() {

            let claveEmpleado = +inputClaveEmpleado.val();
            let listaArchivosAplicables = getValoresMultiples('#selectArchivoAplicable');

            axios.post("EditarExpediente", { claveEmpleado, listaArchivosAplicables }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Expediente actualizado.")
                    cargarExpedientes([0, 1, 2]);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAbrirMenu(id, archivoId, archivoCargado_id) {
            menuConfig = {
                lstOptions: [
                    { text: `<i class="fa fa-download"></i> Descargar`, action: "descargar", fn: parametros => { fncDescargarArchivo(archivoCargado_id, archivoId) } },
                    { text: `<i class="fa fa-eye"></i> Visualizar`, action: "visualizar", fn: parametros => { fncVisualizarArchivo(archivoCargado_id) } },
                    { text: `<i class="fa fa-align-justify"></i> Historial`, action: "historial", fn: parametros => { fncVerHistorial(id, archivoId) } },
                    { text: `<input id="txtArchivo" type="file" style="position: absolute; opacity: 0;"/><i class="fa fa-file"></i> Actualizar`, action: "actualizar", fn: parametros => { } },
                    {
                        text: `<i class="fa fa-file"></i> Eliminar`, action: "eliminar", fn: parametros => {
                            Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el archivo seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarArchivo(id, archivoId))
                        }
                    }

                ]
            }

            mostrarMenu();

        }

        function fncDescargarArchivo(archivoCargado_id, archivoId) {
            let link = document.createElement("button");

            link.download = '/Administrativo/Reclutamientos/DescargarArchivoExpediente?archivoCargado_id=' + archivoCargado_id + "&archivoId=" + archivoId;
            link.href = '/Administrativo/Reclutamientos/DescargarArchivoExpediente?archivoCargado_id=' + archivoCargado_id + "&archivoId=" + archivoId;
            link.click();
            location.href = '/Administrativo/Reclutamientos/DescargarArchivoExpediente?archivoCargado_id=' + archivoCargado_id + "&archivoId=" + archivoId;
        }

        function fncVisualizarArchivo(archivoCargado_id) {
            $.post('/Administrativo/Reclutamientos/CargarDatosArchivoExpediente', { archivoCargado_id })
                .then(response => {
                    if (response.success) {
                        $('#myModal').data().ruta = null;
                        $('#myModal').modal('show');
                    } else {
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function fncVerHistorial(id, archivoID) {
            axios.post('/Administrativo/Reclutamientos/VerHistorialExpediente', { expediente_id: id, archivo_id: archivoID })
                .then(response => {
                    let { success, datos, message } = response.data;
                    if (success) {
                        AddRows(tablaHistorial, response.data.data);
                        modalHistorial.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function fncEliminarArchivo(id, archivoId) {
            fncEliminarArchivoExpediente(id, archivoId);
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        function getFilename(fullPath) {
            return fullPath.replace(/^.*[\\\/]/, '');
        }
    }
    $(document).ready(() => Reclutamientos.ExpedienteDigital = new ExpedienteDigital())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();