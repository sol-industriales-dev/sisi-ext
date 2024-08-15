$(function () {
    $.namespace('recursoshumanos.reportesrh.repcambios');

    repcambios = function () {
        mensajes = {
            NOMBRE: 'Reporte Captura Horometro',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },
        cboConcepto = $("#cboConcepto"),
        cboCC = $("#cboCC"),
        fechaIni = $("#fechaIni"),
        fechaFin = $("#fechaFin"),
        cboEmpleado = $("#cboEmpleado"),
        btnBuscar = $("#btnBuscar"),
        btnImprimir = $("#btnImprimir"),
        tblData = $("#tblData");

        function init() {
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");
            cboCC.change(fnGetEmpleadoByCC);
            cboEmpleado.fillCombo('/Administrativo/ReportesRH/FillComboEmpleado', { cc: getValoresMultiples("#cboCC") }, false, "Todos");
            convertToMultiselect("#cboEmpleado");
            cboConcepto.fillCombo('/Administrativo/ReportesRH/FillComboCambio', { est: true }, false, "Todos");
            convertToMultiselect("#cboConcepto");
            datePicker();
            var now = new Date(),
            year = now.getYear() + 1900;
            fechaIni.datepicker().datepicker("setDate", new Date());
            fechaFin.datepicker().datepicker("setDate", new Date());
            btnBuscar.click(clickBuscar);
            btnImprimir.click(clickImprimir);
        }
        function clickBuscar() {
            if (validateBuscar()) {
                filtrarGrid();
            }
        }
        function filtrarGrid() {
            //  loadGrid(getFiltrosObject(), '/Administrativo/ReportesRH/FillGridRepCambios', tblData);
            bootG('/Administrativo/ReportesRH/FillGridRepCambios');
            loadGraph(getFiltrosObject(), '/Administrativo/ReportesRH/FillGridRepCambios', "myChart");
            btnImprimir.show();
        }

        function bootG(url) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: url,
                type: "POST",
                datatype: "json",
                data: { obj: getFiltrosObject() },
                success: function (response) {
                    $.unblockUI();
                    var reporte = response.rows;
                    tblData.bootgrid("clear");
                    tblData.bootgrid("append", reporte);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }



        function getFiltrosObject() {
            return {
                cc: getValoresMultiples("#cboCC"),
                concepto: getValoresMultiples("#cboConcepto"),
                empleado: getValoresMultiples("#cboEmpleado"),
                fechaInicio: fechaIni.val(),
                fechaFin: fechaFin.val()
            }
        }
        function datePicker() {
            var now = new Date(),
            year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
              from = $("#fechaIni")
                .datepicker({
                    changeMonth: true,
                    changeYear: true,
                    numberOfMonths: 1,
                    defaultDate: new Date(year, 00, 01),
                    maxDate: new Date(year, 11, 31),

                    onChangeMonthYear: function (y, m, i) {
                        var d = i.selectedDay;
                        $(this).datepicker('setDate', new Date(y, m - 1, d));
                        $(this).trigger('change');
                    }

                })
                .on("change", function () {
                    to.datepicker("option", "minDate", getDate(this));
                }),
              to = $("#fechaFin").datepicker({
                  changeMonth: true,
                  changeYear: true,
                  numberOfMonths: 1,
                  defaultDate: new Date(),
                  maxDate: new Date(year, 11, 31),
                  onChangeMonthYear: function (y, m, i) {
                      var d = i.selectedDay;
                      $(this).datepicker('setDate', new Date(y, m - 1, d));
                      $(this).trigger('change');
                  }
              })
              .on("change", function () {
                  from.datepicker("option", "maxDate", getDate(this));
              });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
        }
        function validateBuscar() {
            var state = true;
            if (!validarCampo(cboCC)) { state = false; }
            if (!validarCampo(fechaIni)) { state = false; }
            if (!validarCampo(fechaFin)) { state = false; }
            if (!state) AlertaGeneral("Alerta", "Seleccione al menos un centro de costo");
            return state;
        }
        function fnGetEmpleadoByCC() {
            cboEmpleado.fillCombo('/Administrativo/ReportesRH/FillComboEmpleado', { cc: getValoresMultiples("#cboCC") }, false, "Todos");
            convertToMultiselect("#cboEmpleado");
        }
        function loadGraph(objetoCarga, controller, divChart) {

            $.ajax({
                url: controller,
                type: "POST",
                datatype: "json",
                data: { obj: getFiltrosObject() },
                success: function (response) {
                    if (response.success) {
                        var cc = [];
                        
                        var cantidades = [];
                        var conceptos = ["Puesto", "Sueldo", "Jefe Inmediato", "CC", "Registro Patronal","Tipo de Nomina"];
                        var Unicos = Enumerable.From(response.rows).Select(function (x) { return x.cC }).Distinct().ToArray();

                        $.each(Unicos, function (i, a) {
                            cc.push(a);
                            var cantidad = [];
                            var cant = Enumerable.From(response.rows).Where(function (x) { return x.cC == a && x.cPuesto == 1 }).Count();
                            cantidad.push(cant);
                            var cant = Enumerable.From(response.rows).Where(function (x) { return x.cC == a && x.cSueldo == 1 }).Count();
                            cantidad.push(cant);
                            var cant = Enumerable.From(response.rows).Where(function (x) { return x.cC == a && x.cJeIn == 1 }).Count();
                            cantidad.push(cant);
                            var cant = Enumerable.From(response.rows).Where(function (x) { return x.cC == a && x.cCC == 1 }).Count();
                            cantidad.push(cant);
                            var cant = Enumerable.From(response.rows).Where(function (x) { return x.cC == a && x.cRePa == 1 }).Count();
                            cantidad.push(cant);
                            var cant = Enumerable.From(response.rows).Where(function (x) { return x.cC == a && x.cTiN == 1 }).Count();
                            cantidad.push(cant);


                            cantidades.push(cantidad);

                        });

                        BarChart(Unicos, conceptos, cantidades, divChart);
                    }
                    else {

                    }
                },
                error: function (response) {
                    alert(response.message);
                }
            });
        }

        var myChart;
        function BarChart(cc, conceptos, cantidades, divChart) {


            var DataS = []

            for (x = 0; x < cantidades.length; x++) {
                DataS[x] = {
                    backgroundColor: 'rgba(255, 130, 35,' + 10 * (x + 1) + ')',
                    hoverBackgroundColor: 'rgba(255, 130, 35,0.5)',
                    borderColor: 'rgba(255,131,15,1)',
                    borderWidth: 1,
                    name: cc[x],
                    data: cantidades[x]
                }
            }

            var maximo = Math.max.apply(null, conceptos);
            maximo = (maximo * .2) + maximo;

            var barChartData = {
                labels: conceptos,
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
                    hover: {
                        animationDuration: 0
                    },
                    tooltips: {
                        mode: 'index',
                        callbacks: {
                            // Use the footer callback to display the sum of the items showing in the tooltip
                            footer: function (tooltipItems, data) {
                               
                                return 'Sum: ';
                            },
                        },
                        footerFontStyle: 'normal'
                    },
                    //,
                    //animation: {
                    //    duration: 1,
                    //    onComplete: function () {
                    //        var chartInstance = this.chart,
                    //            ctx = chartInstance.ctx;
                    //        ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                    //        ctx.fillStyle = "#000000";
                    //        ctx.textAlign = 'center';
                    //        ctx.textBaseline = 'bottom';

                    //        this.data.datasets.forEach(function (dataset, i) {
                    //            var meta = chartInstance.controller.getDatasetMeta(i);
                    //            meta.data.forEach(function (bar, index) {
                    //                data = dataset.name;
                    //                ctx.fillText(data, bar._model.x, bar._model.y - 5);
                    //            });
                    //        });
                    //    }
                    //}
                }
            });
            function clickHandler(evt, element) {
                if (element.length) {
                    let data = meses[element[0]._index]
                    if (getIfMeses()) {
                        modalTitle.text("Detalle por Año " + data);
                    }
                    else {
                        modalTitle.text("Detalle por mes " + data);
                    }
                    $("#tituloModalMaquina").text($("#cboFiltroGrupo option:selected").text() + " " + $("#cboFiltroNoEconomico option:selected").text());
                    cargarInicio();
                    loadTabla(getFiltrosObject(data), ruta, gridFiltros, true);
                }
            }

            //inicializarCanvas();
            //addEventListener("resize", inicializarCanvas);
        }
        function clickImprimir(e) {
            verReporte(16, "fechaInicio=" + fechaIni.val() + "&" + "fechaFin=" + fechaFin.val());
            e.preventDefault();
        }
        function verReporte(idReporte, parametros) {

            $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = idReporte;

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&" + parametros;
            ireport = $("#report");
            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();
                openCRModal();

            };
        }
        function loadGridCambios(objetoCarga, controller, grid) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: controller,
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(objetoCarga),
                success: function (response) {
                    if (response.success) {
                        grid.bootgrid("clear");
                        var JSONINFO = response.rows;
                        grid.bootgrid("append", JSONINFO);
                        grid.bootgrid('reload');
                    }
                    else {

                        AlertaGeneral("Alerta", "no se obtuvieron registros con los filtros seleccionados")
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    alert(response.message);
                }
            });
        }
        init();
    }

    $(document).ready(function () {
        recursoshumanos.reportesrh.repcambios = new repcambios();
    });
});



