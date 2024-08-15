<!DOCTYPE html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ajax.aspx.cs" Inherits="Ajax" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AJAX (Update Panel) - Doconut</title>
    <style type="text/css">
        body
        {
            font-family: Verdana;
            font-size: 12px;
        }
        
        .button
        {
            border: 1px solid #ccc;
            padding: 4px;
            background-color: #F5F6CE;
            margin: 5px;
            float: left;
        }
        
        body
        {
            margin: 0px;
            padding: 0px;
        }
        
        #msg
        {
            float: left;
            margin-top: 15px;
        }
    </style>

</head>
<body>
    <form id="frmAjax" runat="server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ctlScriptManager" />
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div id="divTools" style="height: 50px; width: 99%; border: solid 1px #ccc;">
                &nbsp;<asp:Button ID="btnTiff" runat="server" OnClientClick="Wait(true);" Text="TIF" OnClick="View_Click" CssClass="button" />&nbsp;<asp:Button
                    ID="btnPpt" runat="server" OnClientClick="Wait(true);" CssClass="button" Text="PPT" OnClick="View_Click" />&nbsp;<asp:Button
                        ID="btnDoc" CssClass="button" OnClientClick="Wait(true);" runat="server" Text="DOC" OnClick="View_Click" />
                <div id="msg">
                    Please click button to open respective document type</div>
                <div id="ctlTools" style="float: right;margin-top:10px">
                
                    <input type="button" value="Zoom In" onclick="objctlDoc.Zoom(true);" />
                    &nbsp;
                    <input type="button" value="Zoom Out" onclick="objctlDoc.Zoom(false);" />
                </div>
                &nbsp;
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>

     <div id="divDocViewer">
                <asp:DocViewer ID="ctlDoc" runat="server" Zoom="50" ShowThumbs="true" AutoFocusPage="false"
                    AutoScrollThumbs="false" IncludeJQuery="true" ImgFormat="Png"
                    AutoClose="false" />
      </div>
    </form>
    <script language="javascript" type="text/javascript">
    
        function Wait(doWait) {

            if (doWait) {
                $("#msg").text("Loading....");
                $("#divTools :input").css("background-color", "#dcdcdc");
            }
            else {
                $("#msg").text("Please click button to open respective document type");
                $("#divTools :input").css("background-color", "#F5F6CE");
            }

            return true;
        }

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
