(() => {
    $.namespace('Barrenacion.ReporteCapturaDaria');

    ReporteCapturaDaria = function () {

        // Variables.

        // Filtros
        const comboAC = $('#comboAC');
        const comboTurno = $('#comboTurno');
        const comboBarrenadora = $('#comboBarrenadora');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const botonBuscar = $('#botonBuscar');
        const botonReporteGeneral = $('#botonReporteGeneral');

        // Tabla capturas
        const tablaCapturas = $('#tablaCapturas');
        let dtTablaCapturas;
        let turnos;
        var listaTurnos = [];

        // Reporte
        const report = $("#report");

        (function init() {
            comboBarrenadora.fillCombo('/Barrenacion/ObtenerBarrenadorasPorCC', { areaCuenta: comboAC.val() != "" ? comboAC.val() : 0 }, false, "Todos");
            convertToMultiselect("#comboBarrenadora");
            $.fn.dataTable.moment('DD/MM/YYYY');

            // Lógica de inicialización.
            comboAC.fillCombo('/Barrenacion/ObtenerAC', null, false, 'Todos');
            convertToMultiselect("#comboAC");

            inputFechaInicio.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": new Date()
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", new Date())

            inputFechaFin.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": new Date()
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", new Date());

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
            const barrenadoraID = comboBarrenadora.val() === 'Todas' ? 0 : comboBarrenadora.val();
            const fechaInicio = inputFechaInicio.val();
            const fechaFin = inputFechaFin.val();

            verReporteGeneral(areaCuenta, barrenadoraID, fechaInicio, fechaFin);
        }

        function cargarCapturas() {

            const areaCuenta = comboAC.val();

            if (areaCuenta == '') {
                AlertaGeneral(`Aviso`, `Debe seleccionar un centro de costos.`);
                return;
            }


            const barrenadoraID = comboBarrenadora.val() === 'Todas' ? 0 : comboBarrenadora.val();
            if (barrenadoraID == '') {
                AlertaGeneral(`Aviso`, `Debe seleccionar un Barrenadora.`);
                return;
            }
            // const turno = comboTurno.val();
            const fechaInicio = inputFechaInicio.val();
            const fechaFin = inputFechaFin.val();

            let todosTurnos = [1, 2, 3];
            if (comboTurno.val() == 0) {
                turnos = todosTurnos;
            }
            if (comboTurno.val() == 1) {
                turnos = todosTurnos[0];
            }
            if (comboTurno.val() == 2) {
                turnos = todosTurnos[1];
            }
            if (comboTurno.val() == 3) {
                turnos = todosTurnos[2];
            }
            $.post('/Barrenacion/CargarCapturasDiarias', { areaCuenta, barrenadoraID, turnos, fechaInicio, fechaFin })
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
                //"scrollCollapse": true,
                columns: [
                    {
                        data: 'fechaCapturaDate', title: 'Fecha Captura', render: function (data, type, row, meta) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        }
                    },
                    {
                        data: 'noEconomico', title: 'No. Económico'
                    },
                    {
                        data: 'horasTrabajadas', title: 'Horas Trabajadas'
                    },
                    {
                        data: 'turno', title: 'Turno'
                    },
                    {
                        data: 'operador', title: 'Operador'
                    },
                    {
                        data: 'ayudante', title: 'Ayudante'
                    },
                    {
                        data: 'tipoCaptura', title: 'Tipo de Captura'
                    },
                    {
                        data: 'metrosLineales', title: 'Metros Lineales', createdCell: (td, data, rowdata) => $(td).html(data.toFixed(2))

                    },
                    {
                        data: 'metrosLinealesHora', title: 'Metros Lineales / Hora', createdCell: (td, data, rowdata) => $(td).html(data.toFixed(2))
                    },
                    {
                        data: 'toneladas', title: 'Toneladas', createdCell: (td, data, rowdata) => $(td).html(maskNumeroNM(data))
                    },
                    {
                        data: 'toneladasHora', title: 'Toneladas / Hora', createdCell: (td, data, rowdata) => $(td).html(maskNumeroNM(data))
                    },
                    {
                        data: 'id', title: 'Reporte', render: (data, type, row) =>
                            `<button class="btn btn-primary reporte"><i class="fas fa-print"></i></button>`
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '20%', targets: [4, 5, 6] }
                ],
                drawCallback: function () {
                    tablaCapturas.find('.reporte').unbind().click(function () {
                        verReporte(dtTablaCapturas.row($(this).parents('tr')).data().id);
                    });
                },
                footerCallback: function (row, data, start, end, display) {
                    var api = this.api(), data;
                    var intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                    };

                    total7 = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total8 = api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total9 = api.column(9).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total10 = api.column(10).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);

                    if (data.length > 0) {
                        $(row).find("th").eq(1).html(numberWithCommas((total7 / data.length).toFixed(2)));
                        $(row).find("th").eq(2).html(numberWithCommas((total8 / data.length).toFixed(2)));
                        $(row).find("th").eq(3).html(numberWithCommas((total9 / data.length).toFixed(2)));
                        $(row).find("th").eq(4).html(numberWithCommas((total10 / data.length).toFixed(2)));
                    }
                }
            });
        }

        function numberWithCommas(x) {
            return x.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
        }

        function verReporte(id) {
            var path = `/Reportes/Vista.aspx?idReporte=165&id=${id}&isCRModal=${true}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function verReporteGeneral(areaCuenta, barrenadoraID, fechaInicio, fechaFin) {
            var path = `/Reportes/Vista.aspx?idReporte=167&areaCuenta=${areaCuenta}&isCRModal=${true}&barrenadoraID=${barrenadoraID}&fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }
    }

    $(() => Barrenacion.ReporteCapturaDaria = new ReporteCapturaDaria())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();