<!DOCTYPE html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attachments.aspx.cs" Inherits="Attachments" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Attachments - Doconut</title>
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.9.0.min.js"></script>
    <link rel="stylesheet" href="css/thickbox.css" type="text/css" />
    <script type="text/javascript" src="js/thickbox-compressed.js"></script>

    <style type="text/css">
        body
        {
            margin: 10px;
            padding: 10px;
            font-family: Tahoma;
            font-size: 14px;
        }
        
        #email
        {
            width: 99%;
            display: block;
            border: solid 1px #79B933;
            padding: 10px;
        }
        
        #email textarea
        {
            margin: 20px;
            display: block;
            width: 95%;
        }
        
        #attachments
        {
            border: dashed 1px #79B933;
            padding: 10px;
            margin-top: 10px;
            display: inline-block;
            width: 100%;
        }
        
        #attachments .file
        {
            float: left;
            background-color: #ccc;
            margin: 10px;
            width: 100px;
            height: 100px;
        }
        
        .file a
        {
            text-align: center;
            vertical-align: middle;
            width: 100%;
            height: 100%;
            display: block;
        }
        
        #TB_window
        {
            top: 1%;
        }
        
        #TB_iframeContent
        {
            overflow:hidden;
        }
        
        #divDocViewer
        {
            height:740px;
            width:1000px;    
        }
    </style>
</head>
<body>
    <form id="frmAttachments" runat="server">
    <div id="email">
        <b>Sender:</b>&nbsp;admin@doconut.com<br />
        <b>To:</b>&nbsp;You!
        <br />
        <textarea cols="20" rows="15">

The .MSG/.EML is the file to view primarily “Email body text or HTML”. There could be let’s say 10 attachments of various types PDF, IMAGES, DOC etc. How are they all supposed to be viewed at once along with the email body?

The solution is this, please see :  http://www.codeproject.com/Articles/32899/Reading-an-Outlook-MSG-File-in-C  or  http://www.codeproject.com/Tips/712072/Reading-an-Outlook-MSG-File-in-Csharp  (Also read the user comments at bottom of these pages)

1.	You will need to extract each attachment files first!
2.	Have the files somewhere on the server / folder etc. where the .MSG is
3.	Now show the main MSG in viewer (just like this)
4.	Below it have a list of all attachment names (just as this page does for "Resume.doc")
5.	User can now click and individually view attachments (if format is supported), like Gmail !

Thanks!
</textarea>
    
    <b>1 Attachment</b>&nbsp;(click to view)
    <div id="attachments">
        <div class="file">
            <a title="Resume" class="thickbox" href="Attachments.aspx?File=Sample.doc&TB_iframe=false&height=768&width=1024&KeepThis=true">
                Sample.doc</a>
        </div>
    </div>
    </div>
     <div id="divDocViewer">
        <asp:DocViewer ID="ctlDoc" runat="server" ShowThumbs="false" AutoFocusPage="false" AutoScrollThumbs="false" IncludeJQuery="false" ImageResolution="100" ImgFormat="Png" />
     </div>
    </form>

    <script language="javascript" type="text/javascript">
        if (self != top) {

            $(document).ready(function () { $("#divDocViewer").show(); $("#email").hide(); });

        }
        else {
            $("#divDocViewer").hide();
        }
    </script>
</body>
</html>
