(() => {
    $.namespace('Compras.ProveedorCuadroComparativo');

    //#region CONST
    const lblNombreProveedor = $('#lblNombreProveedor');
    const txtSubtotal = $("#txtSubtotal");
    const txtDescuento = $("#txtDescuento");
    const txtSubtotal_conDescuento = $("#txtSubtotal_conDescuento");
    const txtIVA = $("#txtIVA");
    const txtTotal = $("#txtTotal");
    const txtFletes = $("#txtFletes");
    const txtGtosImportacion = $("#txtGtosImportacion");
    const txtGranTotal = $("#txtGranTotal");
    const txtTipoCambio = $("#txtTipoCambio");
    const txtFechaEntrega = $("#txtFechaEntrega");
    const txtComentario = $("#txtComentario");
    const tblPartidas = $('#tblPartidas');
    const btnGuardar = $('#btnGuardar');
    const lblCCNumRequisicion = $('#lblCCNumRequisicion');
    let dtPartidas;
    //#endregion

    ProveedorCuadroComparativo = function () {
        (function init() {
            fncListeners();

            // var clean_uri = location.protocol + "//" + location.host + location.pathname;
            // window.history.replaceState({}, document.title, clean_uri);
        })();

        function fncListeners() {
            fncObtenerURLParams();
            initTblPartidas();

            //#region EVENTOS
            btnGuardar.on("click", function () {
                fncGuardarCuadroComparativo();
            });

            txtDescuento.on("change", function () {
                if (txtDescuento.val() > 0) {
                    fncCalculosCuadroComparativo();
                } else {
                    $(this).val();
                }
            });

            txtIVA.on("change", function () {
                fncCalculosCuadroComparativo();
            });

            txtFletes.on("change", function () {
                fncCalculosCuadroComparativo();
            });

            txtGtosImportacion.on("change", function () {
                fncCalculosCuadroComparativo();
            });

            txtFechaEntrega.datepicker({ dateFormat: "dd/mm/yy", });

            $('.input-number').on('input', function () {
                this.value = this.value.replace(/[^0-9]/g, '');
            });
            //#endregion
        }

        function fncObtenerURLParams() {
            const variables = fncGetURLParams(window.location.href);
        }

        function fncGetURLParams(url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }
            fncGetDatosProveedor(params.hash);
            return params;
        }

        function fncGetDatosProveedor(hash) {
            if (hash != "") {
                let obj = new Object();
                obj.hash = hash;
                axios.post('/Enkontrol/ProveedorCuadroComparativo/GetDatosProveedor', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        lblNombreProveedor.html(response.data.objProv.nombreProveedor);
                        btnGuardar.attr("proveedor", response.data.objProv.proveedor);
                        btnGuardar.attr("data-empresa", response.data.objProv.idEmpresa);
                        btnGuardar.attr("data-cc", response.data.objProv.cc);
                        btnGuardar.attr("data-numRequisicion", response.data.objProv.numRequisicion);
                        btnGuardar.attr("data-numpro", response.data.objProv.numpro);
                        lblCCNumRequisicion.html(`- [${response.data.objProv.cc}] ${response.data.objProv.numRequisicion}`)

                        if (response.data.objProv.moneda == "USD") {
                            txtTipoCambio.attr("disabled", false);
                        } else {
                            txtTipoCambio.val("1");
                            txtTipoCambio.attr("disabled", "disabled");
                        }

                        $(".tipoMoneda").text(response.data.objProv.moneda);

                        // FILL DATATABLE PARTIDAS
                        dtPartidas.clear();
                        dtPartidas.rows.add(response.data.lstRequisicionDetDTO);
                        dtPartidas.draw();
                    } else {
                        window.location.href = "http://sigoplan.construplan.com.mx/Enkontrol/ProveedorCuadroComparativo/ErrorProveedorCuadroComparativo";
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function initTblPartidas() {
            dtPartidas = tblPartidas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'partida', title: 'PARTIDA' },
                    { data: 'insumoDescripcion', title: 'INSUMO' },
                    { data: 'insumoDesc', title: 'DESCRIPCIÓN' },
                    { data: 'cantidad', title: 'CANTIDAD' },
                    {
                        data: 'estatus', title: 'PRECIO',
                        render: function (data, type, row, meta) {
                            return `<div class="input-group marginTop">
                                        <span class="input-group-addon">$</span>
                                        <input type="text" class="form-control inputPrecioInsumo input-number" onclick="this.select()" value="${data}" placeholder="Ejemplo: 50.00" />
                                    </div>`;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblPartidas.on('change', '.inputPrecioInsumo', function () {
                        tblPartidas.find("tbody tr").each(function (idx, row) {
                            precioInsumo = unmaskNumero6DCompras($(row).find(".inputPrecioInsumo").val());
                            $(row).find(".inputPrecioInsumo").val(maskNumero6DCompras(precioInsumo));
                        });
                        fncCalculosCuadroComparativo();
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '25%', targets: [2] },
                    { width: '1%', targets: [0, 3] },
                    { width: '10%', targets: [1] },
                    { width: '18%', targets: [4] }
                ],
            });
        }

        function fncGuardarCuadroComparativo() {
            let data = new FormData();
            $.each(document.getElementById("txtArchivo").files, function (i, file) {
                data.append("archivo", file);
            });

            let cuadro = {
                cc: btnGuardar.attr("data-cc"),
                numero: +btnGuardar.attr("data-numRequisicion"),
                empresa: +btnGuardar.attr("data-empresa"),
                prov1: +btnGuardar.attr("data-numpro"),
                prov2: 0,
                prov3: 0,
                porcent_dcto1: +txtDescuento.val(),
                porcent_dcto2: 0,
                porcent_dcto3: 0,
                porcent_iva1: +txtIVA.val(),
                porcent_iva2: 0,
                porcent_iva3: 0,
                dcto1: unmaskNumero6DCompras(txtSubtotal.val()) > 0 ? GetSubtotalConDescuento(txtSubtotal.val(), +txtDescuento.val()) : 0,
                dcto2: 0,
                dcto3: 0,
                iva1: unmaskNumero6DCompras(txtIVA.val()) > 0 ? GetSubtotalConIVA(txtSubtotal_conDescuento.val(), txtIVA.val()) : 0,
                iva2: 0,
                iva3: 0,
                total1: unmaskNumero6DCompras(txtTotal.val()),
                total2: 0,
                total3: 0,
                tipo_cambio1: +txtTipoCambio.val(),
                tipo_cambio2: 0,
                tipo_cambio3: 0,
                fecha_entrega1: txtFechaEntrega.val(),
                sub_total1: unmaskNumero6DCompras(txtSubtotal.val()),
                sub_total2: 0,
                sub_total3: 0,
                fletes1: unmaskNumero6DCompras(txtFletes.val()),
                fletes2: 0,
                fletes3: 0,
                gastos_imp1: unmaskNumero6DCompras(txtGtosImportacion.val()),
                gastos_imp2: 0,
                gastos_imp3: 0,
                nombre_prov1: btnGuardar.attr("proveedor") != undefined ? btnGuardar.attr("proveedor") : '',
                nombre_prov2: '',
                nombre_prov3: '',
                moneda1: +btnGuardar.attr("data-numpro") >= 9000 ? 2 : 1,
                moneda2: 1,
                moneda3: 1,
                comentarios1: txtComentario.val(),
                comentarios2: '',
                comentarios3: '',

                //Información de prueba para el parseo en el back-end
                fecha_requisicion: txtFechaEntrega.val(),
                fecha_cuadro: txtFechaEntrega.val(),
                fecha: txtFechaEntrega.val(),
                fecha_entrega2: txtFechaEntrega.val(),
                fecha_entrega3: txtFechaEntrega.val(),
                inslic_fecha_ini: txtFechaEntrega.val(),
                inslic_fecha_fin: txtFechaEntrega.val()
            };

            let detalleCuadro = [];

            tblPartidas.find("tbody tr").each(function (idx, row) {
                let rowData = dtPartidas.row(row).data();

                detalleCuadro.push({
                    cc: btnGuardar.attr("data-cc"),
                    numero: +btnGuardar.attr("data-numRequisicion"),
                    partida: +rowData.partida,
                    insumo: +rowData.insumo,
                    cantidad: +rowData.cantidad,
                    precio1: unmaskNumero6DCompras($(row).find(".inputPrecioInsumo").val()),
                    precio2: 0,
                    precio3: 0
                });
            });

            cuadro.detalleCuadro = detalleCuadro;

            data.append("cuadro", JSON.stringify(cuadro));

            axios.post('GuardarCuadroComparativo', data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    window.location.href = "http://sigoplan.construplan.com.mx/Enkontrol/ProveedorCuadroComparativo/ExitoCapturaProveedorCuadroComparativo";
                } else {
                    window.location.href = "http://sigoplan.construplan.com.mx/Enkontrol/ProveedorCuadroComparativo/ErrorProveedorCuadroComparativo";
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#region CALCULOS
        function GetSubtotalConDescuento(subtotal, porcDescuento) {
            let descuento = (+porcDescuento * unmaskNumero6DCompras(subtotal)) / 100;
            let subtotalConDescuento = unmaskNumero6DCompras(subtotal) - descuento;
            return +subtotalConDescuento;
        }

        function GetSubtotalConIVA(subtotal, porcIVA) {
            let porcIVA_ConCero = '0.' + porcIVA;
            let subtotalConIVA = (unmaskNumero6DCompras(subtotal) * porcIVA_ConCero);
            return subtotalConIVA
        }

        function fncCalculosCuadroComparativo() {
            // let tipoCambio = unmaskNumero6D(txtTipoCambio.val());
            let primerSubTotal = 0;

            tblPartidas.find("tbody tr").each(function (idx, row) {
                let rowData = dtPartidas.row(row).data();
                let cantidad = rowData.cantidad;
                let precio1 = unmaskNumero6DCompras($(row).find('.inputPrecioInsumo').val());
                primerSubTotal += cantidad * precio1;
            });
            txtSubtotal.val(maskNumero6DCompras(primerSubTotal));

            let descuento = txtDescuento.val();
            let segundoSubTotal = primerSubTotal - (primerSubTotal * (descuento / 100));
            txtSubtotal_conDescuento.val(maskNumero6DCompras(segundoSubTotal));

            let iva = txtIVA.val() != '' ? unmaskNumero6DCompras(txtIVA.val()) : 0;
            let total = segundoSubTotal + (segundoSubTotal * (iva / 100));
            txtTotal.val(maskNumero6DCompras(total));

            let fletes = unmaskNumero6DCompras(txtFletes.val());
            let gastosImportacion = unmaskNumero6DCompras(txtGtosImportacion.val());
            let granTotal = total + fletes + gastosImportacion;

            txtFletes.val(maskNumero6DCompras(fletes));
            txtGtosImportacion.val(maskNumero6DCompras(gastosImportacion));

            txtGranTotal.val(maskNumero6DCompras(granTotal));
        }
        //#endregion
    }

    $(document).ready(() => {
        Compras.ProveedorCuadroComparativo = new ProveedorCuadroComparativo();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();