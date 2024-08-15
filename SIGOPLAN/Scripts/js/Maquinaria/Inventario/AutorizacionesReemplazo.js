(function () {

    $.namespace('maquinaria.inventario.Solicitud.AutorizacionesReemplazo');

    AutorizacionesReemplazo = function () {
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



        tbCC = $("#tbCC"),
            tbFechaInicio = $("#tbFechaInicio"),
            tbFechaFin = $("#tbFechaFin"),
            btnAplicarFiltros = $("#btnAplicarFiltros"),
            //
            cboTipoFiltro = $("#cboTipoFiltro"),
            txtFolioFiltro = $('#txtFolioFiltro'),
            divSolicitudesPendientes = $("#divSolicitudesPendientes"),
            divAutorizacionSolicitudes = $("#divAutorizacionSolicitudes"),
            ireport = $("#report"),
            btnAutorizacion = $("#btnAutorizacion"),
            btnRechazo = $("#btnRechazo"),
            modalRechazo = $("#modalRechazo"),
            //Autorizadores.-
            lblGerenteDirector = $("#lblGerenteDirector"),
            lblElaboro = $("#lblElaboro"),
            lblGerente = $("#lblGerente"),
            lblAsigna = $("#lblAsigna"),
            lblAltaDireccion = $("#lblAltaDireccion"),
            btnElaboro = $("#btnElaboro"),
            btnGerenteObra = $("#btnGerenteObra"),
            btnAsigna = $("#btnAsigna"),
            btnAltaDireccion = $("#btnAltaDireccion"),
            modalReportes = $("#modalReportes"),
            btnRechazoSave = $("#btnRechazoSave"),
            BntRegresar = $("#BntRegresar"),
            tblSolicitudesPendientes = $("#tblSolicitudesPendientes");

        function init() {
            loadTabla();
            btnRechazoSave.click(RechazoSolicitud);
            cboTipoFiltro.change(loadTabla);
            txtFolioFiltro.change(loadTabla);

            BntRegresar.click(Regresar);
        }

        function loadTabla() {
            bootG('/SolicitudEquipo/GetSolicitudesReemplazoPendientes');
        }

        function Regresar() {

            divSolicitudesPendientes.removeClass('hidden');
            divAutorizacionSolicitudes.addClass('hidden');

        }

        function iniciarGrid() {
            tblSolicitudesPendientes.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "VerSolicitud": function (column, row) {
                        if (row && row.comentario && row.comentario.length > 0) {
                            return "<button type='button' title='Ver solicitud' class='btn btn-primary verSolicitud' data-id='" + row.id + "' data-CC='" + row.cc + "' >" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>" +
                                "<button type='button' title='Ver comentario de rechazo' class='btn btn-danger verComentario' data-comentario='" + row.comentario + "' >" +
                                "<i class='far fa-comment'></i> " +
                                " </button>";
                        } else {
                            return "<button type='button' title='Ver solicitud' class='btn btn-primary verSolicitud' data-id='" + row.id + "' data-CC='" + row.cc + "' >" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>"
                                ;
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblSolicitudesPendientes.find(".verSolicitud").on("click", function (e) {
                    divSolicitudesPendientes.addClass('hidden');

                    divAutorizacionSolicitudes.removeClass('hidden');
                    LoadAutorizadores($(this).attr('data-id'));
                    LoadReporte($(this).attr('data-id'), $(this).attr("data-CC"));
                    divAutorizacionSolicitudes.attr("data-CC", $(this).attr("data-CC"));
                });

                // Al hacer click sobre ver comentario
                tblSolicitudesPendientes.find(".verComentario").click(e => {
                    const comentario = $(e.currentTarget).attr("data-comentario");
                    AlertaGeneral("Motivo de rechazo:", comentario);
                });

            });
        }

        function bootG(url) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: url,
                type: "POST",
                datatype: "json",
                data: { filtro: cboTipoFiltro.val(), folio: txtFolioFiltro.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.Autorizadas;
                    tblSolicitudesPendientes.bootgrid("clear");
                    tblSolicitudesPendientes.bootgrid("append", data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function LoadAutorizadores(idSolicitud) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/SolicitudEquipo/GetDataAutorizadoresReemplazo',
                data: { id: Number(idSolicitud) },
                success: function (response) {

                    var AutorizadorElabora = response.AutorizadorElabora;
                    var AutorizadorGerente = response.AutorizadorGerente;
                    var AutorizadorAsigna = response.AutorizadorAsigna;

                    SetAutorizacion(response.AutorizadorActual, AutorizadorElabora, AutorizadorGerente, AutorizadorAsigna, response.idAutorizacion);

                    SetNombreAutorizadores(AutorizadorElabora.nombreUsuario, AutorizadorGerente.nombreUsuario, AutorizadorAsigna.nombreUsuario);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function saveOrUpdateData() {

            var id = $(this).attr('data-id');
            var indice = $(this).attr('data-autoriza');
            if (id != null) {
                var Autoriza = $(this).attr('data-Autorizado');
                if (Autoriza == "false") {
                    saveOrUpdate(id, indice, $(this));
                }
            }
        }




        function saveOrUpdate(obj, Autoriza) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/SaveOrUpdateSolicitudReemplazo',
                type: 'POST',
                dataType: 'json',
                data: { obj: obj, Autoriza: Autoriza },
                success: function (response) {
                    if (response.success == true) {
                        ConfirmacionGeneral("Confirmación", "Se Autorizo Correctamente", "bg-green");
                        var elemento = $("#btnAutorizacion").parents().find('.noPadding');

                        elemento.next().removeClass('panel-footer-Pendiente');
                        elemento.next().addClass('panel-footer-Autoriza').html("Autorizado");
                        elemento.removeClass('noPadding');
                        elemento.attr('data-Autorizado', true);

                        var cc = divAutorizacionSolicitudes.attr("data-CC");
                        var idSolicitud = response.idSolicitud;
                        LoadReporte(idSolicitud, cc);
                        $("#divAccionesAutorizacion").remove();

                        $.unblockUI();
                    }
                    else {
                        ConfirmacionGeneral("Alerta", response.message, "bg-red");
                        $.unblockUI();
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        $(document).on('click', "#btnAutorizacion", function () {
            var id = $("#btnAutorizacion").attr('data-idAutorizacion');
            var puesto = $("#btnAutorizacion").attr('data-PuestoAutorizador');
            saveOrUpdate(id, puesto);
        });

        $(document).on('click', "#btnRechazo", function () {
            modalRechazo.modal('show');
        });

        function RechazoSolicitud() {
            modalRechazo.modal('hide');
            var obj = $("#btnRechazo").attr('data-idAutorizacion');
            var puesto = $("#btnRechazo").attr('data-PuestoAutorizador');
            var elemento = $("#btnRechazo").parents().find('.noPadding');
            var comentario = $("#txtAreaNota").val();

            if (comentario === "" || comentario.trim().length < 10) {
                AlertaGeneral("Aviso", "Debe agregar un comentario mayor a 10 caracteres antes de poder rechazar la solicitud.");
                return;
            }

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/RechazoSolicitudReemplazo',
                type: 'POST',
                dataType: 'json',
                data: { obj: obj, Autoriza: puesto, comentario: comentario },
                success: function (response) {
                    if (response.success == true) {
                        ConfirmacionGeneral("Confirmación", "La solicitud fue rechazada correctamente", "bg-green");
                        $("#divAccionesAutorizacion").remove();

                        elemento.next().removeClass('panel-footer-Pendiente');
                        elemento.next().addClass('panel-footer-Rechazo').html("Rechazado");
                        elemento.removeClass('noPadding');
                        elemento.attr('data-Autorizado', false);

                        var cc = divAutorizacionSolicitudes.attr("data-CC");
                        var idSolicitud = response.idSolicitud;
                        LoadReporte(idSolicitud, cc);

                        $.unblockUI();
                    }
                    else {
                        ConfirmacionGeneral("Alerta", response.message, "bg-red");
                        $.unblockUI();
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });

        }

        function SetAutorizacion(Autoriza, AutorizadorElabora, AutorizadorGerente, AutorizadorAsigna, idAutorizacion) {//FirmaElabora, FirmaGerente, FirmaDirector, FirmaDireccion, ) {

            var FirmaElabora = AutorizadorElabora.firma;
            var FirmaGerente = AutorizadorGerente.firma;
            var FirmaAsigna = AutorizadorAsigna.firma;



            var cadebaElabora = AutorizadorElabora.firmaCadena;
            var cadenaGerente = AutorizadorGerente.firmaCadena;
            var cadenaAsigna = AutorizadorAsigna.firmaCadena;

            if (FirmaElabora == true) {
                setEstatusBnts(btnElaboro, 1);
            }
            if (FirmaGerente == true) {
                setEstatusBnts(btnGerenteObra, 1);
            }
            else {
                setEstatusBnts(btnGerenteObra, 3);
            }

            if (FirmaAsigna == true) {
                setEstatusBnts(btnAsigna, 1);
            }
            else {
                setEstatusBnts(btnAsigna, 3);
            }
            if (FirmaGerente == false && cadenaGerente != null) {
                setEstatusBnts(btnGerenteObra, 2);
            }

            if (FirmaAsigna == false && cadenaAsigna != null) {
                setEstatusBnts(btnAsigna, 2);
            }

            if (Autoriza == "Elaboro") {
                if (FirmaElabora == false && cadebaElabora == "") {
                    btnElaboro.addClass('noPadding');
                    btnElaboro.children().remove();
                    setFirmas(btnElaboro, "", idAutorizacion, Autoriza);
                }
            }
            if (Autoriza == "gerenteObra") {
                if (FirmaGerente == false && cadenaGerente == null && FirmaElabora == true) {
                    btnGerenteObra.addClass('noPadding');
                    btnGerenteObra.children().remove();
                    setFirmas(btnGerenteObra, "", idAutorizacion, Autoriza);

                }
            }

            if (Autoriza == "asigna") {
                if (FirmaAsigna == false && cadenaAsigna == null && FirmaElabora == true && FirmaGerente == true) {
                    btnAsigna.addClass('noPadding');
                    btnAsigna.children().remove();
                    setFirmas(btnAsigna, "", idAutorizacion, Autoriza);
                }
            }



            //switch (Autoriza) {
            //    case "Elaboro":
            //        if (FirmaElabora == false && cadebaElabora == "") {
            //            btnElaboro.addClass('noPadding');
            //            setFirmas(btnElaboro, "", idAutorizacion, Autoriza);
            //        }
            //        break;
            //    case "gerenteObra":
            //        if (FirmaGerente == false && cadenaGerente == null && FirmaElabora == true) {
            //            btnGerenteObra.addClass('noPadding');
            //            setFirmas(btnGerenteObra, "", idAutorizacion, Autoriza);
            //        }
            //        break;
            //    case "asigna":
            //        if (FirmaAsigna == false && cadenaAsigna == null && FirmaElabora == true && FirmaGerente == true) {
            //            btnAsigna.addClass('noPadding');
            //            setFirmas(btnAsigna, "", idAutorizacion, Autoriza);
            //        }
            //        break;
            //    default:
            //        break;
            //}


        }

        function setEstatusBnts(elemento, tipo) {
            if (tipo == 1) {
                elemento.children().remove();
                elemento.removeClass('noPadding');
                elemento.next().removeClass('panel-footer-Pendiente');
                elemento.next().addClass('panel-footer-Autoriza').html("Autorizado");
                elemento.removeClass('btn btn-block');
                elemento.attr('data-Autorizado', true);
                elemento.removeClass('bg-primary');
            } else
                if (tipo == 2) {
                    elemento.children().remove();
                    elemento.next().removeClass('panel-footer-Pendiente');
                    elemento.next().addClass('panel-footer-Rechazo').html("Rechazado");
                    elemento.removeClass('noPadding');
                    elemento.attr('data-Autorizado', false);
                } else {
                    elemento.next().removeClass('panel-footer-Autoriza')
                    elemento.next().removeClass('panel-footer-Pendiente');
                    elemento.next().addClass('panel-footer-Pendiente').html("Pendiente");
                    elemento.addClass('noPadding');
                    elemento.attr('data-Autorizado', false);
                    elemento.removeClass('noPadding');
                }
        }

        function setFirmas(elemento, texto, idAutorizacion, puesto) {
            var btnsControl = "<div class='row'> <div class='col-lg-12 col-xs-12' id='divAccionesAutorizacion'> <div class='col-xs-6'><button class='form-control btn btn-block colorAutoriza' id='btnAutorizacion'>Autorizar</button></div>" +
                "<div class='col-xs-6'><button class='form-control btn btn-block colorRechaza rechazo' id='btnRechazo'>Rechazar</button></div></div></div>"
            elemento.append(btnsControl);

            $("#btnAutorizacion").attr('data-idAutorizacion', idAutorizacion);
            $("#btnAutorizacion").attr('data-PuestoAutorizador', puesto);

            $("#btnRechazo").attr('data-idAutorizacion', idAutorizacion);
            $("#btnRechazo").attr('data-PuestoAutorizador', puesto);

        }

        function SetNombreAutorizadores(Elabora, Gerente, asigna) {
            lblElaboro.text(Elabora),
                lblGerente.text(Gerente),
                lblAsigna.text(asigna);
        }

        function LoadReporte(idSolicitud, centroCostos) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/SetRptAutorizacionReemplazo',
                type: "POST",
                datatype: "json",
                data: { idSolicitudReemplazo: idSolicitud },
                success: function (response) {
                    var path = "/Reportes/Vista.aspx?idReporte=34&pCC=" + centroCostos;
                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                    };
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        iniciarGrid();
        init();
    };

    $(document).ready(function () {
        maquinaria.inventario.Solicitud.AutorizacionesReemplazo = new AutorizacionesReemplazo();
    });
})();


