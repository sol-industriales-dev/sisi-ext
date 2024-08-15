(function () {
    $.namespace('Administrativo.ReservacionVehiculo.Calendario');

    Calendario = function () {
        calendar = $("#calendar");
        btnAutorizar = $('#btnAutorizar');
        btnRechazar = $('#btnRechazar');
        btnCancelar = $('#btnCancelar');

        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };


        function init() {
            iniciarCalendario();
            cargarSolicitudes();

            btnAutorizar.click(autorizar);
            btnRechazar.click(rechazar);
            btnCancelar.click(cancelar);

        }

        function iniciarCalendario() {
            calendar.fullCalendar({
                header: {
                    left: 'title',
                    center: '',
                    right: 'anio month today prev,next'
                },
                defaultView: 'month',
                timezone: 'local',
                lang: 'es',
                editable: false,
                eventOrder: "description",
                allDayDefault: true,
                dragScroll: false,
                height: "auto",
                contentHeight: 600,
                views: {
                    anio: {
                        type: 'year',
                        buttonText: 'Año'
                    }
                },
                eventClick: function (event, jsEvent, view) {
                    //set the values and open the modal
                    $('#spanUsuario').text(event.solicitante);
                    $('#spanMotivo').text(event.title);
                    $('#inputJustificacion').text(event.description);
                    $('#spanFecha').text(moment(event.fecha).format('DD-MM-YYYY, h:mm:ss a'));
                    $('#spanFechaEntrega').text(moment(event.fechaEntrega).format('DD-MM-YYYY, h:mm:ss a'));
                    $("#btnAutorizar").val(event.id);
                    $("#btnRechazar").val(event.id);
                    $("#btnCancelar").val(event.id);
                    $("#btnCancelar").hide();

                    if (event.tienePermiso && !event.autorizada) {
                        $("#btnAutorizar").show();
                        $("#btnRechazar").show();
                    } else {
                        $("#btnAutorizar").hide();
                        $("#btnRechazar").hide();
                    }

                    if (event.tienePermiso && event.autorizada) $("#btnCancelar").show();
                    else $("#btnCancelar").hide();


                    $('#modalSolicitud').modal('show');
                },

                eventDragStop: function (event, jsEvent, view) {
                },

                eventDrop: function (event, delta, jsEvent, view) {
                }
            });
        }

        function cargarSolicitudes() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Administrativo/ReservacionVehiculo/GetSolicitudes',
                //async: false,
                //data: 
                success: function (response) {

                    $.unblockUI();
                    var solicitudes = [];
                    for (var i = 0; i < response.items.length; i++) {

                        const fecha = new Date(moment(response.items[i].fechaSalida, "DD-MM-YYYY, h:mm a").format());
                        const fechaEntrega = new Date(moment(response.items[i].fechaEntrega, "DD-MM-YYYY, h:mm a").format());

                        const color = response.items[i].autorizada ? '#5cb85c' : '#ffcf24';

                        let aux = {
                            id: response.items[i].id,
                            title: response.items[i].motivo,
                            start: fecha,
                            end: fechaEntrega,
                            description: response.items[i].justificacion,
                            color: color,
                            clickEstatus: false,
                            solicitante: response.items[i].solicitante,
                            fecha: fecha,
                            fechaEntrega: fechaEntrega,
                            tienePermiso: response.items[i].tienePermiso,
                            autorizada: response.items[i].autorizada
                        };
                        solicitudes.push(aux);
                    }
                    calendar.fullCalendar('removeEvents');
                    calendar.fullCalendar('removeEventSource', solicitudes);
                    calendar.fullCalendar('addEventSource', solicitudes);
                    calendar.fullCalendar('refetchEvents');
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function autorizar() {
            $.post("/Administrativo/ReservacionVehiculo/AutorizarSolicitud", { solicitud_id: $("#btnAutorizar").val() }, function (response) {
                if (response.success) {
                    AlertaGeneral("Aviso", "Solicitud autorizada correctamente.");
                    $('#modalSolicitud').modal('hide');
                } else {
                    AlertaGeneral("Aviso", "ocurrió un error al autorizar la solicitud.");
                }
            })
                .done(response => {
                    $.post("/Administrativo/ReservacionVehiculo/EnviarCorreoAutorizacion", { solicitudAutorizada_id: $("#btnAutorizar").val() });
                    cargarSolicitudes();
                });
        }

        function rechazar() {
            $.post("/Administrativo/ReservacionVehiculo/EliminarSolicitud", { solicitud_id: $("#btnRechazar").val() }, function (response) {
                if (response.success) {
                    AlertaGeneral("Aviso", "Solicitud rechazada correctamente.");
                    $('#modalSolicitud').modal('hide');
                } else {
                    AlertaGeneral("Aviso", "ocurrió un error al rechazar la solicitud.");
                }
            })
                .done(response => {
                    $.post("/Administrativo/ReservacionVehiculo/EnviarCorreoRechazo", { solicitudRechazada_id: $("#btnRechazar").val() });
                    cargarSolicitudes();
                });
        }

        function cancelar() {
            $.post("/Administrativo/ReservacionVehiculo/EliminarSolicitud", { solicitud_id: $("#btnCancelar").val() }, function (response) {
                if (response.success) {
                    AlertaGeneral("Aviso", "Solicitud cancelada correctamente.");
                    $('#modalSolicitud').modal('hide');
                } else {
                    AlertaGeneral("Aviso", "ocurrió un error al cancelar la solicitud.");
                }
            })
                .done(response => {
                    $.post("/Administrativo/ReservacionVehiculo/EnviarCorreoCancelacion", { solicitudCancelada_id: $("#btnCancelar").val() });
                    cargarSolicitudes();
                });
        }

        init();
    }

    $(document).ready(function () {
        Administrativo.ReservacionVehiculo.Calendario = new Calendario();
    });
})();