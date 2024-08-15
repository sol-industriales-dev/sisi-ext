(() => {
    $.namespace('Barrenacion.ReporteRendimiento');

    ReporteRendimiento = function () {

        // Variables.

        // Filtros
        const comboAC = $('#comboAC');
        const comboTipoPieza = $('#comboTipoPieza');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const botonBuscar = $('#botonBuscar');

        // Tabla capturas
        const tablaPiezas = $('#tablaPiezas');
        let dtTablaPiezas;


        // Reporte
        const report = $("#report");


        (function init() {
            // Lógica de inicialización.

            comboAC.fillCombo('/Barrenacion/ObtenerAC', null, false);
            cargarTipoPieza();

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

            agregarListeners();

            initTablaPiezas();

        })();

        // Métodos.
        function agregarListeners() {
            botonBuscar.click(cargarRendimientoPiezas);
        }

        function cargarRendimientoPiezas() {

            const areaCuenta = comboAC.val();

            if (areaCuenta === '') {
                AlertaGeneral(`Aviso`, `Debe seleccionar un centro de costos.`);
                return;
            }

            const tipoPieza = comboTipoPieza.val().map((r) => { return +r });
            const fechaInicio = inputFechaInicio.val();
            const fechaFin = inputFechaFin.val();

            $.post('/Barrenacion/CargarRendimientoPiezas', { areaCuenta, tipoPieza: tipoPieza, fechaInicio, fechaFin })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.

                        if (dtTablaPiezas != null) {
                            dtTablaPiezas.clear().draw();
                            dtTablaPiezas.rows.add(response.items).draw();
                        }

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );

        }

        function initTablaPiezas() {
            dtTablaPiezas = tablaPiezas.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: false,
                columns: [
                    {
                        data: 'tipoPieza', title: 'Tipo de Pieza'
                    },
                    {
                        data: 'noSerie', title: 'No. Serie'
                    },
                    {
                        data: 'barrenadora', title: 'Barrenadora'
                    },
                    {
                        data: 'horasTrabajadas', title: 'Horas Trabajadas', render: (data, type, row) => parseFloat(data).toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
                    },
                    {
                        data: 'totalBarrenos', title: 'totalBarrenos', render: (data, type, row) => parseFloat(data).toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
                    },
                    {
                        data: 'metrosLineales', title: 'Metros Lineales', render: (data, type, row) => parseFloat(data).toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
                    },
                    {
                        data: 'toneladasBarreno', title: 'Toneladas Barreno', render: (data, type, row) => parseFloat(data).toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
                    },
                    {
                        data: 'toneladasBarrenoRealizados', title: 'Toneladas Barreno Realizados', render: (data, type, row) => parseFloat(data).toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
                    },
                    {
                        data: 'metrosCubicos', title: 'Metros Cúbicos', render: (data, type, row) => parseFloat(data).toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
                    },
                    {
                        data: 'piezaID', title: 'Reporte', render: (data, type, row) =>
                            `<button class="btn btn-primary reporte"><i class="fas fa-print"></i></button>`
                    },

                ],
                columnDefs: [{ className: "dt-center", "targets": "_all" }],
                drawCallback: function () {
                    tablaPiezas.find('.reporte').unbind().click(function () {

                        const piezaID = dtTablaPiezas.row($(this).parents('tr')).data().piezaID;
                        const objPieza = dtTablaPiezas.row($(this).parents('tr')).data();
                        const fechaInicio = inputFechaInicio.val();
                        const fechaFin = inputFechaFin.val();
                        let noSerie = objPieza.noSerie;
                        let barrenadora = objPieza.barrenadora;
                        let horasTrabajadas = objPieza.horasTrabajadas;
                        let totalBarrenos = objPieza.totalBarrenos;
                        let metrosLineales = objPieza.metrosLineales;
                        let toneladasBarreno = objPieza.toneladasBarreno;
                        let toneladasBarrenoRealizados = objPieza.toneladasBarrenoRealizados;
                        let tipoPieza = objPieza.tipoPieza;
                        let M3 = objPieza.metrosCubicos;

                        verReporte(piezaID, fechaInicio, fechaFin, noSerie, barrenadora, horasTrabajadas, totalBarrenos, metrosLineales, toneladasBarreno, toneladasBarrenoRealizados, M3, tipoPieza);

                    });
                },
                footerCallback: function ( row, data, start, end, display ) {
                    var api = this.api(), data; 
                    var intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '')*1 : typeof i === 'number' ? i : 0;
                    }; 
                    total4 = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total5 = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total6 = api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total7 = api.column(7).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total8 = api.column(8).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total9 = api.column(9).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
 
                    if(data.length > 0) {
                        $(row).find("th").eq(1).html(numberWithCommas((total9 / data.length).toFixed(2)));
                        $(row).find("th").eq(2).html(numberWithCommas((total4 / data.length).toFixed(2)));
                        $(row).find("th").eq(3).html(numberWithCommas((total5 / data.length).toFixed(2)));
                        $(row).find("th").eq(4).html(numberWithCommas((total6 / data.length).toFixed(2)));
                        $(row).find("th").eq(5).html(numberWithCommas((total7 / data.length).toFixed(2)));
                        $(row).find("th").eq(6).html(numberWithCommas((total8 / data.length).toFixed(2)));
                    }
                }
            });
        }

        function numberWithCommas(x) {
            return x.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
        }


        function cargarTipoPieza() {
            $.get('/Barrenacion/ObtenerTiposPieza')
                .then(response => {
                    if (response) {
                        // Operación exitosa.
                        const tipoPiezaHTML = response.map(item => `<option value=${item.Value} >${item.Text}</option>`).join('');
                        comboTipoPieza.append(tipoPiezaHTML);
                        convertToMultiselect("#comboTipoPieza");
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function verReporte(piezaID, fechaInicio, fechaFin, noSerie, barrenadora, horasTrabajadas, totalBarrenos, metrosLineales, toneladasBarreno, toneladasBarrenoRealizados, M3, tipoPieza) {
            var path = `/Reportes/Vista.aspx?idReporte=166&piezaID=${piezaID}&isCRModal=${true}&fechaInicio=${fechaInicio}&fechaFin=${fechaFin}&noSerie=${noSerie}&barrenadora=${barrenadora}&horasTrabajadas=${horasTrabajadas}&totalBarrenos=${totalBarrenos}&metrosLineales=${metrosLineales}&toneladasBarreno=${toneladasBarreno}&toneladasBarrenoRealizados=${toneladasBarrenoRealizados}&tipoPieza=${tipoPieza}&M3=${M3}&tipoPieza=${tipoPieza}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

    }

    $(() => Barrenacion.ReporteRendimiento = new ReporteRendimiento())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();