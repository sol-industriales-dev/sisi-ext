(() => {
    $.namespace('Administrativo.FlujoEfectivoArrendadora._movPoliza');
    _movPoliza = function () {
        var startDate, endDate, minFechaPol, maxFechapol;
        const selMpCc = $('#selMpCc');
        const selTMpTm = $('#selTMpTm');
        const mpMpFecha = $('#mpMpFecha');
        const mdlConcepto = $('#mdlConcepto');
        const btnMpBuscar = $('#btnMpBuscar');
        const btnMpGuardar = $('#btnMpGuardar');
        const selMpTipoBusqueda = $('#selMpTipoBusqueda');
        const getCC = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getComboAreaCuenta');
        const getLstMovPoliza = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getLstMovPoliza');
        const getGuardadoEstado = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getGuardadoEstado');
        const getPeriodoContable = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getPeriodoContable');
        const getCboTipoBusqueda = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getCboTipoBusqueda');
        const getComboTipoMovimiento = new URL(window.location.origin + '/Administrativo/Poliza/getComboTipoMovimiento');
        let init = () => {
            initForm();
            btnMpGuardar.click(setGuardar);
            btnMpBuscar.click(setLstMovPoliza);
            $('#divMpBuscar .input-group-btn').each(function (i, btn) { $(btn).click(); });
            mdlConcepto.on('shown.bs.modal', function () {
                dtCatConcepto.columns.adjust();
            });
            mdlConcepto.on('hide.bs.modal', function () {
                setLstMovPoliza();
            });
        }
        async function setLstMovPoliza() {
            try {
                dtMpMovPol.clear().draw();
                response = await ejectFetchJson(getLstMovPoliza, getMpForm());
                if (response.success) {
                    dtMpMovPol.rows.add(response.lst).draw();
                    limpiarLstData();
                }
            } catch (o_O) { console.log(o_O.message); }
        }
        async function setPeriodoContable() {
            try {
                response = await ejectFetchJson(getPeriodoContable);
                if (response.success) {
                    let arrMin = $.toDate(response.minPeriodo).split('/')
                        , arrMax = $.toDate(response.maxPeriodo).split('/');
                    minFechaPol = new Date(arrMin[2], +arrMin[1] - 1, arrMin[0]);
                    maxFechapol = new Date(arrMax[2], +arrMax[1] - 1, arrMax[0]);
                    mpMpFecha.datepicker({
                        firstDay: 0,
                        showOtherMonths: true,
                        selectOtherMonths: true,
                        minDate: minFechaPol,
                        maxDate: maxFechapol,
                        onSelect: function (dateText, inst) {
                            setSemanaSelecionada();
                            setEstadoGuardado();
                        },
                        beforeShowDay: function (date) {
                            var cssClass = '';
                            if (date >= startDate && date <= endDate)
                                cssClass = 'ui-datepicker-current-day';
                            return [true, cssClass];
                        }
                    }).datepicker("setDate", new Date());
                    setSemanaSelecionada();
                    setEstadoGuardado();
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        async function setEstadoGuardado() {
            try {
                let busq = getMpForm();
                response = await ejectFetchJson(getGuardadoEstado, busq);
                if (response.success) {
                    btnMpGuardar.prop("disabled", response.esGuardado);
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        function getMpForm() {
            return {
                tipo: selMpTipoBusqueda.val()
                , min: startDate.toLocaleDateString()
                , max: endDate.toLocaleDateString()
                , lstAC: selMpCc.val()
                , lstTm: selTMpTm.val()
            };
        }
        function initForm() {
            selMpCc.fillCombo(getCC, null, false, "TODOS");
            selTMpTm.fillCombo(getComboTipoMovimiento, { iSistema: "B" }, true, null);
            selMpTipoBusqueda.fillCombo(getCboTipoBusqueda, null, true, null); selMpTipoBusqueda.val(4);
            convertToMultiselect(selTMpTm);
            convertToMultiselect(selMpCc);
            setPeriodoContable();
        }
        function setSemanaSelecionada() {
            date = mpMpFecha.datepicker('getDate');
            prevDom = date.getDate() - (date.getDay() + 7) % 7;
            startDate = new Date(date.getFullYear(), date.getMonth(), prevDom);
            endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 6);
            diaSemana = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 3);
            noSemana = diaSemana.noSemana();
            mpMpFecha.val(`Semana ${noSemana} - ${startDate.toLocaleDateString()} - ${endDate.toLocaleDateString()}`);
            selectCurrentWeek();
        }
        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                mpMpFecha.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.FlujoEfectivoArrendadora._movPoliza = new _movPoliza();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();