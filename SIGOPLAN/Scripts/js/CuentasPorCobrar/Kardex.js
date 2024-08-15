(() => {
    $.namespace('CuentasPorCobrar.CuentasPorCobrar.Kardex');

    //#region CONSTS
    const comboDivision = $('#comboDivision');
    const comboAC = $('#comboAC');
    const btnFiltroExportar = $('#btnFiltroExportar');
    const botonBuscar = $('#botonBuscar');
    const tblKardex = $('#tblKardex');
    const titleCliente = $('#titleCliente');

    let dtKardex;
    //#endregion

    //#region CONSTS MODAL DET
    const modalKardexDet = $('#modalKardexDet');
    const titleKardexCC = $('#titleKardexCC');
    const titleKardexCliente = $('#titleKardexCliente');
    const titleKardexFactura = $('#titleKardexFactura');

    const tblKardexDet = $('#tblKardexDet');

    let dtKardexDet;

    //#endregion

    let collapsedGroups = [];

    let valTm = [];
    valTm[1] = 'ESTIMACIONES POR CONTRATO';
    valTm[2] = 'CONSTRUCCION PESADA P/CONTRATO';
    valTm[3] = 'PAVIMENTACION POR CONTRATO';
    valTm[4] = 'AREA INDUSTRIAL POR CONTRATO';
    valTm[5] = 'GAS POR CONTRATO';
    valTm[6] = 'INFRA.  URBANA CONTRATO';
    valTm[7] = 'EDIFICACION INDUSTRIAL CONTRAT';
    valTm[10] = 'ANTICIPO DE OBRA';
    valTm[11] = 'ESTIMACION FORD F/CONTRATO';
    valTm[12] = 'CONSTRUCCION PESADA F/CONTRATO';
    valTm[13] = 'PAVIMENTACION F/CONTRATO';
    valTm[14] = 'AREA INDUSTRIAL F/CONTRATO';
    valTm[15] = 'GAS F/CONTRATO';
    valTm[16] = 'INFRA. URBANA F/CONTRATO';
    valTm[17] = 'EDIFICACION IND.Y COM F/CONTRA';
    valTm[18] = 'RENTA DE MAQUINARIA';
    valTm[19] = 'VENTA MAQUINARIA';
    valTm[20] = 'DEVOLUCION RETENCION DE OBRA';
    valTm[24] = 'SALDOS INICIALES';
    valTm[26] = 'NOTA DE CREDITO ESTIMACION';
    valTm[51] = 'COBRO DE ESTIMACIONES';
    valTm[76] = 'CANCELACION DE COBRO';

    let totalAcomul = 0;
    let tempFactura = "";

    Kardex = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            // initTblKardex();
            initTblKardexDet();

            // comboDivision.fillCombo('/CuentasPorCobrar/CuentasPorCobrar/fillComboDivision', null, null);
            // convertToMultiselect('#comboAC');

            botonBuscar.on("click", function () {
                if (comboDivision.val() != "" && comboAC.val() != "") {
                    totalAcomul = 0;
                    fncGetKardexDet();

                } else {
                    Alert2Warning("Favor de seleccionar todos los filtros");

                }
            });

            comboDivision.on("change", function () {

                if ($(this).val() != "") {
                    comboAC.fillCombo('cboObraEstimados', { divisionID: comboDivision.val() }, false, null);

                }
            });
        }

        function initTblKardex() {
            dtKardex = tblKardex.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                order: [[0, 'asc'], [1, 'asc']],
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'ccDesc', title: 'CC' },
                    { data: 'nombreCliente', title: 'CLIENTE' },
                    { data: 'factura', title: 'FACTURA' },
                    {
                        data: 'total', title: 'SALDO',
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        render: (data, type, row, meta) => {
                            return `<button class='btn btn-sm btn-primary verDet'><i class='fas fa-layer-group'></i></button>`;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblKardex.on('click', '.verDet', function () {
                        let rowData = dtKardex.row($(this).closest('tr')).data();

                        fncGetKardexDet(rowData.numcte, rowData.factura);
                        titleKardexCC.text(rowData.ccDesc);
                        titleKardexCliente.text(rowData.nombreCliente);
                        titleKardexFactura.text(rowData.factura);

                    });
                    tblKardex.on('click', '.classBtn', function () {
                        let rowData = dtKardex.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                    tblKardex.on('click', 'tr.group-start', function () {
                        var name = $(this).data('name');
                        collapsedGroups[name] = !collapsedGroups[name];
                        dtKardex.draw(false);
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                rowGroup: {
                    dataSrc: 'ccDesc',
                    startRender: function (rows, group) {
                        var collapsed = !!collapsedGroups[group];

                        rows.nodes().each(function (r) {
                            r.style.display = 'none';
                            if (!collapsed) {
                                r.style.display = '';
                            }
                        });

                        var sum = rows
                            .data()
                            .pluck('total')
                            .reduce(function (a, b) {
                                // console.log(b);
                                return a + b;
                            }, 0);

                        // return 'Average salary in ' + group + ': ' +
                        //     $.fn.dataTable.render.number(',', '.', 0, '$').display(sum);

                        // Add category name to the <tr>. NOTE: Hardcoded colspan
                        return $('<tr/>')
                            .append('<td colspan="8">' + group + ' <span class="pull-right"> SALDO: ' + (group == "DOLARES" ? " USD " : " MXN ") + maskNumero(sum) + '</span></td>')
                            .attr('data-name', group)
                            .toggleClass('collapsed', collapsed);
                    }
                },
                footerCallback: function (rows, _data, _start, _end, _display) {
                    var api = this.api();

                    var intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                    };

                    let total = api
                        .column(3)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    $(api.column(2).footer()).html('TOTAL');
                    $(api.column(3).footer()).html(maskNumero(total));
                },
            });
        }

        function fncGetKardex() {

            axios.post("GetKardex", { lstFiltroCC: getValoresMultiples("#comboAC") }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    dtKardex.clear();
                    dtKardex.rows.add(items);
                    dtKardex.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblKardexDet() {
            tempFactura = "";
            dtKardexDet = tblKardexDet.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                order: [10, 'asc'],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                ],
                columns: [
                    //render: function (data, type, row) { }
                    // { data: 'factura', title: 'FACTURA' },
                    { data: 'ccDesc', title: 'CC', visible: false },
                    {
                        data: 'nombreCliente', title: 'CLIENTE', visible: false,
                        render: function (data, type, row) {
                            return data ?? "";
                        }
                    },
                    { data: 'factura', title: 'FACTURA' },
                    {
                        data: 'tm', title: 'TIPO MOVIMIENTO', render: function (data, type, row) {
                            return valTm[data];
                        }
                    },
                    {
                        data: 'fecha', title: 'FECHA',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'concepto', title: 'CONCEPTO' },
                    {
                        data: 'monto', title: 'FACTURADO',
                        render: function (data, type, row) {
                            if (row.tm <= 24) {
                                return maskNumero(data);
                            } else {
                                return "";
                            }
                        }
                    },
                    {
                        data: 'monto', title: 'PAGADO',
                        render: function (data, type, row) {
                            if (row.tm >= 26) {
                                return maskNumero(data);
                            } else {
                                return "";
                            }
                        }

                    },
                    {
                        data: 'tipocambio', title: 'TIPO CAMBIO',
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'iva', title: 'I.V.A.',
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'moneda', title: 'MONEDA', visible: false,
                        render: function (data, type, row) {
                            return data == 1 ? "PESOS" : "DOLARES";
                        }
                    },
                    {
                        data: 'total', title: 'TOTAL',
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'total', title: 'ACOMUL',
                        render: function (data, type, row) {
                            totalAcomul += data;
                            return maskNumero(totalAcomul);
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblKardexDet.on('click', '.classBtn', function () {
                        let rowData = dtKardexDet.row($(this).closest('tr')).data();
                    });
                    tblKardexDet.on('click', '.classBtn', function () {
                        let rowData = dtKardexDet.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                    tblKardexDet.on('click', 'tr.group-start', function () {
                        var name = $(this).data('name');
                        collapsedGroups[name] = !collapsedGroups[name];
                        dtKardexDet.draw(false);
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                footerCallback: function (row, _data, _start, _end, _display) {
                    // var api = this.api();

                    // var intVal = function (i) {
                    //     return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                    // };

                    // let totalMonto = api.column(3).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    // let totalIva = api.column(5).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    // let total = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);

                    // $(api.column(2).footer()).html('TOTAL');
                    // $(api.column(3).footer()).html(maskNumero(totalMonto));
                    // $(api.column(5).footer()).html(maskNumero(totalIva));
                    // $(api.column(7).footer()).html(maskNumero(total));
                },
                rowGroup: {
                    dataSrc: function (row) {
                        return row.moneda == 1 ? "PESOS" : "DOLARES"
                    },
                    startRender: function (rows, group) {
                        var collapsed = !!collapsedGroups[group];

                        rows.nodes().each(function (r) {
                            r.style.display = 'none';
                            if (!collapsed) {
                                r.style.display = '';
                            }
                        });

                        var sum = rows
                            .data()
                            .pluck('total')
                            .reduce(function (a, b) {
                                // console.log(b);
                                return a + b;
                            }, 0);

                        // return 'Average salary in ' + group + ': ' +
                        //     $.fn.dataTable.render.number(',', '.', 0, '$').display(sum);

                        // Add category name to the <tr>. NOTE: Hardcoded colspan
                        return $('<tr/>')
                            .append('<td colspan="12">' + group + ' <span class="pull-right"> SALDO: ' + (group == "DOLARES" ? " USD " : " MXN ") + maskNumero(sum) + '</span></td>')
                            .attr('data-name', group)
                            .toggleClass('collapsed', collapsed);
                    }
                },
                createdRow: function (row, data, dataIndex) {

                    if (data.tm >= 26) {
                        $(row).find('td:eq(5)').css('color', 'red');
                    }

                    //Agregar break lines a la tabla
                    if (data.factura != tempFactura) {
                        $(row).find('td').addClass('tdBorde');
                    }

                    tempFactura = data.factura;
                },
            });
        }

        function fncGetKardexDet() {

            axios.post("GetKardexDet", { cc: comboAC.val() }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...

                    if (items.length > 0) {

                        titleCliente.text(items[0].nombreCliente ?? "");
                    }
                    dtKardexDet.clear();
                    dtKardexDet.rows.add(items);
                    dtKardexDet.draw();

                    // modalKardexDet.modal("show");
                }
            }).catch(error => Alert2Error(error.message));
        }
    }

    $(document).ready(() => {
        CuentasPorCobrar.CuentasPorCobrar.Kardex = new Kardex();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();