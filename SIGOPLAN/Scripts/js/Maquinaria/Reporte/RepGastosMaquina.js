(function () {

    $.namespace('maquinaria.reporte.repGastosMaquina');

    repGastosMaquina = function () {

        idTipo = 0;
        idGrupo = 0;
        ruta = '/RepGastosMaquinaria/fillGridReporteMaquinariaXMes';
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Reporte Gastos Maquinaria',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        cboFiltroTipo = $("#cboFiltroTipo"),
        btnReporte = $("#btnReporte"),
        cboFiltroGrupo = $("#cboFiltroGrupo"),
        modalMes = $("#modalMes"),
        gridFiltros = $("#grid_RepMensual"),
        cboAnios = $("#cboAnios"),
        modalReportes = $("#modalReportes"),
        title = $("#title"),
        lblDescripcionTipo = $("#lblDescripcionTipo"),
        modalTitle = $("#title-modal"),
        cboFiltroNoEconomico = $("#cboFiltroNoEconomico"),
        cboFiltroTipo = $("#cboFiltroTipo"),
        cboFiltroGrupo = $("#cboFiltroGrupo"),
        lblNombre = $("#lblNombre"),
        lblModelo = $("#lblModelo"),
        lblMarca = $("#lblMarca"),
        lblFechaCompra = $("#lblFechaCompra"),
        lblSaldo = $("#lblSaldo"),
        btnAplicarFiltros = $("#btnAplicarFiltros"),
        lblTotalGeneral = $("#titleTotalGeneral"),
        tituloLegend = $("#tituloLegend"),

        lblDepreciacion = $("#lblDepreciacion"),
        lblCostoXOverhaul = $("#lblCostoXOverhaul"),
        lblCostoXOverhaulAplicado = $("#lblCostoXOverhaulAplicado"),
         dateFin = $("#fechaFin"),
        dateIni = $("#fechaIni");


        function getInfo() {
            title.text("Reporte de Gastos Maquinaria ");

            Chart.defaults.global.legend.display = false;
            Chart.defaults.global.tooltips.enabled = false;

            cboFiltroTipo.fillCombo('/RepGastosMaquinaria/fillCboTipoMaquina');
            cboFiltroTipo.change(FillCboGrupo);

            cboFiltroGrupo.attr('disabled', true);
            cboFiltroGrupo.change(FillCboNoEconomico);

            btnReporte.click(imprimir);

            cboFiltroNoEconomico.attr('disabled', true);
            cboFiltroNoEconomico.change(FillGrafica);
            cboAnios.fillCombo('/RepGastosMaquinaria/FillCbo_Anios');
            cboAnios.change(FillGrafica);
            cboAnios.val('2016');
            btnAplicarFiltros.click(filtraGrafica);
            datePicker();
            var now = new Date(),
            year = now.getYear() + 1900;
            dateIni.datepicker("setDate", "01/01/" + year);
            dateFin.datepicker().datepicker("setDate", new Date());

        }

        $(document).on('click', ".eventoTipo", function () {
            cargarInicio();
        });

        $(document).on('click', ".eventoGrupo", function () {
            $("#tableGrupos").removeClass('hidden');
            $("#tableInsumos").addClass('hidden');
        });

        function imprimir(e) {

            var tipoEquipo = "";
            var grupoEquipo = "";

            var fechaInicio = "";
            var fechaFin = "";
            var idReporte = ""
            modalReportes.modal('show');
            var path = "/Reportes/Vista.aspx?idReporte=2";
            $("#report").attr("src", path);
            e.preventDefault();
        }

        function cargarInicio() {
            $("#divTipo").removeClass('hidden');
            $("#tableGrupos").addClass('hidden');
            $("#tableInsumos").addClass('hidden');
        }

        function FillCboGrupo() {
            clearCbo();
            if (cboFiltroTipo.val() != "") {
                cboFiltroGrupo.fillCombo('/RepGastosMaquinaria/fillCboGrupoMaquina', { idTipo: cboFiltroTipo.val() });
                cboFiltroGrupo.attr('disabled', false);
            }
            else {
                cboFiltroGrupo.clearCombo();
                cboFiltroGrupo.attr('disabled', true);
            }
        }

        function clearCbo() {
            cboFiltroNoEconomico.clearCombo();
            cboFiltroNoEconomico.attr('disabled', true);
            cboFiltroGrupo.attr('disabled', false);
        }

        function FillCboNoEconomico() {
            if (cboFiltroGrupo.val() != "") {
                cboFiltroNoEconomico.fillCombo('/RepGastosMaquinaria/fillCboMaquinarias', { idGrupo: cboFiltroGrupo.val(), idTipo: cboFiltroTipo.val() });
                cboFiltroNoEconomico.attr('disabled', false);
            }
            else {
                cboFiltroNoEconomico.clearCombo();
                cboFiltroNoEconomico.attr('disabled', true);
            }
        }

        function FillGrafica() {

            if (cboFiltroNoEconomico.val() != null && cboFiltroNoEconomico.val() != "") {

            }
            else {
                title.text("");
            }
        }

        function filtraGrafica() {

            var cboTipo = cboFiltroTipo.val();
            var cboGrupo = cboFiltroGrupo.val();
            var cboEconomico = cboFiltroNoEconomico.val();
            var flag = true;

            if (cboTipo == null || cboGrupo == null || cboEconomico == null || cboEconomico == "") {
                flag = false;
            }
            if (flag) {
                $("#notaGrafica").removeClass('hide');
                $("#divGrafica").addClass('verticalLine');
                tituloLegend.text($("#cboFiltroNoEconomico option:selected").text() + " del periodo " + dateIni.val() + " al " + dateFin.val());
                getDatos(getFiltrosGrafica());
            }
            else {
                AlertaGeneral("Alerta", "Debe seleccionar un filtro", "bg-red");
            }



        }

        function getFiltrosGrafica() {
           //var dateTypeVar = $('#fechaIni').datepicker('getDate');
            var fechaIni = $('#fechaIni').val();//$.datepicker.formatDate('mm-dd-yy', dateTypeVar);
           // var dateTypeVar = $('#fechaFin').datepicker('getDate');
            var fechafin = $("#fechaFin").val();//$.datepicker.formatDate('mm-dd-yy', dateTypeVar);
            return {
                fechaInicio: fechaIni,
                fechaFin: fechafin,
                area: cboFiltroNoEconomico.val(),
                cuenta: $("#cboFiltroNoEconomico option:selected").attr("data-Prefijo"),
                maq: $("#cboFiltroNoEconomico option:selected").text(),
            }
        }

        function reset() {
            lblNombre.text('');
            lblModelo.text('');
            lblMarca.text('');
            lblFechaCompra.text('');
            lblSaldo.text('');
        }

        function getDatos(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            reset();
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/RepGastosMaquinaria/fillGraficaReporteMaquinaria",
                data: { obj: obj },
                success: function (response) {
                    var meses = [];
                    var importes = [];
                    var anio = [];
                    var datos = response.info;
                    if (datos != null) {
                        lblNombre.text(datos.descripcion);
                        lblModelo.text(datos.modelo);
                        lblMarca.text(datos.marca);
                        lblFechaCompra.text(datos.fechaAdquisicion);
                        lblSaldo.text(datos.saldoinicial);
                        lblDepreciacion.text(datos.depreciacion);
                    }

                    $.each(response.items, function () {
                        importes.push(this.importe);
                        meses.push(this.mes);
                        anio.push(this.anio);
                    });

                    if (getIfMeses()) {

                        BarChart(anio, importes);
                    }
                    else {
                        BarChart(meses, importes);
                    }

                    lblCostoXOverhaul.text(response.costoOverHaul);
                    lblCostoXOverhaulAplicado.text(response.costoOverHaulAplicado);
                    lblTotalGeneral.text("Total General: " + response.total);
                    lblCostoXOverhaulAplicado.text(response.costoOverHaulAplicado);

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function getIfMeses() {
            var anioInicio = $('#fechaIni').datepicker('getDate').getYear();
            var anioFin = $('#fechaFin').datepicker('getDate').getYear();
            return anioInicio != anioFin;
        }
        function getFiltrosObject(mes) {
            //var dateTypeVar = $('#fechaIni').datepicker('getDate');
            var fechaIni = $('#fechaIni').val();//$.datepicker.formatDate('mm-dd-yy', dateTypeVar);
            // var dateTypeVar = $('#fechaFin').datepicker('getDate');
            var fechafin = $('#fechaFin').val();//$.datepicker.formatDate('mm-dd-yy', dateTypeVar);
            return {
                fechaInicio: fechaIni,
                fechaFin: fechafin,
                area: cboFiltroNoEconomico.val(),
                cuenta: $("#cboFiltroNoEconomico option:selected").attr("data-Prefijo"),
                maq: $("#cboFiltroNoEconomico option:selected").text(),
                mes: mes,
                idTipo: idTipo,
                idGrupo: idGrupo,
                anio: 0
            }
        }

        var myChart;
        function BarChart(meses, importes) {

            var maximo = Math.max.apply(null, importes);
            maximo = (maximo * .2) + maximo;
            var barChartData = {
                labels: meses,
                datasets: [
                    {
                        backgroundColor:
                'rgba(255, 130, 35, 1)'
                        ,
                        hoverBackgroundColor: 'rgba(255, 130, 35,0.5)',
                        borderColor:
                            'rgba(255,131,15,1)'
                        ,
                        borderWidth: 1,
                        data: importes
                    }
                ]
            }


            if (myChart != null) {
                myChart.destroy();
            }

            var ctx = document.getElementById("myChart");
            myChart = new Chart(ctx, {
                type: 'bar',
                data: barChartData,
                options: {
                    onClick: clickHandler,
                    responsive: true,

                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                callback: function (value, index, values) {
                                    return format2(value, "$");
                                },
                                stepSize: Math.trunc(maximo / meses.length)
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
                                    data = '$' + dataset.data[index].toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
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
        function format2(n, currency) {
            return currency + " " + n.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
        }
        function loadTabla(obj, controller, grid, setHeader) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: controller,
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                success: function (response) {
                    grid.bootgrid("clear");
                    var JSONINFO = response.rows;
                    grid.bootgrid("append", JSONINFO);
                    grid.bootgrid('reload');
                    if (setHeader == true) {
                        modalTitle.text("Detalle " + response.encabezadoModal);
                    }
                    modalMes.modal('show');
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    // alert(response.message);
                }
            });
        }

        var canvas = document.querySelector("#myChart");

        var X, Y, W, H, r;
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

        function initGrid() {
            gridFiltros.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-primary modificar' data-economico='" + row.economico + "' data-descripcion='" + row.descripcion + "'" + "data-importe='" + row.importe + "'data-id='" + row.id + "' data-mes ='" + row.mes + "'>" +
                                        "<span class='glyphicon glyphicon-eye-open'></span> " +
                                   " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                gridFiltros.find(".modificar").on("click", function (e) {
                    idTipo = $(this).attr("data-id");
                    $("#divTipo").addClass('hidden');
                    $("#loadingData").removeClass('hidden');
                    createGrid("tablaGrupos");
                    loadTabla(getFiltrosObject($(this).attr("data-mes")), '/RepGastosMaquinaria/FillGridReporteGastosGrupoInsumosXmes', $("#tablaGrupos"), false);
                    $("#lblDescripcionTipo").text($(this).attr("data-descripcion")).attr('tipo', idTipo);
                    $("#lblImporteTipo").text($(this).attr("data-importe"));

                });
            });
        }

        function createGrid(tipoTabla) {

            $("#tableGrupos").empty();
            $("#tableGrupos").append("<div class='col-lg-12'>" +
                        "<div class='col-lg-4'>" +
                            "<label class='eventoTipo'>Tipo Insumo:</label>" +
                            "<label class='eventoTipo' id='lblDescripcionTipo'></label>" +
                             " </div>" +
                         "<div class='col-lg-4'>" +
                            "<label>Importe:</label>" +
                            "<label id='lblImporteTipo'></label>" +
                             " </div>" +
                       " </div>");
            $("#tableGrupos").append(
                       "<table id='" + tipoTabla + "' class='table table-condensed table-hover table-striped text-center'>" +
                       "<thead class='bg-table-header'>" +
                             "<tr>" +
                                "<th data-column-id='cambiarInsumo' data-formatter='cambiarInsumo' data-align='center'>Ver Insumo</th>" +

                                "<th data-column-id='descripcion' data-align='center' data-header-align='center'>Descripción Grupo</th>" +
                                "<th data-column-id='importe' data-align='center' data-header-align='center'>Importe</th>" +
                            "</tr>" +
                        "</thead>" +
                    "</table>");

            $("#" + tipoTabla).bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "cambiarInsumo": function (column, row) {
                        return "<button type='button' class='btn btn-primary eventoGrupo' data-economico='" + row.economico + "' data-descripcion='" + row.descripcion + "'" +
                            "data-importe='" + row.importe + "' data-id='" + row.id + "' data-mes ='" + row.mes + "'>" +
                                        "<span class='glyphicon glyphicon-eye-open'></span> " +
                                   " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                $("#" + tipoTabla).find(".eventoGrupo").on("click", function (e) {
                    $("#loadingData").removeClass('hidden');
                    $("#divTipo").addClass('hidden');
                    $("#tableGrupos").addClass('hidden');
                    idGrupo = $(this).attr("data-id");
                    idTipo = $("#lblDescripcionTipo").attr("tipo");
                    descripcion = $("#lblDescripcionTipo").text();
                    importe = $("#lblImporteTipo").text();
                    gridAxuliar("insumo");
                    loadTabla(getFiltrosObject($(this).attr("data-mes")), '/RepGastosMaquinaria/FillGridReporteGastosInsumosXmes', $("#insumo"), false);
                    $("#tableGrupos").addClass('hidden');

                    $("#lblDescripcionTipo-Insumos").text(descripcion).attr('tipo', idTipo);
                    $("#lblImporteTipo-Insumos").text(importe);
                    $("#lblDescripcionGrupo-Insumos").text($(this).attr("data-descripcion")).attr('grupo', idGrupo);
                    $("#lblImporteGrupo-Insumos").text($(this).attr("data-importe"));
                });
            });
            $("#" + tipoTabla).on("appended.rs.jquery.bootgrid", function () {
                $("#loadingData").addClass('hidden');
                $("#tableGrupos").removeClass('hidden');

            });
        }

        function gridAxuliar(tipoTabla) {

            $("#tableInsumos").empty();
            $("#tableInsumos").append("<div class='col-lg-12 text-'>" +
                         "<div class='col-lg-3'>" +
                            "<label class='eventoTipo'>Tipo Insumo:</label>" +
                            "<label class='eventoTipo' id='lblDescripcionTipo-Insumos'></label>" +
                         " </div>" +
                         "<div class='col-lg-3'>" +
                            "<label>Importe:</label>" +
                            "<label id='lblImporteTipo-Insumos'></label>" +
                         " </div>" +
                         "<div class='col-lg-3'>" +
                            "<label class='eventoGrupo'>Grupo Insumo:</label>" +
                            "<label class='eventoGrupo' id='lblDescripcionGrupo-Insumos'></label>" +
                         " </div>" +
                         "<div class='col-lg-3'>" +
                            "<label>Importe:</label>" +
                            "<label id='lblImporteGrupo-Insumos'></label>" +
                         " </div>" +

                       " </div>");

            $("#tableInsumos").append(
                       "<table id='" + tipoTabla + "' class='table table-condensed table-hover table-striped text-center'>" +
                       "<thead class='bg-table-header'>" +
                             "<tr>" +
                                "<th data-column-id='fecha' data-align='center' data-header-align='center'>Fecha</th>" +
                                "<th data-column-id='descripcion' data-align='center' data-header-align='center'>Descripción Insumo</th>" +
                                "<th data-column-id='importe' data-align='center' data-header-align='center'>Importe</th>" +
                            "</tr>" +
                        "</thead>" +
                    "</table>");

            $("#" + tipoTabla).bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                }
            });

            $("#" + tipoTabla).on("appended.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                $("#loadingData").addClass('hidden');
                $("#tableInsumos").removeClass('hidden');
            });


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

        initGrid();
        getInfo();




    };

    $(document).ready(function () {

        maquinaria.reporte.repGastosMaquina = new repGastosMaquina();
    });
})();

