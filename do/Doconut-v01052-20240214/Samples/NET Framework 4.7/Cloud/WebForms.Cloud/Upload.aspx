<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Upload.aspx.cs" Inherits="Upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Doconut - Upload Document To Cloud</title>
</head>
<body>
    <form id="frmUpload" runat="server">
        <div>
             <asp:Button ID="btnUpload" Text="Start Cloud Upload" runat="server" OnClick="btnUpload_Click" />
        </div>
        <h3>For demo purpose just click above button, it will show the existing CDN demo document</h3>
    </form>
</body>
</html>
