(function () {

    $.namespace('maquinaria.reporte.repConsumoDiesel');

    repConsumoDiesel = function () {

        cboCC = $("#cboCC");
        btnBuscar = $("#btnBuscar");
        btnImprimir = $("#btnImprimir");
        divConsumoTabla = $("#divConsumoTabla");
        txtTotalConsumo = $("#txtTotalConsumo");
        txtTotalEnkontrol = $("#txtTotalEnkontrol");
        txtTotalContratistas = $("#txtTotalContratistas");
        txtTotalProvisionar = $("#txtTotalProvisionar");
        txtFechaInicio = $("#txtFechaInicio");
        txtFechaFin = $("#txtFechaFin");
        cboFiltroTipo = $("#cboFiltroTipo");
        cbInventario = $("#cbInventario");
        txtTotalInventario = $("#txtTotalInventario");
        txttxtBandera = $("#txtBandera");
        cbOrigen = $("#cbOrigen");
        ireport = $("#report");

        mensajes = {
            NOMBRE: 'Reporte Captura Horometro',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        function init() {
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', null, false, null);
            cboFiltroTipo.fillCombo('/CatMaquina/FillCboTipo_Maquina', { estatus: true }, false, "Todos");
            convertToMultiselect("#cboFiltroTipo");
            txtFechaInicio.datepicker().datepicker("setDate", new Date());
            txtFechaFin.datepicker().datepicker("setDate", new Date());
            cbOrigen.bootstrapSwitch();
            cbInventario.bootstrapSwitch();
            //$.datepicker.setDefaults($.datepicker.regional["es"]);
            btnBuscar.click(BuscarMaquinas);
            btnImprimir.click(verReporte);
            cboCC.change(cboCCCambio);
            //cbOrigen.change();
        }

        function BuscarMaquinas() {
            btnImprimir.show();
            $.blockUI({ message: mensajes.PROCESANDO });
            if ($('#cbOrigen').is(":checked")) { txtFechaInicio.datepicker("setDate", "1/1/2011"); }
            else { txtFechaInicio = $("#txtFechaInicio"); }
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/RepConsumoDiesel/getConsumoMaquinas",
                data: { ccs: cboCC.val(), strFechaFin: txtFechaFin.val(), strFechaInicio: txtFechaInicio.val(), lstTipoMaquinaria: getValoresMultiples("#cboFiltroTipo"), bInventario: cbInventario.is(":checked") },
                success: function (response) {

                    $.each(response.lstConsumos, function (index, value) {
                        value.importe = '$ ' + formatNumber.new(value.importe);
                        value.importeKontrol = '$ ' + formatNumber.new(value.importeKontrol);
                    });
                    divConsumoTabla.bootgrid({
                        headerCssClass: '.bg-table-header',
                        align: 'center',
                        templates: {
                            header: ""

                        }
                    });
                    divConsumoTabla.bootgrid("clear");
                    divConsumoTabla.bootgrid("append", response.lstConsumos);

                    txtTotalConsumo.val('$ ' + formatNumber.new(response.totalConsumido));
                    txtTotalEnkontrol.val('$ ' + formatNumber.new(response.totalEnKontrol));
                    txtTotalContratistas.val('$ ' + formatNumber.new(response.totalContratistas));
                    txtTotalProvisionar.val('$ ' + formatNumber.new(response.totalProvisionar));
                    txtTotalInventario.val('$ ' + formatNumber.new(response.totalInventario));

                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });

        }

        function verReporte(e) {

            $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = "23";

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;

            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();
                openCRModal();

            };
            e.preventDefault();

        }

        var formatNumber = {
            separador: ",", // separador para los miles
            sepDecimal: '.', // separador para los decimales
            formatear: function (num) {
                num += '';
                var splitStr = num.split('.');
                var splitLeft = splitStr[0];
                var splitRight = splitStr.length > 1 ? this.sepDecimal + splitStr[1] : '';
                var regx = /(\d+)(\d{3})/;
                while (regx.test(splitLeft)) {
                    splitLeft = splitLeft.replace(regx, '$1' + this.separador + '$2');
                }
                return this.simbol + splitLeft + splitRight;
            },
            new: function (num, simbol) {
                this.simbol = simbol || '';
                return this.formatear(num);
            }
        }

        

        function getDate(element) {
            var date;
            try {
                date = $.datepicker.parseDate(dateFormat, element.value);
            } catch (error) {
                date = null;
            }

            return date;
        }

        cbOrigen.on('switchChange.bootstrapSwitch', function () {
            if (this.checked) {
                $("#txtFechaInicio").attr("disabled", true);
                txtFechaInicio.datepicker("setDate", "1/1/2011");
            } else {
                $("#txtFechaInicio").attr("disabled", false);
                var fechaInicio = new Date("January 1, " + new Date().getUTCFullYear());
                txtFechaInicio.datepicker("setDate", fechaInicio);
            }
        });

        cbInventario.on('switchChange.bootstrapSwitch', function () {
            if (this.checked && (cboCC.val() == 148 || cboCC.val() == 146)) {
                $("#txtBandera").val($("#txtTotalConsumo").val());
                $("#txtTotalConsumo").val($("#txtTotalInventario").val());
                $("#txtTotalInventario").val($("#txtTotalConsumo").val());
            }
            if (!this.checked && (cboCC.val() == 148 || cboCC.val() == 146)) {
                $("#txtTotalInventario").val($("#txtTotalConsumo").val());
                $("#txtTotalConsumo").val($("#txtBandera").val());
                $("#txtBandera").val($("#txtTotalConsumo").val());
            }
        });

        $('#cboFiltroTipo').change(function () {
            if ((cboCC.val() == 148 || cboCC.val() == 146) && ($('#cboFiltroTipo option:selected').val() == "2" || $('#cboFiltroTipo option:selected').val() == "3")) {
                $("#divInventario").removeAttr("hidden");
                $("#divToalProvisionar").removeAttr("hidden");
            }
            if ((cboCC.val() == 148 || cboCC.val() == 146) && !($('#cboFiltroTipo option:selected').val() == "2" || $('#cboFiltroTipo option:selected').val() == "3")) {
                $("#divToalProvisionar").attr('hidden', 'hidden');
                $("#divInventario").attr('hidden', 'hidden');
            }
        });

        function cboCCCambio () {
            if (cboCC.val() == 148 && $('#cboFiltroTipo option:selected').val() == "1") {
                $("#divInventario").removeAttr("hidden");
                $("#divToalProvisionar").attr('hidden', 'hidden');
            }
            $("#divToalProvisionar").removeAttr("hidden");
            $("#divInventario").attr('hidden', 'hidden');
            if (cboCC.val() == 146 || cboCC.val() == 148) {
                cbOrigen.prop("checked", false);
                $(txtFechaInicio).attr("disabled", false);
                txtFechaInicio.datepicker("setDate", "1/1/2017");
                cboFiltroTipo.multiselect("deselect", "1");
                cboFiltroTipo.multiselect("select", "2");
                cboFiltroTipo.multiselect("select", "3");
                $(cboFiltroTipo).multiselect("refresh");
                cboFiltroTipo.change();
            }
            else {
                cbOrigen.prop("checked", true);
                $(txtFechaInicio).attr("disabled", true);
                txtFechaInicio.datepicker("setDate", "1/1/2010");
                $("#cboFiltroTipo option").prop("selected", true);
                $("#cboFiltroTipo option[value='Todos']").prop("selected", false);
                $(cboFiltroTipo).multiselect("refresh");
            }
        }

        init();

    };

    $(document).ready(function () {

        maquinaria.reporte.repConsumoDiesel = new repConsumoDiesel();
    });
})();