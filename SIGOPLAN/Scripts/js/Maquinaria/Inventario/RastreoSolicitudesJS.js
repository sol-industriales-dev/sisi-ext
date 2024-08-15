
(function () {

    $.namespace('maquinaria.inventario.Rastreos');

    Rastreos = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };


        mensajes = {
            NOMBRE: 'Rastreo de solicitudes equipo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };


        tbFolio = $("#tbFolio")
        tbEconomico = $("#tbEconomico"),
        cboEstados = $("#cboEstados"),
        btnBuscar = $("#btnBuscar"),
        ListaTemporale = "";
        tblRastreoAsignados = $("#tblRastreoAsignados");

        function init() {
            LoadInfoTable();          
            btnBuscar.click(LoadInfoTable);
        }

        function LoadInfoTable() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Rastreos/tblPrincipal',
                data: { JsonData: ListaTemporale, Folio: tbFolio.val(), Economico: tbEconomico.val(), Estado: cboEstados.val() },
                success: function (response) {
                    var LoadLista = response.ListaEstatus;

                    tblRastreoAsignados.bootgrid("clear");
                    if (LoadLista != undefined) {
                        tblRastreoAsignados.bootgrid("append", LoadLista);
                    }
                    tblRastreoAsignados.bootgrid('reload');

                    //ListaTemporale = response.dataSetJson;
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function ReloadInfotable() {
            // $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Rastreos/ActualizarTabla',
                type: 'POST',
                dataType: 'json',
                data: { JsonData: ListaTemporale , Folio : tbFolio.val(), Economico:tbEconomico.val() , Estado:cboEstados.val()},
                success: function (response) {

                    if (response.success == true) {
                        var LoadLista = response.ListaEstatus;
                        tblRastreoAsignados.bootgrid("clear");
                        if (LoadLista != undefined) {
                            tblRastreoAsignados.bootgrid("append", LoadLista);
                        }
                        tblRastreoAsignados.bootgrid('reload');

                        ListaTemporale = response.dataSetJson;
                    }

                    //  $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }



        function initGrid() {
            tblRastreoAsignados.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "Rastreo": function (column, row) {
                        var barEstatus1 = "";
                        var barEstatus2 = "";
                        var barEstatus3 = "";
                        var barEstatus4 = "";
                        var barEstatus5 = "";

                        if (row.idEconomico != 0) {
                            if (row.Estatus >= 1 && row.Estatus <= 5) {
                                barEstatus1 = " <div class='progress-bar progress-bar-Asignacion' role='progressbar' style='width:10%;' title='Asignación'> " +
                                                       "&nbsp;&nbsp;" +
                                                        " </div> ";
                            }
                            if (row.Estatus >= 2 && row.Estatus <= 5) {
                                barEstatus2 = " <div class='progress-bar progress-bar-CalidadEnvio' role='progressbar' style='width:10%;' title='Calidad envío'> " +
                                                "&nbsp;&nbsp;"+   //     "Calidad envío" +
                                                        " </div> ";
                            }
                            if (row.Estatus >= 3 && row.Estatus <= 5) {
                                barEstatus3 = " <div class='progress-bar progress-bar-CalidadEnvio' role='progressbar' style='width:10%;' title='Control envío'> " +
                                                 "&nbsp;&nbsp;" +      //     "Control envío" +
                                                        " </div> ";
                            }
                            if (row.Estatus >= 4 && row.Estatus <= 5) {
                                barEstatus4 = " <div class='progress-bar progress-bar-CalidadRecepcion' role='progressbar' style='width:10%;' title='Calidad recepción'> " +
                                                 "&nbsp;&nbsp;" +      //     "Calidad recepción" +
                                                        " </div> ";
                            }
                            if (row.Estatus >= 5 && row.Estatus <= 5) {
                                barEstatus5 = " <div class='progress-bar progress-bar-CalidadRecepcion' role='progressbar' style='width:10%;' title='Control recepción'> " +
                                                "&nbsp;&nbsp;" +          // "Control recepción" +
                                                        " </div> ";
                            }

                            if (row.Estatus >= 6 && row.Estatus <= 9) {
                                barEstatus1 = " <div class='progress-bar progress-bar-CalidaEnvioTMC' role='progressbar' style='width:10%;' title='Calidad Envío TMC'> " +
                                                 "&nbsp;&nbsp;" +          // "Calida Envío TMC" +
                                                        " </div> ";
                                //barEstatus2 += " <div class='progress-bar progress-bar-info' role='progressbar' style='width:10%;' title='Asignación'> " +
                                //              "&nbsp;&nbsp;" +     // "Calidad envío TMC" +
                                //                " </div> ";
                            }
                            if (row.Estatus >= 7 && row.Estatus <= 9) {
                                barEstatus3 = " <div class='progress-bar progress-bar-CalidaEnvioTMC' role='progressbar' style='width:10%;' title='Control Envío TMC'> " +
                                            "&nbsp;&nbsp;" +            //    "Control envío TMC" +
                                                        " </div> ";
                            }
                            if (row.Estatus >= 8) {
                                barEstatus4 = " <div class='progress-bar progress-bar-CalidaRecepcionTMC' role='progressbar' style='width:10%;' title='Calidad recepción TMC'> " +
                                              "&nbsp;&nbsp;" +           //   "Calidad recepción TMC" +
                                                        " </div> ";
                                barEstatus5 = " <div class='progress-bar progress-bar-CalidaRecepcionTMC' role='progressbar' style='width:10%;' title='Recepción Envío TMC'> " +
                                               "&nbsp;&nbsp;" +       //     "Recepción Envío TMC" +
                                                       " </div> ";
                            }
                            if (row.Estatus == 0) {
                                barEstatus1 = " <div class='progress-bar progress-bar-AsignacionPendiente' role='progressbar' style='width:100%;' title='Asignación Pendiente'> " +
                                             "&nbsp;&nbsp;" +       //       "Asignación Pendiente" +
                                                       " </div> ";
                            }
                        }
                        else {
                            barEstatus1 = " <div class='progress-bar progress-bar-PendienteCompraRenta' role='progressbar' style='width:100%;' title='En espera de compras'> " +
                                                "&nbsp;&nbsp;" +         //  "En espera de compras" +
                                                       " </div> ";
                        }


                        return barEstatus1 + barEstatus2 + barEstatus3 + barEstatus4 + barEstatus5;



                    },
                    "Estatus": function (column, row) {

                        switch (row.Estatus) {
                            case 1:
                                return "Asignación";
                            case 2:
                                return "Calida Envío";
                            case 3:
                                return "Control Envío";
                            case 4:
                                return "Calidad Recepción";
                            case 5:
                                return "Control Recepción";
                            case 6:
                                return "Calida Envío TMC";
                            case 7:
                                return "Control Envío TMC";
                            case 8:
                                return "Calidad Recepción TMC";
                            case 9:
                                return "Recepción Envío TMC";
                            default:
                                return "";

                        }
                    }
                }
            });
        }


        init();
        initGrid();
    };

    $(document).ready(function () {

        maquinaria.inventario.Rastreos = new Rastreos();
    });
})();

