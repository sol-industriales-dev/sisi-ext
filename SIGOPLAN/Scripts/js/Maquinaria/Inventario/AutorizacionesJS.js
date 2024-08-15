(function () {
    $.namespace('maquinaria.inventario.Solicitud.Autorizaciones');
    Autorizaciones = function () {
        const mensajes = {
            NOMBRE: 'ReporteCapturaCombustibleMensual',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        divEvidencia = $(".divEvidencia");
        btnEvidencia = $("#btnEvidencia");
        const btnAtras = $("#btnAtras");
        const btnNext = $("#btnNext");
        const BntRegresar = $("#BntRegresar");
        const btnGerenteDirector = $("#btnGerenteDirector");
        const cboTipoFiltro = $("#cboTipoFiltro");
        const divSolicitudesPendientes = $("#divSolicitudesPendientes");
        const divAutorizacionSolicitudes = $("#divAutorizacionSolicitudes");
        const ireport = $("#report");
        const modalRechazo = $("#modalRechazo");
        const lblGerenteDirector = $("#lblGerenteDirector");
        const lblElaboro = $("#lblElaboro");
        const lblGerente = $("#lblGerente");
        const lblDirector = $("#lblDirector");
        const lblServicios = $("#lblServicios");
        const lblAltaDireccion = $("#lblAltaDireccion");
        const btnElaboro = $("#btnElaboro");
        const btnGerenteObra = $("#btnGerenteObra");
        const btnDirectorDivision = $("#btnDirectorDivision");
        const btnAltaDireccion = $("#btnAltaDireccion");
        const btnDirectorServicios = $("#btnDirectorServicios");
        const btnRechazoSave = $("#btnRechazoSave");
        const tblSolicitudesPendientes = $("#tblSolicitudesPendientes");
        function init() {
            var id = $.urlParam('Solicitud');
            bootG('/SolicitudEquipo/GetDataSolicitudesPendientes');
            btnRechazoSave.click(RechazoSolicitud);
            cboTipoFiltro.change(loadTabla);
            BntRegresar.click(Regresar);
            if (id != null) {
                CargarSolicitud(id);
            }
            btnNext.click(SetSiguiente);
            btnAtras.click(SetAtras);
            btnEvidencia.click(fnDescargarEvidencia);
        }
        function fnDescargarEvidencia()
        {
            var _this = $(this);
            var solicitudID = _this.data("id");
            location.href = '/SolicitudEquipo/getFileDownload?id=' + solicitudID;
        }
        function SetAtras() {
            var index = Number(btnAtras.attr('data-id'));
            var objP = tblSolicitudesPendientes.bootgrid().data('.rs.jquery.bootgrid').rows[index];
            if (index > 0) {
                btnAtras.attr('data-id', index - 1);
            }
            btnNext.attr('data-id', index + 1);
            LoadAutorizadores(objP.id);
        }
        function SetSiguiente() {
            var index = Number(btnNext.attr('data-id'));
            var objP = tblSolicitudesPendientes.bootgrid().data('.rs.jquery.bootgrid').rows[index];
            if (index > 0) {
                btnAtras.attr('data-id', index - 1);
            }
            btnNext.attr('data-id', index + 1);
            LoadAutorizadores(objP.id);
        }
        function CargarSolicitud(id) {
            divSolicitudesPendientes.addClass('hidden');
            divAutorizacionSolicitudes.removeClass('hidden');
            LoadAutorizadores(id);
        }
        function Regresar() {
            var url = window.location.href;

            var urlLength = url.split('?').length;
            if (urlLength > 1) {
                window.location.href = url.split('?')[0];
            }
            else {
                divSolicitudesPendientes.removeClass('hidden');
                divAutorizacionSolicitudes.addClass('hidden');
            }
        }
        function loadTabla() {
            bootG('/SolicitudEquipo/GetDataSolicitudesPendientes');
        }

        function iniciarGrid() {
            tblSolicitudesPendientes.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                formatters: {
                    "VerSolicitud": function (column, row) {
                        if (row && row.Comentario && row.Comentario.length > 0) {
                            return "<button type='button' title='Ver solicitud' class='btn btn-primary verSolicitud' data-id='" + row.id + "' data-CC='" + row.cc + "' >" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>" +
                                "<button type='button' title='Ver comentario de rechazo' class='btn btn-danger verComentario' data-comentario='" + row.Comentario + "' >" +
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
                    var idActual = Number($(this).parent().parent().attr('data-row-id'));
                    btnAtras.prop('disabled', true);
                    if (idActual > 0) {
                        btnAtras.attr('data-id', idActual - 1);
                    } else {
                        btnAtras.attr('data-id', idActual);
                    }
                    btnNext.attr('data-id', idActual + 1);
                    LoadAutorizadores($(this).attr('data-id'));

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
                data: { filtro: cboTipoFiltro.val() },
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
                url: '/SolicitudEquipo/GetDataAutorizacion',
                data: { obj: idSolicitud },
                success: function (response) {
                    var AutorizadorElabora = response.AutorizadorElabora;
                    var AutorizadorGerente = response.AutorizadorGerente;
                    var AutorizadorGerenteDirector = response.AutorizadorGerenteDirector;
                    var AutorizadorDirector = response.AutorizadorDirector;
                    var AutorizadorServicios = response.AutorizadorServicios;
                    var AutorizadorDireccion = response.AutorizadorDireccion;
                    
                    SetAutorizacion(response.AutorizadorActual, AutorizadorElabora, AutorizadorGerente, AutorizadorDirector, AutorizadorDireccion, response.idAutorizacion, AutorizadorGerenteDirector, response.observaciones,AutorizadorServicios);
                    SetNombreAutorizadores(AutorizadorElabora.nombreUsuario, AutorizadorGerente.nombreUsuario, AutorizadorDirector.nombreUsuario, AutorizadorDireccion.nombreUsuario, AutorizadorGerenteDirector.nombreUsuario,AutorizadorServicios.nombreUsuario);
                    LoadReporte(idSolicitud, response.Centro_costos);
                    divAutorizacionSolicitudes.attr("data-CC", response.Centro_costos);
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
                url: '/SolicitudEquipo/SaveOrUpdateAutorizacion',
                type: 'POST',
                dataType: 'json',
                data: { obj: obj, Autoriza: Autoriza },
                success: function (response) {
                    if (response.success == true) {
                        ConfirmacionGeneral("Confirmación", "Se Autorizo Correctamente", "bg-green");
                        var elemento = $("#btnAutorizacion").parents().find('.noPadding');
                        $("#divAccionesAutorizacion").remove();
                        elemento.next().removeClass('panel-footer-Pendiente');
                        elemento.next().addClass('panel-footer-Autoriza').html("Autorizado");
                        elemento.removeClass('noPadding');
                        elemento.attr('data-Autorizado', true);

                        var cc = divAutorizacionSolicitudes.attr("data-CC");
                        var idSolicitud = response.idSolicitud;
                        LoadReporte(idSolicitud, cc)

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
            var obj = $("#btnRechazo").attr('data-idAutorizacion');
            var puesto = $("#btnRechazo").attr('data-PuestoAutorizador');
            var elemento = $("#btnRechazo").parents().find('.noPadding');
            var comentario = $("#txtAreaNota").val();

            if (comentario === "" || comentario.trim().length < 10) {
                AlertaGeneral("Aviso", "Debe agregar un comentario mayor a 10 caracteres antes de poder rechazar la solicitud.");
                return;
            }

            modalRechazo.modal('hide');
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/RechazoSolicitud',
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

        function SetAutorizacion(Autoriza, AutorizadorElabora, AutorizadorGerente, AutorizadorDirector, AutorizadorDireccion, idAutorizacion, AutorizadorGerenteDirector, observaciones, AutorizadorServicios) {
            var FirmaElabora = AutorizadorElabora.firma;
            var FirmaGerente = AutorizadorGerente.firma;
            var FirmaGerenteDirector = AutorizadorGerenteDirector.firma;
            var FirmaDirector = AutorizadorDirector.firma;
            var FirmaServicios = AutorizadorServicios.firma; 
            var FirmaDireccion = AutorizadorDireccion.firma;   
            
            var cadebaElabora = AutorizadorElabora.firmaCadena;
            var cadenaGerente = AutorizadorGerente.firmaCadena;
            var cadenaGerenteDirector = AutorizadorGerenteDirector.firmaCadena;
            var cadenaDirector = AutorizadorDirector.firmaCadena;
            var cadenaServicios = AutorizadorServicios.firmaCadena;    
            var cadenaDireccion = AutorizadorDireccion.firmaCadena;     
             
            setEstatusBnts(btnElaboro, AutorizadorElabora);
            setEstatusBnts(btnGerenteObra, AutorizadorGerente);
            setEstatusBnts(btnGerenteDirector, AutorizadorGerenteDirector);
            setEstatusBnts(btnDirectorDivision, AutorizadorDirector);
            setEstatusBnts(btnDirectorServicios, AutorizadorServicios);
            setEstatusBnts(btnAltaDireccion, AutorizadorDireccion);
            
            switch (true) {
                case Autoriza == "Elaboro" && !FirmaElabora && cadebaElabora == "":
                    setFirmas(btnElaboro, "", idAutorizacion, Autoriza);
                    break;
                case Autoriza == "gerenteObra" && !FirmaGerente && cadenaGerente == null && FirmaElabora:
                    setFirmas(btnGerenteObra, "", idAutorizacion, Autoriza);
                    break;
                case Autoriza == "GerenteDirector" && !FirmaGerenteDirector && cadenaDirector == null && FirmaElabora && FirmaGerente:
                    setFirmas(btnGerenteDirector, "", idAutorizacion, Autoriza);
                    break;
                case Autoriza == "directorDivision" && !FirmaDirector && cadenaDirector  == null && FirmaGerente && FirmaGerenteDirector:
                    setFirmas(btnDirectorDivision, "", idAutorizacion, Autoriza);
                    break;
                case Autoriza == "directorServicios" && !FirmaServicios && cadenaServicios == null && FirmaGerenteDirector && FirmaDirector:
                    setFirmas(btnDirectorServicios, "", idAutorizacion, Autoriza);
                    break;
                case Autoriza == "altaDireccion" && !FirmaDireccion && cadenaDireccion == null && FirmaDirector && FirmaServicios:
                    setFirmas(btnAltaDireccion, "", idAutorizacion, Autoriza);
                    break;
                default: break;
            }       
        }
        function setEstatusBnts(elemento, firmaCadena) {
            switch (true) {
                case firmaCadena.firmaCadena === null:
                    elemento.children().remove();
                    elemento.next().removeClass('panel-footer-Autoriza');
                    elemento.next().removeClass('panel-footer-Pendiente');
                    elemento.next().addClass('panel-footer-Pendiente').html("Pendiente");
                    elemento.removeClass('noPadding');
                    elemento.attr('data-Autorizado', false);
                    break;
                case firmaCadena.firmaCadena.includes("R") || firmaCadena.firmaCadena.includes("S"):
                    elemento.children().remove();
                    elemento.next().removeClass('panel-footer-Autoriza');
                    elemento.next().removeClass('panel-footer-Pendiente');
                    elemento.next().addClass('panel-footer-Rechazo').html("Rechazó");
                    elemento.removeClass('noPadding');
                    elemento.attr('data-Autorizado', false); 
                    break;
                case firmaCadena.firmaCadena.includes("A"):
                    elemento.children().remove();
                    elemento.next().removeClass('panel-footer-Rechazo');
                    elemento.next().removeClass('panel-footer-Pendiente');
                    elemento.next().addClass('panel-footer-Autoriza').html("Autorizado");
                    elemento.removeClass('btn btn-block');
                    elemento.attr('data-Autorizado', true);
                    elemento.removeClass('bg-primary');
                    elemento.removeClass('noPadding');
                    break;
                default:
                    break;
            }
        }

        function setFirmas(elemento, texto, idAutorizacion, puesto) {
            elemento.children().remove();
            elemento.addClass('noPadding');
            var btnsControl = "<div class='row'> <div class='col-lg-12 col-xs-12' id='divAccionesAutorizacion'> <div class='col-xs-6'><button class='form-control btn btn-block colorAutoriza' id='btnAutorizacion'>Autorizar</button></div>" +
                "<div class='col-xs-6'><button class='form-control btn btn-block colorRechaza rechazo' id='btnRechazo'>Rechazar</button></div></div></div>"
            elemento.append(btnsControl);
            $("#btnAutorizacion").attr('data-idAutorizacion', idAutorizacion);
            $("#btnAutorizacion").attr('data-PuestoAutorizador', puesto);
            $("#btnRechazo").attr('data-idAutorizacion', idAutorizacion);
            $("#btnRechazo").attr('data-PuestoAutorizador', puesto);

        }
        function SetNombreAutorizadores(Elabora, Gerente, Director, Direccion, GerenteDirector,Servicios) {
            lblElaboro.text(Elabora),
                lblGerente.text(Gerente),
                lblServicios.text(Servicios);
                lblDirector.text(Director);
                lblAltaDireccion.text(Direccion);
            lblGerenteDirector.text(GerenteDirector);
        }
        function LoadReporte(idSolicitud, CC) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/GetReporte',
                type: "POST",
                datatype: "json",
                data: { obj: idSolicitud },
                success: function (response) {
                    if(response.descarga)
                    {
                        btnEvidencia.data("id",idSolicitud);
                        divEvidencia.show();
                    }
                    else{
                        divEvidencia.hide();
                    }
                    var idReporte = response.idReporte;
                    var path = `/Reportes/Vista.aspx?idReporte=${idReporte}&pCC=${CC}&inMemory=1`;
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
        maquinaria.inventario.Solicitud.Autorizaciones = new Autorizaciones();
    });
})();


