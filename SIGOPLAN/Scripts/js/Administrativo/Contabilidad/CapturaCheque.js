(() => {
    $.namespace('Administracion.Cheque.CapturaCheque');

    CapturaCheque = function () {

        //Variables Globales.
        let listaOCCbo;
        let listacbTipoMov;
        let inputSelectorActivo;
        let dtTablaGeneralPolizas;
        let dtTablaCheques;
        let detTablaGeneral;
        let tipoBusqueda;
        let dtTablaPolizas;
        const usuarioCaptura = { usuarioID: 0, usuarioNombre: '' };
        let listaBancos;
        let periodoYear;
        let periodoMes;
        let listaCta;
        let listaProveedores;
        let PermisoDelete;
        let ocTempChange;
        let maxValue = 0;

        //#region  Variable de Pöliza
        let dtTablaPolizaProveedores;
        let dtTablaPolizaChequeCap
        let dtTablaPolizaDiario;

        const checkChequeElectronico = $("#checkChequeElectronico");
        const modalVerpolizas = $("#modalVerpolizas");
        const tablaPolizaProveedores = $("#tablaPolizaProveedores");
        const tablaPolizaChequeCap = $("#tablaPolizaChequeCap");
        const tablaPolizaDiario = $("#tablaPolizaDiario");
        const btnGuardarPolizaDiario = $("#btnGuardarPolizaDiario");
        const btnLineaPolizaDiario = $("#btnLineaPolizaDiario");

        //#endregion

        const report = $("#report");
        const getTipoMov = new URL(window.location.origin + '/administrativo/cheque/GetTipoMovimiento');
        const getOrdenCompra = new URL(window.location.origin + '/administrativo/cheque/GetOrdenCompraAnticipo');
        const getCC = new URL(window.location.origin + '/administrativo/cheque/CboEconomico');

        //Pantalla Principal.
        const btnBuscar = $('#btnBuscar');
        const btnNuevoCheque = $('#btnNuevoCheque');
        const inputCuentaPpal = $('#inputCuentaPpal');
        const inputCuentaDescripcionPpal = $('#inputCuentaDescripcionPpal');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const tablaCheques = $('#tablaCheques');
        const lblGeneral = $('#lblGeneral');
        const btnPrintCheque = $("#btnPrintCheque");

        //Modal Cheques 
        const modalBuscarOC = $('#modalBuscarOC')
        const modalCapturaCheques = $('#modalCapturaCheques');
        const inputCuentaBancoModal = $('#inputCuentaBancoModal');
        const inputDesrcipcionBancoModal = $('#inputDesrcipcionBancoModal');
        const butonCuentaBanco = $('#butonCuentaBanco');
        const inputCheque = $('#inputCheque');
        const inputFechaCheque = $('#inputFechaCheque');
        const checkOrdenCompra = $('#checkOrdenCompra');
        const comboOrdenCompra = $('#comboOrdenCompra');
        const inputDescripcionCheque = $('#inputDescripcionCheque');
        const butonBuscarProv = $('#butonBuscarProv');
        const inputMonto = $('#inputMonto');
        const inputCantidad = $('#inputCantidad');
        const txtCuentaDescripcionModal = $('#txtCuentaDescripcionModal');
        const txtBancoCuenta = $('#txtBancoCuenta');
        const inputConceptoCheque = $('#inputConceptoCheque');
        const inputUsuarioRealiza = $('#inputUsuarioRealiza');
        const comboCC = $('#comboCC');
        const comboTipoMovimiento = $('#comboTipoMovimiento');
        const inputSubTipoMov = $('#inputSubTipoMov');
        const inputPoliza = $('#inputPoliza');
        const inputTipoPolizaCheque = $('#inputTipoPolizaCheque');
        const btnGuardarCheque = $('#btnGuardarCheque');
        const ckChequeElectronico = $('#ckChequeElectronico');

        const tablaGeneral = $('#tablaGeneral');
        const modalGeneral = $('#modalGeneral');

        // Selectores.
        const butonNewRenglon = $('#butonNewRenglon');
        const modalPoliza = $('#modalPoliza');


        //#region Modal Polizas
        const tablaGeneralPolizas = $('#tablaGeneralPolizas');
        const modalGeneralPolizas = $('#modalGeneralPolizas');
        const lblGeneralPolizas = $('#lblGeneralPolizas');
        const inputTipoPoliza = $('#inputTipoPoliza');
        const inputEstatusPoliza = $('#inputEstatusPoliza');
        const inputCargosPoliza = $('#inputCargosPoliza');
        const inputPolizaPoliza = $('#inputPolizaPoliza');
        const inputGeneradaPoliza = $('#inputGeneradaPoliza');
        const inputAbonosPoliza = $('#inputAbonosPoliza');
        const inputDiferenciaPoliza = $('#inputDiferenciaPoliza');
        const inputFechaPoliza = $('#inputFechaPoliza');
        const inputCCPoliza = $('#inputCCPoliza');
        const inputCtaPoliza = $('#inputCtaPoliza');
        const inputInterfasePoliza = $('#inputInterfasePoliza');
        const inputInterfaseDescPoliza = $('#inputInterfaseDescPoliza');
        const inputTMPoliza = $('#inputTMPoliza');
        const inputConceptoPoliza = $('#inputConceptoPoliza');
        const tablaPolizas = $('#tablaPolizas');
        const btnValidaPoliza = $('#btnValidaPoliza');
        const btnGuardarInfo = $('#btnGuardarInfo');
        const divModalCheque = $("#divModalCheque");
        const inputCuenta = $('#inputCuenta');

        //#endregion

        //#region  dtBuscarOC
        let dtBuscarOC;
        const tablaBuscarOC = $('#tablaBuscarOC');
        const cboCCModalOC = $('#cboCCModalOC');
        const inputNumeroOCModalOC = $('#inputNumeroOCModalOC');
        const btnBuscarOC = $('#btnBuscarOC');
        const btnBuscarOCCC = $('#btnBuscarOCCC');
        //#endregion

        (function init() {
            // Lógica de inicialización.
            comboOrdenCompra.prop('disabled', true);
            initCombos();
            ///fnGetPeriodoContable();
            getUsuarioCaptura();
            fnLoadCuenta();
            initTablaCheques();
            initTablaGeneral();
            initTablaPoliza();
            initTablaGeneralPoliza();
            fnEventListener();
            initSelectores();
            initTablaOC();
            initTablasPolizas();
        })();

        // Métodos.
        function fnEventListener() {
            btnGuardarInfo.prop('disabled', true);
            // btnGuardarPoliza.click(fnSaveOrUpdatePoliza);
            butonNewRenglon.click(nuevaFila);
            btnGuardarCheque.click(fnObtenerDatosPoliza);
            btnGuardarInfo.click(saveOrUpdateCheque);
            butonCuentaBanco.click(fnBuscarCuentasBanco);
            butonBuscarProv.click(fnBuscarProveedores);
            btnNuevoCheque.click(nuevoCheque);
            checkOrdenCompra.change(fnSetOrdenCompra);
            inputMonto.change(fnSetCantidadLetra);
            inputCheque.change(fnCambiarPoliza);
            btnBuscar.click(cargarCheques);
            btnValidaPoliza.click(fnValidaPoliza);
            btnPrintCheque.click(verReporte);
            inputCuenta.change(buscarCuentas);
            btnBuscarOC.click(BuscarOC);
            btnBuscarOCCC.click(BuscarOC);
            inputMonto.change(validarMonto);
            btnLineaPolizaDiario.click(fnNuevaFilaPolizaDiario);
        }


        function validarMonto() {
            if (maxValue != 0) {

                if ($(this).val() > maxValue) {
                    $(this).val(maxValue);
                    $(this).trigger('change');
                    return AlertaGeneral('Alerta', 'El monto sobrepasa el monto de la orden de compra.');
                }
            }

        }

        //#region Modal Buscar OC

        function initTablaOC() {
            dtBuscarOC = tablaBuscarOC.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    { data: 'cc', title: 'C.C.' },
                    { data: 'numero', title: 'Número' },
                    { data: 'proveedor', title: 'Proveedor' },
                    { data: 'id', render: (data, type, row) => `<button class="btn btn-success seleccionar" data-id="${data}"><i class="far fa-check-square"></i></button>` },
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ]
            });
        }


        tablaBuscarOC.on('click', '.seleccionar', function () {

            let dataOC = dtBuscarOC.row($(this).parents('tr')).data();//dtBuscarOC.data().toArray().filter(oc => oc.id == $(this).attr('data-id'))[0];

            $.get('/administrativo/cheque/GetOCSeleccionado', { cc: dataOC.cc, numero: dataOC.numero })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        let ocData = response.items[0];
                        maxValue = ocData.totalAnticipo;
                        let ocTemp = {
                            id: dataOC.id,
                            text: (dataOC.cc + "-" + dataOC.numero),
                            totalAnticipo: ocData.totalAnticipo,
                            cc: dataOC.cc,
                            numero: dataOC.numero
                        };

                        listaOCCbo.push(ocTemp);
                        inputMonto.val(ocData.totalAnticipo).trigger('change');
                        inputDescripcionCheque.val(dataOC.proveedor);
                        ocTempChange = ocTemp;
                        comboOrdenCompra.select2({
                            data: listaOCCbo
                        });

                        comboCC.val(ocTemp.cc);
                        comboCC.trigger('change');
                        checkOrdenCompra.prop('checked', true);
                        modalBuscarOC.modal('hide');

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

        function BuscarOC() {

            $.get('/administrativo/cheque/BuscarOC', { cc: cboCCModalOC.val() == "" ? "" : cboCCModalOC.val(), numero: inputNumeroOCModalOC.val() == "" ? 0 : inputNumeroOCModalOC.val() })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        dtBuscarOC.clear().draw();
                        dtBuscarOC.rows.add(response.items).draw();
                        modalBuscarOC.modal('show');
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        //#endregion

        function buscarCuentas() {
            $.get('/administrativo/cheque/BuscarCuenta', { cuenta: inputCuenta.val(), subCuenta: inputSubCuenta.val() == '' ? 0 : inputSubCuenta.val(), ssubCuenta: inputSScuenta.val() == '' ? 0 : inputSScuenta.val() })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function fnValidaPoliza() {
            divModalCheque.block({
                message: null
            });
            let getData = fnGetInfoTblPoliza();
            if (getData.isValid) {
                $.post('/administrativo/cheque/ValidaPoliza', { listaMovpol: getData.datosCaptura })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            if (response.aplica) {
                                btnGuardarInfo.prop('disabled', false);
                                AlertaGeneral('Alerta', 'Poliza Correcta puede continuar.');
                                divModalCheque.unblock();
                            }
                            else {
                                AlertaGeneral('Alerta', 'La poliza tiene datos invalidos.');
                                divModalCheque.unblock();
                                btnGuardarInfo.prop('disabled', true);
                            }
                        } else {
                            // Operación no completada.
                            divModalCheque.unblock();
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => {
                        divModalCheque.unblock();
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
            else {
                return AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${getData.message}`);
            }
        }

        function fnGetInfoTblPoliza() {
            let data = {};
            data.isValid = true;
            let fechaTmp = inputFechaCheque.val().split('/');
            periodoMes = +fechaTmp[1];
            periodoYear = fechaTmp[2];

            if (dtTablaPolizas.data().toArray().length > 0) {

                const datosCaptura = tablaPolizas.find('tbody tr[role="row"]').toArray()
                    .map(row => {
                        let $row = $(row);
                        let objCC = $row.find('.inputCC').val();

                        if (objCC === null && objCC === '') {
                            data.isValid = false;
                            data.message = 'Se debe tener el centro de costos para poder continuar';
                            return undefined;
                        }
                        if (!data.isValid)
                            return undefined;
                        let monto = 0;
                        if (+$row.children().find('.inputTMs').val() == 1)
                            monto = Math.abs(+$row.find('.inputMonto').val());
                        if (+$row.children().find('.inputTMs').val() == 2)
                            monto = -Math.abs(+$row.find('.inputMonto').val());
                        // do something
                        const captura = {
                            id: 0,
                            linea: +$row.find('.inputLinea').val(),
                            cta: +$row.find('.inputCta').val(),
                            scta: +$row.find('.inputScta').val(),
                            sscta: +$row.find('.inputSScta').val(),
                            digito: +$row.find('.inputDigito').val(),
                            itm: +$row.find('.CombotmPolizas').val(),
                            istm: '',
                            numpro: +$row.find('.inputNumpro').val(),
                            referencia: +$row.find('.inputReferencia').val(),
                            cc: $row.find('.inputCC').val(),
                            orden_compra: $row.find('.inputOrdenCompra').val(),
                            concepto: $row.find('.inputConcepto').val(),
                            tm: +$row.children().find('.inputTMs').val(),
                            monto: monto,
                            mes: periodoMes,
                            year: periodoYear,
                            area: +$row.find('inputArea').val(),
                            cuenta_oc: +$row.find('inputCuenta').val()
                        }
                        return captura;

                    }).filter(x => x != undefined);

                data.datosCaptura = datosCaptura;
                return data;
            }
            else {
                //AlertaGeneral('Alerta', 'La tabla no contiene información');
                data.message = 'La tabla no contiene información';
                data.isValid = false;
                return data;
            }
        }

        function initCombos() {
            fnGetTipoMovimientoCombo();
            fnGetComboOrdenCompra();
            comboCC.fillCombo(getCC, null, false);
            comboCC.select2();
            cboCCModalOC.fillCombo(getCC, null, false);
            //cboCCModalOC.select2();
            comboOrdenCompra.on('change', () => {
                if (comboOrdenCompra.select2('data').length > 0) {
                    inputMonto.val(comboOrdenCompra.select2('data')[0].totalAnticipo);
                    inputMonto.trigger('change');
                    comboCC.val(comboOrdenCompra.select2('data')[0].cc);
                    comboCC.trigger('change');
                    inputDescripcionCheque.val(comboOrdenCompra.select2('data')[0].proveedor);

                    /*if (comboOrdenCompra.select2('data')[0].id != 0) {
                        btnGuardarCheque.prop('disabled', false);
                        // inputMonto.prop('disabled', true);
                    }
                    else
                        btnGuardarCheque.prop('disabled', true);*/
                }
            });
        }

        function initSelectores() {

            inputFechaInicio.datepicker({
                'dateFormat': 'dd/mm/yy',
                'maxDate': new Date()
            }).datepicker('option', 'showAnim', 'slide')
                .datepicker('setDate', new Date())

            inputFechaFin.datepicker({
                'dateFormat': 'dd/mm/yy',
                'maxDate': new Date()
            }).datepicker('option', 'showAnim', 'slide')
                .datepicker('setDate', new Date());

            inputDescripcionCheque.data().id = 0;
            inputCuentaPpal.getAutocompleteValid(setClaveCuenta, validarCuenta, { porDesc: true }, '/administrativo/cheque/GetListaCuentas/');
            inputCuentaDescripcionPpal.getAutocompleteValid(setClaveCuentaDesc, validarCuenta, { porDesc: true }, '/administrativo/cheque/GetListaCuentas/');
            inputCuentaBancoModal.getAutocompleteValid(setClaveCuenta, validarCuenta, { porDesc: false }, '/administrativo/cheque/GetListaCuentas/');
            inputDesrcipcionBancoModal.getAutocompleteValid(setClaveCuenta, validarCuenta, { porDesc: true }, '/administrativo/cheque/GetListaCuentas/');

        }

        function fnSetOrdenCompra() {
            if (checkOrdenCompra.prop('checked')) {
                comboOrdenCompra.prop('disabled', false);
            }
            else {
                comboOrdenCompra.val(' ');
                comboOrdenCompra.prop('disabled', true);
            }
        }
        function setClaveCuentaDesc(e, ui) {
            $(this).val(ui.item.value)
            //inputCuentaPpal.val(ui.item.id);
            $(this).parents('.inputCuentasG').find('.inputCuenta').val(ui.item.id);
        }

        function fnGetComboOrdenCompra() {
            $.post('/administrativo/cheque/GetOrdenCompraAnticipo')
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        listaOCCbo = response.items;
                        comboOrdenCompra.select2({
                            data: response.items
                        });
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function fnGetTipoMovimientoCombo() {
            $.post(getTipoMov)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        listacbTipoMov = response.items;
                        comboTipoMovimiento.select2({
                            data: response.items
                        });
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function fnGetPeriodoContable() {
            $.ajax('/administrativo/cheque/GetPeriodoContable')
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        periodoYear = response.year;
                        periodoMes = response.mes;
                        inputFechaCheque.datepicker({
                            'dateFormat': 'dd/mm/yy',
                            'maxDate': new Date()
                        }).datepicker('option', 'showAnim', 'slide')
                            .datepicker('setDate', new Date());

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function fnGetInfoPoliza() {
            return dtTablaPolizas.data().toArray();
        }

        function fnSaveOrUpdatePoliza() {
            var data = fnGetInfoPoliza();
            $.post('/administrativo/cheque/SaveOrUpdatePoliza', { data })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function saveOrUpdateCheque() {
            divModalCheque.block({
                message: null
            });
            let valida = ValidarCheques();
            btnGuardarCheque.prop('disabled', true);
            let ocOD = comboOrdenCompra.val() == null ? 0 : comboOrdenCompra.val();
            if (valida) {
                let objSave = getCheque();
                let getData = fnGetInfoTblPoliza();
                if (!getData.isValid) {
                    return AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${getData.message}`);
                }
                $.post('/administrativo/cheque/SaveOrUpdateCheque', { objSave: objSave, ocID: ocOD, objMovPol: getData.datosCaptura, tipoCheque: ckChequeElectronico.prop('checked') ? 2 : 1 })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            divModalCheque.unblock();
                            // limpiarCheque();
                            if (dtTablaPolizas != null) {
                                btnGuardarCheque.prop('disabled', false);
                                btnPrintCheque.prop('disabled', false);
                            }
                            AlertaGeneral(`Operacion Exitosa,se ha guardado el chequecorrectamente`);
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                            btnGuardarCheque.prop('disabled', false);
                            divModalCheque.unblock();
                        }
                    }, error => {
                        divModalCheque.unblock();
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        btnGuardarCheque.prop('disabled', false);
                    }
                    );
            }

        }

        function verReporte() {
            let fechaCheque = inputFechaCheque.val().split('/');
            let pMes = fechaCheque[1];
            let pYear = fechaCheque[2];

            var path = `/Reportes/Vista.aspx?idReporte=187&&pNumCheque=${inputCheque.val()}&&pMes=${pMes}&&pYear=${pYear}&&pTipo=${1}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function fnObtenerDatosPoliza() {

            divModalCheque.block({
                message: null
            });

            let valida = ValidarCheques();
            btnGuardarCheque.prop('disabled', true);
            let ocOD = comboOrdenCompra.select2('data')[0] != undefined ? comboOrdenCompra.select2('data')[0].numero : 0;//comboOrdenCompra.val() == null ? 0 : comboOrdenCompra.val();
            if (valida) {
                let objSave = getCheque();
                $.post('/administrativo/cheque/ObtenerDatosPoliza', { objSave: objSave, ocID: ocOD })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            divModalCheque.unblock();
                            if (dtTablaPolizas != null) {
                                let objBancos = response.bancosCuentas;
                                let objCheques = response.cheque;
                                let objPolizas = response.polizas[0];

                                dtTablaPolizas.clear().draw();
                                dtTablaPolizas.rows.add(response.listaPolizas).draw();
                                inputTipoPoliza.val(objBancos.descripcion);
                                inputEstatusPoliza.val(getStatusStatus(objPolizas.status));
                                inputPolizaPoliza.val(objCheques.numero);
                                inputGeneradaPoliza.val(getStatusGeneradas(objPolizas.generada));
                                inputCCPoliza.val(objCheques.cc);
                                inputFechaPoliza.val(response.fechaCheque);
                                inputCtaPoliza.val(objBancos.cta);
                                inputConceptoPoliza.val('Poliza ' + objPolizas.concepto);
                                inputTMPoliza.val(response.tm);
                                inputCargosPoliza.val(objPolizas.cargos);
                                inputAbonosPoliza.val(objPolizas.abonos);
                                inputDiferenciaPoliza.val(objPolizas.cargos - objPolizas.abonos);
                                btnGuardarCheque.prop('disabled', false);
                                butonNewRenglon.prop('disabled', false);
                            }
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                            btnGuardarCheque.prop('disabled', false);
                            divModalCheque.unblock();
                        }
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        btnGuardarCheque.prop('disabled', false);
                        divModalCheque.unblock();
                    }
                    );
            }
            else {
                btnGuardarCheque.prop('disabled', false);
                divModalCheque.unblock();
            }
        }

        function ValidarCheques() {
            let bandera = true;
            if (inputCuentaBancoModal.val() == ' ') {
                btnGuardarCheque.prop('disabled', false);
                return AlertaGeneral('Alerta', 'Es necesaria la cuenta para poder continuar');
            }
            if (inputCheque.val() == ' ') {
                btnGuardarCheque.prop('disabled', false);
                return AlertaGeneral('Alerta', 'Es necesario el numero de cheque para continuar con el proceso');
            }
            if (inputDescripcionCheque.val() == ' ') {
                btnGuardarCheque.prop('disabled', false);
                return AlertaGeneral('Alerta', 'No se capturo el beneficiario');
            }
            if (inputMonto.val() == ' ') {
                btnGuardarCheque.prop('disabled', false);
                return AlertaGeneral('Alerta', 'El monto debe ser mayor que cero, favor de verificar');
            }
            if (inputConceptoCheque.val() == ' ') {
                btnGuardarCheque.prop('disabled', false);
                return AlertaGeneral('Alerta', 'Falta agregar un concepto para el cheque.');
            }
            if (comboCC.val() == ' ') {
                btnGuardarCheque.prop('disabled', false);
                return AlertaGeneral('Alerta', 'El valor del centro de costos no es valido.');
            }
            if (comboTipoMovimiento.val() == ' ') {
                btnGuardarCheque.prop('disabled', false);
                return AlertaGeneral('Alerta', 'El valor del tipo de movimiento no es valido.');
            }
            if (inputPoliza.val() == ' ') {
                btnGuardarCheque.prop('disabled', false);
                return AlertaGeneral('Alerta', 'No se encontro el valor de la poliza, favor de verificar.');
            }
            return bandera;
        }

        function getStatusGeneradas(tipo) {
            switch (tipo) {
                case 'B':
                    return 'Banco';
                default:
                    return '';
            }
        }

        function getStatusStatus(tipo) {
            switch (tipo) {
                case 'C':
                    return 'Capturada'

                default:
                    return 'error'
            }
        }

        function getCheque() {
            var fechaSplit = inputFechaCheque.val().split('/');
            return {
                cuenta: inputCuentaBancoModal.val(),
                fecha_mov: inputFechaCheque.val(),
                numero: inputCheque.val(),
                tm: comboTipoMovimiento.val(),
                tipocheque: null,
                descripcion: inputDescripcionCheque.val(),
                cc: comboCC.val(),
                monto: inputMonto.val(),
                hecha_por: inputUsuarioRealiza.val(),
                usuariocapturaID: usuarioCaptura.usuarioID,
                status_bco: '',
                status_lp: 'N',
                num_pro_emp: inputDescripcionCheque.data().id,
                cpto1: inputConceptoCheque.val(),
                cpto2: null,
                cpto3: null,
                iyear: fechaSplit[2],
                imes: fechaSplit[1],
                ipoliza: inputPoliza.val(),
                itp: inputTipoPolizaCheque.val(),
                ilinea: 1,
                tp: null,
                fecha_reten: null,
                desc1: null,
                status_transf_cash: 'N',
                id_empleado_firma: null,
                id_empleado_firma2: null,
                fecha_reten_fin: null,
                firma1: null,
                fecha_firma1: null,
                firma2: null,
                fecha_firma2: null,
                firma3: null,
                fecha_firma3: null,
                clave_sub_tm: null,
                ruta_comprobantebco_pdf: null
            }
        }

        function fnBuscarProveedores() {
            lblGeneral.text('Lista Proveedores');
            tipoBusqueda = 1;
            modalGeneral.modal('show');
            detTablaGeneral.clear().draw();
            cargarTablaGeneral('/administrativo/cheque/GetProveedores');
        }

        function fnBuscarCuentasBanco() {
            lblGeneral.text('Lista Bancos');
            tipoBusqueda = 2;
            modalGeneral.modal('show');
            detTablaGeneral.clear().draw();
            cargarTablaGeneral('/administrativo/cheque/GetCuentasBanco');
        }

        async function getUsuarioCaptura() {
            $.get('/administrativo/cheque/GetUsuarioCaptura')
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        usuarioCaptura.usuarioID = response.usuarioID;
                        usuarioCaptura.usuarioNombre = response.usuarioNombre;
                        inputUsuarioRealiza.val(response.usuarioNombre).prop('disabled', true);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function nuevoCheque() {
            limpiarCheque();
            modalCapturaCheques.modal('show');
        }
        /*Obtiene todos los datos referentes para capturar un cheque en base a la cuenta del cheque */

        function getInfoCuenta() {
            divModalCheque.block({
                message: null
            });
            $.get('/administrativo/cheque/GetInfoCuenta', { cuenta: inputCuentaBancoModal.val() })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        divModalCheque.unblock();
                        if (ckChequeElectronico.prop('checked'))
                            inputCheque.val(response.infocuenta.ult_cheq_electronico + 1);
                        else
                            inputCheque.val(response.infocuenta.ultimo_cheque + 1);

                        inputPoliza.val(inputCheque.val());
                        inputTipoPolizaCheque.val(response.infocuenta.tp);
                        txtCuentaDescripcionModal.text(response.infocuenta.cuenta + ' ' + response.infocuenta.descripcion);
                        txtBancoCuenta.text(response.infocuenta.bancoDescripcion + ' ' + response.infocuenta.sucursal);

                    } else {
                        divModalCheque.unblock();
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    divModalCheque.unblock();
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function fnCambiarPoliza() {
            inputPoliza.val(inputCheque.val());
        }

        function fnSetCantidadLetra() {
            let num = $(this).val();
            let cantidad = `(${NumeroALetras(num)})`;
            inputCantidad.val(cantidad);
        }

        function setClaveCuenta(e, ui) {
            $(this).val(ui.item.value);
            $(this).parents('.inputCuentasG').find('.inputDescripcionCuenta').val(ui.item.id);
            if ($(this).hasClass('iCuentaBanco')) {
                getInfoCuenta();
            }

        }

        function validarCuenta(e, ui) {
            if (ui.item == null) {
                inputCuentaPpal.val('');
                inputCuentaDescripcionPpal.val('');
            }
        }
        //#region  tabla Poliza
        function initTablaPoliza() {
            dtTablaPolizas = tablaPolizas.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                "autoWidth": false,
                scrollCollapse: true,
                scrollX: true,
                columns: [
                    {
                        data: 'linea', title: 'No', "width": "15px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let vDisabled = '';
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = `value = '${row + 1}' ${vDisabled}`;
                            html = `<input type='text' class='form-control inputLinea' ${valueData}  disabled>`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'cta', title: 'Cuenta', "width": "150px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            html = '';
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html += `<div style="display: inline-flex;">`
                            html += `<input type='text' class='form-control inputCta' ${valueData} data-row='${row}' >`;
                            html += `<button class="btn btn-info inputctaBuscar" data-tipo='3'>
                                        <i class="fas fa-search"></i>
                                    </button> </div>`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'scta', title: 'SCta', "width": "150px",
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html = '';
                            html += `<div style="display: inline-flex;">`
                            html += `<input type='text' class='form-control inputScta' ${valueData}  data-row='${row}'>`;
                            html += `<button class="btn btn-info inputsctaBuscar" data-tipo='4'>
                                        <i class="fas fa-search"></i>
                                    </button> </div>`;
                            $(td).append(html);

                        }
                    },
                    {
                        data: 'sscta', title: 'SSCta', "width": "150px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html = '';
                            html += `<div style="display: inline-flex;">`
                            html += `<input type='text' class='form-control inputSScta' ${valueData}  data-row='${row}'>`;
                            html += `<button class="btn btn-info inputssctaBuscar" data-tipo='5'>
                                        <i class="fas fa-search"></i>
                                    </button> </div>`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'digito', title: 'D', "width": "150px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}' ` : "";
                            html = `<input type='text' class='form-control inputDigito' ${valueData} >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'itm', title: 'Mov.', "width": "150px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            $(td).append(renderComboBox(listacbTipoMov, row, "CombotmPolizas"));
                            $(td).children().val(cellData);
                        }
                    },
                    {
                        data: 'istm', title: 'Sub Mov',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html = `<input type='text' class='form-control' ${valueData}  disabled >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'numpro', title: 'Proveedor',
                        createdCell: function (td, cellData, rowData, row, col) {
                            html = "";
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            html += `<div style="display: inline-flex;">`
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html += `<input type='text' class='form-control inputNumpro' ${valueData}  >`;
                            html += `<button class="btn btn-info btnBuscarProveedor">
                            <i class="fas fa-search"></i>
                            </button> </div>`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'referencia', title: 'Referencia',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html = `<input type='text' class='form-control inputReferencia' ${valueData}  >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'cc', title: 'C.C.',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let cc = cellData;
                            if (cellData == 0)
                                cc = '*'
                            valueData = cellData != null ? `value = '${cc}'` : "";
                            html = `<input type='text' class='form-control inputCC' ${valueData}  >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'orden_compra', title: 'O.C/Cte',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}'` : "";
                            html = `<input type='text' class='form-control inputOrdenCompra' ${valueData}  >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'concepto', title: 'Concepto',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html = `<input type='text' class='form-control inputConcepto' ${valueData}  >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'tm', title: 'Tipo Movimiento', "width": "150px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (row == 0) {
                                vDisabled = 'disabled'
                            }

                            html = `<select class='form-control inputTMs ${vDisabled}'>
                                            <option value="1">1-Cargo</option>
                                            <option value="2">2-Abono</option>
                                            <option value="3">3-Cargo Rojo</option>
                                            <option value="4">4-Abono Rojo</option>
                                        </select>`;
                            $(td).append(html);
                            $(td).children().val(cellData);

                        }
                    },
                    {
                        data: 'area', title: 'area',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            html = `<input type='text' class='form-control inputArea' value=${cellData}  >`;
                            $(td).append(html);

                        }
                    }, {
                        data: 'cuenta_oc', title: 'cuenta',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            html = `<input type='text' class='form-control inputCuenta' value=${cellData}  >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'monto', title: 'Monto',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let vDisabled = '';
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            if (cellData == null)
                                cellData = 0;
                            valueData = `value = '${cellData}' ${vDisabled}`;
                            html = `<input type='text' class='form-control inputMonto' ${valueData}  >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'linea',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let btn = '';
                            btn = '<button class="btn btn-info delete"><i class="fas fa-times"></i></button >';
                            $(td).append(btn);
                        }
                    }
                ],
                drawCallback: function (settings) {
                    $('.CombotmPolizas').select2();
                }
            });
        }

        tablaPolizas.on('change', '.inputArea', function () {
            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            data.area = $(this).val();
        });

        tablaPolizas.on('click', '.btnBuscarProveedor', function () {
            inputSelectorActivo = $(this).parents('tr').find('.inputNumpro');
            lblGeneral.text('Lista Proveedores');
            tipoBusqueda = 3;
            modalGeneral.modal('show');
            detTablaGeneral.clear().draw();
            cargarTablaGeneral('/administrativo/cheque/GetProveedores');
        });

        tablaPolizas.on('change', '.inputITM', function () {
            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            data.itm = $(this).val();
        });

        tablaPolizas.on('change', '.inputNumpro', function () {
            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            data.numpro = $(this).val();
        });
        tablaPolizas.on('change', '.inputReferencia', function () {

            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            data.referencia = $(this).val();
        });

        tablaPolizas.on('change', '.inputCC', function () { //tablaPolizas.on('.inputCC').change(function () {
            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            data.cc = $(this).val();
        });

        tablaPolizas.on('change', '.inputOrdenCompra', function () { // tablaPolizas.on('.inputOrdenCompra').change(function () {
            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            data.orden_compra = $(this).val();
        });


        tablaPolizas.on('change', '.concepto', function () {  //    tablaPolizas.on('.concepto').change(function () {
            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            data.concepto = $(this).val();
        });

        tablaPolizas.on('change', '.inputTMs', function () { //    tablaPolizas.on('.inputTMs').change(function () {

            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            let monto = $(this).parents('tr').children().find('.inputMonto').val();
            data.tm = $(this).val();
            data.monto = data.tm == "1" ? Math.abs(monto) : Math.abs(monto) * -1;
            $(this).parents('tr').children().find('.inputMonto').val(data.monto);
            let operacionRes = 0;
            let Cargos = 0;
            let Abonos = 0;
            const datosCaptura = tablaPolizas.find('tbody tr[role="row"]').toArray()
                .map(row => {
                    let $row = $(row);
                    let tm = +$row.find('.inputTMs').val();
                    let monto = +$row.find('.inputMonto').val();
                    if (tm == 1) {
                        operacionRes += monto;
                        Cargos += monto;
                    }
                    else {
                        operacionRes -= monto;
                        Abonos -= monto;
                    }
                    return null;
                }).filter(x => x != undefined);
            inputCargosPoliza.val(Cargos);
            inputAbonosPoliza.val(-Abonos);
            inputDiferenciaPoliza.val(Cargos - Abonos);
        });

        tablaPolizas.on('click', '.delete', function () { //   tablaPolizas.on('.delete').click(function () {

            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            let count = 2;
            let listaDTTablaPolizas = dtTablaPolizas.data().toArray().map((value, index) => {

                if (data.linea != value.linea) {
                    if (index == 0) {
                        return value;
                    }
                    else {
                        value.linea = count;
                        count++;
                        return value;
                    }
                }
            }).filter(x => x != undefined);
            dtTablaPolizas.clear();
            dtTablaPolizas.rows.add(listaDTTablaPolizas).draw();
        });

        tablaPolizas.on('change', '.inputMonto', function () {  //.on('.inputMonto').change(function () {
            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            let tm = data.tm;
            if (tm == 1)
                data.monto = $(this).val();
            else
                data.monto = -$(this).val();

            let operacionRes = 0;
            let Cargos = 0;
            let Abonos = 0;
            const datosCaptura = tablaPolizas.find('tbody tr[role="row"]').toArray()
                .map(row => {
                    let $row = $(row);
                    let monto = +$row.find('.inputMonto').val();
                    if (tm == 1) {
                        operacionRes += monto;
                        Cargos += monto;
                    }
                    else {
                        operacionRes -= monto;
                        Abonos -= monto;
                    }
                    return null;
                }).filter(x => x != undefined);

            inputCargosPoliza.val(Cargos);
            inputAbonosPoliza.val(-Abonos);
            inputDiferenciaPoliza.val(Cargos - Abonos);
        });

        tablaPolizas.on('change', '.tipoTM', function () { //.on('.tipoTM').change(function () {

            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            data.tm = $(this).val();
            let operacionRes = 0;
            let Cargos = 0;
            let Abonos = 0;
            let tm = data.tm;
            if (tm == 1)
                data.monto = $(this).val();
            else
                data.monto = -$(this).val();

            dtTablaGeneralPolizas.data().toArray().forEach(element => {
                if (element.tm == 1) {
                    operacionRes += element.monto;
                    Cargos = operacionRes;
                }
                else {
                    operacionRes -= element.monto;
                    Abonos = operacionRes;
                }
            });
            inputCargosPoliza.val(Cargos);
            inputAbonosPoliza.val(Abonos);
            inputDiferenciaPoliza.val(operacionRes);

        });

        tablaPolizas.on('click', '.inputctaBuscar', function () { //.on('.inputctaBuscar').click(function () {

            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            data.tm = $(this).val();

            inputSelectorActivo = $(this).parents('tr').find('.inputCta');
            modalGeneralPolizas.modal('show');
            let obj = $.map(listaCta, function (element, index) {
                element.tipoS = 3;
                return element;
            });
            dtTablaGeneralPolizas.clear().draw();
            dtTablaGeneralPolizas.rows.add(obj).draw();

        });

        tablaPolizas.on('click', '.inputsctaBuscar', function () {  //.on('.inputsctaBuscar').click(function () {
            let vCta = $(this).parents('tr').find('.inputCta').val();

            inputSelectorActivo = $(this).parents('tr').find('.inputScta');
            $(this).parents('tr').find('.inputScta').val(' ');
            $(this).parents('tr').find('.inputSScta').val(' ');
            $(this).parents('tr').find('.inputDigito').val(' ');
            modalGeneralPolizas.modal('show');

            let obj = $.map(listaCta, function (element, index) {
                element.tipoS = 4;
                if (element.cta == vCta)
                    return element;
            });
            dtTablaGeneralPolizas.clear().draw();
            dtTablaGeneralPolizas.rows.add(obj).draw();
        });

        tablaPolizas.on('click', '.inputssctaBuscar', function () { //.on('.inputssctaBuscar').click(function () {
            let vCta = $(this).parents('tr').find('.inputCta').val();
            let vSCta = $(this).parents('tr').find('.inputScta').val();
            inputSelectorActivo = $(this).parents('tr').find('.inputSScta');
            $(this).parents('tr').find('.inputSScta').val(' ');
            $(this).parents('tr').find('.inputDigito').val(' ');
            modalGeneralPolizas.modal('show');
            let obj = $.map(listaCta, function (element, index) {
                element.tipoS = 5;
                if (element.cta == vCta && element.scta == vSCta)
                    return element;
            });
            dtTablaGeneralPolizas.clear().draw();
            dtTablaGeneralPolizas.rows.add(obj).draw();
        });

        tablaPolizas.on('change', '.CombotmPolizas', function () { //.on('.CombotmPolizas').change(function () {
            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            data.itm = $(this).val();

        });

        tablaPolizas.on('change', '.inputDigito', function () { //.on('.inputDigito').change(function () {
            let data = dtTablaPolizas.row($(this).parents('tr')).data();
            data.digito = $(this).val();
        });

        function limpiarTblInputCtas($inputThis) {
            $inputThis.parents('tr').find('.inputCta').val(' ');
            $inputThis.parents('tr').find('.inputScta').val(' ');
            $inputThis.parents('tr').find('.inputSScta').val(' ');
            $inputThis.parents('tr').find('.inputDigito').val(' ');
        }

        function cargarTablaPoliza(cheque, fecha) {
            $.get('/administrativo/cheque/GetPolizasCheque', { cheque: cheque, fecha: fecha })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        let cheque = response.cheque;
                        let poliza = response.poliza;

                        if (dtTablaPolizas != null) {

                            dtTablaPolizas.clear().draw();
                            dtTablaPolizas.rows.add(response.listaPolizas).draw();
                            modalCapturaCheques.modal('show');
                            var jsDate = moment(cheque.fecha_mov).format('DD/MM/YYYY');
                            inputCheque.val(cheque.numero);
                            inputFechaCheque.val(jsDate);
                            inputDescripcionCheque.val(cheque.descripcion);
                            inputMonto.val(cheque.monto);
                            inputMonto.trigger('change');
                            comboCC.val(cheque.cc);
                            comboCC.trigger('change');
                            comboTipoMovimiento.val()
                            inputPoliza.val(cheque.ipoliza);
                            inputTipoPolizaCheque.val(poliza.tp);
                            btnGuardarCheque.prop('disabled', true);
                        }
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }
        //#endregion

        //#region Tabla Generales
        function cargarTablaGeneral(url) {
            try {
                $('#divmodal').block({
                    messag: null
                });
                $.get(url)
                    .then(response => {
                        if (response.success) {
                            $('#divmodal').unblock();
                            if (detTablaGeneral != null) {
                                detTablaGeneral.clear().draw();
                                detTablaGeneral.rows.add(response.listaObj).draw();
                            }
                        }
                        else
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        $('#divmodal').unblock();
                    },
                        error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            $('#divmodal').unblock();
                        });
            } catch (e) {
                AlertaGeneral(`Operación fallida`, e.message);
                $('#divmodal').unblock();
            }
        }

        function initTablaGeneral() {
            detTablaGeneral = tablaGeneral.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    { data: 'Value', title: 'Número' },
                    { data: 'Text', title: 'Descripción' },
                    { data: 'Value', render: (data, type, row) => `<button class="btn btn-success seleccionar" data-id="${data}"><i class="far fa-check-square"></i></button>` },
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                drawCallback: function (settings) {
                    tablaGeneral.find('.seleccionar').click(function () {
                        let data = detTablaGeneral.row($(this).parents('tr')).data();
                        let ID = $(this).attr('data-id');

                        switch (tipoBusqueda) {
                            case 1:
                                {
                                    inputDescripcionCheque.val(data.Text);
                                    inputDescripcionCheque.data().id = ID;
                                    modalGeneral.modal('hide');
                                }
                                break;
                            case 2:
                                {
                                    modalGeneral.modal('hide');
                                    inputCuentaBancoModal.val(ID);
                                    inputDesrcipcionBancoModal.val(data.Text);
                                    getInfoCuenta();
                                }
                                break;
                            case 3:
                                {
                                    inputSelectorActivo.val(ID);
                                    inputSelectorActivo.trigger('change');
                                    modalGeneral.modal('hide');
                                }
                            default:
                                break;
                        }
                    });
                }
            });
        }

        function initTablaGeneralPoliza() {
            dtTablaGeneralPolizas = tablaGeneralPolizas.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    {
                        data: 'cta', title: 'cta', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text(`${rowData.cta} - ${rowData.scta}-${rowData.sscta}`);
                        }
                    },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'tipoS', render: (data, type, row) => `<button class="btn btn-success seleccionar" data-tipoS="${data}"><i class="far fa-check-square"></i></button>` },
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                drawCallback: function (settings) {
                    tablaGeneralPolizas.find('.seleccionar').click(function () {
                        let data = dtTablaGeneralPolizas.row($(this).parents('tr')).data();
                        let dataPoliza = dtTablaPolizas.row(inputSelectorActivo.parents('tr')).data();
                        let tipoS = $(this).attr('data-tipoS');
                        inputSelectorActivo.parents('tr').find('.inputDigito').val(data.digito);

                        inputSelectorActivo.parents('tr').find('.inputCta').val(data.cta);
                        inputSelectorActivo.parents('tr').find('.inputScta').val(data.scta);
                        inputSelectorActivo.parents('tr').find('.inputSScta').val(data.sscta);
                        dataPoliza.cta = data.cta;
                        dataPoliza.scta = data.scta;
                        dataPoliza.sscta = data.sscta;

                        switch (tipoS) {
                            case "3":
                                {
                                    inputSelectorActivo.val(data.cta);
                                    dtTablaGeneralPolizas.clear().draw();
                                    modalGeneralPolizas.modal('hide');

                                }
                                break;
                            case "4":
                                {
                                    inputSelectorActivo.val(data.scta);
                                    dtTablaGeneralPolizas.clear().draw();
                                    modalGeneralPolizas.modal('hide');

                                }
                                break;
                            case "5":
                                {
                                    inputSelectorActivo.val(data.sscta);
                                    dtTablaGeneralPolizas.clear().draw();
                                    modalGeneralPolizas.modal('hide');
                                }
                                break;
                            default:
                                break;
                        }
                    });
                }
            });
        }

        function nuevaFila() {
            let cc = null;
            let oc = null;
            if (comboOrdenCompra.select2('data').length > 0) {
                if (comboOrdenCompra.select2('data')[0].id != 0) {
                    oc = comboOrdenCompra.select2('data')[0].numero;
                    cc = comboOrdenCompra.select2('data')[0].cc;
                    if (cc == undefined) {
                        oc = ocTempChange.numero;
                        cc = ocTempChange.cc;
                    }
                }
            }
            else {
                cc = comboCC.val();
            }

            dtTablaPolizas.row.add({
                verCFDI: null,
                linea: null,
                cta: null,
                scta: null,
                sscta: null,
                digito: null,
                itm: comboTipoMovimiento.val(),
                istm: null,
                numpro: null,
                referencia: inputCheque.val(),
                cc: cc,
                orden_compra: oc,
                concepto: inputConceptoCheque.val(),
                tm: null,
                monto: null,
                cargaCFDI: null,
                st: null,
                uuid: null,
                cfdiRFC: null,
                metodoPago: null,
                facturaComprobanteExtranjero: null,
                taxID: null,
                area: 0,
                cuenta_oc: 0
            });

            dtTablaPolizas.data().toArray().forEach((element, index) => {
                if (index != 0) {
                    element.linea = index + 1;
                    element.referencia = inputCheque.val();
                    element.tm = 1;
                }
            });
            dtTablaPolizas.draw();
        }

        //#endregion

        //#region Tabla Cheques
        function initTablaCheques() {
            dtTablaCheques = tablaCheques.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    { data: 'cuenta', title: 'Cuenta' },
                    {
                        data: 'cuenta', title: 'Banco', createdCell: function (td, cellData, rowData, row, col) {
                            let objBanco = listaBancos.find(e => e.Value == cellData);
                            $(td).text(objBanco.Text);
                        }
                    },
                    { data: 'numero', title: 'Cheque' },
                    {
                        data: 'fecha_mov', title: 'Fecha', createdCell: function (td, cellData, rowData, row, col) {
                            var jsDate = moment(cellData).format('DD/MM/YYYY');//.toDate();
                            $(td).text(jsDate);
                        }
                    },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'cc', title: 'C.C.' },
                    { data: 'monto', title: 'Monto' },
                    { data: 'hecha_por', title: 'Hecha Por' },
                    { data: 'num_pro_emp', title: 'Proveedor' },
                    { data: 'tipocheque', title: 'T Ch' },
                    { data: 'tm', title: 'T.M.' },
                    {
                        data: 'estatusCheque', title: 'Estatus',
                        createdCell: function (td, cellData, rowData, row, col) {

                            switch (cellData) {
                                case 1:
                                    $(td).text('Con Factura');
                                    break;
                                case 0:
                                    $(td).text('Pendiente');
                                    break;
                                case 3:
                                    $(td).text('Factura Con error');
                                    break;
                                default:
                                    $(td).text('Factura Con error');
                                    break;
                            }
                        }
                    },
                    {
                        data: 'id',
                        render: (data, type, row) => `<button class="btn btn-primary btnLoadCheque" data-id="${data}"><i class="far fa-list-alt"></i></button>`
                    },
                    {
                        data: 'id',
                        render: (data, type, row) => `<button class="btn btn-success btnPrintCheque" data-id="${data}"><i class="fas fa-print"></i></button>`
                    },
                    {
                        data: 'id',
                        render: (data, type, row) => setEliminar(PermisoDelete, data)
                    },
                    {
                        data: 'estatusCheque', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (cellData == 1) {
                                $(td).append(`<button class="btn btn-success editarPolizas" data-id="${rowData.id}"><i class="fas fa-share-square"></i></button>`);
                            }
                        }
                    },
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                drawCallback: function (settings) {

                    tablaCheques.find('.btnLoadCheque').click(function () {
                        let data = dtTablaCheques.row($(this).parents('tr')).data();
                        var jsDate = moment(data.fecha_mov).format('DD/MM/YYYY');//.toDate();
                        let cheque = data.numero;
                        let fecha = jsDate;

                        cargarTablaPoliza(cheque, fecha);
                    });

                    tablaCheques.find('.btnPrintCheque').click(function () {

                        let data = dtTablaCheques.row($(this).parents('tr')).data();
                        var jsDate = moment(data.fecha_mov).format('DD/MM/YYYY');//.toDate();
                        let fechaCheque = jsDate.split('/');
                        let pMes = fechaCheque[1];
                        let pYear = fechaCheque[2];

                        var path = `/Reportes/Vista.aspx?idReporte=187&&pNumCheque=${data.numero}&&pMes=${pMes}&&pYear=${pYear}&&pTipo=${2}`;
                        report.attr("src", path);
                        document.getElementById('report').onload = function () {
                            openCRModal();
                        };
                    });
                    tablaCheques.find('.btnDelete').click(function () {
                        let data = dtTablaCheques.row($(this).parents('tr')).data();
                        fnDeleteCheque(data.id);
                    });
                    tablaCheques.find('.editarPolizas').click(function () {
                        let data = dtTablaCheques.row($(this).parents('tr')).data();
                        fnOpenEditCheques(data.id);
                    });
                }
            });
        }

        function setEliminar(permiso, data) {

            if (permiso)
                return `<button class="btn btn-warning btnDelete" data-id="${data}"><i class="fas fa-trash-alt"></i></button>`
            else
                return '';
        }
        function fnDeleteCheque(chequeID) {

            $.post('/administrativo/cheque/DeleteCheque', { chequeID: chequeID })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral('La operación fue ejecutada correctamente');

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cargarCheques() {
            let filtros = getFiltros();
            if (filtros.cuenta != '') {
                try {
                    $.post('/administrativo/cheque/GetCheques', { filtros })
                        .then(response => {
                            if (response.success) {
                                if (dtTablaCheques != null) {
                                    listaBancos = response.listaBancos;
                                    listaProveedores = response.listaProveedores;
                                    PermisoDelete = response.PermisoDelete;
                                    dtTablaCheques.clear().draw();
                                    dtTablaCheques.rows.add(response.cheques).draw();
                                }
                            }
                            else
                                // Operación no completada.
                                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        },
                            error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            });
                } catch (e) { AlertaGeneral(`Operación fallida`, e.message) }
            }
            else {
                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: Se necesita una cuenta para iniciar la busqueda`);
            }
        }
        //#endregion

        //#region 
        function getFiltros() {
            info = {};

            return {
                cuenta: inputCuentaPpal.val(),
                fechaInicio: inputFechaInicio.val(),
                fechaFin: inputFechaFin.val()
            };
        }

        function limpiarCheque() {

            dtTablaPolizas.clear().draw();
            comboOrdenCompra.val(' ');
            comboOrdenCompra.trigger('change');
            inputCuentaBancoModal.val(' ');
            inputDesrcipcionBancoModal.val(' ');
            inputCheque.val(' ');
            inputFechaCheque.datepicker({
                'dateFormat': 'dd/mm/yy',
                'maxDate': new Date()
            }).datepicker('option', 'showAnim', 'slide')
                .datepicker('setDate', new Date());
            inputDescripcionCheque.val(' ');
            inputDescripcionCheque.data().id = 0;
            inputMonto.val(' ');
            inputCantidad.val(' ');
            inputConceptoCheque.val(' ');
            comboCC.val(' ');
            comboCC.trigger('change');
            comboTipoMovimiento.val(' ');
            comboTipoMovimiento.trigger('change');
            inputPoliza.val(' ');
            inputTipoPolizaCheque.val(' ');
            limpiarPoliza();
        }

        function fnLoadCuenta() {
            $.get('/administrativo/cheque/GetListaCuentasInit')
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        listaCta = response.listaCta;
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function renderComboBox(arrayData, row, classCbo) {
            let html = '';
            html = `<select  class='form-control ${classCbo}' data-index=${row}>`;
            arrayData.forEach(element => {
                html += `<option value='${element.id}'>${element.id}-${element.text}</option>`;
            });
            html += `</select>`;
            return html;
        }

        function limpiarPoliza() {
            inputTipoPoliza.val(' ');
            inputEstatusPoliza.val(' ');
            inputCargosPoliza.val(' ');
            inputPolizaPoliza.val(' ');
            inputGeneradaPoliza.val(' ');
            inputFechaPoliza.val(' ');
            inputCCPoliza.val(' ');
            inputDiferenciaPoliza.val(' ');
            inputConceptoPoliza.val(' ');
            inputCtaPoliza.val(' ');
            inputInterfasePoliza.val(' ');
            inputInterfaseDescPoliza.val(' ');
            inputTMPoliza.val(' ');
            inputAbonosPoliza.val(' ');
            butonNewRenglon.prop('disabled', true);
        }

        modalCapturaCheques.on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        modalVerpolizas.on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        //#endregion

        //#region ver Polizas  de Cheque Capturados para editar.
        function fnOpenEditCheques(idCheque) {
            $.get('/administrativo/cheque/OpenEditCheques', { idCheque: idCheque })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        limpiarParametros();
                        dtTablaPolizaChequeCap.rows.add(response.listaPolizasChequeCap).draw();
                        dtTablaPolizaProveedores.rows.add(response.listaPolizasProveedores).draw();
                        dtTablaPolizaDiario.rows.add(response.listaPolizasDiario).draw();
                        modalVerpolizas.modal('show');
                        setDisabled();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function limpiarParametros() {
            dtTablaPolizaChequeCap.clear().draw();
            dtTablaPolizaProveedores.clear().draw();
            dtTablaPolizaDiario.clear().draw();
        }

        function setDisabled() {
            $('.inputChequeCap').prop('disabled', true);
            $('.inputProveedores').prop('disabled', true);
            $('.inputDiario').prop('disabled', true);
        }

        function initTablasPolizas() {
            initTblPolizaChequeCap();
            initTblPolizaProveedores();
            iniTblPolizaDiario();
        }

        function setInputTabla(classID, valueData) {
            let html = "";
            return html = `<input type='text' class='form-control ${classID}' value= ${valueData} >`;
        }

        function initTblPolizaChequeCap() {
            dtTablaPolizaChequeCap = tablaPolizaChequeCap.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    {
                        data: 'linea', title: 'no',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputChequeCapLinea inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'cta', title: 'Cuenta',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputChequeCapCta inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'scta', title: 'SCta',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputChequeCapSCTA inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'sscta', title: 'SSCta',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputChequeCapSSCTA inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'digito', title: 'D',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputChequeCapDigito inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'itm', title: 'Mov.',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputChequeCapITM inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'istm', title: 'Sub Mov',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputChequeCapISTM inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'numpro', title: 'Proveedor',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputChequeCapNumPro inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'referencia', title: 'Referencia',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputChequeCapReferencia inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'cc', title: 'C.C.',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaChequeCapCC inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'orden_compra',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaChequeCapOrden_Compra inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'concepto',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaChequeCapConcepto inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'tm', title: 'Tipo Movimiento',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaChequeCapTM inputChequeCap", cellData));
                        }
                    },
                    {
                        data: 'monto', title: 'Monto',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaChequeCapMonto inputChequeCap", cellData));
                        }
                    },

                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ]
            });
        }
        function initTblPolizaProveedores() {
            dtTablaPolizaProveedores = tablaPolizaProveedores.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    {
                        data: 'linea', title: 'no',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputProveedoresLinea inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'cta', title: 'Cuenta',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputProveedoresCta inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'scta', title: 'SCta',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputProveedoresSCTA inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'sscta', title: 'SSCta',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputProveedoresSSCTA inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'digito', title: 'D',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputProveedoresDigito inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'itm', title: 'Mov.',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputProveedoresITM inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'istm', title: 'Sub Mov',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputProveedoresISTM inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'numpro', title: 'Proveedor',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputProveedoresNumPro inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'referencia', title: 'Referencia',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputProveedoresReferencia inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'cc', title: 'C.C.',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaProveedoresCC inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'orden_compra',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaProveedoresOrden_Compra inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'concepto',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaProveedoresConcepto inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'tm', title: 'Tipo Movimiento',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaProveedoresTM inputProveedores", cellData));
                        }
                    },
                    {
                        data: 'monto', title: 'Monto',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaProveedoresMonto inputProveedores", cellData));
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ]
            });
        }
        function testDETS() {
            dtTablaPolizaDiario = tablaPolizaDiario.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: false,
                columns: [
                    {
                        data: 'linea', title: 'no',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputDiarioLinea inputDiario", cellData));
                        }
                    },
                    {
                        data: 'cta', title: 'Cuenta',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputDiarioCta inputDiario", cellData));
                        }
                    },
                    {
                        data: 'scta', title: 'SCta',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputDiarioSCTA inputDiario", cellData));
                        }
                    },
                    {
                        data: 'sscta', title: 'SSCta',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputDiarioSSCTA inputDiario", cellData));
                        }
                    },
                    {
                        data: 'digito', title: 'D',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputDiarioDigito inputDiario", cellData));
                        }
                    },
                    {
                        data: 'itm', title: 'Mov.',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputDiarioITM inputDiario", cellData));
                        }
                    },
                    {
                        data: 'istm', title: 'Sub Mov',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputDiarioISTM inputDiario", cellData));
                        }
                    },
                    {
                        data: 'numpro', title: 'Proveedor',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputDiarioNumPro inputDiario", cellData));
                        }
                    },
                    {
                        data: 'referencia', title: 'Referencia',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputDiarioReferencia inputDiario", cellData));
                        }
                    },
                    {
                        data: 'cc', title: 'C.C.',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaDiarioCC inputDiario", cellData));
                        }
                    },
                    {
                        data: 'orden_compra',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaDiarioOrden_Compra inputDiario", cellData));
                        }
                    },
                    {
                        data: 'concepto',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaDiarioConcepto inputDiario", cellData));
                        }
                    },
                    {
                        data: 'tm', title: 'Tipo Movimiento',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaDiarioTM inputDiario", cellData));
                        }
                    },
                    {
                        data: 'monto', title: 'Monto',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append(setInputTabla("inputaDiarioMonto inputDiario", cellData));
                        }
                    },
                    {
                        data: 'id',
                        render: (data, type, row) => `<button class="btn btn-success btnEditarLinea" data-id="${data}" data-tipoTabla=3><i class="fas fa-print"></i></button>`
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ]
            });
        }

        function iniTblPolizaDiario() {
            dtTablaPolizaDiario = tablaPolizaDiario.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: false,
                columns: [
                    {
                        data: 'linea', title: 'no', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let vDisabled = '';
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = `value = '${row + 1}' ${vDisabled}`;
                            html = `<input type='text' class='form-control inputLinea' ${valueData}  disabled>`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'cta', title: 'Cuenta', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            html = '';
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html += `<div style="display: inline-flex;">`
                            html += `<input type='text' class='form-control inputCta' ${valueData} data-row='${row}' >`;
                            html += `<button class="btn btn-info inputctaBuscar" data-tipo='3'>
                                        <i class="fas fa-search"></i>
                                    </button> </div>`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'scta', title: 'SCta', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html = '';
                            html += `<div style="display: inline-flex;">`
                            html += `<input type='text' class='form-control inputScta' ${valueData}  data-row='${row}'>`;
                            html += `<button class="btn btn-info inputsctaBuscar" data-tipo='4'>
                                        <i class="fas fa-search"></i>
                                    </button> </div>`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'sscta', title: 'SSCta', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html = '';
                            html += `<div style="display: inline-flex;">`
                            html += `<input type='text' class='form-control inputSScta' ${valueData}  data-row='${row}'>`;
                            html += `<button class="btn btn-info inputssctaBuscar" data-tipo='5'>
                                        <i class="fas fa-search"></i>
                                    </button> </div>`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'digito', title: 'D', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}' ` : "";
                            html = `<input type='text' class='form-control inputDigito' ${valueData} >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'itm', title: 'Mov.', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            $(td).append(renderComboBox(listacbTipoMov, row, "CombotmPolizasDiario"));
                            $(td).children().val(cellData);
                        }
                    },
                    {
                        data: 'istm', title: 'Sub Mov', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html = `<input type='text' class='form-control' ${valueData}  disabled >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'numpro', title: 'Proveedor', createdCell: function (td, cellData, rowData, row, col) {
                            html = "";
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            html += `<div style="display: inline-flex;">`
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html += `<input type='text' class='form-control inputNumpro' ${valueData}  >`;
                            html += `<button class="btn btn-info btnBuscarProveedor">
                            <i class="fas fa-search"></i>
                        </button> </div>`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'referencia', title: 'Referencia', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html = `<input type='text' class='form-control inputReferencia' ${valueData}  >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'cc', title: 'C.C.', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let cc = cellData;
                            if (cellData == 0)
                                cc = '*'
                            valueData = cellData != null ? `value = '${cc}'` : "";
                            html = `<input type='text' class='form-control inputCC' ${valueData}  >`;

                            $(td).append(html);
                        }
                    },
                    {
                        data: 'orden_compra', title: 'O.C/Cte', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}'` : "";
                            html = `<input type='text' class='form-control inputOrdenCompra' ${valueData}  >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'concepto', title: 'Concepto', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            valueData = cellData != null ? `value = '${cellData}' ${vDisabled}` : "";
                            html = `<input type='text' class='form-control inputConcepto' ${valueData}  >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'tm', title: 'Tipo Movimiento',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (row == 0) {
                                vDisabled = 'disabled'
                            }
                            html = `<select class='form-control inputTMs ${vDisabled}'>
                                            <option value="1">1-Cargo</option>
                                            <option value="2">2-Abono</option>
                                            <option value="3">3-Cargo Rojo</option>
                                            <option value="4">4-Abono Rojo</option>
                                        </select>`;
                            $(td).append(html);
                            $(td).children().val(cellData);

                        }
                    },
                    {
                        data: 'monto', title: 'Monto',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let vDisabled = '';
                            if (rowData.linea == 1) {
                                vDisabled = 'disabled'
                            }
                            if (cellData == null)
                                cellData = 0;
                            valueData = `value = '${cellData}' ${vDisabled}`;
                            html = `<input type='text' class='form-control inputMonto' ${valueData}  >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'linea',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let btn = '';
                            btn = '<button class="btn btn-info delete"><i class="fas fa-times"></i></button >';
                            $(td).append(btn);
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                drawCallback: function (settings) {
                    //   $('.CombotmPolizasDiario').select2();
                    tablaPolizaDiario.find('.btnBuscarProveedor').click(function () {
                        inputSelectorActivo = $(this).parents('tr').find('.inputNumpro');
                        lblGeneral.text('Lista Proveedores');
                        tipoBusqueda = 3;
                        modalGeneral.modal('show');
                        detTablaGeneral.clear().draw();
                        cargarTablaGeneral('/administrativo/cheque/GetProveedores');
                    });

                    tablaPolizaDiario.find('.inputITM').change(function () {
                        let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
                        data.itm = $(this).val();
                    });

                    tablaPolizaDiario.find('.inputNumpro').change(function () {
                        let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
                        data.numpro = $(this).val();
                    });
                    tablaPolizaDiario.find('.inputReferencia').change(function () {

                        let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
                        data.referencia = $(this).val();
                    });

                    tablaPolizaDiario.find('.inputCC').change(function () {
                        let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
                        data.cc = $(this).val();
                    });

                    tablaPolizaDiario.find('.inputOrdenCompra').change(function () {
                        let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
                        data.orden_compra = $(this).val();
                    });


                    tablaPolizaDiario.find('.concepto').change(function () {
                        let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
                        data.concepto = $(this).val();
                    });

                    tablaPolizaDiario.find('.inputTMs').change(function () {

                        let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
                        let monto = $(this).parents('tr').children().find('.inputMonto').val();
                        data.tm = $(this).val();
                        data.monto = data.tm == "1" ? Math.abs(monto) : Math.abs(monto) * -1;
                        $(this).parents('tr').children().find('.inputMonto').val(data.monto);

                    });

                    tablaPolizaDiario.find('.inputMonto').change(function () {
                        let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
                        let tm = data.tm;
                        if (tm == 1)
                            data.monto = $(this).val();
                        else
                            data.monto = -$(this).val();
                    });

                    tablaPolizaDiario.find('.tipoTM').change(function () {

                        let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
                        data.tm = $(this).val();
                        let tm = data.tm;
                        if (tm == 1)
                            data.monto = $(this).val();
                        else
                            data.monto = -$(this).val();

                    });

                    tablaPolizaDiario.find('.inputctaBuscar').click(function () {

                        let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
                        data.tm = $(this).val();
                        inputSelectorActivo = $(this).parents('tr').find('.inputCta');
                        modalGeneralPolizas.modal('show');
                        let obj = $.map(listaCta, function (element, index) {
                            element.tipoS = 3;
                            return element;
                        });
                        dtTablaGeneralPolizas.clear().draw();
                        dtTablaGeneralPolizas.rows.add(obj).draw();

                    });

                    tablaPolizaDiario.find('.inputsctaBuscar').click(function () {
                        let vCta = $(this).parents('tr').find('.inputCta').val();

                        inputSelectorActivo = $(this).parents('tr').find('.inputScta');
                        $(this).parents('tr').find('.inputScta').val(' ');
                        $(this).parents('tr').find('.inputSScta').val(' ');
                        $(this).parents('tr').find('.inputDigito').val(' ');
                        modalGeneralPolizas.modal('show');

                        let obj = $.map(listaCta, function (element, index) {
                            element.tipoS = 4;
                            if (element.cta == vCta)
                                return element;
                        });
                        dtTablaGeneralPolizas.clear().draw();
                        dtTablaGeneralPolizas.rows.add(obj).draw();
                    });

                    tablaPolizaDiario.find('.inputssctaBuscar').click(function () {
                        let vCta = $(this).parents('tr').find('.inputCta').val();
                        let vSCta = $(this).parents('tr').find('.inputScta').val();
                        inputSelectorActivo = $(this).parents('tr').find('.inputSScta');
                        $(this).parents('tr').find('.inputSScta').val(' ');
                        $(this).parents('tr').find('.inputDigito').val(' ');
                        modalGeneralPolizas.modal('show');
                        let obj = $.map(listaCta, function (element, index) {
                            element.tipoS = 5;
                            if (element.cta == vCta && element.scta == vSCta)
                                return element;
                        });
                        dtTablaGeneralPolizas.clear().draw();
                        dtTablaGeneralPolizas.rows.add(obj).draw();
                    });

                    tablaPolizaDiario.find('.CombotmPolizasDiario').change(function () {
                        let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
                        data.itm = $(this).val();

                    });

                    tablaPolizaDiario.find('.inputDigito').change(function () {
                        let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
                        data.digito = $(this).val();
                    });

                }
            });
        }

        tablaPolizaDiario.on('click', '.delete', function () {

            let data = dtTablaPolizaDiario.row($(this).parents('tr')).data();
            let count = 2;
            let listaDTTablaPolizas = dtTablaPolizaDiario.data().toArray().map((value, index) => {

                if (data.linea != value.linea) {
                    if (index == 0) {
                        return value;
                    }
                    else {
                        value.linea = count;
                        count++;
                        return value;
                    }
                }
            }).filter(x => x != undefined);
            dtTablaPolizaDiario.clear();
            dtTablaPolizaDiario.rows.add(listaDTTablaPolizas).draw();
        });


        function fnNuevaFilaPolizaDiario() {

            var polizaDiario = dtTablaPolizaDiario.data().toArray()[0];
            var Linea = dtTablaPolizaDiario.data().toArray()[dtTablaPolizaDiario.data().toArray().length - 1];

            dtTablaPolizaDiario.row.add({
                verCFDI: null,
                linea: Linea,
                cta: null,
                scta: null,
                sscta: null,
                digito: null,
                itm: polizaDiario.itm,
                istm: null,
                numpro: null,
                referencia: polizaDiario.referencia,
                cc: cc,
                orden_compra: oc,
                concepto: polizaDiario.concepto,
                tm: null,
                monto: null,
                cargaCFDI: null,
                st: null,
                uuid: null,
                cfdiRFC: null,
                metodoPago: null,
                facturaComprobanteExtranjero: null,
                taxID: null
            });
            dtTablaPolizaDiario.draw();

        }

        //#endregion
    }

    $(() => Administracion.Cheque.CapturaCheque = new CapturaCheque())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();
