(() => {
    $.namespace('Administrativo.Contabilidad.Propuesta._mdlReservas');
    _mdlReservas = function () {
        let startDate ,endDate;
        const dpResFecha = $('#dpResFecha');
        const selResTipo = $('#selResTipo');
        const tblReserva = $('#tblReserva');
        const btnResElimina = $('#btnResElimina');
        const txtReservaTotal = $('#txtReservaTotal');
        const btnGuardarReserva = $('#btnGuardarReserva');
        const btnResAgrega = $('#btnResAgrega');
        let init = () => {
            initForm();
            selResTipo.change(eventBusqReserva);
            btnResElimina.click(eventElimnarReserva);
            btnGuardarReserva.click(eventGuardarReserva);
            btnResAgrega.click(eventValidaUltimaReserva)
        }
        const getBusqDTO = () => $.post('/Administrativo/Propuesta/getBusqDTO');
        const guardarReserva = () => $.post('/Administrativo/Propuesta/guardarReserva', { lst: getFormReserva() });
        const getLstReservas = () => $.post('/Administrativo/Propuesta/getLstReservas', { busq: getReservaBusq() });
        const getTotalReservas = () => $.post('/Administrativo/Propuesta/getTotalReservas', { busq: getReservaBusq() });
        const ElimnarReserva = () => $.post('/Administrativo/Propuesta/ElimnarReserva', { lst: getSelecionadaReservaId() });
        eventBusqReserva = () => {
            let busq = getReservaBusq();
            if (busq.tipo === 0) {
                dtReserva.clear().draw();
                btnGuardarReserva.prop("disabled", true);
                initTotal();
            }
            else {
                getLstReservas()
                    .then(response => {
                        dtReserva.clear();
                        response.lstReservas.push({
                            id: 0,
                            tipo: busq.tipo,
                            fecha: busq.fecha,
                            cc: "",
                            anterior: 0,
                            cargo: 0,
                            abono: 0,
                            global: 0
                        });
                        dtReserva.rows.add(response.lstReservas).draw();
                        btnGuardarReserva.prop("disabled", false);
                        setTotalReserva();
                    });
            }
        }
        function eventGuardarReserva() {
            let obj = getReservaBusq(),
                res = AlertaAceptarRechazar("Aviso", `¿Desea guardar las reservas para ${selResTipo.find(`option[value="${obj.tipo}"]`)[0].innerText}, a la fecha ${obj.fecha}?`, guardarReserva, null);
            res.then(boton => {
                guardarReserva().then(response => {
                    if (response.success) {
                        eventBusqReserva();
                        AlertaGeneral("Aviso", `Reservas de ${selResTipo.find(`option[value="${selResTipo.val()}"]`)[0].innerText}, guardado con éxito.`);
                    }
                });
            }).catch(err => { });
        }
        function eventElimnarReserva() {
            AlertaAceptarRechazar("Aviso", `las reservas seran eliminadas. ¿Desea continuar?`, ElimnarReserva, null)
                .then(boton => {
                    ElimnarReserva().then(response => {
                        eliminaReserva();
                        
                    }).catch(o_O => {
                        eliminaReserva();
                    });
                    AlertaGeneral("Aviso", `Reservas eliminadas.`);
                    deshabilitarCCSelecionados();
                }).catch(err => { });
        }
        function setTotalReserva() {
            getTotalReservas().done(response => {
                if (response.success) {
                    let total = response.totalReservas;
                    txtReservaTotal.val(maskNumero(total));
                    txtReservaTotal.data().total = total;
                }
                else {
                    txtReservaTotal.val(maskNumero(0));
                    txtReservaTotal.data().total = 0;
                }
            });
        }
        getReservaBusq = () => {
            return {
                fecha: dpResFecha.datepicker("getDate").toLocaleDateString(),
                tipo: +(selResTipo.val())
            };
        }
        function getFormReserva() {
            let lst = [],
                obj = getReservaBusq();
            dtReserva.rows().iterator('row', function (context, index) {
                let node = $(this.row(index).node()),
                    data = {
                        id: node.find(".cc").data().id,
                        tipo: obj.tipo,
                        fecha: obj.fecha,
                        cc: node.find(".cc").data().cc,
                        cargo: unmaskNumero(node.find(".cargo").val()),
                        abono: unmaskNumero(node.find(".abono").val())
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
        function    getSelecionadaReservaId() {
            let lst = [];
            dtReserva.rows().iterator('row', function (context, index) {
                var node = $(this.row(index).node());
                if (node.hasClass('selected'))
                    lst.push(this.row(index).data().id);
            });
            return lst;
        }
        function initDataTblReserva() {
            dtReserva = tblReserva.DataTable({
                destroy: true,
                ordering: false,
                scrollY: "auto",
                scrollCollapse: true,
                paging: false,
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
                        data: 'anterior', createdCell: function (td, data, rowData, row, col) {
                            $(td).addClass('text-right').html(maskNumero(data));
                        }
                    },
                    {
                        data: 'cargo', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right reserva monto cargo`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                    {
                        data: 'abono', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right reserva monto abono`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                    {
                        data: 'global', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right reserva global`);
                            $(td).find(`input`).val(maskNumero(data));
                            $(td).find(`input`).prop('disabled', true);
                        }
                    },
                ],
                headerCallback: function (thead, data, start, end, display) {
                    let cargo = [],
                        abono = [],
                        global = [],
                        sumCargos = 0,
                        sumAbonos = 0,
                        sumGlobal = 0;
                    this.api().rows().iterator('row', function (context, index) {
                        var node = $(this.row(index).node());
                        if (node.find('.cc').data().cc.length === 3) {
                            cargo.push(unmaskNumero(node.find('.cargo').val()));
                            abono.push(unmaskNumero(node.find('.abono').val()));
                            global.push(unmaskNumero(node.find('.global').val()));
                        }
                    });
                    sumCargos = cargo.reduce((a, b) => a + b, 0);
                    sumAbonos = abono.reduce((a, b) => a + b, 0);
                    sumGlobal = sumCargos - sumAbonos;
                    $(thead).find('th:eq(2)').text(maskNumero(sumCargos));
                    $(thead).find('th:eq(3)').text(maskNumero(sumAbonos));
                    $(thead).find('th:eq(4)').text(maskNumero(sumGlobal));
                },
                createdRow: function (tr) {
                    setTipoReservaColor(tr);
                },
                initComplete: function (settings, json) {
                    tblReserva.on('change', '.reserva', function (event) {
                        let row = $(this).parents('tr'),
                            cc = row.find('.cc').val();
                        if (cc === null) {
                            row.find('.cc option').prop('disabled', false);
                            cc = row.find('.cc').val();
                        }
                        //#region montos
                        let cargo = unmaskNumero(row.find('.cargo').val()),
                            abono = unmaskNumero(row.find('.abono').val());
                        global = cc.length === 3 ? row.find(`option[value="${cc}"]`).data().prefijo : cargo - abono;
                        //#endregion
                        row.find('.global').val(maskNumero(global));
                        row.find('.cargo').val(maskNumero(cargo));
                        row.find('.abono').val(maskNumero(abono));
                        row.find('.cc').data().cc = cc;
                        deshabilitarCCSelecionados();
                        dtReserva.draw();
                    });
                    tblReserva.on('change', '.cc', function (event) {
                        let row = $(this).parents('td'),
                            cc = row.find(`.cc`).val();
                        if (cc === null) {
                            row.find(`.cc option`).prop('disabled', false);
                            cc = row.find('.cc').val();
                        }
                        let ant = $(this).find(`option[value="${cc}"]`).data().prefijo;
                        row.eq(1).html(maskNumero(ant));
                        deshabilitarCCSelecionados();
                    });
                    tblReserva.find('tbody').on('click', 'tr', function () {
                        if ($(this).hasClass('selected'))
                            $(this).removeClass('selected');
                        else {
                            tblReserva.find('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                        }
                    });
                }
            });
            dtReserva.columns.adjust();
        }
        setTipoReservaColor = (tr) => {
            let catReserva = selResTipo.find(`option[value="${selResTipo.val()}"]`).data().prefijo;
            if (catReserva !== undefined) {
                $(tr).css(`background-color`, `${catReserva.hexColor}`);
            }
        }
        function deshabilitarCCSelecionados() {
            let ccDeshabilitaddo = [];
            //#region obtener cc a deshabilitar
            dtReserva.rows().iterator('row', function (context, index) {
                let row = $(this.row(index).node()),
                    cc = row.find('.cc').data().cc;
                if (cc.length === 3) {
                    ccDeshabilitaddo.push(cc);
                }
            });
            //#endregion
            //#region deshabilitar cc
            dtReserva.rows().iterator('row', function (context, index) {
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
        function eliminaReserva() {
            dtReserva.row('.selected').remove().draw(false);
            if (dtReserva.data().count() == 0)
                eventValidaUltimaReserva();
        }
        function eventValidaUltimaReserva() {
            let tipo = +(selResTipo.val());
            if (tipo > 0) {
                let data = {
                    id: 0,
                    tipo: tipo,
                    cc: "",
                    anterior: 0,
                    cargo: 0,
                    abono: 0,
                    global: 0
                };
                dtReserva.row.add(data).draw();   
            }
        }
        function initTotal() {
            txtReservaTotal.val(maskNumero(0)).data().total = 0;
        }
        validaReserva = (data) => {
            let valido = data.cargo !== 0 || data.abono !== 0;
            if (data.tipo <= 0)
                valido = false;
            if (data.cc.length != 3)
                valido = false;
            return valido;
        }
        getConcentradoToReserva = (lst) => {
            let tipoReserva = {
                tipo: lst[0].tipo
            };
            selResTipo.val(tipoReserva.tipo);
            dtReserva.clear().rows.add(lst).draw();
        }
        setSemanaReservaSelecionada = () => {
            let date = dpResFecha.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 6);
            dpResFecha.val(`${startDate.toLocaleDateString()} - ${endDate.toLocaleDateString()}`)
            selectCurrentWeek();
        }
        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                dpResFecha.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }
        function initForm() {
            getBusqDTO().then(response => {
                if (response.success) {
                    esDivIndustrial = response.busq.esDivIndustrial;
                    dpResFecha.datepicker({
                        firstDay: 0,
                        showOtherMonths: true,
                        selectOtherMonths: true,
                        onSelect: function (dateText, inst) {
                            setSemanaReservaSelecionada();
                            eventBusqReserva();
                        },
                        beforeShowDay: function (date) {
                            var cssClass = '';
                            if (date >= startDate && date <= endDate)
                                cssClass = 'ui-datepicker-current-day';
                            return [true, cssClass];
                        },
                        onChangeMonthYear: function (year, month, inst) {
                            selectCurrentWeek();
                        },
                        beforeShow: function () {
                            setTimeout(function () {
                                $('.ui-datepicker').css('z-index', 9999);
                            }, 0);
                        }
                    }).datepicker("setDate", new Date());
                }
            }).then(response => {
                selResTipo.fillCombo('/Administrativo/Propuesta/getTipoReserva', null, false, null);
                setSemanaReservaSelecionada();
                initDataTblReserva();
                initTotal();
            });
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta._mdlReservas = new _mdlReservas();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();