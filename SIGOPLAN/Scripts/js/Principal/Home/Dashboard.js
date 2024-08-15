(function () {

    $.namespace('principal.home.dashboard');

    dashboard = function () {
        badgeLicencias = $("#badgeLicencias"),
            badgePolizas = $("#badgePolizas"),
            badgeDisponibilidad = $("#badgeDisponibilidad"),
            badgeRendimiento = $("#badgeRendimiento"),
            badgeRecepcion = $("#badgeRecepcion"),
            badgeCursoManejo = $("#badgeCursoManejo"),
            notificacionesLicencias = $("#notificacionesLicencias"),
            notificacionesPolizas = $("#notificacionesPolizas"),
            notificacionesDisponibilidad = $("#notificacionesDisponibilidad"),
            notificacionesRendimiento = $("#notificacionesRendimiento"),
            notificacionesRecepcion = $("#notificacionesRecepcion"),
            notificacionesCursoManejo = $("#notificacionesCursoManejo"),
            ulRecepcion = $("#ulRecepcion"),

            btnLicencias = $("#btnLicencias"),
            btnPolizas = $("#btnPolizas"),
            btnRecepcion1 = $("#btnRecepcion1"),
            btnDisponibilidad = $("#btnDisponibilidad"),
            btnRendimiento = $("#btnRendimiento"),
            btnCursoManejo = $("#btnCursoManejo"),

            btnEstadisticasLicencias = $("#btnEstadisticasLicencias"),
            btnEstadisticasPolizas = $("#btnEstadisticasPolizas"),
            btnEstadisticasDisponibilidad = $("#btnEstadisticasDisponibilidad"),
            btnEstadisticasRendimiento = $("#btnEstadisticasRendimiento"),
            btnEstadisticasRecepcion1 = $("#btnEstadisticasRecepcion1"),
            btnEstadisticasCursoManejo = $("#btnEstadisticasCursoManejo"),

            LicenciasContenedor = $("#LicenciasContenedor");
        PolizasContenedor = $("#PolizasContenedor");
        DisponibilidadContenedor = $("#DisponibilidadContenedor");
        RendimientoContenedor = $("#RendimientoContenedor");
        RecepcionContenedor = $("#RecepcionContenedor");
        CursoManejoContenedor = $("#CursoManejoContenedor");
        mensajes = { PROCESANDO: 'Procesando...' };
        function init() {
            getNotificaciones();
            LicenciasContenedor.click(cargarLicencias);
            PolizasContenedor.click(cargarPolizas);
            DisponibilidadContenedor.click(cargarDisponibilidad);
            RendimientoContenedor.click(cargarRendimiento);
            RecepcionContenedor.click(cargarRecepcion1);
            CursoManejoContenedor.click(cargarCursoManejo);
            //notificacionesLicencias.click(cargarLicencias);
            //notificacionesPolizas.click(cargarPolizas);
            //notificacionesRecepcion.click(cargarRecepcion1);
            //notificacionesDisponibilidad.click(cargarDisponibilidad);
            btnLicencias.click(cargarLicencias);
            btnPolizas.click(cargarPolizas);
            btnRecepcion1.click(cargarRecepcion1);
            btnDisponibilidad.click(cargarDisponibilidad);
            btnRendimiento.click(cargarRendimiento);
            btnCursoManejo.click(cargarCursoManejo);

            btnEstadisticasLicencias.click(cargarEstadisticasLicencias);
            btnEstadisticasPolizas.click(cargarEstadisticasPolizas);
            btnEstadisticasDisponibilidad.click(cargarEstadisticasDisponibilidad);
            btnEstadisticasRendimiento.click(cargarEstadisticasRendimiento);
            btnEstadisticasRecepcion1.click(cargarEstadisticasRecepcion1);
            btnEstadisticasCursoManejo.click(cargarEstadisticasCursoManejo);
        }

        function getNotificaciones() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Home/getNotificacionesCount",
                data: {},
                asyn: false,
                success: function (response) {
                    $.unblockUI();
                    if (response.licencias > -1) {
                        notificacionesLicencias.text(response.licencias);
                        badgeLicencias.text(response.licencias);
                        badgeLicencias.css("background-color", "red");
                        if (response.licencias > 0) { btnEstadisticasLicencias.removeAttr("style"); }
                    }
                    if (response.polizas > -1) {
                        notificacionesPolizas.text(response.polizas);
                        badgePolizas.text(response.polizas);
                        badgePolizas.css("background-color", "red");
                        if (response.polizas > 0) { btnEstadisticasPolizas.removeAttr("style"); }
                    }
                    if (response.disponibilidad > -1) {
                        notificacionesDisponibilidad.text(response.disponibilidad);
                        badgeDisponibilidad.text(response.disponibilidad);
                        badgeDisponibilidad.css("background-color", "red");
                        if (response.disponibilidad > 0) { btnEstadisticasDisponibilidad.removeAttr("style"); }
                    }
                    if (response.rendimiento > -1) {
                        notificacionesRendimiento.text(response.rendimiento);
                        badgeRendimiento.text(response.rendimiento);
                        badgeRendimiento.css("background-color", "red");
                        if (response.rendimiento > 0) { btnEstadisticasRendimiento.removeAttr("style"); }
                    }
                    if (response.cursoManejo > -1) {
                        notificacionesCursoManejo.text(response.cursoManejo);
                        badgeCursoManejo.text(response.cursoManejo);
                        badgeCursoManejo.css("background-color", "red");
                        if (response.cursoManejo > 0) { btnEstadisticasCursoManejo.removeAttr("style"); }
                    }
                    recepcion = response.recepcion1;
                    if (response.recepcion2 != null) {
                        recepcion += response.recepcion2;
                        ulRecepcion.append("<li> <a href=\"#\"  id=\"btnRecepcion2\"onclick=\"javascript:cargarRecepcion2();\"> <i class=\"zmdi zmdi-notifications\"></i>" +
                            "Notificaciones Recepción TMC/Proveedor  <span class=\"badge\" id=\"badgeNotificacionesRecepcionM\">" + response.recepcion2 + "</span></a> </li>" +
                            "<li> <a href=\"#\" id=\"btnEstadisticasRecepcion2\"> <i class=\"zmdi zmdi-chart\"></i> Estadísticas </a> </li>");
                        document.getElementById("btnRecepcion2").addEventListener("click", cargarRecepcion2, false);
                        document.getElementById("btnEstadisticasRecepcion2").addEventListener("click", cargarEstadisticasRecepcion2, false);
                        document.getElementById("badgeNotificacionesRecepcionM").style = "background-color:red";
                    }
                    if (recepcion > -1) {
                        notificacionesRecepcion.text(recepcion);
                        badgeRecepcion.text(response.recepcion1);
                        badgeRecepcion.css("background-color", "red");
                        if (response.recepcion1 > 0) { btnEstadisticasRecepcion1.removeAttr("style"); }
                    }
                },
                error: function () {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Problemas en la consulta");
                }
            });
        }
        //function cargarLicencias()
        //{  
        //    $("#contenedor").html = 
        //    window.location.href = "/Home/Licencias";
        //}

        //function cargarPolizas()
        //{
        //    window.location.href = "/Home/Polizas";
        //}
        //function cargarDisponibilidad() {
        //    window.location.href = "/Home/Disponibilidad";
        //}
        //function cargarRendimiento() {
        //    window.location.href = "/Home/Rendimiento";
        //}
        //function cargarRecepcion1()
        //{
        //    window.location.href = "/Home/RecepcionMaquinaria";
        //}
        //function cargarRecepcion2() {
        //    window.location.href = "/Home/RecepcionProveedor";
        //}
        function cargarLicencias() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/Licencias");
        }
        function cargarPolizas() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/Polizas");
        }
        function cargarDisponibilidad() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/Disponibilidad");
        }
        function cargarRendimiento() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/Rendimiento");
        }
        function cargarRecepcion1() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/RecepcionMaquinaria");
        }
        function cargarRecepcion2() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/RecepcionProveedor");
        }
        function cargarCursoManejo() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/CursoManejo");
        }

        function cargarEstadisticasLicencias() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/EstadisticasLicencias");
        }
        function cargarEstadisticasPolizas() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/EstadisticasPolizas");
        }
        function cargarEstadisticasDisponibilidad() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/EstadisticasDisponibilidad");
        }
        function cargarEstadisticasRendimiento() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/EstadisticasRendimiento");
        }
        function cargarEstadisticasRecepcion1() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/EstadisticasRecepcionMaquinaria");
        }
        function cargarEstadisticasRecepcion2() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/EstadisticasRecepcionProveedor");
        }
        function cargarEstadisticasCursoManejo() {
            $("#contenedor").html("");
            $("#contenedor").load("/Home/EstadisticasCursoManejo");
        }

        init();
    };


    $(document).ready(function () {
        principal.home.dashboard = new dashboard();
    });
})();
