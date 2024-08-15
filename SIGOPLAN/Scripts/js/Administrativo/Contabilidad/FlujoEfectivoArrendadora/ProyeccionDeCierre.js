(() => {
    $.namespace('Administrativo.FlujoEfectivoArrendadora.ProyeccionDeCierre');
    ProyeccionDeCierre = function () {
        var startDate, endDate, idConceptoDir, areaCuenta, centroCostos, esCC, dtProyCierre, tipo, dtProyeccionCierreAutomatico, dtGrupoReserva;
        const selAC = $('#selAC');
        const selCC = $('#selCC');
        const chbAcCc = $('#chbAcCc');
        const divForm = $('.divForm');
        const selDetAC = $('#selDetAC');
        const selDetCC = $('#selDetCC');
        const txtFecha = $('#txtFecha');
        const lblTitulo = $('#lblTitulo');
        const lblDetalle = $('#lblDetalle');
        const divDetalle = $('#divDetalle');
        const divSelDetCC = $('#divSelDetCC');
        const mdlCatGrupo = $('#mdlCatGrupo');
        const txtDetGrupo = $('#txtDetGrupo');
        const txtDetFecha = $('#txtDetFecha');
        const txtDetMonto = $('#txtDetMonto');
        const btnSelTodos = $('#btnSelTodos');
        const btnDesTodos = $('#btnDesTodos');
        const chProyectado = $('#chProyectado');
        const divAreaCuenta = $('#divAreaCuenta');
        const btnGpoReserva = $('#btnGpoReserva');
        const divDetalleDos = $('.divDetalleDos');
        const btnDetGuardar = $('#btnDetGuardar');
        const btnDetLimpiar = $('#btnDetLimpiar');
        const tblProyCierre = $('#tblProyCierre');
        const btnGpoLimpiar = $('#btnGpoLimpiar');
        const chbGrupoActivo = $('#chbGrupoActivo');
        const chNoProyectado = $('#chNoProyectado');
        const btnAutoGuardar = $('#btnAutoGuardar');
        const tblProyDetalle = $('#tblProyDetalle');
        const divCentroCostos = $('#divCentroCostos');
        const txtGrupoReserva = $('#txtGrupoReserva');
        const tblGrupoReserva = $('#tblGrupoReserva');
        const selDetNaturaleza = $('#selDetNaturaleza');
        const txtDetDescripcion = $('#txtDetDescripcion');
        const lblAcCcDescripcion = $('#lblAcCcDescripcion');
        const txtTotalAutomatico = $('#txtTotalAutomatico');
        const mdlProyeccionCierre = $('#mdlProyeccionCierre');
        const tblProyeccionCierreAutomatico = $('#tblProyeccionCierreAutomatico');
        const getCorte = originURL('/Administrativo/FlujoEfectivoArrendadora/getCorte');
        const getCC = originURL('/Administrativo/FlujoEfectivoArrendadora/getCboCCActivosSigoplan');
        const getLstGpoReserva = originURL('/Administrativo/FlujoEfectivoArrendadora/getLstGpoReserva');
        const getCboNaturaleza = originURL('/Administrativo/FlujoEfectivoArrendadora/getCboNaturaleza');
        const getLstCptoCierre = originURL('/Administrativo/FlujoEfectivoArrendadora/getLstCptoCierre');
        const GuardarGpoReserva = originURL('/Administrativo/FlujoEfectivoArrendadora/GuardarGpoReserva');
        const getLstGrupoReserva = originURL('/Administrativo/FlujoEfectivoArrendadora/getLstGrupoReserva');
        const getLstDetProyeccion = originURL('/Administrativo/FlujoEfectivoArrendadora/getLstDetProyeccion');
        const getLstProyeccionCierre = originURL('/Administrativo/FlujoEfectivoArrendadora/getLstProyeccionCierre');
        const guardarDetProyeccionCierre = originURL('/Administrativo/FlujoEfectivoArrendadora/guardarDetProyeccionCierre');
        const eliminarDetProyeccionCierre = originURL('/Administrativo/FlujoEfectivoArrendadora/eliminarDetProyeccionCierre');
        const getComboAreaCuentaConCentroCostos = originURL('/Administrativo/FlujoEfectivoArrendadora/getComboAreaCuentaConCentroCostos');
        let init = () => {
            initForm();
            txtDetMonto.change(setDetMonto);
            selAC.change(setComboBusqueda);
            selCC.change(setComboBusqueda);
            chbAcCc.change(setComboBusqueda);
            txtDetMonto.focus(function () {
                $(this).select();
            });
            btnDetLimpiar.click(limparDetForm);
            btnSelTodos.click(setChbAutomatico);
            btnDesTodos.click(setChbAutomatico);
            btnGpoLimpiar.click(limpiarGrupoForm);
            btnGpoReserva.click(setGuardarGpoReserva);
            chProyectado.change(setLstProyeccionCierre);
            chNoProyectado.change(setLstProyeccionCierre);
            btnDetGuardar.click(setGuardarDetProyeccionCierre);
            txtDetGrupo.change(setLstDetProyeccionFromSelGrupo);
            btnAutoGuardar.click(setGuardarAutoProyeccionCierre);
            mdlCatGrupo.on('shown.bs.modal', function () {
                dtGrupoReserva.tables({ visible: true, api: true }).columns.adjust();
                setLstGpoReserva();
                limpiarGrupoForm();
            });
        }
        async function setLstCptoCierre() {
            try {
                divDetalle.addClass("hidden");
                response = await ejectFetchJson(getLstCptoCierre, getBusqForm());
                if (response.success) {
                    areaCuenta = selAC.val();
                    centroCostos = selCC.val();
                    esCC = chbAcCc.prop("checked");
                    dtProyCierre.clear().draw();
                    dtProyCierre.rows.add(response.lst).draw();
                }
            } catch (o_O) { NotificacionGeneral('Aviso', o_O.message) }
        }
        async function setLstDetProyeccion() {
            try {
                divDetalle.removeClass("hidden");
                limparDetForm();
                dtProyDetalle.clear().draw();
                response = await ejectFetchJson(getLstDetProyeccion, getBusqForm());
                lblDetalle.html(`<i class="fas fa-table"></i> ${response.lblConcepto}`);
                if (response.success) {
                    dtProyDetalle.clear().draw();
                    dtProyDetalle.rows.add(response.lst).draw();
                    setDetalleDos();
                    setBtnForm();
                }
            } catch (o_O) { NotificacionGeneral('Aviso', o_O.message) }
        }
        async function setLstDetProyeccionFromSelGrupo() {
            try {
                dtProyDetalle.clear().draw();
                response = await ejectFetchJson(getLstDetProyeccion, getBusqForm());
                if (response.success) {
                    dtProyDetalle.clear().draw();
                    dtProyDetalle.rows.add(response.lst).draw();
                }
            } catch (o_O) { NotificacionGeneral('Aviso', o_O.message) }
        }
        async function setGuardarDetProyeccionCierre() {
            try {
                let lst = getLstDetalle();
                if (lst == undefined) {
                    return;
                }
                response = await ejectFetchJson(guardarDetProyeccionCierre, lst);
                if (response.success) {
                    limparDetForm();
                    setMensajeConfirmacion();
                    setSumaManualCptoCierre(response);
                    setLstDetProyeccion();
                }
            } catch (o_O) { NotificacionGeneral('Aviso', o_O.message) }
        }
        async function setGuardarAutoProyeccionCierre() {
            try {
                let lst = getLstAutomatico();
                var scheme = { lst: new Array() };
                $.sm_SplittedSave(guardarDetProyeccionCierre, lst, scheme, 25, doneGuardar);
            } catch (o_O) { NotificacionGeneral('Aviso', o_O.message) }
        }
        async function setEliminarDetProyeccionCierre(id, index) {
            try {
                response = await ejectFetchJson(eliminarDetProyeccionCierre, { id });
                if (response.success) {
                    let data = dtProyDetalle.row(index).data();
                    setRestaManualCptoCierre(data);
                    limparDetForm();
                    dtProyDetalle.row(index).remove().draw();
                    NotificacionGeneral("Aviso", "Proyección eliminada correctamente.");
                }
            } catch (o_O) { NotificacionGeneral('Aviso', o_O.message) }
        }
        async function setLstProyeccionCierre(busq) {
            try {
                txtTotalAutomatico.val(maskNumero(0));
                if ($(busq.target).is("input")) {
                    busq = getBusqForm();
                    busq.idConceptoDir = idConceptoDir;
                    busq.tipo = tipo;
                }
                if (dtProyeccionCierreAutomatico !== undefined) {
                    dtProyeccionCierreAutomatico.clear().draw();
                }
                response = await ejectFetchJson(getLstProyeccionCierre, {
                    busq,
                    esProyectado: chProyectado.prop("checked"),
                    esNoProyectado: chNoProyectado.prop("checked")
                });
                lblTitulo.text(response.titulo);
                if (response.success) {
                    setTblProyeccionCierreAutomatico(response.lst, response.tipo);
                    txtTotalAutomatico.val(maskNumero(response.total));
                    setBtnForm();
                    mdlProyeccionCierre.modal("show");
                } else {
                    NotificacionGeneral("Aviso", `De momento no hay ${response.titulo} vigentes.`)
                }
            } catch (o_O) { NotificacionGeneral('Aviso', o_O.message) }
        }
        async function setLstGpoReserva() {
            try {
                dtGrupoReserva.clear().draw();
                response = await ejectFetchJson(getLstGpoReserva);
                if (response.success) {
                    dtGrupoReserva.rows.add(response.lst).draw();
                }
            } catch (o_O) { NotificacionGeneral('Aviso', o_O.message) }
        }
        async function setGuardarGpoReserva() {
            try {
                response = await ejectFetchJson(GuardarGpoReserva, getGpoForm());
                if (response.success) {
                    setLstGpoReserva();
                    limpiarGrupoForm();
                    txtDetGrupo.fillCombo(getLstGrupoReserva, null, true, null);
                    NotificacionGeneral("Aviso", "Grupo guardado con exíto.");
                }
            } catch (o_O) { NotificacionGeneral('Aviso', o_O.message) }
        }
        async function setMaxCorte() {
            try {
                response = await ejectFetchJson(getCorte);
                if (response.success) {
                    let arrUltimaFecha = $.toDate(response.maxCorte).split('/')
                        , ultimaFecha = new Date(arrUltimaFecha[2], +arrUltimaFecha[1] - 1, +arrUltimaFecha[0] + 7);
                    txtFecha.datepicker({
                        firstDay: 0,
                        showOtherMonths: true,
                        selectOtherMonths: true,
                        onSelect: function (dateText, inst) {
                            setSemanaSelecionada();
                            setLstCptoCierre();
                        },
                        beforeShowDay: function (date) {
                            var cssClass = '';
                            if (date >= startDate && date <= endDate)
                                cssClass = 'ui-datepicker-current-day';
                            return [true, cssClass];
                        },
                        onChangeMonthYear: function (year, month, inst) {
                            selectCurrentWeek();
                        }
                    }).datepicker("setDate", ultimaFecha);
                    setSemanaSelecionada();
                    setComboBusqueda();
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        function initDataTblProyCierre() {
            dtProyCierre = tblProyCierre.DataTable({
                destroy: true
                , ordering: false
                , paging: false
                , ordering: false
                , searching: false
                , bFilter: true
                , info: false
                , language: dtDicEsp
                , columns: [
                    { data: 'concepto', title: "Concepto" }
                    , {
                        data: 'monto', title: 'Monto Total', createdCell: (td, data, rowData, row, col) => {
                            let botonera = createBotoneraProyeccionCierre(rowData);
                            $(td).html(botonera);
                        }
                    }
                ]
                , initComplete: function (settings, json) {
                    tblProyCierre.on('click', '.btnManual', function () {
                        let row = $(this).closest("tr");
                        data = dtProyCierre.row(row).data();
                        idConceptoDir = data.id;
                        setLstDetProyeccion();
                    });
                    tblProyCierre.on('click', '.btnAutomatico', function () {
                        let busq = getBusqForm();
                        busq.tipo = this.value;
                        tipo = busq.tipo;
                        idConceptoDir = $(this).data().idConceptoDir;
                        busq.idConceptoDir = idConceptoDir;
                        chProyectado.prop("checked", false);
                        chNoProyectado.prop("checked", true);
                        mdlProyeccionCierre.modal("show");
                        setLstProyeccionCierre(busq);
                    });
                }
            });
        }
        function createBotoneraProyeccionCierre({ monto, id }) {
            let div = $("<span>", {
                class: "input-group-btn"
            }),
                lst = [];
            lst.push($(`<button>`, {
                type: "button",
                class: "btn btn-xs btn-success btnManual",
                text: maskNumero(monto),
            }));
            switch (id) {
                case 21: //IngresosXCobrar
                    let btnIngresosXCobrar = $(`<button>`, {
                        type: "button",
                        class: `btn btn-xs btn-primary btnAutomatico`,
                        text: "Facturas Clientes",
                        value: 1
                    });
                    btnIngresosXCobrar.data().idConceptoDir = id
                    lst.push(btnIngresosXCobrar);
                    break;
                case 22: //RetencionesClientes
                    let btnRetencionesClientes = $(`<button>`, {
                        type: "button",
                        class: `btn btn-xs btn-primary btnAutomatico`,
                        text: "Retenciones Clientes",
                        value: 2
                    });
                    btnRetencionesClientes.data().idConceptoDir = id
                    lst.push(btnRetencionesClientes);
                    break;
                case 25: //MovimientoProveedor
                    let btnMovimientoProveedor = $(`<button>`, {
                        type: "button",
                        class: "btn btn-xs btn-primary btnAutomatico",
                        text: "Movimiento Proveedor",
                        value: 3
                    });
                    btnMovimientoProveedor.data().idConceptoDir = id
                    lst.push(btnMovimientoProveedor);
                    break;
                case 27: //DocumentosXPagar
                    let btnContrato = $(`<button>`, {
                        type: "button",
                        class: `btn btn-xs btn-primary btnAutomatico`,
                        text: "Contratos",
                        value: 9
                    });
                    btnContrato.data().idConceptoDir = id
                    lst.push(btnContrato);
                    break;
                case 28: //AmortizacionClientes
                    let btnAmortizacionClientes = $(`<button>`, {
                        type: "button",
                        class: `btn btn-xs btn-primary btnAutomatico`,
                        text: "Anticipos de Clientes",
                        value: 5
                    });
                    btnAmortizacionClientes.data().idConceptoDir = id
                    lst.push(btnAmortizacionClientes);
                    break;
                case 31: //MovimientoArrendadora
                    let btnMovimientoArrendadora = $(`<button>`, {
                        type: "button",
                        class: "btn btn-xs btn-primary btnAutomatico",
                        text: "Movimientos Arrendadora",
                        value: 6
                    });
                    btnMovimientoArrendadora.data().idConceptoDir = id
                    lst.push(btnMovimientoArrendadora);
                    break;
                case 32: //AnticipoClientes
                    let btnAnticipoClientes = $(`<button>`, {
                        type: "button",
                        class: `btn btn-xs btn-primary btnAutomatico `,
                        text: "Anticipo Clientes",
                        value: 7
                    });
                    btnAnticipoClientes.data().idConceptoDir = id
                    lst.push(btnAnticipoClientes);
                    break;
                case 33: //AnticipoContratista
                    let btnAnticipoContratista = $(`<button>`, {
                        type: "button",
                        class: `btn btn-xs btn-primary btnAutomatico `,
                        text: "Anticipo Contratistas",
                        value: 8
                    });
                    btnAnticipoContratista.data().idConceptoDir = id
                    lst.push(btnAnticipoContratista);
                    break;
                default:
                    break;
            }
            lst.forEach(btn => div.append(btn));
            return div;
        }
        function initDataTblGrupoReserva() {
            dtGrupoReserva = tblGrupoReserva.DataTable({
                destroy: true
                , language: dtDicEsp
                , columns: [
                    { data: 'esActivo', title: 'Estado', width: '2%', render: (data, type, row, meta) => data ? "Activo" : "No activo" }
                    , { data: 'grupo', title: 'Grupo', width: '98%' }
                ]
                , initComplete: function (settings, json) {
                    tblGrupoReserva.on('click', 'tr', function () {
                        let row = $(this).closest("tr");
                        data = dtGrupoReserva.row(row).data();
                        setGrupoForm(data);
                    });
                }
            });
        }
        function limpiarGrupoForm() {
            setGrupoForm({
                id: 0,
                idDetProyGemelo: 0,
                grupo: "",
                esActivo: true
            });
        }
        function setGrupoForm({ id, grupo, esActivo }) {
            txtGrupoReserva.val(grupo);
            txtGrupoReserva.data().id = id;
            chbGrupoActivo.prop("checked", esActivo);
        }
        function getGpoForm() {
            return {
                id: txtGrupoReserva.data().id,
                grupo: txtGrupoReserva.val(),
                esActivo: chbGrupoActivo.prop("checked")
            }
        }
        function initDataTblProyDetalle() {
            dtProyDetalle = tblProyDetalle.DataTable({
                destroy: true
                , ordering: false
                , paging: false
                , ordering: false
                , bFilter: true
                , info: false
                , language: dtDicEsp
                , scrollY: "400px"
                , scrollCollapse: true
                , createdRow: (tr, data) => $(tr).addClass(data.tipo == 0 ? "detClick" : "")
                , columns: [
                    { data: 'descripcion', title: 'Descripción' }
                    , { data: 'monto', title: 'Monto', createdCell: (td, data, rowData, row, col) => $(td).addClass("text-right").html(maskNumero(data)) }
                    , { data: 'factura', title: 'Factura' }
                    , { data: 'fecha', title: 'Fecha', render: (data, type, row, meta) => $.toDate(data) }
                    , { data: 'acDetProyGemelo', title: 'C.C.', createdCell: (td, data, rowData, row, col) => $(td).html(data === "N/A" ? rowData.cc : data) }
                    , { data: 'ac', title: 'A.C.' }
                    , {
                        data: 'id', width: '2%', createdCell: function (td, data, rowData, row, col) {
                            btnEliminar = $(`<button>`, {
                                type: "button",
                                class: "btn btn-xs btn-danger btnEliminar",
                                html: `<i class="fas fa-times"></i>`
                            });
                            $(td).html(btnEliminar);
                        }
                    }
                ]
                , initComplete: function (settings, json) {
                    tblProyDetalle.on('click', '.detClick', function (event) {
                        let row = $(this).closest("tr");
                        data = dtProyDetalle.row(row).data();
                        setDetForm(data);
                        tblProyDetalle.find("tbody tr").removeClass("selected");
                        row.addClass("selected");
                    });
                    tblProyDetalle.on('click', '.btnEliminar', function (event) {
                        let row = $(this).closest("tr");
                        data = dtProyDetalle.row(row).data();
                        setEliminarDetProyeccionCierre(data.id, row[0]._DT_RowIndex);
                    });
                }
            });
        }
        function setTblProyeccionCierreAutomatico(data, tipo) {
            let config = null;
            switch (tipo) {
                case 1:
                case 2:
                case 5:
                case 7:
                case 8:
                    config = {
                        columns: [
                            {
                                data: 'esActivo', title: 'Estado', width: '2%', createdCell: (td, data, type, row, meta) => {
                                    let chb = $(`<input>`, { type: 'checkbox', checked: data });
                                    $(td).html(chb);
                                }
                            }
                            , { data: 'ac', title: 'A.C.', width: '8%' }
                            , { data: 'cc', title: 'C.C.', width: '8%' }
                            , {
                                data: 'descripcion', title: 'Cliente', createdCell: (td, data, type, row, meta) => {
                                    if (type.id == 0) {
                                        data = `${type.numcte} - ${data}`;
                                    }
                                    $(td).html(data);
                                }
                            }
                            , {
                                data: 'fechaFactura', title: 'Fecha', width: '2%', createdCell: (td, data, type, row, meta) => {
                                    data = $.toDate(data);
                                    $(td).html(data);
                                }
                            }
                            , { data: 'factura', title: 'Factura', width: '2%' }
                            , { data: 'monto', title: 'Monto', width: '2%', createdCell: (td, data, rowData, row, col) => $(td).addClass("text-right").html(maskNumero(data)) },
                        ]
                    };
                    break;
                case 3:
                case 4:
                case 6:
                    config = {
                        columns: [
                            {
                                data: 'esActivo', title: 'Estado', width: '2%', createdCell: (td, data, type, row, meta) => {
                                    let chb = $(`<input>`, { type: 'checkbox', checked: data });
                                    $(td).html(chb);
                                }
                            }
                            , { data: 'ac', title: 'A.C.', width: '8%' }
                            , { data: 'cc', title: 'C.C.', width: '8%' }
                            , {
                                data: 'descripcion', title: 'Proveedor', createdCell: (td, data, type, row, meta) => {
                                    if (type.id == 0) {
                                        data = `${type.numpro} - ${data}`;
                                    }
                                    $(td).html(data);
                                }
                            }
                            , {
                                data: 'fechaFactura', title: 'Fecha', width: '2%', createdCell: (td, data, type, row, meta) => {
                                    data = $.toDate(data);
                                    $(td).html(data);
                                }
                            }
                            , { data: 'factura', title: 'Factura', width: '2%' }
                            , { data: 'monto', title: 'Monto', width: '2%', createdCell: (td, data, rowData, row, col) => $(td).addClass("text-right").html(maskNumero(data)) },
                        ]
                    };
                    break;
                default:
                    break;
            }
            dtProyeccionCierreAutomatico = tblProyeccionCierreAutomatico.DataTable({
                destroy: true
                , language: dtDicEsp
                , columns: config.columns
                , createdRow: (tr, data) => $(tr).addClass(data.id == 0 ? "noEsGuardado" : "esGuardado")
                , data
            });
        }
        function initForm() {
            idConceptoDir = 0;
            idDetProyGemelo = 0;
            areaCuenta = "TODOS";
            limparDetForm();
            divDetalle.addClass("hidden");
            divSelDetCC.addClass("hidden");
            selAC.fillCombo(getCC, null, false, areaCuenta);
            selAC.select2();
            selCC.fillCombo(getComboAreaCuentaConCentroCostos, null, true, null);
            selCC.select2();
            selDetAC.fillCombo(getCC, null, false, "N/A");
            selDetCC.fillCombo(getComboAreaCuentaConCentroCostos, null, true, null);
            selDetCC.prepend(`<option value=${"N/A"}>N/A</option>`);
            txtDetGrupo.fillCombo(getLstGrupoReserva, null, true, null);
            selDetNaturaleza.fillCombo(getCboNaturaleza, null, true, null);
            setMaxCorte();
            txtDetFecha.datepicker({
                firstDay: 0,
                showOtherMonths: true,
                selectOtherMonths: true,
            }).datepicker("setDate", new Date());
            initDataTblProyCierre();
            initDataTblProyDetalle();
            initDataTblGrupoReserva();
        }
        function setDetalleDos() {
            setNaturalezaPorOperador();
            txtDetGrupo.fillCombo(getLstGrupoReserva, null, true, null);
            txtDetFecha.datepicker('option', 'maxDate', endDate.toLocaleDateString());
            txtDetFecha.datepicker('option', 'minDate', startDate.toLocaleDateString());
            txtDetFecha.datepicker("setDate", endDate);
            divDetalleDos.addClass("hidden");
            if (idConceptoDir == 29) {
                divDetalleDos.removeClass("hidden");
            }
        }
        function setBtnForm() {
            divForm.removeClass("hidden");
            btnAutoGuardar.removeClass("hidden");
            btnDetGuardar.removeClass("hidden");
            btnDetLimpiar.removeClass("hidden");
            txtDetDescripcion.prop("disabled", false);
            txtDetMonto.prop("disabled", false);
            dtProyDetalle.column(4).visible(true);
            dtProyDetalle.column(5).visible(true);
            if ((centroCostos !== "TODOS" && esCC) || (areaCuenta !== "TODOS" && !esCC)) {
                divForm.addClass("hidden");
                btnAutoGuardar.addClass("hidden");
                btnDetGuardar.addClass("hidden");
                btnDetLimpiar.addClass("hidden");
                txtDetDescripcion.prop("disabled", true);
                txtDetMonto.prop("disabled", true);
                dtProyDetalle.column(4).visible(false);
                dtProyDetalle.column(5).visible(false);
            }
        }
        function setNaturalezaPorOperador() {
            if (dtProyCierre !== undefined) {
                let cpto = dtProyCierre.data().toArray().find(cpto => cpto.id == idConceptoDir);
                switch (cpto.operador) {
                    case "+": selDetNaturaleza.val(1); break;
                    case "-": selDetNaturaleza.val(2); break;
                    default: break;
                }
            }
        }
        function doneGuardar() {
            setLstCptoCierre();
            setMensajeConfirmacion();
            mdlProyeccionCierre.modal("hide");
        }
        function setMensajeConfirmacion() {
            let periodo = txtFecha.val(),
                cc = selAC.find(`option[value="${areaCuenta}"]`).text();
            NotificacionGeneral("Aviso", `Proyecciones a la ${periodo}  del area cuenta ${cc} se han guardado correctamente.`);
        }
        function setSumaManualCptoCierre({ lst, total }) {
            let cpto = dtProyCierre.data().toArray().find(c => c.id == lst[0].idConceptoDir);
            tblProyCierre.find(`td:contains("${cpto.concepto}")`).next("td").find(".btnManual").text(maskNumero(total));
        }
        function setRestaManualCptoCierre(data) {
            let cpto = dtProyCierre.data().toArray().find(c => c.id == data.idConceptoDir);
            cpto.monto -= data.monto;
            tblProyCierre.find(`td:contains("${cpto.concepto}")`).next("td").find(".btnManual").text(maskNumero(cpto.monto));
        }
        function setDetForm({ id, idDetProyGemelo, descripcion, monto, fecha, naturaleza, grupo, acDetProyGemelo, cc }) {
            if (typeof (fecha) == 'string' && fecha.includes("/Date(")) {
                fecha = $.toDate(fecha);
            }
            txtDetGrupo.val(grupo);
            selDetAC.val(acDetProyGemelo);
            selDetCC.val(cc);
            selDetNaturaleza.val(naturaleza);
            txtDetDescripcion.val(descripcion);
            txtDetMonto.val(maskNumero(monto));
            txtDetFecha.datepicker("setDate", fecha);
            btnDetGuardar.data({ id, idDetProyGemelo });
        }
        function limparDetForm() {
            tblProyDetalle.find("tbody tr").removeClass("selected");
            let fecha = new Date();
            if (endDate !== undefined) {
                fecha = endDate.toLocaleDateString();
            }
            setDetForm({
                id: 0,
                descripcion: "",
                monto: 0,
                fecha,
                naturaleza: 1,
                grupo: txtDetGrupo.prop("selectedIndex", 0).val(),
                idDetProyGemelo: 0,
                acDetProyGemelo: "N/A",
                cc: selDetCC.find("option:first").val()
            });
            setNaturalezaPorOperador();
        }
        function getBusqForm() {
            return {
                min: startDate.toLocaleDateString(),
                max: endDate.toLocaleDateString(),
                lstAC: selAC.val(),
                lstCC: selCC.val(),
                idConceptoDir: idConceptoDir,
                grupo: txtDetGrupo.val(),
                esCC: chbAcCc.prop("checked")
            }
        }
        function setDetMonto() {
            let monto = unmaskNumero(txtDetMonto.val());
            if ((monto < 0) && (idConceptoDir == 21 || idConceptoDir == 22 || idConceptoDir == 24)) {
                monto *= -1;
            }
            txtDetMonto.val(maskNumero(monto));
        }
        function setComboBusqueda() {
            let esCC = chbAcCc.prop("checked");
            lblAcCcDescripcion.text(esCC ? "Centro Costo" : "Area Cuenta");
            if (esCC) {
                divAreaCuenta.addClass("hidden");
                divCentroCostos.removeClass("hidden");
                selCC.select2();
            } else {
                divAreaCuenta.removeClass("hidden");
                divCentroCostos.addClass("hidden");
                selAC.select2();
            }
            setLstCptoCierre();
        }
        function getLstDetalle() {
            diaSemana = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 3);
            let lst = [],
                fecha = txtDetFecha.datepicker("getDate"),
                grupo = idConceptoDir === 29 ? $("#txtDetGrupo option:selected").text() : "",
                grupoID = idConceptoDir === 29 ? txtDetGrupo.val() : null,
                id = btnDetGuardar.data().id,
                idDetProyGemelo = btnDetGuardar.data().idDetProyGemelo,
                cpto = dtProyCierre.data().toArray().find(cpto => cpto.id == idConceptoDir),
                cc = selDetCC.val(),
                monto = unmaskNumero(txtDetMonto.val()),
                ac = selDetAC.val(),
                acDet = ac === "N/A" ? "TODOS" : ac;
            if (acDet == "N/A" && cc == "N/A") {
                return;
            }
            if (cpto.operador === "-" && monto > 0 && cpto.id !== 23 && cpto.id !== 26) {
                monto *= -1;
            }
            
            if (areaCuenta === "TODOS" && acDet !== "N/A") {
                lst.push({
                    id: idDetProyGemelo,
                    idConceptoDir: idConceptoDir,
                    descripcion: txtDetDescripcion.val(),
                    monto: monto,
                    naturaleza: selDetNaturaleza.val(),
                    ac: acDet,
                    cc: cc,
                    fecha: fecha,
                    semana: diaSemana.noSemana(),
                    anio: fecha.getFullYear(),
                    grupo: grupo,
                    grupoID: grupoID,
                    idDetProyGemelo: id,
                    tipo: 0, //manual
                    esActivo: true,
                    factura: 0
                });
            }
            else{
                lst.push({
                    id: id,
                    idConceptoDir: idConceptoDir,
                    descripcion: txtDetDescripcion.val(),
                    monto: monto,
                    naturaleza: selDetNaturaleza.val(),
                    ac: areaCuenta,
                    cc: cc,
                    fecha: fecha,
                    semana: diaSemana.noSemana(),
                    anio: fecha.getFullYear(),
                    grupo: grupo,
                    grupoID: grupoID,
                    idDetProyGemelo: idDetProyGemelo,
                    tipo: 0, //manual
                    esActivo: true,
                    factura: 0,
                });
            }
            return lst;
        }
        function getLstAutomatico() {
            diaSemana = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 3);
            let lst = [],
                operador = dtProyCierre.data().toArray().find(cpto => cpto.id == idConceptoDir).operador;
            dtProyeccionCierreAutomatico.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let row = this.node()
                    , data = this.data();
                if ($(row).find(`input[type="checkbox"]`).prop("checked")) {
                    if (data.id == 0) {
                        if (data.numcte > 0) {
                            data.descripcion = `${data.numcte} - ${data.descripcion}`;
                        }
                        if (data.numpro > 0) {
                            data.descripcion = `${data.numpro} - ${data.descripcion}`;
                        }
                    }
                    data.fechaFactura = $.toDate(data.fechaFactura)
                    data.fecha = endDate;
                    data.semana = diaSemana.noSemana();
                    data.anio = endDate.getFullYear();
                    data.naturaleza = operador == "+" ? 1 : 2;
                    data.grupo = '';
                    lst.push(data);
                }
            });
            return lst;
        }
        function setChbAutomatico() {
            let valor = this.value === 'true';
            dtProyeccionCierreAutomatico.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let row = this.node();
                $(row).find(`input[type="checkbox"]`).prop("checked", valor);
            });
        }
        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                txtFecha.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }
        function setSemanaSelecionada() {
            date = txtFecha.datepicker('getDate');
            prevDom = date.getDate() - (date.getDay() + 14) % 7;
            startDate = new Date(date.getFullYear(), date.getMonth(), prevDom);
            endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 6);
            diaSemana = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 3);
            noSemana = diaSemana.noSemana();
            txtFecha.val(`Semana ${noSemana} - ${startDate.toLocaleDateString()} - ${endDate.toLocaleDateString()}`);
            selectCurrentWeek();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.FlujoEfectivoArrendadora.ProyeccionDeCierre = new ProyeccionDeCierre();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();