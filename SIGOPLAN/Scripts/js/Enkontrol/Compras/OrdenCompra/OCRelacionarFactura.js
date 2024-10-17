(function () {
    $.namespace('Enkontrol.Compras.OrdenCompra.PendienteSurtirCompra');
    PendienteSurtirCompra = function () {
        const selectCCcompraPendiente = $("#selectCCcompraPendiente");
        const selectProveedor = $('#selectProveedor');
        const tblComprasPendientes = $('#tblComprasPendientes');
        const mdlExistenciaDetalle = $('#mdlExistenciaDetalle');
        const labelModal = $('#labelModal');
        const comboEstatus = $('#comboEstatus');
        const inputFechaInicial = $('#inputFechaInicial');
        const inputFechaFinal = $('#inputFechaFinal');
        const botonBuscar = $('#botonBuscar');
        const selectFiltroCompradores = $('#selectFiltroCompradores');
        const selectAreaCuenta = $('#selectAreaCuenta');
        const tblEntradas = $('#tblEntradas');

        const tblPartidas = $('#tblPartidas');
        const tblRetenciones = $('#tblRetenciones');
        const mdlRetenciones = $('#mdlRetenciones');
        const textAreaDescPartida = $('#textAreaDescPartida');
        const mdlDetalleEntradas = $('#mdlDetalleEntradas');
        //#region Panel Izquierdo
        selectCC = $('#selectCC');
        inputNumero = $('#inputNumero');
        inputNumeroReq = $('#inputNumeroReq');
        selectBoS = $('#selectBoS');
        const dtpFecha = $('#dtpFecha');
        const inputProvNum = $('#inputProvNum');
        const inputProvNom = $('#inputProvNom');
        const inputCompNum = $('#inputCompNum');
        const inputCompNom = $('#inputCompNom');
        const inputSolNum = $('#inputSolNum');
        const inputSolNom = $('#inputSolNom');
        const inputAutNum = $('#inputAutNum');
        const inputAutNom = $('#inputAutNom');
        inputEmb = $('#inputEmb');
        selectLab = $('#selectLab');
        const inputConFact = $('#inputConFact');
        checkAutoRecep = $('#checkAutoRecep');
        inputAlmNum = $('#inputAlmNum');
        const inputAlmNom = $('#inputAlmNom');
        inputEmpNum = $('#inputEmpNum');
        const inputEmpNom = $('#inputEmpNom');
        const report = $("#report");
        //#endregion

        //#region Panel Derecho
        selectTipoOC = $('#selectTipoOC');
        selectMoneda = $('#selectMoneda');
        inputTipoCambio = $('#inputTipoCambio');
        const inputSubTotal = $('#inputSubTotal');
        inputIVAPorcentaje = $('#inputIVAPorcentaje');
        const inputIVANumero = $('#inputIVANumero');
        const inputRetencion = $('#inputRetencion');
        const inputTotal = $('#inputTotal');
        const inputTotalFinal = $('#inputTotalFinal');

        const btnRetenciones = $('#btnRetenciones');
        //#endregion

        function init() {
            initForm();

            $('.select2').select2();

            initTableComprasPendientes();
            initTablePartidas();
            initTableRetenciones();
            initTableEntradas();

            botonBuscar.click(() => {
                let centroCosto = selectCCcompraPendiente.val();
                let estatus = comboEstatus.val() == "Todas" ? 0 : parseFloat(comboEstatus.val());
                let proveedor = selectProveedor.val() == "" ? 0 : selectProveedor.val();
                let fechaInicial = inputFechaInicial.val();
                let fechaFinal = inputFechaFinal.val();
                let idAreaCuenta = selectAreaCuenta.val() != '--Todos--' ? selectAreaCuenta.val() + '-' + selectAreaCuenta.find('option:selected').attr('data-prefijo') : selectAreaCuenta.val();
                let idCompradorEK = selectFiltroCompradores.val() == "" ? 0 : +selectFiltroCompradores.val();

                axios.post('/Enkontrol/OrdenCompra/ObtenerComprasPendientes', { cc: centroCosto, estatus, proveedor, fechaInicial, fechaFinal, idAreaCuenta, idCompradorEK })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            AddRows(tblComprasPendientes, response.data.data);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            });

            btnRetenciones.click(() => { mdlRetenciones.modal('show') });

            inputFechaInicial.datepicker().datepicker('setDate', new Date(new Date().getFullYear(), 0, 1));
            inputFechaFinal.datepicker().datepicker('setDate', new Date(new Date().getFullYear(), 11, 31));
        }

        const getCompraDetalle = (cc, num, PERU_tipoOC) => { return $.post('/Enkontrol/OrdenCompra/GetCompra', { cc: cc, num: num, PERU_tipoOC: PERU_tipoOC }) };

        function initForm() {
            selectCCcompraPendiente.fillCombo('/Enkontrol/OrdenCompra/FillComboCc', null, false, null);
            selectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCc', null, false);
            comboEstatus.fillCombo('/Enkontrol/OrdenCompra/FillComboEstatusCompras', null, false, null);
            comboEstatus.find('option[value=""]').remove();
            selectProveedor.fillCombo('/Enkontrol/OrdenCompra/FillComboProveedores', null, false, null);
            selectFiltroCompradores.fillCombo('/Enkontrol/OrdenCompra/FillComboCompradores', null, false, null);
            selectAreaCuenta.fillCombo('/Enkontrol/OrdenCompra/FillComboAreaCuentaTodas', null, false, '--Todos--');
        }

        function initTableComprasPendientes() {
            tblComprasPendientes.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                aaSorting: [2, 'desc'],
                rowId: 'id',
                scrollY: "250px",
                scrollX: true,
                scrollCollapse: true,
                bLengthChange: false,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblComprasPendientes.on('click', '.btn-oc-detalle', function () {
                        let rowData = tblComprasPendientes.DataTable().row($(this).closest('tr')).data();

                        selectCC.val(rowData.cc);
                        inputNumero.val(rowData.numero);

                        $.blockUI({ message: 'Procesando...', baseZ: 9999999 });
                        getCompraDetalle(rowData.cc, rowData.numero, rowData.PERU_tipoCompra).done(response => {
                            if (response.success) {
                                if (response.info.cc != null) {
                                    llenarInformacion(response.info);

                                    if (response.partidas.length > 0) {
                                        inputNumeroReq.val(response.partidas[0].num_requisicion);
                                    }

                                    AddRows(tblPartidas, response.partidas);
                                    AddRows(tblRetenciones, response.retenciones);

                                    tblPartidas.find('tbody tr:eq(0) td:eq(0)').click()
                                } else {
                                    AlertaGeneral('Alerta', 'No se encontró información.');

                                    limpiarInformacion();

                                    limpiarTabla(tblPartidas);
                                    limpiarTabla(tblRetenciones);

                                    textAreaDescPartida.val('');
                                }
                                $.unblockUI();
                            } else {
                                $.unblockUI();
                                AlertaGeneral('Alerta', 'Error al consultar la información.');
                            }
                        });

                        $('#mdlDetalleOC').modal('show');
                        recargarHeaders();
                    });

                    tblComprasPendientes.on('click', '.btn-imprimir', function () {
                        let rowData = tblComprasPendientes.DataTable().row($(this).closest('tr')).data();

                        $.blockUI({ message: 'Procesando...' });
                        $.post('/Enkontrol/OrdenCompra/CheckEstatusOrdenCompraImpresa', { cc: rowData.cc, numero: rowData.numero })
                            .always($.unblockUI)
                            .then(response => {
                                if (response.success) {
                                    verReporteCompra(rowData.cc, rowData.numero);
                                } else {
                                    AlertaGeneral(`Alerta`, `${response.message}`);
                                }
                            }, error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            }
                            );
                    });

                    tblComprasPendientes.on('click', '.cancelar', function () {
                        let rowData = tblComprasPendientes.DataTable().row($(this).closest('tr')).data();

                        AlertaCancelarCompra(`Alerta`, `¿Desea cancelar la Compra #${rowData.numero} del Centro de Costo "${rowData.cc}"?`, rowData.cc, rowData.numero)
                    });

                    tblComprasPendientes.on('click', '.btnEntradas', function () {
                        let rowData = tblComprasPendientes.DataTable().row($(this).closest('tr')).data();

                        $.blockUI({ message: 'Procesando...' });
                        $.post('/Enkontrol/OrdenCompra/GetEntradas', { cc: rowData.cc, numero: rowData.numero })
                            .always($.unblockUI)
                            .then(response => {
                                if (response.success) {
                                    if (response.data.length > 0) {
                                        AddRows(tblEntradas, response.data);

                                        mdlDetalleEntradas.modal('show');
                                    } else {
                                        AlertaGeneral(`Alerta`, `No se encontró información.`);
                                    }
                                } else {
                                    AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                                }
                            }, error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            }
                            );
                    });
                },
                columns: [
                    {
                        title: 'Compra', render: function (data, type, row, meta) {
                            return row.cc + '-' + row.numero;
                        }
                    },
                    { data: 'fecha', title: 'Fecha' },
                    { data: 'almacenLAB', title: 'LAB' },
                    { data: 'estatusSurtido', title: 'Estatus' },
                    { data: 'nombreComprador', title: 'Comprador' },
                    { data: 'areaCuentaDesc', title: 'Área-Cuenta' },
                    {
                        data: 'proveedor', title: 'Proveedor', render: function (data, type, row, meta) {
                            return row.proveedor + ' - ' + row.proveedorDesc;
                        }
                    },
                    { data: 'fechaAutorizacionString', title: 'Fecha Autorización' },
                    { data: 'fechaEntregaString', title: 'Fecha Entrega' },
                    { data: 'st_impresa', title: 'Impresa' },
                    {
                        sortable: false,
                        render: (data, type, row, meta) =>
                            `<div><button id=${meta.row} class="btn btn-xs btn-primary btn-oc-detalle" cc=${row.cc} oc=${row.numero} almacen="${row.libre_abordo}">
                                <i class="fas fa-info-circle"></i> Detalle
                            </button>
                            <button class="btn btn-xs btn-default btn-imprimir">
                                <i class="fas fa-file"></i> Imprimir
                            </button>
                            ${row.flagCancelar ?
                                `<button class="btn btn-xs btn-danger cancelar" cc=${row.cc} oc=${row.numero}>
                                    <i class="fas fa-times"></i> Cancelar
                                </button>` : ``}` +
                            `${row.flagTieneEntrada ?
                                `<button class="btn btn-xs btn-default btnEntradas" cc=${row.cc} oc=${row.numero}>
                                    <i class="fas fa-asterisk"></i> Entradas
                                </button>` : ``}</div>`
                        ,
                        title: ''
                    }
                ],
                columnDefs: [
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        targets: [1]
                    },
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableEntradas() {
            tblEntradas.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bLengthChange: false,
                bInfo: false,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'almacenDesc', title: 'Almacén' },
                    { data: 'numero', title: 'Número' },
                    { data: 'fecha', title: 'Fecha' },
                ],
                columnDefs: [
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        targets: [2]
                    },
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function AlertaCancelarCompra(titulo, mensaje, cc, numero) {
            if (mensaje == null) {
                mensaje = "Error en el resultado de la petición, favor de intentar de nuevo.";
            }

            $("#dialogalertaGeneral").removeClass('hide');
            $("#txtComentarioAlerta").html(mensaje);

            var opt = {
                title: titulo,
                autoOpen: false,
                draggable: false,
                resizable: false,
                modal: true,
                maxWidth: 600,
                minWidth: 400,
                position: {
                    my: "center",
                    at: "center",
                    within: $(".RenderBody")
                },
                buttons: [
                    {
                        text: "Aceptar",
                        click: function () {
                            cancelarCompra(cc, numero)

                            $("#dialogalertaGeneral").addClass('hide');
                            $(this).dialog("close");
                        }
                    },
                    {
                        text: "Cancelar",
                        click: function () {
                            $("#dialogalertaGeneral").addClass('hide');
                            $(this).dialog("close");
                        }
                    }
                ]
            };

            var theDialog = $("#dialogalertaGeneral").dialog(opt);

            theDialog.dialog("open");
        }

        function cancelarCompra(cc, numero) {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/CancelarCompra', { cc: cc, numero: numero })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha cancelado la compra`);
                        cargarCompra();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al modificar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function recargarHeaders() {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        }

        function llenarInformacion(data) {
            //#region Panel Izquierdo
            selectBoS.val(data.bienes_servicios);
            dtpFecha.val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.fecha.substr(6)))));
            inputProvNum.val(data.proveedor);
            inputProvNom.val(data.proveedorNom);
            inputCompNum.val(data.comprador);
            inputCompNom.val(data.compradorNom);
            inputSolNum.val(data.solicito);
            inputSolNom.val(data.solicitoNom);
            inputAutNum.val(data.autorizo);
            inputAutNom.val(data.autorizoNom);
            inputEmb.val(data.embarquese);
            selectLab.val(data.libre_abordo);
            inputConFact.val(data.concepto_factura);

            if (data.bit_autorecepcion == 'S') {
                checkAutoRecep.prop("checked", true);
            } else {
                checkAutoRecep.prop("checked", false);
            }

            inputAlmNum.val(data.almacen_autorecepcion);
            inputAlmNom.val(data.almacenRecepNom);
            inputEmpNum.val(data.empleado_autorecepcion);
            inputEmpNom.val(data.empleadoRecepNom);
            //#endregion

            //#region Panel Derecho
            selectTipoOC.val(data.tipo_oc_req);
            selectMoneda.val(data.moneda);
            inputTipoCambio.val(data.tipo_cambio);
            inputSubTotal.val(maskNumero(data.sub_total));
            inputIVAPorcentaje.val(data.porcent_iva);
            inputIVANumero.val(maskNumero(data.iva));
            inputRetencion.val(maskNumero(data.rentencion_despues_iva));
            inputTotal.val(maskNumero(data.total));
            inputTotalFinal.val('');
            //#endregion
        }

        function limpiarInformacion() {
            //#region Panel Izquierdo
            selectBoS.val('');
            dtpFecha.val('');
            inputProvNum.val('');
            inputProvNom.val('');
            inputCompNum.val('');
            inputCompNom.val('');
            inputSolNum.val('');
            inputSolNom.val('');
            inputAutNum.val('');
            inputAutNom.val('');
            inputEmb.val('');
            selectLab.val('');
            inputConFact.val('');

            checkAutoRecep.prop("checked", false);

            inputAlmNum.val('');
            inputAlmNom.val('');
            inputEmpNum.val('');
            inputEmpNom.val('');
            //#endregion

            //#region Panel Derecho
            selectTipoOC.val('');
            selectMoneda.val('');
            inputTipoCambio.val('');
            inputSubTotal.val('');
            inputIVAPorcentaje.val('');
            inputIVANumero.val('');
            inputRetencion.val('');
            inputTotal.val('');
            inputTotalFinal.val('');
            //#endregion
        }

        function limpiarTabla(tbl) {
            dt = tbl.DataTable();
            dt.clear().draw();
        }

        function initTablePartidas() {
            tblPartidas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                aaSorting: [0, 'asc'],
                rowId: 'id',
                scrollY: "250px",
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tblPartidas.on('click', 'td', function () {
                        let rowData = tblPartidas.DataTable().row($(this).closest('tr')).data();

                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblPartidas.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");
                            textAreaDescPartida.val(rowData.partidaDescripcion);
                        } else {
                            textAreaDescPartida.val('');
                        }
                    });
                },
                columns: [
                    { data: 'partida', title: 'Partida' },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Descripción' },
                    { data: 'areaCuenta', title: 'Área Cuenta' },
                    { data: 'fecha_entrega', title: 'Fecha Entregar' },
                    { data: 'cantidad', title: 'Cantidad' },
                    {
                        data: 'precio', title: 'Precio', render: (data, type, row, meta) => {
                            var mon = selectMoneda.find('option[value="' + row.moneda + '"]').text().trim();
                            return maskNumero(data) + " " + mon;
                        }
                    },
                    { data: 'importe', title: 'Importe' }
                ],
                columnDefs: [
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        targets: [4]
                    },
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return maskNumero(data);
                            }
                        },
                        targets: [7]
                    },
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableRetenciones() {
            tblRetenciones.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                "language": dtDicEsp,
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                "scrollCollapse": true,
                'initComplete': function (settings, json) {

                },
                columns: [
                    { data: 'id_cpto', title: 'Id Cpto' },
                    { data: 'descRet', title: 'Desc Ret' },
                    {
                        data: 'cantidad', render: function (data, type, row, meta) {
                            let html = '<input id="inputRet_' + meta.row + '" class="form-control cantidad" style="text-align: right;" value="' + maskNumero(data).replace('$', '') + '" data-orden="' + row.orden + '" data-id_cpto="' + row.id_cpto + '" data-cantidad="' + row.cantidad + '" />'

                            return html;
                        }, title: 'Cantidad'
                    },
                    { data: 'porc_ret', title: 'Porc Ret' },
                    { data: 'importe', title: 'Importe' },
                    { data: 'facturado', title: 'Facturado' },
                    { data: 'retenido', title: 'Retenido' },
                    { data: 'tm_descto', title: 'Tm Descto' }
                ],
                columnDefs: [
                    {
                        "render": function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return maskNumero(data);
                            }
                        },
                        "targets": [4]
                    },
                    { "className": "dt-center", "targets": "_all" }
                ]
            });
        }

        function verReporteCompra(cc, numero) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", '/Reportes/Vista.aspx?idReporte=201' + '&cc=' + cc + '&numero=' + numero);
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
        }

        String.prototype.parseDate = function () {
            return new Date(parseInt(this.replace('/Date(', '')));
        }
        Date.prototype.parseDate = function () {
            return this;
        }
        Date.prototype.addDays = function (days) {
            var date = new Date(this.valueOf());
            date.setDate(date.getDate() + days);
            return date;
        }
        $.fn.commasFormat = function () {
            this.each(function (i) {
                $(this).change(function (e) {
                    if (isNaN(parseFloat(this.value))) return;
                    this.value = parseFloat(this.value).toFixed(6);
                });
            });
            return this;
        }

        init();
    }
    $(document).ready(function () {
        Enkontrol.Compras.OrdenCompra.PendienteSurtirCompra = new PendienteSurtirCompra();
    });
})();