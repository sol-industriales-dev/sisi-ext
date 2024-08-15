

function ctlDoc_OnViewerBusy() {
    loader.show();
}

function ctlDoc_OnViewerReady() {
    loader.hide();
}

function Resize(orientation) {

    if (resizing) { return; }

    resizing = true;

    w = document.documentElement.clientWidth;
    h = document.documentElement.clientHeight;


    var xdec = 30;
    var ydec = 70;

    if (isMobile) {
        xdec = 30;
        ydec = 70;

        if (typeof orientation !== 'undefined') {
            if (orientation === "landscape") {
                w = document.documentElement.clientHeight;
                h = document.documentElement.clientWidth;
            }
        }
    }

    docViewerDiv.width(w - xdec);
    docViewerDiv.height(h - ydec);

    SetThumbs();
    resizing = false;
}


function SetThumbs() {
    if (annMode)
        return;

    try {
        objctlDoc.HideThumbs(true);
        objctlDoc.HideThumbs(false);
    } catch (exception) {

    }
}


function OpenDocument(fileName) {

    loader.show();

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/OpenFile?name=" + fileName,
        success: function (data) {
            if (data.indexOf("Error") > -1) {
                alert(data);
                loader.hide();
            }
            else {
                objctlDoc.View(data); // use global object to view any document
                globalToken = data; // data is actuall a Token (unique to the document being viewed)
                objctlDoc.FitType('width');
            }
        },
        error: function (xhr) {
            var errorMessage = xhr.status + ': ' + xhr.statusText
            alert(errorMessage);
            loader.hide();
        }
    });
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
        var element = $('#divDocViewer').get(0);

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
            ".doc,.docx,.docm,.odt,.xls,.xlsx,.xlsm,.ods,.csv,.ppt,.pptx,.odp,.vsd,.vsdx,.mpp,.mppx,.pdf,.tif,.tiff,.dwg,.dxf,.dgn,.xps,.psd,.jpg,.jpeg,.jpe,.png,.bmp,.gif,.eml,.msg,.txt,.rtf,.xml,.epub,.svg,.html,.htm,.mht,.dcn,.dcm",
        addRemoveLinks: false,
        init: function () {
            var th = this;
            this.on("success",
                function (file, response) {

                    OpenDocument(response); // Response is the Token itself.

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

