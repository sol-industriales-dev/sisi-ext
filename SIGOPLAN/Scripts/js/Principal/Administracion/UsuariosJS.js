


$(function () {
    $.namespace('Administracion.Usuarios.CatUsuarios');
    CatUsuarios = function () {

        const txtClave = $('#txtClave');
        const checkActivo = $('#checkActivo');
        const checkEnviar = $('#checkEnviar');
        const btnAccesos = $('#btnAccesos');

        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        tblUsuarios = $("#tblUsuarios");
        var lstPermisosGeneral = [];
        var alertaGuardado = true;

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
        cboDepartamento = $("#cboDepartamento");
        cboPuesto = $("#cboPuesto");
        cboPerfil = $("#cboPerfil");
        cboEmpresa = $("#cboEmpresa");

        txtIDEnkontrol = $("#txtIDEnkontrol");
        checkAuditor = $("#checkAuditor");
        checkGestorSeguridad = $("#checkGestorSeguridad");
        checkPatoos = $("#checkPatoos");

        var idSistemaSelected = 0;
        urlCboCC = '/Conciliacion/getCboCC';
        function init() {
            cboCC.fillCombo(urlCboCC, { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");
            cboEmpresa.fillCombo('/Administrador/Usuarios/getLstEmpresasActivas', null, false, "Todos");
            convertToMultiselect("#cboEmpresa");
            cboDepartamento.fillCombo('/Administrador/Usuarios/getLstDept', null, false, null);
            cboPerfil.fillCombo('/Administrador/Usuarios/getLstPerfilActivo', null, false, null);
            cboPuesto.prop("disabled", true);
            btnModalEliminar = $("#btnModalEliminar");
            cboDepartamento.change(setLstPuesto);
            $("[name='SistemasSelect']").click(function (e) {
                if (!alertaGuardado) {
                    e.preventDefault();
                    $(this)[0].dataset.seleccionado = 1;
                    cargarModal($(this));
                }
                else {
                    $(this)[0].dataset.seleccionado = 0;
                    selectSistema();
                }
            });
            $("#btnNuevo").click(function () {
                lstPermisosGeneral = [];
                txtNombre.val("");
                txtApPaterno.val("");
                txtApMaterno.val("");
                txtCorreo.val("");
                txtClave.val("");
                checkActivo[0].checked = true;
                checkEnviar[0].checked = true;
                checkAuditor[0].checked = false;
                checkGestorSeguridad[0].checked = false;
                checkPatoos[0].checked = false;
                txtUsuario.val("");
                lblIdUsuario.text("");
                txtContrasena.val("");
                txtConfirContrasena.val("");
                cboCC.val("");
                cboCC.multiselect("refresh");
                cboPerfil.val("");
                cboPuesto.val("");
                cboPuesto.html("");
                cboPerfil.val(2);
                $("#btnstep3").attr('disabled', 'disabled');
                $('#ulRadioBtn input[type=radio]').each(function () {
                    this.checked = false;
                });
                LoadTreeMenu(0);
            });
            LoadTblUsuarios();
            btnModalEliminar.click(function () {
                alertaGuardado = true;
                $("input[data-seleccionado=1]").prop("checked", true);
                selectSistema();
            });
            btnGuardar.click(guardarUsuarioNuevaFuncion); // btnGuardar.click(guardarUsuario);
            btnSiguienteStep2Validar.click(valInfoDeUsuario);
            btnOK.click(function () { window.location = "/Administrador/Usuarios/PerfilUsuario"; });

            btnAccesos.click(() => {
                AlertaAceptarRechazarNormal(
                    'Confirmar acción',
                    `¿Está seguro de enviar un correo con los datos de acceso a todos los usuarios con el campo de enviar activado?`,
                    () => enviarAccesos())
            })

        }

        function enviarAccesos() {
            $.blockUI({ message: 'Enviando accesos...' });
            $.post('/Usuario/EnviarAccesos')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Accesos enviados correctamente.`);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function setLstPuesto() {
            let idDept = this.value;
            if (idDept == "") {
                cboPuesto.html("");
                cboPuesto.prop("disabled", true);
            }
            else {
                cboPuesto.fillCombo('/Administrador/Usuarios/getLstPuesto', { idDept: idDept }, false, null);
                cboPuesto.prop("disabled", false);
            }
        }
        function cargarModal(obj) {
            modalEliminar.modal("show");
        }

        function selectSistema() {
            var lstSistemas = $('[name="SistemasSelect"]:checked');
            var lstIntSistemas = [];
            for (var x = 0; x < lstSistemas.length; x++) {
                lstIntSistemas[x] = lstSistemas[x].value;
                idSistemaSelected = lstSistemas[x].value;
            }
            LoadTreeMenu(lstIntSistemas);
        }

        function LoadTreeMenu(lstIntSistemas) {
            $.blockUI({ message: 'Cargando vistas...' });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Administrador/Usuarios/getVistas",
                data: { sistemas: lstIntSistemas, idUsuario: lblIdUsuario.text() },
                success: function (response) {
                    $("#divarbol2").fancytree({
                        checkbox: true,
                        selectMode: 3,
                        source: response.menuCompleto,
                        select: function (e, data) {
                            alertaGuardado = false;
                            // Get a list of all selected nodes, and convert to a key array:
                            var selKeys = $.map(data.tree.getSelectedNodes(), function (node) {
                                return node.key;
                            });
                            $("#echoSelection3").text(selKeys.join(", "));
                            // Get a list of all selected TOP nodes
                            var selRootNodes = data.tree.getSelectedNodes(true);
                            // ... and convert to a key array:
                            var selRootKeys = $.map(selRootNodes, function (node) {
                                return node.key;
                            });
                            $("#echoSelectionRootKeys3").text(selRootKeys.join(", "));
                            $("#echoSelectionRoots3").text(selRootNodes.join(", "));
                            lstPermisosGeneral = [];
                            getPermisos();
                            LoadTreeAcciones(lstPermisosGeneral);
                        },
                        dblclick: function (e, data) {
                            data.node.toggleSelected();
                        },
                        keydown: function (e, data) {
                            if (e.which === 32) {
                                data.node.toggleSelected();
                                return false;
                            }
                        },
                        // The following options are only required, if we have more than one tree on one page:
                        //        initId: "treeData",
                        cookieId: "fancytree-Cb3",
                        idPrefix: "fancytree-Cb3-"
                    });
                    var rootNode = $("#divarbol2").fancytree("getRootNode");
                    rootNode.removeChildren();
                    rootNode.addChildren(response.menuCompleto);
                    lstPermisosGeneral = [];
                    getPermisos();
                    LoadTreeAcciones(lstPermisosGeneral);
                },
                error: function () {
                    AlertaGeneral(`Error`, `Ocurrió un error al lanzar la petición.`);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }

        function LoadTreeAcciones(lstIntVistas) {
            $.blockUI({ message: 'Cargando acciones...' });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Administrador/Usuarios/getPermisosVistas",
                data: { lstVistas: lstIntVistas, idUsuario: lblIdUsuario.text() },
                success: function (response) {
                    //var rootNode = $("#divarbol2").fancytree("getRootNode");
                    $("#divarbol3").fancytree({
                        //      extensions: ["select"],
                        checkbox: true,
                        selectMode: 3,
                        source: response,
                        select: function (e, data) {
                            alertaGuardado = false;
                            // Get a list of all selected nodes, and convert to a key array:
                            var selKeys = $.map(data.tree.getSelectedNodes(), function (node) {
                                return node.key;
                            });
                            $("#echoSelection3").text(selKeys.join(", "));
                            // Get a list of all selected TOP nodes
                            var selRootNodes = data.tree.getSelectedNodes(true);
                            // ... and convert to a key array:
                            var selRootKeys = $.map(selRootNodes, function (node) {
                                return node.key;
                            });
                            $("#echoSelectionRootKeys3").text(selRootKeys.join(", "));
                            $("#echoSelectionRoots3").text(selRootNodes.join(", "));
                        },
                        dblclick: function (e, data) {
                            data.node.toggleSelected();
                        },
                        keydown: function (e, data) {
                            if (e.which === 32) {
                                data.node.toggleSelected();
                                return false;
                            }
                        },
                        // The following options are only required, if we have more than one tree on one page:
                        //        initId: "treeData",
                        cookieId: "fancytree-Cb3",
                        idPrefix: "fancytree-Cb3-"
                    });
                    var rootNode = $("#divarbol3").fancytree("getRootNode");
                    rootNode.removeChildren();
                    rootNode.addChildren(response);

                },
                error: function () {
                    AlertaGeneral(`Error`, `Ocurrió un error al lanzar la petición.`);
                },
                complete: function () {
                    $.unblockUI();
                }
            });

        }
        function getPermisos() {
            if ($('#divarbol2')[0].childElementCount > 0) {
                var Permisos = $('#divarbol2').fancytree('getTree').getSelectedNodes();
                for (var x = 0; x < Permisos.length; x++) {
                    var objPermiso = {
                        id: Permisos[x].data.id,
                        tblP_Usuario_id: lblIdUsuario.text(),
                        tblP_Menu_id: Permisos[x].key,
                        sistema: $('[name="SistemasSelect"]:checked')[0].value

                    };
                    lstPermisosGeneral.push(objPermiso);
                    getParents(Permisos[x].parent);
                }
            }
        }
        function getPermisosVistas() {
            var lstPermisosVistas = [];
            if ($('#divarbol3')[0].childElementCount > 0) {
                var Permisos = $('#divarbol3').fancytree('getTree').getSelectedNodes();
                for (var x = 0; x < Permisos.length; x++) {
                    if (Permisos[x].children == null) {

                        var objPermisoVistas = {
                            id: Permisos[x].data.id,
                            tblP_AccionesVista_id: Permisos[x].key,
                            tblP_Usuario_id: lblIdUsuario.text(),
                            sistema: $('[name="SistemasSelect"]:checked')[0].value
                        }
                        lstPermisosVistas.push(objPermisoVistas);
                    }

                }
            }
            return lstPermisosVistas;
        }
        function getParents(Permisos) {
            if (Permisos.parent != null) {
                var objPermiso = {
                    id: Permisos.data.id,
                    tblP_Usuario_id: lblIdUsuario.text(),
                    tblP_Menu_id: Permisos.key,
                    sistema: $('[name="SistemasSelect"]:checked')[0].value
                };
                lstPermisosGeneral.push(objPermiso);
                getParents(Permisos.parent);
            }
        }
        function getCCs() {
            $.blockUI({ message: 'Cargando CCs usuario...' });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Administrador/Usuarios/getIdCCsUsuario",
                data: { id: lblIdUsuario.text() },
                success: function (response) {
                    cboCC.val(response);
                    cboCC.multiselect("refresh");
                    ObtenerIDsCCsUsuarioAutoriza();
                },
                error: function () {
                    AlertaGeneral(`Error`, `Ocurrió un error al lanzar la petición.`);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
        function ObtenerIDsCCsUsuarioAutoriza() {

            const id = lblIdUsuario.text()

            $.blockUI({ message: 'Cargando...' });
            $.get('/Administrador/Usuarios/ObtenerIDsCCsUsuarioAutoriza', { id })
                .always($.unblockUI)
                .then(response => {
                    if (response) {
                        // Operación exitosa.
                        response.forEach(id => {

                            const element = $(`div.btn-group ul.multiselect-container.dropdown-menu input[value=${id}]`);

                            if (element && element.length > 0 && element.prop('checked')) {
                                element.prop('disabled', true);
                            }
                        });

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }
        function valInfoDeUsuario() {
            var Guardar = true;
            let MensajeAlerta = "";
            if (txtNombre.val() == "" || txtUsuario.val() == "") {
                MensajeAlerta = "Nombre del empleado y Nombre de Usuario no pueden ir vacíos";
                Guardar = false;
            }
            else if (txtContrasena.val() == "") {
                MensajeAlerta = "No ha configurado una contraseña";
                Guardar = false;
            }
            else if (txtContrasena.val() != txtConfirContrasena.val()) {
                MensajeAlerta = "Debe confirmar contraseña";
                Guardar = false;
            }
            if (Guardar) {
                btnSiguienteStep2.trigger("click");
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
                cveEmpleado: txtClave.val(),
                estatus: checkActivo.is(':checked'),
                enviar: checkEnviar.is(':checked'),
                contrasena: txtContrasena.val() == txtConfirContrasena.val() ? txtContrasena.val() : null,
                puestoID: cboPuesto.val(),
                perfilID: cboPerfil.val(),
                idEnkontrol: txtIDEnkontrol.val() == '' ? 0 : txtIDEnkontrol.val(),
                usuarioAuditor: checkAuditor.is(':checked'),
                esAuditor: checkAuditor.is(':checked'),
                tipoSeguridad: checkGestorSeguridad.is(':checked'),
                usuarioSeguridad: txtUsuario.val(),
                externoPatoos: checkPatoos.is(':checked')
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
            getPermisos();

            if (Guardar) {
                $.blockUI({ message: 'Guardando usuario...' });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Administrador/Usuarios/SaveUsuario",
                    data: {
                        usuario: objUsuario,
                        permisos: lstPermisosGeneral,
                        ccs: getValoresMultiples("#cboCC"),
                        accVistas: getPermisosVistas(),
                        sistema: idSistemaSelected,
                        empresa: getValoresMultiples("#cboEmpresa")
                    },
                    success: function (success) {
                        if (success) {
                            ConfirmacionGuardadoCalidad("Guardado de Usuario", "Se ha guardado exitosamente la información del usuario", "bg-green");
                        } else {
                            ConfirmacionGuardadoCalidad("Guardado de Usuario", "Ocurrió un error al guardar la información del usuario.", "bg-green");
                        }
                    },
                    error: function () {
                        AlertaGeneral(`Error`, `Ocurrió un error al lanzar la petición.`);
                    }
                }).always($.unblockUI);
            }
            else {
                AlertaGeneral("Guardar Usuario", MensajeAlerta);
            }
        }

        function guardarUsuarioNuevaFuncion() {
            if (txtNombre.val() == "" || txtUsuario.val() == "") {
                Alert2Warning('Nombre del empleado y nombre de usuario no pueden ir vacíos');
                return;
            }
            if (lblIdUsuario.text() == "0" && txtContrasena.val() == "") {
                Alert2Warning('No ha configurado una contraseña')
                return;
            }

            var usuario = {
                id: lblIdUsuario.text(),
                nombre: txtNombre.val(),
                apellidoPaterno: txtApPaterno.val(),
                apellidoMaterno: txtApMaterno.val(),
                nombreUsuario: txtUsuario.val(),
                correo: txtCorreo.val(),
                cveEmpleado: txtClave.val(),
                estatus: checkActivo.is(':checked'),
                enviar: checkEnviar.is(':checked'),
                contrasena: txtContrasena.val() == txtConfirContrasena.val() ? txtContrasena.val() : null,
                puestoID: cboPuesto.val(),
                perfilID: cboPerfil.val(),
                idEnkontrol: txtIDEnkontrol.val() == '' ? 0 : txtIDEnkontrol.val(),
                usuarioAuditor: checkAuditor.is(':checked'),
                esAuditor: checkAuditor.is(':checked'),
                tipoSeguridad: checkGestorSeguridad.is(':checked'),
                usuarioSeguridad: txtUsuario.val(),
                externoPatoos: checkPatoos.is(':checked')
            };

            lstPermisosGeneral = [];
            getPermisos();

            $.blockUI({ message: 'Guardando usuario...' });
            axios.post('/Administrador/Usuarios/SaveUsuarioNuevaFuncion', {
                usuario,
                permisos: lstPermisosGeneral,
                ccs: getValoresMultiples("#cboCC"),
                accVistas: getPermisosVistas(),
                sistema: idSistemaSelected,
                empresa: getValoresMultiples("#cboEmpresa")
            }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    ConfirmacionGuardadoCalidad("Guardado de Usuario", "Se ha guardado exitosamente la información del usuario", "bg-green");
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message)).always($.unblockUI);
        }

        function LoadTblUsuarios() {
            $.blockUI({ message: 'Cargando usuarios...' });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Administrador/Usuarios/GetUsuarios",
                success: function (response) {
                    if (response.success) {
                        tblUsuarios.bootgrid("clear");
                        tblUsuarios.bootgrid("append", response.items);
                    } else {
                        AlertaGeneral(`Aviso`, response.message);
                    }
                },
                error: function () {
                    AlertaGeneral(`Aviso`, `Ocurrió un error al lanzar la petición.`);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
        function iniciarGrid() {
            tblUsuarios.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                formatters: {
                    "verPerfil": function (column, row) {
                        return "<button type='button' class='btn btn-primary nextBtn VerPerfil' data-idUsuario='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                            " </button>";

                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                tblUsuarios.find(".VerPerfil").on('click', function (e) {
                    var idUsuario = $(this).attr('data-idUsuario');

                    $.blockUI({ message: 'Cargando información del usuario...' });
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: "/Administrador/Usuarios/GetInfoUsuario",
                        data: { id: idUsuario },
                        success: function (response) {
                            txtNombre.val(response.nombre);
                            txtApPaterno.val(response.paterno);
                            txtApMaterno.val(response.materno);
                            txtCorreo.val(response.correo);
                            txtClave.val(response.cveEmpleado);
                            checkActivo[0].checked = response.estatus;
                            checkEnviar[0].checked = response.enviar;
                            txtUsuario.val(response.usuario);
                            txtContrasena.val(response.contrasena);
                            txtConfirContrasena.val(response.contrasena);
                            lblIdUsuario.text(response.id);
                            cboEmpresa.val(response.empresa);
                            cboEmpresa.multiselect("refresh");
                            cboDepartamento.val(response.departamentoID).change();
                            cboPuesto.val(response.puestoID);
                            cboPerfil.val(response.perfilID);
                            txtIDEnkontrol.val(response.idEnkontrol == 0 ? '' : response.idEnkontrol);
                            checkAuditor[0].checked = response.auditor;
                            checkGestorSeguridad[0].checked = response.gestorSeguridad;
                            checkPatoos[0].checked = response.externoPatoos;
                            $("#btnstep3").attr('disabled', 'disabled');
                            $('#ulRadioBtn input[type=radio]').each(function () {
                                this.checked = false;
                            });
                            lstPermisosGeneral = [];
                            LoadTreeMenu(0);
                            if (lblIdUsuario.text() !== "0") {
                                getCCs();
                            }
                            btnSiguienteStep1.trigger("click");
                        },
                        error: function () {
                            AlertaGeneral(`Error`, `Ocurrió un error al lanzar la petición.`);
                        },
                        complete: function () {
                            $.unblockUI();
                        }
                    });

                });
            });
        }
        function ConfirmacionGuardadoCalidad(titulo, mensaje, color) {
            $("#txtComentarioPerfilUsuario").text('');
            $("#dialogGuardadoPerfilUsuario").dialog({
                title: titulo,
                draggable: false,
                resizable: false,
                maxWidth: 600,
                minWidth: 400,
                position: {
                    my: "center bottom",
                    at: "center top",
                    within: $(".RenderBody")
                },
                open: function (event, ui) { $(".ui-dialog-titlebar-close", ui.dialog).hide(); }

            });
            $("#btnOK").removeClass("hide");
            $("#txtComentarioPerfilUsuario").text(mensaje);
            $("#dialogGuardadoPerfilUsuario").dialog();
        }
        init();
        iniciarGrid();
    }
    $(document).ready(function () {
        Administracion.Usuarios.CatUsuarios = new CatUsuarios();
    });
});
