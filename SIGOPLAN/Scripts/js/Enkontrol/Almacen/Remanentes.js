(() => {
    $.namespace('Almacen.Remanentes');
    Remanentes = function () {
        //#region Selectores
        const selectAlmacen = $('#selectAlmacen');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const selectSolicitante = $('#selectSolicitante');
        const botonBuscar = $('#botonBuscar');
        const tablaDatos = $('#tablaDatos');
        const checkboxTodosAlmacenes = $('#checkboxTodosAlmacenes');
        const tablaResumen = $('#tablaResumen');
        const tituloImporteTotal = $('#tituloImporteTotal');
        //#endregion

        let dtDatos;
        let dtResumen;

        let _flagPuedeEliminar = false;

        (function init() {
            $.fn.dataTable.moment('DD/MM/YYYY');
            $('.select2').select2();

            inputFechaInicio.datepicker({ dateFormat: 'dd/mm/yy', showAnim: 'slide' }).datepicker("setDate", new Date(new Date().getFullYear(), 0, 1));
            inputFechaFin.datepicker({ dateFormat: 'dd/mm/yy', showAnim: 'slide' }).datepicker("setDate", new Date());

            selectAlmacen.fillCombo('/Enkontrol/Almacen/FillComboAlmacenesFisicos', false, null);
            selectAlmacen.find('option[value=""]').remove();
            selectSolicitante.fillCombo('/Enkontrol/Requisicion/FillComboRequisitores', false, null);

            initTablaDatos();
            initTablaResumen();

            botonBuscar.click(cargarRemanentes);
        })();

        checkboxTodosAlmacenes.on('click', function () {
            if (checkboxTodosAlmacenes.is(':checked')) {
                selectAlmacen.find('option').prop("selected", true);
                selectAlmacen.trigger("change");
            } else {
                selectAlmacen.find('option').prop("selected", false);
                selectAlmacen.trigger("change");
            }
        });

        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function initTablaDatos() {
            dtDatos = tablaDatos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                // ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                scrollY: '45vh',
                scrollX: 'auto',
                scrollCollapse: true,
                dom: 'Bfrtip',
                buttons: [{
                    extend: 'excelHtml5',
                    exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22] },
                    className: 'btn btn-xs btn-success',
                    title: '',
                    customize: function (xlsx) {
                        $('sheets sheet', xlsx.xl['workbook.xml']).attr('name', 'Remanentes');
                    }
                }],
                columns: [
                    { data: 'cantidadConsumo', title: 'Cantidad Consumo' },
                    { data: 'cantidadTraspaso', title: 'Cantidad Traspaso' },
                    { data: 'cantidadSalidas', title: 'Cantidad Salidas' },
                    {
                        data: 'eficiencia', title: 'Eficiencia', render: function (data, type, row, meta) {
                            return data + '%';
                        }
                    },
                    { data: 'inventarioActual', title: 'Inv. Actual' },
                    {
                        data: 'almacen', title: 'No. Almacén', render: function (data, type, row, meta) {
                            return data > 0 ? data : '';
                        }
                    },
                    {
                        data: 'numeroMovimiento', title: 'Número', render: function (data, type, row, meta) {
                            return data > 0 ? data : '';
                        }
                    },
                    { data: 'cc', title: 'CC' },
                    { data: 'fechaCompraString', title: 'Fecha Compra' },
                    { data: 'insumo', title: 'No. Insumo' },
                    { data: 'cantidadEntrada', title: 'Cantidad Comprada' },
                    {
                        data: 'precio', title: 'Precio', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data.toString().replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'importe', title: 'Importe Comprado', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data.toString().replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    { data: 'ccDesc', title: 'Descripción' },
                    { data: 'almacenDesc', title: 'Descripción Almacén' },
                    { data: 'orden_ct', title: 'orden_ct' },
                    { data: 'insumoDesc', title: 'Descripción Insumo' },
                    { data: 'in_out', title: 'In - Out' },
                    {
                        data: 'diferenciaImporte', title: 'Dif. Importe', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data.toString().replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    { data: 'solicitanteNombre', title: 'Nombre Solicitante' },
                    { data: 'autorizaRequisicionNombre', title: 'Autoriza Requisición' },
                    { data: 'voboCompraNombre', title: 'Aut VoBo OC' },
                    { data: 'autorizaCompraNombre', title: 'Aut OC' },
                    {
                        title: '', render: function (data, type, row, meta) {
                            return _flagPuedeEliminar ? `<button class="btn btn-xs btn-danger botonEliminar"><i class="fa fa-times"></i></button>` : ``;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaDatos.on('click', '.botonEliminar', function () {
                        let rowData = dtDatos.row($(this).closest('tr')).data();

                        Alert2AccionConfirmar(
                            'Atención',
                            `¿Desea eliminar el registro del remanente para la compra "${rowData.cc}-${rowData.orden_ct}" con el insumo "${rowData.insumo}"?`,
                            'Confirmar',
                            'Cancelar',
                            () => eliminarRegistroRemanente(rowData.id)
                        );
                    });
                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle dt-nowrap', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function initTablaResumen() {
            dtResumen = tablaResumen.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                // ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                scrollY: '45vh',
                scrollX: 'auto',
                scrollCollapse: true,
                columns: [
                    { data: 'insumo', title: 'No. Insumo' },
                    { data: 'insumoDesc', title: 'Descripción Insumo' },
                    { data: 'fechaCompraString', title: 'Fecha Compra' },
                    { data: 'ccDesc', title: 'Descripción CC' },
                    { data: 'almacenDesc', title: 'Descripción Almacén' },
                    { data: 'solicitanteNombre', title: 'Nombre Solicitante' },
                    {
                        data: 'importe', title: 'Importe', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data.toString().replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                initComplete: function (settings, json) {

                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle dt-nowrap', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function cargarRemanentes() {
            axios.post('CargarRemanentes', { listaAlmacenes: selectAlmacen.val(), fechaInicio: inputFechaInicio.val(), fechaFin: inputFechaFin.val(), solicitante: +selectSolicitante.val() }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    tituloImporteTotal.text(maskNumero2DCompras(response.data.importeTotal));

                    _flagPuedeEliminar = response.data.flagPuedeEliminar;

                    AddRows(tablaDatos, data);
                    AddRows(tablaResumen, response.data.resumen);
                    initChartMeses(response.data.chartMeses);
                    initChartHorizontal(response.data.chartAlmacenes, 'chartAlmacenes');
                    initChartHorizontal(response.data.chartRequisitoresTOP, 'chartRequisitoresTOP');
                    initChartHorizontal(response.data.chartAutorizadoresCompraTOP, 'chartAutorizadoresCompraTOP');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initChartMeses(datos) {
            Highcharts.chart('chartMeses', {
                chart: {
                    zoomType: 'xy'
                },
                lang: highChartsDicEsp,
                title: { text: '' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: [
                    { // Primary yAxis
                        labels: {
                            formatter: function () {
                                let valor = this.axis.defaultLabelFormatter.call(this);

                                return '$' + valor;
                            }
                        },
                        title: { text: '' },
                        allowDecimals: false,
                        // min: 0
                    }
                ],
                tooltip: {
                    formatter: function () {
                        return `
                            <tspan style="font-size: 10px">${this.x}</tspan>
                            <tspan class="highcharts-br" dy="15" x="8">​</tspan>
                            <tspan style="fill:rgb(46, 117, 182)">●</tspan>
                            ${this.series.name}:
                            <tspan style="font-weight:bold;">${maskNumero2DComprasN(this.y)}</tspan>
                            <tspan class="highcharts-br">​</tspan>
                        `;
                    }
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            formatter: function () {

                                return '<b>' + maskNumero2DComprasN(this.point.y) + '</b>';
                            }
                        }
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, type: 'column', data: datos.serie1, color: 'rgb(46, 117, 182)' }
                ],
                legend: { enabled: false },
                credits: { enabled: false }
            });
        }

        function initChartHorizontal(datos, chart) {
            Highcharts.chart(chart, {
                chart: {
                    type: 'bar'
                },
                lang: highChartsDicEsp,
                title: { text: '' },
                xAxis: {
                    categories: datos.categorias,
                    labels: { format: '{value}' },
                },
                yAxis: {
                    visible: false,
                    // min: 0,
                    title: {
                        align: 'high'
                    },
                    labels: {
                        overflow: 'justify',
                        formatter: function () {
                            let valor = this.axis.defaultLabelFormatter.call(this);

                            return '$' + valor;
                        }
                    }
                },
                tooltip: {
                    formatter: function () {
                        return `
                            <tspan style="font-size: 10px">${this.x}</tspan>
                            <tspan class="highcharts-br" dy="15" x="8">​</tspan>
                            <tspan style="fill:rgb(46, 117, 182)">●</tspan>
                            ${this.series.name}:
                            <tspan style="font-weight:bold;">${maskNumero2DComprasN(this.y)}</tspan>
                            <tspan class="highcharts-br">​</tspan>
                        `;
                    }
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                return '<b>' + maskNumero2DComprasN(this.point.y) + '</b>';
                            }
                        }
                    }
                },
                legend: { enabled: false },
                credits: { enabled: false },
                series: [{
                    fontSize: 20,
                    pointWidth: 20,
                    showInLegend: false,
                    name: datos.serie1Descripcion,
                    data: datos.serie1,
                    color: 'rgb(46, 117, 182)'
                }]
            });
        }

        function eliminarRegistroRemanente(remanente_id) {
            axios.post('EliminarRegistroRemanente', { remanente_id }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha eliminado la información.');
                    botonBuscar.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Almacen.Remanentes = new Remanentes())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();