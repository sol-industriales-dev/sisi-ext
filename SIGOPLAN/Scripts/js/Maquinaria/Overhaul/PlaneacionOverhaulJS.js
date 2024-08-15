(function () {

    $.namespace('maquinaria.overhaul.planeacionoverhaul');

    planeacionoverhaul = function () {

        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        esPlaneacionNueva = $('#esPlaneacionNueva');

        calendar = $("#calendar"),
        modalGuardarCalendario = $("#modalGuardarCalendario"),
        cboGrupoMaquina = $("#cboGrupoMaquina"),
        cboModeloMaquina = $("#cboModeloMaquina"),
        cboObra = $("#cboObra"),
        txtSubconjunto = $("#txtSubconjunto"),
        btnGuardarCalendario = $("#btnGuardarCalendario"),
        btnGuardarNuevoCalendario = $("#btnGuardarNuevoCalendario"),
        btnModalGuardar = $("#btnModalGuardar"),
        txtNombreCalendario = $("#txtNombreCalendario"),
        cboAnio = $("#cboAnio"),
        cboCalendarioGuardado = $("#cboCalendarioGuardado"),
        cboCalendarioTerminado = $("#cboCalendarioTerminado"),
        btnRecargarCalendario = $("#btnRecargarCalendario"),
        btnRecargarTerminado = $("#btnRecargarTerminado"),
        btnCargarCalendario = $("#btnCargarCalendario"),
        gridDetallesModalMaestro = $("#gridDetallesModalMaestro"),
        modalListadoMaestro = $("#modalListadoMaestro"),
        lgModalLM = $("#lgModalLM"),
        cmIniciarOverhaul = $("#cmIniciarOverhaul"),
        cmGantt = $("#cmGantt"),
        gridModalGantt = $("#gridModalGantt"),
        modalDiagramaGantt = $("#modalDiagramaGantt"),
        btnModalGanttGuardar = $("#btnModalGanttGuardar"),
        txtModalFecha = $("#txtModalFecha"),
        gridOHFalla = $("#gridOHFalla"),
        btnModalFechaGuardar = $("#btnModalFechaGuardar");
        const modalOverhaulFalla = $("#modalOverhaulFalla");
        const btnAgregParo = $("#btnAgregParo");
        const cboEconomicoFalla = $("#cboEconomicoFalla");
        const btnModalOverhaulFalla = $("#btnModalOverhaulFalla");
        const txtFechaNuevoParo = $("#txtFechaNuevoParo");
        var esGuardado = false;
        var nombreCalendario = "";
        let componenteActual;
        let obraIDGlobal = 0

        function init() {
            iniciarCalendario();
            cboGrupoMaquina.fillCombo('/Overhaul/FillCboGrupoMaquinaComponentes', { obj: 0 });
            cboModeloMaquina.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: -1 }, true);
            cboModeloMaquina.select2();
            cboObra.fillCombo('/Overhaul/FillCboObraMaquina');
            cboCalendarioGuardado.fillCombo("/Overhaul/CargarCalendarios", { anio: cboAnio.val() });
            cboCalendarioTerminado.fillCombo("/Overhaul/CargarCalendarios", { anio: cboAnio.val() });
            IniciarGridGrantt();
            cboGrupoMaquina.change(cargarcboModeloMaquina);
            btnGuardarCalendario.click(guardarCalendario);
            btnGuardarNuevoCalendario.click(guardarNuevoCalendario);
            btnModalGuardar.click(guardarCalendario);
            btnCargarCalendario.click(cargarEventos)
            cboAnio.change(cargarCalendarios);
            cboCalendarioGuardado.change(cargarEventosGuardado);
            btnRecargarCalendario.click(cargarEventosGuardado);
            cboCalendarioTerminado.change(cargarEventosTerminados);
            btnRecargarTerminado.click(cargarEventosTerminados);
            if ( txtSubconjunto.length ) { 
                txtSubconjunto.getAutocomplete(SelectSubconjuntoPO, null, '/Overhaul/getSubConjuntosPlaneacion'); 
            }            
            btnModalGanttGuardar.click(guardarGantt);
            txtFechaNuevoParo.datepicker().datepicker("setDate", new Date());

            $(".custom-menu li").click(function(){    
                //switch($(this).attr("data-action")) {
                //    case "1": 
                //        IniciarOverhaul($(this).parent().attr("data-index")); 
                //        $(".custom-menu").hide(100);
                //        break;
                //    case "2": 
                //        if(cmGantt.attr("data-tipo") == "1"){ VerDiagramaGantt($(this).parent().attr("data-index")); }
                //        else{ GenerarDiagramaGantt($(this).parent().attr("data-index")); }
                //        $(".custom-menu").hide(100);
                //        break;
                //    case "3": 
                //        ListadoMaestro($(this).parent().attr("data-index"));
                //        lgModalLM.text("Detalles Overhaul equipo: ");
                //        $(".custom-menu").hide(100);
                //        modalListadoMaestro.modal("show"); 
                //        break;
                //}
                switch ($(this).attr("data-action"))
                {
                    case "1":
                        $(".custom-menu").finish().toggle(100);
                        var fecha = new Date($(this).attr("data-fecha"));
                        //console.log(fecha);
                        //txtModalFecha.datepicker("setDate", fecha).datepicker('fill');
                        $("#modalFecha").modal("show");
                        break;
                }
            });

            $(document).bind("mousedown", function (e) {    
                if (!$(e.target).parents(".custom-menu").length > 0) {
                    $(".custom-menu").hide(100);
                }
            });

            txtModalFecha.datepicker();

            btnModalFechaGuardar.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();

            });

            if ( gridOHFalla.length ) { 
                IniciarGridOHFalla();
            }  
            btnAgregParo.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                OpenModalFalla();
            });
            $('#modalOverhaulFalla').on('shown.bs.modal', function () {
                gridOHFalla.columns.adjust();
            });
            cboEconomicoFalla.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                CargarGridOHFalla();
            });
            btnModalOverhaulFalla.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                GuardarOHFalla();
            });
        }

        function iniciarCalendario() {
            calendar.fullCalendar({
                header: {
                    left:   'title',
                    center: '',
                    right: 'anio month today prev,next'
                },
                defaultView: 'anio',
                timezone: 'local',
                lang: 'es',
                editable: true,
                allDayDefault: true,
                dragScroll: true,
                height: "auto",
                aspectRatio: 3,

                //contentHeight: "auto",
                eventBorderColor: "black",
                views: {
                    anio: {
                        type: 'year',
                        buttonText: 'Año'
                    }
                },
                eventRender: function(event, element) {     
                    if(!event.hijo) { element.attr("data-index", event.id); }
                    else { element.attr("data-index", event.idPadre); }
                    element.find('span.fc-title').html(element.find('span.fc-title').text());
                },
                eventAfterRender: function(event, element) { 
                    element.find(".closeon").click(function () {
                        for (var i = 0; i < event.componentes.length; i++) { calendar.fullCalendar("removeEvents", event.componentes[i].componenteID); }
                        console.log($('#calendar').fullCalendar('clientEvents'));
                        $('#calendar').fullCalendar('removeEvents', event._id);
                        console.log($('#calendar').fullCalendar('clientEvents'));
                        guardarCalendario();
                    });
                    element.find(".closeon-componente").click(function () {

                    });
                    //$(".fc-content").bind("contextmenu", function (e) {
                    //    e.preventDefault();
                    //    e.stopPropagation();
                    //    e.stopImmediatePropagation();
                    //    //console.log(event);
                    //    //$("#cmFecha").attr("data-fecha", event.start._d);
                    //    $(".custom-menu").finish().toggle(100).
                    //    css({
                    //        top: e.pageY + "px",
                    //        left: e.pageX + "px"
                    //    });
                    //    var fechaEvento = $(this).find(".fechaEvento").text();
                    //    //console.log(fechaEvento);
                    //    //$("#cmFecha").attr("data-fecha", event.start._d);
                    //});

                },
                eventClick: function (event, jsEvent, view) {
                    if (!event.hijo) {
                        if (event.clickEstatus) {
                            event.clickEstatus = false;
                            for (var i = 0; i < event.componentes.length; i++) {
                                calendar.fullCalendar("removeEvents", event.componentes[i].componenteID);
                            }
                        }
                        else {
                            var eventosClick = $('#calendar').fullCalendar('clientEvents', function (evt) { return evt.clickEstatus == true; });
                            for (var i = 0; i < eventosClick.length; i++) { eventosClick[i].clickEstatus = false; }                            
                            var colorEventoHijo = "";
                            calendar.fullCalendar('removeEvents', function (event) { return event.hijo; });
                            event.clickEstatus = true;
                            var date = new Date(event.start);
                            var fechaEvento = new Date(date.getTime() + 86400000);
                            var mesEvento = fechaEvento.getMonth();
                            for (var i = 0; i < event.componentes.length; i++) {
                                colorEventoHijo = GetColorEventoHijo(event.tipo)
                                if(event.componentes[i].Value == "1") { colorEventoHijo = "#D3D3D3"; }
                                let fechaOH = new Date(event.start._d);
                                let fechaActual = new Date();                                
                                let diff = (fechaOH - fechaActual) / (1000 * 60 * 60 * 24);                                
                                diff = Math.round(diff);
                                if (diff <= 0) diff = 0;
                                let horasActuales = event.componentes[i].horasCiclo + (diff * event.ritmo);
                                let stringPrediccionHoras = event.componentes[i].horasCiclo + "/" + event.componentes[i].target + "/" + horasActuales.toFixed(2);
                                var auxComponente = { 
                                    id: event.componentes[i].componenteID,
                                    title: ((mesEvento == 11 && !event.terminado) ? ' <span class="btn btn-danger btn-sm pull-right closeon-componente" style="padding: 0px 5px;">X</span>' : '') + "<b class='textoCompHoras'>" + event.componentes[i].descripcion + " " + event.componentes[i].posicion + "</b><br><b class='textoCompEvento'>" + event.componentes[i].nombre
                                        + "&nbsp;-&nbsp;</b><span class='textoCompEvento'>" + stringPrediccionHoras + "<br>Fecha de Remoción: <span class='fechaEvento'>" + event.componentes[i].fechaRemocion + "</span></span>",
                                    start: event.start, 
                                    description: event.description + "" + event.componentes[i].componenteID,
                                    color: colorEventoHijo,
                                    borderColor: "#404040",
                                    hijo: true,
                                    maquinaID: event.maquinaID,
                                    textColor: "#333",
                                    maquina: event.title, 
                                    idPadre: event.id, 
                                    editable: event.editable,
                                    Tipo: event.componentes[i].Tipo,
                                    horasCiclo: event.componentes[i].horasCiclo,
                                    target: event.componentes[i].target,
                                    nombre: event.componentes[i].nombre,
                                    descripcion: event.componentes[i].descripcion,
                                    posicion: event.componentes[i].posicion,
                                    tipoOverhaul: event.componentes[i].tipoOverhaul,
                                    fechaRemocion: event.componentes[i].fechaRemocion,
                                    className: "eventoHijo",
                                    Value: event.componentes[i].Value
                                };
                                calendar.fullCalendar("renderEvent", auxComponente);
                            }
                        }
                        calendar.fullCalendar("updateEvent", event);
                    }
                },

                eventDrop: function (event, delta, revert, jsEvent, view) {
                    if (esPlaneacionNueva.val() == 'true') {
                        revert();
                        return;
                    }

                    if ((!event.hijo && event.iniciado) || (event.hijo && event.fechaRemocion != '--')) {
                        revert();
                        return;
                    }

                    esGuardado = false;
                    var eventosClick = $('#calendar').fullCalendar('clientEvents', function (evt) { return evt.clickEstatus == true; });
                    for (var i = 0; i < eventosClick.length; i++) { eventosClick[i].clickEstatus = false; }
                    //Checa si es padre o hijo
                    if (event.hijo) {
                        //Checa si ya se removió el evento
                        if (event.fechaRemocion == "--") {
                            //crea un nuevo hijo
                            //var auxComponente = event;
                            var auxComponente = {
                                Value: event.Value, componenteID: event.id, nombre: event.nombre, descripcion: event.descripcion,
                                posicion: event.posicion, Tipo: event.Tipo, horasCiclo: event.horasCiclo, target: event.target,
                                tipoOverhaul: event.tipoOverhaul, fechaRemocion: "--"
                            };
                            //evento Padre
                            var eventoPadre = $("#calendar").fullCalendar('clientEvents', event.idPadre);                            
                            //Eliminar evento hijo de padre original
                            calendar.fullCalendar('removeEvents', event.idPadre);
                            eventoPadre[0].componentes = eventoPadre[0].componentes.filter(function(componente){ return componente.componenteID != auxComponente.componenteID });
                            calendar.fullCalendar('renderEvent', { 
                                id: eventoPadre[0].id,
                                title:  eventoPadre[0].title,
                                start: eventoPadre[0].start,
                                description: eventoPadre[0].description,
                                color: eventoPadre[0].color,
                                editable: eventoPadre[0].editable,
                                tipo: eventoPadre[0].tipo,
                                maquinaID: eventoPadre[0].maquinaID,
                                clickEstatus: eventoPadre[0].clickEstatus,
                                componentes: eventoPadre[0].componentes,
                                hijo: eventoPadre[0].hijo,
                                ritmo: eventoPadre[0].ritmo,
                                terminado: eventoPadre[0].terminado,
                                indexCalOriginal: eventoPadre[0].indexCalOriginal,
                                iniciado: eventoPadre[0].iniciado,
                                maquina:  eventoPadre[0].maquina,
                                allDay: eventoPadre[0].allDay,
                                tablaID: eventoPadre[0].tablaID,
                                //source: eventoPadre[0].source,
                                //className: eventoPadre[0].className,
                                //end: eventoPadre[0].end
                            });
                            //Ocultar todos los eventos  hijos desplegados
                            var eventosHijo = $('#calendar').fullCalendar('clientEvents', function (evt) { return evt.hijo == true; }).map(a => a.id);
                            if (eventosHijo.length > 0) { calendar.fullCalendar("removeEvents", function filter(event) { return jQuery.inArray(event.id, eventosHijo) !== -1; }); }
                            //buscar eventos con el mismo economico en el mes de la nueva fecha
                            var eventosPadreNvaFecha = $('#calendar').fullCalendar('clientEvents', function (evt) {
                                let dateEvt = new Date(evt.start);
                                dateEvt = new Date(dateEvt.getTime() + 86400000);
                                let dateEvent = new Date(event.start);
                                dateEvent = new Date(dateEvent.getTime() + 86400000);
                                return evt.maquinaID == event.maquinaID && dateEvt.getMonth() == dateEvent.getMonth() && dateEvt.getYear() == dateEvent.getYear() && evt.hijo == false;
                            });
                            //Si se encontraron eventos padre con el mismo economico en el mes de la nueva fecha agregar el nuevo

                            var dia = "";
                            var mes = "";
                            var componentes = new Array();
                            var date = new Date(event.start);
                            var fechaEvento = new Date(date.getTime() + 86400000);
                            var mesEvento = fechaEvento.getMonth();
                            date = new Date(event.start);
                            date = new Date(date.getTime() + 86400000);
                            var d = date.getDate().toString();
                            if (d < 10) dia = "0" + d.toString();
                            else dia = d.toString();
                            var m = date.getMonth();
                            m += 1;
                            if (m < 10) mes = "0" + m.toString();
                            else mes = m.toString();
                            var y = date.getFullYear();
                            var color = GetColorEventoPadre(event.tipoOverhaul);
                            if (eventoPadre[0].terminado) { color = "#696969"; }

                            if (eventosPadreNvaFecha.length > 0) {
                                for (var i = 0; i < eventosPadreNvaFecha.length; i++) {
                                    if (eventosPadreNvaFecha[i].tipo > event.tipoOverhaul && event.Value == "0") {
                                        eventosPadreNvaFecha[i].tipo = event.tipoOverhaul;
                                        eventosPadreNvaFecha[i].color = GetColorEventoPadre(event.tipoOverhaul);
                                    }
                                    eventosPadreNvaFecha[i].componentes.push(auxComponente);
                                    calendar.fullCalendar("updateEvent", eventosPadreNvaFecha[i]);
                                }
                                eventoPadre[0].componentes = eventoPadre[0].componentes.filter(function (item) { return item.componenteID != event.id.toString(); });
                                if (eventoPadre[0].componentes.length < 1) { calendar.fullCalendar("removeEvents", eventoPadre[0].id); }
                                else { ChangeColorDropEvent(eventoPadre); }
                            }
                            else {

                                componentes.push({ 
                                    Value: event.Value, componenteID: event.id, nombre: event.nombre, descripcion: event.descripcion,
                                    posicion: event.posicion, Tipo: event.Tipo, horasCiclo: event.horasCiclo, target: event.target,
                                    tipoOverhaul: event.tipoOverhaul, fechaRemocion: "--"                                    
                                });

                                
                                
                                var auxAgregarEventoPadre = {
                                    id: y + mes + d + event.maquinaID.toString(),
                                    title: eventoPadre[0].maquina + ((mesEvento == 11 && !eventoPadre[0].terminado) ? ' <span class="btn btn-danger btn-sm pull-right closeon" style="padding: 0px 5px;">X</span>' : '')
                                    + '<br><span style="font-size: 12px;">Fecha Programada: <span class="fechaEvento">' + y + "-" + mes + "-" + dia + '</span></span>',
                                        //event.maquina,
                                    editable: true,
                                    start: date,
                                    description: y + mes + dia + event.maquinaID.toString(),
                                    color: color,
                                    tipo: event.tipoOverhaul,
                                    calendarioID: eventoPadre[0].calendarioID,
                                    maquinaID: eventoPadre[0].maquinaID,
                                    estatus: event.estatus,
                                    clickEstatus: false,
                                    componentes: componentes,
                                    hijo: false,
                                    ritmo: eventoPadre[0].ritmo,
                                    terminado: false,
                                    indexCalOriginal: y + mes + d + event.maquinaID.toString(),
                                    iniciado: false,
                                    maquina:  eventoPadre[0].maquina,
                                    allDay: eventoPadre[0].allDay,
                                    tablaID: 0,
                                };

                                calendar.fullCalendar("renderEvent", auxAgregarEventoPadre, "stick");
                                eventoPadre[0].componentes = eventoPadre[0].componentes.filter(function (item) { return item.componenteID != event.id.toString(); });
                                if (eventoPadre[0].componentes.length < 1) { calendar.fullCalendar("removeEvents", eventoPadre[0].id); }
                                else { ChangeColorDropEvent(eventoPadre); }
                                calendar.fullCalendar("removeEvents", event.id);
                            }
                        }
                        else
                        {                            
                            ConfirmacionGeneral("Alerta", "No se puede mover el componente pues ya registra una remoción");
                            jsEvent();
                        }
                    }
                    else {
                        var eventosHijo = $('#calendar').fullCalendar('clientEvents', function (evt) { return evt.hijo == true; });
                        if (eventosHijo.length > 0) { for (var i = 0; i < eventosHijo.length; i++) { calendar.fullCalendar("removeEvents", eventosHijo[i].id); } }
                        if (event.clickEstatus) {
                            event.clickEstatus = false;
                        }
                        var eventosPadreNvaFecha = $('#calendar').fullCalendar('clientEvents', function (evt) {
                            let dateEvt = new Date(evt.start);
                            dateEvt = new Date(dateEvt.getTime() + 86400000);
                            let dateEvent = new Date(event.start);
                            dateEvent = new Date(dateEvent.getTime() + 86400000);
                            return evt.maquinaID == event.maquinaID && dateEvt.getMonth() == dateEvent.getMonth() && dateEvt.getYear() == dateEvent.getYear() && evt.hijo == false;
                        });
                        if (!event.iniciado) {
                            for (var i = 0; i < eventosPadreNvaFecha.length; i++) {
                                if (eventosPadreNvaFecha[i].id != event.id && !eventosPadreNvaFecha[i].iniciado) {
                                    $.merge(event.componentes, eventosPadreNvaFecha[i].componentes);
                                    if (eventosPadreNvaFecha[i].tipo < event.tipo) {
                                        event.tipo = eventosPadreNvaFecha[i].tipo;
                                    }
                                    calendar.fullCalendar("removeEvents", eventosPadreNvaFecha[i].id);
                                }
                            }
                        }
                        var mes = "";
                        var dia = "";
                        var date = new Date(event.start);
                        date = new Date(date.getTime() + 86400000);
                        var mesEvento = date.getMonth();
                        var d = date.getDate().toString();
                        if (d < 10) dia = "0" + d.toString();
                        else dia = d.toString();
                        var m = date.getMonth();
                        m += 1;
                        if (m < 10) mes = "0" + m.toString();
                        else mes = m.toString();
                        var y = date.getFullYear();
                        event.id = y + mes + dia + event.maquinaID.toString();
                        event._id = event.id;
                        event.description = event.id;
                        event.title = event.maquina + ((mesEvento == 11 && !event.terminado) ? ' <span class="btn btn-danger btn-sm pull-right closeon" style="padding: 0px 5px;">X</span>' : '')
                            + '<br><span style="font-size: 12px;">Fecha Programada: <span class="fechaEvento">' + y + "-" + mes + "-" + dia + '</span></span>',
                        event.color = GetColorEventoPadre(event.tipo);
                        if (event.terminado) event.color = "#696969";
                    }
                    //guardarCalendario();
                }
            });
        }

        function cargarEventosGuardado()
        {
            esGuardado = true;
            //if (cboCalendarioGuardado.val() == "") { cargarEventos(); }
            //else {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/Overhaul/cargarEventosPlaneacionOverhaulGuardados',
                    //async: false,
                    data: { idCalendario: cboCalendarioGuardado.val() },
                    success: function (response) {
                        $.unblockUI();                        
                        var eventosOH = [];
                        cboEconomicoFalla.fillCombo("/Overhaul/FillCboEconomicosByObraID", { obra: response.obra });
                        for (var i = 0; i < response.eventos.length; i++) {
                            var color = GetColorEventoPadre(response.eventos[i].tipo);
                            if (response.eventos[i].terminado) color = "#696969";
                            var fechaEvento = new Date(response.eventos[i].fecha);
                            var fechaEliminar = new Date(fechaEvento.getTime() + 86400000);
                            var mesEvento = fechaEliminar.getMonth();
                            //if (fechaEvento < new Date()) { color = "gray";}                            
                            var aux = {
                                id: response.eventos[i].id,
                                title: response.eventos[i].maquina + ((mesEvento == 11 && !response.eventos[i].terminado) ? ' <span class="btn btn-danger btn-sm pull-right closeon" style="padding: 0px 5px;">X</span>' : '')
                                    + '<br><span style="font-size: 12px;">' + (response.eventos[i].fechaInicio == '--' ? ('Fecha Programada: <span class="fechaEvento">' + response.eventos[i].fecha) : ('Fecha Inicio: <span class="fechaEvento">' + response.eventos[i].fechaInicio)) + '</span></span>',
                                start: response.eventos[i].fecha,
                                description: response.eventos[i].fecha + response.eventos[i].maquinaID + response.eventos[i].id,
                                color: color,
                                editable: true,
                                tipo: response.eventos[i].tipo,
                                calendarioID: response.eventos[i].calendarioID,
                                maquinaID: response.eventos[i].maquinaID,
                                estatus: response.eventos[i].estatus,
                                clickEstatus: false,
                                componentes: response.eventos[i].componentes,
                                hijo: false,
                                ritmo: response.eventos[i].ritmo,
                                terminado: response.eventos[i].terminado,
                                indexCalOriginal: response.eventos[i].indexCalOriginal,
                                iniciado: response.eventos[i].iniciado,
                                maquina: response.eventos[i].maquina,
                                tablaID: response.eventos[i].tablaID,
                            };
                            eventosOH.push(aux);
                        }
                        if (response.calendario != null) cboGrupoMaquina.val(response.calendario.grupoMaquinaID);
                        else cboGrupoMaquina.val('');

                        if (cboGrupoMaquina.val() == null) { cboGrupoMaquina.val(""); }
                        cargarcboModeloMaquina();
                        if (response.calendario != null) cboModeloMaquina.val(response.calendario.modeloMaquinaID);
                        else cboModeloMaquina.val('');

                        if (cboModeloMaquina.val() == null) { cboModeloMaquina.val(""); }
                        if (response.calendario != null) txtSubconjunto.val(response.calendario.subConjuntoID);
                        else txtSubconjunto.val('');

                        if (response.calendario != null) {cboObra.val(response.calendario.obraID); obraIDGlobal = response.calendario.obraID; }
                        else cboObra.val('');
                        if (cboObra.val() == null) { cboObra.val(""); }
                        calendar.fullCalendar('removeEvents');
                        calendar.fullCalendar('removeEventSource', eventosOH);
                        calendar.fullCalendar('addEventSource', eventosOH);
                        calendar.fullCalendar('refetchEvents');
                        let fechaCalendario = response.calendario != null ? new Date(response.calendario.anio, 1) : new Date();
                        calendar.fullCalendar('gotoDate', fechaCalendario);
                    },
                    error: function () { $.unblockUI(); }
                });
            //}
        }

        function cargarEventosTerminados()
        {
            esGuardado = true;
            //if (cboCalendarioGuardado.val() == "") { cargarEventos(); }
            //else {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Overhaul/cargarEventosPlaneacionOverhaulTerminados',
                //async: false,
                data: { idCalendario: cboCalendarioTerminado.val() },
                success: function (response) {
                    $.unblockUI();                        
                    var eventosOH = [];
                    cboEconomicoFalla.fillCombo("/Overhaul/FillCboEconomicosByObraID", { obra: response.obra });
                    for (var i = 0; i < response.eventos.length; i++) {
                        var color = GetColorEventoPadre(response.eventos[i].tipo);
                        if (response.eventos[i].terminado) color = "#696969";
                        var fechaEvento = new Date(response.eventos[i].fecha);
                        var fechaEliminar = new Date(fechaEvento.getTime() + 86400000);
                        var mesEvento = fechaEliminar.getMonth();
                        //if (fechaEvento < new Date()) { color = "gray";}                            
                        var aux = {
                            id: response.eventos[i].id,
                            title: response.eventos[i].maquina + ((mesEvento == 11 && !response.eventos[i].terminado) ? ' <span class="btn btn-danger btn-sm pull-right closeon" style="padding: 0px 5px;">X</span>' : '')
                                + '<br><span style="font-size: 12px;">' + (response.eventos[i].fechaInicio == '--' ? ('Fecha Programada: <span class="fechaEvento">' + response.eventos[i].fecha) : ('Fecha Inicio: <span class="fechaEvento">' + response.eventos[i].fechaInicio)) + '</span></span>',
                            start: response.eventos[i].fecha,
                            description: response.eventos[i].fecha + response.eventos[i].maquinaID + response.eventos[i].id,
                            color: color,
                            editable: true,
                            tipo: response.eventos[i].tipo,
                            calendarioID: response.eventos[i].calendarioID,
                            maquinaID: response.eventos[i].maquinaID,
                            estatus: response.eventos[i].estatus,
                            clickEstatus: false,
                            componentes: response.eventos[i].componentes,
                            hijo: false,
                            ritmo: response.eventos[i].ritmo,
                            terminado: response.eventos[i].terminado,
                            indexCalOriginal: response.eventos[i].indexCalOriginal,
                            iniciado: response.eventos[i].iniciado,
                            maquina: response.eventos[i].maquina
                        };
                        eventosOH.push(aux);
                    }
                    if (response.calendario != null) cboGrupoMaquina.val(response.calendario.grupoMaquinaID);
                    else cboGrupoMaquina.val('');

                    if (cboGrupoMaquina.val() == null) { cboGrupoMaquina.val(""); }
                    cargarcboModeloMaquina();
                    if (response.calendario != null) cboModeloMaquina.val(response.calendario.modeloMaquinaID);
                    else cboModeloMaquina.val('');

                    if (cboModeloMaquina.val() == null) { cboModeloMaquina.val(""); }
                    if (response.calendario != null) txtSubconjunto.val(response.calendario.subConjuntoID);
                    else txtSubconjunto.val('');

                    if (response.calendario != null) {cboObra.val(response.calendario.obraID); obraIDGlobal = response.calendario.obraID; }
                    else cboObra.val('');
                    if (cboObra.val() == null) { cboObra.val(""); }
                    calendar.fullCalendar('removeEvents');
                    calendar.fullCalendar('removeEventSource', eventosOH);
                    calendar.fullCalendar('addEventSource', eventosOH);
                    calendar.fullCalendar('refetchEvents');
                    let fechaCalendario = response.calendario != null ? new Date(response.calendario.anio, 1) : new Date();
                    calendar.fullCalendar('gotoDate', fechaCalendario);
                },
                error: function () { $.unblockUI(); }
            });
            //}
        }

        function cargarEventos() {
            esGuardado = false;
            cboCalendarioGuardado.val("");
            btnCargarCalendario.attr("data-obra", cboObra.val());
            btnCargarCalendario.attr("data-grupo", cboGrupoMaquina.val());
            var countModelo = $("#cboModeloMaquina :selected").length;
            btnCargarCalendario.attr("data-modelo", countModelo == 1 ? cboModeloMaquina.val()[0] : 0);
            btnCargarCalendario.attr("data-subconjunto", txtSubconjunto.val());
            btnCargarCalendario.attr("data-tipoSubconjunto", txtSubconjunto.attr("data-tipo"));
            $.blockUI({ message: mensajes.PROCESANDO });           
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Overhaul/cargarEventosPlaneacionOverhaul',
                //async: false,
                data: {  grupoMaquina: cboGrupoMaquina.val(), modeloMaquina: cboModeloMaquina.val(), obra: cboObra.val(), subconjunto: txtSubconjunto.val(), tipoSubconjunto: txtSubconjunto.attr("data-tipo") },
                success: function (response) {
                    $.unblockUI();
                    var eventosOH = [];
                    for (var i = 0; i < response.eventos.length; i++)
                    {
                        var color = GetColorEventoPadre(response.eventos[i].tipo);
                        if (response.eventos[i].terminado) color = "#696969";
                        var fecha = new Date(response.eventos[i].fecha);
                        var fechaEliminar = new Date(fecha.getTime() + 86400000);
                        var mesEvento = fechaEliminar.getMonth();
                        var diasDiferencia = (((new Date() - fecha) / 6000000) / 60) / 24;
                        if (fecha < new Date()) { fecha = new Date(); /*color = "red";*/ }
                        var aux = {
                            id: response.eventos[i].id,
                            title: response.eventos[i].maquina + (mesEvento == 11 ? ' <span class="btn btn-danger btn-sm pull-right closeon" style="padding: 0px 5px;">X</span>' : '')
                                + '<br><span style="font-size: 12px;">Fecha Programada: <span class="fechaEvento">' + response.eventos[i].fecha + '</span></span>',
                            start: fecha,
                            description: response.eventos[i].fecha + response.eventos[i].maquinaID + response.eventos[i].id,
                            color: color,
                            editable: true, //response.eventos[i].estatus == 0 ? true : false,
                            //componentesID: response.eventos[i].idComponentes,
                            tipo: response.eventos[i].tipo,
                            calendarioID: response.eventos[i].calendarioID,
                            maquinaID: response.eventos[i].maquinaID,
                            estatus: response.eventos[i].estatus,
                            clickEstatus: false,
                            componentes: response.eventos[i].componentes,
                            hijo: false,
                            ritmo: response.eventos[i].ritmo,
                            terminado: response.eventos[i].terminado,
                            indexCalOriginal: response.eventos[i].indexCalOriginal,
                            iniciado: response.eventos[i].iniciado,
                            maquina: response.eventos[i].maquina
                        };
                        eventosOH.push(aux);
                    }
                    calendar.fullCalendar('removeEvents');
                    calendar.fullCalendar('removeEventSource', eventosOH);
                    calendar.fullCalendar('addEventSource', eventosOH);
                    calendar.fullCalendar('refetchEvents');
                    nombreCalendario = "";
                    if(cboGrupoMaquina.val() != "" && cboGrupoMaquina.val() != null)
                        nombreCalendario += $("#cboGrupoMaquina option:selected").text() + "-";
                    if(cboModeloMaquina.val() != "" && cboModeloMaquina.val() != null)
                    {
                        for(var i = 0; i < cboModeloMaquina.val().length; i++)
                        {
                            nombreCalendario += $($("#cboModeloMaquina option:selected")[i]).text();
                            if(i < (cboModeloMaquina.val().length - 1)) nombreCalendario += "/"
                        }
                        nombreCalendario += "-";
                    }
                        
                    if(cboObra.val() != "" && cboObra.val() != null)
                        nombreCalendario += $("#cboObra option:selected").text() + "-";
                    if(txtSubconjunto.val() != "" && txtSubconjunto.val() != null)
                        nombreCalendario += txtSubconjunto.val() + "-";
                    if(nombreCalendario == "")
                        nombreCalendario = "CALENDARIO GENERAL-";
                    obraIDGlobal = response.obra;
                },
                error: function () { $.unblockUI(); }
            }); 
        }


        function cargarcboModeloMaquina()
        {
            if (cboGrupoMaquina.val() != "") {
                //cboModeloMaquina.prop("disabled", false);
                cboModeloMaquina.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: cboGrupoMaquina.val() }, true);
            }
            else {
                cboModeloMaquina.val("");
                //cboModeloMaquina.prop("disabled", true); 
                cboModeloMaquina.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: -1 }, true);
            }                
        }

        function SelectSubconjuntoPO(event, ui) {
            txtSubconjunto.text(ui.item.descripcion);
            txtSubconjunto.attr("data-tipo", ui.item.tipo);
        }

        function guardarCalendario()
        {                       
            let listaOverhauls = getListaOverhauls();
            if (listaOverhauls.length > 0) {
                nombreCalendario = $("#cboCalendarioGuardado option:selected").text();
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/Overhaul/GuardarCalendarioOverhaul',
                    //async: false,
                    data: { calendario: getCalendario(), listaOverhauls: listaOverhauls },
                    success: function (response) {
                        $.unblockUI();
                        switch (response.index) {
                            case 0:
                                modalGuardarCalendario.modal("hide");
                                AlertaGeneral("Alerta", "No se encontró el calendario a actualizar");
                                break;
                            case -1:
                                AlertaGeneral("Alerta", "Ya existe un calendario almacenado con ese nombre");
                                break;
                            case -2:
                                AlertaGeneral("Alerta", "Ya existe un calendario almacenado con esas características");
                                break;
                            default:
                                modalGuardarCalendario.modal("hide");
                                cboCalendarioGuardado.fillCombo("/Overhaul/CargarCalendarios", { anio: cboAnio.val() });
                                cboCalendarioGuardado.val(response.index);
                                cboCalendarioGuardado.change();
                                esGuardado = true;
                                break;
                        }
                        ConfirmacionGeneral("Confirmación", "Se guardo el calendario", "bg-green");
                    },
                    error: function () {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);
                    }
                });
            }
        }

        function guardarNuevoCalendario()
        {                       
            let listaOverhauls = getListaOverhauls();
            if (listaOverhauls.length > 0) {
                nombreCalendario += calendar.fullCalendar('getDate').year();
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/Overhaul/GuardarNuevoCalendarioOverhaul',
                    data: { calendario: getCalendarioNuevo(), listaOverhauls: listaOverhauls },
                    success: function (response) {
                        $.unblockUI();
                        if(response.exito) { ConfirmacionGeneral("Confirmación", "Se guardo el calendario", "bg-green"); }
                        else { AlertaGeneral("Alerta", "Ya existe un calendario almacenado con esas características"); }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);
                    }
                });
            }
        }

        function abrirModal()
        {
            txtNombreCalendario.val("");
            modalGuardarCalendario.modal("show");
        }

        function getCalendario()
        {
            var fechaActual = new Date();
            return {
                id: (cboCalendarioGuardado.val() == "" ? 0 : cboCalendarioGuardado.val()),
                nombre: nombreCalendario,
                tipo: 0,
                estatus: 0,
                fecha: fechaActual.toLocaleString(),
                grupoMaquinaID: cboGrupoMaquina.val(),
                modeloMaquinaID: cboModeloMaquina.val(),
                subConjuntoID: txtSubconjunto.val(),
                obraID: obraIDGlobal,
                anio: calendar.fullCalendar('getDate').year()
            };
        }

        function getCalendarioNuevo()
        {
            var fechaActual = new Date();
            return {
                id: 0,
                nombre: nombreCalendario,
                tipo: 0,
                estatus: 0,
                fecha: fechaActual.toLocaleString(),
                grupoMaquinaID: btnCargarCalendario.attr("data-grupo"),
                modeloMaquinaID: btnCargarCalendario.attr("data-modelo"),
                subConjuntoID: btnCargarCalendario.attr("data-subconjunto"),
                obraID: btnCargarCalendario.attr("data-obra"),
                anio: calendar.fullCalendar('getDate').year()
            };
        }

        function getListaOverhauls()
        {
            var source = calendar.fullCalendar("clientEvents", function (evt) {
                return evt.hijo == false;
            });
            arrOverhauls = [];
            for(var i = 0; i < source.length; i++)
            {
                var fecha = new Date(source[i].start._d);
                fecha.setHours(fecha.getHours() + 7);
                var aux = {
                    id: source[i].tablaID,
                    idComponentes: JSON.stringify(jQuery.grep(source[i].componentes, function( n, j ) { return n.Value !== null })),
                    maquinaID: source[i].maquinaID,
                    fecha: fecha.toLocaleString(),
                    tipo: source[i].tipo,
                    calendarioID: source[i].calendarioID,
                    estatus: source[i].estatus,
                    indexCal: source[i].id,
                    ritmo: source[i].ritmo,
                    terminado: source[i].terminado,
                    indexCalOriginal: source[i].indexCalOriginal,
                    falla: false
                };
                arrOverhauls.push(aux);
            }            
            return arrOverhauls;
        }

        function IniciarOverhaul(index){
            btnModalGanttGuardar.css("display", "block");
            btnModalGanttGuardar.attr("data-index", index);
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/IniciarActividadesOverhaul",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    idEvento: index
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {                        
                        if(response.exito) { AlertaGeneral("Alerta", "Se inició el proceso de Overhaul"); }
                        else{ AlertaGeneral("Alerta", "Error al realizar la consulta"); }
                    }
                    else { AlertaGeneral("Alerta", "Error al realizar la consulta"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function IniciarGridGrantt()
        {
            gridModalGantt.bootgrid({
                rowCount: -1,
                selection: true,
                multiSelect: true,
                templates: { header: "" }
            });
        }

        function GenerarDiagramaGantt(index)
        {
            btnModalGanttGuardar.css("display", "block");
            btnModalGanttGuardar.attr("data-index", index);
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarDatosDiagramaGantt",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    idEvento: index
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {                        
                        gridModalGantt.bootgrid("clear");
                        gridModalGantt.bootgrid("append", response.actividades);
                        gridModalGantt.bootgrid('reload');                        
                        modalDiagramaGantt.modal("show");
                    }
                    else { AlertaGeneral("Alerta", "Error al realizar la consulta"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function ListadoMaestro(index)
        {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarDatosDetalleMaestroPlaneacion",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    indexCal: index
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridDetallesModalMaestro.bootgrid({
                            rowCount: -1,
                            templates: { header: "" }
                        });
                        gridDetallesModalMaestro.bootgrid("clear");
                        gridDetallesModalMaestro.bootgrid("append", response.detalles);
                        gridDetallesModalMaestro.bootgrid('reload');
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function guardarGantt()
        {
            var actividades = [];
            actividades = gridModalGantt.bootgrid("getSelectedRows");
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/GuardarDiagramaGantt",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idEvento: btnModalGanttGuardar.attr("data-index"), actividades: actividades }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        if(response.exito)
                        {
                            modalDiagramaGantt.modal("hide");
                            AlertaGeneral("Alerta", "Se ha guardado el diagrama con éxito");
                        }
                        else { AlertaGeneral("Alerta", "Ha ocurrido un error, no se ha guardado el diagrama"); }
                    }
                    else { AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function VerDiagramaGantt(index)
        {
            btnModalGanttGuardar.css("display", "none");
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarDatosDiagramaGantt",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idEvento: index }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridModalGantt.bootgrid("clear");
                        gridModalGantt.bootgrid("append", response.actividades);
                        gridModalGantt.bootgrid('reload');                        
                        modalDiagramaGantt.modal("show");
                    }
                    else { AlertaGeneral("Alerta", "Error al realizar la consulta"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GetComponenteActual(index, tipo)
        {
            $.ajax({
                url: "/Overhaul/GetComponenteByID",
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({ index: index, tipo: tipo }),
                success: function (response) {
                    if (response.success) { componenteActual = response.componente; }
                    else{ componenteActual = ""; }
                }
            });
        }

        function GetColorEventoPadre(tipoOverhaul)
        {
            var color = "";
            switch (tipoOverhaul) {
                case 0:
                    color = "#5cb85c";
                    break;
                case 1:
                    color = "#204d74";
                    break;
                case 2:
                    color = "#ff8c1a";
            }
            return color;
        }

        function GetColorEventoHijo(tipoOverhaul) {
            var color = "";
            switch (tipoOverhaul) {
                case 0:
                    color = "#cbe7cb";
                    break;
                case 1:
                    color = "#c3d9ef";
                    break;
                case 2:
                    color = "#ffcc99";
            }
            return color;
        }
        
        function ChangeColorDropEvent(eventoPadre) {
            var auxTipoOverhaul = eventoPadre[0].componentes.filter(function (item) { return item.tipoOverhaul == 0; });
            if (auxTipoOverhaul.length > 0) {
                eventoPadre[0].tipo = 0;
                eventoPadre[0].color = "#5cb85c";
            }
            else {
                auxTipoOverhaul = eventoPadre[0].componentes.filter(function (item) { return item.tipoOverhaul == 1; });
                if (auxTipoOverhaul.length > 0) {
                    eventoPadre[0].tipo = 1;
                    eventoPadre[0].color = "#204d74";
                }
                else {
                    eventoPadre[0].tipo = 2;
                    eventoPadre[0].color = "#ff8c1a";
                }
            }
            if (eventoPadre[0].terminado) { eventoPadre[0].color = "#696969"; }
        }

        function OpenModalFalla()
        {
            $("#title-modal-falla").text("Alta de Overhaul por falla");
            gridOHFalla.clear();
            gridOHFalla.draw();
            modalOverhaulFalla.modal("show");
        }

        function IniciarGridOHFalla() {
            gridOHFalla = $("#gridOHFalla").DataTable({
                language: {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                retrieve: true,
                searching: false,
                paging: false,
                scrollY: '50vh',
                scrollCollapse: true,
                autoWidth: false,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },
                drawCallback: function (settings) {
                    $("#ckSelectAllFalla").change(function () {
                        $("input.checkbox-lista-componentes").prop('checked', $(this).prop("checked"));
                        if ($(this).prop("checked")) { $("input.checkbox-lista-componentes").addClass("agregar"); }
                        else { $("input.checkbox-lista-componentes").removeClass("agregar"); }
                        if ($('.agregar').length > 0) { btnModalOverhaulFalla.prop("disabled", false); }
                        else { btnModalOverhaulFalla.prop("disabled", true); }
                    });
                    $("input.checkbox-lista-componentes").change(function () {
                        if ($(this).prop("checked")) { $(this).addClass("agregar"); }
                        else { $(this).removeClass("agregar"); }
                        if ($('.agregar').length > 0) { btnModalOverhaulFalla.prop("disabled", false); }
                        else { btnModalOverhaulFalla.prop("disabled", true); }
                    });
                },
                columns: [
                    {
                        data: 'id',
                        title: '<input id="ckSelectAllFalla" class="checkbox-select-all checkbox-componentes" value="1" type="checkbox">'
                    },
                    { data: 'nombre', title: 'COMPONENTE' },
                    {
                        "render": function (data, type, row, meta) { return row.descripcion + " " + row.posicion; },
                        title: 'DESCRIPCIÓN'
                    }
                ],
                columnDefs: [
                    {
                        "targets": 0,
                        "data": null,
                        "defaultContent": '',
                        "orderable": false,
                        'render': function (data, type, row, meta) {
                            return '<input class="checkbox-componentes checkbox-lista-componentes" type="checkbox" data-componenteID = "' + row.componenteID + '" data-nombre = "' + row.nombre +
                                '" data-descripcion = "' + row.descripcion + '" data-posicion = "' + row.posicion + '" data-Tipo = "' + row.Tipo + '" data-horasCiclo = "' + row.horasCiclo +
                                '" data-target = "' + row.target + '" data-tipoOverhaul = "' + row.tipoOverhaul + '">';
                        }
                    },
                ],
                order: [[0, 'asc']],
            });
        }

        function CargarGridOHFalla()
        {
            if (cboEconomicoFalla.val() != "") {
                $.blockUI({ message: "Procesando..." });
                $.ajax({
                    url: '/Overhaul/CargarGridOHNuevoParo',
                    datatype: "json",
                    type: "POST",
                    data: {
                        idMaquina: cboEconomicoFalla.val()
                    },
                    success: function (response) {
                        $.unblockUI();
                        if (response.success) {
                            gridOHFalla.clear();
                            gridOHFalla.rows.add(response.data);
                            gridOHFalla.draw();
                        }
                        else { AlertaGeneral("Alerta", "Se encontró un error al cargar los datos"); }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", "Se encontró un error al cargar los datos");
                    }
                });
            }
            else { gridOHFalla.clear(); gridOHFalla.draw(); }
        }

        function GuardarOHFalla()
        {
            var componentes = [];
            $("input.agregar").each(function () {
                componentes.push({
                    Value: "0",
                    componenteID: $(this).attr('data-componenteID'),
                    nombre: $(this).attr('data-nombre'),
                    descripcion: $(this).attr('data-descripcion'),
                    posicion: $(this).attr('data-posicion'),
                    Tipo: $(this).attr('data-Tipo'),
                    horasCiclo: $(this).attr('data-horasCiclo'),
                    target: $(this).attr('data-target'),
                    tipoOverhaul: $(this).attr('data-tipoOverhaul'),
                    //tipoOverhaul: 3,
                    falla: false
                });
            });

            var mes = "";
            var dia = "";
            var date = new Date(txtFechaNuevoParo.val());
            date = new Date(date.getTime() + 86400000);
            var d = date.getDate().toString();
            if (d < 10) dia = "0" + d.toString();
            else dia = d.toString();
            var m = date.getMonth();
            m += 1;
            if (m < 10) mes = "0" + m.toString();
            else mes = m.toString();
            var y = date.getFullYear();
            indexCal = y + mes + dia + cboEconomicoFalla.val().toString();

            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/GuardarOHParo',
                datatype: "json",
                type: "POST",
                data: {
                    idMaquina: cboEconomicoFalla.val(),
                    componentes: componentes,                    
                    fecha : txtFechaNuevoParo.val(),
                    calendarioID: cboCalendarioGuardado.val(),
                    indexCal: indexCal
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        cargarEventosGuardado();
                        AlertaGeneral("Éxito", "Se guardó Overhaul correctamente");
                        modalOverhaulFalla.modal("hide");
                    }
                    else { AlertaGeneral("Alerta", "Se encontró un error al guardar los datos"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Se encontró un error al guardar los datos");
                }
            });
        }

        function cargarCalendarios()
        {
            cboCalendarioGuardado.fillCombo("/Overhaul/CargarCalendarios", { anio: cboAnio.val() });
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.overhaul.planeacionoverhaul = new planeacionoverhaul();
    });
})();


