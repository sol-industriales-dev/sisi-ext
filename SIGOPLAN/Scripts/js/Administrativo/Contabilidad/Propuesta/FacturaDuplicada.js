(() => {
    $.namespace('Administrativo.Contabilidad.Propuesta.FacturaDuplicada');
    FacturaDuplicada = function() {
        //#region Selectores
        const tblDuplicados = $('#tblDuplicados');
        //#endregion

        (function init() {
            initTablas();

            getDuplicados();
        })();

        function initTablas() {
            tblDuplicados.DataTable({
                order: [[0, 'asc']],
                ordering: true,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],

                columns: [
                    { data: 'numpro', title: 'Número proveedor' },
                    { data: 'factura', title: 'Factura' },
                    { data: 'cc', title: 'CC' },
                    { data: 'referenciaoc', title: 'OC' },
                    { data: 'serie', title: 'Número de serie' },
                    { data: 'tm', title: 'TM' },
                    { data: 'concepto', title: 'Concepto' },
                    { data: 'monto', title: 'Monto'},
                    { data: 'tipoCambio', title: 'Tipo de cambio' }
                ],

                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    }
                ],

                headerCallback: function(thead, data, start, end, display) {
                    $(thead).parent().children().addClass('bg-table-header');
                    $(thead).children().addClass('text-center');
                }
            });
        }

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function getDuplicados() {
            $.get('/Administrativo/Propuesta/getDuplicados', {}).then(response => {
                if (response.success) {
                    addRows(tblDuplicados, response.items);
                } else {
                    swal("Alerta!", response.message, "warning");
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }
    }
    $(document).ready(() => Administrativo.Contabilidad.Propuesta.FacturaDuplicada = new FacturaDuplicada())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();