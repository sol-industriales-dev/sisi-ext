(function () {
    $.namespace('Enkontrol.Compras.Requisicion.Entradas');
    Entradas = function () {
        //#region Selectores
        const tblEntradas = $('#tblEntradas');
        const selectAlmacenOrigen = $('#selectAlmacenOrigen');
        const selectCentroCostoOrigen = $('#selectCentroCostoOrigen');
        const selectAlmacenDestino = $('#selectAlmacenDestino');
        const selectCentroCostoDestino = $('#selectCentroCostoDestino');
        const inputFolioTraspaso = $('#inputFolioTraspaso');
        const btnBuscar = $('#btnBuscar');
        const btnGuardarEntradas = $('#btnGuardarEntradas');
        const report = $("#report");
        const mdlUbicacionDetalle = $('#mdlUbicacionDetalle');
        const tblUbicacion = $('#tblUbicacion');
        const btnGuardarUbicacion = $('#btnGuardarUbicacion');
        const btnAgregarUbicacion = $('#btnAgregarUbicacion');
        const btnQuitarUbicacion = $('#btnQuitarUbicacion');
        const mdlHistorialInsumo = $('#mdlHistorialInsumo');
        const tblHistorialInsumo = $('#tblHistorialInsumo');
        const mdlCatalogoUbicaciones = $('#mdlCatalogoUbicaciones');
        const tblCatalogoUbicaciones = $('#tblCatalogoUbicaciones');
        const btnModalReporte = $('#btnModalReporte');
        const mdlVerReporte = $('#mdlVerReporte');
        const selectAlmacenReporte = $('#selectAlmacenReporte');
        const inputNumeroReporte = $('#inputNumeroReporte');
        const btnImprimible = $('#btnImprimible');
        //#endregion

        _countRenglonesEntradas = 0;
        _filaInsumo = null;
        _filaUbicacion = null;

        function init() {
            initForm();
            initTableEntradas();
            initTableUbicacion();
            initTableHistorialInsumo();
            initTableCatalogoUbicaciones();

            btnBuscar.click(buscarEntradas);
            btnModalReporte.click(function () { mdlVerReporte.modal('show'); });
            btnImprimible.click(verImprimible);
        }

        btnGuardarEntradas.on('click', function () {
            let folio_traspaso = +(inputFolioTraspaso.val());

            let entradas = [];

            tblEntradas.find("tbody tr").each(function (idx, row) {
                let rowData = tblEntradas.DataTable().row(row).data();
                let almacenDestinoID = $(row).find('.selectAlmacenDestinoNuevo').val();

                if (rowData) {
                    rowData.ordenTraspaso = 0;
                    rowData.comentarios = $(row).find('td .inputComentarios').val();
                    rowData.numeroDestino = 0;
                    rowData.listUbicacionMovimiento = $(row).find('td .btnUbicacionDetalle').data('listUbicacionMovimiento');
                    rowData.almacenDestinoID = almacenDestinoID;

                    if (folio_traspaso > 0) {
                        rowData.cc = selectCentroCostoOrigen.val();
                        rowData.ccDestino = selectCentroCostoDestino.val();
                    }

                    let cantidadEntrada = $(row).find('td .inputCantidadAutorizar').val();

                    if ((!isNaN(cantidadEntrada) && cantidadEntrada != '' && cantidadEntrada > 0)) {
                        entradas.push(rowData);
                    }
                }
            });

            if (entradas.length > 0) {
                let flagMismoAlmacen = entradas.some(function (x) {
                    return x.almacenOrigenID == x.almacenDestinoID;
                });

                if (!flagMismoAlmacen) {
                    let almacenDestinoOriginal = +(selectAlmacenDestino.val())

                    btnGuardarEntradas.attr('disabled', true);

                    $.post('/Enkontrol/Requisicion/GuardarEntradas', { entradas, folio_traspaso, almacenDestinoOriginal }).then(response => {
                        if (response.success) {
                            limpiarVista();
                            AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                            verReporte();
                        } else {
                            AlertaGeneral(`Alerta`, `Error al guardar la información. ${response.message}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
                } else {
                    AlertaGeneral(`Alerta`, `No se puede dar entrada si el almacén destino es el mismo que el origen.`);
                }
            } else {
                AlertaGeneral(`Alerta`, `No se capturaron datos.`);
            }
        });

        btnGuardarUbicacion.on('click', function () {
            let listUbicacionMovimiento = [];
            let totalUbicacion = 0;

            tblUbicacion.find("tbody tr").each(function (idx, row) {
                let rowData = tblUbicacion.DataTable().row(row).data();

                if (rowData != undefined) {
                    totalUbicacion += rowData.cantidad;

                    if (rowData.cantidad > 0) {
                        listUbicacionMovimiento.push({
                            insumo: rowData.insumo,
                            insumoDesc: rowData.insumoDesc,
                            cantidad: rowData.cantidad,
                            cantidadMovimiento: rowData.cantidad,
                            area_alm: rowData.area_alm,
                            lado_alm: rowData.lado_alm,
                            estante_alm: rowData.estante_alm,
                            nivel_alm: rowData.nivel_alm
                        });
                    }
                }
            });

            let cantidadEntrada = tblEntradas.DataTable().row(_filaInsumo).data().cantidad;

            if (totalUbicacion == cantidadEntrada) {
                if (listUbicacionMovimiento.length > 0) {
                    _filaInsumo.find('td .btnUbicacionDetalle').data('listUbicacionMovimiento', listUbicacionMovimiento);
                    _filaInsumo.find('td .btnUbicacionDetalle').data('totalUbicacion', totalUbicacion);
                    _filaInsumo.find('td .inputCantidadAutorizar').val(totalUbicacion);

                    _filaInsumo.find('td input').change();

                    mdlUbicacionDetalle.modal('hide');
                } else {
                    _filaInsumo.find('td .btnUbicacionDetalle').data('listUbicacionMovimiento', null);
                    _filaInsumo.find('td .btnUbicacionDetalle').data('totalUbicacion', null);
                    _filaInsumo.find('td .inputCantidadAutorizar').val(0);

                    _filaInsumo.find('td input').change();

                    mdlUbicacionDetalle.modal('hide');
                }
            } else if (totalUbicacion > cantidadEntrada) {
                AlertaGeneral(`Alerta`, `No se puede capturar una cantidad mayor a la entrada.`);
            } else if (totalUbicacion < cantidadEntrada) {
                AlertaGeneral(`Alerta`, `No se puede capturar una cantidad menor a la entrada.`);
            }
        });

        btnAgregarUbicacion.on('click', function () {
            let datos = tblUbicacion.DataTable().rows().data();
            let datosEntrada = tblEntradas.DataTable().row(_filaInsumo).data();

            datos.push({
                'insumo': datosEntrada.insumo,
                'insumoDesc': datosEntrada.insumoDesc,
                'cantidad': 0,
                'area_alm': '',
                'lado_alm': '',
                'estante_alm': '',
                'nivel_alm': ''
            });

            tblUbicacion.DataTable().clear();
            tblUbicacion.DataTable().rows.add(datos).draw();
        });

        btnQuitarUbicacion.on('click', function () {
            tblUbicacion.DataTable().row(tblUbicacion.find("tr.active")).remove().draw();

            let cuerpo = tblUbicacion.find('tbody');

            if (cuerpo.find("tr").length == 0) {
                tblUbicacion.DataTable().draw();
            }
        });

        $('#selectAlmacenOrigen, #selectCentroCostoOrigen, #selectAlmacenDestino, #selectCentroCostoDestino, #inputFolioTraspaso').on('change', function () {
            tblEntradas.DataTable().clear().draw();
        });

        function verReporte() {
            // report.attr("src", `/Reportes/Vista.aspx?idReporte=109`);
            report.attr("src", `/Reportes/Vista.aspx?idReporte=129`);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function initForm() {
            selectAlmacenOrigen.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, null);
            selectCentroCostoOrigen.fillCombo('/Enkontrol/Almacen/FillComboCC', null, false, null);
            selectAlmacenDestino.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, null);
            selectCentroCostoDestino.fillCombo('/Enkontrol/Almacen/FillComboCC', null, false, null);

            selectAlmacenReporte.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, null);
        }

        function initTableEntradas() {
            tblEntradas.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                rowId: 'id',
                bInfo: false,
                initComplete: function (settings, json) {
                    tblEntradas.on('click', 'input[type="checkbox"]', function () {
                        if ($(this).prop('checked')) {
                            tblEntradas.DataTable().row($(this).closest('tr')).data().checkbox = true;

                            _countRenglonesEntradas++;
                        } else {
                            tblEntradas.DataTable().row($(this).closest('tr')).data().checkbox = false;

                            _countRenglonesEntradas--;
                        }
                    });

                    tblEntradas.on('click', '.btnUbicacionDetalle', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblEntradas.DataTable().row(row).data();

                        _filaInsumo = row;

                        if ($(this).data('listUbicacionMovimiento') == undefined && $(this).data('listUbicacionMovimiento') == null) {
                            tblUbicacion.DataTable().clear();

                            let datos = tblUbicacion.DataTable().rows().data();

                            datos.push({
                                'insumo': rowData.insumo,
                                'insumoDesc': rowData.insumoDesc,
                                'cantidad': 0,
                                'area_alm': '',
                                'lado_alm': '',
                                'estante_alm': '',
                                'nivel_alm': ''
                            });

                            tblUbicacion.DataTable().clear();
                            tblUbicacion.DataTable().rows.add(datos).draw();
                        } else {
                            let listUbicacionMovimiento = $(this).data('listUbicacionMovimiento');

                            listUbicacionMovimiento.push({
                                'insumo': rowData.insumo,
                                'insumoDesc': rowData.insumoDesc,
                                'cantidad': 0,
                                'area_alm': '',
                                'lado_alm': '',
                                'estante_alm': '',
                                'nivel_alm': ''
                            });

                            AddRows(tblUbicacion, listUbicacionMovimiento);
                        }

                        mdlUbicacionDetalle.modal('show');
                    });
                },
                createdRow: function (row, rowData) {
                    $(row).find('.selectAlmacenDestinoNuevo').fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, null);
                    $(row).find('.selectAlmacenDestinoNuevo').val(rowData.almacenDestinoID);
                },
                columns: [
                    { data: 'ccDesc', title: 'Centro de Costo' },
                    {
                        data: 'numero', title: 'Número', render: function (data, type, row, meta) {
                            if (data != 0) {
                                return data;
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'folio_traspaso', title: 'Folio Traspaso', render: function (data, type, row, meta) {
                            if (data != 0) {
                                return data;
                            } else {
                                return '';
                            }
                        }
                    },
                    { data: 'fecha', title: 'Fecha' },
                    {
                        render: function (data, type, row, meta) {
                            return row.insumo + ' - ' + row.insumoDesc;
                        }, title: 'Insumo'
                    },
                    { data: 'cantidad', title: 'Cantidad' },
                    {
                        title: 'Almacén Origen', render: function (data, type, row, meta) {
                            return row.almacenOrigenID + ' - ' + row.almacenOrigenDesc;
                        }
                    },
                    {
                        data: 'almacenDestinoDesc', title: 'Almacén Destino', render: function (data, type, row, meta) {
                            let select = document.createElement('select');

                            select.classList.add('form-control');
                            select.classList.add('selectAlmacenDestinoNuevo');
                            select.style.height = '22px';

                            return select.outerHTML;
                        }
                    },
                    {
                        sortable: false,
                        render: (data, type, row, meta) => {
                            let input = document.createElement('input');

                            input.classList.add('form-control');
                            input.classList.add('inputComentarios');
                            input.style.height = '22px';

                            return input.outerHTML;
                        },
                        title: 'Comentarios'
                    },
                    {
                        sortable: false,
                        render: (data, type, row, meta) => {
                            let valor = data != '' && data != undefined ? data : 0;

                            return `<div class="input-group">
                                        <span class="input-group-btn">
                                            <button class="btn btn-xs btn-default btnUbicacionDetalle" ${row.checkboxRechazado ? 'disabled' : ''}>
                                                <i class="fa fa-arrow-right"></i>
                                            </button>
                                        </span>
                                        <input type="text" class="form-control text-center inputCantidadAutorizar" disabled value="${valor}" style="height: 22px;">
                                    </div>`;
                        },
                        title: 'Entrada'
                    }
                ],
                columnDefs: [
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        targets: [3]
                    },
                    { className: "dt-center", "targets": "_all" },
                    { width: '30%', targets: [4] }
                ]
            });
        }

        function initTableUbicacion() {
            tblUbicacion.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblUbicacion.on('click', '.btnCantidadTotalSalida', function () {
                        let rowData = tblUbicacion.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');

                        $row.find('.inputCantidadSalida').val(rowData.cantidad);
                        $row.find('.inputCantidadSalida').change();
                    });

                    tblUbicacion.on('change', 'input', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblUbicacion.DataTable().row(row).data();

                        let inputCantidadEntradaUbicacion = row.find('.inputCantidadEntradaUbicacion');
                        let inputArea = row.find('.inputArea');
                        let inputLado = row.find('.inputLado');
                        let inputEstante = row.find('.inputEstante');
                        let inputNivel = row.find('.inputNivel');

                        if (!isNaN(inputCantidadEntradaUbicacion.val())) {
                            let area_alm = inputArea.val();
                            let lado_alm = inputLado.val();
                            let estante_alm = inputEstante.val();
                            let nivel_alm = inputNivel.val();
                            let cantidadEntradaUbicacion = unmaskNumero(inputCantidadEntradaUbicacion.val());

                            rowData.area_alm = area_alm;
                            rowData.lado_alm = lado_alm;
                            rowData.estante_alm = estante_alm;
                            rowData.nivel_alm = nivel_alm;
                            rowData.cantidad = cantidadEntradaUbicacion;

                            tblUbicacion.DataTable().row(row).data(rowData).draw();

                            if ($(this).hasClass('inputCantidadEntradaUbicacion')) {
                                row.find('.inputArea').focus();
                            } else if ($(this).hasClass('inputArea')) {
                                row.find('.inputLado').focus();
                            } else if ($(this).hasClass('inputLado')) {
                                row.find('.inputEstante').focus();
                            } else if ($(this).hasClass('inputEstante')) {
                                row.find('.inputNivel').focus();
                            } else if ($(this).hasClass('inputNivel')) {
                                if (cantidadEntradaUbicacion > 0 && $(row).is(":last-child")) {
                                    let datos = tblUbicacion.DataTable().rows().data();

                                    datos.push({
                                        'insumo': rowData.insumo,
                                        'insumoDesc': rowData.insumoDesc,
                                        'cantidad': 0,
                                        'area_alm': '',
                                        'lado_alm': '',
                                        'estante_alm': '',
                                        'nivel_alm': ''
                                    });

                                    tblUbicacion.DataTable().clear();
                                    tblUbicacion.DataTable().rows.add(datos).draw();
                                    $('.inputCantidadEntradaUbicacion:last').focus();
                                }
                            }
                        } else {
                            AlertaGeneral(`Alerta`, `Ingrese una cantidad válida.`);
                            inputCantidadEntradaUbicacion.val('');
                        }
                    });

                    tblUbicacion.on('click', 'td', function () {
                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblUbicacion.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");
                        }
                    });

                    tblUbicacion.on('keyup', '.inputArea, .inputLado, .inputEstante, .inputNivel', function () {
                        $(this).val($(this).val().toUpperCase());
                    });

                    tblUbicacion.on('click', '.btnHistorialInsumo', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblUbicacion.DataTable().row(row).data();

                        _filaUbicacion = row;

                        cargarHistorialInsumo();
                    });

                    tblUbicacion.on('click', '.btnCatalogoUbicaciones', function () {
                        let row = $(this).closest('tr');

                        _filaUbicacion = row;

                        cargarCatalogoUbicaciones();
                    });
                },
                columns: [
                    {
                        data: 'insumoDesc', title: 'Insumo', render: function (data, type, row, meta) {
                            return row.insumo + ' - ' + row.insumoDesc;
                        }
                    },
                    {
                        data: 'cantidad', title: 'Cantidad', render: function (data, type, row, meta) {
                            let valor = data != undefined && data != '' ? data : '';

                            return `<input type="text" class="form-control text-center inputCantidadEntradaUbicacion" value="${valor}" style="height: 22px;">`;
                        }
                    },
                    {
                        data: 'area_alm', title: 'Área', render: function (data, type, row, meta) {
                            let valor = data != undefined ? data : '';

                            return `<input type="text" class="form-control text-center inputArea" value="${valor}" style="height: 22px;">`;
                        }
                    },
                    {
                        data: 'lado_alm', title: 'Lado', render: function (data, type, row, meta) {
                            let valor = data != undefined ? data : '';

                            return `<input type="text" class="form-control text-center inputLado" value="${valor}" style="height: 22px;">`;
                        }
                    },
                    {
                        data: 'estante_alm', title: 'Estante', render: function (data, type, row, meta) {
                            let valor = data != undefined ? data : '';

                            return `<input type="text" class="form-control text-center inputEstante" value="${valor}" style="height: 22px;">`;
                        }
                    },
                    {
                        data: 'nivel_alm', title: 'Nivel', render: function (data, type, row, meta) {
                            let valor = data != undefined ? data : '';

                            return `<input type="text" class="form-control text-center inputNivel" value="${valor}" style="height: 22px;">`;
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

                        if (_filaUbicacion != null) {
                            $(_filaUbicacion).find('.inputArea').val(rowData.area_alm);
                            $(_filaUbicacion).find('.inputLado').val(rowData.lado_alm);
                            $(_filaUbicacion).find('.inputEstante').val(rowData.estante_alm);
                            $(_filaUbicacion).find('.inputNivel').val(rowData.nivel_alm);

                            mdlHistorialInsumo.modal('hide');

                            tblUbicacion.find('.inputArea').change();
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

                        if (_filaUbicacion != null) {
                            $(_filaUbicacion).find('.inputArea').val(rowData.area_alm);
                            $(_filaUbicacion).find('.inputLado').val(rowData.lado_alm);
                            $(_filaUbicacion).find('.inputEstante').val(rowData.estante_alm);
                            $(_filaUbicacion).find('.inputNivel').val(rowData.nivel_alm);

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

        function cargarHistorialInsumo() {
            if (_filaInsumo != null) {
                let rowData = tblEntradas.DataTable().row(_filaInsumo).data();
                let almacen = rowData.almacenDestinoID;
                let insumo = rowData.insumo;

                $.post('/Enkontrol/Almacen/GetHistorialInsumo', { almacen, insumo }).then(response => {
                    if (response.success) {
                        AddRows(tblHistorialInsumo, response.data);
                        mdlHistorialInsumo.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            }
        }

        function cargarCatalogoUbicaciones() {
            if (_filaInsumo != null) {
                let almacenID = $(_filaInsumo).find('.selectAlmacenDestinoNuevo').val(); // let almacenID = rowData.almacenDestinoID;

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
                    AlertaGeneral(`Alerta`, `No se ha seleccionado un almacen destino para este insumo.`);
                }
            }
        }

        function buscarEntradas() {
            let almacenOrigen = +(selectAlmacenOrigen.val());
            let centroCostoOrigen = selectCentroCostoOrigen.val();
            let almacenDestino = +(selectAlmacenDestino.val());
            let centroCostoDestino = selectCentroCostoDestino.val();
            let folioTraspaso = +(inputFolioTraspaso.val());

            if (almacenOrigen > 0 && centroCostoOrigen != '' && almacenDestino > 0 && centroCostoDestino != '' && folioTraspaso > 0) {
                $.post('/Enkontrol/Requisicion/GetSalidaTraspaso', { almacenOrigen, centroCostoOrigen, almacenDestino, centroCostoDestino, folioTraspaso }).then(response => {
                    if (response.success) {
                        AddRows(tblEntradas, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, `${response.message}`);
                        limpiarVista();
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    limpiarVista();
                }
                );
            } else {
                AlertaGeneral(`Alerta`, `Debe capturar los datos de origen, destino y folio de traspaso.`);
                // $.post('/Enkontrol/Requisicion/GetEntradas', { almacenOrigen, centroCostoOrigen, almacenDestino, centroCostoDestino }).then(response => {
                //     if (response.success) {
                //         AddRows(tblEntradas, response.data);
                //     } else {
                //         AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                //     }
                // }, error => {
                //     AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                // }
                // );
            }
        }

        function verImprimible() {
            let almacen = selectAlmacenReporte.val();
            let numero = inputNumeroReporte.val();

            if (almacen > 0 && numero > 0) {
                mdlVerReporte.modal('hide');
                selectAlmacenReporte.val('');
                inputNumeroReporte.val('');

                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Enkontrol/Almacen/ImprimirMovimientoEntradaTraspaso', { almacen, numero })
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

        function limpiarVista() {
            selectAlmacenOrigen.val('');
            selectCentroCostoOrigen.val('');
            selectAlmacenDestino.val('');
            selectCentroCostoDestino.val('');
            inputFolioTraspaso.val('');

            tblEntradas.DataTable().clear().draw();
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        String.prototype.parseDate = function () {
            return new Date(parseInt(this.replace('/Date(', '')));
        }
        Date.prototype.parseDate = function () {
            return this;
        }
        Date.prototype.addDays = function (days) {
            var date = new Date(this.valueOf());
            date.setDate(date.getDate() + days);
            return date;
        }
        $.fn.commasFormat = function () {
            this.each(function (i) {
                $(this).change(function (e) {
                    if (isNaN(parseFloat(this.value))) return;
                    this.value = parseFloat(this.value).toFixed(6);
                });
            });
            return this;
        }

        init();
    }
    $(document).ready(function () {
        Enkontrol.Compras.Requisicion.Entradas = new Entradas();
    })
        .ajaxStart(function () {
            $.blockUI({ message: 'Procesando...' });
        })
        .ajaxStop(function () {
            $.unblockUI();
        });
})();