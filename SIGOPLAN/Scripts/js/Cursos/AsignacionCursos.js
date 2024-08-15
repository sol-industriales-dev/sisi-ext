$(function () {
    $.namespace('Cursos.Cursos');

    cursos = function () {
        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        /*/Variables nuevo usuario*/
        txtNombre = $("#txtNombre");
        txtApPaterno = $("#txtApPaterno");
        txtApMaterno = $("#txtApMaterno");
        txtCorreo = $("#txtCorreo");
        txtUsuario = $("#txtUsuario");
        txtContrasena = $("#txtContrasena");
        txtConfirContrasena = $("#txtConfirContrasena");
        cboCC = $("#cboCC");
        btnGuardar = $("#btnGuardar");
        lblIdUsuario = $("#lblIdUsuario");
        modalEliminar = $("#modalEliminar");
        btnSiguienteStep1 = $("#btnSiguienteStep1");
        btnSiguienteStep2 = $("#btnSiguienteStep2");
        btnSiguienteStep2Validar = $("#btnSiguienteStep2Validar");
        btnOK = $("#btnOK");
        //**variables**/
        CursoModal = $("#CursoModal");
        tblCursos = $("#tblCursos");
        tblCurUsr = $("#tblCurUsr");
        cursosPanel = $('#cursos');
        departamentoPanel = $("#departamentos")
        usuarioPanel = $("#Usuario");
        deptoPanel = $("#departamento");
        addDepto = $("#addDepto");
        addviewCurso = $("#addviewCurso");
        addUsuario = $("#addUsuario");
        tblUsuarios = $("#tblUsuarios");
        tblDeptos = $("#tblDeptos");
        NumFolio = $("#NumFolio");
        btnguardarUsnuevo = $("#btnguardarUsnuevo");
        nuevoUsuario = $(".nuevoUsuario ");
        lblCurso = $("#lblCurso");//etiqueta curso campo para mostrar el nombre usuario o nombre depto
        lblDato = $("#lblDatp");//etiqueta curso campo para mostrar el id del usuario o el nombre dle usuario
        var idSistemaSelected = 0;
        /* Trabajo detalles cuso 13/02/18 */
        modalDetCurso = $("#modalDetCurso");
        CuerpoModalDet = $(".CuerpoModalDet");
        tablaModalPag = $("#tablaModalPag");
        summernote = $("#summernote");
        grupoAsig = $("#grupoAsig");
        /* GUARDADO CURSO */
        btnGuardarAsignacionUsuario = $("#btnGuardarAsignacionUsuario");
        AsignacionCurso = [];
        function init() {
            $(document).on('click', '.lblCurso', function () {
                viewCurso();
            });
            $(document).on('click', '.lblUsuario', function () {
                viewUsuario();
            });
            $(document).on('click', '.lblDepto', function () {
                viewDepto();
            });
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            $("#modalDetCurso").on("hidden.bs.modal", function () {
                CuerpoModalDet.empty();
                tablaModulo.empty();
            });
            $("#modalPaginado").on("hidden.bs.modal", function () {
                $("#tblPlantilla").empty();
            });
            $(document).on('click', '.pagDet', function () {
                var nombreModulo = $(this).attr('id-pag');
                //titulocontenido.text(idTitulopag.text());
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
            $(document).on('click', '.verPag', function () {
                var nombreModulo = $(this).attr('id-nombre');
                //alert(nombreModulo);;
                //idTitulopag.text(nombreModulo)
                var idpag = $(this).attr('id-pag');
                paginadoMostrar(idpag);
            });
            convertToMultiselect("#cboCC");
            viewDepto();
            addDepto.click(AsDepto);
            addUsuario.click(viewUsuario);
            $('#divMain').bootgrid({
                css: {
                    pagination: 'pagination pagination-sm'
                }
            });
            btnguardarUsnuevo.click(valInfoDeUsuario);
            $("#btnguardarUsnuevo").click(function () {
                valInfoDeUsuario();
            });
            $("#btnNuevo").click(function () {
                //alert('chicho')
                nuevoUsuario.show();
                usuarioPanel.hide();
                //lstPermisosGeneral = [];
                txtNombre.val("");
                txtApPaterno.val("");
                txtApMaterno.val("");
                txtCorreo.val("");
                txtUsuario.val("");
                lblIdUsuario.text("");
                txtContrasena.val("");
                txtConfirContrasena.val("");
            });
            btnGuardarAsignacionUsuario.click(AsignacursoUsuario);
        };
        init();
    }
    //14/02/18 15:17 pm  envio de asignacion e curso
    function AsignacursoUsuario() {
        $.ajax({
            url: "/Curso/Curso/AsignarCurso",
            type: 'POST',
            dataType: 'json',
            data: { lstAsignacion: AsignacionCurso },
            success: function (response) {
                //tblAdvPers.bootgrid("clear");
                //tblAdvPers.bootgrid("append", response);
                //initTabla(tbl);
                //tbl.bootgrid("clear");
                //tbl.bootgrid("append", response);
                //$.unblockUI();
            },
            error: function (response) {
                AlertaGeneral("Alerta", response.message);
                $.unblockUI();
            }
        })

    }


    function viewDepto() {
        cursosPanel.hide();
        departamentoPanel.hide();
        usuarioPanel.hide();
        nuevoUsuario.hide();
        LoadTblDepto();
        deptoPanel.show();
    }
    function viewUsuario() {
        cursosPanel.hide();
        departamentoPanel.hide();
        usuarioPanel.show();
        LoadTblUsuarios();
        nuevoUsuario.hide();
        deptoPanel.hide();
    }
    function viewCurso() {
        departamentoPanel.hide();
        usuarioPanel.hide();
        cursosPanel.show();
        GetListCursos(tblCursos);
        nuevoUsuario.hide();
        deptoPanel.hide();
    }
    function AsDepto() {
        usuarioPanel.hide();
        cursosPanel.hide();
        departamentoPanel.show()
        nuevoUsuario.hide();
    }
    function initTabla(tbl) {
        tbl.bootgrid({
            css: {
                //pagination: 'pagination pagination-sm'
                //pagination: 'pagination pull-left'
            },
            align: 'center',
            selection: true,
            //pagination: 'left',
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
                           "<span class=' glyphicon glyphicon-list' style='margin-rigth:2px;'></span> " + "Detalle" +
                             " </button>"
                },
                "check-asignar": function (column, row) {
                    return "<div class='material-switch' id='grupoAsig'> " +
                             "<input class='chkAsignar' id='someSwitchOptionDanger" + row.id + "' name='DeptoSelect' type='checkbox' data-id='" + row.id + "'/>" +
                             "<label   id='someSwitchOptionDanger" + row.id + "'  for='someSwitchOptionDanger" + row.id + "'  class='label-success lblc'></label>" +
                        "</div>"
                }
            }
        }).on("loaded.rs.jquery.bootgrid", function () {
            tbl.find(".verDet").on('click', function (e) {
                var formatoid = $(this).attr("data-id");
                verDet(formatoid);
                //modalDetCurso.modal("show");
                //GetListCursos(tblCurUsr);
            });
            tbl.find(".chkAsignar").on('click', function (e) {
                var formatoid = $(this).attr("data-id");
                var Habilitar;
                if ($('input.chkAsignar').is(':checked')) {
                    Habilitar = true;
                } else {
                    Habilitar = false;
                }
                var formatoid = $(this).attr("data-id");
                //asignar el usuaruario al curso
                objAsignaCurso = { id: 0, claveUsuario: $("#lblDato").text(), idCurso: formatoid, estado: Habilitar };
                AsignacionCurso.push(objAsignaCurso);
                //al momento de cargar modal rrecorrerlo  shown para habilitar los cursos cargados
            });
        });
    }
    function GetListCursos(tbl) {
        //$.blockUI({ message: mensajes.PROCESANDO });
        $.ajax({
            url: "/Curso/Curso/GetListCursos",
            type: 'POST',
            dataType: 'json',
            async: true,
            success: function (response) {
                //tblAdvPers.bootgrid("clear");
                //tblAdvPers.bootgrid("append", response);
                initTabla(tbl);
                tbl.bootgrid("clear");
                tbl.bootgrid("append", response);
                //$.unblockUI();
            },
            error: function (response) {
                AlertaGeneral("Alerta", response.message);
                $.unblockUI();
            }
        });
    }
    function LoadTblUsuarios() {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/Administrador/Usuarios/GetUsuarios",
            success: function (response) {
                iniciarGrid();
                tblUsuarios.bootgrid("clear");
                tblUsuarios.bootgrid("append", response.items);

            },
            error: function () {
                $.unblockUI();
            }
        });
    }
    function LoadTblDepto() {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/curso/curso/GetListDeptos",
            success: function (response) {
                iniciarGridDepartamento();
                tblDeptos.bootgrid("clear");
                tblDeptos.bootgrid("append", response.departamentos);
            },
            error: function () {
                $.unblockUI();
            }
        });
    }
    function iniciarGridDepartamento() {
        tblDeptos.bootgrid({
            align: 'center',
            selection: true,
            labels:
                {
                    infos: '{{ctx.total}} Departamentos'
                },
            templates: {
                search: ""
            },
            formatters: {

                "verPerfil": function (column, row) {
                    return "<button type='button' class='btn btn-primary nextBtn VerPerfil' data-idUsuario='" + row.id + "'>" +
                        "<span class='fa fa-book'></span> " +
                               " </button>";
                }
            }
        }).on("loaded.rs.jquery.bootgrid", function () {
            tblDeptos.find(".VerPerfil").on('click', function (e) {
                var formatoid = $(this).attr("data-id");
                CursoModal.modal('show');
                $("#lblCurso").text(formatoid);
                //var formatoid = $(this).attr("data-id");
                GetListCursos(tblCurUsr);
            });

        });
    }
    function iniciarGrid() {
        tblUsuarios.bootgrid({
            align: 'center',
            selection: true,
            labels:
                {
                    infos: '{{ctx.total}} Departamentos'
                },
            templates: {
                search: ""
            },
            formatters: {
                "verPerfil": function (column, row) {
                    return "<button type='button' class='btn btn-primary nextBtn VerPerfil' data-idUsuario='" + row.id + "'>" +
                        "<span class='fa fa-book'></span> " +
                               " </button>";
                }
            }
        }).on("loaded.rs.jquery.bootgrid", function () {
            tblUsuarios.find(".VerPerfil").on('click', function (e) {
                var formatoid = $(this).attr("data-idUsuario");
                CursoModal.modal('show');
                $("#lblCurso").text("Nombre Usuario");
                $("#lblDato").text(formatoid);
                GetListCursos(tblCurUsr);
            });
        });
    }
    function setRadioValue(sel, tog) {
        $('#' + tog).prop('value', sel);
        $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
        $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');
    }
    function valInfoDeUsuario() {
        var Guardar = true;
        let MensajeAlerta = "";
        if (txtNombre.val() == "" || txtUsuario.val() == "") {
            MensajeAlerta = "Nombre del empleado y Nombre de Usuario no pueden ir vacios";
            Guardar = false;
        }
        else if (txtContrasena.val() == "") {
            MensajeAlerta = "No a configurado una contraseña";
            Guardar = false;
        }
        else if (txtContrasena.val() != txtConfirContrasena.val()) {
            MensajeAlerta = "Debe confirmar contraseña";
            Guardar = false;
        }
        if (Guardar) {
            guardarUsuario();
        }
        else {

            AlertaGeneral("Guardar Usuario", MensajeAlerta);
        }
    }
    function guardarUsuario() {
        var Guardar = true;
        var objUsuario = {
            id: lblIdUsuario.text(),
            nombre: txtNombre.val(),
            apellidoPaterno: txtApPaterno.val(),
            apellidoMaterno: txtApMaterno.val(),
            nombreUsuario: txtUsuario.val(),
            correo: txtCorreo.val(),
            contrasena: txtContrasena.val() == txtConfirContrasena.val() ? txtContrasena.val() : null
        };
        let MensajeAlerta = "";
        if (objUsuario.nombre == "" || objUsuario.nombreUsuario == "") {
            MensajeAlerta = "Nombre del empleado y Nombre de Usuario no pueden ir vacios";
            Guardar = false;
        }
        if (objUsuario.id == "0" && objUsuario.contrasena == "") {
            MensajeAlerta = "No a configurado una contraseña";
            Guardar = false;
        }
        lstPermisosGeneral = [];

        if (Guardar) {
            //$.ajax({
            //    datatype: "json",
            //    type: "POST",
            //    url: "/Administrador/Usuarios/SaveUsuario",
            //    //data: { usuario: objUsuario, permisos: lstPermisosGeneral, ccs: getValoresMultiples("#cboCC"), accVistas: getPermisosVistas(), sistema: idSistemaSelected },
            //    data: { usuario: objUsuario, permisos: lstPermisosGeneral, ccs: getValoresMultiples("#cboCC"), accVistas: getPermisosVistas(), sistema: 0},
            //    success: function (response) {
            //        ConfirmacionGuardadoCalidad("Guardado de Usuario", "Se a guardado exitosamente la informacion del usuario", "bg-green");
            //    },
            //    error: function () {
            //        $.unblockUI();
            //    }
            //});
            nuevoUsuario.hide();
            usuarioPanel.show();


        }
        else {
            AlertaGeneral("Guardar Usuario", MensajeAlerta);
        }
    }
    //interaccion curso consulta 13/02/18
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
        //$("#lblCurso").text("folio " + curso[0].folio);
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
                $("#modalPaginado").modal("show");
            }
        });
    }
    $(document).ready(function () {
        cursos.cursos = new cursos();
    });
});

