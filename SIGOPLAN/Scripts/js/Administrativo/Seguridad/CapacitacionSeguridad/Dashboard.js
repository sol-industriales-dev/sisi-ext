(() => {
    $.namespace('Administrativo.Seguridad.CapacitacionSeguridad.Dashboard');

    Dashboard = function () {


        // Variables.

        //Filtros
        const comboCplan = $('#comboCplan');
        const comboArr = $('#comboArr');
        const botonBuscar = $('#botonBuscar');

        // Tabs
        const divTabs = $('#divTabs');

        // Subfiltros
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const comboClasificacion = $('#comboClasificacion');
        const botonBuscarSubfiltro = $('#botonBuscarSubfiltro');

        // Empleados por expirar Protocolo Fatalidad.
        const tablaEmpleadosPorExpirar = $('#tablaEmpleadosPorExpirar');
        let dtTablaEmpleadosPorExpirar;

        //Empleados por expirar Procedimiento Operativo.
        const tablaExpProcOper = $('#tablaExpProcOper');
        let dtTablaExpProcOper;

        //Empleados por expirar Instructivo Operativo.
        const tablaExpInstOper = $('#tablaExpInstOper');
        let dtTablaExpInstOper;

        (function init() {
            // Lógica de inicialización.
            $('div.panel.panel-primary.oculto').hide();
            divTabs.hide();
            llenarCombos();
            initDatepickers();

            initTablaEmpleadosPorExpirar();
            initTablaExpProcOper();
            initTablaExpInstOper();

            agregarListeners();
        })();

        // Métodos.
        function agregarListeners() {
            botonBuscar.click(obtenerValoresGenerales);
            // botonBuscarSubfiltro.click(obtenerValoresGenerales);
        }

        function obtenerValoresGenerales() {
            const ccsCplan = getValoresMultiples('#comboCplan');
            const ccsArr = getValoresMultiples('#comboArr');
            const fechaInicio = inputFechaInicio.val();
            const fechaFin = inputFechaFin.val();
            const clasificacion = $('#comboClasificacion option:selected').toArray().map(item => item.text);

            if (ccsCplan.length == 0 && ccsArr.length == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar un centro de costo de por lo menos una empresa.`);
                return;
            }

            $.post('/Administrativo/CapacitacionSeguridad/CargarDatosGeneralesDashboard', { ccsCplan, ccsArr, fechaInicio, fechaFin, clasificacion })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        mostrarPaneles(response.datos);
                        cargarTablaEmpleadosPorExpirar(response.expProtocoloFatalidad);
                        cargarTablaExpProcOper(response.expProcedimientosOperativos);
                        cargarTablaExpInstOper(response.expInstructivoOperativos);
                        Chart.helpers.each(Chart.instances, function (instance) { instance.chart.destroy(); });
                        cargarChart2(response.HHClasificacion);
                        cargarChart1(response.cursosImpartidos);
                        cargarChart3(response.porcentajeVigentes);
                        Chart.defaults.global.defaultFontSize = 14;
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

        function initDatepickers() {
            const fechaActual = new Date();

            var mesActual = fechaActual.getMonth();
            var anioActual = fechaActual.getFullYear();
            var diaActual = fechaActual.getDate();
            var fechaInicial = new Date(anioActual - 1, mesActual, diaActual);

            inputFechaInicio.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": fechaActual
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", fechaInicial);

            inputFechaFin.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": fechaActual
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", fechaActual);
        }

        function mostrarPaneles(datos) {

            let counter = 1000;

            divTabs.show(counter);

            datos.forEach(tuple => {
                const div = $(`#div-${tuple.Item1}`);
                setTimeout(() => {
                    div.show(1000);
                    if (tuple.Item2 > 0) {
                        animateValue(tuple.Item1, 0, tuple.Item2, 3000);
                    }
                }, counter);
                counter += 1000;
            });
            setTimeout(() => {
                $("#div-graficavigentes").show(1000);
            }, counter);
            counter += 1000;
            setTimeout(() => {
                $("#div-expirados").show(1000);
            }, counter);
            counter += 1000;
            setTimeout(() => {
                $("#div-expiradosProcOper").show(1000);
            }, counter);
            setTimeout(() => {
                $("#div-expiradosInstOper").show(1000);
            }, counter);
        }

        function initGrafica(labels, data, colores) {
            var ctx = document.getElementById('myChart').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'pie',
                defaultFontSize: 20,
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Cursos impartidos por clasificación',
                        data: data,
                        backgroundColor: colores,
                        borderWidth: 1
                    }]
                },
                options: {
                    legend: {
                        position: 'left',
                    },
                    plugins: {
                        labels: {
                            // render 'label', 'value', 'percentage', 'image' or custom function, default is 'percentage'
                            render: 'value',
                            // precision for percentage, default is 0
                            precision: 0,
                            // identifies whether or not labels of value 0 are displayed, default is false
                            showZero: true,
                            // font size, default is defaultFontSize
                            fontSize: 12,
                            // font color, can be color array for each data or function for dynamic color, default is defaultFontColor
                            // fontColor: '#fff',
                            // font style, default is defaultFontStyle
                            fontStyle: 'normal',
                            // font family, default is defaultFontFamily
                            fontFamily: "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif",
                            // draw text shadows under labels, default is false
                            textShadow: true,
                            // text shadow intensity, default is 6
                            shadowBlur: 10,
                            // text shadow X offset, default is 3
                            shadowOffsetX: -5,
                            // text shadow Y offset, default is 3
                            shadowOffsetY: 5,
                            // text shadow color, default is 'rgba(0,0,0,0.3)'
                            shadowColor: 'rgba(255,0,0,0.75)',
                            // draw label in arc, default is false
                            // bar chart ignores this
                            // arc: true,
                            // position to draw label, available value is 'default', 'border' and 'outside'
                            // bar chart ignores this
                            // default is 'default'
                            position: 'outside',
                            // draw label even it's overlap, default is true
                            // bar chart ignores this
                            overlap: true,
                            // show the real calculated percentages from the values and don't apply the additional logic to fit the percentages to 100 in total, default is false
                            showActualPercentages: true,
                            // set images when `render` is 'image'
                            images: [
                                {
                                    src: 'image.png',
                                    width: 16,
                                    height: 16
                                }
                            ],
                            // add padding when position is `outside`
                            // default is 2
                            outsidePadding: 4,
                            // add margin of text when position is `outside` or `border`
                            // default is 2
                            textMargin: 4
                        }
                    }
                }
            });
        }

        function initGrafica2(labels, data, colores) {
            var ctx = document.getElementById('myChart2').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Horas Hombre Capacitación por clasificación',
                        data: data,
                        backgroundColor: colores,
                        borderWidth: 1
                    }]
                },
                options: {
                    legend: {
                        position: 'left',
                    },
                    plugins: {
                        labels: {
                            // render 'label', 'value', 'percentage', 'image' or custom function, default is 'percentage'
                            render: 'value',
                            // precision for percentage, default is 0
                            precision: 0,
                            // identifies whether or not labels of value 0 are displayed, default is false
                            showZero: true,
                            // font size, default is defaultFontSize
                            fontSize: 12,
                            // font color, can be color array for each data or function for dynamic color, default is defaultFontColor
                            // fontColor: '#fff',
                            // font style, default is defaultFontStyle
                            fontStyle: 'normal',
                            // font family, default is defaultFontFamily
                            fontFamily: "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif",
                            // draw text shadows under labels, default is false
                            textShadow: true,
                            // text shadow intensity, default is 6
                            shadowBlur: 10,
                            // text shadow X offset, default is 3
                            shadowOffsetX: -5,
                            // text shadow Y offset, default is 3
                            shadowOffsetY: 5,
                            // text shadow color, default is 'rgba(0,0,0,0.3)'
                            shadowColor: 'rgba(255,0,0,0.75)',
                            // draw label in arc, default is false
                            // bar chart ignores this
                            // arc: true,
                            // position to draw label, available value is 'default', 'border' and 'outside'
                            // bar chart ignores this
                            // default is 'default'
                            position: 'outside',
                            // draw label even it's overlap, default is true
                            // bar chart ignores this
                            overlap: true,
                            // show the real calculated percentages from the values and don't apply the additional logic to fit the percentages to 100 in total, default is false
                            showActualPercentages: true,
                            // set images when `render` is 'image'
                            images: [
                                {
                                    src: 'image.png',
                                    width: 16,
                                    height: 16
                                }
                            ],
                            // add padding when position is `outside`
                            // default is 2
                            outsidePadding: 4,
                            // add margin of text when position is `outside` or `border`
                            // default is 2
                            textMargin: 4
                        }
                    }
                }
            });
        }

        function initGrafica3(labels, data, colores) {
            var ctx = document.getElementById('myChart3').getContext('2d');
            myChart3 = new Chart(ctx, {
                type: 'horizontalBar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Porcentaje Vigencia',
                        data: data,
                        backgroundColor: colores,
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        xAxes: [{
                            ticks: {
                                beginAtZero: true,
                                callback: function (value, index, values) {
                                    return value + "%";
                                },
                                suggestedMax: 100
                                // display: false
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                mirror: true,
                                fontStyle: "bold",
                                fontColor: 'black'
                            }
                        }]
                    },
                    maintainAspectRatio: false,
                    legend: {
                        display: false
                    }
                }
            });

            myChart3.canvas.parentNode.style.height = (data.length * 30) + 'px';

            // var sourceCanvas = myChart3.canvas;
            // var targetCtx = document.getElementById("myChart3Axis").getContext("2d");
            // targetCtx.canvas.width = myChart3.canvas.width;
            // targetCtx.drawImage(sourceCanvas, 0, myChart3.canvas.height, myChart3.canvas.width, 30, 0, 0, myChart3.canvas.width, 30);
        }

        function llenarCombos() {

            comboCplan.fillCombo('/Administrativo/CapacitacionSeguridad/ObtenerComboCCEnKontrol', { empresa: 1 }, false, 'Todos');
            convertToMultiselect('#comboCplan');

            comboArr.fillCombo('/Administrativo/CapacitacionSeguridad/ObtenerComboCCEnKontrol', { empresa: 2 }, false, 'Todos');
            convertToMultiselect('#comboArr');

            comboClasificacion.fillCombo('/Administrativo/CapacitacionSeguridad/GetClasificacionCursos', null, true);
            convertToMultiselectSelectAll('#comboClasificacion');
            comboClasificacion.multiselect('selectAll', false).multiselect('updateButtonText');
        }

        function initTablaEmpleadosPorExpirar() {
            dtTablaEmpleadosPorExpirar = tablaEmpleadosPorExpirar.DataTable({
                language: dtDicEsp,
                pageLength: 6,
                bLengthChange: false,
                searching: false,
                order: [[3, "asc"]],
                dom: 'Bfrtip',
                buttons: [{ extend: 'excelHtml5', exportOptions: { columns: [0, 1, 2, 4] }, className: 'btn btn-xs btn-success' }],
                columns: [
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'puesto', title: 'Puesto' },
                    { data: 'curso', title: 'Curso' },
                    { data: 'fechaExpiracion', title: 'Fecha Expiración' },
                    { data: 'fechaExpiracionStr', title: 'Fecha Expiración' }
                ],
                'rowCallback': function (row, data, index) {
                    var fechaActual = new Date();
                    var milli = data.fechaExpiracion.replace(/\/Date\((-?\d+)\)\//, '$1');
                    var fechaEntrada = new Date(parseInt(milli));
                    if (fechaEntrada > fechaActual) {
                        $(row).find('td:eq(3)').css('background-color', '#ffbf00');
                        $(row).find('td:eq(3)').css('color', 'black');
                    }
                    else {
                        $(row).find('td:eq(3)').css('background-color', 'red');
                        $(row).find('td:eq(3)').css('color', 'white');
                    }
                },
                columnDefs: [
                    { "visible": false, "targets": [3] },
                    { className: "dt-center columna_bold", "targets": [4] },
                    { className: "dt-center", "targets": [0, 1, 2, 3] }
                ]
            });

            dtTablaEmpleadosPorExpirar.buttons().container().appendTo($('#divTablaEmpleadosPorExpirar'));
        }

        function initTablaExpProcOper() {
            dtTablaExpProcOper = tablaExpProcOper.DataTable({
                language: dtDicEsp,
                pageLength: 6,
                bLengthChange: false,
                searching: false,
                order: [[3, "asc"]],
                dom: 'Bfrtip',
                buttons: [{ extend: 'excelHtml5', exportOptions: { columns: [0, 1, 2, 4] }, className: 'btn btn-xs btn-success' }],
                columns: [
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'puesto', title: 'Puesto' },
                    { data: 'curso', title: 'Curso' },
                    { data: 'fechaExpiracion', title: 'Fecha Expiración' },
                    { data: 'fechaExpiracionStr', title: 'Fecha Expiración' }
                ],
                'rowCallback': function (row, data, index) {
                    var fechaActual = new Date();
                    var milli = data.fechaExpiracion.replace(/\/Date\((-?\d+)\)\//, '$1');
                    var fechaEntrada = new Date(parseInt(milli));
                    if (fechaEntrada > fechaActual) {
                        $(row).find('td:eq(3)').css('background-color', '#ffbf00');
                        $(row).find('td:eq(3)').css('color', 'black');
                    }
                    else {
                        $(row).find('td:eq(3)').css('background-color', 'red');
                        $(row).find('td:eq(3)').css('color', 'white');
                    }
                },
                columnDefs: [
                    { "visible": false, "targets": [3] },
                    { className: "dt-center columna_bold", "targets": [4] },
                    { className: "dt-center", "targets": [0, 1, 2, 3] }
                ]
            });

            dtTablaExpProcOper.buttons().container().appendTo($('#divTablaExpProcOper'));
        }

        function initTablaExpInstOper() {
            dtTablaExpInstOper = tablaExpInstOper.DataTable({
                language: dtDicEsp,
                pageLength: 6,
                bLengthChange: false,
                searching: false,
                order: [[3, "asc"]],
                dom: 'Bfrtip',
                buttons: [{ extend: 'excelHtml5', exportOptions: { columns: [0, 1, 2, 4] }, className: 'btn btn-xs btn-success' }],
                columns: [
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'puesto', title: 'Puesto' },
                    { data: 'curso', title: 'Curso' },
                    { data: 'fechaExpiracion', title: 'Fecha Expiración' },
                    { data: 'fechaExpiracionStr', title: 'Fecha Expiración' }
                ],
                'rowCallback': function (row, data, index) {
                    var fechaActual = new Date();
                    var milli = data.fechaExpiracion.replace(/\/Date\((-?\d+)\)\//, '$1');
                    var fechaEntrada = new Date(parseInt(milli));
                    if (fechaEntrada > fechaActual) {
                        $(row).find('td:eq(3)').css('background-color', '#ffbf00');
                        $(row).find('td:eq(3)').css('color', 'black');
                    }
                    else {
                        $(row).find('td:eq(3)').css('background-color', 'red');
                        $(row).find('td:eq(3)').css('color', 'white');
                    }
                },
                columnDefs: [
                    { "visible": false, "targets": [3] },
                    { className: "dt-center columna_bold", "targets": [4] },
                    { className: "dt-center", "targets": [0, 1, 2, 3] }
                ]
            });

            dtTablaExpInstOper.buttons().container().appendTo($('#divTablaExpInstOper'));
        }

        function cargarTablaEmpleadosPorExpirar(expirados) {
            dtTablaEmpleadosPorExpirar.clear();
            dtTablaEmpleadosPorExpirar.rows.add(expirados);
            dtTablaEmpleadosPorExpirar.draw();
        }

        function cargarTablaExpProcOper(expirados) {
            dtTablaExpProcOper.clear();
            dtTablaExpProcOper.rows.add(expirados);
            dtTablaExpProcOper.draw();
        }

        function cargarTablaExpInstOper(expirados) {
            dtTablaExpInstOper.clear();
            dtTablaExpInstOper.rows.add(expirados);
            dtTablaExpInstOper.draw();
        }

        function dynamicColors(number, intensidad) {
            var colores = [];
            for (let i = 0; i < number; i++) {
                var r = Math.floor(Math.random() * 255);
                var g = Math.floor(Math.random() * 255);
                var b = Math.floor(Math.random() * 255);
                colores.push("rgba(" + r + "," + g + "," + b + ", " + intensidad + ")");
            }
            return colores;
        };

        function cargarChart3(data) {
            var labels = $.map(data, function (i) { return i.curso; });
            var data = $.map(data, function (i) { return i.porcentaje; });
            var colores = dynamicColors(data.length, 0.4);
            initGrafica3(labels, data, colores);
        }

        function cargarChart1(data) {
            var labels = $.map(data, function (i) { return i.Item2; });
            var data = $.map(data, function (i) { return i.Item1; });
            var colores = dynamicColors(data.length, 0.8);
            initGrafica(labels, data, colores);
        }

        function cargarChart2(data) {
            var labels = $.map(data, function (i) { return i.Item2; });
            var data = $.map(data, function (i) { return i.Item1; });
            var colores = dynamicColors(data.length, 0.8);
            initGrafica2(labels, data, colores);
        }

    }

    $(() => Administrativo.Seguridad.CapacitacionSeguridad.Dashboard = new Dashboard())
        .ajaxStart(() => {
            $.blockUI({ message: 'Procesando...' })
            $(`#icoCirculo`).addClass(`fa-spin`);
        })
        .ajaxStop(() => {
            $.unblockUI();
            $(`#icoCirculo`).removeClass(`fa-spin`);
        });
})();