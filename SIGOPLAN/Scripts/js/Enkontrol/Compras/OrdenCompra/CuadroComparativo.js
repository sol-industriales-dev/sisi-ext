(function () {
    $.namespace('Enkontrol.Compras.OrdenCompra.CuadroComparativo');
    CuadroComparativo = function () {

        //EMPRESA
        const empresaActual = $('#empresaActual');

        //#region Selectores
        const selectCC = $('#selectCC');
        const selectTipo = $('#selectTipo');
        const tblRequisiciones = $('#tblRequisiciones');
        const tblPartidas = $('#tblPartidas');
        const tblUltimaCompra = $('#tblUltimaCompra');
        const inputFechaInicial = $('#inputFechaInicial');
        const inputFechaFinal = $('#inputFechaFinal');
        const inputFechaCuadro = $('#inputFechaCuadro');
        const inputFechaInicialInsumosLicitados = $('#inputFechaInicialInsumosLicitados');
        const inputFechaFinalInsumosLicitados = $('#inputFechaFinalInsumosLicitados');
        const checkInsumosLicitados = $('#checkInsumosLicitados');
        const divInsumoLicitadoInicial = $('#divInsumoLicitadoInicial');
        const divInsumoLicitadoFinal = $('#divInsumoLicitadoFinal');
        const btnBuscarCuadro = $('#btnBuscarCuadro');
        const inputReqInicial = $('#inputReqInicial');
        const inputReqFinal = $('#inputReqFinal');
        const btnGuardarCuadro = $('#btnGuardarCuadro');
        const inputSolicita = $('#inputSolicita');
        const btnAgregarRenglonCuadro = $('#btnAgregarRenglonCuadro');
        const labelRequisicionesPendientes = $('#labelRequisicionesPendientes');
        const dialogListaCompras = $('#dialogListaCompras');
        const btnImprimirCuadro = $('#btnImprimirCuadro');
        const report = $("#report");

        //#region Panel Derecho
        const divPanelDerecho = $('#divPanelDerecho');
        const inputProv1Num = $('#inputProv1Num');
        const inputProv1Desc = $('#inputProv1Desc');
        const inputProv2Num = $('#inputProv2Num');
        const inputProv2Desc = $('#inputProv2Desc');
        const inputProv3Num = $('#inputProv3Num');
        const inputProv3Desc = $('#inputProv3Desc');

        //PERU
        const cboTipoMonedaProv1 = $('#cboTipoMonedaProv1');
        const cboTipoMonedaProv2 = $('#cboTipoMonedaProv2');
        const cboTipoMonedaProv3 = $('#cboTipoMonedaProv3');

        const inputPrimerSubtotalProv1Num = $('#inputPrimerSubtotalProv1Num');
        const inputPrimerSubtotalProv1Moneda = $('#inputPrimerSubtotalProv1Moneda');
        const inputPrimerSubtotalProv2Num = $('#inputPrimerSubtotalProv2Num');
        const inputPrimerSubtotalProv2Moneda = $('#inputPrimerSubtotalProv2Moneda');
        const inputPrimerSubtotalProv3Num = $('#inputPrimerSubtotalProv3Num');
        const inputPrimerSubtotalProv3Moneda = $('#inputPrimerSubtotalProv3Moneda');

        const inputDescuentoProv1 = $('#inputDescuentoProv1');
        const inputDescuentoProv2 = $('#inputDescuentoProv2');
        const inputDescuentoProv3 = $('#inputDescuentoProv3');

        const inputSegundoSubtotalProv1Num = $('#inputSegundoSubtotalProv1Num');
        const inputSegundoSubtotalProv1Moneda = $('#inputSegundoSubtotalProv1Moneda');
        const inputSegundoSubtotalProv2Num = $('#inputSegundoSubtotalProv2Num');
        const inputSegundoSubtotalProv2Moneda = $('#inputSegundoSubtotalProv2Moneda');
        const inputSegundoSubtotalProv3Num = $('#inputSegundoSubtotalProv3Num');
        const inputSegundoSubtotalProv3Moneda = $('#inputSegundoSubtotalProv3Moneda');

        const inputIVAProv1 = $('#inputIVAProv1');
        const inputIVAProv2 = $('#inputIVAProv2');
        const inputIVAProv3 = $('#inputIVAProv3');

        const inputTotalProv1Num = $('#inputTotalProv1Num');
        const inputTotalProv1Moneda = $('#inputTotalProv1Moneda');
        const inputTotalProv2Num = $('#inputTotalProv2Num');
        const inputTotalProv2Moneda = $('#inputTotalProv2Moneda');
        const inputTotalProv3Num = $('#inputTotalProv3Num');
        const inputTotalProv3Moneda = $('#inputTotalProv3Moneda');

        const inputFletesProv1Num = $('#inputFletesProv1Num');
        const inputFletesProv1Moneda = $('#inputFletesProv1Moneda');
        const inputFletesProv2Num = $('#inputFletesProv2Num');
        const inputFletesProv2Moneda = $('#inputFletesProv2Moneda');
        const inputFletesProv3Num = $('#inputFletesProv3Num');
        const inputFletesProv3Moneda = $('#inputFletesProv3Moneda');

        const inputImportacionProv1Num = $('#inputImportacionProv1Num');
        const inputImportacionProv1Moneda = $('#inputImportacionProv1Moneda');
        const inputImportacionProv2Num = $('#inputImportacionProv2Num');
        const inputImportacionProv2Moneda = $('#inputImportacionProv2Moneda');
        const inputImportacionProv3Num = $('#inputImportacionProv3Num');
        const inputImportacionProv3Moneda = $('#inputImportacionProv3Moneda');

        const inputGranTotalProv1Num = $('#inputGranTotalProv1Num');
        const inputGranTotalProv1Moneda = $('#inputGranTotalProv1Moneda');
        const inputGranTotalProv2Num = $('#inputGranTotalProv2Num');
        const inputGranTotalProv2Moneda = $('#inputGranTotalProv2Moneda');
        const inputGranTotalProv3Num = $('#inputGranTotalProv3Num');
        const inputGranTotalProv3Moneda = $('#inputGranTotalProv3Moneda');

        //PERU
        const inputGranTotalProv1NumPERU = $('#inputGranTotalProv1NumPERU');
        const inputGranTotalProv1MonedaPERU = $('#inputGranTotalProv1MonedaPERU');
        const inputGranTotalProv2NumPERU = $('#inputGranTotalProv2NumPERU');
        const inputGranTotalProv2MonedaPERU = $('#inputGranTotalProv2MonedaPERU');
        const inputGranTotalProv3NumPERU = $('#inputGranTotalProv3NumPERU');
        const inputGranTotalProv3MonedaPERU = $('#inputGranTotalProv3MonedaPERU');

        const inputTipoCambioProv1 = $('#inputTipoCambioProv1');
        const inputTipoCambioProv2 = $('#inputTipoCambioProv2');
        const inputTipoCambioProv3 = $('#inputTipoCambioProv3');

        const inputFechaEntregaProv1 = $('#inputFechaEntregaProv1');
        const inputFechaEntregaProv2 = $('#inputFechaEntregaProv2');
        const inputFechaEntregaProv3 = $('#inputFechaEntregaProv3');

        const inputLABProv1Num = $('#inputLABProv1Num');
        const inputLABProv1Desc = $('#inputLABProv1Desc');
        const inputLABProv2Num = $('#inputLABProv2Num');
        const inputLABProv2Desc = $('#inputLABProv2Desc');
        const inputLABProv3Num = $('#inputLABProv3Num');
        const inputLABProv3Desc = $('#inputLABProv3Desc');

        const inputCondPagoProv1 = $('#inputCondPagoProv1');
        const inputCondPagoProv2 = $('#inputCondPagoProv2');
        const inputCondPagoProv3 = $('#inputCondPagoProv3');

        // const inputComentarioProv1 = $('#inputComentarioProv1');
        const textAreaComentarioProv1 = $('#textAreaComentarioProv1');
        // const inputComentarioProv2 = $('#inputComentarioProv2');
        const textAreaComentarioProv2 = $('#textAreaComentarioProv2');
        // const inputComentarioProv3 = $('#inputComentarioProv3');
        const textAreaComentarioProv3 = $('#textAreaComentarioProv3');
        const botonDescargarArchivo1 = $('#botonDescargarArchivo1');
        const botonDescargarArchivo2 = $('#botonDescargarArchivo2');
        const botonDescargarArchivo3 = $('#botonDescargarArchivo3');
        const inputEmpresaActual = $('#inputEmpresaActual')
        //#endregion

        //#endregion

        _urlYaUsada = false;
        _countTotalFilas = 0;
        _countIncremento = 0;
        _countIndex = 0;
        _empresaActual = +inputEmpresaActual.val();
        let _tipoCambioPeru = 1;

        function init() {
            verificarComprasPeru();
            initForm();

            // Se comprueba si hay variables en url.
            const variables = getUrlParams(window.location.href);

            if (variables && variables.cc && variables.numero && !_urlYaUsada) {
                selectCC.val(variables.cc);
                selectCC.change();
            }

            // getContadorRequisicionesPendientes();
        }

        checkInsumosLicitados.on('click', function () {
            if ($(this).prop('checked')) {
                divInsumoLicitadoInicial.removeClass('hidden');
                divInsumoLicitadoFinal.removeClass('hidden');
            } else {
                divInsumoLicitadoInicial.addClass('hidden');
                divInsumoLicitadoFinal.addClass('hidden');
            }
        });

        btnBuscarCuadro.on('click', function () {
            btnAgregarRenglonCuadro.attr('disabled', true);
            btnImprimirCuadro.attr('disabled', true);

            let filtros = {
                cc: selectCC.val(),
                fechaInicial: inputFechaInicial.val(),
                fechaFinal: inputFechaFinal.val(),
                reqInicial: inputReqInicial.val(),
                reqFinal: inputReqFinal.val(),
                tipo: selectTipo.val() != '' ? selectTipo.val() : 0,
                insumosLicitados: checkInsumosLicitados.prop('checked'),
                fechaInicialInsumosLicitados: inputFechaInicialInsumosLicitados.val(),
                fechaFinalInsumosLicitados: inputFechaFinalInsumosLicitados.val()
            };

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/BuscarCuadros', { filtros }).always($.unblockUI).then(response => {
                limpiarCuadro();

                if (response.success) {
                    AddRows(tblRequisiciones, response.data);

                    // Se comprueba si hay variables en url.
                    const variables = getUrlParams(window.location.href);

                    if (variables && variables.cc && variables.numero && !_urlYaUsada) {
                        let rowSeleccionado = null;

                        tblRequisiciones.find('tbody tr').each(function (idx, row) {
                            let rowData = tblRequisiciones.DataTable().row(row).data();

                            if (rowData.numero == variables.numero) {
                                rowSeleccionado = row;
                            }
                        });

                        if (rowSeleccionado != null) {
                            $(rowSeleccionado).click();
                        }

                        _urlYaUsada = true;
                    }
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        });

        btnGuardarCuadro.on('click', function () {
            let requisicion = tblRequisiciones.DataTable().row(tblRequisiciones.find('tbody .requisicionSeleccionada')).data();
            let nuevoCuadro = getNuevoCuadro();
            let flagCrearEditar = fncValidarCuadroComparativo();
            if (flagCrearEditar) {
                if (!requisicion.tieneCuadro) {
                    $.blockUI({ message: 'Procesando...' });
                    $.post('/Enkontrol/OrdenCompra/GuardarNuevoCuadro', { nuevoCuadro })
                        .always($.unblockUI)
                        .then(response => {
                            if (response.success) {
                                btnBuscarCuadro.click();
                                AlertaGeneral('Alerta', 'Se ha guardado la información.');
                                btnGuardarCuadro.attr('disabled', true);
                            } else {
                                AlertaGeneral('Alerta', 'Error al capturar la información.');
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                } else {
                    $.blockUI({ message: 'Procesando...' });
                    $.post('/Enkontrol/OrdenCompra/UpdateCuadro', { cuadro: nuevoCuadro })
                        .always($.unblockUI)
                        .then(response => {
                            if (response.success) {
                                btnBuscarCuadro.click();
                                AlertaGeneral('Alerta', 'Se ha guardado la información.');
                                btnGuardarCuadro.attr('disabled', true);
                            } else {
                                AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                }
            }
        });

        inputLABProv1Num.on("keypress", function (e) {
            aceptaSoloNumeroXD($(this), e, 2);
        });
        inputLABProv2Num.on("keypress", function (e) {
            aceptaSoloNumeroXD($(this), e, 2);
        });
        inputLABProv3Num.on("keypress", function (e) {
            aceptaSoloNumeroXD($(this), e, 2);
        });

        inputCondPagoProv1.on("keypress", function (e) {
            aceptaSoloNumeroXD($(this), e, 2);
        });
        inputCondPagoProv2.on("keypress", function (e) {
            aceptaSoloNumeroXD($(this), e, 2);
        });
        inputCondPagoProv3.on("keypress", function (e) {
            aceptaSoloNumeroXD($(this), e, 2);
        });

        function fncValidarCuadroComparativo() {
            let flagCrearEditar = false;
            let strMensajeError = "";

            if (inputLABProv1Num.val() != "" && inputCondPagoProv1.val() != "") {
                flagCrearEditar = true;
            }
            else if (inputLABProv2Num.val() != "" && inputCondPagoProv2.val() != "") {
                flagCrearEditar = true;
            }
            else if (inputLABProv3Num.val() != "" && inputCondPagoProv3.val() != "") {
                flagCrearEditar = true;
            }

            if ((inputLABProv1Num.val() != "" && inputCondPagoProv1.val() == "") ||
                (inputLABProv1Num.val() == "" && inputCondPagoProv1.val() != "")) {

                inputLABProv1Num.val() == "" ? strMensajeError += "<br>Es necesario ingresar el campo L.A.B. al proveedor #1." : true;
                inputCondPagoProv1.val() == "" ? strMensajeError += "<br>Es necesario ingresar la Cond. de Pago al proveedor #1." : true;
                strMensajeError += "<br>";
            }
            if ((inputLABProv2Num.val() != "" && inputCondPagoProv2.val() == "") ||
                (inputLABProv2Num.val() == "" && inputCondPagoProv2.val() != "")) {

                inputLABProv2Num.val() == "" ? strMensajeError += "<br>Es necesario ingresar el campo L.A.B. al proveedor #2." : true;
                inputCondPagoProv2.val() == "" ? strMensajeError += "<br>Es necesario ingresar la Cond. de Pago al proveedor #2." : true;
                strMensajeError += "<br>";
            }
            if ((inputLABProv3Num.val() != "" && inputCondPagoProv3.val() == "") ||
                (inputLABProv3Num.val() == "" && inputCondPagoProv3.val() != "")) {

                inputLABProv3Num.val() == "" ? strMensajeError += "<br>Es necesario ingresar el campo L.A.B. al proveedor #3." : true;
                inputCondPagoProv3.val() == "" ? strMensajeError += "<br>Es necesario ingresar la Cond. de Pago al proveedor #3." : true;
                strMensajeError += "<br>";
            }
            if ((inputLABProv1Num.val() == "" && inputCondPagoProv1.val() == "") &&
                (inputLABProv2Num.val() == "" && inputCondPagoProv2.val() == "") &&
                (inputLABProv3Num.val() == "" && inputCondPagoProv3.val() == "")) {
                strMensajeError += "Es necesario ingresar el campo L.A.B y la Cond. de Pago a un proveedor para poder guardar.";
            }

            if (strMensajeError != "") {
                flagCrearEditar = false;
                Alert2Warning(strMensajeError);
            }
            return flagCrearEditar;
        }

        selectCC.on('change', function () {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/requisicionesNumeros', { cc: selectCC.val() })
                .always($.unblockUI)
                .then(response => {
                    limpiarCuadro();

                    if (response.success) {
                        inputReqInicial.val(response.data.minimo);
                        inputReqFinal.val(response.data.maximo);

                        // Se comprueba si hay variables en url.
                        const variables = getUrlParams(window.location.href);

                        if (variables && variables.cc && variables.numero && !_urlYaUsada) {
                            btnBuscarCuadro.click();
                        }
                    } else {
                        AlertaGeneral('Alerta', 'Error al consultar la información.');
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        $('#inputLABProv1Num, #inputLABProv2Num, #inputLABProv3Num').on('change', function () {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/GetLABFromNum', { num: $(this).val() })
                .always($.unblockUI)
                .then(response => {
                    switch ($(this).attr('id')) {
                        case 'inputLABProv1Num':
                            inputLABProv1Desc.val(response);
                            break;
                        case 'inputLABProv2Num':
                            inputLABProv2Desc.val(response);
                            break;
                        case 'inputLABProv3Num':
                            inputLABProv3Desc.val(response);
                            break;
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        tblPartidas.on('change', '.inputCantidad', function () {
            $(this).val(unmaskNumero6D($(this).val()));

            calculoProveedor1();
            calculoProveedor2();
            calculoProveedor3();
        });
        tblPartidas.on('change', '.inputProv1', function () {
            // $(this).val(maskNumero6D(unmaskNumero($(this).val())));
            let valor = $(this).val();
            let numero = unmaskNumero(valor);
            let precioFormateado = formatMoney(numero);

            $(this).val('$' + precioFormateado);

            calculoProveedor1();
        });
        tblPartidas.on('change', '.inputProv2', function () {
            // $(this).val(maskNumero6D(unmaskNumero($(this).val())));
            let valor = $(this).val();
            let numero = unmaskNumero(valor);
            let precioFormateado = formatMoney(numero);

            $(this).val('$' + precioFormateado);

            calculoProveedor2();
        });
        tblPartidas.on('change', '.inputProv3', function () {
            // $(this).val(maskNumero6D(unmaskNumero($(this).val())));
            let valor = $(this).val();
            let numero = unmaskNumero(valor);
            let precioFormateado = formatMoney(numero);

            $(this).val('$' + precioFormateado);

            calculoProveedor3();
        });

        divPanelDerecho.on('change', '.calculoProv1', function () {
            calculoProveedor1();
        });
        divPanelDerecho.on('change', '.calculoProv2', function () {
            calculoProveedor2();
        });
        divPanelDerecho.on('change', '.calculoProv3', function () {
            calculoProveedor3();
        });

        $('#inputProv1Num, #inputProv2Num, #inputProv3Num').on('change', function () {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/GetProveedorInfo', { num: $(this).val() })
                .always($.unblockUI)
                .then(response => {
                    switch ($(this).attr('id')) {
                        case 'inputProv1Num':
                            inputProv1Num.val(response.label);
                            inputProv1Num.attr('monedaProv', response.moneda);
                            // inputProv1Num.attr('monedaTipoCambio', response.monedaTipoCambio);
                            inputProv1Desc.val(response.id);
                            agregarTooltip(inputProv1Desc, response.id);

                            if (empresaActual.val() == 6) {
                                if (cboTipoMonedaProv1.val() == 4) {

                                    inputTipoCambioProv1.val("$1.00");
                                    $('.spanProv1').text("SOL");

                                } else {
                                    let numTCPeru = typeof (_tipoCambioPeru) == "string" ? Number(_tipoCambioPeru) : _tipoCambioPeru

                                    inputTipoCambioProv1.val("$" + numTCPeru.toFixed(2));
                                    $('.spanProv1').text("DLS");

                                }
                                inputCondPagoProv1.val(response.PERU_formaPago);

                            } else {
                                $('.spanProv1').text(response.monedaDesc);
                                inputTipoCambioProv1.val(maskNumero(response.monedaTipoCambio));

                            }

                            $(tblPartidas.DataTable().column(3).header()).text(response.id);
                            break;
                        case 'inputProv2Num':
                            inputProv2Num.val(response.label);
                            inputProv2Num.attr('monedaProv', response.moneda);
                            inputProv2Num.attr('monedaTipoCambio', response.monedaTipoCambio);
                            // inputTipoCambioProv2.val(maskNumero(response.monedaTipoCambio));
                            inputProv2Desc.val(response.id);
                            agregarTooltip(inputProv2Desc, response.id);
                            // $('.spanProv2').text(response.monedaDesc);

                            if (empresaActual.val() == 6) {
                                if (cboTipoMonedaProv2.val() == 4) {

                                    inputTipoCambioProv2.val("$1.00");
                                    $('.spanProv2').text("SOL");

                                } else {
                                    let numTCPeru = typeof (_tipoCambioPeru) == "string" ? Number(_tipoCambioPeru) : _tipoCambioPeru

                                    inputTipoCambioProv2.val("$" + numTCPeru.toFixed(2));
                                    $('.spanProv2').text("DLS");

                                }
                                inputCondPagoProv2.val(response.PERU_formaPago);

                            } else {
                                $('.spanProv2').text(response.monedaDesc);
                                inputTipoCambioProv2.val(maskNumero(response.monedaTipoCambio));

                            }

                            $(tblPartidas.DataTable().column(4).header()).text(response.id);
                            break;
                        case 'inputProv3Num':
                            inputProv3Num.val(response.label);
                            inputProv3Num.attr('monedaProv', response.moneda);
                            inputProv3Num.attr('monedaTipoCambio', response.monedaTipoCambio);
                            // inputTipoCambioProv3.val(maskNumero(response.monedaTipoCambio));
                            inputProv3Desc.val(response.id);
                            agregarTooltip(inputProv3Desc, response.id);
                            // $('.spanProv3').text(response.monedaDesc);

                            if (empresaActual.val() == 6) {
                                if (cboTipoMonedaProv3.val() == 4) {

                                    inputTipoCambioProv3.val("$1.00");
                                    $('.spanProv3').text("SOL");

                                } else {
                                    let numTCPeru = typeof (_tipoCambioPeru) == "string" ? Number(_tipoCambioPeru) : _tipoCambioPeru

                                    inputTipoCambioProv3.val("$" + numTCPeru.toFixed(2));
                                    $('.spanProv3').text("DLS");

                                }
                                inputCondPagoProv3.val(response.PERU_formaPago);


                            } else {
                                $('.spanProv3').text(response.monedaDesc);
                                inputTipoCambioProv3.val(maskNumero(response.monedaTipoCambio));

                            }

                            $(tblPartidas.DataTable().column(5).header()).text(response.id);
                            break;
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        btnAgregarRenglonCuadro.on('click', function () {
            let datos = tblRequisiciones.DataTable().rows().data();
            let renglonActivo = tblRequisiciones.DataTable().row(tblRequisiciones.find('tbody tr.requisicionSeleccionada')).data();
            let renglonesMatch = datos.filter(function (x) { return x.numero == renglonActivo.numero });
            let cuadrosExistentes = renglonesMatch.length;
            let indexPrimerRenglonMatch = cuadrosExistentes > 0 ? datos.indexOf(renglonesMatch[0]) : 0;
            let indexNuevoRenglon = indexPrimerRenglonMatch + cuadrosExistentes;

            let renglonNuevo = JSON.parse(JSON.stringify(renglonActivo));

            renglonNuevo.folio = cuadrosExistentes + 1;
            renglonNuevo.fecha_cuadro = getFecha();
            renglonNuevo.tieneCuadro = false;

            datos.splice(indexNuevoRenglon, 0, renglonNuevo);

            tblRequisiciones.DataTable().clear();
            tblRequisiciones.DataTable().rows.add(datos).draw();

            tblRequisiciones.find('tr:eq(' + (indexNuevoRenglon + 1) + ')').click();
        });

        divPanelDerecho.on('focus', 'input', function () {
            $(this).select();
        });

        btnImprimirCuadro.on('click', function () {
            verReporte();
        });

        $('#botonDescargarArchivo1, #botonDescargarArchivo2, #botonDescargarArchivo3').on('click', function () {
            if ($(this).data().rutaArchivo != undefined) {
                location.href = `DescargarArchivoCotizacion?ruta=${$(this).data().rutaArchivo}`;
            }
        });

        cboTipoMonedaProv1.on("change", function () {
            if ($(this).val() == "2") {

                let numTCPeru = typeof (_tipoCambioPeru) == "string" ? Number(_tipoCambioPeru) : _tipoCambioPeru

                inputTipoCambioProv1.val("$" + numTCPeru.toFixed(2));

                var table = tblPartidas.DataTable();

                if (table.data().count() > 0) {
                    $('.spanProv1').text("DLS");
                    inputProv1Num.attr('monedaProv', 2);

                    calculoProveedor1();
                }
            } else {

                inputTipoCambioProv1.val('$1.00');

                var table = tblPartidas.DataTable();

                if (table.data().count() > 0) {
                    $('.spanProv1').text("SOL");
                    inputProv1Num.attr('monedaProv', 4);

                    calculoProveedor1();
                }
            }

        });

        cboTipoMonedaProv2.on("change", function () {
            if ($(this).val() == "2") {

                let numTCPeru = typeof (_tipoCambioPeru) == "string" ? Number(_tipoCambioPeru) : _tipoCambioPeru

                inputTipoCambioProv2.val("$" + numTCPeru.toFixed(2));

                var table = tblPartidas.DataTable();

                if (table.data().count() > 0) {
                    $('.spanProv2').text("DLS");
                    inputProv2Num.attr('monedaProv', 2);

                    calculoProveedor2();
                }
            } else {

                inputTipoCambioProv2.val('$1.00');

                var table = tblPartidas.DataTable();

                if (table.data().count() > 0) {
                    $('.spanProv2').text("SOL");
                    inputProv2Num.attr('monedaProv', 4);

                    calculoProveedor2();
                }
            }

        });

        cboTipoMonedaProv3.on("change", function () {
            if ($(this).val() == "2") {

                let numTCPeru = typeof (_tipoCambioPeru) == "string" ? Number(_tipoCambioPeru) : _tipoCambioPeru

                inputTipoCambioProv3.val("$" + numTCPeru.toFixed(2));

                var table = tblPartidas.DataTable();

                if (table.data().count() > 0) {
                    $('.spanProv3').text("DLS");
                    inputProv3Num.attr('monedaProv', 2);

                    calculoProveedor3();
                }
            } else {

                inputTipoCambioProv3.val('$1.00');

                var table = tblPartidas.DataTable();

                if (table.data().count() > 0) {

                    $('.spanProv3').text("SOL");
                    inputProv3Num.attr('monedaProv', 4);

                    calculoProveedor3();
                }
            }
        });

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
        function quitarTooltip(elemento) {
            $(elemento).removeAttr('data-toggle');
            $(elemento).removeAttr('data-placement');
            $(elemento).removeAttr('title');
        }
        function initForm() {
            let hoy = new Date().toLocaleDateString('es-ES');

            initTableRequisiciones();
            initTablePartidas();
            initTableUltimaCompra();

            selectCC.fillCombo('/Enkontrol/Requisicion/FillComboCcAsigReq', null, false, null);

            inputFechaInicial.datepicker().datepicker('setDate', new Date(new Date().getFullYear(), 0, 1));
            inputFechaFinal.datepicker().datepicker('setDate', new Date(new Date().getFullYear(), 11, 31));
            inputFechaCuadro.datepicker().datepicker("setDate", hoy);
            inputFechaInicialInsumosLicitados.datepicker().datepicker('setDate', new Date(new Date().getFullYear(), 0, 1));
            inputFechaFinalInsumosLicitados.datepicker().datepicker('setDate', new Date(new Date().getFullYear(), 11, 31));

            inputFechaEntregaProv1.datepicker().datepicker("setDate", hoy);
            inputFechaEntregaProv2.datepicker().datepicker("setDate", hoy);
            inputFechaEntregaProv3.datepicker().datepicker("setDate", hoy);

            inputPrimerSubtotalProv1Num.val('$0.00');
            inputPrimerSubtotalProv2Num.val('$0.00');
            inputPrimerSubtotalProv3Num.val('$0.00');

            inputDescuentoProv1.val('0.00');
            inputDescuentoProv2.val('0.00');
            inputDescuentoProv3.val('0.00');

            inputSegundoSubtotalProv1Num.val('$0.00');
            inputSegundoSubtotalProv2Num.val('$0.00');
            inputSegundoSubtotalProv3Num.val('$0.00');

            inputTotalProv1Num.val('$0.00');
            inputTotalProv2Num.val('$0.00');
            inputTotalProv3Num.val('$0.00');

            inputFletesProv1Num.val('$0.00');
            inputFletesProv2Num.val('$0.00');
            inputFletesProv3Num.val('$0.00');

            inputImportacionProv1Num.val('$0.00');
            inputImportacionProv2Num.val('$0.00');
            inputImportacionProv3Num.val('$0.00');

            inputGranTotalProv1Num.val('$0.00');
            inputGranTotalProv2Num.val('$0.00');
            inputGranTotalProv3Num.val('$0.00');

            inputGranTotalProv1NumPERU.val('$0.00');
            inputGranTotalProv2NumPERU.val('$0.00');
            inputGranTotalProv3NumPERU.val('$0.00');

            inputTipoCambioProv1.val('$1.00');
            inputTipoCambioProv2.val('$1.00');
            inputTipoCambioProv3.val('$1.00');

            inputProv1Desc.getAutocompleteValid(setProveedor1, null, null, '/Enkontrol/OrdenCompra/GetProveedorNumero');
            inputProv2Desc.getAutocompleteValid(setProveedor2, null, null, '/Enkontrol/OrdenCompra/GetProveedorNumero');
            inputProv3Desc.getAutocompleteValid(setProveedor3, null, null, '/Enkontrol/OrdenCompra/GetProveedorNumero');
        }

        function initTableRequisiciones() {
            tblRequisiciones.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollY: "290px",
                scrollCollapse: true,
                ordering: false,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblRequisiciones.on('click', 'td', function (e) {
                        let rowData = tblRequisiciones.DataTable().row($(this).closest('tr')).data();

                        tblRequisiciones.find('tr').removeClass('requisicionSeleccionada');
                        $(this).closest('tr').addClass('requisicionSeleccionada');

                        let cuadro = {
                            cc: rowData.cc,
                            numero: rowData.numero,
                            folio: rowData.folio,
                            PERU_tipoRequisicion: rowData.PERU_tipoRequisicion,
                        }

                        btnImprimirCuadro.attr('disabled', true);

                        $.blockUI({ message: 'Procesando...' });
                        $.post('/Enkontrol/OrdenCompra/GetCuadroDet', { cuadro })
                            .always($.unblockUI)
                            .then(response => {
                                if (response.success) {
                                    limpiarCuadro();

                                    let cuadro = response.data;
                                    let cuadroDetalle = response.data.detalleCuadro.filter(function (x) { return x.cantidad > 0 });

                                    if (cuadro.tieneCuadro) {
                                        btnGuardarCuadro.attr('disabled', false); // btnGuardarCuadro.attr('disabled', true);
                                        btnAgregarRenglonCuadro.attr('disabled', false);
                                        btnImprimirCuadro.attr('disabled', false);

                                        llenarPanelDerechoProv(cuadro);
                                        llenarTablaPartidas(cuadro, cuadroDetalle);

                                        tblPartidas.find('tbody tr:eq(0)').click();

                                        _tipoCambioPeru = cuadro.PERU_tipoCambio.toFixed(2);

                                    } else {
                                        btnGuardarCuadro.attr('disabled', false);
                                        btnAgregarRenglonCuadro.attr('disabled', true);

                                        llenarTablaPartidas(cuadro, cuadroDetalle);

                                        tblPartidas.find('tbody tr:eq(0)').click();

                                        inputLABProv1Num.val(cuadro.lab1);
                                        inputLABProv1Desc.val(cuadro.lab1Desc);
                                        inputLABProv2Num.val(cuadro.lab2);
                                        inputLABProv2Desc.val(cuadro.lab2Desc);
                                        inputLABProv3Num.val(cuadro.lab3);
                                        inputLABProv3Desc.val(cuadro.lab3Desc);

                                        _tipoCambioPeru = cuadro.PERU_tipoCambio.toFixed(2);

                                        //#region LABEL UPDATE PERU
                                        if (cboTipoMonedaProv1.val() == "2") {

                                            let numTCPeru = typeof (_tipoCambioPeru) == "string" ? Number(_tipoCambioPeru) : _tipoCambioPeru

                                            inputTipoCambioProv1.val("$" + numTCPeru.toFixed(2));

                                        } else {

                                            inputTipoCambioProv1.val('$1.00');

                                        }

                                        if (cboTipoMonedaProv2.val() == "2") {

                                            let numTCPeru = typeof (_tipoCambioPeru) == "string" ? Number(_tipoCambioPeru) : _tipoCambioPeru

                                            inputTipoCambioProv2.val("$" + numTCPeru.toFixed(2));

                                        } else {

                                            inputTipoCambioProv2.val('$1.00');

                                        }

                                        if (cboTipoMonedaProv3.val() == "2") {

                                            let numTCPeru = typeof (_tipoCambioPeru) == "string" ? Number(_tipoCambioPeru) : _tipoCambioPeru

                                            inputTipoCambioProv3.val("$" + numTCPeru.toFixed(2));

                                        } else {

                                            inputTipoCambioProv3.val('$1.00');

                                        }
                                        //#endregion
                                    }

                                    // if (rowData.tieneCompra) {
                                    //     btnGuardarCuadro.attr('disabled', true);
                                    //     btnAgregarRenglonCuadro.attr('disabled', true);
                                    // }

                                    if (cuadro.rutaArchivo1 != null) {
                                        botonDescargarArchivo1.show();
                                        botonDescargarArchivo1.data('rutaArchivo', cuadro.rutaArchivo1);
                                    } else {
                                        botonDescargarArchivo1.hide();
                                        botonDescargarArchivo1.data('rutaArchivo', null);
                                    }

                                    if (cuadro.rutaArchivo2 != null) {
                                        botonDescargarArchivo2.show();
                                        botonDescargarArchivo2.data('rutaArchivo', cuadro.rutaArchivo2);
                                    } else {
                                        botonDescargarArchivo2.hide();
                                        botonDescargarArchivo2.data('rutaArchivo', null);
                                    }

                                    if (cuadro.rutaArchivo3 != null) {
                                        botonDescargarArchivo3.show();
                                        botonDescargarArchivo3.data('rutaArchivo', cuadro.rutaArchivo3);
                                    } else {
                                        botonDescargarArchivo3.hide();
                                        botonDescargarArchivo3.data('rutaArchivo', null);
                                    }
                                } else {
                                    AlertaGeneral('Alerta', 'Error al consultar la información.');
                                    btnAgregarRenglonCuadro.attr('disabled', true);

                                    limpiarCuadro();
                                }
                            }, error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            }
                            );
                    });

                    tblRequisiciones.on('click', '.btn-lista-compras', function () {
                        let rowData = tblRequisiciones.DataTable().row($(this).closest('tr')).data();

                        dialogListaCompras.text(rowData.listNumeroCompra.join(', '));

                        dialogListaCompras.dialog({
                            width: '30%',
                            modal: true,
                        });
                    });

                    tblRequisiciones.on('click', '.botonBorrarCuadro', function () {
                        let rowData = tblRequisiciones.DataTable().row($(this).closest('tr')).data();
                        let folio = rowData.folio < 10 ? ('0' + rowData.folio) : rowData.folio;

                        AlertaAceptarRechazarNormal(
                            'Confirmar Eliminación',
                            `¿Está seguro de eliminar el cuadro comparativo de la requisición "${rowData.cc}-${rowData.numero}" con el folio "${folio}"?`,
                            () => borrarCuadro(rowData)
                        );
                    });
                },
                createdRow: function (row, rowData) {
                    if (rowData.tieneCompra) {
                        $(row).addClass('renglonTieneCompra');
                    }
                },
                columns: [
                    { data: 'ccNumeroCompuesto', title: 'Requisición' },
                    {
                        data: 'folio', render: function (data) {
                            return data < 10 ? ('0' + data) : data;
                        }, title: 'Folio'
                    },
                    { data: 'fecha_requisicion', title: 'Fecha Req.' },
                    {
                        data: 'fecha_cuadro', render: function (data, type, row) {
                            if (row.tieneCuadro) {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            } else {
                                return '';
                            }
                        }, title: 'Fecha Cuadro'
                    },
                    {
                        data: 'insumosLicitados',
                        render: function (data, type, row) {
                            if (row.tieneCuadro) {
                                if (data) {
                                    return 'SÍ';
                                } else {
                                    return 'NO';
                                }
                            } else {
                                return '';
                            }
                        }, title: 'Insumos Licitados'
                    },
                    {
                        data: 'ccNumeroCompraCompuesto', title: 'Orden Compra', render: function (data, type, row, meta) {
                            if (row.listNumeroCompra.length > 1) {
                                var button = `<button class="btn btn-xs btn-default btn-lista-compras"><i class="fa fa-align-justify"></i></button>`;

                                return button;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        title: '', render: function (data, type, row, meta) {
                            return row.puedeEliminar ? `<button class="btn btn-xs btn-danger botonBorrarCuadro"><i class="fa fa-times"></i></button>` : ``;
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
                        targets: [2, 3]
                    },
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablePartidas() {
            tblPartidas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                aaSorting: [0, 'asc'],
                // scrollY: "400px",
                scrollCollapse: true,
                bInfo: false,
                ordering: false,
                initComplete: function (settings, json) {
                    tblPartidas.on('click', 'tbody tr', function (e) {
                        let rowData = tblPartidas.DataTable().row($(this).closest('tr')).data();

                        tblPartidas.find('tr').removeClass('partidaSeleccionada');
                        $(this).closest('tr').addClass('partidaSeleccionada');

                        let partidaCuadro = {
                            cc: rowData.cc,
                            numero: rowData.numero,
                            folio: rowData.folio,
                            partida: rowData.partida,
                            insumo: rowData.insumo
                        }

                        $.post('/Enkontrol/OrdenCompra/GetUltimaCompra', { partidaCuadro })
                            .then(response => {
                                if (response.success) {
                                    limpiarUltimaCompra();

                                    if (response.data.folioOC != null) {
                                        let listaUltimaCompra = [];
                                        listaUltimaCompra.push(response.data);

                                        AddRows(tblUltimaCompra, listaUltimaCompra);
                                    } else {
                                        limpiarUltimaCompra();
                                    }
                                } else {
                                    AlertaGeneral('Alerta', 'Error al consultar la información.');
                                    limpiarUltimaCompra();
                                }
                            }, error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            }
                            );
                    });

                    tblPartidas.on('focus', 'input', function () {
                        $(this).select();
                    });

                    tblPartidas.on('change', '.inputCantidad', function () {
                        let rowData = tblPartidas.DataTable().row($(this).closest('tr')).data();
                        let valor = unmaskNumero6D($(this).val());

                        if (rowData.cantidad < valor) {
                            AlertaGeneral(`Alerta`, `No puede colocar una cantidad mayor a lo requerido.`);

                            $(this).val(unmaskNumero6D(rowData.cantidad.toString()));
                        }
                    });
                },
                columns: [
                    { data: 'partida', title: 'Pda' },
                    {
                        data: 'insumo',
                        render: function (data, type, row) {
                            return row.insumo + '-' + row.descripcion + ` (${row.partidaDesc})`;
                        },
                        title: 'Insumo'
                    },
                    {
                        data: 'cantidad',
                        render: function (data, type, row) {
                            let div = document.createElement('div');
                            let input = document.createElement('input');
                            let span = document.createElement('label');

                            div.classList.add('input-group');
                            input.classList.add('form-control', 'text-right', 'inputCantidad');
                            span.classList.add('input-group-addon');

                            _countIndex++;

                            $(input).attr('tabindex', _countIndex);
                            _countIncremento = _countIndex;

                            if (row.cant_ordenada > 0) {
                                $(input).attr('disabled', true);
                            }

                            $(input).attr('value', unmaskNumero6DCompras(row.cantidad.toString()));
                            $(span).text(row.unidad);

                            $(div).append(input);
                            $(div).append(span);

                            return div.outerHTML;
                        },
                        title: 'Cantidad'
                    },
                    {
                        defaultContent: '',
                        render: function (data, type, row) {
                            let div = document.createElement('div');
                            let input = document.createElement('input');
                            let span = document.createElement('label');

                            div.classList.add('input-group');
                            input.classList.add('form-control', 'text-right', 'inputProv1');
                            span.classList.add('input-group-addon', 'spanProv1');

                            let valor = !isNaN(row.precio1) ? ('$' + formatMoney(row.precio1)) : '$' + formatMoney(0);

                            _countIncremento += _countTotalFilas;
                            $(input).attr('tabindex', _countIncremento);

                            if (row.cant_ordenada > 0) {
                                $(input).attr('disabled', true);
                            }

                            $(input).attr('value', valor);

                            if (empresaActual.val() == 6) {
                                if (row.moneda1 == 4) {

                                    $(span).text("SOL");

                                } else {
                                    $(span).text("DLS");

                                }

                            } else {
                                $(span).text(row.moneda1);

                            }

                            $(div).append(input);
                            $(div).append(span);

                            return div.outerHTML;
                        }
                    },
                    {
                        defaultContent: '',
                        render: function (data, type, row) {
                            let div = document.createElement('div');
                            let input = document.createElement('input');
                            let span = document.createElement('label');

                            div.classList.add('input-group');
                            input.classList.add('form-control', 'text-right', 'inputProv2');
                            span.classList.add('input-group-addon', 'spanProv2');

                            let valor = !isNaN(row.precio2) ? ('$' + formatMoney(row.precio2)) : '$' + formatMoney(0);

                            _countIncremento += _countTotalFilas;
                            $(input).attr('tabindex', _countIncremento);

                            if (row.cant_ordenada > 0) {
                                $(input).attr('disabled', true);
                            }

                            $(input).attr('value', valor);
                            if (empresaActual.val() == 6) {
                                if (row.moneda2 == 4) {

                                    $(span).text("SOL");

                                } else {
                                    $(span).text("DLS");

                                }

                            } else {
                                $(span).text(row.moneda2);

                            }
                            // $(span).text(row.moneda2);

                            $(div).append(input);
                            $(div).append(span);

                            return div.outerHTML;
                        }
                    },
                    {
                        defaultContent: '',
                        render: function (data, type, row) {
                            let div = document.createElement('div');
                            let input = document.createElement('input');
                            let span = document.createElement('label');

                            div.classList.add('input-group');
                            input.classList.add('form-control', 'text-right', 'inputProv3');
                            span.classList.add('input-group-addon', 'spanProv3');

                            let valor = !isNaN(row.precio3) ? ('$' + formatMoney(row.precio3)) : '$' + formatMoney(0);

                            _countIncremento += _countTotalFilas;
                            $(input).attr('tabindex', _countIncremento);

                            if (row.cant_ordenada > 0) {
                                $(input).attr('disabled', true);
                            }

                            $(input).attr('value', valor);
                            if (empresaActual.val() == 6) {
                                if (row.moneda3 == 4) {

                                    $(span).text("SOL");

                                } else {
                                    $(span).text("DLS");

                                }

                            } else {
                                $(span).text(row.moneda3);

                            }
                            // $(span).text(row.moneda3);

                            $(div).append(input);
                            $(div).append(span);

                            return div.outerHTML;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '30%', targets: [1] },
                    { width: '15%', targets: [2] },
                    { width: '20%', targets: [3, 4, 5] }
                ]
            });
        }

        function initTableUltimaCompra() {
            tblUltimaCompra.DataTable({
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

                },
                columns: [
                    {
                        data: 'proveedor', title: 'Proveedor', render: function (data, type, row, meta) {
                            return row.proveedorNum + '-' + row.proveedorNom;
                        }
                    },
                    { data: 'folioOC', title: 'Orden Compra' },
                    { data: 'fechaString', title: 'Fecha' },
                    {
                        data: 'precio', title: 'Precio', render: function (data, type, row, meta) {
                            return maskNumero6D(row.precio) + ' ' + row.monedaDesc;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            for (let i in lst)
                if (lst.hasOwnProperty(i))
                    AddRow(dt, lst[i]);
        }

        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }

        function llenarPanelDerechoProv(cuadro) {

            if (empresaActual.val() != 6) { // RESTO EMPRESAS
                inputProv1Num.val(cuadro.prov1);
                inputProv2Num.val(cuadro.prov2);
                inputProv3Num.val(cuadro.prov3);

            } else { //PERU
                inputProv1Num.val(cuadro.PERU_prov1);
                inputProv2Num.val(cuadro.PERU_prov2);
                inputProv3Num.val(cuadro.PERU_prov3);

                cboTipoMonedaProv1.val(cuadro.moneda1);
                cboTipoMonedaProv1.change();
                cboTipoMonedaProv2.val(cuadro.moneda2);
                cboTipoMonedaProv2.change();
                cboTipoMonedaProv3.val(cuadro.moneda3);
                cboTipoMonedaProv3.change();
            }

            inputProv1Desc.val(cuadro.nombre_prov1);
            agregarTooltip(inputProv1Desc, cuadro.nombre_prov1);
            inputProv2Desc.val(cuadro.nombre_prov2);
            agregarTooltip(inputProv2Desc, cuadro.nombre_prov2);
            inputProv3Desc.val(cuadro.nombre_prov3);
            agregarTooltip(inputProv3Desc, cuadro.nombre_prov3);

            inputPrimerSubtotalProv1Num.val(maskNumero(cuadro.sub_total1));
            inputPrimerSubtotalProv1Moneda.val(cuadro.moneda1Desc);
            inputPrimerSubtotalProv2Num.val(maskNumero(cuadro.sub_total2));
            inputPrimerSubtotalProv2Moneda.val(cuadro.moneda2Desc);
            inputPrimerSubtotalProv3Num.val(maskNumero(cuadro.sub_total3));
            inputPrimerSubtotalProv3Moneda.val(cuadro.moneda3Desc);

            inputDescuentoProv1.val(maskNumero(cuadro.porcent_dcto1));
            inputDescuentoProv2.val(maskNumero(cuadro.porcent_dcto2));
            inputDescuentoProv3.val(maskNumero(cuadro.porcent_dcto3));

            let segundoSubTotal1 = cuadro.sub_total1 - cuadro.dcto1;
            let segundoSubTotal2 = cuadro.sub_total2 - cuadro.dcto2;
            let segundoSubTotal3 = cuadro.sub_total3 - cuadro.dcto3;

            inputSegundoSubtotalProv1Num.val(maskNumero(segundoSubTotal1));
            inputSegundoSubtotalProv1Moneda.val(cuadro.moneda1Desc);
            inputSegundoSubtotalProv2Num.val(maskNumero(segundoSubTotal2));
            inputSegundoSubtotalProv2Moneda.val(cuadro.moneda2Desc);
            inputSegundoSubtotalProv3Num.val(maskNumero(segundoSubTotal3));
            inputSegundoSubtotalProv3Moneda.val(cuadro.moneda3Desc);

            inputIVAProv1.val(cuadro.porcent_iva1);
            inputIVAProv2.val(cuadro.porcent_iva2);
            inputIVAProv3.val(cuadro.porcent_iva3);

            inputTotalProv1Num.val(maskNumero(cuadro.total1));
            inputTotalProv1Moneda.val(cuadro.moneda1Desc);
            inputTotalProv2Num.val(maskNumero(cuadro.total2));
            inputTotalProv2Moneda.val(cuadro.moneda2Desc);
            inputTotalProv3Num.val(maskNumero(cuadro.total3));
            inputTotalProv3Moneda.val(cuadro.moneda3Desc);

            inputFletesProv1Num.val(maskNumero(cuadro.fletes1));
            inputFletesProv1Moneda.val(cuadro.moneda1Desc);
            inputFletesProv2Num.val(maskNumero(cuadro.fletes2));
            inputFletesProv2Moneda.val(cuadro.moneda2Desc);
            inputFletesProv3Num.val(maskNumero(cuadro.fletes3));
            inputFletesProv3Moneda.val(cuadro.moneda3Desc);

            inputImportacionProv1Num.val(maskNumero(cuadro.gastos_imp1));
            inputImportacionProv1Moneda.val(cuadro.moneda1Desc);
            inputImportacionProv2Num.val(maskNumero(cuadro.gastos_imp2));
            inputImportacionProv2Moneda.val(cuadro.moneda2Desc);
            inputImportacionProv3Num.val(maskNumero(cuadro.gastos_imp3));
            inputImportacionProv3Moneda.val(cuadro.moneda3Desc);

            let granTotal1 = cuadro.total1 + cuadro.fletes1 + cuadro.gastos_imp1;
            let granTotal2 = cuadro.total2 + cuadro.fletes2 + cuadro.gastos_imp2;
            let granTotal3 = cuadro.total3 + cuadro.fletes3 + cuadro.gastos_imp3;

            let granTotal1PERU = cuadro.moneda1 == 2 ? ((cuadro.total1 * cuadro.tipo_cambio1) + (cuadro.fletes1 * cuadro.tipo_cambio1) + (cuadro.gastos_imp1 * cuadro.tipo_cambio1)) : (cuadro.total1 + cuadro.fletes1 + cuadro.gastos_imp1);
            let granTotal2PERU = cuadro.moneda2 == 2 ? ((cuadro.total2 * cuadro.tipo_cambio2) + (cuadro.fletes2 * cuadro.tipo_cambio2) + (cuadro.gastos_imp2 * cuadro.tipo_cambio2)) : (cuadro.total2 + cuadro.fletes2 + cuadro.gastos_imp2);
            let granTotal3PERU = cuadro.moneda3 == 2 ? ((cuadro.total3 * cuadro.tipo_cambio3) + (cuadro.fletes3 * cuadro.tipo_cambio3) + (cuadro.gastos_imp3 * cuadro.tipo_cambio3)) : (cuadro.total3 + cuadro.fletes3 + cuadro.gastos_imp3);

            inputGranTotalProv1Num.val(maskNumero(granTotal1));
            inputGranTotalProv1Moneda.val(cuadro.moneda1Desc);
            inputGranTotalProv2Num.val(maskNumero(granTotal2));
            inputGranTotalProv2Moneda.val(cuadro.moneda2Desc);
            inputGranTotalProv3Num.val(maskNumero(granTotal3));
            inputGranTotalProv3Moneda.val(cuadro.moneda3Desc);

            inputGranTotalProv1NumPERU.val(maskNumero(granTotal1PERU));
            inputGranTotalProv2NumPERU.val(maskNumero(granTotal2PERU));
            inputGranTotalProv3NumPERU.val(maskNumero(granTotal3PERU));

            inputTipoCambioProv1.val(maskNumero(cuadro.tipo_cambio1));
            inputTipoCambioProv2.val(maskNumero(cuadro.tipo_cambio2));
            inputTipoCambioProv3.val(maskNumero(cuadro.tipo_cambio3));

            inputFechaEntregaProv1.val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(cuadro.fecha_entrega1.substr(6)))));
            inputFechaEntregaProv2.val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(cuadro.fecha_entrega2.substr(6)))));
            inputFechaEntregaProv3.val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(cuadro.fecha_entrega3.substr(6)))));

            inputLABProv1Num.val(cuadro.lab1);
            inputLABProv1Desc.val(cuadro.lab1Desc);
            inputLABProv2Num.val(cuadro.lab2);
            inputLABProv2Desc.val(cuadro.lab2Desc);
            inputLABProv3Num.val(cuadro.lab3);
            inputLABProv3Desc.val(cuadro.lab3Desc);

            inputCondPagoProv1.val(cuadro.dias_pago1);
            inputCondPagoProv2.val(cuadro.dias_pago2);
            inputCondPagoProv3.val(cuadro.dias_pago3);

            textAreaComentarioProv1.val(cuadro.comentarios1);
            textAreaComentarioProv2.val(cuadro.comentarios2);
            textAreaComentarioProv3.val(cuadro.comentarios3);
        }

        function llenarTablaPartidas(cuadro, cuadroDetalle) {
            let tablaPartidas = tblPartidas.DataTable();

            tablaPartidas.columns(3).header().to$().text(cuadro.nombre_prov1);
            tablaPartidas.columns(4).header().to$().text(cuadro.nombre_prov2);
            tablaPartidas.columns(5).header().to$().text(cuadro.nombre_prov3);

            _countTotalFilas = cuadroDetalle.length;

            AddRows(tblPartidas, cuadroDetalle);
        }

        function limpiarCuadro() {
            inputProv1Num.val('');
            inputProv1Desc.val('');
            quitarTooltip(inputProv1Desc);
            inputProv2Num.val('');
            inputProv2Desc.val('');
            quitarTooltip(inputProv2Desc);
            inputProv3Num.val('');
            inputProv3Desc.val('');
            quitarTooltip(inputProv3Desc);

            inputPrimerSubtotalProv1Num.val('$0.00');
            inputPrimerSubtotalProv1Moneda.val('');
            inputPrimerSubtotalProv2Num.val('$0.00');
            inputPrimerSubtotalProv2Moneda.val('');
            inputPrimerSubtotalProv3Num.val('$0.00');
            inputPrimerSubtotalProv3Moneda.val('');

            inputDescuentoProv1.val('0.00');
            inputDescuentoProv2.val('0.00');
            inputDescuentoProv3.val('0.00');

            inputSegundoSubtotalProv1Num.val('$0.00');
            inputSegundoSubtotalProv1Moneda.val('');
            inputSegundoSubtotalProv2Num.val('$0.00');
            inputSegundoSubtotalProv2Moneda.val('');
            inputSegundoSubtotalProv3Num.val('$0.00');
            inputSegundoSubtotalProv3Moneda.val('');

            inputIVAProv1.val('');
            inputIVAProv2.val('');
            inputIVAProv3.val('');

            inputTotalProv1Num.val('$0.00');
            inputTotalProv1Moneda.val('');
            inputTotalProv2Num.val('$0.00');
            inputTotalProv2Moneda.val('');
            inputTotalProv3Num.val('$0.00');
            inputTotalProv3Moneda.val('');

            inputFletesProv1Num.val('$0.00');
            inputFletesProv1Moneda.val('');
            inputFletesProv2Num.val('$0.00');
            inputFletesProv2Moneda.val('');
            inputFletesProv3Num.val('$0.00');
            inputFletesProv3Moneda.val('');

            inputImportacionProv1Num.val('$0.00');
            inputImportacionProv1Moneda.val('');
            inputImportacionProv2Num.val('$0.00');
            inputImportacionProv2Moneda.val('');
            inputImportacionProv3Num.val('$0.00');
            inputImportacionProv3Moneda.val('');

            inputGranTotalProv1Num.val('$0.00');
            inputGranTotalProv1Moneda.val('');
            inputGranTotalProv2Num.val('$0.00');
            inputGranTotalProv2Moneda.val('');
            inputGranTotalProv3Num.val('$0.00');
            inputGranTotalProv3Moneda.val('');

            inputTipoCambioProv1.val('$1.00');
            inputTipoCambioProv2.val('$1.00');
            inputTipoCambioProv3.val('$1.00');

            let hoy = new Date().toLocaleDateString('es-ES');

            inputFechaEntregaProv1.datepicker().datepicker("setDate", hoy);
            inputFechaEntregaProv2.datepicker().datepicker("setDate", hoy);
            inputFechaEntregaProv3.datepicker().datepicker("setDate", hoy);

            inputLABProv1Num.val('');
            inputLABProv1Desc.val('');
            inputLABProv2Num.val('');
            inputLABProv2Desc.val('');
            inputLABProv3Num.val('');
            inputLABProv3Desc.val('');

            inputCondPagoProv1.val('');
            inputCondPagoProv2.val('');
            inputCondPagoProv3.val('');

            textAreaComentarioProv1.val('');
            textAreaComentarioProv2.val('');
            textAreaComentarioProv3.val('');

            let tablaPartidas = tblPartidas.DataTable();

            tablaPartidas.columns(3).header().to$().text('');
            tablaPartidas.columns(4).header().to$().text('');
            tablaPartidas.columns(5).header().to$().text('');

            tablaPartidas.clear().draw();
        }

        function calculoProveedor1() {
            let tipoCambio = unmaskNumero(inputTipoCambioProv1.val());
            let primerSubTotal = 0;

            tblPartidas.find("tbody tr").each(function (idx, row) {
                let cantidad = unmaskNumero($(row).find('.inputCantidad').val());
                let precio1 = unmaskNumero($(row).find('.inputProv1').val());

                primerSubTotal += cantidad * precio1;
            });

            inputPrimerSubtotalProv1Num.val(maskNumero(primerSubTotal));

            let descuento = unmaskNumero(inputDescuentoProv1.val());
            let segundoSubTotal = primerSubTotal - (primerSubTotal * (descuento / 100));

            inputSegundoSubtotalProv1Num.val(maskNumero(segundoSubTotal));

            let iva = inputIVAProv1.val() != '' ? unmaskNumero(inputIVAProv1.val()) : 0;
            let total = segundoSubTotal + (segundoSubTotal * (iva / 100));

            inputTotalProv1Num.val(maskNumero(total));

            let fletes = unmaskNumero(inputFletesProv1Num.val());
            let gastosImportacion = unmaskNumero(inputImportacionProv1Num.val());
            let granTotal = total + fletes + gastosImportacion;

            inputGranTotalProv1Num.val(maskNumero(granTotal));

            if (empresaActual.val() == 6) {
                if (cboTipoMonedaProv1.val() == "2") {
                    inputGranTotalProv1NumPERU.val(maskNumero(granTotal * _tipoCambioPeru));

                } else {
                    inputGranTotalProv1NumPERU.val(maskNumero(granTotal));

                }

            }
        }

        function calculoProveedor2() {
            let tipoCambio = unmaskNumero(inputTipoCambioProv2.val());
            let primerSubTotal = 0;

            tblPartidas.find("tbody tr").each(function (idx, row) {
                let cantidad = unmaskNumero($(row).find('.inputCantidad').val());
                let precio2 = unmaskNumero($(row).find('.inputProv2').val());

                primerSubTotal += cantidad * precio2;
            });

            inputPrimerSubtotalProv2Num.val(maskNumero(primerSubTotal));

            let descuento = unmaskNumero(inputDescuentoProv2.val());
            let segundoSubTotal = primerSubTotal - (primerSubTotal * (descuento / 100));

            inputSegundoSubtotalProv2Num.val(maskNumero(segundoSubTotal));

            let iva = inputIVAProv2.val() != '' ? unmaskNumero(inputIVAProv2.val()) : 0;
            let total = segundoSubTotal + (segundoSubTotal * (iva / 100));

            inputTotalProv2Num.val(maskNumero(total));

            let fletes = unmaskNumero(inputFletesProv2Num.val());
            let gastosImportacion = unmaskNumero(inputImportacionProv2Num.val());
            let granTotal = total + fletes + gastosImportacion;

            inputGranTotalProv2Num.val(maskNumero(granTotal));

            if (empresaActual.val() == 6) {
                if (cboTipoMonedaProv2.val() == "2") {
                    inputGranTotalProv2NumPERU.val(maskNumero(granTotal * _tipoCambioPeru));

                } else {
                    inputGranTotalProv2NumPERU.val(maskNumero(granTotal));

                }

            }
        }

        function calculoProveedor3() {
            let tipoCambio = unmaskNumero(inputTipoCambioProv3.val());
            let primerSubTotal = 0;

            tblPartidas.find("tbody tr").each(function (idx, row) {
                let cantidad = unmaskNumero($(row).find('.inputCantidad').val());
                let precio3 = unmaskNumero($(row).find('.inputProv3').val());

                primerSubTotal += cantidad * precio3;
            });

            inputPrimerSubtotalProv3Num.val(maskNumero(primerSubTotal));

            let descuento = unmaskNumero(inputDescuentoProv3.val());
            let segundoSubTotal = primerSubTotal - (primerSubTotal * (descuento / 100));

            inputSegundoSubtotalProv3Num.val(maskNumero(segundoSubTotal));

            let iva = inputIVAProv3.val() != '' ? unmaskNumero(inputIVAProv3.val()) : 0;
            let total = segundoSubTotal + (segundoSubTotal * (iva / 100));

            inputTotalProv3Num.val(maskNumero(total));

            let fletes = unmaskNumero(inputFletesProv3Num.val());
            let gastosImportacion = unmaskNumero(inputImportacionProv3Num.val());
            let granTotal = total + fletes + gastosImportacion;

            inputGranTotalProv3Num.val(maskNumero(granTotal));

            if (empresaActual.val() == 6) {
                if (cboTipoMonedaProv3.val() == "2") {
                    inputGranTotalProv3NumPERU.val(maskNumero(granTotal * _tipoCambioPeru));

                } else {
                    inputGranTotalProv3NumPERU.val(maskNumero(granTotal));

                }

            }
        }

        function getNuevoCuadro() {
            let requisicionRowData = tblRequisiciones.DataTable().row(tblRequisiciones.find('.requisicionSeleccionada')).data();
            let detalleCuadro = [];
            let hoy = new Date().toLocaleDateString('es-ES');

            tblPartidas.find("tbody tr").each(function (idx, row) {
                let partidaRowData = tblPartidas.DataTable().row(idx).data();

                detalleCuadro.push({
                    cc: requisicionRowData.cc,
                    numero: requisicionRowData.numero,
                    folio: requisicionRowData.folio == 0 ? requisicionRowData.folio + 1 : requisicionRowData.folio,
                    partida: partidaRowData.partida,
                    insumo: partidaRowData.insumo,
                    cantidad: unmaskNumero($(row).find('.inputCantidad').val()),
                    precio1: unmaskNumero($(row).find('.inputProv1').val()),
                    precio2: unmaskNumero($(row).find('.inputProv2').val()),
                    precio3: unmaskNumero($(row).find('.inputProv3').val()),
                    proveedor_uc: null,
                    oc_uc: null,
                    fecha_uc: null,
                    precio_uc: null
                });
            });

            let cuadro = {
                cc: requisicionRowData.cc,
                numero: requisicionRowData.numero,
                folio: requisicionRowData.folio == 0 ? requisicionRowData.folio + 1 : requisicionRowData.folio,
                fecha: hoy,

                prov1: inputProv1Num.val(),
                porcent_dcto1: unmaskNumero(inputDescuentoProv1.val()),
                porcent_iva1: unmaskNumero(inputIVAProv1.val()),
                dcto1: unmaskNumero(inputPrimerSubtotalProv1Num.val()) * (unmaskNumero(inputDescuentoProv1.val()) / 100),
                iva1: unmaskNumero(inputSegundoSubtotalProv1Num.val()) * (unmaskNumero(inputIVAProv1.val()) / 100),
                total1: unmaskNumero(inputPrimerSubtotalProv1Num.val()) -
                    unmaskNumero(inputPrimerSubtotalProv1Num.val()) * (unmaskNumero(inputDescuentoProv1.val()) / 100) +
                    unmaskNumero(inputSegundoSubtotalProv1Num.val()) * (unmaskNumero(inputIVAProv1.val()) / 100),
                tipo_cambio1: unmaskNumero(inputTipoCambioProv1.val()),
                fecha_entrega1: inputFechaEntregaProv1.val(),
                lab1: inputLABProv1Num.val(),
                dias_pago1: inputCondPagoProv1.val(),

                prov2: inputProv2Num.val(),
                porcent_dcto2: unmaskNumero(inputDescuentoProv2.val()),
                porcent_iva2: unmaskNumero(inputIVAProv2.val()),
                dcto2: unmaskNumero(inputPrimerSubtotalProv2Num.val()) * (unmaskNumero(inputDescuentoProv2.val()) / 100),
                iva2: unmaskNumero(inputSegundoSubtotalProv2Num.val()) * (unmaskNumero(inputIVAProv2.val()) / 100),
                total2: unmaskNumero(inputPrimerSubtotalProv2Num.val()) -
                    unmaskNumero(inputPrimerSubtotalProv2Num.val()) * (unmaskNumero(inputDescuentoProv2.val()) / 100) +
                    unmaskNumero(inputSegundoSubtotalProv2Num.val()) * (unmaskNumero(inputIVAProv2.val()) / 100),
                tipo_cambio2: unmaskNumero(inputTipoCambioProv2.val()),
                fecha_entrega2: inputFechaEntregaProv2.val(),
                lab2: inputLABProv2Num.val(),
                dias_pago2: inputCondPagoProv2.val(),

                prov3: inputProv3Num.val(),
                porcent_dcto3: unmaskNumero(inputDescuentoProv3.val()),
                porcent_iva3: unmaskNumero(inputIVAProv3.val()),
                dcto3: unmaskNumero(inputPrimerSubtotalProv3Num.val()) * (unmaskNumero(inputDescuentoProv3.val()) / 100),
                iva3: unmaskNumero(inputSegundoSubtotalProv3Num.val()) * (unmaskNumero(inputIVAProv3.val()) / 100),
                total3: unmaskNumero(inputPrimerSubtotalProv3Num.val()) -
                    unmaskNumero(inputPrimerSubtotalProv3Num.val()) * (unmaskNumero(inputDescuentoProv3.val()) / 100) +
                    unmaskNumero(inputSegundoSubtotalProv3Num.val()) * (unmaskNumero(inputIVAProv3.val()) / 100),
                tipo_cambio3: unmaskNumero(inputTipoCambioProv3.val()),
                fecha_entrega3: inputFechaEntregaProv3.val(),
                lab3: inputLABProv3Num.val(),
                dias_pago3: inputCondPagoProv3.val(),

                solicito: inputSolicita.val(),

                sub_total1: unmaskNumero(inputPrimerSubtotalProv1Num.val()),
                sub_total2: unmaskNumero(inputPrimerSubtotalProv2Num.val()),
                sub_total3: unmaskNumero(inputPrimerSubtotalProv3Num.val()),

                fletes1: unmaskNumero(inputFletesProv1Num.val()),
                fletes2: unmaskNumero(inputFletesProv2Num.val()),
                fletes3: unmaskNumero(inputFletesProv3Num.val()),

                gastos_imp1: unmaskNumero(inputImportacionProv1Num.val()),
                gastos_imp2: unmaskNumero(inputImportacionProv2Num.val()),
                gastos_imp3: unmaskNumero(inputImportacionProv3Num.val()),

                nombre_prov1: inputProv1Desc.val(),
                nombre_prov2: inputProv2Desc.val(),
                nombre_prov3: inputProv3Desc.val(),


                moneda1: empresaActual.val() != 6 ? inputProv1Num.attr('monedaProv') : cboTipoMonedaProv1.val(),
                moneda2: empresaActual.val() != 6 ? inputProv2Num.attr('monedaProv') : cboTipoMonedaProv2.val(),
                moneda3: empresaActual.val() != 6 ? inputProv1Num.attr('monedaProv') : cboTipoMonedaProv3.val(),

                inslic: false,
                inslic_fecha_ini: null,
                inslic_fecha_fin: null,

                comentarios1: textAreaComentarioProv1.val(),
                comentarios2: textAreaComentarioProv2.val(),
                comentarios3: textAreaComentarioProv3.val(),

                detalleCuadro: detalleCuadro,

                PERU_prov1: inputProv1Num.val(),
                PERU_prov2: inputProv2Num.val(),
                PERU_prov3: inputProv3Num.val(),
            };

            return cuadro;
        }

        function limpiarUltimaCompra() {
            tblUltimaCompra.DataTable().clear().draw();
        }

        function setProveedor1(e, ui) {
            inputProv1Num.val(ui.item.id);
            inputProv1Desc.val(ui.item.label);
            inputCondPagoProv1.val(ui.item.PERU_formaPago);
            agregarTooltip(inputProv1Desc, ui.item.label);

            inputProv1Num.change();
        }

        function setProveedor2(e, ui) {
            inputProv2Num.val(ui.item.id);
            inputProv2Desc.val(ui.item.label);
            inputCondPagoProv2.val(ui.item.PERU_formaPago);
            agregarTooltip(inputProv2Desc, ui.item.label);

            inputProv2Num.change();
        }

        function setProveedor3(e, ui) {
            inputProv3Num.val(ui.item.id);
            inputProv3Desc.val(ui.item.label);
            inputCondPagoProv3.val(ui.item.PERU_formaPago);
            agregarTooltip(inputProv3Desc, ui.item.label);

            inputProv3Num.change();
        }

        function verReporte() {
            let dataRenglonActivo = tblRequisiciones.DataTable().row(tblRequisiciones.find('tbody .requisicionSeleccionada')).data();

            let cc = dataRenglonActivo.cc;
            let numero = dataRenglonActivo.numero;
            let folio = dataRenglonActivo.folio;

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", '/Reportes/Vista.aspx?idReporte=117' + '&cc=' + cc + '&numero=' + numero + '&folio=' + folio);
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
        }

        function borrarCuadro(rowData) {
            let cuadro = {
                cc: rowData.cc,
                numero: rowData.numero,
                folio: rowData.folio,
                detalleCuadro: rowData.detalleCuadro
            };

            axios.post('/Enkontrol/OrdenCompra/BorrarCuadro', { cuadro }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha eliminado el cuadro comparativo.');

                    limpiarCuadro();
                    limpiarUltimaCompra();

                    btnBuscarCuadro.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        const getUrlParams = function (url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            return params;
        };

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

        function fixVerticalTabindex(selector) {
            var tabindex = 1;
            $(selector).each(function (i, tbl) {
                $(tbl).find('tr').first().find('td').each(function (clmn, el) {
                    $(tbl).find('tr td:nth-child(' + (clmn + 1) + ') input').each(function (j, input) {
                        $(input).attr('placeholder', tabindex);
                        $(input).attr('tabindex', tabindex++);
                    });
                });
            });
        }

        function getContadorRequisicionesPendientes() {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/GetContadorRequisicionesPendientes')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        labelRequisicionesPendientes.text('Requisiciones sin O.C.: ' + response.data);
                    } else {
                        labelRequisicionesPendientes.text('');
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function verificarComprasPeru() {
            if (_empresaActual == 6) {
                $(".labelIVA").html("I.G.V:");
            } else {
                $(".labelIVA").html("I.V.A:");
            }
        }

        init();
    }
    $(document).ready(function () {
        Enkontrol.Compras.OrdenCompra.CuadroComparativo = new CuadroComparativo();
    })
})();