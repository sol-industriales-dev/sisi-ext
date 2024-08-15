(function () {
    $.namespace('sigoplan.seguimientoacuerdos.dashboard');
    dashboard = function () {
        mensajes = {
            PROCESANDO: 'Procesando...'
        },
            currentMinuta = 0,
            spMinutaOwner = $("#spMinutaOwner"),
            minutesObj = new Array(),
            activitiesObj = new Array(),
            promoverObj = {
                actividadID: 0,
                observacion: "",
                columna: 0
            },
            newResponsables = $("#newResponsables"),
            currentActivity = 0,
            timeline = $("#timeline"),
            divDialogPromover = $("#divDialogPromover"),
            cP0 = $(".cP0"),
            cP25 = $(".cP25"),
            cP50 = $(".cP50"),
            cP75 = $(".cP75"),
            cP100 = $(".cP100"),
            txtObservacion = $("#txtObservacion"),
            spPromover = $("#spPromover"),
            btnPromover = $("#btnPromover"),
            modalActividad = $("#modalActividad"),
            txtMActividad = $("#txtMActividad"),
            slMResponsable = $("#slMResponsable"),
            txtMDescripcionActividad = $("#txtMDescripcionActividad"),
            slOrganigrama = $("#slOrganigrama"),
            slMPrioridad = $("#slMPrioridad"),
            txtFechaInicio = $("#txtFechaInicio"),
            txtFechaCompromiso = $("#txtFechaCompromiso"),
            divVerComentario = $("#divVerComentario"),
            ulComentarios = $("#ulComentarios"),
            btnAddComentario = $("#btnAddComentario"),
            txtComentarios = $("#txtComentarios"),
            txtRevisa = $("#txtRevisa"),
            fupAdjunto = $("#fupAdjunto");

        _ui_item = null;
        function init() {
            $(".promoverActividadesCantidadLO").removeClass("esteEsDashBoard");
            $(".promoverActividadesCantidadLO").addClass("esteEsDashBoard");
            loadOrganigrama(Number(usuarioID));
            slOrganigrama.change(loadDownPerfil);
            //Kanban
            $(".column").sortable({
                connectWith: ".column",
                placeholder: "ui-state-highlight",
                handle: ".portlet-header",
                cancel: ".portlet-toggle",
                start: function (event, ui) {
                    ui.item.addClass('tilt');
                },
                stop: function (event, ui) {
                    ui.item.removeClass('tilt');
                },
                receive: function (event, ui) {
                    $.blockUI({ message: 'Procesando... ¡Espere un momento porfavor!!' });
                    var asd = 0;
                    var columna = $(event.target).data("columna");
                    var aID = $(ui.item).data("id");
                    var cID = $(ui.item).attr("id");
                    var responsableID = $(ui.item).data("responsableid");
                    _ui_item = $(ui.item);
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: "/SeguimientoAcuerdos/validarPromoverAvanceActividad",
                        data: { actividadID: Number(aID) },
                        success: function (response) {
                            if (response.success === true) {
                                $("#" + cID).attr("data-columna", columna);
                                var obj = Enumerable.From(activitiesObj).Where(function (x) { return x.id == aID }).Select(function (x) { return x }).FirstOrDefault();
                                obj.columna = columna;
                                headerActividad($(ui.item), columna);
                                updateAvanceActividad(obj.id, obj.columna);

                            }
                            else {
                                $.ajax({
                                    datatype: "json",
                                    type: "POST",
                                    url: "/SeguimientoAcuerdos/esResponsableACtividad",
                                    data: { id: Number(aID), u: usuarioID },
                                    success: function (response) {
                                        if (response.esResponsable == true) {
                                            promoverObj.actividadID = aID;
                                            promoverObj.columna = columna;

                                            spPromover.html("Promover a " + columna + "%");
                                            divDialogPromover.modal("show");
                                            // $(ui.sender).sortable('cancel');
                                            $.unblockUI();
                                        } else {
                                            $.unblockUI();
                                            AlertaGeneral("Alerta", 'Solo la persona seleccionada como "Autoriza" puede mover el avance de una actividad');
                                            $(ui.sender).sortable('cancel');

                                        }
                                    },
                                    error: function () {
                                        $.unblockUI();
                                        $(ui.sender).sortable('cancel');
                                    }
                                });

                            }
                        },
                        error: function () {
                            $.unblockUI();
                            $(ui.sender).sortable('cancel');
                        }
                    });

                }
            });
            //initColumnasEvents(usuarioID);
            $(".column").on("dblclick", ".portlet-header", function () {
                var _this = $(this).parent();
                currentActivity = Number(_this.data("id"));
                loadActiviy();
            });
            btnPromover.click(fnPromover);
            btnAddComentario.click(insertCommentary);
            //Gantt
            gantt.config.readonly = true;
            gantt.config.drag_move = false; //disables the possibility to move tasks by dnd
            gantt.config.drag_links = false; //disables the possibility to create links by dnd
            gantt.config.drag_progress = false; //disables the possibility to change the task //progress by dragging the progress knob
            gantt.config.drag_resize = false; //disables the possibility to resize tasks by dnd
            gantt.config.grid_width = 480;
            gantt.config.add_column = false;
            gantt.config.columns = [
                { name: "text", label: "actividad", tree: true, width: '*' },
                { name: "duration", label: "Dias", tree: true, width: '*' },
                {
                    name: "progress", label: "Progreso", width: 50, align: "center",
                    template: function (item) {
                        return Math.round(item.progress * 100) + "%";
                    }
                },
                {
                    name: "priority", label: "prioridad", width: 70, align: "center",
                    template: function (item) {
                        if (item.priority == 3)
                            return "Baja";
                        else if (item.priority == 2)
                            return "Media";
                        else if (item.priority == 1)
                            return "Alta";
                        else
                            return "";

                    }
                },
                {
                    name: "assigned", label: "responsable", align: "center", width: 100,
                    template: function (item) {
                        if (!item.users) return "Nadie";
                        return item.users.join(", ");
                    }
                }
            ];
            gantt.templates.progress_text = function (start, end, task) {
                return "<span style='text-align:left;'></span>";
            };
            gantt.init("gantt_here");

            timeline.on("click", ".minuta", function () {
                var _this = $(this);
                currentMinuta = Number(_this.data("id"));
                window.location.href = "/SeguimientoAcuerdos/Acuerdo?minuta=" + currentMinuta;
            });
            timeline.on("click", ".filtroMinuta", function () {
                var _this = $(this);
                currentMinuta = Number(_this.data("id"));
                $(".column").empty();
                var actividades = getActividadesByMinuta();
                setAllActividades(actividades);
                loadGantt(actividades);
                $(".timeline-panel").removeClass("minutaSeleccionada");
                _this.parent().find(".timeline-panel").addClass("minutaSeleccionada");
            });
            timeline.on("click", ".filtroMinutaAll", function () {
                currentMinuta = 0;
                setAllActividades(activitiesObj);
                loadGanttAll();
                $(".timeline-panel").removeClass("minutaSeleccionada");
            });
        }
        function fnPromover() {
            promoverObj.observacion = txtObservacion.val();
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/promoverAvanceActividad",
                data: { obj: promoverObj },
                success: function (response) {
                    AlertaGeneral("Confirmación", "Se mando la notificación a la persona que se marco como autorizador de esta actividad");
                    headerActividad(_ui_item, promoverObj.columna);
                    updateAvanceActividad(promoverObj.actividadID, promoverObj.columna);
                },
                error: function () {
                }
            });
        }
        function loadDownPerfil() {
            var id = Number($(this).val());
            var nombre = $("#slOrganigrama option:selected").text();
            //initColumnasEvents(id);
            spMinutaOwner.val(nombre.toUpperCase());
            loadDashboard(id);

        }
        function updateAvanceActividad(id, columna) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/updateAvanceActividad",
                data: { id: id, columna: columna },
                success: function (response) {
                    if (currentMinuta == 0) {
                        loadGanttAll();
                    }
                    else {
                        loadGantt(activitiesObj);
                    }
                },
                error: function () {
                }
            });
        }
        function initDates(inicio, fin) {
            txtFechaInicio.datepicker().datepicker("setDate", inicio);
            txtFechaCompromiso.datepicker().datepicker("setDate", fin);
        }
        function loadActiviy() {
            var data = Enumerable.From(activitiesObj).Where(function (x) { return x.id === currentActivity }).Select(function (x) { return x }).FirstOrDefault();
            slMResponsable.val(data.responsable);
            txtMActividad.val(data.actividad);
            txtMDescripcionActividad.val(data.descripcion);
            txtFechaInicio.val(data.fechaInicio);
            txtFechaCompromiso.val(data.fechaCompromiso);
            slMPrioridad.val(data.prioridad);
            setAllResponsables(currentActivity);
            txtRevisa.val(data.revisa);
            txtRevisa.data("revisaid", data.revisaID);
            modalActividad.modal("show");
        }
        function setAllResponsables(actividadID) {
            $(".ResponsablesInActivity").find(".divResponsables").remove();
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/getResponsablesPorActividad",
                data: { id: actividadID },
                success: function (response) {
                    var data = response.obj;
                    $.each(data, function (i, e) {
                        var html = '<div class="divResponsables">';
                        html += '    <div class="userContainer">';
                        html += '        <span class="ResponsablesFill">&nbsp;</span>';
                        html += '        <span class="ResponsablesComponent" data-user="' + e.usuario + '" data-userid="' + e.usuarioID + '">';
                        html += e.usuario;
                        html += '        </span>';
                        html += '        <button type="button" class="ResponsablesDelete" disabled>&nbsp;X</button>';
                        html += '    </div>';
                        html += '</div>';
                        $(html).insertBefore(newResponsables);
                    });
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function newResponsablesFromProjectInit() {
            newResponsables.focus();
            newResponsables.autocomplete("search");
        }
        function getNewCommentary() {
            var r = {};
            r.id = 0;
            r.actividadID = currentActivity;
            r.comentario = txtComentarios.val();
            r.usuarioNombre = usuarioNombre;
            r.usuarioID = usuarioID;
            r.fecha = gFecha;
            r.tipo = 'new';
            r.adjuntoNombre = "";
            return r;
        }
        function insertCommentary() {

            var obj = getNewCommentary();
            obj.usuarioNombre = '';
            var formData = new FormData();
            //var filesVisor = document.getElementById("fupAdjunto").files.length;
            var file = document.getElementById("fupAdjunto").files[0];
            formData.append("fupAdjunto", file);
            formData.append("obj", JSON.stringify(obj));
            if (file != undefined) {
                $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });
            }
            $.ajax({
                type: "POST",
                url: "/SeguimientoAcuerdos/guardarComentario",
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    fupAdjunto.val("");
                    $.unblockUI();
                    obj.id = Number(response.obj.id);
                    obj.adjuntoNombre = response.obj.adjuntoNombre;
                    var data = Enumerable.From(activitiesObj).Where(function (x) { return x.id === currentActivity }).Select(function (x) { return x }).FirstOrDefault();

                    data.comentarios.push(obj);
                    data.comentariosCount = data.comentarios.length;

                    $("#a" + currentActivity).find(".portlet-toggle").each(function () {
                        var icon = $(this);
                        icon.closest(".portlet").find(".comentariesCount").html(data.comentariosCount);
                    });
                    setComentarios(data.comentarios);
                    txtComentarios.val("");
                },
                error: function (error) {
                    $.unblockUI();
                }
            });
        }
        function setComentarios(data) {
            var htmlComentario = "";
            $.each(data, function (i, e) {
                htmlComentario += "<li class='comentario' data-id='" + e.id + "'>";
                htmlComentario += "    <div class='timeline-item'>";
                htmlComentario += "        <span class='time'><i class='fa fa-clock-o'></i>" + e.fecha + "</span>";
                htmlComentario += "        <h3 class='timeline-header'><a href='#'>" + e.usuarioNombre + "</a></h3>";
                htmlComentario += "        <div class='timeline-body'>";
                htmlComentario += "             " + e.comentario;
                htmlComentario += "        </div>";
                if (e.adjuntoNombre != null && e.adjuntoNombre != "") {
                    htmlComentario += "        <div class='timeline-footer'>";
                    htmlComentario += "             <a href='/SeguimientoAcuerdos/getComentarioArchivoAdjunto/?id=" + e.id + "' class='openComentarios'></span>Descargar: " + e.adjuntoNombre + "</a>";
                    htmlComentario += "        </div>";
                }
                htmlComentario += "    </div>";
                htmlComentario += "</li>";
            });
            ulComentarios.html(htmlComentario);
        }
        function loadGanttAll() {
            gantt.clearAll();
            var tasks = {};
            tasks.data = new Array();
            var cont = 1;
            $.each(minutesObj, function (i, e) {
                var obj = {};
                obj.id = cont++;
                obj.text = e.titulo;
                obj.start_date = e.fechaInicio;
                obj.duration = "" + restaFechas(e.fechaInicio, e.fechaCompromiso);
                obj.parent = null;
                var actividadesTotal = Enumerable.From(activitiesObj).Where(function (x) { return x.minutaID == e.id }).Count();
                var actividadesAvance = Enumerable.From(activitiesObj).Where(function (x) { return x.minutaID == e.id }).Select(function (x) { return x.columna }).Sum();
                var data = (actividadesAvance / actividadesTotal) / 100;
                obj.progress = isNaN(Number(data)) ? 0 : data;
                obj.open = false;
                obj.users = new Array();
                obj.users.push(" ");
                obj.priority = 4;
                obj.color = "#EEEEEE";
                obj.textColor = "black";
                obj.progressColor = "#5cb85c";
                obj.minutaID = e.id;
                tasks.data.push(obj);
            });
            $.each(activitiesObj, function (i, e) {
                var obj = {};
                obj.id = cont++;
                obj.text = e.actividad;
                obj.start_date = e.fechaInicio;
                obj.duration = "" + restaFechas(e.fechaInicio, e.fechaCompromiso);
                var parent = Enumerable.From(tasks.data).Where(function (x) { return x.minutaID == e.minutaID }).Select(function (x) { return x.id }).FirstOrDefault();
                obj.parent = parent;
                obj.progress = getPorcentajeAvance(e.columna);
                obj.open = true;
                obj.users = new Array();
                obj.users.push(e.responsable);
                obj.priority = e.prioridad;
                obj.color = getTaskColorPriority(Number(e.prioridad));
                obj.textColor = "black";
                obj.progressColor = "#5cb85c";
                obj.minutaID = 0;
                tasks.data.push(obj);
            });
            gantt.parse(tasks);
            $.unblockUI();
        }
        function loadGantt(data) {
            gantt.clearAll();
            var tasks = {};
            tasks.data = new Array();
            $.each(data, function (i, e) {
                var obj = {};
                obj.id = e.id;
                obj.text = e.actividad;
                obj.start_date = e.fechaInicio;
                obj.duration = "" + restaFechas(e.fechaInicio, e.fechaCompromiso);
                obj.parent = null;
                obj.progress = getPorcentajeAvance(e.columna);
                obj.open = true;
                obj.users = new Array();
                obj.users.push(e.responsable);
                obj.priority = e.prioridad;
                obj.color = getTaskColorPriority(Number(e.prioridad));
                obj.textColor = "black";
                obj.progressColor = "#5cb85c";
                tasks.data.push(obj);
            });

            gantt.parse(tasks);
            $.unblockUI();
        }
        function restaFechas(f1, f2) {
            var aFecha1 = f1.split('/');
            var aFecha2 = f2.split('/');
            var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
            var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
            var dif = fFecha2 - fFecha1;
            var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
            return Number(dias) + 1;
        }
        function getPorcentajeAvance(o) {
            return Number(o) / 100;
        }
        function loadDashboard(id) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/getDashboard",
                data: { id: id },
                success: function (response) {
                    minutesObj = response.minutas;
                    activitiesObj = response.actividades;
                    setAllMinutas(minutesObj);
                    setAllActividades(activitiesObj)
                    loadGanttAll();
                    $.unblockUI();
                },
                error: function () {
                }
            });
        }
        function setAllMinutas(data) {
            timeline.html("");
            var htmlP = '<li class="timeline-item" style="min-width: 0px !important; padding-right: 149px;">';
            htmlP += '    <div class="timeline-badge filtroMinutaAll"><i class="glyphicon glyphicon-home" title="Mostrar las actividades asignadas de todas las minutas"></i></div>';
            htmlP += '</li>';
            timeline.append(htmlP);
            $.each(data, function (i, e) {
                var html = '<li class="timeline-item" data-id="' + e.id + '">';

                if (e.ver === true) {
                    if (Number(usuarioID) == Number(e.creadorID)) {
                        html += '    <div class="timeline-badge primary filtroMinuta" data-id="' + e.id + '"><i class="glyphicon glyphicon-filter" title="Mostrar solo las actividades asignadas de esta minuta"></i></div>';
                    }
                    else {
                        html += '    <div class="timeline-badge primary filtroMinuta" data-id="' + e.id + '"><i class="glyphicon glyphicon-filter"></i></div>';
                    }

                    html += '    <div class="timeline-panel minuta" data-id="' + e.id + '">';
                }
                else {
                    html += '    <div class="timeline-badge filtroMinuta" style="background-color:black;" data-id="' + e.id + '"><i class="glyphicon glyphicon-filter"></i></div>';
                    html += '    <div class="timeline-panel" data-id="' + e.id + '">';
                }
                html += '        <div class="timeline-heading">';
                html += '            <h4 class="timeline-title" title="Proyecto">' + e.proyecto + '</h4>';
                html += '            <p><small class="text-muted" title="Fecha de registro"><i class="glyphicon glyphicon-time"></i>' + e.fecha + '</small></p>';
                html += '        </div>';
                html += '        <div class="timeline-body">';
                html += '            <p><span title="Titulo de minuta">' + e.titulo + '</span></p>';
                html += '        </div>';
                html += '    </div>';
                html += '</li>';
                timeline.append(html);
            });

        }
        function setAllActividades(data) {
            $(".column").empty();
            $.each(data, function (i, obj) {
                var html = ' <div id="a' + obj.id + '" class="portlet" data-columna="' + obj.columna + '" data-id="' + obj.id + '" data-responsableID="' + obj.responsableID + '" data-orden="' + obj.Orden + '" data-tipo="' + obj.tipo + '">';
                html += '   <div class="portlet-header success">';
                html += '       <div class="acttividadElementoMain" title="Titulo de minuta">' + obj.minuta + '</div>';
                html += '       <div class="acttividadElemento" title="Descripción de actividad">' + obj.actividad + '</div>';
                html += '       <div class="acttividadElemento" title="Fecha compromiso">' + obj.fechaCompromiso + '</div>';
                if (obj.interesado === true) {
                    html += '       <div class="componenteInteresado" title="Interesado en actividad"></div>';
                }
                html += '       <div class="componentePrioridad prioridad' + obj.prioridad + '" title="' + ((obj.prioridad == 1) ? "Prioridad Alta" : (obj.prioridad == 2) ? "Prioridad Media" : "Prioridad Baja") + '"></div>';
                html += '   </div>';
                html += '   <div class="portlet-content">';
                html += obj.descripcion;
                html += '   </div>';
                html += '   <div class="panel-footer">';
                html += '       <a href="#" class="openComentarios" data-id="' + obj.id + '" >(<span class="comentariesCount">' + obj.comentariosCount + '</span> comentarios) - Ver</a>';
                html += '   </div>';
                html += '</div>';

                if (obj.columna == 0) {
                    cP0.append(html);
                }
                else if (obj.columna == 25) {
                    cP25.append(html);
                }
                else if (obj.columna == 50) {
                    cP50.append(html);
                }
                else if (obj.columna == 75) {
                    cP75.append(html);
                }
                else if (obj.columna == 100) {
                    cP100.append(html);
                }
                headerActividad($("#a" + obj.id), obj.columna);
                //loadGantt(activitiesObj);
                $("#a" + obj.id)
                    .addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
                    .find(".portlet-header")
                    .addClass("ui-widget-header ui-corner-all")
                    .prepend("<span class='ui-icon ui-icon-minusthick portlet-toggle'></span>");

                $("#a" + obj.id).find(".portlet-toggle").on("click", function () {
                    var icon = $(this);
                    icon.toggleClass("ui-icon-minusthick ui-icon-plusthick");
                    icon.closest(".portlet").find(".portlet-content").toggle();
                    icon.closest(".portlet").find(".panel-footer").toggle();
                });
                $("#a" + obj.id).find(".openComentarios").on("click", function (event) {
                    event.preventDefault();
                    currentActivity = Number($(this).data("id"));
                    var data = Enumerable.From(activitiesObj).Where(function (x) { return x.id == currentActivity }).Select(function (x) { return x }).FirstOrDefault();
                    setComentarios(data.comentarios);
                    txtComentarios.val("");
                    divVerComentario.modal("show");
                });
                $("#a" + obj.id).find(".portlet-toggle").trigger("click");
            });
        }
        function headerActividad(_this, columna) {
            if (columna == 0) {
                _this.find(".portlet-header").addClass("C0HeaderActivity");
                _this.find(".portlet-header").removeClass("C100HeaderActivity");
            }
            else if (columna == 25) {
                _this.find(".portlet-header").removeClass("C0HeaderActivity");
                _this.find(".portlet-header").removeClass("C100HeaderActivity");
            }
            else if (columna == 50) {
                _this.find(".portlet-header").removeClass("C0HeaderActivity");
                _this.find(".portlet-header").removeClass("C100HeaderActivity");
            }
            else if (columna == 75) {
                _this.find(".portlet-header").removeClass("C0HeaderActivity");
                _this.find(".portlet-header").removeClass("C100HeaderActivity");
            }
            else if (columna == 100) {
                _this.find(".portlet-header").removeClass("C0HeaderActivity");
                _this.find(".portlet-header").addClass("C100HeaderActivity");
            }

        }
        function getTaskColorPriority(prioridad) {
            var color = "";
            if (prioridad == 1) {
                color = "red";//Alta-Rojo
            }
            else if (prioridad == 2) {
                color = "yellow";//Media-amarillo
            }
            else if (prioridad == 3) {
                color = "#3ea9d9";//Baja -Azul
            }
            return color;
        }
        function getActividadesByMinuta() {
            var obj = Enumerable.From(activitiesObj).Where(function (x) { return x.minutaID == currentMinuta }).ToArray();
            return obj;
        }
        function loadOrganigrama(id) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/getOrganigrama",
                data: { id: id },
                async: false,
                success: function (response) {
                    var html = "";
                    var o = response.obj;
                    $.each(o, function (i, e) {
                        html += '<option value="' + e.usuarioID + '">' + e.usuario + '</option>';
                        if (e.childsCount > 0) {
                            html += '<optgroup label="Dependientes de ' + e.usuario + '">';
                            $.each(e.childs, function (i2, e2) {
                                html += '<option value="' + e2.usuarioID + '">' + e2.usuario + '</option>';
                                if (e2.childsCount > 0) {
                                    html += '<optgroup label="Dependientes de ' + e2.usuario + '">';
                                    $.each(e2.childs, function (i3, e3) {
                                        html += '<option value="' + e3.usuarioID + '">' + e3.usuario + '</option>';
                                    });
                                    html += '</optgroup>';
                                }
                            });
                            html += '</optgroup>';
                        }
                    });
                    slOrganigrama.html(html);

                },
                error: function () {
                }
            });
            slOrganigrama.change();
        }

        init();
    };
    $(document).ready(function () {
        sigoplan.seguimientoacuerdos.dashboard = new dashboard();
    });
})();
