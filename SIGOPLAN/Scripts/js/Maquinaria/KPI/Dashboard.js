(() => {
    $.namespace('Maquinaria.KPI.DashboardKPI');
    DashboardKPI = function () {
        //#region Selectores
        const cboAreaCuenta = $('#cboAreaCuenta');
        const cboGrupo = $('#cboGrupo');
        const cboModelo = $('#cboModelo');
        const cboEconomico = $('#cboEconomico');
        const cboTurno = $('#cboTurno');
        const txtFechaInicio = $('#txtFechaInicio');
        const txtFechaFin = $('#txtFechaFin');
        const btnBuscar = $('#btnBuscar');
        const divCboGrupo = $('#divCboGrupo');
        const divCboModelo = $('#divCboModelo');
        const divCboEconomico = $('#divCboEconomico');

        const txtDisponibilidadAnual = $('#txtDisponibilidadAnual');
        const txtUtilizacionAnual = $('#txtUtilizacionAnual');
        const txtEficienciaAnual = $('#txtEficienciaAnual');
        const txtHorasAnual = $('#txtHorasAnual');

        const btnExcel = $('#btnExcel');
        // #endregion

        //#region selectoresTablaTiempos
        const hrsProgramado = $('#hrsProgramado');
        const hrsDisponible = $('#hrsDisponible');
        const porDisponible = $('#porDisponible');
        const hrsMantenimiento = $('#hrsMantenimiento');
        const porMantenimiento = $('#porMantenimiento');
        const hrsOperacion = $('#hrsOperacion');
        const porOperacion = $('#porOperacion');
        const hrsTrabajo = $('#hrsTrabajo');
        const porTrabajo = $('#porTrabajo');
        const hrsDemora = $('#hrsDemora');
        const porDemora = $('#porDemora');
        const hrsParado = $('#hrsParado');
        const porParado = $('#porParado');
        const hrsProgramadoSM = $('#hrsProgramadoSM');
        const porProgramadoSM = $('#porProgramadoSM');
        const hrsNoProgramadoUM = $('#hrsNoProgramadoUM');
        const porNoProgramadoUM = $('#porNoProgramadoUM');
        //#endregion

        //#region graficas disponibilidad vs utilizacion
        const graficaDisponibilidadUtilizacion_grupo = $('#graficaDisponibilidadUtilizacion_grupo');
        const graficaDisponibilidadUtilizacion_modelo = $('#graficaDisponibilidadUtilizacion_modelo');
        const graficaDisponibilidadUtilizacion_economico = $('#graficaDisponibilidadUtilizacion_economico');
        const graficaDisponibilidadUtilizacion_semanal = $('#graficaDisponibilidadUtilizacion_semanal');
        const graficaDisponibilidadUtilizacion_mensual = $('#graficaDisponibilidadUtilizacion_mensual');

        const btnDisVsUti_grupo = $('#btnDisVsUti_grupo');
        const btnDisVsUti_modelo = $('#btnDisVsUti_modelo');
        const btnDisVsUti_economico = $('#btnDisVsUti_economico');
        const btnDisVsUti_semanal = $('#btnDisVsUti_semanal');
        const btnDisVsUti_mensual = $('#btnDisVsUti_mensual');
        //#endregion

        //#region graficas opereacion vs trabajo
        const graficaOperacionTrabajo_grupo = $('#graficaOperacionTrabajo_grupo');
        const graficaOperacionTrabajo_modelo = $('#graficaOperacionTrabajo_modelo');
        const graficaOperacionTrabajo_economico = $('#graficaOperacionTrabajo_economico');
        const graficaOperacionTrabajo_semanal = $('#graficaOperacionTrabajo_semanal');
        const graficaOperacionTrabajo_mensual = $('#graficaOperacionTrabajo_mensual');

        const btnOpeVsTra_grupo = $('#btnOpeVsTra_grupo');
        const btnOpeVsTra_modelo = $('#btnOpeVsTra_modelo');
        const btnOpeVsTra_economico = $('#btnOpeVsTra_economico');
        const btnOpeVsTra_semanal = $('#btnOpeVsTra_semanal');
        const btnOpeVsTra_mensual = $('#btnOpeVsTra_mensual');
        //#endregion

        //#region GRAFICA UTILIZACIÓN
        const graficaUT_grupo = $("#graficaUT_grupo");
        const graficaUT_modelo = $("#graficaUT_modelo");
        const graficaUT_economico = $("#graficaUT_economico");
        const graficaUT_semanal = $("#graficaUT_semanal");
        const graficaUT_mensual = $("#graficaUT_mensual");

        const btnOpeUT_grupo = $("#btnOpeUT_grupo");
        const btnOpeUT_modelo = $("#btnOpeUT_modelo");
        const btnOpeUT_economico = $("#btnOpeUT_economico");
        const btnOpeUT_semanal = $("#btnOpeUT_semanal");
        const btnOpeUT_mensual = $("#btnOpeUT_mensual");
        //#endregion

        //#region CONTROLES DE LA GRAFICA DIAGRAMA DE PARETO DE PAROS PRINCIPALES
        const btnTiempo = $('#btnTiempo');
        const btnFrecuencia = $('#btnFrecuencia');
        const graficaParetoParosPrincipalesTiempoMT = $("#graficaParetoParosPrincipalesTiempoMT");
        const graficaParetoParosPrincipalesTiempoID = $("#graficaParetoParosPrincipalesTiempoID");
        const graficaParetoParosPrincipalesTiempoDL = $("#graficaParetoParosPrincipalesTiempoDL");
        const graficaParetoParosPrincipalesTiempoGeneral = $("#graficaParetoParosPrincipalesTiempoGeneral");
        const graficaParetoParosPrincipalesFrecuenciaMT = $("#graficaParetoParosPrincipalesFrecuenciaMT");
        const graficaParetoParosPrincipalesFrecuenciaID = $("#graficaParetoParosPrincipalesFrecuenciaID");
        const graficaParetoParosPrincipalesFrecuenciaDL = $("#graficaParetoParosPrincipalesFrecuenciaDL");
        const graficaParetoParosPrincipalesFrecuenciaGeneral = $("#graficaParetoParosPrincipalesFrecuenciaGeneral");
        const btnTipoParoMT = $("#btnTipoParoMT");
        const btnTipoParoID = $("#btnTipoParoID");
        const btnTipoParoDL = $("#btnTipoParoDL");
        const btnTipoParoGeneral = $("#btnTipoParoGeneral");
        //#endregion

        // #region TABLE DETALLES DE LAS GRAFICAS PASTEL
        const mdlGraficaDetallesParosMantenimiento = $('#mdlGraficaDetallesParosMantenimiento');
        const tblGraficaDetallesParosMantenimiento = $('#tblGraficaDetallesParosMantenimiento');
        let dtGraficaDetallesParosMantenimiento;

        const mdlGraficaDetallesParosReservaSinUso = $('#mdlGraficaDetallesParosReservaSinUso');
        const tblGraficaDetallesParosReservaSinUso = $('#tblGraficaDetallesParosReservaSinUso');
        let dtGraficaDetallesParosReservaSinUso;

        const mdlGraficaDetallesGraficaParosDemora = $('#mdlGraficaDetallesGraficaParosDemora');
        const tblGraficaDetallesParosDemora = $('#tblGraficaDetallesParosDemora');
        let dtGraficaDetallesParosDemora;

        let filtro;
        // #endregion

        //#region Eventos
        btnBuscar.on('click', function () {
            let infoFiltro = fncCrearFiltro();

            fncGetInfoFiltro(infoFiltro);
            // fncGetDatosGraficaParosReservaSinUso();
        });

        btnExcel.on('click', function () {
            location.href = '/KPI/GetExcel';
        })

        btnDisVsUti_grupo.click(function (e) {
            fncOcultarGraficasDisVsUti();
            graficaDisponibilidadUtilizacion_grupo.removeAttr("style");

            fncBackgroundColorDefaultGraficasDisVsUti();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnDisVsUti_modelo.click(function (e) {
            fncOcultarGraficasDisVsUti();
            graficaDisponibilidadUtilizacion_modelo.removeAttr("style");

            fncBackgroundColorDefaultGraficasDisVsUti();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnDisVsUti_economico.click(function (e) {
            fncOcultarGraficasDisVsUti();
            graficaDisponibilidadUtilizacion_economico.removeAttr("style");

            fncBackgroundColorDefaultGraficasDisVsUti();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnDisVsUti_semanal.click(function (e) {
            fncOcultarGraficasDisVsUti();
            graficaDisponibilidadUtilizacion_semanal.removeAttr("style");

            fncBackgroundColorDefaultGraficasDisVsUti();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnDisVsUti_mensual.click(function (e) {
            fncOcultarGraficasDisVsUti();
            graficaDisponibilidadUtilizacion_mensual.removeAttr("style");

            fncBackgroundColorDefaultGraficasDisVsUti();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnOpeVsTra_grupo.click(function (e) {
            fncOcultarGraficasOpeVsTra();
            graficaOperacionTrabajo_grupo.removeAttr("style");

            fncBackgroundColorDefaultGraficasOpeVsTra();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnOpeVsTra_modelo.click(function (e) {
            fncOcultarGraficasOpeVsTra();
            graficaOperacionTrabajo_modelo.removeAttr("style");

            fncBackgroundColorDefaultGraficasOpeVsTra();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnOpeVsTra_economico.click(function (e) {
            fncOcultarGraficasOpeVsTra();
            graficaOperacionTrabajo_economico.removeAttr("style");

            fncBackgroundColorDefaultGraficasOpeVsTra();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnOpeVsTra_semanal.click(function (e) {
            fncOcultarGraficasOpeVsTra();
            graficaOperacionTrabajo_semanal.removeAttr("style");

            fncBackgroundColorDefaultGraficasOpeVsTra();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnOpeVsTra_mensual.click(function (e) {
            fncOcultarGraficasOpeVsTra();
            graficaOperacionTrabajo_mensual.removeAttr("style");

            fncBackgroundColorDefaultGraficasOpeVsTra();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnOpeUT_grupo.click(function (e) {
            fncOcultarGraficasUT();
            graficaUT_grupo.removeAttr("style");

            fncBackgroundColorDefaultGraficasUT();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnOpeUT_modelo.click(function (e) {
            fncOcultarGraficasUT();
            graficaUT_modelo.removeAttr("style");

            fncBackgroundColorDefaultGraficasUT();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnOpeUT_economico.click(function (e) {
            fncOcultarGraficasUT();
            graficaUT_economico.removeAttr("style");

            fncBackgroundColorDefaultGraficasUT();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnOpeUT_semanal.click(function (e) {
            fncOcultarGraficasUT();
            graficaUT_semanal.removeAttr("style");

            fncBackgroundColorDefaultGraficasUT();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });

        btnOpeUT_mensual.click(function (e) {
            fncOcultarGraficasUT();
            graficaUT_mensual.removeAttr("style");

            fncBackgroundColorDefaultGraficasUT();

            $(this).attr("esActivo", 1);
            $(this).css("background-color", "#008000");
        });
        //#endregion

        (function init() {
            cboAreaCuenta.change(function (e) {
                cboGrupo.fillCombo("/KPI/FillCboGruposEnCaptura", {lstCC: getValoresMultiples("#cboAreaCuenta") }, true);
                convertToMultiselectSelectAll(cboGrupo);
                fncFillCboEconomico();
                if ($(this).val() != "") {
                    cboGrupo.multiselect('enable');
                    cboModelo.multiselect('enable');
                    // cboEconomico.multiselect('enable');
                } else {
                    cboGrupo.multiselect('disable');
                    cboModelo.multiselect('disable');
                    cboEconomico.multiselect('disable');
                }
            });

            cboGrupo.change(function (e) {
                if (cboGrupo.val().length == 0) {
                    fncFillCboModelos();
                    fncFillCboEconomico();
                    cboModelo.multiselect('disable');
                    cboEconomico.multiselect('disable');

                } else {
                    cboModelo.multiselect('enable');
                    fncFillCboModelos();
                }
            });

            txtFechaInicio.datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: "1900:2050"
            }).datepicker("setDate", new Date());

            txtFechaFin.datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: "1900:2050"
            }).datepicker("setDate", new Date());

            txtFechaInicio.attr("autocomplete", "off");
            txtFechaFin.attr("autocomplete", "off");

            fncLimpiarTablaTiempos();

            fncFillCboCC();

            // cboGrupo.multiselect('selectAll', true).multiselect('updateButtonText');
            cboGrupo.change(function (e) {
                // fncFillCboModelos();
            });

            cboModelo.change(function (e) {
                if (cboModelo.val().length == 0) {
                    fncFillCboEconomico();
                    cboEconomico.multiselect('disable');

                } else {
                    cboEconomico.multiselect('disable');
                    fncFillCboEconomico();
                }
            })

            convertToMultiselectSelectAll(cboGrupo);
            convertToMultiselectSelectAll(cboModelo);
            convertToMultiselectSelectAll(cboEconomico);

            cboGrupo.multiselect('disable');
            cboModelo.multiselect('disable');
            cboEconomico.multiselect('disable');

            // fncCargarGraficaDisponibilidadUtilizacion_grupo();
            // fncCargarGraficaDisponibilidadUtilizacion_modelo();
            // fncCargarGraficaDisponibilidadUtilizacion_economico();
            // fncCargarGraficaParosMantenimiento();
            // fncCargarGraficaParosReservaSinUso();
            // fncCargarGraficaParosDemora();

            //#region SE CARGAN LAS GRAFICAS PARETO DE PAROS PRINCIPALES
            fncCargarGraficaParetoParosPrincipalesTiempoMT();
            fncCargarGraficaParetoParosPrincipalesTiempoID();
            fncCargarGraficaParetoParosPrincipalesTiempoDL();
            fncCargarGraficaParetoParosPrincipalesTiempoGeneral();
            fncCargarGraficaParetoParosPrincipalesFrecuenciaMT();
            fncCargarGraficaParetoParosPrincipalesFrecuenciaID();
            fncCargarGraficaParetoParosPrincipalesFrecuenciaDL();
            fncCargarGraficaParetoParosPrincipalesFrecuenciaGeneral();
            //#endregion

            btnTiempo.css("background-color", "#008000");
            btnTiempo.attr("esActivo", 1);
            btnFrecuencia.attr("esActivo", 0);
            btnTiempo.click(function (e) {
                $(this).attr("esActivo", 1);
                $(this).css("background-color", "#008000");
                btnFrecuencia.attr("esActivo", 0);
                btnFrecuencia.css("background-color", "#337ab7");
                fncMostrarGraficasParetoParosPrincipales();
            });
            btnFrecuencia.click(function (e) {
                $(this).attr("esActivo", 1);
                $(this).css("background-color", "#008000");
                btnTiempo.attr("esActivo", 0);
                btnTiempo.css("background-color", "#337ab7");
                fncMostrarGraficasParetoParosPrincipales();
            });

            fncOcultarGraficasParetoParosPrincipales();
            btnTipoParoMT.css("background-color", "#008000");
            btnTipoParoMT.attr("esActivo", 1);
            graficaParetoParosPrincipalesTiempoMT.removeAttr("style");
            btnTipoParoMT.click(function (e) {
                fncOcultarGraficasParetoParosPrincipales();
                if (btnTiempo.attr("esActivo") == 1) {
                    graficaParetoParosPrincipalesTiempoMT.removeAttr("style");
                } else if (btnFrecuencia.attr("esActivo") == 1) {
                    graficaParetoParosPrincipalesFrecuenciaMT.removeAttr("style");
                }
                fncBackgroundColorDefaultGraficasParetoParosPrincipales()
                $(this).attr("esActivo", 1);
                $(this).css("background-color", "#008000");
            });
            btnTipoParoID.click(function (e) {
                fncOcultarGraficasParetoParosPrincipales();
                if (btnTiempo.attr("esActivo") == 1) {
                    graficaParetoParosPrincipalesTiempoID.removeAttr("style");
                } else if (btnFrecuencia.attr("esActivo") == 1) {
                    graficaParetoParosPrincipalesFrecuenciaID.removeAttr("style");
                }
                fncBackgroundColorDefaultGraficasParetoParosPrincipales()
                $(this).attr("esActivo", 1);
                $(this).css("background-color", "#008000");
            });
            btnTipoParoDL.click(function (e) {
                fncOcultarGraficasParetoParosPrincipales();
                if (btnTiempo.attr("esActivo") == 1) {
                    graficaParetoParosPrincipalesTiempoDL.removeAttr("style");
                } else if (btnFrecuencia.attr("esActivo") == 1) {
                    graficaParetoParosPrincipalesFrecuenciaDL.removeAttr("style");
                }
                fncBackgroundColorDefaultGraficasParetoParosPrincipales()
                $(this).attr("esActivo", 1);
                $(this).css("background-color", "#008000");
            });
            btnTipoParoGeneral.click(function (e) {
                fncOcultarGraficasParetoParosPrincipales();
                if (btnTiempo.attr("esActivo") == 1) {
                    graficaParetoParosPrincipalesTiempoGeneral.removeAttr("style");
                } else if (btnFrecuencia.attr("esActivo") == 1) {
                    graficaParetoParosPrincipalesFrecuenciaGeneral.removeAttr("style");
                }
                fncBackgroundColorDefaultGraficasParetoParosPrincipales()
                $(this).attr("esActivo", 1);
                $(this).css("background-color", "#008000");
            });

            fncOcultarGraficasDisVsUti();
            fncOcultarGraficasOpeVsTra();
            fncOcultarGraficasUT();

            btnDisVsUti_economico.css("background-color", "#008000");
            btnDisVsUti_economico.attr("esActivo", 1);
            graficaDisponibilidadUtilizacion_economico.removeAttr("style");

            btnOpeVsTra_economico.css("background-color", "#008000");
            btnOpeVsTra_economico.attr("esActivo", 1);
            graficaOperacionTrabajo_economico.removeAttr("style");

            btnOpeUT_economico.css("background-color", "#008000");
            btnOpeUT_economico.attr("esActivo", 1);
            graficaUT_economico.removeAttr("style");

            btnTipoParoMT.attr("tipoParo", 4);
            btnTipoParoID.attr("tipoParo", 3);
            btnTipoParoDL.attr("tipoParo", 2);
            btnTipoParoGeneral.attr("tipoParo", 0);
        })();

        function fncMostrarGraficasParetoParosPrincipales() {
            fncOcultarGraficasParetoParosPrincipales();
            if (btnTiempo.attr("esActivo") == 1) {
                if (btnTipoParoMT.attr("esActivo") == 1) { graficaParetoParosPrincipalesTiempoMT.removeAttr("style"); }
                if (btnTipoParoID.attr("esActivo") == 1) { graficaParetoParosPrincipalesTiempoID.removeAttr("style"); }
                if (btnTipoParoDL.attr("esActivo") == 1) { graficaParetoParosPrincipalesTiempoDL.removeAttr("style"); }
                if (btnTipoParoGeneral.attr("esActivo") == 1) { graficaParetoParosPrincipalesTiempoGeneral.removeAttr("style"); }
            } else if (btnFrecuencia.attr("esActivo") == 1) {
                if (btnTipoParoMT.attr("esActivo") == 1) { graficaParetoParosPrincipalesFrecuenciaMT.removeAttr("style"); }
                if (btnTipoParoID.attr("esActivo") == 1) { graficaParetoParosPrincipalesFrecuenciaID.removeAttr("style"); }
                if (btnTipoParoDL.attr("esActivo") == 1) { graficaParetoParosPrincipalesFrecuenciaDL.removeAttr("style"); }
                if (btnTipoParoGeneral.attr("esActivo") == 1) { graficaParetoParosPrincipalesFrecuenciaGeneral.removeAttr("style"); }
            } else {
                Alert2Warning("Es necesario selecionar Tiempo o Frencuencia.");
            }
        }

        function fncBackgroundColorDefaultGraficasParetoParosPrincipales() {
            btnTipoParoMT.css("background-color", "#337ab7");
            btnTipoParoID.css("background-color", "#337ab7");
            btnTipoParoDL.css("background-color", "#337ab7");
            btnTipoParoGeneral.css("background-color", "#337ab7");

            btnTipoParoMT.attr("esActivo", 0);
            btnTipoParoID.attr("esActivo", 0);
            btnTipoParoDL.attr("esActivo", 0);
            btnTipoParoGeneral.attr("esActivo", 0);
        }

        function fncBackgroundColorDefaultGraficasDisVsUti() {
            btnDisVsUti_economico.css('background-color', '#337ab7');
            btnDisVsUti_modelo.css('background-color', '#337ab7');
            btnDisVsUti_grupo.css('background-color', '#337ab7');
            btnDisVsUti_semanal.css('background-color', '#337ab7');
            btnDisVsUti_mensual.css('background-color', '#337ab7');

            btnDisVsUti_economico.attr('esActivo', 0);
            btnDisVsUti_modelo.attr('esActivo', 0);
            btnDisVsUti_grupo.attr('esActivo', 0);
            btnDisVsUti_mensual.attr('esActivo', 0);
        }

        function fncBackgroundColorDefaultGraficasOpeVsTra() {
            btnOpeVsTra_grupo.css('background-color', '#337ab7');
            btnOpeVsTra_modelo.css('background-color', '#337ab7');
            btnOpeVsTra_economico.css('background-color', '#337ab7');
            btnOpeVsTra_semanal.css('background-color', '#337ab7');
            btnOpeVsTra_mensual.css('background-color', '#337ab7');

            btnOpeVsTra_grupo.attr('esActivo', 0);
            btnOpeVsTra_modelo.attr('esActivo', 0);
            btnOpeVsTra_economico.attr('esActivo', 0);
            btnOpeVsTra_semanal.attr('esActivo', 0);
            btnOpeVsTra_mensual.attr('esActivo', 0);
        }

        function fncBackgroundColorDefaultGraficasUT() {
            btnOpeUT_grupo.css('background-color', '#337ab7');
            btnOpeUT_modelo.css('background-color', '#337ab7');
            btnOpeUT_economico.css('background-color', '#337ab7');
            btnOpeUT_semanal.css('background-color', '#337ab7');
            btnOpeUT_mensual.css('background-color', '#337ab7');

            btnOpeUT_grupo.attr('esActivo', 0);
            btnOpeUT_modelo.attr('esActivo', 0);
            btnOpeUT_economico.attr('esActivo', 0);
            btnOpeUT_semanal.attr('esActivo', 0);
            btnOpeUT_mensual.attr('esActivo', 0);
        }

        function fncOcultarGraficasDisVsUti() {
            graficaDisponibilidadUtilizacion_grupo.attr("style", "display:none");
            graficaDisponibilidadUtilizacion_modelo.attr("style", "display:none");
            graficaDisponibilidadUtilizacion_economico.attr("style", "display:none");
            graficaDisponibilidadUtilizacion_semanal.attr("style", "display:none");
            graficaDisponibilidadUtilizacion_mensual.attr("style", "display:none");
        }

        function fncOcultarGraficasOpeVsTra() {
            graficaOperacionTrabajo_grupo.attr("style", "display:none");
            graficaOperacionTrabajo_modelo.attr("style", "display:none");
            graficaOperacionTrabajo_economico.attr("style", "display:none");
            graficaOperacionTrabajo_semanal.attr("style", "display:none");
            graficaOperacionTrabajo_mensual.attr("style", "display:none");
        }

        function fncOcultarGraficasUT() {
            graficaUT_grupo.attr("style", "display:none");
            graficaUT_modelo.attr("style", "display:none");
            graficaUT_economico.attr("style", "display:none");
            graficaUT_semanal.attr("style", "display:none");
            graficaUT_mensual.attr("style", "display:none");
        }

        function fncOcultarGraficasParetoParosPrincipales() {
            graficaParetoParosPrincipalesTiempoMT.attr("style", "display:none");
            graficaParetoParosPrincipalesTiempoID.attr("style", "display:none");
            graficaParetoParosPrincipalesTiempoDL.attr("style", "display:none");
            graficaParetoParosPrincipalesTiempoGeneral.attr("style", "display:none");
            graficaParetoParosPrincipalesFrecuenciaMT.attr("style", "display:none");
            graficaParetoParosPrincipalesFrecuenciaID.attr("style", "display:none");
            graficaParetoParosPrincipalesFrecuenciaDL.attr("style", "display:none");
            graficaParetoParosPrincipalesFrecuenciaGeneral.attr("style", "display:none");
        }

        function fncFillCboCC() {
            cboAreaCuenta.fillCombo("/KPI/FillCboCC", {}, false);
            cboGrupo.fillCombo("/KPI/FillCboGruposEnCaptura", {lstCC: getValoresMultiples("#cboAreaCuenta") }, true);

            cboAreaCuenta.select2();
            // convertToMultiselect(cboGrupo);
            // convertToMultiselect(cboModelo);
            // convertToMultiselect(cboEconomico);
        }

        function fncFillCboModelos() {
            cboModelo.fillCombo('/KPI/FillCboModelosEnCaptura', { lstCC: getValoresMultiples("#cboAreaCuenta") ,lstGrupoID: getValoresMultiples("#cboGrupo") }, true, null);
            // cboModelo.multiselect('selectAll', true).multiselect('updateButtonText');
            convertToMultiselectSelectAll(cboModelo);
        }

        function fncFillCboEconomico() {
            cboEconomico.fillCombo('/KPI/FillCboEconomico', {
                lstCC: getValoresMultiples("#cboAreaCuenta"),
                lstGrupoID: getValoresMultiples("#cboGrupo"),
                // lstModeloID: getValoresMultiples("#cboModelo")
                lstModeloID: $("#cboModelo").val()
            }, true, null);
            // cboEconomico.multiselect('selectAll', true).multiselect('updateButtonText');
            convertToMultiselectSelectAll(cboEconomico);
        }

        function fncCargarGraficaDisponibilidadUtilizacion_economico(datos) {
            Highcharts.chart('graficaDisponibilidadUtilizacion_economico', {
                chart: {
                    type: 'column'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
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
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1

                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaDisponibilidadUtilizacion_grupo(datos) {
            Highcharts.chart('graficaDisponibilidadUtilizacion_grupo', {
                chart: {
                    type: 'column'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
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
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1

                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaDisponibilidadUtilizacion_modelo(datos) {
            Highcharts.chart('graficaDisponibilidadUtilizacion_modelo', {
                chart: {
                    type: 'column'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
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
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1

                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaDisponibilidadUtilizacion_semanal(datos) {
            Highcharts.chart('graficaDisponibilidadUtilizacion_semanal', {
                chart: {
                    type: 'line'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: false
                    }
                },
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1
                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaDisponibilidadUtilizacion_mensual(datos) {
            Highcharts.chart('graficaDisponibilidadUtilizacion_mensual', {
                chart: {
                    type: 'line'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: false
                    }
                },
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1
                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        //

        function fncCargarGraficaOperacionTrabajo_economico(datos) {
            Highcharts.chart('graficaOperacionTrabajo_economico', {
                chart: {
                    type: 'column'
                },
                labels: {
                    enabled: false
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
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
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1

                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaOperacionTrabajo_grupo(datos) {
            Highcharts.chart('graficaOperacionTrabajo_grupo', {
                chart: {
                    type: 'column'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
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
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1

                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaOperacionTrabajo_modelo(datos) {
            Highcharts.chart('graficaOperacionTrabajo_modelo', {
                chart: {
                    type: 'column'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
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
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1

                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaOperacionTrabajo_semanal(datos) {
            Highcharts.chart('graficaOperacionTrabajo_semanal', {
                chart: {
                    type: 'line'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: false
                    }
                },
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1
                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaOperacionTrabajo_mensual(datos) {
            Highcharts.chart('graficaOperacionTrabajo_mensual', {
                chart: {
                    type: 'line'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: false
                    }
                },
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1
                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCrearFiltro() {
            filtro = {
                areaCuenta: cboAreaCuenta.val(),
                idGrupos: cboGrupo.val(),
                idModelos: cboModelo.val(),
                idEconomicos: cboEconomico.val(),
                turno: cboTurno.val(),
                fechaInicio: txtFechaInicio.val(),
                fechaFin: txtFechaFin.val()
            }

            return filtro;
        }

        function fncLimpiarTablaTiempos() {
            hrsProgramado.text('- HORAS');
            hrsDisponible.text('- HORAS');
            porDisponible.text('%');
            hrsMantenimiento.text('- HORAS');
            porMantenimiento.text('%');
            hrsOperacion.text('- HORAS');
            porOperacion.text('%');
            hrsTrabajo.text('- HORAS');
            porTrabajo.text('%');
            hrsDemora.text('- HORAS');
            porDemora.text('%');
            hrsParado.text('- HORAS');
            porParado.text('%');
            hrsProgramadoSM.text('-');
            porProgramadoSM.text('-');
            hrsNoProgramadoUM.text('- HORAS');
            porNoProgramadoUM.text('-');
        }

        function fncFillTablaTiempos(datos) {
            hrsProgramado.text(datos.hrsProgramado + ' HORAS');
            hrsDisponible.text(datos.hrsDisponible + ' HORAS');
            porDisponible.text(datos.porDisponible + '%');
            hrsMantenimiento.text(datos.hrsMantenimiento + ' HORAS');
            porMantenimiento.text(datos.porMantenimiento + '%');
            hrsOperacion.text(datos.hrsOperacion + ' HORAS');
            porOperacion.text(datos.porOperacion + '%');
            hrsTrabajo.text(datos.hrsTrabajo + ' HORAS');
            porTrabajo.text(datos.porTrabajo + '%');
            hrsDemora.text(datos.hrsDemora + ' HORAS');
            porDemora.text(datos.porDemora + '%');
            hrsParado.text(datos.hrsParado + ' HORAS');
            porParado.text(datos.porParado + '%');
            hrsProgramadoSM.text(datos.hrsProgramadoSM);
            porProgramadoSM.text(datos.porProgramadoSM);
            hrsNoProgramadoUM.text(datos.hrsNoProgramadoUM + ' HORAS');
            porNoProgramadoUM.text(datos.porNoProgramadoUM);
        }

        //#region GRAFICA UT
        function fncCargarGraficaUT_economico(datos) {
            Highcharts.chart('graficaUT_economico', {
                chart: {
                    type: 'column'
                },
                labels: {
                    enabled: false
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
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
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1

                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaUT_grupo(datos) {
            Highcharts.chart('graficaUT_grupo', {
                chart: {
                    type: 'column'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
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
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1

                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaUT_modelo(datos) {
            Highcharts.chart('graficaUT_modelo', {
                chart: {
                    type: 'column'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
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
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1

                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaUT_semanal(datos) {
            Highcharts.chart('graficaUT_semanal', {
                chart: {
                    type: 'line'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: false
                    }
                },
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1
                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaUT_mensual(datos) {
            Highcharts.chart('graficaUT_mensual', {
                chart: {
                    type: 'line'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: false
                    }
                },
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1
                }],
                credits: {
                    enabled: false
                }
            });
            $('.highcharts-title').css("display", "none");
        }
        //#endregion

        // #region GRAFICAS PASTEL
        // GRAFICA PASTEL PAROS DE MANTENIMIENTO
        function fncCargarGraficaParosMantenimiento(lstValorCodigos, lstCodigosJS, cantDatos) {
            let objData = new Object();
            let lstData = [];
            for (let i = 0; i < cantDatos; i++) {
                objData = {
                    name: lstCodigosJS[i],
                    y: parseFloat(lstValorCodigos[i]),
                    sliced: true,
                    selected: true
                }
                lstData.push(objData);
            }
            Highcharts.chart('graficaParosMantenimiento', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie',
                    //events: {
                    //    click: function () {
                    //        mdlGraficaDetallesParosMantenimiento.modal("show");
                    //    }
                    //}
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
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
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                        }
                    }
                }, credits: {
                    enabled: false
                },
                series: [{
                    // name: 'Brands',
                    colorByPoint: true,
                    data: lstData,
                    events: {
                        click: function (event) {
                            let codigo = event.point.options.name;
                            fncGetInfoFiltroCodigo(filtro, codigo, 1);
                            mdlGraficaDetallesParosMantenimiento.modal("show");
                        }
                    }
                }]
            });
            $('.highcharts-title').css("display", "none");
        }
        function initDataTblS_DetallesGraficaParosMantenimiento() {
            dtGraficaDetallesParosMantenimiento = tblGraficaDetallesParosMantenimiento.DataTable({
                language: dtDicEsp,
                destroy: true,
                scrollY: "500px",
                scrollCollapse: true,
                paging: true,
                // pageLength: 100,
                columns: [
                    { data: 'codigo', title: 'codigo' },
                    { data: 'economico', title: 'economico' },
                    { data: 'ac', title: 'ac' },
                    { data: 'valor', title: 'Valor' },
                    { data: 'fecha', title: 'fecha' },
                    { data: 'turno', title: 'turno' },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                    // { "width": "13%", "targets": [0, 1, 3] }
                ],
                initComplete: function (settings, json) {
                }
            });
        }
        function fncGetDetallesGraficaParosMantenimiento(lstValorCodigos, lstCodigosJS, cantDatos) {
            let objData = {};
            let lstData = [];
            for (let i = 0; i < cantDatos; i++) {
                let codigo = lstCodigosJS[i];
                let valor = lstValorCodigos[i];
                objData = {
                    codigo: codigo,
                    valor: valor
                };
                lstData.push(objData);
            }
            dtGraficaDetallesParosMantenimiento.clear();
            dtGraficaDetallesParosMantenimiento.rows.add(lstData);
            dtGraficaDetallesParosMantenimiento.draw();
            dtGraficaDetallesParosMantenimiento.draw();
        }

        // GRAFICA PASTEL PAROS DE RESERVA SIN USO
        function fncCargarGraficaParosReservaSinUso(lstValorCodigos, lstCodigosJS, cantDatos) {
            let objData = new Object();
            let lstData = [];
            for (let i = 0; i < cantDatos; i++) {
                objData = {
                    name: lstCodigosJS[i],
                    y: parseFloat(lstValorCodigos[i]),
                    sliced: true,
                    selected: true
                }
                lstData.push(objData);
            }
            Highcharts.chart('graficaReservaSinUso', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie',
                    //events: {
                    //    click: function () {
                    //        mdlGraficaDetallesParosReservaSinUso.modal("show");
                    //    }
                    //}
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    margin: 50,
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
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
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        }
                    }
                }, credits: {
                    enabled: false
                },
                series: [{
                    // name: 'Brands',
                    colorByPoint: true,
                    data: lstData,
                    events: {
                        click: function (event) {
                            let codigo = event.point.options.name;
                            fncGetInfoFiltroCodigo(filtro, codigo, 2);
                            mdlGraficaDetallesParosReservaSinUso.modal("show");
                        }
                    }
                }]
            });
            $('.highcharts-title').css("display", "none");
        }
        function initDataTblS_DetallesGraficaParosReservaSinUso() {
            dtDetallesGraficaDL = tblGraficaDetallesParosReservaSinUso.DataTable({
                language: dtDicEsp,
                destroy: true,
                scrollY: "500px",
                scrollCollapse: true,
                paging: true,
                // pageLength: 100,
                columns: [
                    { data: 'codigo', title: 'codigo' },
                    { data: 'economico', title: 'economico' },
                    { data: 'ac', title: 'ac' },
                    { data: 'valor', title: 'Valor' },
                    { data: 'fecha', title: 'fecha' },
                    { data: 'turno', title: 'turno' },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                    // { "width": "13%", "targets": [0, 1, 3] }
                ],
                initComplete: function (settings, json) {
                }
            });
        }
        function fncGetDetallesGraficaParosReservaSinUso(lstValorCodigos, lstCodigosJS, cantDatos) {
            let objData = {};
            let lstData = [];
            for (let i = 0; i < cantDatos; i++) {
                let codigo = lstCodigosJS[i];
                let valor = lstValorCodigos[i];
                objData = {
                    codigo: codigo,
                    valor: valor
                };
                lstData.push(objData);
            }
            dtDetallesGraficaDL.clear();
            dtDetallesGraficaDL.rows.add(lstData);
            dtDetallesGraficaDL.draw();
        }

        // GRAFICA PASTEL PAROS DE DEMORA
        function fncCargarGraficaParosDemora(lstValorCodigos, lstCodigosJS, cantDatos) {
            let objData = new Object();
            let lstData = [];
            for (let i = 0; i < cantDatos; i++) {
                objData = {
                    name: lstCodigosJS[i],
                    y: parseFloat(lstValorCodigos[i]),
                    sliced: true,
                    selected: true
                }
                lstData.push(objData);
            }
            Highcharts.chart('graficaParosDemora', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie',
                    //events: {
                    //    click: function () {
                    //        mdlGraficaDetallesGraficaParosDemora.modal("show");
                    //    }
                    //}$("#cboTurno option:selected").text();
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
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
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                        }
                    }
                }, credits: {
                    enabled: false
                },
                series: [{
                    // name: 'Brands',
                    colorByPoint: true,
                    data: lstData,
                    events: {
                        click: function (event) {
                            let codigo = event.point.options.name;
                            fncGetInfoFiltroCodigo(filtro, codigo, 3);
                            mdlGraficaDetallesGraficaParosDemora.modal("show");
                        }
                    }
                }]
            });
            $('.highcharts-title').css("display", "none");
        }
        function initDataTblS_DetallesGraficaParosDemora() {
            dtGraficaDetallesParosDemora = tblGraficaDetallesParosDemora.DataTable({
                language: dtDicEsp,
                destroy: true,
                scrollY: "500px",
                scrollCollapse: true,
                paging: true,
                // pageLength: 100,
                columns: [
                    { data: 'codigo', title: 'codigo' },
                    { data: 'economico', title: 'economico' },
                    { data: 'ac', title: 'ac' },
                    { data: 'valor', title: 'Valor' },
                    { data: 'fecha', title: 'fecha' },
                    { data: 'turno', title: 'turno' },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                    // { "width": "13%", "targets": [0, 1, 3] }
                ],
                initComplete: function (settings, json) {
                }
            });
        }
        function fncGetDetallesGraficaParosDemoras(lstValorCodigos, lstCodigosJS, cantDatos) {
            let objData = {};
            let lstData = [];
            for (let i = 0; i < cantDatos; i++) {
                let codigo = lstCodigosJS[i];
                let valor = lstValorCodigos[i];
                objData = {
                    codigo: codigo,
                    valor: valor
                };
                lstData.push(objData);
            }
            dtGraficaDetallesParosDemora.clear();
            dtGraficaDetallesParosDemora.rows.add(lstData);
            dtGraficaDetallesParosDemora.draw();
        }
        // #endregion GRAFICAS PASTEL

        // #region GRAFICAS DIAGRAMA DE PARETO DE PAROS PRINCIPALES
        // GRAFICA TIEMPO / MT
        function fncCargarGraficaParetoParosPrincipalesTiempoMT(lstValorCodigos, lstCodigosJS, cantDatos) {
            let arrValorCodigos = [];
            for (let i = 0; i < cantDatos; i++) {
                arrValorCodigos.push(parseFloat(lstValorCodigos[i]));
            }

            let totalFrecuencia = 0;
            for (let i = 0; i < cantDatos; i++) {
                totalFrecuencia += parseFloat(lstValorCodigos[i]);
            }

            let arrFrecuencia = [];
            let arrPorcentaje = [];
            for (let i = 0; i < cantDatos; i++) {
                let porcentaje = 0;
                porcentaje = (parseFloat(lstValorCodigos[i]) / parseFloat(totalFrecuencia)) * 100;
                arrPorcentaje.push(porcentaje == 0 ? 100 : (1 / porcentaje));
                arrFrecuencia.push(80);
            }
            for (let i = 0; i < arrPorcentaje.length; i++) {
                let auxPorcentaje = 0;
                for (let j = 0; j <= i; j++) {
                    auxPorcentaje += arrValorCodigos[j];
                }
                let auxPorcent = (auxPorcentaje / totalFrecuencia * 100);
                 arrPorcentaje[i] ={y: (auxPorcentaje / totalFrecuencia * arrValorCodigos[0]),valormo:auxPorcent};
            }
            arrFrecuencia = [];
            for (let i = 0; i < arrValorCodigos.length; i++) {
                let item = { y: arrValorCodigos[0] * 0.8, valormo: '80 %' }
                arrFrecuencia.push(item);
            }

            Highcharts.chart('graficaParetoParosPrincipalesTiempoMT', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text:
                        '<span><b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val() + '</span>',
                    margin: 50,
                    align: 'center',
                    floating: false,
                    style: {
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                xAxis: {
                    categories: lstCodigosJS,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: arrValorCodigos[0],
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}'
                    }
                },
                tooltip: {
                    // headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    // pointFormat:  '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    //               '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                    // footerFormat: '</table>',
                    shared: true,
                    useHTML: true,

                    formatter: function () {
                        let tblArr = $(this).toArray();
                        let string = '';
                        for (let i = 0; i < tblArr.length; i++) {
                                string += ' <span style="font-size:10px">'+tblArr[0].points[0].key+'</span><table>';
                                for (let b = 0; b < tblArr[i].points.length; b++) {
                                let a = tblArr[i].points[b].point.options.valormo;
                                
                                string += '<tr><td style="color:'+tblArr[i].points[b].series.color+';padding:0">'+tblArr[i].points[b].series.name +': </td>';
                                if (tblArr[i].points[b].series.name == "Tiempo OT") {
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.y+'</b></td></tr>';
                                }else if(tblArr[i].points[b].series.name == "Porcentaje"){
                                    string += '<td style="padding:0"><b>'+ Number(tblArr[i].points[b].point.options.valormo.toFixed(2))+' %</b></td></tr>';
                                }else{
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.valormo+'</b></td></tr>';
                                }
                            }
                            string += '</table>';
                        }
                        return string;
                    },
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    x: 120,
                    verticalAlign: 'top',
                    y: 100,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                }, credits: {
                    enabled: false
                },
                series: [{
                    type: 'column',
                    name: 'Tiempo OT',
                    data: arrValorCodigos
                }, {
                    type: 'spline',
                    name: 'Porcentaje',
                    data: arrPorcentaje
                }, {
                    type: 'spline',
                    name: 'Límite',
                    data: arrFrecuencia
                }]
            });
            $('.highcharts-title').css("display", "none");
        }

        // GRAFICA TIEMPO / ID
        function fncCargarGraficaParetoParosPrincipalesTiempoID(lstValorCodigos, lstCodigosJS, cantDatos) {
            let arrValorCodigos = [];
            for (let i = 0; i < cantDatos; i++) {
                arrValorCodigos.push(parseFloat(lstValorCodigos[i]));
            }

            let totalFrecuencia = 0;
            for (let i = 0; i < cantDatos; i++) {
                totalFrecuencia += parseFloat(lstValorCodigos[i]);
            }
            let arrFrecuencia = [];
            let arrPorcentaje = [];
            for (let i = 0; i < cantDatos; i++) {
                let porcentaje = 0;
                porcentaje = (parseFloat(lstValorCodigos[i]) / parseFloat(totalFrecuencia)) * 100;
                arrPorcentaje.push(porcentaje == 0 ? 100 : (1 / porcentaje));
                arrFrecuencia.push(80);
            }
            arrFrecuencia = [];
            for (let i = 0; i < arrValorCodigos.length; i++) {
                let item = { y: arrValorCodigos[0] * 0.8, valormo: '80 %' }
                arrFrecuencia.push(item);
            }
            for (let i = 0; i < arrPorcentaje.length; i++) {
                let auxPorcentaje = 0;
                for (let j = 0; j <= i; j++) {
                    auxPorcentaje += arrValorCodigos[j];
                }
                let auxPorcent = (auxPorcentaje / totalFrecuencia * 100);
                 arrPorcentaje[i] ={y: (auxPorcentaje / totalFrecuencia * arrValorCodigos[0]),valormo:auxPorcent};
            }

            Highcharts.chart('graficaParetoParosPrincipalesTiempoID', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                xAxis: {
                    categories: lstCodigosJS,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: arrValorCodigos[0],
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}'
                    }
                },
                tooltip: {
                    // headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    // pointFormat:  '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    //               '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                    // footerFormat: '</table>',
                    shared: true,
                    useHTML: true,

                    formatter: function () {
                        let tblArr = $(this).toArray();
                        let string = '';
                        for (let i = 0; i < tblArr.length; i++) {
                                string += ' <span style="font-size:10px">'+tblArr[0].points[0].key+'</span><table>';
                                for (let b = 0; b < tblArr[i].points.length; b++) {
                                let a = tblArr[i].points[b].point.options.valormo;
                                
                                string += '<tr><td style="color:'+tblArr[i].points[b].series.color+';padding:0">'+tblArr[i].points[b].series.name +': </td>';
                                if (tblArr[i].points[b].series.name == "Tiempo OT") {
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.y+'</b></td></tr>';
                                }else if(tblArr[i].points[b].series.name == "Porcentaje"){
                                    string += '<td style="padding:0"><b>'+ Number(tblArr[i].points[b].point.options.valormo.toFixed(2))+' %</b></td></tr>';
                                }else{
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.valormo+'</b></td></tr>';
                                }
                            }
                            string += '</table>';
                        }
                        return string;
                    },
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    x: 120,
                    verticalAlign: 'top',
                    y: 100,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                }, credits: {
                    enabled: false
                },
                series: [{
                    type: 'column',
                    name: 'Tiempo OT',
                    data: arrValorCodigos
                }, {
                    type: 'spline',
                    name: 'Porcentaje',
                    data: arrPorcentaje
                }, {
                    type: 'spline',
                    name: 'Límite',
                    data: arrFrecuencia
                }]
            });
            $('.highcharts-title').css("display", "none");
        }

        // GRAFICA TIEMPO / DL
        function fncCargarGraficaParetoParosPrincipalesTiempoDL(lstValorCodigos, lstCodigosJS, cantDatos) {
            let arrValorCodigos = [];
            for (let i = 0; i < cantDatos; i++) {
                arrValorCodigos.push(parseFloat(lstValorCodigos[i]));
            }

            let totalFrecuencia = 0;
            for (let i = 0; i < cantDatos; i++) {
                totalFrecuencia += parseFloat(lstValorCodigos[i]);
            }

            let arrFrecuencia = [];
            let arrPorcentaje = [];
            for (let i = 0; i < cantDatos; i++) {
                let porcentaje = 0;
                porcentaje = (parseFloat(lstValorCodigos[i]) / parseFloat(totalFrecuencia)) * 100;
                arrPorcentaje.push(porcentaje == 0 ? 100 : (1 / porcentaje));
                arrFrecuencia.push(80);
            }
            arrFrecuencia = [];
            for (let i = 0; i < arrValorCodigos.length; i++) {
                let item = { y: arrValorCodigos[0] * 0.8, valormo: '80 %' }
                arrFrecuencia.push(item);
            }
            for (let i = 0; i < arrPorcentaje.length; i++) {
                let auxPorcentaje = 0;
                for (let j = 0; j <= i; j++) {
                    auxPorcentaje += arrValorCodigos[j];
                }
                let auxPorcent = (auxPorcentaje / totalFrecuencia * 100);
                 arrPorcentaje[i] ={y: (auxPorcentaje / totalFrecuencia * arrValorCodigos[0]),valormo:auxPorcent};
            }

            Highcharts.chart('graficaParetoParosPrincipalesTiempoDL', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                xAxis: {
                    categories: lstCodigosJS,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: arrValorCodigos[0],
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}'
                    }
                },
                tooltip: {
                    // headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    // pointFormat:  '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    //               '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                    // footerFormat: '</table>',
                    shared: true,
                    useHTML: true,

                    formatter: function () {
                        let tblArr = $(this).toArray();
                        let string = '';
                        for (let i = 0; i < tblArr.length; i++) {
                                string += ' <span style="font-size:10px">'+tblArr[0].points[0].key+'</span><table>';
                                for (let b = 0; b < tblArr[i].points.length; b++) {
                                let a = tblArr[i].points[b].point.options.valormo;
                                
                                string += '<tr><td style="color:'+tblArr[i].points[b].series.color+';padding:0">'+tblArr[i].points[b].series.name +': </td>';
                                if (tblArr[i].points[b].series.name == "Tiempo OT") {
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.y+'</b></td></tr>';
                                }else if(tblArr[i].points[b].series.name == "Porcentaje"){
                                    string += '<td style="padding:0"><b>'+ Number(tblArr[i].points[b].point.options.valormo.toFixed(2))+' %</b></td></tr>';
                                }else{
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.valormo+'</b></td></tr>';
                                }
                            }
                            string += '</table>';
                        }
                        return string;
                    },
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    x: 120,
                    verticalAlign: 'top',
                    y: 100,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                }, credits: {
                    enabled: false
                },
                series: [{
                    type: 'column',
                    name: 'Tiempo OT',
                    data: arrValorCodigos
                }, {
                    type: 'spline',
                    name: 'Porcentaje',
                    data: arrPorcentaje
                }, {
                    type: 'spline',
                    name: 'Límite',
                    data: arrFrecuencia
                }]
            });
            $('.highcharts-title').css("display", "none");
        }

        function OrdenarLstPor(a, b) {
            var aValor = a.valor;
            var bValor = b.valor;
            return ((aValor > bValor) ? -1 : ((aValor < bValor) ? 1 : 0));
        }

        // GRAFICA TIEMPO / GENERAL
        function fncCargarGraficaParetoParosPrincipalesTiempoGeneral(lstValorCodigosMT, lstCodigosJSMT, cantDatosMT,
            lstValorCodigosID, lstCodigosJSID, cantDatosID,
            lstValorCodigosDL, lstCodigosJSDL, cantDatosDL) {
            let arrFinal = [];
            let arrCodigos = [];
            let totalFrecuencia = 0;
            let arrFrecuenciaCodigos =[];
            for (let i = 0; i < cantDatosMT; i++) {
                arrCodigos.push(lstCodigosJSMT[i] + " MT");
            }
            for (let i = 0; i < cantDatosID; i++) {
                arrCodigos.push(lstCodigosJSID[i] + " ID");
            }
            for (let i = 0; i < cantDatosDL; i++) {
                arrCodigos.push(lstCodigosJSDL[i] + " DL");
            }

            let arrValorCodigos = [];
            for (let i = 0; i < cantDatosMT; i++) {
                arrValorCodigos.push(parseFloat(lstValorCodigosMT[i]));
            }
            for (let i = 0; i < cantDatosID; i++) {
                arrValorCodigos.push(parseFloat(lstValorCodigosID[i]));
            }
            for (let i = 0; i < cantDatosDL; i++) {
                arrValorCodigos.push(parseFloat(lstValorCodigosDL[i]));
            }

            let arrFrecuencia = [];
            let arrPorcentaje = [];
            let totalFrecuenciaMT = 0;
            for (let i = 0; i < cantDatosMT; i++) {
                totalFrecuenciaMT += parseFloat(lstValorCodigosMT[i]);
                totalFrecuencia += parseFloat(lstValorCodigosMT[i]);
            }
            let totalFrecuenciaID = 0;
            for (let i = 0; i < cantDatosID; i++) {
                totalFrecuenciaID += parseFloat(lstValorCodigosID[i]);
                totalFrecuencia += parseFloat(lstValorCodigosID[i]);
            }
            let totalFrecuenciaDL = 0;
            for (let i = 0; i < cantDatosDL; i++) {
                totalFrecuenciaDL += parseFloat(lstValorCodigosDL[i]);
                totalFrecuencia += parseFloat(lstValorCodigosDL[i]);
            }
            for (let i = 0; i < cantDatosMT; i++) {
                let porcentaje = 0;
                porcentaje = (parseFloat(lstValorCodigosMT[i]) / parseFloat(totalFrecuencia)) * 100;
                arrPorcentaje.push(porcentaje == 0 ? 100 : (1 / porcentaje));
                let item = { y: 80, valormo: '80 %' }
                arrFrecuencia.push(item);
                let aux = { codigo: lstCodigosJSMT[i] + " MT", valor: parseFloat(lstValorCodigosMT[i]), porcentaje: porcentaje == 0 ? -1 : (1 / parseFloat(lstValorCodigosMT[i])), frecuencia: 80, };
                arrFinal.push(aux);
            }


            for (let i = 0; i < cantDatosID; i++) {
                let porcentaje = 0;
                porcentaje = (parseFloat(lstValorCodigosID[i]) / parseFloat(totalFrecuencia)) * 100;
                arrPorcentaje.push(porcentaje == 0 ? 100 : (1 / porcentaje));
                let item = { y: 80, valormo: '80 %' }
                arrFrecuencia.push(item);
                let aux = { codigo: lstCodigosJSID[i] + " ID", valor: parseFloat(lstValorCodigosID[i]), porcentaje: porcentaje == 0 ? -1 : (1 / parseFloat(lstValorCodigosID[i])), frecuencia: 80, };
                arrFinal.push(aux);
            }

            for (let i = 0; i < cantDatosDL; i++) {
                let porcentaje = 0;
                porcentaje = (parseFloat(lstValorCodigosDL[i]) / parseFloat(totalFrecuencia)) * 100;
                arrPorcentaje.push(porcentaje);
                let item = { y: 80, valormo: '80 %' }
                arrFrecuencia.push(item);
                let aux = { codigo: lstCodigosJSDL[i] + " DL", valor: parseFloat(lstValorCodigosDL[i]), porcentaje: porcentaje == 0 ? -1 : (1 / parseFloat(lstValorCodigosDL[i])), frecuencia: 80, };
                arrFinal.push(aux);
            }

            arrFinal.sort(OrdenarLstPor);
            arrCodigos = arrFinal.map(function (e) {
                return e.codigo;
            });

            arrPorcentaje = arrFinal.map(function (e) {
                return e.porcentaje;
            });
            arrValorCodigos = arrFinal.map(function (e) {
                return e.valor;
            });
            arrFrecuencia = [];
            for (let i = 0; i < arrValorCodigos.length; i++) {
                let item = { y: arrValorCodigos[0] * 0.8, valormo: '80 %' }
                arrFrecuencia.push(item);
            }
            for (let i = 0; i < arrPorcentaje.length; i++) {
                let auxPorcentaje = 0;
                for (let j = 0; j <= i; j++) {
                    auxPorcentaje += arrValorCodigos[j];
                }
                
               let auxPorcent = (auxPorcentaje / totalFrecuencia * 100);
                arrPorcentaje[i] ={y: (auxPorcentaje / totalFrecuencia * arrValorCodigos[0]),valormo:auxPorcent};
            }

            Highcharts.chart('graficaParetoParosPrincipalesTiempoGeneral', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                xAxis: [{
                    categories: arrCodigos,
                    crosshair: true
                }],
                yAxis: {
                    min: 0,
                    max: arrValorCodigos[0],
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}'
                    }
                },
                tooltip: {
                  // headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    // pointFormat:  '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    //               '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                    // footerFormat: '</table>',
                    shared: true,
                    useHTML: true,

                    formatter: function () {
                        let tblArr = $(this).toArray();
                        let string = '';
                        for (let i = 0; i < tblArr.length; i++) {
                                string += ' <span style="font-size:10px">'+tblArr[0].points[0].key+'</span><table>';
                                for (let b = 0; b < tblArr[i].points.length; b++) {
                                let a = tblArr[i].points[b].point.options.valormo;
                                
                                string += '<tr><td style="color:'+tblArr[i].points[b].series.color+';padding:0">'+tblArr[i].points[b].series.name +': </td>';
                                if (tblArr[i].points[b].series.name == "Tiempo OT") {
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.y+'</b></td></tr>';
                                }else if(tblArr[i].points[b].series.name == "Frecuencia"){
                                    string += '<td style="padding:0"><b>'+ Number(tblArr[i].points[b].point.options.valormo.toFixed(2))+' </b></td></tr>';
                                }else{
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.valormo+'</b></td></tr>';
                                }
                            }
                            string += '</table>';
                        }
                        return string;
                    },
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    x: 120,
                    verticalAlign: 'top',
                    y: 100,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                }, credits: {
                    enabled: false
                },
                series: [{
                    type: 'column',
                    name: 'Tiempo OT',
                    data: arrValorCodigos
                }, {
                    type: 'spline',
                    name: 'Frecuencia',
                    data: arrPorcentaje
                }, {
                    type: 'spline',
                    name: 'Límite',
                    data: arrFrecuencia
                }]
            });
            $('.highcharts-title').css("display", "none");
        }

        // GRAFICA FRECUENCIA / MT
        function fncCargarGraficaParetoParosPrincipalesFrecuenciaMT(lstCodigosJS, lstFrecuenciaCodigosParo, cantDatos, lstValorCodigos) {
            let arrFrecuenciaCodigos = [];
            for (let i = 0; i < cantDatos; i++) {
                arrFrecuenciaCodigos.push(parseFloat(lstFrecuenciaCodigosParo[i]));
            }

            let totalFrecuencia = 0;
            for (let i = 0; i < cantDatos; i++) {
                totalFrecuencia += parseFloat(arrFrecuenciaCodigos[i]);
            }

            let arrFrecuencia = [];
            let arrPorcentaje = [];
            for (let i = 0; i < cantDatos; i++) {
                let porcentaje = 0;
                porcentaje = (parseFloat(lstValorCodigos[i]) / parseFloat(totalFrecuencia)) * 100;
                arrPorcentaje.push(porcentaje == 0 ? 100 : (1 / porcentaje));
                let item = { y: 80, valormo: '80 %' }
                arrFrecuencia.push(item);
            }
            arrFrecuencia = [];
            for (let i = 0; i < arrFrecuenciaCodigos.length; i++) {
                let item = { y: arrFrecuenciaCodigos[0] * 0.8, valormo: '80 %' }
                arrFrecuencia.push(item);
            }
            for (let i = 0; i < arrPorcentaje.length; i++) {
                let auxPorcentaje = 0;
                for (let j = 0; j <= i; j++) {
                    auxPorcentaje += arrFrecuenciaCodigos[j];
                }
                let auxPorcent = (auxPorcentaje / totalFrecuencia * 100);
                arrPorcentaje[i] ={y: (auxPorcentaje / totalFrecuencia * arrFrecuenciaCodigos[0]),valormo:auxPorcent};
            }
            Highcharts.chart('graficaParetoParosPrincipalesFrecuenciaMT', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                xAxis: [{
                    categories: lstCodigosJS,
                    crosshair: true
                }],
                yAxis: {
                    min: 0,
                    max: arrFrecuenciaCodigos[0],
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}'
                    }
                },
                tooltip: {
                  // headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    // pointFormat:  '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    //               '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                    // footerFormat: '</table>',
                    shared: true,
                    useHTML: true,

                    formatter: function () {
                        let tblArr = $(this).toArray();
                        let string = '';
                        for (let i = 0; i < tblArr.length; i++) {
                                string += ' <span style="font-size:10px">'+tblArr[0].points[0].key+'</span><table>';
                                for (let b = 0; b < tblArr[i].points.length; b++) {
                                let a = tblArr[i].points[b].point.options.valormo;

                                string += '<tr><td style="color:'+tblArr[i].points[b].series.color+';padding:0">'+tblArr[i].points[b].series.name +': </td>';
                                if (tblArr[i].points[b].series.name == "Frecuencia") {
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.y+'</b></td></tr>';
                                }else if(tblArr[i].points[b].series.name == "Porcentaje"){
                                    string += '<td style="padding:0"><b>'+ Number(tblArr[i].points[b].point.options.valormo.toFixed(2))+' %</b></td></tr>';
                                }else{
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.valormo+'</b></td></tr>';
                                }
                            }
                            string += '</table>';
                        }
                        return string;
                    },
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    x: 120,
                    verticalAlign: 'top',
                    y: 100,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                }, credits: {
                    enabled: false
                },
                series: [{
                    type: 'column',
                    name: 'Frecuencia',
                    data: arrFrecuenciaCodigos
                }, {
                    type: 'spline',
                    name: 'Porcentaje',
                    data: arrPorcentaje
                }, {
                    type: 'spline',
                    name: 'Límite',
                    data: arrFrecuencia
                }]
            });
            $('.highcharts-title').css("display", "none");
        }

        // GRAFICA FRECUENCIA / ID
        function fncCargarGraficaParetoParosPrincipalesFrecuenciaID(lstCodigosJS, lstFrecuenciaCodigosParo, cantDatos, lstValorCodigos) {
            let arrFrecuenciaCodigos = [];
            for (let i = 0; i < cantDatos; i++) {
                arrFrecuenciaCodigos.push(parseFloat(lstFrecuenciaCodigosParo[i]));
            }

            let totalFrecuencia = 0;
            for (let i = 0; i < cantDatos; i++) {
                totalFrecuencia += parseFloat(arrFrecuenciaCodigos[i]);
            }

            let arrFrecuencia = [];
            let arrPorcentaje = [];
            for (let i = 0; i < cantDatos; i++) {
                let porcentaje = 0;
                porcentaje = (parseFloat(lstValorCodigos[i]) / parseFloat(totalFrecuencia)) * 100;
                arrPorcentaje.push(porcentaje == 0 ? 100 : (1 / porcentaje));
                let item = { y: 80, valormo: '80 %' }
                arrFrecuencia.push(item);
            }
            arrFrecuencia = [];
            for (let i = 0; i < arrFrecuenciaCodigos.length; i++) {
                let item = { y: arrFrecuenciaCodigos[0] * 0.8, valormo: '80 %' }
                arrFrecuencia.push(item);
            }
            for (let i = 0; i < arrPorcentaje.length; i++) {
                let auxPorcentaje = 0;
                for (let j = 0; j <= i; j++) {
                    auxPorcentaje += arrFrecuenciaCodigos[j];
                }
               let  auxPorcent = (auxPorcentaje / totalFrecuencia * 100);
                arrPorcentaje[i] ={y: (auxPorcentaje / totalFrecuencia * arrFrecuenciaCodigos[0]),valormo:auxPorcent};
            }

            Highcharts.chart('graficaParetoParosPrincipalesFrecuenciaID', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                xAxis: [{
                    categories: lstCodigosJS,
                    crosshair: true
                }],
                yAxis: {
                    min: 0,
                    max: arrFrecuenciaCodigos[0],
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}'
                    }
                },
                tooltip: {
                // headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    // pointFormat:  '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    //               '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                    // footerFormat: '</table>',
                    shared: true,
                    useHTML: true,

                    formatter: function () {
                        let tblArr = $(this).toArray();
                        let string = '';
                        for (let i = 0; i < tblArr.length; i++) {
                                string += ' <span style="font-size:10px">'+tblArr[0].points[0].key+'</span><table>';
                                for (let b = 0; b < tblArr[i].points.length; b++) {
                                let a = tblArr[i].points[b].point.options.valormo;

                                string += '<tr><td style="color:'+tblArr[i].points[b].series.color+';padding:0">'+tblArr[i].points[b].series.name +': </td>';
                                if (tblArr[i].points[b].series.name == "Frecuencia") {
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.y+'</b></td></tr>';
                                }else if(tblArr[i].points[b].series.name == "Porcentaje"){
                                    string += '<td style="padding:0"><b>'+ Number(tblArr[i].points[b].point.options.valormo.toFixed(2))+' %</b></td></tr>';
                                }else{
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.valormo+'</b></td></tr>';
                                }
                            }
                            string += '</table>';
                        }
                        return string;
                    },
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    x: 120,
                    verticalAlign: 'top',
                    y: 100,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                }, credits: {
                    enabled: false
                },
                series: [{
                    type: 'column',
                    name: 'Frecuencia',
                    data: arrFrecuenciaCodigos
                }, {
                    type: 'spline',
                    name: 'Porcentaje',
                    data: arrPorcentaje
                }, {
                    type: 'spline',
                    name: 'Límite',
                    data: arrFrecuencia
                }]
            });
            $('.highcharts-title').css("display", "none");
        }

        // GRAFICA FRECUENCIA / DL
        function fncCargarGraficaParetoParosPrincipalesFrecuenciaDL(lstCodigosJS, lstFrecuenciaCodigosParo, cantDatos, lstValorCodigos) {
            let arrFrecuenciaCodigos = [];
            for (let i = 0; i < cantDatos; i++) {
                arrFrecuenciaCodigos.push(parseFloat(lstFrecuenciaCodigosParo[i]));
            }

            let totalFrecuencia = 0;
            for (let i = 0; i < cantDatos; i++) {
                totalFrecuencia += parseFloat(arrFrecuenciaCodigos[i]);
            }

            let arrFrecuencia = [];
            let arrPorcentaje = [];
            for (let i = 0; i < cantDatos; i++) {
                let porcentaje = 0;
                porcentaje = (parseFloat(lstValorCodigos[i]) / parseFloat(totalFrecuencia)) * 100;
                arrPorcentaje.push(porcentaje == 0 ? 100 : (1 / porcentaje));
                let item = { y: 80, valormo: '80 %' }
                arrFrecuencia.push(item);
            }
            arrFrecuencia = [];
            for (let i = 0; i < arrFrecuenciaCodigos.length; i++) {
                let item = { y: arrFrecuenciaCodigos[0] * 0.8, valormo: '80 %' }
                arrFrecuencia.push(item);
            }
            for (let i = 0; i < arrPorcentaje.length; i++) {
                let auxPorcentaje = 0;
                for (let j = 0; j <= i; j++) {
                    auxPorcentaje += arrFrecuenciaCodigos[j];
                }
               let auxPorcent = (auxPorcentaje / totalFrecuencia * 100);
                arrPorcentaje[i] ={y: (auxPorcentaje / totalFrecuencia * arrFrecuenciaCodigos[0]),valormo:auxPorcent};
            }

            Highcharts.chart('graficaParetoParosPrincipalesFrecuenciaDL', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                xAxis: [{
                    categories: lstCodigosJS,
                    crosshair: true
                }],
                yAxis: {
                    min: 0,
                    max: arrFrecuenciaCodigos[0],
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}'
                    }
                },
                tooltip: {
                     // headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    // pointFormat:  '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    //               '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                    // footerFormat: '</table>',
                    shared: true,
                    useHTML: true,

                    formatter: function () {
                        let tblArr = $(this).toArray();
                        let string = '';
                        for (let i = 0; i < tblArr.length; i++) {
                                string += ' <span style="font-size:10px">'+tblArr[0].points[0].key+'</span><table>';
                                for (let b = 0; b < tblArr[i].points.length; b++) {
                                let a = tblArr[i].points[b].point.options.valormo;

                                string += '<tr><td style="color:'+tblArr[i].points[b].series.color+';padding:0">'+tblArr[i].points[b].series.name +': </td>';
                                if (tblArr[i].points[b].series.name == "Frecuencia") {
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.y+'</b></td></tr>';
                                }else if(tblArr[i].points[b].series.name == "Porcentaje"){
                                    string += '<td style="padding:0"><b>'+ Number(tblArr[i].points[b].point.options.valormo.toFixed(2))+' %</b></td></tr>';
                                }else{
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.valormo+'</b></td></tr>';
                                }
                            }
                            string += '</table>';
                        }
                        return string;
                    },
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    x: 120,
                    verticalAlign: 'top',
                    y: 100,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                }, credits: {
                    enabled: false
                },
                series: [{
                    type: 'column',
                    name: 'Frecuencia',
                    data: arrFrecuenciaCodigos
                }, {
                    type: 'spline',
                    name: 'Porcentaje',
                    data: arrPorcentaje
                }, {
                    type: 'spline',
                    name: 'Límite',
                    data: arrFrecuencia
                }]
            });
            $('.highcharts-title').css("display", "none");
        }

        // GRAFICA FRECUENCIA / GENERAL
        function fncCargarGraficaParetoParosPrincipalesFrecuenciaGeneral(lstCodigosJSMT, lstValorCodigosMT, lstFrecuenciaCodigosParoMT, cantDatosMT,
            lstCodigosJSID, lstValorCodigosID, lstFrecuenciaCodigosParoID, cantDatosID,
            lstCodigosJSDL, lstValorCodigosDL, lstFrecuenciaCodigosParoDL, cantDatosDL) {
            let arrCodigos = [];
            let arrFinal = [];
            for (let i = 0; i < cantDatosMT; i++) {
                arrCodigos.push(lstCodigosJSMT[i] + " MT");
            }
            for (let i = 0; i < cantDatosID; i++) {
                arrCodigos.push(lstCodigosJSID[i] + " ID");
            }
            for (let i = 0; i < cantDatosDL; i++) {
                arrCodigos.push(lstCodigosJSDL[i] + " DL");
            }

            let arrFrecuenciaCodigos = [];
            for (let i = 0; i < cantDatosMT; i++) {
                arrFrecuenciaCodigos.push(parseFloat(lstFrecuenciaCodigosParoMT[i]));
            }
            for (let i = 0; i < cantDatosID; i++) {
                arrFrecuenciaCodigos.push(parseFloat(lstFrecuenciaCodigosParoID[i]));
            }
            for (let i = 0; i < cantDatosDL; i++) {
                arrFrecuenciaCodigos.push(parseFloat(lstFrecuenciaCodigosParoDL[i]));
            }

            let arrFrecuencia = [];
            let arrPorcentaje = [];
            let totalFrecuencia = 0;
            let totalFrecuenciaMT = 0;
            for (let i = 0; i < cantDatosMT; i++) {
                totalFrecuenciaMT += parseFloat(lstFrecuenciaCodigosParoMT[i]);
                totalFrecuencia += parseFloat(lstFrecuenciaCodigosParoMT[i]);
            }
            let totalFrecuenciaID = 0;
            for (let i = 0; i < cantDatosID; i++) {
                totalFrecuenciaID += parseFloat(lstFrecuenciaCodigosParoID[i]);
                totalFrecuencia += parseFloat(lstFrecuenciaCodigosParoID[i]);
            }
            let totalFrecuenciaDL = 0;
            for (let i = 0; i < cantDatosDL; i++) {
                totalFrecuenciaDL += parseFloat(lstFrecuenciaCodigosParoDL[i]);
                totalFrecuencia += parseFloat(lstFrecuenciaCodigosParoDL[i]);
            }
            for (let i = 0; i < cantDatosMT; i++) {
                let porcentaje = 0;
                porcentaje = (parseFloat(lstFrecuenciaCodigosParoMT[i]) / parseFloat(totalFrecuencia)) * 100;
                arrPorcentaje.push(porcentaje == 0 ? 100 : (1 / porcentaje));
                let item = { y: 80, valormo: '80 %' }
                arrFrecuencia.push(item);
                let aux = { codigo: lstCodigosJSMT[i] + " MT", valor: parseFloat(lstFrecuenciaCodigosParoMT[i]), porcentaje: porcentaje == 0 ? -1 : (1 / parseFloat(lstFrecuenciaCodigosParoMT[i])), frecuencia: 80, };
                arrFinal.push(aux);
            }


            for (let i = 0; i < cantDatosID; i++) {
                let porcentaje = 0;
                porcentaje = (parseFloat(lstFrecuenciaCodigosParoID[i]) / parseFloat(totalFrecuencia)) * 100;
                arrPorcentaje.push(porcentaje == 0 ? 100 : (1 / porcentaje));
                let item = { y: 80, valormo: '80 %' }
                arrFrecuencia.push(item);
                let aux = { codigo: lstCodigosJSID[i] + " ID", valor: parseFloat(lstFrecuenciaCodigosParoID[i]), porcentaje: porcentaje == 0 ? -1 : (1 / parseFloat(lstFrecuenciaCodigosParoID[i])), frecuencia: 80, };
                arrFinal.push(aux);
            }


            for (let i = 0; i < cantDatosDL; i++) {
                let porcentaje = 0;
                porcentaje = (parseFloat(lstFrecuenciaCodigosParoDL[i]) / parseFloat(totalFrecuencia)) * 100;
                arrPorcentaje.push(porcentaje == 0 ? 100 : (1 / porcentaje));
                let item = { y: 80, valormo: '80 %' }
                arrFrecuencia.push(item);
                let aux = { codigo: lstCodigosJSDL[i] + " DL", valor: parseFloat(lstFrecuenciaCodigosParoDL[i]), porcentaje: porcentaje == 0 ? -1 : (1 / parseFloat(lstFrecuenciaCodigosParoDL[i])), frecuencia: 80, };
                arrFinal.push(aux);
            }
            arrFinal.sort(OrdenarLstPor);
            arrCodigos = arrFinal.map(function (e) {
                return e.codigo;
            });

            arrPorcentaje = arrFinal.map(function (e) {
                return e.porcentaje;
            });
            arrFrecuenciaCodigos = arrFinal.map(function (e) {
                return e.valor;
            });
            arrFrecuencia = [];
            for (let i = 0; i < arrFrecuenciaCodigos.length; i++) {
                let item = { y: arrFrecuenciaCodigos[0] * 0.8, valormo: '80 %' }
                arrFrecuencia.push(item);
            }
            for (let i = 0; i < arrPorcentaje.length; i++) {
                let auxPorcentaje = 0;
                for (let j = 0; j <= i; j++) {
                    auxPorcentaje += arrFrecuenciaCodigos[j];
                }
                let auxPorcent = (auxPorcentaje / totalFrecuencia * 100);
                arrPorcentaje[i] = { y: (auxPorcentaje / totalFrecuencia * arrFrecuenciaCodigos[0]), valormo: auxPorcent };
            }
           



            Highcharts.chart('graficaParetoParosPrincipalesFrecuenciaGeneral', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text:
                        '<b>AREA CUENTA:</b> ' + $("#cboAreaCuenta option:selected").text() +
                        ' <br/><b>GRUPO:</b> ' + $(".multiselect-selected-text").eq(0).text() +
                        ' <br/><b>MODELO:</b> ' + $(".multiselect-selected-text").eq(1).text() +
                        ' <br/><b>ECONOMICO:</b> ' + $(".multiselect-selected-text").eq(2).text() +
                        ' <br/><b>TURNO:</b> ' + $("#cboTurno option:selected").text() +
                        ' <b>FECHAS:</b> ' + txtFechaInicio.val() + ' - ' + txtFechaFin.val(),
                    align: 'center',
                    floating: false,
                    style: {
                        margin: '50px', // does not work for some reasons, see workaround below
                        color: '#707070',
                        fontSize: '12px',
                        fontWeight: 'normal',
                        textTransform: 'none'
                    }
                },
                xAxis: [{
                    categories: arrCodigos,
                    crosshair: true
                }],
                yAxis: {
                    min: 0,
                    max: arrFrecuenciaCodigos[0],
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}'
                    }
                },
                tooltip: {
                    // headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    // pointFormat:  '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    //               '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                    // footerFormat: '</table>',
                    shared: true,
                    useHTML: true,

                    formatter: function () {
                        let tblArr = $(this).toArray();
                        let string = '';
                        for (let i = 0; i < tblArr.length; i++) {
                                string += ' <span style="font-size:10px">'+tblArr[0].points[0].key+'</span><table>';
                                for (let b = 0; b < tblArr[i].points.length; b++) {
                                let a = tblArr[i].points[b].point.options.valormo;

                                string += '<tr><td style="color:'+tblArr[i].points[b].series.color+';padding:0">'+tblArr[i].points[b].series.name +': </td>';
                                if (tblArr[i].points[b].series.name == "Frecuencia") {
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.y+'</b></td></tr>';
                                }else if(tblArr[i].points[b].series.name == "Porcentaje"){
                                    string += '<td style="padding:0"><b>'+ Number(tblArr[i].points[b].point.options.valormo.toFixed(2))+' %</b></td></tr>';
                                }else{
                                    string += '<td style="padding:0"><b>'+tblArr[i].points[b].point.options.valormo+'</b></td></tr>';
                                }
                            }
                            string += '</table>';
                        }
                        return string;
                    },
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    x: 120,
                    verticalAlign: 'top',
                    y: 100,
                    floating: true,
                    backgroundColor:
                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                        'rgba(255,255,255,0.25)'
                }, credits: {
                    enabled: false
                },
                series: [{
                    type: 'column',
                    name: 'Frecuencia',
                    data: arrFrecuenciaCodigos
                }, {
                    type: 'spline',
                    name: 'Porcentaje',
                    data: arrPorcentaje
                }, {
                    type: 'spline',
                    name: 'Límite',
                    data: arrFrecuencia
                }]
            });
            $('.highcharts-title').css("display", "none");
        }
        // #endregion GRAFICAS DIAGRAMA DE PARETO DE PAROS PRINCIPALES

        function fncGetInfoFiltro(filtro) {
            if (
                filtro.areaCuenta != "" &&
                filtro.fechaFin != "" &&
                filtro.fechaInicio != "" &&
                filtro.idEconomicos.length > 0 &&
                filtro.idGrupos.length > 0 &&
                filtro.idModelos.length > 0
            ) {
                $.post('/KPI/GetInfoFiltro', {
                    filtro: filtro
                }).then(response => {
                    if (response.success) {
                        if (!response.noExcel) {
                            btnExcel.removeAttr('disabled');
                        }
                        fncFillTablaTiempos(response.tiempos);

                        fncCargarGraficaDisponibilidadUtilizacion_grupo(response.gpx_disVsUti_grupo);
                        fncCargarGraficaDisponibilidadUtilizacion_modelo(response.gpx_disVsUti_modelo);
                        fncCargarGraficaDisponibilidadUtilizacion_economico(response.gpx_disVsUti_economico);
                        fncCargarGraficaDisponibilidadUtilizacion_semanal(response.gpx_disVsUti_semanal);
                        fncCargarGraficaDisponibilidadUtilizacion_mensual(response.gpx_disVsUti_mensual);

                        fncCargarGraficaOperacionTrabajo_grupo(response.gpx_opeVsTra_grupo);
                        fncCargarGraficaOperacionTrabajo_modelo(response.gpx_opeVsTra_modelo);
                        fncCargarGraficaOperacionTrabajo_economico(response.gpx_opeVsTra_economico);
                        fncCargarGraficaOperacionTrabajo_semanal(response.gpx_opeVsTra_semanal);
                        fncCargarGraficaOperacionTrabajo_mensual(response.gpx_opeVsTra_mensual);

                        fncCargarGraficaUT_grupo(response.gpx_UT_grupo);
                        fncCargarGraficaUT_modelo(response.gpx_UT_modelo);
                        fncCargarGraficaUT_economico(response.gpx_UT_economico);
                        fncCargarGraficaUT_semanal(response.gpx_UT_semanal);
                        fncCargarGraficaUT_mensual(response.gpx_UT_mensual);

                        txtDisponibilidadAnual.text(response.anual.disponible + '%');
                        txtUtilizacionAnual.text(response.anual.operacion + '%');
                        txtEficienciaAnual.text(response.anual.trabajo + '%');
                        txtHorasAnual.text(response.anual.horas);

                        //#region GRAFICAS PASTEL
                        fncCargarGraficaParosMantenimiento(
                            response.resultadosGraficaMT.lstValorCodigos,
                            response.resultadosGraficaMT.lstCodigosJS,
                            response.resultadosGraficaMT.lstValorCodigos.length);
                        fncCargarGraficaParosReservaSinUso(
                            response.resultadosGraficaID.lstValorCodigos,
                            response.resultadosGraficaID.lstCodigosJS,
                            response.resultadosGraficaID.lstValorCodigos.length);
                        fncCargarGraficaParosDemora(
                            response.resultadosGraficaDL.lstValorCodigos,
                            response.resultadosGraficaDL.lstCodigosJS,
                            response.resultadosGraficaDL.lstValorCodigos.length);
                        //#endregion

                        //#region INIT DETALLES DE GRAFICAS PASTEL
                        initDataTblS_DetallesGraficaParosMantenimiento();
                        //fncGetDetallesGraficaParosMantenimiento(
                        //    response.resultadosGraficaMT.lstValorCodigos,
                        //    response.resultadosGraficaMT.lstCodigosJS,
                        //    response.resultadosGraficaMT.lstValorCodigos.length
                        //);
                        initDataTblS_DetallesGraficaParosReservaSinUso();
                        //fncGetDetallesGraficaParosReservaSinUso(
                        //    response.resultadosGraficaID.lstValorCodigos,
                        //    response.resultadosGraficaID.lstCodigosJS,
                        //    response.resultadosGraficaID.lstValorCodigos.length
                        //);
                        initDataTblS_DetallesGraficaParosDemora();
                        //fncGetDetallesGraficaParosDemoras(
                        //    response.resultadosGraficaDL.lstValorCodigos,
                        //    response.resultadosGraficaDL.lstCodigosJS,
                        //    response.resultadosGraficaDL.lstValorCodigos.length
                        //);
                        //#endregion

                        //#region GRAFICAS DE PARETO DE PAROS PRINCIPALES / TIEMPO
                        fncCargarGraficaParetoParosPrincipalesTiempoMT(response.resultadosGraficaMT.lstValorCodigos,
                            response.resultadosGraficaMT.lstCodigosJS, response.resultadosGraficaMT.lstValorCodigos.length);
                        fncCargarGraficaParetoParosPrincipalesTiempoID(response.resultadosGraficaID.lstValorCodigos,
                            response.resultadosGraficaID.lstCodigosJS, response.resultadosGraficaID.lstValorCodigos.length);
                        fncCargarGraficaParetoParosPrincipalesTiempoDL(response.resultadosGraficaDL.lstValorCodigos,
                            response.resultadosGraficaDL.lstCodigosJS, response.resultadosGraficaDL.lstValorCodigos.length);
                        fncCargarGraficaParetoParosPrincipalesTiempoGeneral(
                            response.resultadosGraficaMT.lstValorCodigos, response.resultadosGraficaMT.lstCodigosJS, response.resultadosGraficaMT.lstValorCodigos.length,
                            response.resultadosGraficaID.lstValorCodigos, response.resultadosGraficaID.lstCodigosJS, response.resultadosGraficaID.lstValorCodigos.length,
                            response.resultadosGraficaDL.lstValorCodigos, response.resultadosGraficaDL.lstCodigosJS, response.resultadosGraficaDL.lstValorCodigos.length);
                        //#endregion

                        //#region GRAFICAS DE PARETO DE PAROS PRINCIPALES / FRECUENCIA
                        fncCargarGraficaParetoParosPrincipalesFrecuenciaMT(response.resultadosGraficaMT.lstCodigosJS,
                            response.resultadosGraficaMT.lstFrecuenciaCodigosParo, response.resultadosGraficaMT.lstValorCodigos.length,
                            response.resultadosGraficaMT.lstValorCodigos);
                        fncCargarGraficaParetoParosPrincipalesFrecuenciaID(response.resultadosGraficaID.lstCodigosJS,
                            response.resultadosGraficaID.lstFrecuenciaCodigosParo, response.resultadosGraficaID.lstValorCodigos.length,
                            response.resultadosGraficaID.lstValorCodigos);
                        fncCargarGraficaParetoParosPrincipalesFrecuenciaDL(response.resultadosGraficaDL.lstCodigosJS,
                            response.resultadosGraficaDL.lstFrecuenciaCodigosParo, response.resultadosGraficaDL.lstValorCodigos.length,
                            response.resultadosGraficaDL.lstValorCodigos);
                        fncCargarGraficaParetoParosPrincipalesFrecuenciaGeneral(
                            response.resultadosGraficaMT.lstCodigosJS, response.resultadosGraficaMT.lstValorCodigos, response.resultadosGraficaMT.lstFrecuenciaCodigosParo, response.resultadosGraficaMT.lstValorCodigos.length,
                            response.resultadosGraficaID.lstCodigosJS, response.resultadosGraficaID.lstValorCodigos, response.resultadosGraficaID.lstFrecuenciaCodigosParo, response.resultadosGraficaID.lstValorCodigos.length,
                            response.resultadosGraficaDL.lstCodigosJS, response.resultadosGraficaDL.lstValorCodigos, response.resultadosGraficaDL.lstFrecuenciaCodigosParo, response.resultadosGraficaDL.lstValorCodigos.length
                        );
                        $('.highcharts-title').css("display", "none");
                        //#endregion
                    }
                    else {
                        AlertaGeneral('Alerta', response.message)
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
            }
            else {
                Alert2Warning("Es necesario seleccionar todos los filtros");
            }
        }
        function fncGetInfoFiltroCodigo(filtro, codigo, tipo) {

            $.post('/KPI/GetInfoFiltroCodigo', {
                filtro: filtro,
                codigo: codigo,
                tipo: tipo
            }).then(response => {
                if (response.success) {
                    switch (tipo) {
                        case 1:
                            dtGraficaDetallesParosMantenimiento.clear();
                            dtGraficaDetallesParosMantenimiento.rows.add(response.lst);
                            dtGraficaDetallesParosMantenimiento.draw();
                            break;
                        case 2:
                            dtDetallesGraficaDL.clear();
                            dtDetallesGraficaDL.rows.add(response.lst);
                            dtDetallesGraficaDL.draw();
                            break;
                        case 3:
                            dtGraficaDetallesParosDemora.clear();
                            dtGraficaDetallesParosDemora.rows.add(response.lst);
                            dtGraficaDetallesParosDemora.draw();
                            break;
                    }
                }
                else {
                    AlertaGeneral('Alerta', response.message)
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }
    }

    $(document).ready(() => Maquinaria.KPI.DashboardKPI = new DashboardKPI())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();