(function () {
    $.namespace('Administrativo.Contabilidad.Reportes.NumeroNafin');
    NumeroNafin = function () {
        mensajes = { PROCESANDO: 'Procesando...' };
        tblNafin = $("#tblNafin");
        btnGuardar = $("#btnGuardar");
        function init() {
            initTabla();
            GetTblNafin();
            btnGuardar.click(Guardar);
        }

        function initTabla() {
            tblNafin.bootgrid({
                rowCount: -1,
                align: 'center',
                selection: true,
                labels:
                    {
                        infos: '{{ctx.total}} Proveedores'
                    },
                templates: {
                    footer: ""
                },
                formatters: {
                    "tipoMoneda": function (column, row) {
                        return row.tipoMoneda == 1 ? "MX" : "USD";
                    },
                    "numNafin": function (column, row) {
                        var html = '<input hidden class="hddnAddid" value="' + row.id + '" />';
                        return html + '<input type="text" class="form-control" value="' + row.numNafin + '"/>';
                    }

                }
            }).on("loaded.rs.jquery.bootgrid", function () {
            });
        }
        function GetTblNafin() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Administrativo/Reportes/GetLstNafin",
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    tblNafin.bootgrid("clear");
                    tblNafin.bootgrid("append", response.rows);
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                    $.unblockUI();
                }
            });
        }
        function Guardar() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Administrativo/Reportes/GuardarLstNafin",
                type: 'POST',
                dataType: 'json',
                data: { lst: getLst() },
                success: function (response) {
                    if (response.success) {
                        AlertaGeneral("Alerta", "Proveedores guardadoscon éxito");
                        GetTblNafin();
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                    $.unblockUI();
                }
            });
        }
        function getLst() {
            var Array = [];
            tblNafin.find("tr").each(function (idx, row) {
                if (idx > 0 && $(":nth-child(2)", row).html() != undefined) {
                    var JsonData = {};
                    JsonData.id = $(this).find('td:eq(4) input[class="hddnAddid"]').val();
                    JsonData.RFC = $(this).find('td:eq(0)').text();
                    JsonData.RazonSocial = $(this).find('td:eq(1)').text();;
                    JsonData.estatus = true;
                    JsonData.NumProveedor = $(this).find('td:eq(3)').text();;
                    JsonData.NumNafin = $(this).find('td:eq(4) input.form-control').val();
                    JsonData.TipoMoneda = $(this).find('td:eq(5)').text() == "MX" ? 1 : 2;
                    if (JsonData.NumProveedor != JsonData.NumNafin)
                        Array.push(JsonData);
                }
            });
            return Array;
        }
        init();
    };
    $(document).ready(function () {
        Administrativo.Contabilidad.Reportes.NumeroNafin = new NumeroNafin();
    });
})();