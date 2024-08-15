(function () {
    $.namespace('Kubrix.BaseDatos.Vencimiento');
    Vencimiento = function () {
        divTblSaldos = $("#divTblSaldos");
        selCC = $("#selCC");
        selAnio = $("#selAnio");
        btnBuscar = $("#btnBuscar");
        function init() {
            btnBuscar.click(buscar);
            selCC.fillCombo('/Administrativo/Poliza/getCC', null, false, null); selCC.val("001");
            selAnio.fillCombo('/Kubrix/BaseDatos/lstAnioSaldos', null, false, null); selAnio.val(new Date().getFullYear());
        }
        function buscar() {
            divTblSaldos.load('/Kubrix/BaseDatos/_tblSaldos',{ 
            cc: selCC.val(), 
            anio: selAnio.val()
            });
        }
        init();
    }
    $(document).ready(function () {
        Kubrix.BaseDatos.Vencimiento = new Vencimiento();
    });
})();