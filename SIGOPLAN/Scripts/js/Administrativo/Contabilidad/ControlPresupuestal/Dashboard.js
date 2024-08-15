(() => {
    $.namespace('ControlPresupuestal.Dashboard');
    Dashboard = function () {
        //#region Selectores
        const inputFechaInicial = $('#inputFechaInicial');
        const inputFechaFinal = $('#inputFechaFinal');
        const selectDivision = $('#selectDivision');
        const selectProyecto = $('#selectProyecto');
        const selectTipo = $('#selectTipo');
        const selectGrupo = $('#selectGrupo');
        const selectModelo = $('#selectModelo');
        const selectConcepto = $('#selectConcepto');
        const botonBuscar = $('#botonBuscar');
        const graficaEmpresaDivision = $('#graficaEmpresaDivision');
        const tablaEmpresaDivision = $('#tablaEmpresaDivision');
        const graficaProyecto = $('#graficaProyecto');
        const tablaProyecto = $('#tablaProyecto');
        const graficaModelo = $('#graficaModelo');
        const tablaModelo = $('#tablaModelo');
        const graficaConcepto = $('#graficaConcepto');
        const tablaConcepto = $('#tablaConcepto');
        const graficaDiagramaParetoPresupuesto = $('#graficaDiagramaParetoPresupuesto');
        const graficaDiagramaParetoReal = $('#graficaDiagramaParetoReal');
        //#endregion

        const botonBuscarConsultadoEmpresaDiv = $('#botonBuscarConsultadoEmpresaDiv');
        const botonBuscarActualEmpresaDiv = $('#botonBuscarActualEmpresaDiv');
        const botonBuscar12EmpresaDiv = $('#botonBuscar12EmpresaDiv');
        const botonBuscar24EmpresaDiv = $('#botonBuscar24EmpresaDiv');

        const botonBuscarConsultadoProyecto = $('#botonBuscarConsultadoProyecto');
        const botonBuscarActualProyecto = $('#botonBuscarActualProyecto');
        const botonBuscar12Proyecto = $('#botonBuscar12Proyecto');
        const botonBuscar24Proyecto = $('#botonBuscar24Proyecto');

        const botonBuscarConsultadoGrupo = $('#botonBuscarConsultadoGrupo');
        const botonBuscarActualGrupo = $('#botonBuscarActualGrupo');
        const botonBuscar12Grupo = $('#botonBuscar12Grupo');
        const botonBuscar24Grupo = $('#botonBuscar24Grupo');

        const botonBuscarConsultadoConcepto = $('#botonBuscarConsultadoConcepto');
        const botonBuscarActualConcepto = $('#botonBuscarActualConcepto');
        const botonBuscar12Concepto = $('#botonBuscar12Concepto');
        const botonBuscar24Concepto = $('#botonBuscar24Concepto');

        const botonBuscarConsultadoPareto = $('#botonBuscarConsultadoPareto');
        const botonBuscarActualPareto = $('#botonBuscarActualPareto');
        const botonBuscar12Pareto = $('#botonBuscar12Pareto');
        const botonBuscar24Pareto = $('#botonBuscar24Pareto');

        const contenidopresaDiv = $('#contenidopresaDiv');
        const contenidoProyecto = $('#contenidoProyecto');
        const contenido12Modelo = $('#contenido12Modelo');
        const contenidoConcepto = $('#contenidoConcepto');


        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        //#endregion

        let dtEmpresaDivision;
        let dtProyecto;
        let dtModelo;
        let dtConcepto;

        (function init() {
            initCombos();
            
            inputFechaInicial.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", new Date(fechaActual.getFullYear(), fechaActual.getMonth(), 1));
            inputFechaFinal.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);

            botonBuscar.click(function () {
                cargarDashboard('consultado', 0);
            });

            botonBuscarConsultadoEmpresaDiv.click(function () {
                cargarDashboard('Consultado', 1);
            });
            botonBuscarActualEmpresaDiv.click(function () {
                cargarDashboard('Actual', 1);
            });
            botonBuscar12EmpresaDiv.click(function () {
                cargarDashboard(12, 1);
            });
            botonBuscar24EmpresaDiv.click(function () {
                cargarDashboard(24, 1);
            });


            botonBuscarConsultadoProyecto.click(function () {
                cargarDashboard('Consultado', 2);
            });
            botonBuscarActualProyecto.click(function () {
                cargarDashboard('Actual', 2);
            });
            botonBuscar12Proyecto.click(function () {
                cargarDashboard(12, 2);
            });
            botonBuscar24Proyecto.click(function () {
                cargarDashboard(24, 2);
            });


            botonBuscarConsultadoGrupo.click(function () {
                cargarDashboard('Consultado', 3);
            });
            botonBuscarActualGrupo.click(function () {
                cargarDashboard('Actual', 3);
            });
            botonBuscar12Grupo.click(function () {
                cargarDashboard(12, 3);
            });
            botonBuscar24Grupo.click(function () {
                cargarDashboard(24, 3);
            });


            botonBuscarConsultadoConcepto.click(function () {
                cargarDashboard('Consultado', 4);
            });
            botonBuscarActualConcepto.click(function () {
                cargarDashboard('Actual', 4);
            });
            botonBuscar12Concepto.click(function () {
                cargarDashboard(12, 4);
            });
            botonBuscar24Concepto.click(function () {
                cargarDashboard(24, 4);
            });

            botonBuscarConsultadoPareto.click(function () {
                cargarDashboard('Consultado', 5);
            });
            botonBuscarActualPareto.click(function () {
                cargarDashboard('Actual', 5);
            });
            botonBuscar12Pareto.click(function () {
                cargarDashboard(12, 5);
            });
            botonBuscar24Pareto.click(function () {
                cargarDashboard(24, 5);
            });

            

        })();

        function initCombos() {
            selectDivision.fillCombo('/ControlPresupuestal/GetComboDivision', null, false, 'Todos');
            convertToMultiselect('#selectDivision');
            selectProyecto.fillCombo('/ControlPresupuestal/getCboCC', null, false, 'Todos');
            convertToMultiselect('#selectProyecto');
            selectConcepto.fillCombo('/ControlPresupuestal/GetComboConcepto', null, false, 'Todos');
            convertToMultiselect('#selectConcepto');

            selectDivision.change(function () {
                let listaDivisiones = [];

                getValoresMultiplesCustom('#selectDivision').forEach(x => {
                    listaDivisiones.push(x.value);
                });
                selectProyecto.fillCombo('/ControlPresupuestal/getCboCC', { divisionesIDs: listaDivisiones }, false, 'Todos');
                convertToMultiselect('#selectProyecto');
            });

            selectTipo.change(function () {
                selectGrupo.fillCombo('/ControlPresupuestal/obtenerGruposMaquinaria', { idTipo: selectTipo.val() }, false, null, () => {
                    selectGrupo.change();
                });
            });

            selectTipo.change();

            selectGrupo.change(function () {
                selectModelo.fillCombo('/ControlPresupuestal/FillCboModeloEquipo', { idGrupo: selectGrupo.val() }, false, null, () => {
                });
            });

            selectGrupo.change();
        }
        function getValoresMultiplesCustom(selector) {
            var _tempObj = $(selector + ' option:selected').map(function (a, item) {
                return { value: item.value, text: $(item).text() };
            });
            var _tempArrObj = new Array();
            $.each(_tempObj, function (i, e) {
                _tempArrObj.push(e);
            });
            return _tempArrObj;
        }
        function cargarDashboard(tipo, FiltroBusqueda) {
            let filtros = getFiltros(tipo,FiltroBusqueda);

            axios.post('CargarDashboard', { filtros })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        console.log(response.data)
                        if (FiltroBusqueda == 0) {
                            initGraficaEmpresaDivision(response.data.graficaEmpresaDivision);
                            initTablaEmpresaDivision(response.data.tablaEmpresaDivision);
                            initGraficaProyecto(response.data.graficaProyecto);
                            initTablaProyecto(response.data.tablaProyecto);
                            initGraficaModelo(response.data.graficaModelo);
                            initTablaModelo(response.data.tablaModelo);
                            initGraficaConcepto(response.data.graficaConcepto);
                            initTablaConcepto(response.data.tablaConcepto);
                            initGraficaDiagramaParetoPresupuesto(response.data.graficaDiagramaParetoPresupuesto);
                            //initGraficaDiagramaParetoReal(response.data.graficaDiagramaParetoReal);
                        } else if (FiltroBusqueda == 1) {
                            initGraficaEmpresaDivision(response.data.graficaEmpresaDivision);
                            initTablaEmpresaDivision(response.data.tablaEmpresaDivision);
                        } else if (FiltroBusqueda == 2) {
                            initGraficaProyecto(response.data.graficaProyecto);
                            initTablaProyecto(response.data.tablaProyecto);
                        } else if (FiltroBusqueda == 3) {
                            initGraficaModelo(response.data.graficaModelo);
                            initTablaModelo(response.data.tablaModelo);
                        } else if (FiltroBusqueda == 4) {
                            initGraficaConcepto(response.data.graficaConcepto);
                            initTablaConcepto(response.data.tablaConcepto);
                        } else if (FiltroBusqueda == 5) {
                            initGraficaDiagramaParetoPresupuesto(response.data.graficaDiagramaParetoPresupuesto);
                        }

                        $('#divDashboard').css('display', 'inline-block');
                        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        function getFirstDatebyMonth(meses)
        {
            var dt = new Date();
            dt.setMonth( dt.getMonth() - meses );
            var firstDay = new Date(dt.getFullYear(), dt.getMonth(), 1);
            return firstDay;
        }

        function getFiltros(tipo,FiltroBusqueda)
        {
            
            var fi = '';
            var ff = '';
            const d = new Date();
            const d2 = new Date();
            var fa = moment(d).format('DD/MM/YYYY');
            
            switch (tipo) {
                case 'consultado':
                    {
                        fi = inputFechaInicial.val();
                        ff = inputFechaFinal.val();
                    }
                    break;
                case 'actual':
                    {
                        fi = '01/01/'+d.getFullYear();
                        ff = fa;
                    }
                    break;
                case 12:
                    {
                        fi = moment(getFirstDatebyMonth(12)).format('DD/MM/YYYY');
                        ff = fa;
                    }
                    break;
                case 24:
                    {
                        fi = moment(getFirstDatebyMonth(24)).format('DD/MM/YYYY');
                        ff = fa;
                    }
                    break;
                default:
            }
            let filtros = {
                fechaInicial: fi,
                fechaFinal: ff,
                listaDivisiones: getValoresMultiples('#selectDivision'),
                listaProyectos: getValoresMultiples('#selectProyecto'),
                tipo: (selectTipo.val() == '' || selectTipo.val() == undefined) ? 0 : selectTipo.val(),
                grupo: (selectGrupo.val() == '' || selectGrupo.val() == undefined) ? 0 : selectGrupo.val(),
                modelo: (selectModelo.val() == '' || selectModelo.val() == undefined) ? 0 : selectModelo.val(),
                listaConceptos: getValoresMultiples('#selectConcepto'),
                TipoBusqueda: 0,
                FiltroBusqueda: FiltroBusqueda,
                TipoPareto:1
            }
            return filtros;
        }
        function initGraficaEmpresaDivision(datos) {
            Highcharts.chart('graficaEmpresaDivision', {
                chart: { type: 'line' },
                lang: highChartsDicEsp,
                title: { text: 'Resultado Presupuestal (Empresa y Divisiones)' },
                xAxis: { categories: datos.meses, crosshair: true },
                yAxis: {
                    title: { text: '' },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: 'red',
                        zIndex: 10
                    }]
                },
                legend: { layout: 'vertical', align: 'right', verticalAlign: 'middle' },
                plotOptions: { series: { label: { connectorAllowed: false } } },
                series: datos.series,
                responsive: { rules: [{ condition: { maxWidth: 500 }, chartOptions: { legend: { layout: 'horizontal', align: 'center', verticalAlign: 'bottom' } } }] },
                credits: { enabled: false },
                legend: { enabled: true }
            });
        }

        function initGraficaProyecto(datos) {
            Highcharts.chart('graficaProyecto', {
                chart: { type: 'line' },
                lang: highChartsDicEsp,
                title: { text: 'Resultado Presupuestal (Proyectos)' },
                xAxis: { categories: datos.meses, crosshair: true },
                yAxis: {
                    title: { text: '' }, plotLines: [{
                        value: 0,
                        width: 1,
                        color: 'red',
                        zIndex: 10
                    }]
                },
                legend: { layout: 'vertical', align: 'right', verticalAlign: 'middle' },
                plotOptions: { series: { label: { connectorAllowed: false } } },
                series: datos.series,
                responsive: { rules: [{ condition: { maxWidth: 500 }, chartOptions: { legend: { layout: 'horizontal', align: 'center', verticalAlign: 'bottom' } } }] },
                credits: { enabled: false },
                legend: { enabled: true }
            });
        }

        function initGraficaModelo(datos) {
            Highcharts.chart('graficaModelo', {
                chart: { type: 'line' },
                lang: highChartsDicEsp,
                title: { text: 'Resultado Presupuestal (Modelo de Maquinaria)' },
                xAxis: { categories: datos.meses, crosshair: true },
                yAxis: {
                    title: { text: '' }, plotLines: [{
                        value: 0,
                        width: 1,
                        color: 'red',
                        zIndex: 10
                    }]
                },
                legend: { layout: 'vertical', align: 'right', verticalAlign: 'middle' },
                plotOptions: { series: { label: { connectorAllowed: false } } },
                series: datos.series,
                responsive: { rules: [{ condition: { maxWidth: 500 }, chartOptions: { legend: { layout: 'horizontal', align: 'center', verticalAlign: 'bottom' } } }] },
                credits: { enabled: false },
                legend: { enabled: true }
            });
        }

        function initGraficaConcepto(datos) {
            Highcharts.chart('graficaConcepto', {
                chart: { type: 'line' },
                lang: highChartsDicEsp,
                title: { text: 'Resultado Presupuestal (Concepto)' },
                xAxis: { categories: datos.meses, crosshair: true },
                yAxis: {
                    title: { text: '' }, plotLines: [{
                        value: 0,
                        width: 1,
                        color: 'red',
                        zIndex: 10
                    }]
                },
                legend: { layout: 'vertical', align: 'right', verticalAlign: 'middle' },
                plotOptions: { series: { label: { connectorAllowed: false } } },
                series: datos.series,
                responsive: { rules: [{ condition: { maxWidth: 500 }, chartOptions: { legend: { layout: 'horizontal', align: 'center', verticalAlign: 'bottom' } } }] },
                credits: { enabled: false },
                legend: { enabled: true }
            });
        }

        function initGraficaDiagramaParetoPresupuesto(datos) {
            Highcharts.chart('graficaDiagramaParetoPresupuesto', {
                chart: { zoomType: 'xy' },
                lang: highChartsDicEsp,
                title: { text: 'Diagrama de Pareto (Presupuesto)' },
                xAxis: { categories: datos.conceptos, crosshair: true },
                yAxis: [
                    {
                        labels: {
                            style: { color: Highcharts.getOptions().colors[1] },
                            formatter: function () {
                                return this.value >= 0 ? maskNumero(this.value) : '-' + (maskNumero(this.value).replace('-', ''));
                            }
                        },
                        title: { text: 'Presupuesto', style: { color: Highcharts.getOptions().colors[1] } }
                    },
                    {
                        labels: { format: '{value}%', style: { color: Highcharts.getOptions().colors[0] } },
                        title: { text: 'Porcentaje', style: { color: Highcharts.getOptions().colors[0] } },
                        opposite: true
                    }
                ],
                tooltip: { shared: true },
                series: [
                    {
                        name: 'Presupuesto',
                        type: 'column',
                        yAxis: 0,
                        data: datos.series[0].data
                    },
                    {
                        name: 'Porcentaje',
                        type: 'spline',
                        yAxis: 1,
                        data: datos.series[1].data,
                        color: 'green',
                        tooltip: { valueSuffix: '%' }
                    },
                    {
                        name: 'Límite',
                        type: 'line',
                        yAxis: 1,
                        data: datos.series[2].data,
                        color: 'red',
                        tooltip: { valueSuffix: '%' }
                    }
                ],
                credits: { enabled: false },
                legend: { enabled: true }
            });
        }

        function initGraficaDiagramaParetoReal(datos) {
            Highcharts.chart('graficaDiagramaParetoReal', {
                chart: { zoomType: 'xy' },
                lang: highChartsDicEsp,
                title: { text: 'Diagrama de Pareto (Real)' },
                xAxis: { categories: datos.conceptos, crosshair: true },
                yAxis: [
                    {
                        labels: {
                            style: { color: Highcharts.getOptions().colors[1] },
                            formatter: function () {
                                return this.value >= 0 ? maskNumero(this.value) : '-' + (maskNumero(this.value).replace('-', ''));
                            }
                        },
                        title: { text: 'Real', style: { color: Highcharts.getOptions().colors[1] } }
                    },
                    {
                        labels: { format: '{value}%', style: { color: Highcharts.getOptions().colors[0] } },
                        title: { text: 'Porcentaje', style: { color: Highcharts.getOptions().colors[0] } },
                        opposite: true
                    }
                ],
                tooltip: { shared: true },
                series: [
                    {
                        name: 'Real',
                        type: 'column',
                        yAxis: 0,
                        data: datos.series[0].data
                    },
                    {
                        name: 'Porcentaje',
                        type: 'spline',
                        yAxis: 1,
                        data: datos.series[1].data,
                        color: 'green',
                        tooltip: { valueSuffix: '%' }
                    },
                    {
                        name: 'Límite',
                        type: 'line',
                        yAxis: 1,
                        data: datos.series[2].data,
                        color: 'red',
                        tooltip: { valueSuffix: '%' }
                    }
                ],
                credits: { enabled: false },
                legend: { enabled: true }
            });
        }

        function initTablaEmpresaDivision(datos) {
            for (let i = 1; i < datos.columns.length; i++) {
                datos.columns[i].render = function (data, type, row, meta) {
                    if (type === 'display') {
                        return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                    } else {
                        return data;
                    }
                };
            }

            if (dtEmpresaDivision != null) {
                dtEmpresaDivision.destroy();
                tablaEmpresaDivision.empty();
            }

            dtEmpresaDivision = tablaEmpresaDivision.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                data: datos.data,
                columns: datos.columns,
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaProyecto(datos) {
            for (let i = 1; i < datos.columns.length; i++) {
                datos.columns[i].render = function (data, type, row, meta) {
                    if (type === 'display') {
                        return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                    } else {
                        return data;
                    }
                };
            }

            if (dtProyecto != null) {
                dtProyecto.destroy();
                tablaProyecto.empty();
            }

            dtProyecto = tablaProyecto.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                data: datos.data,
                columns: datos.columns,
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaModelo(datos) {
            for (let i = 1; i < datos.columns.length; i++) {
                datos.columns[i].render = function (data, type, row, meta) {
                    if (type === 'display') {
                        return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                    } else {
                        return data;
                    }
                };
            }

            if (dtModelo != null) {
                dtModelo.destroy();
                tablaModelo.empty();
            }

            dtModelo = tablaModelo.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                data: datos.data,
                columns: datos.columns,
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaConcepto(datos) {
            for (let i = 1; i < datos.columns.length; i++) {
                datos.columns[i].render = function (data, type, row, meta) {
                    if (type === 'display') {
                        return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                    } else {
                        return data;
                    }
                };
            }

            if (dtConcepto != null) {
                dtConcepto.destroy();
                tablaConcepto.empty();
            }

            dtConcepto = tablaConcepto.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                data: datos.data,
                columns: datos.columns,
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }
    }
    $(document).ready(() => ControlPresupuestal.Dashboard = new Dashboard())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();