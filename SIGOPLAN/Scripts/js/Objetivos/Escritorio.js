(() => {
    $.namespace('Objetivos.Escritorio');
    Escritorio = function () {
        //#region Selectores
        const botonAgregarMeta = $('#botonAgregarMeta');
        const modalAgregarMeta = $('#modalAgregarMeta');
        const tablaMetas = $('#tablaMetas');
        //#endregion

        let datosPrueba = [{ meta: 'Mantenimiento SIGOPLAN', peso: 21, progreso: 0 }];

        (function init() {
            agregarListeners();
            initTablaMetas();
            cargarDatosPrueba();
        })();

        function agregarListeners() {
            botonAgregarMeta.click(() => { modalAgregarMeta.modal('show') });
        }

        function initTablaMetas() {
            tablaMetas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaMetas.on('click', '.btn-editar', function () {
                        modalAgregarMeta.modal('show');
                    });
                },
                columns: [
                    { data: 'meta', title: 'Meta' },
                    {
                        data: 'peso', title: 'Peso', render: function (data, type, row, meta) {
                            return data + '%';
                        }
                    },
                    {
                        data: 'progreso', title: 'Progreso', render: function (data, type, row, meta) {
                            return data + '%';
                        }
                    },
                    {
                        render: function (data, type, row, meta) {
                            return `
                            <button class="btn-editar btn btn-xs btn-warning">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            <button class="btn-eliminar btn btn-xs btn-danger">
                                <i class="fas fa-trash"></i>
                            </button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarDatosPrueba() {
            AddRows(tablaMetas, datosPrueba);
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Objetivos.Escritorio = new Escritorio())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();