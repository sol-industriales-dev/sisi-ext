(function () {
    $.namespace('Kubrix.BaseDatos._tblBal12');
    _tblBal12 = function () {
        tblBal12 = $("#tblBal12");
        lstPaginador = $("#lstPaginador");
        function init() {
            paginador();
        }
        function paginador() {
            $.blockUI({ message: 'Procesando...' });
            let dataPaginador = lstPaginador.data();
            tblBal12.find('tbody').pageMe({
                pagerSelector: '#lstPaginador',
                showPrevNext: dataPaginador.showprevnext,
                hidePageNumbers: dataPaginador.hidepagenumbers,
                perPage: dataPaginador.perpage
            });
            $.unblockUI();
        }
        init();
    }
    $(document).ready(function () {
        Kubrix.BaseDatos._tblBal12 = new _tblBal12();
    })
    .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(function () { $.unblockUI(); });
})();