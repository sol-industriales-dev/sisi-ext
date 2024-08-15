(() => {
    $.namespace('AltaProveedor.AltaProveedorColombia');

    AltaProveedorColombia = function () {


        //#region CONSTS
        const inpIdProv = $('#inpIdProv');
        const numpro = $('#numpro');

        const inpNumProv = $('#inpNumProv');
        const inpNombreProv = $('#inpNombreProv');
        const inpNombreCorto = $('#inpNombreCorto');

        const inpNombres = $('#inpNombres');
        const inpApePaterno = $('#inpApePaterno');
        const inpApeMaterno = $('#inpApeMaterno');

        const inpPostal = $("#inpPostal");
        const inpDireccion = $("#inpDireccion");
        const inpRfc = $("#inpRfc");

        const inpEmail = $("#inpEmail");
        const cboTipoTercero = $("#cboTipoTercero");

        const cboTipoPagoTercero = $("#cboTipoPagoTercero");
        const cboTipoOperacion = $("#cboTipoOperacion");

        const cboTipoProv = $("#cboTipoProv");
        const cboTipoMovBase = $("#cboTipoMovBase");
        const cboCiudad = $("#cboCiudad");
        const inpNacionalidad = $("#inpNacionalidad");
        const chkPersonaFisica = $("#chkPersonaFisica");
        const chkObligaCfi = $("#chkObligaCfi");
        const chkFactoraje = $("#chkFactoraje");
        const chkAutoretenedor = $("#chkAutoretenedor");
        const chkConvenioNomina = $("#chkConvenioNomina");
        const cboTipoRegimen = $("#cboTipoRegimen");
        const cboTipoIdentificacion = $("#cboTipoIdentificacion");
        const inpInfoSocio = $("#inpInfoSocio");
        const inpCalle = $("#inpCalle");
        const inpColonia = $("#inpColonia");
        const inpDelegacion = $("#inpDelegacion");
        const inpIdIdentificacion = $("#inpIdIdentificacion");
        const inpNumIdentificacion = $("#inpNumIdentificacion");
        const inpFax = $("#inpFax");
        const inpNITDV = $('#inpNITDV');

        const inpPyme = $("#inpPyme");

        const divPersonaFisica = $("#divPersonaFisica");
        var ischeck = false;
        const TblCom_sv_proveedores = $('#TblCom_sv_proveedores');
        let dtTblCom_sv_proveedores;
        const tablaArchivosAdjuntos = $('#tablaArchivosAdjuntos');
        let dtArchivoAdjunto;

        const btnNuevo = $('#btnNuevo');
        const mdlNuevoProveedor = $('#mdlNuevoProveedor');
        const txtArchivoAdjunto = $('#txtArchivoAdjunto');

        const btnCrearEditarProv = $('#btnCrearEditarProv');

        //Otros Datos
        const inpResponsable = $('#inpResponsable');
        const inpTelefono1 = $("#inpTelefono1");
        const inpTelefono2 = $("#inpTelefono2");
        const inpCuentaBancaria = $("#inpCuentaBancaria");
        const cboTipoMoneda = $("#cboTipoMoneda");
        const inpCurp = $("#inpCurp");
        const inpLineaCredito = $("#inpLineaCredito")
        const inpCondicionPago = $("#inpCondicionPago");
        const inpBaseIva = $("#inpBaseIva");
        const inpDescuento = $("#inpDescuento");
        const inpLada = $("#inpLada");
        const inpBeneficiario = $("#inpBeneficiario");
        var Personafisica = "";
        var Obliga = "";
        var Factoraje = "";
        var Autoretenedor = "";
        var ConvenioNomina = "";
        var validacionRfcPersona = "";
        var esEdicionKontrol = false;
        const _ESTATUS = {
            AUTORIZADA: 1,
            PENDIENTE: 0,
        };
        var lstArchivosAeliminar = [];
        //#endregion

        (function init() {
            fncListeners();
        })();

        function fncListeners() {            
            initTblCom_sv_proveedores();
            inittablaArchivosAdjuntos();

            GetProveedores();

            cboCiudad.fillCombo('/Enkontrol/AltaProveedor/FillComboCiudad', null, false, null);
            cboTipoProv.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoProveedor', null, false, null);
            cboTipoTercero.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoTercero', null, false, null);
            cboTipoOperacion.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoOperacion', null, false, null);
            // cboTipoPagoTercero.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoPagoTerceroTrans', null, false, null);
            cboTipoMovBase.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoMovBase', null, false, null);
            cboTipoRegimen.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoRegimen', null, false, null);
            cboTipoMoneda.fillCombo('/Enkontrol/AltaProveedor/FillComboTipoMoneda', null, false, null);

            btnCrearEditarProv.click(function () {
                fncGuardarEditarProveedor();
            });
            btnNuevo.click(function () {
                limpiarModal();
                btnCrearEditarProv.attr("data-esNuevo", 0);
                document.getElementById("inpNumProv").readOnly = false;
                dtArchivoAdjunto.clear();
                dtArchivoAdjunto.rows.add(lstArchivosAeliminar);
                dtArchivoAdjunto.draw();
                mdlNuevoProveedor.modal('show');
                fncGetLastProveedor();

            });
            // inpRfc.on('change', function () {
            //     let pattern = /^[a-zA-Z]{3,4}(\d{6})((\D|\d){2,3})?$/;
            //     let rfc = document.getElementById("inpRfc").value;
            //     if (pattern.test(rfc) != false) {
            //         Alert2Exito("RFC Valido")
            //     } else {
            //         Alert2Error("RFC No Valido")
            //         inpRfc.val("");
            //     }
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
        }

        //#region BACK
        function descargarArchivo(idArchivo) {
            if (idArchivo > 0) {
                location.href = `DescargarArchivo?idArchivo=${idArchivo}`;
            }
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
                    { data: 'direccion', title: 'DIRECCION' },
                    { data: 'ciudad', title: 'CIUDAD' },
                    { data: 'rfc', title: 'RFC' },
                    { data: 'descEstatus', title: 'STATUS', },
                    { data: 'puedeAutorizar', title: 'puedeAutorizar', visible: false },
                    {
                        title: 'VOBO',
                        render: function (data, type, row, meta) {
                            let btnVobo = `<button class='btn btn-xs btn-success darVobo' title='Vobo.'><i class='fas fa-check'></i>Vobo</button>&nbsp`;

                            if (row.puedeDarVobo != false) {
                                return `${btnVobo} `
                            } else { return `` }
                        },
                    },
                    {
                        title: 'AUTORIZACIÓN',
                        render: function (data, type, row, meta) {
                            let btnAutorizar = `<button class='btn btn-xs btn-success autorizarRegistro' title='Autorizar registro.'><i class='fas fa-check'></i>Autorizar</button>`;
                            if (row.puedeAutorizar != false) {
                                return `${btnAutorizar}`
                            } else { return `` }
                        },
                    },
                    {
                        data: 'Autorizado', title: 'OPCIONES',
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>&nbsp`;

                            if (row.vobo == true && row.Autorizado == true) {
                                return ``
                            } else if (row.vobo == true && row.Autorizado == false) { return `${btnEditar}` } else {
                                return `${btnEditar} ${btnEliminar}`
                            }
                        },
                    },
                ],
                initComplete: function (settings, json) {
                    TblCom_sv_proveedores.on('click', '.darVobo', function () {
                        let rowData = dtTblCom_sv_proveedores.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea dar el V.o.b.o para autorizar proveedor?', 'Confirmar', 'Cancelar', () => fncNotificarAlta(rowData.id));
                    });
                    TblCom_sv_proveedores.on('click', '.autorizarRegistro', function () {
                        let rowData = dtTblCom_sv_proveedores.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea autorizar el proveedor seleccionado?', 'Confirmar', 'Cancelar', () => fncAutorizarProveedor(rowData.id));
                    });
                    TblCom_sv_proveedores.on('click', '.editarRegistro', function () {
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

        function inittablaArchivosAdjuntos() {
            dtArchivoAdjunto = tablaArchivosAdjuntos.DataTable({
                language: dtDicEsp,
                destroy: false,
                ordering: true,
                searching: false,
                bFilter: true,
                info: true,
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
                        // fncVisualizarArchivoAdjunto(rowData.FK_numpro)
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
                    if (items.persona_fisica != "N") {
                        document.getElementById("chkPersonaFisica").checked = true;
                    } else {
                        document.getElementById("chkPersonaFisica").checked = false;
                    }
                    if (items.obliga_cfd != 0) {
                        document.getElementById("chkObligaCfi").checked = true;
                    } else {
                        document.getElementById("chkObligaCfi").checked = false;
                    }
                    if (items.bit_factoraje != "N") {
                        document.getElementById("chkFactoraje").checked = true;
                    } else {
                        document.getElementById("chkFactoraje").checked = false;
                    }
                    if (items.bit_autoretenedor != "N") {
                        document.getElementById("chkAutoretenedor").checked = true;
                    } else {
                        document.getElementById("chkAutoretenedor").checked = false;
                    }
                    if (items.convenio_nomina != "N") {
                        document.getElementById("chkConvenioNomina").checked = true;
                    } else {
                        document.getElementById("chkConvenioNomina").checked = false;
                    }

                    let NIT = items.rfc.split("-");
                    let numNTI = NIT[0];
                    let dvNTI = NIT.length > 1 ? NIT[1] : "";

                    inpIdProv.val(items.id);
                    inpNumProv.val(items.numpro);
                    inpNombreProv.val(items.nombre);
                    inpNombreCorto.val(items.nomcorto);
                    inpDireccion.val(items.direccion);
                    inpNombres.val(items.a_nombre);
                    inpApePaterno.val(items.a_paterno);
                    inpApeMaterno.val(items.a_materno);
                    inpNacionalidad.val(items.nacionalidad)
                    inpTelefono1.val(items.telefono1);
                    inpTelefono2.val(items.telefono2);
                    inpResponsable.val(items.responsable);
                    inpRfc.val(numNTI);
                    inpNITDV.val(dvNTI);
                    inpFax.val(items.fax);
                    inpPostal.val(items.cp);
                    inpEmail.val(items.email);
                    cboCiudad.val(items.ciudad).change();
                    // $("#cboTipoProv").change($("#cboTipoProv").val(items.tipo_prov));
                    // $("#cboTipoTercero").change($("#cboTipoTercero").val(items.tipo_tercero));
                    // $("#cboTipoOperacion").change($("#cboTipoOperacion").val(items.tipo_operacion));
                    // $("#cboTipoPagoTercero").change($("#cboTipoPagoTercero").val(items.tipo_pago_transferencia));


                    if (items.tipo_prov == "0" || items.tipo_prov == "") {
                        $("#cboTipoProv").change($("#cboTipoProv").val(""));
                    } else {
                        $("#cboTipoProv").change($("#cboTipoProv").val(items.tipo_prov));
                    }

                    if (items.tipo_operacion == "0" || items.tipo_operacion == "") {
                        $("#cboTipoOperacion").change($("#cboTipoOperacion").val(""));
                    } else {
                        $("#cboTipoOperacion").change($("#cboTipoOperacion").val(items.tipo_operacion));
                    }
                    if (items.id_regimen == "0" || items.id_regimen == "") {
                        $("#cboTipoRegimen").change($("#cboTipoRegimen").val(""));
                    } else {

                        $("#cboTipoRegimen").change($("#cboTipoRegimen").val(items.id_regimen));
                    }
                    if (items.tipo_tercero == "0" || items.tipo_tercero == "") {
                        $("#cboTipoTercero").change($("#cboTipcboTipoTercerooRegimen").val(""));
                    } else {

                        $("#cboTipoTercero").change($("#cboTipoTercero").val(items.tipo_tercero));
                    }

                    cboTipoMoneda.val(items.moneda).change();
                    inpCondicionPago.val(items.condpago);
                    inpCuentaBancaria.val(items.cta_bancaria);
                    inpLineaCredito.val(maskNumero(items.limcred));
                    cboTipoMovBase.val(items.tmbase).change();
                    // inpCurp.val(items.curp);
                    inpInfoSocio.val(items.socios);
                    inpCalle.val(items.calle);
                    inpDelegacion.val(items.deleg);
                    inpColonia.val(items.colonia);
                    inpLada.val(items.lada);
                    inpDescuento.val(items.descuento);
                    inpBaseIva.val(items.base_iva);
                    $("#inpCancelado").val(items.cancelado);

                    if (items.lstArchivos != null) {
                        dtArchivoAdjunto.clear();
                        dtArchivoAdjunto.rows.add(items.lstArchivos);
                        dtArchivoAdjunto.draw();
                    }
                    // }

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGuardarEditarProveedor() {
            let objProveedor = fncObjAltaProveedor();
            axios.post("GuardarEditarProveedorColombia", objProveedor, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    GetProveedores();
                    mdlNuevoProveedor.modal("hide");
                    Alert2Exito("Se ha creado el proveedor con éxito.");

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
            if (document.getElementById("chkAutoretenedor").checked == true) {
                Autoretenedor = "S";
            } else {
                Autoretenedor = "N";
            }
            if (document.getElementById("chkConvenioNomina").checked == true) {
                ConvenioNomina = "S";
            } else {
                ConvenioNomina = "N";
            }

            const archivoAdjunto = txtArchivoAdjunto.get(0).files[0];
            let objProveedor = new Object();
            if (archivoAdjunto == "") {
                Alert2Error('Debe adjuntar el documento en la seccion Adjuntar Archivos');
                return;
            }
            objProveedor = {
                persona_fisica: Personafisica,
                obliga_cfd: Obliga,
                bit_factoraje: Factoraje,
                filial: "N",
                id: inpIdProv.val(),
                bit_autoretenedor: Autoretenedor,
                convenio_nomina: ConvenioNomina,
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
                fax: inpFax.val(),
                rfc: inpRfc.val() + (inpNITDV.val() != "" ? ("-" + inpNITDV.val()) : ""),
                cp: inpPostal.val(),
                email: inpEmail.val(),
                ciudad: cboCiudad.val(),
                tipo_prov: cboTipoProv.val(),
                tipo_tercero: cboTipoTercero.val(),
                tipo_operacion: cboTipoOperacion.val(),
                moneda: cboTipoMoneda.val(),
                condpago: inpCondicionPago.val(),
                limcred: unmaskNumero(inpLineaCredito.val()),
                cta_bancaria: inpCuentaBancaria.val(),
                // curp: inpCurp.val(),
                tmbase: cboTipoMovBase.val(),
                socios: inpInfoSocio.val(),
                calle: inpCalle.val(),
                deleg: inpDelegacion.val(),
                colonia: inpColonia.val(),
                id_regimen: cboTipoRegimen.val(),
                // id_doc_identidad: cboTipoIdentificacion.val(),
                descuento: inpDescuento.val(),
                base_iva: inpBaseIva.val(),
                lada: inpLada.val(),
                beneficiario: inpBeneficiario.val(),
                lstArchivosAeliminar: lstArchivosAeliminar,
                cancelado: $("#inpCancelado").val(),
                esEdicionEnKontrol: esEdicionKontrol,
                esNuevo: btnCrearEditarProv.attr("data-esNuevo")
            }


            let formData = new FormData();
            formData.set('objFile', archivoAdjunto);
            formData.set('objProveedor', JSON.stringify(objProveedor));

            return formData;
        }

        function eliminarProveedor(numpro) {
            axios.post("eliminarProveedor", { id: numpro }).then(response => {
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
            inpNITDV.val("");
            inpFax.val("");
            inpPostal.val("");
            inpEmail.val("");
            cboCiudad.val("");
            cboTipoProv.val("");
            cboTipoTercero.val("");
            cboTipoMoneda.val("");
            cboTipoOperacion.val("");
            cboTipoPagoTercero.val("");
            inpCondicionPago.val("");
            inpCuentaBancaria.val("");
            inpLineaCredito.val("");
            cboTipoMovBase.val("1");
            inpDelegacion.val("");
            inpColonia.val("");
            inpInfoSocio.val("");
            lstArchivosAeliminar = [];

            txtArchivoAdjunto.val("");
            document.getElementById("chkFactoraje").checked = false;
            document.getElementById("chkPersonaFisica").checked = false;
            document.getElementById("chkObligaCfi").checked = false;
            document.getElementById("chkAutoretenedor").checked = false;
            document.getElementById("chkConvenioNomina").checked = false;
            cboTipoProv.change(cboTipoProv.val(""));
            cboTipoTercero.change(cboTipoTercero.val(""));
            cboTipoOperacion.change(cboTipoOperacion.val(""));
            cboTipoRegimen.change(cboTipoRegimen.val(""));
        };
        //#endregion

        //#region GENERALES
        function fncGetLastProveedor() {
            axios.post("GetLastProveedor", { tipoProveedor: 0 }).then(response => {
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
        AltaProveedor.AltaProveedorColombia = new AltaProveedorColombia();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();   