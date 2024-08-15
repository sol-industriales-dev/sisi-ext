(() => {
    $.namespace('Enkontrol.Almacen.Almacen.DevolucionEntrada');
    DevolucionEntrada = function () {
        //#region Selectores
        const inputAlmacenNum = $('#inputAlmacenNum');
        const inputAlmacenDesc = $('#inputAlmacenDesc');
        const inputCCNum = $('#inputCCNum');
        const inputCCDesc = $('#inputCCDesc');
        const inputRecibioNum = $('#inputRecibioNum');
        const inputRecibioDesc = $('#inputRecibioDesc');
        const inputComentarios = $('#inputComentarios');
        const inputNumero = $('#inputNumero');
        const inputSectorNum = $('#inputSectorNum');
        const inputSectorDesc = $('#inputSectorDesc');
        const inputFecha = $('#inputFecha');
        const inputTotal = $('#inputTotal');
        const tblPartidas = $('#tblPartidas');
        const btnAgregarInsumo = $('#btnAgregarInsumo');
        const btnQuitarInsumo = $('#btnQuitarInsumo');
        const btnGuardar = $('#btnGuardar');
        const mdlHistorialInsumo = $('#mdlHistorialInsumo');
        const tblHistorialInsumo = $('#tblHistorialInsumo');
        const mdlCatalogoUbicaciones = $('#mdlCatalogoUbicaciones');
        const tblCatalogoUbicaciones = $('#tblCatalogoUbicaciones');
        const report = $("#report");
        const btnVerCC = $('#btnVerCC');
        const mdlCentroCosto = $('#mdlCentroCosto');
        const tblCentroCosto = $('#tblCentroCosto');
        const inputEmpresaActual = $('#inputEmpresaActual');
        //#endregion

        _filaInsumo = null;

        (function init() {
            initTablePartidas();
            initTableHistorialInsumo();
            initTableCatalogoUbicaciones();
            initTableCentroCosto();
            OcultarColumnaAreacuentaHistorialCatalogo();
            inputNumero.change(cargarInformacion);
            btnGuardar.click(guardarDevolucionEntrada);

            inputFecha.datepicker().datepicker();
        })();

        inputAlmacenNum.on('change', function () {
            let almacenID = inputAlmacenNum.val();
            let CAALMA = inputAlmacenNum.val();

            if (almacenID != '' && !isNaN(almacenID)) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GetNuevaDevolucionEntrada', { almacenID, CAALMA })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            inputAlmacenDesc.val(response.almacenDesc);
                            inputNumero.val(response.numeroDisponible);
                            inputRecibioNum.val(response.recibioNum);
                            inputRecibioDesc.val(response.recibioDesc);
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

        btnAgregarInsumo.on('click', function () {
            let datos = tblPartidas.DataTable().rows().data();
            if (inputEmpresaActual.val() != 6) {
                tblPartidas.DataTable().row.add({
                    'partida': datos.length + 1,
                    'insumo': '',
                    'insumoDesc': '',
                    'areaCuenta': '',
                    'noEconomico': '',
                    'cantidad': 0,
                    'unidad': '',
                    'costo_prom': 0,
                    'importe': 0,
                    'area_alm': '',
                    'lado_alm': '',
                    'estante_alm': '',
                    'nivel_alm': ''
                }).draw();
            } else {
                tblPartidas.DataTable().row.add({
                    'partida': datos.length + 1,
                    'insumo': '',
                    'insumoDesc': '',
                    'noEconomico': '',
                    'cantidad': 0,
                    'unidad': '',
                    'costo_prom': 0,
                    'importe': 0,
                    'area_alm': '',
                    'lado_alm': '',
                    'estante_alm': '',
                    'nivel_alm': ''
                }).draw();
            }
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
                    }

                    recargarAutoComplete(row);
                });
            }
        });

        btnVerCC.on('click', function () {
            cargarCentrosCosto();
        });

        tblPartidas.on('change', '.inputInsumo', function () {
            let insumo = $(this).val();
            let almacen = inputAlmacenNum.val();

            if (inputEmpresaActual.val() != 6) {
                if (insumo.length != 7) {
                    return;
                }
            } else {
                if (insumo.length != 11) {
                    return;
                }
            }

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Requisicion/GetInsumoInformacionByAlmacenEntrada', { insumo, almacen }).always($.unblockUI).then(response => {
                if (response.success) {
                    let ins = response.data;
                    let row = $(this).closest('tr');

                    row.find('.inputInsumo').val(ins.value);
                    row.find('.inputInsumoDesc').val(ins.id);
                    row.find(`td:eq(${inputEmpresaActual.val() != 6 ? '5' : '4'})`).text(ins.unidad);
                    row.find(`td:eq(${inputEmpresaActual.val() != 6 ? '6' : '5'})`).text(ins.costoPromedio);

                    let rowData = tblPartidas.DataTable().row(row).data();

                    let insumoNumero = $(row).find('.inputInsumo').val();
                    let insumoDesc = $(row).find('.inputInsumoDesc').val();
                    let nuevaCantidad = !isNaN($(row).find('.inputCantidad').val()) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;
                    let unidad = $(row).find(`td:eq(${inputEmpresaActual.val() != 6 ? '5' : '4'})`).text();
                    let costo_promedio = unmaskNumero($(row).find(`td:eq(${inputEmpresaActual.val() != 6 ? '6' : '5'})`).text());
                    let nuevoImporte = nuevaCantidad * costo_promedio;

                    rowData.insumo = insumoNumero;
                    rowData.insumoDesc = insumoDesc;
                    rowData.cantidad = nuevaCantidad;
                    rowData.unidad = unidad;
                    rowData.costo_prom = costo_promedio;
                    rowData.importe = nuevoImporte;
                    rowData.PERU_insumo = insumoNumero;

                    tblPartidas.DataTable().row(row).data(rowData).draw();

                    recargarAutoComplete(row);
                } else {
                    AlertaGeneral(`Alerta`, `Error al consultar la información del insumo.`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        });

        function OcultarColumnaAreacuentaHistorialCatalogo() {
            if (inputEmpresaActual.val() == 6) {
                // tblPartidas.DataTable().column(3).visible(false);
                // tblPartidas.DataTable().column(12).visible(false);
                // tblPartidas.DataTable().column(13).visible(false);
            } else {
                tblPartidas.DataTable().column(3).visible(true);
                tblPartidas.DataTable().column(12).visible(true);
                tblPartidas.DataTable().column(13).visible(true);
            }

        }

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

                    tblPartidas.on('keyup', '.inputArea, .inputLado, .inputEstante, .inputNivel', function () {
                        $(this).val($(this).val().toUpperCase());
                    });

                    tblPartidas.on('change', '.inputArea, .inputLado, .inputEstante, .inputNivel', function () {
                        let row = $(this).closest('tr');
                        $(row).find('.inputCantidad').change();
                    });
                    tblPartidas.on('click', '.btnHistorialInsumo', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblPartidas.DataTable().row(row).data();

                        _filaInsumo = row;

                        cargarHistorialInsumo(row);
                    });
                    tblPartidas.on('click', '.btnCatalogoUbicaciones', function () {
                        let row = $(this).closest('tr');

                        _filaInsumo = row;

                        cargarCatalogoUbicaciones();
                    });
                    tblPartidas.on('change', '.inputCantidad', function () {
                        tblPartidas.find('tbody tr').each(function (index, row) {
                            let rowData = tblPartidas.DataTable().row(row).data();

                            let insumo = $(row).find('.inputInsumo').val();
                            let insumoDesc = $(row).find('.inputInsumoDesc').val();
                            if (inputEmpresaActual.val() != 6 && inputEmpresaActual.val() != 3) {
                                let area = parseInt($(row).find('.selectAreaCuenta option:selected').val());
                                let cuenta = parseInt($(row).find('.selectAreaCuenta option:selected').attr('data-prefijo'));
                                rowData.area = area;
                                rowData.cuenta = cuenta;
                            } else {
                                rowData.noEconomico = $(row).find('.selectAreaCuenta option:selected').val();
                            }

                            let nuevaCantidad = unmaskNumero($(row).find('.inputCantidad').val());
                            let costo_prom = rowData.costo_prom;
                            let nuevoImporte = nuevaCantidad * costo_prom;
                            let area_alm = $(row).find('.inputArea').val();
                            let lado_alm = $(row).find('.inputLado').val();
                            let estante_alm = $(row).find('.inputEstante').val();
                            let nivel_alm = $(row).find('.inputNivel').val();
                            let unidad = $(row).find(`td:eq(${inputEmpresaActual.val() != 6 ? '5' : '4'})`).text();
                            rowData.insumo = insumo;
                            rowData.insumoDesc = insumoDesc;
                            rowData.cantidad = nuevaCantidad;
                            rowData.costo_prom = costo_prom;
                            rowData.importe = nuevoImporte;
                            rowData.area_alm = area_alm;
                            rowData.lado_alm = lado_alm;
                            rowData.estante_alm = estante_alm;
                            rowData.nivel_alm = nivel_alm;
                            rowData.PERU_insumo = insumo;
                            rowData.unidad = unidad;

                            tblPartidas.DataTable().row(row).data(rowData).draw();

                            recargarAutoComplete(row);

                            calcularTotal();
                        });
                    });

                    tblPartidas.on('change', '.selectAreaCuenta', function () {
                        let row = $(this).closest('tr');
                        $(row).find('.inputCantidad').change();
                    });
                },
                createdRow: function (row, rowData) {
                    let inputInsumo = $(row).find('.inputInsumo');
                    let inputInsumoDesc = $(row).find('.inputInsumoDesc');

                    inputInsumo.getAutocomplete(setInsumoDesc, { cc: inputCCNum.val(), almacen: inputAlmacenNum.val() }, '/Enkontrol/Requisicion/getInsumosByAlmacenEntrada');
                    inputInsumoDesc.getAutocomplete(setInsumoBusqPorDesc, { cc: inputCCNum.val(), almacen: inputAlmacenNum.val() }, '/Enkontrol/Requisicion/getInsumosDescByAlmacenEntrada');
                    if (inputEmpresaActual.val() != 6 && inputEmpresaActual.val() != 3) {
                        let selectAreaCuenta = $(row).find('.selectAreaCuenta');

                        selectAreaCuenta.fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: inputCCNum.val() }, false, "000-000");
                        selectAreaCuenta.find('option[value=' + rowData.area + '][data-prefijo=' + rowData.cuenta + ']').attr('selected', true);
                    } else {
                        let selectAreaCuenta = $(row).find('.selectAreaCuenta');

                        selectAreaCuenta.fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: inputCCNum.val() }, false, "000-000");

                        if (rowData.noEconomico != '') {
                            selectAreaCuenta.find(`option[value=${rowData.noEconomico}]`).attr('selected', true)
                        } else {
                            selectAreaCuenta.find('option[value=000-000]').attr('selected', true);
                        }
                    }
                },
                columns: [
                    // { data: 'partida', title: 'Pda' },
                    // {

                    //     data: 'insumo', title: 'Insumo', render: function (data, type, row, meta) {
                    //         let input = document.createElement('input');

                    //         input.classList.add('form-control', 'text-center', 'inputInsumo');

                    //         $(input).attr('value', data);

                    //         return input.outerHTML;
                    //     }
                    // },
                    { data: 'partida', title: 'Pda' },
                    {
                        data: 'insumo', title: 'Insumo', render: function (data, type, row, meta) {

                            if (inputEmpresaActual.val() == 6) {
                                let input = document.createElement('input');

                                input.classList.add('form-control', 'text-center', 'inputInsumo');

                                $(input).attr('value', row.PERU_insumo);

                                return input.outerHTML;
                            } else {
                                let input = document.createElement('input');

                                input.classList.add('form-control', 'text-center', 'inputInsumo');

                                $(input).attr('value', data);

                                return input.outerHTML;
                            }

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
                        data: 'areaCuenta', title: 'Área-Cuenta', render: function (data, type, row, meta) {
                            let select = document.createElement('select');

                            select.classList.add('form-control', 'text-center', 'selectAreaCuenta');

                            return select.outerHTML;
                        }
                    },
                    {
                        data: 'cantidad', title: 'Cantidad', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputCantidad');

                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        data: 'costo_prom', title: 'Costo Prom.', render: function (data, type, row, meta) {
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
                        title: 'Historial', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-default btnHistorialInsumo"><i class="fa fa-th-list"></i></button>`;
                        }
                    },
                    {
                        title: 'Catálogo', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-default btnCatalogoUbicaciones"><i class="fa fa-bars"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableHistorialInsumo() {
            tblHistorialInsumo.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblHistorialInsumo.on('click', '.btnSeleccionarHistorialInsumo', function () {
                        let rowData = tblHistorialInsumo.DataTable().row($(this).closest('tr')).data();

                        if (_filaInsumo != null) {
                            $(_filaInsumo).find('.inputArea').val(rowData.area_alm);
                            $(_filaInsumo).find('.inputLado').val(rowData.lado_alm);
                            $(_filaInsumo).find('.inputEstante').val(rowData.estante_alm);
                            $(_filaInsumo).find('.inputNivel').val(rowData.nivel_alm);
                            $(_filaInsumo).find('.inputCantidad').change();

                            mdlHistorialInsumo.modal('hide');

                            tblPartidas.find('.inputArea').change();
                        }
                    });
                },
                columns: [
                    {
                        data: 'insumo', title: 'Insumo', render: function (data, type, row, meta) {
                            if (inputEmpresaActual.val() == 6) {
                                return row.PERU_insumo + ' - ' + row.insumoDesc;

                            } else {

                                return row.insumo + ' - ' + row.insumoDesc;
                            }
                        }
                    },
                    { data: 'area_alm', title: 'Área' },
                    { data: 'lado_alm', title: 'Lado' },
                    { data: 'estante_alm', title: 'Estante' },
                    { data: 'nivel_alm', title: 'Nivel' },
                    {
                        title: 'Seleccionar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-primary btnSeleccionarHistorialInsumo"><i class="fa fa-arrow-right"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableCatalogoUbicaciones() {
            tblCatalogoUbicaciones.DataTable({
                retrieve: true,
                deferRender: true,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblCatalogoUbicaciones.on('click', '.btnSeleccionarUbicacion', function () {
                        let rowData = tblCatalogoUbicaciones.DataTable().row($(this).closest('tr')).data();

                        if (_filaInsumo != null) {
                            $(_filaInsumo).find('.inputArea').val(rowData.area_alm);
                            $(_filaInsumo).find('.inputLado').val(rowData.lado_alm);
                            $(_filaInsumo).find('.inputEstante').val(rowData.estante_alm);
                            $(_filaInsumo).find('.inputNivel').val(rowData.nivel_alm);
                            $(_filaInsumo).find('.inputCantidad').change();

                            mdlCatalogoUbicaciones.modal('hide');

                            tblCatalogoUbicaciones.find('.inputArea').change();
                        }
                    });
                },
                columns: [
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
                $.post('/Enkontrol/Almacen/GetDevolucionEntrada', { almacenID, numero })
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
            inputRecibioNum.val('');
            inputRecibioDesc.val('');
            inputComentarios.val('');
            inputNumero.val('');
            inputSectorNum.val('');
            inputSectorDesc.val('');
            inputFecha.val('');
            inputTotal.val('');

            tblPartidas.DataTable().clear().draw();
        }

        function limpiarInformacion() {
            // inputAlmacenNum.val('');
            // inputAlmacenDesc.val('');
            inputCCNum.val('');
            inputCCDesc.val('');
            inputRecibioNum.val('');
            inputRecibioDesc.val('');
            inputComentarios.val('');
            // inputNumero.val('');
            inputSectorNum.val('');
            inputSectorDesc.val('');
            inputFecha.val('');
            inputTotal.val('');
        }

        function llenarInformacion(data) {
            inputAlmacenNum.val(data.almacen);
            inputAlmacenDesc.val(data.almacenDesc);
            inputCCNum.val(data.cc);
            inputCCDesc.val(data.ccDesc);
            inputRecibioNum.val(data.empleado);
            inputRecibioDesc.val(data.empleadoDesc);
            inputComentarios.val(data.comentarios);
            inputNumero.val(data.numero);
            inputSectorNum.val(data.sector_id);
            inputSectorDesc.val('');
            inputFecha.val(data.fechaString);
            inputTotal.val('$' + formatMoney(data.total));

            AddRows(tblPartidas, data.detalle);
        }

        function setInsumoDesc(e, ui) {
            let row = $(this).closest('tr');

            row.find('.inputInsumo').val(ui.item.value);
            row.find('.inputInsumoDesc').val(ui.item.id);
            row.find(`td:eq(${inputEmpresaActual.val() != 6 ? '5' : '4'})`).text(ui.item.unidad);

            let rowData = tblPartidas.DataTable().row(row).data();

            let insumo = $(row).find('.inputInsumo').val();
            let insumoDesc = $(row).find('.inputInsumoDesc').val();
            let nuevaCantidad = !isNaN($(row).find('.inputCantidad').val()) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;
            let unidad = $(row).find(`td:eq(${inputEmpresaActual.val() != 6 ? '5' : '4'})`).text();
            let nuevoImporte = nuevaCantidad * ui.item.costoPromedio;

            rowData.insumo = insumo;
            rowData.insumoDesc = insumoDesc;
            rowData.cantidad = nuevaCantidad;
            rowData.unidad = unidad;
            rowData.costo_prom = ui.item.costoPromedioEntrada;
            rowData.importe = nuevoImporte;
            rowData.PERU_insumo = insumo;

            tblPartidas.DataTable().row(row).data(rowData).draw();

            recargarAutoComplete(row);
        }

        function setInsumoBusqPorDesc(e, ui) {
            let row = $(this).closest('tr');

            row.find('.inputInsumo').val(ui.item.value.split(' - ')[0]);
            row.find('.inputInsumoDesc').val(ui.item.id);
            row.find(`td:eq(${inputEmpresaActual.val() != 6 ? '5' : '4'})`).text(ui.item.unidad);

            let rowData = tblPartidas.DataTable().row(row).data();

            let insumo = $(row).find('.inputInsumo').val();
            let insumoDesc = $(row).find('.inputInsumoDesc').val();
            let nuevaCantidad = !isNaN($(row).find('.inputCantidad').val()) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;
            let unidad = $(row).find(`td:eq(${inputEmpresaActual.val() != 6 ? '5' : '4'})`).text();
            let nuevoImporte = nuevaCantidad * ui.item.costoPromedio;

            rowData.insumo = insumo;
            rowData.insumoDesc = insumoDesc;
            rowData.cantidad = nuevaCantidad;
            rowData.unidad = unidad;
            rowData.costo_prom = ui.item.costoPromedioEntrada;
            rowData.importe = nuevoImporte;
            rowData.PERU_insumo = insumo;

            tblPartidas.DataTable().row(row).data(rowData).draw();

            recargarAutoComplete(row);

            //Retornar falso para que no se ejecute la función de select por default de JQuery UI.
            return false;
        }
        function recargarAutoComplete(row) {
            let rowData = tblPartidas.DataTable().row(row).data();
            let inputInsumo = $(row).find('.inputInsumo');
            let inputInsumoDesc = $(row).find('.inputInsumoDesc');

            inputInsumo.getAutocomplete(setInsumoDesc, { cc: inputCCNum.val(), almacen: inputAlmacenNum.val() }, '/Enkontrol/Requisicion/getInsumosByAlmacenEntrada');
            inputInsumoDesc.getAutocomplete(setInsumoBusqPorDesc, { cc: inputCCNum.val(), almacen: inputAlmacenNum.val() }, '/Enkontrol/Requisicion/getInsumosDescByAlmacenEntrada');
            if (inputEmpresaActual.val() != 6 && inputEmpresaActual.val() != 3) {
                let selectAreaCuenta = $(row).find('.selectAreaCuenta');
                selectAreaCuenta.fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: inputCCNum.val() }, false, "000-000");
                selectAreaCuenta.find('option[value=' + rowData.area + '][data-prefijo=' + rowData.cuenta + ']').attr('selected', true);
            } else {
                let selectAreaCuenta = $(row).find('.selectAreaCuenta');

                selectAreaCuenta.fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: inputCCNum.val() }, false, "000-000");

                if (rowData.noEconomico != '') {
                    selectAreaCuenta.find(`option[value=${rowData.noEconomico}]`).attr('selected', true)
                } else {
                    selectAreaCuenta.find('option[value=000-000]').attr('selected', true);
                }
            }
        }
        function validarInsumo(e, ul) {
            if (ul.item == null) {
                let row = $(this).closest('tr');

                row.find('.inputInsumo').val('');
                row.find('.inputInsumoDesc').val('');
            }
        }

        function guardarDevolucionEntrada() {
            let movimiento = getInformacion();

            if (movimiento.detalle.length > 0) {
                btnGuardar.attr('disabled', true);

                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GuardarDevolucionEntrada', { movimiento })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            // btnGuardar.attr('disabled', false);
                            AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                            limpiarVista();
                            verReporte();
                        } else {
                            AlertaGeneral(`Alerta`, `Error al guardar la información. ${response.message.length > 0 ? response.message : ``}`);
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
                tipo_mov: 3,
                numero: inputNumero.val(), //Posible número disponible
                cc: inputCCNum.val(),
                ccDesc: inputCCDesc.val(),
                fecha: null, //Valor asignado en el back
                fechaString: '', //Valor asignado en el back
                total: unmaskNumero6DCompras(inputTotal.val()),
                estatus: 'A',
                transferida: 'N',
                empleado: 0, //Valor asignado en el back
                empleadoDesc: '', //Valor asignado en el back
                comentarios: inputComentarios.val(),
                sector_id: inputSectorNum.val() != '' ? inputSectorNum.val() : null
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
                    let area = "";
                    let cuenta = "";
                    if (inputEmpresaActual.val() != 6 && inputEmpresaActual.val() != 3) {
                        area = $(row).find('.selectAreaCuenta option:selected').val();
                        cuenta = $(row).find('.selectAreaCuenta option:selected').attr('data-prefijo');
                    }
                    let cantidad = unmaskNumero($(row).find('.inputCantidad').val());

                    let precio = rowData.costo_prom; //let precio = 0;
                    let importe = cantidad * rowData.costo_prom;
                    let costo_prom = rowData.costo_prom;

                    let area_alm = $(row).find('.inputArea').val();
                    let lado_alm = $(row).find('.inputLado').val();
                    let estante_alm = $(row).find('.inputEstante').val();
                    let nivel_alm = $(row).find('.inputNivel').val();
                    let unidad = $(row).find(`td:eq(${inputEmpresaActual.val() != 6 ? '5' : '4'})`).text();
                    if (inputEmpresaActual.val() != 6 && inputEmpresaActual.val() != 3) {
                        if (insumo != '' && !isNaN(insumo)) {
                            detalle.push({
                                almacen: inputAlmacenNum.val(),
                                tipo_mov: 3,
                                numero: inputNumero.val(), //Posible número disponible
                                partida: partida,
                                insumo: insumo,
                                comentarios: '',
                                area: area,
                                cuenta: cuenta,
                                cantidad: cantidad,
                                precio: precio,
                                importe: importe,
                                costo_prom: costo_prom,
                                sector_id: inputSectorNum.val() != '' ? inputSectorNum.val() : null,
                                area_alm: area_alm,
                                lado_alm: lado_alm,
                                estante_alm: estante_alm,
                                nivel_alm: nivel_alm,
                                PERU_insumo: insumo,
                            });
                        }
                    } else {
                        if (insumo != '' && !isNaN(insumo)) {
                            detalle.push({
                                almacen: inputAlmacenNum.val(),
                                tipo_mov: 3,
                                numero: inputNumero.val(), //Posible número disponible
                                partida: partida,
                                insumo: insumo,
                                comentarios: '',
                                area: 0,
                                cuenta: 0,
                                noEconomico: $(row).find('.selectAreaCuenta option:selected').val(),
                                cantidad: cantidad,
                                precio: precio,
                                importe: importe,
                                costo_prom: costo_prom,
                                sector_id: inputSectorNum.val() != '' ? inputSectorNum.val() : null,
                                area_alm: area_alm,
                                lado_alm: lado_alm,
                                estante_alm: estante_alm,
                                nivel_alm: nivel_alm,
                                PERU_insumo: insumo,
                                unidad: unidad,
                            });
                        }
                    }
                }
            });

            return detalle;
        }

        function cargarHistorialInsumo(row) {
            if (inputAlmacenNum.val() > 0) {
                let rowData = tblPartidas.DataTable().row(row).data();
                let almacen = inputAlmacenNum.val();
                let insumo = $(row).find('.inputInsumo').val();

                $.post('/Enkontrol/Almacen/GetHistorialInsumo', { almacen, insumo }).then(response => {
                    if (response.success) {
                        AddRows(tblHistorialInsumo, response.data);
                        mdlHistorialInsumo.modal('show');
                    } else {

                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            }
        }

        function verReporte() {
            report.attr("src", `/Reportes/Vista.aspx?idReporte=120`);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function cargarCatalogoUbicaciones() {
            if (_filaInsumo != null) {
                let almacenID = inputAlmacenNum.val();

                if (almacenID > 0) {
                    $.post('/Enkontrol/Almacen/GetCatalogoUbicaciones', { almacenID }).then(response => {
                        if (response.success) {
                            AddRows(tblCatalogoUbicaciones, response.data);
                            mdlCatalogoUbicaciones.modal('show');
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
                } else {
                    AlertaGeneral(`Alerta`, `Número de almacén inválido.`);
                }
            }
        }

        function calcularTotal() {
            let datos = tblPartidas.DataTable().rows().data();
            let total = 0;

            datos.toArray().forEach(function (element) {
                total += element.importe;
            });

            inputTotal.val('$' + formatMoney(total));
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
    $(document).ready(() => Enkontrol.Almacen.Almacen.DevolucionEntrada = new DevolucionEntrada())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();