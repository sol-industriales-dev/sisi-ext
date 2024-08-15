(function () {

    $.namespace('maquinaria.captura.diaria.ReporteCapturaHorometro');

    ReporteCapturaHorometro = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };

        var flag = true;
        tlbHorometrosMayores = $("#tlbHorometrosMayores"),
        cboGruposEquipo = $("#cboGruposEquipo"),
        cboModeloEquipos = $("#cboModeloEquipos"),
        hInicial = $("#hInicial"),
        HFinal = $("#HFinal"),

        btnReporte = $("#btnReporte"),
        divGridContainer = $("#divGridContainer"),
        btnAplicarFiltros = $("#btnAplicarFiltros"),
        ckAplicaOverhaul = $("#ckAplicaOverhaul"),
        btnAplicaOverhaul = $("#btnAplicaOverhaul"),
        gridResultado = $("#gridResultado"),
        txtCCFiltro = $("#txtCCFiltro"),
        txtNombreCC = $("#txtNombreCC"),
        cboEconomicos = $("#cboEconomicos"),
        cboTurno = $("#cboTurno"),
        tbTotalHoras = $("#tbTotalHoras"),
        cboCentroCostos = $("#cboCentroCostos"),
        fechaIni = $("#fechaIni"),
        fechaFin = $("#fechaFin");
        ireport = $("#report");
        tabTitle1 = $("#tabTitle1");
        tabTitle2 = $("#tabTitle2");
        hInicial = $("#hInicial");
        chkEstatus = $("#chkEstatus");

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

            btnReporte.click(verReporte);
            btnReporte.addClass('hidden');
            cboEconomicos.fillCombo('/Horometros/cboModalEconomico', { obj: txtCCFiltro.val() == "" ? 0 : txtCCFiltro.val() });
            cboEconomicos.change(getCentrosCotos);
            cboGruposEquipo.fillCombo('/CatGrupos/FillCboGrupoMaquina', { obj: 0 });
            cboGruposEquipo.change(cargarModelos);
            cboModeloEquipos.fillCombo('/CatModeloEquipo/FillCboModelo', { obj: (cboGruposEquipo.val() == '' ? 0 : cboGruposEquipo.val()) });
            txtCCFiltro.fillCombo('/CatInventario/FillComboCC', { est: true }, false);
            //txtCCFiltro.focusout(EventoCambio);

            //txtCCFiltro.change(cambiarVariable);
            //txtCCFiltro.keypress(function (e) {
            //    if (e.which == 13) {
            //        getInfoEconomico();
            //    }
            //});
            fechaIni.change(getCentrosCotos);
            fechaFin.change(getCentrosCotos);
            cboCentroCostos.change(filtrarGrid);
            txtCCFiltro.change(getInfoEconomico);
            tlbHorometrosMayores.bootgrid("append", new Array());
            getInfoEconomico();
        }
        function cargarModelos()
        {
            cboModeloEquipos.fillCombo('/CatModeloEquipo/FillCboModelo', { obj: (cboGruposEquipo.val() == '' ? 0 : cboGruposEquipo.val()) });
        }



        function getCentrosCotos() {
            if (cboEconomicos.val() != '' && txtCCFiltro.val() == '') {
                tbTotalHoras.val('');
                cboCentroCostos.fillCombo('/Horometros/GetCentroCosto', { obj: cboEconomicos.val(), fechaInicia: fechaIni.val(), fechaFinal: fechaFin.val() });
            }
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
            tbTotalHoras.val('');
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
                    cboCentroCostos.clearCombo();
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

            tlbHorometrosMayores.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                }
            });
        }

        function filtrarGrid() {
            if (validarHorometroFinal()) {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Horometros/GetInfoReporteCapturaHorometros",
                    data: { cc: txtCCFiltro.val() == "" ? "" : txtCCFiltro.val(), turno: Number(cboTurno.val()), fechaInicia: fechaIni.val(), fechaFinal: fechaFin.val(), economico: cboEconomicos.val(), ccFiltro: cboCentroCostos.val(), grupo: Number(cboGruposEquipo.val()), modelo: Number(cboModeloEquipos.val()), hInicial: Number(hInicial.val()), hFinal: Number(HFinal.val()), estatus: chkEstatus.is(':checked') },
                    success: function (response) {
                        $.unblockUI();
                        tbTotalHoras.val('');
                        var data = response.infoHorometros;
                        gridResultado.bootgrid("clear");
                        gridResultado.bootgrid("append", data);
                        gridResultado.bootgrid('reload');
                        btnReporte.removeClass('hidden');


                        datatbl = response.tblHorometroMayor;
                        tlbHorometrosMayores.bootgrid("clear");
                        tlbHorometrosMayores.bootgrid("append", datatbl);

                        var nf = new Intl.NumberFormat();

                        tbTotalHoras.val(nf.format(response.totalHoras));
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            else {
                AlertaGeneral("Alerta", "El horometro inicial debe ser mayor al horometro inicial", "bg-red");
            }

        }

        function validarHorometroFinal() {
            var dato = false;

            if (hInicial.val() != "") {
                if (Number(hInicial.val()) <= Number(HFinal.val())) {
                    dato = true;
                }
            }
            else {
                dato = true;
            }

            return dato;

        }


        function verReporte() {
            var CC = txtCCFiltro.val();
            var pFechaInico = fechaIni.val();
            var pFechaFin = fechaFin.val();

            var flag = true;
            if (CC == null || pFechaInico == "" || pFechaFin == "") {
                flag = false;
            }
            if (flag) {

                $.blockUI({ message: mensajes.PROCESANDO });

                var idReporte = "8";

                if (CC == "") {
                    CC = cboCentroCostos.val();
                }


                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pCC=" + CC + "&pTurno=" + cboTurno.val() + "&pFechaInicio=" + fechaIni.val() + "&pFechaFin=" + fechaFin.val() + "&pEconomico=" + cboEconomicos.val();

                ireport.attr("src", path);

                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
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
        maquinaria.captura.diaria.ReporteCapturaHorometro = new ReporteCapturaHorometro();
    });
})();


