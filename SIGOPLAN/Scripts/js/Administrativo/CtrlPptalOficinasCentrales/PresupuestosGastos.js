(() => {
    $.namespace('CtrlPptalOfCE.PresupuestosGastos');

    //#region CONST
    const montoGastoPpto = $("#montoGastoPpto");
    const montoGastoPptoInforme = $("#montoGastoPptoInforme");
    const montoIngresoPpto = $("#montoIngresoPpto");
    const montoIngresoPptoInforme = $("#montoIngresoPptoInforme");
    const txtRatioGastoVsIngresoAcumuladoPpto = $("#txtRatioGastoVsIngresoAcumuladoPpto");
    const montoRealPpto = $("#montoRealPpto");
    const porcentajeCumplimientoIngresoGastoPpto = $("#porcentajeCumplimientoIngresoGastoPpto");
    const porcentajeCumplimientoIngresoGastoPptoInforme = $("#porcentajeCumplimientoIngresoGastoPptoInforme");
    const gastoIngresoBarPpto = $("#gastoIngresoBarPpto");
    const gastoIngresoBarPptoInforme = $("#gastoIngresoBarPptoInforme");
    //#endregion

    //#region CONST
    const cboFiltroAnio = $('#cboFiltroAnio');
    const cboFiltroDivisiones = $('#cboFiltroDivisiones');
    const cboFiltroCC = $('#cboFiltroCC');
    const cboFiltroMes = $('#cboFiltroMes');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnCostosTotales = $('#btnCostosTotales');

    const tblCapturas = $('#tblCapturas');
    const graficaArcoPresupuesto = $('#graficaArcoPresupuesto');
    const graficaArcoPresupuestoInforme = $('#graficaArcoPresupuestoInfrome');
    // const graficaArcoGastoIngreso = $('#graficaArcoGastoIngreso');
    const graficaArcoCumplimiento = $('#graficaArcoCumplimiento');
    const panelGraficas = $('#panelGraficas');

    const inpConsultaNivel = $('#inpConsultaNivel');

    const panelGeneral3 = $('#panelGeneral3');
    const gastoIngresoBar = $('#gastoIngresoBar');
    const gastoIngresoBarInforme = $('#gastoIngresoBarInforme');
    const tblNivel3DeIngresosGastos = $('#tblNivel3DeIngresosGastos');

    const panelDetalleNivel4Ctas = $("#panelDetalleNivel4Ctas");
    const tblNivel4Ctas = $("#tblNivel4Ctas");
    let dtNivel4Ctas;

    let dtCapturas;
    let dtNivel4;
    const mdlPlanAccion = $('#mdlPlanAccion');

    const cboFiltroEmpresas = $('#cboFiltroEmpresas');
    //#endregion

    //#region CONST PLAN DE ACCIÓN
    const txtCEPlanAccion = $("#txtCEPlanAccion");
    const txtCEPlanAccionJustificacion = $("#txtCEPlanAccionJustificacion");
    const txtCEPlanAccionCorreoResponsableSeguimiento = $("#txtCEPlanAccionCorreoResponsableSeguimiento");
    const txtCEPlanAccionFechaCompromiso = $("#txtCEPlanAccionFechaCompromiso");
    const btnCEEstatusPlanAccion = $('#btnCEEstatusPlanAccion');
    const btnCEPlanAccion = $('#btnCEPlanAccion');
    const lblBtnCEPlanAccion = $('#lblBtnCEPlanAccion');
    const btnCerrarPlanAccion = $('#btnCerrarPlanAccion');
    // const btnVoBo = $('#btnVoBo');
    //#endregion

    //#region CONST REPORTE PLAN ACCIÓN
    const btnFiltroReportePlanAccion = $("#btnFiltroReportePlanAccion");
    const mdlReportePlanAccion = $("#mdlReportePlanAccion");
    const panelGraficasDashboard = $('#panelGraficasDashboard');
    const btnReportePlanAccionEnviarInforme = $('#btnReportePlanAccionEnviarInforme');
    const botonDescargarReportePlanAccion = $('#botonDescargarReportePlanAccion');
    //const divReportePlanesAccionesHTML = $('#divReportePlanesAccionesHTML');
    const divReportePlanesAccionesHTMLInforme = $('#divReportePlanesAccionesHTMLInforme');
    //#endregion

    //#region CONST PANEL CONTROL EJECUCION (*￣3￣)╭ chulas 💋💋😉😜💕😘😘
    const tblControlEjecucion = $('#tblControlEjecucion');
    const txtCumplimientoControlEjecucion = $('#txtCumplimientoControlEjecucion');
    const txtMesCierreControlEjecucion = $('#txtMesCierreControlEjecucion');
    const tblGastosVsPresupuestos = $('#tblGastosVsPresupuestos');
    const periodoResumen = $('.periodoResumen');
    const panelGeneral0 = $('#panelGeneral0');
    const panelDetalleNivel2 = $('#panelDetalleNivel2');
    const panelDetalleNivel3 = $('#panelDetalleNivel3');
    const panelDetalleNivel4 = $('#panelDetalleNivel4');
    const regresarPanel = $('#regresarPanel');
    const regresarPanel2 = $('#regresarPanel2');
    const mdlDetalleNivel = $('#mdlDetalleNivel');
    const lstBtnDetalleNivel = $('#lstBtnDetalleNivel');
    const btnNivel2 = $('#btnNivel2');
    const btnNivel3 = $('#btnNivel3');
    const btnNivel4 = $('#btnNivel4');
    // const btnPlanAccion = $('#btnPlanAccion');
    const txtBtnNivel2 = $('#txtBtnNivel2');
    const txtBtnNivel3 = $('#txtBtnNivel3');
    const txtBtnNivel4 = $('#txtBtnNivel4');
    const btnCerrarModal = $('#btnCerrarModal');

    const tblNivel4 = $('#tblNivel4');
    const graficaNivel4 = $('#graficaNivel4');

    const montoGasto = $('#montoGasto');
    const montoGastoInforme = $('#montoGastoInforme');
    const montoIngreso = $('#montoIngreso');
    const montoIngresoInforme = $('#montoIngresoInforme');
    const porcentajeCumplimientoIngresoGasto = $('#porcentajeCumplimientoIngresoGasto');
    const porcentajeCumplimientoIngresoGastoInforme = $('#porcentajeCumplimientoIngresoGastoInforme');
    const montoReal = $('#montoReal');
    const montoRealInforme = $('#montoRealInforme');
    const montoGastoMensual = $('#montoGastoMensual');
    const montoGastoMensualInforme = $('#montoGastoMensual');
    const montoIngresoMensual = $('#montoIngresoMensual');
    const montoIngresoMensualInforme = $('#montoIngresoMensualInforme');
    const montoRealMensual = $('#montoRealMensual');
    const montoRealMensualInforme = $('#montoRealMensualInforme');
    const porcentajeCumplimientoIngresoGastoMensual = $('#porcentajeCumplimientoIngresoGastoMensual');
    const porcentajeCumplimientoIngresoGastoMensualInforme = $('#porcentajeCumplimientoIngresoGastoMensualInforme');
    const txtRatioGastoVsIngresoAcumulado = $('#txtRatioGastoVsIngresoAcumulado');
    const txtRatioGastoVsIngresoAcumuladoInforme = $('#txtRatioGastoVsIngresoAcumuladoInforme');
    const txtRatioGastoVsIngresoMensual = $('#txtRatioGastoVsIngresoMensual');
    const txtRatioGastoVsIngresoMensualInforme = $('#txtRatioGastoVsIngresoMensualInforme');

    const mdlAgrupacionConceptosCC = $('#mdlAgrupacionConceptosCC');
    const tblAgrupacionConceptosCC = $('#tblAgrupacionConceptosCC');
    let dtAgrupacionConceptoCC;
    const tblConceptosRelCC = $('#tblConceptosRelCC');
    let dtConceptosRelCC;
    const panelAgrupacionCC = $('#panelAgrupacionCC');
    const panelConceptosPorCC = $('#panelConceptosPorCC');
    const btnCerrarMdlAgrupacionCC = $('#btnCerrarMdlAgrupacionCC');

    let dtControlEjecucion;
    let dtNivel3DeIngresosGastos;

    let gastoTotal = 0;
    let pptoTotal = 0;
    let gastoAcumuladoTotal = 0;
    let pptoAcumuladoTotal = 0;

    const divVerComentario = $("#divVerComentario");
    const txtComentarios = $("#txtComentarios");
    const ulComentarios = $("#ulComentarios");

    //#endregion

    //#region CONST ENVIO INFORME
    const btnFiltroEnvioInforme = $('#btnFiltroEnvioInforme');
    const cboFiltroInformeAnio = $("#cboFiltroInformeAnio");
    const cboFiltroInformeMes = $("#cboFiltroInformeMes");
    const cboFiltroInformeEmpresa = $('#cboFiltroInformeEmpresa');
    const btnFiltroInformeBuscar = $('#btnFiltroInformeBuscar');
    const mdlEnvioInforme = $("#mdlEnvioInforme");
    const tblAF_CtrlPptalOfCe_EnvioInforme = $("#tblAF_CtrlPptalOfCe_EnvioInforme");
    let dtEnvioInforme;
    //#endregion

    //#region CONST CONCEPTOS AGRUPACIÓN CC
    const btnConceptosAgrupacionCC = $('#btnConceptosAgrupacionCC');
    //#endregion

    const tablaAgrupaciones = $('#tablaAgrupaciones');
    const tablaDetalleAnual = $('#tablaDetalleAnual');
    const modalDetalleAnual = $('#modalDetalleAnual');

    let dtAgrupaciones;
    let dtDetalleAnual;

    let imgGraficaCumplimientoPresupuestoAcumulado;
    let imgGraficaCumplimientoPresupuestoMensual;
    let imgGraficaProyeccion;


    let parametro_Anio = 0;
    let parametro_idMes = 0;
    let parametro_idCC = 0;

    PresupuestosGastos = function () {
        const report = $("#report");

        menuConfig = {
            lstOptions: [
                { text: `<i class="fa fa-download"></i> Descargar póliza`, action: "descargarPoliza", fn: parametros => { reportePoliza(true, parametros.poliza, true) } },
                { text: `<i class="fa fa-file"></i> Ver póliza`, action: "visorPoliza", fn: parametros => { reportePoliza(true, parametros.poliza, false) } },
                { text: `<i class="fa fa-download"></i> Descargar póliza detalle`, action: "descargarPolizaDetalle", fn: parametros => { reportePoliza(false, parametros.poliza, true) } },
                { text: `<i class="fa fa-file"></i> Ver póliza detalle`, action: "visorPolizaDetalle", fn: parametros => { reportePoliza(false, parametros.poliza, false) } }
            ]
        };

        (function init() {
            fncListeners();
            //fncGetDataGraficasDashboard();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            initTblNivel4();
            initTblNivel4Ctas();
            initTblCapturas();
            initTblControlEjecucion();
            initTblNivel3DeIngresosGastos();
            initTablaAgrupaciones();
            initTablaDetalleAnual();
            initTblGastosVsPresupuestos();
            initTblEnvioInforme();
            initTblAgrupacionConceptosCC();
            initTblConceptosRelCC();
            //#endregion

            btnFiltroReportePlanAccion.css("display", "none");

            //#region PLAN DE ACCIÓN
            btnCEPlanAccion.on("click", function () {
                fncCEPlanAccion();
            });

            btnCerrarPlanAccion.on("click", function () {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea concluir el plan de acción?', 'Confirmar', 'Cancelar', () => fncCerrarPlanAccion());
            });

            $("body").on("click", ".editarPlanAccion", function () {
                fncGetPlanAccion(0, 0, 0, $(this).attr("data-id"));
            });

            // btnVoBo.on("click", function () {
            //     Alert2AccionConfirmar('¡Cuidado!', '¿Desea indicar visto bueno al plan de acción?', 'Confirmar', 'Cancelar', () => fncIndicarVobo(btnCEPlanAccion.attr("data-id")));
            // });
            //#endregion

            //#region REPORTE PLAN ACCIÓN
            btnFiltroReportePlanAccion.on("click", function () {
                let cc = getValoresMultiples('#cboFiltroCC')[0];
                botonDescargarReportePlanAccion.attr('cc', cc);
                btnReportePlanAccionEnviarInforme.attr('cc', cc);

                //panelGraficasDashboard.html(panelGraficas.html());
                fncGetReportePlanAcciones();
                mdlReportePlanAccion.modal("show");
            });

            btnReportePlanAccionEnviarInforme.on("click", function () {
                fncGenerarReporteCrystalReport($("#graficaArcoPresupuestoInforme").highcharts(), 'chart', 1);
                fncGenerarReporteCrystalReport($("#graficaArcoPresupuestoMensualInforme").highcharts(), 'chart', 2);
                fncGenerarReporteCrystalReport($("#graficaArcoProyeccionInforme").highcharts(), 'chart', 3);

                Alert2AccionConfirmar('¡Cuidado!', '¿Desea enviar el informe de plan de acción?', 'Confirmar', 'Cancelar', () => fncGenerarVariablesSessionReportePlanAccion());
            });
            botonDescargarReportePlanAccion.click(() => {
                fncGenerarReporteCrystalReport($("#graficaArcoPresupuestoInforme").highcharts(), 'chart', 1);
                fncGenerarReporteCrystalReport($("#graficaArcoPresupuestoMensualInforme").highcharts(), 'chart', 2);
                fncGenerarReporteCrystalReport($("#graficaArcoProyeccionInforme").highcharts(), 'chart', 3);

                Alert2AccionConfirmar('¡Cuidado!', '¿Desea descargar el informe de plan de acción?', 'Confirmar', 'Cancelar', () => fncDescargarInformePlanAccion());
            });

            cboFiltroCC.on("change", function () {
                let lstCC = getValoresMultiples('#cboFiltroCC').filter(function (x) { return parseInt(x) })
                if (lstCC.length > 1) {
                    btnFiltroReportePlanAccion.css("display", "none");
                } else if (lstCC.length == 1) {
                    fncVerificarExistenciaPlanAccion();
                } else if (lstCC.length <= 0) {
                    btnFiltroReportePlanAccion.css("display", "none");
                }
            });
            //#endregion

            $("#idSB").on("click", function () {
                let obj = new Object();
                obj = {
                    idCC: 100,
                    anio: 2022,
                    idMes: 1,
                    idConcepto: 49
                }
                axios.post("GetCtasPolizas", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtNivel4Ctas.clear();
                        dtNivel4Ctas.rows.add(response.data.lstGastos);
                        dtNivel4Ctas.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            });

            //#region EVENTOS INDEX
            btnFiltroBuscar.on("click", function () {
                let esVacio = true;
                btnReportePlanAccionEnviarInforme.css("display", "inline-block");
                cboFiltroCC.val().forEach(element => {
                    esVacio = false;
                });

                if (!esVacio) {
                    fncGetDataGraficasDashboard();
                    //cboFiltroCC.trigger("change");
                } else {
                    Alert2Warning("Es necesario seleccionar un centro de costo.");
                }
                // fncGetSumaCapturas();
            });
            //#endregion

            txtBtnNivel2.click(function () {
                tblNivel3DeIngresosGastos.css('display', 'none');
            });

            //#region FILL COMBOS
            $(".select2").select2();
            convertToMultiselect('#cboFiltroCC');

            cboFiltroAnio.fillCombo("FillAnios", {}, false);
            cboFiltroAnio.select2({ width: "100%" });

            fncFillCboEmpresas();

            // cboFiltroAnio.on("change", function () {

            // });

            cboFiltroDivisiones.on("change", function () {
                // if (cboFiltroDivisiones.val() > 0) {
                //     cboFiltroCC.fillCombo("FillCCRelDivisiones", { anio: cboFiltroAnio.val(), divisionID: +cboFiltroDivisiones.val() }, false, 'Todos');
                // } else {
                //     cboFiltroCC.fillCombo("FillUsuarioRelCCPptosAutorizados", { anio: cboFiltroAnio.val() }, false, "Todos");
                //     cboFiltroCC.multiselect('selectAll', false);
                //     cboFiltroCC.multiselect('refresh');
                //     cboFiltroCC.multiselect('deselect', 'Todos');
                //     convertToMultiselect('#cboFiltroCC');
                //     // cboFiltroCC.fillCombo("FillUsuarioRelCC", { anio: cboFiltroAnio.val() }, false);
                // }
                // convertToMultiselect('#cboFiltroCC');
            });

            cboFiltroEmpresas.on("change", function () {
                if (cboFiltroAnio.val() > 0 && cboFiltroMes.val() > 0 && $(this).val() > 0) {
                    // cboFiltroDivisiones.fillCombo("FillDivisiones", { anio: $(this).val() }, false);
                    // cboFiltroDivisiones.select2({ width: "100%" });

                    cboFiltroCC.fillCombo("FillUsuarioRelCCPptosAutorizados", { anio: cboFiltroAnio.val(), idEmpresa: $(this).val() }, false, "Todos");
                    convertToMultiselect('#cboFiltroCC');
                    cboFiltroCC.multiselect('selectAll', false);
                    $("#cboFiltroCC").multiselect('refresh');
                    cboFiltroCC.multiselect('deselect', 'Todos');

                    if (parametro_idCC > 0) {
                        cboFiltroCC.multiselect('deselectAll', false);
                        cboFiltroCC.multiselect('refresh');
                        cboFiltroCC.multiselect("select", parametro_idCC);
                        btnFiltroBuscar.trigger("click");
                    }

                    //#region EN CASO QUE EN EL LISTADO DE CC, SE ENCUENTRE EL 46, SE QUITA SU STRING "cp_" O "arr_"
                    let arrConstruplan = [];
                    let arrArrendadora = [];
                    cboFiltroCC.val().forEach(element => {
                        let objCC = cboFiltroCC.find(`option[value="${element}"]`);
                        let prefijoCC = objCC.attr("data-prefijo");

                        if (prefijoCC == 1) {
                            if (element == "cp_46") {
                                let idCC = element.replace("cp_46", "46");
                                arrConstruplan.push(idCC);
                            } else {
                                arrConstruplan.push(element);
                            }
                        } else if (prefijoCC == 2) {
                            if (element == "arr_46") {
                                let idCC = element.replace("arr_46", "46");
                                arrArrendadora.push(idCC);
                            } else {
                                arrArrendadora.push(element);
                            }
                        }
                    });
                    //#endregion

                    let lstCC = getValoresMultiples('#cboFiltroCC').filter(function (x) { return parseInt(x) })
                    if (lstCC.length > 1) {
                        btnFiltroReportePlanAccion.css("display", "none");
                    } else if (lstCC.length == 1) {
                        fncVerificarExistenciaPlanAccion();
                    } else if (lstCC.length <= 0) {
                        btnFiltroReportePlanAccion.css("display", "none");
                    }
                }
                if ($(this).val() > 1) {
                    btnCostosTotales.parent().css("display", "inline-block");
                }
                else btnCostosTotales.parent().css("display", "none");
            });
            //#endregion

            btnNivel2.on("click", function () {
                panelDetalleNivel2.css('display', 'block');
                panelGeneral0.css('display', 'block');
                panelDetalleNivel3.css('display', 'none');
                panelDetalleNivel4.css('display', 'none');
                panelDetalleNivel4Ctas.css('display', 'none');
                btnNivel2.css('display', 'initial');
                btnNivel3.css('display', 'none');
                btnNivel4.css('display', 'none');
                // btnPlanAccion.css("display", "none");
            });

            btnNivel3.on("click", function () {
                panelDetalleNivel3.css('display', 'block');
                panelDetalleNivel4.css('display', 'none');
                panelDetalleNivel4Ctas.css('display', 'none');
                btnNivel3.css('display', 'initial');
                btnNivel4.css('display', 'none');
                // btnPlanAccion.css("display", "none");
            });

            mdlDetalleNivel.on('hidden.bs.modal', function () {
                panelDetalleNivel2.css('display', 'block');
                panelGeneral0.css('display', 'block');
                panelDetalleNivel3.css('display', 'none');
                panelDetalleNivel4.css('display', 'none');
                panelDetalleNivel4Ctas.css('display', 'none');
                btnNivel2.css('display', 'initial');
                btnNivel3.css('display', 'none');
                btnNivel4.css('display', 'none');
                // btnPlanAccion.css("display", "none");
            });

            //porcentajeCumplimientoIngresoGasto.on('click', function () {
            //    inpConsultaNivel.attr('data-cumplimientoIngreso', 2);
            //    getDetalleNivel2IngresoGasto().done(function (response) {
            //        if (response && response.success) {
            //            txtCumplimientoControlEjecucion.text(`${Math.trunc(response.items.cumplimientoAcumuladoEmpresa)}%`);
            //            if (response.items.cumplimientoAcumuladoEmpresa >= 97) {
            //                txtCumplimientoControlEjecucion.css('color', 'red');
            //            } else if (response.items.cumplimientoAcumuladoEmpresa >= 95) {
            //                txtCumplimientoControlEjecucion.css('color', 'yellow');
            //            } else {
            //                txtCumplimientoControlEjecucion.css('color', 'green');
            //            }
            //            txtMesCierreControlEjecucion.text(response.items.mesCierre);
            //            initGraficaControlEjecucion(response.items.graficaBarraMensual);
            //            periodoResumen.text('DETALLE INGRESOS / GASTOS')
            //            // panelGraficas.fadeToggle('fast', 'linear', function() {
            //            //     panelDetalleNivel2.fadeToggle('fast', 'linear', function() {
            //            //         addRows(tblControlEjecucion, response.items.datosTablaCC);
            //            //     });
            //            // });
            //            SetNombreModal();
            //            mdlDetalleNivel.modal("show");
            //            btnNivel3.css('display', 'none');
            //            btnNivel4.css('display', 'none');
            //            // btnPlanAccion.css("display", "none");
            //            panelDetalleNivel2.css('display', 'block');
            //            panelDetalleNivel3.css('display', 'none');
            //            panelDetalleNivel4.css('display', 'none');
            //            panelDetalleNivel4Ctas.css('display', 'none');
            //            $('.panel-detalle-total').css('display', 'none');
            //            //btnNivel2.css('display','block');

            //            addRows(tblControlEjecucion, response.items.datosTablaCC);

            //            tblControlEjecucion.DataTable().columns(1).visible(false);


            //            $('#tblControlEjecucion_wrapper').find('thead tr th:eq(1)').text('Gasto Mensual');
            //            $('#tblControlEjecucion_wrapper').find('thead tr th:eq(2)').text('Ingreso Mensual');
            //            $('#tblControlEjecucion_wrapper').find('thead tr th:eq(3)').text('Cumplimiento');
            //            $('#tblControlEjecucion_wrapper').find('thead tr th:eq(4)').text('Gasto Acumulado');
            //            $('#tblControlEjecucion_wrapper').find('thead tr th:eq(5)').text('Ingreso Acumulado');
            //            $('#tblControlEjecucion_wrapper').find('thead tr th:eq(6)').text('Cumplimiento');
            //        }
            //    });
            //});

            btnCerrarModal.on("click", function () {
                if (btnNivel4.is(":visible")) {
                    btnNivel3.trigger("click");
                    btnCerrarModal.html(`<i class="fa fa-arrow-left"></i>&nbsp;Regresar`);
                } else if (btnNivel3.is(":visible")) {
                    txtBtnNivel2.trigger("click");
                    btnCerrarModal.html(`<i class="fa fa-arrow-left"></i>&nbsp;Cerrar`);
                } else if (!btnNivel3.is(":visible")) {
                    mdlDetalleNivel.modal("hide");
                }
            });

            //#region EVENTOS ENVIO INFORME
            btnFiltroEnvioInforme.on("click", function () {
                //cboFiltroInformeAnio[0].selectedIndex = 0;
                //cboFiltroInformeAnio.trigger("change");
                //cboFiltroInformeMes[0].selectedIndex = 0;
                //cboFiltroInformeMes.trigger("change");
                //cboFiltroInformeEmpresa[0].selectedIndex = 0;
                //cboFiltroInformeEmpresa.trigger("change");
                btnFiltroInformeBuscar.click();
                var anioStr = $("#cboFiltroAnio option:selected").text();
                var mesStr = $("#cboFiltroMes option:selected").text();
                $("#titleEnvioInformes").text("INFORMES " + mesStr + " " + anioStr);
                mdlEnvioInforme.modal("show");
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            });

            btnFiltroInformeBuscar.on("click", function () {
                fncGetEnvioInforme();
            });


            //#endregion

            //#region EVENTOS CONCEPTOS AGRUPACIÓN CC
            btnConceptosAgrupacionCC.on("click", function () {
                // fncGetPresupuestoVsGastoConceptos();
                // if (inpConsultaNivel.attr('data-cumplimientoIngreso') == 2) { //TODO
                //     panelGeneral0.css('display', 'none');
                //     panelDetalleNivel2.css('display', 'none');
                //     panelGeneral3.css('display', 'block');
                //     tblNivel3DeIngresosGastos.css('display', 'block');
                //     btnNivel3.css('display', 'initial');
                //     // btnNivel3.attr('data-idCC', rowData.idCC);
                //     // txtBtnNivel3.text(`CC: ${rowData.cc}`);
                //     // GetDetalleNivelTresIngresoGasto(rowData.idCC);
                // } else {
                //     panelGeneral0.css('display', 'none');
                //     panelDetalleNivel2.css('display', 'none');
                //     panelDetalleNivel3.css('display', 'block');
                //     btnNivel3.css('display', 'initial');
                //     // btnNivel3.attr('data-idCC', rowData.idCC);
                //     // txtBtnNivel3.text(`CC: ${rowData.cc}`);
                //     // fncGetPresupuestoVsGastoConceptos(rowData.idCC);
                // }
                // btnCerrarModal.html(`<i class="fa fa-arrow-left"></i>&nbsp;Regresar`);
                fncGetDetalleAgrupadorConceptoCC();
                mdlAgrupacionConceptosCC.modal("show");
            });

            btnCerrarMdlAgrupacionCC.on("click", function () {
                if ($(this).text() == "Cerrar") {
                    mdlAgrupacionConceptosCC.modal("hide");
                } else {
                    // panelAgrupacionCC.css("display", "inline");
                    // panelConceptosPorCC.css("display", "none");
                    // $(this).attr("text", "Cerrar");
                    mdlAgrupacionConceptosCC.modal("hide");
                }
            })
            $("#btnAddComentario").click(function (e) {
                insertCommentary(e);
            });
            //#endregion

            obtenerUrlPArams();

            $("#cerrarModalSeguimientoFirmas").click(function (e) {
                $("#modalSeguimientoFirmas").modal("hide");
            });

            $("#cerrarModalRechazo").click(function (e) {
                $("#modalRechazo").modal("hide");
            });

            $("#cerrarModalJustificacion").click(function (e) {
                $("#modalJustificacion").modal("hide");
            });
        }

        //porcentajeCumplimientoIngresoGastoMensual.on('click', function () {
        //    inpConsultaNivel.attr('data-cumplimientoIngreso', 2);
        //    getDetalleNivel2IngresoGasto().done(function (response) {
        //        if (response && response.success) {
        //            txtCumplimientoControlEjecucion.text(`${Math.trunc(response.items.cumplimientoAcumuladoEmpresa)}%`);
        //            if (response.items.cumplimientoAcumuladoEmpresa >= 97) {
        //                txtCumplimientoControlEjecucion.css('color', 'red');
        //            } else if (response.items.cumplimientoAcumuladoEmpresa >= 95) {
        //                txtCumplimientoControlEjecucion.css('color', 'yellow');
        //            } else {
        //                txtCumplimientoControlEjecucion.css('color', 'green');
        //            }
        //            txtMesCierreControlEjecucion.text(response.items.mesCierre);
        //            initGraficaControlEjecucion(response.items.graficaBarraMensual);
        //            periodoResumen.text('DETALLE INGRESOS / GASTOS')
        //            // panelGraficas.fadeToggle('fast', 'linear', function() {
        //            //     panelDetalleNivel2.fadeToggle('fast', 'linear', function() {
        //            //         addRows(tblControlEjecucion, response.items.datosTablaCC);
        //            //     });
        //            // });
        //            SetNombreModal();
        //            mdlDetalleNivel.modal("show");
        //            btnNivel3.css('display', 'none');
        //            btnNivel4.css('display', 'none');
        //            // btnPlanAccion.css("display", "none");
        //            panelDetalleNivel2.css('display', 'block');
        //            panelDetalleNivel3.css('display', 'none');
        //            panelDetalleNivel4.css('display', 'none');
        //            panelDetalleNivel4Ctas.css('display', 'none');
        //            $('.panel-detalle-total').css('display', 'none');
        //            //btnNivel2.css('display','block');

        //            addRows(tblControlEjecucion, response.items.datosTablaCC);

        //            tblControlEjecucion.DataTable().columns(1).visible(false);


        //            $('#tblControlEjecucion_wrapper').find('thead tr th:eq(1)').text('Gasto Mensual');
        //            $('#tblControlEjecucion_wrapper').find('thead tr th:eq(2)').text('Ingreso Mensual');
        //            $('#tblControlEjecucion_wrapper').find('thead tr th:eq(3)').text('Cumplimiento');
        //            $('#tblControlEjecucion_wrapper').find('thead tr th:eq(4)').text('Gasto Acumulado');
        //            $('#tblControlEjecucion_wrapper').find('thead tr th:eq(5)').text('Ingreso Acumulado');
        //            $('#tblControlEjecucion_wrapper').find('thead tr th:eq(6)').text('Cumplimiento');
        //        }
        //    });            
        //})

        //#region INDEX
        function fncGetDetalleAgrupadorConceptoCC() {
            //#region SE OBTIENE LISTADO DE CP Y ARR
            let arrConstruplan = [];
            let arrArrendadora = [];
            cboFiltroCC.val().forEach(element => {
                let objCC = cboFiltroCC.find(`option[value="${element}"]`);
                let prefijoCC = objCC.attr("data-prefijo");

                if (prefijoCC == 1) {
                    if (element == "cp_46") {
                        let idCC = element.replace("cp_46", "46");
                        arrConstruplan.push(idCC);
                    } else {
                        arrConstruplan.push(element);
                    }
                } else if (prefijoCC == 2) {
                    if (element == "arr_46") {
                        let idCC = element.replace("arr_46", "46");
                        arrArrendadora.push(idCC);
                    } else {
                        arrArrendadora.push(element);
                    }
                }
            });
            //#endregion

            let obj = new Object();
            obj.arrConstruplan = arrConstruplan;
            obj.arrArrendadora = arrArrendadora;
            obj.year = cboFiltroAnio.val();
            obj.idMes = cboFiltroMes.val();
            axios.post("GetDetalleAgrupadorConceptoCC", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtAgrupacionConceptoCC.clear();
                    dtAgrupacionConceptoCC.rows.add(response.data.lstPptosVsGastosDTO);
                    dtAgrupacionConceptoCC.draw();
                    panelAgrupacionCC.css("display", "inline");
                    panelConceptosPorCC.css("display", "none");
                    btnCerrarMdlAgrupacionCC.text("Cerrar");
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncFillCboEmpresas() {
            cboFiltroEmpresas.fillCombo("FillCboEmpresas", {}, false);
            cboFiltroEmpresas.select2({ width: "100%" });

            cboFiltroInformeEmpresa.fillCombo("FillCboEmpresas", {}, false);
            cboFiltroInformeEmpresa.select2({ width: "100%" });
        }

        function fncGetPermisoVisualizarEnvioInformes() {
            axios.post("GetPermisoVisualizarEnvioInformes").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //if (response.data.tieneAcceso) {
                    btnFiltroEnvioInforme.css("display", "inline");
                    //} else {
                    //    btnFiltroEnvioInforme.css("display", "none");
                    //}
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblCapturas() {
            dtCapturas = tblCapturas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'concepto', title: 'Concepto', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'enero', title: 'Enero', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                            if (cellData.importeEnero == 8100) {
                            }
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeEnero" mes="1">${maskNumero_NoDecimal(data)}</span>`;
                            } else {
                                if (parseFloat(data) > 100) { // >= 97
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeEnero" mes="1">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) { // >= 95
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeEnero" mes="1">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) { // < 95
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeEnero" mes="1">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    },
                    {
                        data: 'febrero', title: 'Febrero', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeFebrero" mes="2">${maskNumero_NoDecimal(data)}</span>`
                            } else {
                                if (parseFloat(data) > 100) {
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeFebrero" mes="2">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeFebrero" mes="2">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) {
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeFebrero" mes="2">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    },
                    {
                        data: 'marzo', title: 'Marzo', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeMarzo" mes="3">${maskNumero_NoDecimal(data)}</span>`
                            } else {
                                if (parseFloat(data) > 100) {
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeMarzo" mes="3">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeMarzo" mes="3">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) {
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeMarzo" mes="3">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    },
                    {
                        data: 'abril', title: 'Abril', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeAbril" mes="4">${maskNumero_NoDecimal(data)}</span>`
                            } else {
                                if (parseFloat(data) > 100) {
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeAbril" mes="4">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeAbril" mes="4">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) {
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeAbril" mes="4">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    },
                    {
                        data: 'mayo', title: 'Mayo', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeMayo" mes="5">${maskNumero_NoDecimal(data)}</span>`
                            } else {
                                if (parseFloat(data) > 100) {
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeMayo" mes="5">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeMayo" mes="5">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) {
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeMayo" mes="5">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    },
                    {
                        data: 'junio', title: 'Junio', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeJunio" mes="6">${maskNumero_NoDecimal(data)}</span>`
                            } else {
                                if (parseFloat(data) > 100) {
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeJunio" mes="6">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeJunio" mes="6">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) {
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeJunio" mes="6">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    },
                    {
                        data: 'julio', title: 'Julio', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeJulio" mes="7">${maskNumero_NoDecimal(data)}</span>`
                            } else {
                                if (parseFloat(data) > 100) {
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeJulio" mes="7">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeJulio" mes="7">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) {
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeJulio" mes="7">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    },
                    {
                        data: 'agosto', title: 'Agosto', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeAgosto" mes="8">${maskNumero_NoDecimal(data)}</span>`
                            } else {
                                if (parseFloat(data) > 100) {
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeAgosto" mes="8">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeAgosto" mes="8">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) {
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeAgosto" mes="8">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    },
                    {
                        data: 'septiembre', title: 'Septiembre', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeSeptiembre" mes="9">${maskNumero_NoDecimal(data)}</span>`
                            } else {
                                if (parseFloat(data) > 100) {
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeSeptiembre" mes="9">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeSeptiembre" mes="9">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) {
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeSeptiembre" mes="9">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    },
                    {
                        data: 'octubre', title: 'Octubre', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeOctubre" mes="10">${maskNumero_NoDecimal(data)}</span>`
                            } else {
                                if (parseFloat(data) > 100) {
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeOctubre" mes="10">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeOctubre" mes="10">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) {
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeOctubre" mes="10">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    },
                    {
                        data: 'noviembre', title: 'Noviembre', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeNoviembre" mes="11">${maskNumero_NoDecimal(data)}</span>`
                            } else {
                                if (parseFloat(data) > 100) {
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeNoviembre" mes="11">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeNoviembre" mes="11">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) {
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeNoviembre" mes="11">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    },
                    {
                        data: 'diciembre', title: 'Diciembre', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeDiciembre" mes="12">${maskNumero_NoDecimal(data)}</span>`
                            } else {
                                if (parseFloat(data) > 100) {
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeDiciembre" mes="12">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeDiciembre" mes="12">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) {
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeDiciembre" mes="12">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    },
                    {
                        data: 'total', title: 'Total', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto != "CUMPLIMIENTO") {
                                return `<span class="importePresupuesto importeDiciembre" mes="0">${maskNumero_NoDecimal(data)}</span>`
                            } else {
                                if (parseFloat(data) > 100) {
                                    return `<i class="fa fa-arrow-up redArrow" style="color: red;"></i> <span class="importePresupuesto importeDiciembre" mes="12">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                    return `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeDiciembre" mes="12">${Math.trunc(data)}%</span>`
                                } else if (parseFloat(data) < 97) {
                                    return `<i class="fa fa-arrow-down greenArrow" style="color: green;"></i> <span class="importePresupuesto importeDiciembre" mes="12">${Math.trunc(data)}%</span>`
                                }
                            }
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    // tblCapturas.on('click', '.detalle', function () {
                    //     let rowData = dtCapturas.row($(this).closest('tr')).data();
                    //     fncGetDetCapturas(rowData.idAgrupacion, rowData.idConcepto);
                    // });

                    // tblCapturas.on('click', '.importePresupuesto', function () {
                    //     let rowData = dtCapturas.row($(this).closest('tr')).data();
                    //     let mes = $(this).attr('mes');
                    //     if (rowData.concepto != "TOTAL" && rowData.concepto != "CUMPLIMIENTO") {
                    //         panelDetalleNivel3.css('display', 'none');
                    //         panelDetalleNivel4.css('display', 'block');
                    //         panelDetalleNivel4Ctas.css('display', 'block');
                    //         btnNivel4.css('display', 'initial');
                    //         txtBtnNivel4.text(`Concepto: ${rowData.concepto} - Mes: ${mes}`);
                    //         let gasto = 0;
                    //         if (rowData.concepto == "GASTOS") {
                    //             gasto = 2;
                    //         } else {
                    //             gasto = 1
                    //         }
                    //         GetDetalleNivelCuatroPresupuestoGasto(gasto, mes);
                    //     }
                    // });

                    // tblCapturas.on('click', '.importeContable', function () {
                    //     let rowData = dtCapturas.row($(this).closest('tr')).data();
                    //     let mes = $(this).attr('mes');

                    //     if (rowData.concepto != "TOTAL") {
                    //         fncGetDetCapturasMesContable(rowData.idAgrupacion, rowData.idConcepto, mes);
                    //     }
                    // });

                    // tblCapturas.on('click', '.importeTotalConcepto', function () {
                    //     let rowData = dtCapturas.row($(this).closest('tr')).data();
                    //     if (rowData.concepto != "TOTAL") {
                    //         fncGetDetCapturas(rowData.idAgrupacion, rowData.idConcepto);
                    //     }
                    // });

                    // tblCapturas.on('click', '.importeContTotalConcepto', function () {
                    //     let rowData = dtCapturas.row($(this).closest('tr')).data();
                    //     if (rowData.concepto != "TOTAL") {
                    //         fncGetDetCapturasContable(rowData.idAgrupacion, rowData.idConcepto);
                    //     }
                    // });
                },
                columnDefs: [
                    { targets: [0], className: 'dt-body-center' },
                    { targets: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13], className: 'dt-body-right' }
                ],
                drawCallback: function (settings) {
                    //let total = 0;
                    //tblCapturas.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                    //    if (colIdx > 0) {
                    //        for (let x = 0; x < this.data().length; x++) {
                    //            let ppto = this.data(0)[0];
                    //            let gasto = this.data(1)[1];
                    //            total = parseFloat(ppto) + parseFloat(gasto);
                    //        }
                    //        $(this.footer()).html(maskNumero_NoDecimal(total));
                    //    }
                    //    total = 0;
                    //});
                }
            });
        }

        function GetDetalleNivelCuatroPresupuestoGasto(gasto, mes) {
            let objDTO = new Object();
            if (btnNivel3.attr('data-idcc') > -1 && cboFiltroAnio.val() > 0) {
                objDTO = {
                    idCC: btnNivel3.attr('data-idcc'),
                    anio: cboFiltroAnio.val()
                }
            }
            axios.post('GetDetalleNivelCuatroPresupuestoGasto', { objDTO: objDTO, gasto: gasto, mes: mes }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtNivel4.clear();
                    dtNivel4.rows.add(response.data.lstSumaCapturas);
                    dtNivel4.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblNivel4() {
            dtNivel4 = tblNivel4.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'concepto', title: 'CONCEPTO' },
                    {
                        data: 'mes', title: 'MONTO', render: function (data, type, row) {
                            let html = '';
                            if (row.concepto == "Concepto") {
                                html = data;
                            } else {
                                html = maskNumero(data);
                            }
                            return html;
                        }
                    },
                    //render: function (data, type, row) { }
                ],
                initComplete: function (settings, json) {
                    tblNivel4.on('click', '.classBtn', function () {
                        let rowData = dtNivel4.row($(this).closest('tr')).data();
                    });
                    tblNivel4.on('click', '.classBtn', function () {
                        let rowData = dtNivel4.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTblNivel4Ctas() {
            dtNivel4Ctas = tblNivel4Ctas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'poliza',
                        title: 'POLIZA',
                        render: function (data, type, row) {
                            return `<button class="btn btn-success btnVisor"><i class="far fa-eye"></i> ${data}</button>`
                        }
                    },
                    { data: 'linea', title: 'LINEA' },
                    { data: 'cta', title: 'CTA' },
                    { data: 'scta', title: 'SCTA' },
                    { data: 'sscta', title: 'SSCTA' },
                    { data: 'concepto', title: 'CC' },
                    {
                        data: 'gastoMensual', title: 'GASTO MENSUAL',
                        // createdCell: function (td, tr, cellData, rowData, row, col) {
                        //     $(td).css('background-color', '#dbdcd9');
                        // },
                        render: function (data) {
                            return maskNumero_NoDecimal(data);
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblNivel4Ctas.on('click', '.btnVisor', function () {
                        let dataRow = dtNivel4Ctas.row($(this).closest('tr')).data();

                        reportePoliza(false, dataRow.poliza, false);
                        // menuConfig.parametros = {
                        //     esCC: true,
                        //     estatus: 'A',
                        //     poliza: dataRow.poliza
                        // }
                        // mostrarMenu();
                    });
                },
                columnDefs: [
                    // { targets: [0], className: 'dt-body-center' },
                    // { width: "5%", targets: [1, 2, 3] },
                    { targets: [6], className: 'dt-body-right' }
                ],
                drawCallback: function (settings) {
                    let total = 0;
                    tblNivel4Ctas.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                        if (colIdx > 5) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }
                            $(this.footer()).html(maskNumero(total));
                        }
                        total = 0;
                    });
                }
            });
        }

        function reportePoliza(esResumen, poliza, esVisor) {
            if (!esVisor) { $.blockUI({ message: 'Procesando...' }); }
            var path = '/Reportes/Vista.aspx?' +
                'esDescargaVisor=' + esVisor +
                '&esVisor=' + true +
                '&idReporte=' + 64 + // 188 pruebas, 64 producción
                '&isResumen=' + esResumen +
                '&poliza=' + poliza +
                '&ppto=' + true;
            report.attr('src', path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                $('#myModal').modal('show');
            };
        }

        function initTblAgrupacionConceptosCC() {
            dtAgrupacionConceptoCC = tblAgrupacionConceptosCC.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'descripcion', title: 'Descripción', className: 'dt-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.esAgrupador) {
                                return data;
                            } else {
                                return `<a title="${row.idConceptoString}" class="getConceptosRelCC">${data}</a>`;
                            }
                        }
                    },
                    {
                        data: 'pptoMensual', title: 'Presupuesto Mensual', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'gastoMensual', title: 'Gasto Mensual', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'diferenciaMensual', title: 'Diferencia', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                            let colorIndicador = cellData.diferenciaMensual < 0 ? 'red' : 'green'
                            $(td).css('color', colorIndicador);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'cumplimientoMensual', title: 'Cumplimiento', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            let flechaColor = '';
                            let flechaDireccion = '';

                            if (parseFloat(data) < 97) {
                                flechaColor = 'greenArrow';
                                flechaDireccion = 'down';
                            }

                            if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                flechaColor = 'yellowArrow';
                                flechaDireccion = 'right';
                            }

                            if (parseFloat(data) > 100) {
                                flechaColor = 'redArrow';
                                flechaDireccion = 'up';
                            }

                            return `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                        }
                    },
                    {
                        data: 'pptoAcumulado', title: 'Presupuesto Acumulado', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'gastoAcumulado', title: 'Gasto Acumulado', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'diferenciaAcumulado', title: 'Diferencia', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                            let colorIndicador = cellData.diferenciaAcumulado < 0 ? 'red' : 'green'
                            $(td).css('color', colorIndicador);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'cumplimientoAcumulado', title: 'Cumplimiento', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            let flechaColor = '';
                            let flechaDireccion = '';

                            if (parseFloat(data) < 97) {
                                flechaColor = 'greenArrow';
                                flechaDireccion = 'down';
                            }

                            if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                flechaColor = 'yellowArrow';
                                flechaDireccion = 'right';
                            }

                            if (parseFloat(data) > 100) {
                                flechaColor = 'redArrow';
                                flechaDireccion = 'up';
                            }

                            return `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                        }
                    },//omaromar
                    {
                        data: 'gastoAnioPasado', title: 'Gasto Año Anterior', className: 'dt-body-right dt-head-center', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: "diferenciaAnioActualVsAnioAcumulado", title: 'Diferencia', className: 'dt-body-right dt-head-center', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                            let colorIndicador = cellData.diferenciaAnioActualVsAnioAcumulado < 0 ? 'green' : 'red'
                            $(td).css('color', colorIndicador);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    } //omaromar
                ],
                initComplete: function (settings, json) {
                    tblAgrupacionConceptosCC.on('click', '.getConceptosRelCC', function () {
                        let rowData = dtAgrupacionConceptoCC.row($(this).closest('tr')).data();
                        panelAgrupacionCC.css("display", "none");
                        panelConceptosPorCC.css("display", "inline");
                        btnCerrarMdlAgrupacionCC.text("Regresar");
                        fncGetConceptosRelCC(rowData.conceptoCuentaID);
                    });
                },
                columnDefs: [
                ],
                drawCallback: function (settings) {
                    let total = 0;
                    gastoTotal = 0;
                    pptoTotal = 0;
                    gastoAcumuladoTotal = 0;
                    pptoAcumuladoTotal = 0;
                    tblControlEjecucion.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                        for (let i = 0; i < this.data().length; i++) {
                            switch (colIdx) {
                                case 3:
                                    total += this.data(0)[i];
                                    gastoTotal += this.data(0)[i];
                                    break;
                                case 4:
                                    total += this.data(0)[i];
                                    pptoTotal += this.data(0)[i];
                                    break;
                                case 5:
                                    total += this.data(0)[i];
                                    break;
                                case 6:
                                    total += this.data(0)[i];
                                    break;
                                case 7:
                                    total += this.data(0)[i];
                                    gastoAcumuladoTotal += this.data(0)[i];
                                    break;
                                case 8:
                                    total += this.data(0)[i];
                                    pptoAcumuladoTotal += this.data(0)[i];
                                    break;
                                case 9:
                                    total += this.data(0)[i];
                                    break;
                                case 10:
                                    total += this.data(0)[i];
                                    break;
                                case 11:
                                    total += this.data(0)[i];
                                    break;
                                case 12:
                                    total += this.data(0)[i];
                                    break;
                                default:
                                    total = "";
                                    break;
                            }
                        }
                        if (total != 0) {
                            if (colIdx == 6 || colIdx == 10) {
                                let totalCumplimiento = 0;
                                if (colIdx == 6) totalCumplimiento = (pptoTotal * 100) / (gastoTotal != 0 ? gastoTotal : 1);
                                if (colIdx == 10) totalCumplimiento = (pptoAcumuladoTotal * 100) / (gastoAcumuladoTotal != 0 ? gastoAcumuladoTotal : 1);

                                let flechaColor = '';
                                let flechaDireccion = '';

                                if (parseFloat(totalCumplimiento) < 97) {
                                    flechaColor = 'greenArrow';
                                    flechaDireccion = 'down';
                                }

                                if (parseFloat(totalCumplimiento) >= 97 && parseFloat(totalCumplimiento) <= 100) {
                                    flechaColor = 'yellowArrow';
                                    flechaDireccion = 'right';
                                }

                                if (parseFloat(totalCumplimiento) > 100) {
                                    flechaColor = 'redArrow';
                                    flechaDireccion = 'up';
                                };

                                $(this.footer()).html(`<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(totalCumplimiento)}%</span>`);
                            } else {
                                $(this.footer()).html(maskNumero_NoDecimal(total));
                            }
                        } else {
                            $(this.footer()).html(total);
                        }
                        total = 0;
                    });
                }
            });
        }

        function initTblConceptosRelCC() {
            dtConceptosRelCC = tblConceptosRelCC.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'cc', title: 'CC', className: 'dt-center' },
                    { data: 'concepto', title: 'Concepto', className: 'dt-center getDetalle' },
                    {
                        data: 'pptoMensual', title: 'Ppto Mensual', className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'gastoMensual', title: 'Gasto mensual', className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'diferenciaMensual', title: 'Diferencia', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let colorIndicador = cellData.diferenciaMensual < 0 ? 'red' : 'green'
                            $(td).css('color', colorIndicador);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'cumplimientoMensual', title: 'Cumplimiento', className: 'dt-body-right dt-head-center', render: function (data, type, row) {
                            let flechaColor = '';
                            let flechaDireccion = '';

                            if (parseFloat(data) < 97) {
                                flechaColor = 'greenArrow';
                                flechaDireccion = 'down';
                            }

                            if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                flechaColor = 'yellowArrow';
                                flechaDireccion = 'right';
                            }

                            if (parseFloat(data) > 100) {
                                flechaColor = 'redArrow';
                                flechaDireccion = 'up';
                            }

                            return `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                        }
                    },
                    {
                        data: 'pptoAcumulado', title: 'Ppto acumulado', className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'gastoAcumulado', title: 'Gasto acumulado', className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'diferenciaAcumulado', title: 'Diferencia', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let colorIndicador = cellData.diferenciaAcumulado < 0 ? 'red' : 'green'
                            $(td).css('color', colorIndicador);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'cumplimientoAcumulado', title: 'Cumplimiento', className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            let flechaColor = '';
                            let flechaDireccion = '';

                            if (parseFloat(data) < 97) {
                                flechaColor = 'greenArrow';
                                flechaDireccion = 'down';
                            }

                            if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                flechaColor = 'yellowArrow';
                                flechaDireccion = 'right';
                            }

                            if (parseFloat(data) > 100) {
                                flechaColor = 'redArrow';
                                flechaDireccion = 'up';
                            }

                            return `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                        }
                    } //DEV
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                ],
                headerCallback: function (thead, data, start, end, display) {
                },
                drawCallback: function (settings) {
                },
            });
        }

        function fncGetConceptosRelCC(idCapturas) {
            //#region SE OBTIENE LISTADO DE CP Y ARR
            let arrConstruplan = [];
            let arrArrendadora = [];
            cboFiltroCC.val().forEach(element => {
                let objCC = cboFiltroCC.find(`option[value="${element}"]`);
                let prefijoCC = objCC.attr("data-prefijo");

                if (prefijoCC == 1) {
                    if (element == "cp_46") {
                        let idCC = element.replace("cp_46", "46");
                        arrConstruplan.push(idCC);
                    } else {
                        arrConstruplan.push(element);
                    }
                } else if (prefijoCC == 2) {
                    if (element == "arr_46") {
                        let idCC = element.replace("arr_46", "46");
                        arrArrendadora.push(idCC);
                    } else {
                        arrArrendadora.push(element);
                    }
                }
            });
            //#endregion

            //#region SE OBTIENE ARREGLO DE CAPTURA_ID
            let arrCapturasID = new Array();
            //arrCapturasID = idCapturas.split(",");
            arrCapturasID = [idCapturas]
            //#endregion

            if (arrCapturasID.length > 0) {
                let obj = new Object();
                obj.arrConstruplan = arrConstruplan;
                obj.arrArrendadora = arrArrendadora;
                obj.arrCapturasID = arrCapturasID;
                obj.idMes = cboFiltroMes.val();
                obj.anio = cboFiltroAnio.val();
                obj.costosAdministrativos = btnCostosTotales.is(":checked");

                axios.post("GetConceptosRelCC", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtConceptosRelCC.clear();
                        dtConceptosRelCC.rows.add(response.data.lstConceptosDTO);
                        dtConceptosRelCC.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function initTblControlEjecucion() {
            dtControlEjecucion = tblControlEjecucion.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'idCC', title: 'CC', className: 'dt-center', visible: false },
                    {
                        data: 'cc', title: 'CC', className: 'dt-center',
                        render: function (data, type, row) {
                            return `<a class="linkCC" >${data}</a>`
                        }
                    },
                    { data: 'descripcion', title: 'Descripción', className: 'dt-center getDetalle' },
                    {
                        data: 'gastoMensual', title: 'Gasto Mensual', className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'ingresoMensual', title: 'Ingreso Mensual', className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'diferenciaMensual', title: 'Diferencia', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let colorIndicador = cellData.diferenciaMensual < 0 ? 'red' : 'green'
                            $(td).css('color', colorIndicador);
                        },
                        render: function (data, type, row) {
                            // return fncGetDiferenciasDetallePptoVsGastos(row.gastoMensual, row.ingresoMensual);
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'cumplimientoMensual', title: 'Cumplimiento', className: 'dt-body-right dt-head-center', render: function (data, type, row) {
                            let flechaColor = '';
                            let flechaDireccion = '';

                            if (parseFloat(data) < 97) {
                                flechaColor = 'greenArrow';
                                flechaDireccion = 'down';
                            }

                            if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                flechaColor = 'yellowArrow';
                                flechaDireccion = 'right';
                            }

                            if (parseFloat(data) > 100) {
                                flechaColor = 'redArrow';
                                flechaDireccion = 'up';
                            }

                            return `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                        }
                    },
                    {
                        data: 'gastoAcumulado', title: 'Gasto Acumulado', className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'ingresoAcumulado', title: 'Ingreso Acumulado', className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'diferenciaAcumulado', title: 'Diferencia', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let colorIndicador = cellData.diferenciaAcumulado < 0 ? 'red' : 'green'
                            $(td).css('color', colorIndicador);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'cumplimientoAcumulado', title: 'Cumplimiento', className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            let flechaColor = '';
                            let flechaDireccion = '';

                            if (parseFloat(data) < 97) {
                                flechaColor = 'greenArrow';
                                flechaDireccion = 'down';
                            }

                            if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                flechaColor = 'yellowArrow';
                                flechaDireccion = 'right';
                            }

                            if (parseFloat(data) > 100) {
                                flechaColor = 'redArrow';
                                flechaDireccion = 'up';
                            }

                            return `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                        }
                    },
                    {
                        data: 'gastoAnioPasado', title: 'Gasto Año Anterior', className: 'dt-body-right dt-head-center', visible: true,
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: "diferenciaAnioActualVsAnioAcumulado", title: 'Diferencia', className: 'dt-body-right dt-head-center', visible: true,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let colorIndicador = cellData.diferenciaAnioActualVsAnioAcumulado < 0 ? 'green' : 'red'
                            $(td).css('color', colorIndicador);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    } //omaromar
                ],
                initComplete: function (settings, json) {
                    tblControlEjecucion.on("click", ".linkCC", function () {
                        let rowData = dtControlEjecucion.row($(this).closest('tr')).data();
                        //INIT COMPLETE
                        if (inpConsultaNivel.attr('data-cumplimientoIngreso') == 2) { //TODO
                            panelGeneral0.css('display', 'none');
                            panelDetalleNivel2.css('display', 'none');
                            panelGeneral3.css('display', 'block');
                            tblNivel3DeIngresosGastos.css('display', 'block');
                            btnNivel3.css('display', 'initial');
                            btnNivel3.attr('data-idCC', rowData.idCC);
                            txtBtnNivel3.text(`CC: ${rowData.cc}`);
                            GetDetalleNivelTresIngresoGasto(rowData.idCC);
                        } else {
                            panelGeneral0.css('display', 'none');
                            panelDetalleNivel2.css('display', 'none');
                            panelDetalleNivel3.css('display', 'block');
                            btnNivel3.css('display', 'initial');
                            btnNivel3.attr('data-idCC', rowData.idCC);
                            txtBtnNivel3.text(`CC: ${rowData.cc}`);
                            fncGetPresupuestoVsGastoConceptos(rowData.idCC, rowData.empresa);
                        }
                        btnCerrarModal.html(`<i class="fa fa-arrow-left"></i>&nbsp;Regresar`);
                    });

                    // tblControlEjecucion.on('click', '.getDetalle', function () {
                    //     let rowData = dtControlEjecucion.row($(this).closest('tr')).data();
                    //     fncGetDetallePorAgrupacion($(this).closest("tr"));
                    // });

                    tblControlEjecucion.on('click', '.botonDetalle', function () {
                        let rowData = dtControlEjecucion.row($(this).closest('tr')).data();
                        fncGetDetallePorAgrupacion(rowData);
                    });
                },
                columnDefs: [
                ],
                headerCallback: function (thead, data, start, end, display) {

                    // $(tblControlEjecucion).children('thead').html(`
                    // <tr>
                    //     <th colspan="2"></th>

                    //     <th colspan="3">Mensual</th>
                    //     <th colspan="3">Acomulado</th>
                    // </tr>
                    // ${$(thead).html()}
                    // `);
                    // console.log($(thead).html());
                },
                drawCallback: function (settings) {
                    let total = 0;
                    gastoTotal = 0;
                    pptoTotal = 0;
                    gastoAcumuladoTotal = 0;
                    pptoAcumuladoTotal = 0;
                    tblControlEjecucion.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {

                        for (let i = 0; i < this.data().length; i++) {
                            switch (colIdx) {
                                case 3:
                                    total += this.data(0)[i];
                                    gastoTotal += this.data(0)[i];
                                    break;
                                case 4:
                                    total += this.data(0)[i];
                                    pptoTotal += this.data(0)[i];
                                    break;
                                case 5:
                                    total += this.data(0)[i];
                                    break;
                                case 6:
                                    total += this.data(0)[i];
                                    break;
                                case 7:
                                    total += this.data(0)[i];
                                    gastoAcumuladoTotal += this.data(0)[i];
                                    break;
                                case 8:
                                    total += this.data(0)[i];
                                    pptoAcumuladoTotal += this.data(0)[i];
                                    break;
                                case 9:
                                    total += this.data(0)[i];
                                    break;
                                case 10:
                                    total += this.data(0)[i];
                                    break;
                                case 11:
                                    total += this.data(0)[i];
                                    break;
                                case 12:
                                    total += this.data(0)[i];
                                    break;
                                default:
                                    total = "";
                                    break;
                            }
                        }
                        if (total != 0) {
                            if (colIdx == 6 || colIdx == 10) {

                                let totalCumplimiento = 0;
                                if (colIdx == 6) totalCumplimiento = (pptoTotal * 100) / (gastoTotal != 0 ? gastoTotal : 1);
                                if (colIdx == 10) totalCumplimiento = (pptoAcumuladoTotal * 100) / (gastoAcumuladoTotal != 0 ? gastoAcumuladoTotal : 1);

                                let flechaColor = '';
                                let flechaDireccion = '';

                                if (parseFloat(totalCumplimiento) < 97) {
                                    flechaColor = 'greenArrow';
                                    flechaDireccion = 'down';
                                }

                                if (parseFloat(totalCumplimiento) >= 97 && parseFloat(totalCumplimiento) <= 100) {
                                    flechaColor = 'yellowArrow';
                                    flechaDireccion = 'right';
                                }

                                if (parseFloat(totalCumplimiento) > 100) {
                                    flechaColor = 'redArrow';
                                    flechaDireccion = 'up';
                                };

                                $(this.footer()).html(`<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(totalCumplimiento)}%</span>`);
                            } else {
                                $(this.footer()).html(maskNumero_NoDecimal(total));
                            }
                        } else {
                            $(this.footer()).html(total);
                        }
                        total = 0;
                    });
                }
            });
        }

        function fncGetDiferenciasDetallePptoVsGastos(ppto, gasto) {
            let pptoMensual = ppto;
            let gastoMensual = gasto;
            let diferencia = pptoMensual - gastoMensual;
            return maskNumero_NoDecimal(diferencia);
        }

        function initTablaAgrupaciones() {
            dtAgrupaciones = tablaAgrupaciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'descripcion', title: 'Descripción' },
                    {
                        data: 'gastoMensual', title: 'Gasto Mensual', className: 'dt-body-right dt-head-center', render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'presupuestoMensual', title: 'Presupuesto Mensual', className: 'dt-body-right dt-head-center', render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'cumplimientoMensual', title: 'Cumplimiento', className: 'dt-body-right dt-head-center', render: function (data, type, row) {
                            let flechaColor = '';
                            let flechaDireccion = '';

                            if (row.esIngresoGasto) {
                                if (data > 0) {
                                    flechaColor = 'greenArrow';
                                    flechaDireccion = 'up';
                                }

                                if (data <= 0) {
                                    flechaColor = 'redArrow';
                                    flechaDireccion = 'down';
                                }
                            } else {
                                if (data >= 0 && data <= 94) {
                                    flechaColor = 'greenArrow';
                                    flechaDireccion = 'up';
                                }

                                if (data >= 95 && data <= 99) {
                                    flechaColor = 'yellowArrow';
                                    flechaDireccion = 'right';
                                }

                                if (data >= 100) {
                                    flechaColor = 'redArrow';
                                    flechaDireccion = 'down';
                                }
                            }

                            return `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${maskNumeroPorcentaje(data)}</span>`;
                        }
                    },
                    {
                        data: 'gastoAcumulado', title: 'Gasto Acumulado', className: 'dt-body-right dt-head-center', render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'presupuestoAcumulado', title: 'Presupuesto Acumulado', className: 'dt-body-right dt-head-center', render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'cumplimientoAcumulado', title: 'Cumplimiento', className: 'dt-body-right dt-head-center', render: function (data, type, row) {
                            let flechaColor = '';
                            let flechaDireccion = '';

                            if (row.esIngresoGasto) {
                                if (data > 0) {
                                    flechaColor = 'greenArrow';
                                    flechaDireccion = 'up';
                                }

                                if (data <= 0) {
                                    flechaColor = 'redArrow';
                                    flechaDireccion = 'down';
                                }
                            } else {
                                if (data >= 0 && data <= 94) {
                                    flechaColor = 'greenArrow';
                                    flechaDireccion = 'up';
                                }

                                if (data >= 95 && data <= 99) {
                                    flechaColor = 'yellowArrow';
                                    flechaDireccion = 'right';
                                }

                                if (data >= 100) {
                                    flechaColor = 'redArrow';
                                    flechaDireccion = 'down';
                                }
                            }

                            return `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${maskNumeroPorcentaje(data)}</span>`;
                        }
                    },
                    {
                        title: '', className: 'dt-body-center dt-head-center', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-primary botonDetalle"><i class="fa fa-align-justify"></i></button>`;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaAgrupaciones.on('click', '.botonDetalle', function () {
                        let rowData = dtAgrupaciones.row($(this).closest('tr')).data();
                        getDetalleAnual(rowData);
                    });
                },
            });


        }

        function initTablaDetalleAnual() {
            dtDetalleAnual = tablaDetalleAnual.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'gastoEnero', title: 'Gasto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'presupuestoEnero', title: 'Presupuesto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'gastoFebrero', title: 'Gasto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'presupuestoFebrero', title: 'Presupuesto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'gastoMarzo', title: 'Gasto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'presupuestoMarzo', title: 'Presupuesto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'gastoAbril', title: 'Gasto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'presupuestoAbril', title: 'Presupuesto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'gastoMayo', title: 'Gasto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'presupuestoMayo', title: 'Presupuesto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'gastoJunio', title: 'Gasto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'presupuestoJunio', title: 'Presupuesto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'gastoJulio', title: 'Gasto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'presupuestoJulio', title: 'Presupuesto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'gastoAgosto', title: 'Gasto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'presupuestoAgosto', title: 'Presupuesto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'gastoSeptiembre', title: 'Gasto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'presupuestoSeptiembre', title: 'Presupuesto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'gastoOctubre', title: 'Gasto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'presupuestoOctubre', title: 'Presupuesto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'gastoNoviembre', title: 'Gasto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'presupuestoNoviembre', title: 'Presupuesto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'gastoDiciembre', title: 'Gasto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } },
                    { data: 'presupuestoDiciembre', title: 'Presupuesto', className: 'dt-body-right dt-head-center', render: function (data, type, row) { return maskNumero(data); } }
                ],
            });
        }

        function fncGetDetallePorAgrupacion(rowData) {
            let obj = new Object();
            obj = {
                anio: cboFiltroAnio.val(),
                mes: cboFiltroMes.val(),
                idCC: rowData.idCC
            }
            axios.post("GetDetallePorAgrupacion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    tablaAgrupaciones.DataTable().clear().draw();
                    tablaAgrupaciones.DataTable().rows.add(response.data.data).draw(false);
                    $('#panelDetalleAgrupacion').css('display', 'block');
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function getDetalleAnual(rowData) {
            let obj = new Object();
            obj = {
                anio: cboFiltroAnio.val(),
                mes: cboFiltroMes.val(),
                idCC: rowData.idCC,
                tipo: rowData.tipo,
                id: rowData.id
            }
            axios.post("GetDetalleAnual", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    tablaDetalleAnual.DataTable().clear().draw();
                    tablaDetalleAnual.DataTable().rows.add(response.data.data).draw(false);
                    modalDetalleAnual.modal('show');
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        //INIT
        function initTblNivel3DeIngresosGastos() {
            dtNivel3DeIngresosGastos = tblNivel3DeIngresosGastos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'concepto', title: 'concepto' },
                    { data: 'enero', title: 'enero' },
                    { data: 'febrero', title: 'febrero' },
                    { data: 'marzo', title: 'marzo' },
                    { data: 'abril', title: 'abril' },
                    { data: 'mayo', title: 'mayo' },
                    { data: 'junio', title: 'junio' },
                    { data: 'julio', title: 'julio' },
                    { data: 'agosto', title: 'agosto' },
                    { data: 'septiembre', title: 'septiembre' },
                    { data: 'octubre', title: 'octubre' },
                    { data: 'noviembre', title: 'noviembre' },
                    { data: 'diciembre', title: 'diciembre' },
                ],
                initComplete: function (settings, json) {
                    tblNivel3DeIngresosGastos.on('click', '.classBtn', function () {
                        let rowData = dtNivel3DeIngresosGastos.row($(this).closest('tr')).data();
                    });
                    tblNivel3DeIngresosGastos.on('click', '.classBtn', function () {
                        let rowData = dtNivel3DeIngresosGastos.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function GetDetalleNivelTresIngresoGasto(cc) {
            axios.post('GetDetalleNivelTresIngresoGasto', { year: cboFiltroAnio.val(), cc: cc }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtNivel3DeIngresosGastos.clear();
                    dtNivel3DeIngresosGastos.rows.add(items);
                    dtNivel3DeIngresosGastos.draw();
                }
            }).catch(error => Alert2Error(error.message));
            btnFiltroBuscar.trigger("click");
        }

        function fncGetPresupuestoVsGastoConceptos(cc, empresa) {
            //#region SE OBTIENE LISTADO DE CP Y ARR
            let arrConstruplan = [];
            let arrArrendadora = [];
            cboFiltroCC.val().forEach(element => {
                let objCC = cboFiltroCC.find(`option[value="${element}"]`);
                let prefijoCC = objCC.attr("data-prefijo");

                if (prefijoCC == 1) {
                    if (element == "cp_46") {
                        let idCC = element.replace("cp_46", "46");
                        arrConstruplan.push(idCC);
                    } else {
                        arrConstruplan.push(element);
                    }
                } else if (prefijoCC == 2) {
                    if (element == "arr_46") {
                        let idCC = element.replace("arr_46", "46");
                        arrArrendadora.push(idCC);
                    } else {
                        arrArrendadora.push(element);
                    }
                }
            });
            //#endregion

            let obj = new Object();
            obj.year = cboFiltroAnio.val();
            obj.mes = cboFiltroMes.val();
            obj.idCC = cc;
            obj.arrConstruplan = arrConstruplan;
            obj.arrArrendadora = arrArrendadora;
            obj.empresa = empresa;
            obj.costosAdministrativos = btnCostosTotales.is(":checked");
            axios.post("GetDetalleAgrupadorConcepto", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    addRows(tblGastosVsPresupuestos, response.data.items);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetSumaCapturas(lstSumaCapturasGastos) {
            //if (cc != null && cboFiltroAnio.val() > 0) {
            //    let obj = new Object();
            //    obj = {
            //        lstCCID: cc,
            //        anio: cboFiltroAnio.val()
            //    }
            //    axios.post("GetSumaCapturasPptosGastos", obj).then(response => {
            //        let { success, items, message } = response.data;
            //        if (success) {
            //#region FILL DATATABLE

            dtCapturas.clear();
            dtCapturas.rows.add(lstSumaCapturasGastos);
            dtCapturas.draw();
            //            // initGraficaPresupuestoGasto(response.data.graficaPresupuestoGasto);
            //            //#endregion
            //        } else {
            //            Alert2Error(message);
            //        }
            //    }).catch(error => Alert2Error(error.message));
            //} else {
            //    let strMensajeError = "";
            //    cboFiltroAnio.val() <= 0 ? strMensajeError += "Es necesario indicar un año." : "";
            //    cboFiltroCC.val() <= 0 ? strMensajeError += "<br>Es necesario indicar un CC." : "";
            //    Alert2Warning(strMensajeError);
            //}
        }

        function initGraficaPresupuestoGasto(datos) {
            Highcharts.chart('graficaPresuestoGasto', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: '' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
                    labels: { format: '{value}' }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: `
                        <tr>
                            <td style="color:{series.color};padding:0">{series.name}: </td>
                            <td><b>{point.y}</b></td>
                        </tr>`,
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1, color: 'green' },
                    { name: datos.serie2Descripcion, data: datos.serie2, color: 'gray' }
                ],
                credits: { enabled: false }
            });

            // $('.highcharts-title').css("display", "none"); // ESTA LINEA HACE QUE LOS PORCENTAJES DEL NIVEL 1, SE OCULTEN.
        }

        function fncGetDataGraficasDashboard() {
            if (cboFiltroAnio.val() > 0 && cboFiltroMes.val() > 0) {

                //#region SE OBTIENE LISTADO DE CP Y ARR
                let arrConstruplan = [];
                let arrArrendadora = [];
                cboFiltroCC.val().forEach(element => {
                    let objCC = cboFiltroCC.find(`option[value="${element}"]`);
                    let prefijoCC = objCC.attr("data-prefijo");

                    if (prefijoCC == 1) {
                        if (element == "cp_46") {
                            let idCC = element.replace("cp_46", "46");
                            arrConstruplan.push(idCC);
                        } else {
                            arrConstruplan.push(element);
                        }
                    } else if (prefijoCC == 2) {
                        if (element == "arr_46") {
                            let idCC = element.replace("arr_46", "46");
                            arrArrendadora.push(idCC);
                        } else {
                            arrArrendadora.push(element);
                        }
                    }
                });
                //#endregion
                cboFiltroInformeAnio.val(cboFiltroAnio.val());
                cboFiltroInformeAnio.change();
                cboFiltroInformeMes.val(cboFiltroMes.val());
                cboFiltroInformeMes.change();
                cboFiltroInformeEmpresa.val(cboFiltroEmpresas.val());
                cboFiltroInformeEmpresa.change();
                axios.post("GetDataGraficasDashboard",
                    {
                        year: cboFiltroAnio.val(),
                        mes: cboFiltroMes.val(),
                        // listaCC: getValoresMultiples('#cboFiltroCC').filter(function (x) { return parseInt(x) }),
                        arrConstruplan: arrConstruplan,
                        arrArrendadora: arrArrendadora,
                        costosAdministrativos: btnCostosTotales.is(":checked")
                    }).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            //#region FILL DATATABLE
                            // dtCapturas.clear();
                            // dtCapturas.rows.add(response.data.lstSumaCapturas);
                            // dtCapturas.draw();
                            // initGraficaPresupuestoGasto(response.data.graficaPresupuestoGasto);
                            //initGraficaArcoGastoIngreso(response.data.gfxIngresoGasto, response.data.gfxIngresoGastoPorcentaje);
                            initGastoIngreso(response.data.montoGasto, response.data.montoIngreso, response.data.porcentajeCumplimientoIngresoGasto, response.data.gfxIngresoGastoPorcentaje, response.data.colorIngresoGasto);
                            initGastoPpto(response.data.montoGastoDB, response.data.montoIngreso, response.data.porcentajeCumplimientoIngresoGastoPpto, response.data.porcentajeRealAcumuladoDB, response.data.colorIngresoGastoPpto);
                            initGastoIngresoMensual(response.data.montoGastoMensual, response.data.montoIngresoMensual, response.data.porcentajeCumplimientoIngresoGastoMensual, response.data.gfxIngresoGastoRealMensual, response.data.colorIngresoGastoMensual);
                            initGraficaArcoPresupuesto(response.data.gfxPresupuestoGasto, response.data.gfxPresupuestoGastoPorcentaje);
                            initGraficaArcoPresupuestoInforme(response.data.gfxPresupuestoGasto, response.data.gfxPresupuestoGastoPorcentaje);
                            initGraficaArcoCumplimiento(response.data.gfxCumplimiento);
                            initGraficaArcoCumplimientoInforme(response.data.gfxCumplimiento);
                            initGraficaArcoProyeccion(response.data.objGraficaProyeccion);
                            initGraficaArcoProyeccionInforme(response.data.objGraficaProyeccion);
                            initGraficaArcoPresupuestoMensual(response.data.gfxPresupuestoGastoMensual, response.data.gfxPresupuestoGastoPorcentajeMensual);
                            initGraficaArcoPresupuestoMensualInforme(response.data.gfxPresupuestoGastoMensual, response.data.gfxPresupuestoGastoPorcentajeMensual);

                            if (cboFiltroCC.val().length == 1 && cboFiltroCC.val()[0] == '113') { txtRatioGastoVsIngresoAcumulado.text('--%'); txtRatioGastoVsIngresoAcumuladoInforme.text('--%'); }
                            else { txtRatioGastoVsIngresoAcumulado.text(response.data.ratioGastoVsIngreso + '%'); txtRatioGastoVsIngresoAcumuladoInforme.text(response.data.ratioGastoVsIngreso + '%'); }

                            if (cboFiltroCC.val().length == 1 && cboFiltroCC.val()[0] == '113') { txtRatioGastoVsIngresoMensual.text('--%'); txtRatioGastoVsIngresoMensualInforme.text('--%'); }
                            else { txtRatioGastoVsIngresoMensual.text(response.data.ratioGastoVsIngreso + '%'); txtRatioGastoVsIngresoMensualInforme.text(response.data.ratioGastoVsIngreso + '%'); }

                            if (cboFiltroCC.val().length == 1 && cboFiltroCC.val()[0] == '113') txtRatioGastoVsIngresoAcumuladoPpto.text('--%');
                            else txtRatioGastoVsIngresoAcumuladoPpto.text(response.data.ratioGastoVsIngreso + '%');

                            //txtRatioGastoVsIngresoMensual.text(response.data.ratioGastoVsIngreso + '%');
                            //txtRatioGastoVsIngresoAcumuladoPpto.text(response.data.ratioGastoVsIngreso + '%');

                            if (parametro_idCC > 0) {
                                setTimeout(function () {
                                    btnFiltroReportePlanAccion.trigger("click")
                                }, 1000);
                            }
                            fncGetPermisoVisualizarEnvioInformes();
                            //#endregion
                        } else {
                            Alert2Error(message);
                        }
                    }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = "";
                cboFiltroAnio.val() <= 0 ? strMensajeError += "Es necesario indicar un año." : "";
                cboFiltroMes.val() <= 0 ? strMensajeError += "<br>Es necesario indicar un Mes." : "";
                Alert2Warning(strMensajeError);
            }
        }

        function fncGetDataGraficasInforme(cc, empresa) {
            if (cboFiltroAnio.val() > 0 && cboFiltroMes.val() > 0) {

                //#region SE OBTIENE LISTADO DE CP Y ARR
                let arrConstruplan = [];
                let arrArrendadora = [];

                if (empresa == 1) arrConstruplan.push(cc);
                else arrArrendadora.push(cc);
                //#endregion
                axios.post("GetDataGraficasInforme",
                    {
                        year: cboFiltroAnio.val(),
                        mes: cboFiltroMes.val(),
                        // listaCC: getValoresMultiples('#cboFiltroCC').filter(function (x) { return parseInt(x) }),
                        arrConstruplan: arrConstruplan,
                        arrArrendadora: arrArrendadora,
                        costosAdministrativos: btnCostosTotales.is(":checked")
                    }).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            initGastoIngresoInforme(response.data.montoGasto, response.data.montoIngreso, response.data.porcentajeCumplimientoIngresoGasto, response.data.gfxIngresoGastoPorcentaje, response.data.colorIngresoGasto);
                            initGastoPptoInforme(response.data.montoGastoDB, response.data.montoIngreso, response.data.porcentajeCumplimientoIngresoGastoPpto, response.data.porcentajeRealAcumuladoDB, response.data.colorIngresoGastoPpto);
                            initGastoIngresoMensualInforme(response.data.montoGastoMensual, response.data.montoIngresoMensual, response.data.porcentajeCumplimientoIngresoGastoMensual, response.data.gfxIngresoGastoRealMensual, response.data.colorIngresoGastoMensual);
                            initGraficaArcoPresupuestoInforme(response.data.gfxPresupuestoGasto, response.data.gfxPresupuestoGastoPorcentaje);
                            initGraficaArcoCumplimientoInforme(response.data.gfxCumplimiento);
                            initGraficaArcoProyeccionInforme(response.data.objGraficaProyeccion);
                            initGraficaArcoPresupuestoMensualInforme(response.data.gfxPresupuestoGastoMensual, response.data.gfxPresupuestoGastoPorcentajeMensual);

                            if (cboFiltroCC.val().length == 1 && cboFiltroCC.val()[0] == '113') { txtRatioGastoVsIngresoAcumuladoInforme.text('--%'); }
                            else { txtRatioGastoVsIngresoAcumuladoInforme.text(response.data.ratioGastoVsIngreso + '%'); }

                            if (cboFiltroCC.val().length == 1 && cboFiltroCC.val()[0] == '113') { txtRatioGastoVsIngresoMensualInforme.text('--%'); }
                            else { txtRatioGastoVsIngresoMensualInforme.text(response.data.ratioGastoVsIngreso + '%'); }

                            //if(cboFiltroCC.val().length == 1 &&  cboFiltroCC.val()[0] == '113') txtRatioGastoVsIngresoAcumuladoPpto.text('--%');
                            //else txtRatioGastoVsIngresoAcumuladoPpto.text(response.data.ratioGastoVsIngreso + '%');

                            //txtRatioGastoVsIngresoMensual.text(response.data.ratioGastoVsIngreso + '%');
                            //txtRatioGastoVsIngresoAcumuladoPpto.text(response.data.ratioGastoVsIngreso + '%');

                            if (parametro_idCC > 0) {
                                setTimeout(function () {
                                    btnFiltroReportePlanAccion.trigger("click")
                                }, 1000);
                            }
                            //#endregion
                        } else {
                            Alert2Error(message);
                        }
                    }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = "";
                cboFiltroAnio.val() <= 0 ? strMensajeError += "Es necesario indicar un año." : "";
                cboFiltroMes.val() <= 0 ? strMensajeError += "<br>Es necesario indicar un Mes." : "";
                Alert2Warning(strMensajeError);
            }
        }

        function initGastoIngreso(gasto, ingreso, porcentaje, gfxIngresoGastoReal, colorIngresoGasto) {
            let color = '';
            let barColor = '';
            let _porcentaje = (gasto * 100) / ingreso;

            if (cboFiltroCC.val().length == 1 && cboFiltroCC.val()[0] == '113') {
                color = '#0A9396';
                barColor = 'progress-bar-success';
                gasto = 0;
                ingreso = 0;


                montoGasto.text('--');
                montoGastoInforme.text('--');
                montoIngreso.text('--');
                montoIngresoInforme.text('--');
                porcentajeCumplimientoIngresoGasto.text(`--%`);
                porcentajeCumplimientoIngresoGastoInforme.text(`--%`);
                txtRatioGastoVsIngresoAcumulado.text(`--%`);
                txtRatioGastoVsIngresoAcumuladoInforme.text(`--%`);
                porcentajeCumplimientoIngresoGasto.css('color', colorIngresoGasto);
                porcentajeCumplimientoIngresoGastoInforme.css('color', colorIngresoGasto);
                gastoIngresoBar.removeClass('progress-bar-danger');
                gastoIngresoBar.removeClass('progress-bar-warning');
                gastoIngresoBar.removeClass('progress-bar-success');
                gastoIngresoBar.addClass(barColor);
                gastoIngresoBar.css('width', '--%');
                gastoIngresoBarInforme.removeClass('progress-bar-danger');
                gastoIngresoBarInforme.removeClass('progress-bar-warning');
                gastoIngresoBarInforme.removeClass('progress-bar-success');
                gastoIngresoBarInforme.addClass(barColor);
                gastoIngresoBarInforme.css('width', '--%');
            }
            else {
                if (_porcentaje >= 100) {
                    color = '#AE2012';
                    barColor = 'progress-bar-danger';
                } else if (_porcentaje > 95) {
                    color = '#EE9B00';
                    barColor = 'progress-bar-warning';
                } else {
                    color = '#0A9396';
                    barColor = 'progress-bar-success';
                }

                montoGasto.text(maskNumero_NoDecimal(gasto));
                montoGastoInforme.text(maskNumero_NoDecimal(gasto));
                montoIngreso.text(maskNumero_NoDecimal(ingreso));
                montoIngresoInforme.text(maskNumero_NoDecimal(ingreso));
                porcentajeCumplimientoIngresoGasto.text(`${Math.trunc(gfxIngresoGastoReal)}%`);
                porcentajeCumplimientoIngresoGasto.css('color', colorIngresoGasto);
                porcentajeCumplimientoIngresoGastoInforme.text(`${Math.trunc(gfxIngresoGastoReal)}%`);
                porcentajeCumplimientoIngresoGastoInforme.css('color', colorIngresoGasto);
                gastoIngresoBar.removeClass('progress-bar-danger');
                gastoIngresoBar.removeClass('progress-bar-warning');
                gastoIngresoBar.removeClass('progress-bar-success');
                gastoIngresoBar.addClass(barColor);
                gastoIngresoBar.css('width', _porcentaje + '%');
                gastoIngresoBarInforme.removeClass('progress-bar-danger');
                gastoIngresoBarInforme.removeClass('progress-bar-warning');
                gastoIngresoBarInforme.removeClass('progress-bar-success');
                gastoIngresoBarInforme.addClass(barColor);
                gastoIngresoBarInforme.css('width', _porcentaje + '%');

                montoReal.text(maskNumeroPorcentaje(porcentaje));
                montoRealInforme.text(maskNumeroPorcentaje(porcentaje));
            }

        }

        function initGastoIngresoInforme(gasto, ingreso, porcentaje, gfxIngresoGastoReal, colorIngresoGasto) {
            let color = '';
            let barColor = '';
            let _porcentaje = (gasto * 100) / ingreso;

            if (cboFiltroCC.val().length == 1 && cboFiltroCC.val()[0] == '113') {
                color = '#0A9396';
                barColor = 'progress-bar-success';
                gasto = 0;
                ingreso = 0;

                montoGastoInforme.text('--');
                montoIngresoInforme.text('--');
                porcentajeCumplimientoIngresoGastoInforme.text(`--%`);
                txtRatioGastoVsIngresoAcumuladoInforme.text(`--%`);
                porcentajeCumplimientoIngresoGastoInforme.css('color', colorIngresoGasto);
                gastoIngresoBarInforme.removeClass('progress-bar-danger');
                gastoIngresoBarInforme.removeClass('progress-bar-warning');
                gastoIngresoBarInforme.removeClass('progress-bar-success');
                gastoIngresoBarInforme.addClass(barColor);
                gastoIngresoBarInforme.css('width', '--%');
            }
            else {
                if (_porcentaje >= 100) {
                    color = '#AE2012';
                    barColor = 'progress-bar-danger';
                } else if (_porcentaje > 95) {
                    color = '#EE9B00';
                    barColor = 'progress-bar-warning';
                } else {
                    color = '#0A9396';
                    barColor = 'progress-bar-success';
                }

                montoGastoInforme.text(maskNumero_NoDecimal(gasto));
                montoIngresoInforme.text(maskNumero_NoDecimal(ingreso));
                porcentajeCumplimientoIngresoGastoInforme.text(`${Math.trunc(gfxIngresoGastoReal)}%`);
                porcentajeCumplimientoIngresoGastoInforme.css('color', colorIngresoGasto);
                gastoIngresoBarInforme.removeClass('progress-bar-danger');
                gastoIngresoBarInforme.removeClass('progress-bar-warning');
                gastoIngresoBarInforme.removeClass('progress-bar-success');
                gastoIngresoBarInforme.addClass(barColor);
                gastoIngresoBarInforme.css('width', _porcentaje + '%');

                montoRealInforme.text(maskNumeroPorcentaje(porcentaje));
            }

        }

        function initGastoPpto(gasto, ingreso, porcentaje, gfxIngresoGastoReal, colorIngresoGasto) {
            let color = '';
            let barColor = '';
            // console.log(gasto);
            // console.log(ingreso);
            let _porcentaje = (ingreso * 100) / gasto;
            // console.log(_porcentaje);


            if (cboFiltroCC.val().length == 1 && cboFiltroCC.val()[0] == '113') {
                color = '#0A9396';
                barColor = 'progress-bar-success';
                gasto = 0;
                ingreso = 0;


                montoGastoPpto.text('--');
                montoGastoPptoInforme.text('--');
                montoIngresoPpto.text('--');
                montoIngresoPptoInforme.text('--');
                porcentajeCumplimientoIngresoGastoPpto.text(`--%`);
                porcentajeCumplimientoIngresoGastoPpto.css('color', colorIngresoGasto);
                porcentajeCumplimientoIngresoGastoPptoInforme.text(`--%`);
                porcentajeCumplimientoIngresoGastoPptoInforme.css('color', colorIngresoGasto);
                gastoIngresoBarPpto.removeClass('progress-bar-danger');
                gastoIngresoBarPpto.removeClass('progress-bar-warning');
                gastoIngresoBarPpto.removeClass('progress-bar-success');
                gastoIngresoBarPpto.addClass(barColor);
                gastoIngresoBarPpto.css('width', '--%');
                gastoIngresoBarPptoInforme.removeClass('progress-bar-danger');
                gastoIngresoBarPptoInforme.removeClass('progress-bar-warning');
                gastoIngresoBarPptoInforme.removeClass('progress-bar-success');
                gastoIngresoBarPptoInforme.addClass(barColor);
                gastoIngresoBarPptoInforme.css('width', '--%');

                montoRealPpto.text('--');
            }
            else {
                if (_porcentaje >= 100) {
                    color = '#AE2012';
                    barColor = 'progress-bar-danger';
                } else if (_porcentaje > 95) {
                    color = '#EE9B00';
                    barColor = 'progress-bar-warning';
                } else {
                    color = '#0A9396';
                    barColor = 'progress-bar-success';
                }

                montoGastoPpto.text(maskNumero_NoDecimal(gasto));
                montoGastoPptoInforme.text(maskNumero_NoDecimal(gasto));
                montoIngresoPpto.text(maskNumero_NoDecimal(ingreso));
                montoIngresoPptoInforme.text(maskNumero_NoDecimal(ingreso));
                porcentajeCumplimientoIngresoGastoPpto.text(`${Math.trunc(_porcentaje)}%`);
                porcentajeCumplimientoIngresoGastoPpto.css('color', colorIngresoGasto);
                porcentajeCumplimientoIngresoGastoPptoInforme.text(`${Math.trunc(_porcentaje)}%`);
                porcentajeCumplimientoIngresoGastoPptoInforme.css('color', colorIngresoGasto);
                gastoIngresoBarPpto.removeClass('progress-bar-danger');
                gastoIngresoBarPpto.removeClass('progress-bar-warning');
                gastoIngresoBarPpto.removeClass('progress-bar-success');
                gastoIngresoBarPpto.addClass(barColor);
                gastoIngresoBarPpto.css('width', _porcentaje + '%');
                gastoIngresoBarPptoInforme.removeClass('progress-bar-danger');
                gastoIngresoBarPptoInforme.removeClass('progress-bar-warning');
                gastoIngresoBarPptoInforme.removeClass('progress-bar-success');
                gastoIngresoBarPptoInforme.addClass(barColor);
                gastoIngresoBarPptoInforme.css('width', _porcentaje + '%');

                montoRealPpto.text(maskNumeroPorcentaje(porcentaje));
            }
        }

        function initGastoPptoInforme(gasto, ingreso, porcentaje, gfxIngresoGastoReal, colorIngresoGasto) {
            let color = '';
            let barColor = '';
            // console.log(gasto);
            // console.log(ingreso);
            let _porcentaje = (ingreso * 100) / gasto;
            // console.log(_porcentaje);


            if (cboFiltroCC.val().length == 1 && cboFiltroCC.val()[0] == '113') {
                color = '#0A9396';
                barColor = 'progress-bar-success';
                gasto = 0;
                ingreso = 0;

                montoGastoPptoInforme.text('--');
                montoIngresoPptoInforme.text('--');
                porcentajeCumplimientoIngresoGastoPptoInforme.text(`--%`);
                porcentajeCumplimientoIngresoGastoPptoInforme.css('color', colorIngresoGasto);
                gastoIngresoBarPptoInforme.removeClass('progress-bar-danger');
                gastoIngresoBarPptoInforme.removeClass('progress-bar-warning');
                gastoIngresoBarPptoInforme.removeClass('progress-bar-success');
                gastoIngresoBarPptoInforme.addClass(barColor);
                gastoIngresoBarPptoInforme.css('width', '--%');

            }
            else {
                if (_porcentaje >= 100) {
                    color = '#AE2012';
                    barColor = 'progress-bar-danger';
                } else if (_porcentaje > 95) {
                    color = '#EE9B00';
                    barColor = 'progress-bar-warning';
                } else {
                    color = '#0A9396';
                    barColor = 'progress-bar-success';
                }

                montoGastoPptoInforme.text(maskNumero_NoDecimal(gasto));
                montoIngresoPptoInforme.text(maskNumero_NoDecimal(ingreso));
                porcentajeCumplimientoIngresoGastoPptoInforme.text(`${Math.trunc(_porcentaje)}%`);
                porcentajeCumplimientoIngresoGastoPptoInforme.css('color', colorIngresoGasto);
                gastoIngresoBarPptoInforme.removeClass('progress-bar-danger');
                gastoIngresoBarPptoInforme.removeClass('progress-bar-warning');
                gastoIngresoBarPptoInforme.removeClass('progress-bar-success');
                gastoIngresoBarPptoInforme.addClass(barColor);
                gastoIngresoBarPptoInforme.css('width', _porcentaje + '%');

            }
        }

        function initGastoIngresoMensual(gasto, ingreso, porcentaje, gfxIngresoGastoReal, colorIngresoGasto) {
            let color = '';
            let barColor = '';
            let _porcentaje = (gasto * 100) / ingreso;


            if (cboFiltroCC.val().length == 1 && cboFiltroCC.val()[0] == '113') {
                color = '#0A9396';
                barColor = 'progress-bar-success';
                gasto = 0;
                ingreso = 0;

                montoGastoMensual.text('--');
                montoGastoMensualInforme.text('--');
                montoIngresoMensual.text('--');
                montoIngresoMensualInforme.text('--');
                porcentajeCumplimientoIngresoGastoMensual.text(`--%`);
                porcentajeCumplimientoIngresoGastoMensual.css('color', colorIngresoGasto);
                porcentajeCumplimientoIngresoGastoMensualInforme.text(`--%`);
                porcentajeCumplimientoIngresoGastoMensualInforme.css('color', colorIngresoGasto);
                montoRealMensual.text('--');
                montoRealMensualInforme.text('--');
            }
            else {

                if (_porcentaje >= 100) {
                    color = '#AE2012';
                    barColor = 'progress-bar-danger';
                } else if (_porcentaje > 95) {
                    color = '#EE9B00';
                    barColor = 'progress-bar-warning';
                } else {
                    color = '#0A9396';
                    barColor = 'progress-bar-success';
                }

                montoGastoMensual.text(maskNumero_NoDecimal(gasto));
                montoGastoMensualInforme.text(maskNumero_NoDecimal(gasto));
                montoIngresoMensual.text(maskNumero_NoDecimal(ingreso));
                montoIngresoMensualInforme.text(maskNumero_NoDecimal(ingreso));
                porcentajeCumplimientoIngresoGastoMensual.text(`${Math.trunc(porcentaje)}%`);
                porcentajeCumplimientoIngresoGastoMensual.css('color', colorIngresoGasto);
                porcentajeCumplimientoIngresoGastoMensualInforme.text(`${Math.trunc(porcentaje)}%`);
                porcentajeCumplimientoIngresoGastoMensualInforme.css('color', colorIngresoGasto);
                montoRealMensual.text(maskNumeroPorcentaje(gfxIngresoGastoReal));
                montoRealMensualInforme.text(maskNumeroPorcentaje(gfxIngresoGastoReal));
            }

        }

        function initGastoIngresoMensualInforme(gasto, ingreso, porcentaje, gfxIngresoGastoReal, colorIngresoGasto) {
            let color = '';
            let barColor = '';
            let _porcentaje = (gasto * 100) / ingreso;

            if (cboFiltroCC.val().length == 1 && cboFiltroCC.val()[0] == '113') {
                color = '#0A9396';
                barColor = 'progress-bar-success';
                gasto = 0;
                ingreso = 0;

                montoGastoMensualInforme.text('--');
                montoIngresoMensualInforme.text('--');
                porcentajeCumplimientoIngresoGastoMensualInforme.text(`--%`);
                porcentajeCumplimientoIngresoGastoMensualInforme.css('color', colorIngresoGasto);
                montoRealMensualInforme.text('--');
            }
            else {
                if (_porcentaje >= 100) {
                    color = '#AE2012';
                    barColor = 'progress-bar-danger';
                } else if (_porcentaje > 95) {
                    color = '#EE9B00';
                    barColor = 'progress-bar-warning';
                } else {
                    color = '#0A9396';
                    barColor = 'progress-bar-success';
                }

                montoGastoMensualInforme.text(maskNumero_NoDecimal(gasto));
                montoIngresoMensualInforme.text(maskNumero_NoDecimal(ingreso));
                porcentajeCumplimientoIngresoGastoMensualInforme.text(`${Math.trunc(porcentaje)}%`);
                porcentajeCumplimientoIngresoGastoMensualInforme.css('color', colorIngresoGasto);
                montoRealMensualInforme.text(maskNumeroPorcentaje(gfxIngresoGastoReal));
            }
        }

        function initGraficaArcoPresupuesto(datos, porcentaje) {
            // let arr = new Array();
            // let arr2 = new Array();
            // datos.serie.forEach(element => {
            //     let obj = new Object();
            //     obj.color = element.color;
            //     obj.name = element.name;
            //     obj.y = maskNumero_NoDecimal(element.y);
            //     arr.push(obj);
            // });

            Highcharts.chart('graficaArcoPresupuesto', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: 0,
                    plotShadow: false,
                    width: null,
                    height: 240,
                    spacingLeft: 0,
                    spacingRight: 0,
                    spacingBottom: 0,
                    spacingTop: 0
                },
                title: {
                    text: `${Math.trunc(porcentaje)}%`,
                    align: 'center',
                    verticalAlign: 'middle',
                    y: 60
                },
                tooltip: {
                    enabled: false
                    //pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            enabled: true,
                            distance: -50,
                            style: {
                                fontWeight: 'bold',
                                color: 'white'
                            },
                            connectorColor: 'silver',
                            formatter: function () {
                                if (this.point.name == 'PRESUPUESTO') {
                                    return 'PPTO' + ' 100%';
                                }
                                if (this.point.name == 'GASTO' && this.point.index == 1) {
                                    return 'GASTO' + ' ' + maskNumeroPorcentaje(100 + ((this.point.y * 100) / 66.6));
                                }
                                if (this.point.name == 'GASTO' && this.point.index == 0) {
                                    return 'GASTO' + ' ' + maskNumeroPorcentaje(((this.point.y * 100) / 66.6));
                                }
                                return ''
                            }
                        },
                        startAngle: -90,
                        endAngle: 90,
                        center: ['50%', '75%'],
                        size: '140%',
                        point: {
                            events: {
                                click: function () {
                                    getDetalleNivel2PresupuestoGasto().done(function (response) {
                                        inpConsultaNivel.attr('data-cumplimientoIngreso', 1);

                                        if (response && response.success) {
                                            if (inpConsultaNivel.attr('data-cumplimientoIngreso') == 2) {
                                                GetDetalleNivelTresIngresoGasto(cboFiltroCC.val());//omaromar
                                            } else {
                                                //fncGetSumaCapturas(cboFiltroCC.val())
                                                fncGetSumaCapturas(response.lstSumaCapturasGastos);
                                            }
                                            txtCumplimientoControlEjecucion.text(`${Math.trunc(response.items.cumplimientoAcumuladoEmpresa)}%`);
                                            if (response.items.cumplimientoAcumuladoEmpresa > 100) {
                                                txtCumplimientoControlEjecucion.css('color', 'red');
                                            } else if (response.items.cumplimientoAcumuladoEmpresa >= 97 && response.items.cumplimientoAcumuladoEmpresa <= 100) {
                                                txtCumplimientoControlEjecucion.css('color', 'yellow');
                                            } else if (response.items.cumplimientoAcumuladoEmpresa < 97) {
                                                txtCumplimientoControlEjecucion.css('color', 'green');
                                            }
                                            txtMesCierreControlEjecucion.text(response.items.mesCierre);
                                            initGraficaControlEjecucion(response.items.graficaBarraMensual); //TODO
                                            btnCerrarModal.html(`<i class="fa fa-arrow-left"></i>&nbsp;Cerrar`);
                                            periodoResumen.text('DETALLE PRESUPUESTO / GASTOS')
                                            // panelGraficas.fadeToggle('fast', 'linear', function() {
                                            //     panelDetalleNivel2.fadeToggle('fast', 'linear', function() {
                                            //         addRows(tblControlEjecucion, response.items.datosTablaCC);
                                            //     });
                                            // });
                                            SetNombreModal();
                                            mdlDetalleNivel.modal("show");
                                            panelGeneral0.css('display', 'block');
                                            panelDetalleNivel2.css('display', 'block');
                                            panelDetalleNivel3.css('display', 'none');
                                            panelDetalleNivel4.css('display', 'none');
                                            panelDetalleNivel4Ctas.css('display', 'none');
                                            btnNivel3.css('display', 'none');
                                            btnNivel4.css('display', 'none');
                                            // btnPlanAccion.css("display", "none");
                                            $('.panel-detalle-total').css('display', 'block');
                                            //btnNivel2.css('display','block');

                                            // console.log(response.items.datosTablaCC);
                                            addRows(tblControlEjecucion, response.items.datosTablaCC);

                                            // console.log(response.data);
                                            let anioPasado = "2021";

                                            tblControlEjecucion.DataTable().columns(1).visible(true);
                                            tblControlEjecucion.find('thead tr th:eq(2)').text('Presupuesto Mensual');
                                            tblControlEjecucion.find('thead tr th:eq(3)').text('Gasto Mensual');
                                            tblControlEjecucion.find('thead tr th:eq(4)').text('Diferencia');
                                            tblControlEjecucion.find('thead tr th:eq(5)').text('Cumplimiento');
                                            tblControlEjecucion.find('thead tr th:eq(6)').text('Pto Acumulado');
                                            tblControlEjecucion.find('thead tr th:eq(7)').text('Gasto Acumulado');
                                            tblControlEjecucion.find('thead tr th:eq(8)').text('Diferencia');
                                            tblControlEjecucion.find('thead tr th:eq(9)').text('Cumplimiento');
                                            tblControlEjecucion.find('thead tr th:eq(10)').text(`Gasto acumulado ${anioPasado}`);
                                        }
                                    });
                                }
                            }
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: '',
                    innerSize: '50%',
                    // data: [ //COMENTADO
                    //     ['Chrome', 58.9],
                    //     ['Firefox', 13.29],
                    //     ['Internet Explorer', 13],
                    //     ['Edge', 3.78],
                    //     ['Safari', 3.42],
                    //     {
                    //         name: 'Other',
                    //         y: 7.61,
                    //         dataLabels: {
                    //             enabled: false
                    //         }
                    //     }
                    // ] //COMENTADO
                    data: datos.serie // HABILITAR
                }],
                credits: {
                    enabled: false
                }
            });
        }
        function initGraficaArcoPresupuestoInforme(datos, porcentaje) {
            // let arr = new Array();
            // let arr2 = new Array();
            // datos.serie.forEach(element => {
            //     let obj = new Object();
            //     obj.color = element.color;
            //     obj.name = element.name;
            //     obj.y = maskNumero_NoDecimal(element.y);
            //     arr.push(obj);
            // });

            Highcharts.chart('graficaArcoPresupuestoInforme', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: 0,
                    plotShadow: false,
                    width: null,
                    height: 240,
                    spacingLeft: 0,
                    spacingRight: 0,
                    spacingBottom: 0,
                    spacingTop: 0
                },
                title: {
                    text: `${Math.trunc(porcentaje)}%`,
                    align: 'center',
                    verticalAlign: 'middle',
                    y: 60
                },
                tooltip: {
                    enabled: false
                    //pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            enabled: true,
                            distance: -50,
                            style: {
                                fontWeight: 'bold',
                                color: 'white'
                            },
                            connectorColor: 'silver',
                            formatter: function () {
                                if (this.point.name == 'PRESUPUESTO') {
                                    return 'PPTO' + ' 100%';
                                }
                                if (this.point.name == 'GASTO' && this.point.index == 1) {
                                    return 'GASTO' + ' ' + maskNumeroPorcentaje(100 + ((this.point.y * 100) / 66.6));
                                }
                                if (this.point.name == 'GASTO' && this.point.index == 0) {
                                    return 'GASTO' + ' ' + maskNumeroPorcentaje(((this.point.y * 100) / 66.6));
                                }
                                return ''
                            }
                        },
                        startAngle: -90,
                        endAngle: 90,
                        center: ['50%', '75%'],
                        size: '140%',
                        point: {
                            events: {
                                click: function () {
                                    getDetalleNivel2PresupuestoGasto().done(function (response) {
                                        inpConsultaNivel.attr('data-cumplimientoIngreso', 1);

                                        if (response && response.success) {
                                            if (inpConsultaNivel.attr('data-cumplimientoIngreso') == 2) {
                                                GetDetalleNivelTresIngresoGasto(cboFiltroCC.val());//omaromar
                                            } else {
                                                //fncGetSumaCapturas(cboFiltroCC.val())
                                                fncGetSumaCapturas(response.lstSumaCapturasGastos);
                                            }
                                            txtCumplimientoControlEjecucion.text(`${Math.trunc(response.items.cumplimientoAcumuladoEmpresa)}%`);
                                            if (response.items.cumplimientoAcumuladoEmpresa > 100) {
                                                txtCumplimientoControlEjecucion.css('color', 'red');
                                            } else if (response.items.cumplimientoAcumuladoEmpresa >= 97 && response.items.cumplimientoAcumuladoEmpresa <= 100) {
                                                txtCumplimientoControlEjecucion.css('color', 'yellow');
                                            } else if (response.items.cumplimientoAcumuladoEmpresa < 97) {
                                                txtCumplimientoControlEjecucion.css('color', 'green');
                                            }
                                            txtMesCierreControlEjecucion.text(response.items.mesCierre);
                                            initGraficaControlEjecucion(response.items.graficaBarraMensual); //TODO
                                            btnCerrarModal.html(`<i class="fa fa-arrow-left"></i>&nbsp;Cerrar`);
                                            periodoResumen.text('DETALLE PRESUPUESTO / GASTOS')
                                            // panelGraficas.fadeToggle('fast', 'linear', function() {
                                            //     panelDetalleNivel2.fadeToggle('fast', 'linear', function() {
                                            //         addRows(tblControlEjecucion, response.items.datosTablaCC);
                                            //     });
                                            // });
                                            SetNombreModal();
                                            mdlDetalleNivel.modal("show");
                                            panelGeneral0.css('display', 'block');
                                            panelDetalleNivel2.css('display', 'block');
                                            panelDetalleNivel3.css('display', 'none');
                                            panelDetalleNivel4.css('display', 'none');
                                            panelDetalleNivel4Ctas.css('display', 'none');
                                            btnNivel3.css('display', 'none');
                                            btnNivel4.css('display', 'none');
                                            // btnPlanAccion.css("display", "none");
                                            $('.panel-detalle-total').css('display', 'block');
                                            //btnNivel2.css('display','block');

                                            // console.log(response.items.datosTablaCC);
                                            addRows(tblControlEjecucion, response.items.datosTablaCC);

                                            // console.log(response.data);
                                            let anioPasado = "2021";

                                            tblControlEjecucion.DataTable().columns(1).visible(true);
                                            tblControlEjecucion.find('thead tr th:eq(2)').text('Presupuesto Mensual');
                                            tblControlEjecucion.find('thead tr th:eq(3)').text('Gasto Mensual');
                                            tblControlEjecucion.find('thead tr th:eq(4)').text('Diferencia');
                                            tblControlEjecucion.find('thead tr th:eq(5)').text('Cumplimiento');
                                            tblControlEjecucion.find('thead tr th:eq(6)').text('Pto Acumulado');
                                            tblControlEjecucion.find('thead tr th:eq(7)').text('Gasto Acumulado');
                                            tblControlEjecucion.find('thead tr th:eq(8)').text('Diferencia');
                                            tblControlEjecucion.find('thead tr th:eq(9)').text('Cumplimiento');
                                            tblControlEjecucion.find('thead tr th:eq(10)').text(`Gasto acumulado ${anioPasado}`);
                                        }
                                    });
                                }
                            }
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: '',
                    innerSize: '50%',
                    // data: [ //COMENTADO
                    //     ['Chrome', 58.9],
                    //     ['Firefox', 13.29],
                    //     ['Internet Explorer', 13],
                    //     ['Edge', 3.78],
                    //     ['Safari', 3.42],
                    //     {
                    //         name: 'Other',
                    //         y: 7.61,
                    //         dataLabels: {
                    //             enabled: false
                    //         }
                    //     }
                    // ] //COMENTADO
                    data: datos.serie // HABILITAR
                }],
                credits: {
                    enabled: false
                }
            });
        }

        // function initGraficaArcoGastoIngreso(datos, porcentaje) {
        //     Highcharts.chart('graficaArcoGastoIngreso', {
        //         chart: {
        //             plotBackgroundColor: null,
        //             plotBorderWidth: 0,
        //             plotShadow: false,
        //             width: null,
        //             height: 240,
        //             spacingLeft: 0,
        //             spacingRight: 0,
        //             spacingBottom: 0,
        //             spacingTop: 0,
        //         },

        //         title: {
        //             text: maskNumeroPorcentaje(porcentaje),
        //             align: 'center',
        //             verticalAlign: 'middle',
        //             y: 60
        //         },

        //         tooltip: {
        //             pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        //         },

        //         accessibility: {
        //             point: {
        //                 valueSuffix: '%'
        //             }
        //         },

        //         plotOptions: {
        //             pie: {
        //                 dataLabels: {
        //                     enabled: true,
        //                     distance: -50,
        //                     style: {
        //                         fontWeight: 'bold',
        //                         color: 'white'
        //                     }
        //                 },
        //                 startAngle: -90,
        //                 endAngle: 90,
        //                 center: ['50%', '75%'],
        //                 size: '140%',
        //                 point: {
        //                     events: {
        //                         click: function () {
        //                             getDetalleNivel2IngresoGasto().done(function (response) {
        //                                 if (response && response.success) {
        //                                     txtCumplimientoControlEjecucion.text(maskNumeroPorcentaje(response.items.cumplimientoAcumuladoEmpresa));
        //                                     txtMesCierreControlEjecucion.text(response.items.mesCierre);
        //                                     initGraficaControlEjecucion(response.items.graficaBarraMensual);
        //                                     periodoResumen.text('DETALLE INGRESOS / GASTOS')
        //                                     // panelGraficas.fadeToggle('fast', 'linear', function() {
        //                                     //     panelDetalleNivel2.fadeToggle('fast', 'linear', function() {
        //                                     //         addRows(tblControlEjecucion, response.items.datosTablaCC);
        //                                     //     });
        //                                     // });
        //                                     mdlDetalleNivel.modal("show");
        //                                     btnNivel3.css('display', 'none');
        //                                     btnNivel4.css('display', 'none');
        //                                     panelDetalleNivel2.css('display', 'block');
        //                                     panelDetalleNivel3.css('display', 'none');
        //                                     panelDetalleNivel4.css('display', 'none');
        //                                     panelDetalleNivel4Ctas.css('display', 'none');
        //                                     //btnNivel2.css('display','block');

        //                                     addRows(tblControlEjecucion, response.items.datosTablaCC);


        //                                     $('#tblControlEjecucion_wrapper').find('thead tr th:eq(2)').text('Gasto Mensual');
        //                                     $('#tblControlEjecucion_wrapper').find('thead tr th:eq(3)').text('Ingreso Mensual');
        //                                     $('#tblControlEjecucion_wrapper').find('thead tr th:eq(5)').text('Gasto Acumulado');
        //                                     $('#tblControlEjecucion_wrapper').find('thead tr th:eq(6)').text('Ingreso Acumulado');
        //                                 }
        //                             });
        //                         }
        //                     }
        //                 }
        //             }
        //         },

        //         series: [{
        //             type: 'pie',
        //             name: 'Ingresos / Gastos',
        //             innerSize: '50%',
        //             // data: [ //COMENTADO
        //             //     ['Chrome', 58.9],
        //             //     ['Firefox', 13.29],
        //             //     ['Internet Explorer', 13],
        //             //     ['Edge', 3.78],
        //             //     ['Safari', 3.42],
        //             //     {
        //             //         name: 'Other',
        //             //         y: 7.61,
        //             //         dataLabels: {
        //             //             enabled: false
        //             //         }
        //             //     }
        //             // ]  //COMENTADO
        //             data: datos.serie // HABILITAR
        //         }],

        //         credits: {
        //             enabled: false
        //         }
        //     });
        // }

        function getDetalleNivel2IngresoGasto() {

            return $.post('GetDetalleNivelDosIngresoGasto',
                {
                    year: cboFiltroAnio.val(),
                    mes: cboFiltroMes.val(),
                    listaCC: getValoresMultiples('#cboFiltroCC').filter(function (x) { return parseInt(x) })
                }).then(response => {
                    if (response.success) {
                        return response;
                    } else {
                        Alert2Error(response.message);
                    }
                }, error => {
                    Alert2Error(error.message)
                });
        }

        function getDetalleNivel2PresupuestoGasto() {
            //#region SE OBTIENE LISTADO DE CP Y ARR
            let arrConstruplan = [];
            let arrArrendadora = [];
            cboFiltroCC.val().forEach(element => {
                let objCC = cboFiltroCC.find(`option[value="${element}"]`);
                let prefijoCC = objCC.attr("data-prefijo");

                if (prefijoCC == 1) {
                    if (element == "cp_46") {
                        let idCC = element.replace("cp_46", "46");
                        arrConstruplan.push(idCC);
                    } else {
                        arrConstruplan.push(element);
                    }
                } else if (prefijoCC == 2) {
                    if (element == "arr_46") {
                        let idCC = element.replace("arr_46", "46");
                        arrArrendadora.push(idCC);
                    } else {
                        arrArrendadora.push(element);
                    }
                }
            });
            //#endregion
            return $.post('GetDetalleNivelDosPresupuestoGasto',
                {
                    year: cboFiltroAnio.val(),
                    mes: cboFiltroMes.val(),
                    //listaCC: getValoresMultiples('#cboFiltroCC').filter(function (x) { return parseInt(x) })
                    arrConstruplan: arrConstruplan,
                    arrArrendadora: arrArrendadora
                }).then(response => {
                    if (response.success) {
                        return response;
                    } else {
                        Alert2Error(response.message);
                    }
                }, error => {
                    Alert2Error(error.message)
                });
        }

        function initGraficaArcoCumplimiento(datos) {
            Highcharts.chart('graficaArcoCumplimiento', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: 0,
                    plotShadow: false,
                    width: null,
                    height: 240,
                    spacingLeft: 0,
                    spacingRight: 0,
                    spacingBottom: 0,
                    spacingTop: 0,
                },

                title: {
                    text: 'Cumplimiento',
                    align: 'center',
                    verticalAlign: 'middle',
                    y: 60
                },

                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },

                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },

                plotOptions: {
                    pie: {
                        dataLabels: {
                            enabled: true,
                            distance: -50,
                            style: {
                                fontWeight: 'bold',
                                color: 'white'
                            }
                        },
                        startAngle: -90,
                        endAngle: 90,
                        center: ['50%', '75%'],
                        size: '140%',
                        point: {
                            events: {
                                click: function () {
                                    inpConsultaNivel.attr('data-cumplimientoIngreso', 3);

                                    // panelGraficas.fadeToggle('fast', 'linear', function() {
                                    //     panelDetalleNivel2.fadeToggle('fast', 'linear', function() {
                                    //         //AddRows(tblResumenNomina, response.items.detalle);
                                    //     });
                                    // });
                                    SetNombreModal();
                                    mdlDetalleNivel.modal("show");
                                    btnNivel3.css('display', 'none');
                                    btnNivel4.css('display', 'none');
                                    // btnPlanAccion.css("display", "none");
                                    panelDetalleNivel2.css('display', 'block');
                                    panelDetalleNivel3.css('display', 'none');
                                    panelDetalleNivel4.css('display', 'none');
                                    panelDetalleNivel4Ctas.css('display', 'none');
                                    //btnNivel2.css('display','block');

                                }
                            }
                        }
                    }
                },

                series: [{
                    type: 'pie',
                    name: '',
                    innerSize: '50%',
                    // data: [ // COMENTADO
                    //     ['Chrome', 58.9],
                    //     ['Firefox', 13.29],
                    //     ['Internet Explorer', 13],
                    //     ['Edge', 3.78],
                    //     ['Safari', 3.42],
                    //     {
                    //         name: 'Other',
                    //         y: 7.61,
                    //         dataLabels: {
                    //             enabled: false
                    //         }
                    //     }
                    // ] // COMENTADO
                    data: datos.serie // HABILITAR
                }],

                credits: {
                    enabled: false
                }
            });
        }

        function initGraficaArcoCumplimientoInforme(datos) {
            Highcharts.chart('graficaArcoCumplimientoInforme', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: 0,
                    plotShadow: false,
                    width: null,
                    height: 240,
                    spacingLeft: 0,
                    spacingRight: 0,
                    spacingBottom: 0,
                    spacingTop: 0,
                },

                title: {
                    text: 'Cumplimiento',
                    align: 'center',
                    verticalAlign: 'middle',
                    y: 60
                },

                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },

                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },

                plotOptions: {
                    pie: {
                        dataLabels: {
                            enabled: true,
                            distance: -50,
                            style: {
                                fontWeight: 'bold',
                                color: 'white'
                            }
                        },
                        startAngle: -90,
                        endAngle: 90,
                        center: ['50%', '75%'],
                        size: '140%',
                        point: {
                            events: {
                                click: function () {
                                    inpConsultaNivel.attr('data-cumplimientoIngreso', 3);

                                    // panelGraficas.fadeToggle('fast', 'linear', function() {
                                    //     panelDetalleNivel2.fadeToggle('fast', 'linear', function() {
                                    //         //AddRows(tblResumenNomina, response.items.detalle);
                                    //     });
                                    // });
                                    SetNombreModal();
                                    mdlDetalleNivel.modal("show");
                                    btnNivel3.css('display', 'none');
                                    btnNivel4.css('display', 'none');
                                    // btnPlanAccion.css("display", "none");
                                    panelDetalleNivel2.css('display', 'block');
                                    panelDetalleNivel3.css('display', 'none');
                                    panelDetalleNivel4.css('display', 'none');
                                    panelDetalleNivel4Ctas.css('display', 'none');
                                    //btnNivel2.css('display','block');

                                }
                            }
                        }
                    }
                },

                series: [{
                    type: 'pie',
                    name: '',
                    innerSize: '50%',
                    // data: [ // COMENTADO
                    //     ['Chrome', 58.9],
                    //     ['Firefox', 13.29],
                    //     ['Internet Explorer', 13],
                    //     ['Edge', 3.78],
                    //     ['Safari', 3.42],
                    //     {
                    //         name: 'Other',
                    //         y: 7.61,
                    //         dataLabels: {
                    //             enabled: false
                    //         }
                    //     }
                    // ] // COMENTADO
                    data: datos.serie // HABILITAR
                }],

                credits: {
                    enabled: false
                }
            });
        }

        function initGraficaArcoProyeccion(datos) {
            if (datos != undefined) {
                Highcharts.chart('graficaArcoProyeccion', {
                    height: 50,
                    lang: highChartsDicEsp,
                    title: {
                        text: ''
                    },
                    xAxis: {
                        categories: datos.lstCategorias,
                    },
                    yAxis: {
                        min: 0,
                        // max: datos.max,
                        title: { text: '' },
                        labels: { format: '{value}' }
                    },
                    tooltip: {
                        //#region TABLE
                        // formatter: function () {
                        //     let ppto = this.points[0].point.y;
                        //     let pptoReal = this.points[1].point.y;
                        //     let cumplimiento = this.points[2].point.y;

                        //     if (ppto > 0 && pptoReal > 0) {
                        //         cumplimiento = (pptoReal * 100) / ppto;
                        //     }

                        //     if (point.series.name == "CUMPLIMIENTO") {
                        //         return point.series.name += ": " + cumplimiento.toFixed(2) + "%";
                        //     }
                        // },
                        // headerFormat: `<span style="font-size:10px">{point.${datos.lstPointKey}}</span><table>`,
                        // pointFormat: `
                        //     <tr>
                        //         <td style="color:{series.color};padding:0">{series.name}: </td>
                        //         <td><b>{point.y}</b></td>
                        //     </tr>`,
                        // footerFormat: '</table>',
                        // shared: true,
                        // useHTML: true,
                        // #endregion
                        // #region TR
                        formatter: function () {
                            // let ppto = this.points[0].point.y;
                            // let pptoReal = this.points[1].point.y;
                            // let cumplimiento = this.points[2].point.y;

                            // if (ppto > 0 && pptoReal > 0) {
                            //     cumplimiento = (pptoReal * 100) / ppto;
                            // }

                            // return ['<b>' + this.x + '</b>'].concat(
                            //     this.points ?
                            //         this.points.map(function (point) {
                            //             if (point.series.name == "PRESUPUESTO CAPTURADO") {
                            //                 return point.series.name += ": " + maskNumero2DCompras(point.y);
                            //             }

                            //             if (point.series.name == "PRESUPUESTO REAL") {
                            //                 return point.series.name += ": " + maskNumero2DCompras(point.y);
                            //             }

                            //             if (point.series.name == "CUMPLIMIENTO") {
                            //                 return point.series.name += ": " + point.y.toFixed(2) + "%";
                            //             }
                            //         }) : []
                            // );
                            let ppto = this.points[0].point.y;
                            let pptoReal = this.points[1].point.y;
                            let serieName = "";
                            return `${this.points[0].point.series.name}: ${maskNumero_NoDecimal(this.points[0].point.y)} <br/> ${this.points[1].point.series.name}: ${maskNumero_NoDecimal(this.points[1].point.y)} <br/> ${this.points[2].point.series.name}: ${maskNumero_NoDecimal(this.points[2].point.y)}`;

                            //return ['<b>' + this.x + '</b>'].concat(
                            //    this.points ?
                            //        this.points.map(function (point) {
                            //            if (point.series.name == "PRESUPUESTO CAPTURADO") {
                            //                serieName = `${point.series.name}: ${maskNumero_NoDecimal(point.y)}`;
                            //            }

                            //            if (point.series.name == "GASTO") {
                            //                serieName = `${point.series.name}: ${maskNumero_NoDecimal(point.y)}`;
                            //            }
                            //            if (point.series.name == "TENDENCIA") {
                            //                serieName = `${point.series.name}: ${maskNumero_NoDecimal(point.y)}`;
                            //            }

                            //            if (point.series.name == "CUMPLIMIENTO") {
                            //                let cumplimientoPorcentaje = 0;
                            //                if (pptoReal > 0 & ppto > 0) {
                            //                    let cumplimiento = (pptoReal * 100) / ppto;
                            //                    cumplimientoPorcentaje = cumplimiento.toFixed()
                            //                }
                            //                serieName = `${point.series.name}: ${cumplimientoPorcentaje}%`;
                            //            }
                            //            return serieName;
                            //        }) : []
                            //);
                        },
                        // //#endregion
                        split: true
                    },
                    labels: {
                        items: [{
                            // html: 'Total fruit consumption',
                            style: {
                                left: '50px',
                                top: '18px',
                                color: ( // theme
                                    Highcharts.defaultOptions.title.style &&
                                    Highcharts.defaultOptions.title.style.color
                                ) || 'black'
                            }
                        }]
                    },
                    series: [
                        {
                            type: 'column',
                            name: 'PRESUPUESTO CAPTURADO',
                            data: datos.lstPpto,
                            color: "#92d050"
                        },
                        {
                            type: 'column',
                            name: '+ ADITIVAS',
                            data: datos.lstProyeccion,
                            color: "#eb9b34"
                        },
                        {
                            type: 'column',
                            name: 'GASTO',
                            data: datos.lstPptoReal,
                            marker: {
                                lineWidth: 2,
                                lineColor: Highcharts.getOptions().colors[3],
                                fillColor: 'white',
                                color: "#70ad47"
                            }
                        },
                    ],
                    credits: {
                        enabled: false
                    }
                });
            }
        }

        function initGraficaArcoProyeccionInforme(datos) {
            if (datos != undefined) {
                Highcharts.chart('graficaArcoProyeccionInforme', {
                    height: 50,
                    lang: highChartsDicEsp,
                    title: {
                        text: ''
                    },
                    xAxis: {
                        categories: datos.lstCategorias,
                    },
                    yAxis: {
                        min: 0,
                        // max: datos.max,
                        title: { text: '' },
                        labels: { format: '{value}' }
                    },
                    tooltip: {
                        //#region TABLE
                        // formatter: function () {
                        //     let ppto = this.points[0].point.y;
                        //     let pptoReal = this.points[1].point.y;
                        //     let cumplimiento = this.points[2].point.y;

                        //     if (ppto > 0 && pptoReal > 0) {
                        //         cumplimiento = (pptoReal * 100) / ppto;
                        //     }

                        //     if (point.series.name == "CUMPLIMIENTO") {
                        //         return point.series.name += ": " + cumplimiento.toFixed(2) + "%";
                        //     }
                        // },
                        // headerFormat: `<span style="font-size:10px">{point.${datos.lstPointKey}}</span><table>`,
                        // pointFormat: `
                        //     <tr>
                        //         <td style="color:{series.color};padding:0">{series.name}: </td>
                        //         <td><b>{point.y}</b></td>
                        //     </tr>`,
                        // footerFormat: '</table>',
                        // shared: true,
                        // useHTML: true,
                        // #endregion
                        // #region TR
                        formatter: function () {
                            // let ppto = this.points[0].point.y;
                            // let pptoReal = this.points[1].point.y;
                            // let cumplimiento = this.points[2].point.y;

                            // if (ppto > 0 && pptoReal > 0) {
                            //     cumplimiento = (pptoReal * 100) / ppto;
                            // }

                            // return ['<b>' + this.x + '</b>'].concat(
                            //     this.points ?
                            //         this.points.map(function (point) {
                            //             if (point.series.name == "PRESUPUESTO CAPTURADO") {
                            //                 return point.series.name += ": " + maskNumero2DCompras(point.y);
                            //             }

                            //             if (point.series.name == "PRESUPUESTO REAL") {
                            //                 return point.series.name += ": " + maskNumero2DCompras(point.y);
                            //             }

                            //             if (point.series.name == "CUMPLIMIENTO") {
                            //                 return point.series.name += ": " + point.y.toFixed(2) + "%";
                            //             }
                            //         }) : []
                            // );
                            let ppto = this.points[0].point.y;
                            let pptoReal = this.points[1].point.y;
                            let serieName = "";
                            return `${this.points[0].point.series.name}: ${maskNumero_NoDecimal(this.points[0].point.y)} <br/> ${this.points[1].point.series.name}: ${maskNumero_NoDecimal(this.points[1].point.y)} <br/> ${this.points[2].point.series.name}: ${maskNumero_NoDecimal(this.points[2].point.y)}`;

                            //return ['<b>' + this.x + '</b>'].concat(
                            //    this.points ?
                            //        this.points.map(function (point) {
                            //            if (point.series.name == "PRESUPUESTO CAPTURADO") {
                            //                serieName = `${point.series.name}: ${maskNumero_NoDecimal(point.y)}`;
                            //            }

                            //            if (point.series.name == "GASTO") {
                            //                serieName = `${point.series.name}: ${maskNumero_NoDecimal(point.y)}`;
                            //            }
                            //            if (point.series.name == "TENDENCIA") {
                            //                serieName = `${point.series.name}: ${maskNumero_NoDecimal(point.y)}`;
                            //            }

                            //            if (point.series.name == "CUMPLIMIENTO") {
                            //                let cumplimientoPorcentaje = 0;
                            //                if (pptoReal > 0 & ppto > 0) {
                            //                    let cumplimiento = (pptoReal * 100) / ppto;
                            //                    cumplimientoPorcentaje = cumplimiento.toFixed()
                            //                }
                            //                serieName = `${point.series.name}: ${cumplimientoPorcentaje}%`;
                            //            }
                            //            return serieName;
                            //        }) : []
                            //);
                        },
                        // //#endregion
                        split: true
                    },
                    labels: {
                        items: [{
                            // html: 'Total fruit consumption',
                            style: {
                                left: '50px',
                                top: '18px',
                                color: ( // theme
                                    Highcharts.defaultOptions.title.style &&
                                    Highcharts.defaultOptions.title.style.color
                                ) || 'black'
                            }
                        }]
                    },
                    series: [
                        {
                            type: 'column',
                            name: 'PRESUPUESTO CAPTURADO',
                            data: datos.lstPpto,
                            color: "#92d050"
                        },
                        {
                            type: 'column',
                            name: '+ ADITIVAS',
                            data: datos.lstProyeccion,
                            color: "#eb9b34"
                        },
                        {
                            type: 'column',
                            name: 'GASTO',
                            data: datos.lstPptoReal,
                            marker: {
                                lineWidth: 2,
                                lineColor: Highcharts.getOptions().colors[3],
                                fillColor: 'white',
                                color: "#70ad47"
                            }
                        },
                    ],
                    credits: {
                        enabled: false
                    }
                });
            }
        }

        function SetNombreModal() {
            $("#mdlDetalleNivel .modal-title").text(`DETALLE PRESUPUESTO - ${$('select[id="cboFiltroMes"] option:selected').text()}`);
        }

        //#region GRAFICAS LINDXS (✿◡‿◡) 😊

        function initGraficaControlEjecucion(datos) {
            const chartControlEjecucion = Highcharts.chart('graficaControlEjecucion', {
                chart: {
                    type: 'column',
                    height: 150,
                },
                title: {
                    text: null
                },
                exporting: {
                    enabled: false
                },
                subtitle: {
                },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: ''
                    }
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'top',
                    layout: 'vertical',
                    x: 0,
                    y: 50
                },
                tooltip: {
                    formatter: function () {

                        let gasto = this.points[0].point.y;
                        let ppto = this.points[1].point.y;
                        let serieName = "";
                        return ['<b>' + this.x + '</b>'].concat(
                            this.points ?
                                this.points.map(function (point) {
                                    if (point.series.name == "CUMPLIMIENTO") {
                                        let cumplimientoPorcentaje = 0;
                                        if (ppto > 0) {
                                            let cumplimiento = (gasto * 100) / ppto;
                                            cumplimientoPorcentaje = cumplimiento.toFixed()
                                        }
                                        serieName = `${point.series.name}: ${maskNumeroPorcentaje(cumplimientoPorcentaje)}`;
                                    }
                                    else { serieName = `${point.series.name}: ${maskNumero_NoDecimal(point.y)}`; }
                                    return serieName;
                                }) : []
                        );
                    },
                    split: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    },
                    series: {
                        pointWidth: 14
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1, color: 'green' },
                    { name: datos.serie2Descripcion, data: datos.serie2, color: 'gray' },
                    { type: 'line', name: datos.serie3Descripcion, data: datos.serie3, color: 'black' }
                ],

                credits: {
                    enabled: false
                }
            });

            chartControlEjecucion.reflow();
        }

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }
        //#endregion

        //#region gfxPresupuestoGastoMensual
        function initGraficaArcoPresupuestoMensual(datos, porcentaje) {
            Highcharts.chart('graficaArcoPresupuestoMensual', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: 0,
                    plotShadow: false,
                    width: null,
                    height: 240,
                    spacingLeft: 0,
                    spacingRight: 0,
                    spacingBottom: 0,
                    spacingTop: 0
                },
                title: {
                    text: maskNumeroPorcentaje(porcentaje),
                    align: 'center',
                    verticalAlign: 'middle',
                    y: 60
                },
                tooltip: {
                    enabled: false,
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            enabled: true,
                            distance: -50,
                            style: {
                                fontWeight: 'bold',
                                color: 'white'
                            },
                            // format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            connectorColor: 'silver',
                            formatter: function () {
                                if (this.point.name == 'PRESUPUESTO') {
                                    return 'PPTO' + ' 100%';
                                }
                                if (this.point.name == 'GASTO' && this.point.index == 1) {
                                    return 'GASTO' + ' ' + maskNumeroPorcentaje(100 + ((this.point.y * 100) / 66.6));
                                }
                                if (this.point.name == 'GASTO' && this.point.index == 0) {
                                    return 'GASTO' + ' ' + maskNumeroPorcentaje(((this.point.y * 100) / 66.6));
                                }
                                return ''
                            }
                        },
                        startAngle: -90,
                        endAngle: 90,
                        center: ['50%', '75%'],
                        size: '140%',
                        point: {
                            events: {
                                click: function () {
                                    getDetalleNivel2PresupuestoGasto().done(function (response) {
                                        inpConsultaNivel.attr('data-cumplimientoIngreso', 1);

                                        if (response && response.success) {
                                            if (inpConsultaNivel.attr('data-cumplimientoIngreso') == 2) {
                                                GetDetalleNivelTresIngresoGasto(cboFiltroCC.val());

                                            } else {
                                                //fncGetSumaCapturas(cboFiltroCC.val())
                                                fncGetSumaCapturas(response.lstSumaCapturasGastos);
                                            }
                                            txtCumplimientoControlEjecucion.text(`${Math.trunc(response.items.cumplimientoAcumuladoEmpresa)}%`);
                                            if (response.items.cumplimientoAcumuladoEmpresa >= 97) {
                                                txtCumplimientoControlEjecucion.css('color', 'red');
                                            } else if (response.items.cumplimientoAcumuladoEmpresa >= 95) {
                                                txtCumplimientoControlEjecucion.css('color', 'yellow');
                                            } else {
                                                txtCumplimientoControlEjecucion.css('color', 'green');
                                            }
                                            txtMesCierreControlEjecucion.text(response.items.mesCierre);
                                            initGraficaControlEjecucion(response.items.graficaBarraMensual); //TODO
                                            btnCerrarModal.html(`<i class="fa fa-arrow-left"></i>&nbsp;Cerrar`);
                                            periodoResumen.text('DETALLE PRESUPUESTO / GASTOS')
                                            // panelGraficas.fadeToggle('fast', 'linear', function() {
                                            //     panelDetalleNivel2.fadeToggle('fast', 'linear', function() {
                                            //         addRows(tblControlEjecucion, response.items.datosTablaCC);
                                            //     });
                                            // });
                                            SetNombreModal();
                                            mdlDetalleNivel.modal("show");
                                            panelGeneral0.css('display', 'block');
                                            panelDetalleNivel2.css('display', 'block');
                                            panelDetalleNivel3.css('display', 'none');
                                            panelDetalleNivel4.css('display', 'none');
                                            panelDetalleNivel4Ctas.css('display', 'none');
                                            btnNivel3.css('display', 'none');
                                            btnNivel4.css('display', 'none');
                                            // btnPlanAccion.css("display", "none");
                                            $('.panel-detalle-total').css('display', 'block');
                                            //btnNivel2.css('display','block');

                                            addRows(tblControlEjecucion, response.items.datosTablaCC);

                                            tblControlEjecucion.DataTable().columns(1).visible(true);

                                            tblControlEjecucion.find('thead tr th:eq(2)').text('Presupuesto Mensual');
                                            tblControlEjecucion.find('thead tr th:eq(3)').text('Gasto Mensual');
                                            tblControlEjecucion.find('thead tr th:eq(4)').text('Diferencia');
                                            tblControlEjecucion.find('thead tr th:eq(5)').text('Cumplimiento');
                                            tblControlEjecucion.find('thead tr th:eq(6)').text('Pto Acumulado');
                                            tblControlEjecucion.find('thead tr th:eq(7)').text('Gasto Acumulado');
                                            tblControlEjecucion.find('thead tr th:eq(8)').text('Diferencia');
                                            tblControlEjecucion.find('thead tr th:eq(9)').text('Cumplimiento');
                                        }
                                    });
                                }
                            }
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: '',
                    innerSize: '50%',
                    // data: [
                    //     ['GASTO', 33.32],
                    //     ['PRESUPUESTO', 33.3],
                    //     ['Firefox', 33.3]
                    // ]
                    data: datos.serie
                }],

                credits: {
                    enabled: false
                }
            });
        }
        function initGraficaArcoPresupuestoMensualInforme(datos, porcentaje) {
            Highcharts.chart('graficaArcoPresupuestoMensualInforme', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: 0,
                    plotShadow: false,
                    width: null,
                    height: 240,
                    spacingLeft: 0,
                    spacingRight: 0,
                    spacingBottom: 0,
                    spacingTop: 0
                },
                title: {
                    text: maskNumeroPorcentaje(porcentaje),
                    align: 'center',
                    verticalAlign: 'middle',
                    y: 60
                },
                tooltip: {
                    enabled: false,
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            enabled: true,
                            distance: -50,
                            style: {
                                fontWeight: 'bold',
                                color: 'white'
                            },
                            // format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            connectorColor: 'silver',
                            formatter: function () {
                                if (this.point.name == 'PRESUPUESTO') {
                                    return 'PPTO' + ' 100%';
                                }
                                if (this.point.name == 'GASTO' && this.point.index == 1) {
                                    return 'GASTO' + ' ' + maskNumeroPorcentaje(100 + ((this.point.y * 100) / 66.6));
                                }
                                if (this.point.name == 'GASTO' && this.point.index == 0) {
                                    return 'GASTO' + ' ' + maskNumeroPorcentaje(((this.point.y * 100) / 66.6));
                                }
                                return ''
                            }
                        },
                        startAngle: -90,
                        endAngle: 90,
                        center: ['50%', '75%'],
                        size: '140%',
                        point: {
                            events: {
                                click: function () {
                                    getDetalleNivel2PresupuestoGasto().done(function (response) {
                                        inpConsultaNivel.attr('data-cumplimientoIngreso', 1);

                                        if (response && response.success) {
                                            if (inpConsultaNivel.attr('data-cumplimientoIngreso') == 2) {
                                                GetDetalleNivelTresIngresoGasto(cboFiltroCC.val());

                                            } else {
                                                //fncGetSumaCapturas(cboFiltroCC.val())
                                                fncGetSumaCapturas(response.lstSumaCapturasGastos);
                                            }
                                            txtCumplimientoControlEjecucion.text(`${Math.trunc(response.items.cumplimientoAcumuladoEmpresa)}%`);
                                            if (response.items.cumplimientoAcumuladoEmpresa >= 97) {
                                                txtCumplimientoControlEjecucion.css('color', 'red');
                                            } else if (response.items.cumplimientoAcumuladoEmpresa >= 95) {
                                                txtCumplimientoControlEjecucion.css('color', 'yellow');
                                            } else {
                                                txtCumplimientoControlEjecucion.css('color', 'green');
                                            }
                                            txtMesCierreControlEjecucion.text(response.items.mesCierre);
                                            initGraficaControlEjecucion(response.items.graficaBarraMensual); //TODO
                                            btnCerrarModal.html(`<i class="fa fa-arrow-left"></i>&nbsp;Cerrar`);
                                            periodoResumen.text('DETALLE PRESUPUESTO / GASTOS')
                                            // panelGraficas.fadeToggle('fast', 'linear', function() {
                                            //     panelDetalleNivel2.fadeToggle('fast', 'linear', function() {
                                            //         addRows(tblControlEjecucion, response.items.datosTablaCC);
                                            //     });
                                            // });
                                            SetNombreModal();
                                            mdlDetalleNivel.modal("show");
                                            panelGeneral0.css('display', 'block');
                                            panelDetalleNivel2.css('display', 'block');
                                            panelDetalleNivel3.css('display', 'none');
                                            panelDetalleNivel4.css('display', 'none');
                                            panelDetalleNivel4Ctas.css('display', 'none');
                                            btnNivel3.css('display', 'none');
                                            btnNivel4.css('display', 'none');
                                            // btnPlanAccion.css("display", "none");
                                            $('.panel-detalle-total').css('display', 'block');
                                            //btnNivel2.css('display','block');

                                            addRows(tblControlEjecucion, response.items.datosTablaCC);

                                            tblControlEjecucion.DataTable().columns(1).visible(true);

                                            tblControlEjecucion.find('thead tr th:eq(2)').text('Presupuesto Mensual');
                                            tblControlEjecucion.find('thead tr th:eq(3)').text('Gasto Mensual');
                                            tblControlEjecucion.find('thead tr th:eq(4)').text('Diferencia');
                                            tblControlEjecucion.find('thead tr th:eq(5)').text('Cumplimiento');
                                            tblControlEjecucion.find('thead tr th:eq(6)').text('Pto Acumulado');
                                            tblControlEjecucion.find('thead tr th:eq(7)').text('Gasto Acumulado');
                                            tblControlEjecucion.find('thead tr th:eq(8)').text('Diferencia');
                                            tblControlEjecucion.find('thead tr th:eq(9)').text('Cumplimiento');
                                        }
                                    });
                                }
                            }
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: '',
                    innerSize: '50%',
                    // data: [
                    //     ['GASTO', 33.32],
                    //     ['PRESUPUESTO', 33.3],
                    //     ['Firefox', 33.3]
                    // ]
                    data: datos.serie
                }],

                credits: {
                    enabled: false
                }
            });
        }
        //#endregion

        function initTblGastosVsPresupuestos() {
            tblGastosVsPresupuestos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'descripcion', title: 'Descripción', className: 'dt-center getDetalle',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.esAgrupador) {
                                return data;
                            } else {
                                return `<a class="linkCC" >${data}</a>`;
                            }
                        }
                    },
                    {
                        data: 'presupuestoMensual', title: 'Presupuesto Mensual', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'gastoMensual', title: 'Gasto Mensual', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'cumplimientoMensual', title: 'Cumplimiento Mensual', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            let flechaColor = '';
                            let flechaDireccion = '';
                            let cumplimientoMensual = "";

                            if (parseFloat(data) < 97) {
                                flechaColor = 'greenArrow';
                                flechaDireccion = 'down';
                                cumplimientoMensual = `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                            }
                            if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                flechaColor = 'yellowArrow';
                                flechaDireccion = 'right';
                                cumplimientoMensual = `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                            }
                            if (parseFloat(data) > 100) {
                                flechaColor = 'redArrow';
                                flechaDireccion = 'up';

                                if (row.esAgrupador) {
                                    return `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                                } else {
                                    return `<a class="planAccion"><i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span></a>`;
                                }
                            }
                            return cumplimientoMensual;
                        }
                    },
                    {
                        data: 'presupuestoAcumulado', title: 'Presupuesto Acumulado', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'gastoAcumulado', title: 'Gasto Acumulado', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.esAgrupador) {
                                return maskNumero_NoDecimal(data);
                            } else {
                                return `<a class="linkCC_GastoAcumulado" >${maskNumero_NoDecimal(data)}</a>`;
                            }
                            // return maskNumero_NoDecimal(data);
                        }
                    },
                    {
                        data: 'cumplimientoAcumulado', title: 'Cumplimiento Acumulado', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            let flechaColor = '';
                            let flechaDireccion = '';

                            if (parseFloat(data) < 97) {
                                flechaColor = 'greenArrow';
                                flechaDireccion = 'down';
                            }

                            if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                flechaColor = 'yellowArrow';
                                flechaDireccion = 'right';
                            }

                            if (parseFloat(data) > 100) {
                                flechaColor = 'redArrow';
                                flechaDireccion = 'up';
                            }

                            return `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                        }
                    },
                    { data: 'esAgrupador', title: 'esAgrupador', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblGastosVsPresupuestos.on("click", ".linkCC", function () {
                        let rowData = tblGastosVsPresupuestos.DataTable().row($(this).closest('tr')).data();

                        panelDetalleNivel3.css('display', 'none');
                        panelDetalleNivel4Ctas.css('display', 'block');

                        btnNivel4.css('display', 'initial');
                        btnNivel4.text(rowData.descripcion);

                        btnCerrarModal.html(`<i class="fa fa-arrow-left"></i>&nbsp;Regresar`);
                        fncGetCtasPolizas(rowData.idCC, rowData.idConcepto, rowData.empresa, true);
                    });

                    tblGastosVsPresupuestos.on("click", ".linkCC_GastoAcumulado", function () {
                        let rowData = tblGastosVsPresupuestos.DataTable().row($(this).closest('tr')).data();

                        panelDetalleNivel3.css('display', 'none');
                        panelDetalleNivel4Ctas.css('display', 'block');

                        btnNivel4.css('display', 'initial');
                        btnNivel4.text(rowData.descripcion);

                        btnCerrarModal.html(`<i class="fa fa-arrow-left"></i>&nbsp;Regresar`);
                        fncGetCtasPolizas(rowData.idCC, rowData.idConcepto, rowData.empresa, false);
                    });

                    tblGastosVsPresupuestos.on("click", ".planAccion", function () {
                        let rowData = tblGastosVsPresupuestos.DataTable().row($(this).closest('tr')).data();
                        fncGetPlanAccion(rowData.idCC, cboFiltroAnio.val(), rowData.idConcepto, 0);
                    });
                },
                columnDefs: [
                ],
                headerCallback: function (thead, data, start, end, display) {
                },
                footerCallback: function (row, data, start, end, display) {
                    if (data.length > 0) {
                        let lstPptos = data.filter(function (x) {
                            return x.esAgrupador == true;
                        });

                        let pptoMen = 0;
                        let pptoGasto = 0;
                        let pptoAcumulado = 0;
                        let pptoGastoAcumulado = 0;

                        lstPptos.forEach(x => pptoMen += x.presupuestoMensual);
                        lstPptos.forEach(x => pptoGasto += x.gastoMensual);
                        lstPptos.forEach(x => pptoAcumulado += x.presupuestoAcumulado);
                        lstPptos.forEach(x => pptoGastoAcumulado += x.gastoAcumulado);

                        //#region CUMPLIMIENTO MENSUAL
                        let cumplimientoMensual = 0;
                        let cumplimientoMensualPorcentaje = 0;
                        let cumplimientoMensualCantidad = 0;
                        let flechaColor = '';
                        let flechaDireccion = '';
                        data.filter(function (x) {
                            if (x.esAgrupador) {
                                cumplimientoMensual += x.cumplimientoMensual;
                                cumplimientoMensualCantidad++;
                            }
                        });
                        //cumplimientoMensualPorcentaje = cumplimientoMensual / cumplimientoMensualCantidad;
                        cumplimientoMensualPorcentaje = (pptoGasto * 100) / (pptoMen != 0 ? pptoMen : 1);

                        if (parseFloat(data) < 97) { flechaColor = 'greenArrow'; flechaDireccion = 'down'; }
                        if (parseFloat(data) >= 97 && parseFloat(data) <= 100) { flechaColor = 'yellowArrow'; flechaDireccion = 'right'; }
                        if (parseFloat(data) > 100) { flechaColor = 'redArrow'; flechaDireccion = 'up'; }
                        $(row).find('th').eq(3).html(`<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(cumplimientoMensualPorcentaje)}%</span>`);
                        //#endregion

                        //#region CUMPLIMIENTO MENSUAL ACUMULADO
                        let cumplimientoAcumulado = 0;
                        let cumplimientoAcumuladoPorcentaje = 0;
                        let cumplimientoAcumuladoCantidad = 0;
                        flechaColor = '';
                        flechaDireccion = '';
                        data.filter(function (x) {
                            if (x.esAgrupador) {
                                cumplimientoAcumulado += x.cumplimientoAcumulado;
                                cumplimientoAcumuladoCantidad++;
                            }
                        });
                        //cumplimientoAcumuladoPorcentaje = cumplimientoAcumulado / cumplimientoAcumuladoCantidad;
                        cumplimientoAcumuladoPorcentaje = (pptoGastoAcumulado * 100) / (pptoAcumulado != 0 ? pptoAcumulado : 1);

                        if (parseFloat(data) < 97) { flechaColor = 'greenArrow'; flechaDireccion = 'down'; }
                        if (parseFloat(data) >= 97 && parseFloat(data) <= 100) { flechaColor = 'yellowArrow'; flechaDireccion = 'right'; }
                        if (parseFloat(data) > 100) { flechaColor = 'redArrow'; flechaDireccion = 'up'; }
                        let cumplimientoPrint = maskNumero_NoDecimal(cumplimientoAcumuladoPorcentaje);
                        let cumplimientoPrint2 = cumplimientoPrint.replace("$", "");
                        $(row).find('th').eq(6).html(`<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${cumplimientoPrint2}%</span>`);
                        //#endregion

                        $(row).find('th').eq(1).html(maskNumero_NoDecimal(pptoMen));
                        $(row).find('th').eq(2).html(maskNumero_NoDecimal(pptoGasto));
                        $(row).find('th').eq(4).html(maskNumero_NoDecimal(pptoAcumulado));
                        $(row).find('th').eq(5).html(maskNumero_NoDecimal(pptoGastoAcumulado));
                    }
                }
            });
        }

        //#region PLAN DE ACCIÓN
        function fncGetPlanAccion(idCC, anio, idConcepto, idPlanAccion) {
            let obj = fncOBJPlanAccion(idCC, anio, idConcepto, idPlanAccion);
            if (obj != "") {
                axios.post("GetPlanAccion", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncLimpiarMdlPlanAccion();

                        // NO PUEDE DAR VOBO
                        // btnVoBo.attr("disabled", true);
                        // btnVoBo.css("display", "none");

                        if (response.data.objPlanAccion != null) {
                            // ACTUALIZAR PLAN DE ACCIÓN
                            txtCEPlanAccion.val(response.data.objPlanAccion.planAccion);
                            txtCEPlanAccionJustificacion.val(response.data.objPlanAccion.justificacion);
                            txtCEPlanAccionCorreoResponsableSeguimiento.val(response.data.objPlanAccion.correoResponsableSeguimiento);
                            txtCEPlanAccionFechaCompromiso.val(moment(response.data.objPlanAccion.fechaCompromiso).format("YYYY-MM-DD"));
                            btnCEEstatusPlanAccion.html(response.data.objPlanAccion.estatus);
                            btnCEEstatusPlanAccion.css("background-color", response.data.objPlanAccion.backgroundColor);
                            btnCEEstatusPlanAccion.css("color", response.data.objPlanAccion.color);
                            btnCEPlanAccion.attr("data-id", response.data.objPlanAccion.id);
                            lblBtnCEPlanAccion.html("Actualizar");

                            //#region SI EL ESTATUS ES == 4, EL PLAN DE ACCIÓN YA SE ENCUENTRA CERRADO.
                            if (response.data.objPlanAccion.idEstatusPlanAccion == 4) {
                                txtCEPlanAccion.attr("disabled", true);
                                txtCEPlanAccionJustificacion.attr("disabled", true);
                                txtCEPlanAccionCorreoResponsableSeguimiento.attr("disabled", true);
                                txtCEPlanAccionFechaCompromiso.attr("disabled", true);
                                btnCEEstatusPlanAccion.attr("disabled", true);
                                btnCEPlanAccion.attr("disabled", true);
                                btnCerrarPlanAccion.attr("disabled", true);
                            } else {
                                txtCEPlanAccion.attr("disabled", false);
                                txtCEPlanAccionJustificacion.attr("disabled", false);
                                txtCEPlanAccionCorreoResponsableSeguimiento.attr("disabled", false);
                                txtCEPlanAccionFechaCompromiso.attr("disabled", false);
                                btnCEEstatusPlanAccion.attr("disabled", false);
                                btnCEPlanAccion.attr("disabled", false);
                                btnCerrarPlanAccion.attr("disabled", false);
                                btnCerrarPlanAccion.css("display", "inline");
                            }
                            //#endregion

                            //#region SE VERIFICA SI EL USUARIO PUEDE DAR VOBO
                            // if (response.data.objPlanAccion.id > 0) {
                            //     if (response.data.objPlanAccion.usuarioVoBo) {
                            //         //#region PUEDE DAR VOBO
                            //         // btnVoBo.attr("disabled", false);
                            //         // btnVoBo.css("display", "inline");
                            //         //#endregion
                            //     } else {
                            //         //#region NO PUEDE DAR VOBO
                            //         // btnVoBo.attr("disabled", true);
                            //         // btnVoBo.css("display", "none");
                            //         //#endregion
                            //     }
                            // } else {
                            //     // btnVoBo.attr("disabled", true);
                            //     // btnVoBo.css("display", "none");
                            // }
                            //#endregion
                        } else {
                            // NUEVO PLAN DE ACCIÓN
                            txtCEPlanAccionJustificacion.attr("disabled", true);
                            btnCerrarPlanAccion.css("display", "none");
                            btnCEPlanAccion.attr("data-id", 0);
                            lblBtnCEPlanAccion.html("Guardar");
                            btnCEPlanAccion.attr("disabled", false);

                            btnCEEstatusPlanAccion.html("ABIERTO");
                            btnCEEstatusPlanAccion.css("background-color", "#0000ff");
                            btnCEEstatusPlanAccion.css("color", "#fff");
                        }

                        btnCEPlanAccion.attr("data-anio", anio);
                        btnCEPlanAccion.attr("data-idCC", idCC);
                        btnCEPlanAccion.attr("data-idConcepto", idConcepto);
                        mdlPlanAccion.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncOBJPlanAccion(idCC, anio, idConcepto, idPlanAccion) {
            let strMensajeError = "";
            anio <= 0 ? strMensajeError += "Es necesario seleccionar un año en los filtros." : "";
            idCC <= 0 ? strMensajeError += "<br>Ocurrió un error al obtener el CC." : "";
            idConcepto > 0 || idConcepto == -1 ? "" : strMensajeError += "<br>Ocurrió un error al obtener el concepto.";
            cboFiltroMes.val() <= 0 ? strMensajeError += "Es necesario seleccionar un mes en los filtros." : "";
            if (idPlanAccion > 0) { strMensajeError = ""; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj.idCC = idCC;
                obj.anio = anio;
                obj.idConcepto = idConcepto;
                obj.idMes = cboFiltroMes.val();
                obj.idPlanAccion = idPlanAccion;
                return obj;
            }
        }

        function fncCEPlanAccion() {
            let obj = fncOBJCEPlanAccion();
            axios.post("CEPlanAccion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito(response.data.message);
                    mdlPlanAccion.modal("hide");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncOBJCEPlanAccion() {
            let strMensajeError = "";
            btnCEPlanAccion.attr("data-anio") <= 0 ? strMensajeError += "Es necesario seleccionar un año en los filtros." : "";
            btnCEPlanAccion.attr("data-idCC") <= 0 ? strMensajeError += "<br>Ocurrió un error al obtener el CC." : "";
            btnCEPlanAccion.attr("data-idConcepto") > 0 || btnCEPlanAccion.attr("data-idConcepto") == -1 ? "" : strMensajeError += "<br>Ocurrió un error al obtener el concepto.";
            if (cboFiltroAnio.val() <= 0) { strMensajeError += "<br>Es necesario seleccionar un mes en los filtros." } else { ""; }
            if (txtCEPlanAccion == "") { txtCEPlanAccion.css("border", "2px solid red"); strMensajeError += "<br>Es necesario llenar los campos obligatorios."; }
            if (txtCEPlanAccionCorreoResponsableSeguimiento == "") { txtCEPlanAccionCorreoResponsableSeguimiento.css("border", "2px solid red"); strMensajeError += "<br>Es necesario llenar los campos obligatorios."; }
            if (txtCEPlanAccionFechaCompromiso == "") { txtCEPlanAccionFechaCompromiso.css("border", "2px solid red"); strMensajeError += "<br>Es necesario llenar los campos obligatorios."; }

            if (btnCEPlanAccion.attr("data-id") > 0) {
                strMensajeError = "";
            }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj.id = btnCEPlanAccion.attr("data-id");
                obj.anio = btnCEPlanAccion.attr("data-anio");
                obj.idCC = btnCEPlanAccion.attr("data-idCC");
                obj.idConcepto = btnCEPlanAccion.attr("data-idConcepto");
                obj.idMes = cboFiltroMes.val();
                obj.planAccion = txtCEPlanAccion.val();
                obj.justificacion = txtCEPlanAccionJustificacion.val();
                obj.fechaCompromiso = txtCEPlanAccionFechaCompromiso.val();
                obj.correoResponsableSeguimiento = txtCEPlanAccionCorreoResponsableSeguimiento.val();
                return obj;
            }
        }

        function fncLimpiarMdlPlanAccion() {
            txtCEPlanAccion.val("");
            txtCEPlanAccionJustificacion.val("");
            txtCEPlanAccionCorreoResponsableSeguimiento.val("");
            txtCEPlanAccionFechaCompromiso.val("");
            btnCEEstatusPlanAccion.html("ABIERTO");

            txtCEPlanAccion.attr("disabled", false);
            txtCEPlanAccionJustificacion.attr("disabled", false);
            txtCEPlanAccionCorreoResponsableSeguimiento.attr("disabled", false);
            txtCEPlanAccionFechaCompromiso.attr("disabled", false);
            btnCEEstatusPlanAccion.attr("disabled", false);
        }

        function fncCerrarPlanAccion() {
            let obj = fncOBJCerrarPlanAccion();
            if (obj != "") {
                btnCEPlanAccion.trigger("click")
                axios.post("CerrarPlanAccion", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        mdlPlanAccion.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncOBJCerrarPlanAccion() {
            let strMensajeError = "";
            txtCEPlanAccionJustificacion.val() == "" ? strMensajeError += "Es necesario indicar la justificación (Resultado obtenido)." : "";

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj.id = btnCEPlanAccion.attr("data-id");
                obj.justificacion = txtCEPlanAccionJustificacion.val();
                return obj;
            }
        }

        function fncVerificarExistenciaPlanAccion() {
            let obj = fncOBJVerificarExistenciaPlanAccion();
            if (obj != "") {
                axios.post("VerificarExistenciaPlanAccion", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        // cantPlanAccion = response.data.cantPlanAccion;
                        // if (cantPlanAccion > 0) {
                        //     btnFiltroReportePlanAccion.css("display", "inline");
                        // } else {
                        //     btnFiltroReportePlanAccion.css("display", "none");
                        // }
                        btnFiltroReportePlanAccion.css("display", "inline");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncOBJVerificarExistenciaPlanAccion() {
            let strMensajeError = "";
            cboFiltroCC.val() <= 0 ? strMensajeError += "Es necesario seleccionar un CC." : "";

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj.listaCC = getValoresMultiples('#cboFiltroCC').filter(function (x) { return parseInt(x) })
                return obj;
            }
        }

        function fncIndicarVobo() {
            let idPlanAccion = btnCEPlanAccion.attr("data-id");
            if (idPlanAccion <= 0) {
                Alert2Warning("Ocurrió un error al indicar el visto bueno.");
            } else {
                let obj = new Object();
                obj.idPlanAccion = idPlanAccion;
                axios.post("IndicarVobo", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        mdlPlanAccion.modal("hide");
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }
        //#endregion

        //#region REPORTE PLAN DE ACCIÓN
        function fncGetReportePlanAcciones() {
            let strMensajeError = "";
            let anio = cboFiltroAnio.val() <= 0 ? strMensajeError += "Es necesario seleccionar un año." : cboFiltroAnio.val();
            let idCC = cboFiltroCC.val() > 0 ? getValoresMultiples('#cboFiltroCC').filter(function (x) { return parseInt(x) }) : "<br>Es necesario seleccionar solamente un CC.";

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
            } else {
                let obj = new Object();
                obj.anio = anio;
                obj.lstCC = idCC;
                axios.post("GetReportePlanAcciones", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //divReportePlanesAccionesHTML.html(response.data.html);
                        divReportePlanesAccionesHTMLInforme.html(response.data.html)
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetReportePlanAccionesByCC(idCC) {
            let strMensajeError = "";
            let anio = cboFiltroAnio.val() <= 0 ? strMensajeError += "Es necesario seleccionar un año." : cboFiltroAnio.val();
            //let idCC = cboFiltroCC.val() > 0 ? getValoresMultiples('#cboFiltroCC').filter(function (x) { return parseInt(x) }) : "<br>Es necesario seleccionar solamente un CC.";

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
            } else {
                let obj = new Object();
                obj.anio = anio;
                obj.lstCC = idCC;
                axios.post("GetReportePlanAcciones", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //divReportePlanesAccionesHTML.html(response.data.html)
                        divReportePlanesAccionesHTMLInforme.html(response.data.html)
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGenerarVariablesSessionReportePlanAccion() {
            //#region VARIABLES DE SESIÓN
            let obj = new Object();
            obj = {
                graficaCumplimientoPresupuestoAcumulado: imgGraficaCumplimientoPresupuestoAcumulado,
                graficaCumplimientoPresupuestoMensual: imgGraficaCumplimientoPresupuestoMensual,
                graficaProyeccion: imgGraficaProyeccion,
                acumuladoGasto: montoGastoInforme.text(),
                acumuladoIngreso: montoIngresoInforme.text(),
                acumuladoObjetivo: txtRatioGastoVsIngresoAcumuladoInforme.text(),
                acumuladoReal: montoRealInforme.text(),
                acumuladoCumplimiento: porcentajeCumplimientoIngresoGastoInforme.text(),
                mensualGasto: montoGastoMensualInforme.text(),
                mensualIngreso: montoIngresoMensualInforme.text(),
                mensualObjetivo: txtRatioGastoVsIngresoMensualInforme.text(),
                mensualReal: montoRealMensualInforme.text(),
                mensualCumplimiento: porcentajeCumplimientoIngresoGastoMensualInforme.text(),
                anio: cboFiltroAnio.val(),
                idMes: cboFiltroMes.val(),
                idEmpresa: cboFiltroEmpresas.val()
            }
            axios.post("GenerarReportePlanAccion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let cc = botonDescargarReportePlanAccion.attr('cc');
                    report.attr("src", `/Reportes/Vista.aspx?idReporte=255&idCC=${cc}&inMemory=1&idMes=${cboFiltroMes.val()}&anio=${cboFiltroAnio.val()}&idEmpresa=${cboFiltroEmpresas.val()}`);
                    mdlReportePlanAccion.modal("hide");
                    $("#btnCrvReporteEstandarCerrar").trigger("click");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
            //#endregion
        }
        //#endregion

        function fncGetCtasPolizas(idCC, idConcepto, empresa, esConsultaMensual) {
            let obj = new Object();
            obj = {
                idCC: idCC,
                anio: cboFiltroAnio.val(),
                idMes: cboFiltroMes.val(),
                idConcepto: idConcepto,
                empresa: empresa,
                costosAdministrativos: btnCostosTotales.is(":checked"),
                esConsultaMensual: esConsultaMensual
            }
            axios.post("GetCtasPolizas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtNivel4Ctas.clear();
                    dtNivel4Ctas.rows.add(response.data.lstGastos);
                    dtNivel4Ctas.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#region CREACIÓN DE REPORTE CON GRAFICA TOMADA COMO IMAGEN
        EXPORT_WIDTH = 1000;
        function fncGenerarReporteCrystalReport(chart, filename, numGrafica) {
            var render_width = EXPORT_WIDTH;
            var render_height = render_width * chart.chartHeight / chart.chartWidth

            var svg = chart.getSVG({
                exporting: {
                    sourceWidth: chart.chartWidth,
                    sourceHeight: chart.chartHeight
                }
            });

            var canvas = document.createElement('canvas');
            canvas.height = render_height;
            canvas.width = render_width;

            var image = new Image;
            image.onload = function () {
                canvas.getContext('2d').drawImage(this, 0, 0, render_width, render_height);
                var data = canvas.toDataURL("image/png")
                download(data, filename + '.png', numGrafica);
            };
            // image.src = 'data:image/svg+xml;base64,' + window.btoa(svg);

            var imgsrc = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(svg)));
            // var img = new Image(1, 1); // width, height values are optional params 
            image.src = imgsrc;
        }

        function download(data, filename, numGrafica) {
            var a = document.createElement('a');
            document.body.appendChild(a);
            switch (numGrafica) {
                case 1:
                    imgGraficaCumplimientoPresupuestoAcumulado = data;
                    break;
                case 2:
                    imgGraficaCumplimientoPresupuestoMensual = data;
                    break;
                case 3:
                    imgGraficaProyeccion = data;
                    break;
                default:
                    break;
            }
        }

        function fncDescargarInformePlanAccion() {
            let obj = new Object();
            obj = {
                graficaCumplimientoPresupuestoAcumulado: imgGraficaCumplimientoPresupuestoAcumulado,
                graficaCumplimientoPresupuestoMensual: imgGraficaCumplimientoPresupuestoMensual,
                graficaProyeccion: imgGraficaProyeccion,
                acumuladoGasto: montoGastoInforme.text(),
                acumuladoIngreso: montoIngresoInforme.text(),
                acumuladoObjetivo: txtRatioGastoVsIngresoAcumuladoInforme.text(),
                acumuladoReal: montoRealInforme.text(),
                acumuladoCumplimiento: porcentajeCumplimientoIngresoGastoInforme.text(),
                mensualGasto: montoGastoMensualInforme.text(),
                mensualIngreso: montoIngresoMensualInforme.text(),
                mensualObjetivo: txtRatioGastoVsIngresoMensualInforme.text(),
                mensualReal: montoRealMensualInforme.text(),
                mensualCumplimiento: porcentajeCumplimientoIngresoGastoMensualInforme.text()
            }
            axios.post("GenerarReportePlanAccion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let cc = botonDescargarReportePlanAccion.attr('cc');

                    $.blockUI({ message: 'Cargando Reporte...' });
                    report.attr("src", `/Reportes/Vista.aspx?idReporte=255&idCC=${cc}&idMes=${cboFiltroMes.val()}`);
                    document.getElementById('report').onload = function () {
                        openCRModal();
                        $.unblockUI();
                    };
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            if (variables != undefined) {
                let idPlanAccion = variables.idPlanAccion != undefined && variables.idPlanAccion > 0 ? variables.idPlanAccion : 0;
                if (idPlanAccion > 0) {
                    fncGetPlanAccion(0, 0, 0, idPlanAccion);
                }

                let anio = variables.anio != undefined && variables.anio > 0 ? variables.anio : 0;
                let idMes = variables.idMes != undefined && variables.idMes > 0 ? variables.idMes : 0;
                let idCC = variables.idCC != undefined && variables.idCC > 0 ? variables.idCC : 0;

                if (anio > 0 && idMes > 0 && idCC > 0) {
                    parametro_Anio = anio
                    parametro_idMes = idMes;
                    parametro_idCC = idCC;

                    cboFiltroMes.val(parametro_idMes);
                    cboFiltroMes.trigger("change");

                    cboFiltroAnio.val(parametro_Anio);
                    cboFiltroAnio.trigger("change");
                }

                var clean_uri = location.protocol + "//" + location.host + location.pathname;
                window.history.replaceState({}, document.title, clean_uri);
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

        //#region ENVIO INFORME
        function initTblEnvioInforme() {
            dtEnvioInforme = tblAF_CtrlPptalOfCe_EnvioInforme.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'anio', title: 'AÑO', visible: false },
                    { data: 'mes', title: 'MES', visible: false },
                    { data: 'empresa', title: 'EMPRESA' },
                    { data: 'cc', title: 'CC' },
                    {
                        data: 'cumplimientoMensual', title: 'Cumplimiento<br/>Mensual', className: 'dt-body-right dt-head-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupador) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            let flechaColor = '';
                            let flechaDireccion = '';
                            let cumplimientoMensual = "";

                            if (parseFloat(data) < 97) {
                                flechaColor = 'greenArrow';
                                flechaDireccion = 'down';
                                cumplimientoMensual = `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                            }
                            if (parseFloat(data) >= 97 && parseFloat(data) <= 100) {
                                flechaColor = 'yellowArrow';
                                flechaDireccion = 'right';
                                cumplimientoMensual = `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                            }
                            if (parseFloat(data) > 100) {
                                flechaColor = 'redArrow';
                                flechaDireccion = 'up';
                                cumplimientoMensual = `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;

                                //if (row.esAgrupador) {
                                //    return `<i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span>`;
                                //} else {
                                //    return `<a class="planAccion"><i class="fa fa-arrow-${flechaDireccion} ${flechaColor}"></i>&nbsp;<span>${Math.trunc(data)}%</span></a>`;
                                //}
                            }
                            return cumplimientoMensual;
                        }
                    },
                    {
                        data: 'envioInforme', title: 'Estatus informe',
                        render: function (data, type, row, meta) {
                            let color = "";
                            let title = "";
                            let icono = "";
                            if (row.envioInforme == 1) {
                                color = "green";
                                title = "Se ha enviado el informe.";
                                icono = "fas fa-check-circle";
                            } else if (row.envioInforme == 2) {
                                color = "red";
                                title = "No se ha enviado el informe.";
                                icono = "fas fa-times-circle";
                            } else if (row.envioInforme == 3) {
                                color = "gray";
                                title = "No aplica para enviar informe.";
                                icono = "fa fa-minus";
                            }

                            return `<span title="${title}"><i class="${icono}" style="color:${color}; font-size: 20px;"></i></span>`;
                        },
                    },
                    {
                        //data: 'envioInforme', 
                        title: 'Aditivas',
                        render: function (data, type, row, meta) {
                            let idCC = row.idCC;
                            let anio = row.anio;
                            if (row.aplicaAditiva) {
                                let totalAditiva = maskNumero_NoDecimal(row.totalAditivas);

                                var auxHtml = `<button class="btn btn-warning aditiva" title="Total Aditivas = ${totalAditiva}" cc="${idCC}" anio="${anio}">`;
                                auxHtml += `<i class="fa fa-arrow-up greenArrow" style="color: green;"></i> <span class="importePresupuesto importeEnero" mes="1">${Math.trunc(row.numAditivasAutorizadas)} </span>`;
                                auxHtml += `<i class="fa fa-arrow-down redArrow" style="color: red;"></i> <span class="importePresupuesto importeEnero" mes="1">${Math.trunc(row.numDeductivasAutorizadas)} </span>`;
                                auxHtml += `<i class="fa fa-arrow-right yellowArrow" style="color: yellow;"></i> <span class="importePresupuesto importeEnero" mes="1">${Math.trunc(row.numAditivasPendientes)} </span>`;
                                auxHtml += `</button>`;
                                return auxHtml;
                            } else {
                                return '';
                            }
                        },
                    },
                    {
                        title: 'Informe',
                        render: function (data, type, row, meta) {
                            let idCC = row.idCC;
                            let empresa = (row.empresa == 'CONSTRUPLAN' ? 1 : 2);
                            return `<button class="btn btn-success planAccion" title="Informe" cc="${idCC}" empresa="${empresa}"><i class="fa fa-file" aria-hidden="true"></i></button>`;

                        },
                    },
                    {
                        title: 'Mensajes',
                        render: function (data, type, row, meta) {
                            let idCC = row.idCC;
                            let mes = row.idMes;
                            let anio = row.anio;
                            let empresa = (row.empresa == 'CONSTRUPLAN' ? 1 : 2);
                            return `<button class="btn btn-info mensajes" title="Mensajes" cc="${idCC}" empresa="${empresa}" mes="${mes}" anio="${anio}"><i class="fa fa-comments" aria-hidden="true"></i></button>`;

                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblAF_CtrlPptalOfCe_EnvioInforme.on("click", ".aditiva", function () {
                        let idCC = $(this).attr('cc');
                        let anio = $(this).attr('anio');

                        //$('#divAditivasBody').find('#comboBoxYear').val(anio);
                        $('#comboBoxYear').val(anio);
                        $('#comboBoxYear').change();
                        $('#comboBoxCC').val(idCC);
                        $('#comboBoxCC').change();
                        $('#comboBoxEstatus').val('');
                        $('#comboBoxEstatus').trigger('change');
                        $('#botonBuscar').trigger('click');

                        $('#divAditivas').modal('show');

                        //$.ajax({
                        //    url: '/Administrativo/CtrlPptalOficinasCentrales/_AutorizacionesAditivas',
                        //    type: 'GET',
                        //    success: function(data) {
                        //        $('#divAditivasBody').html(data);

                        //        $('#divAditivas').modal('show');
                        //        $('#divAditivasBody').find('#comboBoxYear').val(anio);
                        //        $('#comboBoxYear').change();
                        //        $('#comboBoxCC').val(idCC);
                        //        $('#comboBoxCC').change();
                        //    },
                        //    error: function() {
                        //        //alert("There is some problem in the service!")
                        //    }
                        //});
                        //$(this).href = '/Administrativo/CtrlPptalOficinasCentrales/AutorizacionesAditivas?anio=' + anio + '&idCC=' + idCC;
                        //location.href = '/Administrativo/CtrlPptalOficinasCentrales/AutorizacionesAditivas?anio=' + anio + '&idCC=' + idCC;
                    });
                    tblAF_CtrlPptalOfCe_EnvioInforme.on("click", ".planAccion", function () {
                        let cc = $(this).attr('cc');
                        let empresa = $(this).attr('empresa');
                        botonDescargarReportePlanAccion.attr('cc', cc);
                        btnReportePlanAccionEnviarInforme.attr('cc', cc);
                        fncGetDataGraficasInforme(cc, empresa);

                        //panelGraficasDashboard.html(panelGraficas.html());
                        fncGetReportePlanAccionesByCC(cc);
                        btnReportePlanAccionEnviarInforme.css("display", "none");
                        mdlReportePlanAccion.modal("show");

                        //$.blockUI({ message: 'Cargando Reporte...' });
                        //report.attr("src", `/Reportes/Vista.aspx?idReporte=257&idCC=${cc}&idMes=${cboFiltroMes.val()}`);
                        //document.getElementById('report').onload = function () {
                        //    openCRModal();
                        //    $.unblockUI();
                        //};
                    });
                    tblAF_CtrlPptalOfCe_EnvioInforme.on("click", ".mensajes", function () {
                        let idCC = $(this).attr('cc');
                        let mes = $(this).attr('mes');
                        let anio = $(this).attr('anio');
                        let empresa = $(this).attr('empresa');
                        $("#btnAddComentario").attr('cc', idCC);
                        $("#btnAddComentario").attr('mes', mes);
                        $("#btnAddComentario").attr('anio', anio);
                        $("#btnAddComentario").attr('empresa', empresa);
                        CargarMensajes(idCC, mes, anio, empresa);
                        divVerComentario.modal("show");
                    });

                },
                columnDefs: [
                    // { className: 'dt-center', 'targets': '_all' },
                    { className: 'dt-body-center', targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '500px', targets: [3] }
                ],
            });
        }
        function CargarMensajes(idCC, mes, anio, empresa) {
            axios.post("GetMensajesDashboard", { idCC, mes, anio, empresa }).then(response => {
                if (response.data.success) {
                    setComentarios(response.data.mensajes);
                    txtComentarios.val("");
                }
                else {
                    Alert2Error(response.data.message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetEnvioInforme() {
            if (cboFiltroAnio.val() > 0 && cboFiltroMes.val() > 0) {
                let obj = new Object();
                obj.anio = cboFiltroAnio.val();
                obj.idMes = cboFiltroMes.val();
                obj.idEmpresa = cboFiltroInformeEmpresa.val();
                axios.post("GetEnvioInforme", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtEnvioInforme.clear();
                        dtEnvioInforme.rows.add(response.data.lstEnvioInformeDTO);
                        dtEnvioInforme.draw();
                        mdlEnvioInforme.modal("show");
                        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = ""; //TO DO
                if (cboFiltroAnio.val() <= 0) { strMensajeError += "Es necesario indicar el año."; }
                if (cboFiltroMes.val() <= 0) { strMensajeError += "<br>Es necesario indicar el mes."; }
                Alert2Warning(strMensajeError);
            }
        }

        function setComentarios(data) {
            var htmlComentario = "";
            $.each(data, function (i, e) {
                htmlComentario += "<li class='comentario' data-id='" + e.id + "'>";
                htmlComentario += "    <div class='timeline-item'>";
                htmlComentario += "        <span class='time'><i class='fa fa-clock-o'></i>" + e.fecha + "</span>";
                htmlComentario += "        <h3 class='timeline-header'><a href='#'>" + e.usuarioNombre + "</a></h3>";
                htmlComentario += "        <div class='timeline-body'>";
                htmlComentario += "             " + e.comentario;
                htmlComentario += "        </div>";
                htmlComentario += "    </div>";
                htmlComentario += "</li>";
            });
            ulComentarios.html(htmlComentario);
        }

        function insertCommentary(e) {
            if (validateComentario()) {
                let cc = $("#btnAddComentario").attr('cc');
                let mes = $("#btnAddComentario").attr('mes');
                let anio = $("#btnAddComentario").attr('anio');
                let empresa = $("#btnAddComentario").attr('empresa');
                var obj = getNewCommentary(cc, mes, anio);
                obj.usuarioNombre = '';

                $.ajax({
                    type: "POST",
                    url: "/CtrlPptalOficinasCentrales/GuardarMensajes",
                    data: { obj, empresa },
                    dataType: 'json',
                    success: function (response) {
                        $.unblockUI();
                        CargarMensajes(cc, mes, anio, empresa);
                        //setComentarios(data.comentarios);
                        txtComentarios.val("");
                    },
                    error: function (error) {
                        $.unblockUI();
                    }
                });
            } else {
                e.preventDefault()
            }
        }
        function getNewCommentary(cc, mes, anio) {
            var r = {};
            r.id = 0;
            r.anio = anio;
            r.mes = mes;
            r.idCC = cc;
            r.mensaje = txtComentarios.val();
            r.registroActivo = true;
            return r;
        }
        function validateComentario() {
            var state = true;
            if (!validarCampo(txtComentarios)) { state = false; }
            return state;
        }
        function validarCampo(_this) {
            var r = false;
            if (_this.val() == '') {
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
        //#endregion
    }

    $(document).ready(() => {
        CtrlPptalOfCE.PresupuestosGastos = new PresupuestosGastos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 9999 }); })
        .ajaxStop(() => { $.unblockUI(); });
})();