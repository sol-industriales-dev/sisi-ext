(() => {
    $.namespace('Enkontrol.OrdenCompra.Editar');

    Editar = function () {
        //#region Selectores
        const tblPartidas = $('#tblPartidas');
        const tblPagos = $('#tblPagos');
        const tblRetenciones = $('#tblRetenciones');
        const mdlPagos = $('#mdlPagos');
        const mdlRetenciones = $('#mdlRetenciones');
        const textAreaDescPartida = $('#textAreaDescPartida');
        //#region Panel Izquierdo
        selectCC = $('#selectCC');
        inputNumero = $('#inputNumero');
        inputNumeroReq = $('#inputNumeroReq');
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
        //#endregion

        //#region Panel Derecho
        selectTipoOC = $('#selectTipoOC');
        selectMoneda = $('#selectMoneda');
        inputTipoCambio = $('#inputTipoCambio');
        const inputSubTotal = $('#inputSubTotal');
        inputIVAPorcentaje = $('#inputIVAPorcentaje');
        const inputIVANumero = $('#inputIVANumero');
        const inputRetencion = $('#inputRetencion');
        const inputTotal = $('#inputTotal');
        const inputTotalFinal = $('#inputTotalFinal');

        const btnCondPago = $('#btnCondPago');
        const btnRetenciones = $('#btnRetenciones');
        btnGuardarCambios = $('#btnGuardarCambios');
        btnGuardarRetenciones = $('#btnGuardarRetenciones');
        //#endregion

        //#region Elementos Ocultos
        const inputCompradorSesionNum = $('#inputCompradorSesionNum');
        const inputCompradorSesionNom = $('#inputCompradorSesionNom');
        const dtpInicio = $('#dtpInicio');
        const dtpFin = $('#dtpFin');
        //#endregion
        const getProveedorInfo = (num) => $.post('/Enkontrol/OrdenCompra/GetProveedorInfo', { num: num });
        //#endregion

        function init() {
            let hoy = new Date();

            dtpInicio.datepicker().datepicker("setDate", new Date(hoy.getFullYear(), 0, 1));
            dtpFin.datepicker().datepicker("setDate", new Date(hoy.getFullYear(), 11, 31));
            dtpFecha.datepicker().datepicker();

            isComprador();
            initCbo();

            initTablePartidas();
            initTablePagos();
            initTableRetenciones();

            btnCondPago.click(() => { mdlPagos.modal('show') });
            btnRetenciones.click(() => { mdlRetenciones.modal('show') });
            btnGuardarCambios.click(() => { updateCompra() });
            btnGuardarRetenciones.click(() => { updateRetenciones() });
        }

        const getComprador = () => $.post('/Enkontrol/OrdenCompra/getComprador');
        const getCompra = () => $.post('/Enkontrol/OrdenCompra/GetCompra', {
            cc: (selectCC.val() == '' || selectCC.val() == null) ?
                (selectCC.attr('cc') != '' && selectCC.attr('cc') != undefined) ?
                    selectCC.attr('cc') : ''
                : selectCC.val(),
            num: inputNumero.val() != '' ? inputNumero.val() : 0,
            esOC_Interna: inputNumero.data().esOC_Interna,
            PERU_tipoCompra: inputNumero.data().PERU_tipoCompra
        });
        const guardarCambios = () => $.post('/Enkontrol/OrdenCompra/UpdateCompra', { compra: getInfoCompra() });

        selectCC.on('change', e => {
            if ($(e.currentTarget).val() != '' && inputNumero.val() != '') {
                cargarCompra();
            }
        });

        inputNumero.on('change', e => {
            if ($(e.currentTarget).val() != '' && selectCC.val() != '') {
                cargarCompra();
            }
        });

        inputIVAPorcentaje.on('change', e => {
            let valor = $(e.currentTarget).val();

            if (!isNaN(valor)) {
                let iva = parseFloat(valor);
                let subTotal = unmaskNumero(inputSubTotal.val());

                inputIVANumero.val(maskNumero((subTotal * iva) / 100));
                inputTotal.val(maskNumero(subTotal + (unmaskNumero(inputIVANumero.val()))));
            } else {
                $(e.currentTarget).val(16);
                inputIVAPorcentaje.change();
            }
        });

        function initCbo() {
            selectTipoOC.fillCombo('/Enkontrol/Requisicion/FillComboTipoReq', null, false);
            selectLab.fillCombo('/Enkontrol/Requisicion/FillComboLab', null, false);
        }

        function isComprador() {
            getComprador()
                .done(response => {
                    if (response.success) {
                        inputCompradorSesionNum.val(response.comprador.comprador);
                        inputCompradorSesionNom.val(response.comprador.emplNom);
                    } else {
                        AlertaGeneral("Aviso", "No eres comprador.");
                    }
                })
                .done(() => {
                    // selectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCcComCompradorModalEditar', null, false);
                    if (window.location.pathname != "/Enkontrol/OrdenCompra/Autorizar" && window.location.pathname != "/Enkontrol/OrdenCompra/SeguimientoAutorizacion") {
                        selectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCcAut', { isAuth: false }, false, null);
                    }
                });
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
                },
                columns: [
                    { data: 'partida', title: 'Partida' },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Descripción' },
                    { data: 'areaCuenta', title: 'Área Cuenta' },
                    { data: 'fecha_entrega', title: 'Fecha Entregar' },
                    { data: 'cantidad', title: 'Cantidad' },
                    {
                        data: 'precio', title: 'Precio', render: (data, type, row, meta) => {
                            var mon = selectMoneda.find('option[value="' + row.moneda + '"]').text().trim();
                            return maskNumero(data) + " " + mon;
                        }
                    },
                    { data: 'importe', title: 'Importe' }
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
                        targets: [4]
                    },
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return maskNumero(data);
                            }
                        },
                        targets: [7]
                    },
                    { className: "dt-center", "targets": "_all" }
                ]
            });
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
                                return maskNumero(data);
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
                "language": dtDicEsp,
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                "scrollCollapse": true,
                'initComplete': function (settings, json) {

                },
                columns: [
                    { data: 'id_cpto', title: 'Id Cpto' },
                    { data: 'descRet', title: 'Desc Ret' },
                    {
                        data: 'cantidad', render: function (data, type, row, meta) {
                            let html = '<input id="inputRet_' + meta.row + '" class="form-control cantidad" style="text-align: right;" value="' + maskNumero(data).replace('$', '') + '" data-orden="' + row.orden + '" data-id_cpto="' + row.id_cpto + '" data-cantidad="' + row.cantidad + '" />'

                            return html;
                        }, title: 'Cantidad'
                    },
                    { data: 'porc_ret', title: 'Porc Ret' },
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
                                return maskNumero(data);
                            }
                        },
                        "targets": [4]
                    },
                    { "className": "dt-center", "targets": "_all" }
                ]
            });
        }

        function llenarInformacion(data) {
            //#region Panel Izquierdo
            selectBoS.val(data.bienes_servicios);
            dtpFecha.val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.fecha.substr(6)))));

            if ($('#inputEmpresaActual').val() == 6) {
                inputProvNum.val(data.PERU_proveedor);
            } else if ($('#inputEmpresaActual').val() == 3) {
                $('.elementoPeru').hide();
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

            inputProvNom.val(data.proveedorNom);
            inputCompNum.val(data.comprador);
            inputCompNom.val(data.compradorNom);
            inputSolNum.val(data.solicito);
            inputSolNom.val(data.solicitoNom);
            inputAutNum.val(data.autorizo);
            inputAutNom.val(data.autorizoNom);
            inputEmb.val(data.embarquese);
            selectLab.val(data.libre_abordo);
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

            // $("#selectCuentaCorriente").val(data.PERU_cuentaCorriente);
            // $("#selectFormaPago").val(data.PERU_formaPago);
            //#endregion

            //#region Panel Derecho
            selectTipoOC.val(data.tipo_oc_req);
            selectMoneda.val(data.moneda);
            inputTipoCambio.val(data.tipo_cambio);
            inputSubTotal.val(maskNumero(data.sub_total));
            inputIVAPorcentaje.val(data.porcent_iva);
            inputIVANumero.val(maskNumero(data.iva));
            inputRetencion.val(maskNumero(data.rentencion_despues_iva));
            inputTotal.val(maskNumero(data.total));
            let totalFinal = (unmaskNumero(inputRetencion.val()) - unmaskNumero((inputTotal.val()))) * -1
            inputTotalFinal.val(maskNumero2DCompras((totalFinal)));
            data.ST_OC == 'A' ? $('#btnGuardarRetenciones').hide() : $('#btnGuardarRetenciones').show();
            selectCC.append(`<option value="${data.cc}">${data.ccDesc}</option>`);
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

        function cargarCompra() {
            getCompra().done(response => {
                if (response.success) {
                    if (response.info.cc != null) {
                        llenarInformacion(response.info);
                        if (response.partidas.length > 0) {
                            inputNumeroReq.val(response.partidas[0].num_requisicion);
                        }
                        AddRows(tblPartidas, response.partidas); //TO:DO
                        AddRows(tblPagos, response.pagos);
                        AddRows(tblRetenciones, response.retenciones);

                        tblPartidas.find('tbody tr:eq(0) td:eq(0)').click()
                    } else {
                        AlertaGeneral('Alerta', 'No se encontró información.');

                        limpiarInformacion();

                        limpiarTabla(tblPartidas);
                        limpiarTabla(tblPagos);
                        limpiarTabla(tblRetenciones);

                        textAreaDescPartida.val('');
                    }
                } else {
                    AlertaGeneral('Alerta', 'Error al consultar la información.');
                }
            });
        }

        function getInfoCompra() {
            let compra = {
                //#region Panel Izquierdo
                cc: selectCC.val(),
                numero: inputNumero.val(),
                bienes_servicios: selectBoS.val(),
                fecha: dtpFecha.val(),
                proveedor: inputProvNum.val(),
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
                tipo_cambio: inputTipoCambio.val(),
                sub_total: unmaskNumero(inputSubTotal.val()),
                porcent_iva: inputIVAPorcentaje.val(),
                iva: unmaskNumero(inputIVANumero.val()),
                rentencion_despues_iva: unmaskNumero(inputRetencion.val()),
                total: unmaskNumero(inputTotal.val())
                //#endregion
            };

            return compra;
        }

        function updateCompra() {
            guardarCambios().done(response => {
                if (response.success) {
                    AlertaGeneral('Alerta', 'Se ha guardado la información.');
                    cargarCompra();
                } else {
                    AlertaGeneral("Alerta", "Error al guardar la información.");
                    cargarCompra();
                }
            })
        }

        function updateRetenciones() {
            var request = new XMLHttpRequest();
            const data = getInfoRetenciones();

            request.open("POST", '/Enkontrol/OrdenCompra/UpdateRetencionesCompra');
            request.send(data);
            request.onload = function (response) {
                if (request.status == 200) {
                    debugger;
                    AlertaGeneral('Alerta', 'Se ha guardado la información.');
                    cargarCompra();
                } else {
                    AlertaGeneral("Alerta", "Error al guardar la información.");
                    cargarCompra();
                }
            };

        }

        function getInfoRetenciones() {
            let retenciones = [];
            let obj;

            const table = $('#tblRetenciones').DataTable();
            table.rows().every(function (index, element) {
                const row = $(this.node());
                const data = this.data();
                const cantidad = row.find('.cantidad').val() == "" ? 0 : row.find('.cantidad').val();

                debugger;

                obj = new Object();
                obj.cc = selectCC.val();
                obj.numero = inputNumero.val();
                obj.cantidad = cantidad;
                obj.orden = parseInt(row.find('.cantidad').attr('data-orden'));
                obj.id_cpto = parseInt(row.find('.cantidad').attr('data-id_cpto'));

                retenciones.push(obj);
            });

            let retencion = new FormData();
            retencion.append("retencion", JSON.stringify(retenciones));

            return retencion;
        }

        function llenarInputProveedorDistinto(numeroProveedor) {
            let listInputs = tblPartidas.find('tbody tr .inputProveedorDistinto');

            $(listInputs).each(function (id, element) {
                $(element).val(numeroProveedor);
            });
        }

        init();
    }

    $(document).ready(() => {
        Enkontrol.OrdenCompra.Editar = new Editar();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); });
})();