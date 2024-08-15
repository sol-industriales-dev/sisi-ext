<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Full.Search" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document Search</title>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />

    <style type="text/css">
        body {
            font-family: Tahoma;
        }
    </style>
</head>
<body>
    <link href="css/custom.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <!-- Document Viewer Starts -->
        <input type="button" value="Open Search Window" onclick="DoSearch();" style="margin: 10px;" />&nbsp|&nbsp;&nbsp;<input
            type="button" value="Copy Text" id="btnCopy" onclick="ToggleCopyMode();" />
        &nbsp;<input type="button" value="Copy Page" id="btnCopyPage" onclick="CopyPage();" />&nbsp&nbsp|&nbsp;&nbsp;<input
            type="button" value="Zoom Out" id="btnZoomOut" onclick="Zoom(false);" />
        &nbsp;<input type="button" value="Zoom In" id="btnZoomIn" onclick="Zoom(true);" />&nbsp&nbsp|&nbsp;&nbsp;<input
            type="button" value="File Format" id="btnFileFormat" onclick="ShowFormat();" />
        <div id="divDocViewer">
            <asp:DocViewer ID="ctlDoc" runat="server" ShowThumbs="true" AutoFocusPage="false"
                AutoScrollThumbs="true" Zoom="75" IncludeJQuery="true" ImgFormat="Png" FitType="width"
                FixedZoom="true" ImageResolution="200" ShowHyperlinks="true" />
        </div>
        <!-- Document Viewer Ends -->
        <!-- search starts -->
        <div id="divSearch">
            <table cellpadding="5" cellspacing="5" width="100%">
                <tr>
                    <td>Keyword
                    </td>
                    <td>
                        <input type="text" id="kw" />
                    </td>
                </tr>
                <tr>
                    <td>Exact?
                    </td>
                    <td>
                        <input type="checkbox" id="ext" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="button" value="Search" onclick="SearchKeyword();" />
                        <input type="button" value="New Search" onclick="NewSearch();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="text" id="msg" readonly style="width: 100%; border: solid 1px #ccc; background-color: lightyellow;" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;Show Focus?<input type="checkbox" disabled id="chkFocus" />&nbsp;<input type="button"
                        value="Find Pages" id="btnSum" disabled onclick="SearchSummary();" />&nbsp;&nbsp;&nbsp;<input
                            type="button" value="Previous Page" id="btnPrevious" disabled onclick="SearchPage(false);" />&nbsp;<input
                                type="button" value="Next Page" id="btnNext" disabled onclick="SearchPage(true);" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="text" readonly style="width: 100%; border: solid 1px #ccc; background: transparent" id="log" />
                    </td>
                </tr>
            </table>
        </div>
        <!-- search ends -->
    </form>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        var objDoc = null;

        if (typeof jQuery != 'undefined') {
            $(document).ready(function () {

                var height = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;

                $("#divDocViewer").height(height - 60);
                $('#divSearch').css("display", "none");

                objDoc = <%= ctlDoc.JsObject %>;

                objDoc.ShowPage(1, true);

            });
        }

        function ctlDoc_Copy(text) {
            if (text === "" || text === null) {
                alert("Copy failed");
            }
            else {
                alert(text);
            }
        }

        function ShowMessage(msg) {
            $("#msg").val(msg);
        }

        function ShowLog(msg) {
            $("#log").val(msg);
        }

        function DoSearch() {

            $('#divSearch').css("display", "");

            $("#divSearch").dialog({
                height: 350, width: 500, modal: false, 
                title: 'Document Search',
                resizable: false
            });
        }

        function Zoom(zoomIn) {
            objDoc.Zoom(zoomIn);
        }

        function ToggleCopyMode() {
            var currMode = objDoc.CopyMode();
            objDoc.CopyMode(!currMode);
        }

        function CopyPage() {
            objDoc.CopyPage(objDoc.CurrentPage());
        }

        function ShowFormat() {
            var f = objDoc.FileFormat();
            alert(f);
        }

        function SearchKeyword() {
            if (null != objDoc && $("#kw").val().length > 0) {
                var count = objDoc.Search($("#kw").val(), $("#ext").is(':checked'));

                $("#log").val("");

                if (count > 0) {
                    ShowMessage("Found " + count + " matches!");
                    $("#btnSum").removeAttr("disabled");
                    $("#chkFocus").removeAttr("disabled");

                    $("#btnNext").attr("disabled", "disabled");
                    $("#btnPrevious").attr("disabled", "disabled");
                }
                else {
                    ShowMessage("Nothing found!");
                }
            }
        }

        var resArr = null;
        var counter = -1;

        function SearchSummary() {
            resArr = objDoc.SearchSummary($("#chkFocus").is(':checked'));
            ResetCounter();

            $("#btnNext").removeAttr("disabled");
            $("#btnPrevious").removeAttr("disabled");

            $("#chkFocus").removeAttr("disabled");
        }

        function NewSearch() {
            objDoc.NewSearch();

            $("#kw").val('').focus();
            $("#btnSum").attr("disabled", "disabled");
            $("#btnPrevious").attr("disabled", "disabled");
            $("#btnNext").attr("disabled", "disabled");
            $("#chkFocus").attr("disabled", "disabled");

            ShowMessage("");

            resArr = null;
            ResetCounter();
        }

        function ResetCounter() {
            counter = -1;
            ShowLog("");
        }

        function SearchPage(moveNext) {
            if (null != resArr) {
                if (moveNext) {
                    counter++;
                }
                else {
                    if (counter > 0) {
                        counter--;
                    }
                }

                if (counter > -1 && counter < resArr.length) {
                    var page = resArr[counter][0];
                    var matches = resArr[counter][1];

                    ShowLog("Found " + matches + " instances on page " + page);
                    objDoc.GotoPage(page);
                }
                else {
                    ResetCounter();
                }
            }
        }

    </script>
</body>
</html>

