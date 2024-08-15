(() => {
    $.namespace('AltaDeNotificantes.EvaluacionSubcontratista');

    EvaluacionSubcontratista = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
        }
    }

    $(document).ready(() => {
        AltaDeNotificantes.EvaluacionSubcontratista = new EvaluacionSubcontratista();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();