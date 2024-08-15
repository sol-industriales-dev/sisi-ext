(() => {
    $.namespace('OTROS_SERVICIOS.Calendario');

    RelPuestoFases = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
        }
    }

    $(document).ready(() => {
        OTROS_SERVICIOS.Calendario = new Calendario();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();