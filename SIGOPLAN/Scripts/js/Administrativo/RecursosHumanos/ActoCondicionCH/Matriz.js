(() => {
    $.namespace('CH.Matriz')

    //#region CONST FILTROS
    const btnFiltroBuscar = $('#btnFiltroBuscar')
    //#endregion

    //#region CONST MATRIZ
    const tblMatriz = $('#tblMatriz')
    let dtMatriz;
    //#endregion

    Matriz = function () {
        (function init() {
            fncListeners()
        })()

        function fncListeners() {
            //#region INIT DATATABLE
            initTblMatriz()
            fncGetMatriz()
            //#endregion

            //#region FUNCIONES MATRIZ
            btnFiltroBuscar.click(function () {
                fncGetMatriz()
            })
            //#endregion
        }

        //#region FUNCIONES MATRIZ
        function initTblMatriz() {
            dtMatriz = tblMatriz.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'noMatriz', title: 'No.' },
                    { data: 'tipoInfraccion', title: 'Tipo infracción' },
                    { data: 'nivel', title: 'Nivel' },
                    { data: 'amonestacionDescripcion', title: 'Amonestación' },
                    { data: 'suspensionDescripcion', title: 'Suspensión' },
                    { data: 'recisionDescripcion', title: 'Recisión' },
                    { data: 'sancionDescripcion', title: 'Sanción' }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { className: 'dt-body-center', 'targets': '_all' },
                    { width: '8%', targets: [0, 2, 3, 4, 5, 6] }
                ],
            });
        }

        function fncGetMatriz() {
            axios.post('GetMatriz').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtMatriz.clear();
                    dtMatriz.rows.add(response.data.lstMatriz);
                    dtMatriz.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Matriz = new Matriz()
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }) })
        .ajaxStop(() => { $.unblockUI() })
})()