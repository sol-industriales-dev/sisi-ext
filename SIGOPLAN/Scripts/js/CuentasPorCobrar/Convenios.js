(() => {
    $.namespace('CuentasPorCobrar.CuentasPorCobrar.ControlEstimaciones');

    //#region CONSTS
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroNuevo = $('#btnFiltroNuevo');
    const mdlCEConvenio = $('#mdlCEConvenio');
    const txtCEConvenioTitulo = $('#txtCEConvenioTitulo');
    const btnCEConvenio = $('#btnCEConvenio');
    const cboFiltroEstatus = $('#cboFiltroEstatus');

    const tblConvenios = $('#tblConvenios');
    let dtConvenios;

    const tblAbonos = $('#tblAbonos');
    let dtAbonos;
    //#endregion

    //#region CONSTS CE
    const txtCEConvenioNumcte = $('#txtCEConvenioNumcte');
    const txtCEConvenioNombrecliente = $('#txtCEConvenioNombrecliente');
    const cboCEConvenioFactura = $('#cboCEConvenioFactura');
    const cboCEConvenioCC = $('#cboCEConvenioCC');
    const cboCEConvenioPeriodo = $('#cboCEConvenioPeriodo');
    const txtCEConvenioMonto = $('#txtCEConvenioMonto');
    const txtCEConvenioMontonuevo = $('#txtCEConvenioMontonuevo');
    const txtCEConvenioFechaoriginal = $('#txtCEConvenioFechaoriginal');
    const txtCEConvenioFechanueva = $('#txtCEConvenioFechanueva');
    const cboCEConvenioAutoriza = $('#cboCEConvenioAutoriza');
    const txtCEConvenioComentarios = $('#txtCEConvenioComentarios');
    const cboFiltroCC = $('#cboFiltroCC');
    const cboFiltroPeriodo = $('#cboFiltroPeriodo');
    //#endregion

    ControlEstimaciones = function () {
        (function init() {
            fncListeners();
            initTblConvenios();
            initTblAbonos();
            obtenerUrlPArams();
        })();

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            if (variables.cargarModal == 1) {
                mdlCEConvenio.modal("show");
                GetObjClientePorNombre(variables.clienteID, false, null);
                fncFillComboFactura(variables.clienteID, false, null, (e) => {
                    cboCEConvenioFactura.val(variables.facturaID);
                    cboCEConvenioFactura.trigger("change");
                });

                txtCEConvenioTitulo.text("CAPTURA");

                // cboCandidatosAprobados.val(variables.candidatoID);
                var clean_uri = location.protocol + "//" + location.host + location.pathname;
                window.history.replaceState({}, document.title, clean_uri);
            }
        }

        function getUrlParams(url) {
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

        function fncListeners() {

            txtCEConvenioNombrecliente.getAutocomplete(setClienteId, null, '/Facturacion/Prefacturacion/FillComboClienteNombre');
            cboCEConvenioCC.fillCombo('/Administrativo/Facultamiento/getComboCCEnkontrol', null, false, null);
            // cboCEConvenioPeriodo.fillCombo('FillComboPeriodos', null, false, null);
            // cboFiltroPeriodo.fillCombo('FillComboPeriodos', null, false, null);
            cboFiltroCC.fillCombo('FillComboCC', null, false, null);

            cboCEConvenioFactura.select2({ width: "100%" });


            btnFiltroBuscar.on("click", function () {
                fncGetConvenios();
            });

            btnFiltroNuevo.on("click", function () {
                cboCEConvenioFactura.prop("disabled", false);
                txtCEConvenioNombrecliente.prop("readonly", false);

                dtAbonos.clear();
                dtAbonos.draw();

                fncMdlDefault();
                btnCEConvenio.data("convenio", 0);
                txtCEConvenioTitulo.text("CAPTURA");
                mdlCEConvenio.modal("show");
            });

            btnCEConvenio.on("click", function () {
                fncCrearEditarConvenio();
            });

            cboCEConvenioFactura.on("change", function (event, getInfo) {
                if ($(this).val() != "" && $(this).val() != null) {
                    if (!getInfo) {
                        fncGetInfoFacturaById($(this).val(), false, null);

                    }
                }
            });

            txtCEConvenioMonto.on("change", function () {
                if ($(this).val() != "" && $(this).val() != null) {
                    $(this).val(maskNumero($(this).val()));
                }
            });

            txtCEConvenioMontonuevo.on("change", function () {
                if ($(this).val() != "" && $(this).val() != null) {
                    $(this).val(maskNumero($(this).val()));
                }
            });
        }

        //#region AUTOCOMPLETE
        function setClienteId(event, ui) {
            GetObjClientePorNombre(ui.item.id, false, null);
        }

        function GetObjClientePorNombre(id, esEdit, rowData) {
            // modalBusqCliente.block({ message: mensajes.PROCESANDO });
            if (id > 0) {
                $.ajax({
                    url: "/Facturacion/Prefacturacion/GetObjCliente",
                    type: "POST",
                    data: { id },
                    datatype: "json",
                    success: function (response) {

                        //CALLBACK BELIKON
                        if (esEdit) {
                            cboCEConvenioFactura.fillCombo("GetFacturasByCliente", { idCliente: response.numcte }, rowData.idFactura);
                        } else {
                            cboCEConvenioFactura.fillCombo("GetFacturasByCliente", { idCliente: response.numcte }, false);
                        }

                        txtCEConvenioNombrecliente.val(response.nombre);
                        txtCEConvenioNumcte.val(response.numcte);

                    },
                    error: function () {
                        // modalBusqCliente.unblock();
                    }
                });
            }
        }
        //#endregion

        function initTblConvenios() {
            dtConvenios = tblConvenios.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'nombreCliente', title: 'CLIENTE' },
                    { data: 'ccDescripcion', title: 'CC', render: function (data, type, row) { return "[" + row.cc + "] " + row.ccDescripcion } },
                    { data: 'idFactura', title: 'FACTURA' },
                    { data: 'monto', title: 'MONTO', render: function (data, type, row) { return maskNumero(data) } },
                    // { data: 'montoNuevo', title: 'MONTO NUEVO', render: function (data, type, row) { return maskNumero(data) } },
                    {
                        data: 'fechaOriginal', title: 'FECHA VENCIMIENTO',
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    {
                        render: (data, type, row, meta) => {
                            return `
                                <button title='Ver datos de convenio.' class="btn btn-primary actualizarConvenio btn-xs"><i class="far fa-edit"></i></button>&nbsp;
                                <button title='Eliminar convenio.' class="btn btn-danger eliminarConvenio btn-xs"><i class="far fa-trash-alt"></i></button>&nbsp;
                                `;

                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblConvenios.on('click', '.actualizarConvenio', function () {
                        let rowData = dtConvenios.row($(this).closest('tr')).data();

                        txtCEConvenioTitulo.text("ACTUALIZACION");
                        cboCEConvenioFactura.prop("disabled", true);
                        txtCEConvenioNombrecliente.prop("readonly", true);

                        txtCEConvenioComentarios.val(rowData.comentarios);

                        fncGetInfoFacturaById(rowData.idFactura, true, rowData, SetAutorizanteConvenios);
                        GetObjClientePorNombre(rowData.numcte, true, rowData);

                        dtAbonos.clear();
                        dtAbonos.rows.add(rowData.lstAbonos);
                        dtAbonos.draw();

                        btnCEConvenio.data("convenio", rowData.id);
                        mdlCEConvenio.modal("show");
                    });
                    tblConvenios.on('click', '.eliminarConvenio', function () {
                        let rowData = dtConvenios.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarConvenio(rowData.id));
                    });
                    tblConvenios.on('click', '.autorizarConvenio', function () {
                        let rowData = dtConvenios.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea autorizar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncActualizarEstatusConvenio(rowData.id, 2));
                    });
                    tblConvenios.on('click', '.rechazarConvenio', function () {
                        let rowData = dtConvenios.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea rechazar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncActualizarEstatusConvenio(rowData.id, 3));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { "width": "15%", "targets": 5 },

                ],
            });
        }

        function initTblAbonos() {
            dtAbonos = tblAbonos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                order: [[1, 'asc']],
                columns: [
                    //render: function (data, type, row) { }
                    {
                        data: 'abonoDet', title: 'Monto',
                        render: function (data, type, row) {
                            return maskNumero(data ?? 0);
                        }
                    },
                    {
                        data: 'fechaDet', title: 'Fecha',
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    // {
                    //     render: (data, type, row, meta) => {
                    //         return `<button title='Eliminar abono.' class="btn btn-danger eliminarAbono btn-xs"><i class="far fa-trash-alt"></i></button>&nbsp;`;
                    //     }
                    // },
                ],
                initComplete: function (settings, json) {
                    tblAbonos.on('click', '.classBtn', function () {
                        let rowData = dtAbonos.row($(this).closest('tr')).data();
                    });
                    tblAbonos.on('click', '.eliminarAbono', function () {
                        let rowData = dtAbonos.row($(this).closest('tr')).data();
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetConvenios() {
            let obj = {
                cc: cboFiltroCC.val(),
                periodo: cboFiltroPeriodo.val(),
                estatus: cboFiltroEstatus.val(),
            }

            axios.post("GetConvenios", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    if (items != null && items.length > 0) {
                        dtConvenios.clear();
                        dtConvenios.rows.add(items);
                        dtConvenios.draw();
                    } else {
                        dtConvenios.clear();
                        dtConvenios.draw();
                    }

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarConvenio() {
            let obj = {
                id: btnCEConvenio.data("convenio"),
                numcte: txtCEConvenioNumcte.val(),
                nombreCliente: txtCEConvenioNombrecliente.val(),
                cc: cboCEConvenioCC.val(),
                // periodo: cboCEConvenioPeriodo.val(),
                idFactura: cboCEConvenioFactura.val(),
                monto: unmaskNumero(txtCEConvenioMonto.val()),
                // montoNuevo: unmaskNumero(txtCEConvenioMontonuevo.val()),
                fechaOriginal: txtCEConvenioFechaoriginal.val(),
                // fechaNueva: txtCEConvenioFechanueva.val(),
                comentarios: txtCEConvenioComentarios.val(),
                esPagado: false,
                autoriza: cboCEConvenioAutoriza.val(),
            }
            axios.post("CrearEditarConvenios", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    if (obj.id > 0) {
                        Alert2Exito("Convenio actualizado con exito");
                        mdlCEConvenio.modal("hide");
                    } else {
                        Alert2Exito("Convenio creado con exito");
                        mdlCEConvenio.modal("hide");
                    }

                    fncGetConvenios();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarConvenio(idConvenio) {
            axios.post("EliminarConvenio", { idConvenio }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    fncGetConvenios();
                    Alert2Exito("Registro eliminiado con exito");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncActualizarEstatusConvenio(idConvenio, estatus) {
            axios.post("ActualizarEstatusConvenio", { idConvenio, estatus }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    if (estatus == 2) {
                        Alert2Exito("Convenio autorizado con exito");
                    } else {
                        Alert2Exito("Convenio rechazado con exito");
                    }
                    fncGetConvenios();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncMdlDefault() {
            txtCEConvenioNumcte.val("");
            txtCEConvenioNombrecliente.val("");
            cboCEConvenioCC.val("");
            cboCEConvenioCC.trigger("change");
            cboCEConvenioPeriodo.val("");
            cboCEConvenioPeriodo.trigger("change");
            cboCEConvenioFactura.val("");
            cboCEConvenioFactura.trigger("change");
            txtCEConvenioMonto.val("");
            txtCEConvenioMontonuevo.val("");
            txtCEConvenioFechaoriginal.val("");
            txtCEConvenioFechanueva.val("");
            txtCEConvenioComentarios.val("");
            cboCEConvenioAutoriza.val("");
            cboCEConvenioAutoriza.trigger("change");
        }

        function fncGetInfoFacturaById(idFactura, esEdit, rowData, callBackFactura) {
            axios.post("GetInfoFacturaById", { idFactura }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    cboCEConvenioCC.val(items.areaCuenta);
                    txtCEConvenioFechaoriginal.val(moment(items.fecha).format("YYYY-MM-DD")); //FECHA VENCIMIENTO
                    txtCEConvenioMonto.val(maskNumero(items.monto));

                    let asdf = cboCEConvenioAutoriza.fillCombo('GetAutorizantesCC', { cc: items.areaCuenta }, false, null);

                    if (esEdit) {
                        callBackFactura(esEdit, rowData, asdf);
                    }

                }
            }).catch(error => Alert2Error(error.message));
        }

        function SetAutorizanteConvenios(esEdit, rowData, success) {
            cboCEConvenioAutoriza.val(rowData.autoriza);
            cboCEConvenioAutoriza.trigger("change");

        }
    }

    $(document).ready(() => {
        CuentasPorCobrar.CuentasPorCobrar.ControlEstimaciones = new ControlEstimaciones();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();