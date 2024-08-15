(() => {
    $.namespace('CtrlPptalOfCE.ReporteCont');

    //#region CONST FILTROS
    const cboFiltroCC = $('#cboFiltroCC');
    const cboFiltroAnio = $('#cboFiltroAnio');
    const cboFiltroEmpresa = $('#cboFiltroEmpresa')
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    //#endregion

    //#region CONST
    const tblCapturas = $('#tblCapturas');
    let dtCapturas;
    const graficaPresuestoGasto = $('#graficaPresuestoGasto');
    const presupuesto_title = "Presupuesto";
    const gasto_title = "Gasto";
    //#endregion

    //#region CONST DETALLE CAPTURA
    const mdlDetCaptura = $('#mdlDetCaptura');
    const tblDetCapturas = $('#tblDetCapturas');
    let dtDetCapturas;
    //#endregion

    //#region CONST DETALLE CAPTURA POR MES
    const mdlDetCapturaMes = $('#mdlDetCapturaMes');
    const tblDetCapturasMes = $('#tblDetCapturasMes');
    const lblTitlePorMes = $('#lblTitlePorMes');
    let dtDetCapturasMes;
    let dtMesContable;
    let dtTotalContable;
    //#endregion

    const modalMesContable = $('#modalMesContable');
    const tablaMesContable = $('#tablaMesContable');
    const modalTotalContable = $('#modalTotalContable');
    const tablaTotalContable = $('#tablaTotalContable');

    ReporteCont = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            initTblCapturas();
            initTblDetCapturas();
            initTblDetCapturasMes();
            initTablaMesContable();
            initTablaTotalContable();
            //#endregion

            //#region EVENTOS INDEX
            btnFiltroBuscar.on("click", function () {
                fncGetSumaCapturas();
            });
            //#endregion

            //#region FILL COMBOS
            $(".select2").select2();

            cboFiltroAnio.fillCombo("FillAnios", {}, false);
            cboFiltroAnio.select2({ width: "100%" });

            cboFiltroEmpresa.fillCombo("FillCboEmpresas", {}, false);

            cboFiltroAnio.on("change", function () {
                if ($(this).val() > 0) {
                    cboFiltroCC.fillCombo("FillUsuarioRelCCPptosAutorizados", { anio: $(this).val(), idEmpresa: cboFiltroEmpresa.val() }, false);
                    cboFiltroCC.select2({ width: "100%" });
                } else {
                    if ($(this).val() <= 0) { Alert2Warning("Es necesario seleccionar un año."); }
                }
            });
            //#endregion
        }

        //#region INDEX
        function initTblCapturas() {
            dtCapturas = tblCapturas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                //responsive: true,
                //paging: false,
                scrollY: '45vh',
                scrollX: true,
                scrollCollapse: true,
                columns: [
                    {
                        data: 'concepto', title: 'Concepto',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeEnero', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                            if (cellData.importeEnero == 8100) {
                                console.log("1");
                            }
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeEnero" mes="1">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeContEnero', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContable importeContEnero" mes="1">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeFebrero', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeFebrero" mes="2">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeContFebrero', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContable importeContFebrero" mes="2">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeMarzo', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeMarzo" mes="3">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeContMarzo', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContable importeContMarzo" mes="3">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeAbril', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeAbril" mes="4">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeContAbril', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContable importeContAbril" mes="4">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeMayo', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeMayo" mes="5">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeContMayo', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContable importeContMayo" mes="5">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeJunio', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeJunio" mes="6">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeContJunio', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContable importeContJunio" mes="6">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeJulio', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeJulio" mes="7">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeContJulio', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContable importeContJulio" mes="7">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeAgosto', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeAgosto" mes="8">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeContAgosto', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContable importeContAgosto" mes="8">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeSeptiembre', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeSeptiembre" mes="9">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeContSeptiembre', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContable importeContSeptiembre" mes="9">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeOctubre', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeOctubre" mes="10">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeContOctubre', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContable importeContOctubre" mes="10">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeNoviembre', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeNoviembre" mes="11">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeContNoviembre', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContable importeContNoviembre" mes="11">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeDiciembre', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeDiciembre" mes="12">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeContDiciembre', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContable importeContDiciembre" mes="12">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    {
                        data: 'importeTotalConcepto', title: presupuesto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeTotalConcepto">${maskNumero2DCompras(data)}</a>`;
                        }
                    },
                    {
                        data: 'importeContTotalConcepto', title: gasto_title,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeContTotalConcepto">${maskNumero2DCompras(data)}</a>`;
                        }
                    },
                    // {
                    //     render: function (data, type, row, meta) {
                    //         if (row.concepto != "TOTAL") {
                    //             return `<button class="btn btn-xs btn-primary detalle" title="Detalle del concepto."><i class="fas fa-list-ul"></i></button>&nbsp;`;
                    //         } else {
                    //             return "";
                    //         }
                    //     },
                    // },
                    { data: 'cc', visible: false },
                    { data: 'idAgrupacion', visible: false },
                    { data: 'idConcepto', visible: false },
                    { data: 'agrupacion', title: 'Agrupación', visible: false },
                    { data: 'esAgrupacion', visible: false },
                    { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblCapturas.on('click', '.detalle', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        fncGetDetCapturas(rowData.idAgrupacion, rowData.idConcepto);
                    });

                    tblCapturas.on('click', '.importePresupuesto', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        let mes = $(this).attr('mes');

                        if (rowData.concepto != "TOTAL") {
                            fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, mes);
                        }
                    });

                    tblCapturas.on('click', '.importeContable', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        let mes = $(this).attr('mes');

                        if (rowData.concepto != "TOTAL") {
                            fncGetDetCapturasMesContable(rowData.idAgrupacion, rowData.idConcepto, mes);
                        }
                    });

                    tblCapturas.on('click', '.importeTotalConcepto', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            fncGetDetCapturas(rowData.idAgrupacion, rowData.idConcepto);
                        }
                    });

                    tblCapturas.on('click', '.importeContTotalConcepto', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            fncGetDetCapturasContable(rowData.idAgrupacion, rowData.idConcepto);
                        }
                    });
                },
                columnDefs: [
                    { targets: [0], className: 'dt-body-center' },
                    { targets: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26], className: 'dt-body-right' }
                ],
            });
            new $.fn.dataTable.FixedHeader(dtCapturas);
        }

        function initGraficaPresupuestoGasto(datos) {
            Highcharts.chart('graficaPresuestoGasto', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'Gráfica Presupuesto - Gasto' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
                    labels: { format: '{value}' }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: `
                        <tr>
                            <td style="color:{series.color};padding:0">{series.name}: </td>
                            <td><b>{point.y}</b></td>
                        </tr>`,
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1, color: 'green' },
                    { name: datos.serie2Descripcion, data: datos.serie2, color: 'gray' }
                ],
                credits: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function fncGetSumaCapturas() {
            if (cboFiltroCC.val() > -1 && cboFiltroAnio.val() > 0) {
                let obj = new Object();
                obj = {
                    idCC: +cboFiltroCC.val(),
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetSumaCapturasContable", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtCapturas.clear();
                        dtCapturas.rows.add(response.data.lstSumaCapturas);
                        dtCapturas.draw();
                        initGraficaPresupuestoGasto(response.data.graficaPresupuestoGasto);
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = "";
                cboFiltroAnio.val() <= 0 ? strMensajeError += "Es necesario indicar un año." : "";
                cboFiltroCC.val() <= 0 ? strMensajeError += "<br>Es necesario indicar un CC." : "";
                Alert2Warning(strMensajeError);
            }
        }
        //#endregion

        //#region DETALLE
        function initTblDetCapturas() {
            dtDetCapturas = tblDetCapturas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'agrupacion', title: 'Agrupación' },
                    { data: 'concepto', title: 'Concepto' },
                    {
                        data: 'importeEnero', title: 'Enero',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeFebrero', title: 'Febrero',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeMarzo', title: 'Marzo',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeAbril', title: 'Abril',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeMayo', title: 'Mayo',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeJunio', title: 'Junio',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeJulio', title: 'Julio',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeAgosto', title: 'Agosto',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeSeptiembre', title: 'Septiembre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeOctubre', title: 'Octubre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeNoviembre', title: 'Noviembre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeDiciembre', title: 'Diciembre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    { data: 'anio', title: 'Año' },
                    { data: 'responsable', title: 'Responsable' }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { targets: [0, 1, 14, 15], className: 'dt-body-center' },
                    { targets: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13], className: 'dt-body-right' }
                ],
            });
        }

        function fncGetDetCapturas(idAgrupacion, idConcepto) {
            if (cboFiltroCC.val() > -1) {
                let obj = new Object();
                obj = {
                    cc: cboFiltroCC.val(),
                    idAgrupacion: idAgrupacion,
                    idConcepto: idConcepto,
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetCapturas", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtDetCapturas.clear();
                        dtDetCapturas.rows.add(response.data.lstCapPptos);
                        dtDetCapturas.draw();
                        mdlDetCaptura.modal("show");
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario indicar un CC.");
            }
        }

        function fncGetDetCapturasContable(idAgrupacion, idConcepto) {
            if (cboFiltroCC.val() > -1) {
                let obj = new Object();
                obj = {
                    idCC: cboFiltroCC.val(),
                    idAgrupacion: idAgrupacion,
                    idConcepto: idConcepto,
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetCapturasContable", obj).then(response => {
                    let { success, data, message } = response.data;
                    if (success) {
                        AddRows(tablaMesContable, data);
                        modalMesContable.modal('show');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario indicar un CC.");
            }
        }
        //#endregion

        //#region DETALLE POR MES
        function initTblDetCapturasMes() {
            dtDetCapturasMes = tblDetCapturasMes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'agrupacion', title: 'Agrupación' },
                    { data: 'concepto', title: 'Concepto' },
                    {
                        data: 'importe', title: 'Importe', render: function (data, type, row, meta) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    { data: 'anio', title: 'Año' },
                    { data: 'responsable', title: 'Responsable' }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTablaMesContable() {
            dtMesContable = tablaMesContable.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        title: 'Póliza', render: function (data, type, row, meta) {
                            return row.year + '-' + row.mes + '-' + row.poliza + '-' + row.tp;
                        }
                    },
                    { data: 'linea', title: 'Linea' },
                    { data: 'cta', title: 'cta' },
                    { data: 'scta', title: 'scta' },
                    { data: 'sscta', title: 'sscta' },
                    { data: 'concepto', title: 'Concepto' },
                    {
                        data: 'monto', title: 'Monto', render: function (data, type, row, meta) {
                            return maskNumero2DCompras(data);
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                footerCallback: function (row, data, start, end, display) {
                    var api = this.api();
                    var intVal = function (i) { return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0; };
                    total = api.column(6).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    $(api.column(6).footer()).html(maskNumero2DCompras(total));
                }
            });
        }

        function initTablaTotalContable() {
            dtTotalContable = tablaTotalContable.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'importeEnero', title: 'Enero',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeFebrero', title: 'Febrero',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeMarzo', title: 'Marzo',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeAbril', title: 'Abril',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeMayo', title: 'Mayo',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeJunio', title: 'Junio',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeJulio', title: 'Julio',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeAgosto', title: 'Agosto',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeSeptiembre', title: 'Septiembre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeOctubre', title: 'Octubre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeNoviembre', title: 'Noviembre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeDiciembre', title: 'Diciembre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { targets: [0, 1], className: 'dt-body-center' }
                ],
            });
        }

        function fncGetDetCapturasMes(idAgrupacion, idConcepto, idMes) {
            if (cboFiltroCC.val() > -1) {
                let obj = new Object();
                obj = {
                    cc: cboFiltroCC.val(),
                    idAgrupacion: idAgrupacion,
                    idConcepto: idConcepto,
                    idMes: idMes,
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetCapturasPorMes", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtDetCapturasMes.clear();
                        dtDetCapturasMes.rows.add(response.data.lstCapPptos);
                        dtDetCapturasMes.draw();
                        switch (idMes) {
                            case 1:
                                lblTitlePorMes.html(" - ENERO");
                                break;
                            case 2:
                                lblTitlePorMes.html(" - FEBRERO");
                                break;
                            case 3:
                                lblTitlePorMes.html(" - MARZO");
                                break;
                            case 4:
                                lblTitlePorMes.html(" - ABRIL");
                                break;
                            case 5:
                                lblTitlePorMes.html(" - MAYO");
                                break;
                            case 6:
                                lblTitlePorMes.html(" - JUNIO");
                                break;
                            case 7:
                                lblTitlePorMes.html(" - JULIO");
                                break;
                            case 8:
                                lblTitlePorMes.html(" - AGOSTO");
                                break;
                            case 9:
                                lblTitlePorMes.html(" - SEPTIEMBRE");
                                break;
                            case 10:
                                lblTitlePorMes.html(" - OCTUBRE");
                                break;
                            case 11:
                                lblTitlePorMes.html(" - NOVIEMBRE");
                                break;
                            case 12:
                                lblTitlePorMes.html(" - DICIEMBRE");
                                break;
                            default:
                                break;
                        }
                        mdlDetCapturaMes.modal("show");
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario indicar un CC.");
            }
        }

        function fncGetDetCapturasMesContable(idAgrupacion, idConcepto, idMes) {
            if (cboFiltroCC.val() > -1) {
                let obj = new Object();
                obj = {
                    idCC: cboFiltroCC.val(),
                    idAgrupacion: idAgrupacion,
                    idConcepto: idConcepto,
                    idMes: idMes,
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetCapturasPorMesContable", obj).then(response => {
                    let { success, data, message } = response.data;
                    if (success) {
                        AddRows(tablaMesContable, data);
                        modalMesContable.modal('show');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario indicar un CC.");
            }
        }
        //#endregion

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }

    $(document).ready(() => {
        CtrlPptalOfCE.ReporteCont = new ReporteCont();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();