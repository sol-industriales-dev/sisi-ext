(function () {

    $.namespace('encuestas.responder');

    responder = function () {
        _numeroOC = 0;
        _encuestaID = 0,
        _requisicion = 0;
        _encuestaUsuarioID = 0,
        btnEnviar = $("#btnEnviar"),
        txtTitulo = $("#txtTitulo"),
        txtDescripcion = $("#txtDescripcion"),
        txtAsunto = $("#txtAsunto"),
        txtComentario = $("#txtComentario"),
        asunto = $(".asunto"),
        Preguntas = $(".Preguntas");


        //Elementos de Requisiciones
        encabezadoProveedores = $("#encabezadoProveedores"),
        encabezadoServicios = $("#encabezadoServicios"),
        tbProveedorRequisiciones = $("#tbProveedorRequisiciones"),
        tbEvaluadorRequisiciones = $("#tbEvaluadorRequisiciones");
        //

        //Elementos de Encabezado
        tbProveedor = $("#tbProveedor"),
        tbEvaluacion = $("#tbEvaluacion"),
        tbTipoProveedor = $("#tbTipoProveedor"),
        tbAntiguedadProveedor = $("#tbAntiguedadProveedor"),
        tbUbicacionProveedor = $("#tbUbicacionProveedor"),
        tbEvaluador = $("#tbEvaluador");

        //Fin de elementos de encabezado

        function init() {

            btnEnviar.click(fnEnviar);
            var tipoEncuesta = $.urlParam('tipoEncuesta');
            if (tipoEncuesta == 1) {
                cargarEncuesta();
            }
            else {
                Requisiciones();
            }

        }
        function fnEnviar() {
            var tipoEncuesta = $.urlParam('tipoEncuesta');
            var preguntas = $(".starrr");
            var preguntasList = new Array();
            var validacion = false;
            var totalPonderacion = 0;
            $.each(preguntas, function (i, e) {
                var o = {};
                o.encuestaID = _encuestaID;
                o.encuestaUsuarioID = _encuestaUsuarioID;
                o.preguntaID = $(e).data("id");
                o.calificacion = $(e).attr('data-calificacion');
                o.ponderacion = $(e).attr('data-ponderacion');
                o.respuesta = $('[data-id="txtRespuesta' + o.preguntaID + '"]').val()
                if (o.calificacion == 0 && o.respuesta == "") {
                    ConfirmacionGeneral("Confirmación", "¡No todas las preguntas han sido contestadas! Favor de responderlas.");
                    validacion = true;
                }
                else if (o.calificacion <= 3 && o.respuesta == "") {
                    ConfirmacionGeneral("Confirmación", "¡No todas las preguntas han sido contestadas! Favor de responderlas.");
                    validacion = true;
                }

                if (o.calificacion > 3) {
                    totalPonderacion += Number(o.ponderacion);
                }


                preguntasList.push(o);
            });

            if (totalPonderacion <= 0.74) {
                if (txtComentario.val().trim('') == "") {
                    ConfirmacionGeneral("Confirmación", "¡Calificaciones Regulares y Malos deben llevar un Comentario Obligatorio.");
                    validacion = true;
                }

            }

            if (tipoEncuesta == "1") {
                if (tbTipoProveedor.val() != "") {
                    if (validacion == false) {
                        $.ajax({
                            datatype: "json",
                            type: "POST",
                            url: "/Encuestas/EncuestasProveedor/saveEncuestaResult",
                            data: { obj: preguntasList, objSingle: getObjDetalle(), encuestaID: _encuestaID, comentario: txtComentario.val() },
                            asyn: false,
                            success: function (response) {
                                ConfirmacionGeneral("Confirmación", "¡Encuesta contestada correctamente!");
                                setInterval(function () {
                                    var blob = $.urlParam('blob');
                                    if (blob == null) {
                                        window.location.href = "/Encuestas/EncuestasProveedor/Dashboard";
                                    }
                                    else {
                                        window.location.href = "/Usuario/Login";
                                    }

                                }, 2000);
                            },
                            error: function () {
                                $.unblockUI();
                            }
                        });
                    }
                }
                else {
                    ConfirmacionGeneral("Alerta", "¡Se debe tener el tipo de proveedor!");
                }
            }
            else {
                if (validacion == false) {
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: "/Encuestas/EncuestasProveedor/saveEncuestaResultRequisicion",
                        data: { obj: preguntasList, objSingle: getObjRequisiciones(), encuestaID: _encuestaID, comentario: txtComentario.val() },
                        asyn: false,
                        success: function (response) {
                            ConfirmacionGeneral("Confirmación", "¡Encuesta contestada correctamente!");
                            setInterval(function () {
                                var blob = $.urlParam('blob');
                                if (blob == null) {
                                    window.location.href = "/Encuestas/EncuestasProveedor/Dashboard";
                                }
                                else {
                                    window.location.href = "/Encuestas/EncuestasProveedor/Dashboard";
                                }

                            }, 2000);
                        },
                        error: function () {
                            $.unblockUI();
                        }
                    });
                }

                else {
                    ConfirmacionGeneral("Alerta", "¡Se debe tener el tipo de proveedor!");
                }
            }
        }

        function getObjRequisiciones() {
            return {
                id: 0,
                centroCostos: "",//$.urlParam('CC'),
                centroCostosName: "",
                numeroRequisicion: _requisicion,
                nombreProveedor: tbProveedorRequisiciones.val(),
                comentarios: txtComentario.val(),
                tipoEncuesta: 2
            };
        }

        function getObjDetalle() {
            return {
                numeroOC: _numeroOC,
                numProveedor: tbProveedor.attr('data-idProveedor'),
                centrocostos: tbProveedor.attr('data-cc'),
                nombreProveedor: tbProveedor.val(),
                FechaActual: tbEvaluacion.val(),
                fechaAntiguedad: tbAntiguedadProveedor.val(),
                ubicacionProveedor: tbUbicacionProveedor.val(),
                encuestaID: _encuestaID,
                comentarios: txtComentario.val(),
                tipoProveedor: tbTipoProveedor.val(),
                tipoEncuesta: 1
            };
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
        }

        function cargarEncuesta() {
            var id = $.urlParam('encuesta');
            var numOC = $.urlParam('numOC');
            var centrocostos = $.urlParam('CC');

            if (id != null) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/EncuestasProveedor/ResponderEncuesta",
                    data: { encuestaID: id, numeroOC: numOC, centrocostos: centrocostos },
                    asyn: false,
                    success: function (response) {
                        var obj = response.obj;
                        var blob = $.urlParam('encuesta');

                        if (response.RespuestaEncuesta) {
                            ConfirmacionGeneral("Alerta", "Esta encuesta ya fue contestada.");
                            setInterval(function () {
                                AbandonarSession(this);
                            }, 2000);
                        }
                        else {
                            moment.locale();
                            var proveedoresData = response.getDatosProveedor;
                            _encuestaID = response.id;
                            _numeroOC = numOC;
                            //  _encuestaUsuarioID = obj.encuestaUsuarioID;
                            var FechaActual = new Date();

                            var fechaAnt = Number(proveedoresData.fechaAntiguedad.split('(')[1].split(')')[0]);

                            tbProveedor.val(proveedoresData.nombreProveedor);
                            tbProveedor.attr('data-idProveedor', proveedoresData.numProveedor);
                            tbProveedor.attr('data-cc', proveedoresData.centrocostos);

                            tbEvaluacion.val(moment(FechaActual).format('DD/MM/YYYY'));
                            var fechaAntiguedad = moment(fechaAnt).format('DD/MM/YYYY');
                            tbAntiguedadProveedor.val(fechaAntiguedad);
                            tbUbicacionProveedor.val(proveedoresData.ubicacionProveedor);
                            tbEvaluador.val(response.evaluador);
                            txtTitulo.val(response.titulo);
                            txtDescripcion.val(response.descripcion);
                            txtAsunto.val(response.tipoEncuesta);
                            asunto.show();

                            $.ajax({
                                url: "/Encuestas/Encuesta/getEstrellas",
                                asyn: false,
                                success: function (respuestaEstrellas) {
                                    var estrellas = respuestaEstrellas.data;

                                    $.each(response.preguntas, function (i, e) {
                                        var pregunta = fnAddPregunta(e.pregunta, e.ponderacion);
                                        $(pregunta).appendTo(Preguntas);
                                        $('.starrr').starrr({
                                            rating: 0,
                                            change: function (e, value) {
                                                var id = $(e.currentTarget).data("id");
                                                $(e.currentTarget).attr("data-calificacion", value);
                                                if (value <= 3) {
                                                    $('[data-id="txtRespuesta' + id + '"]').show();
                                                }
                                                if (value > 3) {
                                                    $('[data-id="txtRespuesta' + id + '"]').hide();
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
                            });
                        }
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }

        function Requisiciones() {
            var id = $.urlParam('encuesta');
            var requisicion = $.urlParam('requisicion');
            var centrocostos = $.urlParam('CC');
            encabezadoProveedores.addClass('hide');
            encabezadoServicios.removeClass('hide');

            if (id != null) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/EncuestasProveedor/ResponderEncuestaRequisicion",
                    data: { encuestaID: id, noRequisicion: requisicion, centrocostos: centrocostos },
                    asyn: false,
                    success: function (response) {
                        var obj = response.obj;
                        var blob = $.urlParam('encuesta');

                        if (response.RespuestaEncuesta) {
                            ConfirmacionGeneral("Alerta", "Esta encuesta ya fue contestada.");
                            setInterval(function () {
                                AbandonarSession(this);
                            }, 2000);
                        }
                        else {
                            moment.locale();
                            _encuestaID = response.id;
                            _requisicion = requisicion;
                            var FechaActual = new Date();

                            txtTitulo.val(response.titulo);
                            txtDescripcion.val(response.descripcion);
                            txtAsunto.val(response.tipoEncuesta);
                            asunto.show();
                            $.each(response.preguntas, function (i, e) {
                                var pregunta = fnAddPregunta(e.pregunta, e.id, e.ponderacion);
                                $(pregunta).appendTo(Preguntas);
                                $('.starrr').starrr({
                                    rating: 0,
                                    change: function (e, value) {
                                        var id = $(e.currentTarget).data("id");
                                        $(e.currentTarget).attr("data-calificacion", value);
                                        if (value <= 3) {
                                            $('[data-id="txtRespuesta' + id + '"]').show();
                                        }
                                        if (value > 3) {
                                            $('[data-id="txtRespuesta' + id + '"]').hide();
                                        }
                                    }
                                })
                            });
                        }
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }

        function fnAddPregunta(text, id, poderacion) {
            var html = '<div class="row Pregunta">';
            html += '    <div class="col-lg-12">';
            html += '        <div class="row">';
            html += '            <div class="col-lg-12">';
            html += '                <div class="input-group">';
            html += '                    <span class="input-group-addon">Pregunta:</span>';
            html += '                    <div style="border:1px dotted gray;height: 32px;">';
            html += '                        <div class="starrr" data-id="' + id + '" data-calificacion="0" data-ponderacion="' + poderacion + '"></div>';
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


