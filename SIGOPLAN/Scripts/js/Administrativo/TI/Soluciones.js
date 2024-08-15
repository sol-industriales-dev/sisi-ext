(() => {
    $.namespace('TI.Soluciones');

    //#region CONST FILTROS
    const btnFiltroBuscar = $("#btnFiltroBuscar");
    const btnFiltroNuevo = $("#btnFiltroNuevo");
    //#endregion

    //#region CONST CE SOLUCIÓN
    const mdlCESolucion = $('#mdlCESolucion')
    const btnCESolucion = $('#btnCESolucion')
    //#endregion

    Soluciones = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            $("#menuSoluciones").addClass("opcionSeleccionada");
            //#endregion

            //#region FILTROS
            btnFiltroBuscar.click(function () {
                fncGetSoluciones();
            });

            btnFiltroNuevo.click(function () {
                fncLimpiarModalCESolucion();
                btnCESolucion.html(`<i class='fas fa-save'></i>&nbsp;Guardar`);
                mdlCESolucion.find(".modal-title").text("Nuevo registro");
                mdlCESolucion.modal("show");
            });
            //#endregion
        }

        //#region SOLUCIONES
        function fncLimpiarModalCESolucion() {

        }
        //#endregion
    }

    $(document).ready(() => {
        TI.Soluciones = new Soluciones();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();