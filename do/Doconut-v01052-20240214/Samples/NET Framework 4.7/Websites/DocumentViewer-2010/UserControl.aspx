<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserControl.aspx.cs" Inherits="UserControl" %>
<%@ Register TagName="Doc" Src="~/DocView.ascx" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Control - Doconut</title>
</head>
<body>
    <form id="frmMain" runat="server">
      <cc1:Doc ID="ctlViewer" runat="server" />
    </form>
</body>
</html>
