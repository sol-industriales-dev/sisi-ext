(() => {
    $.namespace('TransferenciasBancarias.TransferenciasBancarias');
    TransferenciasBancarias = function () {
        //#region Selectores
        const inputProveedorInicial = $('#inputProveedorInicial');
        const inputProveedorFinal = $('#inputProveedorFinal');
        const botonBuscar = $('#botonBuscar');
        const tablaTransferencias = $('#tablaTransferencias');
        const inputFechaMovInicial = $('#inputFechaMovInicial');
        const inputFechaMovFinal = $('#inputFechaMovFinal');
        const botonGenerar = $('#botonGenerar');
        const botonCheques = $('#botonCheques');
        const modalCheques = $('#modalCheques');
        const botonGenerarCheques = $('#botonGenerarCheques');
        const selectCuentaBancaria = $('#selectCuentaBancaria');
        //#endregion

        let dtTransferencias;
        let hoy = new Date();

        (function init() {
            initTablaTransferencias();

            botonBuscar.click(cargarMovimientosProveedorAutorizados);
            botonGenerar.click(generarArchivos);
            botonCheques.click(() => { selectCuentaBancaria.val(''); modalCheques.modal('show'); });
            botonGenerarCheques.click(confirmarGeneracionCheques);

            selectCuentaBancaria.fillCombo('/Administrativo/TransferenciasBancarias/FillComboCuentasBancarias', null, false, null);

            inputFechaMovInicial.datepicker().datepicker("setDate", new Date(hoy.getFullYear(), hoy.getMonth(), 1));
            inputFechaMovFinal.datepicker().datepicker("setDate", hoy);
        })();

        inputProveedorInicial.on('focus', function () {
            $(this).select();
        });

        inputProveedorFinal.on('focus', function () {
            $(this).select();
        });

        function cargarMovimientosProveedorAutorizados() {
            let proveedorInicial = inputProveedorInicial.val();
            let proveedorFinal = inputProveedorFinal.val();
            let fechaInicial = inputFechaMovInicial.datepicker("getDate");
            let fechaFinal = inputFechaMovFinal.datepicker("getDate");

            axios.post('/Administrativo/TransferenciasBancarias/CargarMovimientosProveedorAutorizados', { proveedorInicial, proveedorFinal, fechaInicial, fechaFinal })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaTransferencias, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initTablaTransferencias() {
            dtTransferencias = tablaTransferencias.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                scrollY: '45vh',
                scrollX: true,
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaTransferencias.on('focus', '.inputSaldo, .inputMonto, .inputSaldoTotal, .inputMontoTotal', function () {
                        $(this).select();
                    });

                    tablaTransferencias.on('focus', '.inputReferencia', function () {
                        $(this).select();
                    });

                    tablaTransferencias.on('change', '.inputSaldoTotal, .inputMontoTotal', function () {
                        let valor = unmaskNumero($(this).val());

                        $(this).val(maskNumero(valor));
                    });

                    tablaTransferencias.on('change', '.inputSaldo', function () {
                        let valor = unmaskNumero($(this).val());
                        let rowData = dtTransferencias.row($(this).closest('tr')).data();
                        let renglonTotal = tablaTransferencias.find(`tbody tr.renglonTotal[numpro=${rowData.numpro}]`);

                        $(this).val(maskNumero(valor));

                        let valorTotal = 0;

                        tablaTransferencias.find(`tbody tr[numpro=${rowData.numpro}]`).not('.renglonTotal').each((idx, element) => {
                            valorTotal += unmaskNumero($(element).find('.inputSaldo').val());
                        });

                        $(renglonTotal).find('.inputSaldoTotal').val(maskNumero(valorTotal));
                    });

                    tablaTransferencias.on('change', '.inputMonto', function () {
                        let valor = unmaskNumero($(this).val());
                        let rowData = dtTransferencias.row($(this).closest('tr')).data();
                        let renglonTotal = tablaTransferencias.find(`tbody tr.renglonTotal[numpro=${rowData.numpro}]`);

                        $(this).val(maskNumero(valor));

                        let valorTotal = 0;

                        tablaTransferencias.find(`tbody tr[numpro=${rowData.numpro}]`).not('.renglonTotal').each((idx, element) => {
                            valorTotal += unmaskNumero($(element).find('.inputMonto').val());
                        });

                        $(renglonTotal).find('.inputMontoTotal').val(maskNumero(valorTotal));
                    });

                    tablaTransferencias.on('change', '.selectCuentaOrigen', function () {
                        let row = $(this).closest('tr');
                        // let rowData = dtTransferencias.row(row).data();
                        let rowData = $(row).data('rowData');

                        if ($(this).val() != '') {
                            let bancoOrigen = $(this).find('option:selected').text().split(' - ')[0];
                            let bancoDestino = rowData.bancoDesc;

                            if (bancoOrigen === bancoDestino) {
                                $(row).find('.selectOperacion').val(2);
                            } else {
                                $(row).find('.selectOperacion').val(3);
                            }
                        } else {
                            $(row).find('.selectOperacion').val('');
                        }
                    });

                    tablaTransferencias.on('click', '.checkBoxTodos', function () {
                        let row = $(this).closest('tr');
                        let seleccionado = $(this).prop('checked');
                        let numpro = $(row).attr('numpro');

                        tablaTransferencias.find(`tr[numpro="${numpro}"] .regular-checkbox`).prop('checked', seleccionado);
                    });
                },
                drawCallback: function (settings) {
                    axios.post('/Administrativo/TransferenciasBancarias/GetOperacionEnum').then(response => {
                        tablaTransferencias.find('.selectOperacion').fillCombo({ items: response.data.items.filter((x) => x.Value > 0) }, null, false, null);
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));

                    let datos = tablaTransferencias.DataTable().data().toArray();

                    if (datos.length > 0) {
                        let renglones = tablaTransferencias.find('tbody tr');

                        $(datos).each((i, renglonDato) => {
                            const dataBefore = datos[i - 1];
                            const data = datos[i];

                            if (i > 0) {
                                if (dataBefore.numpro !== data.numpro) {
                                    let lstProv = datos.filter(prov => dataBefore.numpro === prov.numpro);
                                    let sumaProvMonto = lstProv.reduce((suma, current) => suma + current.monto, 0);
                                    let sumaProvPlan = lstProv.reduce((suma, current) => suma + current.monto_plan, 0);

                                    $(renglones).eq(i).before(`
                                        <tr class="renglonTotal" numpro="${dataBefore.numpro}">
                                            <td colspan="7" class="fondoTotal">
                                                <div style="display: inline;">
                                                    <input type="checkbox" id="radioChequeTodos_${dataBefore.numpro}" name="radioChequeTodos_${dataBefore.numpro}" class="regular-checkbox checkBoxTodos">
                                                    <label for="radioChequeTodos_${dataBefore.numpro}"></label>
                                                </div>&nbsp;${dataBefore.numpro} - ${dataBefore.proveedor}<span style="float:right">Saldo del proveedor ${dataBefore.numpro < 9000 ? "M.N" : "DLL"}</span></td>
                                            <td class="text-right fondoTotal"><input class="form-control text-right inputSaldoTotal" value="${maskNumero(sumaProvMonto)}" style="height: 24px;"></td>
                                            <td class="text-right fondoTotal"><input class="form-control text-right inputMontoTotal" value="${maskNumero(sumaProvPlan)}" style="height: 24px;"></td>
                                            <td class="text-right fondoTotal">
                                                <select class="form-control selectCuentaOrigen" style="padding: 2px 12px; height: 24px;">
                                                    <option value="">--Seleccione--</option>
                                                    <option value="3" banco="3" cuenta="${dataBefore.cuentaBanorte}">BANORTE - ${dataBefore.cuentaBanorte}</option>
                                                    <option value="4" banco="4" cuenta="${dataBefore.cuentaSantander}">SANTANDER - ${dataBefore.cuentaSantander}</option>
                                                </select>
                                            </td>
                                            <td class="text-right fondoTotal"><select class="form-control selectOperacion" style="padding: 2px 12px; height: 24px;"></select></td>
                                            <td class="text-right fondoTotal"><input class="form-control inputReferencia" style="height: 24px;" value="${('0' + (new Date()).getDate()).slice(-2) + ('0' + ((new Date()).getMonth() + 1)).slice(-2)}"></td>
                                        </tr>
                                    `);

                                    let renglon = tablaTransferencias.find(`tbody tr[numpro="${dataBefore.numpro}"]`);

                                    $(renglon).data('rowData', {
                                        factura: dataBefore.factura,
                                        estado: dataBefore.estado,
                                        tm: dataBefore.tm,
                                        cc: dataBefore.cc,
                                        monto: sumaProvMonto,
                                        monto_plan: sumaProvPlan,
                                        programo: dataBefore.programo,
                                        autorizo: dataBefore.autorizo,
                                        proveedor: dataBefore.proveedor,
                                        cuenta: dataBefore.cuenta,
                                        clabe: dataBefore.clabe,
                                        banco: dataBefore.banco,
                                        bancoDesc: dataBefore.bancoDesc,
                                        numpro: dataBefore.numpro
                                    });
                                }

                                if (i == datos.length - 1) {
                                    let lstProv = datos.filter(prov => data.numpro === prov.numpro);
                                    let sumaProvMonto = lstProv.reduce((suma, current) => suma + current.monto, 0);
                                    let sumaProvPlan = lstProv.reduce((suma, current) => suma + current.monto_plan, 0);

                                    $(renglones).eq(i).after(`
                                        <tr class="renglonTotal" numpro="${data.numpro}">
                                            <td colspan="7" class="fondoTotal">
                                                <div style="display: inline;">
                                                    <input type="checkbox" id="radioChequeTodos_${data.numpro}" name="radioChequeTodos_${data.numpro}" class="regular-checkbox checkBoxTodos">
                                                    <label for="radioChequeTodos_${data.numpro}"></label>
                                                </div>&nbsp;${data.numpro} - ${data.proveedor}<span style="float:right">Saldo del proveedor ${data.numpro < 9000 ? "M.N" : "DLL"}</span></td>
                                            <td class="text-right fondoTotal"><input class="form-control text-right inputSaldoTotal" value="${maskNumero(sumaProvMonto)}" style="height: 24px;"></td>
                                            <td class="text-right fondoTotal"><input class="form-control text-right inputMontoTotal" value="${maskNumero(sumaProvPlan)}" style="height: 24px;"></td>
                                            <td class="text-right fondoTotal">
                                                <select class="form-control selectCuentaOrigen" style="padding: 2px 12px; height: 24px;">
                                                    <option value="">--Seleccione--</option>
                                                    <option value="3" banco="3" cuenta="${data.cuentaBanorte}">BANORTE - ${data.cuentaBanorte}</option>
                                                    <option value="4" banco="4" cuenta="${data.cuentaSantander}">SANTANDER - ${data.cuentaSantander}</option>
                                                </select>
                                            </td>
                                            <td class="text-right fondoTotal"><select class="form-control selectOperacion" style="padding: 2px 12px; height: 24px;"></select></td>
                                            <td class="text-right fondoTotal"><input class="form-control inputReferencia" style="height: 24px;" value="${('0' + (new Date()).getDate()).slice(-2) + ('0' + ((new Date()).getMonth() + 1)).slice(-2)}"></td>
                                        </tr>
                                    `);

                                    let renglon = tablaTransferencias.find(`tbody tr[numpro="${data.numpro}"]`);

                                    $(renglon).data('rowData', {
                                        factura: data.factura,
                                        estado: data.estado,
                                        tm: data.tm,
                                        cc: data.cc,
                                        monto: sumaProvMonto,
                                        monto_plan: sumaProvPlan,
                                        programo: data.programo,
                                        autorizo: data.autorizo,
                                        proveedor: data.proveedor,
                                        cuenta: data.cuenta,
                                        clabe: data.clabe,
                                        banco: data.banco,
                                        bancoDesc: data.bancoDesc,
                                        numpro: data.numpro
                                    });
                                }
                            } else {
                                if (datos.length == 1) {
                                    const data = datos[i];

                                    let lstProv = datos.filter(prov => data.numpro === prov.numpro);
                                    let sumaProvMonto = lstProv.reduce((suma, current) => suma + current.monto, 0);
                                    let sumaProvPlan = lstProv.reduce((suma, current) => suma + current.monto_plan, 0);

                                    $(renglones).eq(i).after(`
                                            <tr class="renglonTotal" numpro="${data.numpro}">
                                                <td colspan="7" class="fondoTotal">
                                                    <div style="display: inline;">
                                                        <input type="checkbox" id="radioChequeTodos_${data.numpro}" name="radioChequeTodos_${data.numpro}" class="regular-checkbox checkBoxTodos">
                                                        <label for="radioChequeTodos_${data.numpro}"></label>
                                                    </div>&nbsp;${data.numpro} - ${data.proveedor}<span style="float:right">Saldo del proveedor ${data.numpro < 9000 ? "M.N" : "DLL"}</span></td>
                                                <td class="text-right fondoTotal"><input class="form-control text-right inputSaldoTotal" value="${maskNumero(sumaProvMonto)}" style="height: 24px;"></td>
                                                <td class="text-right fondoTotal"><input class="form-control text-right inputMontoTotal" value="${maskNumero(sumaProvPlan)}" style="height: 24px;"></td>
                                                <td class="text-right fondoTotal">
                                                    <select class="form-control selectCuentaOrigen" style="padding: 2px 12px; height: 24px;">
                                                        <option value="">--Seleccione--</option>
                                                        <option value="3" banco="3" cuenta="${data.cuentaBanorte}">BANORTE - ${data.cuentaBanorte}</option>
                                                        <option value="4" banco="4" cuenta="${data.cuentaSantander}">SANTANDER - ${data.cuentaSantander}</option>
                                                    </select>
                                                </td>
                                                <td class="text-right fondoTotal"><select class="form-control selectOperacion" style="padding: 2px 12px; height: 24px;"></select></td>
                                                <td class="text-right fondoTotal"><input class="form-control inputReferencia" style="height: 24px;" value="${('0' + (new Date()).getDate()).slice(-2) + ('0' + ((new Date()).getMonth() + 1)).slice(-2)}"></td>
                                            </tr>
                                        `);

                                    let renglon = tablaTransferencias.find(`tbody tr[numpro="${data.numpro}"]`);

                                    $(renglon).data('rowData', {
                                        factura: data.factura,
                                        estado: data.estado,
                                        tm: data.tm,
                                        cc: data.cc,
                                        monto: sumaProvMonto,
                                        monto_plan: sumaProvPlan,
                                        programo: data.programo,
                                        autorizo: data.autorizo,
                                        proveedor: data.proveedor,
                                        cuenta: data.cuenta,
                                        clabe: data.clabe,
                                        banco: data.banco,
                                        bancoDesc: data.bancoDesc,
                                        numpro: data.numpro
                                    });
                                }
                            }
                        });
                    }
                },
                createdRow: function (row, rowData) {
                    if (!$(row).hasClass('renglonTotal')) {
                        $(row).attr('numpro', rowData.numpro);
                    }
                },
                columns: [
                    {
                        title: 'Cheque', render: function (data, type, row, meta) {
                            return !row.chequeGenerado ? `
                                <div>
                                    <input type="checkbox" id="radioCheque_${meta.row}" name="radioCheque_${meta.row}" class="regular-checkbox">
                                    <label for="radioCheque_${meta.row}"></label>
                                </div>` : ``;
                        }
                    },
                    { data: 'factura', title: 'Factura' },
                    {
                        title: 'Proveedor', render: function (data, type, row, meta) {
                            return `${row.numpro} - ${row.proveedor}`;
                        }
                    },
                    { data: 'fecha_timbrado', title: 'Fecha Timbrado' },
                    { data: 'fecha_validacion', title: 'Fecha Validación' },
                    { data: 'clabe', title: 'CLABE Destino' },
                    { data: 'bancoDesc', title: 'Banco Destino' },
                    {
                        data: 'monto', title: 'Saldo', render: function (data, type, row, meta) {
                            return `<input class="form-control text-right inputSaldo" value="${maskNumero(data)}" style="height: 24px;">`;
                        }
                    },
                    {
                        data: 'monto_plan', title: 'Monto', render: function (data, type, row, meta) {
                            return `<input class="form-control text-right inputMonto" value="${maskNumero(data)}" style="height: 24px;">`;
                        }
                    },
                    {
                        title: 'Cuenta Origen', render: function (data, type, row, meta) {
                            return '';
                        }
                    },
                    {
                        title: 'Operación', render: function (data, type, row, meta) {
                            return '';
                        }
                    },
                    {
                        title: 'Referencia', render: function (data, type, row, meta) {
                            return '';
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' },
                    { width: '10%', targets: [5, 6] },
                    { width: '5%', targets: 2 }
                ]
            });
        }

        function generarArchivos() {
            let registros = [];

            tablaTransferencias.find('tbody tr.renglonTotal').each(function (idx, row) {
                let rowData = $(row).data('rowData');
                let monto = unmaskNumero($(row).find('.inputSaldoTotal').val());
                let cuentaOrigen = $(row).find('.selectCuentaOrigen option:selected').attr('cuenta');
                let bancoOrigen = +$(row).find('.selectCuentaOrigen option:selected').attr('banco');
                let operacion = +$(row).find('.selectOperacion').val();
                let referencia = $(row).find('.inputReferencia').val();

                if (bancoOrigen > 0 && operacion > 0) {
                    registros.push({
                        numpro: rowData.numpro,
                        monto,
                        cuentaOrigen,
                        bancoOrigen,
                        operacion,
                        cuentaDestino: rowData.cuenta,
                        clabeDestino: rowData.clabe,
                        bancoDestino: rowData.banco,
                        referencia,
                        descripcion: rowData.proveedor
                    });
                }
            });

            if (registros.length == 0) {
                Alert2Warning('Debe seleccionar el formato de un registro o más.');
                return;
            }

            axios.post('/Administrativo/TransferenciasBancarias/CargarArchivoComprimido', { registros })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        location.href = `/Administrativo/TransferenciasBancarias/DescargarArchivoComprimido`;
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function confirmarGeneracionCheques() {
            let facturas = [];

            tablaTransferencias.find('tbody tr').not('.renglonTotal').each(function (idx, row) {
                let rowData = dtTransferencias.row(row).data();
                let renglonTotal = tablaTransferencias.find(`tbody tr.renglonTotal[numpro=${rowData.numpro}]`);
                let checkboxCheque = $(row).find('.regular-checkbox').prop('checked');
                let saldo = unmaskNumero($(row).find('.inputSaldo').val());
                let monto = unmaskNumero($(row).find('.inputMonto').val());
                let cuentaOrigen = $(renglonTotal).find('.selectCuentaOrigen option:selected').attr('cuenta');
                let bancoOrigen = +$(renglonTotal).find('.selectCuentaOrigen option:selected').attr('banco');
                let operacion = +$(renglonTotal).find('.selectOperacion').val();

                if (checkboxCheque) {
                    facturas.push({
                        factura: rowData.factura,
                        cc: rowData.cc,
                        monto: saldo,
                        monto_plan: monto,
                        tm: rowData.tm,
                        numpro: rowData.numpro,
                        cuenta: rowData.cuenta,
                        clabe: rowData.clabe,
                        cuentaOrigen,
                        bancoOrigen,
                        operacion,
                        referenciaoc: rowData.referenciaoc
                    });
                }
            });

            let cuentaBancaria = +selectCuentaBancaria.val();

            if (cuentaBancaria == 0 || isNaN(cuentaBancaria)) {
                Alert2Warning('Debe seleccionar una cuenta bancaria.');
                return;
            }

            if (facturas.length > 0) {
                // Alert2AccionConfirmar('ATENCIÓN', '¿Desea proceder con el guardado de los cheques?', 'Confirmar', 'Cancelar', () => {
                axios.post('GenerarCheques', { facturas, cuentaBancaria })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            Alert2Exito('Se ha guardado la información.');
                            modalCheques.modal('hide');
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
                // });
            } else {
                Alert2Warning('Debe seleccionar facturas para generar los cheques.');
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => TransferenciasBancarias.TransferenciasBancarias = new TransferenciasBancarias())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();