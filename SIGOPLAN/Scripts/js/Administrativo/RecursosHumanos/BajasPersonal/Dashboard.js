(() => {
    $.namespace('RH.Dashboard');

    //#region SELECTORES

    const tblMesdeBaja = $('#tblMesdeBaja');
    const cboAñoMeses = $('#cboAñoMeses');
    const btnFiltrarMes = $('#btnFiltrarMes');
    const cboFiltroCC = $('#cboFiltroCC');
    let dtMesdeBaja;

    const tblMotivoSeparacion = $('#tblMotivoSeparacion');
    const cboAñoMotivos = $('#cboAñoMotivos');
    const btnFiltrarMotivo = $('#btnFiltrarMotivo');
    const cboFiltroSepCC = $('#cboFiltroSepCC');
    let dtMotivoSeparacion;

    const chkMostrarCeros = $('#chkMostrarCeros');

    let meses = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre",
        "Octubre", "Noviembre", "Diciembre"];

    //#endregion

    Dashboard = function () {
        (function init() {
            fncListeners();
            initTblMesdeBaja();
            initTblMotivoSeparacion();
            fncGetMesdeBaja(cboAñoMeses.val(), cboFiltroCC.val());
            fncGetMotivoSeparacion(cboAñoMotivos.val(), true, cboFiltroSepCC.val());
            initGraficaMesdeBaja();
            initGraficaMotivoSeparacion();
        })();

        function fncListeners() {

            convertToMultiselect('#cboAñoMeses');
            cboAñoMeses.multiselect('selectAll', false);
            cboAñoMeses.multiselect('refresh');
            cboAñoMeses.multiselect('deselect', 'Todos');

            convertToMultiselect('#cboAñoMotivos');
            cboAñoMotivos.multiselect('selectAll', false);
            cboAñoMotivos.multiselect('refresh');
            cboAñoMotivos.multiselect('deselect', 'Todos');

            cboFiltroCC.fillCombo("/Reclutamientos/FillFiltroCboCC", {}, false);
            cboFiltroSepCC.fillCombo("/Reclutamientos/FillFiltroCboCC", {}, false);

            cboFiltroCC.select2({ width: 'resolve' });
            cboFiltroSepCC.select2({ width: 'resolve' });

            btnFiltrarMes.on("click", function () {
                fncGetMesdeBaja(cboAñoMeses.val(), cboFiltroCC.val());
            });

            btnFiltrarMotivo.on("click", function () {
                fncGetMotivoSeparacion(cboAñoMotivos.val(), true, cboFiltroSepCC.val())
            })

            chkMostrarCeros.on("change", function () {
                if (chkMostrarCeros.prop("checked")) {
                    fncGetMotivoSeparacion(cboAñoMotivos.val(), true);
                } else {
                    fncGetMotivoSeparacion(cboAñoMotivos.val(), false);
                }
            });
        }

        //#region tbls
        function initTblMesdeBaja() {
            dtMesdeBaja = tblMesdeBaja.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    {
                        title: "Mes",
                        render: function (data, type, row) {
                            return row.añoFront + " " + meses[row.mesFront];
                        }
                    },
                    { data: 'cantidad', title: 'Cantidad' },
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTblMotivoSeparacion() {
            dtMotivoSeparacion = tblMotivoSeparacion.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: false,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'concepto', title: 'Concepto' },
                    { data: 'cantidad', title: 'Cantidad' },
                ],
                initComplete: function (settings, json) {

                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        //#endregion

        //#region CHARTS

        function initGraficaMesdeBaja(datos) {
            Highcharts.chart('divGraficaMesdeBaja', {
                chart: {
                    type: 'column'
                },

                title: {
                    text: 'Numero de Bajas por Mes'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: [
                        'Ene',
                        'Feb',
                        'Mar',
                        'Abr',
                        'May',
                        'Jun',
                        'Jul',
                        'Ago',
                        'Sep',
                        'Oct',
                        'Nov',
                        'Dec'
                    ],
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Cantidad (Bajas)'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">&nbsp{series.name}:&nbsp</td>' +
                        '<td style="padding:0"><b>&nbsp{point.y} bajas&nbsp</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,
                        minPointLength: 10
                    }
                },
                series: datos
                // [{
                //     name: "2022",
                //     //colorByPoint: true,
                //     data: datos,
                // }]
            });
        }

        function initGraficaMotivoSeparacion(datos) {
            Highcharts.chart('divGraficaMotivoSeparacion', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Numero de Bajas por Motivo de Separacion'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    type: 'category'
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Cantidad (Bajas)'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">&nbsp{series.name}:&nbsp</td>' +
                        '<td style="padding:0"><b>&nbsp{point.y} bajas&nbsp</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,
                        minPointLength: 10
                    }
                },
                series: [{
                    name: "2022",
                    //colorByPoint: true,
                    data: datos,
                }]
            });
        }

        //#endregion

        //#region BACKEND

        function fncGetMesdeBaja(lstAño, cc) {
            axios.post("getMesdeBaja", { año: lstAño, idCC: cc }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    AddRows(tblMesdeBaja, items);
                    let data = new Map();
                    items.forEach(e => {
                        let obj = {};
                        if (!data.has(e.añoFront)) {
                            data.set(e.añoFront, { name: moment(e).year(), data: [e.cantidad] });
                        }

                        data.get(e.añoFront).data.push(e.cantidad);
                        // data.push(e.cantidad);
                    });
                    let reportData = [];

                    for (const [key, value] of data) {
                        // console.log(key + ' = ' + value)
                        reportData.push({ name: key, data: value.data });
                    }
                    console.log(data);
                    console.log(reportData);

                    initGraficaMesdeBaja(reportData);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetMotivoSeparacion(lstAño, filtrar, cc) {
            axios.post("getMotivoSeparacion", { año: lstAño, filterData: filtrar, idCC: cc }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    AddRows(tblMotivoSeparacion, items);
                    console.log(items);
                    let data = [];
                    items.forEach(e => {
                        let item = {
                            name: e.concepto,
                            y: e.cantidad,
                        }
                        data.push(item);
                    });
                    initGraficaMotivoSeparacion(data);
                }
            }).catch(error => Alert2Error(error.message));

        }

        //#endregion

    }

    $(document).ready(() => {
        RH.Dashboard = new Dashboard();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();