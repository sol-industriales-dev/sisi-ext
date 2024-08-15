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

        ireport = $("#report");

        mensajes = {
            NOMBRE: 'Reporte Captura Horometro',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        function init() {

            //cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            //convertToMultiselect("#cboCC");

            btnBuscar.click(BuscarMaquinas);

            btnImprimir.click(verReporte);

        }

        function BuscarMaquinas() {
            btnImprimir.show();
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/RepConsumoDiesel/getConsumoMaquinas",
                data: { ccs: cboCC.val()/*getValoresMultiples("#cboCC")*/ },
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

        init();

    };
    
    $(document).ready(function () {

        maquinaria.reporte.repConsumoDiesel = new repConsumoDiesel();
    });
})();