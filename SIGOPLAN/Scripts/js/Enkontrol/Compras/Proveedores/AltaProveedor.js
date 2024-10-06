(() => {
    $.namespace('AltaProveedor.AltaProveedor');

    AltaProveedor = function () {

        //#region CONSTS Y VARS

        const inpIdProv = $('#inpIdProv');

        const numpro = $('#numpro');

        const inpNumProv = $('#inpNumProv');
        const inpNombreProv = $('#inpNombreProv');
        const inpNombreCorto = $('#inpNombreCorto');
        const inpResponsable = $('#inpResponsable');
        const inpNombres = $('#inpNombres');
        const inpApePaterno = $('#inpApePaterno');
        const inpApeMaterno = $('#inpApeMaterno');

        const inpPostal = $("#inpPostal");
        const inpDireccion = $("#inpDireccion");
        const inpRfc = $("#inpRfc");
        const inpTelefono1 = $("#inpTelefono1");
        const inpTelefono2 = $("#inpTelefono2");
        const inpEmail = $("#inpEmail");
        const cboTipoTercero = $("#cboTipoTercero");
        const inpCurp = $("#inpCurp");
        const cboTipoPagoTercero = $("#cboTipoPagoTercero");
        const cboTipoOperacion = $("#cboTipoOperacion");
        const cboTipoMoneda = $("#cboTipoMoneda");
        const cboTipoProv = $("#cboTipoProv");
        const cboTipoMovBase = $("#cboTipoMovBase");
        const cboCiudad = $("#cboCiudad");
        const inpNacionalidad = $("#inpNacionalidad");
        const inpFax = $("#inpFax");

        const chkPersonaFisica = $("#chkPersonaFisica");
        const chkObligaCfi = $("#chkObligaCfi");
        const chkFactoraje = $("#chkFactoraje");
        const chkFilial = $("#chkFilial");

        const inpCuentaBancaria = $("#inpCuentaBancaria");

        const divPersonaFisica = $("#divPersonaFisica");
        var ischeck = false;
        const TblCom_sv_proveedores = $('#TblCom_sv_proveedores');
        let dtTblCom_sv_proveedores;
        const tablaArchivosAdjuntos = $('#tablaArchivosAdjuntos');
        let dtArchivoAdjunto;
        const tablaSocios = $('#tablaSocios');
        let dttablaSocios;

        const inpInfoSocio = $("#inpInfoSocio");

        const cboTipoPersona = $("#cboTipoPersona");
        const cboEstatusGeneral = $("#cboEstatusGeneral");
        const btnBuscar = $("#btnBuscar");
        const btnNuevo = $('#btnNuevo');
        const mdlNuevoProveedor = $('#mdlNuevoProveedor');
        const txtArchivoAdjunto = $('#txtArchivoAdjunto');
        const btnCrearEditarProv = $('#btnCrearEditarProv');

        const inpLineaCredito = $("#inpLineaCredito");


        const botonAgregarCuenta = $('#botonAgregarCuenta');
        const tblCuentaBancaria = $('#tblCuentaBancaria');
        let dttblCuentaBancaria;
        const cboBanco = $("#cboBanco");
        const cboMoneda = $("#cboMoneda");
        const inpCuenta = $("#inpCuenta");
        const cboTipoCuenta = $("#cboTipoCuenta");
        const inpClabe = $("#inpClabe");
        const inpPlaza = $("#inpPlaza");
        const inpPlastico = $("#inpPlastico");


        const inpSucursal = $("#inpSucursal");
        const inpCondicionPago = $("#inpCondicionPago");
        var Personafisica = "";
        var Obliga = "";
        var Factoraje = "";
        var Filial = "";

        const _ESTATUS = {
            AUTORIZADA: 1,
            PENDIENTE: 0,
        };
        var esEdicionKontrol = false;
        var esNuevo = false;
        var lstCuentasAEliminar = [];
        var lstCuentasNuevas = [];
        var lstArchivosAeliminar = [];

        var lstCuentasAEliminarEK = [];
        var lstArchivosAeliminarEK = [];

        //#endregion

        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            GetProveedores();
            initTblCom_sv_proveedores();
            inittablaArchivosAdjuntos();
            initDatatblCuentaBancaria();
            cboCiudad.fillCombo('/Enkontrol/AltaProveedor/FillComboCiudad', null, false, null);
            cboTipoProv.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoProveedor', null, false, null);
            cboTipoTercero.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoTercero', null, false, null);
            cboTipoOperacion.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoOperacion', null, false, null);
            // cboTipoPagoTercero.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoPagoTerceroTrans', null, false, null);
            cboTipoMovBase.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoMovBase', null, false, null);
            cboTipoMoneda.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoMoneda', null, false, null);
            cboMoneda.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoMoneda', null, false, null);
            cboBanco.fillCombo('/Enkontrol/AltaProveedor/FillComboBancos', null, false, null);
            // cboTipoCuenta.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoCuenta', null, false, null);


            btnCrearEditarProv.click(function () {
                fncGuardarEditarProveedor();
            });

            btnNuevo.click(function () {
                limpiarModal();
                btnCrearEditarProv.attr("data-esNuevo", 0);
                document.getElementById("inpNumProv").readOnly = false;
                dttblCuentaBancaria.clear();
                dttblCuentaBancaria.rows.add(lstCuentasAEliminar);
                dttblCuentaBancaria.draw();
                dtArchivoAdjunto.clear();
                dtArchivoAdjunto.rows.add(lstArchivosAeliminar);
                dtArchivoAdjunto.draw();
                mdlNuevoProveedor.modal('show');

            });

            botonAgregarCuenta.click(function () {
                if (inpNumProv.val() < 0 || inpNumProv.val() == "") {
                    Alert2Error("No se puede agregar cuenta sin numero de proveedor")
                    return;
                }
                agregarCuenta();
            });

            inpRfc.on('change', function () {
                let pattern = /^[a-zA-Z]{3,4}(\d{6})((\D|\d){2,3})?$/;
                let rfc = document.getElementById("inpRfc").value;
                if (pattern.test(rfc) != false) {
                } else {
                    Alert2Error("RFC No Valido")
                    inpRfc.val("");
                }
            })
            // $('#mdlNuevoProveedor').on('hidden', function () {
            //     $(this).removeData('modal');
            //     // limpiarModal();

            // });
            // inpLineaCredito.on('keyup', function () {
            //     maskNumero(inpLineaCredito.val());
            // })
            $("#inpLineaCredito").on({
                "focus": function (event) {
                    $(event.target).select();
                },
                "keypress": function (event) {
                    $(event.target).val(function (index, value) {
                        return value.replace(/\D/g, "")
                            .replace(/([0-9])([0-9]{2})$/, '$1.$2')
                            .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ",");
                    });
                }
            });

            cboTipoProv.on("change", function (event, noMostrarUltimoID) {
                if (!noMostrarUltimoID) {
                    if ($(this).val() > 0) {
                        fncGetLastProveedor($(this).val());
                    } else {
                        inpNumProv.val("");
                    }
                }
            });
        }

        //#region BACK, ETC.
        function descargarArchivo(idArchivo) {
            if (idArchivo > 0) {
                location.href = `DescargarArchivo?idArchivo=${idArchivo}`;
            }
        }
        function agregarCuenta() {

            if ($('#cboBanco').val() == "") {
                Alert2Error('Banco Requerido');
                return;
            }
            if ($('#cboMoneda').val() == "") {
                Alert2Error('Tipo de moneda Requerido');
                return;
            }
            if ($('#cboTipoCuenta').val() == "") {
                Alert2Error('Tipo de cuenta Requerido');
                return;
            }
            if (inpClabe.val() == "") {
                Alert2Error('Clabe requerida');
                return;
            }
            // if ($('#inpCuenta').val() == "") {
            //     Alert2Error('Cuenta Requerido');
            //     return;
            // }
            // if ($('#inpSucursal').val() == "") {
            //     Alert2Error('Sucursal Requerido');
            //     return;
            // }

            let inputData = {
                FK_idProv: 0,
                numpro: inpNumProv.val(),
                id_cta_dep: 0,
                banco: cboBanco.val(),
                descBanco: cboBanco.find('option:selected').text(),
                moneda: cboMoneda.val(),
                descMoneda: cboMoneda.find('option:selected').text(),
                cuenta: inpCuenta.val(),
                tipo_cta: cboTipoCuenta.val(),
                descCuenta: cboTipoCuenta.find('option:selected').text(),
                sucursal: inpSucursal.val(),
                plaza: inpPlaza.val(),
                clabe: inpClabe.val(),
                plastico: inpPlastico.val()

            }

            const { FK_idProv, numpro, id_cta_dep, banco, descBanco, moneda, descMoneda, cuenta, tipo_cta, descCuenta, sucursal, plaza, clabe, plastico } = inputData;

            if (tblCuentaBancaria.find('tbody tr').toArray().some(x => $(x).find('p').text() == numpro)) {
                return;
            }

            añadirRowCuenta(FK_idProv, numpro, id_cta_dep, banco, descBanco, moneda, descMoneda, cuenta, tipo_cta, descCuenta, sucursal, plaza, clabe, plastico);

            cboBanco.val('');
            cboMoneda.val('');
            inpCuenta.val('');
            cboTipoCuenta.val('');
            inpSucursal.val('');
            inpPlaza.val('');
            inpClabe.val('');
            inpPlastico.val('');
            cboBanco.focus();

        }

        function añadirRowCuenta(FK_idProv, numpro, id_cta_dep, banco, descBanco, moneda, descMoneda, cuenta, tipo_cta, descCuenta, sucursal, plaza, clabe, plastico) {
            dttblCuentaBancaria.row.add({
                FK_idProv, numpro, id_cta_dep, banco, descBanco, moneda, descMoneda, cuenta, tipo_cta, descCuenta, sucursal, plaza, clabe, plastico
            }).draw();

            lstCuentasNuevas.push({ FK_idProv: FK_idProv, numpro: numpro, id_cta_dep: id_cta_dep, banco: banco, descBanco: descBanco, moneda: moneda, descMoneda: descMoneda, cuenta: cuenta, tipo_cta: tipo_cta, descCuenta: descCuenta, sucursal: sucursal, plaza: plaza, clabe: clabe, plastico: plastico });

        }

        function initTblCom_sv_proveedores() {
            dtTblCom_sv_proveedores = TblCom_sv_proveedores.DataTable({
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
                    // { data: 'id', title: 'ID', visible: false },
                    { data: 'numpro', title: 'NUM.PROV' },
                    { data: 'nombre', title: 'NOMBRE' },
                    { data: 'direccion', title: 'DIRECCIÓN' },
                    { data: 'ciudad', title: 'CIUDAD' },
                    { data: 'rfc', title: 'RFC' },
                    // { data: 'descEstatus', title: 'STATUS', },
                    // { data: 'puedeAutorizar', title: 'puedeAutorizar', visible: false },
                    // {
                    //     title: 'VOBO',
                    //     render: function (data, type, row, meta) {
                    //         let btnVobo = `<button class='btn btn-xs btn-success darVobo' title='Vobo.'><i class='fas fa-check'></i>Vobo</button>&nbsp`;

                    //         if (row.puedeDarPrimerVobo) {
                    //             return btnVobo;
                    //         } else {
                    //             if (row.puedeDarVobo != false) {
                    //                 return `${btnVobo} `
                    //             } else { return `` }
                    //         }
                    //     },
                    // },
                    // {
                    //     title: 'AUTORIZACIÓN',
                    //     render: function (data, type, row, meta) {
                    //         let btnAutorizar = `<button class='btn btn-xs btn-success autorizarRegistro' title='Autorizar registro.'><i class='fas fa-check'></i>Autorizar</button>`;
                    //         if (row.puedeAutorizar != false) {
                    //             return `${btnAutorizar}`
                    //         } else { return `` }
                    //     },
                    // },
                    {
                        data: 'Autorizado', title: 'OPCIONES',
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>&nbsp`;

                            // if (row.vobo == true && row.Autorizado == true) {
                            //     return ``
                            // } else if (row.vobo == true && row.Autorizado == false) { return `${btnEditar}` } else {
                            //     return `${btnEditar} ${btnEliminar}`
                            // }
                            return `${btnEditar} ${btnEliminar}`
                        },
                    },
                ],
                initComplete: function (settings, json) {
                    // TblCom_sv_proveedores.on('click', '.darVobo', function () {
                    //     let rowData = dtTblCom_sv_proveedores.row($(this).closest('tr')).data();
                    //     Alert2AccionConfirmar('¡Cuidado!', '¿Desea dar el V.o.b.o para autorizar proveedor?', 'Confirmar', 'Cancelar', () => fncNotificarAlta(rowData.id));
                    // });
                    // TblCom_sv_proveedores.on('click', '.autorizarRegistro', function () {
                    //     let rowData = dtTblCom_sv_proveedores.row($(this).closest('tr')).data();
                    //     Alert2AccionConfirmar('¡Cuidado!', '¿Desea autorizar el proveedor seleccionado?', 'Confirmar', 'Cancelar', () => fncAutorizarProveedor(rowData.id));
                    // });
                    TblCom_sv_proveedores.on('click', '.editarRegistro', function () {
                        editarRegistro = [];
                        let rowData = dtTblCom_sv_proveedores.row($(this).closest('tr')).data();
                        btnCrearEditarProv.attr("data-esNuevo", 1);
                        if (rowData.esEnKontrol) {
                            esEdicionKontrol = true;
                        }
                        fncGetConsultaProveedores(rowData.id, rowData.numpro);
                        mdlNuevoProveedor.modal("show");
                    });
                    TblCom_sv_proveedores.on('click', '.eliminarRegistro', function () {
                        let rowData = dtTblCom_sv_proveedores.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => eliminarProveedor(rowData.id));
                    });

                    if (numpro.val()) {
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

        function initDatatblCuentaBancaria() {
            dttblCuentaBancaria = tblCuentaBancaria.DataTable({
                language: dtDicEsp,
                destroy: true,
                ordering: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'numpro', title: 'numpro', visible: false },
                    { data: 'id_cta_dep', title: 'Cnsc. dep', visible: false },
                    { data: 'banco', title: 'idBanco', visible: false },
                    { data: 'descBanco', title: 'Banco' },
                    { data: 'moneda', title: 'idMoneda', visible: false },
                    { data: 'descMoneda', title: 'Moneda' },
                    { data: 'cuenta', title: 'Cuenta' },
                    { data: 'tipo_cta', title: 'idCuenta', visible: false },
                    { data: 'descCuenta', title: 'TipoCuenta' },
                    { data: 'sucursal', title: 'Sucursal' },
                    { data: 'plaza', title: 'Plaza' },
                    { data: 'clabe', title: 'Clabe' },
                    { data: 'plastico', title: 'Plástico' },
                    {
                        title: 'Opciones',
                        render: function (data, type, row, meta) {
                            let btnEiminar = `<button class="btn btn-danger botonEliminarCuenta"><i class="fas fa-trash-alt"></i></button>`;

                            if (row.isKontrol == true) {
                                return ``
                            } else {
                                return `${btnEiminar}`
                            }
                        }

                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {
                    tblCuentaBancaria.find('.botonEliminarCuenta').unbind().click(function () {
                        var objData = dttblCuentaBancaria.row($(this).closest('tr')).data();
                        if (objData.id > 0) {
                            lstCuentasAEliminar.push({ id: objData.id, numpro: objData.numpro, id_cta_dep: objData.id_cta_dep });
                            dttblCuentaBancaria.row($(this).closest('tr')).remove().draw();
                        } else if (objData.esEnKontrol) {
                            lstCuentasAEliminar.push({ id: objData.id, numpro: objData.numpro, id_cta_dep: objData.id_cta_dep });
                            dttblCuentaBancaria.row($(this).closest('tr')).remove().draw();

                        } else {
                            dttblCuentaBancaria.row($(this).closest('tr')).remove().draw();
                        }
                    });
                }
            });
        }
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
                        lstArchivosAeliminar.push(objData.id);

                        dtArchivoAdjunto.row($(this).closest('tr')).remove().draw();

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

        function GetProveedores() {
            axios.post('getProveedores').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtTblCom_sv_proveedores.clear();
                    dtTblCom_sv_proveedores.rows.add(items);
                    dtTblCom_sv_proveedores.draw();

                    TblCom_sv_proveedores.DataTable().search(numpro.val()).draw();

                }
            }).catch(error => Alert2Error(error.message));
        }
        function fncGetConsultaProveedores(id, numpro) {
            axios.post("obtenerDatosProveedores", { id, numpro }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    document.getElementById("inpNumProv").readOnly = true;
                    if (items.persona_fisica == "S") {
                        document.getElementById("chkPersonaFisica").checked = true;
                    } else {
                        document.getElementById("chkPersonaFisica").checked = false;
                    }
                    if (items.obliga_cfd == 1) {
                        document.getElementById("chkObligaCfi").checked = true;
                    } else {
                        document.getElementById("chkObligaCfi").checked = false;
                    }
                    if (items.bit_factoraje == "S") {
                        document.getElementById("chkFactoraje").checked = true;
                    } else {
                        document.getElementById("chkFactoraje").checked = false;
                    }
                    if (items.filial == "S") {
                        document.getElementById("chkFilial").checked = true;
                    } else {
                        document.getElementById("chkFilial").checked = false;
                    }

                    $("#inpIdProv").val(items.id);
                    $("#inpNumProv").val(items.numpro);
                    $("#inpNombreProv").val(items.nombre);
                    $("#inpNombreCorto").val(items.nomcorto);
                    $("#inpDireccion").val(items.direccion);
                    $("#inpNombres").val(items.a_nombre);
                    $("#inpApePaterno").val(items.a_paterno);
                    $("#inpApeMaterno").val(items.a_materno);
                    $("#inpNacionalidad").val(items.nacionalidad)
                    $("#inpTelefono1").val(items.telefono1);
                    $("#inpTelefono2").val(items.telefono2);
                    $("#inpResponsable").val(items.responsable);
                    $("#inpFax").val(items.fax);
                    $("#inpRfc").val(items.rfc);
                    $("#inpPostal").val(items.cp);
                    $("#inpEmail").val(items.email);
                    $("#cboCiudad").val(items.ciudad).change();

                    // $("#cboTipoProv").change($("#cboTipoProv").val(items.tipo_prov));
                    $("#cboTipoTercero").change($("#cboTipoTercero").val(items.tipo_tercero));
                    // $("#cboTipoOperacion").change($("#cboTipoOperacion").val(items.tipo_operacion));
                    $("#cboTipoPagoTercero").change($("#cboTipoPagoTercero").val(items.tipo_pago_transferencia));

                    if (items.tipo_prov == "0" || items.tipo_prov == "") {
                        // $("#cboTipoProv").change($("#cboTipoProv").val(""));
                        cboTipoProv.val("");
                        cboTipoProv.trigger("change", ["noMostrarUltimoID"]);
                    } else {
                        // $("#cboTipoProv").change($("#cboTipoProv").val(items.tipo_prov));
                        cboTipoProv.val(items.tipo_prov);
                        cboTipoProv.trigger("change", ["noMostrarUltimoID"]);
                    }

                    if (items.tipo_operacion == "0" || items.tipo_operacion == "") {
                        $("#cboTipoOperacion").change($("#cboTipoOperacion").val(""));
                    } else {
                        $("#cboTipoOperacion").change($("#cboTipoOperacion").val(items.tipo_operacion));
                    }

                    $("#cboTipoMoneda").val(items.moneda).change();
                    $("#inpCondicionPago").val(items.condpago);
                    $("#inpCuentaBancaria").val(items.cta_bancaria);
                    $("#inpLineaCredito").val(maskNumero(items.limcred));
                    $("#cboTipoMovBase").val(items.tmbase).change();
                    $("#cboBanco").val(items.cve_banco);
                    $("#cboTipoMovBase").val(items.tmbase).change();
                    $("#inpCurp").val(items.curp);
                    $("#inpInfoSocio").val(items.socios);
                    $("#inpCancelado").val(items.cancelado);

                    if (items.lstCuentas != null) {
                        dttblCuentaBancaria.clear();
                        dttblCuentaBancaria.rows.add(items.lstCuentas);
                        dttblCuentaBancaria.draw();
                    }
                    if (items.lstArchivos != null) {
                        dtArchivoAdjunto.clear();
                        dtArchivoAdjunto.rows.add(items.lstArchivos);
                        dtArchivoAdjunto.draw();
                    }

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGuardarEditarProveedor() {
            let objProveedor = fncObjAltaProveedor();
            axios.post("GuardarProveedor", objProveedor, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    GetProveedores();
                    mdlNuevoProveedor.modal("hide");
                    Alert2Exito("Se ha creado el proveedor con éxito.");
                    limpiarModal();
                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        function fncAutorizarProveedor(id) {
            axios.post("AutorizarProveedor", { numpro }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...                    
                    Alert2Exito("Se ha autorizado con éxito.");
                    GetProveedores();
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
                    GetProveedores();
                } else {
                    Alert2Error(message)
                }
            }).catch(error => Alert2Error(error.message));
        }
        function fncObjAltaProveedor() {

            if (document.getElementById("chkPersonaFisica").checked == true) {
                Personafisica = "S";
            } else {
                Personafisica = "N";
            }
            if (document.getElementById("chkObligaCfi").checked == true) {
                Obliga = 1;

            } else {
                Obliga = 0;
            }
            if (document.getElementById("chkFactoraje").checked == true) {
                Factoraje = "S";
            } else {
                Factoraje = "N";
            }
            if (document.getElementById("chkFilial").checked == true) {
                Filial = "S";
            } else {
                Filial = "N";
            }

            const archivoAdjunto = txtArchivoAdjunto.get(0).files[0];

            // if (archivoAdjunto == "") {
            //     Alert2Error('Debe adjuntar el documento en la seccion Adjuntar Archivos');
            //     return;
            // }
            let objProveedor = new Object();
            let cuentas = [];
            // objCuentas = tblCuentaBancaria.DataTable().cells($(this).closest("td").siblings().eq(0)).data();

            dttblCuentaBancaria.rows().data().toArray().forEach(infCuenta => {
                const { id, FK_idProv, numpro, id_cta_dep, banco, descBanco, moneda, descMoneda, cuenta, tipo_cta, descCuenta, sucursal, plaza, clabe, plastico
                    // , aplicaAutorizacion 
                } = infCuenta;
                cuentas.push({
                    id, FK_idProv, numpro, id_cta_dep, banco, descBanco, moneda, descMoneda, cuenta, tipo_cta, descCuenta, sucursal, plaza, clabe, plastico

                    // estatusAutorizacion: aplicaAutorizacion ? 2 : 1
                });

            });

            objProveedor = {
                persona_fisica: Personafisica,
                obliga_cfd: Obliga,
                bit_factoraje: Factoraje,
                filial: Filial,
                id: inpIdProv.val(),
                numpro: inpNumProv.val(),
                nombre: inpNombreProv.val(),
                nomcorto: inpNombreCorto.val(),
                direccion: inpDireccion.val(),
                a_nombre: inpNombres.val(),
                a_paterno: inpApePaterno.val(),
                a_materno: inpApeMaterno.val(),
                nacionalidad: inpNacionalidad.val(),
                telefono1: inpTelefono1.val(),
                telefono2: inpTelefono2.val(),
                responsable: inpResponsable.val(),
                rfc: inpRfc.val(),
                fax: inpFax.val(),
                cp: inpPostal.val(),
                email: inpEmail.val(),
                ciudad: cboCiudad.val(),
                tipo_prov: cboTipoProv.val(),
                tipo_tercero: cboTipoTercero.val(),
                tipo_operacion: cboTipoOperacion.val(),
                tipo_pago_transferencia: cboTipoPagoTercero.val(),
                moneda: cboTipoMoneda.val(),
                condpago: inpCondicionPago.val() != "" ? inpCondicionPago.val() : 0,
                limcred: unmaskNumero(inpLineaCredito.val()),
                cta_bancaria: inpCuentaBancaria.val(),
                curp: inpCurp.val(),
                tmbase: cboTipoMovBase.val(),
                socios: inpInfoSocio.val(),
                cancelado: $("#inpCancelado").val(),
                lstCuentas: cuentas,
                lstCuentasAeliminar: lstCuentasAEliminar,
                lstArchivosAeliminar: lstArchivosAeliminar,
                lstCuentasNuevas: lstCuentasNuevas,
                esEdicionEnKontrol: esEdicionKontrol,
                esNuevo: btnCrearEditarProv.attr("data-esNuevo")
            }

            let formData = new FormData();
            formData.set('objFile', archivoAdjunto);
            formData.set('objProveedor', JSON.stringify(objProveedor));
            // formData.set('objCuentas', cuentas);

            return formData;
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
        function limpiarModal() {
            // btnCrearEditarProv.data().esNuevo = "";
            inpIdProv.val("");
            inpNumProv.val("");
            inpNombreProv.val("");
            inpNombreCorto.val("");
            inpDireccion.val("");
            inpNombres.val("");
            inpApePaterno.val("");
            inpApeMaterno.val("");
            inpNacionalidad.val("")
            inpTelefono1.val("");
            inpTelefono2.val("");
            inpResponsable.val("");
            inpRfc.val("");
            inpPostal.val("");
            inpEmail.val("");
            cboCiudad.val("");
            cboTipoTercero.val("");
            cboTipoMoneda.val("");
            cboTipoOperacion.val("");
            cboTipoPagoTercero.val("");
            inpCondicionPago.val("");
            inpCuentaBancaria.val("");
            inpLineaCredito.val("");
            cboTipoMovBase.val("1");
            cboBanco.val("");
            inpInfoSocio.val("");
            lstCuentasAEliminar = [];
            lstArchivosAeliminar = [];
            txtArchivoAdjunto.val("");
            document.getElementById("chkFactoraje").checked = false;
            document.getElementById("chkPersonaFisica").checked = false;
            document.getElementById("chkObligaCfi").checked = false;
            document.getElementById("chkFilial").checked = false;
            cboTipoProv.val("");
            cboTipoProv.trigger("change", ["noMostrarUltimoID"]);
            // cboTipoProv.change();
            // cboTipoTercero.change(cboTipoTercero.val("0"));
            // cboTipoOperacion.change(cboTipoOperacion.val("0"));
            cboTipoPagoTercero.change(cboTipoPagoTercero.val("0"));
        };
        //#endregion

        //#region GENERALES
        function fncGetLastProveedor(tipoProveedor) {
            axios.post("GetLastProveedor", { tipoProveedor }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    inpNumProv.val(response.data.lastNumPro);
                } else {
                    Alert2Warning("Ocurrio algo mal adquiriendo el ultimo numero de proveedor disponible, favor de comunicarse con el departamento de TI")
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

    }
    $(document).ready(() => {
        AltaProveedor.AltaProveedor = new AltaProveedor();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();   