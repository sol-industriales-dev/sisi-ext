(() => {
    $.namespace('Maquinaria.Reporte.ReporteCostos');
    ReporteCostos = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const inputAnio = $('#inputAnio');
        const botonBuscar = $('#botonBuscar');
        const tablaCostos = $('#tablaCostos');
        const botonGenerarExcel = $('#botonGenerarExcel');
        //#endregion

        let dtCostos;

        (function init() {
            $('.select2').select2();
            initTablaCostos();

            inputAnio.val((new Date()).getFullYear());

            axios.post('/RepGastosMaquinaria/GetCentrosCosto_Rep_Costos')
                .then(response => {
                    selectCentroCosto.fillCombo({ items: response.data.items.filter((x) => x.Value != '1010' && x.Value != '1015' && x.Value != '1018') }, null, false, null);
                }).catch(error => AlertaGeneral(`Alerta`, error.message));

            botonBuscar.click(cargarReporteCostos);
            botonGenerarExcel.click(generarExcel);
        })();

        inputAnio.on('focus', function () {
            $(this).select();
        });

        function cargarReporteCostos() {
            let cc = selectCentroCosto.val();
            let anio = +inputAnio.val();

            axios.post('/RepGastosMaquinaria/CargarReporteCostos', { cc, anio })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaCostos, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initTablaCostos() {
            dtCostos = tablaCostos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                scrollY: '45vh',
                scrollX: 'auto',
                scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'descripcion', title: 'Descripción' },
                    {
                        data: 'enero', title: 'Enero', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    },
                    {
                        data: 'febrero', title: 'Febrero', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    },
                    {
                        data: 'marzo', title: 'Marzo', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    },
                    {
                        data: 'abril', title: 'Abril', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    },
                    {
                        data: 'mayo', title: 'Mayo', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    },
                    {
                        data: 'junio', title: 'Junio', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    },
                    {
                        data: 'julio', title: 'Julio', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    },
                    {
                        data: 'agosto', title: 'Agosto', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    },
                    {
                        data: 'septiembre', title: 'Septiembre', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    },
                    {
                        data: 'octubre', title: 'Octubre', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    },
                    {
                        data: 'noviembre', title: 'Noviembre', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    },
                    {
                        data: 'diciembre', title: 'Diciembre', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    },
                    {
                        data: 'total', title: 'Total', render: function (data, type, row, meta) {
                            return data > 0 ? maskNumero(data) : '';
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        function generarExcel() {
            let cc = selectCentroCosto.val();
            let anio = +inputAnio.val();

            axios.post('/RepGastosMaquinaria/GenerarExcelReporteCostos', { cc, anio })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        location.href = `/RepGastosMaquinaria/DescargarExcelReporteCostos`;
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
    $(document).ready(() => Maquinaria.Reporte.ReporteCostos = new ReporteCostos())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();