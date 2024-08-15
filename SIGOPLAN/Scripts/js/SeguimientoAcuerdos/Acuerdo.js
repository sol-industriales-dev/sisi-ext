(function () {
    $.namespace('sigoplan.seguimientoacuerdos.seguimiento');
    seguimiento = function () {
        mensajes = {
            NOMBRE: 'Reporte Comparativa de Tipos',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },
            minutaID = 0,
            activitiesObj = new Array(),
            currentActivity = 0,
            nuevaVersion = false,
            txtProyecto = $("#txtProyecto"),
            txtTituloMinuta = $("#txtTituloMinuta"),
            txtLugar = $("#txtLugar"),
            txtFechaMinuta = $("#txtFechaMinuta"),
            slHoraInicio = $("#slHoraInicio"),
            slHoraFin = $("#slHoraFin"),
            txtLider = $("#txtLider"),
            btnGuardar = $("#btnGuardar"),
            btnGuardarContinuar = $("#btnGuardarContinuar"),
            btnIniciar = $("#btnIniciar"),
            btnNuevo = $("#btnNuevo"),
            usersInProyect = $(".usersInProyect"),
            summernote = $("#summernote"),
            btnDisplayUsers = $("#btnDisplayUsers"),
            newUser = $("#newUser"),
            ResponsablesInActivity = $(".ResponsablesInActivity"),
            interestedInActivity = $(".interestedInActivity"),
            newResponsables = $("#newResponsables"),
            newInterested = $("#newInterested"),
            btnDisplayResponsables = $("#btnDisplayResponsables"),
            btnDisplayInterested = $("#btnDisplayInterested"),
            modalActividad = $("#modalActividad"),
            btnActividad = $("#btnActividad"),
            btnAddActivity = $("#btnAddActivity"),
            btnUpdateActivity = $("#btnUpdateActivity"),
            txtMActividad = $("#txtMActividad"),
            txtMDescripcionActividad = $("#txtMDescripcionActividad"),
            slMResponsable = $("#slMResponsable"),
            slMPrioridad = $("#slMPrioridad"),
            txtFechaInicio = $("#txtFechaInicio"),
            txtFechaCompromiso = $("#txtFechaCompromiso"),
            cP0 = $(".cP0"),
            cP25 = $(".cP25"),
            cP50 = $(".cP50"),
            cP75 = $(".cP75"),
            cP100 = $(".cP100"),
            divVerComentario = $("#divVerComentario"),
            ulComentarios = $("#ulComentarios"),
            btnAddComentario = $("#btnAddComentario"),
            txtComentarios = $("#txtComentarios"),
            desbloqueo = $(".desbloqueo"),
            btnImprimirMinuta = $("#btnImprimirMinuta"),
            btnImprimirListaAsistencia = $("#btnImprimirListaAsistencia"),
            fupAdjunto = $("#fupAdjunto"),
            btnEnviarCorreo = $("#btnEnviarCorreo"),
            slMCorreos = $("#slMCorreos"),
            btnSendMail = $("#btnSendMail"),
            slUsuarios = $("#slUsuarios"),
            modalCorreos = $("#modalCorreos"),
            btnModNuevaMinuta = $("#btnModNuevaMinuta"),
            btnNuevaVersion = $("#btnNuevaVersion"),
            txtRevisa = $("#txtRevisa"),
            divNuevaVersion = $("#divNuevaVersion"),
            _Modalidad = "modoEditable",
            _revisa = "",
            _revisaID = 0;
        function init() {
            addRequired();
            slHoraInicio.mask('00:00');
            slHoraFin.mask('00:00');
            convertToMultiselect("#slMCorreos");
            btnEnviarCorreo.click(fnOpenEnviarCorreo);
            btnSendMail.click(fnEnviarCorreos);
            slUsuarios.change(fnLoadActividadesByParticipante);
            btnImprimirMinuta.hide();
            btnImprimirListaAsistencia.hide();
            btnEnviarCorreo.hide();
            btnIniciar.show();
            btnGuardar.hide();
            btnGuardarContinuar.hide();
            btnNuevo.hide();
            desbloqueo.block({ message: null });
            txtFechaMinuta.datepicker().datepicker("setDate", new Date());
            //Participantes
            btnIniciar.click(iniciarMinuta);
            btnGuardar.click(guardarMinuta);
            btnGuardarContinuar.click(guardarMinutaContinuar);
            btnNuevo.click(nuevaMinuta);
            //btnNuevo.hide();
            //Participantes
            usersInProyect.on("mouseenter mouseleave", ".userComponent", hoverUserFromProject);
            usersInProyect.on("mouseenter mouseleave", ".userDelete", hoverdelUserFromProject);
            usersInProyect.on("click", ".userComponent", getUserFromProject);
            usersInProyect.on("click", ".userDelete", delUserFromProject);
            newUser.click(newUserFromProjectInit);
            //newUser.focusin(newUserFromProjectInit);
            btnDisplayUsers.click(newUserFromProjectInit);
            newUser.autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '/SeguimientoAcuerdos/getListParticipantes',
                        dataType: 'json',
                        data: {
                            term: request.term,
                            minuta: minutaID
                        },
                        success: function (data) {
                            response(data);
                        }
                    });
                },
                minLength: 0,
                select: newUserSearch
            }).autocomplete("instance")._renderItem = function (ul, item) {
                var t = item.label.replace(new RegExp('(' + this.term + ')', 'gi'), "<b>$1</b>");
                return $("<li>")
                    .data("item.autocomplete", item)
                    .append("<div>" + t + "</div>")
                    .appendTo(ul);
            };
            fillComboPaticipantes();
            //Responsables por actividad
            ResponsablesInActivity.on("mouseenter mouseleave", ".ResponsablesComponent", hoverResponsablesFromProject);
            ResponsablesInActivity.on("mouseenter mouseleave", ".ResponsablesDelete", hoverdelResponsablesFromProject);
            ResponsablesInActivity.on("click", ".ResponsablesComponent", getResponsablesFromProject);
            ResponsablesInActivity.on("click", ".ResponsablesDelete", delResponsablesFromProject);
            newResponsables.click(newResponsablesFromProjectInit);
            btnDisplayResponsables.click(newResponsablesFromProjectInit);

            newResponsables.autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '/SeguimientoAcuerdos/getListResponsables',
                        dataType: 'json',
                        data: {
                            term: request.term,
                            actividad: currentActivity
                        },
                        success: function (data) {
                            response(data);
                        }
                    });
                },
                minLength: 0,
                select: newResponsablesSearch
            }).autocomplete("instance")._renderItem = function (ul, item) {
                var t = item.label.replace(new RegExp('(' + this.term + ')', 'gi'), "<b>$1</b>");
                return $("<li>")
                    .data("item.autocomplete", item)
                    .append("<div>" + t + "</div>")
                    .appendTo(ul);
            };
            //Interesados por actividad
            interestedInActivity.on("mouseenter mouseleave", ".interestedComponent", hoverInterestedFromProject);
            interestedInActivity.on("mouseenter mouseleave", ".interestedDelete", hoverdelInterestedFromProject);
            interestedInActivity.on("click", ".interestedComponent", getInterestedFromProject);
            interestedInActivity.on("click", ".interestedDelete", delInterestedFromProject);
            newInterested.click(newInterestedFromProjectInit);
            btnDisplayInterested.click(newInterestedFromProjectInit);

            newInterested.autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '/SeguimientoAcuerdos/getListInteresados',
                        dataType: 'json',
                        data: {
                            term: request.term,
                            actividad: currentActivity
                        },
                        success: function (data) {
                            response(data);
                        }
                    });
                },
                minLength: 0,
                select: newInterestedSearch
            }).autocomplete("instance")._renderItem = function (ul, item) {
                var t = item.label.replace(new RegExp('(' + this.term + ')', 'gi'), "<b>$1</b>");
                return $("<li>")
                    .data("item.autocomplete", item)
                    .append("<div>" + t + "</div>")
                    .appendTo(ul);
            };
            btnActividad.click(openModalActividad);
            initDates();
            //descripcion
            summernote.summernote({
                height: 210,
                minHeight: null,
                maxHeight: null,
                focus: false,
                lang: 'es-ES'
            });
            summernote.summernote('code', " ");
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
                receive: (event, ui) => {

                    $.blockUI({ message: 'Actualizando minuta...' });
                    const columna = $(event.target).data("columna");
                    const aID = $(ui.item).data("id");
                    const cID = $(ui.item).attr("id");

                    $.post("/SeguimientoAcuerdos/validarPromoverAvanceActividad", { actividadID: Number(aID), desdeMinuta: true })
                        .done(response => {
                            if (response.success) {
                                $("#" + cID).attr("data-columna", columna);
                                var obj = Enumerable.From(activitiesObj).Where(function (x) { return x.id == aID }).Select(function (x) { return x }).FirstOrDefault();
                                obj.columna = columna;
                                headerActividad($(ui.item), columna);
                                updateAvanceActividad(obj.id, obj.columna);
                            } else {
                                $.unblockUI();
                                AlertaGeneral("Aviso", `Solo el líder de la minuta puede actualizar la minuta desde esta vista. 
                                Los demás usuarios deben realizar esta acción desde el Dashboard.`);
                                $(ui.sender).sortable('cancel');
                            }
                        })
                        .fail(() => {
                            $.unblockUI();
                            $(ui.sender).sortable('cancel');
                            AlertaGeneral("Error", "Ocurrió un error al intentar promover el avance.");
                        });
                }
            });
            $(".column").on("dblclick", ".portlet-header", function () {
                var _this = $(this).parent();
                currentActivity = Number(_this.data("id"));
                loadActivity();
            });


            btnAddActivity.click(insertActivity);
            btnUpdateActivity.click(updateActivity);
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
            gantt.templates.task_class = function (start, end, task) {
                switch (task.priority) {
                    case "1":
                        return "high";
                        break;
                    case "2":
                        return "medium";
                        break;
                    case "3":
                        return "low";
                        break;
                }
            };
            gantt.init("gantt_here");
            cargarMinuta();
            btnImprimirMinuta.click(imprimirMinuta);
            btnImprimirListaAsistencia.click(imprimirListaAsistencia);
            btnNuevaVersion.click(fnNuevaVersion);
            btnModNuevaMinuta.click(function () {
                $("#divNuevaVersion .close").click();
            });
            txtRevisa.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/SeguimientoAcuerdos/getRevisaList');
        }
        function fnSelRevisa(event, ui) {
            _revisa = ui.item.value;
            _revisaID = ui.item.id;
        }
        function fnSelNull(event, ui) {
            if (ui.item === null && $(this).val()!='') {
                _revisa = "";
                _revisaID = 0;
                txtRevisa.val("");
                AlertaGeneral("Alerta", "Solo puede seleccionar un usuario de la lista, si no aparece en la lista de autocompletado favor de solicitar al personal de TI");
            }
        }

        function updateAvanceActividad(id, columna) {
            $.post("/SeguimientoAcuerdos/updateAvanceActividad", { id, columna })
                .done(response => {
                    if (response.success) {
                        AlertaGeneral("Éxito", "Minuta actualizada correctamente.")
                        loadGantt(activitiesObj);
                    }
                    else {
                        AlertaGeneral("Error", "Ocurrió un error interno al intentar actualizar la minuta.")
                    }
                })
                .fail(() => AlertaGeneral("Error", "Ocurrió un error al intentar actualizar la minuta."))
                .always(() => $.unblockUI());
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

        function fnNuevaVersion() {
            if ($("#slMinutas").val() != '') {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/SeguimientoAcuerdos/getMinutaForVersion",
                    data: { id: Number($("#slMinutas").val()), userID: Number(usuarioID) },
                    success: function (response) {
                        //try {
                        var obj = response.obj;
                        activitiesObj = obj.actividades;
                        minutaID = obj.id;
                        txtProyecto.val(obj.proyecto);
                        txtTituloMinuta.val(obj.titulo);
                        txtFechaMinuta.val("");
                        var participantes = Enumerable.From(obj.participantes).Where(function (x) { return x.participanteID != obj.creadorID }).ToArray();
                        setAllParticipantes(participantes);
                        setAllActividades(obj.actividades);
                        summernote.summernote('code', obj.descripcion);
                        var creador = Enumerable.From(obj.participantes).Where(function (x) { return x.participanteID === obj.creadorID }).Select(function (x) { return x }).FirstOrDefault();
                        minuteOwner(creador.participante, creador.participanteID);
                        txtLugar.val("");
                        slHoraInicio.val("");
                        slHoraFin.val("");
                        loadGantt(activitiesObj);
                        if (response.owner === false) {
                            modoLectura();
                            _Modalidad = "modoLectura";
                        } else {
                            btnIniciar.hide();
                            btnImprimirMinuta.show();
                            btnImprimirListaAsistencia.show();
                            btnEnviarCorreo.show();
                            btnGuardar.show();
                            btnGuardarContinuar.show();
                            btnNuevo.show();
                            _Modalidad = "modoEditable";
                        }
                        desbloqueo.unblock();
                        //}
                        //catch(err) {

                        //    nuevaMinuta();
                        //}
                        slMCorreos.fillCombo('/SeguimientoAcuerdos/FillComboUsuarios', { minutaID: minutaID }, false, "Todos");
                        convertToMultiselect("#slMCorreos");
                        slUsuarios.fillCombo('/SeguimientoAcuerdos/FillComboParticipantes', { minutaID: minutaID });
                        nuevaVersion = true;
                        $("#divNuevaVersion .close").click();
                        $.unblockUI();
                    },
                    error: function (ex) {
                        nuevaMinuta();
                        $.unblockUI();
                    }
                });
            }
            else {
                AlertaGeneral("Alerta", "¡Debe seleccionar una minuta para continuar!");
            }
        }
        function fnLoadActividadesByParticipante() {
            var _this = $(this);
            if (_this.val() != '') {
                //var data = Enumerable.From(activitiesObj).Where(function (x) { return x.responsablesID == _this.val() }).ToArray();
                var data = Enumerable.From(activitiesObj).Where(function (x) { return Enumerable.From(x.responsablesID).Contains(Number(_this.val())) }).ToArray();


                setAllActividades(data);
            }
            else {
                setAllActividades(activitiesObj);
            }
        }
        function fnOpenEnviarCorreo() {
            slMCorreos.fillCombo('/SeguimientoAcuerdos/FillComboUsuarios', { minutaID: minutaID }, false, "Todos");
            convertToMultiselect("#slMCorreos");
            modalCorreos.modal("show");
        }
        function fnEnviarCorreos() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $("#modalCorreos .close").click();
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Reportes/Vista.aspx?idReporte=4&inMemory=true&minuta=" + minutaID,
                success: function (response) {
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: "/Reportes/Vista.aspx?idReporte=5&inMemory=true&minuta=" + minutaID,
                        success: function (response) {
                            if (response != null) {
                                $.ajax({
                                    datatype: "json",
                                    type: "POST",
                                    url: "/SeguimientoAcuerdos/enviarCorreos",
                                    data: { minutaID: minutaID, usuarios: getValoresMultiples("#slMCorreos") },
                                    success: function (response) {
                                        if (response.success === true) {
                                            $.unblockUI();
                                            AlertaGeneral("Confirmación", "Correos enviados correctamente");
                                        }
                                        else {
                                            $.unblockUI();
                                            AlertaGeneral("Alerta", "¡Ocurrio un problema al enviar a los siguientes usuarios!<br/>" + response.obj);
                                        }
                                    },
                                    error: function () {
                                        $.unblockUI();
                                    }
                                });
                            }
                            else {
                                AlertaGeneral("Alerta", "¡Ocurrio un problema al convertir la minuta a PDF para ser enviada!");
                            }
                        },
                        error: function () {
                            $.unblockUI();
                        }
                    });
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }
        function imprimirMinuta(e) {
            verReporte(4, "minuta=" + minutaID, "H");
            e.preventDefault();
        }
        function imprimirListaAsistencia(e) {
            verReporte(5, "minuta=" + minutaID, "V");
            e.preventDefault();
        }
        function iniciarMinuta() {
            if (validateIniciar()) {
                var obj = {};
                obj.id = minutaID;
                obj.proyecto = txtProyecto.val();
                obj.titulo = txtTituloMinuta.val();
                obj.fecha = txtFechaMinuta.val();
                obj.fechaInicio = txtFechaMinuta.val();
                obj.fechaCompromiso = txtFechaMinuta.val();
                obj.descripcion = summernote.summernote('code');
                obj.creadorID = usuarioID;
                obj.lugar = txtLugar.val();
                obj.horaInicio = slHoraInicio.val();
                obj.horaFin = slHoraFin.val();
                obj.actividades = new Array();
                obj.participantes = new Array();
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/SeguimientoAcuerdos/guardarMinuta",
                    data: { obj: obj, nuevaVersion: nuevaVersion },
                    success: function (response) {
                        minutaID = Number(response.id);
                        btnIniciar.hide();
                        btnGuardar.show();
                        btnGuardarContinuar.show();
                        btnNuevo.show();
                        nuevaVersion = false;
                        minuteOwner(usuarioNombre, usuarioID);
                        btnImprimirMinuta.show();
                        btnImprimirListaAsistencia.show();
                        btnEnviarCorreo.show();

                        btnImprimirMinuta.prop("disabled", true);
                        btnImprimirListaAsistencia.prop("disabled", true);
                        btnEnviarCorreo.prop("disabled", true);
                        desbloqueo.unblock();
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }
        function guardarMinuta() {
            if (validateIniciar()) {
                if (getActivitiesCount() > 0) {
                    var obj = {};
                    obj.id = minutaID;
                    obj.proyecto = txtProyecto.val();
                    obj.titulo = txtTituloMinuta.val();
                    obj.fecha = txtFechaMinuta.val();
                    obj.descripcion = summernote.summernote('code');
                    obj.creadorID = usuarioID;
                    obj.lugar = txtLugar.val();
                    obj.horaInicio = slHoraInicio.val();
                    obj.horaFin = slHoraFin.val();
                    obj.actividades = new Array();
                    obj.participantes = new Array();
                    obj.nuevaVersion = nuevaVersion;
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: "/SeguimientoAcuerdos/guardarMinuta",
                        data: { obj: obj, nuevaVersion: nuevaVersion },
                        success: function (response) {
                            minutaID = Number(response.id);
                            btnIniciar.hide();
                            btnGuardar.show();
                            btnGuardarContinuar.show();
                            btnNuevo.show();
                            nuevaVersion = false;
                            ConfirmacionGeneral("Confirmación", "¡Minuta guardada correctamente!", "bg-green");
                            setInterval(redirectDashboard, 2000);
                        },
                        error: function () {

                        }
                    });
                }
                else {
                    AlertaGeneral("Alerta", "¡Se debe agregar almenos una actividad antes de poder guardar!");
                }
            }
            else {
                AlertaGeneral("Alerta", "¡Todos los campos son obligatorios!");
            }
        }
        function guardarMinutaContinuar() {
            if (validateIniciar()) {
                if (getActivitiesCount() > 0) {
                    var obj = {};
                    obj.id = minutaID;
                    obj.proyecto = txtProyecto.val();
                    obj.titulo = txtTituloMinuta.val();
                    obj.fecha = txtFechaMinuta.val();
                    obj.descripcion = summernote.summernote('code');
                    obj.creadorID = usuarioID;
                    obj.lugar = txtLugar.val();
                    obj.horaInicio = slHoraInicio.val();
                    obj.horaFin = slHoraFin.val();
                    obj.actividades = new Array();
                    obj.participantes = new Array();
                    obj.nuevaVersion = nuevaVersion;
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: "/SeguimientoAcuerdos/guardarMinuta",
                        data: { obj: obj, nuevaVersion: nuevaVersion },
                        success: function (response) {
                            minutaID = Number(response.id);
                            btnIniciar.hide();
                            btnGuardar.show();
                            btnGuardarContinuar.show();
                            btnNuevo.show();
                            nuevaVersion = false;
                            btnImprimirMinuta.prop("disabled", false);
                            btnImprimirListaAsistencia.prop("disabled", false);
                            btnEnviarCorreo.prop("disabled", false);
                            ConfirmacionGeneral("Confirmación", "¡Minuta guardada correctamente!", "bg-green");
                        },
                        error: function () {

                        }
                    });
                }
                else {
                    AlertaGeneral("Alerta", "¡Se debe agregar almenos una actividad antes de poder guardar!");
                }
            }
            else {
                AlertaGeneral("Alerta", "¡Todos los campos son obligatorios!");
            }
        }
        function getActivitiesCount() {
            var cantidad = 0;
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/getActivitiesCount",
                data: { id: minutaID },
                async: false,
                success: function (response) {
                    cantidad = response.cantidad;
                }
            });
            return cantidad;
        }
        //Participantes
        function hoverUserFromProject(e) {
            var _this = $(this);

            if (e.type === "mouseenter") {
                _this.parent().find(".userFill").css({ 'background-color': '#d3d3d3' });
                _this.parent().find(".userDelete").css({ 'background-color': '#d3d3d3' });
            }
            else if (e.type === "mouseleave") {
                _this.parent().find(".userFill").css({ 'background-color': '#e0e0e0' });
                _this.parent().find(".userDelete").css({ 'background-color': '#e0e0e0' });
            }
        }
        function hoverdelUserFromProject(e) {
            var _this = $(this);

            if (e.type === "mouseenter") {
                _this.parent().find(".userDelete").css({ 'background-color': '#bdbdbd' });
            }
            else if (e.type === "mouseleave") {
                _this.parent().find(".userDelete").css({ 'background-color': '#e0e0e0' });
            }
        }
        function getUserFromProject() {

        }
        function delUserFromProject() {
            var _this = $(this);
            var u = _this.parent().find(".userComponent");
            var obj = {};
            obj.minutaID = minutaID;
            obj.participanteID = $(u).data("userid");
            obj.participante = $(u).data("user");
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/eliminarParticipante",
                data: { obj: obj },
                success: function (response) {
                    _this.parent().remove();
                    slMCorreos.fillCombo('/SeguimientoAcuerdos/FillComboUsuarios', { minutaID: minutaID }, false, "Todos");
                    convertToMultiselect("#slMCorreos");
                    slUsuarios.fillCombo('/SeguimientoAcuerdos/FillComboParticipantes', { minutaID: minutaID });
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function newUserFromProjectInit() {
            newUser.focus();
            newUser.autocomplete("search");
        }
        function newUserSearch(event, ui) {
            var obj = {};
            obj.minutaID = minutaID;
            obj.participanteID = ui.item.id;
            obj.participante = ui.item.value;
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/guardarParticipante",
                data: { obj: obj },
                success: function (response) {
                    var html = '<div class="divUser">';
                    html += '    <div class="userContainer">';
                    html += '        <span class="userFill">&nbsp;</span>';
                    html += '        <span class="userComponent" data-user="' + ui.item.value + '" data-userid="' + ui.item.id + '">';
                    html += ui.item.value;
                    html += '        </span>';
                    html += '        <button type="button" class="userDelete">&nbsp;X</button>';
                    html += '    </div>';
                    html += '</div>';
                    $(html).insertBefore(newUser);
                    fillComboPaticipantes();
                    newUser.focus();

                    ui.item.value = "";  // it will clear field 
                    newUser.autocomplete('close').val('');
                },
                error: function () {
                    $.unblockUI();
                }
            });

            return false;
        }
        //Responsables
        function hoverResponsablesFromProject(e) {
            var _this = $(this);

            if (e.type === "mouseenter") {
                _this.parent().find(".ResponsablesFill").css({ 'background-color': '#d3d3d3' });
                _this.parent().find(".ResponsablesDelete").css({ 'background-color': '#d3d3d3' });
            }
            else if (e.type === "mouseleave") {
                _this.parent().find(".ResponsablesFill").css({ 'background-color': '#e0e0e0' });
                _this.parent().find(".ResponsablesDelete").css({ 'background-color': '#e0e0e0' });
            }
        }
        function hoverdelResponsablesFromProject(e) {
            var _this = $(this);

            if (e.type === "mouseenter") {
                _this.parent().find(".ResponsablesDelete").css({ 'background-color': '#bdbdbd' });
            }
            else if (e.type === "mouseleave") {
                _this.parent().find(".ResponsablesDelete").css({ 'background-color': '#e0e0e0' });
            }
        }
        function getResponsablesFromProject() {
            var obj = new Array();
            ResponsablesInActivity.find(".ResponsablesComponent").each(function (i, e) {
                var o = {};
                o.minutaID = minutaID;
                o.actividadID = currentActivity;
                o.usuarioID = $(e).data("userid");
                o.usuario = $(e).data("user");
                obj.push(o);
            });
            return obj;
        }
        function getResponsablesIDFromProject() {
            var obj = new Array();
            ResponsablesInActivity.find(".ResponsablesComponent").each(function (i, e) {
                obj.push($(e).data("userid"));
            });
            return obj;
        }
        function delResponsablesFromProject() {
            var _this = $(this);
            var u = _this.parent().find(".ResponsablesComponent");
            var obj = {};
            obj.id = 0;
            obj.minutaID = minutaID;
            obj.actividadID = currentActivity;
            obj.usuarioID = $(u).data("userid");
            obj.usuario = $(u).data("user");
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/eliminarResponsable",
                data: { obj: obj },
                success: function (response) {
                    _this.parent().remove();
                },
                error: function () {
                    _this.parent().remove();
                    $.unblockUI();
                }
            });
        }
        function newResponsablesFromProjectInit() {
            newResponsables.focus();
            newResponsables.autocomplete("search");
        }
        function newResponsablesSearch(event, ui) {
            var obj = {};
            obj.minutaID = minutaID;
            obj.actividadID = currentActivity;
            obj.usuarioID = ui.item.id;
            obj.usuario = ui.item.value;
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/guardarResponsable",
                data: { obj: obj },
                success: function (response) {
                    var html = '<div class="divResponsables">';
                    html += '    <div class="userContainer">';
                    html += '        <span class="ResponsablesFill">&nbsp;</span>';
                    html += '        <span class="ResponsablesComponent" data-user="' + ui.item.value + '" data-userid="' + ui.item.id + '">';
                    html += ui.item.value;
                    html += '        </span>';
                    html += '        <button type="button" class="ResponsablesDelete">&nbsp;X</button>';
                    html += '    </div>';
                    html += '</div>';
                    $(html).insertBefore(newResponsables);
                    newResponsables.focus();

                    ui.item.value = "";  // it will clear field 
                    newResponsables.autocomplete('close').val('');
                },
                error: function () {
                    $.unblockUI();
                }
            });

            return false;
        }
        function setAllResponsables(actividadID) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/getResponsablesPorActividad",
                data: { id: actividadID },
                async: false,
                success: function (response) {
                    var data = response.obj;
                    $.each(data, function (i, e) {
                        var html = '<div class="divResponsables">';
                        html += '    <div class="userContainer">';
                        html += '        <span class="ResponsablesFill">&nbsp;</span>';
                        html += '        <span class="ResponsablesComponent" data-user="' + e.usuario + '" data-userid="' + e.usuarioID + '">';
                        html += e.usuario;
                        html += '        </span>';
                        html += '        <button type="button" class="ResponsablesDelete">&nbsp;X</button>';
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
        //Interesados
        function hoverInterestedFromProject(e) {
            var _this = $(this);

            if (e.type === "mouseenter") {
                _this.parent().find(".interestedFill").css({ 'background-color': '#d3d3d3' });
                _this.parent().find(".interestedDelete").css({ 'background-color': '#d3d3d3' });
            }
            else if (e.type === "mouseleave") {
                _this.parent().find(".interestedFill").css({ 'background-color': '#e0e0e0' });
                _this.parent().find(".interestedDelete").css({ 'background-color': '#e0e0e0' });
            }
        }
        function hoverdelInterestedFromProject(e) {
            var _this = $(this);

            if (e.type === "mouseenter") {
                _this.parent().find(".interestedDelete").css({ 'background-color': '#bdbdbd' });
            }
            else if (e.type === "mouseleave") {
                _this.parent().find(".interestedDelete").css({ 'background-color': '#e0e0e0' });
            }
        }
        function getInterestedFromProject() {
            var obj = new Array();
            interestedInActivity.find(".interestedComponent").each(function (i, e) {
                var o = {};
                o.minutaID = minutaID;
                o.actividadID = currentActivity;
                o.interesadoID = $(e).data("userid");
                o.interesado = $(e).data("user");
                o.usuarioText = $(e).data("user");
                obj.push(o);
            });
            return obj;
        }
        function delInterestedFromProject() {
            var _this = $(this);
            var u = _this.parent().find(".interestedComponent");
            var obj = {};
            obj.id = 0;
            obj.minutaID = minutaID;
            obj.actividadID = currentActivity;
            obj.interesadoID = $(u).data("userid");
            obj.interesado = $(u).data("user");
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/eliminarInteresado",
                data: { obj: obj },
                success: function (response) {
                    _this.parent().remove();
                },
                error: function () {
                    _this.parent().remove();
                    $.unblockUI();
                }
            });
        }
        function newInterestedFromProjectInit() {
            newInterested.focus();
            newInterested.autocomplete("search");
        }
        function newInterestedSearch(event, ui) {
            var obj = {};
            obj.minutaID = minutaID;
            obj.actividadID = currentActivity;
            obj.interesadoID = ui.item.id;
            obj.interesado = ui.item.value;
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/guardarInteresado",
                data: { obj: obj },
                success: function (response) {
                    var html = '<div class="divInterested">';
                    html += '    <div class="userContainer">';
                    html += '        <span class="interestedFill">&nbsp;</span>';
                    html += '        <span class="interestedComponent" data-user="' + ui.item.value + '" data-userid="' + ui.item.id + '">';
                    html += ui.item.value;
                    html += '        </span>';
                    html += '        <button type="button" class="interestedDelete">&nbsp;X</button>';
                    html += '    </div>';
                    html += '</div>';
                    $(html).insertBefore(newInterested);
                    newInterested.focus();

                    ui.item.value = "";  // it will clear field 
                    newInterested.autocomplete('close').val('');
                },
                error: function () {
                    $.unblockUI();
                }
            });

            return false;
        }
        function setAllInteresados(actividadID) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/getInteresadosPorActividad",
                data: { id: actividadID },
                async: false,
                success: function (response) {
                    var data = response.obj;
                    $.each(data, function (i, e) {
                        var html = '<div class="divInterested">';
                        html += '    <div class="userContainer">';
                        html += '        <span class="interestedFill">&nbsp;</span>';
                        html += '        <span class="interestedComponent" data-user="' + e.interesado + '" data-userid="' + e.interesadoID + '">';
                        html += e.interesado;
                        html += '        </span>';
                        html += '        <button type="button" class="interestedDelete">&nbsp;X</button>';
                        html += '    </div>';
                        html += '</div>';
                        $(html).insertBefore(newInterested);
                    });
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }

        function openModalActividad() {
            currentActivity = 0;
            ResponsablesInActivity.find(".divResponsables").remove();
            interestedInActivity.find(".divInterested").remove();
            btnAddActivity.show();
            btnUpdateActivity.hide();
            txtMActividad.val("");
            txtMDescripcionActividad.val("");
            fillComboPaticipantes();
            slMPrioridad.val(3);
            initDates();
            modalActividad.modal("show");
        }
        function fillComboPaticipantes() {
            slMResponsable.fillCombo('/SeguimientoAcuerdos/getParticipantes', { id: minutaID });
            slMCorreos.fillCombo('/SeguimientoAcuerdos/FillComboUsuarios', { minutaID: minutaID }, false, "Todos");
            convertToMultiselect("#slMCorreos");
            slUsuarios.fillCombo('/SeguimientoAcuerdos/FillComboParticipantes', { minutaID: minutaID });
        }
        function initDates() {
            txtFechaInicio.datepicker().datepicker("setDate", new Date());
            txtFechaCompromiso.datepicker().datepicker("setDate", new Date());
        }
        function getNewActivity() {
            var r = {};
            r.id = 0;
            r.minutaID = minutaID;
            r.minuta = txtTituloMinuta.val();
            r.columna = 0;
            if (activitiesObj.length == 0) {
                r.Orden = 1;
            }
            else {
                r.Orden = Enumerable.From(activitiesObj).Where(function (x) { return x.columna == 0 }).Count() + 1;
            }
            r.tipo = 'new';
            r.actividad = txtMActividad.val();
            r.descripcion = txtMDescripcionActividad.val();
            r.responsableID = slMResponsable.val();
            r.responsablesID = new Array();
            r.responsablesID = getResponsablesIDFromProject();
            r.responsable = $("#slMResponsable option:selected").text();
            r.fechaInicio = txtFechaInicio.val();
            r.fechaCompromiso = txtFechaCompromiso.val();

            r.prioridad = Number(slMPrioridad.val());
            r.comentariosCount = 0;
            r.comentarios = new Array();
            r.enVersion = true;
            r.revisaID = _revisaID;
            r.revisa = _revisa;
            return r;
        }
        function getUpdatedActivity() {
            var data = Enumerable.From(activitiesObj).Where(function (x) { return x.id === currentActivity }).Select(function (x) { return x }).FirstOrDefault();
            data.responsableID = slMResponsable.val();
            data.actividad = txtMActividad.val();
            data.descripcion = txtMDescripcionActividad.val();
            data.responsable = $("#slMResponsable option:selected").text();
            data.fechaInicio = txtFechaInicio.val();
            data.fechaCompromiso = txtFechaCompromiso.val();
            data.prioridad = Number(slMPrioridad.val());
            data.enVersion = true;
            data.revisaID = _revisaID;
            data.revisa = _revisa;
            data.responsablesID = new Array();
            data.responsablesID = getResponsablesIDFromProject();
            return data;
        }
        function loadActivity() {
            ResponsablesInActivity.find(".divResponsables").remove();
            interestedInActivity.find(".divInterested").remove();
            fillComboPaticipantes();
            btnAddActivity.hide();
            btnUpdateActivity.show();
            var data = Enumerable.From(activitiesObj).Where(function (x) { return x.id === currentActivity }).Select(function (x) { return x }).FirstOrDefault();
            //slMResponsable.val(data.responsableID);
            txtMActividad.val(data.actividad);
            txtRevisa.val(data.revisa);
            txtRevisa.data("revisaid", data.revisaID);
            _revisa = data.revisa;
            _revisaID = data.revisaID;
            txtMDescripcionActividad.val(data.descripcion);
            txtFechaInicio.val(data.fechaInicio);
            txtFechaCompromiso.val(data.fechaCompromiso);
            slMPrioridad.val(data.prioridad);
            setAllResponsables(currentActivity);
            setAllInteresados(currentActivity);
            if (_Modalidad == "modoLectura") {
                btnDisplayResponsables.hide();
                $(".ResponsablesDelete").prop("disabled", true);
                $(".interestedDelete").prop("disabled", true);
            }
            else {

            }

            modalActividad.modal("show");
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
        function insertActivity(e) {
            if (validateActividad()) {
                if (getResponsablesFromProject().length > 0) {
                    var obj = getNewActivity();
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: "/SeguimientoAcuerdos/guardarActividad",
                        data: { obj: obj },
                        success: function (response) {
                            obj.id = Number(response.id);
                            currentActivity = obj.id;
                            var html = ' <div id="a' + obj.id + '" class="portlet" data-columna="' + obj.columna + '" data-id="' + obj.id + '" data-responsableID="' + obj.responsableID + '" data-orden="' + obj.Orden + '" data-tipo="' + obj.tipo + '">';
                            html += '   <div class="portlet-header success">';
                            html += '       <div class="acttividadElementoMain" title="Titulo de minuta">' + obj.minuta + '</div>';
                            html += '       <div class="acttividadElemento" title="Descripción de actividad">' + obj.actividad + '</div>';
                            html += '       <div class="acttividadElemento" title="Fecha compromiso">' + obj.fechaCompromiso + '</div>';
                            html += '       <div class="componentePrioridad prioridad' + obj.prioridad + '" title="' + ((obj.prioridad == 1) ? "Prioridad Alta" : (obj.prioridad == 2) ? "Prioridad Media" : "Prioridad Baja") + '"></div>';
                            html += '   </div>';
                            html += '   <div class="portlet-content">';
                            html += obj.descripcion;
                            html += '   </div>';
                            html += '   <div class="panel-footer">';
                            html += '       <a href="#" class="openComentarios" data-id="' + obj.id + '" >(<span class="comentariesCount">' + obj.comentariosCount + '</span> comentarios) - Ver</a>';
                            html += '   </div>';
                            html += '</div>';

                            cP0.append(html);
                            activitiesObj.push(obj);
                            loadGantt(activitiesObj);
                            $('#a' + obj.id)
                                .addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
                                .find(".portlet-header")
                                .addClass("ui-widget-header ui-corner-all")
                                .prepend("<span class='ui-icon ui-icon-minusthick portlet-toggle'></span>");

                            $('#a' + obj.id).find(".portlet-toggle").on("click", function () {
                                var icon = $(this);
                                icon.toggleClass("ui-icon-minusthick ui-icon-plusthick");
                                icon.closest(".portlet").find(".portlet-content").toggle();
                                icon.closest(".portlet").find(".panel-footer").toggle();
                            });
                            $('#a' + obj.id).find(".openComentarios").on("click", function (event) {
                                event.preventDefault();
                                currentActivity = Number($(this).data("id"));
                                var data = Enumerable.From(activitiesObj).Where(function (x) { return x.id == currentActivity }).Select(function (x) { return x }).FirstOrDefault();
                                setComentarios(data.comentarios);
                                txtComentarios.val("");
                                divVerComentario.modal("show");
                            });
                            $('#a' + obj.id).find(".portlet-toggle").trigger("click");
                            $.ajax({
                                datatype: "json",
                                type: "POST",
                                url: "/SeguimientoAcuerdos/guardarResponsables",
                                data: { obj: getResponsablesFromProject() },
                                async: false,
                                success: function (response) {
                                    interestedInActivity.find(".divResponsables").remove();
                                },
                                error: function () {

                                }
                            });
                            $.ajax({
                                datatype: "json",
                                type: "POST",
                                url: "/SeguimientoAcuerdos/guardarInteresados",
                                data: { obj: getInterestedFromProject() },
                                success: function (response) {
                                    interestedInActivity.find(".divInterested").remove();
                                    $("#modalActividad .close").click();
                                },
                                error: function () {

                                }
                            });


                        },
                        error: function () {
                            $.unblockUI();
                        }
                    });
                }
                else {
                    AlertaGeneral("Alerta", "¡Debe agregar almenos un responsable!");
                }
            }
            else {
                e.preventDefault()
            }
        }
        function updateActivity(e) {
            if (validateActividad()) {
                var obj = getUpdatedActivity();
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/SeguimientoAcuerdos/guardarActividad",
                    data: { obj: obj },
                    success: function (response) {
                        var html = ' <div id="a' + obj.id + '" class="portlet" data-columna="' + obj.columna + '" data-id="' + obj.id + '" data-responsableID="' + obj.responsableID + '" data-orden="' + obj.Orden + '" data-tipo="' + obj.tipo + '">';
                        html += '   <div class="portlet-header success">';
                        html += '       <div class="acttividadElementoMain" title="Titulo de minuta">' + obj.minuta + '</div>';
                        html += '       <div class="acttividadElemento" title="Descripción de actividad">' + obj.actividad + '</div>';
                        html += '       <div class="acttividadElemento" title="Fecha compromiso">' + obj.fechaCompromiso + '</div>';
                        html += '       <div class="componentePrioridad prioridad' + obj.prioridad + '" title="' + ((obj.prioridad == 1) ? "Prioridad Alta" : (obj.prioridad == 2) ? "Prioridad Media" : "Prioridad Baja") + '"></div>';
                        html += '   </div>';
                        html += '   <div class="portlet-content">';
                        html += obj.descripcion;
                        html += '   </div>';
                        html += '   <div class="panel-footer">';
                        html += '       <a href="#" class="openComentarios" data-id="' + obj.id + '" >(<span class="comentariesCount">' + obj.comentariosCount + '</span> comentarios) - Ver</a>';
                        html += '   </div>';
                        html += '</div>';
                        loadGantt(activitiesObj);
                        $('#a' + obj.id).replaceWith(html);
                        $('#a' + obj.id)
                            .addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
                            .find(".portlet-header")
                            .addClass("ui-widget-header ui-corner-all")
                            .prepend("<span class='ui-icon ui-icon-minusthick portlet-toggle'></span>");

                        $('#a' + obj.id).find(".portlet-toggle").on("click", function () {
                            var icon = $(this);
                            icon.toggleClass("ui-icon-minusthick ui-icon-plusthick");
                            icon.closest(".portlet").find(".portlet-content").toggle();
                            icon.closest(".portlet").find(".panel-footer").toggle();
                        });
                        $('#a' + obj.id).find(".openComentarios").on("click", function (event) {
                            event.preventDefault();
                            currentActivity = Number($(this).data("id"));
                            var data = Enumerable.From(activitiesObj).Where(function (x) { return x.id === currentActivity }).Select(function (x) { return x }).FirstOrDefault();
                            setComentarios(data.comentarios);
                            txtComentarios.val("");
                            divVerComentario.modal("show");
                        });
                        $('#a' + obj.id).find(".portlet-toggle").trigger("click");
                        $("#modalActividad .close").click();
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            } else {
                e.preventDefault()
            }
        }
        function insertCommentary(e) {
            if (validateComentario()) {
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
            } else {
                e.preventDefault()
            }
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
        function loadGantt(activitiesObjC) {
            gantt.clearAll();
            var tasks = {};
            tasks.data = new Array();
            $.each(activitiesObjC, function (i, e) {
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
        function modoLectura() {
            $(".userDelete").prop("disabled", true);
            txtProyecto.attr("disabled", true);
            txtTituloMinuta.attr("disabled", true);
            txtFechaMinuta.attr("disabled", true);
            txtLugar.attr("disabled", true);
            slHoraInicio.attr("disabled", true);
            slHoraFin.attr("disabled", true);
            btnGuardar.hide();
            btnGuardarContinuar.hide();
            btnNuevo.hide();
            btnDisplayUsers.hide();
            newUser.hide();
            btnActividad.hide();
            summernote.summernote('disable');
            btnAddActivity.hide();
            btnUpdateActivity.hide();
            txtMActividad.attr("disabled", true);
            txtMDescripcionActividad.attr("disabled", true);
            slMResponsable.attr("disabled", true);
            slMPrioridad.attr("disabled", true);
            txtFechaInicio.attr("disabled", true);
            txtFechaCompromiso.attr("disabled", true);
            //btnAddComentario.hide();
            txtComentarios.attr("disabled", false);
            newResponsables.hide();
            ResponsablesInActivity.attr("disabled", true);
            btnDisplayInterested.hide();
            interestedInActivity.attr("disabled", true);
            txtRevisa.attr("disabled", true);
            //$(".ResponsablesDelete").hide();
            //$(".interestedDelete").hide();
            //btnDisplayResponsables.hide();
            //$(".ResponsablesDelete").prop("disabled", true);
            //$(".interestedDelete").prop("disabled", true);
            btnIniciar.hide();
        }
        function modoEditable() {
            txtProyecto.attr("disabled", false);
            txtTituloMinuta.attr("disabled", false);
            txtFechaMinuta.attr("disabled", false);
            txtLugar.attr("disabled", false);
            slHoraInicio.attr("disabled", false);
            slHoraFin.attr("disabled", false);
            btnGuardar.show();
            btnGuardarContinuar.show();
            btnDisplayUsers.show();
            newUser.show();
            btnActividad.show();
            summernote.summernote('enable');
            btnAddActivity.show();
            btnUpdateActivity.show();
            txtMActividad.attr("disabled", false);
            txtMDescripcionActividad.attr("disabled", false);
            slMResponsable.attr("disabled", false);
            slMPrioridad.attr("disabled", false);
            txtFechaInicio.attr("disabled", false);
            txtFechaCompromiso.attr("disabled", false);
            btnAddComentario.show();
            txtComentarios.attr("disabled", false);
            newResponsables.show();
            ResponsablesInActivity.attr("disabled", false);
            btnDisplayInterested.show();
            interestedInActivity.attr("disabled", false);
            txtRevisa.attr("disabled", false);
            $(".ResponsablesDelete").show();
            $(".interestedDelete").show();
            btnDisplayResponsables.show();
            $(".ResponsablesDelete").prop("disabled", false);
            $(".interestedDelete").prop("disabled", false);
        }

        function minuteOwner(nombre, id) {
            txtLider.val(nombre);
            txtLider.data("id", id);
            txtLider.data("lider", nombre);
        }
        function nuevaMinuta() {
            window.location.href = window.location.href.replace(window.location.search, "");
        }
        function redirectDashboard() {
            var d = new Date();
            window.location.href = "/SeguimientoAcuerdos/Dashboard/?id=6";
        }
        function setAllParticipantes(data) {
            $.each(data, function (i, e) {
                var html = '<div class="divUser">';
                html += '    <div class="userContainer">';
                html += '        <span class="userFill">&nbsp;</span>';
                html += '        <span class="userComponent" data-user="' + e.participante + '" data-userid="' + e.participanteID + '">';
                html += e.participante;
                html += '        </span>';
                html += '        <button type="button" class="userDelete">&nbsp;X</button>';
                html += '    </div>';
                html += '</div>';
                $(html).insertBefore(newUser);
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
            loadGantt(data);
        }
        function cargarMinuta() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var minuta = $.urlParam('minuta');
            if (minuta != null) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/SeguimientoAcuerdos/getMinuta",
                    data: { id: Number(minuta), userID: Number(usuarioID) },
                    success: function (response) {
                        //try {
                        var obj = response.obj;
                        activitiesObj = obj.actividades;
                        minutaID = obj.id;
                        txtProyecto.val(obj.proyecto);
                        txtTituloMinuta.val(obj.titulo);
                        txtFechaMinuta.val(obj.fecha);
                        var participantes = Enumerable.From(obj.participantes).Where(function (x) { return x.participanteID != obj.creadorID }).ToArray();
                        setAllParticipantes(participantes);
                        setAllActividades(obj.actividades);
                        summernote.summernote('code', obj.descripcion);
                        var creador = Enumerable.From(obj.participantes).Where(function (x) { return x.participanteID === obj.creadorID }).Select(function (x) { return x }).FirstOrDefault();
                        minuteOwner(creador.participante, creador.participanteID);
                        txtLugar.val(obj.lugar);
                        slHoraInicio.val(obj.horaInicio);
                        slHoraFin.val(obj.horaFin);
                        loadGantt(activitiesObj);
                        if (response.owner === false) {
                            _Modalidad = 'modoLectura';
                            modoLectura();
                        } else {
                            _Modalidad = 'modoEditable';
                            btnIniciar.hide();
                            btnImprimirMinuta.show();
                            btnImprimirListaAsistencia.show();
                            btnEnviarCorreo.show();
                            btnGuardar.show();
                            btnGuardarContinuar.show();
                            btnNuevo.show();
                        }
                        desbloqueo.unblock();
                        //}
                        //catch(err) {

                        //    nuevaMinuta();
                        //}
                        slMCorreos.fillCombo('/SeguimientoAcuerdos/FillComboUsuarios', { minutaID: minutaID }, false, "Todos");
                        convertToMultiselect("#slMCorreos");
                        slUsuarios.fillCombo('/SeguimientoAcuerdos/FillComboParticipantes', { minutaID: minutaID });
                        $.unblockUI();
                    },
                    error: function (ex) {
                        nuevaMinuta();
                        $.unblockUI();
                    }
                });
            }
            else {
                if (gNuevaVersion == 'True') {
                    $("#slMinutas").fillCombo('/SeguimientoAcuerdos/getMinutas', { estatus: true }, false);
                    $("#divNuevaVersion").modal("show");
                }
                $.unblockUI();
            }

        }
        function addRequired() {
            txtProyecto.addClass('required');
            txtTituloMinuta.addClass('required');
            txtLugar.addClass('required');
            slHoraInicio.addClass('required');
            slHoraFin.addClass('required');
            txtFechaMinuta.addClass('required');
            txtMActividad.addClass('required');
            txtMDescripcionActividad.addClass('required');
            slMResponsable.addClass('required');
            slMPrioridad.addClass('required');
            txtFechaInicio.addClass('required');
            txtFechaCompromiso.addClass('required');
            txtComentarios.addClass('required');
        }
        function validateIniciar() {
            var state = true;
            if (!validarCampo(txtProyecto)) { state = false; }
            if (!validarCampo(txtTituloMinuta)) { state = false; }
            if (!validarCampo(txtFechaMinuta)) { state = false; }
            if (!validarCampo(txtLugar)) { state = false; }
            if (!validarCampo(slHoraInicio)) { state = false; }
            if (!validarCampo(slHoraFin)) { state = false; }
            return state;
        }
        function validateActividad() {
            var state = true;
            if (!validarCampo(txtMActividad)) { state = false; }
            if (!validarCampo(txtMDescripcionActividad)) { state = false; }
            //if (!validarCampo(slMResponsable)) { state = false; }
            if (!validarCampo(txtRevisa)) { state = false; }
            if (!validarCampo(slMPrioridad)) { state = false; }
            if (!validarCampo(txtFechaInicio)) { state = false; }
            if (!validarCampo(txtFechaCompromiso)) { state = false; }
            return state;
        }
        function validateComentario() {
            var state = true;
            if (!validarCampo(txtComentarios)) { state = false; }
            return state;
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
        function verReporte(idReporte, parametros, orientacion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&" + parametros;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        function validarCampo(_this) {
            var r = false;
            if (_this.val() == '') {
                if (!_this.hasClass("errorClass")) {
                    _this.addClass("errorClass")
                }
                r = false;
            }
            else {
                if (_this.hasClass("errorClass")) {
                    _this.removeClass("errorClass")
                }
                r = true;
            }
            return r;
        }
        init();
    };
    $(document).ready(function () {
        sigoplan.seguimientoacuerdos.seguimiento = new seguimiento();
    });
})();
