$(function () {
    $.namespace('Cursos.Cursos');
    cursos = function () {
        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        //**variables**/
        chgEstado = $("#chgEstado");
        tblCursos = $("#tblCursos");
        btnAplicarFiltros = $("#btnAplicarFiltros");
        txtIdCurso = $("#txtIdCurso");
        txtNombreCurso = $("#txtNombreCurso");
        NumFolio = $("#NumFolio");
        modalDetCurso = $("#modalDetCurso");
        CuerpoModalDet = $(".CuerpoModalDet");
        tablaModulo = $("#tablaModulo");
        verPag = $(".verPag");
        arrayDet = [];
        modalPaginado = $("#modalPaginado");
        tablaModalPag = $("#tablaModalPag");
        summernote = $("#summernote");
        idTitulopag = $("#idTitulopag");
        titulocontenido = $("#titulocontenido");
        /*cursos**/
        var CursoPlantilla;
        function init() {
            summernote.summernote({
                height: 400,
                minHeight: null,
                maxHeight: null,
                focus: false,
                lang: 'es-ES',
                toolbar: [
                     //["style", ["style"]],
                     //["font", ["bold", "underline", "clear"]],
                     //["fontname", ["fontname"]],
                     //["color", ["color"]],
                     //["para", ["ul", "ol", "paragraph"]],
                     //["table", ["table"]],
                     //["insert", ["picture"]],
                     ////["insert", ["link", "picture", "video"]],
                     //["view", ["fullscreen", "help"]],
                     //["view", ["fullscreen", "codeview", "help"]]
                ],
                disableResizeEditor: true,
            });

            $(document).on('click', '.pagDet', function () {
                var nombreModulo = $(this).attr('id-pag');
                titulocontenido.text(idTitulopag.text());
                var idpag = $(this).attr('id-pag');
                var numpag = $(this).attr('num-pag');
                $("#paginaNum").empty();
                $("#paginaNum").append("Contenido Pagina:  " + numpag);
                arrayDet.forEach(function (valorM, indiceM, arrayM) {
                    if (idpag == valorM.id) {
                        $("#modalpag").modal("show");
                        summernote.summernote('code', valorM.contenido);
                    }
                })
                summernote.summernote('disable');
            });
            $("#modalDetCurso").on("hidden.bs.modal", function () {
                CuerpoModalDet.empty();
                tablaModulo.empty();
            });
            $("#modalPaginado").on("hidden.bs.modal", function () {
                $("#tblPlantilla").empty();
            });
            $(document).on('click', '.verPag', function () {
                var nombreModulo = $(this).attr('id-nombre');
                //alert(nombreModulo);;
                idTitulopag.text(nombreModulo)
                var idpag = $(this).attr('id-pag');
                paginadoMostrar(idpag);
            });
            $("#txtNombreCurso").attr('disabled', true);
            GetListCursos();
            btnAplicarFiltros.click(aplicarFiltro);
            $("#txtIdCurso").keypress(function (e) {
                if (e.which == 13) {
                    getInfoCurso();
                }
                else { txtIdCurso.value = "" }
            });
            function paginadoMostrar(idpag) {//ultimo
                arrayDet.forEach(function (valorM, indiceM, arrayM) {
                    if (valorM.idModulo == idpag) {
                        disabled = "";
                        tiporenglon = "";
                        estatus = "completo";
                        if (valorM.contenido == null || valorM.contenido == "") {
                            disabled = "disabled";
                            estatus = "En progreso";
                        }
                        if (disabled == "disabled") {
                            tiporenglon = "btn-default";
                        } else {
                            tiporenglon = "";
                            tiporenglon = "btn-primary";
                        }
                        renglonpagina =
                            //"<tr  class='" + tiporenglon + "'>"
                               "<tr>"
                                + "<td>"
                                        + "<input type='text' readonly   value='" + ((valorM.pagina) + 1) + "' />"
                                + "</td>"
                                + "<td id='descripcionC'>"
                                    + "<textarea readonly style='width:100%'>" + valorM.descripcion + "</textarea></td>"
                                + "</td>"
                                + "<td>"
                                + "<label> " + estatus + "</label>"
                                + "</td>"
                                + "<td>"
                                + "<button type='button' class='btn " + tiporenglon + " pagDet' " + disabled + "  num-pag ='" + ((valorM.pagina) + 1) + "'  id-pag='" + valorM.id + "'><span class='glyphicon glyphicon-list'></span>Detalle</button>"
                                + "</td>"
                            + "</tr>";
                        tablaModalPag.append(renglonpagina);
                        modalPaginado.modal("show");
                    }
                });
            }
            function getInfoCurso() {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    //url: "/Horometros/getCentroCostos",
                    url: "/Curso/Curso/GetNombreCurso",
                    data: { IdCurso: txtIdCurso.val() },
                    async: false,
                    success: function (response) {
                        $.unblockUI();
                        $("#txtNombreCurso").val(response.nombreCurso);
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            function aplicarFiltro() {
                var combo = $("#chgEstado option:selected").val();
                var id = 0;
                var folio = "";
                var nombre = "";
                var estado = 0;
                if (txtIdCurso.val() != "") {
                    id = txtIdCurso.val()
                }
                if (txtNombreCurso.val() != "") {
                    nombre = txtNombreCurso.val();
                }
                if (NumFolio.val() != "") {
                    folio = NumFolio.val();
                }
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: "/Curso/Curso/filtroCurso",
                    datatype: "html",
                    //(int id, string folio, string nombre, int estado) {
                    type: 'POST',
                    data: { id: id, folio: folio, nombre: nombre, combo: combo },
                    success: function (response) {
                        initTabla();
                        tblCursos.bootgrid("clear");
                        tblCursos.bootgrid("append", response);
                        $.unblockUI();
                    },
                    error: function (response) {
                        $.unblockUI();
                        alert(response.message);
                    }
                });
            }
            function initTabla() {
                tblCursos.bootgrid({
                    align: 'center',
                    selection: true,
                    labels:
                        {
                            infos: '{{ctx.total}} Cursos'
                        },
                    templates: {
                        search: ""
                    },
                    formatters: {
                        "btn-detalle": function (column, row) {
                            return "<button type='button' class='btn btn-primary verDet' data-id='" + row.id + "'>" +
                                   "<span class=' glyphicon glyphicon-list' style='margin-rigth:2px;'></span> " + "Detalle"
                        },
                        "btn-editar": function (column, row) {
                            var btnEditar = "<a class='btn btn-primary edit' data-toggle='collapse' data-id='" + row.id + "'>Editar</a>";
                            if (row.editable === true) {
                                return btnEditar;
                            } else { return ""; }
                            ;
                        },
                        "btn-eliminar": function (column, row) {
                            return "<button type='button' class='btn btn-danger  btnRemove' data-id='" + row.id + "'>" +
                                          "<span class='glyphicon glyphicon-trash'></span> " + "Eliminar" +
                                     " </button>"
                            ;
                        },
                        "completo": function (column, row) {
                            var completo = "";
                            if (row.completo === true) {
                                return completo = "<label data-id='" + row.id + "'>Completo</label>";
                            } else if (row.completo === false) {
                                //$(this).find('.edit');
                                return completo = "<label data-id='" + row.id + "'>En progreso</label>";
                            }
                        }
                    }

                }).on("loaded.rs.jquery.bootgrid", function () {
                    tblCursos.find(".btnRemove").on("click", function (e) {
                        var formatoid = $(this).attr("data-id");
                        Eliminar(formatoid);
                    });
                    tblCursos.find(".verDet").on("click", function (e) {
                        var formatoid = $(this).attr("data-id");
                        verDet(formatoid);
                    });
                    tblCursos.find(".edit").on("click", function (e) {
                        var formatoid = $(this).attr("data-id");
                        EnvioEdit(formatoid);
                    });
                });
            }
            function verDet(id) {
                if (id != undefined && id != null) {
                    $.blockUI({ message: mensajes.PROCESANDO });
                    $.ajax({
                        url: "/Curso/Curso/ObtenerCursobyId",
                        type: 'POST',
                        dataType: 'json',
                        data: { id: id },
                        success: function (response) {
                            $.unblockUI();
                            modalDetCurso.modal("show");
                            moldeoCursoview(response.objCurso, response.objModulo, response.objModuloDet);
                            arrayDet = response.objModuloDet;
                        },
                        error: function (response) {
                            AlertaGeneral("Alerta", response.message);
                            $.unblockUI();
                        }
                    });
                }
            }
            function moldeoCursoview(curso, modulo, detalle) {
                $("#lblCurso").text("folio " + curso[0].folio);
                CursoPlantilla =
                     "<div class='row' style='margin-top:10px;'>" +
                        "<div class='col-lg-12'>" +
                            "<div class='input-group'>" +
                                "<span class='input-group-addon'>Nombre Curso:</span>" +
                                "<input type='text' id='nombreCurso' class='form-control' readonly value='" + curso[0].nombreCurso + "'/>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                    "<div class='row'>" +
                        "<div class='col-lg-12'>" +
                            "<div class='input-group'>" +
                                "<span class='input-group-addon' >Fecha de Creación:</span>" +
                                "<input type='text' id='fechaIniCurso' readonly class='form-control'/>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                    "<div class='row'>" +
                        "<div class='col-lg-12'>" +
                            "<div class='input-group'>" +
                                "<span class='input-group-addon'>Descripción:</span>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                    "<div class='row'>" +
                        "<div class='col-lg-12'>" +
                            "<textarea type='text' id='descripcionCurso' class='form-control' readonly >" + curso[0].descripcion + "</textarea>" +
                        "</div>" +
                    "</div>" +
                    "</div>" +
                "<div class='row'>" +
                     "<div class='col-lg-12'>" +
                         "<div class='input-group'>" +
                             "<span class='input-group-addon'>Usuario Creador:</span>" +
                             "<input type='text' id='nombreCurso' class='form-control' readonly value='" + curso[0].nomUsuarioCap + "'/>" +
                         "</div>" +
                     "</div>" +
                 "</div>";
                //+ curso[0].descripcion + 
                //+ curso[0].nomUsuarioCap +

                CuerpoModalDet.append(CursoPlantilla);//agrega contenido curso

                cabeceraModelo =
                "<fieldset class='fieldset-custm'>" +
                "<legend class='legend-custm'>Listado de Modulos:</legend>" +
                "<div class='table-responsive'>" +
                "<table class='table table-fixed' id='tablaModulo'>" +
                "<thead class='bg-table-header'>" +
                "<tr>" +
                "<th data-column-id='id'>id</th>" +
                "<th data-column-id='nombreCurso'>Nombre</th>" +
                "<th data-column-id='folio'>Descripcion</th>" +
                "<th data-column-id='descripcion'>Paginas</th>" +
                "<th data-formatter='completo'>Estatus</th>" +
                "<th data-formatter='btn-detalle'>Paginado</th>" +
                "</tr>" +
                "</thead>" +
                "<tbody></tbody>" +
                "</table>" +
                "</div>" +
                "</fieldset>" +
                "</div>";

                CuerpoModalDet.append(cabeceraModelo);//agrega cabecera modelo
                modeloModeloView(modulo, detalle);
            }
            function modeloModeloView(modulo, detalle) {
                longdetalle = detalle.length;//numero de paginas
                modulo.forEach(function (valorM, indiceM, arrayM) {
                    contpagbyMod = 0;
                    detalle.forEach(function (valorMD, indiceMD, arrayMD) {
                        if (valorM.id == valorMD.idModulo) {
                            contpagbyMod += 1;
                        }
                    });
                    disabled = "";
                    completo = "En progreso";
                    var tipoCompleto;
                    if (valorM.completo == true) {
                        tipoCompleto = "succes";
                    } else if (valorM.completo == false && contpagbyMod == 0) {
                        tipoCompleto = "danger";
                        disabled = "disabled";
                    }
                    if (valorM.completo)
                        completo = "completo";
                    renglonModulo =
                                "<tr class=" + tipoCompleto + ">" +
		                        "<td>" + valorM.id + "</td>" +
                                "<td>" + valorM.nombreModulo + "</td>" +
		                        "<td>" + valorM.descripcion + "</td>" +
                                "<td>" + contpagbyMod + "</td>" +
		                        "<td> <label>" + completo + "</label></td>" +
		                        "<td>" + "<button type='button' class='btn btn-primary verPag' " + disabled + "  id-nombre='" + valorM.nombreModulo + "'   id-pag='" + valorM.id + "'>" +
                                 "<span class=' glyphicon glyphicon-list' style='margin-rigth:2px;'></span> " + "Detalle" +
                                "</td>" +
		                        "</tr>";
                    $('#tablaModulo').append(renglonModulo);
                });
            }
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
            function Eliminar(id) {
                if (id != undefined && id != null) {
                    $.blockUI({ message: mensajes.PROCESANDO });
                    $.ajax({
                        url: "/Curso/Curso/EliminaGestor",
                        type: 'POST',
                        dataType: 'json',
                        data: { id: id },
                        success: function (response) {
                            $.unblockUI();
                            ConfirmacionGeneralFC("Confirmación", "Se a eliminado el Curso" + response.Elimina, "bg-green");
                            GetListCursos();
                        },
                        error: function (response) {
                            AlertaGeneral("Alerta", response.message);
                            $.unblockUI();
                        }
                    });
                }
            }
            function EnvioEdit(id) {
                if (id != undefined && id != null) {
                    window.location = '/curso/curso/index?id=' + id;
                }
            }
            function GetListCursos() {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: "/Curso/Curso/GetListCursos",
                    type: 'POST',
                    dataType: 'json',
                    success: function (response) {
                        //tblAdvPers.bootgrid("clear");
                        //tblAdvPers.bootgrid("append", response);
                        initTabla();
                        tblCursos.bootgrid("clear");

                        //realizar cambiios

                        tblCursos.bootgrid("append", response);
                        $.unblockUI();
                    },
                    error: function (response) {
                        AlertaGeneral("Alerta", response.message);
                        $.unblockUI();
                    }
                });
            }
        }
        init();
    }

    $(document).ready(function () {
        cursos.cursos = new cursos();
    });
});

