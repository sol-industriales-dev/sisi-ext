<!DOCTYPE html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Events.aspx.cs" Inherits="Events" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JavaScript Events - Doconut</title>
    <style type="text/css">
        body
        {
            font-family: Verdana;
            font-size:12px;
        }
    </style>
    <link type="text/css" rel="Stylesheet" href="css/custom.css" />
</head>
<body>
    <form id="form1" runat="server">
    <input type="text" id="debug" style="padding: 5px; margin: 5px; overflow: scroll;
        height: 25px; width: 98%" onclick="this.value = ''" />
    <div id="divDocViewer">
        <asp:DocViewer ID="ctlDoc" runat="server" Zoom="25"  />
    </div>
    <br />
    &nbsp;<a href="javascript:void(0);" onclick="VisiblePages();">Visible Pages?</a>&nbsp;<a
        href="javascript:void(0);" onclick="alert(<%= ctlDoc.JsObject %>.CurrentPage());">Current Page?</a>
    <br />
    </form>
    <script language="javascript" type="text/javascript">

          var objDebug = document.getElementById("debug");

            function Resize() {

                
                var h = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;
                var w = "innerWidth" in window ? window.innerWidth : document.documentElement.offsetWidth;

                $("#divDocViewer").height(h - 100);
                $("#divDocViewer").width(w - 25);

             }

             function ShowDebug(msg)
             {
                objDebug.value = msg + "...   " + objDebug.value;
             }

             function VisiblePages()
             {
                alert(objDoc.VisiblePages());
             }
        
               $(document).ready(function () {

                     if(typeof(<%= ctlDoc.JsObject %>) != 'undefined')
                     {
                          objDoc = <%= ctlDoc.JsObject %>;
                          Resize();
                     }  
               });


            function ctlDoc_OnThumbnailClicked(t) 
            {
              ShowDebug('Thumbnail #' + t + ' clicked');

              if(objDoc.Annotating())
              {
                objDoc.SaveAnnotations(); // save
                objDoc.CloseAnnotations(true); // close
                OpenAnnotationWindow(t); // open another page
                objDoc.ShowPage(t, false); // focus new page
              }
            }

            function ctlDoc_OnPageClicked(t) 
            {
                ShowDebug('Page #' + t + ' clicked');
            }

            function ctlDoc_OnPageLoading(p) 
            {
              ShowDebug('Page #' + p + ' loading');
              ViewerStatus(true);
            }

            function ctlDoc_OnPageLoaded(p) 
            {
               ShowDebug('Page #' + p + ' loaded');
               ViewerStatus(false);
            }

          
            function ctlDoc_OnViewerBusy() 
            {
              ViewerStatus(true);
            }
     
            function ctlDoc_OnViewerReady() 
            {
              ViewerStatus(false);
            }

            function ctlDoc_DoubleClick() 
            {
                OpenAnnotationWindow();
            }

 	    function ctlDoc_AutoLoadStatus(page)
            {
               var tp = objDoc.TotalPages();

               if(parseInt(page) == parseInt(tp)) {
                 alert("Auto load : ALL DONE!");
               }
            }

            function ctlDoc_OnViewerError()
            {
              alert("THERE WAS AN ERROR!");
            }

            function ViewerStatus(isBusy)
            {
                if(isBusy)
                {
                  $(".docScrollPane").css({ 'opacity' : 0.2 });
                }
                else
                {
                  $(".docScrollPane").css({ 'opacity' : 1 });
                }
            }

            function OpenAnnotationWindow(p)
            {
               objDoc.ShowAnnotations(50, true, p); // (zoom, showButton, page #)
               $('.divAnn').width('80%');
            }
    </script>
</body>
</html>
