$(function () {
    $.namespace('Cursos.AsugnacionCursos');
    cursos = function () {
        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        /**variables 12/1/18 */
        txtTitulo=$("#txtTitulo");
        txtDescripcion=$("#txtDescripcion");
        btnGuardar = $("#btnGuardar");
        txtRespuesta= $("#txtRespuesta");
        var pregunta;
        var incizos;
        arrobjMultiple = [];
        objMultiple = {};
        txtPregunta = $('#txtPregunta');
        BanderaaTipos = false;
        Preguntas = $(".Preguntas"),
        btnAgregar = $("#btnAgregar");
        cboCurso = $("#cboCurso");
        fechaIni = $("#fechaIni");
        b1 = $("#b1");
        tpReac = $("#tpReac");
        preguntaAbierta = $(".pregunta");
        txtMultiple = $("#txtMultiple");
        var letras = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Ñ', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
        var plantilla = "<tr>"
             + "<td id='opcion'>"
                 + "<input type='text' 'class='form-control' />"
             + "</td>"
             + "<td id='DescripcionMultiple'>"
                 + "<textarea style='width:100%'></textarea></td>"
             + "</td>"
             + "<td id='correcto'>"
             + "<input type='checkbox' name='vehicle'>"
             + "</td>"
             + "<td><button type='button' style='margin-top:15px; margin-left:8px;' id='btndropB'> <span class='glyphicon glyphicon-trash'></span></button></td>"
         + "</tr>";
        tablaMultiple = $("#tablaMultiple");
        function init() {
            var hoy = new Date();
            var dd = hoy.getDate();
            var mm = hoy.getMonth() + 1;
            var yyyy = hoy.getFullYear();
            cboCurso.fillCombo('/Curso/Curso/FillComboCurso', { est: true }, false, "Seleccionar Curso");
            fechaIni.datepicker().datepicker("setDate", dd + "/" + mm + "/" + yyyy);
            tpReac.change(function () {
                var TipoReactivo = $("#tpReac option:selected").val();//campo principales
                //$("#tpReac option:selected"), Text();//campo principales
                if (TipoReactivo == 1) {
                    b1.attr('disabled', false);
                    txtMultiple.show();
                    preguntaAbierta.hide();
                } else {
                    b1.attr('disabled', true);
                    preguntaAbierta.show();
                    txtMultiple.hide();
                    tablaMultiple.hide();
                }
            });
            b1.click(AgregaOpcion);
            btnAgregar.click(Agregarreactivo);
            btnGuardar.click(guardarObj);
        };
                    //raguilar 09:10 am
            //longitud de reactivos
            var  respuesCorrecta;
            paqueteopregunta = [];
            paqueterespuesta = [];
            objpregunta = {};
            objrespuesta = {};
        function guardarObj() {
            var pregunta = "";
            var longReactivo = $(".Preguntas .Pregunta").find('div').length
            var a = $(".Preguntas .Pregunta");
            $(a).each(function (index, element) {//enumera los reactivos
                $(element).attr('id-paquete', index);
            });
            paquete = $(".Preguntas .Pregunta");
            paquete.each(function (index, element) {
                tiporeactivo = $(element).find('div').attr("id-tipo");//se enumera paquete de reactivos renglon
                if (tiporeactivo == "abierta") {//es pregunta de tipo abierta
                    pregunta = $(element).find('textarea.txtPregunta').val();//se obtiene el valor de la pregunta
                    var idpregunta= $(element).find('textarea.txtPregunta').attr('id-tmp',index);//se le asisgna un id tempora
                    var respuesta = $(element).find('textarea#respuestaAb').val();//se obtiene la respuesta
                    var idrespuesta = $(element).find('textarea#respuestaAb').attr('id-tmp', index);//se le asigna un id temporal para ligar resp a pregunta
                    objpregunta = { pregunta: pregunta, id: $(idpregunta).attr('id-tmp'),abierta:true}
                    objrespuesta = { opcion: null, respuesta: respuesta, idpregunta: $(idrespuesta).attr('id-tmp'), correcta: true }
                    paqueteopregunta.push(objpregunta);
                    paqueterespuesta.push(objrespuesta);
                    objrespuesta={};
                    objpregunta={};
                } else {
                    pregunta = $(element).find('textarea.txtPregunta').val();//se obtiene el valor de la pregunta
                    var idpregunta = $(element).find('textarea.txtPregunta').attr('id-tmp', index);//se le asisgna un id tempora
                    objpregunta = { pregunta: pregunta, id: $(idpregunta).attr('id-tmp'), abierta:false }
                    id=$(idpregunta).attr('id-tmp')
                    paqueteopregunta.push(objpregunta);
                    var  descripcion="";
                    var longtable = $(element).find("table >tbody >tr").length
                    for (var i = 0; i < longtable; i++) {
                    var  renglon = $(element).find("table >tbody >tr")[i]
                    descripcion = $(renglon).find('td#DescripcionMultiple textarea').val();
                    opcion = $(renglon).find('td#opcion input').val();
                    correcto = $(renglon).find('td#correcto input');
                    if ((correcto).is(':checked')) {
                        respuesCorrecta = true;
                    }else  {
                        respuesCorrecta = false;
                    }                   
                    objrespuesta = { opcion: opcion, respuesta: descripcion, idpregunta: id, correcta: respuesCorrecta }
                    paqueterespuesta.push(objrespuesta);
                    objrespuesta={};
                    objpregunta={};
                    };
                }
            });
            EnvioExamen();
        }

        function  EnvioExamen(){
                objExamen={
                    idCurso:cboCurso.val(),
                    fecha:fechaIni.val(),
                    nombreExamen:txtTitulo.val(),
                    descripcion:txtDescripcion.val()}

            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/curso/curso/guardarexamen',
                data: { objExamen:objExamen ,lstobjExamenPregunta:paqueteopregunta, lstobjExamenRespuesta: paqueterespuesta,NuevoExamen:true},
                async: false,
                success: function (response) {
                    if (response.success) {
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function Agregarreactivo() {
            var TipoReactivo = $("#tpReac option:selected").val();//campo principales
            //$("#tpReac option:selected"), Text();//campo principales
            if (TipoReactivo == 1) {
                rrecorridoOpcionMultiple();
                pregunta = fnAddPregunta(txtPregunta.val(), 0, "Agregar");
                $(pregunta).appendTo(Preguntas);
                arrobjMultiple.forEach(function (valorop, indiceop, arrayop) {
                    respuesta = fnAddRespuesta(valorop.Opcion, valorop.Descripcion);
                });
                arrobjMultiple = [];
                objMultiple = {};
                $("#tablaMultiple >tbody >tr").detach();
                $("#tablaMultipleB >tbody").removeAttr("id")
            } else {
                pregunta = fnAddPreguntaAbierta(txtPregunta.val(), 0, "Agregar", txtRespuesta.val());
                $(pregunta).appendTo(Preguntas);
            }
        }
        function rrecorridoOpcionMultiple() {
            longitud = $("#tablaMultiple >tbody >tr").length;
            for (i = 0; i < longitud; i++) {
                renglon = $("#tablaMultiple >tbody >tr")[i]
                Opcion = $(renglon).find('td#opcion input').val();
                OpcionDesc = $(renglon).find('td#DescripcionMultiple textarea').val();
                objMultiple = { Opcion: Opcion, Descripcion: OpcionDesc };
                arrobjMultiple.push(objMultiple);
            }
        }
        function AgregaOpcion() {
            tablaMultiple.append(plantilla);
            longitud = $("#tablaMultiple >tbody >tr").length;
            for (j = 0; j < longitud; j++) {
                renglon = $("#tablaMultiple >tbody >tr")[j]
                $(renglon).find('td#opcion input').val(letras[j]);
                tablaMultiple.show();
                if (letras[j]=='Z') {
                    b1.attr('disabled', true);
                }
            }
        }   
        function fnAddPregunta(textpreg, id, estatus) {
                        var html = '<div class="Pregunta" id-paquete="' + id + '" ">';
                            html += '    <div class="col-lg-12">';
                            html += '                <div class="input-group">';
                            html += '                    <span class="input-group-addon">Pregunta:</span>';
                            html += '                    <div style="border:1px dotted gray;height: 32px;">';
                            html += '                       <button class="btn btn-sm btn-danger Eliminar pull-right" data-id="' + id + '">Eliminar</button>';
                            html += '                    </div>';
                            html += '                </div>';
                            html += '                <textarea class="form-control txtPregunta" placeholder="Pregunta" data-calificacion="0" data-estatus="' + estatus + '" data-id="' + id + '">' + textpreg + '</textarea>';
                            html += '        <span class="input-group-addon">Respuestas:</span>';
                            html +=         "<table class=' table table-condensed table-hover table-striped text-center table-responsive ' style='border:1px solid black; width:100%; margin-top:4px;' border='1' id='tablaMultipleB'>"
                            html +=         "<thead class='bg-table-header table-responsive' style='text-align:center'>"
                            html +=         "<tr>"
                            html +=             "<th rowspan='2' width='' class='col-lg-2'>Opcion</th>"
                            html +=             "<th rowspan='2'>Descripción</th>"
                            html +=             "<th rowspan='2' >Valida</th>"
                            html +=             "<th rowspan='2' class='col-lg-1'>Remover</th>"
                            html +=         "</tr>"
                            html +=         "</thead>"
                            html +=         "<tbody id='tblPlantillaB'></tbody>"
                            html +=             "</table>"
                            html +=         '</div>';
                            html += '</div>';
                return html;
        }
        function fnAddPreguntaAbierta(textpreg, id, estatus, txtRespuesta) {
            var html = '<div class="Pregunta" id-paquete="' + id + '">';
            html += '    <div class="col-lg-12" id-tipo="abierta">';
            html += '                <div class="input-group">';
            html += '                    <span class="input-group-addon">Pregunta:</span>';
            html += '                    <div style="border:1px dotted gray;height: 32px;">';
            html += '                       <button class="btn btn-sm btn-danger Eliminar pull-right" data-id="' + id + '">Eliminar</button>';
            html += '                    </div>';
            html += '                </div>';
            html += '                <textarea class="form-control txtPregunta" placeholder="Pregunta" data-calificacion="0" data-estatus="' + estatus + '" data-id="' + id + '">' + textpreg + '</textarea>';
            html += '        <span class="input-group-addon">Respuestas:</span>';
            html += '           <textarea style="width:100%" id="respuestaAb">' + txtRespuesta + '</textarea></td>'
            html += '</div>';
            html += '</div>';
            html += '</div>';
            html += '</div>';
            return html;
        }
        function fnAddRespuesta(incizo, descripcion) {
            Multipleb =   "<tr>"
                             + "<td id='opcion'>"
                             + "<input type='text' 'class='form-control' value='" + incizo + "' />"
                          + "</td>"
                          + "<td id='DescripcionMultiple'>"
                           + "<textarea style='width:100%'>"+ descripcion+ "</textarea></td>"
                          + "</td>"
                           + "<td id='correcto'>"
                            + "<input type='checkbox' name='vehicle'>"
                            + "</td>"
                           + "<td><button type='button' style='margin-top:15px; margin-left:8px;' id='btndropB'> <span class='glyphicon glyphicon-trash'></span></button></td>"
                       + "</tr>";
            $("#tblPlantillaB").append(Multipleb);
        }
        init();
    }
    $(document).ready(function () {
        cursos.cursos = new cursos();
    });
});

