(() => {
    $.namespace('Enkontrol.Compras.Requisicion.Seguimiento');
    Seguimiento = function () {
        //#region Selectores
        const tblRequisiciones = $('#tblRequisiciones');
        const multiSelectCC = $('#multiSelectCC');
        const multiSelectTipoInsumo = $('#multiSelectTipoInsumo');
        const inputFechaInicial = $('#inputFechaInicial');
        const inputFechaFinal = $('#inputFechaFinal');
        const btnBuscar = $('#btnBuscar');
        const btnImprimir = $('#btnImprimir');
        const comboFiltroCompra = $('#comboFiltroCompra');
        const selectComprador = $("#selectComprador");
        const selRequisitores = $('#selRequisitores');
        const botonAuditoria = $('#botonAuditoria');
        const modalAuditoria = $('#modalAuditoria');
        const tablaRequisicionesNoAutorizadas = $('#tablaRequisicionesNoAutorizadas');
        const tablaComprasNoAutorizadas = $('#tablaComprasNoAutorizadas');
        const tablaComprasAutorizadasSinEntradas = $('#tablaComprasAutorizadasSinEntradas');
        const botonConfirmar = $('#botonConfirmar');
        const modalSurtido = $('#modalSurtido');
        const tablaSurtido = $('#tablaSurtido');
        const checkboxTodosCC = $('#checkboxTodosCC');
        const selectConsigna = $('#selectConsigna');
        const idEmpresa = $('#idEmpresa');
        const divInsumo = $('#divInsumo');
        const divConsigna = $('#divConsigna');
        const selectProveedor = $('#selectProveedor');
        const checkboxTodosProveedores = $('#checkboxTodosProveedores');

        report = $("#report");
        //#endregion

        (function init() {
            initTableRequisiciones();
            initTableRequisicionesNoAutorizadas();
            initTableComprasNoAutorizadas();
            initTableComprasAutorizadasSinEntradas();
            initTableSurtido();
            OcultarMostrarBotonAuditoriaInsumo();
            multiSelectCC.fillCombo('/Enkontrol/Requisicion/FillComboCcAsigReq', null, false, '');
            multiSelectCC.find('option[value=""]').remove();
            multiSelectCC.select2();
            multiSelectTipoInsumo.fillCombo('/Enkontrol/Requisicion/FillComboTipoInsumo', null, false, 'Todos');
            convertToMultiselect('#multiSelectTipoInsumo');
            selectProveedor.fillCombo('/Enkontrol/Requisicion/FillComboProveedoresReporteProcesoOC', null, false, '');
            selectProveedor.find('option[value=""]').remove();
            selectProveedor.select2();

            var date = new Date(), y = date.getFullYear(), m = date.getMonth();
            var firstDay = new Date(y, m, 1);
            var lastDay = new Date(y, m + 1, 0);

            inputFechaInicial.datepicker().datepicker('setDate', firstDay);
            inputFechaFinal.datepicker().datepicker('setDate', lastDay);

            btnImprimir.click(verReporte);
            botonAuditoria.click(cargarAuditoriaReqOC);
            botonConfirmar.click(confirmarAuditoriaReqOC);
            selectComprador.fillCombo('/Enkontrol/OrdenCompra/FillComboCompradores', false, null)
            selRequisitores.fillCombo('/Enkontrol/Requisicion/FillComboRequisitores', false, null)
            selRequisitores.select2();
        })();

        $(document).on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        btnBuscar.on('click', function () {
            let listaCC = getValoresMultiples('#multiSelectCC');
            let listaTipoInsumo = [];
            let fechaInicial = inputFechaInicial.val();
            let fechaFinal = inputFechaFinal.val();
            let estatus = 0;
            let comprador = selectComprador.val() == "" ? 0 : selectComprador.val();
            let requisitor = 0;
            let consigna = +selectConsigna.val();

            $.post('/Enkontrol/Requisicion/GetTiempoProcesoOC', { listaCC, listaTipoInsumo, fechaInicial, fechaFinal, estatus, comprador, requisitor, consigna, claveProveedor: getValoresMultiples('#selectProveedor') }).then(response => {
                if (response.success) {
                    AddRows(tblRequisiciones, response.data);
                } else {
                    AlertaGeneral(`Alerta`, `${response.message}`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        });

        checkboxTodosCC.on('click', function () {
            if (checkboxTodosCC.is(':checked')) {
                multiSelectCC.find('option').prop("selected", true);
                multiSelectCC.trigger("change");
            } else {
                multiSelectCC.find('option').prop("selected", false);
                multiSelectCC.trigger("change");
            }
        });

        checkboxTodosProveedores.on('click', function () {
            if (checkboxTodosProveedores.is(':checked')) {
                selectProveedor.find('option').prop("selected", true);
                selectProveedor.trigger("change");
            } else {
                selectProveedor.find('option').prop("selected", false);
                selectProveedor.trigger("change");
            }
        });

        function OcultarMostrarBotonAuditoriaInsumo() {
            if (idEmpresa.val() == 6) {
                botonAuditoria.hide();
                divInsumo.hide();
                divConsigna.hide();
            } else if (idEmpresa.val() == 3) {
                botonAuditoria.hide();
                divInsumo.show();
                divConsigna.show();
            } else {
                botonAuditoria.show();
                divInsumo.show();
                divConsigna.show();
            }
        }
        function initTableRequisiciones() {
            tblRequisiciones.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollY: '48vh',
                scrollCollapse: true,
                scrollX: true,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblRequisiciones.on('click', '.botonSurtido', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblRequisiciones.DataTable().row(row).data();
                        let cc = rowData.cc;
                        let req = rowData.numeroRequisicion;
                        let almacen = rowData.almacen;

                        axios.get('/Enkontrol/Requisicion/GetReqDetSalidasConsumo', { params: { cc, req, almacen } })
                            .catch(error => AlertaGeneral(error.message))
                            .then(response => {
                                let { success, data } = response.data;

                                if (success) {
                                    AddRows(tablaSurtido, data);

                                    modalSurtido.modal('show');
                                }
                            });
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'requisitorDesc', title: 'Requisitor' },
                    { data: 'requisicion', title: 'Número Requisición' },
                    {
                        title: 'Consigna', render: function (data, type, row, meta) {
                            return row.consigna ? `<h5 style="margin-top: 0px; margin-bottom: 0px;">&#10004;</h5>` : ``;
                        }
                    },
                    {
                        data: 'fechaAutorizaRe', title: 'Fecha Autorizacion',
                        createdCell: function (td, data, rowData, row, col) {
                            if (data == null) {
                                $(td).addClass('requisicionNoAutorizada');
                            }
                        }
                    },
                    { data: 'fechaEntregaCompras', title: 'Fecha Entrega Compras' },
                    { data: 'tipoRequisicion', title: 'Tipo Requisición' },
                    { data: 'economico', title: 'Económico' },
                    { data: 'compradorDesc', title: 'Comprador' },
                    { data: 'ordenCompra', title: 'Número Orden Compra' },
                    {
                        data: 'fechaCompra', title: 'Fecha Compra', createdCell: function (td, data, rowData, row, col) {
                            if (rowData.ordenCompraAutorizada == 'NO') {
                                $(td).addClass('compraNoAutorizada');
                            }
                        }
                    },
                    { data: 'fechaAutorizacionCompraDesc', title: 'Fecha Aut. Compra' },
                    { data: 'proveedorDesc', title: 'Proveedor' },
                    { data: 'colocadaFecha', title: 'Colocada' },
                    { data: 'tiempoEntregaDiasDesc', title: 'Tiempo de Entrega' },
                    //{ data: 'fechaEntrada', title: 'Fecha de Entrada' },
                    {
                        data: 'fechaEntrada', title: 'Fecha Entrada', createdCell: function (td, data, rowData, row, col) {
                            if (rowData.entregaVencida) {
                                $(td).addClass('compraVencida');
                            }
                        }
                    },
                    // { data: 'almacen', title: 'Almacén Surtido' },
                    { data: 'nivelReqAutReq', title: 'Req - Aut Req' },
                    { data: 'nivelAutReqOC', title: 'Aut Req - Elab OC' },
                    { data: 'nivelOCAutOC', title: 'OC - Aut OC' },
                    { data: 'nivelAutOCEnt', title: 'Aut OC - Ent' },
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
                        targets: [3, 4, 9, 12, 14]
                    },
                    { className: "dt-center dt-nowrap", "targets": "_all" }
                ],
                dom: 'Bfrtip',
                drawCallback: function (settings) {
                    $.unblockUI();
                },
                buttons: ['excel']//parametrosImpresion("Reporte de Minutas con Actividades Pendientes", "<center><h3>Reporte de Minutas con Actividades Pendientes <br/>del</h3></center>")
            });
        }

        function initTableRequisicionesNoAutorizadas() {
            tablaRequisicionesNoAutorizadas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                // scrollY: '48vh',
                // scrollCollapse: true,
                bInfo: false,
                columns: [
                    { data: 'cc', title: 'Centro de Costo' },
                    { data: 'numero', title: 'Número' },
                    { data: 'fechaString', title: 'Fecha Creación' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { orderable: false, targets: [0, 1, 2] }
                ]
            });
        }

        function initTableComprasNoAutorizadas() {
            tablaComprasNoAutorizadas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                // scrollY: '48vh',
                // scrollCollapse: true,
                bInfo: false,
                columns: [
                    { data: 'cc', title: 'Centro de Costo' },
                    { data: 'numero', title: 'Número' },
                    { data: 'fechaString', title: 'Fecha Creación' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { orderable: false, targets: [0, 1, 2] }
                ]
            });
        }

        function initTableComprasAutorizadasSinEntradas() {
            tablaComprasAutorizadasSinEntradas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                // scrollY: '48vh',
                // scrollCollapse: true,
                bInfo: false,
                columns: [
                    { data: 'cc', title: 'Centro de Costo' },
                    { data: 'numero', title: 'Número' },
                    { data: 'fechaString', title: 'Fecha Creación' },
                    { data: 'fechaAutorizaString', title: 'Fecha Autorización' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { orderable: false, targets: [0, 1, 2, 3] }
                ]
            });
        }

        function initTableSurtido() {
            tablaSurtido.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                initComplete: function (settings, json) {

                },
                createdRow: function (row, rowData) {
                    let tdAlmacenPropio = $(row).find('.tdAlmacenPropio');
                    let tdAlmacenExterno = $(row).find('.tdAlmacenExterno');
                    let tdOrdenCompra = $(row).find('.tdOrdenCompra');
                    let tdCantidadConsumida = $(row).find('.tdCantidadConsumida');

                    if (rowData.solicitadoAP > 0 && (rowData.solicitadoAP == rowData.cantidadAP)) {
                        tdAlmacenPropio.css('background-color', '#ccffcc');
                    }
                    if (rowData.solicitadoAP == 0 && (rowData.solicitadoAP == rowData.cantidadAP)) {
                        tdAlmacenPropio.css('background-color', 'white');
                    }
                    if (rowData.solicitadoAP > 0 && (rowData.cantidadAP > 0 && rowData.cantidadAP < rowData.solicitadoAP)) {
                        tdAlmacenPropio.css('background-color', '#ffc000');
                    }
                    if (rowData.solicitadoAP > 0 && (rowData.cantidadAP == 0)) {
                        tdAlmacenPropio.css('background-color', '#c00000');
                    }

                    if (rowData.solicitadoAE > 0 && (rowData.solicitadoAE == rowData.cantidadAE)) {
                        tdAlmacenExterno.css('background-color', '#ccffcc');
                    }
                    if (rowData.solicitadoAE == 0 && (rowData.solicitadoAE == rowData.cantidadAE)) {
                        tdAlmacenExterno.css('background-color', 'white');
                    }
                    if (rowData.solicitadoAE > 0 && (rowData.cantidadAE > 0 && rowData.cantidadAE < rowData.solicitadoAE)) {
                        tdAlmacenExterno.css('background-color', '#ffc000');
                    }
                    if (rowData.solicitadoAE > 0 && (rowData.cantidadAE == 0)) {
                        tdAlmacenExterno.css('background-color', '#c00000');
                    }

                    if (rowData.solicitadoOC > 0 && (rowData.solicitadoOC == rowData.cantidadOC)) {
                        tdOrdenCompra.css('background-color', '#ccffcc');
                    }
                    if (rowData.solicitadoOC == 0 && (rowData.solicitadoOC == rowData.cantidadOC)) {
                        tdOrdenCompra.css('background-color', 'white');
                    }
                    if (rowData.solicitadoOC > 0 && (rowData.cantidadOC > 0 && rowData.cantidadOC < rowData.solicitadoOC)) {
                        tdOrdenCompra.css('background-color', '#ffc000');
                    }
                    if (rowData.solicitadoOC > 0 && (rowData.cantidadOC == 0)) {
                        tdOrdenCompra.css('background-color', '#c00000');
                    }

                    if (rowData.consumidoCompleto) {
                        tdCantidadConsumida.css('background-color', '#ccffcc');
                    }
                },
                columns: [
                    { data: 'partidaRequisicion', title: 'Partida' },
                    { data: 'insumoDescripcion', title: 'Insumo' },
                    {
                        title: 'Solicitado', data: 'solicitado', render: function (data, type, row, meta) {
                            return formatValue(data);
                        }
                    },
                    {
                        title: 'Almacén Propio' + '\n' + '(Solicitado/Surtido)', render: function (data, type, row, meta) {
                            return formatValue(row.solicitadoAP) + " / " + formatValue(row.cantidadAP);
                        }, className: 'dt-center tdAlmacenPropio'
                    },
                    {
                        title: 'Almacén Externo' + '\n' + '(Solicitado/Surtido)', render: function (data, type, row, meta) {
                            return formatValue(row.solicitadoAE) + " / " + formatValue(row.cantidadAE);
                        }, className: 'dt-center tdAlmacenExterno'
                    },
                    {
                        title: 'Orden Compra' + '\n' + '(Solicitado/Surtido)', render: function (data, type, row, meta) {
                            return formatValue(row.solicitadoOC) + " / " + formatValue(row.cantidadOC);
                        }, className: 'dt-center tdOrdenCompra'
                    },
                    { data: 'cantidadConsumida', title: 'Cantidad Consumida', className: 'dt-center tdCantidadConsumida' },
                    {
                        title: 'Almacén Surtido', render: function (data, type, row, meta) {
                            return `${row.almacen} - ${row.almacenDescripcion}`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function verReporte() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", '/Reportes/Vista.aspx?idReporte=293');
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
        }

        function cargarAuditoriaReqOC() {
            $.post('/Enkontrol/OrdenCompra/GetAuditoriaRequisicionesComprasAfectadas').then(response => {
                if (response.success) {
                    AddRows(tablaRequisicionesNoAutorizadas, response.requisicionesNoAutorizadas);
                    AddRows(tablaComprasNoAutorizadas, response.comprasNoAutorizadas);
                    AddRows(tablaComprasAutorizadasSinEntradas, response.comprasAutorizadasSinEntradas);

                    modalAuditoria.modal('show');
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function confirmarAuditoriaReqOC() {
            $.post('/Enkontrol/OrdenCompra/AuditoriaEliminarReqOC').then(response => {
                if (response.success) {
                    modalAuditoria.modal('hide');
                    AlertaGeneral(`Alerta`, `Se ha actualizado la información.`);
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Enkontrol.Compras.Requisicion.Seguimiento = new Seguimiento())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...', baseZ: 20000 }))
        .ajaxStop($.unblockUI);
})();