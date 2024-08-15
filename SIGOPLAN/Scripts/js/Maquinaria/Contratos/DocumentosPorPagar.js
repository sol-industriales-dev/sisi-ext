(() => {
    $.namespace('Contratos.DocumentosPorPagar');
    DocumentosPorPagar = function () {


        //#region SELECTORES
        let _comboACGlobal;
        const btnAddLinea = $('#btnAddLinea');
        const tblContratos = $('#tblContratos');
        const inputRFC = $('#inputRFC');
        const buscarRFC = $('#buscarRFC');
        const modalGeneral = $('#modalGeneral');
        const modalCtas = $('#modalCtas');
        const tablaGeneral = $('#tablaGeneral');

        let detTablaGeneral;
        let detMaquinaContrato;

        const btnNuevoContrato = $('#btnNuevoContrato');

        //#region FILTRO BUSQUEDA CONTRATOS
        const comboEmpresaFiltro = $('#comboEmpresaFiltro');
        const inputFiltroFolio = $('#inputFiltroFolio');
        const inputFiltroDescripcion = $('#inputFiltroDescripcion');
        const inputFiltroFecha = $('#inputFiltroFecha');
        const comboFinancieraFiltro = $('#comboFinancieraFiltro');
        const comboArrendamientoFiltro = $('#comboArrendamientoFiltro');

        const btnFiltroBuscar = $('#btnFiltroBuscar');
        //#endregion

        //#region MODIFICAR PARCIALIDAD
        const inputNuevaFecha = $('#inputNuevaFecha');
        const bntGuardarMensualidad = $('#bntGuardarMensualidad');
        const inputParcialidadModifica = $('#inputParcialidadModifica');
        const modalCambioPeriodo = $('#modalCambioPeriodo');
        //#endregion

        //#region PAGOS
        let _currentButton;
        let _idContrato = 0;
        let _periodo = 0;
        const inputFechaPago = $('#inputFechaPago');
        const tblPagos = $('#tblPagos');
        const modalPagos = $('#modalPagos');
        const inputTotalPago = $('#inputTotalPago');
        const inputPagoParcial = $('#inputPagoParcial');
        const btnGuardarPagos = $('#btnGuardarPagos');

        //#endregion

        //#region Reportes

        const btnImprimir = $('#btnImprimir');
        const report = $("#report");
        const modalUpdateContrato = $("#modalUpdateContrato");
        const inputArchivoContratoUpdate = $('#inputArchivoContratoUpdate');
        const btnGuardarUpdateArchivo = $('#btnGuardarUpdateArchivo');



        //#endregion

        //#region DESGLOSE GENERAL
        const modalDesgloseGeneral = $('#modalDesgloseGeneral');
        const tblDesgloseGeneral = $('#tblDesgloseGeneral');
        const tblDeudas = $('#tblDeudas');
        const btnGuardarDeudas = $('#btnGuardarDeudas');
        const inputFechaPoliza = $('#inputFechaPoliza');
        const inputTipoPoliza = $('#inputTipoPoliza');
        const inputGenerada = $('#inputGenerada');
        const inputPoliza = $('#inputPoliza');
        //#endregion

        //#region MAQUINA
        const modalMaquinas = $('#modalMaquinas');
        const modalDesglosePorMaquina = $('#modalDesglosePorMaquina');
        const inputCreditoMaquina = $('#inputCreditoMaquina');
        const selectMaquina = $('#selectMaquina');
        const btnAgregarMaquina = $('#btnAgregarMaquina');
        const tblMaquinas = $('#tblMaquinas');
        const tblDesglosePorMaquina = $('#tblDesglosePorMaquina');
        //#endregion

        //#region CAPTURA NUEVO CONTRATO
        const comboEmpresa = $('#comboEmpresa');
        const inputCtaInfo = $("#inputCtaInfo");
        const inputCtaIAInfo = $('#inputCtaIAInfo');
        const btnBuscarCuentas = $("#btnBuscarCuentas");
        const tablaCtas = $("#tablaCtas");
        const comboMonedaContrato = $('#comboMonedaContrato');
        const inputMontoOpcionCompra = $('#inputMontoOpcionCompra');
        const inputPenaConvencional = $('#inputPenaConvencional');
        const modalNuevoContrato = $('#modalNuevoContrato');
        const divInputFechaVencimiento = $('#divInputFechaVencimiento');
        const inputFolio = $('#inputFolio');
        const inputDescripcion = $('#inputDescripcion');
        const selectInstitucion = $('#selectInstitucion');
        const inputPlazo = $('#inputPlazo');
        const inputFechaInicio = $('#inputFechaInicio');
        const selectVencimiento = $('#selectVencimiento');
        const inputFechaVencimiento = $('#inputFechaVencimiento');
        const inputCredito = $('#inputCredito');
        const inputAmortizacion = $('#inputAmortizacion');
        const inputTasa = $('#inputTasa');
        const inputInteresMoratorio = $('#inputInteresMoratorio');
        const inputTipoCambio = $('#inputTipoCambio');
        const inputDomiciliado = $('#inputDomiciliado');
        const inputArchivoContrato = $('#inputArchivoContrato');
        const inputArchivoPagare = $('#inputArchivoPagare');
        const divDocumentos = $("#divDocumentos");
        const btnGuardarContrato = $('#btnGuardarContrato');
        const tblEconomicosContratos = $('#tblEconomicosContratos');
        const inputNoSerie = $('#inputNoSerie');
        const comboEconomicos = $('#comboEconomicos');
        const btnAgregarEconomico = $('#btnAgregarEconomico');
        const inputFechaF = $('#inputFechaF');
        const inputPorcentaje = $('#inputPorcentaje');
        const inputTotalesFinales = $('#inputTotalesFinales');
        const inputTasaFija = $('#inputTasaFija');
        const inputAplicaTasa = $('#inputAplicaTasa');
        const inputAplicaInteres = $('#inputAplicaInteres');
        const inputArrendamientoPuro = $('#inputArrendamientoPuro');
        const btnDescargaContrato = $('#btnDescargaContrato');
        const inputArchivoContratoNombre = $('#inputArchivoContratoNombre');
        const colPagoInterino = $('#colPagoInterino');
        const colPagoInterino2 = $('#colPagoInterino2');
        const colDepGarantia = $('#colDepGarantia');
        const inputPagoInterino = $('#inputPagoInterino');
        const inputPagoInterino2 = $('#inputPagoInterino2');
        const inputDepGarantia = $('#inputDepGarantia');
        const inputContratoAplicaInteres = $('#inputContratoAplicaInteres');
        //#endregion

        //#region Nueva Institucion
        const btnAddNewInstitucion = $('#btnAddNewInstitucion');
        const modalNuevaInstitucion = $('#modalNuevaInstitucion');
        const inputInstitucionDescripcion = $('#inputInstitucionDescripcion');
        const btnGuardarInstitucion = $('#btnGuardarInstitucion');

        //#endregion
        //#endregion
        let dtCtas;
        let dtDeudas
        let dtContratos;
        let dtMaquinas;
        let dtPagos;
        let dtDesgloseGeneral;
        let dtDesglosePorMaquina;
        let contrato;
        let contratoID;
        let inputInfoCuentas;

        const enumDiasVencimiento = {
            UltimoDiaMes: 1,
            Dia15: 2,
            Seleccion: 3
        };

        //#region DATEPICKER VARIABLES.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        //#endregion

        //#region EVENTOS
        selectInstitucion.on('change', function () {
            if ($(this).find('option:selected').text() == 'ENGENCAP') {
                colPagoInterino.show();
                colDepGarantia.show();
                colPagoInterino2.show();
            }
            else {
                colPagoInterino.hide();
                colDepGarantia.hide();
                colPagoInterino2.hide();

                inputPagoInterino.val('');
                inputPagoInterino2.val('');
                inputDepGarantia.val('');
            }
        });

        btnImprimir.on('click', function () {
            var path = `/Reportes/Vista.aspx?idReporte=189&pContratoID=${contratoID}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        });

        inputCtaIAInfo.getAutocomplete(SelectData, null, '/Contratos/autoCompleteCtasIA');
        function SelectData(event, ui) {
            inputCtaIAInfo.text(ui.item.label);
            inputCtaIAInfo.attr("data-index", ui.item.id);
        }

        //#endregion
        //#region NUEVO CONTRATO
        function initTablaCtas() {
            dtCtas = tablaCtas.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                "processing": true,
                "serverSide": true,
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

        function fnBuscarCuentasBanco() {
            modalCtas.modal('show');
            if (dtCtas.data().toArray().length == 0) {
                //  cargarTablaGeneral('/Contratos/LoadCtas/');
            }
        }

        function cargarTablaGeneral(url) {
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

        tablaCtas.on('click', '.seleccionar', function () {
            let rowData = dtCtas.row($(this).closest('tr')).data();

            inputInfoCuentas.val(`${rowData.cta}-${rowData.scta}-${rowData.sscta}-${rowData.digito}`)
            inputInfoCuentas.data().cta = rowData.cta;
            inputInfoCuentas.data().scta = rowData.scta;
            inputInfoCuentas.data().sscta = rowData.sscta;
            inputInfoCuentas.data().digito = rowData.digito;

            modalCtas.modal('hide');
        });

        tblContratos.on('click', '.btnTerminarContrato', function () {
            let data = dtContratos.row($(this).parents('tr')).data();
            fnComfirmarTermino(data);
        });

        function fnComfirmarTermino(data) {

            bootbox.confirm({
                title: "Terminar Contrato",
                message: "Deseas Actualizar ?.",
                buttons: {
                    cancel: {
                        label: '<i class="fa fa-times"></i> Cancelar'
                    },
                    confirm: {
                        label: '<i class="fa fa-check"></i> Confirmar'
                    }
                },
                callback: function (result) {
                    if (result) {
                        fnTerminarContratos(data);
                    }
                }
            });
        }

        function fnTerminarContratos(data) {
            $.post('/Contratos/TerminarContrato/', { contratoID: data.Id })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        obtenerContratos(crearObjetoFiltro());
                        AlertaGeneral(`Cofirmación`, `Se termino el contrato.`);
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

        btnGuardarDeudas.on('click', function () {
            guardarDeudas(crearInfoDeudas());
        });

        btnBuscarCuentas.on('click', function () {
            inputInfoCuentas = inputCtaInfo;
            fnBuscarCuentasBanco();
        });

        $('#tblDeudas').on('click', '.btnBuscarCuentasEnDeudas', function () {
            inputInfoCuentas = $(this).closest('span').closest('div').find('.inputInfoCuentaDeudas');
            fnBuscarCuentasBanco();
        });

        inputPlazo.on('keypress', function (event) {
            aceptaSoloNumeroXDIntMax($(this), event, 3);
        });

        inputPlazo.on('paste', function (event) {
            permitePegarSoloNumeroXDIntMax($(this), event, 3);
        });

        inputCredito.on('keypress', function (event) {
            aceptaSoloNumero2D($(this), event);
        });

        inputCredito.on('paste', function (event) {
            permitePegarSoloNumero2D($(this), event);
        });

        inputAmortizacion.on('keypress', function (event) {
            aceptaSoloNumero2D($(this), event);
        });

        inputAmortizacion.on('paste', function (event) {
            permitePegarSoloNumero2D($(this), event);
        });

        inputTasa.on('keypress', function (event) {
            aceptaSoloNumeroXD($(this), event, 6);
        });

        inputTasa.on('paste', function (event) {
            permitePegarSoloNumeroXD($(this), event, 6);
        });

        inputInteresMoratorio.on('keypress', function (event) {
            aceptaSoloNumeroXD($(this), event, 6);
        });

        inputInteresMoratorio.on('paste', function (event) {
            permitePegarSoloNumeroXD($(this), event, 6);
        })

        inputTipoCambio.on('keypress', function (event) {
            aceptaSoloNumeroXD($(this), event, 6);
        });

        inputTipoCambio.on('paste', function (event) {
            permitePegarSoloNumeroXD($(this), event, 6);
        });

        inputPagoInterino.on('keypress', function (event) {
            aceptaSoloNumero2D($(this), event);
        });

        inputPagoInterino.on('paste', function (event) {
            permitePegarSoloNumero2D($(this), event);
        });

        inputPagoInterino.on('change', function () {
            $(this).val(maskNumero($(this).val()));
        });

        inputPagoInterino2.on('keypress', function (event) {
            aceptaSoloNumero2D($(this), event);
        });

        inputPagoInterino2.on('paste', function (event) {
            permitePegarSoloNumero2D($(this), event);
        });

        inputPagoInterino2.on('change', function () {
            $(this).val(maskNumero($(this).val()));
        });

        inputDepGarantia.on('keypress', function (event) {
            aceptaSoloNumero2D($(this), event);
        });

        inputDepGarantia.on('paste', function (event) {
            permitePegarSoloNumero2D($(this), event);
        });

        inputDepGarantia.on('change', function () {
            $(this).val(maskNumero($(this).val()));
        });

        inputFechaF.on('change', function () {
            if (comboMonedaContrato.val() == "2") {
                if (!moment(inputFechaF.val(), 'DD/MM/YY').isValid()) {
                    inputFechaF.val('');
                }
                else {
                    $.get('/Contratos/GetTipoCambioFecha', { fecha: $(this).val() })
                        .then(response => {
                            if (response.success) {
                                // Operación exitosa.
                                inputTipoCambio.val(response.tipoCambio.tipo_cambio);
                            } else {
                                // Operación no completada.
                                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                            }
                        }, error => {
                            // Error al lanzar la petición.
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        });
                }
            }
        });

        selectVencimiento.on('change', function () {

            if ($(this).val() == enumDiasVencimiento.Seleccion) {
                divInputFechaVencimiento.show();
            }
            else {
                divInputFechaVencimiento.hide();
            }
        });

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

        function loadProveedores() {
            $.get('/Contratos/GetProveedores')
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        modalGeneral.modal('show');
                        detTablaGeneral.clear().draw();
                        detTablaGeneral.rows.add(response.listaObj).draw();
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

        //#region TABLA CONTRATOS
        tblEconomicosContratos.on('change', '.inputMontoFinanciado', function () {
            let data = detMaquinaContrato.row($(this).parents('tr')).data();
            let valorActual = unmaskNumero($(this).val());
            let valorFinanciado = unmaskNumero(inputCredito.val());
            let montoTotal = detMaquinaContrato.data().toArray().
                filter(
                    function (economicos) {
                        return economicos.economicoID != data.economicoID
                    }).
                reduce(function (acc, economico) {
                    return acc + (+economico.credito)
                }, 0);
            if (data.length <= 1) {

                //Regresa el total lo que se cuentra en la tabla.
                if ((montoTotal + valorActual) < valorFinanciado) {
                    let porcentaje = (valorActual / valorFinanciado) * 100;
                    data.credito = valorActual.toFixed(2);
                    data.porcentaje = porcentaje;
                    $(this).val(data.credito);
                    $(this).parents('tr').find('.inputPorcentaje').val(data.porcentaje);
                }
                else {
                    $(this).val(maskNumero(data.credito));
                    $(this).parents('tr').find('.inputPorcentaje').val(data.porcentaje);
                    AlertaGeneral('Error', 'El monto seleccinado supera al monto total de financiamiento');
                }
            }
            else {
                if (valorActual + montoTotal <= valorFinanciado) {
                    data.credito = valorActual.toFixed(2);
                    let arryData = detMaquinaContrato.data().toArray().map(function (r) {
                        r.porcentaje = ((r.credito / valorFinanciado) * 100).toFixed(2);
                        return r;
                    });
                    detMaquinaContrato.clear().draw();
                    AddRows(tblEconomicosContratos, arryData);
                }
                else {
                    $(this).val(maskNumero(data.credito));
                    $(this).parents('tr').find('.inputPorcentaje').val(data.porcentaje);
                    AlertaGeneral('Error', 'El monto seleccinado supera al monto total de financiamiento');
                }
            }
        });

        tblEconomicosContratos.on('click', '.btnEliminarEquipo', function () {
            let data = detMaquinaContrato.row($(this).parents('tr')).data();
            var newLista = detMaquinaContrato.data().toArray().filter(function (obj) {
                return obj.economicoID != data.economicoID
            });
            AddRows(tblEconomicosContratos, newLista);
        });

        tblContratos.on('click', '.btn-MoverMensualidades', function () {
            let rowData = dtContratos.row($(this).closest('tr')).data();
            _idContrato = rowData.Id;
            modalCambioPeriodo.modal('show');
        });

        tblContratos.on('click', '.btn-maquinas', function () {
            let rowData = dtContratos.row($(this).closest('tr')).data();
            btnAgregarMaquina.data('id_contrato', rowData.Id);
            inputCreditoMaquina.val(rowData.Credito);
            obtenerMaquinas(rowData.Id);
            modalMaquinas.modal('show');
        });

        tblContratos.on('click', '.btnDesgloseGeneral', function () {
            let rowData = dtContratos.row($(this).closest('tr')).data();
            contratoID = rowData.Id;
            btnGuardarDeudas.data('id_contrato', contratoID);
            obtenerDesgloseGeneral(rowData.Id);
            modalDesgloseGeneral.modal('show');
        });

        tblContratos.on('click', '.btn-EditarContrato', function () {
            inputFolio.data().ContratoId = 0;
            $('#inputFolio').data().ContratoId
            let rowData = dtContratos.row($(this).closest('tr')).data();
            inputFolio.data().ContratoId = rowData.Id;
            obtenerDatosContrato(rowData.Id);
        });

        tblContratos.on('click', '.btnActualizarContratosDet', function () {
            let rowData = dtContratos.row($(this).closest('tr')).data();
            updateContratosDet(rowData.Id);
        });

        tblContratos.on('click', '.btnActualizarArchivo', function () {
            let rowData = dtContratos.row($(this).closest('tr')).data();
            _idContrato = rowData.Id;

            modalUpdateContrato.modal('show');
        });

        function fnGuardarUpdateArchivoObj() {

            let archivoContrato = inputArchivoContratoUpdate.get(0).files[0];
            let formData = new FormData();
            formData.append('idContrato', _idContrato);
            formData.append('ArchivoContrato', archivoContrato);
            subirArchivo(formData);

        }

        function subirArchivo(objContrato) {

            $.ajax({
                type: 'POST',
                url: '/Contratos/UpdateContratoArchivo',
                data: objContrato,
                contentType: false,
                processData: false,
                cache: false,
                success: function (response) {
                    if (response.Success) {
                        AlertaGeneral('Confirmación', '¡Se registró correctamente el contrato!');
                        modalUpdateContrato.modal('hide');
                    }
                    else {
                        modalUpdateContrato.modal('show');
                        AlertaGeneral('Error', response.Message);
                    }
                },
                error: function (xhr, status, error) {
                    AlertaGeneral('Error: ' + error);
                }
            });
        }

        function updateContratosDet(contratoID) {
            $.post('/Contratos/UpdateContratosDet', { contratoID: contratoID })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        //#endregion
        //#region MODIFICAR PARCIALIDAD
        bntGuardarMensualidad.on('click', function () {
            $.post('/Contratos/GuardarFechaNuevoPeriodo', { contratoID: _idContrato, nuevaFecha: inputNuevaFecha.val(), parcialidad: inputParcialidadModifica.val() })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        modalCambioPeriodo.modal('hide');
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
        //#endregion

        //#region DESGLOSE GENERAL

        tblDesgloseGeneral.on('click', '.btnPago', function () {
            let rowData = dtDesgloseGeneral.row($(this).closest('tr')).data();
            $.get('/Contratos/ObtenerPagos', { contratoDetID: rowData.Id, parcialidad: rowData.Parcialidad })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        let totalPago = 0;
                        modalPagos.modal('show');
                        AddRows(tblPagos, response.contratoMaquinaDet);
                        response.contratoMaquinaDet.forEach(element => {
                            totalPago += element.Importe;
                        });
                        inputPagoParcial.val(totalPago);
                        inputTotalPago.val(totalPago).prop('disabled', true);
                        _idContrato = response.contrato.Id;
                        _periodo = rowData.Parcialidad;
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
        //#endregion

        //#region PAGOS
        tblPagos.on('change', '.inputImporte', function () {
            let pagoParcial = +$(this).val();
            let rowData = dtPagos.row($(this).closest('tr')).data();
            let totalPagoParcial = 0;
            rowData.Importe = pagoParcial;

            dtPagos.data().toArray().forEach(element => {
                totalPagoParcial += +element.Importe;
            });

            inputPagoParcial.val(totalPagoParcial.toFixed(4));
        });

        btnGuardarPagos.on('click', function () {
            saveOrUpdatePagos();
        });

        function saveOrUpdatePagos() {
            let objSave = getInfoPagos();
            $.post('/Contratos/saveOrUpdatePagos', { dxpPago: objSave.dxpPago, dxpPagoMaquina: objSave.dxpPagoMaquina, dxpContratoMaquinaDetalle: objSave.dxpContratoMaquinaDetalle })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        limpiarModalPagos();
                        AlertaGeneral('Operacionón exitosa', 'El pago fue guardado correctamente.');
                        obtenerDesgloseGeneral(_idContrato);
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

        function getInfoPagos() {
            let d = new Date();
            let objFecha = `${d.getDate()}/${d.getMonth() + 1}/${d.getFullYear()}`;
            let resultado = {};

            let dxpPago = {};
            dxpPago.Id = 0;
            dxpPago.PeriodoId = _periodo;
            dxpPago.Monto = inputTotalPago.val();
            dxpPago.FechaPago = inputFechaPago.val();
            dxpPago.ArchivoPago = "";
            dxpPago.Estatus = true;
            dxpPago.FechaCreacion = objFecha;
            dxpPago.UsuarioCreacionId = 0;
            dxpPago.FechaModificacion = objFecha;
            dxpPago.UsuarioModificacionId = 0;
            dxpPago.pagoParcial = inputPagoParcial.val();
            resultado.dxpPago = dxpPago;
            let detallePagos = [];
            dtPagos.data().toArray()
                .forEach(row => {
                    let $row = {};
                    if (true) {
                        $row.Id = 0;
                        $row.PagoId = dxpPago.Id;
                        $row.ContratoMaquinaId = row.Id;
                        $row.Estatus = true;
                        $row.montoPagado = row.Importe;
                        $row.fechaPago = inputFechaPago.val();
                        detallePagos.push($row);
                    }
                });
            resultado.dxpPagoMaquina = detallePagos;
            resultado.dxpContratoMaquinaDetalle = dtPagos.data().toArray();
            return resultado;
        }

        function limpiarModalPagos() {
            modalPagos.modal('hide');
            inputPagoParcial.val(' ');
            inputTotalPago.val(' ');
            AddRows(tblPagos, null);
        }

        //#endregion

        //#region MAQUINAS
        inputCreditoMaquina.on('keypress', function (event) {
            aceptaSoloNumeroXD($(this), event, 6);
        });

        inputCreditoMaquina.on('paste', function (event) {
            permitePegarSoloNumeroXD($(this), event, 6);
        });

        btnAgregarMaquina.on('click', function () {
            let valid = validarCampos('capturaMaquina');
            if (valid.valido) {
                guardarMaquina(crearObjetoMaquina());
            }
        });

        tblMaquinas.on('click', '.btnDesglosePorMaquina', function () {
            let rowData = dtMaquinas.row($(this).closest('tr')).data();

            obtenerDesglosePorMaquina(rowData.Id);
            modalDesglosePorMaquina.modal('show');
        });
        //#endregion

        //#region BOTONES FILTRO CONTRATOS
        btnFiltroBuscar.on('click', function () {
            obtenerContratos(crearObjetoFiltro());
        });

        btnNuevoContrato.on('click', function () {
            inputFolio.data().ContratoId = 0;
            limpiarModal('modalNuevoContrato');
            modalNuevoContrato.modal('show');
        });

        btnGuardarContrato.on('click', function () {
            $(this).parents('div.modal-footer').block({ message: null });
            let valid = validarCampos('capturaContrato');
            if (valid.valid) {
                guardarContrato(crearObjetoContrato());
            }
            else {
                $(this).parents('div.modal-footer').unblock();
                AlertaGeneral('Error', `Se encontro un error en los siguientes campos: ${valid.mensaje}`);
            }
        });
        //#endregion

        //#region MODALES
        $('.modal').on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        $('.modal').on('hidden.bs.modal', function () {
            modalNuevoContrato.find('.errorClass').removeClass('errorClass');
        });
        //#endregion
        //#endregion

        function fnGuardarInstitucion() {
            $.post('/Contratos/guardarInstitucion', { descripcion: inputInstitucionDescripcion.val() })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral('Operación, Exitosa', 'Se guardo Correctamente.');
                        inputInstitucionDescripcion.val(' ');
                        modalNuevaInstitucion.modal('hide');
                        initComboInstituciones();

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

        function fnOpenInstitucion() {
            modalNuevaInstitucion.modal('show');
        }

        //#region INITS
        (function init() {
            comboCbosData();
            inputFechaPoliza.prop('disabled', false);
            inputAplicaTasa.prop('checked', true);
            //inputTasa.DecimalFixPr(2);
            //inputInteresMoratorio.DecimalFixPr(2);
            inputRFC.prop('disabled', true);
            inputFolio.data().ContratoId = 0;
            initInputFechas();
            initTablaCtas();
            initTablaContratos();
            initTablaMaquinas();
            initTablaDesgloseGeneral();
            initTblDeudas();
            initTablaDesglosePorMaquina();
            initTablaPagos();
            initComboInstituciones();
            initComboMaquinas();
            initTablaGeneral();
            initTablaEconomicosContrato();
            obtenerContratos(crearObjetoFiltro());
            initListener();
            comboFinancieraFiltro.fillCombo('/Contratos/ObtenerInstituciones', null, false, null);
            inputFechaPoliza.change(setPoliza);
        })();

        function initListener() {

            comboMonedaContrato.change(fnSetTipocambio);
            btnAddNewInstitucion.click(fnOpenInstitucion);
            btnGuardarInstitucion.click(fnGuardarInstitucion);
            comboEconomicos.change(fnLoadEconomicos);
            buscarRFC.click(loadProveedores);
            btnAgregarEconomico.click(fnNewRowEconomicosContrato);
            $('.inputMoneda').change(fnSetMoneda);
            btnAddLinea.click(newLinePoliza);
            btnDescargaContrato.click(downloadURI);
            btnGuardarUpdateArchivo.click(fnGuardarUpdateArchivoObj);
        }

        function downloadURI() {
            var link = document.createElement("button");
            if ($("#inputFolio").data().ContratoId != 0) {
                link.download = '/Contratos/getFileDownload?id=' + $("#inputFolio").data().ContratoId;
                link.href = '/Contratos/getFileDownload?id=' + $("#inputFolio").data().ContratoId;
                link.click();
                location.href = '/Contratos/getFileDownload?id=' + $("#inputFolio").data().ContratoId;
            }
        }

        function newLinePoliza() {

            objDeuda = {
                Id: 0,
                ContratoId: btnGuardarDeudas.data('id_contrato'),
                Cta: 0,
                Scta: 0,
                Sscta: 0,
                Digito: 0,
                Descripcion: '',
                Debe: 0,
                Haber: 0,
                Estatus: true,
                fechaCreacion: new Date(),
                cc: 0,
                area: 0,
                cuenta: 0
            };

            dtDeudas.row.add(objDeuda).draw();
        }

        function fnSetTipocambio() {
            if ($(this).val() == "1")
                inputTipoCambio.val(maskNumero(1));
        }

        tblEconomicosContratos.on('change', '.inputPorcentaje', function () {
            let filtro = detMaquinaContrato.row($(this).parents('tr')).data();
            let credito = unmaskNumero(inputCredito.val());
            var porcentaje = +$(this).val();
            filtro.porcentaje = porcentaje;
            filtro.credito = (credito * porcentaje) / 100;
            $(this).parents('tr').find('.inputMontoFinanciado').val(maskNumero(filtro.credito));
            fnAutosum();
        });

        function fnSetMoneda() {

            var regexp = /^\d+(\.\d{1,2})?$/;

            let tempAttr = $(this).attr('data-monto');
            let tempMonto = unmaskNumero($(this).val());
            if (regexp.test(tempMonto)) {
                $(this).val(maskNumero(tempMonto));
                $(this).attr('data-monto', tempMonto);
            }
            else {
                AlertaGeneral('Alerta', 'Solo se permiten valores numericos');
                $(this).val(maskNumero(tempAttr));
                $(this).attr('data-monto', tempAttr);
            }
        }

        function fnLoadEconomicos() {
            if ($(this).val() != '') {
                $.get('/Overhaul/LoadInfoMaquinaria', { id: $(this).val() })
                    .then(response => {
                        let data = response.DatosEconomico;
                        inputNoSerie.val(data.Serie);
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        }

        function initInputFechas() {

            inputFiltroFecha.datepicker({ dateFormat, maxDate: fechaActual, showAnim });
            inputFechaInicio.datepicker({}).datepicker('setDate', fechaActual);
            inputFechaPago.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker('setDate', fechaActual);
            inputNuevaFecha.datepicker({}).datepicker('setDate', fechaActual);
            inputFechaVencimiento.datepicker({}).datepicker('setDate', fechaActual);
            inputFechaF.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker('setDate', fechaActual);
            inputFechaPoliza.datepicker({}).datepicker('setDate', fechaActual);

        }

        function initComboInstituciones() {
            selectInstitucion.fillCombo('/Contratos/ObtenerInstituciones', null, false, null);
        }

        function initComboMaquinas() {
            selectMaquina.fillCombo('/Contratos/ObtenerMaquinas', null, false, null);
            comboEconomicos.fillCombo('/Contratos/ObtenerMaquinas', null, false, null);
            comboEconomicos.select2();
        }
        //#endregion

        //#region FUNCIONES EN GENERAL
        function limpiarModal(nombreModal) {
            inputFolio.attr('disabled', false);
            switch (nombreModal) {
                case 'modalNuevoContrato':
                    modalNuevoContrato.find('input, select').each(function (index, element) {
                        $(this).val('');
                    });
                    detMaquinaContrato.clear().draw();
                    break;
            }
        }

        function validarCampos(campos) {
            let valido = { valid: true, mensaje: "" };
            let infoCta = inputCtaInfo.val().split('-');
            switch (campos) {
                case 'capturaContrato':
                    modalNuevoContrato.find('input, select').each(function (index, element) {
                        if (fnValidarInputs($(this), 'inputArchivoPagare') &&
                            fnValidarInputs($(this), 'inputFechaVencimiento') &&
                            fnValidarInputs($(this), 'inputDomiciliado') &&
                            fnValidarInputs($(this), 'comboEconomicos') &&
                            fnValidarInputs($(this), 'inputNoSerie') &&
                            fnValidarInputs($(this), 'inputPorcentaje') &&
                            fnValidarInputs($(this), 'inputTotalesFinales') &&
                            fnValidarInputs($(this), 'inputTasaFija') &&
                            fnValidarInputs($(this), 'inputAplicaInteres') &&
                            fnValidarInputs($(this), 'inputContratoAplicaInteres') &&
                            fnValidarInputs($(this), 'inputArrendamientoPuro') &&
                            fnValidarInputs($(this), 'inputArchivoContratoNombre') &&
                            fnValidarInputs($(this), 'inputCtaIAInfo')) {

                            if ($(element).attr('id') == 'inputArchivoContrato') {
                                if (inputFolio.data().contratoID == 0) {
                                    valido.valid = false;
                                    valido.mensaje += $(this).attr('id').replace('combo', '').replace('input', '') + ', ';
                                }
                            } else {
                                if (selectInstitucion.find('option:selected').text() != 'ENGENCAP' && ($(element).attr('id') == 'inputPagoInterino' || $(element).attr('id') == 'inputDepGarantia' || $(element).attr('id') == 'inputPagoInterino2')) {

                                }
                                else if (!validarCampo($(this))) {
                                    valido.valid = false;
                                    valido.mensaje = $(this).attr('id').replace('combo', '').replace('input', '') + ', ';
                                }
                            }
                        }
                    });
                    if (selectVencimiento.val() == enumDiasVencimiento.Seleccion) {
                        if (!validarCampo(inputFechaVencimiento)) {
                            valido.valid = false;
                            valido.mensaje = $(this).attr('id').replace('combo', '').replace('input', '') + ', ';
                        }
                    }
                    break;
                case 'capturaMaquina':
                    console.log('capturaMaquinaVAL')
                    if (!validarCampo($(selectMaquina))) {
                        $(selectMaquina).next('.select2').find('.select2-selection').css('background-color', '#F78181');
                        valido.mensaje = $(this).attr('id').replace('combo', '').replace('input', ''); valido.valid = false;
                    }
                    else { $(selectMaquina).next('.select2').find('.select2-selection').css('background-color', 'white'); }
                    if (!validarCampo($(inputCreditoMaquina))) {
                        valido.mensaje = $(this).attr('id').replace('combo', '').replace('input', '') + ', ';
                        valido.valid = false;
                    }
                    break;
            }
            if (infoCta[0] != undefined)
                inputCtaInfo.data().cta = +infoCta[0];
            else
                valido.valid = false;

            if (infoCta[1] != undefined)
                inputCtaInfo.data().scta = +infoCta[1];
            else
                valido.valid = false;

            if (infoCta[2] != undefined)
                inputCtaInfo.data().sscta = +infoCta[2];
            else
                valido.valid = false;

            if (infoCta[3] != undefined)
                inputCtaInfo.data().digito = +infoCta[3];
            else
                valido.valid = false;

            if (detMaquinaContrato.data().toArray().length == 0) {
                valido.valid = false
                valido.mensaje = 'Se debe agregar un contrato para poder continuar.';
            }

            return valido;
        }

        function fnValidarInputs(element, idValida) {
            return $(element).attr('id') != idValida;
        }

        function crearInfoDeudas() {

            let objDeudas = new Array();
            let movPol = new Array();
            let poliza = {};
            let fechaPoliza = inputFechaPoliza.val().split('/');

            let totalHaber = 0;
            let totalDebe = 0;

            let datosDeudas = $('#tblDeudas').DataTable().rows().data();
            let linea = 0;

            $('#tblDeudas').find('tbody').find('tr').each(function (index, value) {
                linea++;
                let btnCtas = $(value).find('.inputInfoCuentaDeudas').data();
                let descripcion = $(value).find('.inputDescripcion').val();
                let debe = unmaskNumero($(value).find('.inputDebe').val());
                let haber = unmaskNumero($(value).find('.inputHaber').val());
                let area = $(value).find('.inputArea').val();
                let cuenta = $(value).find('.inputCuenta').val();
                let centroCostos = $(value).find('.cboCC').val();

                objDeuda = {
                    Id: 0,
                    ContratoId: btnGuardarDeudas.data('id_contrato'),
                    Cta: btnCtas.cta,
                    Scta: btnCtas.scta,
                    Sscta: btnCtas.sscta,
                    Digito: btnCtas.digito,
                    Descripcion: descripcion,
                    Debe: debe,
                    Haber: haber,
                    Estatus: true,
                    fechaCreacion: new Date(),
                    area: area,
                    cuenta: cuenta,
                    cc: centroCostos
                };

                let monto = debe + haber;
                let tm = 1;
                if (haber != 0) {
                    monto = (monto * -1);
                    tm = 2;
                }


                objMovPol = {
                    id: 0,
                    year: inputFechaPoliza.val().split('/')[2],
                    mes: inputFechaPoliza.val().split('/')[1],
                    poliza: inputPoliza.val(),
                    tp: inputTipoPoliza.val(),
                    linea: linea,
                    cta: btnCtas.cta,
                    scta: btnCtas.scta,
                    sscta: btnCtas.sscta,
                    digito: btnCtas.digito,
                    tm: tm,
                    referencia: '',
                    cc: centroCostos,
                    concepto: descripcion,
                    iclave: 0,
                    itm: btnCtas.cta == 1110 ? 5 : 0,
                    st_par: 0,
                    orden_compra: 0,
                    numpro: 0,
                    cfd_ruta_pdf: '',
                    cfd_ruta_xml: '',
                    area: area,
                    cuenta_oc: cuenta,
                    monto: monto
                };

                movPol.push(objMovPol);
                totalHaber += haber;
                totalDebe += debe;
                objDeudas.push(objDeuda);
            });

            poliza.id = 0;
            poliza.year = fechaPoliza[2];
            poliza.mes = fechaPoliza[1];
            poliza.poliza = inputPoliza.val();
            poliza.tp = inputTipoPoliza.val();
            poliza.cargos = Number(totalHaber.toFixed(2));
            poliza.abonos = Number(totalDebe.toFixed(2)) * -1;
            poliza.generada = "C";
            poliza.status = 'N';
            poliza.error = '';
            poliza.concepto = "P";
            poliza.fechapol = inputFechaPoliza.val();

            return { objDeudas: objDeudas, poliza: poliza, movPol: movPol };
        }

        function crearObjetoContrato() {
            let archivoContrato = inputArchivoContrato.get(0).files[0];
            let archivoPagare = null;

            let economicos = detMaquinaContrato.data().toArray().map(function (economico) {
                let creditoEconomico = +economico.credito;
                let objData = {
                    ContratoId: inputFolio.data().ContratoId,
                    MaquinaId: economico.economicoID,
                    Credito: creditoEconomico.toFixed(2),
                    porcentaje: economico.porcentaje
                };
                return objData;
            });

            if (inputArchivoPagare.val() != '') {
                archivoPagare = inputArchivoPagare.get(0).files[0];
            }

            let formData = new FormData();
            formData.append('idContrato', inputFolio.data().ContratoId);
            formData.append('ArchivoContrato', archivoContrato);
            formData.append('ArchivoPagare', archivoPagare);
            formData.append('Folio', inputFolio.val());
            formData.append('Descripcion', inputDescripcion.val());
            formData.append('InstitucionId', selectInstitucion.val());
            formData.append('Plazo', inputPlazo.val());
            formData.append('FechaInicio', inputFechaInicio.val());
            formData.append('FechaVencimientoTipo', selectVencimiento.val());
            formData.append('FechaVencimiento', inputFechaVencimiento.val());
            formData.append('Credito', unmaskNumero(inputCredito.val()));
            formData.append('AmortizacionCapital', unmaskNumero(inputAmortizacion.val()));
            formData.append('TasaInteres', inputTasa.getVal());
            formData.append('InteresMoratorio', inputTasa.getVal());
            formData.append('TipoCambio', unmaskNumero(inputTipoCambio.val()));
            formData.append('Domiciliado', inputDomiciliado.prop('checked'));
            formData.append('rfc', inputRFC.val());
            formData.append('nomCorto', inputRFC.data().nomCorto);
            formData.append('montoOpcioncompra', unmaskNumero(inputMontoOpcionCompra.val()));
            formData.append('monedaContrato', comboMonedaContrato.val());
            formData.append('penaConvencional', inputPenaConvencional.val());
            formData.append('economicos', JSON.stringify(economicos));
            formData.append('empresa', comboEmpresa.val());
            formData.append('tasaFija', inputTasaFija.prop('checked'));
            formData.append('aplicaInteres', inputAplicaInteres.prop('checked'));
            formData.append('aplicaContratoInteres', inputContratoAplicaInteres.prop('checked'));
            formData.append('arrendamientoPuro', $('#inputArrendamientoPuro').prop('checked'));
            formData.append('pagoInterino', unmaskNumero(inputPagoInterino.val()));
            formData.append('pagoInterino2', unmaskNumero(inputPagoInterino2.val()));
            formData.append('depGarantia', unmaskNumero(inputDepGarantia.val()));

            //#region GET CTA
            let infoCta = inputCtaInfo.val().split('-');

            if (infoCta[0] != undefined)
                inputCtaInfo.data().cta = +infoCta[0];
            if (infoCta[1] != undefined)
                inputCtaInfo.data().scta = +infoCta[1];
            if (infoCta[2] != undefined)
                inputCtaInfo.data().sscta = +infoCta[2];
            if (infoCta[3] != undefined)
                inputCtaInfo.data().digito = +infoCta[3];

            formData.append('cta', inputCtaInfo.data().cta);
            formData.append('scta', inputCtaInfo.data().scta);
            formData.append('sscta', inputCtaInfo.data().sscta);
            formData.append('digito', inputCtaInfo.data().digito);
            //#endregion

            //#region GET CTAIA
            let infoCtaIA = inputCtaIAInfo.val().split('-');

            if (infoCtaIA[0] != undefined)
                inputCtaIAInfo.data().ctaIA = +infoCtaIA[0];
            if (infoCtaIA[1] != undefined)
                inputCtaIAInfo.data().sctaIA = +infoCtaIA[1];
            if (infoCtaIA[2] != undefined)
                inputCtaIAInfo.data().ssctaIA = +infoCtaIA[2];
            if (infoCtaIA[3] != undefined)
                inputCtaIAInfo.data().digitoIA = +infoCtaIA[3];

            formData.append('ctaIA', inputCtaIAInfo.data().ctaIA);
            formData.append('sctaIA', inputCtaIAInfo.data().sctaIA);
            formData.append('ssctaIA', inputCtaIAInfo.data().ssctaIA);
            formData.append('digitoIA', inputCtaIAInfo.data().digitoIA);
            //#endregion

            formData.append('fechaFirma', inputFechaF.val());

            return formData;
        }

        function crearObjetoFiltro() {
            let obj = null;

            if (inputFiltroFolio.val() != '' || inputFiltroDescripcion.val() != '' || inputFiltroFecha.val() != '' || comboEmpresaFiltro.val() != '') {
                obj = {
                    Folio: inputFiltroFolio.val(),
                    Descripcion: inputFiltroDescripcion.val(),
                    Fecha: inputFiltroFecha.val(),
                    empresa: comboEmpresaFiltro.val(),
                    financiera: comboFinancieraFiltro.val(),
                    arrendamiento: comboArrendamientoFiltro.val() == "1" ? false : true
                }
            }
            return obj;
        }

        function crearObjetoMaquina() {
            let obj = {
                ContratoId: btnAgregarMaquina.data('id_contrato'),
                MaquinaId: selectMaquina.val(),
                Credito: inputCreditoMaquina.val()
            }
            return obj;
        }

        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        //#endregion

        //#region ACCIONES AL SERVIDOR
        function obtenerDatosContrato(id) {
            $.get('/Contratos/ObtenerContratoByID', { idContrato: id })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        modalNuevoContrato.modal('show');
                        contrato = response.Value;
                        maquinaria = response.maquinaria;

                        inputFolio.data().contratoID = id;
                        inputFolio.val(contrato.Folio).prop('disabled', true);
                        inputDescripcion.val(contrato.Descripcion);
                        selectInstitucion.val(contrato.InstitucionId);
                        selectInstitucion.trigger('change');
                        inputPagoInterino.val(contrato.pagoInterino != null ? maskNumero(contrato.pagoInterino) : '');
                        inputPagoInterino2.val(contrato.pagoInterino2 != null ? maskNumero(contrato.pagoInterino2) : '');
                        inputDepGarantia.val(contrato.depGarantia != null ? maskNumero(contrato.depGarantia) : '');
                        inputPlazo.val(contrato.Plazo);
                        inputFechaInicio.val(response.fecha);
                        selectVencimiento.val(contrato.FechaVencimientoTipoId).trigger('change');
                        inputFechaVencimiento.val(contrato.fechave)
                        inputCredito.val(contrato.Credito);
                        inputAmortizacion.val(contrato.AmortizacionCapital);
                        inputTasa.val(contrato.TasaInteres);
                        inputInteresMoratorio.val(contrato.InteresMoratorio);
                        inputTipoCambio.val(contrato.TipoCambio);
                        inputDomiciliado.prop('checked', contrato.Domiciliado);
                        inputTasaFija.prop('checked', contrato.tasaFija);
                        inputAplicaInteres.prop('checked', contrato.aplicaInteres);
                        inputContratoAplicaInteres.prop('checked', contrato.aplicaContratoInteres);
                        inputRFC.val(contrato.rfc);
                        inputRFC.data().nomCorto = contrato.nombreCorto;
                        inputMontoOpcionCompra.val(contrato.montoOpcioncompra);
                        inputPenaConvencional.val(contrato.penaConvencional);
                        inputFechaF.val(contrato.fechaFirma);
                        inputFechaVencimiento.val(contrato.fechaVencimiento);
                        inputCtaInfo.data().cta = contrato.cta;
                        inputCtaInfo.data().scta = contrato.scta;
                        inputCtaInfo.data().sscta = contrato.sscta;
                        inputCtaInfo.data().digito = contrato.digito;
                        inputFechaF.val(contrato.fechaFirma);
                        inputCtaInfo.val(contrato.ctaConcat);
                        comboMonedaContrato.val(contrato.monedaContrato).trigger('change');
                        comboEmpresa.val(contrato.empresa);
                        inputArrendamientoPuro.prop('checked', contrato.arrendamientoPuro == 1 ? true : false);
                        inputArchivoContratoNombre.val(contrato.fileContrato);
                        AddRows(tblEconomicosContratos, maquinaria);
                        $('.inputPorcentaje').trigger('change');
                        $('.inputMoneda').trigger('change');
                        divDocumentos.removeClass('hide');

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

        function obtenerContratos(filtro) {
            $.post('/Contratos/ObtenerContratos', {
                filtro: filtro
            }).always().then(response => {
                if (response.Success) {
                    AddRows(tblContratos, response.Value);

                    if (response.Value[0].esAdmin) {
                        dtContratos.column("15").visible(true);
                        dtContratos.column("16").visible(true);
                        dtContratos.column("17").visible(true);
                        dtContratos.column("18").visible(true);
                        dtContratos.column("19").visible(true);
                        dtContratos.column("20").visible(true);
                        dtContratos.column("21").visible(true);
                    }
                }
                else {
                    AlertaGeneral(`Operación fallida`, response.Message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, 'Error: ' + error.statusText);
            });
        }

        function guardarContrato(objContrato) {

            $.ajax({
                type: 'POST',
                url: '/Contratos/GuardarContrato',
                data: objContrato,
                contentType: false,
                processData: false,
                cache: false,
                success: function (response) {
                    if (response.Success) {
                        limpiarModal('modalNuevoContrato');
                        obtenerContratos(crearObjetoFiltro());
                        AlertaGeneral('Confirmación', '¡Se registró correctamente el contrato!');
                        modalNuevoContrato.modal('hide');
                        btnGuardarContrato.parents('div.modal-footer').unblock({ message: null });
                    }
                    else {
                        //  modalNuevoContrato.modal('show');
                        AlertaGeneral('Error', response.Message);
                        btnGuardarContrato.parents('div.modal-footer').unblock({ message: null });
                    }
                },
                error: function (xhr, status, error) {
                    AlertaGeneral('Error: ' + error);
                    btnGuardarContrato.parents('div.modal-footer').unblock({ message: null });
                }
            });
        }

        function guardarMaquina(objMaquina) {
            $.post('/Contratos/GuardarMaquina', {
                maquina: objMaquina
            }).always().then(response => {
                if (response.Success) {
                    obtenerMaquinas(objMaquina.ContratoId);
                    inputFolio.data().ContratoId =
                        AlertaGeneral('Confirmación', '!Se registró correctamente la máquina!');
                }
                else {
                    AlertaGeneral('Error', response.Message);
                }
            }, error => {
                AlertaGeneral('Error: ', error.statusText);
            });
        }

        function obtenerMaquinas(idContrato) {
            $.get('/Contratos/ObtenerMaquinas', {
                idContrato: idContrato
            }).always().then(response => {
                if (response.Success) {
                    AddRows(tblMaquinas, response.Value);
                }
                else {
                    AlertaGeneral('Error', response.Message);
                }
            }, error => {
                AlertaGeneral('Error: ', error.statusText);
            });
        }

        function obtenerDesgloseGeneral(idContrato) {
            $.get('/Contratos/ObtenerDesgloseGeneral', {
                idContrato: idContrato
            }).always().then(response => {
                if (response.Success) {
                    btnGuardarDeudas.attr('disabled', !response.Value.guardarDeuda);
                    inputPoliza.attr('disabled', !response.Value.guardarDeuda);
                    AddRows(tblDesgloseGeneral, response.Value.desglose);
                    AddRows(tblDeudas, response.Value.deuda);
                    if (response.Value.deuda[0].poliza == 0)
                        inputPoliza.val(response.Value.poliza);
                    else
                        inputPoliza.val(response.Value.deuda[0].poliza);
                }
                else {
                    AlertaGeneral('Error', response.Message);
                }
            }, error => {
                AlertaGeneral('Error: ', error.statusText);
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

        function obtenerDesglosePorMaquina(idContratoMaquina) {
            $.get('/Contratos/ObtenerDesglosePorMaquina', {
                idContratoMaquina: idContratoMaquina
            }).always().then(response => {
                if (response.Success) {
                    AddRows(tblDesglosePorMaquina, response.Value);
                }
                else {
                    AlertaGeneral('Error', response.Message);
                }
            }, error => {
                AlertaGeneral('Error: ', error.statusText);
            });
        }

        function guardarDeudas(obj) {
            $.post('/Contratos/GuardarDeudas', {
                objDeudas: obj.objDeudas, poliza: obj.poliza, movPolList: obj.movPol
            }).always().then(response => {
                if (response.Success) {
                    btnGuardarDeudas.attr('disabed', true);
                    AlertaGeneral('Confirmación', 'Se guardó correctamente la información');
                }
                else {
                    AlertaGeneral('Alerta', response.Message);
                }
            }, error => {
                AlertaGeneral('Alerta', null);
            });
        }
        //#endregion

        function fnNewRowEconomicosContrato() {
            let obj = fnValidar();
            if (obj.status) {
                let arrayData = detMaquinaContrato.data().toArray();
                let creditoTotal = unmaskNumero(inputCredito.val());
                let porcentaje = (+inputPorcentaje.val());
                let credito = (creditoTotal * porcentaje) / 100;
                let objRow = {
                    noEconomico: $("#comboEconomicos option:selected").text(),
                    economicoID: comboEconomicos.val(),
                    noSerie: inputNoSerie.val(),
                    credito: +credito.toFixed(2),
                    porcentaje: +inputPorcentaje.val()
                };
                arrayData.push(objRow);
                AddRows(tblEconomicosContratos, arrayData);
                inputPorcentaje.val('');
                inputNoSerie.val('');
                comboEconomicos.val('').trigger('change');
                fnAutosum();
            } else {
                AlertaGeneral('Confirmación', obj.mensaje);
            }
        }

        function fnAutosum() {
            let sum = detMaquinaContrato.data().toArray().reduce((acc, pilot) => acc + pilot.credito, 0);
            inputTotalesFinales.val(sum.toFixed(2));
        }

        function fnValidar() {
            let objValidar = {
                status: true,
                mensaje: ""
            };

            var totalPorcentaje = detMaquinaContrato.data().toArray().reduce(function (accumulator, contrato) {
                return accumulator + contrato.porcentaje;
            }, 0);

            if (totalPorcentaje > 100) {
                objValidar.status = false;
                objValidar.mensaje = "El porcentaje sobre pasa el 100%";
                return objValidar;
            }

            if (comboEconomicos.val() == "") {
                objValidar.status = false;
                objValidar.mensaje = "Es necesario seleccionar un económico para realizar la operación.";
                return objValidar;
            }

            if (detMaquinaContrato.data().toArray().filter(function (economicos) { return economicos.economicoID == comboEconomicos.val() }).length != 0) {
                objValidar.status = false;
                objValidar.mensaje = "El economico seleccionado ya se encuentra agregado en la tabla.";
                return objValidar;
            }
            return objValidar;
        }

        //#region Funciones de Inicializacion de tablas.
        function initTablaEconomicosContrato() {
            detMaquinaContrato = tblEconomicosContratos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                },
                columns: [
                    { data: null, title: '#' },
                    { data: 'noEconomico', title: 'Número económico' },
                    { data: 'noSerie', title: 'Serie Equipo' },
                    {
                        data: 'credito', title: 'MontoFinanciado',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let html = `<input type='text' class='form-control inputMontoFinanciado' data-monto=${cellData} value='${maskNumero(cellData)}' disabled/>`;
                            $(td).text('');
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'porcentaje', title: '%',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let html = `<input type='text' class='form-control inputPorcentaje' value='${cellData}' />`;
                            $(td).text('');
                            $(td).append(html);
                        }
                    },
                    { data: null, title: 'Quitar' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            return '<button type="button" class="btn btn-primary btnEliminarEquipo"><i class="fas fa-trash"></i></button>';
                        }
                    }
                ],
                createdRow: function (row, data, dataIndex) {
                    $(row).find('td').eq(0).text(++dataIndex);
                }
            });
        }

        function initTablaContratos() {
            dtContratos = tblContratos.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                scrollX: true,
                initComplete: function (settings, json) {
                },
                columns: [
                    { data: null, title: 'Desglose' },
                    { data: 'Folio', title: 'Contrato' },
                    { data: 'Descripcion', title: 'Descripción' },
                    { data: 'Institucion', title: 'Institución' },
                    {
                        data: 'ParcialidadActual', title: 'Parcialidad Actual', createdCell: function (td, cellData, rowData, row, col) {
                            let html = cellData;
                            if (cellData <= 0)
                                html = 0;
                            $(td).text('');
                            $(td).append(html);
                        }
                    },
                    { data: 'FechaInicio', title: 'Fecha inicio' },
                    { data: 'FechaVencimiento', title: 'Fecha vencimiento periodo actual' },
                    { data: 'Credito', title: 'Monto Financiado' },
                    { data: 'AmortizacionCapital', title: 'Amortización Capital' },
                    { data: 'Intereses', title: 'Intereses' },
                    { data: 'Plazo', title: 'Plazo' },
                    { data: 'Domiciliado', title: 'Domiciliado' },
                    { data: 'PagosVencidos', title: 'Pendientes Mes' },
                    { data: 'ArchivoContrato', title: 'Archivo contrato' },
                    { data: 'ArchivoPagare', title: 'Archivo pagaré' },
                    { data: 'FechaFin', title: 'Termino Contrato', visible: false },
                    { data: 'OpcionCompra', title: 'Opcion a Compra', visible: false },
                    { data: null, title: 'Máquinas', visible: false },
                    { data: null, title: 'Mover mensualida', visible: false },
                    { data: null, title: 'Editar Contrato', visible: false },
                    { data: null, title: 'Contratos Actualización', visible: false },
                    { data: null, title: 'Actualizar Det', visible: false }
                ],
                columnDefs: [
                    {
                        targets: '_all',
                        className: 'dt-center'
                    },
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            return '<button type="button" class="btn btn-primary btnDesgloseGeneral"><i class="fas fa-align-justify"></i></button>';
                        }
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        targets: [6],
                        render: function (data, type, row) {
                            if (!row.DiaInhabil) {
                                return moment(data).format('DD/MM/YYYY');
                            }
                            else {
                                return moment(data).format('DD/MM/YYYY') + ' <i class="fa-exclamation-circle fas warning" style=" color: #F1C40F;"></i>';
                            }
                        }
                    },
                    {
                        targets: [7, 8],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [11, 13, 14],
                        render: function (data, type, row) {
                            if (data) {
                                return 'Sí';
                            }
                            else {
                                return 'No';
                            }
                        }
                    },
                    {
                        targets: [15],
                        render: function (data, type, row) {
                            return '<button class="btn btn-primary btnTerminarContrato" data-id_archivo_contrato="' + row.Id + '"><i class="glyphicon glyphicon-file"></i></button>';
                        }
                    },
                    {
                        targets: [16],
                        render: function (data, type, row) {
                            if (!data) {
                                return '<i class="far fa-times-circle fa-2x" style="color:gray;"></i>';
                            }
                            else {
                                return '<button class="btn btn-primary btnArchivoPagare" data-id_archivo_pagare="' + row.Id + '"><i class="glyphicon glyphicon-file"></i></button>';
                            }
                        }
                    },
                    {
                        targets: [17],
                        render: function (data, type, row) {
                            return '<button class="btn-maquinas btn btn-sm btn-primary"><i class="fas fa-eye"></i></button>';
                        }
                    },
                    {
                        targets: [18],
                        render: function (data, type, row) {
                            return '<button class="btn-MoverMensualidades btn btn-sm btn-primary"><i class="far fa-calendar-alt"></i></button>'
                        }
                    },
                    {
                        targets: [19],
                        render: function (data, type, row) {
                            if (row)
                                return '<button class="btn-EditarContrato btn btn-sm btn-primary"><i class="fas fa-edit"></i></button>'
                        }
                    },
                    {
                        targets: [20],
                        render: function (data, type, row) {
                            if (row)
                                return '<button class="btnActualizarArchivo btn btn-sm btn-primary"><i class="fas fa-edit"></i></button>'
                        }
                    },
                    {
                        targets: [21],
                        render: function (data, type, row) {
                            if (row)
                                return '<button class="btnActualizarContratosDet btn btn-sm btn-primary"><i class="fas fa-edit"></i></button>'
                        }
                    },
                    {
                        targets: [13, 14, 15, 16, 17],
                        width: '5%'
                    }
                ]
            });
        }

        function initTablaMaquinas() {
            dtMaquinas = tblMaquinas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                },
                columns: [
                    { data: null, title: '#' },
                    { data: 'NumeroEconomico', title: 'Número económico' },
                    { data: 'Credito', title: 'Crédito' },
                    { data: null, title: 'Desglose' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    {
                        targets: [2],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [3],
                        render: function (data, type, row) {
                            return '<button type="button" class="btn btn-primary btnDesglosePorMaquina"><i class="fas fa-align-justify"></i></button>';
                        }
                    }
                ],
                createdRow: function (row, data, dataIndex) {
                    $(row).find('td').eq(0).text(++dataIndex);
                }
            });
        }

        function initTablaDesgloseGeneral() {
            dtDesgloseGeneral = tblDesgloseGeneral.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                scrollX: true,
                initComplete: function (settings, json) {
                },
                columns: [
                    { data: 'Parcialidad', title: 'No Parcialidad' },
                    { data: 'AmortizacionCapital', title: 'Amortización capital' },
                    { data: 'IVASCapital', title: 'IVA S/Capital' },
                    { data: 'Interes', title: 'Intereses periodo' },
                    { data: 'IVAInteres', title: 'IVA de intereses' },
                    { data: 'Importe', title: 'Importe pago' },
                    { data: 'Saldo', title: 'Saldo' },
                    { data: 'FechaVencimiento', title: 'Fecha vencimiento' },
                    { data: 'Pagado', title: 'Pagado' },
                    { data: 'FechaPago', title: 'Fecha pago' },
                    { data: null, title: 'Opciones' }
                ],
                columnDefs: [
                    {
                        targets: '_all',
                        className: 'dt-center'
                    },
                    {
                        targets: [1, 2, 3, 4, 5, 6],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [7],
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        targets: [8],
                        render: function (data, type, row) {
                            return data ? '<i class="far fa-check-circle fa-2x"></i>' : '<i class="far fa-times-circle fa-2x"></i>';
                        }
                    },
                    {
                        targets: [9],
                        render: function (data, type, row) {
                            return row.Pagado ? moment(data).format('DD/MM/YYYY') : '';
                        }
                    },
                    {
                        targets: [10],
                        render: function (data, type, row) {

                            return '<button class="btn btn-primary btnPago"><i class="fas fa-money-check-alt fa-lg"></i> Pagos</button>'
                        }
                    }
                ], "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    // Total over this page
                    for (let index = 1; index < 6; index++) {

                        pageTotal = api
                            .column(index, { page: 'current' })
                            .data()
                            .reduce(function (a, b) {
                                return intVal(a) + intVal(b);
                            }, 0);
                        // Update footer
                        $(api.column(index).footer()).html(
                            maskNumero(pageTotal)
                        );
                    }
                }
            });
        }

        function initTblDeudas() {
            dtDeudas = tblDeudas.DataTable({
                destroy: true
                , language: dtDicEsp
                , ordering: false
                , paging: false
                , ordering: false
                , searching: false
                , bFilter: true
                , info: false
                , columns: [
                    {
                        data: 'Cta', title: 'Cuenta', createdCell: (td, data, rowData, row, col) => {
                            let input = '<div class="input-group">' +
                                '<span class="input-group-btn">' +
                                '<button class="btn btn-default btnBuscarCuentasEnDeudas" type="button">' +
                                '<span class="glyphicon glyphicon-search"></span>' +
                                '</button>' +
                                '</span>' +
                                '<input type="text" class="form-control inputInfoCuentaDeudas" value="' + data + '-' + rowData.Scta + '-' + rowData.Sscta + '-' + rowData.Digito + '"/>' +
                                '</div>';
                            $(td).html(input);
                        }
                    },
                    {
                        data: 'Descripcion', title: 'Descripcion',
                        createdCell: (td, cellData, rowData, row, col) => {
                            let html = `<input type='text' class='form-control inputDescripcion' value='${cellData}'/>`;
                            $(td).text('');
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'Debe', title: 'Debe', createdCell: function (td, cellData, rowData, row, col) {
                            let html = `<input type='text' class='form-control inputDebe' value='${maskNumero(cellData)}'/>`;
                            $(td).text('');
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'Haber', title: 'Haber', createdCell: function (td, cellData, rowData, row, col) {
                            let html = `<input type='text' class='form-control inputHaber' value='${maskNumero(cellData)}'/>`;
                            $(td).text('');
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'cc', title: 'C.C.', createdCell: function (td, cellData, rowData, row, col) {

                            html = `<select class='form-control cboCC'>
                                        ${getInfo()}
                                     </select>`;
                            $(td).text('');
                            $(td).append(html);
                            $(td).parent().children().find('.cboCC').val(cellData);
                        }
                    },
                    {
                        data: 'area', title: 'Area', createdCell: function (td, cellData, rowData, row, col) {
                            let html = `<input type='text' class='form-control inputArea' value='${cellData ?? ''}'/>`;
                            $(td).text('');
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'cuenta', title: 'Cuenta', createdCell: function (td, cellData, rowData, row, col) {
                            let html = `<input type='text' class='form-control inputCuenta' value='${cellData ?? ''}'/>`;
                            $(td).text('');
                            $(td).append(html);
                        }
                    },
                    {
                        data: null, title: '', createdCell: function (td, cellData, rowData, row, col) {
                            let input = `<button type="button" class="btn btn-danger deleteRow" data-idRow="${row}">
                                                <i class="fas fa-minus-square"></i>
                                        </button>`;
                            $(td).text('');
                            $(td).append(input);
                        }
                    }
                ],
                footerCallback: function (row, data, start, end, display) {
                    var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    // Total over this page
                    for (let index = 2; index < 4; index++) {
                        pageTotal = api
                            .column(index, { page: 'current' })
                            .data()
                            .reduce(function (a, b) {
                                return intVal(a) + intVal(b);
                            }, 0);

                        // Update footer
                        $(api.column(index).footer()).html(
                            maskNumero(pageTotal)
                        );
                    }
                },
                drawCallback: function (settings) {
                    $('.cboCC').select2();
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

        tblDeudas.on('click', '.deleteRow', function () {
            dtDeudas.row($(this).parents('tr'))
                .remove()
                .draw();
        });

        tblDeudas.on('change', '.inputInfoCuentaDeudas', function () {
            let objcCTA = $(this).val().split('-');
            const rowInfo = dtDeudas.row($(this).parents('tr')).data();
            rowInfo.cta = objcCTA[0];
            rowInfo.scta = objcCTA[1];
            rowInfo.sscta = objcCTA[2];
            rowInfo.digito = objcCTA[3];

            $(this).data().cta = rowInfo.cta;
            $(this).data().scta = rowInfo.scta;
            $(this).data().sscta = rowInfo.sscta;
            $(this).data().digito = rowInfo.digito;
            dtDeudas.draw();

        });

        tblDeudas.on('change', '.inputHaber', function () {
            let Haber = unmaskNumero($(this).val());
            const rowInfo = dtDeudas.row($(this).parents('tr')).data();
            rowInfo.Haber = Haber;
            dtDeudas.draw();
        });

        tblDeudas.on('change', '.inputDebe', function () {

            let debe = unmaskNumero($(this).val());
            const rowInfo = dtDeudas.row($(this).parents('tr')).data();
            rowInfo.Debe = debe;
            dtDeudas.draw();

        });

        tblDeudas.on('change', '.cboCC', function () {
            let cc = $(this).val();
            let area = $("option:selected", this).attr('data-area');
            let cuenta = $("option:selected", this).attr('data-cuenta');
            const rowInfo = dtDeudas.row($(this).parents('tr')).data();
            let inputTempArea = $(this).parents('tr').children().find('.inputArea');
            let inputTempCuenta = $(this).parents('tr').children().find('.inputCuenta');
            $(inputTempArea).val(area);
            $(inputTempCuenta).val(cuenta);

            rowInfo.cc = cc;
            rowInfo.Area = area;
            rowInfo.Cuenta = cuenta;
            dtDeudas.draw();

        });

        function setClaveCuenta(e, ui) {
            this.value = ui.item;
        }
        function validarCuenta(e, ui) {
            if (ui.item == null) {
                this.value = '0-0-0-0';
            }
        }
        function initTablaDesglosePorMaquina() {
            dtDesglosePorMaquina = tblDesglosePorMaquina.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                scrollX: true,
                initComplete: function (settings, json) {
                },
                columns: [
                    { data: 'Parcialidad', title: 'Pago inicial' },
                    { data: 'AmortizacionCapital', title: 'Amortización capital' },
                    { data: 'IVASCapital', title: 'IVA S/Capital' },
                    { data: 'Interes', title: 'Intereses periodo' },
                    { data: 'IVAInteres', title: 'IVA de intereses' },
                    { data: 'Importe', title: 'Importe pago' },
                    { data: 'Saldo', title: 'Saldo' },
                    { data: 'FechaVencimiento', title: 'Fecha vencimiento' }
                ],
                columnDefs: [
                    {
                        targets: '_all',
                        className: 'dt-center'
                    },
                    {
                        targets: [1, 2, 3, 4, 5, 6],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [7],
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    }
                ]
            });
        }

        function initTablaPagos() {
            dtPagos = tblPagos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollX: true,
                scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'Maquina', title: 'Maquinaria' },
                    { data: 'Importe', title: 'Importe' },
                    { data: 'Pagado', title: 'Aplicar Pago' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    {
                        targets: [1],
                        render: function (data, type, row) {
                            return `<input type='number' class="form-control inputImporte"  value='${row.Importe}'/>`
                        },
                    }
                    , {
                        targets: [2],
                        render: function (data, type, row) {
                            let checked = 'checked';
                            return `<input type='checkbox' class='form-control inputPagado' ${checked} />`
                        }
                    }
                ]
            });
        }

        function initTablaGeneral() {
            detTablaGeneral = tablaGeneral.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    { data: 'Value', title: 'RFC' },
                    { data: 'Text', title: 'Descripción' },
                    { data: 'Prefijo', title: 'Insitucion' },
                    { data: 'Value', render: (data, type, row) => `<button class="btn btn-success seleccionar" data-nom='${row.Prefijo}' data-id="${data}"><i class="far fa-check-square"></i></button>` },
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                drawCallback: function (settings) {
                }
            });
        }

        tablaGeneral.on('click', '.seleccionar', function () {
            let ID = $(this).attr('data-id');
            inputRFC.val(ID);
            inputRFC.data().nomCorto = $(this).attr('data-nom');
            modalGeneral.modal('hide');
        });
        //#endregion
    }
    $(document).ready(() => Contratos.DocumentosPorPagar = new DocumentosPorPagar())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();