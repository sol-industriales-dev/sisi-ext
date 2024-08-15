(function () {

    $.namespace('principal.home.rendimiento');

    rendimiento = function () {
        contenido = $("#contenido")
        function init() {
            iniciarGrid();
            cargarNotificaciones();
        }

        function cargarNotificaciones() {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Home/getNotificacionesRendimiento",
                data: {},
                asyn: false,
                success: function (response) {
                    contenido.bootgrid("clear");
                    notificaciones = response.rendimiento;
                    if (notificaciones.length > 0) {
                        contenido.bootgrid("append", notificaciones);
                    }
                },
                error: function () {
                    AlertaGeneral("Alerta", "Error en la consulta");
                }
            });
        }

        function iniciarGrid() {
            contenido.bootgrid({
                align: 'center',
                caseSensitive: false,
                formatters: {
                    "rendimiento": function (column, row) {
                        return "<span class='rendimiento'> " + row.rendimientofinal + " </span>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                contenido.find("th").css("text-align", "center");
                contenido.find("th").css("color", "white");
                //contenido.find("td").css("text-align", "center");
                contenido.find("td").css("font-size", "15px");
                contenido.find(".rendimiento").parent().css("text-align", "right");
            });
        }

        init();
    };

    $(document).ready(function () {
        principal.home.rendimiento = new rendimiento();
    });
})();