(function () {

    $.namespace('encuestas.EncuestasSubContratista');

    EncuestasSubContratista = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Crear encuestas de proveedores',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        _encuestaID = 0;
        txtTitulo = $("#txtTitulo"),
        txtDescripcion = $("#txtDescripcion"),
        radioSContinuadeProveedores = $("#radioSContinuadeProveedores"),
        radioSProveedoresServicios = $("#radioSProveedoresServicios"),
        txtPregunta = $("#txtPregunta"),
        Preguntas = $(".Preguntas"),
        selectTipoPregunta = $("#selectTipoPregunta"),
        btnAgregar = $("#btnAgregar"),
        btnActualizar = $("#btnActualizar");

        function init() {

            btnAgregar.click(fnBtnAgregar);
            btnActualizar.click(fnBntGuardar);
            var id = $.urlParam('encuesta');
            if (id != undefined) {
                fnLoadEncuesta();
            }

        }
        function fnBtnAgregar() {
            if (txtPregunta.val() != '') {
                if (selectTipoPregunta.val() != '') {
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
            html += '           <div class="col-lg-6">';
            html += '               <div class="input-group">';
            html += '                   <span class="input-group-addon">Tipo de Pregunta:</span>';
            html += '                   <select class="form-control tipoPregunta" data-id="' + tipo + '">';
            html += '                       <option value="">--Seleccione--</option>';
            html += '                       <option value="1">Calidad</option>';
            html += '                       <option value="5">Planeación/Programa</option>';
            html += '                       <option value="3">Facturación</option>';
            html += '                       <option value="6">Seguridad</option>';
            html += '                       <option value="7">Ambiental</option>';
            html += '                       <option value="8">Efectividad del Costo</option>';
            html += '                       <option value="9">Fuerza de Trabajo</option>';
            html += '                       <option value="10">Otro</option>';
            html += '                   </select>';
            html += '               </div>';
            html += '           </div>';
            html += '        </div>';
            html += '    </div>';
            html += '</div>';
            return html;
        }
        function fnBntGuardar() {
            tipoExistente = true;
            if (txtTitulo.val() != '') {
                var obj = {};
                obj.id = _encuestaID;
                obj.titulo = txtTitulo.val();
                obj.descripcion = txtDescripcion.val();

                if (document.getElementById('radioSubContratista').checked) {
                    obj.tipoEncuesta = 1;
                }

                var update = false;
                if (obj.id != 0) {
                    update = true;
                }
                var preguntasLista = new Array();
                var preguntas = $(".txtPregunta");

                var flagVacio = true;
                var flagRepetido = true;
                var flag2 = false;
                var flag1 = false;
                var flag3 = false;
                var flag4 = false;
                var flag5 = false;
                var flag6 = false;
                var flag7 = false;
                $.each(preguntas, function (i, e) {
                    var o = {};
                    o.id = $(e).data("id");
                    o.encuestaID = _encuestaID;
                    o.pregunta = $(e).val();
                    o.estatus = $(e).data("estatus");
                    o.visible = true;
                    var numeroTipo = $(Preguntas).find(".Pregunta .tipoPregunta:eq(" + i + ")").val();
                    if (numeroTipo != "") {
                        if ((numeroTipo == "1" && flag1 == true) || (numeroTipo == "2" && flag2 == true) || (numeroTipo == "3" && flag3 == true)
                            || (numeroTipo == "4" && flag4 == true) || (numeroTipo == "5" && flag5 == true) || (numeroTipo == "6" && flag6 == true)
                            || (numeroTipo == "7" && flag7 == true)) {
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
                                case "4":
                                    flag4 = true;
                                    break;
                                case "5":
                                    flag5 = true;
                                    break;
                                case "6":
                                    flag6 = true;
                                    break;
                                case "7":
                                    flag7 = true;
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
                        preguntasLista.push(o);
                    }
                });

                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/EncuestasSubContratistas/saveEncuesta",
                    data: { encuesta: obj, listObj: preguntasLista, updateInfo: update },
                    asyn: false,
                    success: function (response) {
                        _encuestaID = Number(response.id);

                        if (obj.id == 0) {
                            ConfirmacionGeneral("Confirmación", "¡Encuesta creada y enviada a revisión correctamente!");
                        }
                        else {
                            ConfirmacionGeneral("Confirmación", "¡Encuesta actualizada y enviada a revisión correctamente!");
                            setInterval(function () { window.location.href = "/Encuestas/EncuestasSubContratistas/Dashboard"; }, 2000);
                        }
                    },
                    error: function () {
                    }
                });
            } else {
                ConfirmacionGeneral("Validación", "¡Debes de darle un título a tu encuesta!");
            }
        }
        function fnLoadEncuesta() {

            var id = $.urlParam('encuesta');
            if (id != null) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/EncuestasSubContratistas/getEncuesta",
                    data: { encuestaID: id },
                    asyn: false,
                    success: function (response) {
                        var objPreguntas = response.preguntas;
                        _encuestaID = response.id;
                        txtTitulo.val(response.titulo);
                        txtDescripcion.val(response.descripcion);
                        txtPregunta.val("");

                        switch (response.tipoEncuesta) {
                            case 1:
                                document.getElementById('radioSubContratista').checked = true;
                                break;
                        }

                        $.each(objPreguntas, function (i, e) {
                            var pregunta = fnAddPregunta(e.pregunta, e.id, "Agregado");
                            $(pregunta).appendTo(Preguntas);

                            $(Preguntas).find(".Pregunta .tipoPregunta:eq(" + i + ")").val(e.tipo);
                            $('.starrr').starrr({
                                readOnly: true
                            })
                        });

                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            else {

            }
        }
        function fnEliminarPregunta() {
            var _this = $(this).parent().parent().parent().parent().parent();
            var _thisInput = $(this).parent().parent().parent().parent().parent().find(".txtPregunta")[0];

            $(_thisInput).data("estatus", "Eliminar");
            _this.hide();
        }
        init();

    }
    $(document).ready(function () {

        encuestas.EncuestasSubContratista = new EncuestasSubContratista();
    });
})();



