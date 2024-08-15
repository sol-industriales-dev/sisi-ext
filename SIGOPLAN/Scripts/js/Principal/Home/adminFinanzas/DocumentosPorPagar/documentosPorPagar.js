(() => {
    $.namespace('principal.home.documentosPorPagar');

    documentosPorPagar = function () {

        // Variables.
        const tblContratos = $('#tblContratos');


        (function init() {
            // Lógica de inicialización.
            initGrid();
        })();

        // Métodos.

        function initTablaPagos() {
            dtPagos = tblPagos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollX: true,
                scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'Maquina', title: 'Maquinaria' },
                    { data: 'Importe', title: 'Importe' },
                    { data: 'Pagado', title: 'Aplicar Pago' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    {
                        targets: [1],
                        render: function (data, type, row) {
                            return `<input type='number' class="form-control inputImporte"  value='${row.Importe}'/>`
                        },
                    }
                    , {
                        targets: [2],
                        render: function (data, type, row) {
                            let checked = 'checked';
                            return `<input type='checkbox' class='form-control inputPagado' ${checked} />`
                        }
                    }
                ]
            });
        }

        function obtenerContratos() {
            $.post('/Contratos/ObtenerContratosNotificaciones').always().then(response => {
                if (response.Success) {
                    AddRows(tblContratos, response.Value);
                }
                else {
                    alert(response.Message);
                }
            }, error => {
                alert('Error: ' + error.statusText);
            });
        }

        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

    }


}

    $(() => principal.home.documentosPorPagar = new documentosPorPagar())
    .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    .ajaxStop($.unblockUI);
}) ();