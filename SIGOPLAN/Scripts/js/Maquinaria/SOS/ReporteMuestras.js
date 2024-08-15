
(function () {

    $.namespace('maquinaria.SOS.ReporteMuestras');

    ReporteMuestras = function () {

        cboFiltroLugar = $("#cboFiltroLugar"),
        fechaIni = $("#fechaIni"),
        fechaFin = $("#fechaFin"),
        lblAlerta = $("#lblAlerta"),
        lblPrecacion = $("#lblPrecacion"),
        lblNormal = $("#lblNormal"),
        modalDesglose = $("#modalDesglose"),
        tituloLegend = $("#tituloLegend"),
        btnAplicarFiltros = $("#btnAplicarFiltros"),
        titleModal = $("#titleModal"),
        dateFin = $("#fechaFin"),
        dateIni = $("#fechaIni");
        modalMensaje = $("#modalMensaje");
        lblMensaje = $("#lblMensaje");

        mensajes = {
            NOMBRE: 'Reporte de Muestras',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        function init() {
            cboFiltroLugar.fillCombo('/SOS/cboLugares', { est: true }, false, "Todos");
            btnAplicarFiltros.click(llenarGrafica);
            datePicker();
            var now = new Date(),
            year = now.getYear() + 1900;
            dateIni.datepicker("setDate", "01/01/" + year);
            dateFin.datepicker().datepicker("setDate", new Date());
            $("#table_al").treegrid();
            convertToMultiselect("#cboFiltroLugar");
        }

        function llenarGrafica() {
            //  var dateTypeVar = $('#fechaIni').datepicker('getDate');
            //  var fechaIni = $.datepicker.formatDate('mm-dd-yy', dateTypeVar);
            //  var dateTypeVar = $('#fechaFin').datepicker('getDate');
            // var fechafin = $.datepicker.formatDate('mm-dd-yy', dateTypeVar);

            var filtroLugar = $("#cboFiltroLugar option:selected").text();
            if (cboFiltroLugar.val() != "") {
                var pieData = GetInfo($("#cboFiltroLugar option:selected").text(), fechaIni.val(), fechaFin.val());

                tituloLegend.text("Lugar: " + $("#cboFiltroLugar option:selected").text() + " del " + fechaIni.val() + " al " + fechaFin.val());
            }
            else {
                lblMensaje.text("Debe seleccionar el lugar para realizar la búsqueda");
                modalMensaje.modal("show");
            }

        }

        function GetInfo(lugar, fechaInicio, fechaFin) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SOS/cargarGraficaMuestras",
                data: { Lugar: getValoresMultiples("#cboFiltroLugar"), FechaInicio: fechaInicio, FechaFin: fechaFin },
                success: function (response) {
                    var valores = [];
                    var descripcion = [];
                    var datos = response.info;
                    var colores = [];
                    $.each(response.grafica, function () {
                        valores.push(this.total);
                        descripcion.push(this.Descripcion);
                        colores.push(getColor(this.Descripcion));
                        fillTabla(this.Descripcion, this.total);
                    });
                    renderGrafica(descripcion, valores, colores)

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function GetInfoClick(lugar, fechaInicio, fechaFin, indicador) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SOS/cargarDetalleMuestraGeneral",
                data: { Lugar: getValoresMultiples("#cboFiltroLugar"), FechaInicio: fechaInicio, FechaFin: fechaFin, TipoAlerta: indicador },
                success: function (response) {

                    fillTable(response.al, "table_al");
                    fillTable(response.cu, "table_cu");
                    fillTable(response.fe, "table_fe");
                    fillTable(response.si, "table_si");
                    titleModal.text("Particulas por millon tipo: " + (indicador == "caution" ? "precaución" : indicador));
                    modalDesglose.modal('show');
                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });

        }
        var myChart;
        function renderGrafica(descripcion, valores, colores) {
            Chart.defaults.doughnutLabels = Chart.helpers.clone(Chart.defaults.doughnut);

            var helpers = Chart.helpers;
            var defaults = Chart.defaults;

            Chart.controllers.doughnutLabels = Chart.controllers.doughnut.extend({
                updateElement: function (arc, index, reset) {
                    var _this = this;
                    var chart = _this.chart,
                        chartArea = chart.chartArea,
                        opts = chart.options,
                        animationOpts = opts.animation,
                        arcOpts = opts.elements.arc,
                        centerX = (chartArea.left + chartArea.right) / 2,
                        centerY = (chartArea.top + chartArea.bottom) / 2,
                        startAngle = opts.rotation, // non reset case handled later
                        endAngle = opts.rotation, // non reset case handled later
                        dataset = _this.getDataset(),
                        circumference = reset && animationOpts.animateRotate ? 0 : arc.hidden ? 0 : _this.calculateCircumference(dataset.data[index]) * (opts.circumference / (2.0 * Math.PI)),
                        innerRadius = reset && animationOpts.animateScale ? 0 : _this.innerRadius,
                        outerRadius = reset && animationOpts.animateScale ? 0 : _this.outerRadius,
                        custom = arc.custom || {},
                        valueAtIndexOrDefault = helpers.getValueAtIndexOrDefault;

                    helpers.extend(arc, {
                        // Utility
                        _datasetIndex: _this.index,
                        _index: index,

                        // Desired view properties
                        _model: {
                            x: centerX + chart.offsetX,
                            y: centerY + chart.offsetY,
                            startAngle: startAngle,
                            endAngle: endAngle,
                            circumference: circumference,
                            outerRadius: outerRadius,
                            innerRadius: innerRadius,
                            label: valueAtIndexOrDefault(dataset.label, index, chart.data.labels[index])
                        },

                        draw: function () {
                            var ctx = this._chart.ctx,
                                            vm = this._view,
                                            sA = vm.startAngle,
                                            eA = vm.endAngle,
                                            opts = this._chart.config.options;

                            var labelPos = this.tooltipPosition();
                            var segmentLabel = vm.circumference / opts.circumference * 100;

                            ctx.beginPath();

                            ctx.arc(vm.x, vm.y, vm.outerRadius, sA, eA);
                            ctx.arc(vm.x, vm.y, vm.innerRadius, eA, sA, true);

                            ctx.closePath();
                            ctx.strokeStyle = vm.borderColor;
                            ctx.lineWidth = vm.borderWidth;

                            ctx.fillStyle = vm.backgroundColor;

                            ctx.fill();
                            ctx.lineJoin = 'bevel';

                            if (vm.borderWidth) {
                                ctx.stroke();
                            }

                            if (vm.circumference > 0.15) { // Trying to hide label when it doesn't fit in segment
                                ctx.beginPath();
                                ctx.font = helpers.fontString(opts.defaultFontSize, opts.defaultFontStyle, opts.defaultFontFamily);
                                ctx.fillStyle = "#fff";
                                ctx.textBaseline = "top";
                                ctx.textAlign = "center";

                                // Round percentage in a way that it always adds up to 100%
                                ctx.fillText(segmentLabel.toFixed(0) + "%", labelPos.x, labelPos.y);
                            }
                        }


                    });

                    var model = arc._model;
                    model.backgroundColor = custom.backgroundColor ? custom.backgroundColor : valueAtIndexOrDefault(dataset.backgroundColor, index, arcOpts.backgroundColor);
                    model.hoverBackgroundColor = custom.hoverBackgroundColor ? custom.hoverBackgroundColor : valueAtIndexOrDefault(dataset.hoverBackgroundColor, index, arcOpts.hoverBackgroundColor);
                    model.borderWidth = custom.borderWidth ? custom.borderWidth : valueAtIndexOrDefault(dataset.borderWidth, index, arcOpts.borderWidth);
                    model.borderColor = custom.borderColor ? custom.borderColor : valueAtIndexOrDefault(dataset.borderColor, index, arcOpts.borderColor);

                    // Set correct angles if not resetting
                    if (!reset || !animationOpts.animateRotate) {
                        if (index === 0) {
                            model.startAngle = opts.rotation;
                        } else {
                            model.startAngle = _this.getMeta().data[index - 1]._model.endAngle;
                        }

                        model.endAngle = model.startAngle + model.circumference;
                    }

                    arc.pivot();
                }
            });
            if (valores.length > 0) {


                var config = {
                    type: 'doughnutLabels',

                    data: {
                        datasets: [{
                            data: valores,
                            backgroundColor: colores,
                            label: 'Dataset 1'
                        }],
                        labels: descripcion
                    },
                    options: {
                        onClick: clickHandler,
                        responsive: true,
                        animation: {
                            animateScale: true,
                            animateRotate: true
                        }
                    }
                };

                if (myChart != null) {
                    myChart.destroy();
                }
                var ctx = document.getElementById("skills").getContext("2d");
                myChart = new Chart(ctx, config);
            }
            else {
                lblMensaje.text("No se encontro información");
                modalMensaje.modal("show");
            }
        }
        function fillTable(JSONINFO, div) {
            $("." + div).html("");
            var html = '';
            $.each(JSONINFO, function (i, e) {
                html += '<div class="portlet">';
                html += ' <div class="portlet-header"><div class="keyword">Equipo:' + e.maquina + '</div></div>';
                html += '   <div class="portlet-content"><ul>';
                $.each(e.children, function (i2, e2) {
                    html += '<li>' + e2.maquina + ': ' + e2.elemento + ' PPM</li>';
                });
                html += '   </ul></div>';
                html += ' <div class="panel-footer"><span class="titulo">TOTAL PPM:</span>:' + e.elemento + '</div>';
                html += '</div>';
            });
            html += '</div>';
            $("." + div).html(html);
        }
        function myLoadFilter(data, parentId) {

            function setData(data) {
                var todo = [];
                for (var i = 0; i < data.length; i++) {
                    todo.push(data[i]);
                }
                while (todo.length) {
                    var node = todo.shift();
                    if (node.children && node.children.length) {
                        node.state = 'closed';
                        node.children1 = node.children;
                        node.children = undefined;
                        todo = todo.concat(node.children1);
                    }
                }
            }

            setData(data);
            var tg = $(this);
            var opts = tg.treegrid('options');
            opts.onBeforeExpand = function (row) {
                var divVerPlan = $("#divVerPlan");
                var divVerComentario = $("#divVerComentario");
                var btnVerComentarioClientes = $(".btnVerComentarios");
                var btnVerPlanes = $(".btnVerPlanes");
                var options = {
                    //Which button should trigger the open/close event
                    toggleBtnSelector: "[data-toggle='control-sidebar']",
                    //The sidebar selector
                    selector: ".btnVerCliente",
                    //Enable slide over content
                    slide: true
                };
                //SideBar
                $.AdminLTE.controlSidebar.activate(options);
                //Botones de agregar comentarios
                btnVerComentarioClientes.unbind();
                btnVerComentarioClientes.click(function () {
                    var cli = $(this).attr("data-ID");
                    $("#divSideCliente").attr("data-Cliente", cli);
                    cargarComentarios(cli)
                    $("#txtComentarios").val("");
                    divVerComentario.modal('show');
                });
                btnVerPlanes.unbind();
                btnVerPlanes.click(function () {
                    var agente = $(this).attr("data-ID");
                    $("#divSideCliente").attr("data-Agente", agente);
                    cargarPlanes(agente)
                    $(".cboAgentes").hide();
                    $("#txtPlanes").val("");
                    divVerPlan.modal('show');
                });
                $(".btnVerPlanesG").unbind();
                $(".btnVerPlanesG").click(function () {
                    var Zona = $(this).attr("data-ID");
                    $("#cboAgentes").fillCombo('/Agentes/obtenerListaAgentes', { Zona: Zona });
                    $(".cboAgentes").show();
                    $("#cboAgentes").val("");
                    $("#cboAgentes").trigger("change");
                    $("#txtPlanes").val("");
                    divVerPlan.modal('show');
                });
                if (row.children1) {
                    tg.treegrid('append', {
                        parent: row[opts.idField],
                        data: row.children1
                    });
                    row.children1 = undefined;
                    tg.treegrid('expand', row[opts.idField]);
                }
                return row.children1 == undefined;
            };
            return data;
        }
        function clickHandler(evt, element) {
            if (element.length) {

                indicador = element[0]._index;
                //var dateTypeVar = $('#fechaIni').datepicker('getDate');
                //var fechaIni = $.datepicker.formatDate('mm-dd-yy', dateTypeVar);
                //var dateTypeVar = $('#fechaFin').datepicker('getDate');
                //var fechafin = $.datepicker.formatDate('mm-dd-yy', dateTypeVar);

                GetInfoClick($("#cboFiltroLugar option:selected").text(), fechaIni.val(), fechaFin.val(), getValue(indicador));

            }
        }
        function getValue(valor) {
            switch (valor) {
                case 0:
                    return "alerta";
                case 1:
                    return "caution"
                case 2:
                    return "normal";
                default:
            }
        }
        function fillTabla(descripcion, valores) {
            if (descripcion == "Normal") {

                lblNormal.text(valores);
            }
            else if (descripcion == "Precaucion") {
                lblPrecacion.text(valores);
            }
            else if (descripcion == "Alerta") {
                lblAlerta.text(valores);
            }
        }
        function getColor(obj) {
            if (obj == "Normal") {
                return "#27AE60";
            }
            else if (obj == "Precaucion") {
                return "#F1C40F";
            }
            else (obj == "Alerta")
            {
                return "#E74C3C";
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
        var canvas = document.querySelector("#skills");
        var X, Y, W, H, r;
        canvas.height = 40;
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

    };

    $(document).ready(function () {

        maquinaria.SOS.ReporteMuestras = new ReporteMuestras();
    });
})();

