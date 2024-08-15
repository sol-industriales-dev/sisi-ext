(function () {
    $.namespace('facturacion.index');
    index = function () {
        mensajes = { PROCESANDO: 'Procesando...' };
        dinero = $(".dinero");
        porcentage = $(".porcentage");
        codigoPostal = $(".codigoPostal");
        telefono = $(".telefono");
        selCC = $("#selCC");
        dpFecha = $("#dpFecha");
        txtDetFechaEntrega = $("#txtDetFechaEntrega");
        txtCC = $("#txtCC");
        txtDetCantidad = $("#txtDetCantidad");
        txtDetPrecio = $("#txtDetPrecio");
        txtDetDescuento = $("#txtDetDescuento");
        txtDetImporte = $("#txtDetImporte");
        txtDetDescuentoDinero = $("#txtDetDescuentoDinero");
        txtDetSubTotal = $("#txtDetSubTotal");
        txtDetDescPorcentage = $("#txtDetDescPorcentage");
        txtDetIvaPorcentage = $("#txtDetIvaPorcentage");
        txtDetRentenciones = $("#txtDetRentenciones");
        txtDetIvaDinero = $("#txtDetIvaDinero");
        txtDetDescDinero = $("#txtDetDescDinero");
        txtDetTotal = $("#txtDetTotal");
        tblDetalle = $("#tblDetalle");
        pedido = $(".pedido");
        remision = $(".remision");
        factura = $(".factura");
        divDetalle = $("#divDetalle");
        btnBuscaInsumo = $("#btnBuscaInsumo");
        txtDetDescripcion = $("#txtDetDescripcion");
        txtDetInsumo = $("#txtDetInsumo");
        tblInsumo = $("#tblInsumo");
        EditarInsumo = $("#EditarInsumo");
        txInsumoDescripcion = $("#txInsumoDescripcion");
        txtDetUnidad = $("#txtDetUnidad");
        importe = $(".importe");
        txtAutotc = $('#txtAutotc');
        selAutoMoneda = $("#selAutoMoneda");
        selCiaSuc = $("#selCiaSuc");
        selSucursalNum = $("#selSucursalNum");
        selNumcte = $("#selNumcte");
        selRemisionRegimen = $("#selRemisionRegimen");
        selFacturacionPago = $("#selFacturacionPago");
        selRemisionTipoCuenta = $("#selRemisionTipoCuenta");
        selTm = $("#selTm");
        txtSucursalNomb = $("#txtSucursalNomb");
        txtPedidoCondPago = $("#txtPedidoCondPago");
        selCondEnt = $("#selCondEnt");
        selElaboroNUM = $("#selElaboroNUM");
        txtObservaciones = $("#txtObservaciones");
        txtPedido = $("#txtPedido");
        selFacturacionSerie = $("#selFacturacionSerie");
        txtSucursalNum = $("#txtSucursalNum");
        selTipoFlete = $("#selTipoFlete");
        selVendedorNum = $("#selVendedorNum");
        txtAutoZona = $("#txtAutoZona");
        txtClienteNombre = $("#txtClienteNombre");
        txtClienteNomCorto = $("#txtClienteNomCorto");
        txtClienteRFC = $("#txtClienteRFC");
        txtClienteTelefono = $("#txtClienteTelefono");
        txtClienteDireccion = $("#txtClienteDireccion");
        txtClienteCp = $("#txtClienteCp");
        txtClienteCiudad = $("#txtClienteCiudad");
        txtTransporte = $("#txtTransporte");
        txtTalon = $("#txtTalon");
        txtConsignado = $("#txtConsignado");
        txtPedidoReq = $("#txtPedidoReq");
        txtDetRentenciones = $("#txtDetRentenciones");
        txtRemision = $("#txtRemision");
        txtFactura = $("#txtFactura");
        txtRemisionSat = $("#txtRemisionSat");
        txtFacturacionXml = $("#txtFacturacionXml");
        txtFacturacionPDF = $("#txtFacturacionPDF");
        txtFacturacionGsdb = $("#txtFacturacionGsdb");
        txtFacturacionAsn = $("#txtFacturacionAsn");
        txtEstatus = $("#txtEstatus");
        txtFacturaFormulario = $("#txtFacturaFormulario");
        txtRemisionCuenta = $("#txtRemisionCuenta");
        selRemisionsAT = $("#selRemisionsAT");
        txtRemisionEFolio = $("#txtRemisionEFolio");
        btnGuardar = $("#btnGuardar");
        txtVendedorNomb = $("#txtVendedorNomb");
        txtIdUsuarion = $("#txtIdUsuarion");
        txtNombreUsuarion = $("#txtNombreUsuarion");
        tblRentencion = $("#tblRentencion");
        txtElaboroNomb = $("#txtElaboroNomb ");
        spanPedidoCodPago = $("#spanPedidoCodPago");

        function init() {
            GridListaInsumos();
            initTblInsumo();
            initCombobox();
            initDatepicker();
            inittTblRentención();
            tblRentencion.hide();
            initFactura();
            dinero.change(maskAutoDinero);
            porcentage.change(maskAutoPorcentage);
            codigoPostal.change(maskCodigoPostal);
            telefono.change(maskTelefono);
            importe.change(setImporte);
            selCC.change(setCC);
            selVendedorNum.change(setVendedor);
            selElaboroNUM.change(setElaboro);
            selSucursalNum.change(setSecursalNombre);
            selAutoMoneda.change(asignaTC);
            selFacturacionPago.change(setClaveSat),
            btnBuscaInsumo.click(BuscaInsumo);
            selNumcte.change(GetObjCliente);
            btnGuardar.click(validaGuardado);
        }

        function initSwitch() {
            chbIva.bootstrapSwitch();
        }

        function initCombobox() {
            selCC.fillCombo('/Facturacion/Facturacion/FillComboCC', null, false, null);
            selCiaSuc.fillCombo("/Facturacion/Facturacion/FillComboCiaSuc", null, false, null); selCiaSuc.val("1");
            selNumcte.fillCombo("/Facturacion/Facturacion/FillComboCliente", null, false, null);
            selRemisionRegimen.fillCombo("/Facturacion/Facturacion/FillComboRegFiscal", null, false, null);
            selRemisionsAT.fillCombo("/Facturacion/Facturacion/FillComboMetodoPago", null, false, null);
            selFacturacionPago.fillCombo("/Facturacion/Facturacion/FillComboClaveSat", null, false, null);
            selTm.fillCombo("/Facturacion/Facturacion/FillComboTm", null, false, null);
            selVendedorNum.fillCombo("/Facturacion/Facturacion/FillComboEmpleado", null, false, null);
            selElaboroNUM.fillCombo("/Facturacion/Facturacion/FillComboEmpleado", null, false, null);
            selAutoMoneda.val("1");
            txtAutoZona.fillCombo("/Facturacion/Facturacion/FillComboZonas", null, false, null); txtAutoZona.val("1");
        }

        function initDatepicker() {
            dpFecha.datepicker().datepicker("setDate", new Date());
            txtDetFechaEntrega.datepicker().datepicker("setDate", new Date());
        }

        function initFactura() {
            var pedido = localStorage.getItem("pedido");
            var editar = localStorage.getItem("editar");
            localStorage.removeItem("pedido");
            localStorage.removeItem("editar");
            if (pedido != null) { SetObjPedidio(pedido); }
            else {
                getNew();
            }
            if (editar == "false" && editar != null) {
                $(":input").not(btnRenteciones).attr("disabled", true);
                $("a").attr("disabled", true);
            }
        }

        function GridListaInsumos() {
            tblInsumo.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    search: "<div class=\"col-xs-3 col-sm-3 col-md-3 col-lg-3\"><div class=\"input-group\"><span class=\"input-group-addon\" id=\"basic-addon3\">Insumo</span><input type=\"text\" class=\"form-control\" id=\"txtDetInsumo\" /></div></div>" +
                        "<div class=\"col-xs-3 col-sm-3 col-md-3 col-lg-6\"><div class=\"input-group\"><span class=\"input-group-addon\" id=\"basic-addon3\">Descripcion</span><input type=\"text\" class=\"form-control\" id=\"txtDetDescripcion\" min = \"3\" /></div></div>" +
                        "<div class=\"col-xs-3 col-sm-3 col-md-3 col-lg-1\"><button type=\"button\" class=\"btn btn-primary\" id=\"btnBuscaInsumo\">Buscar</button></div>"
                },
                formatters: {
                    "Accion": function (column, row) {
                        return row.Grupo == 99 ? "<button type='button' class='btn btn-info insumo' data-id='" + row.id + "' value = '" + row.Consecutivo + "' data-toggle='tooltip' title='Agrega partida'>" +
                                "<span class='glyphicon glyphicon-ok-circle'></span> " +
                            " </button> " +
                        "<button type='button' class='btn btn-warning rentencion' data-id='" + row.Insumo + "' data-toggle='tooltip' title='Agrega rentención'>" +
                              "<span class='glyphicon glyphicon-remove-circle'></span> " +
                          " </button> " :
                            "<button type='button' class='btn btn-info insumo' data-id='" + row.id + "' value = '" + row.Consecutivo + "' data-toggle='tooltip' title='Agrega partida'>" +
                                "<span class='glyphicon glyphicon-ok-circle'></span> " +
                            " </button> ";
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                tblInsumo.find(".insumo").on("click", function (e) {
                    var consecutivo = $(this).val();
                    consecutivo == 0 ? SetObjInsumo2($(this).attr('data-id')) : SetObjInsumo(consecutivo);
                });
                tblInsumo.find(".rentencion").on("click", function (e) {
                    var insumo = $(this).attr('data-id');
                    SetObjRentancion(insumo);
                });
                $('[data-toggle="tooltip"]').tooltip();
                var btnBuscaInsumo = $("#btnBuscaInsumo");
                var txtDetDescripcion = $("#txtDetDescripcion");
                btnBuscaInsumo.on('click', function () {
                    if (txtDetDescripcion.val().length > -1) {
                        BuscaInsumo();
                    }
                });
            });
            setRetencion();
            setImporte();
        }

        function inittTblRentención() {
            var contador = 0;
            var contador1 = 0;
            tblRentencion.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                formatters: {
                    "descuento": function (column, row) {
                        return "<input type=\"text\" class=\"form-control rentencion porcentage\" value = '" + maskPorcentage(row.Descuento) + "'>";
                    },
                    "DescuentoDinero": function (column, row) {
                        return "<label class=\"descDinero\">" + maskDinero(row.DescuentoDinero) + "</label>";
                    },
                    "Importe": function (column, row) {
                        return "<label class=\"descDinero\">" + maskDinero(row.Importe) + "</label>";
                    },
                    "Rentencion": function (column, row) {
                        return row.IvaRetenido == "S" ? 
                            "<div class=\"radioBtn btn-group\"><a class=\"btn btn-primary active \" data-toggle=\"radRentencion" + contador++ + "\" data-title=\"S\">Sí</a><a class=\"btn btn-primary notActive \" data-toggle=\"radRentencion" + contador1++ + "\" data-title=\"N\">No</a></div>"
                            : "<div class=\"radioBtn btn-group\"><a class=\"btn btn-primary notActive \" data-toggle=\"radRentencion" + contador++ + "\" data-title=\"S\">Sí</a><a class=\"btn btn-primary active \" data-toggle=\"radRentencion" + contador1++ + "\" data-title=\"N\">No</a></div>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                $(".infos").hide();
                tblRentencion.find(".rentencion").on("change", function (e) {
                    setRetencion();
                });
                tblRentencion.find(".porcentage").on("change", function (e) {
                    tblRentencion.find('tr').each(function () {
                        if ($(this).find('td:eq(4) input[type="text"]').val() != undefined) {
                            var numero = unmaskPorcentage($(this).find('td:eq(4) input[type="text"]').val());
                            var porcentage = numero.toFixed(2).replace(/\d+% ?/g, "") + "%";
                            $(this).find('td:eq(4) input[type="text"]').val(porcentage);
                        }
                    });
                });
                setRetencion();
                setImporte();
                $('.radioBtn a').on('click', function () {
                    var sel = $(this).data('title');
                    var tog = $(this).data('toggle');
                    $('#' + tog).prop('value', sel);

                    $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
                    $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');

                    if (sel == "S") {
                        txtDetRentenciones.val("$10.00");
                    }
                    if (sel == "N") {
                        txtDetRentenciones.val("$0.00");
                    }
                    setRetencion();
                    setImporte();
                });
            });
        }

        function initTblInsumo() {
            tblDetalle.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: "<label>I.V.A. Retenido</label> <div class=\"radioBtn btn-group\"><a class=\"btn btn-primary notActive \" data-toggle=\"radIva\" data-title=\"S\">Sí</a><a class=\"btn btn-primary active \" data-toggle=\"radIva\" data-title=\"N\">No</a></div> " +
                        "<button type=\"button\" class=\"btn btn-success\" data-toggle=\"modal\" data-target=\"#myModal\" id=\"btnAbreModal\"\">Agregar</button> " +
                        "<button type=\"button\" class=\"btn btn-primary\" id=\"btnRenteciones\">Ver Rentenciones</button> "
                },
                formatters: {
                    "cantidad": function (column, row) {
                        return "<input type=\"text\" class=\"form-control importe\" value = '" + row.Cantidad + "'>";
                    },
                    "precio": function (column, row) {
                        return "<input type=\"text\" class=\"form-control importe dinero\" value = '" + maskDinero(row.Precio) + "'>";
                    },
                    "descuento": function (column, row) {
                        return "<input type=\"text\" class=\"form-control importe porcentage\" value = '" + maskPorcentage(row.Descuento) + "'>";
                    },
                    "DescuentoDinero": function (column, row) {
                        return "<label class=\"descDinero\">" + maskDinero(row.DescuentoDinero) + "</label>";
                    },
                    "Importe": function (column, row) {
                        return "<label class=\"descDinero\">" + maskDinero(row.Importe) + "</label>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                $(".infos").hide();
                setRetencion();
                setImporte();
                tblDetalle.find(".importe").on("change", function (e) {
                    setRetencion();
                    setImporte();
                });
                tblDetalle.find(".dinero").on("change", function (e) {
                    tblDetalle.find('tr').each(function () {
                        if ($(this).find('td:eq(4) input').val() != undefined) {

                            var numero = unmaskDinero($(this).find('td:eq(5) input[type="text"]').val());
                            $(this).find('td:eq(5) input[type="text"]').val(maskDinero(numero));
                        }
                    });
                });
                tblDetalle.find(".porcentage").on("change", function (e) {
                    tblDetalle.find('tr').each(function () {
                        if ($(this).find('td:eq(4) input[type="text"]').val() != undefined) {
                            var numero = unmaskPorcentage($(this).find('td:eq(6) input[type="text"]').val());
                            var porcentage = numero.toFixed(2).replace(/\d+% ?/g, "") + "%";
                            $(this).find('td:eq(6) input[type="text"]').val(porcentage);
                        }
                    });
                });

                btnRenteciones = $("#btnRenteciones");
                btnRenteciones.click(verDetalle);
                $('.radioBtn a[data-toggle="radIva"]').on('click', function () {
                    var sel = $(this).data('title');
                    var tog = $(this).data('toggle');
                    $('#' + tog).prop('value', sel);

                    $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
                    $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');

                    if (sel == "S")
                        $("#txtDetIvaPorcentage").val("0.00%");
                    if (sel == "N")
                        $("#txtDetIvaPorcentage").val("16.00%");
                    setRetencion();
                    setImporte();
                });
            });
        }

        function setImporte() {
            var subTotal = 0;
            tblDetalle.find('tr').each(function () {
                if ($(this).find('td:eq(4) input').val() != undefined) {
                    var tipoCambio = unmaskDinero(txtAutotc.val());
                    var iCantidad = $(this).find('td:eq(4) input').val();
                    var iPrecio = unmaskDinero($(this).find('td:eq(5) input[type="text"]').val());
                    var iDescuento = unmaskPorcentage($(this).find('td:eq(6) input[type="text"]').val());

                    var tcPrecio = iPrecio * tipoCambio;
                    var desc = iCantidad * tcPrecio * (iDescuento / 100);
                    var res = (iCantidad * tcPrecio) - desc;

                    $(this).find('td:eq(7) label').text(maskDinero(desc));
                    $(this).find('td:eq(8) label').text(maskDinero(res));

                    subTotal += res;
                }

                var descPorc = unmaskPorcentage(txtDetDescPorcentage.val());
                var iva = unmaskPorcentage(txtDetIvaPorcentage.val());
                var retenciones = unmaskDinero(txtDetRentenciones.val());

                var descDin = subTotal * (descPorc / 100);
                var ivaDin = subTotal * (iva / 100);
                txtDetSubTotal.val(maskDinero(subTotal));
                txtDetDescDinero.val(maskDinero(descDin));
                txtDetIvaDinero.val(maskDinero(ivaDin));
                txtDetTotal.val(maskDinero(subTotal - descDin + ivaDin - retenciones));
            });
        }

        function setRetencion() {
            var rentencion = 0;
            tblRentencion.find('tr').each(function () {
                if ($(this).find('td:eq(4) input').val() != undefined) {
                    var tipoCambio = unmaskDinero(txtAutotc.val());
                    var iPorcentage = unmaskPorcentage($(this).find('td:eq(4) input').val());
                    var subtotal = unmaskDinero(txtDetSubTotal.val());

                    var tcImporte = subtotal * tipoCambio;
                    var porcent = tcImporte * (iPorcentage / 100);
                    var res = tcImporte - porcent;

                    $(this).find('td:eq(5) label').text(maskDinero(porcent));
                    $(this).find('td:eq(6) label').text(maskDinero(res));

                    var radio = $(this).find('.active').data('title');
                    if (radio == 'S') {
                        rentencion += porcent;
                    }
                }
                txtDetRentenciones.val(rentencion == 0 ? maskDinero(rentencion) : "- " + maskDinero(rentencion));
            });
        }

        $('.radioBtn a').on('click', function () {
            var sel = $(this).data('title');
            var tog = $(this).data('toggle');
            $('#' + tog).prop('value', sel);

            $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
            $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');
        });

        function setRadioValue(sel, tog) {
            $('#' + tog).prop('value', sel);
            $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
            $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');
        }

        function getRadioValue(tog) {
            var value = $('a[data-toggle="' + tog + '"]').data('title');
            return value;
        }

        function verDetalle() {
            btnRenteciones = $("#btnRenteciones");
            if (tblDetalle.is(':visible')) {
                btnRenteciones.text("Ver Partidas");
                tblDetalle.hide();
                tblRentencion.show();
            }
            else {
                btnRenteciones.text("Ver Retenciones");
                tblDetalle.show();
                tblRentencion.hide();
            }
        }

        function asignaTC() {
            var tc = $("#selAutoMoneda option:selected").text()
            if (tc == "MX") {
                txtAutotc.prop('disabled', true);
                txtAutotc.val("$1.00");
            }
            if (tc == "DLL") {
                txtAutotc.prop('disabled', false);
            }
        }

        function setClaveSat() {
            var sat = selFacturacionPago.val().slice(0, 2);
            selRemisionsAT.val(sat);
        }

        function setCC() {
            var cc = $("#selCC option:selected").text()
            if (cc == "--Seleccione--") {
                txtCC.val("");
            }
            else {
                txtCC.val(selCC.val());
            }
        }

        function setElaboro() {
            var cc = $("#selElaboroNUM option:selected").text()
            if (cc == "--Seleccione--") {
                txtElaboroNomb.val("");
            }
            else {
                txtElaboroNomb.val(selElaboroNUM.val());
            }
        }

        function setVendedor() {
            var cc = $("#selVendedorNum option:selected").text()
            if (cc == "--Seleccione--") {
                txtVendedorNomb.val("");
            }
            else {
                txtVendedorNomb.val(selVendedorNum.val());
            }
        }

        function setSecursalNombre() {
            var suc = $("#selSucursalNum option:selected").text()
            if (suc == "--Seleccione--") {
                txtSucursalNomb.val("");
            }
            else {
                txtSucursalNomb.val(selSucursalNum.val());
            }
        }

        function maskTelefono() {
            $.each(telefono, function (i, e) {
                if ($(e).val() == "") { $(e).val("000-000-0000"); }
                $(e).val($(e).val().replace(/(\d{3})(\d{3})(\d{4})/, "$1-$2-$3"));
            });
        }

        function maskCodigoPostal() {
            $.each(codigoPostal, function (i, e) {
                if ($(e).val() == "") { $(e).val("00000"); }
                $(e).val($(e).val().replace(/(^\d{5}$)|(^\d{5}-\d{4}$)/, "$1"));
            });
        }

        function maskAutoPorcentage() {
            $.each(porcentage, function (i, e) {
                if ($(e).val() == "") { $(e).val("0.00%"); }
                $(e).val(parseFloat($(e).val()).toFixed(2).replace(/\d+% ?/g, "") + "%");
            });
        }

        function maskPorcentage(numero) {
            return numero.toFixed(2).replace(/\d+% ?/g, "") + "%";
        }

        function unmaskPorcentage(numero) {
            return Number(numero.replace(/%/g, ''));
        }

        function maskAutoDinero() {
            $.each(dinero, function (i, e) {
                if ($(e).val() == "") { $(e).val("$0.00"); }
                $(e).val("$" + parseFloat(unmaskDinero($(e).val())).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            });
        }

        function maskDinero(numero) {
            return "$" + parseFloat(numero).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        }
        function unmaskDinero(dinero) {
            return Number(dinero.replace(/[^0-9\.]+/g, ""));
        }

        function numero3Digitos(num) {
            return ("000" + num).substr(-3, 3)
        }

        function numero2Digitos(num) {
            return ("00" + num).substr(-2, 2)
        }

        function GetObjCliente() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Facturacion/Facturacion/GetObjCliente",
                type: "POST",
                data: { numcte: selNumcte.val() },
                datatype: "json",
                success: function (response) {
                    setObjCliente(response);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function BuscaInsumo() {
            $(".modal-dialog").block({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Facturacion/Facturacion/BuscaInsumo",
                type: "POST",
                data: { insumo: $("#txtDetInsumo").val(), descripcion: $("#txtDetDescripcion").val() },
                datatype: "json",
                success: function (response) {
                    tblInsumo.bootgrid("clear");
                    tblInsumo.bootgrid("append", response);
                    $(".modal-dialog").unblock();
                },
                error: function () {
                    $(".modal-dialog").unblock();
                }
            });
        }

        function getNew() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Facturacion/Facturacion/getNew",
                type: "POST",
                datatype: "json",
                success: function (response) {
                    txtPedido.val(response.pedido);
                    txtRemision.val(response.remision);
                    txtFactura.val(response.factura);
                    txtRemisionEFolio.val(response.cfd_folio);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function SetObjPedidio(pedido) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Facturacion/Facturacion/SetObjPedidio",
                type: "POST",
                data: { pedido: pedido },
                datatype: "json",
                success: function (response) {
                    if (response.success == true) {
                        setObjCliente(response.objCliente);
                        setObjPedido(response.objPedido);
                        setObjRemision(response.objRemision);
                        setObjFactura(response.objFactura);
                        //setObjCiaSurcusal(response.objCdfParametros);
                        setObjUsuario(response.idUsuario, response.NomUsuario);
                        tblDetalle.bootgrid("clear");
                        tblDetalle.bootgrid("append", response.lstPartida);
                        tblRentencion.bootgrid("clear");
                        tblRentencion.bootgrid("append", response.lstRentencion);
                    }
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SetObjInsumo(consecutivo) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Facturacion/Facturacion/SetObjInsumo",
                type: "POST",
                data: { consecutivo: consecutivo },
                datatype: "json",
                success: function (response) {
                    $(".modal-dialog").modal('toggle');
                    var lstDetalle = getLstDetalle(response);
                    tblDetalle.bootgrid("clear");
                    tblDetalle.bootgrid("append", lstDetalle);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SetObjInsumo2(insumo) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Facturacion/Facturacion/SetObjRentencion",
                type: "POST",
                data: { insumo: insumo },
                datatype: "json",
                success: function (response) {
                    $("#myModal").modal('toggle');
                    var lstDetalle = getLstDetalle(response);
                    tblDetalle.bootgrid("clear");
                    tblDetalle.bootgrid("append", lstDetalle);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SetObjRentancion(insumo) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Facturacion/Facturacion/SetObjRentencion",
                type: "POST",
                data: { insumo: insumo },
                datatype: "json",
                success: function (response) {
                    $("#myModal").modal('toggle');
                    var lstRentencion = getLstRentencion(response);
                    tblRentencion.bootgrid("clear");
                    tblRentencion.bootgrid("append", lstRentencion);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function getLstRentencion(response) {
            var ArrDetalle = [];
            tblRentencion.find("tr").each(function (idx, row) {
                if (idx > 0 && $(":nth-child(2)", row).html() != undefined) {
                    var JsonData = {};
                    JsonData.Partida = $("td:eq(0)", row).html();
                    JsonData.Insumo = $("td:eq(1)", row).html();
                    JsonData.Descripcion = $("td:eq(2)", row).html();
                    JsonData.Unidad = $("td:eq(3)", row).html();
                    JsonData.Descuento = unmaskPorcentage($(this).find("td:eq(4) input", row).val());
                    JsonData.DescuentoDinero = unmaskDinero($(this).find('td:eq(5) label').text());
                    JsonData.Importe = unmaskDinero($(this).find('td:eq(6) label').text());
                    JsonData.IvaRetenido = $(this).find('.active').data('title');
                    ArrDetalle.push(JsonData);
                }
            });

            if (response != null) {
                var JsonData = {};
                JsonData.Partida = tblRentencion.bootgrid("getTotalRowCount") + 1;
                JsonData.Insumo = response.Insumo;
                JsonData.Descripcion = response.Descripcion;
                JsonData.Unidad = response.Unidad;
                JsonData.Descuento = response.Descuento;
                JsonData.DescuentoDinero = response.DescuentoDinero;
                JsonData.Importe = response.Importe * unmaskDinero(txtAutotc.val());
                JsonData.IvaRetenido = response.IvaRetenido;
                ArrDetalle.push(JsonData);
            }
            return ArrDetalle;
        }

        function getLstDetalle(response) {
            var ArrDetalle = [];
            tblDetalle.find("tr").each(function (idx, row) {
                if (idx > 0 && $(":nth-child(2)", row).html() != undefined) {
                    var JsonData = {};
                    JsonData.Partida = $(":nth-child(1)", row).html();
                    JsonData.Insumo = $(":nth-child(2)", row).html();
                    JsonData.Descripcion = $(":nth-child(3)", row).html();
                    JsonData.Unidad = $(":nth-child(4)", row).html();
                    JsonData.Cantidad = $(this).find('td:eq(4) input[type="text"]').val();
                    JsonData.Precio = unmaskDinero($(this).find('td:eq(5) input[type="text"]').val());
                    JsonData.Descuento = unmaskPorcentage($(this).find('td:eq(6) input').val());
                    JsonData.DescuentoDinero = unmaskDinero($(this).find('td:eq(7) label').text());
                    JsonData.Importe = unmaskDinero($(this).find('td:eq(8) label').text());
                    ArrDetalle.push(JsonData);

                }
            });

            if (response != null) {
                var JsonData = {};
                JsonData.Partida = tblDetalle.bootgrid("getTotalRowCount") + 1;
                JsonData.Insumo = response.Insumo;
                JsonData.Descripcion = response.Descripcion;
                JsonData.Unidad = response.Unidad;
                JsonData.Cantidad = response.Cantidad;
                JsonData.Precio = response.Precio
                JsonData.Descuento = response.Descuento;
                JsonData.DescuentoDinero = response.DescuentoDinero;
                JsonData.Importe = response.Importe * unmaskDinero(txtAutotc.val());
                ArrDetalle.push(JsonData);
            }
            return ArrDetalle;
        }

        function SaveBigFactura() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Facturacion/Facturacion/SaveBigFactura",
                type: "POST",
                data: { obj: getObjBigFactura(), lst: getLstDetalle(null), lstRentencion: getLstRentencion(null) },
                datatype: "json",
                success: function (response) {
                    if (response) {
                        AlertaGeneral("Factura", "Se guardo con éxito");
                    }
                    if (!response) {
                        AlertaGeneral("Factura", "No se logró completar el guardado");
                    }
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function validaGuardado() {
            var state = true;
            //if (!validarCampo(selSucursalNum)) { state = false; }
            if (!validarCampo(selCC)) { state = false; }
            if (!validarCampo(selNumcte)) { state = false; }
            if (!validarCampo(selFacturacionPago)) { state = false; }
            if (!validarCampo(selTm)) { state = false; }
            if (!validarCampo(selRemisionRegimen)) { state = false; }
            if (!validarCampo(selTipoFlete)) { state = false; }
            if (!validarCampo(selCondEnt)) { state = false; }
            if (!validarCampo(selAutoMoneda)) { state = false; }
            if (!validarCampo(selVendedorNum)) { state = false; }
            if (!validarCampo(selElaboroNUM)) { state = false; }
            $("#btnAbreModal").removeClass('btn-danger').addClass('btn-success');
            if (!validarCampo(txtDetTotal)) { $("#btnAbreModal").removeClass('btn-success').addClass('btn-danger'); state = false; }
            if (state) {
                SaveBigFactura();
            } else {
                AlertaGeneral("Factura", "Llene tododos los datos para guardar");
            }

        }

        function getValFromText(text) {
            var dom_nodes = $($.parseHTML(text));
            return dom_nodes.val();
        }

        function setIdMetPago(id) {
            switch (id) {
                case 1: return "03-1"; break;
                case 2: return "02-2"; break;
                case 3: return "98-3"; break;
                case 4: return "99-4"; break;
                case 5: return "28-5"; break;
                case 6: return "01-6"; break;
                default: return "";
            }
        }

        function validarCampo(_this) {
            var r = false;
            if (_this.val() == '' || _this.val() == '$0.00' || _this.val() == '0' || _this.val() == '0.00%' || _this.val() == '--Seleccione--') {
                if (!_this.hasClass("errorClass")) {
                    _this.addClass("errorClass")
                }
                r = false;
            }
            else {
                if (_this.hasClass("errorClass")) {
                    _this.removeClass("errorClass")
                }
                r = true;
            }
            return r;
        }

        function getObjBigFactura() {
            var obj = {
                fecha: dpFecha.val(),
                cc: $("#selCC option:selected").text(),
                obs: txtObservaciones.val(),
                pedido: txtPedido.val(),
                remision: txtRemision.val(),
                factura: txtFactura.val(),
                estado: txtEstatus.val(),
                cia_sucursal: selCiaSuc.val(),
                numcte: selNumcte.val(),
                nombre: txtClienteNombre.val(),
                direccion: txtClienteDireccion.val(),
                nomcorto: txtClienteNomCorto.val(),
                rfc: txtClienteRFC.val(),
                ciudad: txtClienteCiudad.val(),
                cp: txtClienteCp.val(),
                telefono1: txtClienteTelefono.val(),
                sucursal: $("#selSucursalNum option:selected").text(),
                tipo_pedido: getRadioValue("radTPedido"),
                requisicion: txtPedidoReq.val(),
                cond_pago: txtPedidoCondPago.val(),
                transporte: txtTransporte.val(),
                talon: txtTalon.val(),
                consignado: txtConsignado.val(),
                entregado: getRadioValue("radEntregado"),
                cfd_folio: txtRemisionEFolio.val(),
                id_metodo_pago: selFacturacionPago.val().slice(3, 4),
                id_regimen_fiscal: selRemisionRegimen.val(),
                cfd_num_cta_pago: txtRemisionCuenta.val(),
                cfd_serie: selFacturacionSerie.val(),
                FacturacionPago: selFacturacionPago.val().slice(4, 4),
                gsdb: txtFacturacionGsdb.val(),
                asn: txtFacturacionAsn.val(),
                FacturacionXml: txtFacturacionXml.val(),
                FacturacionPDF: txtFacturacionPDF.val(),
                zona: txtAutoZona.val(),
                moneda: selAutoMoneda.val(),
                tipo_cambio: unmaskDinero(txtAutotc.val()),
                tm: selTm.val(),
                tipo_credito: getRadioValue("radCreCont"),
                sub_total: unmaskDinero(txtDetSubTotal.val()),
                porcent_descto: unmaskPorcentage(txtDetDescPorcentage.val()),
                descuento: unmaskDinero(txtDetDescDinero.val()),
                porcent_iva: unmaskPorcentage(txtDetIvaPorcentage.val()),
                iva: unmaskDinero(txtDetIvaDinero.val()),
                retencion: unmaskDinero(txtDetRentenciones.val()),
                total: unmaskDinero(txtDetTotal.val()),
                aplica_total_antes_iva: unmaskDinero(txtDetDescDinero.val()),
                tipo_flete: selTipoFlete.val(),
                condicion_entrega: selCondEnt.val(),
                tipo: getRadioValue("radTipo"),
                elaboro: $("#selElaboroNUM option:selected").text(),
                vendedor: $("#selVendedorNum option:selected").text(),
                UsuarioNombre: txtNombreUsuarion.val(),
                tipo_clase: selRemisionTipoCuenta.val()
            }
            return obj;
        }

        function setObjPedido(obj) {
            txtPedido.val(obj.pedido);
            $("#selCC option").filter(function () {
                return this.text == obj.cc;
            }).attr('selected', true); selCC.change();
            selCiaSuc.val(obj.cia_sucursal);
            txtPedidoCondPago.val(obj.cond_pago);
            selCondEnt.val(obj.condicion_entrega);
            txtDetDescDinero.val(maskDinero(obj.descuento));
            $("#selElaboroNUM option").filter(function () {
                return this.text == numero3Digitos(obj.elaboro);
            }).attr('selected', true); selElaboroNUM.change();
            dpFecha.val(obj.fecha);
            txtDetIvaDinero.val(maskDinero(obj.iva));
            selAutoMoneda.val(obj.moneda);
            selNumcte.val(obj.numcte);
            txtObservaciones.val(obj.obs);
            txtDetDescPorcentage.val(maskPorcentage(obj.porcent_descto));
            txtDetIvaPorcentage.val(maskPorcentage(obj.porcent_iva));
            txtDetSubTotal.val(maskDinero(obj.sub_total));
            $("#selSucursalNum option").filter(function () {
                return this.text == numero3Digitos(obj.sucursal);
            }).attr('selected', true); selSucursalNum.change();
            txtAutotc.val(maskDinero(obj.tipo_cambio));
            selTipoFlete.val(obj.tipo_flete);
            setRadioValue(obj.tipo_pedido, "radTPedido");
            setRadioValue(obj.tipo, "radTipo");
            setRadioValue(obj.tipo_credito, "radCreCont");
            selTm.val(obj.tm);
            txtDetTotal.val(maskDinero(obj.total_dec));
            $("#selVendedorNum option").filter(function () {
                return this.text == numero3Digitos(obj.vendedor);
            }).attr('selected', true); selVendedorNum.change();
            txtAutoZona.val(obj.zona);
            txtPedidoReq.val(obj.requisicion);
            txtDetRentenciones.val(maskDinero(obj.retencion));
        }

        function setObjCliente(obj) {
            txtClienteNombre.val(obj.nombre);
            txtClienteNomCorto.val(obj.nomcorto);
            txtClienteRFC.val(obj.rfc);
            txtClienteTelefono.val(obj.telefono1);
            txtClienteDireccion.val(obj.direccion);
            txtClienteCp.val(obj.cp);
            txtClienteCiudad.val(obj.ciudad);
            selSucursalNum.fillCombo("/Facturacion/Facturacion/FillComboSurcusal", { numcte: obj.numcte }, false, null);
            $("#selSucursalNum option").filter(function () {
                return this.text == numero3Digitos(1);
            }).attr('selected', true);
            setSecursalNombre();
            txtPedidoCondPago.val(obj.condpago);
            spanPedidoCodPago.text(obj.condpago == 1 ? obj.condpago + " Día" : obj.condpago + " Días");
        }

        function setObjRemision(obj) {
            txtRemision.val(obj.remision != 0 ? obj.remision : "N/A");
            txtTransporte.val(obj.transporte);
            txtTalon.val(obj.talon);
            txtConsignado.val(obj.consignado);
            setRadioValue(obj.entregado, "radEntregado");
        }

        function setObjFactura(obj) {
            txtFactura.val(obj.factura != 0 ? obj.factura : "N/A");
            selRemisionRegimen.val(obj.id_regimen_fiscal);
            txtFacturacionGsdb.val(obj.gsdb);
            txtFacturacionAsn.val(obj.asn);
            txtEstatus.val(obj.estado);
            selFacturacionPago.val(setIdMetPago(obj.id_metodo_pago)); selFacturacionPago.change();
            txtRemisionCuenta.val(obj.cfd_num_cta_pago);
            selFacturacionSerie.val(obj.cfd_serie);
            txtRemisionEFolio.val(obj.cfd_folio);
        }

        function setObjCiaSurcusal(obj) {
            txtFacturacionXml.val(obj.ruta_pdf);
            txtFacturacionPDF.val(obj.ruta_xml);
        }

        function setObjUsuario(idUsuario, NomUsuario) {
            txtIdUsuarion.val(idUsuario);
            txtNombreUsuarion.val(NomUsuario);
        }

        init();
    };
    $(document).ready(function () {
        facturacion.index = new index();
    });
})();