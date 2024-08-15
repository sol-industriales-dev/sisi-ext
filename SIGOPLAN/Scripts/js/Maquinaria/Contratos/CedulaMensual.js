(() => {
    $.namespace('Maquinaria.DocumentosPorPagar.CedulaMensual');
    
    CedulaMensual = function () {
        moment.locale('es');

        //#region Variables
        const version = 1;

        let fechaCorte = new Date();
        let detalleCortoPlazo = null;
        let detalleLargoPlazo = null;
        //#endregion
        
        //#region Selectores
        const fechaTitulo = $('#fechaTitulo');
        const txtFechaCorte = $('#txtFechaCorte');
        const btnFiltro = $('#btnFiltro');
        const btnDetalle = $('#btnDetalle');
        const btnDetallePQ = $('#btnDetallePQ');
        const tblCedula = $('#tblCedula');
        const tblCedulaPQ = $('#tblCedulaPQ');
        const modaldetalle = $('#modaldetalle');
        const modalDetalleTitulo = $('#modalDetalleTitulo');
        const cambiarTablaDetalle = $('#cambiarTablaDetalle');
        const tblDetalle = $('#tblDetalle');
        //#endregion

        //#region Eventos
        txtFechaCorte.on('change', function() {
            if (moment($(this).val(), 'DD/MM/YYYY', true).isValid()) {
                if (moment($(this).val(), 'DD/MM/YYYY').format('YYYY') >= 2021) {
                    fechaCorte = new Date(moment($(this).val(), 'DD/MM/YYYY').format('YYYY'), moment($(this).val(), 'DD/MM/YYYY').format('MM') - 1, moment($(this).val(), 'DD/MM/YYYY').format('DD'));
                }
                else {
                    fechaCorte = new Date(2021, 0, 31);
                    txtFechaCorte.datepicker('setDate', fechaCorte);
                }

                btnFiltro.attr('disabled', false);
                setFechaTitulo();
            }
            else {
                swal("Alerta!", "Favor de ingresar una fecha valida", "warning");
                
                btnFiltro.attr('disabled', true);
            }
        });

        btnFiltro.on('click', function() {
            getCedula();
        });

        btnDetalle.on('click', function() {
            modalDetalleTitulo.text('DETALLE CONTRATOS A CORTO PLAZO');
            
            AddRows(tblDetalle, detalleCortoPlazo);
            
            cambiarTablaDetalle.val(1);

            modaldetalle.modal('show');
        });

        btnDetallePQ.on('click', function() {
            // modalDetalleTitulo.text('DETALLE CONTRATOS A CORTO PLAZO');
            
            // AddRows(tblDetalle, detalleCortoPlazo);
            
            // cambiarTablaDetalle.val(1);

            // modaldetalle.modal('show');
        });

        cambiarTablaDetalle.on('click', function() {
            let numero = cambiarTablaDetalle.val();
            numero++;

            switch(numero) {
                case 1:
                    modalDetalleTitulo.text('DETALLE CONTRATOS A CORTO PLAZO');
                    $(this).text('Largo plazo');
                    AddRows(tblDetalle, detalleCortoPlazo);
                    break;
                case 2:
                    modalDetalleTitulo.text('DETALLE CONTRATOS A LARGO PLAZO');
                    $(this).text('Corto plazo');
                    AddRows(tblDetalle, detalleLargoPlazo);
                    break;
                default:
                    numero = 1;
                    modalDetalleTitulo.text('DETALLE CONTRATOS A CORTO PLAZO');
                    $(this).text('Largo plazo');
                    AddRows(tblDetalle, detalleCortoPlazo);
                    break;
            }

            cambiarTablaDetalle.val(numero);
        });

        modaldetalle.on('shown.bs.modal', function () {
            tblDetalle.DataTable().columns.adjust().draw();
        });
        //#endregion

        (function init() {
            getVersion();
            initDatepicker();
            setFechaTitulo();
            initTblCedula();
            initTblCedulaPQ();
            initTblDetalle();
        })();

        //#region Inicializadores
        //#region Fechas
        function initDatepicker() {
            fechaCorte = fechaCorte.getFullYear >= 2021 ? fechaCorte : new Date(2021, 0, 31);

            txtFechaCorte.datepicker({
                dateFormat: 'dd/mm/yy',
                changeYear: true,
                changeMonth: true,
                showButtonPanel: true,
                yearRange: '2021:c'
            }).datepicker('setDate', fechaCorte);
        }
        //#endregion

        //#region Tablas
        function initTblCedula() {
            tblCedula.DataTable({
                order: [[0, 'asc']],
                ordering: true,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: false,
                //scrollY: '45vh',
                scrollCollapse: false,
                //lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],

                columns: [
                    { data: 'financiera', title: 'Financiera' },
                    { data: 'nacionalCortoPlazo', title: 'Nacional', className: 'text-right' },
                    { data: 'dolaresCortoPlazo', title: 'Dolares', className: 'text-right' },
                    { data: 'contabilidadCortoPlazo', title: 'Contabilidad', className: 'text-right' },
                    { data: 'sigoplanCortoPlazo', title: 'SIGOPLAN', className: 'text-right' },
                    { data: 'diferenciaCortoPlazo', title: 'Diferencia', className: 'text-right' },
                    { data: 'nacionalLargoPlazo', title: 'Nacional', className: 'text-right' },
                    { data: 'dolaresLargoPlazo', title: 'Dolares', className: 'text-right' },
                    { data: 'contabilidadLargoPlazo', title: 'Contabilidad', className: 'text-right' },
                    { data: 'sigoplanLargoPlazo', title: 'SIGOPLAN', className: 'text-right' },
                    { data: 'diferenciaLargoPlazo', title: 'Diferencia', className: 'text-right' }
                ],

                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    },
                    {
                        targets: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {},
                drawCallback: function (settings) {},
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).parent().children().addClass('bg-table-header');
                    $(thead).children().addClass('text-center');
                },
                footerCallback: function (tfoot, data, start, end, display) {
                    let totalNacionalCortoPlazo = 0;
                    let totalDolaresCortoPlazo = 0;
                    let totalContabilidadCortoPlazo = 0;
                    let totalSigoplanCortoPlazo = 0;
                    let totalDiferenciaCortoPlazo = 0;

                    let totalNacionalLargoPlazo = 0;
                    let totalDolaresLargoPlazo = 0;
                    let totalContabilidadLargoPlazo = 0;
                    let totalSigoplanLargoPlazo = 0;
                    let totalDiferenciaLargoPlazo = 0;

                    data.forEach(function (element, index, array) {
                        totalNacionalCortoPlazo += element.nacionalCortoPlazo;
                        totalDolaresCortoPlazo += element.dolaresCortoPlazo;
                        totalContabilidadCortoPlazo += element.contabilidadCortoPlazo;
                        totalSigoplanCortoPlazo += element.sigoplanCortoPlazo;
                        totalDiferenciaCortoPlazo += element.diferenciaCortoPlazo;

                        totalNacionalLargoPlazo += element.nacionalLargoPlazo;
                        totalDolaresLargoPlazo += element.dolaresLargoPlazo;
                        totalContabilidadLargoPlazo += element.contabilidadLargoPlazo;
                        totalSigoplanLargoPlazo += element.sigoplanLargoPlazo;
                        totalDiferenciaLargoPlazo += element.diferenciaLargoPlazo;
                    });

                    $(tfoot).find('th').eq(1).text(maskNumero(totalNacionalCortoPlazo));
                    $(tfoot).find('th').eq(2).text(maskNumero(totalDolaresCortoPlazo));
                    $(tfoot).find('th').eq(3).text(maskNumero(totalContabilidadCortoPlazo));
                    $(tfoot).find('th').eq(4).text(maskNumero(totalSigoplanCortoPlazo));
                    $(tfoot).find('th').eq(5).text(maskNumero(totalDiferenciaCortoPlazo));
                    $(tfoot).find('th').eq(6).text(maskNumero(totalNacionalLargoPlazo));
                    $(tfoot).find('th').eq(7).text(maskNumero(totalDolaresLargoPlazo));
                    $(tfoot).find('th').eq(8).text(maskNumero(totalContabilidadLargoPlazo));
                    $(tfoot).find('th').eq(9).text(maskNumero(totalSigoplanLargoPlazo));
                    $(tfoot).find('th').eq(10).text(maskNumero(totalDiferenciaLargoPlazo));
                }
            });
        }
        function initTblDetalle() {
            tblDetalle.DataTable({
                order: [[0, 'asc']],
                ordering: true,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: false,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],

                columns: [
                    { data: 'cuenta', title: 'Cuenta', className: 'text-center' },
                    { data: 'scta', title: 'Subcuenta', className: 'text-center' },
                    { data: 'sscta', title: 'Subsubcuenta', className: 'text-center' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'saldoInicial', title: 'Saldo inicial', className: 'text-right' },
                    { data: 'cargos', title: 'Cargos', className: 'text-right' },
                    { data: 'abonos', title: 'Abonos', className: 'text-right' },
                    { data: 'saldoActual', title: 'Saldo actual', className: 'text-right' },
                    { data: 'saldoActualSigoplan', title: 'SIGOPLAN', className: 'text-right' },
                    { data: 'diferencia', title: 'Diferencia', className: 'text-right' },
                    { data: 'interesesPagados', title: 'Intereses pagados', visible: false },
                    { data: 'interesesCP', title: 'Intereses CP', visible: false },
                    { data: 'interesesLP', title: 'Intereses LP', visible: false }
                ],

                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    },
                    {
                        targets: [4, 5, 6, 7, 8, 9, 10, 11, 12],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {},
                drawCallback: function (settings) {},
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).addClass('bg-table-header');
                    $(thead).children().addClass('text-center');
                },
                footerCallback: function (tfoot, data, start, end, display) {
                    let totalSaldoInicial = 0;
                    let totalCargos = 0;
                    let totalAbonos = 0;
                    let totalSaldoActual = 0;
                    let totalSaldoActualSigoplan = 0;
                    let totalDiferencia = 0;

                    data.forEach(function (element, index, array) {
                        totalSaldoInicial += element.saldoInicial;
                        totalCargos += element.cargos;
                        totalAbonos += element.abonos;
                        totalSaldoActual += element.saldoActual;
                        totalSaldoActualSigoplan += element.saldoActualSigoplan;
                        totalDiferencia += element.diferencia;
                    });

                    $(tfoot).find('th').eq(1).text(maskNumero(totalSaldoInicial));
                    $(tfoot).find('th').eq(2).text(maskNumero(totalCargos));
                    $(tfoot).find('th').eq(3).text(maskNumero(totalAbonos));
                    $(tfoot).find('th').eq(4).text(maskNumero(totalSaldoActual));
                    $(tfoot).find('th').eq(5).text(maskNumero(totalSaldoActualSigoplan));
                    $(tfoot).find('th').eq(6).text(maskNumero(totalDiferencia));
                }
            });
        }

        function initTblCedulaPQ() {
            tblCedulaPQ.DataTable({
                order: [[0, 'asc']],
                ordering: true,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: false,
                //scrollY: '45vh',
                scrollCollapse: false,
                //lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],

                columns: [
                    { data: 'financiera', title: 'Financiera' },
                    { data: 'nacionalCortoPlazo', title: 'Nacional', className: 'text-right' },
                    { data: 'dolaresCortoPlazo', title: 'Dolares', className: 'text-right' },
                    { data: 'contabilidadCortoPlazo', title: 'Contabilidad', className: 'text-right' },
                    { data: 'sigoplanCortoPlazo', title: 'SIGOPLAN', className: 'text-right' },
                    { data: 'diferenciaCortoPlazo', title: 'Diferencia', className: 'text-right' }
                ],

                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    },
                    {
                        targets: [1],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [3],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [4],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [2],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {},
                drawCallback: function (settings) {},
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).parent().children().addClass('bg-table-header');
                    $(thead).children().addClass('text-center');
                },
                footerCallback: function (tfoot, data, start, end, display) {
                    let totalNacionalCortoPlazo = 0;
                    let totalDolaresCortoPlazo = 0;
                    let totalContabilidadCortoPlazo = 0;
                    let totalSigoplanCortoPlazo = 0;
                    let totalDiferenciaCortoPlazo = 0;

                    data.forEach(function (element, index, array) {
                        totalNacionalCortoPlazo += element.nacionalCortoPlazo;
                        totalDolaresCortoPlazo += element.dolaresCortoPlazo;
                        totalContabilidadCortoPlazo += element.contabilidadCortoPlazo;
                        totalSigoplanCortoPlazo += element.sigoplanCortoPlazo;
                        totalDiferenciaCortoPlazo += element.diferenciaCortoPlazo;
                    });

                    $(tfoot).find('th').eq(1).text(maskNumero(totalNacionalCortoPlazo));
                    $(tfoot).find('th').eq(2).text(maskNumero(totalDolaresCortoPlazo));
                    $(tfoot).find('th').eq(3).text(maskNumero(totalContabilidadCortoPlazo));
                    $(tfoot).find('th').eq(4).text(maskNumero(totalSigoplanCortoPlazo));
                    $(tfoot).find('th').eq(5).text(maskNumero(totalDiferenciaCortoPlazo));
                }
            });
        }
        //#endregion
        //#endregion

        //#region Funciones generales
        function setFechaTitulo() {
            fechaTitulo.text(moment(fechaCorte).format('MMMM').toUpperCase() + ' DE ' + fechaCorte.getFullYear());
        }

        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }
        //#endregion

        //#region Servidor
        function getVersion() {
            $.get('/Contratos/GetVersion',{}).then(response => {
                if (version != response) {
                    swal({
                        title: 'Alerta!',
                        text: 'Es necesario limpiar la caché del navegador,\nPresione <<Aceptar>> para limpiar la caché',
                        icon: "warning",
                        buttons: true,
                        dangerMode: true,
                        buttons: ["Cerrar", "Aceptar"]
                      })
                      .then((aceptar) => {
                        if (aceptar) {
                            window.history.forward(1);
                            location.reload();
                        }
                      });
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }

        function getCedula() {
            $.get('/Contratos/GetCedula', {
                fechaCorte: moment(txtFechaCorte.val(), 'DD/MM/YYYY').toISOString(true)
             }).then(response => {
                if (response.success) {
                    AddRows(tblCedula, response.items);
                    detalleCortoPlazo = response.detalleCortoPlazo;
                    detalleLargoPlazo = response.detalleLargoPlazo;

                    AddRows(tblCedulaPQ, response.pqs);

                    btnDetalle.attr('disabled', false);
                    //btnDetallePQ.attr('disabled', false);
                } else {
                    btnDetalle.attr('disabled', true);
                    //btnDetallePQ.attr('disabled', true);
                    swal("Alerta!", response.message, "warning");
                }
            }, error => {
                btnDetalle.attr('disabled', true);
                //btnDetallePQ.attr('disabled', true);
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }
        //#endregion
    }

    $(document).ready(() => Maquinaria.DocumentosPorPagar.CedulaMensual = new CedulaMensual())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();