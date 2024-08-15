(function () {
    $.namespace('facturacion.gestion');
    gestion = function () {
        mensajes = { PROCESANDO: 'Procesando...' };
        dtBusqInicio = $("#dtBusqInicio");
        dtBusqFin = $("#dtBusqFin");
        txtBusqCliente = $("#txtBusqCliente");
        btnBuscar = $("#btnBuscar");
        tblGestion = $("#tblGestion");
        btnIrFactura = $("#btnIrFactura");

        function init() {
            initDatepicker();
            btnBuscar.click(GetTblGestion);
            btnIrFactura.click(abreFactura);
            initTblGestion();
        }

        function initDatepicker() {
            dtBusqInicio.datepicker().datepicker("setDate", new Date());
            dtBusqFin.datepicker().datepicker("setDate", new Date());
        }

        function initTblGestion() {
            tblGestion.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    search: ""
                },
                formatters: {
                    "Accion": function (column, row) {
                        return "<button type='button' class='btn btn-info verFactura' value = '" + row.pedido + "'>" +
                              "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>" +
                            "<button type='button' class='btn btn-primary editarFactura' value = '" + row.pedido + "'>" +
                              "<span class='glyphicon glyphicon-pencil'></span> " +
                                " </button>" +
                            "<button type='button' class='btn btn-danger' disabled value = '" + row.pedido + "'>" +
                              "<span class='glyphicon glyphicon-remove' ></span> " +
                                " </button>";
                    },
                    "Estatus": function (column, row) {
                        var remision = "";
                        if (row.remision != 0) {
                            remision = " <div class=\"progress-bar progress-bar-warning\" role=\"progressbar\" style=\"width:33.33%\"> " +
                                                "   Remisión  " +
                                                " </div> ";
                        }
                        var factura = "";
                        if (row.factura != 0) {
                            factura = "<div class=\"progress-bar progress-bar-danger\" role=\"progressbar\" style=\"width:33.33%\">  " +
                                "   Factura " +
                            " </div>";
                        }
                        return "<div class=\"progress\"> " +
                            " <div class=\"progress-bar progress-bar-success\" role=\"progressbar\" style=\"width:33.33%\"> " +
                        "   Pedido " +
                        " </div> " + remision + factura + " </div>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                tblGestion.find(".verFactura").on("click", function (e) {
                    localStorage.setItem("pedido", $(this).val());
                    localStorage.setItem("editar", false);
                    abreFactura();
                });
                tblGestion.find(".editarFactura").on("click", function (e) {
                    localStorage.setItem("pedido", $(this).val());
                    localStorage.setItem("editar", true);
                    abreFactura();
                });
            });
        }

        function abreFactura() {
            var path = '/Facturacion/Facturacion/index';
            window.open(path, '_self');
        }

        function GetTblGestion() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Facturacion/Facturacion/GetTblGestion",
                type: "POST",
                data: { inicio: dtBusqInicio.val(), fin: dtBusqFin.val(), cliente: txtBusqCliente.val() },
                datatype: "json",
                success: function (response) {
                    $("#myModal").modal('toggle');
                    tblGestion.bootgrid("clear");
                    tblGestion.bootgrid("append", response);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        init();
    };
    $(document).ready(function () {
        facturacion.gestion = new gestion();
    });
})();