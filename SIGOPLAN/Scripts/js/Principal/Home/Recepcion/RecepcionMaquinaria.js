(function () {

    $.namespace('principal.home.recepcionmaquinaria');

    recepcionmaquinaria = function () {
        contenido = $("#contenido");
        mensajes = {
            NOMBRE: 'Notificaciones Recepción Mauqinaria',
            PROCESANDO: 'Procesando...'
        };
        function init() {
            iniciarGrid();
            cargarNotificaciones();
            $(".tipoRecepcion").change(cargarTipoRecepcion);
        }

        function cargarNotificaciones() {
            $(".tipoRecepcion").val(1);
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/ControlCalidad/GetMaquinariasPendientesRecepcion",
                asyn: false,
                success: function (response) {
                    $.unblockUI();
                    contenido.bootgrid("clear");
                    notificaciones = response.EquiposPendientes;
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
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/ControlCalidad/GetMaquinariasPendientesRecepcion",
                asyn: false,
                success: function (response) {
                    $.unblockUI();
                    if (response.EquiposPendientes != null) { $(".tipoRecepcion").prop('disabled', false) }
                    else { $(".tipoRecepcion").prop('disabled', true) }
                },
                error: function () {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Error en la consulta");
                }
            });


            contenido.bootgrid({
                align: 'center',
                caseSensitive: false,
                formatters: {
                    "VerControlCalidad": function (column, row) {                        
                        if (row.estatus == 9) {
                            return "<button type='button' class='btn btn-danger AlertaResguardo'>" +
                                "<span class='zmdi zmdi-eye'></span>  </button>";
                        }
                        else {
                            return "<button type='button' class='btn btn-primary VerControlCalidad' data-reporte='" + row.reporte + "' data-idAsigacion='" + row.idAsigancion + "' data-idEconomico='" + row.Economico + "' data-html='true' data-toggle= 'tooltip' title='Ir a control recepción:<br/>" + row.Folio + "'>" +
                                "<span class='zmdi zmdi-eye'></span>  </button>";
                        }
                    },
                    "CCNombre": function (column, row) {
                        if (row.CCName == "") { return "<span class='CCNombre'> " + row.cc + " </span>"; }
                        else { return "<span class='CCNombre'> " + row.CCName + " </span>"; }
                    },
                    "nomb": function (column, row) {
                        return "<span class='nomb'> " + row.nomb + " </span>";
                    },
                    "fechaFormato": function (column, row) {
                        return "<span class='fecha'> " + row.Fecha + " </span>";
                    },
                    "CCOrigen": function (column, row) {
                        if (row.CCOrigen == "") { return "<span class='CCOrigen'> " + row.ccor + " </span>"; }
                        else { return "<span class='CCOrigen'> " + row.CCOrigen + " </span>"; }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                contenido.find("th").css("text-align", "center");
                contenido.find("th").css("color", "white");
                contenido.find("td").css("text-align", "center");
                contenido.find("td").css("font-size", "15px");
                contenido.find(".CCNombre").parent().css("text-align", "left");
                contenido.find(".CCOrigen").parent().css("text-align", "left");
                contenido.find(".nomb").parent().css("text-align", "left");
                $('[data-toggle="tooltip"]').tooltip();
                contenido.find(".VerControlCalidad").on('click', function (e) {
                    var idAsignacion = $(this).attr('data-idAsigacion');
                    var CCal = $(this).attr('data-reporte');
                    if ($(this).attr('data-idEconomico') != 0) {
                        window.location = "/ControlCalidad/Preguntas?obj=" + idAsignacion + "&Tipo=2&CCal=" + CCal + "";
                    }
                    else {
                        //GetEconomicosNoAsignados(idAsignacion);
                    }
                });
                contenido.find(".AlertaResguardo").on('click', function (e) {
                    AlertaGeneral("Alerta", "¡El equipo tiene un resguardo activo!<br/>Se debe liberar el equipo para poder hacer un control de envio");
                });
            });
        }

        //function GetEconomicosNoAsignados(idAsignacion) {
        //    $.blockUI({ message: mensajes.PROCESANDO });
        //    $.ajax({
        //        url: "/MovimientoMaquinaria/GetEconomicosNoAsignados",
        //        type: "POST",
        //        datatype: "json",
        //        data: { idAsignacion: idAsignacion },
        //        success: function (response) {
        //            $.unblockUI();
        //            var ListaEconomicos = response.ListaEconomicos;
        //            //Agregar las referencias
        //            tbGrupoMaquinariaModal.val(response.GrupoMaquinaria)
        //            tbTipoMaquinariaModal.val(response.TipoMaquinaria);
        //            tbModeloMaquinariaModal.val(response.ModeloMaquinaria);
        //            tbHorasModal.val(response.Horas);

        //            if (ListaEconomicos != null) {

        //                tblEconomicosNoAsignados.bootgrid("clear");
        //                tblEconomicosNoAsignados.bootgrid("append", ListaEconomicos);
        //                modalListaEquiposAsignados.modal('show');

        //            }
        //        },
        //        error: function () {
        //            $.unblockUI();
        //        }
        //    });
        //}

        function cargarTipoRecepcion() {
            if ($(".tipoRecepcion").val() == 1) {
                $("#contenedor").html("");
                $("#contenedor").load("/Home/RecepcionMaquinaria");
            }
            else {
                if ($(".tipoRecepcion").val() == 2) {
                    $("#contenedor").html("");
                    $("#contenedor").load("/Home/RecepcionProveedor");
                }
            }
        }

        init();
    };


    $(document).ready(function () {
        principal.home.recepcionmaquinaria = new recepcionmaquinaria();
    });
})();