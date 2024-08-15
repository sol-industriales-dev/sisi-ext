(function () {
    $.namespace('Kubrix.BaseDatos._tblSaldos');
    _tblSaldos = function () {
        tblSaldos = $("#tblSaldos");
        lstPaginador = $("#lstPaginador");
        function init() {
            paginador();
        }
        function paginador() {
            $.blockUI({ message: 'Procesando...' });
            let dataPaginador = lstPaginador.data();
            tblSaldos.find('tbody').pageMe({
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
        Kubrix.BaseDatos._tblSaldos = new _tblSaldos();
    })
    .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(function () { $.unblockUI(); });
})();