    (() => {
    $.namespace('Administrativo.Contabilidad.Propuesta.Concentrado');
    Concentrado = function () {
        let startDate,
            endDate;
        const dpSemana = $('#dpSemana');
        const btnExcel = $('#btnExcel');
        const mdlReserva = $('#mdlReserva');
        const mdlRelCtaDiv = $('#mdlRelCtaDiv');
        const btnConciliar = $('#btnConciliar');
        const selResAccion = $('#selResAccion');
        const btnResAccion = $('#btnResAccion');
        const lblResProlateo = $('#lblResProlateo');
        const tblConcentrado = $('#tblConcentrado');
        const btnExcelGlobal = $('#btnExcelGlobal');
        const tblResProlateo = $('#tblResProlateo');
        const mdlResProlateo = $('#mdlResProlateo');
        const mdlResProlateoImpIva = $('#mdlResProlateoImpIva');
        const tblResProlateoImpIva = $('#tblResProlateoImpIva');
        const btnGuardarResProlateo = $('#btnGuardarResProlateo');
        const btnGuardarResProlateoImpIva = $('#btnGuardarResProlateoImpIva');
        let init = () => {
            initForm();
            btnExcel.click(exportExcel);
            btnResAccion.click(setAccion);
            btnConciliar.click(clkConciliacion);
            btnExcelGlobal.click(exportExcelGlobal); //desde _mdlSaldoGlobal.js
            btnGuardarResProlateo.click(clkGuardarResProlateo);
            btnGuardarResProlateoImpIva.click(clkGuardarResProlateoImpIva);
            mdlReserva.on('shown.bs.modal', function () {
                dtReserva.columns.adjust();
            });
            mdlReserva.on('hidden.bs.modal', function () {
                setTblConcentrado();
                setTblSaldosActuales();
            });
            mdlRelCtaDiv.on('hidden.bs.modal', function () {
                refreshGetLstAccionReservas();
                setTblConcentrado();
                setTblSaldosActuales();
            });
        }
        const exportConcentrado = '/Administrativo/Propuesta/exportConcentrado';
        const acctionToProrrateo = new URL(window.location.origin + '/Administrativo/Propuesta/acctionToProrrateo');
        const getConcentrado = () => $.post('/Administrativo/Propuesta/getConcentrado', { busq: BusqConcentradoDTO() });
        const setConciliacion = () => $.post('/Administrativo/Propuesta/setConciliacion', { busq: BusqConcentradoDTO() });
        const guardarReserva = () => $.post('/Administrativo/Propuesta/guardarReserva', { lst: getlstResProlateo() });
        const guardarReservaImpIva = () => $.post('/Administrativo/Propuesta/guardarReservaImpIva', { lst: getlstResProlateoImpIva() });
        function exportExcel() {
            exportUrlToFile(exportConcentrado);
        }
        function clkGuardarResProlateo() {
            try {
                let obj = getReservaBusq();
                AlertaAceptarRechazar("Aviso", `¿Desea guardar las reservas para ${$(`#selResTipo option[value="${obj.tipo}"]`)[0].innerText}, a la fecha ${obj.fecha}?`, guardarReserva, null)
                .then(boton => {
                    guardarReserva().then(response => {
                        if (response.success) {
                            AlertaGeneral("Aviso", "Reservas guardadas con éxito.");
                            eventBusqReserva();
                            mdlResProlateo.modal("toggle");
                            mdlReserva.modal("toggle");   
                        }
                    });
                });
            } catch (o_O) { }
        }
        function clkGuardarResProlateoImpIva() {
            try {
                let obj = getReservaBusq();
                AlertaAceptarRechazar("Aviso", `¿Desea guardar las reservas para Pago de Impuestos de Iva, a la fecha ${obj.fecha}?`, guardarReservaImpIva, null)
                .then(boton => {
                    guardarReservaImpIva().then(response => {
                        if (response.success) {
                            AlertaGeneral("Aviso", "Reservas guardadas con éxito.");
                            eventBusqReserva();
                            mdlResProlateoImpIva.modal("toggle");
                            mdlReserva.modal("toggle");   
                        }
                    });
                });
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        function clkConciliacion() {
            AlertaAceptarRechazar("Aviso", `El gran total será conciliado. ¿Desea continuar?`, setConciliacion, null)
                .then(boton => {
                    setConciliacion().then(response => {
                        if (response.success) {
                            AlertaGeneral("Aviso", "Conciliación realizada con éxito.");
                        }
                    });
                }).catch(o_O => { });
        }
        function setTblConcentrado() {
            getConcentrado()
                .done(response => {
                    dtConcentrado.clear();
                    if (response.success){
                        dtConcentrado.rows.add(response.lstConcentrado).draw();
                    }
                })
        }
        async function setAccion(){
            try {
                let accion = selResAccion.val()
                   ,catRes = selResAccion.find(`option[value="${accion}"]`).data().prefijo
                   ,lst = ConcentradoToLst(catRes.id);
                response = await ejectFetchJson(acctionToProrrateo, {accion, lst});
                if (response.success) {
                    setSemanaConcentradoToSemanaReserva();
                    if (response.esPorcentaje) {
                        sendConcetradoToReservaImpuesto(response.lst);
                    } else {
                        sendConcetradoToReserva(response.lst);   
                    }
                }
                else {
                    AlertaGeneral(`Aviso`, `No hay movimientos para la reserva de ${catRes.descripcion}.`); 
                }
            } catch (o_O) {console.log(o_O.message)}
            
        }
        function ConcentradoToLst(idCatReserva)
        {
            let lst = [];
            tblConcentrado.find(`tbody tr`).not(`.esSaldoConciliado`).each((i ,tr) => {
                let combo = $(tr).find(`td:last .form-control`);
                if (combo !== undefined) {
                    let esReserva = combo.find(`option[value="${idCatReserva}"]`).prop("selected");
                    if (esReserva) {
                        let data = dtConcentrado.row(tr).data();
                        lst.push(data);   
                    }
                }
            });
            var map = lst.map(concentrado => ({
                abono: concentrado.abono,
                beneficiario: concentrado.beneficiario,
                cargo: concentrado.cargo,
                cc: concentrado.cc,
                concepto: concentrado.concepto,
                fecha: $.toDate(concentrado.fecha),
                noCheque: concentrado.noCheque,
                obra: concentrado.obra,
                poliza: concentrado.poliza,
                saldo: concentrado.saldo,
                sonDolares: concentrado.sonDolares,
                tipo: concentrado.tipo,
                tipoReserva: concentrado.tipoReserva,
                tipoReservaAutomatica: concentrado.tipoReservaAutomatica,
                tp: concentrado.tp,
                tm: concentrado.tm
            }));
            return map;
        }
        function sendConcetradoToReserva(lst) {
            $("#selResTipo").val(lst[0].tipo);
            lblResProlateo.text($(`#selResTipo option[value="${lst[0].tipo}"]`).text())
            dtResProlateo.clear();
            dtResProlateo.rows.add(lst).draw(false);
            desResProlateoCCSelecionados();
            mdlResProlateo.modal("show");
        }
        function sendConcetradoToReservaImpuesto(lst) {
            $("#selResTipo").val(lst[0].tipo);
            dtResProlateoImpIva.clear();
            dtResProlateoImpIva.rows.add(lst).draw(false);
            mdlResProlateoImpIva.modal("show");
        }
        function setSemanaConcentradoToSemanaReserva(){
            $("#dpResFecha").datepicker("setDate", dpSemana.datepicker("getDate"));
            setSemanaReservaSelecionada(); //Desde _mdlReservas.js
        }
        function BusqConcentradoDTO() {
            let date = dpSemana.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 6);
            return {
                min: startDate.toLocaleDateString(),
                max: endDate.toLocaleDateString()
            }
        }
        function getlstResProlateo() {
            let lst = [];
            obj = getReservaBusq();
            dtResProlateo.rows().iterator('row', function (context, index) {
                let node = $(this.row(index).node()),
                    data = {
                        tipo: obj.tipo,
                        fecha: obj.fecha,
                        cc: node.find(".cc").data().cc,
                        cargo: unmaskNumero(node.find(".cargo").val()),
                        abono: unmaskNumero(node.find(".abono").val()),
                        global: 1
                    },
                    esValida = validaReserva(data);
                if (esValida) {
                    lst.push(data);
                } else {
                    return;
                }
            });
            return lst;
        }
        function getlstResProlateoImpIva() {
            let lst = [];
            obj = getReservaBusq();
            dtResProlateoImpIva.rows().iterator('row', function (context, index) {
                let node = $(this.row(index).node()),
                    porcentaje = unmaskNumero(node.find(".porcentaje").val()),
                    data = {
                        tipo: obj.tipo,
                        fecha: obj.fecha,
                        cc: node.find(".cc").data().cc,
                        cargo: unmaskNumero(node.find(".cargo").val()),
                        abono: unmaskNumero(node.find(".abono").val()),
                        global: porcentaje === null ? 100 : porcentaje
                    },
                    esValida = validaReserva(data);
                if (esValida) {
                    lst.push(data);
                } else {
                    return;
                }
            });
            return lst;
        }
        function initDataTblResProlateo() {
            dtResProlateo = tblResProlateo.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    {
                        data: 'cc', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<select>`);
                            $(td).find(`select`).addClass(`form-control reserva cc`);
                            $(td).find(`select`).fillCombo('/Administrativo/Propuesta/getCCReservaGlobal', getReservaBusq(), false, null);
                            if (data.length === 3) {
                                $(td).find(`select > option[value="${data}"]`).prop(`selected`, true);
                            }
                            $(td).find(`select`).data().cc = data;
                        }
                    },
                    {
                        data: 'cargo', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right reserva cargo`);
                            $(td).find(`input`).val(maskNumero2D(data));
                        }
                    },
                    {
                        data: 'abono', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right reserva abono`);
                            $(td).find(`input`).val(maskNumero2D(data));
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblResProlateo.on('change', '.reserva', function (event) {
                        let row = $(this).parents('tr'),
                            cc = row.find('.cc').val();
                        if (cc === null) {
                            row.find('.cc option').prop('disabled', false);
                            cc = row.find('.cc').val();
                        }
                        //#region montos
                        let cargo = unmaskNumero(row.find('.cargo').val()),
                            abono = unmaskNumero(row.find('.abono').val());
                        //#endregion
                        row.find('.cargo').val(maskNumero2D(cargo));
                        row.find('.abono').val(maskNumero2D(abono));
                        row.find('.cc').data().cc = cc;
                        desResProlateoCCSelecionados();
                        dtResProlateo.draw();
                    });
                }
            });
        }
        function initDataTblResProlateoImpIva() {
            dtResProlateoImpIva = tblResProlateoImpIva.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    {
                        data: 'cc', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<select>`);
                            $(td).find(`select`).addClass(`form-control impIva cc`);
                            $(td).find(`select`).fillCombo('/Administrativo/Propuesta/getCCReservaGlobal', getReservaBusq(), false, null);
                            if (data.length === 3) {
                                $(td).find(`select > option[value="${data}"]`).prop(`selected`, true);
                            }
                            $(td).find(`select`).data().cc = data;
                            $(td).find(`select`).prop("disabled", true);
                        }
                    },
                    {
                        data: 'cargo', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right impIva cargo`);
                            $(td).find(`input`).val(maskNumero2D(data));
                            $(td).find(`input`).prop("disabled", true);
                        }
                    },
                    {
                        data: 'abono', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right impIva abono`);
                            $(td).find(`input`).val(maskNumero2D(data));
                            $(td).find(`input`).prop("disabled", true);
                        }
                    },
                    {
                        data: 'porcentaje', createdCell: function (td, data, rowData, row, col) {
                            let div = $(`<div>`).addClass(`input-group`)
                               ,input = $(`<input>`).addClass(`form-control text-right impIva porcentaje`)
                               ,span = $(`<span>`).addClass(`input-group-addon`);
                            span.text(`%`);
                            input.val(maskNumero2D(data));
                            if (rowData.cc.includes("R")) {
                                input.val(maskNumero2D(100));
                                input.prop("disabled", true);
                            }
                            div.append(input).append(span);
                            $(td).html(div);
                        }
                    },
                    {
                        data: 'porcentaje', createdCell: function (td, data, rowData, row, col) {
                            let total = (rowData.cargo - rowData.abono) * (rowData.porcentaje/100)
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right impIva total`);
                            $(td).find(`input`).val(maskNumero2D(total));
                            $(td).find(`input`).prop("disabled", true);
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblResProlateoImpIva.on('change', '.impIva', function (event) {
                        let row = $(this).parents('tr');
                        //#region individual
                        let cargo = unmaskNumero(row.find('.cargo').val()),
                            abono = unmaskNumero(row.find('.abono').val()),
                            impIva = unmaskNumero(row.find('.porcentaje').val()),
                            total = (cargo - abono)*(impIva/100);
                        row.find('.cargo').val(maskNumero2D(cargo));
                        row.find('.abono').val(maskNumero2D(abono));
                        row.find('.porcentaje').val(maskNumero2D(impIva));
                        row.find('.total').val(maskNumero2D(total));
                        //#endregion
                        setTotalImpIva();
                    });
                }
            });
        }
        function setTotalImpIva() {
            //region autosuma
            let totalR = cargoR = abonoR = 0;
            tblResProlateoImpIva.find(`tbody tr`).each(function (context, index) {
                let row = $(this),
                    cc = row.find('.cc').data().cc,
                    cargo = unmaskNumero(row.find('.cargo').val()),
                    abono = unmaskNumero(row.find('.abono').val()),
                    impIva = unmaskNumero(row.find('.porcentaje').val());
                if (!cc.includes("R")) {
                    cargoR += cargo;
                    abonoR += abono;
                    totalR += (abono-cargo)*(impIva/100);   
                }
            });
        //#endregion
        //region total
            tblResProlateoImpIva.find(`tbody tr`).each(function (context, index) {
                let row = $(this),
                    cc = row.find('.cc').data().cc;
                if (cc.includes("R")) {
                    row.find('.cargo').val(maskNumero2D(abonoR));
                    row.find('.abono').val(maskNumero2D(cargoR));
                    row.find('.total').val(maskNumero2D(abonoR-cargoR));
                }
            });
        //#endregion
        }
        function desResProlateoCCSelecionados() {
            let ccDeshabilitaddo = [];
            //#region obtener cc a deshabilitar
            dtResProlateo.rows().iterator('row', function (context, index) {
                let row = $(this.row(index).node()),
                    cc = row.find('.cc').data().cc;
                if (cc.length === 3) {
                    ccDeshabilitaddo.push(cc);
                }
            });
            //#endregion
            //#region deshabilitar cc
            dtResProlateo.rows().iterator('row', function (context, index) {
                let row = $(this.row(index).node());
                var sel = row.find('.cc');
                let opHab = sel.find(`option`);
                opHab.prop('disabled', false);
                ccDeshabilitaddo.forEach(ccDes => {
                    let opDes = sel.find(`option[value="${ccDes}"]`);
                    opDes.prop('disabled', true);
                });
            });
            //#endregion
        }
        function initDataTblConcentrado() {
            dtConcentrado = tblConcentrado.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                createdRow: function (tr, data) { setTipoConcentradoColor(tr, data); },
                columns: [
                    {
                        data: 'fecha', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(data.parseDate().toLocaleDateString());
                        }
                    },
                    { data: 'beneficiario' },
                    { data: 'concepto' },
                    { data: 'obra' },
                    {
                        data: 'noCheque', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(data).addClass('text-right')
                            if (rowData.sonDolares)
                                $(td).html(data).addClass('sonDolares');
                        }
                    },
                    {
                        data: 'cargo', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(maskNumero(data)).addClass('text-right');
                        }
                    },
                    {
                        data: 'abono', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(maskNumero(data)).addClass('text-right');
                        }
                    },
                    {
                        data: 'saldo' ,searching: false ,createdCell: function (td, data, rowData, row, col) {
                            $(td).html(maskNumero(data)).addClass('text-right');
                        }
                    },
                    {
                        data: 'tipoReservaAutomatica', createdCell: function (td, data, rowData, row, col) {
                            let tienneReseva = rowData.items !== null && rowData.items.length > 0
                               ,tieneMontos = (rowData.cargo - rowData.abono) != 0;
                            if (tienneReseva && tieneMontos) {
                                let select = $(`<select>`);
                                select.addClass(`form-control`);
                                select.fillComboItems(rowData.items, null, data === 0 ? "" : data);  
                                $(td).html(select);   
                            }
                            else{
                                $(td).html("");
                            }
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblConcentrado.on('change', 'select', function () {
                        let row = $(this).parent().parent()
                            data = dtConcentrado.row(row).data()
                           ,value = this.value
                           ,color = ""
                           ,idTipoRes = 0
                           ,descObra = "";
                        if (value !== "") {
                            let catReserva = JSON.parse($(this).find(`option[value="${value}"]`).data().prefijo)
                            color = catReserva.hexColor;
                            idTipoRes = catReserva.id;
                            descObra = catReserva.descripcion;
                        } 
                        else {
                            descObra = data.obra;
                        }
                        row.css("background-color" ,color);
                        data.tipoReserva = idTipoRes;
                        row.find(`td:eq(3)`).text(descObra.toUpperCase());
                    });
                }
            });
        }
        setTipoConcentradoColor = (tr, { obra ,tipo ,tipoReserva }) => {
            switch (tipo) {
                case 0: $(tr).addClass("esSaldoConciliado"); break;
                case 1: $(tr).addClass("esCadenaProductiva"); break;
                case 3: $(tr).addClass("esCheque"); break;
                case 5: $(tr).addClass("esInteresesFactoraje"); break;
                case 6: $(tr).addClass("esPolizasDiario"); break;
                case 7: $(tr).addClass("esEstadoCuenta"); break;
                case 8: $(tr).addClass("esMovimientoCliente"); break;
                default:
                    break;
            }
            if (tipoReserva > 0) {
                try {
                    let catReserva = $(`#selResTipo option[value="${tipoReserva}"]`).data().prefijo;
                    if (catReserva !== undefined) {
                        $(tr).css(`background-color`, catReserva.hexColor);
                        $(tr).find(`td:eq(3)`).text(catReserva.descripcion.toUpperCase());
                    }   
                } catch (o_O) { }
            }
            else{
                $(tr).find(`td:eq(3)`).text(obra);
            }
        }
        function setSemanaSelecionada() {
            let date = dpSemana.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 6);
            dpSemana.val(`${startDate.toLocaleDateString()} - ${endDate.toLocaleDateString()}`)
            selectCurrentWeek();
        }
        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                dpSemana.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }
        function refreshGetLstAccionReservas() {
            selResAccion.fillCombo('/Administrativo/Propuesta/getLstAccionReservas', null, false, null);
        }
        function initForm() {
            refreshGetLstAccionReservas();
            dpSemana.datepicker({
                firstDay: 0,
                showOtherMonths: true,
                selectOtherMonths: true,
                onSelect: function (dateText, inst) {
                    setSemanaSelecionada();
                    setTblConcentrado();
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
            }).datepicker("setDate", new Date());
            setSemanaSelecionada();
            initDataTblResProlateo();
            initDataTblResProlateoImpIva();
            initDataTblConcentrado();
            setTblConcentrado();
        }
        String.prototype.parseDate = function () { return new Date(parseInt(this.replace('/Date(', ''))); }
        Date.prototype.parseDate = function () { return this; }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta.Concentrado = new Concentrado();
    }).ajaxStart(() => {
            $.blockUI({
                baseZ: 100000,
                message: 'Procesando...'
            });
        })
        .ajaxStop(() => { $.unblockUI(); });
})();   