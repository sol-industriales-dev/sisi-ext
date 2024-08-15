(function () {

    $.namespace('maquinaria.overhaul.administracioncomponentes');

    administracioncomponentes = function () {
        var rowIds = [];
        tab = $.urlParam('tab');
        var confirmacion = 0;
        var autorizar = 0;
        var obra = "";
        var componenteDesecho = 0;
        let evidenciaDesecho = [];
        let trackID = 0;
        let ultimoIDCorreo = 0;
        let tipoUsuario = 6;
        const report = $('#report');
        const reportViewerModal = $('#reportViewerModal');

        //////EN MAQUINA/////////
        gridComponentes = $("#grid_Componentes"),
            
            gridComponentesCRC = $("#grid_ComponentesCRC"),
            gridComponentesAlmacen = $("#grid_ComponentesAlmacen"),
            gridComponentesInactivos = $("#grid_ComponentesInactivos"),

            gridDetalles = $("#grid_Detalles"),
            modalDetalles = $("#modalDetalles"),
            tituloModal = $("#title-modal"),
            btnNuevo = $("#btnNueva_Componente"),
            btnGuardar = $("#btnModalGuardar_Componente"),
            txtDescripcion = $("#txtDescripcion"),
            cboModalCentroCostos = $("#cboModalCentroCostos"),
            txtFiltroDescripcionEconomico = $("#txtFiltroDescripcionEconomico"),
            txtFiltroDescripcionComponentePpal = $("#txtFiltroDescripcionComponentePpal"),
            txtFiltroNoComponenteOperando = $("#txtFiltroNoComponenteOperando"),


            cboFiltroGrupoMaquina = $("#cboFiltroGrupoMaquina"),
            cboFiltroModeloMaquinaAlmacen = $("#cboFiltroModeloMaquinaAlmacen"),


            cboFiltroModeloMaquina = $("#cboFiltroModeloMaquina"),
            cboFiltroGrupoMaquinaAlmacen = $('#cboFiltroGrupoMaquinaAlmacen'),

            reporteAlmacen = $("#reporteAlmacen"),
            btnReporteAlmacen = $("#btnReporteAlmacen"),
            



            cboFiltroEstatusComponente = $("#cboFiltroEstatusComponente"),
            btnBuscar = $("#btnBuscar"),
            //cboFiltroModalEconomico = $("#cboFiltroModalEconomico"),
            txtFiltroModalSub = $("#txtFiltroModalSub"),
            txtFiltroModalComponente = $("#txtFiltroModalComponente"),
            btnBuscarModal = $("#btnBuscarModal"),
            titleModalHistorial = $("#title-modal-historial"),
            tblArchivosHistorial = $("#tblArchivosHistorial"),
            cboObraMaquina = $("#cboObraMaquina"),
            //CRC
            cboFiltroLocacionCRC = $("#cboFiltroLocacionCRC"),
            txtFiltroNoComponenteCRC = $("#txtFiltroNoComponenteCRC"),
            txtFiltroDescripcionComponenteCRC = $("#txtFiltroDescripcionComponenteCRC"),
            txtUnidadCRC = $("#txtUnidadCRC"),
            txtPlacasUnidadCRC = $("#txtPlacasUnidadCRC"),
            txtChoferCRC = $("#txtChoferCRC"),
            cboFiltroModalEconomicoCRC = $("#cboFiltroModalEconomicoCRC"),
            tituloModalCRC = $("#title-modalCRC"),
            modalDetallesCRC = $("#modalDetallesCRC"),
            txtFiltroModalComponenteCRC = $("#txtFiltroModalComponenteCRC"),
            txtFiltroModalSerieCRC = $("#txtFiltroModalSerieCRC"),
            txtFiltroCotiCRC = $("#txtFiltroCotiCRC"),
            gridDetallesCRC = $("#grid_DetallesCRC"),
            titleModalHistorialCRC = $("#title-modal-historialCRC"),
            btnBuscarCRC = $("#btnBuscarCRC"),
            btnBuscarAlmacen = $("#btnBuscarAlmacen"),
            modalFechasCRC = $("#modalFechasCRC"),
            txtModalFechasCRC = $("#txtModalFechasCRC"),
            btnGuardarModalFechas = $("#btnGuardarModalFechas"),
            cboModalAlmacenCRC = $("#cboModalAlmacenCRC"),
            txtModalFolioFacturaCRC = $("#txtModalFolioFacturaCRC"),
            btncargarFactura = $("#btncargarFactura"),
            inCargarFactura = $("#inCargarFactura"),
            txtFiltroModalMaquinaCRC = $("#txtFiltroModalMaquinaCRC"),
            cboFiltroModalEstatusCRC = $("#cboFiltroModalEstatusCRC"),
            cboFiltroModalGrupoMaquinaCRC = $("#cboFiltroModalGrupoMaquinaCRC"),
            cboFiltroModalModeloMaquinaCRC = $("#cboFiltroModalModeloMaquinaCRC"),
            btnBuscarModalCRC = $("#btnBuscarModalCRC"),
            txtModalFolioRqCRC = $("#txtModalFolioRqCRC"),
            btnRechazarOCModalFechas = $("#btnRechazarOCModalFechas"),
            tblCorreos = $("#tblCorreos"),
            modalCorreos = $("#modalCorreos"),
            agregarCorreo = $("#agregarCorreo"),
            gridDetallesHistorialCRC = $("#gridDetallesHistorialCRC"),
            txtModalCompradorCRC = $("#txtModalCompradorCRC");
        //Almacen
            cboFiltroLocacionAlmacen = $("#cboFiltroLocacionAlmacen"),
            txtFiltroDescripcionComponenteAlmacen = $("#txtFiltroDescripcionComponenteAlmacen"),
            txtFiltroNoComponenteAlmacen = $("#txtFiltroNoComponenteAlmacen"),
            cboFiltroLocacionInactivos = $("#cboFiltroLocacionInactivos"),
            txtFiltroDescripcionComponenteInactivos = $("#txtFiltroDescripcionComponenteInactivos"),
            //Inactivos
            btnBuscarInactivos = $("#btnBuscarInactivos"),
            txtFiltroNoComponenteInactivos = $("#txtFiltroNoComponenteInactivos"),

            tabOperando = $("#tabOperando"),
            tabRemociones = $("#tabRemociones"),
            tabCRC = $("#tabCRC"),
            tabAlmacen = $("#tabAlmacen"),
            tabInactivos = $("#tabInactivos"),

            btncargarArchivo = $("#btncargarArchivo"),
            inCargarArchivo = $("#inCargarArchivo"),
            btnSubirArchivo = $("#btnSubirArchivo"),
            gridArchivos = $("#gridArchivos"),
            gridArchivosVer = $("#gridArchivosVer"),
            modalIntercambioAlmacen = $("#modalIntercambioAlmacen"),
            btnCambioAlmacen = $("#btnCambioAlmacen"),
            modalDesechoAlmacen = $("#modalDesechoAlmacen"),
            btnIntercambioAlmacen = $("#btnIntercambioAlmacen"),
            gridCambioAlmacen = $("#gridCambioAlmacen"),
            btnGuardarModalIntercambioAlmacen = $("#btnGuardarModalIntercambioAlmacen"),
            cboModalIntercambioAlmacen = $("#cboModalIntercambioAlmacen"),
            txtModalIntercambioUnidadAlmacen = $("#txtModalIntercambioUnidadAlmacen"),
            txtModalIntercambioChoferAlmacen = $("#txtModalIntercambioChoferAlmacen"),
            ckAplicaNC = $("#ckAplicaNC"),
            cboModalParcialCRC = $("#cboModalParcialCRC"),
            ckIntercambioCRC = $("#ckIntercambioCRC"),
            btnObservacionesOCModalFechas = $("#btnObservacionesOCModalFechas"),

            modalReactivarInactivos = $("#modalReactivarInactivos"),
            lbReactivarInactivos = $("#lbReactivarInactivos"),
            destinoReactivarInactivos = $("#destinoReactivarInactivos"),
            fechaReactivarInactivos = $("#fechaReactivarInactivos"),
            btnReactivarInactivos = $("#btnReactivarInactivos"),
            //Almacen desecho
            btncargarEvidenciaDesecho = $("#btncargarEvidenciaDesecho"),
            inCargarEvidenciaDesecho = $("#inCargarEvidenciaDesecho"),
            gridEvidenciaDesecho = $("#gridEvidenciaDesecho"),
            btnGuardarComentarioDesecho = $("#btnGuardarComentarioDesecho"),
            btnCancelarComentarioDesecho = $("#btnCancelarComentarioDesecho"),
            txtaObsDesecho = $("#txtaObsDesecho"),
            inCargarSerieDesecho = $("#inCargarSerieDesecho"),
            btncargarSerieDesecho = $("#btncargarSerieDesecho"),
            imgDesecho = $("#imgDesecho"),
            gridSerieDesecho = $("#gridSerieDesecho"),
            reporteDesecho = $("#reporteDesecho"),
            ireporteDesecho = $("#reporteDesecho > #reportViewerModal > #report");
        ireporteAlmacen = $('#reporteAlmacen2 > #reportViewerModal > #report');
        let dtDetallesCRC;


        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Está seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        function init() {
            PermisosBotones();
            initGrid();
            initGridCRC();
            initGridAlmacen();
            initGridInactivos();
            initModalComponentesCRC();
            initGridArchivosCRC(gridArchivos);
            initGridArchivosCRC(gridArchivosVer);
            initGridCambioAlmacen();
            initTblCorreos();

            cboFiltroGrupoMaquina.select2();
            cboFiltroGrupoMaquinaAlmacen.select2({width: "100%"});           
            

            cboObraMaquina.select2();

            cboFiltroModeloMaquina.select2();
            cboFiltroModeloMaquinaAlmacen.select2({width: "100%"});



            cboFiltroGrupoMaquina.fillCombo('/Overhaul/FillCboGrupoMaquinaComponentes', { obj: 0 });
            cboFiltroGrupoMaquinaAlmacen.fillCombo('/Overhaul/FillCboGrupoMaquinaComponentes', { obj: 0 });


            cboFiltroModalGrupoMaquinaCRC.fillCombo('/Overhaul/FillCboGrupoMaquinaComponentes', { obj: 0 });
            cboFiltroModalModeloMaquinaCRC.fillCombo('/Overhaul/fillCboModelo');
            cboFiltroModalEstatusCRC.fillCombo('/Overhaul/FillCboEstatusComponentesCRC');
            cboObraMaquina.fillCombo('/Overhaul/FillCboObraMaquina');
            cboFiltroLocacionCRC.fillCombo('/Overhaul/FillCboLocacion', { tipoLocacion: 2 }, true);
            convertToMultiselectSelectAll(cboFiltroLocacionCRC);
            cboFiltroLocacionCRC.multiselect('selectAll', false).multiselect('updateButtonText');
            cboFiltroLocacionAlmacen.fillCombo('/Overhaul/FillCboLocacion', { tipoLocacion: 1 }, true);
            convertToMultiselectSelectAll(cboFiltroLocacionAlmacen);
            cboFiltroLocacionAlmacen.multiselect('selectAll', false).multiselect('updateButtonText');
            cboFiltroLocacionInactivos.fillCombo('/Overhaul/FillCboLocacion', { tipoLocacion: 3 }, true);
            convertToMultiselectSelectAll(cboFiltroLocacionInactivos);
            cboFiltroLocacionInactivos.multiselect('selectAll', false).multiselect('updateButtonText');

            //Operando            
            //tabOperando.click(cargarGrid);
            tabOperando.click();
            tabCRC.click(cargarGridCRC);
            tabAlmacen.click(cargarGridAlmacen);
            //tabInactivos.click(cargarGridInactivos);

            cboFiltroModeloMaquina = $("#cboFiltroModeloMaquina"),
            cboFiltroGrupoMaquina.change(cargarcboFiltroModeloMaquina);

            cboFiltroModeloMaquinaAlmacen = $("#cboFiltroModeloMaquinaAlmacen"),
            cboFiltroGrupoMaquinaAlmacen.change(cargarcboFiltroModeloMaquinaAlmacen);


            //cboFiltroGrupoMaquinaAlmacen.change(cargarcboFiltroModeloMaquina);

            //cboFiltroGrupoMaquina.change(cargarGrid);
            //cboFiltroModeloMaquina.change(cargarGrid);
            //txtFiltroDescripcionEconomico.change(cargarGrid);
            //txtFiltroDescripcionComponentePpal.change(cargarGrid);
            //cboObraMaquina.change(cargarGrid);
            btnBuscar.click(cargarGrid);
            btnBuscarModal.click(cargarModalComponentes);
            txtFiltroModalComponente.change(cargarModalComponentes);
            txtFiltroModalSub.change(cargarModalComponentes);
            IniciarTblArchivosHistorial();
            //CRC
            btnBuscarCRC.click(cargarGridCRC);
            //cboFiltroLocacionCRC.change(cargarGridCRC);
            //txtFiltroNoComponenteCRC.change(cargarGridCRC);
            //txtFiltroDescripcionComponenteCRC.change(cargarGridCRC);
            //cboFiltroModalEconomicoCRC.change(cargarModalComponentesCRC);
            //txtFiltroModalComponenteCRC.change(cargarModalComponentesCRC);
            //txtFiltroModalMaquinaCRC.change(cargarModalComponentesCRC);
            //cboFiltroModalEstatusCRC.change(cargarModalComponentesCRC);
            cboFiltroModalGrupoMaquinaCRC.change(function (e) {
                cboFiltroModalModeloMaquinaCRC.fillCombo('/Overhaul/fillCboModelo', { idGrupo: cboFiltroModalGrupoMaquinaCRC.val() });
            });
            btnBuscarModalCRC.click(cargarModalComponentesCRC);
            btnRechazarOCModalFechas.click(initRechazarOC);
            btncargarFactura.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                inCargarFactura.click();
            });
            inCargarFactura.change(function (e) {
                e.stopPropagation();
                e.stopImmediatePropagation();
                btncargarFactura.text(inCargarFactura[0].files[0].name);
                ValidarArchivoFactura(e, 1, "inCargarFactura");
            });
            modalDetallesCRC.on('hidden.bs.modal', function () {
                cargarGridCRC();
            })
            //$("#btnAdminCorreos").click(function (e) {
            //    e.preventDefault();
            //    e.stopPropagation();
            //    e.stopImmediatePropagation();
            //    $("#txtCorreo").val('');
            //    $("#modalCorreos").modal("show");
            //});
            agregarCorreo.click(AgregarCorreo);
            ckIntercambioCRC.change(HabilitarDatosEnvio);
            cboModalAlmacenCRC.change(CargarCorreosCRCEnvio);


            $("#cboCorreoCRC").select2({
                tags: true,
                createTag: function (params) {
                    if (!validateEmail(params.term)) {
                        return null;
                    }
                    return {
                        id: params.term,
                        text: params.term
                    }
                }
            });

            //Almacen
            btnBuscarAlmacen.click(cargarGridAlmacen);
            //cboFiltroLocacionAlmacen.change(cargarGridAlmacen);
            //txtFiltroDescripcionComponenteAlmacen.change(cargarGridAlmacen);
            //txtFiltroNoComponenteAlmacen.change(cargarGridAlmacen);
            btnGuardarComentarioDesecho.click(ValidarReporteDesecho);
            $("#txtEntradaAlmacen").datepicker().datepicker("setDate", new Date());
            btncargarEvidenciaDesecho.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                inCargarEvidenciaDesecho.click();
            });


            btnReporteAlmacen.click(cargarReporteAlmcen);
            $("#reporteAlmacen2 > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteAlmacen2 > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteAlmacen2 > #reportViewerModal").css("width", "0%");
                $("#reporteAlmacen2 > #reportViewerModal").css("height", "0%");
            });

            

            inCargarEvidenciaDesecho.change(function (e) {
                e.stopPropagation();
                e.stopImmediatePropagation();
                SubirEvidenciaDesecho(e, 2, "inCargarEvidenciaDesecho");
            });
            btncargarSerieDesecho.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                inCargarSerieDesecho.click();
            });
            inCargarSerieDesecho.change(function (e) {
                e.stopPropagation();
                e.stopImmediatePropagation();
                leerURL(this, imgDesecho);
            });
            $("#btnAdminCorreosAlmacen").click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                $("#txtCorreo").val('');
                $("#modalCorreos").modal("show");
            });
            $("#reporteDesecho > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteDesecho > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteDesecho > #reportViewerModal").css("width", "0%");
                $("#reporteDesecho > #reportViewerModal").css("height", "0%");
            });
            cboModalIntercambioAlmacen.change(CargarCorreosAlmacenInter);
            $("#btnGuardarEntrada").click(GuardarEntradaAlmacen);
            //Inactivos
            btnBuscarInactivos.click(cargarGridInactivos);
            fechaReactivarInactivos.datepicker().datepicker("setDate", new Date());
            destinoReactivarInactivos.fillCombo("/Overhaul/FillCboLocacion", { tipoLocacion: 1 });

            btnReactivarInactivos.click(ReactivarComponente);

            //--// Autocompletado subconjunto
            txtFiltroDescripcionComponentePpal.getAutocomplete(SelectSubconjuntoPrincipal, null, '/Overhaul/getSubConjuntos');
            txtFiltroDescripcionComponenteCRC.getAutocomplete(SelectSubconjuntoCRC, null, '/Overhaul/getSubConjuntos');
            txtFiltroDescripcionComponenteAlmacen.getAutocomplete(SelectSubconjuntoAlmacen, null, '/Overhaul/getSubConjuntos');
            txtFiltroDescripcionComponenteInactivos.getAutocomplete(SelectSubconjuntoInactivos, null, '/Overhaul/getSubConjuntos');
            txtFiltroModalComponenteCRC.getAutocomplete(SelectSubconjuntoModalCRC, null, 'getSubConjuntos');
            txtFiltroModalSub.getAutocomplete(SelectSubModalPrin, null, 'getSubConjuntos');
            //--// Autocompletado serie
            txtFiltroNoComponenteOperando.getAutocomplete(SelectNoComponentePrincipal, null, '/Overhaul/getNoComponente');
            txtFiltroModalComponente.getAutocomplete(SelectNoComponenteModal, null, '/Overhaul/getNoComponente');
            txtFiltroNoComponenteCRC.getAutocomplete(SelectNoComponenteCRC, null, '/Overhaul/getNoComponente');
            txtFiltroNoComponenteAlmacen.getAutocomplete(SelectNoComponenteAlmacen, null, '/Overhaul/getNoComponente');
            txtFiltroNoComponenteInactivos.getAutocomplete(SelectNoComponenteInactivos, null, '/Overhaul/getNoComponente');
            txtFiltroModalSerieCRC.getAutocomplete(SelectNoComponenteModalCRC, null, '/Overhaul/getNoComponente');
            //--// Autocompletado economico
            txtFiltroDescripcionEconomico.getAutocomplete(SelectEconomicoPrincipal, null, '/Overhaul/getEconomico');
            //--// Autocompletado chofer
            txtChoferCRC.getAutocomplete(SelectChoferCRC, null, '/Overhaul/FillCboChoferesIntercambioAlmacen');
            txtModalIntercambioChoferAlmacen.getAutocomplete(SelectChoferAlmacen, null, '/Overhaul/FillCboChoferesIntercambioAlmacen');
            //--// Autocompletado unidadTransporte
            txtUnidadCRC.getAutocomplete(SelectUnidadCRC, null, '/Overhaul/FillCboUnidadesIntercambioAlmacen');
            txtModalIntercambioUnidadAlmacen.getAutocomplete(SelectUnidadAlmacen, null, '/Overhaul/FillCboUnidadesIntercambioAlmacen');
            //--// Autocompletado comprador
            txtModalCompradorCRC.getAutocomplete(SelectCompradorCRC, null, '/Overhaul/FillTxtComprador');

            txtModalFechasCRC.datepicker().datepicker("setDate", new Date());
            btnGuardarModalFechas.click(guardarFecha);
            cboModalAlmacenCRC.fillCombo('/Overhaul/FillCboLocacionByListaTipo', { tipoLocaciones: [1, 2] });

            btncargarArchivo.click(function (e) {
                e.preventDefault();
                inCargarArchivo.click();
            });
            inCargarArchivo.change(function (e) {
                SubirArchivoCRC(e);
            });
            btnCambioAlmacen.click(initCambioAlmacen);
            btnIntercambioAlmacen.click(initCambioAlmacenIntercambio);
            btnGuardarModalIntercambioAlmacen.click(function (e) {
                cambioAlmacen(e);
            });
            $(document).on('click', "#btnModalEliminar", function (e) {
                e.preventDefault();
                if ($("#ulNuevo .active a").text() != "Remociones" && $("#ulNuevo .active a").text() != "Inventario") {
                    if (confirmacion == 0) { EnviarComponenteAlmacenRechazo(); }
                    else {
                        if (confirmacion == 1) { ValidarDesecho(btnBuscarAlmacen.attr("data-reporteID"), btnBuscarAlmacen.attr("data-componenteID")); }
                        else { CambioAlmacenIntercambio(); }
                    }
                }
            });
            $(document).on('click', "#btnCancelar", function (e) {
                e.preventDefault();
                if ($("#ulNuevo .active a").text() != "Remociones" && $("#ulNuevo .active a").text() != "Inventario") {
                    if (confirmacion == 0) { RechazarOC(); }
                }
            });
            $(document).on('click', "#btnModalBoton1", function (e) {
                e.preventDefault();
                EnviarComponenteAlmacenRechazo();
                enviarCorreoRechazo($("#txtaObsOCModalFechas").val().trim(), btnRechazarOCModalFechas.attr("data-clave-cotizacion"));
            });
            $(document).on('click', "#btnModalBoton2", function (e) {
                e.preventDefault();
                RechazarOC();
            });

            btnObservacionesOCModalFechas.click(EnviarCorreoObservaciones);

            if (tab == 1) { tabRemociones.click(); }           

        }
        //autocompletado subconjunto
        function SelectSubconjuntoModalCRC(event, ui) { txtFiltroModalComponenteCRC.text(ui.item.descripcion); }
        function SelectSubconjuntoPrincipal(eventppal, uippal) { txtFiltroDescripcionComponentePpal.text(uippal.item.descripcion); }
        function SelectSubconjuntoCRC(event, ui) { txtFiltroDescripcionComponenteCRC.text(ui.item.descripcion); }
        function SelectSubconjuntoAlmacen(event, ui) { txtFiltroDescripcionComponenteAlmacen.text(ui.item.descripcion); }
        function SelectSubconjuntoInactivos(event, ui) { txtFiltroDescripcionComponenteInactivos.text(ui.item.descripcion); }
        function SelectSubModalPrin(event, ui) { txtFiltroModalSub.text(ui.item.descripcion); }
        //autocompletado noComponente
        function SelectNoComponentePrincipal(event, ui) { txtFiltroNoComponenteOperando.text(ui.item.noComponente); }
        function SelectNoComponenteModal(event, ui) { txtFiltroModalComponente.text(ui.item.noComponente); }
        function SelectNoComponenteCRC(event, ui) { txtFiltroNoComponenteCRC.text(ui.item.noComponente); }
        function SelectNoComponenteModalCRC(event, ui) { txtFiltroModalSerieCRC.text(ui.item.noComponente); }
        function SelectNoComponenteAlmacen(event, ui) { txtFiltroNoComponenteAlmacen.text(ui.item.noComponente); }
        function SelectNoComponenteInactivos(event, ui) { txtFiltroNoComponenteInactivos.text(ui.item.noComponente); }
        //autocompletado Economico
        function SelectEconomicoPrincipal(event, ui) { txtFiltroDescripcionEconomico.text(ui.item.noComponente); }
        //autocompletado Chofer
        function SelectChoferCRC(event, ui) { txtChoferCRC.text(ui.item.Text); }
        function SelectChoferAlmacen(event, ui) { txtModalIntercambioChoferAlmacen.text(ui.item.Text); }
        //autocompletado UnidadTransporte
        function SelectUnidadCRC(event, ui) { txtUnidadCRC.text(ui.item.value); txtPlacasUnidadCRC.val(ui.item.id); }
        function SelectUnidadAlmacen(event, ui) { txtModalIntercambioUnidadAlmacen.text(ui.item.noEconomico); txtModalIntercambioUnidadAlmacen.attr("data-index", ui.item.id); }
        //autocompletado Comprador
        function SelectCompradorCRC(event, ui) { txtModalCompradorCRC.text(ui.item.Text); }

        function cargarcboFiltroModeloMaquina() {
            if (cboFiltroGrupoMaquina.val() != "") {
                cboFiltroModeloMaquina.prop("disabled", false);
                cboFiltroModeloMaquina.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: cboFiltroGrupoMaquina.val() });
            }
            else {
                cboFiltroModeloMaquina.val("");
                cboFiltroModeloMaquina.prop("disabled", true);
            }
        }


        function cargarcboFiltroModeloMaquinaAlmacen() {
            if (cboFiltroGrupoMaquinaAlmacen.val() != "") {
                cboFiltroModeloMaquinaAlmacen.prop("disabled", false);
                cboFiltroModeloMaquinaAlmacen.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: cboFiltroGrupoMaquinaAlmacen.val() });
            }
            else {
                cboFiltroModeloMaquinaAlmacen.val("");
                cboFiltroModeloMaquinaAlmacen.prop("disabled", true);
            }
        }


        function initGrid() {
            gridComponentes.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {

                    "CCName": function (column, row) {
                        return "<span class=\"CCName\"> " + row.CCName + " </span>"
                    },
                    "detalle": function (column, row) {
                        return "<button type='button' class='btn btn-primary detalle' data-index='" + row.id + "' data-cc='" + row.cc + "' data-economico = '" + row.economico + "'>" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                            " </button>";
                    },
                    "estatus": function (column, row) {
                        var color;
                        var html = "<div style='text-align:left;'>";
                        for (var i = 0; i < row.listaComponentes.length; i++) {
                            var color;
                            if (row.listaComponentes[i].restaEstatus > 1000) { color = "#009900"; }
                            else {
                                if (row.listaComponentes[i].restaEstatus > 0) { color = "#ffcc00"; }
                                else { color = "#ff6600"; }
                            }
                            if (row.listaComponentes[i].falla == true) { color = "#ff0000"; }
                            var nombreCorto = row.listaComponentes[i].nombreCorto != "" ? row.listaComponentes[i].nombreCorto : row.listaComponentes[i].descripcion.substring(0, 1);
                            html += "<span class='btn dot dotClick' data-noComponente='" + row.listaComponentes[i].noComponente + "' data-economico='" + row.economico + "' data-maquinaID='" + row.id
                                + "' data-toggle='tooltip' title='" + row.listaComponentes[i].descripcion + "<br/>" + row.listaComponentes[i].noComponente + "<br/>" + row.listaComponentes[i].horaCicloActual
                                + "' data-placement='bottom' data-html='true' style='background-color:" + color + "; text-align:center'><b>" + nombreCorto + "</b></span>";
                        }
                        html += "</div>";
                        return html;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                gridComponentes.find(".detalle").parent().css("text-align", "center");
                gridComponentes.find(".detalle").parent().css("width", "3%");
                $('[data-toggle="tooltip"]').tooltip();
                gridComponentes.find(".detalle").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    resetModal();
                    txtFiltroModalComponente.val(txtFiltroNoComponenteOperando.val().trim());
                    txtFiltroModalSub.val(txtFiltroDescripcionComponentePpal.val().trim());
                    btnBuscarModal.attr("data-economico", $(this).attr("data-economico"));
                    btnBuscarModal.attr("data-maquinaID", $(this).attr("data-index"));
                    cargarModalComponentes();
                });
                gridComponentes.find(".dotClick").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    resetModal();
                    txtFiltroModalComponente.val($(this).attr("data-noComponente"));
                    txtFiltroModalSub.val(txtFiltroDescripcionComponentePpal.val().trim());
                    btnBuscarModal.attr("data-economico", $(this).attr("data-economico"));
                    btnBuscarModal.attr("data-maquinaID", $(this).attr("data-maquinaID"));
                    cargarModalComponentes();
                });
            });
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
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function cargarGridHistorialComponente(idComponente, grid) {
            $.blockUI({ baseZ: 2000, message: 'Procesando...' });
            $.ajax({
                url: "/Overhaul/ModalComponentesHistorial",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ idComponente: idComponente }),
                success: function (response) {
                    $.unblockUI();
                    grid.bootgrid({
                        templates: {
                            header: ""
                        },
                        rowCount: -1,
                        sorting: false,
                        formatters: {

                            "reciclado": function (column, row) {
                                return '<span class="reciclado ' + (!row.reciclado ? '' : 'glyphicon glyphicon-ok') + '"> </span>';
                            },
                            "archivos": function (column, row) {
                                return "<button type='button' class='btn btn-warning archivos' data-trackID='" + row.id + "'><span class='fa fa-file-pdf'></span>  </button>"
                            }
                        }
                    }).on("loaded.rs.jquery.bootgrid", function () {
                        /* Executes after data is loaded and rendered */
                        grid.find(".archivos").parent().css("text-align", "center");
                        grid.find(".archivos").parent().css("width", "3%");
                        grid.find(".archivos").on("click", function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            trackID = $(this).attr("data-trackID");
                            cargarTblArchivosHistorial($(this).attr("data-trackID"), tblArchivosHistorial);
                            $("#modalArchivoHistorial").modal("show");
                        });
                    });

                    grid.bootgrid("clear");
                    grid.bootgrid("append", response.historial);
                    grid.bootgrid('reload');
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function IniciarTblArchivosHistorial() {
            tblArchivosHistorial.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "fecha": function (column, row) {
                        var fecha = row.FechaCreacion.substring(0, 2) + "/" + row.FechaCreacion.substring(2, 4) + "/" + row.FechaCreacion.substring(4, 8);
                        return "<span class='estatus'> " + fecha + " </span>";
                    },
                    "descargar": function (column, row) {
                        return "<button type='button' class='btn btn-primary descargar' data-index='" + row.id + "' >" +
                            "<span class='glyphicon glyphicon-ok'></span></button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                tblArchivosHistorial.find(".eliminar").parent().css("text-align", "center");
                tblArchivosHistorial.find(".eliminar").parent().css("width", "3%");
                tblArchivosHistorial.find(".descargar").parent().css("text-align", "center");
                tblArchivosHistorial.find(".descargar").parent().css("width", "3%");
                tblArchivosHistorial.find(".descargar").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    descargarArchivoHistorial($(this).attr("data-index"));
                });
            });
        }

        function descargarArchivoHistorial(idArchivo) {
            window.location.href = "/Overhaul/DescargarArchivoCRC?idTrack=" + trackID + "&idArchivo=" + idArchivo;
        }

        function cargarTblArchivosHistorial(idTrack, tabla) {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/cargarGridArchivosCRC",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idTrack: idTrack }),
                success: function (response) {
                    $.unblockUI();
                    tabla.bootgrid("clear");
                    tabla.bootgrid("append", response.archivos);
                    tabla.bootgrid('reload');
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        function cargarModalComponentes() {
            var componenteBusqueda = txtFiltroModalComponente.val().trim();
            var maquinaID = btnBuscarModal.attr("data-maquinaID");
            var maquina = btnBuscarModal.attr("data-economico");
            var subconjunto = txtFiltroModalSub.val().trim();
            if (maquina == "") { maquina = "-1"; }
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/ModalComponentes",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ subconjunto: subconjunto, componenteBusqueda: componenteBusqueda, maquinaID: maquinaID, maquina: maquina, tipoLocacion: 0 }),
                success: function (response) {
                    $.unblockUI();
                    gridDetalles.bootgrid({
                        templates: {
                            header: ""
                        },
                        sorting: false,
                        rowCount: -1,
                        formatters: {
                            "locacion": function (column, row) {
                                return "<span class='locacion'> " + row.maquina + " </span>"
                            },
                            "estatus": function (column, row) {
                                var resta = row.cicloVidaHoras - row.horasCicloActual;
                                var color;
                                if (resta > 1000) { color = "#009900"; }
                                else {
                                    if (resta > 0) { color = "#ffcc00"; }
                                    else { color = "#ff6600"; }
                                }
                                if (row.falla == true) { color = "#ff0000"; }
                                return "<span class='dot' style='background-color:" + color + "'></span>";
                            },
                            "detalles": function (column, row) {
                                return "<button type='button' class='btn btn-primary detalles' data-index-componente='" + row.idComponente + "' data-noComponente='" + row.noComponente + "' data-proveedor='" + row.proveedor + "' data-ordenCompra='" + row.ordenCompra + "'>" +
                                    "<span class='glyphicon glyphicon-eye-open'></span>  </button>";
                            },
                            "remocion": function (column, row) {
                                //var resta = row.cicloVidaHoras - row.horasCicloActual;
                                return "<button type='button' class='btn btn-danger remocion' data-maquinaID='" + row.maquinariaID + "' data-index-componente='" + row.idComponente + "' data-noComponente='" + row.noComponente +
                                    "' " + ((tipoUsuario > 3 && tipoUsuario < 6) ? "disabled" : "") + "><span class='fa fa-wrench'></span>  </button>";
                            }
                        }
                    }).on("loaded.rs.jquery.bootgrid", function () {
                        gridDetalles.find(".detalles").parent().css("text-align", "center");
                        gridDetalles.find(".detalles").parent().css("width", "3%");
                        gridDetalles.find(".remocion").parent().css("text-align", "center");
                        gridDetalles.find(".remocion").parent().css("width", "3%");
                        gridDetalles.find(".detalles").on("click", function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            cargarGridHistorialComponente($(this).attr("data-index-componente"), $("#gridDetallesHistorial"));
                            var proveedor = $(this).attr("data-proveedor");
                            var ordenCompra = $(this).attr("data-ordenCompra");
                            if (proveedor != "" && proveedor != null) $("#spProveedorHistorial").text($(this).attr("data-proveedor"));
                            else $("#spProveedorHistorial").text("Sin Especificar");
                            if (ordenCompra != "" && ordenCompra != null) $("#spOrdenCompraHistorial").text($(this).attr("data-ordenCompra"));
                            else $("#spOrdenCompraHistorial").text("Sin Especificar");
                            $("#lgHistorial").text($(this).attr("data-noComponente"));
                            titleModalHistorial.text("Historial de componente");
                            openModalComponentes();
                        });

                        gridDetalles.find(".remocion").on("click", function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            if (tipoUsuario <= 3 || tipoUsuario >= 6) {
                                var exito = CheckTallerRemocion($(this).attr("data-index-componente"), $(this).attr("data-maquinaID"));
                            }
                        });
                    });
                    gridDetalles.bootgrid("clear");
                    gridDetalles.bootgrid("append", response.componentes);
                    gridDetalles.bootgrid('reload');
                    openModal();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function cargarGrid() {
            var grupo = cboFiltroGrupoMaquina.val();
            var grupoAlmacen = cboFiltroGrupoMaquinaAlmacen.val();
            var modelo = cboFiltroModeloMaquina.val();
            var modeloAlmacen = cboFiltroModeloMaquinaAlmacen.val();
            var descripcion = txtFiltroDescripcionComponentePpal.val().trim();
            var noComponente = txtFiltroNoComponenteOperando.val();

            if (cboFiltroGrupoMaquina.val() == null || cboFiltroGrupoMaquina.val() == "")
                grupo = -1;
            if (cboFiltroModeloMaquina.val() == null || cboFiltroModeloMaquina.val() == "")
                modelo = -1;

            if (cboFiltroGrupoMaquinaAlmacen.val() == null || cboFiltroGrupoMaquinaAlmacen.val() == "")
                grupoAlmacen = -1;
            if (cboFiltroModeloMaquinaAlmacen.val() == null || cboFiltroModeloMaquinaAlmacen.val() == "")
                modeloAlmacen = -1;
            //if (txtFiltroDescripcionComponentePpal.val().trim() == "")
            //    descripcion = -1
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarMaquinaria",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ grupo: grupo, modelo: modelo, economicoBusqueda: txtFiltroDescripcionEconomico.val().trim(), descripcionComponente: descripcion, obra: cboObraMaquina.val(), noComponente: noComponente }),
                success: function (response) {

                    if (response.success) {
                        gridComponentes.bootgrid({
                            rowCount: -1,
                            templates: {
                                header: ""
                            }
                        });
                        gridComponentes.bootgrid("clear");
                        gridComponentes.bootgrid("append", response.maquinas);
                        gridComponentes.bootgrid('reload');
                        $.unblockUI();
                    }
                    else {
                        $.unblockUI();
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }

                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);

                }
            });
        }

        function openModal() {
            tituloModal.text("Detalles");
            //reset();
            modalDetalles.modal('show');
        }
        function openModalComponentes() {
            $("#title-modal-componente").text("Detalles");
            $("#modalDetallesComponente").modal('show');
        }

        function resetModal() {
            txtFiltroModalComponente.val("");
            txtFiltroModalSub.val("");
            //cboFiltroModalEconomico.val("");
        }

        function CheckTallerRemocion(index, maquinaID) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CheckTallerRemocion",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ componenteID: index, maquinaID: maquinaID }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        if (response.exito) { window.location.href = "/Overhaul/Remocion?id=" + index; }
                        else { AlertaGeneral("Alerta", "No existe un proceso de taller activo para ese componente"); }
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

        ///////////////////////CRC//////////////////
        function initGridCRC() {
            gridComponentesCRC.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {

                    //"CCName": function (column, row) {
                    //    return "<span class=\"CCName\"> " + row.CCName + " </span>"
                    //},
                    "detalle": function (column, row) {
                        return "<button type='button' class='btn btn-primary detalle' data-index='" + row.id + "' data-locacion = '" + row.locacion + "'>" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                            " </button>";
                    },
                    "estatus": function (column, row) {
                        var html = "<div style='text-align:left;'>";
                        for (var i = 0; i < row.listaComponentes.length; i++) {
                            var color;
                            if (row.listaComponentes[i].diasEnLocacion > 29) { color = "#ff0000"; }
                            else {
                                if (row.listaComponentes[i].diasEnLocacion > 20) { color = "#ffcc00"; }
                                else { color = "#009900"; }
                            }
                            var nombreCorto = row.listaComponentes[i].nombreCorto != "" ? row.listaComponentes[i].nombreCorto : row.listaComponentes[i].descripcion.substring(0, 1);
                            html += "<span class='btn dot dotClick' style='background-color:" + color + ";' data-toggle='tooltip' title='" + row.listaComponentes[i].descripcion + "<br/>"
                                + row.listaComponentes[i].noComponente + "<br/>" + row.listaComponentes[i].horaCicloActual + "' data-placement='bottom' data-html='true' style='' data-noEconomico='"
                                + row.listaComponentes[i].noComponente + "' data-index='" + row.id + "'><b>" + nombreCorto + "</b></span>";
                        }
                        html += "</div>";
                        return html;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                gridComponentesCRC.find(".detalle").parent().css("text-align", "center");
                gridComponentesCRC.find(".detalle").parent().css("width", "3%");
                $('[data-toggle="tooltip"]').tooltip();
                gridComponentesCRC.find(".detalle").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    resetModalCRC();
                    cboFiltroModalEconomicoCRC.fillCombo('/Overhaul/FillCboLocacion', { tipoLocacion: 2 });
                    cboFiltroModalEconomicoCRC.val($(this).attr("data-index"));
                    cargarModalComponentesCRC();

                });

                gridComponentesCRC.find(".dotClick").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    resetModalCRC();
                    cboFiltroModalEconomicoCRC.fillCombo('/Overhaul/FillCboLocacion', { tipoLocacion: 2 });
                    cboFiltroModalEconomicoCRC.val($(this).attr("data-index"));
                    txtFiltroModalSerieCRC.val($(this).attr("data-noEconomico"))
                    cargarModalComponentesCRC();
                });
            });
        }
        function openModalCRC() {

            tituloModalCRC.text("Detalles");
            cboFiltroModalEstatusCRC.val("");
            cboFiltroModalEstatusCRC.change();
            txtFiltroModalComponenteCRC.val("");
            txtFiltroModalMaquinaCRC.val("");
            cboFiltroModalGrupoMaquinaCRC.val("");
            //txtFiltroModalSerieCRC.val("");
            txtFiltroCotiCRC.val("");
            cboFiltroModalGrupoMaquinaCRC.change();
            modalDetallesCRC.modal('show');
        }

        function resetModalCRC() {
            cboFiltroModalEconomicoCRC.val("");
            txtFiltroModalSerieCRC.val("");
        }

        function initModalComponentesCRC() {
            var usuario = -1;
            $.ajax({ url: "/Overhaul/getUsuarioRealiza", async: false, type: 'POST', success: function (response) { usuario = response.usuario; } });
            switch (usuario) {
                case -1: autorizar = 0; break;
                case 13: autorizar = 1; break;
                case 6: autorizar = 1; break;
                case 3292: autorizar = 1; break;
                case 8006: autorizar = 1; break;
                case 1230: autorizar = 1; break;
                case 79515: autorizar = 1; break;
                default: autorizar = 0; break;
            }

            dtDetallesCRC = gridDetallesCRC.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                ordering: false,
                dom: 't<i>',
                fixedHeader: {
                    header: true,
                    footer: true
                },
                initComplete: function (settings, json) {

                },
                columns: [
                    { 
                        data: 'estatus', 
                        title: 'Estatus', 
                        render: function (data, type, row) { return "<span class='estatus'> " + row.estatusDescripcion + " </span>"; } 
                    },
                    { 
                        data: 'contadorDias', 
                        title: 'Días', 
                        render: function (data, type, row) 
                        {
                            var color;
                            if (row.contadorDias > 29) { color = "#ff0000"; }
                            else {
                                if (row.contadorDias > 20) { color = "#ffcc00"; }
                                else { color = "#009900"; }
                            }
                            return "<span class='estatus'><span class='dot' style='background-color:" + color + ";width:30px;height:30px;' >" + row.contadorDias + "</span></span>";
                        }
                    },
                    {
                        data: 'noEconomico',
                        title: 'Econ.',
                        render: function (data, type, row) { return "<span class='noEconomico'> " + row.datosMaquina.Value + " </span>"; }
                    },
                    {
                        data: 'noComponente',
                        title: 'Componente',
                        render: function (data, type, row) {
                            return row.subConjunto + "<br/>" + "<button type='button' class='btn btn-primary detalles' data-index-componente='" + row.componenteID +
                                "' data-noComponente='" + row.noComponente + "' data-proveedor='" + row.proveedor + "' data-ordenCompra='" + row.ordenCompra + "'>" +
                                row.noComponente + "</button>";
                        }
                    },
                    {
                        data: 'horasCiclo',
                        title: 'HrsCiclo<br/>HrsAcum.',
                        render: function (data, type, row) { return "<span class='fecha'><strong>" + row.horasCiclo + "</strong></span><br> <SPAN STYLE='text-decoration:overline;'>" + row.horasAcumuladas + " </span>"; }
                    },
                    {
                        data: 'fechaEnvio',
                        title: 'Envío',
                        render: function (data, type, row) { return "<span class='fecha'> " + (row.fechas.fechaEnvio == null ? "" : "&nbsp;" + row.fechas.fechaEnvio + "&nbsp;") + " </span>"; }
                    },                    
                    {
                        data: 'fechaRecepcion',
                        title: 'Recep.',
                        render: function (data, type, row) {
                            var html = "<span class='fecha'> " + (row.fechas.fechaRecepcion == null ? "" : "&nbsp;" + row.fechas.fechaRecepcion + "&nbsp;") + " </span>";
                            if (row.estatus == 2 && (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7)) {
                                html +=
                                    "<button type='button' class='btn btn-xs btn-primary modalFechas' data-index='" + row.id + "' data-estatus='" + row.estatus + "' " + ((autorizar == 1 || autorizar == 2) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-right'></span>  </button>";
                            }
                            return html;
                        }
                    },
                    {
                        data: 'fechaCotizacion',
                        title: 'Coti.',
                        render: function (data, type, row) {
                            var html = "<span class='fecha'> " + (row.fechas.fechaCotizacion == null ? "" : "&nbsp;" + row.fechas.fechaCotizacion + "&nbsp;") + " </span>";
                            if (row.estatus == 4 && (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7)) {
                                html +=
                                    "<span>  </span><button type='button' class='btn btn-xs btn-danger modalRegresar' data-index='" + row.id + "' data-estatus='" + row.estatus + "' data-fechaCotizacion='" + row.fechas.fechaCotizacion + "' " + ((autorizar == 1 || autorizar == 2) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-left'></span>  </button>" +
                                    "<span>  </span><button type='button' class='btn btn-xs btn-primary modalFechas' data-index='" + row.id + "' data-estatus='" + row.estatus + "' data-fechaCotizacion='" + row.fechas.fechaCotizacion + "' " + ((autorizar == 1 || autorizar == 2) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-right'></span>  </button>";
                            }
                            return html;
                        }
                    },
                    {
                        data: 'claveCotizacion',
                        title: 'Clave Cotización',
                        render: function (data, type, row) { return "<span class='claveCotizacion'><b> " + (row.fechas.claveCotizacion == null ? "" : "&nbsp;" + row.fechas.claveCotizacion + "&nbsp;") + " </b></span>"; }
                    },
                    {
                        data: 'costo',
                        title: 'Costo<br/>(US)',
                        render: function (data, type, row) { return "<span class='costo'> " + (row.fechas.costo == null ? "" : "&nbsp;$" + row.fechas.costo + "&nbsp;") + " </span>"; }
                    },
                    {
                        data: 'parcial',
                        title: 'Parcial/<br/>General',
                        render: function (data, type, row) { return "<span class='costo'> " + (row.fechas.parcial == "true" ? " Parcial " : (row.fechas.parcial == "false" ? " General " : "")) + " </span>"; }
                    },
                    {
                        data: 'fechaAutorizacion',
                        title: 'Autori.',
                        render: function (data, type, row) {
                            var html = "<span class='fecha'> " + (row.fechas.fechaAutorizacion == null ? "" : "&nbsp;" + row.fechas.fechaAutorizacion + "&nbsp;") + " </span>";
                            if (row.estatus == 5 && (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 2 || tipoUsuario == 6 || tipoUsuario == 7)) {
                                html +=
                                    "<span>  </span><button type='button' class='btn btn-xs btn-danger modalRegresar' data-index='" + row.id + "' data-estatus='" + row.estatus + "' data-clave-cotizacion='" + row.fechas.claveCotizacion + "' " + ((autorizar == 1 || autorizar == 2 || autorizar == 3) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 2 || tipoUsuario == 6 || tipoUsuario == 7 || autorizar == 3) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-left'></span>  </button>" +
                                    "<span>  </span><button type='button' class='btn btn-xs btn-primary modalFechas' data-index='" + row.id + "' data-estatus='" + row.estatus + "' data-clave-cotizacion='" + row.fechas.claveCotizacion + "' " + ((autorizar == 1 || autorizar == 3) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 2 || tipoUsuario == 6 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-right'></span>  </button>";

                            }
                            return html;
                        }
                    },
                    {
                        data: 'fechaRequisicion',
                        title: 'RQ',
                        render: function (data, type, row) {
                            var html = "<span class='fecha'> " + (row.fechas.fechaRequisicion == null ? "" : "&nbsp;" + row.fechas.fechaRequisicion + "&nbsp;") + " </span>";
                            if (row.estatus == 6 && (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7)) {
                                html +=
                                    "<span>  </span><button type='button' class='btn btn-xs btn-danger modalRegresar' data-index='" + row.id + "' data-estatus='" + row.estatus + "' " + ((autorizar == 1 || autorizar == 2) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-left'></span>  </button>" +
                                    "<span>  </span><button type='button' class='btn btn-xs btn-primary modalFechas' data-index='" + row.id + "' data-estatus='" + row.estatus + "' " + ((autorizar == 1 || autorizar == 2) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-right'></span>  </button>";
                            }
                            return html;
                        }
                    },
                    {
                        data: 'folioRequisicion',
                        title: '&nbsp;&nbsp;Folio RQ&nbsp;&nbsp;',
                        render: function (data, type, row) { return "<span class='folioRQ'><b> " + (row.fechas.folioRequisicion == null ? "" : "&nbsp;" + row.fechas.folioRequisicion + "&nbsp;") + " </b></span>"; }
                    },
                    {
                        data: 'fechaEnvioOC',
                        title: 'Recol.<br/>OC',
                        render: function (data, type, row) {
                            var html = "<span class='fecha'> " + (row.fechas.fechaEnvioOC == null ? "" : "&nbsp;" + row.fechas.fechaEnvioOC + "&nbsp;") + " </span>";
                            if (row.estatus == 7 && (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7)) {
                                html +=
                                    "<span>  </span><button type='button' class='btn btn-xs btn-danger modalRegresar' data-index='" + row.id + "' data-estatus='" + row.estatus + "' " + ((autorizar == 1 || autorizar == 2) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-left'></span>  </button>" +
                                    "<span>  </span><button type='button' class='btn btn-xs btn-primary modalFechas' data-index='" + row.id + "' data-estatus='" + row.estatus + "' " + ((autorizar == 1 || autorizar == 2) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-right'></span>  </button>";
                            }
                            return html;
                        }
                    },
                    {
                        data: 'OC',
                        title: '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;OC&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;',
                        render: function (data, type, row) { return "<span class='OC'><b> " + (row.fechas.OC == null ? "" : "&nbsp;" + row.fechas.OC + "&nbsp;") + " </b></span>"; }
                    },
                    {
                        data: 'comprador',
                        title: '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Comprador&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;',
                        render: function (data, type, row) { return "<span class='OC'><b> " + (row.fechas.comprador == null ? "" : "&nbsp;" + row.fechas.comprador + "&nbsp;") + " </b></span>"; }
                    },
                    {
                        data: 'fechaTerminacion',
                        title: 'Termin.',
                        render: function (data, type, row) {
                            var html = "<span class='fecha'> " + (row.fechas.fechaTerminacion == null ? "" : "&nbsp;" + row.fechas.fechaTerminacion + "&nbsp;") + " </span>";
                            if (row.estatus == 8 && (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7)) {
                                html +=
                                    "<span>  </span><button type='button' class='btn btn-xs btn-danger modalRegresar' data-index='" + row.id + "' data-estatus='" + row.estatus + "' " + ((autorizar == 1 || autorizar == 2) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-left'></span>  </button>" +
                                    "<span>  </span><button type='button' class='btn btn-xs btn-primary modalFechas' data-index='" + row.id + "' data-estatus='" + row.estatus + "' " + ((autorizar == 1 || autorizar == 2) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-right'></span>  </button>";
                            }
                            return html;
                        }
                    },
                    {
                        data: 'fechaRecoleccion',
                        title: 'Envío',
                        render: function (data, type, row) {
                            var html = "<span class='fecha'> " + (row.fechas.fechaRecoleccion == null ? "" : "&nbsp;" + row.fechas.fechaRecoleccion + "&nbsp;") + " </span>";
                            if (row.estatus == 9 && (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7)) {
                                html +=
                                    "<span>  </span><button type='button' class='btn btn-xs btn-danger modalRegresar' data-index='" + row.id + "' data-estatus='" + row.estatus + "' " + ((autorizar == 1 || autorizar == 2) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-left'></span>  </button>" +
                                    "<span>  </span><button type='button' class='btn btn-xs btn-primary modalFechas' data-index='" + row.id + "' data-estatus='" + row.estatus + "' " + ((autorizar == 1 || autorizar == 2) ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + ">" +
                                    "<span class='glyphicon glyphicon-arrow-right'></span>  </button>";
                            }
                            return html;
                        }
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function () {
                    gridDetallesCRC.find(".modalFechas").on("click", function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btnGuardarModalFechas.attr("data-index", $(this).attr("data-index"));
                        btnGuardarModalFechas.attr("data-estatus", $(this).attr("data-estatus"));
                        txtModalFechasCRC.datepicker("setDate", new Date());
                        txtModalFechasCRC.prop("disabled", false);
                        if (ckAplicaNC.prop('checked')) { ckAplicaNC.click(); }
                        if (ckIntercambioCRC.prop('checked')) { ckIntercambioCRC.click(); }

                        $("#txtModalClaveCotizacionCRC").parent().css("display", "none");
                        $("#txtModalCostoCRC").parent().css("display", "none");
                        $("#txtModalOrdenCompraCRC").parent().parent().css("display", "none");
                        $("#txtModalCompradorCRC").parent().parent().css("display", "none");
                        $("#cboModalAlmacenCRC").parent().css("display", "none");
                        txtModalFolioFacturaCRC.parent().parent().css("display", "none");
                        txtModalFolioRqCRC.parent().parent().css("display", "none");
                        $("#txtaObsOCModalFechas").parent().css("display", "none");
                        btnRechazarOCModalFechas.css("display", "none");
                        btncargarArchivo.parent().parent().parent().parent().parent().css("display", "none");
                        gridArchivosVer.parent().parent().css("display", "none");
                        btnObservacionesOCModalFechas.css("display", "none");
                        ckAplicaNC.parent().parent().css("display", "none");
                        $("#cboCorreoCRC").parent().css("display", "none");
                        cboModalParcialCRC.parent().css("display", "none");
                        ckIntercambioCRC.parent().css("display", "none");

                        $("#txtModalClaveCotizacionCRC").val("");
                        $("#txtModalCostoCRC").val("");
                        $("#txtModalOrdenCompraCRC").val("");
                        $("#txtModalCompradorCRC").val("");
                        $("#cboModalAlmacenCRC").val("");
                        $("#cboModalAlmacenCRC").change();
                        txtChoferCRC.val("");
                        txtUnidadCRC.val("");
                        txtPlacasUnidadCRC.val("");
                        cboModalParcialCRC.val("0");
                        //txtUnidadCRC.attr("data-index", "");

                        txtModalFolioFacturaCRC.val("");
                        inCargarFactura.val(null);
                        txtModalFolioRqCRC.val("");
                        $("#txtaObsOCModalFechas").val("");
                        btncargarArchivo.val("");
                        inCargarArchivo.val(null);
                        $("#pathArchivo").text("  NINGÚN ARCHIVO SELECCIONADO");
                        btnGuardarModalFechas.html("<span class='glyphicon glyphicon-floppy-disk'></span> Guardar");
                        //btnSubirArchivo.prop("disabled", true);
                        switch ($(this).attr("data-estatus")) {
                            case "4":
                                if ($(this).attr("data-fechaCotizacion") != "null" && $(this).attr("data-fechaCotizacion") != "") {
                                    txtModalFechasCRC.val($(this).attr("data-fechaCotizacion"));
                                    txtModalFechasCRC.prop("disabled", true);
                                }
                                $("#txtModalClaveCotizacionCRC").parent().css("display", "inline-table");
                                $("#txtModalCostoCRC").parent().css("display", "inline-table");
                                cargarGridArchivosCRC($(this).attr("data-index"), gridArchivos);
                                btncargarArchivo.parent().parent().parent().parent().parent().css("display", "inline-table");
                                btncargarArchivo.attr("data-index", $(this).attr("data-index"));
                                cboModalParcialCRC.parent().css("display", "inline-table");
                                break;
                            case "5":
                                var listaCorreos = [1, 1, 0, 0, 1];
                                //CargartablaCorreos(tblCorreos, listaCorreos, [cboFiltroModalEconomicoCRC.val()]);
                                CargarCorreosOverhaul("cboCorreoCRC", listaCorreos, [cboFiltroModalEconomicoCRC.val()]);
                                $("#txtaObsOCModalFechas").parent().css("display", "inline-table");
                                cargarGridArchivosCRC($(this).attr("data-index"), gridArchivosVer);
                                gridArchivosVer.parent().parent().css("display", "inline-table");
                                ckAplicaNC.parent().parent().css("display", "contents");
                                $("#cboCorreoCRC").parent().css("display", "inline-table");

                                btncargarArchivo.attr("data-index", $(this).attr("data-index"));
                                btnRechazarOCModalFechas.css("display", "inline-table");
                                btnRechazarOCModalFechas.attr("data-index", $(this).attr("data-index"));
                                btnRechazarOCModalFechas.attr("data-estatus", $(this).attr("data-estatus"));
                                btnRechazarOCModalFechas.attr("data-clave-cotizacion", $(this).attr("data-clave-cotizacion"));
                                btnObservacionesOCModalFechas.css("display", "inline-table");
                                btnGuardarModalFechas.html("<span class='glyphicon glyphicon-floppy-disk'></span> Aprobar");
                                break;
                            case "6":
                                txtModalFolioRqCRC.parent().parent().css("display", "inline-table");
                                break;
                            case "7":
                                $("#txtModalOrdenCompraCRC").parent().parent().css("display", "inline-table");
                                $("#txtModalCompradorCRC").parent().parent().css("display", "inline-table");
                                break;
                            case "9":
                                var listaCorreos = [1, 1, 0, 0, 1];
                                //CargartablaCorreos(tblCorreos, listaCorreos, ["1012"]);
                                CargarCorreosOverhaul("cboCorreoCRC", listaCorreos, ["1012"]);
                                $("#cboModalAlmacenCRC").parent().css("display", "inline-table");
                                ckIntercambioCRC.parent().css("display", "inline-table");
                                txtModalFolioFacturaCRC.parent().parent().css("display", "inline-table");
                                cboModalAlmacenCRC.val("1012");
                                inCargarFactura.value = '';
                                $("#cboCorreoCRC").parent().css("display", "inline-table");
                                btncargarFactura.text("SELECCIONAR ARCHIVO");
                                break;
                            default:
                                break;
                        }
                        openModalFechasCRC();
                    });
                    gridDetallesCRC.find(".modalRegresar").on("click", function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        regresarEstadoCRC($(this).attr("data-estatus"), $(this).attr("data-index"));
                    });
                    gridDetallesCRC.find(".detalles").on("click", function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        cargarGridHistorialComponente($(this).attr("data-index-componente"), $("#gridDetallesHistorialCRC"));
                        var proveedor = $(this).attr("data-proveedor");
                        var ordenCompra = $(this).attr("data-ordenCompra");
                        if (proveedor != "" && proveedor != null) $("#spProveedorHistorialCRC").text($(this).attr("data-proveedor"));
                        else $("#spProveedorHistorialCRC").text("Sin Especificar");
                        if (ordenCompra != "" && ordenCompra != null) $("#spOrdenCompraHistorialCRC").text($(this).attr("data-ordenCompra"));
                        else $("#spOrdenCompraHistorialCRC").text("Sin Especificar");
                        $("#lgHistorialCRC").text($(this).attr("data-noComponente"));
                        titleModalHistorial.text("Historial componente");
                        openModalComponentesCRC();
                    });
                }
            });

        }

        function cargarGridArchivosCRC(idTrack, grid) {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/cargarGridArchivosCRC",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ idTrack: idTrack }),
                success: function (response) {
                    $.unblockUI();
                    grid.bootgrid("clear");
                    grid.bootgrid("append", response.archivos);
                    grid.bootgrid('reload');
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function cargarModalComponentesCRC() {
            $.blockUI({ message: mensajes.PROCESANDO, baseZ: 2000 });
            $.ajax({
                url: "/Overhaul/ModalComponentesCRC",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({
                    estatus: cboFiltroModalEstatusCRC.val(),
                    subconjunto: txtFiltroModalComponenteCRC.val().trim(),
                    locacion: cboFiltroModalEconomicoCRC.val(),
                    tipoLocacion: 2,
                    maquina: txtFiltroModalMaquinaCRC.val().trim(),
                    grupoMaquina: cboFiltroModalGrupoMaquinaCRC.val(),
                    noComponente: txtFiltroModalSerieCRC.val().trim(),
                    clvCotizacion: txtFiltroCotiCRC.val().trim(),
                    modeloMaquina: cboFiltroModalModeloMaquinaCRC.val()
                }),
                success: function (response) {
                    AddRows(gridDetallesCRC, response.componentes);

                    if (!modalDetallesCRC.hasClass('in')) openModalCRC();
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function regresarEstadoCRC(estado, idTrack) {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/RegresarEstadoCRC",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ estado: estado, idTrack: idTrack }),
                success: function (response) {
                    //$.unblockUI();
                    cargarModalComponentesCRC();
                    ConfirmacionGeneral("Confirmación", "Se eliminaron los datos", "bg-green");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function openModalComponentesCRC() {
            $("#title-modal-componenteCRC").text("Detalles");
            $("#modalDetallesComponenteCRC").modal('show');
        }
        function openModalFechasCRC() {
            $("#title-modal-fechasCRC").text("Introduzca los datos");
            modalFechasCRC.modal('show');
        }

        function cargarGridCRC() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarMaquinariaCRC",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ noComponente: txtFiltroNoComponenteCRC.val(), locacionBusqueda: cboFiltroLocacionCRC.val(), descripcionComponente: txtFiltroDescripcionComponenteCRC.val().trim(), estatus: 2, obra: "" }),
                success: function (response) {
                    if (response.success) {
                        gridComponentesCRC.bootgrid({
                            rowCount: -1,
                            templates: {
                                header: ""
                            }
                        });
                        gridComponentesCRC.bootgrid("clear");
                        gridComponentesCRC.bootgrid("append", response.locaciones);
                        gridComponentesCRC.bootgrid('reload');
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);

                }
            });
        }

        function guardarFecha() {

            var json = "";
            var varValidacion = true;
            var intercambio = false;
            switch ($(this).attr("data-estatus")) {
                case "4":
                    if ($("#txtModalClaveCotizacionCRC").val().trim() == "" || $("#txtModalCostoCRC").val().trim() == "") { varValidacion = false; }
                    else {
                        json = "{claveCotizacion: '" + $("#txtModalClaveCotizacionCRC").val().trim().toUpperCase() + "', costo: '" + $("#txtModalCostoCRC").val().trim().toUpperCase() + "', parcial: '" + (cboModalParcialCRC.val() == "1") + "'}";
                        $("#txtModalClaveCotizacionCRC").parent().css("display", "none");
                        $("#txtModalCostoCRC").parent().css("display", "none");
                    }
                    break;
                case "5":
                    json = "{notaCredito: '" + ckAplicaNC.prop('checked') + "'}";
                    break;
                case "6":
                    if (txtModalFolioRqCRC.val().trim() == "") { varValidacion = false; }
                    else {
                        json = "{folioRequisicion: '" + txtModalFolioRqCRC.val().trim().toUpperCase() + "'}";
                        txtModalFolioRqCRC.parent().parent().css("display", "none");
                    }
                    break;
                case "7":
                    if ($("#txtModalOrdenCompraCRC").val().trim() == "" || $("#txtModalCompradorCRC").val().trim() == "") { varValidacion = false; }
                    else {
                        json = "{ordenCompra: '" + $("#txtModalOrdenCompraCRC").val().trim().toUpperCase() + "', comprador: '" + $("#txtModalCompradorCRC").val().trim().toUpperCase() + "'}";
                        $("#txtModalOrdenCompraCRC").parent().parent().css("display", "none");
                        $("#txtModalCompradorCRC").parent().parent().css("display", "none");
                    }
                    break;
                case "9":
                    if ($("#cboModalAlmacenCRC").val().trim() == "" || txtModalFolioFacturaCRC.val().trim() == "" || /*inCargarFactura[0].files.length == 0 ||*/
                        (!ckIntercambioCRC.prop('checked') && (txtUnidadCRC.val().trim() == "" || txtChoferCRC.val().trim() == ""))) {
                        varValidacion = false;
                    }
                    else {
                        json = "{almacen: '" + $("#cboModalAlmacenCRC").val() + "' , folioFactura: '" + txtModalFolioFacturaCRC.val().trim() + "' , tipoLocacion: '" + $("#cboModalAlmacenCRC option:selected").attr("data-prefijo") + "'}";
                        $("#cboModalAlmacenCRC").parent().css("display", "none");
                    }
                    intercambio = ckIntercambioCRC.prop('checked');
                    break;
                default:
                    break;
            }
            if (varValidacion) {
                $.blockUI({ message: mensajes.PROCESANDO, baseZ: 2000 });
                $.ajax({
                    url: "/Overhaul/GuardarFechasCRC",
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: JSON.stringify({ idtrack: btnGuardarModalFechas.attr("data-index"), fecha: txtModalFechasCRC.val(), estatus: btnGuardarModalFechas.attr("data-estatus"), intercambio: intercambio, datosExtra: json }),
                    success: function (response) {
                        cargarModalComponentesCRC();
                        modalFechasCRC.modal('hide');
                        if ($(this).attr("data-estatus") == "5") {
                            enviarCorreoAprobado($("#txtaObsOCModalFechas").val().trim(), btnRechazarOCModalFechas.attr("data-clave-cotizacion"));
                        }
                        ConfirmacionGeneral("Confirmación", "Se guardaron los datos", "bg-green");
                        if (btnGuardarModalFechas.attr("data-estatus") == "9")
                            EnviarCorreoTraspaso();
                        //cargarGridCRC();
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);
                    }
                });
                if ($(this).attr("data-estatus") == "5") {
                    //if (ckAplicaNC.prop('checked')) {
                    //    window.location.href = "/Overhaul/NotasCredito";
                    //}
                    enviarCorreoAprobado($("#txtaObsOCModalFechas").val().trim(), btnRechazarOCModalFechas.attr("data-clave-cotizacion"));
                }
            }
            else {
                AlertaGeneral("Alerta", "Se requieren todos los datos del formulario");
            }
        }

        function HabilitarDatosEnvio() {
            if (ckIntercambioCRC.prop('checked')) {
                txtUnidadCRC.prop("disabled", true);
                txtChoferCRC.prop("disabled", true);
                txtPlacasUnidadCRC.prop("disabled", true);
            }
            else {
                txtUnidadCRC.prop("disabled", false);
                txtChoferCRC.prop("disabled", false);
                txtPlacasUnidadCRC.prop("disabled", false);
            }
        }

        function CargartablaCorreos(tabla, listaCorreos, locacionesID) {
            $.ajax({
                url: '/Overhaul/GetCorreosOverhaul',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ locacionesID: locacionesID }),
                success: function (response) {
                    tabla.bootgrid("clear");
                    //var JSONTotal = [];
                    let j = 1;
                    //JSONTotal.push([{ "id": 0, "correo": response.correosPpales[0] }]);
                    //tabla.bootgrid("append", JSONINFO);
                    for (i = 0; i < listaCorreos.length; i++, j++) {
                        if (listaCorreos[i] == 1) {
                            var JSONINFO = [{ "id": (i + 1), "correo": response.correosPpales[i] }];
                            //JSONTotal.push(JSONINFO);
                            tabla.bootgrid("append", JSONINFO);
                        }
                    }
                    for (i = 0; i < response.correosLocacion.length; i++, j++) {
                        var JSONINFO = [{ "id": j, "correo": response.correosLocacion[i] }];
                        //JSONTotal.push(JSONINFO);
                        tabla.bootgrid("append", JSONINFO);

                    }
                    ultimoIDCorreo = j + 1;
                    //tabla.bootgrid("clear");
                    //tabla.bootgrid("append", JSONTotal);
                    tabla.bootgrid('reload');
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function CargarCorreosOverhaul(select, listaCorreos, locacionesID) {
            $("#" + select + "").fillCombo('/Overhaul/FillCboCorreosOverhaul', { locacionesID: locacionesID, listaCorreos: listaCorreos }, true);
            var selectedItems = [];
            var allOptions = $("#" + select + " option");
            allOptions.each(function () {
                selectedItems.push($(this).val());
            });
            $("#" + select + "").val(selectedItems).trigger("change");
            //$("#cboCorreoCRC").next().css("display", "none");
        }

        function initTblCorreos() {
            ultimoIDCorreo = 0;
            tblCorreos.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: { header: "" },
                sorting: false,
                formatters: { "eliminar": function (column, row) { return "<button type='button' class='btn btn-sm btn-danger eliminar'><span class='glyphicon glyphicon-remove'></span></button>"; } }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblCorreos.find(".eliminar").parent().css("text-align", "center");
                tblCorreos.find(".eliminar").parent().css("width", "3%");
                tblCorreos.find(".eliminar").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    var rowID = parseInt($(this).parent().parent().attr('data-row-id'));
                    tblCorreos.bootgrid("remove", [rowID]);
                });
            });
        }

        function AgregarCorreo() {
            let correo = $("#txtCorreo").val().trim();
            if (correo != "" && validateEmail(correo)) {
                var JSONINFO = [{ "id": ultimoIDCorreo, "correo": correo }];
                tblCorreos.bootgrid("append", JSONINFO);
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

        function EnviarCorreoTraspaso() {
            var formData = new FormData();
            var request = new XMLHttpRequest();
            var file = document.getElementById("inCargarFactura").files[0];
            let correos = $("#cboCorreoCRC").val();
            //$('#tblCorreos tbody tr').each(function () {
            //    correos.push($(this).find('td:eq(0)').text());
            //});
            var intercambioActual = ckIntercambioCRC.prop('checked');
            formData.append("archivoFactura", file);
            formData.append("idTrack", JSON.stringify(btnGuardarModalFechas.attr("data-index")));
            formData.append("correos", JSON.stringify(correos));
            formData.append("unidad", txtUnidadCRC.val().trim());
            formData.append("placas", txtPlacasUnidadCRC.val().trim());
            formData.append("chofer", txtChoferCRC.val().trim());
            formData.append("intercambio", JSON.stringify(intercambioActual));
            formData.append("almacen", $("#cboModalAlmacenCRC option:selected").text());
            $.blockUI({ message: mensajes.PROCESANDO, baseZ: 2000 });
            $.ajax({
                type: "POST",
                url: '/Overhaul/EnviarCorreoTraspaso',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    //$.unblockUI();
                    //cargarGridCRC();
                    cargarModalComponentesCRC();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function initRechazarOC() {
            confirmacion = 0;
            ConfirmacionEliminacionCustom("Desecho", "¿Desea rechazar la cotización para recoger el componente o para subir una nueva cotización?", "Recoger Componente", "Subir Nueva Cotización");
        }

        function ValidarArchivoFactura(e, tipo, input) {
            e.preventDefault();
            if (document.getElementById(input).files[0] != null) {
                var ext = document.getElementById(input).files[0].name.match(/\.(.+)$/)[1];
                ext = ext.toLowerCase();
                if (tipo == 1 ? ext == "pdf" : ($.inArray(ext, ["jpg", "jpeg", "png"]) != -1)) {
                    size = document.getElementById(input).files[0].size;
                    if (size > 20971520) {
                        AlertaGeneral("Alerta", "Archivo sobrepasa los 20MB");
                    }
                    else {
                        if (size <= 0) {
                            AlertaGeneral("Alerta", "Archivo vacío");
                        }
                        else {
                            return true;
                        }
                    }
                }
                else {
                    if (tipo == 1) { AlertaGeneral("Alerta", "Sólo se aceptan archivos PDF"); }
                    else { AlertaGeneral("Alerta", "Sólo se aceptan archivos tipo imagen"); }
                }
            }
            return false;
        }

        function RechazarOC() {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/RechazarOC",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idTrack: btnRechazarOCModalFechas.attr("data-index") }),
                success: function (response) {
                    cargarModalComponentesCRC();
                    //$.unblockUI();
                    modalFechasCRC.modal('hide');
                    ConfirmacionGeneral("Confirmación", "Se eliminaron los datos", "bg-green");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function EnviarComponenteAlmacenRechazo() {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/EnviarComponenteAlmacenRechazo",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idTrack: btnRechazarOCModalFechas.attr("data-index"), fecha: txtModalFechasCRC.val() }),
                success: function (response) {
                    cargarModalComponentesCRC();
                    //$.unblockUI();
                    modalFechasCRC.modal('hide');
                    ConfirmacionGeneral("Confirmación", "El componente se encuentra en espera de recolección", "bg-green");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function enviarCorreoRechazo(observaciones, claveCotizacion) {
            $.blockUI({
                message: 'Enviando correo de rechazo...',
                baseZ: 2000
            });
            let correos = $("#cboCorreoCRC").val();
            let mail = $.post("/Overhaul/enviarCorreoCotizacion", { observaciones: observaciones, claveCotizacion: claveCotizacion, idTrack: btnRechazarOCModalFechas.attr("data-index"), correos: correos, tipo: 1 });
            mail.done(function (correo) {
                if (correo.success) {
                    //ConfirmacionGeneral("Confirmación", "Envío correcto", "bg-green");
                }
            });
            mail.always(function (a) {
                $.unblockUI();
            });
        }

        function enviarCorreoAprobado(observaciones, claveCotizacion) {
            $.blockUI({
                message: 'Enviando correo de aprobación...',
                baseZ: 2000
            });
            let correos = $("#cboCorreoCRC").val();
            const mail = $.post("/Overhaul/enviarCorreoCotizacion", { observaciones: observaciones, claveCotizacion: claveCotizacion, idtrack: btnGuardarModalFechas.attr("data-index"), correos: correos, tipo: 0 });
            mail.done(function (correo) {
                if (correo.success) {
                    //ConfirmacionGeneral("Confirmación", "Envío correcto", "bg-green");
                }
            });
            mail.always(function (a) {
                $.unblockUI();
            });
        }

        function EnviarCorreoObservaciones() {
            $.blockUI({
                message: 'Enviando correo de observaciones...',
                baseZ: 2000
            });
            let correos = $("#cboCorreoCRC").val();
            let mail = $.post("/Overhaul/enviarCorreoCotizacion", { observaciones: $("#txtaObsOCModalFechas").val().trim(), claveCotizacion: btnRechazarOCModalFechas.attr("data-clave-cotizacion"), idtrack: btnGuardarModalFechas.attr("data-index"), correos: correos, tipo: 2 });
            mail.done(function (correo) {
                if (correo.success) {
                    modalFechasCRC.modal('hide');
                    //ConfirmacionGeneral("Confirmación", "Envío correcto", "bg-green");
                }
            });
            mail.always(function (a) {
                $.unblockUI();
            });
        }


        function initGridArchivosCRC(grid) {
            grid.bootgrid({
                headerCssClass: '.bg-table-header',
                navigation: 1,
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "fecha": function (column, row) {
                        var fecha = row.FechaCreacion.substring(0, 2) + "/" + row.FechaCreacion.substring(2, 4) + "/" + row.FechaCreacion.substring(4, 8);
                        return "<span class='estatus'> " + fecha + " </span>";
                    },
                    "eliminar": function (column, row) {
                        return "<button type='button' class='btn btn-sm btn-danger eliminar' data-index='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-remove'></span></button>";
                    },
                    "descargar": function (column, row) {
                        return "<button type='button' class='btn btn-sm btn-primary descargar' data-index='" + row.id + "' >" +
                            "<span class='glyphicon glyphicon-ok'></span></button>";
                    },
                    "nombre": function (column, row) {
                        return "<span class='wrap nombre'>" + row.nombre + "</span>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                grid.find(".eliminar").parent().css("text-align", "center");
                grid.find(".eliminar").parent().css("width", "3%");
                grid.find(".descargar").parent().css("text-align", "center");
                grid.find(".descargar").parent().css("width", "3%");
                grid.find(".nombre").parent().css("text-align", "center");
                grid.find(".nombre").parent().css("width", "50%");
                grid.find(".eliminar").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    eliminarArchivoCRC($(this).attr("data-index"));
                    cargarGridArchivosCRC(btncargarArchivo.attr("data-index"), gridArchivos);
                });
                grid.find(".descargar").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    descargarArchivoCRC($(this).attr("data-index"));
                });
            });
        }

        function guardarArchivoCRC(e) {
            e.preventDefault();
            $.blockUI({ message: mensajes.PROCESANDO, baseZ: 2000 });
            var formData = new FormData();
            var request = new XMLHttpRequest();
            var file = document.getElementById("inCargarArchivo").files[0];
            formData.append("archivoCRC", file);
            formData.append("idTrack", btncargarArchivo.attr("data-index"));
            if (file != undefined) { $.blockUI({ message: 'Cargando archivo... Espere un momento', baseZ: 2000 }); }
            $.ajax({
                type: "POST",
                url: '/Overhaul/GuardarArchivoCRC',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    $.unblockUI();
                    cargarGridArchivosCRC(btncargarArchivo.attr("data-index"), gridArchivos);
                    //$("#pathArchivo").text("  NINGÚN ARCHIVO SELECCIONADO");
                    //btnSubirArchivo.prop("disabled", true);
                    //ConfirmacionGeneral("Éxito", "Se agregó un nuevo archivo.", "bg-green");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Se encontró un error al tratar de guardar el archivo");
                }
            });
        }

        function descargarArchivoCRC(idArchivo) {
            window.location.href = "/Overhaul/DescargarArchivoCRC?idTrack=" + btncargarArchivo.attr("data-index") + "&idArchivo=" + idArchivo;
        }

        function eliminarArchivoCRC(idArchivo) {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/DeleteArchivoCRC",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ idTrack: btncargarArchivo.attr("data-index"), idArchivo: idArchivo }),
                success: function (response) {
                    $.unblockUI();
                    ConfirmacionGeneral("Éxito", "Se eliminó el archivo.", "bg-green");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Se encontró un error al tratar de eliminar el archivo");
                }
            });
        }

        function SubirArchivoCRC(e) {
            e.preventDefault();
            if (document.getElementById("inCargarArchivo").files[0] == null) {
                //$("#pathArchivo").text("  NINGÚN ARCHIVO SELECCIONADO");
                //btnSubirArchivo.prop("disabled", true);
            }
            else {
                var ext = document.getElementById("inCargarArchivo").files[0].name.match(/\.(.+)$/)[1];
                ext = ext.toLowerCase();
                if (ext == 'pdf') {
                    size = document.getElementById("inCargarArchivo").files[0].size;
                    if (size > 20971520) {
                        AlertaGeneral("Alerta", "Archivo sobrepasa los 20MB");
                        //inCargarArchivo.val("");
                    }
                    else {
                        if (size <= 0) {
                            AlertaGeneral("Alerta", "Archivo vacío");
                            //inCargarArchivo.val("");
                        }
                        else {
                            //$("#pathArchivo").text("  \"" + e.target.files[0].name + "\"");
                            //btnSubirArchivo.prop("disabled", false);
                            guardarArchivoCRC(e);
                        }
                    }
                }
                else {
                    AlertaGeneral("Alerta", "Sólo se aceptan archivos PDF");
                }
            }
        }

        function CargarCorreosCRCEnvio() {
            var listaCorreos = [1, 1, 0, 0, 1];
            //CargartablaCorreos(tblCorreos, listaCorreos, [cboModalAlmacenCRC.val()]);
            CargarCorreosOverhaul("cboCorreoCRC", listaCorreos, [cboModalAlmacenCRC.val()]);
        }

        ///////////////////////Almacen//////////////////

        function initGridAlmacen() {
            gridComponentesAlmacen.bootgrid({
                headerCssClass: '.bg-table-header',
                selection: true,
                multiSelect: true,
                align: "center",
                headerAlign: "center",
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "detalle": function (column, row) {
                        return "<button type='button' class='btn btn-primary detalle' data-index='" + row.id + "' data-obra = '" + row.obra + "' data-noComponente='" + row.noComponente + "'  data-proveedor='" + row.proveedor + "' data-ordenCompra='" + row.ordenCompra + "'>" +
                            "<span class='glyphicon glyphicon-eye-open'></span>  </button>";
                    },
                    "desecho": function (column, row) {
                        html = "";
                        if ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7 || tipoUsuario == 3) && row.reporteDesecho == -1) {
                            html += "<button type='button' class='btn btn-success desecho' data-index='" + row.id + "' data-obra = '" + row.obra + "' data-noComponente='" + row.noComponente + "' >" +
                                "<span class='glyphicon glyphicon-trash'></span>  </button>";
                        }
                        if ((tipoUsuario < 4 || tipoUsuario == 6 || tipoUsuario == 7) && row.reporteDesecho != -1) {
                            html += "<button type='button' class='btn btn-success verReporte' data-index='" + row.id + "' data-obra = '" + row.obra + "' data-noComponente='" +
                                row.noComponente + "' data-reporteID='" + row.reporteDesecho + "' ><span class='glyphicon glyphicon-eye-open'></span>  </button>";
                        }
                        if ((tipoUsuario == 2 || tipoUsuario == 1 || tipoUsuario == 6 || tipoUsuario == 7) && row.reporteDesecho != -1) {
                            html += "<button type='button' class='btn btn-success autorizacionDesecho' data-index='" + row.id + "' data-obra = '" + row.obra + "' data-noComponente='" + row.noComponente + "' data-reporteID='" + row.reporteDesecho + "' >" +
                                "<span class='glyphicon glyphicon-ok'></span>  </button>";
                        }
                        if ((tipoUsuario < 4 || tipoUsuario == 6 || tipoUsuario == 7) && row.reporteDesecho != -1) {
                            html += "<button type='button' class='btn btn-danger eliminar' data-index='" + row.id + "' data-obra = '" + row.obra + "' data-noComponente='" + row.noComponente + "' data-reporteID='" + row.reporteDesecho + "' >" +
                                "<span class='glyphicon glyphicon-remove'></span>  </button>";
                        }
                        return html;
                    },
                    "dias": function (column, row) {
                        if (row.dias < 0) { return "<span class='dias'>-</span>"; }
                        else { return "<span class='dias'>" + row.dias + "</span>"; }
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                gridComponentesAlmacen.find("td").parent().css("text-align", "center");
                gridComponentesAlmacen.find(".detalle").parent().css("text-align", "center");
                gridComponentesAlmacen.find(".detalle").parent().css("width", "3%");
                gridComponentesAlmacen.find(".desecho").parent().css("text-align", "center");

                $('[data-toggle="tooltip"]').tooltip();
                gridComponentesAlmacen.find(".detalle").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    cargarGridHistorialComponente($(this).attr("data-index"), $("#gridDetallesHistorialAlmacen"));
                    var proveedor = $(this).attr("data-proveedor");
                    var ordenCompra = $(this).attr("data-ordenCompra");
                    if (proveedor != "" && proveedor != null) $("#spProveedorHistorialAlmacen").text($(this).attr("data-proveedor"));
                    else $("#spProveedorHistorialAlmacen").text("Sin Especificar");
                    if (ordenCompra != "" && ordenCompra != null) $("#spOrdenCompraHistorialAlmacen").text($(this).attr("data-ordenCompra"));
                    else $("#spOrdenCompraHistorialAlmacen").text("Sin Especificar");
                    $("#lgHistorialAlmacen").text($(this).attr("data-noComponente"));
                    $("#title-modal-historialAlmacen").text("Historial de componente");
                    openModalComponentesAlmacen();
                });
                gridComponentesAlmacen.find(".desecho").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    obra = $(this).attr("data-obra");
                    componenteDesecho = parseInt($(this).attr("data-index"));
                    initGridEvidenciaDesecho();
                    initGridSerieDesecho();
                    evidenciaDesecho = [];
                    inCargarSerieDesecho.value = "";
                    txtaObsDesecho.val('');
                    imgDesecho.attr('src', '');
                    modalDesechoAlmacen.modal("show");
                });
                gridComponentesAlmacen.find(".autorizacionDesecho").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    confirmacion = 1;
                    btnBuscarAlmacen.attr("data-reporteID", $(this).attr("data-reporteID"));
                    btnBuscarAlmacen.attr("data-componenteID", $(this).attr("data-index"));
                    ConfirmacionEliminacion("Desecho", "¿Está seguro que desea enviar el componente a desecho?");
                });
                gridComponentesAlmacen.find(".verReporte").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    abrirReporteDesecho($(this).attr("data-reporteID"));
                });
                gridComponentesAlmacen.find(".eliminar").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    EliminarReporteDesecho($(this).attr("data-reporteID"));
                });
                gridComponentesAlmacen.find(".entrada").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    var fechaRaw = new Date($(this).attr("data-date"));
                    $("#txtEntradaAlmacen").val("");
                    $("#txtEntradaAlmacen").datepicker('option', 'minDate', moment.utc(fechaRaw)._d);
                    $("#btnGuardarEntrada").attr("data-index", $(this).attr("data-index"));
                    $("#modalEntradaAlmacen").modal("show");
                });

            }).on("selected.rs.jquery.bootgrid", function (e, rows) {
                for (var i = 0; i < rows.length; i++) {
                    rowIds.push(rows[i].id + " " + rows[i].noComponente + " " + rows[i].subconjunto);
                }
                if (rowIds.length > 0) {
                    if (tipoUsuario != 3) { btnCambioAlmacen.prop("disabled", false); }
                    if (tipoUsuario != 3) { btnIntercambioAlmacen.prop("disabled", false); }
                }
                if (rowIds.length > 1) {
                }
            }).on("deselected.rs.jquery.bootgrid", function (e, rows) {
                for (var i = 0; i < rows.length; i++) {
                    rowIds.pop(rows[i].id + " " + rows[i].noComponente);
                }
                if (rowIds.length < 2) {
                }
                if (rowIds.length < 1) {
                    btnCambioAlmacen.prop("disabled", true);
                    btnIntercambioAlmacen.prop("disabled", true);
                }
            });
        }

        function initGridEvidenciaDesecho() {

            gridEvidenciaDesecho.bootgrid({
                rowCount: -1,
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "eliminar": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar'>" +
                            "<span class='glyphicon glyphicon-remove'></span>  </button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                gridEvidenciaDesecho.find(".eliminar").on('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    var rowID = parseInt($(this).parent().parent().attr('data-row-id'));
                    evidenciaDesecho[rowID] = "";
                    gridEvidenciaDesecho.bootgrid("remove", [rowID]);
                });
            });;
            gridEvidenciaDesecho.bootgrid("clear");
        }

        function initGridSerieDesecho() {
            gridSerieDesecho.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {

                }
            }).on("loaded.rs.jquery.bootgrid", function () {

                gridSerieDesecho.find(".detalle").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                });
            });
            gridSerieDesecho.bootgrid("clear");
        }

        function openModalComponentesAlmacen() {
            $("#title-modal-componenteAlmacen").text("Detalles");
            $("#modalDetallesComponenteAlmacen").modal('show');
        }

        function openModalIntercambioAlmacen() {
            CargarCorreosAlmacenInter();
            txtModalIntercambioUnidadAlmacen.val('');
            txtModalIntercambioChoferAlmacen.val('');
            modalIntercambioAlmacen.modal('show');
        }

        function cargarGridAlmacen() {
            rowIds = [];
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarComponentesAlmacen",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ noComponente: txtFiltroNoComponenteAlmacen.val(), idLocacion: cboFiltroLocacionAlmacen.val(),
                     descripcionComponente: txtFiltroDescripcionComponenteAlmacen.val().trim(), 
                    estatus: 1, grupoId: cboFiltroGrupoMaquinaAlmacen.val(), modeloId: cboFiltroModeloMaquinaAlmacen.val() }),
                success: function (response) {
                    if (response.success) {
                        //tipoUsuario = response.tipoUsuario;
                        gridComponentesAlmacen.bootgrid("clear");
                        gridComponentesAlmacen.bootgrid("append", response.componentes);
                        gridComponentesAlmacen.bootgrid('reload');
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function cargarReporteAlmcen(){ 
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarReporteAlmacen",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ 
                    noComponente: txtFiltroNoComponenteAlmacen.val(),
                     idLocacion: cboFiltroLocacionAlmacen.val(),
                     descripcionComponente: txtFiltroDescripcionComponenteAlmacen.val().trim(), 
                     estatus: 1, 
                     grupoId: cboFiltroGrupoMaquinaAlmacen.val(),
                     modeloId: cboFiltroModeloMaquinaAlmacen.val() }),
                success: function (response) {
                    if (response.success) {
                        $.unblockUI();
                        ireporteAlmacen.attr("src", "/Reportes/Vista.aspx?idReporte=215");
                        $(window).scrollTop(0);
                        $("#reporteAlmacen2 > #reportViewerModal > body").css("overflow", "hidden");
                        $("#reporteAlmacen2 > #reportViewerModal").css("width", "100%");
                        $("#reporteAlmacen2 > #reportViewerModal").css("height", "105%");
                        $("#reporteAlmacen2 > #reportViewerModal > #report").onload = function () {
                        };
                        $.unblockUI();
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        function abrirReporteDesecho(idReporteDesecho) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/getReporteDesecho',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({ idReporteDesecho: idReporteDesecho }),
                success: function (response) {
                    $.unblockUI();
                    ireporteDesecho.attr("src", "/Reportes/Vista.aspx?idReporte=164");

                    $(window).scrollTop(0);
                    $("#reporteDesecho > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteDesecho > #reportViewerModal").css("width", "100%");
                    $("#reporteDesecho > #reportViewerModal").css("height", "105%");
                    $("#reporteDesecho > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function EliminarReporteDesecho(idReporteDesecho) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/EliminarReporteDesecho',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({ idReporteDesecho: idReporteDesecho }),
                success: function (response) {
                    $.unblockUI();
                    if (response.exito) {
                        AlertaGeneral("Alerta", "Se ha eliminado el registro.");
                        cargarGridAlmacen();
                    }
                    else { AlertaGeneral("Alerta", "No es posible eliminar el registro. Intente de nuevo."); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function initGridCambioAlmacen() {
            gridCambioAlmacen.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false
            });
        }

        function initCambioAlmacen() {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            cboModalIntercambioAlmacen.fillCombo('/Overhaul/FillCboLocacion', { tipoLocacion: 1 });
            gridCambioAlmacen.bootgrid("clear");
            cboModalIntercambioAlmacen.val("");
            cboModalIntercambioAlmacen.change();
            //$("#cboModalIntercambioAlmacen option[value='" + $(this).attr("data-locacion") + "']").remove();
            for (var i = 0; i < rowIds.length; i++) {
                var indexDescripcion = rowIds[i].indexOf(rowIds[i].split(' ')[2]);
                var JSONINFO = [{ "id": rowIds[i].split(' ')[0], "numero": i, "noComponente": rowIds[i].split(' ')[1], "descripcion": rowIds[i].substring(indexDescripcion, rowIds[i].length) }];
                gridCambioAlmacen.bootgrid("append", JSONINFO);
            }
            openModalIntercambioAlmacen();
            $.unblockUI();
        }

        function initCambioAlmacenIntercambio() {
            confirmacion = 2;
            ConfirmacionEliminacion("Desecho", "¿Está seguro que desea marcar como intercambio los componentes seleccionados?");
        }

        function CambioAlmacenDesecho(componente) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CambioAlmacenDesecho",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ arrComponentes: componente, idAlmacen: 1013 }),
                success: function (response) {
                    $.unblockUI();
                    cargarGridAlmacen();
                    ConfirmacionGeneral("Confirmación", "Se realizó el cambio de almacén a desecho", "bg-green");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function CambioAlmacenIntercambio() {
            var arrComponentes = [];
            for (var i = 0; i < rowIds.length; i++) {
                arrComponentes.push(rowIds[i].split(' ')[0]);
            }
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CambioAlmacenDesecho",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ arrComponentes: arrComponentes, idAlmacen: 1019 }),
                success: function (response) {
                    $.unblockUI();
                    cargarGridAlmacen();
                    //modalIntercambioAlmacen.modal('hide');
                    //gridCambioAlmacen.bootgrid("clear");
                    //$("#cboModalIntercambioAlmacen").val("");
                    //enviarCorreoCambioAlmacen(arrComponentes);
                    btnCambioAlmacen.prop("disabled", true);
                    btnIntercambioAlmacen.prop("disabled", true);
                    ConfirmacionGeneral("Confirmación", "Se realizó el cambio de almacén a intercambio", "bg-green");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
            //enviarCorreoCambioAlmacen(arrComponentes);         
        }

        function validCambioAlmacen() {
            estado = true;
            if (cboModalIntercambioAlmacen.val() == "" || cboModalIntercambioAlmacen.val() == null) { estado = false; }
            if (txtModalIntercambioUnidadAlmacen.val() == "" || txtModalIntercambioUnidadAlmacen.val() == null) { estado = false; }
            if (txtModalIntercambioChoferAlmacen.val() == "" || txtModalIntercambioChoferAlmacen.val() == null) { estado = false; }
            return estado;
        }

        function cambioAlmacen(e) {
            if (validCambioAlmacen()) {
                e.preventDefault();
                var arrComponentes = [];
                for (var i = 0; i < rowIds.length; i++) {
                    arrComponentes.push(rowIds[i].split(' ')[0]);
                }
                let correos = [];
                $('#tblCorreos tbody tr').each(function () {
                    correos.push($(this).find('td:eq(0)').text());
                });
                $.blockUI({ message: "Procesando..." });
                $.ajax({
                    url: "/Overhaul/CambioAlmacen",
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: JSON.stringify({ arrComponentes: arrComponentes, idAlmacen: $("#cboModalIntercambioAlmacen").val(), placas: txtModalIntercambioUnidadAlmacen.attr("data-index"), chofer: txtModalIntercambioChoferAlmacen.val(), correos: correos, unidad: txtModalIntercambioUnidadAlmacen.val() }),
                    success: function (response) {
                        $.unblockUI();
                        cargarGridAlmacen();
                        modalIntercambioAlmacen.modal('hide');
                        gridCambioAlmacen.bootgrid("clear");
                        $("#cboModalIntercambioAlmacen").val("");
                        //enviarCorreoCambioAlmacen(arrComponentes);
                        btnCambioAlmacen.prop("disabled", true);
                        btnIntercambioAlmacen.prop("disabled", true);
                        ConfirmacionGeneral("Confirmación", "Se realizó el cambio de almacén", "bg-green");

                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);
                    }
                });
                //enviarCorreoCambioAlmacen(arrComponentes);
            }
            else {
                AlertaGeneral("Alerta", "Se requiere especificar todos los datos");
            }
        }

        function enviarCorreoCambioAlmacen(arrComponentes) {
            $.blockUI({
                message: 'Enviando correo de aprobación...',
                baseZ: 2000
            });
            let mail = $.post("/Overhaul/enviarCorreoCambioAlmacen", { arrComponentes: arrComponentes, idAlmacen: $("#cboModalIntercambioAlmacen").val() });
            mail.done(function (correo) {
                if (correo.success) {
                }
            });
            mail.always(function (a) {
                $.unblockUI();
            });
        }

        function datosReporteDesecho() {
            return {
                id: 0,
                fechaRemocion: new Date(),
                componenteRemovidoID: componenteDesecho,
                componenteInstaladoID: "-1",
                maquinaID: "-1",
                areaCuenta: obra,
                motivoRemocionID: 5,
                destinoID: 1013,
                comentario: txtaObsDesecho.val().trim(),
                garantia: false,
                empresaResponsable: 0,
                //imgComponenteRemovido: imgDesecho.attr("src"),
                estatus: 5
            }
        }

        function ValidarReporteDesecho() {
            let estado = true;
            if (estado == true && inCargarSerieDesecho[0].files.length == 0) {
                estado = false;
                AlertaGeneral("Alerta", "Se requiere la imagen de la serie del componente a desechar");
            }
            if (estado == true && txtaObsDesecho.val() == '') {
                estado = false;
                AlertaGeneral("Alerta", "Se requiere especificar el motivo de desecho");
            }
            //if (estado == true) {
            //    let arregloEvidencias = jQuery.grep(evidenciaDesecho, function (a) {
            //        return a !== "";
            //    });
            //    if (arregloEvidencias.length < 1) {
            //        estado = false;
            //        AlertaGeneral("Alerta", "Se requieren imágenes de evidencia");
            //    }
            //}
            if (estado) {
                GuardarReporteDesecho();
                //CambioAlmacenDesecho(componenteDesecho);
                modalDesechoAlmacen.modal("hide");
            }
        }

        function GuardarReporteDesecho() {
            let formData = new FormData();
            let request = new XMLHttpRequest();
            let fotoSerie = document.getElementById("inCargarSerieDesecho").files[0];

            formData.append("reporteDesecho", JSON.stringify(datosReporteDesecho()));
            formData.append("archivoSerie", fotoSerie);
            for (let i = 0; i < evidenciaDesecho.length; i++) { formData.append("archivoEvidencia", evidenciaDesecho[i]); }
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                type: "POST",
                url: '/Overhaul/GuardarReporteDesecho',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    $.unblockUI();
                    evidenciaDesecho = [];
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function ValidarDesecho(idReporte, idComponente) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Overhaul/AutorizarDesechoAlmacen",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idReporte: idReporte, idComponente: idComponente }),

                success: function (response) {
                    $.unblockUI();
                    cargarGridAlmacen();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function SubirEvidenciaDesecho(e, tipo, input) {
            e.preventDefault();
            if (document.getElementById(input).files[0] != null) {
                var ext = document.getElementById(input).files[0].name.match(/\.(.+)$/)[1];
                ext = ext.toLowerCase();
                if (tipo == 1 ? ext == "pdf" : ($.inArray(ext, ["jpg", "jpeg", "png"]) != -1)) {
                    size = document.getElementById(input).files[0].size;
                    if (size > 20971520) {
                        AlertaGeneral("Alerta", "Archivo sobrepasa los 20MB");
                    }
                    else {
                        if (size <= 0) {
                            AlertaGeneral("Alerta", "Archivo vacío");
                        }
                        else {
                            guardarEvidenciaDesecho(e, tipo, input);
                        }
                    }
                }
                else {
                    if (tipo == 1) { AlertaGeneral("Alerta", "Sólo se aceptan archivos PDF"); }
                    else { AlertaGeneral("Alerta", "Sólo se aceptan archivos tipo imagen"); }
                }
            }
        }

        function leerURL(input, imagen) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) { imagen.attr('src', e.target.result); }
                reader.readAsDataURL(input.files[0]);
            }
        }

        function guardarEvidenciaDesecho(e, tipo, input) {
            e.preventDefault();
            let formData = new FormData();
            let request = new XMLHttpRequest();
            let file = document.getElementById(input).files[0];
            let idEvidenciaDes = evidenciaDesecho.length;
            evidenciaDesecho.push(file);
            let JSONINFO = [{ "id": idEvidenciaDes, "nombre": file.name }];
            gridEvidenciaDesecho.bootgrid("append", JSONINFO);
        }

        function CargarCorreosAlmacenInter() {
            var listaCorreos = [1, 1, 0, 0, 1];
            CargartablaCorreos(tblCorreos, listaCorreos, [cboModalIntercambioAlmacen.val()]);
        }

        function GuardarEntradaAlmacen() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/GuardarEntradaAlmacen",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ trackingID: $("#btnGuardarEntrada").attr("data-index"), fecha: $("#txtEntradaAlmacen").val() }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        $("#modalEntradaAlmacen").modal("hide");
                        ConfirmacionGeneral("Éxito", "Se ha guardado la fecha de entrada a almacén", "bg-green");
                        cargarGridAlmacen();
                    }
                    else {
                        AlertaGeneral("Alerta", "Se produjo un error al intentar guardar la fecha.");
                    }

                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);

                }
            });
        }

        ///////////////////////Inactivos//////////////////

        function initGridInactivos() {
            gridComponentesInactivos.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "detalle": function (column, row) {
                        return "<button type='button' class='btn btn-primary detalle' data-index='" + row.id + "' data-locacion = '" + row.locacionID + "' data-noComponente='" + row.noComponente + "' data-proveedor='" + row.proveedor + "' data-ordenCompra='" + row.ordenCompra + "'>" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                            " </button>";
                    },
                    "reactivar": function (column, row) {
                        return "<button type='button' class='btn btn-primary reactivar' data-index='" + row.id + "' data-locacion = '" + row.locacionID + "' data-noComponente='" + row.noComponente
                            + "' data-proveedor='" + row.proveedor + "' data-ordenCompra='" + row.ordenCompra + "' data-fecha='" + row.fechaRaw + "'>" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                            " </button>";
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                gridComponentesInactivos.find(".detalle").parent().css("text-align", "center");
                gridComponentesInactivos.find(".detalle").parent().css("width", "3%");
                $('[data-toggle="tooltip"]').tooltip();
                gridComponentesInactivos.find(".detalle").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    cargarGridHistorialComponente($(this).attr("data-index"), $("#gridDetallesHistorialInactivos"));
                    var proveedor = $(this).attr("data-proveedor");
                    var ordenCompra = $(this).attr("data-ordenCompra");
                    if (proveedor != "" && proveedor != null) $("#spProveedorHistorialInactivos").text($(this).attr("data-proveedor"));
                    else $("#spProveedorHistorialInactivos").text("Sin Especificar");
                    if (ordenCompra != "" && ordenCompra != null) $("#spOrdenCompraHistorialInactivos").text($(this).attr("data-ordenCompra"));
                    else $("#spOrdenCompraHistorialInactivos").text("Sin Especificar");
                    $("#lgHistorialInactivos").text($(this).attr("data-noComponente"));
                    $("#title-modal-historialInactivos").text("Historial de componente");
                    openModalComponentesInactivos();
                });
                gridComponentesInactivos.find(".reactivar").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    lbReactivarInactivos.text("Reactivación del componente " + $(this).attr("data-noComponente"));
                    destinoReactivarInactivos.val("");
                    fechaReactivarInactivos.datepicker("setDate", new Date());
                    btnReactivarInactivos.attr("data-index", $(this).attr("data-index"));
                    var fechaReactivarInactivo = new Date($(this).attr("data-fecha"));
                    fechaReactivarInactivos.datepicker("option", "minDate", fechaReactivarInactivo);
                    modalReactivarInactivos.modal("show");
                });
            });
        }

        function ReactivarComponente() {
            if (ValidarReactivarInactivo()) {
                $.blockUI({ message: "Procesando...", baseZ: 2000 });
                $.ajax({
                    url: "/Overhaul/ReactivarComponentesInactivos",
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: JSON.stringify({
                        componenteID: btnReactivarInactivos.attr("data-index"),
                        locacionID: destinoReactivarInactivos.val(),
                        fecha: fechaReactivarInactivos.val()
                    }),
                    success: function (response) {
                        if (response.success) {
                            if (response.exito) {
                                AlertaGeneral("Éxito", "Se ha reactivado el componente con exito");
                                cargarGridInactivos();
                            }
                        }
                        else {
                            AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                        }
                        $.unblockUI();
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);

                    }
                });
            }
        }

        function ValidarReactivarInactivo() {
            let estado = true;
            destinoReactivarInactivos.css("background-color", "#fff");
            fechaReactivarInactivos.css("background-color", "#fff");
            if (destinoReactivarInactivos.val() == null || destinoReactivarInactivos.val() == "") {
                estado = false;
                destinoReactivarInactivos.css("background-color", "pink");
            }
            if (fechaReactivarInactivos.val() == null || fechaReactivarInactivos.val() == "") {
                estado = false;
                fechaReactivarInactivos.css("background-color", "pink");
            }
            return estado;
        }

        function openModalComponentesInactivos() {
            $("#title-modal-componenteInactivos").text("Detalles");
            $("#modalDetallesComponenteInactivos").modal('show');
        }

        function cargarGridInactivos() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarComponentesInactivos",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ noComponente: txtFiltroNoComponenteInactivos.val(), idLocacion: cboFiltroLocacionInactivos.val(), descripcionComponente: txtFiltroDescripcionComponenteInactivos.val().trim(), estatus: 3, grupoId: 0, modeloId: 0 }),
                success: function (response) {
                    if (response.success) {
                        gridComponentesInactivos.bootgrid("clear");
                        gridComponentesInactivos.bootgrid("append", response.componentes);
                        gridComponentesInactivos.bootgrid('reload');
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);

                }
            });
        }

        function unmaskDinero(dinero) {
            return Number(dinero.replace(/[^0-9\.]+/g, ""));
        }
        function maskDinero(numero) {
            return "$" + parseFloat(numero).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        }
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.overhaul.administracioncomponentes = new administracioncomponentes();
    });
})();


