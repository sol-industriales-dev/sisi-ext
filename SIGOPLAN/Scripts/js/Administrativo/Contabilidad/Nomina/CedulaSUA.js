(() => {
    $.namespace('Administrativo.Contabilidad.Nomina');
    Nomina = function () {

        const report = $('#report');

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

        //#region selectores
        //#region panel principal
        const panelGeneral = $('#panelGeneral');
        const cboxPeriodoFiltro = $('#cboxPeriodoFiltro');
        const cboxTipoDocumentoFiltro = $('#cboxTipoDocumentoFiltro');
        const btnPoliza = $('#btnPoliza');
        const btnExcel = $('#btnExcel');
        const tblCedula = $('#tblCedula');
        //#endregion

        //#region generar póliza
        const modalFechaPoliza = $('#modalFechaPoliza');
        const inputFechaPoliza = $('#inputFechaPoliza');
        const btnGenerarPoliza = $('#btnGenerarPoliza');
        const panelPoliza = $('#panelPoliza');
        const periodoPoliza = $('#periodoPoliza');
        const tblPoliza = $('#tblPoliza');
        const btnRegistrarPoliza = $('#btnRegistrarPoliza');

        const cboTipoNomina = $('#cboTipoNomina');
        //#endregion
        //#endregion

        //#region eventos
        //#region panel principal
        cboxPeriodoFiltro.on('change', function () {
            cargarNomina();
        });

        cboxTipoDocumentoFiltro.on('change', function () {
            cargarNomina();

            btnExcel.attr('disabled', (+cboxTipoDocumentoFiltro.val() != 3));
        });

        btnExcel.on('click', function () {
            let tipo_nomina = +cboTipoNomina.val();
            let anio = cboxPeriodoFiltro.find('option:selected').data('prefijo').split('-')[3];
            let periodo = +cboxPeriodoFiltro.val();
            let tipo_documento = +cboxTipoDocumentoFiltro.val();

            location.href = `/Nomina/DescargarExcelSUA?tipo_nomina=${tipo_nomina}&anio=${anio}&periodo=${periodo}&tipo_documento=${tipo_documento}`;
        });

        btnPoliza.on('click', function () {
            modalFechaPoliza.modal('show');
        });

        $('.regresarPanel').on('click', function () {
            reiniciarEstados();

            $(this).closest('.panel').fadeToggle('fast', 'linear', function () {
                panelGeneral.fadeToggle('fast', 'linear', function () {
                    tblCedula.DataTable().draw();
                });
            });
        });

        cboTipoNomina.change(function (e) {
            comboNomina.fillComboGroup('/Nomina/getCbotPeriodoNomina', { tipoNomina: cboTipoNomina.val() }, false, undefined, function () {
                //comboNomina.prop("selectedIndex", 1);
                comboNomina.change();
            });
        });
        //#endregion

        //#region generar póliza
        btnGenerarPoliza.on('click', function () {
            generarPoliza($(this).data('id'), moment(inputFechaPoliza.val(), 'DD/MM/YYYY').toISOString(), cboxTipoDocumentoFiltro.val()).done(function (response) {
                modalFechaPoliza.modal('hide');
                inputFechaPoliza.val('');

                if (response && response.success) {
                    periodoPoliza.text(cboxPeriodoFiltro.find('option:selected').text());

                    panelGeneral.fadeToggle('fast', 'linear', function () {
                        panelPoliza.fadeToggle('fast', 'linear', function () {
                            btnRegistrarPoliza.show();
                            AddRows(tblPoliza, response.items.movimientos);
                        });
                    });
                }
                else {
                }
            });
        });

        btnRegistrarPoliza.on('click', function () {
            registrarPoliza();
        });
        //#endregion
        //#endregion

        //#region tablas
        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function initTblCedula() {
            tblCedula.DataTable({
                order: [[0, 'asc']],
                ordering: true,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],

                columns: [
                    { data: 'cc', title: 'CC' },
                    { data: 'estado', title: 'EDO' },
                    { data: 'registroPatronal', title: 'RP' },
                    { data: 'ccDescripcion', title: 'OBRA / DEPTO.' },
                    { data: 'imssPatronal', title: 'IMSS Patronal' },
                    { data: 'imssObrero', title: 'IMSS Obrero', },
                    { data: 'rcvPatronal', title: 'RCV Patronal' },
                    { data: 'rcvObrero', title: 'RCV Obrero' },
                    { data: 'infonavit', title: 'INFONAVIT' },
                    { data: 'isn', title: 'ISN' },
                    { data: 'total', title: 'TOTAL' }
                ],

                columnDefs: [
                    {
                        targets: [0, 1, 2],
                        className: 'text-center'
                    },
                    {
                        targets: [4, 5, 6, 7, 8, 9, 10],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        },
                        className: 'text-right'
                    }
                ],

                createdRow: function (row, data, dataIndex) { },
                drawCallback: function (settings) { },
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },

                footerCallback: function (tfoot, data, start, end, display) { }
            });
        }

        function initTblPoliza() {
            tblPoliza.DataTable({
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
                        render: function (data, type, row) {
                            if (row.tm == 1 || row.tm == 3) {
                                return maskNumero(row.monto);
                            } else {
                                return maskNumero(0);
                            }
                        }
                    },
                    {
                        targets: [10],
                        className: 'text-right',
                        render: function (data, type, row) {
                            if (row.tm == 2 || row.tm == 4) {
                                return maskNumero(row.monto);
                            } else {
                                return maskNumero(0);
                            }
                        }
                    }
                ],

                initComplete: function (settings, json) { },
                createdRow: function (row, data, dataIndex) { },
                drawCallback: function (settings) { },
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    let totalCargo = 0;
                    let totalAbono = 0;

                    data.forEach(function (element, index, array) {
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

        //#region generales
        function reiniciarEstados() {
            btnGenerarPoliza.removeData();
            periodoPoliza.text('');
            btnRegistrarPoliza.hide();
        }

        function initCamposFechas() {
            inputFechaPoliza.datepicker().datepicker({});
        }

        function cargarNomina() {
            btnPoliza.prop('disabled', true);
            btnGenerarPoliza.data('id', 0);

            if (cboxPeriodoFiltro.val() && cboxTipoDocumentoFiltro.val()) {
                const dataPeriodo = cboxPeriodoFiltro.find('option:selected').data('prefijo').split('-');
                const tipoPeriodo = dataPeriodo[2];
                const periodo = cboxPeriodoFiltro.val();
                const year = dataPeriodo[3];

                getNominas(cboxTipoDocumentoFiltro.val(), year, periodo);
            }
            else {
                swal('Alerta!', 'Debe seleccionar un período y tipo de raya', 'warning');
            }
        }
        //#endregion

        //#region backend
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
                '&firma1=' + 'CP. Liliana Madrid García' +
                '&firma2=' + 'CP. Arturo Sánchez';
            report.attr('src', path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                $('#myModal').modal('show');
            };
        }

        function registrarPoliza() {
            $.post('/Nomina/RegistrarPolizaSUA', { tipoDocumento: cboxTipoDocumentoFiltro.val() }).then(response => {
                if (response.success) {
                    reiniciarEstados();
                    cboxPeriodoFiltro.trigger('change');

                    swal('Confirmación', 'Se registró la póliza: ' + response.poliza, 'success');
                } else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }

        function generarPoliza(suaId, fecha, tipoDocumento) {
            return $.get('/Nomina/GenerarPolizaSUA',
                {
                    suaId,
                    fecha,
                    tipoDocumento
                }).then(response => {
                    if (response.success) {
                        return response;
                    }
                    else {
                        swal('Alerta!', response.message, 'warning');
                        return response;
                    }
                }, error => {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                });
        }

        function getNominas(tipoDocumento, year, periodo) {
            $.get('/Nomina/GetSUA',
                {
                    tipoDocumento,
                    year,
                    periodo
                }).then(response => {
                    if (response.success) {
                        btnPoliza.prop('disabled', response.tienePoliza);
                        btnGenerarPoliza.data('id', response.idSua);
                        AddRows(tblCedula, response.items);
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                });
        }

        function llenarCombos() {
            cboxPeriodoFiltro.fillComboGroup('/Nomina/getCbotPeriodoNomina', { tipoNomina: cboTipoNomina.val() }, false, null, () => {
                // cboxPeriodoFiltro.prop('selectedIndex', 0);
                // cboxPeriodoFiltro.trigger('change');
            });

            // cboxTipoDocumentoFiltro.fillCombo('/Nomina/GetTipoRaya', null, false, null, null);
        }
        //#endregion

        (function init() {
            moment.locale('es');

            initCamposFechas();
            initTblCedula();
            initTblPoliza();
            llenarCombos();
        })();


    }
    $(document).ready(() => Administrativo.Contabilidad.Nomina = new Nomina())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();