(function () {
    $.namespace('encuestas.responder');
    responder = function () {
        _encuestaID = 0,
        _encuestaUsuarioID = 0,
        btnEnviar = $("#btnEnviar"),
        txtTitulo = $("#txtTitulo"),
        txtDescripcion = $("#txtDescripcion"),
        txtAsunto = $("#txtAsunto"),
        txtComentario = $("#txtComentario"),
        asunto = $(".asunto"),
        Preguntas = $(".Preguntas");

        function init() {
            btnEnviar.click(fnEnviar);
            cargarEncuesta();
        }
        function fnEnviar() {
            var preguntas = $(".starrr");
            var preguntasList = new Array();
            var validacion = false;
            $.each(preguntas, function (i, e) {
                var o = {};
                o.encuestaID = _encuestaID;
                o.encuestaUsuarioID = _encuestaUsuarioID;
                o.preguntaID = $(e).data("id");
                o.calificacion = $(e).attr("data-calificacion");
                o.respuesta = $('[data-id="txtRespuesta' + o.preguntaID + '"]').val()
                if (o.calificacion == 0) {
                    ConfirmacionGeneral("Confirmación", "¡No todas las preguntas han sido contestadas! Favor de responderlas.");
                    validacion = true;
                }
                else if (o.calificacion <= 3 && (o.respuesta == "" || o.respuesta == null || o.respuesta == undefined)) {
                    ConfirmacionGeneral("Confirmación", "¡No todas las preguntas han sido contestadas! Favor de responderlas.");
                    validacion = true;
                }
                preguntasList.push(o);
            });
            if (validacion == false) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/Encuesta/saveEncuestaResult",
                    data: { obj: preguntasList, encuestaID: _encuestaID, encuestaUsuarioID: _encuestaUsuarioID, comentario: txtComentario.val() },
                    asyn: false,
                    success: function (response) {
                        if (response.success) {
                            ConfirmacionGeneral("Confirmación", "¡Encuesta contestada correctamente!");
                            setInterval(function () {
                                var blob = $.urlParam('blob');
                                if (blob == null) {
                                    window.location.href = "/Encuestas/Encuesta/Dashboard";
                                }
                                else {
                                    window.location.href = "/Usuario/Login";
                                }

                            }, 2000);
                        }
                        else {
                            AlertaGeneral('Alerta', response.message);
                        }
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }
        function datePicker() {
            var now = new Date(),
            year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
              from = $("#fechaIni")
                .datepicker({
                    changeMonth: true,
                    changeYear: true,
                    numberOfMonths: 1,
                    defaultDate: new Date(year, 00, 01),
                    maxDate: new Date(year, 11, 31),

                    onChangeMonthYear: function (y, m, i) {
                        var d = i.selectedDay;
                        $(this).datepicker('setDate', new Date(y, m - 1, d));
                        $(this).trigger('change');
                    }
                })
                .on("change", function () {
                    to.datepicker("option", "minDate", getDate(this));
                }),
              to = $("#fechaFin").datepicker({
                  changeMonth: true,
                  changeYear: true,
                  numberOfMonths: 1,
                  defaultDate: new Date(),
                  maxDate: new Date(year, 11, 31),
                  onChangeMonthYear: function (y, m, i) {
                      var d = i.selectedDay;
                      $(this).datepicker('setDate', new Date(y, m - 1, d));
                      $(this).trigger('change');
                  }
              })
              .on("change", function () {
                  from.datepicker("option", "maxDate", getDate(this));
              });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
            function cargarEncuesta() {
                var id = $.urlParam('encuesta');
                if (id != null) {
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: "/Encuestas/Encuesta/getEncuesta",
                        data: { id: id },
                        asyn: false,
                        success: function (response) {
                            var obj = response.obj;
                            if (obj.contestada) {
                                ConfirmacionGeneral("Alerta", "Esta encuesta ya fue contestada.");
                                setInterval(function () {
                                    AbandonarSession(this);
                                }, 2000);
                            } else {
                                _encuestaID = obj.id;
                                _encuestaUsuarioID = obj.encuestaUsuarioID;

                                txtTitulo.val(obj.titulo);
                                txtDescripcion.val(obj.descripcion);
                                txtPregunta.val("");
                                $.each(obj.preguntas, function (i, e) {
                                    var pregunta = fnAddPregunta(e.pregunta, e.id);
                                    $(pregunta).appendTo(Preguntas);
                                    //$('.starrr').starrr({
                                    //    rating: i.calificacion
                                    //})
                                    $('.starrr').starrr();
                                });
                            }
                        },
                        error: function () {
                        }
                    });
                }
            }
        }
        function cargarEncuesta() {
            var id = $.urlParam('encuesta');
            if (id != null) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/Encuesta/getEncuesta",
                    data: { id: id },
                    asyn: false,
                    success: function (response) {
                        var responseEncuesta = response;
                        $.ajax({
                            url: "/Encuestas/Encuesta/getEstrellas",
                            asyn: false,
                            success: function (respuestaEstrellas) {
                                var estrellas = respuestaEstrellas.data;

                                var obj = responseEncuesta.obj;
                                var blob = $.urlParam('encuesta');
                                if (obj.contestada) {
                                    ConfirmacionGeneral("Alerta", "Esta encuesta ya fue contestada.");
                                    setInterval(function () {
                                        AbandonarSession(this);
                                    }, 2000);
                                }
                                else {
                                    _encuestaID = obj.id;
                                    _encuestaUsuarioID = obj.encuestaUsuarioID;
                                    txtTitulo.val(obj.titulo);
                                    txtDescripcion.val(obj.descripcion);
                                    txtAsunto.val(obj.asunto);
                                    asunto.show();
                                    $.each(obj.preguntas, function (i, e) {
                                        var pregunta = fnAddPregunta(e.pregunta, e.id);

                                        $(pregunta).appendTo(Preguntas);

                                        $('.starrr').starrr({
                                            rating: 0,
                                            //max: estrellas.length,
                                            change: function (e, value) {
                                                var id = $(e.currentTarget).data("id");
                                                if ($('.starrr[data-id="'+id+'"]').find('.far').length == 5) {
                                                    value = 0;
                                                }
                                                $(e.currentTarget).attr("data-calificacion", value);
                                                if (value <= 3) {
                                                    $('[data-id="txtRespuesta' + id + '"]').show();
                                                }
                                                if (value > 3 || value == 0) {
                                                    $('[data-id="txtRespuesta' + id + '"]').hide();
                                                    $('[data-id="txtRespuesta' + id + '"]').val('');
                                                }

                                                let etiqueta = $(e.currentTarget).find('label');
                                                if (value > 0) {
                                                    $.each(estrellas, function (index, est) {
                                                        if (est.estrellas == value) {
                                                            etiqueta.text(est.descripcion);
                                                        }
                                                    });
                                                } else {
                                                    etiqueta.text('');
                                                }
                                            }
                                        });

                                        var etiqueta = document.createElement('label');
                                        etiqueta.style.marginLeft = '10px';
                                        $('.starrr:last').append(etiqueta);
                                    });
                                }
                            },
                            error: function () {
                                $.unblockUI();
                            }
                        });
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }
        function fnAddPregunta(text, id) {
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
            html += '                <textarea class="form-control txtPregunta" placeholder="Explica tu respuesta" style="display:none;" data-respuesta="" data-id="txtRespuesta' + id + '"></textarea>';
            html += '            </div>';
            html += '        </div>';
            html += '    </div>';
            html += '</div>';
            return html;
        }
        init();
    };

    $(document).ready(function () {
        encuestas.responder = new responder();
    });
})();


