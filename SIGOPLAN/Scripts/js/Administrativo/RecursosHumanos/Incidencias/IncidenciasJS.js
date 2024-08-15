$(function () {

    $.namespace('recursoshumanos.Incidencias.CatIncidencias');

    CatIncidencias = function () {


        mensajes = {
            PROCESANDO: 'Procesando...'
        };

        selAnio = $("#selAnio");
        selCC = $("#selCC");
        selInci = $("#selInci");
        btnAplicarFiltros = $("#btnAplicarFiltros");
        selEmpleados = $("#selEmpleados");
        txtClaveEmp = $("#txtClaveEmp");
        selPeriodo = $("#selPeriodo");
        selDia = $("#selDia");
        divGlobal = $("#divGlobal");
        modalReportes = $("#modalReportes");
        btnReport = $("#btnReport");
        ireport = $("#report");
        divGrafica = $("#myChart");
        btnImprimirDet = $("#btnImprimirDet");
        selFecha = $("#selFecha");
        tblDetalles = $("#tblDetalles");

        dtInicio = $("#dtInicio");
        dtFin = $("#dtFin");

        function init() {
            initTblDetalle();

            var hoy = new Date();
            var dd = hoy.getDate();
            var mm = hoy.getMonth() + 1;
            var yyyy = hoy.getFullYear();

            selFecha.datepicker().datepicker("setDate", dd + "/" + mm + "/" + yyyy);
            dtInicio.datepicker().datepicker("setDate", dd + "/" + mm + "/" + yyyy);
            dtFin.datepicker().datepicker("setDate", dd + "/" + mm + "/" + yyyy);


            btnAplicarFiltros.click(Buscar);
            btnReport.click(verReporte);
            btnImprimirDet.click(verReporteDet);
            fillCombos();

            selCC.change(ChangeCC);
            selAnio.change(ChangeAnio);
            selPeriodo.change(ChangePeriodo);
            selEmpleados.change(changeEmpleado);


        }

        init();

        function fillCombos() {
            selAnio.fillCombo('/Administrativo/Incidencias/CatAnios', null);
            selCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#selCC");
            selEmpleados.fillCombo('/Administrativo/ReportesRH/FillComboEmpleado', { cc: getValoresMultiples("#selCC") }, false, "Todos");
            convertToMultiselect("#selEmpleados");
            // selInci.fillCombo('/Administrativo/Incidencias/CatIncidencias', { est: true }, false, "Todos");
            selInci.fillCombo('/Administrativo/ReportesRH/CatIncidencia', {}, false, "Todos");
            convertToMultiselect("#selInci");

            selAnio[0].disabled = true;

        }
        function getInfoEconomico() {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Horometros/getCentroCostos",
                data: { obj: txtCCFiltro.val() },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        txtNombreCC.value = response.centroCostos;

                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }
        function Buscar() {
            btnReport.show();

            var CC = getValoresMultiples("#selCC");

            if (CC.length > 0 || txtClaveEmp.text() !== "") {
                $.blockUI({ message: mensajes.PROCESANDO });

                var objIncidencia = {
                    lstCC: CC,
                    idEmpleado: txtClaveEmp.text(),
                    idIncidencia: selInci.val(),
                    fecha: selFecha.val(),
                    inicio: dtInicio.val(),
                    fin: dtFin.val()
                };

                if (selAnio.val() > 0) {
                    objIncidencia.Anio = selAnio.val();
                }
                if (selPeriodo.val() > 0) {
                    objIncidencia.Periodo = selPeriodo.val();
                    objIncidencia.nomina = $("#selPeriodo option:selected").text().substr(2)
                }
                if (selDia.val() > 0) {
                    objIncidencia.Dia = selDia.val();
                }

                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/Administrativo/Incidencias/Buscar',
                    data: objIncidencia,
                    success: function (response) {

                        var tabla = "<table id='grid-basic'>";
                        tabla += "<thead class='bg-table-header'>"
                        tabla += "<tr>";
                        tabla += "<th data-column-id='CC' class='text-center'><label>CC</label></th>";
                        tabla += "<th data-column-id='Incidencia' class='text-center'><label>Incidencia</label></th>";
                        //tabla += "<th data-column-id='Periodo' class='text-center'><label>Periodo</label></th>";
                        tabla += "<th data-column-id='Total' class='text-center'><label>Total</label></th>";
                        tabla += "<th data-column-id='Detalle' data-formatter='detalles' class='text-center'><label>Detalle</label></th>";
                        tabla += "</tr>";
                        tabla += "</thead>";
                        tabla += "<tbody>";

                        var i = 0;
                        for (i; i <= response.length - 1; i++) {

                            tabla += "<tr>";
                            tabla += "<td class='text-center'>" + response[i].CC + "</td>";
                            tabla += "<td class='text-center'>" + response[i].incidencia + "</td>";
                            //tabla += "<td class='text-center'>" + response[i].periodo + "</td>";
                            tabla += "<td class='text-center'>" + response[i].Total + "</td>";
                            //tabla += "<td class='text-center'><button class='btn btn-info' value=\"" + response[i].CC + "\">Ver</button></td>";


                            tabla += "</tr>";
                        }


                        tabla += "</tbody>";
                        tabla += "</table>";

                        divGlobal.html(tabla);

                        BarChart(response);

                        var grid = $("#grid-basic").bootgrid({
                            headerCssClass: '.bg-table-header',
                            align: 'center',
                            templates: {
                                header: ""
                            },
                            formatters: {
                                "detalles": function (column, row) {
                                    var desabilita = row.Total == 0 ? "disabled" : "";
                                    var btnDetalle = "<button class='btn btn-info verDet' cc=\"" + row.CC + "\" incidencia=\"" + row.Incidencia + "\" " + desabilita + ">Detalle</button>";
                                    return btnDetalle;

                                }
                            }
                        }).on("loaded.rs.jquery.bootgrid", function () {
                            /* Executes after data is loaded and rendered */
                            grid.find(".verDet").on("click", function (e) {

                                //tring strIncid, string cc, int periodo = 0, int Anio = 0, int empleado = 0, int Dia = 0, string nomina = ""

                                var obj = {
                                    cc: this.attributes.cc.textContent.slice(0, 3),
                                    strIncid: this.attributes.incidencia.textContent,
                                    inicio: dtInicio.val(),
                                    fin: dtFin.val()
                                };
                                if (txtClaveEmp.text() !== "") {
                                    obj.empleado = txtClaveEmp.text();
                                }
                                if (selAnio.val() > 0) {
                                    obj.Anio = selAnio.val();
                                }
                                if (selPeriodo.val() > 0) {
                                    obj.Periodo = selPeriodo.val();
                                    obj.nomina = $("#selPeriodo option:selected").text().substr(2)
                                }
                                if (selDia.val() > 0) {
                                    obj.Dia = selDia.val();
                                }

                                loadTabla(obj, '/Administrativo/Incidencias/buscarDetalle/');

                            });
                            grid.find(".verRep").on("click", function (e) {
                                verReporte(e);
                            });
                        });

                        $.unblockUI();
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });



            }
            else {
                AlertaGeneral("Alerta", "Selecciones a lo menos un Centro de costos o Empleado");

            }


        }
        function changeEmpleado() {
            txtClaveEmp.text(this.value);
        }
        function loadTabla(obj, controller) {
            $.blockUI({ message: mensajes.PROCESANDO });

            $.ajax({
                url: controller,
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                success: function (response) {

                    //#region FILL DATATABLE
                    dtDetalle.clear();
                    dtDetalle.rows.add(response);
                    dtDetalle.draw();
                    //#endregion

                    // var tabla = "<table class=' table table-condensed table-hover table-striped text-center' id='tblDetalle'>";
                    // tabla += "<thead class='bg-table-header'>"
                    // tabla += "<tr>";
                    // tabla += "<th data-column-id='clave' class='text-center'><label>#Empleado</label></th>";
                    // tabla += "<th data-column-id='Nombre' class='text-center'><label>Nombre</label></th>";
                    // tabla += "<th data-column-id='cc' class='text-center'><label>Centro de Costos</label></th>";
                    // tabla += "<th data-column-id='anio' class='text-center'><label>Año</label></th>";
                    // tabla += "<th data-column-id='periodo' class='text-center'><label>Periodo</label></th>";
                    // tabla += "<th data-column-id='fecha' class='text-center'><label>Fecha</label></th>";
                    // tabla += "<th data-column-id='incidencia' class='text-center'><label>Incidencia</label></th>";
                    // tabla += "</tr>";
                    // tabla += "</thead>";
                    // tabla += "<tbody>";

                    // var i = 0;
                    // for (i; i <= response.length - 1; i++) {

                    //     tabla += "<tr>";
                    //     tabla += "<td class='text-center'>" + response[i].clave + "</td>";
                    //     tabla += "<td class='text-center'>" + response[i].Nombre + "</td>";
                    //     tabla += "<td class='text-center'>" + response[i].cc + "</td>";
                    //     tabla += "<td class='text-center'>" + response[i].anio + "</td>";
                    //     tabla += "<td class='text-center'>" + response[i].periodo + "</td>";
                    //     tabla += "<td class='text-center'>" + response[i].fecha + "</td>";
                    //     tabla += "<td class='text-center'>" + response[i].incidencia + "</td>";


                    //     tabla += "</tr>";

                    // }


                    // tabla += "</tbody>";
                    // tabla += "</table>";


                    // $("#tablaDetalles").html(tabla);

                    // var grid = $("#tblDetalle").bootgrid({
                    //     headerCssClass: '.bg-table-header',
                    //     align: 'center'
                    // });

                    $("#modalDetalle").modal('show');
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                }
            });
        }
        function ChangeCC() {

            selEmpleados.fillCombo('/Administrativo/ReportesRH/FillComboEmpleado', { cc: getValoresMultiples("#selCC") }, false, "Todos");
            convertToMultiselect("#selEmpleados");

            if (selCC.val().length > 0 || txtClaveEmp.val() !== "") {
                selAnio[0].disabled = false;

                if (selAnio.val().length > 0) {
                    selPeriodo.fillCombo('/Administrativo/Incidencias/CatPeriodos', { anio: selAnio.val(), cc: selCC.val() });
                    $("#selPeriodo option[value='']").prop('selected', true);
                    selPeriodo[0].disabled = false;
                }

            }
            else {
                $("#selAnio option[value='']").prop('selected', true);
                selAnio.trigger('change');
                selAnio[0].disabled = true;
            }



        }
        function ChangeAnio() {

            if (selAnio.val().length <= 0) {
                $("#selPeriodo option[value='']").prop('selected', true);
                selPeriodo.trigger('change')
                selPeriodo[0].disabled = true;
            }
            else {
                selPeriodo.fillCombo('/Administrativo/Incidencias/CatPeriodos', { anio: selAnio.val(), cc: selCC.val() });
                selPeriodo[0].disabled = false
            }
        }
        function ChangePeriodo() {

            if (selPeriodo.val().length <= 0) {
                $("#selDia option[value='']").prop('selected', true);

                selDia[0].disabled = true;
            }
            else {
                selDia.fillCombo('/Administrativo/Incidencias/CatDias', { nomina: $("#selPeriodo option:selected").text().substr(2) });
                selDia[0].disabled = false
            }
        }
        function verReporte(e) {

            $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = "18";

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;


            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();
                openCRModal();
            };
            e.preventDefault();

        }
        function verReporteDet(e) {

            $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = "19";

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;


            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();
                openCRModal();
            };
            e.preventDefault();

        }



        function initTblDetalle() {
            dtDetalle = tblDetalles.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Reporte", "<center><h3>Reporte</h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            // columns: [':visible', 21]
                            columns: [07, 0, 01, 05, 06]
                        }
                    }
                ],
                columns: [
                    { data: 'clave', title: '#Empleado' }, //0
                    { data: 'Nombre', title: 'Nombre' }, //1
                    { data: 'cc', title: 'CC' }, //2
                    { data: 'anio', title: 'Año' }, //3
                    { data: 'periodo', title: 'Periodo' }, //4
                    {
                        data: 'tipo_nomina', title: 'Tipo Nom',
                        render: function (data, type, row, meta) {
                            return data == 1 ? "Semanal" : "Quincenal";
                        },
                    },
                    { data: 'fecha', title: 'Fecha' }, //5
                    { data: 'incidencia', title: 'Incidencia' }, //6
                    {
                        data: 'cc', title: 'CC', visible: false, //7
                        render: function (data, type, row, meta) {
                            return `${data}.`;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        var myChart;
        function BarChart(obj) {

            var countCC = 0;
            var objIncidencias = [];
            var objIncidenciasTemporal = [];
            var indexArreglo = 0;
            var arrayIncidencias = [];
            var arrayCc = [];

            for (x = 0; x < obj.length; x++) {
                if (x !== obj.length - 1) {

                    if (obj[x].CC === obj[x + 1].CC) {
                        objIncidenciasTemporal[indexArreglo] = obj[x].Total;
                        arrayIncidencias[indexArreglo] = obj[x].incidencia;
                        indexArreglo++;
                    }
                    else {
                        objIncidenciasTemporal[indexArreglo] = obj[x].Total;
                        objIncidencias[countCC] = objIncidenciasTemporal;
                        arrayCc[countCC] = obj[x].CC;
                        objIncidenciasTemporal = [];
                        indexArreglo = 0;
                        countCC++;
                    }
                }
                else {
                    objIncidenciasTemporal[indexArreglo] = obj[x].Total;
                    arrayIncidencias[indexArreglo] = obj[x].incidencia;
                    objIncidencias[countCC] = objIncidenciasTemporal;
                    arrayCc[countCC] = obj[x].CC;
                    indexArreglo = 0;
                    countCC++;
                }

            }

            var DataS = []
            for (x = 0; x < objIncidencias.length; x++) {
                DataS[x] = {
                    backgroundColor: 'rgba(255, 130, 35,' + 10 * (x + 1) + ')',
                    hoverBackgroundColor: 'rgba(255, 130, 35,0.5)',
                    borderColor: 'rgba(255,131,15,1)',
                    borderWidth: 1,
                    name: arrayCc[x],
                    data: objIncidencias[x]
                }
            }

            var maximo = Math.max.apply(null, objIncidencias);
            maximo = (maximo * .2) + maximo;

            var barChartData = {
                labels: arrayIncidencias,
                datasets: DataS
            }




            var ctx = divGrafica;


            if (myChart != null) {
                myChart.destroy();
            }

            myChart = new Chart(ctx, {
                type: 'bar',
                data: barChartData,
                options: {
                    onClick: function () {
                        var obj = {
                            cc: this.config.data.datasets[0].name.slice(0, 3),
                            strIncid: this.active[0]._model.label
                        };
                        if (txtClaveEmp.text() !== "") {
                            obj.empleado = txtClaveEmp.text();
                        }
                        if (selAnio.val() > 0) {
                            obj.Anio = selAnio.val();
                        }
                        if (selPeriodo.val() > 0) {
                            obj.Periodo = selPeriodo.val();
                            obj.nomina = $("#selPeriodo option:selected").text().substr(2)
                        }
                        if (selDia.val() > 0) {
                            obj.Dia = selDia.val();
                        }

                        loadTabla(obj, '/Administrativo/Incidencias/buscarDetalle/');
                    },
                    responsive: true,
                    legend: {
                        display: false
                    },
                    hover: {
                        animationDuration: 0
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
                                meta.data.forEach(function (bar, index) {
                                    data = dataset.name.slice(0, 3);
                                    ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                });
                            });
                        }
                    }
                }
            });
        }

    }

    $(document).ready(function () {
        recursoshumanos.Incidencias.CatIncidencias = new CatIncidencias();
    });
});