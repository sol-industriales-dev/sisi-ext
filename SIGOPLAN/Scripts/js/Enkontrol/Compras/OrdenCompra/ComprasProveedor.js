(() => {
    $.namespace('Enkontrol.OrdenCompra.ComprasProveedor');
    ComprasProveedor = function () {
        //#region Selectores
        const selectCC = $('#selectCC');
        const selectProveedor = $('#selectProveedor');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const selectAreaCuenta = $('#selectAreaCuenta');
        const botonBuscar = $('#botonBuscar');
        const tablaCompras = $('#tablaCompras');
        //#endregion

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);

        function init() {
            $('.select2').select2();
            $.fn.dataTable.moment('DD/MM/YYYY');
            initTableCompras();
            selectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCc', null, false, null);
            selectProveedor.fillCombo('/Enkontrol/OrdenCompra/FillComboProveedores', null, false, null);
            inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioAnio);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
            botonBuscar.click(cargarCompras);
            selectCC.change(cargarProveedores);
            inputFechaInicio.change(cargarProveedores);
            inputFechaFin.change(cargarProveedores);
            selectProveedor.change(cargarAreasCuentas);

            cargarAreasCuentas();

            ocultarFiltroAreaCuenta();
        }

        function cargarProveedores() {
            axios.post('GetProveedoresCC', { cc: selectCC.val(), fechaInicio: inputFechaInicio.val(), fechaFin: inputFechaFin.val() })
                .then(response => {
                    let { success, datos } = response.data;

                    if (success) {
                        selectProveedor.fillCombo(response.data, null, false, null);
                        cargarAreasCuentas(); //Se ejecuta la función de cargar el combo de área-cuenta porque están cambiando los valores de los filtros cc, fecha y proveedor.
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarAreasCuentas() {
            axios.post('GetAreasCuentasCCFechaProveedor', { cc: selectCC.val(), fechaInicio: inputFechaInicio.val(), fechaFin: inputFechaFin.val(), proveedor: +selectProveedor.val() })
                .then(response => {
                    let { success, datos } = response.data;

                    if (success) {
                        selectAreaCuenta.fillCombo(response.data, null, false, null);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initTableCompras() {
            tablaCompras.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                scrollY: '45vh',
                scrollCollapse: true,
                bLengthChange: false,
                bInfo: false,
                searching: false,
                dom: 'Bfrtip',
                buttons: [{ extend: 'excelHtml5', exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8] } }],
                initComplete: function (settings, json) {

                },
                columns: [
                    {
                        title: 'O.C.', render: function (data, type, row, meta) {
                            return row.cc + '-' + row.numero;
                        }
                    },
                    { data: 'fechaString', title: 'FECHA' },
                    { data: 'compradorDesc', title: 'COMPRADOR' },
                    { data: 'proveedorDesc', title: 'PROVEEDOR' },
                    { data: 'tipoCompraDesc', title: 'TIPO COMPRA' },
                    // { data: 'detalle', title: 'DETALLE' },
                    // { data: 'rubro', title: 'RUBRO' },
                    { data: 'subTotalPesos', title: 'SUBTOTAL (M.N)' },
                    { data: 'subTotalDolares', title: 'SUBTOTAL (DLLS)' },
                    { data: 'totalPesos', title: 'TOTAL (M.N)' },
                    { data: 'totalDolares', title: 'TOTAL (DLLS)' }
                ],
                columnDefs: [
                    // {
                    //     render: function (data, type, row) {
                    //         if (data == null) {
                    //             return '';
                    //         } else {
                    //             return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                    //         }
                    //     },
                    //     targets: [0]
                    // },
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarCompras() {
            let cc = selectCC.val();

            let fechaInicio = inputFechaInicio.val();
            let fechaFin = inputFechaFin.val();
            let proveedor = +selectProveedor.val();
            let area = 0;
            let cuenta = 0;

            if (selectAreaCuenta.val() != '') {
                area = +selectAreaCuenta.find('option:selected').val();
                cuenta = +selectAreaCuenta.find('option:selected').attr('data-prefijo');
            }

            // if (cc == '') {
            //     AlertaGeneral(`Alerta`, `Debe seleccionar un centro de costos.`);
            //     return;
            // }

            axios.post('/Enkontrol/OrdenCompra/GetComprasProveedor', { cc, fechaInicio, fechaFin, proveedor, area, cuenta })
                .then(response => {
                    let { success, data } = response.data;

                    if (success) {
                        AddRows(tablaCompras, data);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function ocultarFiltroAreaCuenta() {
            axios.get('checarEmpresaArrendadora')
                .then(response => {
                    $('#divAreaCuenta').css('display', response.data == 'True' ? 'block' : 'none');
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        init();
    }
    $(document).ready(() => {
        Enkontrol.OrdenCompra.ComprasProveedor = new ComprasProveedor();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); });
})();