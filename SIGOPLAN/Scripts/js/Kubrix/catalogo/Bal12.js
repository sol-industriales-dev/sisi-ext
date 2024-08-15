(function (){
    $.namespace('Kubrix.Catalogo.Bal12');
    Bal12 = function(){
        selBusqCC = $("#selBusqCC");
        divTblBal12 = $("#divTblBal12");
        function init(){
            initForm();
            selBusqCC.change(buscar);
        }
        function initForm(){
            selBusqCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', null, false, null);
            selBusqCC.val("001");
        }
        function buscar(){
            divTblBal12.load('/Kubrix/Catalogo/_tblBal12',{
                cc: selBusqCC.val(),
            });
        }
        init();
    }
    $(document).ready(function () {
        Kubrix.Catalogo.Bal12 = new Bal12();
    });
})();