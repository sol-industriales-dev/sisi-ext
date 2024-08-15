(function () {

    $.namespace('encuestas.dashboard');

    dashboard = function () {
        mensajes = {
            PROCESANDO: 'Procesando...'
        },
        _encuestaID = 0,
        _encuestaUpdateID = 0,
        tblPendiente = $("#tblPendiente"),
        tblAutorizada = $("#tblAutorizada"),
        tblRechazada = $("#tblRechazada"),
        btnAceptar = $("#btnAceptar"),
        btnRechazar = $("#btnRechazar"),
        dialogAceptar = $("#dialogAceptar"),
        dialogRechazar = $("#dialogRechazar");
        btnAceptarUpdate = $("#btnAceptarUpdate"),
        btnRechazarUpdate = $("#btnRechazarUpdate"),
        dialogAceptarUpdate = $("#dialogAceptarUpdate"),
        dialogRechazarUpdate = $("#dialogRechazarUpdate");

        function init() {
            btnAceptar.click(function () {
                setAceptarEncuesta();
            });
            btnRechazar.click(function () {
                setRechazarEncuesta();
            });
            btnAceptarUpdate.click(function () {
                setAceptarEncuestaUpdate();
            });
            btnRechazarUpdate.click(function () {
                setRechazarEncuestaUpdate();
            });
            loadGrid(getFiltrosObject(), '/Encuestas/Encuesta/getEncuestaPendiente', tblPendiente);
            loadGrid(getFiltrosObject(), '/Encuestas/Encuesta/getEncuestaAceptada', tblAutorizada);
            loadGrid(getFiltrosObject(), '/Encuestas/Encuesta/getEncuestaRechazada', tblRechazada);
        }
        function getFiltrosObject() {
            return {
                estatusAutorizada: 0
            }
        }
        init();
    };

    $(document).ready(function () {
        encuestas.dashboard = new dashboard();
    });

})();
function loadTablas() {
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Encuestas/Encuesta/getEncuestaPendiente",
        success: function (response) {
            tblPendiente.bootgrid("clear");
            var JSONINFO = response.rows;
            tblPendiente.bootgrid("append", JSONINFO);
            tblPendiente.bootgrid('reload');
        },
        error: function () {
            $.unblockUI();
        }
    });
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Encuestas/Encuesta/getEncuestaAceptada",
        success: function (response) {
            tblAutorizada.bootgrid("clear");
            var JSONINFO = response.rows;
            tblAutorizada.bootgrid("append", JSONINFO);
            tblAutorizada.bootgrid('reload');
        },
        error: function () {
            $.unblockUI();
        }
    });
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Encuestas/Encuesta/getEncuestaRechazada",
        success: function (response) {
            tblRechazada.bootgrid("clear");
            var JSONINFO = response.rows;
            tblRechazada.bootgrid("append", JSONINFO);
            tblRechazada.bootgrid('reload');
        },
        error: function () {
            $.unblockUI();
        }
    });
}
function setAceptarEncuesta() {
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Encuestas/Encuesta/setAceptarEncuesta",
        data: { id: _encuestaID },
        asyn: false,
        success: function (response) {
            $("#dialogAceptar .close").click();
            loadTablas();
        },
        error: function () {
            $.unblockUI();
        }
    });
}
function setRechazarEncuesta() {
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Encuestas/Encuesta/setRechazarEncuesta",
        data: { id: _encuestaID },
        asyn: false,
        success: function (response) {
            $("#dialogRechazar .close").click();
            loadTablas();
        },
        error: function () {
            $.unblockUI();
        }
    });
}
function fnVerEncuesta(id) {
    _encuestaID = id;
    cargarEncuesta(id);
    $("#dialogVerEncuesta").dialog({
        resizable: false,
        height: 600,
        width: "1000px",
        modal: true,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");
            }
        }
    });
}

function fnAceptarEncuesta(id) {
    _encuestaID = id;
    dialogAceptar.modal("show");
}
function fnRechazarEncuesta(id) {
    _encuestaID = id;
    dialogRechazar.modal("show");
}
function cargarEncuesta(id) {
    if (id != null) {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/Encuestas/Encuesta/getEncuestaValidar",
            data: { id: id },
            asyn: false,
            success: function (response) {
                var obj = response.obj;
                _encuestaID = obj.id;
                $("#txtTitulo").val(obj.titulo);
                $("#txtDescripcion").val(obj.descripcion);
                $(".Preguntas").empty();
                $.each(obj.preguntas, function (i, e) {
                    var pregunta = fnAddPregunta(e.pregunta, e.id, e.respuesta, e.calificacion);
                    $(pregunta).appendTo($(".Preguntas"));
                    $('.starrr').starrr({
                        rating: e.calificacion,
                        readOnly: true,
                        change: function (e, value) {
                            var id = $(e.currentTarget).data("id");
                            $(e.currentTarget).attr("data-calificacion", value);
                        }
                    })
                });
                $("#txtComentario").val(obj.comentario);
            },
            error: function () {
                $.unblockUI();
            }
        });
    }
}
function fnAddPregunta(text, id, respuesta, calificacion) {
    var html = '<div class="row Pregunta">';
    html += '    <div class="col-lg-12">';
    html += '        <div class="row">';
    html += '            <div class="col-lg-12">';
    html += '                <div class="input-group">';
    html += '                    <span class="input-group-addon">Pregunta:</span>';
    html += '                    <div style="border:1px dotted gray;height: 32px;">';
    html += '                        <div class="starrr" data-id="' + id + '" data-calificacion="0"></div>';
    html += '                    </div>';
    html += '                </div>';
    html += '            </div>';
    html += '        </div>';

    html += '        <div class="row">';
    html += '            <div class="col-lg-12">';
    html += '                <textarea class="form-control txtPregunta" placeholder="Pregunta" data-calificacion="" data-id="' + id + '" disabled>' + text + '</textarea>';
    if (calificacion <= 3) {
        html += '                <textarea class="form-control" style="border:3px dotted red;" disabled>' + respuesta + '</textarea>';
    }
    html += '            </div>';
    html += '        </div>';

    html += '    </div>';
    html += '</div>';
    return html;
}
//---------------
function fnAceptarEncuestaUpdate(id, idUpdate) {
    _encuestaID = id;
    _encuestaUpdateID = idUpdate;
    dialogAceptarUpdate.modal("show");
}
function fnRechazarEncuestaUpdate(id, idUpdate) {
    _encuestaID = id;
    _encuestaUpdateID = idUpdate;
    dialogRechazarUpdate.modal("show");
}
function fnVerEncuestaUpdate(id, idUpdate) {
    _encuestaID = id;
    _encuestaUpdateID = idUpdate;
    cargarEncuestaUpdate(id, idUpdate);
    $("#dialogVerEncuestaUpdate").dialog({
        resizable: false,
        height: 600,
        width: "95%",
        modal: true,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");
            }
        }
    });
}
function cargarEncuestaUpdate(id, idUpdate) {
    if (id != null) {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/Encuestas/Encuesta/getEncuestaValidarUpdate",
            data: { id: id, idUpdate: idUpdate },
            asyn: false,
            success: function (response) {
                var obj = response.obj;
                var objUpdate = response.objUpdate;
                _encuestaID = obj.id;
                $("#txtTituloa").val(obj.titulo);
                $("#txtDescripciona").val(obj.descripcion);
                $(".Preguntasa").empty();
                $.each(obj.preguntas, function (i, e) {
                    var pregunta = fnAddPregunta(e.pregunta, e.id, e.respuesta, e.calificacion);
                    $(pregunta).appendTo($(".Preguntasa"));
                    $('.starrr').starrr({
                        rating: e.calificacion,
                        readOnly: true,
                        change: function (e, value) {
                            var id = $(e.currentTarget).data("id");
                            $(e.currentTarget).attr("data-calificacion", value);
                        }
                    })
                });
                $("#txtComentarioa").val(obj.comentario);
                //-----------
                $("#txtTituloau").val(objUpdate.titulo);
                if (obj.titulo == objUpdate.titulo) {
                    $("#txtTituloau").css("backgroundColor", "none");
                }
                else {
                    $("#txtTituloau").css("backgroundColor", "yellow");
                }
                $("#txtDescripcionau").val(objUpdate.descripcion);
                if (obj.descripcion == objUpdate.descripcion) {
                    $("#txtDescripcionau").css("backgroundColor", "none");
                }
                else {
                    $("#txtDescripcionau").css("backgroundColor", "yellow");
                }
                $(".Preguntasau").empty();
                $.each(objUpdate.preguntas, function (i, e) {
                    var pregunta = fnAddPreguntaUpdate(e.pregunta, e.id, e.respuesta, e.calificacion, e.estatus);
                    $(pregunta).appendTo($(".Preguntasau"));
                    $('.starrr').starrr({
                        rating: e.calificacion,
                        readOnly: true,
                        change: function (e, value) {
                            var id = $(e.currentTarget).data("id");
                            $(e.currentTarget).attr("data-calificacion", value);
                        }
                    })
                });
                $("#txtComentarioau").val(objUpdate.comentario);
            },
            error: function () {
                $.unblockUI();
            }
        });
    }
}
function setAceptarEncuestaUpdate() {
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Encuestas/Encuesta/setAceptarEncuestaUpdate",
        data: { id: _encuestaUpdateID },
        asyn: false,
        success: function (response) {
            $("#dialogAceptarUpdate .close").click();
            loadTablas();
        },
        error: function () {
            $.unblockUI();
        }
    });
}
function setRechazarEncuestaUpdate() {
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Encuestas/Encuesta/setRechazarEncuestaUpdate",
        data: { id: _encuestaUpdateID },
        asyn: false,
        success: function (response) {
            $("#dialogRechazarUpdate .close").click();
            loadTablas();
        },
        error: function () {
            $.unblockUI();
        }
    });
}
function fnAddPreguntaUpdate(text, id, respuesta, calificacion, estatus) {
    var html = '<div class="row Pregunta">';
    html += '    <div class="col-lg-12">';
    html += '        <div class="row">';
    html += '            <div class="col-lg-12">';
    html += '                <div class="input-group">';
    html += '                    <span class="input-group-addon">Pregunta:</span>';
    html += '                    <div style="border:1px dotted gray;height: 32px;">';
    html += '                        <div class="starrr" data-id="' + id + '" data-calificacion="0"></div>';
    html += '                    </div>';
    html += '                </div>';
    html += '            </div>';
    html += '        </div>';

    html += '        <div class="row">';
    html += '            <div class="col-lg-12">';
    if (estatus == 'Agregado') {
        html += '                <textarea class="form-control txtPregunta" placeholder="Pregunta" data-calificacion="" data-id="' + id + '" disabled>' + text + '</textarea>';
    }
    else if (estatus == 'Agregar') {
        html += '                <textarea class="form-control txtPregunta" style="background-color:green;color:white;" placeholder="Pregunta" data-calificacion="" data-id="' + id + '" disabled>' + text + '</textarea>';
    }
    else if (estatus == 'Actualizar') {
        html += '                <textarea class="form-control txtPregunta" style="background-color:yellow;" placeholder="Pregunta" data-calificacion="" data-id="' + id + '" disabled>' + text + '</textarea>';
    }
    else if (estatus == 'Eliminar') {
        html += '                <textarea class="form-control txtPregunta" style="background-color:red;color:white;" placeholder="Pregunta" data-calificacion="" data-id="' + id + '" disabled>' + text + '</textarea>';
    }
    if (calificacion <= 3) {
        html += '                <textarea class="form-control" style="border:3px dotted red;" disabled>' + respuesta + '</textarea>';
    }
    html += '            </div>';
    html += '        </div>';

    html += '    </div>';
    html += '</div>';
    return html;
}

function stars(num) {
    var s = "";
    for (var i = 1; i <= num; i++) {
        s = s + '★';
    }
    var n = 5 - num;
    if (n > 0) {
        for (var i = 1; i <= n; i++) {
            s = s + '☆';
        }
    }
    return "<span style='color:yellow;font-size:34px;position: relative;top: -11px;'>" + s + "</span>";
}