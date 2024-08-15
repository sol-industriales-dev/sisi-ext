<!DOCTYPE html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Mobile.aspx.cs" Inherits="Mobile" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mobile - Doconut</title>
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1, maximum-scale=1">
    <script src="https://hammerjs.github.io/dist/hammer.js" type="text/javascript"></script>
    <style type="text/css">
        body
        {
            margin: 0px;
            padding: 5px;
        }
        
        #divDocViewer {
             height: 97vh;
             width: 97vw;
         }
    </style>
</head>
<body>
    <form id="frmMobile" runat="server">
    <div id="divDocViewer">
        <asp:DocViewer ID="ctlDoc" runat="server" ShowThumbs="false" ShowToolTip="true"
            AutoFocusPage="true" AutoScrollThumbs="false" FitType="width" IncludeJQuery="true"
            ImgFormat="Png" CacheEnabled="false" AutoLoadPages="false" Zoom="75" />
    </div>
    </form>
    <script language="javascript" type="text/javascript">

          var isMobile = false;
          var resizing = false;
          var w = 0;
          var h = 0;
          var objDoc = null;
          var docViewerDiv = $("#divDocViewer");

          if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
            isMobile = true;
          }

          
            function Resize(orientation) {

                    if (resizing) { return; }

                    if(typeof objDoc !== 'undefined' && null !== objDoc) {
                      objDoc.FitType('width');
                      objDoc.Refit();
                    }

                    resizing = false;
             }

             function SetPinchZoom()
             {
                    // the scroll element ID is div_yourcontrolid_divScroll

                    var mc = new Hammer.Manager(document.getElementById('div_ctlDoc_divScroll'), {
                        touchAction: 'manipulation'
                    });
                    mc.add([new Hammer.Pinch()]);

                    mc.on("pinchin", function (ev) {
                        objDoc.Zoom(false); 
                    });
                    mc.on("pinchout", function (ev) {
                         objDoc.Zoom(true);
                    });
             }

                   
           $(document).ready(function () {

                 if(typeof(<%= ctlDoc.JsObject %>) != 'undefined')
                 {
                      objDoc = <%= ctlDoc.JsObject %>;
                 }

                 Resize();
                 SetPinchZoom();
            });


           $(window).on('load', function () {
                 Resize();
            });
     

            if (isMobile) {
               $(window).on("orientationchange", function (event) { Resize(event.orientation); });
            }
            else {
                $(window).on("resize", function () { Resize(); });
            }  
                      

    </script>
</body>
</html>
