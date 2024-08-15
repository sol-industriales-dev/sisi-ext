<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Documents.aspx.cs" Inherits="Documents"
    EnableViewState="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Document View - Doconut</title>
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1, maximum-scale=1">

    <!-- jQuery and Bootstrap -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css" />
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"
        type="text/javascript"></script>
    <!-- DOCONUT CONTROL SCRIPTS -->
    <%= ViewState["ViewerScripts"] %>
    <!-- DOCONUT CONTROL CSS -->
    <%= ViewState["ViewerCSS"]%>
    <style type="text/css">
        #divDocViewer
        {
            width: 100%;
            border-radius: 6px;
            background-color: whitesmoke;
            border: 1px grey;
        }
        
        .modal-dialog
        {
            width: 100%;
            height: 100%;
            margin: 0;
            padding: 0;
        }
        
        .modal-content
        {
            height: auto;
            min-height: 100%;
            border-radius: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-sm-12">
                <h1>
                    Click Button To View</h1>
                <input type="button" id="btnDoc" class="btn btn-primary btn-lg" data-token="" data-doc="Sample.doc"
                    value="DOCX" />&nbsp;
                <input type="button" id="btnPdf" class="btn btn-info btn-lg" data-token="" data-doc="Sample.pdf"
                    value="PDF" />&nbsp;
                <input type="button" id="btnPpt" class="btn btn-warning btn-lg" data-token="" data-doc="Sample.ppt"
                    value="PPT" />&nbsp;
            </div>
        </div>
    </div>
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div class="pull-left"><a href="javascript:void(0);" onclick="ZoomIn();" class="btn btn-primary btn-sm">&nbsp;Zoom-In&nbsp;</a>&nbsp;&nbsp;<a href="javascript:void(0);" onclick="ZoomOut();" class="btn btn-info btn-sm">Zoom-Out</a></div>
                    <h4 id="msg" class="hidden-xs modal-title pull-right">View Document&nbsp;</h4>
                </div>
                <div class="modal-body">
                    <div id="divDocViewer">
                        <!-- Viewer client ID -->
                        <div id="div_<%= ViewState["ViewerID"] %>">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript">
                /* Viewer JS Object */
                var <%= ViewState["ViewerObject"] %> = null;
               
                  /* Init code for viewer */
                 <%= ViewState["ViewerInit"] %>

                  var jsDoc = <%= ViewState["ViewerObject"] %>;

                function Resize() {
                    var h = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;
                    $("#divDocViewer").height(h - 160); // adjust value as required
                }

                $(window).on("resize", function () {
                    Resize();
                });

                function ViewDocument(btn, token)
                {
                   var bToken = btn.attr("data-token");
                   var dFile = btn.attr("data-doc");
                   var tokenToView = "";

                   /* document was already viewed */
                   if(bToken.length > 0){
                      tokenToView = bToken;   
                   }
                   else
                   {
                     /* get the token */

                         $.ajax({
                            type: "POST",
                            cache: false,
                            async: false,
                            url: "Documents.aspx?token=yes&file=" + dFile,
                            success: function (data) { 
                                tokenToView = data;  // store token for print, etc
                                btn.attr("data-token", tokenToView); // set the data-token attr                               
                            },
                            error: function (textStatus, errorThrown, data) {
                                tokenToView = "";
                                alert("Unable to open document. Error: " + data.responseText);
                            }
                        });

                   }
                   
                  
                   if(tokenToView.length > 0)
                   {
                        jsDoc.View(tokenToView);
                        $('#myModal').modal('show');
                        btn.removeAttr("disabled");
                   }

                   Resize();
                }

                function ZoomIn()
                {
                   jsDoc.Zoom(true);
                }

                function ZoomOut()
                {
                    jsDoc.Zoom(false);
                }

                 $('#myModal').on('shown.bs.modal', function () {
                    // Refresh the viewer
                    jsDoc.Refit(); 
                    jsDoc.HideThumbs(false);
                    jsDoc.GotoPage(1);
                });


                 /* View document, bind button's onclick event */
                $("input").each(
                function() 
                {  
                    var ele = $(this); 
                    ele.on("click", function() { 
                       ele.attr("disabled", "disabled");
                       ele.css('color','lime');
                       ViewDocument(ele);
                    }); 
                });
    </script>
</body>
</html>
