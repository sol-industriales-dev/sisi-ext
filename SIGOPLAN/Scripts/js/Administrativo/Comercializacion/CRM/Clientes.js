(() => {
    $.namespace('ADMIN_FINANZAS.Clientes');

    Clientes = function () {

        //#region CONST FILTROS
        const cboFiltro_Division = $('#cboFiltro_Division');
        const cboFiltro_Cliente = $('#cboFiltro_Cliente');
        const btnFiltro_Clientes = $('#btnFiltro_Clientes');
        const cboFiltro_Historial = $('#cboFiltro_Historial');
        const btnFiltro_BuscarContacto = $("#btnFiltro_BuscarContacto");
        const btnFiltro_NuevoContacto = $("#btnFiltro_NuevoContacto");
        //#endregion

        //#region CONST CREAR/EDITAR CONTACTOS
        const mdlCE_Contacto = $("#mdlCE_Contacto");
        const cboCE_Contacto_Clientes = $("#cboCE_Contacto_Clientes");
        const txtCE_Contacto_NombreContacto = $("#txtCE_Contacto_NombreContacto");
        const txtCE_Contacto_Puesto = $("#txtCE_Contacto_Puesto");
        const txtCE_Contacto_Telefono = $("#txtCE_Contacto_Telefono");
        const txtCE_Contacto_Extension = $("#txtCE_Contacto_Extension");
        const txtCE_Contacto_Celular = $("#txtCE_Contacto_Celular");
        const txtCE_Contacto_Correo = $("#txtCE_Contacto_Correo");
        const btnCE_Contacto = $('#btnCE_Contacto');
        //#endregion

        //#region CONST LISTADO CONTACTOS
        let dtContactos;
        const tblContactos = $('#tblContactos');
        //#endregion

        //#region CONST CREAR/EDITAR CLIENTES
        const mdlCE_Cliente = $('#mdlCE_Cliente');
        const btnFiltro_NuevoCliente = $('#btnFiltro_NuevoCliente');
        const txtCE_Cliente_NombreCliente = $("#txtCE_Cliente_NombreCliente");
        const cboCE_Cliente_Division = $("#cboCE_Cliente_Division");
        const cboCE_Cliente_Pais = $('#cboCE_Cliente_Pais');
        const cboCE_Cliente_Estado = $('#cboCE_Cliente_Estado');
        const cboCE_Cliente_Municipio = $('#cboCE_Cliente_Municipio');
        const txtCE_Cliente_PaginaWeb = $('#txtCE_Cliente_PaginaWeb');
        const cboCE_Cliente_TipoCliente = $('#cboCE_Cliente_TipoCliente');
        const btnCE_Cliente = $('#btnCE_Cliente');
        //#endregion

        //#region CONST LISTADO CLIENTES
        let dtClientes;
        const tblClientes = $('#tblClientes');
        const mdlClientes_Listado = $('#mdlClientes_Listado');
        const cboFiltro_Clientes_Historial = $('#cboFiltro_Clientes_Historial');
        const btnFiltro_BuscarCliente = $('#btnFiltro_BuscarCliente');
        //#endregion

        //#region ENUM
        const HistorialEnum = { ACTIVO: 1, HISTORIAL: 2 };
        //#endregion

        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblClientes();
            initTblClientesContactos();
            fncFillCbos();
            fncGetContactos();
            fncIndicarMenuSeleccion();
            //#endregion

            //#region FILTROS
            btnFiltro_Clientes.click(function () {
                fncGetClientes();
            });

            btnFiltro_BuscarContacto.click(function () {
                fncGetContactos();
            });

            btnFiltro_NuevoContacto.click(function () {
                btnCE_Contacto.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCE_Contacto.data().id = 0;
                fncLimpiarMdlCE_Contacto();
                mdlCE_Contacto.modal("show");
            });
            //#endregion

            //#region FILTROS CLIENTES
            btnFiltro_BuscarCliente.click(function () {
                fncGetClientes();
            });
            //#endregion

            //#region CREAR/EDITAR CLIENTES
            btnFiltro_NuevoCliente.click(function () {
                btnCE_Cliente.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCE_Cliente.data().id = 0;
                fncLimpiarMdlCE_Cliente();
                mdlClientes_Listado.modal("hide");
                mdlCE_Cliente.modal("show");
            });

            btnCE_Cliente.click(function () {
                if ($(this).data().id <= 0) {
                    fncCrearCliente();
                } else {
                    fncActualizarCliente();
                }
            });

            cboCE_Cliente_Pais.change(function () {
                if ($(this).val() > 0) {
                    cboCE_Cliente_Estado.fillCombo('FillCboEstados', { FK_Pais: $(this).val() }, false, null);
                }
            });

            cboCE_Cliente_Estado.change(function () {
                if ($(this).val() > 0) {
                    cboCE_Cliente_Municipio.fillCombo('FillCboMunicipios', { FK_Estado: $(this).val() }, false, null);
                }
            });
            //#endregion

            //#region CREAR/EDITAR CONTACTOS
            btnFiltro_NuevoContacto.click(function () {
                btnCE_Contacto.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCE_Contacto.data().id = 0;
                fncHabilitarDeshabilitarCboCE_Contacto_Clientes(false);
                fncLimpiarMdlCE_Contacto();
                cboCE_Contacto_Clientes.fillCombo('FillCboClientes', null, false, null);
                mdlCE_Contacto.modal("show");
            });

            btnCE_Contacto.click(function () {
                if ($(this).data().id <= 0) {
                    fncCrearContacto();
                } else {
                    fncActualizarContacto();
                }
            });
            //#endregion
        }

        //#region CLIENTES
        function initTblClientes() {
            dtClientes = tblClientes.DataTable({
                language: dtDicEsp,
                paging: true,
                ordering: true,
                searching: true,
                columns: [
                    { data: 'nombreCliente', title: 'Cliente' },
                    { data: 'division', title: 'División' },
                    { data: 'ubicacion', title: 'Ubicación' },
                    {
                        data: 'paginaWeb', title: "Página web"
                        // render: (data, type, row, meta) => {
                        //     return `<a href="${data}" target="_blank">${data}</a>`;
                        // }
                    },
                    {
                        title: "Acciones",
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            let btnEnviarHistorial = `<button class='btn btn-xs btn-primary enviarHistorial' title='Enviar cliente a historial.'><i class="fas fa-archive"></i></button>`
                            let btnActivarCliente = `<button class='btn btn-xs btn-primary activarCliente' title='Activar cliente.'><i class="fas fa-power-off"></i></button>`

                            botones = `${btnEditar} ${btnEliminar}`;
                            if (cboFiltro_Clientes_Historial.val() == HistorialEnum.ACTIVO) {
                                botones += ` ${btnEnviarHistorial}`;
                            }
                            else if (cboFiltro_Clientes_Historial.val() == HistorialEnum.HISTORIAL) {
                                botones += ` ${btnActivarCliente}`;
                            }

                            return botones;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblClientes.on('click', '.editarRegistro', function () {
                        let rowData = dtClientes.row($(this).closest('tr')).data();
                        fncGetDatosActualizarCliente(rowData.id);
                    });

                    tblClientes.on('click', '.eliminarRegistro', function () {
                        let rowData = dtClientes.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCliente(rowData.id));
                    });

                    tblClientes.on('click', '.enviarHistorial', function () {
                        let rowData = dtClientes.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea enviar al cliente a historial?<br>Nota: También se enviara sus contactos a historial.', 'Confirmar', 'Cancelar',
                            () => fncEnviarClienteHistorial(rowData.id));
                    });

                    tblClientes.on('click', '.activarCliente', function () {
                        let rowData = dtClientes.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea activar al cliente?', 'Confirmar', 'Cancelar', () => fncActivarCliente(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { className: 'dt-body-center', 'targets': '_all' },
                    // { width: '8%', targets: [4] }
                ],
            });
        }

        function fncGetClientes() {
            fncDefaultCtrls("cboFiltro_Clientes_Historial", true);
            if (cboFiltro_Clientes_Historial.val() > 0) {
                let objParamsDTO = {};
                objParamsDTO.FK_EstatusHistorial = cboFiltro_Clientes_Historial.val();
                axios.post('GetClientes', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtClientes.clear();
                        dtClientes.rows.add(items);
                        dtClientes.draw();
                        mdlClientes_Listado.modal("show");
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (cboFiltro_Clientes_Historial.val() <= 0) { fncValidacionCtrl("cboFiltro_Clientes_Historial", true, "Es necesario seleccionar el estatus."); }
            }
        }

        function fncEliminarCliente(idCliente) {
            if (idCliente > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idCliente;
                axios.post('EliminarCliente', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetClientes();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el registro.");
            }
        }

        function fncCrearCliente() {
            let objParamsDTO = fncCEOBJCliente();
            if (objParamsDTO != "") {
                axios.post('CrearCliente', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetClientes();
                        mdlCE_Cliente.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncActualizarCliente() {
            let objParamsDTO = fncCEOBJCliente();
            if (objParamsDTO != "") {
                axios.post('ActualizarCliente', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetClientes();
                        fncGetContactos();
                        mdlCE_Cliente.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetDatosActualizarCliente(idCliente) {
            if (idCliente > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idCliente;
                axios.post('GetDatosActualizarCliente', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnCE_Cliente.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
                        btnCE_Cliente.data().id = idCliente;
                        txtCE_Cliente_NombreCliente.val(items.nombreCliente);
                        cboCE_Cliente_Division.val(items.FK_Division);
                        cboCE_Cliente_Division.trigger("change");
                        cboCE_Cliente_TipoCliente.val(items.FK_TipoCliente);
                        cboCE_Cliente_TipoCliente.trigger("change");

                        if (items.FK_Pais > 0) {
                            cboCE_Cliente_Pais.val(items.FK_Pais);
                            cboCE_Cliente_Pais.trigger("change");
                        }

                        if (items.FK_Estado > 0) {
                            cboCE_Cliente_Estado.val(items.FK_Estado);
                            cboCE_Cliente_Estado.trigger("change");
                        }

                        if (items.FK_Municipio > 0) {
                            cboCE_Cliente_Municipio.val(items.FK_Municipio);
                            cboCE_Cliente_Municipio.trigger("change");
                        }

                        txtCE_Cliente_PaginaWeb.val(items.paginaWeb);
                        mdlClientes_Listado.modal("hide");
                        mdlCE_Cliente.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener la información del cliente a actualizar.");
            }
        }

        function fncCEOBJCliente() {
            fncDefaultCtrls("txtCE_Cliente_NombreCliente", false);
            fncDefaultCtrls("cboCE_Cliente_Division", true);
            fncDefaultCtrls("cboCE_Cliente_TipoCliente", true);
            fncDefaultCtrls("cboCE_Cliente_Pais", true);
            fncDefaultCtrls("cboCE_Cliente_Estado", true);
            fncDefaultCtrls("cboCE_Cliente_Municipio", true);
            fncDefaultCtrls("txtCE_Cliente_PaginaWeb", false);

            if ($.trim(txtCE_Cliente_NombreCliente.val()) == "") { fncValidacionCtrl("txtCE_Cliente_NombreCliente", false, "Es necesario indicar el nombre del cliente."); return ""; }
            if (cboCE_Cliente_Division.val() <= 0) { fncValidacionCtrl("cboCE_Cliente_Division", true, "Es necesario seleccionar una división."); return ""; }
            if (cboCE_Cliente_TipoCliente.val() <= 0) { fncValidacionCtrl("cboCE_Cliente_TipoCliente", true, "Es necesario seleccionar el tipo de cliente."); return ""; }
            if (cboCE_Cliente_Pais.val() <= 0) { fncValidacionCtrl("cboCE_Cliente_Pais", true, "Es necesario seleccionar el país."); return ""; }
            if (cboCE_Cliente_Estado.val() <= 0) { fncValidacionCtrl("cboCE_Cliente_Estado", true, "Es necesario seleccionar el estado."); return ""; }
            if (cboCE_Cliente_Municipio.val() <= 0) { fncValidacionCtrl("cboCE_Cliente_Municipio", true, "Es necesario seleccionar el municipio."); return ""; }

            let objParamsDTO = {};
            objParamsDTO.id = btnCE_Cliente.data().id;
            objParamsDTO.nombreCliente = txtCE_Cliente_NombreCliente.val();
            objParamsDTO.FK_Division = cboCE_Cliente_Division.val();
            objParamsDTO.FK_TipoCliente = cboCE_Cliente_TipoCliente.val();
            objParamsDTO.FK_Municipio = cboCE_Cliente_Municipio.val();
            objParamsDTO.paginaWeb = txtCE_Cliente_PaginaWeb.val();
            return objParamsDTO;
        }

        function fncLimpiarMdlCE_Cliente() {
            $("input[type='text']").val("");
            cboCE_Cliente_Division[0].selectedIndex = 0;
            cboCE_Cliente_Division.trigger("change");
            cboCE_Cliente_TipoCliente[0].selectedIndex = 0;
            cboCE_Cliente_TipoCliente.trigger("change");
            cboCE_Cliente_Pais[0].selectedIndex = 0;
            cboCE_Cliente_Pais.trigger("change");
            cboCE_Cliente_Estado[0].selectedIndex = 0;
            cboCE_Cliente_Estado.trigger("change");
            cboCE_Cliente_Municipio[0].selectedIndex = 0;
            cboCE_Cliente_Municipio.trigger("change");
        }

        function fncEnviarClienteHistorial(idCliente) {
            if (idCliente > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idCliente;
                axios.post('EnviarClienteHistorial', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetClientes();
                        fncGetContactos();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al enviar el cliente a historial.");
            }
        }

        function fncActivarCliente(idCliente) {
            if (idCliente > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idCliente;
                axios.post('ActivarCliente', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetClientes();
                        fncGetContactos();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al activar el cliente.");
            }
        }
        //#endregion

        //#region CONTACTOS
        function initTblClientesContactos() {
            dtContactos = tblContactos.DataTable({
                language: dtDicEsp,
                paging: true,
                ordering: true,
                searching: true,
                columns: [
                    { data: 'division', title: 'División' },
                    { data: 'nombreCliente', title: 'Cliente' },
                    { data: 'ubicacion', title: 'Ubicación' },
                    { data: 'nombreContacto', title: 'Contacto' },
                    { data: 'puesto', title: 'Puesto' },
                    { data: 'correo', title: 'Correo' },
                    { data: 'telefono', title: 'Teléfono' },
                    { data: 'extension', title: 'Extensión' },
                    { data: 'celular', title: 'Celular' },
                    {
                        title: "Acciones",
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            let btnEnviarHistorial = `<button class='btn btn-xs btn-primary enviarHistorial' title='Enviar cliente a historial.'><i class="fas fa-archive"></i></button>`
                            let btnActivarContacto = `<button class='btn btn-xs btn-primary activarContacto' title='Activar contacto.'><i class="fas fa-power-off"></i></button>`

                            botones = `${btnEditar} ${btnEliminar}`;
                            if (cboFiltro_Historial.val() == HistorialEnum.ACTIVO) {
                                botones += ` ${btnEnviarHistorial}`;
                            }
                            else if (cboFiltro_Historial.val() == HistorialEnum.HISTORIAL) {
                                botones += ` ${btnActivarContacto}`;
                            }

                            return botones;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblContactos.on('click', '.editarRegistro', function () {
                        let rowData = dtContactos.row($(this).closest('tr')).data();
                        fncGetDatosActualizarContacto(rowData.id);
                    });

                    tblContactos.on('click', '.eliminarRegistro', function () {
                        let rowData = dtContactos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarContacto(rowData.id));
                    });

                    tblContactos.on('click', '.enviarHistorial', function () {
                        let rowData = dtContactos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea enviar al contacto a historial?', 'Confirmar', 'Cancelar', () => fncEnviarContactoHistorial(rowData.id));
                    });

                    tblContactos.on('click', '.activarContacto', function () {
                        let rowData = dtContactos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea activar al contacto?', 'Confirmar', 'Cancelar', () => fncActivarContacto(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { className: 'dt-body-center', 'targets': '_all' },
                    { width: '8%', targets: [4] }
                ],
            });
        }

        function fncGetContactos() {
            fncDefaultCtrls("cboFiltro_Historial", true);
            if (cboFiltro_Historial.val() > 0) {
                let objParamsDTO = {};
                objParamsDTO.FK_Division = cboFiltro_Division.val();
                objParamsDTO.FK_Cliente = cboFiltro_Cliente.val();
                objParamsDTO.FK_EstatusHistorial = cboFiltro_Historial.val();
                axios.post('GetContactos', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtContactos.clear();
                        dtContactos.rows.add(items);
                        dtContactos.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (cboFiltro_Historial.val() <= 0) { fncValidacionCtrl("cboFiltro_Historial", true, "Es necesario seleccionar el estatus."); }
            }
        }

        function fncEliminarContacto(idContacto) {
            if (idContacto > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idContacto;
                axios.post('EliminarContacto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetContactos();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el registro.");
            }
        }

        function fncCrearContacto() {
            let objParamsDTO = fncCEOBJContacto();
            if (objParamsDTO != "") {
                axios.post('CrearContacto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetContactos();
                        mdlCE_Contacto.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncActualizarContacto() {
            let objParamsDTO = fncCEOBJContacto();
            if (objParamsDTO != "") {
                axios.post('ActualizarContacto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetContactos();
                        mdlCE_Contacto.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetDatosActualizarContacto(idContacto) {
            if (idContacto > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idContacto;
                axios.post('GetDatosActualizarContacto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnCE_Contacto.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
                        btnCE_Contacto.data().id = idContacto;
                        cboCE_Contacto_Clientes.fillCombo('FillCboClientes', null, false, null);
                        cboCE_Contacto_Clientes.val(items.FK_Cliente);
                        cboCE_Contacto_Clientes.trigger("change");
                        fncHabilitarDeshabilitarCboCE_Contacto_Clientes(true);
                        txtCE_Contacto_NombreContacto.val(items.nombreContacto);
                        txtCE_Contacto_Puesto.val(items.puesto);
                        txtCE_Contacto_Correo.val(items.correo);
                        txtCE_Contacto_Telefono.val(items.telefono);
                        txtCE_Contacto_Extension.val(items.extension);
                        txtCE_Contacto_Celular.val(items.celular);
                        mdlCE_Contacto.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener la información del contacto a actualizar.");
            }
        }

        function fncCEOBJContacto() {
            fncDefaultCtrls("cboCE_Contacto_Clientes", true);
            fncDefaultCtrls("txtCE_Contacto_NombreContacto", false);
            fncDefaultCtrls("txtCE_Contacto_Puesto", false);
            fncDefaultCtrls("txtCE_Contacto_Correo", false);
            fncDefaultCtrls("txtCE_Contacto_Telefono", false);
            fncDefaultCtrls("txtCE_Contacto_Extension", false);
            fncDefaultCtrls("txtCE_Contacto_Celular", false);

            if (cboCE_Contacto_Clientes.val() <= 0) { fncValidacionCtrl("cboCE_Contacto_Clientes", true, "Es necesario seleccionar un cliente."); return ""; }
            if ($.trim(txtCE_Contacto_NombreContacto.val()) == "") { fncValidacionCtrl("txtCE_Contacto_NombreContacto", false, "Es necesario indicar el nombre del contacto."); return ""; }
            if ($.trim(txtCE_Contacto_Puesto.val()) == "") { fncValidacionCtrl("txtCE_Contacto_Puesto", false, "Es necesario indicar el puesto."); return ""; }
            if ($.trim(txtCE_Contacto_Correo.val()) == "") { fncValidacionCtrl("txtCE_Contacto_Correo", false, "Es necesario indicar el correo."); return ""; }
            if ($.trim(txtCE_Contacto_Telefono.val()) == "") { fncValidacionCtrl("txtCE_Contacto_Telefono", false, "Es necesario indicar el teléfono."); return ""; }
            if ($.trim(txtCE_Contacto_Extension.val()) == "") { fncValidacionCtrl("txtCE_Contacto_Extension", false, "Es necesario indicar la extensión."); return ""; }
            if ($.trim(txtCE_Contacto_Celular.val()) == "") { fncValidacionCtrl("txtCE_Contacto_Celular", false, "Es necesario indicar el celular."); return ""; }

            let objParamsDTO = {};
            objParamsDTO.id = btnCE_Contacto.data().id;
            objParamsDTO.FK_Cliente = cboCE_Contacto_Clientes.val();
            objParamsDTO.nombreContacto = txtCE_Contacto_NombreContacto.val();
            objParamsDTO.puesto = txtCE_Contacto_Puesto.val();
            objParamsDTO.correo = txtCE_Contacto_Correo.val();
            objParamsDTO.telefono = txtCE_Contacto_Telefono.val();
            objParamsDTO.extension = txtCE_Contacto_Extension.val();
            objParamsDTO.celular = txtCE_Contacto_Celular.val();
            return objParamsDTO;
        }

        function fncLimpiarMdlCE_Contacto() {
            $("input[type='text']").val("");
            cboCE_Contacto_Clientes[0].selectedIndex = 0;
            cboCE_Contacto_Clientes.trigger("change");
        }

        function fncEnviarContactoHistorial(idContacto) {
            if (idContacto > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idContacto;
                axios.post('EnviarContactoHistorial', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetContactos();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al enviar el contacto a historial.");
            }
        }

        function fncActivarContacto(idContacto) {
            if (idContacto > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idContacto;
                axios.post('ActivarContacto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetContactos();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al activar el contacto.");
            }
        }
        //#endregion

        //#region GENERALES
        function fncFillCbos() {
            cboFiltro_Division.fillCombo('FillCboFiltro_Clientes_Divisiones', null, false, null);
            cboFiltro_Cliente.fillCombo('FillCboClientes', null, false, null);
            cboFiltro_Historial.fillCombo('FillCboHistorial', null, false, null);
            cboFiltro_Historial[0].selectedIndex = 1;
            cboFiltro_Historial.trigger("change");

            cboCE_Cliente_Division.fillCombo('FillCboDivisiones', null, false, null);
            cboCE_Cliente_Pais.fillCombo('FillCboPaises', null, false, null);
            cboCE_Cliente_TipoCliente.fillCombo('FillCboTipoClientes', null, false, null);
            cboFiltro_Clientes_Historial.fillCombo('FillCboHistorial', null, false, null);
            cboFiltro_Clientes_Historial[0].selectedIndex = 1;
            cboFiltro_Clientes_Historial.trigger("change");
            $(".select2").select2();
        }

        function fncHabilitarDeshabilitarCboCE_Contacto_Clientes(disabled) {
            cboCE_Contacto_Clientes.attr("disabled", disabled);
        }

        function fncIndicarMenuSeleccion() {
            const variables = fncGetParamsURL(window.location.href);
            if (variables != undefined) {
                $("#btnMenu_Clientes").removeClass("btn-success");
                $("#btnMenu_Clientes").addClass("btn-primary");
            }
        }

        function fncGetParamsURL(url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');
            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }
            return params;
        }
        //#endregion
    }

    $(document).ready(() => {
        ADMIN_FINANZAS.Clientes = new Clientes();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();