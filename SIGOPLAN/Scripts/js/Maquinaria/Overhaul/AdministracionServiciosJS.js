(function () {

    $.namespace('maquinaria.overhaul.administracionservicios');

    administracionservicios = function () {
        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        let tipoUsuario = 6;
        const ulNuevo = $("#ulNuevo");
        // Servicios Activos
        const txtFiltroEconomicoServAct = $("#txtFiltroEconomicoServAct");
        const txtFiltroNombreServAct = $("#txtFiltroNombreServAct");
        const cboCCServAct = $("#cboCCServAct");
        const cboFiltroGrupoMaquinaServAct = $("#cboFiltroGrupoMaquinaServAct");
        const cboFiltroModeloMaquinaServAct = $("#cboFiltroModeloMaquinaServAct");
        const cboFiltroEstatusMaquinaServAct = $("#cboFiltroEstatusMaquinaServAct");
        const btnBuscarServAct = $("#btnBuscarServAct");
        const gridServAct = $("#gridServAct");
        let dtServAct;

        const modalDetallesServAct = $("#modalDetallesServAct");
        const titlemodalServAct = $("#titlemodalServAct");
        //cboFiltroModalEconomicoServAct = $("#cboFiltroModalEconomicoServAct");
        const txtFiltroModalServAct = $("#txtFiltroModalServAct");
        const btnBuscarModalServAct = $("#btnBuscarModalServAct");
        const frmServAct = $("#frmServAct");
        const gridDetallesServAct = $("#gridDetallesServAct");
        const modalHistorialServAct = $("#modalHistorialServAct");
        const titlemodalhistorialServAct = $("#titlemodalhistorialServAct");
        const frmDetallesHistorialServAct = $("#frmDetallesHistorialServAct");
        const lgHistorialServAct = $("#lgHistorialServAct");
        const gridDetallesHistorialServAct = $("#gridDetallesHistorialServAct");
        const btnModificar = $("#btnModificarServicio");
        const modalModificar = $("#modalModificar");
        const txtModalModificarHoraCiclo = $("#txtModalModificarHoraCiclo");
        const cboModalModificarEstatus = $("#cboModalModificarEstatus");
        const btnModalModificarGuardar = $("#btnModalModificarGuardar");

        // Catalogo Servicio
        const txtFiltroServicio = $("#txtFiltroServicio");
        const cboGrupoMaquinaServicio = $("#cboGrupoMaquinaServicio");
        const cboModeloMaquinaServicio = $("#cboModeloMaquinaServicio");
        const cboFiltroEstatusServicio = $("#cboFiltroEstatusServicio");
        const btnBuscarServicios = $("#btnBuscarServicios");
        const btnNuevoServicio = $("#btnNuevoServicio");
        const gridServicios = $("#gridServicios");
        const modalServicios = $("#modalServicios");
        const titlemodalServicios = $("#titlemodalServicios");
        const frmServicios = $("#frmServicios");
        const divEncabezado = $("#divEncabezado");
        const txtModalNombreServicio = $("#txtModalNombreServicio");
        const cboModalGrupoMaquinaServicio = $("#cboModalGrupoMaquinaServicio");
        const cboModalModeloMaquinaServicio = $("#cboModalModeloMaquinaServicio");
        const txtModalDescripcionServicio = $("#txtModalDescripcionServicio");
        const divFechaInstalacion = $("#divFechaInstalacion");
        const btnModalGuardarServicio = $("#btnModalGuardarServicio");
        const btnModalCancelarServicio = $("#btnModalCancelarServicio");
        const modalAsignacionServ = $("#modalAsignacionServ");
        const cboModalAsignacionServ = $("#cboModalAsignacionServ");
        const btnGuardarModalAsignacionServ = $("#btnGuardarModalAsignacionServ");
        const txtModalAsignacionServHorasCiclo = $("#txtModalAsignacionServHorasCiclo");
        const txtModalAsignacionServCicloActual = $("#txtModalAsignacionServCicloActual");
        const ckPlaneacion = $("#ckPlaneacion");
        var ConfirmarEliminarEstatus = 0;

        //Guardar Archivos Aplicar Servicio
        const modalArchivosServicio = $("#modalArchivosServicio");
        const btncargarArchivo = $("#btncargarArchivo");
        const inCargarArchivo = $("#inCargarArchivo");
        const btnSubirArchivo = $("#btnSubirArchivo");
        const gridArchivos = $("#gridArchivos");
        const txtFechaAplicaServicio = $("#txtFechaAplicaServicio");
        let dtArchivos;
        const btnAplicarServicio = $("#btnAplicarServicio");

        const modalArchivosEvidencia = $("#modalArchivosEvidencia");
        const gridArchivosEvidencia = $("#gridArchivosEvidencia");
        let dtArchivosEvidencia;

        //Guardar Archivo Evidencia Historial
        const btnArchivoEvidencia = $("#btnArchivoEvidencia");
        const inArchivoEvidencia = $("#inArchivoEvidencia");

        let ruta = "";

        function init() {
            PermisosBotones();
            // Catalogo Servicio 
            initGridTipoServicio();
            cargarGridTipoServicio();

            // Servicios Activos
            initGridServAct();
            cargarGridServAct();
            initGridDetallesServAct();
            initGridHistorialServAct();

            //Historial Servicios
            initGridArchivos();
            initGridArchivosEvidencia();

            fillCombos();
            agregarListeners();
        }

        function PermisosBotones() {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/PermisosBotonesAdminComp",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                success: function (response) {
                    $.unblockUI();
                    tipoUsuario = response.tipoUsuario;
                    if (response.tipoUsuario < 3) {
                        btnNuevoServicio.prop("disabled", false);
                        btnModificar.prop("disabled", false);
                    }
                    else {
                        btnNuevoServicio.prop("disabled", true);
                        btnModificar.prop("disabled", true);
                    }
                    if (response.tipoUsuario == 7) {
                        btnNuevoServicio.prop("disabled", false);
                        btnModificar.prop("disabled", false);
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function fillCombos() {
            cboGrupoMaquinaServicio.fillCombo('/CatComponentes/FillCboGrupo_Componente');
            cboModalGrupoMaquinaServicio.fillCombo('/CatComponentes/FillCboGrupo_Componente');

            cboCCServAct.fillCombo('/Overhaul/FillCboObraMaquina');
            cboFiltroGrupoMaquinaServAct.fillCombo('/Overhaul/FillCboGrupoMaquinaComponentes', { obj: 0 });
        }

        function agregarListeners() {
            cboGrupoMaquinaServicio.change(FillCboModelo)
            cboModalGrupoMaquinaServicio.change(FillCboModalModelo);
            btnNuevoServicio.click(AbrirModalAltaServicio);
            btnModalGuardarServicio.click(GuardarTipoServicio);
            btnBuscarServicios.click(cargarGridTipoServicio);
            btnGuardarModalAsignacionServ.click(AsignarServicio);

            cboFiltroGrupoMaquinaServAct.change(cargarcboFiltroModeloMaquinaServAct);
            btnBuscarModalServAct.click(cargarGridDetallesServAct);
            btnBuscarServAct.click(cargarGridServAct);

            btncargarArchivo.click(function (e) {
                e.preventDefault();
                inCargarArchivo.click();
            });
            inCargarArchivo.change(function (e) {
                SubirArchivo(e);
            });

            btnArchivoEvidencia.click(function (e) {
                e.preventDefault();
                inArchivoEvidencia.click();
            });
            inArchivoEvidencia.change(function (e) {
                SubirArchivoHistorial(e);
            });


            ///////////
            $(document).on('click', "#btnModalEliminar", function (e) {
                e.preventDefault();
                console.log("ConfimarEliminarEstatus: " + ConfirmarEliminarEstatus);
                switch (ConfirmarEliminarEstatus) {
                    case 1:
                        DeshabilitarServicio(btnBuscarServicios.attr("data-index-eliminar"));
                        btnBuscarServicios.removeAttr("data-index-eliminar");
                        break;
                    case 2:
                        DesasignarServicio(btnBuscarModalServAct.attr("data-index-desasignar"));
                        break;
                    case 3:
                        AplicarServicio(btnBuscarModalServAct.attr("data-index"), btnBuscarModalServAct.attr("data-idmaquina"), btnBuscarModalServAct.attr("data-isplaneacion"));
                        break;
                }
            });

            $(document).on('click', "#btnCancelar", function (e) {
                e.preventDefault();
                switch (ConfirmarEliminarEstatus) {
                    case 1:
                        btnBuscarServicios.removeAttr("data-index-eliminar");
                        break;
                    case 2:
                        btnBuscarModalServAct.removeAttr("data-index-desasignar")
                        break;
                }
            });
            btnAplicarServicio.click(function (e) {
                ConfirmarEliminarEstatus = 3;
                ConfirmacionEliminacion("Desecho", "¿Desea aplicar el servicio " + btnBuscarModalServAct.attr("data-nombreservicio") + "?");
            });

            btnModificar.click(openModalModificar);
            btnModalModificarGuardar.click(guardarCambiosGrupo);

            $("#cbCicloVidaHoras").change(habilitarModificarVidaHoras);
            $("#cbEstatus").change(habilitarModificarEstatus);

            txtFechaAplicaServicio.datepicker().datepicker("setDate", new Date());
        }

        function cargarcboFiltroModeloMaquinaServAct() {
            if (cboFiltroGrupoMaquinaServAct.val() != "") {
                cboFiltroModeloMaquinaServAct.prop("disabled", false);
                cboFiltroModeloMaquinaServAct.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: cboFiltroGrupoMaquinaServAct.val() });
            }
            else {
                cboFiltroModeloMaquinaServAct.val("");
                cboFiltroModeloMaquinaServAct.prop("disabled", true);
            }
        }

        function initGridTipoServicio() {
            gridServicios.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {

                    "nombre": function (column, row) {
                        return "<span class='nombreTipoServ'> " + row.nombre + " </span>"
                    },
                    "modeloMaquina": function (column, row) {
                        return "<span class='modMaqTipoServ'> " + row.modeloMaquina + " </span>"
                    },
                    "estatus": function (column, row) {
                        return "<span class='modMaqTipoServ'> " + row.estatus + " </span>"
                    },
                    "modificar": function (column, row) {
                        var html = "";
                        if (tipoUsuario < 3 || tipoUsuario >= 7) {
                            html = "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "' data-modelomaquinaid='" + row.modeloMaquinaID + "' data-nombre='" + row.nombre + "' data-grupomaquina='" + row.grupoMaquinaID + "' data-descripcion='" + row.descripcion + "' data-planeacion='" + row.planeacion + "'>" +
                                "<span class='glyphicon glyphicon-edit'></span> " +
                                " </button>";
                        }
                        return html;
                    },
                    "deshabilitar": function (column, row) {
                        var html = "";
                        if (tipoUsuario < 3 || tipoUsuario >= 7) {
                            html = "<button type='button' class='btn btn-danger deshabilitar' data-index='" + row.id + "' data-nombre='" + row.nombre + "' data-modelo='" + row.modeloMaquina + "'>" +
                                "<span class='glyphicon glyphicon-remove'></span> " +
                                " </button>";
                        }
                        return html;
                    },
                    "asignar": function (column, row) {
                        var html = "";
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 7) {
                            html = "<button type='button' class='btn btn-primary asignar' data-index='" + row.id + "' data-modelomaquinaid='" + row.modeloMaquinaID + "' data-nombre='" + row.nombre + "' data-modelo='" + row.modeloMaquina + "'>" +
                                "<span class='glyphicon glyphicon-pushpin'></span> " +
                                " </button>";
                        }
                        return html;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                gridServicios.find(".modificar").parent().css("text-align", "center");
                gridServicios.find(".modificar").parent().css("width", "3%");
                gridServicios.find(".deshabilitar").parent().css("text-align", "center");
                gridServicios.find(".deshabilitar").parent().css("width", "3%");
                gridServicios.find(".modificar").on("click", function (e) {
                    txtModalNombreServicio.val($(this).attr("data-nombre"));
                    txtModalNombreServicio.prop("disabled", true);
                    cboModalGrupoMaquinaServicio.val($(this).attr("data-grupomaquina"));
                    cboModalGrupoMaquinaServicio.change();
                    cboModalGrupoMaquinaServicio.prop("disabled", true);
                    cboModalModeloMaquinaServicio.val($(this).attr("data-modelomaquinaid"));
                    cboModalModeloMaquinaServicio.change();
                    cboModalModeloMaquinaServicio.prop("disabled", true);
                    txtModalDescripcionServicio.val($(this).attr("data-descripcion"));
                    btnModalGuardarServicio.attr("data-index", $(this).attr("data-index"));
                    $(this).attr("data-planeacion") == "true" ? ckPlaneacion.prop('checked', true) : ckPlaneacion.prop('checked', false);
                    AbrirModalUpdateServicio();
                });
                gridServicios.find(".deshabilitar").on("click", function (e) {
                    ConfirmarEliminarEstatus = 1;
                    ConfirmacionEliminacion("Desecho", "¿Desea eliminar el servicio " + $(this).attr("data-nombre") + " para " + $(this).attr("data-modelo") + "?");
                    btnBuscarServicios.attr("data-index-eliminar", $(this).attr("data-index"));
                });
                gridServicios.find(".asignar").on("click", function (e) {
                    cboModalAsignacionServ.fillCombo('/Overhaul/FillEconomicoByModelo', { modeloID: $(this).attr("data-modelomaquinaid") });
                    btnGuardarModalAsignacionServ.attr("data-servicioid", $(this).attr("data-index"));
                    $("#txtModalAsignacionServNombre").val($(this).attr("data-nombre"));
                    $("#txtModalAsignacionServModelo").val($(this).attr("data-modelo"));
                    txtModalAsignacionServHorasCiclo.val("0");
                    txtModalAsignacionServCicloActual.val("0");
                    modalAsignacionServ.modal("show");
                });
            });
        }

        function cargarGridTipoServicio() {
            var nombre = txtFiltroServicio.val().trim().toUpperCase();
            var grupo = cboGrupoMaquinaServicio.val();
            var modelo = cboModeloMaquinaServicio.val();
            var estatus = cboFiltroEstatusServicio.val() == 1;

            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarTipoServicios",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ nombre: nombre, grupo: grupo, modelo: modelo, estatus: estatus }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridServicios.bootgrid({
                            rowCount: -1,
                            templates: {
                                header: ""
                            }
                        });
                        gridServicios.bootgrid("clear");
                        gridServicios.bootgrid("append", response.tipoServicios);
                        gridServicios.bootgrid('reload');
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

        function AbrirModalAltaServicio() {
            resetModal();
            btnModalGuardarServicio.attr("data-index", "-1");
            txtModalNombreServicio.prop("disabled", false);
            cboModalGrupoMaquinaServicio.prop("disabled", false);
            cboModalModeloMaquinaServicio.prop("disabled", false);
            titlemodalServicios.text("Alta de servicio");
            modalServicios.modal("show");
        }

        function AbrirModalUpdateServicio() {
            //resetModal();
            titlemodalServicios.text("Modificación del servicio");
            modalServicios.modal("show");
        }

        function resetModal() {
            txtModalNombreServicio.val("");
            txtModalDescripcionServicio.val("");
            cboModalGrupoMaquinaServicio.val("");
            cboModalModeloMaquinaServicio.val("");
            ckPlaneacion.prop('checked', false);
        }

        function FillCboModelo() {
            if (cboGrupoMaquinaServicio.val() != null && cboGrupoMaquinaServicio.val() != "") {
                cboModeloMaquinaServicio.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: cboGrupoMaquinaServicio.val() });
                cboModeloMaquinaServicio.attr('disabled', false);
            }
            else {
                cboModeloMaquinaServicio.clearCombo();
                cboModeloMaquinaServicio.attr('disabled', true);
            }
        }

        function FillCboModalModelo() {
            if (cboModalGrupoMaquinaServicio.val() != null && cboModalGrupoMaquinaServicio.val() != "") {
                cboModalModeloMaquinaServicio.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: cboModalGrupoMaquinaServicio.val() });
                cboModalModeloMaquinaServicio.attr('disabled', false);
            }
            else {
                cboModalModeloMaquinaServicio.clearCombo();
                cboModalModeloMaquinaServicio.attr('disabled', true);
            }
        }

        function GuardarTipoServicio() {
            if (ValidarInfoTipoServicio()) {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/Overhaul/GuardarTipoServicioOverhaul',
                    //async: false,
                    data: { obj: GetDatosTipoServicio() },
                    success: function (response) {
                        if (response.exito) {
                            AlertaGeneral("Éxito", "Se ha guardado el servicio");
                            cargarGridTipoServicio();
                            modalServicios.modal("hide");
                        }
                        else { AlertaGeneral("Alerta", "Ya existe un servicio con ese nombre, no se han guardado los datos"); }
                        $.unblockUI();
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.MESSAGE);
                    }
                });
            }
            else {
                AlertaGeneral("Alerta", "Faltan datos por proporcionar");
            }
        }

        function ValidarInfoTipoServicio() {
            var estado = true;
            if (txtModalNombreServicio.val() == null || txtModalNombreServicio.val() == "") { estado = false; }
            if (cboModalGrupoMaquinaServicio.val() == null || cboModalGrupoMaquinaServicio.val() == "") { estado = false; }
            if (cboModalModeloMaquinaServicio.val() == null || cboModalModeloMaquinaServicio.val() == "") { estado = false; }
            if (txtModalDescripcionServicio.val() == null || txtModalDescripcionServicio.val() == "") { estado = false; }
            return estado;
        }

        function GetDatosTipoServicio() {
            return {
                id: (btnModalGuardarServicio.attr("data-index") == "-1" ? 0 : btnModalGuardarServicio.attr("data-index")),
                nombre: txtModalNombreServicio.val().trim().toUpperCase(),
                descripcion: txtModalDescripcionServicio.val().trim(),
                grupoMaquinaID: cboModalGrupoMaquinaServicio.val(),
                modeloMaquinaID: cboModalModeloMaquinaServicio.val(),
                estatus: true,
                planeacion: ckPlaneacion.is(":checked")
            }
        }

        function AsignarServicio() {
            if (ValidarInfoAsignacion()) {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/Overhaul/GuardarAsignacionServicioOverhaul',
                    //async: false,
                    data: { obj: GetDatosAsignacionServicio() },
                    success: function (response) {
                        $.unblockUI();
                        if (response.exito) {
                            modalAsignacionServ.modal("hide");
                            cargarGridServAct();
                            AlertaGeneral("Éxito", "Se ha asignado el servicio " + $("#txtModalAsignacionServNombre").val() + " en el Económico " + $("#cboModalAsignacionServ option:selected").text());

                        }
                        else {
                            modalAsignacionServ.modal("hide");
                            AlertaGeneral("Alerta", "Ya existe un servicio con el nombre " + $("#txtModalAsignacionServNombre").val().trim().toUpperCase() + " para ese modelo, no se han guardado los datos");
                        }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.MESSAGE);
                    }
                });
            }
            else {
                AlertaGeneral("Alerta", "Faltan datos por proporcionar");
            }
        }

        function ValidarInfoAsignacion() {
            var estado = true;
            if (cboModalAsignacionServ.val() == null || cboModalAsignacionServ.val() == "") { estado = false; }
            if (txtModalAsignacionServHorasCiclo.val() == null || txtModalAsignacionServHorasCiclo.val() == "" || txtModalAsignacionServHorasCiclo.val() <= 0) { estado = false; }
            if (txtModalAsignacionServCicloActual.val() == null || txtModalAsignacionServCicloActual.val() == "" || txtModalAsignacionServCicloActual.val() < 0) { estado = false; }
            return estado;
        }

        function GetDatosAsignacionServicio() {
            return {
                id: 0,
                tipoServicioID: btnGuardarModalAsignacionServ.attr("data-servicioid"),
                maquinaID: cboModalAsignacionServ.val(),
                centroCostos: 0,
                cicloVidaHoras: txtModalAsignacionServHorasCiclo.val(),
                horasCicloActual: txtModalAsignacionServCicloActual.val(),
                fechaAsignacion: new Date(),
                estatus: true
            }
        }

        function DeshabilitarServicio(index) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/DeshabilitarServicioOverhaul",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idServicio: index }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        if (response.exito) {
                            AlertaGeneral("Éxito", "Se ha deshabilitado el servicio");
                            cargarGridTipoServicio();
                        }
                        else { AlertaGeneral("Alerta", "No se pudo deshabilitar el servicio. Intente más tarde"); }
                    }
                    else {
                        AlertaGeneral("Alerta", "Error al realizar la consulta");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        ////////////////
        function initGridServAct() {
            dtServAct = gridServAct.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                dom: '<tp>',
                columns: [
                    {
                        data: 'CCName',
                        title: 'Centro De Costos',
                        render: function (data, type, row) {
                            return "<span class='nombreTipoServ'> " + data + " </span>"
                        }
                    },
                    {
                        data: 'economico',
                        title: 'Económico',
                        render: function (data, type, row) {
                            return "<span class='modMaqTipoServ'> " + data + " </span>"
                        }
                    },
                    {
                        data: 'servicios',
                        title: 'Estatus servicios',
                        render: function (data, type, row) {
                            var color = "";
                            var html = "";
                            for (var i = 0; i < data.length; i++) {
                                var resta = data[i].Item2.cicloVidaHoras - data[i].Item2.horasCicloActual;
                                color = "";
                                if (resta > 1000) { color = "#009900"; }
                                else {
                                    if (resta > 0) { color = "#ffcc00"; }
                                    else { color = "#ff6600"; }
                                }
                                var nombreCorto = data[i].Item1.nombre.substring(0, 1);
                                html += "<span><span class='btn dot dotClick' data-toggle='tooltip' title='" + data[i].Item1.nombre + " - " + data[i].Item1.descripcion
                                    + "' data-placement='bottom' style='background-color:" + color + "; text-align:center' data-nombreServicio='" + data[i].Item1.nombre + "' data-idmaquina='" + row.maquinaID
                                    + "'><b>" + nombreCorto + "</b></span>&nbsp;&nbsp;</span>";
                            }
                            return html;
                        }
                    },
                    {
                        data: 'vidaRestanteMaxima',
                        title: 'Detalle',
                        render: function (data, type, row) {
                            return "<button type='button' class='btn btn-primary detalle' data-index='" + row.id + "' data-idmaquina='" + row.maquinaID + "'>" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                            " </button>";
                        }
                    },
                ],
                columnDefs: [
                    { "width": "300", "targets": [0] },
                    { "width": "100", "targets": [1] },
                    { "width": "40", "targets": [3] }
                ],
                order: [[3, 'desc']],
                drawCallback: function () {
                    gridServAct.find(".detalle").on("click", function (e) {
                        //cboFiltroModalEconomicoServAct.val($(this).attr("data-idmaquina"));
                        btnBuscarModalServAct.attr("data-maquinaID", $(this).attr("data-idmaquina"));
                        txtFiltroModalServAct.val("");
                        cargarGridDetallesServAct();
                        modalDetallesServAct.modal("show");
                    });
                    gridServAct.find(".dotClick").on("click", function (e) {
                        //cboFiltroModalEconomicoServAct.val($(this).attr("data-idmaquina"));
                        btnBuscarModalServAct.attr("data-maquinaID", $(this).attr("data-idmaquina"));
                        txtFiltroModalServAct.val("");
                        txtFiltroModalServAct.val($(this).attr("data-nombreServicio"))
                        cargarGridDetallesServAct();
                        modalDetallesServAct.modal("show");
                    });
                }

            });
        }

        function cargarGridServAct() {
            var economico = txtFiltroEconomicoServAct.val().trim().toUpperCase();
            var servicio = txtFiltroNombreServAct.val().trim().toUpperCase();

            var cc = cboCCServAct.val();
            var grupoMaquina = cboFiltroGrupoMaquinaServAct.val();
            var modeloMaquina = cboFiltroModeloMaquinaServAct.val();
            var estatus = cboFiltroEstatusMaquinaServAct.val() == 1;

            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarServiciosActivos",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ economico: economico, servicio: servicio, cc: cc, grupoMaquina: grupoMaquina, modeloMaquina: modeloMaquina, estatus: estatus }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        //gridServAct.bootgrid({
                        //    rowCount: -1,
                        //    templates: {
                        //        header: ""
                        //    }
                        ////});
                        //gridServAct.bootgrid("clear");
                        //gridServAct.bootgrid("append", response.servicios);
                        //gridServAct.bootgrid('reload');


                        dtServAct.clear();
                        dtServAct.rows.add(response.servicios);
                        dtServAct.draw();

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

        function initGridDetallesServAct() {
            gridDetallesServAct.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {

                    "estatus": function (column, row) {
                        var resta = row.cicloVidaHoras - row.horasCicloActual;
                        var color;
                        if (resta > 1000) { color = "#009900"; }
                        else {
                            if (resta > 0) { color = "#ffcc00"; }
                            else { color = "#ff6600"; }
                        }
                        return "<span class='dot' style='background-color:" + color + "'></span>";
                    },
                    "detalle": function (column, row) {
                        return "<button type='button' class='btn btn-primary detalle' data-index='" + row.id + "' data-economico='" + row.economico + "' data-nombreservicio='" + row.nombreServicio + "'>" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                            " </button>";
                    },
                    "aplicar": function (column, row) {
                        var html = "";
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                            html = "<button type='button' class='btn btn-primary aplicar' data-index='" + row.id + "' data-idmaquina='" + row.maquinaID + "' data-isplaneacion='" + row.isPlaneacion + "' " + (row.estatus == 0 ? "disabled" : "") + " data-nombreservicio='" + row.nombreServicio + "' data-economico='" + row.economico + "'>" +
                                "<span class='glyphicon glyphicon-wrench'></span> " +
                                " </button>";
                        }
                        return html;
                    },
                    "eliminar": function (column, row) {
                        var html = "";
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 7) {
                            html = "<button type='button' class='btn btn-primary eliminar' data-index='" + row.id + "' data-idmaquina='" + row.maquinaID + "' data-economico='" + row.economico + "' data-nombreservicio='" + row.nombreServicio + "' " + (row.estatus == 0 ? "disabled" : "") + ">" +
                                "<span class='glyphicon glyphicon-remove'></span></button>";
                        }
                        return html;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                gridDetallesServAct.find(".estatus").parent().css("text-align", "center");
                gridDetallesServAct.find(".estatus").parent().css("width", "3%");
                gridDetallesServAct.find(".detalle").parent().css("text-align", "center");
                gridDetallesServAct.find(".detalle").parent().css("width", "3%");
                gridDetallesServAct.find(".aplicar").parent().css("text-align", "center");
                gridDetallesServAct.find(".aplicar").parent().css("width", "3%");

                gridDetallesServAct.find(".detalle").on("click", function (e) {
                    titlemodalhistorialServAct.text("Historial de " + $(this).attr("data-nombreservicio") + " para el económico " + $(this).attr("data-economico"));
                    CargarModalHistorialServAct($(this).attr("data-index"));
                    btnArchivoEvidencia.attr("data-index", $(this).attr("data-index"));
                    modalHistorialServAct.modal("show");
                });

                gridDetallesServAct.find(".aplicar").on("click", function (e) {
                    dtArchivos.clear().draw(); //TODO
                    btnArchivoEvidencia.attr("data-index", $(this).attr("data-index"));

                    btnBuscarModalServAct.attr("data-index", $(this).attr("data-index"));
                    btnBuscarModalServAct.attr("data-idmaquina", $(this).attr("data-idmaquina"));
                    btnBuscarModalServAct.attr("data-isplaneacion", $(this).attr("data-isplaneacion"));
                    btnBuscarModalServAct.attr("data-nombreservicio", $(this).attr("data-nombreservicio"));
                    $("#title-modal-aplicar").text("Aplicar " + $(this).attr("data-nombreservicio") + " para " + $(this).attr("data-economico"));

                    //gridArchivos.bootgrid("clear");
                    modalArchivosServicio.modal("show");
                    //ConfirmacionEliminacion("Desecho", "¿Desea aplicar el servicio " + $(this).attr("data-nombreservicio") + "?");

                });
                gridDetallesServAct.find(".eliminar").parent().css("text-align", "center");
                gridDetallesServAct.find(".eliminar").parent().css("width", "3%");
                gridDetallesServAct.find(".eliminar").on("click", function (e) {
                    ConfirmarEliminarEstatus = 2;
                    ConfirmacionEliminacion("Desecho", "¿Desea desasignar el servicio " + $(this).attr("data-nombreservicio") + " para el económico " + $(this).attr("data-economico") + "?");
                    btnBuscarModalServAct.attr("data-index-desasignar", $(this).attr("data-index"));
                });
            });
        }

        function cargarGridDetallesServAct() {
            var idMaquina = btnBuscarModalServAct.attr("data-maquinaID");
            var servicio = txtFiltroModalServAct.val().trim().toUpperCase();
            var estatus = cboFiltroEstatusMaquinaServAct.val() == 1;

            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarModalServiciosActivos",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ idMaquina: idMaquina, servicio: servicio, estatus: estatus }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridDetallesServAct.bootgrid({
                            rowCount: -1,
                            templates: {
                                header: ""
                            }
                        });
                        gridDetallesServAct.bootgrid("clear");
                        gridDetallesServAct.bootgrid("append", response.servicios);
                        gridDetallesServAct.bootgrid('reload');
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

        function initGridHistorialServAct() {
            gridDetallesHistorialServAct.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "evidencia": function (column, row) {
                        var html = "";
                        //if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 7) {
                            html = "<button type='button' class='btn btn-primary evidencia' data-index='" + row.id + "'>" +
                                "<span class='glyphicon glyphicon-file'></span></button>";
                        //}
                        return html;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                gridDetallesHistorialServAct.find(".evidencia").parent().css("text-align", "center");
                gridDetallesHistorialServAct.find(".evidencia").parent().css("width", "3%");
                gridDetallesHistorialServAct.find(".subirEvidencia").parent().css("text-align", "center");
                gridDetallesHistorialServAct.find(".subirEvidencia").parent().css("width", "3%");
                gridDetallesHistorialServAct.find(".evidencia").on("click", function (e) {
                    //var actualRows = gridDetallesHistorialServAct.bootgrid("getCurrentRows");
                    //var archivosActual = actualRows.find(x => x.id === parseInt($(this).attr("data-index"))).archivos;
                    btnArchivoEvidencia.attr("data-trackID", $(this).attr("data-index"));
                    CargarArchivosEvidencia(parseInt($(this).attr("data-index")));
                });
            });
        }

        function CargarArchivosEvidencia(index) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarArchivosEvidenciaServicio",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ id: index }),
                success: function (response) {
                    $.unblockUI();
                    //if (response.archivos.length > 0) {

                    dtArchivosEvidencia.clear();
                    dtArchivosEvidencia.rows.add(response.archivos);
                    dtArchivosEvidencia.draw();


                    $("#title-modal-evidencia").text("Evidencia");
                    modalArchivosEvidencia.modal("show");

                    //}
                    //else { AlertaGeneral("Alerta", "No se encontraron archivos relacionados con el servicio"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);

                }
            });
        }

        function AplicarServicio(id, idMaquina, isPlaneacion) { //TODO
            //var archivos = gridArchivos.bootgrid("getCurrentRows");
            
            /*let archivos = {};
            let FechaCreacion = [];
            let nombre = [];
            let rows = gridArchivos.dataTable().fnGetNodes();
            for (let i = 0; i < rows.length; i++) {
                FechaCreacion.push($(rows[i]).find("td:eq(0)").html());
                nombre.push($(rows[i]).find("td:eq(1)").html());

                archivos.FechaCreacion = FechaCreacion[i];
                archivos.nombre = nombre[i];
            }
            console.log("1: " + archivos);
            console.log("2: " + Object.values(archivos));*/

            let archivos = dtArchivos.rows().data().toArray();
            //let archivos = "";
            console.log("archivos: " + archivos);

            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/AplicarServicioOverhaul",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ id: id, idMaquina: idMaquina, isPlaneacion: isPlaneacion, archivos: archivos, fecha: txtFechaAplicaServicio.val() }),
                //data: { id: id, idMaquina: idMaquina, isPlaneacion: isPlaneacion, archivos: archivos },
                //data: { id: id, idMaquina: idMaquina, isPlaneacion: isPlaneacion, archivos: archivos },
                success: function (response) {
                    $.unblockUI();
                    if (response.exito) {
                        AlertaGeneral("Éxito", "Se aplicó el servicio");
                        modalArchivosServicio.modal("hide");
                        modalDetallesServAct.modal("hide");
                        cargarGridServAct();
                    }
                    else { AlertaGeneral("Alerta", "No se pudo aplicar el servicio"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);

                }
            });
        }

        function DesasignarServicio(index) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/DesasignarServicioOverhaul",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ idServicio: index }),
                success: function (response) {
                    $.unblockUI();
                    if (response.exito) {
                        AlertaGeneral("Éxito", "Se desasignó el servicio");
                        modalDetallesServAct.modal("hide");
                        cargarGridServAct();
                    }
                    else { AlertaGeneral("Alerta", "No se pudo desasignar el servicio"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);

                }
            });
        }

        function CargarModalHistorialServAct(index) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarHistorialServiciosActivos",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ idServicio: index }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridDetallesHistorialServAct.bootgrid("clear");
                        gridDetallesHistorialServAct.bootgrid("append", response.servicios);
                        gridDetallesHistorialServAct.bootgrid('reload');
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

        //Modal Archivos Aplicar 
        function initGridArchivos() {
            dtArchivos = gridArchivos.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                columns: [
                    //{ data: "id", title: 'id', visible: true },
                    { data: "FechaCreacion", title: 'Fecha' },
                    { data: "nombre", title: 'Nombre' },
                    {
                        data: 'descargar',
                        title: 'Descargar',
                        render: function (data, type, row) {
                            return "<button type='button' class='btn btn-sm btn-primary descargar' data-fecha='" + row.FechaCreacionSinFormato + "' data-nombre='" + row.nombre + "' >" +
                                "<span class='glyphicon glyphicon-ok'></span></button>";
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                order: [[0, 'asc'], [1, 'asc'], [2, 'asc']],
                drawCallback: function () {
                    gridArchivos.find('button.descargar').click(function (e) {
                        descargarArchivo($(this).attr("data-nombre"), $(this).attr("data-fecha"));
                    });
                }

            });
        }

        function initGridArchivosEvidencia() {
            dtArchivosEvidencia = gridArchivosEvidencia.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    { data: 'id', title: 'ID', visible: false },
                    { data: 'FechaCreacion', title: 'Fecha' },
                    /*{
                        data: 'fecha', title: 'Fecha',
                        render: function (data, type, row) {
                            /*var fecha = row.FechaCreacion.substring(0, 2) + "/" + row.FechaCreacion.substring(2, 4) + "/" + row.FechaCreacion.substring(4, 8);
                            return "<span class='estatus'> " + fecha + " </span>";
                        }
                    },*/
                    { data: 'nombre', title: 'Nombre' },
                    {
                        data: 'descargar',
                        title: 'Descargar',
                        render: function (data, type, row) {
                            return "<button type='button' class='btn btn-sm btn-primary descargar' data-fecha='" + row.FechaCreacion + "' data-nombre='" + row.nombre + "' >" +
                                "<span class='glyphicon glyphicon-ok'></span></button>";
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                order: [[0, 'asc'], [1, 'asc'], [2, 'asc']],
                drawCallback: function () {
                    gridArchivosEvidencia.find('button.descargar').click(function (e) {
                        descargarArchivo($(this).attr("data-nombre"), $(this).attr("data-fecha"));
                    });
                }

            });
        }

        function eliminarArchivo(nombre) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/EliminarArchivoHistServ",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ nombre: nombre }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) { }
                    else { AlertaGeneral("Alerta", "No se encontró el archivo a borrar"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function descargarArchivo(nombre, fecha) {
            $.blockUI({ message: "Procesando..." });
            window.location.href = "/Overhaul/DescargarArchivoHistServ?nombre=" + nombre + "&fecha=" + fecha;
            $.unblockUI();
        }

        function SubirArchivo(e) {
            e.preventDefault();
            if (document.getElementById("inCargarArchivo").files[0] == null) {
                //$("#pathArchivo").text("  NINGÚN ARCHIVO SELECCIONADO");
                //btnSubirArchivo.prop("disabled", true);
            }
            else {
                var split = document.getElementById("inCargarArchivo").files[0].name.split('.');
                var ext = split[split.length - 1];
                ext = ext.toLowerCase();
                if (ext == 'pdf') {
                    size = document.getElementById("inCargarArchivo").files[0].size;
                    if (size > 20971520) {
                        AlertaGeneral("Alerta", "Archivo sobrepasa los 20MB");
                    }
                    else {
                        if (size <= 0) {
                            AlertaGeneral("Alerta", "Archivo vacío");
                        }
                        else {
                            guardarArchivo(e);
                        }
                    }
                }
                else {
                    AlertaGeneral("Alerta", "Sólo se aceptan archivos PDF");
                }
            }
            $("#inCargarArchivo").val("");
        }

        function SubirArchivoHistorial(e) {
            e.preventDefault();
            if (document.getElementById("inArchivoEvidencia").files[0] == null) {
                //$("#pathArchivo").text("  NINGÚN ARCHIVO SELECCIONADO");
                //btnSubirArchivo.prop("disabled", true);
            }
            else {
                var split = document.getElementById("inArchivoEvidencia").files[0].name.split('.');
                var ext = split[split.length - 1];
                ext = ext.toLowerCase();
                if (ext == 'pdf') {
                    size = document.getElementById("inArchivoEvidencia").files[0].size;
                    if (size > 20971520) {
                        AlertaGeneral("Alerta", "Archivo sobrepasa los 20MB");
                    }
                    else {
                        if (size <= 0) {
                            AlertaGeneral("Alerta", "Archivo vacío");
                        }
                        else {
                            guardarArchivoHistorial(e);
                            $("#inArchivoEvidencia").val("");
                        }
                    }
                }
                else {
                    AlertaGeneral("Alerta", "Sólo se aceptan archivos PDF");
                }
            }
        }

        function guardarArchivo(e) {
            e.preventDefault();
            $.blockUI({ message: mensajes.PROCESANDO, baseZ: 2000 });
            var formData = new FormData();
            var request = new XMLHttpRequest();
            var file = document.getElementById("inCargarArchivo").files[0];
            formData.append("archivoCRC", file);
            if (file != undefined) { $.blockUI({ message: 'Cargando archivo... Espere un momento', baseZ: 2000 }); }

            $.ajax({
                type: "POST",
                url: '/Overhaul/GuardarArchivoServHist',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    $.unblockUI();
                    CargarModalHistorialServAct(btnArchivoEvidencia.attr("data-index"));

                    //var actualRows = gridDetallesHistorialServAct.bootgrid("getCurrentRows");
                    //var archivosActual = actualRows.find(x => x.id === parseInt(btnArchivoEvidencia.attr("data-trackID"))).archivos;

                    //dtArchivos.clear();
                    dtArchivos.row.add(response.archivo);
                    dtArchivos.draw();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Se encontró un error al tratar de guardar el archivo");
                }
            });
        }

        function guardarArchivoHistorial(e) {
            e.preventDefault();
            $.blockUI({ message: mensajes.PROCESANDO, baseZ: 2000 });
            var formData = new FormData();
            var request = new XMLHttpRequest();
            var file = document.getElementById("inArchivoEvidencia").files[0];
            var trackID = parseInt(btnArchivoEvidencia.attr("data-trackID"));
            formData.append("archivoCRC", file);
            formData.append("idTrack", trackID);
            if (file != undefined) { $.blockUI({ message: 'Cargando archivo... Espere un momento', baseZ: 2000 }); }

            $.ajax({
                type: "POST",
                url: '/Overhaul/GuardarArchivoEnTrack',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.success) {
                        //$("#gridArchivosEvidencia").empty();
                        dtArchivosEvidencia.clear();
                        dtArchivosEvidencia.rows.add(response.listaArchivos);
                        dtArchivosEvidencia.draw();
                    }else{
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Se encontró un error al tratar de guardar el archivo");
                }
            });
        }

        //Modificar en masa
        function openModalModificar() {
            if (tipoUsuario < 3 || tipoUsuario == 7) {
                modalModificar.modal("show");
            }
        }

        function habilitarModificarVidaHoras() {
            if ($("#cbCicloVidaHoras").is(':checked')) {
                txtModalModificarHoraCiclo.attr("disabled", false);
            }
            else {
                txtModalModificarHoraCiclo.attr("disabled", true);
            }
        }
        function habilitarModificarGarantia() {
            if ($("#cbGarantia").is(':checked')) {
                txtModalModificarGarantia.attr("disabled", false);
            }
            else {
                txtModalModificarGarantia.attr("disabled", true);
            }
        }
        function habilitarModificarEstatus() {
            if ($("#cbEstatus").is(':checked')) {
                cboModalModificarEstatus.attr("disabled", false);
            }
            else {
                cboModalModificarEstatus.attr("disabled", true);
            }
        }

        function guardarCambiosGrupo() {
            let cicloVidaHorasLocal = "-1";
            let EstatusLocal = "-1";
            if ($("#cbCicloVidaHoras").is(':checked')) { cicloVidaHorasLocal = txtModalModificarHoraCiclo.val(); }
            if ($("#cbEstatus").is(':checked')) { EstatusLocal = cboModalModificarEstatus.val(); }

            var economico = txtFiltroEconomicoServAct.val().trim().toUpperCase();
            var servicio = txtFiltroNombreServAct.val().trim().toUpperCase();

            var cc = cboCCServAct.val();
            var grupoMaquina = cboFiltroGrupoMaquinaServAct.val();
            var modeloMaquina = cboFiltroModeloMaquinaServAct.val();
            var estatus = cboFiltroEstatusMaquinaServAct.val() == 1;

            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/guardarModificacionesServicios",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({
                    economico: economico,
                    servicio: servicio,
                    cc: cc,
                    grupoMaquina: grupoMaquina,
                    modeloMaquina: modeloMaquina,
                    estatus: estatus,

                    cicloVidaHoras: parseInt(cicloVidaHorasLocal),
                    estatusNuevo: parseInt(EstatusLocal),
                }),
                success: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Se actualizaron los datos");
                    resetModalModificar();
                    cargarGridServAct();
                    $("#btnModalModificarCancelar").click();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function resetModalModificar() {
            if ($("#cbCicloVidaHoras").is(':checked')) { $("#cbCicloVidaHoras").click(); }
            if ($("#cbGarantia").is(':checked')) { $("#cbGarantia").click(); }
            if ($("#cbEstatus").is(':checked')) { $("#cbEstatus").is(':checked').click(); }
            txtModalModificarHoraCiclo.val("0");
            txtModalModificarGarantia.val("0");
            cboModalModificarEstatus.val("1");
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.overhaul.administracionservicios = new administracionservicios();
    });
})();


