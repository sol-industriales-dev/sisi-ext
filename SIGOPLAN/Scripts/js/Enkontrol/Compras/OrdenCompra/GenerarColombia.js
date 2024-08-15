(() => {
    $.namespace('Enkontrol.OrdenCompra.Generar');
    Generar = function () {

        //#region Selectores
        const tblPartidas = $('#tblPartidas');
        const tblPagos = $('#tblPagos');
        const tblRetenciones = $('#tblRetenciones');
        const mdlPagos = $('#mdlPagos');
        const mdlRetenciones = $('#mdlRetenciones');
        const textAreaDescPartida = $('#textAreaDescPartida');
        const btnAgregarRetencion = $('#btnAgregarRetencion');
        const btnQuitarRetencion = $('#btnQuitarRetencion');
        const btnAgregarInsumo = $('#btnAgregarInsumo');
        const btnQuitarInsumo = $('#btnQuitarInsumo');
        const btnAgregarInsumoEditar = $('#btnAgregarInsumoEditar');
        const btnQuitarInsumoEditar = $('#btnQuitarInsumoEditar');
        const btnCancelarCompra = $('#btnCancelarCompra');

        const mdlCuadroComparativo = $('#mdlCuadroComparativo');
        const tblPartidasCuadro = $('#tblPartidasCuadro');
        const tblUltimaCompra = $('#tblUltimaCompra');
        const btnVerCuadro = $('#btnVerCuadro');
        const btnNoTieneCuadro = $('#btnNoTieneCuadro');
        const selectFolioCuadro = $('#selectFolioCuadro');
        const btnImprimir = $('#btnImprimir');
        const report = $("#report");
        const btnSeleccionarProv1 = $('#btnSeleccionarProv1');
        const btnSeleccionarProv2 = $('#btnSeleccionarProv2');
        const btnSeleccionarProv3 = $('#btnSeleccionarProv3');
        const labelRequisicionesPendientes = $('#labelRequisicionesPendientes');
        const btnBorrarCompra = $('#btnBorrarCompra');
        const dialogConfirmarBorrado = $('#dialogConfirmarBorrado');
        const mdlCatalogoRetenciones = $('#mdlCatalogoRetenciones');
        const tblCatalogoRetenciones = $('#tblCatalogoRetenciones');
        //#region Panel Izquierdo
        selectCC = $('#selectCC');
        inputNumero = $('#inputNumero');

        //Selectores para Editar Compra
        selectCCEditar = $('#selectCCEditar');
        inputNumeroEditar = $('#inputNumeroEditar');
        btnGuardarCambios = $('#btnGuardarCambios');
        const tblPartidasEditar = $('#tblPartidasEditar');

        selectBoS = $('#selectBoS');
        const dtpFecha = $('#dtpFecha');
        const inputProvNum = $('#inputProvNum');
        const inputProvNom = $('#inputProvNom');
        const inputCompNum = $('#inputCompNum');
        const inputCompNom = $('#inputCompNom');
        const inputSolNum = $('#inputSolNum');
        const inputSolNom = $('#inputSolNom');
        const inputAutNum = $('#inputAutNum');
        const inputAutNom = $('#inputAutNom');
        inputEmb = $('#inputEmb');
        selectLab = $('#selectLab');
        const inputConFact = $('#inputConFact');
        checkAutoRecep = $('#checkAutoRecep');
        inputAlmNum = $('#inputAlmNum');
        const inputAlmNom = $('#inputAlmNom');
        inputEmpNum = $('#inputEmpNum');
        const inputEmpNom = $('#inputEmpNom');
        const checkboxConsigna = $('#checkboxConsigna');
        const checkboxLicitacion = $('#checkboxLicitacion');
        const checkboxCRC = $('#checkboxCRC');
        const checkboxConvenio = $('#checkboxConvenio');
        // const checkboxTMC = $('#checkboxTMC');
        const divAutorecepcionable = $('#divAutorecepcionable');
        const selectCuentaCorriente = $('#selectCuentaCorriente');
        const selectFormaPago = $('#selectFormaPago');
        const selectTipoCompraVenta = $('#selectTipoCompraVenta');
        //#endregion
        const idEmpresa = $('#idEmpresa');

        //#region Panel Derecho
        selectTipoOC = $('#selectTipoOC');
        selectMoneda = $('#selectMoneda');
        inputTipoCambio = $('#inputTipoCambio');
        const inputSubTotal = $('#inputSubTotal');
        inputIVAPorcentaje = $('#inputIVAPorcentaje');
        const inputIVANumero = $('#inputIVANumero');
        const spanIVA = $('#spanIVA')
        const inputRetencion = $('#inputRetencion');
        const inputTotal = $('#inputTotal');
        const inputTotalFinal = $('#inputTotalFinal');

        const btnCondPago = $('#btnCondPago');
        const btnRetenciones = $('#btnRetenciones');
        btnGuardarNuevaCompra = $('#btnGuardarNuevaCompra');
        btnGuardarRetenciones = $('#btnGuardarRetenciones');
        const inputCFDI = $('#inputCFDI');
        const fieldsetEstatusCompra = $('#fieldsetEstatusCompra');
        const labelEstatusCompra = $('#labelEstatusCompra');
        const fieldsetPresupuesto = $('#fieldsetPresupuesto');
        const labelPresupuestoGlobal = $('#labelPresupuestoGlobal');
        const labelPresupuestoActual = $('#labelPresupuestoActual');
        const inputTiempoEntregaDias = $('#inputTiempoEntregaDias');
        const inputTiempoEntregaComentarios = $('#inputTiempoEntregaComentarios');
        const checkboxAnticipo = $('#checkboxAnticipo');
        const inputTotalAnticipo = $('#inputTotalAnticipo');
        const labelAnticipo = $('#labelAnticipo');
        //#endregion

        //#region Elementos Ocultos
        const inputCompradorSesionNum = $('#inputCompradorSesionNum');
        const inputCompradorSesionNom = $('#inputCompradorSesionNom');
        const dtpInicio = $('#dtpInicio');
        const dtpFin = $('#dtpFin');
        const inputEmpresaActual = $('#inputEmpresaActual');
        //#endregion

        //#region Panel Derecho Cuadro
        const divPanelDerecho = $('#divPanelDerecho');
        const inputProv1Num = $('#inputProv1Num');
        const inputProv1Desc = $('#inputProv1Desc');
        const btnEscogerProv1 = $('#btnEscogerProv1');
        const inputProv2Num = $('#inputProv2Num');
        const inputProv2Desc = $('#inputProv2Desc');
        const btnEscogerProv2 = $('#btnEscogerProv2');
        const inputProv3Num = $('#inputProv3Num');
        const inputProv3Desc = $('#inputProv3Desc');
        const btnEscogerProv3 = $('#btnEscogerProv3');

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

        const textAreaComentarioProv1 = $('#textAreaComentarioProv1');
        const textAreaComentarioProv2 = $('#textAreaComentarioProv2');
        const textAreaComentarioProv3 = $('#textAreaComentarioProv3');
        const inputFileCotizacion = $('#inputFileCotizacion');
        const inputCorreoProveedor = $('#inputCorreoProveedor');
        const mdlEnviar = $('#mdlEnviar');
        const btnEnviar = $('#btnEnviar');
        const btnOpenEnviar = $('#btnOpenEnviar');
        //#endregion


        //#endregion
        _Colocada = false;
        _variableCompraNuevaEditar = 0;
        _renglonRetencion = null;
        _validaGlobal = false;
        _flagPeriodoContable = false;
        _flagValidacionInsumoSubcontratista = false;
        _empresaActual = +inputEmpresaActual.val();

        function init() {
            verificarComprasPeru();
            usuarioExiste();
            let hoy = new Date();

            dtpInicio.datepicker().datepicker("setDate", new Date(hoy.getFullYear(), 0, 1));
            dtpFin.datepicker().datepicker("setDate", new Date(hoy.getFullYear(), 11, 31));
            dtpFecha.datepicker().datepicker();

            isComprador();
            initCbo();

            initTablePartidas();
            initTablePartidasEditar();
            initTablePagos();
            initTableRetenciones();
            initTableCatalogoRetenciones();

            initTablePartidasCuadro();
            initTableUltimaCompra();
            // OcultarColumnaAreaCuenta();
            inputProvNum.change(getProveedorPorNumero);
            //inputProvNom.getAutocompleteValid(null, validarProveedor, null, '/Enkontrol/OrdenCompra/GetProveedorNumero');
            inputProvNom.getAutocompleteValid(setProveedor, validarProveedor, null, '/Enkontrol/OrdenCompra/GetProveedorNumero');
            inputCompNum.change(getCompradorPorNumero);
            inputSolNum.change(getSolicitoPorNumero);
            inputAutNum.change(getAutorizoPorNumero);
            inputAlmNum.change(getAlmacen);
            inputEmpNum.change(getEmpleadoRecepPorNumero);

            btnCondPago.click(() => { mdlPagos.modal('show') });
            btnRetenciones.click(() => {
                mdlRetenciones.modal('show');
                $('#labelSubTotal').text('Sub Total: ' + inputSubTotal.val());
            });

            btnBorrarCompra.click(borrarCompra);

            agregarTooltip(btnBorrarCompra, 'Borrar Orden de Compra');
            agregarTooltip(selectLab, 'Dirección de Envío');
            agregarTooltip(btnEscogerProv1, 'Seleccionar Proveedor');
            agregarTooltip(btnEscogerProv2, 'Seleccionar Proveedor');
            agregarTooltip(btnEscogerProv3, 'Seleccionar Proveedor');

            // Se comprueba si hay variables en url.
            const variables = getUrlParams(window.location.href);

            if (variables && variables.cc && variables.numero) {
                selectCC.val(variables.cc);
                inputNumero.val(variables.numero);
                inputNumero.change();
            }

            //getContadorRequisicionesPendientes();
            cargarCatalogoRetenciones();
            // checkPeriodoContable();
            btnOpenEnviar.click(fnOpenEnviar);
            btnEnviar.click(fnEnviarOC);
        }

        function usuarioExiste() {
            axios.post('/OrdenCompra/usuarioCompradorExiste').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    $('#btnOpenEnviar').hide();

                }
                else {
                    $('#btnOpenEnviar').show();
                }
            }).catch(error => Alert2Error(error.message));

        }

        const getComprador = () => $.post('/Enkontrol/OrdenCompra/getComprador');
        const getRequisicion = () => $.post('/Enkontrol/OrdenCompra/GetRequisicion', { cc: selectCC.val(), num: inputNumero.val() != '' ? inputNumero.val() : 0 });
        const guardarNuevaCompra = (compra) => $.post('/Enkontrol/OrdenCompra/GuardarNuevaCompra', { compra: compra });
        const getProveedorInfo = (num, PERU_tipoCambio) => $.post('/Enkontrol/OrdenCompra/GetProveedorInfo', { num, PERU_tipoCambio });
        const getEmpleadoUsuarioEK = (numEmpleado) => $.post('/Enkontrol/OrdenCompra/GetEmpleadoUsuarioEK', { numEmpleado: numEmpleado });
        const getAlmacenNombre = (numAlmacen) => $.post('/Enkontrol/OrdenCompra/GetAlmacenNombre', { numAlmacen: numAlmacen });
        const getRetencionInfo = (id_cpto) => $.post('/Enkontrol/OrdenCompra/GetRetencionInfo', { id_cpto: id_cpto });
        const getCuadroDet = (cuadro) => $.post('/Enkontrol/OrdenCompra/GetCuadroDet', cuadro);
        const getUltimaCompra = (partidaCuadro) => $.post('/Enkontrol/OrdenCompra/GetUltimaCompra', { partidaCuadro: partidaCuadro });

        //Enviar a proveedor
        function fnOpenEnviar() {
            inputFileCotizacion.val("");

            mdlEnviar.modal("show");
        }
        function fnEnviarOC() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            var file = document.getElementById("inputFileCotizacion").files[0];
            if (inputCorreoProveedor.val() != '') {


                report.attr("src", '/Reportes/Vista.aspx?idReporte=113' + '&cc=' + selectCCEditar.val() + '&numero=' + inputNumeroEditar.val() + '&inMemory=1');
                report.on('load', function () {

                    var formData = new FormData();

                    formData.append("fuEvidencia", file);
                    formData.append("cc", selectCCEditar.val());
                    formData.append("numero", inputNumeroEditar.val());
                    formData.append("correo", inputCorreoProveedor.val());

                    $.ajax({
                        type: "POST",
                        url: "/Enkontrol/OrdenCompra/enviarOCProv",
                        data: formData,
                        dataType: 'json',
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            if (response.success) {
                                $("#mdlEnviar .close").click();
                                $.unblockUI();
                                btnOpenEnviar.prop("disabled", true);
                                AlertaGeneral("confirmación", "¡OC Enviada correctamente!");
                                window.location = "/Enkontrol/OrdenCompra/Generar";
                            } else {
                                $.unblockUI();
                                Alert2Warning(response.message);
                            }
                        },
                        error: function (error) {
                            $.unblockUI();
                            AlertaGeneral("Alerta", "¡Ocurrio un error, favor de validar el correo del proveedor!");
                        }
                    });
                });

            } else {
                $.unblockUI();
                AlertaGeneral("Alerta", "Debe capturar el correo del proveedor.");
            }

        }

        //Peticiones para Editar Compra
        const getCompra = () => $.post('/Enkontrol/OrdenCompra/GetCompra', { cc: selectCCEditar.val(), num: inputNumeroEditar.val() != '' ? inputNumeroEditar.val() : 0 });
        const guardarCambios = (compra) => $.post('/Enkontrol/OrdenCompra/UpdateCompra', { compra: compra });

        btnGuardarNuevaCompra.on('click', function () {
            if (_flagPeriodoContable) {
                if (selectMoneda.val() != '') {

                    if (selectMoneda.val() == '2' && unmaskNumero6DCompras(inputTipoCambio.val()) <= 1) {
                        AlertaGeneral('Alerta', 'Ingresar un tipo de cambio valido.');
                        return null;
                    }

                    if (inputProvNum.val() != '') {
                        if (_variableCompraNuevaEditar == 1) {
                            if (selectBoS.val() == 'B' && (checkAutoRecep.attr('checked') || inputAlmNum.val() != '' || inputEmpNum.val() != '')) {
                                AlertaGeneral('Alerta', 'No se puede crear una Orden de Compra Auto Recepcionable de tipo "Bienes".');
                            } else {
                                let compra = getInfoCompra(false);

                                if (compra != null) {
                                    let listProveedoresDistintos = compra.lstPartidas.map(x => x.proveedorDistinto);
                                    let proveedorVacio = listProveedoresDistintos.some(function (x) { return x == 0; });

                                    if (!proveedorVacio) {
                                        let flagPresupuesto = true;

                                        if (_validaGlobal) {
                                            let presupuestoGlobal = unmaskNumero6DCompras(labelPresupuestoGlobal.attr('presupuesto'));
                                            let presupuestoActual = unmaskNumero6DCompras(labelPresupuestoActual.attr('presupuesto'));

                                            if (presupuestoGlobal >= (presupuestoActual + compra.sub_total)) {
                                                flagPresupuesto = true;
                                            } else {
                                                flagPresupuesto = false;
                                            }
                                        }

                                        if (flagPresupuesto) {
                                            if (compra.tiempoEntregaDias > 0) {
                                                if (compra.anticipoBool) {
                                                    if (compra.totalAnticipo > 0 && compra.totalAnticipo <= compra.total_rec) {
                                                        $.blockUI({ message: 'Procesando...' });
                                                        guardarNuevaCompra(compra).done(response => {
                                                            if (response.success) {
                                                                AlertaGeneral('Alerta', 'Se ha guardado la información. Compras: ' + response.info.stringVobosAutorizaciones);

                                                                limpiarInformacion();
                                                                limpiarTabla(tblPartidas);
                                                                limpiarTabla(tblPartidasEditar);
                                                                limpiarTabla(tblPagos);
                                                                limpiarTabla(tblRetenciones);

                                                                selectCC.val('');
                                                                inputNumero.val('');
                                                                textAreaDescPartida.val('');

                                                                btnVerCuadro.css('display', 'none');
                                                                btnNoTieneCuadro.css('display', 'none');

                                                                btnImprimir.attr('disabled', true);
                                                                btnCancelarCompra.attr('disabled', true);
                                                            } else {
                                                                AlertaGeneral(`Alerta`, `No se guardó la información. ${response.message}`);
                                                                // cargarRequisicion();
                                                            }
                                                        }).always($.unblockUI);
                                                    } else {
                                                        AlertaGeneral(`Alerta`, `Las compras con anticipo no pueden llevar el monto en cero ni puede ser mayor al total de la compra.`);
                                                    }
                                                } else {
                                                    compra.totalAnticipo = compra.total_rec;

                                                    $.blockUI({ message: 'Procesando...' });
                                                    guardarNuevaCompra(compra).done(response => {
                                                        if (response.success) {
                                                            AlertaGeneral('Alerta', 'Se ha guardado la información. Compras: ' + response.info.stringVobosAutorizaciones);

                                                            limpiarInformacion();
                                                            limpiarTabla(tblPartidas);
                                                            limpiarTabla(tblPartidasEditar);
                                                            limpiarTabla(tblPagos);
                                                            limpiarTabla(tblRetenciones);

                                                            selectCC.val('');
                                                            inputNumero.val('');
                                                            textAreaDescPartida.val('');

                                                            btnVerCuadro.css('display', 'none');
                                                            btnNoTieneCuadro.css('display', 'none');

                                                            btnImprimir.attr('disabled', true);
                                                            btnCancelarCompra.attr('disabled', true);
                                                        } else {
                                                            AlertaGeneral(`Alerta`, `No se guardó la información. ${response.message}`);
                                                            // cargarRequisicion();
                                                        }
                                                    }).always($.unblockUI);
                                                }
                                            } else {
                                                AlertaGeneral(`Alerta`, `Debe capturar la cantidad de días para el tiempo de entrega.`);
                                            }
                                        } else {
                                            AlertaGeneral(`Alerta`, `El presupuesto actual sobrepasa el global para el centro de costo.`);
                                        }
                                    } else {
                                        AlertaGeneral(`Alerta`, `Capture un número de proveedor para todas las partidas.`);
                                    }
                                } else {
                                    AlertaGeneral(`Alerta`, `No se puede comprar más de lo requerido.`);
                                }
                            }
                        } else if (_variableCompraNuevaEditar == 2) {
                            if (selectBoS.val() == 'B' && (checkAutoRecep.attr('checked') || inputAlmNum.val() != '' || inputEmpNum.val() != '')) {
                                AlertaGeneral('Alerta', 'No se puede crear una Orden de Compra Auto Recepcionable de tipo "Bienes".');
                            } else {
                                let compra = getInfoCompra(true);

                                if (compra != null) {
                                    if (compra.tiempoEntregaDias) {
                                        if (compra.anticipoBool) {
                                            if (compra.totalAnticipo > 0 && compra.totalAnticipo <= compra.total_rec) {
                                                $.blockUI({ message: 'Procesando...' });
                                                guardarCambios(compra).done(response => {
                                                    if (response.success) {
                                                        AlertaGeneral('Alerta', 'Se ha guardado la información. ' + response.info.stringVobosAutorizaciones);

                                                        cargarCompra();
                                                    } else {
                                                        AlertaGeneral(`Alerta`, `No se guardó la información. ${response.message}`);
                                                        cargarCompra();
                                                    }
                                                }).always($.unblockUI);
                                            } else {
                                                AlertaGeneral(`Alerta`, `Las compras con anticipo no pueden llevar el monto en cero ni puede ser mayor al total de la compra.`);
                                            }
                                        } else {
                                            compra.totalAnticipo = compra.total_rec;

                                            $.blockUI({ message: 'Procesando...' });
                                            guardarCambios(compra).done(response => {
                                                if (response.success) {
                                                    AlertaGeneral('Alerta', 'Se ha guardado la información. ' + response.info.stringVobosAutorizaciones);

                                                    cargarCompra();
                                                } else {
                                                    AlertaGeneral(`Alerta`, `No se guardó la información. ${response.message}`);
                                                    cargarCompra();
                                                }
                                            }).always($.unblockUI);
                                        }

                                    } else {
                                        AlertaGeneral(`Alerta`, `Debe capturar la cantidad de días para el tiempo de entrega.`);
                                    }
                                } else {
                                    AlertaGeneral(`Alerta`, `No se puede comprar más de lo requerido.`);
                                }
                            }
                        }

                        inputProvNum.removeClass('campoInvalido')
                    } else {
                        AlertaGeneral('Alerta', 'Especifique un proveedor.');
                        inputProvNum.addClass('campoInvalido')
                    }

                    selectMoneda.removeClass('campoInvalido');
                } else {
                    AlertaGeneral('Alerta', 'Seleccione el tipo de moneda.');
                    selectMoneda.addClass('campoInvalido')
                }
            } else {
                AlertaGeneral(`Alerta`, `El Periodo Contable no está activo.`);
            }
        });

        selectCC.on('change', e => {
            if ($(e.currentTarget).val() != '') {
                cargarPresupuesto();
            } else {
                fieldsetPresupuesto.css('display', 'none');
                _validaGlobal = false;
            }

            if ($(e.currentTarget).val() != '' && inputNumero.val() != '') {
                cargarRequisicion();
            }
        });

        inputNumero.on('change', e => {
            if ($(e.currentTarget).val() != '' && selectCC.val() != '') {
                cargarRequisicion();
            }
        });

        selectCCEditar.on('change', e => {
            fieldsetPresupuesto.css('display', 'none');
            _validaGlobal = false;

            if ($(e.currentTarget).val() != '' && inputNumeroEditar.val() != '') {
                cargarCompra();
            }
        });

        inputNumeroEditar.on('change', e => {
            if ($(e.currentTarget).val() != '' && selectCCEditar.val() != '') {
                cargarCompra();
            }
        });

        btnAgregarRetencion.on('click', function () {
            let datos = tblRetenciones.DataTable().rows().data();

            datos.push({
                'id_cpto': '',
                'descRet': '',
                'cantidad': 0.00,
                'porc_ret': 0,
                'importe': 0,
                'facturado': 0,
                'retenido': 0,
                'tm_descto': 0
            });

            tblRetenciones.DataTable().clear();
            tblRetenciones.DataTable().rows.add(datos).draw();

            btnQuitarRetencion.prop("disabled", false);
        });

        btnQuitarRetencion.on('click', function () {
            tblRetenciones.DataTable().row(tblRetenciones.find("tr.active")).remove().draw();

            let cuerpo = tblRetenciones.find('tbody');

            if (cuerpo.find("tr").length == 0) {
                tblRetenciones.DataTable().draw();
            }

            calcularRetenciones();
        });

        inputIVAPorcentaje.on('change', e => {
            let valor = $(e.currentTarget).val() != '' ? $(e.currentTarget).val() : 0;

            if (!isNaN(valor)) {
                // let iva = parseFloat(valor);
                // let subTotal = unmaskNumero6DCompras(inputSubTotal.val());

                // inputIVANumero.val(maskNumero6DComprasColombia((subTotal * iva) / 100));
                // inputTotal.val(maskNumero6DComprasColombia(subTotal + (unmaskNumero6DCompras(inputIVANumero.val()))));

                if (_variableCompraNuevaEditar == 1) {
                    tblPartidas.find('tbody tr').each(function (idx, row) {
                        let exento_iva = $(row).find('.checkboxExentoIVA').prop('checked');

                        if (!exento_iva) {
                            $(row).find('.inputPorcentajeIVA').val(valor);
                            $(row).find('.inputPorcentajeIVA').change();
                        }
                    });
                } else if (_variableCompraNuevaEditar == 2) {
                    tblPartidasEditar.find('tbody tr').each(function (idx, row) {
                        let exento_iva = $(row).find('.checkboxExentoIVA').prop('checked');

                        if (!exento_iva) {
                            $(row).find('.inputPorcentajeIVA').val(valor);
                            $(row).find('.inputPorcentajeIVA').change();
                        }
                    });
                }
            } else {
                $(e.currentTarget).val(19);
                inputIVAPorcentaje.change();
            }

            // calcularRetenciones();
        });

        tblPartidas.on('change', '.inputColocada', function () {
            let row = $(this).closest('tr');

            calcularImportePorRenglon(row);
            calcularResultados();
            calcularRetenciones();
        });

        tblPartidas.on('change', '.inputPrecio', function () {
            let row = $(this).closest('tr');

            calcularImportePorRenglon(row);
            // verificarPrecioSegurido(row);
            calcularResultados();
            calcularRetenciones();
        });

        btnAgregarInsumo.on('click', function () {
            let datos = tblPartidas.DataTable().rows().data();

            datos.push({
                'partida': datos.length + 1,
                'insumo': '',
                'insumoDesc': '',
                'areaCuenta': '',
                'cantidad': 0,
                'requerido': 0,
                'surtido': 0,
                'pendiente': 0,
                'colocada': 0,
                'precio': 0,
                'importe': 0
            });

            tblPartidas.DataTable().clear();
            tblPartidas.DataTable().rows.add(datos).draw();

            btnQuitarInsumo.prop("disabled", false);
        });

        btnQuitarInsumo.on('click', function () {
            tblPartidas.DataTable().row(tblPartidas.find("tr.active")).remove().draw();

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
                });
            }

            calcularRetenciones();
        });

        btnAgregarInsumoEditar.on('click', function () {
            let datos = tblPartidasEditar.DataTable().rows().data();

            datos.push({
                'partida': datos.length + 1,
                'insumo': '',
                'insumoDesc': '',
                'areaCuenta': '',
                // 'fecha_entrega': '',
                'cantidad': 0,
                'precio': 0,
                'importe': 0
            });

            tblPartidasEditar.DataTable().clear();
            tblPartidasEditar.DataTable().rows.add(datos).draw();

            btnQuitarInsumoEditar.prop("disabled", false);
        });

        btnQuitarInsumoEditar.on('click', function () {
            tblPartidasEditar.DataTable().row(tblPartidasEditar.find("tr.active")).remove().draw();

            let cuerpo = tblPartidasEditar.find('tbody');

            if (cuerpo.find("tr").length == 0) {
                tblPartidasEditar.DataTable().draw();
            } else {
                tblPartidasEditar.find('tbody tr').each(function (idx, row) {
                    let rowData = tblPartidasEditar.DataTable().row(row).data();

                    rowData.partida = ++idx;

                    tblPartidasEditar.DataTable().row(row).data(rowData).draw();
                });
            }

            calcularRetenciones();
        });

        btnVerCuadro.on('click', function () {
            selectFolioCuadro.val(1);

            mdlCuadroComparativo.modal('show');

            getCuadroDetalle(1);
        });

        selectFolioCuadro.on('change', function () {
            if ($(this).val() != '') {
                getCuadroDetalle($(this).val());
            } else {
                limpiarCuadro();
            }
        });

        btnImprimir.on('click', function () {
            let cc = selectCCEditar.val();
            let numero = inputNumeroEditar.val();

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/CheckEstatusOrdenCompraImpresa', { cc, numero })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        if (cc != '' && !isNaN(numero)) {
                            verReporteCompra(cc, numero);
                        }
                    } else {
                        AlertaGeneral(`Alerta`, `${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        btnCancelarCompra.on('click', function () {
            let cc = selectCCEditar.val();
            let numero = inputNumeroEditar.val();

            if (cc != '' && !isNaN(numero)) {
                AlertaCancelarCompra(
                    `Alerta`, `¿Desea cancelar los insumos de la Compra #${numero} del Centro de Costo "${selectCCEditar.find('option:selected').text()}"?`
                );
            }
        });

        textAreaDescPartida.on('change', function () {
            let renglonActivo = tblPartidas.find('tbody .active');
            let renglonActivoEditar = tblPartidasEditar.find('tbody .active');
            let rowData = tblPartidas.DataTable().row(renglonActivo).data();
            let rowDataEditar = tblPartidasEditar.DataTable().row(renglonActivoEditar).data();

            if (rowData != undefined) {
                rowData.partidaDescripcion = textAreaDescPartida.val();
            }

            if (rowDataEditar != undefined) {
                rowDataEditar.partidaDescripcion = textAreaDescPartida.val();
            }
        });

        inputIVANumero.on('focus', function () {
            $(this).select();
        });

        inputIVANumero.on('change', function () {
            // let cantidadIVA = unmaskNumero6DCompras(inputIVANumero.val());
            // let subTotal = unmaskNumero6DCompras(inputSubTotal.val());

            // if (!isNaN(cantidadIVA)) {
            //     let porcentajeIVA = (cantidadIVA * 100) / subTotal;

            //     inputIVAPorcentaje.val(porcentajeIVA.toFixed(2));
            //     inputTotal.val(maskNumero6DComprasColombia(subTotal + (unmaskNumero6DCompras(inputIVANumero.val()))));

            //     calcularRetenciones();
            // }
        });

        btnGuardarRetenciones.on('click', function () {
            mdlRetenciones.modal('hide');
        });

        $('#btnEscogerProv1, #btnEscogerProv2, #btnEscogerProv3').on('click', function () {
            let folioCuadro = selectFolioCuadro.val();
            let posicionProveedor = unmaskNumero($(this).attr('data-proveedor'));

            switch (posicionProveedor) {
                case 1:
                    inputProvNum.val(inputProv1Num.val());
                    inputProvNum.change();
                    getPreciosPorProveedorEditar(folioCuadro, inputProv1Num.val());
                    break;
                case 2:
                    inputProvNum.val(inputProv2Num.val());
                    inputProvNum.change();
                    getPreciosPorProveedorEditar(folioCuadro, inputProv2Num.val());
                    break;
                case 3:
                    inputProvNum.val(inputProv3Num.val());
                    inputProvNum.change();
                    getPreciosPorProveedorEditar(folioCuadro, inputProv3Num.val());
                    break;
                default:
                    break;
            }

            mdlCuadroComparativo.modal('hide');
        });

        checkboxAnticipo.on('click', function () {
            let checked = $(this).prop('checked');

            inputTotalAnticipo.css('display', checked ? 'inline-block' : 'none');
            inputTotalAnticipo.val(checked ? maskNumero6DComprasColombia(0) : '');
            labelAnticipo.text(checked ? 'Anticipo:' : 'Anticipo');
        });

        inputTotalAnticipo.on('focus', function () {
            $(this).select();
        });

        inputTotalAnticipo.on('change', function () {
            let valor = unmaskNumero6D($(this).val());

            inputTotalAnticipo.val(maskNumero6DComprasColombia(valor));
        });

        if (_empresaActual == 6) {
            $('#selectMoneda, #selectTipoCompraVenta').on('change', function () {
                let moneda = +selectMoneda.val();

                if (moneda == 2) {
                    let cuentaDolares = selectCuentaCorriente.find('option[moneda="ME"]');

                    if (cuentaDolares.length > 0) {
                        selectCuentaCorriente.find('option[selected="selected"]').attr('selected', false);
                        $(cuentaDolares[0]).attr('selected', true);
                    }

                    axios.post('GetTipoCambioPeru').then(response => {
                        let { success, data, message } = response.data;

                        if (success) {
                            if (+selectTipoCompraVenta.val() == 1) {
                                inputTipoCambio.val(maskNumero6DComprasColombia(response.data.data.TIPOCAMB_COMPRA));
                            } else if (+selectTipoCompraVenta.val() == 4) {
                                inputTipoCambio.val(maskNumero6DComprasColombia(response.data.data.TIPOCAMB_VENTA));
                            }
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
                } else {
                    let cuentaSoles = selectCuentaCorriente.find('option[moneda="MN"]');

                    if (cuentaSoles.length > 0) {
                        selectCuentaCorriente.find('option[selected="selected"]').attr('selected', false);
                        $(cuentaSoles[0]).attr('selected', true);
                    }

                    inputTipoCambio.val(maskNumero6DComprasColombia(1));
                }
            });

            selectCuentaCorriente.on('change', function () {
                if (selectCuentaCorriente.val() != '') {
                    let opcionSeleccionada = selectCuentaCorriente.find('option:selected');

                    selectMoneda.val(opcionSeleccionada.attr('moneda') == 'MN' ? 4 : 2);
                    selectMoneda.change();
                }
            })
        }

        function initCbo() {
            selectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCcComCompradorModalEditar', null, false);
            // selectCCEditar.fillCombo('/Enkontrol/OrdenCompra/FillComboCcComCompradorModalEditar', null, false);
            selectCCEditar.fillCombo('/Enkontrol/OrdenCompra/FillComboCc', null, false);
            selectLab.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false);
            selectTipoOC.fillCombo('/Enkontrol/Requisicion/FillComboTipoReq', null, false);

            if (_empresaActual == 6) {
                selectFormaPago.fillCombo('/Enkontrol/OrdenCompra/FillComboFormaPagoPeru', null, false, null);
            }
        }

        function isComprador() {
            $.blockUI({ message: 'Procesando...' });

            getComprador().done(response => {
                if (response.success) {
                    inputCompradorSesionNum.val(response.comprador.comprador);
                    inputCompradorSesionNom.val(response.comprador.emplNom);
                } else {
                    AlertaGeneral("Aviso", "No eres comprador.");
                }
            }).then($.unblockUI);
        }

        function puedeCancelar() {
            btnCancelarCompra.attr('disabled', true);

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/puedeCancelar')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        btnCancelarCompra.attr('disabled', !response.puedeCancelar);
                    } else {
                        btnCancelarCompra.attr('disabled', true);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }
        function OcultarColumnaAreaCuenta() {
            if (idEmpresa.val() == 6) {
                tblPartidas.DataTable().column(3).visible(false);
            } else {
                tblPartidas.DataTable().column(3).visible(true);
            }
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
                scrollX: '1000px',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tblPartidas.on('click', 'td', function () {
                        let rowData = tblPartidas.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblPartidas.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");
                            if (rowData != undefined) {
                                textAreaDescPartida.val(rowData.partidaDescripcion);
                            }

                        } else {
                            textAreaDescPartida.val('');
                        }
                    });

                    tblPartidas.on('change', 'input, select', function () {
                        let row = $(this).closest('tr');
                        let proveedorDistinto = row.find('.inputProveedorDistinto').val();

                        if (!$(this).hasClass('inputProveedorDistinto')) {
                            let rowData = tblPartidas.DataTable().row(row).data();
                            let insumo = unmaskNumero(row.find('.inputInsumo').val());
                            let insumoDesc = row.find('.inputInsumoDesc').val();
                            let fecha_entrega = row.find('.inputRequerido').val();
                            let colocada = unmaskNumero(row.find('.inputColocada').val());
                            let precio = unmaskNumero6DCompras(row.find('.inputPrecio').val());
                            let exentoIVA = row.find('.checkboxExentoIVA').prop('checked');
                            let porcentajeIVA = +row.find('.inputPorcentajeIVA').val();

                            if (exentoIVA) {
                                row.find('.inputPorcentajeIVA').prop('disabled', exentoIVA);
                                porcentajeIVA = 0;
                            } else {
                                if (porcentajeIVA == 0) {
                                    porcentajeIVA = +inputIVAPorcentaje.val();
                                }
                            }

                            rowData.area = parseInt($(row).find('.selectAreaCuenta option:selected').val());
                            rowData.cuenta = parseInt($(row).find('.selectAreaCuenta option:selected').attr('data-prefijo'));
                            rowData.noEconomico = $(row).find('.selectAreaCuenta option:selected').val();
                            rowData.insumo = insumo;
                            rowData.insumoDesc = insumoDesc;
                            rowData.fecha_entrega = fecha_entrega;
                            rowData.colocada = colocada;
                            rowData.precio = precio;
                            rowData.importe = colocada * precio;
                            rowData.proveedorDistinto = proveedorDistinto;
                            rowData.exento_iva = exentoIVA;
                            rowData.porcent_iva = porcentajeIVA;
                            rowData.iva = rowData.importe * (porcentajeIVA / 100);

                            let posicionScroll = document.querySelector('#tblPartidas_wrapper .dataTables_scrollBody').scrollTop;

                            tblPartidas.DataTable().row(row).data(rowData).draw();

                            document.querySelector('#tblPartidas_wrapper .dataTables_scrollBody').scrollTop = posicionScroll;

                            reInicializarCamposRenglon(row, rowData, false);
                            calcularResultados();
                            calcularIVA(false);
                            calcularRetenciones();
                        } else {
                            $.post('/Enkontrol/OrdenCompra/GetProveedorInfo', { num: (proveedorDistinto != '' ? proveedorDistinto : 0), PERU_tipoCambio: (_empresaActual == 6 ? selectTipoCompraVenta.find('option:selected').text() : '') }).then(response => {
                                let rowData = tblPartidas.DataTable().row(row).data();
                                let insumo = unmaskNumero(row.find('.inputInsumo').val());
                                let insumoDesc = row.find('.inputInsumoDesc').val();
                                let fecha_entrega = row.find('.inputRequerido').val();
                                let colocada = unmaskNumero(row.find('.inputColocada').val());
                                let precio = unmaskNumero6DCompras(row.find('.inputPrecio').val());
                                let exentoIVA = row.find('.checkboxExentoIVA').prop('checked');
                                let porcentajeIVA = +row.find('.inputPorcentajeIVA').val();

                                if (exentoIVA) {
                                    row.find('.inputPorcentajeIVA').prop('disabled', exentoIVA);
                                    porcentajeIVA = 0;
                                } else {
                                    if (porcentajeIVA == 0) {
                                        porcentajeIVA = +inputIVAPorcentaje.val();
                                    }
                                }

                                rowData.area = parseInt($(row).find('.selectAreaCuenta option:selected').val());
                                rowData.cuenta = parseInt($(row).find('.selectAreaCuenta option:selected').attr('data-prefijo'));
                                rowData.noEconomico = $(row).find('.selectAreaCuenta option:selected').val();
                                rowData.insumo = insumo;
                                rowData.insumoDesc = insumoDesc;
                                rowData.fecha_entrega = fecha_entrega;
                                rowData.colocada = colocada;
                                rowData.precio = precio;
                                rowData.importe = colocada * precio;
                                rowData.exento_iva = exentoIVA;
                                rowData.porcent_iva = porcentajeIVA;
                                rowData.iva = rowData.importe * (porcentajeIVA / 100);

                                if (response != null) {
                                    if (response.cancelado != 'C') {
                                        rowData.proveedorDistinto = proveedorDistinto;
                                    } else {
                                        AlertaGeneral(`Alerta`, `El proveedor "${response.label} - ${response.id}" no está activo.`);

                                        rowData.proveedorDistinto = 0;
                                    }

                                    if (response.proveedorSubcontratistaBloqueado) {
                                        Alert2Warning('El proveedor está bloqueado como subcontratista.');
                                    } else if (_flagValidacionInsumoSubcontratista) {
                                        if (!response.proveedorSubcontratistaExiste) {
                                            //Alert2Warning('El proveedor no está registrado como subcontratista.')
                                        }
                                    }
                                } else {
                                    rowData.proveedorDistinto = 0;
                                }

                                let posicionScroll = document.querySelector('#tblPartidas_wrapper .dataTables_scrollBody').scrollTop;

                                tblPartidas.DataTable().row(row).data(rowData).draw();

                                document.querySelector('#tblPartidas_wrapper .dataTables_scrollBody').scrollTop = posicionScroll;

                                reInicializarCamposRenglon(row, rowData, false);
                                calcularResultados();
                                calcularIVA(false);
                                calcularRetenciones();
                            }, error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            }
                            );
                        }
                    });

                    tblPartidas.on('focus', 'input', function () {
                        $(this).select();
                    });
                },
                createdRow: function (row, rowData) {
                    reInicializarCamposRenglon(row, rowData, false);

                    if (rowData.compras_req == 0) {
                        $(row).find('input, select, button').attr('disabled', true);
                        $(row).addClass('renglonInsumoBloqueado');
                        $(row).find('.inputColocada').val(0);
                    }
                },
                columns: [
                    { data: 'partida', title: 'Partida' },
                    {
                        data: 'insumo', title: 'Insumo', render: (data, type, row, meta) => {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputInsumo');
                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'insumoDesc', title: 'Descripción', render: (data, type, row, meta) => {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputInsumoDesc');
                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'areaCuenta', title: 'Área Cuenta', render: (data, type, row, meta) => {
                            let select = document.createElement('select');

                            select.classList.add('form-control', 'text-center', 'selectAreaCuenta');

                            return select.outerHTML;
                        }
                    },
                    { data: 'cantidad', title: 'Cantidad' },
                    {
                        data: 'requerido', title: 'Requerido', render: (data, type, row, meta) => {
                            let input = document.createElement('input');
                            let fecha = new Date();
                            let diasTipoRequisicion = selectTipoOC.val() != '' ? parseInt(selectTipoOC.find('option:selected').attr('data-prefijo')) : 0;
                            let nuevaFechaRequerido = new Date(fecha.setTime(fecha.getTime() + diasTipoRequisicion * 86400000));
                            let formatoNuevaFechaRequerido = $.datepicker.formatDate('dd/mm/yy', nuevaFechaRequerido);

                            input.classList.add('form-control', 'text-center', 'inputRequerido');
                            $(input).attr('value', formatoNuevaFechaRequerido);

                            return input.outerHTML;
                        }
                    },
                    { data: 'surtido', title: 'Surtido' },
                    { data: 'pendiente', title: 'Pendiente' },
                    {
                        data: 'colocada', title: 'Colocada', render: (data, type, row, meta) => {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputColocada');
                            // $(input).attr('value', row.pendiente);
                            $(input).attr('value', row.colocada);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'precio', title: 'Precio', render: (data, type, row, meta) => {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-right', 'inputPrecio');
                            $(input).attr('value', '$' + formatMoney(data));
                            // $(input).attr('disabled', true);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'importe', title: 'Importe', render: (data, type, row, meta) => {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-right', 'inputImporte');
                            $(input).attr('value', '$' + formatMoney(data));
                            $(input).attr('disabled', true);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'proveedorDistinto', title: 'Proveedor', render: (data, type, row, meta) => {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputProveedorDistinto');
                            $(input).attr('value', data != 0 ? data : '');

                            return input.outerHTML;
                        }
                    },
                    {
                        title: 'Exento IVA', render: (data, type, row, meta) => {
                            return `
                                <input type="checkbox" id="checkboxExentoIVA_${row.partida}" class="regular-checkbox checkboxExentoIVA" ${row.exento_iva ? 'checked' : ''}>
                                <label for="checkboxExentoIVA_${row.partida}"></label>
                            `;
                        }
                    },
                    {
                        title: '% IVA', render: (data, type, row, meta) => {
                            return `<input class="form-control text-center inputPorcentajeIVA" value="${!isNaN(row.porcent_iva) ? row.porcent_iva : ''}" ${row.exento_iva ? 'disabled' : ''}>`;
                        }
                    },
                    {
                        title: 'IVA', render: (data, type, row, meta) => {
                            return `<input class="form-control text-center inputMontoIVA" value="${!isNaN(row.iva) ? maskNumero6DComprasColombia(row.iva) : ''}" disabled>`;
                        }
                    }
                ],
                columnDefs: [
                    {
                        render: function (data, type, row) {
                            return data != null ? maskNumero6DComprasColombia(data) : '';
                        },
                        targets: [9]
                    },
                    {
                        render: function (data, type, row) {
                            return data != null ? unmaskNumero6D(data) : '';
                        },
                        targets: [4, 5, 6]
                    },
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablePartidasEditar() {
            tblPartidasEditar.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                aaSorting: [0, 'asc'],
                rowId: 'id',
                scrollY: "250px",
                scrollX: '1000px',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tblPartidasEditar.on('click', 'td', function () {
                        let rowData = tblPartidasEditar.DataTable().row($(this).closest('tr')).data();

                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblPartidasEditar.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");
                            textAreaDescPartida.val(rowData.partidaDescripcion);
                        } else {
                            textAreaDescPartida.val('');
                        }
                    });

                    tblPartidasEditar.on('change', 'input, select', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblPartidasEditar.DataTable().row(row).data();
                        let insumo = unmaskNumero(row.find('.inputInsumo').val());
                        let insumoDesc = row.find('.inputInsumoDesc').val();
                        let fecha_entrega = row.find('.inputRequerido').val();
                        let cantidad = unmaskNumero(row.find('.inputCantidad').val());
                        let precio = unmaskNumero6DCompras(row.find('.inputPrecio').val());
                        let exentoIVA = row.find('.checkboxExentoIVA').prop('checked');
                        let porcentajeIVA = +row.find('.inputPorcentajeIVA').val();

                        if (exentoIVA) {
                            row.find('.inputPorcentajeIVA').prop('disabled', exentoIVA);
                            porcentajeIVA = 0;
                        } else {
                            if (porcentajeIVA == 0) {
                                porcentajeIVA = +inputIVAPorcentaje.val();
                            }
                        }

                        rowData.area = parseInt($(row).find('.selectAreaCuenta option:selected').val());
                        rowData.cuenta = parseInt($(row).find('.selectAreaCuenta option:selected').attr('data-prefijo'));
                        rowData.noEconomico = $(row).find('.selectAreaCuenta option:selected').val();
                        rowData.insumo = insumo;
                        rowData.insumoDesc = insumoDesc;
                        rowData.fecha_entrega = fecha_entrega;
                        rowData.cantidad = cantidad;
                        rowData.precio = precio;
                        rowData.importe = cantidad * precio;
                        rowData.exento_iva = exentoIVA;
                        rowData.porcent_iva = porcentajeIVA;
                        rowData.iva = rowData.importe * (porcentajeIVA / 100);

                        let posicionScroll = document.querySelector('#tblPartidasEditar_wrapper .dataTables_scrollBody').scrollTop;

                        tblPartidasEditar.DataTable().row(row).data(rowData).draw();

                        document.querySelector('#tblPartidasEditar_wrapper .dataTables_scrollBody').scrollTop = posicionScroll;

                        reInicializarCamposRenglon(row, rowData, true);
                        calcularResultadosEditar();
                        calcularIVA(true);
                        calcularRetenciones();
                    });

                    tblPartidasEditar.on('focus', 'input', function () {
                        $(this).select();
                    });
                },
                createdRow: function (row, rowData) {
                    reInicializarCamposRenglon(row, rowData, true);

                    if (rowData.compras_req == 0) {
                        $(row).find('input, select, button').attr('disabled', true);
                        $(row).addClass('renglonInsumoBloqueado');
                        $(row).find('.inputColocada').val(0);
                    }
                },
                columns: [
                    { data: 'partida', title: 'Partida' },
                    {
                        data: 'insumo', title: 'Insumo', render: (data, type, row, meta) => {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputInsumo');

                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'insumoDesc', title: 'Descripción', render: (data, type, row, meta) => {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputInsumoDesc');

                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'areaCuenta', title: 'Área Cuenta', render: (data, type, row, meta) => {
                            let select = document.createElement('select');

                            select.classList.add('form-control', 'text-center', 'selectAreaCuenta');

                            return select.outerHTML;
                        }
                    },
                    {
                        data: 'fecha_entrega', title: 'Fecha Entregar', render: (data, type, row, meta) => {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputEntrega');

                            if (!isNaN(data) && data != '') {
                                $(input).attr('value', $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6)))));
                            } else {
                                let fecha = new Date();
                                let diasTipoRequisicion = selectTipoOC.val() != '' ? parseInt(selectTipoOC.find('option:selected').attr('data-prefijo')) : 0;
                                let nuevaFechaEntregar = new Date(fecha.setTime(fecha.getTime() + diasTipoRequisicion * 86400000));
                                let formatoNuevaFechaEntregar = $.datepicker.formatDate('dd/mm/yy', nuevaFechaEntregar);

                                $(input).attr('value', formatoNuevaFechaEntregar);
                            }

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'cantidad', render: (data, type, row, meta) => {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputCantidad');

                            $(input).attr('value', data);

                            return input.outerHTML;
                        }, title: 'Cantidad'
                    },
                    {
                        data: 'precio', title: 'Precio', render: (data, type, row, meta) => {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-right', 'inputPrecio');

                            $(input).attr('value', maskNumero6DComprasColombia(data));
                            // $(input).attr('disabled', true);

                            return input.outerHTML;
                        }
                    },
                    { data: 'importe', title: 'Importe' },
                    {
                        title: 'Exento IVA', render: (data, type, row, meta) => {
                            return `
                                <input type="checkbox" id="checkboxExentoIVA_${row.partida}" class="regular-checkbox checkboxExentoIVA" ${row.exento_iva ? 'checked' : ''}>
                                <label for="checkboxExentoIVA_${row.partida}"></label>
                            `;
                        }
                    },
                    {
                        title: '% IVA', render: (data, type, row, meta) => {
                            return `<input class="form-control text-center inputPorcentajeIVA" value="${!isNaN(row.porcent_iva) ? row.porcent_iva : ''}" ${row.exento_iva ? 'disabled' : ''}>`;
                        }
                    },
                    {
                        title: 'IVA', render: (data, type, row, meta) => {
                            return `<input class="form-control text-center inputMontoIVA" value="${!isNaN(row.iva) ? maskNumero6DComprasColombia(row.iva) : ''}" disabled>`;
                        }
                    }
                ],
                columnDefs: [
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return unmaskNumero6D(data);
                            }
                        },
                        targets: [4]
                    },
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return maskNumero6DComprasColombia(data);
                            }
                        },
                        targets: [7]
                    },
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function calcularIVA(editar) {
            if (!editar) {
                let iva = 0;
                let subTotal = unmaskNumero6DCompras(inputSubTotal.val());

                tblPartidas.find('tbody tr').each(function (idx, row) {
                    let rowData = tblPartidas.DataTable().row(row).data();
                    let porcent_iva = unmaskNumero6DCompras($(row).find('.inputPorcentajeIVA').val());
                    let partidaIVA = rowData.importe * (porcent_iva / 100);

                    $(row).find('.inputMontoIVA').val(maskNumero6DComprasColombia(partidaIVA));

                    iva += partidaIVA;
                });

                inputIVANumero.val(maskNumero6DComprasColombia(iva));
                inputTotal.val(maskNumero6DComprasColombia(subTotal + (unmaskNumero6DCompras(inputIVANumero.val()))));
            } else {
                let iva = 0;
                let subTotal = unmaskNumero6DCompras(inputSubTotal.val());

                tblPartidasEditar.find('tbody tr').each(function (idx, row) {
                    let rowData = tblPartidasEditar.DataTable().row(row).data();
                    let porcent_iva = unmaskNumero6DCompras($(row).find('.inputPorcentajeIVA').val());
                    let partidaIVA = rowData.importe * (porcent_iva / 100);

                    $(row).find('.inputMontoIVA').val(maskNumero6DComprasColombia(partidaIVA));

                    iva += partidaIVA;
                });

                inputIVANumero.val(maskNumero6DComprasColombia(iva));
                inputTotal.val(maskNumero6DComprasColombia(subTotal + (unmaskNumero6DCompras(inputIVANumero.val()))));
            }
        }

        function initTablePagos() {
            tblPagos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                "language": dtDicEsp,
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                "scrollCollapse": true,
                'initComplete': function (settings, json) {

                },
                columns: [
                    { data: 'partida', title: 'Partida' },
                    { data: 'dias_pago', title: 'Días' },
                    { data: 'fecha_pago', title: 'Vencimiento' },
                    { data: 'comentarios', title: 'Comentarios' },
                    { data: 'porcentaje', title: '%' },
                    { data: 'importe', title: 'Importe' }
                ],
                columnDefs: [
                    {
                        "render": function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        "targets": [2]
                    },
                    {
                        "render": function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return maskNumero6DComprasColombia(data);
                            }
                        },
                        "targets": [5]
                    },
                    { "className": "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableRetenciones() {
            tblRetenciones.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                aaSorting: [0, 'asc'],
                rowId: 'id',
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblRetenciones.on('click', 'td', function () {
                        let rowData = tblRetenciones.DataTable().row($(this).closest('tr')).data();

                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblRetenciones.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");
                        }
                    });

                    tblRetenciones.on('change', 'input', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblRetenciones.DataTable().row(row).data();

                        let flagCantidad = $(this).hasClass('cantidad');
                        let flagPorcentaje = $(this).hasClass('inputPorcentaje');
                        let subTotal = unmaskNumero6DCompras(inputSubTotal.val());

                        let cantidad = unmaskNumero($(row).find('.cantidad').val());
                        let porcentaje = unmaskNumero($(row).find('.inputPorcentaje').val());

                        if ($(this).hasClass('inputIDCpto')) {
                            $.blockUI({ message: 'Procesando...' });

                            getRetencionInfo($(this).val()).done(function (response) {
                                if (response.success) {
                                    if (response.data != null) {
                                        rowData.descRet = response.data.desc_ret;
                                        rowData.calc_iva = response.data.calc_iva;
                                        rowData.bit_afecta_oc = response.data.bit_afecta_oc;
                                        rowData.afecta_fac = response.data.afecta_fac;
                                    }

                                    rowData.id_cpto = $(row).find('.inputIDCpto').val();

                                    if (flagCantidad) { //Input Cantidad
                                        let nuevoPorcentaje = (cantidad / subTotal) * 100;

                                        rowData.cantidad = cantidad;
                                        rowData.porc_ret = nuevoPorcentaje;
                                        rowData.importe = cantidad;
                                    } else if (flagPorcentaje) { //Input Porcentaje
                                        let nuevaCantidad = subTotal * (porcentaje / 100);

                                        rowData.cantidad = nuevaCantidad;
                                        rowData.porc_ret = porcentaje;
                                        rowData.importe = nuevaCantidad;
                                    } else { //Input ID Cpto
                                        let cantidadDefault = subTotal * (response.data.porc_default / 100);

                                        rowData.cantidad = cantidadDefault;
                                        rowData.porc_ret = response.data.porc_default;
                                        rowData.importe = cantidadDefault;
                                    }

                                    tblRetenciones.DataTable().row(row).data(rowData).draw();

                                    calcularRetenciones();
                                }
                            }).then($.unblockUI);
                        } else {
                            rowData.id_cpto = $(row).find('.inputIDCpto').val();

                            if (flagCantidad) {
                                let nuevoPorcentaje = (cantidad / subTotal) * 100;

                                rowData.cantidad = cantidad;
                                rowData.porc_ret = nuevoPorcentaje;
                                rowData.importe = cantidad;
                            }

                            if (flagPorcentaje) {
                                let nuevaCantidad = subTotal * (porcentaje / 100);

                                rowData.cantidad = nuevaCantidad;
                                rowData.porc_ret = porcentaje;
                                rowData.importe = nuevaCantidad;
                            }

                            tblRetenciones.DataTable().row(row).data(rowData).draw();

                            calcularRetenciones();
                        }
                    });

                    tblRetenciones.on('click', '.btnCatalogoRetenciones', function () {
                        _renglonRetencion = $(this).closest('tr');

                        mdlCatalogoRetenciones.modal('show');
                    });

                    tblRetenciones.on('focus', 'input', function () {
                        $(this).select();
                    });
                },
                columns: [
                    {
                        data: 'id_cpto', render: function (data, type, row, meta) {
                            let div = document.createElement('div');
                            let input = document.createElement('input');
                            let button = document.createElement('button');
                            let icon = document.createElement('i');

                            input.classList.add('form-control', 'text-center', 'inputIDCpto');
                            input.style.height = '24px';
                            input.style.display = 'inline';
                            input.style.width = '70%';
                            button.classList.add('btn', 'btn-xs', 'btn-default', 'btnCatalogoRetenciones');
                            button.style.marginLeft = '5px';
                            icon.classList.add('fa', 'fa-eye');

                            $(input).attr('value', data);

                            $(button).append(icon);

                            $(div).append(input);
                            $(div).append(button);

                            return div.outerHTML;
                        }, title: 'Id Cpto'
                    },
                    { data: 'descRet', title: 'Desc Ret' },
                    {
                        data: 'cantidad', render: function (data, type, row, meta) {
                            let html = '<input id="inputRet_' + meta.row + '" class="form-control cantidad" style="text-align: right; height: 24px;" value="' + maskNumero6DComprasColombia(data).replace('$', '') + '" data-orden="' + row.orden + '" data-id_cpto="' + row.id_cpto + '" data-cantidad="' + row.cantidad + '" />'

                            return html;
                        }, title: 'Cantidad'
                    },
                    {
                        data: 'porc_ret', render: function (data, type, row, meta) {
                            let html = '<input class="form-control text-right inputPorcentaje" value="' + maskNumero6DComprasColombia(data).replace('$', '') + '" style="height: 24px;" />'

                            return html;
                        }, title: 'Porc Ret'
                    },
                    { data: 'importe', title: 'Importe' },
                    { data: 'facturado', title: 'Facturado' },
                    { data: 'retenido', title: 'Retenido' },
                    { data: 'tm_descto', title: 'Tm Descto' }
                ],
                columnDefs: [
                    {
                        "render": function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return maskNumero6DComprasColombia(data);
                            }
                        },
                        "targets": [4, 5, 6]
                    },
                    { "className": "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableCatalogoRetenciones() {
            tblCatalogoRetenciones.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                aaSorting: [0, 'asc'],
                rowId: 'id',
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblCatalogoRetenciones.on('click', '.btnSeleccionarRetencion', function () {
                        let rowData = tblCatalogoRetenciones.DataTable().row($(this).closest('tr')).data();

                        $(_renglonRetencion).find('.inputIDCpto').val(rowData.id_cpto);
                        $(_renglonRetencion).find('.inputIDCpto').change();

                        mdlCatalogoRetenciones.modal('hide');
                    });
                },
                columns: [
                    { data: 'id_cpto', title: 'Id Cpto' },
                    { data: 'desc_ret', title: 'Descripción' },
                    {
                        title: 'Seleccionar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-primary btnSeleccionarRetencion"><i class="fa fa-arrow-right"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: "_all" }
                ]
            });
        }

        function llenarInformacion(data) {
            //#region Panel Izquierdo
            selectBoS.val(data.bienes_servicios);
            dtpFecha.val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.fecha.substr(6)))));

            if (_empresaActual != 6) {
                inputProvNum.val(data.proveedor != 0 ? data.proveedor : '');

                if (data.proveedor != 0) {
                    inputProvNum.change();
                }

                if (data.solicito != 0) {
                    inputSolNum.val(data.solicito);
                    inputSolNum.change();
                } else {
                    inputSolNum.val('');
                }
            } else {
                inputProvNum.val(data.PERU_proveedor);
                selectFormaPago.val(data.PERU_formaPago);
                inputSolNum.val(data.solicito);
                inputSolNom.val(data.solicitoNom);
                inputCompNom.val(data.compradorNom);
                inputAutNom.val(data.autorizoNom);
                selectTipoCompraVenta.find(`option[text="${data.PERU_tipoCambio}"]`).attr('selected', true);

                $.blockUI({ message: 'Procesando...' });
                getProveedorInfo(inputProvNum.val(), selectTipoCompraVenta.find('option:selected').text()).done(function (response) {
                    selectCuentaCorriente.empty();
                    selectCuentaCorriente.append(`<option value="">--Seleccione--</option>`);

                    if (response.listaCuentasCorrientes != null && response.listaCuentasCorrientes.length > 0) {
                        response.listaCuentasCorrientes.forEach((element) => {
                            selectCuentaCorriente.append(`<option value="${element.Value}" moneda="${element.TextoOpcional}">${element.Text}</option>`);
                        });

                        if (data.PERU_cuentaCorriente != null && data.PERU_cuentaCorriente != '') {
                            selectCuentaCorriente.find(`option:contains('${data.PERU_cuentaCorriente}')`).attr('selected', true);
                        }
                    }
                }).then($.unblockUI);
            }

            inputProvNom.val(data.proveedorNom);

            if (data.comprador != 0) {
                inputCompNum.val(data.comprador);
                inputCompNum.change();
            } else {
                inputCompNum.val('');
            }

            // if (data.autorizo != 0) {
            inputAutNum.val(data.autorizo);
            inputAutNum.change();
            // } else {
            //     inputAutNum.val('');
            // }
            // inputAutNom.val(data.autorizoNom);

            inputEmb.val(data.embarquese);
            selectLab.val(data.libre_abordo);
            inputConFact.val(data.concepto_factura);

            if (data.bit_autorecepcion == 'S') {
                checkAutoRecep.prop("checked", true);
            } else {
                checkAutoRecep.prop("checked", false);
            }

            inputAlmNum.val(data.almacen_autorecepcion != 0 ? data.almacen_autorecepcion : '');
            inputAlmNom.val(data.almacenRecepNom);
            inputEmpNum.val(data.empleado_autorecepcion != 0 ? data.empleado_autorecepcion : '');
            inputEmpNom.val(data.empleadoRecepNom);
            //#endregion

            //#region Panel Derecho
            selectTipoOC.val(data.tipo_oc_req);
            selectMoneda.val(5);
            selectMoneda.trigger("change");
            inputTipoCambio.val(maskNumero6DComprasColombia(data.tipo_cambio != 0 ? data.tipo_cambio : 1));
            inputSubTotal.val(maskNumero6DComprasColombia(data.sub_total));
            inputIVAPorcentaje.val(data.porcent_iva != 0 ? data.porcent_iva : '');
            inputIVANumero.val(maskNumero6DComprasColombia(data.iva));
            inputRetencion.val(maskNumero6DComprasColombia(data.rentencion_despues_iva));
            inputTotal.val(maskNumero6DComprasColombia(data.total + data.rentencion_despues_iva));
            inputTotalFinal.val(maskNumero6DComprasColombia(data.sub_total + data.iva - data.rentencion_despues_iva));
            checkboxConsigna.prop('checked', data.consigna);
            checkboxLicitacion.prop('checked', data.licitacion);
            checkboxCRC.prop('checked', data.crc);
            checkboxConvenio.prop('checked', data.convenio);
            // checkboxTMC.prop('checked', data.tmc == 1);

            checkboxAnticipo.prop('checked', data.anticipoBool);
            inputTotalAnticipo.css('display', data.anticipoBool ? 'inline-block' : 'none');
            inputTotalAnticipo.val(data.anticipoBool ? maskNumero6DComprasColombia(data.totalAnticipo) : '');
            labelAnticipo.text(data.anticipoBool ? 'Anticipo:' : 'Anticipo');

            inputCFDI.val((data.CFDI != '' && data.CFDI != null) ? data.CFDI : 'G03 (Gastos en general)');
            inputTiempoEntregaDias.val(data.tiempoEntregaDias != 0 ? data.tiempoEntregaDias : '');
            inputTiempoEntregaComentarios.val(data.tiempoEntregaComentarios);
            //#endregion
        }

        function limpiarInformacion() {
            //#region Panel Izquierdo
            selectBoS.val('');
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
            selectLab.val('');
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
            inputTiempoEntregaDias.val('');
            inputTiempoEntregaComentarios.val('');
            checkboxConsigna.prop('checked', false);
            checkboxLicitacion.prop('checked', false);
            checkboxCRC.prop('checked', false);
            checkboxConvenio.prop('checked', false);
            // checkboxTMC.prop('checked', false);

            checkboxAnticipo.prop('checked', false);
            inputTotalAnticipo.css('display', 'none');
            inputTotalAnticipo.val('');
            labelAnticipo.text('Anticipo');
            //#endregion
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

        function limpiarTabla(tbl) {
            dt = tbl.DataTable();
            dt.clear().draw();
        }

        function cargarRequisicion() {
            _variableCompraNuevaEditar = 1;
            btnBorrarCompra.attr('disabled', true);

            inputProvNum.attr('disabled', false);
            inputProvNom.attr('disabled', false);
            inputIVAPorcentaje.attr('disabled', false);
            inputIVANumero.attr('disabled', false);

            selectCCEditar.val('');
            inputNumeroEditar.val('');
            btnImprimir.attr('disabled', true);
            btnCancelarCompra.attr('disabled', true);

            $('#divTablaPartidas').css('display', 'block');
            $('#divTablaPartidasEditar').css('display', 'none');

            fieldsetEstatusCompra.css('display', 'none');

            tblPartidas.DataTable().columns.adjust();
            tblPartidasEditar.DataTable().columns.adjust();

            limpiarInformacion();

            $.blockUI({ message: 'Procesando...' });

            let deshabilitarGuardado = false;

            divAutorecepcionable.children().attr('disabled', false);

            getRequisicion().done(response => {
                if (response.success) {
                    if (response.info.cc != null) {
                        llenarInformacion(response.info);

                        inputIVAPorcentaje.val('19');

                        if (response.partidas.some(x => x.inventariado == 'I')) {
                            divAutorecepcionable.children().attr('disabled', true);
                        }

                        limpiarTabla(tblPartidas);
                        limpiarTabla(tblPartidasEditar);

                        AddRows(tblPartidas, response.partidas);
                        AddRows(tblPagos, response.pagos);
                        AddRows(tblRetenciones, response.retenciones);

                        tblPartidas.find('tbody tr:eq(0) td:eq(0)').click();

                        tblPartidas.find('tbody tr').each(function (idx, row) {
                            $(row).find('.inputPorcentajeIVA').val(19);
                            $(row).find('.inputPrecio').change(); //Se dispara el evento change para cada input de precio para que se asigne correctamente a la data de fondo.
                        });

                        if (response.info.tieneCuadro) {
                            btnVerCuadro.css('display', 'inline-block');
                            btnNoTieneCuadro.css('display', 'none');
                            selectFolioCuadro.fillCombo('/Enkontrol/OrdenCompra/FillComboFolioCuadro', { cuadrosExistentes: response.info.cuadrosExistentes }, false);
                        } else {
                            btnVerCuadro.css('display', 'none');
                            btnNoTieneCuadro.css('display', 'inline-block');
                        }

                        if (response.info.validadoCompras) {
                            deshabilitarGuardado = false;
                        } else {
                            AlertaGeneral(`Alerta`, `La requisición no ha sido validada por almacén.`);

                            deshabilitarGuardado = true;
                        }

                        if (response.info.flagRequisicionComprada) {
                            AlertaGeneral(`Alerta`, `Requisición Totalmente Comprada: ${response.info.listComprasString}`);
                            deshabilitarGuardado = true;
                        } else {
                            deshabilitarGuardado = false;

                            if (response.info.listComprasString.length > 0) {
                                AlertaGeneral(`Alerta`, `Requisición Parcialmente Comprada: ${response.info.listComprasString}`);
                            }
                        }

                        if (response.info.st_autoriza == 'S' && !response.info.flagRequisicionComprada) {
                            deshabilitarGuardado = false;
                        } else {
                            if (response.info.st_autoriza != 'S') {
                                AlertaGeneral(`Alerta`, `La requisición no ha sido autorizada.`);
                            }

                            deshabilitarGuardado = true;
                        }

                        _flagValidacionInsumoSubcontratista = response.info.flagValidacionInsumoSubcontratista;
                    } else {
                        AlertaGeneral('Alerta', 'No se encontró información.');

                        limpiarInformacion();

                        limpiarTabla(tblPartidas);
                        limpiarTabla(tblPartidasEditar);
                        limpiarTabla(tblPagos);
                        limpiarTabla(tblRetenciones);

                        textAreaDescPartida.val('');

                        btnVerCuadro.css('display', 'none');
                        btnNoTieneCuadro.css('display', 'none');
                    }
                } else {
                    AlertaGeneral('Alerta', response.message);

                    btnVerCuadro.css('display', 'none');
                    btnNoTieneCuadro.css('display', 'none');
                }
            }).then(function () {
                $.unblockUI;
                tblPartidas.DataTable().columns.adjust();
                tblPartidasEditar.DataTable().columns.adjust();

                checkRenglonInsumoBloqueado();

                if (_flagPeriodoContable) {
                    btnGuardarNuevaCompra.attr('disabled', deshabilitarGuardado);
                } else {
                    btnGuardarNuevaCompra.attr('disabled', true);
                }
            });
        }

        function getInfoCompra(flagEditar) {
            let compra = {
                //#region Panel Izquierdo
                cc: flagEditar ? selectCCEditar.val() : selectCC.val(),
                numero: flagEditar ? inputNumeroEditar.val() : 0,
                bienes_servicios: selectBoS.val(),
                fecha: dtpFecha.val(),
                proveedor: inputProvNum.val(),
                PERU_proveedor: inputProvNum.val(),
                PERU_cuentaCorriente: _empresaActual == 6 ? selectCuentaCorriente.val() != '' ? selectCuentaCorriente.val() : "" : "",
                PERU_formaPago: _empresaActual == 6 ? selectFormaPago.val() : "",
                PERU_tipoCambio: _empresaActual == 6 ? selectTipoCompraVenta.find('option:selected').text() : "",
                comprador: inputCompNum.val(),
                solicito: inputSolNum.val(),
                autorizo: inputAutNum.val(),
                embarquese: inputEmb.val(),
                libre_abordo: selectLab.val(),
                concepto_factura: inputConFact.val(),

                bit_autorecepcion: checkAutoRecep.prop("checked") ? 'S' : 'N',

                almacen_autorecepcion: inputAlmNum.val(),
                empleado_autorecepcion: inputEmpNum.val(),
                //#endregion

                //#region Panel Derecho
                tipo_oc_req: selectTipoOC.val(),
                moneda: selectMoneda.val(),
                tipo_cambio: unmaskNumero6DCompras(inputTipoCambio.val()),
                sub_total: unmaskNumero6DCompras(inputSubTotal.val()),
                porcent_iva: inputIVAPorcentaje.val(),
                iva: unmaskNumero6DCompras(inputIVANumero.val()),
                rentencion_despues_iva: unmaskNumero6DCompras(inputRetencion.val()),
                // total: unmaskNumero(inputTotal.val())
                total_rec: unmaskNumero6DCompras(inputTotal.val()),
                CFDI: inputCFDI.val(),
                tiempoEntregaDias: +(inputTiempoEntregaDias.val()),
                tiempoEntregaComentarios: inputTiempoEntregaComentarios.val(),
                anticipoBool: checkboxAnticipo.prop('checked'),
                totalAnticipo: unmaskNumero6DCompras(inputTotalAnticipo.val())
                //#endregion
            };

            let lstPartidas = [];
            let flagColocadaMayor = false;

            if (!flagEditar) {
                tblPartidas.find('tbody tr').each(function (idx, row) {
                    let rowData = tblPartidas.DataTable().row($(row)).data();

                    if (rowData.pendiente < unmaskNumero($(row).find('.inputColocada').val())) {
                        flagColocadaMayor = true;
                    }

                    let partida = {
                        cc: compra.cc,
                        numero: compra.numero,
                        partida: rowData.partida,
                        insumo: rowData.insumo,

                        fecha_entrega: $(row).find('.inputRequerido').val(),
                        fecha_entregaString: $(row).find('.inputRequerido').val(),

                        cantidad: unmaskNumero($(row).find('.inputColocada').val()),
                        precio: unmaskNumero6DCompras($(row).find('.inputPrecio').val()),
                        importe: unmaskNumero6DCompras($(row).find('.inputImporte').val()),
                        ajuste_cant: 0,
                        ajuste_imp: 0,
                        num_requisicion: inputNumero.val(),
                        part_requisicion: rowData.partida,
                        cant_recibida: 0,
                        imp_recibido: 0,
                        fecha_recibido: null,
                        cant_canc: 0,
                        imp_canc: 0,

                        acum_ant: 0,

                        max_orig: 0,
                        max_ppto: 0,
                        area: rowData.area,
                        cuenta: rowData.cuenta,
                        noEconomico: rowData.noEconomico,
                        porcent_iva: +$(row).find('.inputPorcentajeIVA').val(),
                        iva: unmaskNumero6DCompras($(row).find('.inputMontoIVA').val()),
                        exento_iva: $(row).find('.checkboxExentoIVA').prop('checked'),
                        partidaDescripcion: rowData.partidaDescripcion,
                        proveedorDistinto: unmaskNumero($(row).find('.inputProveedorDistinto').val()),
                        PERU_proveedor: $(row).find('.inputProveedorDistinto').val()
                    };

                    lstPartidas.push(partida);
                });
            } else {
                tblPartidasEditar.find('tbody tr').each(function (idx, row) {
                    let rowData = tblPartidasEditar.DataTable().row($(row)).data();

                    if (rowData.cantidadRequisicion > 0) {
                        if (rowData.cantidadRequisicion < rowData.cantidad) {
                            flagColocadaMayor = true;
                        }
                    }

                    let partida = {
                        cc: compra.cc,
                        numero: compra.numero,
                        partida: rowData.partida,
                        insumo: rowData.insumo,

                        fecha_entrega: $(row).find('.inputEntrega').val(),
                        fecha_entregaString: $(row).find('.inputEntrega').val(),

                        cantidad: rowData.cantidad,
                        precio: rowData.precio,
                        importe: rowData.importe,
                        ajuste_cant: 0,
                        ajuste_imp: 0,
                        num_requisicion: inputNumero.val(),
                        part_requisicion: rowData.partida,
                        cant_recibida: 0,
                        imp_recibido: 0,
                        fecha_recibido: null,
                        cant_canc: 0,
                        imp_canc: 0,

                        acum_ant: 0,

                        max_orig: 0,
                        max_ppto: 0,
                        area: rowData.area,
                        cuenta: rowData.cuenta,
                        noEconomico: rowData.noEconomico,
                        porcent_iva: +$(row).find('.inputPorcentajeIVA').val(),
                        iva: unmaskNumero6DCompras($(row).find('.inputMontoIVA').val()),
                        exento_iva: $(row).find('.checkboxExentoIVA').prop('checked'),
                        partidaDescripcion: rowData.partidaDescripcion
                    };

                    lstPartidas.push(partida);
                });
            }

            let lstRetenciones = [];

            if (tblRetenciones.DataTable().rows().data().length > 0) {
                tblRetenciones.find('tbody tr').each(function (idx, row) {
                    let rowData = tblRetenciones.DataTable().row($(row)).data();

                    let retencion = {
                        cc: compra.cc,
                        numero: compra.numero,
                        id_cpto: rowData.id_cpto,
                        orden: ++idx,
                        cantidad: rowData.cantidad,
                        porc_ret: rowData.porc_ret,
                        importe: rowData.importe,

                        facturado: 0,
                        retenido: 0,

                        aplica: 1,

                        forma_pago: 0,
                        tm_descto: null,

                        calc_iva: rowData.calc_iva,
                        bit_afecta_oc: rowData.bit_afecta_oc,
                        afecta_fac: rowData.afecta_fac,
                        facturado_ret: 0,
                        facturado_iva: 0,
                        facturado_total: 0,
                        retenido_ret: 0,
                        retenido_iva: 0,
                        retenido_total: 0
                    };

                    lstRetenciones.push(retencion);
                });
            }

            compra.lstPartidas = lstPartidas;
            compra.lstRetenciones = lstRetenciones;

            if (!flagColocadaMayor) {
                return compra;
            } else {
                return null;
            }
        }

        function getProveedorPorNumero() {
            $.blockUI({ message: 'Procesando...' });

            getProveedorInfo(inputProvNum.val(), _empresaActual == 6 ? selectTipoCompraVenta.find('option:selected').text() : '').done(function (response) {
                if (response != null) {
                    if (response.cancelado != 'C') {
                        inputProvNom.val(response.id);
                        selectMoneda.val(response.moneda);
                        inputTipoCambio.val(maskNumero6DComprasColombia(response.monedaTipoCambio));

                        if (_empresaActual == 6) {
                            selectCuentaCorriente.empty();
                            selectCuentaCorriente.append(`<option value="">--Seleccione--</option>`);

                            if (response.listaCuentasCorrientes != null && response.listaCuentasCorrientes.length > 0) {
                                response.listaCuentasCorrientes.forEach((element) => {
                                    selectCuentaCorriente.append(`<option value="${element.Value}" moneda="${element.TextoOpcional}">${element.Text}</option>`);
                                });
                            }

                            selectCuentaCorriente.val('');
                            selectFormaPago.val('');

                            if (response.PERU_cuentaCorriente != null && response.PERU_cuentaCorriente != '') {
                                selectCuentaCorriente.find(`option:contains('${response.PERU_cuentaCorriente.split('-')[2]}')`).attr('selected', true);
                            }

                            if (response.PERU_formaPago != '' && response.PERU_formaPago != null) {
                                selectFormaPago.val(response.PERU_formaPago);
                            }
                        }

                        llenarInputProveedorDistinto(inputProvNum.val());
                    } else {
                        AlertaGeneral(`Alerta`, `El proveedor "${response.label} - ${response.id}" no está activo.`);
                        inputProvNum.val('');
                        inputProvNom.val('');

                        llenarInputProveedorDistinto('');
                    }

                    if (response.proveedorSubcontratistaBloqueado) {
                        Alert2Warning('El proveedor está bloqueado como subcontratista.');
                    } else if (_flagValidacionInsumoSubcontratista) {
                        if (!response.proveedorSubcontratistaExiste) {
                            //Alert2Warning('El proveedor no está registrado como subcontratista.') bloqueado por mientras andrea felix se pone las pilas
                        }
                    }
                } else {
                    inputProvNum.val('');
                    inputProvNom.val('');

                    llenarInputProveedorDistinto('');
                }
            }).then($.unblockUI);

            getPreciosPorProveedor();
        }
        function getCompradorPorNumero() {
            if (_empresaActual != 6) {
                $.blockUI({ message: 'Procesando...' });

                getEmpleadoUsuarioEK(inputCompNum.val()).done(function (response) {
                    inputCompNom.val(response);
                }).then($.unblockUI);
            }
        }
        function getSolicitoPorNumero() {
            if (_empresaActual != 6) {
                $.blockUI({ message: 'Procesando...' });

                getEmpleadoUsuarioEK(inputSolNum.val()).done(function (response) {
                    inputSolNom.val(response);
                }).then($.unblockUI);
            }
        }
        function getAutorizoPorNumero() {
            if (_empresaActual != 6) {
                $.blockUI({ message: 'Procesando...' });

                getEmpleadoUsuarioEK(inputAutNum.val()).done(function (response) {
                    inputAutNom.val(response);
                }).then($.unblockUI);
            }
        }
        function getEmpleadoRecepPorNumero() {
            $.blockUI({ message: 'Procesando...' });

            getEmpleadoUsuarioEK(inputEmpNum.val()).done(function (response) {
                if (response != '') {
                    inputEmpNom.val(response);
                } else {
                    AlertaGeneral('Alerta', 'No se encontró el empleado.');
                    inputEmpNum.val('');
                    inputEmpNom.val('');
                }

            }).then($.unblockUI);;
        }
        function getAlmacen() {
            $.blockUI({ message: 'Procesando...' });

            getAlmacenNombre(inputAlmNum.val()).done(function (response) {
                if (response != '') {
                    inputAlmNom.val(response);
                } else {
                    AlertaGeneral('Alerta', 'No se encontró el almacén.');
                    inputAlmNum.val('');
                    inputAlmNom.val('');
                }

            }).then($.unblockUI);
        }

        function verificarPrecioSegurido(row) {
            const costoPromedio = unmaskNumero6DCompras(row.find('.inputCostoPromedio').val());
            if (costoPromedio && costoPromedio != 0) {
                const precio = unmaskNumero6DCompras(row.find('.inputPrecio').val());

                const precioMinimoSugerido = costoPromedio * .9;
                const precioMaximoSugerido = costoPromedio * 1.1;


                if (precio < precioMinimoSugerido) {
                    AlertaGeneral('Aviso', 'El precio establecido para el insumo está muy por debajo del costo promedio.');
                } else if (precio > precioMaximoSugerido) {
                    AlertaGeneral('Aviso', 'El precio establecido para el insumo está muy por arriba del costo promedio general.');
                }
            }
        }

        function calcularImportePorRenglon(row) {
            const rowData = tblPartidas.DataTable().row(row).data();

            const colocada = unmaskNumero(row.find('.inputColocada').val());
            const precio = unmaskNumero6DCompras(row.find('.inputPrecio').val());

            let importe = colocada * precio;

            row.find('.inputImporte').val(maskNumero6DComprasColombia(importe));
        }

        function calcularResultados() {
            let subTotal = 0;

            tblPartidas.find('tbody tr').each(function (idx, row) {
                let colocada = unmaskNumero($(row).find('.inputColocada').val());
                let precio = unmaskNumero6DCompras($(row).find('.inputPrecio').val());
                let importe = colocada * precio;

                subTotal += importe;
            });

            inputSubTotal.val(maskNumero6DComprasColombia(subTotal));
            // inputIVAPorcentaje.change();
        }

        function calcularResultadosEditar() {
            let subTotal = 0;

            tblPartidasEditar.find('tbody tr').each(function (idx, row) {
                let cantidad = unmaskNumero($(row).find('.inputCantidad').val());
                let precio = unmaskNumero6DCompras($(row).find('.inputPrecio').val());
                let importe = cantidad * precio;

                subTotal += importe;
            });

            inputSubTotal.val(maskNumero6DComprasColombia(subTotal));
            // inputIVAPorcentaje.change();
        }

        function calcularRetenciones() {
            let totalRetenciones = 0;
            let total = unmaskNumero6DCompras(inputTotal.val());

            tblRetenciones.find('tbody tr').each(function (idx, row) {
                let rowData = tblRetenciones.DataTable().row(row).data();

                if (rowData != undefined) {
                    totalRetenciones += rowData.cantidad;
                }
            });

            inputRetencion.val(maskNumero6DComprasColombia(totalRetenciones));
            inputTotalFinal.val(maskNumero6DComprasColombia(total - totalRetenciones));
        }

        function setInsumoDesc(e, ui) {
            let row = $(this).closest('tr');

            row.find('.inputInsumoDesc').val(ui.item.id);
            // row.find('.inputCostoPromedio').val(maskNumero6DComprasColombia(ui.item.costoPromedio));
        }

        function setInsumoBusqPorDesc(e, ui) {
            let row = $(this).closest('tr');

            row.find('.inputInsumo').val(ui.item.id);
            row.find('.inputInsumoDesc').val(ui.item.value);
            // row.find('.inputCostoPromedio').val(maskNumero6DComprasColombia(ui.item.costoPromedio));
        }

        function validarInsumo(e, ul) {
            if (ul.item == null) {
                let row = $(this).closest('tr');
                row.find('.inputInsumo').val('');
                row.find('.inputInsumoDesc').val('');
                // row.find('.inputCostoPromedio').val(maskNumero6DComprasColombia(0));
            }
        }

        function cargarCompra() {
            _variableCompraNuevaEditar = 2;

            btnBorrarCompra.attr('disabled', true);

            inputProvNum.attr('disabled', true);
            inputProvNom.attr('disabled', true);

            divAutorecepcionable.children().attr('disabled', false);

            selectCC.val('');
            inputNumero.val('');
            btnCancelarCompra.attr('disabled', true);

            if (_flagPeriodoContable) {
                btnGuardarNuevaCompra.attr('disabled', false);
            } else {
                btnGuardarNuevaCompra.attr('disabled', true);
            }

            $('#divTablaPartidas').css('display', 'none');
            $('#divTablaPartidasEditar').css('display', 'block');

            fieldsetEstatusCompra.css('display', 'none');

            tblPartidas.DataTable().columns.adjust();
            tblPartidasEditar.DataTable().columns.adjust();

            limpiarInformacion();

            $.blockUI({ message: 'Procesando...' });

            let flagPuedeGuardar = false;

            getCompra().done(response => {
                if (response.success) {
                    if (response.info.cc != null) {
                        llenarInformacion(response.info);

                        if (response.info.ST_OC != 'A') {
                            btnBorrarCompra.attr('disabled', false);
                            inputIVAPorcentaje.attr('disabled', false);
                            inputIVANumero.attr('disabled', false);

                            $('#btnEscogerProv1, #btnEscogerProv2, #btnEscogerProv3').attr('disabled', false);
                        } else {
                            // inputIVAPorcentaje.attr('disabled', true);
                            // inputIVANumero.attr('disabled', true);

                            $('#btnEscogerProv1, #btnEscogerProv2, #btnEscogerProv3').attr('disabled', true);
                        }

                        inputNumeroEditar.attr('num_requisicion', response.partidas[0].num_requisicion);

                        limpiarTabla(tblPartidas);
                        limpiarTabla(tblPartidasEditar);

                        AddRows(tblPartidasEditar, response.partidas);
                        AddRows(tblPagos, response.pagos);
                        AddRows(tblRetenciones, response.retenciones);

                        tblPartidasEditar.find('tbody tr:eq(0) td:eq(0)').click();

                        if (response.info.tieneCuadro) {
                            btnVerCuadro.css('display', 'inline-block');
                            btnNoTieneCuadro.css('display', 'none');
                            selectFolioCuadro.fillCombo('/Enkontrol/OrdenCompra/FillComboFolioCuadro', { cuadrosExistentes: response.info.cuadrosExistentes }, false);
                        } else {
                            btnVerCuadro.css('display', 'none');
                            btnNoTieneCuadro.css('display', 'inline-block');
                        }

                        if (response.info.estatus == 'C') {
                            btnImprimir.attr('disabled', false);
                            fieldsetEstatusCompra.css('display', 'block');
                            labelEstatusCompra.css('color', 'red');
                            labelEstatusCompra.text('CANCELADA');
                        } else {
                            if (response.info.ST_OC == 'A') {
                                btnImprimir.attr('disabled', false);
                                fieldsetEstatusCompra.css('display', 'block');
                                labelEstatusCompra.css('color', 'green');
                                labelEstatusCompra.text('AUTORIZADA');
                            } else {
                                btnImprimir.attr('disabled', true);
                                fieldsetEstatusCompra.css('display', 'block');
                                labelEstatusCompra.css('color', 'red');
                                labelEstatusCompra.text('NO AUTORIZADA');
                            }
                        }

                        if (response.info.estatus == 'C') {
                            btnCancelarCompra.attr('disabled', true);
                        } else {
                            if (response.info.ST_OC == 'A' && response.info.st_impresa == 'I') { //Para cancelar compras deben estar autorizadas e impresas.
                                let flagTieneMovimientos = response.partidas.some(function (x) {
                                    return x.surtido > 0;
                                });

                                if (!flagTieneMovimientos) {
                                    puedeCancelar();
                                    inputIVAPorcentaje.attr('disabled', false);
                                    inputIVANumero.attr('disabled', false);
                                } else {
                                    btnCancelarCompra.attr('disabled', true);
                                    inputIVAPorcentaje.attr('disabled', true);
                                    inputIVANumero.attr('disabled', true);
                                }
                            } else {
                                btnCancelarCompra.attr('disabled', true);
                            }
                        }

                        if (response.partidas.some(x => x.inventariado == 'I')) {
                            divAutorecepcionable.children().attr('disabled', true);
                        }

                        if (response.info.estatus == 'C') {
                            flagPuedeGuardar = false;
                        } else {
                            flagPuedeGuardar = response.info.flagPuedeGuardar;
                        }

                        if (
                            // response.info.st_impresa == 'I' && 
                            response.info.colocada == false) {
                            _Colocada = false;
                            btnOpenEnviar.prop("disabled", false);
                            inputCorreoProveedor.val(response.info.correoProveedor);
                        } else {
                            _Colocada = true;
                            inputCorreoProveedor.val("");
                            btnOpenEnviar.prop("disabled", true);
                        }
                    } else {
                        AlertaGeneral('Alerta', 'No se encontró información.');

                        limpiarInformacion();

                        limpiarTabla(tblPartidas);
                        limpiarTabla(tblPartidasEditar);
                        limpiarTabla(tblPagos);
                        limpiarTabla(tblRetenciones);

                        textAreaDescPartida.val('');

                        btnVerCuadro.css('display', 'none');
                        btnNoTieneCuadro.css('display', 'none');
                        btnImprimir.attr('disabled', true);
                    }
                } else {
                    AlertaGeneral('Alerta', response.message);

                    btnVerCuadro.css('display', 'none');
                    btnNoTieneCuadro.css('display', 'none');
                    btnImprimir.attr('disabled', true);
                }
            }).then(function () {
                $.unblockUI;
                tblPartidas.DataTable().columns.adjust();
                tblPartidasEditar.DataTable().columns.adjust();

                checkRenglonInsumoBloqueadoEditar();

                if (_flagPeriodoContable) {
                    btnGuardarNuevaCompra.attr('disabled', !flagPuedeGuardar);
                } else {
                    btnGuardarNuevaCompra.attr('disabled', true);
                }
            });
        }

        function setProveedor(e, ui) {
            inputProvNum.val(ui.item.id);
            inputProvNom.val(ui.item.label);

            getProveedorInfo(inputProvNum.val(), _empresaActual == 6 ? selectTipoCompraVenta.find('option:selected').text() : '').done(function (response) {
                if (response != null) {
                    if (response.cancelado != 'C') {
                        inputProvNom.val(response.id);
                        selectMoneda.val(response.moneda);
                        inputTipoCambio.val(maskNumero6DComprasColombia(response.monedaTipoCambio));

                        if (_empresaActual == 6) {
                            selectCuentaCorriente.empty();
                            selectCuentaCorriente.append(`<option value="">--Seleccione--</option>`);

                            if (response.listaCuentasCorrientes != null && response.listaCuentasCorrientes.length > 0) {
                                response.listaCuentasCorrientes.forEach((element) => {
                                    selectCuentaCorriente.append(`<option value="${element.Value}" moneda="${element.TextoOpcional}">${element.Text}</option>`);
                                });
                            }

                            selectCuentaCorriente.val('');
                            selectFormaPago.val('');

                            if (response.PERU_cuentaCorriente != null && response.PERU_cuentaCorriente != '') {
                                selectCuentaCorriente.find(`option:contains('${response.PERU_cuentaCorriente}')`).attr('selected', true);
                            }

                            if (response.PERU_formaPago != '' && response.PERU_formaPago != null) {
                                selectFormaPago.val(response.PERU_formaPago);
                            }
                        }

                        llenarInputProveedorDistinto(ui.item.id);
                    } else {
                        AlertaGeneral(`Alerta`, `El proveedor "${response.label} - ${response.id}" no está activo.`);
                        inputProvNum.val('');
                        inputProvNom.val('');

                        llenarInputProveedorDistinto('');
                    }

                    if (response.proveedorSubcontratistaBloqueado) {
                        Alert2Warning('El proveedor está bloqueado como subcontratista.');
                    } else if (_flagValidacionInsumoSubcontratista) {
                        if (!response.proveedorSubcontratistaExiste) {
                            //Alert2Warning('El proveedor no está registrado como subcontratista.')
                        }
                    }
                }
            }).then($.unblockUI);

            getPreciosPorProveedor();
        }

        function validarProveedor(e, ul) {
            if (ul.item == null) {
                // inputProvNum.val('');
                // inputProvNom.val('');
            }
        }

        function reInicializarCamposRenglon(row, rowData, flagEditar) {
            let inputInsumo = $(row).find('.inputInsumo');
            let inputInsumoDesc = $(row).find('.inputInsumoDesc');
            let selectAreaCuenta = $(row).find('.selectAreaCuenta');
            let inputRequerido = $(row).find('.inputRequerido');
            //Renglón Editar
            let inputEntrega = $(row).find('.inputEntrega');

            inputInsumo.getAutocompleteValid(setInsumoDesc, validarInsumo, { cc: !flagEditar ? selectCC.val() : selectCCEditar.val() }, '/Enkontrol/Requisicion/getInsumos');
            inputInsumoDesc.getAutocompleteValid(setInsumoBusqPorDesc, validarInsumo, { cc: !flagEditar ? selectCC.val() : selectCCEditar.val() }, '/Enkontrol/Requisicion/getInsumosDesc');

            selectAreaCuenta.fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: !flagEditar ? selectCC.val() : selectCCEditar.val() }, false, "000-000");

            let optionExistente = selectAreaCuenta.find(`option[value="${rowData.noEconomico}"]`)[0];

            if (rowData.noEconomico != '' && optionExistente == undefined) {
                selectAreaCuenta.append(`
                    <option value="${rowData.noEconomico}" name="undefined" data-prefijo="0" data-comboid="undefined">${rowData.noEconomico}</option>
                `);
            }

            if (rowData.noEconomico == '') {
                selectAreaCuenta.find('option[value=000-000]').attr('selected', true);
            } else {
                selectAreaCuenta.find('option[value=' + rowData.noEconomico + ']').attr('selected', true);
            }

            // if (flagEditar && selectAreaCuenta.find('option[value=' + rowData.area + '][data-prefijo=' + rowData.cuenta + ']').length == 0) {
            //     let option = document.createElement('option');
            //     let areaFormat = rowData.area.toString().padStart(3, '0');
            //     let cuentaFormat = rowData.cuenta.toString().padStart(3, '0');

            //     $(option).text(`${areaFormat}-${cuentaFormat}`);
            //     $(option).attr('value', rowData.area);
            //     $(option).attr('data-prefijo', rowData.cuenta);

            //     selectAreaCuenta.append(option.outerHTML);
            //     // sortSelect(selectAreaCuenta.get(0));
            //     selectAreaCuenta.find('option[value=' + rowData.area + '][data-prefijo=' + rowData.cuenta + ']').attr('selected', true);
            // }

            !flagEditar ? inputRequerido.datepicker().datepicker() : inputEntrega.datepicker().datepicker();

            if (rowData.flagBloquearPartidaSurtida) {
                $(row).addClass('partidaBloqueada');
                $(row).removeClass('active');
                $(row).find('input').attr('disabled', true);
                $(row).find('select').attr('disabled', true);
            } else if (rowData.flagBloquearPartidaEntrada) {
                $(row).addClass('partidaBloqueada');
                $(row).removeClass('active');
                $(row).find('input').attr('disabled', true);
                $(row).find('select').attr('disabled', true);

                $(row).find('.inputCantidad').attr('disabled', false); //Se habilita el input de cantidad cuando es surtido parcial.
            }
        }

        function sortSelect(selElem) {
            var tmpAry = new Array();
            for (var i = 0; i < selElem.options.length; i++) {
                tmpAry[i] = new Array();
                tmpAry[i][0] = selElem.options[i].text;
                tmpAry[i][1] = selElem.options[i].value;
            }
            tmpAry.sort();
            while (selElem.options.length > 0) {
                selElem.options[0] = null;
            }
            for (var i = 0; i < tmpAry.length; i++) {
                var op = new Option(tmpAry[i][0], tmpAry[i][1]);
                selElem.options[i] = op;
            }
            return;
        }

        function llenarPanelDerechoProv(cuadro) {



            if (_empresaActual == 6) {
                inputProv1Num.val(cuadro.PERU_prov1);
                inputProv2Num.val(cuadro.PERU_prov2);
                inputProv3Num.val(cuadro.PERU_prov3);

            } else {
                inputProv1Num.val(cuadro.prov1);
                inputProv2Num.val(cuadro.prov2);
                inputProv3Num.val(cuadro.prov3);
            }
            inputProv1Desc.val(cuadro.nombre_prov1);
            agregarTooltip(inputProv1Desc, cuadro.nombre_prov1);
            inputProv2Desc.val(cuadro.nombre_prov2);
            agregarTooltip(inputProv2Desc, cuadro.nombre_prov2);
            inputProv3Desc.val(cuadro.nombre_prov3);
            agregarTooltip(inputProv3Desc, cuadro.nombre_prov3);

            inputPrimerSubtotalProv1Num.val(maskNumero6DComprasColombia(cuadro.sub_total1));
            inputPrimerSubtotalProv1Moneda.val(cuadro.moneda1Desc);
            inputPrimerSubtotalProv2Num.val(maskNumero6DComprasColombia(cuadro.sub_total2));
            inputPrimerSubtotalProv2Moneda.val(cuadro.moneda2Desc);
            inputPrimerSubtotalProv3Num.val(maskNumero6DComprasColombia(cuadro.sub_total3));
            inputPrimerSubtotalProv3Moneda.val(cuadro.moneda3Desc);

            inputDescuentoProv1.val(maskNumero6DComprasColombia(cuadro.porcent_dcto1));
            inputDescuentoProv2.val(maskNumero6DComprasColombia(cuadro.porcent_dcto2));
            inputDescuentoProv3.val(maskNumero6DComprasColombia(cuadro.porcent_dcto3));

            let segundoSubTotal1 = cuadro.sub_total1 - cuadro.dcto1;
            let segundoSubTotal2 = cuadro.sub_total2 - cuadro.dcto2;
            let segundoSubTotal3 = cuadro.sub_total3 - cuadro.dcto3;

            inputSegundoSubtotalProv1Num.val(maskNumero6DComprasColombia(segundoSubTotal1));
            inputSegundoSubtotalProv1Moneda.val(cuadro.moneda1Desc);
            inputSegundoSubtotalProv2Num.val(maskNumero6DComprasColombia(segundoSubTotal2));
            inputSegundoSubtotalProv2Moneda.val(cuadro.moneda2Desc);
            inputSegundoSubtotalProv3Num.val(maskNumero6DComprasColombia(segundoSubTotal3));
            inputSegundoSubtotalProv3Moneda.val(cuadro.moneda3Desc);

            inputIVAProv1.val(cuadro.porcent_iva1);
            inputIVAProv2.val(cuadro.porcent_iva2);
            inputIVAProv3.val(cuadro.porcent_iva3);

            inputTotalProv1Num.val(maskNumero6DComprasColombia(cuadro.total1));
            inputTotalProv1Moneda.val(cuadro.moneda1Desc);
            inputTotalProv2Num.val(maskNumero6DComprasColombia(cuadro.total2));
            inputTotalProv2Moneda.val(cuadro.moneda2Desc);
            inputTotalProv3Num.val(maskNumero6DComprasColombia(cuadro.total3));
            inputTotalProv3Moneda.val(cuadro.moneda3Desc);

            inputFletesProv1Num.val(maskNumero6DComprasColombia(cuadro.fletes1));
            inputFletesProv1Moneda.val(cuadro.moneda1Desc);
            inputFletesProv2Num.val(maskNumero6DComprasColombia(cuadro.fletes2));
            inputFletesProv2Moneda.val(cuadro.moneda2Desc);
            inputFletesProv3Num.val(maskNumero6DComprasColombia(cuadro.fletes3));
            inputFletesProv3Moneda.val(cuadro.moneda3Desc);

            inputImportacionProv1Num.val(maskNumero6DComprasColombia(cuadro.gastos_imp1));
            inputImportacionProv1Moneda.val(cuadro.moneda1Desc);
            inputImportacionProv2Num.val(maskNumero6DComprasColombia(cuadro.gastos_imp2));
            inputImportacionProv2Moneda.val(cuadro.moneda2Desc);
            inputImportacionProv3Num.val(maskNumero6DComprasColombia(cuadro.gastos_imp3));
            inputImportacionProv3Moneda.val(cuadro.moneda3Desc);

            let granTotal1 = cuadro.total1 + cuadro.fletes1 + cuadro.gastos_imp1;
            let granTotal2 = cuadro.total2 + cuadro.fletes2 + cuadro.gastos_imp2;
            let granTotal3 = cuadro.total3 + cuadro.fletes3 + cuadro.gastos_imp3;

            inputGranTotalProv1Num.val(maskNumero6DComprasColombia(granTotal1));
            inputGranTotalProv1Moneda.val(cuadro.moneda1Desc);
            inputGranTotalProv2Num.val(maskNumero6DComprasColombia(granTotal2));
            inputGranTotalProv2Moneda.val(cuadro.moneda2Desc);
            inputGranTotalProv3Num.val(maskNumero6DComprasColombia(granTotal3));
            inputGranTotalProv3Moneda.val(cuadro.moneda3Desc);

            inputTipoCambioProv1.val(maskNumero6DComprasColombia(cuadro.tipo_cambio1));
            inputTipoCambioProv2.val(maskNumero6DComprasColombia(cuadro.tipo_cambio2));
            inputTipoCambioProv3.val(maskNumero6DComprasColombia(cuadro.tipo_cambio3));

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
            let tablaPartidas = tblPartidasCuadro.DataTable();

            tablaPartidas.columns(3).header().to$().text(cuadro.nombre_prov1);
            tablaPartidas.columns(4).header().to$().text(cuadro.nombre_prov2);
            tablaPartidas.columns(5).header().to$().text(cuadro.nombre_prov3);

            AddRows(tblPartidasCuadro, cuadroDetalle);
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

            let hoy = new Date().toLocaleDateString();

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

            let tablaPartidas = tblPartidasCuadro.DataTable();

            tablaPartidas.columns(3).header().to$().text('');
            tablaPartidas.columns(4).header().to$().text('');
            tablaPartidas.columns(5).header().to$().text('');

            tablaPartidas.clear().draw();

            limpiarUltimaCompra();
        }

        function initTablePartidasCuadro() {
            tblPartidasCuadro.DataTable({
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
                    tblPartidasCuadro.on('click', 'tr', function (e) {
                        let rowData = tblPartidasCuadro.DataTable().row($(this).closest('tr')).data();

                        tblPartidasCuadro.find('tr').removeClass('partidaSeleccionada');
                        $(this).closest('tr').addClass('partidaSeleccionada');

                        let partidaCuadro = {
                            cc: rowData.cc,
                            numero: rowData.numero,
                            folio: rowData.folio,
                            partida: rowData.partida,
                            insumo: rowData.insumo
                        }

                        getUltimaCompra(partidaCuadro).done(function (response) {
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
                        });
                    });
                },
                columns: [
                    { data: 'partida', title: 'Pda' },
                    {
                        data: 'insumo',
                        render: function (data, type, row) {
                            return row.insumo + '-' + row.descripcion;
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

                            $(input).attr('value', unmaskNumero6D(row.cantidad.toString()));
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

                            $(input).attr('value', maskNumero6DComprasColombia(row.precio1));
                            if (_empresaActual == 6) {
                                if (row.moneda1 == 4) {

                                    $(span).text("SOL");

                                } else {
                                    $(span).text("DLS");

                                }

                            } else {
                                $(span).text(row.moneda1);

                            }
                            // $(span).text(row.moneda1);

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

                            $(input).attr('value', maskNumero6DComprasColombia(row.precio2));
                            $(input).attr('value', maskNumero6DComprasColombia(row.precio1));
                            if (_empresaActual == 6) {
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

                            $(input).attr('value', maskNumero6DComprasColombia(row.precio3));

                            if (_empresaActual == 6) {
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
                    { className: "dt-center", "targets": "_all" }
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
                            return maskNumero6DComprasColombia(row.precio) + ' ' + row.monedaDesc;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function limpiarUltimaCompra() {
            tblUltimaCompra.DataTable().clear().draw();
        }

        function getCuadroDetalle(folio) {
            let cuadro = {
                cc: _variableCompraNuevaEditar == 1 ? selectCC.val() : selectCCEditar.val(),
                numero: _variableCompraNuevaEditar == 1 ? inputNumero.val() : inputNumeroEditar.attr('num_requisicion'),
                folio: folio
            }

            getCuadroDet(cuadro).done(function (response) {
                if (response.success) {
                    limpiarCuadro();

                    let cuadro = response.data;
                    let cuadroDetalle = response.data.detalleCuadro;

                    if (cuadro.tieneCuadro) {
                        llenarPanelDerechoProv(cuadro);
                        llenarTablaPartidas(cuadro, cuadroDetalle);
                        tblPartidasCuadro.find('tbody tr:eq(0)').click();
                    } else {
                        llenarTablaPartidas(cuadro, cuadroDetalle);
                        tblPartidasCuadro.find('tbody tr:eq(0)').click();
                    }
                } else {
                    AlertaGeneral('Alerta', 'Error al consultar la información.');
                    limpiarCuadro();
                }
            }).always(function () {
                tblPartidasCuadro.DataTable().columns.adjust();
                tblUltimaCompra.DataTable().columns.adjust();
            });
        }

        function verReporteCompra(cc, numero) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", '/Reportes/Vista.aspx?idReporte=113' + '&cc=' + cc + '&numero=' + numero);
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
                if (_Colocada == true) {
                    btnOpenEnviar.prop("disabled", true);
                }
                else {
                    btnOpenEnviar.prop("disabled", false);
                }

            });
        }

        function AlertaCancelarCompra(titulo, mensaje) {
            if (mensaje == null) {
                mensaje = "Error en el resultado de la petición, favor de intentar de nuevo.";
            }

            $("#dialogalertaGeneral").removeClass('hide');
            $("#txtComentarioAlerta").html(mensaje);

            var opt = {
                title: titulo,
                autoOpen: false,
                draggable: false,
                resizable: false,
                modal: true,
                maxWidth: 600,
                minWidth: 400,
                position: {
                    my: "center",
                    at: "center",
                    within: $(".RenderBody")
                },
                buttons: [
                    {
                        text: "Sí",
                        click: function () {
                            cancelarCompra();

                            $("#dialogalertaGeneral").addClass('hide');
                            $(this).dialog("close");
                        }
                    },
                    {
                        text: "No",
                        click: function () {
                            $("#dialogalertaGeneral").addClass('hide');
                            $(this).dialog("close");
                        }
                    }
                ]
            };

            var theDialog = $("#dialogalertaGeneral").dialog(opt);

            theDialog.dialog("open");
        }

        function cancelarCompra() {
            let cc = selectCCEditar.val();
            let numero = inputNumeroEditar.val();

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/CancelarCompra', { cc, numero })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha cancelado la compra`);
                        cargarCompra();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cancelarParcialCompra() {
            let compra = getInfoCompra(true);

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/CancelarParcialCompra', { compra: compra })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha cancelado la compra`);
                        cargarCompra();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al modificar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
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
        function quitarTooltip(elemento) {
            $(elemento).removeAttr('data-toggle');
            $(elemento).removeAttr('data-placement');
            $(elemento).removeAttr('title');
        }

        function getPreciosPorProveedor() {
            let numeroProveedor = inputProvNum.val();

            if (numeroProveedor > 0) {
                let cuadro = {
                    cc: _variableCompraNuevaEditar == 1 ? selectCC.val() : selectCCEditar.val(),
                    numero: _variableCompraNuevaEditar == 1 ? inputNumero.val() : inputNumeroEditar.attr('num_requisicion'),
                    folio: 1
                }

                getCuadroDet(cuadro).done(function (response) {
                    if (response.success) {
                        let cuadro = response.data;

                        if (cuadro.tieneCuadro) {
                            if (_variableCompraNuevaEditar == 1) {
                                let cc = selectCC.val();
                                let numeroRequisicion = inputNumero.val();

                                $.blockUI({ message: 'Procesando...' });
                                $.post('/Enkontrol/OrdenCompra/GetPreciosPorProveedor', { cc, numeroRequisicion, numeroProveedor })
                                    .always($.unblockUI)
                                    .then(response => {
                                        if (response.cuadro != null) {
                                            var cuadro = response.cuadro;
                                            var cuadroDetalle = response.cuadroDetalle;

                                            cuadroDetalle.forEach(function (x) {
                                                let renglonPartida = $(tblPartidas).find(`tbody tr:eq(${x.partida - 1})`);

                                                if (renglonPartida != null) {
                                                    let inputPrecio = renglonPartida.find('.inputPrecio');

                                                    if (_empresaActual == 6) {
                                                        if (numeroProveedor == cuadro[0].PERU_prov1) {
                                                            selectMoneda.val(cuadro[0].moneda1);
                                                            inputPrecio.val(x.precio1);
                                                            inputPrecio.change();
                                                        } else if (numeroProveedor == cuadro[0].PERU_prov2) {
                                                            selectMoneda.val(cuadro[0].moneda2);
                                                            inputPrecio.val(x.precio2);
                                                            inputPrecio.change();
                                                        } else if (numeroProveedor == cuadro[0].PERU_prov3) {
                                                            selectMoneda.val(cuadro[0].moneda3);
                                                            inputPrecio.val(x.precio3);
                                                            inputPrecio.change();
                                                        }
                                                        selectMoneda.trigger('change');
                                                    } else {
                                                        if (numeroProveedor == cuadro[0].prov1) {
                                                            inputPrecio.val(x.precio1);
                                                            inputPrecio.change();
                                                        } else if (numeroProveedor == cuadro[0].prov2) {
                                                            inputPrecio.val(x.precio2);
                                                            inputPrecio.change();
                                                        } else if (numeroProveedor == cuadro[0].prov3) {
                                                            inputPrecio.val(x.precio3);
                                                            inputPrecio.change();
                                                        }
                                                    }
                                                }
                                            });
                                        }
                                    }, error => {
                                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                                    }
                                    );
                            } else {

                            }
                        }
                    }
                });
            } else {
                AlertaGeneral(`Alerta`, `Número de Proveedor no válido.`);
            }
        }

        function getPreciosPorProveedorEditar(folioCuadro, numeroProveedor) {

            if (numeroProveedor > 0) {
                let cuadro = {
                    cc: selectCCEditar.val(),
                    numero: inputNumeroEditar.attr('num_requisicion'),
                    folio: folioCuadro
                }

                getCuadroDet(cuadro).done(function (response) {
                    if (response.success) {
                        let cuadro = response.data;

                        if (cuadro.tieneCuadro) {
                            let cc = cuadro.cc;
                            let numeroRequisicion = inputNumeroEditar.attr('num_requisicion');

                            $.blockUI({ message: 'Procesando...' });
                            $.post('/Enkontrol/OrdenCompra/GetPreciosPorProveedor', { cc, numeroRequisicion, numeroProveedor })
                                .always($.unblockUI)
                                .then(response => {
                                    if (response.cuadro != null) {
                                        var cuadro = response.cuadro;
                                        var cuadroDetalle = response.cuadroDetalle;

                                        cuadroDetalle.forEach(function (x) {
                                            let renglonPartida = $(tblPartidasEditar).find(`tbody tr:eq(${x.partida - 1})`);

                                            if (renglonPartida != null) {
                                                let inputPrecio = renglonPartida.find('.inputPrecio');

                                                if (numeroProveedor == cuadro[0].prov1) {
                                                    inputPrecio.val(x.precio1);
                                                    inputPrecio.change();
                                                } else if (numeroProveedor == cuadro[0].prov2) {
                                                    inputPrecio.val(x.precio2);
                                                    inputPrecio.change();
                                                } else if (numeroProveedor == cuadro[0].prov3) {
                                                    inputPrecio.val(x.precio3);
                                                    inputPrecio.change();
                                                }
                                            }
                                        });
                                    }
                                }, error => {
                                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                                }
                                );
                        }
                    }
                });
            } else {
                AlertaGeneral(`Alerta`, `Número de Proveedor no válido.`);
            }
        }

        function checkRenglonInsumoBloqueado() {
            let existeRenglonBloqueado = tblPartidas.find('tbody .renglonInsumoBloqueado').length > 0;

            if (_flagPeriodoContable) {
                btnGuardarNuevaCompra.attr('disabled', existeRenglonBloqueado);
            } else {
                btnGuardarNuevaCompra.attr('disabled', true);
            }
        }

        function checkRenglonInsumoBloqueadoEditar() {
            let existeRenglonBloqueado = tblPartidasEditar.find('tbody .renglonInsumoBloqueado').length > 0;

            if (_flagPeriodoContable) {
                btnGuardarNuevaCompra.attr('disabled', existeRenglonBloqueado);
            } else {
                btnGuardarNuevaCompra.attr('disabled', true);
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

        function borrarCompra() {
            if (_flagPeriodoContable) {
                let cc = selectCCEditar.val();
                let numero = inputNumeroEditar.val();

                dialogConfirmarBorrado.text(`¿Desea borrar la compra ${cc}-${numero}?`);

                dialogConfirmarBorrado.dialog({
                    width: '30%',
                    modal: true,
                    buttons: {
                        'Borrar': function () {
                            $.blockUI({ message: 'Procesando...' });
                            $.post('/Enkontrol/OrdenCompra/BorrarCompra', { cc: cc, numero: numero })
                                .always($.unblockUI)
                                .then(response => {
                                    dialogConfirmarBorrado.dialog('close');

                                    if (response.success) {
                                        AlertaGeneral(`Alerta`, `Se ha borrado la Orden de Compra ${cc}-${numero}.`);

                                        limpiarInformacion();
                                        limpiarTabla(tblPartidasEditar);
                                        limpiarTabla(tblPartidas);

                                        textAreaDescPartida.val('');
                                    } else {
                                        AlertaGeneral(`Alerta`, `Error al borrar la orden de compra. ${response.message}`);
                                    }
                                }, error => {
                                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                                }
                                );

                            dialogConfirmarBorrado.dialog('close');
                        },
                        'Cancelar': function () {
                            dialogConfirmarBorrado.dialog('close');
                        }
                    }
                });
            } else {
                AlertaGeneral(`Alerta`, `El Periodo Contable no está activo.`);
            }
        }

        function cargarCatalogoRetenciones() {
            $.post('/Enkontrol/OrdenCompra/GetCatalogoRetenciones').then(response => {
                if (response.success) {
                    AddRows(tblCatalogoRetenciones, response.data);
                } else {
                    AlertaGeneral(`Alerta`, `Error al cargar la información de las retenciones.`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function llenarInputProveedorDistinto(numeroProveedor) {
            let listInputs = tblPartidas.find('tbody tr .inputProveedorDistinto');

            $(listInputs).each(function (id, element) {
                $(element).val(numeroProveedor);
            });
        }

        function asignarProveedorDistinto(numeroProveedor) {
            if (!isNaN(numeroProveedor) && numeroProveedor != '' && numeroProveedor > 0) {
                let numeroExiste = 0;

                $.post('/Enkontrol/OrdenCompra/GetProveedorInfo', { num: numeroProveedor }).then(response => {
                    if (response != null) {
                        if (response.cancelado != 'C') {
                            numeroExiste = numeroProveedor;
                        } else {
                            AlertaGeneral(`Alerta`, `El proveedor "${response.label} - ${response.id}" no está activo.`);
                        }
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información del proveedor.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );

                return numeroExiste;
            } else {
                return 0;
            }
        }

        function cargarPresupuesto() {
            let cc = selectCC.val();

            $.post('/Enkontrol/OrdenCompra/GetPresupuestoCC', { cc }).then(response => {
                if (response.success) {
                    if (response.data != null) {
                        fieldsetPresupuesto.css('display', 'block');
                        _validaGlobal = true;
                        labelPresupuestoGlobal.text(`Presupuesto Global (${cc}): ${maskNumero6DComprasColombia(response.data.presupuestoGlobal)}`);
                        labelPresupuestoGlobal.attr('presupuesto', response.data.presupuestoGlobal);
                        labelPresupuestoActual.text(`Presupuesto Actual (${cc}): ${maskNumero6DComprasColombia(response.data.presupuestoActual)}`);
                        labelPresupuestoActual.attr('presupuesto', response.data.presupuestoActual);
                    } else {
                        fieldsetPresupuesto.css('display', 'none');
                        _validaGlobal = false;
                    }
                } else {
                    AlertaGeneral(`Alerta`, `Error al consultar la información del presupuesto.`);
                    fieldsetPresupuesto.css('display', 'none');
                    _validaGlobal = false;
                    labelPresupuestoGlobal.text('');
                    labelPresupuestoGlobal.attr('presupuesto', '');
                    labelPresupuestoActual.text('');
                    labelPresupuestoActual.attr('presupuesto', '');
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function checkPeriodoContable() {
            $.post('/Enkontrol/OrdenCompra/GetPeriodoContable').then(response => {
                if (response.success) {
                    if (response.data != null) {
                        _flagPeriodoContable = true;
                        btnGuardarNuevaCompra.attr('disabled', false);
                        btnBorrarCompra.css('display', 'inline-block');
                    } else {
                        _flagPeriodoContable = false;
                        btnGuardarNuevaCompra.attr('disabled', true);
                        btnBorrarCompra.css('display', 'none');
                    }
                } else {
                    AlertaGeneral(`Alerta`, `Error al consultar la información del periodo contable.`);
                    _flagPeriodoContable = false;
                    btnGuardarNuevaCompra.attr('disabled', true);
                    btnBorrarCompra.css('display', 'none');
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                _flagPeriodoContable = false;
                btnGuardarNuevaCompra.attr('disabled', true);
                btnBorrarCompra.css('display', 'none');
            }
            );
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

        function verificarComprasPeru() {
            if (_empresaActual == 6) {
                selectMoneda.attr('disabled', false);
                $('.elementoPeru').show();
                $(".spanIVA").html("I.G.V.");
            } else {
                $('.elementoPeru').hide();
                $(".spanIVA").html("I.V.A.");
            }
        }

        init();
    }
    $(document).ready(() => {
        Enkontrol.OrdenCompra.Generar = new Generar();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); })
        ;
})();