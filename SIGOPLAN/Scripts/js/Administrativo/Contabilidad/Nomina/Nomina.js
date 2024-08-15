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
        const cboxTipoRayaFiltro = $('#cboxTipoRayaFiltro');
        const tblNomina = $('#tblNomina');
        const cboTipoNomina = $('#cboTipoNomina');
        const btnExcel = $('#btnExcel');
        //#endregion

        //#region panel resumen
        const panelResumen = $('#panelResumen');
        const periodoResumen = $('#periodoResumen');
        const tblResumenNomina = $('#tblResumenNomina');
        const btnValidarRaya = $('#btnValidarRaya');
        //#endregion

        //#region generar póliza
        const modalFechaPoliza = $('#modalFechaPoliza');
        const inputFechaPoliza = $('#inputFechaPoliza');
        const btnGenerarPoliza = $('#btnGenerarPoliza');
        const panelPoliza = $('#panelPoliza');
        const periodoPoliza = $('#periodoPoliza');
        const tblPoliza = $('#tblPoliza');
        const btnRegistrarPoliza = $('#btnRegistrarPoliza');
        //#endregion

        //#region empleados sin registrar
        const modalEmpleadosSinRegistrarCuenta = $('#modalEmpleadosSinRegistrarCuenta');
        const tblEmpleadosSinRegistrar = $('#tblEmpleadosSinRegistrar');
        //#endregion
        //#endregion

        //#region eventos
        //#region panel principal
        cboxPeriodoFiltro.on('change', function () {
            cargarNomina();
        });

        cboxTipoRayaFiltro.on('change', function () {
            if (cboxTipoRayaFiltro.val() == "3") cboxPeriodoFiltro.fillComboGroup('/Nomina/GetCbotPeriodoNominaAguinaldo', null, false, null, () => { });
            else cboxPeriodoFiltro.fillComboGroup('/Nomina/GetCbotPeriodoNomina', null, false, null, () => { });
            cargarNomina();

            btnExcel.attr('disabled', !(+cboxTipoRayaFiltro.val() == 1));
        });

        btnExcel.on('click', function () {
            let tipo_nomina = +cboTipoNomina.val();
            let anio = cboxPeriodoFiltro.find('option:selected').data('prefijo').split('-')[3];
            let periodo = +cboxPeriodoFiltro.val();

            location.href = `/Nomina/DescargarExcelNomina?tipo_nomina=${tipo_nomina}&anio=${anio}&periodo=${periodo}`;
        });

        $('.regresarPanel').on('click', function () {
            reiniciarEstados();

            $(this).closest('.panel').fadeToggle('fast', 'linear', function () {
                panelGeneral.fadeToggle('fast', 'linear', function () {
                    tblNomina.DataTable().draw();
                });
            });
        });
        //#endregion

        //#region generar póliza
        btnGenerarPoliza.on('click', function () {
            generarPoliza($(this).data('nominaId'), moment(inputFechaPoliza.val(), 'DD/MM/YYYY').toISOString()).done(function (response) {
                modalFechaPoliza.modal('hide');
                inputFechaPoliza.val('');

                if (response && response.success) {
                    periodoPoliza.text(cboxPeriodoFiltro.find('option:selected').text());

                    panelGeneral.fadeToggle('fast', 'linear', function () {
                        panelPoliza.fadeToggle('fast', 'linear', function () {
                            btnRegistrarPoliza.show();
                            AddRows(tblPoliza, response.items);
                        });
                    });
                }
                else {
                    AddRows(tblEmpleadosSinRegistrar, response.empleadosSinRegistrar);
                    modalEmpleadosSinRegistrarCuenta.modal('show');
                }
            });
        });

        modalEmpleadosSinRegistrarCuenta.on('shown.bs.modal', function () {
            tblEmpleadosSinRegistrar.DataTable().columns.adjust().draw();
        });

        btnRegistrarPoliza.on('click', function () {
            registrarPoliza();
        });
        //#endregion

        //#region panel resumen
        btnValidarRaya.on('click', function () {
            const nominaId = $(this).data().nominaId;

            swal({
                title: 'Alerta!',
                text: 'Se validará la nómina. ¿Desea continuar con la validación?',
                icon: 'warning',
                buttons: true,
                dangerMode: true,
                buttons: ['Cerrar', 'Validar']
            })
                .then((aceptar) => {
                    if (aceptar) {
                        validarNomina(nominaId).done(function (response) {
                            if (response && response.success) {
                                reiniciarEstados();

                                cboxPeriodoFiltro.trigger('change');

                                swal('Confirmación', 'Se validó la nómina correctamente', 'success');
                            }
                        });
                    }
                });
        });
        //#endregion
        //#endregion

        //#region tablas
        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function initTblNomina() {
            tblNomina.DataTable({
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
                    { data: 'year', title: 'Año' },
                    { data: 'periodo', title: 'Período' },
                    { data: 'tipoNomina', title: 'Tipo nómina' },
                    { data: 'tipoRaya', title: 'Tipo raya' },
                    { data: 'descripcionCC', title: 'CC' },
                    { data: 'cantidadEmpleados', title: 'Empleados', className: 'text-center' },
                    { data: 'netoPagar', title: 'Neto a pagar' },
                    { data: 'netoPagar2', title: 'Neto a pgar (900)' },
                    { data: 'fechaCaptura', title: 'Fecha de captura' },
                    { data: 'validada', title: 'Validada' },
                    { data: 'fechaValidacion', title: 'Fecha validación' },
                    { data: null, title: 'Opciones' }
                ],

                columnDefs: [
                    {
                        targets: [0, 1, 2, 8, 9, 10, 11],
                        className: 'text-center'
                    },
                    {
                        targets: [6, 7],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        },
                        className: 'text-right'
                    },
                    {
                        targets: [8, 10],
                        render: function (data, type, row) {
                            if (data) {
                                return moment(data).format('DD/MM/YYYY');
                            }
                            else {
                                return '';
                            }
                        }
                    },
                    {
                        targets: [11],
                        render: function (data, type, row) {
                            let botones;

                            const verResumen = "<button class='btn btn-primary btnVerResumen' title='Ver resumen' data-cc=" + data.cc + " data-year=" + data.year + " data-tipoNomina=" + data.tipoNomina + "><i class='far fa-eye'></i></button>";
                            const generarPoliza = `<button class="btn btn-success btnGenerarPoliza" title="Crear póliza"><i class="far fa-file-alt"></i></button>`;
                            const verPoliza = `<button class="btn btn-info btnVerPoliza" title="Ver póliza"><i class="fas fa-file-download"></i></button>`;

                            botones = verResumen;

                            if (row.validadaEstatus && !row.tienePoliza) {
                                botones += ' ' + generarPoliza;
                            }

                            if (row.tienePoliza) {
                                botones += ' ' + verPoliza;
                            }

                            return botones;
                        }
                    }
                ],

                initComplete: function (settings, json) {
                    tblNomina.on('click', '.btnVerResumen', function () {
                        const rowData = tblNomina.DataTable().row($(this).closest('tr')).data();

                        resumenNomina(rowData.id, $(this).attr("data-year"), $(this).attr("data-cc"), $(this).attr("data-tipoNomina")).done(function (response) {
                            if (response && response.success) {
                                periodoResumen.text(cboxPeriodoFiltro.find('option:selected').text());

                                if (!rowData.validadaEstatus) {
                                    btnValidarRaya.data('nominaId', rowData.id);
                                    btnValidarRaya.show();
                                }

                                panelGeneral.fadeToggle('fast', 'linear', function () {
                                    panelResumen.fadeToggle('fast', 'linear', function () {
                                        AddRows(tblResumenNomina, response.items.detalle);
                                    });
                                });
                            }
                        });
                    });

                    tblNomina.on('click', '.btnGenerarPoliza', function () {
                        const rowData = tblNomina.DataTable().row($(this).closest('tr')).data();

                        btnGenerarPoliza.data('nominaId', rowData.id);

                        modalFechaPoliza.modal('show');
                    });

                    tblNomina.on('click', '.btnVerPoliza', function () {
                        const rowData = tblNomina.DataTable().row($(this).closest('tr')).data();

                        const infoPoliza = rowData.poliza.split('-');

                        menuConfig.parametros = {
                            esCC: true,
                            estatus: rowData.estatusPoliza,
                            poliza: infoPoliza[2],
                            fecha: new Date(infoPoliza[0], infoPoliza[1] - 1, 1)
                        }
                        mostrarMenu();
                    });
                },

                createdRow: function (row, data, dataIndex) { },
                drawCallback: function (settings) { },
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },

                footerCallback: function (tfoot, data, start, end, display) { }
            });
        }

        function initTblResumen() {
            tblResumenNomina.DataTable({
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
                    { data: 'cta', title: 'Cuenta', className: 'text-center' },
                    { data: 'concepto', title: 'Concepto', className: 'text-left' },
                    { data: 'monto', title: 'Monto', className: 'text-right' }
                ],

                columnDefs: [
                    {
                        targets: [2],
                        render: function (data, type, row) {
                            return maskNumero(data);
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
                    let totalCargoMenosAbono = 0;

                    data.forEach(function (element, index, array) {
                        totalCargoMenosAbono += element.monto;
                    });

                    $(tfoot).find('th').eq(0).removeClass('text-left');
                    $(tfoot).find('th').eq(0).removeClass('text-center');
                    $(tfoot).find('th').eq(0).addClass('text-right');
                    $(tfoot).find('th').eq(1).text(maskNumero(totalCargoMenosAbono));
                }
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
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'cc', title: 'CC' },
                    { data: 'referencia', title: 'Referencia' },
                    { data: 'concepto', title: 'Concepto' },
                    { data: 'tm', title: 'Tipo movimiento' },
                    { data: 'cargo', title: 'Cargo' },
                    { data: 'abono', title: 'Abono' }
                ],

                columnDefs: [
                    {
                        targets: [0, 1, 2, 3, 4, 6, 9],
                        className: 'text-center'
                    },
                    {
                        targets: [10, 11],
                        className: 'text-right',
                        render: function (data, type, row) {
                            return maskNumero(data);
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
                        totalCargo += element.cargo;
                        totalAbono += element.abono;
                    });

                    $(tfoot).find('th').eq(0).removeClass('text-left');
                    $(tfoot).find('th').eq(0).removeClass('text-center');
                    $(tfoot).find('th').eq(0).addClass('text-right');
                    $(tfoot).find('th').eq(1).text(maskNumero(totalCargo));
                    $(tfoot).find('th').eq(2).text(maskNumero(totalAbono));
                }
            });
        }

        function initTblEmpleadosSinRegistrar() {
            tblEmpleadosSinRegistrar.DataTable({
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
                    }
                ],

                columns: [
                    { data: 'numeroEmpleado', title: 'Clave empleado' },
                    { data: 'descripcionTipoCuenta', title: 'Tipo cuenta' },
                    { data: 'cc', title: 'CC' },
                ],

                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-center'
                    }
                ],

                initComplete: function (settings, json) { },
                createdRow: function (row, data, dataIndex) { },
                drawCallback: function (settings) { },
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },

                footerCallback: function (tfoot, data, start, end, display) { }
            });
        }
        //#endregion

        //#region generales
        function reiniciarEstados() {
            btnValidarRaya.removeData();
            btnValidarRaya.hide();
            btnGenerarPoliza.removeData();
            periodoPoliza.text('');
            btnRegistrarPoliza.hide();
        }

        function initCamposFechas() {
            inputFechaPoliza.datepicker().datepicker({});
        }

        function cargarNomina() {
            if (cboxPeriodoFiltro.val() && cboxTipoRayaFiltro.val()) {
                const dataPeriodo = cboxPeriodoFiltro.find('option:selected').data('prefijo').split('-');
                const tipoPeriodo = dataPeriodo[2];
                const periodo = cboxPeriodoFiltro.val();
                const year = dataPeriodo[3];

                getNominas(year, tipoPeriodo, periodo, cboxTipoRayaFiltro.val());
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
            $.post('/Nomina/RegistrarPoliza', {}).then(response => {
                if (response.success) {
                    reiniciarEstados();
                    cboxPeriodoFiltro.trigger('change');

                    swal('Confirmación', `Se registró la póliza: ${response.poliza}. Compras Generadas: ${response.stringComprasGeneradas}`, 'success');
                } else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }

        function generarPoliza(nominaId, fechaPol) {
            return $.get('/Nomina/GenerarPoliza',
                {
                    nominaId,
                    fechaPol
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

        function validarNomina(nominaId) {
            return $.post('/Nomina/ValidarNomina',
                {
                    nominaId
                }).then(response => {
                    if (response.success) {
                        return response;
                    }
                    else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                });
        }

        function resumenNomina(nominaId, anio, cc, tipoNomina) {
            if (tipoNomina == "Aguinaldo") {
                return $.get('/Nomina/ResumenNominaAguinaldo',
                    {
                        anio,
                        cc
                    }).then(response => {
                        if (response.success) {
                            return response;
                        }
                        else {
                            swal('Alerta!', response.message, 'warning');
                        }
                    }, error => {
                        swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                    });
            }
            else {
                return $.get('/Nomina/ResumenNomina',
                    {
                        nominaId
                    }).then(response => {
                        if (response.success) {
                            return response;
                        }
                        else {
                            swal('Alerta!', response.message, 'warning');
                        }
                    }, error => {
                        swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                    });
            }

        }

        function getNominas(year, tipoPeriodo, periodo, tipoRaya) {
            $.get('/Nomina/GetNominas',
                {
                    year,
                    tipoPeriodo,
                    periodo,
                    tipoRaya
                }).then(response => {
                    if (response.success) {
                        AddRows(tblNomina, response.items);
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

            cboxTipoRayaFiltro.fillCombo('/Nomina/GetTipoRaya', null, false, null, null);
        }
        //#endregion

        (function init() {
            moment.locale('es');

            initCamposFechas();
            initTblNomina();
            initTblResumen();
            initTblPoliza();
            initTblEmpleadosSinRegistrar();
            llenarCombos();
            cboTipoNomina.change(function (e) {
                cboxPeriodoFiltro.fillComboGroup('/Nomina/getCbotPeriodoNomina', { tipoNomina: cboTipoNomina.val() }, false, null, () => { });
            });
        })();


    }
    $(document).ready(() => Administrativo.Contabilidad.Nomina = new Nomina())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();