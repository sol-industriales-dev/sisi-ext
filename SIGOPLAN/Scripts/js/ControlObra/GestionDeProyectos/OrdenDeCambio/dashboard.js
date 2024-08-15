(() => {
    $.namespace('ControlObra.Dashboard');
    Dashboard = function () {

        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const selectContrato = $('#selectContrato');
        const txtContratista = $('#txtContratista');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const botonBuscar = $('#botonBuscar');
        const inputMontoTotalOrdenCambio = $('#inputMontoTotalOrdenCambio');
        const graficaMontos = $('#graficaMontos');
        const tablaOrdenCambio = $('#tablaOrdenCambio');
        const divDashboard = $('#divDashboard');
        //#endregion

        let dtOrdenCambio;

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);
        //#endregion

        (function init() {
            $('.select2').select2();
            selectCentroCosto.fillCombo('getProyecto', null, false, null);
            // selectContrato.fillCombo('obtenerContratos', null, false, null);
            inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioAnio);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);

            initTablaOrdenCambio();

            botonBuscar.click(cargarDashboardOrdenCambio);

            selectCentroCosto.on("change", function () {
                selectContrato.fillCombo("obtenerContratosByCC", { cc: $(this).val() }, false, null);
            });

            selectContrato.on("change", function () {
                // selectContratista.fillCombo("fillComboContratistasByContrato", { idContrato: $(this).val() }, false, null);
                let option = $(this).find(`option[value="${$(this).val()}"]`);
                let prefijo = option.attr("data-prefijo");
                txtContratista.val(prefijo);
            });

        })();

        function initTablaOrdenCambio() {
            dtOrdenCambio = tablaOrdenCambio.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    { data: 'numeroContrato', title: 'Contrato' },
                    { data: 'NoOrden', title: 'Orden de Cambio' },
                    { data: 'fechaEfectiva', title: 'Fecha' },
                    {
                        data: 'montoTotalOrdenCambio', title: 'Monto', render: function (data, type, row, meta) {
                            return maskNumero2DCompras(data);
                        }
                    }
                ],
                initComplete: function (settings, json) {

                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function cargarDashboardOrdenCambio() {
            axios.post('GetDashboardOrdenCambio', { cc: selectCentroCosto.val(), contrato_id: +selectContrato.val(), fechaInicio: inputFechaInicio.val(), fechaFin: inputFechaFin.val() })
                .then(response => {
                    let { success, data, message } = response.data;
                    if (success) {
                        inputMontoTotalOrdenCambio.val(maskNumero2DCompras(response.data.montoTotalOrdenCambio))
                        initGraficaMontos(response.data.graficaOrdenCambio);
                        AddRows(tablaOrdenCambio, response.data.tablaContratos);

                        divDashboard.css('display', 'block');
                        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initGraficaMontos(datos) {
            Highcharts.chart('graficaMontos', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: '' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
                    allowDecimals: false,
                    labels: {
                        formatter: function () {
                            return maskNumero2DCompras(this.value);
                        }
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + ': ' + maskNumero2DCompras(this.y) + '</b>';
                    }
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    },
                    series: {
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                return maskNumero2DCompras(this.point.y);
                            }
                        },
                        colorByPoint: true,
                        colors: ['rgb(248, 132, 53)', 'rgb(2, 185, 85)']
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1 }
                ],
                credits: { enabled: false },
                legend: { enabled: false }
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => ControlObra.Dashboard = new Dashboard())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();