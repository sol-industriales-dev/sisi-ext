
(function () {

    $.namespace('maquinaria.OT.rptAnalisisFrecuenciaParos');

    rptAnalisisFrecuenciaParos = function () {

        //Variables Globales 
        base64 = ""; //Identificador que funciona para obtener la cadena Base64 de la imagen de las graficas.
        var dtColores = new Array();

        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Analisis de Frecuencias de Paros',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        color = ["red", "orange", "yellow", "green", "blue", "purple", "grey"];
        colorNames = ["red", "orange", "yellow", "green", "blue", "purple", "grey"];

        //Fin de Variables Gloabales

        //Elementos Generales

        report = $("#ireport");
        /////////////////////

        //Inicio Area de Acciones
        btnExportarGrafica = $("#btnExportarGrafica"), // Este boton es el encargado de imprimir la grafica de la pantalla activa.
        btnCargar = $("#btnCargar"), // Este boton se encargade de hacer la búsqueda la información según los filtros seleccionados.
        btnVerReporte = $("#btnVerReporte"), // Este boton ejecuta la accion para visualizar el reporte de la informacion desplegada del detalle.
        btnGraficaMantenimientos = $("#btnGraficaMantenimientos"), //Se des habilito el boton por requerimiento que no era necesario las graficas.
        btnVerGraficas = $("#btnVerGraficas");  // Se encarga de abrir el modal de las graficas.

        //Fin Area Acciones

        //Inicio Modal Detalles de paro 
        modalDetalleParoEquipo = $("#modalDetalleParoEquipo"), // Este modal es el que se despliega del boton de la tabla principal.
        titleModalDetalle = $("#titleModalDetalle"), //Titulo de modal.
        tblDetalleTiposParo = $("#tblDetalleTiposParo");
        //Fin Modal Detalles de paro 

        //Inicio Modal de Graficas
        modalGraficasFrecuencias = $("#modalGraficasFrecuencias"),
        titleModalGraficas = $("#titleModalGraficas"),

        //--------Area de Canvas -------
                LineWithLine4 = $("#LineWithLine4"),
                LineWithLine10 = $("#LineWithLine10"),
                LineWithLine11 = $("#LineWithLine11"),
                LineWithLine2 = $("#LineWithLine2"),
                LineWithLine12 = $("#LineWithLine12"),
                LineWithLine13 = $("#LineWithLine13"),
                LineWithLine5 = $("#LineWithLine5"),
                LineWithLine9 = $("#LineWithLine9");
        LineWithLine14 = $("#LineWithLine14");
        LineWithLine15 = $("#LineWithLine15");


        //--------Fin area canvas ------

        //--------Tabs------------------
        tabPareto = $("#tabPareto"),
        tabCentroCostos = $("#tabCentroCostos"),
        tabTendencia = $("#tabTendencia"),

        tabVentanaTiempoOT = $("#tabVentanaTiempoOT"),
        tabVentanaTiemposCC = $("#tabVentanaTiemposCC"),
        tabVentanaTiempoTendencia = $("#tabVentanaTiempoTendencia"),
        tabVentanaFrecuenciaParo = $("#tabVentanaFrecuenciaParo"),
        tabVentanaFrecuenciaParoCC = $("#tabVentanaFrecuenciaParoCC"),
        tabVentanaFrecuenciaParoTendencia = $("#tabVentanaFrecuenciaParoTendencia"),
        tabVentanaHorasHombre = $("#tabVentanaHorasHombre"),
        tabVentanaTendencia = $("#tabVentanaTendencia"),
        tabVentanaGeneral = $("#tabVentanaGeneral");
        //------------------------------

        //-----------Tablas-----------------

        tblTiemposOTCC = $("#tblTiemposOTCC"),
        tblTiemposOTTendencia = $("#tblTiemposOTTendencia"),
        tblFrecuenciaParoPorCC = $("#tblFrecuenciaParoPorCC"),
        tblFrecuenciaParoTendencia = $("#tblFrecuenciaParoTendencia"),
        tblGeneralMantenimientos = $("#tblGeneralMantenimientos");
        //----------------------------------

        //Fin Modal de Graficas

        //Tipos de paro (esta deshabilitado)
        titleModalTiposParo = $("#titleModalTiposParo"),
        modalGraficasTiposParos = $("#modalGraficasTiposParos"),
        LineWithLine3 = $("#LineWithLine3"),
        LineWithLine6 = $("#LineWithLine6");
        LineWithLine7 = $("#LineWithLine7");
        LineWithLine8 = $("#LineWithLine8");
        tblFrecuenciasPreventivo = $("#tblFrecuenciasPreventivo");

        //
        //Filtros Busqueda
        cboCC = $("#cboCC"),
        txtFechaIncioFiltro = $("#txtFechaIncioFiltro"),
        txtFechaFinFiltro = $("#txtFechaFinFiltro"),
        txtHorometroInicial = $("#txtHorometroInicial"),
        txtHorometroFinal = $("#txtHorometroFinal"),
        cboMotivoParo = $("#cboMotivoParo"),
        cboTipoParo = $("#cboTipoParo"),
        cboCondicionParo = $("#cboCondicionParo"),
        cboGrupo = $("#cboGrupo"),
        cboModelo = $("#cboModelo"),
        cboNoEconomico = $("#cboNoEconomico"),
        cboEstatusEquipo = $("#cboEstatusEquipo"),
        tblfrecuenciaParo = $("#tblfrecuenciaParo"),
        tabVentanaPreventivo = $("#tabVentanaPreventivo"),
        tblFrecuenciasMatenimientos = $("#tblFrecuenciasMatenimientos"),
        tabVentanaCorrectivo = $("#tabVentanaCorrectivo"),
        tblFrecuenciaCorrectivo = $("#tblFrecuenciaCorrectivo"),
        tabVentanaPredictivo = $("#tabVentanaPredictivo"),
        tblFrecuenciaPredictivo = $("#tblFrecuenciaPredictivo");
        divTiposMantenimiento = $("#divTiposMantenimientoFrecuencias");
        divTiposMantenimientoTiempos = $("#divTiposMantenimientoTiempos");
        ///*---------------------------------------
        //Inicializacion de Graficas
        myChart1 = null;
        myChart2 = null;
        myChart3 = null;
        myChart4 = null;
        myChart5 = null;
        myChart6 = null;
        myChart7 = null;
        myChart8 = null;
        myChart9 = null;
        myChart10 = null;
        myChart11 = null;
        myChart12 = null;
        myChart13 = null;
        myChart14 = null;
        myChart15 = null;
        //Fin Inicializacion de Graficas

        function init() {
            //Set Values
            var now = new Date(),
            year = now.getYear() + 1900;
            txtFechaIncioFiltro.datepicker().datepicker("setDate", "01/01/" + year);
            txtFechaFinFiltro.datepicker().datepicker("setDate", new Date());
            ///

            /// Eventos Click
            btnExportarGrafica.click(done);
            btnVerReporte.click(verReporte);
            btnVerGraficas.click(verGraficas);
            btnCargar.click(LoadTablaData);
            ///

            //Carga de Modales
            cboCC.fillCombo('/CatInventario/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");
            cboGrupo.fillCombo('/CatGrupos/FillCboGrupoMaquina', { obj: 1 });
            cboModelo.clearCombo();
            cboMotivoParo.fillCombo('/OT/cboMotivosParo', null, false);
            loadEconomicos();
            ///

            /// Eventos Change
            cboGrupo.change(loadEconomicos);
            txtHorometroInicial.change(horometros);
            txtHorometroFinal.change(horometros);
            ////
        }

        function displayReporte(ptituloGrafica) {

            $.blockUI({ message: mensajes.PROCESANDO });
            var path = "/Reportes/Vista.aspx?idReporte=" + 69 + "&pFIncio=" + txtFechaIncioFiltro.val() + "&pFFin=" + txtFechaFinFiltro.val() + "&pMotivoParo=" + SetSelected("cboMotivoParo") + "&pTipoParo=" + SetSelected("cboTipoParo") + "&pCodicionParo=" + SetSelected("cboCondicionParo") + "&pGrupoMaquinaria=" + SetSelected("cboGrupo") + "&pEconomico=" + SetSelected("cboNoEconomico") + "&pCodicionEquipo=" + SetSelected("cboEstatusEquipo") + "&ptituloGrafica=" + ptituloGrafica;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function SetSelected(elemento) {

            var texto = $("#" + elemento + " option:selected").text()
            if ("--Seleccione--" == texto) {
                return "Todos";
            }
            else {
                return texto;
            }

        }

        function draw() {
            tblFrecuenciasPreventivoGrid.draw();
            tblFrecuenciaCorrectivoGrid.draw();
            tblFrecuenciaPredictivoGrid.draw();
        }

        function verReporte() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var path = "/Reportes/Vista.aspx?idReporte=" + 68;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function horometros() {

            var inicial = Number(txtHorometroInicial.val());
            var Final = Number(txtHorometroFinal.val());
            if (Final < inicial)
                txtHorometroFinal.val(inicial);
        }

        function loadEconomicos() {
            if (cboGrupo.val() != "") {
                cboModelo.fillCombo('/CatComponentes/FillCboFiltroModelo_Componente', { idGrupo: cboGrupo.val() });
                cboModelo.prop("disabled", false);
            }
            else {
                cboModelo.clearCombo();
                cboModelo.prop("disabled", true);
            }
            cboNoEconomico.fillCombo('/CatInventario/FillCboEconomicos', { ccs: getValoresMultiples("#cboCC"), grupo: cboGrupo.val() != "" ? cboGrupo.val() : 0 });
        }

        function verGraficas() {
            titleModalGraficas.text("Reporte Del periodo : " + txtFechaIncioFiltro.val() + " AL " + txtFechaFinFiltro.val() + (cboModelo.val() == "" ? (cboGrupo.val() == "" ? "" : " PARA " + $("#cboGrupo option:selected").text()) : " PARA MODELO " + $("#cboModelo option:selected").text()));
            SetGraficaFrecuenciaParos();
            SetGraficaHorasHombre();
            SetGraficaTiempoOT();
            SetGraficaMantenimientoGeneral();
            SetGraficaMantenimientoGeneralTiempos();
            SetGraficaTiemposGroup();
            SetGraficaFrecuenciasGroup();
            SetGraficaTiemposTendencia();
            SetGraficaFrecuenciaTendencia();
        }

        function SetGraficaFrecuenciasGroup() {
            if (myChart12 != null) {
                myChart12.destroy();
            }

            var dt = tblFrecuenciaParoPorCCGrid.data().sort(function (a, b) { return b.Frecuencia - a.Frecuencia });

            var dtSetInfo = new Array();
            var dtLabel = new Array();
            var dbColores = new Array();
            for (var i = 0; i < dt.length ; i++) {
                var colorName = GetColor();
                dtSetInfo.push(dt[i].Frecuencia);
                dtLabel.push(dt[i].CentroCostos);
                dbColores.push(colorName);
            }
            var ctx = document.getElementById("LineWithLine12").getContext("2d");

            var data = {
                labels: dtLabel,
                datasets: [{
                    label: "Frecuencia",
                    backgroundColor: dbColores,
                    borderColor: dbColores,
                    borderWidth: 1,
                    data: dtSetInfo,
                    fill: false,
                }]
            };

            myChart12 = new Chart(ctx, {
                type: 'bar',
                data: data,
                options:
                     {
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                         scales: {
                             xAxes: [{
                                 ticks: {
                                     autoSkip: false,

                                 }
                             }],
                             yAxes: [{
                                 ticks: {
                                     beginAtZero: true
                                 }
                             }]
                         }
                     }
            });

        }

        function SetGraficaTiemposGroup() {
            if (myChart10 != null) {
                myChart10.destroy();
            }

            var dt = tblTiemposOTCCGrid.data().sort(function (a, b) { return b.Tiempos - a.Tiempos });

            var dtSetInfo = new Array();
            var dtLabel = new Array();
            var dbColores = new Array();


            for (var i = 0; i < dt.length ; i++) {
                var colorName = GetColor();
                dtSetInfo.push(dt[i].Tiempos);
                dtLabel.push(dt[i].CentroCostos);
                dbColores.push(colorName);
            }

            var ctx = document.getElementById("LineWithLine10").getContext("2d");

            var data = {
                labels: dtLabel,
                datasets: [{
                    label: "Tiempos",
                    backgroundColor: dbColores,
                    borderColor: dbColores,
                    borderWidth: 1,
                    data: dtSetInfo,
                    fill: false,
                }]
            };

            myChart10 = new Chart(ctx, {
                type: 'bar',
                data: data,
                options:
                     {
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                         scales: {
                             xAxes: [{
                                 ticks: {
                                     autoSkip: false,

                                 }
                             }],
                             yAxes: [{
                                 ticks: {
                                     beginAtZero: true
                                 }
                             }]
                         }
                     }
            });
        }

        function SetGraficaTiemposTendencia() {
            if (myChart11 != null) {
                myChart11.destroy();
            }

            var dt = tblTiemposOTTendenciaGrid.data();
            var parts = txtFechaFinFiltro.val().split('/');
            var dtLine = new Array();

            var meses = ["ENERO", "FEB", "MARZO", "ABRIL", "MAY", "JUN", "JUL", "AGO", "SEP", "OCTUBRE", "NOV", "DIC"];
            var dtLabel = new Array();
            var dtItemArreglo = new Array();
            for (var i = 0; i < Number(parts[1]) ; i++) {
                dtLabel.push(meses[i]);
            }

            var group_to_values = dt.reduce(function (obj, item) {
                obj[item.CentroCostos] = obj[item.CentroCostos] || [];
                obj[item.CentroCostos].push(item.TiemposTendecia);
                return obj;
            }, {});

            var groups = Object.keys(group_to_values).map(function (key) {
                var colorName = GetColor();
                return {
                    label: key,
                    backgroundColor: colorName,
                    borderColor: colorName,
                    borderWidth: 1,
                    data: group_to_values[key],
                    fill: false,
                }
            });
            var ctx = document.getElementById("LineWithLine11").getContext("2d");

            var data = {
                labels: dtLabel,
                datasets: groups
            };

            myChart11 = new Chart(ctx, {
                type: 'line',
                data: data,
                options:
                     {
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                     }
            });

        }


        function SetGraficaFrecuenciaTendencia() {
            if (myChart13 != null) {
                myChart13.destroy();
            }

            var dt = tblFrecuenciaParoTendenciaGrid.data();
            var parts = txtFechaFinFiltro.val().split('/');
            var dtLine = new Array();

            var meses = ["ENERO", "FEB", "MARZO", "ABRIL", "MAY", "JUN", "JUL", "AGO", "SEP", "OCTUBRE", "NOV", "DIC"];
            var dtLabel = new Array();
            var dtItemArreglo = new Array();
            for (var i = 0; i < Number(parts[1]) ; i++) {
                dtLabel.push(meses[i]);
            }

            var group_to_values = dt.reduce(function (obj, item) {
                obj[item.CentroCostos] = obj[item.CentroCostos] || [];
                obj[item.CentroCostos].push(item.FrecuenciaTendecia);
                return obj;
            }, {});

            var groups = Object.keys(group_to_values).map(function (key) {
                var colorName = GetColor();
                return {
                    label: key,
                    backgroundColor: colorName,
                    borderColor: colorName,
                    borderWidth: 1,
                    data: group_to_values[key],
                    fill: false,
                }
            });

            var ctx = document.getElementById("LineWithLine13").getContext("2d");

            var data = {
                labels: dtLabel,
                datasets: groups
            };

            myChart13 = new Chart(ctx, {
                type: 'line',
                data: data,
                options:
                     {
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                     }
            });

        }


        function SetGraficaFrecuenciaParos() {
            if (myChart1 != null) {
                myChart1.destroy();
            }
            modalGraficasFrecuencias.modal('show');
            var dt = tblfrecuenciaParo.data().sort(function (a, b) { return b.frecuenciaParo - a.frecuenciaParo });

            var dtParos = new Array();
            var dtFrecuenciaParo = new Array();
            var dtTiempoParo = new Array();
            var dtHoraHombre = new Array();
            var dtPorcentajeFrencuencia = new Array();
            var dtPorcentajeRestante = new Array();
            var dtPorcentajeFrencuenciaAcumulado = new Array();
            var objTotalFrecuencia = 0;
            for (var i = 0; i < dt.length ; i++) {
                dtParos.push(dt[i].motivoParo);
                dtFrecuenciaParo.push(dt[i].frecuenciaParo);
                dtTiempoParo.push(dt[i].tiempoOT);
                dtHoraHombre.push(dt[i].horasHombre);
                objTotalFrecuencia += dt[i].frecuenciaParo;

            }

            var porcentaje = 0
            for (var i = 0; i < dtFrecuenciaParo.length; i++) {

                var temp = (dtFrecuenciaParo[i] / objTotalFrecuencia) * 100;
                dtPorcentajeFrencuencia.push(80);
                porcentaje += temp;

                dtPorcentajeFrencuenciaAcumulado.push(porcentaje.toFixed(2));
            }
            var ctx = document.getElementById("LineWithLine2").getContext("2d");

            var data = {
                labels: dtParos,
                datasets: [{
                    label: "Frecuencia",
                    backgroundColor: "rgba(255, 159, 64, 0.5)",
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtFrecuenciaParo,
                    fill: false,
                    yAxisID: 'y-axis-1',
                }, {
                    type: 'line',
                    label: "Porcentaje",
                    backgroundColor: "green",
                    borderColor: "green",
                    data: dtPorcentajeFrencuenciaAcumulado,
                    fill: false,
                    yAxisID: 'y-axis-2',
                },
                {
                    type: 'line',
                    label: "Limite",
                    backgroundColor: "red",
                    borderColor: "red",
                    data: dtPorcentajeFrencuencia,
                    fill: false,
                    yAxisID: 'y-axis-2',
                    tooltip: false,
                    intersect: false
                }
                ]
            };

            myChart1 = new Chart(ctx, {
                type: 'bar',
                data: data,
                beforeDraw: function (chartInstance) {
                    var ctx = chartInstance.chart.ctx;
                    ctx.fillStyle = "white";
                    ctx.fillRect(0, 0, chartInstance.chart.width, chartInstance.chart.height);
                },
                options:
                     {
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                         scales: {
                             xAxes: [{
                                 display: true,
                                 gridLines: {
                                     display: false
                                 },
                                 labels: {
                                     show: true,
                                 },
                                 ticks: {
                                     autoSkip: false,

                                 }

                             }],
                             yAxes: [{
                                 type: "linear",
                                 display: true,
                                 position: "left",
                                 id: "y-axis-1",
                                 gridLines: {
                                     display: false
                                 },
                                 labels: {
                                     show: true,

                                 },
                             }, {
                                 type: "linear",
                                 display: true,
                                 position: "right",
                                 id: "y-axis-2",
                                 gridLines: {
                                     display: false
                                 },
                                 labels: {
                                     show: true,

                                 },
                                 ticks: {
                                     min: 0,
                                     max: 100.01,
                                     callback: function (value) {
                                         return value.toFixed(0) + "%";
                                     }
                                 }
                             }]
                         },
                         animation: {
                             duration: 1,
                             onComplete: function () {
                                 var chartInstance = this.chart,
                                     ctx = chartInstance.ctx;

                                 if (ctx.canvas.id == 'LineWithLine2') {
                                     ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                                     ctx.fillStyle = "#000000";
                                     ctx.textAlign = 'center';
                                     ctx.textBaseline = 'bottom';

                                     this.data.datasets.forEach(function (dataset, i) {
                                         var meta = chartInstance.controller.getDatasetMeta(i);
                                         if (dataset.type != 'line') {
                                             meta.data.forEach(function (bar, index) {

                                                 var dato = Number(dataset.data[index]);
                                                 data = dato.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                                 ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                             });
                                         }

                                     });
                                 }
                             }
                         }
                     },
            });
        }

        function SetGraficaHorasHombre() {
            if (myChart3 != null) {
                myChart3.destroy();

            }
            var dt = tblfrecuenciaParo.data().sort(function (a, b) { return b.horasHombre - a.horasHombre });

            var dtParos = new Array();


            var dtHoraHombre = new Array();
            var dtPorcentajeHorasHombre = new Array();

            var dtPorcentajeHorasHombreAcumulado = new Array();
            var objTotalHorasHombre = 0;

            for (var i = 0; i < dt.length ; i++) {
                dtParos.push(dt[i].motivoParo);
                dtHoraHombre.push(dt[i].horasHombre);
                objTotalHorasHombre += dt[i].horasHombre;
            }

            var porcentaje = 0
            for (var i = 0; i < dtHoraHombre.length; i++) {

                var temp = (dtHoraHombre[i] / objTotalHorasHombre) * 100;

                dtPorcentajeHorasHombre.push(80);
                porcentaje += temp;

                dtPorcentajeHorasHombreAcumulado.push(porcentaje.toFixed(2));
            }
            var ctx = document.getElementById("LineWithLine3").getContext("2d");

            var data = {
                labels: dtParos,
                datasets: [{
                    label: "Horas Hombre",
                    backgroundColor: "rgba(255, 159, 64, 0.5)",
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtHoraHombre,
                    fill: false,
                    yAxisID: 'y-axis-1',
                }, {
                    type: 'line',
                    label: "Porcentaje",
                    backgroundColor: "green",
                    borderColor: "green",
                    data: dtPorcentajeHorasHombreAcumulado,
                    fill: false,
                    yAxisID: 'y-axis-2',
                },
                {
                    type: 'line',
                    label: "Limite",
                    backgroundColor: "red",
                    borderColor: "red",
                    data: dtPorcentajeHorasHombre,
                    fill: false,
                    yAxisID: 'y-axis-2',
                    tooltip: false,
                    intersect: false
                }
                ]
            };

            myChart3 = new Chart(ctx, {
                type: 'bar',
                data: data,
                options:
                     {
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                         scales: {
                             xAxes: [{
                                 display: true,
                                 gridLines: {
                                     display: false
                                 },
                                 labels: {
                                     show: true,
                                 }, ticks: {
                                     autoSkip: false,

                                 }
                             }],
                             yAxes: [{
                                 type: "linear",
                                 display: true,
                                 position: "left",
                                 id: "y-axis-1",
                                 gridLines: {
                                     display: false
                                 },
                                 labels: {
                                     show: true,
                                 },
                             }, {
                                 type: "linear",
                                 display: true,
                                 position: "right",
                                 id: "y-axis-2",
                                 gridLines: {
                                     display: false
                                 },
                                 labels: {
                                     show: true,
                                 },
                                 ticks: {
                                     min: 0,
                                     max: 100.01,
                                     callback: function (value) {
                                         return value.toFixed(0) + "%";
                                     }
                                 }
                             }]
                         },
                         animation: {
                             duration: 1,
                             onComplete: function () {
                                 var chartInstance = this.chart,
                                     ctx = chartInstance.ctx;

                                 if (ctx.canvas.id == 'LineWithLine3') {
                                     ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                                     ctx.fillStyle = "#000000";
                                     ctx.textAlign = 'center';
                                     ctx.textBaseline = 'bottom';

                                     this.data.datasets.forEach(function (dataset, i) {
                                         var meta = chartInstance.controller.getDatasetMeta(i);
                                         if (dataset.type != 'line') {
                                             meta.data.forEach(function (bar, index) {

                                                 var dato = Number(dataset.data[index]);
                                                 data = dato.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                                 ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                             });
                                         }

                                     });
                                 }
                             }
                         }

                     }
            });
        }


        var canvas = document.querySelector("#LineWithLine4");

        var X, Y, W, H, r;
        function inicializarCanvas() {
            if (canvas && canvas.getContext) {
                var ctx = canvas.getContext("2d");
                if (ctx) {
                    var s = getComputedStyle(canvas);
                    var w = s.width;
                    var h = s.height;

                    W = canvas.width = w.split("px")[0];
                    H = canvas.height = h.split("px")[0];
                }
            }
        }

        function SetGraficaTiempoOT() {
            if (myChart4 != null) {
                myChart4.destroy();

            }
            var dt = tblfrecuenciaParo.data().sort(function (a, b) { return b.tiempoOT - a.tiempoOT });

            var dtParos = new Array();

            var dtTiempoOT = new Array();
            var dtPorcentajeTiempoOT = new Array();

            var dtPorcentajeTiempoAcumulado = new Array();
            var objTotalTiemposOT = 0;

            for (var i = 0; i < dt.length ; i++) {
                dtParos.push(dt[i].motivoParo);
                dtTiempoOT.push(dt[i].tiempoOT);
                objTotalTiemposOT += dt[i].tiempoOT;
            }

            var porcentaje = 0
            for (var i = 0; i < dtTiempoOT.length; i++) {

                var temp = (dtTiempoOT[i] / objTotalTiemposOT) * 100;

                dtPorcentajeTiempoOT.push(80);
                porcentaje += temp;

                dtPorcentajeTiempoAcumulado.push(porcentaje.toFixed(2));
            }
            var ctx = document.getElementById("LineWithLine4").getContext("2d");

            var data = {
                labels: dtParos,
                datasets: [{
                    label: "Tiempo OT",
                    backgroundColor: "rgba(255, 159, 64, 0.5)",
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtTiempoOT,
                    fill: false,
                    yAxisID: 'y-axis-1',
                }, {
                    type: 'line',
                    label: "Porcentaje",
                    backgroundColor: "green",
                    borderColor: "green",
                    data: dtPorcentajeTiempoAcumulado,
                    fill: false,
                    yAxisID: 'y-axis-2',
                },
                {
                    type: 'line',
                    label: "Limite",
                    backgroundColor: "red",
                    borderColor: "red",
                    data: dtPorcentajeTiempoOT,
                    fill: false,
                    yAxisID: 'y-axis-2',
                    tooltip: false,
                    intersect: false
                }
                ]
            };

            myChart4 = new Chart(ctx, {
                type: 'bar',
                data: data,
                options:
                     {
                         scale: {
                             pointLabels: {
                                 fontSize: 5
                             }
                         },
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                         scales: {
                             xAxes: [{
                                 display: true,
                                 gridLines: {
                                     display: false
                                 },
                                 labels: {
                                     show: true,
                                 },
                                 ticks: {
                                     autoSkip: false,

                                 }
                             }],
                             yAxes: [{
                                 type: "linear",
                                 display: true,
                                 position: "left",
                                 id: "y-axis-1",
                                 gridLines: {
                                     display: false
                                 },
                                 labels: {
                                     show: true,
                                 },
                             }, {
                                 type: "linear",
                                 display: true,
                                 position: "right",
                                 id: "y-axis-2",
                                 gridLines: {
                                     display: false
                                 },
                                 labels: {
                                     show: true,
                                 },
                                 ticks: {
                                     min: 0,
                                     max: 100.01,
                                     callback: function (value) {
                                         return value.toFixed(0) + "%";
                                     }
                                 }
                             }]
                         },
                         animation: {
                             duration: 1,
                             onComplete: function () {
                                 var chartInstance = this.chart,
                                     ctx = chartInstance.ctx;

                                 if (ctx.canvas.id == 'LineWithLine4') {
                                     ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                                     ctx.fillStyle = "#000000";
                                     ctx.textAlign = 'center';
                                     ctx.textBaseline = 'bottom';

                                     this.data.datasets.forEach(function (dataset, i) {
                                         var meta = chartInstance.controller.getDatasetMeta(i);
                                         if (dataset.type != 'line') {
                                             meta.data.forEach(function (bar, index) {

                                                 var dato = Number(dataset.data[index]);
                                                 data = dato.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                                 ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                             });
                                         }

                                     });
                                 }
                             }
                         }


                     }
            });
        }

        function SetGraficaMantenimientoGeneral() {
            if (myChart5 != null) {
                myChart5.destroy();

            }
            var dt = tblGeneralMantenimientos.data();

            var lblTitulos = new Array();
            var porcentajes = new Array();
            var frecuencia = new Array();


            for (var i = 0; i < dt.length; i++) {
                lblTitulos.push(dt[i].descripcion);
                porcentajes.push(dt[i].porcentajeRelativo);
                frecuencia.push(dt[i].cantidadFrecuencias);
            }

            var config = {
                type: 'pie',
                data: {
                    datasets: [{
                        data: porcentajes,
                        backgroundColor: [
                           "#52BE80",
                            "#E74C3C",
                            "#F4D03F"
                        ],
                        label: 'Dataset 1'
                    }],
                    labels: lblTitulos
                },
                options: {
                    responsive: true
                },
                title: {
                    display: true,
                    position: "top",
                    text: "Status General de mantenimientos",
                    fontSize: 18,
                    fontColor: "#111"
                },
                legend: {
                    display: true,
                    position: 'bottom',
                    labels: {
                        fontColor: "#000080",
                    }
                },
            };

            var ctxBar = document.getElementById('LineWithLine5').getContext('2d');

            Chart.plugins.register({
                afterDatasetsDraw: function (chartInstance, easing) {
                    // To only draw at the end of animation, check for easing === 1
                    //chartInstance.chart.ctx;
                    chartInstance.data.datasets.forEach(function (dataset, i) {
                        var meta = chartInstance.getDatasetMeta(i);
                        if (!meta.hidden) {
                            if (meta.controller.chart.chart.canvas.id == "LineWithLine5") {
                                meta.data.forEach(function (element, index) {
                                    // Draw the text in black, with the specified font
                                    var ctx = ctxBar;
                                    ctx.fillStyle = 'black';
                                    var fontSize = 16;
                                    var fontStyle = 'normal';
                                    var fontFamily = 'Helvetica Neue';
                                    ctx.font = Chart.helpers.fontString(fontSize, fontStyle, fontFamily);
                                    // Just naively convert to string for now
                                    var dataString = dataset.data[index].toString();
                                    // Make sure alignment settings are correct
                                    ctx.textAlign = 'center';
                                    ctx.textBaseline = 'middle';
                                    var padding = 5;
                                    var position = element.tooltipPosition();
                                    ctx.fillText(dataString + '%', position.x, position.y - (fontSize / 2) - padding);
                                });


                            }
                            else {
                                switch (meta.controller.chart.chart.canvas.id) {
                                    case "LineWithLine6":
                                        meta.data.forEach(function (element, index) {
                                            // Draw the text in black, with the specified font
                                            var ctx2 = document.getElementById('LineWithLine6').getContext('2d');;
                                            ctx2.fillStyle = 'black';
                                            var fontSize = 16;
                                            var fontStyle = 'normal';
                                            var fontFamily = 'Helvetica Neue';
                                            ctx2.font = Chart.helpers.fontString(fontSize, fontStyle, fontFamily);
                                            // Just naively convert to string for now
                                            var dataString = dataset.data[index].toString();
                                            // Make sure alignment settings are correct
                                            ctx2.textAlign = 'center';
                                            ctx2.textBaseline = 'middle';
                                            var padding = 5;
                                            var position = element.tooltipPosition();
                                            ctx2.fillText(dataString, position.x, position.y - (fontSize / 2) - padding);
                                        });
                                        break;
                                    case "LineWithLine7":
                                        meta.data.forEach(function (element, index) {
                                            // Draw the text in black, with the specified font
                                            var ctx3 = document.getElementById('LineWithLine7').getContext('2d');;
                                            ctx3.fillStyle = 'black';
                                            var fontSize = 16;
                                            var fontStyle = 'normal';
                                            var fontFamily = 'Helvetica Neue';
                                            ctx3.font = Chart.helpers.fontString(fontSize, fontStyle, fontFamily);
                                            // Just naively convert to string for now
                                            var dataString = dataset.data[index].toString();
                                            // Make sure alignment settings are correct
                                            ctx3.textAlign = 'center';
                                            ctx3.textBaseline = 'middle';
                                            var padding = 5;
                                            var position = element.tooltipPosition();
                                            ctx3.fillText(dataString, position.x, position.y - (fontSize / 2) - padding);
                                        });
                                        break;
                                    case "LineWithLine8":
                                        meta.data.forEach(function (element, index) {
                                            // Draw the text in black, with the specified font
                                            var ctx4 = document.getElementById('LineWithLine8').getContext('2d');;
                                            ctx4.fillStyle = 'black';
                                            var fontSize = 16;
                                            var fontStyle = 'normal';
                                            var fontFamily = 'Helvetica Neue';
                                            ctx4.font = Chart.helpers.fontString(fontSize, fontStyle, fontFamily);
                                            // Just naively convert to string for now
                                            var dataString = dataset.data[index].toString();
                                            // Make sure alignment settings are correct
                                            ctx4.textAlign = 'center';
                                            ctx4.textBaseline = 'middle';
                                            var padding = 5;
                                            var position = element.tooltipPosition();
                                            ctx4.fillText(dataString, position.x, position.y - (fontSize / 2) - padding);
                                        });
                                        break;
                                    case "LineWithLine14":
                                        meta.data.forEach(function (element, index) {
                                            // Draw the text in black, with the specified font
                                            var ctx6 = document.getElementById('LineWithLine14').getContext('2d');;
                                            ctx6.fillStyle = 'black';
                                            var fontSize = 16;
                                            var fontStyle = 'normal';
                                            var fontFamily = 'Helvetica Neue';
                                            ctx6.font = Chart.helpers.fontString(fontSize, fontStyle, fontFamily);
                                            // Just naively convert to string for now
                                            var dataString = dataset.data[index].toString();
                                            // Make sure alignment settings are correct
                                            ctx6.textAlign = 'center';
                                            ctx6.textBaseline = 'middle';
                                            var padding = 5;
                                            var position = element.tooltipPosition();
                                            ctx6.fillText(dataString + '%', position.x, position.y - (fontSize / 2) - padding);
                                        }); break;
                                    default:
                                        break;
                                }
                            }
                        }
                    });
                }
            });

            myChart5 = new Chart(ctxBar, config);
            myChart5.resize();
        }


        function SetGraficaMantenimientoGeneralTiempos() {
            if (myChart14 != null) {
                myChart14.destroy();

            }
            var dt = tblGeneralMantenimientos.data();

            var lblTitulos = new Array();
            var porcentajes = new Array();
            var tiempos = new Array();


            for (var i = 0; i < dt.length; i++) {
                lblTitulos.push(dt[i].descripcion);
                porcentajes.push(dt[i].porcentajeTiempo);
                tiempos.push(dt[i].tiempo);
            }

            var config = {
                type: 'pie',
                data: {
                    datasets: [{
                        data: porcentajes,
                        backgroundColor: [
                           "#52BE80",
                            "#E74C3C",
                            "#F4D03F"
                        ],
                        label: 'Dataset 1'
                    }],
                    labels: lblTitulos
                },
                options: {
                    responsive: true
                },
                title: {
                    display: true,
                    position: "top",
                    text: "Status General de mantenimientos",
                    fontSize: 18,
                    fontColor: "#111"
                },
                legend: {
                    display: true,
                    position: 'bottom',
                    labels: {
                        fontColor: "#000080",
                    }
                },

            };

            var ctx = document.getElementById('LineWithLine14').getContext('2d');


            myChart14 = new Chart(ctx, config);
            myChart14.resize();
        }

        function setGraficasTiposMantenimientos() {

            tabVentanaGeneral.removeClass('active');
            tabVentanaGeneral.addClass('active in');
            tabVentanaPreventivo.removeClass('active');
            tabVentanaCorrectivo.removeClass('active');
            tabVentanaPredictivo.removeClass('active');
            titleModalTiposParo.text("Reporte Del periodo : " + txtFechaIncioFiltro.val() + " AL " + txtFechaFinFiltro.val() + (cboModelo.val() == "" ? (cboGrupo.val() == "" ? "" : " PARA " + $("#cboGrupo option:selected").text()) : " PARA MODELO " + $("#cboModelo option:selected").text()));
            dtColores = new Array();
            SetGraficaMantenimientoGeneral();
            SetGraficaMantenimientoGeneralTiempos();
            SetGraficaFrecuenciaPreventivo("LineWithLine6");
            SetGraficaFrecuenciaCorrectivo("LineWithLine7");
            SetGrarficaFrecuenciaPredictivo("LineWithLine8");

        }

        function SetGraficaFrecuenciaPreventivo(LineWithLine) {
            if (myChart6 != null) {
                myChart6.destroy();

            }
            var dt = tblFrecuenciasPreventivoGrid.data().sort(function (a, b) { return b.cantidadFrecuencias - a.cantidadFrecuencias });

            var dtLabel = new Array();
            var dtFrecuencia = new Array();

            var objTotalTiemposOT = 0;


            for (var i = 0; i < dt.length ; i++) {
                var colorName = GetColor();
                dtLabel.push(dt[i].cc);
                dtFrecuencia.push(dt[i].cantidadFrecuencias);
                dtColores.push(colorName);
            }

            var ctx = document.getElementById(LineWithLine).getContext("2d");

            var data = {
                labels: dtLabel,
                datasets: [{
                    label: "Centro Costos",
                    backgroundColor: dtColores,
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtFrecuencia,
                    fill: true,
                }
                ]
            };

            myChart6 = new Chart(ctx, {
                type: 'bar',
                data: data,
                options:
                     {
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                         scales: {
                             xAxes: [{
                                 ticks: {
                                     autoSkip: false,

                                 }
                             }],
                                 yAxes: [{
                                     ticks: {
                                         beginAtZero: true
                                     }
                                 }]
                         }
                     }
            });
        }
        function SetGraficaFrecuenciaCorrectivo(LineWithLine) {
            if (myChart7 != null) {
                myChart7.destroy();

            }
            var dt = tblFrecuenciaCorrectivoGrid.data().sort(function (a, b) { return b.cantidadFrecuencias - a.cantidadFrecuencias });

            var dtLabel = new Array();
            var dtFrecuencia = new Array();

            var objTotalTiemposOT = 0;

            for (var i = 0; i < dt.length ; i++) {
                dtLabel.push(dt[i].cc);
                dtFrecuencia.push(dt[i].cantidadFrecuencias);
            }

            var ctx = document.getElementById(LineWithLine).getContext("2d");

            var data = {
                labels: dtLabel,
                datasets: [{
                    label: dtLabel,
                    backgroundColor: dtColores,
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtFrecuencia,
                    fill: true,
                }
                ]
            };

            myChart7 = new Chart(ctx, {
                type: 'bar',
                data: data,
                options:
                     {
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                         scales: {
                             xAxes: [{
                                 ticks: {
                                     autoSkip: false,

                                 }
                             }],
                             yAxes: [{
                                 ticks: {
                                     beginAtZero: true
                                 }
                             }]
                         }
                     }
            });
        }
        function SetGrarficaFrecuenciaPredictivo(LineWithLine) {
            if (myChart8 != null) {
                myChart8.destroy();

            }
            var dt = tblFrecuenciaPredictivoGrid.data().sort(function (a, b) { return b.cantidadFrecuencias - a.cantidadFrecuencias });

            var dtLabel = new Array();
            var dtFrecuencia = new Array();

            var objTotalTiemposOT = 0;

            for (var i = 0; i < dt.length ; i++) {
                dtLabel.push(dt[i].cc);
                dtFrecuencia.push(dt[i].cantidadFrecuencias);
            }

            var ctx = document.getElementById(LineWithLine).getContext("2d");

            var data = {
                labels: dtLabel,
                datasets: [{
                    label: dtLabel,
                    backgroundColor: dtColores,
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtFrecuencia,
                    fill: true,
                }
                ]
            };

            myChart8 = new Chart(ctx, {
                type: 'bar',
                data: data,
                options:
                     {
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                         scales: {
                             xAxes: [{
                                 ticks: {
                                     autoSkip: false,

                                 }
                             }],
                             yAxes: [{
                                 ticks: {
                                     beginAtZero: true
                                 }
                             }]
                         }
                     }
            });
        }
        function SetGraficaTendencia(dtObjPreventivo, dtObjCorrectivo, dtObjPredictivo) {
            if (myChart9 != null) {
                myChart9.destroy();

            }
            var dtPreventivo = new Array();
            var dtCorrectivo = new Array();
            var dtPredictivo = new Array();

            for (var i = 0; i < dtObjPreventivo.length ; i++) {
                dtPreventivo.push(dtObjPreventivo[i].cantidadFrecuencias);

            }

            for (var i = 0; i < dtObjCorrectivo.length ; i++) {
                dtCorrectivo.push(dtObjCorrectivo[i].cantidadFrecuencias);

            }
            for (var i = 0; i < dtObjPredictivo.length ; i++) {
                dtPredictivo.push(dtObjPredictivo[i].cantidadFrecuencias);
            }

            var ctx = document.getElementById("LineWithLine9").getContext("2d");

            var parts = txtFechaFinFiltro.val().split('/');

            var meses = ["ENERO", "FEB", "MARZO", "ABRIL", "MAY", "JUN", "JUL", "AGO", "SEP", "OCTUBRE", "NOV", "DIC"];
            var dtMeses = new Array();
            for (var i = 0; i < Number(parts[1]) ; i++) {
                dtMeses.push(meses[i]);
            }

            var array = []


            var data = {
                labels: dtMeses,
                datasets: [{
                    type: 'line',
                    label: "Preventivo",
                    backgroundColor: "#45B39D",
                    borderColor: "#0B5345",
                    data: dtPreventivo,
                    fill: false
                },
                {
                    type: 'line',
                    label: "Correctivo",
                    backgroundColor: "#C0392B",
                    borderColor: "#A93226",
                    data: dtCorrectivo,
                },
                {
                    type: 'line',
                    label: "Predictivo",
                    backgroundColor: "#F7DC6F",
                    borderColor: "#F1C40F",
                    data: dtPredictivo,
                    fill: false,
                }
                ]
            };

            myChart9 = new Chart(ctx, {
                type: 'line',
                data: data,
                options:
                     {
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                     }
            });
        }

        function SetGraficaTendenciaTiempos(dtObjPreventivo, dtObjCorrectivo, dtObjPredictivo) {
            if (myChart15 != null) {
                myChart15.destroy();

            }
            var dtPreventivo = new Array();
            var dtCorrectivo = new Array();
            var dtPredictivo = new Array();

            for (var i = 0; i < dtObjPreventivo.length ; i++) {
                dtPreventivo.push(dtObjPreventivo[i].tiempo.toFixed(2));

            }

            for (var i = 0; i < dtObjCorrectivo.length ; i++) {
                dtCorrectivo.push(dtObjCorrectivo[i].tiempo.toFixed(2));

            }
            for (var i = 0; i < dtObjPredictivo.length ; i++) {
                dtPredictivo.push(dtObjPredictivo[i].tiempo.toFixed(2));
            }

            var ctx = document.getElementById("LineWithLine15").getContext("2d");

            var parts = txtFechaFinFiltro.val().split('/');

            var meses = ["ENERO", "FEB", "MARZO", "ABRIL", "MAY", "JUN", "JUL", "AGO", "SEP", "OCTUBRE", "NOV", "DIC"];
            var dtMeses = new Array();
            for (var i = 0; i < Number(parts[1]) ; i++) {
                dtMeses.push(meses[i]);
            }

            var array = []

            var data = {
                labels: dtMeses,
                datasets: [{
                    type: 'line',
                    label: "Preventivo",
                    backgroundColor: "#45B39D",
                    borderColor: "#0B5345",
                    data: dtPreventivo,
                    fill: false
                },
                {
                    type: 'line',
                    label: "Correctivo",
                    backgroundColor: "#C0392B",
                    borderColor: "#A93226",
                    data: dtCorrectivo,
                },
                {
                    type: 'line',
                    label: "Predictivo",
                    backgroundColor: "#F7DC6F",
                    borderColor: "#F1C40F",
                    data: dtPredictivo,
                    fill: false,
                }
                ]
            };

            myChart15 = new Chart(ctx, {
                type: 'line',
                data: data,
                options:
                     {
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                     }
            });
        }

        function GetColor() {
            hexadecimal = new Array("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F")
            color_aleatorio = "#";
            for (i = 0; i < 6; i++) {
                posarray = aleatorio(0, hexadecimal.length)
                color_aleatorio += hexadecimal[posarray]
            }
            return color_aleatorio
        }

        function aleatorio(inferior, superior) {
            numPosibilidades = superior - inferior
            aleat = Math.random() * numPosibilidades
            aleat = Math.floor(aleat)
            return parseInt(inferior) + aleat
        }

        function SetGraficaCorrectivo() {
            if (myChart4 != null) {
                myChart4.destroy();

            }
            var dt = tblfrecuenciaParo.data().sort(function (a, b) { return b.tiempoOT - a.tiempoOT });

            var dtParos = new Array();

            var dtTiempoOT = new Array();
            var dtPorcentajeTiempoOT = new Array();

            var dtPorcentajeTiempoAcumulado = new Array();
            var objTotalTiemposOT = 0;

            for (var i = 0; i < dt.length ; i++) {
                dtParos.push(dt[i].motivoParo);
                dtTiempoOT.push(dt[i].tiempoOT);
                objTotalTiemposOT += dt[i].tiempoOT;
            }

            var porcentaje = 0
            for (var i = 0; i < dtTiempoOT.length; i++) {

                var temp = (dtTiempoOT[i] / objTotalTiemposOT) * 100;

                dtPorcentajeTiempoOT.push(80);
                porcentaje += temp;

                dtPorcentajeTiempoAcumulado.push(porcentaje.toFixed(2));
            }
            var ctx = document.getElementById("LineWithLine4").getContext("2d");

            var data = {
                labels: dtParos,
                datasets: [{
                    label: "Tiempo OT",
                    backgroundColor: "rgba(255, 159, 64, 0.5)",
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtTiempoOT,
                    fill: false,
                    yAxisID: 'y-axis-1',
                }
                ]
            };

            myChart4 = new Chart(ctx, {
                type: 'bar',
                data: data,
                options:
                     {
                         responsive: false,
                         tooltips: {
                             mode: 'label'
                         },
                         elements: {
                             line: {
                                 fill: false
                             }
                         },
                         scales: {
                             xAxes: [{
                                 ticks: {
                                     autoSkip: false,

                                 }
                             }],
                             yAxes: [{
                                 ticks: {
                                     beginAtZero: true
                                 }
                             }]
                         }
                     }
            });
        }

        var backgroundColor = 'white';
        Chart.plugins.register({
            beforeDraw: function (c) {
                var ctx = c.chart.ctx;
                ctx.fillStyle = backgroundColor;
                ctx.fillRect(0, 0, c.chart.width, c.chart.height);
            }
        });

        function done() {
            var Data = "";
            var titulo = "";

            if ($("#tabPareto").hasClass('active')) {
                if ($("#tabVentanaTiempoOT").hasClass('active')) {
                    Data = document.getElementById('LineWithLine4').toDataURL('image/png');
                    titulo = "GRAFICA DE TIEMPOS DE OT";
                }

                if ($("#tabVentanaFrecuenciaParo").hasClass('active')) {
                    Data = document.getElementById('LineWithLine2').toDataURL('image/png');
                    titulo = "GRAFICA DE FRECUENCIA DE PAROS";
                }
            }
            if ($("#tabCentroCostos").hasClass('active')) {

                if ($("#tabVentanaTiemposCC").hasClass('active')) {
                    Data = document.getElementById('LineWithLine10').toDataURL('image/png');
                    titulo = "GRAFICA DE TIEMPOS POR CC"
                }

                if ($("#tabVentanaFrecuenciaParoCC").hasClass('active')) {
                    Data = document.getElementById('LineWithLine12').toDataURL('image/png');
                    titulo = "GRAFICA DE FRECUENCIAS POR CC"
                }
            }

            if ($("#tabTendencia").hasClass('active')) {

                if ($("#tabVentanaTiempoTendencia").hasClass('active')) {
                    Data = document.getElementById('LineWithLine11').toDataURL('image/png');
                    titulo = "GRAFICA DE TENDENCIA DE TIEMPOS POR RANGO DE FECHAS, PERIODO: " + txtFechaIncioFiltro.val() + " AL " + txtFechaFinFiltro.val();
                }

                if ($("#tabVentanaFrecuenciaParoTendencia").hasClass('active')) {
                    Data = document.getElementById('LineWithLine13').toDataURL('image/png');
                    titulo = "GRAFICA DE TENDENCIA DE FRECUENCIAS POR RANGO DE FECHAS, PERIODO: " + txtFechaIncioFiltro.val() + " AL " + txtFechaFinFiltro.val();
                }

                if ($("#tabVentanaTendencia").hasClass('active')) {
                    Data = document.getElementById('LineWithLine9').toDataURL('image/png');
                    titulo = "GRAFICA DE TENDENCIA POR RANGO DE FECHAS, PERIODO: " + txtFechaIncioFiltro.val() + " AL " + txtFechaFinFiltro.val();
                }
            }

            if ($("#tabVentanaHorasHombre").hasClass('active')) {
                Data = document.getElementById('LineWithLine3').toDataURL('image/png');
                titulo = "GRAFICA DE HORAS HOMBRE";
            }

            var bandera = false;
            if ($("#tabVentanaGeneral").hasClass('active')) {
                Data = document.getElementById('LineWithLine5').toDataURL('image/png');
                titulo = "GRAFICA DE MANTENIMIENTOS GENERAL";
                bandera = true;
            }
            if (!bandera) {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/OT/GetIMG',
                    data: { obj: Data },
                    async: false,
                    success: function (response) {

                        displayReporte(titulo);
                        $.unblockUI();
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            else {

                displayRptGeneral();
            }
        }

        function displayRptGeneral() {

            var arrayIMG = new Array();
            if ($("#tabVentanaTiempoPie").hasClass('active')) {

                arrayIMG.push(document.getElementById('LineWithLine15').toDataURL('image/png'));
                arrayIMG.push(document.getElementById('LineWithLine14').toDataURL('image/png'));
            }

            if ($("#tabVentanaFrecuenciaParoPie").hasClass('active')) {
                arrayIMG.push(document.getElementById('LineWithLine5').toDataURL('image/png'));
                arrayIMG.push(document.getElementById('LineWithLine9').toDataURL('image/png'));
            }

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/HorasHombre/loadCargaHorasHombre',
                data: { obj: arrayIMG },
                async: false,
                success: function (response) {
                    displayReporteMantenimiento("");
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function displayReporteMantenimiento(ptituloGrafica) {

            $.blockUI({ message: mensajes.PROCESANDO });
            var path = "/Reportes/Vista.aspx?idReporte=" + 73 + "&pFIncio=" + txtFechaIncioFiltro.val() + "&pFFin=" + txtFechaFinFiltro.val() + "&pMotivoParo=" + SetSelected("cboMotivoParo") + "&pTipoParo=" + SetSelected("cboTipoParo") + "&pCodicionParo=" + SetSelected("cboCondicionParo") + "&pGrupoMaquinaria=" + SetSelected("cboGrupo") + "&pEconomico=" + SetSelected("cboNoEconomico") + "&pCodicionEquipo=" + SetSelected("cboEstatusEquipo") + "&ptituloGrafica=" + ptituloGrafica;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }


        function LoadTablaData() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/OT/loadDataFrecuenciasParo',
                data: { obj: getFiltrosObject(), horaIncio: txtHorometroInicial.val() != "" ? txtHorometroInicial.val() : 0, horaFinal: txtHorometroFinal.val() != "" ? txtHorometroFinal.val() : 0 },
                success: function (response) {

                    SetDataInTablesDetalle(response.dataset);
                    SetGraficaTendencia(response.dtPreventivoFecha, response.dtCorrectivoFecha, response.dtPredictivoFecha);
                    SetGraficaTendenciaTiempos(response.dtPreventivoFechaTendencia, response.dtCorrectivoFechaTendencia, response.dtPredictivoFechaTendencia);
                    SetDataGeneralMantenimientos(response.generalMantto);
                    SetDataGroupCCFrecuenciaParos(response.GroupCCFrecuenciaParos);
                    SetDataGroupCCTiempos(response.GroupCCTiempos);
                    SetDataGroupCCFrecuenciaParosTendencia(response.GroupCCFrecuenciaParosTendencia);
                    SetDataGroupCCTiemposTendencia(response.GroupCCTiemposTendencia);

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function SetDataGroupCCTiemposTendencia(dataSet) {
            tblTiemposOTTendenciaGrid = $("#tblTiemposOTTendencia").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: false,
                "bFilter": false,
                destroy: true,
                data: dataSet,
                buttons: [{ extend: 'excel', text: 'Exportar', footer: true, }, ],
                columns: [
                       {
                           data: "CentroCostos"
                       },
                       {
                           data: "Mes"
                       },
                        {
                            data: "TiemposTendecia",
                            createdCell: function (td, cellData, rowData, row, col) {
                                $(td).text(cellData);
                            }

                        }
                ],
                "paging": true,
                "info": false
            });
        }
        function SetDataGroupCCFrecuenciaParosTendencia(dataSet) {
            tblFrecuenciaParoTendenciaGrid = $("#tblFrecuenciaParoTendencia").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: false,
                "bFilter": false,
                destroy: true,
                data: dataSet,
                buttons: [{ extend: 'excel', text: 'Exportar', footer: true, }, ],
                columns: [
                       {
                           data: "CentroCostos"
                       },
                       {
                           data: "Mes"
                       },
                        {
                            data: "FrecuenciaTendecia"
                        }
                ],
                "paging": true,
                "info": false
            });
        }
        function SetDataGroupCCTiempos(dataSet) {
            tblTiemposOTCCGrid = $("#tblTiemposOTCC").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: false,
                "bFilter": false,
                destroy: true,
                data: dataSet,
                buttons: [{ extend: 'excel', text: 'Exportar', footer: true, }, ],
                columns: [
                       {
                           data: "CentroCostos"
                       },
                       {
                           data: "Tiempos",
                           createdCell: function (td, cellData, rowData, row, col) {
                               $(td).text(cellData);
                           }

                       }
                ],
                "order": [[1, "desc"]],
                "paging": true,
                "info": false
            });
        }
        function SetDataGroupCCFrecuenciaParos(dataSet) {
            tblFrecuenciaParoPorCCGrid = $("#tblFrecuenciaParoPorCC").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: false,
                "bFilter": false,
                destroy: true,
                data: dataSet,
                buttons: [{ extend: 'excel', text: 'Exportar', footer: true, }, ],
                columns: [
                       {
                           data: "CentroCostos"
                       },
                       {
                           data: "Frecuencia"
                       }
                ],
                "order": [[1, "desc"]],
                "paging": true,
                "info": false
            });
        }

        function SetDataInTablesDetalle(dataSet) {
            tblfrecuenciaParo = $("#tblfrecuenciaParo").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: false,
                "bFilter": false,
                dom: 'Bfrtip',
                destroy: true,
                data: dataSet,
                buttons: [{ extend: 'excel', text: 'Exportar', title: 'Análisis Frecuencia de Paro' }, ],
                columns: [
                       {
                           data: "no"
                       },
                       {
                           data: "motivoParo"
                       },
                       {
                           data: "frecuenciaParo"
                       },
                       {
                           data: "tiempoOT",
                           createdCell: function (td, cellData, rowData, row, col) {
                               $(td).text(rowData.tiempoOT + ' ' + "(Horas)");

                           }
                       },
                       {
                           data: "horasHombre"
                       },
                       {
                           data: "detalleFrecuencia",
                           createdCell: function (td, cellData, rowData, row, col) {
                               $(td).text('');
                               $(td).append('<button type="button" class="btn btn-default btn-block btn-sm" onclick="verDetalleParo(' + rowData.motivoParoID + ',\'' + rowData.motivoParo + '\')" >Detalle</button>');
                           }
                       },
                ],
                "paging": true,
                "info": false
            });
        }

        function setDataFrecuenciasPreventivo(dataSet) {
            tblFrecuenciasPreventivoGrid = $("#tblFrecuenciasPreventivo").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: false,
                "bFilter": false,
                destroy: true,
                data: dataSet,
                buttons: [{ extend: 'excel', text: 'Exportar', footer: true, }, ],
                columns: [
                      {
                          data: "cc",
                      },
                      {
                          data: "cantidadFrecuencias"
                      }
                ],
                "order": [[1, "desc"]],
                "paging": true,
                "info": false

            });
        }

        function setDataFrecuenciasCorrectivo(dataSet) {
            tblFrecuenciaCorrectivoGrid = $("#tblFrecuenciaCorrectivo").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: false,
                "bFilter": false,
                destroy: true,
                scrollCollapse: true,
                data: dataSet,
                buttons: [{ extend: 'excel', text: 'Exportar', footer: true, }, ],
                columns: [
                      {
                          data: "cc",
                      },
                      {
                          data: "cantidadFrecuencias"
                      }
                ],
                "order": [[1, "desc"]],
                "paging": true,
                "info": false

            });
        }

        function setDataFrecuenciasPredictivo(dataSet) {
            tblFrecuenciaPredictivoGrid = $("#tblFrecuenciaPredictivo").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: false,
                "bFilter": false,
                destroy: true,
                scrollCollapse: true,
                data: dataSet,
                buttons: [{ extend: 'excel', text: 'Exportar', footer: true, }, ],
                columns: [
                      {
                          data: "cc",
                      },
                      {
                          data: "cantidadFrecuencias"
                      }
                ],
                "order": [[1, "desc"]],
                "paging": true,
                "info": false

            });
        }

        function SetDataGeneralMantenimientos(dataSet) {
            tblGeneralMantenimientos = $("#tblGeneralMantenimientos").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: false,
                "bFilter": false,
                destroy: true,
                data: dataSet,
                buttons: [{ extend: 'excel', text: 'Exportar', footer: true, }, ],
                columns: [
                      {
                          data: "descripcion",
                          createdCell: function (td, cellData, rowData, row, col) {
                              switch (rowData.tipoParo) {
                                  case 1:
                                      $(td).css("background", "#52BE80");
                                      break;
                                  case 2:
                                      $(td).css("background", "#E74C3C");
                                      break;
                                  case 3:
                                      $(td).css("background", "#F4D03F");
                                      break;
                                  default:
                              }
                          }
                      },
                      {
                          data: "cantidadFrecuencias"
                      },
                                            {
                                                data: "tiempo"
                                            },
                      //{
                      //    data: "porcentajeRelativo",
                      //    createdCell: function (td, cellData, rowData, row, col) {
                      //        $(td).text(cellData + ' %');
                      //    }
                      //}
                ],
                "order": [[1, "desc"]],
                "paging": true,
                "info": false

            });
        }

        function getFiltrosObject() {
            return {

                CC: getValoresMultiples("#cboCC"),
                FechaInicio: txtFechaIncioFiltro.val(),
                FechaFin: txtFechaFinFiltro.val(),
                MotivoParo: cboMotivoParo.val(),
                TipoParo: cboTipoParo.val(),
                TipoMatto: cboCondicionParo.val(),
                grupo: cboGrupo.val() == "" ? 0 : cboGrupo.val(),
                economico: cboNoEconomico.val() == "" ? 0 : cboNoEconomico.val(),
                CondicionParo: cboEstatusEquipo.val(),
                modelo : cboModelo.val() == "" ? 0 : cboModelo.val()
            }
        }
        init();
    };

    $(document).ready(function () {
        maquinaria.OT.rptAnalisisFrecuenciaParos = new rptAnalisisFrecuenciaParos();
    });
})();

function verDetalleParo(tipoParoID, descripcion) {
    $("#titleModalDetalle").text(descripcion + "- PERIODO " + txtFechaIncioFiltro.val() + " AL " + txtFechaFinFiltro.val());
    $("#modalDetalleParoEquipo").block({ message: null });
    verDetalleParoLoad(tipoParoID, descripcion);
}
$(document).ready(function () {
    $('#modalDetalleParoEquipo').on('shown.bs.modal', function () {
        tblDetalleTiposParo.draw();
    });
});

function verDetalleParoLoad(tipoParoID, motivoParo) {
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        datatype: "json",
        type: "POST",
        url: '/OT/GetInfoMotivoParo',
        data: { TipoParoID: tipoParoID, FechaInicio: $("#txtFechaIncioFiltro").val(), FechaFin: $("#txtFechaFinFiltro").val() },
        async: false,
        success: function (response) {

            $("#modalDetalleParoEquipo").modal('show');
            setTablaTipoParoDet(response.dataset);
            tblDetalleTiposParo.draw();

            $("#txtOTDetalle").text(motivoParo);
            $("#modalDetalleParoEquipo").unblock();
            $.unblockUI();
        },
        error: function () {
            $.unblockUI();
        }
    });

}

function setTablaTipoParoDet(dataset) {
    tblDetalleTiposParo = $("#tblDetalleTiposParo").DataTable({
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de MAX registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        },
        responsive: false,
        "bFilter": false,
        destroy: true,
        scrollY: '50vh',
        scrollCollapse: true,
        data: dataset,
        buttons: [{ extend: 'excel', text: 'Exportar', footer: true, }, ],
        columns: [

              {
                  data: "folio"
              },
              {
                  data: "cantidadPersonas"
              },
              {
                  data: "economico"
              },
              {
                  data: "tiempoUtil"
              },
               {
                   data: "tiempoMuerto"
               },
               {
                   data: "comentariosSolucion"

               },
               {
                   data: "horashombre"

               },
        ],
        "paging": true,
        "info": false

    });
}