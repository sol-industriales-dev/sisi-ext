(() => {
    $.namespace('Administrativo.RecursosHumanos.Capacitacion.MatrizNecesidades');

    MatrizNecesidades = function () {

        // Filtros.
        const comboCplan = $('#comboCplan');
        const comboArr = $('#comboArr');
        const comboArea = $('#comboArea');
        const comboClasificacion = $('#comboClasificacion');
        const comboMando = $('#comboMando');
        const botonBuscar = $('#botonBuscar');
        const botonConsultarPersonalActivo = $('#botonConsultarPersonalActivo');
        const botonConsultarEstadisticasIndividuales = $('#botonConsultarEstadisticasIndividuales');
        const botonConsultarMandoAdministrativoProtocoloFatalidad = $('#botonConsultarMandoAdministrativoProtocoloFatalidad');
        const botonConsultarMandoMedioProtocoloFatalidad = $('#botonConsultarMandoMedioProtocoloFatalidad');
        const botonConsultarMandoOperativoProtocoloFatalidad = $('#botonConsultarMandoOperativoProtocoloFatalidad');
        const botonConsultarMandoAdministrativoNormativo = $('#botonConsultarMandoAdministrativoNormativo');
        const botonConsultarMandoMedioNormativo = $('#botonConsultarMandoMedioNormativo');
        const botonConsultarMandoOperativoNormativo = $('#botonConsultarMandoOperativoNormativo');
        const botonConsultarMandoAdministrativoTecnicoOperativo = $('#botonConsultarMandoAdministrativoTecnicoOperativo');
        const botonConsultarMandoMedioTecnicoOperativo = $('#botonConsultarMandoMedioTecnicoOperativo');
        const botonConsultarMandoOperativoTecnicoOperativo = $('#botonConsultarMandoOperativoTecnicoOperativo');
        const botonConsultarMandoAdministrativoInstructivoOperativo = $('#botonConsultarMandoAdministrativoInstructivoOperativo');
        const botonConsultarMandoMedioInstructivoOperativo = $('#botonConsultarMandoMedioInstructivoOperativo');
        const botonConsultarMandoOperativoInstructivoOperativo = $('#botonConsultarMandoOperativoInstructivoOperativo');

        const botonExportarExcel = $('#botonExportarExcel');
        let tablaPersonalActivo = $('#tablaPersonalActivo');
        let dtTablaPersonalActivo = null;

        let tablaEstadisticas = $('#tablaEstadisticas');
        let dtTablaEstadisticas = null;

        const tablaIndicadoresGlobales = $('#tablaIndicadoresGlobales');
        let dtTablaIndicadoresGlobales = null;

        const spanPorcentajeGlobal = $('#spanPorcentajeGlobal');
        const spanTotalPersonal = $('#spanTotalPersonal');

        const bodyTablaRequeridos = $('#bodyTablaRequeridos');
        const bodyTablaCapacitados = $('#bodyTablaCapacitados');
        const bodyTablaRestantes = $('#bodyTablaRestantes');

        // Gráfica General
        let chartPorcentajeTotalClasificacion = null;

        // Gráficas por clasificación
        let chartPorcentajeProtocolo = null;
        let chartCantidadProtocolo = null;

        let chartPorcentajeNormativo = null;
        let chartCantidadNormativo = null;

        let chartPorcentajeGeneral = null;
        let chartCantidadGeneral = null;

        let chartPorcentajeFormativo = null;
        let chartCantidadFormativo = null;

        let chartPorcentajeInstructivo = null;
        let chartCantidadInstructivo = null;

        let chartPorcentajeTecnico = null;
        let chartCantidadTecnico = null;

        //Gráficas de Capacitación Operativa
        let chartPorcentajeMandoAdministrativoProtocoloFatalidad = null;
        let chartCantidadMandoAdministrativoProtocoloFatalidad = null;

        let chartPorcentajeMandoMedioProtocoloFatalidad = null;
        let chartCantidadMandoMedioProtocoloFatalidad = null;

        let chartPorcentajeMandoOperativoProtocoloFatalidad = null;
        let chartCantidadMandoOperativoProtocoloFatalidad = null;

        let chartPorcentajeMandoAdministrativoNormativo = null;
        let chartCantidadMandoAdministrativoNormativo = null;

        let chartPorcentajeMandoMedioNormativo = null;
        let chartCantidadMandoMedioNormativo = null;

        let chartPorcentajeMandoOperativoNormativo = null;
        let chartCantidadMandoOperativoNormativo = null;

        let chartPorcentajeMandoAdministrativoTecnicoOperativo = null;
        let chartCantidadMandoAdministrativoTecnicoOperativo = null;

        let chartPorcentajeMandoMedioTecnicoOperativo = null;
        let chartCantidadMandoMedioTecnicoOperativo = null;

        let chartPorcentajeMandoOperativoTecnicoOperativo = null;
        let chartCantidadMandoOperativoTecnicoOperativo = null;

        let chartPorcentajeMandoAdministrativoInstructivoOperativo = null;
        let chartCantidadMandoAdministrativoInstructivoOperativo = null;

        let chartPorcentajeMandoMedioInstructivoOperativo = null;
        let chartCantidadMandoMedioInstructivoOperativo = null;

        let chartPorcentajeMandoOperativoInstructivoOperativo = null;
        let chartCantidadMandoOperativoInstructivoOperativo = null;

        const SECCIONES = {
            PERSONAL_ACTIVO: 1,
            ESTADISTICAS_INDIVIDUALES: 2,
            MANDO_ADMINISTRATIVO_PROTOCOLO_FATALIDAD: 3,
            MANDO_MEDIO_PROTOCOLO_FATALIDAD: 4,
            MANDO_OPERATIVO_PROTOCOLO_FATALIDAD: 5,
            MANDO_ADMINISTRATIVO_NORMATIVO: 6,
            MANDO_MEDIO_NORMATIVO: 7,
            MANDO_OPERATIVO_NORMATIVO: 8,
            MANDO_ADMINISTRATIVO_TECNICO_OPERATIVO: 9,
            MANDO_MEDIO_TECNICO_OPERATIVO: 10,
            MANDO_OPERATIVO_TECNICO_OPERATIVO: 11,
            MANDO_ADMINISTRATIVO_INSTRUCTIVO_OPERATIVO: 12,
            MANDO_MEDIO_INSTRUCTIVO_OPERATIVO: 13,
            MANDO_OPERATIVO_INSTRUCTIVO_OPERATIVO: 14
        };

        (function init() {
            // Lógica de inicialización.
            llenarCombos();
            agregarListeners();
            initTablaEstadisticas();
            initTablaIndicadoresGlobales();
            botonExportarExcel.hide();
            ocultarGraficasCapacitacionOperativa();
        })();

        // Métodos.
        function llenarCombos() {

            // comboArea.fillCombo('/Administrativo/CapacitacionCapitalHumano/GetAreas', null, false, 'Todos');
            convertToMultiselect('#comboArea');
            comboClasificacion.fillCombo('/Administrativo/CapacitacionCapitalHumano/GetClasificacionCursos', null, false, 'Todos');
            convertToMultiselect('#comboClasificacion');
            comboCplan.fillCombo('/Administrativo/CapacitacionCapitalHumano/ObtenerComboCCEnKontrol', { empresa: 1 }, false, 'Todos');
            convertToMultiselect('#comboCplan');
            comboArr.fillCombo('/Administrativo/CapacitacionCapitalHumano/ObtenerComboCCEnKontrol', { empresa: 2 }, false, 'Todos');
            convertToMultiselect('#comboArr');
            comboMando.fillCombo('/Administrativo/CapacitacionCapitalHumano/GetMandosEnum', null, false, 'Todos');
            convertToMultiselect('#comboMando');
        }

        function agregarListeners() {
            $('.comboChange').change(cargarAreasCC);
            botonBuscar.click(cargarDatosMatrizNecesidades);

            botonConsultarPersonalActivo.click(() => { cargarDatosSeccion(SECCIONES.PERSONAL_ACTIVO) });
            botonConsultarEstadisticasIndividuales.click(() => { cargarDatosSeccion(SECCIONES.ESTADISTICAS_INDIVIDUALES) });
            botonConsultarMandoAdministrativoProtocoloFatalidad.click(() => { cargarDatosSeccion(SECCIONES.MANDO_ADMINISTRATIVO_PROTOCOLO_FATALIDAD) });
            botonConsultarMandoMedioProtocoloFatalidad.click(() => { cargarDatosSeccion(SECCIONES.MANDO_MEDIO_PROTOCOLO_FATALIDAD) });
            botonConsultarMandoOperativoProtocoloFatalidad.click(() => { cargarDatosSeccion(SECCIONES.MANDO_OPERATIVO_PROTOCOLO_FATALIDAD) });
            botonConsultarMandoAdministrativoNormativo.click(() => { cargarDatosSeccion(SECCIONES.MANDO_ADMINISTRATIVO_NORMATIVO) });
            botonConsultarMandoMedioNormativo.click(() => { cargarDatosSeccion(SECCIONES.MANDO_MEDIO_NORMATIVO) });
            botonConsultarMandoOperativoNormativo.click(() => { cargarDatosSeccion(SECCIONES.MANDO_OPERATIVO_NORMATIVO) });
            botonConsultarMandoAdministrativoTecnicoOperativo.click(() => { cargarDatosSeccion(SECCIONES.MANDO_ADMINISTRATIVO_TECNICO_OPERATIVO) });
            botonConsultarMandoMedioTecnicoOperativo.click(() => { cargarDatosSeccion(SECCIONES.MANDO_MEDIO_TECNICO_OPERATIVO) });
            botonConsultarMandoOperativoTecnicoOperativo.click(() => { cargarDatosSeccion(SECCIONES.MANDO_OPERATIVO_TECNICO_OPERATIVO) });
            botonConsultarMandoAdministrativoInstructivoOperativo.click(() => { cargarDatosSeccion(SECCIONES.MANDO_ADMINISTRATIVO_INSTRUCTIVO_OPERATIVO) });
            botonConsultarMandoMedioInstructivoOperativo.click(() => { cargarDatosSeccion(SECCIONES.MANDO_MEDIO_INSTRUCTIVO_OPERATIVO) });
            botonConsultarMandoOperativoInstructivoOperativo.click(() => { cargarDatosSeccion(SECCIONES.MANDO_OPERATIVO_INSTRUCTIVO_OPERATIVO) });

            botonExportarExcel.click(descargarExcelPersonalActivo);
        }


        function cargarDatosMatrizNecesidades() {
            const ccsCplan = getValoresMultiples('#comboCplan');
            const ccsArr = getValoresMultiples('#comboArr');
            const departamentosIDs = getValoresMultiples('#comboArea');
            const clasificaciones = getValoresMultiples('#comboClasificacion');
            const mandos = getValoresMultiples('#comboMando');

            if (ccsCplan.length == 0 && ccsArr.length == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar por lo menos un centro de costo de una empresa.`);
                return;
            } else if (departamentosIDs.length == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar por lo menos un área operativa.`);
                return;
            }

            axios.post('/Administrativo/CapacitacionCapitalHumano/CargarDatosMatrizNecesidades', { ccsCplan, ccsArr, departamentosIDs, clasificaciones, mandos })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        limpiarIndicadoresGlobales();
                        mostrarIndicadoresGlobales(response.data);

                        AddRows(tablaIndicadoresGlobales, response.data.tablaIndicadoresGlobales);

                        initChartPorcentajeTotalClasificacion('Porcentaje de cumplimiento por clasificación', chartPorcentajeTotalClasificacion,
                            'chartPorcentajeTotalClasificacion', response.data.porcentajeTotalClasificacion.labels, response.data.porcentajeTotalClasificacion.datasets, false);

                        $('#panelIndicadoresGlobales').addClass('show');
                        $('#panelPorcentajeTotalClasificacion').addClass('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarDatosSeccion(seccion) {
            const ccsCplan = getValoresMultiples('#comboCplan');
            const ccsArr = getValoresMultiples('#comboArr');
            const departamentosIDs = getValoresMultiples('#comboArea');
            const clasificaciones = getValoresMultiples('#comboClasificacion');
            const mandos = getValoresMultiples('#comboMando');

            if (ccsCplan.length == 0 && ccsArr.length == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar por lo menos un centro de costo de una empresa.`);
                return;
            } else if (departamentosIDs.length == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar por lo menos un área operativa.`);
                return;
            }

            axios.post('/Administrativo/CapacitacionCapitalHumano/CargarDatosSeccionMatriz', { ccsCplan, ccsArr, departamentosIDs, clasificaciones, seccion, mandos })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        mostrarDatosSeccion(seccion, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function mostrarDatosSeccion(seccion, response) {
            switch (seccion) {
                case SECCIONES.PERSONAL_ACTIVO:
                    if (response.listaEmpleadosRelacionCursos.length > 0) {
                        establecerColumnasCursos(response.listaEmpleadosRelacionCursos, response.columnasCursos);
                        botonExportarExcel.show(500);
                        $('#panelPersonalActivo').addClass('show');
                        tablaPersonalActivo.DataTable().columns.adjust().draw();
                    } else {
                        AlertaGeneral(`Aviso`, `No se encontraron empleados con los parámetros indicados.`);
                    }
                    break;
                case SECCIONES.ESTADISTICAS_INDIVIDUALES:
                    AddRows(tablaEstadisticas, response.estadisticas);
                    $('#panelEstadisticas').addClass('show');
                    tablaEstadisticas.DataTable().columns.adjust().draw();
                    break;
                case SECCIONES.MANDO_ADMINISTRATIVO_PROTOCOLO_FATALIDAD:
                    initChartPorcentajeMandoAdministrativoProtocoloFatalidad(
                        'Porcentaje de cumplimiento - Mando Administrativo - Protocolos de Fatalidad',
                        chartPorcentajeMandoAdministrativoProtocoloFatalidad,
                        'chartPorcentajeMandoAdministrativoProtocoloFatalidad',
                        response.porcentajeMandoAdministrativoProtocoloFatalidad.labels,
                        response.porcentajeMandoAdministrativoProtocoloFatalidad.datasets,
                        false
                    );
                    initChartQuantityCantidadMandoAdministrativoProtocoloFatalidad(
                        'Cantidad de capacitaciones - Mando Administrativo - Protocolos de Fatalidad',
                        chartCantidadMandoAdministrativoProtocoloFatalidad,
                        'chartCantidadMandoAdministrativoProtocoloFatalidad',
                        response.chartCantidadMandoAdministrativoProtocoloFatalidad.labels,
                        response.chartCantidadMandoAdministrativoProtocoloFatalidad.datasets,
                        true
                    );
                    $('#panelMandoAdministrativoProtocoloFatalidad').addClass('show');
                    break;
                case SECCIONES.MANDO_MEDIO_PROTOCOLO_FATALIDAD:
                    initChartPorcentajeMandoMedioProtocoloFatalidad(
                        'Porcentaje de cumplimiento - Mando Medio - Protocolos de Fatalidad',
                        chartPorcentajeMandoMedioProtocoloFatalidad,
                        'chartPorcentajeMandoMedioProtocoloFatalidad',
                        response.porcentajeMandoMedioProtocoloFatalidad.labels,
                        response.porcentajeMandoMedioProtocoloFatalidad.datasets,
                        false
                    );
                    initChartQuantityCantidadMandoMedioProtocoloFatalidad(
                        'Cantidad de capacitaciones - Mando Medio - Protocolos de Fatalidad',
                        chartCantidadMandoMedioProtocoloFatalidad,
                        'chartCantidadMandoMedioProtocoloFatalidad',
                        response.chartCantidadMandoMedioProtocoloFatalidad.labels,
                        response.chartCantidadMandoMedioProtocoloFatalidad.datasets,
                        true
                    );
                    $('#panelMandoMedioProtocoloFatalidad').addClass('show');
                    break;
                case SECCIONES.MANDO_OPERATIVO_PROTOCOLO_FATALIDAD:
                    initChartPorcentajeMandoOperativoProtocoloFatalidad(
                        'Porcentaje de cumplimiento - Mando Operativo - Protocolos de Fatalidad',
                        chartPorcentajeMandoOperativoProtocoloFatalidad,
                        'chartPorcentajeMandoOperativoProtocoloFatalidad',
                        response.porcentajeMandoOperativoProtocoloFatalidad.labels,
                        response.porcentajeMandoOperativoProtocoloFatalidad.datasets,
                        false
                    );
                    initChartQuantityCantidadMandoOperativoProtocoloFatalidad(
                        'Cantidad de capacitaciones - Mando Operativo - Protocolos de Fatalidad',
                        chartCantidadMandoOperativoProtocoloFatalidad,
                        'chartCantidadMandoOperativoProtocoloFatalidad',
                        response.chartCantidadMandoOperativoProtocoloFatalidad.labels,
                        response.chartCantidadMandoOperativoProtocoloFatalidad.datasets,
                        true
                    );
                    $('#panelMandoOperativoProtocoloFatalidad').addClass('show');
                    break;
                case SECCIONES.MANDO_ADMINISTRATIVO_NORMATIVO:
                    initChartPorcentajeMandoAdministrativoNormativo(
                        'Porcentaje de cumplimiento - Mando Administrativo - Normativo',
                        chartPorcentajeMandoAdministrativoNormativo,
                        'chartPorcentajeMandoAdministrativoNormativo',
                        response.porcentajeMandoAdministrativoNormativo.labels,
                        response.porcentajeMandoAdministrativoNormativo.datasets,
                        false
                    );
                    initChartQuantityCantidadMandoAdministrativoNormativo(
                        'Cantidad de capacitaciones - Mando Administrativo - Normativo',
                        chartCantidadMandoAdministrativoNormativo,
                        'chartCantidadMandoAdministrativoNormativo',
                        response.chartCantidadMandoAdministrativoNormativo.labels,
                        response.chartCantidadMandoAdministrativoNormativo.datasets,
                        true
                    );
                    $('#panelMandoAdministrativoNormativo').addClass('show');
                    break;
                case SECCIONES.MANDO_MEDIO_NORMATIVO:
                    initChartPorcentajeMandoMedioNormativo(
                        'Porcentaje de cumplimiento - Mando Medio - Normativo',
                        chartPorcentajeMandoMedioNormativo,
                        'chartPorcentajeMandoMedioNormativo',
                        response.porcentajeMandoMedioNormativo.labels,
                        response.porcentajeMandoMedioNormativo.datasets,
                        false
                    );
                    initChartQuantityCantidadMandoMedioNormativo(
                        'Cantidad de capacitaciones - Mando Medio - Normativo',
                        chartCantidadMandoMedioNormativo,
                        'chartCantidadMandoMedioNormativo',
                        response.chartCantidadMandoMedioNormativo.labels,
                        response.chartCantidadMandoMedioNormativo.datasets,
                        true
                    );
                    $('#panelMandoMedioNormativo').addClass('show');
                    break;
                case SECCIONES.MANDO_OPERATIVO_NORMATIVO:
                    initChartPorcentajeMandoOperativoNormativo(
                        'Porcentaje de cumplimiento - Mando Operativo - Normativo',
                        chartPorcentajeMandoOperativoNormativo,
                        'chartPorcentajeMandoOperativoNormativo',
                        response.porcentajeMandoOperativoNormativo.labels,
                        response.porcentajeMandoOperativoNormativo.datasets,
                        false
                    );
                    initChartQuantityCantidadMandoOperativoNormativo(
                        'Cantidad de capacitaciones - Mando Operativo - Normativo',
                        chartCantidadMandoOperativoNormativo,
                        'chartCantidadMandoOperativoNormativo',
                        response.chartCantidadMandoOperativoNormativo.labels,
                        response.chartCantidadMandoOperativoNormativo.datasets,
                        true
                    );
                    $('#panelMandoOperativoNormativo').addClass('show');
                    break;
                case SECCIONES.MANDO_ADMINISTRATIVO_TECNICO_OPERATIVO:
                    initChartPorcentajeMandoAdministrativoTecnicoOperativo(
                        'Porcentaje de cumplimiento - Mando Administrativo - Técnico Operativo',
                        chartPorcentajeMandoAdministrativoTecnicoOperativo,
                        'chartPorcentajeMandoAdministrativoTecnicoOperativo',
                        response.porcentajeMandoAdministrativoTecnicoOperativo.labels,
                        response.porcentajeMandoAdministrativoTecnicoOperativo.datasets,
                        false
                    );
                    initChartQuantityCantidadMandoAdministrativoTecnicoOperativo(
                        'Cantidad de capacitaciones - Mando Administrativo - Técnico Operativo',
                        chartCantidadMandoAdministrativoTecnicoOperativo,
                        'chartCantidadMandoAdministrativoTecnicoOperativo',
                        response.chartCantidadMandoAdministrativoTecnicoOperativo.labels,
                        response.chartCantidadMandoAdministrativoTecnicoOperativo.datasets,
                        true
                    );
                    $('#panelMandoAdministrativoTecnicoOperativo').addClass('show');
                    break;
                case SECCIONES.MANDO_MEDIO_TECNICO_OPERATIVO:
                    initChartPorcentajeMandoMedioTecnicoOperativo(
                        'Porcentaje de cumplimiento - Mando Medio - Técnico Operativo',
                        chartPorcentajeMandoMedioTecnicoOperativo,
                        'chartPorcentajeMandoMedioTecnicoOperativo',
                        response.porcentajeMandoMedioTecnicoOperativo.labels,
                        response.porcentajeMandoMedioTecnicoOperativo.datasets,
                        false
                    );
                    initChartQuantityCantidadMandoMedioTecnicoOperativo(
                        'Cantidad de capacitaciones - Mando Medio - Técnico Operativo',
                        chartCantidadMandoMedioTecnicoOperativo,
                        'chartCantidadMandoMedioTecnicoOperativo',
                        response.chartCantidadMandoMedioTecnicoOperativo.labels,
                        response.chartCantidadMandoMedioTecnicoOperativo.datasets,
                        true
                    );
                    $('#panelMandoMedioTecnicoOperativo').addClass('show');
                    break;
                case SECCIONES.MANDO_OPERATIVO_TECNICO_OPERATIVO:
                    initChartPorcentajeMandoOperativoTecnicoOperativo(
                        'Porcentaje de cumplimiento - Mando Operativo - Técnico Operativo',
                        chartPorcentajeMandoOperativoTecnicoOperativo,
                        'chartPorcentajeMandoOperativoTecnicoOperativo',
                        response.porcentajeMandoOperativoTecnicoOperativo.labels,
                        response.porcentajeMandoOperativoTecnicoOperativo.datasets,
                        false
                    );
                    initChartQuantityCantidadMandoOperativoTecnicoOperativo(
                        'Cantidad de capacitaciones - Mando Operativo - Técnico Operativo',
                        chartCantidadMandoOperativoTecnicoOperativo,
                        'chartCantidadMandoOperativoTecnicoOperativo',
                        response.chartCantidadMandoOperativoTecnicoOperativo.labels,
                        response.chartCantidadMandoOperativoTecnicoOperativo.datasets,
                        true
                    );
                    $('#panelMandoOperativoTecnicoOperativo').addClass('show');
                    break;
                case SECCIONES.MANDO_ADMINISTRATIVO_INSTRUCTIVO_OPERATIVO:
                    initChartPorcentajeMandoAdministrativoInstructivoOperativo(
                        'Porcentaje de cumplimiento - Mando Administrativo - Instructivo Operativo',
                        chartPorcentajeMandoAdministrativoInstructivoOperativo,
                        'chartPorcentajeMandoAdministrativoInstructivoOperativo',
                        response.porcentajeMandoAdministrativoInstructivoOperativo.labels,
                        response.porcentajeMandoAdministrativoInstructivoOperativo.datasets,
                        false
                    );
                    initChartQuantityCantidadMandoAdministrativoInstructivoOperativo(
                        'Cantidad de capacitaciones - Mando Administrativo - Instructivo Operativo',
                        chartCantidadMandoAdministrativoInstructivoOperativo,
                        'chartCantidadMandoAdministrativoInstructivoOperativo',
                        response.chartCantidadMandoAdministrativoInstructivoOperativo.labels,
                        response.chartCantidadMandoAdministrativoInstructivoOperativo.datasets,
                        true
                    );
                    $('#panelMandoAdministrativoInstructivoOperativo').addClass('show');
                    break;
                case SECCIONES.MANDO_MEDIO_INSTRUCTIVO_OPERATIVO:
                    initChartPorcentajeMandoMedioInstructivoOperativo(
                        'Porcentaje de cumplimiento - Mando Medio - Instructivo Operativo',
                        chartPorcentajeMandoMedioInstructivoOperativo,
                        'chartPorcentajeMandoMedioInstructivoOperativo',
                        response.porcentajeMandoMedioInstructivoOperativo.labels,
                        response.porcentajeMandoMedioInstructivoOperativo.datasets,
                        false
                    );
                    initChartQuantityCantidadMandoMedioInstructivoOperativo(
                        'Cantidad de capacitaciones - Mando Medio - Instructivo Operativo',
                        chartCantidadMandoMedioInstructivoOperativo,
                        'chartCantidadMandoMedioInstructivoOperativo',
                        response.chartCantidadMandoMedioInstructivoOperativo.labels,
                        response.chartCantidadMandoMedioInstructivoOperativo.datasets,
                        true
                    );
                    $('#panelMandoMedioInstructivoOperativo').addClass('show');
                    break;
                case SECCIONES.MANDO_OPERATIVO_INSTRUCTIVO_OPERATIVO:
                    initChartPorcentajeMandoOperativoInstructivoOperativo(
                        'Porcentaje de cumplimiento - Mando Operativo - Instructivo Operativo',
                        chartPorcentajeMandoOperativoInstructivoOperativo,
                        'chartPorcentajeMandoOperativoInstructivoOperativo',
                        response.porcentajeMandoOperativoInstructivoOperativo.labels,
                        response.porcentajeMandoOperativoInstructivoOperativo.datasets,
                        false
                    );
                    initChartQuantityCantidadMandoOperativoInstructivoOperativo(
                        'Cantidad de capacitaciones - Mando Operativo - Instructivo Operativo',
                        chartCantidadMandoOperativoInstructivoOperativo,
                        'chartCantidadMandoOperativoInstructivoOperativo',
                        response.chartCantidadMandoOperativoInstructivoOperativo.labels,
                        response.chartCantidadMandoOperativoInstructivoOperativo.datasets,
                        true
                    );
                    $('#panelMandoOperativoInstructivoOperativo').addClass('show');
                    break;
            }
        }

        function mostrarIndicadoresGlobales(indicadores) {

            spanPorcentajeGlobal.html(indicadores.porcentajeGlobal);
            animateValue('spanTotalPersonal', 0, indicadores.totalEmpleados, 7000);

            agregarFileEncabezado(bodyTablaRequeridos, 'Requeridos', indicadores.totalRequeridos);
            agregarFileEncabezado(bodyTablaCapacitados, 'Capacitados', indicadores.totalCapacitados);
            agregarFileEncabezado(bodyTablaRestantes, 'Restantes', indicadores.totalRestantes);

            agregarFilasNormales(indicadores.departamentos);

        }

        function agregarFileEncabezado(elemento, titulo, valor) {
            elemento.append(`
                <tr>
                    <td class="blue-background">${titulo}</td>
                    <td class="normal-background"><span>${valor}</span></td>
                </tr>`);
        }

        function agregarFilasNormales(datos) {

            if (datos.length == 0) {
                return;
            }

            datos.forEach(x => {
                bodyTablaRequeridos.append(`
                    <tr>
                        <td class="normal-background">${x.departamento}</td>
                        <td class="normal-background"><span>${x.capacitados + x.restantes}</span></td>
                    </tr>`);
            });

            datos.forEach(x => {
                bodyTablaCapacitados.append(`
                    <tr>
                        <td class="normal-background">${x.departamento}</td>
                        <td class="normal-background"><span>${x.capacitados}</span></td>
                    </tr>`);
            });

            datos.forEach(x => {
                bodyTablaRestantes.append(`
                    <tr>
                        <td class="normal-background">${x.departamento}</td>
                        <td class="normal-background"><span>${x.restantes}</span></td>
                    </tr>`);
            });

        }

        function limpiarIndicadoresGlobales() {
            spanPorcentajeGlobal.html('0.00 %');
            spanTotalPersonal.html('0');
            bodyTablaRequeridos.empty();
            bodyTablaCapacitados.empty();
            bodyTablaRestantes.empty();
        }

        function establecerColumnasCursos(listadoEmpleados, columnasCursos) {

            const columnas = [
                { data: 'claveEmpleado', title: 'Clave' },
                { data: 'curp', title: 'CURP' },
                { data: 'nombre', title: 'Nombre' },
                { data: 'puesto', title: 'Puesto' },
                { data: 'ccDesc', title: 'Obra' }
            ];

            columnasCursos.forEach(x => columnas.push({ data: x.Item1, title: x.Item2 }));

            initTablaPersonalActivo(listadoEmpleados, columnas);
        }

        function initTablaPersonalActivo(data, columns) {

            if (dtTablaPersonalActivo != null) {
                dtTablaPersonalActivo.clear().destroy();
                $('#tablaPersonalActivo').empty();
            }

            dtTablaPersonalActivo = tablaPersonalActivo.DataTable({
                destroy: true,
                language: dtDicEsp,
                data,
                paging: false,
                ordering: false,
                searching: true,
                order: [[3, "asc"], [1, "asc"]],
                // fixedHeader: true,
                columns,
                scrollX: true,
                scrollY: '60vh',
                scrollCollapse: true,
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });

            // Al agregar columnas estáticas, agregarlas de esta forma para evitar el error: "fixedColumns already initialised on this table"
            new $.fn.dataTable.FixedColumns(dtTablaPersonalActivo, {
                leftColumns: 4
            });

            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        }

        function initTablaEstadisticas() {

            dtTablaEstadisticas = tablaEstadisticas.DataTable({
                destroy: true,
                language: dtDicEsp,
                paging: false,
                // ordering: false,
                // scrollX: true,
                scrollY: '60vh',
                // scrollCollapse: true,
                columns: [
                    { data: 'claveCurso', title: 'Clave' },
                    { data: 'cursoDesc', title: 'Capacitación' },
                    { data: 'clasificacion', title: 'Clasificación' },
                    { data: 'totalVigentes', title: 'Capacitaciones vigentes' },
                    { data: 'totalFaltante', title: 'Capacitaciones faltantes' },
                    { data: 'personalAplica', title: 'Personal al que aplica' },
                    { data: 'porcentaje', title: 'Porcentaje' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
            });

        }

        function initTablaIndicadoresGlobales() {
            dtTablaIndicadoresGlobales = tablaIndicadoresGlobales.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                columns: [
                    { data: 'areaOperativa', title: 'Áreas Operativas' },
                    { data: 'porcentajeCapacitacion', title: '% cumplimiento capacitación' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
            });
        }

        function cargarAreasCC() {
            const ccsCplan = getValoresMultiples('#comboCplan');
            const ccsArr = getValoresMultiples('#comboArr');

            if (ccsCplan.length == 0 && ccsArr.length == 0) {
                comboArea.empty();
                convertToMultiselect('#comboArea');
                return;
            }

            $.post('/Administrativo/CapacitacionCapitalHumano/ObtenerAreasPorCC', { ccsCplan, ccsArr })
                .then(response => {
                    comboArea.empty();
                    if (response.success) {
                        // Operación exitosa.
                        const todosOption = `<option value="Todos">Todos</option>`;
                        comboArea.append(todosOption);

                        response.items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}">${y.Text}</option>`;
                            });
                            comboArea.append(groupOption);
                        });

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }

                    convertToMultiselect('#comboArea');
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function setCharts(response) {
            ///////////////////// Por clasificación /////////////////////

            //// Protocolos de Fatalidad
            // initChartPorcentajeProtocolo('Porcentaje de cumplimiento - Protocolos de Fatalidad', chartPorcentajeProtocolo,
            //     'chartPorcentajeProtocolo', response.porcentajeProtocolo.labels, response.porcentajeProtocolo.datasets, false);
            // initChartQuantityCantidadProtocolo('Cantidad de capacitaciones - Protocolos de Fatalidad', chartCantidadProtocolo,
            //     'chartCantidadProtocolo', response.cantidadProtocolo.labels, response.cantidadProtocolo.datasets, true);

            //// Normativo
            // initChartPorcentajeNormativo('Porcentaje de cumplimiento - Normativo', chartPorcentajeNormativo,
            //     'chartPorcentajeNormativo', response.porcentajeNormativo.labels, response.porcentajeNormativo.datasets, false);
            // initChartQuantityCantidadNormativo('Cantidad de capacitaciones - Normativo', chartCantidadNormativo,
            //     'chartCantidadNormativo', response.cantidadNormativo.labels, response.cantidadNormativo.datasets, true);

            //// General
            // initChartPorcentajeGeneral('Porcentaje de cumplimiento - General', chartPorcentajeGeneral,
            //     'chartPorcentajeGeneral', response.porcentajeGeneral.labels, response.porcentajeGeneral.datasets, false);
            // initChartQuantityCantidadGeneral('Cantidad de capacitaciones - General', chartCantidadGeneral,
            //     'chartCantidadGeneral', response.cantidadGeneral.labels, response.cantidadGeneral.datasets, true);

            //// Formativo
            // initChartPorcentajeFormativo('Porcentaje de cumplimiento - Formativo', chartPorcentajeFormativo,
            //     'chartPorcentajeFormativo', response.porcentajeFormativo.labels, response.porcentajeFormativo.datasets, false);
            // initChartQuantityCantidadFormativo('Cantidad de capacitaciones - Formativo', chartCantidadFormativo,
            //     'chartCantidadFormativo', response.cantidadFormativo.labels, response.cantidadFormativo.datasets, true);

            // //// Instructivo Operativo
            // initChartPorcentajeInstructivo('Porcentaje de cumplimiento - Instructivo Operativo', chartPorcentajeInstructivo,
            //     'chartPorcentajeInstructivo', response.porcentajeInstructivo.labels, response.porcentajeInstructivo.datasets, false);
            // initChartQuantityCantidadInstructivo('Cantidad de capacitaciones - Instructivo Operativo', chartCantidadInstructivo,
            //     'chartCantidadInstructivo', response.cantidadInstructivo.labels, response.cantidadInstructivo.datasets, true);

            //// Técnico Operativo
            // initChartPorcentajeTecnico('Porcentaje de cumplimiento - Técnico Operativo', chartPorcentajeTecnico,
            //     'chartPorcentajeTecnico', response.porcentajeTecnico.labels, response.porcentajeTecnico.datasets, false);
            // initChartQuantityCantidadTecnico('Cantidad de capacitaciones - Técnico Operativo', chartCantidadTecnico,
            //     'chartCantidadTecnico', response.cantidadTecnico.labels, response.cantidadTecnico.datasets, true);

            //#region Gráficas de Capacitación Operativa
            initChartPorcentajeMandoAdministrativoProtocoloFatalidad(
                'Porcentaje de cumplimiento - Mando Administrativo - Protocolos de Fatalidad',
                chartPorcentajeMandoAdministrativoProtocoloFatalidad,
                'chartPorcentajeMandoAdministrativoProtocoloFatalidad',
                response.porcentajeMandoAdministrativoProtocoloFatalidad.labels,
                response.porcentajeMandoAdministrativoProtocoloFatalidad.datasets,
                false
            );
            initChartQuantityCantidadMandoAdministrativoProtocoloFatalidad(
                'Cantidad de capacitaciones - Mando Administrativo - Protocolos de Fatalidad',
                chartCantidadMandoAdministrativoProtocoloFatalidad,
                'chartCantidadMandoAdministrativoProtocoloFatalidad',
                response.chartCantidadMandoAdministrativoProtocoloFatalidad.labels,
                response.chartCantidadMandoAdministrativoProtocoloFatalidad.datasets,
                true
            );

            initChartPorcentajeMandoMedioProtocoloFatalidad(
                'Porcentaje de cumplimiento - Mando Medio - Protocolos de Fatalidad',
                chartPorcentajeMandoMedioProtocoloFatalidad,
                'chartPorcentajeMandoMedioProtocoloFatalidad',
                response.porcentajeMandoMedioProtocoloFatalidad.labels,
                response.porcentajeMandoMedioProtocoloFatalidad.datasets,
                false
            );
            initChartQuantityCantidadMandoMedioProtocoloFatalidad(
                'Cantidad de capacitaciones - Mando Medio - Protocolos de Fatalidad',
                chartCantidadMandoMedioProtocoloFatalidad,
                'chartCantidadMandoMedioProtocoloFatalidad',
                response.chartCantidadMandoMedioProtocoloFatalidad.labels,
                response.chartCantidadMandoMedioProtocoloFatalidad.datasets,
                true
            );

            initChartPorcentajeMandoOperativoProtocoloFatalidad(
                'Porcentaje de cumplimiento - Mando Operativo - Protocolos de Fatalidad',
                chartPorcentajeMandoOperativoProtocoloFatalidad,
                'chartPorcentajeMandoOperativoProtocoloFatalidad',
                response.porcentajeMandoOperativoProtocoloFatalidad.labels,
                response.porcentajeMandoOperativoProtocoloFatalidad.datasets,
                false
            );
            initChartQuantityCantidadMandoOperativoProtocoloFatalidad(
                'Cantidad de capacitaciones - Mando Operativo - Protocolos de Fatalidad',
                chartCantidadMandoOperativoProtocoloFatalidad,
                'chartCantidadMandoOperativoProtocoloFatalidad',
                response.chartCantidadMandoOperativoProtocoloFatalidad.labels,
                response.chartCantidadMandoOperativoProtocoloFatalidad.datasets,
                true
            );

            //--

            initChartPorcentajeMandoAdministrativoNormativo(
                'Porcentaje de cumplimiento - Mando Administrativo - Normativo',
                chartPorcentajeMandoAdministrativoNormativo,
                'chartPorcentajeMandoAdministrativoNormativo',
                response.porcentajeMandoAdministrativoNormativo.labels,
                response.porcentajeMandoAdministrativoNormativo.datasets,
                false
            );
            initChartQuantityCantidadMandoAdministrativoNormativo(
                'Cantidad de capacitaciones - Mando Administrativo - Normativo',
                chartCantidadMandoAdministrativoNormativo,
                'chartCantidadMandoAdministrativoNormativo',
                response.chartCantidadMandoAdministrativoNormativo.labels,
                response.chartCantidadMandoAdministrativoNormativo.datasets,
                true
            );

            initChartPorcentajeMandoMedioNormativo(
                'Porcentaje de cumplimiento - Mando Medio - Normativo',
                chartPorcentajeMandoMedioNormativo,
                'chartPorcentajeMandoMedioNormativo',
                response.porcentajeMandoMedioNormativo.labels,
                response.porcentajeMandoMedioNormativo.datasets,
                false
            );
            initChartQuantityCantidadMandoMedioNormativo(
                'Cantidad de capacitaciones - Mando Medio - Normativo',
                chartCantidadMandoMedioNormativo,
                'chartCantidadMandoMedioNormativo',
                response.chartCantidadMandoMedioNormativo.labels,
                response.chartCantidadMandoMedioNormativo.datasets,
                true
            );

            initChartPorcentajeMandoOperativoNormativo(
                'Porcentaje de cumplimiento - Mando Operativo - Normativo',
                chartPorcentajeMandoOperativoNormativo,
                'chartPorcentajeMandoOperativoNormativo',
                response.porcentajeMandoOperativoNormativo.labels,
                response.porcentajeMandoOperativoNormativo.datasets,
                false
            );
            initChartQuantityCantidadMandoOperativoNormativo(
                'Cantidad de capacitaciones - Mando Operativo - Normativo',
                chartCantidadMandoOperativoNormativo,
                'chartCantidadMandoOperativoNormativo',
                response.chartCantidadMandoOperativoNormativo.labels,
                response.chartCantidadMandoOperativoNormativo.datasets,
                true
            );

            //--

            initChartPorcentajeMandoAdministrativoTecnicoOperativo(
                'Porcentaje de cumplimiento - Mando Administrativo - Técnico Operativo',
                chartPorcentajeMandoAdministrativoTecnicoOperativo,
                'chartPorcentajeMandoAdministrativoTecnicoOperativo',
                response.porcentajeMandoAdministrativoTecnicoOperativo.labels,
                response.porcentajeMandoAdministrativoTecnicoOperativo.datasets,
                false
            );
            initChartQuantityCantidadMandoAdministrativoTecnicoOperativo(
                'Cantidad de capacitaciones - Mando Administrativo - Técnico Operativo',
                chartCantidadMandoAdministrativoTecnicoOperativo,
                'chartCantidadMandoAdministrativoTecnicoOperativo',
                response.chartCantidadMandoAdministrativoTecnicoOperativo.labels,
                response.chartCantidadMandoAdministrativoTecnicoOperativo.datasets,
                true
            );

            initChartPorcentajeMandoMedioTecnicoOperativo(
                'Porcentaje de cumplimiento - Mando Medio - Técnico Operativo',
                chartPorcentajeMandoMedioTecnicoOperativo,
                'chartPorcentajeMandoMedioTecnicoOperativo',
                response.porcentajeMandoMedioTecnicoOperativo.labels,
                response.porcentajeMandoMedioTecnicoOperativo.datasets,
                false
            );
            initChartQuantityCantidadMandoMedioTecnicoOperativo(
                'Cantidad de capacitaciones - Mando Medio - Técnico Operativo',
                chartCantidadMandoMedioTecnicoOperativo,
                'chartCantidadMandoMedioTecnicoOperativo',
                response.chartCantidadMandoMedioTecnicoOperativo.labels,
                response.chartCantidadMandoMedioTecnicoOperativo.datasets,
                true
            );

            initChartPorcentajeMandoOperativoTecnicoOperativo(
                'Porcentaje de cumplimiento - Mando Operativo - Técnico Operativo',
                chartPorcentajeMandoOperativoTecnicoOperativo,
                'chartPorcentajeMandoOperativoTecnicoOperativo',
                response.porcentajeMandoOperativoTecnicoOperativo.labels,
                response.porcentajeMandoOperativoTecnicoOperativo.datasets,
                false
            );
            initChartQuantityCantidadMandoOperativoTecnicoOperativo(
                'Cantidad de capacitaciones - Mando Operativo - Técnico Operativo',
                chartCantidadMandoOperativoTecnicoOperativo,
                'chartCantidadMandoOperativoTecnicoOperativo',
                response.chartCantidadMandoOperativoTecnicoOperativo.labels,
                response.chartCantidadMandoOperativoTecnicoOperativo.datasets,
                true
            );

            //--

            initChartPorcentajeMandoAdministrativoInstructivoOperativo(
                'Porcentaje de cumplimiento - Mando Administrativo - Instructivo Operativo',
                chartPorcentajeMandoAdministrativoInstructivoOperativo,
                'chartPorcentajeMandoAdministrativoInstructivoOperativo',
                response.porcentajeMandoAdministrativoInstructivoOperativo.labels,
                response.porcentajeMandoAdministrativoInstructivoOperativo.datasets,
                false
            );
            initChartQuantityCantidadMandoAdministrativoInstructivoOperativo(
                'Cantidad de capacitaciones - Mando Administrativo - Instructivo Operativo',
                chartCantidadMandoAdministrativoInstructivoOperativo,
                'chartCantidadMandoAdministrativoInstructivoOperativo',
                response.chartCantidadMandoAdministrativoInstructivoOperativo.labels,
                response.chartCantidadMandoAdministrativoInstructivoOperativo.datasets,
                true
            );

            initChartPorcentajeMandoMedioInstructivoOperativo(
                'Porcentaje de cumplimiento - Mando Medio - Instructivo Operativo',
                chartPorcentajeMandoMedioInstructivoOperativo,
                'chartPorcentajeMandoMedioInstructivoOperativo',
                response.porcentajeMandoMedioInstructivoOperativo.labels,
                response.porcentajeMandoMedioInstructivoOperativo.datasets,
                false
            );
            initChartQuantityCantidadMandoMedioInstructivoOperativo(
                'Cantidad de capacitaciones - Mando Medio - Instructivo Operativo',
                chartCantidadMandoMedioInstructivoOperativo,
                'chartCantidadMandoMedioInstructivoOperativo',
                response.chartCantidadMandoMedioInstructivoOperativo.labels,
                response.chartCantidadMandoMedioInstructivoOperativo.datasets,
                true
            );

            initChartPorcentajeMandoOperativoInstructivoOperativo(
                'Porcentaje de cumplimiento - Mando Operativo - Instructivo Operativo',
                chartPorcentajeMandoOperativoInstructivoOperativo,
                'chartPorcentajeMandoOperativoInstructivoOperativo',
                response.porcentajeMandoOperativoInstructivoOperativo.labels,
                response.porcentajeMandoOperativoInstructivoOperativo.datasets,
                false
            );
            initChartQuantityCantidadMandoOperativoInstructivoOperativo(
                'Cantidad de capacitaciones - Mando Operativo - Instructivo Operativo',
                chartCantidadMandoOperativoInstructivoOperativo,
                'chartCantidadMandoOperativoInstructivoOperativo',
                response.chartCantidadMandoOperativoInstructivoOperativo.labels,
                response.chartCantidadMandoOperativoInstructivoOperativo.datasets,
                true
            );
            //#endregion
        }

        function initChartPorcentajeTotalClasificacion(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeTotalClasificacion) {
                chartPorcentajeTotalClasificacion.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeTotalClasificacion = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }

        function initChartPorcentajeProtocolo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeProtocolo) {
                chartPorcentajeProtocolo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeProtocolo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }

        function initChartPorcentajeNormativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeNormativo) {
                chartPorcentajeNormativo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeNormativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }

        function initChartPorcentajeGeneral(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeGeneral) {
                chartPorcentajeGeneral.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            chartPorcentajeGeneral = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }

        function initChartPorcentajeFormativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeFormativo) {
                chartPorcentajeFormativo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            chartPorcentajeFormativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }

        function initChartPorcentajeInstructivo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeInstructivo) {
                chartPorcentajeInstructivo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeInstructivo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }

        function initChartPorcentajeTecnico(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeTecnico) {
                chartPorcentajeTecnico.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeTecnico = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }

        function initChartQuantityCantidadProtocolo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadProtocolo) {
                chartCantidadProtocolo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadProtocolo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartQuantityCantidadNormativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadNormativo) {
                chartCantidadNormativo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadNormativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartQuantityCantidadGeneral(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadGeneral) {
                chartCantidadGeneral.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadGeneral = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartQuantityCantidadFormativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadFormativo) {
                chartCantidadFormativo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadFormativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartQuantityCantidadInstructivo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadInstructivo) {
                chartCantidadInstructivo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadInstructivo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartQuantityCantidadTecnico(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadTecnico) {
                chartCantidadTecnico.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadTecnico = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        //#region Init. de Gráficas de Capacitación Operativa
        function initChartPorcentajeMandoAdministrativoProtocoloFatalidad(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeMandoAdministrativoProtocoloFatalidad) {
                chartPorcentajeMandoAdministrativoProtocoloFatalidad.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeMandoAdministrativoProtocoloFatalidad = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function initChartQuantityCantidadMandoAdministrativoProtocoloFatalidad(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadMandoAdministrativoProtocoloFatalidad) {
                chartCantidadMandoAdministrativoProtocoloFatalidad.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadMandoAdministrativoProtocoloFatalidad = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartPorcentajeMandoMedioProtocoloFatalidad(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeMandoMedioProtocoloFatalidad) {
                chartPorcentajeMandoMedioProtocoloFatalidad.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeMandoMedioProtocoloFatalidad = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function initChartQuantityCantidadMandoMedioProtocoloFatalidad(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadMandoMedioProtocoloFatalidad) {
                chartCantidadMandoMedioProtocoloFatalidad.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadMandoMedioProtocoloFatalidad = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartPorcentajeMandoOperativoProtocoloFatalidad(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeMandoOperativoProtocoloFatalidad) {
                chartPorcentajeMandoOperativoProtocoloFatalidad.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeMandoOperativoProtocoloFatalidad = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function initChartQuantityCantidadMandoOperativoProtocoloFatalidad(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadMandoOperativoProtocoloFatalidad) {
                chartCantidadMandoOperativoProtocoloFatalidad.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadMandoOperativoProtocoloFatalidad = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartPorcentajeMandoAdministrativoNormativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeMandoAdministrativoNormativo) {
                chartPorcentajeMandoAdministrativoNormativo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeMandoAdministrativoNormativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function initChartQuantityCantidadMandoAdministrativoNormativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadMandoAdministrativoNormativo) {
                chartCantidadMandoAdministrativoNormativo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadMandoAdministrativoNormativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartPorcentajeMandoMedioNormativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeMandoMedioNormativo) {
                chartPorcentajeMandoMedioNormativo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeMandoMedioNormativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function initChartQuantityCantidadMandoMedioNormativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadMandoMedioNormativo) {
                chartCantidadMandoMedioNormativo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadMandoMedioNormativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartPorcentajeMandoOperativoNormativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeMandoOperativoNormativo) {
                chartPorcentajeMandoOperativoNormativo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeMandoOperativoNormativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function initChartQuantityCantidadMandoOperativoNormativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadMandoOperativoNormativo) {
                chartCantidadMandoOperativoNormativo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadMandoOperativoNormativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartPorcentajeMandoAdministrativoTecnicoOperativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeMandoAdministrativoTecnicoOperativo) {
                chartPorcentajeMandoAdministrativoTecnicoOperativo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeMandoAdministrativoTecnicoOperativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function initChartQuantityCantidadMandoAdministrativoTecnicoOperativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadMandoAdministrativoTecnicoOperativo) {
                chartCantidadMandoAdministrativoTecnicoOperativo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadMandoAdministrativoTecnicoOperativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartPorcentajeMandoMedioTecnicoOperativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeMandoMedioTecnicoOperativo) {
                chartPorcentajeMandoMedioTecnicoOperativo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeMandoMedioTecnicoOperativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function initChartQuantityCantidadMandoMedioTecnicoOperativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadMandoMedioTecnicoOperativo) {
                chartCantidadMandoMedioTecnicoOperativo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadMandoMedioTecnicoOperativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartPorcentajeMandoOperativoTecnicoOperativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeMandoOperativoTecnicoOperativo) {
                chartPorcentajeMandoOperativoTecnicoOperativo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeMandoOperativoTecnicoOperativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function initChartQuantityCantidadMandoOperativoTecnicoOperativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadMandoOperativoTecnicoOperativo) {
                chartCantidadMandoOperativoTecnicoOperativo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadMandoOperativoTecnicoOperativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartPorcentajeMandoAdministrativoInstructivoOperativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeMandoAdministrativoInstructivoOperativo) {
                chartPorcentajeMandoAdministrativoInstructivoOperativo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeMandoAdministrativoInstructivoOperativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function initChartQuantityCantidadMandoAdministrativoInstructivoOperativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadMandoAdministrativoInstructivoOperativo) {
                chartCantidadMandoAdministrativoInstructivoOperativo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadMandoAdministrativoInstructivoOperativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartPorcentajeMandoMedioInstructivoOperativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeMandoMedioInstructivoOperativo) {
                chartPorcentajeMandoMedioInstructivoOperativo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeMandoMedioInstructivoOperativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function initChartQuantityCantidadMandoMedioInstructivoOperativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadMandoMedioInstructivoOperativo) {
                chartCantidadMandoMedioInstructivoOperativo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadMandoMedioInstructivoOperativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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

        function initChartPorcentajeMandoOperativoInstructivoOperativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartPorcentajeMandoOperativoInstructivoOperativo) {
                chartPorcentajeMandoOperativoInstructivoOperativo.destroy();
            }
            const ctx = document.getElementById(charID).getContext('2d');

            datasets[0].borderColor = 'red';

            chartPorcentajeMandoOperativoInstructivoOperativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
                                max: 100,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }
        function initChartQuantityCantidadMandoOperativoInstructivoOperativo(title, chart, charID, labels, datasets, displayLegend) {
            if (chartCantidadMandoOperativoInstructivoOperativo) {
                chartCantidadMandoOperativoInstructivoOperativo.destroy();
            }

            const ctx = document.getElementById(charID).getContext('2d');

            chartCantidadMandoOperativoInstructivoOperativo = new Chart(ctx, {
                type: 'bar',
                data: { labels, datasets },
                options: {
                    maintainAspectRatio: false,
                    title: {
                        display: true,
                        text: title
                    },
                    legend: {
                        display: displayLegend,
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
        //#endregion

        function descargarExcelPersonalActivo() {
            location.href = `DescargarExcelPersonalActivo`;
        }

        function ocultarGraficasCapacitacionOperativa() {
            axios.get('checarModuloCapacitacionOperativa')
                .then(response => {
                    $('#divGraficasOperativas').css('display', response.data == 'True' ? 'block' : 'none');
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }

    $(() => Administrativo.RecursosHumanos.Capacitacion.MatrizNecesidades = new MatrizNecesidades())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();