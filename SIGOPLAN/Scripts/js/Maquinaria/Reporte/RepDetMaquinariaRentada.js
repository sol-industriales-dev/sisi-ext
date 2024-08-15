(function () {

    $.namespace('maquinaria.reporte.repDetMaquinariaRentada');

    repDetMaquinariaRentada = function () {

        cboCC = $("#cboCC");
        btnBuscar = $("#btnBuscar");
        btnImprimir = $("#btnImprimir");
        txtFechaCorte = $("#txtFechaCorte");
        cboMaquinas = $("#cboMaquinas");

        function init() {
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false);
            txtFechaCorte.datepicker().datepicker("setDate", new Date());
            cboCC.keydown(function (e) {
                var code = e.keyCode || e.which;
                if (code == '13') {
                    buscarMaquinas();
                }
            });
            cboCC.change(buscarMaquinas);
            btnBuscar.click(loadReporte);
            convertToMultiselect("#cboMaquinas");
            cboMaquinas.change(loadReporte);
        }

        function buscarMaquinas() {

            cboMaquinas.fillCombo('/MaquinariaRentada/FiltrarMaquinasRentadas', { cc: cboCC.val(), fechaCorte: txtFechaCorte.val() }, false,"Todos");
            convertToMultiselect("#cboMaquinas");
            cboMaquinas.change();
            cboMaquinas.multiselect('selectAll', true);
            cboMaquinas.multiselect('deselect', "Todos");
        }

        function loadReporte() {
            $.ajax({
                url: '/MaquinariaRentada/GetlstInfoMaquina',
                type: 'POST',
                dataType: 'json',
                data: { lstEconomico: cboMaquinas.val() },
                success: function (response) {

                    

                    var i = 0;
                    var html = "";

                    var auxCantidad = [];
                    var auxMaquinaEconomico = [];
                    for (i; i < response.length; i++) {
                        html += "<div class='col-lg-4'>";
                        html += "<h3 class='text-center'>" + response[i][0].noEconomico + "</h3>";
                        auxMaquinaEconomico[i] = response[i][0].noEconomico;
                        var tabla = "";
                        if (response[i].length > 1) {
                            tabla += "<table class='row form-group col-lg-12' id='grid-basic-" + i + "'>";
                            tabla += "<thead class='bg-table-header'>"
                            tabla += "<tr>";
                            tabla += "<th class='text-center'><label>PERIODOS ANT</label></th>";
                            tabla += "<th class='text-center'><label>HRS ADICIONAL</label></th>";
                            tabla += "<th class='text-center'><label>D TRABAJADOS</label></th>";
                            tabla += "</tr>";
                            tabla += "</thead>";
                            tabla += "<tbody>";
                        }
                        
                        for (var x = 0; x < response[i].length; x++) {
                            if (x == response[i].length - 1) {
                                if (response[i].length > 1) {
                                    tabla += "</tbody>";
                                    tabla += "</table>";
                                    html += tabla;
                                }                              
                                var auxPeriodo = response[i][x].Periodo;
                                var auxBaseHorasxDia = response[i][x].HBase / 31;
                                var auxBaseHoras = response[i][x].HBase;
                                var auxRenta = response[i][x].Renta;


                                $.ajax({
                                    url: '/MaquinariaRentada/GetHorasTrabajadasPorDia',
                                    type: 'POST',
                                    async: false,
                                    dataType: 'json',
                                    data: { economico: cboMaquinas.val(), fechaDel: response[i][x].FechaDel, fechaAl: response[i][x].FechaAl },
                                    success: function (result) {
                                        
                                        var tabla = "";
                                        tabla += "<table class='row form-group col-lg-12' id='grid-basic-" + x + "'>";
                                        tabla += "<thead class='bg-table-header'>"
                                        tabla += "<tr>";
                                        tabla += "<th class='text-center' colspan='2'><label>ACTUAL</label></th>";
                                        tabla += "</tr>";
                                        tabla += "</thead>";
                                        tabla += "<tbody>";

                                        tabla += "<tr>";
                                        tabla += "<td class='text-center'>Periodo</td>";
                                        tabla += "<td class='text-center' rowspan='2'>Horas/Dia</td>";
                                        tabla += "</tr>";
                                        tabla += "<tr>";
                                        tabla += "<td class='text-center'>" + auxPeriodo + "</td>";
                                        tabla += "</tr>";

                                        var suma = 0;
                                        for (var y = 0; y < result.length; y++)
                                        {
                                            tabla += "<tr>";
                                            tabla += "<td class='text-center'>" + result[y].fecha + "</td>";
                                            tabla += "<td class='text-center'>" + result[y].horas + "</td>";
                                            tabla += "</tr>";

                                            suma += result[y].horas;
                                        }
                                    
                                    
                                        tabla += "</tbody>";
                                        tabla += "</table>";
                                        tabla += " ";

                                        html += tabla;

                                        var tablaR = "";
                                        tablaR += "<table class='row form-group col-lg-12' id='grid-basic-R'>";
                                        tablaR += "<tbody>";

                                        tablaR += "<tr>";
                                        tablaR += "<td>TOTAL</td>";
                                        tablaR += "<td class='text-center'>"+suma+"</td>";
                                        tablaR += "</tr>";

                                        tablaR += "<tr>";
                                        tablaR += "<td>HORAS BASE</td>";
                                        tablaR += "<td class='text-center'>" + auxBaseHoras + "</td>";
                                        tablaR += "</tr>";

                                        tablaR += "<tr>";
                                        tablaR += "<td>PROM. ESTIMADO</td>";
                                        tablaR += "<td class='text-center'>" + auxBaseHorasxDia.toFixed(2) + "</td>";
                                        tablaR += "</tr>";

                                        tablaR += "<tr>";
                                        tablaR += "<td>PROM. REAL</td>";
                                        var calculo;
                                        if (result.length == 0) {
                                            calculo = 0;
                                        }
                                        else {
                                            calculo = parseFloat(suma / result.length).toFixed(2);
                                        }
                                        tablaR += "<td class='text-center'>" + calculo + "</td>";
                                        tablaR += "</tr>";

                                        tablaR += "<tr>";
                                        tablaR += "<td>PROYECCION CIERRE PERIODO</td>";
                                        if (result.length == 0) {
                                            calculo = 0;
                                        }
                                        else {
                                            calculo = parseFloat((31 - result.length) * (suma / result.length)).toFixed(2);
                                        }
                                        tablaR += "<td class='text-center'>" + calculo + "</td>";
                                        tablaR += "</tr>";

                                        tablaR += "</tbody>";
                                        tablaR += "</table>";
                                        tablaR += " ";

                                        tablaR += "";


                                        tablaR += "<table class='row form-group col-lg-12' id='grid-basic-R2'>";
                                        tablaR += "<thead class='bg-table-header'>"
                                        tablaR += "<tr>";
                                        tablaR += "<th class='text-center' colspan='2'><label>IMPACTO ECONÓMICO</label></th>";
                                        tablaR += "</tr>";
                                        tablaR += "</thead>";
                                        tablaR += "<tbody>";
                                        tablaR += "<tr>";
                                        tablaR += "<td>$ RENTA PERIODO (MN)</td>";
                                        tablaR += "<td class='text-center'>" + auxRenta + "</td>";
                                        tablaR += "</tr>";

                                        tablaR += "<tr>";
                                        tablaR += "<td>$ HRS TRABAJADAS REAL (MN)</td>";
                                        if (auxBaseHoras == 0) {
                                            calculo = 0;
                                        }
                                        else {
                                            calculo = parseFloat((auxRenta / auxBaseHoras) * suma).toFixed(2);
                                        }
                                        tablaR += "<td class='text-center'>" + calculo + "</td>";
                                        tablaR += "</tr>";

                                        tablaR += "<tr>";
                                        tablaR += "<td>$ PROYECCIÓN CIERRE (MN)</td>";
                                        if (result.length == 0 || auxBaseHoras == 0) {
                                            calculo = 0;
                                        }
                                        else {
                                            calculo = parseFloat(auxRenta + (((31 - result.length) * (suma / result.length)) - auxBaseHoras) * (auxRenta / auxBaseHoras) * suma).toFixed(2);
                                        }
                                        tablaR += "<td class='text-center'>" + calculo + "</td>";
                                        tablaR += "</tr>";

                                        tablaR += "</tbody>";
                                        tablaR += "</table>";
                                        tablaR += " ";

                                        html += tablaR;

                                        

                                        
                                        var cHoras = [auxBaseHoras, suma, ((31 - result.length) * (suma / result.length)).toFixed(2)]
                                        auxCantidad[i] = cHoras;

                                        

                                        $.unblockUI();
                                    },
                                    error: function (response) {
                                    }
                                });



                            }
                            else {

                                
                                tabla += "<tr>";
                                tabla += "<td class='text-center'>" + response[i][x].Periodo + "</td>";
                                tabla += "<td class='text-center'>" + response[i][x].HTrabajadas + "</td>";
                                tabla += "<td class='text-center'>" + response[i][x].HAdicionales + "</td>";
                                tabla += "</tr>";
                            }

                        }
                        html += " </div>";
                        
                    }

                    $("#divAcomulado").html(html);

                    var tHoras = ["BASE", "TRABAJADAS", "PROYECCION"];
                    BarChart(tHoras, auxCantidad,auxMaquinaEconomico, "myChart");

                    $.unblockUI();
                },
                error: function (response) {
                }
            });
        }

        var myChart;
        function BarChart(horas, cantidad,maquinas, divChart) {

            var maximo = Math.max.apply(null, cantidad);
            maximo = (maximo * .2) + maximo;


            var cHoras1 = [];
            var cHoras2 = [];
            var cHoras3 = [];

            var count = 0;
            for (i = 0; i < cantidad.length;i++) {
                cHoras1[count] = cantidad[i][0];
                cHoras2[count] = cantidad[i][1];
                cHoras3[count] = cantidad[i][2];
                count++;
            }

            var DataS = []
            DataS[0] = {
                backgroundColor: '#6F5DD2',
                hoverBackgroundColor: 'rgba(255, 130, 35,0.5)',
                borderColor: 'rgba(255,131,15,1)',
                borderWidth: 1,
                data: cHoras1
            }
            DataS[1] = {
                backgroundColor: '#B53535',
                hoverBackgroundColor: 'rgba(255, 130, 35,0.5)',
                borderColor: 'rgba(255,131,15,1)',
                borderWidth: 1,
                data: cHoras2
            }
            DataS[2] = {
                backgroundColor: '#7AB631',
                hoverBackgroundColor: 'rgba(255, 130, 35,0.5)',
                borderColor: 'rgba(255,131,15,1)',
                borderWidth: 1,
                data: cHoras3
            }


            var barChartData = {
                labels: maquinas,
                datasets: DataS
            }


            if (myChart != null) {
                myChart.destroy();
            }

            var ctx = document.getElementById(divChart);
            myChart = new Chart(ctx, {
                type: 'bar',
                data: barChartData,
                options: {
                    //onClick: clickHandler,
                    responsive: true,
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                callback: function (value, index, values) {
                                    return value.toFixed(1);
                                },
                                stepSize: Math.trunc(maximo / horas.length)
                            }
                        }],
                        xAxes: [{
                            ticks: {
                                autoSkip: false
                            }
                        }]
                    }
                    ,
                    hover: {
                        animationDuration: 0
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;
                            ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                            ctx.fillStyle = "#000000";
                            ctx.textAlign = 'center';
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                meta.data.forEach(function (bar, index) {
                                    data = dataset.data[index];
                                    ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                });
                            });
                        }
                    }
                }
            });
            function clickHandler(evt, element) {
                if (element.length) {
                    
                }
            }
            //inicializarCanvas();
            //addEventListener("resize", inicializarCanvas);
        }

        $('#right-button').click(function () {
            event.preventDefault();
            $('#divAcomulado').animate({
                scrollLeft: "+=300px"
            }, "slow");
        });

        $('#left-button').click(function () {
            event.preventDefault();
            $('#divAcomulado').animate({
                scrollLeft: "-=300px"
            }, "slow");
        });
        init();
    };

    $(document).ready(function () {

        maquinaria.reporte.repDetMaquinariaRentada = new repDetMaquinariaRentada();
    });
})();