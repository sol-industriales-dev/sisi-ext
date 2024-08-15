(() => {
    $.namespace('Enkontrol.Almacen.Almacen.GestionTraspaso');
    GestionTraspaso = function () {
        //#region Selectores
        const selectCCOrigen = $('#selectCCOrigen');
        const selectAlmacenOrigen = $('#selectAlmacenOrigen');
        const selectCCDestino = $('#selectCCDestino');
        const selectAlmacenDestino = $('#selectAlmacenDestino');
        const tblTraspasosPendientes = $('#tblTraspasosPendientes');
        const tblTraspasosRechazados = $('#tblTraspasosRechazados');
        const btnBuscar = $('#btnBuscar');
        const btnGuardar = $('#btnGuardar');
        const inputFolioInterno = $('#inputFolioInterno');
        const textAreaComentarios = $('#textAreaComentarios');
        const mdlUbicacionDetalle = $('#mdlUbicacionDetalle');
        const tblUbicacion = $('#tblUbicacion');
        const btnGuardarUbicacion = $('#btnGuardarUbicacion');
        const btnCargarExcel = $('#btnCargarExcel');
        const mdlCargarExcel = $('#mdlCargarExcel');
        const btnGuardarExcel = $('#btnGuardarExcel');
        const report = $("#report");
        const btnModalReporte = $('#btnModalReporte');
        const mdlVerReporte = $('#mdlVerReporte');
        const selectAlmacenReporte = $('#selectAlmacenReporte');
        const inputNumeroReporte = $('#inputNumeroReporte');
        const btnImprimible = $('#btnImprimible');
        //#endregion

        _filaInsumo = null;

        (function init() {
            initTblTraspasosPendientes();
            initTblTraspasosRechazados();
            initTableUbicacion();

            checkPermisoTraspasoMasivo();

            selectCCOrigen.fillCombo('/Enkontrol/Almacen/FillComboCC', null, false, 'Todos');
            selectAlmacenOrigen.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, 'Todos');
            selectCCDestino.fillCombo('/Enkontrol/Almacen/FillComboCC', null, false, 'Todos');
            selectAlmacenDestino.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, 'Todos');

            selectAlmacenReporte.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, null);

            btnBuscar.click(cargarTraspasos);
            btnGuardar.click(guardarAutorizacionesTraspasos);
            btnModalReporte.click(function () { mdlVerReporte.modal('show'); });
            btnImprimible.click(verImprimible);
        })();

        btnGuardarUbicacion.on('click', function () {
            let flagSobrepasoExistencia = false;
            let listUbicacionMovimiento = [];
            let totalUbicacion = 0;

            tblUbicacion.find("tbody tr").each(function (idx, row) {
                let rowData = tblUbicacion.DataTable().row(row).data();
                let inputCantidadSalida = $(row).find('.inputCantidadSalida');
                let cantidadSalida = inputCantidadSalida.val() != '' ? parseFloat(inputCantidadSalida.val()) : 0;

                if (cantidadSalida > rowData.cantidad) {
                    flagSobrepasoExistencia = true;
                    $(row).find('.inputCantidadSalida').addClass('campoInvalido');
                } else {
                    $(row).find('.inputCantidadSalida').removeClass('campoInvalido');
                }

                if (cantidadSalida < 0) {
                    AlertaGeneral('Alerta', 'No puede introducir números negativos.');
                }

                totalUbicacion += cantidadSalida;

                if (cantidadSalida > 0) {
                    listUbicacionMovimiento.push({
                        insumo: rowData.insumo,
                        cantidad: rowData.cantidad,
                        area_alm: rowData.area_alm,
                        lado_alm: rowData.lado_alm,
                        estante_alm: rowData.estante_alm,
                        nivel_alm: rowData.nivel_alm,
                        cantidadMovimiento: cantidadSalida
                    });
                }
            });

            if (flagSobrepasoExistencia) {
                AlertaGeneral('Alerta', 'No puede surtir más de las existencias por almacén.');
            } else {
                _filaInsumo.find('td .btnUbicacionDetalle').data('listUbicacionMovimiento', listUbicacionMovimiento);
                _filaInsumo.find('td .btnUbicacionDetalle').data('totalUbicacion', totalUbicacion);
                _filaInsumo.find('td .labelUbicacion').val(totalUbicacion);
                _filaInsumo.find('td .labelUbicacion').text(totalUbicacion);
                _filaInsumo.find('td .inputCantidadAutorizar').val(totalUbicacion);

                _filaInsumo.find('td input').change();

                mdlUbicacionDetalle.modal('hide');
            }
        });

        btnCargarExcel.on('click', function () {
            mdlCargarExcel.modal('show');
        });

        btnGuardarExcel.on('click', function () {
            var request = new XMLHttpRequest();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            mdlCargarExcel.modal('hide');

            request.open("POST", "/Enkontrol/Almacen/CargarExcelTraspasoMasivo");
            request.send(formData());

            request.onload = function (response) {
                if (request.status == 200) {
                    let respuesta = JSON.parse(request.response);

                    if (respuesta.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);

                        $('#inputFileExcel').val('');
                    } else {
                        AlertaGeneral(`Alerta`, `Error al guardar la información. ${respuesta.message}`);
                    }
                } else {
                    AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                }

                $.unblockUI();
            };
        });

        function initTblTraspasosPendientes() {
            tblTraspasosPendientes.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                searching: false,
                initComplete: function (settings, json) {
                    tblTraspasosPendientes.on('click', 'input[type="checkbox"]', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblTraspasosPendientes.DataTable().row(row).data();

                        if ($(this).hasClass('checkBoxAut')) {
                            if ($(this).prop('checked')) {
                                rowData.checkboxAutorizado = true;
                            } else {
                                rowData.checkboxAutorizado = false;
                            }
                        } else if ($(this).hasClass('checkBoxRech')) {
                            if ($(this).prop('checked')) {
                                rowData.checkboxRechazado = true;

                                // row.find('.btnCantidadTotalAutorizar').attr('disabled', true);
                                row.find('.btnUbicacionDetalle').attr('disabled', true);
                                row.find('.btnUbicacionDetalle').data('listUbicacionMovimiento', null);
                                row.find('.btnUbicacionDetalle').data('totalUbicacion', null);
                                // row.find('.inputCantidadAutorizar').attr('disabled', true);
                                row.find('.inputCantidadAutorizar').val('');
                            } else {
                                rowData.checkboxRechazado = false;

                                // row.find('.btnCantidadTotalAutorizar').attr('disabled', false);
                                row.find('.btnUbicacionDetalle').attr('disabled', false);
                                row.find('.btnUbicacionDetalle').data('listUbicacionMovimiento', null);
                                row.find('.btnUbicacionDetalle').data('totalUbicacion', null);
                                // row.find('.inputCantidadAutorizar').attr('disabled', false);
                                row.find('.inputCantidadAutorizar').val('');
                            }
                        }
                    });

                    tblTraspasosPendientes.on('click', 'td', function () {
                        let rowData = tblTraspasosPendientes.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblTraspasosPendientes.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");

                            textAreaComentarios.val(rowData.comentarios);
                        } else {
                            textAreaComentarios.val('');
                        }
                    });

                    tblTraspasosPendientes.on('click', '.btnCantidadTotalAutorizar', function () {
                        let rowData = tblTraspasosPendientes.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');

                        $row.find('.inputCantidadAutorizar').val(rowData.cantidad);
                        $row.find('.inputCantidadAutorizar').change();
                    });

                    tblTraspasosPendientes.on('click', '.btnUbicacionDetalle', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblTraspasosPendientes.DataTable().row(row).data();

                        _filaInsumo = row;

                        let cc = rowData.ccOrigen;
                        let almacenID = rowData.almacenOrigen;
                        let insumo = rowData.insumo;

                        $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                        $.post('/Enkontrol/Requisicion/GetUbicacionDetalle', { cc, almacenID, insumo })
                            .always($.unblockUI)
                            .then(response => {
                                if (response.success) {
                                    if (response.data != null) {
                                        let ubicacionesFiltradas = response.data.filter(x => x.cantidad > 0);

                                        AddRows(tblUbicacion, ubicacionesFiltradas);

                                        if ($(this).data('listUbicacionMovimiento') != undefined && $(this).data('listUbicacionMovimiento') != null) {
                                            let listUbicacionMovimiento = $(this).data('listUbicacionMovimiento');
                                            let tablaData = tblUbicacion.DataTable().rows().data();

                                            listUbicacionMovimiento.forEach(item => {
                                                let renglonData = tablaData.toArray().find(x => {
                                                    return x.area_alm == item.area_alm &&
                                                        x.lado_alm == item.lado_alm &&
                                                        x.estante_alm == item.estante_alm &&
                                                        x.nivel_alm == item.nivel_alm
                                                });

                                                if (renglonData != undefined) {
                                                    renglonData.cantidadMovimiento = item.cantidadMovimiento;
                                                }
                                            });

                                            tblUbicacion.DataTable().clear();
                                            tblUbicacion.DataTable().rows.add(tablaData).draw();
                                        }

                                        mdlUbicacionDetalle.modal('show');
                                    }
                                } else {
                                    AlertaGeneral(`Alerta`, `Error al recuperar la información.`);
                                }
                            }, error => {
                                AlertaGeneral(
                                    `Operación fallida`,
                                    `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`
                                );
                            }
                            );
                    });
                },
                columns: [
                    { data: 'folioInternoString', title: 'Folio Interno' },
                    { data: 'descInsumo', title: 'Insumo' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'ccOrigenDesc', title: 'CC Origen' },
                    { data: 'almacenOrigenDesc', title: 'Almacén Origen' },
                    { data: 'ccDestinoDesc', title: 'CC Destino' },
                    { data: 'almacenDestinoDesc', title: 'Almacén Destino' },
                    { data: 'fecha', title: 'Fecha' },
                    {
                        sortable: false, data: 'comentariosGestion',
                        render: (data, type, row, meta) => {
                            let valor = data != undefined ? data : '';

                            return `<input type="text" class="form-control text-left inputComentariosGestion" value="${valor}" style="height: 22px;">`;
                        },
                        title: 'Comentarios'
                    },
                    // {
                    //     sortable: false,
                    //     render: (data, type, row, meta) => {
                    //         return `<div class="input-group">
                    //                     <span class="input-group-btn">
                    //                         <button class="btn btn-xs btn-default btnCantidadTotalAutorizar" type="button"><i class="fa fa-arrow-right"></i></button>
                    //                     </span>
                    //                     <input type="text" class="form-control text-center inputCantidadAutorizar" disabled style="height: 22px;">
                    //                 </div>`;
                    //     },
                    //     title: 'Autorizar'
                    // },
                    {
                        data: 'cantidadTraspasar', title: 'Autorizar', render: function (data, type, row, meta) {
                            let valor = data != '' && data != undefined ? data : 0;

                            return `<div class="input-group">
                                        <span class="input-group-btn">
                                            <button class="btn btn-xs btn-default btnUbicacionDetalle" ${row.checkboxRechazado ? 'disabled' : ''}>
                                                <i class="fa fa-eye"></i>
                                            </button>
                                        </span>
                                        <input type="text" class="form-control text-center inputCantidadAutorizar" disabled value="${valor}" style="height: 22px;">
                                    </div>`;
                        }
                    },
                    {
                        sortable: false,
                        render: (data, type, row, meta) => {
                            let div = document.createElement('div');
                            let checkbox = document.createElement('input');
                            let label = document.createElement('label');

                            checkbox.id = 'checkboxRech_' + meta.row;
                            checkbox.setAttribute('type', 'checkbox');
                            checkbox.classList.add('form-control');
                            checkbox.classList.add('regular-checkboxdanger');
                            checkbox.classList.add('checkBoxRech');
                            checkbox.style.height = '25px';

                            label.setAttribute('for', checkbox.id);

                            $(div).append(checkbox);
                            $(div).append(label);

                            if (row.checkboxRechazado) {
                                $(checkbox).attr('checked', true);
                            }

                            return div.outerHTML;
                        },
                        title: 'Rechazar'
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        targets: [7]
                    },
                    { width: '15%', targets: [1, 3, 4, 5, 6, 8] }
                ]
            });
        }

        function initTblTraspasosRechazados() {
            tblTraspasosRechazados.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                searching: false,
                initComplete: function (settings, json) {
                    tblTraspasosRechazados.on('click', 'td', function () {
                        let rowData = tblTraspasosRechazados.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblTraspasosRechazados.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");

                            textAreaComentarios.val(rowData.comentarios);
                        } else {
                            textAreaComentarios.val('');
                        }
                    });
                },
                columns: [
                    { data: 'folioInternoString', title: 'Folio Interno' },
                    { data: 'descInsumo', title: 'Insumo' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'ccOrigenDesc', title: 'CC Origen' },
                    { data: 'almacenOrigenDesc', title: 'Almacén Origen' },
                    { data: 'ccDestinoDesc', title: 'CC Destino' },
                    { data: 'almacenDestinoDesc', title: 'Almacén Destino' },
                    { data: 'area_alm', title: 'Área' },
                    { data: 'lado_alm', title: 'Lado' },
                    { data: 'estante_alm', title: 'Estante' },
                    { data: 'nivel_alm', title: 'Nivel' },
                    { data: 'fecha', title: 'Fecha' },
                    { data: 'comentariosGestion', title: 'Comentarios' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        targets: [11]
                    },
                    { width: '20%', targets: [1, 3, 4, 5, 6] }
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
                        let rowData = tblTraspasosPendientes.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');

                        if (_filaInsumo != null) {
                            let cantidadTraspaso = tblTraspasosPendientes.DataTable().row(_filaInsumo).data().cantidad;
                            let cantidadAnteriorUbicacion = $row.find('.inputCantidadSalida').val();

                            let sumatoriaUbicacion = 0;

                            tblUbicacion.find('tbody tr').each(function (idx, row) {
                                let cantidadUbicacion = $(row).find('.inputCantidadSalida').val();

                                sumatoriaUbicacion += cantidadUbicacion;
                            });

                            sumatoriaUbicacion -= cantidadAnteriorUbicacion;

                            let cantidadPendiente = cantidadTraspaso - sumatoriaUbicacion;

                            if (cantidadPendiente <= rowData.cantidad) {
                                $row.find('.inputCantidadSalida').val(cantidadPendiente);
                                $row.find('.inputCantidadSalida').change();
                            } else {
                                $row.find('.inputCantidadSalida').val(rowData.cantidad);
                                $row.find('.inputCantidadSalida').change();
                            }
                        } else {
                            $row.find('.inputCantidadSalida').val(rowData.cantidad);
                            $row.find('.inputCantidadSalida').change();
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
                        sortable: false,
                        data: 'cantidadMovimiento',
                        render: (data, type, row, meta) => {
                            let valor = data != undefined ? data : '';

                            return `<div class="input-group">
                                        <span class="input-group-btn">
                                            <button class="btn btn-xs btn-default btnCantidadTotalSalida" type="button">
                                                <i class="fa fa-arrow-right"></i>
                                            </button>
                                        </span>
                                        <input type="text" class="form-control text-center inputCantidadSalida" value="${valor}" style="height: 22px;">
                                    </div>`;
                        },
                        title: 'Salida'
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '50%', targets: [0] }
                ]
            });
        }

        function cargarTraspasos() {
            cargarTraspasosPendientes();
            cargarTraspasosRechazados();
        }

        function cargarTraspasosPendientes() {
            let ccOrigen = selectCCOrigen.val() != 'Todos' ? selectCCOrigen.val() : 0;
            let almacenOrigen = selectAlmacenOrigen.val() != 'Todos' ? selectAlmacenOrigen.val() : 0;
            let ccDestino = selectCCDestino.val() != 'Todos' ? selectCCDestino.val() : 0;
            let almacenDestino = selectAlmacenDestino.val() != 'Todos' ? selectAlmacenDestino.val() : 0;
            let folioInterno = inputFolioInterno.val() != '' && !isNaN(inputFolioInterno.val()) ? inputFolioInterno.val() : 0;

            $.post('/Enkontrol/Almacen/GetTraspasosPendientes', { ccOrigen, almacenOrigen, ccDestino, almacenDestino, folioInterno }).then(response => {
                if (response.success) {
                    if (response.data != null) {
                        AddRows(tblTraspasosPendientes, response.data);
                    } else {
                        limpiarTabla(tblTraspasosPendientes);
                    }
                } else {
                    AlertaGeneral(`Alerta`, `No se encontró información.`);
                    limpiarTabla(tblTraspasosPendientes);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                limpiarTabla(tblTraspasosPendientes);
            }
            );
        }

        function cargarTraspasosRechazados() {
            let ccOrigen = selectCCOrigen.val() != 'Todos' ? selectCCOrigen.val() : 0;
            let almacenOrigen = selectAlmacenOrigen.val() != 'Todos' ? selectAlmacenOrigen.val() : 0;
            let ccDestino = selectCCDestino.val() != 'Todos' ? selectCCDestino.val() : 0;
            let almacenDestino = selectAlmacenDestino.val() != 'Todos' ? selectAlmacenDestino.val() : 0;
            let folioInterno = inputFolioInterno.val() != '' && !isNaN(inputFolioInterno.val()) ? inputFolioInterno.val() : 0;

            $.post('/Enkontrol/Almacen/GetTraspasosRechazados', { ccOrigen, almacenOrigen, ccDestino, almacenDestino, folioInterno }).then(response => {
                if (response.success) {
                    if (response.data != null) {
                        AddRows(tblTraspasosRechazados, response.data);
                    } else {
                        limpiarTabla(tblTraspasosRechazados);
                    }
                } else {
                    AlertaGeneral(`Alerta`, `No se encontró información.`);
                    limpiarTabla(tblTraspasosRechazados);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                limpiarTabla(tblTraspasosRechazados);
            }
            );
        }

        function guardarAutorizacionesTraspasos() {
            let listaAutorizados = [];

            tblTraspasosPendientes.find('tbody tr').each(function (idx, row) {
                let rowData = tblTraspasosPendientes.DataTable().row($(row)).data();

                let comentariosGestion = $(row).find('.inputComentariosGestion').val();
                let cantidadAutorizar = $(row).find('.inputCantidadAutorizar').val();
                let checkBoxRech = $(row).find('.checkBoxRech').prop('checked');

                if ((!isNaN(cantidadAutorizar) && cantidadAutorizar != '' && cantidadAutorizar > 0) || checkBoxRech) {
                    listaAutorizados.push({
                        folioInterno: rowData.folioInterno,
                        folioInternoString: rowData.folioInternoString,
                        ccOrigen: rowData.ccOrigen,
                        almacenOrigen: rowData.almacenOrigen,
                        ccDestino: rowData.ccDestino,
                        almacenDestino: rowData.almacenDestino,
                        insumo: rowData.insumo,
                        comentariosGestion: comentariosGestion,
                        cantidad: rowData.cantidad,
                        cantidadTraspasar: cantidadAutorizar,
                        checkBoxAutorizado: true,
                        checkBoxRechazado: checkBoxRech,
                        listUbicacionMovimiento: $(row).find('.btnUbicacionDetalle').data('listUbicacionMovimiento')
                    });
                }
            });

            if (listaAutorizados.length > 0) {
                btnGuardar.attr('disabled', true);

                $.post('/Enkontrol/Almacen/GuardarAutorizacionesTraspasos', { listaAutorizados }).then(response => {
                    if (response.success) {
                        btnGuardar.attr('disabled', false);
                        if (response.flagMaquinaStandBy) {
                            AlertaGeneral(`Alerta`, `Se ha guardado la información. Se quitó el estado "Stand-By" de la máquina.`);
                        } else {
                            AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        }
                        btnBuscar.click();
                    } else {
                        AlertaGeneral(`Alerta`, `${response.message}`);
                        btnBuscar.click();
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            } else {
                AlertaGeneral(`Alerta`, `No se ha capturado información.`);
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

        function checkPermisoTraspasoMasivo() {
            $.post('/Enkontrol/Almacen/CheckPermisoTraspasoMasivo').then(response => {
                if (response.success) {
                    if (response.data != null) {
                        btnCargarExcel.css('display', 'inline-block');
                    }
                } else {
                    AlertaGeneral(`Alerta`, `Error al consultar la información de permisos para el traspaso masivo.`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function verImprimible() {
            let almacen = selectAlmacenReporte.val();
            let numero = inputNumeroReporte.val();

            if (almacen > 0 && numero > 0) {
                mdlVerReporte.modal('hide');
                selectAlmacenReporte.val('');
                inputNumeroReporte.val('');

                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Enkontrol/Almacen/ImprimirMovimientoSalidaTraspaso', { almacen, numero })
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

        function verReporte() {
            report.attr("src", `/Reportes/Vista.aspx?idReporte=110`);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function formData() {
            let formData = new FormData();

            $.each(document.getElementById("inputFileExcel").files, function (i, file) {
                formData.append("files[]", file);
            });

            return formData;
        }
    }
    $(document).ready(() => Enkontrol.Almacen.Almacen.GestionTraspaso = new GestionTraspaso())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();