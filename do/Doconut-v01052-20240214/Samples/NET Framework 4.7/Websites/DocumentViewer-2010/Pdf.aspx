<!DOCTYPE html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pdf.aspx.cs" Inherits="Pdf" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Browser Inbuilt - PDF Viewer</title>
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
        }
    </style>
</head>
<body>
 <form id="form1" runat="server">
    This example demonstrates how you can convert the documents on fly to PDF and stream<br />
    them back to the browser. Many platforms now have dedicated PDF viewers, so it works<br />
    great on desktops, tablets and phones out of the box!<br /><br />You need not use this example as to view PDF files. You can do that from the <a href="Viewer.aspx">Viewer.aspx</a> sample.<br /><br />
    <asp:FileUpload ID="txtUpload" CssClass="button" runat="server" />&nbsp;&nbsp;<asp:Button ID="btnUpload" CssClass="button" runat="server" OnClick="Upload_Insert_Click" Text="Upload & View File!" />
    </form>
</body>
</html>
