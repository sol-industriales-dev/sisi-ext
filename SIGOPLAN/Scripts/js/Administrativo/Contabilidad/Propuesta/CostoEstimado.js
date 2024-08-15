(() => {
$.namespace('Administrativo.Contabilidad.Propuesta.CostoEstimado');
    CostoEstimado = function (){
        const dpFecha = $('#dpFecha');
        const btnBuscar = $('#btnBuscar');
        const btnAddRen = $('#btnAddRen');
        const btnResRen = $('#btnResRen');
        const btnGuardar = $('#btnGuardar');
        const tblCostosEstimacion = $('#tblCostosEstimacion');
        let startDate ,endDate;
        let init = () => {
            initForm();
            btnAddRen.click(addNuevoRenglon);
            btnResRen.click(remSelecRenglon);
            btnBuscar.click(setLstCostoEstimado);
            btnGuardar.click(setGuardarCostoEstimado);
        }
        const getCC = new URL(window.location.origin + '/Administrativo/Poliza/getCC');
        const getLstCostoEstimado = new URL(window.location.origin + '/Administrativo/Propuesta/getLstCostoEstimado');
        const guardarLstCostoEstimado = new URL(window.location.origin + '/Administrativo/Propuesta/guardarLstCostoEstimado');
        async function setLstCostoEstimado() {
            try {
                dtCostosEstimacion.clear().draw();
                response = await ejectFetchJson(getLstCostoEstimado, { fecha: dpFecha.datepicker("getDate")});
                if (response.success) {
                    dtCostosEstimacion.rows.add(response.lst).draw();
                }
            } catch (o_O) { }
        }
        async function setGuardarCostoEstimado() {
            try {
                let lst = getLstTblCostosEstimacion();
                if (lst.length) {
                    response = await ejectFetchJson(guardarLstCostoEstimado ,lst);
                    if (response.success) {
                        AlertaGeneral("Aviso", "Costos Estimados guardado con éxito.");
                    }   
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
            dtCostosEstimacion.row.add({
                id: 0
               ,cc: ""
               ,estimacion: 0
            }).draw();
            deshabilitarCCSelecionados();
        }
        function remSelecRenglon() {
            dtCostosEstimacion.row('.selected').remove().draw(false);
            deshabilitarCCSelecionados();
        }
        function initDataTblCostosEstimacion() {
            dtCostosEstimacion = tblCostosEstimacion.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    {
                        data: 'cc', createdCell: function (td, data, rowData, row, col) {
                            let cbo = $(`<select>`);
                            cbo.addClass(`form-control cc`);
                            cbo.fillComboItems(lstCC, null, data);
                            $(td).html(cbo);
                        }
                    },
                    {
                        data: 'estimacion', createdCell: (td ,data) => {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right est`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblCostosEstimacion.on('change', '.cc', function (event) {
                        let row = $(this).parents('td'),
                            cc = row.find(`.cc`).val();
                        if (cc === null) {
                            row.find(`.cc option`).prop('disabled', false);
                            cc = row.find('.cc').val();
                        }
                        deshabilitarCCSelecionados();
                    });
                    tblCostosEstimacion.on('change', '.est', function () {
                        let valor = unmaskNumero(this.value);
                        this.value = maskNumero(valor);
                    });
                    tblCostosEstimacion.find('tbody').on('click', 'tr', function () {
                        if ($(this).hasClass('selected'))
                            $(this).removeClass('selected');
                        else {
                            tblCostosEstimacion.find('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                        }
                    });
                }
            });
        }
        function deshabilitarCCSelecionados() {
            let ccDeshabilitaddo = [];
            //#region obtener cc a deshabilitar
            dtCostosEstimacion.rows().iterator('row', function (context, index) {
                let row = $(this.row(index).node())
                   ,sel = row.find('.cc');
                   sel.find(`option`).prop("disabled", false);
                   let cc = sel.val();
                    ccDeshabilitaddo.push(cc);
            });
            //#endregion
            //#region deshabilitar cc
            dtCostosEstimacion.rows().iterator('row', function (context, index) {
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
        function getLstTblCostosEstimacion() {
            let lst = []
               ,fecha = dpFecha.datepicker('getDate');
            dtCostosEstimacion.rows().iterator('row', function (context, index) {
                let row = $(this.row(index).node())
                   ,sel = row.find('.cc');
                   sel.find(`option`).prop("disabled", false);
                   let cc = sel.val()
                      ,data = $(this.row(index).data());
                      data.cc = cc;
                      data.estimacion = unmaskNumero(row.find('.est').val());
                      data.fecha = fecha;
                      if (cc.length === 3 && data.estimacion > 0) {
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
                beforeShow: function(input, inst) {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 9999);
                    }, 0);                    
                }
            }).datepicker('setDate', new Date());
            initDataTblCostosEstimacion();
            setSemanaReservaSelecionada();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta.CostoEstimado = new CostoEstimado();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();