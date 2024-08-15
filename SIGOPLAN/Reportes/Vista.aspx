<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vista.aspx.cs" Inherits="SIGOPLAN.Reportes.Vista" ValidateRequest="false" uiCulture="es" culture="es-MX" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="../Content/style/bootstrap.css" />
    <link href="../Content/style/css/CRViewer.css" rel="stylesheet" />
    <script src="../Scripts/jquery-3.1.0.js"></script>
    <script type="text/javascript" src="../Scripts/bootstrap.js"></script>
    <script>
        $(document).ready(function () {
            var pNumCheque = getUrlParameter('pNumCheque');

            if (window.top.isRemoteModal()) {
                $("#btnmdlPanelAuth").click(function () {
                    window.top.closeRemoteModal();
                });
            }
            else {
                $("#btnmdlPanelAuth").hide();
            }
            if (window.location.search.indexOf("idReporte=4&minuta") != -1 || window.location.search.indexOf("idReporte=5&minuta") != -1) {
                $(".clsExportar").hide();
            }

            if (pNumCheque != null) {
                $('#btnPrint').removeClass('hide');
            }

        });

        function Print() {
            var id = $.urlParam('pNumCheque');
            var dvReport = document.getElementById("dvReport");
            if (id != null) {
                dvReport
                var frame1 = dvReport.getElementsByTagName("iframe")[0];
                $('#crvReporteEstandar').contents().find('iframe').contents().find('head').append("<style> .cssb{font-family:'Times New roman'!important; font-size:11px !important; }</style>");
                if (navigator.appName.indexOf("Internet Explorer") != -1 || navigator.appVersion.indexOf("Trident") != -1) {
                    frame1.name = frame1.id;
                    window.frames[frame1.id].focus();
                    window.frames[frame1.id].print();
                } else {
                    var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                    frameDoc.print();
                    
                }
            }

        }

        var getUrlParameter = function getUrlParameter(sParam) {
            var sPageURL = window.location.search.substring(1),
                sURLVariables = sPageURL.split('&'),
                sParameterName,
                i;

            for (i = 0; i < sURLVariables.length; i++) {
                sParameterName = sURLVariables[i].split('=');

                if (sParameterName[0] === sParam) {
                    return typeof sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
                }
            }
            return false;
        };

    </script>
   
</head>

<body style="margin: 0px 0px 0px 0px;">
    <form id="form1" runat="server">
        <asp:Button ID="Button1" runat="server" CssClass="clsExportar" Text="Exportar PDF" OnClick="Button1_Click" />
        <asp:Button ID="Button2" runat="server" CssClass="clsExportar" Text="Exportar EXCEL" OnClick="Button2_Click" />
        <asp:Button ID="Button3" runat="server" CssClass="clsExportar" Text="Exportar WORD" OnClick="Button3_Click" />
        <input id="btnPrint" type="button" value="Print" onclick="Print()" class="hide" />
        <button type="button" id="btnmdlPanelAuth" class="btn btn-warning" style="float: right;display:none;" data-dismiss="modal"><i class="fa fa-arrow-left"></i>regresar</button>
        <div id="dvReport">
            <CR:CrystalReportViewer ID="crvReporteEstandar" runat="server" AutoDataBind="true" CssClass="center-block" BestFitPage="False" OnPreRender="crvReporteEstandar_PreRender" />

        </div>
    </form>
</body>
</html>

