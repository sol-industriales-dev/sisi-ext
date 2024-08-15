(function () {

    $.namespace('maquinaria.inventario.MovimientoMaquinaria.ControlEnvioyRecepcion');

    ControlEnvioyRecepcion = function () {

        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        divMotivosEnvio = $("#divMotivosEnvio"),
        tbLugarEnvio = $("#tbLugarEnvio"),
        cboFiltroEquipos = $("#cboFiltroEquipos"),
        btnGuardar = $("#btnGuardar"),
        cboTipoControl = $("#cboTipoControl")
        tbTipoControl = $("#tbTipoControl"),
        tblFechaActual = $("#tblFechaActual"),
        tblEquiposPendientes = $("#tblEquiposPendientes"),
        divTrafico = $("#divTrafico"),
        frmDatosGenerales = $("#frmDatosGenerales"),
        tbFolio = $("#tbFolio"),
        tbEconomico = $("#tbEconomico"),
        lblLugarControl = $("#lblLugarControl"),
        lblFechaControl = $("#lblFechaControl"),
        tbTipoMaquina = $("#tbTipoMaquina"),
        tbGrupoMaquina = $("#tbGrupoMaquina"),
        tbMarcaMaquina = $("#tbMarcaMaquina"),
        tbModeloMaquina = $("#tbModeloMaquina"),
        tbSerieMaquina = $("#tbSerieMaquina"),
        tbArregloMaquina = $("#tbArregloMaquina"),
        tbTipoComponente = $("#tbGrupoComponente"),
        tbMarcaComponente = $("#tbMarcaComponente"),
        tbModeloComponente = $("#tbModeloComponente"),
        tbNoSerieComponente = $("#tbNoSerieComponente"),
        tbArregloComponente = $("#tbArregloComponente"),
        btnSiguiente1 = $("#btnSiguiente1"),
        tbTipoOtro = $("#tbTipoOtro"),
        tbGrupoOtros = $("#tbGrupoOtros"),
        tbMarcaOtros = $("#tbMarcaOtros"),
        tbModeloOtros = $("#tbModeloOtros"),
        tbSerieOtros = $("#tbSerieOtros"),
        tbArregloOtros = $("#tbArregloOtros"),
        tbNumPoliza = $("#tbNumPoliza"),
        txtAreaNota = $("#txtAreaNota"),
        tbHorometro = $("#tbHorometro"),
        tbKilometraje = $("#tbKilometraje"),
        lblFechaTipoControl = $("#lblFechaTipoControl"),
        tbFechaTipoControl = $("#tbFechaTipoControl"),
        tbDiasTransalado = $("#tbDiasTransalado"),
        cboTanque1 = $("#cboTanque1"),
        cboTanque2 = $("#cboTanque2"),
        cboFallas = $("#cboFallas"),
        cboFactura = $("#cboFactura"),
        cboPedAduana = $("#cboPedAduana"),
        cboControlCalida = $("#cboControlCalida"),
        cboCatalogoPartes = $("#cboCatalogoPartes"),
        cboManualOperacion = $("#cboManualOperacion"),
        cboManualMantenimiento = $("#cboManualMantenimiento"),
        cboBitacora = $("#cboBitacora"),
        cboPlacas = $("#cboPlacas"),
        divListaPendientesLiberar = $("#divListaPendientesLiberar"),
        divTrafico = $("#divTrafico"),
        divHorometros = $("divHorometros"),
         divKilometraje = $("divKilometraje"),
         divPlacas = $("divPlacas"),
        ireport = $("#report")
        tbCompania = $("#tbCompania"),
        tbResponsable = $("#tbResponsable"),
        tbTransporte = $("#tbTransporte"),
        lblTipoResponsable = $("#lblTipoResponsable"),
        tbNombreResponsable = $("#tbNombreResponsable"),
        tbCompaniaResponsable = $("#tbCompaniaResponsable"),
        tbFirmaResponsable = $("#tbFirmaResponsable"),
        btnVerReporte = $("#btnVerReporte"),
        modalReportes = $("#modalReportes");

        fupAdjunto = $("#fupAdjunto"),
        lblTxtArchivo = $("#lblTxtArchivo"),
        cboMovimientoTipo = $("#cboMovimientoTipo"),
        tbGrupoMaquinariaModal = $("#tbGrupoMaquinariaModal");
        tbTipoMaquinariaModal = $("#tbTipoMaquinariaModal");
        tbModeloMaquinariaModal = $("#tbModeloMaquinariaModal"),
        tbHorasModal = $("#tbHorasModal");
        cboDestinos = $("#cboDestinos");
        tblEconomicosNoAsignados = $("#tblEconomicosNoAsignados"),
        modalListaEquiposAsignados = $("#modalListaEquiposAsignados"),
        tbNombreResponsableEnvio = $("#tbNombreResponsableEnvio"),
        tbCompaniaResponsableEnvio = $("#tbCompaniaResponsableEnvio"),
        tbNombreResponsableRecepcion = $("#tbNombreResponsableRecepcion"),
        tbCompaniaResponsableRecepcion = $("#tbCompaniaResponsableRecepcion");
        divTipoControl = $("#divTipoControl");
        mensajes = {
            NOMBRE: 'Control Envio y Recepcion',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        function init() {
            var id = $.urlParam('asignacion');


            cboTipoControl.change(fillInfo);
            cboFiltroEquipos.change(fillInfo);
            fillInfo();
            btnGuardar.click(Guardar);

            tbHorometro.change(checkHorometro);
            cboControlCalida.prop('disabled', true);
            btnVerReporte.click(PrintReport);
            cboMovimientoTipo.change(changeMovimiento);
            lblTxtArchivo.addClass('hide');
            lblTxtArchivo.click(downloadURI);

            $("#cboDestinos").change(SetDestino);

            
            var continuar = $.urlParam('continuar');

            if (continuar != null) {
                var idEconomicoP1 = $.urlParam('p1');
                var folioP2 = $.urlParam('p2');
                var idAsignacionP3 = $.urlParam('p3');
                var ccP4 = $.urlParam('p4');
                var nomCCP5 = $.urlParam('p5');
                var idSolicitudP6 = $.urlParam('p6');
                var continuar = $.urlParam('continuar');

                if (continuar != null) {
                    var idEconomicoP1 = $.urlParam('p1');
                    var folioP2 = $.urlParam('p2');
                    var idAsignacionP3 = $.urlParam('p3');
                    var ccP4 = $.urlParam('p4');
                    var nomCCP5 = "";
                    var idSolicitudP6 = $.urlParam('p6');
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: '/horometros/getCentroCostos',
                        data: { obj: ccP4 },
                        async: false,
                        success: function (response) {
                            nomCCP5 = response.centroCostos;
                            LoadInformacion(idEconomicoP1, folioP2, idAsignacionP3, ccP4, nomCCP5, 2, idSolicitudP6);
                        }
                    });
                }
            }
        }

        function setDestino() {
            tbLugarEnvio.attr('data-idcc', cboDestinos.val());
        }

        function downloadURI() {
            var link = document.createElement("a");
            link.download = '/MovimientoMaquinaria/getFileDownload?id=' + lblTxtArchivo.attr('data-idcontrol');
            link.href = '/MovimientoMaquinaria/getFileDownload?id=' + lblTxtArchivo.attr('data-idcontrol');
            link.click();
        }

        function PrintReport() {
            var id = btnVerReporte.attr('data-idReporte');
            var tipoControl = btnVerReporte.attr('data-tipoControl');
            LoadReporte(id, tipoControl);
        }

        function checkHorometro() {
            var HorometroSistema = tbHorometro.val();
            var HorometroActual = tbHorometro.attr('data-horometros');
            if (HorometroActual != 0 || HorometroActual != undefined) {
                if (HorometroSistema > (parseInt(HorometroActual) + 15)) {
                    AlertaGeneral("Alerta", "Su horometro es mayor al ultimo capturado verifique con el administrador de maquinaria.", "bg-red");
                    btnGuardar.prop("disabled", true);
                } else if (HorometroSistema < (parseInt(HorometroActual) - 15)) {
                    AlertaGeneral("Alerta", "Verifique que el horometro capturado sea el ultimo en sistema.", "bg-red");
                    btnGuardar.prop("disabled", true);
                }
                else {
                    btnGuardar.prop("disabled", false);
                }
                PermitirCeros = false;
            } else {
                PermitirCeros = true;
            }

        }

        function Guardar() {

            if (true) {
                var objeto = {};
                objeto.id = 0;
                objeto.tipoControl = cboTipoControl.attr('data-tipo');
                objeto.noEconomico = tbEconomico.attr('data-idEconomico');
                objeto.lugar = tbLugarEnvio.attr('data-idcc');
                objeto.fechaElaboracion = '01/01/2000'; //tblFechaActual.val();
                objeto.nota = txtAreaNota.val();
                objeto.horometros = tbHorometro.val() == "" ? 0 : tbHorometro.val();
                objeto.kilometraje = tbKilometraje.val() == "" ? 0 : tbKilometraje.val();
                objeto.fechaRecepcionEmbarque = '01/01/2000';//tbFechaTipoControl.val();


                objeto.diasTranslado = tbDiasTransalado.val() == "" ? 0 : tbDiasTransalado.val();
                objeto.tanque1 = cboTanque1.val();
                objeto.tanque2 = cboTanque2.val();
                objeto.pedAduana = cboPedAduana.val();
                objeto.controlCalidad = cboControlCalida.val();
                objeto.bitacora = cboBitacora.val();
                objeto.placas = cboPlacas.val() == "0" ? false : true;
                objeto.companiaTransporte = tbCompania.val();
                objeto.responsableTrasnporte = tbResponsable.val();
                objeto.Transporte = tbTransporte.val();
                objeto.nombreResponsable = tbNombreResponsable.val();
                objeto.compañiaResponsable = tbCompaniaResponsable.val();
                objeto.firma = tbFirmaResponsable.val();

                objeto.ReporteFalla = cboFallas.val();
                objeto.copiaFactura = cboFactura.val();
                objeto.manualOperacion = cboManualMantenimiento.val();
                objeto.manualMant = cboManualMantenimiento.val();

                objeto.solicitudEquipoID = Number(btnGuardar.attr('data-solicitud'));
                objeto.nombreResponsableEnvio = tbNombreResponsableEnvio.val();
                objeto.compañiaResponsableEnvio = tbCompaniaResponsableEnvio.val();
                objeto.nombreResponsableRecepcion = tbNombreResponsableRecepcion.val();
                objeto.compañiaResponsableRecepcion = tbCompaniaResponsableRecepcion.val();




                var idAsignacion = btnGuardar.attr('data-idAsignacion');

                if (setValidaciones()) {

                    if (cboTipoControl.val() != "3") {
                        //saveOrUpdate(objeto, idAsignacion);
                        saveOrUpdate(null, objeto, tbFechaTipoControl.val(), tblFechaActual.val(), idAsignacion);
                    }
                    else {
                        SendEspecial(null, objeto, idAsignacion, cboMovimientoTipo.val(), tbFechaTipoControl.val(), tblFechaActual.val());
                    }

                }
                else {
                    AlertaGeneral("Alerta", "Se encontraron valores sin agregar favor de verificarlos");
                }
            }


        }

        function setValidaciones() {
            var state = true;
            if ((tbHorometro.attr('data-Horometros') != undefined)) {
                if (!validarCampo(tbHorometro)) { state = false; }
            }


            if (cboTipoControl.attr('data-tipo') == "2") {
                tbDiasTransalado.addClass('required');
                if (!validarCampo(tbDiasTransalado)) { state = false; }
            }

            if (!validarCampo(tbCompania)) { state = false; }
            if (!validarCampo(tbResponsable)) { state = false; }
            if (!validarCampo(tbTransporte)) { state = false; }
            if (!validarCampo(tbNombreResponsableEnvio)) { state = false; }
            if (!validarCampo(tbCompaniaResponsableEnvio)) { state = false; }
            if (!validarCampo(tbNombreResponsableRecepcion)) { state = false; }
            if (!validarCampo(tbCompaniaResponsableRecepcion)) { state = false; }

            return state;
        }

        function saveOrUpdate(e, obj, fechaRecepcionEmbarque, fechaElaboracion, idAsignacion) {
            if (true) {

                var formData = new FormData();
                //var filesVisor = document.getElementById("fupAdjunto").files.length;
                var file = document.getElementById("fupAdjunto").files[0];

                formData.append("fupAdjunto", file);
                formData.append("obj", JSON.stringify(obj));
                formData.append("idAsignacion", JSON.stringify(idAsignacion));
                formData.append("fechaRecepcionEmbarque", JSON.stringify(fechaRecepcionEmbarque));
                formData.append("fechaElaboracion", JSON.stringify(fechaElaboracion));

                if (file != undefined) {
                    $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });
                }
                $.ajax({
                    type: "POST",
                    url: '/MovimientoMaquinaria/GuardarEnvioRecepcion',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        fupAdjunto.val("");
                        redirreccion();
                        var id = response.idControl;
                        LoadReporte(id, obj.tipoControl);
                        $.unblockUI();
                    },
                    error: function (error) {
                        $.unblockUI();
                    }
                });
            } else {
                e.preventDefault()
            }
        }

        function SendEspecial(e, objeto, idAsignacion, tipoEnvio, fechaRecepcionEmbarque, fechaElaboracion) {
            if (true) {

                var formData = new FormData();
                //var filesVisor = document.getElementById("fupAdjunto").files.length;
                var file = document.getElementById("fupAdjunto").files[0];

                formData.append("fupAdjunto", file);
                formData.append("tipoEnvio", JSON.stringify(tipoEnvio));
                formData.append("obj", JSON.stringify(objeto));
                formData.append("idAsignacion", JSON.stringify(idAsignacion));
                formData.append("fechaRecepcionEmbarque", JSON.stringify(fechaRecepcionEmbarque));
                formData.append("fechaElaboracion", JSON.stringify(fechaElaboracion));

                if (file != undefined) {
                    $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });
                }
                $.ajax({
                    type: "POST",
                    url: '/MovimientoMaquinaria/SaveSendEspecial',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        fupAdjunto.val("");
                        redirreccion();
                        var id = response.idControl;
                        LoadReporte(id, objeto.tipoControl);
                        $.unblockUI();
                    },
                    error: function (error) {
                        $.unblockUI();
                    }
                });
            } else {
                e.preventDefault()
            }
        }

        function SendEspecial1(objeto, idAsignacion, tipoEnvio) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/MovimientoMaquinaria/SaveSendEspecial',
                type: 'POST',
                dataType: 'json',
                data: { obj: objeto, idAsignacion: idAsignacion, tipoEnvio: tipoEnvio },
                success: function (response) {
                    //ConfirmacionGeneral("Confirmación", "El control de envio se guardo correctamente", "bg-green");
                    redirreccion();
                    var id = response.idControl;
                    LoadReporte(id, objeto.tipoControl);
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function UpdateAsignacion(idEconomico, idAsignacion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/MovimientoMaquinaria/UpdateAsignacion',
                type: 'POST',
                dataType: 'json',
                data: { idEconomico: idEconomico, idAsignacion: idAsignacion },
                success: function (response) {
                    ConfirmacionGeneral("Confirmación", "Se Asigno Correctamente el Equipo " + response.numEconomico, "bg-green");

                    var idEconomico = response.idEconomico;
                    var folio = response.Folio;
                    var idAsignacion = response.idAsignacion;
                    var cc = response.CCOrigen;
                    var nomCC = response.nomCC;
                    var idSolicitud = response.solicitudEquipoID;

                    btnGuardar.attr("data-solicitud", idSolicitud);
                    tblFechaActual.datepicker().datepicker("setDate", new Date());
                    tbFechaTipoControl.datepicker().datepicker("setDate", new Date());
                    divListaPendientesLiberar.addClass("hidden");
                    divTrafico.removeClass("hidden");

                    LoadInformacion(idEconomico, folio, idAsignacion, cc, nomCC, 1, idSolicitud);

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function bootG(url, TipoControl, tipoFiltro) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: url,
                type: "POST",
                datatype: "json",
                data: { obj: TipoControl, tipoFiltro: tipoFiltro },
                success: function (response) {
                    $.unblockUI();
                    var data = response.EquiposPendientes;
                    var EconomicosSinAsignar = response.EconomicosSinAsignar;
                    var ListaEconomicos = response.ListaEconomicos;
                    tblEquiposPendientes.bootgrid("clear");
                    tblEquiposPendientes.bootgrid("append", data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function LoadReporte(id, tipoControl) {
            $.blockUI({ message: mensajes.PROCESANDO });

            var path = "/Reportes/Vista.aspx?idReporte=13&pidRegistro=" + id + "&ptipoControl=" + tipoControl;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function changeMovimiento() {
            if (cboMovimientoTipo.val() == 0) {
                tbLugarEnvio.val('MAQUINARIA NO ASIGNADA A OBRA');
                tbLugarEnvio.attr('data-idcc', 997); //

            }
            else if (cboMovimientoTipo.val() == 1 || cboMovimientoTipo.val() == 2) {
                tbLugarEnvio.val('TALLER');
                tbLugarEnvio.attr('data-idcc', 10);
            }
        }

        function fillInfo() {

            var TipoControl = cboTipoControl.val();
            var TipoFiltro = cboFiltroEquipos.val();

            cboTipoControl.attr('data-tipo', TipoControl);

            switch (Number(TipoControl)) {
                case 1:
                case 3:
                    divTipoControl.removeClass('hide');
                    divMotivosEnvio.addClass('hide');
                    lblLugarControl.text("Lugar Envío:");
                    tbTipoControl.val("Envío");
                    lblFechaTipoControl.text("Fecha Embarque:");
                    lblTipoResponsable.text("Responsable Envió");
                    break;
                case 2:
                case 4:
                    divTipoControl.removeClass('hide');
                    divMotivosEnvio.addClass('hide');
                    lblLugarControl.text("Lugar Recepción:");
                    tbTipoControl.val("Recepción");
                    lblFechaTipoControl.text("Fecha Recepción:");
                    lblTipoResponsable.text("Responsable Recepción");
                    break;
                default:

            }
            bootG('/MovimientoMaquinaria/GetMaquinariasPendientesEnvios', TipoControl, TipoFiltro);
        }

        function iniciarGrid() {
            tblEquiposPendientes.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "VerCalidad": function (column, row) {

                        if (cboTipoControl.val() == "1") {
                            return "<button type='button' class='btn btn-primary verCalidad' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-tipo='1' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "' data-needAsignacion=" + row.needAsignacion + " data-SolicitudDetalleId=" + row.SolicitudDetalleId + ">" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                       " </button>"
                            ;
                        }
                        else if (cboTipoControl.val() == "2") {
                            return "<button type='button' class='btn btn-primary verCalidad' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-tipo='3' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "' data-needAsignacion=" + row.needAsignacion + " data-SolicitudDetalleId=" + row.SolicitudDetalleId + ">" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                       " </button>"
                            ;
                        }
                        else if (cboTipoControl.val() == "3") {
                            return "<button type='button' class='btn btn-primary verCalidad' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-tipo='5' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "' data-needAsignacion=" + row.needAsignacion + " data-SolicitudDetalleId=" + row.SolicitudDetalleId + ">" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                       " </button>"
                            ;
                        }
                        else if (cboTipoControl.val() == "4") {
                            return "<button type='button' class='btn btn-primary verCalidad' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-tipo='7' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "' data-needAsignacion=" + row.needAsignacion + " data-SolicitudDetalleId=" + row.SolicitudDetalleId + ">" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                       " </button>"
                            ;
                        }
                    },
                    "VerSolicitud": function (column, row) {

                        if (row.estatus == 2 && cboTipoControl.val() == "1") {
                            return "<button type='button' class='btn btn-primary verSolicitud' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "' data-needAsignacion=" + row.needAsignacion + " data-SolicitudDetalleId=" + row.SolicitudDetalleId + ">" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                       " </button>"
                            ;
                        }
                        if (row.estatus == 2 && row.isRenta) {
                            return "<button type='button' class='btn btn-primary verSolicitud' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "' data-needAsignacion=" + row.needAsignacion + " data-SolicitudDetalleId=" + row.SolicitudDetalleId + ">" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                       " </button>"
                            ;
                        }

                        if (row.estatus == 4 && cboTipoControl.val() == "2") {
                            return "<button type='button' class='btn btn-primary verSolicitud' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "' data-needAsignacion=" + row.needAsignacion + " data-SolicitudDetalleId=" + row.SolicitudDetalleId + ">" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                       " </button>"
                            ;
                        }
                        if (row.estatus == 6 && cboTipoControl.val() == "3") {
                            return "<button type='button' class='btn btn-primary verSolicitud' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "' data-needAsignacion=" + row.needAsignacion + " data-SolicitudDetalleId=" + row.SolicitudDetalleId + ">" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                       " </button>"
                            ;
                        }
                        if (row.estatus == 8 && cboTipoControl.val() == "4") {
                            return "<button type='button' class='btn btn-primary verSolicitud' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "' data-needAsignacion=" + row.needAsignacion + " data-SolicitudDetalleId=" + row.SolicitudDetalleId + ">" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                       " </button>"
                            ;
                        }
                    },
                    "verReporte": function (column, row) {
                        if (row.estatus >= 3 && cboTipoControl.val() == "1") {
                            return "<button type='button' class='btn btn-primary verReporte' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "'>" +
                          "<span class='glyphicon glyphicon-eye-open'></span> " +
                                 " </button>"
                            ;
                        }
                        if (row.estatus >= 5 && cboTipoControl.val() == "2") {
                            return "<button type='button' class='btn btn-primary verReporte' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "'>" +
                          "<span class='glyphicon glyphicon-eye-open'></span> " +
                                 " </button>"
                            ;
                        }
                        if (row.estatus >= 7 && cboTipoControl.val() == "3") {
                            return "<button type='button' class='btn btn-primary verReporte' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "' data-needAsignacion=" + row.needAsignacion + " data-SolicitudDetalleId=" + row.SolicitudDetalleId + ">" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                       " </button>"
                            ;
                        }
                        if (row.estatus == 10 && cboTipoControl.val() == "4") {
                            return "<button type='button' class='btn btn-primary verReporte' data-idEconomico=" + row.Economico + " data-idSolicitud=" + row.id + " data-Folio='" + row.Folio + "' data-idAsigacion='" + row.idAsigancion + "' data-CC='" + row.cc + "' data-ccname='" + row.CCName + "' data-needAsignacion=" + row.needAsignacion + " data-SolicitudDetalleId=" + row.SolicitudDetalleId + ">" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                       " </button>"
                            ;
                        }

                        return "";

                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblEquiposPendientes.find(".verCalidad").on('click', function (e) {
                    var needAsigancion = $(this).attr("data-needAsignacion");
                    var tipo = $(this).attr("data-tipo");
                    ImprimirCalidad(needAsigancion, tipo);

                });
                tblEquiposPendientes.find(".verSolicitud").on('click', function (e) {
                    var needAsigancion = $(this).attr("data-needAsignacion");

                    if (needAsigancion == "1") {
                        var idEconomico = $(this).attr("data-idEconomico");
                        var folio = $(this).attr("data-Folio");
                        var idAsignacion = $(this).attr('data-idAsigacion');
                        var cc = $(this).attr('data-CC');
                        var nomCC = $(this).attr('data-ccname');

                        var idSolicitud = $(this).attr('data-idsolicitud');

                        btnGuardar.attr("data-solicitud", $(this).attr("data-idsolicitud"));
                        tblFechaActual.datepicker().datepicker("setDate", new Date());
                        tbFechaTipoControl.datepicker().datepicker("setDate", new Date());
                        divListaPendientesLiberar.addClass("hidden");
                        divTrafico.removeClass("hidden");
                        LoadInformacion(idEconomico, folio, idAsignacion, cc, nomCC, 1, idSolicitud);


                    } else {
                        var idAsignacion = $(this).attr('data-idAsigacion');
                        GetEconomicosNoAsignados(idAsignacion);
                    }

                });

                tblEquiposPendientes.find(".verReporte").on('click', function (e) {

                    var idEconomico = $(this).attr("data-idEconomico");
                    var folio = $(this).attr("data-Folio");
                    var idAsignacion = $(this).attr('data-idAsigacion');
                    var cc = $(this).attr('data-CC');
                    var nomCC = $(this).attr('data-ccname');
                    var idSolicitud = $(this).attr('data-idsolicitud');
                    btnGuardar.attr("data-solicitud", $(this).attr("data-idsolicitud"));
                    tblFechaActual.datepicker().datepicker("setDate", new Date());
                    tbFechaTipoControl.datepicker().datepicker("setDate", new Date());
                    divListaPendientesLiberar.addClass("hidden");
                    divTrafico.removeClass("hidden");
                    LoadInformacion(idEconomico, folio, idAsignacion, cc, nomCC, 2, idSolicitud);


                });

            });

            tblEconomicosNoAsignados.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                formatters: {
                    "Asignar": function (column, row) {

                        return "<button type='button' class='btn btn-success verSolicitud' data-idEconomico=" + row.idEconomico + " data-idAsignacion=" + row.idAsignacion + ">" +
                                 "<span class='glyphicon glyphicon-plus'></span> " +
                                        " </button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblEconomicosNoAsignados.find(".verSolicitud").on('click', function (e) {
                    var idEconomico = $(this).attr("data-idEconomico");
                    var idAsignacion = $(this).attr('data-idAsignacion')

                    UpdateAsignacion(idEconomico, idAsignacion);
                });
            });

        }

        function GetEconomicosNoAsignados(idAsignacion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/MovimientoMaquinaria/GetEconomicosNoAsignados",
                type: "POST",
                datatype: "json",
                data: { idAsignacion: idAsignacion },
                success: function (response) {
                    $.unblockUI();
                    var ListaEconomicos = response.ListaEconomicos;
                    //Agregar las referencias
                    tbGrupoMaquinariaModal.val(response.GrupoMaquinaria)
                    tbTipoMaquinariaModal.val(response.TipoMaquinaria);
                    tbModeloMaquinariaModal.val(response.ModeloMaquinaria);
                    tbHorasModal.val(response.Horas);

                    if (ListaEconomicos != null) {

                        tblEconomicosNoAsignados.bootgrid("clear");
                        tblEconomicosNoAsignados.bootgrid("append", ListaEconomicos);
                        modalListaEquiposAsignados.modal('show');
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function LoadInformacion(id, folio, idAsignacion, cc, nomCC, accion, idSolicitud) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/MovimientoMaquinaria/GetDataFromEconomico',
                type: "POST",
                datatype: "json",
                data: { obj: id, tipoControl: cboTipoControl.attr('data-tipo'), tipoAccion: accion, idAsignacion: idAsignacion, idSolicitud: idSolicitud },
                success: function (response) {
                    $.unblockUI();
                    var data = response.infoEconomico;

                    if (response.EsTMC) {
                        $("#cboDestinos").removeClass('hide');
                        tbLugarEnvio.addClass('hide');
                        tbLugarEnvio.attr('data-idCC', 1010);

                    } else {
                        $("#cboDestinos").addClass('hide');
                        tbLugarEnvio.val(response.Lugar);
                        tbLugarEnvio.attr('data-idCC', response.idCC);
                    }
                    tbHorometro.removeClass('required');
                    if (response.tipodeCaptura == 1) {
                        tbHorometro.addClass('required');
                    }

                    tbTipoMaquina.val(response.tipo);
                    if (!response.esRenta || accion == 2) {
                        switch (cboTipoControl.val()) {
                            case "1":
                                {
                                    if (accion == 2) {
                                        var dataControl = response.dataControl;
                                        dataControlInfo(dataControl);
                                        tbFechaTipoControl.val(response.fechaRecepcionEmbarque);
                                    }

                                    break;
                                }
                            case "2":
                                {

                                    var dataControl = response.dataControl;
                                    if (accion == 1 && response.esRenta == false) {
                                        tbCompania.val(dataControl.companiaTransporte).prop('disabled', true);
                                        tbResponsable.val(dataControl.responsableTrasnporte).prop('disabled', true);
                                        tbTransporte.val(dataControl.Transporte).prop('disabled', true);
                                        tbHorometro.attr('data-H');
                                        tbNombreResponsableEnvio.val(dataControl.nombreResponsableEnvio).prop('disabled', true);
                                        tbCompaniaResponsableEnvio.val(dataControl.compañiaResponsableEnvio).prop('disabled', true);
                                        tbNombreResponsableRecepcion.val(dataControl.nombreResponsableRecepcion).prop('disabled', true);
                                        tbCompaniaResponsableRecepcion.val(dataControl.compañiaResponsableRecepcion).prop('disabled', true);
                                    }
                                    else {
                                        dataControlInfo(dataControl);
                                    }
                                    break;
                                }
                            case "3":
                                {
                                    if (accion == 2) {
                                        var dataControl = response.dataControl;
                                        divTipoControl.removeClass('hide');
                                        tbTipoControl.val('Envio');
                                        dataControlInfo(dataControl);

                                        tbCompania.val(dataControl.companiaTransporte).prop('disabled', true);
                                        tbResponsable.val(dataControl.responsableTrasnporte).prop('disabled', true);
                                        tbTransporte.val(dataControl.Transporte).prop('disabled', true);
                                        tbHorometro.attr('data-H')
                                        tbNombreResponsableEnvio.val(dataControl.nombreResponsableEnvio).prop('disabled', true);
                                        tbCompaniaResponsableEnvio.val(dataControl.compañiaResponsableEnvio).prop('disabled', true);
                                        tbNombreResponsableRecepcion.val(dataControl.nombreResponsableRecepcion).prop('disabled', true);
                                        tbCompaniaResponsableRecepcion.val(dataControl.compañiaResponsableRecepcion).prop('disabled', true);
                                    }

                                    break;
                                }
                            case "4":
                                {

                                    var dataControl = response.dataControl;
                                    if (accion == 1) {
                                        tbCompania.val(dataControl.companiaTransporte).prop('disabled', true);
                                        tbResponsable.val(dataControl.responsableTrasnporte).prop('disabled', true);
                                        tbTransporte.val(dataControl.Transporte).prop('disabled', true);

                                        tbNombreResponsableEnvio.val(dataControl.nombreResponsableEnvio).prop('disabled', true);
                                        tbCompaniaResponsableEnvio.val(dataControl.compañiaResponsableEnvio).prop('disabled', true);
                                        tbNombreResponsableRecepcion.val(dataControl.nombreResponsableRecepcion).prop('disabled', true);
                                        tbCompaniaResponsableRecepcion.val(dataControl.compañiaResponsableRecepcion).prop('disabled', true);
                                    }
                                    else {
                                        dataControlInfo(dataControl);

                                    }
                                    break;
                                }
                            default:

                        }
                    } else {
                        //var dataControl = response.dataControl;
                        //dataControlInfo(dataControl);
                        //tbFechaTipoControl.val(response.fechaRecepcionEmbarque);

                    }

                    if (response.tipo == "MAYOR" || response.tipo == "MENOR") {
                        $("#divPlacas").addClass('hidden');
                        $("#divKilometraje").addClass('hidden');
                        $("#divHorometros").removeClass('hidden');
                    }
                    else {
                        divPlacas.removeClass('hidden');
                        divKilometraje.removeClass('hidden');
                        divHorometros.addClass('hidden');
                    }
                    tbGrupoMaquina.val(response.grupo);
                    tbMarcaMaquina.val(response.marca);
                    tbModeloMaquina.val(response.modelo);
                    tbSerieMaquina.val(response.serie);
                    tbNumPoliza.val(response.noPoliza);
                    tbEconomico.val(response.Economico);

                    tbHorometro.attr('data-Horometros', response.Horometro);
                    tbEconomico.attr('data-idEconomico', id);

                    btnGuardar.attr('data-idAsignacion', idAsignacion);
                    tbFolio.val(folio);
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }

        function dataControlInfo(dataControl) {
            bloquear();


            txtAreaNota.val(dataControl.nota);
            tbHorometro.val(dataControl.horometros);

            // tbFechaTipoControl.val(dataControl.fechaElaboracion);
            tbDiasTransalado.val(dataControl.diasTranslado);
            cboTanque1.val(dataControl.tanque1);
            cboTanque2.val(dataControl.tanque2);
            cboFallas.val(dataControl.ReporteFalla ? "true" : "false").change();
            cboFactura.val(dataControl.copiaFactura ? "true" : "false").change();
            cboPedAduana.val(dataControl.pedAduana ? "true" : "false").change();
            cboControlCalida.val(dataControl.controlCalidad ? "true" : "false").change();
            cboManualOperacion.val(dataControl.manualOperacion ? "true" : "false").change();
            cboManualMantenimiento.val(dataControl.manualMant ? "true" : "false").change();
            cboBitacora.val(dataControl.bitacora ? "true" : "false").change();
            cboPlacas.val(dataControl.placas);
            tbCompania.val(dataControl.companiaTransporte).prop('disabled', true);
            tbResponsable.val(dataControl.responsableTrasnporte).prop('disabled', true);
            tbTransporte.val(dataControl.Transporte).prop('disabled', true);
            tbNombreResponsable.val(dataControl.nombreResponsable).prop('disabled', true);
            tbCompaniaResponsable.val(dataControl.compañiaResponsable).prop('disabled', true);

            tbNombreResponsableEnvio.val(dataControl.nombreResponsableEnvio);
            tbCompaniaResponsableEnvio.val(dataControl.compañiaResponsableEnvio);
            tbNombreResponsableRecepcion.val(dataControl.nombreResponsableRecepcion);
            tbCompaniaResponsableRecepcion.val(dataControl.compañiaResponsableRecepcion);
            lblTxtArchivo.text(dataControl.Nombre + "    ");
            lblTxtArchivo.append("<span class='glyphicon glyphicon-save-file'> </span>")
            lblTxtArchivo.attr('data-idControl', dataControl.id);

            btnGuardar.prop('disabled', true);
            btnVerReporte.removeClass('hidden');
            btnVerReporte.attr('data-idReporte', dataControl.id);
            btnVerReporte.attr('data-tipoControl', cboTipoControl.val());
        }

        function bloquear() {
            cboMovimientoTipo.prop('disabled', true);
            tbNumPoliza.prop('disabled', true);
            txtAreaNota.prop('disabled', true);
            tbHorometro.prop('disabled', true);
            cboBitacora.prop('disabled', true);
            cboPlacas.prop('disabled', true);
            cboFallas.prop('disabled', true);
            cboFactura.prop('disabled', true);
            tbFechaTipoControl.prop('disabled', true);
            tbDiasTransalado.prop('disabled', true);
            cboTanque1.prop('disabled', true);
            cboTanque2.prop('disabled', true);
            cboPedAduana.prop('disabled', true);
            cboManualOperacion.prop('disabled', true);
            cboManualMantenimiento.prop('disabled', true);
            cboBitacora.prop('disabled', true);
            tbNombreResponsableEnvio.prop('disabled', true);
            tbCompaniaResponsableEnvio.prop('disabled', true);
            tbNombreResponsableRecepcion.prop('disabled', true);
            tbCompaniaResponsableRecepcion.prop('disabled', true);
            fupAdjunto.addClass('hide');
            lblTxtArchivo.removeClass('hide');

        }

        function redirreccion() {
            fupAdjunto.removeClass('hide');
            divTrafico.addClass('hidden');
            divListaPendientesLiberar.removeClass('hidden');
            tbGrupoMaquina.val('');
            tbMarcaMaquina.val('');
            tbModeloMaquina.val('');
            tbSerieMaquina.val('');
            tbNumPoliza.val('');
            tbEconomico.val('');
            btnGuardar.attr('data-idAsignacion', '');
            tbFolio.val('');
            txtAreaNota.val('');
            tbTipoControl.val('');
            tbEconomico.val('');
            tbLugarEnvio.val('');
            tblFechaActual.val('');
            tbKilometraje.val('');
            tbHorometro.val('');
            tbFechaTipoControl.val('');
            tbDiasTransalado.val('');
            cboTanque1.val(0);
            cboTanque2.val(0);
            cboPedAduana.val("true");
            cboControlCalida.val("true");
            cboBitacora.val("true");
            cboPlacas.val(0);
            cboFallas.val("false");
            cboFactura.val("true");
            cboManualMantenimiento.val("true");
            cboManualOperacion.val("true");
            tbCompania.val('');
            tbResponsable.val('');
            tbTransporte.val('');
            tbNombreResponsable.val('');
            tbCompaniaResponsable.val('');
            tbFirmaResponsable.val('');
            btnGuardar.attr('data-idAsignacion', '');
            $('a[href^="#step-1"]').trigger('click');
            $('a[href^="#step-2"]').attr("disabled", "disabled");
            btnVerReporte.addClass('hidden');
            btnGuardar.prop('disabled', false);

            tbNombreResponsableEnvio.val('');
            tbCompaniaResponsableEnvio.val('');
            tbNombreResponsableRecepcion.val('');
            tbCompaniaResponsableRecepcion.val('');

            fillInfo();
        }

        function ImprimirCalidad(asignacion, tipo) {
            verReporteCalidad(22, "idAsignacion=" + asignacion + "&" + "TipoControl=" + tipo);
        }
        function verReporteCalidad(idReporte, parametros) {

            $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = idReporte;

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&" + parametros;
            ireport = $("#report");
            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();
                openCRModal();

            };
        }

        iniciarGrid();
        init();
    };

    $(document).ready(function () {
        maquinaria.inventario.MovimientoMaquinaria.ControlEnvioyRecepcion = new ControlEnvioyRecepcion();
    });
})();


