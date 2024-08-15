(function (){
    $.namespace('Kubrix.Catalogo.CentroCostos');
    CentroCostos = function(){
        selBusqCC = $("#selBusqCC");
        selBusqDiv = $("#selBusqDiv");
        function init(){
            initForm();
        }
        function initForm(){
            selBusqCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', null, false, null);
            selBusqDiv.fillCombo('/Kubrix/Catalogo/getCboDivision', null, false, null);
        }
        init();
    }
    $(document).ready(function () {
        Kubrix.Catalogo.CentroCostos = new CentroCostos();
    });
})();