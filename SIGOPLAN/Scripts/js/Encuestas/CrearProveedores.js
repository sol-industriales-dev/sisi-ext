(function () {
    $.namespace('encuestas.EncuestasProveedores');
    EncuestasProveedores = function () {
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
            txtPregunta = $("#txtPregunta"),
            Preguntas = $(".Preguntas"),
            selectTipoPregunta = $("#selectTipoPregunta"),
            btnAgregar = $("#btnAgregar"),
            btnActualizar = $("#btnActualizar");
        tbPonderacion = $("#tbPonderacion");
        divTipoEncuesta = $('#divTipoEncuesta');
        const cboTipoEncuesta = new URL(window.location.origin + '/Encuestas/EncuestasProveedor/cboTipoEncuesta');
        function init() {
            setDivRadioTipoEnciesta();
            btnAgregar.click(fnBtnAgregar);
            btnActualizar.click(fnBntGuardar);
            var id = $.urlParam('encuesta');
            if (id != undefined) {
                fnLoadEncuesta();
            }
        }

        $('body').on('click', '.Eliminar', function() {
            console.log($(this).parent().parent().parent().parent().parent().parent());
            $(this).parent().parent().parent().parent().parent().parent().remove();
        });

        async function setDivRadioTipoEnciesta() {
            try {
                divTipoEncuesta.html("");
                response = await ejectFetchJson(cboTipoEncuesta);
                let { success, items } = response;
                if (success) {
                    items.forEach(tipo => {
                        let radio = $("<input>", {
                            type: "radio",
                            value: tipo.Value,
                            name: "tipoEncuesta"
                        });
                        let label = $("<label>", {
                            text: tipo.Text,
                            style: "font-weight: normal; margin-right: 5px;"
                        });
                        divTipoEncuesta.append(radio);
                        divTipoEncuesta.append(label);
                    });
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        function fnBtnAgregar() {
            if (txtPregunta.val() != '') {
                if (selectTipoPregunta.val() != '') {

                    if (fnValidarPonderacion()) {
                        var pregunta = {};
                        pregunta.id = 0;
                        pregunta.encuestaID = _encuestaID;
                        pregunta.pregunta = txtPregunta.val();
                        pregunta.tipo = selectTipoPregunta.val();
                        pregunta.ponderacion = tbPonderacion.val()

                        var pregunta = fnAddPregunta(txtPregunta.val(), 0, "Agregar", selectTipoPregunta.val(),);
                        $(pregunta).appendTo(Preguntas);
                        $('.starrr').starrr({
                            readOnly: true
                        })
                        txtPregunta.val("");

                        $(Preguntas).find(".tipoPregunta").last().val(selectTipoPregunta.val());
                        $(Preguntas).find(".tipoPonderacion").last().val(tbPonderacion.val());

                        selectTipoPregunta.val("");
                        txtPregunta.focus();
                        tbPonderacion.val('');
                    }

                } else {
                    AlertaGeneral("Alerta", "¡Debe seleccionar un tipo de pregunta para poder agregar!");
                }
            }
            else {
                AlertaGeneral("Alerta", "¡Debe escribir una pregunta para poder agregar!");
            }
        }

        function fnValidarPonderacion() {
            ponderaciones = $(".tipoPonderacion")
            var contador = 0;
            $.each(ponderaciones, function (i, e) {

                var valorActual = $(e).val();
                contador += Number(valorActual);
            });

            contador += Number(tbPonderacion.val());
            if (contador > 1) {
                AlertaGeneral("Alerta", "¡La ponderacion Total debe ser de 1!");
                return false;

            }
            else {
                return true;
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
            html += '           <div class="col-lg-4">';
            html += '               <div class="input-group">';
            html += '                   <span class="input-group-addon">Tipo de Pregunta:</span>';
            html += '                   <select class="form-control tipoPregunta" data-id="' + tipo + '">';
            html += '                       <option value="">--Seleccione--</option>';
            html += '                       <option value="1">Calidad</option>';
            html += '                       <option value="2">Tiempo</option>';
            html += '                       <option value="3">Facturación</option>';
            html += '                       <option value="4">Atención</option>';
            html += '                       <option value="10">Otro</option>';
            html += '                   </select>';
            html += '               </div>';
            html += '           </div>';
            html += '           <div class="col-lg-4">';
            html += '               <div class="input-group">';
            html += '                   <span class="input-group-addon">Ponderacion:</span>';
            html += '                   <input class="form-control tipoPonderacion">';
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
                let listaTipoPreguntasRepetidas = new Array();
                let ponderacionTotal = 0;
                var obj = {};
                obj.id = _encuestaID;
                obj.titulo = txtTitulo.val();
                obj.descripcion = txtDescripcion.val();
                if (document.querySelectorAll("input[name='tipoEncuesta']:checked").length == 0) {
                    AlertaGeneral('Aviso', 'Seleccione un tipo de encuesta');
                    return false;
                }
                obj.tipoEncuesta = +document.querySelectorAll("input[name='tipoEncuesta']:checked")[0].value;
                var update = false;
                if (obj.id != 0) {
                    update = true;
                }
                var preguntasLista = new Array();
                var preguntas = $(".txtPregunta");
                var ponderaciones = $(".tipoPonderacion");

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
                    var numeroTipo = $(Preguntas).find(".Pregunta .tipoPregunta:eq(" + i + ")").val();
                    var ponderacion = $(ponderaciones[i]).val();
                    o.ponderacion = ponderacion;
                    ponderacionTotal += +o.ponderacion;
                    if (numeroTipo != "" && numeroTipo != undefined && numeroTipo != null) {
                        if ((numeroTipo == "1" && flag1 == true) || (numeroTipo == "2" && flag2 == true) || (numeroTipo == "3" && flag3 == true)) {
                            o.tipo = parseInt(numeroTipo);
                            listaTipoPreguntasRepetidas.push(o);
                            flagRepetido = false;
                            //return false;
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
                            flagRepetido = true;
                        }
                    } else {
                        flagVacio = false;
                        AlertaGeneral('Seleccione un tipo de pregunta en la pregunta: ' + o.pregunta);
                        return false;
                    }

                    if (((_encuestaID == 0 && o.estatus != 'Eliminar') || (_encuestaID > 0)) && flagRepetido) {
                        preguntasLista.push(o);
                    }
                });

                console.log(ponderacionTotal);
                if (ponderacionTotal != 1) {
                    AlertaGeneral('Alerta', `Total de ponderación de: ${ponderacionTotal}. La ponderación debe ser de 1`);
                    return false;
                }

                if (listaTipoPreguntasRepetidas.length > 0) {
                    let mensajeRepetidos = 'Los siguientes tipos de pregunta estan repetidos: ';
                    
                    listaTipoPreguntasRepetidas.forEach((value, index, array) => {
                        let mensajeError = $('.Pregunta .tipoPregunta:eq(0)').find('option[value="'+value.tipo+'"]').text();
                        if (array.length > 1 && index != 0) {
                            if (index == (array.length - 1)) {
                                mensajeRepetidos += ' y ' + mensajeError;
                            } else {
                                mensajeRepetidos += ', ' + mensajeError;
                            }
                        } else {
                            mensajeRepetidos += mensajeError;
                        }
                    });
                    AlertaGeneral('Alerta', mensajeRepetidos + '.');
                    return false;
                }

                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/EncuestasProveedor/saveEncuesta",
                    data: { encuesta: obj, listObj: preguntasLista, updateInfo: update },
                    asyn: false,
                    success: function (response) {
                        _encuestaID = Number(response.id);

                        if (obj.id == 0) {
                            ConfirmacionGeneral("Confirmación", "¡Encuesta creada y enviada a revisión correctamente!");
                        }
                        else {
                            ConfirmacionGeneral("Confirmación", "¡Encuesta actualizada y enviada a revisión correctamente!");
                            setInterval(function () { window.location.href = "/Encuestas/EncuestasProveedor/Dashboard"; }, 2000);
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
                    url: "/Encuestas/EncuestasProveedor/getEncuesta",
                    data: { encuestaID: id },
                    asyn: false,
                    success: function (response) {
                        var objPreguntas = response.preguntas;
                        _encuestaID = response.id;
                        txtTitulo.val(response.titulo);
                        txtDescripcion.val(response.descripcion);
                        txtPregunta.val("");
                        $(`input[name="tipoEncuesta" value="${response.tipoEncuesta}"]`).prop("checked", true);
                        $.each(objPreguntas, function (i, e) {
                            var pregunta = fnAddPregunta(e.pregunta, e.id, "Agregado");
                            $(pregunta).appendTo(Preguntas);
                            $(Preguntas).find(".Pregunta .tipoPregunta:eq(" + i + ")").val(e.tipo);
                            $(Preguntas).find(".Pregunta .tipoPonderacion:eq(" + i + ")").val(e.ponderacion);

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

        encuestas.EncuestasProveedores = new EncuestasProveedores();
    });
})();



