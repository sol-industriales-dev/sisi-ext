(function () {
    $.namespace('Maquinaria.Backlogs.IndicadoresRehabilitacionTMC');

    //#region CONST FILTROS
    const cboProyecto = $('#cboProyecto')
    const cboFiltroMes = $('#cboFiltroMes')
    const cboFiltroTipo = $('#cboFiltroTipo')
    const cboFiltroMotivo = $('#cboFiltroMotivo')
    const cboFiltroAnios = $('#cboFiltroAnios')
    const btnFiltroBuscar = $('#btnFiltroBuscar')
    const btnFiltroLimpiar = $('#btnFiltroLimpiar')
    //#endregion

    //#region CONST TABLAS
    const tbl1 = $('#tbl1');
    const tbl2 = $('#tbl2');
    const tbl3 = $('#tbl3');
    const tbl4 = $('#tbl4');
    const tabMenuTbl1 = $('#tabMenuTbl1');
    let dt1;
    let dt2;
    let dt3;
    let dt4;
    //#endregion

    //#region CONST BACKLOGS
    const mdlBackLogs = $('#mdlBackLogs');
    const tblBL_CatBackLogs = $('#tblBL_CatBackLogs');
    const btnMdlCerrarBL = $('#btnMdlCerrarBL');
    const mdlDescripcionBL = $('#mdlDescripcionBL');
    const txtDescripcionBL = $('#txtDescripcionBL');
    let dtBL;
    //#endregion

    //#region botones para mostar por separado
    const btnEconomico = $('#btnEconomico');
    const btnGlobal = $('#btnGlobal');
    const btnEconomicoTabla1 = $('#btnEconomicoTabla1');
    const btnMes = $('#btnMes');
    //#endregion

    BackLogs = function () {
        (function init() {
            fncListeners();
            obtenerUrlPArams();
        })();

        function fncListeners() {
            //#region INIT
            initDataTbl1();
            initDataTbl2();
            initDataTbl3();
            initDataTbl4();
            initDataTblBL();
            //#endregion

            //#region MENU
            $("#btnInicioTMC").click(function (e) {
                document.location.href = '/BackLogs/IndexTMC?areaCuenta=' + cboProyecto.val();
            });
            $("#btnProgramaInspeccion").click(function (e) {
                document.location.href = '/BackLogs/ProgramaInspTMC?areaCuenta=' + cboProyecto.val();
            })
            $("#btnPresupuestoRehabilitacion").click(function (e) {
                document.location.href = '/BackLogs/PresupuestoRehabilitacionTMC?areaCuenta=' + cboProyecto.val();
            });
            $("#btnSeguimientoPresupuestos").click(function (e) {
                document.location.href = '/BackLogs/SeguimientoDePresupuestoTMC?areaCuenta=' + cboProyecto.val();
            });
            $("#btnInformeRehabilitacion").click(function (e) {
                document.location.href = '/BackLogs/InformeTMC?areaCuenta=' + cboProyecto.val();
            });
            $("#btnFrenteBackLogs").click(function (e) {
                document.location.href = '/BackLogs/FrenteTMC?areaCuenta=' + cboProyecto.val();
            });
            $("#btnIndicadoresRehabilitacionTMC").click(function (e) {
                document.location.href = '/BackLogs/IndicadoresRehabilitacionTMC?areaCuenta=' + cboProyecto.val();
            });
            //#endregion

            //#region FUNCIONES FILTROS
            tabMenuTbl1.trigger("click");
            btnFiltroBuscar.click(function (e) {
                if (cboFiltroMes.val() != "") {
                    fncGetIndicadoresRehabilitacionTMC();
                    btnEconomico.trigger("click");
                } else {
                    Alert2Warning("Es necesario seleccionar un mes.");
                }
            });

            btnFiltroLimpiar.click(function (e) {
                cboFiltroMes[0].selectedIndex = 0;
                cboFiltroMes.trigger("change");
                cboFiltroTipo[0].selectedIndex = 0;
                cboFiltroTipo.trigger("change");
                cboFiltroMotivo[0].selectedIndex = 0;
                cboFiltroMotivo.trigger("change");
            });
            //#endregion

            //#region FILL COMBOS
            fncFillCboProyectosObra();
            cboProyecto.select2();
            cboFiltroMes.attr("multiple", true);
            convertToMultiselectSelectAll(cboFiltroMes);

            fncFillTipoMaquinariaTMC();
            cboFiltroMotivo.select2();

            cboFiltroAnios.fillCombo("/BackLogs/FillCboAniosTMC", {}, false);
            //#endregion
        }

        //#region INDICADOR CUMPLIMIENTO DE TIEMPOS
        function initDataTbl1() {
            dt1 = tbl1.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: false,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'noEconomico', title: 'Económico' },
                    { data: 'descripcion', title: 'Desc.', visible: false },
                    { data: 'modelo', title: 'Modelo', visible: false },
                    { data: 'horometro', title: 'Horas' },
                    { data: 'ppto', title: 'Ppto.' },
                    { data: 'motivo', title: 'Motivo' },
                    {
                        data: 'estatus', title: 'Estatus',
                        render: function (data, type, row) {
                            let porcentaje = ``;
                            if (data == 100) {
                                porcentaje = "<i class='fas fa-check'></i>";
                            }
                            else if (data <= 20) {
                                porcentaje = `<div class="progress">
                                                <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 55%">${data.toFixed(2)}%</div>
                                            </div>`;
                            } else {
                                porcentaje = `<div class="progress">
                                                <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: ${data.toFixed(2)}%">${data.toFixed(2)}%</div>
                                            </div>`;
                            }
                            return porcentaje;
                        }
                    },
                    {
                        data: 'fechaPromesa', title: 'Fecha<br>promesa',
                        render: function (data, type, row) {
                            let fechaPromesa = moment(data).format("DD/MM/YYYY");
                            if (fechaPromesa == "01/01/2000" || fechaPromesa == "31/12/0000") {
                                return "-";
                            } else {
                                return fechaPromesa;
                            }
                        }
                    },
                    {
                        data: 'fechaTermino', title: 'Fecha<br>termino',
                        render: function (data, type, row) {
                            let fechaTermino = moment(data).format("DD/MM/YYYY");
                            if (fechaTermino == "01/01/2000" || fechaTermino == "31/12/0000") {
                                return "-";
                            } else {
                                return fechaTermino;
                            }
                        }
                    },
                    { data: 'diasDesface', title: 'Días<br>desface' },
                    { data: 'cantBLEjecutados', title: 'Cant. BL' },
                    {
                        data: 'porcCump', title: 'Cumplimiento %',
                        render: function (data, type, row) {
                            let porcentaje = ``;
                            if (data <= 20) {
                                porcentaje = `<div class="progress">
                                                <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 55%">${data}%</div>
                                            </div>`;
                            } else {
                                porcentaje = `<div class="progress">
                                                <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: ${data}%">${data}%</div>
                                            </div>`;
                            }
                            return porcentaje;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGraficaTbl1(categories, data, meta) {
            Highcharts.chart('graficaTbl1', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                xAxis: {
                    categories: categories == null ? "" : categories,
                    crosshair: true
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                series: [{
                    type: 'column',
                    name: "% Cumplimiento",
                    data: data == null ? "" : data
                }, {
                    type: 'spline',
                    name: "Meta",
                    data: meta
                }],
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                align: 'center',
                                verticalAlign: 'bottom',
                                layout: 'horizontal'
                            },
                            yAxis: {
                                labels: {
                                    align: 'left',
                                    x: 0,
                                    y: -5
                                },
                                title: {
                                    text: null
                                }
                            },
                            subtitle: {
                                text: null
                            },
                            credits: {
                                enabled: false
                            }
                        }
                    }]
                },
                credits: {
                    enabled: false
                }
            });
        }

        function fncGraficaporMes() {
            let arrMeses = new Array();
            cboFiltroMes.val().forEach(element => {
                arrMeses.push(element);
            });
            let obj = new Object();
            obj = {
                lstMeses: arrMeses,
                tipoMaquina: cboFiltroTipo.val(),
                areaCuenta: cboProyecto.val()
            };

            axios.post('GetIndicadoresRehabilitacionTMC', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region TABLA #1
                    dt1.clear();
                    dt1.rows.add(response.data.lstDataTbl1);
                    dt1.draw();

                    let arrCategoriesGrafica1 = [];
                    let arrDataGrafica1 = [];
                    let arrMetaGrafica1 = [];
                    let dataGrafica1 = response.data.dataGrafica1.toFixed(2);
                    let objDataGrafica1 = {};
                    objDataGrafica1 = {
                        y: parseFloat(dataGrafica1),
                        color: dataGrafica1 <= 80 ? "red" : "green"
                    }
                    arrCategoriesGrafica1.push(response.data.categorieGrafica1);
                    arrDataGrafica1.push(objDataGrafica1);
                    arrMetaGrafica1.push(80);
                    fncGraficaTbl1(arrCategoriesGrafica1, arrDataGrafica1, arrMetaGrafica1);
                    //#endregion    
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGraficaPorEconomicoTabla1() {
            let arrMeses = new Array();
            cboFiltroMes.val().forEach(element => {
                arrMeses.push(element);
            });
            let obj = new Object();
            obj = {
                lstMeses: arrMeses,
                tipoMaquina: cboFiltroTipo.val(),
                areaCuenta: cboProyecto.val()
            };
            $.post('GetIndicadoresRehabilitacionTMC', {
                objFiltro: obj
            }).then(response => {
                if (response.success) {

                    //#region TABLA #1
                    dt1.clear();
                    dt1.rows.add(response.lstDataTbl1);
                    dt1.draw();

                    let arrCategoriesGrafica1 = [];
                    let arrDataGrafica1 = [];
                    let arrMetaGrafica1 = [];
                    for (let i = 0; i < response.lstDataTbl1.length; i++) {
                        let porcCump = response.lstDataTbl1[i].porcCump.toFixed(2);
                        let objData = {};
                        objData = {
                            y: parseFloat(porcCump),
                            color: porcCump <= 90 ? "red" : "green"
                        }
                        arrCategoriesGrafica1.push(response.lstDataTbl1[i].noEconomico);
                        arrDataGrafica1.push(objData);
                        arrMetaGrafica1.push(90);
                    }
                    fncGraficaTbl1(arrCategoriesGrafica1, arrDataGrafica1, arrMetaGrafica1);
                    //#endregion                  
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }
        //#endregion

        //#region INDICADOR CUMPLIMIENTO DE PPTO
        //#endregion

        //#region INDICADOR CUMPLIMIENTO DE TIEMPOS Y BL
        //#endregion

        //#region INDICADOR CANTIDAD DE OBRAS Y VENTAS
        //#endregion

        btnEconomico.click(function () {
            fncGraficaEconomico();
        });

        btnGlobal.click(function () {
            fncGraficaGlobal();
        });

        btnEconomicoTabla1.click(function () {
            fncGraficaPorEconomicoTabla1();
        });

        btnMes.click(function () {
            fncGraficaporMes();
        });

        function fncFillCboProyectosObra() {
            let idProyecto = cboProyecto.val();
            if (idProyecto == null) {
                cboProyecto.fillCombo("/BackLogs/FillAreasCuentasTMC", {}, false);
                cboProyecto.select2({
                    width: "resolve"
                });
            }
        }

        function fncFillTipoMaquinariaTMC() {
            cboFiltroTipo.fillCombo("/BackLogs/FillTipoMaquinariaTMC", {}, false);
            cboFiltroTipo.select2();
        }



        function initDataTbl2() {
            dt2 = tbl2.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: false,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'noEconomico', title: 'Económico' },
                    { data: 'descripcion', title: 'Desc.', visible: false },
                    { data: 'modelo', title: 'Modelo', visible: false },
                    {
                        data: 'estatus', title: 'Estatus',
                        render: function (data, type, row) {
                            let porcentaje = ``;
                            if (data == 100) {
                                porcentaje = "<i class='fas fa-check'></i>";
                            } else if (data <= 20) {
                                porcentaje = `<div class="progress">
                                                <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 55%">${data.toFixed(2)}%</div>
                                            </div>`;
                            } else {
                                porcentaje = `<div class="progress">
                                                <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: ${data.toFixed(2)}%">${data.toFixed(2)}%</div>
                                            </div>`;
                            }
                            return porcentaje;
                        }
                    },
                    {
                        data: 'ppto', title: 'Ppto. Autorizado',
                        render: function (data, type, row) {
                            return "$" + data.toFixed(2);
                        }
                    },
                    {
                        data: 'pptoReal', title: 'Ppto. real',
                        render: function (data, type, row) {
                            return "$" + data.toFixed(2);
                        }
                    },
                    {
                        data: 'cumpDePpto', title: 'Cump. de ppto.',
                        render: function (data, type, row) {
                            return "$" + data.toFixed(2);
                        }
                    },
                    {
                        data: 'porcCump', title: 'Cump. %',
                        render: function (data, type, row) {
                            let cumpPorc = parseFloat(data) * 100;
                            if (cumpPorc <= 20) {
                                porcentaje = `<div class="progress">
                                                <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 55%">${cumpPorc.toFixed(2)}%</div>
                                            </div>`;
                            } else {
                                porcentaje = `<div class="progress">
                                                <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: ${cumpPorc.toFixed(2)}%">${cumpPorc.toFixed(2)}%</div>
                                            </div>`;
                            }
                            return porcentaje;
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGraficaTbl2(categories, data, meta) {
            Highcharts.chart('graficaTbl2', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                xAxis: {
                    categories: categories == null ? "" : categories,
                    crosshair: true
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                series: [{
                    type: 'column',
                    name: "% Cumplimiento",
                    data: data == null ? "" : data
                }],
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                align: 'center',
                                verticalAlign: 'bottom',
                                layout: 'horizontal'
                            },
                            yAxis: {
                                labels: {
                                    align: 'left',
                                    x: 0,
                                    y: -5
                                },
                                title: {
                                    text: null
                                }
                            },
                            subtitle: {
                                text: null
                            },
                            credits: {
                                enabled: false
                            }
                        }
                    }]
                },
                credits: {
                    enabled: false
                }
            });
        }

        function initDataTbl3() {
            dt3 = tbl3.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: false,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'frenteTrabajo', title: 'Frente' },
                    { data: 'noEconomico', title: 'Económico' },
                    { data: 'descripcion', title: 'Desc.' },
                    { data: 'modelo', title: 'Modelo' },
                    {
                        data: 'fechaAsignacion', title: 'Fecha asignación',
                        render: function (data, type, row) {
                            let fechaAsignacion = moment(data).format("DD/MM/YYYY");
                            if (fechaAsignacion == "01/01/2000") {
                                return "-";
                            } else {
                                return fechaAsignacion;
                            }
                        }
                    },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary verBL"><i class="fas fa-list-ul"></i></button>`;
                        }
                    },
                    { data: 'cantBLEjecutados', title: 'Cant. de BackLogs ejecutados' }
                ],
                initComplete: function (settings, json) {
                    tbl3.on('click', '.verBL', function () {
                        let rowData = dt3.row($(this).closest('tr')).data();
                        fncGetBackLogs(rowData.id)
                        mdlBackLogs.modal("show");
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGraficaTbl3(categories, data) {
            Highcharts.chart('graficaTbl3', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                xAxis: {
                    categories: categories == null ? "" : categories,
                    crosshair: true
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                series: [{
                    name: "Cant. BackLogs ejecutados",
                    data: data == null ? "" : data
                }],
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                align: 'center',
                                verticalAlign: 'bottom',
                                layout: 'horizontal'
                            },
                            yAxis: {
                                labels: {
                                    align: 'left',
                                    x: 0,
                                    y: -5
                                },
                                title: {
                                    text: null
                                }
                            },
                            subtitle: {
                                text: null
                            },
                            credits: {
                                enabled: false
                            }
                        }
                    }]
                },
                credits: {
                    enabled: false
                }
            });
        }

        function initDataTbl4() {
            dt4 = tbl4.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: false,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'noEconomico', title: 'Económico' },
                    { data: 'descripcion', title: 'Desc.' },
                    { data: 'modelo', title: 'Modelo' },
                    { data: 'horometro', title: 'Horas' },
                    { data: 'ppto', title: 'Ppto.' },
                    { data: 'motivo', title: 'Motivo' },
                    {
                        data: 'estatus', title: 'Estatus',
                        render: function (data, type, row) {
                            let porcentaje = ``;
                            if (data == 100) {
                                porcentaje = "<i class='fas fa-check'></i>";
                            } else if (data <= 20) {
                                porcentaje = `<div class="progress">
                                                <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 40%">${data.toFixed(2)}%</div>
                                            </div>`;
                            } else {
                                porcentaje = `<div class="progress">
                                                <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: ${data.toFixed(2)}%">${data.toFixed(2)}%</div>
                                            </div>`;
                            }
                            return porcentaje;
                        }
                    },
                    {
                        data: 'fechaPromesa', title: 'Fecha promesa',
                        render: function (data, type, row) {
                            let fechaPromesa = moment(data).format("DD/MM/YYYY");
                            if (fechaPromesa == "01/01/2000") {
                                return "-";
                            } else {
                                return fechaPromesa;
                            }
                        }
                    },
                    {
                        data: 'fechaTermino', title: 'Fecha termino',
                        render: function (data, type, row) {
                            let fechaTermino = moment(data).format("DD/MM/YYYY");
                            if (fechaTermino == "01/01/2000") {
                                return "-";
                            } else {
                                return fechaTermino;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGraficaTbl4(categories) {
            Highcharts.chart('graficaTbl4', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                xAxis: {
                    categories: ['Obra', 'Venta'],
                    crosshair: true
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                series: [{
                    name: "",
                    data: categories == null ? "" : categories
                }],
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                align: 'center',
                                verticalAlign: 'bottom',
                                layout: 'horizontal'
                            },
                            yAxis: {
                                labels: {
                                    align: 'left',
                                    x: 0,
                                    y: -5
                                },
                                title: {
                                    text: null
                                }
                            },
                            subtitle: {
                                text: null
                            },
                            credits: {
                                enabled: false
                            }
                        }
                    }]
                },
                credits: {
                    enabled: false
                }
            });
        }

        function fncGraficaEconomico() {
            let arrMeses = new Array();
            cboFiltroMes.val().forEach(element => {
                arrMeses.push(element);
            });
            let obj = new Object();
            obj = {
                lstMeses: arrMeses,
                tipoMaquina: cboFiltroTipo.val(),
                areaCuenta: cboProyecto.val()
            };
            $.post('GetIndicadoresRehabilitacionTMC', {
                objFiltro: obj
            }).then(response => {
                if (response.success) {

                    //#region TABLA #2
                    dt2.clear();
                    dt2.rows.add(response.lstDataTbl2);
                    dt2.draw();

                    let arrCategoriesGrafica2 = [];
                    let arrDataGrafica2 = [];
                    let arrMetaGrafica2 = [];
                    for (let i = 0; i < response.lstDataTbl2.length; i++) {
                        let porcCump = response.lstDataTbl2[i].porcCump.toFixed(2);
                        let objData = {};
                        objData = {
                            y: parseFloat(porcCump),
                            color: porcCump <= 90 ? "green" : "red"
                        }
                        arrCategoriesGrafica2.push(response.lstDataTbl2[i].noEconomico);
                        arrDataGrafica2.push(objData);
                        arrMetaGrafica2.push(90);
                    }
                    fncGraficaTbl2(arrCategoriesGrafica2, arrDataGrafica2, arrMetaGrafica2);
                    //#endregion                  
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        function fncGraficaGlobal() {
            let arrMeses = new Array();
            cboFiltroMes.val().forEach(element => {
                arrMeses.push(element);
            });
            let obj = new Object();
            obj = {
                lstMeses: arrMeses,
                tipoMaquina: cboFiltroTipo.val(),
                areaCuenta: cboProyecto.val()
            };
            $.post('GetIndicadoresRehabilitacionTMC', {
                objFiltro: obj
            }).then(response => {
                if (response.success) {
                    dt2.clear();
                    dt2.rows.add(response.lstDataTbl2);
                    dt2.draw();

                    let arrCategoriesGrafica2 = [];
                    let arrDataGrafica2 = [];
                    let arrMetaGrafica2 = [];

                    let porcCump = response.dataPorcGlobal.dataGrafica2.toFixed(2);
                    let objData = {};
                    objData = {
                        y: parseFloat(porcCump),
                        color: porcCump <= 90 ? "green" : "red"
                    }
                    arrMetaGrafica2.push(90);
                    arrDataGrafica2.push(objData);
                    arrCategoriesGrafica2.push(response.dataPorcGlobal.categoriesGrafica2);
                    fncGraficaTbl2(arrCategoriesGrafica2, arrDataGrafica2, arrMetaGrafica2);
                    //#endregion


                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        function fncGetIndicadoresRehabilitacionTMC() {
            let idMotivo = cboFiltroMotivo.val() == "" ? "2" : cboFiltroMotivo.val();
            let arrMeses = new Array();
            cboFiltroMes.val().forEach(element => {
                arrMeses.push(element);
            });
            let obj = new Object();
            obj = {
                lstMeses: arrMeses,
                tipoMaquina: cboFiltroTipo.val(),
                areaCuenta: cboProyecto.val(),
                idMotivo: idMotivo
            };

            $.post('GetIndicadoresRehabilitacionTMC', {
                objFiltro: obj
            }).then(response => {
                if (response.success) {
                    //#region TABLA #1
                    if (response.lstDataTbl1.length > 0) {
                        // console.log("1");
                        dt1.clear();
                        dt1.rows.add(response.lstDataTbl1);
                        dt1.draw();
                    } else {
                        // console.log("0"); 
                        dt1.clear();
                        dt1.draw();
                    }

                    let arrCategoriesGrafica1 = [];
                    let arrDataGrafica1 = [];
                    let arrMetaGrafica1 = [];
                    let dataGrafica1 = response.dataGrafica1.toFixed(2);
                    let objDataGrafica1 = {};
                    objDataGrafica1 = {
                        y: parseFloat(dataGrafica1),
                        color: dataGrafica1 <= 80 ? "red" : "green"
                    }
                    arrCategoriesGrafica1.push(response.categorieGrafica1);
                    arrDataGrafica1.push(objDataGrafica1);
                    arrMetaGrafica1.push(80);
                    fncGraficaTbl1(arrCategoriesGrafica1, arrDataGrafica1, arrMetaGrafica1);
                    //#endregion

                    //#region TABLA #2
                    dt2.clear();
                    dt2.rows.add(response.lstDataTbl2);
                    dt2.draw();

                    let arrCategoriesGrafica2 = [];
                    let arrDataGrafica2 = [];
                    let arrMetaGrafica2 = [];
                    for (let i = 0; i < response.lstDataTbl2.length; i++) {
                        let porcCump = response.lstDataTbl2[i].porcCump.toFixed(2);
                        let objData = {};
                        objData = {
                            y: parseFloat(porcCump),
                            color: porcCump <= 90 ? "green" : "red"
                        }
                        arrCategoriesGrafica2.push(response.lstDataTbl2[i].noEconomico);
                        arrDataGrafica2.push(objData);
                        arrMetaGrafica2.push(90);
                    }

                    // let porcCump = response.dataPorcGlobal.dataGrafica2.toFixed(2);
                    // let objData = {};
                    // objData = {
                    //     y: parseFloat(porcCump),
                    //     color: porcCump <= 90 ? "green" : "red"
                    // }
                    // arrMetaGrafica2.push(90);
                    // arrDataGrafica2.push(objData);
                    // arrCategoriesGrafica2.push(response.dataPorcGlobal.categoriesGrafica2);
                    fncGraficaTbl2(arrCategoriesGrafica2, arrDataGrafica2);
                    //#endregion

                    //#region TABLA #3
                    dt3.clear();
                    dt3.rows.add(response.lstDataTbl3);
                    dt3.draw();

                    let arrCategoriesGrafica3 = [];
                    let arrDataGrafica3 = [];
                    // console.log(response.lstGrafica3);
                    for (let i = 0; i < response.lstGrafica3.length; i++) {
                        arrCategoriesGrafica3.push(response.lstGrafica3[i].frenteTrabajo);
                        arrDataGrafica3.push(response.lstGrafica3[i].cantBLEjecutados);
                    }
                    fncGraficaTbl3(arrCategoriesGrafica3, arrDataGrafica3);
                    //#endregion

                    //#region TABLA #4
                    dt4.clear();
                    dt4.rows.add(response.lstDataTbl4);
                    dt4.draw();

                    fncGraficaTbl4(response.lstCantLiberados);
                    //#endregion
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        function initDataTblBL() {
            dtBL = tblBL_CatBackLogs.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'folioBL', title: 'Folio BL' },
                    { data: 'noEconomico', title: 'Económico' },
                    { data: 'horas', title: 'Horas' },
                    {
                        data: 'descripcion', title: 'Descripción',
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary btn-xs verDescripcionBL" title="Descripción del BL.">Descripción BL</button>`;
                        }
                    },
                    {
                        data: 'fechaLiberado', title: 'Fecha liberado',
                        render: function (data, type, row) {
                            let fechaLiberado = moment(data).format("DD/MM/YYYY");
                            if (fechaLiberado == "01/01/2000") {
                                return "-";
                            } else {
                                return fechaLiberado;
                            }
                        }
                    },
                    { data: "horasTerminacion", title: "Horas de reparación" }
                ],
                initComplete: function (settings, json) {
                    tblBL_CatBackLogs.on("click", ".verDescripcionBL", function () {
                        const rowData = dtBL.row($(this).closest("tr")).data();
                        txtDescripcionBL.val(rowData.descripcion);
                        mdlDescripcionBL.modal("show");
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetBackLogs(id) {
            let obj = new Object();
            obj = {
                id: parseFloat(id)
            }
            axios.post("GetBackLogs", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtBL.clear();
                    dtBL.rows.add(response.data.lstBL);
                    dtBL.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#region FUNCIONES GENERALES
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
        //#endregion
    };

    $(document).ready(() => BackLogs = new BackLogs())
    { $.blockUI({ message: "procesando..." /*$('#domMessage')*/ }); }
    { $.unblockUI(); }
})();