(() => {
$.namespace('Administrativo.Contabilidad.CcDivision');
    CcDivision = function (){
        const chbDivCtaEstado = $('#chbDivCtaEstado');
        const selCuentaBancaria = $('#selCuentaBancaria');
        const tblCuentaDivision = $('#tblCuentaDivision');
        const selCcDivsionCuenta = $('#selCcDivsionCuenta');
        const btnGuardarCuentaDivision = $('#btnGuardarCuentaDivision');
        let init = () => {
            initForm();
            btnGuardarCuentaDivision.click(clkSetRelDivCta);
            selCuentaBancaria.change(chgCboDiv);
        }
        const getLstCuenta = new URL(window.location.origin + '/Administrativo/CcDivision/getLstCuenta');
        const setRelDivCta = new URL(window.location.origin + '/Administrativo/CcDivision/setRelDivCta');
        async function setTblDivCta() {
            try {
                dtCuentaDivision.clear().draw();
                response = await ejectFetchJson(getLstCuenta);
                if (response.success) {
                    dtCuentaDivision.rows.add(response.lst).draw();
                    selCuentaBancaria.val(0).change();
                }   
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        async function clkSetRelDivCta() {
            try {
                response = await ejectFetchJson(setRelDivCta, getForm());
                if (response.success) {
                    AlertaGeneral("Aviso", "Cuenta guardada correctamente.");
                    setTblDivCta();
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        function chgCboDiv() {
            setFrom({cuenta: +(selCuentaBancaria.val())});
        }
        function setFrom({cuenta}) {
            limpiarMultiselect(selCcDivsionCuenta);
            let rel = cuenta == 0 ? [] : selCuentaBancaria.find(`option[value='${cuenta}']`).data().prefijo,
                esActivo = rel.length > 0 && rel.every(r => r.esActivo);
            selCcDivsionCuenta.val(rel.map(r => r.division));
            chbDivCtaEstado.prop("checked", esActivo);
            convertToMultiselect(selCcDivsionCuenta);
        }
        function getForm() {
            let lst = [],
                lstDiv = selCcDivsionCuenta.val(), 
                cuenta = selCuentaBancaria.val(),
                esActivo = chbDivCtaEstado.prop("checked");
                lstDiv.forEach(division => lst.push({cuenta ,division, esActivo}));
            return lst;
        }
        function initDataTblCuentaDivision() {
            dtCuentaDivision = tblCuentaDivision.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    { data: 'cuenta'},
                    { data: 'descripcion'},
                    { data: 'descBanco'},
                    { data: 'descDivision'},
                    { data: 'moneda', createdCell: (td, data) =>  $(td).html(data === 1 ? "MX" :"DLL") },
                    { data: 'num_cta_banco'},
                    { data: 'esActivo', createdCell: (td, data) => $(td).html(data ? "Activo" :"No activo") },
                ]
                ,initComplete: function (settings, json) {
                    tblCuentaDivision.find('tbody').on('click', 'tr', function () {
                        let data = dtCuentaDivision.row(this).data();
                        selCuentaBancaria.val(data.cuenta).change();
                    });
                }
            });
        }
        function initForm() {
            selCuentaBancaria.fillCombo('/Administrativo/CcDivision/cboCuentaBanco', null, false, null);
            selCcDivsionCuenta.fillCombo('/Administrativo/Poliza/getLstTipoDivision', null, true, null);
            selCuentaBancaria.select2({width: '100%'});
            convertToMultiselect(selCcDivsionCuenta);
            initDataTblCuentaDivision();
            setTblDivCta();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.CcDivision = new CcDivision();
    })
    .ajaxStart(() => { $.blockUI({ 
        message: 'Procesando...' 

    }); })
    .ajaxStop(() => { $.unblockUI(); });
})();