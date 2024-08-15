
(function () {

    $.namespace('maquinaria.Rastreo.SolicitudesPendientesSurtir');

    SolicitudesPendientesSurtir = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Solicitud de equipo pendiente por surtir',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        cboTipo = $("#cboTipo");
        txtCentroCostos = $("#txtCentroCostos");
        cboModalGrupoMaquinaria = $("#cboModalGrupoMaquinaria");

        function init() {
            txtCentroCostos.fillCombo('/CatObra/cboCentroCostosUsuarios');
            txtCentroCostos.change(loadTabla);
            cboTipo.fillCombo('/CatMaquina/FillCboTipo_Maquina', { estatus: true });
            cboTipo.change(FillCboGrupo);
            cboModalGrupoMaquinaria.change(loadTabla)
            loadTabla();

        }

        function loadTabla() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Rastreos/tblSolicitudesEquipoPendientesAsignacion",
                type: "POST",
                datatype: "json",
                data: { cc: txtCentroCostos.val(), Tipo: cboTipo.val() == "" ? 0 : cboTipo.val(), grupo: cboModalGrupoMaquinaria.val() == undefined ? 0 : cboModalGrupoMaquinaria.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.DataTables;
                    SetDataInTables(data);

                    var data2 = response.DataTables2;
                    SetDataInTablesRemp(data2);

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function FillCboGrupo() {
            if (cboTipo.val() != "") {
                cboModalGrupoMaquinaria.fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: cboTipo.val() });

                cboModalGrupoMaquinaria.attr('disabled', false);
            }
            else {
                cboModalGrupoMaquinaria.clearCombo();
                cboModalGrupoMaquinaria.attr('disabled', true);
            }

        }

        function SetDataInTablesRemp(dataSet) {
            tblSolicitudEquipoReGrid = $("#tblSolicitudEquipoRe").DataTable({
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
                "bFilter": false,
                "order": false,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [

                      {
                          data: "Proyecto"
                      },
                      {
                          data: "GrupoEquipo"
                      },
                      {
                          data: "Modelo"
                      },
                       {
                           data: "economicoAsignado"
                       },
                      {
                          data: "Prioridad"
                      },
                      {
                          data: "EquipoPropio"
                      },
                      {
                          data: "EquipoRenta"
                      },
                      {
                          data: "FechaSolicitud"
                      },
                      {
                          data: "FechaRequerida"
                      },
                      {
                          data: "Estatus"
                      }
                ],
                "paging": true,
                "info": false

            });

        }
        function SetDataInTables(dataSet) {
            tblSolicitudEquipoGrid = $("#tblSolicitudEquipo").DataTable({
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
                "bFilter": false,
                "order": false,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [

                      {
                          data: "Proyecto"
                      },
                      {
                          data: "GrupoEquipo"
                      },
                      {
                          data: "Modelo"
                      },
                       {
                           data: "Cantidad"
                       },
                      {
                          data: "Prioridad"
                      },
                      {
                          data: "EquipoPropio"
                      },
                      {
                          data: "EquipoRenta"
                      },
                      {
                          data: "FechaSolicitud"
                      },
                      {
                          data: "FechaRequerida"
                      },
                      {
                          data: "Estatus"
                      },

                ],
                "paging": true,
                "info": false

            });

        }

        init();
    };

    $(document).ready(function () {

        maquinaria.Rastreo.SolicitudesPendientesSurtir = new SolicitudesPendientesSurtir();
    });
})();

