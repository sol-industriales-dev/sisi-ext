(() => {
    $.namespace('Maquinaria.DocumentosPorPagar.ReporteDeAdeudoDetalle');

    ReporteDeAdeudoDetalle = function () {
        // Variables Principales.
        const comboInstituciones = $('#comboInstituciones');
        const comboTipoMoneda = $('#comboTipoMoneda');
        const btnBusqueda = $('#btnBusqueda');
        const btnPrintReporte = $('#btnPrintReporte');
        const tblReporteAdedudo = $('#tblReporteAdedudo');
        const inputFecha = $('#inputFecha');
        const comboTipoArre = $('#comboTipoArre');
        let dtTablaReporteAdedudo;
        const fechaActual = new Date();
        const report = $("#report");
        const checkDetalle = $("#checkDetalle");
        const txtEmpresaActual = $('#txtEmpresaActual');

        (function init() {
            convertToMultiselectSelectAll(comboTipoArre);
            // Lógica de inicialización.
            checkDetalle.prop('checked', true);
            fnLoadComboInstituciones();
            initTablaAdeudo();
            btnBusqueda.click(fnBuscar);
            initFechas();
            btnPrintReporte.click(fnLoadReporte);
            checkDetalle.change(fnBuscar);
        })();

        //#region Reporte de Programacion Pagos.
        function fnLoadReporte() {
            $.blockUI({ message: 'Cargando Información' });
            var idReporte = "198";
            var pInstitucion = comboInstituciones.val() == "" ? 0 : comboInstituciones.val();
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pTipoMoneda=" + comboTipoMoneda.val() + "&pInstitucion=" + pInstitucion + "&pFechaAdeudos=" + inputFecha.val();
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                openCRModal();
                $.unblockUI();
            };
            $.unblockUI();
        }
        //#endregion

        // Métodos.
        function initFechas() {
            inputFecha.datepicker({}).datepicker('setDate', fechaActual);
        }

        function fnBuscar() {
            // Get the column API object
            var column = dtTablaReporteAdedudo.column(3);
            // Toggle the visibility
            column.visible($(checkDetalle).prop('checked'));

            if (comboInstituciones.val() != "") {
                $.blockUI({ message: 'Cargando Información' });
                $.post('/Contratos/getRptAdeudosDetalle', {
                    tipoMoneda: comboTipoMoneda.val(),
                    instituciones: comboInstituciones.val(),
                    fechaFin: inputFecha.val(),
                    tipo: checkDetalle.prop('checked') ? 1 : 2,
                    tipoArre: comboTipoArre.val()
                }).always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            dtTablaReporteAdedudo.clear();
                            dtTablaReporteAdedudo.rows.add(response.reporte).draw();
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                Alert2Warning("Es necesario seleccionar una institución.");
            }
        }

        function fnLoadComboInstituciones() {
            comboInstituciones.fillCombo('/Contratos/ObtenerInstituciones', {}, true);
            // convertToMultiselect("#comboInstituciones");
            convertToMultiselectSelectAll(comboInstituciones);
        }

        function initTablaAdeudo() {
            dtTablaReporteAdedudo = tblReporteAdedudo.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                scrollY: '52vh',
                scrollCollapse: true,
                scrollX: true,
                "bLengthChange": false,
                "autoWidth": false,
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Reporte detalle Adeudos", "<center><h3>Reporte Detalle Adeudos </h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            columns: [':visible', 17, 23]
                            //columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27]
                        }
                    }
                ],
                columns: [
                    { data: 'proveedor', title: 'Proveedor' },
                    { data: 'tipoFinanciamiento', title: 'Financiamiento' },
                    { data: 'contrato', title: 'Contrato' },
                    { data: 'noEconomico', title: 'No.Eco' },
                    { data: 'fechaInicio', title: 'Fecha Inicio' },
                    { data: 'fechaFin', title: 'Fecha Terminacion' },
                    { data: 'tasaInteres', title: 'Tasa Intereses' },
                    {
                        data: 'valorFinanciado', title: 'Total Deuda', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    { data: 'moneda', title: 'Moneda' },
                    {
                        data: 'pagoMensual', title: 'Pago Mensual', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    { data: 'plazo', title: 'Plazo' },
                    {
                        data: 'intereses', title: 'Interes', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'ivaSCapital', title: 'iva SCapital', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'ivaIntereses', title: 'iva Interes', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    { data: 'fechaPago', title: 'Fecha Pago' },
                    { data: 'pagoRealizados', title: 'Pag Efe' },
                    {
                        data: 'importePagado', title: 'Importe Pagado', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    { data: 'tipoCambio', title: 'Tipo cambio', visible: false },
                    { data: 'pagosPendientes', title: 'Pagos Pendientes' },
                    {
                        data: 'saldoPendienteConversionDllsMxn', title: 'Saldo pendiente SOL', visible: false, render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'saldoPendiente', title: 'Saldo Pendiente', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'saldoCP', title: 'Saldo CP',
                        render: (data, type, row) => {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'saldoLP', title: 'Saldo LP',
                        render: (data, type, row) => {
                            return maskNumero(data);
                        }
                    },
                    { data: 'cargoObra', title: 'Cargo Obra', visible: false },
                    { data: 'division', title: 'División', visible: false },
                    { data: 'isAdmin', title: 'Admin u Obra', visible: false },
                    {
                        data: 'saldoInsoluto', title: 'Saldo Insoluto',
                        render: (data, type, row) => {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'saldoPendienteConversionMxn', title: 'Saldo pendiente MXN', visible: false, render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    {
                        // "targets": [20, 23, 24, 25],
                        // "visible": false
                    },
                    {
                        "targets": [3],
                        "visible": $(checkDetalle).prop('checked')
                    }
                ],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    for (let index = 0; index <= 27; index++) {
                        if (index == 7 || index == 16 || index == 19 || index == 20 || index == 21 || index == 22 || index == 26 || index == 27) {

                            pageTotalAbono = api.column(index, { page: 'current' }).data().reduce(function (a, b) {
                                return intVal(a) + intVal(b);
                            }, 0);

                            if (comboTipoMoneda.val() == 2) {
                                if (index == 20) {
                                    pageTotalAbono2 = api.column(19, { page: 'current' }).data().reduce(function (a, b) {
                                        return intVal(a) + intVal(b);
                                    }, 0);

                                    pageTotalAbono3 = api.column(27, { page: 'current' }).data().reduce(function (a, b) {
                                        return intVal(a) + intVal(b);
                                    }, 0);

                                    if (txtEmpresaActual.val() == 6) {
                                        $(api.column(index).footer()).html(
                                            maskNumero(pageTotalAbono) + " USD <br /> " + maskNumero(pageTotalAbono2) + " SOL <br /> " + maskNumero(pageTotalAbono3) + " MXN"
                                        );
                                    } else {
                                        $(api.column(index).footer()).html(
                                            maskNumero(pageTotalAbono) + " USD <br /> " + maskNumero(pageTotalAbono2) + " MXN"
                                        );
                                    }
                                } else {
                                    $(api.column(index).footer()).html(
                                        maskNumero(pageTotalAbono)
                                    );
                                }
                            } else {
                                $(api.column(index).footer()).html(
                                    maskNumero(pageTotalAbono)
                                );
                            }
                        }
                    }
                }
            });
        }
    }

    $(() => Maquinaria.DocumentosPorPagar.ReporteDeAdeudoDetalle = new ReporteDeAdeudoDetalle())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();