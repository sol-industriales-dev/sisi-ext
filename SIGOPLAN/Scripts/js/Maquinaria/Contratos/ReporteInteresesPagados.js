(() => {
    $.namespace('Contratos.ReporteInteresesPagados');
    ReporteInteresesPagados = function () {
        //#region Selectores
        const inputFechaCorte = $('#inputFechaCorte');
        const botonBuscar = $('#botonBuscar');
        const tablaIntereses = $('#tablaIntereses');
        const btnExcel = $('#btnExcel');
        //#endregion

        let dtIntereses;
        let hoy = new Date();

        (function init() {
            initTablaIntereses();

            botonBuscar.click(cargarReporteInteresesPagados);

            btnExcel.on('click', function() {
                location.href = '/Contratos/DescargarExcelInteresesPagados?fecha=' + moment(inputFechaCorte.val(), "DD/MM/YYYY").toISOString(true);
            });

            inputFechaCorte.datepicker().datepicker("setDate", hoy);
        })();

        function cargarReporteInteresesPagados() {
            let fecha = inputFechaCorte.val();

            axios.post('/Contratos/CargarReporteInteresesPagados/', { fecha })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaIntereses, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initTablaIntereses() {
            dtIntereses = tablaIntereses.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                scrollY: '45vh',
                scrollCollapse: true,

                rowGroup: {
                    startRender: null,
                    endRender: function ( rows, group ) {
                        var totalPagado = rows
                            .data()
                            .pluck('Pagado')
                            .reduce(function (a, b) { return a + b; }, 0);
                        var totalCP = rows
                            .data()
                            .pluck('CP')
                            .reduce(function (a, b) { return a + b; }, 0);
                        var totalLP = rows
                            .data()
                            .pluck('LP')
                            .reduce(function (a, b) { return a + b; }, 0);
                        let totalCPLP = totalCP + totalLP;
                        return $('<tr/>')
                            .append( '<td class="dt-head-center dt-body-center">' + group + '</td>' )
                            .append( '<td class="dt-head-center dt-body-center"></td>' )
                            .append( '<td class="dt-head-center dt-body-center"></td>' )
                            .append( '<td class="dt-head-center dt-body-center"></td>' )
                            .append( '<td class="dt-head-center dt-body-center"></td>' )
                            .append( '<td class="dt-head-center dt-body-center"></td>' )
                            .append( '<td class="dt-head-center dt-body-center"></td>' )
                            .append( '<td class="dt-head-center dt-body-center"></td>' )
                            .append( '<td class="dt-head-center dt-body-center">' + maskNumero(totalPagado) + '</td>' )
                            .append( '<td class="dt-head-center dt-body-center">' + maskNumero(totalCP) + '</td>' )
                            .append( '<td class="dt-head-center dt-body-center">' + maskNumero(totalLP) + '</td>' )
                            .append( '<td class="dt-head-center dt-body-center">' + maskNumero(totalCPLP) + '</td>' );
                    },
                    dataSrc: function(row) {
                        return row.nombre + ' ' +row.Moneda;
                      }
                },
                initComplete: function (settings, json) {

                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'nombre', title: 'Banco' },
                    { data: 'Moneda', title: 'Moneda' },
                    { data: 'Cta', title: 'Cta' },
                    { data: 'Scta', title: 'Scta' },
                    { data: 'Sscta', title: 'Sscta' },
                    { data: 'Contrato', title: 'Contrato' },
                    { data: 'Folio', title: 'Folio' },
                    {
                        data: 'tipoCambio', title: 'Tipo de Cambio', render: function (data, type, row, meta) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'Pagado', title: 'Pagado', render: function (data, type, row, meta) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'CP', title: 'CP', render: function (data, type, row, meta) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'LP', title: 'LP', render: function (data, type, row, meta) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: null, title: 'TOTAL', render: function (data, type, row, meta) {
                            return maskNumero(row.CP + row.LP);
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-head-center dt-body-center', targets: '_all' }
                ],

                drawCallback: function(settings) {
                    let total = 0;
                    let totalCPLP = 0;
                    tablaIntereses.DataTable().columns().every(function(colIdx, tableLoop, colLoop) {
                       if (colIdx > 7 && colIdx < 11) {
                           for (let x = 0; x < this.data().length; x++) {
                               total += this.data()[x];
                           }
                           $(this.footer()).html(maskNumero(total));

                           if (colIdx == 9 || colIdx == 10) {
                               totalCPLP += total;
                           }

                           total = 0;
                       } else if (colIdx == 11) {
                           $(this.footer()).html(maskNumero(totalCPLP));
                       }
                    });
                }
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Contratos.ReporteInteresesPagados = new ReporteInteresesPagados())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();