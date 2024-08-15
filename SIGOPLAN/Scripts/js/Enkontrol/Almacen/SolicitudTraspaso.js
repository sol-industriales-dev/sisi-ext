(() => {
    $.namespace('Enkontrol.Almacen.Almacen.SolicitudTraspaso');
    SolicitudTraspaso = function () {
        //#region Selectores
        const selectCCOrigen = $('#selectCCOrigen');
        const selectAlmacenOrigen = $('#selectAlmacenOrigen');
        const selectCCDestino = $('#selectCCDestino');
        const selectAlmacenDestino = $('#selectAlmacenDestino');
        const tblStock = $('#tblStock');
        const btnGuardar = $('#btnGuardar');
        const textAreaComentarios = $('#textAreaComentarios');
        const btnAgregarInsumo = $('#btnAgregarInsumo');
        const btnQuitarInsumo = $('#btnQuitarInsumo');
        const btnCargarMovimiento = $('#btnCargarMovimiento');
        const mdlCargarMovimiento = $('#mdlCargarMovimiento');
        const selectAlmacenMovimiento = $('#selectAlmacenMovimiento');
        const selectTipoMovimiento = $('#selectTipoMovimiento');
        const inputNumeroMovimiento = $('#inputNumeroMovimiento');
        const btnAceptarMovimiento = $('#btnAceptarMovimiento');
        //#endregion

        (function init() {
            initTblStock();

            selectCCOrigen.fillCombo('/Enkontrol/Almacen/FillComboCC');
            selectAlmacenOrigen.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos');
            selectCCDestino.fillCombo('/Enkontrol/Almacen/FillComboCC');
            selectAlmacenDestino.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos');
            selectAlmacenMovimiento.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos');
            selectTipoMovimiento.fillCombo('/Enkontrol/Almacen/FillComboTipoMovimiento');

            agregarTooltip(btnCargarMovimiento, 'Cargar Movimiento');
        })();

        $('#selectCCOrigen, #selectAlmacenOrigen').on('change', function () {
            if (selectCCOrigen.val() != '' && selectAlmacenOrigen.val() != '') {
                let rowData = tblStock.DataTable().row(tblStock.find('tbody tr:eq(0)')).data();

                if (rowData == undefined) {
                    btnAgregarInsumo.click();
                }
            } else {
                limpiarTabla(tblStock);
            }
        });

        btnGuardar.on('click', function () {
            let ccOrigen = selectCCOrigen.val();
            let almacenOrigen = selectAlmacenOrigen.val();
            let ccDestino = selectCCDestino.val();
            let almacenDestino = selectAlmacenDestino.val();
            let comentarios = textAreaComentarios.val();
            let insumos = getInsumosTraspasos();

            if (ccOrigen != '' && almacenOrigen != '' && ccDestino != '' && almacenDestino != '') {
                if (insumos.length > 0) {
                    $.blockUI({ message: 'Procesando...' });
                    $.post('/Enkontrol/Almacen/GuardarTraspasos', { ccOrigen, almacenOrigen, ccDestino, almacenDestino, comentarios, insumos })
                        .always($.unblockUI)
                        .then(response => {
                            if (response.success) {
                                AlertaGeneral(`Alerta`, `Se ha guardado la información`);
                                selectCCOrigen.change();
                                tblStock.DataTable().clear().draw();
                                textAreaComentarios.val('');
                            } else {
                                AlertaGeneral(`Alerta`, `Error al guardar la información`);
                                selectCCOrigen.change();
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                } else {
                    AlertaGeneral(`Alerta`, `Debe ingresar cantidades válidas igual o menor al Stock.`);
                }
            } else {
                AlertaGeneral(`Alerta`, `Debe seleccionar los centros de costo y almacenes para el origen y destino.`);
            }
        });

        btnAgregarInsumo.on('click', function () {
            if (selectCCOrigen.val() != '' && selectAlmacenOrigen.val() != '') {
                let datos = tblStock.DataTable().rows().data();

                datos.push({
                    'insumo': '',
                    'descInsumo': '',
                    'cantidad': 0,
                    'solicitadoPendiente': 0,
                    'minimo': 0,
                    'cantidadTraspasar': 0
                });

                tblStock.DataTable().clear();
                tblStock.DataTable().rows.add(datos).draw();

                btnQuitarInsumo.prop("disabled", false);
            } else {
                AlertaGeneral(`Alerta`, `Seleccione el Centro de Costos y Almacén del Origen.`);
            }
        });

        btnQuitarInsumo.on('click', function () {
            tblStock.DataTable().row(tblStock.find("tr.active")).remove().draw();

            let cuerpo = tblStock.find('tbody');

            if (cuerpo.find("tr").length == 0) {
                tblStock.DataTable().draw();
            } else {
                tblStock.find('tbody tr').each(function (idx, row) {
                    let rowData = tblStock.DataTable().row(row).data();

                    if (rowData != undefined) {
                        rowData.partida = ++idx;

                        tblStock.DataTable().row(row).data(rowData).draw();
                    }
                });
            }
        });

        btnCargarMovimiento.on('click', function () {
            mdlCargarMovimiento.modal('show');
        });

        btnAceptarMovimiento.on('click', function () {
            let almacen = selectAlmacenMovimiento.val();
            let tipo_mov = selectTipoMovimiento.val();
            let numero = inputNumeroMovimiento.val();

            if (almacen != '' && tipo_mov != '' && numero > 0) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/CargarMovimiento', { almacen, tipo_mov, numero })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            AddRows(tblStock, response.data.detalle);

                            mdlCargarMovimiento.modal('hide');
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );

                selectAlmacenMovimiento.val('');
                selectTipoMovimiento.val('');
                inputNumeroMovimiento.val('');
            }
        });

        tblStock.on('change', '.inputInsumo', function () {
            let insumo = $(this).val();
            let almacen = +(selectAlmacenOrigen.val());

            if (insumo.length == 7) {
                let row = $(this).closest('tr');

                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Requisicion/GetInsumoInformacionByAlmacen', { insumo, almacen })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            let ins = response.data;

                            row.find('.inputInsumo').val(ins.value);
                            row.find('.inputInsumoDesc').val(ins.id);
                            reInicializarCamposRenglon(row);
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información del insumo.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        });

        function initTblStock() {
            tblStock.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                searching: false,
                initComplete: function (settings, json) {
                    tblStock.on('change', 'input', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblStock.DataTable().row(row).data();

                        let inputInsumo = row.find('.inputInsumo');
                        let inputInsumoDesc = row.find('.inputInsumoDesc');
                        let inputTraspasar = row.find('.inputTraspasar');

                        if (!isNaN(inputInsumo.val())) {
                            let cantidadTraspasar = unmaskNumero(inputTraspasar.val());

                            if (rowData.cantidad >= cantidadTraspasar) {
                                let almacenOrigen = selectAlmacenOrigen.val();
                                let insumo = inputInsumo.val();
                                let insumoDesc = inputInsumoDesc.val();

                                let filtros = {
                                    almacen: almacenOrigen,
                                    insumo: insumo
                                }

                                $.blockUI({ message: 'Procesando...' });
                                $.post('/Enkontrol/Almacen/GetInformacionInsumo', { filtros })
                                    .always($.unblockUI)
                                    .then(response => {
                                        if (response.success) {
                                            rowData.insumo = insumo;
                                            rowData.descInsumo = insumoDesc;
                                            rowData.cantidad = response.data.cantidad;
                                            rowData.solicitadoPendiente = response.data.solicitadoPendiente;
                                            rowData.minimo = response.data.minimoDesc;
                                            rowData.cantidadTraspasar = cantidadTraspasar;

                                            tblStock.DataTable().row(row).data(rowData).draw();

                                            reInicializarCamposRenglon(row);

                                            row.find('.inputTraspasar').focus();

                                            if (insumo != '' && cantidadTraspasar > 0 && $(row).is(":last-child")) {
                                                btnAgregarInsumo.click();
                                                $('.inputInsumo:last').focus();
                                            }
                                        } else {

                                        }
                                    }, error => {
                                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                                    }
                                    );
                            } else {
                                AlertaGeneral(`Alerta`, `No puede traspasar una cantidad mayor al Stock.`);
                                inputTraspasar.val('');
                            }
                        } else {
                            AlertaGeneral(`Alerta`, `Ingrese un insumo válido.`);
                            inputInsumo.val('');
                        }

                        reInicializarCamposRenglon(row);
                    });

                    tblStock.on('click', 'td', function () {
                        let rowData = tblStock.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblStock.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");
                        }
                    });
                },
                createdRow: function (row, rowData) {
                    reInicializarCamposRenglon(row);
                },
                columns: [
                    {
                        data: 'insumo', title: 'Insumo', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputInsumo');

                            $(input).attr('value', !isNaN(data) && data > 0 ? data : '');

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'descInsumo', title: 'Descripción', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-left', 'inputInsumoDesc');

                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'cantidad', title: 'Stock', render: function (data, type, row, meta) {
                            if (data != 0) {
                                return data;
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'solicitadoPendiente', title: 'Solicitado', render: function (data, type, row, meta) {
                            if (data != 0) {
                                return data;
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        title: 'Disponible', render: function (data, type, row, meta) {
                            if (row.insumo != '') {
                                return row.cantidad - row.solicitadoPendiente;
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'minimo', title: 'Mínimo', render: function (data, type, row, meta) {
                            if (row.insumo != '') {
                                let min = data;

                                if (min == '' || min == 'SOBRE PEDIDO') {
                                    min = 'SP';
                                }

                                return min;
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'cantidadTraspasar', title: 'Cantidad', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputTraspasar');

                            $(input).attr('value', !isNaN(data) && data > 0 ? data : '');

                            return input.outerHTML;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '15%', targets: [1] }
                ]
            });
        }

        function reInicializarCamposRenglon(row) {
            let inputInsumo = $(row).find('.inputInsumo');
            let inputInsumoDesc = $(row).find('.inputInsumoDesc');

            inputInsumo.getAutocompleteValid(setInsumoDesc, validarInsumo, { cc: selectCCOrigen.val() }, '/Enkontrol/Requisicion/getInsumos');
            inputInsumoDesc.getAutocompleteValid(setInsumoBusqPorDesc, validarInsumo, { cc: selectCCOrigen.val() }, '/Enkontrol/Requisicion/getInsumosDesc');
        }

        function setInsumoDesc(e, ui) {
            let row = $(this).closest('tr');

            row.find('.inputInsumoDesc').val(ui.item.id);
            row.find('.inputInsumo').change();
        }

        function setInsumoBusqPorDesc(e, ui) {
            let row = $(this).closest('tr');

            row.find('.inputInsumo').val(ui.item.id);
            row.find('.inputInsumoDesc').val(ui.item.value);
            row.find('.inputInsumoDesc').change();
        }

        function validarInsumo(e, ul) {
            if (ul.item == null) {
                let row = $(this).closest('tr');

                row.find('.inputInsumo').val('');
                row.find('.inputInsumoDesc').val('');
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function limpiarTabla(tbl) {
            dt = tbl.DataTable();
            dt.clear().draw();
        }

        function cargarInsumos(almacenID, cc) {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Almacen/GetInsumos', { almacenID, cc })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tblStock, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, `No se encontró información.`);
                        limpiarTabla(tblStock);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    limpiarTabla(tblStock);
                }
                );
        }

        function getInformacionInsumo(cc, almacen, insumo) {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Almacen/GetInformacionInsumo', { cc: cc, almacen: almacen, insumo: insumo })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {

                    } else {

                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getInsumosTraspasos() {
            let lstInsumos = [];

            tblStock.find('tbody tr').each(function (idx, row) {
                let rowData = tblStock.DataTable().row($(row)).data();

                if (rowData.cantidadTraspasar > 0 && rowData.cantidad >= rowData.cantidadTraspasar) {
                    let insumo = {
                        insumo: rowData.insumo,
                        descInsumo: rowData.descInsumo,
                        cantidad: rowData.cantidad,
                        minimo: rowData.minimo,
                        cantidadTraspasar: rowData.cantidadTraspasar
                        // area_alm: rowData.area_alm,
                        // lado_alm: rowData.lado_alm,
                        // estante_alm: rowData.estante_alm,
                        // nivel_alm: rowData.nivel_alm
                    };

                    lstInsumos.push(insumo);
                }
            });

            return lstInsumos;
        }

        function agregarTooltip(elemento, mensaje) {
            $(elemento).attr('data-toggle', 'tooltip');
            $(elemento).attr('data-placement', 'top');

            if (mensaje != "") {
                $(elemento).attr('title', mensaje);
            }

            $('[data-toggle="tooltip"]').tooltip({
                position: {
                    my: "center bottom-20",
                    at: "center top+8",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
                            .addClass("arrow")
                            .addClass(feedback.vertical)
                            .addClass(feedback.horizontal)
                            .appendTo(this);
                    }
                }
            });
        }
    }
    $(document).ready(() => Enkontrol.Almacen.Almacen.SolicitudTraspaso = new SolicitudTraspaso())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();