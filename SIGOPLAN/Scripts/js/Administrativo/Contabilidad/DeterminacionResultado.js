(() => {
    $.namespace('Administrativo.Contabilidad.DeterminacionResultado');
    DeterminacionResultado = function () {
        //#region Selectores
        const tablaDeterminacionResultado = $('#tablaDeterminacionResultado');
        //#endregion

        let dtTablaDeterminacionResultado;

        (function init() {
            initTablaDeterminacionResultado();
        })();

        function initTablaDeterminacionResultado() {
            dtTablaDeterminacionResultado = tablaDeterminacionResultado.DataTable({
                language: dtDicEsp,
                paging: false,
                info: false,
                searching: false,
                scrollX: 'auto',
                columns: [
                    { data: 'a1' },
                    { data: 'a2' },
                    { data: 'a3' },
                    { data: 'a4' },
                    { data: 'a5' },
                    { data: 'a6' },
                    { data: 'a7' },
                    { data: 'a8' },
                    { data: 'a9' },
                    { data: 'a10' },
                    { data: 'a11' },
                    { data: 'a12' },
                    { data: 'a13' },
                    { data: 'a14' },
                    { data: 'a15' },
                    { data: 'a16' },
                    { data: 'a17' },
                    { data: 'a18' },
                    { data: 'a19' },
                    { data: 'a20' },
                    { data: 'a21' },
                    { data: 'a22' },
                    { data: 'a23' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }
    }
    $(document).ready(() => Administrativo.Contabilidad.DeterminacionResultado = new DeterminacionResultado())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();