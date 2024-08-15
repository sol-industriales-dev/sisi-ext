(function () {

    $.namespace('maquinaria.overhaul.reportesremocion');

    reportesremocion = function () {
        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        tab = $.urlParam('tab');
        gridReportes = $("#gridReportes"),
        fsReporteRemocion = $("#fsReporteRemocion"),
        modalReporteRemocion = $("#modalReporteRemocion"),
        titleModal = $("#title-modal"),
        ireport = $("#report"),
        botonAprobar = $("#dialogalertaGeneral .ui-button"),
        cboEstatusReporte = $("#cboEstatusReporte"),
        txtFiltroDescripcionComponenteRR = $("#txtFiltroDescripcionComponenteRR"),
        btnBuscarRR = $("#btnBuscarRR"),
        txtFiltroEconomicoRR = $("#txtFiltroEconomicoRR"),
        cboCCRR = $("#cboCCRR"),
        cboMotivoRemocion = $("#cboMotivoRemocion"),
        tabRemociones = $("#tabRemociones"),
        btnReporteRR = $("#btnReporteRR"),
        modalCorreosRR = $("#modalCorreosRR"),
        tblCorreosRR = $("#tblCorreosRR"),
        btnAgregarCorreoRR = $("#btnAgregarCorreoRR"),
        btnEnviarCorreo = $("#btnEnviarCorreo");
        let idReporteRR = 0;
        let banderaRR = 0;
        let ultimoIDCorreo = 0;
        let tipoUsuario = 6;

        function init() {            
            initGridRR();
            cboCCRR.select2();
            cboMotivoRemocion.select2();
            cboEstatusReporte.select2();

            cboCCRR.fillCombo('/CatComponentes/FillCbo_CentroCostos');            
            tabRemociones.click(cargarGridRR);
            if (tab == 1) { tabRemociones.click(); }
            cboEstatusReporte.change(cargarGridRR);
            btnBuscarRR.click(cargarGridRR);
            btnReporteRR.click(abrirReportePorFiltros);            
            txtFiltroDescripcionComponenteRR.getAutocomplete(SelectSubconjuntoRR, null, '/Overhaul/getSubConjuntos');
            txtFiltroDescripcionComponenteRR.change(cargarGridRR);
            txtFiltroEconomicoRR.change(cargarGridRR);
            cboCCRR.change(cargarGridRR);
            cboMotivoRemocion.change(cargarGridRR);
            txtFiltroEconomicoRR.getAutocomplete(SelectNoComponenteRR, null, '/Overhaul/getEconomico');
            initTblCorreosRR();
            btnAgregarCorreoRR.click(AgregarCorreo);
            $(document).on('click', "#modalEliminar #btnModalEliminar", function () {
                if($("#ulNuevo .active a").text() == "Remociones")
                {
                    switch (banderaRR) {
                        case 0:
                            eliminarReporte();
                            break;
                        case 1:
                            verificarReporte();
                            break;
                        case 2:
                            enviarReporte();
                            //enviarCorreo();
                            break;
                        case 3:
                            autorizarReporte();
                            break;
                        default:
                            break;
                    }
                    cargarGridRR();
                }                
            });
            btnEnviarCorreo.click(ConfirmarEnvioReporte);
        }

        function SelectNoComponenteRR(event, ui) { txtFiltroEconomicoRR.text(ui.item.noEconomico); }

        function SelectSubconjuntoRR(event2, ui2) {
            txtFiltroDescripcionComponenteRR.text(ui2.item.descripcion);
        }

        function initGridRR() {
            gridReportes.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "estatus": function (column, row) {
                        var estado = "";

                        switch (row.estatus)
                        {
                            case 0:
                                estado = "PENDIENTE VoBo";
                                break;
                            case 1:
                                estado = "PENDIENTE VoBo";
                                break;
                            case 2:
                                estado = "PENDIENTE ENVIO";
                                break;
                            case 3:
                                estado = "PENDIENTE AUTORIZACION";
                                break;
                            case 4:
                                estado = "PENDIENTE AUTORIZACION";
                                break;
                            case 5:
                                estado = "APROBADO";
                                break;
                            default:
                                break;
                        }
                        return "<span class=''> " + estado + "</span>";
                    },
                    "motivo": function (column, row) {
                        var stringMotivo = row.motivo == 0 ? "VIDA ÚTIL" : (row.motivo == 1 ? "FALLA" : (row.motivo == 2 ? "ESTRATEGIA" : "DESECHO"));
                        return "<span>" + stringMotivo + "</span>";
                    },
                    "voboReporte": function (column, row) { 
                        return "<span class='' style='" + (row.fechaVoBo == "N/A" ? "display:none" : "") + "'> " + row.fechaVoBo + "</span>" +

                        "<button type='button' class='btn btn-info completar' data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" + row.componenteID +
                        "' data-id='" + row.id + "' style='" + (row.fechaVoBo == "N/A" ? (tipoUsuario == 6 ? "display:none" : "") : "display:none") + "' >" +
                        "<span class='glyphicon glyphicon-file'></span></button><div class='divider'/>" +

                        "<button type='button' class='btn btn-primary vobo' data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" +
                        row.componenteID + "' data-id='" + row.id + "' style='" + ((row.estatus == 1 && row.fechaVoBo == "N/A") ? ((tipoUsuario != 0 && tipoUsuario != 1 && tipoUsuario != 7) ? "display:none" : "") : "display:none") + "' >" +
                        "<span class='glyphicon glyphicon-arrow-right'></span></button><div class='divider'/>" +

                        "<button type='button' class='btn btn-danger eliminar' data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" +
                        row.componenteID + "' data-id='" + row.id + "' style='" + (row.fechaVoBo == "N/A" ? (tipoUsuario == 6 ? "display:none" : "") : "display:none") + "' >" +
                        "<span class='glyphicon glyphicon-remove'></span></button>";
                    },
                    "enviarReporte": function (column, row) {
                        return "<span class='' style='" + (row.fechaEnvio == "N/A" ? "display:none" : "") + "'> " + row.fechaEnvio + "</span>" +

                        "<button type='button' class='btn btn-info " + (row.estatus < 5 ? "completar" : "reporte") + "' data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" + row.componenteID +
                        "' data-id='" + row.id + "' style='" + ((row.estatus >= 2 && row.estatus < 5 && row.fechaEnvio == "N/A") ? (tipoUsuario == 6 ? "display:none" : "") : "display:none") + "' ><span class='glyphicon glyphicon-file'></span></button><div class='divider'/>" +

                        "<button type='button' class='btn btn-primary enviar' data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" + row.componenteID +
                        "' data-id='" + row.id + "' data-motivo='" + row.motivo + "' data-destino='" + row.destino + "' style='" + ((row.estatus >= 2 && row.fechaEnvio == "N/A") ? (tipoUsuario == 6 ? "display:none" : "") : "display:none") + "' >" +
                        "<span class='glyphicon glyphicon-arrow-right'></span></button>" +

                        "<div class='divider'/><button type='button' class='btn btn-danger eliminar' data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" +
                        row.componenteID + "' data-id='" + row.id + "' style='" + ((row.estatus == 2 && row.fechaEnvio == "N/A") ? (tipoUsuario == 6 ? "display:none" : "") : "display:none") + "' >" +
                        "<span class='glyphicon glyphicon-remove'></span></button>";
                    },
                    "autorizarReporte": function (column, row) {
                        return "<button type='button' class='btn btn-info completar' data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" + row.componenteID +
                        "' data-id='" + row.id + "' style='" + (row.fechaAutorizacion == "N/A" ? (tipoUsuario == 6 ? "display:none" : "") : "display:none") + "' >" +
                        "<span class='glyphicon glyphicon-file'></span></button>" +

                        "<div class='divider'/><button type='button' class='btn btn-success autorizar' data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" +
                        row.componenteID + "' data-id='" + row.id + "' style='" + ((row.fechaAutorizacion == "N/A" && row.estatus == 4) ? ((tipoUsuario == 1 || tipoUsuario == 2 || tipoUsuario == 7) ? "" : "display:none") : "display:none") + "' >" +
                        "<span class='glyphicon glyphicon-arrow-right'></span></button>";
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                gridReportes.find(".vobo").parent().css("text-align", "center");
                gridReportes.find(".enviar").parent().css("text-align", "center");
                gridReportes.find(".autorizar").parent().css("text-align", "center");
                gridReportes.find(".vobo").parent().css("width", "13%");
                gridReportes.find(".enviar").parent().css("width", "13%");
                gridReportes.find(".autorizar").parent().css("width", "10%");
                                
                gridReportes.find(".completar").on('click', function (e) {
                    window.location.href = "/Overhaul/Remocion?id=" + $(this).attr("data-componenteid");
                });

                gridReportes.find(".reporte").on('click', function (e) {
                    //window.location.href = "/Overhaul/Remocion?id=" + $(this).attr("data-componenteid");
                });

                gridReportes.find(".eliminar").on('click', function (e) {
                    banderaRR = 0;
                    idReporteRR = $(this).attr("data-id");
                    ConfirmacionEliminacion("Eliminar Reporte de Remoción", "¿Esta seguro que desea dar de baja el reporte de remoción para el componente \"" + $(this).attr("data-componente") + "\"?");                    
                });

                gridReportes.find(".vobo").on('click', function (e) {
                    banderaRR = 1;
                    idReporteRR = $(this).attr("data-id");
                    ConfirmacionEliminacion("Verificar", "Se aprobará el BoVo de remoción para el componente \"" + $(this).attr("data-componente") + "\", ¿Desea continuar?");
                });

                gridReportes.find(".enviar").on('click', function (e) {
                    
                    idReporteRR = $(this).attr("data-id");
                    var listaCorreos = [1, 1, 0, 0, 1];
                    var destinoCorreo = $(this).attr("data-motivo") != "2" ? $(this).attr("data-destino") : "";
                    CargartablaCorreosRR(tblCorreosRR, listaCorreos, destinoCorreo);
                    $("#txtCorreoRR").val('');
                    modalCorreosRR.modal("show");
                    btnEnviarCorreo.attr("data-id", $(this).attr("data-id"));
                    btnEnviarCorreo.attr("data-componente", $(this).attr("data-componente"));                 
                });

                gridReportes.find(".autorizar").on('click', function (e) {
                    banderaRR = 3;
                    idReporteRR = $(this).attr("data-id");
                    ConfirmacionEliminacion("Aprobación de reporte de remoción", "Se autorizará el reporte de remoción para el componente \"" + $(this).attr("data-componente") + "\", ¿Desea continuar?");
                });

            });
        }

        function cargarGridRR() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarReportesRemocion",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    estatus: cboEstatusReporte.val() == "" ? "-1" : cboEstatusReporte.val(),
                    descripcionComponente: txtFiltroDescripcionComponenteRR.val(),
                    noEconomico: txtFiltroEconomicoRR.val() == "" ? -1 : txtFiltroEconomicoRR.val(),
                    cc: cboCCRR.val() == "" ? null : cboCCRR.val(),
                    motivoRemocion: cboMotivoRemocion.val() == "" ? -1 : cboMotivoRemocion.val()
                    //fechaInicio: txtFechaInicio.val(),
                    //fechaFinal: txtFechaFinal.val()
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridReportes.bootgrid({
                            rowCount: -1,
                            templates: {
                                header: ""
                            }
                        });
                        tipoUsuario = response.tipoUsuario;
                        gridReportes.bootgrid("clear");
                        gridReportes.bootgrid("append", response.reportes);
                        gridReportes.bootgrid('reload');                        
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function eliminarReporte()
        {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/EliminarReportesRemocion",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ id: idReporteRR }),
                success: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "El reporte se eliminó correctamente");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function abrirReporte(idReporteRemocion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/getReporteRemocionComponente',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({ idReporteRemocion: idReporteRemocion }),
                success: function (response) {                    
                    ireport.attr("src", response.html);
                    document.getElementById('report').onload = function () {
                        openCRModal();
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });            
        }

        function abrirReportePorFiltros()
        {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/getReportesRemocionComponenteGrupo',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    estatus: cboEstatusReporte.val() == "" ? "-1" : cboEstatusReporte.val(),
                    descripcionComponente: txtFiltroDescripcionComponenteRR.val(),
                    noEconomico: txtFiltroEconomicoRR.val() == "" ? -1 : txtFiltroEconomicoRR.val(),
                    cc: cboCCRR.val() == "" ? -1 : cboCCRR.val(),
                    motivoRemocion: cboMotivoRemocion.val() == "" ? -1 : cboMotivoRemocion.val()
                    //fechaInicio: txtFechaInicio.val(),
                    //fechaFinal: txtFechaFinal.val()
                }),
                success: function (response) {
                    ireport.attr("src", "/Reportes/Vista.aspx?idReporte=151");
                    document.getElementById('report').onload = function () {
                        openCRModal();
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function verificarReporte() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/verificarReporteRemocion',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idReporte: idReporteRR }),
                success: function (response) {

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function enviarReporte() {
            $(".enviarReporte").prop("disabled", true);
            let reporteHtml = "";
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: '/Overhaul/enviarReporteRemocion',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({ idReporte: idReporteRR }),
                success: function (response) {                    
                    if (response.success) {
                        modalCorreosRR.modal("hide");
                        enviarCorreo();                        
                    }
                    else { AlertaGeneral("Alerta", response.message); $.unblockUI(); }
                },
                error: function (response) {
                    $.unblockUI();
                    $(".enviarReporte").prop("disabled", false);
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function enviarCorreo()
        {

            $.ajax({
                url: '/Overhaul/getReporteRemocionComponente',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({ idReporteRemocion: idReporteRR }),
                success: function (response) {                    
                    ireport.attr("src", response.html + "&inMemory=1");
                    document.getElementById('report').onload = function () {
                        sendCorreo();
                    };
                    ConfirmacionGeneral("Confirmación", "Envío correcto", "bg-green");
                    //$.unblockUI();
                },
                error: function (response) {
                    $(".enviarReporte").prop("disabled", false);
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });              
        }

        function sendCorreo()
        {
            let correos = [];
            $('#tblCorreosRR tbody tr').each(function () {
                correos.push($(this).find('td:eq(0)').text());
            });

            $.ajax({
                url: '/Overhaul/enviarCorreoReporteRemocion',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({ idReporte: idReporteRR, correos: correos }),
                success: function (response) {                    
                    // ireport.attr("src", response.html);
                    // document.getElementById('report').onload = function () {
                    //     modalCorreosRR.modal("hide");
                    //     ConfirmacionGeneral("Confirmación", "Envío correcto", "bg-green");
                    // };   
                    $.unblockUI();
                    $(".enviarReporte").prop("disabled", false);
                },
                error: function (response) {
                    $.unblockUI();
                    $(".enviarReporte").prop("disabled", false);
                    AlertaGeneral("Alerta", response.message);
                }
            });  
        }

        function autorizarReporte()
        {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/autorizarReporteRemocion',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idReporte: idReporteRR }),
                success: function (response) {

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function CargartablaCorreosRR(tabla, listaCorreos, locacionID) {
            $.ajax({
                url: '/Overhaul/GetCorreosOverhaul',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ locacionID: locacionID }),
                success: function (response) {
                    tabla.bootgrid("clear");
                    let j = 1;
                    var JSONINFO = [{ "id": 0, "correo": response.correosPpales[0] }];
                    tabla.bootgrid("append", JSONINFO);
                    for (i = 0; i < listaCorreos.length; i++, j++) {
                        if (listaCorreos[i] == 1) {
                            var JSONINFO = [{ "id": (i + 1), "correo": response.correosPpales[(i + 1)] }];
                            tabla.bootgrid("append", JSONINFO);
                        }
                    }
                    for (i = 0; i < response.correosLocacion.length; i++, j++) {
                        var JSONINFO = [{ "id": j, "correo": response.correosLocacion[i] }];
                        tabla.bootgrid("append", JSONINFO);

                    }
                    ultimoIDCorreo = j;
                    tabla.bootgrid('reload');
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function initTblCorreosRR() {
            ultimoIDCorreo = 0;
            tblCorreosRR.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: { header: "" },
                sorting: false,
                formatters: { "eliminar": function (column, row) { return "<button type='button' class='btn btn-danger eliminar'><span class='glyphicon glyphicon-remove'></span></button>"; } }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblCorreosRR.find(".eliminar").parent().css("text-align", "center");
                tblCorreosRR.find(".eliminar").parent().css("width", "3%");
                tblCorreosRR.find(".eliminar").on("click", function (e) {
                    var rowID = parseInt($(this).parent().parent().attr('data-row-id'));
                    tblCorreosRR.bootgrid("remove", [rowID]);
                });
            });
        }

        function AgregarCorreo() {
            let correo = $("#txtCorreoRR").val().trim();
            if (correo != "" && validateEmail(correo)) {
                var JSONINFO = [{ "id": ultimoIDCorreo, "correo": correo }];
                tblCorreosRR.bootgrid("append", JSONINFO);
                ultimoIDCorreo++;
            }
            else { AlertaGeneral("Alerta", "No se ha proporcionado un correo electrónico válido"); }
        }

        function validateEmail(email) {
            var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
            if (!emailReg.test(email)) {
                return false;
            } else {
                return true;
            }
        }

        function ConfirmarEnvioReporte()
        {
            banderaRR = 2;
            ConfirmacionEliminacion("Verificar", "Se enviará el reporte de remoción para el componente \"" + btnEnviarCorreo.attr("data-componente") + "\", ¿Desea continuar?");
        }
        
        init();
    };

    $(document).ready(function () {
        maquinaria.overhaul.reportesremocion = new reportesremocion();        
    });

})();


