(() => {
    $.namespace('Administrativo.ActoCondicionCH.Dashboard');

    Dashboard = function () {

        // Variables.

        //#region CONST TABLA INFRACCIONES CLASIFICACIÓN
        const tblInfraccionesClasificacion = $('#tblInfraccionesClasificacion')
        let dtInfraccionClasificacion;
        //#endregion

        //Filtros.
        const comboCC = $('#comboCC');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const comboSupervisor = $('#comboSupervisor');
        const comboDepartamento = $('#comboDepartamento');
        const botonBuscar = $('#botonBuscar');
        const selectAnio = $('#selectAnio');

        const gpxTotalDep = $('#gpxTotalDep');
        let gpxTotalDepl;

        // Gráficas
        let chartSucesosPorMes = null;
        let chartSucesosPorDepartamento = null;
        let chartActosClasificacion = null;
        let chartCondicionesClasificacion = null;
        let chartComportamiento = null;

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);

        (function init() {
            // Lógica de inicialización.
            llenarCombos();
            initDatepickers();
            initTblInfraccionClasificacion()
            agregarListeners();
        })();

        // Métodos.
        function llenarCombos() {
            comboCC.fillCombo('/Administrativo/ActoCondicionCH/FillCboCC', null, false, 'Seleccione');
            convertToMultiselect('#comboCC');
            comboSupervisor.fillCombo('/Administrativo/ActoCondicionCH/ObtenerSupervisores', null, false, 'Todos');
            comboDepartamento.fillCombo('/Administrativo/ActoCondicionCH/ObtenerDepartamentos', null, false, 'Todos');

            $('.select2').select2();
        }

        function initDatepickers() {
            inputFechaInicio.datepicker({ dateFormat, showAnim }).datepicker("setDate", fechaInicioAnio);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
        }

        function agregarListeners() {
            botonBuscar.click(function () {
                cargarDatosDashboard();
                obtenerGraficaTotalDep();
            });
        }

        function cargarDatosDashboard() {
            let obj = new Object()
            obj.anio = selectAnio.val();
            axios.post('GetDashboard', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtInfraccionClasificacion.clear();
                    dtInfraccionClasificacion.rows.add(response.data.lstInfraccionesClasificacion);
                    dtInfraccionClasificacion.draw();
                    initGraficaTotalActos(response.data.lstGraficaTotalActasDTO);
                    initGraficaActasPorCC(response.data.lstGraficaActasPorCC);
                    initGraficaCantClasificaciones(response.data.lstGraficaCantActasPorClasificaciones);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblInfraccionClasificacion() {
            dtInfraccionClasificacion = tblInfraccionesClasificacion.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'cc', title: 'CC' },
                    { data: 'clasificacion', title: 'Clasificación' },
                    { data: 'infraccion', title: 'Infracción' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEditar + btnEliminar;
                        },
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '10%', targets: [0] }
                ],
            });
        }

        function obtenerFiltroBusqueda() {
            const claveSupervisor = comboSupervisor.val();
            const departamentoID = comboDepartamento.val();
            // const arrGrupos = $("#comboCC").getMultiSeg();

            //#region SE ELIMINA LOS PARAMETROS ADICIONALES DEL VALUE EN CASO DE SER CONTRATISTA O AGRUPACION DE CONTRATISTAS
            let objGrupos = new Object();
            let arrGrupos = [];
            for (let i = 0; i < $("#comboCC").getMultiSeg().length; i++) {
                let str = $("#comboCC").getMultiSeg()[i].idAgrupacion;
                let idEmpresa = $("#comboCC").getMultiSeg()[i].idEmpresa;
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
                        idEmpresa: $("#comboCC").getMultiSeg()[i].idEmpresa,
                        idAgrupacion: $("#comboCC").getMultiSeg()[i].idAgrupacion
                    };
                    arrGrupos.push(objGrupos);
                }
            }
            //#endregion

            let obj = new Object();
            obj = {
                arrGrupos: arrGrupos,
                fechaInicial: inputFechaInicio.val(),
                fechaFinal: inputFechaFin.val(),
                claveSupervisor: claveSupervisor == "Todos" ? 0 : claveSupervisor,
                departamentoID: departamentoID == "Todos" ? 0 : departamentoID,
            };
            return obj;
        }

        function initGraficaTotalActos(datos) {
            console.log(datos);
            let obj = {}
            let arr = []
            datos.forEach(element => {
                obj = {}
                obj.name = element.name
                obj.y = element.y
                obj.drilldown = element.drilldown
                arr.push(obj)
            });

            Highcharts.chart("graficaTotalActas", {
                chart: {
                    type: 'column'
                },
                title: {
                    align: 'center',
                    text: 'Actas por mes'
                },
                accessibility: {
                    announceNewData: {
                        enabled: true
                    }
                },
                xAxis: {
                    type: 'category'
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true
                            // format: '{point.y:.1f}%'
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> of total<br/>'
                },
                series: [
                    {
                        name: "Actas",
                        colorByPoint: true,
                        data: arr
                    }
                ]
            });
        }

        function initGraficaActasPorCC(datos) {
            let obj = {}
            let arr = []
            datos.forEach(element => {
                obj = {}
                obj.name = element.name
                obj.y = element.y
                obj.drilldown = element.drilldown
                arr.push(obj)
            });

            Highcharts.chart("graficaActasPorCC", {
                chart: {
                    type: 'column'
                },
                title: {
                    align: 'center',
                    text: 'Cantidad actas por CC'
                },
                accessibility: {
                    announceNewData: {
                        enabled: true
                    }
                },
                xAxis: {
                    type: 'category'
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true
                            // format: '{point.y:.1f}%'
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> of total<br/>'
                },
                series: [
                    {
                        name: "Actas",
                        colorByPoint: true,
                        data: arr
                    }
                ]
            });
        }

        function initGraficaCantClasificaciones(datos) {
            let obj = {}
            let arr = []
            datos.forEach(element => {
                obj = {}
                obj.name = element.name
                obj.y = element.y
                obj.drilldown = element.drilldown
                arr.push(obj)
            });

            Highcharts.chart("graficaCantClasificaciones", {
                chart: {
                    type: 'column'
                },
                title: {
                    align: 'center',
                    text: 'Cantidad actas por clasificaciones'
                },
                accessibility: {
                    announceNewData: {
                        enabled: true
                    }
                },
                xAxis: {
                    type: 'category'
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true
                            // format: '{point.y:.1f}%'
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> of total<br/>'
                },
                series: [
                    {
                        name: "Clasificaciones",
                        colorByPoint: true,
                        data: arr
                    }
                ]
            });
        }

        function obtenerGraficaTotalDep() {
            let parametros = obtenerFiltroBusqueda();
            axios.post('obtenerGraficaTotalDep', { filtro: parametros })
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    if (response) {
                        gxpGraficaTotal('gpxTotalDep', response.data, null, null);
                        gpxCumplimientoActo(response.data.gpxActos);
                        gpxVacunacionDepartamentos(response.data.barraVacunacion);
                        gpxVacunacionTotal(response.data.gpxVacunacionTotal);
                        gpxVacunacionAgrupacion(response.data.barraVacunacionAgrupacion);
                    } else {
                        AlertaGeneral(`Alerta`, 'Ocurrió un error');
                    }
                });
        }

        function gxpGraficaTotal(grafica, datos, callback, items) {
            Highcharts.chart(grafica, {
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                accessibility: {
                    announceNewData: {
                        enabled: true
                    }
                },
                xAxis: {
                    type: 'category'
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
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y %}'
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b><br/>'
                },
                series: datos.gpxGrafica,
                credits: {
                    enabled: false
                }
            });
        }

        function gpxCumplimientoActo(datos) {
            Highcharts.chart('gpxCumplimientoActos', {
                chart: {
                    type: 'pie'
                },

                title: {
                    text: ''
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

                series: datos,

                credits: {
                    enabled: false
                }
            });
        }

        function gpxVacunacionDepartamentos(data) {
            Highcharts.chart('gpxVacunacionDepartamento', {
                chart: {
                    type: 'column'
                },

                title: {
                    text: ''
                },

                xAxis: {
                    categories: data.categorias
                },

                yAxis: {
                    allowDecimals: false,
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },

                legend: {
                    enabled: true
                },

                plotOptions: {
                    column: {
                        stacking: 'normal'
                    }
                },

                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + '</b><br/>' +
                            this.series.name + ': ' + this.y + '<br/>' +
                            'Total: ' + this.point.stackTotal;
                    }
                },

                series: data.series,

                credits: {
                    enabled: false
                }
            });
        }

        function gpxVacunacionAgrupacion(data) {
            Highcharts.chart('gpxVacunacionAgrupacion', {
                chart: {
                    type: 'column'
                },

                title: {
                    text: ''
                },

                xAxis: {
                    categories: data.categorias
                },

                yAxis: {
                    allowDecimals: false,
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    },
                    stackLabels: {
                        enabled: true,
                        style: {
                            fontWeight: 'bold',
                            color: ( // theme
                                Highcharts.defaultOptions.title.style &&
                                Highcharts.defaultOptions.title.style.color
                            ) || 'gray'
                        }
                    }
                },

                legend: {
                    enabled: true
                },

                plotOptions: {
                    column: {
                        stacking: 'normal'
                    }
                },

                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + '</b><br/>' +
                            this.series.name + ': ' + this.y + '<br/>' +
                            'Total: ' + this.point.stackTotal;
                    }
                },

                series: data.series,

                credits: {
                    enabled: false
                }
            });
        }

        function gpxVacunacionTotal(datos) {
            Highcharts.chart('gpxVacunacionTotal', {
                chart: {
                    type: 'pie'
                },

                title: {
                    text: ''
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

                series: datos,

                credits: {
                    enabled: false
                }
            });
        }
    }
    $(() => Administrativo.ActoCondicionCH.Dashboard = new Dashboard())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();