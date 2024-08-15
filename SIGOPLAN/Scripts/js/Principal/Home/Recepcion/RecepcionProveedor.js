(function () {

    $.namespace('principal.home.recepcionproveedor');

    recepcionproveedor = function () {
        contenido = $("#contenido");
        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        function init() {
            iniciarSelect();
            iniciarGrid();
            cargarNotificaciones();
            $(".tipoRecepcion").change(cargarTipoRecepcion);
        }

        function cargarNotificaciones() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/ControlCalidad/GetMaquinariasPendientesEnvios",
                data: { obj: 4, tipoFiltro: 1 },
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

        function iniciarSelect() {
            $(".tipoRecepcion").val(2);
        }

        function iniciarGrid() {

            contenido.bootgrid({
                align: 'center',
                caseSensitive: false,
                formatters: {
                    "VerControlCalidad": function (column, row) {
                        if (row.estatus == 4 || row.estatus - 1 == 4) {
                            return "<button type='button' class='btn btn-primary VerControlCalidad' data-reporte='" + row.reporte + "' data-idAsigacion='" + row.idAsigancion + "' data-idEconomico='" + row.Economico + "' data-html='true' data-toggle= 'tooltip' title='Ir a control recepción:<br/>" + row.Folio + "'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span>  </button>";
                        }
                        if (row.estatus == 3 && row.isrenta == true) {
                            return "<button type='button' class='btn btn-primary VerControlCalidad' data-reporte='" + row.reporte + "' data-idAsigacion='" + row.idAsigancion + "' data-idEconomico='" + row.Economico + "' data-html='true' data-toggle= 'tooltip' title='Ir a control recepción:<br/>" + row.Folio + "'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span>  </button>";
                        }
                        if (row.estatus == 1 && row.isrenta == true) {
                            return "<button type='button' class='btn btn-primary VerControlCalidad' data-reporte='" + row.reporte + "' data-idAsigacion='" + row.idAsigancion + "' data-idEconomico='" + row.Economico + "' data-html='true' data-toggle= 'tooltip' title='Ir a control recepción:<br/>" + row.Folio + "'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span>  </button>";
                        }
                        if (row.estatus == 7) {
                            return "<button type='button' class='btn btn-primary VerControlCalidad' data-reporte='" + row.reporte + "' data-idAsigacion='" + row.idAsigancion + "' data-idEconomico='" + row.Economico + "' data-html='true' data-toggle= 'tooltip' title='Ir a control recepción:<br/>" + row.Folio + "'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span>  </button>";
                        }
                        if (row.estatus == 9) {
                            return "<button type='button' class='btn btn-danger AlertaResguardo'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span>  </button>";
                        }
                    },
                    "CCNombre": function (column, row) {
                        if (row.CCName == "") { return "<span class='CCNombre'> " + row.cc + " </span>"; }
                        else { return "<span class='CCNombre'> " + row.CCName + " </span>"; }
                    },
                    "nomb": function (column, row) {
                        return "<span class='CCName'> " + row.nomb + " </span>";
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
                        window.location = "/ControlCalidad/Preguntas?obj=" + idAsignacion + "&Tipo=4&CCal=" + CCal + "";
                    }
                    else {
                        //GetEconomicosNoAsignados(idAsignacion);
                    }
                });
            });
        }

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
        principal.home.recepcionproveedor = new recepcionproveedor();
    });
})();