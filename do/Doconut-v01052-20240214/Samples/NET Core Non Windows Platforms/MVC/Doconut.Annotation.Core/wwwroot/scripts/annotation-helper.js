
// GLOBAL

var annotationsSaved = false;
var annotationsChanged = false;

$('#menuTools').find('a').bind('click', function () { if (this.title.length > 0) SelectAnnType(this.title); });


function SetDirty(isDirty) {
    if (isDirty) {
        annotationsSaved = !isDirty;
    }

    annotationsChanged = isDirty;
}

function SelectAnnType(type) {
    if (null != objctlDoc.AnnotationController()) {
        objctlDoc.AnnotationController().annotationType = type;
    }
}

function OpenAnnotation() {
    if (globalToken === "") {
        alert("Please open a document first. Then click Annotate.");
        return;
    }

    var zoom = objctlDoc.CurrentZoom();
    objctlDoc.ShowAnnotations(parseInt(zoom), false);


    $("#navDefault").hide();
    $("#navAnnotation").show();
}

function CloseAnnotation() {

    var returnClose = false;

    if (annotationsChanged === true && annotationsSaved === false) {
        var doExit = confirm('Exit without saving?');
        if (doExit === true) {
            objctlDoc.CloseAnnotations(false);
            returnClose = true;
        }
    }
    else {
        objctlDoc.CloseAnnotations(annotationsSaved);
        returnClose = true;
    }

    return returnClose;
}

function ExportAnnotation() {

    loader.show();

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: "/Home/ExportAnnotations?token=" + globalToken,
        success: function (data) {
            if (data.indexOf("error") > -1) {
                alert("Error exporting, " + data);
            } else {
                window.open("/files/" + data);
            }

            loader.hide();
        },
        error: function (textStatus, errorThrown, data) {
            alert("Error exporting to pdf");

            loader.hide();
        }
    });

}

function ExportXml() {

    loader.show();

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: "/Home/ExportXml?token=" + globalToken,
        success: function (data) {
            if (data.indexOf("error") > -1) {
                alert("Error exporting, " + data);
            } else {
                window.open("/files/" + data);
            }

            loader.hide();
        },
        error: function (textStatus, errorThrown, data) {
            alert("Error exporting to xml");

            loader.hide();
        }
    });

}

function ctlDoc_AnnClosed() {

    $("#navDefault").show();
    $("#navAnnotation").hide();

    SetDirty(false);
}

function ctlDoc_Deleted() {

    ClearAnnotationData();
    SetDirty(true);
}

function ctlDoc_Changed() {
    SetDirty(true);
}

function ctlDoc_AnnLoaded() {
    SetDirty(false);
}

// Do something when any annotation is double clicked
function ctlDoc_Properties() {
    
    LoadAnnotationData();

    $('#annModal').modal('show');

    HideProperties();
}

// Hide properties which are not used by an annotation type
function HideProperties() {
    var ann = objctlDoc.AnnotationController().GetAnnotationById(objctlDoc.AnnotationController().GetSelectedAnnotation());

    // Title
    var annTitle = $('#annTitle');
    var lblannTitle = $('#lblannTitle');
    // Show Title
    var annshowTitle = $('#annshowTitle');
    var lblannshowTitle = $('#lblannshowTitle');
    // Title Color
    var anntitleColor = $('#anntitleColor');
    var lblanntitleColor = $('#lblanntitleColor');
    // Title Font size
    var anntitleFontSize = $('#anntitleFontSize');
    var lblanntitleFontSize = $('#lblanntitleFontSize');
    // Border Color
    var annborderColor = $('#annborderColor');
    var lblannborderColor = $('#lblannborderColor');
    // Show Border
    var annshowBorder = $('#annshowBorder');
    var lblannshowBorder = $('#lblannshowBorder');
    // Back Color
    var annbackColor = $('#annbackColor');
    var lblannbackColor = $('#lblannbackColor');
    // Opacity
    var annOpactiy = $('#annOpacity');
    var lblannOpactiy = $('#lblannOpacity');
    // Rotate
    var annRotate = $('#annRotate');
    var lblannRotate = $('#lblannRotate');
    // Can Rotate
    var annCanRotate = $('#annCanRotate');
    var lblannCanRotate = $('#lblannCanRotate');
    // Note
    var annNote = $('#annNote');
    var lblannNote = $('#lblannNote');
    // Show Note
    var annshowNote = $('#annshowNote');
    var lblannshowNote = $('#lblannshowNote');
    // Text Align
    var anntextAlign = $('#anntextAlign');
    var lblanntextAlign = $('#lblanntextAlign');
    // Line Vertical
    var annlineVertical = $('#annlineVertical');
    var lblannlineVertical = $('#lblannlineVertical');
    // Arrow direction
    var annarrowDirection = $('#annarrowDirection');
    var lblannarrowDirection = $('#lblannarrowDirection');

    // Center div
    var divCenter = $('#divCenter');

    // Show all first
    $(".form-group").each(function () {
        var childEle = $(this).children();
        childEle.each(function () { $(this).show(); });
    });

    divCenter.show();

    // Hide least used  
    annarrowDirection.hide(); lblannarrowDirection.hide();
    annlineVertical.hide(); lblannlineVertical.hide();
    anntextAlign.hide(); lblanntextAlign.hide();

    annNote.hide(); lblannNote.hide();
    annshowNote.hide(); lblannshowNote.hide();

    if (null != ann) {
        $(".modal-title").text("Properties (" + ann.annType + ")");

        switch (ann.annType) {
            case "note":
                annNote.show(); lblannNote.show();
                annTitle.hide(); lblannTitle.hide();
                annshowTitle.hide(); lblannshowTitle.hide();
                lblannNote.text("Note");
                break;
            case "image":
                annNote.show(); lblannNote.show();
                lblannNote.text("Image Url");
                break;
            case "stamp":
                annshowTitle.hide(); lblannshowTitle.hide();
                anntitleColor.hide(); lblanntitleColor.hide();
                break;
            case "triangle":
                annbackColor.hide(); lblannbackColor.hide();
                break;
            case "line":
                annlineVertical.show(); lblannlineVertical.show(); // show

                divCenter.hide();

                annshowBorder.hide(); lblannshowBorder.hide();
                annbackColor.hide(); lblannbackColor.hide();
                annOpactiy.hide(); lblannOpactiy.hide();
                annRotate.hide(); lblannRotate.hide();
                annCanRotate.hide(); lblannCanRotate.hide();

                break;
            case "arrow":
                annarrowDirection.show(); lblannarrowDirection.show(); // show

                divCenter.hide();

                annshowBorder.hide(); lblannshowBorder.hide();
                annbackColor.hide(); lblannbackColor.hide();
                annOpactiy.hide(); lblannOpactiy.hide();
                annRotate.hide(); lblannRotate.hide();
                annCanRotate.hide(); lblannCanRotate.hide();

                break;
        }
    }
}

function DeleteAnnotations() {
    if (null != objctlDoc.AnnotationController()) {
        objctlDoc.AnnotationController().DeleteAnnotation(objctlDoc.AnnotationController().GetSelectedAnnotation());
    }
}

function ClearAnnotations() {
    if (null != objctlDoc.AnnotationController()) {
        objctlDoc.AnnotationController().ClearAnnotations();
    }
}

function LockAnnotations(lock) {
    if (null != objctlDoc.AnnotationController()) {
        objctlDoc.AnnotationController().LockAnnotations(lock);
    }
}

function ctlDoc_OnThumbnailClicked(t) {
    if (annMode) {
        if (CloseAnnotation()) {

            objctlDoc.GotoPage(parseInt(t));
            OpenAnnotation();  

            annotationsSaved = false;
            SetDirty(false);
        }
    }

    return true;
}

function CallNext(goNext) {
    if (CloseAnnotation()) {

        objctlDoc.Next(goNext);
        OpenAnnotation();

        annotationsSaved = false;
        SetDirty(false);
    }
}


function CallSave() {
    var didSave = objctlDoc.SaveAnnotations();

    if (didSave) {

        SetDirty(false);
        annotationsSaved = true;
    }
}

function ctlDoc_Created() {

    // Do something when any annotation is created
    var annObj = objctlDoc.AnnotationController().GetAnnotationById(objctlDoc.AnnotationController().GetSelectedAnnotation());

    annObj.SetAuthor('Admin');

    // you can change color and other properties as element is created

    //annObj.SetBackColor('orange');
    //annObj.Paint();

    SetDirty(true);
}

/* Custom Annotations */

function AddOrangeRect() {
    if (null != objctlDoc.AnnotationController()) {
        var objRect = new RectangleAnnotation({ left: 10, top: 10, width: 300, height: 100, backColor: 'orange', opacity: 70 });
        objctlDoc.AnnotationController().AddAnnotation(null, objRect, null);
    }
}

function AddGoogle() {
    if (null != objctlDoc.AnnotationController()) {
        var objImage = new ImageAnnotation({ left: 100, top: 100, width: 250, height: 100 });
        objctlDoc.AnnotationController().AddAnnotation(null, objImage, null);

        objImage.SetNote('https://www.google.com/images/srpr/logo11w.png');
        objImage.Paint();
    }
}


function AddStamp() {
    if (null != objctlDoc.AnnotationController()) {
        var objStamp = new StampAnnotation({ left: 350, top: 300, width: 300, height: 100, rotate: -15, title: 'Hello World !', borderWidth: 4, borderColor: 'red' });
        objctlDoc.AnnotationController().AddAnnotation(null, objStamp, null);
    }
}

function ImageStamp() {
    if (null != objctlDoc.AnnotationController()) {
        var objStampImage = new ImageAnnotation({ left: 200, top: 200, width: 260, height: 55 });
        objctlDoc.AnnotationController().AddAnnotation(null, objStampImage, null);

        objStampImage.SetNote('/images/Approved_Stamp.png');
        objStampImage.Paint();
    }
}


function LoadAnnotationData() {
    var ann = objctlDoc.AnnotationController().GetAnnotationById(objctlDoc.AnnotationController().GetSelectedAnnotation());

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
        // $('#annBase64').val(Base64.encode(ann.toString()));

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

}


function SaveAnnotationData() {


    var ann = objctlDoc.AnnotationController()
        .GetAnnotationById(objctlDoc.AnnotationController().GetSelectedAnnotation());

    if (null != ann) {
        ann.SetBorderColor($('#annborderColor').val());
        ann.SetBackColor($('#annbackColor').val());
        ann.SetBorderWidth($('#annborderWidth').val());

        ann.SetOpacity($('#annOpactiy').val());
        ann.SetShowBorder($('#annshowBorder').val().toString().toLowerCase() === 'true');

        ann.SetCanRotate($('#annCanRotate').val().toString().toLowerCase() === 'true');
        ann.SetRotate($('#annRotate').val());

        ann.SetShowTitle($('#annshowTitle').val().toString().toLowerCase() === 'true');
        ann.SetTitle($('#annTitle').val());

        ann.SetTitleColor($('#anntitleColor').val());
        ann.SetTitleFontSize($('#anntitleFontSize').val());

        ann.SetNote($('#annNote').val());
        ann.SetShowNote($('#annshowNote').val().toString().toLowerCase() === 'true');

        ann.SetBurn($('#annBurn').val().toString().toLowerCase() === 'true');
        ann.SetLocked($('#annLocked').val().toString().toLowerCase() === 'true');
        ann.SetzIndex(parseInt($('#annzindex').val()));
        ann.SetAuthor($('#annAuthor').val());

        switch (ann.annType) {
        case "line":
            ann.SetLineVertical($('#annlineVertical').val().toString().toLowerCase() === 'true');
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


    SetDirty(true);
}

function ClearAnnotationData() {

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


