<!DOCTYPE html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Folder.aspx.cs" Inherits="Folder" %>
<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Doconut - Folder Demo</title>
    <style type="text/css">
        body
        {
            font-family:Verdana;
            font-size:12px;
        }
    </style>
</head>
<body>
   <form id="frmFolder" runat="server">
      <div style="display:none"><asp:DocViewer ID="ctlDoc" runat="server" Zoom="75" /></div> <!-- Dummy instance -->
        <asp:DataList ID="lstDocs" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" EnableViewState="False" >
            <ItemTemplate>
              <table cellspacing="5">
                <tr>
                    <td style="width:100%; text-align:center"><a class="thickbox" target="_blank" href="Ajax.aspx?Token=<%# Eval("Token") %>"><img alt="Document Preview" style="border:1px solid #ccc;" src="<%= Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath %>/DocImage.axd?Token=<%# Eval("Token") %>&Page=1&thumb=1&width=200" /><br /><%# Eval("Title") %></a></td>
                </tr>
              </table>                
            </ItemTemplate>
        </asp:DataList>
    </form>
</body>
</html>
