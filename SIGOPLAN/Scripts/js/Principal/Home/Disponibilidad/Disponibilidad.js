(function () {

    $.namespace('principal.home.disponibilidad');

    disponibilidad = function () {
        contenido = $("#contenido");
        mensajes = { PROCESANDO: 'Procesando...' };
        function init() {
            iniciarGrid();
            cargarNotificaciones();
        }

        function cargarNotificaciones() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Home/getNotificacionesDisponibilidad",
                data: {},
                asyn: false,
                success: function (response) {
                    $.unblockUI();
                    contenido.bootgrid("clear");
                    notificaciones = response.disponibilidad;
                    if (notificaciones.length > 0) {
                        contenido.bootgrid("append", notificaciones);
                    }
                },
                error: function () {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Error en la consulta");
                }
            });
        }

        function iniciarGrid() {
            contenido.bootgrid({
                caseSensitive: false,
                formatters: {
                    "Disponibilidad": function (column, row) {
                        return "<span class='Disponibilidad'> " + row.disponibilidad + " </span>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                contenido.find("th").css("text-align", "center");
                contenido.find("th").css("color", "white");
                //contenido.find("td").css("text-align", "center");
                contenido.find("td").css("font-size", "15px");
                contenido.find(".Disponibilidad").parent().css("text-align", "center");
            });
        }

        init();
    };

    $(document).ready(function () {
        principal.home.disponibilidad = new disponibilidad();
    });
})();
