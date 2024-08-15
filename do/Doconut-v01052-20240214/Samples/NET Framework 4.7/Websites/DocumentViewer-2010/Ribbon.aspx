<!DOCTYPE html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ribbon.aspx.cs" Inherits="Ribbon" %>
<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Office Ribbon Menu - Doconut</title>
    <meta http-equiv="Content-type" content="text/html;charset=UTF-8" />

      <!-- using external jQuery, see IncludejQuery property -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.11.0/jquery.js" type="text/javascript"></script>

    <!-- magnifier -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/magnify/2.3.3/js/jquery.magnify.min.js"
        type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/magnify/2.3.3/js/jquery.magnify-mobile.min.js"
        type="text/javascript"></script>
  
    <script src="js/jquery.ribbon.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/thickbox-compressed.js"></script>
    <link href="css/ribbon.css" rel="Stylesheet" type="text/css" />
    <link href="css/thickbox.css" rel="stylesheet" type="text/css" />

    <!-- magnifier -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/magnify/2.3.3/css/magnify.min.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .ui-widget
        {
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id="frmRibbon" runat="server">
    <!-- Ribbon Starts -->
    <div class="mainContainer">
        <ul class="ribbon">
            <li>
                <ul class="orb">
                    <li><a href="javascript:void(0);" accesskey="1" class="orbButton">&nbsp;</a><span>Menu</span>
                        <ul>
                            <li><a href="javascript:void(0);" onclick="DoUpload();">
                                <img src="ribbon/images/icon_open.png" title="Open" /><span>Open</span></a>
                            </li>
                            <li><a href="#">
                                <img src="ribbon/images/icon_saveas.png" title="Save as" /><span>Save As</span></a>
                                <ul>
                                    <li>
                                        <asp:LinkButton ID="lnkExport" OnClick="SaveDCN_Click" runat="server">
                                        <img src="ribbon/images/save_dcn.png" title="DCN Format" /><span>Doconut - DCN Format</span></asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="lnkExportPDF" OnClick="SavePDF_Click" runat="server">
                                         <img src="ribbon/images/save_pdf.png" title="PDF Format" /><span>Acrobat - PDF</span></asp:LinkButton>
                                    </li>
                                </ul>
                            </li>
                            <li><a href="#">
                                <img src="ribbon/images/icon_print.png" title="Print" /><span>Print</span></a>
                                <ul>
                                    <li><a class="thickbox" title="ASP.NET Document Print" href='Print.aspx?token=<%= ctlDoc.Token %>&printtotal=<%= ctlDoc.TotalPages %>&KeepThis=false&TB_iframe=true&height=140&width=300'>
                                        <img src="ribbon/images/print.png" title="Print ASP.NET" /><span>Print - ASP.NET</span></a>
                                    </li>
                                </ul>
                            </li>
                            <li><a href="javascript:void(0);" onclick="alert('Doconut - ASP.NET Online Document Viewer');">
                                <img src="ribbon/images/icon_about.png" title="About" /><span>About</span></a>
                            </li>
                            <li><a href="javascript:void(0);" onclick="window.location.href='https://doconut.com/'">
                                <img src="ribbon/images/icon_exit.png" title="Exit" /><span>Exit</span></a>
                            </li>
                        </ul>
                    </li>
                </ul>
            </li>
            <li>
                <ul class="menu">
                    <li><a href="#document" accesskey="2">Document</a>
                        <ul>
                            <li>
                                <h2>
                                    <span>Navigation</span></h2>
                                <div onclick="objDoc.GotoPage(1);">
                                    <img src="ribbon/images/first_page.png" title="Go First" />
                                    First
                                </div>
                                <div onclick="objDoc.Next(false);">
                                    <img src="ribbon/images/prev_page.png" title="Go Previous" />
                                    Previous
                                </div>
                                <div onclick="objDoc.Next(true);">
                                    <img src="ribbon/images/next_page.png" title="Go Next" />
                                    Next
                                </div>
                                <div onclick="objDoc.GotoPage(parseInt(objDoc.TotalPages()));">
                                    <img id="imgLast" src="ribbon/images/last_page.png" title="Go Last" />
                                    Last
                                </div>
                            </li>
                            <li>
                                <div class="ribbon-list">
                                    <div>
                                        <img src="ribbon/images/fit_type.png" title="Fit Type" />
                                        Fit Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <ul>
                                            <li onclick="objDoc.FitType('width');">Width</li>
                                            <li onclick="objDoc.FitType('height');">Height</li>
                                            <li onclick="objDoc.FitType('');">None</li>
                                        </ul>
                                    </div>
                                    <br />
                                    <div>
                                        <img src="ribbon/images/thumb_size.png" title="Thumbnail Size" />
                                        Thumbnail Size&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <ul>
                                            <li onclick="objDoc.ThumbSize('small');">Small</li>
                                            <li onclick="objDoc.ThumbSize('normal');">Normal</li>
                                            <li onclick="objDoc.ThumbSize('large');">Large</li>
                                        </ul>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </li>
                    <li><a href="#view" accesskey="3">View</a>
                        <ul>
                            <li>
                                <h2>
                                    Zoom</h2>
                                <div onclick="objDoc.Zoom(true)">
                                    <img src="ribbon/images/zoom_in.png" title="Zoom in" />
                                    Zoom in
                                </div>
                                <div onclick="objDoc.Zoom(false)">
                                    <img src="ribbon/images/zoom_out.png" title="Zoom out" />
                                    Zoom out
                                </div>
                                <div onclick="objDoc.Zoom(100)">
                                    <img src="ribbon/images/zoom_100.png" title="Zoom" />
                                    100%
                                </div>
                                <div onclick="objDoc.Zoom(25)">
                                    <img src="ribbon/images/zoom_100.png" title="Zoom" />
                                    25%
                                </div>
                                <div onclick="ToggleMagnify();">
                                        <img src="ribbon/images/icon_printpreview.png" title="Magnify" />Magnify
                                </div>
                            </li>
                            <li>
                                <h2>
                                    Show or hide</h2>
                                <div class="ribbon-list">
                                    <div>
                                        <input type="checkbox" id="chkTh" checked="checked" />
                                        <label accesskey="t" for="chkTh">
                                            Thumbnails</label>
                                    </div>
                                    <div>
                                        <input type="checkbox" id="chkSp" checked="checked" />
                                        <label accesskey="s" for="chkSp">
                                            Splitter</label>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </li>
                </ul>
            </li>
        </ul>
    </div>
    <!-- Ribbon Ends -->
    <!-- Document Viewer Starts -->
    <div id="divDocViewer" style="position: fixed; width: 100%; z-index: -1">
        <asp:DocViewer ID="ctlDoc" runat="server" ShowThumbs="true" AutoScrollThumbs="true"
            AutoFocusPage="false" Zoom="50" IncludeJQuery="false" ImgFormat="Png" FitType="width"
            ImageResolution="200" TimeOut="10" AutoClose="true" />
    </div>
    <div id="divUpload" style="display: none">
        <b>Please upload your document:</b><br />
        <asp:FileUpload ID="uploadFile" runat="server" />&nbsp;<asp:Button ID="btnUploadFile"
            runat="server" Text="Upload File" OnClick="btnUpload_Click" />
        <br />
        &nbsp;<b>OR</b>&nbsp;<br />
        <br />
        Enter full document or website url:&nbsp;<asp:TextBox ID="txtUrl" runat="server"
            Text="http://www." Width="200px"></asp:TextBox>&nbsp;<asp:Button ID="btnGo" runat="server"
                Text="Go" OnClick="btnGo_Click" />
    </div>
    <!-- Document Viewer Ends -->
    </form>
    <script language="javascript" type="text/javascript">

         var objDoc = null;
         var rotAlert = false;

         var origWid = 0;
         var origZoom = 50;

         if (typeof jQuery != 'undefined')
         {
             $(document).ready(function () {

                 $().Ribbon({ theme: 'windows7' });
                 origWid = $(window).width();

                 var height = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;
                 $("#divDocViewer").height(height - 130);

                      
                 if(typeof(<%= ctlDoc.JsObject %>) !== 'undefined')
                 {
                     objDoc = <%= ctlDoc.JsObject %>;
                   
                     $("#chkTh").on("click", function () { objDoc.HideThumbs(!$(this).is(':checked')) });  
                     $("#chkSp").on("click", function () { objDoc.HideSplitter(!$(this).is(':checked')) });  


                     // Resizing (optional code, can be removed)
                     $(window).resize(function () {

                         // If you want to resize viewer with window resizing

                         var h = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;
                         $("#divDocViewer").height(h - 130);

                         // If you want to change zoom with window resizing

                         var newWidth = $(window).width();
                         var newZoom = parseInt((newWidth * origZoom) / origWid);

                         setTimeout(function () { objDoc.Zoom(newZoom) }, 1500);

                     });

                 }

             });      
         }

        var magnifyMode = false;
        var magnifyGlass = null;

        function ToggleMagnify() {
            magnifyMode = !magnifyMode;

            if (magnifyMode == true)
            {
                if (magnifyGlass == null) {
                    var page = objDoc.CurrentPage();
                    AddMagnify(page);
                }
            }
            else {
                ClearMagnify();
            }
        }        

        function AddMagnify(pageNum) {
            var ele = "div_ctlDoc_divPreview_divPage_" + pageNum;

            var jQele = $('#' + ele);
            var zoomVal = getUrlVars(jQele.attr("src"))["zoom"];

            var newSrc = jQele.attr("src").replace(zoomVal, "200");

            jQele.attr("data-magnify-src", newSrc);

            magnifyGlass = jQele.magnify();
        }

        function ClearMagnify() {
            if (null != magnifyGlass) {
                magnifyGlass.destroy();
                magnifyGlass = null;
            }
        }

        function ctlDoc_OnPageClicked(t) {
            if (magnifyMode == true) {
                ClearMagnify();
                AddMagnify(t);
            }
        }

        function getUrlVars(url) {
            var vars = [], hash;
            var hashes = url.slice(url.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        }

         function DoUpload()
         {
             $("#divUpload").dialog({
                height: 220, width: 360, modal: false,
                title: 'View Document',
                resizable: false
            });

             $('#divUpload').parent().appendTo($('form:first'));
         }
    </script>
</body>
</html>
