$(function () {

    $.namespace('recursoshumanos.reportesrh.repmodificaciones');

    repmodificaciones = function () {
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
        btnBuscar = $("#btnBuscar"),
        btnImprimir = $("#btnImprimir"),
        tblData = $("#tblData");

        function init() {
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false,"Todos");
            convertToMultiselect("#cboCC");
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
            loadGrid(getFiltrosObject(), '/Administrativo/ReportesRH/FillGridRepModificaciones', tblData);
            loadGraph(getFiltrosObject(), '/Administrativo/ReportesRH/FillGridRepModificaciones', "myChart");
            btnImprimir.show();
        }
        function getFiltrosObject() {
            return {
                cc: getValoresMultiples("#cboCC"),
                concepto: getValoresMultiples("#cboConcepto"),
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
        function loadGraph(objetoCarga, controller, divChart) {

            $.ajax({
                url: controller,
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(objetoCarga),
                success: function (response) {
                    if (response.success) {
                        var cc = [];
                        var conceptos = ["Aditivas", "Deductivas"];
                        var cantidades = [];
                        var Unicos = Enumerable.From(response.rows).Select(function (x) { return x.cC }).Distinct().ToArray();
                        $.each(Unicos, function (i, e) {
                            cc.push(e);

                            var cantidad = [];

                            var cant = Enumerable.From(response.rows).Where(function (x) { return x.cC == e && x.concepto == "A" }).Count();
                            cantidad.push(cant);
                            var cant = Enumerable.From(response.rows).Where(function (x) { return x.cC == e && x.concepto == "D" }).Count();
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
                                    data = dataset.name;
                                    ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                });
                            });
                        }
                    }
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
            verReporte(17, "fechaInicio=" + fechaIni.val() + "&" + "fechaFin=" + fechaFin.val());
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
        init();
    }

    $(document).ready(function () {
        recursoshumanos.reportesrh.repmodificaciones = new repmodificaciones();
    });
});



