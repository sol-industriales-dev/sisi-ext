(() => {
    $.namespace('CH.Dashboard');

    //#region CONST FILTROS
    const cboFiltro_Anio = $("#cboFiltro_Anio")
    const cboFiltro_CC = $("#cboFiltro_CC")
    const btnFiltroBuscar = $('#btnFiltroBuscar')
    //#endregion

    //#region CONST GRAFICA: CANTIDAD DE DEMANDAS SEGUN SEMAFORO
    const tblCantDemandasSegunSemaforo = $('#tblCantDemandasSegunSemaforo')
    let dtCantDemandasSegunSemaforo
    //#endregion

    //#region CONST GRAFICA: COMPORTAMIENTO DE LAS DEMANDAS SEGUN EL AÑO SELECCIONADO
    const tdAnio = $('#tdAnio')
    const tdInicioMes_Ene = $("#tdInicioMes_Ene")
    const tdInicioMes_Feb = $("#tdInicioMes_Feb")
    const tdInicioMes_Mar = $("#tdInicioMes_Mar")
    const tdInicioMes_Abr = $("#tdInicioMes_Abr")
    const tdInicioMes_May = $("#tdInicioMes_May")
    const tdInicioMes_Jun = $("#tdInicioMes_Jun")
    const tdInicioMes_Jul = $("#tdInicioMes_Jul")
    const tdInicioMes_Ago = $("#tdInicioMes_Ago")
    const tdInicioMes_Sep = $("#tdInicioMes_Sep")
    const tdInicioMes_Oct = $("#tdInicioMes_Oct")
    const tdInicioMes_Nov = $("#tdInicioMes_Nov")
    const tdInicioMes_Dic = $("#tdInicioMes_Dic")
    const tdInicioMes_Total = $("#tdInicioMes_Total")
    const tdNuevas_Ene = $("#tdNuevas_Ene")
    const tdNuevas_Feb = $("#tdNuevas_Feb")
    const tdNuevas_Mar = $("#tdNuevas_Mar")
    const tdNuevas_Abr = $("#tdNuevas_Abr")
    const tdNuevas_May = $("#tdNuevas_May")
    const tdNuevas_Jun = $("#tdNuevas_Jun")
    const tdNuevas_Jul = $("#tdNuevas_Jul")
    const tdNuevas_Ago = $("#tdNuevas_Ago")
    const tdNuevas_Sep = $("#tdNuevas_Sep")
    const tdNuevas_Oct = $("#tdNuevas_Oct")
    const tdNuevas_Nov = $("#tdNuevas_Nov")
    const tdNuevas_Dic = $("#tdNuevas_Dic")
    const tdNuevas_Total = $("#tdNuevas_Total")
    const tdCierres_Ene = $("#tdCierres_Ene")
    const tdCierres_Feb = $("#tdCierres_Feb")
    const tdCierres_Mar = $("#tdCierres_Mar")
    const tdCierres_Abr = $("#tdCierres_Abr")
    const tdCierres_May = $("#tdCierres_May")
    const tdCierres_Jun = $("#tdCierres_Jun")
    const tdCierres_Jul = $("#tdCierres_Jul")
    const tdCierres_Ago = $("#tdCierres_Ago")
    const tdCierres_Sep = $("#tdCierres_Sep")
    const tdCierres_Oct = $("#tdCierres_Oct")
    const tdCierres_Nov = $("#tdCierres_Nov")
    const tdCierres_Dic = $("#tdCierres_Dic")
    const tdCierres_Total = $("#tdCierres_Total")
    const tdFinDeMes_Ene = $("#tdFinDeMes_Ene")
    const tdFinDeMes_Feb = $("#tdFinDeMes_Feb")
    const tdFinDeMes_Mar = $("#tdFinDeMes_Mar")
    const tdFinDeMes_Abr = $("#tdFinDeMes_Abr")
    const tdFinDeMes_May = $("#tdFinDeMes_May")
    const tdFinDeMes_Jun = $("#tdFinDeMes_Jun")
    const tdFinDeMes_Jul = $("#tdFinDeMes_Jul")
    const tdFinDeMes_Ago = $("#tdFinDeMes_Ago")
    const tdFinDeMes_Sep = $("#tdFinDeMes_Sep")
    const tdFinDeMes_Oct = $("#tdFinDeMes_Oct")
    const tdFinDeMes_Nov = $("#tdFinDeMes_Nov")
    const tdFinDeMes_Dic = $("#tdFinDeMes_Dic")
    const tdFinDeMes_Total = $("#tdFinDeMes_Total")
    const tdTotalBajas_Anio = $('#tdTotalBajas_Anio')
    const tdTotalBajas_Ene = $("#tdTotalBajas_Ene")
    const tdTotalBajas_Feb = $("#tdTotalBajas_Feb")
    const tdTotalBajas_Mar = $("#tdTotalBajas_Mar")
    const tdTotalBajas_Abr = $("#tdTotalBajas_Abr")
    const tdTotalBajas_May = $("#tdTotalBajas_May")
    const tdTotalBajas_Jun = $("#tdTotalBajas_Jun")
    const tdTotalBajas_Jul = $("#tdTotalBajas_Jul")
    const tdTotalBajas_Ago = $("#tdTotalBajas_Ago")
    const tdTotalBajas_Sep = $("#tdTotalBajas_Sep")
    const tdTotalBajas_Oct = $("#tdTotalBajas_Oct")
    const tdTotalBajas_Nov = $("#tdTotalBajas_Nov")
    const tdTotalBajas_Dic = $("#tdTotalBajas_Dic")
    const tdTotalBajas_Total = $("#tdTotalBajas_Total")
    const tdTotalEmpleados_Anio = $('#tdTotalEmpleados_Anio')
    const tdTotalEmpleados_Ene = $("#tdTotalEmpleados_Ene")
    const tdTotalEmpleados_Feb = $("#tdTotalEmpleados_Feb")
    const tdTotalEmpleados_Mar = $("#tdTotalEmpleados_Mar")
    const tdTotalEmpleados_Abr = $("#tdTotalEmpleados_Abr")
    const tdTotalEmpleados_May = $("#tdTotalEmpleados_May")
    const tdTotalEmpleados_Jun = $("#tdTotalEmpleados_Jun")
    const tdTotalEmpleados_Jul = $("#tdTotalEmpleados_Jul")
    const tdTotalEmpleados_Ago = $("#tdTotalEmpleados_Ago")
    const tdTotalEmpleados_Sep = $("#tdTotalEmpleados_Sep")
    const tdTotalEmpleados_Oct = $("#tdTotalEmpleados_Oct")
    const tdTotalEmpleados_Nov = $("#tdTotalEmpleados_Nov")
    const tdTotalEmpleados_Dic = $("#tdTotalEmpleados_Dic")
    const tdTotalEmpleados_Total = $("#tdTotalEmpleados_Total")
    //#endregion

    //#region CONST TABLA: CUANTILLA - FINIQUITO 100%
    const tblCuantillaFiniquito = $('#tblCuantillaFiniquito')
    let dtCuantillaFiniquito
    //#endregion

    Dashboard = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            fncFillCombos()
            initTblCantDemandasSegunSemaforo()
            initTblCuantillaFiniquito()
            //#endregion

            //#region FUNCIONES FILTROS
            btnFiltroBuscar.click(function () {
                fncGetDashboard()
            })
            //#endregion
        }

        //#region FUNCIONES DASHBOARD
        function fncGetDashboard() {
            if (cboFiltro_Anio.val() > 0) {
                let obj = {}
                obj.filtro_Anio = cboFiltro_Anio.val()
                obj.filtro_CC = cboFiltro_CC.val()
                axios.post('GetDashboard', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        // GRAFICA #1
                        initGraficaComportamientoHistorico(response.data.lstGraficaComportamientoHistorico)

                        // GRAFICA #2
                        initGraficaCantDemandasActivas(response.data.lstGraficaCantDemandasActivas)

                        //#region GRAFICA #3
                        fncSetTblComportamientoDemandas(response.data.objTblComportamientosDTO)
                        //#endregion

                        //#region GRAFICA #4
                        initGraficaCantDemandasSegunSemaforo(response.data.lstGraficaCantDemandasSegunSemaforo)
                        dtCantDemandasSegunSemaforo.clear();
                        dtCantDemandasSegunSemaforo.rows.add(response.data.lstDemandasDTO);
                        dtCantDemandasSegunSemaforo.draw();
                        //#endregion

                        //#region TABLA: CUANTILLA TOTAL - FINIQUITO 100%
                        dtCuantillaFiniquito.clear();
                        dtCuantillaFiniquito.rows.add(response.data.lstDiferencias);
                        dtCuantillaFiniquito.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else { Alert2Warning("Es necesario seleccionar un año.") }
        }

        function initGraficaComportamientoHistorico(datos) {
            let arrCategories = []
            datos.forEach(element => {
                arrCategories.push(element.name)
            });

            let objDatos = {}
            let arrDatos = []

            objDatos = {}
            objDatos.name = "Nuevas"
            let arrDemandas = []
            for (let i = 0; i < arrCategories.length; i++) {
                arrDemandas.push(datos[i].lst_y[0])
            }
            objDatos.data = arrDemandas
            arrDatos.push(objDatos)

            objDatos = {}
            objDatos.name = "Cerradas"
            arrDemandas = []
            for (let i = 0; i < arrCategories.length; i++) {
                arrDemandas.push(datos[i].lst_y[1])
            }
            objDatos.data = arrDemandas
            arrDatos.push(objDatos)

            objDatos = {}
            objDatos.name = "Al cierre de cada año"
            arrDemandas = []
            for (let i = 0; i < arrCategories.length; i++) {
                arrDemandas.push(datos[i].lst_y[2])
            }
            objDatos.data = arrDemandas
            arrDatos.push(objDatos)

            Highcharts.chart("graficaComportamientoHistorico", {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Comportamiento histórico de las demandas'
                },
                subtitle: {
                    // text: 'Source: ' +
                    //     '<a href="https://www.ssb.no/en/statbank/table/08940/" ' +
                    //     'target="_blank">SSB</a>'
                },
                xAxis: {
                    // categories: ['2010', '2011', '2012'],
                    // crosshair: true
                    categories: arrCategories
                },
                yAxis: {
                    title: {
                        useHTML: true,
                        text: 'Million tonnes CO<sub>2</sub>-equivalents'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
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
                series: arrDatos,
                // series: [{
                //     name: "Nuevas",
                //     data: [1, 1, 2]
                // }, {
                //     name: "Cerradas",
                //     data: [1, 0, 1]
                // }, {
                //     name: "Al cierre de cada año",
                //     data: [0, 1, 2]
                // }],
                credits: {
                    enabled: false
                }
            });
        }

        function initGraficaCantDemandasActivas(datos) {
            let obj = {}
            let arr = []
            datos.forEach(element => {
                obj = {}
                obj.name = element.name
                obj.y = element.y
                obj.drilldown = element.drilldown
                arr.push(obj)
            });

            Highcharts.chart("graficaCantDemandasActivas", {
                chart: {
                    type: 'column'
                },
                title: {
                    align: 'center',
                    text: 'Cantidad de demandas activas por año'
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
                            enabled: true,
                            format: '{point.y:.1f}'
                        }
                    }
                },
                tooltip: {
                    // headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    // pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> of total<br/>'
                },
                series: [
                    {
                        name: "Demandas",
                        colorByPoint: true,
                        data: arr
                    }
                ],
                credits: {
                    enabled: false
                }
            });
        }

        function initGraficaCantDemandasSegunSemaforo(datos) {
            let arrData = []
            let objSerie = {}
            let arrSerie = []
            let contador = 0
            datos.forEach(element => {
                arrData = []
                arrData.push(element)

                objSerie = {}
                objSerie.name = "Demandas"
                objSerie.data = arrData
                objSerie.color = contador == 0 ? "#4f81bd" : contador == 1 ? "#00b251" : contador == 2 ? "#ffff00" : "#ff0000"
                arrSerie.push(objSerie)
                contador++
            });

            Highcharts.chart("graficaCantDemandasSegunSemaforo", {
                chart: {
                    type: 'column'
                },
                title: {
                    align: 'center',
                    text: 'Cantidad de demandas según semaforo'
                },
                accessibility: {
                    announceNewData: {
                        enabled: true
                    }
                },
                xAxis: {
                    categories: ['Demandas', 'Verde', 'Ambar', 'Rojo'],
                    crosshair: true
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:.1f}'
                        }
                    }
                },
                tooltip: {
                    // headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    // pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> of total<br/>'
                },
                // series: arrSerie,
                series: [{
                    name: 'Demandas',
                    colorByPoint: true,
                    data: [
                        {
                            name: "Demandas",
                            y: datos[0].y,
                            drilldown: "Demandas",
                            color: "#4f81bd"
                        }, {
                            name: "Verde",
                            y: datos[1].y,
                            drilldown: "Verde",
                            color: "#16b75f"
                        }, {
                            name: "Ambar",
                            y: datos[2].y,
                            drilldown: "Ambar",
                            color: "#ffff00"
                        }, {
                            name: "Rojo",
                            y: datos[3].y,
                            drilldown: "Rojo",
                            color: "#ff0000"
                        }]
                }],
                credits: {
                    enabled: false
                }
            });
        }

        function initTblCantDemandasSegunSemaforo() {
            dtCantDemandasSegunSemaforo = tblCantDemandasSegunSemaforo.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreDemandante', title: 'Nombre' },
                    { data: 'cc', title: 'CC' },
                    { data: 'puesto', title: 'Puesto' },
                    { data: 'anioFechaDemanda', title: 'Fecha demanda' },
                    { data: 'motivoSalida', title: 'Motivo de salida' },
                    { data: 'demandado', title: 'Demanda a' },
                    { data: 'estadoActual', title: 'Estado actual' },
                    { data: 'cuantiaTotal', title: 'Cuantia total' },
                    {
                        title: 'Semaforo',
                        render: function (data, type, row) {
                            let semaforo = "";
                            switch (row.strSemaforo) {
                                case "Rojo":
                                    semaforo = `<i class="fas fa-circle" style="font-size: xx-large; color: red;"></i>`
                                    break;
                                case "Ambar":
                                    semaforo = `<i class="fas fa-circle" style="font-size: xx-large; color: yellow;"></i>`
                                    break;
                                case "Verde":
                                    semaforo = `<i class="fas fa-circle" style="font-size: xx-large; color: green;"></i>`
                                    break;
                            }
                            return semaforo;
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function initTblCuantillaFiniquito() {
            dtCuantillaFiniquito = tblCuantillaFiniquito.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombre', title: 'Demandante' },
                    { data: 'puesto', title: 'Puesto' },
                    {
                        title: 'Cuantilla final',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(row.cuantillaTotal)
                        }
                    },
                    {
                        title: 'Negociación cerrada %',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(row.finiquitoAl100)
                        }
                    },
                    {
                        title: 'Diferencia',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(row.diferencia)
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function fncSetTblComportamientoDemandas(objTblComportamientosDTO) {
            //#region SE INDICA LA INFORMACIÓN EN LA TABLA
            tdAnio.html(`<td>${objTblComportamientosDTO.tdAnio}</td>`)
            tdInicioMes_Ene.html(`<td>${objTblComportamientosDTO.tdInicioMes_Ene}</td>`)
            tdInicioMes_Feb.html(`<td>${objTblComportamientosDTO.tdInicioMes_Feb}</td>`)
            tdInicioMes_Mar.html(`<td>${objTblComportamientosDTO.tdInicioMes_Mar}</td>`)
            tdInicioMes_Abr.html(`<td>${objTblComportamientosDTO.tdInicioMes_Abr}</td>`)
            tdInicioMes_May.html(`<td>${objTblComportamientosDTO.tdInicioMes_May}</td>`)
            tdInicioMes_Jun.html(`<td>${objTblComportamientosDTO.tdInicioMes_Jun}</td>`)
            tdInicioMes_Jul.html(`<td>${objTblComportamientosDTO.tdInicioMes_Jul}</td>`)
            tdInicioMes_Ago.html(`<td>${objTblComportamientosDTO.tdInicioMes_Ago}</td>`)
            tdInicioMes_Sep.html(`<td>${objTblComportamientosDTO.tdInicioMes_Sep}</td>`)
            tdInicioMes_Oct.html(`<td>${objTblComportamientosDTO.tdInicioMes_Oct}</td>`)
            tdInicioMes_Nov.html(`<td>${objTblComportamientosDTO.tdInicioMes_Nov}</td>`)
            tdInicioMes_Dic.html(`<td>${objTblComportamientosDTO.tdInicioMes_Dic}</td>`)
            tdInicioMes_Total.html(`<td>${objTblComportamientosDTO.tdInicioMes_Total}</td>`)

            tdNuevas_Ene.html(`<td>${objTblComportamientosDTO.tdNuevas_Ene}</td>`)
            tdNuevas_Feb.html(`<td>${objTblComportamientosDTO.tdNuevas_Feb}</td>`)
            tdNuevas_Mar.html(`<td>${objTblComportamientosDTO.tdNuevas_Mar}</td>`)
            tdNuevas_Abr.html(`<td>${objTblComportamientosDTO.tdNuevas_Abr}</td>`)
            tdNuevas_May.html(`<td>${objTblComportamientosDTO.tdNuevas_May}</td>`)
            tdNuevas_Jun.html(`<td>${objTblComportamientosDTO.tdNuevas_Jun}</td>`)
            tdNuevas_Jul.html(`<td>${objTblComportamientosDTO.tdNuevas_Jul}</td>`)
            tdNuevas_Ago.html(`<td>${objTblComportamientosDTO.tdNuevas_Ago}</td>`)
            tdNuevas_Sep.html(`<td>${objTblComportamientosDTO.tdNuevas_Sep}</td>`)
            tdNuevas_Oct.html(`<td>${objTblComportamientosDTO.tdNuevas_Oct}</td>`)
            tdNuevas_Nov.html(`<td>${objTblComportamientosDTO.tdNuevas_Nov}</td>`)
            tdNuevas_Dic.html(`<td>${objTblComportamientosDTO.tdNuevas_Dic}</td>`)
            tdNuevas_Total.html(`<td>${objTblComportamientosDTO.tdNuevas_Total}</td>`)

            tdCierres_Ene.html(`<td>${objTblComportamientosDTO.tdCierres_Ene}</td>`)
            tdCierres_Feb.html(`<td>${objTblComportamientosDTO.tdCierres_Feb}</td>`)
            tdCierres_Mar.html(`<td>${objTblComportamientosDTO.tdCierres_Mar}</td>`)
            tdCierres_Abr.html(`<td>${objTblComportamientosDTO.tdCierres_Abr}</td>`)
            tdCierres_May.html(`<td>${objTblComportamientosDTO.tdCierres_May}</td>`)
            tdCierres_Jun.html(`<td>${objTblComportamientosDTO.tdCierres_Jun}</td>`)
            tdCierres_Jul.html(`<td>${objTblComportamientosDTO.tdCierres_Jul}</td>`)
            tdCierres_Ago.html(`<td>${objTblComportamientosDTO.tdCierres_Ago}</td>`)
            tdCierres_Sep.html(`<td>${objTblComportamientosDTO.tdCierres_Sep}</td>`)
            tdCierres_Oct.html(`<td>${objTblComportamientosDTO.tdCierres_Oct}</td>`)
            tdCierres_Nov.html(`<td>${objTblComportamientosDTO.tdCierres_Nov}</td>`)
            tdCierres_Dic.html(`<td>${objTblComportamientosDTO.tdCierres_Dic}</td>`)
            tdCierres_Total.html(`<td>${objTblComportamientosDTO.tdCierres_Total}</td>`)

            tdFinDeMes_Ene.html(`<td>${objTblComportamientosDTO.tdFinDeMes_Ene}</td>`)
            tdFinDeMes_Feb.html(`<td>${objTblComportamientosDTO.tdFinDeMes_Feb}</td>`)
            tdFinDeMes_Mar.html(`<td>${objTblComportamientosDTO.tdFinDeMes_Mar}</td>`)
            tdFinDeMes_Abr.html(`<td>${objTblComportamientosDTO.tdFinDeMes_Abr}</td>`)
            tdFinDeMes_May.html(`<td>${objTblComportamientosDTO.tdFinDeMes_May}</td>`)
            tdFinDeMes_Jun.html(`<td>${objTblComportamientosDTO.tdFinDeMes_Jun}</td>`)
            tdFinDeMes_Jul.html(`<td>${objTblComportamientosDTO.tdFinDeMes_Jul}</td>`)
            tdFinDeMes_Ago.html(`<td>${objTblComportamientosDTO.tdFinDeMes_Ago}</td>`)
            tdFinDeMes_Sep.html(`<td>${objTblComportamientosDTO.tdFinDeMes_Sep}</td>`)
            tdFinDeMes_Oct.html(`<td>${objTblComportamientosDTO.tdFinDeMes_Oct}</td>`)
            tdFinDeMes_Nov.html(`<td>${objTblComportamientosDTO.tdFinDeMes_Nov}</td>`)
            tdFinDeMes_Dic.html(`<td>${objTblComportamientosDTO.tdFinDeMes_Dic}</td>`)
            tdFinDeMes_Total.html(`<td>${objTblComportamientosDTO.tdFinDeMes_Total}</td>`)

            tdTotalBajas_Anio.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Anio}</td>`)
            tdTotalBajas_Ene.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Ene}</td>`)
            tdTotalBajas_Feb.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Feb}</td>`)
            tdTotalBajas_Mar.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Mar}</td>`)
            tdTotalBajas_Abr.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Abr}</td>`)
            tdTotalBajas_May.html(`<td>${objTblComportamientosDTO.tdTotalBajas_May}</td>`)
            tdTotalBajas_Jun.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Jun}</td>`)
            tdTotalBajas_Jul.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Jul}</td>`)
            tdTotalBajas_Ago.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Ago}</td>`)
            tdTotalBajas_Sep.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Sep}</td>`)
            tdTotalBajas_Oct.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Oct}</td>`)
            tdTotalBajas_Nov.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Nov}</td>`)
            tdTotalBajas_Dic.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Dic}</td>`)
            tdTotalBajas_Total.html(`<td>${objTblComportamientosDTO.tdTotalBajas_Total}</td>`)

            tdTotalEmpleados_Anio.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Anio}</td>`)
            tdTotalEmpleados_Ene.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Ene}</td>`)
            tdTotalEmpleados_Feb.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Feb}</td>`)
            tdTotalEmpleados_Mar.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Mar}</td>`)
            tdTotalEmpleados_Abr.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Abr}</td>`)
            tdTotalEmpleados_May.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_May}</td>`)
            tdTotalEmpleados_Jun.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Jun}</td>`)
            tdTotalEmpleados_Jul.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Jul}</td>`)
            tdTotalEmpleados_Ago.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Ago}</td>`)
            tdTotalEmpleados_Sep.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Sep}</td>`)
            tdTotalEmpleados_Oct.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Oct}</td>`)
            tdTotalEmpleados_Nov.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Nov}</td>`)
            tdTotalEmpleados_Dic.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Dic}</td>`)
            tdTotalEmpleados_Total.html(`<td>${objTblComportamientosDTO.tdTotalEmpleados_Total}</td>`)
            //#endregion
        }

        function fncFillCombos() {
            cboFiltro_Anio.fillCombo('/Administrativo/DemandaCH/FillCboAnios', null, false, null)
            cboFiltro_CC.fillCombo('/Administrativo/DemandaCH/FillCboCCDemandasRegistradas', null, false, null)
            $(".select2").select2()

            // cboFiltro_Anio.val(2022)
            // cboFiltro_Anio.trigger("change")
            // btnFiltroBuscar.click()
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Dashboard = new Dashboard();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();