<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExcelViewer.aspx.cs" Inherits="ExcelViewer" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Excel Viewer - Doconut</title>
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1, maximum-scale=1" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js" type="text/javascript"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"
        type="text/javascript"></script>
    <style type="text/css">
        .sheetContainer
        {
            background-color: #ddd;
            overflow: auto;
            cursor:grab;
        }
        
        .sheetContainer img
        {
            pointer-events: none;
        }
        
        .tab-content
        {
            border-left: 1px solid #ddd;
            border-right: 1px solid #ddd;
            padding: 10px;
        }
        
        .nav-tabs
        {
            margin-bottom: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divUpload" class="container-fluid">
        <div class="row">
            <div>
                <asp:FileUpload ID="txtUpload" CssClass="form-control" runat="server" /><br />
                <asp:Button ID="btnUpload" CssClass="btn  btn-primary" runat="server" OnClick="Upload_Insert_Click"
                    Text="Upload & View File!" />
            </div>
        </div>
    </div>
    </form>
    <div id="divViewer" class="container-fluid">
        <h2>
            Excel Viewer</h2>
        <ul id="ulTab" class="nav nav-tabs">
        </ul>
        <div id="ulTabContent" class="tab-content">
        </div>
    </div>
    <script type="text/javascript">

        var clicking = false;
        var previousX;
        var previousY;

        $("#divViewer").hide();

        function BuildExcelUI(token, pages, sheetNames) {
            $("#divUpload").hide();
            $("#divViewer").show();

            var sNames = sheetNames.split("^");

            var ulTab = $("#ulTab");
            var ulTabContent = $("#ulTabContent");

            for (var iSheet = 0; iSheet < pages; iSheet++) {
                var stab = '<li><a data-toggle="tab" href="#sheet' + (iSheet + 1) + '">' + sNames[iSheet] + '</a></li>';
                ulTab.append(stab);

                // IMPORTANT: CHANGE THE DOCIMAGE.AXD PATH AS PER YOUR PROJECT
                // IF YOU SEE A BROKEN IMAGE / 404 ICON

                var sheetImgSrc = 'DocImage.axd?token=' + token + '&page=' + (iSheet + 1) + '&zoom=100';
                var sheetImage = '<div class="sheetContainer"><img src="' + sheetImgSrc + '" /></div>';

                var scontent = '<div id="sheet' + (iSheet + 1) + '" class="tab-pane fade"><p>' + sheetImage + '</p></div>';
                ulTabContent.append(scontent);
            }

            $('.nav-tabs a[href="#sheet1"]').tab('show');

            w = ulTabContent.width();
            h = document.documentElement.clientHeight;

            $('.sheetContainer').each(function (i) { $(this).width(w - 10).height(h - 150); });
    

        $(".sheetContainer").each(function (i) {

            $(this).on('mousedown', function (e) {
                
                e.preventDefault();
                previousX = e.clientX;
                previousY = e.clientY;
                clicking = true;
            });
        });

        $(document).on('mouseup', function () {
            clicking = false;
        });

        $(".sheetContainer").each(function (i) {
            $(this).on('mousemove', function (e) {

                scroll = $(this);

                if (clicking) {
                    e.preventDefault();
                    var directionX = (previousX - e.clientX) > 0 ? 1 : -1;
                    var directionY = (previousY - e.clientY) > 0 ? 1 : -1;
                    scroll.scrollLeft(scroll.scrollLeft() + (previousX - e.clientX));
                    scroll.scrollTop(scroll.scrollTop() + (previousY - e.clientY));
                    previousX = e.clientX;
                    previousY = e.clientY;
                }
            });
        });

        $(".sheetContainer").on('mouseleave', function (e) {
            clicking = false;
        });

    }

    </script>
</body>
</html>
