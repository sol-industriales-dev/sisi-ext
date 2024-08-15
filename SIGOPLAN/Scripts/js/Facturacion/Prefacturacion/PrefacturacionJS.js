/// <reference path="PrefacturacionJS.js" />
(function () {
    $.namespace('facturacion.prefacturacion.index');
    index = function () {
        mensajes = { PROCESANDO: 'Procesando...', IMPRIMIENDO: 'Imprimiendo...' };
        hdnId = $("#hdnId");
        txtUnidad = $("#txtUnidad");
        numPrecio = $("#numPrecio");
        numImporte = $("#numImporte");
        dpFecha = $("#dpFecha");
        selCC = $("#selCC");
        dtFechaFiltro = $("#dtFechaFiltro");
        selCCFiltro = $("#selCCFiltro");
        tblPrefactura = $("#tblPrefactura");
        btnBuscar = $("#btnBuscar");
        btnReporte = $("#btnReporte");
        busqCliente = $(".busqCliente");
        modalPrefactura = $("#modalPrefactura");
        tblModalPrefactura = $("#tblModalPrefactura");
        txtNombre = $("#txtNombre");
        dpFechaReporte = $("#dpFechaReporte");
        txtCalle = $("#txtCalle");
        txtNumero = $("#txtNumero");
        txtColonia = $("#txtColonia");
        txtCp = $("#txtCp");
        txtCiudad = $("#txtCiudad");
        txtRfc = $("#txtRfc");
        txtMetodoPago = $("#txtMetodoPago");
        txtTipoMoneda = $("#txtTipoMoneda");
        btnVerReporte = $("#btnVerReporte");
        ireport = $("#report");
        selUsocfdi = $('#selUsocfdi');

        txtInsumo = $("#txtInsumo");
        txtDescripcion = $("#txtDescripcion");
        porDescuento = $("#porDescuento");
        dinDescuento = $("#dinDescuento");
        selTipoInsumo = $("#selTipoInsumo");
        tblOrdenCompra = $("#tblOrdenCompra");
        dtFechaFiltroInicio = $("#dtFechaFiltroInicio");
        dtFechaFiltroFin = $("#dtFechaFiltroFin");
        getInsumo = $(".getInsumo");

        divPrefactura = $("#divPrefactura");
        btnAbrePrefactura = $("#btnAbrePrefactura");
        tblUnidades = $("#tblUnidades");
        selCCPractura = $("#selCCPractura");
        txtDescCC = $("#txtDescCC");
        dpFechaPrefactura = $("#dpFechaPrefactura");
        tblEnEspera = $("#tblEnEspera");
        tblAceptado = $("#tblAceptado");
        tblRechazado = $("#tblRechazado");
        txtIdReporte = $("#txtIdReporte");
        txtSubTotal = $("#txtSubTotal");
        txtIva = $("#txtIva");
        txtImporteTotal = $("#txtImporteTotal");

        modalListaArchivos = $("#modalListaArchivos");
        tblListaArchivo = $("#tblListaArchivo");
        btnSubirN = $("#btnSubirN");
        cboTipo = $("#cboTipo");
        fupAdjunto2 = $("#fupAdjunto2");
        btnCerrar = $("#btnCerrar");
        lblFolio = $("#lblFolio");
        txtEstado = $("#txtEstado");
        modalConfirmacion = $("#modalConfirmacion");
        btnConfirmar = $("#btnConfirmar");
        rfc = $(".rfc");
        codigoPostal = $(".codigoPostal");
        validaSel = $(".validaSel");

        modalBusqCliente = $("#modalBusqCliente");
        btnBusqClienteCerrar = $("#btnBusqClienteCerrar");
        selBusqNumcte = $("#selBusqNumcte");
        txtxBusqNombre = $("#txtxBusqNombre");
        btnBusqClienteSelecciona = $("#btnBusqClienteSelecciona");
        lblPlantillaMensaje = $("#lblPlantillaMensaje");

        var contId = 0; var contCantidad = 0; var contUnidad = 0; var contPrecio = 0; var contImporte = 0; var contTipo = 0; var contConcepto = 0;
        btnAddRenglon = $("#btnAddRenglon");
        btnAddImporte = $("#btnAddImporte");
        tblImpuesto = $("#tblImpuesto");
        lstNombre = $("#lstNombre");
        divIndentificacion = $("#divIndentificacion");
        lstConceptoImporte = $("#lstConceptoImporte");
        btnMinusRenglon = $("#btnMinusRenglon");
        btnMinusImporte = $("#btnMinusImporte");
        hdnTotalId = $("#hdnTotalId");
        txtValTotal = $("#txtValTotal");

        modalBusqPlantila = $("#modalBusqPlantila");
        selPlantillaNumcte = $("#selPlantillaNumcte");
        txtxPlantillaNombre = $("#txtxPlantillaNombre");
        selPlantillaCC = $("#selPlantillaCC");
        selPlantillaMoneda = $("#selPlantillaMoneda");
        btnPlantillaClienteCerrar = $("#btnPlantillaClienteCerrar");
        btnPLantillaSelecciona = $("#btnPLantillaSelecciona");
        numCliente = $("#numCliente");

        $.ui.dialog.prototype._allowInteraction = function (e) {
            return !!$(e.target).closest('.ui-dialog, .ui-datepicker, .select2-dropdown').length;
        };

        function init() {
            $('.select2').select2();
            initModal();
            initDatePicker();
            selCCFiltro.fillCombo('/Administrativo/Facultamiento/getComboCCEnkontrol', null, false, null);
            initTblUnidades();
            btnBuscar.click(getTablaPrefactura);
            btnReporte.click(openModal);
            btnVerReporte.click(validaReporte);
            getInsumo.change(getObjInsumo);
            btnAbrePrefactura.click(openPrefactura);
            selCCPractura.change(setCC);
            btnSubirN.click(subir);
            btnCerrar.click(closeModal);
            btnConfirmar.click(actualizacionConfirmada);
            setInsumosDefault();
            GridListaArchivos();
            rfc.change(maskRFC);
            validaSel.change(maskSel);
            codigoPostal.change(maskCodigoPostal);
            busqCliente.keydown(buscarCliente);
            btnBusqClienteCerrar.click(cerrarCliente);
            selBusqNumcte.change(setClienteNombre);
            txtNombre.change(GetObjClientePorNombre);
            btnBusqClienteSelecciona.click(GetObjCliente);
            btnAddRenglon.click(AgregarRenglon);
            btnAddImporte.click(AgregarImporte);
            btnMinusRenglon.click(RemueveRenglon);
            btnMinusImporte.click(RemueveImporte);
            btnPLantillaSelecciona.click(validaPlantilla);
            selPlantillaNumcte.change(setClienteNombrePlantilla);
            $("#tblImpuesto tbody").sortable({
                stop: function () {
                    setImporte();
                }
            });
            tblImpuesto.find(".importe").on("change", function (e) {
                setImporte();
            });
        }

        function initModal() {
            modal = modalPrefactura.dialog({
                autoOpen: false,
                resizable: false,
                draggable: false,
                modal: true,
                height: window.innerHeight - 80,
                width: window.innerWidth - 30,
                maxHeight: window.innerHeight - 15,
                maxWidth: window.innerWidth - 30,
                overflow: 'scroll',
            });
            modal.prev(".ui-dialog-titlebar").css("background", "white");
            modal.dialog({
                close: function (event, ui) { setInsumosDefault(); },
                open: function (event, ui) {
                    modal.block({
                        message: "Preparando reporte",
                        css: {
                            border: 'none',
                            padding: '15px',
                            backgroundColor: '#000',
                            '-webkit-border-radius': '10px',
                            '-moz-border-radius': '10px',
                            opacity: .5,
                            color: '#fff'
                        }
                    });
                    setFolio();
                    //initCombobox();
                    modal.unblock();
                }

            });
            unaPrefactura = divPrefactura.dialog({
                autoOpen: false,
                resizable: true,
                draggable: false,
                modal: true,
                height: "auto",
                width: "auto",
            });
            modalBusqCliente = modalBusqCliente.dialog({
                autoOpen: false,
                resizable: true,
                draggable: false,
                modal: true,
                width: "auto"
            });
        }
        function initDatePicker() {
            dpFecha.datepicker().datepicker("setDate", new Date());
            dpFechaReporte.datepicker().datepicker("setDate", new Date());
            dtFechaFiltroFin.datepicker().datepicker("setDate", new Date());
            dtFechaFiltroInicio.datepicker().datepicker("setDate", new Date());
            dpFechaPrefactura.datepicker().datepicker("setDate", new Date());
        }
        function initTblUnidades() {
            tblUnidades.bootgrid({
                rowCount: -1,
                sortable: false,
                css: {
                    columnHeaderText: '',
                },
                templates: {
                    header: "",
                    headerCell: '<th data-column-id="{{ctx.column.id}}" class="text-center" style="border: 1px solid #000 !important; backgroundColor: rgb(221,221,221);"><span>{{ctx.column.text}}</span>',
                    footer: ""
                },
                formatters: {
                    "Id": function (column, row) {
                        var id = row.id == undefined ? "0" : row.id;
                        return "<input type=\"text\" class=\"form-control\" id=\"id" + contId++ + "\" hidden value =" + id + " />";
                    },
                    "Tipo": function (column, row) {
                        var id = row.Id == undefined ? "0" : row.Id;
                        var insumo = row.Tipo == 1 ? "selected" : "";
                        //var linea = row.Tipo == 2 ? "selected" : "";
                        var concepto = row.Tipo == 3 || row.Tipo == undefined ? "selected" : "";
                        var total = row.Tipo == 4 ? "selected" : "";
                        return "<select class=\"form-control selTipo\" id=\"tipo" + contTipo++ + "\"><option value=\"1\" " + insumo + " >Insumo</option><option value=\"3\" " + concepto + ">Concepto</option><option value=\"4\" " + total + ">Total</option></select><input type=\"hidden\" class=\"form-control\" id=\"id" + contId++ + "\" value =" + id + " />";
                    },
                    "Cantidad": function (column, row) {
                        var cantidad = row.Cantidad == "undefined" || row.Cantidad == undefined ? "&nbsp" : row.Cantidad;
                        return "<input type=\"text\" class=\"form-control importe\" id=\"cantidad" + contCantidad++ + "\" value =" + cantidad + " />"
                    },
                    "Concepto": function (column, row) {
                        var unidad = row.Unidad == "undefined" || row.Unidad == undefined ? "&nbsp" : row.Unidad;
                        return "<div class=\"editar\" id=\"unidad" + contUnidad++ + "\">" + unidad + "</div>";
                    },

                    "Unidad": function (column, row) {
                        var concepto = row.Concepto == "undefined" || row.Concepto == undefined ? "&nbsp" : row.Concepto;
                        return "<input type=\"text\" class=\"form-control\" id=\"concepto" + contConcepto++ + "\" value =" + concepto + " />"
                    },
                    "Precio": function (column, row) {
                        var precio = row.Precio == "undefined" || row.Precio == undefined ? "&nbsp" : maskDinero(row.Precio);
                        return "<input type=\"text\" class=\"form-control dinero importe\" id=\"precio" + contPrecio++ + "\" value =" + precio + " />";
                    },
                    "Importe": function (column, row) {
                        var importe = row.Importe == "undefined" || row.Importe == undefined ? "&nbsp" : maskDinero(row.Importe);
                        return "<input type=\"text\" class=\"form-control dinero importe\" id=\"importe" + contImporte++ + "\" value =" + importe + " />";
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                initEditor();
                tipoRenglon();
                tblUnidades.find(".selTipo").on("change", function (e) {
                    tipoRenglon();
                });
                tblUnidades.find(".dinero").on("change", function (e) {
                    tblUnidades.find('tr').each(function () {
                        var tipo = $(this).find("td:eq(0) select").val();
                        if ($(this).find('td:eq(5) input').val() != undefined && tipo == "1") {
                            var numero1 = unmaskDinero($(this).find('td:eq(4) input').val());
                            $(this).find('td:eq(4) input[type="text"]').val(maskDinero(numero1));
                            var numero2 = unmaskDinero($(this).find('td:eq(5) input').val());
                            $(this).find('td:eq(5) input[type="text"]').val(maskDinero(numero2));
                        }
                    });
                });
                tblUnidades.find(".importe").on("change", function (e) {
                    setImporte();
                });
            });
            tblEnEspera.bootgrid({
                templates: {
                    headerCell: '<th data-column-id="{{ctx.column.id}}" class="text-center" style="width:10%;"><a href="javascript:void(0);" class="column-header-anchor"><span>{{ctx.column.text}}</span>',
                },
            });
            tblAceptado.bootgrid({
                templates: {
                    headerCell: '<th data-column-id="{{ctx.column.id}}" class="text-center" style="width:10%;"><a href="javascript:void(0);" class="column-header-anchor"><span>{{ctx.column.text}}</span>',
                },
            });
            tblRechazado.bootgrid({
                templates: {
                    headerCell: '<th data-column-id="{{ctx.column.id}}" class="text-center" style="width:10%;"><a href="javascript:void(0);" class="column-header-anchor"><span>{{ctx.column.text}}</span>',
                },
            });
            setInsumos(null);
        }

        function initEditor() {
            var editar = $('.editar');
            var config = {
                ui: {
                    toolbar: {
                        draggable: true,
                        visible: true
                    }
                }
            };

            $.each(editar, function (i, e) {
                var editor = textboxio.inline($(e)[0], config);
            })

        }

        function AgregarRenglon() {
            tblUnidades.find('tbody')
                .append($('<tr data-row-id="' + getUltimoDataRow() + '">')
                    .append($('<td>')
                        .append($("<select class=\"form-control\" id=\"tipo" + contTipo++ + "\" onchange=\"tipoRenglon()\"><option value=\"1\">Insumo</option><option value=\"3\" selected >Concepto</option><option value=\"4\">Total</option></select>")
                        )
                    ).append($('<td>')
                        .append($("<input type=\"text\" id=\"cantidad" + contCantidad++ + "\" class=\"form-control importe\" value =\"&nbsp\" onchange=\"setImporte()\" />")
                        )
                    )
                    .append($('<td>')
                        .append($("<input type=\"text\" id=\"cantidad" + contConcepto++ + "\" class=\"form-control importe\" value =\"&nbsp\" />")
                        )
                    )
                    .append($('<td>')
                        .append($("<div class=\"editar\" id=\"unidad" + contUnidad++ + " value =\"\" \"></div>")
                        )
                    )
                    .append($('<td>')
                        .append($("<input type=\"text\" class=\"form-control importe\" id=\"precio" + contPrecio++ + "\" value =\"&nbsp\" onchange=\"setImporte()\"/>")
                        )
                    )
                    .append($('<td>')
                        .append($("<input type=\"text\" class=\"form-control importe\" id=\"importe" + contImporte++ + "\" value =\"&nbsp\" onchange=\"maskDinero(this)\"/>")
                        )
                    )
                );
            initEditor();
            tipoRenglon();
        }

        function AgregarImporte() {
            tblImpuesto.find('tbody').append(`<tr><td><div class="input-group"><input type="hidden" class="form-control" value="0"/><input type="text" class="form-control importe lblImpuesto" list="lstConceptoImporte" onchange="setImporte();"/><span class="input-group-addon">■</span><input type="text" class="form-control importe valImpuesto" value ="$0.00" onchange="setImporte();"/></div><input type="hidden" class="form-control" value ="0" /></td>`)
            setImporte();
        }

        function getObjInsumo() {
            if (getInsumo != "" || getInsumo != 0) {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: "/Facturacion/Prefacturacion/getObjInsumo",
                    type: "POST",
                    datatype: "json",
                    data: { fecha: dpFecha.val(), cc: selCC.val(), insumo: txtInsumo.val() },
                    success: function (response) {
                        numPrecio.val(maskDinero(response.PRECIO_INSUMO));
                        txtDescripcion.val(response.descripcion);
                        selTipoInsumo.val(response.tipo);
                        txtUnidad.val(response.unidad);
                        $.unblockUI();
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }

        function setFolio() {
            if (txtIdReporte.val() == '0') {
                $.ajax({
                    url: '/Facturacion/Prefacturacion/setFolio',
                    type: "POST",
                    datatype: "json",
                    success: function (response) {
                        lblFolio.text(generarFolio(response + 1));
                    },
                    error: function () { }
                });
            }
            else {
                lblFolio.text(generarFolio(txtIdReporte.val()));
            }

        }

        function subir() {
            id = btnSubirN.attr('data-id');
            SubirArchivo(null, id)
        }

        function SubirArchivo(e, id) {
            if (true) {

                var formData = new FormData();
                //var filesVisor = document.getElementById("fupAdjunto").files.length;
                var file = document.getElementById("fupAdjunto2").files[0];

                // formData.append("fupAdjunto", file);
                formData.append("TipoArchivo", JSON.stringify(cboTipo.val()));
                formData.append("id", JSON.stringify(id));

                var files = document.getElementById("fupAdjunto2").files;
                $.each(files, function (i, file) {
                    formData.append('fupAdjunto[]', file);
                });

                if (file != undefined) {
                    modalListaArchivos.block({ message: 'Cargando archivo... ¡Espere un momento!' });
                }
                $.ajax({
                    type: "POST",
                    url: '/Facturacion/Prefacturacion/SubirNuevoArchivo',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        LoadListaArchivos(id);
                        fupAdjunto2.val('');
                        cboTipo.val('1');
                        modalListaArchivos.unblock();
                        modalListaArchivos.block({
                            message: "El archivo se Subio exitósamente",
                            timeout: 3000,
                            css: {
                                border: 'none',
                                padding: '15px',
                                backgroundColor: '#000',
                                '-webkit-border-radius': '10px',
                                '-moz-border-radius': '10px',
                                opacity: .5,
                                color: '#fff'
                            }
                        });
                    },
                    error: function (error) {
                        modalListaArchivos.unblock();
                    }
                });
            } else {
                e.preventDefault()
            }
        }

        function saveRepPrefactura(id) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Facturacion/Prefacturacion/savePrefactura",
                data: { obj: getObjPrefactura(), lst: getLstPrefactura(), lstImpuesto: getLstImpuesto() },
                success: function (response) {
                    $.unblockUI();
                    abrirPDF(id, response.id);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function setClienteNombre() {
            var clie = $("#selBusqNumcte option:selected").text()
            if (clie == "--Seleccione--") {
                txtxBusqNombre.val("");
            }
            else {
                txtxBusqNombre.val(selBusqNumcte.val());
            }
        }

        function setClienteNombrePlantilla() {
            var clie = $("#selPlantillaNumcte option:selected").text()
            if (clie == "--Seleccione--") {
                txtxPlantillaNombre.val("");
            }
            else {
                txtxPlantillaNombre.val(selPlantillaNumcte.val());
            }
        }

        function autoClienteNombre() {
            lstNombre.find("option").each(function () {
                if ($(this).val() == txtNombre.val()) {
                    selBusqNumcte.val($(this).val());
                    btnBusqClienteSelecciona.click();
                }
            });
        }

        function openModal() {
            initCombobox();
            modal.dialog("open");
        }

        function openPrefactura() {
            unaPrefactura.dialog("open");
        }

        function closeModal() {
            modal.dialog("close");
        }

        function cerrarCliente() {
            modalBusqCliente.dialog("close");
        }

        function buscarCliente() {
            if (event.which == 114) {
                modalBusqCliente.dialog("open");
                return false;
            }
        }

        function abrirModalPLantilla() {
            modalBusqPlantila.dialog("open");
        }

        function cerrarModalPLantilla() {
            modalBusqPlantila.dialog("close");
        }

        function viewReporte(id) {
            saveRepPrefactura(id);
            closeModal();
        }

        function getUltimoDataRow() {
            return Number(tblUnidades.bootgrid("getTotalRowCount"));
        }

        function rfcValido(rfc, aceptarGenerico) {
            const re = /^([A-ZÑ&]{3,4}) ?(?:- ?)?(\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])) ?(?:- ?)?([A-Z\d]{2})([A\d])$/;
            var validado = rfc.match(re);

            if (!validado)
                return false;

            const digitoVerificador = validado.pop(),
                rfcSinDigito = validado.slice(1).join(''),
                len = rfcSinDigito.length,

                diccionario = "0123456789ABCDEFGHIJKLMN&OPQRSTUVWXYZ Ñ",
                indice = len + 1;
            var suma,
                digitoEsperado;

            if (len == 12) suma = 0
            else suma = 481;

            for (var i = 0; i < len; i++)
                suma += diccionario.indexOf(rfcSinDigito.charAt(i)) * (indice - i);
            digitoEsperado = 11 - suma % 11;
            if (digitoEsperado == 11) digitoEsperado = 0;
            else if (digitoEsperado == 10) digitoEsperado = "A";

            if ((digitoVerificador != digitoEsperado)
                && (!aceptarGenerico || rfcSinDigito + digitoVerificador != "XAXX010101000"))
                return false;
            else if (!aceptarGenerico && rfcSinDigito + digitoVerificador == "XEXX010101000")
                return false;
            return rfcSinDigito + digitoVerificador;
        }


        function validarInput(input) {
            var rfc = $(input).val().trim().toUpperCase();

            var rfcCorrecto = rfcValido(rfc, true);

            if (rfcCorrecto) {
                $(input).removeClass("errorClass")
                return true;
            } else {
                $(input).addClass("errorClass");
                return false;
            }

        }

        function maskRFC() {
            $.each(rfc, function (i, e) {
                validarCampo($(e))
            });
        }

        function maskSel() {
            $.each(validaSel, function (i, e) {
                validarCampo($(e))
            });
        }

        function validarCodigoPostal(input) {
            const re = /(^\d{5}$)|(^\d{5}-\d{4}$)/;
            var cpCorrecto = re.test($(input).val());
            if (cpCorrecto) {
                $(input).removeClass("errorClass")
            } else {
                $(input).addClass("errorClass");
            }
            return cpCorrecto;
        }

        function maskCodigoPostal() {
            $.each(codigoPostal, function (i, e) {
                //validarCodigoPostal(e);
            });
        }

        function maskPorcentage() {
            $.each(porcentage, function (i, e) {
                if ($(e).val() == "") { $(e).val("0.00%"); }
                $(e).val(parseFloat($(e).val()).toFixed(2).replace(/\d+% ?/g, "") + "%");
            });
        }

        function maskDinero(numero) {
            var numeroFixed = parseFloat(numero).toFixed(4);
            return "$" + numeroFixed.split('.')[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "." + numeroFixed.split('.')[1];
        }

        function unmaskPorcentage(numero) {
            return numero.replace(/%/g, '')
        }

        function setCC() {
            var cc = $("#selCCPractura option:selected").text()
            if (cc == "--Seleccione--") {
                txtDescCC.val("");
            }
            else {
                txtDescCC.val(cc.substring(5, 45));
            }

        }

        $('.radioBtn a').on('click', function () {
            var sel = $(this).data('title');
            var tog = $(this).data('toggle');
            $('#' + tog).prop('value', sel);

            $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
            $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');
        });

        $('.radioBtn a[data-toggle="radCC"]').on('click', function () {
            getRadioValue("radCC") ? selCCPractura.prop('disabled', false) : selCCPractura.prop('disabled', true);
        });

        $('.radioBtn a[data-toggle="radMetodoPago"]').on('click', function () {
            getRadioValue("radMetodoPago") ? txtMetodoPago.prop('disabled', false) : txtMetodoPago.prop('disabled', true);
        });

        $('.radioBtn a[data-toggle="radTipoMoneda"]').on('click', function () {
            getRadioValue("radTipoMoneda") ? txtTipoMoneda.prop('disabled', false) : txtTipoMoneda.prop('disabled', true);
        });

        function RemueveRenglon() {
            $('#tblUnidades tr:last').remove();
        }

        function RemueveImporte() {
            $('#tblImpuesto tr:last').remove();
        }


        function getObjPrefactura() {
            var obj = {
                id: $("#txtIdReporte").val(),
                Folio: lblFolio.text(),
                Estado: txtEstado.val(),
                Fecha: dpFechaPrefactura.val(),
                CC: selCCPractura.val(),
                MetodoPago: txtMetodoPago.val(),
                TipoMoneda: txtTipoMoneda.val(),
                Nombre: txtNombre.val(),
                Direccion: txtCalle.val(),
                Ciudad: txtCiudad.val(),
                CP: txtCp.val(),
                RFC: txtRfc.val(),
                PosicionImporte: getRadioValue("radImporte"),
                VerCC: getRadioValue("radCC"),
                VerMetodoPago: getRadioValue("radMetodoPago"),
                VerTipoMoneda: getRadioValue("radTipoMoneda"),
                CalAuto: getRadioValue("radCalAuto"),
                Usocfdi: selUsocfdi.val()
            }
            if (txtEstado.val() == '0') {
                obj.Estado = '1';
            }
            return obj;
        }

        function getLstPrefactura() {
            var Array = [];
            tblUnidades.find("tr").each(function (idx, row) {
                if (idx > 0 && $(":nth-child(2)", row).html() != undefined) {
                    var JsonData = {};
                    JsonData.Renglon = idx;
                    JsonData.id = $(this).find('td:eq(0) input').val();
                    JsonData.Tipo = $(this).find('td:eq(0) select').val();
                    JsonData.Cantidad = $(this).find('td:eq(1) input[type="text"]').val();
                    JsonData.Concepto = $(this).find('td:eq(2) input[type="text"]').val();
                    var unidad = $(this).find('td:eq(3) div.editar').html().replace("&nbsp;", "");
                    JsonData.Unidad = encodeURIComponent(unidad);
                    JsonData.Precio = unmaskDinero($(this).find('td:eq(4) input[type="text"]').val());
                    JsonData.Importe = unmaskDinero($(this).find('td:eq(5) input[type="text"]').val());
                    switch (JsonData.Tipo) {
                        case "1": {
                            if (!validaVacio(unidad) || (!validaVacio(JsonData.Cantidad) && JsonData.Precio > 0))
                                Array.push(JsonData);
                            break;
                        }
                        case "3": {
                            if (!validaVacio(unidad) || (!validaVacio(JsonData.Cantidad) && JsonData.Precio > 0))
                                Array.push(JsonData);
                            break;
                        }
                        default: {
                            if (!validaVacio(unidad))
                                Array.push(JsonData);
                            break;
                        }

                    }
                }
            });
            return Array;
        }

        function getLstImpuesto() {
            var Array = [];
            tblImpuesto.find("tr").each(function (idx, row) {
                var JsonData = {};
                JsonData.Renglon = idx;
                JsonData.Id = $(this).find('td:eq(0) input[type="hidden"]').val();
                JsonData.Label = $(this).find('td:eq(0) input.lblImpuesto').val();
                JsonData.Valor = $(this).find('td:eq(0) input.valImpuesto').val();
                if (JsonData.Label != null && JsonData.Valor != null) {
                    Array.push(JsonData);
                }
            });
            var JsonData = {};
            JsonData.Renglon = Array.length;
            JsonData.Id = hdnTotalId.val();
            JsonData.Label = "Total";
            JsonData.Valor = txtValTotal.val();
            Array.push(JsonData);
            return Array;
        }

        function validaReporte() {
            var state = true;
            var focoArriba = false;
            if (!validarCampo(txtNombre)) { state = false; focoArriba = true; }
            if (!validarCampo(txtCalle)) { state = false; focoArriba = true; }
            //if (!validarCodigoPostal(txtCp)) { state = false; focoArriba = true; }
            if (!validarCampo(txtCiudad)) { state = false; focoArriba = true; }
            if (!validarCampo(txtRfc)) { state = false; focoArriba = true; }
            if (getRadioValue("radCC"))
                if (!validarCampo(txtMetodoPago)) { state = false; }
            if (getRadioValue("radMetodoPago"))
                if (!validarCampo(txtTipoMoneda)) { state = false; }
            if (getRadioValue("radTipoMoneda"))
                if (!validarCampo(selCCPractura)) { state = false; }
            if (!validarCampo(selUsocfdi)) { state = false; }
            if (state) { viewReporte($(this).val()); }
            if (focoArriba) { modal.animate({ scrollTop: 0 }, 'slow'); }
        }

        function validaPlantilla() {
            var state = true;
            if (!validarCampo(selPlantillaNumcte)) { state = false };
            if (!validarCampo(selPlantillaCC)) { state = false };
            if (!validarCampo(selPlantillaMoneda)) { state = false };
            if (!validarCampo(selUsocfdi)) { state = false; }
            if (state) getUltimaPrefacturaCliente();
        }

        function validarCampo(_this) {
            var r = false;
            if (_this.val() == '' || _this.val() == '$0.00' || _this.val() == '0' || _this.val() == '0.00%') {
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
        init();
    };
    $(document).ready(function () {
        facturacion.prefacturacion.index = new index();
    });
})();

function initCombobox() {
    selCC.fillCombo('/Administrativo/Facultamiento/getComboCCEnkontrol', null, false, null);
    selTipoInsumo.fillCombo('/Facturacion/Prefacturacion/cboTipoInsumo', null, false, null);
    selCCPractura.fillCombo('/Administrativo/Facultamiento/getComboCCEnkontrol', null, false, null);
    selBusqNumcte.fillCombo('/Facturacion/Facturacion/FillComboClienteNombre', null, false, null);
    txtMetodoPago.fillCombo('/Facturacion/Facturacion/FillComboMetodoPago', null, false, null);
    lstConceptoImporte.fillCombo('/Facturacion/Prefacturacion/cboConceptoImporte', null, false, null);
    selPlantillaCC.fillCombo('/Administrativo/Facultamiento/getComboCCEnkontrol', null, false, null);
    selPlantillaNumcte.fillCombo('/Facturacion/Facturacion/FillComboClienteNombre', null, false, null);
    txtNombre.getAutocomplete(setClienteId, null, '/Facturacion/Prefacturacion/FillComboClienteNombre');
    selUsocfdi.fillCombo('/Facturacion/Prefacturacion/FillComboUsocfdi', null, false, null);
}

function setClienteId(event, ui) {
    numCliente.val(ui.item.id);
    GetObjClientePorNombre();
}

function ActualizaEstatus(id, estatus) {
    modalConfirmacion.modal('show');
    txtIdReporte.val(id);
    txtEstado.val(estatus);
}

function actualizacionConfirmada() {
    modalConfirmacion.modal('toggle');
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: '/Facturacion/Prefacturacion/ActualizaEstatus',
        type: "POST",
        datatype: "json",
        data: { id: txtIdReporte.val(), estatus: txtEstado.val() },
        success: function (response) {
            getTablaPrefactura();
            if (response.Estado == 3) {
                ConfirmacionGeneral("Confirmación", "¡Prefactura fue rechzada!");
            }
            if (response.Estado == 2) {
                ConfirmacionGeneral("Confirmación", "¡Prefactura fue aceptada!");
            }
            txtIdReporte.val("0");
            txtEstado.val("");
            $.unblockUI();
        },
        error: function () {
            $.unblockUI();
        }
    });
}

function getPrefactura(id) {
    $.blockUI({ message: mensajes.PROCESANDO });
    initCombobox();
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Facturacion/Prefacturacion/getPrefactura",
        data: { id: id },
        success: function (response) {
            setPrefactura(response.obj);
            setInsumos(response.restabla);
            setTblImpuesto(response.lstImpuesto);
            validaSel.change();
            modal.dialog("open");
            $.unblockUI();
        },
        error: function () {
            $.unblockUI();
        }
    });
}

function getUltimaPrefacturaCliente() {
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Facturacion/Prefacturacion/getUltimaPrefacturaCliente",
        data: { nombre: txtxPlantillaNombre.val(), moneda: selPlantillaMoneda.val(), cc: selPlantillaCC.val() },
        success: function (response) {
            if (response.success) {
                lblPlantillaMensaje.text("");
                setPrefactura(response.obj);
                setInsumos(response.restabla);
                setTblImpuesto(response.lstImpuesto);
                btnPlantillaClienteCerrar.click();
            }
            else {
                lblPlantillaMensaje.text("No hay plantilla disponible");
            }
            $.unblockUI();
        },
        error: function () {
            GetObjCliente();
            $.unblockUI();
        }
    });
}

function GetObjClientePorNombre() {
    modalBusqCliente.block({ message: mensajes.PROCESANDO });
    if (numCliente.val() > 0) {
        $.ajax({
            url: "/Facturacion/Prefacturacion/GetObjCliente",
            type: "POST",
            data: { id: numCliente.val() },
            datatype: "json",
            success: function (response) {
                txtNombre.val(response.numcte + " - " + response.nombre);
                txtNumero.val(response.telefono1);
                txtColonia.val(response.colonia);
                txtCalle.val(response.direccion);
                txtCp.val(response.cp);
                txtCiudad.val(response.ciudad);
                txtRfc.val(response.rfc);
                modalBusqCliente.unblock();
            },
            error: function () {
                modalBusqCliente.unblock();
            }
        });
    }
}

function GetObjCliente() {
    modalBusqCliente.block({ message: mensajes.PROCESANDO });
    $.ajax({
        url: "/Facturacion/Facturacion/GetObjCliente",
        type: "POST",
        data: { numcte: $("#selBusqNumcte option:selected").text() },
        datatype: "json",
        success: function (response) {
            txtNombre.val(response.numcte + " - " + response.nombre);
            txtNumero.val(response.telefono1);
            txtColonia.val(response.colonia);
            txtCalle.val(response.direccion);
            txtCp.val(response.cp);
            txtCiudad.val(response.ciudad);
            txtRfc.val(response.rfc);
            modalBusqCliente.unblock();
        },
        error: function () {
            modalBusqCliente.unblock();
        }
    });
}

function GetAutoObjCliente() {
    modalBusqCliente.block({ message: mensajes.PROCESANDO });
    $.ajax({
        url: "/Facturacion/Facturacion/GetObjCliente",
        type: "POST",
        data: { numcte: $("#selBusqNumcte option:selected").text() },
        datatype: "json",
        success: function (response) {
            txtNombre.val(response.numcte + " - " + response.nombre);
            txtNumero.val(response.telefono1);
            txtColonia.val(response.colonia);
            txtCalle.val(response.direccion);
            txtCp.val(response.cp);
            txtCiudad.val(response.ciudad);
            txtRfc.val(response.rfc);
            modalBusqCliente.unblock();
        },
        error: function () {
            modalBusqCliente.unblock();
        }
    });
}

function getTablaPrefactura() {
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: "/Facturacion/Prefacturacion/getTablaPrefactura",
        type: "POST",
        datatype: "json",
        data: { inicio: dtFechaFiltroInicio.val(), fin: dtFechaFiltroFin.val(), cc: selCCFiltro.val() },
        success: function (response) {
            var tablaCompleta = CrearAccionesEnEspera(response.lstEnEspera)
            response.lstEnEspera = tablaCompleta;
            response.lstEnEspera = tblFolio(response.lstEnEspera);
            tblEnEspera.bootgrid("clear");
            tblEnEspera.bootgrid("append", response.lstEnEspera);

            tablaCompleta = CrearAccionesAceptar(response.lstAceptado);
            response.lstAceptado = tablaCompleta;
            response.lstAceptado = tblFolio(response.lstAceptado);
            tblAceptado.bootgrid("clear");
            tblAceptado.bootgrid("append", response.lstAceptado);

            tablaCompleta = CrearAccionesPDF(response.lstRechzado);
            response.lstRechzado = tablaCompleta;
            response.lstRechzado = tblFolio(response.lstRechzado);
            tblRechazado.bootgrid("clear");
            tblRechazado.bootgrid("append", response.lstRechzado);
            $.unblockUI();
        },
        error: function () {
            $.unblockUI();
        }
    });
}

function tblFolio(tbl) {
    for (var i in tbl) {
        tbl[i].Folio = generarFolio(tbl[i].id);
    }
    return tbl;
}

function Gestionar(id) {
    LoadListaArchivos(Number(id));
    btnSubirN.attr('data-id', id);
    modalListaArchivos.modal('show');
}

function LoadListaArchivos(id) {
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Facturacion/Prefacturacion/getListaArchivos",
        data: { id: id },
        success: function (response) {
            var dataSet = response.ListaArchivos;
            if (dataSet != undefined) {
                tblListaArchivo.bootgrid("clear");
                tblListaArchivo.bootgrid("append", dataSet);
                tblListaArchivo.bootgrid('reload');
            }
            $.unblockUI();
        },
        error: function () {
            $.unblockUI();
        }
    });
}

function GridListaArchivos() {
    $("#tblListaArchivo").bootgrid({
        headerCssClass: '.bg-table-header',
        align: 'center',
        formatters: {

            "Accion": function (column, row) {
                return "<button type='button' class='btn btn-info descargar' data-id='" + row.id + "'>" +
                    "<span class='glyphicon glyphicon-download'></span> " +
                    " </button>";
            }
        }
    }).on("loaded.rs.jquery.bootgrid", function () {
        /* Executes after data is loaded and rendered */
        $("#tblListaArchivo").find(".descargar").on("click", function (e) {
            var elemento = $(this).attr('data-id');
            downloadURI(elemento);
        });
    });
}

function downloadURI(elemento) {
    var link = document.createElement("button");
    link.download = '/Facturacion/Prefacturacion/getFileDownload?id=' + elemento;
    link.href = '/Facturacion/Prefacturacion/getFileDownload?id=' + elemento;
    link.click();
    location.href = '/Facturacion/Prefacturacion/getFileDownload?id=' + elemento;
}

function CrearAccionesPDF(data) {
    for (var i in data) {
        var p = document.createElement('button');
        p.innerHTML = p.innerHTML + '<button type="button" class="btn btn-info btnPDF" value="' + data[i].id + '"  onclick="abrirPDF(41, this.value)" data-toggle="tooltip" title="Reporte"><span class="glyphicon glyphicon-print"></span></button>';
        var f = document.createElement('button');
        f.innerHTML = f.innerHTML + '<button type="button" class="btn btn-default btnExcel" style="background-color:#1D6F42;" value="' + data[i].id + '"  onclick="getExcel(this.value)" data-toggle="tooltip" title="Excel"><span class="fa fa-file-excel"></span></button>';
        data[i].Acciones = p.innerHTML + f.innerHTML;
    }
    return data;
}

function CrearAccionesAceptar(data) {
    for (var i in data) {
        var p = document.createElement('button');
        p.innerHTML = p.innerHTML + '<button type="button" class="btn btn-info btnPDF" value="' + data[i].id + '"  onclick="abrirPDF(41, this.value)" data-toggle="tooltip" title="Reporte"><span class="glyphicon glyphicon-print"></span></button>';
        var d = document.createElement('button');
        d.innerHTML = d.innerHTML + '<button type="button" class="btn btn-primary btnGestionar" value="' + data[i].id + '"  onclick="Gestionar(this.value)" data-toggle="tooltip" title="Archivos"><span class="glyphicon glyphicon-tasks"></span></button>';

        var f = document.createElement('button');
        f.innerHTML = f.innerHTML + '<button type="button" class="btn btn-default btnExcel" style="background-color:#1D6F42;" value="' + data[i].id + '"  onclick="getExcel(this.value)" data-toggle="tooltip" title="Excel"><span class="fa fa-file-excel"></span></button>';
        data[i].Acciones = p.innerHTML + d.innerHTML + f.innerHTML;
    }
    return data;
}

function CrearAccionesEnEspera(data) {
    for (var i in data) {
        var d = document.createElement('button');
        d.innerHTML = d.innerHTML + '<button type="button" class="btn btn-primary btnGestionar" value="' + data[i].id + '"  onclick="Gestionar(this.value)" data-toggle="tooltip" title="Archivos"><span class="glyphicon glyphicon-tasks"></span></button>';
        var a = document.createElement('button');
        a.innerHTML = a.innerHTML + '<button type="button" class="btn btn-success btnAceptar" value="' + data[i].id + '"  onclick="ActualizaEstatus(this.value, 2)" data-toggle="tooltip" title="Aceptar"><span class="glyphicon glyphicon-check"></span></button>';
        var r = document.createElement('button');
        r.innerHTML = r.innerHTML + '<button type="button" class="btn btn-danger btnRechazar" value="' + data[i].id + '"  onclick="ActualizaEstatus(this.value, 3)" data-toggle="tooltip" title="Rechazar"><span class="glyphicon glyphicon-remove"></span></button>';
        var f = document.createElement('button');
        f.innerHTML = f.innerHTML + '<button type="button" class="btn btn-default btnExcel" style="background-color:#1D6F42;" value="' + data[i].id + '"  onclick="getExcel(this.value)" data-toggle="tooltip" title="Excel"><span class="fa fa-file-excel"></span></button>';
        var p = document.createElement('button');
        p.innerHTML = p.innerHTML + '<button type="button" class="btn btn-info btnPDF" value="' + data[i].id + '"  onclick="abrirPDF(41, this.value)" data-toggle="tooltip" title="Reporte"><span class="glyphicon glyphicon-print"></span></button>';
        var e = document.createElement('button');
        e.innerHTML = e.innerHTML + '<button type="button" class="btn btn-default btnEditar" value="' + data[i].id + '"  onclick="getPrefactura(this.value)" data-toggle="tooltip" title="Editar"><span class="glyphicon glyphicon-pencil"></span></button>';

        data[i].Acciones = d.innerHTML + a.innerHTML + r.innerHTML + f.innerHTML + p.innerHTML + e.innerHTML;

    }
    return data;
}

function generarFolio(id) {
    return ("0000000000" + id).substr(-10, 10);
}

function unmaskDinero(dinero) {
    return Number(dinero.replace(/[\$\(\),]/g, ""));
}

function setPrefactura(data) {
    txtIdReporte.val(data.id);
    txtEstado.val(data.Estado);
    txtNombre.val(data.Nombre);
    dpFechaPrefactura.datepicker("setDate", data.Fecha);
    txtCalle.val(data.Direccion);
    txtCp.val(data.CP);
    txtCiudad.val(data.Ciudad);
    txtRfc.val(data.RFC);
    selCCPractura.val(("000" + data.CC).substr(-3, 3));
    selCCPractura.change();
    selUsocfdi.val(data.Usocfdi);
    txtMetodoPago.val(data.MetodoPago);
    txtTipoMoneda.val(data.TipoMoneda);
    setRadioValue("radImporte", data.PosicionImporte);
    setRadioValue("radCC", data.VerCC);
    setRadioValue("radMetodoPago", data.VerMetodoPago);
    setRadioValue("radTipoMoneda", data.VerTipoMoneda);
    setRadioValue("radCalAuto", data.CalAuto);
    data.VerCC ? selCCPractura.prop('disabled', false) : selCCPractura.prop('disabled', true);
    data.VerMetodoPago ? txtMetodoPago.prop('disabled', false) : txtMetodoPago.prop('disabled', true);
    data.VerTipoMoneda ? txtTipoMoneda.prop('disabled', false) : txtTipoMoneda.prop('disabled', true);
}

function setInsumos(lst) {
    var arr = createTable(lst);
    tblUnidades.bootgrid("clear");
    tblUnidades.bootgrid("append", arr);
}

function setTblImpuesto(lst) {
    tblImpuesto.find("tbody").empty();
    for (var i in lst) {
        if (lst[i].Label == "Total" || lst[i].Label == "Importe total") {
            hdnTotalId.val(lst[i].id);
            txtValTotal.val(lst[i].Valor);
        } else {
            tblImpuesto.find('tbody')
                .append($('<tr>')
                    .append($('<td>')
                        .append(lst[i].Label == undefined || lst[i].Valor == undefined ? "" : '<div class="input-group"><input type="hidden" class="form-control" value="' + lst[i].id + '"/><input type="text" class="form-control lblImpuesto" list="lstConceptoImporte"/ onchange="setImporte();" value="' + lst[i].Label + '"><span class="input-group-addon"></span><input type="text" class="form-control valImpuesto" onchange="setImporte();" value="' + lst[i].Valor + '"/></div><input type=\"hidden\" class=\"form-control\" value ="' + lst[i].id + '" />'
                        )
                    )
                );
        }
    }
}

function setRadioValue(sel, tog) {
    $('#' + tog).prop('value', sel);
    $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('btn-success').addClass('btn-default');
    $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('btn-default').addClass('btn-success');
}

function getRadioValue(tog) {
    var value = $('a.active[data-toggle="' + tog + '"]').data('title');
    return value;
}

function createTable(response) {
    var Arr = [];
    if (response != null) {
        var largo = response[response.length - 1].Renglon
        for (var j = 0; j <= largo; j++) {
            if (response[j] != undefined) {
                var unidad = decodeURIComponent(response[j].Unidad).replace("&nbsp;", "");
                var JsonData = {};
                JsonData.Id = response[j].id;
                JsonData.Tipo = response[j].Tipo;
                JsonData.Cantidad = response[j].Cantidad;
                JsonData.Concepto = response[j].Concepto == "" ? "&nbsp" : response[j].Concepto;
                JsonData.Unidad = unidad == "null" ? "" : unidad;
                JsonData.Precio = response[j].Precio;
                JsonData.Importe = response[j].Importe;
                Arr.push(JsonData);
            }
        }
    }

    if (response == null) {
        tblUnidades.find("tr").each(function (idx, row) {
            if (idx > 0 && $(":nth-child(2)", row).html() != undefined) {
                var JsonData = {};
                JsonData.Id = $(this).find("td:eq(1) input", row).val();
                JsonData.Renglon = idx;
                JsonData.Cantidad = $(this).find("td:eq(2) input", row).val();
                JsonData.Concepto = $(this).find("td:eq(3) input", row).val();
                JsonData.Unidad = $(this).find("td:eq(4) input", row).val();
                JsonData.Precio = $(this).find("td:eq(5) input", row).val();
                JsonData.Importe = $(this).find("td:eq(6) input", row).val();
                Arr.push(JsonData);
            }
        });
        var largo = Arr.length;
        if (largo <= 15) {
            var limite = 15 - largo;
            for (var i = 0; i <= limite; i++) {
                Arr.push(setArrUndefined());
            }
        }
    }
    return Arr;
}

function setArrUndefined() {
    var JsonData = {};
    JsonData.Id = "undefined";
    JsonData.Cantidad = "undefined";
    JsonData.Concepto = "undefined";
    JsonData.Unidad = "undefined";
    JsonData.Precio = "undefined";
    JsonData.Importe = "undefined";
    return JsonData;
}

function setInsumosDefault() {
    txtIdReporte.val("0");
    lblFolio.text("0000000000");
    txtNombre.val("");
    dpFechaPrefactura.datepicker("setDate", new Date);
    txtCalle.val("");
    txtCp.val("");
    txtCiudad.val("");
    txtRfc.val("");
    selCCPractura.val("");
    txtDescCC.val("");
    txtMetodoPago.val("");
    txtTipoMoneda.val("");
    setRadioValue("radImporte", true);
    setRadioValue("radCC", true);
    setRadioValue("radMetodoPago", true);
    setRadioValue("radTipoMoneda", true);
    setRadioValue("radCalAuto", true);
    selCCPractura.prop('disabled', false);
    txtMetodoPago.prop('disabled', false);
    txtTipoMoneda.prop('disabled', false);
    hdnTotalId.val(0);
    txtValTotal.val("$0.00");

    var Arr = [];
    for (var i = 0; i < 15; i++) {
        var JsonData = {};
        JsonData.Id = undefined;
        JsonData.Cantidad = undefined;
        JsonData.Concepto = undefined;
        JsonData.Unidad = undefined;
        JsonData.Precio = undefined;
        JsonData.Importe = undefined;
        Arr.push(JsonData);
    }

    tblUnidades.bootgrid("clear");
    tblUnidades.bootgrid("append", Arr);

    var lst = [{ Label: "Subtotal", Valor: "$0.00" }, { Label: "16% I.V.A.", Valor: "$0.00" }]

    tblImpuesto.find("tbody").empty();
    for (var i in lst) {
        if (0 <= +(i) && typeof +(i) == "number") {
            tblImpuesto.find('tbody')
                .append($('<tr>')
                    .append($('<td>')
                        .append(lst[i].Label == undefined ? "" : '<div class="input-group"><input type="text" class="form-control importe lblImpuesto" list="lstConceptoImporte"/ value="' + lst[i].Label + '"><span class="input-group-addon" style="cursor:n-resize">■</span><input type="text" class="form-control importe valImpuesto" value="' + lst[i].Valor + '"/></div>'
                        )
                    )
                );
        }
    }

}

function maskDinero(numero) {
    var numeroFixed = parseFloat(numero).toFixed(4);
    var dinero = "$" + numeroFixed.split('.')[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "." + numeroFixed.split('.')[1];
    //var dinero = "$" + parseFloat(numero.value).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    numero.value = dinero;
}

function maskNumDinero(numero) {
    var numeroFixed = parseFloat(numero).toFixed(4);
    return "$" + numeroFixed.split('.')[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "." + numeroFixed.split('.')[1];
    //return "$" + parseFloat(numero).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}

function tipoRenglon() {
    tblUnidades.find('tr').each(function (idx, row) {
        if (idx > 0) {
            if ($(this).find("td:eq(0) select", row).val() == '1') {

                $(this).find("td:eq(1)", row).removeClass('hidden');
                $(this).find("td:eq(2)", row).removeClass('hidden');
                $(this).find("td:eq(4)", row).removeClass('hidden');
                $(this).find("td:eq(5)", row).removeClass('hidden');

                $(this).find("td:eq(3)", row).removeAttr('colspan');

                $(this).find("td:eq(1) input", row).removeClass('hidden');
                $(this).find("td:eq(2) input", row).removeClass('hidden');
                $(this).find("td:eq(4) input", row).removeClass('hidden');
                $(this).find("td:eq(5) input", row).removeClass('hidden');

                if (validaVacio($(this).find("td:eq(1) input", row).val()))
                    $(this).find("td:eq(1) input", row).val(1);
                $(this).find("td:eq(5) input", row).prop("readonly", true);
            }
            if ($(this).find("td:eq(0) select", row).val() == '2') {

                $(this).find("td:eq(1)", row).addClass('hidden');
                $(this).find("td:eq(2)", row).addClass('hidden');
                $(this).find("td:eq(4)", row).addClass('hidden');
                $(this).find("td:eq(5)", row).addClass('hidden');

                $(this).find("td:eq(3)", row).attr('colspan', 5);

                $(this).find("td:eq(1) input", row).removeClass('hidden');
                $(this).find("td:eq(2) input", row).removeClass('hidden');
                $(this).find("td:eq(4) input", row).removeClass('hidden');
                $(this).find("td:eq(5) input", row).removeClass('hidden');
            }
            if ($(this).find("td:eq(0) select", row).val() == '3') {

                $(this).find("td:eq(1)", row).removeClass('hidden');
                $(this).find("td:eq(2)", row).removeClass('hidden');
                $(this).find("td:eq(4)", row).removeClass('hidden');
                $(this).find("td:eq(5)", row).removeClass('hidden');

                $(this).find("td:eq(3)", row).removeAttr('colspan');

                $(this).find("td:eq(1) input", row).removeClass('hidden');
                $(this).find("td:eq(2) input", row).removeClass('hidden');
                $(this).find("td:eq(4) input", row).removeClass('hidden');
                $(this).find("td:eq(5) input", row).removeClass('hidden');

                $(this).find("td:eq(5) input", row).prop("readonly", false);
            }
            if ($(this).find("td:eq(0) select", row).val() == '4') {

                $(this).find("td:eq(1)", row).removeClass('hidden');
                $(this).find("td:eq(2)", row).removeClass('hidden');
                $(this).find("td:eq(4)", row).removeClass('hidden');
                $(this).find("td:eq(5)", row).removeClass('hidden');

                $(this).find("td:eq(3)", row).removeAttr('colspan');

                $(this).find("td:eq(1) input", row).addClass('hidden');
                $(this).find("td:eq(2) input", row).addClass('hidden');
                $(this).find("td:eq(4) input", row).addClass('hidden');
                $(this).find("td:eq(5) input", row).removeClass('hidden');
                $(this).find("td:eq(5) input", row).prop("readonly", false);
            }
        }
    });
}

function setImporte() {
    if (getRadioValue("radCalAuto")) {
        var subTotal = 0;
        var totalCuerpo = 0;
        tblUnidades.find('tr').each(function (idx, row) {
            if (idx > 0) {
                var tipo = $(this).find("td:eq(0) select", row).val();
                if (tipo == '1') {
                    var cantidad = $(this).find('td:eq(1) input').val();
                    if (validaVacio(cantidad)) {
                        $(this).find("td:eq(1) input").val(1);
                        cantidad = 1;
                    }
                    var precio = unmaskDinero($(this).find('td:eq(4) input').val());
                    var importe = cantidad * precio;
                    $(this).find('td:eq(4) input[type="text"]').val((maskNumDinero(precio)));
                    $(this).find('td:eq(5) input[type="text"]').val((maskNumDinero(importe)));
                    subTotal += importe;
                    totalCuerpo += importe;
                }
                if (tipo == '4') {
                    $(this).find('td:eq(5) input[type="text"]').val((maskNumDinero(totalCuerpo)));
                    totalCuerpo = 0;
                }
            }
        });
        var sumaParcial = 0;
        var sumarTodo = 0;
        tblImpuesto.find('tr').each(function (idx, row) {
            var concepto = $(this).find('td:eq(0) input.lblImpuesto').val();
            if (concepto != undefined) {
                var opcion = $("#lstConceptoImporte option[value=\"" + concepto + "\"]").val();
                var valor = unmaskDinero($(this).find('td:eq(0) div input.valImpuesto').val());
                switch (opcion) {
                    case 'Subtotal': {
                        $(this).find('td:eq(0) div input.valImpuesto').val(maskNumDinero(subTotal));
                        sumaParcial += subTotal;
                        sumarTodo += subTotal;
                        break;
                    }
                    case '16% I.V.A.': {
                        var iva = sumarTodo * 0.16;
                        $(this).find('td:eq(0) div input.valImpuesto').val(maskNumDinero(iva));
                        sumaParcial += iva;
                        sumarTodo += iva;
                        break;
                    }
                    case '4': {
                        $(this).find('td:eq(0) div input.valImpuesto').val(maskNumDinero(sumaParcial));
                        sumaParcial = 0;
                        sumaParcial += valor;
                        sumarTodo += valor;
                    }
                    default: {
                        $(this).find('td:eq(0) div input.valImpuesto').val(maskNumDinero(valor));
                        sumaParcial += valor;
                        sumarTodo += valor;
                        break;
                    }
                }
            }
        });
        txtValTotal.val(maskNumDinero(sumarTodo));
    }
}

function abrirPDF(reporte, id) {
    var contents = $("#modalPrefactura").html();
    var frame1 = $('<iframe></iframe>');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);

    var frameDoc = frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;

    $.when(
        AlertaGeneral("Alerta", "Imprimiendo...")
    ).then(function () {
        frameDoc.document.open();
        frameDoc.document.write('<html><head>');
        frameDoc.document.write('<style type="text/css" media="print">@page { size: A4 portrait; }</style>');
        frameDoc.document.write('</head><body>');
        frameDoc.document.write('<link href="/Content/style/bootstrap.min.css" rel="stylesheet"/>');
        frameDoc.document.write('<link href="/Content/style/css/Facturación/PrefacturacionCSS.css" rel="stylesheet" media="print"/>');
        frameDoc.document.write(contents);
        frameDoc.document.write('</body></html>');

        var doc = frameDoc.document;

        setTimeout(function () {
            printVentana(frameDoc, id);
            $(".ui-button").click();
            window.frames["frame1"].focus();
            window.frames["frame1"].print();
            frame1.remove();
        }, 250);
    }).then(function () {
        frameDoc.document.close();
    })
}

function getExcel(id) {
    window.location.href = "/Facturacion/Prefacturacion/GetExcel?index=" + id;
}


function printVentana(docprint, id) {
    initCombobox();
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Facturacion/Prefacturacion/getPrefactura",
        async: false,
        data: { id: id },
        success: function (response) {
            var lstUnidad = response.obj.PosicionImporte ? printAjustarUnidad(response.restabla, response.lstImpuesto) : response.restabla;
            var lstImpuesto = response.obj.PosicionImporte ? printAjustarImporte(response.lstImpuesto) : response.lstImpuesto;
            printPrefactura(docprint, response.obj);
            printInsumos(docprint, lstUnidad);
            printImpuesto(docprint, lstImpuesto)
            printDiseño(docprint);
            setInsumosDefault();
        },
        error: function () { }
    });
}

function printAjustarUnidad(lstUnidad, lstImpuesto) {
    var copy = JSON.parse(JSON.stringify(lstImpuesto));
    copy.splice(copy.findIndex(i => i.Label === "16% I.V.A."), 1);
    copy.splice(copy.findIndex(i => i.Label === "Total"), 1);
    copy.splice(copy.findIndex(i => i.Label === "Subtotal"), 1);
    var largo = copy.length - 1;
    var Renglon = lstUnidad[lstUnidad.length - 1].Renglon
    for (var i = 0; i <= largo; i++) {
        var JsonData = {};
        JsonData.Id = "";
        JsonData.Tipo = "";
        JsonData.Cantidad = "";
        JsonData.Unidad = copy[i].Label;
        JsonData.Renglon = ++Renglon;
        JsonData.Precio = "";
        JsonData.Importe = unmaskDinero(copy[i].Valor);
        lstUnidad.push(JsonData);
    }
    return lstUnidad;
}

function printAjustarImporte(lstImpuesto) {
    var arr = [];
    arr.push(lstImpuesto.find(o => o.Label === 'Subtotal'));
    arr.push(lstImpuesto.find(o => o.Label === '16% I.V.A.'));
    arr.push(lstImpuesto.find(o => o.Label === 'Total'));
    return arr;
}

function printPrefactura(docprint, data) {
    docprint.document.getElementById("txtIdReporte").value = data.id;
    docprint.document.getElementById("txtEstado").value = data.Estado;
    docprint.document.getElementById("divNombre").innerHTML = data.Nombre;
    docprint.document.getElementById("divFecha").innerHTML = new Date(+(data.Fecha.replace(/^\D+|\D+$/g, ""))).toLocaleDateString();
    docprint.document.getElementById("divDireccion").innerHTML = data.Direccion;
    docprint.document.getElementById("divCp").innerHTML = data.CP;
    docprint.document.getElementById("divCiudad").innerHTML = data.Ciudad;
    docprint.document.getElementById("divRfc").innerHTML = data.RFC;
    selCCPractura.val(("000" + data.CC).substr(-3, 3));

    var s = '<table><tbody></tbody></table>';

    var div = document.createElement('div');
    div.innerHTML = s;
    var tabla = div.childNodes;
    if (data.VerCC) {
        $(tabla).find('tbody').append($('<tr>').append($('<td>').append("<u>Centro de Costos:</u>")).append($('<td class="texr-right" style="width: 70%;">').append($("#selCCPractura option:selected").text())));
    }
    if (data.VerMetodoPago) {
        txtMetodoPago.val(data.MetodoPago);
        $(tabla).find('tbody').append($('<tr>').append($('<td>').append("<u>Método de pago:</u>")).append($('<td class="texr-right style="width: 70%;"">').append($("#txtMetodoPago option:selected").text())));
    }
    if (data.VerTipoMoneda) {
        txtTipoMoneda.val(data.TipoMoneda);
        $(tabla).find('tbody').append($('<tr>').append($('<td>').append("<u>Tipo de Moneda:</u>")).append($('<td class="texr-right style="width: 70%;"">').append($("#txtTipoMoneda option:selected").text())));
    }
    selUsocfdi.val(data.Usocfdi);
    $(tabla).find('tbody').append($('<tr>').append($('<td>').append("<u>Uso CFDI:</u>")).append($('<td class="texr-right style="width: 70%;"">').append($("#selUsocfdi option:selected").text())));
    docprint.document.getElementById("divPago").innerHTML = div.innerHTML;
    docprint.document.getElementById("lblFolio").innerHTML = generarFolio(data.id);
}

function printInsumos(docprint, arr) {
    var lst = createTable(arr);
    var tblPrint = docprint.document.getElementById("tblUnidades");
    $(tblPrint).find("tbody").empty();
    $(tblPrint).find("th:first-child").remove();
    var ban = validaUnidad(lst);
    var j = 1;
    var k = 1;
    if (!ban) {
        for (var i in lst) {
            if (0 <= +(i) && typeof +(i) == "number") {

                $(tblPrint).find('tbody')
                    .append($('<tr>')
                        .append($('<td style="border-right: 2px solid #000 !important;border-left: 2px solid #000 !important;border-top: 0px !important;border-bottom: 0px !important;">')
                            .append(lst[i].Cantidad == undefined ? "" : lst[i].Cantidad
                            )
                        )
                        .append($('<td style="border-right: 2px solid #000 !important;border-left: 2px solid #000 !important;border-top: 0px !important;border-bottom: 0px !important;">')
                            .append(lst[i].Concepto == undefined ? "" : lst[i].Concepto
                            )
                        )
                        .append($('<td style="border-top: 0px !important;border-bottom: 0px !important; width:85% !important;">')
                            .append(lst[i].Id == undefined ? lst[i].Unidad : lst[i].Unidad == "undefined" ? "" : lst[i].Unidad
                            )
                        )
                        .append($('<td style="border-right: 2px solid #000 !important;border-left: 2px solid #000 !important;border-top: 0px !important;border-bottom: 0px !important;">')
                            .append(lst[i].Precio == undefined || lst[i].Precio == "" ? "" : maskNumDinero(lst[i].Precio)
                            )
                        )
                        .append($('<td style="border-right: 2px solid #000 !important;border-left: 2px solid #000 !important;border-top: 0px !important;border-bottom: 0px !important;">')
                            .append(lst[i].Id == undefined ? maskNumDinero(lst[i].Importe) : lst[i].Importe == undefined ? "" : maskNumDinero(lst[i].Importe)
                            )
                        )
                    );
                var element = tblPrint.rows[k].cells[2];
                html2canvas($(element), {
                    onrendered: function (canvas) {
                        tblPrint.rows[j].cells[2].innerHTML = '<img src="' + canvas.toDataURL() + '">';
                        j++;
                    },
                });
                k++;
                switch (lst[i].Tipo) {
                    case 2: {
                        $(tblPrint).find('tr:last-child').each(function (idx, row) {
                            $(this).find("td:eq(4)", row).remove();
                            $(this).find("td:eq(3)", row).remove();
                            $(this).find("td:eq(1)", row).remove();
                            $(this).find("td:eq(0)", row).remove();
                            $(this).find("td", row).attr('colspan', 4);
                        });
                        break;
                    }
                    default: { break; }
                }
            }
        }
        docprint.document.getElementById("tblUnidades").getElementsByTagName("tr")[0].getElementsByTagName("th")[2].style.width = "85%";
    } else {
        $(tblPrint).find("th:eq(2)").remove();
        for (var i in lst) {
            if (0 <= +(i) && typeof +(i) == "number") {

                $(tblPrint).find('tbody')
                    .append($('<tr>')
                        .append($('<td style="border-right: 2px solid #000 !important;border-left: 2px solid #000 !important;border-top: 0px !important;border-bottom: 0px !important;">')
                            .append(lst[i].Cantidad == undefined ? "" : lst[i].Cantidad
                            )
                        )
                        .append($('<td style="border-top: 0px !important;border-bottom: 0px !important; width:85% !important;">')
                            .append(lst[i].Id == undefined ? lst[i].Unidad : lst[i].Unidad == "undefined" ? "" : lst[i].Unidad
                            ))
                        .append($('<td style="border-right: 2px solid #000 !important;border-left: 2px solid #000 !important;border-top: 0px !important;border-bottom: 0px !important;">')
                            .append(lst[i].Precio == undefined || lst[i].Precio == "" ? "" : maskNumDinero(lst[i].Precio)
                            )
                        )
                        .append($('<td style="border-right: 2px solid #000 !important;border-left: 2px solid #000 !important;border-top: 0px !important;border-bottom: 0px !important;">')
                            .append(lst[i].Id == undefined ? maskNumDinero(lst[i].Importe) : lst[i].Importe == undefined ? "" : maskNumDinero(lst[i].Importe)
                            )
                        )
                    );
                var element = tblPrint.rows[k].cells[1];
                html2canvas($(element), {
                    onrendered: function (canvas) {
                        tblPrint.rows[j].cells[1].innerHTML = '<img src="' + canvas.toDataURL() + '">';
                        j++;
                    },
                });
                k++;
                switch (lst[i].Tipo) {
                    case 2: {
                        $(tblPrint).find('tr:last-child').each(function (idx, row) {
                            $(this).find("td:eq(3)", row).remove();
                            $(this).find("td:eq(2)", row).remove();
                            $(this).find("td:eq(0)", row).remove();
                            $(this).find("td", row).attr('colspan', 4);
                        });
                        break;
                    }
                    default: { break; }
                }
            }
        }
        docprint.document.getElementById("tblUnidades").getElementsByTagName("tr")[0].getElementsByTagName("th")[1].style.width = "85%";
    }

    $(tblPrint).find('tr:last-child').attr('style', 'border-bottom: 1px solid #000 !important');
}

function validaUnidad(lst) {
    var bandera = false;
    for (var i in lst) {
        if (0 <= +(i) && typeof +(i) == "number") {
            bandera = validaVacio(lst[i].Concepto);
        }
    }
    return bandera;
}

function validaVacio(valor) {
    valor = valor == "&nbsp" || valor == undefined ? "" : valor;
    valor = valor.replace("&nbsp", "");
    if (!valor || 0 === valor.trim().length) {
        return true;
    }
    else {
        return false;
    }
}

function printImpuesto(docprint, lst) {
    var tblPrint = docprint.document.getElementById("tblImpuesto");
    $(tblPrint).find("tbody").empty();
    for (var i in lst) {
        if (0 <= +(i) && typeof +(i) == "number") {
            $(tblPrint).find('tbody')
                .append($('<tr>')
                    .append($('<td>')
                        .append(lst[i].Label == undefined ? "" : lst[i].Label
                        )
                    )
                    .append($('<td class="texr-right">')
                        .append(lst[i].Valor == "undefined" ? "" : lst[i].Valor
                        )
                    ));
        }

    }
}

function printDiseño(docprint) {
    docprint.document.getElementById("botonera1").remove();
    docprint.document.getElementById("botonera5").remove();
    docprint.document.getElementById("botonera6").remove();
    docprint.document.getElementById("divTotal").remove();
    docprint.document.getElementById("tblUnidades").getElementsByTagName("tr")[0].style.backgroundColor = "rgb(221,221,221)";
}
