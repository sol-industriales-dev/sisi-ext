(() => {
    $.namespace('Enkontrol.Compras.OrdenCompra.EntradaNoInventariable');

    EntradaNoInventariable = function () {
        //#region Selectores
        const tblPartidas = $('#tblPartidas');
        const textAreaDescPartida = $('#textAreaDescPartida');
        const btnGuardarSurtido = $('#btnGuardarSurtido');

        //#region Panel Izquierdo
        const selectCC = $('#selectCC');
        const inputNumero = $('#inputNumero');
        const inputNumeroReq = $('#inputNumeroReq');
        const selectBoS = $('#selectBoS');
        const dtpFecha = $('#dtpFecha');
        const inputProvNum = $('#inputProvNum');
        const inputProvNom = $('#inputProvNom');
        const inputCompNum = $('#inputCompNum');
        const inputCompNom = $('#inputCompNom');
        const inputSolNum = $('#inputSolNum');
        const inputSolNom = $('#inputSolNom');
        const inputAutNum = $('#inputAutNum');
        const inputAutNom = $('#inputAutNom');
        const inputEmb = $('#inputEmb');
        // const selectLab = $('#selectLab');
        const inputConFact = $('#inputConFact');
        const checkAutoRecep = $('#checkAutoRecep');
        const inputAlmNum = $('#inputAlmNum');
        const inputAlmNom = $('#inputAlmNom');
        const inputEmpNum = $('#inputEmpNum');
        const inputEmpNom = $('#inputEmpNom');
        const inputOrdenTraspaso = $('#inputOrdenTraspaso');
        const inputComentarios = $('#inputComentarios');
        const inputAlmacenMovimiento = $('#inputAlmacenMovimiento');
        const inputRemision = $('#inputRemision');
        //#endregion

        //#region Panel Derecho
        const selectTipoOC = $('#selectTipoOC');
        const selectMoneda = $('#selectMoneda');
        const inputTipoCambio = $('#inputTipoCambio');
        const inputSubTotal = $('#inputSubTotal');
        const inputIVAPorcentaje = $('#inputIVAPorcentaje');
        const inputIVANumero = $('#inputIVANumero');
        const inputRetencion = $('#inputRetencion');
        const inputTotal = $('#inputTotal');
        const inputTotalFinal = $('#inputTotalFinal');
        const btnGlobal = $('#btnGlobal');
        const report = $("#report");
        const inputNumMovimiento = $('#inputNumMovimiento');
        const botonReporte = $('#botonReporte');
        const inputGuiaPrefijo = $('#inputGuiaPrefijo');
        const inputGuiaFolio = $('#inputGuiaFolio');
        const selectTipoDocumento = $('#selectTipoDocumento');
        const inputFolioDocumento = $('#inputFolioDocumento');
        //#endregion

        //#region Elementos Ocultos
        const inputCompradorSesionNum = $('#inputCompradorSesionNum');
        const inputCompradorSesionNom = $('#inputCompradorSesionNom');
        const dtpInicio = $('#dtpInicio');
        const dtpFin = $('#dtpFin');
        //#endregion

        //#region CONST ELEMENTOS OCULTOS
        const inputEmpresaActual = $('#inputEmpresaActual');
        _empresaActual = +inputEmpresaActual.val();
        //#endregion

        //#endregion

        (function init() {
            verificarComprasPeru();
            let hoy = new Date();

            dtpInicio.datepicker().datepicker("setDate", new Date(hoy.getFullYear(), 0, 1));
            dtpFin.datepicker().datepicker("setDate", new Date(hoy.getFullYear(), 11, 31));
            dtpFecha.datepicker().datepicker();

            isComprador();
            initCbo();

            initTablePartidas();

            btnGlobal.click(aplicarGlobal);

            if (_empresaActual == 6) {
                $("#selectFormaPago").fillCombo('/Enkontrol/OrdenCompra/FillComboFormaPagoPeru', null, false, null);
                tblPartidas.DataTable().column(2).visible(false);
                selectTipoDocumento.fillCombo('/Enkontrol/OrdenCompra/FillComboTipoDocumentoPeru', null, false, null);
            } else {
                $('.elementoPeru').hide();
            }
        })();

        const getProveedorInfo = (num) => $.post('/Enkontrol/OrdenCompra/GetProveedorInfo', { num: num });

        $('#selectCC, #inputNumero').on('change', function () {
            if (selectCC.val() != '' && inputNumero.val() != '') {
                cargarCompra();
            }
        });

        $('#inputAlmacenMovimiento, #inputRemision').on('change', function () {
            if (!isNaN(inputAlmacenMovimiento.val()) && inputAlmacenMovimiento.val() > 0 && !isNaN(inputRemision.val()) && inputRemision.val() > 0) {
                cargarMovimiento();
            }
        });

        inputIVAPorcentaje.on('change', function () {
            let valor = $(this).val();

            if (!isNaN(valor)) {
                let iva = parseFloat(valor);
                let subTotal = unmaskNumero(inputSubTotal.val());

                inputIVANumero.val(maskNumero((subTotal * iva) / 100));
                inputTotal.val(maskNumero(subTotal + (unmaskNumero(inputIVANumero.val()))));
            } else {
                $(this).val(16);
                inputIVAPorcentaje.change();
            }
        });

        tblPartidas.on('change', '.inputSurtir', function () {
            tblPartidas.find("tbody tr").each(function (idx, row) {
                let valor = $(row).find(".inputSurtir").val();

                if (valor > 0) {
                    flagSurtir = true;
                }
            });
        });

        btnGuardarSurtido.on('click', function () {
            let compra = {
                cc: selectCC.val(),
                numero: inputNumero.val(),
                ordenTraspaso: inputOrdenTraspaso.val(),
                proveedor: inputProvNum.val(),
                comentarios: inputComentarios.val(),
                almacen: inputAlmNum.val(),
                PERU_guiaCompraPrefijo: inputGuiaPrefijo.val(),
                PERU_guiaCompraFolio: inputGuiaFolio.val(),
                PERU_tipoDocumento: selectTipoDocumento.val(),
                PERU_folioDocumento: inputFolioDocumento.val(),
                PERU_tipoCompra: 'RS'
            }

            let flagSurtidoMayor = false;
            let surtido = [];

            tblPartidas.find("tbody tr").each(function (idx, row) {
                let rowData = tblPartidas.DataTable().row(row).data();
                let valorSurtir = $(row).find(".inputValorSurtir").val();


                if ((!isNaN(valorSurtir) && valorSurtir != '' && valorSurtir > 0)) {
                    if (valorSurtir > rowData.cantidadPendiente) {
                        flagSurtidoMayor = true;
                    }

                    surtido.push({
                        partida: rowData.partida,
                        insumo: rowData.insumo,
                        cantidad: rowData.cantidad,
                        cantidadMovimiento: rowData.cantidad,
                        surtido: rowData.surtido,
                        aSurtir: valorSurtir
                    });
                }
            });

            if (flagSurtidoMayor) {
                AlertaGeneral(`Alerta`, `No se puede surtir una cantidad mayor a lo pendiente.`);

                return;
            }

            if (surtido.length > 0) {
                if (_empresaActual != 6) {
                    btnGuardarSurtido.attr('disabled', true);

                    $.post('/Enkontrol/OrdenCompra/GuardarSurtidoNoInventariable', { compra, surtido }).then(response => {
                        if (response.success) {
                            // btnGuardarSurtido.attr('disabled', false);
                            AlertaGeneral('Alerta', 'Se ha guardado la información.');
                            limpiarInformacion();
                            limpiarTabla(tblPartidas);
                            verReporte();
                        } else {
                            AlertaGeneral('Alerta', `Error al guardar la información. ${response.message.length > 0 ? response.message : ``}`);
                            inputNumero.change();
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    });
                } else {
                    if (inputGuiaPrefijo.val() != '' && inputGuiaFolio.val() != '') {
                        btnGuardarSurtido.attr('disabled', true);

                        $.post('/Enkontrol/OrdenCompra/GuardarSurtidoNoInventariable', { compra, surtido }).then(response => {
                            if (response.success) {
                                // btnGuardarSurtido.attr('disabled', false);
                                AlertaGeneral('Alerta', 'Se ha guardado la información.');
                                limpiarInformacion();
                                limpiarTabla(tblPartidas);
                                verReporte();
                            } else {
                                AlertaGeneral('Alerta', `Error al guardar la información. ${response.message.length > 0 ? response.message : ``}`);
                                inputNumero.change();
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        });
                    } else {
                        Alert2AccionConfirmar('Atención', 'No ha capturado la guía de compra. ¿Desea guardar el surtido de la compra?', 'Confirmar', 'Cancelar', () => {
                            btnGuardarSurtido.attr('disabled', true);

                            $.post('/Enkontrol/OrdenCompra/GuardarSurtidoNoInventariable', { compra, surtido }).then(response => {
                                if (response.success) {
                                    // btnGuardarSurtido.attr('disabled', false);
                                    AlertaGeneral('Alerta', 'Se ha guardado la información.');
                                    limpiarInformacion();
                                    limpiarTabla(tblPartidas);
                                    verReporte();
                                } else {
                                    AlertaGeneral('Alerta', `Error al guardar la información. ${response.message.length > 0 ? response.message : ``}`);
                                    inputNumero.change();
                                }
                            }, error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            });
                        });
                    }
                }
            }
        });

        botonReporte.on('click', function () {
            const cc = selectCC.val();
            const num = inputNumero.val();
            const numMovimiento = inputNumMovimiento.val();

            if (cc == "" || numMovimiento == "") {
                AlertaGeneral(`Aviso`, `Debe seleccionar un cc e ingresar un número de movimiento con información de compra.`);
                return;
            }

            $.get('/Enkontrol/OrdenCompra/GetDatosReporteEntradaNoInvOC', { cc, num, numMovimiento })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        verReporte();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        function isComprador() {
            $.post('/Enkontrol/OrdenCompra/getComprador').then(response => {
                if (response.success) {
                    inputCompradorSesionNum.val(response.comprador.comprador);
                    inputCompradorSesionNom.val(response.comprador.emplNom);
                } else {
                    AlertaGeneral("Aviso", "No eres comprador.");
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function initTablePartidas() {
            tblPartidas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                aaSorting: [0, 'asc'],
                rowId: 'id',
                scrollY: "250px",
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblPartidas.on('click', 'td', function () {
                        let rowData = tblPartidas.DataTable().row($(this).closest('tr')).data();

                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblPartidas.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");
                            textAreaDescPartida.val(rowData.partidaDescripcion);
                        } else {
                            textAreaDescPartida.val('');
                        }
                    });

                    tblPartidas.on('focus', 'input', function () {
                        $(this).select();
                    });
                },
                columns: [
                    { data: 'partida', title: 'Partida' },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Descripción' },
                    {
                        title: 'Folio Rem.', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center inputFolioRemision">`;
                        }
                    },
                    { data: 'area', title: 'Área' },
                    { data: 'cuenta', title: 'Cuenta' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'cant_recibida', title: 'Cant. Recibida' },
                    { data: 'cantidadPendiente', title: 'Cant. Pendiente' },
                    {
                        title: 'Recibido', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center inputValorSurtir" value="">`;
                        }
                    },
                    { data: 'unidad', title: 'Unidad' },
                    { data: 'precio', title: 'Precio' },
                    { data: 'monedaDesc', title: 'Moneda' },
                    { data: 'importe', title: 'Importe' }
                ],
                columnDefs: [
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return maskNumero(data);
                            }
                        },
                        targets: [11, 13]
                    },
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initCbo() {
            // selectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCcComCompradorModalEditar', null, false);
            selectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCc', null, false);
            // selectLab.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false);
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function llenarInformacion(data) {
            if ($('#inputEmpresaActual').val() == 6) {
                inputProvNum.val(data.PERU_proveedor);
            } else {
                inputProvNum.val(data.proveedor);
            }

            getProveedorInfo(inputProvNum.val()).done(function (response) {
                if (response != null) {
                    if (response.cancelado != 'C') {
                        inputProvNom.val(response.id);
                        $("#selectMoneda").val(response.moneda);
                        if (response.listaCuentasCorrientes != null && response.listaCuentasCorrientes.length > 0) {
                            $("#selectCuentaCorriente").empty();
                            $("#selectCuentaCorriente").append(`<option value="">--Seleccione--</option>`);

                            response.listaCuentasCorrientes.forEach((element) => {
                                $("#selectCuentaCorriente").append(`<option value="${element.Value}">${element.Text}</option>`);
                            });

                            if (data.PERU_cuentaCorriente != null && data.PERU_cuentaCorriente != '') {
                                $("#selectCuentaCorriente").find(`option:contains('${data.PERU_cuentaCorriente}')`).attr('selected', true);
                            }
                        }

                        if (data.PERU_formaPago != null && data.PERU_formaPago != "") {
                            // $("#selectFormaPago").find(`option:contains('${data.PERU_formaPago}')`).attr('selected', true);
                            $("#selectFormaPago").val(data.PERU_formaPago);
                        }

                        llenarInputProveedorDistinto(inputProvNum.val());
                    } else {
                        AlertaGeneral(`Alerta`, `El proveedor "${response.label} - ${response.id}" no está activo.`);
                        inputProvNum.val('');
                        inputProvNom.val('');

                        llenarInputProveedorDistinto('');
                    }
                }
            }).then($.unblockUI);

            //#region Panel Izquierdo
            selectBoS.val(data.bienes_servicios);
            dtpFecha.val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.fecha.substr(6)))));
            // inputProvNum.val(data.proveedor);
            inputProvNom.val(data.proveedorNom);
            inputCompNum.val(data.comprador);
            inputCompNom.val(data.compradorNom);
            inputSolNum.val(data.solicito);
            inputSolNom.val(data.solicitoNom);
            inputAutNum.val(data.autorizo);
            inputAutNom.val(data.autorizoNom);
            inputEmb.val(data.embarquese);
            // selectLab.val(data.libre_abordo);
            inputConFact.val(data.concepto_factura);

            if (data.bit_autorecepcion == 'S') {
                checkAutoRecep.prop("checked", true);
            } else {
                checkAutoRecep.prop("checked", false);
            }

            inputAlmNum.val(data.almacen_autorecepcion);
            inputAlmNom.val(data.almacenRecepNom);
            inputEmpNum.val(data.empleado_autorecepcion);
            inputEmpNom.val(data.empleadoRecepNom);
            //#endregion

            //#region Panel Derecho
            selectTipoOC.val(data.tipo_oc_req);
            selectMoneda.val(data.moneda);
            inputTipoCambio.val(data.tipo_cambio);
            inputIVAPorcentaje.val(data.porcent_iva);

            if (_empresaActual == 6) {
                inputSubTotal.val(maskNumero2DCompras_PERU(data.sub_total));
                inputIVANumero.val(maskNumero2DCompras_PERU(data.iva));
                inputRetencion.val(maskNumero2DCompras_PERU(data.rentencion_despues_iva));
                inputTotal.val(maskNumero2DCompras_PERU(data.total));
                let totalFinal = (unmaskNumero(inputRetencion.val()) - unmaskNumero((inputTotal.val()))) * -1
                inputTotalFinal.val(maskNumero2DCompras_PERU((totalFinal)));
            } else {
                inputSubTotal.val(maskNumero(data.sub_total));
                inputIVANumero.val(maskNumero(data.iva));
                inputRetencion.val(maskNumero(data.rentencion_despues_iva));
                inputTotal.val(maskNumero(data.total));
                let totalFinal = (unmaskNumero(inputRetencion.val()) - unmaskNumero((inputTotal.val()))) * -1
                inputTotalFinal.val(maskNumero2DCompras((totalFinal)));
            }

            data.ST_OC == 'A' ? $('#btnGuardarRetenciones').hide() : $('#btnGuardarRetenciones').show();
            //#endregion
        }

        function llenarInputProveedorDistinto(numeroProveedor) {
            let listInputs = tblPartidas.find('tbody tr .inputProveedorDistinto');

            $(listInputs).each(function (id, element) {
                $(element).val(numeroProveedor);
            });
        }

        function limpiarInformacion() {
            //#region Panel Izquierdo
            selectBoS.val('');
            inputAlmacenMovimiento.val('');
            inputRemision.val('');
            dtpFecha.val('');
            inputProvNum.val('');
            inputProvNom.val('');
            inputCompNum.val('');
            inputCompNom.val('');
            inputSolNum.val('');
            inputSolNom.val('');
            inputAutNum.val('');
            inputAutNom.val('');
            inputEmb.val('');
            // selectLab.val('');
            inputConFact.val('');

            checkAutoRecep.prop("checked", false);

            inputAlmNum.val('');
            inputAlmNom.val('');
            inputEmpNum.val('');
            inputEmpNom.val('');
            //#endregion

            //#region Panel Derecho
            selectTipoOC.val('');
            selectMoneda.val('');
            inputTipoCambio.val('');
            inputSubTotal.val('');
            inputIVAPorcentaje.val('');
            inputIVANumero.val('');
            inputRetencion.val('');
            inputTotal.val('');
            inputTotalFinal.val('');
            //#endregion
        }

        function limpiarTabla(tbl) {
            dt = tbl.DataTable();
            dt.clear().draw();
        }

        function cargarCompra() {
            $.post('/Enkontrol/OrdenCompra/GetCompra', { cc: selectCC.val(), num: inputNumero.val() != '' ? inputNumero.val() : 0, PERU_tipoCompra: "RS" }).then(response => {
                if (response.success) {
                    inputNumMovimiento.val(response.ultimoMovimiento);
                    if (response.info.cc != null) {
                        if (response.info.estatus != 'C') {
                            // if (response.info.inventariado == 'N') {
                            let flagCompraPendienteSurtir = response.partidas.some(function (x) {
                                return x.cantidad > x.cant_recibida;
                            });

                            if (flagCompraPendienteSurtir) {
                                btnGlobal.attr('disabled', false);
                                btnGuardarSurtido.attr('disabled', false);

                                tblPartidas.DataTable().column(3).visible(true);
                                tblPartidas.DataTable().column(8).visible(true);
                                $(tblPartidas.DataTable().column(6).header()).text('Cantidad');

                                llenarInformacion(response.info);

                                if (response.partidas.length > 0) {
                                    inputNumeroReq.val(response.partidas[0].num_requisicion);
                                }

                                let partidasFiltradas = response.partidas.filter(x => x.inventariado == 'N');

                                AddRows(tblPartidas, partidasFiltradas);

                                // tblPartidas.find('tbody tr:eq(0) td:eq(0)').click()
                                btnGuardarSurtido.attr('disabled', !(partidasFiltradas.length > 0));

                                if (partidasFiltradas.length == 0) {
                                    AlertaGeneral(`Alerta`, `La compra no contiene partidas no inventariables.`);
                                }
                            } else {
                                AlertaGeneral(`Alerta`, `La compra ya ha sido surtida.`);

                                limpiarInformacion();
                                limpiarTabla(tblPartidas);

                                textAreaDescPartida.val('');
                            }
                            // } else {
                            //     AlertaGeneral(`Alerta`, `La compra "${selectCC.val()}-${inputNumero.val()}" no es inventariable.`);

                            //     limpiarInformacion();
                            //     limpiarTabla(tblPartidas);

                            //     selectCC.val('');
                            //     inputNumero.val('');
                            //     textAreaDescPartida.val('');
                            // }
                        } else {
                            AlertaGeneral(`Alerta`, `La compra "${selectCC.val()}-${inputNumero.val()}" está cancelada.`);

                            limpiarInformacion();
                            limpiarTabla(tblPartidas);

                            selectCC.val('');
                            inputNumero.val('');
                            textAreaDescPartida.val('');
                        }
                    } else {
                        AlertaGeneral('Alerta', 'No se encontró información.');

                        limpiarInformacion();
                        limpiarTabla(tblPartidas);

                        textAreaDescPartida.val('');
                    }
                } else {
                    AlertaGeneral('Alerta', 'Error al consultar la información.');
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function cargarMovimiento() {
            $.post('/Enkontrol/OrdenCompra/GetMovimientoNoInv', { almacenID: inputAlmacenMovimiento.val(), remision: inputRemision.val() }).then(response => {
                if (response.success) {
                    if (response.movimiento != null) {
                        btnGlobal.attr('disabled', true);
                        btnGuardarSurtido.attr('disabled', true);

                        tblPartidas.DataTable().column(3).visible(false);
                        tblPartidas.DataTable().column(8).visible(false);
                        $(tblPartidas.DataTable().column(6).header()).text('Cantidad');

                        if (response.movimiento.tieneCompra) {
                            llenarInformacion(response.movimiento.compra);

                            if (response.movimiento.compra.lstPartidas.length > 0) {
                                inputNumeroReq.val(response.movimiento.compra.lstPartidas[0].num_requisicion);
                            }

                            let detalle = [];

                            $(response.movimiento.detalle).each(function (id, det) {
                                let partidaCompra = response.movimiento.compra.lstPartidas.find(function (element) {
                                    return element.partida == det.partida && element.insumo == det.insumo;
                                });

                                detalle.push({
                                    partida: det.partida,
                                    insumo: det.insumo,
                                    insumoDesc: partidaCompra.insumoDesc,
                                    area: partidaCompra.area,
                                    cuenta: partidaCompra.cuenta,
                                    cantidadPendiente: det.cantidad, //Se usa la propiedad "cantidadPendiente" de la tabla nomás para mostrar la cantidad del movimiento.
                                    unidad: partidaCompra.unidad,
                                    cant_recibida: 0,
                                    precio: partidaCompra.precio,
                                    monedaDesc: partidaCompra.monedaDesc,
                                    importe: partidaCompra.importe
                                });
                            });

                            // AddRows(tblPartidas, response.movimiento.compra.lstPartidas);
                            AddRows(tblPartidas, detalle);

                            tblPartidas.find('tbody tr:eq(0) td:eq(0)').click()
                        }
                    } else {
                        AlertaGeneral('Alerta', 'No se encontró información.');

                        limpiarInformacion();
                        limpiarTabla(tblPartidas);

                        textAreaDescPartida.val('');
                    }
                } else {
                    AlertaGeneral('Alerta', 'Error al consultar la información.');
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function aplicarGlobal() {
            if (tblPartidas.DataTable().data().any()) {
                tblPartidas.find('tbody tr').each(function (id, row) {
                    let rowData = tblPartidas.DataTable().row(row).data();
                    let inputValorSurtir = $(row).find('.inputValorSurtir');

                    inputValorSurtir.val(rowData.cantidadPendiente);
                });

                AlertaGeneral(`Alerta`, `Entrada Global. Verifique las cantidades capturadas antes de guardar.`);
            }
        }

        function verReporte() {
            report.attr("src", `/Reportes/Vista.aspx?idReporte=119`);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function verificarComprasPeru() {
            if (_empresaActual == 6) {
                // selectMoneda.attr('disabled', false);
                $('.elementoPeru').show();
                $(".spanIVA").html("I.G.V.");
                $(".elementoNoPeru").hide();
            } else {
                $('.elementoPeru').hide();
                $(".spanIVA").html("I.V.A.");
            }
        }
    }

    $(document).ready(() => Enkontrol.Compras.OrdenCompra.EntradaNoInventariable = new EntradaNoInventariable())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();