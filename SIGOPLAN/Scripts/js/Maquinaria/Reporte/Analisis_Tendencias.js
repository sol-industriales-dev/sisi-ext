(() => {
    $.namespace('Maquinaria.Reportes.Tendencias');
Tendencias = function () {

    const fieldPrincipal = $("#fieldPrincipal");
    const cboEmpresa = $('#cboEmpresa');
    const cboGrupoEquipo = $('#cboGrupoEquipo');
    const cboModeloEquipo = $('#cboModeloEquipo');
    const cboCC = $('#cboCC');
    const cboAnio = $('#cboAnio');
    const btnBuscar = $('#btnBuscar');
    const tblDataEmpresa = $('#tblDataEmpresa');
    const tblData = $('#tblData');
    const tblDetalle = $("#tblDetalle");
    const tblDetalle2 = $("#tblDetalle2");
    const modalDetalle = $('#modalDetalle');
    const btnGraficar = $("#btnGraficar");
    const modalGrafica = $("#modalGrafica");
    const btnEnviarTendenciaCorreo = $('#btnEnviarTendenciaCorreo');

    const reegresar = $(".reegresar");
    const btnRegresar = $("#btnRegresar");
    const rowDetalle = $(".rowDetalle");
    const rowDetalle2 = $(".rowDetalle2");
    const getDatosGeneralesEmpresa = new URL(window.location.origin + '/RepGastosMaquinaria/getDatosGeneralesEmpresa');
    const getDatosGenerales = new URL(window.location.origin + '/RepGastosMaquinaria/getDatosGenerales');
    const getDatosGeneralesCTA = new URL(window.location.origin + '/RepGastosMaquinaria/getDatosGeneralesCTA');
    const getDatosDetalle = new URL(window.location.origin + '/RepGastosMaquinaria/getDatosDetalle');
    const getDatosDetalle_Movto = new URL(window.location.origin + '/RepGastosMaquinaria/getDatosDetalle_Movto');
    var _meses = [];
    _meses.push("Ene");
    _meses.push("Feb");
    _meses.push("Mar");
    _meses.push("Abr");
    _meses.push("May");
    _meses.push("Jun");
    _meses.push("Jul");
    _meses.push("Ago");
    _meses.push("Sep");
    _meses.push("Oct");
    _meses.push("Nov");
    _meses.push("Dic");
    var objConsultado = null;
    var _mesConsultado = 1;
    let init = () => {

        initForm();
}
async function fnVerDetalle(empresa, anio, mes, cta, scta, sscta) {
    _mesConsultado = mes;
    var _this = $(this);
    reegresar.hide();
    rowDetalle.show();
    rowDetalle2.hide();
    response = await ejectFetchJson(getDatosDetalle, { empresa: empresa, anio: anio, mes: mes, cc: getValoresMultiples('#cboCC'), cta: cta, scta: scta, sscta: sscta });
    if (response.success) {
        dtDetalle.clear().draw();
        dtDetalle.rows.add(response.datos).draw();
    }
    modalDetalle.modal("show");
}
async function fnVerDetalle_Movto(empresa, anio, mes, cta, scta, sscta, economico) {
    var _this = $(this);

    response = await ejectFetchJson(getDatosDetalle_Movto, { empresa: empresa, anio: anio, mes: _mesConsultado, cc: getValoresMultiples('#cboCC'), cta: cta, scta: scta, sscta: sscta, economico: economico });
    if (response.success) {
        dtDetalle2.clear().draw();
        dtDetalle2.rows.add(response.datos).draw();
        reegresar.show();
        rowDetalle.hide();
        rowDetalle2.show();
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
    }
    modalDetalle.modal("show");
}
function initDataTblPrincipal() {
    dtDataEmpresa = tblDataEmpresa.DataTable({
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
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 19
        }],
        select: {
            style: 'os',
            selector: 'td:last-child'
        },
        initComplete: function (settings, json) {
            tblDataEmpresa.on('click', '.select-checkbox', function () {
                let count = dtDataEmpresa.rows({ selected: true }).count();
                if (count > 0) {
                    let rowData = dtDataEmpresa.row($(this).closest('tr')).data();
                    setDatosGeneralesCTA(rowData.empresa, rowData.cta);
                    fieldPrincipal.show();
                }
                else {
                    fieldPrincipal.hide();
                    dtData.clear().draw();
                }
            });


        },
        drawCallback: function (settings) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        },
        footerCallback: function (row, data, start, end, display) {
            if (data.length > 0) {
                let ene = 0;
                let feb = 0;
                let mar = 0;
                let abr = 0;
                let may = 0;
                let jun = 0;
                let jul = 0;
                let ago = 0;
                let sep = 0;
                let oct = 0;
                let nov = 0;
                let dic = 0;
                let total = 0;
                let variabilidad = 0;
                data.forEach(function (x, i) {
                    if (i == 0) {
                        ene = x.ene;
                        feb = x.feb;
                        mar = x.mar;
                        abr = x.abr;
                        may = x.may;
                        jun = x.jun;
                        jul = x.jul;
                        ago = x.ago;
                        sep = x.sep;
                        oct = x.oct;
                        nov = x.nov;
                        dic = x.dic;
                        total = x.total;
                        variabilidad = x.variabilidad;
                    } else if (i == 2 && data.length == 5) {
                        ene += x.ene;
                        feb += x.feb;
                        mar += x.mar;
                        abr += x.abr;
                        may += x.may;
                        jun += x.jun;
                        jul += x.jul;
                        ago += x.ago;
                        sep += x.sep;
                        oct += x.oct;
                        nov += x.nov;
                        dic += x.dic;
                        total += x.total;
                        variabilidad += x.variabilidad;
                    }
                    else {
                        ene -= x.ene;
                        feb -= x.feb;
                        mar -= x.mar;
                        abr -= x.abr;
                        may -= x.may;
                        jun -= x.jun;
                        jul -= x.jul;
                        ago -= x.ago;
                        sep -= x.sep;
                        oct -= x.oct;
                        nov -= x.nov;
                        dic -= x.dic;
                        total -= x.total;
                        variabilidad += x.variabilidad;
                    }

                });

                $(row).find('th').eq(2).html("RESULTADO");
                $(row).find('th').eq(3).html(formatoDato(ene));
                $(row).find('th').eq(4).html(formatoDato(feb));
                $(row).find('th').eq(5).html(formatoDato(mar));
                $(row).find('th').eq(6).html(formatoDato(abr));
                $(row).find('th').eq(7).html(formatoDato(may));
                $(row).find('th').eq(8).html(formatoDato(jun));
                $(row).find('th').eq(9).html(formatoDato(jul));
                $(row).find('th').eq(10).html(formatoDato(ago));
                $(row).find('th').eq(11).html(formatoDato(sep));
                $(row).find('th').eq(12).html(formatoDato(oct));
                $(row).find('th').eq(13).html(formatoDato(nov));
                $(row).find('th').eq(14).html(formatoDato(dic));
                $(row).find('th').eq(15).html(formatoDato(total));
                $(row).find('th').eq(17).html(formatoDato(variabilidad));

            }
        },
        columns: [
            { title: 'Empresa', data: 'empresa', width: 80 }
            , { title: 'CTA', data: 'cuenta', width: 80 }
            , { title: 'Descripcion', data: 'descripcion', width: 80 }
            , {
                title: 'Enero', data: 'ene', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 1);
                    return formato;
                }
            }
            , {
                title: 'Febrero', data: 'feb', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 2);
                    return formato;
                }
            }
            , {
                title: 'Marzo', data: 'mar', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 3);
                    return formato;
                }
            }
            , {
                title: 'Abril', data: 'abr', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 4);
                    return formato;
                }
            }
            , {
                title: 'Mayo', data: 'may', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 5);
                    return formato;
                }
            }
            , {
                title: 'Junio', data: 'jun', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 6);
                    return formato;
                }
            }
            , {
                title: 'Julio', data: 'jul', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 7);
                    return formato;
                }
            }
            , {
                title: 'Agosto', data: 'ago', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 8);
                    return formato;
                }
            }
            , {
                title: 'Septiembre', data: 'sep', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 9);
                    return formato;
                }
            }
            , {
                title: 'Octubre', data: 'oct', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 10);
                    return formato;
                }
            }
            , {
                title: 'Noviembre', data: 'nov', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 11);
                    return formato;
                }
            }
            , {
                title: 'Diciembre', data: 'dic', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 12);
                    return formato;
                }
            }
            , {
                title: 'Total', data: 'total', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTabla(data, type);
                    return formato;
                }
            }
            , {
                title: '%', data: 'porcentaje', width: 20, render: function (data, type, row, meta) {
                    var html = formatoDato_Porcentaje(data);
                    return html;
                }
            }
            , {
                title: 'Variabilidad', width: 70, data: 'variabilidad', render: function (data, type, row, meta) {
                    var formato = formatoTabla(data, type);
                    return formato;
                }
            }
            , {
                title: '%', data: 'porcentajeVariabilidad', width: 40, render: function (data, type, row, meta) {
                    var html = formatoDato_Porcentaje(data);
                    return html;
                }
            }
            , {
                targets: 19,
                data: null,
                defaultContent: '',
                orderable: false,
                className: 'select-checkbox'
            }
        ],
        "columnDefs": [
            { targets: [0], className: 'dt-body-left' },
            { targets: [16, 18], className: 'dt-body-center' },
            { targets: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 17], className: 'dt-body-right' }]

    });
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
        "searching": false,
        "bFilter": true,
        "bInfo": true,
        "bAutoWidth": false,
        "order": [[15, 'desc']],
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 17
        }],
        select: {
            style: 'multi',
            selector: 'td:last-child'
        },
        initComplete: function (settings, json) {
            tblData.on('click', '.modalDetalle', function () {
                let rowData = dtData.row($(this).closest('tr')).data();
                let concepto = +$(this).attr('concepto');
                let mes = +$(this).attr('mes');
                _rowData = rowData;
                _conceptoDetalle = concepto;
                objConsultado = rowData;
                fnVerDetalle(rowData.empresa, cboAnio.val(), mes, _rowData.cta, _rowData.scta, _rowData.sscta);
            });
        },
        drawCallback: function (settings) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        },
        rowCallback: function (row, data, index) {
            if (data.es90_10) {
                $(row).find('td').eq(0).css('backgroundColor', 'yellow');
                $(row).find('td').eq(16).css('backgroundColor', 'yellow');
            }
        },
        footerCallback: function (row, data, start, end, display) {
            if (data.length > 0) {
                let ene = 0;
                let feb = 0;
                let mar = 0;
                let abr = 0;
                let may = 0;
                let jun = 0;
                let jul = 0;
                let ago = 0;
                let sep = 0;
                let oct = 0;
                let nov = 0;
                let dic = 0;
                let total = 0;
                let variabilidad = 0;
                data.forEach(function (x) {
                    ene += x.ene;
                    feb += x.feb;
                    mar += x.mar;
                    abr += x.abr;
                    may += x.may;
                    jun += x.jun;
                    jul += x.jul;
                    ago += x.ago;
                    sep += x.sep;
                    oct += x.oct;
                    nov += x.nov;
                    dic += x.dic;
                    total += x.total;
                    variabilidad += x.variabilidad;
                });

                $(row).find('th').eq(0).html("TOTALES");
                $(row).find('th').eq(1).html(formatoDato(ene));
                $(row).find('th').eq(2).html(formatoDato(feb));
                $(row).find('th').eq(3).html(formatoDato(mar));
                $(row).find('th').eq(4).html(formatoDato(abr));
                $(row).find('th').eq(5).html(formatoDato(may));
                $(row).find('th').eq(6).html(formatoDato(jun));
                $(row).find('th').eq(7).html(formatoDato(jul));
                $(row).find('th').eq(8).html(formatoDato(ago));
                $(row).find('th').eq(9).html(formatoDato(sep));
                $(row).find('th').eq(10).html(formatoDato(oct));
                $(row).find('th').eq(11).html(formatoDato(nov));
                $(row).find('th').eq(12).html(formatoDato(dic));
                $(row).find('th').eq(13).html(formatoDato(total));
                $(row).find('th').eq(15).html(formatoDato(variabilidad));
            }
        },
        columns: [
            { title: 'CTA Nombre', data: 'descripcion', width: 240 }
            , {
                title: 'Enero', data: 'ene', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 1);
                    return formato;
                }
            }
            , {
                title: 'Febrero', data: 'feb', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 2);
                    return formato;
                }
            }
            , {
                title: 'Marzo', data: 'mar', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 3);
                    return formato;
                }
            }
            , {
                title: 'Abril', data: 'abr', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 4);
                    return formato;
                }
            }
            , {
                title: 'Mayo', data: 'may', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 5);
                    return formato;
                }
            }
            , {
                title: 'Junio', data: 'jun', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 6);
                    return formato;
                }
            }
            , {
                title: 'Julio', data: 'jul', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 7);
                    return formato;
                }
            }
            , {
                title: 'Agosto', data: 'ago', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 8);
                    return formato;
                }
            }
            , {
                title: 'Septiembre', data: 'sep', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 9);
                    return formato;
                }
            }
            , {
                title: 'Octubre', data: 'oct', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 10);
                    return formato;
                }
            }
            , {
                title: 'Noviembre', data: 'nov', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 11);
                    return formato;
                }
            }
            , {
                title: 'Diciembre', data: 'dic', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink(data, type, 12);
                    return formato;
                }
            }
            , {
                title: 'Total', data: 'total', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTabla(data, type);
                    return formato;
                }
            }
            , {
                title: '%', data: 'porcentaje', width: 20, render: function (data, type, row, meta) {
                    var html = formatoDato_Porcentaje(data);
                    return html;
                }
            }
            , {
                title: 'Variabilidad', data: 'variabilidad', width: 70, render: function (data, type, row, meta) {
                    var formato = formatoTabla(data, type);
                    return formato;
                }
            }
            , {
                title: '%', data: 'porcentajeVariabilidad', width: 40, render: function (data, type, row, meta) {
                    var html = formatoDato_Porcentaje(data);
                    return html;
                }
            }
            , {
                targets: 17,
                data: null,
                defaultContent: '',
                orderable: false,
                className: 'select-checkbox'
            }
        ],
        "columnDefs": [{ targets: [0], className: 'dt-body-left' },
        { targets: [14, 16], className: 'dt-body-center' },
        { targets: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15], className: 'dt-body-right' }]

    });

}
function formatoTabla(data, type) {
    if (type === 'display') {
        return data >= 0 ? maskNumero_NoDecimal(data) : `<p style="color: red;">${('-' + (maskNumero_NoDecimal(data).replace('-', '')))}</p>`;
    } else if (type === 'exportxls') {
        return data >= 0 ? maskNumero_NoDecimal(data) : '-' + (maskNumero_NoDecimal(data).replace('-', ''));
    } else {
        return data;
    }
}
function formatoDato(data) {
    return data >= 0 ? maskNumero_NoDecimal(data) : `<p style="color: red;">${('-' + (maskNumero_NoDecimal(data).replace('-', '')))}</p>`;
}
function formatoDato_Porcentaje(data) {
    return data >= 0 ? data + "%" : `<p style="color: red;">${data} %</p>`;
}
function formatoTablaLink(data, type, mes) {

    if (type === 'display') {
        return data >= 0 ? `<a class='modalDetalle' data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="1" mes=${mes}>${maskNumero_NoDecimal(data)}</a>` : `<a class='modalDetalle' style="color: red;" data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="1" mes=${mes}>${'-' + (maskNumero_NoDecimal(data).replace('-', ''))}</a>`;
    } else if (type === 'exportxls') {
        return data >= 0 ? maskNumero_NoDecimal(data) : '-' + (maskNumero_NoDecimal(data).replace('-', ''));
    } else {
        return data;
    }
}
function formatoTablaLink_NoMes(data) {
    return data >= 0 ? `<a class='modalDetalle_NoMes' data-target="#modalDetalle_NoMes" data-toggle="modal" href="#modalDetalle_NoMes" concepto="1">${maskNumero_NoDecimal(data)}</a>` : `<a class='modalDetalle' style="color: red;" data-target="#modalDetalle_NoMes" data-toggle="modal" href="#modalDetalle_NoMes" concepto="1" mes=${mes}>${'-' + (maskNumero_NoDecimal(data).replace('-', ''))}</a>`;
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
        initComplete: function (settings, json) {
            tblDetalle.on('click', '.modalDetalle_NoMes', function () {
                reegresar.hide();
                rowDetalle.show();
                rowDetalle2.hide();
                let rowData = dtDetalle.row($(this).closest('tr')).data();


                _rowData = rowData;

                fnVerDetalle_Movto(objConsultado.empresa, cboAnio.val(), _mesConsultado, objConsultado.cta, objConsultado.scta, objConsultado.sscta, _rowData.economico);
            });
        },
        footerCallback: function (row, data, start, end, display) {
            if (data.length > 0) {
                let monto = 0;

                data.forEach(function (x) {
                    monto += x.monto
                });

                $(row).find('th').eq(1).html("TOTAL");
                $(row).find('th').eq(2).html(formatoDato(monto));

            }
        },
        columns: [
            { title: 'CC', data: 'economico' }
            , { title: 'Descripcion', data: 'descripcion' }
            , {
                title: 'Monto', data: 'monto', width: 50, render: function (data, type, row, meta) {
                    var formato = formatoTablaLink_NoMes(data);
                    return formato;
                }
            }
        ]
    });

    dtDetalle2 = tblDetalle2.DataTable({
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

                $(row).find('th').eq(6).html("TOTAL");
                $(row).find('th').eq(7).html(formatoDato(monto));

            }
        },
        columns: [
            { title: 'CC', data: 'descripcion' }
            , { title: 'Anio', data: 'year' }
            , { title: 'Mes', data: 'mes' }
            , { title: 'CTA', data: 'cta' }
            , { title: 'SCTA', data: 'scta' }
            , { title: 'SSCTA', data: 'sscta' }
            , { title: 'Cooncepto', data: 'concepto' }
            , {
                title: 'Monto', data: 'monto', width: 50, render: function (data, type, row, meta) {
                    var formato = formatoDato(data);
                    return formato;
                }
            }
        ]
    });
}
async function setDatosGenerales() {
    try {
        if (cboCC.val() != '') {
            dtData.clear().draw();
            //var url = cboEmpresa.val()!=2?getDatosGeneralesEmpresa:getDatosGenerales;
            response = await ejectFetchJson(getDatosGeneralesEmpresa, { empresa: cboEmpresa.val(), anio: cboAnio.val(), cc: getValoresMultiples('#cboCC'), grupo: (cboGrupoEquipo.val() == '' ? 0 : cboGrupoEquipo.val()), modelo: (cboModeloEquipo.val() == '' ? 0 : cboModeloEquipo.val()) });
            if (response.success) {
                var data = response.datosGenerales;
                dtDataEmpresa.clear().draw();
                dtData.clear().draw();
                dtDataEmpresa.rows.add(data.datos).draw();
                initgraficaTendencia(data.grafica_tendencia, false);
            } else {
                AlertaGeneral(`Erro`, `No se encontraron datos.`);
            }
        }
        else {
            AlertaGeneral('Alerta', 'Debe seleccionar una obra!');
        }

    } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
}

function enviarCorreoTendenciasIngresosCostos() {
    $.post('EnviarCorreoTendenciasIngresosCostos').then(response => {
        if (response.success) {
            AlertaGeneral('Confirmación', 'Se envió el correo');
} else {
                    AlertaGeneral('Error', response.message);
}
}, error => {
    AlertaGeneral('Alerta');
});
}

async function setDatosGeneralesCTA(empresa, cta) {
    try {
        dtData.clear().draw();
        var emp = empresa == 'CPLAN' ? 1 : 2;
        response = await ejectFetchJson(getDatosGeneralesCTA, { empresa: emp, cta: cta, anio: cboAnio.val(), cc: getValoresMultiples('#cboCC'), grupo: (cboGrupoEquipo.val() == '' ? 0 : cboGrupoEquipo.val()), modelo: (cboModeloEquipo.val() == '' ? 0 : cboModeloEquipo.val()) });
        if (response.success) {
            var data = response.datosGenerales;
            dtData.clear().draw();
            dtData.rows.add(data.datos).draw();
        } else {
            AlertaGeneral(`Erro`, `No se encontraron datos.`);
        }

    } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
}
function fnAbrirGrafica() {
    let count = dtData.rows({ selected: true }).count();
    if (count > 0) {
        let rowData = dtData.rows({ selected: true }).data();
        var series = [];
        $.each(rowData, function (i, e) {
            var valores = [];
            valores.push(e.ene);
            valores.push(e.feb);
            valores.push(e.mar);
            valores.push(e.abr);
            valores.push(e.may);
            valores.push(e.jun);
            valores.push(e.jul);
            valores.push(e.ago);
            valores.push(e.sep);
            valores.push(e.oct);
            valores.push(e.nov);
            valores.push(e.dic);
            var serie = {
                name: e.descripcion,
                data: valores
            };

            series.push(serie);
        });

        fnGraficaDetalle(series);
        modalGrafica.modal('show');
    }
    else {
        AlertaGeneral("Alerta", "Debe seleccionar almenos un concepto para graficar")
    }
}
function fnGraficaDetalle(series) {


    Highcharts.chart('grafica_modal_tendencia', {
        chart: { type: 'line', backgroundColor: '#E5E8E8' },
        lang: highChartsDicEsp,
        title: { text: 'Tendencia' },
        xAxis: {
            categories: _meses,
            crosshair: true
        },
        yAxis: {
            title: {
                text: ''
            },
            min: 0
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },
        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                },
                // pointStart: 2010
            }
        },
        series: series,
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        },
        credits: { enabled: false },
        legend: { enabled: true }
    });


}
function initgraficaTendencia(datos, tieneOtros) {

    Highcharts.chart('grafica_tendencia', {
        chart: { type: 'line', backgroundColor: '#E5E8E8' },
        lang: highChartsDicEsp,
        title: { text: 'Tendencia' },
        xAxis: {
            categories: _meses,
            crosshair: true
        },
        yAxis: {
            title: {
                text: ''
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },
        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                },
                // pointStart: 2010
            }
        },
        series: datos.series,
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        },
        credits: { enabled: false },
        legend: { enabled: true }
    });
}
function LoadGrupos() {
    cboGrupoEquipo.fillCombo('/KPI/CboGrupoEquipos', { areaCuenta: (cboCC.val() == '' ? 0 : cboCC.val()) });
}
function LoadModelos() {
    cboModeloEquipo.fillCombo('/KPI/CboModeloEquipos', { grupoID: (cboGrupoEquipo.val() == '' ? 0 : cboGrupoEquipo.val()) });
}
function initForm() {
    cboCC.fillCombo('/CatObra/cboCentroCostosUsuarios', {}, false, 'Todos', () => {
        LoadGrupos();
});
cboCC.change();
convertToMultiselect('#cboCC');
cboCC.change();
cboGrupoEquipo.change(LoadModelos);
cboGrupoEquipo.change();
btnBuscar.click(setDatosGenerales);
btnEnviarTendenciaCorreo.click(enviarCorreoTendenciasIngresosCostos);
btnGraficar.click(fnAbrirGrafica);
initDataTblPrincipal();
initDataTblDetalle();
btnRegresar.click(function () {
    reegresar.hide();
    rowDetalle.show();
    rowDetalle2.hide();
});
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
