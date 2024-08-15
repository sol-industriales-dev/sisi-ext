(function () {

    $.namespace('maquinaria.inventario.Solicitud.ElaboracionDeSolicitud');

    ElaboracionDeSolicitud = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'ElaboracionDeSolicitud',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        var Update = 0;
        var btnGlobal;
        var ListaRows = [];
        var ListaTable = [];
        var Countid = 0;
        var ListaPrograma = [];
        var listaJustificaciones = [];
        frmSolicitud = $("#frmSolicitud"),
            FrmModalProgramMaquinaria = $("#FrmModalProgramMaquinaria"),
            Step1 = $("#Step1"),
            Step2 = $("#Step2"),
            Step3 = $("#Step3"),
            btnArranque = $("#btnArranque"),
            cboConcepto = $("#cboConcepto"),
            fuEvidencia = $("#fuEvidencia"),
            otraJustificacion = $(".otraJustificacion"),
            txtOtraJustificacion = $("#txtOtraJustificacion"),
            //Inicializacion de Selectores:
            //Inicio Pestaña 1 
            txtCondicionInicial = $("#txtCondicionInicial"),
            txtCondicionActual = $("#txtCondicionActual"),
            tblJustificacion = $("#tblJustificacion"),
            tbodyJustificacion = $("#tbodyJustificacion"),
            txtHorasUso = $("#txtHorasUso"),
            tbFolioSolicitud = $("#tbFolioSolicitud"),
            tbCC = $("#tbCC"),
            tbDescripcionCC = $("#tbDescripcionCC"),
            tbSingleDate = $("#tbSingleDate"),
            tbElabora = $("#tbElabora"),
            tbGerenteObra = $("#tbGerenteObra"),
            tbDirectorDivision = $("#tbDirectorDivision"),
            tbDirectorServicios = $("#tbDirectorServicios"),
            tbAltaDireccion = $("#tbAltaDireccion"),
            tbGerenteDirector = $("#tbGerenteDirector"),
            //Botones de Accion
            btnSiguiente1 = $("#btnSiguiente1"),
            btnFolioSolicitud = $("#btnFolioSolicitud"),
            //Fin Pestaña 1 
            divUtilizacion = $("#divUtilizacion"),
            //Pestaña2
            tblSolicitudesMaquinaria = $("#tblSolicitudesMaquinaria"),
            radioPrioridad1 = $("#radioPrioridad1"),
            radioPrioridad2 = $("#radioPrioridad2"),
            radioPrioridad3 = $("#radioPrioridad3")
        tbFechaIni = $("#tbFechaIni"),
            tbFechaFin = $("#tbFechaFin"),
            TbHorasUtilizacion = $("#TbHorasUtilizacion")
        btnRegresar = $("#btnRegresar"),
            //Botones Accion
            cboFolio = $("#cboFolio"),
            btnAgregarSolicitud = $("#btnAgregarSolicitud"),
            btnSiguiente2 = $("#btnSiguiente2"),
            txtMensaje = $("#txtMensaje"),
            //Modales
            cboDirector = $("#cboDirector"),
            modalMaquinariaProgramada = $("#modalMaquinariaProgramada"), // Equipo ya asignado.
            modalProgramMaquinaria = $("#modalProgramMaquinaria"), //Se Agregan los equipos
            btnGuardarPrograma = $("#btnGuardarPrograma"),
            tblEquiposSolicitados = $("#tblEquiposSolicitados"),
            cboTipoMaquinaria = $("#cboTipoMaquinaria"),
            cboGrupoMaquinaria = $("#cboGrupoMaquinaria"),
            cboModeloMaquinaria = $("#cboModeloMaquinaria"),
            tbCantidadSolicitada = $("#tbCantidadSolicitada"),
            btnGuardarComentario = $("#btnGuardarComentario"),
            txtAComentarios = $("#txtAComentarios"),
            ModalTbHorasUtilizacion = $("#ModalTbHorasUtilizacion"),

            divEquiposSolicitadosA = $("#divEquiposSolicitadosA"),
            divEquiposSolicitadosN = $("#divEquiposSolicitadosN"),
            //fin Pestaña 2
            tblAsignacionEquipos = $("#tblAsignacionEquipos"),
            modalConfirmacion = $("#modalConfirmacion"),
            ireport = $("#report"),
            btnGuardarSolicitud = $("#btnGuardarSolicitud");

        InfObjeto = {};

        var ListaEconomicosFillCbo = [];

        function init() {

            SetToggle();
            $('[data-toggle="tooltip"]').tooltip();
            cboModeloMaquinaria.attr('disabled', true);
            cboGrupoMaquinaria.attr('disabled', true);

            tbSingleDate.datepicker().datepicker("setDate", new Date());

            $('input[name=radioTipoUtilizacion]:radio').change(setTipo);
            $('input[name=radioInline1]:radio').change(getFechas);
            getFechas();

            tbElabora.val(NombreUsuario).prop('disabled', true);
            tbElabora.attr("data-id", idUsuario);

            cboTipoMaquinaria.fillCombo('/CatGrupos/FillCboTipoMaquinaria', { estatus: true });
            cboTipoMaquinaria.change(FillCboGrupo);
            cboGrupoMaquinaria.change(FillCboModelo);
            tbCC.onEnter(GetNombreCC);
            btnAgregarSolicitud.click(NextStep);
            btnGuardarSolicitud.click(GuardarSolicitud);

            btnGuardarPrograma.click(guardarMaquinas);
            addValidation();

            btnSiguiente2.click(getDataToSend);
            btnSiguiente1.click(setSolicitud);
            btnFolioSolicitud.click(editarFolio);
            btnGuardarComentario.click(GuardarComentario);
            tbFolioSolicitud.onEnter(editarFolio);
            tbCantidadSolicitada.keydown(alertaMensaje);
            ModalTbHorasUtilizacion.keydown(alertaMensaje);
            btnRegresar.click(evento);
            cboFolio.change(getDataFolio);
            initJustificacion();
            cboConcepto.change(function () {
                var _this = $(this);
                if (_this.val() == 5) {
                    otraJustificacion.val("");
                    otraJustificacion.show();
                }
                else {
                    otraJustificacion.hide();
                    otraJustificacion.val("");
                }
            });
        }
        function initJustificacion() {

        }
        function alertaMensaje() {
            txtMensaje.text("");
        }

        function setTipo() {
            ModalTbHorasUtilizacion.val('').prop('disabled', false);
            if ($('input[name=radioInline1]:checked').val() == '1') {
                txtHorasUso.text("Total Utilización Horas por grupo:");
            }
            switch ($('input[name=radioTipoUtilizacion]:checked').val()) {
                case "1":
                    txtHorasUso.text("Total Utilización Horas por grupo:");
                    divUtilizacion.removeClass('hide');
                    break;
                case "2":
                    txtHorasUso.text("Total Utilización KM por grupo:");
                    divUtilizacion.addClass('hide');
                    break;
                case "3":
                    txtHorasUso.text("Utilización en tiempo:");
                    divUtilizacion.addClass('hide');
                    ModalTbHorasUtilizacion.val('NA').prop('disabled', true);
                    break;
                default:
            }

        }

        function updateAutorizador() {
            if ($(this).val() == "") {
                $(this).val('');
                $(this).removeAttr('data-id');
            }
        }

        function evento() {
            $('a[href^="#Step2"]').trigger('click');
        }

        function autoComplete() {
            //     tbGerenteObra.getAutocomplete(fillGerente, getDataIdAutorizadores(), '/SolicitudEquipo/GetInfoUsuarios');
            // tbDirectorDivision.getAutocomplete(fillDirector, getDataIdAutorizadores(), '/SolicitudEquipo/GetInfoUsuarios');
            //tbAltaDireccion.getAutocomplete(fillAltaDireccion, getDataIdAutorizadores(), '/SolicitudEquipo/GetInfoUsuarios');
        }

        function getDataIdAutorizadores() {
            var obj = {};
            obj.gerente = tbGerenteObra.val() == null || tbGerenteObra.val() == "" ? 0 : tbGerenteObra.val();
            obj.director = tbDirectorDivision.val() == null || tbDirectorDivision.val() == "" ? 0 : tbDirectorDivision.val();
            obj.servicios = tbDirectorServicios.val() == null || tbDirectorServicios.val() == "" ? 0 : tbDirectorServicios.val();
            obj.direccion = tbAltaDireccion.val() == null || tbAltaDireccion.val() == "" ? 0 : tbAltaDireccion.val();

            obj.Elabora = tbElabora.attr('data-id') == null || tbElabora.val() == "" ? 0 : tbElabora.attr('data-id');

            return obj;
        }

        function getFechas() {

            var now = new Date();
            month = now.getMonth();
            dias = now.getDate();
            year = now.getYear() + 1900;
            if ($('input[name=radioInline1]:checked').val() == 'A') {
                dias = dias + 3;

            } else if ($('input[name=radioInline1]:checked').val() == 'B') {
                dias = dias + 15;
            }
            else if ($('input[name=radioInline1]:checked').val() == 'C') {
                dias = dias + 10;
            }

            tbFechaIni.datepicker("option", {
                minDate: new Date(year, month, dias),
                maxDate: null
            });
            tbFechaFin.datepicker("option", {
                minDate: new Date(year, month, dias),
                maxDate: null
            });
            datePicker(year, month, dias);
            tbFechaIni.datepicker("setDate", new Date(year, month, dias));
            tbFechaFin.datepicker("setDate", new Date(year, month, dias + 1));

        }

        function GuardarGeneral() {
            var flag = true;
            var listaComentarios = $('.ComentarioEquipo');
            var listaHoras = $('.HorasEquipo');
            var tempDis = ListaTable;
            listaHoras.addClass('required');

            var idTabla = $('.ComentarioEquipo').attr('data-idPrincipal');
            var tabla = getInfo(idTabla);
            $.each(listaComentarios, function (index, value) {
                $.each(tabla, function (i, v) {
                    if ($(value).attr('data-id') == v.id) {
                        v.Comentario = $(value).val();
                    }
                });
            });
            var totalHoras = 0;

            $.each(listaHoras, function (index, value) {
                $.each(tabla, function (i, v) {
                    if ($(value).attr('data-id') == v.id) {
                        validarCampo($(value));
                        if ($(value).val() == 0 || $(value).attr('data-valida') == "false") {
                            flag = false;
                        }
                        v.pHoras = $(value).val();
                    }
                });
            });
            if (flag) {
                ConfirmacionGeneral("Confirmación", "Se modificaron correctamente los registros.", "bg-green");
                modalMaquinariaProgramada.modal('hide');
            }
            else {
                ConfirmacionGeneral("Confirmación", "El total de las horas debe coincidir", "bg-red");
            }

        }

        function GuardarComentario() {
            var TablaActual;
            var flag = true;
            var ListaEconomicosActual;
            var ListaComentariosActuales;
            var ListaHorasActuales;
            var isArranque = $("#ckArranque").is(':checked');
            if (!isArranque) {
                //TablaActual = $("#tblEquiposSolicitados");

                ListaComentariosActuales = $("#tblEquiposSolicitados").children('tbody').find('input.ComentarioEquipo');
                ListaHorasActuales = $("#tblEquiposSolicitados").children('tbody').find('input.HorasEquipo');
            }
            else {
                ListaComentariosActuales = $("#tblAsignacionEquipos").children('tbody').find('input.ComentarioEquipo');
                ListaHorasActuales = $("#tblAsignacionEquipos").children('tbody').find('input.HorasEquipo');
            }


            $.each(ListaComentariosActuales, function (index, value) {
                $.each(ListaTable, function (i, v) {
                    var objInput = $(value).data();
                    if (objInput.id == v.id && objInput.idprincipal == Number(v.idPrincipal)) {
                        ListaTable[i].Comentario = $(value).val();
                        if (isArranque) {
                            ListaTable[i].Economico = $(value).parents('tr').find('.EconomicoDat').find("option:selected").text() != "Seleccione:" ? $(value).parents('tr').find('.EconomicoDat').find("option:selected").text() : "";

                            ListaTable[i].idNoEconomico = $(value).parents('tr').find('.EconomicoDat').val() != "Seleccione:" ? $(value).parents('tr').find('.EconomicoDat').val() : "";
                        }
                    }


                });
            });

            $.each(ListaHorasActuales, function (index, value) {
                $.each(ListaTable, function (i, v) {
                    var objInput = $(value).data();
                    if (objInput.id == v.id && objInput.idprincipal == Number(v.idPrincipal)) {

                        if ($(value).val() == 0 || $(value).attr('data-valida') == "false") {
                            flag = false;
                        }

                        ListaTable[i].pHoras = $(value).val();
                    }
                });
            });


            if (flag) {
                ConfirmacionGeneral("Confirmación", "Se modificaron correctamente los registros.", "bg-green");
                modalMaquinariaProgramada.modal('hide');
            }
            else {
                ConfirmacionGeneral("Confirmación", "El total de las horas debe coincidir", "bg-red");
            }
        }

        function guardarMaquinas() {
            var valDivision = 0;
            var HacerDivision = true;

            if (tbCantidadSolicitada.val() > 0) {
                if (listaJustificaciones.length == 0) {
                    var obj = {};
                    obj.id = 0;
                    obj.solicitudID = 0;
                    obj.grupoID = cboGrupoMaquinaria.val();
                    obj.grupo = $("#cboGrupoMaquinaria option:selected").text();
                    obj.modeloID = cboModeloMaquinaria.val();
                    obj.modelo = $("#cboModeloMaquinaria option:selected").text();
                    obj.justificacion = "";
                    listaJustificaciones.push(obj);
                    var html = '<tr class="clsJustificacion"><td style="width:350px;">' + obj.grupo + '</td><td style="width:150px;">' + obj.modelo + '</td><td><textarea class="txtJustificacion" style="width:100%;" rows="2" data-grupoid="' + obj.grupoID + '" data-grupo="' + obj.grupo + '"  data-modeloid="' + obj.modeloID + '" data-modelo="' + obj.modelo + '"></textarea></td></tr>';
                    tbodyJustificacion.append(html);
                }
                else {
                    var obj = {};
                    obj.grupoID = cboGrupoMaquinaria.val();
                    obj.grupo = $("#cboGrupoMaquinaria option:selected").text();
                    obj.modeloID = cboModeloMaquinaria.val();
                    obj.modelo = $("#cboModeloMaquinaria option:selected").text();
                    obj.justificacion = "";
                    var existe = listaJustificaciones.find(x => x.grupoID == obj.grupoID && x.modeloID == obj.modeloID);
                    if (existe == undefined) {
                        listaJustificaciones.push(obj);
                        var html = '<tr class="clsJustificacion"><td style="width:350px;">' + obj.grupo + '</td><td style="width:150px;">' + obj.modelo + '</td><td><textarea class="txtJustificacion" style="width:100%;" rows="2" data-grupoid="' + obj.grupoID + '" data-grupo="' + obj.grupo + '"  data-modeloid="' + obj.modeloID + '" data-modelo="' + obj.modelo + '"></textarea></td></tr>';
                        tbodyJustificacion.append(html);
                    }
                }
                switch ($('input[name=radioTipoUtilizacion]:checked').val()) {
                    case "1":
                        if (ModalTbHorasUtilizacion.val() > 0) {
                            valDivision = ModalTbHorasUtilizacion.val() % tbCantidadSolicitada.val();
                            var DivisionHoras = ModalTbHorasUtilizacion.val() / tbCantidadSolicitada.val();


                            SetMaquinariaPrograma(true, Math.round(DivisionHoras));

                        }
                        break;
                    case "2":
                        SetMaquinariaPrograma(false, ModalTbHorasUtilizacion.val());
                        break;
                    case "3":
                        // SetMaquinariaPrograma(false, ModalTbHorasUtilizacion.val());
                        SetMaquinariaPrograma(false, 0);
                        break;
                    default:

                }

            }
            else {
                ConfirmacionGeneral("Alerta", "Debe Añadir llenar el formulario Completo.", "bg-red");
            }

        }

        function SetMaquinariaPrograma(flag, horas) {
            var ListaObjetos = new Object();


            var key = btnGlobal.attr('data-id');
            getCountData(key);

            var id = getCountData(key) + 1;
            banderaAdd = true;


            var horasTotales = $('input[name=radioTipoUtilizacion]:checked').val() == "1" ? ModalTbHorasUtilizacion.val() : "NA";

            $.each(ListaTable, function (i, v) {
                if (v.Grupoid == cboGrupoMaquinaria.val()) {
                    if (horasTotales != "NA" && banderaAdd) {
                        horasTotales = Number(v.HorasTotales) + Number(ModalTbHorasUtilizacion.val());
                        banderaAdd = false;
                    }
                    if (!banderaAdd) {
                        v.HorasTotales = horasTotales;
                    }

                }
            });

            for (var i = 0; i < tbCantidadSolicitada.val(); i++) {
                var Objeto = {};
                var tempId = id + i;
                Objeto.id = tempId;
                Objeto.Tipoid = cboTipoMaquinaria.val();
                Objeto.Grupoid = cboGrupoMaquinaria.val();
                Objeto.Modeloid = cboModeloMaquinaria.val();
                Objeto.Tipo = cboTipoMaquinaria.find("option:selected").text();
                Objeto.Grupo = cboGrupoMaquinaria.find("option:selected").text();
                Objeto.Modelo = cboModeloMaquinaria.find("option:selected").text();
                Objeto.Descripcion = txtAComentarios.val();
                Objeto.pHoras = horas,
                    Objeto.HasPrograma = "0";
                Objeto.HorasTotales = horasTotales;
                Objeto.pFechaInicio = btnGlobal.attr('data-pFechaInicio');
                Objeto.pFechaFin = btnGlobal.attr('data-pFechaFin');
                Objeto.pTipoPrioridad = btnGlobal.attr('data-pTipoPrioridad');
                Objeto.idPrincipal = key;
                Objeto.Comentario = "";
                Objeto.letDivision = flag;
                Objeto.tipoUtilizacion = $('input[name=radioTipoUtilizacion]:checked').val();
                Objeto.Economico = "";
                Objeto.idNoEconomico = "";
                ListaTable.push(Objeto);
            }
            CboEconomicos(cboGrupoMaquinaria.val(), key)


            ConfirmacionGeneral("Confirmación", "Se añadio equipo al programa.", "bg-green");
            btnSiguiente2.prop("disabled", false);
            modalProgramMaquinaria.modal('hide');
            limpiarInformacion();

            InfObjeto.HorasTotales = ModalTbHorasUtilizacion.val() == "NA" ? 0 : ModalTbHorasUtilizacion.val();

        }

        function getCountData(obj) {
            var retorno = 0;
            $.each(ListaTable, function (index, value) {
                if (value.idPrincipal == obj) {
                    retorno = value.id;
                }
            });
            return retorno;
        }

        ///Obtiene informacion de la tabla que se va llenar. la segunda tabla
        function getInfo(obj) {

            obj = Number(obj);
            var ObjetoTabla = [];
            var auxHorasTotales = 0;
            $.each(ListaTable, function (index, value) {
                if (value.idPrincipal == obj) {
                    auxHorasTotales += Number(value.pHoras);
                }
            });

            $.each(ListaTable, function (index, value) {
                if (value.idPrincipal == obj) {
                    value.HorasTotales = auxHorasTotales;
                    ObjetoTabla.push(value);
                }
            });
            return ObjetoTabla;
        }

        function editarFolio() {
            if (tbFolioSolicitud.is(':enabled')) {
                tbFolioSolicitud.prop("disabled", true);
                cboFolio.addClass('hide');
                tbFolioSolicitud.removeClass('hide');
                var Folio = tbFolioSolicitud.val();
                var patt = new RegExp("[0-9]+-[-0-9]*");

                if (tbFolioSolicitud != "" && patt.test(Folio)) {
                    LOADDataSolicitud();
                }
            }
            else {
                if (tbDescripcionCC.val() != "") {
                    tbFolioSolicitud.prop("disabled", false);
                    tbFolioSolicitud.addClass('hide');
                    cboFolio.removeClass('hide');
                    cboFolio.fillCombo('/SolicitudEquipo/FillCboSolicitudes', { CC: tbCC.val() });
                }
                else {
                    AlertaGeneral("Alerta", "Debe seleccionar un centro de costos primero");
                }
            }
        }

        function getDataFolio() {
            tblSolicitudesMaquinaria.bootgrid('clear');
            var folio = cboFolio.val();
            tbFolioSolicitud.val(folio);

            LOADDataSolicitud();
        }

        function LOADDataSolicitud() {
            $.ajax({
                url: '/SolicitudEquipo/LoadSolicitud',
                type: 'POST',
                data: { Folio: tbFolioSolicitud.val() },
                success: function (response) {
                    var TipoSolicitud = response.EsArranque;


                    if (TipoSolicitud == true) {
                        btnArranque.trigger('click');
                        ListaEconomicosFillCbo = response.ListaEconomicosFillCbo;
                    }
                    var DatosSolicitud = response.DatosSolicitud;

                    tbFolioSolicitud.attr('data-idFolio', DatosSolicitud.id);
                    var DataosSolicitudDetalle = response.DatosSolicitudDet;
                    var Autorizadores = response.DatosAutorizadores;
                    tbElabora.val(Autorizadores.Elabora).prop('disabled', true);
                    tbElabora.attr('data-id', Autorizadores.ElaboraId);

                    //  tbGerenteObra.val(Autorizadores.Gerente).prop('disabled', true);
                    // tbGerenteObra.attr('data-id', Autorizadores.GerenteId);

                    tbGerenteObra.val(Autorizadores.GerenteId);
                    tbGerenteDirector.val(Autorizadores.GerenteDirectorId);
                    tbDirectorDivision.val(Autorizadores.DirectorId);//.prop('disabled', true);
                    tbDirectorServicios.val(Autorizadores.ServiciosId);
                    // tbDirectorDivision.attr('data-id', Autorizadores.DirectorId);
                    tbAltaDireccion.val(Autorizadores.DireccionId);//.prop('disabled', true);
                    // tbAltaDireccion.attr('data-id', Autorizadores.DireccionId);
                    tbCC.val(DatosSolicitud.CC);//.prop('disabled', true);
                    tbSingleDate.val(response.FechaElaboracion);
                    tbDescripcionCC.val(response.nombreCentroCostos);
                    var ListaProgramaT = response.ListaPrograma;
                    tblSolicitudesMaquinaria.bootgrid("append", ListaProgramaT);
                    ListaTable = response.ListaTable;

                    Update = 1;

                    Countid = ListaProgramaT.length;

                    btnSiguiente2.prop('disabled', false);
                },
                error: function (response) {
                }
            });
        }

        function valid(tipo) {
            var state = true;

            if (!cboTipoMaquinaria.valid()) { state = false; }
            if (!cboGrupoMaquinaria.valid()) { state = false; }
            if (!cboModeloMaquinaria.valid()) { state = false; }
            if (!ModalTbHorasUtilizacion.valid()) { state = false; }

            return state;
        }

        function NextStep() {

            if (true) {
                fillTablePrograma();
                btnSiguiente1.prop("disabled", false);
                ConfirmacionGeneral("Confirmación", "Se añadio un programa nuevo.", "bg-green");
            }
            else {
                ConfirmacionGeneral("Alerta", "Debe llenar todos los campos", "bg-red");
            }
        }

        function GetNombreCC() {
            Update = 0;
            cboFolio.clearCombo();
            tbDescripcionCC.val('');
            tbFolioSolicitud.val('').prop('disabled', true);
            cboFolio.addClass('hide');
            tbFolioSolicitud.removeClass('hide');
            getName(tbCC.val());

            if ($("#ckArranque").is(':checked')) {
                SetArranqueObra();
                $(".divJustificacion").hide();
                limpiarJustificacion();
            }
            else {
                tbGerenteObra.fillCombo('/SolicitudEquipo/FillCboAutorizadores', { CC: tbCC.val(), autorizador: 1 }, true);
                tbDirectorDivision.fillCombo('/SolicitudEquipo/FillCboAutorizadores', { CC: tbCC.val(), autorizador: 2 }, true);
                tbDirectorServicios.fillCombo('/SolicitudEquipo/FillCboAutorizadores', { CC: tbCC.val(), autorizador: 11 }, true);
                tbAltaDireccion.fillCombo('/SolicitudEquipo/FillCboAutorizadores', { CC: tbCC.val(), autorizador: 3 }, true);
                tbGerenteDirector.fillCombo('/SolicitudEquipo/FillCboAutorizadores', { CC: tbCC.val(), autorizador: 4 }, true);
                $(".divJustificacion").show();
                limpiarJustificacion();
            }

        }
        function limpiarJustificacion() {
            cboConcepto.val(1);
            fuEvidencia.val("");
            txtOtraJustificacion.val("");
            txtCondicionInicial.val("");
            txtCondicionActual.val("");
            tbodyJustificacion.val("");
            tbodyJustificacion.empty();
        }
        function getName(obj) {
            $.ajax({
                url: '/SolicitudEquipo/GetInfoSolicitud',
                type: 'POST',
                data: { obj: obj },
                success: function (response) {
                    if (response.success != 'False') {
                        if (response.descripcionCC != '') {
                            tbDescripcionCC.val(response.descripcionCC);
                            tbFolioSolicitud.val(response.folio);
                        }
                        else {
                            tbDescripcionCC.val();
                            tbFolioSolicitud.val();
                        }
                    }
                    else if (response.message != '') {
                        AlertaGeneral('Error', response.message);
                    }
                },
                error: function (response) {
                }
            });
        };

        function FillCboGrupo() {
            if (cboTipoMaquinaria.val() != "") {
                cboGrupoMaquinaria.fillCombo('/CatGrupos/FillCboGrupoMaquina', { obj: cboTipoMaquinaria.val() });
                cboGrupoMaquinaria.attr('disabled', false);
            }
            else {
                cboGrupoMaquinaria.clearCombo();
                cboGrupoMaquinaria.attr('disabled', true);
            }
        }

        function FillCboModelo() {
            if (cboGrupoMaquinaria.val() != "") {
                cboModeloMaquinaria.fillCombo('/CatModeloEquipo/FillCboModelo', { obj: cboGrupoMaquinaria.val() });
                cboModeloMaquinaria.attr('disabled', false);
                cboModeloMaquinaria.append($('<option>', {
                    value: cboGrupoMaquinaria.val() + "1",
                    text: "OTRO"
                }));
            }
            else {
                cboModeloMaquinaria.clearCombo();
                cboModeloMaquinaria.attr('disabled', true);
            }
        }

        function addValidation() {
            cboTipoMaquinaria.addClass('required');
            cboGrupoMaquinaria.addClass('required');
            cboModeloMaquinaria.addClass('required');

            tbCC.addClass('required');
            tbSingleDate.addClass('required');
            tbFechaIni.addClass('required');
            tbFechaFin.addClass('required');
            tbGerenteObra.addClass('required');
            tbDirectorDivision.addClass('required');
            tbDirectorServicios.addClass('required');
            tbAltaDireccion.addClass('required');
            tbFolioSolicitud.addClass('required');
        }

        function limpiarData() {
            tbFechaIni.datepicker().datepicker("setDate", new Date());
            tbFechaFin.datepicker().datepicker("setDate", new Date());
            ModalTbHorasUtilizacion.val('');
            radioPrioridad2.prop('checked', true);

        }

        function limpiarInformacion() {
            cboTipoMaquinaria.val('');
            cboGrupoMaquinaria.clearCombo();
            cboGrupoMaquinaria.attr('disabled', true);
            cboModeloMaquinaria.clearCombo();
            cboModeloMaquinaria.attr('disabled', true);
            tbCantidadSolicitada.val(0);
            ModalTbHorasUtilizacion.val(0);


        }

        function insertPrograma() {

            if (true) {
                updateTable(btnGlobal.attr("data-id"));
                tblSolicitudesMaquinaria.bootgrid('clear');
                tblSolicitudesMaquinaria.bootgrid("append", ListaRows);

                modalProgramMaquinaria.modal('hide');

                btnGlobal.attr("data-pFechaInicio", tbFechaIni.val());
                btnGlobal.attr("data-pFechaFin", tbFechaFin.val());
                btnGlobal.attr("data-pHoras", ModalTbHorasUtilizacion.val());

                btnGlobal.attr("data-pTipoPrioridad", $('input[name=radioInline1]:checked').val());

                btnSiguiente2.attr('disabled', false);
                limpiarData();
            }
        }

        function iniciarGrid() {
            tblSolicitudesMaquinaria.bootgrid({
                headerCssClass: '.bg-table-header',
                rows: -1,
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "AddEquipos": function (column, row) {

                        return "<button type='button' class='btn btn-success addEquipo' data-id='" + row.id + "' data-id='" + row.id + "' data-pFechaInicio='" + row.FechaInicio + "'" +
                            " data-pFechaFin='" + row.FechaFin + "' " + " data-pHoras='" + row.Horas + "' " + " data-pTipoPrioridad='" + row.TipoPrioridad + "'>" +
                            "<span class='glyphicon glyphicon-plus '></span> " +
                            " </button>"
                            ;

                    },
                    "VerEquipos": function (column, row) {

                        return "<button type='button' class='btn btn-primary verEquipos' data-id='" + row.id + "' data-pFechaInicio='" + row.FechaInicio + "'" +
                            " data-pFechaFin='" + row.FechaFin + "' " + " data-pHoras='" + row.Horas + "' " + " data-pTipoPrioridad='" + row.TipoPrioridad + "' >" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                            " </button>"
                            ;

                    },
                    "Remover": function (column, row) {

                        return "<button type='button' class='btn btn-danger RemoverPrograma' data-id='" + row.id + "' data-pFechaInicio='" + row.FechaInicio + "'" +
                            " data-pFechaFin='" + row.FechaFin + "' " + " data-pHoras='" + row.Horas + "' " + " data-pTipoPrioridad='" + row.TipoPrioridad + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;

                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */

                var rows = tblSolicitudesMaquinaria.bootgrid('getCurrentRows');
                //   fillComboTable(rows);

                tblSolicitudesMaquinaria.find(".addEquipo").on('click', function (e) {

                    LimpiarModal();
                    btnGlobal = $(this);
                    modalProgramMaquinaria.modal('show');

                });

                tblSolicitudesMaquinaria.find(".verEquipos").on('click', function (e) {

                    var dataTable = getInfo($(this).attr('data-id'));

                    var isArranque = $("#ckArranque").is(':checked');

                    if (!isArranque) {
                        divEquiposSolicitadosN.removeClass('hide');
                        divEquiposSolicitadosA.addClass('hide');
                        initGridSecundario();
                        tblEquiposSolicitados.bootgrid('clear');
                        tblEquiposSolicitados.bootgrid("append", dataTable);
                    } else {
                        divEquiposSolicitadosN.addClass('hide');
                        divEquiposSolicitadosA.removeClass('hide');
                        initGridArranqueObra();
                        tblAsignacionEquipos.bootgrid('clear');
                        tblAsignacionEquipos.bootgrid("append", dataTable);
                    }

                    modalMaquinariaProgramada.modal('show');
                });

                tblSolicitudesMaquinaria.find(".RemoverPrograma").on('click', function (e) {

                    var idDocumento = $(this).attr('data-id');

                    for (var i = 0; i < ListaTable.length; i++) {
                        if (ListaTable[i].idPrincipal == idDocumento) {
                            var obj = ListaEconomicosFillCbo.find(x => x.idPrincipal === idDocumento);
                            ListaEconomicosFillCbo.splice(ListaEconomicosFillCbo.indexOf(obj), 1);
                            var idv = ListaTable[i].idTempSolicitudes;
                            if (idv != null) {
                                removePrograma(idv);
                            }
                        }
                    }

                    ListaPrograma = $.grep(ListaPrograma,
                        function (o, i) { return o.id == Number(idDocumento); },
                        true);
                    ListaTable = $.grep(ListaTable,
                        function (o, i) { return o.idPrincipal == Number(idDocumento); },
                        true);

                    tblSolicitudesMaquinaria.bootgrid('clear');
                    tblSolicitudesMaquinaria.bootgrid("append", ListaPrograma);

                });
            });
        }

        function fillComboTable(data) {
            if (data.length > 0) {
                $("#tblAsignacionEquipos tbody tr").each(function (index) {
                    if (data[index].id != 0) {
                        $(this).find('select.EconomicoDat').val(data[index].idNoEconomico);
                    } else {
                        $("#tblAsignacionEquipos").find('select.EconomicoDat').val('');
                    }

                });
            }
        }


        function LimpiarModal() {
            cboTipoMaquinaria.val('');
            cboTipoMaquinaria.trigger('change');
            cboGrupoMaquinaria.trigger('change');
            cboModeloMaquinaria.clearCombo();
            cboModeloMaquinaria.prop('disabled', true);
            tbCantidadSolicitada.val('0');
            ModalTbHorasUtilizacion.val('');
            txtMensaje.text('');
        }
        function removePrograma(id) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/DeleteDetSolicitud',
                type: 'POST',
                dataType: 'json',
                data: { id: id, idSolicitud: tbFolioSolicitud.attr('data-idfolio') },
                success: function (response) {
                    if (response.success == true) {
                        ConfirmacionGeneral("Confirmacion", "Se elimino el programa correctamente", "bg-red");
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

        function deleteRow(row, id) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/DeleteDetSolicitud',
                type: 'POST',
                dataType: 'json',
                data: { id: row, idSolicitud: tbFolioSolicitud.attr('data-idfolio') },
                success: function (response) {
                    if (response.success == true) {
                        ListaTable = $.grep(ListaTable,
                            function (o, i) { return o.id == Number(id); },
                            true);

                        tblEquiposSolicitados.bootgrid('clear');
                        tblEquiposSolicitados.bootgrid("append", ListaTable);
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

        function initGridSecundario() {
            tblEquiposSolicitados.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                formatters: {
                    "HorasTotales": function (column, row) {
                        if (row.tipoUtilizacion == "1") {
                            var horas = row.pHoras;
                            if (row.pHoras > 0) {
                                var modelo = row.Modeloid;
                                var inputs = $("#tblEquiposSolicitados").children('tbody').children().find("input[data-ModeloHC='" + modelo + "']");
                                var totals = 0
                                $.each(inputs, function (index, value) {
                                    var valor = $(value).val();

                                    totals += Number(valor);

                                });
                                horas = totals;
                            }

                            var variable = horas + "/" + row.HorasTotales;

                            return "<input type=\"text\" class=\"HorasUsadas form-control\" data-horasTotales='" + row.HorasTotales + "' data-ModeloHT='" + row.Modeloid + "' value='" + variable + "' data-id='" + row.id + "' data-idPrincipal='" + row.idPrincipal + "' disabled >";
                        } else
                            variable = "NA"

                        return "<input type=\"text\" class=\"HorasUsadas form-control\" data-horasTotales='" + row.HorasTotales + "' data-ModeloHT='" + row.Modeloid + "' value='" + variable + "' data-id='" + row.id + "' data-idPrincipal='" + row.idPrincipal + "' disabled >";
                    },
                    "AddHoras": function (column, row) {

                        if (row.tipoUtilizacion == "1") {
                            var horas = row.pHoras;
                            if (row.pHoras > 0) {
                                var modelo = row.Modeloid;
                                var inputs = $("#tblEquiposSolicitados").children('tbody').children().find("input[data-ModeloHC='" + modelo + "']");
                                var totals = 0
                                $.each(inputs, function (index, value) {
                                    var valor = $(value).val();

                                    totals += Number(valor);

                                });
                                horas = totals;
                            }
                            var variable = horas + "/" + row.HorasTotales;
                            var flag = false;
                            if (horas == row.HorasTotales) {
                                flag = true;
                            }


                            return "<input type=\"text\" class=\"HorasEquipo form-control\"  value='" + row.pHoras + "'  data-horasT='" + 0 + "' data-horasTotales='" + row.HorasTotales + "' data-ModeloHC='" + row.Modeloid + "'  data-id='" + row.id + "' data-idPrincipal='" + row.idPrincipal + "' data-valida=" + flag + ">";
                        }
                        else {
                            flag = true;
                            variable = "NA";
                            return "<input type=\"text\" class=\"HorasUsadas form-control\" data-horasTotales='" + row.HorasTotales + "' data-ModeloHT='" + row.Modeloid + "' value='" + variable + "' data-id='" + row.id + "' data-idPrincipal='" + row.idPrincipal + "' disabled >";
                        }

                    },
                    "AddComentario": function (column, row) {
                        return "<input type=\"text\" class=\"ComentarioEquipo form-control\" value='" + row.Comentario + "' data-id='" + row.id + "' data-idPrincipal='" + row.idPrincipal + "' >";
                    },
                    "RemoveEquipo": function (column, row) {

                        return "<button type='button' class='btn btn-danger removeEquipo' data-id='" + row.id + "' data-idDetalle='" + row.idTempSolicitudes + "' data-idPrincipal='" + row.idPrincipal + "'>" +
                            "<span class='glyphicon glyphicon-remove '></span> " +
                            " </button>"
                            ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblEquiposSolicitados.find(".HorasEquipo").addClass('required');

                tblEquiposSolicitados.find(".removeEquipo").on('click', function (e) {
                    var id = $(this).attr('data-id');
                    var idPrincipal = $(this).attr('data-idPrincipal');
                    if ($(this).attr('data-idDetalle') != "undefined") {
                        deleteRow($(this).attr('data-idDetalle'), id);
                        ListaTable = $.grep(ListaTable,
                            function (o, i) { return o.id == id && o.idPrincipal == idPrincipal; },
                            true);

                        var dataTable = getInfo(idPrincipal);
                        tblEquiposSolicitados.bootgrid('clear');
                        tblEquiposSolicitados.bootgrid("append", dataTable);
                    }
                    else {

                        ListaTable = $.grep(ListaTable,
                            function (o, i) { return o.id == id && o.idPrincipal == idPrincipal; },
                            true);

                        var dataTable = getInfo(idPrincipal);

                        tblEquiposSolicitados.bootgrid('clear');
                        tblEquiposSolicitados.bootgrid("append", dataTable);
                    }


                });

                tblEquiposSolicitados.find(".HorasEquipo").on('change', function (e) {

                    var CurrentElemento = $(this);
                    var objeto = CurrentElemento.data();

                    var modelo = CurrentElemento.attr('data-ModeloHC');
                    var inputs = $("#tblEquiposSolicitados").children('tbody').children().find("input[data-ModeloHC='" + modelo + "']");
                    var TotalHoras = CurrentElemento.attr('data-horasT');
                    var HorasActuales = CurrentElemento.val();
                    var totalT = CurrentElemento.attr('data-horasTotales');

                    var totals = 0;

                    $.each(inputs, function (index, value) {
                        var valor = $(value).val();

                        totals += Number(valor);

                    });

                    if (totals == totalT) {
                        $("#tblEquiposSolicitados").children('tbody').children().find("input[data-ModeloHC='" + modelo + "']").attr('data-valida', true);
                    }
                    if (totals >= 0 && totals <= totalT) {

                        $("#tblEquiposSolicitados").children('tbody').children().find("input[data-ModeloHC='" + modelo + "']").attr('data-horasT', totals);
                        $("#tblEquiposSolicitados").children('tbody').children().find("input[data-ModeloHT='" + modelo + "']").val(totals + "/" + totalT);
                    }
                    else {
                        $("#tblEquiposSolicitados").children('tbody').children().find("input[data-ModeloHC='" + modelo + "']").attr('data-valida', false);
                        CurrentElemento.val(0);
                        CurrentElemento.trigger('change');

                    }
                    validarCampo(CurrentElemento);

                });
            });
        }

        function initGridArranqueObra() {

            tblAsignacionEquipos.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                formatters: {
                    "HorasTotales": function (column, row) {
                        if (row.tipoUtilizacion != "3") {
                            var horas = row.pHoras;
                            if (row.pHoras > 0) {
                                var modelo = row.Modeloid;
                                var inputs = $("#tblAsignacionEquipos").children('tbody').children().find("input[data-ModeloHC='" + modelo + "']");
                                var totals = 0
                                $.each(inputs, function (index, value) {
                                    var valor = $(value).val();

                                    totals += Number(valor);

                                });
                                horas = totals;
                            }
                            var variable = horas + "/" + row.HorasTotales;

                            return "<input type=\"text\" class=\"HorasUsadas form-control\" data-horasTotales='" + row.HorasTotales + "' data-ModeloHT='" + row.Modeloid + "' value='" + variable + "' data-id='" + row.id + "' data-idPrincipal='" + row.idPrincipal + "' disabled >";
                        } else
                            variable = "NA"

                        return "<input type=\"text\" class=\"HorasUsadas form-control\" data-horasTotales='" + row.HorasTotales + "' data-ModeloHT='" + row.Modeloid + "' value='" + variable + "' data-id='" + row.id + "' data-idPrincipal='" + row.idPrincipal + "' disabled >";
                    },
                    "AddHoras": function (column, row) {

                        if (row.tipoUtilizacion != "3") {
                            var horas = row.pHoras;
                            if (row.pHoras > 0) {
                                var modelo = row.Modeloid;
                                var inputs = $("#tblAsignacionEquipos").children('tbody').children().find("input[data-ModeloHC='" + modelo + "']");
                                var totals = 0
                                $.each(inputs, function (index, value) {
                                    var valor = $(value).val();

                                    totals += Number(valor);

                                });
                                horas = totals;
                            }
                            var variable = horas + "/" + row.HorasTotales;
                            var flag = false;
                            if (horas == row.HorasTotales) {
                                flag = true;
                            }
                            return "<input type=\"text\" class=\"HorasEquipo form-control\"  value='" + row.pHoras + "'  data-horasT='" + 0 + "' data-horasTotales='" + row.HorasTotales + "' data-ModeloHC='" + row.Modeloid + "'  data-id='" + row.id + "' data-idPrincipal='" + row.idPrincipal + "' data-valida=" + flag + ">";
                        }
                        else {
                            flag = true;
                            variable = "NA";
                            return "<input type=\"text\" class=\"HorasUsadas form-control\" data-horasTotales='" + row.HorasTotales + "' data-ModeloHT='" + row.Modeloid + "' value='" + variable + "' data-id='" + row.id + "' data-idPrincipal='" + row.idPrincipal + "' disabled >";
                        }

                    },
                    "AddComentario": function (column, row) {
                        return "<input type=\"text\" class=\"ComentarioEquipo form-control\" value='" + row.Comentario + "' data-id='" + row.id + "' data-idPrincipal='" + row.idPrincipal + "' >";
                    },
                    "RemoveEquipo": function (column, row) {

                        return "<button type='button' class='btn btn-danger removeEquipo' data-id='" + row.id + "' data-idDetalle='" + row.idTempSolicitudes + "' data-idPrincipal='" + row.idPrincipal + "'>" +
                            "<span class='glyphicon glyphicon-remove '></span> " +
                            " </button>"
                            ;
                    },
                    "Economicos": function (column, row) {
                        var cadena = "";
                        $.each(ListaEconomicosFillCbo, function (index, value) {
                            if (value.idPrincipal == row.idPrincipal && (row.Grupoid != undefined ? row.Grupoid : row.GrupoId) == value.idGrupo) {

                                cadena = value.stringCombo;
                            }
                        });

                        return cadena;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                var rows = tblAsignacionEquipos.bootgrid('getCurrentRows');
                fillComboTable(rows);
                tblAsignacionEquipos.find(".HorasEquipo").addClass('required');

                tblAsignacionEquipos.find(".removeEquipo").on('click', function (e) {
                    var id = $(this).attr('data-id');
                    var idPrincipal = $(this).attr('data-idPrincipal');
                    if ($(this).attr('data-idDetalle') != "undefined") {
                        deleteRow($(this).attr('data-idDetalle'), id);
                        ListaTable = $.grep(ListaTable,
                            function (o, i) { return o.id == id && o.idPrincipal == idPrincipal; },
                            true);

                        var dataTable = getInfo(idPrincipal);
                        tblAsignacionEquipos.bootgrid('clear');
                        tblAsignacionEquipos.bootgrid("append", dataTable);
                    }
                    else {

                        ListaTable = $.grep(ListaTable,
                            function (o, i) { return o.id == id && o.idPrincipal == idPrincipal; },
                            true);

                        var dataTable = getInfo(idPrincipal);

                        tblAsignacionEquipos.bootgrid('clear');
                        tblAsignacionEquipos.bootgrid("append", dataTable);
                    }


                });

                tblAsignacionEquipos.find(".HorasEquipo").on('change', function (e) {

                    var CurrentElemento = $(this);
                    var objeto = CurrentElemento.data();

                    var modelo = CurrentElemento.attr('data-ModeloHC');
                    var inputs = $("#tblAsignacionEquipos").children('tbody').children().find("input[data-ModeloHC='" + modelo + "']");
                    var TotalHoras = CurrentElemento.attr('data-horasT');
                    var HorasActuales = CurrentElemento.val();
                    var totalT = CurrentElemento.attr('data-horasTotales');

                    var totals = 0;

                    $.each(inputs, function (index, value) {
                        var valor = $(value).val();

                        totals += Number(valor);

                    });

                    if (totals == totalT) {
                        $("#tblAsignacionEquipos").children('tbody').children().find("input[data-ModeloHC='" + modelo + "']").attr('data-valida', true);
                    }
                    if (totals >= 0 && totals <= totalT) {

                        $("#tblAsignacionEquipos").children('tbody').children().find("input[data-ModeloHC='" + modelo + "']").attr('data-horasT', totals);
                        $("#tblAsignacionEquipos").children('tbody').children().find("input[data-ModeloHT='" + modelo + "']").val(totals + "/" + totalT);
                    }
                    else {
                        $("#tblAsignacionEquipos").children('tbody').children().find("input[data-ModeloHC='" + modelo + "']").attr('data-valida', false);
                        CurrentElemento.val(0);
                        CurrentElemento.trigger('change');

                    }
                    validarCampo(CurrentElemento);

                });

                tblAsignacionEquipos.find(".EconomicoDat").on('change', function (e) {

                    var CurrentElemento = $(this);
                    var currentValue = $(CurrentElemento).val();
                    var listaEconomicos = $(".EconomicoDat")
                    var bandera = false;
                    var count = 0;
                    $.each(listaEconomicos, function (index, value) {
                        var indexValue = $(value).val();

                        if (indexValue != "0") {
                            if (indexValue != "9999") {
                                if (indexValue == currentValue) {
                                    count++;
                                }
                            }

                        }

                    });
                    var CurrentText = CurrentElemento.find("option:selected").text();
                    $("#list option:selected").text();
                    if (count >= 2) {
                        AlertaGeneral("Alerta", "Ya selecciono el economico " + CurrentText + " , favor de seleccionar uno diferente");
                        $(this).val('');
                    }
                });
            });

        }

        function CboEconomicos(idGrupoEconomico, key) {
            $.ajax({
                url: '/SolicitudEquipo/cboEconomicosByGrupo',
                type: 'POST',
                dataType: 'json',
                data: { idGrupo: idGrupoEconomico },
                success: function (response) {
                    var objEconomico = {};
                    objEconomico.idPrincipal = key;
                    objEconomico.idGrupo = idGrupoEconomico;
                    objEconomico.stringCombo = response.stringCboEconomico;
                    ListaEconomicosFillCbo.push(objEconomico);
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        function deleteRow(row, id) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/DeleteDetSolicitud',
                type: 'POST',
                dataType: 'json',
                data: { id: row, idSolicitud: tbFolioSolicitud.attr('data-idfolio') },
                success: function (response) {
                    if (response.success == true) {


                        ListaTable = $.grep(ListaTable,
                            function (o, i) { return o.id == Number(id); },
                            true);

                        tblEquiposSolicitados.bootgrid('clear');
                        tblEquiposSolicitados.bootgrid("append", ListaTable);
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

        function fillTablePrograma() {
            var id = Countid == 0 ? 1 : Countid + 1;
            Countid = id;
            var tamaño = ListaPrograma.length
            //   for (var i = 0; i <= tamaño; i++) {
            var Objeto = {};
            var tempId = id;
            Objeto.id = tempId;
            Objeto.FechaInicio = tbFechaIni.val();
            Objeto.FechaFin = tbFechaFin.val();
            Objeto.TipoPrioridad = GetPrioridad($('input[name=radioInline1]:checked').val());//  == 'A' ? 'URGENTE' : 'PROGRAMADA';
            Objeto.HasPrograma = "0";
            ListaPrograma.push(Objeto);
            tblSolicitudesMaquinaria.bootgrid("append", [Objeto]);
            //     }

        }

        function GetPrioridad(value) {

            switch (value) {
                case 'A':
                    return 'URGENTE';
                case 'B':
                    return 'PROGRAMADA';
                case 'C':
                    return 'NORMAL';

                default:

            }
        }

        function updateTable(id) {
            for (var i = 0; i < ListaRows.length; i++) {
                if (ListaRows[i].id == id) {
                    ListaRows[i].FechaInicio = tbFechaIni.val();
                    ListaRows[i].FechaFin = tbFechaFin.val();
                    ListaRows[i].HasPrograma = "1";
                    ListaRows[i].TipoPrioridad = GetPrioridad($('input[name=radioInline1]:checked').val()); //$('input[name=radioInline1]:checked').val() == 'A' ? 'URGENTE' : 'PROGRAMADA';
                }
            }
        }

        function updateTableIgualar(id, elementoData) {
            for (var i = 0; i < ListaRows.length; i++) {
                if (ListaRows[i].id == id) {
                    ListaRows[i].FechaInicio = elementoData.attr("data-pFechaInicio");
                    ListaRows[i].FechaFin = elementoData.attr("data-pFechaFin");
                    ListaRows[i].FechaObra = elementoData.attr("data-pFechaObra");
                    ListaRows[i].HasPrograma = "1";
                    ListaRows[i].TipoPrioridad = elementoData.attr("data-pTipoPrioridad");
                }
            }
        }

        function setAttr(elemento, elementoData) {

            updateTableIgualar(elemento.attr("data-id"), elementoData);
            elemento.attr("data-pFechaInicio", elementoData.attr("data-pFechaInicio"));
            elemento.attr("data-pFechaFin", elementoData.attr("data-pFechaFin"));
            elemento.attr("data-pHoras", elementoData.attr("data-pHoras"));
            elemento.attr("data-pFechaObra", elementoData.attr("data-pFechaObra"));
            elemento.attr("data-pTipoPrioridad", elementoData.attr("data-pTipoPrioridad"));
            elemento.attr("data-hasprograma", "1");

        }

        function getDataToSend() {
            var valor = validacion();
            if (valor) {
                var array = [];
                $.each(ListaTable, function (index, value) {
                    var json = {};
                    var idSave = 0;
                    if (value.idTempSolicitudes != null) {
                        idSave = value.idTempSolicitudes;
                    }

                    var idNoEconomico = Number(value.idNoEconomico);
                    json.Folio = (tbFolioSolicitud.val()) == cboFolio.val() ? tbFolioSolicitud.val() : tbCC.val() + "-" + tbFolioSolicitud.val();
                    json.id = idSave;
                    json.TipoId = idSave != 0 ? Number(value.TipoId) : Number(value.Tipoid);
                    json.GrupoId = idSave != 0 ? Number(value.GrupoId) : Number(value.Grupoid);
                    json.Modeloid = value.Modeloid;
                    json.Tipo = value.Tipo;
                    json.Grupo = value.Grupo;
                    json.Modelo = value.Modelo;
                    json.Descripcion = value.Comentario;
                    json.pFechaInicio = value.pFechaInicio;
                    json.pFechaFin = value.pFechaFin;
                    json.pHoras = value.pHoras == "NA" ? Number(0) : Number(value.pHoras);
                    json.tipoUtilizacion = value.tipoUtilizacion;
                    json.pFechaObra = value.pFechaObra;
                    json.pTipoPrioridad = value.pTipoPrioridad;
                    json.pCapacidad = value.pcapacidad;
                    json.Economico = value.Economico;
                    json.idNoEconomico = idNoEconomico == 9999 ? 0 : idNoEconomico;
                    json.meses = 0;
                    json.condicionInicial = txtCondicionInicial.val();
                    json.condicionActual = txtCondicionActual.val();
                    json.link = "";
                    if (cboConcepto.val() != 5) {
                        json.justificacion = $("#cboConcepto option:selected").text();
                    }
                    else {
                        json.justificacion = txtOtraJustificacion.val();
                    }
                    // Json.cantidad = 1;
                    if (!$("#ckArranque").is(':checked')) {
                        json.arranqueObra = false;
                    }
                    else {
                        json.arranqueObra = true;
                    }
                    array.push(json);
                });


                if (array.length > 0) {

                    if (!$("#ckArranque").is(':checked')) {

                        sendData(array, 1, getJustificaciones());
                    }
                    else {
                        sendDataArranqueObra(array);
                    }

                }
            }
            else {
                $('a[href^="#Step2"]').trigger('click');
                $('a[href^="#step-3"]').attr("disabled", "disabled");
            }
        }
        function getJustificaciones() {
            var data = [];
            var just = $(".txtJustificacion");
            $.each(just, function (i, e) {
                var el = $(e);
                var obj = {};
                obj.id = 0;
                obj.solicitudID = 0;
                obj.grupoID = el.data("grupoid");
                obj.grupo = el.data("grupo");
                obj.modeloID = el.data("modeloid");
                obj.modelo = el.data("modelo");
                obj.justificacion = el.val();
                data.push(obj);
            });
            return data;
        }

        function validacion() {
            var valid = true;
            if (tbDescripcionCC.val() == "") {
                AlertaGeneral("Alerta", "No se asigno un centro de costos.");
                return false;
            }
            if (tbGerenteObra.val() == null) {
                AlertaGeneral("Alerta", "Debe seleccionar un gerente de obra para continuar");
                return false;
            }
            if (tbDirectorDivision.val() == null) {
                AlertaGeneral("Alerta", "Debe seleccionar un director de división para continuar");
                return false;
            }
            if (tbDirectorServicios.val() == null) {
                AlertaGeneral("Alerta", "Debe seleccionar un director de servicios para continuar");
                return false;
            }
            if (tbAltaDireccion.val() == null) {
                AlertaGeneral("Alerta", "Debe seleccionar autorizador de alta direccion para continuar");
                return false;
            }
            return valid;

        }

        function GuardarSolicitud() {


            InfObjeto.condicionInicial = txtCondicionInicial.val();
            InfObjeto.condicionActual = txtCondicionActual.val();
            InfObjeto.link = "";
            if (cboConcepto.val() != 5) {
                InfObjeto.justificacion = $("#cboConcepto option:selected").text();
            }
            else {
                InfObjeto.justificacion = txtOtraJustificacion.val();
            }
            obj = InfObjeto;


            var array = [];


            $.each(ListaTable, function (index, value) {
                var json = {};
                var idSave = 0;
                if (value.idTempSolicitudes != null) {
                    idSave = value.idTempSolicitudes;
                }

                var idNoEconomico = Number(value.idNoEconomico);
                json.Folio = (tbFolioSolicitud.val()) == cboFolio.val() ? tbFolioSolicitud.val() : tbCC.val() + "-" + tbFolioSolicitud.val();
                json.id = idSave;
                json.TipoId = idSave != 0 ? Number(value.TipoId) : Number(value.Tipoid);
                json.GrupoId = idSave != 0 ? Number(value.GrupoId) : Number(value.Grupoid);
                json.Modeloid = value.Modeloid;
                json.Tipo = value.Tipo;
                json.Grupo = value.Grupo;
                json.Modelo = value.Modelo;
                json.Descripcion = value.Comentario;
                json.pFechaInicio = value.pFechaInicio;
                json.pFechaFin = value.pFechaFin;
                json.pHoras = value.pHoras == "NA" ? Number(0) : Number(value.pHoras);
                json.tipoUtilizacion = value.tipoUtilizacion;
                json.pFechaObra = value.pFechaObra;
                json.pTipoPrioridad = value.pTipoPrioridad;
                json.pCapacidad = value.pcapacidad;
                json.Economico = value.Economico;
                json.idNoEconomico = idNoEconomico == 9999 ? 0 : idNoEconomico;
                json.meses = 0;
                json.condicionInicial = txtCondicionInicial.val();
                json.condicionActual = txtCondicionActual.val();
                json.link = "";
                if (cboConcepto.val() != 5) {
                    json.justificacion = $("#cboConcepto option:selected").text();
                }
                else {
                    json.justificacion = txtOtraJustificacion.val();
                }
                // Json.cantidad = 1;
                if (!$("#ckArranque").is(':checked')) {
                    json.arranqueObra = false;
                }
                else {
                    json.arranqueObra = true;
                }
                array.push(json);
            });
            if (!$("#ckArranque").is(':checked')) {
                if (validarJustificacion()) {
                    if ($("#fuEvidencia").val() != '') {
                        saveOrUpdate(array, obj, GetAutorizadores(), 1, getJustificaciones());
                    }
                    else {
                        AlertaGeneral("¡Alerta", "Favor de cargar un archivo de evidencia!");
                    }
                }
                else {
                    AlertaGeneral("Alerta", "Toda la información de justificación es obligatoria");
                }
            }
            else {
                saveOrUpdateArranqueObra(array, obj, GetAutorizadores(), 1);
            }

        }

        function validarJustificacion() {
            var result = true;
            if (txtCondicionInicial.val() == "") {
                result = false;
            }
            else if (txtCondicionActual.val() == "") {
                result = false;
            }
            var just = $(".txtJustificacion");
            $.each(just, function (i, e) {
                var el = $(e);
                if (el.val() == "") {
                    result = false;
                }
            });
            return result;
        }
        function saveOrUpdate(array, obj, autoriza, accion, arrayJustificacion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/SaveOrUpdate',
                type: 'POST',
                dataType: 'json',
                data: { array: array, obj: obj, autoriza: autoriza, actualizacion: accion, arrayJustificacion: arrayJustificacion },
                success: function (response) {
                    if (response.success == true) {
                        $.unblockUI();
                        var file = document.getElementById("fuEvidencia").files[0];
                        if (file != undefined) {
                            subirEvidencia(response.solicitudID, file, response.folio)
                        }
                        else {
                            if (!tbFolioSolicitud.is(':enabled')) {
                                regresarInicio();
                                ConfirmacionGeneral("Confirmación", "Se realizó el folio " + response.folio);
                            }
                            else {
                                regresarInicio();
                                ConfirmacionGeneral("Confirmación", "El folio fue modificado Correctamente");
                            }
                        }
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

        function saveOrUpdateArranqueObra(array, obj, autoriza, accion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/SaveOrUpdateArranqueObra',
                type: 'POST',
                dataType: 'json',
                data: { array: array, obj: obj, autoriza: autoriza, actualizacion: accion, CentroCostos: 3 },
                success: function (response) {
                    if (response.success == true) {
                        $.unblockUI();
                        regresarInicio();
                        ConfirmacionGeneral("Confirmación", "Se realizó el folio " + response.folio);
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
        function subirEvidencia(solicitudID, file, folio) {
            $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });
            var formData = new FormData();

            formData.append("fuEvidencia", file);
            formData.append("solicitudID", solicitudID);

            $.ajax({
                type: "POST",
                url: "/SolicitudEquipo/SubirEvidenciaSolicitud",
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    $.unblockUI();
                    if (!tbFolioSolicitud.is(':enabled')) {
                        fuEvidencia.val("");
                        regresarInicio();
                        ConfirmacionGeneral("Confirmación", "Se realizó el folio " + folio);
                    }
                    else {
                        fuEvidencia.val("");
                        regresarInicio();
                        ConfirmacionGeneral("Confirmación", "El folio fue modificado Correctamente");
                    }
                },
                error: function (error) {
                    $.unblockUI();
                }
            });
        }
        function regresarInicio() {
            $('a[href^="#Step1"]').trigger('click');
            $('a[href^="#Step2"]').attr("disabled", "disabled");
            $('a[href^="#step-3"]').attr("disabled", "disabled");

            ListaRows = [];
            ListaTable = [];
            Countid = 0;
            ListaPrograma = [];
            getFechas();
            tblEquiposSolicitados.bootgrid('clear');
            tblSolicitudesMaquinaria.bootgrid('clear');
            tbFolioSolicitud.val('');
            tbCC.val('');
            tbDescripcionCC.val('');
            tbGerenteObra.val('');
            tbDirectorDivision.val('');
            tbDirectorServicios.val('');
            tbAltaDireccion.val('');
            tbGerenteDirector.val('');
            tbElabora.removeAttr('data-id');
            LimpiarAutorizadores();

            tbFolioSolicitud.removeClass('hide');
            tbFolioSolicitud.attr('data-idfolio', 0);
            tbFolioSolicitud.prop("disabled", true);
            cboFolio.addClass('hide');
            cboFolio.clearCombo();
        }

        function sendData(array, accion, arrayJustificacion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/getDataFromTableSolicitudElaboracion',
                type: 'POST',
                dataType: 'json',
                data: { array: array, obj: GetAutorizadores(), arrayJustificacion: arrayJustificacion },
                success: function (response) {
                    if (response.success == true) {

                        var idReporte = "30";
                        var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pCC=" + tbCC.val() + "&inMemory=1";

                        ireport.attr("src", path);

                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                        };
                    }
                    else {
                    }
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                    $.unblockUI();
                }
            });
        }

        function sendDataArranqueObra(array) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/getDataForReporteAsignacion',
                type: 'POST',
                dataType: 'json',
                data: { array: array, obj: GetAutorizadores() },
                success: function (response) {
                    if (response.success == true) {

                        var idReporte = "12";
                        var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pCC=" + tbCC.val();

                        ireport.attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();

                        };
                    }
                    else {
                    }

                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                    $.unblockUI();
                }
            });
        }

        function getValue(pElementoInicial, pElementoItera) {

            var atributosElementoInicial = pElementoInicial;

            var ElementoItera = pElementoItera.parent().parent().prev().children('td').children('button.agregarPrograma');

            if (ElementoItera.attr("data-hasPrograma") == "0") {
                getValue(pElementoInicial, ElementoItera);
            }
            else {

                if (atributosElementoInicial.attr("data-hasPrograma") != 0) {

                    if (atributosElementoInicial.attr("data-pFechaInicio") != ElementoItera.attr("data-pFechaInicio") ||
                        atributosElementoInicial.attr("data-pFechaFin") != ElementoItera.attr("data-pFechaFin") ||
                        atributosElementoInicial.attr("data-pFechaObra") != ElementoItera.attr("data-pFechaObra") ||
                        atributosElementoInicial.attr("data-pTipoPrioridad") != ElementoItera.attr("data-pTipoPrioridad") ||
                        atributosElementoInicial.attr("data-pHoras") != ElementoItera.attr("data-pHoras")) {
                        setAttr(atributosElementoInicial, ElementoItera);
                    }
                    else if (ElementoItera.attr("data-hasPrograma") != "0" && ElementoItera.attr("data-id") != 1) {
                        getValue(pElementoInicial, ElementoItera);
                    }

                }
                else {
                    setAttr(atributosElementoInicial, ElementoItera);
                }

            }


        }

        function setSolicitud() {

            if (ValidaAutorizadores()) {
                InfObjeto.id = tbFolioSolicitud.attr('data-idFolio');
                InfObjeto.folio = tbFolioSolicitud.val();
                InfObjeto.CC = tbCC.val();
                InfObjeto.fechaElaboracion = tbSingleDate.val();
                InfObjeto.usuarioID = Number(tbElabora.attr('data-id'));
                InfObjeto.descripcion = txtAComentarios.val();
                InfObjeto.ArranqueObra = $("#ckArranque").is(':checked');
            }
            else {
                $('a[href^="#Step1"]').trigger('click');
                AlertaGeneral("Alerta", "Hay Campos Obligatorios");
            }
        }

        function ValidaAutorizadores() {
            var state = true;
            tbCC.addClass('required')
            tbGerenteObra.addClass('required');
            tbGerenteDirector.addClass('required');
            tbDirectorDivision.addClass('required');
            tbDirectorServicios.addClass('required');
            tbAltaDireccion.addClass('required');


            if (!validarCampo(tbCC)) { state = false; }
            if (!validarCampo(tbGerenteObra)) { state = false; }
            if (!validarCampo(tbGerenteDirector)) { state = false; }
            if (!validarCampo(tbDirectorDivision)) { state = false; }
            if (!validarCampo(tbAltaDireccion)) { state = false; }
            return state;
        }

        function fillGerente(event, ui) {

            tbGerenteObra.text(ui.item.value);
            tbGerenteObra.attr("data-id", ui.item.id);
        }

        function fillDirector(event, ui) {
            tbDirectorDivision.text(ui.item.value);
            tbDirectorDivision.attr("data-id", ui.item.id);
        }
        function fillServicios(event, ui) {
            tbDirectorServicios.text(ui.item.value);
            tbDirectorServicios.attr("data-id", ui.item.id);
        }
        function fillAltaDireccion(event, ui) {
            tbAltaDireccion.text(ui.item.value);
            tbAltaDireccion.attr("data-id", ui.item.id);
        }

        function GetAutorizadores() {

            var autorizadores = {};

            autorizadores.usuarioElaboro = Number(tbElabora.attr('data-id'));
            autorizadores.GerenteDirector = Number(tbGerenteDirector.val());
            autorizadores.gerenteObra = Number(tbGerenteObra.val());
            autorizadores.directorDivision = Number(tbDirectorDivision.val());
            autorizadores.directorServicios = Number(tbDirectorServicios.val());

            autorizadores.altaDireccion = Number(tbAltaDireccion.val());

            return autorizadores;
        }

        function datePicker(year, month, day) {

            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
                from = tbFechaIni
                    .datepicker({
                        changeMonth: true,
                        changeYear: true,
                        numberOfMonths: 1,
                        defaultDate: new Date(year, 00, 01),
                        maxDate: new Date(year, 11, 31),
                        minDate: new Date(year, month, day),
                        onChangeMonthYear: function (y, m, i) {
                            var d = i.selectedDay;
                            $(this).datepicker('setDate', new Date(y, m - 1, d));
                            //    $(this).trigger('change');

                        },
                        onSelect: function () {
                            var event;
                            if (typeof window.Event == "function") {
                                event = new Event('change');
                                this.dispatchEvent(event);
                            } else {
                                event = document.createEvent('HTMLEvents');
                                event.initEvent('change', false, false);
                                this.dispatchEvent(event);
                            }
                        },
                        onClose: function (evt, ui) {
                            $(this).datepicker("refresh");
                        }

                    })
                    .on("change", function () {

                    }),
                to = tbFechaFin.datepicker({
                    changeMonth: true,
                    changeYear: true,
                    numberOfMonths: 1,
                    defaultDate: new Date(),
                    minDate: new Date(year, month, day),
                    onChangeMonthYear: function (y, m, i) {
                        var d = i.selectedDay;
                        $(this).datepicker('setDate', new Date(y, m - 1, d));

                    }
                })
                    .on("change", function () {
                        // from.datepicker("option", "maxDate", getDate(this));

                    });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
        }

        iniciarGrid();
        init();

        function SetToggle() {
            $('.button-checkbox').each(function () {
                // Settings
                var $widget = $(this),
                    $button = $widget.find('button'),
                    $checkbox = $widget.find('input:checkbox'),
                    color = $button.data('color'),
                    settings = {
                        on: {
                            icon: 'glyphicon glyphicon-check'
                        },
                        off: {
                            icon: 'glyphicon glyphicon-unchecked'
                        }
                    };

                // Event Handlers
                $button.on('click', function () {
                    $checkbox.prop('checked', !$checkbox.is(':checked'));
                    $checkbox.triggerHandler('change');
                    updateDisplay();
                });
                $checkbox.on('change', function () {
                    updateDisplay();
                });

                // Actions
                function updateDisplay() {
                    var isChecked = $checkbox.is(':checked');
                    // Set the button's state
                    $button.data('state', (isChecked) ? "on" : "off");

                    // Set the button's icon
                    $button.find('.state-icon')
                        .removeClass()
                        .addClass('state-icon ' + settings[$button.data('state')].icon);

                    // Update the button's color
                    if (isChecked) {
                        SetArranqueObra();
                        $("#lblArranqueObra").text('SI');
                        $button
                            .removeClass('btn-default')
                            .addClass('btn-' + color + ' active');
                        $(".divJustificacion").hide();
                    }
                    else {
                        LimpiarAutorizadores();
                        GetNombreCC();
                        $("#lblArranqueObra").text('NO');
                        $button
                            .removeClass('btn-' + color + ' active')
                            .addClass('btn-default');
                        $(".divJustificacion").show();
                    }
                }

                // Initialization
                function init1() {

                    updateDisplay();

                    // Inject the icon if applicable
                    if ($button.find('.state-icon').length == 0) {
                        $button.prepend('<i class="state-icon ' + settings[$button.data('state')].icon + '"></i> ');
                    }
                }
                init1();
            });
        }


        function LimpiarAutorizadores() {
            tbGerenteObra.clearCombo();
            tbDirectorDivision.clearCombo();
            tbDirectorServicios.clearCombo();
            tbAltaDireccion.clearCombo();
            tbGerenteDirector.clearCombo();
        }

        function SetArranqueObra() {
            tbGerenteObra.fillCombo('/SolicitudEquipo/FillCboAutorizadoresArranque', { autorizador: 1 });
            tbDirectorDivision.fillCombo('/SolicitudEquipo/FillCboAutorizadoresArranque', { autorizador: 2 });
            tbDirectorServicios.fillCombo('/SolicitudEquipo/FillCboAutorizadoresArranque', { autorizador: 11 });
            tbAltaDireccion.fillCombo('/SolicitudEquipo/FillCboAutorizadoresArranque', { autorizador: 3 });
            tbGerenteDirector.fillCombo('/SolicitudEquipo/FillCboAutorizadoresArranque', { autorizador: 4 });
        }
    };

    $(document).ready(function () {
        maquinaria.inventario.Solicitud.ElaboracionDeSolicitud = new ElaboracionDeSolicitud();
    });
})();


