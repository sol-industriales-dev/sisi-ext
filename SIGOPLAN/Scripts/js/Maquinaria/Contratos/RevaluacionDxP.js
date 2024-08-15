(() => {
    $.namespace('Maquinaria.DocumentosPorPagar.RevaluacionDxP');
    RevaluacionDxP = function() {

        //#region variables
        let polizaGenerada;
        //#endregion

        //#region selectores
        const txtFechaFiltro = $('#txtFechaFiltro');
        const btnGenerarPoliza = $('#btnGenerarPoliza');
        const tblDiferencia = $('#tblDiferencia');

        const modalPoliza = $('#modalPoliza');
        const tblPolizas = $('#tblPolizas');
        const btnRegistrarPoliza = $('#btnRegistrarPoliza');
        //#endregion

        //#region eventos
        txtFechaFiltro.on('change', function() {
            getInfoRevaluacion();
        });

        btnGenerarPoliza.on('click', function() {
            addRows(tblPolizas, polizaGenerada);
            modalPoliza.modal('show');
        });

        modalPoliza.on('shown.bs.modal', function() {
            tblPolizas.DataTable().columns.adjust().draw();
        });

        btnRegistrarPoliza.on('click', function() {
            registrarPoliza();
        });
        //#endregion

        //#region tablas
        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function initTablas() {
            tblDiferencia.DataTable({
                order: [[0, 'asc']],
                ordering: true,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        text: 'Descargar en excel',
                        exportOptions: {
                            modifier: {
                                page: '_all'
                            }
                        }
                    },
                    {
                        extend: 'print',
                        text: 'Imprimir',
                        title: 'Cálculo de ajuste por diferencia cambiaria a documentos por pagar',
                        footer: true 
                    }
                ],

                columns: [
                    { data: 'proveedor', title: 'PROVEEDOR' },
                    { data: 'contrato', title: 'CONTRATO', className: 'text-center' },
                    { data: 'activo', title: 'NO. ECO./USUARIO' },
                    { data: 'tipoCambio', title: 'TIPO CAMBIO' },
                    { data: 'deudaCP', title: 'DEUDA USD' },
                    { data: 'valuacionCP', title: 'VALUACIÓN M.N.' },
                    { data: 'contabilidadCP', title: 'CONTABILIDAD' },
                    { data: 'diferenciaCP', title: 'DIFERENCIA' },
                    { data: 'deudaLP', title: 'DEUDA USD' },
                    { data: 'valuacionLP', title: 'VALUACION M.N.' },
                    { data: 'contabilidadLP', title: 'CONTABILIDAD' },
                    { data: 'diferenciaLP', title: 'DIFERENCIA' },
                    { data: 'gananciaPerdidaCambiaria', title: 'GANANCIA O PERDIDA CAMBIARIA' }
                ],

                columnDefs: [
                    {
                        targets: [4, 5, 6, 7, 8, 9, 10, 11, 12],
                        render: function(data, type, row) {
                            return maskNumero(data);
                        },
                        className: 'text-right'
                    },
                    {
                        targets: [3],
                        render: function(data, type, row) {
                            return maskNumeroXD(data, 4);
                        },
                        className: 'text-right'
                    }
                ],

                initComplete: function(settings, json) {},
                createdRow: function(row, data, dataIndex) {},
                drawCallback: function(settings) {
                    // let proveedor = '';

                    // tblDiferencia.DataTable().rows().every(function(rowIdx, tableLoop, rowLoop) {
                    //     if (proveedor == '') {
                    //         proveedor = this.data().proveedor;
                    //     }
                    //     else {
                    //         if (proveedor != this.data().proveedor) {
                    //             proveedor = this.data().proveedor;

                    //             const tr = $(this.node()).proveedor('tr');
                    //             let trTbl = tblPQs.DataTable().row(tr);

                    //             trTbl.child('').show();
                    //             trTbl.child().addClass('rowSepatador');
                    //         }
                    //     }
                    // });
                },
                headerCallback: function(thead, data, start, end, display) {
                    $(thead).children().addClass('dtHeader');
                },
                footerCallback: function(tfoot, data, start, end, display) {
                    let total = 0;
                    tblDiferencia.DataTable().columns().every(function(colIdx, tableLoop, colLoop) {
                        if (colIdx > 3) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            $(this.footer()).html(maskNumero(total));
                            total = 0;
                        }
                    });
                }
            });

            tblPolizas.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        text: 'Descargar en excel',
                        exportOptions: {
                            modifier: {
                                page: '_all'
                            }
                        }
                    },
                    {
                        extend: 'print',
                        text: 'Imprimir',
                        title: 'Póliza de revaluación a documentos por pagar'
                    }
                ],

                columns: [
                    { data: 'linea', title: 'Línea' },
                    { data: 'cta', title: 'Cuenta' },
                    { data: 'scta', title: 'Subcuenta' },
                    { data: 'sscta', title: 'Subsubcuenta' },
                    { data: 'digito', title: 'Digito' },
                    { data: 'cc', title: 'CC' },
                    { data: 'referencia', title: 'Referencia' },
                    { data: 'concepto', title: 'Concepto' },
                    { data: 'tm', title: 'Tipo movimiento' },
                    { data: null, title: 'Cargo' },
                    { data: null, title: 'Abono' }
                ],

                columnDefs: [
                    {
                        targets: [0, 1, 2, 3, 4, 5, 8],
                        className: 'text-center'
                    },
                    {
                        targets: [9],
                        className: 'text-right',
                        render: function(data, type, row) {
                            if (row.monto != 0 && (row.tm == 1 || row.tm == 3)) {
                                return maskNumero(row.monto);
                            }
                            else {
                                return maskNumero(0);
                            }
                        }
                    },
                    {
                        targets: [10],
                        className: 'text-right',
                        render: function(data, type, row) {
                            if (row.monto != 0 && (row.tm == 2 || row.tm == 4)) {
                                return maskNumero(row.monto);
                            }
                            else {
                                return maskNumero(0);
                            }
                        }
                    }
                ],
                
                initComplete: function(settings, json) {},
                createdRow: function(row, data, dataIndex) {},
                drawCallback: function(settings) {},
                headerCallback: function(thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },

                footerCallback: function(tfoot, data, start, end, display) {
                    let totalCargo = 0;
                    let totalAbono = 0;

                    data.forEach(function(element, index, array) {
                        totalCargo += element.tm == 1 || element.tm == 3 ? element.monto : 0;
                        totalAbono += element.tm == 2 || element.tm == 4 ? element.monto : 0;
                    });

                    $(tfoot).find('th').eq(0).removeClass('text-left');
                    $(tfoot).find('th').eq(0).removeClass('text-center');
                    $(tfoot).find('th').eq(0).addClass('text-right');
                    $(tfoot).find('th').eq(1).text(maskNumero(totalCargo));
                    $(tfoot).find('th').eq(2).text(maskNumero(totalAbono));
                }
            });
        }
        //#endregion

        //#region fechas
        function initInputsFechas() {
            txtFechaFiltro.datepicker().datepicker('setDate', new Date());
        }
        //#endregion

        //#region servidor
        function getInfoRevaluacion() {
            $.get('/Contratos/GetInfoRevaluacion',
            {
                fecha: moment(txtFechaFiltro.val(), 'DD/MM/YYYY').toISOString(true)
            }).then(response => {
                if (response.success) {
                    addRows(tblDiferencia, response.items);
                    polizaGenerada = response.poliza.movimientos;
                    console.log(polizaGenerada);
                    if (response.items.length > 0) {
                        btnGenerarPoliza.show();
                    }
                    else {
                        btnGenerarPoliza.hide();
                    }
                }
                else {
                    swal('Alerta!', response.message, 'warning');
                    btnGenerarPoliza.hide();
                    polizaGenerada = null;
                }
            }, error => {
                swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                btnGenerarPoliza.hide();
                polizaGenerada = null;
            });
        }

        function registrarPoliza() {
            $.post('/Contratos/RegistrarPolizaRevaluacion', {}).then(response => {
                if (response.success) {
                    txtFechaFiltro.trigger('change');
                    polizaGenerada = null;
                    modalPoliza.modal('hide');
                    swal('Confirmación', 'Se registró la póliza: ' + response.poliza, 'success');
                } else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }
        //#endregion

        (function init() {
            moment.locale('es');

            initInputsFechas();
            initTablas();
        })();
    }
    $(document).ready(() => Maquinaria.DocumentosPorPagar.RevaluacionDxP = new RevaluacionDxP())
    .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    .ajaxStop($.unblockUI);
})();