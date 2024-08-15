(function () {
    $.namespace('Kubrix.BaseDatos._tblVencimiento');
    _tblVencimiento = function () {
        tblVencimiento = $("#tblVencimiento");
        lstPaginador = $("#lstPaginador");
        function init() {
            paginador();
        }
        function paginador() {
            let dataPaginador = lstPaginador.data();
            tblVencimiento.find('tbody').pageMe({ 
                pagerSelector: '#lstPaginador', 
                showPrevNext: dataPaginador.showprevnext, 
                hidePageNumbers: dataPaginador.hidepagenumbers, 
                perPage: dataPaginador.perpage 
            });
        }
        init();
    }
    $(document).ready(function () {
        Kubrix.BaseDatos._tblVencimiento = new _tblVencimiento();
    })
    .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(function () { $.unblockUI(); });
})();