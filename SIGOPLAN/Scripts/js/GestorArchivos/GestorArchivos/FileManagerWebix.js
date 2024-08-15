let fManager;

const botonMenu = $("#webix-area > div > div.webix_view.webix_fmanager_toolbar.webix_layout_line > div.webix_view.webix_control.webix_el_button.webix_fmanager_back > div > button");

const MENU_BASE = [
    {
        id: "create",
        method: "createFolder",
        icon: "folder-open",
        value: "Crear Carpeta"
    }, {
        id: "edit",
        method: "editFile",
        icon: "edit",
        value: "Renombrar Carpeta"
    }, {
        id: "upload",
        method: "uploadFile",
        icon: "arrow-up",
        value: "Subir"
    }, {
        id: "uploadFolder",
        method: "uploadFolder",
        icon: "caret-square-o-up",
        value: "Subir Carpeta"
    }, {
        id: "downloadFile",
        method: "download",
        icon: "arrow-circle-o-down",
        value: "Descargar"
    }, {
        id: "downloadFolder",
        method: "downloadFolder",
        icon: "archive",
        value: "Descargar Carpeta"
    }, {
        id: "updateFile",
        method: "updateFile",
        icon: "refresh",
        value: "Actualizar"
    }, {
        id: "versionHistory",
        method: "versionHistory",
        icon: "clock-o",
        value: "Historial de Versiones"
    }, {
        id: "remove",
        method: "deleteFile",
        icon: "trash",
        value: "Eliminar"
    }
];

function mostrarPermisosCarpetaRaiz(permisos) {

    fManager.getMenu().clearAll();
    MENU_BASE.forEach(element => {
        fManager.getMenu().add(element);
    });
    let actionMenu = fManager.getMenu();
    if (!permisos.puedeCrear) {
        actionMenu.remove("create");
    }
    actionMenu.remove("edit");
    actionMenu.remove("updateFile");
    actionMenu.remove("downloadFile");
    actionMenu.remove("downloadFolder");
    actionMenu.remove("versionHistory");
    actionMenu.remove("remove");
    actionMenu.remove("upload");
    actionMenu.remove("uploadFolder");
    actionMenu.refresh();
}

function mostrarPermisos(permisos, esCarpeta) {
    if (permisos == null) {
        fManager.getMenu().clearAll();
        actionMenu.refresh();
        return;
    }
    let total = 0;
    fManager.getMenu().clearAll();
    MENU_BASE.forEach(element => {
        fManager.getMenu().add(element);
    });

    let actionMenu = fManager.getMenu();

    if (!permisos.puedeActualizar) {
        actionMenu.remove("updateFile");
        total++;
    }
    if (!permisos.puedeCrear) {
        actionMenu.remove("create");
        actionMenu.remove("edit");
        total++;
    }
    if (!permisos.puedeDescargarArchivo) {
        actionMenu.remove("downloadFile");
        actionMenu.remove("versionHistory");
        total++;
    }
    if (!permisos.puedeDescargarCarpeta) {
        actionMenu.remove("downloadFolder");
        total++;
    }
    if (esCarpeta) {
        actionMenu.remove("downloadFile");
        actionMenu.remove("versionHistory");
        actionMenu.remove("updateFile");
    } else {
        actionMenu.remove("edit");
        actionMenu.remove("downloadFolder");
    }
    if (!permisos.puedeEliminar) {
        actionMenu.remove("remove");
        total++;
    }
    if ((!permisos.puedeSubir) && (!permisos.puedeCrear)) {
        actionMenu.remove("uploadFolder");
        total++;
    }
    if (!permisos.puedeSubir) {
        actionMenu.remove("upload");
        total++;
    }
    if (total === 7) {
        actionMenu.clearAll();
    }

    actionMenu.refresh();
}

function mostrarModal(titulo, cuerpo) {
    const modal = $("#modalAviso");
    $("#modalAvisoTitutlo").html(titulo);
    $("#modalAvisoCuerpo").text(cuerpo);
    modal.modal('show');
}

function obtenerHistorialVersiones(archivoID) {
    $.blockUI({ message: "Obteniendo historial de versiones del archivo" });
    $.ajax({
        url: '/GestorArchivos/GestorArchivos/obtenerHistorialVeriones',
        datatype: "json",
        data: {
            id: archivoID
        },
        success: function (response) {
            $.unblockUI();
            if (response.length > 0) {
                mostrarTablaVersiones(response);
            } else {
                mostrarModal("Error", "Ocurrió un error al intentar obtener el historial de versiones del archivo.");
            }
        },
        error: function (error) {
            $.unblockUI();
            mostrarModal("Error", "Ocurrió un error al intentar obtener el historial de versiones del archivo.");
        }
    });
};

function mostrarTablaVersiones(listaVersiones) {
    if (null === listaVersiones) {
        mostrarModal("Error", "Ocurrió un error al intentar cargar las diferentes versiones del archivo.")
        return;
    }

    let tablaVersiones = `
    <table id="tablaVersion" class="table table-hover table-bordered text-center tablaVersiones">
        <thead>
            <tr>
                <th>Nombre</th>
                <th>Fecha de Creación</th>
                <th>Usuario</th>
                <th>Version</th>
                <th>Descargar Archivo</th>
            </tr>
        </thead>
        <tbody>`;
    listaVersiones.forEach(function (element) {
        const tr = `
            <tr id="${element.id}">
                <td>${element.value}</td>
                <td>${element.date}</td>
                <td>${element.usuario}</td>
                <td>${element.version}</td>
                <td><button class="botonDescargarVersion"><i class="fa fa-arrow-circle-o-down"></i> Descargar</button></td>
            </tr>`;
        tablaVersiones += tr;
    });
    tablaVersiones += `
        </tbody>
    </table>`;

    this.webix.modalbox({
        title: "Historial de Versiones",
        buttons: ["Ok"],
        width: 1200,
        text: tablaVersiones
    });
    $(".botonDescargarVersion").click(function () {
        const archivoID = $(this).closest("tr").attr("id");
        fManager.downloadVersion(archivoID);
    });
}

webix.type(webix.ui.tree, {
    name: "FileTree",
    css: "webix_fmanager_tree",
    folder: function (t) {
        return t.$count && t.open ? "<div class='webix_icon icon fa-folder-open'></div>" : "<div class='webix_icon icon fa-folder'></div>"
    }
}),

    webix.type(webix.ui.dataview, {
        name: "FileView",
        css: "webix_fmanager_files",
        height: 10,
        margin: 10,
        width: 200,
        template: function (t, e) {
            var i = t.type || "file";
            i = e.icons[i] || e.icons.file;
            var s = "webix_fmanager_data_icon"
                , n = e.templateName(t, e);
            return "<div class='webix_fmanager_file'><div class='" + s + "'>" + e.templateIcon(t, e) + "</div>" + n + "</div>"
        }
    }),

    //Diccionario de valor del texto
    webix.i18n.filemanager = {
        name: "Nombre",
        size: "Tamaño",
        type: "Tipo de Archivo",
        date: "Fecha de Creación",
        copy: "Copiar",
        cut: "Cortar",
        paste: "Pegar",
        upload: "Subir",
        updateFile: "Actualizar Archivo",
        downloadFile: "Descargar Archivo",
        versionHistory: "Historial de versiones",
        remove: "Borrar",
        create: "Crear Carpeta",
        rename: "Renombrar carpeta",
        location: "Ubicación",
        select: "Seleccionar Archivo",
        sizeLabels: ["B", "KB", "MB", "GB"],
        saving: "Guardando...",
        errorResponse: "Error: ¡Los cambios no pudieron ser guardados!",
        replaceConfirmation: "La carpeta ya contiene archivos con esos nombres. ¿Quiere reemplazar los archivos existentes?",
        createConfirmation: "Una carpeta con ese nombre ya existe. ¿Quiere reemplazarla?",
        renameConfirmation: "Ya existe un archivo con ese nombre. ¿Quiere reemplazarlo?",
        yes: "Si",
        no: "No",
        types: {
            folder: "Carpeta",
            file: "Archivo",
            txt: "Archivo de Texto",
            pdf: "Formato de Documento Portátil",
            rar: "Colección de Archivos",
            zip: "Colección de Archivos",
            javascript: "JavaScript",
            txt: "Archivo de Texto",
            jpg: "Imagen",
            jpeg: "Imagen",
            tiff: "Imagen",
            ico: "Ícono",
            png: "Imagen",
            gif: "Imagen",
            bmp: "Imagen",
            doc: "Documento 97-2003",
            docx: "Documento",
            docm: "Documento habilitado para macros",
            dotx: "Plantilla",
            dotm: "Plantilla habilitada para macros",
            xls: "Libro 97-2003",
            xlsx: "Libro",
            xlsm: "Libro habilitado para macros",
            xltx: "Plantilla",
            xltm: "Plantilla habilitada para macros",
            xlsb: "Libro binario no XML",
            xlam: "Complemento habilitado para macros",
            ppt: "Presentación 97-2003",
            pptx: "Presentación",
            pptm: "Presentación habilitada para macros",
            potx: "Plantilla",
            potm: "Plantilla habilitada para macros",
            ppam: "Complemento habilitado para macros",
            ppsx: "Presentación con diapositivas",
            ppsm: "Presentación con diapositivas habilitada para macros",
            sldx: "Diapositiva",
            sldm: "Diapositiva habilitada para macros",
        }
    },

    webix.protoUI({
        name: "filetree"
    }, webix.EditAbility, webix.ui.tree),

    webix.protoUI({
        name: "fileview"
    }, webix.EditAbility, webix.ui.dataview),

    webix.protoUI({
        name: "filetable",
        $dragHTML: function (t) {
            var e = "<div class='webix_dd_drag webix_fmanager_drag' >"
                , i = this.getColumnIndex("value");
            return e += "<div style='width:auto'>" + this.config.columns[i].template(t, this.type) + "</div>",
                e + "</div>"
        }
    }, webix.ui.datatable),

    webix.protoUI({
        name: "path",
        defaults: {
            layout: "x",
            separator: ",",
            scroll: false
        },
        $skin: function () {
            this.type.height = webix.skin.$active.buttonHeight || webix.skin.$active.inputHeight
        },
        $init: function () {
            this.$view.className += " webix_path"
        },
        value_setter: function (t) {
            return this.setValue(),
                t
        },
        setValue: function (t) {
            this.clearAll(),
                t && ("string" == typeof t && (t = t.split(this.config.separator)),
                    this.parse(webix.copy(t)))
        },
        getValue: function () {
            return this.serialize()
        }
    }, webix.ui.list),

    webix.FileManagerStructure = {
        structure: {

            //#region Actions
            actions: {

                config: function () {
                    var t = this.config.templateName;
                    return {
                        view: "contextmenu",
                        width: 200,
                        padding: 0,
                        autofocus: !1,
                        css: "webix_fmanager_actions",
                        template: function (e, i) {
                            var s = t(e, i);
                            return "<span class='fa fa-" + e.icon + "'></span> " + s
                        },
                        data: "actionsData"
                    }
                },

                oninit: function () {
                    var t = this.getMenu();
                    t.$q = !1,
                        t && (this.getMenu().attachEvent("onItemClick", webix.bind(function (e, i) {
                            var s = this.getMenu().getItem(e)
                                , n = this[s.method] || this[e];
                            if (n) {
                                var a = this.getActive();
                                if (this.callEvent("onbefore" + (s.method || e), [a])) {
                                    ("upload" != e || !webix.isUndefined(XMLHttpRequest) && !webix.isUndefined((new XMLHttpRequest).upload)) && (t.Uq(!0),
                                        t.hide());
                                    var r = [a];
                                    "upload" == e && (i = webix.html.pos(i),
                                        r.push(i)),
                                        webix.delay(function () {
                                            n.apply(this, r),
                                                this.callEvent("onafter" + (s.method || e), [])
                                        }, this)
                                }
                            }
                        }, this)),
                            this.getMenu().attachEvent("onBeforeShow", function (t) {
                                var e = this.getContext();
                                return e && e.obj ? e.obj.callEvent("onBeforeMenuShow", [e.id, t]) : !0
                            }))
                }
            },
            //#endregion

            //#region ActionsData
            actionsData: {
                config: function () {
                    return [{
                        id: "create",
                        method: "createFolder",
                        icon: "folder-open",
                        value: webix.i18n.filemanager.create
                    },
                    {
                        id: "edit",
                        method: "editFile",
                        icon: "edit",
                        value: webix.i18n.filemanager.rename
                    }, {
                        id: "upload",
                        method: "uploadFile",
                        icon: "cloud-upload",
                        value: webix.i18n.filemanager.upload
                    }, {
                        id: "uploadFolder",
                        method: "uploadFolder",
                        icon: "caret-square-o-up",
                        value: "Subir carpeta"
                    }, {
                        id: "downloadFile",
                        method: "download",
                        icon: "arrow-circle-o-down",
                        value: webix.i18n.filemanager.downloadFile
                    }, {
                        $template: "Separator"
                    }, {
                        id: "updateFile",
                        method: "updateFile",
                        icon: "refresh",
                        value: webix.i18n.filemanager.updateFile
                    }, {
                        id: "versionHistory",
                        method: "versionHistory",
                        icon: "clock-o",
                        value: webix.i18n.filemanager.versionHistory
                    }, {
                        id: "remove",
                        method: "deleteFile",
                        icon: "trash",
                        value: webix.i18n.filemanager.remove
                    }
                    ]
                }
            },
            //#endregion

            mainLayout: {
                type: "clean",
                rows: "mainRows"
            },
            mainRows: ["toolbar", "bodyLayout"],
            toolbar: {
                css: "webix_fmanager_toolbar",
                paddingX: 10,
                paddingY: 5,
                margin: 7,
                cols: "toolbarElements"
            },
            toolbarElements: [/*"menu", */
                // {
                //     id: "menuSpacer",
                //     width: 70
                // }, 
                {
                    margin: 2,
                    cols: ["back", "forward"]
                }, "up", "path", "search"/*, "modes"*/],
            menu: {
                config: {
                    view: "button",
                    type: "iconButton",
                    css: "webix_fmanager_back",
                    icon: "bars",
                    width: 1
                },
                oninit: function () {
                    this.$$("menu") && (this.$$("menu").attachEvent("onItemClick", webix.bind(function () {
                        this.callEvent("onBeforeMenu", []) && (this.getMenu().nh = null,
                            this.getMenu().show(this.$$("menu").$view),
                            this.callEvent("onAfterMenu", []))
                    }, this)),
                        this.config.readonly && (this.$$("menu").hide(),
                            this.$$("menuSpacer") && this.$$("menuSpacer").hide()))
                }
            },
            back: {
                config: {
                    view: "button",
                    type: "iconButton",
                    css: "webix_fmanager_back",
                    icon: "angle-left",
                    width: 37
                },
                oninit: function () {
                    this.$$("back") && this.$$("back").attachEvent("onItemClick", webix.bind(function () {
                        this.callEvent("onBeforeBack", []) && (this.goBack(),
                            this.callEvent("onAfterBack", []))
                    }, this))
                }
            },
            forward: {
                config: {
                    view: "button",
                    type: "iconButton",
                    css: "webix_fmanager_forward",
                    icon: "angle-right",
                    width: 37
                },
                oninit: function () {
                    this.$$("forward") && this.$$("forward").attachEvent("onItemClick", webix.bind(function () {
                        this.callEvent("onBeforeForward", []) && (this.goForward(),
                            this.callEvent("onAfterForward", []))
                    }, this))
                }
            },
            up: {
                config: {
                    view: "button",
                    type: "iconButton",
                    css: "webix_fmanager_up",
                    icon: "level-up",
                    disable: !0,
                    width: 37
                },
                oninit: function () {
                    this.$$("up") && this.$$("up").attachEvent("onItemClick", webix.bind(function () {
                        this.callEvent("onBeforeLevelUp", []) && (this.levelUp(),
                            this.callEvent("onAfterLevelUp", []))
                    }, this))
                }
            },
            path: {
                config: {
                    view: "path",
                    borderless: !0
                },
                oninit: function () {
                    this.$$("path") && (this.attachEvent("onFolderSelect", webix.bind(function (t) {
                        this.$$("path").setValue(this.getPathNames(t))
                    }, this)),
                        this.$$("path").attachEvent("onItemClick", webix.bind(function (t) {
                            var e = this.$$("path").getIndexById(t)
                                , i = this.$$("path").count() - e - 1;
                            if (this.$searchResults && this.hideSearchResults(),
                                i) {
                                for (t = this.getCursor(); i;) {
                                    if (window.CP.shouldStopExecution(1)) {
                                        break;
                                    }
                                    if (window.CP.shouldStopExecution(1)) {
                                        break;
                                    }
                                    t = this.getParentId(t),
                                        i--;
                                    window.CP.exitedLoop(1);
                                }
                                window.CP.exitedLoop(1);

                                this.setCursor(t)
                            }
                            this.callEvent("onAfterPathClick", [t])
                        }, this)),
                        this.data.attachEvent("onClearAll", webix.bind(function () {
                            this.clearAll()
                        }, this.$$("path"))))
                }
            },
            search: {
                config: {
                    view: "search",
                    gravity: .3,
                    css: "webix_fmanager_search"
                },
                oninit: function () {
                    var t = this.$$("search");
                    t && (t.attachEvent("onTimedKeyPress", webix.bind(function () {
                        if (9 != this.cx) {
                            var e = t.getValue();
                            e ? this.callEvent("onBeforeSearch", [e]) && (this.showSearchResults(e),
                                this.callEvent("onAfterSearch", [e])) : this.$searchResults && this.hideSearchResults()
                        }
                    }, this)),
                        t.attachEvent("onKeyPress", function (t) {
                            this.cx = t
                        }),
                        this.attachEvent("onAfterModeChange", function () {
                            this.$searchResults && this.showSearchResults(t.getValue())
                        }))
                }
            },
            bodyLayout: {
                css: "webix_fmanager_body",
                cols: "bodyCols"
            },
            bodyCols: ["tree", {
                view: "resizer",
                width: 2
            }, "modeViews"],
            tree: {
                config: {
                    width: 251,
                    view: "filetree",
                    id: "tree",
                    select: !0,
                    filterMode: {
                        showSubItems: !1,
                        openParents: !1
                    },
                    type: "FileTree",
                    navigation: !0,
                    scroll: "xy",
                    editor: "text",
                    editable: !0,
                    editaction: !1,
                    drag: !1,
                    tabFocus: !0,
                    onContext: {}
                },
                oninit: function () {
                    var tree = this.$$("tree");
                    if (tree) {
                        tree.type.icons = this.config.icons,
                            tree.sync(this, function () {
                                this.filter(function (t) {
                                    return t.$count || "folder" == t.type
                                })
                            }),
                            tree.attachEvent("onAfterSelect", webix.bind(function (t) {
                                this.callEvent("onFolderSelect", [t])
                            }, this)),
                            this.attachEvent("onAfterCursorChange", function (e) {
                                e && (tree.select(e),
                                    tree.open(this.getParentId(e)))
                            }),
                            tree.attachEvent("onItemClick", webix.bind(function () {
                                this.$searchResults && this.hideSearchResults()
                            }, this)),
                            tree.attachEvent("onItemDblClick", function (t) {
                                this.isBranchOpen(t) ? this.close(t) : this.open(t)
                            }),
                            tree.attachEvent("onBlur", function () {
                                var t = this.getTopParentView();
                                t.getMenu() && t.getMenu().isVisible() || webix.html.addCss(this.$view, "webix_blur")
                            }),
                            tree.attachEvent("onFocus", webix.bind(function () {
                                this.dx = tree,
                                    webix.html.removeCss(tree.$view, "webix_blur"),
                                    this.$$(this.config.mode).unselect()
                            }, this)),
                            this.attachEvent("onPathLevel", function (e) {
                                tree.open(e)
                            }),
                            this.attachEvent("onPathComplete", function (e) {
                                tree.showItem(e)
                            }),
                            this.config.readonly || (this.getMenu() && this.getMenu().attachTo(tree),
                                tree.attachEvent("onBeforeMenuShow", function (t) {
                                    this.select(t),
                                        webix.UIManager.setFocus(this)
                                })),
                            tree.attachEvent("onBeforeEditStop", webix.bind(function (e, i) {
                                return this.callEvent("onBeforeEditStop", [i.id, e, i, tree])
                            }, this)),
                            tree.attachEvent("onAfterEditStop", webix.bind(function (e, i) {
                                this.callEvent("onAfterEditStop", [i.id, e, i, tree]) && this.renameFile(i.id, e.value)
                            }, this)),
                            tree.attachEvent("onBeforeDrag", function (t, e) {
                                var i = this.getTopParentView();
                                return !i.config.readonly && i.callEvent("onBeforeDrag", [t, e])
                            }),
                            tree.attachEvent("onBeforeDragIn", function (t, e) {
                                var i = this.getTopParentView();
                                return !i.config.readonly && i.callEvent("onBeforeDragIn", [t, e])
                            }),
                            tree.attachEvent("onBeforeDrop", function (t, e) {
                                var i = this.getTopParentView();
                                return i.callEvent("onBeforeDrop", [t, e]) && t.from && (i.moveFile(t.source, t.target),
                                    i.callEvent("onAfterDrop", [t, e])),
                                    !1
                            });
                        var e = function () {
                            tree && webix.UIManager.setFocus(tree)
                        };
                        this.attachEvent("onAfterBack", e),
                            this.attachEvent("onAfterForward", e),
                            this.attachEvent("onAfterLevelUp", e),
                            this.attachEvent("onAfterPathClick", e),
                            this.config.readonly && (tree.define("drag", !1),
                                tree.define("editable", !1))
                    }
                }
            },
            modeViews: {
                config: function (t) {
                    var e = [];
                    if (t.modes)
                        for (var i = 0; i < t.modes.length; i++) {
                            if (window.CP.shouldStopExecution(2)) {
                                break;
                            }
                            if (window.CP.shouldStopExecution(2)) {
                                break;
                            }
                            e.push(t.modes[i]);
                        }
                    window.CP.exitedLoop(2);

                    window.CP.exitedLoop(2);

                    return {
                        animate: !1,
                        cells: e
                    }
                },
                oninit: function () {
                    this.$$(this.config.mode) && this.$$(this.config.mode).show(),
                        this.attachEvent("onBeforeCursorChange", function () {
                            return this.$$(this.config.mode).unselect(),
                                !0
                        });
                    var t = this.config.modes;
                    if (t)
                        for (var e = 0; e < t.length; e++) {
                            if (window.CP.shouldStopExecution(3)) {
                                break;
                            }
                            if (window.CP.shouldStopExecution(3)) {
                                break;
                            }
                            this.$$(t[e]) && this.$$(t[e]).filter && this.ex(t[e])
                        }
                    window.CP.exitedLoop(3);

                    window.CP.exitedLoop(3);

                }
            },
            modes: {
                config: function (t) {
                    var e = 0
                        , i = this.structure.modeOptions;
                    if (i)
                        for (var s = 0; s < i.length; s++) {
                            if (window.CP.shouldStopExecution(4)) {
                                break;
                            }
                            if (window.CP.shouldStopExecution(4)) {
                                break;
                            }
                            i[s].width && (e += i[s].width + (i.length ? 1 : 0));
                        }
                    window.CP.exitedLoop(4);

                    window.CP.exitedLoop(4);

                    var n = {
                        view: "segmented",
                        options: "modeOptions",
                        css: "webix_fmanager_modes",
                        value: t.mode
                    };
                    return e && (n.width = e + 4),
                        n
                },
                oninit: function () {
                    this.$$("modes") && this.$$("modes").attachEvent("onBeforeTabClick", webix.bind(function (t) {
                        var e = this.$$("modes").getValue();
                        return this.callEvent("onBeforeModeChange", [e, t]) && this.$$(t) ? (this.config.mode = t,
                            this.$$(t).show(),
                            this.callEvent("onAfterModeChange", [e, t]),
                            !0) : !1
                    }, this))
                }
            },
            modeOptions: [
                {
                    id: "table",
                    width: 32,
                    value: '<span class="webix_fmanager_mode_option webix_icon fa-list-ul"></span>'
                }],
            files: {
                config: {
                    view: "fileview",
                    type: "FileView",
                    select: "cell",
                    editable: !0,
                    editaction: !1,
                    editor: "text",
                    editValue: "value",
                    drag: false,
                    navigation: !0,
                    tabFocus: !0,
                    onContext: {}
                }
            },
            table: {
                config: {
                    view: "filetable",
                    css: "webix_fmanager_table",
                    columns: "columns",
                    editable: !0,
                    scroll: true,
                    editaction: true,
                    select: "cell",
                    drag: false,
                    navigation: !0,
                    resizeColumn: !0,
                    tabFocus: !0,
                    onContext: {}
                },
                oninit: function () {
                    this.$$("table") && (this.attachEvent("onHideSearchResults", function () {
                        this.$$("table").isColumnVisible("location") && this.$$("table").hideColumn("location")
                    }),
                        this.attachEvent("onShowSearchResults", function () {
                            this.$$("table").isColumnVisible("location") || this.$$("table").showColumn("location")
                        }),
                        this.$$("table").attachEvent("onBeforeEditStart", function (t) {
                            return this.fx ? !0 : "object" == typeof t ? !1 : (this.fx = !0,
                                this.edit({
                                    row: t,
                                    column: "value"
                                }),
                                this.fx = !1,
                                !1)
                        }))
                }
            },
            columns: {
                config: function () {
                    var t = webix.i18n.filemanager
                        , e = this;
                    return [{
                        id: "value",
                        header: t.name,
                        fillspace: 2,
                        template: function (t, e) {
                            var i = e.templateName(t, e);
                            return e.templateIcon(t, e) + i
                        },
                        sort: "string",
                        editor: "text"
                    }, {
                        id: "date",
                        header: t.date,
                        fillspace: 0.7,
                        template: function (t, e) {
                            return e.templateDate(t, e)
                        },
                        sort: "date"
                    }, {
                        id: "type",
                        header: t.type,
                        fillspace: 1.5,
                        sort: "string",
                        template: function (t, e) {
                            return e.templateType(t)
                        }
                    }, {
                        id: "location",
                        header: t.location,
                        fillspace: 2,
                        template: function (t) {
                            for (var i = e.getPathNames(t.id), s = [], n = 0; n < i.length - 1; n++) {
                                if (window.CP.shouldStopExecution(5)) {
                                    break;
                                }
                                if (window.CP.shouldStopExecution(5)) {
                                    break;
                                }
                                s.push(i[n].value);
                                window.CP.exitedLoop(5);
                            }
                            window.CP.exitedLoop(5);

                            return s.join("/")
                        },
                        sort: "string"
                    },
                    ]
                }
            },
            upload: {
                config: function () {
                    var t = {};
                    return t = webix.isUndefined(XMLHttpRequest) || webix.isUndefined((new XMLHttpRequest).upload) ? {
                        view: "uploader",
                        css: "webix_upload_select_ie",
                        type: "iconButton",
                        icon: "check",
                        label: webix.i18n.filemanager.select,
                        formData: {
                            action: "upload"
                        }
                    } : {
                            view: "uploader",
                            apiOnly: !0,
                            formData: {
                                action: "upload"
                            }
                        }
                },
                oninit: function () {
                    var t = this.getUploader();
                    if (t) {
                        t.config.upload = this.config.handlers.upload;
                        var e = this.config.modes;
                        if (e)
                            for (var i = 0; i < e.length; i++) {
                                if (window.CP.shouldStopExecution(6)) {
                                    break;
                                }
                                if (window.CP.shouldStopExecution(6)) {
                                    break;
                                }
                                this.$$(e[i]) && t.addDropZone(this.$$(e[i]).$view);
                            }
                        window.CP.exitedLoop(6);

                        window.CP.exitedLoop(6);

                        t.attachEvent("onBeforeFileAdd", webix.bind(function (e) {
                            return e.oldId = e.id,
                                t.config.formData.target = this.gx(),
                                this.callEvent("onBeforeFileUpload", [e])
                        }, this)),
                            t.attachEvent("onAfterFileAdd", webix.bind(function (e) {
                                this.hx = null,
                                    this.add({
                                        id: e.id,
                                        value: e.name,
                                        type: e.type,
                                        size: e.size,
                                        date: Math.round((new Date).valueOf() / 1e3)
                                    }, -1, t.config.formData.target),
                                    this.config.uploadProgress && this.showProgress(this.config.uploadProgress),
                                    this.refreshCursor()
                            }, this)),
                            t.attachEvent("onFileUpload", webix.bind(function (t) {
                                t.oldId && this.data.changeId(t.oldId, t.id),
                                    this.getItem(t.id).type = t.type,
                                    this.refreshCursor(),
                                    this.hideProgress()
                            }, this)),
                            t.attachEvent("onFileUploadError", webix.bind(function (t, e) {
                                this.ix(t, e),
                                    this.hideProgress()
                            }, this))
                    }
                }
            }
        }
    },

    webix.FileManagerUpload = {
        px: function () {
            var t = webix.copy(this.structure.upload)
                , e = this.qx(t, this.config);
            e && (webix.isUndefined(XMLHttpRequest) || webix.isUndefined((new XMLHttpRequest).upload) ? (this.Ix = webix.ui({
                view: "popup",
                padding: 0,
                width: 250,
                body: e
            }),
                this.rx = this.Ix.getBody(),
                this.attachEvent("onDestruct", function () {
                    this.Ix.destructor()
                })) : (this.rx = webix.ui(e),
                    this.attachEvent("onDestruct", function () {
                        this.rx.destructor()
                    })),
                t.oninit && t.oninit.call(this))
        },
        getUploader: function () {
            return this.rx
        },
        gx: function () {
            return this.hx || this.getCursor()
        },
        uploadFile: function (t, e) {
            $("#subirArchivo").click();
        },
        uploadFolder: function (t, e) {
            $("#subirFolder").click();
        }
    },
    webix.protoUI({
        name: "filemanager",
        $init: function (t) {
            this.$view.className += " webix_fmanager",
                webix.extend(this.data, webix.TreeStore, !0),
                webix.extend(t, this.defaults),
                this.data.provideApi(this, !0),
                this.jx = webix.extend([], webix.PowerArray, !0),
                this.Pw(t),
                this.$ready.push(this.kx),
                webix.UIManager.tabControl = !0,
                webix.extend(t, this.zv(t))
        },
        kx: function () {
            this.lx(),
                this.attachEvent("onAfterLoad", function () {
                    if (!this.config.disabledHistory) {
                        var t = window.location.hash;
                        t && 0 === t.indexOf("#!/") && this.setPath(t.replace("#!/", ""))
                    }
                    this.getCursor() || this.setCursor(this.Rw())
                }),
                this.attachEvent("onFolderSelect", function (t) {
                    this.setCursor(t)
                }),
                this.attachEvent("onAfterCursorChange", function (t) {
                    this.mx || (this.nx || this.jx.splice(1),
                        20 == this.jx.length && this.jx.splice(0, 1),
                        this.jx.push(t),
                        this.nx = this.jx.length - 1),
                        this.mx = !1,
                        this.config.disabledHistory || this.ox(t)
                }),
                this.attachEvent("onBeforeDragIn", function (t) {
                    var e = t.target;
                    if (e)
                        for (var i = t.source, s = 0; s < i.length; s++) {
                            if (window.CP.shouldStopExecution(8)) {
                                break;
                            }
                            if (window.CP.shouldStopExecution(8)) {
                                break;
                            }
                            for (; e;) {
                                if (window.CP.shouldStopExecution(7)) {
                                    break;
                                }
                                if (window.CP.shouldStopExecution(7)) {
                                    break;
                                }
                                if (e == i[s])
                                    return !1;
                                e = this.getParentId(e)
                            }
                            window.CP.exitedLoop(7);

                            window.CP.exitedLoop(8);

                            window.CP.exitedLoop(7);
                        }
                    window.CP.exitedLoop(8);

                    return !0
                }),
                this.px()
        },
        ox: function (t) {
            t = t || this.getCursor(),
                window.history && window.history.replaceState ? window.history.replaceState({
                    webix: !0,
                    id: this.config.id,
                    value: t
                }, "", "#!/" + t) : window.location.hash = "#!/" + t
        },
        zv: function (t) {
            var e = this.structure.mainLayout
                , i = webix.extend({}, e.config || e);
            return this.Sw(i, t),
                t.on && t.on.onViewInit && t.on.onViewInit.apply(this, [t.id || "mainLayout", i]),
                webix.callEvent("onViewInit", [t.id || "mainLayout", i, this]),
                i
        },
        updateStructure: function () {
            var t = this.zv()
                , e = this.mc ? "rows" : "cols";
            this.define(e, t[e]),
                this.reconstruct()
        },
        Sw: function (t, e) {
            var i, s, n, a, r = "", o = ["rows", "cols", "elements", "cells", "columns", "options", "data"];
            for (n = 0; n < o.length; n++) {
                if (window.CP.shouldStopExecution(9)) {
                    break;
                }
                if (window.CP.shouldStopExecution(9)) {
                    break;
                }
                t[o[n]] && (r = o[n],
                    i = t[r]);
            }
            window.CP.exitedLoop(9);

            window.CP.exitedLoop(9);

            if (i)
                for ("string" == typeof i && this.structure[i] && (t[r] = this.qx(this.structure[i], e),
                    i = t[r]),
                    n = 0; n < i.length; n++) {
                    if (window.CP.shouldStopExecution(10)) {
                        break;
                    }
                    if (window.CP.shouldStopExecution(10)) {
                        break;
                    }
                    if (s = null,
                        "string" == typeof i[n])
                        if (s = a = i[n],
                            this.structure[a]) {
                            var h = webix.extend({}, this.structure[a]);
                            i[n] = this.qx(h, e),
                                i[n].id = a,
                                h.oninit && this.$ready.push(h.oninit)
                        } else
                            i[n] = {};
                    this.Sw(i[n], e),
                        s && (e.on && e.on.onViewInit && e.on.onViewInit.apply(this, [s, i[n]]),
                            webix.callEvent("onViewInit", [s, i[n], this]))
                }
            window.CP.exitedLoop(10);

            window.CP.exitedLoop(10);

        },
        lx: function () {
            if (this.structure.actions) {
                var t = webix.copy(this.structure.actions)
                    , e = t.config || t;
                "function" == typeof e && (e = e.call(this)),
                    this.Sw(e, this.config),
                    this.sx = webix.ui(e),
                    this.attachEvent("onDestruct", function () {
                        this.sx.destructor()
                    }),
                    t.oninit && this.$ready.push(t.oninit)
            }
        },
        getMenu: function () {
            return this.sx
        },
        getPath: function (t) {
            t = t || this.getCursor();
            for (var e = null, i = []; t && this.getItem(t);) {
                if (window.CP.shouldStopExecution(11)) {
                    break;
                }
                if (window.CP.shouldStopExecution(11)) {
                    break;
                }
                e = this.getItem(t),
                    i.push(t),
                    t = this.getParentId(t);
                window.CP.exitedLoop(11);
            }
            window.CP.exitedLoop(11);

            return i.reverse()
        },
        getPathNames: function (archivoID) {
            archivoID = archivoID || this.getCursor();
            for (var e = null, i = []; archivoID && this.getItem(archivoID);) {
                if (window.CP.shouldStopExecution(12)) {
                    break;
                }
                if (window.CP.shouldStopExecution(12)) {
                    break;
                }
                e = this.getItem(archivoID),
                    i.push({
                        id: archivoID,
                        value: this.config.templateName(e)
                    }),
                    archivoID = this.getParentId(archivoID);
                window.CP.exitedLoop(12);
            }
            window.CP.exitedLoop(12);

            return i.reverse()
        },
        setPath: function (t) {
            for (var e = t; e && this.getItem(e);) {
                if (window.CP.shouldStopExecution(13)) {
                    break;
                }
                if (window.CP.shouldStopExecution(13)) {
                    break;
                }
                this.callEvent("onPathLevel", [e]),
                    e = this.getParentId(e);
            }
            window.CP.exitedLoop(13);

            window.CP.exitedLoop(13);

            this.setCursor(t),
                this.callEvent("onPathComplete", [t])
        },
        tx: function (t) {
            if (this.jx.length > 1) {
                var e = this.nx + t;
                e > -1 && e < this.jx.length && (this.mx = !0,
                    this.setCursor(this.jx[e]),
                    this.nx = e)
            }
            return this.getCursor()
        },
        getSearchData: function (t, e) {
            var i = [];
            return this.data.each(function (t) {
                var s = this.config.templateName(t);
                s.toLowerCase().indexOf(e.toLowerCase()) >= 0 && i.push(webix.copy(t))
            }, this, !0, t),
                i
        },
        showSearchResults: function (t) {
            this.callEvent("onShowSearchResults", []);
            var e = this.getSearchData(this.getCursor(), t);
            this.$searchResults = !0,
                this.$$(this.config.mode).filter && (this.$$(this.config.mode).clearAll(),
                    this.$$(this.config.mode).parse(e))
        },
        hideSearchResults: function () {
            this.callEvent("onHideSearchResults", []),
                this.$searchResults = !1;
            var t = this.getCursor();
            this.ib = null,
                this.setCursor(t)
        },
        goBack: function (t) {
            return t = t ? -1 * Math.abs(t) : -1,
                this.tx(t)
        },
        goForward: function (t) {
            return this.tx(t || 1)
        },
        levelUp: function (t) {
            t = t || this.getCursor(),
                t && (t = this.getParentId(t),
                    this.setCursor(t))
        },
        markCopy: function (t) {
            t && (webix.isArray(t) || (t = [t]),
                this.ux = t,
                this.vx = !0)
        },
        markCut: function (t) {
            t && (webix.isArray(t) || (t = [t]),
                this.ux = t,
                this.vx = !1)
        },
        pasteFile: function (t) {
            webix.isArray(t) && (t = t[0]),
                t && (t = t.toString(),
                    this.data.branch[t] && "folder" == this.getItem(t).type && this.ux && (this.vx ? this.copyFile(this.ux, t) : this.moveFile(this.ux, t)))
        },
        downloadVersion: function (archivoID) {
            const e = this.config.handlers.download;
            e && webix.send(e, {
                action: "downloadVersion",
                source: archivoID
            })
        },
        downloadFolder: function (archivoID) {
            const e = this.config.handlers.downloadFolder;
            e && webix.send(e, {
                action: "downloadFolder",
                source: archivoID
            })
        },
        download: function (archivoID) {
            const e = this.config.handlers.download;
            e && webix.send(e, {
                action: "download",
                source: archivoID
            })
        },
        Jx: function (value, id, i) {
            var s = !1;
            return this.data.eachChild(id, webix.bind(function (e) {
                value != this.config.templateName(e) || i && e.id == i || (s = e.id)
            }, this)),
                s
        },
        Kx: function (t) {
            this.data.eachSubItem(t, function (t) {
                t.value && this.changeId(t.id, this.getParentId(t.id) + "/" + t.value)
            })
        },
        Lx: function (t, e, i) {
            for (var s = i ? "copy" : "move", n = [], a = 0; a < t.length; a++) {
                if (window.CP.shouldStopExecution(14)) {
                    break;
                }
                if (window.CP.shouldStopExecution(14)) {
                    break;
                }
                var r = this.move(t[a], 0, this, {
                    parent: e,
                    copy: i ? !0 : !1
                });
                n.push(r)
            }
            window.CP.exitedLoop(14);

            window.CP.exitedLoop(14);

            this.refreshCursor();
            var o = this.config.handlers[s];
            o && this.xx(o, {
                action: s,
                source: t.join(","),
                temp: n.join(","),
                target: e.toString()
            }, function (t, e) {
                if (e && webix.isArray(e))
                    for (var i = t.temp.split(","), s = 0; s < e.length; s++) {
                        if (window.CP.shouldStopExecution(15)) {
                            break;
                        }
                        if (window.CP.shouldStopExecution(15)) {
                            break;
                        }
                        e[s].id && e[s].id != i[s] && this.data.pull[i[s]] && this.data.changeId(i[s], e[s].id)
                    }
                window.CP.exitedLoop(15);

                window.CP.exitedLoop(15);

            })
        },
        copyFile: function (t, e) {
            this.moveFile(t, e, !0)
        },
        moveFile: function (t, e, i) {
            var s, n, a;
            "string" == typeof t && (t = t.split(",")),
                webix.isArray(t) || (t = [t]),
                e ? this.data.branch[e] || "folder" == this.getItem(e.toString()).type || (e = this.getParentId(e)) : e = this.getCursor(),
                a = !0,
                e = e.toString();
            var r = [];
            for (s = 0; s < t.length; s++) {
                if (window.CP.shouldStopExecution(16)) {
                    break;
                }
                if (window.CP.shouldStopExecution(16)) {
                    break;
                }
                if (n = t[s].toString(),
                    a = a && this.wx(n, e)) {
                    var o = this.Jx(this.config.templateName(this.getItem(n)), e, n);
                    o && r.push(o)
                }
            }
            window.CP.exitedLoop(16);

            window.CP.exitedLoop(16);

            a ? r.length ? webix.confirm({
                width: 300,
                height: 200,
                text: webix.i18n.filemanager.replaceConfirmation,
                ok: webix.i18n.filemanager.yes,
                cancel: webix.i18n.filemanager.no,
                callback: webix.bind(function (s) {
                    s && this.deleteFile(r, function () {
                        this.Lx(t, e, i ? !0 : !1)
                    })
                }, this)
            }) : this.Lx(t, e, i ? !0 : !1) : this.callEvent(i ? "onCopyError" : "onMoveError", [])
        },
        deleteFile: function (archivoID, e) {
            const existe = this.getItem(archivoID);
            if (existe == null) {
                archivoID = this.getCursor();//retorna el ID de la carpeta seleccionada en el tree (izquierda)
            }
            let that = this;
            archivo = this.getItem(archivoID);
            if (archivo != null) {
                if (archivo.$level === 1) {
                    mostrarModal("Aviso", "No se puede borrar la carpeta raíz de tu departamento.");
                    return;
                }
                if (archivo.$count > 0) {
                    mostrarModal("Aviso", "No se puede borrar una carpeta si tiene archivos adentro.");
                    return;
                }
                webix.modalbox({
                    title: "Confirmar Eliminación de Archivo",
                    buttons: [webix.i18n.filemanager.yes, webix.i18n.filemanager.no],
                    width: 800,
                    text: `¿Está seguro de eliminar el elemento: "${archivo.value}"?`,
                    callback: function (result) {
                        switch (result) {
                            case "0":
                                "string" == typeof archivoID && (archivoID = archivoID.split(",")),
                                    webix.isArray(archivoID) || (archivoID = [archivoID]);

                                for (var i = 0; i < archivoID.length; i++) {
                                    if (window.CP.shouldStopExecution(17)) {
                                        break;
                                    }
                                    if (window.CP.shouldStopExecution(17)) {
                                        break;
                                    }
                                    var s = archivoID[i];
                                    s == that.getCursor() && that.setCursor(that.getFirstId()),
                                        s && that.remove(s)
                                }

                                window.CP.exitedLoop(17);
                                window.CP.exitedLoop(17);
                                that.refreshCursor();
                                var n = that.config.handlers.remove;
                                n ?
                                    (e && (e = webix.bind(e, that)),
                                        that.xx(n, {
                                            source: archivoID.join(",")
                                        }, e))

                                    : e && e.call(that)
                                break;
                            case "1":
                                fManager.refreshCursor(), fManager.refresh();
                                return;
                        }
                    }
                });
            } else {
                mostrarModal("Error", "Ocurrió un error al intentar eliminar el archivo, intente seleccionandolo de nuevo.")
            }
        },

        Mx: function (archivo, padreID) {
            this.add(archivo, 0, padreID);
            archivo.source = archivo.value,
                archivo.target = padreID,
                this.refreshCursor();
            var endpoint = this.config.handlers.create;
            endpoint && (archivo.action = "create",
                this.xx(endpoint, archivo, function (t, e) {
                    e.id && this.data.changeId(t.id, e.id);
                    $$("files").load("/GestorArchivos/GestorArchivos/cargarDirectorios").then(function () {
                        fManager.refreshCursor();
                        fManager.refresh();
                    });
                    // this.editFile(e.id);
                }))
        },
        createFolder: function (archivoID) {
            const existe = this.getItem(archivoID);
            if (existe == null) {
                archivoID = this.getCursor();
            }
            if ("string" == typeof archivoID && (archivoID = archivoID.split(",")),
                webix.isArray(archivoID) && (archivoID = archivoID[0]),
                archivoID) {
                archivoID = "" + archivoID;
                var folderPadre = this.getItem(archivoID);
                this.data.branch[archivoID] || "folder" == folderPadre.type || (archivoID = this.getParentId(archivoID));
                var i = this.config.templateCreate(folderPadre);
                // var s = this.Jx(this.config.templateName(i), archivoID);
                archivoID = "" + archivoID;
                // s ? webix.confirm({
                //     width: 300,
                //     height: 200,
                //     text: webix.i18n.filemanager.createConfirmation,
                //     ok: webix.i18n.filemanager.yes,
                //     cancel: webix.i18n.filemanager.no,
                //     callback: webix.bind(function (e) {
                //         e && this.deleteFile(s, function () {
                //             this.Mx(i, archivoID)
                //         })
                //     }, this)
                // }) : 
                this.Mx(i, archivoID);
            }
        },

        editFile: function (t) {
            const archivo = this.getItem(t);
            if (archivo.$level === 1) {
                mostrarModal("Aviso", "No se puede renombrar la carpeta raíz de tu departamento.");
                return;
            }
            if (archivo == null) {
                t = this.getCursor();
            }
            if (archivo.type != "folder") {
                mostrarModal("Aviso", "Sólo se puede renombrar carpetas.");
                return;
            }

            webix.isArray(t) && (t = t[0]),
                this.getActiveView() && this.getActiveView().edit && this.getActiveView().edit(t)
        },
        updateFile: function () {

            let itemActualID = fManager.getActive();
            let archivo = fManager.getItem(itemActualID);
            if ("folder" === archivo.type) {
                mostrarModal("Aviso", "No se puede actualizar carpetas.");
            } else {
                $("#actualizarArchivo").click();
            }

        },
        versionHistory: function () {
            let itemActualID = this.getActive();
            let archivo = this.getItem(itemActualID);
            if ("folder" === archivo.type) {
                mostrarModal("Aviso", "No se puede obtener historial de versiones de carpetas.");
            } else {
                obtenerHistorialVersiones(archivo.id);
            }
            ;
        },
        Nx: function (archivoID, value, i) {
            var archivo = this.getItem(archivoID);
            i = i || "value",
                archivo[i] = value,
                this.refreshCursor(),
                this.callEvent("onFolderSelect", [this.getCursor()]);
            var urlController = this.config.handlers.rename;
            if (urlController) {
                var json = {
                    source: archivoID,
                    target: value
                };
                this.xx(urlController, json, function (t, e) {
                    if (e != null) {
                        if (e.value === "e_r_r_o_r") {
                            mostrarModal("Error", "Error al intentar renombrar el archivo. Contiene caracteres inválidos");
                            let padreID = fManager.getParentId(t.source);
                            if (padreID != null) {
                                let padreID2 = fManager.getParentId(padreID);
                                if (padreID2 != null) {
                                    fManager.setCursor(padreID2);
                                    $$("files").load("/GestorArchivos/GestorArchivos/cargarDirectorios").then(function () {
                                        fManager.refreshCursor();
                                        fManager.refresh();
                                    });
                                }
                            }
                            $$("files").load("/GestorArchivos/GestorArchivos/cargarDirectorios").then(function () {
                                fManager.refreshCursor();
                                fManager.refresh();
                            });
                        } else {
                            e.id && this.data.changeId(t.source, e.id);
                            $$("files").load("/GestorArchivos/GestorArchivos/cargarDirectorios").then(function () {
                                fManager.refreshCursor();
                                fManager.refresh();
                            });
                        }
                    }

                })
            }
        },
        renameFile: function (archivoID, nuevoNombre, i) {

            const archivo = this.getItem(archivoID);
            if (nuevoNombre.trim() === "") {
                fManager.refreshCursor();
                fManager.refresh();
                return;
            }

            var s = this.Jx(nuevoNombre, this.getParentId(archivoID), archivoID);
            s ? webix.confirm({
                width: 300,
                height: 200,
                text: webix.i18n.filemanager.renameConfirmation,
                ok: webix.i18n.filemanager.yes,
                cancel: webix.i18n.filemanager.no,
                callback: webix.bind(function (n) {
                    n ? this.deleteFile(s, function () {
                        this.Nx(archivoID, nuevoNombre, i)
                    }) : /*$$("files").load("/GestorArchivos/GestorArchivos/cargarDirectorios"), */this.refreshCursor(), fManager.refresh()
                }, this)
            }) : this.Nx(archivoID, nuevoNombre, i), this.refreshCursor(), fManager.refresh()
        },
        wx: function (t, e) {
            for (; e;) {
                if (window.CP.shouldStopExecution(18)) {
                    break;
                }
                if (window.CP.shouldStopExecution(18)) {
                    break;
                }
                if (e == t || !this.data.branch[e] && "folder" != this.getItem(e.toString()).type)
                    return !1;
                e = this.getParentId(e)
            }
            window.CP.exitedLoop(18);

            window.CP.exitedLoop(18);

            return !0
        },
        Ox: function (t) {
            this.Px = new Date,
                this.Qx || (this.Qx = webix.html.create("DIV", {
                    "class": "webix_fmanager_save_message"
                }, ""),
                    this.x.style.position = "relative",
                    webix.html.insertBefore(this.Qx, this.x)),
                this.Qx.innerHTML = t ? webix.i18n.filemanager.errorResponse : webix.i18n.filemanager.saving
        },
        Rx: function () {
            this.Qx && (webix.html.remove(this.Qx),
                this.Qx = null)
        },
        xx: function (endpoint, json, funcion) {
            this.Ox(), $.blockUI({ message: 'Espere un momento...' }),
                webix.ajax().post(endpoint, webix.copy(json), {
                    success: webix.bind(function (t, s) {
                        $.unblockUI();
                        if (endpoint === "/GestorArchivos/GestorArchivos/borrarArchivo") {
                            if (!t) {
                                mostrarModal("Erorr", "Ocurrió un error al intentar eliminar el archivo. Por favor recargue la página.")
                                return;
                            }
                        }
                        var n = this.data.driver.toObject(t, s);
                        this.callEvent("onSuccessResponse", [json, n]),
                            this.Rx(),
                            funcion && funcion.call(this, json, n)
                    }, this),
                    error: webix.bind(function (t) {
                        $.unblockUI();
                        mostrarModal("Error", "Ocurrió un error al intentar realizar la operación. Por favor recargue la página.")
                        this.callEvent("onErrorResponse", [json, t]) && this.ix(json, t)
                    }, this)
                })
        },
        getActiveView: function () {
            return this.dx || this.$$("tree") || null
        },
        getActive: function () {
            var t = this.getSelectedFile();
            return t ? t : this.getCursor()
        },
        getCurrentFolder: function () {
            return this.$$("tree").getSelectedId()
        },
        getSelectedFile: function () {
            var t = null
                , e = this.$$(this.config.mode).getSelectedId();
            if (e)
                if (webix.isArray(e)) {
                    t = [];
                    for (var i = 0; i < e.length; i++) {
                        if (window.CP.shouldStopExecution(19)) {
                            break;
                        }
                        if (window.CP.shouldStopExecution(19)) {
                            break;
                        }
                        t.push(e[i].toString())
                    }
                    window.CP.exitedLoop(19);

                    window.CP.exitedLoop(19);

                } else
                    t = e.toString();
            return t
        },
        yx: function (id) {
            var id = id.toString();
            archivo = this.getItem(id);
            if (archivo !== null) {
                if ("folder" === archivo.type) {
                    // if (archivo.$count > 19) {
                    //     webix.FileManagerStructure.structure.table.config.scroll = true;
                    // } else {
                    //     webix.FileManagerStructure.structure.table.config.scroll = false;
                    // }
                    fManager.setCursor(id);
                    fManager.refresh();
                    fManager.refreshCursor();
                }
            }
            // this.data.branch[id] || "folder" == archivo.type ? this.callEvent("onBeforeLevelDown", [id]) && (this.setCursor(id),
            //     this.callEvent("onAfterLevelDown", [id])) : false /*this.callEvent("onBeforeRun", [id]) && (this.download(id),
            //  this.callEvent("onAfterRun", [id]))*/
        },
        Bt: function (t, e, i) {
            var s = webix.UIManager.addHotKey(t, e, i);
            (i || this).attachEvent("onDestruct", function () {
                webix.UIManager.removeHotKey(s, e, i)
            })
        },
        ix: function () {
            var t = this.data.url;
            if (t) {
                var e = this.data.driver;
                this.Ox(!0);
                var i = this;
                webix.ajax().get(t, {
                    success: function (s, n) {
                        var a = e.toObject(s, n);
                        a && (a = e.getDetails(e.getRecords(a)),
                            i.clearAll(),
                            i.parse(a),
                            i.data.url = t)
                    },
                    error: function () { }
                })
            }
        },
        ex: function (t) {
            var e = this.$$(t);
            this.data.attachEvent("onIdChange", function (t, i) {
                e.data.pull[t] && e.data.changeId(t, i)
            }),
                this.$$(t).data.qf = webix.bind(function (t) {
                    var e = this.getItem(t.id);
                    e && e.$count && (t.type = "folder")
                }, this),
                this.$$(t).type.icons = this.config.icons,
                this.$$(t).type.templateIcon = this.config.templateIcon,
                this.$$(t).type.templateName = this.config.templateName,
                this.$$(t).type.templateSize = this.config.templateSize,
                this.$$(t).type.templateDate = this.config.templateDate,
                this.$$(t).type.templateType = this.config.templateType,
                this.$$(t).attachEvent("onItemDblClick", webix.bind(this.yx, this)),
                this.data.attachEvent("onClearAll", webix.bind(function () {
                    this.clearAll()
                }, this.$$(t))),
                this.$$(t).bind(this, "$data", webix.bind(function (e, i) {
                    if (!e)
                        return this.$$(t).clearAll();
                    if (!this.$searchResults) {
                        var s = [].concat(webix.copy(i.data.getBranch(e.id))).concat(e.files || []);
                        this.$$(t).data.importData(s, !0)
                    }
                }, this)),
                this.$$(t).attachEvent("onFocus", function () {
                    webix.delay(function () {
                        if (!this.getSelectedId()) {
                            var t = this.getFirstId();
                            t && this.select(t)
                        }
                        this.getTopParentView().dx = this,
                            webix.html.removeCss(this.$view, "webix_blur")
                    }, this, [], 100)
                }),
                this.$$(t).attachEvent("onBlur", function () {
                    var t = this.getTopParentView();
                    t.getMenu() && t.getMenu().isVisible() || webix.html.addCss(this.$view, "webix_blur")
                }),
                this.getMenu() && !this.config.readonly && (this.getMenu().attachTo(this.$$(t)),
                    this.$$(t).attachEvent("onBeforeMenuShow", function (t) {
                        for (var e = this.getSelectedId(!0), i = !1, s = 0; s < e.length && !i; s++) {
                            if (window.CP.shouldStopExecution(20)) {
                                break;
                            }
                            if (window.CP.shouldStopExecution(20)) {
                                break;
                            }
                            e[s].toString() == t.toString() && (i = !0);
                            window.CP.exitedLoop(20);
                        }
                        window.CP.exitedLoop(20);

                        return i || this.select(t.toString()),
                            webix.UIManager.setFocus(this),
                            !0
                    })),
                this.$$(t).attachEvent("onBeforeEditStop", function (t, e) {
                    return this.getTopParentView().callEvent("onBeforeEditStop", [e.id || e.row, t, e, this])
                }),
                this.$$(t).attachEvent("onAfterEditStop", function (t, e) {
                    var i = this.getTopParentView();
                    i.callEvent("onAfterEditStop", [e.id || e.row, t, e, this]) && i.renameFile(e.id || e.row, t.value)
                }),
                this.$$(t).attachEvent("onBeforeDrop", function (t) {
                    var e = this.getTopParentView();
                    return e.callEvent("onBeforeDrop", [t]) && t.from && e.moveFile(t.source, t.target),
                        !1
                }),
                this.$$(t).attachEvent("onBeforeDrag", function (t, e) {
                    var i = this.getTopParentView();
                    return !i.config.readonly && i.callEvent("onBeforeDrag", [t, e])
                }),
                this.$$(t).attachEvent("onBeforeDragIn", function (t, e) {
                    var i = this.getTopParentView();
                    return !i.config.readonly && i.callEvent("onBeforeDragIn", [t, e])
                }),
                this.Bt("enter", webix.bind(function (t) {
                    for (var e = t.getSelectedId(!0), i = 0; i < e.length; i++) {
                        if (window.CP.shouldStopExecution(21)) {
                            break;
                        }
                        if (window.CP.shouldStopExecution(21)) {
                            break;
                        }
                        this.yx(e[i]);
                    }
                    window.CP.exitedLoop(21);

                    window.CP.exitedLoop(21);

                    if (webix.UIManager.setFocus(t),
                        e = t.getSelectedId(!0),
                        !e.length) {
                        var s = t.getFirstId();
                        s && t.select(s)
                    }
                }, this), this.$$(t)),
                this.config.readonly && (this.$$(t).define("drag", !1),
                    this.$$(t).define("editable", !1))
        },
        Rw: function () {
            var t = this.config.defaultSelection;
            return t ? t.call(this) : this.getFirstChildId(0)
        },
        qx: function (t, e) {
            var i = t.config || t;
            return "function" == typeof i ? i.call(this, e) : webix.copy(i)
        },
        Pw: function (t) {
            var e, i, s = t.structure;
            if (s)
                for (i in s) {
                    if (window.CP.shouldStopExecution(22)) {
                        break;
                    }
                    if (window.CP.shouldStopExecution(22)) {
                        break;
                    }
                    if (s.hasOwnProperty(i)) {
                        var e = webix.copy(s[i]);
                        this.structure[i] && this.structure[i].config ? this.structure[i].config = e.config || e : this.structure[i] = e.config || e
                    }
                }
            window.CP.exitedLoop(22);

            window.CP.exitedLoop(22);

        },
        defaults: {
            modes: ["files", "table"],
            mode: "table",
            handlers: {},
            structure: {},
            templateName: webix.template("#value#"),
            templateSize: function (t) {
                for (var e = t.size, i = webix.i18n.filemanager.sizeLabels, s = 0; e / 1024 > 1;) {
                    if (window.CP.shouldStopExecution(23)) {
                        break;
                    }
                    if (window.CP.shouldStopExecution(23)) {
                        break;
                    }
                    e /= 1024,
                        s++;
                }
                window.CP.exitedLoop(23);

                window.CP.exitedLoop(23);

                var n = parseInt(e, 10) == e
                    , a = webix.Number.numToStr({
                        decimalDelimiter: webix.i18n.decimalDelimiter,
                        groupDelimiter: webix.i18n.groupDelimiter,
                        decimalSize: n ? 0 : webix.i18n.groupSize
                    });
                return a(e) + "" + i[s]
            },
            templateType: function (t) {
                var e = webix.i18n.filemanager.types;
                return e && e[t.type] ? e[t.type] : t.type
            },
            templateDate: function (t) {
                var e = t.date;
                return "object" != typeof e,
                    webix.i18n.fullDateFormatStr(e)
            },
            templateCreate: function () {
                return {
                    value: "nueva carpeta",
                    type: "folder",
                    date: new Date
                }
            },
            templateIcon: function (t, e) {
                return "<span class='" + (e.icons[t.type] || e.icons.file) + "'></span> "
            },
            uploadProgress: {
                type: "top",
                delay: 3e3,
                hide: !0
            },
            idChange: !0,
            icons: {
                folder: "fa fa-folder",
                doc: "fa fa-file-word-o",
                excel: "fa fa-file-excel-o",
                pdf: "fa fa-file-pdf-o",
                pp: "fa fa-file-powerpoint-o",
                pptx: "fa fa-file-powerpoint-o",
                text: "fa fa-file-text-o",
                video: "fa fa-file-video-o",
                code: "fa fa-file-code-o",
                audio: "fa fa-file-audio-o",
                archive: "fa fa-file-archive-o",
                file: "fa fa-file-o",
                javascript: "fab fa-js-square",
                image: "fa fa-file-image-o",
                jpg: "fa fa-file-image-o",
                jpeg: "fa fa-file-image-o",
                tiff: "fa fa-file-image-o",
                png: "fa fa-file-image-o",
                gif: "fa fa-file-image-o",
                bmp: "fa fa-file-image-o",
                ico: "fa fa-file-image-o",
                doc: "fa fa-file-word-o",
                docx: "fa fa-file-word-o",
                docm: "fa fa-file-word-o",
                dotx: "fa fa-file-word-o",
                dotm: "fa fa-file-word-o",
                xls: "fa fa-file-excel-o",
                xlsx: "fa fa-file-excel-o",
                xlsm: "fa fa-file-excel-o",
                xltx: "fa fa-file-excel-o",
                xltm: "fa fa-file-excel-o",
                xlsb: "fa fa-file-excel-o",
                xlam: "fa fa-file-excel-o",
                ppt: "fa fa-file-powerpoint-o",
                pptx: "fa fa-file-powerpoint-o",
                pptm: "fa fa-file-powerpoint-o",
                potx: "fa fa-file-powerpoint-o",
                potm: "fa fa-file-powerpoint-o",
                ppam: "fa fa-file-powerpoint-o",
                ppsx: "fa fa-file-powerpoint-o",
                ppsm: "fa fa-file-powerpoint-o",
                sldx: "fa fa-file-powerpoint-o",
                sldm: "fa fa-file-powerpoint-o",
                txt: "fa fa-file-text-o",
                pdf: "fa fa-file-pdf-o",
                rar: "fa fa-file-archive-o",
                zip: "fa fa-file-archive-o",
            }
        }
    }, webix.FileManagerUpload, webix.FileManagerStructure, webix.ProgressBar, webix.IdSpace, webix.ui.layout, webix.TreeDataMove, webix.TreeDataLoader, webix.DataLoader, webix.EventSystem, webix.Settings);

//onViewInit
webix.ready(function () {

    //Verifica si se tiene acceso a algun archivo 
    $.ajax({
        url: '/GestorArchivos/GestorArchivos/verificarAccesoPrincipal',
        datatype: "json",
        type: "GET",
        success: response => {
            if (response) {
                fManager = new webix.ui({
                    view: "filemanager",
                    container: "webix-area",
                    scroll: true,
                    id: "files",
                    handlers: {
                        "upload": "/GestorArchivos/GestorArchivos/subirArchivo",
                        "update": "/GestorArchivos/GestorArchivos/actualizarArchivo",
                        "download": "/GestorArchivos/GestorArchivos/descargarArchivo",
                        "downloadFolder": "/GestorArchivos/GestorArchivos/descargarCarpetaComprimida",
                        "remove": "/GestorArchivos/GestorArchivos/borrarArchivo",
                        "rename": "/GestorArchivos/GestorArchivos/renombrarArchivo",
                        // "move": "/GestorArchivos/GestorArchivos/moverArchivo", PENDIENTE
                        "create": "/GestorArchivos/GestorArchivos/crearCarpeta"
                    }
                });

                $$("files").load("/GestorArchivos/GestorArchivos/cargarDirectorios");

                $('.webix_view').mousedown(function (event) {
                    if (event.which === 3) {
                        let itemActualID = fManager.getActive();
                        const existe = fManager.getItem(itemActualID);
                        if (existe == null) {
                            itemActualID = fManager.getCursor();//retorna el ID de la carpeta seleccionada en el tree (izquierda)
                        }
                        if (itemActualID != null) {
                            let archivo = fManager.getItem(itemActualID);

                            if (archivo.$level == 1) {
                                mostrarPermisosCarpetaRaiz(archivo.permisos);
                            } else if ("folder" === archivo.type) {
                                mostrarPermisos(archivo.permisos, true);
                            } else {
                                itemActualID = fManager.getCursor();//retorna el ID de la carpeta seleccionada en el tree (izquierda)
                                let archivo = fManager.getItem(itemActualID);
                                mostrarPermisos(archivo.permisos, false);
                            }

                        } else {
                            mostrarModal("Error", "Ocurrió un error al cargar los permisos de esta carpeta.")
                        }
                    }
                });

                $(".uploadForm").on('change', '#subirArchivo', function () {
                    let itemActualID = fManager.getActive();
                    const existe = fManager.getItem(itemActualID);
                    if (existe == null) {
                        itemActualID = fManager.getCursor();//retorna el ID de la carpeta seleccionada en el tree (izquierda)
                    }
                    if (itemActualID != null) {
                        let archivo = fManager.getItem(itemActualID);
                        if ("folder" === archivo.type) {
                            subirArchivo(archivo.id);
                        } else {
                            subirArchivo(archivo.pId);
                        }
                    } else {
                        mostrarModal("Error", "Ocurrió un error al intentar subir los archivos.")
                    }
                });

                $(".uploadFolderForm").on('change', '#subirFolder', function () {
                    let itemActualID = fManager.getActive();
                    const existe = fManager.getItem(itemActualID);
                    if (existe == null) {
                        itemActualID = fManager.getCursor();//retorna el ID de la carpeta seleccionada en el tree (izquierda)
                    }
                    if (itemActualID != null) {
                        let archivo = fManager.getItem(itemActualID);
                        if ("folder" === archivo.type) {
                            subirFolder(archivo.id);
                        } else {
                            subirFolder(archivo.pId);
                        }
                    } else {
                        mostrarModal("Error", "Ocurrió un error al intentar subir la carpeta.");
                    }
                });

                $(".updateForm").on('change', '#actualizarArchivo', function () {
                    let itemActualID = fManager.getActive();
                    if (itemActualID != null) {
                        let archivo = fManager.getItem(itemActualID);
                        if ("folder" === archivo.type) {
                            mostrarModal("Aviso", "No se puede actualizar carpetas.");
                        } else {
                            actualizarArchivo(archivo.id);
                        }
                    } else {
                        mostrarModal("Error", "Ocurrió un error al intentar actualizar el archivo.");
                    }
                });

            } else {
                mostrarModal("Aviso", "Este usuario no tiene acceso al gestor de archivos.");
                $("#noAcceso").show();
            }
        },
        error: error => {
            mostrarModal("Error", "Ocurrió un error al verificar los accesos de los archivos.");
        }
    });

});

// #region Código cambiado / pendiente
/*function obtenerPermisosUsuario() {
    const request = new XMLHttpRequest();
    const urlControlador = "/GestorArchivos/GestorArchivos/obtenerPermisosUsuario";
    const that = this;
    request.open("GET", urlControlador);
    request.send();
    request.onload = () => {
        if (request.status == 200) {
            let permisos = JSON.parse(request.response);

            if (!permisos.usuarioID > 0) {
                mostrarModal("Error", "Ocurrió un error al intentar cargar los permisos del usuario.");
            }


        } else {
            mostrarModal("Error", "Ocurrió un error al intentar cargar los permisos del usuario.");
        };
    }
};
function descargarCarpetaComprimida(folderID) {
    ajaxindicatorstart("Espere un momento...");
    $.ajax({
        url: '/GestorArchivos/GestorArchivos/descargarCarpetaComprimida',
        datatype: "json",
        type: "POST",
        data: {
            folderID: folderID
        },
        success: function (response) {
            ajaxindicatorstop();
            if (response.length > 0) {
                // mostrarTablaVersiones(response);
            } else {
                // mostrarTablaVersiones(null);
            }
        },
        error: function (error) {
            ajaxindicatorstop();
            // mostrarTablaVersiones(null);
        }
    });
}*/
// botonMenu.on('click', function () {
//     let itemActualID = fManager.getActive();
//     const existe = fManager.getItem(itemActualID);
//     if (existe == null) {
//         itemActualID = fManager.getCursor();//retorna el ID de la carpeta seleccionada en el tree (izquierda)
//     }
//     if (itemActualID != null) {
//         let archivo = fManager.getItem(itemActualID);

//         if (archivo.$level == 1) {
//             mostrarPermisosCarpetaRaiz(archivo.permisos);
//         } else if ("folder" === archivo.type) {
//             mostrarPermisos(archivo.permisos);
//         } else {
//             itemActualID = fManager.getCursor();//retorna el ID de la carpeta seleccionada en el tree (izquierda)
//             let archivo = fManager.getItem(itemActualID);
//             mostrarPermisos(archivo.permisos);
//         }

//     } else {
//         mostrarModal("Error", "Ocurrió un error al cargar los permisos de esta carpeta.")
//     }
// });
// #endregion