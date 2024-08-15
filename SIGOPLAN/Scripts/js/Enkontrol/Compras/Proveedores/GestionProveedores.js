(() => {
    $.namespace('Compra.Proveedores.GestionProveedores');
    GestionProveedores = function () {

        //#region CONSTS
        const idProv = $('#idProv');

        const inpCodigo = $('#inpCodigo');
        const inpPrimerNombre = $('#inpPrimerNombre');
        const inpApePaterno = $('#inpApePaterno');
        const inpSegundoNombre = $('#inpSegundoNombre');
        const inpApeMaterno = $('#inpApeMaterno');
        const inpRazon = $("#inpRazon");
        const inpDireccion = $("#inpDireccion");
        const inpLocalidad = $("#inpLocalidad");
        const inpPais = $("#inpPais");
        const inpTelefono = $("#inpTelefono");
        const inpFax = $("#inpFax");
        const inpRepresentante = $("#inpRepresentante");
        const inpCargoRepresentante = $("#inpCargoRepresentante");
        const inpTelRepresentante = $("#inpTelRepresentante");
        const inpEmail = $("#inpEmail");
        const cboFormaPago = $("#cboFormaPago");
        const inpEstado = $("#inpEstado");
        const inpFecha = $("#inpFecha");
        const cboTipoDoc = $("#cboTipoDoc");
        const inpUbigueo = $("#inpUbigueo");
        const cboTipoMov = $("#cboTipoMov");


        const divRazon = $("#divRazon");
        const divDatos = $("#divDatos");

        const TblCom_RegistroProveedores = $('#TblCom_RegistroProveedores');
        let dtTblCom_RegistroProveedores;

        const TblCom_RegistrosCuentas = $('#TblCom_RegistrosCuentas');
        let dtTblCom_RegistrosCuentas;


        const btnGuardar = $('#btnGuardar');
        const btnBuscar = $('#btnBuscar');
        const mdlNuevoProveedor = $('#mdlNuevoProveedor');


        const btnCuentaBanco = $('#btnCuentaBanco');
        const mdlCuentasBancarias = $('#mdlCuentasBancarias');
        const mdlNuevaCuenta = $('#mdlNuevaCuenta');

        const cboCuentaBanco = $("#cboCuentaBanco");
        const inpCuentaCorriente = $("#inpCuentaCorriente");
        const cboMoneda = $("#cboMoneda");
        const inpidProv = $("#inpidProv");
        const inpProveedor = $("#inpProveedor");
        const btnGuardarCuenta = $('#btnGuardarCuenta');
        var esCajaChica = false;
        //#endregion

        //#region ADJUNTAR ARCHIVOS
        const tablaArchivosAdjuntos = $('#tablaArchivosAdjuntos');
        let dtArchivoAdjunto;
        const txtArchivoAdjunto = $('#txtArchivoAdjunto');
        const btnNuevo = $('#btnNuevo');
        //#endregion

        const inpid = $('#inpid');
        const _ESTATUS = {
            AUTORIZADA: 1,
            PENDIENTE: 0,
        };

        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            initTblCom_RegistroProveedores();
            initTblCom_RegistrosCuentas();
            inittablaArchivosAdjuntos();
            GetProveedores();
            inpFecha.datepicker({
                format: 'DD/MM/YYYY',
            });
            cboTipoDoc.on("change", function () {
                if ($(this).val() == "0") {
                    divRazon.show();
                    divDatos.hide();
                } else if ($(this).val() == "1") {
                    divDatos.show();
                    divRazon.hide();
                }
                else if ($(this).val() == "2") {
                    divDatos.show();
                    divRazon.hide();
                }
                else if ($(this).val() == "3") {
                    divDatos.show();
                    divRazon.hide();
                }
                else if ($(this).val() == "4") {
                    divRazon.show();
                    divDatos.hide();
                }
                else if ($(this).val() == "6") {
                    divRazon.show();
                    divDatos.hide();
                } else if ($(this).val() == "7") {
                    divRazon.show();
                    divDatos.hide();
                }
                else {
                    divDatos.hide();
                }
            });

            btnBuscar.click(function () {
                GetProveedores();
            });
            cboTipoMov.change(function () {
                if (document.getElementById('cboTipoMov').checked) {
                    esCajaChica = true;
                }
            })
            btnGuardar.click(function () {
                //Validation/         
                if ($('#inpCodigo').val() == "") {
                    Alert2Error('Codigo Requerido');
                    return;
                }
                if ($('#inpDireccion').val() == "") {
                    Alert2Error('Dirección Requerida');
                    return;
                }
                if ($('#inpLocalidad').val() == "") {
                    Alert2Error('Localidad Requerido');
                    return;
                }
                if ($('#inpPais').val() == "") {
                    Alert2Error('País Requerido');
                    return;
                }
                if ($('#inpRazon').val() == "") {
                    Alert2Error('Razón Social Requerido');
                    return;
                }
                if (esCajaChica == true) {
                    Alert2AccionConfirmar('¡Cuidado!', '¿Desea dar de alta el proveedor como caja chica?, Se autorizará automaticamente', 'Confirmar', 'Cancelar', () => fncGuardarEditarProveedor());
                } else { fncGuardarEditarProveedor() }
            });


            btnGuardarCuenta.click(function () {
                //Validation/ 
                if ($('#inpProveedor').val() == "") {
                    Alert2Error('Codigo Requerido');
                    return;
                }
                if ($('#cboCuentaBanco').val() == "") {
                    Alert2Error('Banco Requerido');
                    return;
                }
                if ($('#inpCuentaCorriente').val() == "") {
                    Alert2Error('Cuenta  Requerida');
                    return;
                }
                if ($('#cboMoneda').val() == "") {
                    Alert2Error('Moneda Requerida');
                    return;
                }
                fncGuardarEditarCuentaBancoProveedor();
            });

            btnCuentaBanco.click(function () {
                var anexo = "03" + $('#inpCodigo').val();
                GetGetCuentaBancoProveedores(anexo);
                mdlCuentasBancarias.modal('show');
            });

            btnNuevo.on("click", function () {
                dtArchivoAdjunto.clear();
                dtArchivoAdjunto.draw();
            });
        }

        function initTblCom_RegistroProveedores() {
            dtTblCom_RegistroProveedores = TblCom_RegistroProveedores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                scrollX: false,
                scrollY: '45vh',
                columns: [
                    { data: 'id', title: 'ID' },
                    { data: 'PRVCCODIGO', title: 'CODIGO' },
                    { data: 'PRVCNOMBRE', title: 'RAZON SOCIAL' },
                    { data: 'PRVCDIRECC', title: 'DIRECCION' },
                    { data: 'PRVCTELEF1', title: 'TELEFONO' },
                    {
                        data: 'statusAutorizacion', title: 'STATUS', render: function (data, type, row) {
                            if (data == _ESTATUS.AUTORIZADA) {
                                return 'Autorizada';
                            }
                            if (data == _ESTATUS.PENDIENTE) {
                                return 'Pendiente'
                            }
                        }
                    },
                    {
                        title: 'OPCIONES',
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>&nbsp`;

                            return `${btnEditar} ${btnEliminar}`
                        },
                    },
                    {
                        title: 'VOBO',
                        render: function (data, type, row, meta) {
                            let btnVobo = `<button class='btn btn-xs btn-success darVobo' title='Vobo.'><i class='fas fa-check'></i>Vobo</button>&nbsp`;

                            if (row.PuedeVobo == true) {
                                return `${btnVobo} `
                            } else { return `` }
                        },
                    },
                    {
                        title: 'AUTORIZACIÓN',
                        render: function (data, type, row, meta) {
                            let btnAutorizar = `<button class='btn btn-xs btn-success autorizarRegistro' title='Autorizar registro.'><i class='fas fa-check'></i>Autorizar</button>`;
                            if (row.PuedeAutorizar == true) {
                                return `${btnAutorizar}`
                            } else { return `` }
                        },
                    },
                ],
                initComplete: function (settings, json) {
                    TblCom_RegistroProveedores.on('click', '.editarRegistro', function () {
                        let rowData = dtTblCom_RegistroProveedores.row($(this).closest('tr')).data();
                        fncGetConsultaProveedores(rowData.id);
                        mdlNuevoProveedor.modal('show');
                    });
                    TblCom_RegistroProveedores.on('click', '.eliminarRegistro', function () {
                        let rowData = dtTblCom_RegistroProveedores.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => eliminarProveedor(rowData.id));
                    });
                    TblCom_RegistroProveedores.on('click', '.autorizarRegistro', function () {
                        let rowData = dtTblCom_RegistroProveedores.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea autorizar el proveedor seleccionado?', 'Confirmar', 'Cancelar', () => fncAutorizarProveedor(rowData.id));
                    });
                    TblCom_RegistroProveedores.on('click', '.darVobo', function () {
                        let rowData = dtTblCom_RegistroProveedores.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea dar el V.o.b.o para autorizar proveedor?', 'Confirmar', 'Cancelar', () => fncNotificarAlta(rowData.id));
                    });

                    if (idProv.val()) {
                        var clean_uri = location.protocol + "//" + location.host + location.pathname;
                        window.history.replaceState({}, document.title, clean_uri);
                    }
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function initTblCom_RegistrosCuentas() {
            dtTblCom_RegistrosCuentas = TblCom_RegistrosCuentas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id', title: 'ID' },
                    { data: 'ANEXO', title: 'Código' },
                    { data: 'BAN_CODIGO', title: 'Banco' },
                    { data: 'MON_CODIGO', title: 'Moneda' },
                    { data: 'CTABAN_CODIGO', title: 'Cuenta Corriente' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>&nbsp`;

                            return `${btnEditar} ${btnEliminar}`

                        },
                    }
                ],
                initComplete: function (settings, json) {
                    TblCom_RegistrosCuentas.on('click', '.editarRegistro', function () {
                        let rowData = dtTblCom_RegistrosCuentas.row($(this).closest('tr')).data();
                        fncgetDatosCuentasBancoProveedor(rowData.id);
                        mdlNuevaCuenta.modal('show');
                    });
                    TblCom_RegistrosCuentas.on('click', '.eliminarRegistro', function () {
                        let rowData = dtTblCom_RegistrosCuentas.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => eliminarCuentaBancoProveedor(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
                drawCallback: function (settings) {
                }
            });
        }

        function GetProveedores() {
            axios.post('getProveedores').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtTblCom_RegistroProveedores.clear();
                    dtTblCom_RegistroProveedores.rows.add(items);
                    dtTblCom_RegistroProveedores.draw();
                    //$('#TblCom_RegistroProveedores_filter').find('input').val(idProv.val());
                    //$('#TblCom_RegistroProveedores_filter').find('input').trigger('keyUp');
                    TblCom_RegistroProveedores.DataTable().search(idProv.val()).draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetConsultaProveedores(id) {
            axios.post("getDatosProveedores", { id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    inpid.val(items.id);
                    inpCodigo.val(items.PRVCCODIGO);
                    inpRazon.val(items.PRVCNOMBRE);
                    inpDireccion.val(items.PRVCDIRECC);
                    inpLocalidad.val(items.PRVCLOCALI);
                    inpPais.val(items.PRVCPAISAC);
                    inpTelefono.val(items.PRVCTELEF1);
                    inpFax.val(items.PRVCFAXACR);
                    inpEstado.val(items.PRVCESTADO);
                    inpRepresentante.val(items.PRVCREPRES);
                    inpCargoRepresentante.val(items.PRVCCARREP);
                    inpTelRepresentante.val(items.PRVCTELREP);
                    inpEmail.val(items.PRVEMAIL);
                    inpFecha.val(moment(items.PRVDFECCRE).format('DD/MM/YYYY'));
                    cboFormaPago.val(items.PRVPAGO);
                    cboTipoDoc.val(items.PRVCTIPO_DOCUMENTO).change();
                    inpApePaterno.val(items.PRVCAPELLIDO_PATERNO);
                    inpApeMaterno.val(items.PRVCAPELLIDO_MATERNO);
                    inpPrimerNombre.val(items.PRVCPRIMER_NOMBRE);
                    inpSegundoNombre.val(items.PRVCSEGUNDO_NOMBRE);
                    inpUbigueo.val(items.UBIGEO)

                    dtArchivoAdjunto.clear();
                    dtArchivoAdjunto.rows.add(response.data.lstArchivos);
                    dtArchivoAdjunto.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGuardarEditarProveedor() {
            var DocIndent = "";
            var DocRuc = "";
            if (cboTipoDoc.val() == 1 || cboTipoDoc.val() == 2 || cboTipoDoc.val() == 3 || cboTipoDoc.val() == 4) {
                DocIndent = $('#inpCodigo').val()
            } else if (cboTipoDoc.val() == 4 || cboTipoDoc.val() == 5 || cboTipoDoc.val() == 6) {
                DocRuc = $('#inpCodigo').val()
            }
            var proveedor = {
                id: $('#inpid').val(),
                PRVCCODIGO: $('#inpCodigo').val(),
                PRVCNOMBRE: $('#inpRazon').val(),
                PRVCDIRECC: $('#inpDireccion').val(),
                PRVCLOCALI: $('#inpLocalidad').val(),
                PRVCPAISAC: $('#inpPais').val(),
                PRVCTELEF1: $('#inpTelefono').val(),
                PRVCFAXACR: $('#inpFax').val(),
                PRVCREPRES: $('#inpRepresentante').val(),
                PRVCCARREP: $('#inpCargoRepresentante').val(),
                PRVCTELREP: $('#inpTelRepresentante').val(),
                PRVDFECCRE: $('#inpFecha').val(),
                PRVCESTADO: $('#inpEstado').val(),
                PRVEMAIL: $('#inpEmail').val(),
                PRVPAGO: $('#cboFormaPago').val(),
                PRVCTIPO_DOCUMENTO: $('#cboTipoDoc').val(),
                PRVCAPELLIDO_PATERNO: $('#inpApePaterno').val(),
                PRVCAPELLIDO_MATERNO: $('#inpApeMaterno').val(),
                PRVCPRIMER_NOMBRE: $('#inpPrimerNombre').val(),
                PRVCSEGUNDO_NOMBRE: $('#inpSegundoNombre').val(),
                UBIGEO: $('#inpUbigueo').val(),
                PRVCDOCIDEN: DocIndent,
                PRVCRUC: DocRuc
            }

            const archivoAdjunto = txtArchivoAdjunto.get(0).files[0];
            let formData = new FormData();
            formData.set('objFile', archivoAdjunto);
            console.log(archivoAdjunto);
            formData.set('proveedor', JSON.stringify(proveedor));

            if (esCajaChica == true) {
                axios.post("GuardadoCajaChica", formData, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito("Se ha registrado con éxito.");
                        GetProveedores();
                        mdlNuevoProveedor.modal('hide');
                    } else {
                        Alert2Warning(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                axios.post("GuardarEditarProveedor", formData, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        GetProveedores();
                        mdlNuevoProveedor.modal('hide');
                    } else {
                        Alert2Warning(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGuardarCajaChica() {
            var DocIndent = "";
            var DocRuc = "";
            if (cboTipoDoc.val() == 1 || cboTipoDoc.val() == 2 || cboTipoDoc.val() == 3 || cboTipoDoc.val() == 4) {
                DocIndent = $('#inpCodigo').val()
            } else if (cboTipoDoc.val() == 4 || cboTipoDoc.val() == 5 || cboTipoDoc.val() == 6) {
                DocRuc = $('#inpCodigo').val()
            }
            var proveedor = {
                id: $('#inpid').val(),
                PRVCCODIGO: $('#inpCodigo').val(),
                PRVCNOMBRE: $('#inpRazon').val(),
                PRVCDIRECC: $('#inpDireccion').val(),
                PRVCLOCALI: $('#inpLocalidad').val(),
                PRVCPAISAC: $('#inpPais').val(),
                PRVCTELEF1: $('#inpTelefono').val(),
                PRVCFAXACR: $('#inpFax').val(),
                PRVCREPRES: $('#inpRepresentante').val(),
                PRVCCARREP: $('#inpCargoRepresentante').val(),
                PRVCTELREP: $('#inpTelRepresentante').val(),
                PRVDFECCRE: $('#inpFecha').val(),
                PRVCESTADO: $('#inpEstado').val(),
                PRVEMAIL: $('#inpEmail').val(),
                PRVPAGO: $('#cboFormaPago').val(),
                PRVCTIPO_DOCUMENTO: $('#cboTipoDoc').val(),
                PRVCAPELLIDO_PATERNO: $('#inpApePaterno').val(),
                PRVCAPELLIDO_MATERNO: $('#inpApeMaterno').val(),
                PRVCPRIMER_NOMBRE: $('#inpPrimerNombre').val(),
                PRVCSEGUNDO_NOMBRE: $('#inpSegundoNombre').val(),
                UBIGEO: $('#inpUbigueo').val(),
                PRVCDOCIDEN: DocIndent,
                PRVCRUC: DocRuc
            }
            axios.post("GuardadoCajaChica", { proveedor }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Se ha registrado con éxito.");
                    GetProveedores();
                    mdlNuevoProveedor.modal('hide');
                    // limpiarModalProveedor();
                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAutorizarProveedor(id) {
            axios.post("AutorizarProveedor", { id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    GetProveedores();
                    Alert2Exito("Se ha autorizado con éxito.");

                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncNotificarAlta(id) {
            axios.post('NotificarAltaProveedor', { id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito(message)
                    btnBuscar.trigger('click');
                } else {
                    Alert2Error(message)
                }
            }).catch(error => Alert2Error(error.message));
        }

        function eliminarProveedor(id) {
            axios.post("eliminarProveedor", { id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...

                    GetProveedores();
                    Alert2Exito("Se ha eliminado con éxito.");
                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));

        };

        function limpiarModalProveedor() {
            inpCodigo.val() = "";
            inpRazon.val() = "";
            inpDireccion.val() = "";
            inpLocalidad.val() = "";
            inpPais.val() = "";
            inpTelefono.val() = "";
            inpFax.val() = "";
            inpEstado.val() = "";
            inpRepresentante.val() = "";
            inpCargoRepresentante.val() = "";
            inpTelRepresentante.val() = "";
            inpEmail.val() = "";
            cboFormaPago.val() = "";
            inpEstado.val() = "";
            inpApePaterno.val() = "";
            inpApeMaterno.val() = "";
            inpPrimerNombre.val() = "";
            inpSegundoNombre.val() = "";
            inpFecha.val() = "";
            inpUbigueo.val() = "";
        }
        function limpiarModalCuentaProv() {
            inpidProv.val() = "";
            cboCuentaBanco.val() = "";
            inpCuentaCorriente.val() = "";
            inpProveedor.val() = "";
            cboMoneda.val() = "";

        }

        function fncgetDatosCuentasBancoProveedor(id) {
            axios.post("getDatosCuentasBancoProveedor", { id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    inpidProv.val(items.id);
                    inpProveedor.val(items.ANEXO);
                    cboCuentaBanco.val(items.BAN_CODIGO);
                    inpCuentaCorriente.val(items.CTABAN_CODIGO);
                    cboMoneda.val(items.MON_CODIGO);
                }
            }).catch(error => Alert2Error(error.message));
        }
        function GetGetCuentaBancoProveedores(anexo) {
            $('#inpProveedor').val(inpCodigo.val());
            anexo = "03" + $('#inpProveedor').val();

            axios.post('getCuentasBancosProveedores', { anexo }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtTblCom_RegistrosCuentas.clear();
                    dtTblCom_RegistrosCuentas.rows.add(items);
                    dtTblCom_RegistrosCuentas.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGuardarEditarCuentaBancoProveedor() {
            var CuentasBancosProveedor = {
                id: $('#inpidProv').val(),
                ANEXO: "03" + $('#inpProveedor').val(),
                BAN_CODIGO: $('#cboCuentaBanco').val(),
                CTABAN_CODIGO: $('#inpCuentaCorriente').val(),
                MON_CODIGO: $('#cboMoneda').val()

            }
            axios.post("guardarEditarCuentaBancoProveedor", { CuentasBancosProveedor }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Se ha registrado con éxito.");
                    GetGetCuentaBancoProveedores();
                    mdlNuevaCuenta.modal('hide');
                    // limpiarModalCuentaProv();
                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function eliminarCuentaBancoProveedor(id) {
            axios.post("eliminarCuentaBancoProveedor", { id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Se ha eliminado con éxito.");
                    GetGetCuentaBancoProveedores();
                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        };

        //#region ARCHIVOS ADJUNTOS
        function inittablaArchivosAdjuntos() {
            dtArchivoAdjunto = tablaArchivosAdjuntos.DataTable({
                language: dtDicEsp,
                destroy: false,
                ordering: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'FK_numpro', title: 'idArchivo', visible: false },
                    { data: 'nombreArchivo', title: 'Archivo' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title = 'Eliminar registro.'> <i class='fas fa-trash'></i></button>&nbsp;`;
                            let btnDescargar = `<button class='btn btn-xs btn-primary descargarArchivo' title = 'Descargar archivo.'> <i class="fas fa-file-download"></i></button>`;
                            return btnEliminar + btnDescargar;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tablaArchivosAdjuntos.on('click', '.eliminarRegistro', function () {
                        let objData = dtArchivoAdjunto.row($(this).closest('tr')).data();

                        // dtArchivoAdjunto.row($(this).closest('tr')).remove().draw();
                        fncRemoveArchivos(objData.id);
                    });

                    tablaArchivosAdjuntos.on('click', '.descargarArchivo', function () {
                        let rowData = dtArchivoAdjunto.row($(this).closest('tr')).data();
                        // fncVisualizarArchivoAdjunto(rowData.FK_numpro);
                        descargarArchivo(rowData.id);
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function descargarArchivo(idArchivo) {
            if (idArchivo > 0) {
                location.href = `DescargarArchivo?idArchivo=${idArchivo}`;
            }
        }

        function fncRemoveArchivos(idArchivo) {
            axios.post("RemoveArchivos", { idArchivo }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Registro eliminado")
                    dtArchivoAdjunto.clear();
                    dtArchivoAdjunto.draw();
                } else {
                    Alert2Warning("Ocurrio algo mal favor de comunicarse con el departamento de TI")
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        Compra.Proveedores.GestionProveedores = new GestionProveedores();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();