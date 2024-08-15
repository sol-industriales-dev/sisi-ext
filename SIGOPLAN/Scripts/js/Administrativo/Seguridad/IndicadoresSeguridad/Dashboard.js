(() => {
    $.namespace('Dashboard.Seguridad');
    Seguridad = function () {
        //#region Variables.
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);
        let chartIncidentesRegistrables
        let chartIncidentesReportables
        let chartPotencialSeveridad
        let chartIncidentesMes
        let chartIncidentesRegistrablesMesAnterior
        let chartIncidentesRegistrablesMesActual
        let chartTasaIncidencias
        let chartDanoInstalacionEquipo
        let chartIncidentesDepartamento
        let chartTIFR
        let chartTPDFR

        const tablaMetas = $('#tablaMetas');
        let dtTablaMetas;
        const tblIncidenciasPresentadas = $('#tblIncidenciasPresentadas');

        const dateFormat = 'dd/mm/yy';

        // Reporte
        const report = $("#report");

        //#region GRAFICAS LESIONES Y DAÑOS

        let chartLesionesRegionAnatomica;
        let chartLesionesCausasInmediatas;
        let chartDañosCausasInmediatas;
        let chartDañosProtocoloFatalidad;

        //#endregion

        //#endregion
        //#region PETICIONES
        const getIncidentesRegistrables = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetIncidentesRegistrables', getFormMejora()) };
        const getIncidentesReportables = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetIncidentesReportables', getFormMejora()) };
        const getHorasHombreLostDay = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetHorasHombreLostDay', getFormMejora()) };
        const getPotencialSeveridad = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetPotencialSeveridad', getFormMejora()) };
        const getIncidentesMes = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetIncidentesMes', getFormMejora()) };
        const getIncidentesRegistrablesXmesAnterior = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetIncidentesRegistrablesXmesAnterior', getFormMejora()) };
        const getIncidentesRegistrablesXmesActual = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetIncidentesRegistrablesXmesActual', getFormMejora()) };
        const getDanoInstalacionEquipo = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetDanoInstalacionEquipo', getFormMejora()) };
        const getIncidentesDepartamento = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetIncidentesDepartamento', getFormMejora()) };
        const getTasaIncidencias = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetTasaIncidencias', { busq: getFormMejora(), tipoCarga: $('#selectTipoCarga').val() }) };
        const getTIFR = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetTIFR', { busq: getFormMejora(), tipoCarga: $('#selectTipoCargaTIFR').val() }) };
        const getTPDFR = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetTPDFR', { busq: getFormMejora(), tipoCarga: $('#selectTipoCargaTPDFR').val() }) };
        const getIncidenciasPresentadas = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetIncidenciasPresentadas', getFormIndicadores()) };
        const getIncidenciasPresentadasTipo = tipo => { return $.post('/Administrativo/IndicadoresSeguridad/GetIncidenciasPresentadasTipo', { tipo, busq: getFormIndicadores() }) };
        const getAccidentabilidad = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetAccidentabilidad', getFormAccidentabilidad()) };
        const getAccidentabilidadTop = busq => { return $.post('/Administrativo/IndicadoresSeguridad/getAccidentabilidadTop', busq) };
        const getCausasIncidencias = () => { return $.post('/Administrativo/IndicadoresSeguridad/GetCausasIncidencias', getFormCausas()) };
        const obtenerMetasGrafica = () => $.post('/Administrativo/IndicadoresSeguridad/ObtenerMetasGrafica');
        const agregarMetaGrafica = meta => $.post('/Administrativo/IndicadoresSeguridad/AgregarMetaGrafica', meta);
        const eliminarMetaGrafica = id => $.post('/Administrativo/IndicadoresSeguridad/EliminarMetaGrafica', { id });
        //#endregion

        _permisoBotonMetas = false;

        // Función init autoejecutable.
        (function init() {
            getPermisoBotonMetas();

            initFiltrosMejora()
            initFiltrosIndicadores()
            initFiltrosAccidentabilidad()
            initFiltrosCausas()

            // initChartMejora()
            // initChartIndicadoresSeguridad()
            initTablaMetas();
            initTableIndicadoresSeguridad();
            // initChartAccidentabilidad()
            // initChartCausasIncidencias()
        })();

        $('#selectDivision').on('change', function () {
            $("#selectCCFiltros").fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivision').val(), $('#selectLineaNegocio').val());
            convertToMultiselect('#selectCCFiltros');
        });

        $('#selectLineaNegocio').on('change', function () {
            $("#selectCCFiltros").fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivision').val(), $('#selectLineaNegocio').val());
            convertToMultiselect('#selectCCFiltros');
        });

        $('#selectDivisionIndicadoresSeguridad').on('change', function () {
            $("#selectCCIndicadoresSeguridad").fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivisionIndicadoresSeguridad').val(), $('#selectLineaNegocioIndicadoresSeguridad').val());
            convertToMultiselect('#selectCCIndicadoresSeguridad');
        });

        $('#selectLineaNegocioIndicadoresSeguridad').on('change', function () {
            $("#selectCCIndicadoresSeguridad").fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivision').val(), $('#selectLineaNegocioIndicadoresSeguridad').val());
            convertToMultiselect('#selectCCIndicadoresSeguridad');
        });

        $('#selectDivisionAccidentabilidad').on('change', function () {
            $("#selectCCFiltrosAccidentabilidad").fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivisionAccidentabilidad').val(), $('#selectLineaNegocioAccidentabilidad').val());
            convertToMultiselect('#selectCCFiltrosAccidentabilidad');
        });

        $('#selectLineaNegocioAccidentabilidad').on('change', function () {
            $("#selectCCFiltrosAccidentabilidad").fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivisionAccidentabilidad').val(), $('#selectLineaNegocioAccidentabilidad').val());
            convertToMultiselect('#selectCCFiltrosAccidentabilidad');
        });

        $('#selectDivisionCausas').on('change', function () {
            $("#selectCCFiltrosCausas").fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivisionCausas').val(), $('#selectLineaNegocioCausas').val());
            convertToMultiselect('#selectCCFiltrosCausas');
        });

        $('#selectLineaNegocioCausas').on('change', function () {
            $("#selectCCFiltrosCausas").fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivisionCausas').val(), $('#selectLineaNegocioCausas').val());
            convertToMultiselect('#selectCCFiltrosCausas');
        });

        function getPermisoBotonMetas() {
            axios.post('/Administrativo/IndicadoresSeguridad/GetPermisoBotonMetas')
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        _permisoBotonMetas = response.data.permisoBotonMetas;

                        if (_permisoBotonMetas) {
                            $('#botonMetas').show(500);
                            $('#botonMetasTIFR').show(500);
                            $('#botonMetasTPDFR').show(500);
                        } else {
                            $('#botonMetas').hide(500);
                            $('#botonMetasTIFR').hide(500);
                            $('#botonMetasTPDFR').hide(500);
                        }
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        //#region CARGAS INICIALES
        function initFiltrosMejora() {
            $('#selectDivision').fillCombo('/Administrativo/Requerimientos/GetDivisionesCombo', null, false, 'Todos');
            convertToMultiselect('#selectDivision');
            $('#selectLineaNegocio').fillCombo('/Administrativo/Requerimientos/GetLineaNegocioCombo', { division: 0 }, false, 'Todos');
            convertToMultiselect('#selectLineaNegocio');

            $('#selectDivisionIndicadoresSeguridad').fillCombo('/Administrativo/Requerimientos/GetDivisionesCombo', null, false, 'Todos');
            convertToMultiselect('#selectDivisionIndicadoresSeguridad');
            $('#selectLineaNegocioIndicadoresSeguridad').fillCombo('/Administrativo/Requerimientos/GetLineaNegocioCombo', { division: 0 }, false, 'Todos');
            convertToMultiselect('#selectLineaNegocioIndicadoresSeguridad');

            $('#selectDivisionAccidentabilidad').fillCombo('/Administrativo/Requerimientos/GetDivisionesCombo', null, false, 'Todos');
            convertToMultiselect('#selectDivisionAccidentabilidad');
            $('#selectLineaNegocioAccidentabilidad').fillCombo('/Administrativo/Requerimientos/GetLineaNegocioCombo', { division: 0 }, false, 'Todos');
            convertToMultiselect('#selectLineaNegocioAccidentabilidad');

            $('#selectDivisionCausas').fillCombo('/Administrativo/Requerimientos/GetDivisionesCombo', null, false, 'Todos');
            convertToMultiselect('#selectDivisionCausas');
            $('#selectLineaNegocioCausas').fillCombo('/Administrativo/Requerimientos/GetLineaNegocioCombo', { division: 0 }, false, 'Todos');
            convertToMultiselect('#selectLineaNegocioCausas');

            $('#selectClasificacionHHT').fillCombo('GetClasificacionHHTCombo', null, false, null);

            //$("#selectCCFiltros").fillCombo('LlenarComboCCUsuario', { est: true }, false, "Todos");
            $("#selectCCFiltros").fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivision').val(), $('#selectLineaNegocio').val());
            convertToMultiselect('#selectCCFiltros');
            $("#selectCCIndicadoresSeguridad").fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivisionIndicadoresSeguridad').val(), $('#selectLineaNegocioIndicadoresSeguridad').val());
            convertToMultiselect('#selectCCIndicadoresSeguridad');

            // $("#selectDeptoFiltros").fillCombo('getDepartamentosList', { est: true }, false, "Todos");
            // convertToMultiselect('#selectDeptoFiltros');
            $("#selectSupervisorFiltros").fillCombo('getSupervisoresList', { est: true }, false, "Todos");
            convertToMultiselect('#selectSupervisorFiltros');

            $("#selectTipoCarga").fillCombo('GetTiposCargaTasaAnual', null, false);
            $("#selectTipoCarga")[0].options[0].remove();

            $("#selectTipoCargaTIFR").fillCombo('GetTiposCargaTasaAnual', null, false);
            $("#selectTipoCargaTIFR")[0].options[0].remove();

            $("#selectTipoCargaTPDFR").fillCombo('GetTiposCargaTasaAnual', null, false);
            $("#selectTipoCargaTPDFR")[0].options[0].remove();

            $("#selectTipoGrafica").fillCombo('GetTiposGraficaMeta', null, false);
            $("#selectTipoGrafica")[0].options[0].remove();

            $('#txtFechaInicio').datepicker({ dateFormat }).datepicker("setDate", fechaInicioAnio);
            $('#txtFechaFin').datepicker({ dateFormat }).datepicker("setDate", fechaActual);
            $('#btnBuscar').click(buscar)
        }

        function initFiltrosIndicadores() {
            $("#selectDeptoIndicadoresSeguridad").fillCombo('getDepartamentosList', { est: true }, false, "Todos");
            convertToMultiselect('#selectDeptoIndicadoresSeguridad');
            $("#selectSupervisorIndicadoresSeguridad").fillCombo('getSupervisoresList', { est: true }, false, "Todos");
            convertToMultiselect('#selectSupervisorIndicadoresSeguridad');
            $('#txtFechaInicioIndicadoresSeguridad').datepicker({ dateFormat }).datepicker("setDate", fechaInicioAnio);
            $('#txtFechaFinIndicadoresSeguridad').datepicker({ dateFormat }).datepicker("setDate", fechaActual);
            $('#btnBuscarIndicadoresSeguridad').click(buscarIndicadoresSeguridad)
        }

        function initFiltrosAccidentabilidad() {
            $("#selectCCFiltrosAccidentabilidad").fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivisionAccidentabilidad').val(), $('#selectLineaNegocioAccidentabilidad').val());
            convertToMultiselect('#selectCCFiltrosAccidentabilidad');
            $("#selectDeptoAccidentabilidad").fillCombo('getDepartamentosList', { est: true }, false, "Todos");
            convertToMultiselect('#selectDeptoAccidentabilidad');
            $("#selectSupervisorAccidentabilidad").fillCombo('getSupervisoresIncidentesList', { est: true }, false, "Todos");
            convertToMultiselect('#selectSupervisorAccidentabilidad');
            $('#txtFechaInicioAccidentabilidad').datepicker({ dateFormat }).datepicker("setDate", fechaInicioAnio);
            $('#txtFechaFinAccidentabilidad').datepicker({ dateFormat }).datepicker("setDate", fechaActual);
            $('#btnBuscarAccidentabilidad').click(buscarAccidentabilidad)
        }

        function initTablaMetas() {
            dtTablaMetas = tablaMetas.DataTable({
                language: dtDicEsp,
                paging: false,
                info: false,
                searching: false,
                order: [[2, "desc"], [1, "desc"], [0, "desc"]],
                columns: [
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'valor', title: 'Valor' },
                    { data: 'año', title: 'Año' },
                    { data: 'tipoGrafica', title: 'Gráfica' },
                    { data: 'colorString', title: 'Color', render: data => `<input disabled type="color" value="${data}">` },
                    { data: 'id', title: 'Eliminar', render: () => `<button class="btn btn-danger botonEliminarMeta"><i class="fas fa-trash"></i></button>` },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {
                    tablaMetas.find('button.botonEliminarMeta').click(function () {
                        const meta = dtTablaMetas.row($(this).parents('tr')).data();
                        AlertaAceptarRechazarNormal(
                            'Confirmar eliminación',
                            '¿Está seguro de eliminar esta meta?',
                            () => eliminarMeta(meta.id))
                    });
                }
            });
        }

        function eliminarMeta(metaID) {
            if (metaID == "") {
                return;
            }
            eliminarMetaGrafica(metaID)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Meta eliminada correctamente`);
                        cargarMetas();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`));
        }

        function initFiltrosCausas() {
            //$("#selectCCFiltrosCausas").fillCombo('LlenarComboCCUsuario', { est: true }, false, "Todos");
            $("#selectCCFiltrosCausas").fillComboSeguridadDivisionLineaNegocio(false, true, $('#selectDivisionCausas').val(), $('#selectLineaNegocioCausas').val());
            convertToMultiselect('#selectCCFiltrosCausas');

            $("#selectDeptoCausas").fillCombo('getDepartamentosList', { est: true }, false, "Todos");
            convertToMultiselect('#selectDeptoCausas');
            $("#selectSupervisorCausas").fillCombo('getSupervisoresIncidentesList', { est: true }, false, "Todos");
            convertToMultiselect('#selectSupervisorCausas');
            $('#txtFechaInicioCausas').datepicker({ dateFormat }).datepicker("setDate", fechaInicioAnio);
            $('#txtFechaFinCausas').datepicker({ dateFormat }).datepicker("setDate", fechaActual);
            $('#btnBuscarCausas').click(buscarCausas)
        }
        function getFormMejora() {
            //#region SE ELIMINA LOS PARAMETROS ADICIONALES DEL VALUE EN CASO DE SER CONTRATISTA O AGRUPACION DE CONTRATISTAS
            let objGrupos = new Object();
            let arrGrupos = [];
            for (let i = 0; i < $("#selectCCFiltros").getMultiSeg().length; i++) {
                let str = $("#selectCCFiltros").getMultiSeg()[i].idAgrupacion;
                let idEmpresa = $("#selectCCFiltros").getMultiSeg()[i].idEmpresa;
                if (parseFloat(idEmpresa) == 1000) {
                    let idAgrupacion = str.replace("c_", "");
                    objGrupos = {
                        idEmpresa: idEmpresa,
                        idAgrupacion: parseFloat(idAgrupacion)
                    };
                    arrGrupos.push(objGrupos);
                } else if (parseFloat(idEmpresa) == 2000) {
                    let idAgrupacion = str.replace("a_", "");
                    objGrupos = {
                        idEmpresa: idEmpresa,
                        idAgrupacion: parseFloat(idAgrupacion)
                    };
                    arrGrupos.push(objGrupos);
                } else {
                    objGrupos = {
                        idEmpresa: $("#selectCCFiltros").getMultiSeg()[i].idEmpresa,
                        idAgrupacion: $("#selectCCFiltros").getMultiSeg()[i].idAgrupacion
                    };
                    arrGrupos.push(objGrupos);
                }
            }
            //#endregion

            let obj = new Object();
            obj = {
                clasificacion: +($('#selectClasificacionHHT').val()),
                arrCC: null,
                // arrGrupos: $("#selectCCFiltros").getMultiSeg(), 
                arrGrupos: arrGrupos,
                fechaInicio: $('#txtFechaInicio').val(),
                fechaFin: $('#txtFechaFin').val(),
                // arrDepto: $("#selectDeptoFiltros").val(),
                arrSupervisor: $("#selectSupervisorFiltros").val(),
                arrDivisiones: $('#selectDivision').val(),
                arrLineasNegocio: $('#selectLineaNegocio').val()
            }
            return obj;
        }
        function getFormIndicadores() {
            //#region SE ELIMINA LOS PARAMETROS ADICIONALES DEL VALUE EN CASO DE SER CONTRATISTA O AGRUPACION DE CONTRATISTAS
            let objGrupos = new Object();
            let arrGrupos = [];
            for (let i = 0; i < $("#selectCCIndicadoresSeguridad").getMultiSeg().length; i++) {
                let str = $("#selectCCIndicadoresSeguridad").getMultiSeg()[i].idAgrupacion;
                let idEmpresa = $("#selectCCIndicadoresSeguridad").getMultiSeg()[i].idEmpresa;
                if (parseFloat(idEmpresa) == 1000) {
                    let idAgrupacion = str.replace("c_", "");
                    objGrupos = {
                        idEmpresa: idEmpresa,
                        idAgrupacion: parseFloat(idAgrupacion)
                    };
                    arrGrupos.push(objGrupos);
                } else if (parseFloat(idEmpresa) == 2000) {
                    let idAgrupacion = str.replace("a_", "");
                    objGrupos = {
                        idEmpresa: idEmpresa,
                        idAgrupacion: parseFloat(idAgrupacion)
                    };
                    arrGrupos.push(objGrupos);
                } else {
                    objGrupos = {
                        idEmpresa: $("#selectCCIndicadoresSeguridad").getMultiSeg()[i].idEmpresa,
                        idAgrupacion: $("#selectCCIndicadoresSeguridad").getMultiSeg()[i].idAgrupacion
                    };
                    arrGrupos.push(objGrupos);
                }
            }
            //#endregion

            let obj = new Object();
            obj = {
                arrCC: null,
                // , arrGrupos: $("#selectCCIndicadoresSeguridad").getMultiSeg() 
                arrGrupos: arrGrupos,
                fechaInicio: $('#txtFechaInicioIndicadoresSeguridad').val(),
                fechaFin: $('#txtFechaFinIndicadoresSeguridad').val(),
                arrDepto: $("#selectDeptoIndicadoresSeguridad").val(),
                arrSupervisor: $("#selectSupervisorIndicadoresSeguridad").val(),
                arrDivisiones: $('#selectDivisionIndicadoresSeguridad').val(),
                arrLineasNegocio: $('#selectLineaNegocioIndicadoresSeguridad').val()
            };
            return obj;
        }
        function getFormAccidentabilidad() {
            //#region SE ELIMINA LOS PARAMETROS ADICIONALES DEL VALUE EN CASO DE SER CONTRATISTA O AGRUPACION DE CONTRATISTAS
            let objGrupos = new Object();
            let arrGrupos = [];
            for (let i = 0; i < $("#selectCCFiltrosAccidentabilidad").getMultiSeg().length; i++) {
                let str = $("#selectCCFiltrosAccidentabilidad").getMultiSeg()[i].idAgrupacion;
                let idEmpresa = $("#selectCCFiltrosAccidentabilidad").getMultiSeg()[i].idEmpresa;
                if (parseFloat(idEmpresa) == 1000) {
                    let idAgrupacion = str.replace("c_", "");
                    objGrupos = {
                        idEmpresa: idEmpresa,
                        idAgrupacion: parseFloat(idAgrupacion)
                    };
                    arrGrupos.push(objGrupos);
                } else if (parseFloat(idEmpresa) == 2000) {
                    let idAgrupacion = str.replace("a_", "");
                    objGrupos = {
                        idEmpresa: idEmpresa,
                        idAgrupacion: parseFloat(idAgrupacion)
                    };
                    arrGrupos.push(objGrupos);
                } else {
                    objGrupos = {
                        idEmpresa: $("#selectCCFiltrosAccidentabilidad").getMultiSeg()[i].idEmpresa,
                        idAgrupacion: $("#selectCCFiltrosAccidentabilidad").getMultiSeg()[i].idAgrupacion
                    };
                    arrGrupos.push(objGrupos);
                }
            }
            //#endregion

            let obj = new Object();
            obj = {
                //arrCC: $("#selectCCFiltrosAccidentabilidad").val()
                arrCC: null,
                // arrGrupos: $("#selectCCFiltrosAccidentabilidad").getMultiSeg(), 
                arrGrupos: arrGrupos,
                fechaInicio: $("#txtFechaInicioAccidentabilidad").val(),
                fechaFin: $("#txtFechaFinAccidentabilidad").val(),
                arrDepto: $("#selectDeptoAccidentabilidad").val(),
                arrSupervisor: $("#selectSupervisorAccidentabilidad").val(),
                arrDivisiones: $('#selectDivisionAccidentabilidad').val(),
                arrLineasNegocio: $('#selectLineaNegocioAccidentabilidad').val()
            };
            return obj;
        }
        function getFormCausas() {
            //#region SE ELIMINA LOS PARAMETROS ADICIONALES DEL VALUE EN CASO DE SER CONTRATISTA O AGRUPACION DE CONTRATISTAS
            let objGrupos = new Object();
            let arrGrupos = [];
            for (let i = 0; i < $("#selectCCFiltrosCausas").getMultiSeg().length; i++) {
                let str = $("#selectCCFiltrosCausas").getMultiSeg()[i].idAgrupacion;
                let idEmpresa = $("#selectCCFiltrosCausas").getMultiSeg()[i].idEmpresa;
                if (parseFloat(idEmpresa) == 1000) {
                    let idAgrupacion = str.replace("c_", "");
                    objGrupos = {
                        idEmpresa: idEmpresa,
                        idAgrupacion: parseFloat(idAgrupacion)
                    };
                    arrGrupos.push(objGrupos);
                } else if (parseFloat(idEmpresa) == 2000) {
                    let idAgrupacion = str.replace("a_", "");
                    objGrupos = {
                        idEmpresa: idEmpresa,
                        idAgrupacion: parseFloat(idAgrupacion)
                    };
                    arrGrupos.push(objGrupos);
                } else {
                    objGrupos = {
                        idEmpresa: $("#selectCCFiltrosCausas").getMultiSeg()[i].idEmpresa,
                        idAgrupacion: $("#selectCCFiltrosCausas").getMultiSeg()[i].idAgrupacion
                    };
                    arrGrupos.push(objGrupos);
                }
            }
            //#endregion

            let obj = new Object();
            obj = {
                //arrCC: $("#selectCCFiltrosCausas").val()
                arrCC: null,
                // arrGrupos: $("#selectCCFiltrosCausas").getMultiSeg(), 
                arrGrupos: arrGrupos,
                fechaInicio: $('#txtFechaInicioCausas').val(),
                fechaFin: $('#txtFechaFinCausas').val(),
                arrDepto: $("#selectDeptoCausas").val(),
                arrSupervisor: $("#selectSupervisorCausas").val(),
                arrDivisiones: $('#selectDivisionCausas').val(),
                arrLineasNegocio: $('#selectLineaNegocioCausas').val()
            }
            return obj;
        }
        function initChartMejora() {
            let incidentesNombresRegistrable, incidentesCantidadRegistrable
            let incidenteNombresReportable, incidentesCantidadReportable
            getIncidentesRegistrables().done(function (response) {
                if (response.success) {
                    incidentesNombresRegistrable = response.items.map(x => x.incidenteTipo)
                    incidentesCantidadRegistrable = response.items.map(x => x.incidenteCantidad)

                    setChartRegistrables(incidentesNombresRegistrable, incidentesCantidadRegistrable);
                }
            });
            getIncidentesReportables().done(function (response) {
                if (response.success) {
                    incidenteNombresReportable = response.items.map(x => x.incidenteTipo)
                    incidentesCantidadReportable = response.items.map(x => x.incidenteCantidad)

                    setChartReportables(incidenteNombresReportable, incidentesCantidadReportable);
                }
            });
            getHorasHombreLostDay().done(function (response) {
                if (response.success) {
                    $('#tablaHorasHombre tbody').append(setRowTable(response.items))
                }
            });
            getPotencialSeveridad().done(function (response) {
                if (response.success) {
                    setChartPotencialSeveridad(Object.values(response.items));
                }
            });
            getIncidentesMes().done(function (response) {
                if (response.success) {
                    setChartIncidentesMes(response.labels, response.datasets);
                }
            });
            getIncidentesRegistrablesXmesAnterior().done(function (response) {
                if (response.success) {
                    setChartIncidentesRegistrablesMesAnterior(response.labels, response.datasets);
                }
            });
            getIncidentesRegistrablesXmesActual().done(function (response) {
                if (response.success) {
                    setChartIncidentesRegistrablesMesActual(response.labels, response.datasets);

                }
            });
            getTasaIncidencias().done(function (response) {
                if (response.success) {
                    setChartTasaIncidencias(response.labels, response.datasets);
                }
            })
        }
        function initChartIndicadoresSeguridad() {
            getIncidenciasPresentadas().done(function (response) {
                if (response.success) {
                    if (response.permisoEliminar) {
                        $('#botonReporteGlobal').show(500);
                        $('#botonMetas').show(500);
                    } else {
                        $('#botonReporteGlobal').hide(500);
                        $('#botonMetas').hide(500);
                    }

                    AddRows(tblIncidenciasPresentadas, response.items);

                    setCantidadesIndicadores(response.indicadores);
                } else {
                    if (response.EMPTY) {
                        AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros establecidos.`);
                    } else {
                        AlertaGeneral(`Error`, response.error);
                    }
                }
            });
        }
        function initChartAccidentabilidad() {
            getAccidentabilidad().done(function (response) {
                if (response.success) {
                    setCantidadesAccidentabilidad(response.indicadores)
                }
            });
        }
        function initChartCausasIncidencias() {
            getCausasIncidencias().done(function (response) {
                if (response.success) {
                    setCantidadesCausasIndicadores(response.indicadores)
                }
            });
        }
        //#endregion
        //#region EVALUACION MEJORA
        function setChartRegistrables(incidentesNombres, incidentesCantidad) {
            var options = {
                responsive: true,
                plugins: {
                    labels: [
                        {
                            render: 'label',
                            position: 'outside'
                        },
                        {
                            render: 'percentage'
                        }
                    ]
                }
            };
            var ctx = document.getElementById('chartRegistrables').getContext('2d');
            chartIncidentesRegistrables = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: incidentesNombres,
                    datasets: [
                        {
                            label: '',
                            data: incidentesCantidad,
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)'
                            ],
                            borderColor: [
                                'rgba(255, 99, 132, 1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)'
                            ]
                        }
                    ]
                },
                options: options
            });
        }
        function setChartReportables(incidentesNombres, incidentesCantidad) {
            var options = {
                responsive: true,
                plugins: {
                    labels: [
                        {
                            render: 'label',
                            position: 'outside'
                        },
                        {
                            render: 'percentage'
                        }
                    ]
                }
            };
            var ctx = document.getElementById('chartReportables').getContext('2d');
            chartIncidentesReportables = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: incidentesNombres,
                    datasets: [
                        {
                            label: 'Reportable',
                            data: incidentesCantidad,
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)'
                            ],
                            borderColor: [
                                'rgba(255, 99, 132, 1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)'
                            ]
                        }
                    ]
                },
                options: options
            });
        }
        function setChartPotencialSeveridad(severidad) {
            var options = {
                responsive: true,
                plugins: {
                    labels: [
                        {
                            render: 'label',
                            position: 'outside'
                        },
                        {
                            render: 'percentage'
                        }
                    ]
                }
            };
            var ctx = document.getElementById('chartPotencialSeveridad').getContext('2d');
            chartPotencialSeveridad = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: ['Menor', 'Moderado', 'Mayor', 'Catastrofico'],
                    datasets: [
                        {
                            label: 'POTENCIAL SEVERIDAD',
                            data: severidad,
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)'
                            ],
                            borderColor: [
                                'rgba(255, 99, 132, 1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)'
                            ]
                        }
                    ]
                },
                options: options
            });
        }

        function setChartIncidentesMes(labels, datasets) {
            var ctx = document.getElementById('chartIncidentesMes').getContext('2d');
            chartIncidentesMes = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'INCIDENTES POR MES'
                    },
                    legend: {
                        display: true,
                        position: 'bottom',
                    },
                    plugins: {
                        labels: {
                            render: 'value'
                        }
                    }
                }
            });
        }

        function setChartIncidentesRegistrablesMesAnterior(labels, datasets) {
            var ctx = document.getElementById('chartAccidentesRegistrablesMesAnterior').getContext('2d');
            chartIncidentesRegistrablesMesAnterior = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'INCIDENTES REGISTRABLES POR MES AÑO ANTERIOR'
                    },
                    legend: {
                        display: true,
                        position: 'bottom',
                    },
                    plugins: {
                        labels: {
                            render: 'value'
                        }
                    }
                }
            });
        }

        function setChartIncidentesRegistrablesMesActual(labels, datasets) {
            var ctx = document.getElementById('chartAccidentesRegistrablesMesActual').getContext('2d');
            chartIncidentesRegistrablesMesActual = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets, },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'INCIDENTES REGISTRABLES POR MES AÑO ACTUAL'
                    },
                    legend: {
                        display: true,
                        position: 'bottom',
                    },
                    plugins: {
                        labels: {
                            render: 'value'
                        }
                    }
                }
            });
        }

        function setChartDanoInstalacionEquipo(labels, values) {
            var options = {
                responsive: true,
                plugins: {
                    labels: [
                        {
                            render: 'label',
                            position: 'outside'
                        },
                        {
                            render: 'percentage'
                        }
                    ]
                }
            };

            var ctx = document.getElementById('chartDanoInstalacionEquipo').getContext('2d');

            chartDanoInstalacionEquipo = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels,
                    datasets: [
                        {
                            label: 'DAÑO INSTALACIÓN/EQUIPO',
                            data: values,
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)'
                            ],
                            borderColor: [
                                'rgba(255, 99, 132, 1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                            ]
                        }
                    ]
                },
                options: options
            });
        }

        function setChartIncidentesDepartamento(departamentos, cantidades) {
            var ctx = document.getElementById('chartIncidentesDepartamento').getContext('2d');

            chartIncidentesDepartamento = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: departamentos,
                    datasets: [
                        {
                            data: cantidades,
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)',
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)'
                            ],
                            borderColor: [
                                'rgba(255, 99, 132, 1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)',
                                'rgba(255, 99, 132, 1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                            ],
                            borderWidth: 1
                        }
                    ]
                },

                // Configuration options go here
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'INCIDENTES POR DEPARTAMENTO'
                    },
                    legend: {
                        display: false,
                    },
                    plugins: {
                        labels: {
                            render: 'value'
                        }
                    }
                }
            });
        }

        function setChartTasaIncidencias(labels, datasets) {
            var ctx = document.getElementById('chartTasaIncidencia').getContext('2d');
            chartTasaIncidencias = new Chart(ctx, {
                type: 'line',
                data: { labels, datasets },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: 'TASA DE INCIDENCIA'
                    },
                    legend: {
                        display: true,
                        position: 'bottom',
                    },
                    plugins: {
                        labels: {
                            render: 'value'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function setChartTIFR(labels, datasets) {
            var ctx = document.getElementById('chartTIFR').getContext('2d');
            chartTIFR = new Chart(ctx, {
                type: 'line',
                data: { labels, datasets },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: 'TIFR'
                    },
                    legend: {
                        display: true,
                        position: 'bottom',
                    },
                    plugins: {
                        labels: {
                            render: 'value'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function setChartTPDFR(labels, datasets) {
            var ctx = document.getElementById('chartTPDFR').getContext('2d');
            chartTPDFR = new Chart(ctx, {
                type: 'line',
                data: { labels, datasets },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: 'TPDFR'
                    },
                    legend: {
                        display: true,
                        position: 'bottom',
                    },
                    plugins: {
                        labels: {
                            render: 'value'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function setChartLesionesRegionAnatomica(labels, values) {

            let data = []

            let i = 0
            for (const item of labels) {
                data.push({ name: item, y: values[i] });
                i++;
            }

            Highcharts.chart('chartLesionesRegionAnatomica', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Region anatomica',
                    align: 'left'
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
                },
                series: [{
                    // name: '',
                    colorByPoint: true,
                    data: data
                }]
            });

        }
        function setChartLesionesCausasInmediatas(labels, values) {
            let data = []

            let i = 0
            for (const item of labels) {
                data.push({ name: item, y: values[i] });
                i++;
            }

            Highcharts.chart('chartLesionesCausasInmediatas', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Causas inmediatas',
                    align: 'left'
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
                },
                series: [{
                    // name: '',
                    colorByPoint: true,
                    data: data
                }]
            });
        }
        function setChartDañosCausasInmediatas(labels, values) {
            let data = []

            let i = 0
            for (const item of labels) {
                data.push({ name: item, y: values[i] });
                i++;
            }

            Highcharts.chart('chartDañosCausasInmediatas', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Causas inmediatas',
                    align: 'left'
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
                },
                series: [{
                    // name: '',
                    colorByPoint: true,
                    data: data
                }]
            });
        }
        function setChartDañosProtocoloFatalidad(labels, values) {
            let data = []

            let i = 0
            for (const item of labels) {
                data.push({ name: item, y: values[i] });
                i++;
            }

            Highcharts.chart('chartDañosProtocoloFatalidad', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Protocolo de fatalidad',
                    align: 'left'
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
                },
                series: [{
                    // name: '',
                    colorByPoint: true,
                    data: data
                }]
            });
        }
        function setRowTable(items) {
            const tr = `
                <tr>
                    <td class="text-center">${formatValue(items.lostDays.toFixed(2))}</td>
                    <td class="text-center">${formatValue(items.trabadoresPromedio)}</td>
                    <td class="text-center">${formatValue(items.horasHombres.toFixed(2))}</td>
                    <td class="text-center">${formatValue(items.horasHombresSinIncidentes.toFixed(2))}</td>
                </tr>
            `;
            return tr;
        }
        function buscar() {
            getIncidentesRegistrables().done(function (response) {
                if (response.success) {
                    incidentesNombresRegistrable = response.items.map(x => x.incidenteTipo)
                    incidentesCantidadRegistrable = response.items.map(x => x.incidenteCantidad)
                    if (chartIncidentesRegistrables) {
                        chartIncidentesRegistrables.destroy();
                    }
                    setChartRegistrables(incidentesNombresRegistrable, incidentesCantidadRegistrable);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            });
            getIncidentesReportables().done(function (response) {
                if (response.success) {
                    incidenteNombresReportable = response.items.map(x => x.incidenteTipo)
                    incidentesCantidadReportable = response.items.map(x => x.incidenteCantidad)
                    if (chartIncidentesReportables) {
                        chartIncidentesReportables.destroy();
                    }
                    setChartReportables(incidenteNombresReportable, incidentesCantidadReportable);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            });

            axios.post('/Administrativo/IndicadoresSeguridad/GetHorasHombreLostDay', { busq: getFormMejora() }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    let tabla1 = response.data.tablaHorasHombre1;
                    let tabla2 = response.data.tablaHorasHombre2;

                    $('#tablaHorasHombre tbody tr').remove();
                    $('#tablaHorasHombre tbody').append(`
                        <tr>
                            <th class="text-center" style="font-weight: bold; vertical-align: middle;">HHT CONSTRUPLAN</th>
                            <th class="text-center" style="font-weight: bold; vertical-align: middle;">HHT Contratistas</th>
                            <th class="text-center" style="font-weight: bold; vertical-align: middle;">HHT Total</th>
                            <th class="text-center" style="font-weight: bold; vertical-align: middle;">HHT Sin LTI</th>
                            <th class="text-center" style="font-weight: bold; vertical-align: middle;">Colaboradores Activos</th>
                        </tr>
                        <tr>
                            <td class="text-center">${formatValue(tabla1.hhtConstruplan.toFixed(2))}</td>
                            <td class="text-center">${formatValue(tabla1.hhtContratistas.toFixed(2))}</td>
                            <td class="text-center">${formatValue(tabla1.hhtTotal.toFixed(2))}</td>
                            <td class="text-center">${formatValue(tabla1.hhtSinLTI.toFixed(2))}</td>
                            <td class="text-center">${formatValue(tabla1.colaboradoresActivos.toFixed(2))}</td>
                        </tr>
                        <tr>
                            <th class="text-center" style="font-weight: bold; vertical-align: middle;">LTIFR</th>
                            <th class="text-center" style="font-weight: bold; vertical-align: middle;">TRIFR</th>
                            <th class="text-center" style="font-weight: bold; vertical-align: middle;">TIFR</th>
                            <th class="text-center" style="font-weight: bold; vertical-align: middle;">Lost Days</th>
                            <th class="text-center" style="font-weight: bold; vertical-align: middle;">Severity Rate</th>
                        </tr>
                        <tr>
                            <td class="text-center">${formatValue(tabla2.LTIFR.toFixed(2))}</td>
                            <td class="text-center">${formatValue(tabla2.TRIFR.toFixed(2))}</td>
                            <td class="text-center">${formatValue(tabla2.TIFR.toFixed(2))}</td>
                            <td class="text-center">${formatValue(tabla2.lostDays.toFixed(2))}</td>
                            <td class="text-center">${formatValue(tabla2.severityRate.toFixed(2))}</td>
                        </tr>
                    `);
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));

            getPotencialSeveridad().done(function (response) {
                if (response.success) {
                    if (chartPotencialSeveridad) {
                        chartPotencialSeveridad.destroy();
                    }
                    setChartPotencialSeveridad(Object.values(response.items));
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            });
            getIncidentesMes().done(function (response) {
                if (response.success) {
                    if (chartIncidentesMes) {
                        chartIncidentesMes.destroy();
                    }
                    setChartIncidentesMes(response.labels, response.datasets);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            });
            getIncidentesRegistrablesXmesAnterior().done(function (response) {
                if (response.success) {
                    if (chartIncidentesRegistrablesMesAnterior) {
                        chartIncidentesRegistrablesMesAnterior.destroy();
                    }
                    setChartIncidentesRegistrablesMesAnterior(response.labels, response.datasets);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            });
            getIncidentesRegistrablesXmesActual().done(function (response) {
                if (response.success) {
                    if (chartIncidentesRegistrablesMesActual) {
                        chartIncidentesRegistrablesMesActual.destroy();
                    }
                    setChartIncidentesRegistrablesMesActual(response.labels, response.datasets);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            });
            getDanoInstalacionEquipo().done(function (response) {
                if (response.success) {
                    if (chartDanoInstalacionEquipo) {
                        chartDanoInstalacionEquipo.destroy();
                    }

                    const labels = response.items.map(x => x.desc);
                    const values = response.items.map(x => x.cantidad);
                    setChartDanoInstalacionEquipo(labels, values);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            })

            getIncidentesDepartamento().done(function (response) {
                if (response.success) {
                    if (chartIncidentesDepartamento) {
                        chartIncidentesDepartamento.destroy();
                    }

                    let departamentos = response.data.map(x => x.departamentoDesc);
                    let cantidades = response.data.map(x => x.cantidad);

                    setChartIncidentesDepartamento(departamentos, cantidades);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            })

            getTasaIncidencias().done(function (response) {
                if (response.success) {
                    if (chartTasaIncidencias) {
                        chartTasaIncidencias.destroy();
                    }
                    setChartTasaIncidencias(response.labels, response.datasets);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            });

            getTIFR().done(function (response) {
                if (response.success) {
                    if (chartTIFR) {
                        chartTIFR.destroy();
                    }
                    setChartTIFR(response.labels, response.datasets);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            });

            getTPDFR().done(function (response) {
                if (response.success) {
                    if (chartTPDFR) {
                        chartTPDFR.destroy();
                    }
                    setChartTPDFR(response.labels, response.datasets);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            });
        }
        function buscarAccidentabilidad() {
            getAccidentabilidad().done(function (response) {
                if (response.success) {
                    $('.accidentabilidad').empty()
                    setCantidadesAccidentabilidad(response.indicadores)
                    GetDatosGraficasAccidentabilidad();
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            })
        }
        function buscarCausas() {
            getCausasIncidencias().done(function (response) {
                if (response.success) {
                    $('.causas').empty()
                    setCantidadesCausasIndicadores(response.indicadores)
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            })
        }
        //#endregion
        //#region INDICADORES SEGURIDAD
        function setCantidadesIndicadores(incidentesIndicadores) {
            $('#badgeSeguridad').removeAttr('style')
            $('#badgeSeguridad').html('');
            $('#badgeSeguridad').append(incidentesIndicadores.cantidadTotalIndicador);
            $('#badgeSeguridad').attr('style', setColorBadgeIndicadores(incidentesIndicadores.cantidadTotalIndicador))

            $('#indicadorTotalCantidad').html('');
            $('#indicadorTotalCantidad').append(incidentesIndicadores.cantidadTotalIndicador)
            $('#indicadorTotalHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadTotalIndicador))

            $('#indicadorFatalCantidad').html('');
            $('#indicadorFatalCantidad').append(incidentesIndicadores.cantidadFatalIndicador)
            $('#indicadorFatalHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadFatalIndicador))

            $('#indicadorLTACantidad').html('');
            $('#indicadorLTACantidad').append(incidentesIndicadores.cantidadLTAIndicador)
            $('#indicadorLTAHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadLTAIndicador))

            $('#indicadorATRCantidad').html('');
            $('#indicadorATRCantidad').append(incidentesIndicadores.cantidadATRIndicador)
            $('#indicadorATRHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadATRIndicador))

            $('#indicadorATMCantidad').html('');
            $('#indicadorATMCantidad').append(incidentesIndicadores.cantidadATMIndicador)
            $('#indicadorATMHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadATMIndicador))

            $('#indicadorAPACantidad').html('');
            $('#indicadorAPACantidad').append(incidentesIndicadores.cantidadAPAIndicador)
            $('#indicadorAPAHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadAPAIndicador))

            $('#indicadorDAMEQCantidad').html('');
            $('#indicadorDAMEQCantidad').append(incidentesIndicadores.cantidadDAMEQIndicador)
            $('#indicadorDAMEQHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadDAMEQIndicador))

            $('#indicadorNMCantidad').html('');
            $('#indicadorNMCantidad').append(incidentesIndicadores.cantidadNMIndicador)
            $('#indicadorNMHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadNMIndicador))

            $('#indicadorOICantidad').html('');
            $('#indicadorOICantidad').append(incidentesIndicadores.cantidadOIindicador)
            $('#indicadorOIHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadOIindicador))

            $('#indicadorEICantidad').html('');
            $('#indicadorEICantidad').append(incidentesIndicadores.cantidadEIindicador)
            $('#indicadorEIHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadEIindicador))
        }
        function setColoresIndicadores(numeroIncidencias) {
            let clase = '';
            switch (numeroIncidencias) {
                case 0: clase = 'label-success'
                    break;
                case 1: clase = 'label-warning'
                    break;
                case 2: clase = 'label-warning'
                    break;
                case 3: clase = 'label-warning'
                    break;
                case 4: clase = 'label-danger'
                    break;
                case 5: clase = 'label-danger'
                    break;
                case 6: clase = 'label-danger'
                    break;
                default: clase = 'label-danger'
                    break;
            }
            return clase;
        }

        function setColorBadgeIndicadores(numeroIncidencias) {
            let clase = '';
            switch (numeroIncidencias) {
                case 0: clase = 'background-color: #5cb85c !important'
                    break;
                case 1: clase = 'background-color: #f0ad4e !important'
                    break;
                case 2: clase = 'background-color: #f0ad4e !important'
                    break;
                case 3: clase = 'background-color: #f0ad4e !important'
                    break;
                case 4: clase = 'background-color: #d9534f !important'
                    break;
                case 5: clase = 'background-color: #d9534f !important'
                    break;
                case 6: clase = 'background-color: #d9534f !important'
                    break;
                default: clase = 'background-color: #d9534f !important'
                    break;
            }

            return clase;
        }

        function setRowDetalleIndicadores(items) {
            const tr = `
                <tr>
                    <td class="text-left">${items.centroCosto}</td>
                    <td class="text-center">${formatValue(items.cantidadTotalTipo)}</td>
                    <td class="text-center">${formatValue(items.cantidadHH)}</td>
                </tr>
            `;

            return tr;
        }
        function setRowDetalleAccidentalidad(items) {
            const tr = `
                <tr>
                    <td class="text-left">${items.descripcion}</td>
                    <td class="text-center">${formatValue(items.cantidad)}</td>
                    <td class="text-center">${formatValue(items.porcentaje)}%</td>
                </tr>
            `;

            return tr;
        }
        function setColorModal(color) {
            let clase = '';
            switch (color) {
                case 'label-success': clase = 'background-color: #5cb85c !important'
                    break;
                case 'label-warning': clase = 'background-color: #f0ad4e !important'
                    break;
                case 'label-danger': clase = 'background-color: #d9534f !important'
                    break;
                default: clase = 'background-color: #d9534f !important'
                    break;
            }
            return clase;
        }
        function buscarIndicadoresSeguridad() {
            getIncidenciasPresentadas().done(function (response) {
                if (response.success) {
                    AddRows(tblIncidenciasPresentadas, response.items);

                    setCantidadesIndicadores(response.indicadores);
                } else {
                    if (response.EMPTY) {
                        AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros establecidos.`);
                    } else {
                        AlertaGeneral(`Error`, response.error);
                    }
                }
            });
        }

        var hidden = $.fn.dataTable.absoluteOrder([
            "total"
        ]);

        function initTableIndicadoresSeguridad() {
            tblIncidenciasPresentadas.DataTable({
                retrieve: true,
                deferRender: true,
                language: dtDicEsp,
                bInfo: false,
                bLengthChange: false,
                searching: false,
                paging: false,
                ordering: false,
                orderFixed: { 'pre': [0, 'asc'] },
                initComplete: function (settings, json) {

                },
                dom: 'Bfrtip',
                buttons: [{ extend: 'excel', className: 'btn btn-primary pull-right mrgBottom', title: 'Incidencias Presentadas' }],
                columns: [
                    { data: 'centroCosto', title: 'PROYECTO' },
                    {
                        data: 'cantidadFatal', title: 'FATAL', render: function (data, type, row, meta) {
                            return `<h4><span class="label" style="${setColorBadgeIndicadores(row.cantidadFatal)}">${row.cantidadFatal}</span></h4>`;
                        }
                    },
                    {
                        data: 'cantidadLTA', title: 'LTI', render: function (data, type, row, meta) {
                            return `<h4><span class="label" style="${setColorBadgeIndicadores(row.cantidadLTA)}">${row.cantidadLTA}</span></h4>`;
                        }
                    },
                    {
                        data: 'cantidadATR', title: 'MDI', render: function (data, type, row, meta) {
                            return `<h4><span class="label" style="${setColorBadgeIndicadores(row.cantidadATR)}">${row.cantidadATR}</span></h4>`;
                        }
                    },
                    {
                        data: 'cantidadATM', title: 'MTI', render: function (data, type, row, meta) {
                            return `<h4><span class="label" style="${setColorBadgeIndicadores(row.cantidadATM)}">${row.cantidadATM}</span></h4>`;
                        }
                    },
                    {
                        data: 'cantidadAPA', title: 'FAI', render: function (data, type, row, meta) {
                            return `<h4><span class="label" style="${setColorBadgeIndicadores(row.cantidadAPA)}">${row.cantidadAPA}</span></h4>`;
                        }
                    },
                    {
                        data: 'cantidadDAMEQ', title: 'PD', render: function (data, type, row, meta) {
                            return `<h4><span class="label" style="${setColorBadgeIndicadores(row.cantidadDAMEQ)}">${row.cantidadDAMEQ}</span></h4>`;
                        }
                    },
                    {
                        data: 'cantidadNM', title: 'NM', render: function (data, type, row, meta) {
                            return `<h4><span class="label" style="${setColorBadgeIndicadores(row.cantidadNM)}">${row.cantidadNM}</span></h4>`;
                        }
                    },
                    {
                        data: 'cantidadOI', title: 'OI', render: function (data, type, row, meta) {
                            return `<h4><span class="label" style="${setColorBadgeIndicadores(row.cantidadOI)}">${row.cantidadOI}</span></h4>`;
                        }
                    },
                    {
                        data: 'cantidadEI', title: 'EI', render: function (data, type, row, meta) {
                            return `<h4><span class="label" style="${setColorBadgeIndicadores(row.cantidadEI)}">${row.cantidadEI}</span></h4>`;
                        }
                    },
                    {
                        data: 'horasHombre', title: 'HHT', render: function (data, type, row, meta) {
                            return formatValue(data);
                        }
                    },
                    {
                        data: 'LTIFR', title: 'LTIFR', render: function (data, type, row, meta) {
                            return formatValue(row.LTIFR.toFixed(2));
                        }
                    },
                    {
                        data: 'TRIFR', title: 'TRIFR', render: function (data, type, row, meta) {
                            return formatValue(row.TRIFR.toFixed(2));
                        }
                    },
                    {
                        data: 'TIFR', title: 'TIFR', render: function (data, type, row, meta) {
                            return formatValue(row.TIFR.toFixed(2));
                        }
                    },
                    {
                        data: 'severidad', title: 'Severity Rate', render: function (data, type, row, meta) {
                            return formatValue(row.severidad.toFixed(2));
                        }
                    },
                    {
                        data: 'IFA', title: 'IFA', render: function (data, type, row, meta) {
                            return formatValue(row.IFA.toFixed(2));
                        }
                    },
                    {
                        data: 'ISA', title: 'ISA', render: function (data, type, row, meta) {
                            return formatValue(row.ISA.toFixed(2));
                        }
                    },
                    {
                        data: 'IA', title: 'IA', render: function (data, type, row, meta) {
                            return formatValue(row.IA.toFixed(2));
                        }
                    },
                    {
                        data: 'lostDays', title: 'Lost Days', render: function (data, type, row, meta) {
                            return formatValue(row.lostDays);
                        }
                    },
                    { data: 'orden', title: 'orden' },
                ],
                columnDefs: [
                    {
                        targets: -1,
                        visible: false,
                        type: hidden,
                    },
                    { className: "dt-center", "targets": "_all" },
                ]
            });

            let flagMostrarColumnasPeru = +$('#inputEmpresaActual').val() == 6;

            tblIncidenciasPresentadas.DataTable().column(15).visible(flagMostrarColumnasPeru);
            tblIncidenciasPresentadas.DataTable().column(16).visible(flagMostrarColumnasPeru);
            tblIncidenciasPresentadas.DataTable().column(17).visible(flagMostrarColumnasPeru);

            tblIncidenciasPresentadas.DataTable().buttons().container().appendTo($('#divBotonExcel'));
        }
        //#endregion

        //#region ACCIDENTABILIDAD
        function setCantidadesAccidentabilidad(accidentabilidad) {
            $('#indicadorDiaCantidad').append(accidentabilidad.dia);
            $('#indicadorDiaPercent').append(accidentabilidad.porcentajeDia + '%');
            $('#indicadorHoraCantidad').append(accidentabilidad.hora);
            $('#indicadorHoraPercent').append(accidentabilidad.porcentajeHora + '%');
            $('#indicadorTurnoCantidad').append(accidentabilidad.turno);
            $('#indicadorTurnoPercent').append(accidentabilidad.porcentajeTurno + '%');
            $('#indicadorProyectoCantidad').append(accidentabilidad.cc);
            $('#indicadorProyectoPercent').append(accidentabilidad.porcentajeCC + '%');
            $('#indicadorActividadCantidad').append(accidentabilidad.actividad);
            $('#indicadorActividadPercent').append(accidentabilidad.porcentajeActividad + '%');
            $('#indicadorTareaCantidad').append(accidentabilidad.tarea);
            $('#indicadorTareaPercent').append(accidentabilidad.porcentajeTarea + '%');
            $('#indicadorAgenteCantidad').append(accidentabilidad.agente);
            $('#indicadorAgentePercent').append(accidentabilidad.porcentajeAgente + '%');
            $('#indicadorEdadCantidad').append(accidentabilidad.edad);
            $('#indicadorEdadPercent').append(accidentabilidad.porcentajeEdad + '%');
            $('#indicadorPuestoCantidad').append(accidentabilidad.puesto);
            $('#indicadorPuestoPercent').append(accidentabilidad.porcentajePuesto + '%');
            $('#indicadorExperienciaCantidad').append(accidentabilidad.experiencia);
            $('#indicadorExperienciaPercent').append(accidentabilidad.porcentajeExperiencia + '%');
            $('#indicadorAntiguedadCantidad').append(accidentabilidad.antiguedadEmpresa);
            $('#indicadorAntiguedadPercent').append(accidentabilidad.porcentajeAntiguedadEmpresa + '%');
            $('#indicadorDiasTrabajadosCantidad').append(accidentabilidad.diasTrabajados);
            $('#indicadorDiasTrabajadosPercent').append(accidentabilidad.porcentajeDiasTrabajados + '%');
            $('#indicadorDepartamentoCantidad').append(accidentabilidad.departamento);
            $('#indicadorDepartamentoPercent').append(accidentabilidad.porcentajeDepartamento + '%');
            $('#indicadorLugarCantidad').append(accidentabilidad.lugar);
            $('#indicadorLugarPercent').append(accidentabilidad.porcentajeLugar + '%');
            $('#indicadorTipoContactoCantidad').append(accidentabilidad.tipoContacto);
            $('#indicadorTipoContactoPercent').append(accidentabilidad.porcentajeTipoContacto + '%');
            $('#indicadorCapacitadoCantidad').append(accidentabilidad.capacitado);
            $('#indicadorCapacitadoPercent').append(accidentabilidad.porcentajeCapacitado + '%');
            $('#indicadorProtocoloTrabajoCantidad').append(accidentabilidad.protocoloTrabajo);
            $('#indicadorProtocoloTrabajoPercent').append(accidentabilidad.porcentajeProtocolo + '%');
            $('#indicadorPotencialSeveridadCantidad').append(accidentabilidad.potencialSeveridad);
            $('#indicadorPotencialSeveridadPercent').append(accidentabilidad.porcentajePotencial + '%');
        }

        function GetDatosGraficasAccidentabilidad() {

            let obj = getFormAccidentabilidad();

            axios.post("GetDatosLesionesPersonal", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    setChartLesionesRegionAnatomica(response.data.conceptosRegionAnatomica, response.data.datosRegionAnatomica);
                    setChartLesionesCausasInmediatas(response.data.conceptosCausasInmediatas, response.data.datosCausasInmediatas);

                }
            }).catch(error => Alert2Error(error.message));

            axios.post("GetDatosDañosMateriales", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    setChartDañosCausasInmediatas(response.data.conceptosCausasInmediatas, response.data.datosCausasInmediatas);
                    setChartDañosProtocoloFatalidad(response.data.conceptosProtocoloFatalidada, response.data.datosProtocoloFatalidada);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
        //#region CAUSA INCIDENCIAS
        function setCantidadesCausasIndicadores(incidentesIndicadores) {
            $('#indicadorAlturasCantidad').append(incidentesIndicadores.alturas + '%');
            //$('#indicadorTotalHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadTotalIndicador))
            $('#indicadorCorteSoldaduraCantidad').append(incidentesIndicadores.corteSoldadura + '%');
            //$('#indicadorFatalHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadFatalIndicador))
            $('#indicadorEspaciosConfinadosCantidad').append(incidentesIndicadores.espaciosConfinados + '%');
            //$('#indicadorLTAHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadLTAIndicador))
            $('#indicadorExcavacionesCantidad').append(incidentesIndicadores.excavaciones + '%');
            //$('#indicadorATRHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadATRIndicador))
            $('#indicadorControlEnergiasCantidad').append(incidentesIndicadores.controlEnergias + '%');
            // $('#indicadorATMHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadATMIndicador))
            $('#indicadorManejoDefensivoCantidad').append(incidentesIndicadores.manejoDefensivo + '%');
            //$('#indicadorAPAHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadAPAIndicador))
            $('#indicadorManipulacionCargasCantidad').append(incidentesIndicadores.manipulacionCargas + '%');
            //$('#indicadorDAMEQHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadDAMEQIndicador))
            $('#indicadorEstabilizacionCantidad').append(incidentesIndicadores.estabilizacionTaludez + '%');
            //$('#indicadorNMHeader').addClass(setColoresIndicadores(incidentesIndicadores.cantidadNMIndicador))
            $('#indicadorSustanciaQuimicaCantidad').append(incidentesIndicadores.sustanciasQuimicas + '%');
            $('#indicadorVoladuraCantidad').append(incidentesIndicadores.voladura + '%');
            $('#indicadorNDCantidad').append(incidentesIndicadores.nd + '%');
        }
        //#endregion

        //#region METODOS GENERALES
        $('.indicador').on('click', function () {
            const clase = $(this).attr("class").split(' ')[2];
            const titulo = $(this).find('.text').text();
            $('#tblDetalleTipoIndicador tbody tr').remove();
            getIncidenciasPresentadasTipo(titulo).done(function (response) {
                if (response.success) {
                    response.indicadores.forEach(element => $('#tblDetalleTipoIndicador tbody').append(setRowDetalleIndicadores(element)));
                    $('#modalDetalleIndicadores').css({ display: "block" });
                    $('#modalContent').removeAttr('style');
                    $('#modalContent').attr('style', setColorModal(clase));
                    $('#modalTitle').append(titulo);
                }
            });
        });
        $('#closeModal').on('click', function () {
            const clase = $(this).parent().attr("class").split(' ')[1]
            $('#modalDetalleIndicadores').css({ display: "none" });
            // $('#modalContent').removeClass(clase);
            $('#modalTitle').html('')
        });
        $('#closeModalAccidentabilidad').on('click', function () {
            const clase = $(this).parent().attr("class").split(' ')[1]
            $('#modalDetalleAccidentabilidad').css({ display: "none" });
            // $('#modalContentAccidentabilidad').removeClass(clase);
            $('#modalTitleAccidentabilidad').html('')
        });
        $(".divAccidentabilidad").on('click', function () {
            let busq = getFormAccidentabilidad();
            busq.tipoAccidente = $(this).data().tipo;
            getAccidentabilidadTop(busq).done(response => {
                $('#tblDetalleTipoAccidentabilidad tbody tr').remove();
                if (response.success) {
                    response.indicadores.lst.forEach(element => {
                        $('#tblDetalleTipoAccidentabilidad tbody').append(setRowDetalleAccidentalidad(element));
                    });
                    $('#modalDetalleAccidentabilidad').css({ display: "block" });
                    $('#modalContentAccidentabilidad').removeAttr('style');
                    $('#modalContentAccidentabilidad').attr('style', setColorModal('label-warning'));
                    $('#modalTitleAccidentabilidad').append(response.indicadores.titulo);
                }
            }).catch(o_O => { });
        });

        $('#botonMetas').click(cargarMetas);
        $('#botonMetasTIFR').click(cargarMetas);
        $('#botonMetasTPDFR').click(cargarMetas);

        function cargarMetas() {
            $('#inputNombreMeta').val('');
            $('#inputValorMeta').val('');
            $('#inputAñoMeta').val(new Date().getFullYear());
            obtenerMetasGrafica()
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        dtTablaMetas.clear().rows.add(response.items).draw();
                        $('#modalMetas').modal('show');
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        dtTablaMetas.clear().draw();
                    }
                }, error => AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`));
        }

        $('#botonAgregarMeta').click(() => {
            const nombre = $('#inputNombreMeta').val().trim();
            const valor = $('#inputValorMeta').val();
            const año = $('#inputAñoMeta').val();
            const tipoGrafica = $('#selectTipoGrafica').val();
            const colorString = $('#inputColor').val();

            if (nombre == "" || valor <= 0 || año <= 0 || tipoGrafica <= 0 || colorString == "") {
                AlertaGeneral(`Aviso`, `Información incompleta.`);
                return;
            }

            agregarMetaGrafica({ nombre, valor, año, tipoGrafica, colorString })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Meta agregada correctamente`);
                        cargarMetas();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`));
        });

        $('#selectTipoCarga').change(() => {
            getTasaIncidencias().done(function (response) {
                if (response.success) {
                    if (chartTasaIncidencias) {
                        chartTasaIncidencias.destroy();
                    }
                    setChartTasaIncidencias(response.labels, response.datasets);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            });
        })

        $('#selectTipoCargaTIFR').change(() => {
            getTIFR().done(function (response) {
                if (response.success) {
                    if (chartTIFR) {
                        chartTIFR.destroy();
                    }
                    setChartTIFR(response.labels, response.datasets);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            });
        })

        $('#selectTipoCargaTPDFR').change(() => {
            getTPDFR().done(function (response) {
                if (response.success) {
                    if (chartTIFR) {
                        chartTIFR.destroy();
                    }
                    setChartTPDFR(response.labels, response.datasets);
                } else if (response.EMPTY) {
                    AlertaGeneral(`Aviso`, `No se encontraron registros con los filtros especificados.`);
                }
            });
        })

        $('#botonReporteGlobal').click(() => {
            AlertaAceptarRechazarNormal(
                'Confirmar acción',
                '¿Está seguro que desea enviar el reporte semanal?',
                () => verReporte())
        });

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }
        //#endregion

        function verReporte() {
            const tipoReporteSemanal = 1;

            $.blockUI({ message: 'Generando reporte global...' });
            var path = `/Reportes/Vista.aspx?idReporte=177&tipoReporte=${tipoReporteSemanal}&isCRModal=${true}&inMemory=${1}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                enviarCorreoReporteGlobal();
            };
        }

        function enviarCorreoReporteGlobal() {
            $.post('/Administrativo/IndicadoresSeguridad/EnviarCorreoReporteGlobal')
                .always(() => {
                    $.unblockUI();
                })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Correo enviado correctamente.`);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error el enviar el correo.`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }
    }
    $(document).ready(() => Dashboard.Seguridad = new Seguridad())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();