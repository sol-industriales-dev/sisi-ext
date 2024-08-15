<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Custom-Scripts.aspx.cs" Inherits="Full.Custom_Scripts" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Custom Scripts - Doconut</title>
</head>
<body>
    <form id="frmCustomScripts" runat="server">

        <div id="divDocViewer">
            <asp:DocViewer ID="ctlDoc" runat="server" Zoom="100" ShowThumbs="true" ShowToolTip="true"
                AutoFocusPage="true" AutoScrollThumbs="false" ImageResolution="100"
                ImgFormat="Png" AddScripts="false" />
        </div>

    </form>

    <!-- All external scripts -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.12.3/jquery.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js" type="text/javascript"></script>


    <!-- Doconut related scripts -->
    <%= ctlDoc.RegisterScripts() %>


    <!-- Doconut Init (main) -->
    <script type="text/javascript">
     <%= ctlDoc.GetAjaxInitArguments(ctlDoc.Token) %>
    </script>

    <script language="javascript" type="text/javascript">

        var objDoc = null;

        if (typeof jQuery != 'undefined') {
            $(document).ready(function () {

                var height = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;

                $("#divDocViewer").height(height - 80);

                if (typeof (<%= ctlDoc.JsObject %>) != 'undefined') {
                    objDoc = <%= ctlDoc.JsObject %>;

                    // alert(objDoc.TotalPages());
                }

            });
        }

    </script>
</body>
</html>
