(function () {
    $.namespace('Administrativo.ReservacionVehiculo.Solicitud');

    Solicitud = function () {
        const btnSolicitar = $('#btnSolicitar');
        const btnReemplezarSolicitud = $('#btnReemplezarSolicitud');
        const btnCerrarSolicitud = $('#btnCerrarSolicitud');
        const inputFecha = $("#inputFecha");
        const selAutSolicita = $("#selAutSolicita");
        esReemplazo = false;
        solicitudAnterior = 0;

        function init() {
            initSelect();
            btnSolicitar.click(guardarSolicitud);
            btnReemplezarSolicitud.click(reemplazarSolicitud);
            btnCerrarSolicitud.click(setCancelarReemplazo);

            $('#inputFecha').datetimepicker({
                format: 'DD/MM/YYYY h:mm a'
            });

            $('#inputFechaEntrega').datetimepicker({
                format: 'DD/MM/YYYY h:mm a',
                useCurrent: true
            });

            $('#inputVigencia').datetimepicker({
                format: 'DD/MM/YYYY'
            });

            $('#inputVigencia').on('focus', function () {
                const input = $(this);
                const date = new Date(moment(input.val(), "DD-MM-YYYY").format());
                const hoy = new Date();

                if (date > hoy) { input.removeClass("invalid").addClass("valid"); }
                else { input.removeClass("valid").addClass("invalid"); }
            });

            $("#inputFecha").on("dp.change", function (e) {
                const input = $(this);
                const date = new Date(moment(input.val(), "DD-MM-YYYY, h:mm a").format());
                $("#inputFechaEntrega").data("DateTimePicker").minDate(date);
            });

        }

        function initSelect() {
            selAutSolicita.getAutocompleteValid(funAutSolicita, null, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
        }

        function funAutSolicita(event, ui) {
            selAutSolicita.text(ui.item.value);
        }

        function guardarSolicitud() {
            if (validaSolicitud()) {

                validaFechasSolicitud();

                // if (validaHorasSolicitud()) {
                //     AlertaGeneral("Aviso", "No puede reservar el vehiculo por mas de 5 horas.")
                // } else {
                //     validaFechasSolicitud();
                // }

            } else {
                AlertaGeneral("Aviso", "Todos los campos deben ser llenados y la licencia debe estar vigente");
            }
        }

        function validaHorasSolicitud() {
            debugger;
            const fecha = moment(new Date(moment($('#inputFecha').val(), "DD-MM-YYYY, h:mm a").format()));
            const fechaEntrega = moment(new Date(moment($('#inputFechaEntrega').val(), "DD-MM-YYYY, h:mm a").format()));

            const duration = moment.duration(fecha.diff(fechaEntrega)).asHours();

            return Math.abs(duration) > 5 ? true : false
        }

        function validaSolicitud() {
            let esValida = false;

            esValida = validaInputsVacios();
            if (esValida) esValida = validaVigencia();

            return esValida;
        }

        function validaVigencia() {
            let esValida = false;

            if (new Date(moment($('#inputVigencia').val(), "DD-MM-YYYY").format()) > new Date()) {
                esValida = true;
            } else esValida = false;

            return esValida;
        }

        function validaInputsVacios() {
            let esValida = false;

            if ($("#selAutSolicita").val() != "" && $('#inputMotivo').val() != "" && $('#inputJustificacion').val() != "" && $('#inputFecha').val() != "" && $('#inputFechaEntrega').val() != "" && $('#inputVigencia').val() != "")
                esValida = true;
            else esValida = false;

            return esValida;
        }

        function validaFechasSolicitud() {
            debugger;
            const fechaSalida = moment($('#inputFecha').val(), "DD-MM-YYYY, h:mm a").format();
            const fechaEntrega = moment($('#inputFechaEntrega').val(), "DD-MM-YYYY, h:mm a").format();

            $.get("/Administrativo/ReservacionVehiculo/GetSolicitudFecha", { fechaSalida: fechaSalida, fechaEntrega: fechaEntrega }, function (data) {
                if (data.success) {
                    data.items.forEach(function (solicitud) {
                        debugger;
                        solicitudAnterior = solicitud.id;
                        $('#spanUsuarioAuth').text(solicitud.solicitante);
                        $('#spanMotivoAuth').text(solicitud.motivo);
                        $('#inputJustificacionAuth').text(solicitud.justificacion);
                        $('#spanFechaAuth').text(solicitud.fechaSalida);
                        $('#spanFechaEntregaAuth').text(solicitud.fechaEntrega);

                        if (data.permiso) $('#btnReemplezarSolicitud').show();
                        else $('#btnReemplezarSolicitud').hide();
                    });
                    $('#modalSolicitud').modal('show');

                    esReemplazo = true;
                }
                else {
                    esReemplazo = false;
                }
            }).done(response => {
                if (!esReemplazo) agregarSolicitud();
            });
        }

        function agregarSolicitud() {
            let id = 0;
            $.post("/Administrativo/ReservacionVehiculo/GuardarSolicitud", getSolicitud(), function (response) {
                if (response.success) {
                    id = response.id;
                    AlertaGeneral("Aviso", "Solicitud enviada correctamente.");
                }
            }).done(response => {
                $.post("/Administrativo/ReservacionVehiculo/EnviarCorreoSolicitud", { id: id });
                recargarTodo();
            });
        }

        function reemplazarSolicitud() {
            var json = JSON.stringify(solicitudAnterior);
            let solicitudNueva_id = 0;

            $.post("/Administrativo/ReservacionVehiculo/EliminarSolicitud", { solicitud_id: solicitudAnterior })
                .done(response => {
                    $.post("/Administrativo/ReservacionVehiculo/GuardarSolicitud", getSolicitud())
                        .done(response => {
                            if (response.success) {
                                solicitudNueva_id = response.id;
                                AlertaGeneral("Aviso", "Se reemplazo correctamente");
                                $('#modalSolicitud').modal('hide');
                            }
                        })
                        .done(response => {
                            $.post("/Administrativo/ReservacionVehiculo/EnviarCorreoReemplazo", { solicitudAnterior_id: solicitudAnterior, solicitud: solicitudNueva_id });
                            recargarTodo();
                        });
                });
        }

        function getSolicitud() {
            return {
                fechaSalida: moment($('#inputFecha').val(), "DD-MM-YYYY, h:mm a").format(),
                fechaEntrega: moment($('#inputFechaEntrega').val(), "DD-MM-YYYY, h:mm a").format(),
                motivo: $('#inputMotivo').val(),
                descripcion: $('#inputJustificacion').val(),
                solicitante: $("#selAutSolicita").val(),
                vigenciaLicencia: moment($('#inputVigencia').val(), "DD-MM-YYYY").format(),
                autorizada: false,
                estatus: true
            };
        }

        function recargarTodo() {
            $('#inputMotivo').val("");
            $('#inputJustificacion').val("");
            $('#inputFecha').val("");
            $('#inputFechaEntrega').val("");
            $('#inputVigencia').val("");
            selAutSolicita.val("");
        }

        function setCancelarReemplazo() {
            esReemplazo = true;
            solicitudAnterior = 0;
        }

        function setFechaEntrega() {
            alert('dsffs');
        }


        init();
    }

    $(document).ready(function () {
        Administrativo.ReservacionVehiculo.Solicitud = new Solicitud();
    });
})();