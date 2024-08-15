(() => {
$.namespace('Administracion.Contabilidad.Propuesta._tblReservasTotales');
    _tblReservasTotales = function (){
        const selResTotalTipo = $('#selResTotalTipo');
        const tblReservaTotal = $('#tblReservaTotal');
        let init = () => {
            initForm();
            selResTotalTipo.change(setTotalReserva);
        }
        const getTotalReservas = () => $.post('/Administrativo/Propuesta/getTotalReservas', { busq: getReservaBusq() });
        function setTotalReserva(){
            dtReservaTotal.clear().draw();
            getTotalReservas().done(response => {
                if(response.success){
                    dtReservaTotal.rows.add(response.lstReservaTotal).draw();
                }
            });
        }
        function getReservaBusq() {
            return {
                tipo: +(selResTotalTipo.val())
            };
        }
        function initDataTblReservaTotal() {
            dtReservaTotal = tblReservaTotal.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    { data: 'cc' },
                    { data: 'total', sClass: 'text-right'},
                ],
                initComplete: function (settings, json) {
                }
            });
        }
        function initForm() {
            initDataTblReservaTotal();
            selResTotalTipo.fillCombo('/Administrativo/Propuesta/getTipoReserva', null, false, "Todos");
            selResTotalTipo.change();
        }
        init();
    }
    $(document).ready(() => {
        Administracion.Contabilidad.Propuesta._tblReservasTotales = new _tblReservasTotales();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();