
(function () {

    $.namespace('maquinaria.reporte.repComparaivaTipos');

    repComparaivaTipos = function () {

        idTipo = 0;
        idGrupo = 0;
        ruta = '/RepComparativaTipos/fillGridReporteComparativaTipos';
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Reporte Comparativa de Tipos',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        tbCentroCostos = $("#tbCentroCostos"),
        cboFiltroTipo = $("#cboFiltroTipo"),
        cboFiltroGrupo = $("#cboFiltroGrupo"),
        modalReportes = $("#modalReportes"),
        modalMes = $("#modalMes"),
        gridFiltros = $("#grid_RepMensual"),
        cboAnios = $("#cboAnios"),
        ireport = $("#report"),
        btnReporte = $("#btnReporte"),
        title = $("#title"),
        lblDescripcionTipo = $("#lblDescripcionTipo"),
        modalTitle = $("#title-modal"),
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
        dateFin = $("#fechaFin"),
        openModal = $(".eveOpenModal"),
        lblDepreciacion = $("#lblDepreciacion"),
        lblCostoXOverhaul = $("#lblCostoXOverhaul"),
        dateIni = $("#fechaIni"),
        divMain_Table = $("#divMain_Table"),
        grid_Main = $("#grid_Main"),
        fechaIni = $("#fechaIni"),
        btnRegresar = $("#btnRegresar"),
        fechaFin = $("#fechaFin");
        modalMensaje = $("#modalMensaje");
        lblMensaje = $("#lblMensaje");

        function init() {

            //tituloLegend.text($("#cboFiltroGrupo option:selected").text() + " del periodo " + dateIni.val() + " al " + dateFin.val());
            tbCentroCostos.fillCombo('/RepAnalisisUtilizacion/cboAC', { est: true }, false);
            cboFiltroTipo.fillCombo('/RepGastosMaquinaria/fillCboTipoMaquina');
            cboFiltroTipo.change(FillCboGrupo);
            btnReporte.click(verReporte);
            btnRegresar.click(fnRegresar);
            cboFiltroGrupo.attr('disabled', true);
            cboAnios.fillCombo('/RepGastosMaquinaria/FillCbo_Anios');
            cboAnios.val('2016');
            btnAplicarFiltros.click(Validar);
            datePicker();
            var now = new Date(),
            year = now.getYear() + 1900;
            fechaIni.datepicker("setDate", "01/01/" + year);
            fechaFin.datepicker().datepicker("setDate", new Date());
        }
        function fnRegresar() {
            fnGetMainTableSesion();
            btnRegresar.hide();
            cboFiltroGrupo.val("");
        }
        function habilitarCC() {
            if (cboFiltroGrupo.val() != "") {
                tbCentroCostos.prop('disabled', false);
            }
            else {
                tbCentroCostos.prop('disabled', true);
            }
        }

        function verReporte(e) {
            var cboTipo = cboFiltroTipo.val();
            var cboGrupo = cboFiltroGrupo.val();
            var flag = true;
            if (cboTipo == null) {
                flag = false;
            }
            if (flag) {
                $.blockUI({ message: mensajes.PROCESANDO });
                var tipoEquipo = cboFiltroTipo.val();
                var grupoEquipo = cboFiltroGrupo.val();
                var idReporte = "2";

                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&area=" + grupoEquipo + "&fechaInicio=" + fechaIni.val() + "&fechaFin=" + fechaFin.val();

                ireport.attr("src", path);

                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };

                e.preventDefault();
            }
            else {
                lblMensaje.text("Debe seleccionar un filtro");
                modalMensaje.modal("show");
            }

        }



        function Validar() {
            btnRegresar.hide();
            if (myChart != null) {
                myChart.destroy();
            }
            var cboTipo = cboFiltroTipo.val();
            var cboGrupo = cboFiltroGrupo.val();
            var flag = true;
            //|| cboGrupo == null || cboGrupo == ""
            if (cboTipo == "" || cboGrupo == null) {
                flag = false;
            }

            if (flag) {
                fnGetMainTable();
            }
            else {
                lblMensaje.text("Debe seleccionar un filtro");
                modalMensaje.modal("show");
            }


        }

        function fnGetMainTable() {
            $.blockUI({ message: mensajes.PROCESANDO });
            //reset();

            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/RepComparativaTipos/getTableStruct",
                data: { obj: getPlainFilterObj() },
                success: function (response) {
                    divMain_Table.empty();
                    divMain_Table.html(response.table);
                    if (cboFiltroGrupo.val() == "" && cboFiltroTipo != "") {
                        var grupoDescripcion = [];
                        var costo = []

                        $.each(response.Grafica, function () {
                            grupoDescripcion.push(this.Descripcion);
                            costo.push(this.CostoHorario);
                        });
                        BarChart(grupoDescripcion, costo);
                    }

                    $("#grid_Main").datagrid({
                        rownumbers: true,
                        singleSelect: true,
                        autoRowHeight: false,
                        pagination: true,
                        pageSize: 10,
                        rowStyler: function (index, row) {
                            if (row["NO. ECO"] == "      Total General:       ") {
                                return 'background-color:orange;';
                            }
                        }
                    }).datagrid('clientPaging');
                    var tds = $(".datagrid-htable td");
                    $.each(tds, function (i, e) {
                        e.style.color = '#f5f5f5';
                        e.style.fontSize = 'smaller';
                        e.style.fontWeight = 'bold';
                        e.style.textAlign = "center";
                        e.style.backgroundColor = '#81bd72';
                    });


                    var TitleGrupo = $("#cboFiltroGrupo option:selected").val() != "" ? $("#cboFiltroGrupo option:selected").text() : "Maquinaria por Tipo";
                    var TitleCC = tbCentroCostos.val();
                    TitleCC = TitleCC != "" ? " del Centro Costos " + TitleCC : "";

                    var Descripcion = TitleGrupo + " DEL PERIODO " + fechaIni.val() + " AL " + fechaFin.val() + TitleCC;
                    $("#tituloLegend").text(Descripcion)
                    $(".divTooltip").show();

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function fnGetMainTableSesion() {
            $.blockUI({ message: mensajes.PROCESANDO });
            //reset();

            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/RepComparativaTipos/getTableStructSesion",
                data: { },
                success: function (response) {
                    divMain_Table.empty();
                    divMain_Table.html(response.table);
                    if (cboFiltroGrupo.val() == "" && cboFiltroTipo != "") {
                        var grupoDescripcion = [];
                        var costo = []

                        $.each(response.Grafica, function () {
                            grupoDescripcion.push(this.Descripcion);
                            costo.push(this.CostoHorario);
                        });
                        BarChart(grupoDescripcion, costo);
                    }

                    $("#grid_Main").datagrid({
                        rownumbers: true,
                        singleSelect: true,
                        autoRowHeight: false,
                        pagination: true,
                        pageSize: 10,
                        rowStyler: function (index, row) {
                            if (row["NO. ECO"] == "      Total General:       ") {
                                return 'background-color:orange;';
                            }
                        }
                    }).datagrid('clientPaging');
                    var tds = $(".datagrid-htable td");
                    $.each(tds, function (i, e) {
                        e.style.color = '#f5f5f5';
                        e.style.fontSize = 'smaller';
                        e.style.fontWeight = 'bold';
                        e.style.textAlign = "center";
                        e.style.backgroundColor = '#81bd72';
                    });


                    var TitleGrupo = $("#cboFiltroGrupo option:selected").val() != "" ? $("#cboFiltroGrupo option:selected").text() : "Maquinaria por Tipo";
                    var TitleCC = tbCentroCostos.val();
                    TitleCC = TitleCC != "" ? " del Centro Costos " + TitleCC : "";

                    var Descripcion = TitleGrupo + " DEL PERIODO " + fechaIni.val() + " AL " + fechaFin.val() + TitleCC;
                    $("#tituloLegend").text(Descripcion)
                    $(".divTooltip").show();

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        $(document).on('dblclick', ".eveOpenModal", function () {
            var cuenta = $(this).attr("data-cuenta");
            var area = $(this).attr("data-area");
            var idTipo = $(this).attr("data-tipo");
            var idTipoS = $(this).attr("data-tipoS");
            var valor = $(this).attr("data-valor");
            var noEco = $(this).attr("data-noeco");
            $("#loadingData").removeClass('hidden');
            $("#title-modal").text("Detalle del " + fechaIni.val() + " al " + fechaFin.val());
            $("#tituloModalMaquina").text($("#cboFiltroGrupo option:selected").text() + " " + noEco);
            createGrid("tablaGrupos");
            loadTabla(getFiltrosObject(area, cuenta, idTipo), '/RepComparativaTipos/getTableGrupos/?noEco=' + noEco, $("#tablaGrupos"), false);
            $("#lblDescripcionTipo").text(idTipoS);
            $("#lblImporteTipo").text(valor);
        });
        $(document).on('dblclick', ".eveRefreshTable", function () {
            btnRegresar.show();
            var cuenta = $(this).attr("data-area");

            cboFiltroGrupo.val(cuenta);
            fnGetMainTable();
        });


        (function ($) {
            function pagerFilter(data) {
                if ($.isArray(data)) {    // is array
                    data = {
                        total: data.length,
                        rows: data
                    }
                }
                var target = this;
                var dg = $(target);
                var state = dg.data('datagrid');
                var opts = dg.datagrid('options');
                if (!state.allRows) {
                    state.allRows = (data.rows);
                }
                if (!opts.remoteSort && opts.sortName) {
                    var names = opts.sortName.split(',');
                    var orders = opts.sortOrder.split(',');
                    state.allRows.sort(function (r1, r2) {
                        var r = 0;
                        for (var i = 0; i < names.length; i++) {
                            var sn = names[i];
                            var so = orders[i];
                            var col = $(target).datagrid('getColumnOption', sn);
                            var sortFunc = col.sorter || function (a, b) {
                                return a == b ? 0 : (a > b ? 1 : -1);
                            };
                            r = sortFunc(r1[sn], r2[sn]) * (so == 'asc' ? 1 : -1);
                            if (r != 0) {
                                return r;
                            }
                        }
                        return r;
                    });
                }
                var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
                var end = start + parseInt(opts.pageSize);
                data.rows = state.allRows.slice(start, end);
                return data;
            }

            var loadDataMethod = $.fn.datagrid.methods.loadData;
            var deleteRowMethod = $.fn.datagrid.methods.deleteRow;
            $.extend($.fn.datagrid.methods, {
                clientPaging: function (jq) {
                    return jq.each(function () {
                        var dg = $(this);
                        var state = dg.data('datagrid');
                        var opts = state.options;
                        opts.loadFilter = pagerFilter;
                        var onBeforeLoad = opts.onBeforeLoad;
                        opts.onBeforeLoad = function (param) {
                            state.allRows = null;
                            return onBeforeLoad.call(this, param);
                        }
                        var pager = dg.datagrid('getPager');
                        pager.pagination({
                            onSelectPage: function (pageNum, pageSize) {
                                opts.pageNumber = pageNum;
                                opts.pageSize = pageSize;
                                pager.pagination('refresh', {
                                    pageNumber: pageNum,
                                    pageSize: pageSize
                                });
                                dg.datagrid('loadData', state.allRows);
                            }
                        });
                        $(this).datagrid('loadData', state.data);
                        if (opts.url) {
                            $(this).datagrid('reload');
                        }
                    });
                },
                loadData: function (jq, data) {
                    jq.each(function () {
                        $(this).data('datagrid').allRows = null;
                    });
                    return loadDataMethod.call($.fn.datagrid.methods, jq, data);
                },
                deleteRow: function (jq, index) {
                    return jq.each(function () {
                        var row = $(this).datagrid('getRows')[index];
                        deleteRowMethod.call($.fn.datagrid.methods, $(this), index);
                        var state = $(this).data('datagrid');
                        if (state.options.loadFilter == pagerFilter) {
                            for (var i = 0; i < state.allRows.length; i++) {
                                if (state.allRows[i] == row) {
                                    state.allRows.splice(i, 1);
                                    break;
                                }
                            }
                            $(this).datagrid('loadData', state.allRows);
                        }
                    });
                },
                getAllRows: function (jq) {
                    return jq.data('datagrid').allRows;
                }
            })
        })(jQuery);

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
                    $("#modalMes").modal('show');
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    // alert(response.message);
                }
            });
        }

        function getFiltrosObject(area, cuenta, tipo, grupo) {
            /*   var dateTypeVar = $('#fechaIni').datepicker('getDate');
               var fechaIni = $.datepicker.formatDate('mm-dd-yy', dateTypeVar);
               var dateTypeVar = $('#fechaFin').datepicker('getDate');
               var fechafin = $.datepicker.formatDate('mm-dd-yy', dateTypeVar);*/
            return {
                fechaInicio: fechaIni.val(),
                fechaFin: fechaFin.val(),
                area: area,
                cuenta: cuenta,
                idTipo: tipo,
                idGrupo: grupo
            }
        }

        function createGrid(tipoTabla) {

            $("#tableGrupos").empty();
            $("#tableInsumos").empty();
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
                            "data-importe='" + row.importe + "' data-id='" + row.id + "' data-mes ='" + row.mes + "' data-area='" + row.area + "' data-cuenta='" + row.cuenta + "' data-tipo='" + row.tipo + "'>" +
                                        "<span class='glyphicon glyphicon-eye-open'></span> " +
                                   " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                $("#" + tipoTabla).find(".eventoGrupo").on("click", function (e) {
                    var DescripcionTipo = $("#lblDescripcionTipo").text();
                    var lblImporteTipo = $("#lblImporteTipo").text();
                    $("#loadingData").removeClass('hidden');
                    $("#tableGrupos").addClass('hidden');
                    idGrupo = $(this).attr("data-id");
                    idTipo = $(this).attr("data-tipo");
                    area = $(this).attr("data-area");
                    cuenta = $(this).attr("data-cuenta");
                    var noEco = $(this).attr("data-economico");
                    var DescripcionGrupo = $(this).attr("data-descripcion");
                    gridAxuliar("insumo");
                    loadTabla(getFiltrosObject(area, cuenta, idTipo, idGrupo), '/RepComparativaTipos/getTableInsumos/?noEco=' + noEco, $("#insumo"), false);
                    //$("#lblDescripcionTipo-Insumos").text(DescripcionTipo);
                    //$("#lblImporteTipo-Insumos").text(lblImporteTipo);
                    $("#lblDescripcionTipo-Insumos").text(DescripcionTipo).attr('tipo', idTipo);
                    $("#lblImporteTipo-Insumos").text(lblImporteTipo);
                    $("#lblDescripcionGrupo-Insumos").text($(this).attr("data-descripcion")).attr('grupo', idGrupo);
                    $("#lblImporteGrupo-Insumos").text($(this).attr("data-importe"));
                    //  $("#lblDescripcionTipo-Insumos").click(function () {

                    $("#lblDescripcionGrupo-Insumos").click(function () {
                        $("#tableGrupos").removeClass('hidden');
                        $("#tableInsumos").addClass('hidden');
                    });
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

        function getPlainFilterObj() {
            /*   var dateTypeVar = $('#fechaIni').datepicker('getDate');
               var fechaIni = $.datepicker.formatDate('mm-dd-yy', dateTypeVar);
               var dateTypeVar = $('#fechaFin').datepicker('getDate');
               var fechafin = $.datepicker.formatDate('mm-dd-yy', dateTypeVar);*/


            return {
                fechaInicio: fechaIni.val(),
                fechaFin: fechaFin.val(),
                ac : tbCentroCostos.val(),
                idGrupo: cboFiltroGrupo.val(),
                idTipo: cboFiltroTipo.val()
            }
        }


        function FillCboGrupo() {
            //clearCbo();
            if (cboFiltroTipo.val() != "") {
                cboFiltroGrupo.fillCombo('/RepGastosMaquinaria/fillCboGrupoMaquina', { idTipo: cboFiltroTipo.val() });
                cboFiltroGrupo.attr('disabled', false);
            }
            else {
                cboFiltroGrupo.clearCombo();
                cboFiltroGrupo.attr('disabled', true);

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



        var myChart;
        function BarChart(Grupos, Gastos) {
            Chart.defaults.global.legend.display = false;
            Chart.defaults.global.tooltips.enabled = false;

            var maximo = Math.max.apply(null, Gastos);
            maximo = (maximo * 1) + maximo;
            var barChartData = {
                labels: Grupos,
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
                        data: Gastos
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
                    responsive: true,

                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                callback: function (value, index, values) {
                                    return format2(value, "$");
                                },

                                stepSize: Math.trunc(maximo / Grupos.length)
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
            function format2(n, currency) {
                return currency + " " + n.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
            }

            //inicializarCanvas();
            //addEventListener("resize", inicializarCanvas);
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
        init();
    };

    $(document).ready(function () {
        maquinaria.reporte.repComparaivaTipos = new repComparaivaTipos();
    });
})();

