(() => {
    $.namespace('Maquinaria.ActivoFijo.PolizaDepreciacion');
    PolizaDepreciacion = function () {
        //#region SELECTORES
        const idEmpresa = $('#idEmpresa');

        const selectFiltroCuenta = $('#selectFiltroCuenta');
        const selectFiltroAño = $('#selectFiltroAño');
        const selectFiltroMes = $('#selectFiltroMes');
        const btnFiltroBuscar = $('#btnFiltroBuscar');

        const btnCapturarPoliza = $('#btnCapturarPoliza');

        const hdrCuentaConsultada = $('#hdrCuentaConsultada');
        const tblPolizasDepreciacion = $('#tblPolizasDepreciacion');

        const modalPolizaDetalle = $('#modalPolizaDetalle');
        const tblPolizaDetalle = $('#tblPolizaDetalle');

        const modalCapturaPoliza = $('#modalCapturaPoliza');
        const selectCapturaCuenta = $('#selectCapturaCuenta');
        const selectCapturaAño = $('#selectCapturaAño');
        const selectCapturaMes = $('#selectCapturaMes');
        const selectCapturaSemana = $('#selectCapturaSemana');
        const divOverhaul = $('#divOverhaul');
        const divCuentasDep = $('#divCuentasDep');
        const selectCapturaCuentaOverhaul = $('#selectCapturaCuentaOverhaul');
        const chkCapturaOverhaul = $('#chkCapturaOverhaul');
        const btnGenerarPoliza = $('#btnGenerarPoliza');
        const tblNuevaPoliza = $('#tblNuevaPoliza');
        const btnRegistrarPoliza = $('#btnRegistrarPoliza');
        const btnModificarPoliza = $('#btnModificarPoliza');

        const divDepPendienteOH = $('#divDepPendienteOH');
        const btnGenerarPolizaOHPendiente = $('#btnGenerarPolizaOHPendiente');
        //#endregion

        //#region VARIABLES
        let dtPolizasDepreciacion;
        let dtPolizaDetalle;
        let dtPolizaCreada;

        let añoMesCaptura;
        let numSemana;
        //#endregion

        $('#mdlbtnAceptar').html('<i class="fa fa-check"></i> Si');
        $('#mdlbtnCancelar').html('<i class="fa fa-arrow-left"></i> No');

        const fechaActual = new Date();
        const añoInicial = 2018;
        const añoActual = fechaActual.getFullYear();
        const mesActual = fechaActual.getMonth() + 1;
        const diaActual = fechaActual.getDate();
        const meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];

        const EstatusPoliza = {
            Actualizada: 'A',
            Bloqueada: 'B',
            Capturada: 'C',
            Erronea: 'E',
            Validada: 'V'
        }

        const report = $('#report');

        const CuentasConOverhaul = ['1210'];

        const TiposMovimiento = {
            Cargo: 1,
            Abono: 2
        }

        //reportePoliza(true, false, false, false, dataRow.Estatus, dataRow.Poliza, new Date(dataRow.Año, dataRow.Mes - 1, 1));
        //reportePoliza(true, true, false, false, dataRow.Estatus, dataRow.Poliza, dataRow.FechaPoliza);
        menuConfig = {
            lstOptions: [
                { text: `<i class="fa fa-download"></i> Descargar póliza`, action: "descargarPoliza", fn: parametros => { reportePoliza(true, parametros.esCC, false, false, parametros.estatus, parametros.poliza, parametros.fecha, true) } },
                { text: `<i class="fa fa-file"></i> Ver póliza`, action: "visorPoliza", fn: parametros => { reportePoliza(true, parametros.esCC, false, false, parametros.estatus, parametros.poliza, parametros.fecha, false) } },
                { text: `<i class="fa fa-download"></i> Descargar póliza detalle`, action: "descargarPolizaDetalle", fn: parametros => { reportePoliza(false, parametros.esCC, true, false, parametros.estatus, parametros.poliza, parametros.fecha, true) } },
                { text: `<i class="fa fa-file"></i> Ver póliza detalle`, action: "visorPolizaDetalle", fn: parametros => { reportePoliza(false, parametros.esCC, true, false, parametros.estatus, parametros.poliza, parametros.fecha, false) } },
                { text: `<i class="fa fa-download"></i> Descargar póliza firma`, action: "descargarPolizaFirma", fn: parametros => { reportePoliza(false, parametros.esCC, true, true, parametros.estatus, parametros.poliza, parametros.fecha, true) } },
                { text: `<i class="fa fa-file"></i> Ver póliza firma`, action: "visorPolizaFirma", fn: parametros => { reportePoliza(false, parametros.esCC, true, true, parametros.estatus, parametros.poliza, parametros.fecha, false) } },
            ]
        }

        //#region EVENTOS
        btnFiltroBuscar.on('click', function () {
            let año = selectFiltroAño.val();
            let mes = selectFiltroMes.val();
            let cuenta = selectFiltroCuenta.val();
            let textoCuenta = selectFiltroCuenta.find('option:selected').text();

            if (cuenta != '-- Cuentas --') {
                hdrCuentaConsultada.text(textoCuenta);
                obtenerPolizasDepreciacion(año, mes, cuenta);
            }
            else {
                AlertaGeneral('Alerta', 'Seleccione una cuenta');
            }
        });

        $('.modal').on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        $('#modalCapturaPoliza').on('hide.bs.modal', function (e) {
            selectCapturaCuenta.val('-- Cuentas --').trigger('change');
            tblNuevaPoliza.DataTable().clear().draw();
            btnRegistrarPoliza.hide();
        });

        btnCapturarPoliza.on('click', function () {
            modalCapturaPoliza.modal('show');
        });

        selectCapturaCuenta.on('change', function () {
            tblNuevaPoliza.DataTable().clear().draw();
            let cuenta = selectCapturaCuenta.val();

            if (CuentasConOverhaul.includes(cuenta)) {
                divOverhaul.show();
            }
            else {
                chkCapturaOverhaul.prop('checked', false);
                divOverhaul.hide();
            }

            if (cuenta == '-- Cuentas --') {

            }
            else {
                fechaCaptura(cuenta, chkCapturaOverhaul.prop('checked'));
            }
        });

        chkCapturaOverhaul.on('click', function () {
            if (chkCapturaOverhaul.prop('checked')) {
                divCuentasDep.show();

                if (selectCapturaSemana.val() == 4) {
                    divDepPendienteOH.show();
                } else {
                    divDepPendienteOH.hide();
                }
            }
            else {
                divCuentasDep.hide();
                divDepPendienteOH.hide();
            }

            tblNuevaPoliza.DataTable().clear().draw();
            selectCapturaCuenta.trigger('change');
        });

        btnGenerarPoliza.on('click', function () {
            let cuenta = selectCapturaCuenta.val();

            if (cuenta != '-- Cuentas --') {
                let año = añoMesCaptura.getFullYear();
                let mes = añoMesCaptura.getMonth() + 1;
                let semana = numSemana;
                let dia = añoMesCaptura.getDate();
                let esOverhaul = chkCapturaOverhaul.prop('checked');
                let idCuentaDepOverhaul = null;

                if (esOverhaul && selectCapturaCuentaOverhaul.val() == '-- Ctas Dep --') {
                    AlertaGeneral('Alerta', 'Seleccione una cuenta de depreciación');
                    return;
                }
                else {
                    idCuentaDepOverhaul = selectCapturaCuentaOverhaul.val();
                }

                generarPoliza(cuenta, año, mes, semana, dia, esOverhaul, idCuentaDepOverhaul);
            }
            else {
                AlertaGeneral('Alerta', 'Seleccione una cuenta');
            }
        });

        btnGenerarPolizaOHPendiente.on('click', function () {
            let cuenta = selectCapturaCuenta.val();

            if (cuenta != '-- Cuentas --') {
                let año = añoMesCaptura.getFullYear();
                let mes = añoMesCaptura.getMonth() + 1;
                let semana = numSemana;
                let dia = añoMesCaptura.getDate();
                let esOverhaul = chkCapturaOverhaul.prop('checked');
                let idCuentaDepOverhaul = null;

                if (esOverhaul && semana == 4) {
                    generarPoliza(cuenta, año, mes, semana, dia, esOverhaul, -141);
                } else {
                    AlertaGeneral('Tiene que ser la semana 4 y overhaul');
                }
            }
        });

        btnRegistrarPoliza.on('click', function () {
            registrarPoliza();
        });
        //#endregion

        //#region TABLAS
        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function initTablas() {
            dtPolizasDepreciacion = tblPolizasDepreciacion.DataTable({
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollY: '400px',
                scrollCollapse: true,
                scrollX: false,

                columns: [
                    { data: null, title: '#' },
                    { data: 'Año', title: 'Año' },
                    { data: 'Mes', title: 'Mes' },
                    { data: 'TipoPoliza', title: 'Tipo póliza' },
                    { data: 'Poliza', title: 'Póliza' },
                    { data: 'Cargo', title: 'Cargos' },
                    { data: 'Abono', title: 'Abonos' },
                    { data: 'Descripcion', title: 'Descripción' },
                    { data: 'FechaPoliza', title: 'Fecha de la póliza' },
                    { data: 'Estatus', title: 'Estatus' },
                    { data: null, title: 'Sistemas' },
                    { data: null, title: 'Opciones' }
                ],

                columnDefs: [
                    {
                        targets: [0, 1, 2, 3, 4, 8, 9, 10, 11],
                        className: 'dt-body-center'
                    },
                    {
                        targets: [5, 6],
                        className: 'dt-body-right',
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [7],
                        className: 'dt-body-nowrap'
                    },
                    {
                        targets: [8],
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        targets: [9],
                        render: function (data, type, row) {
                            if (data == EstatusPoliza.Actualizada) { return 'Actualizada'; }
                            if (data == EstatusPoliza.Bloqueada) { return 'Bloqueada'; }
                            if (data == EstatusPoliza.Capturada) { return 'Capturada'; }
                            if (data == EstatusPoliza.Erronea) { return 'Errónea'; }
                            if (data == EstatusPoliza.Validada) { return 'Validada'; }
                        }
                    },
                    {
                        targets: [10],
                        render: function (data, type, row) {
                            if (row.Sigoplan) {
                                return '<span class="label label-primary">SIGOPLAN</span> <span class="label label-success">ENKONTROL</span>';
                            }
                            else {
                                return '<span class="label label-success">ENKONTROL</span>';
                            }
                        }
                    },
                    {
                        targets: [11],
                        width: '300px',
                        render: function (data, type, row) {
                            let dtBtnVerReportePoliza = '<button class="btn btn-success btnVisor" data-tipo_archivo="1"><i class="far fa-eye"></i> Poliza</button>';
                            let dtBtnVerDetalle = '<button class="btn btn-primary btnVerDetallePoliza"><i class="far fa-eye"></i> Detalle</button>';
                            let dtBtnEditar = '<button class="btn btn-warning btnEditarPoliza"><i class="far fa-edit"></i> Editar';
                            let dtBtnEliminar = '';
                            if (row.UltimaSigoplan) {
                                dtBtnEliminar = '<button class="btn btn-danger btnEliminarPoliza"><i class="far fa-trash-alt"></i> Eliminar</button>';
                            }

                            if (row.Estatus == EstatusPoliza.Capturada && row.Sigoplan) {
                                return dtBtnVerReportePoliza + ' ' + dtBtnVerDetalle + ' ' + /*' ' + dtBtnEditar + ' ' +*/ dtBtnEliminar;
                            }
                            else {
                                return dtBtnVerReportePoliza + ' ' + dtBtnVerDetalle;
                            }
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    $(row).find('td').eq(0).text(++dataIndex);
                },

                drawCallback: function (settings) { },
                headerCallback: function (thead, data, start, end, display) { },
                footerCallback: function (tfoot, data, start, end, display) { },

                initComplete: function (settings, json) {
                    tblPolizasDepreciacion.on('click', '.btnVerDetallePoliza', function () {
                        let dataRow = dtPolizasDepreciacion.row($(this).closest('tr')).data();

                        modalPolizaDetalle.find('.modal-title').text('Detalle póliza: ' + dataRow.Poliza + ', descripción: ' + dataRow.Descripcion);

                        obtenerPolizaDetalle(dataRow.Año, dataRow.Mes, dataRow.Poliza);
                    });

                    tblPolizasDepreciacion.on('click', '.btnEliminarPoliza', function () {
                        let dataRow = dtPolizasDepreciacion.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal(
                            'Confirmación',
                            '!Se eliminara la póliza: ' + dataRow.Poliza + ' ¿Desea continuar?',
                            () => eliminarPoliza(dataRow.IdPolSigoplan)
                        );
                    });

                    tblPolizasDepreciacion.on('click', '.btnVisor', function () {
                        let dataRow = dtPolizasDepreciacion.row($(this).closest('tr')).data();

                        menuConfig.parametros = {
                            esCC: true,
                            estatus: dataRow.Estatus,
                            poliza: dataRow.Poliza,
                            fecha: dataRow.FechaPoliza
                        }
                        mostrarMenu();
                    });
                },
            });

            dtPolizaDetalle = tblPolizaDetalle.DataTable({
                ordering: true,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollY: '400px',
                scrollCollapse: true,
                scrollX: false,

                columns: [
                    { data: 'Linea', title: 'Línea' },
                    { data: 'Cuenta', title: 'Cuenta' },
                    { data: 'Subcuenta', title: 'Subcuenta' },
                    { data: 'SubSubcuenta', title: 'SubSubcuenta' },
                    { data: 'DescripcionCuenta', title: 'Descripción cuenta' },
                    { data: 'CC', title: 'CC' },
                    { data: 'Referencia', title: 'Referencia' },
                    { data: 'Concepto', title: 'Concepto' },
                    { data: 'TipoMovimiento', title: 'Tipo del movimiento' },
                    { data: 'Cargos', title: 'Cargos' },
                    { data: 'Abonos', title: 'Abonos' },
                    { data: 'AreaCuenta', title: 'Área cuenta' }
                ],

                columnDefs: [
                    {
                        targets: [0, 1, 2, 3, 5, 6, 8],
                        className: 'dt-body-center'
                    },
                    {
                        targets: [9],
                        className: 'dt-body-right',
                        render: function (data, type, row) {
                            if (row.TipoMovimiento == TiposMovimiento.Cargo) {
                                return maskNumero(row.Monto);
                            }
                            else {
                                return maskNumero(0.0);
                            }
                        }
                    },
                    {
                        targets: [10],
                        className: 'dt-body-right',
                        render: function (data, type, row) {
                            if (row.TipoMovimiento == TiposMovimiento.Abono) {
                                return maskNumero(row.Monto);
                            }
                            else {
                                return maskNumero(0.0);
                            }
                        }
                    },
                    {
                        targets: [11],
                        render: function (data, type, row) {
                            return row.Area + '-' + row.Cuenta_OC + ' - ' + data;
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) { },
                drawCallback: function (settings) { },
                headerCallback: function (thead, data, start, end, display) { },

                footerCallback: function (tfoot, data, start, end, display) {
                    let totalCargos = 0.0;
                    let totalAbonos = 0.0;

                    data.forEach(function (value, index, array) {
                        totalCargos += value.TipoMovimiento == TiposMovimiento.Cargo ? value.Monto : 0.0;
                        totalAbonos += value.TipoMovimiento == TiposMovimiento.Abono ? value.Monto : 0.0;
                    });

                    $(tfoot).find('th').eq(1).text(maskNumero(totalCargos));
                    $(tfoot).find('th').eq(2).text(maskNumero(totalAbonos));
                },

                initComplete: function (settings, json) { }
            });

            dtPolizaCreada = tblNuevaPoliza.DataTable({
                ordering: true,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollY: '400px',
                scrollCollapse: true,
                scrollX: false,

                columns: [
                    { data: 'Linea', title: 'Línea' },
                    { data: 'Cuenta', title: 'Cuenta' },
                    { data: 'Subcuenta', title: 'Subcuenta' },
                    { data: 'SubSubcuenta', title: 'SubSubcuenta' },
                    { data: 'CC', title: 'CC' },
                    { data: 'Referencia', title: 'Referencia' },
                    { data: 'Concepto', title: 'Concepto' },
                    { data: 'TipoMovimiento', title: 'Tipo del movimiento' },
                    { data: 'Cargos', title: 'Cargos' },
                    { data: 'Abonos', title: 'Abonos' },
                    { data: 'AreaCuenta', title: 'Área cuenta' }
                ],

                columnDefs: [
                    {
                        targets: [0, 1, 2, 3, 4, 5, 7],
                        className: 'dt-body-center'
                    },
                    {
                        targets: [8],
                        className: 'dt-body-right',
                        render: function (data, type, row) {
                            if (row.TipoMovimiento == TiposMovimiento.Cargo) {
                                return maskNumero(row.Monto);
                            }
                            else {
                                return maskNumero(0.0);
                            }
                        }
                    },
                    {
                        targets: [9],
                        className: 'dt-body-right',
                        render: function (data, type, row) {
                            if (row.TipoMovimiento == TiposMovimiento.Abono) {
                                return maskNumero(row.Monto);
                            }
                            else {
                                return maskNumero(0.0);
                            }
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) { },
                drawCallback: function (settings) { },
                headerCallback: function (thead, data, start, end, display) { },

                footerCallback: function (tfoot, data, start, end, display) {
                    let totalCargos = 0.0;
                    let totalAbonos = 0.0;

                    data.forEach(function (value, index, array) {
                        totalCargos += value.TipoMovimiento == TiposMovimiento.Cargo ? value.Monto : 0.0;
                        totalAbonos += value.TipoMovimiento == TiposMovimiento.Abono ? value.Monto : 0.0;
                    });

                    $(tfoot).find('th').eq(1).text(maskNumero(totalCargos));
                    $(tfoot).find('th').eq(2).text(maskNumero(totalAbonos));
                },

                initComplete: function (settings, json) { }
            });
        }
        //#endregion

        //#region SERVIDOR
        function initComboBox() {
            selectFiltroCuenta.fillCombo('/ActivoFijo/GetCuentasCBO', null, false, '-- Cuentas --');

            selectFiltroCuenta.find('option').each(function (value, index, array) {
                $(this).clone().appendTo(selectCapturaCuenta);
            });
            selectCapturaCuenta.val('-- Cuentas --');

            selectCapturaCuentaOverhaul.fillCombo('/ActivoFijo/CuentasDepOverhaul', null, false, '-- Ctas Dep --');

            for (let años = añoInicial; años <= añoActual; años++) {
                selectFiltroAño.append('<option value="' + años + '">' + años + '</option>');
                selectCapturaAño.append('<option value="' + años + '">' + años + '</option>');
            }
            selectFiltroAño.val(añoActual);
            selectCapturaAño.val(añoActual);

            meses.forEach(function (value, index, array) {
                selectFiltroMes.append('<option value="' + (index + 1) + '">' + value + '</option>');
                selectCapturaMes.append('<option value="' + (index + 1) + '">' + value + '</option>');
            });
            selectFiltroMes.val(mesActual);
            selectCapturaMes.val(mesActual);

            for (let semana = 1; semana <= 4; semana++) {
                selectCapturaSemana.append('<option value="' + semana + '">' + semana + '</option>');
            }
            selectCapturaSemana.val(Math.ceil(diaActual / 7));
        }

        const getFileRuta = new URL(window.location.origin + '/ActivoFijo/getFileRuta');
        async function setVisor(id) {
            try {
                response = await ejectFetchJson(getFileRuta, { id });
                if (response.success) {
                    $('#myModal').data().ruta = response.ruta;
                    $('#myModal').modal('show');
                }
            } catch (o_O) { }
        }

        function reportePoliza(esResumen, esCC, esPorHoja, esFirma, estatus, poliza, fecha, esVisor) {
            if (!esVisor) { $.blockUI({ message: 'Procesando...' }); }
            var path = '/Reportes/Vista.aspx?' +
                'esDescargaVisor=' + esVisor +
                '&esVisor=' + true +
                '&idReporte=' + 64 + // 188 pruebas, 64 producción
                '&isResumen=' + esResumen +
                '&isCC=' + esCC +
                '&isPorHoja=' + esPorHoja +
                '&isFirma=' + esFirma +
                '&Estatus=' + estatus +
                '&icc=' + '001' +
                '&fcc=' + 'S01' +
                '&iPol=' + poliza +
                '&fPol=' + poliza +
                '&iPer=' + moment(fecha).format('MM/YYYY') +
                '&fPer=' + moment(fecha).format('MM/YYYY') +
                '&iTp=' + '03' +
                '&fTp=' + '03' +
                '&firma1=' + (idEmpresa.val() == 1 ? 'CP. Liliana Lavandera Torres' : 'CP. Jessica Galdean') +
                '&firma2=' + 'CP. Arturo Sánchez';
            report.attr('src', path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                $('#myModal').modal('show');
                // $.post('/Visor/LoadFile').always().then(response => {
                //     if(response.success) {
                //         setTimeout(() => objctlDoc.View(token), 100);
                //         $('#myModal').modal('show');
                //     }
                //     else {
                //         AlertaGeneral('Error', response.message);
                //     }
                // }, error => {
                //     AlertaGeneral('Error', error.statusText);
                // });
                //     openCRModal();
            };
        }

        function reporteCaptura(reporte) {
            $.blockUI({ message: 'Procesando...' });
            var path = '/Reportes/Vista.aspx?' +
                'esDescargaVisor=' + false +
                '&esVisor=' + true +
                '&idReporte=' + 64 + // 188 pruebas, 64 producción
                '&isResumen=' + reporte.ReporteResumido +
                '&isCC=' + reporte.EsCC +
                '&isPorHoja=' + reporte.PolizaPorHoja +
                '&isFirma=' + reporte.IncluirFirmas +
                '&Estatus=' + reporte.Estatus +
                '&icc=' + reporte.CCInicial +
                '&fcc=' + reporte.CCFinal +
                '&iPol=' + reporte.PolizaInicial +
                '&fPol=' + reporte.PolizaFinal +
                '&iPer=' + moment(reporte.PeriodoInicial).format('MM/YYYY') +
                '&fPer=' + moment(reporte.PeriodoFinal).format('MM/YYYY') +
                '&iTp=' + reporte.TipoPolizaInicial +
                '&fTp=' + reporte.TipoPolizaFinal +
                '&firma1=' + reporte.Reviso +
                '&firma2=' + reporte.Autorizo;
            report.attr('src', path);
            document.getElementById('report').onload = function () {
                $('#myModal').modal('show');
                //$.unblockUI();
                //openCRModal();
            };
        }

        function fechaCaptura(cuenta, esOverhaul) {
            $.get('/ActivoFijo/FechaCaptura', {
                cuenta: cuenta,
                esOverhaul: esOverhaul
            }).always().then(response => {
                if (response.Success) {
                    btnGenerarPoliza.prop('disabled', false);
                    añoMesCaptura = new Date(moment(response.Value.Fecha).format('YYYY'), moment(response.Value.Fecha).format('MM') - 1, moment(response.Value.Fecha).format('DD'));
                    numSemana = response.Value.Semana;
                    selectCapturaAño.val(añoMesCaptura.getFullYear());
                    selectCapturaMes.val(añoMesCaptura.getMonth() + 1);
                    selectCapturaSemana.val(numSemana);
                }
                else {
                    btnGenerarPoliza.prop('disabled', true);
                    AlertaGeneral('Alerta', response.Message);
                }
            }, error => {
                AlertaGeneral('Alerta', error.statusText);
            });
        }

        function eliminarPoliza(polizaId) {
            $.post('/ActivoFijo/EliminarPoliza', {
                polizaId: polizaId
            }).always().then(response => {
                if (response.Success) {
                    btnFiltroBuscar.trigger('click');
                    ConfirmacionGeneral('Confirmación', '!Se eliminó correctamente la póliza!');
                }
                else {
                    AlertaGeneral('Alerta', response.Message);
                }
            }, error => {
                AlertaGeneral('Alerta', error.statusText);
            });
        }

        function registrarPoliza() {
            $.post('/ActivoFijo/RegistrarPoliza').always().then(response => {
                if (response.Success) {
                    tblNuevaPoliza.DataTable().clear().draw();

                    let cuentaCapturada = selectCapturaCuenta.val();
                    let añoCapturado = selectCapturaAño.val();
                    let mesCaptutado = selectCapturaMes.val();
                    selectFiltroCuenta.val(cuentaCapturada);
                    selectFiltroAño.val(añoCapturado);
                    selectFiltroMes.val(mesCaptutado);

                    btnFiltroBuscar.trigger('click');
                    selectCapturaCuenta.val('-- Cuentas --');
                    btnRegistrarPoliza.prop('disabled', true);

                    AlertaAceptarRechazarNormal(
                        'Confirmación',
                        '!Se registró correctamente la póliza: ' + response.Value.Poliza + ' ¿Desea ver la póliza?',
                        () => reporteCaptura(response.Value)
                    );
                }
                else {
                    AlertaGeneral('Alerta', response.Message);
                }
            }, error => {
                AlertaGeneral('Alerta', error.statusText);
            });
        }

        function generarPoliza(cuenta, año, mes, semana, dia, esOverhaul, idCuentaDepOverhaul) {
            $.get('/ActivoFijo/GenerarPoliza', {
                cuenta: cuenta,
                año: año,
                mes: mes,
                semana: semana,
                dia: dia,
                esOverhaul: esOverhaul,
                idCuentaDepOverhaul: idCuentaDepOverhaul
            }).always().then(response => {
                if (response.Success) {
                    btnGenerarPoliza.prop('disabled', true);
                    btnRegistrarPoliza.prop('disabled', false);
                    addRows(tblNuevaPoliza, response.Value);
                    if (response.Value.length > 0) {
                        btnRegistrarPoliza.show();
                    }
                }
                else {
                    AlertaGeneral('Alerta', response.Message);
                }
            }, error => {
                AlertaGeneral('Alerta', error.statusText);
            });
        }

        function obtenerPolizaDetalle(año, mes, poliza) {
            $.get('/ActivoFijo/ObtenerPolizaDetalle', {
                año: año,
                mes: mes,
                poliza: poliza
            }).always().then(response => {
                if (response.Success) {
                    addRows(tblPolizaDetalle, response.Value);
                    modalPolizaDetalle.modal('show');
                }
                else {
                    AlertaGeneral('Alerta', response.Message);
                }
            }, error => {
                AlertaGeneral('Alerta', error.statusText);
            });
        }

        function obtenerPolizasDepreciacion(año, mes, cuenta) {
            $.get('/ActivoFijo/ObtenerPolizasDepreciacion', {
                año: año,
                mes: mes,
                cuenta: cuenta
            }).always().then(response => {
                if (response.Success) {
                    addRows(tblPolizasDepreciacion, response.Value);
                }
                else {
                    AlertaGeneral('Alerta', response.Message);
                }
            }, error => {
                AlertaGeneral('Alerta', error.statusText);
            });
        }
        //#endregion

        let init = () => {
            initTablas();
            initComboBox();
        }

        init();
    }
    $(document).ready(() => {
        Maquinaria.ActivoFijo.PolizaDepreciacion = new PolizaDepreciacion();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();