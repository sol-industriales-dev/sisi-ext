(() => {
    $.namespace('Maquinaria.ActivoFijo.DetalleDepreciacion');
    DetalleDepreciacion = function () {
        //Objetos tablas
        const tblDetalleDepreciacion = $('#tblDetalleDepreciacion');
        const tblDepMaquina = $('#tblDepMaquina');

        //Objetos inputs
        const cboFiltroPeriodo = $('#cboFiltroPeriodo');
        const cboFiltroActivos = $('#cboFiltroActivos');
        const cboFiltroConceptos = $('#cboFiltroConceptos');
        const txtFiltroNoEconomico = $('#txtFiltroNoEconomico');
        const cboFiltroTipoMovimiento = $('#cboFiltroTipoMovimiento');
        const cboFiltroAreaCuenta = $('#cboFiltroAreaCuenta');
        const cboFiltroCuentasOverhaul = $('#cboFiltroCuentasOverhaul');
        const filtroCuentasOverhaul = $('#filtroCuentasOverhaul');
        const txtFiltroFecha = $('#txtFiltroFecha');

        //Modales
        const modalDetalleDep = $('#modalDetalleDep');
        const modaltitulo = $('#modalTitulo');

        //Objetos informativos
        const txtMonto = $('#txtMonto');
        const txtDepTotal = $('#txtDepTotal');
        const txtMesesTotalesDep = $('#txtMesesTotalesDep');
        const txtMesesFaltantes = $('#txtMesesFaltantes');
        const txtPorcentajeDep = $('#txtPorcentajeDep');
        const txtFechaBaja = $('#txtFechaBaja');
        const leyendaTipoMovimiento = $('#leyendaTipoMovimiento');
        const filtroNoEconomico = $('#filtroNoEconomico');
        const filtroTipoDelMovimiento = $('#filtroTipoDelMovimiento');
        const btnTabExcel = $('#btnTabExcel');

        const btnImprimir = $("#btnImprimir");
        const btnImprimirResumen = $('#btnImprimirResumen');
        const btnImprimirTab = $('#btnImprimirTab');

        //Botones
        const btnFiltroBuscar = $('#btnFiltroBuscar');

        // Reporte
        //Eventos
        btnImprimirResumen.on('click', function () {
            ReporteResumenDepreciacion($(this).val());
        });

        btnImprimirTab.on('click', function () {
            location.href = '/ActivoFijo/GetConsultaEnExcel';
        });

        btnTabExcel.on('click', function () {
            DescargarTabulador();
        });

        cboFiltroPeriodo.on('change', function () {
            btnImprimirResumen.attr('disabled', true);
            if ($(this).val() != '') {
                GetTabulador($(this).val());
            }
            else {
                limpiarInformacion();
                limpiarTabla(tblDetalleDepreciacion);
            }
        });

        cboFiltroConceptos.on('change', function () {
            btnImprimirResumen.attr('disabled', true);
            btnImprimirTab.attr('disabled', true);
            if ($(this).find('option:selected').data('prefijo') != 'EsMaquinaria') {
                $('#filtroAreaCuenta').hide();
                filtroCuentasOverhaul.hide();
                cboFiltroCuentasOverhaul.val('');
            }
            else {
                $('#filtroAreaCuenta').show();

                if ($(this).val() == 1210) {
                    filtroCuentasOverhaul.show();
                }
                else {
                    filtroCuentasOverhaul.hide();
                    cboFiltroCuentasOverhaul.val('');
                }
            }
        });

        btnFiltroBuscar.on('click', function () {
            if (cboFiltroConceptos.val() == '' && txtFiltroNoEconomico.val() == '') {
                AlertaGeneral('Alerta', 'Favor de elegir un concepto y/o ingresar un # económico');
            }
            else {
                let _mesesOverhaul = cboFiltroCuentasOverhaul.find('option:selected').data('prefijo');
                console.log(_mesesOverhaul);
                GetDepMaquinas(cboFiltroActivos.val(), cboFiltroConceptos.val(), txtFiltroNoEconomico.val(), cboFiltroTipoMovimiento.val(), cboFiltroAreaCuenta.val(), _mesesOverhaul);
            }
        });

        cboFiltroActivos.on('change', function () {
            btnImprimirResumen.attr('disabled', true);
            btnImprimirTab.attr('disabled', true);
            // GetDepMaquinas($(this).val());
        });

        $('table').on('click', '.btnDetalleDepCC', function () {
            if ($(this).data('id_dep_maquina') != 0) {
                btnTabExcel.show();
                // GetPeriodosDepreciacion($(this).data('id_dep_maquina'));
                GetTabulador($(this).data('id_dep_maquina'), $(this).data('extra_cat_maq_dep'));
                modaltitulo.text('Detalle de depreciación Número económico: ' + $(this).data('no_economico'));
                modalDetalleDep.modal('show');
            }
            else {
                btnTabExcel.hide();
                CrearTabuladorDesdeFront($(this));
            }
        });

        modalDetalleDep.on('hidden.bs.modal', function () {
            limpiarTabla(tblDetalleDepreciacion);
        });

        modalDetalleDep.on('shown.bs.modal', function () {
            tblDetalleDepreciacion.DataTable().columns.adjust().draw();
        });

        let totalesTabulador;

        let tblDetDep;

        const report = $('#report');

        let init = () => {
            moment.locale('es');
            checkJS_VERSION();
            initComboBoxCuentas();
            initComboBoxTiposMovimiento();
            initComboBoxAreasCuenta();
            initTablaDepMaquina();
            initTablaDetalleDepreciacion();

            txtFiltroFecha.datepicker({ yearRange: '2018:c' }).datepicker('setDate', new Date());
        }

        function obtenerUrlParams() {
            const variables = getUrlParams(window.location.href);
            var clean_uri = location.protocol + "//" + location.host + location.pathname;

            if (variables.noEconomico) {
                cboFiltroConceptos.val('1210');
                cboFiltroConceptos.trigger('change');
                cboFiltroActivos.val('1');
                cboFiltroActivos.trigger('change');
                txtFiltroNoEconomico.val(variables.noEconomico);
                btnFiltroBuscar.trigger('click');
            }

            window.history.replaceState({}, document.title, clean_uri);
        }

        function getUrlParams(url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            return params;
        }

        //Inits
        function checkJS_VERSION() {
            let JS_VERSION = 2;
            if ($('#JS_VERSION').val() != JS_VERSION) {
                alert('ELIMINAR CACHE');
            }
        }

        function initComboBoxCuentas() {
            cboFiltroConceptos.fillCombo('/ActivoFijo/GetCuentasCBO', null, false, null, () => {
                cboFiltroCuentasOverhaul.fillCombo('/ActivoFijo/CuentasDepOverhaul', null, false, null, () => {
                    cboFiltroTipoMovimiento.attr('multiple', true);
                    cboFiltroTipoMovimiento.fillCombo('/ActivoFijo/GetTiposMovimiento', null, false, 'Todos', () => {
                        cboFiltroAreaCuenta.attr('multiple', true);
                        cboFiltroAreaCuenta.fillCombo('/ActivoFijo/GetAreasCuenta', null, false, 'Todos', () => {
                            obtenerUrlParams();
                        });
                        convertToMultiselect('#cboFiltroAreaCuenta');
                    });
                    convertToMultiselect('#cboFiltroTipoMovimiento');

                    crearLeyendaTipoMovimiento();
                });
            });
        }

        function initComboBoxTiposMovimiento() {
        }

        function initComboBoxAreasCuenta() {

        }

        //#region tabla
        function initTablaDepMaquina() {
            tblDepMaquina.DataTable({
                ordering: true,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],

                columns: [
                    { data: null, title: 'Detalle', className: 'dt-body-center' },
                    { data: 'CC', title: 'CC', className: 'dt-body-center' },
                    { data: 'NoEconomico', title: 'Número económico', className: 'dt-body-center' },
                    { data: 'Descripcion', title: 'Descripción' },
                    { data: 'AreaCuenta', title: 'Área cuenta' },
                    { data: 'TipoMovimiento', title: 'Tipo movimiento', className: 'dt-body-center' },
                    { data: 'TipoActivo', title: 'Tipo activo', className: 'dt-body-center' },
                    { data: 'Factura', title: 'Factura', className: 'dt-body-center' },
                    { data: 'Poliza', title: 'Poliza', className: 'dt-body-center' },
                    { data: 'Monto', title: 'MOI', className: 'dt-body-right' },
                    { data: 'DepreciacionSemanal', title: 'Depreciación semanal', className: 'dt-body-right' },
                    { data: 'DepreciacionMensual', title: 'Depreciación mensual', className: 'dt-body-right' },
                    { data: 'FechaInicioDepreciacion', title: 'Fecha inicio depreciación', className: 'dt-body-center' },
                    { data: 'DepreciacionAcumulada', title: 'Depreciación acumulada', className: 'dt-body-right' },
                    { data: 'MesesTotalesDepreciacion', title: 'Meses a depreciar', className: 'dt-body-center' },
                    { data: 'MesesFaltantes', title: 'Meses faltantes', className: 'dt-body-center' },
                    { data: 'ValorLibro', title: 'Falta por depreciar', className: 'dt-body-center' },
                    { data: 'PorcentajeDepreciacion', title: '% Depreciación', className: 'dt-body-center' },
                    { data: 'FechaBaja', title: 'Fecha baja', className: 'dt-body-center' },
                    { data: 'semanasDepreciacionOH_14_1', title: 'Semanas Dep OH 14-1', className: 'dt-body-center' },
                    { data: 'depreciacionOH_14_1', title: 'Dep OH 14-1', className: 'dt-body-right' }

                ],

                columnDefs: [
                    {
                        targets: [2],
                        render: function (data, type, row) {
                            if (data != null) {
                                //return '<button type="button" class="btn btn-link btnDetalleDepCC" data-no_economico="' + data + '" data-id_cat_maquina="' + row.IdCatMaquina + '" data-id_dep_maquina="' + row.IdCatMaquina + '">' + data + '</button>'
                                return data;
                            }
                            else {
                                return '';
                            }
                        }
                    },
                    {
                        targets: [9, 10, 11, 13],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [17],
                        render: function (data, type, row) {
                            return data + '%';
                        }
                    },
                    {
                        targets: [12, 18],
                        render: function (data, type, row) {
                            return moment(data).isValid() ? moment(data).format('DD/MM/YYYY') : '';
                        }
                    },
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            return '<button type="button" class="btn btn-primary btnDetalleDepCC" data-extra_cat_maq_dep="' + row.EsExtraCatMaqDep + '" data-no_economico="' + row.NoEconomico + '" data-id_cat_maquina="' + row.IdCatMaquina + '" data-id_dep_maquina="' + row.IdDepMaquina + '"><i class="fas fa-align-justify"></i></button>'
                        }
                    },
                    {
                        targets: [16, 20],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    // $(row).find('td').eq(0).addClass('text-center');
                    // $(row).find('td').eq(0).nextAll().addClass('text-right');
                },

                drawCallback: function (settings) {
                    let esMaquinaria = cboFiltroConceptos.find('option:selected').data('prefijo') == 'EsMaquinaria' ? true : false;

                    if (txtFiltroNoEconomico.val() != '') {
                        tblDepMaquina.DataTable().columns([1, 2, 5, 6]).visible(true);
                    }
                    else {
                        tblDepMaquina.DataTable().columns([1, 2, 4, 5, 6]).visible(esMaquinaria);
                    }
                },

                headerCallback: function (thead, data, start, end, display) {
                    // $(thead).addClass('bg-table-header');
                    $(thead).find('th').addClass('text-center');
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    // $(tfoot).find('th').addClass('text-right');
                    $(tfoot).find('th').removeClass('dt-body-center');
                    $(tfoot).find('th').removeClass('dt-body-right');
                    // $(tfoot).find('th').addClass('text-right');
                    if (totalesTabulador != null) {
                        $(tfoot).find('th').eq(1).html(maskNumero(totalesTabulador.SumaMOI));
                        $(tfoot).find('th').eq(2).html(maskNumero(totalesTabulador.SumaSemanal));
                        $(tfoot).find('th').eq(3).html(maskNumero(totalesTabulador.SumaMensual));
                        $(tfoot).find('th').eq(5).html(maskNumero(totalesTabulador.SumaAcumulada));
                        $(tfoot).find('th').eq(8).html(maskNumero(totalesTabulador.SumaLibros));
                        $(tfoot).find('th').eq(12).html(maskNumero(totalesTabulador.SumaOH14_1));
                    }
                }
            });
        }

        function initTablaDetalleDepreciacion() {
            tblDetDep = tblDetalleDepreciacion.DataTable({
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollY: '400px',
                scrollCollapse: true,

                columns: [
                    { data: 'Año', title: 'Año', className: 'dt-body-center' },
                    { data: 'Mensualidad', title: 'Mensualidad', className: 'dt-body-center' },
                    { data: 'Mes', title: 'Mes' },
                    { data: 'DepreciacionSemanal', title: 'Depreciación semanal', className: 'dt-body-right' },
                    { data: 'DepreciacionMensual', title: 'Depreciación mensual', className: 'dt-body-right' },
                    { data: 'DepreciacionAcumulada', title: 'Depreciación acumulada', className: 'dt-body-right' },
                    { data: 'ValorEnLibros', title: 'Valor en libros', className: 'dt-body-right' },
                    //{ data: 'AreaCuenta', title: 'Área cuenta' }
                ],

                columnDefs: [
                    {
                        targets: [3],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [4, 5, 6],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [2],
                        render: function (data, type, row) {
                            return data.charAt(0).toUpperCase() + data.slice(1)
                        }
                    }//,
                    //{
                    //targets: [7],
                    //render: function (data, type, row) {
                    //return '<span style="padding-left: 20px;">' + data +'</span>';
                    //}
                    //}
                ],

                createdRow: function (row, data, dataIndex) {
                    // $(row).find('td').eq(0).addClass('text-center');
                    // $(row).find('td').eq(0).nextAll().addClass('text-right');
                },

                drawCallback: function (settings) {
                    var datosDT = tblDetalleDepreciacion.DataTable().rows().data();

                    let añoDetalleTabulador = 0;
                    let totalDepSemanal = 0.0;
                    let totalDepMensual = 0.0;
                    let totalDepAcumulada = 0.0;
                    let totalValLibro = 0.0;

                    for (let index = 0; index < datosDT.length; index++) {
                        añoDetalleTabulador = index == 0 ? datosDT[index].Año : añoDetalleTabulador;

                        if (index == (datosDT.length - 1) || añoDetalleTabulador != datosDT[index].Año) {
                            let renglon;

                            if (index == (datosDT.length - 1)) {
                                renglon = index;

                                totalDepSemanal += datosDT[index].DepreciacionSemanal;
                                totalDepMensual += datosDT[index].DepreciacionMensual;
                                totalDepAcumulada = datosDT[index].DepreciacionAcumulada < 0 ? 0 : datosDT[index].DepreciacionAcumulada;
                                totalValLibro = datosDT[index].ValorEnLibros;
                            }
                            else {
                                renglon = (index - 1);
                            }
                            tblDetDep.row(renglon).child(
                                $(
                                    '<tr class="trTotalesDetDep">' +
                                    '<td colspan="3" class="text-right">TOTAL AÑO ' + añoDetalleTabulador +
                                    '</td>' +
                                    '<td class="text-right">' + maskNumero(totalDepSemanal) +
                                    '</td>' +
                                    '<td class="text-right">' + maskNumero(totalDepMensual) +
                                    '</td>' +
                                    '<td class="text-right">' + maskNumero(totalDepAcumulada) +
                                    '</td>' +
                                    '<td class="text-right">' + maskNumero(totalValLibro) +
                                    '</td>' +
                                    // '<td class="text-right">' +
                                    // '</td>' +
                                    '</tr>'
                                )
                            ).show();
                            añoDetalleTabulador = datosDT[index].Año;

                            totalDepSemanal = datosDT[index].DepreciacionSemanal;
                            totalDepMensual = datosDT[index].DepreciacionMensual;
                            totalDepAcumulada = datosDT[index].DepreciacionAcumulada;
                            totalValLibro = datosDT[index].ValorEnLibros;
                        }
                        else {
                            totalDepSemanal += datosDT[index].DepreciacionSemanal;
                            totalDepMensual += datosDT[index].DepreciacionMensual;
                            totalDepAcumulada = datosDT[index].DepreciacionAcumulada;
                            totalValLibro = datosDT[index].ValorEnLibros;
                        }
                    }
                },

                headerCallback: function (thead, data, start, end, display) {
                    // $(thead).addClass('bg-table-header');
                    $(thead).find('th').addClass('text-center');
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    // $(tfoot).find('th').addClass('text-right');
                    let sumaSemanal = 0;
                    let sumaMensual = 0;
                    let sumaAcumulada = 0;
                    let sumaLibro = 0;

                    $('.trTotalesDetDep').each(function (index, value) {
                        sumaSemanal += unmaskNumero($(this).find('td').eq(1).text());
                        sumaMensual += unmaskNumero($(this).find('td').eq(2).text());
                        sumaAcumulada = unmaskNumero($(this).find('td').eq(3).text());
                        sumaLibro = unmaskNumero($(this).find('td').eq(4).text());
                    });

                    $(tfoot).find('th').removeClass('dt-body-right');
                    $(tfoot).find('th').removeClass('dt-body-center');

                    $(tfoot).find('th').eq(1).html(maskNumero(sumaSemanal));
                    $(tfoot).find('th').eq(2).html(maskNumero(sumaMensual));
                    $(tfoot).find('th').eq(3).html(maskNumero(sumaAcumulada));
                    $(tfoot).find('th').eq(4).html(maskNumero(sumaLibro));
                }
            });
        }
        //#endregion

        function crearLeyendaTipoMovimiento() {
            cboFiltroTipoMovimiento.find('option').each(function (index, item) {
                if (index != 0) {
                    let text = $(item).text();
                    let descripcion = $(item).data('prefijo');

                    leyendaTipoMovimiento.append('<li><span class="leyendaId">' + text + '</span> ' + descripcion + '</li>');
                }
            });
        }

        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function limpiarTabla(tabla) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
        }

        function limpiarInformacion() {
            txtMonto.text('');
            txtDepTotal.text('');
            txtMesesTotalesDep.text('');
            txtMesesFaltantes.text('');
            txtPorcentajeDep.text('');
            txtFechaBaja.text('');
        }

        function CrearTabuladorDesdeFront(registro) {
            let Descripcion = $(registro).closest('tr').find('td').eq(1).text();
            let MOI = parseFloat(unmaskNumero($(registro).closest('tr').find('td').eq(4).text()));
            let DepSemanal = parseFloat(unmaskNumero($(registro).closest('tr').find('td').eq(5).text()));
            let FechaInicioDep = $(registro).closest('tr').find('td').eq(7).text().split('/');
            let AñoIni = FechaInicioDep[2];
            let MesIni = FechaInicioDep[1];
            let DiaIni = FechaInicioDep[0];
            let FechaIni = new Date(AñoIni, MesIni - 1, DiaIni);
            let MesesMaxDepreciacion = $(registro).closest('tr').find('td').eq(9).text();
            let MesesFaltantes = $(registro).closest('tr').find('td').eq(10).text();
            let PorcentajeDep = parseFloat(unmaskNumero($(registro).closest('tr').find('td').eq(12).text())) / 100;

            let afTabulador = new Array();

            let DepAcum = 0.0;
            let ValLib = 0.0;

            for (let index = 0; index < (MesesMaxDepreciacion - MesesFaltantes); index++) {
                let tab = {
                    Año: moment(new Date(FechaIni.setMonth(FechaIni.getMonth() + 1))).format('YYYY'),
                    Mensualidad: index + 1,
                    Mes: moment(FechaIni).format('MMMM'),
                    DepreciacionMensual: parseFloat(((MOI * (PorcentajeDep)) / 12)),
                    DepreciacionSemanal: ((MOI * (PorcentajeDep)) / 12) / 4,
                    DepreciacionAcumulada: index == 0 ? ((MOI * (PorcentajeDep)) / 12) : DepAcum + parseFloat(((MOI * (PorcentajeDep)) / 12)),
                    ValorEnLibros: index == 0 ? MOI - ((MOI * (PorcentajeDep)) / 12) : ValLib
                }
                DepAcum += tab.DepreciacionMensual;
                ValLib = tab.ValorEnLibros - tab.DepreciacionMensual;

                afTabulador.push(tab);
            }

            AddRows(tblDetalleDepreciacion, afTabulador);
            modaltitulo.text('Detalle de depreciación: ' + Descripcion);
            modalDetalleDep.modal('show');

        }

        function ReporteResumenDepreciacion(idReporte) {
            $.blockUI({ message: 'Procesando...' });
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&maquinaActiva=" + cboFiltroActivos.val() + "&cuenta=" + cboFiltroConceptos.val() + "&noEconomico=" + txtFiltroNoEconomico.val() + "&tipoMovimiento=" + cboFiltroTipoMovimiento.val();
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function DescargarTabulador() {
            location.href = '/ActivoFijo/ExcelTabulador';
        }

        function GetPeriodosDepreciacion(idCatMaquina) {
            cboFiltroPeriodo.fillCombo('/ActivoFijo/GetPeriodosDepreciacion', { idCatMaquina: idCatMaquina }, false, null);
            let num = $('#cboFiltroPeriodo option').length;
            cboFiltroPeriodo.prop('selectedIndex', num - 1);
            cboFiltroPeriodo.change();
        }

        //Llamadas al servidor
        function GetTabulador(idDepMaquina, EsExtraCatMaqDep) {
            $.get('/ActivoFijo/GetTabulador', {
                idDepMaquina: idDepMaquina,
                EsExtraCatMaqDep
            }).always().then(response => {
                if (response.success) {
                    txtMonto.text(maskNumero(response.items.Monto));
                    txtDepTotal.text(maskNumero(response.items.DepreciacionTotal));
                    txtMesesTotalesDep.text(response.items.MesesTotalesDepreciacion);
                    txtMesesFaltantes.text(response.items.MesesFaltantesDepreciacion);
                    txtPorcentajeDep.text(response.items.PorcentajeDepreciacion);
                    if (moment(response.items.FechaBaja).isValid()) {
                        txtFechaBaja.text(moment(response.items.FechaBaja).format('DD/MM/YYYY'));
                    }
                    else {
                        txtFechaBaja.text('');
                    }

                    AddRows(tblDetalleDepreciacion, response.items.Tabulador);
                }
                else {
                    AlertaGeneral('Alerta', response.message);
                }
            }, error => {
                AlertaGeneral('Error', response.message);
            });
        }

        function GetDepMaquinas(maquinaActiva, cuenta, noEconomico, tipoMovimiento, areasCuenta, cuentaOverhaul) {
            $.post('/ActivoFijo/GetDepMaquinas', {
                maquinaActiva: maquinaActiva,
                cuenta: cuenta,
                noEconomico: noEconomico,
                tipoMovimiento: tipoMovimiento,
                areasCuenta: areasCuenta,
                cuentaOverhaul,
                fecha: txtFiltroFecha.val()
            }).always().then(response => {
                if (response.success) {
                    totalesTabulador = response.items;
                    AddRows(tblDepMaquina, response.items.Depreciaciones);
                    btnImprimirResumen.attr('disabled', false);
                    btnImprimirTab.attr('disabled', false);
                }
                else {
                    AlertaGeneral('Alerta', response.message);
                    btnImprimirResumen.attr('disabled', true);
                    btnImprimirTab.attr('disabled', true);
                }
            }, error => {
                AlertaGeneral('Error', response.message)
                btnImprimirResumen.attr('disabled', true);
                btnImprimirTab.attr('disabled', true);
            });
        }

        init();
    }

    // drawCallback: function (settings) {
    //     var api = this.api(),
    //         rows = api.rows({ page: 'current' }).nodes(),
    //         head = null;
    //     foot = null;
    //     api.column({ page: 'current' }).data().each((group, i, dtable) => {
    //         const data = dtable.data()[i];
    //         let descripcionNomina = data.descripcionNomina;
    //         let descripcionCuenta = data.descripcionCuenta;
    //         //header
    //         if (head !== `${descripcionNomina}-${descripcionCuenta}`) {
    //             $(rows).eq(i).before(`<tr class="text-center encabezadoOtros"><td>C.C.</td><td>OBRA</td><td class="conceptoNomina">${descripcionNomina}</td><td>IVA 16%</td><td>TOTAL</td>/tr>`);
    //             head = `${descripcionNomina}-${descripcionCuenta}`;
    //         }
    //     });
    // },

    $(document).ready(() => {
        Maquinaria.ActivoFijo.DetalleDepreciacion = new DetalleDepreciacion();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();