(() => {
    $.namespace('CuentasPorCobrar.CuentasPorCobrar.CuentasCobrar');

    //#region CONSTS FILTROS

    const comboDivision = $('#comboDivision');
    const inputCorte = $('#inputCorte');
    const btnFiltroExportar = $('#btnFiltroExportar');
    const botonBuscar = $('#botonBuscar');
    const chkCETipoTabla = $('#chkCETipoTabla');
    //#endregion

    //#region CONSTS BODY
    const spanTotalSaldo = $('#spanTotalSaldo');
    const spanTotalPronostico = $('#spanTotalPronostico');
    const spanTotalClientes = $('#spanTotalClientes');
    const spanTotalCCs = $('#spanTotalCCs');
    const spanTotalFacturas = $('#spanTotalFacturas');

    const tblReporteCXC = $('#tblReporteCXC');

    let dtReporteCXC;
    //#endregion

    let collapsedGroups = [];
    let filtroGrupo = ["descDivision", "areaCuenta"];

    CuentasCobrar = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            inputCorte.datepicker().datepicker('setDate', new Date());

            comboDivision.fillCombo('/CuentasPorCobrar/CuentasPorCobrar/fillComboDivision', null, null);

            initTblReporteCXC();

            botonBuscar.on("click", function () {
                fncGetCXC();
            });

            btnFiltroExportar.on("click", function () {
                fncImprimirReporte();
            });

            chkCETipoTabla.on("change", function () {
                if ($(this).prop("checked")) {
                    filtroGrupo = ["descDivision", "areaCuenta"];
                    dtReporteCXC
                        .order([0, 'asc'], [3, 'asc'])
                        .draw();
                    dtReporteCXC.column(3).visible(false);
                    dtReporteCXC.column(2).visible(true);
                } else {
                    filtroGrupo = ["descDivision", "responsable"];
                    dtReporteCXC
                        .order([0, 'asc'], [2, 'asc'])
                        .draw();
                    dtReporteCXC.column(3).visible(true);
                    dtReporteCXC.column(2).visible(false);
                }

                dtReporteCXC.rowGroup().dataSrc(filtroGrupo);
                fncGetCXC();

            });

        }

        function initTblReporteCXC() {
            dtReporteCXC = tblReporteCXC.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                order: [[0, 'asc'], [3, 'asc']],
                columns: [
                    //render: function (data, type, row) { }
                    //render: function (data, type, row) { }
                    // {
                    //     data: 'idDivision', render: function (data, type, row) {
                    //         if (type === 'display') {
                    //             if (data) {
                    //                 return data;
                    //             }
                    //             else {
                    //                 return "";
                    //             }
                    //         }
                    //         else {
                    //             return data;
                    //         }
                    //     }
                    // },
                    { data: 'descDivision', title: 'DIVISION', visible: false },
                    { data: 'factura', title: 'FACTURA' },
                    { data: 'responsable', title: 'CLIENTE' },
                    { data: 'areaCuenta', title: 'CC', render: function (data, type, row) { return "[" + data + "] " + row.areaCuentaDesc; }, visible: false },
                    { data: 'monto', title: 'IMPORTE', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'montoPronosticado', title: 'PRONOSTICO', render: function (data, type, row) { return maskNumero(data); } },
                ],
                initComplete: function (settings, json) {
                    tblReporteCXC.on('click', '.classBtn', function () {
                        let rowData = dtReporteCXC.row($(this).closest('tr')).data();
                    });
                    tblReporteCXC.on('click', '.classBtn', function () {
                        let rowData = dtReporteCXC.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                    tblReporteCXC.on('click', 'tr.dtrg-start', function () {
                        var name = $(this).data('name');
                        collapsedGroups[name] = !collapsedGroups[name];
                        dtReporteCXC.draw(false);
                    });
                    tblReporteCXC.on('rowgroup-datasrc', function (e, dt, val) {
                        dtReporteCXC.order.fixed({ pre: [[val, 'asc']] }).draw();
                    });
                    // tblReporteCXC.on('rowgroup-datasrc', function (e, dt, val) {
                    //     dtReporteCXC.order.fixed({ pre: [[val, 'asc']] }).draw();
                    // });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                ],
                rowGroup: {
                    dataSrc: filtroGrupo,
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
                            .pluck('monto')
                            .reduce(function (a, b) {
                                // console.log(b);
                                return a + b;
                            }, 0);

                        var sumPronostico = rows
                            .data()
                            .pluck('montoPronosticado')
                            .reduce(function (a, b) {
                                // console.log(b);
                                return a + b;
                            }, 0);

                        // return 'Average salary in ' + group + ': ' +
                        //     $.fn.dataTable.render.number(',', '.', 0, '$').display(sum);

                        // Add category name to the <tr>. NOTE: Hardcoded colspan
                        return $('<tr/>')
                            .append('<td colspan="8">' + group + ' <span class="pull-right"> TOTAL: ' + maskNumero(sum) + ' &nbsp;&nbsp;&nbsp;PRONOSTICO: ' + maskNumero(sumPronostico) + '</span></td>')
                            .attr('data-name', group)
                            .toggleClass('collapsed', collapsed);
                    }
                },
                footerCallback: function (rows, _data, _start, _end, _display) {
                    var api = this.api();

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                    };

                    // Total over all pages
                    let total = api
                        .column(4)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    let totalPronostico = api
                        .column(5)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Total over this page
                    // pageTotal = api
                    //     .column(4, { page: 'current' })
                    //     .data()
                    //     .reduce(function (a, b) {
                    //         return intVal(a) + intVal(b);
                    //     }, 0);

                    // Update footer
                    // $(api.column(4).footer()).html('$' + pageTotal + ' ( $' + total + ' total)')

                    $(api.column(3).footer()).html('GRAN TOTAL');
                    $(api.column(2).footer()).html('GRAN TOTAL');
                    $(api.column(4).footer()).html(maskNumero(total));
                    $(api.column(5).footer()).html(maskNumero(totalPronostico));

                },
            });
        }

        function fncGetCXC() {
            axios.post("getCXCReporte", {
                fecha: inputCorte.val(),
                areaCuenta: [],
                // idDivision: comboDivision.val(),
            }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...

                    let itemsCorte = [];

                    for (const item of items) {
                        if (item.esCorte) {
                            itemsCorte.push(item);
                        }
                    }

                    dtReporteCXC.clear();
                    dtReporteCXC.rows.add(itemsCorte);
                    dtReporteCXC.draw();

                    // spanTotalSaldo.text(maskNumero(Number(response.data.totalImporte).toFixed(2)));
                    // spanTotalPronostico.text(maskNumero(Number(response.data.totalPronostico).toFixed(2)));
                    // spanTotalClientes.text(response.data.numClientes);
                    // spanTotalCCs.text(response.data.numCCs);
                    // spanTotalFacturas.text(itemsCorte.length);

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncImprimirReporte() {
            // console.log('imprimir')
            var path = `/Reportes/Vista.aspx?idReporte=281&fechaCorte=${moment(inputCorte.datepicker('getDate')).format("YYYY-MM-DD")}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
    }

    $(document).ready(() => {
        CuentasPorCobrar.CuentasPorCobrar.CuentasCobrar = new CuentasCobrar();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();