(function () {
    $.namespace('administrativo.RepTraspaso');
    RepTraspaso = function () {
        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        cboCC = $("#cboCC");
        txtFolio = $("#txtFolio");
        txtAlmacen = $("#txtAlmacen");
        btnBuscarTraspaso = $("#btnBuscarTraspaso");
        btnReporteAbierto = $("#btnReporteAbierto");
        btnReporteCerrado = $("#btnReporteCerrado");
        divMovAbiertosTabla = $("#divMovAbiertosTabla");
        divMovCerradosTabla = $("#divMovCerradosTabla");
        ireport = $("#report"),
        dateFin = $("#fechaFin"),
        dateIni = $("#fechaIni");

        function init() {
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', null, false, null);
            btnBuscarTraspaso.click(LoadTableTraspaso);
            //btnBuscarTraspaso.click();
            btnReporteAbierto.click(verReporte);
            btnReporteCerrado.click(verReporte);
            dateIni.datepicker();
            dateFin.datepicker();
        }

        function LoadTableTraspaso() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/RepTraspaso/fnLoadTable",
                data: { cc: cboCC.val(), folio: txtFolio.val(), almacen: txtAlmacen.val(), fechaIni: dateIni.val(), fechaFin: dateFin.val() },
                success: function (response) {

                    divMovAbiertosTabla.bootgrid({
                        headerCssClass: '.bg-table-header',
                        align: 'center'
                    });
                    divMovCerradosTabla.bootgrid({
                        headerCssClass: '.bg-table-header',
                        align: 'center'
                    });
                    divMovAbiertosTabla.bootgrid("clear");
                    divMovCerradosTabla.bootgrid("clear");
                    if (response.abiertoSUCCESS) {
                        divMovAbiertosTabla.bootgrid("append", response.abierto);
                    }
                    if (response.cerradoSUCCESS) {
                        divMovCerradosTabla.bootgrid("append", response.cerrado);
                    }
                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function verReporte() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var idReporte = $(this).val();;
            var CC = cboCC.val();
            var Folio = txtFolio.val();
            var Almacen = txtAlmacen.val();
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;
                if (CC != "") {
                    path += "&CC=" + CC;
                    }
                if (Folio != "") {
                    path += "&folio=" + Folio;
                    }
                if (Almacen != "") {
                    path += "&almacen=" + Almacen;
                }
                if (dateIni.val() != "") {
                    path += "&fechaIni=" + dateIni.val();
                }
                if (dateFin.val() != "") {
                    path += "&fechaFin=" + dateFin.val();
                }
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        init();
    };
    $(document).ready(function () {
        administrativo.RepTraspaso = new RepTraspaso();
    });
})();