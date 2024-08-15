var objctlDoc = null;
var divContainer = null;
var docViewerDiv = null;
var isMobile = false;
var resizing = false;
var globalToken = "";

if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
    isMobile = true;
}

function DocInit(ID, docModel) {

    docViewerDiv = $("#divDocViewer_" + ID);
    divContainer = $("#divContainer");

    var docJson = JSON.parse(docModel);

    objctlDoc = $("#div_" + ID).docViewer(
        {
            showThumbs: docJson.ShowThumbs,
            autoFocus: docJson.AutoFocus,
            autoPageFocus: docJson.AutoPageFocus,
            pageZoom: docJson.PageZoom,
            zoomStep: docJson.ZoomStep,
            maxZoom: docJson.MaxZoom,
            FitType: docJson.FitType,
            showToolTip: docJson.ShowToolTip,
            cacheEnabled: docJson.CacheEnabled,
            autoLoad: docJson.AutoLoad,
            toolTipPageText: docJson.ToolTipPageText,
            BasePath: docJson.BasePath,
            ResPath: docJson.ResPath,
            largeDoc: docJson.LargeDoc,
            showHyperlinks: docJson.ShowHyperlinks,
            fixedZoom: docJson.FixedZoom,
            fixedZoomPercent: docJson.FixedZoomPercent,
            fixedZoomPercentMobile: docJson.FixedZoomPercentMobile,
            debugMode: true
        });

    AttachEvents();
}

/* This method will finally call the instance method on control */
async function openDocumentJS(dotNetObjRef, fileName, filePassword) {
    // show loading image.
    $("#imgLoading").show();

    var promise = dotNetObjRef.invokeMethodAsync("OpenDocument", fileName, filePassword);
    var token = await promise;

    if (token.length > 0) {

        if (/\s/.test(token)) {
            alert("Error: " + token);
            token = "";
        }
        else {
            globalToken = token;   /* For print. etc. */
            objctlDoc.View(token);
        }

    } else {
        alert("Error opening " + fileName);
    }

    $("#imgLoading").hide();
    return token;
}

/* Method to view from token */
async function viewDocumentJS(token) {
    if (token.length > 0) {
        objctlDoc.View(token);
    } else {
        alert("Invalid token");
    }
}

function Resize(orientation) {

    if (resizing) { return; }
    if (null == divContainer || null == docViewerDiv) { return; }

    resizing = true;

    w = divContainer.width();
    h = divContainer.height();

    if (isMobile) {
        if (typeof orientation !== 'undefined') {
            if (orientation === "landscape") {
                w = divContainer.height();
                h = divContainer.width();
            }
        }
    }

    docViewerDiv.width(w);
    docViewerDiv.height(h);

    SetThumbs();
    resizing = false;
}

function SetThumbs() {
    try {
        objctlDoc.Refit();
    } catch (exception) {

    }
}

if (isMobile) {
    $(window).on("orientationchange", function (event) { Resize(event.orientation); });
}
else {
    $(window).on("resize", function () { Resize(); });
}

function AttachEvents() {

    Resize();

    SetPageNumber();

    SliderEvent();

    if (isMobile) {
        $("#altScroll").text("");
        objctlDoc.HideThumbs(true);
    }
    else {
        MouseWheelScroll();
    }

    $('#txtPage').on('input', function () {
        var page = parseInt($(this).val());
        objctlDoc.GotoPage(page);
    });


    $("#txtPage").attr("placeholder", "Max " + objctlDoc.TotalPages() + " pages");

}

function SliderEvent() {
    var txtZoom = $("#txtZoom");
    txtZoom.on("mouseup keyup touchend", function () { ChangeZoom(); });
}


function ChangeZoom() {
    var newZoom = $("#txtZoom").val();
    objctlDoc.Zoom(newZoom);
}

function GoNext() {
    objctlDoc.Next(true);
}

function GoPrevious() {
    objctlDoc.Next(false);
}

function SetPageNumber() {
    $("#txtPage").val(objctlDoc.CurrentPage());
}

function MouseWheelScroll() {
    $(window).on('mousewheel DOMMouseScroll', function (event) {
        if (event.altKey == true && null !== objctlDoc) {

            if (event.originalEvent.wheelDelta > 0) {
                objctlDoc.Zoom(true);
            } else {
                objctlDoc.Zoom(false);
            }
        }
    });
}

function ctlDoc_OnViewerBusy() {
 
}

function ctlDoc_OnViewerReady() {
    $("#txtZoom").val(objctlDoc.CurrentZoom());
    SetPageNumber();
}

function ctlDoc_OnPageClicked(t) {
    SetPageNumber();
}

function ctlDoc_OnThumbnailClicked(t) {
    SetPageNumber();
}


function PrintDocument() {
    if (globalToken.length == 0) {
        alert("There is no document to print.");
        return;
    }

    $('#modalPrint').on('shown.bs.modal', function () {
        var printLink = "/Print?token=" + globalToken + "&printtotal=" + objctlDoc.TotalPages();
        $("#printFrame").attr("src", printLink);
    }).modal('show');

}

function ClosePrint() {
    $('#modalPrint').modal('hide');
}
