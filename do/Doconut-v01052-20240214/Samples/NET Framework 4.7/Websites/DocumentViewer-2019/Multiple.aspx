<!DOCTYPE html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Multiple.aspx.cs" Inherits="Multiple" %>
<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Multiple Documents</title>
<style type="text/css">
body {
	font-size: 100%;
}
p {
	margin: 0;
	padding: 0;
}
#tabs_wrapper {
	
}
#tabs_container {
	border-bottom: 1px solid #ccc;
}
#tabs {
	list-style: none;
	padding: 5px 0 4px 0;
	margin: 0 0 0 10px;
	font: 0.75em arial;
}
#tabs li {
	display: inline;
}
#tabs li a {
	border: 1px solid #ccc;
	padding: 4px 6px;
	text-decoration: none;
	background-color: #eeeeee;
	border-bottom: none;
	outline: none;
	border-radius: 5px 5px 0 0;
	-moz-border-radius: 5px 5px 0 0;
	-webkit-border-top-left-radius: 5px;
	-webkit-border-top-right-radius: 5px;
}
#tabs li a:hover {
	background-color: #dddddd;
	padding: 4px 6px;
}
#tabs li.active a {
	border-bottom: 1px solid #fff;
	background-color: #fff;
	padding: 4px 6px 5px 6px;
	border-bottom: none;
}
#tabs li.active a:hover {
	background-color: #eeeeee;
	padding: 4px 6px 5px 6px;
	border-bottom: none;
}

#tabs_content_container {
	border: 1px solid #ccc;
	border-top: none;
	padding: 10px;
	height:600px;
}
.tab_content {
	visibility: hidden;
	height:600px;
	position:absolute;
	margin-right:20px;
}
</style>


</head>
<body>
    <form id="form1" runat="server">
<div id="tabs_wrapper">
	<div id="tabs_container">
		<ul id="tabs">
			<li class="active"><a href="#tab1">PPT Slide</a></li>
			<li><a href="#tab2">Word Document</a></li>
            <li><a href="#tab3">Tiff File</a></li>
            <li><a href="#tab4">XLS File</a></li>
		</ul>
	</div>
	<div id="tabs_content_container">
		<div id="tab1" class="tab_content" style="display: block;">
			<asp:DocViewer ID="ctlDoc1" runat="server" ShowThumbs="true" AutoScrollThumbs="false" Zoom="25" IncludeJQuery="true" ImgFormat="Png" ImageResolution="100" />
		</div>
		<div id="tab2" class="tab_content">
			<asp:DocViewer ID="ctlDoc2" runat="server" ShowThumbs="true" FitType="width" IncludeJQuery="false" ImgFormat="Jpeg" />
		</div>
        <div id="tab3" class="tab_content">
			<asp:DocViewer ID="ctlDoc3" runat="server" ShowThumbs="true" Zoom="50" IncludeJQuery="false" ImgFormat="Gif" />
		</div>
         <div id="tab4" class="tab_content">
			<asp:DocViewer ID="ctlDoc4" runat="server" ShowThumbs="false" IncludeJQuery="false"  ImageResolution="50" />
		</div>
	</div>
</div>

 </form>

<script type="text/javascript">

    $(document).ready(function () {
        $("#tabs li").click(function () {
            //	First remove class "active" from currently active tab
            $("#tabs li").removeClass('active');

            //	Now add class "active" to the selected/clicked tab
            $(this).addClass("active");

            //	Hide all tab content
            $(".tab_content").css('visibility', 'hidden');

            //	Here we get the href value of the selected tab
            var selected_tab = $(this).find("a").attr("href");

            //	Show the selected tab content
            $(selected_tab).css('visibility', 'visible');
            $(selected_tab).css('display', 'block');

            //	At the end, we add return false so that the click on the link is not executed
            return false;
        });

        $("#tab1").css('visibility', 'visible');
    });

</script>

</body>
</html>
