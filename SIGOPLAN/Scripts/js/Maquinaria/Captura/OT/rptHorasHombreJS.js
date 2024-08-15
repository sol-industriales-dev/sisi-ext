
(function () {

    $.namespace('maquinaria.OT.rptHorasHombre');

    rptHorasHombre = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Reporte de horas hombre',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        titleModal = $("#titleModal"),
        btnRtpGenerales = $("#btnRtpGenerales"),
        tabCostoTotalXHoraHombre = $("#tabCostoTotalXHoraHombre");
        tabCostoXHora = $("#tabCostoXHora"),
        tabTotalHorasHombre = $("#tabTotalHorasHombre");
        btnReportePersonal = $("#btnReportePersonal"),
        btnReporteOTsPersonal = $("#btnReporteOTsPersonal"),
        report = $("#ireport"),
        btnVerReporte = $("#btnVerReporte"),
        btnBuscarrpt = $("#btnBuscarrpt"),
        modalverRPTHorasHombre = $("#modalverRPTHorasHombre"),
        cboPuestosModal = $("#cboPuestosModal"),
        cboNombrePersonal = $("#cboNombrePersonal"),
        tblDetalleHorasHombreRpt = $("#tblDetalleHorasHombreRpt"),
        btnReporteGeneral = $("#btnReporteGeneral"),
        divPersonalModalDetalle = $("#divPersonalModalDetalle"),
        divOTModalDetalle = $("#divOTModalDetalle"),
        btnRegresar = $("#btnRegresar"),
        titleModalDetalle = $("#titleModalDetalle"),
        cboCC = $("#cboCC"),
        modalverGrafica = $("#modalverGrafica"),
        txtFechaIncioFiltro = $("#txtFechaIncioFiltro"),
        txtFechaFinFiltro = $("#txtFechaFinFiltro"),
        cboMotivoParo = $("#cboMotivoParo"),
        cboTipoParo = $("#cboTipoParo"),
        cboCondicionParo = $("#cboCondicionParo"),
        cboGrupo = $("#cboGrupo"),
        cboNoEconomico = $("#cboNoEconomico"),
        cboEstatusEquipo = $("#cboEstatusEquipo"),
        btnCargar = $("#btnCargar"),
        btnGraficaHorasHombre = $("#btnGraficaHorasHombre"),
        tblHorasHombre = $("#tblHorasHombre"),
        modalDetalleHorasHombre = $("#modalDetalleHorasHombre"),
        cboCondicionParoModal = $("#cboCondicionParoModal"),
        btnReporteMaquinaria = $("#btnReporteMaquinaria"),
        cboMaquinaria = $("#cboMaquinaria"),
        tblDetalleHorasHombre = $("#tblDetalleHorasHombre");
        modalverRPTMaquinaria = $("#modalverRPTMaquinaria");
        cboPuestosModal2 = $("#cboPuestosModal2"),
        btnVerReporte2 = $("#btnVerReporte2"),
        cboNombrePersonal2 = $("#cboNombrePersonal2");

        myChart1 = null;

        function init() {

            btnRtpGenerales.click(displayRptGeneral);
            btnReporteOTsPersonal.click(displayReporteDetalleOT);
            btnReportePersonal.click(verReporteGraficaPersonal);

            cboCC.fillCombo('/CatInventario/FillComboCC', { est: true }, false, "Seleccione");
            cboCC.change(loadEconomicos);
            cboGrupo.fillCombo('/CatMaquina/FillCboGrupos');
            cboGrupo.change(loadEconomicos);

            var now = new Date();

            txtFechaIncioFiltro.datepicker().datepicker("setDate", "01/" + now.getMonth().toString().padStart(2, '0') + "/" + now.getUTCFullYear());
            txtFechaFinFiltro.datepicker().datepicker("setDate", new Date());

            loadEconomicos();
            cboMotivoParo.fillCombo('/OT/cboMotivosParo', null, false);
            cboMotivoParo.change(RevisarFiltro);

            btnCargar.click(loadData);
            btnGraficaHorasHombre.click(VerModal);

            btnRegresar.click(regresar);

            btnReporteGeneral.click(VerModalRPT);
            cboPuestosModal.change(LoadPersonal);

            cboNombrePersonal.fillCombo('/OT/FillCboPersonal', { Puestos: getValoresMultiples("#cboPuestosModal") }, false, "Todos");
            convertToMultiselect("#cboNombrePersonal");

            btnBuscarrpt.click(setReporte);
            btnVerReporte.click(verReporte)

            btnReporteMaquinaria.click(verReporteMaquinaria);

            cboPuestosModal2.fillCombo('/OT/FillCboPuestosRpt', null, false, "Todos");

            convertToMultiselect("#cboPuestosModal2");
            cboPuestosModal2.change(cambioPuestosModal);

            cboNombrePersonal2.fillCombo('/OT/FillCboPersonalPuestosRpt', { Puestos: getValoresMultiples("#cboPuestosModal2"), ccs: getValoresMultiples("#cboCC") }, false, "Todos");
            convertToMultiselect("#cboNombrePersonal2");

            btnVerReporte2.click(verSetReporteMaquinaria);

        }

        function cambioPuestosModal() {
            cboNombrePersonal2.clearCombo();
            cboNombrePersonal2.fillCombo('/OT/FillCboPersonalPuestosRpt', { Puestos: getValoresMultiples("#cboPuestosModal2"), ccs: getValoresMultiples("#cboCC") }, false, "Todos");
            convertToMultiselect("#cboNombrePersonal2");
        }

        function verReporteMaquinaria() {


            modalverRPTMaquinaria.modal('show');
        }



        function verSetReporteMaquinaria() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/OT/SetRptMaquinariaHorasHombre',
                data: { economicos: getValoresMultiples("#cboMaquinaria"), fechaInicio: txtFechaIncioFiltro.val(), fechaFin: txtFechaFinFiltro.val(), Puestos: getValoresMultiples("#cboPuestosModal2"), Empleados: getValoresMultiples("#cboNombrePersonal2"), cc: cboCC.val() },
                async: false,
                success: function (response) {

                    $.blockUI({ message: mensajes.PROCESANDO });
                    var path = "/Reportes/Vista.aspx?idReporte=" + 83 + "&pFechainicio=" + txtFechaIncioFiltro.val() + "&pFechaFin=" + txtFechaFinFiltro.val() + "&pCC=" + cboCC.val();
                    $("#report").attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function displayRptGeneral() {

            var arrayIMG = new Array();

            arrayIMG.push(document.getElementById('LineWithLine2').toDataURL('image/png'));
            arrayIMG.push(document.getElementById('LineWithLine4').toDataURL('image/png'));

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/OT/SetListGraficas',
                data: { obj: arrayIMG },
                async: false,
                success: function (response) {

                    displayReporte2(titleModal.text());
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function verReporteGraficaPersonal() {

            Data = document.getElementById('LineWithLine1').toDataURL('image/png');

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/OT/GetIMG',
                data: { obj: Data },
                async: false,
                success: function (response) {

                    displayReporte(titleModalDetalle.text());
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
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

        function displayReporte(ptituloGrafica) {

            $.blockUI({ message: mensajes.PROCESANDO });
            var path = "/Reportes/Vista.aspx?idReporte=" + 70 + "&pFIncio=" + txtFechaIncioFiltro.val() + "&pFFin=" + txtFechaFinFiltro.val() + "&pMotivoParo=" + SetSelected("cboMotivoParo") + "&pTipoParo=" + SetSelected("cboTipoParo") + "&pCodicionParo=" + SetSelected("cboCondicionParo") + "&pGrupoMaquinaria=" + SetSelected("cboGrupo") + "&pEconomico=" + SetSelected("cboNoEconomico") + "&pCodicionEquipo=" + SetSelected("cboEstatusEquipo") + "&ptituloGrafica=" + ptituloGrafica;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        function displayReporte2(ptituloGrafica) {

            $.blockUI({ message: mensajes.PROCESANDO });
            var path = "/Reportes/Vista.aspx?idReporte=" + 72 + "&pFIncio=" + txtFechaIncioFiltro.val() + "&pFFin=" + txtFechaFinFiltro.val() + "&pMotivoParo=" + SetSelected("cboMotivoParo") + "&pTipoParo=" + SetSelected("cboTipoParo") + "&pCodicionParo=" + SetSelected("cboCondicionParo") + "&pGrupoMaquinaria=" + SetSelected("cboGrupo") + "&pEconomico=" + SetSelected("cboNoEconomico") + "&pCodicionEquipo=" + SetSelected("cboEstatusEquipo") + "&ptituloGrafica=" + ptituloGrafica;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function displayReporteDetalleOT() {

            ptituloGrafica = titleModalDetalle.text();

            $.blockUI({ message: mensajes.PROCESANDO });
            var path = "/Reportes/Vista.aspx?idReporte=" + 71 + "&pFIncio=" + txtFechaIncioFiltro.val() + "&pFFin=" + txtFechaFinFiltro.val() + "&pMotivoParo=" + SetSelected("cboMotivoParo") + "&pTipoParo=" + SetSelected("cboTipoParo") + "&pCodicionParo=" + SetSelected("cboCondicionParo") + "&pGrupoMaquinaria=" + SetSelected("cboGrupo") + "&pEconomico=" + SetSelected("cboNoEconomico") + "&pCodicionEquipo=" + SetSelected("cboEstatusEquipo") + "&ptituloGrafica=" + ptituloGrafica;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function LoadPersonal() {
            cboNombrePersonal.clearCombo();
            cboNombrePersonal.fillCombo('/OT/FillCboPersonal', { Puestos: getValoresMultiples("#cboPuestosModal") }, false, "Todos");
            convertToMultiselect("#cboNombrePersonal");
        }

        function VerModalRPT() {
            modalverRPTHorasHombre.modal('show');
        }

        function verReporte() {

            $.blockUI({ message: mensajes.PROCESANDO });
            var path = "/Reportes/Vista.aspx?idReporte=" + 67 + "&pFIncio=" + txtFechaIncioFiltro.val() + "&pFFin=" + txtFechaFinFiltro.val() + "&pMotivoParo=" + SetSelected("cboMotivoParo") + "&pTipoParo=" + SetSelected("cboTipoParo") + "&pCodicionParo=" + SetSelected("cboCondicionParo") + "&pGrupoMaquinaria=" + SetSelected("cboGrupo") + "&pEconomico=" + SetSelected("cboNoEconomico") + "&pCodicionEquipo=" + SetSelected("cboEstatusEquipo");
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
        function setReporte() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/OT/loadRptHorasHombre',
                data: { listaPuestos: getValoresMultiples("#cboPuestosModal"), listaPersonal: getValoresMultiples("#cboNombrePersonal") },
                async: false,
                success: function (response) {

                    SetDataInTablesDetalleRPT(response.dataset);

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SetDataInTablesDetalleRPT(dataSet) {
            tblDetalleHorasHombreRpt = $("#tblDetalleHorasHombreRpt").DataTable({
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
                "bFilter": true,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [

                      {
                          data: "personalNombre"
                      },
                      {
                          data: "hrasPreventivo"
                      },
                      {
                          data: "hrasPredictivo"
                      },
                       {
                           data: "hrasCorrectivo"
                       },
                      {
                          data: "cantidadOT"

                      }
                      , {
                          data: "promedioHrasOT"

                      }
                ],
                "paging": true,
                "info": false

            });
        }


        function RevisarFiltro() {
            var valorActual = $(this).val();

            cboTipoParo.val('0');
            cboCondicionParo.val('0');
            cboEstatusEquipo.val('0');
            if (valorActual != "") {
                cboTipoParo.prop('disabled', true);
                cboCondicionParo.prop('disabled', true);
                cboEstatusEquipo.prop('disabled', true);


            }
            else {

                cboTipoParo.prop('disabled', false);
                cboCondicionParo.prop('disabled', false);
                cboEstatusEquipo.prop('disabled', false);
            }
        }

        function regresar() {

            divOTModalDetalle.addClass('hide');
            divPersonalModalDetalle.removeClass('hide');

        }

        function remove() {

            var tabTotalHorasHombreE = $("a[href='#tabTotalHorasHombre']")[0];
            // var tabCostoXHoraE = $("a[href='#tabCostoXHora']")[0];
            var tabCostoTotalXHoraHombreE = $("a[href='#tabCostoTotalXHoraHombre']")[0];

            $(tabTotalHorasHombreE).removeClass('active');
            $(tabTotalHorasHombreE).attr('aria-expanded', false);

            //$(tabCostoXHoraE).removeClass('active');
            //$(tabCostoXHoraE).attr('aria-expanded', false);

            $(tabCostoTotalXHoraHombreE).removeClass('active');
            $(tabCostoTotalXHoraHombreE).attr('aria-expanded', false);


            tabCostoXHora.removeClass('fade in').removeClass('active');
            tabCostoTotalXHoraHombre.removeClass('fade in').removeClass('active');

            tabTotalHorasHombre.addClass('fade in').addClass('active');
        }

        $('#modalverGrafica').on('shown.bs.modal', function () {
            verGrafica();
            // setTimeout(remove(),4000);
        });

        function VerModal() {
            modalverGrafica.modal('show');
        }

        function verGrafica() {

            $("#titleModal").text("Periodo del " + txtFechaIncioFiltro.val() + " AL " + txtFechaFinFiltro.val());

            if (myChart1 != null) {
                myChart1.destroy();
            }
            var dt = tblHorasHombreGrid.data();

            var dtPuesto = new Array();
            var dtTotalHH = new Array();
            var dtCostoHH = new Array();
            var dtTotalHorasHombre = new Array();

            for (var i = 0; i < dt.length ; i++) {
                dtPuesto.push(dt[i].Puesto);
                dtTotalHH.push(dt[i].TotalHorasHombre);
                dtCostoHH.push(dt[i].CostoHH);
                dtTotalHorasHombre.push(dt[i].CostoTotal);

            }

            var ctx = document.getElementById("LineWithLine2").getContext("2d");

            var data = {
                labels: dtPuesto,
                datasets: [{
                    label: "Total Horas Hombre",
                    backgroundColor: "blue",
                    data: dtTotalHH.sort(function (a, b) { return b - a })
                }]
            };

            myChart1 = new Chart(ctx, {
                type: 'bar',
                data: data,
                options: {
                    barValueSpacing: 20,
                    scales: {
                        yAxes: [{
                            ticks: {
                                min: 0,
                            }
                        }]
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;


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
            });
            var data3 = {
                labels: dtPuesto,
                datasets: [{
                    label: "Costo total X Hora Hombre",
                    backgroundColor: "green",
                    data: dtTotalHorasHombre.sort(function (a, b) { return b - a })
                }]
            };
            var ctx3 = document.getElementById("LineWithLine4").getContext("2d");

            myChart3 = new Chart(ctx3, {
                type: 'bar',
                data: data3,
                options: {
                    barValueSpacing: 20,
                    scales: {
                        yAxes: [{
                            ticks: {
                                min: 0,
                            }
                        }],

                    },

                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;
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
            });
        }

        function loadData() {
            var datos = getValoresMultiples("#cboCC");
            if (datos.length > 0) {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/OT/loadTablaHorasHombre',
                    data: { obj: getFiltrosObject() },
                    async: false,
                    success: function (response) {

                        SetDataInTables(response.dataset);

                        cboPuestosModal.fillCombo('/OT/FillCboPuesto', null, false, "Todos");
                        convertToMultiselect("#cboPuestosModal");

                        $.unblockUI();
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            else {
                AlertaGeneral('Alerta', 'Debe seleccionar al menos un centro de costos para poder continuar.')
            }
        }

        function SetDataInTables(dataSet) {
            tblHorasHombreGrid = $("#tblHorasHombre").DataTable({
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
                "bFilter": true,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [

                      {
                          data: "No"
                      },
                      {
                          data: "Puesto"
                      },
                      {
                          data: "TotalHorasHombre"
                      },
                       {
                           data: "CostoHH"
                       },
                      {
                          data: "CostoTotal"

                      }
                      , {
                          data: "btn",
                          createdCell: function (td, cellData, rowData, row, col) {
                              $(td).text('');
                              $(td).append('<button type="button" class="btn btn-default btn-block btn-sm" onclick="verDetalle(' + rowData.PuestoID + ',\'' + rowData.Puesto + '\')" >Detalle</button>');

                          }
                      }
                ],
                "paging": true,
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
                        .column(4)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    HorasTotales = api
                        .column(2)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    HorasTotalespageTotal = api
                    .column(2, { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                    // Total over this page
                    CostoTotalHHpageTotal = api
                        .column(4, { page: 'current' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Update footer
                    $(api.column(4).footer()).html(
                         '$' + CostoTotalHHpageTotal.toFixed(2) + ' ( $' + CostoTotalHH.toFixed(2) + ' total)'
                    );
                    $(api.column(2).footer()).html(
                        HorasTotalespageTotal.toFixed(2) + ' (' + HorasTotales.toFixed(2) + ' total)'
                   );
                }

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
                CondicionParo: cboEstatusEquipo.val()
            }
        }

        function loadEconomicos() {
            cboNoEconomico.fillCombo('/CatInventario/FillCboEconomicos', { ccs: getValoresMultiples("#cboCC"), grupo: cboGrupo.val() != "" ? cboGrupo.val() : 0 });


            cboMaquinaria.fillCombo('/CatInventario/FillCboEconomicos', { ccs: getValoresMultiples("#cboCC"), grupo: cboGrupo.val() != "" ? cboGrupo.val() : 0 }, false, "Todos");
            convertToMultiselect("#cboMaquinaria");


        }

        function datePicker() {
            var now = new Date(),
            year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
              from = $("#txtFechaIncioFiltro")
                .datepicker({
                    changeMonth: true,
                    changeYear: true,
                    numberOfMonths: 1,
                    defaultDate: new Date(year, 00, 01),
                    maxDate: new Date(year, 11, 31),

                    onChangeMonthYear: function (y, m, i) {
                        var d = i.selectedDay;
                        $(this).datepicker('setDate', new Date(y, m - 1, d));
                        $(this).trigger('change');
                    }

                })
                .on("change", function () {
                    to.datepicker("option", "minDate", getDate(this));
                }),
              to = $("#txtFechaFinFiltro").datepicker({
                  changeMonth: true,
                  changeYear: true,
                  numberOfMonths: 1,
                  defaultDate: new Date(),
                  maxDate: new Date(year, 11, 31),
                  onChangeMonthYear: function (y, m, i) {
                      var d = i.selectedDay;
                      $(this).datepicker('setDate', new Date(y, m - 1, d));
                      $(this).trigger('change');
                  }
              })
              .on("change", function () {
                  from.datepicker("option", "maxDate", getDate(this));
              });
            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }
                return date;
            }
        }

        init();
    };

    $(document).ready(function () {

        maquinaria.OT.rptHorasHombre = new rptHorasHombre();
    });
})();

myChart2 = null;

function verDetalle(PuestoID, descripcion) {
    $("#divOTModalDetalle").addClass('hide');
    $("#divPersonalModalDetalle").removeClass('hide');

    $("#modalDetalleHorasHombre").modal('show');
    $("#titleModalDetalle").text(descripcion + "- PERIODO " + txtFechaIncioFiltro.val() + " AL " + txtFechaFinFiltro.val());
    $("#modalDetalleHorasHombre").block({ message: null });
    $('#modalDetalleHorasHombre').on('shown.bs.modal', function () {
        loadDataDetalle(PuestoID);
        $("#modalDetalleHorasHombre").unblock();
    });
}

function loadDataDetalle(puestoID) {

    $.ajax({
        datatype: "json",
        type: "POST",
        url: '/OT/loadTablaHorasHombreDetalle',
        data: { obj: getFiltrosObjectDetalle(puestoID) },
        async: false,
        success: function (response) {

            SetDataInTablesDetalle(response.dataset);
            setGrafica(response.dataset);

            $.unblockUI();
        },
        error: function () {
            $.unblockUI();
        }
    });
}

function setGrafica(dt) {

    if (myChart2 != null) {
        myChart2.destroy();
    }

    var dtPersonal = new Array();
    var dtPredictivo = new Array();
    var dtCorrectivo = new Array();
    var dtPreventivo = new Array();
    for (var i = 0; i < dt.length ; i++) {
        dtPersonal.push(dt[i].personalNombre);
        dtCorrectivo.push(dt[i].hrasCorrectivo);
        dtPredictivo.push(dt[i].hrasPredictivo);
        dtPreventivo.push(dt[i].hrasPreventivo);

    }

    var ctx = document.getElementById("LineWithLine1").getContext("2d");

    var data = {
        labels: dtPersonal,
        datasets: [{
            label: "Predictivo",
            backgroundColor: "blue",
            data: dtPredictivo
        }, {
            label: "Correctivo",
            backgroundColor: "red",
            data: dtCorrectivo
        }, {
            label: "Preventivo",
            backgroundColor: "green",
            data: dtPreventivo
        }]
    };

    myChart2 = new Chart(ctx, {
        type: 'bar',
        data: data,
        options: {
            barValueSpacing: 20,
            scales: {
                yAxes: [{
                    ticks: {
                        min: 0,
                    }
                }]
            }
        }
    });
}

function setGraficaCostoTo(dt) {

    if (myChart2 != null) {
        myChart2.destroy();
    }

    var dtPersonal = new Array();
    var dtPredictivo = new Array();
    var dtCorrectivo = new Array();
    var dtPreventivo = new Array();
    for (var i = 0; i < dt.length ; i++) {
        dtPersonal.push(dt[i].personalNombre);
        dtCorrectivo.push(dt[i].hrasCorrectivo);
        dtPredictivo.push(dt[i].hrasPredictivo);
        dtPreventivo.push(dt[i].hrasPreventivo);

    }

    var ctx = document.getElementById("LineWithLine1").getContext("2d");

    var data = {
        labels: dtPersonal,
        datasets: [{
            label: "Predictivo",
            backgroundColor: "blue",
            data: dtPredictivo
        }, {
            label: "Correctivo",
            backgroundColor: "red",
            data: dtCorrectivo
        }, {
            label: "Preventivo",
            backgroundColor: "green",
            data: dtPreventivo
        }]
    };

    myChart2 = new Chart(ctx, {
        type: 'bar',
        data: data,
        options: {
            barValueSpacing: 20,
            scales: {
                yAxes: [{
                    ticks: {
                        min: 0,
                    }
                }]
            }
        }
    });
}

function setGraficaTotalHorasHombre(dt) {

    if (myChart2 != null) {
        myChart2.destroy();
    }

    var dtPersonal = new Array();
    var dtPredictivo = new Array();
    var dtCorrectivo = new Array();
    var dtPreventivo = new Array();
    for (var i = 0; i < dt.length ; i++) {
        dtPersonal.push(dt[i].personalNombre);
        dtCorrectivo.push(dt[i].hrasCorrectivo);
        dtPredictivo.push(dt[i].hrasPredictivo);
        dtPreventivo.push(dt[i].hrasPreventivo);

    }

    var ctx = document.getElementById("LineWithLine1").getContext("2d");

    var data = {
        labels: dtPersonal,
        datasets: [{
            label: "Predictivo",
            backgroundColor: "blue",
            data: dtPredictivo
        }, {
            label: "Correctivo",
            backgroundColor: "red",
            data: dtCorrectivo
        }, {
            label: "Preventivo",
            backgroundColor: "green",
            data: dtPreventivo
        }]
    };

    myChart2 = new Chart(ctx, {
        type: 'bar',
        data: data,
        options: {
            barValueSpacing: 20,
            scales: {
                yAxes: [{
                    ticks: {
                        min: 0,
                    }
                }]
            }
        }
    });
}

function setGraficaCostoxHora(dt) {

    if (myChart2 != null) {
        myChart2.destroy();
    }

    var dtPersonal = new Array();
    var dtPredictivo = new Array();
    var dtCorrectivo = new Array();
    var dtPreventivo = new Array();
    for (var i = 0; i < dt.length ; i++) {
        dtPersonal.push(dt[i].personalNombre);
        dtCorrectivo.push(dt[i].hrasCorrectivo);
        dtPredictivo.push(dt[i].hrasPredictivo);
        dtPreventivo.push(dt[i].hrasPreventivo);

    }

    var ctx = document.getElementById("LineWithLine1").getContext("2d");

    var data = {
        labels: dtPersonal,
        datasets: [{
            label: "Predictivo",
            backgroundColor: "blue",
            data: dtPredictivo
        }, {
            label: "Correctivo",
            backgroundColor: "red",
            data: dtCorrectivo
        }, {
            label: "Preventivo",
            backgroundColor: "green",
            data: dtPreventivo
        }]
    };

    myChart2 = new Chart(ctx, {
        type: 'bar',
        data: data,
        options: {
            barValueSpacing: 20,
            scales: {
                yAxes: [{
                    ticks: {
                        min: 0,
                    }
                }]
            }
        }
    });
}

function SetDataInTablesDetalle(dataSet) {
    tblDetalleHorasHombre = $("#tblDetalleHorasHombre").DataTable({
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
        "bFilter": true,
        destroy: true,
        scrollY: '50vh',
        scrollCollapse: true,
        data: dataSet,
        columns: [

              {
                  data: "personalNombre"
              },
              {
                  data: "hrasPreventivo"
              },
              {
                  data: "hrasPredictivo"
              },
               {
                   data: "hrasCorrectivo"
               },
              {
                  data: "cantidadOT"

              }
              , {
                  data: "promedioHrasOT"

              },
              {
                  data: "btnDetalle",
                  createdCell: function (td, cellData, rowData, row, col) {
                      $(td).text('');
                      $(td).append('<button type="button" class="btn btn-default btn-block btn-sm" onclick="verOTEmpleado(' + rowData.personalID + ',\'' + rowData.personalNombre + '\')" >Detalle</button>');
                  }
              },
        ],
        "paging": true,
        "info": false

    });
}

function verOTEmpleado(personalID, nombrePersona) {

    $.ajax({
        datatype: "json",
        type: "POST",
        url: '/OT/GetOTEmpleado',
        data: { EmpleadoID: personalID, FechaInicio: txtFechaIncioFiltro.val(), FechaFin: txtFechaFinFiltro.val() },
        async: false,
        success: function (response) {
            $("#divPersonalModalDetalle").addClass('hide');
            $("#divOTModalDetalle").removeClass('hide');
            setTablaOT(response.dataResult);

            $("#txtOTDetalle").text(nombrePersona);

            $.unblockUI();
        },
        error: function () {
            $.unblockUI();
        }
    });
}

function setTablaOT(dataSet) {
    tblOTsPersonal = $("#tblOTsPersonal").DataTable({
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
        "bFilter": true,
        destroy: true,
        scrollY: '50vh',
        scrollCollapse: true,
        data: dataSet,
        columns: [

              {
                  data: "folio"
              },
              {
                  data: "economico"
              },
              {
                  data: "motivoParo"
              },
              {
                  data: "inicioParo"
              },
              {
                  data: "finParo"
              }
        ],
        "paging": true,
        "info": false

    });
}
function getFiltrosObjectDetalle(puestoID) {
    return {

        CC: getValoresMultiples("#cboCC"),
        FechaInicio: $("#txtFechaIncioFiltro").val(),
        FechaFin: $("#txtFechaFinFiltro").val(),
        grupo: $("#cboGrupo").val() == "" ? 0 : $("#cboGrupo").val(),
        economico: $("#cboNoEconomico").val() == "" ? 0 : $("#cboNoEconomico").val(),
        tipoParo: $("#cboMotivoParo").val() == "" ? 0 : $("#cboMotivoParo").val(),
        empleadoID: $("#tbNombreEmpleado").val(),
        puestoID: puestoID
    }
}
