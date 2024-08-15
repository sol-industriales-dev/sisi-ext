(function () {
    $.namespace('Kubrix.BaseDatos.Bal12');
    Bal12 = function () {
        divTblBal12 = $("#divTblBal12");
        selCC = $("#selCC");
        btnBuscar = $("#btnBuscar");
        function init() {
            btnBuscar.click(buscar);
            selCC.fillCombo('/Administrativo/Poliza/getCC', null, false, null); selCC.val("001");
        }
        function buscar() {
            divTblBal12.load('/Kubrix/BaseDatos/_tblBal12', {
                cc: selCC.val(),
                anio: new Date().getFullYear()
            });
        }
        init();
    }
    $(document).ready(function () {
        Kubrix.BaseDatos.Bal12 = new Bal12();
    });
})();