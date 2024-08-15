
(function () {

    $.namespace('maquinaria.Captura.Diaria.rptConcentradoHH');

    rptConcentradoHH = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        },
        mensajes = {
            NOMBRE: 'Reporte concentrado de horas-hombre',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },

        divDetalleEmpleados = $("#divDetalleEmpleados"),
        divDEtallePuesto = $("#divDEtallePuesto"),
        modalDetalleDistribucion = $("#modalDetalleDistribucion"),
        btnRptDetalleDistribucion = $("#btnRptDetalleDistribucion"),
        tblDetallePuesto = $("#tblDetallePuesto"),

        modalDetalleUtilizacion = $("#modalDetalleUtilizacion");
        modalEmpleadoDetalle = $("#modalEmpleadoDetalle"),
        btnRptDetalleEmpleado = $("#btnRptDetalleEmpleado"),
        modalEmpleadoDetalleNombre = $("#modalEmpleadoDetalleNombre"),
        modalEmpleadoDetallePuesto = $("#modalEmpleadoDetallePuesto"),


        cboCC = $("#cboCC"),
        txtFechaIncioFiltro = $("#txtFechaIncioFiltro"),
        txtFechaFinFiltro = $("#txtFechaFinFiltro"),
        cboCategorias = $("#cboCategorias"),
        cboSubCategorias = $("#cboSubCategorias"),
        btnRptPorPuesto = $("#btnRptPorPuesto"),
        divDistribucionGeneral = $("#divDistribucionGeneral"),
        divConcentradoGeneral = $("#divConcentradoGeneral"),
        btnRptPorCatySub = $("#btnRptPorCatySub"),
        tblGeneralPuestos = $("#tblGeneralPuestos");
        tblReporteGlobalDistribucion = $("#tblReporteGlobalDistribucion"),

        btnBusqueda = $("#btnBusqueda");


        lblMaquinariaOT = $("#lblMaquinariaOT"),
        lblInstalacion = $("#lblInstalacion"),
        lblLimpieza = $("#lblLimpieza"),
        lblConsultaInformacion = $("#lblConsultaInformacion"),
        lblTiempoDescanso = $("#lblTiempoDescanso"),
        lblCursosyCapacitaciones = $("#lblCursosyCapacitaciones"),
        lblMonitoreoDiario = $("#lblMonitoreoDiario");

        LineWithLine1 = $("#LineWithLine1"),
        myChart1 = null;
        LineWithLine2 = $("#LineWithLine2"),
        myChart2 = null;
        LineWithLine3 = $("#LineWithLine3"),
        myChart3 = null;
        LineWithLine4 = $("#LineWithLine4"),
        myChart4 = null;
        LineWithLine5 = $("#LineWithLine5"),
        myChart5 = null;
        LineWithLine6 = $("#LineWithLine6"),
        myChart6 = null;
        LineWithLine13 = $("#LineWithLine13"),
        myChart13 = null;

        LineWithLine7 = $("#LineWithLine7"),
        LineWithLine8 = $("#LineWithLine8"),
        btnRptUtilizacion = $("#btnRptUtilizacion");
        modalGraficaPareto = $("#modalGraficaPareto");

        myChart12 = null;
        myChart11 = null;

        modalverRPTPuesto = $("#modalverRPTPuesto");
        cboPuestosModal = $("#cboPuestosModal"),
        cboNombrePersonal = $("#cboNombrePersonal"),
        btnBuscarrpt = $("#btnBuscarrpt"),
        tblDetalleHorasHombreRpt = $("#tblDetalleHorasHombreRpt"),
        btnVerReporte = $("#btnVerReporte");
        cboCentroCostosMOdal = $("#cboCentroCostosMOdal"),
        tblRptPorPuestos = $("#tblRptPorPuestos");


        function init() {

            var now = new Date();
           
            var mes = now.getMonth() + 1;
            txtFechaIncioFiltro.datepicker().datepicker("setDate", "01/" + mes.toString().padStart(2, '0') + "/" + now.getUTCFullYear());
            txtFechaFinFiltro.datepicker().datepicker("setDate", new Date());
            cboCC.fillCombo('/HorasHombre/fillCboCC', null, false, "Todos");
            //cboCC.fillCombo('/HorasHombre/fillCboCC', null, false, "--Seleccione--");
            convertToMultiselect("#cboCC");
            cboCC.change(setCC);
            cboCategorias.fillCombo('/OT/FillCboCategorias', null, false, "Todos");
            convertToMultiselect("#cboCategorias");

            cboCategorias.change(loadSubCatergoriasHH);
            btnBusqueda.click(btnLoadTablas);

            cboSubCategorias.fillCombo('/OT/FillCboSubCategorias', { listCategorias: getValoresMultiples("#cboCategorias") }, false, "Todos");
            convertToMultiselect("#cboSubCategorias");
            btnRptPorPuesto.click(RptPorPuestos);
            btnRptDetalleEmpleado.click(verRptConcentradoHorasHombrePersonal);
            btnRptDetalleDistribucion.click(verReportePuestosDistribucion);
            btnRptUtilizacion.click(verReporteUtilizacion);
            btnRptPorCatySub.click(loadParetoCategorias);
            cboPuestosModal.fillCombo('/OT/FillCboPuestosRpt', null, false, "Todos");
            convertToMultiselect("#cboPuestosModal")

            cboPuestosModal.change(LoadPersonal);
            cboNombrePersonal.fillCombo('/OT/FillCboPersonalPuestosRpt', { Puestos: getValoresMultiples("#cboPuestosModal"), ccs: getValoresMultiples("#cboCC") }, false, "Todos");
            convertToMultiselect("#cboNombrePersonal");

            btnBuscarrpt.click(LoadtablaPorPuestos);
            btnVerReporte.click(verRptPorPuesto);

        }

        function setCC() {
            cboCentroCostosMOdal.fillCombo('/OT/setCCConcentrado', { ccs: getValoresMultiples("#cboCC") }, false, "Todos");
            convertToMultiselect("#cboCentroCostosMOdal");
        }

        function LoadtablaPorPuestos() {
            url = '/OT/DetalleGeneralPorPuesto';

            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'json',
                data: { ccs: getValoresMultiples("#cboCentroCostosMOdal"), FechaInicio: $("#txtFechaIncioFiltro").val(), FechaFin: $("#txtFechaFinFiltro").val(), listaCategorias: getValoresMultiples("#cboPuestosModal"), listaSubCategoria: getValoresMultiples("#cboNombrePersonal") },
                success: function (response) {
                    $("#tblTitleColumns").html(tipoRpt == 1 ? "CC" : (tipoRpt == 2 ? "PUESTO" : "EMPLEADO"));

                    loadTablaConcentradoRpt(response.dtSet, response.TipoRpt);

                    LoadGraficasRptConcentrado(response.dataSetGrafica, response.titulos);

                    lblMaquinariaOT.text(response.trabajosInstalaciones);
                    lblInstalacion.text(response.trabajoMaquinariaOT);
                    lblLimpieza.text(response.limpieza);
                    lblTiempoDescanso.text(response.tiempoDescanso);
                    lblConsultaInformacion.text(response.consultaInformacion);
                    lblCursosyCapacitaciones.text(response.cursosCapacitaciones);
                    lblMonitoreoDiario.text(response.monitoreoDiario);


                },
                error: function (response) {
                    alert(response.message);
                }
            });
        }

        function LoadGraficasRptConcentrado(array, lblTitulos) {
            if (myChart13 != null) {
                myChart13.destroy();
            }
            var config = {
                type: 'pie',
                data: {
                    datasets: [{
                        data: array,
                        backgroundColor: [
                           "#A569BD",
                           "#5DADE2",
                           "#45B39D",
                           "#F4D03F",
                           "#EB984E",
                           "#CACFD2"

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
                    text: "Concentrado General",
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

            var ctxBar = document.getElementById('LineWithLine13').getContext('2d');

            myChart13 = new Chart(ctxBar, config);
            myChart13.resize();


        }


        function verRptPorPuesto() {

            if (getValoresMultiples("#cboCentroCostosMOdal").length > 0) {
                $.blockUI({ message: mensajes.PROCESANDO });
                var Periodo = "Del " + $("#txtFechaIncioFiltro").val() + " AL " + $("#txtFechaFinFiltro").val();

                var path = "/Reportes/Vista.aspx?idReporte=" + 82 + "&pPerioriodo=" + Periodo;
                $("#report").attr("src", path);
                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
            }
            else {
                AlertaGeneral('Alerta', 'Favor de Seleccionar un centro de costos para poder mostrar reporte.')
            }
        }

        function loadTablaConcentradoRpt(dataSet, tipoRpt) {
            tblRptPorPuestosGrid = $("#tblRptPorPuestos").DataTable({
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
                responsive: true,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                      {

                          data: "nombreEmpleado",
                          createdCell: function (td, cellData, rowData, row, col) {
                              $(td).text('');
                              switch (tipoRpt) {
                                  case 1:
                                      {
                                          $(td).text(rowData.centrosCosto);
                                          break;
                                      }
                                  case 2:
                                      {
                                          $(td).text(rowData.puesto);
                                          break;
                                      }
                                  case 3:
                                      {
                                          $(td).text(rowData.nombreEmpleado);
                                          break;
                                      }

                                  default:

                              }

                          }

                      },
                      {
                          data: "trabajoMaquinariaOT"
                      },
                      {
                          data: "trabajosInstalaciones"
                      },
                      {
                          data: "monitoreoDiario"
                      },
                      {
                          data: "limpieza"
                      },
                      {
                          data: "consultaInformacion"

                      },
                      {
                          data: "tiempoDescanso"

                      },
                      {
                          data: "cursosCapacitaciones"

                      },
                      {
                          data: "totalHorashombre"
                      }
                ],
                "paging": false,
                "info": false
            });
        }

        function LoadPersonal() {
            cboNombrePersonal.clearCombo();
            cboNombrePersonal.fillCombo('/OT/FillCboPersonalPuestosRpt', { Puestos: getValoresMultiples("#cboPuestosModal"), ccs: getValoresMultiples("#cboCC") }, false, "Todos");
            convertToMultiselect("#cboNombrePersonal");
        }

        function RptPorPuestos() {
            if (getValoresMultiples("#cboCC").length > 0) {
                tipoRpt = 1;
                $("#tblTitleColumns").html(tipoRpt == 1 ? "CC" : (tipoRpt == 2 ? "PUESTO" : "EMPLEADO"));
                setCC();
                cboPuestosModal.fillCombo('/OT/FillCboPuestosRpt', null, false, "Todos");
                convertToMultiselect("#cboPuestosModal")
                LoadPersonal();
                modalverRPTPuesto.modal('show');
            }
            else {

                AlertaGeneral('Alerta', 'Se debe seleccionar por lo menos un centro de costos para ver el reporte de parteo.')
            }


        }

        function loadParetoCategorias() {

            if (getValoresMultiples("#cboCC").length > 0) {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/OT/loadParetoCategorias',
                    data: { ccs: getValoresMultiples("#cboCC"), fechaInicio: txtFechaIncioFiltro.val(), fechaFin: txtFechaFinFiltro.val(), listaCategorias: getValoresMultiples("#cboCategorias"), listaSubCategoria: getValoresMultiples("#cboSubCategorias") },
                    async: false,
                    success: function (response) {
                        SetParetoCategorias(response.Categoria);
                        SetParetoSubCategorias(response.SubCategoria);
                        modalGraficaPareto.modal('show');
                        $.unblockUI();
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            else {

                AlertaGeneral('Alerta', 'Se debe seleccionar por lo menos un centro de costos para ver el reporte de parteo.')
            }

        }



        function SetParetoCategorias(dt) {

            if (myChart12 != null) {
                myChart12.destroy();
            }
            var objTotalCategorias = 0;

            var dtParos = new Array();
            var dtCategoriasValue = new Array();
            var dtPorcentaje = new Array();
            var dtPorcentajeAcumulado = new Array();

            for (var i = 0; i < dt.length ; i++) {
                dtParos.push(dt[i].Descripcion);
                dtCategoriasValue.push(dt[i].valuePareto);
                objTotalCategorias += dt[i].valuePareto;
            }

            var porcentaje = 0
            for (var i = 0; i < dtCategoriasValue.length; i++) {

                var temp = (dtCategoriasValue[i] / objTotalCategorias) * 100;

                dtPorcentaje.push(80);
                porcentaje += temp;

                dtPorcentajeAcumulado.push(porcentaje.toFixed(2));
            }

            var ctx = document.getElementById("LineWithLine11").getContext("2d");

            var data = {
                labels: dtParos,
                datasets: [{
                    label: "Categorias",
                    backgroundColor: "rgba(255, 159, 64, 0.5)",
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtCategoriasValue,
                    fill: false,
                    yAxisID: 'y-axis-1',
                }, {
                    type: 'line',
                    label: "Porcentaje",
                    backgroundColor: "green",
                    borderColor: "green",
                    data: dtPorcentajeAcumulado,
                    fill: false,
                    yAxisID: 'y-axis-2',
                },
                {
                    type: 'line',
                    label: "Limite",
                    backgroundColor: "red",
                    borderColor: "red",
                    data: dtPorcentaje,
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

                                 if (ctx.canvas.id == 'LineWithLine9') {
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


        function SetParetoSubCategorias(dt) {

            if (myChart11 != null) {
                myChart11.destroy();
            }
            var objTotalCategorias = 0;

            var dtParos = new Array();
            var dtCategoriasValue = new Array();
            var dtPorcentaje = new Array();
            var dtPorcentajeAcumulado = new Array();

            for (var i = 0; i < dt.length ; i++) {
                dtParos.push(dt[i].Descripcion);
                dtCategoriasValue.push(dt[i].valuePareto);
                objTotalCategorias += dt[i].valuePareto;
            }

            var porcentaje = 0
            for (var i = 0; i < dtCategoriasValue.length; i++) {

                var temp = (dtCategoriasValue[i] / objTotalCategorias) * 100;

                dtPorcentaje.push(80);
                porcentaje += temp;

                dtPorcentajeAcumulado.push(porcentaje.toFixed(2));
            }

            var ctx = document.getElementById("LineWithLine12").getContext("2d");

            var data = {
                labels: dtParos,
                datasets: [{
                    label: "SubCategorias",
                    backgroundColor: "rgba(255, 159, 64, 0.5)",
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtCategoriasValue,
                    fill: false,
                    yAxisID: 'y-axis-1',
                }, {
                    type: 'line',
                    label: "Porcentaje",
                    backgroundColor: "green",
                    borderColor: "green",
                    data: dtPorcentajeAcumulado,
                    fill: false,
                    yAxisID: 'y-axis-2',
                },
                {
                    type: 'line',
                    label: "Limite",
                    backgroundColor: "red",
                    borderColor: "red",
                    data: dtPorcentaje,
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

                                 if (ctx.canvas.id == 'LineWithLine12') {
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

        function verReportePuestosDistribucion() {
            $.blockUI({ message: mensajes.PROCESANDO });

            var Periodo = "Del " + txtFechaIncioFiltro.val() + " AL " + txtFechaFinFiltro.val();
            var puestoID = btnRptDetalleDistribucion.data().idpuesto;

            var path = "/Reportes/Vista.aspx?idReporte=" + 80 + "&pPerioriodo=" + Periodo + "&pPuestoID=" + puestoID;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function verReporteUtilizacion() {
            $.blockUI({ message: mensajes.PROCESANDO });

            var Periodo = "Del " + txtFechaIncioFiltro.val() + " AL " + txtFechaFinFiltro.val();
            var puestoID = btnRptUtilizacion.data().idpuesto;
            var path = "/Reportes/Vista.aspx?idReporte=" + 81 + "&pPerioriodo=" + Periodo + "&pPuestoID=" + puestoID;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }


        function verRptConcentradoHorasHombrePersonal() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var path = "/Reportes/Vista.aspx?idReporte=" + 79 + "&pPuesto=" + modalEmpleadoDetallePuesto.val() + "&pNombre=" + modalEmpleadoDetalleNombre.val();
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        function loadrptUtilizacionGeneral() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/OT/loadrptConcentradoHorasHombre',
                data: { ccs: getValoresMultiples("#cboCC"), fechaInicio: txtFechaIncioFiltro.val(), fechaFin: txtFechaFinFiltro.val(), listaCategorias: getValoresMultiples("#cboCategorias"), listaSubCategoria: getValoresMultiples("#cboSubCategorias") },
                async: false,
                success: function (response) {

                    setDatatableGeneral(response.GeneralConcentradoHHDTOList);
                    setDatatableDistribucion(response.DistribucionHHDTOList)
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }

        $("a[href='#DistribucionGeneral']").on('shown.bs.tab', function (e) {
            tblReporteGlobalDistribucionGrid.draw();
        });
        $("a[href='#ConcentradoGeneral']").on('shown.bs.tab', function (e) {
            tblGeneralPuestosGrid.draw();
        });



        function autoComplete() {
            modalEmpleadoDetalleNombre.getAutocomplete(SelectEmpleado, getFiltrosEmpleados(), '/HorasHombre/searchEmpleado');
        }

        function getFiltrosEmpleados() {
            var obj = {};
            obj.puesto = modalEmpleadoDetallePuesto.val() == null || modalEmpleadoDetallePuesto.val() == "" ? 0 : modalEmpleadoDetallePuesto.val();


            return obj;
        }

        function SelectEmpleado(event, ui) {
            modalEmpleadoDetalleNombre.text(ui.item.value);
            modalEmpleadoDetalleNombre.attr('data-numEmpleado', ui.item.id)
        }

        function loadSubCatergoriasHH() {
            cboSubCategorias.fillCombo('/OT/FillCboSubCategorias', { listCategorias: getValoresMultiples("#cboCategorias") }, false, "Todos");
            convertToMultiselect("#cboSubCategorias");
        }

        function btnLoadTablas() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/OT/loadrptConcentradoHorasHombre',
                data: { ccs: getValoresMultiples("#cboCC"), fechaInicio: txtFechaIncioFiltro.val(), fechaFin: txtFechaFinFiltro.val(), listaCategorias: getValoresMultiples("#cboCategorias"), listaSubCategoria: getValoresMultiples("#cboSubCategorias") },
                async: false,
                success: function (response) {

                    setDatatableGeneral(response.GeneralConcentradoHHDTOList);
                    setDatatableDistribucion(response.DistribucionHHDTOList)
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function setDatatableGeneral(dataSet) {
            tblGeneralPuestosGrid = $("#tblGeneralPuestos").DataTable({
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
                responsive: true,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [

                      {
                          data: "puesto"
                      },
                      {
                          data: "horashombre"
                      },
                      {
                          data: "costohorashombre"
                      },
                       {
                           data: "CostoTotalHorasHombre"
                       },
                      {
                          data: "porRegistro"
                      }
                      , {
                          data: "porHorasEfectivas"

                      }
                      , {
                          data: "btn",
                          createdCell: function (td, cellData, rowData, row, col) {
                              $(td).text('');

                              $(td).append('<button type="button" class="btn btn-default btn-block btn-sm" onclick="LoadDetalleUtilizacion(' + rowData.puestoID + ')" >Detalle</button>');

                          }

                      }
                ],
                "paging": false,
                "info": false,
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };

                    // Total over all pages
                    CostoTotalHH = api
                        .column(3)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    HorasTotales = api
                        .column(1)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    HorasTotalespageTotal = api
                    .column(1, { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                    // Total over this page
                    CostoTotalHHpageTotal = api
                        .column(3, { page: 'current' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Update footer
                    $(api.column(3).footer()).html(
                         '$' + CostoTotalHHpageTotal.toFixed(2) + ' ( $' + CostoTotalHH.toFixed(2) + ' total)'
                    );
                    $(api.column(1).footer()).html(
                        HorasTotalespageTotal.toFixed(2) + ' (' + HorasTotales.toFixed(2) + ' total)'
                   );
                }

            });
        }

        function setDatatableDistribucion(dataSet) {
            tblReporteGlobalDistribucionGrid = $("#tblReporteGlobalDistribucion").DataTable({
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
                responsive: true,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [

                      {
                          data: "puesto"
                      },
                      {
                          data: "trabajoMaquinariaOT"
                      },
                      {
                          data: "trabajosInstalaciones"
                      },
                      {
                          data: "monitoreoDiario"
                      },
                      {
                          data: "limpieza"
                      },
                      {
                          data: "consultaInformacion"

                      },
                      {
                          data: "tiempoDescanso"

                      },
                      {
                          data: "cursosCapacitaciones"

                      },
                      {
                          data: "totalHorashombre"
                      },
                      {
                          data: "btn",
                          createdCell: function (td, cellData, rowData, row, col) {
                              $(td).text('');
                              $(td).append('<button type="button" class="btn btn-primary btn-block btn-sm" onclick="DetalleDistribucionGeneral(' + rowData.puestoID + ')" >Ver Detalle</button>');

                          }
                      }
                ],
                "paging": false,
                "info": false,
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    totalTotalHH = api
                    .column(8)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);


                    // Total over all pages
                    totalOT = api
                        .column(1)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    TotalInstalaciones = api
                        .column(2)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    // Total over all pages
                    TotalLimpieza = api
                        .column(3)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    TotalConsultaInformacion = api
                        .column(4)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    TotalTiempoDescanso = api
                    .column(5)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                    TotalCursosCapacitaciones = api
                        .column(6)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    TotalCursosCapacitaciones2 = api
                                    .column(7)
                                    .data()
                                    .reduce(function (a, b) {
                                        return intVal(a) + intVal(b);
                                    }, 0);


                    totalRowsOT = api
                    .column(1, { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                    totalRowInstalaciones = api
                     .column(2, { page: 'current' })
                     .data()
                     .reduce(function (a, b) {
                         return intVal(a) + intVal(b);
                     }, 0);

                    totalRowLimpieza = api
                     .column(3, { page: 'current' })
                     .data()
                     .reduce(function (a, b) {
                         return intVal(a) + intVal(b);
                     }, 0);
                    totalRowConsultaInformacion = api
                     .column(4, { page: 'current' })
                     .data()
                     .reduce(function (a, b) {
                         return intVal(a) + intVal(b);
                     }, 0);
                    totalRowTiempoDescanso = api
                     .column(5, { page: 'current' })
                     .data()
                     .reduce(function (a, b) {
                         return intVal(a) + intVal(b);
                     }, 0);
                    totalRowCursosCapacitaciones = api
                     .column(6, { page: 'current' })
                     .data()
                     .reduce(function (a, b) {
                         return intVal(a) + intVal(b);
                     }, 0);
                    totalRowCursosCapacitaciones2 = api
                     .column(7, { page: 'current' })
                     .data()
                     .reduce(function (a, b) {
                         return intVal(a) + intVal(b);
                     }, 0);
                    totalRowHH = api
                     .column(8, { page: 'current' })
                     .data()
                     .reduce(function (a, b) {
                         return intVal(a) + intVal(b);
                     }, 0);


                    // Update footer
                    $(api.column(1).footer()).html(
                          totalRowsOT.toFixed(2) + ' (' + totalOT.toFixed(2) + ' total)'
                    );
                    $(api.column(2).footer()).html(
                        totalRowInstalaciones.toFixed(2) + ' (' + TotalInstalaciones.toFixed(2) + ' total)'
                   );
                    $(api.column(3).footer()).html(
                         totalRowLimpieza.toFixed(2) + ' (' + TotalLimpieza.toFixed(2) + ' total)'
                    );
                    $(api.column(4).footer()).html(
                        totalRowConsultaInformacion.toFixed(2) + ' (' + TotalConsultaInformacion.toFixed(2) + ' total)'
                   );
                    $(api.column(5).footer()).html(
                        totalRowTiempoDescanso.toFixed(2) + ' (' + TotalTiempoDescanso.toFixed(2) + ' total)'
                    );
                    $(api.column(6).footer()).html(
                        totalRowCursosCapacitaciones.toFixed(2) + ' (' + TotalCursosCapacitaciones.toFixed(2) + ' total)'
                    );
                    $(api.column(7).footer()).html(
                         totalRowCursosCapacitaciones2.toFixed(2) + ' (' + TotalCursosCapacitaciones2.toFixed(2) + ' total)'
                    );
                    $(api.column(8).footer()).html(
                        totalRowHH.toFixed(2) + ' (' + totalRowHH.toFixed(2) + ' total)'
                    );
                }
            });
        }


        init();

    };
    $(document).ready(function () {

        maquinaria.Captura.Diaria.rptConcentradoHH = new rptConcentradoHH();
    });
})();

var dtColores = new Array();
color = ["red", "orange", "yellow", "green", "blue", "purple", "grey"];
colorNames = ["red", "orange", "yellow", "green", "blue", "purple", "grey"];

function GetColor() {
    hexadecimal = new Array("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F")
    color_aleatorio = "#";
    for (i = 0; i < 6; i++) {
        posarray = aleatorio(0, hexadecimal.length)
        color_aleatorio += hexadecimal[posarray]
    }
    return color_aleatorio
}


function DetalleDistribucionGeneral(puestoID) {
    url = '/OT/DetalleDistribucionGeneral';

    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'json',
        data: { ccs: getValoresMultiples("#cboCC"), FechaInicio: $("#txtFechaIncioFiltro").val(), FechaFin: $("#txtFechaFinFiltro").val(), puestoID: puestoID },
        success: function (response) {

            $("#btnRptDetalleDistribucion").attr('data-idPuesto', puestoID);
            $("#divDEtallePuesto").removeClass('hide');
            $("#divDetalleEmpleados").addClass('hide');
            $("#modalDetalleDistribucion").modal('show');
            loadTableDetalleDistribucionGeneral(response.ConcentradoGeneral);



        },
        error: function (response) {
            alert(response.message);
        }
    });
}

function loadTableDetalleDistribucionGeneral(dataSet) {
    tblDetallePuestoGrid = $("#tblDetallePuesto").DataTable({
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
        responsive: true,
        destroy: true,
        scrollY: '50vh',
        scrollCollapse: true,
        data: dataSet,
        columns: [

              {
                  data: "nombreEmpleado"
              },
              {
                  data: "trabajoMaquinariaOT"
              },
              {
                  data: "trabajosInstalaciones"
              },
              {
                  data: "monitoreoDiario"
              },
              {
                  data: "limpieza"
              },
              {
                  data: "consultaInformacion"

              },
              {
                  data: "tiempoDescanso"

              },
              {
                  data: "cursosCapacitaciones"

              },
              {
                  data: "totalHorashombre"
              },
              {
                  data: "btn",
                  createdCell: function (td, cellData, rowData, row, col) {
                      $(td).text('');
                      $(td).append('<button type="button" class="btn btn-primary btn-block btn-sm" onclick="DetallePorPersonal(' + rowData.numEmpleado + ')" >Ver Detalle</button>');

                  }
              }
        ],
        "paging": false,
        "info": false,
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                    i : 0;
            };

            // Total over all pages
            totalOT = api
                .column(1)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            TotalInstalaciones = api
                .column(2)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            // Total over all pages
            TotalLimpieza = api
                .column(3)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            TotalConsultaInformacion = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            TotalTiempoDescanso = api
            .column(5)
            .data()
            .reduce(function (a, b) {
                return intVal(a) + intVal(b);
            }, 0);
            TotalCursosCapacitaciones = api
                .column(6)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            TotalCursosCapacitaciones2 = api
            .column(7)
            .data()
            .reduce(function (a, b) {
                return intVal(a) + intVal(b);
            }, 0);

            totalPage = api
            .column(8)
            .data()
    .reduce(function (a, b) {
        return intVal(a) + intVal(b);
    }, 0);

            totalRowsOT = api
            .column(1, { page: 'current' })
            .data()
            .reduce(function (a, b) {
                return intVal(a) + intVal(b);
            }, 0);
            totalRowInstalaciones = api
             .column(2, { page: 'current' })
             .data()
             .reduce(function (a, b) {
                 return intVal(a) + intVal(b);
             }, 0);

            totalRowLimpieza = api
             .column(3, { page: 'current' })
             .data()
             .reduce(function (a, b) {
                 return intVal(a) + intVal(b);
             }, 0);
            totalRowConsultaInformacion = api
             .column(4, { page: 'current' })
             .data()
             .reduce(function (a, b) {
                 return intVal(a) + intVal(b);
             }, 0);
            totalRowTiempoDescanso = api
             .column(5, { page: 'current' })
             .data()
             .reduce(function (a, b) {
                 return intVal(a) + intVal(b);
             }, 0);
            totalRowCursosCapacitaciones = api
             .column(6, { page: 'current' })
             .data()
             .reduce(function (a, b) {
                 return intVal(a) + intVal(b);
             }, 0);
            totalRowCursosCapacitaciones2 = api
             .column(7, { page: 'current' })
             .data()
             .reduce(function (a, b) {
                 return intVal(a) + intVal(b);
             }, 0);
            TotalTotal = api
 .column(8, { page: 'current' })
 .data()
 .reduce(function (a, b) {
     return intVal(a) + intVal(b);
 }, 0);
            // Update footer
            $(api.column(1).footer()).html(
                  totalOT.toFixed(2) + ' (' + totalRowsOT.toFixed(2) + ' total)'
            );
            $(api.column(2).footer()).html(
                TotalInstalaciones.toFixed(2) + ' (' + totalRowInstalaciones.toFixed(2) + ' total)'
           );
            $(api.column(3).footer()).html(
                 TotalLimpieza.toFixed(2) + ' (' + totalRowLimpieza.toFixed(2) + ' total)'
            );
            $(api.column(4).footer()).html(
                TotalConsultaInformacion.toFixed(2) + ' (' + totalRowConsultaInformacion.toFixed(2) + ' total)'
           );
            $(api.column(5).footer()).html(
                TotalTiempoDescanso.toFixed(2) + ' (' + totalRowTiempoDescanso.toFixed(2) + ' total)'
            );
            $(api.column(6).footer()).html(
                TotalCursosCapacitaciones.toFixed(2) + ' (' + totalRowCursosCapacitaciones.toFixed(2) + ' total)'
            );
            $(api.column(7).footer()).html(
    TotalCursosCapacitaciones2.toFixed(2) + ' (' + totalRowCursosCapacitaciones2.toFixed(2) + ' total)'
);
            $(api.column(7).footer()).html(
    totalPage.toFixed(2) + ' (' + TotalTotal.toFixed(2) + ' total)'
);
        }

    });
}


$(document).ready(function () {
    $('#modalDetalleUtilizacion').on('shown.bs.modal', function () {
        tlbUtilizacionDetalleGrid.draw();

    });
});

$(document).ready(function () {
    $('#modalDetalleDistribucion').on('shown.bs.modal', function () {
        tblDetallePuestoGrid.draw();
    });
});



$(document).ready(function () {
    $('#modalEmpleadoDetalle').on('shown.bs.modal', function () {
        tblEmpleadoDetalleDistibucionGrid.draw();
    });
});



function DetallePorPersonal(numEmpleado) {
    url = '/OT/DetallePersonalInfo';

    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'json',
        data: { ccs: getValoresMultiples("#cboCC"), FechaInicio: $("#txtFechaIncioFiltro").val(), FechaFin: $("#txtFechaFinFiltro").val(), numEmpleado: numEmpleado },
        success: function (response) {

            // $("#modalDetalleDistribucion").modal('hide');
            // $("#modalEmpleadoDetalle").modal('show');
            $("#divDEtallePuesto").addClass('hide'),
           $("#divDetalleEmpleados").removeClass('hide'),


            LoadTablaEmpleadosDetalle(response.DetallePersonaDT);
            modalEmpleadoDetallePuesto.val(response.nombre);
            modalEmpleadoDetalleNombre.val(response.puesto);
            setGraficaCategoriasPastel();

        },
        error: function (response) {
            alert(response.message);
        }
    });
}


function DetallePorPersonalUtilizacion(numEmpleado) {
    url = '/OT/DetallePersonalInfo';

    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'json',
        data: { ccs: getValoresMultiples("#cboCC"), FechaInicio: $("#txtFechaIncioFiltro").val(), FechaFin: $("#txtFechaFinFiltro").val(), numEmpleado: numEmpleado },
        success: function (response) {

            $("#modalDetalleDistribucion").modal('hide');
            $("#modalEmpleadoDetalle").modal('show');

            LoadTablaEmpleadosDetalle(response.DetallePersonaDT);
            modalEmpleadoDetallePuesto.val(response.nombre);
            modalEmpleadoDetalleNombre.val(response.puesto);

        },
        error: function (response) {
            alert(response.message);
        }
    });
}



function LoadTablaEmpleadosDetalle(dataSet) {
    var groupColumn = 5;
    tblEmpleadoDetalleDistibucionGrid = $('#tblEmpleadoDetalleDistibucion').DataTable({
        "columnDefs": [
            { "visible": false, "targets": 5 }
        ],
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar MENU registros",
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
        "order": [[5, "desc"]],
        "bFilter": false,
        destroy: true,
        scrollY: '50vh',
        scrollCollapse: true,
        paging: false,
        data: dataSet,
        columns: [

              {
                  data: "subCategoria"
              },
              {
                  data: "horasHombre"
              },
              {
                  data: "costoHH"
              },
              {
                  data: "costoTotal"
              },
              {
                  data: "totalGrupo",
                  createdCell: function (td, cellData, rowData, row, col) {
                      $(td).text('');
                      $(td).addClass('TotalGrupo');

                  }


              }, {
                  data: "categoria"

              },

        ],
        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;
            var datos = api.data();
            api.column(5, { page: 'current' }).data().each(function (group, i) {

                if (last !== group) {
                    var Total = datos[i].totalGrupo;
                    $(rows).eq(i).before(
                        '<tr class="group"><td colspan="4">' + group + '</td><td colspan="1">' + Total + '</td></tr>'
                    );

                    last = group;
                }
            });
        },
        "paging": false,
        "info": false

    });

}


/*Grafias Por Categorias*/
function setGraficaCategoriasPastel() {
    if (myChart1 != null) {
        myChart1.destroy();

    }
    if (myChart2 != null) {
        myChart2.destroy();

    }
    if (myChart3 != null) {
        myChart3.destroy();

    }
    if (myChart4 != null) {
        myChart4.destroy();
    }
    if (myChart5 != null) {
        myChart5.destroy();

    }

    if (myChart6 != null) {
        myChart6.destroy();

    }
    if (myChart15 != null) {
        myChart15.destroy();
    }

    var dt = tblEmpleadoDetalleDistibucionGrid.data();

    var TrabajoseninstalacionesValue = new Array();
    var TrabajoseninstalacionesTitulos = new Array();

    var LimpiezaValue = new Array();
    var LimpiezaTitulos = new Array();

    var ConsultaInformacionValue = new Array();
    var ConsultaInformacionTitulos = new Array();

    var TiempoDescansoValue = new Array();
    var TiempoDescansoTitulos = new Array();

    var CursosCapacitacionesValue = new Array();
    var CursosCapacitacionesTitulos = new Array();

    var MonitoreoDiarioValue = new Array();
    var MonitoreoDiarioTitulos = new Array();

    var OTSValue = new Array();
    var OTSitulos = new Array();

    var Colroes1 = new Array();
    var Colroes2 = new Array();
    var Colroes3 = new Array();
    var Colroes4 = new Array();
    var Colroes5 = new Array();
    var Colroes6 = new Array();
    var Colroes7 = new Array();


    for (var i = 0; i < dt.length; i++) {

        if (dt[i].categoria == "Trabajos en instalaciones") {
            TrabajoseninstalacionesValue.push(dt[i].horasHombre);
            TrabajoseninstalacionesTitulos.push(dt[i].subCategoria);
            Colroes1.push(GetColor());

        }
        else if (dt[i].categoria == "Limpieza") {
            LimpiezaValue.push(dt[i].horasHombre);
            LimpiezaTitulos.push(dt[i].subCategoria);
            Colroes2.push(GetColor());
        }
        else if (dt[i].categoria == "Consulta de información") {
            ConsultaInformacionValue.push(dt[i].horasHombre);
            ConsultaInformacionTitulos.push(dt[i].subCategoria);
            Colroes3.push(GetColor());
        } else if (dt[i].categoria == "Tiempo de descanso") {
            TiempoDescansoValue.push(dt[i].horasHombre);
            TiempoDescansoTitulos.push(dt[i].subCategoria);
            Colroes4.push(GetColor());
        } else if (dt[i].categoria == "Cursos y Capacitaciones") {
            CursosCapacitacionesValue.push(dt[i].horasHombre);
            CursosCapacitacionesTitulos.push(dt[i].subCategoria);
            Colroes5.push(GetColor());
        } else if (dt[i].categoria == "Trabajos en maquinaria (OT)") {
            OTSValue.push(dt[i].horasHombre);
            OTSitulos.push(dt[i].subCategoria);
            Colroes6.push(GetColor());

        }
        else if (dt[i].categoria == "Monitoreo diario") {
            MonitoreoDiarioValue.push(dt[i].horasHombre);
            MonitoreoDiarioTitulos.push(dt[i].subCategoria);
            Colroes7.push(GetColor());

        }
    }

    var config1 = {
        type: 'pie',
        data: {
            datasets: [{
                data: TrabajoseninstalacionesValue,
                backgroundColor: Colroes1,
                label: 'Dataset 1'
            }],
            labels: TrabajoseninstalacionesTitulos
        },
        options: {
            responsive: true
        },
        title: {
            display: true,
            position: "top",
            text: "",
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
    var config2 = {
        type: 'pie',
        data: {
            datasets: [{
                data: LimpiezaValue,
                backgroundColor: Colroes2,
                label: 'Dataset 1'
            }],
            labels: LimpiezaTitulos
        },
        options: {
            responsive: true
        },
        title: {
            display: true,
            position: "top",
            text: "",
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

    var config3 = {
        type: 'pie',
        data: {
            datasets: [{
                data: ConsultaInformacionValue,
                backgroundColor: Colroes3,
                label: 'Dataset 1'
            }],
            labels: ConsultaInformacionTitulos
        },
        options: {
            responsive: true
        },
        title: {
            display: true,
            position: "top",
            text: "",
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

    var config4 = {
        type: 'pie',
        data: {
            datasets: [{
                data: TiempoDescansoValue,
                backgroundColor: Colroes4,
                label: 'Dataset 1'
            }],
            labels: TiempoDescansoTitulos
        },
        options: {
            responsive: true
        },
        title: {
            display: true,
            position: "top",
            text: "",
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

    var config5 = {
        type: 'pie',
        data: {
            datasets: [{
                data: CursosCapacitacionesValue,
                backgroundColor: Colroes5,
                label: 'Dataset 1'
            }],
            labels: CursosCapacitacionesTitulos
        },
        options: {
            responsive: true
        },
        title: {
            display: true,
            position: "top",
            text: "",
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

    var config6 = {
        type: 'pie',
        data: {
            datasets: [{
                data: OTSValue,
                backgroundColor: Colroes6,
                label: 'Dataset 1'
            }],
            labels: OTSitulos
        },
        options: {
            responsive: true
        },
        title: {
            display: true,
            position: "top",
            text: "",
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
    var config7 = {
        type: 'pie',
        data: {
            datasets: [{
                data: MonitoreoDiarioValue,
                backgroundColor: Colroes7,
                label: 'Dataset 1'
            }],
            labels: MonitoreoDiarioTitulos
        },
        options: {
            responsive: true
        },
        title: {
            display: true,
            position: "top",
            text: "",
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

    var ctx1 = document.getElementById('LineWithLine1').getContext('2d');
    var ctx2 = document.getElementById('LineWithLine2').getContext('2d');
    var ctx3 = document.getElementById('LineWithLine3').getContext('2d');
    var ctx4 = document.getElementById('LineWithLine4').getContext('2d');
    var ctx5 = document.getElementById('LineWithLine5').getContext('2d');
    var ctx = document.getElementById('LineWithLine6').getContext('2d');
    var ctx7 = document.getElementById('LineWithLine15').getContext('2d');


    myChart1 = new Chart(ctx1, config1);
    myChart1.resize();
    myChart2 = new Chart(ctx2, config2);
    myChart2.resize();
    myChart3 = new Chart(ctx3, config3);
    myChart3.resize();
    myChart4 = new Chart(ctx4, config4);
    myChart4.resize();
    myChart5 = new Chart(ctx5, config5);
    myChart5.resize();
    myChart6 = new Chart(ctx, config6);
    myChart6.resize();
    myChart15 = new Chart(ctx7, config6);
    myChart15.resize();

}
/**/

function LoadDetalleUtilizacion(idPuesto) {
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        datatype: "json",
        type: "POST",
        url: '/OT/rptUtilizacionDetallePuesto',
        data: { ccs: getValoresMultiples("#cboCC"), fechaInicio: txtFechaIncioFiltro.val(), fechaFin: txtFechaFinFiltro.val(), puestoID: idPuesto },
        async: false,
        success: function (response) {
            $("#btnRptUtilizacion").attr('data-idPuesto', idPuesto);
            $("#modalDetalleUtilizacion").modal('show');
            loadTableUtilizacion(response.dt);
            loadGraficasUtilizacion();

            $.unblockUI();
        },
        error: function () {
            $.unblockUI();
        }
    });

}


myChart8 = null;
myChart7 = null
function loadGraficasUtilizacion() {

    if (myChart8 != null) {
        myChart8.destroy();

    }
    if (myChart7 != null) {
        myChart7.destroy();

    }
    var listaNombres = new Array();
    var valueRegistro = new Array();
    var valueHorasEfectivas = new Array();


    var dt = tlbUtilizacionDetalleGrid.data();
    var coloresBg = new Array();


    for (var i = 0; i < dt.length; i++) {
        listaNombres.push(dt[i].Nombre);
        valueRegistro.push(dt[i].porRegistro);
        valueHorasEfectivas.push(dt[i].porHorasEfectivas);
        var colorName = GetColor();
        coloresBg.push(colorName);
    }

    var config7 = {
        type: 'pie',
        data: {
            datasets: [{
                data: valueRegistro,
                backgroundColor: coloresBg,
                label: 'Dataset 1'
            }],
            labels: listaNombres
        },
        options: {
            responsive: true
        },
        title: {
            display: true,
            position: 'right',
            text: "",
            fontSize: 18,
            fontColor: "#111"
        },
        legend: {
            display: true,
            position: 'right',
            labels: {
                fontColor: "#000080",
            }
        },

    };
    var ctx7 = document.getElementById('LineWithLine7').getContext('2d');
    myChart7 = new Chart(ctx7, config7);
    myChart7.resize();

    var config8 = {
        type: 'pie',
        data: {
            datasets: [{
                data: valueHorasEfectivas,
                backgroundColor: coloresBg,
                label: 'Dataset 1'
            }],
            labels: listaNombres
        },
        options: {
            responsive: true
        },
        title: {
            display: true,
            position: "top",
            text: "",
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
    var ctx8 = document.getElementById('LineWithLine8').getContext('2d');
    myChart8 = new Chart(ctx8, config8);
    myChart8.resize();

    setLabel();
}

//tlbUtilizacionDetalleGrid = $("#tlbUtilizacionDetalle").DataTable();
function setLabel() {

    Chart.plugins.register({
        afterDatasetsDraw: function (chartInstance, easing) {
            // To only draw at the end of animation, check for easing === 1
            //chartInstance.chart.ctx;
            chartInstance.data.datasets.forEach(function (dataset, i) {
                var meta = chartInstance.getDatasetMeta(i);
                if (!meta.hidden) {

                    switch (meta.controller.chart.chart.canvas.id) {
                        case "LineWithLine8":
                            meta.data.forEach(function (element, index) {
                                // Draw the text in black, with the specified font
                                var ctx6 = document.getElementById('LineWithLine8').getContext('2d');;
                                ctx6.fillStyle = 'black';
                                var fontSize = 10;
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
                                ctx6.fillText(dataString, position.x, position.y - (fontSize / 2) - padding);
                            }); break;
                        case "LineWithLine7":
                            meta.data.forEach(function (element, index) {
                                // Draw the text in black, with the specified font
                                var ctx6 = document.getElementById('LineWithLine7').getContext('2d');;
                                ctx6.fillStyle = 'black';
                                var fontSize = 10;
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
                                ctx6.fillText(dataString, position.x, position.y - (fontSize / 2) - padding);
                            }); break;
                        default:
                            break;
                    }
                }
            });
        }
    });
}


function loadTableUtilizacion(dataSet) {

    if (dataSet != undefined) {

        tlbUtilizacionDetalleGrid = $("#tlbUtilizacionDetalle").DataTable({
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
            responsive: true,
            destroy: true,
            scrollY: '50vh',
            scrollCollapse: true,
            data: dataSet,
            columns: [

                  {
                      data: "Nombre"
                  },
                  {
                      data: "hrsProyectadas"
                  },
                  {
                      data: "hrsRegistradasTrabajo"
                  },
                  {
                      data: "hrsFaltantesRegistro"
                  },
                  {
                      data: "porRegistro"

                  },
                  {
                      data: "porHorasEfectivas"

                  }
            ],
            "paging": false,
            "info": false,
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;

                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                        i : 0;
                };

                // Total over all pages
                horasDisponiblesTotalC = api
                    .column(1)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                horasRegistradasC = api
                    .column(2)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                // Total over all pages
                HorasFaltantesRegistro = api
                    .column(3)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                HorasPorRegistro = api
                    .column(4)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                horasEfectivas = api
                .column(5)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

                horasDisponiblesTotal = api
                .column(1, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

                horasRegistradas = api
                 .column(2, { page: 'current' })
                 .data()
                 .reduce(function (a, b) {
                     return intVal(a) + intVal(b);
                 }, 0);

                horasFaltantes = api
                 .column(3, { page: 'current' })
                 .data()
                 .reduce(function (a, b) {
                     return intVal(a) + intVal(b);
                 }, 0);

                porRegistro = api
                 .column(4, { page: 'current' })
                 .data()
                 .reduce(function (a, b) {
                     return intVal(a) + intVal(b);
                 }, 0);

                porEfectividad = api
                 .column(5, { page: 'current' })
                 .data()
                 .reduce(function (a, b) {
                     return intVal(a) + intVal(b);
                 }, 0);

                // Update footer
                $(api.column(1).footer()).html(
                      horasDisponiblesTotalC.toFixed(2) + ' (' + horasDisponiblesTotal.toFixed(2) + ' total)'
                );
                $(api.column(2).footer()).html(
                    horasRegistradasC.toFixed(2) + ' (' + horasRegistradas.toFixed(2) + ' total)'
               );
                $(api.column(3).footer()).html(
                     HorasFaltantesRegistro.toFixed(2) + ' (' + horasFaltantes.toFixed(2) + ' total)'
                );
                $(api.column(4).footer()).html(
                    HorasPorRegistro.toFixed(2) + ' (' + porRegistro.toFixed(2) + ' total)'
               );
                $(api.column(5).footer()).html(
                    horasEfectivas.toFixed(2) + ' (' + porEfectividad.toFixed(2) + ' total)'
                );

            }

        });
    }
    else {
        tlbUtilizacionDetalleGrid.clear().draw();
    }
}

function aleatorio(inferior, superior) {
    numPosibilidades = superior - inferior
    aleat = Math.random() * numPosibilidades
    aleat = Math.floor(aleat)
    return parseInt(inferior) + aleat
}