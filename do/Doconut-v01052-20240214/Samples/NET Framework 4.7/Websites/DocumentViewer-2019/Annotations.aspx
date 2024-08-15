<!DOCTYPE html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Annotations.aspx.cs" Inherits="Annotations" %>

<%@ Register Assembly="DocumentViewer" Namespace="DotnetDaddy.DocumentViewer" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Doconut - Annotations Demo</title>
    <style type="text/css">
        body
        {
            margin: 5px;
            font-family: Verdana;
            font-size: 12px;
        }
        
        #cmbPages
        {
            margin-bottom: 2px;
        }
        
        .menuDiv
        {
            display: inline-block;
            text-align: center;
            padding: 5px;
        }
        
        select
        {
            -moz-appearance: button;
            -webkit-appearance: button;
            border: 1px dotted #766A62;
            background: #fff;
            color: #766A62;
            margin: 0;
            overflow: hidden;
            padding: 2px;
        }
        
        #toolsDiv
        {
            background-color: #F5F6CE;
            padding: 8px;
        }
        
        #toolsDiv a
        {
            margin: 5px;
            border-right: 1px solid #ccc;
            border-bottom: 1px solid #ccc;
            padding-right: 4px;
        }
        
        #divProperties
        {
            display: none;
        }
        
        #toolsDiv input[type=button]
        {
            padding: 3px;
            border: 1px solid #ccc;
            border-radius: 6px;
        }
        
        #toolsDiv a img
        {
            margin-top: 2px;
        }
        
        .button
        {
            border: 1px solid #ccc;
            padding: 4px;
            background-color: #F5F6CE;
        }
        
        .controlset
        {
            width: 90px;
        }
        
        #divFloatProperties
        {
            width: 250px;
            border-radius: 6px;
            border: solid 1px black;
            background-color: #dcdcdc;
            display: none;
            position: absolute;
        }
    </style>
    <link type="text/css" rel="Stylesheet" href="css/custom.css" />
 
</head>
<body>
    <form id="frmMain" runat="server">
    <!-- <<START>> For Mobile Support For Annotations -->
    <script src="js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <!-- <<END>> For Mobile Support For Annotations -->
    <div id="toolsDiv">
        <a href="javascript:void(0);" title="rectangle">
            <img src="images/rectangle.png" /></a>&nbsp; <a href="javascript:void(0);" title="line">
                <img src="images/line.png" /></a>&nbsp; <a href="javascript:void(0);" title="arrow">
                    <img src="images/arrow.png" /></a>&nbsp; <a href="javascript:void(0);" title="triangle">
                        <img src="images/triangle.png" /></a>&nbsp; <a href="javascript:void(0);" title="circle">
                            <img src="images/circle.png" /></a>&nbsp; <a href="javascript:void(0);" title="ellipse">
                                <img src="images/ellipse.png" /></a>&nbsp; <a href="javascript:void(0);" title="note">
                                    <img src="images/note.png" /></a>&nbsp; <a href="javascript:void(0);" title="image">
                                        <img src="images/image.png" /></a>&nbsp; <a href="javascript:void(0);" title="stamp">
                                            <img src="images/stamp.png" /></a>&nbsp; <a href="javascript:void(0);" title="freehand">
                                                <img src="images/freehand.png" /></a>&nbsp;&nbsp;&nbsp;&nbsp;
        <a href="javascript:void(0);" onclick="DeleteAnnot();">
            <img src="images/ann-delete.png" /></a>&nbsp; <a href="javascript:void(0);" onclick="ClearAnnot();">
                <img src="images/ann-clear.png" /></a>&nbsp;|&nbsp;<input type="button" id='btnAnnotationSave'
                    style="background: green; color: White" value="Save Annotations" onclick="CallSave();" />&nbsp;|&nbsp;<input
                        type="button" style="background: orange" value="Close Window" onclick="CallClose();" />
        &nbsp;|&nbsp;<a type="button" onclick="AddRect()" href="javascript:void(0);">Orange</a>&nbsp;|&nbsp;<a
            onclick="AddGoogle()" href="javascript:void(0);">Google</a>&nbsp;|&nbsp;<a onclick="ImageStamp()"
                href="javascript:void(0);">Approved</a> &nbsp;|&nbsp;<a onclick="HideDate()" href="javascript:void(0);">Hide</a>&nbsp;|&nbsp;<input
                    type="button" id='btnPrevious' style="background: #DCDCDC;" value="Previous Page"
                    onclick="CallNext(false);" />&nbsp;|&nbsp;<input type="button" id='btnNext' style="background: #DCDCDC;"
                        value="Next Page" onclick="CallNext(true);" />
    </div>
    <div id="divDocViewer">
        <asp:DocViewer ID="ctlDoc" runat="server" ShowThumbs="true" AutoScrollThumbs="true"
            AutoFocusPage="false" Zoom="50" IncludeJQuery="true" IncludeJQueryUI="true" ShowToolTip="false" ImgFormat="Png"
            FitType="width" ImageResolution="200" FixedZoom="true" />
    </div>
    <br />
    <asp:Button CssClass="button" ID="btnCodeAnn" Text="Annotation By Code" runat="server" OnClick="btnCodeAnn_Click" />&nbsp;&nbsp;&nbsp;<asp:Button CssClass="button" ID="btnExportPDF" Text="Export Document To PDF"
        runat="server" OnClick="SavePDF_Click" />&nbsp;&nbsp;&nbsp;<asp:Button CssClass="button"
            ID="btnExportXML" Text="Export Annotations To XML" runat="server" OnClick="btnExportXML_Click" />&nbsp;&nbsp;
    <div id="div_Debug" style="width: 40%; height: 20px; float: right; border: 1px solid;
        margin-right: 5px; padding: 2px; background-color: #F5F6CE;">
    </div>
    <br />
    <br />
    <asp:FileUpload ID="uploadXml" runat="server" />&nbsp;<asp:Button 
        ID="btnUpload" runat="server" Text="Upload XML" onclick="btnUpload_Click" />
    &nbsp;
    <input type="button" id="btnOpen" class="button" value="Open Annotation Window" onclick="ctlDoc_DoubleClick();" />
    &nbsp;Zoom<select id="cmbAnnZoom">>
        <option value="50">50</option>
        <option selected="selected" value="default">default</option>
        <option value="100">100</option>
    </select>&nbsp;Show buttons<input type="checkbox" id="showButtons" />
    <br />
    <!-- Float properties window start -->
    <div id="divFloatProperties">
        <table cellpadding="4" cellspacing="4" width="100%">
            <tr>
                <td>
                    <img src="images/close.png" style="float: right; cursor: pointer" onclick="$('#divFloatProperties').hide(); $('#hidAnnId').val('');" />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <table style="width: 100%" cellpadding="4" cellspacing="4">
                        <tr id="optBackColor">
                            <td>
                                Color:
                            </td>
                            <td>
                                <div class="controlset">
                                    <input type="text" id="annbackColorFloat" /></div>
                            </td>
                        </tr>
                        <tr id="optBorderColor">
                            <td>
                                Color:
                            </td>
                            <td>
                                <div class="controlset">
                                    <input type="text" id="annborderColorFloat" /></div>
                            </td>
                        </tr>
                        <tr id="optRotate">
                            <td>
                                Rotate:
                            </td>
                            <td>
                                <input type="text" id="annRotateFloat" />
                            </td>
                        </tr>
                        <tr id="optArrow">
                            <td>
                                Direction:
                            </td>
                            <td>
                                <select id="annArrowDirection">
                                    <option value="N">N</option>
                                    <option selected="selected" value="NE">NE</option>
                                    <option value="E">E</option>
                                    <option value="SE">SE</option>
                                    <option value="S">S</option>
                                    <option value="SW">SW</option>
                                    <option value="W">W</option>
                                    <option value="NW">NW</option>
                                </select>
                            </td>
                        </tr>
                        <tr id="optLine">
                            <td>
                                Vertical:
                            </td>
                            <td>
                                <input type="checkbox" id="annLineDirection" />
                            </td>
                        </tr>
                        <tr id="optBorderWidth">
                            <td>
                                Thickness:
                            </td>
                            <td>
                                <div class="controlset">
                                    <input type="text" id="annBorderWidthFloat" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <input type="hidden" id="hidAnnId" />
                            </td>
                        </tr>
                        <tr id="optImages">
                            <td>
                                Url:
                            </td>
                            <td>
                                <input type="text" id="annImageUrl" />
                            </td>
                        </tr>
                        <tr id="optNotes">
                            <td>
                                Text:
                            </td>
                            <td>
                                <input type="text" id="annNotesText" />
                            </td>
                        </tr>
                        <tr id="optTextColor">
                            <td>
                                Text Color:
                            </td>
                            <td>
                                <input type="text" id="annTextColor" />
                            </td>
                        </tr>
                        <tr id="optTextSize">
                            <td>
                                Text Size:
                            </td>
                            <td>
                                <input type="text" id="annTextSize" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <input type="button" value="Save" style="background-color: Green; color: White;"
                        onclick="SavePropertiesFloating()" />&nbsp;
                    <input type="button" value="More..." style="background-color: Orange; color: Black;"
                        onclick="$('#divFloatProperties').hide(); ctlDoc_Properties();" />
                </td>
            </tr>
        </table>
    </div>
    <!-- Float properties window end -->
    <!-- Annotation properties window start -->
    <div id="divProperties">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                Back color:
                            </td>
                            <td>
                                <div class="controlset">
                                    <input type="text" id="annbackColor" /><input id="color1" type="text" name="color1"
                                        value="#FFFFFF" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Border&nbsp;color:
                            </td>
                            <td>
                                <div class="controlset">
                                    <input type="text" id="annborderColor" /><input id="color2" type="text" name="color2"
                                        value="#000000" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Border&nbsp;width:
                            </td>
                            <td>
                                <input type="text" id="annborderWidth" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Show&nbsp;Border:
                            </td>
                            <td>
                                <input type="text" id="annshowBorder" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Opacity:
                            </td>
                            <td>
                                <input type="text" id="annOpactiy" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Rotate:
                            </td>
                            <td>
                                <input type="text" id="annRotate" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Can Rotate:
                            </td>
                            <td>
                                <input type="text" id="annCanRotate" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                Title:
                            </td>
                            <td>
                                <input type="text" id="annTitle" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Show Title:
                            </td>
                            <td>
                                <input type="text" id="annshowTitle" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Title color:
                            </td>
                            <td>
                                <input type="text" id="anntitleColor" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Title Font Size:
                            </td>
                            <td>
                                <input type="text" id="anntitleFontSize" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Note/Url:
                            </td>
                            <td>
                                <input type="text" id="annNote" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Show Note:
                            </td>
                            <td>
                                <input type="text" id="annshowNote" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Text align:
                            </td>
                            <td>
                                <input type="text" id="anntextAlign" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <b>Arrow</b> Direction:
                            </td>
                            <td>
                                <input type="text" id="annarrowDirection" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Line</b> Vertical:
                            </td>
                            <td>
                                <input type="text" id="annlineVertical" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Burn:
                            </td>
                            <td>
                                <input type="text" id="annBurn" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Locked:
                            </td>
                            <td>
                                <input type="text" id="annLocked" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                z-Index:
                            </td>
                            <td>
                                <input type="text" id="annzindex" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Author:
                            </td>
                            <td>
                                <input type="text" id="annAuthor" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table>
                        <tr>
                            <td>
                                <b>Annotations</b><br />
                                <select id="lstAnnotations" size="2" name="annotations" style="height: 150px; width: 90px">
                                </select>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <input type="button" id="btnApply" onclick="SaveAnnotationData();" value="Save Properties" />
        <!-- If you want to save it as soon as possible. Add following after above function call
          var boolSaved = objDoc.SaveAnnotations();
         -->
        <br />
    </div>
    <!-- Annotation properties window end
    -->
    </form>
    <script language="javascript" type="text/javascript">

        var objDoc = null;

        var annotationsSaved = false;
        var annotationsChanged = false;

        function ShowDebug(msg)
        {
            document.getElementById('div_Debug').innerHTML = "&nbsp;>>&nbsp;" + msg + " ...";
        }

        if (typeof jQuery != 'undefined') 
        {
            $(document).ready(function () 
            {

                var height = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;

                $("#divDocViewer").height(height - 100);
                $('#divProperties').css("display", "none");

                $('#lstAnnotations').click(function () {

                    var selected = $(this).find(':selected').text();
                    objDoc.AnnotationController().SelectAnnotation(selected);
                    LoadAnnotationData();

                });

                $('#color1').colorPicker();
                $('#color2').colorPicker();

                 $('#color1').change(function () {
                    $('#annbackColor').val($('#color1').val());
                });

                $('#color2').change(function () {
                    $('#annborderColor').val($('#color2').val());
                });

                objDoc = <%= ctlDoc.JsObject %>;
              
                if(typeof(objDoc) != 'undefined')
                {
                   // comment this if you don't want to open the annotation window when page loads
                   ctlDoc_DoubleClick();

                   // add any default stamp annotation on it or change language for buttons (if visible)
                   // $(window).load(function () { AddStamp(); ChangeLang(); }); 
                   // or use
                   // setTimeout(function () { AddStamp(); ChangeLang(); }, 2000);
                }

                $('#toolsDiv').children('a').bind('click', function(){ if(this.title.length > 0) SelectAnnType(this.title) });

            });
        }

        function ctlDoc_DoubleClick()
        {          
          var zoom = objDoc.CurrentZoom();
          if($('#cmbAnnZoom').val() != 'default')
          {
            zoom = parseInt($('#cmbAnnZoom').val() );  
          }          

          objDoc.ShowAnnotations(zoom, $('#showButtons').is(':checked'));

          $('#toolsDiv').show();

          ShowDebug("Select Annotation, Drag and Draw! Double click object to change its properties");

          // Hide thumbnails and splitter if required

           objDoc.HideSplitter(true);
           objDoc.HideThumbs(true);

          setTimeout(function () { ChangeLang(); }, 500);
        }

        /* Annotation functions */


    function CallSave()
    {
        var btnSave = $('#btnAnnotationSave');
        var didSave = objDoc.SaveAnnotations(); 
        
        if(didSave)
        {
           SetDirty(false);
           annotationsSaved = true;

           btnSave.css('color','white');
        }        
    }

    function CopyAnnotations()
    {
       var currAnnotations = objDoc.GetAnnotations();
       if(currAnnotations !== null)
       {
          var success = objDoc.PushAnnotations(2, currAnnotations);
       }
    }

    function CallClose()
    {
        if(annotationsChanged == true && annotationsSaved == false)
        {
            var doExit = confirm('Exit without saving?');
            if(doExit == true)
            {
                objDoc.CloseAnnotations(false);
                 return true;
            }
        }
        else
        {
           objDoc.CloseAnnotations(annotationsSaved);
            return true;
        } 
        
        return false;       
    }

    function CallNext(goNext)
    {
       if(CallClose())
       {
          objDoc.Next(goNext);
          ctlDoc_DoubleClick();

          annotationsSaved = false;
          SetDirty(false);
       }
    }

       function SelectAnnType(type) {
            if (null != objDoc.AnnotationController()) {
                objDoc.AnnotationController().annotationType = type;
            }
        }

        function DeleteAnnot()
        {
          if (null != objDoc.AnnotationController()) {
            objDoc.AnnotationController().DeleteAnnotation(objDoc.AnnotationController().GetSelectedAnnotation());
          }
        }

        function ClearAnnot()
        {
          if (null != objDoc.AnnotationController()) {
            objDoc.AnnotationController().ClearAnnotations();
          }
        }

        /*
         function ctlDoc_OnViewerReady() 
         {
            setTimeout(function () { 
                if (null != objDoc.AnnotationController()) 
                { 
                    objDoc.AnnotationController().annotationType = 'circle';
                 }
                }
            ,1000);
         } 
        */

        /* Annotation Callback functions */

        function ctlDoc_Selected()   // or use as: function <%= ctlDoc.ClientID %>_Selected()
        {
           // Do something when any annotation is selected
        }

        function ctlDoc_Created()
        {
          // Do something when any annotation is created
          var annObj = objDoc.AnnotationController().GetAnnotationById(objDoc.AnnotationController().GetSelectedAnnotation());

          annObj.SetAuthor('Admin');

          // you can change color as element is created

          //annObj.SetBackColor('orange');
          //annObj.Paint();


          if($("#lstAnnotations option[value='" + annObj.annId + "']").length == 0)
          {
            $("#lstAnnotations").append($('<option></option>').attr('value', annObj.annId).text(annObj.annId));
          }


          //create a custom properties box (optional)

           var btnProp = $('<img/>').attr('src','images/properties.png').css({ float: 'left', position: 'absolute', left: '0px', top: '0px', 'z-index': '999'}).bind('click', function() { ShowFloatingProperties(annObj.annId);});
           btnProp.hide();

           $("#" + annObj.annId).append(btnProp).bind('mouseover', function() { btnProp.show(); } ).bind('mouseout', function() { btnProp.hide(); } ).bind('mouseup', function() { PositionFloatProperties(annObj.annId);});

           // custom properties box ends
           


           SetDirty(true);
        }

        function SetDirty(isDirty)
        {
           if(isDirty)
           {
             $('#btnAnnotationSave').css('color','orange');
             annotationsSaved = !isDirty;
           }
           else
           {
             $('#btnAnnotationSave').css('color','white');
           }

           annotationsChanged = isDirty;          
        }

        function ctlDoc_Deleted()
        {
         // Do something when any annotation is deleted
         ShowDebug(objDoc.AnnotationController().GetSelectedAnnotation() + " was deleted");

          $("#lstAnnotations option[value='" + objDoc.AnnotationController().GetSelectedAnnotation() + "']").remove();

           ClearAnnotationData();

           SetDirty(true);
        }

        function ctlDoc_Changed()
        {
          // Do something when any annotation is changed / modified
          ShowDebug(objDoc.AnnotationController().GetSelectedAnnotation() + " was modified");

          SetDirty(true);
        }

        function ctlDoc_Properties()
        {
           // Do something when any annotation is double clicked
           LoadAnnotationData();

            $('#divProperties').css("display", "");

            $("#divProperties").dialog({
                height: 300, width: 570, modal: true,
                title: 'Annotation Properties',
                resizable: false
            });
        }

        function ctlDoc_AnnClosed()
        {
            ShowDebug('Annotation window closed. Double Click the viewer to open Annotation window again!');
            $('#toolsDiv').hide();
            $("#divFloatProperties").hide();

            // Show thumbnails and splitter
            objDoc.HideSplitter(false);
            objDoc.HideThumbs(false);

            SetDirty(false);
        }

        function ctlDoc_AnnLoaded()
        {
           ShowDebug('Finished loading annotations.');
           SetDirty(false);
        }

        function ctlDoc_AnnSaved()
        {
            ShowDebug('Annotation were saved!');
        }

        function ctlDoc_AnnSaveError()
        {
            ShowDebug('Annotation were NOT saved !');
        }


        function ShowFloatingProperties(element)
        {
            var ann = objDoc.AnnotationController().GetAnnotationById(element);

            // hide all options

            $('#optImages').hide();
            $('#optNotes').hide();
            $('#optTextColor').hide();  
            $('#optTextSize').hide(); 
             
            $('#optBorderColor').hide(); 
            $('#optArrow').hide(); 
            $('#optLine').hide(); 
            $('#optBorderWidth').hide(); 


            $('#optRotate').show(); 
            $('#optBackColor').show(); 

            if (null != ann) 
            {

                // custom logic based on the type of annotation, to show or hide certain properties
                switch(ann.annType) {
                    case 'rectangle':
                        break;
                    case 'image':
                        $('#optImages').show();
                        break;
                    case 'note':
                        $('#optNotes').show();
                        $('#optTextColor').show();  
                        $('#optTextSize').show(); 
                        $('#optRotate').hide(); 

                        setTimeout(function () { document.getElementById('annNotesText').focus(); }, 500);

                        break;
                    case 'line':
                        $('#optRotate').hide(); 
                        $('#optBackColor').hide(); 

                        $('#optBorderColor').show(); 
                        $('#optLine').show(); 
                        $('#optBorderWidth').show(); 

                        $("#annLineDirection").val(ann.GetLineVertical());

                        break;
                    case 'arrow':
                        $('#optRotate').hide(); 
                        $('#optBackColor').hide(); 

                        $('#optBorderColor').show(); 
                        $('#optArrow').show(); 
                        $('#optBorderWidth').show(); 

                        $("#annArrowDirection").val(ann.GetArrowDirection());

                        break;
                   case 'stamp':
                        $('#optBackColor').hide(); 
                        $('#optBorderColor').show(); 
                        $('#optBorderWidth').show(); 

                        break;

                    case 'freehand':
                        $('#optRotate').hide(); 
                        $('#optBackColor').hide(); 

                        $('#optBorderColor').show(); 
                        $('#optBorderWidth').show(); 

                        break;

                    case 'circle':
                    case 'ellipse':
                    case 'triangle':
                    case 'rectangle':

                        $('#optRotate').hide(); 

                        break;
                } 


                $("#hidAnnId").val(element);


                $("#annImageUrl").val(ann.GetNote());
                $("#annRotateFloat").val(ann.GetRotate());
                $("#annbackColorFloat").val(ann.GetBackColor());
                

                $("#annNotesText").val(ann.GetNote());
                $("#annTextColor").val(ann.GetTitleColor());
                $("#annTextSize").val(ann.GetTitleFontSize());
               
                $("#annborderColorFloat").val(ann.GetBorderColor());
                $("#annBorderWidthFloat").val(ann.GetBorderWidth());

                PositionFloatProperties(element);
                

                $("#divFloatProperties").show();
   
            }
        }

        function SavePropertiesFloating()
        {
            var ann =  objDoc.AnnotationController().GetAnnotationById($("#hidAnnId").val());

            if (null != ann) {

                ann.SetBackColor($('#annbackColorFloat').val());
                ann.SetRotate($('#annRotateFloat').val());

                ann.SetNote($('#annNotesText').val());
                ann.SetTitleColor($('#annTextColor').val());
                ann.SetTitleFontSize($('#annTextSize').val());

                ann.SetBorderColor($('#annborderColorFloat').val());
                ann.SetBorderWidth($('#annBorderWidthFloat').val());

                switch(ann.annType) {
                    case 'line':
                         ann.SetLineVertical($('#annLineDirection').is(":checked"));
                    break;
                    case 'arrow':
                        ann.SetArrowDirection($('#annArrowDirection').val());
                    break;
                }

                ann.Paint();

                SetDirty(true);
            }
        }

        function PositionFloatProperties(element)
        {
            if(element != $("#hidAnnId").val())
            {
                return;
            }

            var ann = objDoc.AnnotationController().GetAnnotationById(element);
               
            if (null != ann) 
            {
                $("#divFloatProperties").css({left: ( parseInt(ann.GetLeft()) + parseInt(ann.GetWidth())) , top: ann.GetTop() + ann.GetHeight() / 2});
            }
        }

        function ChangeLang()
        {
            // here you can change any properties of the buttons (if visible)
            $('.btnAnnSave').val('Save Annotations').show('fast');
            $('.btnAnnCancel').val('Return To Viewer').show('fast');
            
        }

         function LoadAnnotationData() {
            var ann = objDoc.AnnotationController().GetAnnotationById(objDoc.AnnotationController().GetSelectedAnnotation());

            if (null != ann) {
                $('#annlineVertical').val("false");
                $('#annarrowDirection').val("");

                $('#annId').val(ann.annId);
                $('#annType').val(ann.annType);

                $('#annWidth').val(ann.GetWidth());
                $('#annHeight').val(ann.GetHeight());

                $('#annborderColor').val(ann.GetBorderColor());
                $('#annbackColor').val(ann.GetBackColor());

                $('#color1_color_picker').css("background-color", ann.GetBackColor());
                $('#color2_color_picker').css("background-color", ann.GetBorderColor());

                $('#color1').val(ann.GetBackColor());
                $('#color2').val(ann.GetBorderColor());

                $('#annborderWidth').val(ann.GetBorderWidth());

                $('#annOpactiy').val(ann.GetOpacity());
                $('#annborderWidth').val(ann.GetBorderWidth());
                $('#annshowBorder').val(ann.GetShowBorder());

                $('#annRotate').val(ann.GetRotate());
                $('#annCanRotate').val(ann.GetCanRotate());

                $('#annTitle').val(ann.GetTitle());
                $('#annshowTitle').val(ann.GetShowTitle());

                $('#anntitleColor').val(ann.GetTitleColor());
                $('#anntitleFontSize').val(ann.GetTitleFontSize());

                $('#annNote').val(ann.GetNote());
                $('#annshowNote').val(ann.GetShowNote());

                $('#annBurn').val(ann.GetBurn());
                $('#annLocked').val(ann.GetLocked());

                $('#annzindex').val(ann.GetzIndex());
                $('#annBase64').val(Base64.encode(ann.toString()));

                $('#anntextAlign').val(ann.GetTextAlign());

                $('#annAuthor').val(ann.GetAuthor());

                switch (ann.annType) {

                    case "line":
                        $('#annlineVertical').val(ann.GetLineVertical());
                        break;
                    case "arrow":
                        $('#annarrowDirection').val(ann.GetArrowDirection());
                        break;
                }
            }

            $("#lstAnnotations").val(objDoc.AnnotationController().GetSelectedAnnotation());
        }

         function SaveAnnotationData() {

            if (frmvalidator.ValidateAll()) {

                var ann = objDoc.AnnotationController().GetAnnotationById(objDoc.AnnotationController().GetSelectedAnnotation());

                if (null != ann) {
                    ann.SetBorderColor($('#annborderColor').val());
                    ann.SetBackColor($('#annbackColor').val());
                    ann.SetBorderWidth($('#annborderWidth').val());

                    ann.SetOpacity($('#annOpactiy').val());
                    ann.SetShowBorder($('#annshowBorder').val().toString().toLowerCase() == 'true');

                    ann.SetCanRotate($('#annCanRotate').val().toString().toLowerCase() == 'true');
                    ann.SetRotate($('#annRotate').val());

                    ann.SetShowTitle($('#annshowTitle').val().toString().toLowerCase() == 'true');
                    ann.SetTitle($('#annTitle').val());

                    ann.SetTitleColor($('#anntitleColor').val());
                    ann.SetTitleFontSize($('#anntitleFontSize').val());

                    ann.SetNote($('#annNote').val());
                    ann.SetShowNote($('#annshowNote').val().toString().toLowerCase() == 'true');

                    ann.SetBurn($('#annBurn').val().toString().toLowerCase() == 'true');
                    ann.SetLocked($('#annLocked').val().toString().toLowerCase() == 'true');
                    ann.SetzIndex(parseInt($('#annzindex').val()));
                    ann.SetAuthor($('#annAuthor').val());

                    switch (ann.annType) {

                        case "line":
                            ann.SetLineVertical($('#annlineVertical').val().toString().toLowerCase() == 'true');
                            break;
                        case "arrow":
                            ann.SetArrowDirection($('#annarrowDirection').val());
                            break;
                        case "note":
                            ann.SetTextAlign($('#anntextAlign').val());
                            break;
                    }

                    ann.Paint();
                }

                $('#divProperties').css("display", "none");
                $('#divProperties').dialog('close');

                SetDirty(true);

            }
        }

        function ClearAnnotationData(ann) {

            $('#annlineVertical').val("");
            $('#annarrowDirection').val("");

            $('#annId').val("");
            $('#annType').val("");
            $('#annLeft').val("");
            $('#annTop').val("");
            $('#annWidth').val("");
            $('#annHeight').val("");

            $('#annborderColor').val("");
            $('#annbackColor').val("");
            $('#annborderWidth').val("");

            $('#annOpactiy').val("");
            $('#annborderWidth').val("");
            $('#annshowBorder').val("");

            $('#annRotate').val("");
            $('#annCanRotate').val("");

            $('#annTitle').val("");
            $('#annshowTitle').val("");

            $('#anntitleColor').val("");
            $('#anntitleFontSize').val("");

            $('#annNote').val("");
            $('#annshowNote').val("");

            $('#annBurn').val("");
            $('#annLocked').val("");

            $('#annzindex').val("");
            $('#annBase64').val("");

            $('#anntextAlign').val("");
            $('#annAuthor').val("");
        }

        /* Custom Annotation */

        function AddRect() {
            if (null != objDoc.AnnotationController()) {
                var objRect = new RectangleAnnotation({ left: 10, top: 10, width: 300, height: 100, backColor: 'orange', opacity: 70 });
                objDoc.AnnotationController().AddAnnotation(null, objRect, null);
            }
        }

        function AddGoogle() {
            if (null != objDoc.AnnotationController()) {
                var objImage = new ImageAnnotation({ left: 100, top: 100, width: 250, height: 100});
                objDoc.AnnotationController().AddAnnotation(null, objImage, null);

                objImage.SetNote('http://www.google.com/images/srpr/logo11w.png');
                objImage.Paint();
            }
        }

        function AddEllipse() {
            if (null != objDoc.AnnotationController()) {
                var objEllipse = new EllipseAnnotation({ left: 10, top: 200, width: 200, borderWidth: 4, height: 100, borderColor: 'black', backColor: 'red' });
                objDoc.AnnotationController().AddAnnotation(null, objEllipse, null);
            }
        }

       function AddStamp() {
            if (null != objDoc.AnnotationController()) {
                var objStamp = new StampAnnotation({ left: 350, top: 300, width: 300, height: 100, rotate: -15, title: 'Hello World !', borderWidth: 4,borderColor: 'red' });
                objDoc.AnnotationController().AddAnnotation(null, objStamp, null);
                ShowDebug("Stamp added"); 
            }
        }

        function ImageStamp() {
         if (null != objDoc.AnnotationController()) {
                var objStampImage = new ImageAnnotation({ left: 50, top: 50, width: 260, height: 55});
                objDoc.AnnotationController().AddAnnotation(null, objStampImage, null);

                objStampImage.SetNote('images/Approved_Stamp.png');
                objStampImage.Paint();
            }
            
        }

        function HideDate()
        {
             if (null != objDoc.AnnotationController()) {
                var objHide = new RectangleAnnotation({ left: 692, top: 311, width: 231, height: 42, backColor: 'black', opacity: 100, locked: true });
                objDoc.AnnotationController().AddAnnotation(null, objHide, null);
            }
        }

       var frmvalidator = null;

       if (typeof jQuery != 'undefined')
        {
            $(document).ready(function () {

         // Annotation properties Validations 

            frmvalidator = new Validator();

            frmvalidator.addValidation("annOpactiy", "req", "Please enter opacity");
            frmvalidator.addValidation("annOpactiy", "num", "Please enter a valid opacity 0 to 100");
            frmvalidator.addValidation("annOpactiy", "lessthan=101", "Please enter a valid opacity 0 to 100");
            frmvalidator.addValidation("annOpactiy", "greaterthan=-1", "Please enter a valid opacity 0 to 100");

            frmvalidator.addValidation("annbackColor", "req", "Please enter background color. You can use transparent i.e #000000FF");
            frmvalidator.addValidation("annborderColor", "req", "Please enter border color");

            frmvalidator.addValidation("annborderWidth", "req", "Please enter border width");
            frmvalidator.addValidation("annborderWidth", "num", "Please enter a valid border width");

            frmvalidator.addValidation("annshowBorder", "bool", "Please enter true or false for show border");

            frmvalidator.addValidation("annRotate", "req", "Please enter rotate angle");
            frmvalidator.addValidation("annRotate", "num", "Please enter a valid rotate angle");

            frmvalidator.addValidation("annCanRotate", "bool", "Please enter true or false for show border");
            frmvalidator.addValidation("annshowTitle", "bool", "Please enter true or false for show title");
            frmvalidator.addValidation("annshowNote", "bool", "Please enter true or false for show note");
            frmvalidator.addValidation("annlineVertical", "bool", "Please enter true or false for line vertical");
            frmvalidator.addValidation("annBurn", "bool", "Please enter true or false for burn annotation");
            frmvalidator.addValidation("annLocked", "bool", "Please enter true or false for lock annotation");

            frmvalidator.addValidation("anntitleFontSize", "req", "Please enter title font size");
            frmvalidator.addValidation("anntitleFontSize", "num", "Please enter a valid title font size");

            frmvalidator.addValidation("annzindex", "req", "Please enter zIndex");
            frmvalidator.addValidation("annzindex", "num", "Please enter a valid zIndex");

            });
        }

    </script>
    <!-- Annotation Properties Window Related Starts -->
    <link rel="stylesheet" href="css/thickbox.css" type="text/css" />
    <script type="text/javascript" src="js/thickbox-compressed.js"></script>
    <script type="text/javascript" src="js/gen_validatorv2.js"></script>
    <script type="text/javascript" src="js/jquery.colorPicker.js"></script>
    <!-- Annotation Properties Window Related Ends -->
</body>
</html>
