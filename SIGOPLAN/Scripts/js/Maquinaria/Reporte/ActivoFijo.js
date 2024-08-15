(() => {
    $.namespace('Maquinaria.ActivoFijo.Index');
    Index = function () {
        const tblSaldos = $('#tblSaldos');
        const tblDepreciacion = $('#tblDepreciacion');
        const tblAFDetalle = $('#tblAFDetalle');
        const tblAFDiferenciasSaldos = $('#tblAFDiferenciasSaldos');
        const tblAFDiferenciasSaldosDetalle = $('#tblAFDiferenciasSaldosDetalle');
        const tblTotalizadores = $('#tblTotalizadores');
        const txtSaldoFechaAl = $('#txtSaldoFechaAl');
        const idEmpresa = $('#idEmpresa');
        const txtSaldoFechaAltaBaja = $('#txtSaldoFechaAltaBaja');
        const btnFiltro = $('#btnFiltro');
        const btnDescargarExcel = $('#btnDescargarExcel');
        const btnDepreciacionCuentas = $('#btnDepreciacionCuentas');
        const btnModificarDepCuentas = $('#btnModificarDepCuentas');
        const btnImprimir = $('#btnImprimir');
        const report = $("#report");
        const tituloPrincipal = $('#tituloPrincipal');
        const contenedorDifSaldos = $('#contenedorDifSaldos');
        const contenedorDifSaldosDetalles = $('#contenedorDifSaldosDetalles');
        const goBackDetallesSaldos = $('#goBackDetallesSaldos');

        //Modales
        const modalDetalleCuenta = $('#modalDetalleCuenta');
        const modalDepreciacionCuentas = $('#modalDepreciacionCuentas');
        const modalDetalleDiferenciaSaldos = $('#modalDetalleDiferenciaSaldos');

        //Colombia
        const divColombia = $('.divColombia');
        const tblSaldosColombia = $('#tblSaldosColombia');
        const tblDepColombia = $('#tblDepColombia');
        const modalDetalleCuentaColombia = $('#modalDetalleCuentaColombia');
        const tblDetalleColombia = $('#tblDetalleColombia');

        //Peru
        const divPeru = $('.divPeru');
        const tblSaldosPeru = $('#tblSaldosPeru');
        const tblDepPeru = $('#tblDepPeru');
        const modalDetalleCuentaPeru = $('#modalDetalleCuentaPeru');
        const tblDetallePeru = $('#tblDetallePeru');


        btnDescargarExcel.on('click', function (e) {
            location.href = '/ActivoFijo/Excel?fechaSaldoAnterior=' + '&fechaSaldoActual=' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').toISOString(true);
        });

        btnDepreciacionCuentas.on('click', function () {
            GetDepreciacionCuenta();
        });

        btnFiltro.on('click', function () {
            if (idEmpresa.val() == 2 && moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').isBefore('2018-01-01')) {
                AlertaGeneral('Alerta', 'Fecha invalida, no puede ser menor al año 2018');
                return;
            }
            if (idEmpresa.val() == 1 && moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').isBefore('2020-10-1')) {
                AlertaGeneral('Alerta', 'Fecha invalida, no puede ser menor del 31/10/2020');
                return;
            }
            tituloPrincipal.text('RESUMEN CEDULA DE ACTIVO FIJO ' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').format('YYYY'));
            getResumen();
        });

        btnImprimir.on('click', () => {
            verReporte();
        });

        btnModificarDepCuentas.on('click', function () {
            var cuentas = $(this).data('cuentas');
            objDepCuentas = new Array();
            for (let index = 0; index < cuentas; index++) {
                objDepCuenta = {
                    Id: $('#idDepCuenta' + index).data('id'),
                    Cuenta: '',
                    Descripcion: '',
                    PorcentajeDepreciacion: $('#txtPorcentajeDepreciacion' + $('#idDepCuenta' + index).data('id')).val(),
                    MesesDeDepreciacion: $('#txtMesesDepreciacion' + $('#idDepCuenta' + index).data('id')).val()
                }
                objDepCuentas.push(objDepCuenta);
            }
            ModificarDepreciacionCuenta(objDepCuentas);
        });

        tblSaldos.on('keypress', '.soloNumero2D', function (event) {
            aceptaSoloNumero2D($(this), event);
        });

        tblSaldos.on('paste', '.soloNumero2D', function (event) {
            permitePegarSoloNumero2D($(this), event);
        });

        tblSaldos.on('change', '.soloNumero2D', function () {
            formatoMoneda($(this));
            let _dif = $(this).closest('td').prev('td').text();
            let _numDif = unmaskNumero(_dif);
            let _totalDif = Math.abs(_numDif - unmaskNumero($(this).val()));
            $(this).closest('td').next('td').text(maskNumero(_totalDif))
            recalcularTotalContabilidad();
        });

        tblDepreciacion.on('keypress', '.soloNumero2D', function (event) {
            aceptaSoloNumero2D($(this), event);
        });

        tblDepreciacion.on('paste', '.soloNumero2D', function (event) {
            permitePegarSoloNumero2D($(this), event);
        });

        tblDepreciacion.on('change', '.soloNumero2D', function () {
            formatoMoneda($(this));
            let _dif = $(this).closest('td').prev('td').text();
            let _numDif = unmaskNumero(_dif);
            let _totalDif = Math.abs(_numDif - unmaskNumero($(this).val()));
            $(this).closest('td').next('td').text(maskNumero(_totalDif))
            recalcularTotalContabilidadDepreciacion();
        });

        txtSaldoFechaAltaBaja.on('change', function () {
            if (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').isBefore('2018-01-01')) {
                txtSaldoFechaAltaBaja.val('');
            }
        });

        modalDetalleCuenta.on('hidden.bs.modal', function () {
            dt = tblAFDetalle.DataTable();
            dt.clear().draw();
        })

        $('table').on('click', '.verDetalle', function () {
            if (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').isBefore('2018-01-01') || !moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').isValid()) {
                txtSaldoFechaAltaBaja.val('');
                AlertaGeneral('Error', 'Fecha invalida, no puede ser menor al año 2018 y debe tener el formato día/mes/año');
                return;
            }
            modalDetalleCuenta.modal('show');
            modalDetalleCuenta.find('.modal-title').text('Detalle cuenta ' + $(this).data('cuenta'));
            GetDetalle($(this).data('cuenta'));
        });

        $('table').on('click', '.difDetalleMensual_saldos', function () {
            let cta = $(this).data('cuenta');
            let año = $(this).data('año');
            let mes = $(this).data('mes');

            let titulo = modalDetalleDiferenciaSaldos.find('.modal-title').text();
            modalDetalleDiferenciaSaldos.find('.modal-title').text(titulo + '. Año: ' + año + ' Mes: ' + mes);

            console.log(cta + ' ' + año + ' ' + mes);
            let diferenciaDetalle = new Array();

            datosDifSaldos.forEach(function (element, index, array) {
                if (element.Cuenta === cta && element.Año === año && mes === element.Mes) {
                    element.Detalles.forEach(function (elementDetalle) {
                        diferenciaDetalle.push(elementDetalle);
                    });
                }
            });

            console.log(diferenciaDetalle);
            AddRows(tblAFDiferenciasSaldosDetalle, diferenciaDetalle);

            contenedorDifSaldos.toggle('slide', null, 250, function () {
                goBackDetallesSaldos.data('cuenta', cta);
                contenedorDifSaldosDetalles.toggle('slide', null, 250, function () {
                    tblAFDiferenciasSaldosDetalle.DataTable().columns.adjust().draw();
                });
            });
        });

        goBackDetallesSaldos.on('click', function () {
            modalDetalleDiferenciaSaldos.find('.modal-title').text('Diferencia ' + tipoDiferencia.toLowerCase() + ', cuenta: ' + $(this).data('cuenta'));
            contenedorDifSaldosDetalles.toggle('slide', null, 250, function () {
                contenedorDifSaldos.toggle('slide', null, 250);
            });
        });

        $('table').on('click', '.difContabilidad', function () {
            tipoDiferencia = $(this).data('tipo-diferencia');
            modalDetalleDiferenciaSaldos.find('.modal-title').text('Diferencia ' + tipoDiferencia.toLowerCase() + ', cuenta: ' + $(this).data('cuenta'));
            contenedorDifSaldos.show();
            contenedorDifSaldosDetalles.hide();
            ObtenerDiferenciasSaldos($(this).data('cuenta'));
        });

        $('table').on('click', '.difContabilidadDep', function () {
            tipoDiferencia = $(this).data('tipo-diferencia');
            modalDetalleDiferenciaSaldos.find('.modal-title').text('Diferencia ' + tipoDiferencia.toLowerCase() + ', cuenta: ' + $(this).data('cuenta'));
            contenedorDifSaldos.show();
            contenedorDifSaldosDetalles.hide();
            ObtenerDiferenciasDep($(this).data('cuenta'));
        })

        modalDetalleDiferenciaSaldos.on('shown.bs.modal', function () {
            tblAFDiferenciasSaldos.DataTable().columns.adjust().draw();
        });

        modalDetalleCuenta.on('shown.bs.modal', function () {
            tblAFDetalle.DataTable().columns.adjust().draw();
        });

        //eventos colombia
        modalDetalleCuentaColombia.on('shown.bs.modal', function () {
            tblDetalleColombia.DataTable().columns.adjust().draw();
        });

        //eventos peru
        modalDetalleCuentaPeru.on('shown.bs.modal', function () {
            tblDetallePeru.DataTable().columns.adjust().draw();
        });

        function recalcularTotalContabilidad() {
            let _sumTotal = 0.0;
            $('.txtSaldosContabilidad').each(function () {
                _sumTotal += unmaskNumero($(this).val());
            });
            tblSaldos.find('tfoot').find('.totalSaldosContabilidad').text(maskNumero(_sumTotal));

            let _sumTotalDif = 0.0;
            $('.txtSaldosDiferencia').each(function () {
                console.log(unmaskNumero($(this).text()));
                _sumTotalDif += unmaskNumero($(this).text());
            });
            tblSaldos.find('tfoot').find('.totalSaldosDiferencia').text(maskNumero(_sumTotalDif));
        }

        function recalcularTotalContabilidadDepreciacion() {
            let _sumTotal = 0.0;
            $('.txtDepreciacionContabilidad').each(function () {
                _sumTotal += unmaskNumero($(this).val());
            });
            tblDepreciacion.find('tfoot').find('.totalDepreciacionContabilidad').text(maskNumero(_sumTotal));

            let _sumTotalDif = 0.0;
            $('.txtDepreciacionDiferencia').each(function () {
                console.log(unmaskNumero($(this).text()));
                _sumTotalDif += unmaskNumero($(this).text());
            });
            tblDepreciacion.find('tfoot').find('.totalDepreciacionDiferencia').text(maskNumero(_sumTotalDif));
        }

        let tblSaldosTotales;
        let tblTotalizadoresTotales;
        let fecha;
        let añoFechaHasta;
        let cuentaDetalle = null;
        let datosDifSaldos;
        let tipoDiferencia = 'ninguna';

        (function init() {
            switch (idEmpresa.val()) {
                case "1":
                    {
                    }
                    break;
                case "2":
                    {
                        divColombia.hide();
                        divPeru.hide();
                    }
                    break;
                case "3":
                    {
                        divPeru.hide();
                    }
                    break;
                case "6":
                    {
                        divColombia.hide();
                    }
                    break;
            }

            tituloPrincipal.text('RESUMEN CEDULA DE ACTIVO FIJO ' + new Date().getFullYear());
            initFechas();
            initTableSaldos();
            initTableDepreciacion();
            initTblAFDiferenciasSaldos();
            initTblAFDiferenciasSaldosDetalles();
            initTableTotalizadores();

            initTableDetalle();
        })();

        function initFechas() {
            const añoActual = new Date().getFullYear();
            const diaActual = new Date().getDate();
            const mesActual = new Date().getMonth();
            const ultimoDiaDelMes = moment(new Date(añoActual, mesActual, 1)).endOf('month').format('DD');

            txtSaldoFechaAl.datepicker({
                changeYear: true,
                changeMonth: false,
                showButtonPanel: true,
                onClose: function (dateText, inst) {
                    function isDonePressed() {
                        return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                    }

                    if (isDonePressed()) {
                        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                        $(this).datepicker('setDate', new Date(year, 11, 31)).trigger('change');

                        $(this).focusout()//Added to remove focus from datepicker input box on selecting date
                    }
                }
            }).datepicker('setDate', new Date(2018, 11, 31));

            txtSaldoFechaAltaBaja.datepicker({ yearRange: '2018:c' }).datepicker('setDate', new Date(añoActual, mesActual, ultimoDiaDelMes));
            // txtSaldoFechaAltaBaja.datepicker({
            //     dateFormat: "dd/mm/yy",
            //     changeYear: true,
            //     changeMonth: true,
            //     showButtonPanel: true,
            //     yearRange: '2018:c',
            //     onClose: function (dateText, inst) {
            //         function isDonePressed() {
            //             return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
            //         }

            //         if (isDonePressed()) {
            //             var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            //             var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            //             var day = moment(new Date(year, month, 1)).endOf('month').format('DD');
            //             $(this).datepicker('setDate', new Date(year, month, day)).trigger('change');

            //             $(this).focusout()//Added to remove focus from datepicker input box on selecting date
            //         }
            //     }
            // }).datepicker('setDate', new Date(añoActual, mesActual, ultimoDiaDelMes));
        }

        function initTableSaldos() {
            tblSaldos.DataTable({
                order: [[0, "asc"]],
                searching: false,
                paging: false,
                ordering: true,
                info: false,
                language: dtDicEsp,
                columns: [
                    { data: 'Cuenta', title: 'Cuenta' },
                    { data: 'Concepto', title: 'Concepto' },
                    { data: 'SaldoAnterior', title: 'Saldo al 31/12/2018' },
                    { data: 'Altas', title: 'Altas 2019' },
                    { data: 'Bajas', title: 'Bajas 2019' },
                    { data: 'SaldoActual', title: 'Saldo 2019' },
                    { data: 'Contabilidad', title: 'Contabilidad' },
                    { data: 'Diferencia', title: 'Diferencia' }
                ],
                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    },
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            var linkDetalle = '<span class="verDetalle" data-cuenta="' + data + '">' + data + '</span>'
                            return linkDetalle;
                        }
                    },
                    {
                        targets: [2, 3, 4, 5, 6],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [7],
                        render: function (data, type, row) {
                            if (data >= 0.4 || data <= -0.4 /*data != 0*/) {
                                return '<span class="numeroRojo difContabilidad" data-cuenta="' + row.Cuenta + '" data-tipo-diferencia="Saldo">' + maskNumero(data) + '</span>';
                            }
                            return maskNumero(data);
                        }
                    }
                    // {
                    //     targets: [6],
                    //     className: 'text-nowrap',
                    //     render: function(data, type, row){
                    //         let _inputContabilidad = '';
                    //         _inputContabilidad = '<input type="text" class="form-control txtSaldosContabilidad soloNumero2D" />';
                    //         return _inputContabilidad;
                    //     }
                    // }
                ],
                createdRow: function (row, data, dataIndex) {
                    $(row).find('td').eq(0).addClass('text-center');
                    $(row).find('td').eq(1).nextAll().addClass('text-right');
                    $(row).find('td').eq(7).addClass('txtSaldosDiferencia');
                },
                drawCallback: function (settings) {
                },
                headerCallback: function (thead, data, start, end, display) {

                    const ultimoDiaDelMes = moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').endOf('month').format('DD');

                    $(thead).find('th').addClass('text-center');
                    $(thead).addClass('bg-table-header');
                    $(thead).find('th').eq(2).html('Saldo al 31/12/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year() - 1));
                    $(thead).find('th').eq(3).html('Altas ' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year());
                    $(thead).find('th').eq(4).html('Bajas ' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year());
                    $(thead).find('th').eq(5).html('Saldos al ' + ultimoDiaDelMes + '/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').month() + 1) + '/' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year());
                },
                footerCallback: function (tfoot, data, start, end, display) {
                    if (tblSaldosTotales != null) {
                        $(tfoot).find('th').eq(1).html(maskNumero(tblSaldosTotales.SaldosSuma_SaldoAnterior));
                        $(tfoot).find('th').eq(2).html(maskNumero(tblSaldosTotales.SaldosSuma_Altas));
                        $(tfoot).find('th').eq(3).html(maskNumero(tblSaldosTotales.SaldosSuma_Bajas));
                        $(tfoot).find('th').eq(4).html(maskNumero(tblSaldosTotales.SaldosSuma_SaldoActual));
                        $(tfoot).find('th').eq(5).html(maskNumero(tblSaldosTotales.SaldosSuma_Contabildiad));
                        // $(tfoot).find('th').eq(5).addClass('totalSaldosContabilidad');
                        $(tfoot).find('th').eq(6).html(maskNumero(tblSaldosTotales.SaldosSuma_Diferencia));
                        $(tfoot).find('th').eq(6).addClass('totalSaldosDiferencia');
                    }
                }
            });

            tblSaldosColombia.DataTable({
                order: [[0, "asc"]],
                searching: false,
                paging: false,
                ordering: true,
                info: false,
                language: dtDicEsp,
                columns: [
                    { data: 'saldo.cuenta', title: 'Cuenta' },
                    { data: 'saldo.concepto', title: 'Concepto' },
                    { data: 'saldo.saldoAnteriorMN', title: 'MN' },
                    { data: 'saldo.saldoAnteriorCOP', title: 'COP' },
                    { data: 'saldo.altaActualMN', title: 'MN' },
                    { data: 'saldo.altaActualCOP', title: 'COP' },
                    { data: 'saldo.bajaActualMN', title: 'MN' },
                    { data: 'saldo.bajaActualCOP', title: 'COP' },
                    { data: 'saldo.saldoActualMN', title: 'MN' },
                    { data: 'saldo.saldoActualCOP', title: 'COP' },
                    { data: 'saldo.contabilidadMN', title: 'MN' },
                    { data: 'saldo.contabilidadCOP', title: 'COP' },
                    { data: 'saldo.diferenciaMN', title: 'MN' },
                    { data: 'saldo.diferenciaCOP', title: 'COP' }
                ],
                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    },
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            var linkDetalle = '<span class="verDetalleColombia" data-cuenta="' + data + '">' + data + '</span>'
                            return linkDetalle;
                        }
                    },
                    {
                        targets: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [12, 13],
                        render: function (data, type, row) {
                            // if(data >= 0.4 || data <= -0.4 /*data != 0*/) {
                            //     return '<span class="numeroRojo difContabilidad" data-cuenta="' + row.Cuenta + '" data-tipo-diferencia="Saldo">' + maskNumero(data) + '</span>';
                            // }
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    $(row).find('td').eq(0).addClass('text-center');
                    $(row).find('td').eq(1).nextAll().addClass('text-right');
                    $(row).find('td').eq(12).addClass('txtSaldosDiferencia');
                    $(row).find('td').eq(13).addClass('txtSaldosDiferencia');
                },

                drawCallback: function (settings) {
                },

                initComplete: function (settings, json) {
                    tblSaldosColombia.on('click', '.verDetalleColombia', function () {
                        const rowData = tblSaldosColombia.DataTable().row($(this).closest('tr')).data();

                        AddRows(tblDetalleColombia, rowData.detalle);
                        modalDetalleCuentaColombia.find('.modal-title').text(`Detalle cuenta ${rowData.saldo.cuenta} - ${rowData.saldo.concepto}`);
                        modalDetalleCuentaColombia.modal('show');
                    });
                },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).next().find('th').removeClass('text-right');
                    $(thead).next().find('th').addClass('text-center');
                    $(thead).next().addClass('bg-table-header');
                    $(thead).addClass('bg-table-header');
                    $(thead).find('th').addClass('text-center');

                    $(thead).find('th').eq(1).html('Saldo al 31/12/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year() - 1));
                    $(thead).find('th').eq(2).html('Altas ' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year()));
                    $(thead).find('th').eq(3).html('Bajas ' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year()));
                    $(thead).find('th').eq(4).html('Saldos al 31/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').month() + 1) + '/' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year());
                    $(thead).find('th').eq(5).html('Saldos contabilidad al 31/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').month() + 1) + '/' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year());
                    $(thead).find('th').eq(6).html('Diferencia');
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    let total = 0;

                    tblSaldosColombia.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                        if (colIdx > 1) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            $(this.footer()).html(maskNumero(total));
                            total = 0;
                        }
                    });
                }
            });

            tblSaldosPeru.DataTable({
                order: [[0, "asc"]],
                searching: false,
                paging: false,
                ordering: true,
                info: false,
                language: dtDicEsp,
                columns: [
                    { data: 'saldo.cuenta', title: 'Cuenta' },
                    { data: 'saldo.concepto', title: 'Concepto' },
                    { data: 'saldo.saldoAnteriorMN', title: 'MN' },
                    { data: 'saldo.saldoAnteriorCOP', title: 'SOL' },
                    { data: 'saldo.altaActualMN', title: 'MN' },
                    { data: 'saldo.altaActualCOP', title: 'SOL' },
                    { data: 'saldo.bajaActualMN', title: 'MN' },
                    { data: 'saldo.bajaActualCOP', title: 'SOL' },
                    { data: 'saldo.saldoActualMN', title: 'MN' },
                    { data: 'saldo.saldoActualCOP', title: 'SOL' },
                    { data: 'saldo.contabilidadMN', title: 'MN' },
                    { data: 'saldo.contabilidadCOP', title: 'SOL' },
                    { data: 'saldo.diferenciaMN', title: 'MN' },
                    { data: 'saldo.diferenciaCOP', title: 'SOL' }
                ],
                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    },
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            var linkDetalle = '<span class="verDetallePeru" data-cuenta="' + data + '">' + data + '</span>'
                            return linkDetalle;
                        }
                    },
                    {
                        targets: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [12, 13],
                        render: function (data, type, row) {
                            // if(data >= 0.4 || data <= -0.4 /*data != 0*/) {
                            //     return '<span class="numeroRojo difContabilidad" data-cuenta="' + row.Cuenta + '" data-tipo-diferencia="Saldo">' + maskNumero(data) + '</span>';
                            // }
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    $(row).find('td').eq(0).addClass('text-center');
                    $(row).find('td').eq(1).nextAll().addClass('text-right');
                    $(row).find('td').eq(12).addClass('txtSaldosDiferencia');
                    $(row).find('td').eq(13).addClass('txtSaldosDiferencia');
                },

                drawCallback: function (settings) {
                },

                initComplete: function (settings, json) {
                    tblSaldosPeru.on('click', '.verDetallePeru', function () {
                        const rowData = tblSaldosPeru.DataTable().row($(this).closest('tr')).data();

                        AddRows(tblDetallePeru, rowData.detalle);
                        modalDetalleCuentaPeru.find('.modal-title').text(`Detalle cuenta ${rowData.saldo.cuenta} - ${rowData.saldo.concepto}`);
                        modalDetalleCuentaPeru.modal('show');
                    });
                },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).next().find('th').removeClass('text-right');
                    $(thead).next().find('th').addClass('text-center');
                    $(thead).next().addClass('bg-table-header');
                    $(thead).addClass('bg-table-header');
                    $(thead).find('th').addClass('text-center');

                    $(thead).find('th').eq(1).html('Saldo al 31/12/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year() - 1));
                    $(thead).find('th').eq(2).html('Altas ' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year()));
                    $(thead).find('th').eq(3).html('Bajas ' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year()));
                    $(thead).find('th').eq(4).html('Saldos al 31/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').month() + 1) + '/' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year());
                    $(thead).find('th').eq(5).html('Saldos contabilidad al 31/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').month() + 1) + '/' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year());
                    $(thead).find('th').eq(6).html('Diferencia');
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    let total = 0;

                    tblSaldosPeru.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                        if (colIdx > 1) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            $(this.footer()).html(maskNumero(total));
                            total = 0;
                        }
                    });
                }
            });

            tblDepColombia.DataTable({
                order: [[0, "asc"]],
                searching: false,
                paging: false,
                ordering: true,
                info: false,
                language: dtDicEsp,
                columns: [
                    { data: 'dep.cuenta', title: 'Cuenta' },
                    { data: 'dep.concepto', title: 'Concepto' },
                    { data: 'dep.depActualMN', title: 'MN' },
                    { data: 'dep.depActualCOP', title: 'COP' },
                    { data: 'dep.depAcumuladaMN', title: 'MN' },
                    { data: 'dep.depAcumuladaCOP', title: 'COP' },
                    { data: 'dep.depContabilidadMN', title: 'MN' },
                    { data: 'dep.depContabilidadCOP', title: 'COP' },
                    { data: 'dep.diferenciaMN', title: 'MN' },
                    { data: 'dep.diferenciaCOP', title: 'COP' }
                ],

                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    },
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            var linkDetalle = '<span class="verDetalleColombia" data-cuenta="' + data + '">' + data + '</span>'
                            return linkDetalle;
                        }
                    },
                    {
                        targets: [2, 3, 4, 5, 6, 7, 8, 9],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [8, 9],
                        render: function (data, type, row) {
                            // if(data >= 0.4 || data <= -0.4 /*data != 0*/) {
                            //     return '<span class="numeroRojo difContabilidad" data-cuenta="' + row.Cuenta + '" data-tipo-diferencia="Saldo">' + maskNumero(data) + '</span>';
                            // }
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    $(row).find('td').eq(0).addClass('text-center');
                    $(row).find('td').eq(1).nextAll().addClass('text-right');
                    $(row).find('td').eq(8).addClass('txtSaldosDiferencia');
                    $(row).find('td').eq(9).addClass('txtSaldosDiferencia');
                },

                drawCallback: function (settings) { },

                initComplete: function (settings, json) {
                    tblDepColombia.on('click', '.verDetalleColombia', function () {
                        const rowData = tblDepColombia.DataTable().row($(this).closest('tr')).data();

                        AddRows(tblDetalleColombia, rowData.detalle);
                        modalDetalleCuentaColombia.find('.modal-title').text(`Detalle cuenta ${rowData.saldo.cuenta} - ${rowData.saldo.concepto}`);
                        modalDetalleCuentaColombia.modal('show');
                    });
                },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).next().find('th').removeClass('text-right');
                    $(thead).next().find('th').addClass('text-center');
                    $(thead).next().addClass('bg-table-header');
                    $(thead).addClass('bg-table-header');
                    $(thead).find('th').addClass('text-center');

                    const ultimoDiaDelMes = moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').endOf('month').format('DD');

                    $(thead).find('th').eq(1).html('Depreciación contable ejercicio ' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year()));
                    $(thead).find('th').eq(2).html('Depreciación acumulada al ' + ultimoDiaDelMes + '/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').month() + 1) + '/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year()));
                    $(thead).find('th').eq(3).html('Depreciación contable registrada');
                    $(thead).find('th').eq(4).html('Diferencia');
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    let total = 0;

                    tblDepColombia.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                        if (colIdx > 1) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            $(this.footer()).html(maskNumero(total));
                            total = 0;
                        }
                    });
                }
            });

            tblDepPeru.DataTable({
                order: [[0, "asc"]],
                searching: false,
                paging: false,
                ordering: true,
                info: false,
                language: dtDicEsp,
                columns: [
                    { data: 'dep.cuenta', title: 'Cuenta' },
                    { data: 'dep.concepto', title: 'Concepto' },
                    { data: 'dep.depActualMN', title: 'MN' },
                    { data: 'dep.depActualCOP', title: 'SOL' },
                    { data: 'dep.depAcumuladaMN', title: 'MN' },
                    { data: 'dep.depAcumuladaCOP', title: 'SOL' },
                    { data: 'dep.depContabilidadMN', title: 'MN' },
                    { data: 'dep.depContabilidadCOP', title: 'SOL' },
                    { data: 'dep.diferenciaMN', title: 'MN' },
                    { data: 'dep.diferenciaCOP', title: 'SOL' }
                ],

                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    },
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            var linkDetalle = '<span class="verDetallePeru" data-cuenta="' + data + '">' + data + '</span>'
                            return linkDetalle;
                        }
                    },
                    {
                        targets: [2, 3, 4, 5, 6, 7, 8, 9],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [8, 9],
                        render: function (data, type, row) {
                            // if(data >= 0.4 || data <= -0.4 /*data != 0*/) {
                            //     return '<span class="numeroRojo difContabilidad" data-cuenta="' + row.Cuenta + '" data-tipo-diferencia="Saldo">' + maskNumero(data) + '</span>';
                            // }
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    $(row).find('td').eq(0).addClass('text-center');
                    $(row).find('td').eq(1).nextAll().addClass('text-right');
                    $(row).find('td').eq(8).addClass('txtSaldosDiferencia');
                    $(row).find('td').eq(9).addClass('txtSaldosDiferencia');
                },

                drawCallback: function (settings) { },

                initComplete: function (settings, json) {
                    tblDepPeru.on('click', '.verDetallePeru', function () {
                        const rowData = tblDepPeru.DataTable().row($(this).closest('tr')).data();

                        AddRows(tblDetallePeru, rowData.detalle);
                        modalDetalleCuentaPeru.find('.modal-title').text(`Detalle cuenta ${rowData.saldo.cuenta} - ${rowData.saldo.concepto}`);
                        modalDetalleCuentaPeru.modal('show');
                    });
                },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).next().find('th').removeClass('text-right');
                    $(thead).next().find('th').addClass('text-center');
                    $(thead).next().addClass('bg-table-header');
                    $(thead).addClass('bg-table-header');
                    $(thead).find('th').addClass('text-center');

                    const ultimoDiaDelMes = moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').endOf('month').format('DD');

                    $(thead).find('th').eq(1).html('Depreciación contable ejercicio ' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year()));
                    $(thead).find('th').eq(2).html('Depreciación acumulada al ' + ultimoDiaDelMes + '/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').month() + 1) + '/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year()));
                    $(thead).find('th').eq(3).html('Depreciación contable registrada');
                    $(thead).find('th').eq(4).html('Diferencia');
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    let total = 0;

                    tblDepPeru.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                        if (colIdx > 1) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            $(this.footer()).html(maskNumero(total));
                            total = 0;
                        }
                    });
                }
            });

            tblDetalleColombia.DataTable({
                order: [[0, "asc"]],
                searching: true,
                paging: false,
                ordering: true,
                info: true,
                language: dtDicEsp,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    { data: 'noEconomico', title: '# económico' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'fechaMovimiento', title: 'Fecha movimiento' },
                    { data: 'fechaInicioDep', title: 'Fecha inicio dep' },
                    { data: 'mesesDepreciacion', title: 'Meses dep' },
                    { data: 'porcentajeDepreciacion', title: '% dep' },
                    { data: 'ccCompleto', title: 'cc' },
                    { data: 'polizaCompleta', title: 'Póliza' },
                    { data: 'tmPolizaId', tite: 'TM' },
                    { data: 'cuentaCompleta', title: 'Cuenta' },
                    { data: 'moi', title: 'MOI' },
                    { data: 'alta', title: 'Alta' },
                    { data: 'fechaBaja', title: 'Fecha baja' },
                    { data: 'baja', title: 'Baja' },
                    { data: 'tmPolizaId_Baja', title: 'TM baja' },
                    { data: 'depMensual', title: 'Dep mensual' },
                    { data: 'mesesDepreciadosAnteriormente', title: 'Meses dep anteriores' },
                    { data: 'mesesDepreciadosActualmente', title: 'Meses dep actuales' },
                    { data: 'depreciacionAnterior', title: 'Dep anterior' },
                    { data: 'depreciacionActual', title: 'Dep actual' },
                    { data: 'bajaDepreciacion', title: 'Baja dep' },
                    { data: 'depreciacionAcumulada', title: 'Dep acumulada' },
                    { data: 'saldoLibro', title: 'Saldo libros' }
                ],
                columnDefs: [
                    {
                        targets: [0, 2, 3, 4, 5, 8, 12, 14, 16, 17],
                        className: 'text-center'
                    },
                    {
                        targets: [2, 3, 12],
                        render: function (data, type, row) {
                            return data == null ? '' : moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            return maskNumeroPorcentaje(data);
                        }
                    },
                    {
                        targets: [10, 11, 13, 15, 18, 19, 20, 21, 22],
                        className: 'text-right',
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                },

                drawCallback: function (settings) {
                    let total = 0;
                    const columnas = [10, 11, 13, 15, 18, 19, 20, 21, 22];

                    tblDetalleColombia.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                        if (columnas.includes(colIdx)) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            $(this.footer()).html(maskNumero(total));
                            total = 0;
                        }
                    });
                },

                initComplete: function (settings, json) { },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).find('th').addClass('text-center');
                    $(thead).find('th').eq('10').text('MOI al 31/12/' + (añoFechaHasta - 1));
                    $(thead).find('th').eq('11').text('Altas ' + (añoFechaHasta));
                    $(thead).find('th').eq('16').text('Meses depreciados a ' + (añoFechaHasta - 1));
                    $(thead).find('th').eq('17').text('Meses ' + (añoFechaHasta));
                    $(thead).find('th').eq('18').text('Dep. contable acumulada al 31/12/' + (añoFechaHasta - 1));
                    $(thead).find('th').eq('19').text('Dep. contable ' + (añoFechaHasta));
                    $(thead).find('th').eq('21').text('Dep. contable acumulada a ' + (añoFechaHasta));
                },

                footerCallback: function (tfoot, data, start, end, display) { }
            });

            tblDetallePeru.DataTable({
                order: [[0, "asc"]],
                searching: true,
                paging: false,
                ordering: true,
                info: true,
                language: dtDicEsp,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    { data: 'noEconomico', title: '# económico' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'fechaMovimiento', title: 'Fecha movimiento' },
                    { data: 'fechaInicioDep', title: 'Fecha inicio dep' },
                    { data: 'mesesDepreciacion', title: 'Meses dep' },
                    { data: 'porcentajeDepreciacion', title: '% dep' },
                    { data: 'ccCompleto', title: 'cc' },
                    { data: 'polizaCompleta', title: 'Póliza' },
                    { data: 'tmPolizaId', tite: 'TM' },
                    { data: 'cuentaCompleta', title: 'Cuenta' },
                    { data: 'moi', title: 'MOI' },
                    { data: 'alta', title: 'Alta' },
                    { data: 'fechaBaja', title: 'Fecha baja' },
                    { data: 'baja', title: 'Baja' },
                    { data: 'tmPolizaId_Baja', title: 'TM baja' },
                    { data: 'depMensual', title: 'Dep mensual' },
                    { data: 'mesesDepreciadosAnteriormente', title: 'Meses dep anteriores' },
                    { data: 'mesesDepreciadosActualmente', title: 'Meses dep actuales' },
                    { data: 'depreciacionAnterior', title: 'Dep anterior' },
                    { data: 'depreciacionActual', title: 'Dep actual' },
                    { data: 'bajaDepreciacion', title: 'Baja dep' },
                    { data: 'depreciacionAcumulada', title: 'Dep acumulada' },
                    { data: 'saldoLibro', title: 'Saldo libros' }
                ],
                columnDefs: [
                    {
                        targets: [0, 2, 3, 4, 5, 8, 12, 14, 16, 17],
                        className: 'text-center'
                    },
                    {
                        targets: [2, 3, 12],
                        render: function (data, type, row) {
                            return data == null ? '' : moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            return maskNumeroPorcentaje(data);
                        }
                    },
                    {
                        targets: [10, 11, 13, 15, 18, 19, 20, 21, 22],
                        className: 'text-right',
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                },

                drawCallback: function (settings) {
                    let total = 0;
                    const columnas = [10, 11, 13, 15, 18, 19, 20, 21, 22];

                    tblDetallePeru.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                        if (columnas.includes(colIdx)) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            $(this.footer()).html(maskNumero(total));
                            total = 0;
                        }
                    });
                },

                initComplete: function (settings, json) { },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).find('th').addClass('text-center');
                    $(thead).find('th').eq('10').text('MOI al 31/12/' + (añoFechaHasta - 1));
                    $(thead).find('th').eq('11').text('Altas ' + (añoFechaHasta));
                    $(thead).find('th').eq('16').text('Meses depreciados a ' + (añoFechaHasta - 1));
                    $(thead).find('th').eq('17').text('Meses ' + (añoFechaHasta));
                    $(thead).find('th').eq('18').text('Dep. contable acumulada al 31/12/' + (añoFechaHasta - 1));
                    $(thead).find('th').eq('19').text('Dep. contable ' + (añoFechaHasta));
                    $(thead).find('th').eq('21').text('Dep. contable acumulada a ' + (añoFechaHasta));
                },

                footerCallback: function (tfoot, data, start, end, display) { }
            });
        }

        function initTableTotalizadores() {
            tblTotalizadores.DataTable({
                order: [[0, 'asc']],
                searching: false,
                paging: false,
                ordering: true,
                info: false,
                languege: dtDicEsp,
                // scrollX: true,

                // fixedColumns: {
                //     leftColumns: 2
                // },

                columns: [
                    { data: 'Cuenta', title: 'Cuenta', className: 'text-center' },
                    { data: 'Concepto', title: 'Concepto' },
                    { data: 'DepAcumuladaAnterior', title: 'Cálculos', className: 'text-right' },
                    { data: 'DepAcumuladaAnteriorSalCont', title: 'Contabilidad', className: 'text-right' },
                    { data: 'DepAñoActual', title: 'Cálculos', className: 'text-right' },
                    { data: 'DepAñoActualSalCont', title: 'Contabilidad', className: 'text-right' },
                    { data: 'BajaDep', title: 'Cálculos', className: 'text-right' },
                    { data: 'BajaDepSalCont', title: 'Contabilidad', className: 'text-right' },
                    { data: 'DepContableAcumulada', title: 'Cálculos', className: 'text-right' },
                    { data: 'DepContableAcumuladaSalCont', title: 'Contabilidad', className: 'text-right' },
                    { data: 'DepValLibros', title: 'Cálculos', className: 'text-right' },
                    { data: 'DepValLibrosSalCont', title: 'Contabilidad', className: 'text-right' }
                ],

                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    },
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            var linkDetalle = data;
                            if (idEmpresa.val() == 1 && data == 1204) {
                                linkDetalle = data.toString() + '-' + data.toString();
                            }
                            return linkDetalle;
                        }
                    },
                    {
                        targets: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }

                    }
                ],

                createdRow: function (row, data, dataIndex) {
                },

                drawCallback: function (settings) {
                },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).next().find('th').removeClass('text-right');
                    $(thead).next().find('th').addClass('text-center');
                    $(thead).next().addClass('bg-table-header');
                    // $(thead).find('th').removeClass('text-center');
                    $(thead).addClass('bg-table-header');
                    $(thead).find('th').addClass('text-center');

                    $(thead).find('th').eq(1).html('Dep. contable acumulada al ' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year() - 1));
                    $(thead).find('th').eq(2).html('Dep. contable del ' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year()));
                    $(thead).find('th').eq(3).html('Baja de dep. ' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year()));
                    $(thead).find('th').eq(4).html('Dep. contable acumulada al ' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year()));
                    $(thead).find('th').eq(5).html('Valor en libros al ' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').format('DD/MM/YYYY')));

                    // $(thead).find('th').eq(2).html('Saldo al 31/12/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year() - 1));
                    // $(thead).find('th').eq(3).html('Altas ' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year());
                    // $(thead).find('th').eq(4).html('Bajas ' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year());
                    // $(thead).find('th').eq(5).html('Saldos al 31/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').month() + 1) + '/' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year());
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    $(tfoot).find('th').removeClass('text-right');
                    $(tfoot).find('th').addClass('text-right');

                    if (tblTotalizadoresTotales != undefined) {
                        $(tfoot).find('th').eq(1).html(maskNumero(tblTotalizadoresTotales.TotalDepAcumuladaAnterior));
                        $(tfoot).find('th').eq(2).html(maskNumero(tblTotalizadoresTotales.TotalDepAcumuladaAnteriorSalCont));
                        $(tfoot).find('th').eq(3).html(maskNumero(tblTotalizadoresTotales.TotalDepAñoActual));
                        $(tfoot).find('th').eq(4).html(maskNumero(tblTotalizadoresTotales.TotalDepAñoActualSalCont));
                        $(tfoot).find('th').eq(5).html(maskNumero(tblTotalizadoresTotales.TotalBajaDep));
                        $(tfoot).find('th').eq(6).html(maskNumero(tblTotalizadoresTotales.TotalBajaDepSalCont));
                        $(tfoot).find('th').eq(7).html(maskNumero(tblTotalizadoresTotales.TotalDepContableAcumulada));
                        $(tfoot).find('th').eq(8).html(maskNumero(tblTotalizadoresTotales.TotalDepContableAcumuladaSalCont));
                        $(tfoot).find('th').eq(9).html(maskNumero(tblTotalizadoresTotales.TotalDepValLibros));
                        $(tfoot).find('th').eq(10).html(maskNumero(tblTotalizadoresTotales.TotalDepValLibrosSalCont));
                    }
                }
            });
        }

        function initTableDepreciacion() {
            tblDepreciacion.DataTable({
                order: [[0, "asc"]],
                searching: false,
                paging: false,
                ordering: true,
                info: false,
                language: dtDicEsp,
                columns: [
                    { data: 'Cuenta', title: 'Cuenta' },
                    { data: 'Concepto', title: 'Concepto' },
                    { data: 'DepreciacionAnterior', title: 'Depreciación contable ejercicio 2019' },
                    { data: 'DepreciacionAcumulada', title: 'Depreciación acumulada 2019' },
                    { data: 'DepreciacionRegistrada', title: 'Depreciación contable registrada' },
                    { data: 'Diferencia', title: 'Diferencia' },
                ],
                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    },
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            var linkDetalle = '<span class="verDetalle" data-cuenta="' + data + '">' + data + '</span>'
                            if (idEmpresa.val() == 1 && data == 1204) {
                                linkDetalle = '<span class="verDetalle" data-cuenta="' + data + '">' + data + '-' + 1230 + '</span>'
                            }
                            return linkDetalle;
                        }
                    },
                    {
                        targets: [2, 3, 4],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            if (data >= 0.4 || data <= -0.4 /*data != 0*/) {
                                //return '<span class="numeroRojo">' + maskNumero(data) + '</span>';
                                return '<span class="numeroRojo difContabilidadDep" data-cuenta="' + row.Cuenta + '" data-tipo-diferencia="Depreciación">' + maskNumero(data) + '</span>';
                            }
                            return maskNumero(data);
                        }
                    }
                    // {
                    //     targets: [4],
                    //     className: 'text-nowrap',
                    //     render: function(data, type, row){
                    //         let _inputDepreciacionContableRegistrada = '';
                    //         _inputDepreciacionContableRegistrada = '<input type="text" class="form-control txtDepreciacionContabilidad soloNumero2D" />';
                    //         return _inputDepreciacionContableRegistrada;
                    //     }
                    // }
                ],
                createdRow: function (row, data, dataIndex) {
                    $(row).find('td').eq(0).addClass('text-center');
                    $(row).find('td').eq(1).nextAll().addClass('text-right');
                    $(row).find('td').eq(5).addClass('txtDepreciacionDiferencia');
                },
                drawCallback: function (settings) {
                },
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).addClass('bg-table-header');
                    $(thead).find('th').addClass('text-center');

                    $(thead).find('th').eq(2).html('Depreciación contable ejercicio ' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year()));
                    $(thead).find('th').eq(3).html('Depreciación acumulada al 31/' + (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').month() + 1) + '/' + moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year());
                    // $(thead).find('th').eq(2).html(txtSaldoFechaAl.val());
                },
                footerCallback: function (tfoot, data, start, end, display) {
                    if (tblSaldosTotales != null) {
                        $(tfoot).find('th').eq(1).html(maskNumero(tblSaldosTotales.SumaDepreciacion_Anterior));
                        $(tfoot).find('th').eq(2).html(maskNumero(tblSaldosTotales.SumaDepreciacion_Acumulada));
                        $(tfoot).find('th').eq(3).html(maskNumero(tblSaldosTotales.SumaDepreciacion_Registrada));
                        $(tfoot).find('th').eq(3).addClass('totalDepreciacionContabilidad');
                        if (tblSaldosTotales.SumaDepreciacion_Diferencia < 0) {
                            $(tfoot).find('th').eq(4).html('<span class="numeroRojo">' + maskNumero(tblSaldosTotales.SumaDepreciacion_Diferencia) + '</span>');
                        } else {
                            $(tfoot).find('th').eq(4).html(maskNumero(tblSaldosTotales.SumaDepreciacion_Diferencia));
                        }
                        // $(tfoot).find('th').eq(4).addClass('totalDepreciacionDiferencia');
                    }
                }
            });
        }

        function initTableDetalle() {
            tblAFDetalle.DataTable({
                order: [[20, 'desc']],
                searching: true,
                paging: false,
                ordering: true,
                info: false,
                scrollX: true,
                scrollY: '47vh',
                scrollCollapse: true,
                paging: false,
                language: dtDicEsp,

                columns: [
                /*00*/{ data: 'Fecha', title: 'Fecha alta' },
                    { data: 'FechaInicioDepreciacion', title: 'Fecha inicio depreciación' },
                /*01*/{ data: 'Cc', title: 'Cc' },
                /*02*/{ data: 'Clave', title: 'Clave' },
                /*03*/{ data: 'Descripcion', title: 'Descripción' },
                /*04*/{ data: 'MOI', title: 'MOI al 31/12/XX' },
                /*05*/{ data: 'Altas', title: 'Altas' },
                /*06*/{ data: 'Overhaul', title: 'Compra componentes' },
                /*07*/{ data: 'FechaCancelacion', title: 'Fecha de cancelación' },
                /*08*/{ data: 'MontoCancelacion', title: 'Monto de cancelación' },
                /*09*/{ data: 'FechaBaja', title: 'Fecha de baja' },
                /*10*/{ data: 'MontoBaja', title: 'Monto de la baja' },
                /*11*/{ data: 'PorcentajeDepreciacion', title: '% de depreciación' },
                /*12*/{ data: 'DepreciacionMensual', title: 'Depreciación mensual' },
                /*13*/{ data: 'MesesDepreciadosAñoAnterior', title: 'Meses depreciados a' },
                /*14*/{ data: 'MesesDepreciadosAñoActual', title: 'Meses' },
                /*15*/{ data: 'DepreciacionAcumuladaAñoAnterior', title: 'Dep. contable acumulada al 31/12/' },
                /*16*/{ data: 'DepreciacionAñoActual', title: 'Dep. contable del ejercicio a Diciembre' },
                /*17*/{ data: 'BajaDepreciacion', title: 'Baja de DEPN' },
                /*18*/{ data: 'DepreciacionContableAcumulada', title: 'Dep. contable acumulada a diciembre' },
                /*19*/{ data: 'faltante' }
                ],

                columnDefs: [
                    {
                        targets: [0, 1, 8, 10],
                        render: function (data, type, row) {
                            return moment(data).isValid() ? moment(data).format('DD/MM/YYYY') : '';
                        }
                    },
                    {
                        targets: [5, 6, 7, 9, 11, 13, 16, 17, 18, 19],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [20],
                        visible: false
                    },
                    {
                        targets: [12],
                        render: function (data, type, row) {
                            return (data * 100) + '%';
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    if (data.faltante) {
                        $(row).addClass('renglonRojo');
                    }
                    $(row).find('td').addClass('text-nowrap');
                    $(row).find('td').eq(0).addClass('text-center');
                    $(row).find('td').eq(1).addClass('text-center');
                    $(row).find('td').eq(2).addClass('text-center');
                    $(row).find('td').eq(3).addClass('text-center');
                    $(row).find('td').eq(4).addClass('text-left');
                    $(row).find('td').eq(5).addClass('text-right');
                    $(row).find('td').eq(6).addClass('text-right');
                    $(row).find('td').eq(7).addClass('text-right');
                    $(row).find('td').eq(8).addClass('text-center');
                    $(row).find('td').eq(9).addClass('text-right');
                    $(row).find('td').eq(10).addClass('text-center');
                    $(row).find('td').eq(11).addClass('text-right');
                    $(row).find('td').eq(12).addClass('text-center');
                    $(row).find('td').eq(13).addClass('text-right');
                    $(row).find('td').eq(14).addClass('text-center');
                    $(row).find('td').eq(15).addClass('text-center');
                    $(row).find('td').eq(16).addClass('text-right');
                    $(row).find('td').eq(17).addClass('text-right');
                    $(row).find('td').eq(18).addClass('text-right');
                    $(row).find('td').eq(19).addClass('text-right');

                },

                drawCallback: function (settings) {
                },

                headerCallback: function (thead, data, start, end, display) {
                    // $(thead).addClass('bg-table-header');
                    $(thead).find('th').addClass('text-center');
                    $(thead).find('th').addClass('text-nowrap');
                    $(thead).find('th').eq('5').text('MOI al 31/12/' + (añoFechaHasta - 1));
                    $(thead).find('th').eq('6').text('Altas ' + (añoFechaHasta));
                    $(thead).find('th').eq('7').text('Compra componentes ' + (añoFechaHasta));
                    $(thead).find('th').eq('14').text('Meses depreciados a ' + (añoFechaHasta - 1));
                    $(thead).find('th').eq('15').text('Meses ' + (añoFechaHasta));
                    $(thead).find('th').eq('16').text('Dep. contable acumulada al 31/12/' + (añoFechaHasta - 1));
                    $(thead).find('th').eq('17').text('Dep. contable del ejercicio a Diciembre ' + (añoFechaHasta));
                    $(thead).find('th').eq('18').text('Baja de DEPN ' + (añoFechaHasta));
                    $(thead).find('th').eq('19').text('Dep. contable acumulada a diciembre ' + (añoFechaHasta));
                    // $(thead).find('th').eq('4').html('MOI al 31/12/' + (moment(txtFiltroFecha.val(), 'DD/MM/YYYY').year() - 1));
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    if (cuentaDetalle != null) {
                        var moi = 0;
                        var altas = 0;
                        var overhaul = 0;
                        var montoCancelacion = 0;
                        var montoBaja = 0;
                        var depMensual = 0;
                        var depAcumuladaAñoAnterior = 0;
                        var depAñoActual = 0;
                        var depBaja = 0;
                        var DepreciacionContableAcumulada = 0;
                        for (let index = 0; index < cuentaDetalle.length; index++) {
                            moi += cuentaDetalle[index].MOI;
                            altas += cuentaDetalle[index].Altas;
                            overhaul += cuentaDetalle[index].Overhaul;
                            montoCancelacion += cuentaDetalle[index].MontoCancelacion;
                            montoBaja += cuentaDetalle[index].MontoBaja;
                            depMensual += cuentaDetalle[index].DepreciacionMensual;
                            depAcumuladaAñoAnterior += cuentaDetalle[index].DepreciacionAcumuladaAñoAnterior;
                            depAñoActual += cuentaDetalle[index].DepreciacionAñoActual;
                            depBaja += cuentaDetalle[index].BajaDepreciacion;
                            DepreciacionContableAcumulada += cuentaDetalle[index].DepreciacionContableAcumulada;
                        }
                        $(tfoot).find('th').eq(1).html(maskNumero(moi));
                        $(tfoot).find('th').eq(2).html(maskNumero(altas));
                        $(tfoot).find('th').eq(3).html(maskNumero(overhaul));
                        $(tfoot).find('th').eq(5).html(maskNumero(montoCancelacion));
                        $(tfoot).find('th').eq(5).addClass('numeroRojo');
                        $(tfoot).find('th').eq(7).html(maskNumero(montoBaja));
                        $(tfoot).find('th').eq(7).addClass('numeroRojo');
                        $(tfoot).find('th').eq(9).html(maskNumero(depMensual));
                        $(tfoot).find('th').eq(12).html(maskNumero(depAcumuladaAñoAnterior));
                        $(tfoot).find('th').eq(13).html(maskNumero(depAñoActual));
                        $(tfoot).find('th').eq(14).html(maskNumero(depBaja));
                        $(tfoot).find('th').eq(15).html(maskNumero(DepreciacionContableAcumulada));


                        //         /*15*/{ data: 'DepreciacionAcumuladaAñoAnterior', title: 'Dep. contable acumulada al 31/12/' },
                        // /*16*/{ data: 'DepreciacionAñoActual', title: 'Dep. contable del ejercicio a Diciembre' },
                        // /*17*/{ data: 'BajaDepreciacion', title: 'Baja de DEPN' },
                        // /*18*/{ data: 'DepreciacionContableAcumulada', title: 'Dep. contable acumulada a diciembre' },
                    }
                }
            });
        }

        function initTblAFDiferenciasSaldosDetalles() {
            tblAFDiferenciasSaldosDetalle.DataTable({
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,

                columns: [
                    { data: 'CC', title: 'CC', className: 'text-center' },
                    { data: 'NumEconomico', title: '# Económico', className: 'text-center' },
                    { data: 'MontoMovPol', title: 'Saldo', className: 'text-right' },
                    { data: 'MontoSalCont', title: 'Contabilidad', className: 'text-right' },
                    { data: 'Diferencia', title: 'Diferencia', className: 'text-right' }
                ],

                columnDefs: [
                    {
                        targets: [2, 3, 4],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                },

                drawCallback: function (settings) {
                },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).find('th').addClass('text-center');

                    $(thead).find('th').eq(2).text(tipoDiferencia);
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    $(tfoot).find('th').removeClass('dt-body-left');
                    $(tfoot).find('th').removeClass('dt-body-center');
                    $(tfoot).find('th').removeClass('text-center');

                    let totalMOI = 0;
                    let totalContabilidad = 0;
                    let totalDiferencia = 0;

                    data.forEach(function (element) {
                        totalMOI += element.MontoMovPol;
                        totalContabilidad += element.MontoSalCont;
                        totalDiferencia += element.Diferencia;
                    });

                    $(tfoot).find('th').eq(1).text(maskNumero(totalMOI));
                    $(tfoot).find('th').eq(2).text(maskNumero(totalContabilidad));
                    $(tfoot).find('th').eq(3).text(maskNumero(totalDiferencia));
                }
            });
        }

        function initTblAFDiferenciasSaldos() {
            tblAFDiferenciasSaldos.DataTable({
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],

                columns: [
                    { data: 'Año', title: 'Año', className: 'dt-body-center' },
                    { data: 'Mes', title: 'Mes', className: 'dt-body-center' },
                    { data: 'MontoMovPol', title: 'Saldo', className: 'dt-body-right' },
                    { data: 'MontoSalCont', title: 'Contabilidad', className: 'dt-body-right' },
                    { data: 'Diferencia', title: 'Diferencia', className: 'dt-body-right' }
                ],

                columnDefs: [
                    {
                        targets: [1],
                        render: function (data, type, row) {
                            return data.charAt(0).toUpperCase() + data.slice(1)
                        }
                    },
                    {
                        targets: [2, 3],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [2, 3, 4],
                        render: function (data, type, row) {
                            if (data != 0) {
                                return '<span class="difDetalleMensual_saldos" data-cuenta="' + row.Cuenta + '" data-año="' + row.Año + '" data-mes="' + row.Mes + '">' + maskNumero(data) + '</span>'
                            }
                            else {
                                return maskNumero(data);
                            }
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    // $(row).find('td').eq(0).addClass('text-center');
                    // $(row).find('td').eq(0).nextAll().addClass('text-right');
                },

                drawCallback: function (settings) {
                },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).find('th').addClass('text-center');
                    $(thead).find('th').eq(2).text(tipoDiferencia);
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    $(tfoot).find('th').removeClass('dt-body-left');
                    $(tfoot).find('th').removeClass('dt-body-center');

                    let totalMontoCalculado = 0;
                    let totalMontoContabilidad = 0;
                    let totalDif = 0;

                    data.forEach(function (element, index, array) {
                        totalMontoCalculado += element.MontoMovPol;
                        totalMontoContabilidad += element.MontoSalCont;
                        totalDif += element.Diferencia;
                    });

                    $(tfoot).find('th').eq(1).text(maskNumero(totalMontoCalculado));
                    $(tfoot).find('th').eq(2).text(maskNumero(totalMontoContabilidad));
                    $(tfoot).find('th').eq(3).text(maskNumero(totalDif));
                }
            });
        }

        function formatoMoneda(control) {
            if (control.val() == '') {
            } else {
                control.val(maskNumero(unmaskNumero(control.val())));
            }
        }

        function getResumen() {
            fecha = moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').toISOString(true);
            añoFechaHasta = (moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').year());
            console.log('añoFechaHasta: ' + añoFechaHasta);
            $.get('/ActivoFijo/Resumen', {
                // fechaSaldoAnterior: moment(txtSaldoFechaAl.val(), 'DD/MM/YYYY').toISOString(true),
                fechaHasta: moment(txtSaldoFechaAltaBaja.val(), 'DD/MM/YYYY').toISOString(true)
            }).always().then(response => {
                if (response.success) {

                    if (idEmpresa.val() == 6) {
                        AddRows(tblSaldosPeru, response.peru);
                        AddRows(tblDepPeru, response.peru);
                    } else if (idEmpresa.val() == 3) {
                        AddRows(tblSaldosColombia, response.colombia);
                        AddRows(tblDepColombia, response.colombia);
                    } else {
                        tblSaldosTotales = response.items;
                        tblTotalizadoresTotales = response.Totalizadores;
                        AddRows(tblSaldos, response.items.Saldos);
                        AddRows(tblDepreciacion, response.items.Depreciaciones);
                        AddRows(tblTotalizadores, response.Totalizadores.Detalles);
                        if (idEmpresa.val() == 1) {
                            AddRows(tblSaldosColombia, response.colombia);
                            AddRows(tblDepColombia, response.colombia);
                            AddRows(tblSaldosPeru, response.peru);
                            AddRows(tblDepPeru, response.peru);
                        }
                    }
                    btnDescargarExcel.attr('disabled', false);
                    btnImprimir.attr('disabled', false);
                } else {
                    btnDescargarExcel.attr('disabled', true);
                    alert('ERROR: ' + response.message);
                }
            }, error => {
                btnDescargarExcel.attr('disabled', true);
                alert('SERVER ERROR: ' + response.message);
            });
        }

        function GetDetalle(cuenta) {
            $.get('/ActivoFijo/DetalleCuenta', {
                fechaHasta: fecha,
                cuenta: cuenta
            }).always().then(response => {
                if (response.success) {
                    console.log(response.items);
                    cuentaDetalle = response.items;
                    AddRows(tblAFDetalle, response.items);
                } else {
                    // alert('ERROR: ' + response.message);
                }
            }, error => {
                alert('SERVER ERROR: ' + response.message);
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function GetDepreciacionCuenta() {
            $.get('/ActivoFijo/GetDepreciacionCuenta', {

            }).always().then(response => {
                if (response.success) {
                    var cuentas = 0;
                    $('#modalDepreciacionCuentas').find('.modal-body').empty();
                    for (let index = 0; index < response.items.length; index++) {
                        console.log(index);
                        $('#modalDepreciacionCuentas').find('.modal-body').append
                            (
                                '<div class="row">' +
                                '<div class="col-sm-6">' +
                                '<label id="idDepCuenta' + index + '" data-id="' + response.items[index].Id + '">' + response.items[index].Cuenta + ': ' + response.items[index].Descripcion + '</label>' +
                                '</div>' +
                                '</div>' +
                                '<div class="row">' +
                                '<div class="col-sm-6">' +
                                '<div class="input-group">' +
                                '<span class="input-group-addon">% Depreciación</span>' +
                                '<input type="text" class="form-control" id="txtPorcentajeDepreciacion' + response.items[index].Id + '" value="' + response.items[index].PorcentajeDepreciacion * 100 + '" />' +
                                '</div>' +
                                '</div>' +
                                '<div class="col-sm-6">' +
                                '<div class="input-group">' +
                                '<span class="input-group-addon">Meses de depreciación</span>' +
                                '<input type="text" class="form-control" id="txtMesesDepreciacion' + response.items[index].Id + '" value="' + response.items[index].MesesDeDepreciacion + '" />' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '<br />'
                            );
                        cuentas++;
                    }
                    btnModificarDepCuentas.data('cuentas', cuentas);
                    modalDepreciacionCuentas.modal('show');
                    console.log(btnModificarDepCuentas.data('cuentas'));
                }
                else {

                }
            }, error => {
                alert('SERVER ERROR: ' + response.message);
            })
        }

        function ModificarDepreciacionCuenta(objDepMaquina) {
            $.post('/ActivoFijo/ModificarDepreciacionCuenta', {
                depCuentas: objDepMaquina
            }).always().then(response => {
                if (response.success) {
                    modalDepreciacionCuentas.modal('hide');
                }
                else {
                    alert('ERROR: ' + response.message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + response.message);
            });
        }

        function ObtenerDiferenciasSaldos(cuenta) {
            $.get('/ActivoFijo/ObtenerDiferenciasSaldos', {
                cuenta: cuenta
            }).always().then(response => {
                datosDifSaldos = response;
                AddRows(tblAFDiferenciasSaldos, response);
                modalDetalleDiferenciaSaldos.modal('show');
            }, error => {
                alert('ERROR AL CONSULTAR EL SERVIDOR: ', error.statusText);
            })
        }

        function ObtenerDiferenciasDep(cuenta) {
            $.get('/ActivoFijo/ObtenerDiferenciasDep', {
                cuenta: cuenta
            }).always().then(response => {
                datosDifSaldos = response;
                AddRows(tblAFDiferenciasSaldos, response);
                modalDetalleDiferenciaSaldos.modal('show');
            }, error => {
                alert('ERROR AL CONSULTAR EL SERVIDOR: ', error.statusText);
            })
        }

        function verReporte() {
            let fechaSaldoActual = txtSaldoFechaAltaBaja.val();
            var path = `/Reportes/Vista.aspx?idReporte=181&pFechaSaldoActual=${fechaSaldoActual}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

    }

    $(document).ready(() => {
        Maquinaria.ActivoFijo = new Index();
    })

        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();