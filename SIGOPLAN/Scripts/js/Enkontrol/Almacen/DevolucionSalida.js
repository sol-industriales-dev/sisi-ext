(() => {
    $.namespace('Enkontrol.Almacen.Almacen.DevolucionSalida');
    DevolucionSalida = function () {
        //#region Selectores
        const inputAlmacenNum = $('#inputAlmacenNum');
        const inputAlmacenDesc = $('#inputAlmacenDesc');
        const inputCCNum = $('#inputCCNum');
        const inputCCDesc = $('#inputCCDesc');
        const inputEntregoNum = $('#inputEntregoNum');
        const inputEntregoDesc = $('#inputEntregoDesc');
        const inputComentarios = $('#inputComentarios');
        const inputNumero = $('#inputNumero');
        const inputFecha = $('#inputFecha');
        const inputTotal = $('#inputTotal');
        const tblPartidas = $('#tblPartidas');
        const btnAgregarInsumo = $('#btnAgregarInsumo');
        const btnQuitarInsumo = $('#btnQuitarInsumo');
        const btnGuardar = $('#btnGuardar');
        const inputOrdenCompra = $('#inputOrdenCompra');
        const inputProveedorNum = $('#inputProveedorNum');
        const inputProveedorDesc = $('#inputProveedorDesc');
        const mdlExistencias = $('#mdlExistencias');
        const tblExistencias = $('#tblExistencias');
        const btnImprimir = $('#btnImprimir');
        const report = $("#report");
        const btnVerCC = $('#btnVerCC');
        const mdlCentroCosto = $('#mdlCentroCosto');
        const tblCentroCosto = $('#tblCentroCosto');
        const inputEmpresaActual = $('#inputEmpresaActual');
        //#endregion

        _filaInsumo = null;

        (function init() {
            initTablePartidas();
            initTableExistencias();
            initTableCentroCosto();

            inputNumero.change(cargarInformacion);
            btnGuardar.click(guardarDevolucionSalida);
            btnImprimir.click(imprimirMovimiento);

            inputFecha.datepicker().datepicker();
        })();

        inputAlmacenNum.on('change', function () {
            let almacenID = inputAlmacenNum.val();

            if (almacenID != '' && !isNaN(almacenID)) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/ChecarAccesoAlmacenista', { almacen: almacenID })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            if (response.data != null) {
                                let flagTraspaso = response.data.some(function (x) {
                                    return x.traspasos == 1;
                                });

                                if (!flagTraspaso) {
                                    $.post('/Enkontrol/Almacen/GetNuevaDevolucionSalida', { almacenID })
                                        .then(response => {
                                            if (response.success) {
                                                inputAlmacenDesc.val(response.almacenDesc);
                                                inputNumero.val(response.numeroDisponible);
                                                inputEntregoNum.val(response.entregoNum);
                                                inputEntregoDesc.val(response.entregoDesc);
                                                inputFecha.val(response.fecha);
                                                inputTotal.val('$' + formatMoney(0));
                                            } else {
                                                AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                                            }
                                        }, error => {
                                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                                        }
                                        );
                                } else {
                                    AlertaGeneral(`Alerta`, `No tiene acceso a este tipo de movimiento para ese almacén.`);
                                    limpiarVista();
                                }
                            } else {
                                AlertaGeneral(`Alerta`, `No tiene acceso a ese almacén.`);
                                limpiarVista();
                            }
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                limpiarVista();
            }
        });

        inputCCNum.on('change', function () {
            let cc = inputCCNum.val();

            if (cc != '') {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GetCentroCosto', { cc })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            inputCCDesc.val(response.ccDesc);
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                inputCCNum.val('');
                inputCCDesc.val('');
            }
        });

        inputOrdenCompra.on('change', function () {
            let almacen = inputAlmacenNum.val();
            let cc = inputCCNum.val();
            let numeroOrdenCompra = inputOrdenCompra.val();

            if ((almacen != '' && !isNaN(almacen)) && cc != '' && (numeroOrdenCompra != '' && !isNaN(numeroOrdenCompra))) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GetEntradasCompra', { almacen, cc, numeroOrdenCompra }).always($.unblockUI).then(response => {
                    if (response.success) {
                        inputProveedorNum.val(response.data[0].proveedor);
                        inputProveedorDesc.val(response.data[0].proveedorDesc);

                        var listaPartidas = [];
                        var contadorPartida = 1;

                        response.data.forEach(x => {
                            x.detalle.forEach(y => {
                                y.partida = contadorPartida++;
                                listaPartidas.push(y);
                            });
                        });

                        AddRows(tblPartidas, listaPartidas);

                        calcularTotal();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            }
        });

        btnAgregarInsumo.on('click', function () {
            let datos = tblPartidas.DataTable().rows().data();

            datos.push({
                'partida': datos.length + 1,
                'insumo': '',
                'insumoDesc': '',
                'cantidad': 0,
                'unidad': '',
                'precio': 0,
                'importe': 0,
                'area_alm': '',
                'lado_alm': '',
                'estante_alm': '',
                'nivel_alm': ''
            });

            tblPartidas.DataTable().clear();
            tblPartidas.DataTable().rows.add(datos).draw();
        });

        btnQuitarInsumo.on('click', function () {
            tblPartidas.DataTable().row(tblPartidas.find("tr.selected")).remove().draw();

            let cuerpo = tblPartidas.find('tbody');

            if (cuerpo.find("tr").length == 0) {
                tblPartidas.DataTable().draw();
            } else {
                tblPartidas.find('tbody tr').each(function (idx, row) {
                    let rowData = tblPartidas.DataTable().row(row).data();

                    if (rowData != undefined) {
                        rowData.partida = ++idx;

                        tblPartidas.DataTable().row(row).data(rowData).draw();

                        recargarAutoComplete(row);
                    }
                });
            }
        });

        btnVerCC.on('click', function () {
            cargarCentrosCosto();
        });

        tblPartidas.on('change', '.inputInsumo', function () {
            let insumo = $(this).val();
            let almacen = inputAlmacenNum.val();

            if (insumo.length == 7) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Requisicion/GetInsumoInformacionByAlmacen', { insumo, almacen })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            let ins = response.data;
                            let row = $(this).closest('tr');

                            row.find('.inputInsumo').val(ins.value);
                            row.find('.inputInsumoDesc').val(ins.id);
                            row.find('td:eq(4)').text(ins.unidad);

                            let rowData = tblPartidas.DataTable().row(row).data();

                            let insumoNumero = $(row).find('.inputInsumo').val();
                            let insumoDesc = $(row).find('.inputInsumoDesc').val();
                            let nuevaCantidad = !isNaN($(row).find('.inputCantidad').val()) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;
                            let unidad = $(row).find('td:eq(4)').text();
                            let nuevoImporte = nuevaCantidad * rowData.precio;

                            rowData.insumo = insumoNumero;
                            rowData.insumoDesc = insumoDesc;
                            rowData.cantidad = nuevaCantidad;
                            rowData.unidad = unidad;
                            rowData.precio = rowData.precio;
                            rowData.importe = nuevoImporte;

                            tblPartidas.DataTable().row(row).data(rowData).draw();

                            recargarAutoComplete(row);
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información del insumo.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        });

        function initTablePartidas() {
            tblPartidas.DataTable({
                retrieve: true,
                bLengthChange: false,
                deferRender: true,
                dom: 'lrtp',
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                paging: false,
                initComplete: function (settings, json) {
                    tblPartidas.on('click', 'td', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblPartidas.DataTable().row(row).data();

                        if (row.hasClass('selected')) {
                            row.removeClass('selected');
                        } else {
                            tblPartidas.DataTable().$('tr.selected').removeClass('selected');
                            row.addClass('selected');
                        }
                    });

                    tblPartidas.on('change', '.inputCantidad', function () {
                        tblPartidas.find('tbody tr').each(function (index, row) {
                            let rowData = tblPartidas.DataTable().row(row).data();

                            let insumo = $(row).find('.inputInsumo').val();
                            let insumoDesc = $(row).find('.inputInsumoDesc').val();
                            let nuevaCantidad = !isNaN(unmaskNumero($(row).find('.inputCantidad').val())) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;
                            let unidad = $(row).find('td:eq(4)').text();
                            let nuevoImporte = nuevaCantidad * rowData.precio;
                            let area_alm = $(row).find('.inputArea').val();
                            let lado_alm = $(row).find('.inputLado').val();
                            let estante_alm = $(row).find('.inputEstante').val();
                            let nivel_alm = $(row).find('.inputNivel').val();

                            rowData.insumo = insumo;
                            rowData.insumoDesc = insumoDesc;
                            rowData.cantidad = nuevaCantidad;
                            rowData.unidad = unidad;
                            rowData.importe = nuevoImporte;
                            rowData.area_alm = area_alm;
                            rowData.lado_alm = lado_alm;
                            rowData.estante_alm = estante_alm;
                            rowData.nivel_alm = nivel_alm;

                            tblPartidas.DataTable().row(row).data(rowData).draw();

                            recargarAutoComplete(row);
                        });

                        calcularTotal();
                    });

                    tblPartidas.on('focus', 'input', function () {
                        $(this).select();
                    });

                    tblPartidas.on('keyup', '.inputArea, .inputLado, .inputEstante, .inputNivel', function () {
                        $(this).val($(this).val().toUpperCase());
                    });

                    tblPartidas.on('change', '.inputArea, .inputLado, .inputEstante, .inputNivel', function () {
                        let row = $(this).closest('tr');
                        $(row).find('.inputCantidad').change();
                    });

                    tblPartidas.on('click', '.btnExistencias', function () {
                        let row = $(this).closest('tr');

                        _filaInsumo = row;

                        cargarExistencias();
                    });
                },
                createdRow: function (row, rowData) {
                    let inputInsumo = $(row).find('.inputInsumo');
                    let inputInsumoDesc = $(row).find('.inputInsumoDesc');

                    inputInsumo.getAutocomplete(setInsumoDesc, { cc: inputCCNum.val() }, '/Enkontrol/Requisicion/getInsumos');
                    inputInsumoDesc.getAutocomplete(setInsumoBusqPorDesc, { cc: inputCCNum.val() }, '/Enkontrol/Requisicion/getInsumosDesc');
                },
                columns: [
                    { data: 'partida', title: 'Pda' },
                    {
                        data: 'insumo', title: 'Insumo', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputInsumo');

                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'insumoDesc', title: 'Descripción', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputInsumoDesc');

                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'cantidad', title: 'Cantidad', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputCantidad');

                            $(input).attr('value', formatMoney(data));

                            return input.outerHTML;
                        }
                    },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        data: 'precio', title: 'Precio', render: function (data, type, row, meta) {
                            if (!isNaN(data)) {
                                return '$' + formatMoney(data);
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'importe', title: 'Importe', render: function (data, type, row, meta) {
                            if (!isNaN(data)) {
                                return '$' + formatMoney(data);
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'area_alm', title: 'Área', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputArea');

                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'lado_alm', title: 'Lado', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputLado');

                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'estante_alm', title: 'Estante', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputEstante');

                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'nivel_alm', title: 'Nivel', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputNivel');

                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        title: 'Existencias', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-default btnExistencias"><i class="fa fa-bars"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });

            if (inputEmpresaActual.val() == 6) {
                // tblPartidas.DataTable().column(11).visible(false);
            }
        }

        function initTableExistencias() {
            tblExistencias.DataTable({
                retrieve: true,
                deferRender: true,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblExistencias.on('click', '.btnSeleccionarUbicacion', function () {
                        let rowData = tblExistencias.DataTable().row($(this).closest('tr')).data();

                        if (_filaInsumo != null) {
                            $(_filaInsumo).find('.inputArea').val(rowData.area_alm);
                            $(_filaInsumo).find('.inputLado').val(rowData.lado_alm);
                            $(_filaInsumo).find('.inputEstante').val(rowData.estante_alm);
                            $(_filaInsumo).find('.inputNivel').val(rowData.nivel_alm);
                            $(_filaInsumo).find('.inputCantidad').change();

                            mdlExistencias.modal('hide');
                        }
                    });
                },
                columns: [
                    { data: 'insumoDesc', title: 'Insumo' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'area_alm', title: 'Área' },
                    { data: 'lado_alm', title: 'Lado' },
                    { data: 'estante_alm', title: 'Estante' },
                    { data: 'nivel_alm', title: 'Nivel' },
                    {
                        title: 'Seleccionar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-primary btnSeleccionarUbicacion"><i class="fa fa-arrow-right"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableCentroCosto() {
            tblCentroCosto.DataTable({
                retrieve: true,
                deferRender: true,
                language: dtDicEsp,
                bInfo: false,
                bLengthChange: false,
                initComplete: function (settings, json) {
                    tblCentroCosto.on('click', '.btnSeleccionarCC', function () {
                        let rowData = tblCentroCosto.DataTable().row($(this).closest('tr')).data();

                        inputCCNum.val(rowData.cc);
                        inputCCDesc.val(rowData.descripcion);

                        mdlCentroCosto.modal('hide');
                    });
                },
                columns: [
                    { data: 'cc', title: 'Centro de Costo' },
                    { data: 'descripcion', title: 'Descripción' },
                    {
                        title: 'Seleccionar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-primary btnSeleccionarCC"><i class="fa fa-arrow-right"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarInformacion() {
            let almacenID = inputAlmacenNum.val();
            let numero = inputNumero.val();

            limpiarInformacion();
            tblPartidas.DataTable().clear().draw();

            if ((almacenID != '' && !isNaN(almacenID)) && (numero != '' && !isNaN(numero))) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GetDevolucionSalida', { almacenID, numero })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            llenarInformacion(response.data);
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                limpiarInformacion();
                tblPartidas.DataTable().clear().draw();
            }
        }

        function limpiarVista() {
            inputAlmacenNum.val('');
            inputAlmacenDesc.val('');
            inputCCNum.val('');
            inputCCDesc.val('');
            inputEntregoNum.val('');
            inputEntregoDesc.val('');
            inputOrdenCompra.val('');
            inputProveedorNum.val('');
            inputProveedorDesc.val('');
            inputComentarios.val('');
            inputNumero.val('');
            inputFecha.val('');
            inputTotal.val('');

            tblPartidas.DataTable().clear().draw();
        }

        function limpiarInformacion() {
            // inputAlmacenNum.val('');
            // inputAlmacenDesc.val('');
            inputCCNum.val('');
            inputCCDesc.val('');
            inputEntregoNum.val('');
            inputEntregoDesc.val('');
            inputOrdenCompra.val('');
            inputProveedorNum.val('');
            inputProveedorDesc.val('');
            inputComentarios.val('');
            // inputNumero.val('');
            inputFecha.val('');
            inputTotal.val('');
        }

        function llenarInformacion(data) {
            inputAlmacenNum.val(data.almacen);
            inputAlmacenDesc.val(data.almacenDesc);
            inputCCNum.val(data.cc);
            inputCCDesc.val(data.ccDesc);
            inputEntregoNum.val(data.empleado);
            inputEntregoDesc.val(data.empleadoDesc);
            inputOrdenCompra.val(data.orden_ct);
            inputProveedorNum.val(data.proveedor);
            inputProveedorDesc.val(data.proveedorDesc);
            inputComentarios.val(data.comentarios);
            inputNumero.val(data.numero);
            inputFecha.val(data.fechaString);
            inputTotal.val('$' + formatMoney(data.total));

            AddRows(tblPartidas, data.detalle);
        }

        function setInsumoDesc(e, ui) {
            let row = $(this).closest('tr');

            row.find('.inputInsumo').val(ui.item.value);
            row.find('.inputInsumoDesc').val(ui.item.id);
            row.find('td:eq(4)').text(ui.item.unidad);

            let rowData = tblPartidas.DataTable().row(row).data();

            let insumo = $(row).find('.inputInsumo').val();
            let insumoDesc = $(row).find('.inputInsumoDesc').val();
            let nuevaCantidad = !isNaN($(row).find('.inputCantidad').val()) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;
            let unidad = $(row).find('td:eq(4)').text();
            let nuevoImporte = nuevaCantidad * rowData.precio;

            rowData.insumo = insumo;
            rowData.insumoDesc = insumoDesc;
            rowData.cantidad = nuevaCantidad;
            rowData.unidad = unidad;
            rowData.importe = nuevoImporte;

            tblPartidas.DataTable().row(row).data(rowData).draw();

            recargarAutoComplete(row);
        }

        function setInsumoBusqPorDesc(e, ui) {
            let row = $(this).closest('tr');

            row.find('.inputInsumo').val(ui.item.id);
            row.find('.inputInsumoDesc').val(ui.item.descripcion);
            row.find('td:eq(4)').text(ui.item.unidad);

            let rowData = tblPartidas.DataTable().row(row).data();

            let insumo = $(row).find('.inputInsumo').val();
            let insumoDesc = $(row).find('.inputInsumoDesc').val();
            let nuevaCantidad = !isNaN($(row).find('.inputCantidad').val()) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;
            let unidad = $(row).find('td:eq(4)').text();
            let nuevoImporte = nuevaCantidad * rowData.precio;

            rowData.insumo = insumo;
            rowData.insumoDesc = insumoDesc;
            rowData.cantidad = nuevaCantidad;
            rowData.unidad = unidad;
            rowData.importe = nuevoImporte;

            tblPartidas.DataTable().row(row).data(rowData).draw();

            recargarAutoComplete(row);

            //Retornar falso para que no se ejecute la función de select por default de JQuery UI.
            return false;
        }

        function validarInsumo(e, ul) {
            if (ul.item == null) {
                let row = $(this).closest('tr');

                row.find('.inputInsumo').val('');
                row.find('.inputInsumoDesc').val('');
            }
        }

        function guardarDevolucionSalida() {
            let movimiento = getInformacion();

            if (movimiento.detalle.length > 0) {
                btnGuardar.attr('disabled', true);

                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GuardarDevolucionSalida', { movimiento })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            // btnGuardar.attr('disabled', false);
                            if (response.flagMaquinaStandBy) {
                                AlertaGeneral(`Alerta`, `Se ha guardado la información. Se quitó el estado "Stand-By" de la máquina "${movimiento.cc}".`);
                            } else {
                                AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                            }

                            limpiarVista();
                            verReporte();
                        } else {
                            AlertaGeneral(`Alerta`, `${response.message}`);
                            // btnGuardar.attr('disabled', false);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        btnGuardar.attr('disabled', false);
                    }
                    );
            }
        }

        function getInformacion() {
            let movimiento = {
                almacen: inputAlmacenNum.val(),
                almacenDesc: inputAlmacenDesc.val(),
                tipo_mov: 53,
                numero: inputNumero.val(), //Posible número disponible
                cc: inputCCNum.val(),
                ccDesc: inputCCDesc.val(),
                orden_ct: inputOrdenCompra.val(),
                proveedor: inputProveedorNum.val() != '' ? inputProveedorNum.val() : 9999,
                fecha: null, //Valor asignado en el back
                fechaString: '', //Valor asignado en el back
                total: unmaskNumero6DCompras(inputTotal.val()),
                estatus: 'A',
                transferida: 'N',
                empleado: 0, //Valor asignado en el back
                empleadoDesc: '', //Valor asignado en el back
                comentarios: inputComentarios.val(),
                sector_id: null
            };

            movimiento.detalle = getPartidas();

            return movimiento;
        }

        function getPartidas() {
            let detalle = [];

            tblPartidas.find('tbody tr').each((index, row) => {
                let rowData = tblPartidas.DataTable().row(row).data();

                if (rowData != undefined) {
                    let partida = rowData.partida;
                    let insumo = $(row).find('.inputInsumo').val();
                    let cantidad = unmaskNumero($(row).find('.inputCantidad').val());

                    let precio = rowData.precio;
                    let importe = rowData.importe;

                    let area_alm = $(row).find('.inputArea').val();
                    let lado_alm = $(row).find('.inputLado').val();
                    let estante_alm = $(row).find('.inputEstante').val();
                    let nivel_alm = $(row).find('.inputNivel').val();

                    let partida_oc = rowData.partida_oc != null ? rowData.partida_oc : 0;

                    if (insumo != '' && !isNaN(insumo) && cantidad > 0) {
                        detalle.push({
                            almacen: inputAlmacenNum.val(),
                            tipo_mov: 53,
                            numero: inputNumero.val(), //Posible número disponible
                            partida: partida,
                            insumo: insumo,
                            comentarios: '',
                            cantidad: cantidad,
                            precio: precio,
                            importe: importe,
                            sector_id: null,
                            area_alm: area_alm,
                            lado_alm: lado_alm,
                            estante_alm: estante_alm,
                            nivel_alm: nivel_alm,
                            partida_oc: partida_oc
                        });
                    }
                }
            });

            return detalle;
        }

        function calcularTotal() {
            let datos = tblPartidas.DataTable().rows().data();
            let total = 0;

            datos.toArray().forEach(function (element) {
                total += element.importe;
            });

            inputTotal.val('$' + formatMoney(total));
        }

        function recargarAutoComplete(row) {
            let inputInsumo = $(row).find('.inputInsumo');
            let inputInsumoDesc = $(row).find('.inputInsumoDesc');

            inputInsumo.getAutocomplete(setInsumoDesc, { cc: inputCCNum.val() }, '/Enkontrol/Requisicion/getInsumos');
            inputInsumoDesc.getAutocomplete(setInsumoBusqPorDesc, { cc: inputCCNum.val() }, '/Enkontrol/Requisicion/getInsumosDesc');
        }

        function verReporte() {
            $.blockUI({ message: 'Procesando...' });
            report.attr("src", `/Reportes/Vista.aspx?idReporte=122`);
            document.getElementById('report').onload = function () {
                openCRModal();
                $.unblockUI;
            };
        }

        function cargarExistencias() {
            if (_filaInsumo != null) {
                let rowData = tblPartidas.DataTable().row(_filaInsumo).data();
                let cc = inputCCNum.val();
                let almacenID = inputAlmacenNum.val();
                let insumo = rowData.insumo;

                if (cc != '' && almacenID > 0) {
                    $.blockUI({ message: 'Procesando...' });
                    $.post('/Enkontrol/Requisicion/GetUbicacionDetalle', { cc, almacenID, insumo })
                        .always($.unblockUI)
                        .then(response => {
                            if (response.success) {
                                let existenciasFiltradas = $(response.data).filter(function (id, element) {
                                    return element.cantidad > 0;
                                });

                                AddRows(tblExistencias, existenciasFiltradas);
                                mdlExistencias.modal('show');
                            } else {
                                AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                } else {
                    if (cc == '') {
                        AlertaGeneral(`Alerta`, `Centro de Costo inválido.`);
                    } else if (almacenID == 0) {
                        AlertaGeneral(`Alerta`, `Número de almacén inválido.`);
                    }
                }
            }
        }

        function imprimirMovimiento() {
            let almacen = inputAlmacenNum.val();
            let numero = inputNumero.val();

            if (almacen > 0 && numero > 0) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/ImprimirMovimientoSalidaDevolucion', { almacen, numero })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            verReporte();
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                AlertaGeneral(`Alerta`, `Seleccione un almacén y un número de movimiento válido.`);
            }
        }

        function cargarCentrosCosto() {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Almacen/GetCentrosCostoModal')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tblCentroCosto, response.data);
                        tblCentroCosto.DataTable().search('').draw();
                        mdlCentroCosto.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function formatMoney(amount, decimalCount = 6, decimal = ".", thousands = ",") {
            try {
                decimalCount = Math.abs(decimalCount);
                decimalCount = isNaN(decimalCount) ? 2 : decimalCount;

                const negativeSign = amount < 0 ? "-" : "";

                let i = parseInt(amount = Math.abs(Number(amount) || 0).toFixed(decimalCount)).toString();
                let j = (i.length > 3) ? i.length % 3 : 0;

                return negativeSign + (j ? i.substr(0, j) + thousands : '') + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousands) + (decimalCount ? decimal + Math.abs(amount - i).toFixed(decimalCount).slice(2) : "");
            } catch (e) {
                console.log(e)
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }
    }
    $(document).ready(() => Enkontrol.Almacen.Almacen.DevolucionSalida = new DevolucionSalida())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();