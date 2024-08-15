(() => {
    $.namespace('Enkontrol.');
    _mdlGuardar = function () {
        mdlTC = $('#mdlTC');
        dtHoy = $('#dtHoy');
        lblMoneda = $('#lblMoneda');
        btnGuardarTC = $('#btnGuardarTC');
        txtTipoCambio = $('#txtTipoCambio');
        let init = () => {
            initForm();
            btnGuardarTC.click(getTC);
            txtTipoCambio.change(setTC);
        }
        const guardarTC = () => { return $.post('/Enkontrol/Moneda/guardarTC', { moneda: lblMoneda.data().moneda, tc: unmaskNumero(txtTipoCambio.val()) }); };
        getTC = () => {
            if (+(txtTipoCambio.val()) > 0)
                guardarTC()
                    .done(response => {
                        if (response.success) {
                            AlertaGeneral("Aviso", "Moneda de hoy guardada");
                            mdlTC.modal("hide");
                            $('select.moneda').each(function (index, item){
                                $(item).fillCombo('/Enkontrol/Moneda/FillComboMonedaHoy', null, false, null);
                            });
                        }
                    });
        }
        setTC = () => {
            let val = unmaskNumero6D(txtTipoCambio.val());
            txtTipoCambio.val(val);
        }
        setForm = (moneda) => {
            lblMoneda.val(moneda.clave).data().moneda = moneda.moneda;
            txtTipoCambio.val(maskNumero(moneda.tipo_cambio));
        }
        initForm = () => {
            dtHoy.val(new Date().toLocaleDateString());
            setForm({
                moneda: 1,
                clave: "MN",
                tipo_cambio: 1
            });
        }
        init();
    }
    $(document).ready(() => { Enkontrol = new _mdlGuardar(); })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();