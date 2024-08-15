(() => {
    $.namespace('Seguridad.Indicadores');

    Indicadores = function () {

        // Variables.


        // Función init autoejecutable.
        (function init() {
            initTotalesTable()
        })();

        //#region TABLA TOTALES
        function initTotalesTable() {
            $('#tblTotales').append(createRowTableTotales())
        }

        function createRowTableTotales() {
            const header =
                `
                    <tr>
                        <td></td>
                    </tr>
                `;
        }
        //#endregion




    }
    $(document).ready(() => Seguridad.Indicadores = new Indicadores())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();