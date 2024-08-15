(
    () => {
        $.namespace("Maquinaria.CatMaquina.AltaFinacieros");

        AltaFinancieros = function()
        {
            const cbBanco = $("#cbBanco");
            const cbPlazo = $("#cbPlazo");
            const btnAltaPlazo = $("#btnAltaPlazo");
            const btnBuscar = $("#btnBuscar");            
                        
            const btnGuardarFinanciero = $("#btnGuardarFinanciero");
            const btnAgregarFinanciero = $("#btnAgregarFinanciero");

            // --> Modal Financiero
            const modalAltaFinanciero = $("#modalAltaFinanciero");
            const txtFinanciero = $("#txtFinanciero");

            // --> Modal Plazo
            const modalAltaPlazo = $("#modalAltaPlazo");
            const cbFinancieroAltaPlazo = $("#cbFinancieroAltaPlazo");
            const cbTipoOperacionAltaPlazo = $("#cbTipoOperacionAltaPlazo");
            const txtOpcionCompraAltaPlazo = $("#txtOpcionCompraAltaPlazo");
            const txtEngancheAltaPlazo = $("#txtEngancheAltaPlazo");
            const txtDepositoPorcentajeAltaPlazo = $("#txtDepositoPorcentajeAltaPlazo");
            const txtDepositoAltaPlazo = $("#txtDepositoAltaPlazo");
            const cbMonedaAltaPlazo = $("#cbMonedaAltaPlazo");
            const cbPlazoAltaPlazo = $("#cbPlazoAltaPlazo");
            const txtInteresAltaPlazo = $("#txtInteresAltaPlazo");
            const txtGastosAltaPlazo = $("#txtGastosAltaPlazo");
            const txtComisionAltaPlazo = $("#txtComisionAltaPlazo");
            const txtRentaGarantiaAltaPlazo = $("#txtRentaGarantiaAltaPlazo");
            const txtCrecimientoAltaPlazo = $("#txtCrecimientoAltaPlazo");
            const btnGuardarPlazo = $("#btnGuardarPlazo");

            // --> Tabla Plazos
            const tablaPlazos = $("#tablaPlazos");
            let dtPlazos;
            let esEdicion = false;
            let plazoActualID = 0;

            (function init(){
                AgregarListeners();
                LlenarCombos();
                InicializarTablaPlazos();
            })();

            function AgregarListeners()
            {
                btnGuardarFinanciero.click(GuardarFinanciero);
                btnAgregarFinanciero.click(AbrirModalFinanciero);
                btnAltaPlazo.click(AbrirModalPlazo);
                btnGuardarPlazo.click(GuardarPlazo);
                btnBuscar.click(CargarTablaPlazos);
            }

            function LlenarCombos()
            {
                cbBanco.fillCombo("/CatMaquina/FillFinanciero", {}, false);
                cbFinancieroAltaPlazo.fillCombo("/CatMaquina/FillFinanciero", {}, false);
            }

            function GetFinanciero()
            {
                let financiero = 
                {
                    descripcion: txtFinanciero.val().trim(),
                    estado: 1 
                }
                return financiero;
            }

            function GuardarFinanciero() {
                let financiero = GetFinanciero();
                if(ValidarFinanciero()){
                    axios.post('/CatMaquina/GuardarFinanciero', { financiero: financiero })
                    .catch(error => Alert2Error("Ocurrió un error al intentar almacenar la informacion: " + error.message))
                    .then(response => {
                        let { success, exito } = response.data;
                        if (success) {
                            if(exito)
                            {
                                Alert2Exito("Se guardaron los datos con éxito");
                                LlenarCombos();
                                modalAltaFinanciero.modal("hide");
                            }
                            else
                                Alert2Error("Ya existe un registro con esas características. no se realizó ninguna acción");
                        }
                    });
                }
                else
                {
                    Alert2Error("Se requieren todos los datos para proceder con el alta del registro");
                }
            }

            function GetPlazo()
            {
                let plazo = {
                    id: plazoActualID,
                    financieroID: cbFinancieroAltaPlazo.val(),
                    tipoOperacion: cbTipoOperacionAltaPlazo.val(),
                    opcionCompra: txtOpcionCompraAltaPlazo.val(),
                    enganche: txtEngancheAltaPlazo.val(),
                    depositoPorcentaje: txtDepositoPorcentajeAltaPlazo.val(),
                    depositoMoneda: txtDepositoAltaPlazo.val(),
                    moneda: cbMonedaAltaPlazo.val(),
                    plazo: cbPlazoAltaPlazo.val(),
                    tasaInteres: txtInteresAltaPlazo.val(),
                    gastosFijos: txtGastosAltaPlazo.val(),
                    comision: txtComisionAltaPlazo.val(),
                    rentasGarantia: txtRentaGarantiaAltaPlazo.val(),
                    crecimientoPagos: txtCrecimientoAltaPlazo.val(),
                    estado: 1
                }     
                return plazo;           
            }

            function GuardarPlazo() {
                let plazo = GetPlazo();
                if(ValidarPlazo()){
                    axios.post('/CatMaquina/GuardarPlazo', { plazo: plazo, esEdicion: esEdicion })
                    .catch(error => Alert2Error("Ocurrió un error al intentar almacenar la informacion: " + error.message))
                    .then(response => {
                        let { success, exito } = response.data;
                        if (success) {
                            if(exito)
                            {
                                Alert2Exito("Se guardaron los datos con éxito");
                                LlenarCombos();
                                modalAltaPlazo.modal("hide");
                            }
                            else
                                Alert2Error("Ya existe un registro con esas características. no se realizó ninguna acción");
                        }
                    });
                }
                else
                {
                    Alert2Error("Se requieren todos los datos para proceder con el alta del registro");
                }
            }

            function AbrirModalFinanciero()
            {
                txtFinanciero.val("");
                modalAltaFinanciero.modal("show");
            }

            function ValidarFinanciero()
            {
                let exito = true;
                if(txtFinanciero.val().trim() == "") exito = false;
                return exito;
            }

            function ValidarPlazo()
            {
                let exito = true;
                if(cbFinancieroAltaPlazo.val() == "" || cbFinancieroAltaPlazo.val() == null) exito = false;
                if(cbPlazoAltaPlazo.val() == "" || cbPlazoAltaPlazo.val() == null) exito = false;
                return exito;
            }

            function AbrirModalPlazo()
            {
                esEdicion = false;
                plazoActualID = 0;
                cbFinancieroAltaPlazo.attr("disabled", false);
                cbPlazoAltaPlazo.attr("disabled", false);

                cbFinancieroAltaPlazo.val(''),
                cbTipoOperacionAltaPlazo.val('1').change(),
                txtOpcionCompraAltaPlazo.val(''),
                txtEngancheAltaPlazo.val(''),
                txtDepositoPorcentajeAltaPlazo.val(''),
                txtDepositoAltaPlazo.val(''),
                cbMonedaAltaPlazo.val('1').change(),
                cbPlazoAltaPlazo.val('12').change(),
                txtInteresAltaPlazo.val(''),
                txtGastosAltaPlazo.val(''),
                txtComisionAltaPlazo.val(''),
                txtRentaGarantiaAltaPlazo.val(''),
                txtCrecimientoAltaPlazo.val(''),
                modalAltaPlazo.modal("show");
            }

            function InicializarTablaPlazos()
            {
                dtPlazos = tablaPlazos.DataTable({
                    destroy: true,
                    ordering: false,
                    language: dtDicEsp,
                    autoWidth: false,
                    searching: false,
                    paging: false,
                    info: false,
                    language: {
                        searchPanes: {
                            "emptyTable": '-'
                        }
                    },
                    columns: [
                        { title: 'id', data: 'id', visible: false },
                        { title: 'financieroID', data: 'financieroID', visible: false },
                        { 
                            title: 'Financiero', 
                            data: 'financiero', 
                            render: (data, type, row) => { return data == null ? "N/A" : data.descripcion; } 
                        },
                        { title: 'Plazo (meses)', data: 'plazo' },
                        { 
                            title: 'Tipo de Operación', 
                            data: 'tipoOperacion' , 
                            render: (data, type, row) => {
                                let html = "";
                                return data == 1 ? "ARRENDAMIENTO" : (data == 2 ? "CRÉDITO SIMPLE" : "N/A");                        
                            } 
                        },
                        { title: 'Opción Compra', data: 'opcionCompra' },
                        { title: 'Valor Residual / Enganche', data: 'enganche' },
                        { title: 'Deposito en efectivo %', data: 'depositoPorcentaje' },
                        { title: 'Deposito en efectivo $$', data: 'depositoMoneda' },
                        { 
                            title: 'Moneda', 
                            data: 'moneda', 
                            render: (data, type, row) => {
                                let html = "";
                                return data == 1 ? "PESOS" : (data == 2 ? "DÓLARES" : "N/A");                        
                            } 
                        },                        
                        { title: 'Tasa de Interés', data: 'tasaInteres' },
                        { title: 'Gastos fijos', data: 'gastosFijos' },
                        { title: 'Comisión %', data: 'comision' }, 
                        { title: 'Rentas en Garantía', data: 'rentasGarantia' },
                        { title: 'Crecimiento pagos', data: 'crecimientoPagos' },
                        { title: 'Puesto', data: 'estado', visible: false },
                        { title: 'Nombre', data: 'usuarioRegistra', visible: false },
                        { title: 'Nombre', data: 'fechaRegistro', visible: false },
                        {
                            title: 'Editar', 
                            data: 'id', 
                            render: (data, type, row) => {
                                return '<button class="btn btn-sm btn-warning editar" data-index="' + data + '"><i class="fas fa-edit"></i></button>'
                            }        
                        },
                    ],
                    initComplete: function (settings, json) 
                    {
                        tablaPlazos.on("click", ".editar", function () {
                            let plazoID = $(this).attr("data-index")
                            CargarModalEditar(plazoID);
                            modalAltaPlazo.modal("show");
                        });
                    }
                }); 
                if (!tablaPlazos.is(":blk-transpose"))
                    tablaPlazos.transpose({ mode: 0 });   
                tablaPlazos.transpose("transpose");           
            }

            function ValidarCargaPlazos()
            {
                let exito = true;
                // if(cbBanco.val() == "" || cbBanco.val() == null) exito = false;
                // if(plazoMeses.val() == "" || plazoMeses.val() == null) exito = false;
                return exito;
            }

            function CargarTablaPlazos()
            {
                if(ValidarCargaPlazos())
                {
                    let financieroID = cbBanco.val();
                    let plazoMeses = cbPlazo.val();
                    
                    axios.post('/CatMaquina/GetPlazo', { financieroID: financieroID, plazoMeses: plazoMeses })
                    .catch(error => AlertaGeneral(error.message))
                    .then(response => {
                        let { success, lstTablaPlazos } = response.data;
                        if (success) {
                            tablaPlazos.transpose("reset");
                            console.log(response.data.items);
                            dtPlazos.clear();
                            dtPlazos.rows.add(response.data.items);
                            dtPlazos.draw();        
                            tablaPlazos.transpose("transpose"); 
                            $('#tablaPlazos thead th:eq(1)').width('10');                   
                        }
                    });
                }
                else
                {
                    Alert2Error("Se requieren todos los campos en filtros para la búsqueda");
                }
            }

            function CargarModalEditar(index)
            {
                cbFinancieroAltaPlazo.attr("disabled", true);
                cbPlazoAltaPlazo.attr("disabled", true);
                esEdicion = true;
                axios.post('/CatMaquina/GetPlazoByID', { plazoID: index })
                .catch(error => AlertaGeneral(error.message))
                .then(response => {
                    let { success, plazo } = response.data;
                    if (success) {
                        plazoActualID = plazo.id;
                        cbFinancieroAltaPlazo.val(plazo.financieroID);
                        cbTipoOperacionAltaPlazo.val(plazo.tipoOperacion);
                        txtOpcionCompraAltaPlazo.val(plazo.opcionCompra);
                        txtEngancheAltaPlazo.val(plazo.enganche);
                        txtDepositoPorcentajeAltaPlazo.val(plazo.depositoPorcentaje);
                        txtDepositoAltaPlazo.val(plazo.depositoMoneda);
                        cbMonedaAltaPlazo.val(plazo.moneda);
                        cbPlazoAltaPlazo.val(plazo.plazo),
                        txtInteresAltaPlazo.val(plazo.tasaInteres);
                        txtGastosAltaPlazo.val(plazo.gastosFijos);
                        txtComisionAltaPlazo.val(plazo.comision);
                        txtRentaGarantiaAltaPlazo.val(plazo.rentasGarantia);
                        txtCrecimientoAltaPlazo.val(plazo.crecimientoPagos);              
                    }
                });
            }
        }
        $(document).ready(() => AltaFinancieros = new AltaFinancieros())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
    }
)();