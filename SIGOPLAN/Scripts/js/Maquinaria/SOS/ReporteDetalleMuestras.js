
(function () {

    $.namespace('maquinaria.SOS.ReporteDetalleMuestras');

    ReporteDetalleMuestras = function () {

        cboFiltroLugar = $("#cboFiltroLugar"),
        cboFiltroMaquinaria = $("#cboFiltroMaquinaria"),
        cboFiltroElemento = $("#cboFiltroElemento"),
        cboComponente = $("#cboComponente"),
        cboFiltroModelo = $("#cboFiltroModelo"),
        fechaIni = $("#fechaIni"),
        fechaFin = $("#fechaFin"),
        lblAlerta = $("#lblAlerta"),
        lblPrecacion = $("#lblPrecacion"),
        lblNormal = $("#lblNormal"),
        tituloLegend = $("#tituloLegend"),
        btnAplicarFiltros = $("#btnAplicarFiltros"),
        btnVerDesglose = $("#btnVerDesglose"),
        modalDesglose = $("#modalDesglose"),
        titleModal = $("#titleModal"),
        cboTipoInformacion = $("#cboTipoInformacion");

        mensajes = {
            NOMBRE: 'Reporte detalle de muestras',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        function init() {
            cboFiltroLugar.addClass('required');
            cboFiltroElemento.addClass('required');
            fechaIni.addClass('required');
            fechaFin.addClass('required');


            cboComponente.fillCombo('/SOS/cboComponente');
            cboFiltroLugar.fillCombo('/SOS/cboLugares', { est: true }, false, "Todos");
            cboFiltroLugar.change(FillCboModelo);

            cboFiltroElemento.fillCombo('/SOS/FillCbo_Elementos');
            btnAplicarFiltros.click(llenarGrafica);
            btnVerDesglose.click(openModal);
            tabs();
            datePicker();
            var now = new Date(),
            year = now.getYear() + 1900;
            fechaIni.datepicker("setDate", "01/01/" + year);
            fechaFin.datepicker().datepicker("setDate", new Date());

            convertToMultiselect("#cboFiltroLugar");

        }

        function FillCboModelo() {
            if (cboFiltroLugar.val() != null && cboFiltroLugar.val() != "") {
                cboFiltroModelo.fillCombo('/SOS/cboModelo', { lugar: $("#cboFiltroLugar option:selected").text() });
              //  cboFiltroMaquinaria.fillCombo('/SOS/cboMaquinaMultiple', { lugar: $("#cboFiltroLugar option:selected").text() });
                cboFiltroMaquinaria.fillCombo('/SOS/cboMaquinaMultiple', { lugar: getValoresMultiples("#cboFiltroLugar") });

                cboFiltroMaquinaria.attr('disabled', false);
                cboFiltroModelo.attr('disabled', false);
            }
            else {
                cboFiltroMaquinaria.clearCombo();
                cboFiltroMaquinaria.attr('disabled', true);
                cboFiltroModelo.clearCombo();
                cboFiltroModelo.attr('disabled', true);
            }
        }

        function tabs() {
            $('ul.tabs li:first').addClass('active');
            $('.block article').hide();
            $('.block article:first').show();
            $('ul.tabs li').on('click', function () {
                $('ul.tabs li').removeClass('active');
                $(this).addClass('active')
                $('.block article').hide();
                var activeTab = $(this).find('a').attr('href');
                $(activeTab).show();
                return false;
            });
        }

        function openModal() {
            if (myLineChart != null) {

                modalDesglose.modal('show');
            }
            else {
                AlertaGeneral("Alerta", "no se encontró información");
            }
        }

        function llenarGrafica() {
            if (Validate()) {
                //var dateTypeVar = $('#fechaIni').datepicker('getDate');
                //var fechaIni = $.datepicker.formatDate('mm-dd-yy', dateTypeVar);
                //var dateTypeVar = $('#fechaFin').datepicker('getDate');
                //var fechafin = $.datepicker.formatDate('mm-dd-yy', dateTypeVar);

                GetInfo($("#cboFiltroLugar option:selected").text(), cboComponente.val(), cboFiltroMaquinaria.val(), cboFiltroModelo.val(), $("#cboFiltroElemento option:selected").text(), fechaIni.val(), fechaFin.val());
                tituloLegend.text("Lugar: " + $("#cboFiltroLugar option:selected").text() + " del " + fechaIni.val() + " al " + fechaFin.val());
            }
            else {
                AlertaGeneral("Alerta", "Seleccione los filtros obligatorios");
            }

        }

        function Validate() {
            var state = true;

            if (!cboFiltroLugar.valid()) { state = false; }
            if (!cboFiltroElemento.valid()) { state = false; }
            if (!fechaIni.valid()) { state = false; }
            if (!fechaFin.valid()) { state = false; }

            return state;
        }

        function GetInfo(lugar, componente, unitID, modelo, tipoElemento, fechaInicio, fechaFin) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SOS/cargarGraficaReporteMuestras",
                data: { Lugar: getValoresMultiples("#cboFiltroLugar"), Componente: componente, Unitid: unitID, Modelo: modelo, Elemento: tipoElemento, FechaInicio: fechaInicio, FechaFin: fechaFin },
                success: function (response) {

                    var alerta = [];
                    var precaucion = [];
                    var elemento = [];
                    var Etiquetas = [];
                    valor = 0;
                    var Titulo = "";
                    var tituloLabelX = "";

                    switch (cboTipoInformacion.val()) {
                        case "1":
                            Titulo = "Grafica Cantidad de muestras con el elemento "
                            tituloLabelX = "Cantidad de Muestras";
                            break;
                        case "2":
                            Titulo = "Grafica Horas de Equipo con el elemento "
                            tituloLabelX = "Horas del Equipo";
                            break;
                        case "3":
                            Titulo = "Grafica Horas de Aceite con el elemento "
                            tituloLabelX = "Horas del Aceite";
                            break;
                        default:
                    }
                    if (cboFiltroElemento.val() == 0) {
                        titleModal.text("Limites condenatorios generales");
                        var al = [];
                        var cu = [];
                        var si = [];
                        var fe = [];
                        var val = 0;
                        var descripcion = new Array();
                        $.each(response.grafica, function () {
                            al.push(this.al);
                            cu.push(this.cu);
                            si.push(this.si);
                            fe.push(this.fe);
                            val = val + 1;
                            Etiquetas.push(val);
                            var obj = {};
                            obj.maquina=this.name;
                            obj.fecha=this.fecha;
                            descripcion.push(obj);
                        });

                        graficaTotal(al, cu, fe, si, Etiquetas, descripcion);
                        createTableTotal();
                       // createTable();

                        var JSONINFO = response.tabla;
                        $("#grid_Todos").bootgrid("append", JSONINFO);
                        $("#grid_Todos").bootgrid('reload');
                        
                    }
                    else {
                        var descripcion = new Array();

                        titleModal.text("Limites condenatorios del " + tipoElemento);
                        $.each(response.grafica, function () {
                            elemento.push(this.indicador);
                            alerta.push(this.alerta);
                            precaucion.push(this.precaucion);
                            valor = valor + 1;
                            switch (cboTipoInformacion.val()) {
                                case "1":
                                    Etiquetas.push(valor);
                                    break;
                                case "2":
                                    Etiquetas.push(this.hora_equipo);
                                    break;
                                case "3":
                                    Etiquetas.push(this.hora_Aceite);
                                    break;
                                default:
                            };
                            var obj = {};
                            obj.maquina=this.name;
                            obj.fecha=this.fecha;
                            descripcion.push(obj);
                        });
                        createTable();
                        var JSONINFO = response.tabla;
                        $("#grid_General").bootgrid("append", JSONINFO);
                        $("#grid_General").bootgrid('reload');
                        renderGrafica(elemento, alerta, precaucion, Etiquetas, tipoElemento, getColorElemento(tipoElemento), Titulo, tituloLabelX, descripcion);
                    }
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        var myLineChart;
        function renderGrafica(elemento, alerta, precaucion, Etiquetas, tipoElemento, color, Titulo, tituloLabelX,descripcion) {

            var data = {
                labels: Etiquetas,
                datasets: [
                    {
                        label: "Alerta",
                        fill: false,
                        lineTension: 0.1,
                        backgroundColor: "#F53939",
                        borderColor: "#F53939",
                        borderJoinStyle: 'miter',
                        data: alerta
                    },
                     {
                         label: "Precaución",
                         fill: false,
                         lineTension: 0.1,
                         backgroundColor: "#F8E831",
                         borderColor: "#F8E831",
                         borderJoinStyle: 'miter',
                         data: precaucion
                     },
                     {
                         label: tipoElemento,
                         fill: false,
                         lineTension: 0.1,
                         backgroundColor: color,// "#29BCFF",
                         borderColor: color, //"#29BCFF",
                         borderJoinStyle: 'miter',
                         pointBorderColor: color,
                         data: elemento
                     }

                ]

            };
            if (myLineChart != null) {
                myLineChart.destroy();
            }
            var ctx = document.getElementById("grafica").getContext("2d");
            myLineChart = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    title: {
                        display: true,
                        text: Titulo
                    },
                    tooltips: {
                        enabled: true,
                        mode: 'single',
                        callbacks: {
                            title: function (tooltipItem, data) {
                                var obj = tooltipItem;
                                return descripcion[obj[0].index].maquina;
                            },
                            label: function (tooltipItem, data) {
                                var obj = tooltipItem;
                                return descripcion[obj.index].fecha;
                            }
                        }
                    },
                    responsive: true,
                    animation: false,
                    scales: {
                        yAxes: [{
                            scaleLabel: {
                                display: true,
                                labelString: 'Particulas Por Millon'
                            },
                            ticks: {
                                min: 0
                            }
                        }],
                        xAxes: [{
                            scaleLabel: {
                                display: true,
                                labelString: tituloLabelX
                            }
                        }]
                    }

                }
            });

            var originalGetElementAtEvent = myLineChart.getElementAtEvent;
            myLineChart.getElementAtEvent = function () {
                return originalGetElementAtEvent.apply(this, arguments).filter(function (e) {
                    return e._datasetIndex == 2;
                });
            }
        }

        function graficaTotal(Al, Cu, Fe, Si, Etiquetas, descripcion) {
            var data = {
                labels: Etiquetas,
                datasets: [
                    {
                        label: "Al",
                        fill: false,
                        lineTension: 0.1,
                        backgroundColor: "#29BCFF",
                        borderColor: "#29BCFF",
                        borderJoinStyle: 'miter',
                        data: Al
                    },
                     {
                         label: "Cu",
                         fill: false,
                         lineTension: 0.1,
                         backgroundColor: "#FFD61A",
                         borderColor: "#FFD61A",
                         borderJoinStyle: 'miter',
                         data: Cu
                     },
                     {
                         label: "Fe",
                         fill: false,
                         lineTension: 0.1,
                         backgroundColor: "#0214D1",
                         borderColor: "#0214D1",
                         borderJoinStyle: 'miter',
                         pointBorderColor: "rgba(75,192,192,1)",
                         data: Fe
                     },
                     {
                         label: "Si",
                         fill: false,
                         lineTension: 0.1,
                         backgroundColor: "#587436",
                         borderColor: "#587436",
                         borderJoinStyle: 'miter',
                         pointBorderColor: "rgba(75,192,192,1)",
                         data: Si
                     }

                ]

            };
            if (myLineChart != null) {
                myLineChart.destroy();
            }
            var ctx = document.getElementById("grafica").getContext("2d");
            myLineChart = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    animation: false,
                    tooltips: {
                        enabled: true,
                        mode: 'single',
                        callbacks: {
                            title: function (tooltipItem, data) {
                                var obj = tooltipItem;
                                return descripcion[obj[0].index].maquina;
                            },
                            label: function (tooltipItem, data) {
                                var obj = tooltipItem;
                                var t = new Array();
                                t.push(obj.xLabel);
                                t.push(descripcion[obj.index].fecha);
                                return t;
                            }
                        }
                    },

                }
            });

            //var originalGetElementAtEvent = myLineChart.getElementAtEvent;
            //myLineChart.getElementAtEvent = function () {
            //    return originalGetElementAtEvent.apply(this, arguments).filter(function (e) {
            //        return e._datasetIndex == 2;
            //    });
            //}
        }

        var canvas = document.querySelector("#grafica");

        var X, Y, W, H, r;
        canvas.height = 80;
        function inicializarCanvas() {
            if (canvas && canvas.getContext) {
                var ctx = canvas.getContext("2d");
                if (ctx) {
                    var s = getComputedStyle(canvas);
                    var w = s.width;
                    var h = s.height;

                    W = canvas.width = w.split("px")[0];
                    H = canvas.height = h.split("px")[0];
                }
            }
        }

        init();

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

        function createTable() {

            $("#tableMuestras").empty();
            $("#tableMuestras").append(
                       "<table id='grid_General' class='table table-condensed table-hover table-striped text-center'>" +
                       "<thead class='bg-table-header'>" +
                             "<tr>" +
                                "<th data-column-id='Equipo' data-align='center' data-header-align='center'>Equipo</th>" +
                                "<th data-column-id='" + $("#cboFiltroElemento option:selected").text() + "' data-align='center' data-header-align='center'data-formatter='cambiarInsumo' >" + $("#cboFiltroElemento option:selected").text() + "</th>" +

                                "<th data-column-id='caution" + $("#cboFiltroElemento option:selected").text() + "' data-align='center' data-header-align='center'>Precaución</th>" +
                                "<th data-column-id='alerta" + $("#cboFiltroElemento option:selected").text() + "' data-align='center' data-header-align='center'>Alerta</th>" +
                            "</tr>" +
                        "</thead>" +
                    "</table>");

            $("#grid_General").bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                'formatters': {
                    'cambiarInsumo': function (column, row) {
                        return "<span class='" + getElemento(column.id, row) + "'>" + row[column.id] + "</span>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function (e) {
                /* your code goes here */
                $(".alerta").parent().addClass('bg-color-red');
                $(".caution").parent().addClass('bg-color-caution');
            });;
        }


        function createTableTotal() {

            $("#tableMuestras").empty();
            $("#tableMuestras").append(
                       "<table id='grid_Todos' class='table table-condensed table-hover table-striped text-center'>" +
                       "<thead class='bg-table-header'>" +
                             "<tr>" +
                                "<th data-column-id='Equipo' data-align='center' data-header-align='center'>Equipo</th>" +
                                "<th data-column-id='description' data-align='center' data-header-align='center'>Punto de muestreo</th>" +
                                "<th data-column-id='fecha' data-align='center' data-header-align='center'>Fecha</th>" +
                                "<th data-column-id='al' data-align='center' data-header-align='center'>AL</th>" +
                                "<th data-column-id='cu' data-align='center' data-header-align='center'>CU</th>" +
                                "<th data-column-id='fe' data-align='center' data-header-align='center'>FE</th>" +
                                "<th data-column-id='si' data-align='center' data-header-align='center'>SI</th>" +
                            "</tr>" +
                        "</thead>" +
                    "</table>");

            $("#grid_Todos").bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                }
            });
        }
        function getColorElemento(color) {
            switch (color) {
                case "AL":
                    return "#29BCFF";
                case "CU":
                    return "#FFA80E";
                case "FE":
                    return "#0214D1";
                case "SI":
                    return "#587436";
                default:

            }
        }

        function getElemento(idElemento, row) {
            switch (idElemento) {
                case "AL":
                    if (row.AL >= row.alertaAL) {
                        return "alerta";
                    }
                    if (row.AL >= row.cautionAL && row.AL < row.alertaAL) {
                        return "caution";
                    }
                    else {
                        return "normal";
                    }

                case "CU":
                    if (row.CU >= row.alertaCU) {
                        return "alerta";
                    }
                    if (row.CU >= row.cautionAL && row.CU < row.alertaCU) {
                        return "caution";
                    }
                    else {
                        return "normal";
                    }
                case "FE":
                    if (row.FE >= row.alertaFE) {
                        return "alerta";
                    }
                    if (row.FE >= row.cautionFE && row.FE < row.alertaFE) {
                        return "caution";
                    }
                    else {
                        return "normal";
                    }
                case "SI":
                    if (row.SI >= row.alertaSI) {
                        return "alerta";
                    }
                    if (row.SI >= row.cautionSI && row.SI < row.alertaSI) {
                        return "caution";
                    }
                    else {
                        return "normal";
                    }
                default:

            }
        }

    };

    $(document).ready(function () {

        maquinaria.SOS.ReporteDetalleMuestras = new ReporteDetalleMuestras();
    });
})();

