(() => {
    $.namespace("Maquinaria.BackLogs.PresupuestoRehabilitacionTMC");

    //#region CONST
    let _noEconomico = "";
    const txtTotal = $('#txtTotal');
    const btnCrearEditarNumRequisicionManual = $('#btnCrearEditarNumRequisicionManual');
    const btnAbrirModalRequisicionManual = $('#btnAbrirModalRequisicionManual');
    const modalCrearEditarNumRequisicionManual = $('#modalCrearEditarNumRequisicionManual');
    const lblTitleCrearEditarNumRequisicionManual = $("#lblTitleCrearEditarNumRequisicionManual");
    const lblBtnCrearEditarNumRequisicionManual = $("#lblBtnCrearEditarNumRequisicionManual");
    const txtCrearEditarNumRequisicionManual = $("#txtCrearEditarNumRequisicionManual");
    const cboProyecto = $('#cboProyecto');
    const cboFiltroMotivo = $('#cboFiltroMotivo');
    const cboFiltroTipo = $('#cboFiltroTipo');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroLimpiar = $('#btnFiltroLimpiar');
    const cboFiltroModelo = $('#cboFiltroModelo');
    const cboFiltroGrupo = $('#cboFiltroGrupo');
    const tblBL_CatBackLogs = $('#tblBL_CatBackLogs');
    const tblBL_CatBackLogs_tbody = $("#tblBL_CatBackLogs tbody");
    const tblEvidencias = $('#tblEvidencias');

    const tblEnPrograma = $('#tblEnPrograma');
    const btnCostoPromedio = $('#btnCostoPromedio');
    const txtUnidad = $('#txtUnidad');

    let dtEnPrograma;

    const btnInformeRehabilitacion = $("#btnInformeRehabilitacion");
    const btnProgramaInspeccion = $("#btnProgramaInspeccion");
    const btnPresupuestoRehabilitacion = $('#btnPresupuestoRehabilitacion');
    const btnSeguimientoPresupuestos = $('#btnSeguimientoPresupuestos');
    const btnFrenteBackLogs = $('#btnFrenteBackLogs');
    const btnIndicadoresRehabilitacionTMC = $('#btnIndicadoresRehabilitacionTMC');
    const btnInicioTMC = $('#btnInicioTMC');

    const cboCC = $("#cboCC");
    const txtModelo = $("#txtModelo");
    const txtGrupo = $("#txtGrupo");
    const txtFecha = $("#txtFecha");
    const txtHoras = $("#txtHoras");
    const txtFolio = $("#txtFolio");
    const txtDescripcion = $("#txtDescripcion");
    const cboConjunto = $("#cboConjunto");
    const cboSubconjunto = $("#cboSubconjunto");
    const chkParte = $("#chkParte");
    const chkMO = $("#chkMO");
    const btnModalAbrirParte = $("#btnModalAbrirParte");
    const btnModalAbrirManoObra = $("#btnModalAbrirManoObra");
    const btnCargarEvidencias = $("#btnCargarEvidencias");
    const btnLimpiarFormCrearEditarBackLog = $("#btnLimpiarFormCrearEditarBackLog");
    const btnCrearEditarBackLog = $("#btnCrearEditarBackLog");
    const btnGuardarEvidencia = $('#btnGuardarEvidencia');

    const modalCrearParte = $('#modalCrearParte');
    const txtInsumo = $('#txtInsumo');
    const txtCantidad = $('#txtCantidad');
    const txtParte = $('#txtParte');
    const txtArticulo = $('#txtArticulo');
    const btnCatInsumos = $('#btnCatInsumos');
    const cboTipoMoneda = $('#cboTipoMoneda')
    const btnCrearEditarParte = $('#btnCrearEditarParte');
    const btnCrearEditarCancelarParte = $('#btnCrearEditarCancelarParte');
    const modalEliminarParte = $('#modalEliminarParte');
    const lblEliminarParte = $('#lblEliminarParte');
    const btnEliminarParte = $('#btnEliminarParte');
    const btnEliminarCancelarParte = $('#btnEliminarCancelarParte');
    const tblBL_Partes = $('#tblBL_Partes');
    const tblBL_Partes_tbody = $("#tblBL_Partes tbody");
    const txtCostoPromedio = $('#txtCostoPromedio');
    var rowEditarParte = 0;
    var ContPartidasParte = 0;
    var idRowParte = 0;
    var arrPartes = [];
    let dtPartes;

    const modalCrearManoObra = $('#modalCrearManoObra');
    const txtDescripcionManoObra = $('#txtDescripcionManoObra');
    const btnCrearEditarManoObra = $('#btnCrearEditarManoObra');
    const btnCrearEditarCancelarManoObra = $('#btnCrearEditarCancelarManoObra');
    const modalEliminarManoObra = $('#modalEliminarManoObra');
    const lblEliminarManoObra = $('#lblEliminarManoObra');
    const btnEliminarManoObra = $('#btnEliminarManoObra');
    const tblBL_ManoObra = $("#tblBL_ManoObra");
    const tblBL_ManoObra_tbody = $("#tblBL_ManoObra tbody");
    var idRowManoObra = 0;
    var ContPartidasManoObra = 0;
    var rowEditarManoObra = 0;
    var arrManoObra = [];
    let dtManoObra;

    const mdlCatalogoInsumos = $('#mdlCatalogoInsumos');
    const txtFiltroInsumo = $('#txtFiltroInsumo');
    const txtFiltroDescripcion = $('#txtFiltroDescripcion');
    const btnFiltroBuscarInsumo = $('#btnFiltroBuscarInsumo');
    const tblCatInsumos = $('#tblCatInsumos');
    let dtInsumos;

    let dtBackLogs;

    const tblBL_SolicitudPpto = $('#tblBL_SolicitudPpto');
    let dtSolicitudPpto;

    const mdlDescripcionBL = $('#mdlDescripcionBL');
    const txtDescripcionBL = $('#txtDescripcionBL');

    const cboMdlEvidenciasTipoEvidencia = $('#cboMdlEvidenciasTipoEvidencia');

    //#region CONST CATALOGO CONJUNTOS
    const modalCrearEditarConjuntoCatConjuntos = $('#modalCrearEditarConjuntoCatConjuntos');
    const btnNuevoConjuntoCatConjuntos = $('#btnNuevoConjuntoCatConjuntos');
    const divCrearEditarConjuntoCatConjuntos = $('#divCrearEditarConjuntoCatConjuntos');
    const txtConjuntoCatConjuntos = $('#txtConjuntoCatConjuntos');
    const btnCrearEditarCancelarConjunto = $('#btnCrearEditarCancelarConjunto');
    const btnCrearEditarConjuntoCatConjuntos = $('#btnCrearEditarConjuntoCatConjuntos');
    const tblConjuntosCatConjuntos = $('#tblConjuntosCatConjuntos');
    const btnCrearEditarCancelarConjuntoCatConjuntos = $('#btnCrearEditarCancelarConjuntoCatConjuntos');
    const btnConjuntosCatConjuntos = $('#btnConjuntosCatConjuntos');
    const txtAbreviacionoCatConjuntos = $('#txtAbreviacionoCatConjuntos');
    let dtConjuntos;
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
    //#endregion

    //#region CONST MODAL LISTADO REQUISICIONES CC
    const mdlLstReqCC = $('#mdlLstReqCC');
    const tblLstReqCC = $('#tblLstReqCC');
    let dtRequisiciones;
    //#endregion

    //#region CONST MODAL LISTADO DETALLE DE REQUISICIONES CC
    const mdlLstDetReqCC = $('#mdlLstDetReqCC');
    const tblLstDetReqCC = $('#tblLstDetReqCC');
    const txtNumRequisicion = $('#txtNumRequisicion');
    const btnCrearEditarNumRequisicion = $('#btnCrearEditarNumRequisicion');
    const modalCrearEditarNumRequisicion = $('#modalCrearEditarNumRequisicion');
    const tblBL_Requisiciones = $('#tblBL_Requisiciones');
    const btnCrearEditarCancelarNumRequisicion = $('#btnCrearEditarCancelarNumRequisicion');
    const btnVerificarRequisiciones = $('#btnVerificarRequisiciones');
    const btnCrearRequisicion = $('#btnCrearRequisicion');
    const btnCancelarRequisicion = $('#btnCancelarRequisicion');
    const btnMotivoCancelacion = $('#btnMotivoCancelacion');
    const mdlMotivosCancelacionReq = $('#mdlMotivosCancelacionReq');
    const tblBL_MotivoCancelacionReq = $('#tblBL_MotivoCancelacionReq');
    //#endregion

    //#region CONST MODAL LISTADO ORDENES DE COMPRA CON RELACION A LAS REQUISICIONES
    const tblLstOcReq = $('#tblLstOcReq');
    const mdlLstOcReq = $('#mdlLstOcReq');
    const tblBL_OrdenesCompra = $('#tblBL_OrdenesCompra');

    const tblLstDetOcReq = $('#tblLstDetOcReq');
    const mdlLstDetOcReq = $('#mdlLstDetOcReq');
    let dtLstOcReq;
    let dtLstDetOcReq;
    const divOrdenesCompra = $('#divOrdenesCompra');
    const btnVerificarOrdenesCompra = $('#btnVerificarOrdenesCompra');
    const modalCrearEditarNumOC = $('#modalCrearEditarNumOC');

    let combo;

    //#endregion

    BackLogs = function () {
        (function init() {
            fncListeners();
            obtenerUrlPArams();
            tblBL_ManoObra.hide();
        })();

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            if (variables != undefined) {
                cboProyecto.val(variables.areaCuenta);
                cboProyecto.trigger('change');
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
            //#region INIT
            initTablaBackLogs();
            fncDatepicker();
            fncFillCboProyectosObra();
            fncHabilitarDeshabilitarControles();
            initTablaEnPrograma();
            fncFillCboConjunto();
            initTablaPartes();
            initTablaCatInsumos();
            initTablaManoObra();
            initTablaConjuntos();
            initTablaSubconjuntos();
            initTablaLstReqCC();
            initTablaLstDetReqCC();
            initTablaLstOcReq();
            initTablaLstDetOcReq();
            initTablaRequisiciones();
            initTablaOC();
            initTablaSolicitudPpto();
            initTablaMotivoCancelacionReq();
            initTablaEvidencias();
            bon();
            //#endregion

            cboMdlEvidenciasTipoEvidencia.select2();
            cboMdlEvidenciasTipoEvidencia.select2({ width: "100%" });

            btnAbrirModalRequisicionManual.on("click", function (e) {
                lblTitleCrearEditarNumRequisicionManual.html("Agregar requisición existente");
                lblBtnCrearEditarNumRequisicionManual.html("Guardar");
                txtCrearEditarNumRequisicionManual.val("");
                modalCrearEditarNumRequisicionManual.modal("show");
            });

            btnCrearEditarNumRequisicionManual.click(function () {
                fncCrearEditarNumRequisicionManual();
            });

            btnCrearEditarBackLog.attr("data-id", 0);
            btnInicioTMC.click(function (e) {
                document.location.href = '/BackLogs/IndexTMC?areaCuenta=' + cboProyecto.val();
            });
            btnProgramaInspeccion.click(function (e) {
                document.location.href = '/BackLogs/ProgramaInspTMC?areaCuenta=' + cboProyecto.val();
            });
            btnPresupuestoRehabilitacion.click(function (e) {
                document.location.href = '/BackLogs/PresupuestoRehabilitacionTMC?areaCuenta=' + cboProyecto.val();
            });
            btnSeguimientoPresupuestos.click(function (e) {
                document.location.href = '/BackLogs/SeguimientoDePresupuestoTMC?areaCuenta=' + cboProyecto.val();
            });
            btnInformeRehabilitacion.click(function (e) {
                document.location.href = '/BackLogs/InformeTMC?areaCuenta=' + cboProyecto.val();
            });
            btnFrenteBackLogs.click(function (e) {
                document.location.href = '/BackLogs/FrenteTMC?areaCuenta=' + cboProyecto.val();
            });
            btnIndicadoresRehabilitacionTMC.click(function (e) {
                document.location.href = '/BackLogs/IndicadoresRehabilitacionTMC?areaCuenta=' + cboProyecto.val();
            });



            btnFiltroLimpiar.click(function (e) {
                cboFiltroMotivo[0].selectedIndex = 0;
                cboFiltroMotivo.trigger("change");

                cboFiltroModelo[0].selectedIndex = 0;
                cboFiltroModelo.trigger("change");

                cboFiltroGrupo[0].selectedIndex = 0;
                cboFiltroGrupo.trigger("change");

                cboFiltroTipo[0].selectedIndex = 0;
                cboFiltroTipo.trigger("change");
            });

            cboProyecto.change(function (e) {
                if ($(this).val() != "") {
                    // fncGetBackLogs();
                    retornarAlmacen();
                    fncFillTipoMaquinariaTMC();
                    fncFillCboModelos();
                    fncFillCboGrupos();

                    fncFillCboCC($(this).val());
                }
                fncHabilitarDeshabilitarControles();
            });

            btnFiltroBuscar.click(function (e) {
                fncGetProgramaInspeccionTMC();
            });

            cboCC.change(function (e) {
                if ($(this).val() != "") {
                    fncGetDatosCC();
                    if ($('select[id="cboCC"] option:selected').text() != $(this).attr("cc")) {
                        $(this).attr("esActualizarCC", true);
                    } else {
                        $(this).attr("esActualizarCC", false);
                    }
                    // fncFillTablaSolicitudPpto();
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

            chkMO.prop("checked", false);
            if (chkMO.prop("checked")) {
                btnModalAbrirManoObra.attr("disabled", false);
                tblBL_Partes.show();
            } else {
                btnModalAbrirManoObra.attr("disabled", true);
                tblBL_Partes.hide();
            }

            btnModalAbrirManoObra.click(function (e) {
                fncLimpiarCtrlsCrearEditarManoObra();
                modalCrearManoObra.modal("show");
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

            btnCrearEditarCancelarManoObra.click(function (e) {
                modalCrearManoObra.modal("hide");
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
                fncCrearEditarBackLog();
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

            btnLimpiarFormCrearEditarBackLog.click(function (e) {
                fncLimpiarCtrlsCrearEditarBackLog();
            })

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

            btnVerificarOrdenesCompra.click(function (e) {
                //fncGetAllOC();
                fncGetLstOcReq();
            });

            btnCrearRequisicion.click(function () {
                Alert2AccionConfirmar("¡Cuidado!", "¿Desea redirigirse a <b>Compras</b> para crear una <b>Requisición</b>?", "Confirmar", "Cancelar", () => fncPrecargarRequisicion());
            });

            btnCancelarRequisicion.click(function (e) {
                Alert2AccionConfirmarInput("Cancelar requisición", "Es necesario indicar el motivo de que este BackLog <b>no requiere requisición</b>.", "Confirmar", "Cancelar",
                    (strMotivo) => fncCancelarRequisicion(strMotivo))
            });

            btnMotivoCancelacion.click(function (e) {
                mdlMotivosCancelacionReq.modal("show");
                fncGetMotivosCancelacion(); //TODO
            });

            btnGuardarEvidencia.click(function (e) {
                subiendoArchivo();
            });

            //#region FUNCIONES: PARTES
            btnFiltroBuscarInsumo.click(function (e) {
                fncGetCatInsumos();
            });

            chkParte.prop("checked", false);
            if (chkParte.prop("checked")) {
                btnModalAbrirParte.attr("disabled", false);
                tblBL_Partes.show();
            } else {
                btnModalAbrirParte.attr("disabled", true);
                tblBL_Partes.hide();
            }

            chkParte.change(function (e) {
                if (chkParte.prop("checked")) {
                    btnModalAbrirParte.attr("disabled", false);
                    tblBL_Partes.show();
                } else {
                    btnModalAbrirParte.attr("disabled", true);
                    tblBL_Partes.hide();
                }
            });

            btnCatInsumos.click(function (e) {
                mdlCatalogoInsumos.modal("show");
            });

            txtFiltroInsumo.click(function (e) {
                $(this).select();
            });

            txtFiltroDescripcion.click(function (e) {
                $(this).select();
            })

            btnModalAbrirParte.click(function () {
                fncBorderDefault();
                fncLimpiarCtrlsCrearEditarParte();
                bon();
                modalCrearParte.modal("show");
            });

            btnCrearEditarParte.click(function (e) {
                let idBackLog = btnCrearEditarBackLog.attr("data-id");
                if (idBackLog > 0) {
                    fncCrearEditarParte();
                } else {
                    fncCrearParte();
                }
                fncFillTablaSolicitudPpto();
                tblBL_Partes.show();
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

            txtCantidad.keyup(function (e) {
                fncObtenerTotalPrecioInsumo();
            });

            txtCostoPromedio.keyup(function (e) {
                fncObtenerTotalPrecioInsumo();
            })

            btnCrearEditarCancelarParte.click(function (e) {
                modalCrearParte.modal("hide");
            });

            tblBL_Partes.on("click", ".btnEditarParte", function () {
                rowEditarParte = $(this).parents("tr").find("td").eq(0).html()
                txtInsumo.val($(this).parents("tr").find("td").eq(1).html())
                txtParte.val($(this).parents("tr").find("td").eq(3).html())
                txtArticulo.val($(this).parents("tr").find("td").eq(4).html())
                txtUnidad.val($(this).parents("tr").find("td").eq(6).html())

                txtCantidad.val($(this).parents("tr").find("td").eq(2).html())
                txtCostoPromedio.val(maskNumero2DCompras(unmaskNumero($(this).parents("tr").find("td").eq(5).html()) / unmaskNumero(txtCantidad.val())))
                txtTotal.val($(this).parents("tr").find("td").eq(5).html())

                let tipoMoneda = 0
                switch ($(this).parents("tr").find("td").eq(7).html()) {
                    case "MXN":
                        tipoMoneda = 1
                        break;
                    case "USD":
                        tipoMoneda = 2
                        break;
                    case "COP":
                        tipoMoneda = 3
                        break;
                    case "SOL":
                        tipoMoneda = 5
                        break;
                }
                cboTipoMoneda.val(tipoMoneda)
                cboTipoMoneda.select2({
                    width: "resolve"
                });

                btnCrearEditarParte.attr("data-id", $(this).attr("data-id"))
                modalCrearParte.modal("show");
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

            $("#btnCostoPromedio").click(function (e) {
                fncGetCostoPromedio();
            });

            txtCantidad.click(function () {
                $(this).select()
            })

            txtInsumo.click(function () {
                $(this).select()
            })

            txtParte.click(function () {
                $(this).select()
            })

            txtArticulo.click(function () {
                $(this).select()
            })

            txtUnidad.click(function () {
                $(this).select()
            })

            txtCostoPromedio.click(function () {
                $(this).select()
            })

            cboTipoMoneda.fillCombo("/BackLogs/FillCboTipoMonedas", {}, false);
            //#endregion
        }

        //#region FUNCIONES: PARTES
        function fncObtenerTotalPrecioInsumo() {
            let cantidad = txtCantidad.val()
            let costoPromedio = unmaskNumero(txtCostoPromedio.val())
            let total = 0
            if (cantidad > 0 && costoPromedio > 0) {
                total = cantidad * costoPromedio;
                txtTotal.val(maskNumero2DCompras(total))
            }
        }

        function fncGetCatInsumos() {
            let objFiltro = new Object();
            objFiltro = {
                insumo: txtFiltroInsumo.val(),
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
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary seleccionarInsumo" title="Seleccionar insumo."><i class="fas fa-arrow-circle-right"></i></button>`;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblCatInsumos.on('click', '.seleccionarInsumo', function () {
                        let rowData = dtInsumos.row($(this).closest('tr')).data();
                        txtInsumo.val(rowData.insumo);
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
                    console.log(response.data.lstPartes);
                    dtPartes.clear();
                    dtPartes.rows.add(response.data.lstPartes);
                    dtPartes.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncLimpiarCtrlsCrearEditarParte() {
            txtInsumo.val("")
            txtCantidad.val(0)
            txtParte.val("")
            txtArticulo.val("")
            txtUnidad.val("")
            txtCostoPromedio.val(maskNumero2DCompras(0))
            txtTotal.val(maskNumero2DCompras(0))
            cboTipoMoneda[0].selectedIndex = 0
            cboTipoMoneda.trigger("change")
            btnCrearEditarParte.attr("data-id", "0");
        }

        function initTablaPartes() {
            dtPartes = tblBL_Partes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: "partida", title: "Partida" },
                    { data: "insumo", title: "Insumo" },
                    { data: "cantidad", title: "Cantidad" },
                    { data: "parte", title: "Parte" },
                    { data: "articulo", title: "Artículo" },
                    {
                        data: "costoPromedio", title: "Costo promedio",
                        render: function (data, type, row) {
                            return maskNumero2DCompras(row.costoPromedio)
                        }
                    },
                    { data: "unidad", title: "Unidad" },
                    {
                        data: 'tipoMoneda', title: 'Moneda',
                        render: function (data, type, row) {
                            console.log(row.tipoMoneda);
                            switch (row.tipoMoneda) {
                                case 1:
                                    return "MXN"
                                case 2:
                                    return "USD"
                                case 3:
                                    return "COP"
                                case 5:
                                    return "SOL"
                                default:
                                    break;
                            }
                        }
                    },
                    {
                        render: function (data, type, row, cliente) {
                            let btnEditar = `<button type="button" class="btn-editar btn btn-xs btn-warning btnEditarParte" data-id="${row.id}"><i class="fa fa-pen"></i></button>`;
                            let btnEliminar = `<button type="button" class="btn-eliminar btn btn-xs btn-danger btnEliminarParte"  data-id="${row.id}"><i class="fa fa-trash"></i></button>`;
                            return `${btnEditar} ${btnEliminar}`
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblBL_CatBackLogs.on("click", ".editarBackLog", function () {
                        fncGetPartes();
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });

            tblBL_Partes_tbody.empty();
        }

        function bon() {
            $(document).ready(function () {
                $("[id*='btnCostoPromedio']").attr('disabled', 'disabled');
                $("[id*='txtInsumo']" && "[id*='txtCantidad']").keyup(function () {
                    if ($("#txtInsumo").val() != '' && $("#txtCantidad").val() != '') {
                        $("[id*='btnCostoPromedio']").removeAttr('disabled');
                    }
                    else {
                        $("[id*='btnCostoPromedio']").attr('disabled', 'disabled');
                    }
                });
            });
        }

        function fncGetCostoPromedio() {
            let obj = new Object();
            obj = {
                almacen: $('#cboProyecto').attr('data-Almacen'),
                insumo: txtInsumo.val()
            };
            axios.post("GetCostoPromedio", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    txtCostoPromedio.val(response.data.costoPromedio);
                    txtCostoPromedio.val(txtCostoPromedio.val() * txtCantidad.val());
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region BACKLOGS
        function retornarAlmacen() {
            axios.post('RetornarAlmacen', { areaCuenta: cboProyecto.val() })
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, items } = response.data;
                    if (success) {
                        cboProyecto.attr('data-Almacen', items)
                    }
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
            // cboCC[0].selectedIndex = 0;
            // cboCC.trigger("change");
            // cboCC.attr("disabled", false);
            fncFillCboConjunto();
            fncDatepicker();

            btnCrearEditarBackLog.attr("data-id", "0");
            btnCrearEditarBackLog.html("<i class='fas fa-save'></i>&nbsp;Guardar");

            tblBL_Partes_tbody.empty();
            tblBL_ManoObra_tbody.empty();
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
                        title: "Fecha",
                        render: function (data, type, row) {
                            return moment(row.fechaInspeccion).format('DD/MM/YYYY');
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
                                    return "50%";
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
                        data: 'presupuestoEstimado', title: 'Ppto. estimado',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        title: 'Evidencia', visible: false,
                        render: function (type, data, row) {
                            return "";
                        }
                    },
                    {
                        render: function (data, type, row) {
                            let btnActualizarBL = `<button class='btn-editar btn btn-xs btn-warning editarBackLog' title='Actualizar BackLog.'><i class='fas fa-pencil-alt'></i></button>`
                            let btnEliminarBL = `<button class='btn-eliminar btn btn-xs btn-danger eliminarBackLog' title='Eliminar BackLog.'><i class='fas fa-trash'></i></button>`
                            let btnEvidencias = `<button type="button" class="btn btn-primary btn-xs cargarEvidencia" title="Carga de evidencia."><i class="far fa-folder-open"></i></button>`
                            let btnRequisiciones = `<button class='btn-editarRequisicion btn btn-xs btn-success editarRequisicion' title='Actualizar número de requisición.'><i class="fas fa-list-ul"></i></button>`
                            let btnOC = `<button class='btn-editarOC btn btn-xs btn-success editarOC' title='Actualizar número de OC.'><i class='fas fa-shopping-cart'></i></button>`
                            let btnProcesoInstalacion = `<button class="btn btn-xs btn-primary confirmarProcesoInstalacion" title="Confirmar que ya se dio salida las piezas de almacen."><i class="fas fa-sign-out-alt"></i></button>`
                            let btnCrearOT = `<button class="btn btn-xs btn-primary crearOT" title="Crear OT"><i class="fas fa-scroll"></i></button>`
                            let btnImprimirOT = `<button class="btn btn-xs btn-primary imprimirOT" title="Imprimir OT."><i class="fas fa-print"></i></button>`
                            let btnOrdenBL = `<button class='btn btn-xs btn-primary imprimirOrdenBL' title='Imprimir orden BL.'><i class='fas fa-print'></i></button>`;
                            let btnBackLogInstalado = `<button class='btn btn btn-xs btn-success confirmarBackLogInstalado' title="Confirmar instalación del BackLog."><i class="fas fa-check-double"></i></button>`

                            switch (row.estatus) {
                                case "Elaboración de Inspección (20%)":
                                    return `${btnActualizarBL} ${btnEliminarBL} ${btnEvidencias} ${btnRequisiciones} ${btnOrdenBL} ${btnCrearOT}`
                                case "Elaboración de Requisición (40%)":
                                    return `${btnActualizarBL} ${btnEliminarBL} ${btnEvidencias} ${btnRequisiciones} ${btnOC} ${btnOrdenBL} ${btnCrearOT} ${btnProcesoInstalacion}`
                                case "Elaboración de OC (50%)":
                                    return `${btnActualizarBL} ${btnEliminarBL} ${btnEvidencias} ${btnRequisiciones} ${btnOC} ${btnOrdenBL} ${btnCrearOT} ${btnProcesoInstalacion}`
                                case "Suministro de Refacciones (60%)":
                                    return `${btnActualizarBL} ${btnEliminarBL} ${btnEvidencias} ${btnRequisiciones} ${btnOC} ${btnOrdenBL} ${btnCrearOT} ${btnProcesoInstalacion}`
                                case "Rehabilitación Programada (80%)":
                                    return `${btnActualizarBL} ${btnEliminarBL} ${btnEvidencias} ${btnRequisiciones} ${btnOC} ${btnOrdenBL} ${btnCrearOT} ${btnProcesoInstalacion}`
                                case "Proceso de Instalación (90%)":
                                    return `${btnActualizarBL} ${btnEliminarBL} ${btnEvidencias} ${btnRequisiciones} ${btnOC} ${btnOrdenBL} ${btnCrearOT} ${btnImprimirOT} ${btnBackLogInstalado}`
                                case "BackLogs instalado (100%)":
                                    return `${btnEvidencias} ${btnRequisiciones} ${btnOC} ${btnOrdenBL} ${btnCrearOT} ${btnImprimirOT}`
                            }
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblBL_CatBackLogs.on("click", ".editarBackLog", function () {
                        fncLimpiarCtrlsCrearEditarBackLog();

                        const rowData = dtBackLogs.row($(this).closest("tr")).data();

                        let optionNoEconomico = cboCC.find(`option[data-prefijo="${rowData.noEconomico}"]`);
                        let valNoEconomico = optionNoEconomico.val();
                        btnCrearEditarBackLog.attr("data-noEconomico", rowData.noEconomico);
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
                        btnCrearEditarNumRequisicion.attr("data-BL", rowData.id);
                        fncGetMotivosCancelacion();
                        fncGetRequisiciones();
                        modalCrearEditarNumRequisicion.modal("show");
                    });

                    tblBL_CatBackLogs.on("click", ".editarOC", function () {
                        divOrdenesCompra.html('');
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        modalCrearEditarNumOC.attr("data-id", rowData.id);
                        fncGetOC();
                        modalCrearEditarNumOC.modal("show");
                    });

                    tblBL_CatBackLogs.on("click", ".confirmarRehabilitacionProgramada", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("Rehabilitacion Programada (80%)", "Confirmar que las refacciones llegaron al almacen.",
                            "Confirmar", "Cancelar", () => fncConfirmarRehabilitacionProgramada(idBL));
                    });

                    tblBL_CatBackLogs.on("click", ".confirmarProcesoInstalacion", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("Proceso de Instalacion (90%)", "Confirmar que ya se dio salida las piezas de almacen.", "Confirmar", "Cancelar",
                            () => fncConfirmarProcesoInstalacion(idBL));
                    });

                    tblBL_CatBackLogs.on("click", ".confirmarBackLogInstalado", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("Backlog instalado (100%)", "Confirmar la instalación del BackLog.", "Confirmar", "Cancelar",
                            () => fncConfirmarBackLogInstalado(idBL));
                    });

                    tblBL_CatBackLogs.on("click", ".imprimirOT", function (e) {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("Imprimir OT", "¿Desea imprimir la orden de trabajo?", "Confirmar", "Cancelar", () => fncImprimirOT(idBL));
                    });

                    tblBL_CatBackLogs.on("click", ".crearOT", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("Captura OT", "¿Desea capturar la orden de trabajo?", "Confirmar", "Cancelar", () => fncPrecargarOT(idBL));
                    });

                    tblBL_CatBackLogs.on('click', '.cargarEvidencia', function () {
                        let rowData = dtBackLogs.row($(this).closest("tr")).data();
                        $('#mdlEvidencias').modal('show');
                        $("#rowDataId").val(rowData.id);
                        btnGuardarEvidencia.attr('data-id', $('.subirArchivos').attr("data-id"));
                        $("#titleCurso").empty();
                        $('#inputExamen').val('');
                        $('#lblTexto1').text('Ningún archivo seleccionado');
                        $('#inputExamen').change(function () {
                            $('#lblTexto1').text($(this)[0].files[0].name);
                        });
                        fncGetArchivos(rowData.id);

                    });

                    tblBL_CatBackLogs.on("click", ".verDescripcionBL", function () {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        txtDescripcionBL.val(rowData.descripcion);
                        mdlDescripcionBL.modal("show");
                    });

                    tblBL_CatBackLogs.on("click", ".imprimirOrdenBL", function (e) {
                        const rowData = dtBackLogs.row($(this).closest("tr")).data();
                        let idBL = parseFloat(rowData.id);
                        Alert2AccionConfirmar("Imprimir orden BL", "¿Desea imprimir la <b>Orden de BackLog</b>?", "Confirmar", "Cancelar", () => fncImprimirOrdenBL(idBL));
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] }
                    // { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function fncImprimirOrdenBL(idBL) {
            if (idBL > 0) {
                var path = `/Reportes/Vista.aspx?idReporte=224&idBL=${idBL}`;
                $("#report").attr("src", path);
                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
            } else {
                Alert2Warning("Ocurrió un error al generar la <b>Orden de BackLog</b>.")
            }
        }

        function fncImprimirOT(idBL) {
            let obj = new Object();
            obj = {
                idBL: idBL
            };
            axios.post("GetIDOT", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let idOT = response.data.idOT;
                    if (idOT > 0 && idBL > 0) {
                        var path = "/Reportes/Vista.aspx?idReporte=45&idOT=" + idOT + "&idBL=" + idBL;
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

        function fncPrecargarOT(idBL) {
            document.location.href = '/OT/CapturaOT?idBL=' + idBL;
            // document.location.href = '/OT/CapturaOT?idBL=' + 1006;
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
                            fncGetBackLogsPresupuesto(_noEconomico);
                            fncFillTablaSolicitudPpto();
                        } else {
                            Alert2Error(message);
                        }
                    }).catch(error => AlertaGeneral(error.message));
            } else {
                Alert2Error("Ocurrió un error al intentar eliminar el BackLog.");
            }
        }

        function initTablaSolicitudPpto() {
            dtSolicitudPpto = tblBL_SolicitudPpto.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'folioPpto', title: 'Folio ppto' },
                    { data: 'fechaActual', title: 'Fecha' },
                    { data: 'noEconomico', title: 'C.C' },
                    { data: 'horas', title: 'Horas' },
                    {
                        data: 'motivo', title: 'Motivo',
                        render: function (type, data, row) {
                            return cboCC.attr("data-motivo");
                        }
                    },
                    {
                        title: 'Estimado total',
                        render: function (type, data, row) {
                            return maskNumero2DCompras(row.presupuestoEstimado);
                        }
                    },
                    {
                        render: function (type, data, row) {
                            return `<button class="btn btn-warning solicitarAutorizacion">Solicitar autorización</button>`;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblBL_SolicitudPpto.on('click', '.solicitarAutorizacion', function () {
                        let rowData = dtSolicitudPpto.row($(this).closest('tr')).data();
                        let idInsp = cboCC.attr("data-idInsp");
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea enviar una solicitud de autorización?', 'Confirmar', 'Cancelar',
                            () => fncSolicitarAutorizacion(idInsp, rowData.noEconomico, rowData.folioPpto, rowData.presupuestoEstimado));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncSolicitarAutorizacion(idInsp, noEconomico, folioPpto, presupuestoEstimado) {
            //#region SE OBTIENE EL LISTADO DE BACKLOGS
            var datosBL = tblBL_CatBackLogs.DataTable().rows().data();
            let arrBL = new Array();
            tblBL_CatBackLogs.find('tbody').find('tr').each(function (i, value) {
                // objBL = {
                //     folioBL: datosBL[i].folioBL
                // };
                // arrBL.push(objBL);
                arrBL.push(datosBL[i].folioBL);
            });
            //#endregion

            //#region GUARDAR LA SOLICITUD DE PPTO
            let objGuardar = {}
            objGuardar = {
                idInsp: idInsp,
                noEconomico: noEconomico,
                cc: cboCC.val(),
                folioPpto: folioPpto,
                presupuestoEstimado: presupuestoEstimado,
                lstFoliosBL: arrBL
            };
            axios.post("SolicitarAutorizacion", objGuardar).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Éxito al solicitar la autorización.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
            //#endregion
        }

        function fncFillTablaSolicitudPpto() {
            let obj = new Object();
            obj = {
                cc: cboCC.val(),
                areaCuenta: cboProyecto.val()
            }
            axios.post('FillTablaSolicitudPpto', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (response.data.lstSolicitudPpto != null) {
                        dtSolicitudPpto.clear();
                        dtSolicitudPpto.rows.add(response.data.lstSolicitudPpto);
                        dtSolicitudPpto.draw();
                    } else {
                        dtSolicitudPpto.clear();
                        dtSolicitudPpto.draw();
                        //FillTablaSolicitudPpto
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
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
                    fncGetBackLogsPresupuesto(_noEconomico);
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
                    fncGetBackLogsPresupuesto(_noEconomico);
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
                    Alert2Exito(`Se ha actualizado el estatus del BackLog a "BackLog instalado (100%)".`);
                    fncGetBackLogsPresupuesto(_noEconomico);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
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

        function fncCrearManoObra() {
            let esCrearEditar = fncValidarCrearEditarManoObra();
            let existoRegistro = false;
            if (esCrearEditar && rowEditarManoObra == 0) {
                var rowCount = $("#tblBL_ManoObra tbody tr").length;
                rowCount++;

                let manoObraID = "manoObraID" + rowCount;

                let tr = $("<tr>", { id: manoObraID, class: "ContPartidasManoObra", style: "text-align: center;" });
                let tdContPartidasManoObra = $("<td>", { text: rowCount });
                let tdDescripcion = $("<td>", { text: txtDescripcionManoObra.val() });
                let td = $("<td>");
                let btnEditar = $("<button>", { class: "btn btn-editar btn-xs btn-warning btnEditarManoObra" });
                let iEditar = $("<i>", { class: "fas fa-pencil-alt" });
                let btnEliminar = $("<button>", { class: "btn btn-eliminar btn-xs btn-danger btnEliminarManoObra" });
                let iEliminar = $("<i>", { class: "fas fa-trash" });

                td.append(btnEditar);
                td.append(" ");
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

        function fncLimpiarCtrlsCrearEditarManoObra() {
            btnCrearEditarManoObra.attr("data-id", 0);
            txtDescripcionManoObra.val("");
        }

        function fncGetDatosPartes() {
            let PartidasPartes = 0;
            let Partida = "";
            let Cantidad = "";
            let Parte = "";
            let Articulo = "";
            let CostoPromedio = "";
            let TipoMoneda = "";
            arrPartes = [];

            $("#tblBL_Partes tbody tr").each(function (index) {
                $(this).children("td").each(function (index2) {
                    switch (index2) {
                        case 0:
                            PartidasPartes++;
                            Partida = $(this).text();
                            break;
                        case 1:
                            Cantidad = $(this).text();
                            break;
                        case 2:
                            Parte = $(this).text();
                            break;
                        case 3:
                            Articulo = $(this).text();
                            break;
                        case 4:
                            CostoPromedio = $(this).text();
                            break;
                        case 5:
                            tipoMoneda = $(this).text();
                            break;
                    }
                })

                let objJS = {};
                objJS.Cantidad = Cantidad;
                objJS.Parte = Parte;
                objJS.Articulo = Articulo;
                objJS.CostoPromedio = CostoPromedio;
                objJS.tipoMoneda = tipoMoneda;
                arrPartes.push(objJS);
            });
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
                areaCuenta: 1010
            };

            return {
                objBL,
                datosPartes: arrPartes,
                datosManoObra: arrManoObra,
                esParte: chkParte.prop("checked"),
                esManoObra: chkMO.prop("checked"),
                esActualizarCC: cboCC.attr("esActualizarCC"),
                esObra: false,
                idUsuarioResponsable: 0
            };
        }

        function fncGetDatosPartes() {
            let PartidasPartes = 0;
            let Partida = "";
            let Insumo = "";
            let Cantidad = "";
            let Parte = "";
            let Articulo = "";
            let CostoPromedio = "";
            let TipoMoneda = "";
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
                        case 5:
                            CostoPromedio = $(this).text();
                            break;
                        case 6:
                            TipoMoneda = $(this).text();
                            break;
                    }
                })

                let objJS = {};
                objJS.Insumo = Insumo;
                objJS.Cantidad = Cantidad;
                objJS.Parte = Parte;
                objJS.Articulo = Articulo;
                objJS.CostoPromedio = CostoPromedio;
                objJS.TipoMoneda = TipoMoneda;
                arrPartes.push(objJS);
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

        function initTablaManoObra() {
            dtManoObra = tblBL_ManoObra.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
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
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });

            tblBL_ManoObra_tbody.empty();
        }

        function fncValidarCrearEditarBL() {
            let strMensajeError = "";
            let esCrearEditar = true;
            if (cboCC.val() == "" && btnCrearEditarBackLog.attr("data-id") <= 0) { strMensajeError += "Es necesario seleccionar una máquina."; }
            if (txtFecha.val() == "") { strMensajeError += "<br>Es necesario ingresar la fecha de inspección."; }
            if (txtHoras.val() == "") { strMensajeError += "<br>Es necesario ingresar las horas."; }
            if (txtDescripcion.val() == "") { strMensajeError += "<br>Es necesario ingresar la descripción."; }
            if (cboConjunto.val() == "") { strMensajeError += "<br>Es necesario seleccionar un conjunto."; }
            if (cboSubconjunto.val() == "" || cboSubconjunto.val() == null) { strMensajeError += "<br>Es necesario seleccionar un subconjunto."; }
            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                esCrearEditar = false;
            }
            return esCrearEditar;
        }

        function fncCrearEditarBackLog() {
            let esCrearEditar = fncValidarCrearEditarBL();
            if (esCrearEditar) {
                let objBL = fncGetDatosFormulario();
                axios.post("CrearEditarBackLog", objBL).then(response => { // OMAR
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito("Se registro correctamente el BackLog.");
                        fncLimpiarCtrlsCrearEditarBackLog();
                        fncGetBackLogsPresupuesto(_noEconomico);
                        fncFillTablaSolicitudPpto();
                    } else {
                        Alert2Error("Ocurrió un error al registrar el BackLog.");
                    }
                }).catch(error => Alert2Error(error.message));
            }
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

        function fncColorearCeldaEstatus() {
            tblBL_CatBackLogs.find("tbody tr").each(function (index) {
                $(this).addClass("rowHoverRegistroObra");
                $(this).children("td").each(function (index2) {
                    switch (index2) {
                        case 7:
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

        function fncGetProyectoID() {
            let objBackLog = new Object();

            // let optionAreaCuenta = cboProyecto.find(`option[value="${cboProyecto.val()}"]`);
            // let prefijoAreaCuenta = optionAreaCuenta.attr("data-prefijo");
            objBackLog = {
                areaCuenta: cboProyecto.val(),
                esObra: false
            };
            return objBackLog;
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
                        fncGetManoObra();
                        fncGetBackLogsPresupuesto(btnCrearEditarBackLog.attr("data-noEconomico"));
                    }
                }).catch(error => AlertaGeneral(error.message));
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

        function fncCrearParte() {
            let esCrearEditar = fncValidarCrearEditarParte();
            let existoRegistro = false;
            if (esCrearEditar && rowEditarParte == 0) {
                var rowCount = $("#tblBL_Partes tbody tr").length;
                rowCount++;

                let tr = $("<tr>", { id: rowCount, class: "ContPartidasParte", style: "text-align: center;" });
                let tdContPartidasParte = $("<td>", { text: rowCount });
                let tdInsumo = $("<td>", { text: txtInsumo.val() });
                let tdCantidad = $("<td>", { text: txtCantidad.val() });
                let tdParte = $("<td>", { text: txtParte.val() });
                let tdArticulo = $("<td>", { text: txtArticulo.val() });
                let tdCostoPromedio = $("<td>", { text: txtTotal.val() });
                let tdUnidad = $("<td>", { text: txtUnidad.val() });
                let tdTipoMoneda = $("<td>", { text: cboTipoMoneda.val() == 1 ? "MXN" : cboTipoMoneda.val() == 2 ? "USD" : cboTipoMoneda == 3 ? "COP" : "SOL" });
                let td = $("<td>", { text: "" });
                let btnEditar = $("<button> ", { class: "btn btn-editar btn-xs btn-warning btnEditarParte" });
                let iEditar = $("<i>", { class: "fas fa-pencil-alt" });
                let btnEliminar = $("<button>", { class: "btn btn-eliminar btn-xs btn-danger btnEliminarParte" });
                let iEliminar = $("<i>", { class: "fas fa-trash" });

                td.append(btnEditar);
                td.append(" ");
                td.append(btnEliminar);
                btnEditar.append(iEditar);
                btnEliminar.append(iEliminar);

                tr.append(tdContPartidasParte, tdInsumo, tdCantidad, tdParte, tdArticulo, tdCostoPromedio, tdUnidad, tdTipoMoneda, td);
                tr.data = {
                    contadorPartida: ContPartidasParte,
                    insumo: txtInsumo.val(),
                    cantidad: txtCantidad.val(),
                    parte: txtParte.val(),
                    articulo: txtArticulo.val(),
                    costoPromedio: txtTotal.val(),
                    unidad: txtUnidad.val(),
                    tipoMoneda: cboTipoMoneda.val()
                };
                tblBL_Partes_tbody.append(tr);
                existoRegistro = true;
            } else if (esCrearEditar) {
                $("#" + rowEditarParte + "").closest("tr").find("td").eq(1).html(txtInsumo.val().trim());
                $("#" + rowEditarParte + "").closest("tr").find("td").eq(2).html(txtCantidad.val().trim());
                $("#" + rowEditarParte + "").closest("tr").find("td").eq(3).html(txtParte.val().trim());
                $("#" + rowEditarParte + "").closest("tr").find("td").eq(4).html(txtArticulo.val().trim());
                $("#" + rowEditarParte + "").closest("tr").find("td").eq(5).html(txtTotal.val().trim());
                $("#" + rowEditarParte + "").closest("tr").find("td").eq(6).html(txtUnidad.val().trim());
                $("#" + rowEditarParte + "").closest("tr").find("td").eq(7).html(cboTipoMoneda.val() == 1 ? "MXN" : cboTipoMoneda.val() == 2 ? "USD" : cboTipoMoneda == 3 ? "COP" : "SOL");
                rowEditarParte = 0;
                existoRegistro = true;
            }
            if (existoRegistro) { modalCrearParte.modal("hide"); }
        }

        function fncCrearEditarParte() {
            let esCrearEditar = fncValidarCrearEditarParte();
            if (esCrearEditar) {
                if (chkParte.prop("checked")) {
                    let objParte = new Object();
                    objParte = {
                        insumo: txtInsumo.val().trim(),
                        cantidad: txtCantidad.val().trim(),
                        parte: txtParte.val().trim(),
                        articulo: txtArticulo.val().trim(),
                        // costoPromedio: txtCostoPromedio.val().trim(), SE OBTIENE EL INPUT TOTAL
                        costoPromedio: txtTotal.val().trim(),
                        unidad: txtUnidad.val().trim(),
                        id: btnCrearEditarParte.attr("data-id"),
                        tipoMoneda: cboTipoMoneda.val(),
                        idBackLog: btnCrearEditarBackLog.attr("data-id")
                    };
                    axios.post("CrearEditarParte", objParte).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            fncGetPartes();
                            modalCrearParte.modal("hide");
                            fncGetManoObra();
                            fncGetBackLogsPresupuesto(btnCrearEditarBackLog.attr("data-noEconomico"));
                        } else {
                            Alert2Error(message)
                        }
                    }).catch(error => Alert2Error(error.message));
                }
            }
        }

        function fncValidarCrearEditarParte() {
            fncBorderDefault();
            let esCrearEditar = true;
            let strMensajeError = "";
            if ($.trim(txtInsumo.val()) == "") { txtInsumo.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtCantidad.val()) == "") { txtCantidad.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtParte.val()) == "") { txtParte.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtArticulo.val()) == "") { txtArticulo.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtCostoPromedio.val()) == "") { txtCostoPromedio.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                esCrearEditar = false;
                Alert2Warning(strMensajeError);
            }
            return esCrearEditar;
        }

        function fncBorderDefault() {
            //#region CAPTURA PARTES
            txtInsumo.css("border", "1px solid #CCC");
            txtCantidad.css("border", "1px solid #CCC");
            txtParte.css("border", "1px solid #CCC");
            txtArticulo.css("border", "1px solid #CCC");
            txtCostoPromedio.css("border", "1px solid #CCC");
            //#endregion
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

        function fncGetDatosCC() {
            fncGetDatosMaquina();
            // fncGetHorometroActual();
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

        function fncGetDatosMaquina() {
            let optionCC = cboCC.find(`option[value="${cboCC.val()}"]`);
            let prefijoCC = optionCC.attr("data-prefijo");
            let objNoEconomico = new Object();
            objNoEconomico = {
                areaCuenta: 1010,
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
                    Alert2Warning(`${response.data.message}`);
                    txtModelo.val("");
                    txtGrupo.val("");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetUltimoFolio() {
            let obj = new Object();
            obj = {
                esObra: false
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

        function fncFillCboCC(areaCuenta) {
            cboCC.fillCombo("/BackLogs/FillCboCC", { areaCuenta: 1010, esObra: false }, false);
            cboCC.select2({
                width: "resolve"
            });
        }

        function initTablaEnPrograma() {
            dtEnPrograma = tblEnPrograma.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    {
                        title: "Abrir ppto.",
                        render: function (type, data, row) {
                            return "<th></th>";
                        }
                    },
                    { data: 'id', visible: false },
                    { data: 'partida', title: 'Partida' },
                    { data: 'noEconomico', title: 'Económico' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'modelo', title: 'Modelo' },
                    { data: 'horas', title: 'Horas' },
                    { data: 'motivo', title: 'Motivo' },
                    {
                        data: 'fechaProgramacion', title: 'Fecha programación',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    {
                        data: 'fechaRequerido', title: 'Fecha requerido',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblEnPrograma.on('click', '.select-checkbox', function () {
                        let rowData = dtEnPrograma.row($(this).closest('tr')).data();
                        let noEconomico = rowData.noEconomico;
                        let option = cboCC.find(`option[data-prefijo="${noEconomico}"]`);
                        let valEconomico = option.val();
                        cboCC.val(valEconomico);
                        cboCC.trigger("change");
                        cboCC.attr("data-motivo", rowData.motivo);
                        cboCC.attr("data-idInsp", rowData.id);

                        txtHoras.val(rowData.horas)
                        _noEconomico = noEconomico;
                        fncGetBackLogsPresupuesto(noEconomico);
                        fncFillTablaSolicitudPpto();
                    });

                    // tblEnPrograma.on('click', '.abrirPresupuesto', function () {

                    //     // llenarCombo();

                    //     // $('#tblEnPrograma tr').each(function(row) {  

                    //     //  var cc = $(this).find("td:nth-child(2)").html();                   

                    //     // // $("#cboCC").val(cc);
                    //     // // $('#cboCC').change();
                    //     //     // if ($(this).find("td").eq(2).length > 0) {
                    //     //     //  var cutomerId = $(this).find("td:first").html();
                    //     //     //     var CC =  $(this).find("td").eq(2).html(); 
                    //     //     // }                                        


                    //     // });
                    // });
                },
                columnDefs: [{
                    orderable: false,
                    className: 'select-checkbox',
                    targets: 0
                }, {
                    className: 'dt-center', 'targets': '_all'
                }],
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                }
            });
        }

        function fncGetBackLogsPresupuesto(noEconomico) {
            let obj = new Object();
            obj = {
                noEconomico: noEconomico
            }
            axios.post('GetBackLogsPresupuesto', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    tblBL_CatBackLogs_tbody.empty();
                    dtBackLogs.clear();
                    dtBackLogs.rows.add(response.data.lstBackLogsPresupuesto);
                    dtBackLogs.draw();
                    fncColorearCeldaEstatus();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function llenarCombo() {
            if ($('.abrirPresupuesto').prop('checked')) {
                $("#tblEnPrograma").find('td:eq(2)').each(function () {
                    let rowData = dtEnPrograma.row($(this).closest('tr')).data();
                    valor = $(this).html();
                    combo = valor;
                    cboCC.val(combo);
                    cboCC.change();
                });
            }
        }

        function fncGetProgramaInspeccionTMC() {
            let objFiltro = new Object();
            objFiltro = {
                motivo: cboFiltroMotivo.val(),
                modelo: cboFiltroModelo.val()
            };
            axios.post("GetProgramaInspeccionTMC", { objFiltro: objFiltro }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtEnPrograma.clear();
                    dtEnPrograma.rows.add(response.data.lstProgramaInspTMC);
                    dtEnPrograma.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncFillCboProyectosObra() {
            let idProyecto = cboProyecto.val();
            if (idProyecto == null) {
                cboProyecto.fillCombo("/BackLogs/FillAreasCuentasTMC", {}, false);
                cboProyecto.select2({
                    width: "resolve"
                });
            }
        }

        function fncFillCboModelos() {
            cboFiltroModelo.fillCombo("/Backlogs/FillCboModelo", {}, false);
            cboFiltroModelo.select2({
                width: "resolve"
            });
        }

        function fncFillCboGrupos() {
            cboFiltroGrupo.fillCombo("/BackLogs/FillCboGrupo", {}, false);
            cboFiltroGrupo.select2({
                width: "resolve"
            });
        }

        function fncHabilitarDeshabilitarControles() {
            if (cboProyecto.val() != "") {
                cboFiltroMotivo.attr("disabled", false);
                cboFiltroTipo.attr("disabled", false);
                cboFiltroModelo.attr("disabled", false);
                cboFiltroGrupo.attr("disabled", false);
                btnFiltroBuscar.attr("disabled", false);
                btnFiltroLimpiar.attr("disabled", false);
                // cboCC.attr("disabled", false);
                // txtModelo.attr("disabled", false);
                // txtGrupo.attr("disabled", false);
                txtFecha.attr("disabled", false);
                txtHoras.attr("disabled", false);
                // txtFolio.attr("disabled", false);
                txtDescripcion.attr("disabled", false);
                cboConjunto.attr("disabled", false);
                cboSubconjunto.attr("disabled", false);
                chkParte.attr("disabled", false);
                chkMO.attr("disabled", false);
                // btnModalAbrirParte.attr("disabled", false);
                // btnModalAbrirManoObra.attr("disabled", false);
                btnCargarEvidencias.attr("disabled", false);
                btnLimpiarFormCrearEditarBackLog.attr("disabled", false);
                btnCrearEditarBackLog.attr("disabled", false);
            } else {
                cboFiltroMotivo.attr("disabled", true);
                cboFiltroTipo.attr("disabled", true);
                cboFiltroModelo.attr("disabled", true);
                cboFiltroGrupo.attr("disabled", true);
                btnFiltroBuscar.attr("disabled", true);
                btnFiltroLimpiar.attr("disabled", true);
                cboCC.attr("disabled", true);
                txtModelo.attr("disabled", true);
                txtGrupo.attr("disabled", true);
                txtFecha.attr("disabled", true);
                txtHoras.attr("disabled", true);
                txtFolio.attr("disabled", true);
                txtDescripcion.attr("disabled", true);
                cboConjunto.attr("disabled", true);
                cboSubconjunto.attr("disabled", true);
                chkParte.attr("disabled", true);
                chkMO.attr("disabled", true);
                // btnModalAbrirParte.attr("disabled", true);
                // btnModalAbrirManoObra.attr("disabled", true);
                btnCargarEvidencias.attr("disabled", true);
                btnLimpiarFormCrearEditarBackLog.attr("disabled", true);
                btnCrearEditarBackLog.attr("disabled", true);
            }
            cboFiltroMotivo.select2();
            cboFiltroTipo.select2();
            cboFiltroModelo.select2();
            cboFiltroGrupo.select2();
        }

        function fncDatepicker() {
            txtFecha.datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: "1900:2030"
            }).datepicker("setDate", new Date());
        }

        function fncFillTipoMaquinariaTMC() {
            cboFiltroTipo.fillCombo("/BackLogs/FillTipoMaquinariaTMC", {}, false);
            cboFiltroTipo.select2();
        }
        //#endregion

        //#region FUNCIONES CATALOGO CONJUNTOS
        function initTablaConjuntos() {
            dtConjuntos = tblConjuntosCatConjuntos.DataTable({
                language: dtDicEsp,
                destroy: true,
                ordering: true,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: true,
                info: false,
                columns: [
                    { data: 'descripcion', title: 'Conjunto' },
                    { data: 'abreviacion', title: 'Abreviación' },

                    { data: 'id', title: 'id', visible: false },
                    {
                        render: function (data, type, row) {
                            let editarConjunto =
                                `<button class='btn-editar btn btn-warning editarConjunto' data-id="${row.id}" title="Actualizar conjunto."><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let eliminarConjunto =
                                `<button class='btn-eliminar btn btn-danger eliminarConjunto' data-id="${row.id}" title="Eliminar conjunto"><i class="far fa-trash-alt"></i></button>`;
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
                ordering: true,
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
                            let editarSubconjunto =
                                `<button class='btn-editar btn btn-warning editarSubconjunto' data-id="${row.id}" title="Actualizar subconjunto."><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let eliminarSubconjunto =
                                `<button class='btn-eliminar btn btn-danger eliminarSubconjunto' data-id="${row.id}" title="Eliminar subconjunto."><i class="far fa-trash-alt"></i></button>`;
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
        function fncPrecargarRequisicion() {
            let idBL = btnCrearEditarNumRequisicion.attr("data-BL");
            document.location.href = '/Enkontrol/Requisicion/Solicitar?idBL=' + idBL;
        }

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
                            // fncGetBackLogs();
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
                numRequisicion: txtCrearEditarNumRequisicionManual.val(),
                idBackLog: btnCrearEditarNumRequisicion.attr("data-bl")
            }
            axios.post("CrearEditarRequisicion", objCrearEditarNumRequisicion).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha registrado con éxito la requisición.");
                    fncGetRequisiciones(false);
                    fncGetBackLogsPresupuesto(_noEconomico);
                    modalCrearEditarNumRequisicionManual.modal("hide");
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
                    { data: 'numOC', title: 'Núm. OC' },
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
            let idBackLog = modalCrearEditarNumOC.attr("data-id");

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
                    { data: 'numero', title: 'Núm. OC' },
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
                    { data: 'numero', title: 'Núm. OC' },
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
                idBackLog: modalCrearEditarNumOC.attr("data-id")
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
                    { data: 'nombreArchivo', title: 'Archivo' },
                    { data: 'tipoEvidencia', title: 'Tipo evidencia' },
                    {
                        render: function (type, data, row) {
                            return `<button class="btn btn-danger eliminarEvidencia" data-id="${row.id}"><i class="fas fa-trash"></i></button>`;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblEvidencias.on('click', '.eliminarEvidencia', function () {
                        let rowData = dtEvidencias.row($(this).closest('tr')).data();
                        let id = parseFloat(rowData.id);
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarArchivo(id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
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
                }
                else {
                    Alert2Error(message)
                }

            }).catch(error => Alert2Error(error.message));
        }

        const subiendoArchivo = function () {
            var data = fncGetEvidenciaParaGuardar();
            let objPresupuesto = new Object();
            objPresupuesto = {
                tipoEvidencia: cboMdlEvidenciasTipoEvidencia.val()
            };
            if (cboMdlEvidenciasTipoEvidencia.val() != "") {
                axios.post('/BackLogs/postSubirArchivos', data,
                    { params: tipoEvidencia = objPresupuesto },
                    { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                        let { success, datos, message } = response.data;
                        if (success) {
                            Alert2Exito('Se ha registrado con éxito la evidencia.');
                            $('#lblTexto1').text('Ningún archivo seleccionado');
                            fncGetArchivos($("#rowDataId").val());
                            cboMdlEvidenciasTipoEvidencia[0].selectedIndex = 0;
                            cboMdlEvidenciasTipoEvidencia.trigger("change");
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
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
            axios.post('EliminarArchivos', { id: id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha eliminado con éxito.");
                    fncGetArchivos(id);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => BackLogs = new BackLogs())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();