(() => {
    $.namespace('Barrenacion.ReporteEstadisticoOperadores');

    ReporteEstadisticoOperadores = function () {
        // Variables.

        // Filtros
        const comboAC = $('#comboAC');
        const comboOperadores = $('#comboOperadores');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const botonBuscar = $('#botonBuscar');
        const botonReporteGeneral = $('#botonReporteGeneral');

        // Tabla capturas
        const tablaCapturas = $('#tablaCapturas');
        let dtTablaCapturas;

        // Reporte
        const report = $("#report");

        (function init() {
            $.fn.dataTable.moment('DD/MM/YYYY');
            // Lógica de inicialización.
            comboAC.fillCombo('/Barrenacion/ObtenerAC', null, false);
            comboOperadores.fillCombo('/Barrenacion/ObtenerOperadores', { areaCuenta: '2-1' }, false, 'Todos');
            convertToMultiselect("#comboOperadores");
            initInputFechas();
            initTablaCapturas();
            agregarListeners();
            botonReporteGeneral.hide();

        })();

        function agregarListeners() {
            botonBuscar.click(cargarCapturas);
            botonReporteGeneral.click(cargarReporteGeneral);
        }

        function cargarReporteGeneral() {
            const areaCuenta = comboAC.val();
            //  const barrenadoraID = comboBarrenadora.val() === 'Todas' ? 0 : comboBarrenadora.val();
            const fechaInicio = inputFechaInicio.val();
            const fechaFin = inputFechaFin.val();
            verReporteGeneral(areaCuenta, fechaInicio, fechaFin);
        }

        function cargarCapturas() {

            const areaCuenta = comboAC.val();

            if (areaCuenta === '') {
                AlertaGeneral(`Aviso`, `Debe seleccionar un centro de costos.`);
                return;
            }

            const operadores = comboOperadores.val().map(officer => +officer);
            const fechaInicio = inputFechaInicio.val();
            const fechaFin = inputFechaFin.val();

            $.post('/Barrenacion/CargarRptOperadores', { areaCuenta, operadores, fechaInicio, fechaFin })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        if (dtTablaCapturas != null) {
                            dtTablaCapturas.clear().draw();
                            dtTablaCapturas.rows.add(response.items).draw();
                            if (response.items && response.items.length > 0) {
                                botonReporteGeneral.show(500);
                            } else {
                                botonReporteGeneral.hide(500);
                            }
                        }
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        botonReporteGeneral.hide(500);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    botonReporteGeneral.hide(500);
                }
                );
        }

        // Métodos.
        function initTablaCapturas() {
            dtTablaCapturas = tablaCapturas.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                "scrollY": "400px",
                "scrollCollapse": true,
                columns: [
                    { data: 'operador', title: 'Nombre<br>Operador' },
                    {
                        data: 'fecha', title: 'Fecha<br>Captura', render: function (data, type, row, meta) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        }
                    },
                    { data: 'turno', title: 'Turno' },
                    {
                        data: 'barrenos', title: 'No.<br>Barrenos',
                        createdCell: (td, data, rowdata) => $(td).html(numberWithCommas(data.toFixed(2)))
                    },
                    {
                        data: 'rehabilitacion', title: 'No.<br>Rehab.',
                        createdCell: (td, data, rowdata) => $(td).html(numberWithCommas(data.toFixed(2)))
                    },
                    {
                        data: 'metrosLineales', title: 'ML',
                        createdCell: (td, data, rowdata) => $(td).html(numberWithCommas(data.toFixed(2)))
                    },
                    {
                        data: 'metrosLinealesEfectivos', title: 'ML<br>Efectivos',
                        createdCell: (td, data, rowdata) => $(td).html(numberWithCommas(data.toFixed(2)))
                    },
                    {
                        data: 'bordo', title: 'Bordo',
                        createdCell: (td, data, rowdata) => $(td).html(numberWithCommas(data.toFixed(2)))

                    },
                    {
                        data: 'espaciamiento', title: 'Espacia-<br>miento',
                        createdCell: (td, data, rowdata) => $(td).html(numberWithCommas(data.toFixed(2)))
                    },
                    {
                        data: 'densidadMaterial', title: 'Densidad<br>Material',
                        createdCell: (td, data, rowdata) => $(td).html(numberWithCommas(data.toFixed(2)))
                    },
                    {
                        data: 'm3', title: 'M3',
                        createdCell: (td, data, rowdata) => $(td).html(numberWithCommas(data.toFixed(2)))
                    },
                    {
                        data: 'metrolinealHr', title: 'ML/HR',
                        createdCell: (td, data, rowdata) => $(td).html(numberWithCommas(data.toFixed(2)))
                    },
                    {
                        data: 'toneladaHR', title: 'TON/HR',
                        createdCell: (td, data, rowdata) => $(td).html(numberWithCommas(data.toFixed(2)))
                    },
                    {
                        data: 'm3HR', title: 'M3/HR',
                        createdCell: (td, data, rowdata) => $(td).html(numberWithCommas(data.toFixed(2)))
                    }
                ],
                footerCallback: function ( row, data, start, end, display ) {
                    var api = this.api(), data; 
                    var intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '')*1 : typeof i === 'number' ? i : 0;
                    };
 
                    total3 = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total4 = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total5 = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total6 = api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total7 = api.column(7).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total8 = api.column(8).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total9 = api.column(9).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total10 = api.column(10).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total11 = api.column(11).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total12 = api.column(12).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total13 = api.column(13).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
 
                    if(data.length > 0) {
                        $(row).find("td").eq(1).html(numberWithCommas((total3 / data.length).toFixed(2)));
                        $(row).find("td").eq(2).html(numberWithCommas((total4 / data.length).toFixed(2)));
                        $(row).find("td").eq(3).html(numberWithCommas((total5 / data.length).toFixed(2)));
                        $(row).find("td").eq(4).html(numberWithCommas((total6 / data.length).toFixed(2)));
                        $(row).find("td").eq(5).html(numberWithCommas((total7 / data.length).toFixed(2)));
                        $(row).find("td").eq(6).html(numberWithCommas((total8 / data.length).toFixed(2)));
                        $(row).find("td").eq(7).html(numberWithCommas((total9 / data.length).toFixed(2)));
                        $(row).find("td").eq(8).html(numberWithCommas((total10 / data.length).toFixed(2)));
                        $(row).find("td").eq(9).html(numberWithCommas((total11 / data.length).toFixed(2)));
                        $(row).find("td").eq(10).html(numberWithCommas((total12 / data.length).toFixed(2)));
                        $(row).find("td").eq(11).html(numberWithCommas((total13 / data.length).toFixed(2)));
                    }
                }
            });
        }


        function numberWithCommas(x) {
            return x.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
        }


        function verReporteGeneral() {
            var path = `/Reportes/Vista.aspx?idReporte=205`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function initInputFechas() {
            inputFechaInicio.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": new Date()
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", new Date());

            inputFechaFin.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": new Date()
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", new Date());
        }
    }

    $(() => Barrenacion.ReporteEstadisticoOperadores = new ReporteEstadisticoOperadores())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();