(function () {
$.namespace('Encuesta._tarjetaEstrella');
    mdlInfoEstrella = $('#mdlInfoEstrella');
    _tarjetaEstrella = function (){
        function init() {
            mdlInfoEstrella.on('shown.bs.modal', function () {
                $(this).find('.modal-dialog').css({ 'padding-right': 0 });
            });
        }
        
        init();
    }
    $(document).ready(function () {
        Encuesta._tarjetaEstrella = new _tarjetaEstrella();
    });
})();