(() => {
    $.namespace('Maquinaria.DocumentosPorPagar.ProgramacionPagos');

    ProgramacionPagos = function () {

        let _comboACGlobal;
        const tipoCambioTabla = $('#tipoCambioTabla');
        const cboTipocampo = $("#cboTipocampo");
        //Selectores Modal Generacion Polizas
        const comboEmpresa = $("#comboEmpresa");
        const modalPolizaProgramados = $('#modalPolizaProgramados');
        const inputFechaPoliza = $('#inputFechaPoliza');
        const inputPoliza = $('#inputPoliza');
        const inputTipoPoliza = $('#inputTipoPoliza');
        const inputGenerada = $('#inputGenerada');
        const btnGuardarPoliza = $('#btnGuardarPoliza');
        const modalGeneral = $('#modalGeneral');
        const btnDeselect = $("#btnDeselect");
        const lblDeselect = $("#lblDeselect");
        const report = $("#report");
        const cboInstitucionPP = $('#cboInstitucionPP');//Add mfjavier
        const inputFechaCambio = $('#inputFechaCambio');//Add mfjavier
        const inputTipoCambio = $('#inputTipoCambio');//Add mfjavier
        const comboEmpresaPoliza = $('#comboEmpresaPoliza');
        const inputTipoCambioPropuesta = $("#inputTipoCambioPropuesta");
        const inputFechaCaptura = $("#inputFechaCaptura");

        const modalDetalleProgramacionPagos = $("#modalDetalleProgramacionPagos");
        //TAGS Pantalla Principal
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const cboEstatus = $('#cboEstatus');
        const cboInstitucion = $("#cboInstitucion");

        //TAGS Botones
        const btnBuscar = $('#btnBuscar');
        const btnGuardarPagos = $('#btnGuardarPagos');
        const btnPoliza = $('#btnPoliza');
        const btnReporteProgramado = $('#btnReporteProgramado');

        //TAGS Tablas
        const tblPpalProgramacion = $('#tblPpalProgramacion');
        const tblPagosProgramados = $('#tblPagosProgramados');
        const tblPolizas = $('#tblPolizas');
        const tablaCtas = $("#tablaCtas");
        const tableDetalleProgramacionPagos = $('#tableDetalleProgramacionPagos');
        // Variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        let _dtPagosProgramados;
        let _currentButton;

        //#region datatables --Aquí se 
        let dtPpalProgramacion;
        let dtPagosProgramados;
        let dtPolizas;
        let dtCtas;
        let dtDetalleProgramacionPagos;
        let consecutivoParaReferenciaUtilizados = [];

        //#endregion
        //#region  selectores Programacion de pagos
        let dtTablaProgramacion;
        const tablaProgramacion = $('#tablaProgramacion');
        //#endregion

        let tablaGenPropuesta = $("#tablaGenPropuesta");
        let dtTablaGenPropuesta;

        const fechaActual = new Date();

        /* */
        let divTablaProgramacion = $("#divTablaProgramacion");
        let divTablaGenPropuesta = $("#divTablaGenPropuesta");

        let inpuntMoneda = $("#inpuntMoneda");

        (function init() {

            inputFechaInicio.prop('disabled', true);
            // Lógica de inicialización.
            btnPoliza.prop('disabled');
            iniTablas();
            initEventListener();
            fnCargarTablaPpal();
            initTablaPropuesta();

        })();

        //#region  Métodos.
        function iniTablas() {

            initTablaPPagos();
            initTablaProgramacionPagosPoliza();
            initTablaDetallePoliza();
            initTablaCtas();
            iniTableDetalleProgramacionPagos();
            initTablaGenPropuesta();
        }

        function initEventListener() {
            divTablaProgramacion.addClass('hide');
            btnReporteProgramado.prop('disabled', true);
            btnGuardarPoliza.click(fnGuardarPoliza);
            btnPoliza.click(fnCargaInfoPolizas);
            btnBuscar.click(fnLoadProgramacionPagos);
            btnGuardarPagos.click(fnGuardarPropuesta);
            inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker('setDate', fechaActual);
            inputFechaFin.datepicker({}).datepicker('setDate', fechaActual);
            inputFechaPoliza.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker('setDate', fechaActual);
            inputFechaCambio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker('setDate', fechaActual);//Add mfjavier
            inputFechaCaptura.datepicker().datepicker('setDate', fechaActual);
            inputFechaFin.datepicker({}).datepicker('setDate', fechaActual);

            inputFechaCambio.change(GetTipoCambioFecha);//Add mfjavier
            cboInstitucionPP.change(fnCargaInfoPolizas);//Add mfjavier
            cboTipocampo.change(fnCargaInfoPolizas);
            comboEmpresaPoliza.change(fnCargaInfoPolizas)
            cboEstatus.change(fnCambiarEstatus);
            btnDeselect.change(fnDeselectALL);
            btnReporteProgramado.click(fnLoadReporte);
            initComboInstituciones();
            tipoCambioTabla.change(fnChangeTipoCambio);
            inputFechaPoliza.change(setPoliza);

        }

        function fnChangeTipoCambio() {
            if (inpuntMoneda.val() == "2") {
                dtTablaGenPropuesta.data().toArray().forEach(element => {
                    element.tipoCambio = (+$(this).val());
                    element.total = element.importeDLLS * (+$(this).val());
                });

                let dtTemp = dtTablaGenPropuesta.data().toArray();
                dtTablaGenPropuesta.clear();
                dtTablaGenPropuesta.rows.add(dtTemp).draw();
            }
        }

        function comboCbosData() {
            $.get('/Contratos/comboCbosData')
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        _comboACGlobal = response.comboAC;
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

        function initTablaCtas() {
            dtCtas = tablaCtas.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                "processing": true,
                "bServerSide": true,
                "sAjaxSource": "/Contratos/LoadCtasServerSide/",
                "fnServerData": function (sSource, aoData, fnCallback) {
                    $.ajax({
                        type: "Get",
                        data: aoData,
                        url: sSource,
                        success: fnCallback
                    })
                },
                columns: [
                    {
                        data: 'cta', title: 'cta', createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text(`${rowData.cta}-${rowData.scta}-${rowData.sscta}`);
                        }
                    },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'tipoS', render: (data, type, row) => `<button class="btn btn-success seleccionar" data-tipoS="${data}"><i class="far fa-check-square"></i></button>` },
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ]
            });


        }

        function initComboInstituciones() {
            cboInstitucion.fillCombo('/Contratos/ObtenerInstituciones', null, false, "Todos", initComboInstitucionesPP);//Cambio mfjavier
            convertToMultiselect("#cboInstitucion");
            comboCbosData();

        }

        //#endregion

        //#region Polizas
        //Add mfjavier
        function initComboInstitucionesPP() {
            $('#cboInstitucion option').each(function (value, index, array) {
                $(this).clone().appendTo('#cboInstitucionPP');
            });
        }

        function setPoliza() {

            $.get('/Contratos/getPolizaByFecha', { fechaP: inputFechaPoliza.val() })
                .then(response => {
                    // Operación exitosa.
                    inputPoliza.val(response.maxPoliza);
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }
        //Add fin

        function fnDeselectALL() {

            if ($(this).prop('checked')) {
                $('.checkProgramar').prop('checked', true);
                dtTablaGenPropuesta.data().toArray().forEach(element => {
                    element.programado = 1
                });
                lblDeselect.text('Quitar Seleccion');
            }
            else {
                $('.checkProgramar').prop('checked', false);
                dtTablaGenPropuesta.data().toArray().forEach(element => {
                    element.programado = 0
                });
                lblDeselect.text('Seleccionar Todos');
            }
            dtTablaGenPropuesta.draw();
        }

        function fnGuardarPoliza() {
            let _tCambio = 1;
            if (cboTipocampo.val() == 2) {
                _tCambio = inputTipoCambio.val();
            }

            let data = getInfoTablas();

            if (data.esValida) {
                $.blockUI({ message: 'Procesando' });
                $.post('/Contratos/guardarPoliza', { poliza: data.poliza, movPol: data.movpol, contrato: data.contrato, tipoCambio: _tCambio })
                    .always($.unblockUI)
                    .then(response => {
                        $.unblockUI;
                        if (response.success) {
                            // Operación exitosa.
                            inputPoliza.val(response.poliza);
                            switch (comboEmpresaPoliza.val()) {
                                case 6:
                                    AlertaGeneral(`Operacion Exitosa`, `Se completo el guardado Exitosamente.`);
                                    break;
                                default:
                                    AlertaGeneral(`Operacion Exitosa`, `Se completo el guardado Exitosamente. Póliza: ${response.poliza}`);
                                    break;
                            }
                            modalPolizaProgramados.modal('hide');
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => {
                        $.unblockUI;
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    });
            }
        }


        function getInfoTablas() {

            if (comboEmpresaPoliza.val()) {
                dtPolizas.data().toArray().forEach(element => {
                    element.referencia = element.descripcion;
                });
            }

            let movpol = dtPolizas.data().toArray();
            let esValida = false;
            let contrato = [];
            let cargos = 0;
            let abonos = 0;
            dtPolizas.data().toArray().forEach(element => {
                let obj = { contratoID: element.contratoPolizaID, parcialidad: element.parcialidad };
                cargos += element.cargo;
                abonos += element.abono;
                contrato.push(obj);
            });
            let poliza = {
                id: 0,
                poliza: inputPoliza.val(),
                generada: 'C',//inputGenerada.val(),
                mes: moment(inputFechaPoliza.val(), 'DD/MM/YYYY').format("MM"),
                year: moment(inputFechaPoliza.val(), 'DD/MM/YYYY').format("YYYY"),
                tp: "03",
                fechapol: moment(inputFechaPoliza.val(), 'DD/MM/YYYY').toISOString(true),
                cargos: cargos,
                abonos: abonos,
                status: null,
                error: null,
                fec_hora_movto: inputFechaPoliza.val(),
                usauario_movto: 1,
                fecha_hora_crea: inputFechaPoliza.val(),
                usuario_crea: '',
                socio_inversionista: 0,
                status_carga_pol: 'S',
                concepto: 'CONTRATOS'
            };
            if (movpol.length > 0) {
                esValida = true;
            }
            return {
                poliza: poliza,
                movpol: movpol,
                esValida: esValida,
                contrato: contrato
            };
        };

        //Add mfjavier
        function GetTipoCambioFecha() {
            $.get('/Contratos/GetTipoCambioFecha', {
                fecha: inputFechaCambio.val()
            }).then(response => {
                if (response.success) {
                    if (response.tipoCambio != null && response.tipoCambio.moneda == 2) {
                        inputTipoCambio.val(response.tipoCambio.tipo_cambio);
                        inputTipoCambioPropuesta.val(response.tipoCambio.tipo_cambio);
                    } else {
                        inputTipoCambio.val('$0.00');
                        inputTipoCambioPropuesta.val('$0.00');
                        //     btnGuardarPoliza.attr('disabled', true);
                    }
                } else {
                    inputTipoCambio.val('$0.00');
                    inputTipoCambioPropuesta.val('$0.00');
                    //  btnGuardarPoliza.attr('disabled', true);
                    AlertaGeneral(`Alerta`, `${response.message}`);
                }
            }, error => {
                inputTipoCambio.val('$0.00');
                inputTipoCambioPropuesta.val('$0.00');
                //  btnGuardarPoliza.attr('disabled', true);
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }
        //Add Fin

        function fnCargaInfoPolizas() {
            consecutivoParaReferenciaUtilizados = [];

            $.get('/Contratos/loadPropuestas', {
                idInstitucion: cboInstitucionPP.val(),
                empresa: comboEmpresaPoliza.val(),
                moneda: cboTipocampo.val()
            }).then(response => {
                if (response.success) {
                    // Operación exitosa.
                    dtPolizas.clear().draw();
                    GetTipoCambioFecha();
                    modalPolizaProgramados.modal('show');
                    _dtPagosProgramados = response.result;//.filter(r => (r.tipocambio != 1 ? 2 : 1) == (+cboTipocampo.val()));//response.result.toArray().filter(r => { return r.tipoCambio == cboTipocampo.val() });
                    dtPagosProgramados.clear();
                    dtPagosProgramados.rows.add(_dtPagosProgramados).draw();

                } else {
                    // Operación no completada.
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                // Error al lanzar la petición.
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            ).then(setPoliza());
        }

        tblPagosProgramados.on('click', '.btnAddPago', function () {
            let data = dtPagosProgramados.row($(this).parents('tr')).data();
            fnGenerarContratoPoliza(data.contratoID, data.parcialidad);

        });

        tblPagosProgramados.on('click', '.btnMostrarDetalle', function () {
            let data = dtPagosProgramados.row($(this).parents('tr')).data();
            fnGetDetallePago(data.parcialidad, data.contratoID);
        });

        function fnGetDetallePago(parcialdiad, contratoId) {
            $.get('/Contratos/CargarDetallePago', { parcialidad: parcialdiad, contratoId: contratoId })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        dtDetalleProgramacionPagos.clear();
                        dtDetalleProgramacionPagos.rows.add(response.contratoDetalle).draw(false);
                        modalDetalleProgramacionPagos.modal('show');
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        function fnGenerarContratoPoliza(contratoId, parcialidad) {
            $.blockUI({ message: 'Espere Porfavor' });
            $.get('/Contratos/CargarContrato', { contratoId: contratoId, parcialidad: parcialidad, fechaPol: moment(inputFechaPoliza.val(), 'DD/MM/YYYY').toISOString(true) })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        let dataProgramacion = response.contratoDetalle;
                        let contratoHistorico = response.contratoHistorico;
                        let ctasInstitucion = response.ctasInstitucion;
                        setLineaPoliza(dataProgramacion, contratoHistorico, contratoId, parcialidad, ctasInstitucion);
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

        function setLineaPoliza(dataProgramacion, contratoHistorico, contratoId, parcialidad, ctasInstitucion) {

            _ccMaquina = cc = dataProgramacion[0].cc;

            let tipoCambio = 1;
            let cc191 = comboEmpresaPoliza.val() == "2" && contratoHistorico.cc191;
            if (contratoHistorico.monedaContrato == 2) {
                tipoCambio = unmaskNumero(inputTipoCambio.val());
            }

            if (tipoCambio != 0) {
                let contadorLinea = tblPolizas.DataTable().data().length + 1;
                let descripcionCplan2 = "";
                dataProgramacion.forEach(item => {
                    if ((descripcionCplan2.length + item.noEconomico.length) <= 11)
                        descripcionCplan2 += item.noEconomico;
                });

                // Linea 1 Poliza Cuenta 2120/5000
                const calcularIva = contratoHistorico.calcularIVA;
                let linea = contadorLinea++;
                let cta = contratoHistorico.cta;
                let scta = contratoHistorico.scta;
                let sscta = contratoHistorico.sscta;
                let d = contratoHistorico.digito;
                let ctaIA = contratoHistorico.ctaIA;
                let sctaIA = contratoHistorico.sctaIA;
                let ssctaIA = contratoHistorico.ssctaIA;
                let digitoIA = contratoHistorico.digitoIA;
                let descripcion = '';

                let cc = '200';
                let areaCuenta = "";
                if (comboEmpresaPoliza.val() == "2") {
                    cc = '191';
                    descripcion = contratoHistorico.descripcion;
                }

                else {
                    cc = '994';
                    descripcion = descripcionCplan2

                    if (contratoHistorico.tieneCCEspecial) {
                        cc = contratoHistorico.ccEspecial;
                    }
                }

                if (!calcularIva) {
                    cc = _ccMaquina;
                }

                let referencia = contratoHistorico.folio.match(/\d/g).join("");
                let concepto = `${contratoHistorico.folio.replace(/^\D+/g, '')} PARC ${contratoHistorico.parcialidad}/${contratoHistorico.plazo}`;
                let tm = 1;
                let itm = 0;
                let cargos = contratoHistorico.importePago * contratoHistorico.tipoCambioPeriodioAnterior;

                //
                if (contratoHistorico.arrendamientoPuro) {
                    cargos = 0;

                    dataProgramacion.forEach(item => {
                        cargos += (item.capital * inputTipoCambio.val());
                    });
                }
                //

                let abono = 0;

                if (comboEmpresaPoliza.val() == "2") {
                    areaCuenta = '16-2';
                }
                else {
                    areaCuenta = '0-0';
                }
                if (!calcularIva) {
                    areaCuenta = dataProgramacion[0].areaCuenta;
                }
                //fnAddRowPoliza(linea, cta, scta, sscta, d, descripcion, cc, referencia, concepto, itm, tm, cargos, abono, contratoId, parcialidad, areaCuenta);
                let _refPrimerasLineas = "";
                if (dataProgramacion.length <= 2) {
                    dataProgramacion.forEach(function (value, index, arr) {
                        _refPrimerasLineas += (value.noEconomico + "_");
                    });
                    _refPrimerasLineas = _refPrimerasLineas.substring(0, _refPrimerasLineas.length - 1);
                }

                fnAddRowPoliza(
                    linea,
                    cta,
                    scta,
                    sscta,
                    d,
                    comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : _refPrimerasLineas != "" ? _refPrimerasLineas : contratoHistorico.descripcion,
                    cc,
                    comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : _refPrimerasLineas != "" ? _refPrimerasLineas : contratoHistorico.descripcion,
                    concepto,
                    itm,
                    tm,
                    cargos,
                    abono,
                    contratoId,
                    parcialidad,
                    areaCuenta
                );
                //
                //Linea 2 Poliza Cuenta 1315
                if (calcularIva) {
                    cta = 1315;
                    scta = 11;
                    sscta = 1;
                    if (cboInstitucionPP.find('option:selected').text() == 'CATERPILLAR') {
                        scta = 12;
                        sscta = 0;
                    }
                    if (cboInstitucionPP.find('option:selected').text() == 'SANDVIK') {
                        scta = 15;
                        sscta = 0;
                    }
                    d = 0;
                    if (comboEmpresaPoliza.val() == "2") {
                        descripcion = contratoHistorico.descripcion;
                        cc = '200';

                        if (cc191) {
                            cc = '191';
                        }
                    }
                    else {
                        cc = '994';
                        descripcion = descripcionCplan2;

                        if (contratoHistorico.tieneCCEspecial) {
                            cc = contratoHistorico.ccEspecial;
                        }
                    }

                    if (!calcularIva) {
                        cc = _ccMaquina;
                    }

                    if (ctaIA > 0) {
                        cta = ctaIA;
                        scta = sctaIA;
                        sscta = ssctaIA;
                        d = digitoIA;
                    }

                    referencia = contratoHistorico.folio.match(/\d/g).join("");
                    concepto = `${contratoHistorico.folio.replace(/^\D+/g, '')} PARC ${contratoHistorico.parcialidad}/${contratoHistorico.plazo}`;
                    tm = 2;
                    itm = 0;
                    cargos = 0;
                    if (comboEmpresaPoliza.val() == "2") {
                        areaCuenta = '14-1';

                        if (cc191) {
                            areaCuenta = '16-2';
                        }
                    } else {
                        areaCuenta = '0-0';
                    }

                    abono = -contratoHistorico.interesesPeriodo * contratoHistorico.tipoCambioHistorico;//contratoHistorico.tipoCambioContrato;
                    if ((cargos + abono) != 0) {
                        linea = contadorLinea++;
                        //fnAddRowPoliza(linea, cta, scta, sscta, d, descripcion, cc, referencia, concepto, itm, tm, cargos, abono, contratoId, parcialidad, areaCuenta);
                        fnAddRowPoliza(
                            linea,
                            cta,
                            scta,
                            sscta,
                            d,
                            comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : _refPrimerasLineas != "" ? _refPrimerasLineas : contratoHistorico.descripcion,
                            cc,
                            comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : _refPrimerasLineas != "" ? _refPrimerasLineas : contratoHistorico.descripcion,
                            concepto,
                            itm,
                            tm,
                            cargos,
                            abono,
                            contratoId,
                            parcialidad,
                            areaCuenta
                        );
                    }
                    // Linea 3 Poliza Se agrega el iva 14-1 cta 1146
                    cta = 1146;
                    scta = 4;
                    sscta = 0;
                    d = 0;

                    if (comboEmpresaPoliza.val() == "2") {
                        cc = '200';

                        if (cc191) {
                            cc = '191';
                        }
                    }
                    else {
                        cc = '994';

                        if (contratoHistorico.tieneCCEspecial) {
                            cc = contratoHistorico.ccEspecial;
                        }
                    }

                    if (!calcularIva) {
                        cc = _ccMaquina;
                    }

                    referencia = contratoHistorico.folio.match(/\d/g).join("");
                    concepto = `${contratoHistorico.folio.replace(/^\D+/g, '')} PARC ${contratoHistorico.parcialidad} / ${contratoHistorico.plazo}`;
                    tm = 2;
                    itm = 0;
                    cargos = 0;
                    abono = -(contratoHistorico.ivaCapita + contratoHistorico.ivaIntereses) * contratoHistorico.tipoCambioHistorico;
                    if (comboEmpresaPoliza.val() == "2") {
                        descripcion = contratoHistorico.descripcion;
                        areaCuenta = '14-1';

                        if (cc191) {
                            areaCuenta = '16-2';
                        }
                    }
                    else {
                        areaCuenta = '0-0';
                        descripcion = descripcionCplan2;
                    }
                    if ((cargos + abono) != 0) {
                        linea = contadorLinea++;
                        //fnAddRowPoliza(linea, cta, scta, sscta, d, descripcion, cc, referencia, concepto, itm, tm, cargos, abono, contratoId, parcialidad, areaCuenta);
                        fnAddRowPoliza(
                            linea,
                            cta,
                            scta,
                            sscta,
                            d,
                            comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : _refPrimerasLineas != "" ? _refPrimerasLineas : contratoHistorico.descripcion,
                            cc,
                            comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : _refPrimerasLineas != "" ? _refPrimerasLineas : contratoHistorico.descripcion,
                            concepto,
                            itm,
                            tm,
                            cargos,
                            abono,
                            contratoId,
                            parcialidad,
                            areaCuenta
                        );
                    }
                }

                // 
                //
                // Linea 4 Poliza Se agrega el Iva Cuenta 1147
                dataProgramacion.forEach(item => {
                    cta = 1147;
                    scta = 4;
                    sscta = 0;
                    d = 0;

                    cc = item.cc;
                    referencia = contratoHistorico.folio.match(/\d/g).join("");
                    if (contratoHistorico.mostrarRFCenIVA) {
                        concepto = contratoHistorico.rfc;
                    } else {
                        concepto = `${contratoHistorico.folio.replace(/^\D+/g, '')} PARC ${contratoHistorico.parcialidad} / ${contratoHistorico.plazo}`;
                    }
                    tm = 1;
                    itm = 0;
                    cargos = (item.iva + item.ivaInteres) * tipoCambio
                    abono = 0;
                    if (comboEmpresaPoliza.val() == "2") {
                        descripcion = contratoHistorico.descripcion;
                        if (item.areaCuenta.indexOf("-") != -1)
                            areaCuenta = item.areaCuenta;
                        else
                            areaCuenta = '14-1'
                    }
                    else {
                        descripcion = descripcionCplan2;
                        areaCuenta = '0-0';
                    }
                    if ((cargos + abono) != 0) {
                        linea = contadorLinea++;
                        //fnAddRowPoliza(linea, cta, scta, sscta, d, descripcion, cc, referencia, concepto, itm, tm, cargos, abono, contratoId, parcialidad, areaCuenta);
                        fnAddRowPoliza(
                            linea,
                            cta,
                            scta,
                            sscta,
                            d,
                            comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : item.noEconomico,
                            cc,
                            comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : item.noEconomico,
                            concepto,
                            itm,
                            tm,
                            cargos,
                            abono,
                            contratoId,
                            parcialidad,
                            areaCuenta
                        );
                    }
                });
                // Linea 5++ Poliza Se agrega el Iva Cuenta 1146

                cargos = 0;
                if (calcularIva) {
                    dataProgramacion.forEach(item => {

                        cta = 5900;
                        scta = 3;
                        sscta = 0;
                        d = 0;

                        cc = item.cc;
                        referencia = referencia;
                        concepto = `${contratoHistorico.folio.replace(/^\D+/g, '')} PARC ${contratoHistorico.parcialidad}/${contratoHistorico.plazo} ${item.noEconomico}`;
                        tm = 1;
                        itm = 0;
                        cargos = (item.intereses * tipoCambio);
                        abono = 0;
                        if (comboEmpresaPoliza.val() == "2") {
                            descripcion = referencia;
                            if (item.areaCuenta.indexOf("-") != -1)
                                areaCuenta = item.areaCuenta;
                            else
                                areaCuenta = '14-1'
                        }
                        else {
                            areaCuenta = '0-0';
                            descripcion = descripcionCplan2;
                        }

                        if ((cargos + abono) != 0) {
                            linea = contadorLinea++;
                            //fnAddRowPoliza(linea, cta, scta, sscta, d, descripcion, cc, referencia, concepto, itm, tm, cargos, abono, contratoId, parcialidad, areaCuenta);
                            fnAddRowPoliza(
                                linea,
                                cta,
                                scta,
                                sscta,
                                d,
                                comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : item.noEconomico,
                                cc,
                                comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : item.noEconomico,
                                concepto,
                                itm,
                                tm,
                                cargos,
                                abono,
                                contratoId,
                                parcialidad,
                                areaCuenta
                            );
                        }
                    });
                }

                let consecutivoParaReferencia = 0;
                dataProgramacion.forEach(item => {

                    let descripcionCplan = "";
                    let cuentasPesos = ctasInstitucion.filter(r => r.moneda == cboTipocampo.val() && !r.complementaria);
                    // linea = contadorLinea++;
                    cta = cuentasPesos[0].cta;
                    scta = cuentasPesos[0].scta;
                    sscta = cuentasPesos[0].sscta;
                    d = cuentasPesos[0].digito;

                    descripcionCplan += item.noEconomico;
                    cc = item.cc;
                    referencia = referencia;
                    concepto = `${contratoHistorico.folio.replace(/^\D+/g, '')} PARC ${contratoHistorico.parcialidad}/${contratoHistorico.plazo} ${item.noEconomico}`;
                    tm = 2;
                    itm = 51;
                    cargos = 0;
                    if (contratoHistorico.monedaContrato == 2)
                        abono = -(item.importeDLLS);
                    else
                        abono = -(item.importe);
                    // item.tipocam
                    if (comboEmpresaPoliza.val() == "2") {
                        descripcion = referencia;
                        if (item.areaCuenta.indexOf("-") != -1)
                            areaCuenta = item.areaCuenta;
                        else
                            areaCuenta = '14-1'
                    }
                    else {
                        areaCuenta = '0-0';
                        descripcion = descripcionCplan2;
                    }
                    if ((cargos + abono) != 0) {
                        linea = contadorLinea++;
                        //fnAddRowPoliza(linea, cta, scta, sscta, d, descripcion, cc, referencia, concepto, itm, tm, cargos, abono, contratoId, parcialidad, areaCuenta);
                        let existe = undefined;
                        do {

                            existe = consecutivoParaReferenciaUtilizados.find(element => element == contratoHistorico.descripcion + consecutivoParaReferencia);

                            if (existe != undefined) {
                                consecutivoParaReferencia++;
                            }

                        } while (existe != undefined);

                        fnAddRowPoliza(
                            linea,
                            cta,
                            scta,
                            sscta,
                            d,
                            contratoHistorico.descripcion + consecutivoParaReferencia,
                            cc,
                            contratoHistorico.descripcion + consecutivoParaReferencia,
                            concepto,
                            itm,
                            tm,
                            cargos,
                            abono,
                            contratoId,
                            parcialidad,
                            areaCuenta,
                            item.liquidar);

                        consecutivoParaReferenciaUtilizados.push(contratoHistorico.descripcion + consecutivoParaReferencia);
                    }


                    if (contratoHistorico.monedaContrato == 2) {
                        let cuentasDlls = ctasInstitucion.filter(r => r.moneda == cboTipocampo.val() && r.complementaria)[0];
                        // linea = contadorLinea++;
                        cta = cuentasDlls.cta;
                        scta = cuentasDlls.scta;
                        sscta = cuentasDlls.sscta;
                        d = cuentasDlls.digito;
                        cc = item.cc;
                        referencia = referencia;
                        concepto = `${contratoHistorico.folio.replace(/^\D+/g, '')} PARC ${contratoHistorico.parcialidad}/${contratoHistorico.plazo} ${item.noEconomico}`;
                        tm = 2;
                        itm = 51;
                        cargos = 0;
                        abono = - ((item.importeDLLS * tipoCambio) - item.importeDLLS);
                        if (comboEmpresaPoliza.val() == "2") {
                            descripcion = referencia;
                            if (item.areaCuenta.indexOf("-") != -1)
                                areaCuenta = item.areaCuenta;
                            else
                                areaCuenta = '14-1'
                        }
                        else {
                            areaCuenta = '0-0';
                            descripcion = descripcionCplan2;;
                        }

                        if ((cargos + abono) != 0) {
                            linea = contadorLinea++;
                            //fnAddRowPoliza(linea, cta, scta, sscta, d, descripcion, cc, referencia, concepto, itm, tm, cargos, abono, contratoId, parcialidad, areaCuenta);
                            fnAddRowPoliza(
                                linea,
                                cta,
                                scta,
                                sscta,
                                d,
                                contratoHistorico.descripcion + consecutivoParaReferencia - 1,
                                cc, contratoHistorico.descripcion + consecutivoParaReferencia - 1,
                                concepto,
                                itm,
                                tm,
                                cargos,
                                abono,
                                contratoId,
                                parcialidad,
                                areaCuenta
                            );
                        }

                    }
                });

                if (calcularIva) {
                    if (contratoHistorico.monedaContrato == 2) {
                        const cargosC = tblPolizas.DataTable().data().toArray().reduce((acc, item) => acc + item.cargo, 0);
                        const abonoA = tblPolizas.DataTable().data().toArray().reduce((acc, item) => acc + item.abono, 0);

                        cambiaria = Math.abs(cargosC) - Math.abs(abonoA);

                        //  linea = contadorLinea++;
                        cta = cambiaria > 0 ? 4900 : 5900;;
                        scta = 1;
                        sscta = 0;
                        d = 6;

                        referencia = referencia;
                        concepto = `${contratoHistorico.folio.replace(/^\D+/g, '')} PARC ${contratoHistorico.parcialidad}/${contratoHistorico.plazo}`;
                        tm = cambiaria > 0 ? 2 : 1;
                        itm = 51;
                        cargos = cambiaria < 0 ? Math.abs(cambiaria) : 0;
                        abono = cambiaria > 0 ? -Math.abs(cambiaria) : 0;
                        if (comboEmpresaPoliza.val() == "2") {
                            areaCuenta = '16-2'
                            descripcion = referencia;
                            cc = 191;
                        }
                        else {
                            areaCuenta = '0-0';
                            descripcion = descripcionCplan2;
                            cc = cc;
                        }

                        if (!calcularIva) {
                            cc = _ccMaquina;
                        }

                        if ((cargos + abono) != 0) {
                            linea = contadorLinea++;
                            //fnAddRowPoliza(linea, cta, scta, sscta, d, descripcion, cc, referencia, concepto, itm, tm, cargos, abono, contratoId, parcialidad, areaCuenta);
                            fnAddRowPoliza(
                                linea,
                                cta,
                                scta,
                                sscta,
                                d,
                                comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : _refPrimerasLineas != "" ? _refPrimerasLineas : contratoHistorico.descripcion,
                                cc,
                                comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : _refPrimerasLineas != "" ? _refPrimerasLineas : contratoHistorico.descripcion,
                                concepto,
                                itm,
                                tm,
                                cargos,
                                abono,
                                contratoId,
                                parcialidad,
                                areaCuenta
                            );
                        }
                    }
                }

                //***OPCIONES A COMPRA**/
                let opcionesCompra = dataProgramacion.filter(opcionComprar => opcionComprar.opcionCompra);

                opcionesCompra.forEach(item => {
                    let areaCuentaActivoFijo = '';

                    if (comboEmpresaPoliza.val() == "2") {
                        if (item.areaCuenta.indexOf("-") != -1)
                            areaCuentaActivoFijo = item.areaCuenta;
                        else
                            areaCuentaActivoFijo = '14-1'
                    }
                    else {
                        areaCuentaActivoFijo = '0-0';
                    }

                    let linea = contadorLinea++;
                    let cta = 0;
                    let scta = 0;
                    let sscta = 0;
                    let d = 0;
                    let descripcion = comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : item.noEconomico;
                    let cc = item.cc;
                    let referencia = comboEmpresaPoliza.val() == 2 ? contratoHistorico.descripcion : item.noEconomico;
                    let concepto = `Opción a compra ${item.noEconomico}`;
                    let itm = 0;
                    let tm = 1;
                    let cargos = item.montoOpcionCompra;
                    let abonos = 0;

                    fnAddRowPoliza(
                        linea,
                        cta,
                        scta,
                        sscta,
                        d,
                        descripcion,
                        cc,
                        referencia,
                        concepto,
                        itm,
                        tm,
                        cargos,
                        abonos,
                        contratoId,
                        parcialidad,
                        areaCuentaActivoFijo,
                        false,
                        true
                    );
                });
                // 
                dtPolizas.draw();
                dtPagosProgramados.draw('page');
            }
            else
                AlertaGeneral('Alerta', 'No se encontro tipo de cambio.');
        }

        function fnAddRowPoliza(linea, cta, scta, sscta, d, descripcion, cc, referencia, concepto, itm, tm, cargos, abono, contratoID, parcialidad, areaCuenta, liquidar, opcionCompra) {
            dtPolizas.row.add({
                linea: linea,
                cta: cta,
                scta: scta,
                sscta: sscta,
                digito: d,
                descripcion: descripcion,
                cc: cc,
                areaCuenta: areaCuenta,
                referencia: referencia,
                concepto: concepto,
                tm: tm,
                itm: itm,
                cargo: cargos,
                abono: abono,
                contratoPolizaID: contratoID,
                parcialidad: parcialidad,
                monto: cargos + abono,
                area: areaCuenta.split('-')[0],
                cuenta_oc: areaCuenta.split('-')[1],
                liquidar: liquidar ? liquidar : false,
                opcionCompra: opcionCompra ? opcionCompra : false
            });
        }

        function initTablaProgramacionPagosPoliza() {
            dtPagosProgramados = tblPagosProgramados.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: false,
                columns: [
                    { data: 'folioContrato', title: 'Folio Contrato' },
                    {
                        data: 'parcialidad', title: 'Parcialidad',
                        render: (data, type, row) => {

                            return `${row.parcialidad}/${row.plazos}`;
                        }
                    },
                    { data: 'fechaVencimiento', title: 'Fecha Vencimiento' },
                    {
                        data: 'importeProgramado', title: 'Importe', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'tipoCambio', title: 'Moneda', render: (data, type, row) => {
                            if (data <= 1)
                                return 'Pesos Mexicanos';
                            else
                                return 'Dolares USA';
                        }
                    },
                    {
                        data: 'aplicaPago', render: (data, type, row) => {
                            return `<button type="button" class="btn btn-primary btnAddPago" ><i class="far fa-check-circle"></i></button> `
                        }
                    },
                    {
                        data: null, render: (data, type, row) => {
                            return `<button type="button" class="btn btn-primary btnMostrarDetalle" ><i class="fas fa-info-circle"></i></button> `
                        }
                    }
                ],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    // Total over this page
                    pageTotalAbono = api
                        .column(3, { page: 'current' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Update footer
                    $(api.column(3).footer()).html(
                        maskNumero(pageTotalAbono)
                    );


                },
                rowCallback: function (row, data, index) {
                    if (dtPolizas.data().toArray().some(elem => elem.contratoPolizaID === data.contratoID && elem.parcialidad == data.parcialidad)) {
                        $(row).hide();
                    }
                    else {
                        $(row).show();
                    }
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
            });
        }

        function initTablaDetallePoliza() {
            dtPolizas = tblPolizas.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                "autoWidth": false,
                columns: [
                    { data: 'linea', title: 'Ln', "width": "15px" },
                    {
                        data: 'cta', title: 'Cuenta', "width": "60px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let selector = '';
                            selector = fnSetInputCtas('inputcta', '1', cellData, row);
                            $(td).append(selector);
                        }
                    },
                    {
                        data: 'scta', title: 'Scta', "width": "60px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let selector = '';
                            selector = fnSetInputCtas('inputscta', '2', cellData, row);
                            $(td).append(selector);
                        }
                    },
                    {
                        data: 'sscta', title: 'Sscta', "width": "60px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let selector = '';
                            selector = fnSetInputCtas('inputsscta', '3', cellData, row);
                            $(td).append(selector);
                        }
                    },
                    {
                        data: 'digito', title: 'D', "width": "60px", createdCell: function (td, cellData, rowData, row, col) {
                            let html = '';
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
                        data: 'descripcion', title: 'Descripcion', "width": "70px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let html = '';
                            html = `<input type='text' class='form-control inputDescripcion' value=${cellData}  >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'cc', title: 'C.C.', "width": "100px", createdCell: function (td, cellData, rowData, row, col) {

                            html = `<select class='form-control cboCC'>
                                    ${getInfo()}
                                 </select>`;
                            $(td).text('');
                            $(td).append(html);
                            $(td).parent().children().find('.cboCC').val(cellData);
                        }
                    },
                    {
                        data: 'areaCuenta', title: 'AC', "width": "70px"

                    },
                    { data: 'concepto', "width": "50px", title: 'concepto' },
                    { data: 'tm', title: 'TM', "width": "20px" },
                    { data: 'itm', title: 'TM. interfase', "width": "80px" },
                    {
                        data: 'cargo', title: 'Cargo', "width": "70px", createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let html = '';
                            html = `<input type='text' class='form-control inputCargos' value=${maskNumero(cellData)}  >`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'abono', title: 'Abono', "width": "70px", createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            let html = '';
                            html = `<input type='text' class='form-control inputAbonos' value=${maskNumero(cellData)}  >`;
                            $(td).append(html);
                        }

                    },
                    {
                        data: 'contratoPolizaID', "width": "15px", render: (data, type, row) => {
                            return `<button type="button" class="btn btn-sm btn-primary btnEliminarLineas"><i class="far fa-times-circle"></i></button> `
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    {
                        "targets": [7],
                        "visible": comboEmpresaPoliza.val() == "1" ? false : true
                    }
                ],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    // Total over this page
                    pageTotalAbono = api
                        .column(11, { page: 'current' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Update footer
                    $(api.column(11).footer()).html(
                        maskNumero(pageTotalAbono)
                    );

                    pageTotalCargo = api
                        .column(12, { page: 'current' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Update footer
                    $(api.column(12).footer()).html(
                        maskNumero(pageTotalCargo)
                    );
                }
            });
        }

        function getInfo() {
            let option = '';

            _comboACGlobal.forEach(data => {
                option += `<option data-area='${data.area}' data-cuenta='${data.cuenta}' value="${data.cc}" >${data.cc}-${data.descripcion}</option>`;
            });
            return option;
        }

        function iniTableDetalleProgramacionPagos() {
            dtDetalleProgramacionPagos = tableDetalleProgramacionPagos.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                columns: [
                    { data: 'folio', title: 'No. Contrato' },
                    { data: 'noEconomico', title: 'No.Economico' },
                    { data: 'ac', title: 'Area Cuenta' },
                    { data: 'financiamiento', title: 'Financiamiento' },
                    {
                        data: 'fechaVencimiento', title: 'Fecha Vencimiento',
                        render: (data, type, row) => {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        data: 'capital', title: 'Capital', "width": "100px",
                        render: (data, type, row) => {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'intereses', title: 'Intereses', "width": "100px",
                        render: (data, type, row) => {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'iva', title: 'IVA', "width": "100px",
                        render: (data, type, row) => {
                            let disabled = cboEstatus.val() == "0" ? '' : 'disabled';
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'importe', title: 'Importe', "width": "100px",
                        render: (data, type, row) => {
                            if (data != 0)
                                return maskNumero(data);
                            else
                                return '';
                        }
                    },
                    {
                        data: 'porcentaje', title: '%', "width": "40px",
                        render: (data, type, row) => {
                            return data + '%';
                        }
                    },
                    {
                        data: 'importeDLLS', title: 'Importe DLLS', "width": "100px",
                        render: (data, type, row) => {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'tipoCambio', title: 'Tipo De Cambio', "width": "50px",
                        render: (data, type, row) => {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'total', title: 'Total', "width": "100px",
                        render: (data, type, row) => {
                            return maskNumero(row.total);
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    // Total over this page
                    pageTotal = api
                        .column(5, { page: 'current' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    // Update footer
                    $(api.column(5).footer()).html(
                        maskNumero(pageTotal)
                    );
                }
            });
        }

        function fnSetInputCtas(clase, tipo, valueData, row) {
            let html = '';
            html += `<div style="display: inline-flex;">`
            html += `<input type='text' class='form-control ${clase}' value='${valueData}' data-row='${row}' >`;
            html += `<button class="btn btn-info inputBuscarCuentas" data-tipo='${tipo}'>
                        <i class="fas fa-search"></i>
                    </button> </div>`;
            return html;
        }

        tblPolizas.on('change', '.inputCargos', function () {
            let poliza = dtPolizas.row($(this).closest('tr')).data();
            poliza.cargo = unmaskNumero($(this).val());
            poliza.monto = poliza.cargo;
            // dtPolizas.row($(this).closest('tr')).data().cargo = poliza.cargo;
            // dtPolizas.row($(this).closest('tr')).data().monto = poliza.cargo;
            let polizasDTO = dtPolizas.data().toArray();
            dtPolizas.clear();
            dtPolizas.rows.add(polizasDTO).draw();
        });

        tblPolizas.on('change', '.inputAbonos', function () {
            let poliza = dtPolizas.row($(this).closest('tr')).data();
            poliza.abono = unmaskNumero($(this).val());
            poliza.monto = poliza.abono;
            // dtPolizas.row($(this).closest('tr')).data().abono = poliza.abono;
            // dtPolizas.row($(this).closest('tr')).data().monto = poliza.abono;
            let polizasDTO = dtPolizas.data().toArray();
            dtPolizas.clear();
            dtPolizas.rows.add(polizasDTO).draw();
        });

        tblPolizas.on('click', '.btnEliminarLineas', function () {
            let dataLista = dtPolizas.data().toArray();
            let filtro = dtPolizas.row($(this).parents('tr')).data();
            let linea = 1;
            let newLista = dataLista.filter(linea => linea.contratoPolizaID != filtro.contratoPolizaID).map(r => {
                r.linea = linea;
                linea++;
                return r;
            });
            dtPolizas.clear();
            dtPolizas.rows.add(newLista).draw();
            dtPagosProgramados.clear();
            dtPagosProgramados.rows.add(_dtPagosProgramados).draw();
        });

        tblPolizas.on('click', '.inputBuscarCuentas', function () {
            _currentButton = $(this);
            fnBuscarCuentasBanco();
        });

        tblPolizas.on('change', '.inputcta', function () {
            let poliza = dtPolizas.row($(this).closest('tr')).data();
            poliza.cta = $(this).val();
            let polizasDTO = dtPolizas.data().toArray();
            dtPolizas.clear();
            dtPolizas.rows.add(polizasDTO).draw();
        });

        tblPolizas.on('change', '.inputscta', function () {
            let poliza = dtPolizas.row($(this).closest('tr')).data();
            poliza.scta = $(this).val();
            let polizasDTO = dtPolizas.data().toArray();
            dtPolizas.clear();
            dtPolizas.rows.add(polizasDTO).draw();
        });

        tblPolizas.on('change', '.inputsscta', function () {
            let poliza = dtPolizas.row($(this).closest('tr')).data();
            poliza.sscta = $(this).val();
            let polizasDTO = dtPolizas.data().toArray();
            dtPolizas.clear();
            dtPolizas.rows.add(polizasDTO).draw();
        });

        tblPolizas.on('change', '.inputDigito', function () {
            let poliza = dtPolizas.row($(this).closest('tr')).data();
            poliza.digito = $(this).val();
            let polizasDTO = dtPolizas.data().toArray();
            dtPolizas.clear().draw();
            dtPolizas.rows.add(polizasDTO).draw();
        });

        //inputDigito

        tblPolizas.on('change', '.inputDescripcion', function () {
            let poliza = dtPolizas.row($(this).closest('tr')).data();
            poliza.descripcion = $(this).val();
            poliza.referencia = poliza.descripcion;
            let polizasDTO = dtPolizas.data().toArray();
            dtPolizas.clear().draw();
            dtPolizas.rows.add(polizasDTO).draw();
        });

        tblPolizas.on('change', '.cboCC', function () {
            let poliza = dtPolizas.row($(this).closest('tr')).data();
            poliza.cc = $(this).val();
            poliza.areaCuenta = $(this).find('option:selected').attr('data-area') + "-" + $(this).find('option:selected').attr('data-cuenta');
            poliza.area = $(this).find('option:selected').attr('data-area');
            poliza.cuenta_oc = $(this).find('option:selected').attr('data-cuenta');
            let polizasDTO = dtPolizas.data().toArray();
            dtPolizas.clear().draw();
            dtPolizas.rows.add(polizasDTO).draw();
        });

        //#endregion

        //#region interfaz principal.
        function fnCargarTablaPpal() {
            $.blockUI({ message: 'Espere Porfavor Cargando Información' });
            if (inputFechaInicio.val() != "") {
                $.get('/Contratos/CargarPropuesta',
                    {
                        pInicio: moment(inputFechaPoliza.val(), 'DD/MM/YYYY').toISOString(true),
                        pFinal: moment(inputFechaFin.val(), 'DD/MM/YYYY').toISOString(true),
                        pEstatus: cboEstatus.val(),
                        institucion: cboInstitucion.val() == "" ? 0 : cboInstitucion.val()
                    }).always($.unblockUI).then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            dtPpalProgramacion.clear().draw();
                            dtPpalProgramacion.rows.add(response.result).draw();
                            // sumTotales();
                            if (cboEstatus == 2)
                                btnPoliza.prop('disabled');
                        } else {
                            // Operación no completada.onvertToMultiselect
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    });
            }
        }

        function fnGuardarPagos() {
            let data = [];
            dtPpalProgramacion.data().toArray().forEach((pago, index) => {
                parPagos = {};
                if (pago.aplicaPago == 1) {
                    parPagos.id = 0;
                    parPagos.contratoid = pago.contratoID;
                    parPagos.folioContrato = pago.folioContrato;
                    parPagos.parcialidad = pago.parcialidad;
                    parPagos.fechaVencimiento = pago.fechaVencimiento;
                    parPagos.fechaPago = '01/07/2017';
                    parPagos.montoProgramado = pago.importeProgramado;
                    parPagos.montoFinal = pago.importeFinal == 0 ? pago.importeProgramado : parseFloat(pago.importeFinal);
                    parPagos.tipoCambio = pago.tipoCambio;
                    parPagos.estatus = 1;
                    parPagos.usuarioCatpura = 13;
                    parPagos.fechacaptura = '01/07/2017';
                    data.push(parPagos);
                }
            });

            if (data.length > 0) {
                $.post('/Contratos/GuardarProgramacion', { obj: data })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            AlertaGeneral('Confirmacion', `La operación se completo con exito`);
                            fnCargarTablaPpal();
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
            else {
                AlertaGeneral(`Alerta, Revise qur toda la informacion este capturada correctamente.`)
            }
        }

        tblPpalProgramacion.on('change', '.inputImporteFinal', function () {
            let rowData = dtPpalProgramacion.row($(this).closest('tr')).data();
            rowData.importeFinal = +$(this).val();
            dtPpalProgramacion.draw(true);
        });


        function initTablaPPagos() {
            dtPpalProgramacion = tblPpalProgramacion.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                scrollY: '45vh',
                scrollCollapse: true,
                scrollX: true,
                columns: [
                    { data: 'folioContrato', title: 'Folio Contrato' },
                    { data: 'parcialidad', title: 'Parcialidad' },
                    { data: 'fechaVencimiento', title: 'Fecha Vencimiento' },
                    {
                        data: 'importeProgramado', title: 'Importe Programado',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text(cellData.toFixed(2));
                        }
                    },
                    {
                        data: 'importeFinal', title: 'Importe Final',
                        render: (data, type, row) => {
                            let monto = 0
                            if (data == 0) {
                                monto = row.importeProgramado;
                            }
                            else {
                                monto = row.importeFinal;
                            }
                            if (row.aplicaPago == 1)
                                return `<input type='text' class='form-control inputImporteFinal' value='${monto.toFixed(2)}'/>`;
                            else
                                return `<input type='text' class='form-control inputImporteFinal' value='${monto.toFixed(2)}' disabled/>`;
                        }
                    },
                    {
                        data: 'tipoCambio', title: 'T.C.', render: (data, type, row) => {
                            if (data <= 1)
                                return 'Pesos Mexicanos';
                            else
                                return 'Dolares USA';
                        }
                    },
                    {
                        data: 'estatus', title: 'Estatus', createdCell: function (td, cellData, rowData, row, col) {
                            let texto = '';
                            switch (cellData) {
                                case 1:
                                    texto = "PROGRAMADO";
                                    break;
                                case 0:
                                    texto = "PENDIENTE PAGO";
                                    break;
                                case 2:
                                    texto = "APLICADO";
                                    break;
                                default:
                                    texto = "PROGRAMADO";
                                    break; 1
                            }
                            $(td).text(texto);
                        }
                    },
                    {
                        data: 'aplicaPago', title: 'Aplica Pago', render: (data, type, row) => {
                            if (data == 1) {
                                let checked = 'checked';
                                return `<input type='checkbox' class='form-control inputAplicaPago' ${checked} />`
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                "preDrawCallback": function (settings) {
                    pageScrollPos = $('div.dataTables_scrollBody').scrollTop();
                },
                "drawCallback": function (settings) {
                    $('div.dataTables_scrollBody').scrollTop(pageScrollPos);
                }, "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    // Total over this page
                    pageTotal = api.data().filter(function (r) { return r.aplicaPago == 1 }).toArray().reduce(function (a, b) {
                        return a + b.importeFinal;
                    }, 0);
                    // Update footer
                    $(api.column(4).footer()).html(
                        "Total: " + pageTotal.toFixed(2)
                    );
                }
            });
        }

        function fnCambiarEstatus() {
            let estatus = $(this).val();
            switch (estatus) {
                case "0":
                    inputFechaInicio.prop('disabled', true);
                    btnReporteProgramado.prop('disabled', true);
                    break;
                case "1":
                    inputFechaInicio.prop('disabled', false);
                    btnReporteProgramado.prop('disabled', false);
                    break;
                case "2":
                    inputFechaInicio.prop('disabled', false);
                    btnReporteProgramado.prop('disabled', true);
                    break;
                default:
                    break;
            }
        }
        //#endregion

        //#region Modal Ctas 
        /*  function initTablaCtas() {
              /*dtCtas = tablaCtas.DataTable({
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
                  ]
              });
        } */

        tablaCtas.on('click', '.seleccionar', function () {
            let rowData = dtCtas.row($(this).closest('tr')).data();
            dtPolizas.row($(_currentButton).closest('tr')).data().cta = rowData.cta;
            dtPolizas.row($(_currentButton).closest('tr')).data().scta = rowData.scta;
            dtPolizas.row($(_currentButton).closest('tr')).data().sscta = rowData.sscta;
            $(_currentButton).closest('tr').children().find('.inputcta').val(rowData.cta);
            $(_currentButton).closest('tr').children().find('.inputscta').val(rowData.scta);
            $(_currentButton).closest('tr').children().find('.inputsscta').val(rowData.sscta);
            $(_currentButton).closest('tr').children().find('.inputDigito').val(rowData.digito);
            modalGeneral.modal('hide');
        });

        function fnBuscarCuentasBanco() {
            modalGeneral.modal('show');
            // dtCtas.clear().draw();
            // initTablaCtas();
            //   cargarTablaCtas('/administrativo/cheque/GetListaCuentasInit');
        }

        function cargarTablaCtas(url) {
            try {
                $('#divmodal').block({
                    message: 'Cargando información de las cuentas.'
                });
                $.get(url)
                    .then(response => {
                        if (response.success) {
                            $('#divmodal').unblock();
                            if (dtCtas != null) {
                                dtCtas.clear().draw();
                                dtCtas.rows.add(response.listaCta).draw();
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

        //#endregion

        //#region Reporte de Programacion Pagos.
        function fnLoadReporte() {
            var idReporte = "195";
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pFechaInicio=" + inputFechaInicio.val() + "&pFechaFin=" + inputFechaFin.val();
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        //#endregion

        //#region Metodos de Propuesta Pago
        function fnLoadProgramacionPagos() {
            $.post('/Contratos/LoadProgramacionPagos', {
                pInicio: inputFechaInicio.val(),
                pFinal: inputFechaFin.val(),
                // pInicio: moment(inputFechaInicio.val()).format("DD/MM/YYYY"),
                // pFinal: moment(inputFechaFin.val()).format("DD/MM/YYYY"),
                pEstatus: cboEstatus.val(),
                institucion: cboInstitucion.val(),
                empresa: comboEmpresa.val(),
                moneda: inpuntMoneda.val()
            }).then(response => {
                if (response.success) {
                    // Operación exitosa.
                    dtTablaProgramacion.clear().draw();
                    dtTablaGenPropuesta.clear().draw();
                    if (response.contratoLista.length > 0) {
                        if (cboEstatus.val() == "0") {
                            divTablaGenPropuesta.removeClass('hide');
                            divTablaProgramacion.addClass('hide');
                            dtTablaGenPropuesta.rows.add(response.contratoLista).draw();
                            if (tipoCambioTabla.val() != "") {
                                tipoCambioTabla.change();
                            }
                        }
                        else {
                            divTablaGenPropuesta.addClass('hide');
                            divTablaProgramacion.removeClass('hide');
                            dtTablaProgramacion.rows.add(response.contratoLista).draw();
                        }
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

        function initTablaPropuesta() {
            var groupColumn = 0;
            dtTablaProgramacion = tablaProgramacion.DataTable({
                paging: false
                , destroy: true
                , ordering: false
                , language: dtDicEsp
                , scrollY: '52vh',
                scrollCollapse: true,
                scrollX: true
                , "bLengthChange": false
                , "searching": false
                , "bFilter": true
                , "bInfo": true
                , columns: [
                    // { data: 'rfc', title: 'rfc' },
                    { data: 'noEconomico', title: 'No Eco' },
                    { data: 'mensualidad', title: 'Par', width: "25px" },
                    { data: 'financiamiento', title: 'Financiera' },
                    {
                        data: 'fechaVencimiento', title: 'Vencimiento',
                        render: (data, type, row) => {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    { data: 'contrato', title: 'Contrato' },
                    {
                        data: 'capital', title: 'Capital', width: "65px",
                        render: (data, type, row) => {
                            let disabled = cboEstatus.val() == "0" ? '' : 'disabled';
                            return `<input type='text' class='form-control inputCapital inputFontS input-sm' value='${maskNumero(data)}' ${disabled}/>`;
                        }
                    },
                    {
                        data: 'ivaInteres', title: 'iva Int.s', width: "65px",
                        render: (data, type, row) => {
                            let disabled = cboEstatus.val() == "0" ? '' : 'disabled';
                            return `<input type='text' class='form-control inputIvaIntereses inputFontS input-sm' value='${maskNumero(data)}' ${disabled}/>`;
                        }
                    },
                    {
                        data: 'intereses', title: 'Intereses', width: "65px",
                        render: (data, type, row) => {
                            let disabled = cboEstatus.val() == "0" ? '' : 'disabled';
                            return `<input type='text' class='form-control inputIntereses inputFontS input-sm' value='${maskNumero(data)}' ${disabled}/>`;
                        }
                    },
                    {
                        data: 'iva', title: 'IVA', width: "65px",
                        render: (data, type, row) => {
                            let disabled = cboEstatus.val() == "0" ? '' : 'disabled';
                            return `<input type='text' class='form-control inputIVA inputFontS input-sm' value='${maskNumero(data)}' ${disabled}/>`;
                        }
                    },
                    {
                        data: 'importe', title: 'Importe',
                        render: (data, type, row) => {
                            if (data != 0)
                                return maskNumero(data);
                            else
                                return '';
                        }
                    },
                    {
                        data: 'importeDLLS', title: 'Imp. DLLS',
                        render: (data, type, row) => {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'tipoCambio', title: 'T.C.',
                        render: (data, type, row) => {
                            return maskNumeroXD(data, 4);
                        }
                    },
                    {
                        data: 'total', title: 'Total',
                        render: (data, type, row) => {
                            return maskNumero((row.total));
                        }
                    }, {
                        data: 'programado', title: 'Programar', render: (data, type, row) => {
                            if (cboEstatus.val() == 0) {
                                if (data == "1") {
                                    let checked = 'checked';
                                    return `<input type='checkbox' class='form-control checkProgramar' ${checked} />`;
                                }
                                else {
                                    return `<input type='checkbox' class='form-control checkProgramar'  />`;
                                }
                            }
                            else {
                                return '';
                            }
                        }
                    }
                ],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    // Total over this page
                    totalDLLS = api.data().toArray().reduce((acc, c) => acc + c.importeDLLS, 0);

                    // Update footer
                    $(api.column(10).footer()).html(
                        maskNumero(totalDLLS)
                    );

                    total = api.data().toArray().reduce((acc, c) => acc + c.total, 0);

                    // Update footer
                    $(api.column(12).footer()).html(
                        maskNumero(total)
                    );
                }
            });
        }

        function initTablaGenPropuesta() {
            dtTablaGenPropuesta = tablaGenPropuesta.DataTable({
                paging: false
                , destroy: true
                , ordering: false
                , language: dtDicEsp
                , scrollY: '52vh',
                scrollCollapse: true,
                scrollX: true
                , "bLengthChange": false
                , "searching": false
                , "bFilter": true
                , "bInfo": true,
                columns: [
                    { data: 'noEconomico', title: 'No Eco' },
                    { data: 'cc', title: 'C.C.' },
                    { data: 'mensualidad', title: 'Par', width: "25px" },
                    { data: 'financiamiento', title: 'Financiamiento' },
                    {
                        data: 'fechaVencimiento', title: 'Vencimiento',
                        render: (data, type, row) => {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    { data: 'contrato', title: 'No. Contrato' },
                    { data: 'areaCuenta', title: 'A.C' },
                    {
                        data: 'capital', title: 'Capital', width: "65px",
                        render: (data, type, row) => {
                            let disabled = cboEstatus.val() == "0" ? '' : 'disabled';
                            return `<input type='text' class='form-control inputCapital inputFontS input-sm' value='${maskNumero(data)}' ${disabled}/>`;
                        }
                    },
                    {
                        data: 'iva', title: 'IVA', width: "65px",
                        render: (data, type, row) => {
                            let disabled = cboEstatus.val() == "0" ? '' : 'disabled';
                            return `<input type='text' class='form-control inputIVA inputFontS input-sm' value='${maskNumero(data)}' ${disabled}/>`;
                        }
                    },
                    {
                        data: 'intereses', title: 'Intereses', width: "65px",
                        render: (data, type, row) => {
                            let disabled = cboEstatus.val() == "0" ? '' : 'disabled';
                            return `<input type='text' class='form-control inputIntereses inputFontS input-sm' value='${maskNumero(data)}' ${disabled}/>`;
                        }
                    },
                    {
                        data: 'ivaInteres', title: 'iva Int.s', width: "65px",
                        render: (data, type, row) => {
                            let disabled = cboEstatus.val() == "0" ? '' : 'disabled';
                            return `<input type='text' class='form-control inputIvaIntereses inputFontS input-sm' value='${maskNumero(data)}' ${disabled}/>`;
                        }
                    },
                    {
                        data: 'importe', title: 'Importe',
                        render: (data, type, row) => {
                            if (data != 0)
                                return maskNumero(data);
                            else
                                return '';
                        },
                        className: 'colImporte'
                    },
                    {
                        data: 'porcentaje', title: '%',
                        render: (data, type, row) => {
                            return `<input type='text' class='form-control inputPorcentaje inputFontS input-sm' value='${data}' />`;
                        }
                    },
                    {
                        data: 'importeDLLS', title: 'Imp. DLLS',
                        render: (data, type, row) => {
                            // if (tipoCambioTabla.val()) {
                            //     return maskNumero((data * (row.porcentaje / 100)) * (+tipoCambioTabla.val()));
                            // } else {
                            //     return maskNumero((data * (row.porcentaje / 100)) * row.tipoCambio);
                            // }
                            return maskNumero(data);
                        },
                        className: 'colImporteDLLS'
                    },
                    {
                        data: 'tipoCambio', title: 'T.C.',
                        render: (data, type, row) => {
                            if (inpuntMoneda.val() == 2 && tipoCambioTabla.val() > 0) {
                                let valor = (+tipoCambioTabla.val());
                                return `<input type='text' class='form-control inputTipoCambio inputFontS input-sm' value='${valor}' />`;
                            }
                            else {
                                return `<input type='text' class='form-control inputTipoCambio inputFontS input-sm' value='${data}' />`;
                            }

                        }
                    },
                    {
                        data: 'total', title: 'Total',
                        render: (data, type, row) => {
                            if (tipoCambioTabla.val()) {
                                return maskNumero(((row.importe + row.penaConvencional + row.montoOpcionCompra) * (+tipoCambioTabla.val()) /** (row.porcentaje / 100)*/));
                            } else {
                                return maskNumero(((row.importe + row.penaConvencional + row.montoOpcionCompra) * row.tipoCambio /** (row.porcentaje / 100)*/));
                            }
                        },
                        className: 'colTotal'
                    }, {
                        data: 'programado', title: 'Programar', render: (data, type, row) => {
                            if (cboEstatus.val() == 0) {
                                if (data == "1") {
                                    let checked = 'checked';
                                    return `<input type='checkbox' class='form-control checkProgramar' ${checked} />`;
                                }
                                else {
                                    return `<input type='checkbox' class='form-control checkProgramar'  />`;
                                }
                            }
                            else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'liquidar', title: 'Liquidar',
                        render: function (data, type, row) {
                            var chkLiquidar = `<input type="checkbox" class="cboxLiquidar form-control checkButton" ${data ? 'checked' : ''} />`;
                            return chkLiquidar;
                        },
                        className: 'text-center'
                    },
                    {
                        data: 'penaConvencional',
                        title: 'Pena convencional',
                        render: function (data, type, row) {
                            return `<input type="text" class="form-control inputPenaConvencional inputFontS input-sm" ${row.liquidar ? '' : 'disabled'} value="${row.liquidar ? maskNumero(data) : ''}" />`;
                        }
                    },
                    {
                        data: 'opcionCompra', title: 'Opción compra',
                        render: function (data, type, row) {
                            var chkOpcionCompra = `<input type="checkbox" class="cboxOpcionCompra form-control checkButton" ${data ? 'checked' : ''} ${data ? '' : 'disabled'} />`;
                            return chkOpcionCompra;
                        }
                    },
                    {
                        data: 'montoOpcionCompra',
                        title: 'Monto opción compra',
                        render: function (data, type, row) {
                            return `<input type="text" class="form-control inputOpcionCompra inputFontS input-sm" ${row.opcionCompra ? '' : 'disabled'} value="${row.opcionCompra ? maskNumero(data) : ''}" />`;
                        }
                    }
                ],
                "footerCallback": function (row, data, start, end, display) {
                    // var api = this.api(), data;
                    // // Remove the formatting to get integer data for summation
                    // var intVal = function (i) {
                    //     return typeof i === 'string' ?
                    //         i.replace(/[\$,]/g, '') * 1 :
                    //         typeof i === 'number' ?
                    //             i : 0;
                    // };
                    // // Total over this page
                    // capital = api.data().toArray().filter(contrato => contrato.programado == 1).reduce((acc, c) => acc + c.capital, 0);
                    // intereses = api.data().toArray().filter(contrato => contrato.programado == 1).reduce((acc, c) => acc + c.intereses, 0);
                    // iva = api.data().toArray().filter(contrato => contrato.programado == 1).reduce((acc, c) => acc + c.iva, 0);
                    // ivaInteres = api.data().toArray().filter(contrato => contrato.programado == 1).reduce((acc, c) => acc + c.ivaInteres, 0);
                    // importe = api.data().toArray().filter(contrato => contrato.programado == 1).reduce((acc, c) => acc + c.importe, 0);
                    // // totalDLLS = api.data().toArray().filter(contrato => contrato.programado == 1).reduce((acc, c) => acc + c.importeDLLS, 0);
                    // total = api.data().toArray().filter(contrato => contrato.programado == 1).reduce((acc, c) => acc + c.total, 0);

                    // $(api.column(7).footer()).html(maskNumero(capital));
                    // $(api.column(8).footer()).html(maskNumero(iva));
                    // $(api.column(9).footer()).html(maskNumero(intereses));
                    // $(api.column(10).footer()).html(maskNumero(ivaInteres));
                    // $(api.column(11).footer()).html(maskNumero(importe));
                    // // $(api.column(13).footer()).html(maskNumero(totalDLLS));
                    // $(api.column(15).footer()).html(maskNumero(total));
                },
                initComplete: function () {
                    tablaGenPropuesta.on('change', '.cboxLiquidar', function () {
                        let row = $(this).closest('tr');

                        let rowData = tablaGenPropuesta.DataTable().row(row).data();

                        let checked = $(this).prop('checked');

                        getInfoLiquidar(checked, rowData.contratoid, rowData.parcialidad).done(function (response) {
                            if (response && response.success) {
                                let info = response.items;

                                tablaGenPropuesta.DataTable().row(row).data().liquidar = checked;

                                tablaGenPropuesta.DataTable().row(row).data().capital = info.capital;
                                tablaGenPropuesta.DataTable().row(row).data().iva = info.iva;
                                tablaGenPropuesta.DataTable().row(row).data().intereses = info.intereses;
                                tablaGenPropuesta.DataTable().row(row).data().ivaInteres = info.ivaInteres;
                                tablaGenPropuesta.DataTable().row(row).data().importe = info.importe;
                                tablaGenPropuesta.DataTable().row(row).data().importeDLLS = info.importeDLLS;
                                tablaGenPropuesta.DataTable().row(row).data().total = (info.importe * rowData.tipoCambio * (rowData.porcentaje / 100));
                                tablaGenPropuesta.DataTable().row(row).data().penaConvencional = 0;
                                tablaGenPropuesta.DataTable().row(row).data().montoOpcionCompra = 0;

                                row.find('.inputCapital').val(maskNumero(info.capital));
                                row.find('.inputIVA').val(maskNumero(info.iva));
                                row.find('.inputIntereses').val(maskNumero(info.intereses));
                                row.find('.inputIvaIntereses').val(maskNumero(info.ivaInteres));
                                row.find('.colImporte').text(maskNumero(info.importe));
                                row.find('.colImporteDLLS').text(maskNumero(info.importeDLLS));
                                row.find('.colTotal').text(maskNumero(rowData.total));

                                if (checked) {
                                    row.find('.cboxOpcionCompra').prop('disabled', false);
                                    row.find('.inputPenaConvencional').prop('disabled', false);
                                } else {
                                    tablaGenPropuesta.DataTable().row(row).opcionCompra = false;

                                    row.find('.cboxOpcionCompra').prop('disabled', true);
                                    row.find('.cboxOpcionCompra').prop('checked', false);
                                    row.find('.inputOpcionCompra').val(null);
                                    row.find('.inputOpcionCompra').prop('disabled', true);
                                    row.find('.inputPenaConvencional').prop('disabled', true);
                                    row.find('.inputPenaConvencional').val(null);
                                }

                                actualizarFooterGenPropuesta();
                            }
                        });
                    });

                    tablaGenPropuesta.on('change', '.inputPenaConvencional', function () {
                        let row = $(this).closest('tr');

                        let rowData = tablaGenPropuesta.DataTable().row(row).data();

                        let montoPenaConvencional = unmaskNumero($(this).val());
                        $(this).val(maskNumero(montoPenaConvencional));

                        rowData.penaConvencional = montoPenaConvencional;
                        rowData.total = ((rowData.importe * (rowData.porcentaje / 100)) + montoPenaConvencional + rowData.montoOpcionCompra) * rowData.tipoCambio;

                        row.find('.colTotal').text(maskNumero(rowData.total));

                        actualizarFooterGenPropuesta();
                    });

                    tablaGenPropuesta.on('change', '.cboxOpcionCompra', function () {
                        let row = $(this).closest('tr');

                        tablaGenPropuesta.DataTable().row(row).data().montoOpcionCompra = 0;

                        let checked = $(this).prop('checked');

                        tablaGenPropuesta.DataTable().row(row).data().opcionCompra = checked;

                        if (checked) {
                            row.find('.inputOpcionCompra').prop('disabled', false);
                        } else {
                            row.find('.inputOpcionCompra').val(null);
                            row.find('.inputOpcionCompra').prop('disabled', true);
                        }

                        actualizarFooterGenPropuesta();
                    });

                    tablaGenPropuesta.on('change', '.inputOpcionCompra', function () {
                        let row = $(this).closest('tr');

                        let rowData = tablaGenPropuesta.DataTable().row(row).data();

                        let montoOpcionCompra = unmaskNumero($(this).val());
                        $(this).val(maskNumero(montoOpcionCompra));

                        rowData.montoOpcionCompra = montoOpcionCompra;
                        rowData.total = ((rowData.importe * (rowData.porcentaje / 100)) + montoOpcionCompra + rowData.penaConvencional) * rowData.tipoCambio;

                        row.find('.colTotal').text(maskNumero(rowData.total));

                        actualizarFooterGenPropuesta();
                    });
                },
                drawCallback: function (settings) {
                    // fncRecalcularFooter();
                    actualizarFooterGenPropuesta();
                }
            });
        }

        function fncRecalcularFooter() {
            // let capital = 0;
            // let IVA = 0;
            // let intereses = 0;
            // let ivaSinIntereses = 0;
            // let importe = 0;
            // let importeDLLS = 0;
            // let total = 0;

            // let arrContratos = new Array();
            // let arrImporte = new Array();
            // let arrTipoCambio = new Array();

            // tablaGenPropuesta.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
            //     // if (colIdx == 16) {
            //     //     console.log(this.data());
            //     // }

            //     switch (colIdx) {
            //         case 5:
            //             for (let x = 0; x < this.data().length; x++) {
            //                 arrContratos.push(this.data()[x]);
            //             }
            //             break;
            //         case 7:
            //             for (let x = 0; x < this.data().length; x++) {
            //                 capital += this.data()[x];
            //             }
            //             $(this.footer()).html(maskNumero(capital));
            //             break;
            //         case 8:
            //             for (let x = 0; x < this.data().length; x++) {
            //                 IVA += this.data()[x];
            //             }
            //             $(this.footer()).html(maskNumero(IVA));
            //             break;
            //         case 9:
            //             for (let x = 0; x < this.data().length; x++) {
            //                 intereses += this.data()[x];
            //             }
            //             $(this.footer()).html(maskNumero(intereses));
            //             break;
            //         case 10:
            //             for (let x = 0; x < this.data().length; x++) {
            //                 ivaSinIntereses += this.data()[x];
            //             }
            //             $(this.footer()).html(maskNumero(ivaSinIntereses));
            //             break;
            //         case 11:
            //             for (let x = 0; x < this.data().length; x++) {
            //                 importe += this.data()[x];
            //             }
            //             $(this.footer()).html(maskNumero(importe));
            //             break;
            //         case 13:
            //             for (let x = 0; x < this.data().length; x++) {
            //                 importeDLLS += this.data()[x];
            //             }
            //             $(this.footer()).html(maskNumero(importeDLLS));
            //             break;
            //         case 15:
            //             for (let x = 0; x < this.data().length; x++) {
            //                 total += this.data()[x];
            //             }
            //             $(this.footer()).html(maskNumero(total));
            //             break;
            //     }

            //     capital = 0;
            //     IVA = 0;
            //     intereses = 0;
            //     ivaSinIntereses = 0;
            //     importe = 0;
            //     importeDLLS = 0;
            //     total = 0;
            //     footer = 0;
            // });

            // tablaGenPropuesta.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
            //     let importeDLLS = 0;
            //     if (colIdx == 13) {

            //         //#region VERIFICA QUE CONTRATO ESTA DUPLICADO
            //         // const numeros = arrContratos;
            //         // const numerosUnicos = [...new Set(arrContratos)]; // Array sin duplicados
            //         // let duplicados = [...numeros]; // Creamos una copia del array original
            //         // numerosUnicos.forEach((numero) => {
            //         //     const indice = duplicados.indexOf(numero);
            //         //     duplicados = duplicados.slice(0, indice).concat(duplicados.slice(indice + 1, duplicados.length));
            //         // });

            //         // duplicados.forEach(element => {
            //         //     let indexContrato = arrContratos.findIndex(w => w == element);

            //         //     arrImporte.splice(indexContrato, 1);
            //         //     arrContratos.splice(indexContrato, 1);
            //         // });
            //         //#endregion

            //         // for (let i = 0; i < arrImporte.length; i++) {
            //         //     let importe = arrImporte[i];
            //         //     let tipoCambio = arrTipoCambio[i];
            //         //     if (tipoCambio > 1) {
            //         //         importeDLLS += importe * tipoCambio;
            //         //     } else {
            //         //         importeDLLS += importe * 1;
            //         //     }
            //         // }

            //         $(this.footer()).html(maskNumero(importeDLLS));
            //     }
            // });
        }

        function actualizarFooterGenPropuesta() {
            let infoTabla = tablaGenPropuesta.DataTable().data().toArray().filter(contrato => contrato.programado);

            let capital = infoTabla.reduce((x, y) => x + y.capital, 0);
            let intereses = infoTabla.reduce((x, y) => x + y.intereses, 0);
            let iva = infoTabla.reduce((x, y) => x + y.iva, 0);
            let ivaInteres = infoTabla.reduce((x, y) => x + y.ivaInteres, 0);
            let importe = infoTabla.reduce((x, y) => x + y.importe, 0);
            let totalDLLS = infoTabla.reduce((x, y) => x + y.importeDLLS, 0);
            let total = infoTabla.reduce((x, y) => x + y.total, 0);

            $($(tablaGenPropuesta).DataTable().column(7).footer()).text(maskNumero(capital));
            $($(tablaGenPropuesta).DataTable().column(8).footer()).text(maskNumero(iva));
            $($(tablaGenPropuesta).DataTable().column(9).footer()).text(maskNumero(intereses));
            $($(tablaGenPropuesta).DataTable().column(10).footer()).text(maskNumero(ivaInteres));
            $($(tablaGenPropuesta).DataTable().column(11).footer()).text(maskNumero(importe));
            $($(tablaGenPropuesta).DataTable().column(13).footer()).text(maskNumero(totalDLLS));
            $($(tablaGenPropuesta).DataTable().column(15).footer()).text(maskNumero(total));

            // fncRecalcularFooter();
        }

        tablaGenPropuesta.on('change', '.inputCapital', function () {
            let rowF = dtTablaGenPropuesta.row($(this).closest('tr')).data();

            dtTablaGenPropuesta.data().toArray().forEach(rowData => {

                if (rowF.contratoid == rowData.contratoid) {
                    rowData.capital = unmaskNumero($(this).val());
                    rowData.importe = (rowData.capital + rowData.intereses + rowData.iva + rowData.ivaInteres) * (rowData.porcentaje / 100);
                    if (rowData.moneda != 1) {
                        // rowData.importeDLLS = rowData.importe * (rowData.porcentaje / 100);
                        rowData.importeDLLS = (rowData.capital + rowData.intereses + rowData.iva + rowData.ivaInteres) * (rowData.porcentaje / 100);

                        // rowData.total = (rowData.importeDLLS * rowData.tipoCambio);
                        rowData.total = ((rowData.importeDLLS + rowData.penaConvencional) * rowData.tipoCambio);
                    }
                    else {
                        // rowData.total = (rowData.importe * (rowData.porcentaje / 100) * rowData.tipoCambio);
                        // rowData.total = (((rowData.importe * (rowData.porcentaje / 100)) + rowData.penaConvencional) * rowData.tipoCambio);
                        rowData.total = (((rowData.importe) + rowData.penaConvencional) * rowData.tipoCambio);
                    }
                }
            });
            let dt = dtTablaGenPropuesta.data().toArray();
            dtTablaGenPropuesta.clear();
            dtTablaGenPropuesta.rows.add(dt).draw(true);
        });
        tablaGenPropuesta.on('change', '.inputIntereses', function () {
            let rowF = dtTablaGenPropuesta.row($(this).closest('tr')).data();
            dtTablaGenPropuesta.data().toArray().forEach(rowData => {
                if (rowF.contratoid == rowData.contratoid) {
                    rowData.intereses = unmaskNumero($(this).val());
                    rowData.importe = (rowData.capital + rowData.intereses + rowData.iva + rowData.ivaInteres) * (rowData.porcentaje / 100);
                    if (rowData.moneda != 1) {
                        // rowData.importeDLLS = rowData.importe * (rowData.porcentaje / 100);
                        rowData.importeDLLS = (rowData.capital + rowData.intereses + rowData.iva + rowData.ivaInteres) * (rowData.porcentaje / 100);

                        // rowData.total = (rowData.importeDLLS * rowData.tipoCambio);
                        rowData.total = ((rowData.importeDLLS + rowData.penaConvencional) * rowData.tipoCambio);
                    }
                    else {
                        // rowData.total = (rowData.importe * (rowData.porcentaje / 100) * rowData.tipoCambio);
                        // rowData.total = (((rowData.importe * (rowData.porcentaje / 100)) + rowData.penaConvencional) * rowData.tipoCambio);
                        rowData.total = (((rowData.importe) + rowData.penaConvencional) * rowData.tipoCambio);
                    }
                }
            });
            let dt = dtTablaGenPropuesta.data().toArray();
            dtTablaGenPropuesta.clear();
            dtTablaGenPropuesta.rows.add(dt).draw(true);
        });
        tablaGenPropuesta.on('change', '.inputIvaIntereses', function () {
            let rowF = dtTablaGenPropuesta.row($(this).closest('tr')).data();
            dtTablaGenPropuesta.data().toArray().forEach(rowData => {
                if (rowF.contratoid == rowData.contratoid) {
                    rowData.ivaInteres = unmaskNumero($(this).val());
                    rowData.importe = (rowData.capital + rowData.intereses + rowData.iva + rowData.ivaInteres) * (rowData.porcentaje / 100);
                    if (rowData.moneda != 1) {
                        // rowData.importeDLLS = rowData.importe * (rowData.porcentaje / 100);
                        rowData.importeDLLS = (rowData.capital + rowData.intereses + rowData.iva + rowData.ivaInteres) * (rowData.porcentaje / 100);

                        // rowData.total = (rowData.importeDLLS * rowData.tipoCambio);
                        rowData.total = ((rowData.importeDLLS + rowData.penaConvencional) * rowData.tipoCambio);
                    }
                    else {
                        // rowData.total = (rowData.importe * (rowData.porcentaje / 100) * rowData.tipoCambio);
                        // rowData.total = (((rowData.importe * (rowData.porcentaje / 100)) + rowData.penaConvencional) * rowData.tipoCambio);
                        rowData.total = (((rowData.importe) + rowData.penaConvencional) * rowData.tipoCambio);
                    }
                }
            });
            let dt = dtTablaGenPropuesta.data().toArray();
            dtTablaGenPropuesta.clear();
            dtTablaGenPropuesta.rows.add(dt).draw(true);
        });

        tablaGenPropuesta.on('change', '.inputIVA', function () {
            let rowF = dtTablaGenPropuesta.row($(this).closest('tr')).data();
            dtTablaGenPropuesta.data().toArray().forEach(rowData => {
                if (rowF.contratoid == rowData.contratoid) {
                    rowData.iva = unmaskNumero($(this).val());
                    rowData.importe = (rowData.capital + rowData.intereses + rowData.iva + rowData.ivaInteres) * (rowData.porcentaje / 100);
                    if (rowData.moneda != 1) {
                        // rowData.importeDLLS = rowData.importe * (rowData.porcentaje / 100);
                        rowData.importeDLLS = (rowData.capital + rowData.intereses + rowData.iva + rowData.ivaInteres) * (rowData.porcentaje / 100);

                        // rowData.total = (rowData.importeDLLS * rowData.tipoCambio);
                        rowData.total = ((rowData.importeDLLS + rowData.penaConvencional) * rowData.tipoCambio);
                    }
                    else {
                        // rowData.total = (rowData.importe * (rowData.porcentaje / 100) * rowData.tipoCambio);
                        // rowData.total = (((rowData.importe * (rowData.porcentaje / 100)) + rowData.penaConvencional) * rowData.tipoCambio);
                        rowData.total = (((rowData.importe) + rowData.penaConvencional) * rowData.tipoCambio);
                    }
                }
            });
            let dt = dtTablaGenPropuesta.data().toArray();
            dtTablaGenPropuesta.clear();
            dtTablaGenPropuesta.rows.add(dt).draw(true);
        });

        tablaGenPropuesta.on('change', '.inputPorcentaje', function () {
            let rowData = dtTablaGenPropuesta.row($(this).closest('tr')).data();
            rowData.porcentaje = unmaskNumero($(this).val());
            rowData.importe = (rowData.capital + rowData.intereses + rowData.iva + rowData.ivaInteres) * (rowData.porcentaje / 100);
            if (rowData.moneda != 1) {
                // rowData.importeDLLS = rowData.importe * (rowData.porcentaje / 100);
                rowData.importeDLLS = (rowData.capital + rowData.intereses + rowData.iva + rowData.ivaInteres) * (rowData.porcentaje / 100);

                // rowData.total = (rowData.importeDLLS * rowData.tipoCambio);
                rowData.total = ((rowData.importeDLLS + rowData.penaConvencional) * rowData.tipoCambio);
            }
            else {
                // rowData.total = (rowData.importe * (rowData.porcentaje / 100) * rowData.tipoCambio);
                // rowData.total = (((rowData.importe * (rowData.porcentaje / 100)) + rowData.penaConvencional) * rowData.tipoCambio);
                rowData.total = (((rowData.importe) + rowData.penaConvencional) * rowData.tipoCambio);
            }
            let dt = dtTablaGenPropuesta.data().toArray();
            dtTablaGenPropuesta.clear();
            dtTablaGenPropuesta.rows.add(dt).draw(true);

            actualizarFooterGenPropuesta();
            // fncRecalcularFooter();
        });
        tablaGenPropuesta.on('change', '.inputTipoCambio', function () {
            let rowF = dtTablaGenPropuesta.row($(this).closest('tr')).data();
            dtTablaGenPropuesta.data().toArray().forEach(rowData => {
                if (rowF.contratoid == rowData.contratoid) {
                    rowData.tipoCambio = unmaskNumero($(this).val());
                    rowData.importe = (rowData.capital + rowData.intereses + rowData.iva + rowData.ivaInteres) * (rowData.porcentaje / 100);
                    if (rowData.moneda != 1) {
                        // rowData.importeDLLS = rowData.importe * (rowData.porcentaje / 100);
                        rowData.importeDLLS = (rowData.capital + rowData.intereses + rowData.iva + rowData.ivaInteres) * (rowData.porcentaje / 100);

                        // rowData.total = (rowData.importeDLLS * rowData.tipoCambio);
                        rowData.total = ((rowData.importeDLLS + rowData.penaConvencional) * rowData.tipoCambio);
                    }
                    else {
                        // rowData.total = (rowData.importe * (rowData.porcentaje / 100) * rowData.tipoCambio);
                        // rowData.total = (((rowData.importe * (rowData.porcentaje / 100)) + rowData.penaConvencional) * rowData.tipoCambio);
                        rowData.total = (((rowData.importe) + rowData.penaConvencional) * rowData.tipoCambio);
                    }
                }
            });
            let dt = dtTablaGenPropuesta.data().toArray();
            dtTablaGenPropuesta.clear();
            dtTablaGenPropuesta.rows.add(dt).draw(true);
        });

        tablaGenPropuesta.on('change', '.checkProgramar', function () {
            let rowData = dtTablaGenPropuesta.row($(this).closest('tr')).data();
            rowData.programado = $(this).prop('checked') ? 1 : 0;

            let dt = dtTablaGenPropuesta.data().toArray();
            dtTablaGenPropuesta.clear();
            dtTablaGenPropuesta.rows.add(dt).draw(true);
        });

        function getInfoLiquidar(liquidar, contratoId, parcialidad) {
            return $.get('/Contratos/GetInfoLiquidar', { liquidar, contratoId, parcialidad }).then(response => {
                if (response.success) {
                    return response;
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        function fnGuardarPropuesta() {

            var dataDt2 = dtTablaGenPropuesta.data().toArray().filter(contrato => contrato.programado == 1).map(
                function (seleccionados) {
                    let newSeleccion = seleccionados;
                    newSeleccion.fechaVencimiento = moment(newSeleccion.fechaVencimiento).format('DD/MM/YYYY');
                    newSeleccion.capital = seleccionados.capital * (seleccionados.porcentaje / 100);
                    newSeleccion.intereses = seleccionados.intereses * (seleccionados.porcentaje / 100);
                    newSeleccion.iva = seleccionados.iva * (seleccionados.porcentaje / 100);
                    newSeleccion.ivaInteres = seleccionados.ivaInteres * (seleccionados.porcentaje / 100);

                    let importe = newSeleccion.capital + newSeleccion.intereses + newSeleccion.iva + newSeleccion.ivaInteres;
                    newSeleccion.importe = unmaskNumero(maskNumero(importe));
                    newSeleccion.importeDLLS = unmaskNumero(maskNumero(seleccionados.importeDLLS));
                    newSeleccion.total = unmaskNumero(maskNumero(seleccionados.total));
                    newSeleccion.fechaCaptura = inputFechaCaptura.val();

                    return newSeleccion;
                }
            );

            if (dataDt2.length > 0) {
                $.post('/Contratos/GuardarProgramacionPagos', { obj: dataDt2 })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            AlertaGeneral('Confirmacion', `La operación se completo con exito`);
                            fnLoadProgramacionPagos();
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
        }
        //#endregion

        modalPolizaProgramados.on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

    }
    $(() => Maquinaria.DocumentosPorPagar.ProgramacionPagos = new ProgramacionPagos())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();