(function () {

    $.namespace('maquinaria.captura.combustibles.ReporteCapturasCombustible');

    ReporteCapturasCombustible = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'

        };

        btnReporte = $("#btnReporte"),
        ruta = "/Combustibles/fillGridRendimiento",
        divGridContainer = $("#divGridContainer"),
        btnAplicarFiltros = $("#btnAplicarFiltros"),
        gridResultado = $("#grid_RendimientoCombustible"),
        txtCCFiltro = $("#txtCCFiltro"),
        fechaIni = $("#fechaIni"),
        fechaFin = $("#fechaFin");
        txtNombreCC = $("#txtNombreCC");

        modalReportes = $("#modalReportes");
        ireport = $("#report");

        mensajes = {
            NOMBRE: 'ReporteCapturasCombustible',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        function init() {
            datePicker();
            var now = new Date(),
            year = now.getYear() + 1900;
            fechaIni.datepicker().datepicker("setDate", new Date(year, now.getMonth(), 1));
            fechaFin.datepicker().datepicker("setDate", new Date(year, now.getMonth() + 1, 0));
            btnAplicarFiltros.click(filtrarGrid);
            initGrid();
            txtCCFiltro.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false);
            btnReporte.click(verReporte);
            btnReporte.addClass('hidden');
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
            loadGrid(getFiltrosObject(), ruta, gridResultado);
            btnReporte.removeClass('hidden');
            divGridContainer.addClass('scroll');
        }

        function getFiltrosObject() {
            return {
                cc: txtCCFiltro.val(),
                fechainicio: fechaIni.datepicker('getDate'),
                fechaFin: fechaFin.datepicker('getDate')
            }
        }

        function loadGrid(objetoCarga, controller, grid) {
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
                        txtNombreCC.val(response.nombreCC);
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


        function initGrid() {
            gridResultado.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""

                }
            });
        }

        function verReporte(e) {
            var CC = txtCCFiltro.val();
            var pFechaInico = fechaIni.val();
            var pFechaFin = fechaFin.val();

            var flag = true;
            if (CC == null || pFechaInico == "" || pFechaFin == "") {
                flag = false;
            }
            if (flag) {

                $.blockUI({ message: mensajes.PROCESANDO });

                var idReporte = "7";

                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&CC=" + CC + "&pFechaInicio=" + pFechaInico + "&pFechaFin=" + pFechaFin;

                ireport.attr("src", path);

                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
                e.preventDefault();
            }
            else {
                AlertaGeneral("Alerta", "Debe seleccionar un filtro", "bg-red");
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
                    onSelect: function () {
                        $(this).trigger('change');
                    }
                })
                .on("change", function () {

                   

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
        maquinaria.captura.combustibles.ReporteCapturasCombustible = new ReporteCapturasCombustible();
    });
})();


