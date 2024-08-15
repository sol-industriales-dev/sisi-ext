(function () {
    $.namespace('Kubrix.BaseDatos._tblCxp');
    _tblCxp = function () {
        tblCxp = $("#tblCxp");
        lstPaginador = $("#lstPaginador");
        function init() {
            paginador();
        }
        function paginador() {
            let dataPaginador = lstPaginador.data();
            tblCxp.find('tbody').pageMe({
                pagerSelector: '#lstPaginador',
                showPrevNext: dataPaginador.showprevnext,
                hidePageNumbers: dataPaginador.hidepagenumbers,
                perPage: dataPaginador.perpage
            });
        }
        init();
    }
    $(document).ready(function () {
        Kubrix.BaseDatos._tblCxp = new _tblCxp();
    })
    .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(function () { $.unblockUI(); });
})();