
(function () {

    $.namespace('maquinaria.Desautorizacion');

    Desautorizacion = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Desautorizar Solicitudes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        btnEliminarRegistro = $("#btnEliminarRegistro"),
        modalEliminarRegistro = $("#modalEliminarRegistro"),
        ireport = $("#report")
        tblSolicitudesPendientes = $("#tblSolicitudesPendientes");

        function init() {

            loadTabla();
            btnEliminarRegistro.click(EliminarDetalle);
        }

        function loadTabla() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/DesAutorizaciones/GetSolicitudes",
                type: "POST",
                datatype: "json",
                success: function (response) {
                    $.unblockUI();
                    var data = response.DataTables;
                    tblSolicitudesPendientes.bootgrid("clear");
                    tblSolicitudesPendientes.bootgrid("append", data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function initGrid() {

            tblSolicitudesPendientes.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: "",
                },
                formatters: {
                    "VerSolicitud": function (column, row) {

                        return "<button type='button' class='btn btn-primary btnReporte' data-id='" + row.id + "' data-cc='" + row.cc + "'>" +
                                    "<span class='glyphicon glyphicon glyphicon-print'></span> " +
                                  " </button>";
                    },
                    "Cancelacion": function (column, row) {
                        return "<button type='button' class='btn btn-danger btnDesautorizar' data-id='" + row.id + "' data-cc='" + row.cc + "'>" +
                                    "<i class='fa fa-unlock' aria-hidden='true'></i> " +
                                " </button>";

                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */

                tblSolicitudesPendientes.find(".btnReporte").on("click", function (e) {
                    idSolicitud = $(this).attr('data-id');
                    CC = $(this).attr('data-cc');

                    LoadReporte(idSolicitud, CC);
                });

                tblSolicitudesPendientes.find(".btnDesautorizar").on("click", function (e) {
                    idSolicitud = $(this).attr('data-id');
                    btnEliminarRegistro.attr('data-id', idSolicitud);
                    modalEliminarRegistro.modal('show');
                });

            });
        }

        function LoadReporte(idSolicitud, CC) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/GetReporte',
                type: "POST",
                datatype: "json",
                data: { obj: idSolicitud },
                success: function (response) {

                    var idReporte = response.idReporte;
                    var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pCC=" + CC;
                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();

                    };

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function EliminarDetalle() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/DesAutorizaciones/SetDeshabilitada',
                type: "POST",
                datatype: "json",
                data: { idSolicitud: btnEliminarRegistro.attr('data-id') },
                success: function (response) {
                    modalEliminarRegistro.modal('hide');

                    var Result = response.success;

                    if (Result)
                    {
                        ConfirmacionGeneral("Confirmación", "El registro fue eliminado correctamente", "bg-green");
                        loadTabla();
                    }
                    else
                    {
                        loadTabla();
                        ConfirmacionGeneral("Confirmación", "El registro no se pudo eliminar.", "bg-red");
                    }
                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        init();
        initGrid();
    };

    $(document).ready(function () {

        maquinaria.Desautorizacion = new Desautorizacion();
    });
})();

