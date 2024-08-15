(function () {

    $.namespace('maquinaria.captura.combustibles.ReporteCapturaDiesel');

    ReporteCapturaDiesel = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };

        btnReporte = $("#btnReporte"),
        divGridContainer = $("#divGridContainer"),
        btnAplicarFiltros = $("#btnAplicarFiltros"),

        gridResultado = $("#gridResultado"),
        txtCCFiltro = $("#txtCCFiltro"),
        txtNombreCC = $("#txtNombreCC"),
        cboEconomicos = $("#cboEconomicos"),
        cboTurno = $("#cboTurno"),
        tbTotalLitros = $("#tbTotalLitros"),
        fechaIni = $("#fechaIni"),
        fechaFin = $("#fechaFin");
        ireport = $("#report");

        mensajes = {
            NOMBRE: 'Reporte Captura Horometro',
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
            cboEconomicos.fillCombo('/Horometros/cboModalEconomico', { obj: txtCCFiltro.val() == "" ? 0 : txtCCFiltro.val() });
            //txtCCFiltro.focusout(EventoCambio);
            //txtCCFiltro.change(cambiarVariable);

            btnReporte.click(verReporte);
            btnReporte.addClass('hidden');

        }
        function EventoCambio() {
            if (flag) {
                flag = false;
                getInfoEconomico();
            }
        }
        function cambiarVariable() {
            flag = true;
        }

        function triggerCombo() {
            cboEconomicos.clearCombo();
            cboEconomicos.fillCombo('/Horometros/cboModalEconomico', { obj: txtCCFiltro.val() == "" ? 0 : txtCCFiltro.val() });
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
                        triggerCombo();
                        if (nombreCC == "") {
                            triggerCombo();
                            ConfirmacionGeneral("", "No se Encontro ese centro de costos", "bg-red");
                        }
                        txtNombreCC.val(nombreCC);

                    }
                    else {
                        triggerCombo()
                        txtNombreCC.val('');
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }

        function getFiltrosObject() {
            return {
                cc: txtCCFiltro.val(),
                fechainicio: fechaIni.datepicker('getDate'),
                fechaFin: fechaFin.datepicker('getDate')
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

        function filtrarGrid() {

            // getInfoEconomico();

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Combustibles/GetInfoReporteCapturaCombustibles",
                data: { cc: txtCCFiltro.val() == "" ? 0 : txtCCFiltro.val(), turno: cboTurno.val(), fechaInicia: fechaIni.val(), fechaFinal: fechaFin.val(), economico: cboEconomicos.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.infoCombustibles;
                    var Litros = response.Total;
                    tbTotalLitros.val(Litros);
                    gridResultado.bootgrid("clear");
                    gridResultado.bootgrid("append", data);
                    gridResultado.bootgrid('reload');
                    btnReporte.removeClass('hidden');
                },
                error: function () {
                    $.unblockUI();
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

                var idReporte = "9";

                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pCC=" + txtCCFiltro.val() + "&pTurno=" + cboTurno.val() + "&pFechaInicio=" + fechaIni.val() + "&pFechaFin=" + fechaFin.val() + "&pEconomico=" + cboEconomicos.val();

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
    };

    $(document).ready(function () {
        maquinaria.captura.combustibles.ReporteCapturaDiesel = new ReporteCapturaDiesel();
    });
})();


