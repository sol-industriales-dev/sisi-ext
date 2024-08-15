$(function () {

    $.namespace('recursoshumanos.reportesrh.repbajas');

    repbajas = function () {
        mensajes = {
            NOMBRE: 'Reporte Captura Horometro',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },
            cboConcepto = $("#cboConcepto"),
            cboEstatus = $('#cboEstatus'),
            cboCC = $("#cboCC"),
            fechaIni = $("#fechaIni"),
            fechaFin = $("#fechaFin"),
            chkTipo = $("#chkTipo"),
            btnBuscar = $("#btnBuscar"),
            btnImprimir = $("#btnImprimir"),
            btnExportar = $("#btnExportar"),
            btnSeleccionar = $("#btnSeleccionar"),
            tblData = $("#tblData");
        selected = false;
        const fechaFiltroContaInicio = $('#fechaFiltroContaInicio');
        const fechaFiltroContaFin = $('#fechaFiltroContaFin');
        const cboTipoBajas = $('#cboTipoBajas');

        const tblBajas = $('#tblBajas');
        let dtBajas;

        function init() {
            cboConcepto.fillCombo('/Administrativo/ReportesRH/FillComboConceptosBaja', { est: true }, false, "Todos");
            convertToMultiselect("#cboConcepto");
            convertToMultiselect("#cboEstatus");
            cboCC.fillCombo('/Administrativo/ReportesRH/getListaCCRHBajas', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");
            datePicker();
            initTblBajas();

            var now = new Date(),
                year = now.getYear() + 1900;
            fechaIni.datepicker().datepicker("setDate", new Date());
            fechaFin.datepicker().datepicker("setDate", new Date());
            fechaFiltroContaInicio.datepicker().datepicker();
            fechaFiltroContaFin.datepicker().datepicker();
            btnBuscar.click(clickBuscar);
            btnImprimir.click(clickImprimir);
            btnExportar.click(exportData);
            btnSeleccionar.click(selectAll);


        }
        function clickBuscar() {
            if (validateBuscar()) {
                filtrarGrid();
            }
        }
        function filtrarGrid() {
            // loadGrid(getFiltrosObject(), '/Administrativo/ReportesRH/FillGridRepBajas', tblData);
            fncGetListaBajas();
            loadGraph(getFiltrosObject(), '/Administrativo/ReportesRH/FillGridRepBajas', "myChart");
            btnImprimir.show();
        }
        function getFiltrosObject() {
            return {
                cc: getValoresMultiples("#cboCC"),
                concepto: getValoresMultiples("#cboConcepto"),
                fechaInicio: fechaIni.val(),
                fechaFin: fechaFin.val(),
                tipo: chkTipo.is(":checked"),
                estatus: cboEstatus.val(),
                fechaContaInicio: fechaFiltroContaInicio.val(),
                fechaContaFin: fechaFiltroContaFin.val(),
                tipoBajas: cboTipoBajas.val(),
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
            if (!validarCampo(cboConcepto)) { state = false; }
            if (!validarCampo(cboCC)) { state = false; }
            if (!validarCampo(fechaIni)) { state = false; }
            if (!validarCampo(fechaFin)) { state = false; }
            if (!state) AlertaGeneral("Alerta", "Seleccione al menos unos centro de costo y conceptos");
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
                        var cantidades = [];
                        var conceptos = [];
                        var conceptosUnicos = Enumerable.From(response.rows).Select(function (x) { return x.concepto }).Distinct().ToArray();
                        var Unicos = Enumerable.From(response.rows).Select(function (x) { return x.cCSolo }).Distinct().ToArray();

                        $.each(Unicos, function (i, a) {
                            conceptos.push(a);
                            cantidad = [];
                            $.each(conceptosUnicos, function (i, e) {
                                var cant = Enumerable.From(response.rows).Where(function (x) { return x.cCSolo == a && x.concepto == e }).Count();
                                cantidad.push(cant);
                            });
                            cantidades.push(cantidad);

                        });

                        BarChart(Unicos, conceptosUnicos, cantidades, divChart);
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
        function BarChart(cc, conceptos, cantidades, divChart) {


            var DataS = []

            for (x = 0; x < cantidades.length; x++) {
                DataS[x] = {
                    backgroundColor: 'rgba(255, 130, 35,' + 10 * (x + 1) + ')',
                    hoverBackgroundColor: 'rgba(255, 130, 35,0.5)',
                    borderColor: 'rgba(255,131,15,1)',
                    borderWidth: 1,
                    name: cc[x],
                    data: cantidades[x]
                }
            }

            var maximo = Math.max.apply(null, conceptos);
            maximo = (maximo * .2) + maximo;

            var barChartData = {
                labels: conceptos,
                datasets: DataS
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
                                    data = dataset.name;
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
            verReporte(15, "fechaInicio=" + fechaIni.val() + "&" + "fechaFin=" + fechaFin.val());
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


        function selectAll() {

            if (!selected) {
                selected = true;
                $("input:checkbox[name=cckEmpleados]").prop('checked', true);
            }
            else {
                selected = false;
                $("input:checkbox[name=cckEmpleados]").prop('checked', false);
            }

        }
        function exportData() {

            var array = [];
            $("input:checkbox[name=cckEmpleados]:checked").each(function () {
                array.push($(this).attr('data-cveEmpleado'));
            });

            if (array.length > 0) {
                getFileDownload(array);
            }
            else {
                AlertaGeneral("Alerta", "Debe seleccionar almenos un registro para exportar!");
            }
        }

        function getFileDownload(array) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Administrativo/LayoutRH/ExportInformacion',
                type: "POST",
                datatype: "json",
                data: { empleados: array },
                success: function (response) {
                    $.unblockUI();
                    window.location = '/Administrativo/LayoutRH/getFileDownloadBajas';
                    filtrarGrid();
                    AlertaGeneral("Confirmación", "¡Archivo Generado Correctamente!");

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }


        function initGrid() {

            tblData.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                formatters: {
                    "seleccion": function (column, row) {
                        return "<input type='checkbox'  class='form-control' name='cckEmpleados' data-cveEmpleado='" + row.empleadoID + "'>";
                    }

                }
            });
        }

        function initTblBajas() {
            dtBajas = tblBajas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: true,
                dom: 'Bfrtip',
                columns: [
                    //render: function (data, type, row) { }
                    { data: "cCSolo", title: "", visible: false },
                    { data: "cC", title: "CC", },
                    { data: "empleadoID", title: "NO. EMPLEADO", },
                    {
                        data: "empleado", title: "NOMBRE",
                        render: function (data, type, row) {
                            return data.replaceAll("/", " ");
                        }
                    },
                    { data: "puestosDes", title: "PUESTO", },
                    { data: 'categoria', title: 'CATEGORIA', render: function (data, type, row) { return data ?? "S/N" } },
                    { data: "concepto", title: "RAZON", },
                    { data: "fechaAltaBaja", title: "FECHA ALTA", },
                    { data: "fechaBajaStr", title: "FECHA BAJA", },
                    { data: "recontratable", title: "RECONTRATABLE", },
                    { data: "estatus_bajaDesc", title: "ESTATUS", },
                    { data: "regPatronal", title: "REG. PATRONAL", },
                    { data: "est_inventario_comentario", title: "ALMACEN", },
                    { data: "est_compras_comentario", title: "TALLER", },
                    { data: "est_contabilidad_comentario", title: "CONTABILIDAD", },
                    { data: "est_nominas_comentario", title: "NOMINAS", },
                    {
                        data: "fechaLiberacion", title: "FECHA ULTIMA LIB.",
                        render: function (data, type, row) {
                            return data != null ? moment(data).format("DD/MM/YYYY") : "";
                        }
                    },
                    {
                        data: "comentarios", title: "COMENTARIOS",
                        render: function (data, type, row) {
                            return data ?? "";
                        }
                    },

                    { data: "jefeInmediatoID", title: "", visible: false },
                    { data: "jefeInmediato", title: "", visible: false },
                    { data: "fechaBaja", title: "", visible: false },
                    { data: "FechaRec", title: "", visible: false },
                    { data: "fechaAltaStr", title: "", visible: false },
                    { data: "nss", title: "", visible: false },
                    { data: "puestoID", title: "", visible: false },
                    { data: "est_baja", title: "", visible: false },
                    { data: "est_inventario", title: "", visible: false },
                    { data: "est_contabilidad", title: "", visible: false },
                    { data: "est_compras", title: "", visible: false },
                    { data: "estatus_baja", title: "", visible: false },
                ],
                initComplete: function (settings, json) {
                    tblBajas.on('click', '.classBtn', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                    });
                    tblBajas.on('click', '.classBtn', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17]
                            // columns: [1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61]
                        }

                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetListaBajas() {
            let obj = getFiltrosObject();
            axios.post("/Administrativo/ReportesRH/FillGridRepBajas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtBajas.clear();
                    dtBajas.rows.add(response.data.rows);
                    dtBajas.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        initGrid();
        init();
    }

    $(document).ready(function () {
        recursoshumanos.reportesrh.repbajas = new repbajas();
    });
});