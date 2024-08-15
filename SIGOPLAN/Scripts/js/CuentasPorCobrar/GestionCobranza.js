(function () {
    $.namespace('CuentasPorCobrar.CuentasPorCobrar.GestionCobranza');

    gestioncobranza = function () {
        const comboAC = $('#comboAC');
        const cbTipoCorte = $('#cbTipoCorte');
        const inputCorte = $('#inputCorte');
        const botonBuscar = $('#botonBuscar');
        const comboDivision = $("#comboDivision");
        const comboResponsable = $("#comboResponsable");
        var lstDetalle = [];

        const getLstCXC = new URL(window.location.origin + '/CuentasPorCobrar/CuentasPorCobrar/getCXC');

        const getLstCC = new URL(window.location.origin + '/CuentasPorCobrar/CuentasPorCobrar/getLstCC');
        const checkResponsable = new URL(window.location.origin + '/CuentasPorCobrar/CuentasPorCobrar/checkResponsable');
        const comboCC = $('#comboCC');

        let fechaCorteGeneral = new Date();

        const tablaCXC = $("#tablaCXC");
        let dtTablaCXC;

        const tablaCXCFacturas = $("#tablaCXCFacturas");
        let dtTablaCXCFacturas;

        const tablaCXCAC = $("#tablaCXCAC");
        let dtTablaCXCAC;

        const titleModalCXCFacturas = $('#titleModalCXCFacturas');

        const tblCXCCc = $('#tblCXCCc');
        let dtCXCCc;

        const modalCXCClientes = $('#modalCXCClientes');
        const titleModalCXCClientes = $('#titleModalCXCClientes');
        const titleClienteModalCXCClientes = $('#titleClienteModalCXCClientes');
        const titleClienteModalCXCClientes2 = $('#titleClienteModalCXCClientes2');
        const tblCXCCcClientes = $('#tblCXCCcClientes');
        let dtCXCCcClientes;

        //#region FILTROS
        const chkCETipoTabla = $('#chkCETipoTabla');

        const divShowCC = $('#divShowCC');
        const divShowClientes = $('#divShowClientes');
        const btnFiltroCorte = $('#btnFiltroCorte');
        const btnSegComentarios = $('#btnSegComentarios');
        //#endregion

        //#region CONSTS MODAL
        const mdlCEConvenio = $('#mdlCEConvenio');
        const txtCEConvenioTitulo = $('#txtCEConvenioTitulo');
        const btnCEConvenio = $('#btnCEConvenio');

        const txtCEConvenioNumcte = $('#txtCEConvenioNumcte');
        const txtCEConvenioNombrecliente = $('#txtCEConvenioNombrecliente');
        const cboCEConvenioFactura = $('#cboCEConvenioFactura');
        const cboCEConvenioCC = $('#cboCEConvenioCC');
        const cboCEConvenioPeriodo = $('#cboCEConvenioPeriodo');
        const txtCEConvenioMonto = $('#txtCEConvenioMonto');
        const txtCEConvenioMontonuevo = $('#txtCEConvenioMontonuevo');
        const txtCEConvenioFechaoriginal = $('#txtCEConvenioFechaoriginal');
        const txtCEConvenioFechanueva = $('#txtCEConvenioFechanueva');
        const cboCEConvenioAutoriza = $('#cboCEConvenioAutoriza');
        const txtCEConvenioComentarios = $('#txtCEConvenioComentarios');
        const cboFiltroCC = $('#cboFiltroCC');
        const cboFiltroPeriodo = $('#cboFiltroPeriodo');
        const divCEConvenioAutoriza = $('#divCEConvenioAutoriza');
        const txtCEAcuerdoTitle = $('#txtCEAcuerdoTitle');

        const tblAbonos = $('#tblAbonos');
        let dtAbonos
        //#endregion

        //#region CONSTS MODAL ACUERDO DET
        const mdlCEAcuerdoDet = $('#mdlCEAcuerdoDet');
        const txtCEAcuerdoDetMonto = $('#txtCEAcuerdoDetMonto');
        const txtCEAcuerdoDetFecha = $('#txtCEAcuerdoDetFecha');
        const btnCEAcuerdoDet = $('#btnCEAcuerdoDet');
        const btnCEAcuerdoAddBono = $('#btnCEAcuerdoAddBono');
        //#endregion

        //#region MODAL COMENTARIOS REMOVER FACTURA
        const mdlRemoveComentarios = $('#mdlRemoveComentarios');
        const txtRemoveTituloFactura = $('#txtRemoveTituloFactura');
        const txtRemoveComentario = $('#txtRemoveComentario');
        //#endregion

        //#region MODAL COMENTARIOS

        const divVerComentario = $('#divVerComentario');
        const timeline = $('#timeline');
        const ulComentarios = $('#ulComentarios');
        const txtComentarios = $('#txtComentarios');
        const btnCrearComentario = $('#btnCrearComentario');
        const cboCEComentarioTipo = $('#cboCEComentarioTipo');
        const txtCEComentarioTitle = $('#txtCEComentarioTitle');
        const divCEComentarioFechaCompromiso = $('#divCEComentarioFechaCompromiso');
        const txtCEComentarioFechaCompromiso = $('#txtCEComentarioFechaCompromiso');
        //#endregion

        //#region CONSTS MODAL SEG COMENTARIOS

        const mdlSegComentarios = $('#mdlSegComentarios');
        const divSegComentariosCC = $('#divSegComentariosCC');
        const divSegComentariosCliente = $('#divSegComentariosCliente');
        const chkCETipoTablaComentarios = $('#chkCETipoTablaComentarios');

        const tblSegComentariosCC = $('#tblSegComentariosCC');
        const tblSegComentariosCliente = $('#tblSegComentariosCliente');

        let dtSegComentariosCC;
        let dtSegComentariosCliente;

        //#endregion

        //#region MDL CAROUSEL
        const mdlCarousel = $('#mdlCarousel');
        const carouselNotis = $('#carouselNotis');
        const carouselNotisInner = $('#carouselNotisInner');
        var btnNotiCapturar;
        //#endregion

        //#region MDL FECHA MOD
        const mdlCEFacturaMod = $('#mdlCEFacturaMod');
        const fechaCEFacturaModFechaOG = $('#fechaCEFacturaModFechaOG');
        const fechaCEFacturaModFechaNueva = $('#fechaCEFacturaModFechaNueva');
        const btnCEFacturaMod = $('#btnCEFacturaMod');
        //#endregion

        //Facturas del corte
        let lstFacturasCorte = [];

        //Bandera para saber si un acuerdo de gestion de cobranza es autorizable
        let esAutorizar = false;
        let multipleCount = 0;

        function init() {
            setResponsable();

            inputCorte.datepicker().datepicker('setDate', new Date());

            fillCombos();
            agregarListeners();
            initTablaCXC();
            initTablaCXCFacturas();
            initTablaCXCAC();
            initTblCXCCc();
            initTblCXCCcClientes();
            initTblAbonos();
            initTblSegComentariosCC();
            // initTblSegComentariosCliente();
            fncMostrarCarrucelVencer();

            //OCULTAR UNO DE LOS DOS DIVS
            divShowClientes.hide();
            divSegComentariosCliente.hide();
        }

        function agregarListeners() {
            botonBuscar.click(function (e) {
                lstFacturasCorte = [];
                setLstCXC(e);
            });
            comboDivision.change(cargarAC);
            comboResponsable.change(cargarAC);

            comboAC.next(".select2-container").css("display", "none");
            $("#spanComboAC").click(function (_e) {
                comboAC.next(".select2-container").css("display", "block");
                comboAC.siblings("span").find(".select2-selection__rendered")[0].click();
            });
            comboAC.on('select2:close', function (_e) {
                comboAC.next(".select2-container").css("display", "none");
                var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
                if (seleccionados.length == 0) $("#spanComboAC").text("TODOS");
                else {
                    if (seleccionados.length == 1) $("#spanComboAC").text($(seleccionados[0]).text().slice(1));
                    else $("#spanComboAC").text(seleccionados.length.toString() + " Seleccionados");
                }
            });
            comboAC.on("select2:unselect", function (evt) {
                if (!evt.params.originalEvent) { return; }
                evt.params.originalEvent.stopPropagation();
            });

            $("#modalCXCFacturas").on('shown.bs.modal', function (_e) {
                dtTablaCXCFacturas.columns.adjust();
            });
            $("#botonCerrarCXCFacturas").click(function (_e) {
                $("#modalCXCFacturas").modal("hide");
            });
            $("#modalCXCAC").on('shown.bs.modal', function (_e) {
                dtTablaCXCAC.columns.adjust();
            });
            $("#botonCerrarCXCAC").click(function (_e) {
                $("#modalCXCAC").modal("hide");
            });

            chkCETipoTabla.on("change", function () {
                if ($(this).prop("checked")) {
                    divShowCC.show();
                    divShowClientes.hide();
                } else {
                    divShowCC.hide();
                    divShowClientes.show();
                    dtTablaCXC.columns.adjust();
                }
            });

            btnFiltroCorte.on("click", function () {
                fncCrearEditarCorte(lstFacturasCorte);
            });

            //#region LISTENERS MODAL
            txtCEConvenioNombrecliente.getAutocomplete(setClienteId, null, '/Facturacion/Prefacturacion/FillComboClienteNombre');
            cboCEConvenioCC.fillCombo('/Administrativo/Facultamiento/getComboCCEnkontrol', null, false, null);
            cboCEConvenioPeriodo.fillCombo('FillComboPeriodos', null, false, null);

            btnCEConvenio.on("click", function () {
                fncCrearEditarConvenio();
            });

            cboCEConvenioFactura.on("change", function (_event, getInfo) {
                if ($(this).val() != "" && $(this).val() != null) {
                    fncGetInfoFacturaById($(this).val(), getInfo);
                }
            });

            txtCEConvenioMonto.on("change", function () {
                if ($(this).val() != "" && $(this).val() != null) {
                    $(this).val(maskNumero($(this).val()));
                }
            });

            txtCEConvenioMontonuevo.on("change", function () {
                if ($(this).val() != "" && $(this).val() != null) {
                    $(this).val(maskNumero($(this).val()));
                }
            });

            //#endregion

            //#region LISTENERS MODAL ACUERDO DET
            btnCEAcuerdoDet.on("click", function () {
                let abono = {
                    id: 0,
                    abonoDet: unmaskNumero(txtCEAcuerdoDetMonto.val()),
                    fechaDet: txtCEAcuerdoDetFecha.val(),
                    esNuevo: true,
                }

                let esAddFechaDup = true;
                let esAddMontoExces = true;
                let esAddFechaPrev = true;

                if (moment(txtCEAcuerdoDetFecha.val())._d > moment().add(1, "month")._d) {
                    esAutorizar = true;
                }

                let totalAbonos = 0;

                if (dtAbonos.data().count() > 0) {
                    totalAbonos += abono.abonoDet;
                    dtAbonos.rows(function (_idx, data, _node) {
                        if (moment(data.fechaDet).format("DD/MM/YYYY") == moment(abono.fechaDet).format("DD/MM/YYYY")) {
                            esAddFechaDup = false;
                        }
                        if (typeof (data.abonoDet) == "string") {
                            totalAbonos += unmaskNumero(data.abonoDet);

                        } else {
                            totalAbonos += data.abonoDet;

                        }
                    });
                } else {
                    totalAbonos = abono.abonoDet;
                }

                if (totalAbonos > Number(unmaskNumero(txtCEConvenioMonto.val()))) {
                    esAddMontoExces = false;
                }

                // if (moment(abono.fechaDet)._d < moment(txtCEConvenioFechaoriginal.val())._d) {
                if (moment(abono.fechaDet)._d < moment()._d) {
                    esAddFechaPrev = false;
                }

                if (esAddFechaDup && esAddFechaPrev && esAddMontoExces) {
                    if (txtCEAcuerdoDetMonto.val() == "" || unmaskNumero(txtCEAcuerdoDetMonto.val()) == 0) {

                    } else {
                        dtAbonos.rows.add([abono]);
                        dtAbonos.draw();
                        // dtAbonos.order([1, 'asc']).draw();
                        mdlCEAcuerdoDet.modal("hide");


                        // if (esAutorizar) {
                        //     divCEConvenioAutoriza.css("display", "initial");
                        // } else {
                        //     divCEConvenioAutoriza.css("display", "none");
                        // }
                    }


                } else {
                    let warningMsg = "Datos de abono invalidos:\n";

                    if (!esAddFechaDup) {
                        warningMsg += "Ya existe un abono con la fecha capturada. ";
                    }

                    if (!esAddFechaPrev) {
                        warningMsg += "La fecha capturada del abono tiene que ser posterior a la fecha de corte. ";
                    }

                    if (!esAddMontoExces) {
                        warningMsg += "El monto de los abonos excede el total de la factura. ";
                    }

                    Alert2Warning(warningMsg);

                }

            });

            btnCEAcuerdoAddBono.on("click", function () {
                txtCEAcuerdoDetMonto.val("");
                txtCEAcuerdoDetFecha.val(moment().format("YYYY-MM-DD"));
                mdlCEAcuerdoDet.modal("show");
            });

            txtCEAcuerdoDetMonto.on("change", function () {
                if ($(this).val() != "" && $(this).val() != null) {
                    let newVal = unmaskNumero($(this).val());
                    $(this).val(maskNumero(newVal));
                    // 
                }
            });
            //#endregion

            //#region LISTENERS MODAL COMENTARIOS

            cboCEComentarioTipo.fillCombo('/CuentasPorCobrar/CuentasPorCobrar/FillComboTiposComentarios', null, false, null);

            btnCrearComentario.on("click", function () {
                fncCrearEditarComentarios(btnCrearComentario.data("factura"));
                // if (chkCETipoTabla.prop("checked")) {
                //     fncCrearEditarComentarios(btnCrearComentario.data("factura"));

                // } else {
                //     fncCrearEditarComentarios(btnCrearComentario.data("factura"));

                // }
            });

            cboCEComentarioTipo.on("change", function () {
                // 1.- LLAMADA
                if ($(this).val() == "2") {
                    divCEComentarioFechaCompromiso.css("display", "inline");
                } else {
                    divCEComentarioFechaCompromiso.css("display", "none");
                }
            });
            //#endregion

            //#region LISTENERS MODAL SEG COMENTARIOS

            btnSegComentarios.on("click", function () {
                fncGetComentariosVencer();
            });

            chkCETipoTablaComentarios.on("change", function () {
                if ($(this).prop("checked")) {
                    divSegComentariosCC.show();
                    divSegComentariosCliente.hide();

                } else {
                    divSegComentariosCC.hide();
                    divSegComentariosCliente.show();

                }
            });

            //#endregion

            //#region LISTENERS MODAL FACTURA MOD
            btnCEFacturaMod.on("click", function () {
                fncGuardarFacturaMod();
            });
            //#endregion
        }

        function fillCombos() {
            comboAC.select2({ closeOnSelect: false });
            // comboDivision.fillCombo('/CuentasPorCobrar/CuentasPorCobrar/fillComboDivision', {}, false, "TODAS");
            comboResponsable.fillCombo('/CuentasPorCobrar/CuentasPorCobrar/fillComboResponsable', {}, false, "TODOS");

            comboAC.fillCombo('cboObraEstimados', { divisionID: comboDivision.val() }, false, "TODOS");
            comboAC.find('option').get(0).remove();

            comboCC.multiselect();
            comboCC.multiselect('disable');

            // comboDivision.fillCombo('/CuentasPorCobrar/CuentasPorCobrar/fillComboDivision', {}, false, "TODOS");
            comboResponsable.fillCombo('/CuentasPorCobrar/CuentasPorCobrar/fillComboResponsable', {}, false, "TODOS");
        }

        //#region AUTOCOMPLETE CLIENTE MODAL
        function setClienteId(_event, ui) {
            GetObjClientePorNombre(ui.item.id, false, null);
        }

        function GetObjClientePorNombre(id, _esEdit, _rowData) {
            // modalBusqCliente.block({ message: mensajes.PROCESANDO });
            if (id > 0) {
                $.ajax({
                    url: "/Facturacion/Prefacturacion/GetObjCliente",
                    type: "POST",
                    data: { id },
                    datatype: "json",
                    success: function (response) {

                        txtCEConvenioNombrecliente.val(response.nombre);
                        txtCEConvenioNumcte.val(response.numcte);

                    },
                    error: function () {
                        // modalBusqCliente.unblock();
                    }
                });
            }
        }
        //#endregion

        //#region TBLS

        function setResponsable() {
            $.post(checkResponsable, { responsableID: comboResponsable.val() })
                .then(function (response) {
                    if (response.success) {
                        responsable = response.responsable;
                    } else {
                        // Operación no completada.
                    }
                }, function (_error) {
                    // Error al lanzar la petición.
                }
                );
        }

        function setLstCXC(e) {
            // e.preventDefault();
            // e.stopPropagation();
            // e.stopImmediatePropagation();
            var notSelected = $("#comboAC").find('option').not(':selected');
            var array = notSelected.map(function () {
                return this.value;
            }).get();

            var array2 = [];
            areasCuentaGlobal = comboAC.val().length > 0 ? comboAC.val() : ((responsable || comboDivision.val().length > 0) ? array : array2);

            if (areasCuentaGlobal.length == 1 && areasCuentaGlobal[0] == "TODOS") {
                areasCuentaGlobal = [];
            }

            $('#tablaKubrixDetalle tbody td').addClass("blurry");
            $.post(getLstCXC, {
                fecha: inputCorte.val(),
                areaCuenta: areasCuentaGlobal,
            })
                .then(function (response) {
                    //$.unblockUI;
                    if (response.success) {
                        //CXC
                        var detallesCXC = response.CXC;
                        var totalCXC = 0;
                        $.each(detallesCXC, function (_i, n) { totalCXC += n.monto; });
                        $(".pCXC").text(parseFloat(totalCXC).toFixed(2));
                        fechaCorteGeneral = inputCorte.datepicker('getDate');

                        cargarTablaCXC(response.CXC, fechaCorteGeneral);
                        cargarTablaCXCCc(response.CXC, fechaCorteGeneral);
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    //$.unblockUI;
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
                );
        }

        function initTablaCXC() {
            dtTablaCXC = tablaCXC.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                paging: true,
                ordering: true,
                dom: 'Bfrtip',
                columns: [
                    { title: '', render: function (_data, _type, row) { return "<button class='btn btn-sm btn-primary verCC' data-cliente='" + row.cliente + "'><i class='fas fa-layer-group'></i></button>"; } },
                    // { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-primary verFacturas' data-cliente='" + row.cliente + "'><i class='fas fa-clipboard-list'></i></button>"; } },
                    { data: 'cliente', title: 'Cliente' },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'montoPronosticado', title: 'Pronóstico', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { title: 'Abonos', render: function () { return maskNumero("0.00"); } },
                    { data: 'total', title: 'Saldo', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    // {
                    //     data: 'porcentaje',
                    //     title: '%',
                    //     render: function (data, _type, _row) {
                    //         return data.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                    //     }
                    // },

                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },

                ],
                initComplete: function () {
                    tablaCXC.on("click", '.verFacturas', function () {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaCXC.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        const proveedor = $(this).attr("data-proveedor");
                        // cargarTablaCXCFacturas(detalles, fechaCorteGeneral, null);
                        $("#modalCXCFacturas").modal("show");

                    });
                    tablaCXC.on("click", '.verCC', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaCXC.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        const proveedor = $(this).attr("data-proveedor");
                        cargarTablaCXCAC(detalles, fechaCorteGeneral);
                        titleClienteModalCXCClientes.text(rowData.cliente);
                        titleClienteModalCXCClientes2.text(rowData.cliente);

                        $("#modalCXCAC").modal("show");

                    });
                },
                footerCallback: function (_row, _data, _start, _end, _display) {
                    var api = this.api(), data;
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    totalPorVencer = (api.column(2).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0));
                    totalVencido15 = (api.column(3).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0));
                    totalVencido30 = (api.column(4).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0));
                    totalVencido60 = (api.column(5).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0));
                    totalVencido90 = (api.column(6).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0));
                    totalVencidoMas = (api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0));
                    pronostico = (api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0));
                    //programado = (api.column(10).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 ));
                    total = (api.column(10).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0));
                    // totalPorcentaje = (api.column(11).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0));
                    $(api.column(1).footer()).html('TOTAL');
                    $(api.column(2).footer()).html(maskNumero(parseFloat(totalPorVencer).toFixed(2).toString()));
                    $(api.column(3).footer()).html(maskNumero(parseFloat(totalVencido15).toFixed(2).toString()));
                    $(api.column(4).footer()).html(maskNumero(parseFloat(totalVencido30).toFixed(2).toString()));
                    $(api.column(5).footer()).html(maskNumero(parseFloat(totalVencido60).toFixed(2).toString()));
                    $(api.column(6).footer()).html(maskNumero(parseFloat(totalVencido90).toFixed(2).toString()));
                    $(api.column(7).footer()).html(maskNumero(parseFloat(totalVencidoMas).toFixed(2).toString()));
                    $(api.column(8).footer()).html(maskNumero(parseFloat(pronostico).toFixed(2).toString()));
                    $(api.column(9).footer()).html('$0.00');
                    $(api.column(10).footer()).html(maskNumero(parseFloat(total).toFixed(2).toString()));
                    // $(api.column(11).footer()).html(parseFloat(totalPorcentaje).toFixed(2).toString() + '%');
                },
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            columns: ':visible'
                        }

                    }
                ],
                // select: {
                //     style: 'multi',
                //     selector: 'td:last-child'
                // }
            }).columns.adjust();

        }

        function cargarTablaCXC(detalles, fechaCorte, detallesReverse) { //LVL1

            if (detalles == null) {
                return false;
            }
            else {

                if (detallesReverse != null && detallesReverse.length > 0) {
                    let facturasReverse = detallesReverse.map(function (e) { return e.factura; });
                    for (const item of detalles) {
                        let indexReverse = facturasReverse.indexOf(item.factura);
                        if (indexReverse != -1) {
                            let itemDet = detallesReverse[indexReverse];
                            item.montoPronosticado = itemDet.montoPronosticado;
                            item.esCorte = itemDet.esCorte;
                            item.esRemoved = itemDet.esRemoved;
                            item.comentarios = itemDet.comentarios;
                            item.idAcuerdo = itemDet.idAcuerdo;
                            item.fecha = typeof (itemDet.fecha) == 'object' ? ("/Date(" + Date.parse(itemDet.fecha).toString() + ")/") : itemDet.fecha;
                            // console.log(item);
                        }
                    }
                }

                var datosFinales = [];
                var auxDatosFinales = [];
                var totalDetalles = 0;
                $.each(detalles, function (_i, n) { totalDetalles += n.monto; });
                const grouped = groupBy(detalles, function (detalle) { return detalle.responsable; });
                dtTablaCXC.clear();
                Array.from(grouped, function ([key, value]) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const cliente = value[0].responsable;
                    var total = 0;
                    var detallesFiltrados = value;
                    //if(areasCuentaDetalle.length > 0) {
                    //    detallesFiltrados = $.grep(detallesFiltrados,function(el,index){ 
                    //        return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0); 
                    //    });
                    //}
                    $.each(detallesFiltrados, function (_i, n) { total += (n.monto - n.montoPagado); });
                    const porcentaje = totalDetalles > 0 ? (total * 100) / totalDetalles : 0;
                    const porVencerDetalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    var porVencer = 0;
                    if (porVencerDetalles.length > 0) $.each(porVencerDetalles, function (_i, n) { porVencer += n.monto; });
                    const vencido15Detalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido15 = 0;
                    if (vencido15Detalles.length > 0) $.each(vencido15Detalles, function (_i, n) { vencido15 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    const vencido30Detalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido30 = 0;
                    if (vencido30Detalles.length > 0) $.each(vencido30Detalles, function (_i, n) { vencido30 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    const vencido60Detalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido60 = 0;
                    if (vencido60Detalles.length > 0) $.each(vencido60Detalles, function (_i, n) { vencido60 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencido90Detalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido90 = 0;
                    if (vencido90Detalles.length > 0) $.each(vencido90Detalles, function (_i, n) { vencido90 += n.monto; });
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencidoMasDetalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    var vencidoMas = 0;
                    if (vencidoMasDetalles.length > 0) $.each(vencidoMasDetalles, function (_i, n) { vencidoMas += n.monto; });
                    // const pronosticoDetalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    let valPronostico = 0;
                    $.each(detallesFiltrados, function (_i, n) { valPronostico += n.montoPronosticado; });

                    const grupo = {
                        cliente: cliente,
                        numcte: value[0].numcte,
                        porVencer: porVencer,
                        vencido15: vencido15,
                        vencido30: vencido30,
                        vencido60: vencido60,
                        vencido90: vencido90,
                        vencidoMas: vencidoMas,
                        montoPronosticado: valPronostico,
                        total: total,
                        porcentaje: porcentaje,
                        detalles: value,
                        detallesOrig: detalles
                    };
                    auxDatosFinales.push(grupo);
                });
                auxDatosFinales.sort((a, b) => b.total - a.total);
                dtTablaCXC.rows.add(auxDatosFinales);
                dtTablaCXC.draw();

                return true;
            }
        }

        function fncGetAcuerdoById(idAcuerdo) {
            axios.post("GetAcuerdoById", { idAcuerdo }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    cboCEConvenioFactura.prop("disabled", false);
                    txtCEConvenioNombrecliente.prop("readonly", false);
                    fncMdlDefault();

                    GetObjClientePorNombre(items.numcte, true, null);
                    cboCEConvenioFactura.fillCombo("GetFacturasByCliente", { idCliente: items.numcte }, false, null, (_e) => {
                        cboCEConvenioFactura.val(items.idFactura);
                        cboCEConvenioFactura.trigger("change", ['getInfo']);

                    });

                    // cboCEConvenioAutoriza.fillCombo('GetAutorizantesCC', { cc: items.cc }, false, null, (e) => {
                    //     cboCEConvenioAutoriza.val(items.autoriza);
                    //     cboCEConvenioAutoriza.trigger("change");
                    // });

                    txtCEConvenioComentarios.val(items.comentarios);
                    esAutorizar = items.esAutorizar;

                    dtAbonos.clear();
                    dtAbonos.rows.add(response.data.lstAcuerdoDet);
                    dtAbonos.draw();

                    // dtAbonos.rows(function (_idx, data, _node) {
                    //     if (moment(data.fechaDet).format("YYYY-MM-DD") > moment(data.fechaCreacion).add(1, "month").format("YYYY-MM-DD")) {
                    //         esAutorizar = true;
                    //     }
                    // });

                    // if (esAutorizar) {
                    //     divCEConvenioAutoriza.css("display", "initial");
                    // } else {
                    //     divCEConvenioAutoriza.css("display", "none");
                    // }

                    // fncFillComboFactura(rowData.factura, false, null, (e) => {
                    //     cboCEConvenioFactura.val(variables.facturaID);
                    //     cboCEConvenioFactura.trigger("change");
                    // });

                    btnCEConvenio.data("convenio", idAcuerdo);
                    txtCEConvenioTitulo.text("");
                    mdlCEConvenio.modal("show");

                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaCXCFacturas() {
            dtTablaCXCFacturas = tablaCXCFacturas.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                scrollY: "450px",
                scrollCollapse: true,
                paging: false,
                ordering: true,
                order: [[2, 'asc']],
                dom: 'Bfrtip',
                // buttons: [
                //     {
                //         text: 'Get selected data',
                //         action: function () {
                //             var count = dtTablaCXCFacturas.rows({ selected: true }).data();

                //             
                //         }
                //     }
                // ],
                columns: [
                    { data: 'factura', title: 'Factura' },
                    {
                        data: 'fechaOrig', title: 'Fecha', render: function (data, type, _row) {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    { data: 'concepto', title: 'Por Vencer' },
                    {
                        data: 'fecha', title: 'Fecha vencimiento', render: function (data, type, _row) {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'montoPronosticado', title: 'Pronóstico', render: function (data, _type, _row) { return maskNumero(parseFloat(data ?? 0).toFixed(2)); } },
                    { title: 'Abonos', render: function () { return maskNumero(0); } },
                    { data: 'total', title: 'Saldo', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    {
                        title: '', render: function (_data, _type, row) {
                            // return "<button class='btn btn-sm btn-primary verLog' title='Ver comentarios de factura' data-cliente='" + row.cliente + "'><i class='far fa-comments'></i></button>";
                            if (row.esCorteTemp) {
                                if (row.idAcuerdo == null) {
                                    return `<button class='btn btn-sm btn-success capturarConvenio' title='Agregar gestion de cobranza' data-cliente='" + row.cliente + "'><i class='fas fa-plus'></i></button>
                                            <button class='btn btn-sm btn-primary editFechaFactura' title='Modificar fecha de vencimiento' data-cliente='" + row.cliente + "'><i class="far fa-calendar-alt"></i></button>
                                            <button class='btn btn-sm btn-primary verLog' title='Ver comentarios de factura' data-cliente='" + row.cliente + "'><i class='far fa-comments'></i></button>`;

                                } else {
                                    if (row.esRemoved) {
                                        return `<button class='btn btn-sm btn-danger verComentario' title='Ver comentarios de factura removida' data-cliente='` + row.cliente + `'><i class='far fa-comments'></i></button>
                                                <button class='btn btn-sm btn-primary verLog' title='Ver comentarios de factura' data-cliente='" + row.cliente + "'><i class='far fa-comments'></i></button>`;

                                    } else {
                                        return `<button class='btn btn-sm btn-warning editarConvenio' title='Editar gestion de cobranza' data-cliente='` + row.cliente + `'><i class='far fa-edit'></i></button>
                                                <button class='btn btn-sm btn-primary verLog' title='Ver comentarios de factura' data-cliente='" + row.cliente + "'><i class='far fa-comments'></i></button>`;

                                    }

                                }
                                // return "<button class='btn btn-sm btn-success capturarConvenio' title='Agregar gestion de cobranza' data-cliente='" + row.cliente + "'><i class='fas fa-plus'></i></button>";
                            } else {
                                if (row.esCorte) {
                                    if (row.esRemoved) {
                                        return `<button class='btn btn-sm btn-danger verComentario' title='Ver comentarios de factura removida' data-cliente='` + row.cliente + `'><i class='far fa-comments'></i></button>
                                        <button class='btn btn-sm btn-primary verLog' title='Ver comentarios de factura' data-cliente='" + row.cliente + "'><i class='far fa-comments'></i></button>`;

                                    } else {
                                        if (row.idAcuerdo == null) {
                                            return `<button class='btn btn-sm btn-success capturarConvenio' title='Agregar gestion de cobranza' data-cliente='` + row.cliente + `'><i class='fas fa-plus'></i></button>
                                            <button class='btn btn-sm btn-primary editFechaFactura' title='Modificar fecha de vencimiento' data-cliente='` + row.cliente + `'><i class='far fa-calendar-alt'></i></button>
                                            <button class='btn btn-sm btn-primary verLog' title='Ver comentarios de factura' data-cliente='" + row.cliente + "'><i class='far fa-comments'></i></button>`;

                                        } else {
                                            return `<button class='btn btn-sm btn-warning editarConvenio' title='Editar gestion de cobranza' data-cliente='` + row.cliente + `'><i class='far fa-edit'></i></button>
                                            <button class='btn btn-sm btn-primary verLog' title='Ver comentarios de factura' data-cliente='" + row.cliente + "'><i class='far fa-comments'></i></button>`;

                                        }
                                    }

                                } else {
                                    if (row.esRemoved) {
                                        return `<button class='btn btn-sm btn-danger verComentario' title='Ver comentarios de factura removida' data-cliente='` + row.cliente + `'><i class='far fa-comments'></i></button>
                                        <button class='btn btn-sm btn-primary verLog' title='Ver comentarios de factura' data-cliente='" + row.cliente + "'><i class='far fa-comments'></i></button>`;

                                    } else {
                                        return `<button class='btn btn-sm btn-success capturarConvenio' title='Agregar gestion de cobranza' data-cliente='` + row.cliente + `' disabled><i class='fas fa-plus'></i></button>
                                        <button class='btn btn-sm btn-primary verLog' title='Ver comentarios de factura' data-cliente='" + row.cliente + "'><i class='far fa-comments'></i></button>`;

                                    }
                                }

                            }
                        }
                    },
                    {
                        data: 'factura',
                        render: function (data, type, row) {
                            // 
                            if (type === 'display') {
                                if (row.esCorte || row.esCorteTemp) {
                                    // row.addClass("selected");
                                    return '<input type="checkbox" checked>';
                                } else {
                                    return '<input type="checkbox">';
                                }
                            }
                            return data;
                        },
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: "7%", targets: 13 }
                ],
                //order: [[9, 'desc']],
                initComplete: function () {
                    tablaCXCFacturas.on("click", '.capturarConvenio', function () {
                        var row = dtTablaCXCFacturas.row($(this).parents('tr'));
                        const rowData = row.data();
                        // fncCapturarConvenio(rowData.datafactura.numcte, rowData.factura);
                        cboCEConvenioFactura.prop("disabled", false);
                        txtCEConvenioNombrecliente.prop("readonly", false);
                        fncMdlDefault();

                        dtAbonos.clear();
                        dtAbonos.draw();

                        txtCEConvenioNombrecliente.val(rowData.datafactura.responsable);
                        txtCEConvenioNumcte.val(rowData.datafactura.numcte);

                        cboCEConvenioFactura.append($('<option>', {
                            value: rowData.factura,
                            text: rowData.factura
                        }));
                        cboCEConvenioFactura.val(rowData.factura);
                        cboCEConvenioCC.val(rowData.datafactura.areaCuenta);

                        var day = ("0" + rowData.fecha.getDate()).slice(-2);
                        var month = ("0" + (rowData.fecha.getMonth() + 1)).slice(-2);
                        var _fecha = rowData.fecha.getFullYear()+"-"+(month)+"-"+(day);
                        
                        txtCEConvenioFechaoriginal.val(_fecha);
                        txtCEConvenioMonto.val(maskNumero(rowData.datafactura.monto));

                        //cboCEConvenioFactura.trigger("change");

                        //GetObjClientePorNombre(rowData.datafactura.numcte, true, { idFactura: rowData.factura });
                        //cboCEConvenioFactura.fillCombo("GetFacturasByCliente", { idCliente: rowData.datafactura.numcte }, false, null, (_e) => {
                        //    cboCEConvenioFactura.val(rowData.factura);
                        //    cboCEConvenioFactura.trigger("change");

                        //});

                        // fncFillComboFactura(rowData.factura, false, null, (e) => {
                        //     cboCEConvenioFactura.val(variables.facturaID);
                        //     cboCEConvenioFactura.trigger("change");
                        // });

                        esAutorizar = false;
                        // divCEConvenioAutoriza.css("display", "none");

                        // 
                        btnCEConvenio.data("dtrow", row.index());
                        btnCEConvenio.data("convenio", 0);
                        txtCEAcuerdoTitle.text("CAPTURA");
                        mdlCEConvenio.modal("show");
                    });

                    tablaCXCFacturas.on("click", '.editarConvenio', function () {
                        var row = dtTablaCXCFacturas.row($(this).parents('tr'));
                        const rowData = row.data();
                        // fncCapturarConvenio(rowData.datafactura.numcte, rowData.factura);
                        // 
                        btnCEConvenio.data("dtrow", row.index());
                        fncGetAcuerdoById(rowData.idAcuerdo);
                        txtCEAcuerdoTitle.text("ACTUALIZACION");
                    });

                    tablaCXCFacturas.on("click", '.verComentario', function () {
                        const rowData = dtTablaCXCFacturas.row($(this).parents('tr')).data();

                        mdlRemoveComentarios.modal("show");
                        txtRemoveComentario.val(rowData.comentarios);
                    });

                    tablaCXCFacturas.on("click", '.editFechaFactura', function () {
                        const row = dtTablaCXCFacturas.row($(this).parents('tr'));
                        const rowData = row.data();

                        fechaCEFacturaModFechaOG.val(moment(rowData.fechaOGVenc).format("YYYY-MM-DD"));
                        fechaCEFacturaModFechaNueva.val(moment(rowData.fecha).format("YYYY-MM-DD"));
                        // txtCEConvenioFechanueva.val();

                        btnCEConvenio.data("dtrow", row.index());
                        mdlCEFacturaMod.modal("show");
                    });

                    tablaCXCFacturas.on('click', '.verLog', function () {
                        let rowData = dtTablaCXCFacturas.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));

                        // btnCrearComentario.data("cc", "");
                        // btnCrearComentario.data("ccdesc", "");
                        // btnCrearComentario.data("cliente", rowData.numcte);
                        // btnCrearComentario.data("nomcliente", rowData.cliente);
                        btnCrearComentario.data("factura", rowData.factura);
                        txtCEComentarioTitle.text("Factura : " + rowData.factura);
                        // 

                        // 

                        fncGetComentarios(rowData.factura);
                        divVerComentario.modal("show");
                    });
                },
                footerCallback: function (_row, _data, _start, _end, _display) {
                    var api = this.api(), data;
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    totalPorVencer = api.column(4).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido15 = api.column(5).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido30 = api.column(6).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido60 = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido90 = api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencidoMas = api.column(9).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    pronostico = api.column(10).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total = api.column(12).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    $(api.column(3).footer()).html('TOTAL');
                    $(api.column(4).footer()).html(maskNumero(parseFloat(totalPorVencer).toFixed(2).toString()));
                    $(api.column(5).footer()).html(maskNumero(parseFloat(totalVencido15).toFixed(2).toString()));
                    $(api.column(6).footer()).html(maskNumero(parseFloat(totalVencido30).toFixed(2).toString()));
                    $(api.column(7).footer()).html(maskNumero(parseFloat(totalVencido60).toFixed(2).toString()));
                    $(api.column(8).footer()).html(maskNumero(parseFloat(totalVencido90).toFixed(2).toString()));
                    $(api.column(9).footer()).html(maskNumero(parseFloat(totalVencidoMas).toFixed(2).toString()));
                    $(api.column(10).footer()).html(maskNumero(parseFloat(pronostico).toFixed(2).toString()));
                    $(api.column(11).footer()).html('$0.00');
                    $(api.column(12).footer()).html(maskNumero(parseFloat(total).toFixed(2).toString()));
                },
                select: {
                    style: 'multi',
                    selector: 'td:last-child'
                },
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            columns: ':visible'
                        }

                    }
                ],
            });

            dtTablaCXCFacturas
                .on('select', function (_e, _dt, _type, indexes) {

                    if (indexes.length > 1) { //LISTA DE REGISTROS AL ABRIR CUALQUIER NUEVO 3ER NIVEL}

                        // 
                        for (i = 0; i < indexes.length; i++) {
                            let row = dtTablaCXCFacturas.row(indexes[i]);
                            let item = row.data();

                            //SI EL REGISTRO NO CONTIENE UN ACUERDO DESDE BASE DE DATOS SE BUSCA SI LA FACTURA ESTA RELACIONADA A UN ACUERDO CALLBACK PERRON
                            if (item.esCorteTemp && (item.idAcuerdo == null || item.idAcuerdo == 0)) {
                                fncGetAcuerdoByFactura(item, row.index(), (idAcuerdo, montoPronosticado, rowData, idRow) => {
                                    rowData.montoPronosticado = montoPronosticado ?? rowData.total; //SI LA FACTURA NO TIENE UN ACUERDO SE ASIGNA EL TOTAL COMO PRONOSTICO SINO SE ASIGNA EL MONTO MAS CERCANO A LA FECHA DE CORTE
                                    rowData.idAcuerdo = idAcuerdo;
                                    dtTablaCXCFacturas.row(idRow).data(rowData).invalidate().draw(); //EDITAR ROW
                                });
                            }

                            //SE AGREGA LA FACTURA A LA LISTA DE FACTURAS DE CORTE
                            if (lstFacturasCorte.indexOf(item.factura) == -1) {
                                lstFacturasCorte.push(item.factura);
                            }
                        }


                    } else { //AGREGAR REGISTRO SINGULAR AL CORTE

                        // 
                        var rowData = dtTablaCXCFacturas.row(indexes[0]).data();

                        // if (lstFacturasCorte.length == 0) {
                        //     lstFacturasCorte.push(rowData.factura);
                        // }

                        if (lstFacturasCorte.indexOf(rowData.factura) == -1) {
                            lstFacturasCorte.push(rowData.factura);
                        }

                        //#region AGREGAR AL CORTE

                        if (!rowData.esCorte) {
                            axios.post("CrearEditarCorte", { fechaCorte: inputCorte.datepicker('getDate'), lstFacturas: lstFacturasCorte }).then(response => {
                                let { success, items, message } = response.data;
                                if (success) {

                                }
                            }).catch(error => Alert2Error(error.message));
                        }

                        if (!rowData.esCorte) {
                            rowData.esCorteTemp = true; //CORTE EN MEMORIA VOLATIL
                            rowData.esCorte = true;
                        }

                        //SI EL REGISTRO NO CONTIENE UN ACUERDO DESDE BASE DE DATOS SE BUSCA SI LA FACTURA ESTA RELACIONADA A UN ACUERDO CALLBACK PERRON
                        if (rowData.esCorteTemp && (rowData.idAcuerdo == null || rowData.idAcuerdo == 0)) {
                            esConsultada = true;
                            fncGetAcuerdoByFactura(rowData, indexes[0], (idAcuerdo, montoPronosticado, rowDataCallBack, idRow) => {
                                rowDataCallBack.montoPronosticado = montoPronosticado ?? rowDataCallBack.total;
                                rowDataCallBack.esRemoved = false;
                                rowDataCallBack.idAcuerdo = idAcuerdo;

                                dtTablaCXCFacturas.row(idRow).data(rowDataCallBack).invalidate().draw();

                                if (chkCETipoTabla.prop("checked")) {
                                    let detClient = dtCXCCcClientes.rows().data().toArray();
                                    cargarTablaCXCClientes(detClient[0].detallesOrig, inputCorte.datepicker('getDate'), null, dtTablaCXCFacturas.rows().data().toArray());

                                    // 

                                    let detCc = dtCXCCc.rows().data().toArray();
                                    detClient = dtCXCCcClientes.rows().data().toArray();

                                    cargarTablaCXCCc(detCc[0].detallesOrig, inputCorte.datepicker('getDate'), detClient);
                                    cargarTablaCXC(detCc[0].detallesOrig, inputCorte.datepicker('getDate'), detClient);

                                } else {
                                    let detCC1 = dtTablaCXCAC.rows().data().toArray();
                                    // 
                                    cargarTablaCXCAC(detCC1[0].detallesOrig, inputCorte.datepicker('getDate'), dtTablaCXCFacturas.rows().data().toArray())

                                    detCC1 = dtTablaCXCAC.rows().data().toArray();
                                    let detCliente1 = dtTablaCXC.rows().data().toArray();

                                    cargarTablaCXC(detCliente1[0].detallesOrig, inputCorte.datepicker('getDate'), detCC1);
                                    cargarTablaCXCCc(detCliente1[0].detallesOrig, inputCorte.datepicker('getDate'), detCC1);
                                }

                            });

                        } else {
                            //SI LA FACTURA TIENE ACUERDO SE REGRESA SU VALOR DE PRONOSTICO QUE FUE ELIMNADO
                            if (rowData.montoPronosticado == 0) {
                                rowData.montoPronosticado = rowData.montoPronosticadoSaved ?? 0;
                            }

                            if (rowData.esCorte) {
                                fncGetAcuerdoByFactura(rowData, indexes[0], (idAcuerdo, montoPronosticado, rowDataCallBack, idRow) => {
                                    rowDataCallBack.montoPronosticado = montoPronosticado ?? rowDataCallBack.total;
                                    rowDataCallBack.esRemoved = false;
                                    rowDataCallBack.idAcuerdo = idAcuerdo;

                                    dtTablaCXCFacturas.row(idRow).data(rowDataCallBack).invalidate().draw();
                                    //SE VUELVE A GENERAR EL NIVEL 2 (TABLA DE CLIENTES) CON LOS NUEVOS VALORES DE PRONOSTICO Y TOTALIZADORES ACTUALIZADOS DE LA NUEVA FACTURA
                                    // if (chkCETipoTabla.prop("checked")) {
                                    //     cargarTablaCXCClientes(rowDataCallBack.detallesOrig, inputCorte.datepicker('getDate'), null, dtTablaCXCFacturas.rows().data().toArray());
                                    //     let detCc = dtCXCCc.rows().data().toArray();
                                    //     let detClient = dtCXCCcClientes.rows().data().toArray();
                                    //     cargarTablaCXCCc(detCc[0].detallesOrig, inputCorte.datepicker('getDate'), detClient);

                                    // } else {
                                    //     cargarTablaCXCAC(rowDataCallBack.detallesOrig, inputCorte.datepicker('getDate'), dtTablaCXCFacturas.rows().data().toArray())

                                    //     let detCC1 = dtTablaCXCAC.rows().data().toArray();
                                    //     let detCliente1 = dtTablaCXC.rows().data().toArray();

                                    //     cargarTablaCXC(detCliente1[0].detallesOrig, inputCorte.datepicker('getDate'), detCC1);
                                    // }

                                    if (chkCETipoTabla.prop("checked")) {
                                        let detClient = dtCXCCcClientes.rows().data().toArray();
                                        cargarTablaCXCClientes(detClient[0].detallesOrig, inputCorte.datepicker('getDate'), null, dtTablaCXCFacturas.rows().data().toArray());

                                        // 

                                        let detCc = dtCXCCc.rows().data().toArray();
                                        detClient = dtCXCCcClientes.rows().data().toArray();

                                        cargarTablaCXCCc(detCc[0].detallesOrig, inputCorte.datepicker('getDate'), detClient);
                                        cargarTablaCXC(detCc[0].detallesOrig, inputCorte.datepicker('getDate'), detClient);

                                    } else {
                                        let detCC1 = dtTablaCXCAC.rows().data().toArray();
                                        // 
                                        cargarTablaCXCAC(detCC1[0].detallesOrig, inputCorte.datepicker('getDate'), dtTablaCXCFacturas.rows().data().toArray())

                                        detCC1 = dtTablaCXCAC.rows().data().toArray();
                                        let detCliente1 = dtTablaCXC.rows().data().toArray();

                                        cargarTablaCXC(detCliente1[0].detallesOrig, inputCorte.datepicker('getDate'), detCC1);
                                        cargarTablaCXCCc(detCliente1[0].detallesOrig, inputCorte.datepicker('getDate'), detCC1);
                                    }

                                });

                            }
                        }

                        if (!rowData.esCorte) {

                            if (lstFacturasCorte.indexOf(rowData.factura) == -1) {
                                lstFacturasCorte.push(rowData.factura);
                            }


                            dtTablaCXCFacturas.row(indexes[0]).data(rowData).invalidate(); //EDITAR ROW

                            dtTablaCXCFacturas.draw();

                            let node = dtTablaCXCFacturas.row(indexes[0]).node();

                            setTimeout(function () {
                                $(node).addClass('highlight');

                                setTimeout(function () {
                                    $(node)
                                        .addClass('noHighlight')
                                        .removeClass('highlight');

                                    setTimeout(function () {
                                        $(node).removeClass('noHighlight');
                                    }, 1550);
                                }, 1500);
                            }, 20);
                        } else {
                            // 

                            if (lstFacturasCorte.indexOf(rowData.factura) == -1) {
                                lstFacturasCorte.push(rowData.factura);
                            }
                        }
                        //SE VUELVE A GENERAR EL NIVEL 2 (TABLA DE CLIENTES) CON LOS NUEVOS VALORES DE PRONOSTICO Y TOTALIZADORES ACTUALIZADOS DE LA NUEVA FACTURA
                        if (chkCETipoTabla.prop("checked")) {
                            let detClient = dtCXCCcClientes.rows().data().toArray();
                            cargarTablaCXCClientes(detClient[0].detallesOrig, inputCorte.datepicker('getDate'), null, dtTablaCXCFacturas.rows().data().toArray());

                            // 

                            let detCc = dtCXCCc.rows().data().toArray();
                            detClient = dtCXCCcClientes.rows().data().toArray();

                            cargarTablaCXCCc(detCc[0].detallesOrig, inputCorte.datepicker('getDate'), detClient);
                            cargarTablaCXC(detCc[0].detallesOrig, inputCorte.datepicker('getDate'), detClient);

                        } else {
                            let detCC1 = dtTablaCXCAC.rows().data().toArray();
                            // 
                            cargarTablaCXCAC(detCC1[0].detallesOrig, inputCorte.datepicker('getDate'), dtTablaCXCFacturas.rows().data().toArray())

                            detCC1 = dtTablaCXCAC.rows().data().toArray();
                            let detCliente1 = dtTablaCXC.rows().data().toArray();

                            cargarTablaCXC(detCliente1[0].detallesOrig, inputCorte.datepicker('getDate'), detCC1);
                            cargarTablaCXCCc(detCliente1[0].detallesOrig, inputCorte.datepicker('getDate'), detCC1);
                        }


                        //#endregion

                        //SE ACTUALIZA A MANO EL CHECK BOX DEL ROW
                        $(dtTablaCXCFacturas.cell(indexes, 13).node()).html(`<input type="checkbox" checked="">`);

                    }

                    // 
                })
                .on('deselect', function (_e, _dt, _type, indexes) {
                    var rowData = dtTablaCXCFacturas.row(indexes[0]).data();

                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "Remover factura",
                        input: rowData.esCorte ? 'textarea' : '',
                        width: '50%',
                        showCancelButton: true,
                        html: rowData.esCorte ? "<h3>¿Desea remover del <b>pronostico</b> la factura seleccionada?</h3><br><h3>Comentario:</h3>" : "<h3>¿Desea remover del PRONOSTICO la factura seleccionada?</h3>",
                        confirmButtonText: "Aceptar",
                        confirmButtonColor: "#449d44",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#5c636a",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.isConfirmed) {
                            // 

                            if (rowData.esCorte) {


                                if (lstFacturasCorte.indexOf(rowData.factura) != -1) {
                                    lstFacturasCorte.splice(lstFacturasCorte.indexOf(rowData.factura), 1);
                                }

                                // 


                                //Remover del corte en backend
                                fncRemoveFactura(rowData.factura, $('.swal2-textarea').val());
                            } else {
                                // 
                                if (lstFacturasCorte.indexOf(rowData.factura) != -1) {
                                    lstFacturasCorte.splice(lstFacturasCorte.indexOf(rowData.factura), 1);
                                }

                                // 

                            }

                            rowData.montoPronosticadoSaved = rowData.montoPronosticado;
                            rowData.montoPronosticado = 0;
                            rowData.esCorte = false;
                            rowData.esCorteTemp = false;
                            rowData.esRemoved = true;
                            rowData.comentarios = $('.swal2-textarea').val();

                            dtTablaCXCFacturas.row(indexes[0]).remove();
                            let row = dtTablaCXCFacturas.row.add(rowData).draw();

                            //SE VUELVE A GENERAR EL NIVEL 2 (TABLA DE CLIENTES) CON LOS NUEVOS VALORES DE PRONOSTICO Y TOTALIZADORES ACTUALIZADOS DE LA NUEVA FACTURA
                            // if (chkCETipoTabla.prop("checked")) {
                            //     cargarTablaCXCClientes(rowData.detallesOrig, inputCorte.datepicker('getDate'), null, dtTablaCXCFacturas.rows().data().toArray());
                            //     let detCc = dtCXCCc.rows().data().toArray();
                            //     let detClient = dtCXCCcClientes.rows().data().toArray();
                            //     cargarTablaCXCCc(detCc[0].detallesOrig, inputCorte.datepicker('getDate'), detClient);

                            // } else {
                            //     cargarTablaCXCAC(rowData.detallesOrig, inputCorte.datepicker('getDate'), dtTablaCXCFacturas.rows().data().toArray())

                            //     let detCC1 = dtTablaCXCAC.rows().data().toArray();
                            //     let detCliente1 = dtTablaCXC.rows().data().toArray();

                            //     cargarTablaCXC(detCliente1[0].detallesOrig, inputCorte.datepicker('getDate'), detCC1);
                            // }

                            if (chkCETipoTabla.prop("checked")) {
                                let detClient = dtCXCCcClientes.rows().data().toArray();
                                cargarTablaCXCClientes(detClient[0].detallesOrig, inputCorte.datepicker('getDate'), null, dtTablaCXCFacturas.rows().data().toArray());

                                // 

                                let detCc = dtCXCCc.rows().data().toArray();
                                detClient = dtCXCCcClientes.rows().data().toArray();

                                cargarTablaCXCCc(detCc[0].detallesOrig, inputCorte.datepicker('getDate'), detClient);
                                cargarTablaCXC(detCc[0].detallesOrig, inputCorte.datepicker('getDate'), detClient);

                            } else {
                                let detCC1 = dtTablaCXCAC.rows().data().toArray();
                                // 
                                cargarTablaCXCAC(detCC1[0].detallesOrig, inputCorte.datepicker('getDate'), dtTablaCXCFacturas.rows().data().toArray())

                                detCC1 = dtTablaCXCAC.rows().data().toArray();
                                let detCliente1 = dtTablaCXC.rows().data().toArray();

                                cargarTablaCXC(detCliente1[0].detallesOrig, inputCorte.datepicker('getDate'), detCC1);
                                cargarTablaCXCCc(detCliente1[0].detallesOrig, inputCorte.datepicker('getDate'), detCC1);
                            }

                            let node = row.node();

                            setTimeout(function () {
                                $(node).addClass('highlight');

                                setTimeout(function () {
                                    $(node)
                                        .addClass('noHighlight')
                                        .removeClass('highlight');

                                    setTimeout(function () {
                                        $(node).removeClass('noHighlight');
                                    }, 1550);
                                }, 1500);
                            }, 20);


                        } else {
                            $(dtTablaCXCFacturas.cell(indexes, 13).node()).html(`<input type="checkbox" checked="">`);
                            dtTablaCXCFacturas.row(indexes[0]).select();
                        }
                    });

                });
        }

        function fncCapturarConvenio(numcte, factura) {
            document.location.href = `/CuentasPorCobrar/CuentasPorCobrar/Convenios?cargarModal=${1}&clienteID=${numcte}&facturaID=${factura}`;
        }

        function cargarTablaCXCFacturas(detalles, fechaCorte, cc, numcte) {
            // 

            if (detalles == null) {
                return false;
            }
            else {
                dtTablaCXCFacturas.clear();
                var datosFinales = [];
                $.each(detalles, function (_i, n) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const factura = n.factura;
                    const concepto = n.concepto;
                    // console.log(n.fecha);
                    // n.fecha = typeof (n.fecha) == 'object' ? ("/Date(" + Date.parse(n.fecha).toString() + ")/") : n.fecha;
                    const fecha = new Date(parseInt(n.fecha.substr(6)));
                    var porVencer = 0;
                    if (fecha > fechaFin) porVencer = n.monto;
                    var vencido15 = 0;
                    if (fecha > fechaInicio && fecha <= fechaFin) vencido15 = n.monto;
                    var vencido30 = 0;
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    if (fecha > fechaInicio && fecha <= fechaFin) vencido30 = n.monto;
                    var vencido60 = 0;
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    if (fecha > fechaInicio && fecha <= fechaFin) vencido60 = n.monto;
                    var vencido90 = 0;
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    if (fecha > fechaInicio && fecha <= fechaFin) vencido90 = n.monto;
                    var vencidoMas = 0;
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    if (fecha < fechaInicio) vencidoMas = n.monto;
                    let montoPagado = 0;

                    var total = n.monto - montoPagado;
                    var valPronostico = n.montoPronosticado;
                    const group = {
                        esRemoved: n.esRemoved,
                        comentarios: n.comentarios,
                        idAcuerdo: n.idAcuerdo,
                        esCorte: n.esCorte,
                        esCorteTemp: false,
                        factura: factura,
                        concepto: concepto,
                        fecha: fecha,
                        porVencer: porVencer,
                        vencido15: vencido15,
                        vencido30: vencido30,
                        vencido60: vencido60,
                        vencido90: vencido90,
                        vencidoMas: vencidoMas,
                        montoPronosticado: valPronostico,
                        total: total,
                        datafactura: n,
                        detallesOrig: detalles,
                        fechaOrig: n.fechaOrig,
                        fechaOGVenc: n.fechaOGVenc,
                    };



                    if (lstFacturasCorte.includes(factura)) {
                        group.esCorteTemp = true;

                    }

                    datosFinales.push(group);
                    // if (!chkCETipoTabla.prop("checked")) {
                    //     if (n.areaCuenta == cc) {
                    //         datosFinales.push(group);
                    //     }
                    // } else {
                    //     if (n.numcte == numcte) {
                    //         datosFinales.push(group);
                    //     }
                    // }

                });
                datosFinales.sort((a, b) => b.total - a.total);
                dtTablaCXCFacturas.rows.add(datosFinales);
                dtTablaCXCFacturas.draw();

                dtTablaCXCFacturas.rows().every(function (rowIdx, tableLoop, rowLoop) {

                    let data = this.data();

                    if (data.esCorte || data.esCorteTemp) {
                        if (data.esRemoved) {
                        } else {
                            dtTablaCXCFacturas.row(rowIdx).select();
                        }
                    } else {
                    }
                });

                return true;
            }
        }

        function initTablaCXCAC() {
            dtTablaCXCAC = tablaCXCAC.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                scrollY: "450px",
                scrollCollapse: true,
                paging: false,
                ordering: false,
                dom: 'Bfrtip',
                columns: [
                    { title: '', render: function (_data, _type, row) { return "<button class='btn btn-sm btn-primary facturasXCC' data-cliente='" + row.cliente + "'><i class='fas fa-clipboard-list'></i></button>"; } },
                    { data: 'descripcionCC', title: 'Descripcion CC' },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, _type, _row) { return maskNumero(data); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, _type, _row) { return maskNumero(data); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, _type, _row) { return maskNumero(data); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, _type, _row) { return maskNumero(data); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, _type, _row) { return maskNumero(data); } },
                    { data: 'montoPronosticado', title: 'Pronóstico', render: function (data, _type, _row) { return maskNumero(data); } },
                    { title: 'Abonos', render: function () { return maskNumero(0); } },
                    { data: 'total', title: 'Saldo', render: function (data, _type, _row) { return maskNumero(data); } },
                    {
                        data: 'porcentaje',
                        title: '%',
                        render: function (data, _type, _row) {
                            return data.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                        }
                    },
                    // {
                    //     render: (data, type, row, meta) => {
                    //         return "<button class='btn btn-sm btn-primary verLog' title='Ver comentarios de factura removida' data-cliente='" + row.cliente + "'><i class='far fa-comments'></i></button>";
                    //     }
                    // },
                    // { data: 'cc', },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                //order: [[9, 'desc']],
                initComplete: function () {
                    tablaCXCAC.on('click', '.facturasXCC', function () {
                        let rowData = dtTablaCXCAC.row($(this).closest('tr')).data();

                        var detalles = rowData.detalles;
                        // 
                        const proveedor = $(this).attr("data-proveedor");
                        cargarTablaCXCFacturas(detalles, fechaCorteGeneral, rowData.cc, null);

                        titleModalCXCFacturas.text("CC: " + rowData.descripcionCC);
                        titleClienteModalCXCClientes.text(rowData.cliente);

                        $("#modalCXCFacturas").modal("show");
                    });
                    // tablaCXCAC.on('click', '.verLog', function () {
                    //     let rowData = dtTablaCXCAC.row($(this).closest('tr')).data();

                    //     btnCrearComentario.data("cc", rowData.cc);
                    //     btnCrearComentario.data("ccdesc", rowData.descripcionCC);
                    //     btnCrearComentario.data("cliente", "");
                    //     btnCrearComentario.data("nomcliente", "");

                    //     txtCEComentarioTitle.text(" CC : " + rowData.descripcionCC);
                    //     // 

                    //     fncGetComentarios(null, rowData.cc);
                    //     divVerComentario.modal("show");
                    // });

                },
                footerCallback: function (_row, _data, _start, _end, _display) {
                    var api = this.api(), data;
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    totalPorVencer = api.column(2).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido15 = api.column(3).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido30 = api.column(4).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido60 = api.column(5).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido90 = api.column(6).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencidoMas = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    pronostico = api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total = api.column(10).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalPorcentaje = (api.column(11).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0));
                    $(api.column(1).footer()).html('TOTAL');
                    $(api.column(2).footer()).html(maskNumero(parseFloat(totalPorVencer).toFixed(2).toString()));
                    $(api.column(3).footer()).html(maskNumero(parseFloat(totalVencido15).toFixed(2).toString()));
                    $(api.column(4).footer()).html(maskNumero(parseFloat(totalVencido30).toFixed(2).toString()));
                    $(api.column(5).footer()).html(maskNumero(parseFloat(totalVencido60).toFixed(2).toString()));
                    $(api.column(6).footer()).html(maskNumero(parseFloat(totalVencido90).toFixed(2).toString()));
                    $(api.column(7).footer()).html(maskNumero(parseFloat(totalVencidoMas).toFixed(2).toString()));
                    $(api.column(8).footer()).html(maskNumero(parseFloat(pronostico).toFixed(2).toString()));
                    $(api.column(9).footer()).html('$0.00');
                    $(api.column(10).footer()).html(maskNumero(parseFloat(total).toFixed(2).toString()));
                    $(api.column(11).footer()).html(parseFloat(totalPorcentaje).toFixed(2).toString() + '%');
                },
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            columns: ':visible'
                        }

                    }
                ],
            });
        }

        function cargarTablaCXCAC(detalles, fechaCorte, detallesReverse) {
            if (detalles == null) {
                return false;
            }
            else {
                if (detallesReverse != null && detallesReverse.length > 0) {
                    let facturasReverse = detallesReverse.map(function (e) { return e.factura; });
                    for (const item of detalles) {
                        let indexReverse = facturasReverse.indexOf(item.factura);
                        if (indexReverse != -1) {
                            let itemDet = detallesReverse[indexReverse];
                            item.montoPronosticado = itemDet.montoPronosticado;
                            item.esCorte = itemDet.esCorte;
                            item.esRemoved = itemDet.esRemoved;
                            item.comentarios = itemDet.comentarios;
                            item.idAcuerdo = itemDet.idAcuerdo;
                            item.fecha = typeof (itemDet.fecha) == 'object' ? ("/Date(" + Date.parse(itemDet.fecha).toString() + ")/") : itemDet.fecha;
                        }
                    }
                }

                var datosFinales = [];
                var auxDatosFinales = [];
                var totalDetalles = 0;
                $.each(detalles, function (_i, n) { totalDetalles += n.monto; });
                const grouped = groupBy(detalles, function (detalle) { return detalle.areaCuenta; });
                dtTablaCXCAC.clear();
                Array.from(grouped, function ([key, value]) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const descripcionCC = value[0].areaCuenta + " " + value[0].areaCuentaDesc;
                    var total = 0;
                    $.each(value, function (_i, n) { total += (n.monto - n.montoPagado); });
                    const porcentaje = totalDetalles > 0 ? (total * 100) / totalDetalles : 0;
                    const porVencerDetalles = jQuery.grep(value, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    var porVencer = 0;
                    if (porVencerDetalles.length > 0) $.each(porVencerDetalles, function (_i, n) { porVencer += n.monto; });
                    const vencido15Detalles = jQuery.grep(value, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido15 = 0;
                    if (vencido15Detalles.length > 0) $.each(vencido15Detalles, function (_i, n) { vencido15 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    const vencido30Detalles = jQuery.grep(value, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido30 = 0;
                    if (vencido30Detalles.length > 0) $.each(vencido30Detalles, function (_i, n) { vencido30 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    const vencido60Detalles = jQuery.grep(value, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido60 = 0;
                    if (vencido60Detalles.length > 0) $.each(vencido60Detalles, function (_i, n) { vencido60 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencido90Detalles = jQuery.grep(value, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido90 = 0;
                    if (vencido90Detalles.length > 0) $.each(vencido90Detalles, function (_i, n) { vencido90 += n.monto; });
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencidoMasDetalles = jQuery.grep(value, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    var vencidoMas = 0;
                    if (vencidoMasDetalles.length > 0) $.each(vencidoMasDetalles, function (_i, n) { vencidoMas += n.monto; });
                    let valPronostico = 0;
                    $.each(value, function (_i, n) { valPronostico += n.montoPronosticado; });
                    const grupo = {
                        descripcionCC: descripcionCC,
                        numcte: value[0].numcte,
                        porVencer: porVencer,
                        vencido15: vencido15,
                        vencido30: vencido30,
                        vencido60: vencido60,
                        vencido90: vencido90,
                        vencidoMas: vencidoMas,
                        montoPronosticado: valPronostico,
                        total: total,
                        porcentaje: porcentaje,
                        cc: value[0].areaCuenta,
                        detalles: value,
                        detallesOrig: detalles,
                        // fechaOrig: n.fechaOrig
                    };
                    datosFinales.push(grupo);
                });
                datosFinales.sort((a, b) => b.total - a.total);
                dtTablaCXCAC.rows.add(datosFinales);
                dtTablaCXCAC.draw();
                return true;
            }
        }

        function cargarTablaCXCCc(detalles, fechaCorte, detallesReverse) {//LVL1
            if (detalles == null) {
                return false;
            }
            else {

                if (detallesReverse != null && detallesReverse.length > 0) {
                    let facturasReverse = detallesReverse.map(function (e) { return e.factura; });
                    for (const item of detalles) {
                        let indexReverse = facturasReverse.indexOf(item.factura);
                        if (indexReverse != -1) {
                            let itemDet = detallesReverse[indexReverse];
                            item.montoPronosticado = itemDet.montoPronosticado;
                            item.esCorte = itemDet.esCorte;
                            item.esRemoved = itemDet.esRemoved;
                            item.comentarios = itemDet.comentarios;
                            item.idAcuerdo = itemDet.idAcuerdo;
                            item.montoPronosticado = itemDet.montoPronosticado;
                            item.fecha = typeof (itemDet.fecha) == 'object' ? ("/Date(" + Date.parse(itemDet.fecha).toString() + ")/") : itemDet.fecha;
                        }
                    }
                }

                var datosFinales = [];
                var auxDatosFinales = [];
                var totalDetalles = 0;
                $.each(detalles, function (_i, n) { totalDetalles += n.monto; });
                const grouped = groupBy(detalles, function (detalle) { return detalle.areaCuenta; });
                dtCXCCc.clear();
                Array.from(grouped, function ([key, value]) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const descripcionCC = value[0].areaCuenta + " " + value[0].areaCuentaDesc;
                    var total = 0;
                    $.each(value, function (_i, n) { total += (n.monto - n.montoPagado); });
                    const porcentaje = totalDetalles > 0 ? (total * 100) / totalDetalles : 0;
                    const porVencerDetalles = jQuery.grep(value, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    var porVencer = 0;
                    if (porVencerDetalles.length > 0) $.each(porVencerDetalles, function (_i, n) { porVencer += n.monto; });
                    const vencido15Detalles = jQuery.grep(value, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido15 = 0;
                    if (vencido15Detalles.length > 0) $.each(vencido15Detalles, function (_i, n) { vencido15 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    const vencido30Detalles = jQuery.grep(value, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido30 = 0;
                    if (vencido30Detalles.length > 0) $.each(vencido30Detalles, function (_i, n) { vencido30 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    const vencido60Detalles = jQuery.grep(value, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido60 = 0;
                    if (vencido60Detalles.length > 0) $.each(vencido60Detalles, function (_i, n) { vencido60 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencido90Detalles = jQuery.grep(value, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido90 = 0;
                    if (vencido90Detalles.length > 0) $.each(vencido90Detalles, function (_i, n) { vencido90 += n.monto; });
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencidoMasDetalles = jQuery.grep(value, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    var vencidoMas = 0;
                    if (vencidoMasDetalles.length > 0) $.each(vencidoMasDetalles, function (_i, n) { vencidoMas += n.monto; });
                    let valPronostico = 0;
                    $.each(value, function (_i, n) { valPronostico += n.montoPronosticado; });

                    const grupo = {
                        descripcionCC: descripcionCC,
                        numcte: value[0].numcte,
                        porVencer: porVencer,
                        vencido15: vencido15,
                        vencido30: vencido30,
                        vencido60: vencido60,
                        vencido90: vencido90,
                        vencidoMas: vencidoMas,
                        montoPronosticado: valPronostico,
                        total: total,
                        porcentaje: porcentaje,
                        cc: value[0].areaCuenta,
                        detalles: value,
                        detallesOrig: detalles,
                        // fechaOrig: n.fechaOrig
                    };
                    datosFinales.push(grupo);
                });
                datosFinales.sort((a, b) => b.total - a.total);
                dtCXCCc.rows.add(datosFinales);
                dtCXCCc.draw();
                return true;
            }
        }

        function recargarTotalizadoresCX() {
            let rowData = dtTablaCXP.data();
            var detallesRaw = rowData.map(function (x) { return x.detalles });
            var detalles = [].concat.apply([], detallesRaw);
            detalles = $.grep(detalles, function (el, index) { return (index == $.inArray(el, detalles) && el != null); });
            if (areasCuentaDetalle.length > 0) {
                detalles = $.grep(detalles, function (el, _index) {
                    return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0);
                });
            }
            var totalizador = 0;
            $.each(detalles, function (_i, n) { totalizador += n.monto; });
            $(".pCXP").text(parseFloat(totalizador).toFixed(2));

            rowData = dtTablaCXC.data();
            detallesRaw = rowData.map(function (x) { return x.detalles });
            detalles = [].concat.apply([], detallesRaw);
            detalles = $.grep(detalles, function (el, index) { return (index == $.inArray(el, detalles) && el != null); });
            if (areasCuentaDetalle.length > 0) {
                detalles = $.grep(detalles, function (el, _index) {
                    return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0);
                });
            }
            totalizador = 0;
            $.each(detalles, function (_i, n) { totalizador += n.monto; });
            $(".pCXC").text(parseFloat(totalizador).toFixed(2));
        }

        function cargarAC() {
            setResponsable();
            comboAC.fillComboAsync('cboObraEstimados', { divisionID: comboDivision.val() }, false, "TODOS");
            comboAC.find('option').get(0).remove();
            if (comboDivision.val() == "TODOS" && comboResponsable.val() == "TODOS") comboAC.find('option').prop('selected', false).change();
            else comboAC.find('option').prop('selected', true).change();
            comboAC.trigger({ type: 'select2:close' });

            comboAC.next(".select2-container").css("display", "none");
            var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
            if (seleccionados.length == 0) $("#spanComboAC").text("TODOS");
            else {
                if (seleccionados.length == 1) $("#spanComboAC").text($(seleccionados[0]).text().slice(1));
                else $("#spanComboAC").text(seleccionados.length.toString() + " Seleccionados");
            }
        }

        function groupBy(list, keyGetter) {
            const map = new Map();
            list.forEach(function (item) {
                const key = keyGetter(item);
                const collection = map.get(key);
                if (!collection) {
                    map.set(key, [item]);
                } else {
                    collection.push(item);
                }
            });
            return map;
        }

        function getNumberHTML(value) {
            return '<p class="' + (value != 0 ? 'noDesplegable' : '') + (value < 0 ? ' Danger' : '') + '" >' + parseFloat(value).toFixed(2) + '</p>';
        }

        function initTblCXCCc() {
            dtCXCCc = tblCXCCc.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                dom: "Bfrtip",
                columns: [
                    //render: function (data, type, row) { }
                    { title: '', render: function (_data, _type, row) { return "<button class='btn btn-sm btn-primary detalleClientes' data-cliente='" + row.cliente + "'><i class='fas fa-clipboard-list'></i></button>"; } },
                    { data: 'descripcionCC', title: 'Descripcion CC' },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'montoPronosticado', title: 'Pronóstico', render: function (data, _type, _row) { return maskNumero(parseFloat(data ?? 0).toFixed(2)); } },
                    { title: 'Abonos', render: function () { return maskNumero(0); } },
                    { data: 'total', title: 'Saldo', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    // { data: 'cc', },
                ],
                initComplete: function (_settings, _json) {
                    tblCXCCc.on('click', '.detalleClientes', function () {
                        let rowData = dtCXCCc.row($(this).closest('tr')).data();
                        cargarTablaCXCClientes(rowData.detalles, fechaCorteGeneral, rowData.cc);
                        modalCXCClientes.modal("show");
                        titleModalCXCClientes.text(rowData.descripcionCC);
                        titleClienteModalCXCClientes.text(rowData.cliente);
                    });
                    tblCXCCc.on('click', '.classBtn', function () {
                        let rowData = dtCXCCc.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                ],
                footerCallback: function (_row, _data, _start, _end, _display) {
                    var api = this.api(), data;
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    totalPorVencer = api.column(2).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido15 = api.column(3).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido30 = api.column(4).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido60 = api.column(5).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido90 = api.column(6).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencidoMas = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    pronostico = api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total = api.column(10).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    $(api.column(1).footer()).html('TOTAL');
                    $(api.column(2).footer()).html(maskNumero(parseFloat(totalPorVencer).toFixed(2).toString()));
                    $(api.column(3).footer()).html(maskNumero(parseFloat(totalVencido15).toFixed(2).toString()));
                    $(api.column(4).footer()).html(maskNumero(parseFloat(totalVencido30).toFixed(2).toString()));
                    $(api.column(5).footer()).html(maskNumero(parseFloat(totalVencido60).toFixed(2).toString()));
                    $(api.column(6).footer()).html(maskNumero(parseFloat(totalVencido90).toFixed(2).toString()));
                    $(api.column(7).footer()).html(maskNumero(parseFloat(totalVencidoMas).toFixed(2).toString()));
                    $(api.column(8).footer()).html(maskNumero(parseFloat(pronostico).toFixed(2).toString()));
                    $(api.column(9).footer()).html('$0.00');
                    $(api.column(10).footer()).html(maskNumero(parseFloat(total).toFixed(2).toString()));
                },
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            columns: ':visible'
                        }

                    }
                ],
                // select: {
                //     style: 'multi',
                //     selector: 'td:last-child'
                // }
            });
        }

        function initTblCXCCcClientes() {
            dtCXCCcClientes = tblCXCCcClientes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                dom: "Bfrtip",
                columns: [
                    //render: function (data, type, row) { }
                    { title: '', render: function (_data, _type, row) { return "<button class='btn btn-sm btn-primary detalleFacturas' data-cliente='" + row.cliente + "'><i class='fas fa-layer-group'></i></button>"; } },
                    // { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-primary verFacturas' data-cliente='" + row.cliente + "'><i class='fas fa-clipboard-list'></i></button>"; } },
                    { data: 'cliente', title: 'Cliente' },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'montoPronosticado', title: 'Pronóstico', render: function (data, _type, _row) { return maskNumero(parseFloat(data ?? 0).toFixed(2)); } },
                    { title: 'Abonos', render: function () { return maskNumero(0); } },
                    { data: 'total', title: 'Saldo', render: function (data, _type, _row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    {
                        data: 'porcentaje',
                        title: '%',
                        render: function (data, _type, _row) {
                            return data.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                        }
                    },
                    // {
                    //     render: (data, type, row, meta) => {
                    //         return "<button class='btn btn-sm btn-primary verLog' title='Ver comentarios de factura removida' data-cliente='" + row.cliente + "'><i class='far fa-comments'></i></button>";
                    //     }
                    // },
                    // { data: 'numcte', },
                ],
                initComplete: function (_settings, _json) {
                    tblCXCCcClientes.on('click', '.detalleFacturas', function () {
                        let rowData = dtCXCCcClientes.row($(this).closest('tr')).data();

                        var detalles = rowData.detalles;
                        // 
                        const proveedor = $(this).attr("data-proveedor");
                        cargarTablaCXCFacturas(detalles, fechaCorteGeneral, null, rowData.numcte);

                        // titleModalCXCFacturas.text("Cliente: " + rowData.cliente);
                        titleModalCXCFacturas.text("CC: " + rowData.descripcionCC);
                        titleClienteModalCXCClientes.text(rowData.cliente);
                        titleClienteModalCXCClientes2.text(rowData.cliente);

                        $("#modalCXCFacturas").modal("show");

                    });
                    tblCXCCcClientes.on('click', '.verLog', function () {
                        let rowData = dtCXCCcClientes.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));

                        btnCrearComentario.data("cc", "");
                        btnCrearComentario.data("ccdesc", "");
                        btnCrearComentario.data("cliente", rowData.numcte);
                        btnCrearComentario.data("nomcliente", rowData.cliente);
                        txtCEComentarioTitle.text(" Cliente : " + rowData.cliente);
                        // 

                        // 

                        fncGetComentarios(rowData.numcte, null);
                        divVerComentario.modal("show");
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },

                ],
                footerCallback: function (_row, _data, _start, _end, _display) {
                    var api = this.api(), data;
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    totalPorVencer = api.column(2).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido15 = api.column(3).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido30 = api.column(4).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido60 = api.column(5).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido90 = api.column(6).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencidoMas = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    pronostico = api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total = api.column(10).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalPorcentaje = (api.column(11).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0));
                    $(api.column(1).footer()).html('TOTAL');
                    $(api.column(2).footer()).html(maskNumero(parseFloat(totalPorVencer).toFixed(2).toString()));
                    $(api.column(3).footer()).html(maskNumero(parseFloat(totalVencido15).toFixed(2).toString()));
                    $(api.column(4).footer()).html(maskNumero(parseFloat(totalVencido30).toFixed(2).toString()));
                    $(api.column(5).footer()).html(maskNumero(parseFloat(totalVencido60).toFixed(2).toString()));
                    $(api.column(6).footer()).html(maskNumero(parseFloat(totalVencido90).toFixed(2).toString()));
                    $(api.column(7).footer()).html(maskNumero(parseFloat(totalVencidoMas).toFixed(2).toString()));
                    $(api.column(8).footer()).html(maskNumero(parseFloat(pronostico).toFixed(2).toString()));
                    $(api.column(9).footer()).html('$0.00');
                    $(api.column(10).footer()).html(maskNumero(parseFloat(total).toFixed(2).toString()));
                    $(api.column(11).footer()).html(parseFloat(totalPorcentaje).toFixed(2).toString() + '%');
                },
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            columns: ':visible'
                        }

                    }
                ],
                // select: {
                //     style: 'multi',
                //     selector: 'td:last-child'
                // }
            });
        }

        function cargarTablaCXCClientes(detalles, fechaCorte, clienteCC, detallesReverse) {
            if (detalles == null) {
                return false;
            }
            else {
                if (detallesReverse != null && detallesReverse.length > 0) {
                    let facturasReverse = detallesReverse.map(function (e) { return e.factura; });
                    for (const item of detalles) {
                        let indexReverse = facturasReverse.indexOf(item.factura);
                        if (indexReverse != -1) {
                            let itemDet = detallesReverse[indexReverse];
                            item.montoPronosticado = itemDet.montoPronosticado;
                            item.esCorte = itemDet.esCorte;
                            item.esRemoved = itemDet.esRemoved;
                            item.comentarios = itemDet.comentarios;
                            item.idAcuerdo = itemDet.idAcuerdo;
                            // console.log(item.fecha);
                            item.fecha = typeof (itemDet.fecha) == 'object' ? ("/Date(" + Date.parse(itemDet.fecha).toString() + ")/") : itemDet.fecha;
                            // console.log(itemDet.fecha);
                        }
                    }
                }

                var datosFinales = [];
                var auxDatosFinales = [];
                var totalDetalles = 0;
                $.each(detalles, function (_i, n) { totalDetalles += n.monto; });
                const grouped = groupBy(detalles, function (detalle) { return detalle.responsable; });
                dtCXCCcClientes.clear();
                Array.from(grouped, function ([key, value]) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const cliente = value[0].responsable;
                    let ccDet = value[0].areaCuenta;
                    let descripcionCC = value[0].areaCuenta + " " + value[0].areaCuentaDesc;
                    var total = 0;
                    var detallesFiltrados = value;
                    //if(areasCuentaDetalle.length > 0) {
                    //    detallesFiltrados = $.grep(detallesFiltrados,function(el,index){ 
                    //        return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0); 
                    //    });
                    //}
                    $.each(detallesFiltrados, function (_i, n) { total += (n.monto - n.montoPagado); });
                    const porcentaje = totalDetalles > 0 ? (total * 100) / totalDetalles : 0;
                    const porVencerDetalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    var porVencer = 0;
                    if (porVencerDetalles.length > 0) $.each(porVencerDetalles, function (_i, n) { porVencer += n.monto; });
                    const vencido15Detalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido15 = 0;
                    if (vencido15Detalles.length > 0) $.each(vencido15Detalles, function (_i, n) { vencido15 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    const vencido30Detalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido30 = 0;
                    if (vencido30Detalles.length > 0) $.each(vencido30Detalles, function (_i, n) { vencido30 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    const vencido60Detalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido60 = 0;
                    if (vencido60Detalles.length > 0) $.each(vencido60Detalles, function (_i, n) { vencido60 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencido90Detalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido90 = 0;
                    if (vencido90Detalles.length > 0) $.each(vencido90Detalles, function (_i, n) { vencido90 += n.monto; });
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencidoMasDetalles = jQuery.grep(detallesFiltrados, function (n, _i) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    var vencidoMas = 0;
                    if (vencidoMasDetalles.length > 0) $.each(vencidoMasDetalles, function (_i, n) { vencidoMas += n.monto; });
                    let valPronostico = 0;
                    $.each(detallesFiltrados, function (_i, n) { valPronostico += n.montoPronosticado; });
                    const grupo = { descripcionCC: descripcionCC, cliente: cliente, numcte: value[0].numcte, porVencer: porVencer, vencido15: vencido15, vencido30: vencido30, vencido60: vencido60, vencido90: vencido90, vencidoMas: vencidoMas, montoPronosticado: valPronostico, total: total, porcentaje: porcentaje, cc: ccDet, detalles: value, detallesOrig: detalles };

                    auxDatosFinales.push(grupo);

                    // if (cc == clienteCC) {
                    //     auxDatosFinales.push(grupo);

                    // }
                });
                auxDatosFinales.sort((a, b) => b.total - a.total);
                dtCXCCcClientes.rows.add(auxDatosFinales);
                dtCXCCcClientes.draw();
                return true;
            }
        }
        //#endregion

        //#region MODAL CRUD
        function fncGetObjAbonos() {
            let rowData = tblAbonos.DataTable().rows().data().toArray();

            let abonos = new Array();

            for (let index = 0; index < rowData.length; index++) {
                if (rowData[index].esNuevo) {

                    abonos.push(rowData[index]);
                }
            }

            return abonos;
        }

        function fncGetObjAcuerdo() {
            let strMensajeError = "";

            // if (txtCEConvenioComentarios.val() == "") { txtCEConvenioComentarios.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            // if (esAutorizar) {
            //     if (cboCEConvenioAutoriza.val() == "") { cboCEConvenioAutoriza.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            // }

            if (strMensajeError == "") {
                let obj = {
                    id: btnCEConvenio.data("convenio"),
                    numcte: txtCEConvenioNumcte.val(),
                    nombreCliente: txtCEConvenioNombrecliente.val(),
                    cc: cboCEConvenioCC.val(),
                    idFactura: cboCEConvenioFactura.val(),
                    monto: unmaskNumero(txtCEConvenioMonto.val()),
                    fechaOriginal: txtCEConvenioFechaoriginal.val(),
                    comentarios: txtCEConvenioComentarios.val(),
                    esPagado: false,
                    autoriza: 0,
                    lstAbonos: lstAbonos,
                }

                return obj;
            } else {
                Alert2Warning(strMensajeError);
                return "";
            }


        }

        function fncCrearEditarConvenio() {
            let lstAbonos = fncGetObjAbonos();

            let obj = {
                id: btnCEConvenio.data("convenio"),
                numcte: txtCEConvenioNumcte.val(),
                nombreCliente: txtCEConvenioNombrecliente.val(),
                cc: cboCEConvenioCC.val(),
                ccDescripcion: $("#cboCEConvenioCC option:selected").text(),
                idFactura: cboCEConvenioFactura.val(),
                monto: unmaskNumero(txtCEConvenioMonto.val()),
                fechaOriginal: txtCEConvenioFechaoriginal.val(),
                comentarios: txtCEConvenioComentarios.val(),
                esPagado: false,
                autoriza: 0,
                lstAbonos: lstAbonos,
                esAutorizar: esAutorizar,
                fechaCorte: inputCorte.datepicker('getDate'),
            }

            // if (esAutorizar && cboCEConvenioAutoriza.val() == "") {
            //     cboCEConvenioAutoriza.css("border", "2px solid red");
            //     Alert2Warning("El acuerdo de cobranza necesita un autorizador");
            //     return "";
            // }

            if (dtAbonos.data().count() > 0) {
                axios.post("CrearEditarConvenios", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //CODE
                        // 
                        let rowID = btnCEConvenio.data("dtrow");
                        let row = dtTablaCXCFacturas.row(rowID);
                        let rowData = row.data();

                        if (obj.id > 0) {
                            // 

                            // rowData.idAcuerdo = items;
                            rowData.montoPronosticado = response.data.montoProgramado;

                            row.data(rowData).invalidate();
                            dtTablaCXCFacturas.draw();

                            // 

                            Alert2Exito("Acuerdo actualizado con exito");
                            mdlCEConvenio.modal("hide");

                        } else {

                            rowData.idAcuerdo = items;
                            rowData.montoPronosticado = response.data.montoProgramado;

                            row.data(rowData).invalidate();
                            dtTablaCXCFacturas.draw();

                            Alert2Exito("Acuerdo creado con exito");
                            mdlCEConvenio.modal("hide");
                        }

                        if (chkCETipoTabla.prop("checked")) {
                            let detClient = dtCXCCcClientes.rows().data().toArray();
                            cargarTablaCXCClientes(detClient[0].detallesOrig, inputCorte.datepicker('getDate'), null, dtTablaCXCFacturas.rows().data().toArray());

                            // 

                            let detCc = dtCXCCc.rows().data().toArray();
                            detClient = dtCXCCcClientes.rows().data().toArray();

                            cargarTablaCXCCc(detCc[0].detallesOrig, inputCorte.datepicker('getDate'), detClient);
                            cargarTablaCXC(detCc[0].detallesOrig, inputCorte.datepicker('getDate'), detClient);

                        } else {
                            let detCC1 = dtTablaCXCAC.rows().data().toArray();
                            // 
                            cargarTablaCXCAC(detCC1[0].detallesOrig, inputCorte.datepicker('getDate'), dtTablaCXCFacturas.rows().data().toArray())

                            detCC1 = dtTablaCXCAC.rows().data().toArray();
                            let detCliente1 = dtTablaCXC.rows().data().toArray();

                            cargarTablaCXC(detCliente1[0].detallesOrig, inputCorte.datepicker('getDate'), detCC1);
                            cargarTablaCXCCc(detCliente1[0].detallesOrig, inputCorte.datepicker('getDate'), detCC1);
                        }
                        // fncGetConvenios();
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("El acuerdo no tiene ningun abono capturado")
            }

        }

        function fncMdlDefault() {
            txtCEConvenioNumcte.val("");
            txtCEConvenioNombrecliente.val("");
            cboCEConvenioCC.val("");
            cboCEConvenioCC.trigger("change");
            cboCEConvenioPeriodo.val("");
            cboCEConvenioPeriodo.trigger("change");
            cboCEConvenioFactura.val("");
            cboCEConvenioFactura.trigger("change");
            txtCEConvenioMonto.val("");
            txtCEConvenioMontonuevo.val("");
            txtCEConvenioFechaoriginal.val("");
            txtCEConvenioFechanueva.val("");
            txtCEConvenioComentarios.val("");
            // cboCEConvenioAutoriza.val("");
            // cboCEConvenioAutoriza.trigger("change");
        }

        function fncGetInfoFacturaById(idFactura, getInfo) {
            axios.post("GetInfoFacturaById", { idFactura }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    cboCEConvenioCC.val(items.areaCuenta);
                    txtCEConvenioFechaoriginal.val(moment(items.fecha).format("YYYY-MM-DD")); //FECHA VENCIMIENTO
                    txtCEConvenioMonto.val(maskNumero(items.monto));

                    // if (!getInfo) {
                    //     cboCEConvenioAutoriza.fillCombo('GetAutorizantesCC', { cc: items.areaCuenta }, false);

                    // }
                }
            }).catch(error => Alert2Error(error.message));
        }

        function SetAutorizanteConvenios(_esEdit, rowData, _success) {
            cboCEConvenioAutoriza.val(rowData.autoriza);
            cboCEConvenioAutoriza.trigger("change");

        }

        function initTblAbonos() {
            dtAbonos = tblAbonos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                order: [[1, 'asc']],
                columns: [
                    //render: function (data, type, row) { }
                    {
                        data: 'abonoDet', title: 'Monto',
                        render: function (data, type, row) {
                            return maskNumero(data ?? 0);
                        }
                    },
                    {
                        data: 'fechaDet', title: 'Fecha',
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    {
                        render: (data, type, row, meta) => {
                            return `<button title='Eliminar abono.' class="btn btn-danger eliminarAbono btn-xs"><i class="far fa-trash-alt"></i></button>&nbsp;`;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblAbonos.on('click', '.classBtn', function () {
                        let rowData = dtAbonos.row($(this).closest('tr')).data();
                    });
                    tblAbonos.on('click', '.eliminarAbono', function () {
                        let rowData = dtAbonos.row($(this).closest('tr')).data();


                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncRemoveAcuerdoDet(rowData.id, () => {
                            dtAbonos.row($(this).closest('tr')).remove().draw();
                            esAutorizar = false;

                            let nextMonth = moment().add(1, "month").format("YYYY-MM-DD");

                            if (rowData.id == 0) {
                                dtAbonos.rows(function (_idx, data, _node) {
                                    if (moment(data.fechaDet).format("YYYY-MM-DD") > nextMonth) {
                                        esAutorizar = true;
                                    }
                                });
                            } else {
                                dtAbonos.rows(function (_idx, data, _node) {
                                    if (moment(data.fechaDet).format("YYYY-MM-DD") > moment(data.fechaCreacion).add(1, "month").format("YYYY-MM-DD")) {
                                        esAutorizar = true;
                                    }
                                });
                            }

                            // if (esAutorizar) {
                            //     divCEConvenioAutoriza.css("display", "initial");
                            // } else {
                            //     divCEConvenioAutoriza.css("display", "none");
                            // }

                        }));

                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncRemoveAcuerdoDet(idAcuerdoDet, callbackRemoverDT) {
            if (idAcuerdoDet == 0) {
                callbackRemoverDT();

            } else {
                axios.post("RemoveAcuerdoDet", { idAcuerdoDet }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //CODE...
                        Alert2Exito("Abono eliminado con exito");
                        callbackRemoverDT();
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncRemoveFactura(idFactura, comentarioRemove) {
            axios.post("RemoveFactura", { idFactura, comentarioRemove }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Factura removida ");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetAcuerdoByFactura(rowData, idRow, callbackDatatable) {
            axios.post("GetAcuerdoByFactura", { idFactura: rowData.factura }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    callbackDatatable(items.id, response.data.montoPronosticado, rowData, idRow);
                } else {
                    callbackDatatable(null, null, rowData, idRow);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region MODAL COMENTARIOS

        function fncGetComentarios(factura) {
            axios.post("GetComentarios", {
                factura
            }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let comentarios = [];

                    for (const item of items) {
                        comentarios.push({
                            fecha: moment(item.fechaCreacion).format("DD/MM/YYYY"),
                            usuarioNombre: item.nombreUsuarioCreacion,
                            comentario: item.comentario,
                        });
                    }

                    txtCEComentarioFechaCompromiso.val(moment().format("YYYY-MM-DD"));
                    cboCEComentarioTipo.val("");
                    cboCEComentarioTipo.trigger("change");

                    setComentarios(comentarios);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function setComentarios(data) {
            var htmlComentario = "";
            $.each(data, function (i, e) {
                htmlComentario += "<li class='comentario' data-id='100'>";
                htmlComentario += "    <div class='timeline-item'>";
                htmlComentario += "        <span class='time'><i class='fa fa-clock-o'></i>" + e.fecha + "</span>";
                htmlComentario += "        <h3 class='timeline-header'><a href='#'>" + e.usuarioNombre + "</a></h3>";
                htmlComentario += "        <div class='timeline-body'>";
                htmlComentario += "             " + e.comentario;
                htmlComentario += "        </div>";
                htmlComentario += "    </div>";
                htmlComentario += "</li>";
            });
            ulComentarios.html(htmlComentario);
        }

        function fncCrearEditarComentarios(factura) {

            if (cboCEComentarioTipo.val() == "") {
                Alert2Warning("Seleccione el tipo de comentario");
                return "";
            }

            if (txtComentarios.val() == "") {
                Alert2Warning("Ingrese un comentario");
                return "";
            }

            axios.post("CrearEditarComentarios", {
                comentario: txtComentarios.val(),
                factura: factura,
                clienteID: null,
                nomCliente: null,
                cc: null,
                ccDesc: null,
                tipoComentario: cboCEComentarioTipo.val(),
                fechaCompromiso: cboCEComentarioTipo.val() == 2 ? txtCEComentarioFechaCompromiso.val() : null,

            }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    txtComentarios.val("");
                    fncGetComentarios(factura);
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#endregion

        //#region MODAL SEG COMENTARIOS

        function initTblSegComentariosCC() {
            dtSegComentariosCC = tblSegComentariosCC.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                order: [[4, 'asc'], [3, 'asc']],
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'nombreUsuarioCreacion', title: 'USUARIO CAPTURA' },
                    { data: 'comentario', title: 'COMENTARIO' },
                    { data: 'descTipoComentario', title: 'TIPO' },
                    { data: 'factura', title: 'FACTURA' },
                    {
                        data: 'fechaCompromiso', title: "FECHA COMPROMISO", render: (data, type, row, meta) => {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblSegComentariosCC.on('click', '.classBtn', function () {
                        let rowData = dtSegComentariosCC.row($(this).closest('tr')).data();
                    });
                    tblSegComentariosCC.on('click', '.classBtn', function () {
                        let rowData = dtSegComentariosCC.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTblSegComentariosCliente() {
            dtSegComentariosCliente = tblSegComentariosCliente.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                order: [[4, 'asc'], [3, 'asc']],
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'nombreUsuarioCreacion', title: 'USUARIO CAPTURA' },
                    { data: 'comentario', title: 'COMENTARIO' },
                    { data: 'descTipoComentario', title: 'TIPO' },
                    { data: 'nomCliente', title: 'CLIENTE' },
                    {
                        data: 'fechaCompromiso', title: "FECHA COMPROMISO", render: (data, type, row, meta) => {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblSegComentariosCliente.on('click', '.classBtn', function () {
                        let rowData = dtSegComentariosCliente.row($(this).closest('tr')).data();
                    });
                    tblSegComentariosCliente.on('click', '.classBtn', function () {
                        let rowData = dtSegComentariosCliente.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetComentariosVencer() {
            axios.post("GetComentariosVencer").then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    mdlSegComentarios.modal("show");

                    //AHORA ES POR FACTURA
                    dtSegComentariosCC.clear();
                    dtSegComentariosCC.rows.add(response.data.comentariosCC);
                    dtSegComentariosCC.draw();

                    // dtSegComentariosCliente.clear();
                    // dtSegComentariosCliente.rows.add(response.data.comentariosCliente);
                    // dtSegComentariosCliente.draw();

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncMostrarCarrucelVencer() {

            axios.post("GetComentariosVencer").then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    let first = false;
                    let numVencen = 0;

                    //#region POR CC

                    response.data.comentariosCC.forEach(e => {
                        if (e.esVenceMañana || e.esVencePasado) {
                            carouselNotisInner.html(carouselNotisInner.html() +
                                `
                            <div class="item ${first ? "" : "active"}" style="height: 300px;">
                                <div class="row" style="text-align: center;">
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-8">
                                        <h3 id="carouselNombreEmp${e.fatura}" > ${e.fatura} - Tipo: ${e.descTipoComentario}</h3>
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                </div>
                                <div class="row" style="text-align: center;">
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-4">
                                        <h3 id="">Capturo: </h3>
                                        <h4 id="">${e.nombreUsuarioCreacion}</h4>
                                    </div>
                                    <div class="col-sm-4">
                                        <h3 id="carouselClaveEmp">Fecha Compromiso: </h3>
                                        <h3 id="carouselClaveEmp">${moment(e.fechaCompromiso).format("DD/MM/YYYY")}</h3>
                                    </div>
                                    
                                    <div class="col-sm-2">
                                    </div>
                                </div>
                                <div class="row" style="text-align: center;">
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-8">
                                        <textarea class="" id="" cols="40" rows="6" readonly style="width: 100%">${e.comentario}</textarea>
                                    </div>
                                    
                                    <div class="col-sm-2">
                                    </div>
                                </div>
                            </div>
                        `
                            );
                            first = true;
                            numVencen++;
                        }
                    });

                    //#endregion 

                    //#region POR CLIENTE

                    // response.data.comentariosCliente.forEach(e => {
                    //     if (e.esVenceMañana || e.esVencePasado) {
                    //         carouselNotisInner.html(carouselNotisInner.html() +
                    //             `
                    //             <div class="item ${first ? "" : "active"}" style="height: 300px;">
                    //                 <div class="row" style="text-align: center;">
                    //                     <div class="col-sm-2">
                    //                     </div>
                    //                     <div class="col-sm-8">
                    //                         <h3 id="carouselNombreEmp${e.clienteID}" >${e.nomCliente} - Tipo: ${e.descTipoComentario}</h3>
                    //                     </div>
                    //                     <div class="col-sm-2">
                    //                     </div>
                    //                 </div>
                    //                 <div class="row" style="text-align: center;">
                    //                     <div class="col-sm-2">
                    //                     </div>
                    //                     <div class="col-sm-4">
                    //                         <h3 id="">Capturo: </h3>
                    //                         <h4 id="">${e.nombreUsuarioCreacion}</h4>
                    //                     </div>
                    //                     <div class="col-sm-4">
                    //                         <h3 id="carouselClaveEmp">Fecha Compromiso: </h3>
                    //                         <h3 id="carouselClaveEmp">${moment(e.fechaCompromiso).format("DD/MM/YYYY")}</h3>
                    //                     </div>

                    //                     <div class="col-sm-2">
                    //                     </div>
                    //                 </div>
                    //                 <div class="row" style="text-align: center;">
                    //                     <div class="col-sm-2">
                    //                     </div>
                    //                     <div class="col-sm-8">
                    //                         <textarea class="" id="" cols="40" rows="6" readonly style="width: 100%">${e.comentario}</textarea>
                    //                     </div>

                    //                     <div class="col-sm-2">
                    //                     </div>
                    //                 </div>
                    //             </div>
                    //         `
                    //         );
                    //         first = true;
                    //         numVencen++;
                    //     }

                    // });

                    //#endregion

                    // $(".btnNotiCapturar").on("click", function () {
                    //     // 
                    //     mdlCarousel.modal("hide");
                    //     mdlCEIncap.modal("show");

                    //     spanCEIncapTitulo.text("CAPTURA");

                    //     txtCEIncapClaveEmpleado.val($(this).data("clave"));
                    //     fncGetDatosEmpleado();
                    // });

                    if (numVencen > 0) {
                        mdlCarousel.modal("show");
                        carouselNotis.carousel({
                            interval: false,
                        });
                    }


                }
            }).catch(error => Alert2Error(error.message));

        }
        //#endregion

        //#region MODAL FACTURA MOD

        function fncGuardarFacturaMod() {

            let rowID = btnCEConvenio.data("dtrow");
            let row = dtTablaCXCFacturas.row(rowID);
            let rowData = row.data();

            let obj = {
                factura: rowData.factura,
                fechaVencimientoOG: fechaCEFacturaModFechaOG.val(),
                fechaNueva: fechaCEFacturaModFechaNueva.val(),
            }

            axios.post("GuardarFacturaMod", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    rowData.fecha = moment(fechaCEFacturaModFechaNueva.val())._d;

                    row.data(rowData).invalidate();
                    dtTablaCXCFacturas.draw();

                    let fechaInput = inputCorte.datepicker('getDate');
                    let arrTblFacturas = dtTablaCXCFacturas.rows().data().toArray();

                    //ACTUALIZAR TODAS LAS TABLAS
                    if (chkCETipoTabla.prop("checked")) {
                        let detClient = dtCXCCcClientes.rows().data().toArray();
                        cargarTablaCXCClientes(detClient[0].detallesOrig, fechaInput, null, arrTblFacturas);

                        let detCc = dtCXCCc.rows().data().toArray();
                        detClient = dtCXCCcClientes.rows().data().toArray();

                        cargarTablaCXCCc(detCc[0].detallesOrig, fechaInput, detClient);
                        cargarTablaCXC(detCc[0].detallesOrig, fechaInput, detClient);

                        let clientes = detClient.map(function (e) { return e.numcte });
                        let indexCliente = clientes.indexOf(rowData.datafactura.numcte);

                        if (indexCliente == -1) {
                            // clientesOrig = rowData.detallesOrig.map(function (e) { return e.numcte });
                            clientes = detClient.map(function (e) { return e.cliente });

                            indexCliente = clientes.indexOf(rowData.datafactura.responsable);
                        }

                        cargarTablaCXCFacturas(detClient[indexCliente].detalles, fechaInput);

                    } else {
                        let detCC1 = dtTablaCXCAC.rows().data().toArray();
                        cargarTablaCXCAC(detCC1[0].detallesOrig, fechaInput, arrTblFacturas)

                        detCC1 = dtTablaCXCAC.rows().data().toArray();
                        let detCliente1 = dtTablaCXC.rows().data().toArray();

                        cargarTablaCXC(detCliente1[0].detallesOrig, fechaInput, detCC1);
                        cargarTablaCXCCc(detCliente1[0].detallesOrig, fechaInput, detCC1);

                        let ccs = detCC1.map(function (e) { return e.cc });
                        let indexCC = ccs.indexOf(rowData.datafactura.areaCuenta);

                        cargarTablaCXCFacturas(detCC1[indexCC].detalles, fechaInput);
                    }

                    mdlCEFacturaMod.modal("hide");

                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
        init();
    };

    $(document).ready(function () {
        CuentasPorCobrar.CuentasPorCobrar.GestionCobranza = new gestioncobranza();
    }).ajaxStart(function () { $.blockUI({ baseZ: 2000, message: 'Procesando...' }) }).ajaxStop($.unblockUI);
})(); 