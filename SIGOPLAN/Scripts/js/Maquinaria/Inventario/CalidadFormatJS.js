(function () {

    $.namespace('maquinaria.inventario.ControlCalidadFormato');

    CalidadFormato = function () {

        mensajes = {
            NOMBRE: 'Control Envio y Recepcion',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        //// Accion de mejora 66-20 Agregar archivos al control de calidad y recepcion.
        const inputArchivoCheckList = $("#inputArchivoCheckList");
        const inputArchivoSetFotografico = $('#inputArchivoSetFotografico');
        const inputArchivoSOS = $('#inputArchivoSOS');
        const inputArchivoRehabilitacion = $('#inputArchivoRehabilitacion');
        const inputArchivoDN = $('#inputArchivoDN');
        const inputArchivoBitacora = $('#inputArchivoBitacora');
        const inputArchivoVidaAceites = $('#inputArchivoVidaAceites');

        const divSetFotografico = $('#divSetFotografico');
        const divInformeRehabilitacion = $('#divInformeRehabilitacion');
        const divSOS = $('#divSOS');
        const divBitacora = $('#divBitacora');
        const divDN = $('#divDN');
        const divCheckList = $('#divCheckList');

        let archivoSOS;
        let archivoBitacora;
        let archivoDN;
        let archivoSetFotografico;
        let archivoRehabilitacion;

        ////
        let d = new Date()
        const fechaHoy = `${d.getDate()}/${d.getMonth() + 1}/${d.getFullYear()}`
        ireport = $("#report")
        cboDestinos = $("#cboDestinos");
        lblIdSolicitud = $("#lblIdSolicitud");
        lblIdAsignacion = $("#lblIdAsignacion");
        lblIdCalidad = $("#lblIdCalidad");
        lblIdEconomico = $("#lblIdEconomico");
        lblTipoControl = $("#lblTipoControl");
        txtTipoControl = $("#txtTipoControl");
        txtFolio = $("#txtFolio");
        txtNoEconomico = $("#txtNoEconomico");
        txtDateTime = $("#txtDateTime");
        txtHorometro = $("#txtHorometro");
        txtObra = $("#txtObra");
        txtOrigen = $("#txtOrigen");
        txtDestino = $("#txtDestino");
        txtMarcaMotor = $("#txtMarcaMotor");
        txtModeloMotor = $("#txtModeloMotor");
        txtSerieMotor = $("#txtSerieMotor");
        txtCompania = $("#txtCompania");
        txtVehiculo = $("#txtVehiculo");
        txtOperador = $("#txtOperador");
        txtObservaciones = $("#txtObservaciones");

        /*Anexo de Controles*/
        tbHorometro = $("#tbHorometro");
        tbKilometraje = $("#tbKilometraje");
        tbFechaTipoControl = $("#tbFechaTipoControl");
        tbDiasTransalado = $("#tbDiasTransalado");
        cboTanque1 = $("#cboTanque1");
        cboTanque2 = $("#cboTanque2");
        cboFallas = $("#cboFallas");
        cboFactura = $("#cboFactura");
        cboPedAduana = $("#cboPedAduana");
        cboControlCalida = $("#cboControlCalida");
        cboManualOperacion = $("#cboManualOperacion");
        cboManualMantenimiento = $("#cboManualMantenimiento");
        cboBitacora = $("#cboBitacora");
        divPlacas = $("#divPlacas");
        txtAreaNota = $("#txtAreaNota");
        tbCompania = $("#tbCompania");
        tbResponsable = $("#tbResponsable");
        tbTransporte = $("#tbTransporte");
        tbNombreResponsableEnvio = $("#tbNombreResponsableEnvio");
        tbCompaniaResponsableEnvio = $("#tbCompaniaResponsableEnvio");
        tbNombreResponsableRecepcion = $("#tbNombreResponsableRecepcion");
        tbCompaniaResponsableRecepcion = $("#tbCompaniaResponsableRecepcion");
        cboPlacas = $("#cboPlacas");

        btnGuardar = $("#btnGuardar");
        totalGrupos = $("#totalGrupos");
        totalPreguntas = $("#totalPreguntas");

        txtDesDestino = $("#txtDesDestino");
        btnModalGuardar = $("#btnModalGuardar");
        modalControl = $("#modalControl");
        txtConEnvio = $("#txtConEnvio");
        txtConRecepcion = $("#txtConRecepcion");

        function init() {
            addEListener();

            tbFechaTipoControl.datepicker().datepicker("setDate", new Date());
            //ajuste para no mover mucho codigo
            var url = window.location.href;
            var lastChar = url.substr(url.length - 1);
            if (lastChar != "N") {
                getRespuestas();
            } else {
                txtObservaciones.val("");
            }
            getDataControl();

            // GetTransportista();
            if (Number($.urlParam('Tipo')) >= 3) {
                localizacion = "1015-TALLER MECANICO CENTRAL";
                txtDestino.val(localizacion);
                txtObra.val(txtOrigen.val());
                cboDestinos.change();
                if (Number($.urlParam('Tipo')) == 3) {

                }
            }
        }

        function addEListener() {
            tbNombreResponsableEnvio.change(SetCompania1);
            tbNombreResponsableRecepcion.change(SetCompania2);
            tbCompania.change(SetResponsable);
            btnGuardar.click(fnConfirmacionGuardado);
            btnModalGuardar.click(GuardarCalidad);
            tbHorometro.change(fnValidarHorometro);
            cboDestinos.trigger('change');
            cboDestinos.change(setDestino);
        }

        function fnConfirmacionGuardado() {

            txtConEnvio.val(txtOrigen.val());
            txtConRecepcion.val(getCCDestino());
            modalControl.modal("show");
        }

        function fnValidarHorometro() {

            if ($(this).attr('data-aplicaHrometro')) {
                if ($(this).val() != $(this).attr('data-horometros')) {
                    AlertaGeneral("Alerta", "El horometro no esta al dia.");
                }
            }


        }

        function SetCompania1() {
            if (tbNombreResponsableEnvio.val() != "0") {
                tbCompaniaResponsableEnvio.val("CONSTRUPLAN");
            }
        }

        function SetCompania2() {
            if (tbNombreResponsableRecepcion.val() != "0") {
                tbCompaniaResponsableRecepcion.val("CONSTRUPLAN");
            }
        }

        function SetResponsable() {
            tbResponsable.val('');
            if (tbCompania.val() != "0") {
                var Responsable = $("#tbCompania option:selected").text();
                tbResponsable.val(Responsable);

                if (cboDestinos.val() == "3" || cboDestinos.val() == "4") {
                    txtDestino.val(Responsable);
                }
            }
        }

        function setDestino() {
            var tipo2 = 0;
            var CCusuarioRecibe = "";
            localizacion = "";
            switch (cboDestinos.val()) {
                case "1010":
                    localizacion = "1010-TALLER MECANICO CENTRAL";
                    tipo2 = 1;
                    CCusuarioRecibe = "1015";
                    txtDesDestino.val(localizacion);
                    break;
                case "1015":
                    localizacion = "1015-PATIO MAQUINARIA HERMOSILLO";
                    CCusuarioRecibe = "1010";
                    txtDesDestino.val(localizacion);
                    tipo2 = 1;
                    break;
                case "1097":

                    localizacion = "ENVÍO PROVEEDOR";
                    txtDesDestino.val("");
                    break;
                case "10997":
                    localizacion = "VENTA DE EQUIPO";
                    txtDesDestino.val("");
                    break;
                case "1":
                    txtDesDestino.val($("#cboDestinos option:selected").text());
                    break;

                default:
            }
            txtDestino.val(localizacion);
            if (tipo2 != 0) {
                tbNombreResponsableRecepcion.fillCombo('/ControlesMovimientoEquipo/FillCboResponsableControles', { cc: CCusuarioRecibe, tipo: tipo2 });
            }
        }

        function GuardarCalidad() {
            $.blockUI({ message: mensajes.PROCESANDO });
            //#region CAmbio 
            /* var lstFull = [];
 
             for (var x = 1; x <= totalPreguntas.text(); x++) {
                 objRespuesta = {};
 
                 if ($("#tr-" + x).attr('data-tipo') == 1) {
                     objRespuesta = {
                         id: $("#tr-" + x).attr('data-respuesta'),
                         IdPregunta: $("#tr-" + x).attr('id').slice(3),
                         Respuesta: $("#tr-" + x).find("td.value").attr("data-option"),
                         Cantidad: $("#inp-" + x).val()
                     }
                 }
                 else if ($("#tr-" + x).attr('data-tipo') == 2) {
                     objRespuesta = {
                         id: $("#tr-" + x).attr('data-respuesta'),
                         IdPregunta: $("#tr-" + x).attr('id').slice(3),
                         Respuesta: $("#tr-" + x).find("td.value").attr("data-option"),
                         Cantidad: $("#inp-" + x).val(),
                         Marca: $("#mar-" + x).val(),
                         Serie: $("#ser-" + x).val()
                     }
                 }
                 if ($("#tr-" + x).attr('data-tipo') == 3) {
                     objRespuesta = {
                         id: $("#tr-" + x).attr('data-respuesta'),
                         IdPregunta: $("#tr-" + x).attr('id').slice(3),
                         Marca: $("#mar-" + x).val(),
                         Medida: $("#med-" + x).val(),
                         Serie: $("#ser-" + x).val(),
                         VidaUtil: $("#vid-" + x).val()
                     }
                 }
                 lstFull[x - 1] = objRespuesta;
             }
             EnvioNormal = lblTipoControl.text();*/
            //#endregion
            if (validarCampos()) {
                $.ajax({
                    type: 'POST',
                    url: "/ControlCalidad/Guardar",
                    data: crearObjetosGuardar(),
                    contentType: false,
                    processData: false,
                    cache: false,
                    success: function (response) {
                        if (response.success) {
                            $.unblockUI();
                            var objAsignacion = response.items.IdAsignacion;
                            var TipoControl = response.items.TipoControl;
                            var solicitud = lblIdSolicitud.text();
                            GetDataIdReporte(objAsignacion, TipoControl, solicitud);
                        } else {
                            AlertaGeneral('Alerta', response.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert('Error: ' + error);
                    }
                });
            }
            else {
                $.unblockUI();
            }
        }

        function crearObjetosGuardar() {
            var lstFull = [];

            for (var x = 1; x <= totalPreguntas.text(); x++) {
                objRespuesta = {};

                if ($("#tr-" + x).attr('data-tipo') == 1) {
                    objRespuesta = {
                        id: $("#tr-" + x).attr('data-respuesta'),
                        IdPregunta: $("#tr-" + x).attr('id').slice(3),
                        Respuesta: $("#tr-" + x).find("td.value").attr("data-option"),
                        Cantidad: $("#inp-" + x).val()
                    }
                }
                else if ($("#tr-" + x).attr('data-tipo') == 2) {
                    objRespuesta = {
                        id: $("#tr-" + x).attr('data-respuesta'),
                        IdPregunta: $("#tr-" + x).attr('id').slice(3),
                        Respuesta: $("#tr-" + x).find("td.value").attr("data-option"),
                        Cantidad: $("#inp-" + x).val(),
                        Marca: $("#mar-" + x).val(),
                        Serie: $("#ser-" + x).val()
                    }
                }
                if ($("#tr-" + x).attr('data-tipo') == 3) {
                    objRespuesta = {
                        id: $("#tr-" + x).attr('data-respuesta'),
                        IdPregunta: $("#tr-" + x).attr('id').slice(3),
                        Marca: $("#mar-" + x).val(),
                        Medida: $("#med-" + x).val(),
                        Serie: $("#ser-" + x).val(),
                        VidaUtil: $("#vid-" + x).val()
                    }
                }
                lstFull[x - 1] = objRespuesta;
            }
            let archivoSetFotografico = inputArchivoSetFotografico.get(0).files[0];
            let archivoRehabilitacion = inputArchivoRehabilitacion.get(0).files[0];
            let archivoCheckList = inputArchivoCheckList.get(0).files[0];
            let archivoDN = null;
            let archivoSOS = null;
            let archivoBitacora = null;
            let archivoVidaAceites = null;

            if (inputArchivoSOS.val() != '') {
                archivoSOS = inputArchivoSOS.get(0).files[0];
            }
            if (inputArchivoDN.val() != '') {
                archivoDN = inputArchivoDN.get(0).files[0];
            }
            if (inputArchivoBitacora.val() != '') {
                archivoBitacora = inputArchivoBitacora.get(0).files[0];
            }
            if (inputArchivoVidaAceites.val() != '') {
                archivoVidaAceites = inputArchivoVidaAceites.get(0).files[0];
            }

            let formData = new FormData();
            let EnvioNormal = lblTipoControl.text();

            formData.append('archivoSetFotografico', archivoSetFotografico);
            formData.append('archivoRehabilitacion', archivoRehabilitacion);
            formData.append('archivoChecklist', archivoCheckList);
            formData.append('archivoDN', archivoDN);
            formData.append('archivoSOS', archivoSOS);
            formData.append('archivoBitacora', archivoBitacora);
            formData.append('archivoVidaAceites', archivoVidaAceites);
            formData.append('objCalidad', JSON.stringify(getObjCalidad()));
            formData.append('lstRespuestas', JSON.stringify(lstFull));
            formData.append('objControl', JSON.stringify(getObjControl()));
            formData.append('Destino', getCCDestino());
            formData.append('EnvioNormal', EnvioNormal);
            formData.append('DestinoCBOX', getCCDestinoID());
            return formData;

        }

        function GetDataIdReporte(idAsignacion, TipoControl, solicitudID) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/ControlCalidad/GetIDControl',
                type: 'POST',
                dataType: 'json',
                data: { asignacionID: idAsignacion, TipoControl: TipoControl, solicitudID: solicitudID },
                success: function (response) {
                    if (response.success) {
                        var id = response.ControlID;
                        var tipoControl = TipoControl;
                        var path = "/Reportes/Vista.aspx?idReporte=193&pidRegistro=" + id + "&ptipoControl=" + tipoControl + "&idAsignacion=" + idAsignacion;
                        ireport.attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            $.ajax({
                                url: '/ControlCalidad/enviarCorreosCalidad',
                                type: 'POST',
                                dataType: 'json',
                                success: function (response) {
                                    if (lblTipoControl.text() == "1" || lblTipoControl.text() == "4") {
                                        window.location.href = "/ControlCalidad/index/?Tipo=E"
                                    }
                                    else {
                                        window.location.href = "/ControlCalidad/index/?Tipo=R"
                                    }
                                },
                                error: function (response) {
                                    $.unblockUI();
                                    AlertaGeneral("Alerta", response.message);
                                }
                            });
                        };
                    } else {
                        AlertaGeneral('Alerta', response.message);
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function validarCampos() {
            var state = true;

            if (tbHorometro.hasClass('requiered')) {
                if (!validarCampo(tbHorometro)) { return false; }
            }
            if (txtTipoControl.val() != 'Recepcion') {
                if (inputArchivoSetFotografico.val() == "") {
                    if (inputArchivoSetFotografico.hasClass('requiered')) {
                        validarCampo(inputArchivoSetFotografico)
                        return false;
                    }
                }
                if (inputArchivoRehabilitacion.val() == "") {
                    if (inputArchivoRehabilitacion.hasClass('requiered')) {
                        validarCampo(inputArchivoRehabilitacion)
                        return false;
                    }
                }
                if (inputArchivoDN.val() == "") {
                    if (inputArchivoDN.hasClass('requiered')) {
                        validarCampo(inputArchivoDN)
                        return false;
                    }
                }
                if (inputArchivoSOS.val() == "") {
                    if (inputArchivoSOS.hasClass('requiered')) {
                        validarCampo(inputArchivoSOS)
                        return false;
                    }
                }
                if (inputArchivoBitacora.val() == "") {
                    if (inputArchivoBitacora.hasClass('requiered')) {
                        validarCampo(inputArchivoBitacora)
                        return false;
                    }
                }
            }
            else {
                if (inputArchivoCheckList.val() == "") {
                    if (inputArchivoCheckList.hasClass('requiered')) {
                        validarCampo(inputArchivoCheckList)
                        return false;
                    }
                }
            }
            return state;
        }

        function getRespuestas() {

            $.ajax({
                url: "/ControlCalidad/GetRespuestas",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ id: lblIdCalidad.text() }),
                success: function (response) {

                    var count = response.length;

                    for (var x = 0; x < count; x++) {
                        var aux = x + 1;

                        $("#tr-" + aux).attr("data-respuesta", response[x].id);

                        if (response[x].Respuesta == 1) {
                            var td = $("#tr-" + aux).find("td.bueno")
                            td.parent().find('input').prop('disabled', false);
                            td.parent().find('input').val(1);
                            td.siblings(".value").attr("data-option", "1");
                            td.addClass('Activo');
                            td.addClass("bg-primary");
                        }
                        if (response[x].Respuesta == 2) {
                            var td = $("#tr-" + aux).find("td.regular")
                            td.siblings(".value").attr("data-option", "2");
                            td.parent().find('input').prop('disabled', false);
                            td.parent().find('input').val(1);
                            td.addClass('Activo');
                            td.addClass("bg-primary");
                        }
                        if (response[x].Respuesta == 3) {
                            var td = $("#tr-" + aux).find("td.malo")
                            td.parent().find('input').prop('disabled', false);
                            td.parent().find('input').val(1);
                            td.siblings(".value").attr("data-option", "3");
                            td.addClass('Activo');
                            td.addClass("bg-primary");
                        }
                        if (response[x].Respuesta == 0) {
                            var td = $("#tr-" + aux).find("td.null")
                            td.parent().find('input').prop('disabled', true);
                            td.parent().find('input').val(0);
                            td.siblings(".value").attr("data-option", "0");
                            td.addClass('Activo');
                            td.addClass("bg-primary");
                        }

                        $("#inp-" + aux).val(response[x].Cantidad);

                        if ($("#mar-" + aux).length > 0) {
                            $("#mar-" + aux).val(response[x].Marca);
                        }
                        if ($("#ser-" + aux).length > 0) {
                            $("#ser-" + aux).val(response[x].Serie);
                        }
                        if ($("#med-" + aux).length > 0) {
                            $("#med-" + aux).val(response[x].Medida);
                        }
                        if ($("#vid-" + aux).length > 0) {
                            $("#vid-" + aux).val(response[x].VidaUtil);
                        }
                    }


                },
                error: function (response) {
                    alert(response.message);
                }
            });

        }

        ///Funcion para cargar la información de la maquinaria, y setear los datos en la interfaz-
        function getDataControl() {
            $.ajax({
                url: "/ControlesMovimientoEquipo/GetDataFromEconomico",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ AsignacionID: lblIdAsignacion.text() }),
                success: function (response) {
                    tbHorometro.removeClass('requiered');
                    var objDetalleControlDTO = response.objDetalleControlDTO;
                    var TipoCaptura = objDetalleControlDTO.TipoCaptura;
                    var Compania = response.Compania;
                    var CCEnvio = "";
                    var CCusuarioRecibe = "";//response.CCusuarioRecibe;

                    let archivoSOS = response.archivoSOS;
                    let archivoBitacora = response.archivoBitacora;
                    let archivoDN = response.archivoDN;
                    let archivoSetFotografico = response.archivoSetFotografico;
                    let archivoRehabilitacion = response.archivoRehabilitacion;


                    if (Number($.urlParam('Tipo')) < 3) {

                        CCEnvio = response.CCusuarioEnvia;
                        CCusuarioRecibe = response.CCusuarioRecibe;
                    }
                    else {

                        CCEnvio = response.CCusuarioRecibe
                        CCusuarioRecibe = "997";
                    }
                    tbHorometro.attr('data-aplicaHrometro', false);
                    switch (TipoCaptura) {
                        case 1:
                            tbHorometro.addClass('requiered');
                            tbHorometro.attr('data-horometros', response.UltimoHorometro);
                            tbHorometro.attr('data-aplicaHrometro', true);
                            break;
                        case 2:
                            tbHorometro.prop('disabled', true);
                            tbHorometro.val('0');
                            tbHorometro.removeClass('requiered');
                            break;
                        case 0:
                            tbHorometro.prop('disabled', true);
                            tbHorometro.val('0');
                            tbHorometro.removeClass('requiered');
                            tbKilometraje.prop('disabled', true);
                            tbKilometraje.val('0');
                            break;
                        default:

                    }
                    tipo1 = 0;
                    tipo2 = 0;
                    if (lblTipoControl.text() == "3" || lblTipoControl.text() == "4") {
                        tipo2 = 1;
                    }

                    ///Se agrego el dia 01/09/2020  Accion de mejora #66-20
                    if (archivoSetFotografico) {
                        inputArchivoSetFotografico.addClass('requiered');
                        divSetFotografico.removeClass('hide');
                    }
                    else {
                        inputArchivoSetFotografico.removeClass('requiered');
                        divSetFotografico.addClass('hide');
                    }

                    if (archivoRehabilitacion) {
                        inputArchivoRehabilitacion.addClass('requiered');
                        divInformeRehabilitacion.removeClass('hide');
                    }
                    else {
                        inputArchivoRehabilitacion.removeClass('requiered');
                        divInformeRehabilitacion.addClass('hide');
                    }

                    if (archivoSOS) {
                        inputArchivoSOS.addClass('requiered');
                        divSOS.removeClass('hide');
                    }
                    else {
                        inputArchivoSOS.removeClass('requiered');
                        divSOS.addClass('hide');
                    }

                    if (archivoBitacora) {
                        inputArchivoBitacora.addClass('requiered');
                        divBitacora.removeClass('hide');
                    }
                    else {
                        inputArchivoBitacora.removeClass('requiered');
                        divBitacora.addClass('hide');
                    }

                    if (archivoDN) {
                        inputArchivoDN.addClass('requiered');
                        divDN.removeClass('hide');
                    }
                    else {
                        inputArchivoDN.removeClass('requiered');
                        divDN.addClass('hide');
                    }

                    if (txtTipoControl.val() == "Recepcion") {
                        inputArchivoSetFotografico.removeClass('requiered').addClass('hide');
                        inputArchivoRehabilitacion.removeClass('requiered').addClass('hide');
                        inputArchivoDN.removeClass('requiered').addClass('hide');
                        inputArchivoBitacora.removeClass('requiered').addClass('hide');
                        inputArchivoSOS.removeClass('requiered').addClass('hide');
                        inputArchivoCheckList.addClass('requiered').removeClass('hide');

                        divSetFotografico.addClass('hide');
                        divInformeRehabilitacion.addClass('hide');
                    }

                    tbCompaniaResponsableEnvio.val(Compania);
                    tbCompaniaResponsableRecepcion.val(Compania);
                    tbNombreResponsableEnvio.fillCombo('/ControlesMovimientoEquipo/FillCboResponsableControles', { cc: CCEnvio, tipo: tipo1 }, null, null, () => {
                        tbNombreResponsableEnvio.find('option:eq(2)').prop('selected', true);
                    });
                    tbNombreResponsableRecepcion.fillCombo('/ControlesMovimientoEquipo/FillCboResponsableControles', { cc: CCusuarioRecibe, tipo: tipo2 });
                    // $("#tbNombreResponsableEnvio option:contains(" + objDetalleControlDTO.UsuarioEnviaNombre + ")").attr("selected", "selected");
                    $("#tbNombreResponsableRecepcion option:contains(" + objDetalleControlDTO.UsuarioRecibeNombre + ")").attr("selected", "selected");
                },
                error: function (response) {
                    alert(response.message);
                }
            });
        }

        function ConfirmacionGuardadoCalidad(titulo, mensaje, color) {

            // if (!$("#dialogalertaGeneral").is(':visible')) {
            var html = '<div id="dialogalertaGeneral" class="modal fade" role="dialog">' +
                '<div class="modal-dialog">' +
                '<div class="modal-content">' +
                '<div class="modal-header text-center">' +
                '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
                '&times;</button>' +
                '<h4  class="modal-title">' + titulo + '</h4>' +
                '</div>' +
                '<div class="modal-body">' +
                '<div class="container">' +
                '<div class="row">' +
                '<div class="col-lg-12">' +
                '<h3> <span class="glyphicon glyphicon-ok-circle ' + color + '" aria-hidden="true" style="font-size:40px;"></span> <label style="position: fixed;">' + mensaje + '</label></h3>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '<div class="modal-footer">' +
                '<a id="btndialogalertaGeneral" href="/ControlCalidad/index" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
                '</div>' +
                '</div>' +
                '</div></div>';
            $("#txtComentarioAlerta").text('');
            //var _this = $(html);
            //  _this.modal("show");
            $("#dialogalertaGeneral").dialog({
                resizable: false,
                height: "auto",
                title: titulo,
                width: 400,
                modal: true,
                close: function (event, ui) { window.location.href = "/ControlCalidad/index"; },
                buttons: {
                    "Aceptar": function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#txtComentarioAlerta").text(mensaje);
            $("#dialogalertaGeneral").dialog();
            // }
        }

        function initTableCuestionario() {
            $(".tableCuestionario").find("td").on("click", function () {
                var td = $(this);
                if (td.hasClass("bueno") || td.hasClass("regular") || td.hasClass("malo") || td.hasClass("null")) {
                    quitarActivos(td);
                    if (td.hasClass("bueno")) {
                        td.parent().find('input').prop('disabled', false);
                        td.parent().find('input').val(1);
                        td.siblings(".value").attr("data-option", "1");
                        td.addClass('Activo');
                        td.addClass("bg-primary");
                    }
                    if (td.hasClass("regular")) {
                        td.siblings(".value").attr("data-option", "2");
                        td.parent().find('input').prop('disabled', false);
                        td.parent().find('input').val(1);
                        td.addClass('Activo');
                        td.addClass("bg-primary");
                    }
                    if (td.hasClass("malo")) {
                        td.parent().find('input').prop('disabled', false);
                        td.parent().find('input').val(1);
                        td.siblings(".value").attr("data-option", "3");
                        td.addClass('Activo');
                        td.addClass("bg-primary");
                    }
                    if (td.hasClass("null")) {
                        td.parent().find('input').prop('disabled', true);
                        td.parent().find('input').val(0);
                        td.siblings(".value").attr("data-option", "0");
                        td.addClass('Activo');
                        td.addClass("bg-primary");
                    }
                }
            });
        }

        function quitarActivos(td) {
            if ($(".tableCuestionario").find("td").hasClass('Activo')) {
                td.siblings(".Activo").removeClass('bg-primary');
                td.siblings(".Activo").removeClass('Activo');
                td.siblings(".value").attr("data-option", "");
            }
        }

        function getObjCalidad() {
            return {
                Id: lblIdCalidad.text(),
                IdSolicitud: lblIdSolicitud.text(),
                IdAsignacion: lblIdAsignacion.text(),
                TipoControl: lblTipoControl.text(),
                Folio: txtFolio.val(),
                IdEconomico: lblIdEconomico.text(),
                NoEconomico: txtNoEconomico.val(),
                FechaCaptura: moment(fechaHoy, 'DD/MM/YYYY').toISOString(true),//txtDateTime.val(),
                Horometro: txtHorometro.val(),
                Obra: txtObra.val(),
                CcOrigen: txtOrigen.val(),
                CcDestino: getCCDestino(),// txtDesDestino.val() == "" ? cboDestinos.val() : txtDestino.val(),
                MarcaMotor: txtMarcaMotor.val(),
                ModeloMotor: txtModeloMotor.val(),
                SerieMotor: txtSerieMotor.val(),
                CompañiaTraslado: txtCompania.val(),
                VehiculoTraslado: txtVehiculo.val(),
                OperadorTraslado: txtOperador.val(),
                Observaciones: txtObservaciones.val()
            }
        }

        function getCCDestino() {

            switch (lblTipoControl.text()) {
                case "1":
                case "2":
                    return txtDestino.val();
                case "3":
                case "4":
                    return $("#cboDestinos option:selected").text();
                default:
                    return "";
            }
        }
        function getCCDestinoID() {

            switch (lblTipoControl.text()) {
                case "1":
                case "2":
                    return "";
                case "3":
                case "4":
                    return cboDestinos.val();
                default:
                    return "";
            }
        }
        function getObjControl() {
            var objeto = {};
            objeto.id = 0;
            objeto.tipoControl = lblTipoControl.text();
            objeto.noEconomico = lblIdEconomico.text();
            objeto.lugar = "";
            objeto.fechaElaboracion = moment(fechaHoy, 'DD/MM/YYYY').toISOString(true);
            objeto.nota = txtAreaNota.val();
            objeto.horometros = tbHorometro.val() == "" ? 0 : tbHorometro.val();
            objeto.kilometraje = tbKilometraje.val() == "" ? 0 : tbKilometraje.val();
            objeto.fechaRecepcionEmbarque = moment(tbFechaTipoControl.val(), 'DD/MM/YYYY').toISOString(true);
            objeto.diasTranslado = tbDiasTransalado.val() == "" ? 0 : tbDiasTransalado.val();
            objeto.tanque1 = cboTanque1.val();
            objeto.tanque2 = cboTanque2.val();
            objeto.pedAduana = cboPedAduana.val();
            objeto.controlCalidad = cboControlCalida.val();
            objeto.bitacora = cboBitacora.val();
            objeto.placas = cboPlacas.val() == "0" ? false : true;
            objeto.companiaTransporte = $("#tbCompania option:selected").text();
            objeto.responsableTrasnporte = tbResponsable.val();
            objeto.Transporte = tbTransporte.val();
            objeto.nombreResponsable = $("#tbNombreResponsableEnvio option:selected").text();
            objeto.compañiaResponsable = tbCompaniaResponsableEnvio.val();
            objeto.firma = "";
            objeto.ReporteFalla = cboFallas.val();
            objeto.copiaFactura = cboFactura.val();
            objeto.manualOperacion = cboManualMantenimiento.val();
            objeto.manualMant = cboManualMantenimiento.val();
            objeto.solicitudEquipoID = Number(lblIdSolicitud.text());
            objeto.nombreResponsableEnvio = $("#tbNombreResponsableEnvio option:selected").text();
            objeto.compañiaResponsableEnvio = tbCompaniaResponsableEnvio.val();
            objeto.nombreResponsableRecepcion = $("#tbNombreResponsableRecepcion option:selected").text();;
            objeto.compañiaResponsableRecepcion = tbCompaniaResponsableRecepcion.val();
            return objeto;
        }

        init();
        initTableCuestionario();
    };

    $(document).ready(function () {
        maquinaria.inventario.ControlCalidadFormato = new CalidadFormato();
    });
})();