(function () {

    $.namespace('maquinaria.catalogo.maquina');

    maquina = function () {
        idMaquina = 0,
            Actualizacion = 1;
        noEconomico = "",
            fecha_hoy = getFecha(),
            ruta = '/CatMaquina/FillGrid_Maquina';

        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Maquina',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        var dialog, dialog1,
            cboCentroCostos = $("#cboCentroCostos"),
            frmModal = $("#frmMaquina"),
            txtFiltroNoEconomico = $("#txtFiltroNoEconomico"),
            cboModalEquipoRenta = $("#cboModalEquipoRenta"),
            cboModalTipoEncierro = $("#cboModalTipoEncierro"),
            cboModalTipoMaquinaria = $("#cboModalTipoMaquinaria"),
            cboModalGrupoMaquinaria = $("#cboModalGrupoMaquinaria"),
            txtModalNoEconomico = $("#txtModalNoEconomico"),
            cboModalModelo = $("#cboModalModelo"),
            txtModaldescripcion = $("#txtModalDescripcion"),
            cboModalMarca = $("#cboModalMarca"),
            cboModalAnios = $("#cboModalAnios"),

            /***/
            btnSetReporte = $("#btnSetReporte"),
            cboModalEstatus = $("#cboModalEstatus"),
            txtModalNoSerie = $("#txtModalNoSerie"),
            cboModalAseguradoras = $("#cboModalAseguradoras"),
            txtModalPoliza = $("#txtModalPoliza"),
            dateModalVigenciaPoliza = $("#dateModalVigenciaPoliza"),
            cboModalTipoCombustible = $("#cboModalTipoCombustible"),
            txtModalCapTanque = $("#txtModalCapTanque"),
            txtModalCapacidadCarga = $("#txtModalCapacidadCarga"),
            cboModalUnidadCarga = $("#cboModalUnidadCarga"),
            txtModalProveedor = $("#txtModalProveedor"),
            dateModalFechaAdquiere = $("#dateModalFechaAdquiere"),
            txtModalHorasAdquisicion = $("#txtModalHorasAdquisicion"),
            txtModalHorometroActual = $("#txtModalHorometroActual"),
            txtModalPlaca = $("#txtModalPlaca"),
            txtModalVTipoMaquina = $("#txtModalVTipoMaquina"),
            txtModalVGrupo = $("#txtModalVGrupo"),
            txtModalVNoEConomico = $("#txtModalVNoEConomico"),
            txtModalVDescripcion = $("#txtModalVDescripcion"),
            txtModalVModelo = $("#txtModalVModelo"),
            txtModalVNoSerie = $("#txtModalVNoSerie"),
            txtModalVMarca = $("txtModalVMarca"),
            txtModalVHorometroActual = $("txtModalVHorometroActual"),
            txtModalVHorometroAdquisicion = $("txtModalVHorometroAdquisicion"),
            cboFiltroTipo = $("#cboFiltroTipo"),
            cboFiltroGrupo = $("#cboFiltroGrupo"),
            cboFiltroEstatus = $("#cboFiltroEstatus"),
            txtFiltroDescripcion = $("#txtFiltroDescripcion"),

            btnBuscar = $("#btnBuscar_Maquina"),
            btnNuevo = $("#btnNuevo_Maquina"),
            btnGuardar = $("#btnModalGuardar_Maquina"),
            btnCancelar = $("#btnModalCancelar_Maquina"),
            btnNext = $(".nextBtn"),
            btnRow = $('a[href^="#step-4"]'),

            modalAcciones = $("#modalMaquina"),
            tituloModal = $("#title-modal"),
            gridResultado = $("#grid_Maquina"),

            modalDesactivar = $("#modalDesactivar"),
            lblBaja = $("#lblBaja"),
            cboTipoBaja = $("#cboTipoBaja"),
            txtKMBaja = $("#txtKMBaja"),
            modalActivar = $("#modalActivar"),
            lblAlta = $("#lblAlta");
        txtHoroBaja = $("#txtHoroBaja");

        $(document).on('click', "#btnModalEliminar", function () {
            if (validBaja()) {
                // beforeSaveOrUpdate();
                SaveOrUpdateDarBaja(getPlainObject());
                reset();
                modalDesactivar.modal('hide');
            }
        });

        $(document).on('click', "#btnModalActivar", function () {
            beforeSaveOrUpdate();
            reset();
            modalActivar.modal('hide');
        });

        function ModalView() {
            dialog = $("#dialog-form").dialog({
                draggable: false,
                modal: true,
                resizable: true,
                height: "auto",
                width: "auto",
                autoOpen: false
            });

            dialog1 = $("#dialog-form1").dialog({
                draggable: false,
                modal: true,
                resizable: true,
                height: "auto",
                width: "auto",
                autoOpen: false
            });
        }

        function getInfoEliminar(id) {
            $.ajax({
                url: '/CatMaquina/GetHormetroFinalByMaquina',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ noEconomidoID: id }),
                success: function (response) {

                    if (!Number.isNaN(Number(response.horometro))) {
                        txtHoroBaja.attr('data-horometro', response.horometro);
                    }

                    lblBaja.text(response.Economico);
                    txtHoroBaja.setVal(response.horometro);
                    $(".txtHorometroBaja").show();
                    modalDesactivar.modal('show');
                },
                error: function (response) {
                }
            });
        }

        function init() {
            cboCentroCostos.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false);
            var id = $.urlParam('idMaquinaria');
            if (id != undefined) {
                idMaquina = id;
                getInfoEliminar(id);
            }
            else {
                if (id != -1) {

                }
            }

            txtHoroBaja.change(getNumeroVal);

            ModalView();
            txtKMBaja.DecimalFixNS(0);
            datosRequeridos();
            dateModalVigenciaPoliza.datepicker().datepicker("setDate", new Date());
            dateModalFechaAdquiere.datepicker().datepicker("setDate", new Date());

            cboModalTipoMaquinaria.fillCombo('/CatMaquina/FillCboTipo_Maquina', { estatus: true });
            cboFiltroTipo.fillCombo('/CatMaquina/FillCboTipo_Maquina', { estatus: true });
            cboFiltroGrupo.fillCombo('/CatMaquina/FillCboFiltro_Maquina', { estatus: true });
            cboModalTipoMaquinaria.change(FillCboGrupo);
            cboModalTipoMaquinaria.change(FillCboMarca);
            cboModalTipoMaquinaria.change(FillCboModelo);

            cboModalGrupoMaquinaria.attr('disabled', true);
            cboModalGrupoMaquinaria.change(FillCboMarca);
            cboModalGrupoMaquinaria.change(FillCboModelo);

            cboModalMarca.change(FillCboModelo);

            cboModalMarca.attr('disabled', true);
            cboModalMarca.change(FillCboModelo);
            cboModalModelo.attr('disabled', true);

            cboModalAseguradoras.fillCombo('/CatMaquina/FillCboAseguradora_Maquina', { estatus: true });
            cboModalTipoEncierro.fillCombo('/CatMaquina/FillCbo_TipoEncierro');
            cboModalAnios.fillCombo('/CatMaquina/FillCbo_Anios');
            cboModalUnidadCarga.fillCombo('/CatMaquina/FillCbo_UnidadCarga');
            cboModalTipoCombustible.fillCombo('/CatMaquina/FillCbo_TipoCombustible');
            cboModalEquipoRenta.change(fillTXTNoEconomico);
            txtFiltroDescripcion.attr('maxlength', 100);

            cboTipoBaja.fillCombo('/CatMaquina/FillCboTipoBaja', null, false, null);

            btnNuevo.click(openModal);
            btnGuardar.click(guardar);
            btnCancelar.click(reset);
            btnBuscar.click(clickBuscar);
            btnNext.click(clickLlenar);
            btnRow.click(clickLlenar);

            btnSetReporte.click(setFichaTecnica);
            initGrid();
        }

        function getNumeroVal() {

            if (!Number.isNaN(Number(txtHoroBaja.attr('data-horometro')))) {
                var tempNumber = Number(txtHoroBaja.val());
                var Anterior = Number(txtHoroBaja.attr('data-horometro'));

                if (Anterior + 24 > tempNumber) {

                    txtHoroBaja.val(Anterior);
                }

                if (Anterior - 24 < tempNumber) {

                    txtHoroBaja.val(Anterior);
                }
            }
        }

        function datosRequeridos() {

            cboModalTipoMaquinaria.addClass('required');
            cboModalGrupoMaquinaria.addClass('required');
            cboModalMarca.addClass('required');
            cboModalModelo.addClass('required');
            cboModalAnios.addClass('required');
            txtModalNoSerie.addClass('required');
            cboModalAseguradoras.addClass('required');
            txtModalPoliza.addClass('required');
            //  cboModalTipoCombustible.addClass('required');
            // txtModalCapTanque.addClass('required');
            // cboModalUnidadCarga.valid();

            dateModalFechaAdquiere.addClass('required');
            dateModalVigenciaPoliza.addClass('required');
            txtModaldescripcion.addClass('required');
            //txtModalHorasAdquisicion.addClass('required');
            //txtModalHorometroActual.addClass('required');
        }
        function fillTXTNoEconomico() {

            if (cboModalEquipoRenta.val() == 1) {
                var res = cboModalGrupoMaquinaria.find(':selected').attr('data-prefijo').split("-");
                txtModalNoEconomico.val(res[0] + "-R" + res[1]);
            }
            else {

                txtModalNoEconomico.val(cboModalGrupoMaquinaria.find(':selected').attr('data-prefijo'));
            }

        }

        function clickLlenar() {

            valid();
            txtModalVNoEConomico.val(txtModalNoEconomico.val());
            txtModalVTipoMaquina.val(cboModalTipoMaquinaria.find("option:selected").text());
            txtModalVGrupo.val(cboModalGrupoMaquinaria.find("option:selected").text());
            txtModalVDescripcion.val(txtModaldescripcion.val());
            txtModalVNoSerie.val(txtModalNoSerie.val());
            txtModalVMarca.val(cboModalMarca.find("option:selected").text());
            txtModalVModelo.val(cboModalModelo.find("option:selected").text());
            txtModalVHorometroActual.val(txtModalHorometroActual.val());
            txtModalVHorometroAdquisicion.val(txtModalHorasAdquisicion.val());
        }

        function FillCboGrupo() {
            if (cboModalTipoMaquinaria.val() != "") {
                cboModalGrupoMaquinaria.fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: cboModalTipoMaquinaria.val() });

                cboModalGrupoMaquinaria.attr('disabled', false);
                noEconomico = "";
                txtModalNoEconomico.val(noEconomico);
            }
            else {
                cboModalGrupoMaquinaria.clearCombo();
                cboModalGrupoMaquinaria.attr('disabled', true);
            }

        }

        function FillCboModelo() {

            if (cboModalMarca.val() != null && cboModalMarca.val() != "") {
                cboModalModelo.fillCombo('/CatMaquina/FillCboModelo_Maquina', { idMarca: cboModalMarca.val() });
                cboModalModelo.attr('disabled', false);
            }
            else {
                cboModalModelo.clearCombo();
                cboModalModelo.attr('disabled', true);
            }
        }

        function FillCboMarca() {

            fillTXTNoEconomico();
            if (cboModalGrupoMaquinaria.val() != null && cboModalGrupoMaquinaria.val() != "") {
                GetInfo();
                cboModalMarca.fillCombo('/CatMaquina/FillCboMarca_Maquina', { idGrupo: cboModalGrupoMaquinaria.val() });
                cboModalMarca.attr('disabled', false);
            }
            else {
                cboModalMarca.clearCombo();
                cboModalMarca.attr('disabled', true);
            }
        }

        function GetNumEconomico() {
            if (cboModalGrupoMaquinaria.val() != "") {

            }
            else {
                cboModalGrupoMaquinaria.clearCombo();
                cboModalGrupoMaquinaria.attr('disabled', true);
            }
        }


        function clickBuscar() {
            filtrarGrid();
        }

        function openModal() {

            tituloModal.text("Alta Maquinaria");
            reset();
            cboModalEstatus.prop('disabled', true);
            modalAcciones.modal('show');
        }
        function update() {
            tituloModal.text("Actualizar Maquinaria");
            cboModalEstatus.prop('disabled', false);
            modalAcciones.modal('show');
        }

        function guardar() {
            beforeSaveOrUpdate();
        }

        function beforeSaveOrUpdate() {

            if (valid()) {
                saveOrUpdate(getPlainObject());
            }
        }

        function getPlainObject() {
            return {
                id: idMaquina,
                noEconomico: txtModalNoEconomico.val(),
                descripcion: txtModaldescripcion.val().trim(),
                grupoMaquinariaID: cboModalGrupoMaquinaria.val(),
                proveedor: txtModalProveedor.val(),
                modeloEquipoID: cboModalModelo.val(),
                marcaID: cboModalMarca.val(),
                anio: cboModalAnios.val(),
                placas: txtModalPlaca.val(),
                noSerie: txtModalNoSerie.val(),
                noPoliza: txtModalPoliza.val(),
                TipoCombustibleID: cboModalTipoCombustible.val(),
                capacidadTanque: txtModalCapTanque.val(),
                fechaAdquisicion: dateModalFechaAdquiere.val(),
                fechaPoliza: dateModalVigenciaPoliza.val(),
                unidadCarga: cboModalUnidadCarga.val(),
                aseguradoraID: cboModalAseguradoras.val(),
                capacidadCarga: txtModalCapacidadCarga.val(),
                renta: cboModalEquipoRenta.val(),
                tipoEncierro: cboModalTipoEncierro.val(),
                horometroAdquisicion: txtModalHorasAdquisicion.val(),
                horometroActual: txtModalHorometroActual.val(),
                Estatus: cboModalEstatus.val() == estatus.ACTIVO ? true : false,
                TipoBajaID: cboTipoBaja.val(),
                kmBaja: txtKMBaja.getVal(0),
                HorometroBaja: txtHoroBaja.getVal(0),
                centro_costos: cboCentroCostos.val()

            }
        }

        function valid() {
            var state = true;

            if (!validarCampo(txtModalNoEconomico)) { state = false; }
            if (!validarCampo(cboModalTipoMaquinaria)) { state = false; }
            if (!validarCampo(cboModalGrupoMaquinaria)) { state = false; }
            if (!validarCampo(cboModalMarca)) { state = false; }
            if (!validarCampo(cboModalModelo)) { state = false; }
            if (!validarCampo(cboModalAnios)) { state = false; }
            if (!validarCampo(txtModalNoSerie)) { state = false; }
            if (!validarCampo(cboModalAseguradoras)) { state = false; }
            if (!validarCampo(txtModalPoliza)) { state = false; }
            //if (!validarCampo(cboModalTipoCombustible)) { state = false; }
            //  if (!validarCampo(txtModalCapTanque)) { state = false; }
            // if (!validarCampo(cboModalUnidadCarga)) { state = false; }
            if (!validarCampo(cboCentroCostos)) { state = false; }
            if (!validarCampo(txtModalProveedor)) { state = false; }
            if (!validarCampo(dateModalFechaAdquiere)) { state = false; }
            if (!validarCampo(dateModalVigenciaPoliza)) { state = false; }
            if (!validarCampo(txtModaldescripcion)) { state = false; }
            //  if (!validarCampo(txtModalHorasAdquisicion)) { state = false; }
            //  if (!validarCampo(txtModalHorometroActual)) { state = false; }
            return state;
        }

        function validBaja() {
            var state = true;
            if (!validarCampo(cboTipoBaja)) { state = false; }
            return state;
        }

        function validarCampo(_this) {
            var r = false;
            if (_this.val() == '' || _this.val() == '$0.00' || _this.val() == '0' || _this.val() == '0.00%' || _this.val() == '--Seleccione--') {
                if (!_this.hasClass("errorClass")) {
                    _this.addClass("errorClass")
                }
                r = false;
            }
            else {
                if (_this.hasClass("errorClass")) {
                    _this.removeClass("errorClass")
                }
                r = true;
            }
            return r;
        }

        function saveOrUpdate(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatMaquina/SaveOrUpdate_Maquinaria',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: obj, Actualizacion: Actualizacion }),
                success: function (response) {
                    modalAcciones.modal('hide');
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    reset();
                    if (Actualizacion == 1) {
                        resetFiltros();
                    }
                    else {
                        filtrarGrid();
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function SaveOrUpdateDarBaja(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatMaquina/SaveOrUpdateDarBaja',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: obj, Actualizacion: Actualizacion }),
                success: function (response) {
                    modalAcciones.modal('hide');
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    reset();
                    if (Actualizacion == 1) {
                        if (response.success) {
                            ConfirmacionGuardadoCalidad("Mensaje Confirmación", "El registro fue dado de baja correctamente", '')

                        } else {
                            AlertaGeneral("Alerta", "El registro NO pudo ser dado de baja, verifique los campos");

                        }

                    }
                    else {
                        filtrarGrid();
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
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
                close: function (event, ui) { window.location.href = "/CatMaquina/index"; },
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


        function resetFiltros() {
            cboFiltroEstatus.val('1');
            txtFiltroDescripcion.val('');
            txtFiltroNoEconomico.val('');
            gridResultado.bootgrid('clear');
        }

        function initGrid() {
            gridResultado.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-grupoMaquinariaID='" + row.grupoMaquinariaID + "' data-modeloEquipoID='" + row.modeloEquipoID + "' data-marcaID='" +
                            row.marcaID + "'" + "' data-anio='" + row.anio + "' data-placas='" + row.placas + "'" + "data-TipoEncierro='" + row.tipoEncierro + "'" +
                            "data-noSerie='" + row.noSerie + "' data-aseguradoraID='" + row.aseguradoraID + "' data-noPoliza='" + row.noPoliza + "'" +
                            "data-noEconomico='" + row.noEconomico + "'" +
                            "data-TipoCombustibleID='" + row.TipoCombustibleID + "' data-capacidadTanque='" + row.capacidadTanque + "' data-unidadCarga='" + row.unidadCarga + "'" +
                            "data-capacidadCarga='" + row.capacidadCarga + "' data-horometroAdquisicion='" + row.horometroAdquisicion + "' data-horometroActual='" + row.horometroActual + "'" + "' data-renta='" + row.renta + "' data-estatus='" + row.estatus + "'" + " data-tipoEquipoID='" + row.tipoEquipoID + "'" +
                            "data-fechaAdquisicion='" + row.fechaAdquisicion + "'data-fechaPoliza='" + row.fechaPoliza + "' data-proveedor='" + row.proveedor + "'>" +
                            "<span class='glyphicon glyphicon-edit '></span> " +
                            " </button>";
                    },
                    "delete": function (column, row) {
                        var boton =
                            "<button type='button' class='btn btn-danger eliminar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-grupoMaquinariaID='" + row.grupoMaquinariaID + "' data-modeloEquipoID='" + row.modeloEquipoID + "' data-marcaID='" + row.marcaID + "'" +
                            "data-anio='" + row.anio + "' data-placas='" + row.placas + "'" + "data-TipoEncierro='" + row.tipoEncierro + "'" +
                            "data-noSerie='" + row.noSerie + "' data-aseguradoraID='" + row.aseguradoraID + "' data-noPoliza='" + row.noPoliza + "'" + "data-noEconomico='" + row.noEconomico + "'" +
                            "data-TipoCombustibleID='" + row.TipoCombustibleID + "' data-capacidadTanque='" + row.capacidadTanque + "' data-unidadCarga='" + row.unidadCarga + "'" +
                            "data-capacidadCarga='" + row.capacidadCarga + "' data-horometroAdquisicion='" + row.horometroAdquisicion + "' data-horometroActual='"
                            + row.horometroActual + "'" + "' data-renta='" + row.renta + "' data-estatus='" + row.estatus + "' data-tipoEquipoID='" + row.tipoEquipoID + "'" +
                            "data-fechaAdquisicion='" + row.fechaAdquisicion + "'data-fechaPoliza='" + row.fechaPoliza + "' data-proveedor='" + row.proveedor + "' data-tipocaptura='" + row.tipoCaptura + "'>" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>";
                        return boton;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                gridResultado.find(".modificar").on("click", function (e) {
                    idMaquina = $(this).attr("data-index");
                    $('a[href^="#step-1"]').trigger('click');
                    $('a[href^="#step-2"]').removeAttr("disabled", "disabled");
                    $('a[href^="#step-4"]').removeAttr("disabled", "disabled");
                    Actualizacion = 2;
                    datosActualizacion($(this));
                    update();
                });

                gridResultado.find(".eliminar").on("click", function (e) {
                    var estado = $(this).attr("data-estatus");

                    if (estado == "ACTIVO") {
                        var tipoCaptura = $(this).attr("data-tipocaptura");
                        if (tipoCaptura == '2') {
                            txtKMBaja.setVal(0);
                            $(".txtKMBaja").show();
                        }
                        else {
                            txtKMBaja.setVal(0);
                            $(".txtKMBaja").hide();
                        }
                        if (tipoCaptura == '1') {
                            $.ajax({
                                url: '/CatMaquina/GetHorometroFinal',
                                type: 'POST',
                                dataType: 'json',
                                contentType: 'application/json',
                                data: JSON.stringify({ eco: $(this).attr("data-noeconomico") }),
                                success: function (response) {
                                    if (!Number.isNaN(Number(response.horometro))) {
                                        txtHoroBaja.attr('data-horometro', response.horometro);
                                    }
                                    txtHoroBaja.setVal(response.horometro);
                                    $(".txtHorometroBaja").show();
                                },
                                error: function (response) {
                                }
                            });

                        }
                        else {
                            txtHoroBaja.setVal(0);
                            $(".txtHorometroBaja").hide();
                        }
                        idMaquina = $(this).attr("data-index");
                        Actualizacion = 3;
                        datosActualizacion($(this));
                        cboModalEstatus.val("0");
                        lblBaja.text($(this).attr("data-noEconomico"));
                        modalDesactivar.modal();
                    }
                    else {
                        reset();
                    }

                });

                gridResultado.find(".activar").on("click", function (e) {
                    var estado = $(this).attr("data-estatus");

                    if (estado == "INACTIVO") {
                        idMaquina = $(this).attr("data-index");
                        Actualizacion = 4;
                        datosActualizacion($(this));
                        cboModalEstatus.val("0");
                        lblAlta.text($(this).attr("data-noEconomico"));
                        modalActivar.modal();
                    }
                    else {
                        reset();
                    }

                });
            });
        }

        function datosActualizacion(row) {
            cboModalTipoMaquinaria.val(row.attr("data-tipoEquipoID"));
            cboModalEquipoRenta.val(row.attr("data-renta")).attr('disabled', true);;
            cboModalTipoEncierro.val(row.attr("data-TipoEncierro"));
            cboModalGrupoMaquinaria.fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: row.attr("data-tipoEquipoID") });

            cboModalGrupoMaquinaria.val(row.attr("data-grupoMaquinariaID"));
            cboModalMarca.fillCombo('/CatMaquina/FillCboMarca_Maquina', { idGrupo: row.attr("data-grupoMaquinariaID") });
            cboModalMarca.val(row.attr("data-marcaID"));

            cboModalModelo.fillCombo('/CatMaquina/FillCboModelo_Maquina', { idMarca: row.attr("data-marcaID") });
            cboModalModelo.val(row.attr("data-modeloEquipoID"));
            txtModalNoEconomico.val(row.attr("data-noeconomico"));
            txtModaldescripcion.val(row.attr("data-descrip"));

            cboModalAnios.val(row.attr("data-anio"));
            txtModalPlaca.val(row.attr("data-placas"));
            txtModalNoSerie.val(row.attr("data-noSerie"));
            cboModalAseguradoras.val(row.attr("data-aseguradoraID"));
            txtModalPoliza.val(row.attr("data-noPoliza"));
            dateModalVigenciaPoliza.val(row.attr("data-fechaPoliza"));
            cboModalTipoCombustible.val(row.attr("data-TipoCombustibleID"));
            txtModalCapTanque.val(row.attr("data-capacidadTanque"));
            txtModalCapacidadCarga.val(row.attr("data-capacidadCarga"));
            cboModalUnidadCarga.val(row.attr("data-unidadCarga"));
            txtModalProveedor.val(row.attr("data-proveedor"));
            dateModalFechaAdquiere.val(row.attr("data-fechaadquisicion"));
            txtModalHorasAdquisicion.val(row.attr("data-horometroAdquisicion"));
            txtModalVHorometroAdquisicion.val(row.attr("data-horometroAdquisicion"));
            txtModalHorometroActual.val(row.attr("data-horometroActual"));
            cboModalEstatus.val(row.attr("data-estatus") == "ACTIVO" ? "1" : "0");
        }

        function reset() {
            idMaquina = 0;
            $('a[href^="#step-1"]').trigger('click');
            $('a[href^="#step-2"]').attr("disabled", "disabled");
            $('a[href^="#step-4"]').attr("disabled", "disabled");
            cboModalEquipoRenta.val('0');
            cboModalTipoEncierro.val('');
            cboModalTipoMaquinaria.val('');
            cboModalGrupoMaquinaria.val('');
            txtModalNoEconomico.val('');
            cboModalModelo.val('');
            txtModaldescripcion.val('');
            cboModalMarca.val('');
            cboModalAnios.val('');
            cboModalEstatus.val('');
            txtModalNoSerie.val('');
            cboModalAseguradoras.val('');
            txtModalPoliza.val('');
            dateModalVigenciaPoliza.datepicker().datepicker("setDate", new Date());
            cboModalTipoCombustible.val('');
            txtModalCapTanque.val('0');
            txtModalCapacidadCarga.val('0');
            cboModalUnidadCarga.val('');
            txtModalProveedor.val('');
            dateModalFechaAdquiere.datepicker().datepicker("setDate", new Date());
            txtModalHorasAdquisicion.val('0');
            txtModalHorometroActual.val('0');
            txtModalPlaca.val('');
            txtModalVTipoMaquina.val('');
            txtModalVGrupo.val('');
            txtModalVNoEConomico.val('');
            txtModalVDescripcion.val('');
            txtModalVModelo.val('');
            txtModalVNoSerie.val('');
            Actualizacion = 1;
            cboModalEstatus.val('1');
            cboTipoBaja.val('');
            cboCentroCostos.val('');
            //   frmModal.validate().resetForm();
        }
        function filtrarGrid() {
            loadGrid(getDataFilter(), ruta, gridResultado);
        }

        function getDataFilter() {
            return {
                idTipo: cboFiltroTipo.val(),
                idGrupo: cboFiltroGrupo.val(),
                descripcion: txtFiltroDescripcion.val(),
                estatus: cboFiltroEstatus.val(),
                noEconomico: txtFiltroNoEconomico.val()
            }

        }

        /*Nuevo Codigo*/
        frmMaquinaFichaTecnica = $("#frmMaquinaFichaTecnica"),
            Paso1 = $("#Paso1"),
            txtModalProveedorFichaT = $("#txtModalProveedorFichaT"),
            txtFechaEntregaSitioFichaT = $("#txtFechaEntregaSitioFichaT"),
            txtLugarEntrega = $("#txtLugarEntrega"),
            tbOrdenCompra = $("#tbOrdenCompra"),
            tbCostoEquipo = $("#tbCostoEquipo"),
            cboTipoFichaT = $("#cboTipoFichaT"), // Tipo Equipo
            txtDescripcionFichaT = $("#txtDescripcionFichaT"),
            cboModalMarcaFichaT = $("#cboModalMarcaFichaT"),
            cboModalModeloFichaT = $("#cboModalModeloFichaT"),
            txtModalNoSerie = $("#txtModalNoSerie"),
            txtArreglo = $("#txtArreglo"),
            txtMarcaMotor = $("#txtMarcaMotor"),
            txtModeloMotor = $("#txtModeloMotor"),
            txtSerieMotor = $("#txtSerieMotor"),
            txtArrelgoMotor = $("#txtArrelgoMotor"),
            cboCondicionesUso = $("#cboCondicionesUso"),
            cboAdquisicionEquipo = $("#cboAdquisicionEquipo"),
            cboModalAniosFichaT = $("#cboModalAniosFichaT"),
            cboLugarFabricacion = $("#cboLugarFabricacion"),
            tbnumPedimento = $("#tbnumPedimento"),
            report = $("#report"),
            btnGuardarFicha = $("#btnGuardarFicha");

        /*END*/
        function setFichaTecnica() {

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatMaquina/getData',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: getFichaTecnica() }),
                success: function (response) {
                    var path = "/Reportes/Vista.aspx?idReporte=25";
                    report.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();

                    };

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function getFichaTecnica() {
            return {
                Proveedor: txtModalProveedorFichaT.val(),
                EntregaSitio: txtFechaEntregaSitioFichaT.val(),
                LugarEntrega: txtLugarEntrega.val(),
                OrdenCompra: tbOrdenCompra.val(),
                CostoEquipo: tbCostoEquipo.val(),
                TipoEquipo: cboTipoFichaT.val(),
                Descripcion: txtDescripcionFichaT.val(),
                Marca: cboModalMarcaFichaT.val(),
                Modelo: cboModalModeloFichaT.val(),
                NoSerie: txtModalNoSerie.val(),
                Arreglo: txtArreglo.val(),
                MarcaMotor: txtMarcaMotor.val(),
                ModeloMotor: txtModeloMotor.val(),
                SerieMotor: txtSerieMotor.val(),
                ArregloMotor: txtArrelgoMotor.val(),
                CodicionesUso: cboCondicionesUso.val(),
                Adquisicion: cboAdquisicionEquipo.val(),
                añoEquipo: cboModalAniosFichaT.val(),
                LugarFabricacion: cboLugarFabricacion.val(),
                Pedimento: tbnumPedimento.val()

            }

        }

        function GetInfo() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/GetNumeroEconomico",
                data: { idGrupo: cboModalGrupoMaquinaria.val(), renta: cboModalEquipoRenta.val() == 0 ? false : true },
                success: function (response) {
                    var datos = response.NumEconomico;
                    txtModalNoEconomico.val(datos);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        init();

    };

    $(document).ready(function () {
        maquinaria.catalogo.maquina = new maquina();
    });
})();


