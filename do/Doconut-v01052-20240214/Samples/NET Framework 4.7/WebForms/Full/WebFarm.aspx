<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFarm.aspx.cs" Inherits="Full.WebFarm" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WebFarm - Doconut</title>
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.11.0.min.js"></script>
    <script>
        var objctlDoc = null;
    </script>
</head>
<body>
    <h2 id="loading">Loading...</h2>
    <form id="frmWebFarm" runat="server">
        <div id="divDocViewer">
            <div id="div_ctlDoc"></div>
        </div>
    </form>

    <div style="display: none">
        <asp:DocViewer ID="dummy" runat="server" IncludeJQuery="false" />
    </div>

    <script language="javascript" type="text/javascript">

        function Resize() {

            var h = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;
            var w = "innerWidth" in window ? window.innerWidth : document.documentElement.offsetWidth;

            $("#divDocViewer").height(h - 70);
            $("#divDocViewer").width(w);

        }

        $(window).resize(function () {
            Resize();
        });

    </script>
</body>
</html>
