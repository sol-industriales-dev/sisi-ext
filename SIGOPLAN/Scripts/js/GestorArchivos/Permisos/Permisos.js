(() => {

    $.namespace('GestorArchivos.GestorArchivos.Permisos');

    Permisos = function () {

        let primeraVezActivate = true;

        //Select usuario
        const cboEmpleado = $("#cboEmpleado");
        const cboDepartamento = $("#cboDepartamento");
        const cboEmpleadoExterno = $("#cboEmpleadoExterno");

        const botonModalGuardado = $("#botonModalGuardado");
        const botonSeleccionarUsuarioExterno = $("#btnSeleccionarUsuarioExterno");
        const botonReset = $("#reset");

        //Botones azules para delegar acciones
        const botonPermisoSubir = $("#botonPermisoSubir");
        const botonPermisoEliminar = $("#botonPermisoEliminar");
        const botonPermisoDescargarArchivo = $("#botonPermisoDescargarArchivos");
        const botonPermisoDescargarCarpeta = $("#botonPermisoDescargarCarpetas");
        const botonPermisoActualizar = $("#botonPermisoActualizar");
        const botonPermisoCrear = $("#botonPermisoCrear");
        const botonPermisoTodo = $("#botonPermisoTodo");

        //Checkboxes para mostrar feedback de las opciones seleccionadas
        const checkboxPermisoSubir = $("#checkboxPermisoSubir");
        const checkboxPermisoEliminar = $("#checkboxPermisoEliminar");
        const checkboxPermisoDescargarArchivo = $("#checkboxPermisoDescargarArchivos");
        const checkboxPermisoDescargarCarpeta = $("#checkboxPermisoDescargarCarpetas");
        const checkboxPermisoActualizar = $("#checkboxPermisoActualizar");
        const checkboxPermisoCrear = $("#checkboxPermisoCrear");

        const listaBotones = [botonPermisoSubir, botonPermisoEliminar,
            botonPermisoDescargarArchivo, botonPermisoDescargarCarpeta, botonPermisoActualizar, botonPermisoCrear];
        const listaCheckboxes = [checkboxPermisoSubir, checkboxPermisoEliminar,
            checkboxPermisoDescargarArchivo, checkboxPermisoDescargarCarpeta, checkboxPermisoActualizar, checkboxPermisoCrear];

        function mostrarModal(titulo, cuerpo) {
            var modal = $("#modalAviso");
            $("#modalAvisoTitutlo").html(titulo);
            $("#modalAvisoCuerpo").text(cuerpo);
            modal.modal('show');
        }

        function mostrarModalGuardado(titulo, cuerpo) {
            var modal = $("#modalGuardado");
            $("#modalGuardadoTitutlo").html(titulo);
            $("#modalGuardadoCuerpo").text(cuerpo);
            modal.modal('show');
        }

        function actualizarPermisoSeleccionado(checkbox, permiso) {
            if (checkbox.is(":checked")) {
                actualizarPermisos(`${permiso}True`);
            } else {
                actualizarPermisos(`${permiso}False`);
            }
        }

        function agregarEventosBotones() {

            botonPermisoSubir.on('click', () => {
                if (botonPermisoTodo.hasClass("active")) {
                    botonPermisoTodo.click();
                }
                checkboxPermisoSubir.attr("checked", !checkboxPermisoSubir.attr("checked"));
                actualizarPermisoSeleccionado(checkboxPermisoSubir, "subir");

            });

            botonPermisoEliminar.on('click', () => {
                if (botonPermisoTodo.hasClass("active")) {
                    botonPermisoTodo.click();
                }
                checkboxPermisoEliminar.attr("checked", !checkboxPermisoEliminar.attr("checked"));
                actualizarPermisoSeleccionado(checkboxPermisoEliminar, "eliminar");
            });

            botonPermisoDescargarArchivo.on('click', () => {
                if (botonPermisoTodo.hasClass("active")) {
                    botonPermisoTodo.click();
                }
                checkboxPermisoDescargarArchivo.attr("checked", !checkboxPermisoDescargarArchivo.attr("checked"));
                actualizarPermisoSeleccionado(checkboxPermisoDescargarArchivo, "descargarArchivo");
            });

            botonPermisoDescargarCarpeta.on('click', () => {
                if (botonPermisoTodo.hasClass("active")) {
                    botonPermisoTodo.click();
                }
                checkboxPermisoDescargarCarpeta.attr("checked", !checkboxPermisoDescargarCarpeta.attr("checked"));
                actualizarPermisoSeleccionado(checkboxPermisoDescargarCarpeta, "descargarCarpeta");
            });

            botonPermisoActualizar.on('click', () => {
                if (botonPermisoTodo.hasClass("active")) {
                    botonPermisoTodo.click();
                }
                checkboxPermisoActualizar.attr("checked", !checkboxPermisoActualizar.attr("checked"));
                actualizarPermisoSeleccionado(checkboxPermisoActualizar, "actualizar");
            });

            botonPermisoCrear.on('click', () => {
                if (botonPermisoTodo.hasClass("active")) {
                    botonPermisoTodo.click();
                }
                checkboxPermisoCrear.attr("checked", !checkboxPermisoCrear.attr("checked"));
                actualizarPermisoSeleccionado(checkboxPermisoCrear, "crear");
            });

            botonPermisoTodo.on('click', () => {
                if (botonPermisoTodo.hasClass("active")) {

                    listaCheckboxes.forEach(element => {
                        element.attr("checked", false);
                    });
                    actualizarPermisos("todoFalse");
                } else {

                    listaBotones.forEach(element => {
                        element.removeClass("active");
                    });

                    listaCheckboxes.forEach(element => {
                        element.attr("checked", true);
                    });
                    actualizarPermisos("todoTrue");
                }
            });
        }

        function actualizarPermisos(permiso) {
            const tree = $.ui.fancytree.getTree();
            const nodoActivo = tree.getActiveNode();
            if (nodoActivo != null) {

                if (nodoActivo.hasChildren()) {
                    actualizarCarpetasHijas(nodoActivo, permiso);
                }

                switch (permiso) {
                    case "subirTrue":
                        nodoActivo.data.permisos.puedeSubir = true;
                        break;
                    case "subirFalse":
                        nodoActivo.data.permisos.puedeSubir = false;
                        break;

                    case "eliminarTrue":
                        nodoActivo.data.permisos.puedeEliminar = true;
                        break;
                    case "eliminarFalse":
                        nodoActivo.data.permisos.puedeEliminar = false;
                        break;

                    case "descargarArchivoTrue":
                        nodoActivo.data.permisos.puedeDescargarArchivo = true;
                        break;
                    case "descargarArchivoFalse":
                        nodoActivo.data.permisos.puedeDescargarArchivo = false;
                        break;

                    case "descargarCarpetaTrue":
                        nodoActivo.data.permisos.puedeDescargarCarpeta = true;
                        break;
                    case "descargarCarpetaFalse":
                        nodoActivo.data.permisos.puedeDescargarCarpeta = false;
                        break;

                    case "actualizarTrue":
                        nodoActivo.data.permisos.puedeActualizar = true;
                        break;
                    case "actualizarFalse":
                        nodoActivo.data.permisos.puedeActualizar = false;
                        break;

                    case "crearTrue":
                        nodoActivo.data.permisos.puedeCrear = true;
                        break;
                    case "crearFalse":
                        nodoActivo.data.permisos.puedeCrear = false;
                        break;

                    case "todoTrue":
                        nodoActivo.data.permisos.puedeSubir = true;
                        nodoActivo.data.permisos.puedeEliminar = true;
                        nodoActivo.data.permisos.puedeActualizar = true;
                        nodoActivo.data.permisos.puedeDescargarArchivo = true;
                        nodoActivo.data.permisos.puedeDescargarCarpeta = true;
                        nodoActivo.data.permisos.puedeCrear = true;
                        break;
                    case "todoFalse":
                        nodoActivo.data.permisos.puedeSubir = false;
                        nodoActivo.data.permisos.puedeEliminar = false;
                        nodoActivo.data.permisos.puedeActualizar = false;
                        nodoActivo.data.permisos.puedeDescargarArchivo = false;
                        nodoActivo.data.permisos.puedeDescargarCarpeta = false;
                        nodoActivo.data.permisos.puedeCrear = false;
                        break;
                }
            }
        }

        function actualizarCarpetasHijas(nodoActivo, permiso) {

            let nodosHijos = nodoActivo.getChildren();
            nodosHijos.forEach(nodo => {

                if (nodo.hasChildren()) {
                    actualizarCarpetasHijas(nodo, permiso);
                }

                if (!nodo.data.permisos.puedeSubir
                    && !nodo.data.permisos.puedeEliminar
                    && !nodo.data.permisos.puedeDescargarArchivo
                    && !nodo.data.permisos.puedeDescargarCarpeta
                    && !nodo.data.permisos.puedeActualizar
                    && !nodo.data.permisos.puedeCrear) {

                    switch (permiso) {
                        case "subirTrue":
                            nodo.data.permisos.puedeSubir = true;
                            break;
                        case "subirFalse":
                            nodo.data.permisos.puedeSubir = false;
                            break;

                        case "eliminarTrue":
                            nodo.data.permisos.puedeEliminar = true;
                            break;
                        case "eliminarFalse":
                            nodo.data.permisos.puedeEliminar = false;
                            break;

                        case "descargarArchivoTrue":
                            nodo.data.permisos.puedeDescargarArchivo = true;
                            break;
                        case "descargarArchivoFalse":
                            nodo.data.permisos.puedeDescargarArchivo = false;
                            break;

                        case "descargarCarpetaTrue":
                            nodo.data.permisos.puedeDescargarCarpeta = true;
                            break;
                        case "descargarCarpetaFalse":
                            nodo.data.permisos.puedeDescargarCarpeta = false;
                            break;

                        case "actualizarTrue":
                            nodo.data.permisos.puedeActualizar = true;
                            break;
                        case "actualizarFalse":
                            nodo.data.permisos.puedeActualizar = false;
                            break;

                        case "crearTrue":
                            nodo.data.permisos.puedeCrear = true;
                            break;
                        case "crearFalse":
                            nodo.data.permisos.puedeCrear = false;
                            break;

                        case "todoTrue":
                            nodo.data.permisos.puedeSubir = true;
                            nodo.data.permisos.puedeEliminar = true;
                            nodo.data.permisos.puedeActualizar = true;
                            nodo.data.permisos.puedeDescargarArchivo = true;
                            nodo.data.permisos.puedeDescargarCarpeta = true;
                            nodo.data.permisos.puedeCrear = true;
                            break;
                        case "todoFalse":
                            nodo.data.permisos.puedeSubir = false;
                            nodo.data.permisos.puedeEliminar = false;
                            nodo.data.permisos.puedeActualizar = false;
                            nodo.data.permisos.puedeDescargarArchivo = false;
                            nodo.data.permisos.puedeDescargarCarpeta = false;
                            nodo.data.permisos.puedeCrear = false;
                            break;
                    }
                }
            });

        }

        function resetearBotones() {
            listaCheckboxes.forEach(element => {
                element.attr("checked", false);
            });
            listaBotones.forEach(element => {
                element.removeClass("active");
                element.attr("disabled", false);
            });
            botonPermisoTodo.removeClass("active");
            botonPermisoTodo.attr("disabled", false);
        }

        function desactivarBotones() {
            resetearBotones();
            listaBotones.forEach(element => {
                element.attr("disabled", true);
            });
            botonPermisoTodo.attr("disabled", true);
        }

        function activarBotones() {
            resetearBotones();
            listaBotones.forEach(element => {
                element.attr("disabled", false);
            });
            botonPermisoTodo.attr("disabled", false);
        }

        function mostrarPermisosBotones(permiso) {
            resetearBotones();
            let contador = 0;
            if (permiso.puedeSubir) {
                botonPermisoSubir.addClass("active");
                checkboxPermisoSubir.attr("checked", true);
                contador++;
            }
            if (permiso.puedeEliminar) {
                botonPermisoEliminar.addClass("active");
                checkboxPermisoEliminar.attr("checked", true);
                contador++;
            }
            if (permiso.puedeDescargarArchivo) {
                botonPermisoDescargarArchivo.addClass("active");
                checkboxPermisoDescargarArchivo.attr("checked", true);
                contador++;
            }
            if (permiso.puedeDescargarCarpeta) {
                botonPermisoDescargarCarpeta.addClass("active");
                checkboxPermisoDescargarCarpeta.attr("checked", true);
                contador++;
            }
            if (permiso.puedeActualizar) {
                botonPermisoActualizar.addClass("active");
                checkboxPermisoActualizar.attr("checked", true);
                contador++;
            }
            if (permiso.puedeCrear) {
                botonPermisoCrear.addClass("active");
                checkboxPermisoCrear.attr("checked", true);
                contador++;
            }
            if (contador === 6) {
                resetearBotones();
                botonPermisoSubir.removeClass("active");
                checkboxPermisoSubir.attr("checked", true);
                botonPermisoEliminar.removeClass("active");
                checkboxPermisoEliminar.attr("checked", true);
                botonPermisoDescargarArchivo.removeClass("active");
                checkboxPermisoDescargarArchivo.attr("checked", true);
                botonPermisoDescargarCarpeta.removeClass("active");
                checkboxPermisoDescargarCarpeta.attr("checked", true);
                botonPermisoActualizar.removeClass("active");
                checkboxPermisoActualizar.attr("checked", true);
                botonPermisoCrear.removeClass("active");
                checkboxPermisoCrear.attr("checked", true);

                botonPermisoTodo.addClass("active");
                contador = 0;
            }
        }

        function mostrarPermisosBotonesRaiz(permiso) {
            resetearBotones();
            listaBotones.forEach(element => {
                element.attr("disabled", true);
            });
            botonPermisoTodo.attr("disabled", true);
            botonPermisoCrear.attr("disabled", false);

            if (permiso.puedeCrear) {
                botonPermisoCrear.click();
            }
        }

        function LoadTreeMenu(usuarioID) {
            $.blockUI({ message: 'Cargando estructura de datos...' });
            $.ajax({
                datatype: "json",
                type: "POST",
                data: { userID: usuarioID },
                url: "/GestorArchivos/GestorArchivos/cargarEstructuraPermisos",
                success: response => {

                    $.unblockUI();
                    $("#divarbol2").fancytree({

                        checkbox: true,
                        selectMode: 3,
                        quicksearch: true,
                        source: response.menuCompleto,

                        activate: function (event, data) {

                            if (primeraVezActivate) {
                                $(".acciones").fadeIn(1000);
                                $("#btn").show(1500);
                                primeraVezActivate = false;
                            }

                            $("#carpetaActual").html(` para <em>"${data.node.title}"</em>`);

                            if (data.node.selected || data.node.partsel) {
                                activarBotones();
                                if (data.node.getLevel() == 1) {
                                    mostrarPermisosBotonesRaiz(data.node.data.permisos);
                                } else {
                                    mostrarPermisosBotones(data.node.data.permisos);
                                }
                            } else {
                                desactivarBotones();
                            }
                        },
                        select: function (event, data) {

                            if (primeraVezActivate) {
                                $(".acciones").fadeIn(1000);
                                $("#btn").show(1500);
                                primeraVezActivate = false;
                            }

                            data.node.setActive();

                            if (data.node.selected || data.node.partsel) {
                                activarBotones();
                            } else {
                                desactivarBotones();
                            }
                            if (data.node.selected) {
                                if (data.node.getLevel() == 1) {
                                    mostrarPermisosBotonesRaiz(data.node.data.permisos);
                                } else {
                                    mostrarPermisosBotones(data.node.data.permisos);
                                }
                            }
                        },

                        dblclick: (e, data) => {
                            data.node.toggleSelected();
                        },
                        keydown: (e, data) => {
                            if (e.which === 32) {
                                data.node.toggleSelected();
                                return false;
                            }
                        },

                    });
                    let rootNode = $("#divarbol2").fancytree("getRootNode");
                    rootNode.removeChildren();

                    if (response.menuCompleto[0].key != 0) {
                        rootNode.addChildren(response.menuCompleto);
                        $("#numeroCarpetas").html($.ui.fancytree.getTree().count());
                    } else {
                        mostrarModal("Aviso", "Este departamento no cuenta con carpetas en el momento.")
                        $("#numeroCarpetas").html("");
                        return;
                    }
                },
                error: () => {
                    $.unblockUI();
                    mostrarModal("Error", "Error al cargar la estructura de carpetas.")
                }
            });
        }

        $("#btnGuardar").click(() => {

            const tree = $.ui.fancytree.getTree();
            let carpetas = [];

            tree.findAll(node => {
                let obj = {
                    partsel: false,
                    selected: false,
                    key: 0,
                    permisos: {}
                };
                obj.partsel = node.partsel;
                obj.selected = node.selected;
                obj.key = node.key;
                obj.permisos = node.data.permisos;
                carpetas.push(obj);
            });

            let usuarioID;

            if (cboEmpleado.val() === "Seleccione usuario") {
                usuarioID = cboEmpleadoExterno.val();
            } else {
                usuarioID = cboEmpleado.val();
            }

            
            $.blockUI({ message: 'Guardando datos...' });
            $.ajax({
                url: '/GestorArchivos/GestorArchivos/guardarVistasAcciones',
                datatype: "json",
                type: "POST",
                data: {
                    carpetas: carpetas,
                    usuarioID: usuarioID
                },
                success: response => {
                    $.unblockUI();
                    if (response) {
                        mostrarModalGuardado("Aviso", "Cambios guardados con éxito.");
                    } else {
                        mostrarModal("Error", "Ocurrió un error al intentar guardar los cambios.");
                    }
                },
                error: error => {
                    $.unblockUI();
                    mostrarModal("Error", "Ocurrió un error al intentar guardar los cambios.");
                }
            });
        });

        botonModalGuardado.click(() => {
            limpiarCampos();
        });

        botonReset.click(() => {
            limpiarCampos();
        })

        function limpiarCampos() {
            primeraVezActivate = true;
            cboEmpleado[0].selectedIndex = 0;
            cboDepartamento[0].selectedIndex = 0;
            cboEmpleadoExterno[0].selectedIndex = 0;
            botonSeleccionarUsuarioExterno.attr("disabled", false);
            $(".carpetas").hide(1000);
            $(".acciones").fadeOut(1000);
            $("#btn").hide(1500);
            $(".comboUsuarioExterno").hide(1000);
            $(".comboDepartamento").hide(1000);
            $(".comboEmpleado").show(1000);
            $("#numeroCarpetas").html("");
            $("#carpetaActual").html(``);
            $.ui.fancytree.getTree().clear();
        }

        cboEmpleado.change(function () {
            primeraVezActivate = true;
            resetearBotones();
            $("#carpetaActual").html(``);
            if ($(this).val() != "Seleccione usuario") {
                LoadTreeMenu($(this).val());
                $(".carpetas").show(1500);
            } else {
                limpiarCampos();
            }
        });

        botonSeleccionarUsuarioExterno.click(() => {
            botonSeleccionarUsuarioExterno.attr("disabled", true);
            cboDepartamento.fillCombo('/GestorArchivos/GestorArchivos/llenarComboDepartamentos', null, false, "Seleccione departamento");
            $(".comboDepartamento").show(1000);
            cboEmpleado[0].selectedIndex = 0;
            $(".carpetas").hide(1000);
            $(".acciones").fadeOut(1000);
            $("#btn").hide(1500);
            $(".comboEmpleado").hide(1000);
        });

        cboDepartamento.change(function () {
            if ($(this).val() != "Seleccione departamento") {
                cboEmpleadoExterno.fillCombo('/GestorArchivos/GestorArchivos/llenarComboUsuarios', { departamentoID: cboDepartamento.val() }, false, "Seleccione usuario externo");
                $(".comboUsuarioExterno").show(1000);
            } else {
                $(".comboUsuarioExterno").hide(1000);
            }
        });

        cboEmpleadoExterno.change(function () {
            primeraVezActivate = true;
            resetearBotones();
            if ($(this).val() != "Seleccione usuario externo") {
                LoadTreeMenu($(this).val());
                $(".carpetas").show(1500);
            } else {
                $(".carpetas").hide(1000);
                $(".acciones").fadeOut(1000);
                $("#btn").hide(1500);
                $.ui.fancytree.getTree().clear();
            }

        })

        function init() {
            cboEmpleado.fillCombo('/GestorArchivos/GestorArchivos/llenarComboUsuarios', { departamentoID: 0 }, false, "Seleccione usuario");
            agregarEventosBotones();
        }

        init();

    };
    $(document).ready(() => GestorArchivos.GestorArchivos.Permisos = new Permisos());
})();

// #region Código cambiado / pendiente
// $.ajax({
//     url: '/GestorArchivos/GestorArchivos/obtenerAccionesUsuarioPorCarpeta',
//     datatype: "json",
//     type: "POST",
//     data: { carpetaID: data.node.key, usuarioID: cboEmpleado.val() },
//     success: response => {
//         if (data.node.getLevel() == 1) {
//             mostrarPermisosBotonesRaiz(response);
//         } else {
//             mostrarPermisosBotones(response);
//         }
//     },
//     error: error => {
//         mostrarModal("Error", "Error al cargar los permisos de la carpeta.")
//     }
// });
// icon: function (event, data) {
//     if (data.node.getLevel() === 2) {
//         return "my-folder-icon-class";
//     }
// }
// function cambiarIconoCarpetaBase() {
//     // const tree = $.ui.fancytree.getTree();
//     // let carpetasBase = [];
//     // tree.findAll(node => {
//     //     if (node.getLevel() === 2) {
//     //         node.icon = "~/Content/img/ico/folderBase.png";
//     //         node.renderTitle();
//     //     }
//     // });
// }
/*        function obtenerAccionesActivas() {

            const Acciones = {
                puedeSubir: false,
                puedeEliminar: false,
                puedeDescargar: false,
                puedeActualizar: false,
                puedeCrear: false
            };

            if (checkboxPermisoSubir.is(':checked')) {
                Acciones.puedeSubir = true;
            } else {
                Acciones.puedeSubir = false;
            }

            if (checkboxPermisoEliminar.is(':checked')) {
                Acciones.puedeEliminar = true;
            } else {
                Acciones.puedeEliminar = false;
            }

            if (checkboxPermisoDescargar.is(':checked')) {
                Acciones.puedeDescargar = true;
            } else {
                Acciones.puedeDescargar = false;
            }

            if (checkboxPermisoActualizar.is(':checked')) {
                Acciones.puedeActualizar = true;
            } else {
                Acciones.puedeActualizar = false;
            }

            if (checkboxPermisoCrear.is(':checked')) {
                Acciones.puedeCrear = true;
            } else {
                Acciones.puedeCrear = false;
            }

            return Acciones
        }*/
        // #endregion