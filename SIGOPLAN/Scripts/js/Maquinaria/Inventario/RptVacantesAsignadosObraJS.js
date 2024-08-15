(function () {

    $.namespace('maquinaria.Rastreos.RptVacantesAsignados');

    RptVacantesAsignados = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Autorizacion de Solicitudes Reemplazo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        BntRegresar = $("#BntRegresar"),
        divSolDetalle = $("#divSolDetalle"),
        divSolGrupo = $("#divSolGrupo"),
        tblVistaSolicitudes = $("#tblVistaSolicitudes"),
        modalDetalleSolicitud = $("#modalDetalleSolicitud"),
        tblVistaSolicitudesDet = $("#tblVistaSolicitudesDet"),
        cboListaCC = $("#cboListaCC");
        tblVistaSolicitudesDet = $("#tblVistaSolicitudesDet").DataTable({});

        function init() {

            BntRegresar.click(regresar);
            cboListaCC.fillCombo('/MovimientoMaquinaria/cboGetCentroCostos');
            cboListaCC.change(loadTabla);

            $("#tblVistaSolicitudes").DataTable({
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

        }
        function regresar() {
            divSolDetalle.removeClass('hide');
            divSolGrupo.addClass('hide');
        }

        function loadTabla() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Rastreos/RptVacantesAsignados",
                type: "POST",
                datatype: "json",
                data: { CentroCostos: cboListaCC.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.DataTables;
                    SetDataInTables(data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SetDataInTablesDet(dataSet) {
            tblVistaSolicitudesDet = $("#tblVistaSolicitudesDet").DataTable({
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
                                              data: "Tipo"
                                          },
                      {
                          data: "Grupo"
                      },

                                            {
                                                data: "Modelo"
                                            },
                      {
                          data: "CantidadSolicitudes"
                      },
                      {
                          data: "CantidadVacantes"
                      },
                      {
                          data: "Solicitud", "width": "85px",
                          createdCell: function (td, cellData, rowData, row, col) {
                              var BtnControl = "<button type='button' class='btn btn-primary btnVerDetalle' data-id='" + cellData + "' data-grupo='" + rowData.IDGrupo + "' >" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                                   " </button>";
                              $(td).text('');
                              $(td).append(BtnControl);
                          }
                      }
                ],
                "paging": true,
                "info": false

            });
            modalDetalleSolicitud.modal("show");


        }

        function SetDataInTables(dataSet) {
            tblVistaSolicitudes = $("#tblVistaSolicitudes").DataTable({
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
                select: true,
                scrollY: '50vh',
                data: dataSet,
                columns: [

                      {
                          data: "CentroCostos"
                      },
                      {
                          data: "Folio"
                      },
                      {
                          data: "TotalDeVacantes"
                      },
                      {
                          data: "TotalOcupadas"
                      },
                      {
                          data: "TotalLibres"
                      },
                      {
                          data: "noSolicitudes", "width": "85px",
                          createdCell: function (td, cellData, rowData, row, col) {
                              var BtnControl = "<button type='button' class='btn btn-primary btnVer' data-id='" + cellData + "' >" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                                   " </button>";
                              $(td).text('');
                              $(td).append(BtnControl);
                          }
                      }
                ],
                "paging": false,
                "info": false

            });
        }

        function ViewDetalle(idSolicitud) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Rastreos/RptDetalleVacantes",
                type: "POST",
                datatype: "json",
                data: { idSolicitud: idSolicitud },
                success: function (response) {
                    $.unblockUI();
                    var data = response.DataTables;
                    SetDataInTablesDet(data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        $('#modalDetalleSolicitud').on('shown.bs.modal', function () {
            tblVistaSolicitudesDet.draw();
        });

        function ViewDetalleGrupotbl(dataSet) {
            tblVistaSolicitudesGrupo = $("#tblVistaSolicitudesGrupo").DataTable({
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
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [

                      {
                          data: "Tipo"
                      },
                      {
                          data: "Descripcion"
                      },
                      {
                          data: "Economico"
                      }
                ],
                "paging": true,
                "info": false

            });

        }

        $(document).on('click', ".btnVer", function () {
            regresar();
            var idSolicitud = $(this).attr('data-id');
            ViewDetalle(idSolicitud);

        });

        $(document).on('click', ".btnVerDetalle", function () {
            var idSolicitud = $(this).attr('data-id');
            var idGrupo = $(this).attr('data-grupo');

            ViewDetalleGrupo(idSolicitud, idGrupo);

        });


        function ViewDetalleGrupo(idSolicitud, idGrupo) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Rastreos/RptDetalleVacantesGrupo",
                type: "POST",
                datatype: "json",
                data: { idSolicitud: idSolicitud, idGrupo: idGrupo },
                success: function (response) {
                    $.unblockUI();
                    var data = response.DataTables;
                    ViewDetalleGrupotbl(data);

                    divSolDetalle.addClass('hide');
                    divSolGrupo.removeClass('hide');
                    tblVistaSolicitudesGrupo.draw();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }


        //btnVerDetalle
        init();

    };

    $(document).ready(function () {

        maquinaria.Rastreos.RptVacantesAsignados = new RptVacantesAsignados();
    });
})();
