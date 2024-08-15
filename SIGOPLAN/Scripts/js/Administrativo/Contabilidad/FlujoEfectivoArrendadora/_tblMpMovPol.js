(() => {
    $.namespace('Administrativo.FlujoEfectivoArrendadora._tblMpMovPol');
    _tblMpMovPol = function () {
        var startDateCorte, endDateCorte;
        const dpMpCorte = $('#dpMpCorte');
        const btnMpCorte = $('#btnMpCorte');
        const tblMpMovPol = $('#tblMpMovPol');
        const getCorte = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getCorte');
        const guardarCorte = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/guardarCorte');
        const guardarMovPol = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/guardarMovPol');
        const getCorteEstado = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getCorteEstado');
        const getIniTblPoliza = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getIniTblPoliza');
        let lstTmBancario, lstConcepto, lstRel = [], lstConceptoDir, lstRelDir;
        let init = async () => {
            setPeriodoCorte();
            setCboGrupoConcepto();
            initDataTblMpMovPol();
            btnMpCorte.click(setCorte);
        }
        setCboGrupoConcepto = async () => {
            try {
                response = await ejectFetchJson(getIniTblPoliza);
                if (response.success) {
                    lstConcepto = response.lstConcepto;
                    lstRel = response.lstRel;
                    lstConceptoDir = response.lstConceptoDir;
                    lstRelDir = response.lstRelDir;
                    lstTmBancario = response.lstTmBancario;
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        setPeriodoCorte = async () => {
            try {
                response = await ejectFetchJson(getCorte);
                if (response.success) {
                    let arrMin = $.toDate(response.minCorte).split('/')
                        , arrMax = $.toDate(response.maxCorte).split('/');
                    minFechaCorte = new Date(arrMin[2], +arrMin[1] - 1, arrMin[0]);
                    maxFechaCorte = new Date(arrMax[2], +arrMax[1] - 1, arrMax[0]);
                    dpMpCorte.datepicker({
                        firstDay: 0,
                        showOtherMonths: true,
                        selectOtherMonths: true,
                        minDate: minFechaCorte,
                        maxDate: maxFechaCorte,
                        onSelect: function (dateText, inst) {
                            setSemanaSelecionadaCorte();
                            setEstadoFlujo();
                        },
                        beforeShowDay: function (date) {
                            var cssClass = '';
                            if (date >= startDateCorte && date <= endDateCorte) {
                                cssClass = 'ui-datepicker-current-day';
                            }
                            return [true, cssClass];
                        }
                    }).datepicker("setDate", new Date());
                    setSemanaSelecionadaCorte();
                    setEstadoFlujo();
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        setEstadoFlujo = async () => {
            try {
                let busq = getMpFechaCorte();
                response = await ejectFetchJson(getCorteEstado, busq);
                if (response.success) {
                    btnMpCorte.data().esCorte = response.esCorte;
                    btnMpCorte.removeClass("btn-danger").html("<i class='fas fa-calendar'></i> Corte");
                    if (response.esCorte) {
                        btnMpCorte.addClass("btn-danger").html("<i class='fas fa-calendar-times'></i> Corte");
                    }
                }
            } catch (o_O) { }
        }
        setGuardarCorte = async () => {
            try {
                AlertaGeneral("Aviso", "Favor de esperar. Tomará unos minutos.");
                let esCorte = !btnMpCorte.data().esCorte;
                response = await ejectFetchJson(guardarCorte, {
                    busq: getMpFechaCorte()
                    , esCorte: esCorte
                });
                if (response.success) {
                    AlertaGeneral("Aviso", `Corte de ${dpMpCorte.val()} para el flujo de efectivo generado con éxito.`);
                    setEstadoFlujo();
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        setCorte = () => {
            let esCorte = btnMpCorte.data().esCorte;
            AlertaAceptarRechazarNormal(
                `Confirmación de corte`
                , `Se ${esCorte ? "cancelará" : "realizará"} el corte de pólizas para el mes de ${dpMpCorte.val()}. ¿Deseas continuar?`
                , setGuardarCorte
                , null
            )
        }
        setGuardar = async () => {
            try {
                let lst = getMpLstMovPol()
                if (esMovpolValida(lst)) {
                    var scheme = { lst: new Array() };
                    $.sm_SplittedSave(guardarMovPol, lst, scheme, 10, doneGuardar);
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        function doneGuardar() {
            setEstadoFlujo();
            setPeriodoCorte();
            AlertaGeneral("Aviso", `Conceptos de movimientos fueron guardados con éxito.`);
        }
        getMpFechaCorte = () => {
            return {
                min: startDateCorte.toLocaleDateString()
                , max: endDateCorte.toLocaleDateString()
            };
        }
        getMpLstMovPol = () => {
            let lst = [];
            dtMpMovPol.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let row = $(this.node())
                    , data = this.data()
                    , cboConcepto = row.find(`.concepto`)
                    , cboConceptoDir = row.find(`.conceptoDir`)
                    , esDisabled = cboConcepto[0].disabled
                    , esDisabledDir = cboConceptoDir[0].disabled
                    , cboTm = row.find('.itm');
                cboTm.prop("disabled", false);
                if (esDisabled) {
                    cboConcepto[0].disabled = false;
                }
                if (esDisabledDir) {
                    cboConceptoDir[0].disabled = false;
                }
                let idConcepto = +cboConcepto.val(),
                    idConceptoDir = +cboConceptoDir.val();
                cboConcepto[0].disabled = esDisabled;
                data.itm = +cboTm.val();
                if ((data.id > 0 || idConcepto > 0 || idConceptoDir > 0) && data.itm > 0) {
                    data.idConcepto = idConcepto;
                    data.idConceptoDir = idConceptoDir;
                    lst.push(data);
                }
            });
            return lst;
        }
        esMovpolValida = lst => {
            let esValido = true;
            if (lst.length == 0) {
                esValido = false;
                return esValido;
            }
            lst.forEach(movpol => {
                switch (true) {
                    case movpol.year == 0:
                    case movpol.mes == 0:
                    case movpol.tp.length == 0:
                    case movpol.poliza == 0:
                    case movpol.linea == 0:
                    case movpol.cta == 0:
                        esValido = false;
                        break;
                    default:
                        break;
                }
                if (!esValido) {
                    return;
                }
            });
            return esValido;
        }
        limpiarLstData = () => {
            dtMpMovPol.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let data = this.data();
                data.fechapol = data.fecha;
                delete data["ctaDesc"];
                delete data["ctaStr"];
                delete data["folio"];
                delete data["itmDesc"];
                delete data["proveedor"];
            });
        }
        function initDataTblMpMovPol() {
            dtMpMovPol = tblMpMovPol.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    { data: 'folio', width: '10%' }
                    , { data: 'ctaDesc', width: '13%' }
                    , { data: 'orden_compra', width: '5%' }
                    , { data: 'ac' }
                    , { data: 'fecha', width: '5%' }
                    , { data: 'concepto', width: '13%' }
                    , { data: 'monto', width: '5%', createdCell: (td, data, rowData, row, col) => $(td).html(maskNumero(data)).addClass("text-right") }
                    , {
                        data: 'itm', width: '16%', createdCell: function (td, data, rowData, row, col) {
                            let cbo = $("<select>").addClass("form-control itm")
                                , div = $("<div>").addClass("input-group")
                                , span = $("<span>").addClass("input-group-btn")
                                , btn = $("<button>").addClass("btn btn-default btnItmDefault")
                                , i = $("<i>").addClass("fab fa-maxcdn");
                            btn.append(i);
                            span.append(btn);
                            div.append(span);
                            cbo.fillComboItems(lstTmBancario, null, data);
                            div.append(cbo);
                            $(td).html(div);
                        }
                    }
                    , {
                        data: 'idConcepto', width: '16%', createdCell: function (td, data, rowData, row, col) {
                            let cbo = $("<select>").addClass("form-control concepto")
                            cbo = crearCboConcepto(cbo, rowData.itm, data);
                            $(td).html(cbo);
                        }
                    }
                    , {
                        data: 'idConceptoDir', width: '16%', createdCell: function (td, data, rowData, row, col) {
                            let cbo = $("<select>").addClass("form-control conceptoDir")
                            cbo = crearCboConceptoProyectado(cbo, rowData.itm, data);
                            $(td).html(cbo);
                        }
                    }
                ]
                , initComplete: function () {
                    tblMpMovPol.on("change", ".itm", function () {
                        let row = $(this).closest("tr")
                            , data = dtMpMovPol.row(row).data()
                            , cbo = row.find(".concepto")
                            , cboDir = row.find(".conceptoDir");
                        cbo = crearCboConcepto(cbo, +this.value, data.idConcepto);
                        cboDir = crearCboConceptoProyectado(cboDir, +this.value, data.idConceptoDir)
                    });
                    tblMpMovPol.on("click", ".btnItmDefault", function () {
                        let row = $(this).closest("tr")
                            , mov = dtMpMovPol.row(row).data()
                            , itmOri = mov.itmOri
                            , cbo = row.find(".concepto")
                            , cboDir = row.find(".conceptoDir");
                        row.find(".itm").val(itmOri);
                        cbo = crearCboConcepto(cbo, itmOri, mov.idConcepto);
                        cboDir = crearCboConceptoProyectado(cboDir, itmOri, mov.idConceptoDir)
                    });
                }
            });
        }
        crearCboConcepto = (cbo, itm, idConcepto) => {
            let lstGpoConcepto = createLstGrupoConcepto(itm, lstConcepto, lstRel);
            cbo.empty();
            cbo.prop("disabled", false);
            cbo.fillComboItemsGroup(lstGpoConcepto, null, idConcepto);
            if (esUnUnicoConcepto(lstGpoConcepto)) {
                idConcepto = lstGpoConcepto[0].options[0].Value;
                cbo.val(idConcepto);
                cbo.prop("disabled", true);
            }
            return cbo;
        }
        crearCboConceptoProyectado = (cbo, itm, idConcepto) => {
            let lstGpoConcepto = createLstGrupoConcepto(itm, lstConceptoDir, lstRelDir);
            cbo.empty();
            cbo.prop("disabled", false);
            cbo.fillComboItemsGroup(lstGpoConcepto, null, idConcepto);
            if (esUnUnicoConcepto(lstGpoConcepto)) {
                idConcepto = lstGpoConcepto[0].options[0].Value;
                cbo.val(idConcepto);
                cbo.prop("disabled", true);
            }
            return cbo;
        }
        esUnUnicoConcepto = (lstGpo) => {
            return lstGpo.length == 1 && lstGpo.map(gpo => gpo.options).length === 1;
        }
        createLstGrupoConcepto = (itm, cboConcepto, cboRel) => {
            let lstCpto = []
                , lstGpoConcepto = []
                , lstTm = cboRel.filter(rel => rel.tm === itm).map(rel => rel.idConcepto);
            lstTm.forEach(idConcepto => {
                cboConcepto.forEach(cpto => {
                    if (cpto.id === idConcepto) {
                        lstCpto.push(cpto);
                    }
                });
            });
            let lstGpo = groupBy(lstCpto, cpto => cpto.idpadre);
            Array.from(lstGpo, ([key, value]) => {
                let label = cboConcepto.find(cpto => cpto.id === key).Concepto
                    , options = value.map(opt => {
                        Value = opt.id
                            , Text = opt.Concepto
                        return { Value, Text }
                    });
                lstGpoConcepto.push({ label, options });
            });
            return lstGpoConcepto;
        }
        groupBy = (list, keyGetter) => {
            const map = new Map();
            list.forEach(item => {
                const key = keyGetter(item);
                const collection = map.get(key);
                if (!collection) {
                    map.set(key, [item]);
                } else {
                    collection.push(item);
                }
            });
            return map;
        }
        setSemanaSelecionadaCorte = () => {
            if (dpMpCorte.length == 0) {
                date = $(mpInicial).datepicker().datepicker('getDate');
            } else {
                date = dpMpCorte.datepicker('getDate');
            }
            prevDom = date.getDate() - (date.getDay() + 7) % 7;
            startDateCorte = new Date(date.getFullYear(), date.getMonth(), prevDom);
            endDateCorte = new Date(startDateCorte.getFullYear(), startDateCorte.getMonth(), startDateCorte.getDate() - startDateCorte.getDay() + 6);
            fechaSemana = new Date(startDateCorte.getFullYear(), startDateCorte.getMonth(), startDateCorte.getDate() - startDateCorte.getDay() + 3);
            noSemana = fechaSemana.noSemana();
            dpMpCorte.val(`Semana ${noSemana} - ${startDateCorte.toLocaleDateString()} - ${endDateCorte.toLocaleDateString()}`);
            selectCurrentWeekCorte();
        }
        selectCurrentWeekCorte = () => {
            window.setTimeout(function () {
                dpMpCorte.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.FlujoEfectivoArrendadora._tblMpMovPol = new _tblMpMovPol();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();