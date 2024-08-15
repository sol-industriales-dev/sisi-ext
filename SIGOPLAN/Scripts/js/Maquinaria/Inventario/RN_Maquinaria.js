(() => {
    $.namespace('Maquinaria.Capturas.MaquinariaRentada');
    MaquinariaRentada = function () {

        // Elementos de la pantalla resumen de rentas (Grid maquinas rentadas)
        // Tabla
        const tblMaquinasRentadas = $('#tblMaquinasRentadas');
        // Filtro
        const cboFiltroAreaCuenta = $('#cboFiltroAreaCuenta');
        const cboFiltroCentroCosto = $('#cboFiltroCentroCosto');
        const txtFiltroPeriodoDesde = $('#txtFiltroPeriodoDesde');
        const txtFiltroPeriodoHasta = $('#txtFiltroPeriodoHasta');
        // Botones
        const btnFiltroBuscar = $('#btnFiltroBuscar');
        const btnFiltroOrdenarUrgentes = $('#btnFiltroOrdenarUrgentes');
        const btnAbrirModalRentaNueva = $('#btnAbrirModalRentaNueva');
        const btnReporteTiempoUtilizadoVs = $('#btnReporteTiempoUtilizadoVs');

        // Elementos del modal capturar/editar/renovar renta
        const txtRentaId = $('#txtRentaId');
        const txtFolio = $('#txtFolio');
        const txtPeriodoInicial = $('#txtPeriodoInicial');
        const txtIdProveedor = $('#txtIdProveedor');
        const txtIdAreaCuenta = $('#txtIdAreaCuenta');
        // Inputs maquina
        const cboRentaCentroCosto = $('#cboRentaCentroCosto');
        const txtRentaEquipo = $('#txtRentaEquipo');
        const txtRentaNumeroSerie = $('#txtRentaNumeroSerie');
        const txtRentaModelo = $('#txtRentaModelo');
        const txtRentaProveedor = $('#txtRentaProveedor');
        // Inputs información general
        const txtRentaObra = $('#txtRentaObra');
        const txtRentaNumeroFactura = $('#txtRentaNumeroFactura');
        const txtRentaDepGarantia = $('#txtRentaDepGarantia');
        const cbRentaTramiteDg = $('#cbRentaTramiteDg');
        const txtRentaNotaCredito = $('#txtRentaNotaCredito');
        const cbRentaAplicaNc = $('#cbRentaAplicaNc');
        const txtRentaBaseHoraMensual = $('#txtRentaBaseHoraMensual');
        const txtRentaPeriodoDel = $('#txtRentaPeriodoDel');
        const txtRentaPeriodoAl = $('#txtRentaPeriodoAl');
        const txtRentaHorometroInicial = $('#txtRentaHorometroInicial');
        const txtRentaHorometroFinal = $('#txtRentaHorometroFinal');
        const txtRentaHorasTrabajadas = $('#txtRentaHorasTrabajadas');
        const txtRentaHorasExtras = $('#txtRentaHorasExtras');
        const txtRentaTotalHorasExtras = $('#txtRentaTotalHorasExtras');
        const txtRentaPrecioPorMes = $('#txtRentaPrecioPorMes');
        const txtRentaSeguroPorMes = $('#txtRentaSeguroPorMes');
        const txtRentaIVA = $('#txtRentaIVA');
        const txtRentaTotalRenta = $('#txtRentaTotalRenta');
        const txtRentaOrdenCompra = $('#txtRentaOrdenCompra');
        const txtRentaContraRecibo = $('#txtRentaContraRecibo');
        const txtRentaAnotaciones = $('#txtRentaAnotaciones');
        const cboRentaMoneda = $('#cboRentaMoneda');
        // Inputs diferencia horas
        const txtRentaDifHorasExtras = $('#txtRentaDifHorasExtras');
        const txtRentaDifContraRecibo = $('#txtRentaDifContraRecibo');
        const txtRentaDifNumeroFactura = $('#txtRentaDifNumeroFactura');
        const txtRentaDifOrdenCompra = $('#txtRentaDifOrdenCompra');
        const txtRentaDifFechaContraRecibo = $('#txtRentaDifFechaContraRecibo');
        const cbRentaDifHoras = $('#cbRentaDifHoras');
        // Inputs cargo por daño
        const txtRentaDañoNumeroFactura = $('#txtRentaDañoNumeroFactura');
        const txtRentaDañoCompra = $('#txtRentaDañoCompra');
        const cbRentaCargoDaño = $('#cbRentaCargoDaño');
        // Inputs fletes
        const txtRentaFletesNumeroFactura = $('#txtRentaFletesNumeroFactura');
        const txtRentaFletesOrdenCompra = $('#txtRentaFletesOrdenCompra');
        const cbRentaFletes = $('#cbRentaFletes');
        // Botones
        const btnRentaLimpiarCampos = $('#btnRentaLimpiarCampos');
        const btnRentaRegistrar = $('#btnRentaRegistrar');

        // Modales
        const modalOpcionesTblMaquinasRentadas = $('#modalOpcionesTblMaquinasRentadas');
        const modalCapturaRentaMaquinaria = $('#modalCapturaRentaMaquinaria');
        const modalCapturaRentaMaquinariaTitle = $(modalCapturaRentaMaquinaria.find('.modal-title'));

        // iFrame
        const report = $('#report');

        // Eventos
        cboFiltroAreaCuenta.on('change', function () {
            initCboFiltroCentroCosto($(this));
        });

        btnFiltroBuscar.on('click', function () {
            getMaquinasRentadas();
        });

        btnFiltroOrdenarUrgentes.on('click', function () {
            ordenarTablaPorCierranPronto();
        });

        btnReporteTiempoUtilizadoVs.on('click', function () {
            ReporteTiempoRequeridoVsUtilizado($(this).val());
        });

        cboRentaCentroCosto.on('change', function () {
            getInformacionCentroCosto($(this).val());
        });

        txtRentaPeriodoDel.on('change', function () {
            getHorometros(moment($(this).val(), 'DD/MM/YYYY').toISOString(true), moment($(txtRentaPeriodoAl).val(), 'DD/MM/YYYY').toISOString(true), cboRentaCentroCosto.find('option:selected').text(), txtRentaHorometroInicial.val(), txtRentaHorometroFinal.val());
        });

        txtRentaPeriodoAl.on('change', function () {
            getHorometros(moment($(txtRentaPeriodoDel).val(), 'DD/MM/YYYY').toISOString(true), moment($(this).val(), 'DD/MM/YYYY').toISOString(true), cboRentaCentroCosto.find('option:selected').text(), txtRentaHorometroInicial.val(), txtRentaHorometroFinal.val());
        });

        cbRentaDifHoras.on('switchChange.bootstrapSwitch', function () {
            switchDiferenciasHoras();
        });

        cbRentaFletes.on('switchChange.bootstrapSwitch', function () {
            switchFletes();
        });

        cbRentaCargoDaño.on('switchChange.bootstrapSwitch', function () {
            switchCargoDaño();
        });

        btnRentaLimpiarCampos.on('click', function () {
            initInputsRenta($(this).data('tipo_renta'), null);
        });

        btnRentaRegistrar.on('click', function () {
            prepararRegistro($(this).data('tipo_renta'));
        });

        txtRentaBaseHoraMensual.on('change', function () {
            sumarHoras();
        });

        txtRentaHorometroInicial.on('change', function () {
            sumarHoras();
        });

        txtRentaHorometroFinal.on('change', function () {
            sumarHoras();
        })

        txtRentaPrecioPorMes.on('change', function () {
            formatoMoneda($(this));
            sumarRenta();
        });

        txtRentaDepGarantia.on('change', function () {
            formatoMoneda($(this));
        });

        $('.soloNumero2D').on('paste', function (e) {
            permitePegarSoloNumero2D($(this), e);
        });

        $('.soloNumero2D').on('keypress', function (event) {
            aceptaSoloNumero2D($(this), event);
        });

        $('.soloNumero0D').on('keypress', function (event) {
            aceptaSoloNumero0D($(this), event);
        });

        $('.soloNumero0D').on('paste', function (e) {
            permitePegarSoloNumero0D($(this), e);
        });

        txtRentaSeguroPorMes.on('change', function () {
            formatoMoneda($(this));
            sumarRenta();
        });

        txtRentaTotalHorasExtras.on('change', function () {
            formatoMoneda($(this));
        });

        modalOpcionesTblMaquinasRentadas.on('hidden.bs.modal', function () {
            $(this).find('.modal-title').text('Opciones');
        });

        modalCapturaRentaMaquinaria.on('hidden.bs.modal', function () {
            $(this).find('.modal-title').text('Nueva renta');
        });

        $(tblMaquinasRentadas).on('click', '.btnOpcionesMaquinaRentada', function () {
            prepararModalOpcionesMaquinaRentada($(this));
        });

        $(modalOpcionesTblMaquinasRentadas).on('click', '#btnTerminarMaquinaRentada', function () {
            terminarRenta($(this), false);
        });

        $(modalOpcionesTblMaquinasRentadas).on('click', '#RenovarMaquinaRentada', function () {
            prepararModalCapturaRenta($(this), TipoRenta.Renovar);
        });

        $(modalOpcionesTblMaquinasRentadas).on('click', '#btnTerminarRenovarMaquinaRentada', function () {
            terminarRenta($(this), true);
        });

        $('#tblMaquinasRentadas').on('click', '.btnEditarMaquinaRentada', function () {
            prepararModalCapturaRenta($(this), TipoRenta.Editar);
        });

        btnAbrirModalRentaNueva.on('click', function () {
            prepararModalCapturaRenta(null, TipoRenta.Nueva);
        });

        // Variables globales
        // Fechas
        const _fechaActual = new Date();
        const _añoActual = _fechaActual.getFullYear();
        const _mesActual = _fechaActual.getMonth();
        const _diaActual = _fechaActual.getDate();
        // Tipos de moneda
        const Monedas = {
            MX: '1',
            US: '2',
            CO: '3'
        };
        // Tipos de captura
        const TipoRenta = {
            Nueva: '1',
            Editar: '2',
            Renovar: '3'
        };

        (function init() {
            initFechas();
            initComboBox();
            initCheckBox();
            initTblMaquinasRentadas();
        })();

        // Init fechas
        function initFechas() {
            txtFiltroPeriodoDesde.datepicker({
            }).datepicker('setDate', new Date(_añoActual, _mesActual, _diaActual));

            txtFiltroPeriodoHasta.datepicker({
            }).datepicker('setDate', new Date(_añoActual, _mesActual, _diaActual));

            txtRentaPeriodoDel.datepicker({}).datepicker('setDate', new Date());
            txtRentaPeriodoAl.datepicker({}).datepicker('setDate', new Date());
            txtRentaDifFechaContraRecibo.datepicker({}).datepicker('setDate', new Date());
        }

        // Init comboBox
        function initComboBox() {
            // Busqueda por una AreaCuenta
            cboFiltroAreaCuenta.attr('multiple', true);
            cboFiltroAreaCuenta.fillCombo('/MaquinariaRentada/GetAreasCuentaPorUsuario', null, false, 'Todos');
            convertToMultiselect('#cboFiltroAreaCuenta');
            // Busqueda por varioas AreasCuenta fin
            cboRentaCentroCosto.fillCombo('/MaquinariaRentada/GetCentrosCosto', null, false, null);
        }

        function initCboFiltroCentroCosto(control) {
            // console.log('initCboFiltroCentroCosto: IdAC = ' + $(control).find('option:selected').data('prefijo'));
            if ($(control).val() != '') {
                cboFiltroCentroCosto.attr('multiple', true);
                cboFiltroCentroCosto.fillCombo('/MaquinariaRentada/GetCentrosCostoRentados', { idAreasCuenta: getValoresMultiples('#cboFiltroAreaCuenta') }, false, 'Todos');
                convertToMultiselect('#cboFiltroCentroCosto');
            }
            else {
                // console.log('initCboFiltroCentroCosto: reset');
                cboFiltroCentroCosto.resetCombo();
            }
        }

        // Init checkBox
        function initCheckBox() {
            cbRentaTramiteDg.bootstrapSwitch();
            cbRentaAplicaNc.bootstrapSwitch();
            cbRentaDifHoras.bootstrapSwitch();
            cbRentaCargoDaño.bootstrapSwitch();
            cbRentaFletes.bootstrapSwitch();
        }

        // Habilitar inputs
        function switchFletes() {
            if (cbRentaFletes.bootstrapSwitch('state')) {
                txtRentaFletesNumeroFactura.prop('disabled', false);
                txtRentaFletesOrdenCompra.prop('disabled', false);
            } else {
                initInputsFletes();
                txtRentaFletesNumeroFactura.prop('disabled', true);
                txtRentaFletesOrdenCompra.prop('disabled', true);
            }
        }

        function switchCargoDaño() {
            if (cbRentaCargoDaño.bootstrapSwitch('state')) {
                txtRentaDañoNumeroFactura.prop('disabled', false);
                txtRentaDañoCompra.prop('disabled', false);
            } else {
                initInputsCargoDaño();
                txtRentaDañoNumeroFactura.prop('disabled', true);
                txtRentaDañoCompra.prop('disabled', true);
            }
        }

        function switchDiferenciasHoras() {
            if (cbRentaDifHoras.bootstrapSwitch('state')) {
                txtRentaDifContraRecibo.prop('disabled', false);
                txtRentaDifFechaContraRecibo.prop('disabled', false);
                txtRentaDifHorasExtras.prop('disabled', false);
                txtRentaDifNumeroFactura.prop('disabled', false);
                txtRentaDifOrdenCompra.prop('disabled', false);
            } else {
                initInputsDifHoras();
                txtRentaDifContraRecibo.prop('disabled', true);
                txtRentaDifFechaContraRecibo.prop('disabled', true);
                txtRentaDifHorasExtras.prop('disabled', true);
                txtRentaDifNumeroFactura.prop('disabled', true);
                txtRentaDifOrdenCompra.prop('disabled', true);
            }
        }

        // DataTable
        function initTblMaquinasRentadas() {
            tblMaquinasRentadas.DataTable({
                order: [[8, "asc"]],
                language: dtDicEsp,
                columns: [
                    { data: 'Folio', title: 'Folio' },
                    { data: 'CentroCosto', title: 'Centro de costo' },
                    { data: 'AreaCuenta', title: 'Área cuenta' },
                    { data: 'PeriodoDel', title: 'Periodo del' },
                    { data: 'PeriodoA', title: 'Periodo al' },
                    { data: 'TotalRenta', title: 'Total de la renta' },
                    { data: 'OrdenCompra', title: 'Orden compra' },
                    { data: null, title: 'Opciones' },
                    { data: 'DiasParaTerminar' }
                ],
                columnDefs: [
                    {
                        targets: [0],
                        className: 'text-nowrap',
                        render: function (data, type, row) {
                            let _botonEditar = '';
                            // Se modificó para siempre permitir la edición.
                            // if (!row.RentaTerminada){
                            //     _botonEditar = '<button type="button" data-folio_maquina_rentada="' + row.Folio + '" data-id_maquina_rentada="' + row.Id + '" data-renta_terminada="false" class="btn btn-success btnEditarMaquinaRentada"><i class="glyphicon glyphicon-edit"></i></button> ';
                            // }
                            _botonEditar = '<button type="button" data-folio_maquina_rentada="' + row.Folio + '" data-id_maquina_rentada="' + row.Id + '" data-renta_terminada="false" class="btn btn-success btnEditarMaquinaRentada"><i class="glyphicon glyphicon-edit"></i></button> ';
                            let _folio = _botonEditar + '<strong>' + data + '</strong>';

                            if (row.DifHora) { _folio += ' <span class="label label-warning">Diferencia de horas</span>' }
                            if (row.CargoDaño) { _folio += ' <span class="label label-danger">Existe cargo por daño</span>' }
                            if (row.Fletes) { _folio += ' <span class="label label-primary">Existe fletes</span>' }

                            return _folio;
                        }
                    },
                    {
                        targets: [5],
                        className: 'text-right',
                        render: function (data, type, row) {
                            let _total = maskNumero(data);
                            if (row.IdTipoMoneda == Monedas.MX) {
                                _total += ' MX';
                            }
                            if (row.IdTipoMoneda == Monedas.US) {
                                _total += ' US';
                            }
                            if (row.IdTipoMoneda == Monedas.CO) {
                                _totla += ' CO';
                            }
                            return _total;
                        }
                    },
                    {
                        targets: [3, 4],
                        className: 'text-center',
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        targets: [7],
                        orderable: false,
                        className: 'text-center',
                        render: function (data, type, row) {
                            if (row.RentaTerminada) {
                                return '<button type="button" data-folio_maquina_rentada="' + row.Folio + '" data-id_maquina_rentada="' + row.Id + '" data-renta_terminada="true" class="btn btn-success btnOpcionesMaquinaRentada"><i class="glyphicon glyphicon-plus"></i></button>';
                            }
                            else {
                                return '<button type="button" data-folio_maquina_rentada="' + row.Folio + '" data-id_maquina_rentada="' + row.Id + '" data-renta_terminada="false" class="btn btn-success btnOpcionesMaquinaRentada"><i class="glyphicon glyphicon-plus"></i></button>';
                            }
                        }
                    },
                    {
                        targets: [8],
                        visible: false,
                    }
                ],
                createdRow: function (row, data, dataIndex) {
                    // console.log('DiasPaTerminar: ' + data.DiasParaTerminar);
                    if (data.DiasParaTerminar <= 0 && !data.RentaTerminada) {
                        $(row).addClass('TerminarRentaHoy');
                    }
                    if (data.DiasParaTerminar > 0 && data.DiasParaTerminar <= 2 && !data.RentaTerminada) {
                        $(row).addClass('TerminarRentaProxima');
                    }
                },
                drawCallback: function (settings) {
                },
                headerCallback: function (thead, data, start, end, display) {
                    // Actualiza el header
                    $(thead).closest('thead').addClass('bg-table-header');
                    $(thead).find('th').addClass('text-center');
                    //$(thead).find('th').eq(2).html('');
                },
                footerCallback: function (tfoot, data, start, end, display) {
                    // Actualiza el footer
                    //$(tfoot).find('th').eq(1).html('');
                }
            });
        }

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).order([8, 'asc']).draw();
        }

        function ordenarTablaPorCierranPronto() {
            var tabla = tblMaquinasRentadas.DataTable();
            tabla.order([8, 'asc']).draw();
        }

        // Preparar modales
        function prepararModalOpcionesMaquinaRentada(boton) {
            modalOpcionesTblMaquinasRentadas.find('.modal-title').empty().append('Opciones renta con folio: ' + '<strong>' + $(boton).data('folio_maquina_rentada') + '</strong>');
            // console.log('Renta terminada: ' + $(boton).data('renta_terminada'));
            if (!$(boton).data('renta_terminada')) {
                modalOpcionesTblMaquinasRentadas.find('.modal-body').empty().append(
                    '<button id="btnTerminarRenovarMaquinaRentada" type="button" class="btn btn-success" data-id_maquina_rentada="' + $(boton).data('id_maquina_rentada') + '"><span class="glyphicon glyphicon-retweet"></span> Terminar y renovar</button> ' +
                    '<button id="btnTerminarMaquinaRentada" type="button" class="btn btn-primary" data-id_maquina_rentada="' + $(boton).data('id_maquina_rentada') + '"><span class="glyphicon glyphicon-ok-circle"></span> Terminar</button>'
                );
            }
            else {
                modalOpcionesTblMaquinasRentadas.find('.modal-body').empty().append(
                    '<button id="RenovarMaquinaRentada" type="button" class="btn btn-primary" data-id_maquina_rentada="' + $(boton).data('id_maquina_rentada') + '"><span class="glyphicon glyphicon-retweet"></span> Renovar</button>'
                );
            }
            modalOpcionesTblMaquinasRentadas.modal('show');
        }

        function prepararModalCapturaRenta(boton, tipoRenta, TerminarRenovar) {
            cerrarModales();

            if (tipoRenta == TipoRenta.Nueva) {
                initInputsRenta(tipoRenta, null);
                modalCapturaRentaMaquinariaTitle.text('Nueva renta');
                btnRentaRegistrar.find('#btnRentaRegistrarText').text(' Registrar');
                btnRentaLimpiarCampos.show();
                modalCapturaRentaMaquinaria.modal('show');
            }
            if (tipoRenta == TipoRenta.Editar) {
                txtRentaId.val($(boton).data('id_maquina_rentada'));
                $(btnRentaLimpiarCampos).hide();
                modalCapturaRentaMaquinariaTitle.text('Actualizar renta folio: ' + $(boton).data('folio_maquina_rentada'));
                btnRentaRegistrar.find('#btnRentaRegistrarText').text('Actualizar');
                getMaquinaRentada(txtRentaId.val(), tipoRenta);
            }
            if (tipoRenta == TipoRenta.Renovar) {
                txtRentaId.val($(boton).data('id_maquina_rentada'));
                btnRentaRegistrar.find('#btnRentaRegistrarText').text('Renovar');
                $(btnRentaLimpiarCampos).hide();
                modalCapturaRentaMaquinariaTitle.text('Renovar renta');
                if (!TerminarRenovar) {
                    // console.log('IF terminarRenovar');
                    getMaquinaRentada(txtRentaId.val(), tipoRenta);
                }
            }
        }

        function cerrarModales() {
            $('.modal').modal('hide');
        }

        // Preparar registros
        function prepararRegistro(tipoRenta, terminarRenovar) {
            if (validarCaptura()) {
                if (tipoRenta == TipoRenta.Nueva) {
                    AlertaAceptarRechazarNormal(
                        'Registrar nueva renta',
                        'Se registrará la nueva renta del centro de costo: ' + cboRentaCentroCosto.find('option:selected').text() + ' ¿Desea continuar?',
                        () => registrar(tipoRenta)
                    );
                }
                if (tipoRenta == TipoRenta.Editar) {
                    AlertaAceptarRechazarNormal(
                        'Actualizar renta',
                        'Se actualizara la información de la renta. ¿Desea continuar?',
                        () => registrar(tipoRenta)
                    );
                }
                if (tipoRenta == TipoRenta.Renovar) {
                    if (terminarRenovar) {
                        AlertaAceptarRechazarNormal(
                            'Confirmación',
                            'Se terminó la renta. Ahora puede renovarla',
                            () => modalCapturaRentaMaquinaria.modal('show')
                        );
                    } else {
                        AlertaAceptarRechazarNormal(
                            'Renovar renta',
                            'Se renovará la renta. ¿Desea continuar?',
                            () => registrar(tipoRenta)
                        );
                    }
                }
            }
        }

        // Validaciones
        function validarFiltroBusquedaMaquinasRentadas() {
            let valido = true;

            if (cboFiltroAreaCuenta.val() == '') { valido = false; }
            if (cboFiltroCentroCosto.val() == '') { valido = false; }
            if (!moment(txtFiltroPeriodoDesde.val(), 'DD/MM/YYYY').isValid()) { valido = false; }
            if (!moment(txtFiltroPeriodoHasta.val(), 'DD/MM/YYYY').isValid()) { valido = false; }

            return valido;
        }

        function validarCaptura() {
            let valido = true;

            if (txtRentaObra.val() == '') { valido = false; }
            if (txtRentaEquipo.val() == '') { valido = false; }
            if (txtRentaNumeroSerie.val() == '') { valido = false; }
            if (txtRentaProveedor.val() == '') { valido = false; }
            if (cboRentaCentroCosto.val() == '') { valido = false; }
            if (!validarCampo(txtRentaPeriodoDel)) { valido = false; }
            if (!validarCampo(txtRentaPeriodoAl)) { valido = false; }
            if (!validarCampo(txtRentaPrecioPorMes)) { valido = false; }

            return valido;
        }

        function validarCboOptionSelected(valor) {
            let valido = true;
            if (valor == '') {
                valido = false;
                txtRentaPeriodoDel.prop('disabled', true);
                txtRentaPeriodoAl.prop('disabled', true);
            }
            return valido;
        }

        function validarPeriodos() {
            let valido = true;
            if (txtRentaPeriodoDel.val() == '' || txtRentaPeriodoAl.val() == '') { valido = false; }
            return valido;
        }

        // Operaciones locales
        function sumarHoras() {
            let horometroFinalTemp = txtRentaHorometroFinal.val();
            if (horometroFinalTemp == '') { horometroFinalTemp = 0; }
            if (txtRentaBaseHoraMensual.val() == '') { txtRentaBaseHoraMensual.val(0); }
            if (txtRentaHorasTrabajadas.val() == '') { txtRentaHorasTrabajadas.val(0); }
            if (txtRentaHorometroInicial.val() == '') { txtRentaHorometroInicial.val(0); }
            let suma = parseFloat(horometroFinalTemp) - parseFloat(txtRentaHorometroInicial.val());
            if (suma < 0) { suma = 0 }
            txtRentaHorasTrabajadas.val(suma);
            suma = parseFloat(txtRentaHorasTrabajadas.val()) - parseFloat(txtRentaBaseHoraMensual.val());
            if (suma < 0) { suma = 0; }
            txtRentaHorasExtras.val(parseFloat(suma).toFixed(0));
        }

        function sumarRenta() {
            let precioPorMes = unmaskNumero(txtRentaPrecioPorMes.val());
            let seguroPorMes = unmaskNumero(txtRentaSeguroPorMes.val());
            if (precioPorMes == '') { precioPorMes = 0; }
            if (seguroPorMes == '') { seguroPorMes = 0; }
            let ivaPrecioMes = parseFloat(precioPorMes * .16).toFixed(2);
            let ivaSeguro = parseFloat(seguroPorMes * .16).toFixed(2);
            txtRentaIVA.val(maskNumero(parseFloat(ivaPrecioMes) + parseFloat(ivaSeguro)));
            let iva = unmaskNumero(txtRentaIVA.val());
            let total = parseFloat(iva) + parseFloat(precioPorMes) + parseFloat(seguroPorMes);
            // console.log('total $ renta: ' + total);
            total = parseFloat(total).toFixed(2);
            txtRentaTotalRenta.val(maskNumero(total));
        }

        function formatoMoneda(control) {
            if (control.val() == '') {
            } else {
                control.val(maskNumero(control.val()));
            }
        }

        // Inicializar inputs y objetos nueva/editar/renovar renta
        function initInputsRenta(tipoRenta, maquinaRentada) {
            if (tipoRenta == TipoRenta.Nueva) {
                txtRentaId.val(0);
                txtFolio.val('');
                txtPeriodoInicial.val(0);
                txtIdProveedor.val(0);
                txtIdAreaCuenta.val(0);
                // Inputs maquina
                cboRentaCentroCosto.find('option:selected').prop('selected', false);
                cboRentaCentroCosto.val('');
                cboRentaCentroCosto.prop('disabled', false);
                txtRentaEquipo.val('');
                txtRentaNumeroSerie.val('');
                txtRentaModelo.val('');
                txtRentaProveedor.val('');
                // Inputs información general
                txtRentaObra.val('');
                txtRentaNumeroFactura.val('');
                txtRentaDepGarantia.val('');
                cbRentaTramiteDg.bootstrapSwitch('state', false);
                txtRentaNotaCredito.val('');
                cbRentaAplicaNc.bootstrapSwitch('state', false);
                txtRentaBaseHoraMensual.val('');
                txtRentaPeriodoDel.val('');
                txtRentaPeriodoAl.val('');
                txtRentaPeriodoDel.prop('disabled', true);
                txtRentaPeriodoAl.prop('disabled', true);
                txtRentaHorometroInicial.val('');
                txtRentaHorometroFinal.val('');
                txtRentaHorasTrabajadas.val('');
                txtRentaHorasExtras.val('');
                txtRentaTotalHorasExtras.val('');
                txtRentaPrecioPorMes.val('');
                txtRentaSeguroPorMes.val('');
                txtRentaIVA.val('');
                txtRentaTotalRenta.val('');
                txtRentaOrdenCompra.val('');
                txtRentaContraRecibo.val('');
                txtRentaAnotaciones.val('');
                cboRentaMoneda.find('option:selected').prop('selected', false);
                cboRentaMoneda.find('option[value="1"]').prop('selected', true);
                cboRentaMoneda.val('1');
                // Inputs diferencia horas
                txtRentaDifHorasExtras.val('');
                txtRentaDifContraRecibo.val('');
                txtRentaDifNumeroFactura.val('');
                txtRentaDifOrdenCompra.val('');
                txtRentaDifFechaContraRecibo.val('');
                cbRentaDifHoras.bootstrapSwitch('state', false);
                // Inputs cargo por daño
                txtRentaDañoNumeroFactura.val('');
                txtRentaDañoCompra.val('');
                cbRentaCargoDaño.bootstrapSwitch('state', false);
                // Inputs fletes
                txtRentaFletesNumeroFactura.val('');
                txtRentaFletesOrdenCompra.val('');
                cbRentaFletes.bootstrapSwitch('state', false);
                // Botones
                btnRentaLimpiarCampos.data('tipo_renta', tipoRenta);
                btnRentaRegistrar.data('tipo_renta', tipoRenta);
            }
            if (tipoRenta == TipoRenta.Editar || tipoRenta == TipoRenta.Renovar) {
                let depGarantia = maquinaRentada.DepGarantia;
                let totalHorasExtras = maquinaRentada.TotalHorasExtras;
                let precioPorMes = maquinaRentada.PrecioPorMes;
                let seguroPorMes = maquinaRentada.SeguroPorMes;
                let iva = maquinaRentada.IVA;
                let totalRenta = maquinaRentada.TotalRenta;

                if (depGarantia != '' && depGarantia != null) { depGarantia = maskNumero(depGarantia); }
                if (totalHorasExtras != '' && totalHorasExtras != null) { totalHorasExtras = maskNumero(totalHorasExtras); }
                if (precioPorMes != '' && precioPorMes != null) { precioPorMes = maskNumero(precioPorMes); }
                if (seguroPorMes != '' && seguroPorMes != null) { seguroPorMes = maskNumero(seguroPorMes); }
                if (iva != '' && iva != null) { iva = maskNumero(iva); }
                if (totalRenta != '' && totalRenta != null) { totalRenta = maskNumero(totalRenta); }

                txtRentaId.val(maquinaRentada.Id);
                txtFolio.val(maquinaRentada.Folio);
                // console.log('maquinaRentadaPeriodoInicial: ' + maquinaRentada.PeriodoInicial);
                // console.log('maquinaRentadaId: ' + maquinaRentada.Id);
                if (tipoRenta == TipoRenta.Renovar && maquinaRentada.PeriodoInicial == 0) {
                    // console.log('entroIF: ');
                    txtPeriodoInicial.val(maquinaRentada.Id);
                    // console.log('txtPeriodoInicial: ' + txtPeriodoInicial.val());
                } else {
                    // console.log('entroELSE: ');
                    txtPeriodoInicial.val(maquinaRentada.PeriodoInicial);
                    // console.log('txtPeriodoInicial: ' + txtPeriodoInicial.val());
                }
                txtIdProveedor.val(maquinaRentada.IdProveedor);
                txtIdAreaCuenta.val(maquinaRentada.IdAreaCuenta);
                // Inputs maquina
                cboRentaCentroCosto.find('option:selected').prop('selected', false);
                // cboRentaCentroCosto.val(maquinaRentada.IdCentroCosto); // Se cambia los el siguiente 'append' debido a que si la maquina esta dada de baja no aparece desde un principio.
                cboRentaCentroCosto.prop('disabled', true);
                //
                cboRentaCentroCosto.empty();
                cboRentaCentroCosto.append('<option value="' + maquinaRentada.IdCentroCosto + '" selected>' + maquinaRentada.CentroCosto + '</option>');
                cboRentaCentroCosto.val(maquinaRentada.IdCentroCosto);
                //
                txtRentaEquipo.val(maquinaRentada.Equipo);
                txtRentaNumeroSerie.val(maquinaRentada.NumeroSerie);
                txtRentaModelo.val(maquinaRentada.Modelo);
                txtRentaProveedor.val(maquinaRentada.Proveedor);
                // Inputs información general
                txtRentaObra.val(maquinaRentada.AreaCuenta);
                txtRentaNumeroFactura.val(maquinaRentada.NumeroFactura);
                txtRentaDepGarantia.val(depGarantia);
                cbRentaTramiteDg.bootstrapSwitch('state', maquinaRentada.TramiteDG);
                txtRentaNotaCredito.val(maquinaRentada.NotaCredito);
                cbRentaAplicaNc.bootstrapSwitch('state', maquinaRentada.AplicaNC);
                txtRentaBaseHoraMensual.val(maquinaRentada.BaseHoraMensual);
                txtRentaPeriodoDel.val(moment(maquinaRentada.PeriodoDel).format('DD/MM/YYYY'));
                txtRentaPeriodoAl.val(moment(maquinaRentada.PeriodoA).format('DD/MM/YYYY'));
                txtRentaPeriodoDel.prop('disbaled', false);
                txtRentaPeriodoAl.prop('disabled', false);
                txtRentaHorometroInicial.val(maquinaRentada.HorometroInicial);
                txtRentaHorometroFinal.val(maquinaRentada.HorometroFinal);
                txtRentaHorasTrabajadas.val(maquinaRentada.HorasTrabajadas);
                txtRentaHorasExtras.val(maquinaRentada.HorasExtras);
                txtRentaTotalHorasExtras.val(totalHorasExtras);
                txtRentaPrecioPorMes.val(precioPorMes);
                txtRentaSeguroPorMes.val(seguroPorMes);
                txtRentaIVA.val(iva);
                txtRentaTotalRenta.val(totalRenta);
                txtRentaOrdenCompra.val(maquinaRentada.OrdenCompra);
                txtRentaContraRecibo.val(maquinaRentada.ContraRecibo);
                txtRentaAnotaciones.val(maquinaRentada.Anotaciones);
                cboRentaMoneda.find('option:selected').prop('selected', false);
                cboRentaMoneda.val(maquinaRentada.IdTipoMoneda);
                // Inputs diferencia horas
                txtRentaDifHorasExtras.val(maquinaRentada.DifHoraExtra);
                txtRentaDifContraRecibo.val(maquinaRentada.DifContraRecibo);
                txtRentaDifNumeroFactura.val(maquinaRentada.DifFactura);
                txtRentaDifOrdenCompra.val(maquinaRentada.DifOrdenCompra);
                txtRentaDifFechaContraRecibo.val(moment(maquinaRentada.DifFechaContraRecibo).format('DD/MM/YYYY'));
                cbRentaDifHoras.bootstrapSwitch('state', maquinaRentada.DifHora);
                // Inputs cargo por daño
                txtRentaDañoNumeroFactura.val(maquinaRentada.CargoDañoFactura);
                txtRentaDañoCompra.val(maquinaRentada.CargoDañoOrdenCompra);
                cbRentaCargoDaño.bootstrapSwitch('state', maquinaRentada.CargoDaño);
                // Inputs fletes
                txtRentaFletesNumeroFactura.val(maquinaRentada.FletesFactura);
                txtRentaFletesOrdenCompra.val(maquinaRentada.FletesOrdenCompra);
                cbRentaFletes.bootstrapSwitch('state', maquinaRentada.Fletes);
                // Botones
                btnRentaLimpiarCampos.data('tipo_renta', tipoRenta);
                btnRentaRegistrar.data('tipo_renta', tipoRenta);
            }
            if (tipoRenta == TipoRenta.Renovar) {
                // Inputs información general
                txtRentaNumeroFactura.val('');
                txtRentaNotaCredito.val('');
                txtRentaPeriodoDel.val('');
                txtRentaPeriodoAl.val('');
                txtRentaPeriodoDel.prop('disbaled', false);
                txtRentaPeriodoAl.prop('disabled', false);
                txtRentaHorometroInicial.val('');
                txtRentaHorometroFinal.val('');
                txtRentaHorasTrabajadas.val('');
                txtRentaHorasExtras.val('');
                txtRentaTotalHorasExtras.val('');
                txtRentaOrdenCompra.val('');
                txtRentaContraRecibo.val('');
                // Inputs diferencia horas
                txtRentaDifHorasExtras.val('');
                txtRentaDifContraRecibo.val('');
                txtRentaDifNumeroFactura.val('');
                txtRentaDifOrdenCompra.val('');
                txtRentaDifFechaContraRecibo.val('');
                cbRentaDifHoras.bootstrapSwitch('state', false);
                // Inputs cargo por daño
                txtRentaDañoNumeroFactura.val('');
                txtRentaDañoCompra.val('');
                cbRentaCargoDaño.bootstrapSwitch('state', false);
                // Inputs fletes
                txtRentaFletesNumeroFactura.val('');
                txtRentaFletesOrdenCompra.val('');
                cbRentaFletes.bootstrapSwitch('state', false);
                // Botones
                btnRentaLimpiarCampos.data('tipo_renta', tipoRenta);
                btnRentaRegistrar.data('tipo_renta', tipoRenta);
            }
        }

        function initInputsDifHoras() {
            txtRentaDifHorasExtras.val('');
            txtRentaDifContraRecibo.val('');
            txtRentaDifNumeroFactura.val('');
            txtRentaDifOrdenCompra.val('');
            txtRentaDifFechaContraRecibo.val('');
        }

        function initInputsFletes() {
            txtRentaFletesNumeroFactura.val('');
            txtRentaFletesOrdenCompra.val('');
        }

        function initInputsCargoDaño() {
            txtRentaDañoNumeroFactura.val('');
            txtRentaDañoCompra.val('');
        }

        function initInputsCentroCosto() {
            txtRentaEquipo.val('');
            txtRentaNumeroSerie.val('');
            txtRentaModelo.val('');
            txtRentaProveedor.val('');
            txtRentaObra.val('');
        }

        function putInformacionCentroCosto(informacionCentroCosto) {
            txtRentaEquipo.val(informacionCentroCosto.Equipo);
            txtRentaNumeroSerie.val(informacionCentroCosto.NumeroSerie);
            txtRentaModelo.val(informacionCentroCosto.Modelo);
            txtRentaProveedor.val(informacionCentroCosto.Proveedor);
            txtRentaObra.val(informacionCentroCosto.AreaCuenta);
            txtRentaPeriodoDel.prop('disabled', false);
            txtRentaPeriodoAl.prop('disabled', false);
            // txtRentaHorometroInicial.val(informacionCentroCosto.HorometroInicial);
        }

        function initObjetoMaquina() {
            objMaquinaRentada = {
                Id: txtRentaId.val(),
                Folio: txtFolio.val(),
                PeriodoInicial: txtPeriodoInicial.val(),
                IdCentroCosto: cboRentaCentroCosto.val(),
                IdProveedor: txtIdProveedor.val(),
                IdAreaCuenta: txtIdAreaCuenta.val(),
                NumFactura: txtRentaNumeroFactura.val(),
                DepGarantia: unmaskNumero(txtRentaDepGarantia.val()),
                TramiteDG: cbRentaTramiteDg.bootstrapSwitch('state'),
                NotaCredito: txtRentaNotaCredito.val(),
                AplicaNC: cbRentaAplicaNc.bootstrapSwitch('state'),
                BaseHoraMensual: txtRentaBaseHoraMensual.val(),
                PeriodoDel: moment(txtRentaPeriodoDel.val(), 'DD/MM/YYYY').toISOString(true),
                PeriodoA: moment(txtRentaPeriodoAl.val(), 'DD/MM/YYYY').toISOString(true),
                HorometroInicial: txtRentaHorometroInicial.val(),
                HorometroFinal: txtRentaHorometroFinal.val(),
                HorasTrabajadas: txtRentaHorasTrabajadas.val(),
                HorasExtras: txtRentaHorasExtras.val(),
                TotalHorasExtras: unmaskNumero(txtRentaTotalHorasExtras.val()),
                PrecioPorMes: unmaskNumero(txtRentaPrecioPorMes.val()),
                SeguroPorMes: unmaskNumero(txtRentaSeguroPorMes.val()),
                IVA: unmaskNumero(txtRentaIVA.val()),
                TotalRenta: unmaskNumero(txtRentaTotalRenta.val()),
                OrdenCompra: txtRentaOrdenCompra.val(),
                ContraRecibo: txtRentaContraRecibo.val(),
                Anotaciones: txtRentaAnotaciones.val(),
                IdTipoMoneda: cboRentaMoneda.val(),
                DifHora: cbRentaDifHoras.bootstrapSwitch('state'),
                DifHoraExtra: txtRentaDifHorasExtras.val(),
                DifContraRecibo: txtRentaDifContraRecibo.val(),
                DifFactura: txtRentaDifNumeroFactura.val(),
                DifOrdenCompra: txtRentaDifOrdenCompra.val(),
                DifFechaContraRecibo: moment(txtRentaDifFechaContraRecibo.val(), 'DD/MM/YYYY').toISOString(true),
                CargoDaño: cbRentaCargoDaño.bootstrapSwitch('state'),
                CargoDañoFactura: txtRentaDañoNumeroFactura.val(),
                CargoDañoOrdenCompra: txtRentaDañoCompra.val(),
                Fletes: cbRentaFletes.bootstrapSwitch('state'),
                FletesFactura: txtRentaFletesNumeroFactura.val(),
                FletesOrdenCompra: txtRentaFletesOrdenCompra.val(),
                Activo: true,
                FechaCreacion: moment(_fechaActual, 'DD/MM/YYYY').toISOString(true),
                FechaModificacion: moment(_fechaActual, 'DD/MM/YYYY').toISOString(true),
                IdUsuario: 1
            }
            return objMaquinaRentada;
        }

        // Llamadas al servidor
        // Busqueda
        function getMaquinasRentadas() {
            if (validarFiltroBusquedaMaquinasRentadas()) {
                // console.log('validarFiltroBusquedaMaquinasRentadas: true');
                // console.log('List Area Cuenta: ' + getValoresMultiples('#cboFiltroAreaCuenta'));

                $.post('/MaquinariaRentada/GetMaquinasRentadas', {
                    idAreaCuenta: cboFiltroAreaCuenta.val(),
                    idCentroCosto: cboFiltroCentroCosto.val(),
                    periodoDel: moment(txtFiltroPeriodoDesde.val(), 'DD/MM/YYYY').toISOString(true),
                    periodoA: moment(txtFiltroPeriodoHasta.val(), 'DD/MM/YYYY').toISOString(true)
                }).always().then(response => {
                    if (response.success) {
                        // console.log('getMaquinasRentadas: success');
                        addRows(tblMaquinasRentadas, response.items);
                    } else {
                        // console.log('getMaquinasRentadas: false');
                    }
                }, error => {
                    // console.log('getMaquinasRentadas: error');
                });
            }
            else {
                // console.log('validarFiltroBusquedaMaquinasRentadas: false');
            }
        }

        function getMaquinaRentada(idMaquinaRentada, tipoRenta) {
            $.get('/MaquinariaRentada/GetInfoMaquinaRentada', {
                idMaquinaRentada: idMaquinaRentada
            }).always().then(response => {
                if (response.success) {
                    initInputsRenta(tipoRenta, response.items);
                    modalCapturaRentaMaquinaria.modal('show');
                } else {
                    AlertaGeneral('Alerta', response.message);
                }
            }, error => {
                AlertaGeneral('Error', 'Error al realizar la consulta al servidor: \n' + error.statusText);
            });
        }

        function registrar(tipoRenta) {
            // console.log('registrar(): tipoRenta: ' + tipoRenta);
            let objMaquina = initObjetoMaquina();
            $.post('/MaquinariaRentada/RegistrarRenta', {
                informacionRenta: objMaquina,
                tipoRenta: tipoRenta
            }).always().then(response => {
                if (response.success) {
                    cerrarModales();
                    // cboFiltroAreaCuenta.change();
                    // cboFiltroCentroCosto.resetCombo();
                    getMaquinasRentadas();
                    if (tipoRenta == TipoRenta.Nueva) {
                        ConfirmacionGeneral('Confirmación', 'Se registró nueva renta, folio: ' + response.items);
                    }
                    if (tipoRenta == TipoRenta.Editar) {
                        ConfirmacionGeneral('Confirmación', 'Se actualizó la renta, folio: ' + txtFolio.val());
                    }
                    if (tipoRenta == TipoRenta.Renovar) {
                        ConfirmacionGeneral('Confirmación', 'Se renovó la renta, folio: ' + txtFolio.val());
                    }
                } else {
                    // console.log('registrar(): ELSE');
                    if (tipoRenta == TipoRenta.Nueva) {
                        AlertaGeneral('Alerta', 'Error al registrar la renta: \n' + response.message);
                    }
                    if (tipoRenta == TipoRenta.Editar) {
                        AlertaGeneral('Alerta', 'Error al actualizar la renta: \n' + response.message);
                    }
                    if (tipoRenta == TipoRenta.Renovar) {
                        AlertaGeneral('Alerta', 'Error al renovar la renta: \n' + response.message);
                    }
                }
            }, error => {
                AlertaGeneral('Error', 'Error al realizar la consulta al servidor: \n' + error.statusText);
            });
        }

        function terminarRenta(boton, renovar) {
            // console.log('terminarRenta: in');
            $.post('/MaquinariaRentada/TerminarRentaMaquina', {
                idRentaMaquina: $(boton).data('id_maquina_rentada')
            }).always(cerrarModales()).then(response => {
                if (response.success) {
                    getMaquinasRentadas();
                    if (renovar) {
                        prepararModalCapturaRenta(boton, TipoRenta.Renovar, true);
                        initInputsRenta(TipoRenta.Renovar, response.items);
                        prepararRegistro(TipoRenta.Renovar, true);
                    } else {
                        ConfirmacionGeneral('Confirmación', '¡Renta terminada!');
                    }
                }
                else {
                    AlertaGeneral('Alerta', 'Error al terminar la renta: \n' + response.message);
                }
            }, error => {
                AlertaGeneral('Error', 'Error al realizar la consulta al servidor: \n' + error.statusText);
            });
        }

        function getHorometros(periodoInicio, periodoFinal, centroCosto, horometroInicial, horometroFinal) {
            // console.log('pI: ' + periodoInicio + ' pF: ' + periodoFinal + ' cc: ' + centroCosto);
            if (validarPeriodos()) {
                $.get('/MaquinariaRentada/GetHorometroPorPeriodoYCentroCosto', {
                    periodoInicio: periodoInicio,
                    periodoFinal: periodoFinal,
                    Cc: centroCosto,
                    horometroInicial: horometroInicial,
                    horometroFinal: horometroFinal,
                }).always().then(response => {
                    if (response.success) {
                        // console.log(response.items);
                        txtRentaHorometroInicial.val(response.items.HorometroInicial);
                        txtRentaHorometroFinal.val(response.items.HorometroFinal);
                    } else {
                        // console.log('getHorometros: ' + response.message);
                        // AlertaGeneral('Alerta', response.message);
                    }
                }, error => {
                    AlertaGeneral('Error', 'Error al realizar la consulta al servidor: \n' + error.statusText);
                });
            }
        }

        function getInformacionCentroCosto(idCentroCosto) {
            // console.log('getInformacionCentroCosto: in');
            if (validarCboOptionSelected(idCentroCosto)) {
                $.get('/MaquinariaRentada/GetInformacionCentroCosto', {
                    idCentroCosto: idCentroCosto
                }).always().then(response => {
                    if (response.success) {
                        putInformacionCentroCosto(response.items);
                    } else {
                        AlertaGeneral('Alerta', response.message);

                    }
                }, error => {
                    AlertaGeneral('Error', 'Error al realizar la consulta al servidor: \n' + error.statusText);
                });
            } else {
                initInputsCentroCosto();
            }
        }
        // async function getInformacionCentroCosto(idCentroCosto){
        //     try{
        //         response = await ejectFetchJson(window.location.origin + '/MaquinariaRentada/GetInformacionCentroCosto', {idCentroCosto: idCentroCosto});
        //         if (response.success){
        //             putInformacionCentroCosto(response.items);
        //         }else{
        //             AlertaGeneral('Alerta', response.message);
        //         }
        //     }catch(o_O) { AlertaGeneral('Alerta', o_O.message); }
        // }

        function ReporteTiempoRequeridoVsUtilizado(idReporte) {
            var btnValue = idReporte;
            if (validarFiltroBusquedaMaquinasRentadas()) {
                $.blockUI({ message: 'Procesando...' });
                var idReporte = btnValue;
                var areasCuentas = getValoresMultiples('#cboFiltroAreaCuenta');
                var centrosCosto = getValoresMultiples('#cboFiltroCentroCosto');
                var periodoInicio = moment(txtFiltroPeriodoDesde.val(), 'DD/MM/YYYY').toISOString(true);
                var periodoFinal = moment(txtFiltroPeriodoHasta.val(), 'DD/MM/YYYY').toISOString(true);
                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&areasCuentas=" + areasCuentas + "&centrosCosto=" + centrosCosto + "&periodoInicio=" + periodoInicio + "&periodoFinal=" + periodoFinal;
                report.attr("src", path);
                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
            }
        }
    }

    $(document).ready(() => {
        Maquinaria.Capturas.MaquinariaRentada = new MaquinariaRentada();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();