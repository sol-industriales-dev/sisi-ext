(() => {
    $.namespace('TI._MenuTI');

    //#region CONST
    const menuSoluciones = $("#menuSoluciones");
    //#endregion

    _MenuTI = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region REDIRECCIONAMIENTOS
            menuSoluciones.click(function () {
                document.location.href = '/Administrativo/TI/Soluciones';
            });
            //#endregion
        }
    }

    $(document).ready(() => {
        TI._MenuTI = new _MenuTI();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();