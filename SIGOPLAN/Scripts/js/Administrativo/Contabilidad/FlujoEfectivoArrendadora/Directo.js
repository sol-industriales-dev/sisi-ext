(() => {
    $.namespace('Administrativo.FlujoEfectivoArrendadora.Directo');
Directo = function () {
    var fechaPlaneacion, fechaCorte, areaCuenta, centroCostos, dtDirDetalle, idConceptoDir, lstBtnNiveles, lstCCVistos, esAdmin, esCC;
    let semanaVerModal = 0, anioVerModal = 0;
    const chbAcCc = $('.chbAcCc');
    const divDirAC = $('#divDirAC');
    const divDirCC = $('#divDirCC');
    const selDirAC = $('#selDirAC');
    const chbVisto = $('#chbVisto');
    const chbFlujo = $('#chbFlujo');
    const selDirCC = $('#selDirCC');
    const tblDirecto = $('#tblDirecto');
    const divNiveles = $('#divNiveles');
    const mpDirSemana = $('#mpDirSemana');
    const selDirSemana = $('#selDirSemana');
    const btnDirBuscar = $('#btnDirBuscar');
    const mdlDirDetale = $('#mdlDirDetale');
    const lblDirDetalle = $('#lblDirDetalle');
    const divTendencias = $('#divTendencias');
    const tblProyCierre = $('#tblProyCierre');
    const divProyCierre = $('#divProyCierre');
    const btnGraDirecto = $('#btnGraDirecto');
    const tblDirDetalle = $('#tblDirDetalle');
    const txtDirDetTotal = $('#txtDirDetTotal');
    const btnPrintReport = $("#btnPrintReport");
    const txtDirInicioObra = $('#txtDirInicioObra');
    const getCCTodos = originURL('/Administrativo/FlujoEfectivoArrendadora/getCboCCTodos');
    const guardarCcVisto = originURL('/Administrativo/FlujoEfectivoArrendadora/guardarCcVisto');
    const getLstCCvistos = originURL('/Administrativo/FlujoEfectivoArrendadora/getLstCCvistos');
    const geTblDirDetalle = originURL('/Administrativo/FlujoEfectivoArrendadora/geTblDirDetalle');
    const getIniCboDirecto = originURL('/Administrativo/FlujoEfectivoArrendadora/getIniCboDirecto');
    const getFechaUltimoCorte = originURL('/Administrativo/FlujoEfectivoArrendadora/getFechaUltimoCorte');
    const getlstFlujoEfectivoDirecto = originURL('/Administrativo/FlujoEfectivoArrendadora/getlstFlujoEfectivoDirecto');
    const report = $("#report");
    //#region Planeacion
    //GeneralesModal Planeacion
    const modalPlaneacion = $("#modalPlaneacion");
    const labelTituloModalPlaneacion = $("#labelTituloModalPlaneacion");
    const divPpalPlaneacion = $('#divPpalPlaneacion');
    const divDetallePlaneacion = $('#divDetallePlaneacion');
    const divSubDetallePlaneacion = $('#divSubDetallePlaneacion');
    const bntBackPpal = $("#bntBackPpal");
    const bntBackDetalle = $("#bntBackDetalle");
    const chbSemanaAnterior = $("#chbSemanaAnterior");
    //Data Tablas
    let dtTablaPlaneacionPpal;
    let dtTablaPlaneacionetalle;
    let dtTablaPlaneacionDetalleN1;
    //Selectores Tablas
    const tablaPlaneacionPpal = $('#tablaPlaneacionPpal');
    const tablaPlaneacionDetalle = $('#tablaPlaneacionDetalle');
    const tablaPlaneacionDetalleN1 = $("#tablaPlaneacionDetalleN1");

    //Constante para indicar si es semana anterior
    let esAnterior = false;

    //#endregion
    let init = () => {
        initForm();
    chbVisto.change(setGuardarCCVisto);
    chbAcCc.click(changeCcAc);
    selDirCC.change(setInicioObra);
    selDirAC.change(setCCDetalle);
    btnDirBuscar.click(setlstFlujoEfectivoDirecto);
    mdlDirDetale.on('shown.bs.modal', function () {
        dtDirDetalle.columns.adjust();
        if (idConceptoDir == 29) { //Reserva
            mdlDirDetale.find(".modal-content").css({
                width: "30em"
            });
        }
    });
    mdlDirDetale.on('hidden.bs.modal', function () {
        mdlDirDetale.find(".modal-content").css({
            width: "60em"
        });
    });
    $(document).on('click', ".btnNivel", function (event) {
        setNivel(event.target);
    });
    btnPrintReport.on('click', () => {
        verReporte();
});
//#region Inicializaciones para planeacion
bntBackPpal.click(initDivPpal);
bntBackDetalle.click(initDivDetalle);
initTablaPlaneacionPpal();
initTablaPlaneacionDetalles();
initTablaPlaneacionDetallesSubnivel();
//#endregion
chbSemanaAnterior.change(setTablaCierreAnterior);
}
modalPlaneacion.on('shown.bs.modal', function () {
    initDivPpal();
    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
});
modalPlaneacion.on('hidden.bs.modal', function () {
    divDetallePlaneacion.addClass('hide');
    divSubDetallePlaneacion.addClass('hide');
});

async function setlstFlujoEfectivoDirecto() {
    $.blockUI({ message: 'Procesando...' });
    btnGraDirecto.prop("disabled", true);
    btnPrintReport.prop("disabled", true);
    dtDirecto.clear().draw();
    dtProyCierre.clear().draw();
    let busq = getDirBusq();
    if (busq.esCC) {
        divTendencias.addClass("hidden");
    } else {
        divTendencias.removeClass("hidden");
    }
    await axios.post(getlstFlujoEfectivoDirecto, busq)
        .then(res => {
            $.blockUI({ message: 'Procesando...' });
    let response = res.data;
    if (response.success) {
        setLstCCvistos();
        setTblDirectoEncabezado(response.encabezado);
        dtDirecto.rows.add(response.lst).draw();
        let lstSemanas = getSemanaslabels(response.lstGrafico)
        btnPrintReport.prop("disabled", response.lstGrafico.length === 0);
        if (esAdmin) {
            dtProyCierre.rows.add(response.lstCierre).draw();
            setMixedChartProyCierre(response.lstCierreGrafico, lstSemanas);
        }
    }
})
.catch(o_O => AlertaGeneral(o_O.message))
.finally(o_O => $.unblockUI());
}
//async function setFechaUltimoCorte() {
//    await axios.get(getFechaUltimoCorte)
//        .then(response => {
//            if (response.data.success) {
//                chbAcCc.data().esCC = true;
//    setLstCCvistos();
//    setSelCC();
//    changeCcAc();
//    setlstFlujoEfectivoDirecto();
//    esAdmin = response.data.esAdmin;
//    initDivProyeccionCierre();
//}
//}).catch(o_O => AlertaGeneral(o_O.message));
//}
function setFechaUltimoCorte() {
    chbAcCc.data().esCC = true;
    setLstCCvistos();
    setSelCC();
    changeCcAc();
    $.ajax({
        url: getFechaUltimoCorte,
        type: "POST",
        dataType: "json",
        success: function (result) {
            if (result.success) { 
                esAdmin = result.esAdmin;
                setlstFlujoEfectivoDirecto();
                initDivProyeccionCierre();
            }
            else {
                AlertaGeneral('Alerta','Ocurrio un error favor de reportarlo a TI');
            }
        }
    });
            
}
async function setGuardarCCVisto() {
    let semana = getSemanaSeleccionada();
    if (selDirCC.val() !== areaCuenta) {
        this.checked = !this.checked;
        return
    }
    await axios.post(guardarCcVisto, {
        cc: {
            anio: semana.anio,
            semana: semana.semana,
            cc: areaCuenta,
            esVisto: this.checked
        }
    }).then(response => {
        let { success } = response.data;
    if (success) {
        setLstCCvistos();
    }
}).catch(o_O => AlertaGeneral(o_O.message));
}
async function setLstCCvistos() {
    let semana = getSemanaSeleccionada();
    await axios.post(getLstCCvistos, {
        anio: semana.anio,
        semana: semana.semana
    }).then(response => {
        let { lst } = response.data;
    lstCCVistos = lst;
    setPintarCboCCSeleccionado();
    setPintarCboCCOption();
}).catch(o_O => console.log(o_O));
}
function setPintarCboCCSeleccionado() {
    let cc = selDirCC.val();
    selDirCC.removeClass("ccObraCerrada");
    if (lstCCVistos !== undefined) {
        let ccVisto = lstCCVistos.find(c => c.cc == cc);
        selDirCC.removeClass("ccVisto");
        chbVisto.prop("checked", false);
        if (ccVisto !== undefined) {
            chbVisto.prop("checked", ccVisto.esVisto);
            if (ccVisto.esVisto) {
                selDirCC.addClass("ccVisto");
            }
        }
    }
    let opt = selDirCC.find(`[value=${cc}]`)
                , gpo = opt.closest("optgroup");
if (gpo !== undefined && gpo.prop("label") === "PRESUPUESTO TERMINADO") {
    selDirCC.addClass("ccObraCerrada");
}
}
function setPintarCboCCOption() {
    if (lstCCVistos !== undefined) {
        lstCCVistos.forEach(cbo => {
            let ccVisto = lstCCVistos.find(c => c.cc == cbo.cc);
        selDirCC.find(`option[value=${cbo.cc}]`).removeClass("ccVisto");
    if (ccVisto !== undefined) {
        if (ccVisto.esVisto) {
            selDirCC.find(`option[value=${cbo.cc}]`).addClass("ccVisto");
    }
}
});
}
}
function setTblDirectoEncabezado({ noSemanaConsulta, fecha, noSemanaCorte, noSemanaSiguiente, ac, cc, fechaCorteMax }) {
    let arrFecha = $.toDate(fecha).split('/')
        , arrCorte = $.toDate(fechaCorteMax).split('/');
    fechaCorte = new Date(arrCorte[2], +arrCorte[1] - 1, arrCorte[0]);
    fechaPlaneacion = new Date(arrFecha[2], +arrFecha[1] - 1, arrFecha[0] - 4);
    anioFlujo = fechaCorte.getFullYear();
    centroCostos = cc;
    areaCuenta = ac;
    esCC = chbAcCc.data().esCC;
    arranque = selDirCC.find("option:selected").data().prefijo;
    esAnioFlujo = +arranque.split("/")[1] == anioFlujo;
    inicioObra = chbFlujo.prop("checked") ? arranque : esAnioFlujo ? arranque : `ENERO / ${anioFlujo}`;
    tblDirecto.find("thead tr:eq(1) th:eq(0)").text(`DE ${inicioObra} AL ${fechaCorte.toLocaleDateString()}`);
tblDirecto.find("thead tr:eq(1) th:eq(1)").text(`SEMANA - ${noSemanaConsulta}`);
tblDirecto.find("thead tr:eq(1) th:eq(2)").text(`PLANEACION - ${noSemanaCorte}`);
tblDirecto.find("thead tr:eq(1) th:eq(3)").text(`SEMANA - ${noSemanaCorte}`);
tblDirecto.find("thead tr:eq(0) th:eq(4)").text(`PLANEACION - ${noSemanaSiguiente}`);
}
function getDirBusq() {
    let semana = getSemanaSeleccionada();
    return {
        tipo: 0
        , min: semana.min
        , max: semana.max
        , lstCC: selDirAC.val()
        , lstAC: selDirCC.val()
        , lstTm: null
        , idConcepto: 0
        , esFlujo: chbFlujo.prop("checked")
        , esCC: chbAcCc.data().esCC
    }
}
function getSemanaSeleccionada() {
    return JSON.parse(selDirSemana.find("option:selected").data().prefijo);
}
async function initForm() {
    $.blockUI({ message: 'Procesando...' });
    setChbDescripcion();
    txtDirInicioObra.val("ENERO / 2020");
    btnGraDirecto.prop("disabled", true);
    await axios.get(getIniCboDirecto)
        .then(response => {
            let { success, cboAC, cboSemana } = response.data;
    if (success) {
        selDirSemana.fillComboItems(cboSemana, undefined);
        selDirAC.fillComboItems(cboAC);
        selDirAC.val(cboAC[0].Value);
        selDirAC.select2({ dropdownAutoWidth: false });
        selDirSemana.find("option:last").prop("selected", true);
    }
}).then(() => {
    initDataTblDirecto();
initDataTblProyCierre();
setFechaUltimoCorte();
}).catch(o_O => AlertaGeneral(o_O.message));
}
function setSelCC() {
    selDirCC.fillComboGroupSelectable(getCCTodos, null, true, "TODOS");
    areaCuenta = selDirCC.find("option:first").val();
    selDirCC.val(areaCuenta);
}
function initDivProyeccionCierre() {
    if (esAdmin) {
        divProyCierre.removeClass("hidden");
    } else {
        divProyCierre.addClass("hidden");
    }
}
function initDataTblDirecto() {
    dtDirecto = tblDirecto.DataTable({
        destroy: true,
        ordering: false,
        paging: false,
        ordering: false,
        searching: false,
        bFilter: true,
        info: false,
        language: dtDicEsp,
        sClass: 'text-center',
        createdRow: (tr, data) => { $(tr).addClass(data.clase); }
                , columns: [
                    { data: 'descripcion', width: '20%' }
                    , { data: 'flujoTotalProyecto', width: '15%', createdCell: (td, data, rowdata) => { setTdNumero(td, data, rowdata.clase); } }
                    , { data: 'consulta', width: '15%', createdCell: (td, data, rowdata) => { setTdNumero(td, data, rowdata.clase); } }
                    , { data: 'recientePlaneacion1', width: '15%', createdCell: (td, data, rowdata) => { setTdNumero(td, data, rowdata.clase); } }
                    , { data: 'recientePlaneacion2', width: '15%', createdCell: (td, data, rowdata) => { setTdNumero(td, data, rowdata.clase); } }
                    , { data: 'planeacion', width: '15%', createdCell: (td, data, rowdata) => { setTdNumero(td, data, rowdata.clase); } }
]
                , initComplete: function (settings, json) {
                    tblDirecto.on("click", "tbody tr td:nth-child(2)", async function () {
                        let data = dtDirecto.row($(this).closest("tr")).data();
                        if (data.flujoTotalProyecto !== 0 && data.idCpto > 0) {
                            idConceptoDir = data.idCpto;
                            esAnterior = false;
                            setPrimerDetalle(3, fechaPlaneacion);
                        }
                    });
                    tblDirecto.on("click", "tbody tr td:nth-child(3)", async function () {
                        let data = dtDirecto.row($(this).closest("tr")).data();
                        if (data.consulta !== 0 && data.idCpto > 0) {
                            idConceptoDir = data.idCpto;
                            esAnterior = false;
                            setPrimerDetalle(0, fechaPlaneacion);
                        }
                    });
                    tblDirecto.on("click", "tbody tr td:nth-child(4)", async function () {
                        let data = dtDirecto.row($(this).closest("tr")).data();
                        if (data.recientePlaneacion1 !== 0 && data.idCpto > 0) {
                            idConceptoDir = data.idCpto;
                            semanaVerModal = fechaPlaneacion.noSemana(),
                            anioVerModal = fechaPlaneacion.getFullYear();
                            getDetallesPlaneacionPPal();
                        }
                    });
                    tblDirecto.on("click", "tbody tr td:nth-child(5)", async function () {
                        let data = dtDirecto.row($(this).closest("tr")).data();
                        if (data.recientePlaneacion2 !== 0 && data.idCpto > 0) {
                            idConceptoDir = data.idCpto;
                            esAnterior = false;
                            setPrimerDetalle(0, fechaCorte);
                        }
                    });
                    tblDirecto.on("click", "tbody tr td:nth-child(6)", async function () {
                        let data = dtDirecto.row($(this).closest("tr")).data();
                        if (data.planeacion !== 0 && data.idCpto > 0) {

                            dtTablaPlaneacionPpal.clear().draw();
                            idConceptoDir = data.idCpto;
                            semanaVerModal = fechaPlaneacion.noSemana() + 1;
                            anio = fechaPlaneacion.getFullYear();
                            getDetallesPlaneacionPPal();
                        }
                    });
                }
});
}

function initDataTblProyCierre() {
    dtProyCierre = tblProyCierre.DataTable({
        destroy: true,
        ordering: false,
        paging: false,
        searching: false,
        bFilter: true,
        info: false,
        language: dtDicEsp,
        sClass: 'text-center',
        createdRow: (tr, data) => { $(tr).addClass(data.clase); },
columns: [
    { data: 'descripcion' }
    , { data: 'flujoTotalProyectoAnterior', createdCell: (td, data, rowdata) => { setTdNumero(td, data, rowdata.clase); } }
                    , { data: 'flujoTotalProyecto', createdCell: (td, data, rowdata) => { setTdNumero(td, data, rowdata.clase); } }
]
                , initComplete: function (settings, json) {
                    tblProyCierre.on("click", "tbody tr td:nth-child(2)", async function () {
                        let data = dtProyCierre.row($(this).closest("tr")).data(),
                            concepto = "";
                        if (data.flujoTotalProyectoAnterior !== 0 && data.idCpto > 0) {
                            idConceptoDir = data.idCpto;
                            tipo = data.idCpto == 29 ? 9 : 7;
                            if (data.idCpto == 29) {
                                concepto = data.descripcion.replace("(-) RESERVAS - ", "")
                            }
                            esAnterior = chbSemanaAnterior.prop("checked");
                            setPrimerDetalle(tipo, fechaCorte, concepto);
                        }
                    }),
                    tblProyCierre.on("click", "tbody tr td:nth-child(3)", async function () {
                        let data = dtProyCierre.row($(this).closest("tr")).data(),
                            concepto = "";
                        if (data.flujoTotalProyecto !== 0 && data.idCpto > 0) {
                            idConceptoDir = data.idCpto;
                            tipo = data.idCpto == 29 ? 9 : 7;
                            if (data.idCpto == 29) {
                                concepto = data.descripcion.replace("(-) RESERVAS - ", "")
                            }
                            esAnterior = false;
                            setPrimerDetalle(tipo, fechaCorte, concepto);
                        }
                    });
                }
});
}
function setPrimerDetalle(tipo, fecha, concepto) {
    setTblDirDetalle({
        tipo
        , idConceptoDir
        , fechaPlaneacion: fecha
        , cta: 0
        , scta: 0
        , sscta: 0
        , concepto
        , esAnterior
        });
}
async function setTblDirDetalle(busq) {
    busq.esAnterior = esAnterior;
    $.blockUI({ message: 'Procesando...' });
    let config = null;
    if (dtDirDetalle != null) {
        dtDirDetalle.clear().destroy();
        tblDirDetalle.empty();
        tblDirDetalle.append($('<thead>', { class: 'bg-table-header' }))
    }
    txtDirDetTotal.val(maskNumero(0));
    response = await axios.post(geTblDirDetalle, busq).then(res => res.data);
    if (response.success) {
        lblDirDetalle.text(response.title);
        txtDirDetTotal.val(maskNumero(response.total));
        switch (busq.tipo) {
            case 0: //ConsultaCuenta
                config = {
                    createdRow: (row, data, dataIndex) => { $(row).addClass("detClick"); }
                    , columns: [
                        { title: "Cuenta", data: 'cta', render: (data, type, row, meta) => `${data}-${row.scta}-${row.sscta}` }
                                , { title: "Descripcion", data: 'descripcion' }
                                , { title: "Monto", data: 'monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
]
};
lstBtnNiveles = [];
break;
                    case 1: //ConsultaPoliza
config = {
    createdRow: (row, data, dataIndex) => { $(row).addClass(""); }
                            , columns: [
                                { title: "Folio", data: 'folio' }
                                , { title: "Cuenta", data: 'cta', render: (data, type, row, meta) => `${data}-${row.scta}-${row.sscta}` }
                                , { title: "Descripcion", data: 'descripcion' }
                                , { title: "AC", data: 'areaCuenta' }
                                , { title: "CC", data: 'centroCostos' }
                                , { title: "Concepto", data: 'concepto' }
                                , { title: "Monto", data: 'monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
]
};
lstBtnNiveles.length = 3;
break;
                    case 2: //ConsultaProveedor
config = {
    createdRow: (row, data, dataIndex) => { $(row).addClass("detClick"); }
                            , columns: [
                                { title: "Concepto", data: 'concepto' }
                                , { title: "Cuenta", data: 'cta', render: (data, type, row, meta) => `${data}-${row.scta}-${row.sscta}` }
                                , { title: "Descripcion", data: 'descripcion' }
                                , { title: esCC ? "Centro Costos" : "Area Cuenta", data: 'centroCostos' }
                                , { title: "Monto", data: 'monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
]
};
lstBtnNiveles.length = 2;
break;
                    case 5: //ConsultaCentroCostos
config = {
    createdRow: (row, data, dataIndex) => { $(row).addClass("detClick"); }
                            , columns: [
                                { title: "Cuenta", data: 'cta', render: (data, type, row, meta) => `${data}-${row.scta}-${row.sscta}` }
                                , { title: "Descripcion", data: 'descripcion' }
                                , { title: esCC ? "Centro Costos" : "Area Cuenta", data: 'centroCostos' }
                                , { title: "Monto", data: 'monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
]
};
lstBtnNiveles.length = 1;
break;
                    case 3: //FlujoTotalCuenta
config = {
    createdRow: (row, data, dataIndex) => { $(row).addClass("detClick"); }
                            , columns: [
                                { title: "Cuenta", data: 'cta', render: (data, type, row, meta) => `${data}-${row.scta}-${row.sscta}` }
                                , { title: "Descripcion", data: 'descripcion' }
                                , { title: "Monto", data: 'monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
]
};
lstBtnNiveles = [];
break;
                    case 4: //FlujoTotalProveedor
config = {
    createdRow: (row, data, dataIndex) => { $(row).addClass(""); }
                            , columns: [
                                { title: "Concepto", data: 'concepto' }
                                , { title: "Cuenta", data: 'cta', render: (data, type, row, meta) => `${data}-${row.scta}-${row.sscta}` }
                                , { title: "Descripcion", data: 'descripcion' }
                                , { title: "Centro Costos", data: 'centroCostos' }
                                , { title: "Area Cuenta", data: 'areaCuenta' }
                                , { title: "Monto", data: 'monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
]
};
lstBtnNiveles.length = 2;
break;
                    case 6: //FlujoTotalCentroCostos
config = {
    createdRow: (row, data, dataIndex) => { $(row).addClass("detClick"); }
                            , columns: [
                                { title: "Cuenta", data: 'cta', render: (data, type, row, meta) => `${data}-${row.scta}-${row.sscta}` }
                                , { title: "Descripcion", data: 'descripcion' }
                                , { title: esCC ? "Centro Costos" : "Area Cuenta", data: 'centroCostos' }
                                , { title: "Monto", data: 'monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
]
};
lstBtnNiveles.length = 1;
break;
                    case 7: //CierrePrincipal
config = {
    createdRow: (row, data, dataIndex) => { $(row).addClass("detClick"); }
                            , columns: [
                                { title: "Descripcion", data: 'descripcion' }
                                , { title: "Monto", data: 'monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
                            ]
};
lstBtnNiveles = [];
break;
                    case 8: //CierreManual
config = {
    createdRow: (row, data, dataIndex) => { $(row).addClass(""); }
                            , columns: [
                                { title: "Descripcion", data: 'descripcion' }
                                , { title: esCC ? "Centro Costos" : "Area Cuenta", data: 'cc' }
                                , { title: "Monto", data: 'monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
                            ]
};
lstBtnNiveles.length = 1;
break;
                    case 9: //CierreReserva
config = {
    createdRow: (row, data, dataIndex) => { $(row).addClass(""); }
                            , columns: [

                                {
                                    title: "Ingreso", data: 'abono', createdCell: (td, data, rowdata) => {
                                        if (rowdata.naturaleza == 2 || rowdata.naturaleza == 4) {
                                            $(td).addClass("text-right").prop("title", rowdata.abonoDesc).html(maskNumero(data));
} else {
    $(td).html("");
}
}
},
{
    title: "Egreso", data: 'cargo', createdCell: (td, data, rowdata) => {
        if (rowdata.naturaleza == 1 || rowdata.naturaleza == 3) {
            $(td).prop("title", rowdata.cargoDesc).html(setTdNumero(td, data, "Saldo"));
} else {
                                            $(td).html("");
}
}
}
]
};
lstBtnNiveles = [];
break;
                    case 10: //CierreConcepto
config = {
    createdRow: (row, data, dataIndex) => { $(row).addClass("detClick"); }
                            , columns: [
                                { title: "Descripcion", data: 'descripcion' }
                                , { title: "Monto", data: 'monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
                            ]
};
lstBtnNiveles.length = 1;
break;
                    case 11: //CierreFactura
config = {
    createdRow: (row, data, dataIndex) => { $(row).addClass(""); }
                            , columns: [
                                { title: "Descripcion", data: 'descripcion' }
                                , { title: 'Area Cuenta', data: 'ac' }
                                , { title: 'Centro Costos', data: 'cc' }
                                , { title: "Factura", data: 'factura' }
                                , { title: "Fecha", data: 'fecha', render: (data, type, row, meta) => $.toDate(data) }
                                , { title: "Monto", data: 'monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
                            ]
};
lstBtnNiveles.length = 2;
break;
                    default:
break;
}
lstBtnNiveles.push({
    idConceptoDir: busq.idConceptoDir,
    cta: busq.cta,
    scta: busq.scta,
    sscta: busq.sscta,
    descripcion: response.nivelDesc,
    tipo: busq.tipo,
    cc: busq.cc,
    concepto: busq.concepto,
    tipoCierre: busq.tipoCierre
});
setBotoneraNivel(busq.tipo);
dtDirDetalle = tblDirDetalle.DataTable({
    destroy: true
    , language: dtDicEsp
    , paging: false
    , searching: false
    , bFilter: true
    , info: false
    , scrollY: "70vh"
    , scrollCollapse: true
    , createdRow: config.createdRow
    , data: response.data
    , columns: config.columns
    , initComplete: function (settings, json) {
        tblDirDetalle.off().on("click", "tbody tr.detClick", async function () {
            $(".ui-tooltip").remove();
            let data = dtDirDetalle.row($(this)).data();
            if (data.idConceptoDir > 0) {
                if (data.tipo == 0) {
                    data.cta = 0;
                    data.scta = 0;
                    data.sscta = 0;
                }
                data.fechaPlaneacion = fechaPlaneacion;
                setTblDirDetalle(data);
            }
        });
        $('#tblDirDetalle td[title]')
            .hover(function () {
                showTooltip($(this));
            }, function () {
                hideTooltip();
            });

        function showTooltip($el) {
            $(this).html($el.attr('title'));
        }
        function hideTooltip() {
            $(this).hide();
        }
    }
});
$.unblockUI()
mdlDirDetale.modal("show");
}
}
function setNivel(btn) {
    let data = $(btn).data();
    setTblDirDetalle({
        tipo: +btn.value
        , idConceptoDir
        , fechaPlaneacion
        , cta: data.cta
        , scta: data.scta
        , sscta: data.sscta
        , cc: data.cc
        , ac: data.ac
        , concepto: data.concepto
        , tipoCierre: data.tipoCierre
        , esAnterior
        });
}
function setBotoneraNivel(tipo) {
    let botones = [];
    lstBtnNiveles.forEach(data => {
        let boton = $(`<button>`, {
            value: data.tipo
            , class: `btnNivel btn ${data.tipo == tipo ? "btn-success" : "btn-primary"} ${data.tipo == tipo ? "disabled" : ""}`
                    , text: data.descripcion
});
boton.data(data);
botones.push(boton);
});
divNiveles.html(botones);
}
function setTdNumero(td, numero, clase) {
    let esNegativo = numero < 0
        , esNumero = ["Saldo", "Suma", "InputEncabezado"].includes(clase)
        , texto = $(`<p>`).text(esNumero ? maskNumero(numero).replace("-", "") : "");
    if (esNegativo) {
        texto.addClass(`danger`);
    }
    $(td).html(texto);
}
function changeCcAc() {
    chbAcCc.data().esCC = !chbAcCc.data().esCC;
    setChbDescripcion();
    if (chbAcCc.data().esCC) {
        setCCDetalle();
        divDirAC.removeClass('hidden');
        divDirCC.addClass('hidden');
        chbVisto.prop("disabled", true);
    }
    else {
        setInicioObra();
        divDirAC.addClass('hidden');
        divDirCC.removeClass('hidden');
        chbVisto.prop("disabled", false);
    }
    selDirAC.select2();
}
function setCCDetalle() {
    let cc = selDirAC.val();
    if (cc.includes("TODOS")) {
        chbFlujo.prop("checked", false);
        chbFlujo.attr("disabled", true);
    } else {
        chbFlujo.removeAttr("disabled");
    }
}
function setInicioObra() {
    let fecha = selDirCC.find("option:selected").data().prefijo;
    txtDirInicioObra.val(fecha);
    let ac = selDirCC.val();
    if (ac === "TODOS") {
        chbFlujo.prop("checked", false);
        chbFlujo.attr("disabled", true);
    } else {
        chbFlujo.removeAttr("disabled");
    }
    setPintarCboCCSeleccionado();
}
function setChbDescripcion() {
    let esCC = chbAcCc.data().esCC;
    chbAcCc.text(esCC ? "Centro Costo" : "Area Cuenta");
}
//#region charts
function setMixedChartData(lst) {
    let lstSemanas = getSemanaslabels(lst)
        , lstConceptos = getGpoConceptosMontos(lst);
    mixedChart = Highcharts.chart('chartDirecto', {
        chart: { type: 'line' },
        title: { text: '' },
        xAxis: {
            categories: lstSemanas,
            plotLines: [{
                color: '#FF0000',
                width: 2,
            }],
        },
        yAxis: { title: { text: '' } },
        tooltip: {
            formatter: function () {
                return `<b>${this.x}</b><br/>${this.series.name}: ${maskNumero(this.y)}`
            }
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: lstConceptos,
        responsive: {
            rules: [{
                condition: { maxWidth: 93 },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    },

                }
            }]
        },
        credits: { enabled: false }
    });
}
function setMixedChartPerdidaGanancia(lst) {
    let lstSemanas = getSemanaslabels(lst)
        , lstPerdidaGanancia = getGpoPerdidaGanancia(lst);
    mixedChartPerdidaGanancia = Highcharts.chart('chartDirectoPerdidaGanancia', {
        chart: { type: 'line' },
        title: { text: '' },
        xAxis: { categories: lstSemanas },
        yAxis: { title: { text: '' } },
        tooltip: {
            formatter: function () {
                return `<b>${this.x}</b><br/>${this.y < 0 ? "Perdida" : "Ganancia"}: ${maskNumero(this.y)}`
            }
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: lstPerdidaGanancia,
        responsive: {
            rules: [{
                condition: { maxWidth: 93 },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    },

                }
            }]
        },
        credits: { enabled: false }
    });
}
function setMixedChartProyCierre(lst, lstSemanas) {
    let lstColor = ['#9586F7', '#50B432', '#ED561B', '#D07F00', '#24CBE5', '#645572', '#FF9655', '#FF6263', '#6AF9C4', '#64A572', '#FA2655', '#009263', '#6A89D4', '#991D80'];
    // if (fechaCorte.getFullYear() == 2020) {
    //     lstSemanas = lst[0].data.map((data, i) => `Semana ${i + 23}`);
    // }
    mixedChartPerdidaGanancia = Highcharts.chart('chartProyCierre', {
        chart: {
            type: 'line',
        },
        title: { text: '' },
        xAxis: {
            categories: lstSemanas,
        },
        yAxis: {
            title: { text: '' },
        },
        tooltip: {
            formatter: function () {
                return `<b>${this.x}</b><br/>${this.series.name}: ${maskNumero(this.y)}`
            }
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true,
                    formatter: function () {
                        result = this.y;
                        let numeroLongutid = result.toString().split(".")[0].length;
                        switch (true) {
                            case numeroLongutid >= 7:
                                result = `${(this.y / 1000000).toFixed(2)}M`;
                                break;
                            case numeroLongutid >= 4 && numeroLongutid <= 6:
                                result = `${(this.y / 1000).toFixed(2)}K`;
                                break;
                            default:
                                result = (this.y).toFixed(2);
                                break;
                        }
                        return result;
                    },
                }
            }
        },
        series: lst,
        colors: lstColor,
        responsive: {
            rules: [{
                condition: { maxWidth: 93 },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    },
                }
            }]
        },
        credits: { enabled: false }
    });
}
function getSemanaslabels(lst) {
    return lst.map(d => `Semana ${d.noSemana}`);
}
function getGpoConceptosMontos(lst) {
    lst = lst.filter(g => g.stack !== "PerdidaGanancia");
    return Object.values(lst.reduce((r, a) => {
        r[a.name] = r[a.name] || [];
    r[a.name].push(a);
    return r;
}, Object.create(null)))
                .map(d => ({
                    name: d[0].name
                    , data: d.map(m => m.monto.monto)
                    , tooltip: { valueDecimals: 2 },
                    stack: d[0].stack,
                    type: d[0].stack == "Ingresos" || d[0].stack == "Egresos" ? "column" : "line",
                }))
}
function getGpoPerdidaGanancia(lst) {
    lst = lst.filter(g => g.stack === "PerdidaGanancia");
    return Object.values(lst.reduce((r, a) => {
        r[a.name] = r[a.name] || [];
    r[a.name].push(a);
    return r;
}, Object.create(null)))
                .map(d => ({
                    name: d[0].name
                    , data: d.map(m => m.monto.monto)
                    , tooltip: { valueDecimals: 2 },
                    stack: d[0].stack,
                    type: "line"
                }))
}
//#endregion
function verReporte() {
    let pFechaPeriodo = mpDirSemana.val();
    var path = `/Reportes/Vista.aspx?idReporte=182&pFechaPeriodo=${pFechaPeriodo}`;
    report.attr("src", path);
    document.getElementById('report').onload = function () {
        openCRModal();
    };
}


//#region Modal para visualizar información de planeacion+
function initDivPpal() {
    divPpalPlaneacion.removeClass('hide');
    divDetallePlaneacion.addClass('hide');
    $('#divTam').removeClass('modal-dialog-ppal');
}
function initDivDetalle() {
    divDetallePlaneacion.removeClass('hide');
    divSubDetallePlaneacion.addClass('hide');
}
//Cargar la primera información de de la tabla.
function getDetallesPlaneacionPPal() {
    $.get('/Administrativo/FlujoEfectivoArrendadora/getDetallesPlaneacionPPal', {
        conceptoID: idConceptoDir,
        areaCuenta: areaCuenta,
        centroCostos: centroCostos,
        semana: semanaVerModal,
        esCC: esCC,
        anio: fechaPlaneacion.getFullYear(),
    }).then(response => {
        if (response.success) {
            if (dtTablaPlaneacionPpal != null) {
                labelTituloModalPlaneacion.text(response.conceptos.Concepto);
                modalPlaneacion.modal('show');
                dtTablaPlaneacionPpal.clear().draw();
                dtTablaPlaneacionPpal.rows.add(response.planeacionPpal).draw();
            }
else {
                AlertaGeneral('Alerta', 'No se encontro ningun registro.');
}
} else {
    // Operación no completada.
    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
}
}, error => {
    // Error al lanzar la petición.
    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
}
            );
}

//Es la primera tabla mostrada en el modal.
function initTablaPlaneacionPpal() {
    dtTablaPlaneacionPpal = tablaPlaneacionPpal.DataTable({
        language: dtDicEsp,
        "scrollY": "400px",
        "scrollCollapse": true,
        destroy: true,
        paging: false,
        searching: false,
        columns: [
            {
                data: 'descripcion', title: 'Concepto',
                createdCell: (td, data, rowdata) => {
                    $(td).addClass('viewPpal').html(data);
}
},
{ data: 'monto', title: 'Monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
],
columnDefs: [{
    className: "dt-center",
    "targets": "_all",
}],
    drawCallback: function (settings) {

        tablaPlaneacionPpal.find('.viewPpal').click(function () {
            let data = dtTablaPlaneacionPpal.row($(this).parents('tr')).data();
            idConceptoDir = data.conceptoID;
            idTipoConcepto = data.tipoConcepto;
            getDetallesPlaneacion(idTipoConcepto);
        });
    }
});
}
//Segunda Tabla, Detalle de los conceptos.
async function getDetallesPlaneacion(idTipoConcepto) {
    try {
        $.get('/Administrativo/FlujoEfectivoArrendadora/getSubNivelDetallePlaneacion/', {
            conceptoID: idConceptoDir,
            ac: areaCuenta,
            cc: centroCostos,
            semana: semanaVerModal,
            esCC,
            anio: fechaPlaneacion.getFullYear(),
            tipo: idTipoConcepto,
        })
            .then(response => {
                if (response.success) {
                    if (dtTablaPlaneacionetalle != null) {
                        divPpalPlaneacion.addClass('hide');
                        divDetallePlaneacion.removeClass('hide');
                        $('#divTam').addClass('modal-dialog-ppal');

                        dtTablaPlaneacionetalle.clear().draw();
                        dtTablaPlaneacionetalle.rows.add(response.planeacionDetalle).draw();
                    }
    else {
                        AlertaGeneral('Alerta', 'No se encontro ningun registro.');
    }
}
else {
                            AlertaGeneral(`Operaci�n fallida`, `No se pudo completar la operaci�n ${response.message}`);
}
},
error => {
    AlertaGeneral(`Operaci�n fallida`, `Ocurri� un error al lanzar la petici�n al servidor: Error ${error.status} - ${error.statusText}.`);
});
} catch (e) { AlertaGeneral(`Operaci�n fallida`, e.message) }
}

function initTablaPlaneacionDetalles() {
    dtTablaPlaneacionetalle = tablaPlaneacionDetalle.DataTable({
        language: dtDicEsp,
        "scrollY": "400px",
        "scrollCollapse": true,
        destroy: true,
        paging: false,
        searching: false,
        columns: [
            {
                data: 'numProv', title: '# proveedor/Cliente',
                createdCell: (td, data, rowdata) => {
                    return rowdata.numProv == 0 || data == undefined ? $(td).html(rowdata.numcte) : $(td).html(data);
}
},
{
    data: 'descripcion', title: 'Concepto',
    createdCell: (td, data, rowdata) => {
        return rowdata.detalle ? $(td).addClass('detalleInfo').html(data) : $(td).html(data);
}
},
{ data: 'ac', title: 'Area Cuenta' },
{ data: 'cc', title: 'Centro Costos' },
{ data: 'monto', title: 'Monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
],
columnDefs: [{
    className: "dt-center",
    "targets": "_all",
}],
    drawCallback: function (settings) {
        tablaPlaneacionDetalle.find('.detalleInfo').click(function () {
            let data = dtTablaPlaneacionetalle.row($(this).parents('tr')).data();
            idConceptoDir = data.conceptoID;
            getDetallesPlaneacionSubnivel(idConceptoDir, data.numProv, data.numcte);
        });
    }
});
}

function initTablaPlaneacionDetallesSubnivel() {
    dtTablaPlaneacionDetalleN1 = tablaPlaneacionDetalleN1.DataTable({
        language: dtDicEsp,
        "scrollY": "400px",
        "scrollCollapse": true,
        destroy: true,
        paging: false,
        searching: false,
        columns: [
            {
                data: 'numprov', title: '# Proveedor / Cliente', createdCell: (td, data, rowdata) => {
                    return rowdata.numprov == 0 || data == undefined ? $(td).html(rowdata.numcte) : $(td).html(data);
}
},
{
    data: 'descripcion', title: 'Concepto',
    createdCell: (td, data, rowdata) => {
        return rowdata.detalle ? $(td).addClass('detalleInfo').html(data) : $(td).html(data);
}
},
{ data: 'ac', title: 'Area Cuenta' },
{ data: 'cc', title: 'Centro Costos' },
{
    data: 'factura', title: 'Factura', createdCell: (td, data, rowdata) => {
        return data == undefined ? $(td).html('-------------') : $(td).html(data);
}
},
{
    data: 'fechaFactura', title: 'Factura', createdCell: (td, data, rowdata) => {
        return data == undefined ? $(td).html('-------------') : $(td).html(data);
}
},
{ data: 'monto', title: 'Monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) }
],
columnDefs: [{
    className: "dt-center",
    "targets": "_all",
}]
});
}

function getDetallesPlaneacionSubnivel(idConceptoDir, numProv, numcte) {

    $.get('/Administrativo/FlujoEfectivoArrendadora/getSubDetalle', {
        ac: areaCuenta,
        cc: centroCostos,
        semana: semanaVerModal,
        esCC,
        anio: fechaPlaneacion.getFullYear(),
        conceptoID: idConceptoDir,
        numProv,
        numcte
        }).then(response => {
            if (response.success) {
            if (dtTablaPlaneacionDetalleN1 != null) {
                divDetallePlaneacion.addClass('hide');
                divSubDetallePlaneacion.removeClass('hide');
                dtTablaPlaneacionDetalleN1.clear().draw();
                dtTablaPlaneacionDetalleN1.rows.add(response.planeacionDetalle).draw();
            }
else {
                AlertaGeneral('Alerta', 'No se encontro ningun registro.');
}
} else {
    // Operación no completada.
    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
}
}, error => {
    // Error al lanzar la petición.
    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
}
            );
}
function setTablaCierreAnterior()
{
    var columna = dtProyCierre.column(1);
    columna.visible(chbSemanaAnterior.prop("checked"));
    dtProyCierre.columns.adjust();
}

//Opciones de Tablas

//#endregion
$.unblockUI();
init();
}
$(document).ready(() => {
    Administrativo.FlujoEfectivoArrendadora.Directo = new Directo();
});
})();