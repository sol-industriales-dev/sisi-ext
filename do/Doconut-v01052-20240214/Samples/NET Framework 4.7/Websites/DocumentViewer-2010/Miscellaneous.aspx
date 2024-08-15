<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Miscellaneous.aspx.cs" Inherits="Miscellaneous" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Miscellaneous - Doconut</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divDocViewer">
<asp:DocViewer ID="ctlDoc" runat="server" Zoom="100" ShowThumbs="true" ShowToolTip="false"
            AutoFocusPage="false" AutoScrollThumbs="false" IncludeJQuery="true" TimeOut="5" ImageResolution="100"
            ImgFormat="Png" ToolTipPageText="Page #" ZoomStep="5" DebugMode="true" AutoLoadPages="false" WatermarkInfo="^Sample Copy~Gray~24~Arial~70~-45" />
    </div>
    </form>
    <script language="javascript" type="text/javascript">

          var objDoc = null;

            function Resize() {

                var h = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;
                var w = "innerWidth" in window ? window.innerWidth : document.documentElement.offsetWidth;

                $("#divDocViewer").height(h - 20);
                $("#divDocViewer").width(w - 20);

             }

        
           $(document).ready(function () {

                 if(typeof(<%= ctlDoc.JsObject %>) != 'undefined')
                 {
                    objDoc = <%= ctlDoc.JsObject %>;


                    // move thumbnail to right side
                    objDoc.HideSplitter(true);

                    // or
                    // $('#' + objDoc.attr("id") + '_splitter').css('float','right');

                     $('#' + objDoc.attr("id") + '_divThumbs').css('float','right');

                     Resize();

                     $(window).resize(function () {
                        Resize();
                     });        
                 }  

           });

     

    </script>
</body>
</html>
