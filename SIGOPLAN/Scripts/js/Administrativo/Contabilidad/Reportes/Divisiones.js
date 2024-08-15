(function () {
    $.namespace('Administrativo.Contabilidad.Divisiones');
    Divisiones = function (){
        const selCC = $("#selCC");
        const btnGuardar = $("#btnGuardar");
        const selDivision = $("#selDivision");
        const tblDivision = $("#tblDivision");
        const selCCPrincipal = $('#selCCPrincipal');
        function init() {
            initForm();
            selCC.change(showDivision);
            btnGuardar.click(saveDivision);
            $(`a[href='#tabCcDividiones']`).on('show.bs.tab', function () {
                tablaDivision.columns.adjust();
            });
        }
        const setDivision = new URL(window.location.origin + '/Administrativo/Reportes/setDivision');
        const getCCPrincipal = new URL(window.location.origin + '/Administrativo/Poliza/getCCPrincipal');
        function initForm(){
            selCC.fillCombo('/Administrativo/Poliza/getCC', null, false, null);
            selCCPrincipal.fillCombo('/Administrativo/Poliza/getCC', null, false, null);
            selDivision.fillCombo('/Administrativo/Poliza/getLstTipoDivision', null, false, null);
            initTable();
        }
        function initTable() {
            tablaDivision = tblDivision.DataTable({
                destroy: true,
                searching: true,
                paging: true,
                language: dtDicEsp,
                ajax: { url: '/Administrativo/Poliza/getLstDivision'},
                columnDefs: [
                    { "targets": 0,  "data": 'cc' },
                    { "targets": 1, "data": 'division' },
                    { "targets": 2, "data": 'ccPrincipal' }
                ],
                initComplete: function (settings, json) {
                    tablaDivision.on('click', 'tr', function () {
                        selCC.val(tablaDivision.row($(this)).data().value).change();
                        postGetCCPrincipal(selCC.val());
                    });
                }
            });
        }
        async function postGetCCPrincipal(cc) {
            try {
                selCCPrincipal.val("");
                response = await ejectFetchJson(getCCPrincipal, {
                    cc: cc,
                });
                if (response.success) {
                    selCCPrincipal.val(response.ccPrincipal);
                }    
            } catch (o_O) { }
        }
        function showDivision(){
            selDivision.val(selCC.find(':selected').data().prefijo);
            postGetCCPrincipal(selCC.val());
        }
        async function saveDivision(){
            try {
                let mensaje = `Al centro de costos ${selCC.find(":selected").text()} se le asignó la división ${selDivision.find(":selected").text()}.`;
                response = await ejectFetchJson(setDivision, {
                    cc: selCC.val(),
                    div: selDivision.val(),
                    ccPrincipal: selCCPrincipal.val()
                });
                if (response.success) {
                    AlertaGeneral("Aviso", mensaje);
                    initForm();
                    selDivision.val("0");
                }
            } catch (o_O) { }       
        }
        init();
    }
    $(document).ready(function () {
        Administrativo.Contabilidad.Divisiones = new Divisiones();
    })
    .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(function () { $.unblockUI(); });
})();