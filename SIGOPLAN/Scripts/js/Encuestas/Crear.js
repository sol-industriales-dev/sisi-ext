(function () {

    $.namespace('encuestas.crear');

    crear = function () {
        _encuestaID = 0,
        btnCrear = $("#btnCrear"),
        btnActualizar = $("#btnActualizar"),
        btnAgregar = $("#btnAgregar"),
        txtTitulo = $("#txtTitulo"),
        txtDescripcion = $("#txtDescripcion"),
        txtComentario = $("#txtComentario"),
        txtPregunta = $("#txtPregunta"),
        Preguntas = $(".Preguntas"),
        hideActualizar = $(".hideActualizar"),
        cboDept = $("#cboDept"),
        selectTipoPregunta = $("#selectTipoPregunta");

        function init() {


            cboDept.fillCombo('/Encuestas/Encuesta/cboDepartamentos', null, true);

            Preguntas.on("click", ".Eliminar", fnEliminarPregunta);
            Preguntas.on("change", ".txtPregunta", fnActualizarPregunta);
            btnActualizar.click(fnEncuestaToSave);
            btnAgregar.click(fnBtnAgregar);
   
            setDepartamentoUsaurio();
            cargarEncuesta();
        }

        function setDepartamentoUsaurio() {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Encuestas/Encuesta/setDepartamentoUsuario",
                asyn: false,
                success: function (response) {

                    cboDept.val(response.departamentoId);
                    //cboDept.prop('disabled', !response.permisoAdmin);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function validarCantidadEncuestas() {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Encuestas/Encuesta/validarCantidadEncuestas",
                asyn: false,
                success: function (response) {
                    if (response.success === false) {
                        ConfirmacionGeneral("Alerta", "¡Este departamento ya cuenta con una encuesta creada!");
                        setInterval(function () { window.location.href = "/Encuestas/Encuesta/Dashboard"; }, 2000);
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function fnBtnAgregar() {
            if (txtPregunta.val() != '') {
                if (selectTipoPregunta.val() != '') {
                    //var valores = [];
                    //$.each($(Preguntas).find('.tipoPregunta').val(), function (i, e) {
                    //    valores.push($(Preguntas).find('.tipoPregunta')[i].val());
                    //});
                    //if ((selectTipoPregunta.val() == 1 || selectTipoPregunta.val() == 2 || selectTipoPregunta.val() == 3) && $(Preguntas).find(".tipoPregunta").val()) {

                    //}

                    var pregunta = {};
                    pregunta.id = 0;
                    pregunta.encuestaID = _encuestaID;
                    pregunta.pregunta = txtPregunta.val();
                    pregunta.tipo = selectTipoPregunta.val();

                    var pregunta = fnAddPregunta(txtPregunta.val(), 0, "Agregar", selectTipoPregunta.val());
                    $(pregunta).appendTo(Preguntas);
                    $('.starrr').starrr({
                        readOnly: true
                    })
                    txtPregunta.val("");

                    $(Preguntas).find(".tipoPregunta").last().val(selectTipoPregunta.val());

                    selectTipoPregunta.val("");
                    txtPregunta.focus();
                } else {
                    AlertaGeneral("Alerta", "¡Debe seleccionar un tipo de pregunta para poder agregar!");
                }
            }
            else {
                AlertaGeneral("Alerta", "¡Debe escribir una pregunta para poder agregar!");
            }
            //$.ajax({
            //    datatype: "json",
            //    type: "POST",
            //    url: "/Encuestas/Encuesta/savePregunta",
            //    data: { obj: pregunta },
            //    asyn: false,
            //    success: function (response) {
            //        var pregunta = fnAddPregunta(txtPregunta.val(), response.id);
            //        $(pregunta).appendTo(Preguntas);
            //        $('.starrr').starrr({
            //            readOnly: true
            //        })
            //        txtPregunta.val("");
            //    },
            //    error: function () {
            //        $.unblockUI();
            //    }
            //});
        }
        function fnActualizarPregunta() {
            var pregunta = {};
            pregunta.id = Number($(this).data("id"));
            pregunta.encuestaID = _encuestaID;
            pregunta.pregunta = $(this).val();

            if (pregunta.id > 0) {
                $(this).data("estatus", "Actualizar");
            }


            //$.ajax({
            //    datatype: "json",
            //    type: "POST",
            //    url: "/Encuestas/Encuesta/savePregunta",
            //    data: { obj: pregunta },
            //    asyn: false,
            //    success: function (response) {
            //    },
            //    error: function () {
            //        $.unblockUI();
            //    }
            //});
        }
        function fnEliminarPregunta() {
            var _this = $(this).parent().parent().parent().parent().parent();
            var _thisInput = $(this).parent().parent().parent().parent().parent().find(".txtPregunta")[0];

            $(_thisInput).data("estatus", "Eliminar");
            _this.hide();
            //$.ajax({
            //    datatype: "json",
            //    type: "POST",
            //    url: "/Encuestas/Encuesta/delPregunta",
            //    data: { id: id },
            //    asyn: false,
            //    success: function (response) {
            //        _this.remove();
            //    },
            //    error: function () {
            //        $.unblockUI();
            //    }
            //});
        }
        function fnAddPregunta(text, id, estatus, tipo) {
            var html = '<div class="row Pregunta">';
            html += '    <div class="col-lg-12">';
            html += '        <div class="row">';
            html += '            <div class="col-lg-12">';
            html += '                <div class="input-group">';
            html += '                    <span class="input-group-addon">Pregunta:</span>';
            html += '                    <div style="border:1px dotted gray;height: 32px;">';
            html += '                        <div class="starrr"></div>';
            html += '                       <button class="btn btn-sm btn-danger Eliminar pull-right" data-id="' + id + '">Eliminar</button>';
            html += '                    </div>';
            html += '                </div>';
            html += '            </div>';
            html += '        </div>';
            html += '        <div class="row">';
            html += '            <div class="col-lg-12">';
            html += '                <textarea class="form-control txtPregunta" placeholder="Pregunta" data-calificacion="0" data-estatus="' + estatus + '" data-id="' + id + '">' + text + '</textarea>';
            html += '            </div>';
            html += '        </div>';
            html += '        <div class="row">';
            html += '           <div class="col-lg-2">';
            html += '               <div class="input-group">';
            html += '                   <span class="input-group-addon">Tipo de Pregunta:</span>';
            html += '                   <select class="form-control tipoPregunta" data-id="' + tipo + '">';
            html += '                       <option value="">--Seleccione--</option>';
            html += '                       <option value="2">Calidad</option>';
            html += '                       <option value="1">Tiempo</option>';
            html += '                       <option value="3">Atención</option>';
            html += '                       <option value="4">Otro</option>';
            html += '                   </select>';
            html += '               </div>';
            html += '           </div>';
            html += '        </div>';
            html += '    </div>';
            html += '</div>';
            return html;
        }
        function fnEncuestaToSave() {
            if (txtTitulo.val() != '') {
                $.ajax({
                    url: "/Encuestas/Encuesta/getEncuestasTodasByDeptoConEncuestaID",
                    data: { id: _encuestaID },
                    asyn: false,
                    success: function (response) {
                        var encuestas = response.obj;

                        //var tipoExistente = false;

                        //for (i = 0; i < encuestas.length; i++) {
                        //    if (((encuestas[i].tipo == 1 && document.getElementById('radioSatClienteInterno').checked) ||
                        //        (encuestas[i].tipo == 2 && document.getElementById('radioSatClienteExterno').checked)) &&
                        //        encuestas[i].id != _encuestaID) {
                        //        tipoExistente = true;
                        //    }
                        //}

                        //if (tipoExistente == false) {
                        var obj = {};
                        obj.id = _encuestaID;
                        obj.titulo = txtTitulo.val();
                        obj.descripcion = txtDescripcion.val();

                        if (document.getElementById('radioSatClienteInterno').checked) {
                            obj.tipo = 1;
                        } else {
                            if (document.getElementById('radioSatClienteExterno').checked) {
                                obj.tipo = 2;
                            } else {
                                obj.tipo = 0;
                            }
                        }

                        obj.preguntas = new Array();
                        obj.departamentoID = cboDept.val();
                        var preguntas = $(".txtPregunta");

                        var flagVacio = true;
                        var flagRepetido = true;
                        var flag2 = false;
                        var flag1 = false;
                        var flag3 = false;
                        $.each(preguntas, function (i, e) {
                            var o = {};
                            o.id = $(e).data("id");
                            o.encuestaID = _encuestaID;
                            o.pregunta = $(e).val();
                            o.estatus = $(e).data("estatus");
                            o.visible = true;
                            o.orden = i + 1;
                            var numeroTipo = $(Preguntas).find(".Pregunta .tipoPregunta:eq(" + i + ")").val();
                            if (numeroTipo != "") {
                                if ((numeroTipo == "1" && flag1 == true) || (numeroTipo == "2" && flag2 == true) || (numeroTipo == "3" && flag3 == true)) {
                                    flagRepetido = false;
                                    return false;
                                } else {
                                    switch (numeroTipo) {
                                        case "1":
                                            flag1 = true;
                                            break;
                                        case "2":
                                            flag2 = true;
                                            break;
                                        case "3":
                                            flag3 = true;
                                            break;
                                        default:
                                            break;
                                    }
                                    o.tipo = parseInt(numeroTipo);
                                }
                            } else {
                                flagVacio = false;
                                return false;
                            }

                            if ((_encuestaID == 0 && o.estatus != 'Eliminar') || _encuestaID > 0) {
                                obj.preguntas.push(o);
                            }
                        });
                        if (flagVacio == true) {
                            if (flagRepetido == true) {
                                $.ajax({
                                    datatype: "json",
                                    type: "POST",
                                    url: "/Encuestas/Encuesta/saveEncuesta",
                                    data: { obj: obj },
                                    asyn: false,
                                    success: function (response) {
                                        _encuestaID = Number(response.id);
                                        btnCrear.hide();
                                        hideActualizar.show();
                                        if (obj.id == 0) {
                                            ConfirmacionGeneral("Confirmación", "¡Encuesta creada y enviada a revisión correctamente!");
                                        }
                                        else {
                                            ConfirmacionGeneral("Confirmación", "¡Encuesta actualizada y enviada a revisión correctamente!");
                                            setInterval(function () { window.location.href = "/Encuestas/Encuesta/Dashboard"; }, 2000);
                                        }
                                    },
                                    error: function () {
                                    }
                                });
                            } else {
                                AlertaGeneral('Alerta', 'No se permite repetir los tipos Calidad,Tiempo y Atención. Seleccione el tipo "Otro"');
                            }
                        } else {
                            AlertaGeneral("Alerta", "No se permiten preguntas sin su tipo.");
                        }
                        //} else {
                        //    AlertaGeneral("Alerta", 'Sólo se permite una encuesta de tipo "Satisfacción de Cliente Interno" o "Satisfacción de Cliente Externo" por departamento. Seleccione la opción "Otros".')
                        //}
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        //alert('error');
                    }
                });

            } else {
                ConfirmacionGeneral("Validación", "¡Debes de darle un título a tu encuesta!");
            }
        }

        function cargarEncuesta() {
            var id = $.urlParam('encuesta');
            if (id != null) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/Encuesta/getEncuestaByID",
                    data: { id: id },
                    asyn: false,
                    success: function (response) {
                        var obj = response.obj;
                        _encuestaID = obj.id;
                        txtTitulo.val(obj.titulo);
                        txtDescripcion.val(obj.descripcion);
                        txtPregunta.val("");
                        cboDept.val(obj.departamentoID);


                        switch (obj.tipo) {
                            case 1:
                                document.getElementById('radioSatClienteInterno').checked = true;
                                break;
                            case 2:
                                document.getElementById('radioSatClienteExterno').checked = true;
                                break;
                            case 0:
                                document.getElementById('radioSatOtros').checked = true;
                                break;
                            default:
                                document.getElementById('radioSatOtros').checked = true;
                                break;
                        }

                        $.each(obj.preguntas, function (i, e) {
                            var pregunta = fnAddPregunta(e.pregunta, e.id, "Agregado");
                            $(pregunta).appendTo(Preguntas);

                            $(Preguntas).find(".Pregunta .tipoPregunta:eq(" + i + ")").val(e.tipo);

                            //$('.starrr').starrr({
                            //    rating: i.calificacion
                            //})
                            $('.starrr').starrr({
                                readOnly: true
                            })
                        });
                        btnCrear.hide();
                        hideActualizar.show();
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            else {
                //validarCantidadEncuestas();
                btnCrear.hide();
                hideActualizar.show();
            }
        }
        init();
    };

    $(document).ready(function () {
        encuestas.crear = new crear();
    });
})();


