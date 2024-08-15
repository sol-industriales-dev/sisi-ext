(() => {
    $.namespace('Administrativo.Seguridad.Capacitacion.Dashboard');

    Dashboard = function () {


        // Variables.

        //Filtros
        const comboCplan = $('#comboCplan');
        const comboArr = $('#comboArr');
        const comboArea = $('#comboArea');
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

        // Empleados por expirar Protocolo Fatalidad.
        const tablaVigenciaNormativos = $('#tablaVigenciaNormativos');
        let dtTablaVigenciaNormativos;

        //Empleados por expirar Procedimiento Operativo.
        const tablaExpProcOper = $('#tablaExpProcOper');
        let dtTablaExpProcOper;

        //Empleados por expirar Instructivo Operativo.
        const tablaExpInstOper = $('#tablaExpInstOper');
        let dtTablaExpInstOper;

        const spanProtocoloVencidos = $('#spanProtocoloVencidos');
        const spanProtocoloProximosVencer = $('#spanProtocoloProximosVencer');
        const spanNormativoVencidos = $('#spanNormativoVencidos');
        const spanNormativoProximosVencer = $('#spanNormativoProximosVencer');
        const spanInstructivoVencidos = $('#spanInstructivoVencidos');
        const spanInstructivoProximosVencer = $('#spanInstructivoProximosVencer');
        const spanTecnicoVencidos = $('#spanTecnicoVencidos');
        const spanTecnicoProximosVencer = $('#spanTecnicoProximosVencer');

        const botonExcelExpirados = $('#botonExcelExpirados');

        let _graficaPorcentajeCursos;

        (function init() {
            $('.comboChange').change(cargarAreasCC);

            // Lógica de inicialización.
            $('div.panel.panel-primary.oculto').hide();
            divTabs.hide();
            llenarCombos();
            initDatepickers();

            initTablaEmpleadosPorExpirar();
            initTablaVigenciaNormativos();
            initTablaExpProcOper();
            initTablaExpInstOper();

            agregarListeners();
        })();

        // Métodos.
        function agregarListeners() {
            botonBuscar.click(obtenerValoresGenerales);
            botonExcelExpirados.click(descargarExcelExpirados);
        }

        function obtenerValoresGenerales() {
            const ccsCplan = getValoresMultiples('#comboCplan');
            const ccsArr = getValoresMultiples('#comboArr');
            const departamentosIDs = getValoresMultiples('#comboArea');
            const fechaInicio = inputFechaInicio.val();
            const fechaFin = inputFechaFin.val();
            const clasificacion = $('#comboClasificacion option:selected').toArray().map(item => item.text);

            if (ccsCplan.length == 0 && ccsArr.length == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar un centro de costo de por lo menos una empresa.`);
                return;
            }

            $.post('/Administrativo/Capacitacion/CargarDatosGeneralesDashboard', { ccsCplan, ccsArr, departamentosIDs, fechaInicio, fechaFin, clasificacion })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        mostrarPaneles(response.datos);

                        spanProtocoloVencidos.text(response.expProtocoloFatalidad.filter((x) => { return x.expirado; }).length);
                        spanProtocoloProximosVencer.text(response.expProtocoloFatalidad.filter((x) => { return !x.expirado; }).length);
                        spanNormativoVencidos.text(response.expNormativos.filter((x) => { return x.expirado; }).length);
                        spanNormativoProximosVencer.text(response.expNormativos.filter((x) => { return !x.expirado; }).length);
                        spanInstructivoVencidos.text(response.expInstructivoOperativos.filter((x) => { return x.expirado; }).length);
                        spanInstructivoProximosVencer.text(response.expInstructivoOperativos.filter((x) => { return !x.expirado; }).length);
                        spanTecnicoVencidos.text(response.expProcedimientosOperativos.filter((x) => { return x.expirado; }).length);
                        spanTecnicoProximosVencer.text(response.expProcedimientosOperativos.filter((x) => { return !x.expirado; }).length);

                        botonExcelExpirados.attr('disabled', false);

                        cargarTablaEmpleadosPorExpirar(response.expProtocoloFatalidad);
                        cargarTablaVigenciaNormativos(response.expNormativos);
                        cargarTablaExpProcOper(response.expProcedimientosOperativos);
                        cargarTablaExpInstOper(response.expInstructivoOperativos);
                        Chart.helpers.each(Chart.instances, function (instance) { instance.chart.destroy(); });
                        cargarChart2(response.HHClasificacion);
                        cargarChart1(response.cursosImpartidos);
                        initGraficaPorcentajeCursos(response.porcentajeVigentes); // cargarChart3(response.porcentajeVigentes);
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
                        // animateValue(tuple.Item1, 0, tuple.Item2, 3000);
                        $(`#${tuple.Item1}`).text(tuple.Item2);
                    }
                }, counter);
                counter += 1000;
            });
            setTimeout(() => {
                $("#div-graficavigentes").show(1000);
            }, counter);
            counter += 1000;
            setTimeout(() => {
                _graficaPorcentajeCursos.reflow();
            }, counter);
            setTimeout(() => {
                $("#div-expirados").show(1000);
            }, counter);
            counter += 1000;
            setTimeout(() => {
                $("#divVigenciaNormativos").show(1000);
            }, counter);
            setTimeout(() => {
                $("#div-expiradosProcOper").show(1000);
            }, counter);
            setTimeout(() => {
                $("#div-expiradosInstOper").show(1000);
            }, counter);


        }

        function descargarExcelExpirados() {
            location.href = `DescargarExcelExpirados`;
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
        }

        function initGraficaPorcentajeCursos(datos) {
            let colores = dynamicColors(datos.length, 0.4);
            let datosColores = [];

            for (let i = 0; i < datos.length; i++) {
                datosColores.push({ y: datos[i].porcentaje, color: colores[i] });
            }

            _graficaPorcentajeCursos = Highcharts.chart('graficaPorcentajeCursos', {
                chart: { type: 'bar' },
                lang: highChartsDicEsp,
                title: { text: '', enabled: false },
                xAxis: {
                    categories: datos.map((x) => { return x.curso }),
                    crosshair: true,
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: { text: '' },
                    labels: { format: '{value}%' }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: `
                        <tr>
                            <td style="color:{series.color};padding:0">{series.name}: </td>
                            <td><b>{point.y}%</b></td>
                        </tr>`,
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    },
                    series: {
                        dataLabels: {
                            enabled: true,
                            // color: '#000',
                            align: 'left',
                            formatter: function () { return this.x + ': ' + this.y + '%' },
                            inside: true,
                            // rotation: 270
                        },
                        pointPadding: 0.1,
                        groupPadding: 0
                    }
                },
                series: [
                    {
                        name: 'Porcentaje',
                        data: datosColores,
                        dataLabels: {
                            inside: true
                        }
                    }
                ],
                credits: { enabled: false },
                legend: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function llenarCombos() {
            convertToMultiselect('#comboArea');
            comboCplan.fillCombo('/Administrativo/Capacitacion/ObtenerComboCCEnKontrol', { empresa: 1 }, false, 'Todos');
            convertToMultiselect('#comboCplan');

            comboArr.fillCombo('/Administrativo/Capacitacion/ObtenerComboCCEnKontrol', { empresa: 2 }, false, 'Todos');
            convertToMultiselect('#comboArr');

            comboClasificacion.fillCombo('/Administrativo/Capacitacion/GetClasificacionCursos', null, true);
            convertToMultiselectSelectAll('#comboClasificacion');
            comboClasificacion.multiselect('selectAll', false).multiselect('updateButtonText');
        }

        function cargarAreasCC() {
            const ccsCplan = getValoresMultiples('#comboCplan');
            const ccsArr = getValoresMultiples('#comboArr');

            if (ccsCplan.length == 0 && ccsArr.length == 0) {
                comboArea.empty();
                convertToMultiselect('#comboArea');
                return;
            }

            $.post('/Administrativo/Capacitacion/ObtenerAreasPorCC', { ccsCplan, ccsArr })
                .then(response => {
                    comboArea.empty();

                    if (response.success) {
                        // Operación exitosa.
                        const todosOption = `<option value="Todos">Todos</option>`;
                        comboArea.append(todosOption);

                        response.items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}">${y.Text}</option>`;
                            });
                            comboArea.append(groupOption);
                        });
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }

                    convertToMultiselect('#comboArea');
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
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
                rowCallback: function (row, data, index) {
                    if (data.expirado) {
                        $(row).find('td:eq(3)').css('background-color', 'red');
                        $(row).find('td:eq(3)').css('color', 'white');
                    } else {
                        $(row).find('td:eq(3)').css('background-color', 'cornflowerblue');
                        $(row).find('td:eq(3)').css('color', 'black');
                    }

                    // var fechaActual = new Date();
                    // var milli = data.fechaExpiracion.replace(/\/Date\((-?\d+)\)\//, '$1');
                    // var fechaEntrada = new Date(parseInt(milli));
                    // if (fechaEntrada > fechaActual) {
                    //     $(row).find('td:eq(3)').css('background-color', 'cornflowerblue');
                    //     $(row).find('td:eq(3)').css('color', 'black');
                    // }
                    // else {
                    //     $(row).find('td:eq(3)').css('background-color', 'red');
                    //     $(row).find('td:eq(3)').css('color', 'white');
                    // }
                },
                columnDefs: [
                    { "visible": false, "targets": [3] },
                    { className: "dt-center columna_bold", "targets": [4] },
                    { className: "dt-center", "targets": [0, 1, 2, 3] }
                ]
            });

            dtTablaEmpleadosPorExpirar.buttons().container().appendTo($('#divTablaEmpleadosPorExpirar'));
        }

        function initTablaVigenciaNormativos() {
            dtTablaVigenciaNormativos = tablaVigenciaNormativos.DataTable({
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
                rowCallback: function (row, data, index) {
                    if (data.expirado) {
                        $(row).find('td:eq(3)').css('background-color', 'red');
                        $(row).find('td:eq(3)').css('color', 'white');
                    } else {
                        $(row).find('td:eq(3)').css('background-color', 'cornflowerblue');
                        $(row).find('td:eq(3)').css('color', 'black');
                    }

                    // var fechaActual = new Date();
                    // var milli = data.fechaExpiracion.replace(/\/Date\((-?\d+)\)\//, '$1');
                    // var fechaEntrada = new Date(parseInt(milli));
                    // if (fechaEntrada > fechaActual) {
                    //     $(row).find('td:eq(3)').css('background-color', 'cornflowerblue');
                    //     $(row).find('td:eq(3)').css('color', 'black');
                    // }
                    // else {
                    //     $(row).find('td:eq(3)').css('background-color', 'red');
                    //     $(row).find('td:eq(3)').css('color', 'white');
                    // }
                },
                columnDefs: [
                    { "visible": false, "targets": [3] },
                    { className: "dt-center columna_bold", "targets": [4] },
                    { className: "dt-center", "targets": [0, 1, 2, 3] }
                ]
            });

            dtTablaVigenciaNormativos.buttons().container().appendTo($('#divTablaVigenciaNormativos'));
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
                rowCallback: function (row, data, index) {
                    if (data.expirado) {
                        $(row).find('td:eq(3)').css('background-color', 'red');
                        $(row).find('td:eq(3)').css('color', 'white');
                    } else {
                        $(row).find('td:eq(3)').css('background-color', 'cornflowerblue');
                        $(row).find('td:eq(3)').css('color', 'black');
                    }

                    // var fechaActual = new Date();
                    // var milli = data.fechaExpiracion.replace(/\/Date\((-?\d+)\)\//, '$1');
                    // var fechaEntrada = new Date(parseInt(milli));
                    // if (fechaEntrada > fechaActual) {
                    //     $(row).find('td:eq(3)').css('background-color', 'cornflowerblue');
                    //     $(row).find('td:eq(3)').css('color', 'black');
                    // }
                    // else {
                    //     $(row).find('td:eq(3)').css('background-color', 'red');
                    //     $(row).find('td:eq(3)').css('color', 'white');
                    // }
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
                rowCallback: function (row, data, index) {
                    if (data.expirado) {
                        $(row).find('td:eq(3)').css('background-color', 'red');
                        $(row).find('td:eq(3)').css('color', 'white');
                    } else {
                        $(row).find('td:eq(3)').css('background-color', 'cornflowerblue');
                        $(row).find('td:eq(3)').css('color', 'black');
                    }

                    // var fechaActual = new Date();
                    // var milli = data.fechaExpiracion.replace(/\/Date\((-?\d+)\)\//, '$1');
                    // var fechaEntrada = new Date(parseInt(milli));
                    // if (fechaEntrada > fechaActual) {
                    //     $(row).find('td:eq(3)').css('background-color', 'cornflowerblue');
                    //     $(row).find('td:eq(3)').css('color', 'black');
                    // }
                    // else {
                    //     $(row).find('td:eq(3)').css('background-color', 'red');
                    //     $(row).find('td:eq(3)').css('color', 'white');
                    // }
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

        function cargarTablaVigenciaNormativos(data) {
            dtTablaVigenciaNormativos.clear();
            dtTablaVigenciaNormativos.rows.add(data);
            dtTablaVigenciaNormativos.draw();
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

    $(() => Administrativo.Seguridad.Capacitacion.Dashboard = new Dashboard())
        .ajaxStart(() => {
            $.blockUI({ message: 'Procesando...' })
            $(`#icoCirculo`).addClass(`fa-spin`);
        })
        .ajaxStop(() => {
            $.unblockUI();
            $(`#icoCirculo`).removeClass(`fa-spin`);
        });
})();