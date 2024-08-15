(() => {
    $.namespace('Enkontrol.Compras.OrdenCompra.PendienteSurtir');
    PendienteSurtir = function () {
        //#region Selectores
        const selectCCcompraPendiente = $("#selectCCcompraPendiente");
        const tblComprasPendientes = $('#tblComprasPendientes');
        const comboEstatus = $('#comboEstatus');
        const botonBuscar = $('#botonBuscar');
        const tblPartidas = $('#tblPartidas');
        const tblRetenciones = $('#tblRetenciones');
        const mdlRetenciones = $('#mdlRetenciones');
        const textAreaDescPartida = $('#textAreaDescPartida');
        const selectAlmacen = $('#selectAlmacen');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');

        //#region Panel Izquierdo
        const selectCC = $('#selectCC');
        const inputNumero = $('#inputNumero');
        const inputNumeroReq = $('#inputNumeroReq');
        const selectBoS = $('#selectBoS');
        const dtpFecha = $('#dtpFecha');
        const inputProvNum = $('#inputProvNum');
        const inputProvNom = $('#inputProvNom');
        const inputCompNum = $('#inputCompNum');
        const inputCompNom = $('#inputCompNom');
        const inputSolNum = $('#inputSolNum');
        const inputSolNom = $('#inputSolNom');
        const inputAutNum = $('#inputAutNum');
        const inputAutNom = $('#inputAutNom');
        const inputEmb = $('#inputEmb');
        const selectLab = $('#selectLab');
        const inputConFact = $('#inputConFact');
        const checkAutoRecep = $('#checkAutoRecep');
        const inputAlmNum = $('#inputAlmNum');
        const inputAlmNom = $('#inputAlmNom');
        const inputEmpNum = $('#inputEmpNum');
        const inputEmpNom = $('#inputEmpNom');
        //#endregion

        //#region Panel Derecho
        const selectTipoOC = $('#selectTipoOC');
        const selectMoneda = $('#selectMoneda');
        const inputTipoCambio = $('#inputTipoCambio');
        const inputSubTotal = $('#inputSubTotal');
        const inputIVAPorcentaje = $('#inputIVAPorcentaje');
        const inputIVANumero = $('#inputIVANumero');
        const inputRetencion = $('#inputRetencion');
        const inputTotal = $('#inputTotal');
        const inputTotalFinal = $('#inputTotalFinal');
        const btnRetenciones = $('#btnRetenciones');
        //#endregion
        //#endregion

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);
        const fechaActual = new Date();

        // const getCompraDetalle = (cc, num) => { return $.post('/Enkontrol/OrdenCompra/GetCompra', { cc: cc, num: num }) };
        const getCompraDetalle = (cc, num, PERU_tipoCompra) => { return $.post('/Enkontrol/OrdenCompra/GetCompra', { cc: cc, num: num, PERU_tipoCompra: PERU_tipoCompra }) };

        (function init() {
            $.fn.dataTable.moment('DD/MM/YYYY');
            $('.select2').select2();

            initForm();
            initTableComprasPendientes();
            initTablePartidas();
            initTableRetenciones();

            botonBuscar.click(buscarComprasPendientes);
            btnRetenciones.click(() => { mdlRetenciones.modal('show') });
        })();

        function initForm() {
            inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioAnio);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
            selectCCcompraPendiente.fillCombo('/Enkontrol/OrdenCompra/FillComboCcFiltroPorUsuario', null, false, 'Todos');
            convertToMultiselect('#selectCCcompraPendiente');
            selectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCc', null, false);
            comboEstatus.fillCombo('/Enkontrol/Requisicion/FillComboEstatusSurtidoRequisicion', null, false, 'Ambas');
            selectAlmacen.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, 'Todos');
            convertToMultiselect('#selectAlmacen');
        }

        function buscarComprasPendientes() {
            let strlistaCC = JSON.stringify(getValoresMultiples('#selectCCcompraPendiente'));
            let estatus = comboEstatus.val() === 'Ambas' ? 0 : comboEstatus.val();
            let listaAlmacenes = getValoresMultiples('#selectAlmacen');
            let fechaInicio = inputFechaInicio.val();
            let fechaFin = inputFechaFin.val();

            axios.post('/Enkontrol/Almacen/ObtenerComprasPendientes', { strlistaCC, estatus, listaAlmacenes, fechaInicio, fechaFin }).then(response => {
                let { success, data } = response.data;

                if (success) {
                    AddRows(tblComprasPendientes, data);
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
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
                scrollY: "48vh",
                scrollCollapse: true,
                bLengthChange: false,
                bInfo: false,
                dom: 'B',
                buttons: [{ extend: 'excelHtml5', exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6] }, className: 'btn btn-default', title: null }],
                initComplete: function (settings, json) {
                    tblComprasPendientes.on('click', '.btn-oc-detalle', function () {
                        let rowData = tblComprasPendientes.DataTable().row($(this).closest('tr')).data();

                        selectCC.val(rowData.cc);
                        inputNumero.val(rowData.numero);

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
                            } else {
                                AlertaGeneral('Alerta', 'Error al consultar la información.');
                            }
                        });

                        $('#mdlDetalleOC').modal('show');
                        recargarHeaders();
                    });
                },
                columns: [
                    // { data: 'ccDescripcion', title: 'Centro de Costo' },
                    {
                        render: function (data, type, row, meta) {
                            return row.cc + '-' + row.numero;
                        }, title: 'Compra'
                    },
                    { data: 'fecha', title: 'Fecha' },
                    { data: 'almacenLAB', title: 'LAB' },
                    { data: 'proveedorDesc', title: 'Proveedor' },
                    { data: 'compradorDesc', title: 'Comprador' },
                    {
                        title: 'Consigna', render: function (data, type, row, meta) {
                            return row.consigna ? '<h4 style="margin-top: 0px; margin-bottom: 0px;">&#10004;</h4>' : '';
                        }
                    },
                    { data: 'estatus', title: 'Estatus' },
                    // { data: 'estatusSurtido', title: 'Estatus' },
                    {
                        sortable: false,
                        render: (data, type, row, meta) =>
                            `<button id=${meta.row} class="btn btn-xs btn-primary btn-oc-detalle" cc=${row.cc} oc=${row.numero} almacen="${row.libre_abordo}">
                                <i class="fas fa-info-circle"></i> Detalle O.C.
                            </button>
                            <button class="btn btn-xs btn-warning surtir" cc=${row.cc} oc=${row.numero}>
                                Surtir <i class="fas fa-arrow-right"></i>
                            </button>`
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
                    { className: "dt-center", "targets": "_all" },
                    { width: '20%', targets: [7] }
                ],
                drawCallback: function (settings) {
                    $('button.surtir').unbind().click(e => {
                        const boton = $(e.currentTarget);
                        const cc = boton.attr('cc');
                        const oc = boton.attr('oc');
                        if (cc && oc) {
                            const getUrl = window.location;
                            const baseUrl = getUrl.protocol + "//" + getUrl.host;
                            const urlSurtido = baseUrl + `/Enkontrol/OrdenCompra/Surtido?cc=${cc}&oc=${oc}`;

                            window.location.href = urlSurtido;
                        }
                    });
                }
            });

            tblComprasPendientes.DataTable().buttons().container().appendTo($('#divBotonExcel'));
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
    }
    $(document).ready(() => Enkontrol.Compras.OrdenCompra.PendienteSurtir = new PendienteSurtir())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();