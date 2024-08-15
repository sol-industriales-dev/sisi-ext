$(function () {

    $.namespace('recursoshumanos.reportesrh.repaltas');

    repaltas = function () {
        mensajes = {
            NOMBRE: 'Reporte Captura Horometro',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },
            cboCC = $("#cboCC"),
            fechaIni = $("#fechaIni"),
            fechaFin = $("#fechaFin"),
            btnBuscar = $("#btnBuscar"),
            btnImprimir = $("#btnImprimir"),
            tblData = $("#tblData");
        let dtData;

        function init() {
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");
            datePicker();
            var now = new Date(),
                year = now.getYear() + 1900;
            fechaIni.datepicker().datepicker("setDate", new Date());
            fechaFin.datepicker().datepicker("setDate", new Date());
            btnBuscar.click(clickBuscar);
            btnImprimir.click(clickImprimir);
            initTblData();
            // tblData.DataTable({
            //     buttons: [
            //         {
            //             extend: 'excelHtml5', footer: true,
            //             exportOptions: {
            //                 // columns: [':visible', 21]
            //                 // columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26]
            //             }
            //         }
            //     ],
            // });
        }

        function initTblData() {
            dtData = tblData.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                "bLengthChange": false,
                "autoWidth": false,
                dom: 'Bfrtip',
                // buttons: parametrosImpresion("Reporte detalle Adeudos", "<center><h3>Reporte Detalle Adeudos </h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            // columns: [':visible', 21]
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]
                        }
                    }
                ],
                columns: [
                    { data: 'cCdes', title: 'CC' },
                    { data: 'empleadoID', title: 'CLAVE EMPLEADO' },
                    { data: 'empleado', title: 'NOMBRE' },
                    { data: 'puesto', title: 'PUESTO' },
                    { data: 'categoria', title: 'CATEGORIA', render: function (data, type, row) { return data ?? "S/N" } },
                    { data: 'tipo_nomina', title: 'TIPO NOMINA' },
                    { data: 'nss', title: 'NSS' },
                    { data: 'jefeInmediato', title: 'RESPONSABLE DE CC' },
                    {
                        data: 'fecha_alta', title: 'FECHA ALTA',
                        render: (data, type, row, meta) => {
                            return moment(data).format("DD/MM/YYYY")
                        }
                    },
                    {
                        data: 'fecha_antiguedad', title: 'ANTIGUEDAD',
                        render: (data, type, row, meta) => {
                            return moment(data).format("DD/MM/YYYY")

                        }
                    },
                    { data: 'nombre_corto', title: 'REGISTRO PATRONAL', visible: false },
                    { data: 'salario_base', title: 'SALARIO BASE', visible: false },
                    { data: 'complemento', title: 'COMPLEMENTO', visible: false },
                    { data: 'bono_zona', title: 'BONO ZONA', visible: false },
                    { data: 'total_mensual', title: 'TOTAL MENSUAL', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblData.on('click', '.classBtn', function () {
                        let rowData = dtData.row($(this).closest('tr')).data();
                    });
                    tblData.on('click', '.classBtn', function () {
                        let rowData = dtData.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function clickBuscar() {
            if (validateBuscar()) {
                filtrarGrid();
            }
        }

        function filtrarGrid() {
            //loadGrid(getFiltrosObject(), '/Administrativo/ReportesRH/FillGridRepAltas', tblData);
            loadGraph(getFiltrosObject(), '/Administrativo/ReportesRH/FillGridRepAltas', "myChart");
            fncGetAltas();
            btnImprimir.show();
        }

        function fncGetAltas() {
            obj = getFiltrosObject()
            axios.post("FillGridRepAltas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtData.clear();
                    dtData.rows.add(response.data.rows);
                    dtData.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function getFiltrosObject() {
            return {
                cc: getValoresMultiples("#cboCC"),
                fechaInicio: fechaIni.val(),
                fechaFin: fechaFin.val()
            }
        }

        function datePicker() {
            var now = new Date(),
                year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
                from = $("#fechaIni")
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
                to = $("#fechaFin").datepicker({
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
        function validateBuscar() {
            var state = true;
            if (!validarCampo(cboCC)) { state = false; }
            if (!validarCampo(fechaIni)) { state = false; }
            if (!validarCampo(fechaFin)) { state = false; }
            if (!state) AlertaGeneral("Alerta", "Seleccione al menos un centro de costo");
            return state;
        }
        function loadGraph(objetoCarga, controller, divChart) {

            $.ajax({
                url: controller,
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(objetoCarga),
                success: function (response) {
                    if (response.success) {
                        var cc = [];
                        var cantidad = [];
                        var Unicos = Enumerable.From(response.rows).Select(function (x) { return x.cCdes }).Distinct().ToArray();
                        $.each(Unicos, function (i, e) {
                            cc.push(e);
                            var cant = Enumerable.From(response.rows).Where(function (x) { return x.cCdes == e }).Count();
                            cantidad.push(cant);
                        });

                        BarChart(cc, cantidad, divChart);
                    }
                    else {

                    }
                },
                error: function (response) {
                    alert(response.message);
                }
            });
        }
        var myChart;
        function BarChart(meses, importes, divChart) {

            var maximo = Math.max.apply(null, importes);
            maximo = (maximo * .2) + maximo;
            var barChartData = {
                labels: meses,
                datasets: [
                    {
                        backgroundColor: 'rgba(255, 130, 35, 1)',
                        hoverBackgroundColor: 'rgba(255, 130, 35,0.5)',
                        borderColor: 'rgba(255,131,15,1)',
                        borderWidth: 1,
                        data: importes
                    }
                ]
            }


            if (myChart != null) {
                myChart.destroy();
            }

            var ctx = document.getElementById(divChart);
            myChart = new Chart(ctx, {
                type: 'bar',
                data: barChartData,
                options: {
                    //onClick: clickHandler,
                    responsive: true,
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                callback: function (value, index, values) {
                                    return value.toFixed(1);
                                },
                                stepSize: Math.trunc(maximo / meses.length)
                            }
                        }],
                        xAxes: [{
                            ticks: {
                                autoSkip: false
                            }
                        }]
                    }
                    ,
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
                                    data = dataset.data[index].toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                    ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                });
                            });
                        }
                    }
                }
            });
            function clickHandler(evt, element) {
                if (element.length) {
                    let data = meses[element[0]._index]
                    if (getIfMeses()) {
                        modalTitle.text("Detalle por Año " + data);
                    }
                    else {
                        modalTitle.text("Detalle por mes " + data);
                    }
                    $("#tituloModalMaquina").text($("#cboFiltroGrupo option:selected").text() + " " + $("#cboFiltroNoEconomico option:selected").text());
                    cargarInicio();
                    loadTabla(getFiltrosObject(data), ruta, gridFiltros, true);
                }
            }

            //inicializarCanvas();
            //addEventListener("resize", inicializarCanvas);
        }
        function clickImprimir(e) {
            verReporte(14, "fechaInicio=" + fechaIni.val() + "&" + "fechaFin=" + fechaFin.val());
            e.preventDefault();
        }
        function verReporte(idReporte, parametros) {

            $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = idReporte;

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&" + parametros;
            ireport = $("#report");
            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();
                openCRModal();

            };
        }

        function initTblAltas() {
            dtAltas = tblAltas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'cCdes', title: 'cCdes' },
                    { data: 'empleadoID', title: 'empleadoID' },
                    { data: 'empleado', title: 'empleado' },
                    { data: 'puesto', title: 'puesto' },
                    { data: 'tipo_nomina', title: 'tipo_nomina' },
                    { data: 'nss', title: 'nss' },
                    { data: 'jefeInmediato', title: 'jefeInmediato' },
                    { data: 'fechaAltaStr', title: 'fechaAltaStr' },
                    { data: 'antiguedad', title: 'antiguedad' },
                ],
                initComplete: function (settings, json) {
                    tblAltas.on('click', '.classBtn', function () {
                        let rowData = dtAltas.row($(this).closest('tr')).data();
                    });
                    tblAltas.on('click', '.classBtn', function () {
                        let rowData = dtAltas.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        init();
    }

    $(document).ready(function () {
        recursoshumanos.reportesrh.repaltas = new repaltas();
    });
});



