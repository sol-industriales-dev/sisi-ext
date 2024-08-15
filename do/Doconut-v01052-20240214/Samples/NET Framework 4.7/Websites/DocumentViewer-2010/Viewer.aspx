<!DOCTYPE html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Viewer.aspx.cs" Inherits="Viewer" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Welcome To Doconut - Online Document Viewer</title>
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1, maximum-scale=1" />
    <style type="text/css">
        body
        {
            margin: 0px;
            font-family: Verdana;
            font-size: 12px;
        }
        
    #divDocViewer {
        width: 100%;
        border-radius: 6px;
        background-color: whitesmoke;
        border: 1px grey;
    }

    /* IMP: This css is required to prevent bootstrap css from shrinking the main viewer */

    .row {
        margin-right: 0;
        margin-left: 0
    }
    
    /* end IMP */
    

    .loader {
        border: 5px solid #f3f3f3;
        border-top: 5px solid #3498db;
        border-radius: 50%;
        width: 30px;
        height: 30px;
        animation: spin 2s linear infinite;
    }

    @keyframes spin {
        0% {
            transform: rotate(0);
        }

        100% {
            transform: rotate(360deg);
        }
    }
    
        /* General CSS */
        
       
        #upload
        {
            margin: 20px;
            width: 95%;
            height: 100%;
            background-color: #79b933;
            text-align: center;
            padding: 20px;
        }
        
        #upload span
        {
            color: #fff;
            font-size: 14px;
        }
        
        /* General CSS Ends */
        
 
    </style>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js" type="text/javascript"></script>
    <!-- Bootstrap 4 JS -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js"></script>
    <!-- Bootstrap 4 CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css">
    <script type="text/javascript" language="javascript" src="js/menu.js"></script>
    <link rel="stylesheet" href="css/thickbox.css" type="text/css" />
    <link href="js/dropzone/basic.css" rel="stylesheet" />
    <link href="js/dropzone/dropzone.css" rel="stylesheet" />
</head>
<body>
    <form runat="server">
    <div id="viewer" runat="server" visible="false">
        <script type="text/javascript" src="js/thickbox-compressed.js"></script>
        <!-- Document Tools Starts -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container">
        <a class="navbar-brand" href="https://www.doconut.com" target="_blank">Webforms</a>&nbsp;<div class="loader"></div>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarResponsive" aria-controls="navbarResponsive" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarResponsive">
            <ul class="nav navbar-nav ml-auto">
                <li class="dropdown">
                    <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#">
                        Other Demos
                        <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" target="_blank" href="http://mvc.doconut.com">MVC Demo</a></li>
                        <li><div class="dropdown-divider"></div></li>
                        <li><a class="dropdown-item" target="_blank" href="http://core.doconut.com">.NET Core</a></li>
                    </ul>
                </li>
                <li class="dropdown">
                    <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#">
                        Navigation
                        <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.GotoPage(1);">First</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.Next(false);">Previous</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.Next(true);">Next</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.GotoPage(parseInt(objDoc.TotalPages()));">Last</a></li>
                        <li>&nbsp;Goto Page: <select id="cmbPages"></select></li>
                    </ul>
                </li>
                <li class="dropdown">
                    <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#">
                        Rotate
                        <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="RotateDocument(1);">90</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="RotateDocument(2);">180</a></li>              
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="RotateDocument(3);">270</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="RotateDocument(0);">None</a></li>
                    </ul>
                </li>
                <li class="dropdown">
                                <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#">Flip
                        <span class="caret"></span>
                                </a>
                                <ul class="dropdown-menu">
                                    <li><a class="dropdown-item" href="javascript:void(0)" onclick="FlipDocument('X');">X</a></li>
                                    <li><a class="dropdown-item" href="javascript:void(0)" onclick="FlipDocument('Y');">Y</a></li>
                                    <li><a class="dropdown-item" href="javascript:void(0)" onclick="FlipDocument('XY');">XY</a></li>
                                    <li><a class="dropdown-item" href="javascript:void(0)" onclick="FlipDocument('');">None</a></li>
                                </ul>
                            </li>
                <li class="dropdown">
                    <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#">
                        Zoom
                        <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.Zoom(true);">In</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.Zoom(false);">Out</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.Zoom(25);">25 %</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.Zoom(100);">100 %</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.FitType('width');">Fit Width</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.FitType('height');">Fit Height</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.FitType('');">Fit None</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="alert(objDoc.CurrentZoom());">Show Zoom</a></li>
                    </ul>
                </li>
                 <li class="dropdown">
                    <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#">
                        Copy Text
                        <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.CopyMode(true);">Copy On</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.CopyMode(false);">Copy Off</a></li>
                        <li><div class="dropdown-divider"></div></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.CopyPage(objDoc.CurrentPage());">Copy Page</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="var f = objDoc.FileFormat();alert(f);">File Format</a></li>
                    </ul>
                </li>
                <li class="dropdown">
                    <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#">
                        Thumbnails
                        <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.ThumbSize('small');">Small</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.ThumbSize('normal');">Normal</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.ThumbSize('large');">Large</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.HideThumbs(true);">Hide Thumbs</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.HideThumbs(false);">Show Thumbs</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.HideSplitter(true);">Splitter Off</a></li>
                        <li><a class="dropdown-item" href="javascript:void(0)" onclick="objDoc.HideSplitter(false);">Splitter On</a></li>
                    </ul>
                </li>
                <li class="dropdown">
                    <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#">
                        Functions
                        <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                      
                        <li>&nbsp;<asp:Button ID="btnThumbnail" runat="server" CssClass="button" Text="Get Thumbnail"
                        OnClick="btnThumbnail_Click" /></li>
                          <li>&nbsp;<asp:Button ID="btnSaveDocument" runat="server" CssClass="button" Text="Export To Dcn"
                        OnClick="btnSaveDocument_Click" /></li>
                        <li>&nbsp;<asp:Button ID="btnSaveSearch" runat="server" CssClass="button" Text="Export To Srh"
                        OnClick="btnSaveSearch_Click" /></li>
                        <li>&nbsp;<asp:Button ID="btnSavePdf" runat="server" CssClass="button" Text="Export To Pdf"
                        OnClick="SavePDF_Click" /></li>
                        <li>&nbsp;<asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close Document"
                        OnClick="btnClose_Click" /></li>
                    </ul>
                </li>
                <li class="nav-item active">
                    <a class="thickbox nav-link" title="ASP.NET Document Print" href='Print.aspx?token=<%= ctlDoc.Token %>&printtotal=<%= ctlDoc.TotalPages %>&KeepThis=false&TB_iframe=true&height=150&width=400'>Print File</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="javascript:void(0);" onclick="GoFS();">Full Screen</a>
                </li>               
            </ul>
        </div>
    </div>
</nav>
        <!-- Document Tools Ends -->
        <!-- Document Viewer Starts -->
        <div id="divDocViewer">
            <asp:DocViewer ID="ctlDoc" runat="server" ShowThumbs="true" AutoScrollThumbs="true"
                AutoFocusPage="false" IncludeJQuery="false" ImgFormat="Png" FitType="width" ImageResolution="200"
                TimeOut="15" ShowHyperlinks="true" ZoomStep="5" DebugMode="true" FixedZoom="true"
                FixedZoomPercent="100" FixedZoomPercentMobile="75" />
        </div>
        <!-- Document Viewer Ends -->
        <!-- Modify following to customize document viewer  -->
        <link type="text/css" rel="Stylesheet" href="css/custom.css" />
    </div>
    <!-- Upload Document Starts -->
    <div id="upload" runat="server">
        <div class="row">
            <div class="col-sm-12">
                <span>You can view DOC/DOCX/ODT - XLS/XLSX/ODS/CSV - PPT/PPTX/ODP & PDF - VSD - MPP
                    - TIF - XPS - PSD - DWG - DXF - DGN - EML - MSG - TXT - RTF - XML - EPUB - SVG -
                    HTML - MHT - DICOM & Common Image Formats</span><br />
                <br />
                <div id="dropZoneForm" class="dropzone">
                    <div class="dz-message">
                        Click or drop your document here</div>
                    <div>
                        <div class="fallback">
                            <input name="file" id="file" type="file" />
                        </div>
                    </div>
                </div>
                <br />
                <br />
                <b>OR</b>&nbsp;Enter full document or website url:&nbsp;<asp:TextBox ID="txtUrl"
                    runat="server" Text="http://www." Width="200px"></asp:TextBox>&nbsp;<asp:Button ID="btnGo"
                        runat="server" Text="Go" OnClick="btnGo_Click" />
                <br />
                <span>copyright
                    <%= DateTime.Now.Year.ToString() %>
                    &copy; www.doconut.com</span>
            </div>
        </div>
    </div>
    <!-- Upload Document Ends -->
    </form>
    <script type="text/javascript" src="js/dropzone/dropzone.js"></script>
    <script language="javascript" type="text/javascript">

     var objDoc = null;
     Dropzone.autoDiscover = false;
     var loader = $(".loader");

     if (typeof jQuery != 'undefined')
     {

            $(document).ready(function () {

                 $("#dropZoneForm").dropzone({
                    url: "UploadFile.aspx",
                    maxFiles: 1,
                    paramName: "file",
                    uploadMultiple: false,
                    maxFilesize: 20,
                    acceptedFiles: ".doc,.docx,.docm,.odt,.xls,.xlsx,.xlsm,.ods,.csv,.ppt,.pptx,.odp,.vsd,.vsdx,.mpp,.mppx,.pdf,.tif,.tiff,.dwg,.dxf,.dgn,.xps,.psd,.jpg,.jpeg,.jpe,.png,.bmp,.gif,.eml,.msg,.txt,.rtf,.xml,.epub,.svg,.html,.htm,.mht,.dcn,.dcm",
                    addRemoveLinks: false,
                    init: function () {
                        var th = this;
                        this.on("success", function (file, response) {
			   if(response.length == 0) {
                               alert("Error uploading file, please refer UploadFile.aspx");
                               return;
                            }

                           $(".dz-message").html("Opening Document...Please wait.");
                           window.location.href= "Viewer.aspx?file=" + response;
                        }),
                            this.on("error", function (file, errorMessage, c) {
                                alert("Error uploading document [[" + file.name + "]]. Technical team has been notified.");
                            }),
                            this.on("queuecomplete", function () {
                                setTimeout(function () {
                                    th.removeAllFiles();
                                }, 3000);
                            });
                    }
                });


          

            
            if(typeof(<%= ctlDoc.JsObject %>) != 'undefined')
            {
                objDoc = <%= ctlDoc.JsObject %>;

                var height = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;

                // If you want to resize viewer with window resizing
                $(window).on("resize", 
                
                    function() { 
                    
                        var h = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;  
                        $("#divDocViewer").height(h - 80); 
                       
                        }
                 );
         
                $("#divDocViewer").height(height - 80);


                   var totalp = objDoc.TotalPages();

                    if (totalp > 0) 
                    {

                        for (var i = 1; i <= totalp; i++) {
                            $("#cmbPages").append('<option value=' + i + '>' + i + '</option>');
                        }
            
                        $("#cmbPages").on("change", function () { objDoc.GotoPage(parseInt($(this).val())); });
                    }
            }
          
          });
       }      

       function RotateDocument(iAngle)
       {
            objDoc.Rotate(objDoc.CurrentPage(), iAngle);
       }

        function FlipDocument(flipType) {
            objDoc.Flip(objDoc.CurrentPage(), flipType);
        }

        function ctlDoc_DoubleClick() // or use as: function <%= ctlDoc.ClientID %>_DoubleClick()
        {
           objDoc.Zoom(true);
        }

         function ctlDoc_Copy(text) {
            var f = objDoc.FileFormat();
            if(f !== "Pdf")
            {
                alert("Copy function not available for " + f);
            }
            else{
                if (text === "" || text === null) {
                    alert("Copy failed");
                }
                else {
                    alert(text);
                }
            }
        }

        // optional code start
        function ctlDoc_OnViewerBusy() {
            loader.show();
        }

        function ctlDoc_OnViewerReady() {
            loader.hide();
        }

        function GoFS() {

            if (
                document.fullscreenElement ||
                    document.webkitFullscreenElement ||
                    document.mozFullScreenElement ||
                    document.msFullscreenElement
            ) {
                if (document.exitFullscreen) {
                    document.exitFullscreen();
                } else if (document.mozCancelFullScreen) {
                    document.mozCancelFullScreen();
                } else if (document.webkitExitFullscreen) {
                    document.webkitExitFullscreen();
                } else if (document.msExitFullscreen) {
                    document.msExitFullscreen();
                }
            } else {
                var element = $('#divDocViewer').get(0);

                if (element.requestFullscreen) {
                    element.requestFullscreen();
                } else if (element.mozRequestFullScreen) {
                    element.mozRequestFullScreen();
                } else if (element.webkitRequestFullscreen) {
                    element.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
                } else if (element.msRequestFullscreen) {
                    element.msRequestFullscreen();
                }
            }
        }

    </script>
</body>
</html>
