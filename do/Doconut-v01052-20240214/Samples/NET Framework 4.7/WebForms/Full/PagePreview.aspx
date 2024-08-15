<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PagePreview.aspx.cs" Inherits="Full.PagePreview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Page Preview - Doconut</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css" />
    <style>
        .th {
            float: left;
            margin: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="divUpload" class="container-fluid">
            <div class="row">
                <div>
                    <asp:FileUpload ID="txtUpload" CssClass="form-control" runat="server" /><br />
                    <asp:Button ID="btnUpload" CssClass="btn  btn-primary" runat="server" OnClick="btnUpload_Click"
                        Text="Upload & View File!" />
                </div>
            </div>
        </div>
        <br />
        <div id="divThumbs">
        </div>
    </form>

    <script type="text/javascript">

        var divThumbs = $("#divThumbs");
        divThumbs.hide();

        function ShowThumbnails(token, pages) {
            $("#divUpload").hide();
            divThumbs.show();

            for (var iPage = 0; iPage < pages; iPage++) {
                var sThumb = '<img class="th thumbnail" src="PagePreview.aspx?page=' + (iPage + 1) + '&token=' + token + '" /></li>';
                divThumbs.append(sThumb);
            }

        }

    </script>
</body>
</html>
