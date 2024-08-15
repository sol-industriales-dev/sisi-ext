/// <reference path="CapturaCombustibleMensualJS.js" />
(function () {

    $.namespace('maquinaria.captura.combustibles.ReporteCapturaCombustibleMensual');

    ReporteCapturaCombustibleMensual = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'

        };
        ruta = "/Combustibles/fillGridCapturaMensual",
        btnReporte = $("#btnReporte"),
        divGridContainer = $("#divGridContainer"),
        btnAplicarFiltros = $("#btnAplicarFiltros"),
        gridResultado = $("#grid_CapturaCombustible"),
        txtCCFiltro = $("#txtCCFiltro"),
        fechaIni = $("#fechaIni"),
        fechaFin = $("#fechaFin");
        ireport = $("#report");
        txtNombreCC = $("#txtNombreCC");
        mensajes = {
            NOMBRE: 'ReporteCapturaCombustibleMensual',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        function init() {
            datePicker();
            var now = new Date(),
            year = now.getYear() + 1900;
            fechaIni.datepicker().datepicker("setDate", new Date(year, now.getMonth(), 1));
            fechaFin.datepicker().datepicker("setDate", new Date(year, now.getMonth() + 1, 0)).prop('disabled', true);
            btnAplicarFiltros.click(filtrarGrid);
            btnReporte.click(verReporte);
            btnReporte.addClass('hidden');
            initGrid();
            txtCCFiltro.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false);
        }

        function getInfoEconomico() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Horometros/getCentroCostos",
                data: { obj: txtCCFiltro.val() == "" ? 0 : txtCCFiltro.val() },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        var nombreCC = response.centroCostos;

                        if (nombreCC == "") {
                            ConfirmacionGeneral("", "No se Encontro ese centro de costos", "bg-red");
                        }
                        txtNombreCC.val(nombreCC);
                    }
                    else {

                        txtNombreCC.val('');
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function filtrarGrid() {
            loadGridT(getFiltrosObject(), ruta, gridResultado);

            divGridContainer.addClass('scroll');
        }

        function getFiltrosObject() {
            return {
                cc: txtCCFiltro.val(),
                fechainicio: fechaIni.datepicker('getDate'),
                fechaFin: fechaFin.datepicker('getDate')
            }
        }

        function loadGridT(objetoCarga, controller, grid) {
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
                        var myObj = JSON.parse(JSONINFO);
                        grid.bootgrid("append", myObj);
                        grid.bootgrid('reload');
                        txtNombreCC.val(response.nombreCC);
                        btnReporte.removeClass('hidden');

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

        function verReporte(e) {
            var CC = txtCCFiltro.val();
            //var turno = txtTurno.val();
            // var fecha = fechaIni.val();

            //   var dateTypeVar = $("#fechaIni").datepicker('getDate');
            var pFechaInicial = fechaIni.val();

            //var dateTypeVar = $("#fechaFin").datepicker('getDate');
            var pFechaFinal = fechaFin.val();

            var flag = true;
            if (CC == null || pFechaInicial == "" || pFechaFinal == "") {
                flag = false;
            }
            if (flag) {

                $.blockUI({ message: mensajes.PROCESANDO });

                var idReporte = "6";

                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&CC=" + CC + "&pFechaInicio=" + pFechaInicial + "&pFechaFin=" + pFechaFinal;

                ireport.attr("src", path);

                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
                $.unblockUI();
                e.preventDefault();
            }
            else {
                AlertaGeneral("Alerta", "Debe seleccionar un filtro", "bg-red");
            }

        }


        function initGrid() {
            gridResultado.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""

                }
            });
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
                    onSelect: function () {
                        $(this).trigger('change');
                    }
                })
                .on("change", function () {

                    var date = $(this).val();
                    var array = new Array();
                    array = date.split('/');
                    $(this).datepicker('setDate', new Date(array[2], array[1] - 1, 1));
                    fechaFin.datepicker('setDate', new Date(array[2], array[1], 0));
                    $(this).blur();

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
    };

    $(document).ready(function () {
        maquinaria.captura.combustibles.ReporteCapturaCombustibleMensual = new ReporteCapturaCombustibleMensual();
    });
})();


