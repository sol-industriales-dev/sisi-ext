<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocView.ascx.cs" Inherits="Full.DocView" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>

<div id="divDocViewer">
    <asp:DocViewer ID="ctlDoc" runat="server" />
</div>
<script language="javascript" type="text/javascript">

    var objDoc = null;

    function Resize() {

        var h = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;
        var w = "innerWidth" in window ? window.innerWidth : document.documentElement.offsetWidth;

        $("#divDocViewer").height(h - 25);
        $("#divDocViewer").width(w - 25);

    }

    function InitControl() {

        if (typeof (<%= ctlDoc.JsObject %>) != 'undefined') {
                     objDoc = <%= ctlDoc.JsObject %>;

            Resize();

            $(window).resize(function () {
                Resize();
            });
        }

    }

</script>
