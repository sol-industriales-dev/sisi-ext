(() => {
    $.namespace("Maquinaria.BackLogs.BackLogsRegistroObra");

    BackLogs = function () {
        const cboProyecto = $("#cboProyecto");

        //#region CONST BACKLOGS
        const btnFiltroBuscarBL = $('#btnFiltroBuscarBL');
        const cboAnio = $('#cboAnio');
        const mdlDescripcionBL = $('#mdlDescripcionBL');
        const txtDescripcionBL = $('#txtDescripcionBL');
        const lblCantBL = $('#lblCantBL');
        const cboCC = $("#cboCC");
        const txtModelo = $("#txtModelo");
        const txtGrupo = $("#txtGrupo");
        const txtFecha = $("#txtFecha");
        const txtHoras = $("#txtHoras");
        const txtFolio = $("#txtFolio");
        const txtDescripcion = $("#txtDescripcion");
        const chkParte = $("#chkParte");
        const chkMO = $("#chkMO");
        const cboConjunto = $("#cboConjunto");
        const cboSubconjunto = $("#cboSubconjunto");
        const btnCrearEditarBackLog = $("#btnCrearEditarBackLog");
        const btnInformeRehabilitacion = $("#btnInformeRehabilitacion");
        const btnCatInsumos = $('#btnCatInsumos');
        const txtInsumo = $('#txtInsumo');

        const tblBL_Partes_tbody = $("#tblBL_Partes tbody");
        const tblBL_Partes = $("#tblBL_Partes");

        const modalCrearParte = $("#modalCrearParte");
        const btnModalAbrirParte = $("#btnModalAbrirParte");
        const btnCrearEditarParte = $("#btnCrearEditarParte");
        const btnCrearEditarCancelarParte = $("#btnCrearEditarCancelarParte");
        const txtCantidad = $("#txtCantidad");
        const txtParte = $("#txtParte");
        const txtArticulo = $("#txtArticulo");
        const txtUnidad = $('#txtUnidad');
        const lblEliminarParte = $("#lblEliminarParte");

        const modalEliminarParte = $("#modalEliminarParte");
        const btnEliminarParte = $("#btnEliminarParte");
        const btnEliminarCancelarParte = $("#btnEliminarCancelarParte");

        const tblBL_ManoObra = $("#tblBL_ManoObra");
        const tblBL_ManoObra_tbody = $("#tblBL_ManoObra tbody");
        const modalCrearManoObra = $("#modalCrearManoObra");
        const btnModalAbrirManoObra = $("#btnModalAbrirManoObra");
        const txtDescripcionManoObra = $("#txtDescripcionManoObra");
        const modalEliminarManoObra = $("#modalEliminarManoObra");
        const btnEliminarManoObra = $("#btnEliminarManoObra");
        const btnEliminarCancelarManoObra = $("#btnEliminarCancelarManoObra");
        const btnGuardarManoObra = $("#btnGuardarManoObra");
        const btnCrearEditarCancelarManoObra = $("#btnCrearEditarCancelarManoObra");
        const lblEliminarManoObra = $("#lblEliminarManoObra");
        const btnCrearEditarManoObra = $("#btnCrearEditarManoObra");

        const tblBL_CatBackLogs = $("#tblBL_CatBackLogs");
        const tblBL_CatBackLogs_tbody = $("#tblBL_CatBackLogs tbody");

        const modalEliminarBackLog = $("#modalEliminarBackLog");
        const btnEliminarBackLog = $("#btnEliminarBackLog");
        const btnEliminarCancelarBackLog = $("#btnEliminarCancelarBackLog");
        const btnLimpiarFormCrearEditarBackLog = $("#btnLimpiarFormCrearEditarBackLog");

        const btnRegistroBackLogs = $("#btnRegistroBackLogs");
        const btnProgramaInspeccion = $("#btnProgramaInspeccion");

        const modalCrearEditarNumOC = $('#modalCrearEditarNumOC');
        const btnVerificarOrdenesCompra = $('#btnVerificarOrdenesCompra');
        const divOrdenesCompra = $('#divOrdenesCompra');
        const btnCollapseCrearEditarOC = $('#btnCollapseCrearEditarOC');
        const chkVoBoOC = $('#chkVoBoOC');

        const mdlCatalogoInsumos = $('#mdlCatalogoInsumos');
        const tblCatInsumos = $('#tblCatInsumos');
        const btnFiltroBuscar = $('#btnFiltroBuscar');
        const txtFiltroInsumo = $('#txtFiltroInsumo');
        const txtFiltroDescripcion = $('#txtFiltroDescripcion');
        const btnInicioObra = $('#btnInicioObra');
        const btnCancelarRequisicion = $('#btnCancelarRequisicion');
        const btnMotivoCancelacion = $('#btnMotivoCancelacion');
        const cboUsuarioResponsable = $('#cboUsuarioResponsable');

        var rowEditarParte = 0;
        var rowEditarManoObra = 0;

        let dtBackLogs;
        let dtPartes;
        let dtManoObra;
        let BanderaCrearEditarBacklog = false;
        let dtInsumos;

        var ContPartidasParte = 0;
        var ContPartidasManoObra = 0;
        var idRowParte = 0;
        var idRowManoObra = 0;
        var arrPartes = [];
        var arrManoObra = [];
        //#endregion

        //#region CONST CATALOGO CONJUNTOS
        const btnConjuntosCatConjuntos = $('#btnConjuntosCatConjuntos');
        const modalCrearEditarConjuntoCatConjuntos = $('#modalCrearEditarConjuntoCatConjuntos');
        const txtConjuntoCatConjuntos = $('#txtConjuntoCatConjuntos');
        const txtAbreviacionoCatConjuntos = $('#txtAbreviacionoCatConjuntos');
        const btnCrearEditarConjuntoCatConjuntos = $('#btnCrearEditarConjuntoCatConjuntos');
        const btnCrearEditarCancelarConjuntoCatConjuntos = $('#btnCrearEditarCancelarConjuntoCatConjuntos');
        const tblConjuntosCatConjuntos = $('#tblConjuntosCatConjuntos');
        const btnNuevoConjuntoCatConjuntos = $('#btnNuevoConjuntoCatConjuntos');
        const btnCollapseConjuntosCatConjuntos = $('#btnCollapseConjuntosCatConjuntos');
        const divCrearEditarConjuntoCatConjuntos = $('#divCrearEditarConjuntoCatConjuntos');
        //#endregion

        //#region CONST CATALOGO SUBCONJUNTOS
        const btnSubconjuntosCatSubconjuntos = $('#btnSubconjuntosCatSubconjuntos');
        const modalCrearEditarSubconjuntoCatSubconjuntos = $('#modalCrearEditarSubconjuntoCatSubconjuntos');
        const btnNuevoSubconjuntoCatSubconjuntos = $('#btnNuevoSubconjuntoCatSubconjuntos');
        const divCrearEditarSubconjuntoCatSubconjuntos = $('#divCrearEditarSubconjuntoCatSubconjuntos');
        const btnCollapseSubconjuntosCatSubconjuntos = $('#btnCollapseSubconjuntosCatSubconjuntos');
        const cboConjuntoCatSubconjuntos = $('#cboConjuntoCatSubconjuntos');
        const txtSubconjuntoCatSubconjuntos = $('#txtSubconjuntoCatSubconjuntos');
        const txtAbreviacionCatSubconjuntos = $('#txtAbreviacionCatSubconjuntos');
        const btnCrearEditarCancelarSubconjuntoCatSubconjuntos = $('#btnCrearEditarCancelarSubconjuntoCatSubconjuntos');
        const btnCrearEditarSubconjuntoCatSubconjuntos = $('#btnCrearEditarSubconjuntoCatSubconjuntos');
        const tblSubconjuntosCatSubconjuntos = $('#tblSubconjuntosCatSubconjuntos');
        //#endregion

        //#region CONST CATALOGO NUMERO DE REQUISICIONES
        const btnVerificarRequisiciones = $('#btnVerificarRequisiciones');
        const divCrearEditarNumRequisicion = $('#divCrearEditarNumRequisicion');
        const btnCollapseNuevoNumRequisicion = $('#btnCollapseNuevoNumRequisicion');
        const btnCrearEditarCancelarNumRequisicion = $('#btnCrearEditarCancelarNumRequisicion');
        const tblBL_Requisiciones = $('#tblBL_Requisiciones');
        const modalCrearEditarNumRequisicion = $('#modalCrearEditarNumRequisicion');
        const txtNumRequisicion = $('#txtNumRequisicion');
        const btnCrearEditarNumRequisicion = $('#btnCrearEditarNumRequisicion');
        const tblBL_MotivoCancelacionReq = $('#tblBL_MotivoCancelacionReq');
        const btnCrearRequisicion = $('#btnCrearRequisicion');
        const mdlMotivosCancelacionReq = $('#mdlMotivosCancelacionReq');
        const modalCrearEditarNumRequisicionManual = $('#modalCrearEditarNumRequisicionManual');
        const lblTitleCrearEditarNumRequisicionManual = $('#lblTitleCrearEditarNumRequisicionManual');
        const txtCrearEditarNumRequisicionManual = $('#txtCrearEditarNumRequisicionManual');
        const lblBtnCrearEditarNumRequisicionManual = $('#lblBtnCrearEditarNumRequisicionManual');
        const btnAbrirModalRequisicionManual = $('#btnAbrirModalRequisicionManual');
        const btnCrearEditarNumRequisicionManual = $('#btnCrearEditarNumRequisicionManual');
        const btnAbrirModalTraspaso = $('#btnAbrirModalTraspaso');
        const mdlCETraspaso = $('#mdlCETraspaso');
        const tblBL_FoliosTraspasos = $('#tblBL_FoliosTraspasos');
        const btnCETraspasoFolio = $('#btnCETraspasoFolio');
        const lblBtnCETraspasoFolio = $('#lblBtnCETraspasoFolio');
        const btnShowMdlCEFolioTraspaso = $('#btnShowMdlCEFolioTraspaso');
        const mdlCETraspasoFolio = $('#mdlCETraspasoFolio');
        const txtCETraspasoFolio = $('#txtCETraspasoFolio');
        const txtCETraspasoFolioAlmacen = $('#txtCETraspasoFolioAlmacen');
        const txtCETraspasoFolioAlmacenDescripcion = $('#txtCETraspasoFolioAlmacenDescripcion');
        const lblTitleCETraspasoFolio = $('#lblTitleCETraspasoFolio');
        const txtCETraspasoFolioAlmacenDestino = $('#txtCETraspasoFolioAlmacenDestino');
        const txtCETraspasoFolioAlmacenDescripcionDestino = $('#txtCETraspasoFolioAlmacenDescripcionDestino');
        const txtCETraspasoFolioNumero = $('#txtCETraspasoFolioNumero');
        const txtCETraspasoFolioCC = $('#txtCETraspasoFolioCC');
        const txtCETraspasoFolioCCDestino = $('#txtCETraspasoFolioCCDestino');
        let dtLstReqCC;
        let dtLstDetReqCC;
        let dtFoliosTraspasos;
        //#endregion

        //#region CONST MODAL LISTADO REQUISICIONES CC
        const mdlLstReqCC = $('#mdlLstReqCC');
        const tblLstReqCC = $('#tblLstReqCC');
        //#endregion

        //#region CONST MODAL LISTADO DETALLE DE REQUISICIONES CC
        const mdlLstDetReqCC = $('#mdlLstDetReqCC');
        const tblLstDetReqCC = $('#tblLstDetReqCC');
        //#endregion

        //#region CONST MODAL LISTADO ORDENES DE COMPRA CON RELACION A LAS REQUISICIONES
        const tblLstOcReq = $('#tblLstOcReq');
        const mdlLstOcReq = $('#mdlLstOcReq');

        const tblLstDetOcReq = $('#tblLstDetOcReq');
        const mdlLstDetOcReq = $('#mdlLstDetOcReq');
        let dtLstOcReq;
        let dtLstDetOcReq;
        //#endregion

        //#region CONST ORDENES DE COMPRA
        const tblBL_OrdenesCompra = $('#tblBL_OrdenesCompra');
        const btnCrearEditarOC = $('#btnCrearEditarOC');
        //#endregion

        //#region CONST EVIDENCIAS
        const mdlEvidencias = $('#mdlEvidencias');
        const btnMdlEvidencias = $('#btnMdlEvidencias');
        const btnCargarEvidencia = $('#btnCargarEvidencia');
        const btnCargarEvidenciaFile = $('#btnCargarEvidenciaFile');
        const tblEvidencias = $('#tblEvidencias');
        const lblNombreArchivo = $('#lblNombreArchivo');
        const btnGuardarEvidencia = $('#btnGuardarEvidencia');
        const chkMdlEvidenciasOTVacia = $('#chkMdlEvidenciasOTVacia');
        const cboMdlEvidenciasTipoEvidencia = $('#cboMdlEvidenciasTipoEvidencia');
        //#endregion

        //#region CONST LISTADO OC SURTIDAS
        const mdlListadoOCSurtidas = $('#mdlListadoOCSurtidas');
        const tblListadoOCSurtidas = $('#tblListadoOCSurtidas');
        //#endregion

        //#region CONST OCULTOS
        const inputEmpresaActual = $('#inputEmpresaActual');
        _empresaActual = +inputEmpresaActual.val();
        //#endregion

        (function init() {
            fncListeners();
            fncFillCombos();
            initTablaBackLogs();
            initTablaPartes();
            initTablaManoObra();
            btnCrearEditarBackLog.attr("data-id", 0);
            fncHabilitarDeshabilitarFormulario();
            initTablaConjuntos();
            initTablaSubconjuntos();
            initTablaRequisiciones();
            initTablaOC();
            initTablaCatInsumos();
            initTablaLstReqCC();
            initTablaLstDetReqCC();
            initTablaLstOcReq();
            initTablaLstDetOcReq();
            initTablaMotivoCancelacionReq();
            initTablaEvidencias();
            obtenerUrlPArams();
            initTblFoliosTraspasos();
        })();

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            if (variables.areaCuenta != undefined) {
                cboProyecto.val(variables.areaCuenta);
                if (variables.areaCuenta != "") {
                    cboProyecto.trigger('change');
                }
            }
        }

        function getUrlParams(url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            return params;
        }

        function fncListeners() {
            fncAddPlaceholders();
            fncSoloNumeros();
            fncDeshabilitarAutocomplete();
            fncDatepicker();
            fncReadOnly();
            fncMaxLength();

            cboMdlEvidenciasTipoEvidencia.select2();
            cboMdlEvidenciasTipoEvidencia.select2({
                width: "100%"
            });

            btnFiltroBuscarBL.on("click", function () {
                if (cboProyecto.val() != "" && cboAnio.val() != "") {
                    fncFillCboCC(cboProyecto.val());
                    fncLimpiarCtrlsCrearEditarBackLog();
                    fncGetBackLogs();
                    btnCrearEditarBackLog.attr("data-id", 0);
                } else {
                    let strMensajeError = "";
                    strMensajeError += cboProyecto.val() == "" ? "Es necesario indicar el proyecto." : "";
                    strMensajeError += cboAnio.val() == "" ? "Es necesario indicar el año." : "";
                    Alert2Warning(strMensajeError);
                }
                fncHabilitarDeshabilitarFormulario();
            });

            cboProyecto.change(function (e) {
                // if ($(this).val() != "") {
                //     fncFillCboCC($(this).val());
                //     fncLimpiarCtrlsCrearEditarBackLog();
                //     fncGetBackLogs();
                //     btnCrearEditarBackLog.attr("data-id", 0);
                // } else {
                //     Alert2Warning("Es necesario seleccionar un proyecto.");
                // }
                // fncHabilitarDeshabilitarFormulario();
            });

            txtFiltroInsumo.click(function (e) {
                $(this).select();
            });

            txtFiltroDescripcion.click(function (e) {
                $(this).select();
            });

            //#region EVENTOS BACKLOGS
            cboUsuarioResponsable.fillCombo("/BackLogs/FillcboUsuarios", {}, false);
            cboUsuarioResponsable.select2();

            btnCancelarRequisicion.click(function (e) {
                Alert2AccionConfirmarInput("Cancelar requisición", "Es necesario indicar el motivo de que este BackLog <b>no requiere requisición</b>.", "Confirmar", "Cancelar",
                    (strMotivo) => fncCancelarRequisicion(strMotivo))
            });

            btnMotivoCancelacion.click(function (e) {
                mdlMotivosCancelacionReq.modal("show");
                fncGetMotivosCancelacion(); //TODO
            });

            btnCatInsumos.click(function (e) {
                mdlCatalogoInsumos.modal("show");
            });

            btnFiltroBuscar.click(function (e) {
                fncGetCatInsumos();
            });

            btnLimpiarFormCrearEditarBackLog.click(function (e) {
                fncLimpiarCtrlsCrearEditarBackLog();
            })

            btnEliminarCancelarBackLog.click(function (e) {
                modalEliminarBackLog.modal("hide");
            })

            btnEliminarBackLog.click(function (e) {
                fncEliminarBackLog();
            })

            btnEliminarCancelarManoObra.click(function (e) {
                modalEliminarManoObra.modal("hide");
            })

            btnModalAbrirParte.click(function () {
                fncLimpiarCtrlsCrearEditarParte();
                modalCrearParte.modal("show");
            });

            btnCrearEditarCancelarParte.click(function () {
                modalCrearParte.modal("hide");
            });

            btnModalAbrirManoObra.click(function (e) {
                fncLimpiarCtrlsCrearEditarManoObra();
                modalCrearManoObra.modal("show");
            });

            btnCrearEditarCancelarManoObra.click(function (e) {
                modalCrearManoObra.modal("hide");
            });

            cboCC.change(function (e) {
                if ($(this).val() != "") {
                    fncGetDatosCC();
                    if ($('select[id="cboCC"] option:selected').text() != $(this).attr("cc")) {
                        $(this).attr("esActualizarCC", true);
                    } else {
                        $(this).attr("esActualizarCC", false);
                    }
                } else {
                    txtModelo.val("");
                    txtGrupo.val("");
                }
            });

            cboConjunto.change(function (e) {
                if (cboConjunto.val() > 0) {
                    fncFillCboSubconjuntos();
                }
            });

            btnGuardarManoObra.click(function (e) {
                fncCrearManoObra();
            });

            btnEliminarCancelarParte.click(function (e) {
                modalEliminarParte.modal("hide");
            });

            tblBL_Partes.on("click", ".btnEliminarParte", function () {
                modalEliminarParte.modal("show");
                idRowParte = $(this).closest("tr").find("td").eq(0).html();
                let Insumo = $(this).closest("tr").find("td").eq(1).html();
                lblEliminarParte.html(Insumo);
                btnEliminarParte.attr("data-id", $(this).attr("data-id"));

                var row = $("#" + idRowParte + "").val();
                if (row == null) {
                    let x = 0;
                    tblBL_Partes.find("tbody tr").each(function (index) {
                        x++;
                        $(this).attr("id", x);
                    })
                }
            });

            btnEliminarParte.click(function () {
                let idBackLog = btnCrearEditarBackLog.attr("data-id");

                if (idBackLog > 0) {
                    fncEliminarParte(idRowParte);
                } else {
                    $("#" + idRowParte + "").remove();
                    modalEliminarParte.modal("hide");
                    fncReordenarPartidasPartes();
                }
            });

            tblBL_ManoObra.on("click", ".btnEliminarManoObra", function () {
                modalEliminarManoObra.modal("show");
                idRowManoObra = $(this).closest("tr").find("td").eq(0).html();
                let Descripcion = $(this).closest("tr").find("td").eq(1).html();
                lblEliminarManoObra.html(Descripcion);
                btnEliminarManoObra.attr("data-id", $(this).attr("data-id"));

                var row = $("#" + idRowManoObra + "").val();
                if (row == null) {
                    let x = 0;
                    tblBL_ManoObra.find("tbody tr").each(function (index) {
                        x++;
                        let manoObraID = "manoObraID" + x;
                        $(this).attr("id", manoObraID);
                    })
                }
            });

            btnEliminarManoObra.click(function () {
                let idBackLog = btnCrearEditarBackLog.attr("data-id");

                if (idBackLog > 0) {
                    fncEliminarManoObra(idRowManoObra);
                } else {
                    let manoObraID = "manoObraID" + idRowManoObra;
                    $("#" + manoObraID + "").remove();
                    fncReordenarPartidasManoObra();
                    modalEliminarManoObra.modal("hide");
                }
            });

            btnCrearEditarBackLog.click(function (e) {
                let dataID = btnCrearEditarBackLog.attr("data-id");
                let cboCCText = $('select[id="cboCC"] option:selected').text();
                let cboAttr = cboCC.attr("cc");

                if (dataID > 0 && cboCCText != cboAttr) {
                    Alert2AccionConfirmar("¡Cuidado!",
                        "Sí actualiza el CC, se eliminaran las requisiciones y ordenes de compra relacionadas a este BackLog. <br> ¿Desea continuar?",
                        "Confirmar", "Cancelar", () => fncCrearEditarBackLog());
                } else {
                    fncCrearEditarBackLog();
                }
            });

            btnCrearEditarParte.click(function (e) {
                let idBackLog = btnCrearEditarBackLog.attr("data-id");
                if (idBackLog > 0) {
                    fncCrearEditarParte(); //TODO
                } else {
                    fncCrearParte();
                }
                tblBL_Partes.show();
            });

            tblBL_Partes.on("click", ".btnEditarParte", function () {
                modalCrearParte.modal("show");

                rowEditarParte = $(this).parents("tr").find("td").eq(0).html();
                txtInsumo.val($(this).parents("tr").find("td").eq(1).html());
                txtCantidad.val($(this).parents("tr").find("td").eq(2).html());
                txtParte.val($(this).parents("tr").find("td").eq(3).html());
                txtArticulo.val($(this).parents("tr").find("td").eq(4).html());
                txtUnidad.val($(this).parents("tr").find("td").eq(5).html());
                btnCrearEditarParte.attr("data-id", $(this).attr("data-id"));
            });

            btnCrearEditarManoObra.click(function (e) {
                let idBackLog = btnCrearEditarBackLog.attr("data-id");
                if (idBackLog > 0) {
                    fncCrearEditarManoObra();
                } else {
                    fncCrearManoObra();
                }
                tblBL_ManoObra.show();
            });

            tblBL_ManoObra.on("click", ".btnEditarManoObra", function () {
                modalCrearManoObra.modal("show");

                rowEditarManoObra = $(this).parents("tr").find("td").eq(0).html();
                txtDescripcionManoObra.val($(this).parents("tr").find("td").eq(1).html());
                btnCrearEditarManoObra.attr("data-id", $(this).attr("data-id"));
            });

            btnInicioObra.click(function (e) {
                document.location.href = '/BackLogs/Index?areaCuenta=' + cboProyecto.val();
            });
            btnRegistroBackLogs.click(function (e) {
                document.location.href = '/BackLogs/RegistroBackLogsObra?areaCuenta=' + cboProyecto.val();
            });

            btnInformeRehabilitacion.click(function (e) {
                document.location.href = '/BackLogs/InformeBackLogsRehabilitacion?areaCuenta=' + cboProyecto.val();
            });

            btnProgramaInspeccion.click(function (e) {
                document.location.href = '/BackLogs/ProgramaInspeccionBackLogs?areCuenta=' + cboProyecto.val();
            })

            $("#btnReporteIndicadores").click(function (e) {
                document.location.href = "/BackLogs/ReporteIndicadoresObra?areaCuenta=" + cboProyecto.val();
            });

            chkParte.change(function (e) {
                if (chkParte.prop("checked")) {
                    btnModalAbrirParte.attr("disabled", false);
                    tblBL_Partes.show();
                } else {
                    btnModalAbrirParte.attr("disabled", true);
                    tblBL_Partes.hide();
                }
            });

            chkMO.change(function (e) {
                if (chkMO.prop("checked")) {
                    btnModalAbrirManoObra.attr("disabled", false);
                    tblBL_ManoObra.show();
                } else {
                    btnModalAbrirManoObra.attr("disabled", true);
                    tblBL_ManoObra.hide();
                }
            });

            tblBL_CatBackLogs.on('draw.dt', function () {
                fncColorearCeldaEstatus();
            });

            txtHoras.on("keypress", function (e) {
                aceptaSoloNumeroXD($(this), e, 2);
            });

            btnVerificarOrdenesCompra.click(function (e) {
                //fncGetAllOC();
                fncGetLstOcReq();
            });

            btnCrearEditarNumRequisicion.click(function (e) {
                fncCrearEditarRequisicion();
            });
            //#endregion

            //#region EVENTOS CATALOGO CONJUNTOS
            btnConjuntosCatConjuntos.click(function (e) {
                fncLimpiarCrearEditarConjunto();
                if (divCrearEditarConjuntoCatConjuntos.hasClass('in')) {
                    btnCrearEditarCancelarConjuntoCatConjuntos.trigger('click');
                }
                modalCrearEditarConjuntoCatConjuntos.modal("show");
                fncGetConjuntos();
            });

            btnNuevoConjuntoCatConjuntos.click(function (e) {
                fncLimpiarCrearEditarConjunto();
            });

            btnCrearEditarCancelarConjuntoCatConjuntos.click(function (e) {
                fncLimpiarCrearEditarConjunto();
            });

            btnCrearEditarConjuntoCatConjuntos.click(function (e) {
                fncCrearEditarConjunto();
                fncGetBackLogs();
            });
            //#endregion

            //#region EVENTOS CATALOGO SUBCONJUNTOS
            btnSubconjuntosCatSubconjuntos.click(function (e) {
                fncLimpiarCrearEditarSubconjunto();
                fncFillCboConjunto();
                if (divCrearEditarSubconjuntoCatSubconjuntos.hasClass('in')) {
                    btnCrearEditarCancelarSubconjuntoCatSubconjuntos.trigger('click');
                }
                modalCrearEditarSubconjuntoCatSubconjuntos.modal("show");
                fncGetSubconjuntos();
            });

            btnNuevoSubconjuntoCatSubconjuntos.click(function (e) {
                fncLimpiarCrearEditarSubconjunto();
            });

            btnCrearEditarCancelarSubconjuntoCatSubconjuntos.click(function (e) {
                fncLimpiarCrearEditarSubconjunto();
            });

            btnCrearEditarSubconjuntoCatSubconjuntos.click(function (e) {
                fncCrearEditarSubconjunto();
            });
            //#endregion

            //#region EVENTOS REQUISICIONES
            btnCrearEditarCancelarNumRequisicion.click(function (e) {
                fncLimpiarCrearEditarRequisicion();
            });

            btnVerificarRequisiciones.click(function (e) {
                // txtNumRequisicion.val("");
                // btnCrearEditarNumRequisicion.text("Guardar");
                // btnCrearEditarNumRequisicion.attr("data-id", 0);
                fncGetLstReqCC();
                mdlLstReqCC.modal("show");
            });

            btnCrearRequisicion.click(function () {
                Alert2AccionConfirmar("¡Cuidado!", "¿Desea redirigirse a <b>Compras</b> para crear una <b>Requisición</b>?", "Confirmar", "Cancelar", () => fncPrecargarRequisicion());
            });

            btnAbrirModalRequisicionManual.click(function () {
                lblTitleCrearEditarNumRequisicionManual.html("Agregar requisición existente");
                lblBtnCrearEditarNumRequisicionManual.html("Guardar");
                txtCrearEditarNumRequisicionManual.val("");
                modalCrearEditarNumRequisicionManual.modal("show");
            });

            btnCrearEditarNumRequisicionManual.click(function () {
                fncCrearEditarNumRequisicionManual();
            });

            btnAbrirModalTraspaso.click(function () {
                fncGetFoliosTraspasos();
                mdlCETraspaso.modal("show");
            });

            btnShowMdlCEFolioTraspaso.click(function () {
                lblTitleCETraspasoFolio.html("Guardar traspaso folio");
                lblBtnCETraspasoFolio.html("Guardar");
                btnCETraspasoFolio.attr("data-id", 0);
                txtCETraspasoFolioAlmacen.val("");
                txtCETraspasoFolioAlmacenDescripcion.val("");
                txtCETraspasoFolio.val("");
                txtCETraspasoFolioAlmacenDestino.val("");
                txtCETraspasoFolioAlmacenDescripcionDestino.val("");
                txtCETraspasoFolioCC.val("");
                // txtCETraspasoFolioCCDestino.val("");
                txtCETraspasoFolioNumero.val("");
                mdlCETraspasoFolio.modal("show");
            });

            btnCETraspasoFolio.click(function () {
                fncCrearEditarTraspasoFolio();
            });

            txtCETraspasoFolioAlmacen.change(function () {
                let almacenID = $(this).val();
                $.post('/Enkontrol/Almacen/GetNuevaSalidaConsultaTraspaso', { almacenID })
                    .then(response => {
                        if (response.success) {
                            txtCETraspasoFolioAlmacenDescripcion.val(response.almacenDesc);
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            });

            txtCETraspasoFolioAlmacenDestino.change(function () {
                let almacenID = $(this).val();
                $.post('/Enkontrol/Almacen/GetNuevaSalidaConsultaTraspaso', { almacenID })
                    .then(response => {
                        if (response.success) {
                            txtCETraspasoFolioAlmacenDescripcionDestino.val(response.almacenDesc);
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            });

            txtCETraspasoFolioAlmacen.click(function () {
                $(this).select();
            });

            txtCETraspasoFolioAlmacenDestino.click(function () {
                $(this).select();
            });

            $("#btnVerificarFolio").click(function () {
                fncVerificarFolios();
            });
            //#endregion

            //#region EVENTOS ORDENES DE COMPRA
            btnCrearEditarOC.click(function (e) {
                fncObtenerOCGuardarJS();
            });
            //#endregion

            //#region EVENTOS EVIDENCIAS
            btnMdlEvidencias.click(function (e) {
                lblNombreArchivo.text('Ningún archivo seleccionado');
                // btnCargarEvidenciaFile.css("display", "none");
                mdlEvidencias.modal("show");
            });

            btnCargarEvidencia.click(function (e) {
                btnCargarEvidenciaFile.trigger("click");
            });

            btnCargarEvidenciaFile.change(function (e) {
                lblNombreArchivo.text($(this)[0].files[0].name);
            });

            btnGuardarEvidencia.click(function (e) {
                subiendoArchivo();
            });
            //#endregion
        }

        function fncVerificarFolios() {
            let obj = new Object();
            obj = {
                areaCuenta: cboProyecto.val()
            }
            axios.post("VerificarTraspasosBL", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Éxito");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncPrecargarRequisicion() {
            let idBL = btnCrearEditarNumRequisicion.attr("data-BL");
            document.location.href = `/Enkontrol/Requisicion/Solicitar?idBL=${idBL}`;
        }

        function fncPrecargarOT(idBL) {
            let url = `/OT/CapturaOT?idBL=${idBL}`;
            let win = window.open(url, '_blank');
            win.focus();
        }

        function fncHabilitarDeshabilitarFormulario() {
            let idProyecto = cboProyecto.val();

            if (idProyecto == "") {
                fncCtrlsDeshabilitados();
                lblCantBL.html(``);
            } else {
                fncCtrlsHabilitados();
            }
        }

        function fncCtrlsHabilitados() {
            cboCC.attr("disabled", false);
            txtFecha.attr("disabled", false);
            txtHoras.attr("disabled", false);
            txtDescripcion.attr("disabled", false);
            chkParte.attr("disabled", false);
            chkMO.attr("disabled", false);
            cboConjunto.attr("disabled", false);
            cboSubconjunto.attr("disabled", false);
            btnLimpiarFormCrearEditarBackLog.attr("disabled", false);
            btnCrearEditarBackLog.attr("disabled", false);
            cboUsuarioResponsable.attr("disabled", false);
            btnMdlEvidencias.attr("disabled", false);
        }

        function fncCtrlsDeshabilitados() {
            cboCC.attr("disabled", true);
            txtFecha.attr("disabled", true);
            txtHoras.attr("disabled", true);
            txtDescripcion.attr("disabled", true);
            chkParte.attr("disabled", true);
            chkMO.attr("disabled", true);
            cboConjunto.attr("disabled", true);
            cboSubconjunto.attr("disabled", true);
            btnModalAbrirParte.attr("disabled", true);
            btnModalAbrirManoObra.attr("disabled", true);
            btnLimpiarFormCrearEditarBackLog.attr("disabled", true);
            btnCrearEditarBackLog.attr("disabled", true);
            cboUsuarioResponsable.attr("disabled", true)
            btnMdlEvidencias.attr("disabled", true);
        }

        function fncFillCombos() {
            if (_empresaActual == 6) {
                cboProyecto.fillCombo("FillCboAC", {}, false);
            } else {
                cboProyecto.fillCombo("/CatObra/cboCentroCostosUsuarios", {}, false);
            }
            cboProyecto.select2({ width: "resolve" });
            cboAnio.select2();
            fncFillCboConjunto();
        }

        function fncFillCboCC(areaCuenta) {
            cboCC.fillCombo("/BackLogs/FillCboCC", { areaCuenta: areaCuenta, esObra: true }, false);
            cboCC.select2({
                width: "resolve"
            });
        }

        function fncFillCboConjunto() {
            cboConjunto.fillCombo("/BackLogs/FillCboConjunto", {}, false);
            cboConjuntoCatSubconjuntos.fillCombo("/BackLogs/FillCboConjunto", {}, false);
            cboConjunto.select2({
                width: "resolve"
            });
            cboSubconjunto.select2({
                width: "resolve"
            });
        }

        function fncFillCboSubconjuntos() {
            cboSubconjunto.fillCombo("/BackLogs/FillCboSubconjunto", { idConjunto: cboConjunto.val() }, false);
        }

        //#region FUNCIONES BACKLOGS
        function fncCancelarRequisicion(strMotivo) {
            let obj = new Object();
            obj = {
                idBL: btnCrearEditarNumRequisicion.attr("data-BL"),
                motivo: strMotivo
            };
            axios.post("CancelarRequisicion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Éxito al registrar el motivo.");
                    fncGetBackLogs();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetMotivosCancelacion() {
            let obj = new Object();
            obj = {
                idBL: btnCrearEditarNumRequisicion.attr("data-BL")
            };
            axios.post("GetMotivosCancelacion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtMotivoCancelacion.clear();
                    dtMotivoCancelacion.rows.add(response.data.lstMotivoCancelacionReq);
                    dtMotivoCancelacion.draw();
                    if (response.data.lstMotivoCancelacionReq.length > 0) {
                        btnCrearRequisicion.attr("disabled", true);
                    } else {
                        btnCrearRequisicion.attr("disabled", false);
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaMotivoCancelacionReq() {
            dtMotivoCancelacion = tblBL_MotivoCancelacionReq.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'motivo', title: 'Motivo' },
                    { data: 'usuario', title: 'Usuario cancelación' },
                    {
                        data: 'fechaCreacion', title: 'Fecha cancelación',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    // CODE ...
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetCatInsumos() {
            let objFiltro = new Object();
            objFiltro = {
                insumo: txtFiltroInsumo.val(),
                PERU_insumo: txtFiltroInsumo.val(),
                descripcion: txtFiltroDescripcion.val()
            };
            axios.post("GetInsumos", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtInsumos.clear();
                    dtInsumos.rows.add(response.data.lstInsumos);
                    dtInsumos.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaCatInsumos() {
            dtInsumos = tblCatInsumos.DataTable({
                language: dtDicEsp,
                destroy: false,
                ordering: true,
                paging: true,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        title: 'Insumo',
                        render: (data, type, row, meta) => {
                            if (_empresaActual == 6) {
                                return row.PERU_insumo;
                            } else {
                                return row.insumo;
                            }
                        }
                    },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary seleccionarInsumo"><i class="fas fa-arrow-circle-right"></i></button>`;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblCatInsumos.on('click', '.seleccionarInsumo', function () {
                        let rowData = dtInsumos.row($(this).closest('tr')).data();
                        if (_empresaActual == 6) {
                            txtInsumo.val(rowData.PERU_insumo);
                        } else {
                            txtInsumo.val(rowData.insumo);
                        }

                        txtArticulo.val(rowData.descripcion);
                        txtUnidad.val(rowData.unidad)
                        dtInsumos.clear();
                        dtInsumos.draw();
                        mdlCatalogoInsumos.modal("hide");
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncReordenarPartidasPartes() {
            let i = 0;
            let x = 0;
            tblBL_Partes.find("tbody tr").each(function (index) {
                x++;
                $(this).attr("id", x);
                $(this).children("td").each(function (index2) {
                    switch (index2) {
                        case 0:
                            i++;
                            Partida = $(this).text();
                            $(this).html(i);
                            break;
                    }
                })
            });
        }

        function fncGetDatosPartes() {
            let PartidasPartes = 0;
            let Partida = "";
            let Insumo = "";
            let Cantidad = "";
            let Parte = "";
            let Articulo = "";
            arrPartes = [];

            $("#tblBL_Partes tbody tr").each(function (index) {
                $(this).children("td").each(function (index2) {
                    switch (index2) {
                        case 0:
                            PartidasPartes++;
                            Partida = $(this).text();
                            break;
                        case 1:
                            Insumo = $(this).text();
                            break;
                        case 2:
                            Cantidad = $(this).text();
                            break;
                        case 3:
                            Parte = $(this).text();
                            break;
                        case 4:
                            Articulo = $(this).text();
                            break;
                    }
                })

                let objJS = {};
                objJS.Insumo = Insumo;
                objJS.Cantidad = Cantidad;
                objJS.Parte = Parte;
                objJS.Articulo = Articulo;
                arrPartes.push(objJS);
            });
        }

        function fncReordenarPartidasManoObra() {
            let Partida;
            let i = 0;
            let x = 0;
            tblBL_ManoObra.find("tbody tr").each(function (index) {
                x++;
                let manoObraID = "manoObraID" + x;
                $(this).attr("id", manoObraID);
                $(this).children("td").each(function (index2) {
                    switch (index2) {
                        case 0:
                            i++;
                            Partida = $(this).text();
                            $(this).html(i);
                            break;
                        case 1:
                            Descripcion = $(this).text();
                            break;
                    }
                })
            });
        }

        function fncGetDatosManoObra() {
            let PartidasManoObra = 0;
            let Descripcion = "";
            arrManoObra = [];

            $("#tblBL_ManoObra tbody tr").each(function (index) {
                $(this).children("td").each(function (index2) {
                    switch (index2) {
                        case 0:
                            PartidasManoObra++;
                            Partida = $(this).text();
                            break;
                        case 1:
                            Descripcion = $(this).text();
                            break;
                    }
                })

                let objJS = {};
                objJS.Descripcion = Descripcion;
                arrManoObra.push(objJS);
            });
        }

        function fncMaxLength() {
            txtFecha.attr("maxlength", "10");
            txtDescripcion.attr("maxlength", "500");
            txtParte.attr("maxlength", "250");
            txtArticulo.attr("maxlength", "250");
            txtUnidad.attr("maxlength", "10");
            txtDescripcionManoObra.attr("maxlength", "500");
        }

        function fncGetDatosCC() {
            fncGetDatosMaquina();
            fncGetHorometroActual();
            fncGetCantBLObligatorios();
        }

        function fncDatepicker() {
            txtFecha.datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: "1900:2030"
            }).datepicker("setDate", new Date());
        }

        function fncDeshabilitarAutocomplete() {
            //#region BACKLOGS
            txtInsumo.attr("autocomplete", "off");
            txtCantidad.attr("autocomplete", "off");
            txtFecha.attr("autocomplete", "off");
            txtHoras.attr("autocomplete", "off");
            txtParte.attr("autocomplete", "off");
            txtArticulo.attr("autocomplete", "off");
            txtUnidad.attr("autocomplete", "off");
            txtNumRequisicion.attr("autocomplete", "off");
            //#endregion

            //#region CATALOGO CONJUNTOS
            txtConjuntoCatConjuntos.attr("autocomplete", "off");
            //#endregion

            //#region TRASPASOS
            txtCETraspasoFolioAlmacen.attr("autocomplete", "off");
            txtCETraspasoFolio.attr("autocomplete", "off");
            //#endregion
        }

        function fncAddPlaceholders() {
            cboCC.attr("placeholder", "Ingresar cc.");
            txtFecha.attr("placeholder", "Seleccionar fecha.");
            txtHoras.attr("placeholder", "Ingresar horas.");
            txtDescripcion.attr("placeholder", "Ingresar descripción.");
            cboConjunto.attr("placeholder", "Seleccionar conjunto.");
            cboSubconjunto.attr("placeholder", "Seleccionar subconjunto.");

            txtInsumo.attr("placeholder", "Ingresar insumo.");
            txtCantidad.attr("placeholder", "Ingresar cantidad.");
            txtParte.attr("placeholder", "Ingresar parte.");
            txtArticulo.attr("placeholder", "Ingresar artículo.");
            txtUnidad.attr("placeholder", "Ingresar unidad.");
        }

        function fncSoloNumeros() {
            txtCantidad.keypress(function (e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57 || e.which == 46 || e.which == 8)) {
                    return false;
                }
            });
        }

        function fncColorearCeldaEstatus() {
            tblBL_CatBackLogs.find("tbody tr").each(function (index) {
                $(this).addClass("rowHoverRegistroObra");
                $(this).children("td").each(function (index2) {
                    switch (index2) {
                        case 8:
                            let Estatus = $(this).html();
                            switch (Estatus) {
                                case "20%":
                                    $(this).addClass("boton-estatus-20 text-color");
                                    break;
                                case "40%":
                                    $(this).addClass("boton-estatus-40 text-color");
                                    break;
                                case "50%":
                                    $(this).addClass("boton-estatus-50 text-color");
                                    break;
                                case "60%":
                                    $(this).addClass("boton-estatus-60 text-color");
                                    break;
                                case "80%":
                                    $(this).addClass("boton-estatus-80 text-color");
                                    break;
                                case "90%":
                                    $(this).addClass("boton-estatus-90 text-color");
                                    break;
                                default:
                                    $(this).addClass("boton-estatus-100 text-color");
                                    break;
                            }
                    }
                })
            });
        }

        function fncLimpiarCtrlsCrearEditarBackLog() {
            $("input[type='text']").val("");
            txtDescripcion.val("");

            tblBL_Partes.hide();
            chkParte.prop("checked", false);
            btnModalAbrirParte.attr("disabled", true);

            tblBL_ManoObra.hide();
            chkMO.prop("checked", false);
            btnModalAbrirManoObra.attr("disabled", true);

            cboSubconjunto.empty().append("");

            // fncFillCboCC();
            cboCC[0].selectedIndex = 0;
            cboCC.trigger("change");
            cboCC.attr("disabled", false);
            fncFillCboConjunto();
            fncDatepicker();

            btnCrearEditarBackLog.attr("data-id", "0");
            btnCrearEditarBackLog.html("<i class='fas fa-save'></i>&nbsp;Guardar");

            tblBL_Partes_tbody.empty();
            tblBL_ManoObra_tbody.empty();
        }

        function fncLimpiarCtrlsCrearEditarParte() {
            txtInsumo.val("");
            txtCantidad.val("");
            txtParte.val("");
            txtArticulo.val("");
            txtUnidad.val("");

            btnCrearEditarParte.attr("data-id", "0");
        }

        function fncLimpiarCtrlsCrearEditarManoObra() {
            btnCrearEditarManoObra.attr("data-id", 0);
            txtDescripcionManoObra.val("");
        }

        function fncReadOnly() {
            txtModelo.attr("readonly", true);
            txtGrupo.attr("readonly", true);
            txtFolio.attr("readonly", true);
        }

        function fncCrearParte() {
            let esCrearEditar = fncValidarCrearEditarParte();
            let existoRegistro = false;
            if (esCrearEditar && rowEditarParte == 0) {
                var rowCount = $("#tblBL_Partes tbody tr").length;
                rowCount++;

                let tr = $("<tr>", { id: rowCount, class: "ContPartidasParte" });
                let tdContPartidasParte = $("<td>", { text: rowCount });
                let tdInsumo = $("<td>", { text: txtInsumo.val() });
                let tdCantidad = $("<td>", { text: txtCantidad.val() });
                let tdParte = $("<td>", { text: txtParte.val() });
                let tdArticulo = $("<td>", { text: txtArticulo.val() });
                let tdUnidad = $("<td>", { text: txtUnidad.val() });
                let td = $("<td>", { text: "" });
                let btnEditar = $("<button> ", { class: "btn btn-editar btn-xs btn-warning btnEditarParte" });
                let iEditar = $("<i>", { class: "fas fa-pencil-alt" });
                let btnEliminar = $("<button>", { class: "btn btn-eliminar btn-xs btn-danger btnEliminarParte" });
                let iEliminar = $("<i>", { class: "fas fa-trash" });

                td.append(btnEditar);
                td.append(btnEliminar);
                btnEditar.append(iEditar);
                btnEliminar.append(iEliminar);

                tr.append(tdContPartidasParte, tdInsumo, tdCantidad, tdParte, tdArticulo, tdUnidad, td);
                tr.data = {
                    contadorPartida: ContPartidasParte,
                    insumo: txtInsumo.val(),
                    cantidad: txtCantidad.val(),
                    parte: txtParte.val(),
                    articulo: txtArticulo.val(),
                    unidad: txtUnidad.val()
                };
                tblBL_Partes_tbody.append(tr);
                existoRegistro = true;
            } else if (esCrearEditar) {
                $("#" + rowEditarParte + "").closest("tr").find("td").eq(1).html(txtInsumo.val().trim());
                $("#" + rowEditarParte + "").closest("tr").find("td").eq(2).html(txtCantidad.val().trim());
                $("#" + rowEditarParte + "").closest("tr").find("td").eq(3).html(txtParte.val().trim());
                $("#" + rowEditarParte + "").closest("tr").find("td").eq(4).html(txtArticulo.val().trim());
                $("#" + rowEditarParte + "").closest("tr").find("td").eq(5).html(txtUnidad.val().trim());
                rowEditarParte = 0;
                existoRegistro = true;
            }
            if (existoRegistro) { modalCrearParte.modal("hide"); }
        }

        function fncValidarCrearEditarParte() {
            let esCrearEditar = true;
            let strMensajeError = "";
            if (txtInsumo.val() == "") { strMensajeError += "Es necesario ingresar el insumo."; }
            if (txtCantidad.val() == "") { strMensajeError += "Es necesario ingresar la cantidad."; }
            if (txtParte.val() == "") { strMensajeError += "<br>Es necesario ingresar la parte."; }
            if (txtArticulo.val() == "") { strMensajeError += "<br>Es necesario ingresar el artículo."; }
            if (txtUnidad.val() == "") { strMensajeError += "<br>Es necesario ingresar la unidad."; }
            if (strMensajeError != "") {
                esCrearEditar = false;
                Alert2Warning(strMensajeError);
            }
            return esCrearEditar;
        }

        function fncCrearManoObra() {
            let esCrearEditar = fncValidarCrearEditarManoObra();
            let existoRegistro = false;
            if (esCrearEditar && rowEditarManoObra == 0) {
                var rowCount = $("#tblBL_ManoObra tbody tr").length;
                rowCount++;

                let manoObraID = "manoObraID" + rowCount;

                let tr = $("<tr>", { id: manoObraID, class: "ContPartidasManoObra" });
                let tdContPartidasManoObra = $("<td>", { text: rowCount });
                let tdDescripcion = $("<td>", { text: txtDescripcionManoObra.val() });
                let td = $("<td>");
                let btnEditar = $("<button>", { class: "btn btn-editar btn-xs btn-warning btnEditarManoObra" });
                let iEditar = $("<i>", { class: "fas fa-pencil-alt" });
                let btnEliminar = $("<button>", { class: "btn btn-eliminar btn-xs btn-danger btnEliminarManoObra" });
                let iEliminar = $("<i>", { class: "fas fa-trash" });

                td.append(btnEditar);
                td.append(btnEliminar);
                btnEditar.append(iEditar);
                btnEliminar.append(iEliminar);

                tr.append(tdContPartidasManoObra, tdDescripcion, td);
                tr.data = {
                    contadorPartida: ContPartidasManoObra,
                    descripcion: txtDescripcionManoObra.val()
                };
                tblBL_ManoObra_tbody.append(tr);
                existoRegistro = true;
            } else if (esCrearEditar) {
                let row = "manoObraID" + rowEditarManoObra;
                $("#" + row + "").closest("tr").find("td").eq(1).html(txtDescripcionManoObra.val().trim());
                rowEditarManoObra = 0;
                existoRegistro = true;
            }
            if (existoRegistro) { modalCrearManoObra.modal("hide"); }
        }

        function fncValidarCrearEditarManoObra() {
            let esCrearEditar = true;
            let strMensajeError = "";
            if (txtDescripcionManoObra.val() == "") { strMensajeError += "Es necesario ingresar descripción de mano de obra."; }
            if (strMensajeError != "") {
                esCrearEditar = false;
                Alert2Warning(strMensajeError);
            }
            return esCrearEditar;
        }

        function fncCrearEditarBackLog() {
            let esCrearEditar = fncValidarCrearEditarBL();
            if (esCrearEditar) {
                let objBL = fncGetDatosFormulario();
                if (objBL != null) {
                    axios.post("CrearEditarBackLog", objBL).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            Alert2Exito("Se registro correctamente el BackLog.");
                            fncLimpiarCtrlsCrearEditarBackLog();
                            fncGetBackLogs();
                            $(".select-checkbox").prop("checked", false);
                        } else {
                            Alert2Error("Ocurrió un error al registrar el BackLog.");
                        }
                    }).catch(error => Alert2Error(error.message));
                }
            }
        }

        function fncValidarCrearEditarBL() {
            let strMensajeError = "";
            let esCrearEditar = true;
            if (cboCC.val() == "") { strMensajeError += "Es necesario seleccionar una máquina."; }
            if (txtFecha.val() == "") { strMensajeError += "<br>Es necesario ingresar la fecha de inspección."; }
            if (txtHoras.val() == "") { strMensajeError += "<br>Es necesario ingresar las horas."; }
            if (txtDescripcion.val() == "") { strMensajeError += "<br>Es necesario ingresar la descripción."; }
            if (cboConjunto.val() == "") { strMensajeError += "<br>Es necesario seleccionar un conjunto."; }
            if (cboSubconjunto.val() == "" || cboSubconjunto.val() == null) { strMensajeError += "<br>Es necesario seleccionar un subconjunto."; }
            if (cboUsuarioResponsable.val() <= 0) { strMensajeError += "<br>Es necesario seleccionar al responsable."; }
            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                esCrearEditar = false;
            }
            return esCrearEditar;
        }

        function fncGetDatosFormulario() {
            fncGetDatosPartes();
            fncGetDatosManoObra();

            let optionNoEconomico = cboCC.find(`option[value="${cboCC.val()}"]`);
            let prefijoNoEconomico = optionNoEconomico.attr("data-prefijo");

            let objBL = {
                id: btnCrearEditarBackLog.attr("data-id"),
                folio: txtFolio.val().trim(),
                fechaInspeccion: txtFecha.val().trim(),
                cc: cboCC.val(),
                noEconomico: prefijoNoEconomico,
                horas: txtHoras.val().trim(),
                idSubconjunto: cboSubconjunto.val(),
                parte: chkParte.prop("checked"),
                manoObra: chkMO.prop("checked"),
                descripcion: txtDescripcion.val().trim(),
                areaCuenta: cboProyecto.val()
            };

            return {
                objBL,
                datosPartes: arrPartes,
                datosManoObra: arrManoObra,
                esParte: chkParte.prop("checked"),
                esManoObra: chkMO.prop("checked"),
                esActualizarCC: cboCC.attr("esActualizarCC"),
                esObra: true,
                idUsuarioResponsable: cboUsuarioResponsable.val()
            };
        }

        function initTablaBackLogs() {
            dtBackLogs = tblBL_CatBackLogs.DataTable({
                paging: true,
                searching: true,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: "folioBL", title: "Folio" },
                    {
                        title: "Fecha de<br>elaboración",
                        render: function (data, type, row) {
                            return moment(row.fechaInspeccion).format('DD/MM/YYYY');
                        }
                    },
                    {
                        title: "Fecha<br>instalación",
                        render: function (data, type, row) {
                            let fecha = moment(row.fechaModificacionBL).format('DD/MM/YYYY');
                            if (fecha != "01/01/0001") {
                                return fecha;
                            } else {
                                return "";
                            }
                        }
                    },
                    { data: "noEconomico", title: "C.C." },
                    { data: "horas", title: "Horas" },
                    { data: "conjunto", title: "Conjunto" },
                    { data: "subconjunto", title: "Subconjunto" },
                    {
                        data: "descripcion", title: "Descripción",
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary btn-xs verDescripcionBL" title="Descripción del BL.">Descripción BL</button>`;
                        }
                    },
                    {
                        data: "estatus", title: "Estatus",
                        render: function (data, type, row) {
                            switch (data) {
                                case "Elaboración de Inspección (20%)":
                                    return "20%";
                                case "Elaboración de Requisición (40%)":
                                    return "40%";
                                case "Elaboración de OC (50%)":
                                    return `50%`;
                                case "Suministro de Refacciones (60%)":
                                    return "60%";
                                case "Rehabilitación Programada (80%)":
                                    return "80%";
                                case "Proceso de Instalación (90%)":
                                    return "90%";
                                case "BackLogs instalado (100%)":
                                    return "<i class='fas fa-check'></i>";
                            }
                        }
                    },
                    {
                        render: function (data, type, row) {
                            let btnAcciones = "";
                            let btnActualizarBL = `<button class='btn-editar btn btn-xs btn-warning editarBackLog' title='Actualizar BackLog.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminarBL = `<button class='btn-eliminar btn btn-xs btn-danger eliminarBackLog' title='Eliminar BackLog.'><i class='fas fa-trash'></i></button>&nbsp;`;
                            let btnOrdenBL = `<button class='btn btn-xs btn-primary imprimirOrdenBL' title='Imprimir orden BL.'><i class='fas fa-print'></i></button>&nbsp;`;
                            let btnEvidencias = `<button type="button" class="btn btn-primary btn-xs cargarEvidencia" title="Carga de evidencia."><i class="far fa-folder-open"></i></button>&nbsp;`;
                            let btnRequisiciones = `<button class='btn-editarRequisicion btn btn-xs btn-success editarRequisicion' title='Actualizar número de requisición. (40%)'><i class="fas fa-list-ul"></i></button>&nbsp;`;
                            let btnOC = `<button class='btn-editarOC btn btn-xs btn-success editarOC' title='Actualizar número de OC. (50%)'><i class='fas fa-shopping-cart'></i></button>&nbsp;`;
                            let btnConfirmRehabilitacionProgramada = `<button class="btn btn-xs btn-primary confirmarRehabilitacionProgramada" title="Confirmar que los suministros llegaron a almacen. (60% - 80%)"><i class="fas fa-truck-loading"></i></button>&nbsp;`;
                            let btnProcesoInstalacion = `<button class="btn btn-xs btn-primary confirmarProcesoInstalacion" title="Confirmar que ya se dio salida las piezas de almacen. (90%)"><i class="fas fa-sign-out-alt"></i></button>&nbsp;`;
                            let btnCrearOT = `<button class="btn btn-xs btn-primary crearOT" title="Crear OT."><i class="fas fa-scroll"></i></button>&nbsp;`;
                            let btnImprimirOTVacia = `<button class="btn btn-xs btn-primary imprimirOTVacia" title="Imprimir OT vacía."><i class="far fa-sticky-note"></i></button>&nbsp;`;
                            let btnImprimirOT = `<button class="btn btn-xs btn-primary imprimirOT" title="Imprimir OT."><i class="fas fa-print"></i></button>&nbsp;`;
                            let btnEstatusBL90a80 = `<button class="btn btn-xs btn-warning regresarBL90a80" title="Regresar BL al 80%"><i class="fas fa-arrow-circle-left"></i></button>&nbsp;`;
                            let btnBackLogInstalado = `<button class='btn btn btn-xs btn-success confirmarBackLogInstalado' title="Confirmar instalación del BackLog."><i class="fas fa-check-double"></i></button>&nbsp;`;

                            switch (row.estatus) {
                                case "Elaboración de Inspección (20%)":
                                    btnAcciones += btnActualizarBL + btnEliminarBL + btnOrdenBL + btnEvidencias + btnRequisiciones + btnImprimirOTVacia;
                                    break;
                                case "Elaboración de Requisición (40%)":
                                    btnAcciones += btnActualizarBL + btnEliminarBL + btnOrdenBL + btnEvidencias + btnRequisiciones + btnOC + btnConfirmRehabilitacionProgramada + btnImprimirOTVacia;
                                    break;
                                case "Elaboración de OC (50%)":
                                    btnAcciones += btnActualizarBL + btnEliminarBL + btnOrdenBL + btnEvidencias + btnRequisiciones + btnOC + btnConfirmRehabilitacionProgramada;
                                    break;
                                case "Suministro de Refacciones (60%)":
                                    btnAcciones += btnActualizarBL + btnEliminarBL + btnOrdenBL + btnEvidencias + btnRequisiciones + btnOC + btnConfirmRehabilitacionProgramada;
                                    break;
                                case "Rehabilitación Programada (80%)":
                                    btnAcciones += btnActualizarBL + btnEliminarBL + btnOrdenBL + btnEvidencias + btnRequisiciones + btnOC + btnConfirmRehabilitacionProgramada + btnImprimirOTVacia + btnProcesoInstalacion;
                                    break;
                                case "Proceso de Instalación (90%)":
                                    btnAcciones += btnActualizarBL + btnEliminarBL + btnOrdenBL + btnEvidencias + btnRequisiciones + btnOC + btnCrearOT + btnImprimirOT + btnEstatusBL90a80 + btnBackLogInstalado;
                                    break;
                                case "BackLogs instalado (100%)":
                                    btnAcciones += btnEvidencias + btnRequisiciones + btnOC + btnImprimirOT;
                                    break;
                            }
                            return btnAcciones;
                        }
                    },
                    { data: 'cc', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblBL_CatBackLogs.on("click", ".lblTest", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        console.log("1");
                    });

                    tblBL_CatBackLogs.on("click", ".verDescripcionBL", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        txtDescripcionBL.val(rowData.descripcion);
                        mdlDescripcionBL.modal("show");
                    });

                    tblBL_CatBackLogs.on("click", ".editarBackLog", function () {
                        fncLimpiarCtrlsCrearEditarBackLog();

                        const rowData = dtBackLogs.row($(this).closest("tr")).data();

                        let optionNoEconomico = cboCC.find(`option[data-prefijo="${rowData.noEconomico}"]`);
                        let valNoEconomico = optionNoEconomico.val();
                        cboCC.val(valNoEconomico);
                        cboCC.trigger("change");
                        cboCC.attr("esActualizarCC", false);
                        cboCC.attr("cc", rowData.noEconomico);
                        cboCC.attr("disabled", true);

                        txtFecha.val(moment(rowData.fechaInspeccion).format("DD/MM/YYYY"));
                        txtHoras.val(rowData.horas);
                        txtFolio.val(rowData.folioBL);
                        txtDescripcion.val(rowData.descripcion);

                        chkParte.prop("checked", rowData.parte);
                        tblBL_Partes.hide();
                        if (chkParte.prop("checked")) {
                            btnModalAbrirParte.attr("disabled", false);
                            tblBL_Partes.show();
                        }

                        chkMO.prop("checked", rowData.manoObra);
                        tblBL_ManoObra.hide();
                        if (chkMO.prop("checked")) {
                            btnModalAbrirManoObra.attr("disabled", false);
                            tblBL_ManoObra.show();
                        }
                        cboConjunto.val(rowData.idConjunto);
                        cboConjunto.trigger("change");
                        cboSubconjunto.val(rowData.idSubconjunto);
                        cboSubconjunto.trigger("change");
                        btnCrearEditarBackLog.attr("data-id", rowData.id);
                        btnCrearEditarBackLog.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
                    });

                    tblBL_CatBackLogs.on("click", ".eliminarBackLog", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBackLog = parseFloat(rowData.id);
                        Alert2AccionConfirmar("¡Cuidado!", "¿Desea eliminar el registro seleccionado?", "Confirmar", "Cancelar", () => fncEliminarBackLog(idBackLog));
                    });

                    tblBL_CatBackLogs.on("click", ".editarRequisicion", function () {
                        btnCrearRequisicion.attr("disabled", false);
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        txtNumRequisicion.val(rowData.numRequisicion);
                        txtCETraspasoFolioCCDestino.val(rowData.cc);
                        btnCrearEditarNumRequisicion.attr("data-BL", rowData.id);
                        btnMotivoCancelacion.attr("data-idBL", rowData.id);
                        fncGetMotivosCancelacion();
                        fncGetRequisiciones();
                        modalCrearEditarNumRequisicion.modal("show");
                    });

                    tblBL_CatBackLogs.on("click", ".editarOC", function () {
                        divOrdenesCompra.html('');
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        btnVerificarOrdenesCompra.attr("data-id", rowData.id);
                        fncGetOC();
                        modalCrearEditarNumOC.modal("show");
                    });

                    tblBL_CatBackLogs.on("click", ".confirmarRehabilitacionProgramada", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("Programación de BackLogs (80%)", "Confirmar que las refacciones llegaron al almacen.",
                            "Confirmar", "Cancelar", () => fncConfirmarRehabilitacionProgramada(idBL));
                    });

                    tblBL_CatBackLogs.on("click", ".confirmarProcesoInstalacion", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("Proceso de Instalacion (90%)", "Confirmar que ya se dio salida las piezas de almacen.", "Confirmar", "Cancelar",
                            () => fncConfirmarProcesoInstalacion(idBL));
                    });

                    tblBL_CatBackLogs.on("click", ".crearOT", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("Captura OT", "¿Desea capturar la <b>Orden de trabajo</b>?", "Confirmar", "Cancelar", () => fncPrecargarOT(idBL));
                    });

                    tblBL_CatBackLogs.on("click", ".imprimirOT", function (e) {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("Imprimir OT", "¿Desea imprimir la <b>Orden de trabajo</b>?", "Confirmar", "Cancelar", () => fncImprimirOT(idBL));
                    });

                    tblBL_CatBackLogs.on("click", ".imprimirOrdenBL", function (e) {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("Imprimir orden BL", "¿Desea imprimir la <b>Orden de BackLog</b>?", "Confirmar", "Cancelar", () => fncImprimirOrdenBL(idBL));
                    });

                    tblBL_CatBackLogs.on("click", ".imprimirOTVacia", function (e) {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("Imprimir OT vacía", "¿Desea imprimir la <b>Orden de trabajo</b>?", "Confirmar", "Cancelar", () => fncImprimirOTVacia(idBL));
                    });

                    tblBL_CatBackLogs.on("click", ".confirmarBackLogInstalado", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("BackLog instalado (100%)", "Confirmar la instalación del BackLog.", "Confirmar", "Cancelar",
                            () => fncConfirmarBackLogInstalado(idBL));
                    });

                    tblBL_CatBackLogs.on('click', '.cargarEvidencia', function () {
                        let rowData = dtBackLogs.row($(this).closest("tr")).data();
                        $("#rowDataId").val(rowData.id);
                        btnGuardarEvidencia.attr('data-id', $('.subirArchivos').attr("data-id"));
                        $("#titleCurso").empty();
                        $('#inputExamen').val('');
                        $('#lblTexto1').text('Ningún archivo seleccionado');
                        $('#inputExamen').change(function () {
                            $('#lblTexto1').text($(this)[0].files[0].name);
                        });
                        cboMdlEvidenciasTipoEvidencia[0].selectedIndex = 0;
                        cboMdlEvidenciasTipoEvidencia.trigger("change");
                        btnGuardarEvidencia.attr("data-bl", rowData.id);
                        $('#mdlEvidencias').modal('show');
                        fncGetArchivos(rowData.id);
                    });

                    tblBL_CatBackLogs.on("click", ".regresarBL90a80", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        Alert2AccionConfirmar("Estatus BL 90% a 80%", "¿Desea cambiar el estatus del BL 90% a 80%?", "Confirmar", "Cancelar", () => fncCambiarEstatusBL90a80(rowData.id));
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": [1, 2, 3, 4, 5, 6, 7, 9] }
                    // { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function fncCambiarEstatusBL90a80(idBL) {
            let obj = new Object();
            obj = {
                idBL: idBL
            }
            axios.post("CambiarEstatusBL90a80", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha actualizado el BL de 90% a 80% con éxito.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncImprimirOT(idBL, esVacia) {
            let obj = new Object();
            obj = {
                idBL: idBL
            }
            axios.post("GetIDOT", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let idOT = response.data.idOT;
                    if (idOT > 0 && idBL > 0) {
                        var path = `/Reportes/Vista.aspx?idReporte=45&idOT=${idOT}&idBL=${idBL}`;
                        $("#report").attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            openCRModal();
                        };
                    } else {
                        Alert2Error("Ocurrió un error al generar el reporte.");
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncImprimirOrdenBL(idBL) {
            let obj = new Object();
            obj = {
                idBL: idBL
            }
            var path = `/Reportes/Vista.aspx?idReporte=224&idBL=${idBL}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function fncImprimirOTVacia(idBL) {
            var path = `/Reportes/Vista.aspx?idReporte=45&idOT=${0}&idBL=${idBL}&esVacia=${1}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function fncConfirmarRehabilitacionProgramada(idBL) {
            let obj = {};
            obj = {
                idBL: idBL
            };
            axios.post("ConfirmarRehabilitacionProgramada", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito(`Se ha actualizado el estatus del BackLog a "Rehabilitacion Programada (80%)".`);
                    fncGetBackLogs();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncConfirmarProcesoInstalacion(idBL) {
            let obj = {};
            obj = {
                idBL: idBL
            };
            axios.post("ConfirmarProcesoInstalacion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito(`Se ha actualizado el estatus del BackLog a "Proceso de Instalacion (90%)".`);
                    fncGetBackLogs();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncConfirmarBackLogInstalado(idBL) {
            let obj = {};
            obj = {
                idBL: idBL
            };
            axios.post("ConfirmarBackLogInstalado", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito(`Se ha actualizado el estatus a "BackLog instalado (100%)".`);
                    fncGetBackLogs();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetPartes() {
            let idBackLog = btnCrearEditarBackLog.attr("data-id");
            let objBL = new Object();
            objBL = {
                idBackLog: idBackLog
            };
            axios.post("GetPartes", objBL).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    tblBL_Partes_tbody.empty();
                    dtPartes.clear();
                    dtPartes.rows.add(response.data.lstPartes);
                    dtPartes.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaPartes() {
            dtPartes = tblBL_Partes.DataTable({
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblBL_CatBackLogs.on("click", ".editarBackLog", function () {
                        fncGetPartes();
                    });
                },
                columns: [
                    { data: "partida", title: "Partida", "bSortable": false },
                    { data: "insumo", title: "Insumo", "bSortable": false },
                    { data: "cantidad", title: "Cantidad", "bSortable": false },
                    { data: "parte", title: "Parte", "bSortable": false },
                    { data: "articulo", title: "Artículo", "bSortable": false },
                    { data: "unidad", title: "Unidad", "bSortable": false },
                    {
                        render: function (data, type, row, cliente) {
                            return "<button class='btn-editar btn btn-xs btn-warning btnEditarParte' data-id='" + row.id + "'>" +
                                "<i class='fas fa-pencil-alt'></i>" +
                                "</button>" +
                                "<button class='btn-eliminar btn btn-xs btn-danger btnEliminarParte' data-id='" + row.id + "'>" +
                                "<i class='fas fa-trash'></i>" +
                                "</button>";
                        },
                        "bSortable": false
                    }
                ]
            });

            tblBL_Partes_tbody.empty();
        }

        function initTablaManoObra() {
            dtManoObra = tblBL_ManoObra.DataTable({
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblBL_CatBackLogs.on("click", ".editarBackLog", function () {
                        fncGetManoObra();
                    });
                },
                columns: [
                    { data: "partida", title: "Partida", "bSortable": false },
                    { data: "descripcion", title: "Descripción", "bSortable": false },
                    {
                        render: function (data, type, row, cliente) {
                            return "<button class='btn-editar btn btn-xs btn-warning btnEditarManoObra' data-id='" + row.id + "'>" +
                                "<i class='fas fa-pencil-alt'></i>" +
                                "</button>" +
                                "<button class='btn-eliminar btn btn-xs btn-danger btnEliminarManoObra' data-id='" + row.id + "'>" +
                                "<i class='fas fa-trash'></i>" +
                                "</button>";
                        },
                        "bSortable": false
                    }
                ]
            });

            tblBL_ManoObra_tbody.empty();
        }

        function fncGetManoObra() {
            let idBackLog = btnCrearEditarBackLog.attr("data-id");
            let objBL = new Object();
            objBL = {
                idBackLog: idBackLog
            };
            axios.post("GetManoObra", objBL).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    tblBL_ManoObra_tbody.empty();
                    dtManoObra.clear();
                    dtManoObra.rows.add(response.data.lstManoObra);
                    dtManoObra.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetBackLogs() {
            let objBackLog = fncGetProyectoID();
            axios.post('GetBackLogsFiltros', objBackLog).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    tblBL_CatBackLogs_tbody.empty();
                    dtBackLogs.clear();
                    dtBackLogs.rows.add(response.data.lstBackLogs);
                    dtBackLogs.draw();
                    fncColorearCeldaEstatus();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetProyectoID() {
            let objBackLog = new Object();

            // let optionAreaCuenta = cboProyecto.find(`option[value="${cboProyecto.val()}"]`);
            // let prefijoAreaCuenta = optionAreaCuenta.attr("data-prefijo");
            objBackLog = {
                anio: cboAnio.val(),
                areaCuenta: cboProyecto.val(),
                esObra: true
            };
            return objBackLog;
        }

        function fncGetDatosMaquina() {
            let optionCC = cboCC.find(`option[value="${cboCC.val()}"]`);
            let prefijoCC = optionCC.attr("data-prefijo");
            let objNoEconomico = new Object();
            objNoEconomico = {
                areaCuenta: cboProyecto.val(),
                noEconomico: prefijoCC
            };
            axios.post("GetMaquina", objNoEconomico).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (response.data.lstMaquina != null) {
                        txtModelo.val(response.data.lstMaquina[0].modeloEquipo);
                        txtGrupo.val(response.data.lstMaquina[0].grupoMaquinaria);

                        let esActualizar = btnCrearEditarBackLog.attr("data-id");
                        if (esActualizar == 0) {
                            fncGetUltimoFolio();
                        }
                    } else {
                        txtModelo.val("");
                        txtGrupo.val("");
                    }
                } else {
                    Alert2Warning(response.data.message);
                    txtModelo.val("");
                    txtGrupo.val("");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetHorometroActual() {
            let optionCC = cboCC.find(`option[value="${cboCC.val()}"]`);
            let prefijoCC = optionCC.attr("data-prefijo");
            let objNoEconomico = new Object();
            objNoEconomico = {
                areaCuenta: cboProyecto.val(),
                noEconomico: prefijoCC
            };
            axios.post("GetHorometroActual", objNoEconomico).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    txtHoras.val(response.data.horometroActual);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetCantBLObligatorios() {
            let optionCC = cboCC.find(`option[value="${cboCC.val()}"]`);
            let prefijoCC = optionCC.attr("data-prefijo");
            let objNoEconomico = new Object();
            objNoEconomico = {
                areaCuenta: cboProyecto.val(),
                noEconomico: prefijoCC
            };
            axios.post("GetCantBLObligatorios", objNoEconomico).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    lblCantBL.html(` Cant. BackLogs a registrar: ${response.data.cantRegistroBL} `);
                    fncCtrlsHabilitados();
                } else {
                    lblCantBL.html(` Cant. BackLogs a registrar: 0 `);
                    fncCtrlsDeshabilitados();
                    cboCC.attr("disabled", false);
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetUltimoFolio() {
            let obj = new Object();
            obj = {
                esObra: true
            };
            axios.post("GetUltimoFolio", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (response.data.folio != null) {
                        txtFolio.val(response.data.folio);
                    } else {
                        txtFolio.val("");
                    }
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarBackLog(idBackLog) {
            if (idBackLog > 0) {
                let obj = new Object();
                obj = {
                    id: idBackLog
                };
                axios.post("EliminarBackLog", obj)
                    .catch(o_O => AlertaGeneral(o_O.message))
                    .then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            Alert2Exito("Se ha eliminado con éxito el BackLog.");
                            fncGetBackLogs();
                        } else {
                            Alert2Error(message);
                        }
                    }).catch(error => AlertaGeneral(error.message));
            } else {
                Alert2Error("Ocurrió un error al intentar eliminar el BackLog.");
            }
        }

        function fncEliminarParte() {
            let idParte = btnEliminarParte.attr("data-id");
            let obj = new Object();
            obj = {
                id: idParte
            };
            axios.post("EliminarParte", obj)
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        $("#" + idRowParte + "").remove();
                        modalEliminarParte.modal("hide");
                        fncReordenarPartidasPartes();
                    }
                }).catch(error => AlertaGeneral(error.message));
        }

        function fncEliminarManoObra(idParte) {
            let idManoObra = btnEliminarManoObra.attr("data-id");
            let obj = new Object();
            obj = {
                id: idManoObra
            };
            axios.post("EliminarManoObra", obj)
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        $("#" + idRowManoObra + "").remove();
                        modalEliminarManoObra.modal("hide");
                        fncGetManoObra();
                        fncReordenarPartidasManoObra();
                    }
                }).catch(error => AlertaGeneral(error.message));
        }

        function fncCrearEditarParte() {
            if (chkParte.prop("checked")) {
                let objParte = new Object();
                objParte = {
                    insumo: txtInsumo.val().trim(),
                    PERU_insumo: txtInsumo.val().trim(),
                    cantidad: txtCantidad.val().trim(),
                    parte: txtParte.val().trim(),
                    articulo: txtArticulo.val().trim(),
                    unidad: txtUnidad.val().trim(),
                    id: btnCrearEditarParte.attr("data-id"),
                    idBackLog: btnCrearEditarBackLog.attr("data-id")
                };
                axios.post("CrearEditarParte", objParte).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetPartes();
                        modalCrearParte.modal("hide");
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCrearEditarManoObra() {
            if (chkMO.prop("checked")) {
                let objManoObra = new Object();
                objManoObra = {
                    descripcion: txtDescripcionManoObra.val().trim(),
                    id: btnCrearEditarManoObra.attr("data-id"),
                    idBackLog: btnCrearEditarBackLog.attr("data-id")
                };
                axios.post("CrearEditarManoObra", objManoObra).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetManoObra();
                        modalCrearManoObra.modal("hide");
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }
        //#endregion

        //#region FUNCIONES CATALOGO CONJUNTOS
        function initTablaConjuntos() {
            dtConjuntos = tblConjuntosCatConjuntos.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: true,
                info: false,
                columns: [
                    { data: 'descripcion', title: 'Conjunto' },
                    { data: 'abreviacion', title: 'Abreviación' },

                    { data: 'id', title: 'id', visible: false },
                    {
                        render: function (data, type, row) {
                            let editarConjunto = `<button class='btn-editar btn btn-warning editarConjunto' data-id="${row.id}">` +
                                `<i class='fas fa-pencil-alt'></i>` +
                                `</button>&nbsp;`;
                            let eliminarConjunto = `<button class='btn-eliminar btn btn-danger eliminarConjunto' data-id="${row.id}">` +
                                `<i class="far fa-trash-alt"></i>
                                                </button>`;
                            return editarConjunto + eliminarConjunto;
                        }
                    },
                ]
                , initComplete: function (settings, json) {
                    tblConjuntosCatConjuntos.on("click", ".editarConjunto", function () {
                        let rowData = dtConjuntos.row($(this).closest('tr')).data();
                        txtConjuntoCatConjuntos.val(rowData.descripcion);
                        txtAbreviacionoCatConjuntos.val(rowData.abreviacion);
                        btnCrearEditarConjuntoCatConjuntos.attr("data-id", rowData.id);
                        if (!divCrearEditarConjuntoCatConjuntos.hasClass('in')) {
                            btnCollapseConjuntosCatConjuntos.trigger('click');
                        }
                    });

                    tblConjuntosCatConjuntos.on("click", ".eliminarConjunto", function () {
                        let rowData = dtConjuntos.row($(this).closest("tr")).data();
                        let idConjunto = parseFloat(rowData.id);
                        Alert2AccionConfirmar("¡Cuidado!", "¿Desea eliminar el registro seleccionado?", "Confirmar", "Cancelar", () => fncEliminarConjunto(idConjunto));
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
            });
        }

        function fncLimpiarCrearEditarConjunto() {
            btnCrearEditarConjuntoCatConjuntos.attr("data-id", 0);
            txtConjuntoCatConjuntos.val("");
            txtAbreviacionoCatConjuntos.val("");

        }

        function fncCrearEditarConjunto() {
            let objConjunto = fncCrearObjConjunto();
            if (objConjunto["descripcion"] != "") {
                axios.post('CrearEditarConjunto', objConjunto).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetConjuntos();
                        txtConjuntoCatConjuntos.val("");
                        txtAbreviacionoCatConjuntos.val("");
                        btnCrearEditarConjuntoCatConjuntos.attr("data-id", 0);
                        Alert2Exito("Se registro correctamente el conjunto.");
                        fncFillCboSubconjuntos();
                        fncFillCboConjunto();
                        if (cboProyecto.val() != "") {
                            fncFillCboConjunto();
                        }
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Es necesario indicar nombre del conjunto.");
            }
        }

        function fncCrearObjConjunto() {
            let objConjunto = new Object();
            let idConjunto = btnCrearEditarConjuntoCatConjuntos.attr("data-id");
            let descripcion = txtConjuntoCatConjuntos.val();
            let abreviacion = txtAbreviacionoCatConjuntos.val();

            if (descripcion == "")
                Alert2Warning("Es necesario indicar nombre del conjunto.");

            objConjunto = {
                id: idConjunto,
                descripcion: descripcion,
                abreviacion: abreviacion
            };
            return objConjunto;
        }

        function fncGetConjuntos() {
            axios.post('GetConjuntos').catch(o_O => Alert2Error(o_O.message)).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtConjuntos.clear();
                    dtConjuntos.rows.add(response.data.lstConjuntos);
                    dtConjuntos.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarConjunto(idConjunto) {
            if (idConjunto > 0) {
                let obj = new Object();
                obj = {
                    idConjunto: idConjunto
                };
                axios.post('EliminarConjunto', obj)
                    .catch(o_O => Alert2Error(o_O.message))
                    .then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            Alert2Exito("Se ha eliminado con éxito el conjunto.");
                            fncGetConjuntos();
                            if (cboProyecto.val() != "") {
                                fncFillCboConjunto();
                            }
                        } else {
                            Alert2Error(message);
                        }
                    }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al intentar eliminar el conjunto.");
            }
        }
        //#endregion

        //#region FUNCIONES CATALOGO SUBCONJUNTOS
        function initTablaSubconjuntos() {
            dtSubconjuntos = tblSubconjuntosCatSubconjuntos.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: true,
                info: false,
                columns: [
                    { data: 'conjunto', title: 'Conjunto' },
                    { data: 'descripcion', title: 'Subconjunto' },
                    { data: 'id', title: 'id', visible: false },
                    { data: 'idConjunto', title: 'idConjunto', visible: false },
                    { data: 'abreviacion', title: 'Abreviación' },
                    {
                        render: function (data, type, row) {
                            let editarSubconjunto = `<button class='btn-editar btn btn-warning editarSubconjunto' data-id="${row.id}">` +
                                `<i class='fas fa-pencil-alt'></i>` +
                                `</button>&nbsp;`;
                            let eliminarSubconjunto = `<button class='btn-eliminar btn btn-danger eliminarSubconjunto' data-id="${row.id}">` +
                                `<i class="far fa-trash-alt"></i>
                                                </button>`;
                            return editarSubconjunto + eliminarSubconjunto;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblSubconjuntosCatSubconjuntos.on("click", ".editarSubconjunto", function () {
                        let rowData = dtSubconjuntos.row($(this).closest('tr')).data();
                        cboConjuntoCatSubconjuntos.val(rowData.idConjunto);
                        cboConjuntoCatSubconjuntos.trigger("change");
                        txtSubconjuntoCatSubconjuntos.val(rowData.descripcion);
                        txtAbreviacionCatSubconjuntos.val(rowData.abreviacion);
                        btnCrearEditarSubconjuntoCatSubconjuntos.attr("data-id", rowData.id);
                        if (!divCrearEditarSubconjuntoCatSubconjuntos.hasClass('in')) {
                            btnCollapseSubconjuntosCatSubconjuntos.trigger('click');
                        }
                    });

                    tblSubconjuntosCatSubconjuntos.on("click", ".eliminarSubconjunto", function () {
                        let rowData = dtSubconjuntos.row($(this).closest("tr")).data();
                        let idSubconjunto = parseFloat(rowData.id);
                        Alert2AccionConfirmar("¡Cuidado!", "¿Desea eliminar el registro seleccionado?", "Confirmar", "Cancelar", () => fncEliminarSubconjunto(idSubconjunto));
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
            });
        }

        function fncLimpiarCrearEditarSubconjunto() {
            btnCrearEditarSubconjuntoCatSubconjuntos.attr("data-id", 0);
            txtSubconjuntoCatSubconjuntos.val("");
            txtAbreviacionCatSubconjuntos.val("");
        }

        function fncCrearEditarSubconjunto() {
            let objSubconjunto = fncCrearObjSubconjunto();
            if (objSubconjunto["idConjunto"] != "" && objSubconjunto["descripcion"] != "") {
                axios.post('CrearEditarSubconjunto', objSubconjunto).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetSubconjuntos();
                        txtSubconjuntoCatSubconjuntos.val("");
                        txtAbreviacionCatSubconjuntos.val("");
                        btnCrearEditarSubconjuntoCatSubconjuntos.attr("data-id", 0);
                        Alert2Exito("Se registro correctamente el subconjunto.");
                        if (cboProyecto.val() != "") {
                            fncFillCboSubconjuntos();
                        }
                        cboConjuntoCatSubconjuntos[0].selectedIndex = 0;
                        cboConjuntoCatSubconjuntos.trigger("change");
                        txtConjuntoCatConjuntos.val("");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensaje = "";
                if (objSubconjunto["idConjunto"] == "") { strMensaje += "Es necesario seleccionar un conjunto."; }
                if (objSubconjunto["descripcion"] == "") { strMensaje += "<br>Es necesario ingresar nombre del subconjunto."; }
                Alert2Warning(strMensaje);
            }
        }

        function fncCrearObjSubconjunto() {
            let objSubconjunto = new Object();
            let idSubconjunto = btnCrearEditarSubconjuntoCatSubconjuntos.attr("data-id");
            let descripcion = txtSubconjuntoCatSubconjuntos.val();
            let idConjunto = cboConjuntoCatSubconjuntos.val();
            let abreviacion = txtAbreviacionCatSubconjuntos.val();

            objSubconjunto = {
                id: idSubconjunto,
                descripcion: descripcion,
                idConjunto: idConjunto,
                abreviacion: abreviacion

            };
            return objSubconjunto;
        }

        function fncGetSubconjuntos() {
            axios.post('GetSubconjuntos').catch(o_O => Alert2Error(o_O.message)).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtSubconjuntos.clear();
                    dtSubconjuntos.rows.add(response.data.lstCatSubconjuntos);
                    dtSubconjuntos.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarSubconjunto(idSubconjunto) {
            if (idSubconjunto > 0) {
                let obj = new Object();
                obj = {
                    idSubconjunto: idSubconjunto
                };
                axios.post('EliminarSubconjunto', obj)
                    .catch(o_O => Alert2Error(o_O.message))
                    .then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            Alert2Exito("Se ha eliminado con éxito el subconjunto.");
                            fncGetSubconjuntos();
                            if (cboProyecto.val() != "") {
                                fncFillCboSubconjuntos();
                            }
                        } else {
                            Alert2Error(message);
                        }
                    }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al intentar eliminar el subconjunto.");
            }
        }
        //#endregion

        //#region FUNCIONES CATALOGO REQUISICIONES
        function initTablaRequisiciones() {
            dtRequisiciones = tblBL_Requisiciones.DataTable({
                language: dtDicEsp,
                destroy: true,
                ordering: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: true,
                info: false,
                columns: [
                    { data: 'numRequisicion', title: 'Núm. requisición' },
                    { data: 'id', title: 'id', visible: false },
                    {
                        render: function (data, type, row) {
                            // let editarRequisicion = `<button class='btn-editar btn btn-warning editarRequisicion' data-id="${row.id}">` +
                            //                             `<i class='fas fa-pencil-alt'></i>` +
                            //                         `</button>&nbsp;`;
                            // let eliminarRequisicion = `<button class='btn-eliminar btn btn-danger eliminarRequisicion' data-id="${row.id}">` +
                            //                             `<i class="far fa-trash-alt"></i>
                            //                         </button>`;
                            // return editarRequisicion + eliminarRequisicion;

                            let btnVerDetReq = `<button class="btn btn-primary verDetReq"><i class='fas fa-list'></i></button>&nbsp;`;
                            let btnEliminarRequisicion = `<button class="btn-eliminar btn btn-danger eliminarRequisicion" data-id="${row.id}"><i class="far fa-trash-alt"></i></button>`;
                            return btnVerDetReq + btnEliminarRequisicion;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    // tblBL_Requisiciones.on("click", ".editarRequisicion", function () {
                    //     let rowData = dtRequisiciones.row($(this).closest('tr')).data();
                    //     txtNumRequisicion.val(rowData.numRequisicion);
                    //     btnCrearEditarNumRequisicion.attr("data-id", rowData.id);
                    //     if (!divCrearEditarNumRequisicion.hasClass('in')) {
                    //         btnCollapseNuevoNumRequisicion.trigger('click');
                    //     }
                    //     btnCrearEditarNumRequisicion.text("Actualizar");
                    // });

                    tblBL_Requisiciones.on("click", ".verDetReq", function () {
                        let rowData = dtRequisiciones.row($(this).closest('tr')).data();
                        fncGetLstDetReqCC(rowData.numRequisicion);
                        mdlLstDetReqCC.modal("show");
                    });

                    tblBL_Requisiciones.on("click", ".eliminarRequisicion", function () {
                        let rowData = dtRequisiciones.row($(this).closest("tr")).data();
                        let idRequisicion = parseFloat(rowData.id);
                        Alert2AccionConfirmar("¡Cuidado!", "¿Desea eliminar el registro seleccionado?", "Confirmar", "Cancelar", () => fncEliminarRequisicion(idRequisicion));
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
            });
        }

        function fncLimpiarCrearEditarRequisicion() {
            btnCrearEditarNumRequisicion.attr("data-BL", 0);
            btnCrearEditarNumRequisicion.attr("data-id", 0);
            txtNumRequisicion.val("");
        }

        function fncCrearEditarRequisicion() {
            let objRequisicion = fncCrearObjRequisicion(true);
            axios.post('CrearEditarRequisicion', objRequisicion).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetRequisiciones();
                    fncGetBackLogs();
                    Alert2Exito("Se registro correctamente el número de requisición.");
                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearObjRequisicion(esCrearEditar) {
            let objRequisicion = new Object();
            let idBackLog = btnCrearEditarNumRequisicion.attr("data-BL");
            let idRequisicion = btnCrearEditarNumRequisicion.attr("data-id");
            let numRequisicion = 0;

            if (numRequisicion == "" && esCrearEditar)
                Alert2Warning("Es necesario indicar número de requisición.");

            objRequisicion = {
                id: idRequisicion,
                idBackLog: idBackLog,
                numRequisicion: numRequisicion
            };
            return objRequisicion;
        }

        function fncGetRequisiciones() {
            let objRequisicion = fncCrearObjRequisicion(false);
            axios.post('GetRequisiciones', objRequisicion).catch(o_O => Alert2Error(o_O.message)).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtRequisiciones.clear();
                    dtRequisiciones.rows.add(response.data.lstRequisiciones);
                    dtRequisiciones.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarRequisicion(idRequisicion) {
            if (idRequisicion > 0) {
                let obj = new Object();
                obj = {
                    id: idRequisicion
                };
                axios.post('EliminarRequisicion', obj)
                    .catch(o_O => Alert2Error(o_O.message))
                    .then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            Alert2Exito("Se ha eliminado con éxito el número de requisición.");
                            fncGetRequisiciones();
                            fncGetBackLogs();
                        } else {
                            Alert2Error(message);
                        }
                    }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al intentar eliminar el número de requisición.");
            }
        }

        function initTablaLstReqCC() {
            dtLstReqCC = tblLstReqCC.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                "order": [[0, "desc"]],
                columns: [
                    { data: 'numero', title: 'Núm. requisición' },
                    {
                        data: 'fecha', title: 'Fecha',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'comentarios', title: 'Comentarios' },
                    {
                        render: function (data, type, row) {
                            let btnVerDetReq = `<button class="btn btn-primary verDetReq"><i class="fas fa-list"></i></button>&nbsp;`;
                            let btnGuardarReq = `<button class="btn btn-success guardarReq"><i class="fas fa-arrow-circle-right"></i></button>`;
                            return btnVerDetReq + btnGuardarReq;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblLstReqCC.on('click', '.verDetReq', function () {
                        let rowData = dtLstReqCC.row($(this).closest('tr')).data();
                        fncGetLstDetReqCC(rowData.numero);
                        mdlLstDetReqCC.modal("show");
                    });
                    tblLstReqCC.on("click", ".guardarReq", function () {
                        let rowData = dtLstReqCC.row($(this).closest("tr")).data();
                        let idBL = btnCrearEditarNumRequisicion.attr("data-BL");
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea guardar la requisición seleccionada?', 'Confirmar', 'Cancelar', () => fncGuardarReqSeleccionada(idBL, rowData.numero));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGuardarReqSeleccionada(idBL, numReq) {
            let obj = new Object();
            obj = {
                idBackLog: idBL,
                numRequisicion: numReq
            };
            axios.post("CrearEditarRequisicion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Éxito al registrar la requisición en el BackLog.");
                    fncGetBackLogs();
                    fncGetRequisiciones();
                    mdlLstReqCC.modal("hide");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetLstReqCC() {
            let obj = new Object();
            obj = {
                idBackLog: btnCrearEditarNumRequisicion.attr("data-BL")
            };
            axios.post("GetAllRequisiciones", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtLstReqCC.clear();
                    dtLstReqCC.rows.add(response.data.lstRequisicionesEK);
                    dtLstReqCC.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaLstDetReqCC() {
            dtLstDetReqCC = tblLstDetReqCC.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'numero', title: 'Núm. requisición' },
                    { data: 'partida', title: 'Partida' },
                    { data: 'insumo', title: 'Insumo' },
                    {
                        data: 'fecha_requerido', title: 'Fecha requerido',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'cantidad', title: 'Cantidad' },
                    {
                        data: 'fecha_ordenada', title: 'Fecha ordenada',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    // CODE ...
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetLstDetReqCC(numero) {
            let obj = new Object();
            obj = {
                idBackLog: btnCrearEditarNumRequisicion.attr("data-BL"),
                numero: numero
            };
            axios.post("GetAllDetRequisiciones", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtLstDetReqCC.clear();
                    dtLstDetReqCC.rows.add(response.data.lstDetRequisicionesEK);
                    dtLstDetReqCC.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarNumRequisicionManual() {
            let objCrearEditarNumRequisicion = new Object();
            objCrearEditarNumRequisicion = {
                // id: 
                numRequisicion: txtCrearEditarNumRequisicionManual.val(),
                idBackLog: btnCrearEditarNumRequisicion.attr("data-bl")
            }
            axios.post("CrearEditarRequisicion", objCrearEditarNumRequisicion).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha registrado con éxito la requisición.");
                    fncGetRequisiciones(false);
                    modalCrearEditarNumRequisicionManual.modal("hide");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblFoliosTraspasos() {
            dtFoliosTraspasos = tblBL_FoliosTraspasos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'cc', visible: false },
                    { data: 'almacenID', title: 'Almacen' },
                    { data: 'almacen', title: 'Almacen', visible: false },
                    { data: 'almDestino', title: 'Alm. destino', visible: false },
                    { data: 'almDestinoID', title: 'Alm. destino' },
                    { data: 'folioTraspaso', title: 'Folio traspaso' },
                    { data: 'numero', visible: false },
                    { data: 'traspasoCompleto', title: 'Estatus traspaso' },
                    {
                        render: function (data, type, row) {
                            let btnActualizar = `<button class="btn btn-warning actualizarFolioTraspaso btn-xs"><i class="far fa-edit"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-danger eliminarFolioTraspaso btn-xs"><i class="far fa-trash-alt"></i></button>`;
                            return btnActualizar + btnEliminar;
                        }
                    },
                    { data: 'id', visible: false },
                    { data: 'idBL', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblBL_FoliosTraspasos.on('click', '.actualizarFolioTraspaso', function () {
                        let rowData = dtFoliosTraspasos.row($(this).closest('tr')).data();
                        txtCETraspasoFolioAlmacen.val(rowData.almacenID);
                        txtCETraspasoFolioAlmacen.trigger("change");
                        txtCETraspasoFolio.val(rowData.folioTraspaso);
                        txtCETraspasoFolioAlmacenDestino.val(rowData.almDestinoID);
                        txtCETraspasoFolioAlmacenDestino.trigger("change");
                        txtCETraspasoFolioNumero.val(rowData.numero);
                        txtCETraspasoFolioCC.val(rowData.cc);
                        lblTitleCETraspasoFolio.html("Actualizar folio traspaso");
                        btnCETraspasoFolio.html("Actualizar");
                        btnCETraspasoFolio.attr("data-id", rowData.id);
                        mdlCETraspasoFolio.modal("show");
                    });
                    tblBL_FoliosTraspasos.on('click', '.eliminarFolioTraspaso', function () {
                        let rowData = dtFoliosTraspasos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarFolioTraspaso(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetFoliosTraspasos() {
            let obj = new Object();
            obj = {
                idBL: btnMotivoCancelacion.attr("data-idBL")
            }
            axios.post("GetFoliosTraspasos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtFoliosTraspasos.clear();
                    dtFoliosTraspasos.rows.add(response.data.lstFoliosTraspasos);
                    dtFoliosTraspasos.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarTraspasoFolio() {
            let obj = fncGetObjCETraspasoFolio();
            if (obj != "") {
                axios.post("CrearEditarTraspasoFolio", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        mdlCETraspasoFolio.modal("hide");
                        Alert2Exito("Se ha registrado con éxito el folio de traspaso.");
                        fncGetFoliosTraspasos();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetObjCETraspasoFolio() {
            let strMensajeError = "";

            if (txtCETraspasoFolioAlmacen.val() == "") { txtCETraspasoFolioAlmacen.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCETraspasoFolioAlmacenDestino.val() == "") { txtCETraspasoFolioAlmacenDestino.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCETraspasoFolio.val() == "") { txtCETraspasoFolio.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            // if (txtCETraspasoFolioNumero.val() == "") { txtCETraspasoFolioNumero.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCETraspasoFolio.attr("data-id"),
                    idBL: btnMotivoCancelacion.attr("data-idBL"),
                    areaCuenta: cboProyecto.val(),
                    almacenID: txtCETraspasoFolioAlmacen.val(),
                    almDestinoID: txtCETraspasoFolioAlmacenDestino.val(),
                    cc: txtCETraspasoFolioCC.val(),
                    folioTraspaso: txtCETraspasoFolio.val(),
                    numero: txtCETraspasoFolioNumero.val()
                }
                return obj;
            }
        }

        function fncEliminarFolioTraspaso(idFolioTraspaso) {
            let obj = new Object();
            obj = {
                idFolioTraspaso: idFolioTraspaso
            }
            axios.post("EliminarFolioTraspaso", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha eliminado con éxito el registro.");
                    fncGetFoliosTraspasos();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region FUNCIONES ORDENES DE COMPRA
        function initTablaOC() {
            dtOC = tblBL_OrdenesCompra.DataTable({
                language: dtDicEsp,
                destroy: true,
                ordering: false,
                paging: true,
                searching: false,
                bFilter: true,
                info: false,
                columns: [
                    { data: 'numRequisicion', title: 'Núm. requisición' },
                    { data: 'numOC', title: 'Núm. O.C' },
                    {
                        data: 'estatus', title: 'Surtido',
                        render: function (data, type, row) {
                            if (data == "T") {
                                return `<input type="checkbox" checked="checked" />`;
                            } else {
                                return `<input type="checkbox" />`;
                            }
                        }
                    },
                    {
                        render: function (data, type, row) {
                            let btnVerDetOC = `<button class="btn btn-primary verDetOc"><i class="fas fa-list"></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn-eliminar btn btn-danger eliminarOC' data-id="${row.id}"><i class="far fa-trash-alt"></i></button>`;
                            return btnVerDetOC + btnEliminar;
                        }
                    },
                    { data: 'id', title: 'id', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblBL_OrdenesCompra.on("click", ".eliminarOC", function () {
                        let rowData = dtOC.row($(this).closest("tr")).data();
                        let idOC = parseFloat(rowData.id);
                        Alert2AccionConfirmar("¡Cuidado!", "¿Desea eliminar el registro seleccionado?", "Confirmar", "Cancelar", () => fncEliminarOC(idOC));
                    });

                    tblBL_OrdenesCompra.on('click', '.verDetOc', function () {
                        fncGetLstDetOcReq();
                        mdlLstDetOcReq.modal("show");
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
            });
        }

        function fncEliminarOC(idOC) {
            if (idOC > 0) {
                let obj = new Object();
                obj = {
                    id: idOC
                };
                axios.post('EliminarOC', obj)
                    .catch(o_O => Alert2Error(o_O.message))
                    .then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            Alert2Exito("Se ha eliminado con éxito la orden de compra.");
                            fncGetOC();
                            fncGetBackLogs();
                        } else {
                            Alert2Error(message);
                        }
                    }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al intentar eliminar la orden de compra.");
            }
        }

        function fncGetAllOC() {
            let objOC = fncCrearObjOC(false);
            axios.post('GetAllOrdenesCompra', objOC).catch(o_O => Alert2Error(o_O.message)).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let objInputs = fncCrearInputsOC(response.data.lstOC.length, response.data.lstOC);
                    divOrdenesCompra.html(objInputs);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetOC() {
            let objOC = fncCrearObjOC(true);
            axios.post('GetOrdenesCompra', objOC).catch(o_O => Alert2Error(o_O.message)).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtOC.clear();
                    dtOC.rows.add(response.data.lstOC);
                    dtOC.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearObjOC(esCrearEditar) {
            let objOC = new Object();
            let idBackLog = btnVerificarOrdenesCompra.attr("data-id");

            objOC = {
                idBackLog: idBackLog
            };
            return objOC;
        }

        function fncCrearInputsOC(cantOC, lstOC) {
            let divInputs = "";
            for (let i = 0; i < cantOC; i++) {
                divInputs += `<div class="col-sm-3">
                                <label>Número OC: </label>&nbsp;
                                <input type="checkbox" id="chkOC${i}" data-req="${lstOC[i].num_requisicion}" name="chkOC" value="${lstOC[i].numero}" />
                                <input value="${lstOC[i].numero}" id="txtOC${i}" class="form-control" />
                            </div>`;
            }
            btnCrearEditarOC.attr("data-cantOC", cantOC);
            return divInputs;
        }

        function fncObtenerOCGuardarJS() {
            let cantOC = btnCrearEditarOC.attr("data-cantOC");
            let arrDataOC = [];
            let arrDataReq = [];
            for (let i = 0; i < cantOC; i++) {
                if ($("#chkOC" + i + "").prop("checked")) {
                    arrDataOC.push($("#chkOC" + i + "").val());
                    arrDataReq.push($("#chkOC" + i + "").attr("data-req"));
                }
            }

            let strMensaje = "";
            arrDataOC.length > 1 ?
                strMensaje = "¿Desea guardar las ordenes de compra seleccionadas?" :
                strMensaje = "¿Desea guardar la orden de compra seleccionada?";
            Alert2AccionConfirmar("", strMensaje, "Confirmar", "Cancelar", () => fncCrearOC(arrDataOC, arrDataReq));
        }

        function fncCrearOC(arrDataOC, arrDataReq) {
            let objData = new Object();
            let lstDataOC = [];
            let idBackLog = btnVerificarOrdenesCompra.attr("data-id");
            for (let i = 0; i < arrDataOC.length; i++) {
                objData = {
                    idBackLog: idBackLog,
                    numOC: arrDataOC[i],
                    numRequisicion: arrDataReq[i]
                };
                lstDataOC.push(objData);
            }

            if (arrDataOC.length <= 0) {
                Alert2Warning("No hay alguna orden de compra seleccionada.");
            } else {
                axios.post("CrearOC", lstDataOC).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetOC();
                        fncGetBackLogs();
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function initTablaLstOcReq() {
            dtLstOcReq = tblLstOcReq.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    //numero, moneda, tipo_cambio, porcent_iva, sub_total, iva, total, comentarios
                    { data: 'numero', title: 'No. OC' },
                    {
                        data: 'moneda', title: 'Moneda',
                        render: function (data, type, row) {
                            let tipoMoneda = data;
                            if (tipoMoneda == 2) {
                                return "USD";
                            } else {
                                return "MXN";
                            }
                        }
                    },
                    { data: 'tipo_cambio', title: 'Tipo cambio' },
                    { data: 'porcent_iva', title: 'IVA %' },
                    { data: 'sub_total', title: 'Subtotal' },
                    { data: 'iva', title: 'IVA' },
                    { data: 'total', title: 'Total' },
                    { data: 'comentarios', title: 'Comentarios' },
                    {
                        render: function (data, type, row) {
                            let btnVerDetOc = `<button class="btn btn-primary verDetOc"><i class="fas fa-list"></i></button>&nbsp;`;
                            let btnGuardarOc = `<button class="btn btn-success guardarOc"><i class="fas fa-arrow-circle-right"></i></button>`;
                            return btnVerDetOc + btnGuardarOc;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblLstOcReq.on('click', '.verDetOc', function () {
                        let rowData = dtLstOcReq.row($(this).closest('tr')).data();
                        fncGetLstDetOcReq();
                        mdlLstDetOcReq.modal("show");
                    });
                    tblLstOcReq.on("click", ".guardarOc", function () {
                        let rowData = dtLstOcReq.row($(this).closest("tr")).data();
                        let idBL = btnVerificarOrdenesCompra.attr("data-id");
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea guardar la OC seleccionada?', 'Confirmar', 'Cancelar', () => fncGuardarOCSeleccionada(idBL, rowData.numero));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGuardarOCSeleccionada(idBL, numOC) {
            let obj = new Object();
            obj = {
                numero: numOC,
                idBackLog: idBL
            };
            axios.post("GuardarOC", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Éxito al registrar la OC en el BackLog.");
                    fncGetBackLogs();
                    fncGetOC();
                    mdlLstOcReq.modal("hide");
                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetLstOcReq() {
            let obj = new Object();
            obj = {
                idBackLog: btnVerificarOrdenesCompra.attr("data-id")
            };
            axios.post("GetLstOcReq", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtLstOcReq.clear();
                    dtLstOcReq.rows.add(response.data.lstOCEK);
                    dtLstOcReq.draw();
                    mdlLstOcReq.modal("show");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaLstDetOcReq() {
            dtLstDetOcReq = tblLstDetOcReq.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    //numero, partida, insumo, fecha_entrega, cantidad, precio, importe, num_requisicion
                    { data: 'numero', title: 'Núm. O.C' },
                    { data: 'partida', title: 'Partida' },
                    { data: 'num_requisicion', title: 'Núm. requisición' },
                    { data: 'insumo', title: 'Insumo' },
                    // { 
                    //     data: 'fecha_entrega', title: 'Fecha entrega',
                    //     render: function (type, data, row){
                    //         return moment(data).format("DD/MM/YYYY");
                    //     }
                    // },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'precio', title: 'Precio' },
                    { data: 'importe', title: 'Importe' }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetLstDetOcReq() {
            let obj = new Object();
            obj = {
                idBackLog: btnVerificarOrdenesCompra.attr("data-id")
            };
            axios.post("GetLstDetOcReq", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtLstDetOcReq.clear();
                    dtLstDetOcReq.rows.add(response.data.lstDetOCEK);
                    dtLstDetOcReq.draw();
                    mdlLstDetOcReq.modal("show");
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region FUNCIONES EVIDENCIAS
        function initTablaEvidencias() {
            dtEvidencias = tblEvidencias.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'nombreArchivo', title: 'ARCHIVO' },
                    {
                        data: 'tipoEvidencia', title: 'TIPO EVIDENCIA',
                        render: function (type, data, row) {
                            let esOTVacia = row.tipoEvidencia;
                            return esOTVacia;
                        }
                    },
                    {
                        render: function (type, data, row) {
                            let btnMostrarEvidencia = `<button class="btn btn-primary mostrarEvidencia" data-id="${row.id}" title="Visualizar evidencia."><i class="fas fa-eye"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-danger eliminarEvidencia" data-id="${row.id}" title="Eliminar evidencia."><i class="fas fa-trash"></i></button>`;
                            return btnMostrarEvidencia + btnEliminar;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblEvidencias.on('click', '.eliminarEvidencia', function () {
                        let rowData = dtEvidencias.row($(this).closest('tr')).data();
                        let id = parseFloat(rowData.id);
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarArchivo(id));
                    });

                    tblEvidencias.on("click", ".mostrarEvidencia", function () {
                        let rowData = dtEvidencias.row($(this).closest('tr')).data();
                        fncVisualizarEvidencia(rowData.id);
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncVisualizarEvidencia(idEvidencia) {
            let obj = new Object();
            obj.idEvidencia = idEvidencia
            axios.post("VisualizarEvidencia", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    $('#myModal').data().ruta = null;
                    $('#myModal').modal('show');
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetArchivos(id) {
            let obj = new Object();
            obj = {
                id: id
            }
            axios.post("GetArchivos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtEvidencias.clear().draw();
                    dtEvidencias.rows.add(response.data.lstArchivos).draw();
                    $('#tblEvidencias tr').each(function () {
                        // need this to skip the first row
                        if ($(this).find("td:first").length > 0) {
                            var cutomerId = $(this).find("td:first").html();
                            if (cutomerId > 0) {
                                btnGuardarEvidencia.attr("disabled", true);
                            }
                            else {
                                btnGuardarEvidencia.attr("disabled", false);
                            }
                        }
                    });
                }
                else {
                    Alert2Error(message)
                }

            }).catch(error => Alert2Error(error.message));
        }

        const subiendoArchivo = function () {
            var data = fncGetEvidenciaParaGuardar();
            let objRegistro = new Object();
            objRegistro = {
                tipoEvidencia: cboMdlEvidenciasTipoEvidencia.val()
            };
            if (cboMdlEvidenciasTipoEvidencia.val() != "") {
                $("#divCboMdlEvidenciasTipoEvidencias").css("border", "0px solid red");
                axios.post('/BackLogs/postSubirArchivos', data,
                    { params: tipoEvidencia = objRegistro },
                    { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                        let { success, datos, message } = response.data;
                        if (success) {
                            Alert2Exito("Se ha registrado con éxito la evidencia.");
                            $('#lblTexto1').text('Ningún archivo seleccionado');
                            let idBL = btnGuardarEvidencia.attr("data-bl");
                            fncGetArchivos(idBL);
                            cboMdlEvidenciasTipoEvidencia[0].selectedIndex = 0;
                            cboMdlEvidenciasTipoEvidencia.trigger("change");
                            // $('#mdlEvidencias').modal('hide');
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message)
                    );
            } else {
                $("#divCboMdlEvidenciasTipoEvidencias").css("border", "2px solid red");
                Alert2Warning("Es necesario seleccionar el tipo de evidencia.");
            }
        }

        function fncGetEvidenciaParaGuardar() {
            let data = new FormData();
            data.append("id", $("#rowDataId").val());
            $.each(document.getElementById("inputExamen").files, function (i, file) {
                data.append("archivo", file);
            });
            return data;
        }

        function fncEliminarArchivo(id) {
            // console.log(id)
            axios.post('EliminarArchivos', { id: id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha eliminado con éxito.");
                    let idBL = btnGuardarEvidencia.attr("data-bl");
                    fncGetArchivos(idBL);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region FUNCIONES LISTADO OC SURTIDAS
        function initTblOCSurtidas() {
            dtOCSurtidas = tblListadoOCSurtidas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetListadoOCSurtidas() {
            let obj = new Object();
            obj = {

            }
            axios.post("GetListadoOCSurtidas", parametros).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dt.clear();
                    dt.rows.add(response.data.lstOCSurtidas);
                    dt.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => BackLogs = new BackLogs())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();