
(function () {

    $.namespace('maquinaria.Inventario.VisorSolicitudesJS');

    VisorSolicitudesJS = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Visor de solicitudes.',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        BntRegresar = $("#BntRegresar"),
        ireport = $("#report"),
        tblSolicitudesDetalle = $("#tblSolicitudesDetalle"),
        tblSolcitudesAprobadas = $("#tblSolcitudesAprobadas"),
        btnBuscar = $("#btnBuscar"),
        cboCC = $("#cboCC");


        function init() {

            cboCC.fillCombo('/CatInventario/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");
            tblSolcitudesAprobadas = $("#tblSolcitudesAprobadas").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
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
                }
            });
            BntRegresar.click(regresar);
            btnBuscar.click(loadTabla);
        }

        function regresar() {
            $("#divtblDetalle").addClass('hide');
            $("#divtblGeneral").removeClass('hide');
        }

        function loadTabla() {
            regresar();
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/SolicitudEquipo/GetlistaSolicitudes",
                type: "POST",
                datatype: "json",
                data: { CentroCostos: getValoresMultiples("#cboCC") },
                success: function (response) {
                    $.unblockUI();
                    var data = response.DataSend;
                    SetDataInTables(data);

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SetDataInTables(dataSet) {
            tblSolcitudesAprobadas = $("#tblSolcitudesAprobadas").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
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
                responsive: true,
                "bFilter": true,
                "order": false,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [

                      {
                          data: "Folio"
                      },
                      {
                          data: "CentroCostosName"
                      },
                      {
                          data: "UsuarioElabora"
                      },
                       {
                           data: "FechaCreacion"
                       },
                      {
                          data: "Estatus"
                      },
                      {
                          data: "btnDetalle"
                      }
                ],
                "paging": true,
                "info": false,
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Reporte de Solicitudes", "<center><h3>Reporte de Solicitudes </h3></center>"),
                buttons: [
                    {
                        extend: 'excel',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4] //Your Colume value those you want
                        }
                    }
                ]

            });

        }
        init();

    };

    $(document).ready(function () {

        maquinaria.Inventario.VisorSolicitudesJS = new VisorSolicitudesJS();
    });
})();


function GetDetalle(id) {
    registroActual = id;
    $.ajax({
        url: '/SolicitudEquipo/GetDetalleSolicitudes',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ id: registroActual }),
        success: function (response) {

            $("#divtblDetalle").removeClass('hide');
            $("#divtblGeneral").addClass('hide');
            var data = response.data;
            SetDataInTables2(data);

        },
        error: function (response) {
            AlertaGeneral("Error", response.message);
        }
    });

}

function SetDataInTables2(dataSet) {
    $("#tblSolicitudesDetalle").DataTable({
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de MAX registros)",
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
        responsive: true,
        "bFilter": true,
        "order": false,
        destroy: true,
        scrollY: '50vh',
        scrollCollapse: true,
        data: dataSet,
        columns: [

              {
                  data: "Economico"
              },
              {
                  data: "Equipo"
              },
              {
                  data: "FechaPromesa"
              },
              {
                  data: "btnCalidadEnvio"
              },
              {
                  data: "btnControlEnvio"
              },
              {
                  data: "btnCalidadRecepcion"
              },
              {
                  data: "btnControlRecepcion"
              },
        ],
        "paging": true,
        "info": false

    });



}

function LoadReporte(idSolicitud, CC) {
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: '/SolicitudEquipo/GetReporte2',
        type: "POST",
        datatype: "json",
        data: { obj: idSolicitud },
        success: function (response) {
            var path = "/Reportes/Vista.aspx?idReporte=12&pCC=" + CC;
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



//function GetReporteCalidad(asignacion, tipo) {
//    verReporteCalidad(22, "idAsignacion=" + asignacion + "&" + "TipoControl=" + tipo);
//}

function verReporteCalidad(idReporte, parametros) {

    $.blockUI({ message: mensajes.PROCESANDO });

    var idReporte = idReporte;

    var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&" + parametros;
    ireport.attr("src", path);

    document.getElementById('report').onload = function () {
        $.unblockUI();
        openCRModal();
    };
}

function GetReporteCalidad(asignacion, tipo) {
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: '/ControlCalidad/GETidControlCalidad',
        type: 'POST',
        dataType: 'json',
        data: { asignacionID: asignacion, TipoControl: tipo, solicitudID: 0 },
        success: function (response) {
            var id = response.ControlExiste;

            if (id ) {
                verReporteCalidad(22, "idAsignacion=" + asignacion + "&" + "TipoControl=" + tipo);
            }
            else {
                AlertaGeneral("Alerta", "No se encontro el control del equipo seleccionado.");
            }

            $.unblockUI();
        },
        error: function (response) {
            $.unblockUI();
            AlertaGeneral("Alerta", response.message);
        }
    });
};


function GetReporteControl(idAsignacion, TipoControl, solicitudID) {
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: '/ControlCalidad/GetIDControl',
        type: 'POST',
        dataType: 'json',
        data: { asignacionID: idAsignacion, TipoControl: TipoControl, solicitudID: solicitudID },
        success: function (response) {
            var id = response.ControlID;

            if (id != 0)
            {
                var tipoControl = TipoControl;
                var path = "/Reportes/Vista.aspx?idReporte=13&pidRegistro=" + id + "&ptipoControl=" + tipoControl;
                ireport.attr("src", path);
                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
            }
            else {
                AlertaGeneral("Alerta", "No se encontro el control del equipo seleccionado.");
            }
           
            $.unblockUI();
        },
        error: function (response) {
            $.unblockUI();
            AlertaGeneral("Alerta", response.message);
        }
    });
};