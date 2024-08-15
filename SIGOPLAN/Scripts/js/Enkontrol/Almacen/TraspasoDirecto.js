(() => {
    $.namespace('Enkontrol.Almacen.Almacen.TraspasoDirecto');
    TraspasoDirecto = function () {
        //#region Selectores
        //#region Salida
        const almOrigenNumSal = $('#almOrigenNumSal');
        const almOrigenDescSal = $('#almOrigenDescSal');
        const numMovSal = $('#numMovSal');
        const ccOrigenNumSal = $('#ccOrigenNumSal');
        const ccOrigenDescSal = $('#ccOrigenDescSal');
        const fechaMovSal = $('#fechaMovSal');
        const entregoNumSal = $('#entregoNumSal');
        const entregoDescSal = $('#entregoDescSal');
        const tipoMovSal = $('#tipoMovSal');
        const comentariosSal = $('#comentariosSal');
        const almDestinoNumSal = $('#almDestinoNumSal');
        const almDestinoDescSal = $('#almDestinoDescSal');
        const ccDestinoNumSal = $('#ccDestinoNumSal');
        const ccDestinoDescSal = $('#ccDestinoDescSal');
        const ordenTraspasoSal = $('#ordenTraspasoSal');
        const folioTraspasoSal = $('#folioTraspasoSal');
        const totalSal = $('#totalSal');
        const tablaPartidasSal = $('#tablaPartidasSal');
        const botonReporteSalida = $('#botonReporteSalida');
        const btnCargarExcel = $('#btnCargarExcel');
        const mdlCargarExcel = $('#mdlCargarExcel');
        const inputFileExcel = $('#inputFileExcel');
        const btnGuardarExcel = $('#btnGuardarExcel');
        //#endregion

        //#region Entrada
        const almDestinoNumEnt = $('#almDestinoNumEnt');
        const almDestinoDescEnt = $('#almDestinoDescEnt');
        const numMovEnt = $('#numMovEnt');
        const ccDestinoNumEnt = $('#ccDestinoNumEnt');
        const ccDestinoDescEnt = $('#ccDestinoDescEnt');
        const fechaMovEnt = $('#fechaMovEnt');
        const recibioNumEnt = $('#recibioNumEnt');
        const recibioDescEnt = $('#recibioDescEnt');
        const tipoMovEnt = $('#tipoMovEnt');
        const comentariosEnt = $('#comentariosEnt');
        const almOrigenNumEnt = $('#almOrigenNumEnt');
        const almOrigenDescEnt = $('#almOrigenDescEnt');
        const ccOrigenNumEnt = $('#ccOrigenNumEnt');
        const ccOrigenDescEnt = $('#ccOrigenDescEnt');
        const ordenTraspasoEnt = $('#ordenTraspasoEnt');
        const folioTraspasoEnt = $('#folioTraspasoEnt');
        const totalEnt = $('#totalEnt');
        const tablaPartidasEnt = $('#tablaPartidasEnt');
        const botonReporteEntrada = $('#botonReporteEntrada');
        //#endregion

        //#region partidas
        const btnAgregarInsumo = $('#btnAgregarInsumo');
        const btnQuitarInsumo = $('#btnQuitarInsumo');
        const btnGuardar = $('#btnGuardar');
        const btnGuardarEntrada = $('#btnGuardarEntrada');

        const tblPartidas = $('#tblPartidas');
        const tblPartidasEntrada = $('#tblPartidasEntrada');

        const mdlExistencias = $('#mdlExistencias');
        const tblExistencias = $('#tblExistencias');

        const mdlUbicacionDetalle = $('#mdlUbicacionDetalle');
        const btnAgregarUbicacion = $('#btnAgregarUbicacion');
        const btnQuitarUbicacion = $('#btnQuitarUbicacion');
        const tblUbicacion = $('#tblUbicacion');
        const btnGuardarUbicacion = $('#btnGuardarUbicacion');
        const mdlHistorialInsumo = $('#mdlHistorialInsumo');
        const tblHistorialInsumo = $('#tblHistorialInsumo');
        const mdlCatalogoUbicaciones = $('#mdlCatalogoUbicaciones');
        const tblCatalogoUbicaciones = $('#tblCatalogoUbicaciones');
        //#endregion

        const report = $("#report");
        //#endregion

        //#region partidas
        _filaInsumo = null;
        _countRenglonesEntradas = 0;
        //#endregion

        let dtPartidasSal;
        let dtPartidasEnt;

        (function init() {
            btnGuardarEntrada.attr('disabled', true);

            agregarListeners();
            initTablaPartidasSal();
            initTablaPartidasEnt();
            initTablePartidas();
            initTableExistencias();

            numMovSal.change(cargarSalidaTraspaso);
            numMovEnt.change(cargarEntradaTraspaso);
            //ordenTraspasoEnt.change(cargarSalidaParaEntrada);

            folioTraspasoEnt.change(cargarSalidaParaEntrada);
        })();

        //#region eventos
        btnGuardarEntrada.on('click', function () {
            let folio_traspaso = +(folioTraspasoEnt.val());

            let entradas = [];

            tblPartidasEntrada.find("tbody tr").each(function (idx, row) {
                let rowData = tblPartidasEntrada.DataTable().row(row).data();
                //let almacenDestinoID = $(row).find('.selectAlmacenDestinoNuevo').val();

                if (rowData) {
                    rowData.ordenTraspaso = 0;
                    rowData.comentarios = comentariosEnt.val();
                    rowData.numeroDestino = 0;
                    rowData.listUbicacionMovimiento = $(row).find('td .btnUbicacionDetalle').data('listUbicacionMovimiento');;
                    //rowData.almacenDestinoID = almacenDestinoID;

                    if (folio_traspaso > 0) {
                        rowData.cc = ccOrigenNumEnt.val();
                        rowData.ccDestino = ccDestinoNumEnt.val();
                    }

                    let cantidadEntrada = $(row).find('td .inputCantidadAutorizar').val();

                    if ((!isNaN(cantidadEntrada) && cantidadEntrada != '' && cantidadEntrada > 0)) {
                        entradas.push(rowData);
                    }
                }
            });

            if (entradas.length > 0) {
                // let flagMismoAlmacen = entradas.some(function (x) {
                //     return x.almacenOrigenID == x.almacenDestinoID;
                // });

                let almacenDestinoOriginal = +(almDestinoNumEnt.val())

                $.post('/Enkontrol/Requisicion/GuardarEntradas', { entradas, folio_traspaso, almacenDestinoOriginal }).then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        verReporteEntrada();
                    } else {
                        AlertaGeneral(`Alerta`, `No se ha guardado la información. ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            } else {
                AlertaGeneral(`Alerta`, `No se capturaron datos.`);
            }
        });

        btnGuardar.on('click', function () {
            btnGuardar.attr('disabled', true);

            let listaAutorizados = [];

            tblPartidas.find('tbody tr').each(function (idx, row) {
                let rowData = tblPartidas.DataTable().row($(row)).data();

                let cantidadAutorizar = $(row).find('.inputCantidad').getVal(2);

                if ((!isNaN(cantidadAutorizar) && cantidadAutorizar != '' && cantidadAutorizar > 0)) {
                    let ubicacion = [];

                    ubicacion.push({
                        insumo: rowData.insumo,
                        cantidad: rowData.cantidad,
                        area_alm: rowData.area_alm,
                        lado_alm: rowData.lado_alm,
                        estante_alm: rowData.estante_alm,
                        nivel_alm: rowData.nivel_alm,
                        cantidadMovimiento: rowData.cantidad
                    });

                    listaAutorizados.push({
                        folioInterno: 1,
                        folioInternoString: '1',
                        ccOrigen: ccOrigenNumSal.val(),
                        almacenOrigen: almOrigenNumSal.val(),
                        ccDestino: ccDestinoNumSal.val(),
                        almacenDestino: almDestinoNumSal.val(),
                        insumo: rowData.insumo,
                        comentariosGestion: comentariosSal.val(),
                        cantidad: rowData.cantidad,
                        cantidadTraspasar: rowData.cantidad,
                        checkBoxAutorizado: true,
                        checkBoxRechazado: false,
                        listUbicacionMovimiento: ubicacion
                    });
                }
            });

            if (listaAutorizados.length > 0) {
                $.post('/Enkontrol/Almacen/GuardarAutorizacionesTraspasosDirectos', { listaAutorizados, numReq: numMovSal.val() }).then(response => {
                    if (response.success) {
                        if (response.flagMaquinaStandBy) {
                            AlertaGeneral(`Alerta`, `Se ha guardado la información. Se quitó el estado "Stand-By" de la máquina. Folio: ${response.orden_ct}`);
                            ordenTraspasoSal.val(response.orden_ct);
                            folioTraspasoSal.val(response.orden_ct);
                            verReporteSalida();
                        } else {
                            AlertaGeneral(`Alerta`, `Se ha guardado la información. Folio: ${response.orden_ct}`);
                            ordenTraspasoSal.val(response.orden_ct);
                            folioTraspasoSal.val(response.orden_ct);
                            verReporteSalida();
                        }
                    } else {
                        AlertaGeneral(`Alerta`, `${response.message}`);
                        btnGuardar.attr('disabled', false);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    btnGuardar.attr('disabled', false);
                }
                );
            } else {
                AlertaGeneral('Alerta', 'No se ha capturado información');
                btnGuardar.attr('disabled', false);
            }
        });

        almOrigenNumSal.on('change', function () {
            let almacenID = almOrigenNumSal.val();

            botonReporteSalida.attr('disabled', true);

            prepararSalidaNueva(false);

            $.post('/Enkontrol/Almacen/GetNuevaSalidaConsultaTraspaso', { almacenID })
                .then(response => {
                    if (response.success) {
                        almOrigenDescSal.val(response.almacenDesc);
                        numMovSal.val(response.numeroDisponible);
                        entregoNumSal.val(response.entregoNum);
                        entregoDescSal.val(response.entregoDesc);
                        fechaMovSal.val(response.fecha);
                        totalSal.val('$' + formatMoney(0));
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        ccOrigenNumSal.on('change', function () {
            let cc = ccOrigenNumSal.val();

            if (/*cc != ccDestinoNumSal.val()*/true) {
                if (cc != '') {
                    $.blockUI({ message: 'Procesando...' });
                    $.post('/Enkontrol/Almacen/GetCentroCosto', { cc })
                        .always($.unblockUI)
                        .then(response => {
                            if (response.success) {
                                ccOrigenDescSal.val(response.ccDesc);
                            } else {
                                AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                } else {
                    ccOrigenNumSal.val('');
                    ccOrigenDescSal.val('');
                }
            } else {
                ccOrigenNumSal.val('');
                AlertaGeneral('Alerta', 'No es válido que el Centro de Costo Origen sea igual al Centro de Costo Destino');
            }
        });

        ccDestinoNumEnt.on('change', function () {
            let cc = ccDestinoNumEnt.val();

            if (/*cc != ccOrigenNumEnt.val()*/true) {
                if (cc != '') {
                    $.blockUI({ message: 'Procesando...' });
                    $.post('/Enkontrol/Almacen/GetCentroCosto', { cc })
                        .always($.unblockUI)
                        .then(response => {
                            if (response.success) {
                                ccDestinoDescEnt.val(response.ccDesc);
                                cargarSalidaParaEntrada();
                            } else {
                                AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                } else {
                    ccDestinoNumEnt.val('');
                    ccDestinoDescEnt.val('');
                }
            } else {
                ccDestinoNumEnt.val('');
                AlertaGeneral('Alerta', 'No es válido que el Centro de Costo Origen sea igual al Centro de Costo Destino');
            }
        });

        ccOrigenNumEnt.on('change', function () {
            let cc = ccOrigenNumEnt.val();

            if (/*cc != ccDestinoNumEnt.val()*/true) {
                if (cc != '') {
                    $.blockUI({ message: 'Procesando...' });
                    $.post('/Enkontrol/Almacen/GetCentroCosto', { cc })
                        .always($.unblockUI)
                        .then(response => {
                            if (response.success) {
                                ccOrigenDescEnt.val(response.ccDesc);
                                cargarSalidaParaEntrada();
                            } else {
                                AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                } else {
                    ccOrigenNumEnt.val('');
                    ccOrigenDescEnt.val('');
                }
            } else {
                ccOrigenNumEnt.val('');
                AlertaGeneral('Alerta', 'No es válido que el Centro de Costo Origen sea igual al Centro de Costo Destino');
            }
        });

        almDestinoNumSal.on('change', function () {
            let almacenID = almDestinoNumSal.val();

            if (checarVirtualesPrincipales(almOrigenNumSal.val(), almacenID)) {
                if (almacenID != almOrigenNumSal.val()) {
                    $.post('/Enkontrol/Almacen/GetNuevaSalidaConsultaTraspaso', { almacenID })
                        .then(response => {
                            if (response.success) {
                                almDestinoDescSal.val(response.almacenDesc);
                            } else {
                                AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                } else {
                    almDestinoNumSal.val('');
                    AlertaGeneral('Alerta', 'No es válido que el Almacén Origen sea igual al Almacén Destino');
                }
            } else {
                almDestinoNumSal.val('');
                AlertaGeneral('Alerta', 'No se pueden mezclar almacenes virtuales con principales');
            }
        });

        ccDestinoNumSal.on('change', function () {
            let cc = ccDestinoNumSal.val();

            if (/*cc != ccOrigenNumSal.val()*/true) {
                if (cc != '') {
                    $.blockUI({ message: 'Procesando...' });
                    $.post('/Enkontrol/Almacen/GetCentroCosto', { cc })
                        .always($.unblockUI)
                        .then(response => {
                            if (response.success) {
                                ccDestinoDescSal.val(response.ccDesc);
                            } else {
                                AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                } else {
                    ccDestinoNumSal.val('');
                    ccDestinoDescSal.val('');
                }
            } else {
                ccDestinoNumSal.val('');
                AlertaGeneral('Alerta', 'No es válido que el Centro de Costo Origen sea igual al Centro de Costo Destino');
            }
        });

        almDestinoNumEnt.on('change', function () {
            let almacenID = almDestinoNumEnt.val();

            botonReporteEntrada.attr('disabled', true);

            prepararEntradaNueva(false);

            $.post('/Enkontrol/Almacen/GetNuevaEntradaConsultaTraspaso', { almacenID })
                .then(response => {
                    if (response.success) {
                        almDestinoDescEnt.val(response.almacenDesc);
                        numMovEnt.val(response.numeroDisponible);
                        recibioNumEnt.val(response.recibioNum);
                        recibioDescEnt.val(response.recibioDesc);
                        fechaMovEnt.val(response.fecha);
                        totalEnt.val('$' + formatMoney(0));
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        almOrigenNumEnt.on('change', function () {
            let almacenID = almOrigenNumEnt.val();

            if (checarVirtualesPrincipales(almacenID, almDestinoNumEnt.val())) {
                if (almacenID != almDestinoNumEnt.val()) {
                    $.post('/Enkontrol/Almacen/GetNuevaSalidaConsultaTraspaso', { almacenID })
                        .then(response => {
                            if (response.success) {
                                almOrigenDescEnt.val(response.almacenDesc);
                                cargarSalidaParaEntrada();
                            } else {
                                AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                } else {
                    almOrigenNumEnt.val('');
                    AlertaGeneral('Alerta', 'No es válido que el Almacén Origen sea igual al Almacén Destino');
                }
            } else {
                almOrigenNumEnt.val('');
                AlertaGeneral('Alerta', 'No se pueden mezclar almacenes virtuales con principales');
            }
        });

        btnCargarExcel.on('click', function () {
            mdlCargarExcel.modal('show');
        });

        btnGuardarExcel.on('click', function () {
            try {
                var request = new XMLHttpRequest();

                $.blockUI({ message: 'Procesando...', baseZ: 2000 });

                request.open("POST", "/Enkontrol/Almacen/CargarExcelSalidaTraspaso");
                request.send(formData());

                request.onload = function (response) {
                    if (request.status == 200) {
                        $.unblockUI();

                        let respuesta = JSON.parse(request.response);

                        if (respuesta.success) {
                            mdlCargarExcel.modal('hide');
                            AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        } else {
                            AlertaGeneral(`Alerta`, `No se ha guardado la información. ${respuesta.message}`);
                        }
                    } else {
                        $.unblockUI();
                        AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                    }
                };
            } catch {
                $.unblockUI();
            }
        });

        function agregarListeners() {
            botonReporteSalida.click(verReporteSalida);
            botonReporteEntrada.click(verReporteEntrada);
        }

        //#region ubicaciones
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

            let cantidadEntrada = tblPartidasEntrada.DataTable().row(_filaInsumo).data().cantidad;

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
            let datosEntrada = tblPartidasEntrada.DataTable().row(_filaInsumo).data();

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
        //#endregion

        //#region partidas
        btnAgregarInsumo.on('click', function () {
            let datos = tblPartidas.DataTable().rows().data();

            datos.push({
                'partida': datos.length + 1,
                'insumo': '',
                'insumoDesc': '',
                'areaCuenta': '',
                'cantidad': 0,
                'unidad': '',
                'costo_prom': 0,
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

        tblPartidas.on('change', '.inputInsumo', function () {
            let row = $(this).closest('tr');
            let insumo = $(this).val();
            let almacen = almOrigenNumSal.val();

            if (insumo.length == 7) {
                checarFamilias();

                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Requisicion/GetInsumoInformacionByAlmacen', { insumo, almacen })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            let ins = response.data;

                            row.find('.inputInsumo').val(ins.value);
                            row.find('.inputInsumoDesc').val(ins.id);
                            row.find('td:eq(5)').text(ins.unidad);
                            row.find('td:eq(6)').text(ins.costoPromedio);

                            let rowData = tblPartidas.DataTable().row(row).data();

                            let insumoNumero = $(row).find('.inputInsumo').val();
                            let insumoDesc = $(row).find('.inputInsumoDesc').val();
                            let nuevaCantidad = !isNaN($(row).find('.inputCantidad').val()) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;
                            let unidad = $(row).find('td:eq(5)').text();
                            let costo_promedio = unmaskNumero($(row).find('td:eq(6)').text());
                            let nuevoImporte = nuevaCantidad * costo_promedio;

                            rowData.insumo = insumoNumero;
                            rowData.insumoDesc = insumoDesc;
                            rowData.cantidad = nuevaCantidad;
                            rowData.unidad = unidad;
                            rowData.costo_prom = costo_promedio;
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
        //#endregion
        //#endregion

        //#region tablas
        function initTablaPartidasSal() {
            dtPartidasSal = tablaPartidasSal.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                // scrollY: '45vh',
                // scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'partida', title: 'Pda.' },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Descripción' },
                    {
                        title: 'Área-Cuenta', render: function (data, type, row, meta) {
                            return `${row.area}-${row.cuenta}`;
                        }
                    },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        data: 'precio', title: 'Costo Promedio', render: function (data, type, row, meta) {
                            return row.precio != null ? '$' + formatMoney(row.precio) : '$' + formatMoney(0); //Se pone el precio porque en Enkontrol muestra el precio pero con nombre de "Costo Promedio".
                        }
                    },
                    {
                        data: 'importe', title: 'Importe', render: function (data, type, row, meta) {
                            return row.importe != null ? '$' + formatMoney(row.importe) : '$' + formatMoney(0);
                        }
                    },
                    { data: 'area_alm', title: 'Área Alm.' },
                    { data: 'lado_alm', title: 'Lado Área' },
                    { data: 'estante_alm', title: 'Estante Lado' },
                    { data: 'nivel_alm', title: 'Nivel Estante' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaPartidasEnt() {
            dtPartidasEnt = tablaPartidasEnt.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                // scrollY: '45vh',
                // scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'partida', title: 'Pda.' },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Descripción' },
                    {
                        title: 'Área-Cuenta', render: function (data, type, row, meta) {
                            return `${row.area}-${row.cuenta}`;
                        }
                    },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        data: 'precio', title: 'Precio Unitario', render: function (data, type, row, meta) {
                            return row.precio != null ? '$' + formatMoney(row.precio) : '$' + formatMoney(0);
                        }
                    },
                    {
                        data: 'importe', title: 'Importe', render: function (data, type, row, meta) {
                            return row.importe != null ? '$' + formatMoney(row.importe) : '$' + formatMoney(0);
                        }
                    },
                    { data: 'area_alm', title: 'Área Alm.' },
                    { data: 'lado_alm', title: 'Lado Área' },
                    { data: 'estante_alm', title: 'Estante Lado' },
                    { data: 'nivel_alm', title: 'Nivel Estante' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
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

                    tblPartidas.on('change', '.inputCantidad', function () {
                        tblPartidas.find('tbody tr').each(function (index, row) {
                            let rowData = tblPartidas.DataTable().row(row).data();

                            let insumo = $(row).find('.inputInsumo').val();
                            let insumoDesc = $(row).find('.inputInsumoDesc').val();
                            let nuevaCantidad = !isNaN(unmaskNumero($(row).find('.inputCantidad').val())) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;
                            let unidad = $(row).find('td:eq(4)').text();
                            let nuevoImporte = nuevaCantidad * rowData.costo_prom;
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

                    tblPartidas.on('change', '.selectAreaCuenta', function () {
                        checarPermisoAreaCuenta(tblPartidas); //Áreas cuenta 14-1 y 14-2.

                        let row = $(this).closest('tr');
                        $(row).find('.inputCantidad').change();
                    });
                },
                createdRow: function (row, rowData) {
                    let inputInsumo = $(row).find('.inputInsumo');
                    let inputInsumoDesc = $(row).find('.inputInsumoDesc');

                    inputInsumo.getAutocomplete(setInsumoDesc, { cc: ccOrigenNumSal.val(), almacen: almOrigenNumSal.val() }, '/Enkontrol/Requisicion/getInsumosByAlmacen');
                    inputInsumoDesc.getAutocomplete(setInsumoBusqPorDesc, { cc: ccOrigenNumSal.val(), almacen: almOrigenNumSal.val() }, '/Enkontrol/Requisicion/getInsumosDescByAlmacen');

                    let selectAreaCuenta = $(row).find('.selectAreaCuenta');

                    selectAreaCuenta.fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: ccOrigenNumSal.val() }, false, "000-000");
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
                        },
                        visible: false
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
                        title: 'Existencias', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-default btnExistencias"><i class="fa fa-bars"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });

            tblPartidasEntrada.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                rowId: 'id',
                bInfo: false,
                initComplete: function (settings, json) {
                    tblPartidasEntrada.on('change', '.inputCantidadAutorizar', function () {
                        let total_entrada = 0;
                        tblPartidasEntrada.find("tbody tr").each(function (idx, row) {
                            let rowData = tblPartidasEntrada.DataTable().row(row).data();
                            total_entrada += (rowData.cantidad * rowData.precio);

                            let area = $(row).find('.selectAreaCuenta option:selected').val();
                            let cuenta = $(row).find('.selectAreaCuenta option:selected').attr('data-prefijo');
                        });
                        totalEnt.val('$' + formatMoney(total_entrada));
                    });

                    tblPartidasEntrada.on('click', 'input[type="checkbox"]', function () {
                        if ($(this).prop('checked')) {
                            tblPartidasEntrada.DataTable().row($(this).closest('tr')).data().checkbox = true;

                            _countRenglonesEntradas++;
                        } else {
                            tblPartidasEntrada.DataTable().row($(this).closest('tr')).data().checkbox = false;

                            _countRenglonesEntradas--;
                        }
                    });

                    tblPartidasEntrada.on('click', '.btnUbicacionDetalle', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblPartidasEntrada.DataTable().row(row).data();

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

                    tblPartidasEntrada.on('change', '.selectAreaCuenta', function () {
                        checarPermisoAreaCuenta(tblPartidasEntrada); //Áreas cuenta 14-1 y 14-2.

                        let row = $(this).closest('tr');
                        $(row).find('.inputCantidadAutorizar').change();
                    });
                },
                createdRow: function (row, rowData) {
                    // $(row).find('.selectAlmacenDestinoNuevo').fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, null);
                    // $(row).find('.selectAlmacenDestinoNuevo').val(rowData.almacenDestinoID);

                    let selectAreaCuenta = $(row).find('.selectAreaCuenta');

                    selectAreaCuenta.fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: ccOrigenNumSal.val() }, false, "000-000");
                    selectAreaCuenta.find('option[value=' + rowData.area + '][data-prefijo=' + rowData.cuenta + ']').attr('selected', true);
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
                        data: 'areaCuenta', title: 'Área-Cuenta', render: function (data, type, row, meta) {
                            let select = document.createElement('select');

                            select.classList.add('form-control', 'text-center', 'selectAreaCuenta');

                            return select.outerHTML;
                        },
                        visible: false
                    },
                    {
                        data: 'almacenDestinoDesc', title: 'Almacén Destino', render: function (data, type, row, meta) {
                            let select = document.createElement('select');

                            select.classList.add('form-control');
                            select.classList.add('selectAlmacenDestinoNuevo');
                            //select.style.height = '22px';

                            return select.outerHTML;
                        },
                        visible: false
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
                        title: 'Comentarios',
                        visible: false
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
        //#endregion

        //#region server
        function cargarSalidaTraspaso() {
            let almacenID = almOrigenNumSal.val();
            let numero = numMovSal.val();

            limpiarSalida();
            botonReporteSalida.attr('disabled', true);
            btnGuardarEntrada.attr('disabled', true);

            if ((almacenID != '' && !isNaN(almacenID)) && (numero != '' && !isNaN(numero))) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GetSalidaConsultaTraspasoDirecto', { almacenID, numero })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            if (response.noExiste) {
                                prepararSalidaNueva(false);
                                btnGuardarEntrada.attr('disabled', false);
                            } else {
                                prepararSalidaNueva(true);
                                llenarSalida(response.data);
                                botonReporteSalida.attr('disabled', false);
                            }
                        } else {
                            AlertaGeneral(`Alerta`, `${response.message}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                limpiarSalida();
            }
        }

        function cargarEntradaTraspaso() {
            let almacenID = almDestinoNumEnt.val();
            let numero = numMovEnt.val();

            limpiarEntrada();
            botonReporteEntrada.attr('disabled', true);

            if ((almacenID != '' && !isNaN(almacenID)) && (numero != '' && !isNaN(numero))) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GetEntradaConsultaTraspasoDirecto', { almacenID, numero })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            if (response.noExiste) {
                                prepararEntradaNueva(false);
                            } else {
                                prepararEntradaNueva(true);
                                llenarEntrada(response.data);
                                AddRows(tablaPartidasEnt, response.data.detalle);
                                botonReporteEntrada.attr('disabled', false);
                            }
                        } else {
                            AlertaGeneral(`Alerta`, `${response.message}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                limpiarEntrada();
            }
        }

        function cargarSalidaParaEntrada() {
            btnGuardarEntrada.attr('disabled', true);

            if (almDestinoNumEnt.val() && numMovEnt.val() && ccDestinoNumEnt.val() && almOrigenNumEnt.val() && ccOrigenNumEnt.val() && folioTraspasoEnt.val()) {
                let almacenOrigen = +(almOrigenNumEnt.val());
                let centroCostoOrigen = ccOrigenNumEnt.val();
                let almacenDestino = +(almDestinoNumEnt.val());
                let centroCostoDestino = ccDestinoNumEnt.val();
                let folioTraspaso = +(folioTraspasoEnt.val());

                ordenTraspasoEnt.val(folioTraspasoEnt.val());

                $.post('/Enkontrol/Requisicion/GetSalidaTraspaso', { almacenOrigen, centroCostoOrigen, almacenDestino, centroCostoDestino, folioTraspaso }).then(response => {
                    if (response.success) {
                        AddRows(tblPartidasEntrada, response.data);
                        btnGuardarEntrada.attr('disabled', false);
                    } else {
                        AlertaGeneral(`Alerta`, `${response.message}`);
                        //limpiarVista();
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    limpiarVista();
                }
                );
            }
        }

        //#region partidas
        function checarPermisosFamilias(almacen, insumos) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Almacen/ChecarPermisosFamilias', { almacen, insumos })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        if (!response.flagPermiso) {
                            AlertaGeneral(`Alerta`, `No se puede dar salida a los insumos de la familia 101 y 102 en el almacén ${almacen}.`);
                        }
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            // AlertaGeneral(`Alerta`, `No se puede dar salida a las familias de insumos "101" y "102".`);
        }

        function cargarExistencias() {
            if (_filaInsumo != null) {
                let rowData = tblPartidas.DataTable().row(_filaInsumo).data();
                let cc = ccOrigenNumSal.val();
                let almacenID = almOrigenNumSal.val();
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

        function checarPermisoAreaCuenta(tabla) {
            let listaAreasCuenta = [];

            tabla.find('tbody tr').each((index, row) => {
                let rowData = tabla.DataTable().row(row).data();

                if (rowData != undefined) {
                    let area = $(row).find('.selectAreaCuenta option:selected').val();
                    let cuenta = $(row).find('.selectAreaCuenta option:selected').attr('data-prefijo');

                    listaAreasCuenta.push({
                        area: area,
                        cuenta: cuenta
                    });
                }
            });

            let flagPermiso_14_1_14_2 = listaAreasCuenta.some(function (x) {
                return (x.area == 14 && x.cuenta == 1) || (x.area == 14 && x.cuenta == 2)
            });

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
        //#endregion
        //#endregion

        //#region ubicaciones
        function cargarHistorialInsumo() {
            if (_filaInsumo != null) {
                let rowData = tblPartidasEntrada.DataTable().row(_filaInsumo).data();
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
                //let almacenID = $(_filaInsumo).find('.selectAlmacenDestinoNuevo').val(); // let almacenID = rowData.almacenDestinoID;
                let almacenID = almDestinoNumEnt.val();

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
        //#endregion

        //#region partidas
        function recargarAutoComplete(row) {
            let rowData = tblPartidas.DataTable().row(row).data();
            let inputInsumo = $(row).find('.inputInsumo');
            let inputInsumoDesc = $(row).find('.inputInsumoDesc');

            inputInsumo.getAutocomplete(setInsumoDesc, { cc: ccOrigenNumSal.val(), almacen: almOrigenNumSal.val() }, '/Enkontrol/Requisicion/getInsumosByAlmacen');
            inputInsumoDesc.getAutocomplete(setInsumoBusqPorDesc, { cc: ccOrigenNumSal.val(), almacen: almOrigenNumSal.val() }, '/Enkontrol/Requisicion/getInsumosDescByAlmacen');

            let selectAreaCuenta = $(row).find('.selectAreaCuenta');

            selectAreaCuenta.fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: ccOrigenNumSal.val() }, false, "000-000");
            selectAreaCuenta.find('option[value=' + rowData.area + '][data-prefijo=' + rowData.cuenta + ']').attr('selected', true);
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
            let nuevoImporte = nuevaCantidad * ui.item.costoPromedio;

            rowData.insumo = insumo;
            rowData.insumoDesc = insumoDesc;
            rowData.cantidad = nuevaCantidad;
            rowData.unidad = unidad;
            rowData.costo_prom = ui.item.costoPromedio;
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

            let insumo = ui.item.id; // let insumo = $(row).find('.inputInsumo').val();
            let insumoDesc = $(row).find('.inputInsumoDesc').val();
            let nuevaCantidad = !isNaN($(row).find('.inputCantidad').val()) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;
            let unidad = $(row).find('td:eq(4)').text();
            let nuevoImporte = nuevaCantidad * ui.item.costoPromedio;

            rowData.insumo = insumo;
            rowData.insumoDesc = insumoDesc;
            rowData.cantidad = nuevaCantidad;
            rowData.unidad = unidad;
            rowData.costo_prom = ui.item.costoPromedio;
            rowData.importe = nuevoImporte;

            tblPartidas.DataTable().row(row).data(rowData).draw();

            recargarAutoComplete(row);

            row.find('.inputInsumo').val(ui.item.id);

            //Retornar falso para que no se ejecute la función de select por default de JQuery UI.
            return false;
        }

        function checarFamilias() {
            let almacen = +(almOrigenNumSal.val());
            let insumos = [];

            if (almacen < 600 || almacen > 620) {
                tblPartidas.find('tbody tr').each(function (idx, row) {
                    if ($(row).find('.inputInsumo').length > 0) {
                        let insumo = $(row).find('.inputInsumo').val();

                        if (insumo.length == 7) {
                            insumos.push(+(insumo));
                        }
                    }
                });

                if (insumos.length > 0) {
                    checarPermisosFamilias(almacen, insumos);
                }
            }
        }

        function checarVirtualesPrincipales(almacenOrigen, almacenDestino) {
            let permitido = true;

            if ((almacenOrigen < 900 || almacenOrigen == 910) && (almacenDestino >= 900 && almacenDestino != 910)) {
                permitido = false;
            }
            if ((almacenOrigen >= 900 && almacenOrigen != 910) && (almacenDestino < 900 || almacenDestino == 910)) {
                permitido = false;
            }

            return permitido;
        }

        function calcularTotal() {
            let datos = tblPartidas.DataTable().rows().data();
            let total = 0;

            datos.toArray().forEach(function (element) {
                total += element.importe;
            });

            totalSal.val('$' + formatMoney(total));
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

                    let costo_prom = rowData.costo_prom;
                    let importe = rowData.importe;

                    let area_alm = $(row).find('.inputArea').val();
                    let lado_alm = $(row).find('.inputLado').val();
                    let estante_alm = $(row).find('.inputEstante').val();
                    let nivel_alm = $(row).find('.inputNivel').val();

                    let partida_oc = rowData.partida_oc != null ? rowData.partida_oc : 0;

                    if (insumo != '' && !isNaN(insumo)) {
                        detalle.push({
                            almacen: almOrigenNumSal.val(),
                            tipo_mov: 51,
                            numero: numMovSal.val(), //Posible número disponible
                            partida: partida,
                            insumo: insumo,
                            comentarios: '',
                            area: area,
                            cuenta: cuenta,
                            cantidad: cantidad,
                            precio: costo_prom,
                            costo_prom: costo_prom,
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
        //#endregion

        function llenarSalida(data) {
            almOrigenNumSal.val(data.almacen);
            almOrigenDescSal.val(data.almacenDesc);
            numMovSal.val(data.numero);
            ccOrigenNumSal.val(data.cc);
            ccOrigenDescSal.val(data.ccDesc);
            fechaMovSal.val(data.fechaString);
            entregoNumSal.val(data.empleado);
            entregoDescSal.val(data.empleadoDesc);
            tipoMovSal.val(52);
            comentariosSal.val(data.comentarios);
            almDestinoNumSal.val(data.alm_destino);
            almDestinoDescSal.val(data.alm_destinoDesc);
            ccDestinoNumSal.val(data.cc_destino);
            ccDestinoDescSal.val(data.cc_destinoDesc);
            ordenTraspasoSal.val(data.orden_ct);
            folioTraspasoSal.val(data.folio_traspaso);
            totalSal.val('$' + formatMoney(data.total));

            AddRows(tablaPartidasSal, data.detalle);
            AddRows(tblPartidas, data.detalle);
            //ToDo: llenar tabla partidas salidas.
        }

        function prepararSalidaNueva(bloquear) {
            if (bloquear) {
                ccOrigenNumSal.attr('disabled', true);
                comentariosSal.attr('disabled', true);
                almDestinoNumSal.attr('disabled', true);
                ccDestinoNumSal.attr('disabled', true);

                btnAgregarInsumo.attr('disabled', true);
                btnQuitarInsumo.attr('disabled', true);
                btnGuardar.attr('disabled', true);
            } else {
                ccOrigenNumSal.attr('disabled', false);
                comentariosSal.attr('disabled', false);
                almDestinoNumSal.attr('disabled', false);
                ccDestinoNumSal.attr('disabled', false);

                btnAgregarInsumo.attr('disabled', false);
                btnQuitarInsumo.attr('disabled', false);
                btnGuardar.attr('disabled', false);
            }
        }

        function prepararEntradaNueva(bloquear) {
            if (bloquear) {
                ccOrigenNumEnt.attr('disabled', true);
                comentariosEnt.attr('disabled', true);
                ccDestinoNumEnt.attr('disabled', true);
                almOrigenNumEnt.attr('disabled', true);
                ordenTraspasoEnt.attr('disabled', true);
                folioTraspasoEnt.attr('disabled', true);

                btnAgregarInsumo.attr('disabled', true);
                btnQuitarInsumo.attr('disabled', true);
                btnGuardarEntrada.attr('disabled', true);

                tablaPartidasEnt.show();
                tblPartidasEntrada.hide();
            } else {
                ccOrigenNumEnt.attr('disabled', false);
                comentariosEnt.attr('disabled', false);
                ccDestinoNumEnt.attr('disabled', false);
                almOrigenNumEnt.attr('disabled', false);
                ordenTraspasoEnt.attr('disabled', true);
                folioTraspasoEnt.attr('disabled', false);

                btnAgregarInsumo.attr('disabled', false);
                btnQuitarInsumo.attr('disabled', false);
                btnGuardarEntrada.attr('disabled', true);

                tablaPartidasEnt.hide();
                tblPartidasEntrada.show();
            }
        }

        function llenarEntrada(data) {
            almDestinoNumEnt.val(data.almacen); //Campos volteados en la tabla de Enkontrol.
            almDestinoDescEnt.val(data.almacenDesc); //Campos volteados en la tabla de Enkontrol.
            numMovEnt.val(data.numero);
            ccDestinoNumEnt.val(data.cc); //Campos volteados en la tabla de Enkontrol.
            ccDestinoDescEnt.val(data.ccDesc); //Campos volteados en la tabla de Enkontrol.
            fechaMovEnt.val(data.fechaString);
            recibioNumEnt.val(data.empleado);
            recibioDescEnt.val(data.empleadoDesc);
            tipoMovEnt.val(2);
            comentariosEnt.val(data.comentarios);
            almOrigenNumEnt.val(data.alm_destino); //Campos volteados en la tabla de Enkontrol.
            almOrigenDescEnt.val(data.alm_destinoDesc); //Campos volteados en la tabla de Enkontrol.
            ccOrigenNumEnt.val(data.cc_destino); //Campos volteados en la tabla de Enkontrol.
            ccOrigenDescEnt.val(data.cc_destinoDesc); //Campos volteados en la tabla de Enkontrol.
            ordenTraspasoEnt.val(data.orden_ct);
            folioTraspasoEnt.val(data.folio_traspaso);
            totalEnt.val('$' + formatMoney(data.total));
        }

        function limpiarSalida() {
            //almOrigenNumSal.val('');
            //almOrigenDescSal.val('');
            //numMovSal.val('');
            ccOrigenNumSal.val('');
            ccOrigenDescSal.val('');
            fechaMovSal.val('');
            entregoNumSal.val('');
            entregoDescSal.val('');
            tipoMovSal.val('');
            comentariosSal.val('');
            almDestinoNumSal.val('');
            almDestinoDescSal.val('');
            ccDestinoNumSal.val('');
            ccDestinoDescSal.val('');
            ordenTraspasoSal.val('');
            folioTraspasoSal.val('');
            totalSal.val('');

            dtPartidasSal.clear().draw();
            tblPartidas.DataTable().clear().draw();
        }

        function limpiarEntrada() {
            almDestinoNumEnt.val('');
            almDestinoDescEnt.val('');
            numMovEnt.val('');
            ccDestinoNumEnt.val('');
            ccDestinoDescEnt.val('');
            fechaMovEnt.val('');
            recibioNumEnt.val('');
            recibioDescEnt.val('');
            tipoMovEnt.val('');
            comentariosEnt.val('');
            almOrigenNumEnt.val('');
            almOrigenDescEnt.val('');
            ccOrigenNumEnt.val('');
            ccOrigenDescEnt.val('');
            ordenTraspasoEnt.val('');
            folioTraspasoEnt.val('');
            totalEnt.val('');

            dtPartidasEnt.clear().draw();
        }

        function verReporteSalida() {
            report.attr("src", `/Reportes/Vista.aspx?idReporte=128`);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function verReporteEntrada() {
            report.attr("src", `/Reportes/Vista.aspx?idReporte=129`);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        function formatMoney(amount, decimalCount = 2, decimal = ".", thousands = ",") {
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

        function formData() {
            let formData = new FormData();

            $.each(document.getElementById("inputFileExcel").files, function (i, file) {
                formData.append("files[]", file);
            });

            return formData;
        }
    }
    $(document).ready(() => Enkontrol.Almacen.Almacen.TraspasoDirecto = new TraspasoDirecto())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();