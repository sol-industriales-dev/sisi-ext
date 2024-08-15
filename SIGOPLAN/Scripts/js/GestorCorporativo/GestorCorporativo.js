(() => {
    $.namespace('GestorCorporativo.GestorCorporativo');
    GestorCorporativo = function () {

        // Declaración de variables
        let fileManagerWebix;

        const extensionesNoAceptadas = ["exe", "app", "vb", "scr", "vbe", "vbs"];

        const MENU_BASE = [
            { // 0
                id: "create",
                method: "createFolder",
                icon: " fas fa-folder-plus",
                value: "Crear carpeta"
            },
            { // 1
                id: "edit",
                method: "editFile",
                icon: "edit",
                value: "Renombrar carpeta"
            },
            { // 2
                id: "upload",
                method: "uploadFile",
                icon: " fas fa-file-upload",
                value: "Subir"
            },
            { // 3
                id: "downloadFolder",
                method: "downloadFolder",
                icon: " fas fa-file-archive",
                value: "Descargar carpeta"
            },
            { // 4
                id: "downloadFile",
                method: "download",
                icon: " fas fa-file-download",
                value: "Descargar"
            },
            { // 5
                id: "remove",
                method: "deleteFile",
                icon: "trash",
                value: "Eliminar"
            },
            { // 6
                id: "createSession",
                method: "createSession",
                icon: " fas fa-calendar-day",
                value: "Nueva Sesión"
            }
        ];

        // Modal subir archivo
        const modalCargaArchivo = $('#modalCargaArchivo');
        const tituloModalCargaArchivo = $('#tituloModalCargaArchivo');
        const inputArchivo = $('#inputArchivo');
        const botonSubirArchivo = $('#botonSubirArchivo');

        // Modal aviso
        const modalAviso = $("#modalAviso");
        const modalAvisoTitutlo = $('#modalAvisoTitutlo');
        const modalAvisoCuerpo = $('#modalAvisoCuerpo');
        const botonModalAviso = $('#botonModalAviso');

        // Modal crear carpeta
        const modalCrearCarpeta = $('#modalCrearCarpeta');
        const inputNombreCarpeta = $('#inputNombreCarpeta');
        const botonCrearCarpeta = $('#botonCrearCarpeta');
        const textoPrevioCarpeta = $('#textoPrevioCarpeta');

        function init() {
            initWebixTree();
            agregarListeners();
        }

        //////////////////   Webix FileManager   ///////////////////////////
        function initWebixTree() {

            webix.type(webix.ui.tree, {
                name: "FileTree",
                css: "webix_fmanager_tree",
                folder: function (t) {
                    return t.$count && t.open ? "<div class='webix_icon icon fa-folder-open'></div>" : "<div class='webix_icon icon fa-folder'></div>"
                }
            });

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
            });

            //Diccionario de valor del texto
            webix.i18n.filemanager = {
                name: "Nombre",
                size: "Tamaño",
                type: "Tipo de Archivo",
                date: "Fecha Modificación",
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
                    dwg: "Dibujo Computarizado",
                }
            };

            webix.protoUI({ name: "filetree" }, webix.EditAbility, webix.ui.tree);
            webix.protoUI({ name: "fileview" }, webix.EditAbility, webix.ui.dataview)
            webix.protoUI({
                name: "filetable",
                $dragHTML: function (t) {
                    var e = "<div class='webix_dd_drag webix_fmanager_drag' >"
                        , i = this.getColumnIndex("value");
                    return e += "<div style='width:auto'>" + this.config.columns[i].template(t, this.type) + "</div>",
                        e + "</div>"
                }
            }, webix.ui.datatable);

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
            }, webix.ui.list);

            // Estructura
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
                        }, "up", "toggleCollapse", "path", "search"/*, "modes"*/],
                    menu: {
                        config: {
                            view: "button",
                            type: "iconButton",
                            css: "webix_fmanager_back",
                            icon: "bars",
                            width: 1,
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
                    toggleCollapse: {
                        config: {
                            view: "button",
                            type: "iconButton",
                            css: "webix_fmanager_up",
                            icon: " fas fa-angle-double-up",
                            disable: !0,
                            width: 37
                        },
                        oninit: function () {
                            this.$$("toggleCollapse") && this.$$("toggleCollapse").attachEvent("onItemClick", webix.bind(function () {
                                const span = $('div[view_id="$button4"]').find('span');
                                span.toggleClass('fa-angle-double-up').toggleClass('fa-angle-double-down');
                                span.hasClass('fa-angle-double-down') ? fileManagerWebix.$$("tree").closeAll() : fileManagerWebix.$$("tree").openAll();
                            }, this));
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
                                            t = this.getParentId(t),
                                                i--;
                                        }

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
                            width: 500,
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
                                    e.push(t.modes[i]);
                                }




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

                                    this.$$(t[e]) && this.$$(t[e]).filter && this.ex(t[e])
                                }




                        }
                    },
                    modes: {
                        config: function (t) {
                            var e = 0
                                , i = this.structure.modeOptions;
                            if (i)
                                for (var s = 0; s < i.length; s++) {
                                    i[s].width && (e += i[s].width + (i.length ? 1 : 0));
                                }




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
                                fillspace: 1.3,
                                template: function (t, e) {
                                    var i = e.templateName(t, e);
                                    return e.templateIcon(t, e) + i
                                },
                                sort: "string",
                                editor: "text"
                            }, {
                                id: "date",
                                header: t.date,
                                fillspace: 0.8,
                                template: function (t, e) {
                                    return e.templateDate(t, e)
                                },
                                sort: "date"
                            }, {
                                id: "type",
                                header: t.type,
                                fillspace: 0.7,
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
                                        s.push(i[n].value);
                                    }
                                    return s.join("/")
                                },
                                sort: "string"
                            },
                            ]
                        }
                    }
                }
            };

            // FileManager Upload
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
                gx: function () {
                    return this.hx || this.getCursor()
                },
                uploadFile: archivoID => {

                    const archivo = fileManagerWebix.getItem(archivoID);

                    if (archivo == null) {
                        return;
                    }
                    mostrarModalSubirArchivo();
                }
            };

            // FileManager Logic
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
                                    for (; e;) {
                                        if (e == i[s])
                                            return !1;
                                        e = this.getParentId(e)
                                    }

                                }

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
                        t[o[n]] && (r = o[n],
                            i = t[r]);
                    }

                    if (i)
                        for ("string" == typeof i && this.structure[i] && (t[r] = this.qx(this.structure[i], e),
                            i = t[r]),
                            n = 0; n < i.length; n++) {
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
                        e = this.getItem(t),
                            i.push(t),
                            t = this.getParentId(t);
                    }

                    return i.reverse()
                },
                getPathNames: function (archivoID) {
                    archivoID = archivoID || this.getCursor();
                    for (var e = null, i = []; archivoID && this.getItem(archivoID);) {
                        e = this.getItem(archivoID),
                            i.push({
                                id: archivoID,
                                value: this.config.templateName(e)
                            }),
                            archivoID = this.getParentId(archivoID);
                    }

                    return i.reverse()
                },
                setPath: function (t) {
                    for (var e = t; e && this.getItem(e);) {
                        this.callEvent("onPathLevel", [e]),
                            e = this.getParentId(e);
                    }

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
                downloadFolder: function (archivoID) {
                    const e = this.config.handlers.downloadFolder;
                    e && webix.send(e, {
                        action: "downloadFolder",
                        source: archivoID
                    });
                },
                download: function (archivoID) {
                    const e = this.config.handlers.download;
                    e && webix.send(e, {
                        action: "download",
                        source: archivoID
                    });
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
                        var r = this.move(t[a], 0, this, {
                            parent: e,
                            copy: i ? !0 : !1
                        });
                        n.push(r)
                    }

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
                                e[s].id && e[s].id != i[s] && this.data.pull[i[s]] && this.data.changeId(i[s], e[s].id)
                            }

                    })
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
                        if (n = t[s].toString(),
                            a = a && this.wx(n, e)) {
                            var o = this.Jx(this.config.templateName(this.getItem(n)), e, n);
                            o && r.push(o)
                        }
                    }

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
                deleteFile: function (archivoID, event) {

                    const existe = this.getItem(archivoID);
                    if (existe == null) {
                        archivoID = this.getCursor();//retorna el ID de la carpeta seleccionada en el tree (izquierda)
                    }
                    let that = this;
                    archivo = this.getItem(archivoID);
                    if (archivo != null) {
                        if (archivo.$level === 1) {
                            mostrarModal(2, "No se puede borrar la carpeta raíz.");
                            return;
                        }
                        if (archivo.$count > 0) {
                            mostrarModal(2, "No se puede borrar una carpeta si tiene archivos.");
                            return;
                        }
                        webix.modalbox({
                            title: "Confirmar eliminación",
                            buttons: [webix.i18n.filemanager.yes, webix.i18n.filemanager.no],
                            width: 800,
                            text: `¿Está seguro de eliminar el elemento: "${archivo.value}"?`,
                            callback: function (result) {
                                switch (result) {
                                    case "0":
                                        var urlRemoveFile = that.config.handlers.remove;
                                        (event && (event = webix.bind(event, that)),
                                            that.xx(urlRemoveFile, {
                                                source: archivoID
                                            }, event));
                                        break;
                                    case "1":
                                        fileManagerWebix.refreshCursor();
                                        fileManagerWebix.refresh();
                                        return;
                                }
                            }
                        });
                    } else {
                        mostrarModal(2, "No se pudo encontrar al archivo que intenta eliminar.")
                    }
                },

                createFolder: function (archivoID) {

                    const archivo = fileManagerWebix.getItem(archivoID);

                    if (archivo == null) {
                        return;
                    }

                    modalCrearCarpeta.modal({
                        backdrop: 'static',
                        keyboard: false
                    });

                    botonCrearCarpeta.data().archivoID = archivo.type === 'folder' ? archivoID : archivo.pId;
                },

                editFile: function (archivoID) {
                    const archivo = this.getItem(archivoID);
                    if (archivo.$level === 1) {
                        mostrarModal(1, "No se puede renombrar la carpeta raíz.");
                        return;
                    }
                    if (archivo == null) {
                        archivoID = this.getCursor();
                    }
                    if (archivo.type != "folder") {
                        mostrarModal(1, "Sólo se puede renombrar carpetas.");
                        return;
                    }

                    // Activa el input text para editar el nombre del archivo.
                    this.getActiveView().edit(archivoID);
                },
                createSession: archivoID => {

                    let archivo = fileManagerWebix.getItem(archivoID);

                    if (archivo == null) {
                        return;
                    }

                    if (archivo.type !== 'folder') {
                        archivoID = archivo.pId;
                        archivo = fileManagerWebix.getItem(archivoID);
                    }

                    const grupoCarpeta = archivo.grupoCarpeta;

                    $.post('GestorCorporativo/CrearCarpetaSesion', { grupoCarpeta })
                        .then(response => {
                            if (response.success) {
                                $$("files").load("GestorCorporativo/ObtenerEstructuraDirectorios")
                                    .then(() => {
                                        fileManagerWebix.refreshCursor();
                                        fileManagerWebix.refresh();
                                        mostrarModal(1, 'Sesión creada correctamente.');
                                    });
                            } else {
                                mostrarModal(2, `Error al crear la sesión: ${response.message}`);
                            }
                        }, () => mostrarModal(2, 'Ocurrió un error al crear la sesión.'));
                },

                // Renombrar archivo
                Nx: function (archivoID, nuevoNombre) {
                    const urlRenameFile = this.config.handlers.rename;
                    const archivo = {
                        source: archivoID,
                        target: nuevoNombre
                    };
                    this.xx(urlRenameFile, archivo, function (t, response) {
                        if (response && response.success) {
                            const archivo = fileManagerWebix.getItem(response.archivoID)
                            archivo.value = nuevoNombre.trim();
                            fileManagerWebix.refreshCursor();
                            fileManagerWebix.refresh();
                            mostrarModal(1, "Carpeta renombrada con éxito.");
                        } else {
                            mostrarModal(2, `No se pudo renombrar la carpeta: ${response.message}`);
                            fileManagerWebix.refreshCursor();
                            fileManagerWebix.refresh();
                        }
                    });

                },
                renameFile: function (archivoID, nuevoNombre, i) {
                    const esNombreRepetido = this.Jx(nuevoNombre, this.getParentId(archivoID), archivoID);
                    if (esNombreRepetido) {
                        this.refreshCursor();
                        this.refresh();
                        mostrarModal(2, "Ya existe una carpeta con ese nombre, intente con otro.");
                    } else {
                        this.Nx(archivoID, nuevoNombre, i);
                    }
                },
                wx: function (t, e) {
                    for (; e;) {
                        if (e == t || !this.data.branch[e] && "folder" != this.getItem(e.toString()).type)
                            return !1;
                        e = this.getParentId(e)
                    }

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
                xx: function (endpoint, json, callback) {
                    this.Ox(),
                        $.blockUI({ message: 'Procesando...' }),
                        webix.ajax().post(endpoint, webix.copy(json), {
                            success: webix.bind(function (t, s) {

                                $.unblockUI();
                                // Lógica al eliminar archivo
                                if (endpoint === "GestorCorporativo/EliminarArchivo") {
                                    const response = JSON.parse(t);
                                    if (response.success) {
                                        fileManagerWebix.remove(json.source);
                                        fileManagerWebix.refreshCursor();
                                        fileManagerWebix.refresh();
                                        mostrarModal(1, "Archivo eliminado correctamente.");
                                        return;
                                    } else {
                                        mostrarModal(2, `Error al eliminar el archivo: ${response.message}`);
                                    }
                                }

                                var n = this.data.driver.toObject(t, s);
                                this.callEvent("onSuccessResponse", [json, n]),
                                    this.Rx(),
                                    callback && callback.call(this, json, n)
                            }, this),
                            error: webix.bind(function (t) {
                                $.unblockUI();
                                mostrarModal(2, "Ocurrió un error al intentar realizar la operación. Por favor recargue la página.")
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
                                t.push(e[i].toString())
                            }

                        } else
                            t = e.toString();
                    return t
                },
                yx: function (id) {
                    var id = id.toString();
                    archivo = this.getItem(id);
                    if (archivo !== null) {
                        if ("folder" === archivo.type) {
                            fileManagerWebix.setCursor(id);
                            fileManagerWebix.refreshCursor();
                            fileManagerWebix.refresh();
                        }
                    }
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
                        $.blockUI({ message: 'Procesando...' });
                        webix.ajax().get(t, {
                            success: function (s, n) {
                                $.unblockUI();
                                var a = e.toObject(s, n);
                                a && (a = e.getDetails(e.getRecords(a)),
                                    i.clearAll(),
                                    i.parse(a),
                                    i.data.url = t)
                            },
                            error: function () { $.unblockUI(); }
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
                                    e[s].toString() == t.toString() && (i = !0);
                                }

                                return i || this.select(t.toString()),
                                    webix.UIManager.setFocus(this),
                                    !0
                            })),
                        this.$$(t).attachEvent("onBeforeEditStop", function (t, e) {
                            return this.getTopParentView().callEvent("onBeforeEditStop", [e.id || e.row, t, e, this])
                        }),
                        this.$$(t).attachEvent("onAfterEditStop", function (values, event) {
                            if (values.old && values.value && (values.old.trim() === values.value.trim())) {
                                return;
                            } else if (values.value === "") {
                                fileManagerWebix.refreshCursor();
                                fileManagerWebix.refresh();
                                return;
                            }
                            var topParentView = this.getTopParentView();
                            // event row is fileID
                            topParentView.callEvent("onAfterEditStop", [event.row, values, event, this]);
                            topParentView.renameFile(event.row, values.value);
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
                                this.yx(e[i]);
                            }

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
                            if (s.hasOwnProperty(i)) {
                                var e = webix.copy(s[i]);
                                this.structure[i] && this.structure[i].config ? this.structure[i].config = e.config || e : this.structure[i] = e.config || e
                            }
                        }

                },
                defaults: {
                    modes: ["files", "table"],
                    mode: "table",
                    handlers: {},
                    structure: {},
                    templateName: webix.template("#value#"),
                    templateSize: function (t) {
                        for (var e = t.size, i = webix.i18n.filemanager.sizeLabels, s = 0; e / 1024 > 1;) {
                            e /= 1024,
                                s++;
                        }

                        var n = parseInt(e, 10) == e
                            , a = webix.Number.numToStr({
                                decimalDelimiter: webix.i18n.decimalDelimiter,
                                groupDelimiter: webix.i18n.groupDelimiter,
                                decimalSize: n ? 0 : webix.i18n.groupSize
                            });
                        return a(e) + "" + i[s]
                    },
                    templateType: function (t) {
                        return t.type;
                    },
                    templateDate: function (t) {
                        return t.date;
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
                        dwg: "fa fa-file-contract"
                    }
                }
            }, webix.FileManagerUpload, webix.FileManagerStructure, webix.ProgressBar, webix.IdSpace, webix.ui.layout, webix.TreeDataMove, webix.TreeDataLoader, webix.DataLoader, webix.EventSystem, webix.Settings);

            // Inicializador Webix
            webix.ready(() => {
                $.get('GestorCorporativo/VerificarAccesoGestor')
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            if (response.acceso) {
                                cargarDatos();
                            } else {
                                mostrarModal(2, "Este usuario no tiene acceso al gestor de archivos.");
                                $("#noAcceso").show(500);
                            }
                        } else {
                            // Operación no completada.
                            mostrarModal(2, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => mostrarModal(2, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`));
            });
        }

        function cargarDatos() {
            fileManagerWebix = new webix.ui({
                view: "filemanager",
                container: "webix-area",
                scroll: true,
                id: "files",
                handlers: {
                    "download": "GestorCorporativo/DescargarArchivo",
                    "downloadFolder": "GestorCorporativo/DescargarCarpeta",
                    "remove": "GestorCorporativo/EliminarArchivo",
                    "rename": "GestorCorporativo/RenombrarArchivo"
                }
            });

            $$("files").load("GestorCorporativo/ObtenerEstructuraDirectorios").then(agregarEventoMenu);
        }

        function agregarListeners() {

            // Modal aviso
            modalAviso.on('shown.bs.modal', () => {
                setTimeout(() => botonModalAviso.focus(), 100);
            });

            inputArchivo.change(e => {
                if (e.currentTarget.files.length === 0) {
                    botonSubirArchivo.addClass('disabled');
                } else {
                    botonSubirArchivo.removeClass('disabled');
                }
            });

            modalCargaArchivo.on("hide.bs.modal", () => {
                inputArchivo.val('');
                botonSubirArchivo.addClass('disabled');
            });

            botonSubirArchivo.click(() => subirArchivo());

            modalCrearCarpeta.on('hide.bs.modal', () => {
                inputNombreCarpeta.val('');
                botonCrearCarpeta.data().archivoID = null;
                textoPrevioCarpeta.html(`<i class="fas fa-folder"></i>`);
            });

            inputNombreCarpeta.keyup(e => {
                const val = $(e.currentTarget).val();
                textoPrevioCarpeta.html(`<i class="fas fa-folder"></i> ${val}`);
            })

            botonCrearCarpeta.click(() => {
                const nombreCarpeta = inputNombreCarpeta.val();
                const archivoID = botonCrearCarpeta.data().archivoID;

                if (nombreCarpeta == null || nombreCarpeta.trim().length < 2) {
                    mostrarModal(2, 'Nombre de carpeta inválido.');
                    return;
                }
                crearCarpeta(nombreCarpeta.trim(), archivoID);
            });

        }

        function crearCarpeta(nombreCarpeta, padreID) {
            $.post('GestorCorporativo/CrearCarpeta', { nombreCarpeta, padreID })
                .then(response => {
                    if (response.success) {
                        modalCrearCarpeta.modal('hide');
                        $$("files").load("GestorCorporativo/ObtenerEstructuraDirectorios")
                            .then(() => {
                                fileManagerWebix.refreshCursor();
                                fileManagerWebix.refresh();
                                mostrarModal(1, 'Carpeta creada correctamente.');
                            });
                    } else {
                        mostrarModal(2, `Error al crear la carpeta: ${response.message}`);
                    }
                }, () => mostrarModal(2, 'Ocurrió un error al crear la carpeta.'));
        }

        function mostrarModalSubirArchivo() {
            modalCargaArchivo.modal({
                backdrop: 'static',
                keyboard: false
            });
            tituloModalCargaArchivo.html(`Subir archivo <i class="fas fa-file-upload"></i>`);
        }

        function subirArchivo() {
            const archivo = inputArchivo[0].files[0];

            if (esExtensionInvalida(archivo.name)) {
                mostrarModal(2, "El archivo tiene una extensión inválida");
                return;
            }

            const padreID = getActiveParentItemID();

            const data = new FormData();
            data.append("archivo", archivo);
            data.append("padreID", padreID);
            $.ajax({
                url: 'GestorCorporativo/SubirArchivo',
                data,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
            })
                .then(addFileToTree, () => mostrarModal(2, "Error al intentar subir el archivo."))
        }

        function addFileToTree(response) {
            if (response.success) {
                const { archivo } = response;
                fileManagerWebix.add({
                    id: archivo.id,
                    value: archivo.value,
                    type: archivo.type,
                    date: archivo.date,
                    grupoCarpeta: archivo.grupoCarpeta,
                    subGrupoCarpeta: archivo.subGrupoCarpeta,
                    permisos: archivo.permisos
                }, -1, archivo.pId);
                fileManagerWebix.refreshCursor();
                fileManagerWebix.refresh();
                mostrarModal(1, `Archivo agregado correctamente.`);
            } else {
                mostrarModal(2, `Error al cargar el archivo: ${response.message}`);
            }
        }

        function esExtensionInvalida(nombreArchivo) {
            const extension = nombreArchivo.split('.').pop();
            const extensionNoEsValida = extensionesNoAceptadas.filter(x => x === extension);
            return extensionNoEsValida.length > 0 ? true : false;
        }

        function mostrarModal(titulo, cuerpo) {
            const tituloModal = titulo === 1 ? 'Éxito <i class="far fa-check-circle"></i>' : 'Aviso <i class="fas fa-exclamation-triangle"></i>'
            modalAvisoTitutlo.html(tituloModal);
            modalAvisoCuerpo.text(cuerpo);
            modalAviso.modal('show');
        }

        function mostrarPermisosArchivo(archivo) {

            limpiarMenu();

            const actionMenu = fileManagerWebix.getMenu();

            // Se verifican los permisos de la carpeta padre
            const carpetaPadre = fileManagerWebix.getItem(archivo.$parent);
            const permisosCarpeta = carpetaPadre.permisos;

            if (permisosCarpeta.puedeSubir) {
                actionMenu.add(MENU_BASE[2]);
            }
            if (permisosCarpeta.puedeCrear) {
                actionMenu.add(MENU_BASE[0]);
            }

            const { permisos } = archivo;

            if (permisos.puedeDescargarArchivo) {
                actionMenu.add(MENU_BASE[4]);
            }
            if (permisos.puedeEliminar) {
                actionMenu.add(MENU_BASE[5]);
            }
        }

        function mostrarPermisosCarpeta(permisos) {
            limpiarMenu();
            const actionMenu = fileManagerWebix.getMenu();

            if (permisos.puedeSubir) {
                actionMenu.add(MENU_BASE[2]);
            }

            if (permisos.puedeCrear) {
                const archivoActivo = getActiveItem();

                // Crear sesion = MENU BASE 6
                if ((archivoActivo && archivoActivo.type === 'folder' && archivoActivo.grupoCarpeta > 1 && archivoActivo.grupoCarpeta < 5 && archivoActivo.$level === 3)) {
                    actionMenu.add(MENU_BASE[6]);
                } else {
                    actionMenu.add(MENU_BASE[0]);
                }

                actionMenu.add(MENU_BASE[1]);
            }

            if (permisos.puedeDescargarCarpeta) {
                actionMenu.add(MENU_BASE[3]);
            }

            if (permisos.puedeEliminar) {
                actionMenu.add(MENU_BASE[5]);
            }
        }

        function limpiarMenu() {
            const actionMenu = fileManagerWebix.getMenu();
            actionMenu.clearAll();
            actionMenu.refresh();
            actionMenu.render();
        }

        function agregarEventoMenu() {
            $('.webix_view').mousedown(function (event) {
                if (event.which === 3) {
                    let itemActualID = fileManagerWebix.getActive();
                    let archivo = fileManagerWebix.getItem(itemActualID);

                    if (archivo != null) {
                        if ("folder" === archivo.type) {
                            mostrarPermisosCarpeta(archivo.permisos);
                        } else {
                            mostrarPermisosArchivo(archivo);
                        }
                    } else {
                        limpiarMenu();
                    }
                    const actionMenu = fileManagerWebix.getMenu();
                    // actionMenu.refresh();
                    actionMenu.render();
                }
            });
        }

        function getActiveParentItemID() {
            let itemSeleccionadoID = fileManagerWebix.getActive();
            let archivo = fileManagerWebix.getItem(itemSeleccionadoID);

            if (archivo == null) {
                itemSeleccionadoID = fileManagerWebix.getCursor();//retorna el ID de la carpeta seleccionada en el tree (izquierda)
                archivo = fileManagerWebix.getItem(itemSeleccionadoID);
            }
            return "folder" === archivo.type ? archivo.id : archivo.$parent;
        }

        function getActiveItem() {
            const archivo = fileManagerWebix.getItem(fileManagerWebix.getActive());
            return archivo != null ? archivo : null;
        }

        init();
    }
    $(() => GestorCorporativo.GestorCorporativo = new GestorCorporativo())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();
