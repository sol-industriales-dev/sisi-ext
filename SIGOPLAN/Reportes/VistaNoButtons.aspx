<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VistaNoButtons.aspx.cs" Inherits="SIGOPLAN.Reportes.Vista" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Content/style/css/CRViewer.css" rel="stylesheet" />
    <script src="../Scripts/jquery-3.1.0.js"></script>
</head>

<body style="margin: 0px 0px 0px 0px;">
    <form id="form1" runat="server">
<%--        <asp:Button ID="Button1" runat="server" Text="Exportar PDF" OnClick="Button1_Click" />
        <asp:Button ID="Button2" runat="server" Text="Exportar EXCEL" OnClick="Button2_Click" />
        <asp:Button ID="Button3" runat="server" Text="Exportar WORD" OnClick="Button3_Click" />--%>
        <CR:CrystalReportViewer ID="crvReporteEstandar" runat="server" AutoDataBind="true" CssClass="center-block" BestFitPage="False" />
    </form>
</body>
</html>

