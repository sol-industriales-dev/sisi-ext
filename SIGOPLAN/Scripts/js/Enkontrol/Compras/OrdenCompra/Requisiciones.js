(() => {
    $.namespace('Enkontrol.OrdenCompra.Requisiciones');
    Requisiciones = function () {

        //#region Selectores
        const multiSelectCC = $('#multiSelectCC');
        const multiSelectFamiliaInsumos = $('#multiSelectFamiliaInsumos');
        const multiSelectCompradores = $('#multiSelectCompradores');
        const btnBuscar = $('#btnBuscar');
        const tblRequisiciones = $('#tblRequisiciones');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const selectAreaCuenta = $('#selectAreaCuenta');
        const modalEnviarRequisicion = $('#modalEnviarRequisicion');
        const textareaCorreosEnviarRequisicion = $('#textareaCorreosEnviarRequisicion');
        const botonConfirmarEnvioCorreo = $('#botonConfirmarEnvioCorreo');
        const labelTituloModalEnviarRequisicion = $('#labelTituloModalEnviarRequisicion');

        const idEmpresa = $('#idEmpresa');
        const divFamiliaInsumo = $('#divFamiliaInsumo');
        const divAreaCuenta = $('#divAreaCuenta');

        const report = $("#report");
        //#endregion

        //#region CONST GENERAR LINK PROVEEDOR
        const cbo_CEProveedorLink_FiltroProveedor = $('#cbo_CEProveedorLink_FiltroProveedor');
        const btnNuevoProveedorLink = $('#btnNuevoProveedorLink');
        const cbo_CEProveedorLink_Proveedor = $('#cbo_CEProveedorLink_Proveedor');
        const txt_CEProveedorLink_NumRequisicion = $('#txt_CEProveedorLink_NumRequisicion');
        const btnCENuevoProveedorLinkCancelar = $('#btnCENuevoProveedorLinkCancelar');
        const btnCENuevoProveedorLink = $('#btnCENuevoProveedorLink');
        const mdlListadoLinks = $('#mdlListadoLinks');
        const tblCom_ProveedoresLinks = $('#tblCom_ProveedoresLinks');
        const divListadoLinks = $('#divListadoLinks');
        const btnCENuevoProveedorLinkCerrar = $('#btnCENuevoProveedorLinkCerrar');
        const divCEProveedorLink = $('#divCEProveedorLink');
        const mdlListadoCorreosProveedor = $('#mdlListadoCorreosProveedor');
        const txtCorreosProveedorLink = $('#txtCorreosProveedorLink');
        const btnEnviarCorreoLinkProveedor = $('#btnEnviarCorreoLinkProveedor');
        //#endregion

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioMes = new Date(new Date().getFullYear(), new Date().getMonth(), 1);

        (function init() {
            $('.select2').select2();

            $.fn.dataTable.moment('DD/MM/YYYY');

            initTableRequisiciones();
            OcultarMostrarCboFamiliaArea();
            inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioMes);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);

            // multiSelectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCc', null, false, null);
            multiSelectCC.fillCombo('/Enkontrol/Requisicion/FillComboCcAsigReq', null, false, null);
            multiSelectCC.find('option[value=""]').remove();
            // convertToMultiselect('#multiSelectCC');
            multiSelectFamiliaInsumos.fillCombo('/Enkontrol/OrdenCompra/FillComboFamiliasInsumos', null, false, 'Todos');
            convertToMultiselect('#multiSelectFamiliaInsumos');
            multiSelectCompradores.fillCombo('/Enkontrol/OrdenCompra/FillComboCompradores', null, false, 'Todos');
            convertToMultiselect('#multiSelectCompradores');
            selectAreaCuenta.fillCombo('/Enkontrol/OrdenCompra/FillComboAreaCuentaTodas', null, false, '--Todos--');

            btnBuscar.click(getRequisicionesValidadas);
            botonConfirmarEnvioCorreo.click(enviarRequisicion);

            // seleccionarTodosMultiselect('#multiSelectCC');
            seleccionarTodosMultiselect('#multiSelectFamiliaInsumos');
            // seleccionarTodosMultiselect('#multiSelectCompradores');

            //#region FUNCIONES GENERAR LINK
            initTblProveedorLink();

            btnNuevoProveedorLink.on("click", function () {
                cbo_CEProveedorLink_Proveedor.fillCombo('/Enkontrol/OrdenCompra/FillCboProveedoresGenerarLink', {}, false);
                cbo_CEProveedorLink_Proveedor.select2({ width: "100%" });
                txt_CEProveedorLink_NumRequisicion.val(mdlListadoLinks.attr("data-numero"));
                fncBorderDefault();

                fncMostrarCEProveedorLink();
            });

            btnCENuevoProveedorLinkCancelar.on("click", function () {
                fncMostrarListadoProveedorLink();
            });

            btnCENuevoProveedorLink.on("click", function () {
                fncCEProveedorLink();
            });

            btnEnviarCorreoLinkProveedor.on("click", function () {
                fncEnviarCorreoLinkProveedores();
            });
            //#endregion

            // btnBuscar.click();
        })();

        $(document).on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function initTableRequisiciones() {
            tblRequisiciones.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollY: '45vh',
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblRequisiciones.on('click', '.btn-generar-cuadro', function () {
                        const cc = $(this).attr('cc');
                        const numero = $(this).attr('numero');

                        if (cc && numero) {
                            const getUrl = window.location;
                            const baseUrl = getUrl.protocol + "//" + getUrl.host;
                            const urlGenerarCuadroComparativo = baseUrl + `/Enkontrol/OrdenCompra/CuadroComparativo?cc=${cc}&numero=${numero}`;

                            window.location.href = urlGenerarCuadroComparativo;
                        }
                    });

                    tblRequisiciones.on('click', '.btn-generar-oc', function () {
                        const cc = $(this).attr('cc');
                        const numero = $(this).attr('numero');

                        if (cc && numero) {
                            const getUrl = window.location;
                            const baseUrl = getUrl.protocol + "//" + getUrl.host;
                            const urlGenerarOrdenCompra = baseUrl + `/Enkontrol/OrdenCompra/Generar?cc=${cc}&numero=${numero}`;

                            window.location.href = urlGenerarOrdenCompra;
                        }
                    });

                    tblRequisiciones.on('click', '.botonEnviarRequisicion', function () {
                        let rowData = tblRequisiciones.DataTable().row($(this).closest('tr')).data();

                        labelTituloModalEnviarRequisicion.text(`Enviar Requisición [${rowData.cc}-${rowData.numero}]`)
                        textareaCorreosEnviarRequisicion.val('');
                        botonConfirmarEnvioCorreo.data().id = rowData.id;
                        botonConfirmarEnvioCorreo.data().cc = rowData.cc;
                        botonConfirmarEnvioCorreo.data().numero = rowData.numero;
                        modalEnviarRequisicion.modal('show');
                    });

                    tblRequisiciones.on("click", ".generarLink", function () {
                        let rowData = tblRequisiciones.DataTable().row($(this).closest('tr')).data();

                        mdlListadoLinks.attr("data-cc", rowData.cc);
                        mdlListadoLinks.attr("data-numero", rowData.numero);
                        fncMostrarListadoProveedorLink();
                        fncGetProveedoresLinks();
                        cbo_CEProveedorLink_FiltroProveedor.fillCombo('/Enkontrol/OrdenCompra/FillCboProveedoresGenerarLinkRegistrados', { cc: rowData.cc, numRequisicion: rowData.numero }, false);
                        cbo_CEProveedorLink_FiltroProveedor.select2({ width: "100%" });
                        mdlListadoLinks.modal("show");
                    });

                    tblRequisiciones.on('click', '.btn-imprimir', function () {
                        let rowData = tblRequisiciones.DataTable().row($(this).closest('tr')).data();

                        verReporteRequisicion(rowData.cc, rowData.numero, rowData.PERU_tipoRequisicion);
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'tipoRequisicionDesc', title: 'Tipo Requisición' },
                    { data: 'ccDesc', title: 'Centro de Costos' },
                    { data: 'numero', title: 'Número' },
                    { data: 'areaCuentaDesc', title: 'Área-Cuenta' },
                    {
                        title: 'Tipo Compra', render: function (data, type, row, meta) {
                            if (row.consigna) {
                                return 'CONSIGNA';
                            } else if (row.licitacion) {
                                return 'LICITACIÓN';
                            } else {
                                return 'DIRECTA';
                            }
                        }
                    },
                    { data: 'solicitoDesc', title: 'Solicitó' },
                    { data: 'compradorDesc', title: 'Comprador' },
                    { data: 'fecha', title: 'Fecha Captura' },
                    { data: 'fechaValidacionAlmacenString', title: 'Fecha Validación' },
                    { data: 'estatusVencido', title: 'Estatus' },
                    {
                        title: 'Generar Cuadro', visible: false, render: function (data, type, row, meta) {
                            return `
                                <button class="btn btn-xs btn-default btn-generar-cuadro" cc=${row.cc} numero=${row.numero}>
                                    <i class="fas fa-arrow-right"></i>
                                </button>`;
                        }
                    },
                    {
                        title: 'Generar O.C.', visible: false, render: function (data, type, row, meta) {
                            return `
                                <button class="btn btn-xs btn-warning btn-generar-oc" cc=${row.cc} numero=${row.numero}>
                                    <i class="fas fa-arrow-right"></i>
                                </button>`;
                        }
                    },
                    {
                        title: '', render: function (data, type, row, meta) {
                            if (idEmpresa.val() == 3 || idEmpresa.val() == 6) {
                                return `
                                <button title="Detalle" class="btn btn-xs btn-default btn-imprimir"><i class="fa fa-eye"></i></button>
                                <button title="Enviar Requisición" class="btn btn-xs btn-primary botonEnviarRequisicion"><i class="fas fa-envelope"></i></button>
                                `;
                            } else {
                                return `
                                <button title="Detalle" class="btn btn-xs btn-default btn-imprimir"><i class="fa fa-eye"></i></button>
                                <button title="Enviar Requisición" class="btn btn-xs btn-primary botonEnviarRequisicion"><i class="fas fa-envelope"></i></button>
                                <button title="Generar Link" class="btn btn-xs btn-default generarLink"><i class="fas fa-at"></i></button>
                                `;
                            }

                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '10%', targets: [12] },
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        targets: [7]
                    }
                ]
            });
        }


        function OcultarMostrarCboFamiliaArea() {
            if (idEmpresa.val() == 6) {
                divFamiliaInsumo.hide();
                // divAreaCuenta.hide();
                // tblRequisiciones.DataTable().column(3).visible(false);
            } else {
                divFamiliaInsumo.show();
                divAreaCuenta.show();
                tblRequisiciones.DataTable().column(3).visible(true);
            }

            if (idEmpresa.val() == 3) {
                divFamiliaInsumo.hide();
                // divAreaCuenta.hide();
                // tblRequisiciones.DataTable().column(3).visible(false);
            } else {
                divAreaCuenta.show();
                tblRequisiciones.DataTable().column(3).visible(true);
            }
        }

        function verReporteRequisicion(cc, numero, tipoRequisicion) {
            if (!tipoRequisicion) {
                tipoRequisicion = '';
            }
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", '/Reportes/Vista.aspx?idReporte=112' + '&cc=' + cc + '&numero=' + numero + '&PERU_tipoRequisicion=' + tipoRequisicion);
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
        }

        function getRequisicionesValidadas() {
            let listCC = getValoresMultiples('#multiSelectCC');
            let listFamiliasInsumos = getValoresMultiples('#multiSelectFamiliaInsumos');
            let listCompradores = getValoresMultiples('#multiSelectCompradores');
            let fechaInicio = inputFechaInicio.val();
            let fechaFin = inputFechaFin.val();
            let area = 0;
            let cuenta = 0;
            let noEconomico = '';

            if (idEmpresa.val() == 6 || idEmpresa.val() == 3) {
                if (selectAreaCuenta.val() != '--Todos--') {
                    noEconomico = selectAreaCuenta.find('option:selected').val();
                }

                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/OrdenCompra/GetRequisicionesValidadas', { listCC, listFamiliasInsumos, listCompradores, fechaInicio, fechaFin, noEconomico })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            AddRows(tblRequisiciones, response.data);
                            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                if (selectAreaCuenta.val() != '--Todos--') {
                    area = +selectAreaCuenta.find('option:selected').val();
                    cuenta = +selectAreaCuenta.find('option:selected').attr('data-prefijo');
                }

                if (listFamiliasInsumos.length > 0) {
                    $.blockUI({ message: 'Procesando...' });
                    $.post('/Enkontrol/OrdenCompra/GetRequisicionesValidadas', { listCC, listFamiliasInsumos, listCompradores, fechaInicio, fechaFin, area, cuenta })
                        .always($.unblockUI)
                        .then(response => {
                            if (response.success) {
                                AddRows(tblRequisiciones, response.data);
                                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                            } else {
                                AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                } else {
                    AlertaGeneral(`Alerta`, `Seleccione una Familia de Insumos.`);
                }
            }
        }

        function enviarRequisicion() {
            let listaCorreos = textareaCorreosEnviarRequisicion.val().replace(/[,\s+;\s+:\s+]+/g, ',').split(/[\;\n,\s+]/);

            if (!listaCorreos.some((x) => x == '')) {
                let cc = botonConfirmarEnvioCorreo.data().cc;
                let numero = botonConfirmarEnvioCorreo.data().numero;

                axios.post(`/Reportes/Vista.aspx?idReporte=${112}&cc=${cc}&numero=${numero}&inMemory=1`, { listaCorreos, enviarCorreo: true }).then(response => {
                    if (response.status == 200) {
                        Alert2Exito('Se han enviado los correos.');
                        modalEnviarRequisicion.modal('hide');
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                Alert2Warning('Debe ingresar correos válidos.');
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function agregarTooltip(elemento, mensaje) {
            $(elemento).attr('data-toggle', 'tooltip');
            $(elemento).attr('data-placement', 'top');

            if (mensaje != "") {
                $(elemento).attr('title', mensaje);
            }

            $('[data-toggle="tooltip"]').tooltip({
                position: {
                    my: "center bottom-20",
                    at: "center top+8",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
                            .addClass("arrow")
                            .addClass(feedback.vertical)
                            .addClass(feedback.horizontal)
                            .appendTo(this);
                    }
                }
            });
        }
        function quitarTooltip(elemento) {
            $(elemento).removeAttr('data-toggle');
            $(elemento).removeAttr('data-placement');
            $(elemento).removeAttr('title');
        }

        //#region GENERAR LINK
        function initTblProveedorLink() {
            dtProveedorLink = tblCom_ProveedoresLinks.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'proveedor', title: 'Proveedor' },
                    { data: 'numRequisicion', title: '# Req' },
                    { data: 'estatusRegistro', title: 'Estatus Link' },
                    {
                        data: 'link', title: 'Link', render: function (data, type, row, meta) {
                            if (row.envioCorreo != '') {
                                return data;
                            } else {
                                return '<label style="color: red;">ENVÍO DE CORREO PENDIENTE</label>';
                            }
                        }
                    },
                    { data: 'envioCorreo', title: 'Estatus correo' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEnviarCorreo = `<button class='btn btn-xs btn-info enviarCorreoProveedor' title='Enviar link por correo.'><i class="fas fa-envelope"></i></button>&nbsp;`;
                            // let btnEnvioCorreoExterno = `<button class='btn btn-xs btn-primary envioCorreoExterno' title='Indicar que el link se envio por correo externamente.'><i class="fas fa-external-link-alt"></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEnviarCorreo + btnEliminar;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblCom_ProveedoresLinks.on('click', '.enviarCorreoProveedor', function () {
                        let rowData = dtProveedorLink.row($(this).closest('tr')).data();
                        mdlListadoCorreosProveedor.attr("data-link", rowData.link);
                        mdlListadoCorreosProveedor.modal("show");
                    });

                    tblCom_ProveedoresLinks.on('click', '.eliminarRegistro', function () {
                        let rowData = dtProveedorLink.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarProveedorLink(rowData.id, rowData.cc, rowData.numero));
                    });

                    tblCom_ProveedoresLinks.on('click', '.envioCorreoExterno', function () {
                        let rowData = dtProveedorLink.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea indicar que el correo se envio externamente?', 'Confirmar', 'Cancelar', () => fncIndicarEnvioCorreoExternamente(rowData.cc, rowData.numero));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: '15%', targets: [5] }
                ],
            });
        }

        function fncGetProveedoresLinks() {
            let cc = mdlListadoLinks.attr("data-cc");
            let numRequisicion = +mdlListadoLinks.attr("data-numero");
            if (cc != "" && numRequisicion > 0) {
                let obj = new Object();
                obj.cc = cc;
                obj.idProveedor = cc;
                obj.numRequisicion = numRequisicion;
                axios.post("/Enkontrol/OrdenCompra/GetProveedoresLink", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtProveedorLink.clear();
                        dtProveedorLink.rows.add(response.data.lstProveedoresDTO);
                        dtProveedorLink.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (cc == "") { Alert2Warning("Es necesario indicar un centro de costo."); }
                if (numero <= 0) { Alert2Warning("Es necesario indicar número de requisición."); }
            }
        }

        function fncEliminarProveedorLink(id) {
            if (id > 0) {
                let obj = new Object();
                obj.id = id;
                axios.post("/Enkontrol/OrdenCompra/EliminarProveedorLink", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetProveedoresLinks();
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (id <= 0) { Alert2Warning("Ocurrió un error al eliminar el link del proveedor."); }
            }
        }

        function fncCEProveedorLink() {
            let obj = fncCEObjProveedorLink();
            if (obj != "") {
                axios.post("/Enkontrol/OrdenCompra/CEProveedorLink", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncMostrarListadoProveedorLink();
                        fncGetProveedoresLinks();

                        cbo_CEProveedorLink_FiltroProveedor.fillCombo('/Enkontrol/OrdenCompra/FillCboProveedoresGenerarLinkRegistrados', {
                            cc: mdlListadoLinks.attr("data-cc"), numRequisicion: mdlListadoLinks.attr("data-numero")
                        }, false);
                        cbo_CEProveedorLink_FiltroProveedor.select2({ width: "100%" });
                        btnCENuevoProveedorLinkCancelar.click();
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEObjProveedorLink() {
            fncBorderDefault();
            let strMensajeError = "";
            if (mdlListadoLinks.attr("data-cc") <= 0) { strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_CEProveedorLink_Proveedor.val() == "") { $("#select2-cbo_CEProveedorLink_Proveedor-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj.idProveedor = cbo_CEProveedorLink_Proveedor.val();
                obj.cc = mdlListadoLinks.attr("data-cc");
                obj.numRequisicion = txt_CEProveedorLink_NumRequisicion.val();
                return obj;
            }
        }

        function fncBorderDefault() {
            $("#select2-cbo_CEProveedorLink_Proveedor-container").css("border", "1px solid #CCC");
        }

        function fncMostrarCEProveedorLink() {
            divListadoLinks.css("display", "none");
            cbo_CEProveedorLink_FiltroProveedor.css("display", "none");
            btnCENuevoProveedorLinkCerrar.css("display", "none");

            divCEProveedorLink.css("display", "inline");
            btnCENuevoProveedorLink.css("display", "inline");
            btnCENuevoProveedorLinkCancelar.css("display", "inline");
        }

        function fncMostrarListadoProveedorLink() {
            divListadoLinks.css("display", "inline");
            cbo_CEProveedorLink_FiltroProveedor.css("display", "inline");
            btnCENuevoProveedorLinkCerrar.css("display", "inline");

            divCEProveedorLink.css("display", "none");
            btnCENuevoProveedorLink.css("display", "none");
            btnCENuevoProveedorLinkCancelar.css("display", "none");
        }

        function fncEnviarCorreoLinkProveedores() {
            let listaCorreos = txtCorreosProveedorLink.val().replace(/[,\s+;\s+:\s+]+/g, ',').split(/[\;\n,\s+]/);

            if (!listaCorreos.some((x) => x == '')) {
                let cc = mdlListadoLinks.attr("data-cc");
                let numero = mdlListadoLinks.attr("data-numero");
                let link = mdlListadoCorreosProveedor.attr("data-link");

                axios.post(`/Reportes/Vista.aspx?idReporte=${112}&cc=${cc}&numero=${numero}&link=${link}&inMemory=1`, { listaCorreos, enviarCorreo: true }).then(response => {
                    if (response.status == 200) {
                        Alert2Exito('Se han enviado los correos.');
                        modalEnviarRequisicion.modal('hide');
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                Alert2Warning('Debe ingresar correos válidos.');
            }
        }

        function fncIndicarEnvioCorreoExternamente() {
            let cc = mdlListadoLinks.attr("data-cc");
            let numRequisicion = +mdlListadoLinks.attr("data-numero");
            if (cc != "" && +numRequisicion > 0) {
                let obj = new Object();
                obj.cc = cc;
                obj.numRequisicion = numRequisicion;
                axios.post('IndicarEnvioCorreoExternamente', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetProveedoresLinks();
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error durante la petición.");
            }
        }
    }

    $(document).ready(() => Enkontrol.OrdenCompra.Requisiciones = new Requisiciones())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();