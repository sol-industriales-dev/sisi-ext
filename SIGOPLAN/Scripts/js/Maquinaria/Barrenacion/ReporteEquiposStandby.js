(() => {
    $.namespace('Barrenacion.ReporteEquiposStandby');

    ReporteEquiposStandby = function () {
        // Variables.

        // Filtros
        const comboAC = $('#comboAC');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const botonBuscar = $('#botonBuscar');
        const botonReporteGeneral = $('#botonReporteGeneral');
        const modalVerGraficas = $('#modalVerGraficas');
        const botonImprimirRpt = $("#botonImprimirRpt");
        const inputArrendamientoPuro = $('#inputArrendamientoPuro');

        // Tabla capturas
        const tablaCapturas = $('#tablaCapturas');
        let dtTablaCapturas;

        // Reporte
        const report = $("#report");
        var myChart1 = null;

        (function init() {

            $.fn.dataTable.moment('DD/MM/YYYY');
            // Lógica de inicialización.
            comboAC.fillCombo('/Barrenacion/ObtenerAC', null, false);
            initInputFechas();
            initTablaCapturas();
            agregarListeners();
            botonReporteGeneral.hide();

        })();

        function agregarListeners() {
            botonBuscar.click(cargarCapturas);
            botonReporteGeneral.click(verGrafica);
            botonImprimirRpt.click(createObjSend);
        }

        function cargarCapturas() {

            const areaCuenta = comboAC.val();

            if (areaCuenta === '') {
                AlertaGeneral(`Aviso`, `Debe seleccionar un centro de costos.`);
                return;
            }
            const fechaInicio = inputFechaInicio.val();
            const fechaFin = inputFechaFin.val();

            $.post('/Barrenacion/CargarRptEquiposstanby', { areaCuenta, fechaInicio, fechaFin })
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
                    { data: 'noEconomico', title: 'no Economico.' },
                    { data: 'combustible', title: 'Falta Combustible.' },
                    { data: 'malClima', title: 'Mal Clima' },
                    { data: 'standby', title: 'Stand by' },
                    { data: 'faltaTramo', title: 'Falta de Tramo' },
                    { data: 'mantenimiento', title: 'Mantenimiento/Reparación' }
                ]
            });
        }
        function verReporteGeneral(areaCuenta, barrenadoraID, fechaInicio, fechaFin) {
            var path = `/Reportes/Vista.aspx?idReporte=167&areaCuenta=${areaCuenta}&isCRModal=${true}&barrenadoraID=${barrenadoraID}&fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`;
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

        function verGrafica() {

            modalVerGraficas.modal('show');
            if (myChart1 != null) {
                myChart1.clear();
            }

            let barData = ['#5DADE2', '#F1948A', '#A569BD', '#58D68D', '#F39C12']
            var dataSetData = dtTablaCapturas.data().toArray().map((element, index) => {
                return {
                    label: element.noEconomico, // Equipos
                    backgroundColor: barData[index],
                    borderColor: barData[index],
                    borderWidth: 1,
                    data: [
                        element.combustible,
                        element.malClima,
                        element.standby,
                        element.faltaTramo,
                        element.mantenimiento
                    ]
                }
            });

            var barChartData = {
                labels: ['Falta Combustible', 'Mal Clima', 'Stand by', 'Falta Tramo', 'Mantenimiento/Reparación'], //Listado de insidentes
                datasets: dataSetData
            };

            var ctx = document.getElementById('LineWithLine').getContext('2d');
            myChart1 = new Chart(ctx, {
                type: 'bar',
                data: barChartData,
                options: {
                    responsive: true,
                    legend: {
                        position: 'top',
                    },
                    title: {
                        display: true,
                        text: 'Reporte de Paros Perforadoras.'
                    }
                }
            });

            myChart1.resize();

            Chart.plugins.register({
                beforeDraw: function (chartInstance) {
                    var ctx = chartInstance.chart.ctx;
                    ctx.fillStyle = "white";
                    ctx.fillRect(0, 0, chartInstance.chart.width, chartInstance.chart.height);
                }
            });
        }

        function base64ToBlob(base64, mime) {
            mime = mime || '';
            var sliceSize = 1024;
            var byteChars = window.atob(base64);
            var byteArrays = [];

            for (var offset = 0, len = byteChars.length; offset < len; offset += sliceSize) {
                var slice = byteChars.slice(offset, offset + sliceSize);

                var byteNumbers = new Array(slice.length);
                for (var i = 0; i < slice.length; i++) {
                    byteNumbers[i] = slice.charCodeAt(i);
                }

                var byteArray = new Uint8Array(byteNumbers);

                byteArrays.push(byteArray);
            }

            return new Blob(byteArrays, { type: mime });
        }

        function createObjSend() {
            var image = document.getElementById('LineWithLine').toDataURL('image/jpg');// $('#image-id').attr('src');
            var base64ImageContent = image.replace(/^data:image\/(png|jpg);base64,/, "");
            var blob = base64ToBlob(base64ImageContent, 'image/jpg');
            var formData = new FormData();
            formData.append('picture', blob);
            formData.append('dataTabla', JSON.stringify(dtTablaCapturas.data().toArray()));
            guardarContrato(formData);
        }

        function guardarContrato(objData) {
            $.ajax({
                type: 'POST',
                url: '/Barrenacion/SetReporteStandby',
                data: objData,
                contentType: false,
                processData: false,
                cache: false,
                success: function (response) {
                    var path = `/Reportes/Vista.aspx?idReporte=203`;
                    report.attr("src", path);
                    document.getElementById('report').onload = function () {
                        openCRModal();
                    };
                },
                error: function (xhr, status, error) {
                    AlertaGeneral('Error: ' + error);
                    btnGuardarContrato.parents('div.modal-footer').unblock({ message: null });
                }
            });



        }
    }

    $(() => Barrenacion.ReporteEquiposStandby = new ReporteEquiposStandby())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();