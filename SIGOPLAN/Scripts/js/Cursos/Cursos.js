
$(function () {
    $.namespace('Cursos.Cursos');

    /*Variables raguilar 05/01/18*/
    cursos = function () {
        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        var objA;//data Curso
        var objB;//data Modulo
        var objC;//data paginas
        var objD;//data Contenido
        var objE;//data paginas+contenido
        ArregloB = [];
        ArregloC = [];
        ArregloD = [];
        ArregloE = [];//arreglo Pagina Modulo+Contenido
        var idmodalC;
        var idpaginaC;
        var ModalidBEdit;//variable de asociacion en caso de edicion de curso para relacionar pagina nueva a modulo a editar
        var ideliminamodulo;//variable id modulo eliminado
        var flagCargaupdate = false;
        var BanderaFocus = false;
        //raguilar 26/1/18/12:17pm
        arraylstEliminaModDet = [];//arreglo a eliminar al actualizar aditiva1
        arraylstEliminaMod = [];//arreglo a eliminar  aditivaDet
        //var longtabablaB = $("#tablaB >tbody >tr");
        btndet = $("#btndet");
        btndropB = $('#btndropB');
        btnA = $("#btnA");//Guardar Todo
        btnBAgregar = $("#btnBAgregar");//Agregar Modulo
        tablaB = $("#tablaB");//tabla Modulo
        btnAgregarC = $("#btnAgregarC");
        modalC = $("#modalC");
        btnEditB = $('#btnEditB');
        tablaModalC = $('#tablaModalC');
        modalD = $("#modalD");
        tituloD = $("#tituloD");
        tituloC = $("#tituloC");
        btnCancelarC = $("#btnCancelarC");
        btnModalAceptarEliminar = $("#btnModalAceptarEliminar");
       
        /***id**/
        var ModalidB;
        summernote = $("#summernote");
        fechaIniCurso = $("#fechaIniCurso");
        nombreCurso = $("#nombreCurso");
        descripcionCurso = $("#descripcionCurso");
        //cursofolio = $("#cursofolio");
        /* Coola*/
        var Bandera;
        var plantillaB = "<tr>"
                  + "<td id='NombreB'>"
                              + "<textarea  style='width:100%'></textarea></td>"
                  + "</td>"
                  + "<td id='DescripcionModuloB'>"
                      + "<textarea style='width:100%'></textarea></td>"
                  + "</td>"
                  + "<td style='text-align:center !important'><button type='button' class='btn btn-default' id='btnEditB'> <span class='fa fa-pencil-square-o'></span></button></td>"
                   + "<td align='center'><button type='button' class='btn btn-default' id='btndet'> <span class='fa fa-eye'></span></button></td>"
                  + "<td align='center'><button type='button' class='btn btn-danger' id='btndropB'> <span class='glyphicon glyphicon-trash'></span></button></td>"
              + "</tr>";
        var plantillaModulo = "<tr>"
                + "<td id='pagina'>"
                      + "<input type='text' disabled  class='form-control' />"
                + "</td>"
                + "<td id='descripcionC'>"
                    + "<textarea  style='width:100%'></textarea></td>"
                + "</td>"
                + "<td><button type='button'class='btn btn-default' id='btnEditC'> <span class='fa fa-pencil-square-o'></span></button></td>"
                + "<td><button type='button'class='btn btn-danger' id='btndropC'> <span class='glyphicon glyphicon-trash'></span></button></td>"
            + "</tr>";

        function init() {
            $('#tablaB').on('focus', 'tr td #btndropB', function (evt) {
                EliminadoModal("Alerta", "Se perdera la informacion Relacionada Al Modulo ¿Desea Continuar?","btnModalB");
            });

            $('#tablaModalC').on('focus', 'tr td #btndropC', function (evt) {
                BanderaFocus = true;
                EliminadoModal("Alerta", "Se perdera la informacion Relacionada A la Pagina ¿Desea Continuar?","btnModalC");
            });    
            $(document).on('click', '#btnModalC', function () {
                BanderaFocus = false;
                $("#btndropC").click();
            });

            var hoy = new Date();
            var dd = hoy.getDate();
            var mm = hoy.getMonth() + 1;
            var yyyy = hoy.getFullYear();
            fechaIniCurso.datepicker().datepicker("setDate", dd + "/" + mm + "/" + yyyy);
            $(fechaIniCurso).attr('disabled', true);
            summernote.summernote({
                height: 400,
                minHeight: null,
                maxHeight: null,
                focus: false,
                lang: 'es-ES',
                toolbar: [
                     ["style", ["style"]],
                     ["font", ["bold", "underline", "clear"]],
                     ["fontname", ["fontname"]],
                     ["color", ["color"]],
                     ["para", ["ul", "ol", "paragraph"]],
                     ["table", ["table"]],
                     ["insert", ["picture"]],
                     //["insert", ["link", "picture", "video"]],
                     ["view", ["fullscreen", "help"]],
                     //["view", ["fullscreen", "codeview", "help"]]
                ],
                disableResizeEditor: true,
            });

            function EliminadoB() {
                btndropB.click();
            }
            $("#modalD").on("show.bs.modal", function () {
                summernote.summernote('code', '');
                ////rrecorrer el arreglo D y  asignar valor al summer note cunado sea debido 25/1/18/10:40
                ArregloD.forEach(function (valorD, indiceD, arrayD) {
                    if (valorD.idmodulo == idmodalC && valorD.idpagina == idpaginaC) {
                        summernote.summernote('code', valorD.contenido);
                    }
                });
            });

            $(document).on('click', '#btndropC', function () {
                if (BanderaFocus== false) {
                    $(this).closest('tr').remove();
                    idpaginaEdit = $(this).attr("id-edit");
                    var longitud = $("#tablaModalC >tbody >tr").length;
                    PaginaIdModuloId(longitud);
                    if (idpaginaEdit != undefined) {
                        objModElimina = { id: idpaginaEdit };
                        arraylstEliminaModDet.push(objModElimina);
                        //sacar del arreglo todos los detalles relacionados
                        longArregloD = ArregloD.length;
                        for (var i = 0; i < longArregloD; i++) {
                            ArregloD.forEach(function (itemD, indexD, objectD) {
                                if (itemD.idpaginaEdit == idpaginaEdit) {
                                    objectD.splice(indexD, 1);
                                }
                            });
                        }
                        //refactorizacion
                        ArregloD.forEach(function (itemD, indexD, objectD) {
                            itemD.idpagina = indexD
                        });
                        longArregloC = ArregloC.length;
                        for (var i = 0; i < longArregloC; i++) {
                            ArregloC.forEach(function (itemC, indexC, objectC) {
                                if (itemC.idpaginaEdit == idpaginaEdit) {
                                    objectC.splice(indexC, 1);
                                }
                            });
                        }
                        //refactorizacion
                        ArregloC.forEach(function (itemC, indexC, objectC) {
                            itemC.idpagina = indexC
                        });
                    }
                }
            });
            $(document).on('click', '#btnModalB', function () {
                $("#btndropB").click();
            });
            $(document).on('click', '#btndropB', function () {
                $(this).closest('tr').remove();
                eliminadoTipoB();
                //agregar al arreglo eliminar  26/1/18 12_26pm
                ideliminamodulo = $(this).attr("id-edit");
                if (ideliminamodulo != undefined) {
                    objModElimina = { id: ideliminamodulo };
                    arraylstEliminaMod.push(objModElimina);
                    //sacar del arreglo todos los detalles relacionados
                    longArregloD = ArregloD.length;
                    for (var i = 0; i < longArregloD; i++) {
                        ArregloD.forEach(function (itemD, indexD, objectD) {
                            if (itemD.idmoduloEdit == ideliminamodulo) {
                                objectD.splice(indexD, 1);
                            }
                        });
                    }
                    longArregloC = ArregloC.length;
                    for (var i = 0; i < longArregloC; i++) {
                        ArregloC.forEach(function (itemC, indexC, objectC) {
                            if (itemC.idmoduloEdit == ideliminamodulo) {
                                objectC.splice(indexC, 1);
                            }
                        });
                    }
                }
            });
            //raguilar 1/2/18 
            btnBAgregar.click();
            $(document).on('click', '#btnEditB', function () {
                ModalidB = $(this).closest('tr').attr('id-Modulo');
                if (idCurso > 0) {
                    ModalidBEdit = $(this).attr("id-edit");
                    //$(renglon).find("#btnEditC").attr("id-edit"
                }
                //validar que el modulo actual tenga nombre  y descipcion
                //tomar el id del botn
                nombreB = $('#tablaB tr[id-modulo="' + ModalidB + '"]').find('td#NombreB textarea').val();
                DescripcionModuloB = $('#tablaB tr[id-modulo="' + ModalidB + '"]').find('td#DescripcionModuloB textarea').val();
                if (nombreB == "") {
                    AlertaGeneral('Alerta', 'Favor de Ingresar nombe de Modulo');
                } else if (DescripcionModuloB == "") {
                    AlertaGeneral('Alerta', 'Favor de Ingresar descripcion de Modulo');
                } else {
                    NombreB = $(this).closest('tr').find('td#NombreB textarea').val();
                    modalC.modal('show');
                    tituloC.text('Modulo ' + NombreB);
                }
            });
            //trabajo mostrado
            modalC.on('shown.bs.modal', function () {
                $("#tablaModalC >tbody").attr("id-modulo", ModalidB);
                //AgregaPagC();
                //Mostrado de valores raguilar 24/1/18
                ArregloC.forEach(function (valorD, indiceD, arrayD) {
                    if (valorD.idmodulo == ModalidB) {
                        AgregaPagC();
                        $('tr[id-pagina="' + valorD.idpagina + '"]').find('td#descripcionC textarea').val(valorD.descripcion);
                        $('tr[id-pagina="' + valorD.idpagina + '"]').find('td#input textarea').val(valorD.idpagina);
                    }
                });
                ColorArregloD();
            });
            btnAgregarC.click(AgregaPagC);
            $(document).on('click', '#btnEditC', function () {
                idmodalC = $(this).closest('tr').attr('id-modulo');
                idpaginaC = $(this).closest('tr').attr('id-pagina');
                var longitud = $("#tablaModal >tbody >tr").length;
                modalD.modal('show');
                $("#modalD").attr("id-modulo", idmodalC);
                $("#modalD").attr("id-pagina", idpaginaC);

                banderaAsignacion = false;//evalua
                ArregloC.forEach(function (valorD, indiceD, arrayD) {
                    if (valorD.idmodulo == idmodalC && valorD.estado == true) {
                        banderaAsignacion = true;
                        $("#modalD").attr("id-moduloEdit", valorD.idmoduloEdit);//modal attr edit 6/2/18 
                    }
                });
                if (banderaAsignacion==false) {
                    $("#modalD").removeAttr("id-moduloEdit");//modal attr edit 6/2/18 
                }

                nombreModuloB = $("#modalC").find('#tituloC').text();
                tituloD.text(nombreModuloB + '  Pagina  ' + (parseInt(idpaginaC) + 1));
            });
            $(document).on('click', '#btnAceptarD', function () {
                GuardarobjD();
            });
            $(document).on('click', '#btnAceptarB', function () {
                ValidacionArrayModPag(ModalidB);
                GuardarobjC();
                colorCompletoC();
            });
            $(document).on('mousedown', '#btnAceptarB', function () {
                var longitud = $("#tablaModalC >tbody >tr").length;
                var idModalPadre = $("#tablaModalC >tbody ").attr('id-modulo');
                for (var i = 0; i < longitud; i++) {
                    txtdescripcion = $('tr[id-pagina="' + i + '"]').find('td#descripcionC textarea').val();
                    if (longitud == (i + 1) && txtdescripcion == "") {
                        AlertaGeneral('Alerta', 'Agregue descripción a la pagina' + (i + 1));
                    }
                }
            });
            var banderaNuevoCurso = true;
            $(document).on('click', '#btnA', function () {
                if (idCurso > 0) {
                    PrepararObjetoEnvioEdicion();
                } else {
                    PrepararObjetoEnvio();
                }
                var longitud = $("#tablaB >tbody >tr").length;
                //raguilar modificcion de validcion modulo al querer enviar 1/2/18 09:12am
                if ($("#nombreCurso").val() == "") {
                    AlertaGeneral('Alerta', 'Favor de Ingresar nombe de al Curso');
                } else if ($("#descripcionCurso").val() == "") {
                    AlertaGeneral('Alerta', 'Favor de Ingresar descripcion al Curso');
                } else if (longitud == 0) {
                    AlertaGeneral('Alerta', 'Favor de Ingresar Modulo');
                } else {
                    $.blockUI({ message: mensajes.Enviando });
                    $.ajax({
                        url: "/curso/curso/saveCurso",
                        type: 'POST',
                        async: false,
                        dataType: 'json',
                        data: { objCurso: objA, lstobjModulo: ArregloB, lstobjModuloDet: ArregloE, NuevoCurso: banderaNuevoCurso, arraylstEliminaModDet: arraylstEliminaModDet, arraylstEliminaMod: arraylstEliminaMod },
                        success: function (data) {
                            VaciadoObjetos();
                            $.unblockUI();
                            if (data.modificacion == true) {
                                ConfirmacionGeneralFC("Confirmación", "Se a modifico el folio " + data.folio, "bg-green");
                            } else {
                                ConfirmacionGeneralFC("Confirmación", "Se a guardado el folio " + data.folio, "bg-green");
                            }
                        }, error: function () {
                            $.unblockUI();
                        }
                    });
                }
            });
            $(document).on('click', '#btndet', function () {
                MetodoDetalle();
            });
            idCursoUpdate();
            idCurso = $.get("id");
            if (idCurso > 0) {
                ObtenerCursobyId(idCurso);
            }
            $(document).on('click', '#btndrop', function () {
                $(this).closest('tr').remove();
            });
        
            $(document).on('click', '#btnCancelarPadre', function () {
                $("#tablaModal >tbody").empty();
            });
            $("#myModal").on("hidden.bs.modal", function () {
                summernote.summernote('code', " ");
            });
            btnCancelarC.click(cancelarC);
            btnBAgregar.click(Modulo);
            $(document).ready(function () {
                flagCargaupdate = true;
            });
        };//fin init
        function MetodoDetalle() {
            alert('prueba')
        }
        function eliminadoTipoB() {
            longArregloC = ArregloC.length;
            for (var i = 0; i < longArregloC; i++) {
                ArregloC.forEach(function (itemC, indexC, objectC) {
                    if (itemC.idmodulo == idmodalC) {
                        objectC.splice(indexC, 1);
                    }
                });
            }
            longArregloD = ArregloD.length;
            for (var i = 0; i < longArregloD; i++) {
                ArregloD.forEach(function (itemD, indexD, objectD) {
                    if (itemD.idmodulo == idmodalC) {
                        objectD.splice(indexD, 1);
                    }
                });
            }
            ColorArregloC();
            ModuloId($("#tablaB >tbody >tr").length);
        }
        function cancelarC() {
            $("#tablaModalC >tbody ").empty();
        };
        /*inicio funciones*/
        //validacion interaccion 25/1/18 09:12am
        function ValidacionArrayModPag(ModalidB) {
            var longArregloC = ArregloC.length;
            for (var i = 0; i < longArregloC; i++) {
                ArregloC.forEach(function (item, index, object) {
                    if (item.idmodulo == ModalidB) {
                        object.splice(index, 1);
                    }
                });
            }

        }
        //raguilar  Agregar Plantilla A tabla B
        function Modulo() {
            //ajustes de correcion alertas de agregado 31/1/12 12:18pm "no agregar modulos sin agregar nombre y descripcion de curso"
            var NombreCat = $('#nombreCurso').val();
            var Descripcion = $('#descripcionCurso').val();
            if (NombreCat == "" || Descripcion == "") {
                if (NombreCat == "") {
                    AlertaGeneral('Alerta', 'Favor de Tipear el nombre del modulo');
                } else if (Descripcion == "") {
                    AlertaGeneral('Alerta', 'Favor de Tipear la descripción del modulo');
                }
            } else {
                //validacion 12:43pm rrecorrer renglones si no se le a indicado nombre al modulo ni descipcion mandar una alerta
                if (idCurso == null) {
                    ModuloIdAgregado();
                } else {
                    if (flagCargaupdate == false) {
                        tablaB.append(plantillaB);
                    }
                    ModuloIdAgregado();
                }
            }
        }
        function ModuloIdAgregado() {//modificacion moduloid//raguilar 31/1/18 12.48pm "nuevas modificaciones"
            longitud = $("#tablaB >tbody >tr").length;
            if (longitud > 0) {
                for (var i = 0; i <= longitud; i++) {
                    //renglon = $("" + tablaB + " >tbody >tr")[i];
                    renglon = $("#tablaB >tbody >tr")[i]
                    //$(renglon).attr("id-modulo", i);

                    //var a = $(renglon).find('#btndropB').hide();
                    if ((longitud - 1) == i) {
                        if (flagCargaupdate == true) {//reglas para cuando cargue la pagina
                            nombreB = $(renglon).find('td#NombreB textarea').val();
                            DescripcionModuloB = $(renglon).find('td#DescripcionModuloB textarea').val();
                            if (nombreB == "") {
                                AlertaGeneral('Alerta', 'Agregue nombre al modulo ' + (i + 1));
                            } else if (DescripcionModuloB == "") {
                                AlertaGeneral('Alerta', 'Agregue descripción al modulo ' + (i + 1));
                            } else {
                                tablaB.append(plantillaB);
                            }
                        }
                        //var b = $(renglon).find('#btndropB').css("display", "block");
                    }
                };
            } else {
                tablaB.append(plantillaB);//hace la inserccion cuando no hay modulos registrdos aun
            }
            ModuloId(longitud);
        };
        //raguilar 7 agregar modalid B
        function ModuloId(longitud) {
            for (var i = 0; i <= longitud; i++) {
                renglon = $("#tablaB >tbody >tr")[i]
                $(renglon).attr("id-modulo", i);//agrega id al modulo
            };
        };
        //raguilar  Agregar Pagina Modal C
        function AgregaPagC() {
            PaginaIdModuloIdAgregado(longitud);
        }
        //ragular asignacion de idmodal padre paginaid por renglon y valor default por renglon
        function PaginaIdModuloIdAgregado(longitud) {
            var longitud = $("#tablaModalC >tbody >tr").length;
            if (longitud > 0) {
                for (var i = 0; i <= longitud; i++) {
                    renglon = $("#tablaModalC >tbody >tr")[i]
                    $(renglon).attr("id-pagina", i);
                    var idModalPadre = $("#tablaModalC >tbody ").attr('id-modulo')
                    $(renglon).attr("id-modulo", idModalPadre);
                    $('tr[id-pagina="' + i + '"]').find('td#pagina input').val(i);
                    //var a = $(renglon).find('#btndropC').hide();
                    if ((longitud - 1) == i) {
                        if (flagCargaupdate == true) {//reglas para cuando cargue la pagina
                            descripcionPag = $(renglon).find('td#descripcionC textarea').val();
                            if (descripcionPag == "") {
                                AlertaGeneral('Alerta', 'Agregue descripción a la pagina' + (i + 1));
                            } else {
                                tablaModalC.append(plantillaModulo);
                            }
                        }
                    }
                };
            } else {
                tablaModalC.append(plantillaModulo);
            }
            PaginaIdModuloId(longitud);
        };
        function PaginaIdModuloId(longitud) {
            for (var i = 0; i <= longitud; i++) {
                renglon = $("#tablaModalC >tbody >tr")[i]
                $(renglon).attr("id-pagina", i);
                var idModalPadre = $("#tablaModalC >tbody ").attr('id-modulo')
                $(renglon).attr("id-modulo", idModalPadre);
                $('tr[id-pagina="' + i + '"]').find('td#pagina input').val((i + 1));
            };
        };
        function GuardarobjD() {
            idmodulo = $("#modalD").attr("id-modulo");
            idpagina = $("#modalD").attr("id-pagina");
            idmoduloEdit= $("#modalD").attr("id-moduloEdit");
            contenido = summernote.summernote('code');
            if (contenido != "") {//evita que se colore el estatus
                objD = {
                    idmodulo: idmodulo,
                    idpagina: idpagina,
                    contenido: contenido,
                    idmoduloEdit: idmoduloEdit,
                    estado: false
                };
                var banderaNuevo = false;
                //sacar el elemento antiguo 30/1/18 14:50pm
                ArregloD.forEach(function (valorD, indiceD, arrayD) {
                    if (valorD.idmodulo == idmodulo && valorD.idpagina == idpagina) {//solo habra una pagina y para el mismo modulo
                        valorD.contenido = contenido;
                        banderaNuevo = true;
                    }
                });
                if (banderaNuevo == false) {
                    ArregloD.push(objD);
                }
                modalD.modal('hide');
                summernote.summernote('code', " ");
                ColorArregloD();
            }
        };
        //raguilar color  tabla modal c "paginas"
        function ColorArregloD() {
            ArregloD.forEach(function (valorD, indiceD, arrayD) {
                var longitud = $("#tablaModalC >tbody >tr").length;
                for (var i = 0; i < longitud; i++) {
                    var renglon = $("#tablaModalC  >tbody >tr")[i];
                    var idPagina = $(renglon).attr('id-pagina')
                    if (valorD.idpagina == idPagina && valorD.idmodulo == ModalidB && valorD.contenido != null) {
                        FlagColorArregloD = true;
                        var e = $(renglon).find("#btnEditC").css("background-color", "#ffa700");
                    }
                    //asociar el id-edit al boton en caso de actualizacon de pagina
                    if (idCurso != "" && valorD.idmodulo == ModalidB &&  valorD.idpagina==i) {
                      
                        var edicion = $(renglon).find("#btnEditC").attr("id-edit", valorD.idpaginaEdit);
                        var elimina = $(renglon).find('#btndropC').attr('id-edit', valorD.idpaginaEdit);
                    }
                }
            });
        }
        function GuardarobjC() {
            var longitud = $("#tablaModalC >tbody >tr").length;
            var idModalPadre = $("#tablaModalC >tbody ").attr('id-modulo');
            for (var i = 0; i < longitud; i++) {
                //var tmpEstado = false;
                var renglon = $("#tablaModalC  >tbody >tr")[i];
                renglon = $("#tablaModalC >tbody >tr")[i]
                $(renglon).attr("id-pagina", i);
                var idModalPadre = $("#tablaModalC >tbody ").attr('id-modulo');
                if (idCurso > 0 && ModalidBEdit != undefined) {
                    //botonEdit = $('#tablaB tr[id-modulo="' + i + '"]').find('#btnEditC').attr('id-edit');
                    botonEdit = $(renglon).find("#btnEditC").attr("id-edit");
                    tmp = true;//
                    if (botonEdit == undefined) {
                        tmp = false
                    }
                    $(renglon).attr("id-modulo", idModalPadre);
                    objC = {
                        idmodulo: idModalPadre,//modulo al que se le agregara una pagina
                        idpagina: i,
                        descripcion: $('tr[id-pagina="' + i + '"]').find('td#descripcionC textarea').val(),
                        idmoduloEdit: ModalidBEdit,
                        estado: tmp
                    }
                } else {
                    $(renglon).attr("id-modulo", idModalPadre);
                    objC = {
                        idmodulo: idModalPadre,
                        idpagina: i,
                        descripcion: $('tr[id-pagina="' + i + '"]').find('td#descripcionC textarea').val(),
                        estado: false
                        //estado: true prueba 311/1/18 
                    }
                }
                ArregloC.push(objC);
            }
            $("#tablaModalC >tbody ").empty();
            ColorArregloC();
        }
        //raguilar 23/1/18 1612 coloreo de modulos con paginas
        function ColorArregloC() {
            ArregloC.forEach(function (valorC, indiceC, arrayC) {
                var longitud = $("#tablaB >tbody >tr").length;
                for (var i = 0; i < longitud; i++) {
                    var renglon = $("#tablaB  >tbody >tr")[i];
                    var idModal = $(renglon).attr('id-modulo')
                    if (valorC.idmodulo == idModal) {
                        var e = $(renglon).find("#btnEditB").css("background-color", "#ffa700");
                        ArregloD.forEach(function (valorD, indiceD, arrayD) {
                            for (var a = 0; a < longitud; a++) {
                                var idPagina = $(renglon).attr('id-pagina')
                                var idDet = $(renglon).find("#btndet").attr('data-id', a);
                            }
                        });
                    }
                }
            });
        }
        function colorCompletoC() {
            ArregloC.forEach(function (valorC, indiceC, arrayC) {
                var longitud = $("#tablaB >tbody >tr").length;
                for (var i = 0; i < longitud; i++) {
                    var renglon = $("#tablaB  >tbody >tr")[i];
                    var idModal = $(renglon).attr('id-modulo')
                    var flagModal = false;
                    if (valorC.idmodulo == idModal) {
                        ArregloD.forEach(function (valorD, indiceD, arrayD) {
                            for (var a = 0; a < longitud; a++) {
                                var idPagina = $(renglon).attr('id-pagina')
                                var idDet = $(renglon).find("#btndet").attr('data-id', a);
                                if (valorD.idmodulo == idModal && valorD.contenido != null) {
                                    flagModal == true;
                                    var f = $(renglon).find("#btndet").css("background-color", "#ffa700");
                                }
                            }
                        });
                    }
                }
            });
        }   
        function PrepararObjetoEnvioEdicion() {
            //ObtenerArregloBEdit()
            /*objA objeto Curso**/
            objA = {
                nombreCurso: nombreCurso.val(),
                fecha: fechaIniCurso.val(),
                descripcion: descripcionCurso.val(),
                id: idCurso,//se anexa el id del curso a editiar 
                folio: $("input").attr('id-folio')//agregado de folio al input
            };
            /** arreglo C + ArregloD =  arreglo  E**/
            longArregloD = ArregloD.length;
            ArregloC.forEach(function (valorC, indiceC, arrayC) {
                var flagObjetoEnvioEdicion = false;
                var flagObjetoEnvioEdicion = false;//cambia si encuentra un contenido asociado a la pagina
                if (ArregloD != 0) {
                    //ajuste validacion  valorD.idmodulo 6/2/18 utlimo
                    ArregloD.forEach(function (valorD, indiceD, arrayD) {
                        if (valorC.idpagina == valorD.idpagina && valorC.idmodulo == valorD.idmodulo && valorC.estado == false && valorD.estado == false && (valorD.idmoduloEdit == undefined || valorD.idmoduloEdit < 0 || valorD.idmoduloEdit != "") && (valorD.idpagina == undefined || valorD.idpagina < 0 || valorD.idmoduloEdit != "") && valorD.idmoduloEdit == undefined) {
                            flagObjetoEnvioEdicion = true;
                            objE = {
                                id: 0,
                                pagina: valorD.idpagina,
                                descripcion: valorC.descripcion,
                                contenido: valorD.contenido,
                                idmodulo: valorD.idmodulo,
                                estado: valorC.estado
                            }
                            ArregloE.push(objE);//vaciar al finalizar el ajax
                            ArregloD.splice(indiceD, 1);
                        } else if (valorC.idpagina == valorD.idpagina && valorC.idmodulo == valorD.idmodulo && valorC.estado == false && valorD.estado == false && (valorD.idmoduloEdit == undefined || valorD.idmoduloEdit < 0 || valorD.idmoduloEdit != "") && (valorD.idpagina == undefined || valorD.idpagina < 0 || valorD.idmoduloEdit != "") && valorD.idmoduloEdit != undefined) {
                            flagObjetoEnvioEdicion = true;
                            objE = {
                                id: 0,
                                pagina: valorD.idpagina,
                                descripcion: valorC.descripcion,
                                contenido: valorD.contenido,
                                idmodulo: valorD.idmoduloEdit,
                                estado: valorC.estado
                            }
                            ArregloE.push(objE);//vaciar al finalizar el ajax
                            ArregloD.splice(indiceD, 1);
                        }
                        if (valorC.idpagina == valorD.idpagina && valorC.idmoduloEdit == valorD.idmoduloEdit && valorD.estado == true) {
                            flagObjetoEnvioEdicion = true;
                            objE = {
                                id: valorD.idpaginaEdit,
                                pagina: valorD.idpagina,
                                descripcion: valorC.descripcion,
                                contenido: valorD.contenido,
                                idmodulo: valorD.idmoduloEdit,
                                //idModulo: valorD.idmodulo,
                                estado: valorC.estado
                            }
                            ArregloE.push(objE);//vaciar al finalizar el ajax
                            ArregloD.splice(indiceD, 1);
                        } else if (valorC.idpagina == valorD.idpagina && valorC.idmodulo == valorD.idmodulo && (valorC.idmoduloEdit < 0 || valorC.idmoduloEdit == undefined || valorC.estado == false) && flagCargaupdate == false) {
                            flagObjetoEnvioEdicion = true;
                            objE = {
                                idModulo: valorD.idmodulo,
                                pagina: valorD.idpagina,
                                descripcion: valorC.descripcion,
                                contenido: valorD.contenido,
                                estado: false
                            }
                            flagObjetoEnvioEdicion = true
                            ArregloE.push(objE);//vaciar al finalizar el ajax
                            ArregloD.splice(indiceD, 1);
                        } else if (longArregloD == (indiceD + 1) && flagObjetoEnvioEdicion == false) {
                            flagObjetoEnvioEdicion = true;
                            PaginasNoContenido(valorC);
                            //flagObjetoEnvioEdicion =true
                        }
                        longArregloD = ArregloD.length;
                    });
                } else {
                    PaginasNoContenido(valorC);
                    //ArregloE
                }
            });
            //Array.prototype.unique(ArregloE);
            ObtenerArregloBEdit();
        }
        function ObtenerArregloBEdit() {
            var longitud = $("#tablaB >tbody >tr").length;
            for (var i = 0; i < longitud; i++) {
                botonEdit = $('#tablaB tr[id-modulo="' + i + '"]').find('#btnEditB').attr('id-edit');
                //nombreB = $('#tablaB tr[id-modulo="' + i + '"]').find('td#NombreB input').val(); // ajuste tipo text 31/1/18 12:33
                nombreB = $('#tablaB tr[id-modulo="' + i + '"]').find('td#NombreB textarea').val();
                DescripcionModuloB = $('#tablaB tr[id-modulo="' + i + '"]').find('td#DescripcionModuloB textarea').val();
                if (botonEdit == undefined) {
                    botonEdit = i;
                    estado = false;
                } else {
                    estado = true;
                }

                objB = {
                    nombreModulo: nombreB,
                    descripcion: DescripcionModuloB,
                    id: botonEdit,//edit modulo id,
                    estado: estado
                };
                ArregloB.push(objB);
            };
        };
        function ValidacionModuloUltimo() {
            var longitudtablaB = $("#tablaB >tbody >tr").length;
            if (longitudtablaB > 0) {//validar de no guardar nombre y descripcion del modulo en cero
                longitud = $("#tablaB >tbody >tr").length;
                if (longitud > 0) {
                    banderaUltimo = false;//bandera para indicar si se agrego un  modulo y este tiene unespacio en blanco
                    for (var i = 0; i <= longitud; i++) {
                        renglon = $("#tablaB >tbody >tr")[i]
                        if ((longitud - 1) == i) {
                            nombreB = $(renglon).find('td#NombreB textarea').val();
                            DescripcionModuloB = $(renglon).find('td#DescripcionModuloB textarea').val();
                            if (nombreB == "") {
                                AlertaGeneral('Alerta', 'Agregue nombre al modulo ' + (i + 1));
                                banderaUltimo == true;
                            } else if (DescripcionModuloB == "") {
                                AlertaGeneral('Alerta', 'Agregue descripción al modulo ' + (i + 1));
                                banderaUltimo == true;
                            }
                        }
                    };
                }
            }
        }
        var banderaUltimo;
        function PrepararObjetoEnvio() {
            if (idCurso>0) {
                banderaUltimo = ValidacionModuloUltimo();
            } else {
                banderaUltimo = false;
            }
            if (banderaUltimo == false) {
                logArregloD = ArregloD.length;
                /*objA objeto Curso**/
                objA = {
                    nombreCurso: nombreCurso.val(),
                    fecha: fechaIniCurso.val(),
                    descripcion: descripcionCurso.val()
                };
                /** arreglo C + ArregloD =  arreglo  E**/
                ArregloC.forEach(function (valorC, indiceC, arrayC) {
                    var badenraA = false;//cambia si encuentra un contenido asociado a la pagina
                    if (ArregloD != 0) {
                        ArregloD.forEach(function (valorD, indiceD, arrayD) {
                            if (valorC.idpagina == valorD.idpagina && valorC.idmodulo == valorD.idmodulo) {
                                badenraA = true;//pudo hacer la insercccion en el arreglo
                                objE = {
                                    idModulo: valorD.idmodulo,
                                    pagina: valorD.idpagina,
                                    descripcion: valorC.descripcion,
                                    contenido: valorD.contenido
                                }
                                ArregloE.push(objE);//vaciar al finalizar el ajax
                            }
                            if (badenraA == false && logArregloD == (indiceD + 1)) {
                                PaginasNoContenido(valorC);
                                badenraA = true;//pudo hacer la insercccion en el arreglo
                            }
                        });
                    } else {
                        PaginasNoContenido(valorC);
                    }
                });
                ObtenerArregloB();
            }
        }
        function PaginasNoContenido(valorC) {//en caso de los elementos pagina que no contengan un contenido relacionado
            if (idCurso > 0) {
                idtmpmodulo = 0;
                idtdet = 0;
                if (valorC.idmoduloEdit == undefined) {
                    idtmpmodulo = valorC.idmodulo;
                    id = 0;
                } else {
                    idtmpmodulo = valorC.idmoduloEdit;
                    id = valorC.idmodulo;
                }
                objE = {
                    id: idtdet,
                    idmodulo: idtmpmodulo,
                    pagina: valorC.idpagina,
                    descripcion: valorC.descripcion,
                    contenido: "",
                    estado: valorC.estado
                }
            } else {
                objE = {
                    id: valorC.idmodulo,
                    idmodulo: valorC.idmodulo,
                    pagina: valorC.idpagina,
                    descripcion: valorC.descripcion,
                    contenido: "",
                    estado: false
                }
            }
            ArregloE.push(objE);//vaciar al finalizar el ajax
        }
        function VaciadoObjetos() {
            objA = {};
            objB = {};
            objC = {};
            objE = {};
            ArregloB = [];
            ArregloC = [];
            ArregloD = [];
            ArregloE = [];
            nombreCurso.val('');
            descripcionCurso.val('');
            $("#tablaB >tbody >tr").empty();
        }
        function ObtenerArregloB() {
            var longitud = $("#tablaB >tbody >tr").length;
            for (var i = 0; i < longitud; i++) {
                //nombreB = $('#tablaB tr[id-modulo="' + i + '"]').find('td#NombreB input').val();//ajustes tipo textarea 31/1/18 12:23pm
                nombreB = $('#tablaB tr[id-modulo="' + i + '"]').find('td#NombreB textarea').val();
                DescripcionModuloB = $('#tablaB tr[id-modulo="' + i + '"]').find('td#DescripcionModuloB textarea').val();
                objB = {
                    nombreModulo: nombreB,
                    descripcion: DescripcionModuloB,
                    id: i
                };
                ArregloB.push(objB);
            };
        };
        function rrecorrerModulo(longitud) {
            for (var i = 0; i <= longitud; i++) {
                NombreModulo = $('#tblPlantilla tr[id-renglon="' + i + '"]').find('td#NombreModulo input').val();
                Descripcion = $('#tblPlantilla tr[id-renglon="' + i + '"]').find('td#DescripcionModulo textarea').val();
                //idModulo= $('#tblPlantilla tr[id-renglon="' + i + '"]').va;
                objD = {
                    nombreModulo: NombreModulo,
                    descripcion: Descripcion,
                    idmodulo: i
                };
                ArregloModulo.push(objD);
            };
        };
        function ConfirmacionGeneralFC(titulo, mensaje, color) {
            if (!$("#dialogalertaGeneral").is(':visible')) {
                var html = '<div id="dialogalertaGeneral" class="modal fade" role="dialog">' +
                '<div class="modal-dialog">' +
                    '<div class="modal-content">' +
                        '<div class="modal-header text-center modal-lg">' +
                            '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
                                '&times;</button>' +
                            '<h4  class="modal-title">' + titulo + '</h4>' +
                        '</div>' +
                        '<div class="modal-body">' +
                                          '<div class="container">' +
                        '<div class="row">' +
                        '<div class="col-lg-12">' +
                            '<h3> <span class="glyphicon glyphicon-ok-circle ' + color + '" aria-hidden="true" style="font-size:40px;"></span> <label style="position: fixed;">' + mensaje + '</label></h3>' +
                        '</div>' +
                        '</div>' +
                      '</div>' +
                        '</div>' +
                        '<div class="modal-footer">' +
                            '<a id="btndialogalertaGeneral" href= "/curso/curso/gestorcursos" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
                        '</div>' +
                    '</div>' +
                '</div></div>';

                var _this = $(html);
                _this.modal("show");
            }

        }
        function idCursoUpdate() {
            (function ($) {
                $.get = function (key) {
                    key = key.replace(/[\[]/, '\\[');
                    key = key.replace(/[\]]/, '\\]');
                    var pattern = "[\\?&]" + key + "=([^&#]*)";
                    var regex = new RegExp(pattern);
                    var url = unescape(window.location.href);
                    var results = regex.exec(url);
                    if (results === null) {
                        return null;
                    } else {
                        return results[1];
                    }
                }
            })(jQuery);
        }
        function ObtenerCursobyId(idCurso) {
            var objEditar;
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/curso/curso/ObtenerCursobyId',
                data: { id: idCurso },
                async: false,
                success: function (response) {
                    if (response.success) {
                        $("input").attr('id-folio',response.objCurso[0].folio);//asociar folio 7/2/18
                        response.objCurso.nombreCurso;
                        FormateoFechaEdicion(response.objCurso[0].fecha);
                        //response.objModulo
                        //response.objModuloDet
                        //$("#cboCC option:selected").text(response.AditivaDeductiva[0].cC);
                        nombreCurso.val(response.objCurso[0].nombreCurso);
                        descripcionCurso.val(response.objCurso[0].descripcion);
                        //raguilar 25/1/18 17:14
                        //llenar mis objetos
                        prepararObjetoEditr(response.objModulo, response.objModuloDet);
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        //raguilar preparando edicion 25/1/8/ 17:31pm
        function prepararObjetoEditr(objModulo, objModuloDet) {
            objModulo.forEach(function (valorM, indiceM, arrayM) {
                Modulo();
                //nombreB = $('#tablaB tr[id-modulo="' + indiceM + '"]').find('td#NombreB input').val(valorM.nombreModulo);  ajuste tipo textarea 31/1/18:12:24pm
                nombreB = $('#tablaB tr[id-modulo="' + indiceM + '"]').find('td#NombreB textarea').val(valorM.nombreModulo);
                DescripcionModuloB = $('#tablaB tr[id-modulo="' + indiceM + '"]').find('td#DescripcionModuloB textarea').val(valorM.descripcion);
                botonEdit = $('#tablaB tr[id-modulo="' + indiceM + '"]').find('#btnEditB').attr('id-edit', valorM.id);
                botondrop = $('#tablaB tr[id-modulo="' + indiceM + '"]').find('#btndropB').attr('id-edit', valorM.id);
                //juego edicion 26/1/18 07.21am
                //+ "<td><button type='button' style='margin-top:15px; margin-left:8px;' id='btnEditB'> <span class='fa fa-pencil-square-o'></span></button></td>"
            });

            objModulo.forEach(function (valorM, indiceM, arrayM) {

                idPadre = 0;
                idComparativa = 0;
                banderaPadre = false;
                contador = 0;
                objModuloDet.forEach(function (valorMd, indiceMd, arrayMd) {
                    if (valorMd.idModulo == valorM.id) {
                        if (banderaPadre == false) {
                            idPadre = contador;
                            idComparativa = valorMd.idModulo;
                            banderaPadre = true;
                        } else if (valorMd.idModulo != idPadre && idComparativa != valorMd.idModulo) {
                            contador = contador + 1;
                            idPadre = contador;
                            banderaPadre = false;
                        }
                        objC = {
                            //idmodulo: idPadre,
                            idmodulo: indiceM,
                            idpagina: valorMd.pagina,
                            descripcion: valorMd.descripcion,
                            idmoduloEdit: valorMd.idModulo,
                            idpaginaEdit: valorMd.id,
                            estado: true
                        };
                        objD = {
                            idmodulo: indiceM,
                            //idmodulo: idPadre,
                            idpagina: valorMd.pagina,
                            contenido: valorMd.contenido,
                            idpaginaEdit: valorMd.id,
                            idmoduloEdit: valorMd.idModulo,
                            estado: true
                        };
                        ArregloD.push(objD);
                        ArregloC.push(objC);
                    }
                });
            });
            ColorArregloC();
        }
        function EliminadoModal(titulo, mensaje,btnAceptar) {
            if (!$("#modalEliminacion").is(':visible')) {
                var html = '<div id="modalEliminar" class="modal fade" role="dialog" data-backdrop="static">' +
                '<div class="modal-dialog modal-dialog-fix modal-md" >' +
                    '<div class="modal-content">' +
                        '<div class="modal-header text-center modal-bg">' +
                            '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
                                '&times;</button>' +
                            '<h4  class="modal-title">' + titulo + '</h4>' +
                        '</div>' +
                        '<div class="modal-body ajustar-texto">' +
                            '<h5 id="pMessage">' +
                            '</h5>' +
                            '<div class="row">' +
                            '<div id="icon" class="col-md-2">' +
                            '<span class="glyphicon glyphicon-warning-sign alert-warning-span" style="font-size:40px;" aria-hidden="true"></span>' +
                             '</div>' +
                                '<div class="col-md-10">' +
                                    '<h3>  ' + mensaje + '</h3>' +
                                '</div>' +
                            '</div>' +
                        '</div>' +
                        '<div class="modal-footer">' +
                            '<a data-dismiss="modal" id="' + btnAceptar + '" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
                            '<a data-dismiss="modal" id="btnCancelarEliminar" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-remove"></span> Cancelar</a>' +
                        '</div>' +
                    '</div>' +
                '</div></div>';

                var _this = $(html);
                _this.modal("show");
            }

        }
        function FormateoFechaEdicion(Fecha) {
            var dateString = Fecha.substr(6);
            var currentTime = new Date(parseInt(dateString));
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var date = day + "/" + month + "/" + year;
            fechaIniCurso.val(date);
        }
        init();
    }
    $(document).ready(function () {
        cursos.cursos = new cursos();
    });
});
