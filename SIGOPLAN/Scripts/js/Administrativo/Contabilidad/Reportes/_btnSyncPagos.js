(function () {
    $.namespace('Administrativo.Contabilidad.Reportes._btnSyncPagos');
    _btnSyncPagos = function (){
        btnSyncPago = $("#btnSyncPago");
        lblFecha = $("#lblFecha");
        function init() {
            getFechaLabel();
            btnSyncPago.click(setPago);
        }
        function setPago() {
            $.blockUI({ message: 'Sincronización de pagos en proceso...' });
            let res = $.post("/Administrativo/Reportes/SetPago");
            res.done(function(pago){
                if (pago.success) {
                    getFechaLabel();
                    AlertaGeneral("Aviso", "Sincronización completa");   
                }
            });
            res.always(function(a){
                $.unblockUI();
            });
          }
        function getFechaLabel(){
            $.blockUI({ message: 'Sincronización de pagos en proceso...' });
            let res = $.post("/Administrativo/Reportes/GetFechaSync");
            res.done(function(label){
                lblFecha.text("Ultima: " + label.fecha);
            });
            res.always(function(a){
                $.unblockUI();
            });
        }
        init();
    }
    $(document).ready(function () {
        Administrativo.Contabilidad.Reportes._btnSyncPagos = new _btnSyncPagos();
    });
})();