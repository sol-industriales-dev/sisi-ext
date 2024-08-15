(() => {
    $.namespace('Maquinaria.Reportes.Tendencias');
    Tendencias = function () {

        const cboCC = $('#cboCC');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const cboAutorecepcionable = $('#cboAutorecepcionable');
        const cboEntrada = $('#cboEntrada');
        const cboFactura = $('#cboFactura');
        const cboContrarecibo = $('#cboContrarecibo');
        const cboSISUN = $('#cboSISUN');
        const btnBuscar = $('#btnBuscar');
        const tblData = $('#tblData');
        const tblDetalle = $('#tblDetalle');
        const modalDetalle = $('#modalDetalle');
        const inputReq = $('#inputReq');
        const inputOC = $('#inputOC');
        const report = $("#report");
        const getDatosGenerales = new URL(window.location.origin + '/Enkontrol/OrdenCompra/getTrazabilidadGeneralv2');
        const getDatosDetalle = new URL(window.location.origin + '/Enkontrol/OrdenCompra/getDatosDetalle');
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);
        const selectProveedor = $('#selectProveedor');
        let init = () => {
            $('.select2').select2();

            selectProveedor.fillCombo('/Enkontrol/OrdenCompra/FillComboProveedores', null, false, null);

            inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioAnio);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
            initForm();
        }
        async function fnVerDetalle(anio, mes, cta, scta, sscta) {
            var _this = $(this);
            var cc = cboCC.val();
            response = await ejectFetchJson(getDatosDetalle, { anio: anio, mes: mes, cc: cc, cta: cta, scta: scta, sscta: sscta });
            if (response.success) {
                dtDetalle.clear().draw();
                dtDetalle.rows.add(response.datos).draw();
            }
            modalDetalle.modal("show");
        }

        function initDataTblPrincipal() {
            dtData = tblData.DataTable({
                paging: false,
                destroy: true,
                ordering: true,
                language: dtDicEsp,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": true,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                initComplete: function (settings, json) {
                    tblData.on('click', '.modalReq', function () {
                        let rowData = dtData.row($(this).closest('tr')).data();
                        verReq(rowData.req_cc, rowData.req_num);
                    });
                    tblData.on('click', '.modalOC', function () {
                        let rowData = dtData.row($(this).closest('tr')).data();
                        verOC(rowData.req_cc, rowData.comp_num);
                    });
                },
                drawCallback: function (settings) {
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                },
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Reporte de Trazabvilidad", "<center><h3>Reporte de trazabilidad</h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            // columns: [':visible', 21]
                            columns: [0, 2, 4, 5, 6, 7, 8, 9, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37]
                        }
                    }
                ],
                columns: [
                    {
                        title: 'CC', data: 'req_cc', render: function (data, type, row, meta) {
                            return row.req_cc + ' - ' + row.req_ccDesc;
                        }
                    },
                    {
                        title: 'Área-Cuenta', render: function (data, type, row, meta) {
                            return (row.areaCuentaDesc != '' && row.areaCuentaDesc != null) ? `[${row.area}-${row.cuenta}] ${row.areaCuentaDesc}` : '';
                        }
                    },
                    { title: '# Req', data: 'req_num', 'visible': false },
                    {
                        title: 'Req #', data: 'req_num', render: function (data, type, row, meta) {
                            if (data != null) {
                                var formato = `<a class='modalReq' data-target="#modalReq" data-toggle="modal" href="#modalReq" concepto="3">${data}</a>`;
                                return formato;
                            }
                            else {
                                return '';
                            }
                        }
                    },
                    {
                        title: 'Req fecha', data: 'req_fecha', render: function (data, type, row, meta) {
                            var formato = moment(data).format('DD/MM/YYYY') == '31/12/0000' ? '' : moment(data).format('DD/MM/YYYY');
                            return formato;
                        }
                    },
                    { title: 'Req Usuario', data: 'req_usuario' },
                    { title: 'Req Est', data: 'req_estatus' },
                    { title: 'Req Aut', data: 'req_autoriza' },
                    { title: 'Req Aut Fecha', data: 'req_autoriza_fecha' },
                    { title: '# Comp', data: 'comp_num', 'visible': false },
                    {
                        title: 'Comp #', data: 'comp_num', render: function (data, type, row, meta) {
                            if (data != null) {
                                var formato = `<a class='modalOC' data-target="#modalOC" data-toggle="modal" href="#modalOC" concepto="3">${data}</a>`;
                                return formato;
                            }
                            else {
                                return '';
                            }
                        }
                    },
                    {
                        title: 'Comp Fecha', data: 'comp_fecha', render: function (data, type, row, meta) {
                            var formato = moment(data).format('DD/MM/YYYY') == '31/12/0000' || moment(data).format('DD/MM/YYYY') == '01/01/0001' ? '' : moment(data).format('DD/MM/YYYY');
                            return formato;
                        }
                    },
                    {
                        title: 'Comp Prov #', data: 'comp_prov_num', render: function (data, type, row, meta) {
                            return row.comp_prov_num > 0 ? data : '';
                        }
                    },
                    { title: 'Comp Prov', data: 'comp_prov_nom' },
                    { title: 'Comp Est', data: 'comp_estatus' },
                    { title: 'Comp Vobo 1', data: 'comp_vobo1_nom' },
                    {
                        title: 'Comp Vobo 1 Fecha', data: 'comp_vobo1_fecha', render: function (data, type, row, meta) {
                            var formato = moment(data).format('DD/MM/YYYY') == '31/12/0000' || moment(data).format('DD/MM/YYYY') == '01/01/0001' ? '' : moment(data).format('DD/MM/YYYY');
                            return formato;
                        }
                    },
                    { title: 'Comp Vobo 2', data: 'comp_vobo2_nom' },
                    {
                        title: 'Comp Vobo 2 Fecha', data: 'comp_vobo2_fecha', render: function (data, type, row, meta) {
                            var formato = moment(data).format('DD/MM/YYYY') == '31/12/0000' || moment(data).format('DD/MM/YYYY') == '01/01/0001' ? '' : moment(data).format('DD/MM/YYYY');
                            return formato;
                        }
                    },
                    { title: 'Comp Aut ', data: 'comp_aut_nom' },
                    {
                        title: 'Comp Aut Fecha', data: 'comp_aut_fecha', render: function (data, type, row, meta) {
                            var formato = moment(data).format('DD/MM/YYYY') == '31/12/0000' || moment(data).format('DD/MM/YYYY') == '01/01/0001' ? '' : moment(data).format('DD/MM/YYYY');
                            return formato;
                        }
                    },
                    { title: 'Comp Surtido', data: 'comp_surtido' },
                    { title: 'Comp Comprador', data: 'comp_comprador_nom' },
                    { title: 'Tiene Entrada', data: 'comp_tiene_entrada' },
                    { title: 'Tiene Factura', data: 'comp_tiene_factura' },
                    { title: 'Portal', data: 'fac_portal_existe' },
                    { title: 'Factura Tipo', data: 'fac_tipo' },
                    { title: 'Portal Estatus', data: 'fac_portal_estatus' },
                    {
                        title: 'Portal Fecha', data: 'fac_portal_fecha', render: function (data, type, row, meta) {
                            var formato = moment(data).format('DD/MM/YYYY') == '31/12/0000' || moment(data).format('DD/MM/YYYY') == '01/01/0001' ? '' : moment(data).format('DD/MM/YYYY');
                            return formato;
                        }
                    },
                    { title: 'Tiene Contrarecibo', data: 'fac_tiene_contrarecibo' },
                    {
                        title: 'Fecha Contrarecibo', data: 'fac_contrarecibo_fecha', render: function (data, type, row, meta) {
                            var formato = moment(data).format('DD/MM/YYYY') == '31/12/0000' || moment(data).format('DD/MM/YYYY') == '01/01/0001' ? '' : moment(data).format('DD/MM/YYYY');
                            return formato;
                        }
                    },
                    { title: 'Folio Contrarecibo', data: 'fac_contrarecibo_folio' },
                    { title: 'Factura', data: 'fac_numero' },
                    { title: 'Total Facturado', data: 'comp_total_factura' },
                    { title: 'Tiene Pago', data: 'fac_tiene_pago' },
                    { title: 'Total Pagado', data: 'fac_pagado' },
                    { title: 'Total Entrada', data: 'comp_total_entrada' },
                    { title: 'Cambio Moneda', data: 'comp_tipo_cambio' }

                ],
                "columnDefs": [
                    {
                        "targets": [5],
                        "visible": false
                    }
                ]
            });


        }
        function formatoTabla(data, type) {
            if (type === 'display') {
                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
            } else if (type === 'exportxls') {
                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
            } else {
                return data;
            }
        }
        function formatoDato(data) {
            return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
        }
        function formatoTablaLink(data, type, mes) {
            if (type === 'display') {
                return data >= 0 ? `<a class='modalDetalle' data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="1" mes=${mes}>${maskNumero(data)}</a>` : `<a class='modalDetalle' style="color: red;" data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="1" mes=${mes}>${'-' + (maskNumero(data).replace('-', ''))}</a>`;
            } else if (type === 'exportxls') {
                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
            } else {
                return data;
            }
        }
        function initDataTblDetalle() {
            dtDetalle = tblDetalle.DataTable({
                paging: false,
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": false,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                footerCallback: function (row, data, start, end, display) {
                    if (data.length > 0) {
                        let monto = 0;

                        data.forEach(function (x) {
                            monto += x.monto
                        });

                        $(row).find('th').eq(0).html("TOTAL");
                        $(row).find('th').eq(1).html(formatoDato(monto));

                    }
                },
                columns: [
                    { title: 'Economico', data: 'economico' }
                    , {
                        title: 'Monto', data: 'monto', render: function (data, type, row, meta) {
                            var formato = formatoDato(data);
                            return formato;
                        }
                    }
                ]
            });
        }
        function getData() {
            var o = {};
            o.cc = cboCC.val();
            o.fecha_inicio = inputFechaInicio.val();
            o.fecha_fin = inputFechaFin.val();
            o.autorecepcionable = cboAutorecepcionable.val();
            o.tipo_surtido = cboEntrada.val();
            o.tiene_entrada = cboEntrada.val();
            o.tiene_factura = cboFactura.val();
            o.tiene_contrarecibo = cboContrarecibo.val();
            o.sisun = cboSISUN.val();
            o.req = inputReq.val() == '' ? 0 : inputReq.val();
            o.oc = inputOC.val() == '' ? 0 : inputOC.val();
            o.proveedor = +selectProveedor.val();
            return o;
        }
        function setDatosGenerales() {
            try {
                $.ajax({
                    datatype: "json",
                    url: getDatosGenerales,
                    type: "POST",
                    data: { filtro: getData() },
                    success: function (response) {
                        if (response.success) {
                            var temp = response.data;
                            var data = JSON.parse(temp);
                            dtData.clear().draw();
                            dtData.rows.add(data).draw();
                        } else {
                            AlertaGeneral(`Erro`, `No se encontraron datos.`);
                        }
                    }
                });
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        function verReq(cc, numero) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", '/Reportes/Vista.aspx?idReporte=112&cc=' + cc + '&numero=' + numero);
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
        }
        function verOC(cc, numero) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", '/Reportes/Vista.aspx?idReporte=240&cc=' + cc + '&numero=' + numero);
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
        }
        function initForm() {
            cboCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCc', null, false);

            btnBuscar.click(setDatosGenerales);
            initDataTblPrincipal();
            initDataTblDetalle();

            $('#modalDetalle').on('shown.bs.modal', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            });
        }
        init();
    }
    $(document).ready(() => {
        Maquinaria.Reportes.Tendencias = new Tendencias();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();
