<!DOCTYPE html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="iFrame.aspx.cs" Inherits="iFrame" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>iFrame - Doconut</title>
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.9.0.min.js"></script>
    <style type="text/css">
        body
        {
            margin:0px;
            padding:0px;
        }
    </style>
</head>
<body>
    <form id="frmIframe" runat="server">
    <iframe src="Mobile.aspx" id="docframe" frameborder="0" scrolling="no" width="100%" ></iframe>
    </form>
    <script language="javascript" type="text/javascript">
        $(window).on('load', function () {
            var height = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;
            $("#docframe").height(height - 5); // to avoid scroll-bar
        });

    </script>
</body>
</html>
