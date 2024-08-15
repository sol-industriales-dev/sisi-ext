<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Password.aspx.cs" Inherits="Full.Password" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Open Password Protected Document - Doconut</title>
    <style type="text/css">
        body {
            font-family: Verdana;
            font-size: 12px;
        }

        .button {
            border: 1px solid #ccc;
            padding: 4px;
            background-color: #F5F6CE;
        }
    </style>
</head>
<body>
    <form id="frmPassword" runat="server">
        <asp:HiddenField ID="hidFileName" runat="server" />

        <asp:FileUpload ID="txtUpload" CssClass="button" runat="server" />&nbsp;&nbsp;<asp:Button
            ID="btnUpload" CssClass="button" runat="server" OnClick="btnUpload_Click" Text="Upload File!" />
        <asp:Panel ID="pnlPassword" runat="server" Visible="false" BorderColor="Gray" BackColor="LightGray"
            BorderWidth="2" Width="250px">
            This document is password protected. Please provide the password to open:<br />
            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox><asp:Button
                ID="btnView" runat="server" Text="Open" OnClick="btnView_Click" />
        </asp:Panel>
        <div id="divDocViewer" runat="server" visible="false">
            <asp:DocViewer ID="ctlDoc" runat="server" Zoom="100" ShowThumbs="true"
                ShowToolTip="true" AutoFocusPage="false" AutoScrollThumbs="false" IncludeJQuery="true"
                ImageResolution="100" ImgFormat="Png" CacheEnabled="false" ToolTipPageText="# " />
        </div>
    </form>
    <script language="javascript" type="text/javascript">

        if (typeof jQuery != 'undefined') {
            $(document).ready(function () {

                if (typeof (<%= ctlDoc.JsObject %>) != 'undefined') {
                    var h = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;
                    var w = "innerWidth" in window ? window.innerWidth : document.documentElement.offsetWidth;

                    $("#divDocViewer").height(h - 40);
                    $("#divDocViewer").width(w - 30);
                }
            });
        }
    </script>
</body>
</html>
