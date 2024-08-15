<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlipPage.aspx.cs" Inherits="Full.FlipPage" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Flip Page (Book) - Doconut</title>
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.10.0.min.js"></script>
    <script src="Turn/js/turn.min.js" type="text/javascript"></script>
    <link href="Turn/css/basic.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <center>
            <div id="flipbook">
            </div>
            <div id="tools">
                <a href="javascript:void(0);" onclick="FB.turn('previous')">Previous</a>&nbsp;<a
                    href="javascript:void(0);" onclick="FB.turn('next')">Next</a>&nbsp;&nbsp;<a href="http://turnjs.com/docs/Main_Page" target="_blank">Need More Functions?</a>
            </div>
        </center>
    </form>
    <div style="display: none">
        <asp:DocViewer ID="ctlDoc" runat="server" Visible="true" ShowThumbs="false" AutoFocusPage="false"
            AutoScrollThumbs="false" IncludeJQuery="false" ImageResolution="100" ImgFormat="Png" />
    </div>

    <script language="javascript" type="text/javascript">

        var FB = null;

        function InitFlipBook() {

            var docToken = '<%= Session["DOC_FB_Token"] %>';
            var baseUrl = '<%= Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath %>';
            baseUrl = baseUrl + "/DocImage.axd";
            var desiredWidth = 600;

            var firstImg = $("<img />").load(function () {

                var h = this.height;
                var totalPages = objctlDoc.TotalPages();
                var fbContainer = $("#flipbook");


                for (var ipg = 1; ipg <= totalPages; ipg++) {
                    var divPage = $('<div style="background-image:url(' + baseUrl + "?thumb=1&page=" + ipg + "&width=" + desiredWidth + "&token=" + docToken + ')" />');

                    fbContainer.append(divPage);
                }

                FB = $("#flipbook").turn({
                    width: desiredWidth * 2,
                    height: h,
                    autoCenter: true
                });


            }).error(function () {
                alert("Error loading flip book");
            }).attr('src', baseUrl + "?thumb=1&page=1&width=" + desiredWidth + "&token=" + docToken);

        }
    </script>
</body>
</html>
