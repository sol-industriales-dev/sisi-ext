(() => {
    $.namespace('Enkontrol.Almacen.Almacen.EntradaTraspasoSinOrigen');
    EntradaTraspasoSinOrigen = function () {
        //#region Selectores
        const inputAlmacenDestinoNum = $('#inputAlmacenDestinoNum');
        const inputAlmacenDestinoDesc = $('#inputAlmacenDestinoDesc');
        const inputCCDestinoNum = $('#inputCCDestinoNum');
        const inputCCDestinoDesc = $('#inputCCDestinoDesc');

        const inputAlmacenOrigenNum = $('#inputAlmacenOrigenNum');
        const inputAlmacenOrigenDesc = $('#inputAlmacenOrigenDesc');
        const inputCCOrigenNum = $('#inputCCOrigenNum');
        const inputCCOrigenDesc = $('#inputCCOrigenDesc');

        const inputRecibioNum = $('#inputRecibioNum');
        const inputRecibioDesc = $('#inputRecibioDesc');
        const inputComentarios = $('#inputComentarios');
        const inputNumero = $('#inputNumero');
        const inputFecha = $('#inputFecha');
        const inputTotal = $('#inputTotal');

        const inputOrdenTraspaso = $('#inputOrdenTraspaso');
        const inputFolioTraspaso = $('#inputFolioTraspaso');

        const tblPartidas = $('#tblPartidas');
        const btnAgregarInsumo = $('#btnAgregarInsumo');
        const btnQuitarInsumo = $('#btnQuitarInsumo');
        const btnGuardar = $('#btnGuardar');
        const mdlHistorialInsumo = $('#mdlHistorialInsumo');
        const tblHistorialInsumo = $('#tblHistorialInsumo');
        const mdlCatalogoUbicaciones = $('#mdlCatalogoUbicaciones');
        const tblCatalogoUbicaciones = $('#tblCatalogoUbicaciones');
        const report = $("#report");
        //#endregion

        _filaInsumo = null;

        (function init() {
            initTablePartidas();
            initTableHistorialInsumo();
            initTableCatalogoUbicaciones();

            inputNumero.change(cargarInformacion);
            btnGuardar.click(guardarEntradaTraspaso);

            inputFecha.datepicker().datepicker();
        })();

        inputAlmacenDestinoNum.on('change', function () {
            let almacenID = inputAlmacenDestinoNum.val();

            if (almacenID != '' && !isNaN(almacenID)) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GetNuevaEntradaTraspasoSinOrigen', { almacenID })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            inputAlmacenDestinoDesc.val(response.almacenDesc);
                            inputNumero.val(response.numeroDisponible);
                            inputRecibioNum.val(response.entregoNum);
                            inputRecibioDesc.val(response.entregoDesc);
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

        inputCCDestinoNum.on('change', function () {
            let cc = inputCCDestinoNum.val();

            if (cc != '') {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GetCentroCosto', { cc })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            inputCCDestinoDesc.val(response.ccDesc);
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                inputCCDestinoNum.val('');
                inputCCDestinoDesc.val('');
            }
        });

        btnAgregarInsumo.on('click', function () {
            let datos = tblPartidas.DataTable().rows().data();

            tblPartidas.DataTable().row.add({
                'partida': datos.length + 1,
                'insumo': '',
                'insumoDesc': '',
                'areaCuenta': '',
                'cantidad': 0,
                'unidad': '',
                'precio': 0,
                'importe': 0,
                'area_alm': '',
                'lado_alm': '',
                'estante_alm': '',
                'nivel_alm': ''
            }).draw();
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

                    tblPartidas.on('change', '.inputCantidad, .inputPrecio', function () {
                        tblPartidas.find('tbody tr').each(function (index, row) {
                            let rowData = tblPartidas.DataTable().row(row).data();

                            let insumo = $(row).find('.inputInsumo').val();
                            let insumoDesc = $(row).find('.inputInsumoDesc').val();
                            let nuevaCantidad = unmaskNumero($(row).find('.inputCantidad').val());
                            let unidad = $(row).find('td:eq(4)').text();
                            let precio = unmaskNumero($(row).find('.inputPrecio').val());
                            let nuevoImporte = nuevaCantidad * precio;
                            let area = $(row).find('.selectAreaCuenta option:selected').val();
                            let cuenta = $(row).find('.selectAreaCuenta option:selected').attr('data-prefijo');
                            let area_alm = $(row).find('.inputArea').val();
                            let lado_alm = $(row).find('.inputLado').val();
                            let estante_alm = $(row).find('.inputEstante').val();
                            let nivel_alm = $(row).find('.inputNivel').val();

                            rowData.insumo = insumo;
                            rowData.insumoDesc = insumoDesc;
                            rowData.area = area;
                            rowData.cuenta = cuenta;
                            rowData.cantidad = nuevaCantidad;
                            rowData.unidad = unidad;
                            rowData.precio = precio;
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
                },
                createdRow: function (row, rowData) {
                    let inputInsumo = $(row).find('.inputInsumo');
                    let inputInsumoDesc = $(row).find('.inputInsumoDesc');

                    inputInsumo.getAutocompleteValid(setInsumoDesc, validarInsumo, { cc: inputCCDestinoNum.val() }, '/Enkontrol/Requisicion/getInsumos');
                    inputInsumoDesc.getAutocompleteValid(setInsumoBusqPorDesc, validarInsumo, { cc: inputCCDestinoNum.val() }, '/Enkontrol/Requisicion/getInsumosDesc');

                    let selectAreaCuenta = $(row).find('.selectAreaCuenta');

                    selectAreaCuenta.fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: inputCCDestinoNum.val() }, false, "000-000");
                    selectAreaCuenta.find('option[value=' + rowData.area + '][data-prefijo=' + rowData.cuenta + ']').attr('selected', true);
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

                            $(input).attr('value', formatMoney(data));

                            return input.outerHTML;
                        }
                    },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        data: 'precio', title: 'Precio Unitario', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputPrecio');

                            $(input).attr('value', '$' + formatMoney(data));

                            return input.outerHTML;
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

                            mdlHistorialInsumo.modal('hide');

                            tblPartidas.find('.inputArea').change();
                        }
                    });
                },
                columns: [
                    {
                        data: 'insumo', title: 'Insumo', render: function (data, type, row, meta) {
                            return row.insumo + ' - ' + row.insumoDesc;
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

                            mdlCatalogoUbicaciones.modal('hide');

                            tblUbicacion.find('.inputArea').change();
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

        function cargarInformacion() {
            let almacenDestinoID = inputAlmacenDestinoNum.val();
            let numero = inputNumero.val();

            limpiarInformacion();
            tblPartidas.DataTable().clear().draw();

            if ((almacenDestinoID != '' && !isNaN(almacenDestinoID)) && (numero != '' && !isNaN(numero))) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GetEntradaTraspasoSinOrigen', { almacenDestinoID, numero })
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
            inputAlmacenDestinoNum.val('');
            inputAlmacenDestinoDesc.val('');
            inputCCDestinoNum.val('');
            inputCCDestinoDesc.val('');
            inputRecibioNum.val('');
            inputRecibioDesc.val('');
            inputComentarios.val('');
            inputNumero.val('');
            inputFecha.val('');
            inputTotal.val('');

            tblPartidas.DataTable().clear().draw();
        }

        function limpiarInformacion() {
            inputCCDestinoNum.val('');
            inputCCDestinoDesc.val('');
            inputRecibioNum.val('');
            inputRecibioDesc.val('');
            inputComentarios.val('');
            inputFecha.val('');
            inputTotal.val('');
        }

        function llenarInformacion(data) {
            inputAlmacenDestinoNum.val(data.almacen);
            inputAlmacenDestinoDesc.val(data.almacenDesc);
            inputCCDestinoNum.val(data.cc);
            inputCCDestinoDesc.val(data.ccDesc);
            inputRecibioNum.val(data.empleado);
            inputRecibioDesc.val(data.empleadoDesc);
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
            let precio = unmaskNumero6DCompras($(row).find('.inputPrecio').val());
            let nuevoImporte = nuevaCantidad * precio;

            rowData.insumo = insumo;
            rowData.insumoDesc = insumoDesc;
            rowData.cantidad = nuevaCantidad;
            rowData.unidad = unidad;
            rowData.precio = precio;
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
            let precio = unmaskNumero6DCompras($(row).find('.inputPrecio').val());
            let nuevoImporte = nuevaCantidad * precio;

            rowData.insumo = insumo;
            rowData.insumoDesc = insumoDesc;
            rowData.cantidad = nuevaCantidad;
            rowData.unidad = unidad;
            rowData.precio = precio;
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

        function guardarEntradaTraspaso() {
            let movimiento = getInformacion();

            if (movimiento.detalle.length > 0) {
                btnGuardar.attr('disabled', true);

                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GuardarEntradaTraspasoSinOrigen', { movimiento })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            btnGuardar.attr('disabled', false);
                            AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                            limpiarVista();
                            verReporte();
                        } else {
                            AlertaGeneral(`Alerta`, `Error al guardar la información. ${response.message.length > 0 ? response.message : ``}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        }

        function getInformacion() {
            let movimiento = {
                almacen: inputAlmacenDestinoNum.val(),
                almacenDesc: inputAlmacenDestinoDesc.val(),
                tipo_mov: 5,
                numero: inputNumero.val(), //Posible número disponible
                cc: inputCCDestinoNum.val(),
                ccDesc: inputCCDestinoDesc.val(),
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
                    let area = $(row).find('.selectAreaCuenta option:selected').val();
                    let cuenta = $(row).find('.selectAreaCuenta option:selected').attr('data-prefijo');
                    let cantidad = unmaskNumero($(row).find('.inputCantidad').val());

                    let precio = rowData.precio;
                    let importe = rowData.importe;

                    let area_alm = $(row).find('.inputArea').val();
                    let lado_alm = $(row).find('.inputLado').val();
                    let estante_alm = $(row).find('.inputEstante').val();
                    let nivel_alm = $(row).find('.inputNivel').val();

                    let partida_oc = rowData.partida_oc != null ? rowData.partida_oc : 0;

                    if (insumo != '' && !isNaN(insumo)) {
                        detalle.push({
                            almacen: inputAlmacenDestinoNum.val(),
                            tipo_mov: 5,
                            numero: inputNumero.val(), //Posible número disponible
                            partida: partida,
                            insumo: insumo,
                            comentarios: '',
                            area: area,
                            cuenta: cuenta,
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

            inputInsumo.getAutocompleteValid(setInsumoDesc, validarInsumo, { cc: inputCCDestinoNum.val() }, '/Enkontrol/Requisicion/getInsumos');
            inputInsumoDesc.getAutocompleteValid(setInsumoBusqPorDesc, validarInsumo, { cc: inputCCDestinoNum.val() }, '/Enkontrol/Requisicion/getInsumosDesc');

            let selectAreaCuenta = $(row).find('.selectAreaCuenta');

            selectAreaCuenta.fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: inputCCDestinoNum.val() }, false, "000-000");
            selectAreaCuenta.find('option[value=' + rowData.area + '][data-prefijo=' + rowData.cuenta + ']').attr('selected', true);
        }

        function cargarHistorialInsumo(row) {
            if (inputAlmacenDestinoNum.val() > 0) {
                let rowData = tblPartidas.DataTable().row(row).data();
                let almacen = inputAlmacenDestinoNum.val();
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
            report.attr("src", `/Reportes/Vista.aspx?idReporte=121`);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function cargarCatalogoUbicaciones() {
            if (_filaInsumo != null) {
                let almacenID = inputAlmacenDestinoNum.val();

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
    $(document).ready(() => Enkontrol.Almacen.Almacen.EntradaTraspasoSinOrigen = new EntradaTraspasoSinOrigen())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();