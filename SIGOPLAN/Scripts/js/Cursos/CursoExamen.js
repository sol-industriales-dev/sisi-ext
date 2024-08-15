$(function () {
    $.namespace('Cursos.CursoExamen');
    cursos = function () {
        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        /**variables 16/1/18 */
        cboCurso = $("#cboCurso");
        paginadorModulo = $("#paginadorModulo");
        description = $("#description");
        arrModulo=[];
        pagModulo = [];
        author = $("#author");
        fechaCurso = $("#fechaCurso");
        nombreModulo = $("#nombreModulo");
        moduloDescripcion = $("#moduloDescripcion");
        folio = $("#folio");
        tablaCurso = $("#tablaCurso");
        btndetalle = $("button#btndetalle");
        var idmod = 0;
        btnIngresar = $("#btnIngresar");
        modalCurso = $("#modalCurso");
        carrusel = $(".stepwizard");
        arrPagDet = [];//22/1/18 15:17 raguilar
        btnsiguiente = $("#btnsiguiente");
        contenidopag = $("#contenidopag");
        btnAtras=$("#btnAtras");
        step = $("#step");
        var paginado;//toma  el contenido de las paginas del modulo
        var idactual;//id del paginado
        var numPag;//numero de pagina activa
        function init() {
            cboCurso.fillCombo('/Curso/Curso/FillComboCurso', { est: true }, false, "Seleccionar Curso");
            $(cboCurso).change(ConsultaModulos);            
            $(document).on('click', '#btndetalle', function () {
                idmod = $(this).attr('id-modulo');
                GeneradorMod(idmod);
                //window.location = "..../#nombreModulo";
            });
            $(document).ready(function () {
                $("#cboCurso").prop("selectedIndex", 1);
                ConsultaModulos();
            });
            $(document).on('click', '#btnIngresar', function () {
                idmod = $(this).attr('id-modulo');
                modalCurso.modal('show');
            });
            modalCurso.on('shown.bs.modal', function () {
                DibujandoSlidesCurso();//asociacion de id y paginado 
                
            });
            modalCurso.on('hide.bs.modal', function () {
                contenidopag.empty();
                step.empty();
            });
            btnsiguiente.click(DibujandoSlidesCurso2);
            btnAtras.click(retroceso);
        };

        //raguilar 23/1/18 09:23am
        function retroceso() {
            paginado = $(carrusel).find('div.stepwizard-step')
            //longpaginado = paginado.length;
            idactual = paginado.find(".active").attr('id');
            numPag = paginado.find(".active").html();
            numPag = parseInt(numPag);
            numPag = (numPag - 1);
            if (numPag>=0) {
                paginado.find(".active").removeClass("active");
                paginado.each(function (indexpag, valorpag, arraypag) {
                    if (numPag == indexpag) {
                        $(valorpag).find('a').addClass("active");
                    }
                });
                pagModulo.forEach(function (valor, i, array) {
                    if (i == numPag) {
                        contenidopag.empty();
                        contenidopag.append(valor.contenido)
                    }
                });
            }
        }
        function DibujandoSlidesCurso2() {
            paginado = $(carrusel).find('div.stepwizard-step')
            idactual= paginado.find(".active").attr('id');
            numPag = paginado.find(".active").html();
            longpaginado = paginado.length;
     
            numPag = parseInt(numPag);
            numPag = (numPag + 1);
            if (numPag <= (longpaginado-1)) {
                paginado.find(".active").removeClass("active");
                paginado.each(function (indexpag, valorpag, arraypag) {
                    if (numPag == indexpag) {
                        $(valorpag).find('a').addClass("active");
                    }
                });
                pagModulo.forEach(function (valor, i, array) {
                    if (i == numPag) {
                        contenidopag.empty();
                        contenidopag.append(valor.contenido)
                    }
                });
            }
        };
        
        function DibujandoSlidesCurso() {
            $(carrusel).attr('id-modulo', idmod);
            //22/1/18 rrecorrer el arreglo y quitar el disabled a los pasos
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/curso/curso/ObtenerPagbyId',
                data: { id: idmod },
                async: false,
                success: function (response) {
                    //pagModulo = response.objModuloDet.length;
                    pagModulo = response.objModuloDet;
                    response.objModuloDet.forEach(function (valor, i, array) {
                        if (i==0) {
                            paginado = "<div class='stepwizard-step'>" +
                               "<a  id='" + valor.id + "' disabled type='button' class='btn btn-circle btn-default btn-primary item active'>" + i  + "</a>" +
                          //"<p>Pagina " + (i + 1) + "</p>" +
                          "</div>"
                            step.append(paginado);
                            contenidopag.append(valor.contenido)
                        } else {
                            paginado = "<div class='stepwizard-step'>" +
                                                          "<a id='" + valor.id + "' disabled  type='button' class='btn btn-circle btn-default btn-primary'>" + i + "</a>" +
                                                     "</div>"
                            step.append(paginado);
                        }
                    });
                }
            });
        };


        function GeneradorMod(idmod) {
            arrModulo.forEach(function (valor, indice, array) {
                if (idmod==valor.idModulo) {
                    nombreModulo.html("Nombre Modulo: " + valor.nombremodulo);
                    moduloDescripcion.html("Descripcion Del Modulo: " + valor.descripcion);
                    btnIngresar.attr('id-modulo', valor.idModulo);
                }
            });
        }  
        function ConsultaModulos() {
            //$("#tablaCurso >tbody").empty();
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/curso/curso/ObtenerCursobyId',
                data: { id: cboCurso.val() },
                async: false,
                success: function (response) {
                    if (response.success) {
                        description.val(response.objCurso[0].descripcion);
                        author.html("Creador: " + response.objCurso[0].nomUsuarioCap);
                        folio.html("folio: " + response.objCurso[0].folio);
                        FormateoFechaEdicion(response.objCurso[0].fecha);
                        response.objModulo.forEach(function (valor, indice, array) {
                            if(indice==0){
                                nombreModulo.html("Nombre Modulo: "+valor.nombreModulo);
                                moduloDescripcion.html("Descripcion Del Modulo: "+valor.descripcion);
                                btnIngresar.attr('id-modulo', valor.id);
                            }
                            objModulo = { id: (indice + 1), idModulo: valor.id, nombremodulo: valor.nombreModulo, descripcion: valor.descripcion };
                            arrModulo.push(objModulo);

                        });
                        arrPagDet.push(response.objModuloDet);
                        TablillaRelacion(response.objModuloDet)
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        //tabla relacion modulos paginas.
        var PlantillaModPag;
        function TablillaRelacion(objModuloDet) {
            arrModPag = [];
            idtmpMod = 0
            contar = 0;
            bandera = false;
            objModuloDet.forEach(function (valorDet, indiceDet, arrayDet) {
                if (bandera == false) {
                    idtmpMod = valorDet.idModulo;
                    bandera = true;
                }
                if (valorDet.idModulo == idtmpMod) {
                    contar += 1;
                    if (objModuloDet.length == (indiceDet + 1)) {
                        idtmpMod = valorDet.idModulo;
                        objModpag = { idmodulo: idtmpMod, pagina: contar };
                        arrModPag.push(objModpag);
                    }
                } else {
                    objModpag = { idmodulo: idtmpMod, pagina: contar };
                    arrModPag.push(objModpag);
                    contar = 0;
                    if (objModuloDet.length == (indiceDet + 1)) {
                        idtmpMod = valorDet.idModulo;
                        objModpag = { idmodulo: idtmpMod, pagina: 1 };
                        arrModPag.push(objModpag);
                    } else {
                        contar += 1;
                        bandera = false;
                    }
                }
            });
            baderaarrModulo = false;
            idtmparrModulo = 0;
            arrModulo.forEach(function (valorMod, indiceMod, arrayMod) {
                contadorarrModPag = 0;
                longarrModPag = arrModPag.length;
                arrModPag.forEach(function (valorDet, indiceDet, arrayDet) {
                    contadorarrModPag += 1;
                    if (valorMod.idModulo == valorDet.idmodulo) {
                        PlantillaModPag = "<tr>"
                                           + "<td>"
                                           + indiceDet
                                           + "</td>"
                                           + "<td>"
                                           + valorMod.nombremodulo
                                           + "</td>"
                                           + "<td>"
                                           + valorDet.pagina
                                           + "</td>"
                                           + "<td>"
                                           + "<a href='#nombreModulo'>"
                                           +"<button type='button' src='nombreModulo' style='margin-top:15px; margin-left:8px;' id='btndetalle' id-modulo='" + valorDet.idmodulo + "' > <span  class='fa fa-graduation-cap'></span></button>" 
                                           + "</a>" 
                                           + "</td>"
                                     + "</tr>";
                        tablaCurso.append(PlantillaModPag);
                    }
                });
            });
        }
        function FormateoFechaEdicion(Fecha) {
            var dateString = Fecha.substr(6);
            var currentTime = new Date(parseInt(dateString));
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var date = day + "/" + month + "/" + year;
            fechaCurso.html(date);
 
        }
        init();
    }
    $(document).ready(function () {

        cursos.cursos = new cursos();
    });
});

