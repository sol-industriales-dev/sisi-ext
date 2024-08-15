(function () {
    $.namespace('Kubrix.BaseDatos.Vencimiento');
    Vencimiento = function () {
        divTblVencimiento = $("#divTblVencimiento");
        function init() {
            
        }
        function buscar() {
            divTblVencimiento.load('/Kubrix/BaseDatos/_tblVencimiento');
        }
        init();
    }
    $(document).ready(function () {
        Kubrix.BaseDatos.Vencimiento = new Vencimiento();
    });
})();