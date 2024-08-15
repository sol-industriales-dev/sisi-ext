(function () {
    $.namespace('maquinaria.Reportes.RepProvisionMaquina');

    RepProvisionMaquina = function () {
        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        cboCC = $("#cboCC");
        dpfechaCorte = $("#fechaCorte");
        btnReporteProvicional = $("#btnReporteProvicional");
        nTC = $("#nTC");
        ireport = $("#report");
        btnBuscarProvicional = $("#btnBuscarProvicional");
        divProvisionTabla = $("#divProvisionTabla");
        divExtraTabla = $("#divExtraTabla");
        cbTodoReporte = $("#cbTodoReporte");

        txtTotalDlls = $("#txtTotalDlls");
        txtTotalTC = $("#txtTotalTC");
        txtTotalPesos = $("#txtTotalPesos");
        txtTotalMN = $("#txtTotalMN");

        txtExtraDlls = $("#txtExtraDlls");
        txtExtraTC = $("#txtExtraTC");
        txtExtraPesos = $("#txtExtraPesos");
        txtExtraMN = $("#txtExtraMN");

        txtResumenDlls = $("#txtResumenDlls");
        txtResumenTC = $("#txtResumenTC");
        txtResumenPesos = $("#txtResumenPesos");
        txtResumenMN = $("#txtResumenMN");

        function init() {
            getDataPiker();
            cbTodoReporte.bootstrapSwitch();
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', null, false, null);
            btnReporteProvicional.click(VerReporte);
            btnBuscarProvicional.click(fnbtnValidaTala);
        }
        function fnbtnValidaTala() {
            if (validacion()) {
                fnLoadTable()
            }
        }
        function getDataPiker() {
            dpfechaCorte.datepicker().datepicker("setDate", new Date());
        }
        function VerReporte() {
            if (validacion()) {
                var btnValue = 33;
                $.blockUI({ message: mensajes.PROCESANDO });
                var idReporte = btnValue;
                var CC = cboCC.val();
                var fechaCorte = dpfechaCorte.val();
                var TC = nTC.val();
                var todoReporte = cbTodoReporte.bootstrapSwitch('state');
                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&CC=" + CC + "&fechaCorte=" + fechaCorte + "&TC=" + TC + "&todoReporte=" + todoReporte;
                ireport.attr("src", path);
                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
            }
        }
        function fnLoadTable() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/RepProvisionMaquina/fnLoadTable",
                data: { cc: cboCC.val(), FechaCorte: dpfechaCorte.val(), TC: nTC.val(), TodoReporte: cbTodoReporte.bootstrapSwitch('state'), },
                success: function (response) {

                    divProvisionTabla.bootgrid({
                        headerCssClass: '.bg-table-header',
                        align: 'center',
                        templates: {
                            header: ""

                        }
                    });
                    divExtraTabla.bootgrid({
                        headerCssClass: '.bg-table-header',
                        align: 'center',
                        templates: {
                            header: ""

                        }
                    });
                    divProvisionTabla.bootgrid("clear");
                    divExtraTabla.bootgrid("clear");
                    divProvisionTabla.bootgrid("append", response.data.lstEquipos);
                    divExtraTabla.bootgrid("append", response.data.lstEquipos);

                    txtTotalDlls.val('$ ' + formatNumber.new(response.data.lstTotal.TotalDlls));
                    txtTotalTC.val('$ ' + formatNumber.new(response.data.lstTotal.TC));
                    txtTotalPesos.val('$ ' + formatNumber.new(response.data.lstTotal.TotalPesos));
                    txtTotalMN.val('$ ' + formatNumber.new(response.data.lstTotal.TotalMN));

                    txtExtraDlls.val('$ ' + formatNumber.new(response.data.lstTotalExtra.TotalDlls));
                    txtExtraTC.val('$ ' + formatNumber.new(response.data.lstTotalExtra.TC));
                    txtExtraPesos.val('$ ' + formatNumber.new(response.data.lstTotalExtra.TotalPesos));
                    txtExtraMN.val('$ ' + formatNumber.new(response.data.lstTotalExtra.TotalMN));

                    txtResumenDlls.val('$ ' + formatNumber.new(response.data.lstResumen.TotalDlls));
                    txtResumenTC.val('$ ' + formatNumber.new(response.data.lstResumen.TC));
                    txtResumenPesos.val('$ ' + formatNumber.new(response.data.lstResumen.TotalPesos));
                    txtResumenMN.val('$ ' + formatNumber.new(response.data.lstResumen.TotalMN));

                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });
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
        }

        function validacion() {
            var valida = true;
            if (nTC.val() == '' || nTC.val() == null) {
                valida = false;
            }
            if (cboCC.val() == '' || cboCC.val() == null) {
                valida = false;
            }
            return valida;
        }
        init();
    };
    $(document).ready(function () {
        maquinaria.Reportes.RepProvisionMaquina = new RepProvisionMaquina();
    });
})();