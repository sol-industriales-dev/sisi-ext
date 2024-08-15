(function () {
    $.namespace('Enkontrol.Compras.Requisicion.salidaConsumo');
    salidaConsumo = function () {
        const selCC = $("#selCC");
        const selReqTipo = $("#selReqTipo");
        const tblSalidas = $('#tblSalidas');
        const mdlExistenciaDetalle = $('#mdlExistenciaDetalle');
        const report = $("#report");
        const tblBorrador = $('#tblBorrador');
        const tblUbicacion = $('#tblUbicacion');
        const mdlUbicacionDetalle = $('#mdlUbicacionDetalle');
        const btnImprimirBorrador = $('#btnImprimirBorrador');
        const btnLimpiarBorrador = $('#btnLimpiarBorrador');
        const mdlSalidaDetalle = $('#mdlSalidaDetalle');
        const tblSalidaDetalle = $('#tblSalidaDetalle');
        const btnGuardarUbicacion = $('#btnGuardarUbicacion');
        const botonGuardarSalida = $('#botonGuardarSalida');
        const selectCentroCostoDestino = $('#selectCentroCostoDestino');
        const selectAlmacenDestino = $('#selectAlmacenDestino');
        const inputComentarios = $('#inputComentarios');

        _filaInsumo = null;
        _ccRequisicion = null;
        _numeroRequisicion = null;

        const guardarSalidas = (salidaNormal, salidasConsumo) => { return $.post('/Enkontrol/Requisicion/GuardarConsumo', { salidaNormal, salidasConsumo }) };

        selCC.on('change', function () {
            getRequisiciones()
        });

        selReqTipo.on('change', function () {
            getRequisiciones()
        });

        mdlExistenciaDetalle.on('click', '.btnUbicacionDetalle', function () {
            let row = $(this).closest('tr');
            let cc = $(row).attr('cc');
            let almacenID = $(row).attr('almacenorigenid');
            let insumo = $(row).attr('insumo');

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Requisicion/GetUbicacionDetalle', { cc, almacenID, insumo })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        if (response.data != null) {
                            AddRows(tblUbicacion, response.data);

                            mdlUbicacionDetalle.modal('show');
                        }
                    } else {

                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

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

                listUbicacionMovimiento.push({
                    insumo: rowData.insumo,
                    cantidad: rowData.cantidad,
                    area_alm: rowData.area_alm,
                    lado_alm: rowData.lado_alm,
                    estante_alm: rowData.estante_alm,
                    nivel_alm: rowData.nivel_alm,
                    cantidadMovimiento: cantidadSalida
                });
            });

            if (flagSobrepasoExistencia) {
                AlertaGeneral('Alerta', 'No puede surtir más de las existencias por almacén.');
            } else {
                _filaInsumo.find('td .btnUbicacionDetalle').data('listUbicacionMovimiento', listUbicacionMovimiento);
                _filaInsumo.find('td .btnUbicacionDetalle').data('totalUbicacion', totalUbicacion);
                _filaInsumo.find('td .labelUbicacion').val(totalUbicacion);
                _filaInsumo.find('td .labelUbicacion').text(totalUbicacion);
                _filaInsumo.find('td .SalidaConsumo').val(totalUbicacion);

                mdlUbicacionDetalle.modal('hide');
            }
        });

        tblSalidaDetalle.on('change', '.selectAreaCuenta', function () {
            checarPermisoAreaCuenta($(this)); //Áreas cuenta 14-1 y 14-2.
        });

        function init() {
            $('.select2').select2();

            initForm();
            initTableSalidas();
            initTableBorrador();
            initTableUbicacion();
            initTableSalidaDetalle();

            selectCentroCostoDestino.fillCombo('/Enkontrol/Requisicion/FillComboCcAsigReq', null, false, null);
            selectAlmacenDestino.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, null);

            btnImprimirBorrador.click(verReporteBorrador);
            btnLimpiarBorrador.click(limpiarBorrador);
            btnLimpiarBorrador.click();
            botonGuardarSalida.click(guardarSalida);
        }

        function initForm() {
            selCC.fillCombo('/Enkontrol/Requisicion/FillComboCcAsigReq', null, false, null);
        }

        function getRequisiciones() {
            axios.get('/Enkontrol/Requisicion/GetReqSalidasConsumo', { params: { cc: selCC.val() != '' ? selCC.val() : 0, tipo: +selReqTipo.val() } })
                .then(response => {
                    let { success, data } = response.data;
                    if (success) {
                        let datos = JSON.parse(data);
                        AddRows(tblSalidas, datos);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));

            // $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            // $.get('/Enkontrol/Requisicion/GetReqSalidasConsumo', { cc: selCC.val() != '' ? selCC.val() : 0, tipo: selReqTipo.val() })
            //     .always($.unblockUI)
            //     .then(response => {
            //         if (response.success) {
            //             AddRows(tblSalidas, response.data);
            //         } else {
            //             AlertaGeneral(`Alerta`, response.message);
            //         }
            //     }, error => {
            //         AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            //     }
            //     );
        }

        function initTableSalidas() {
            tblSalidas.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                rowId: 'id',
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tblSalidas.on('click', '.btn-req-detalle', function () {
                        const cc = $(this).attr('cc');
                        const req = $(this).attr('req');
                        const almacen = $(this).attr('almacen');

                        _ccRequisicion = cc;
                        _numeroRequisicion = req;

                        axios.get('/Enkontrol/Requisicion/GetReqDetSalidasConsumo', { params: { cc, req, almacen } })
                            .catch(error => AlertaGeneral(error.message))
                            .then(response => {
                                let { success, data } = response.data;

                                if (success) {
                                    AddRows(tblSalidaDetalle, data);

                                    if (data.length > 0) {
                                        selectCentroCostoDestino.val(data[0].cc);
                                        selectAlmacenDestino.val(data[0].almacen);
                                    }

                                    mdlSalidaDetalle.modal('show');
                                }
                            });
                    });

                    tblSalidas.on('click', '.btnAgregarBorrador', function () {
                        let $row = $(this).closest('tr');
                        let rowData = tblSalidas.DataTable().row($row).data();

                        let dataBorrador = tblBorrador.DataTable().rows().data();
                        let flagRequisicionRepetida = dataBorrador.filter(x => x.numero == rowData.numero).length > 0;

                        if (!flagRequisicionRepetida) {
                            $.blockUI({ message: 'Procesando...' });
                            $.post('/Enkontrol/Requisicion/GetUbicacionPorRequisicion', { requisicion: rowData })
                                .always($.unblockUI)
                                .then(response => {
                                    if (response.success) {
                                        $.each(response.data, function (index, row) {
                                            dataBorrador.push(row);
                                        });

                                        tblBorrador.DataTable().clear();
                                        tblBorrador.DataTable().rows.add(dataBorrador).draw();
                                        // AddRows(tblBorrador, response.data);
                                    } else {
                                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                                    }
                                }, error => {
                                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                                }
                                );
                        } else {
                            AlertaGeneral(`Alerta`, `Ya se agregó la requisición #${rowData.numero}.`);
                        }
                    });
                },
                columns: [
                    { data: 'ccDescripcion', title: 'Centro de Costo' },
                    { data: 'numero', title: 'Número Requisición' },
                    { data: 'numeroOCString', title: 'Números O.C.' },
                    { data: 'fecha', title: 'Fecha' },
                    { data: 'almacenLAB', title: 'LAB' },
                    {
                        sortable: false,
                        title: 'Detalle', render: (data, type, row, meta) => {
                            let div = document.createElement('div');
                            let button = document.createElement('button');
                            let i = document.createElement('i')

                            button.id = 'button_' + meta.row;
                            button.classList.add('btn');
                            button.classList.add('btn-xs');
                            button.classList.add('btn-primary');
                            button.classList.add('btn-req-detalle');

                            $(button).attr('cc', row.cc);
                            $(button).attr('req', row.numero);
                            $(button).attr("almacen", row.libre_abordo);

                            i.classList.add('fas')
                            i.classList.add('fa-eye')


                            $(button).append(i);
                            $(div).append(button);

                            return div.outerHTML;
                        }
                    },
                    {
                        title: 'Borrador', render: function (data, type, row, meta) {
                            let html = `<button class="btn btn-xs btn-warning btnAgregarBorrador">
                                            <i class="fa fa-arrow-down"></i>
                                        </button>`;

                            return html;
                        }
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
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableBorrador() {
            tblBorrador.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bLengthChange: false,
                bInfo: false,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'ccDescripcion', title: 'Centro de Costo' },
                    { data: 'numero', title: 'Núm. Requisición' },
                    { data: 'fecha', title: 'Fecha' },
                    { data: 'almacenLAB', title: 'LAB' },
                    { data: 'insumoDesc', title: 'Insumo' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'area_alm', title: 'Área' },
                    { data: 'lado_alm', title: 'Lado' },
                    { data: 'estante_alm', title: 'Estante' },
                    { data: 'nivel_alm', title: 'Nivel' }
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
                        targets: [2]
                    },
                    { width: '20%', targets: [0, 4] }
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
                        render: (data, type, row, meta) => {
                            return `<div class="input-group">
                                        <span class="input-group-btn">
                                            <button class="btn btn-xs btn-default btnCantidadTotalSalida" type="button">
                                                <i class="fa fa-arrow-right"></i>
                                            </button>
                                        </span>
                                        <input type="text" class="form-control text-center inputCantidadSalida" style="height: 22px;">
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

        function initTableSalidaDetalle() {
            tblSalidaDetalle.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblSalidaDetalle.on('click', '.btnUbicacionDetalle', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblSalidaDetalle.DataTable().row(row).data();

                        _filaInsumo = row;

                        let cc = rowData.cc;
                        let almacenID = rowData.almacen;
                        let partida = rowData.partidaRequisicion;
                        let insumo = rowData.insumo;

                        $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                        $.post('/Enkontrol/Requisicion/GetUbicacionDetalleSurtido', { cc, numero_requisicion: _numeroRequisicion, almacenID, partida, insumo })
                            .always($.unblockUI)
                            .then(response => {
                                if (response.success) {
                                    if (response.data != null) {
                                        AddRows(tblUbicacion, response.data.filter(function (x) { return x.cantidad > 0 }));

                                        mdlUbicacionDetalle.modal('show');
                                    }
                                } else {
                                    AlertaGeneral(`Alerta`, `Error al recuperar la información.`);
                                }
                            }, error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            }
                            );
                    });

                    tblSalidaDetalle.on('click', '.btn-salida', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblSalidaDetalle.DataTable().row(row).data();

                        let lstSalidas = [];
                        let esSalidaNormal = false;

                        const cc = rowData.cc;
                        const cc_destino = row.find('.selectCCDestino').val()
                        const almacenOrigenID = rowData.almacen;
                        const importe = 0;
                        const almacenDestinoID = row.find('.selectAlmacenDestino').val()
                        const numeroReq = rowData.numeroReq;
                        const insumo = rowData.insumo;
                        const solicitado = rowData.solicitado;
                        const cantidad = unmaskNumero(row.find('.SalidaConsumo').val());

                        if (almacenOrigenID != almacenDestinoID) {
                            esSalidaNormal = false
                        }
                        else {
                            esSalidaNormal = true
                        }

                        detalle = new Object();

                        detalle.cc = cc;
                        detalle.ccDestino = cc_destino;
                        detalle.almacenOrigenID = almacenOrigenID;
                        detalle.importe = importe;
                        detalle.almacenDestinoID = almacenDestinoID;
                        detalle.numeroReq = numeroReq;
                        detalle.insumo = insumo;
                        detalle.solicitado = solicitado;
                        detalle.cantidad = cantidad;
                        detalle.area = unmaskNumero($(row).find('.selectAreaCuenta option:selected').val());
                        detalle.cuenta = unmaskNumero($(row).find('.selectAreaCuenta option:selected').attr('data-prefijo'));

                        detalle.listUbicacionMovimiento = row.find('.btnUbicacionDetalle').data('listUbicacionMovimiento');

                        if (detalle.cantidad > 0) {
                            lstSalidas.push(detalle);
                        }

                        if (lstSalidas.length > 0) {
                            if (lstSalidas)
                                $.blockUI({ message: 'Procesando...' });
                            guardarSalidas(esSalidaNormal, lstSalidas).always($.unblockUI).done(function (response) {
                                if (response.success) {
                                    mdlSalidaDetalle.modal('hide');

                                    if (response.flagMaquinaStandBy) {
                                        AlertaGeneral(`Alerta`, `Se ha generado la salida por consumo. 
                                        Se quitó el estado "Stand-By" de la máquina "${cc}".`);
                                    } else {
                                        AlertaGeneral('Aviso', 'Se ha generado la salida por consumo.');
                                    }

                                    initForm();
                                    verReporte();
                                } else {
                                    AlertaGeneral('Aviso', 'Ocurrió un error al intentar guardar las salidas por consumo.');
                                }
                            });
                        }
                    });
                },
                createdRow: function (row, rowData) {
                    let tdAlmacenPropio = $(row).find('.tdAlmacenPropio');
                    let tdAlmacenExterno = $(row).find('.tdAlmacenExterno');
                    let tdOrdenCompra = $(row).find('.tdOrdenCompra');
                    let tdCantidadConsumida = $(row).find('.tdCantidadConsumida');

                    if (rowData.solicitadoAP > 0 && (rowData.solicitadoAP == rowData.cantidadAP)) {
                        tdAlmacenPropio.css('background-color', '#ccffcc');
                    }
                    if (rowData.solicitadoAP == 0 && (rowData.solicitadoAP == rowData.cantidadAP)) {
                        tdAlmacenPropio.css('background-color', 'white');
                    }
                    if (rowData.solicitadoAP > 0 && (rowData.cantidadAP > 0 && rowData.cantidadAP < rowData.solicitadoAP)) {
                        tdAlmacenPropio.css('background-color', '#ffc000');
                    }
                    if (rowData.solicitadoAP > 0 && (rowData.cantidadAP == 0)) {
                        tdAlmacenPropio.css('background-color', '#c00000');
                    }

                    if (rowData.solicitadoAE > 0 && (rowData.solicitadoAE == rowData.cantidadAE)) {
                        tdAlmacenExterno.css('background-color', '#ccffcc');
                    }
                    if (rowData.solicitadoAE == 0 && (rowData.solicitadoAE == rowData.cantidadAE)) {
                        tdAlmacenExterno.css('background-color', 'white');
                    }
                    if (rowData.solicitadoAE > 0 && (rowData.cantidadAE > 0 && rowData.cantidadAE < rowData.solicitadoAE)) {
                        tdAlmacenExterno.css('background-color', '#ffc000');
                    }
                    if (rowData.solicitadoAE > 0 && (rowData.cantidadAE == 0)) {
                        tdAlmacenExterno.css('background-color', '#c00000');
                    }

                    if (rowData.solicitadoOC > 0 && (rowData.solicitadoOC == rowData.cantidadOC)) {
                        tdOrdenCompra.css('background-color', '#ccffcc');
                    }
                    if (rowData.solicitadoOC == 0 && (rowData.solicitadoOC == rowData.cantidadOC)) {
                        tdOrdenCompra.css('background-color', 'white');
                    }
                    if (rowData.solicitadoOC > 0 && (rowData.cantidadOC > 0 && rowData.cantidadOC < rowData.solicitadoOC)) {
                        tdOrdenCompra.css('background-color', '#ffc000');
                    }
                    if (rowData.solicitadoOC > 0 && (rowData.cantidadOC == 0)) {
                        tdOrdenCompra.css('background-color', '#c00000');
                    }

                    $(row).find('.selectAreaCuenta').fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: rowData.cc }, false, "000-000");

                    if (rowData.consumidoCompleto) {
                        tdCantidadConsumida.css('background-color', '#ccffcc');
                        $(row).find('input, select, button').attr('disabled', true);
                        // $(row).find('.btnUbicacionDetalle').attr('disabled', false);
                        $(row).addClass('renglonConsumidoCompleto');
                    }
                },
                columns: [
                    { data: 'partidaRequisicion', title: 'Partida' },
                    { data: 'insumoDescripcion', title: 'Insumo' },
                    { data: 'solicitado', title: 'Solicitado' },
                    {
                        title: 'Almacén Propio' + '\n' + '(Solicitado/Surtido)', render: function (data, type, row, meta) {
                            return formatValue(row.solicitadoAP) + " / " + formatValue(row.cantidadAP);
                        }, className: 'dt-center tdAlmacenPropio'
                    },
                    {
                        title: 'Almacén Externo' + '\n' + '(Solicitado/Surtido)', render: function (data, type, row, meta) {
                            return formatValue(row.solicitadoAE) + " / " + formatValue(row.cantidadAE);
                        }, className: 'dt-center tdAlmacenExterno'
                    },
                    {
                        title: 'Orden Compra' + '\n' + '(Solicitado/Surtido)', render: function (data, type, row, meta) {
                            return formatValue(row.solicitadoOC) + " / " + formatValue(row.cantidadOC);
                        }, className: 'dt-center tdOrdenCompra'
                    },
                    { data: 'cantidadConsumida', title: 'Cantidad Consumida', className: 'dt-center tdCantidadConsumida' },
                    // { data: 'existencia', title: 'Existencia' },
                    {
                        title: 'Salida', render: function (data, type, row, meta) {
                            return `<div class="input-group">
                                        <span class="input-group-btn">
                                            <button class="btn btn-default btn-sm btnUbicacionDetalle" type="button">
                                                <i class="fa fa-eye"></i>
                                            </button>
                                        </span>
                                        <input class="form-control text-center SalidaConsumo" disabled value="0">
                                    </div>`;
                        }
                    },
                    {
                        title: 'Área-Cuenta', render: function (data, type, row, meta) {
                            return `<select class="form-control text-center selectAreaCuenta"></select>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '10%', targets: 7 }
                ]
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function verReporte() {
            // $.blockUI({ message: 'Procesando...' });
            // report.attr("src", `/Reportes/Vista.aspx?idReporte=110`);
            // document.getElementById('report').onload = function () {
            //     openCRModal();
            //     $.unblockUI();
            // };

            $.blockUI({ message: 'Procesando...' });
            report.attr("src", `/Reportes/Vista.aspx?idReporte=124`);
            document.getElementById('report').onload = function () {
                openCRModal();
                $.unblockUI;
            };
        }

        function verReporteBorrador() {
            if (tblBorrador.DataTable().rows().data().length > 0) {
                $.blockUI({ message: 'Procesando...' });
                report.attr("src", `/Reportes/Vista.aspx?idReporte=114`);
                document.getElementById('report').onload = function () {
                    openCRModal();
                    $.unblockUI();
                };
            } else {
                AlertaGeneral(`Alerta`, `No hay datos en el borrador.`);
            }
        }

        function limpiarBorrador() {
            tblBorrador.DataTable().clear().draw();

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Requisicion/LimpiarSesionBorrador', null)
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

        function checarPermisoAreaCuenta(element) {
            let area = $(element).find('option:selected').val();
            let cuenta = $(element).find('option:selected').attr('data-prefijo');

            let flagPermiso_14_1_14_2 = false;

            if ((area == 14 && cuenta == 1) || (area == 14 && cuenta == 2)) {
                flagPermiso_14_1_14_2 = true;
            }

            if (flagPermiso_14_1_14_2) {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Enkontrol/Almacen/ChecarPermisoAreaCuenta')
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            if (!response.flagPermiso_14_1_14_2) {
                                AlertaGeneral(`Alerta`, `Su usuario no puede dar salida por consumo para las áreas cuenta 14-1 y 14-2.`);
                            }
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        }

        function guardarSalida() {
            // let listaSalida = [];

            // tblSalidaDetalle.find("tbody tr").each(function (idx, row) {
            //     let rowData = tblSalidaDetalle.DataTable().row(row).data();

            //     // let esSalidaNormal = !(almacenOrigenID != almacenDestinoID);
            //     let salida = {
            //         cc: rowData.cc,
            //         ccDestino: $(row).find('.selectCCDestino').val(),
            //         almacenOrigenID: rowData.almacen,
            //         importe: 0,
            //         almacenDestinoID: $(row).find('.selectAlmacenDestino').val(),
            //         numeroReq: rowData.numeroReq,
            //         insumo: rowData.insumo,
            //         solicitado: rowData.solicitado,
            //         cantidad: unmaskNumero($(row).find('.SalidaConsumo').val()),
            //         area: unmaskNumero($(row).find('.selectAreaCuenta').val()),
            //         cuenta: unmaskNumero($(row).find('.selectAreaCuenta option:selected').attr('data-prefijo')),
            //         listUbicacionMovimiento: $(row).find('.btnUbicacionDetalle').data('listUbicacionMovimiento')
            //     };

            //     if (salida.cantidad > 0) {
            //         listaSalida.push(salida);
            //     }
            // });

            // if (listaSalida.length > 0) {
            //     $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            //     $.post('/Enkontrol/Almacen/GuardarSalidaConsumoOrigen', { listaSalida })
            //         .always($.unblockUI)
            //         .then(response => {
            //             if (response.success) {
            //                 mdlSalidaDetalle.modal('hide');

            //                 if (response.flagMaquinaStandBy) {
            //                     AlertaGeneral(`Alerta`, `Se ha generado la salida por consumo. Se quitó el estado "Stand-By" de la máquina "${listaSalida[0].cc}".`);
            //                 } else {
            //                     AlertaGeneral('Aviso', 'Se ha generado la salida por consumo.');
            //                 }

            //                 initForm();
            //                 verReporte();
            //             } else {
            //                 AlertaGeneral(`Alerta`, response.message);
            //             }
            //         }, error => {
            //             AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            //         }
            //         );
            // }

            let movimiento = getInformacion();

            if (movimiento.detalle.length > 0) {
                // btnGuardar.attr('disabled', true);

                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GuardarSalidaConsumoOrigen', { movimiento })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            // btnGuardar.attr('disabled', false);
                            if (response.flagMaquinaStandBy) {
                                AlertaGeneral(`Alerta`, `Se ha guardado la información. Se quitó el estado "Stand-By" de la máquina "${movimiento.cc}".`);
                            } else {
                                AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                            }

                            initForm();
                            verReporte();
                            mdlSalidaDetalle.modal('hide');

                            // limpiarVista();
                            // verReporte();

                            // if (response.flagEntradaInvFisico) {
                            //     let almacenEntrada = response.almacenEntradaInvFisico;
                            //     let numeroEntrada = response.numeroEntradaInvFisico;

                            //     llenarInformacionEntrada(almacenEntrada, numeroEntrada, response.centroCostoEntradaInvFisico, response.listaPartidasEntradaInvFisico);
                            //     modalEntradaInvFisico.modal('show');
                            //     // cargarInformacionEntrada(almacenEntrada, numeroEntrada, response.listaPartidasEntradaInvFisico);
                            // }
                        } else {
                            AlertaGeneral(`Alerta`, `${response.message}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                AlertaGeneral(`Alerta`, `El movimiento no tiene partidas.`);
            }
        }

        function getInformacion() {
            let movimiento = {
                almacen: selectAlmacenDestino.val(),
                almacenDesc: '',
                tipo_mov: 51,
                numero: 0,
                cc: selectCentroCostoDestino.val(),
                ccDesc: '',
                fecha: null, //Valor asignado en el back
                fechaString: '', //Valor asignado en el back
                total: 0, //Valor asignado en el back
                estatus: 'A',
                transferida: 'N',
                empleado: 0, //Valor asignado en el back
                empleadoDesc: '', //Valor asignado en el back
                comentarios: inputComentarios.val(),
                sector_id: null
            };

            movimiento.detalle = getPartidas();
            movimiento.numeroRequisicion = movimiento.detalle[0].numeroRequisicion;

            return movimiento;
        }

        function getPartidas() {
            let detalle = [];

            tblSalidaDetalle.find('tbody tr').each((index, row) => {
                let rowData = tblSalidaDetalle.DataTable().row(row).data();
                // let cantidad = unmaskNumero($(row).find('.SalidaConsumo').val());

                if (rowData != undefined) {
                    let listaUbicacionMovimiento = $(row).find('.btnUbicacionDetalle').data('listUbicacionMovimiento');

                    if (typeof listaUbicacionMovimiento !== 'undefined' && listaUbicacionMovimiento != null) {
                        $(listaUbicacionMovimiento).each(function (index, x) {
                            // let costo_prom = rowData.costo_prom;
                            // let importe = 0;

                            detalle.push({
                                partidaRequisicion: rowData.partidaRequisicion,
                                almacen: selectAlmacenDestino.val(),
                                tipo_mov: 51,
                                numero: 0,
                                partida: detalle.length + 1,
                                insumo: rowData.insumo,
                                comentarios: '',
                                area: $(row).find('.selectAreaCuenta option:selected').val(),
                                cuenta: $(row).find('.selectAreaCuenta option:selected').attr('data-prefijo'),
                                cantidad: x.cantidadMovimiento,
                                // precio: costo_prom,
                                // costo_prom: costo_prom,
                                // importe: importe,
                                sector_id: null,
                                area_alm: x.area_alm,
                                lado_alm: x.lado_alm,
                                estante_alm: x.estante_alm,
                                nivel_alm: x.nivel_alm,
                                partida_oc: 0,
                                numeroRequisicion: rowData.numeroReq
                            });
                        });
                    }
                }
            });

            return detalle;
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
        Enkontrol.Compras.Requisicion.salidaConsumo = new salidaConsumo();
    });
})();