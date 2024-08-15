(() => {
    $.namespace('OrdenCompra.ReporteUltimaCompraInsumo');
    ReporteUltimaCompraInsumo = function () {
        //#region Selectores
        const botonBuscar = $('#botonBuscar');
        const textareaInsumos = $('#textareaInsumos');
        const tablaProveedores = $('#tablaProveedores');
        //#endregion

        let dtProveedores;

        (function init() {
            initTablaProveedores();

            botonBuscar.click(buscarProveedores);
        })();

        function initTablaProveedores() {
            dtProveedores = tablaProveedores.DataTable({
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
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'proveedor', title: 'Proveedor' },
                    { data: 'proveedorDesc', title: 'Nombre' },
                    { data: 'direccion', title: 'Dirección' },
                    { data: 'telefono1', title: 'Teléfono' },
                    { data: 'email', title: 'Correo' },
                    { data: 'estado', title: 'Estado' },
                    { data: 'insumoDesc', title: 'Descripción' },
                    { data: 'categoria', title: 'Categoría' },
                    {
                        title: 'Fecha', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return row.fechaUltimaCompraString;
                            } else {
                                return row.fechaUltimaCompra;
                            }
                        }
                    },
                    {
                        data: 'ultimoPrecio', title: 'Precio', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return maskNumero6DCompras(data);
                            } else {
                                return data;
                            }
                        }
                    },
                    { data: 'proyectoDesc', title: 'Proyecto' }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        function buscarProveedores() {
            let listaInsumos = textareaInsumos.val().replace(/[,\s+\.\s+;\s+]+/g, ',').split(/[\.\;\n,\s+]/);

            axios.post('/OrdenCompra/GetProveedoresInsumos', { listaInsumos })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        dtProveedores.clear().draw();
                        dtProveedores.rows.add(response.data.data).draw(false);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
    }
    $(document).ready(() => OrdenCompra.ReporteUltimaCompraInsumo = new ReporteUltimaCompraInsumo())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();