
Dropzone.autoDiscover = false;

var objctlDoc = null;
var globalToken = "";
var loader = $(".loader");
var isMobile = false;
var resizing = false;
var docViewerDiv = $("#divDocViewer");
var divHeader = $("#divHeader");
var w = 0;
var h = 0;

loader.hide();
if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
    isMobile = true;
}

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function SetPageNumber() {

    if (globalToken.length == 0) {
        return;
    }

    const pages = objctlDoc.VisiblePages() + "";
    const page = pages.split(",");
    const scrollPage = page[0];

    var currentPage = objctlDoc.CurrentPage();

    if (page.length <= 2) {
        currentPage = scrollPage;
    }

    $("#pageCounter").show().html(currentPage + " of " + objctlDoc.TotalPages());
}

function ctlDoc_OnViewerBusy() {
    loader.show();
}

function ctlDoc_OnViewerReady() {
    loader.hide();
    SetPageNumber();
}

function ctlDoc_OnPageClicked(t) {
    SetPageNumber();
}

function ctlDoc_OnThumbnailClicked(t) {
    SetPageNumber();

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

function ctlDoc_Copy(text) {
    var f = objctlDoc.FileFormat();

    if (f !== "Pdf") {
        alert("Copy function not available for " + f);
    }
    else {
        if (text === "" || text === null) {
            alert("Copy failed");
        }
        else {
            alert(text);
        }
    }
}

function RotateDocument(iAngle) {
    objctlDoc.Rotate(objctlDoc.CurrentPage(), iAngle);
}

function FlipDocument(flipType) {
    objctlDoc.Flip(objctlDoc.CurrentPage(), flipType);
}

function Resize() {

    if (resizing === true) { return; }

    resizing = true;

    docViewerDiv.css("height", "93vh");
    docViewerDiv.height(docViewerDiv.height() - divHeader.height());

    if (isMobile) {
        docViewerDiv.height(docViewerDiv.height() - 20);
    }

    Refit();

    resizing = false;
}


function Refit() {
    if (null !== objctlDoc) {
        objctlDoc.Refit();

        setTimeout(function () { objctlDoc.FitType('width'); }, 1000); // 1 seconds
    }
}

function CloseDocument() {

    if (globalToken.length == 0) {
        return;
    }

    loader.show();

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: "/Home/CloseDocument?token=" + globalToken,
        success: function (data) {
            objctlDoc.Close();

            globalToken = "";
            $("#pageCounter").html("").hide();

            loader.hide();
        },
        error: function (err) {
            alert("Unable to close document. Error: " + err.responseText);
            loader.hide();
        }
    });
}

function OpenDocument(fileName) {

    loader.show();

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: "/Home/OpenDocument?fileName=" + fileName,
        success: function (data) {

            globalToken = data; // for print
            objctlDoc.View(data);

            SetPageNumber();

            // Set keyboard focus to the first page
            objctlDoc.ShowPage(1, true);

            // Custom adjustments for mobile (optional)
            if (isMobile) {
                RefitMobile();
            }
        },
        error: function (err) {
            alert("Unable to open document. Error: " + err.responseText);
            loader.hide();
        }
    });
}

$(window).on("load", function () {
    objctlDoc = $("#div_ctlDoc").docViewer(
        {
            showThumbs: true,
            autoFocus: true,
            autoPageFocus: false,
            pageZoom: 75,
            zoomStep: 10,
            maxZoom: 200,
            format: 'png',
            FitType: 'width',
            debugMode: true,
            showToolTip: true,
            cacheEnabled: false,
            autoLoad: false,
            toolTipPageText: 'Page ',
            BasePath: '/',
            ResPath: 'images',
            largeDoc: true,
            showHyperlinks: true,
            fixedZoom: true,
            fixedZoomPercent: 100,
            fixedZoomPercentMobile: 75
        });

    Resize();
});


$(window).on("orientationchange resize", function () { Resize(); });


function PrintDocument() {
    if (globalToken.length == 0) {
        alert("There is no document to print.");
        return;
    }

    $('#modalPrint').on('shown.bs.modal', function () {
        var printLink = "/Home/Print?token=" + globalToken + "&printtotal=" + objctlDoc.TotalPages();
        $("#printFrame").attr("src", printLink);
    }).modal('show');

}

function ClosePrint() {
    $('#modalPrint').modal('hide');
}

function ShowSearch() {
    NewSearch();
    $('#txtSearch').toggle().focus();
}

function DoSearch() {
    objctlDoc.Search($("#txtSearch").val(), false);

    //optional code
    objctlDoc.SearchSummary(true); // false to remove border box
}

function NewSearch() {
    $("#txtSearch").val("");
    objctlDoc.NewSearch();
}

function GoFS() {

    if (
        document.fullscreenElement ||
        document.webkitFullscreenElement ||
        document.mozFullScreenElement ||
        document.msFullscreenElement
    ) {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        }
    } else {
        var element = document.getElementById("divMain");

        if (element.requestFullscreen) {
            element.requestFullscreen();
        } else if (element.mozRequestFullScreen) {
            element.mozRequestFullScreen();
        } else if (element.webkitRequestFullscreen) {
            element.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        } else if (element.msRequestFullscreen) {
            element.msRequestFullscreen();
        }
    }
}

function OpenUpload() {
    $('#myModal').modal('show');
}

$(document).ready(function () {

    $("#dropZoneForm").dropzone({
        url: "/Home/UploadFile",
        maxFiles: 1,
        paramName: "file",
        uploadMultiple: false,
        maxFilesize: 20,
        acceptedFiles:
            ".doc,.docx,.docm,.odt,.xls,.xlsx,.xlsm,.ods,.csv,.ppt,.pptx,.odp,.vsd,.vsdx,.mpp,.mppx,.pdf,.tif,.tiff,.dwg,.dxf,.dgn,.xps,.psd,.jpg,.jpeg,.jpe,.png,.bmp,.gif,.eml,.msg,.txt,.rtf,.xml,.epub,.svg,.html,.htm,.mht,.dcn,.dcm,.dng,.ico,.eps,.tga,.webp,.cdr,.cmx",
        addRemoveLinks: false,
        init: function () {
            var th = this;
            this.on("success",
                function (file, response) {
                    OpenDocument(response);
                    $('#myModal').modal('hide');
                }),
                this.on("error",
                    function (file, errorMessage, c) {
                        alert("Error uploading document [[" +
                            file.name +
                            "]]. Technical team has been notified.");
                    }),
                this.on("queuecomplete",
                    function () {
                        setTimeout(function () {
                            th.removeAllFiles();
                        },
                            3000);
                    });
        }
    });

});

// Keyboard functions
$(document).keypress(function (e) {
    switch (e.which) {
        case 43:
            objctlDoc.Zoom(true);
            break;
        case 45:
            objctlDoc.Zoom(false);
            break;
        default:
            break;
    }
});

$("#txtSearch").keypress(function (e) {
    switch (e.which) {
        case 13:
            DoSearch();
            break;
        default:
            break;
    }
});
