(function () {

    $.namespace('principal.home.polizas');

    polizas = function () {
        contenido = $("#contenido"),
        modalEditFecha = $("#modalEditFecha"),
        btnGuardarEditPoliza = $("#btnGuardarEditPoliza"),
        modalEditPoliza = $("#modalEditPoliza");
        mensajes = { PROCESANDO: 'Procesando...' };
        function init() {
            iniciarGrid();
            cargarNotificaciones();
            btnGuardarEditPoliza.click(GuardarPolizaActualizada);
        }

        function cargarNotificaciones() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Home/getNotificacionesPolizas",
                data: {},
                asyn: false,
                success: function (response) {
                    $.unblockUI();
                    contenido.bootgrid("clear");
                    notificaciones = response.polizas;
                    if (notificaciones.length > 0) {
                        contenido.bootgrid("append", notificaciones);
                    }
                },
                error: function () {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "No se encontró el usuario");
                }
            });
        }

        function iniciarGrid() {
            contenido.bootgrid({
                align: 'center',
                caseSensitive: false,
                formatters: {
                    "noEmpleado": function (column, row) {
                        return "<span class='noEmpleado'> " + row.noEmpleado + " </span>";
                    },
                    "ModificarPoliza": function (column, row) {
                        return "<button type='button' class='btn btn-primary editLicencia' data-id='" + row.id + "' data-toggle= 'tooltip' title='Modificar Poliza " + row.nombEmpleado + "'>" +
                            "<span class='zmdi zmdi-edit' ></span> " +
                                   " </button>";
                    },
                    "diasVencimientoPoliza": function (column, row) {
                        return "<span class='diasVencimiento'> " + row.diasVencimientoPoliza + " </span>";
                    },
                    "nombEmpleado": function (column, row) {
                        return "<span class='nombEmpleado'> " + row.nombEmpleado + " </span>";
                    },
                    "Obra": function (column, row) {
                        return "<span class='Obra'> " + row.Obra + " </span>";
                    },
                    "Kilometraje": function (column, row) {
                        return "<span class='Kilometraje'> " + row.Kilometraje + " </span>";
                    },
                    "Placas": function (column, row) {
                        if (row.Placas == "") { return "<span class='Placas'> -- </span>"; }
                        else { return "<span class='Placas'> " + row.Placas.toUpperCase() + " </span>"; }
                    },
                    "fecha": function (column, row) {
                        return "<span class='fecha'> " + row.fechaVencimientoPoliza + " </span>";
                    },
                    "maquinaNoEconomico": function (column, row) {
                        return "<span class='maquinaNoEconomico'> " + row.maquinaNoEconomico + " </span>";
                    },
                    "maquina": function (column, row) {
                        return "<span class='maquina'> " + row.maquina + " </span>";
                    }                
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                contenido.find("th").css("text-align", "center");
                contenido.find("th").css("color", "white");
                contenido.find("td").css("text-align", "center");
                contenido.find("td").css("font-size", "15px");

                contenido.find(".noEmpleado").parent().css("text-align", "left");
                contenido.find(".diasVencimiento").parent().css("text-align", "right");
                contenido.find(".nombEmpleado").parent().css("text-align", "left");
                contenido.find(".Obra").parent().css("text-align", "left");
                contenido.find(".Kilometraje").parent().css("text-align", "right");
                contenido.find(".maquinaNoEconomico").parent().css("text-align", "left");
                contenido.find(".maquina").parent().css("text-align", "left");
                $('[data-toggle="tooltip"]').tooltip();
                contenido.find(".editPoliza").on("click", function (e) {
                    var idResguardo = Number($(this).attr('data-id'));
                    modalEditFecha.datepicker().datepicker("setDate", new Date());

                    btnGuardarEditPoliza.removeAttr('data-idResguardo');
                    btnGuardarEditPoliza.attr('data-idResguardo', idResguardo);
                    modalEditPoliza.modal('show');
                });
            });
        }

        function GuardarPolizaActualizada() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/ResguardoEquipo/EditarPoliza',
                type: "POST",
                datatype: "json",
                data: { obj: btnGuardarEditPoliza.attr('data-idResguardo'), fechaVencimiento: modalEditFecha.val() },
                success: function (response) {
                    $.unblockUI();
                    modalEditFecha.val('');
                    btnGuardarEditPoliza.removeAttr('data-idResguardo');
                    modalEditPoliza.modal('hide');
                    cargarNotificaciones();
                    AlertaGeneral('Alerta', 'El registro fue actualizado correctamente');
                },
                error: function () {
                    $.unblockUI();
                    AlertaGeneral('Alerta', 'Problemas en la actualización del registro');
                }
            });
        }

        init();
    };

    $(document).ready(function () {
        principal.home.polizas = new polizas();
    });
})();
