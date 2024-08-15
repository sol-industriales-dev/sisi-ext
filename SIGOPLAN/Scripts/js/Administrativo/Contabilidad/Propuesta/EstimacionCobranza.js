(() => {
$.namespace('Administrativo.Contabilidad.Propuesta.EstimacionCobranza');
    EstimacionCobranza = function (){
        let startDate ,endDate;
        const dpFecha = $('#dpFecha');
        const btnBuscar = $('#btnBuscar');
        const btnAddRen = $('#btnAddRen');
        const btnResRen = $('#btnResRen');
        const tblEstCob = $('#tblEstCob');
        const btnGuardar = $('#btnGuardar');
        lstCC = [];
        let init = () => {
            initForm();
            btnAddRen.click(addNuevoRenglon);
            btnResRen.click(remSelecRenglon);
            btnBuscar.click(setEstimacionCobranza);
            btnGuardar.click(setGuardarEstimacionCobro);
        }
        const getCC = new URL(window.location.origin + '/Administrativo/Poliza/getCC');
        const guardarEstimacionCobro = new URL(window.location.origin + '/Administrativo/Propuesta/guardarEstimacionCobro');
        const getLstEstimacionCobranza = new URL(window.location.origin + '/Administrativo/Propuesta/getLstEstimacionCobranza');
        async function setGuardarEstimacionCobro() {
            try {
                let lst = getTblEstCob();
                response = await ejectFetchJson(guardarEstimacionCobro, {lst});
                if (response.success) {
                    AlertaGeneral("Aviso", "Estimaciones de Cobranza guardadas con éxito.");
                }
            } catch (o_O) { }
        }
        async function setEstimacionCobranza() {
            try {
                dtEstCob.clear().draw();
                response = await ejectFetchJson(getLstEstimacionCobranza, {fecha: dpFecha.datepicker("getDate")});
                if (response.success) {
                    dtEstCob.rows.add(response.lst).draw();
                }
            } catch (o_O) { }
        }
        async function setSelCC() {
            try {    
                response = await ejectFetchJson(getCC);
                if (response.success) {
                    lstCC = response.items;
                }
            } catch (o_O) { }
        }
        function addNuevoRenglon() {
            dtEstCob.row.add({
                id: 0
               ,cc: ""
               ,estimado: 0
               ,semana1: 0
               ,semana2: 0
               ,semana3: 0
            }).draw();
            deshabilitarCCSelecionados();
        }
        function remSelecRenglon() {
            dtEstCob.row('.selected').remove().draw(false);
            deshabilitarCCSelecionados();
        }
        function initDataTblEstCob() {
            dtEstCob = tblEstCob.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    {
                        data: 'cc', width:'9%', createdCell: function (td, data, rowData, row, col) {
                            let cbo = $(`<select>`);
                            cbo.addClass(`form-control cc`);
                            cbo.fillComboItems(lstCC, null, data);
                            $(td).html(cbo);
                        }
                    },
                    {
                        data: 'estimado', width:'9%', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right est dinero`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                    {
                        data: 'semana1', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right sem1 dinero`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                    {
                        data: 'semana2', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right sem2 dinero`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                    {
                        data: 'semana3', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right sem3 dinero`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblEstCob.on('change', '.cc', function (event) {
                        let row = $(this).parents('td'),
                            cc = row.find(`.cc`).val();
                        if (cc === null) {
                            row.find(`.cc option`).prop('disabled', false);
                            cc = row.find('.cc').val();
                        }
                        deshabilitarCCSelecionados();
                    });
                    tblEstCob.on('change', '.dinero', function (event) {
                        let valor = unmaskNumero(this.value);
                        this.value = maskNumero(valor);
                    });
                    tblEstCob.find('tbody').on('click', 'tr', function () {
                        if ($(this).hasClass('selected'))
                            $(this).removeClass('selected');
                        else {
                            tblEstCob.find('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                        }
                    });
                }
            });
        }
        function deshabilitarCCSelecionados() {
            let ccDeshabilitaddo = [];
            //#region obtener cc a deshabilitar
            dtEstCob.rows().iterator('row', function (context, index) {
                let row = $(this.row(index).node())
                   ,sel =row.find('.cc');
                   sel.find(`option`).prop("disabled", false);
                   let cc = sel.val();
                    ccDeshabilitaddo.push(cc);
            });
            //#endregion
            //#region deshabilitar cc
            dtEstCob.rows().iterator('row', function (context, index) {
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
        function getTblEstCob() {
            let lst = []
               ,fecha = dpFecha.datepicker("getDate");
            dtEstCob.rows().iterator('row', function (context, index) {
                let row = $(this.row(index).node())
                   ,sel = row.find('.cc');
                   sel.find(`option`).prop("disabled", false);
                   let cc = sel.val()
                      ,data = $(this.row(index).data());
                      data.cc = cc;
                      data.estimado = unmaskNumero(row.find('.est').val());
                      data.semana1 = unmaskNumero(row.find('.sem1').val());
                      data.semana2 = unmaskNumero(row.find('.sem2').val());
                      data.semana3 = unmaskNumero(row.find('.sem3').val());
                      data.fecha = fecha;
                      if (cc.length === 3 && (data.estimado > 0 || data.semana1 > 0 || data.semana2 > 0 || data.semana3 > 0)) {
                        lst.push(data);   
                      }
            });
            deshabilitarCCSelecionados();
            return lst;
        }
        setSemanaReservaSelecionada = () => {
            let date = dpFecha.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 6);
                dpFecha.val(`${startDate.toLocaleDateString()} - ${endDate.toLocaleDateString()}`)
            selectCurrentWeek();
        }
        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                dpFecha.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }
        function initForm() {
            setSelCC();
            initDataTblEstCob();
            dpFecha.datepicker({
                firstDay: 0,
                showOtherMonths: true,
                selectOtherMonths: true,
                onSelect: function (dateText, inst) {
                    setSemanaReservaSelecionada();
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
            }).datepicker('setDate', new Date());
            setSemanaReservaSelecionada();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta.EstimacionCobranza = new EstimacionCobranza();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();