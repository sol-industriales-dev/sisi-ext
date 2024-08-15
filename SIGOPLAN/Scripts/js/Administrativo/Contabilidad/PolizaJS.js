(function () {
    $.namespace('Administrativo.Contabilidad.Poliza.Captura');
    Captura = function () {
        const mensajes = {
            PROCESANDO: 'Procesando...',
            GUARDANDO: 'Guardando Póliza',
            GENPOLIZA: 'Generando Póliza',
            PAGANDO: 'Pagando facturas'
        };
        const dtIni = $("#dtIni");
        const dtFin = $("#dtFin");
        const numPoliza = $("#numPoliza");
        const tblPolizas = $("#tblPolizas");
        const selTp = $("#selTp");
        const modalCaptura = $("#modalCaptura");
        const txtCapPoliza = $("#txtCapPoliza");
        const clsnumPoliza = $(".numPoliza");
        const tblMovPolizas = $("#tblMovPolizas");
        const dpCapFecha = $("#dpCapFecha");
        const selCapTp = $("#selCapTp");
        const txtCapReferencia = $("#txtCapReferencia");
        const txtCapConcepto = $("#txtCapConcepto");
        const txtCapCC = $("#txtCapCC");
        const txtCapStatus = $("#txtCapStatus");
        const txtCapGenerada = $("#txtCapGenerada");
        const txtCapCargo = $("#txtCapCargo");
        const txtCapAbono = $("#txtCapAbono");
        const txtCapDif = $("#txtCapDif");
        const txtCapTm = $("#txtCapTm");
        const txtCapCuenta = $("#txtCapCuenta");
        const txtCapIDesc = $("#txtCapIDesc");
        const txtCapITipo = $("#txtCapITipo")
        const txtCapPeriodo = $("#txtCapPeriodo");
        const btnNuevo = $("#btnNuevo");
        const btnGetPoliza = $("#btnGetPoliza");
        const btnLimpiar = $("#btnLimpiar");
        const btnAddMov = $("#btnAddMov");
        const btnGuardar = $("#btnGuardar");
        const btnEliminar = $("#btnEliminar");
        const txtDolar = $("#txtDolar");
        const msjError = $("#msjError");
        const modalAyuda = $("#modalAyuda");
        const tblProveedor = $("#tblProveedor");
        const tblFactura = $("#tblFactura");
        const btnIva = $(".btnIva");
        const btnCargarIva = $("#btnCargarIva");
        const btnAbonarIva = $("#btnAbonarIva");
        const btnDiffCambiaria = $("#btnDiffCambiaria");
        const btnReporte = $("#btnReporte");
        const modalReporte = $("#modalReporte");
        const radioBtn = $('.radioBtn a');
        const selRepITipoPoliza = $("#selRepITipoPoliza");
        const selRepFTipoPoliza = $("#selRepFTipoPoliza");
        const selRepICC = $("#selRepICC");
        const selRepFCC = $("#selRepFCC");
        const selRepEstatus = $("#selRepEstatus");
        const txtRepIPoliza = $("#txtRepIPoliza");
        const txtRepFPoliza = $("#txtRepFPoliza");
        const txtRepIPeriodo = $("#txtRepIPeriodo");
        const txtRepFPeriodo = $("#txtRepFPeriodo");
        const txtRepFirma1 = $("#txtRepFirma1");
        const txtRepFirma2 = $("#txtRepFirma2");
        const radCC = $('.radioBtn a[data-toggle="radCC"]');
        const radResumido = $('.radioBtn a[data-toggle="radResumido"]');
        const radPorHoja = $('.radioBtn a[data-toggle="radPorHoja"]');
        const radFirma = $('.radioBtn a[data-toggle="radFirma"]');
        const radIntereses = $('.radioBtn a[data-toggle="radIntereses"]');
        const firma = $(".firma");
        const btnAbreReporte = $("#btnAbreReporte");
        const report = $("#report");
        const tblCadena = $("#tblCadena");
        const modalPago = $("#modalPago");
        const lblProv = $("#lblProv");
        const tblProductiva = $("#tblProductiva");
        const btnPagar = $("#btnPagar");
        const btnTodo = $("#btnTodo");
        const cboBancos = $("#cboBancos");
        const dtComplementaria = $("#dtComplementaria");
        const txtDllComplementaria = $("#txtDllComplementaria");
        const tblAplicado = $("#tblAplicado");
        const btnLstAplicado = $("#btnLstAplicado");
        const selAplMoneda = $("#selAplMoneda");
        const selCadMoneda = $("#selCadMoneda");
        const lblMoneda = $("#lblMoneda");
        const modalCuenta = $("#modalCuenta");
        const tblCuenta = $("#tblCuenta");
        const txtSelcTotal = $("#txtSelcTotal");
        const btnLstLimpiar = $("#btnLstLimpiar");
        const btnLstLimpiarCadena = $("#btnLstLimpiarCadena");
        const getEmpresa = new URL(window.location.origin + '/Base/getEmpresa');
        const DolarDelDia = new URL(window.location.origin + '/Administrativo/Poliza/getDolarDelDia');
        const getComboTipoPoliza = new URL(window.location.origin + '/Administrativo/Poliza/getComboTipoPoliza');
        const getComboAreaCuenta = new URL(window.location.origin + '/Administrativo/Poliza/getComboAreaCuenta');
        const getComboCentroCostos = new URL(window.location.origin + '/Administrativo/Poliza/getComboCentroCostos');
        const getCboTipoMovimiento = new URL(window.location.origin + '/Administrativo/Poliza/getCboTipoMovimiento');
        var tablaPoliza,
            tablaMovPoliza,
            tablaProductiva,
            tablaProveedor,
            diffCambiaria,
            tablaAplicado,
            tablaFactura,
            tablaCuentas,
            tablaCadena,
            diferencia,
            ivaCargo,
            ivaAbono,
            cargo,
            abono,
            iva,
            dolar,
            empresa,
            itemsCC,
            itemsAC,
            itemsTM;

        function init() {
            $.when(initForm()).then(
                getPoliza(),
                getAplicados(),
                getCuenta(),
                DefaultMovPoliza(),
                DefaultPago()
            );
            btnGetPoliza.click(getPoliza);
            btnNuevo.click(abreCaptura);
            btnLimpiar.click(DefaultMovPoliza);
            btnAddMov.click(AddMovimiento);
            btnEliminar.click(Hakai);
            btnGuardar.click(Guardar);
            btnIva.click(AddIva);
            btnReporte.click(AbreModalReporte);
            btnAbreReporte.click(verReporte);
            btnTodo.click(selecionarTodo);
            btnPagar.click(getCtaCadena);
            btnLstAplicado.click(setAplicado);
            btnLstLimpiar.click(quitSelAplicados);
            btnLstLimpiarCadena.click(quitSelCadena);
            selCapTp.change(setConceptoPoliza);
            dpCapFecha.change(setPeriodo);
            dtComplementaria.change(setDllComplementaria);
            txtCapPoliza.change(buscaPoliza);
            firma.change(validaFirma);
            selAplMoneda.change(getAplicados);
            selCadMoneda.change(getCadena);
            clsnumPoliza.keydown(function (event) {
                limitText(event.currentTarget, 5);
            });
            modalCaptura.on('shown.bs.modal', function () {
                $(this).find('.modal-dialog').css({ width: 'auto', height: 'auto', 'max-height': '100%' });
                //tablaMovPoliza.columns.adjust();
                modalCaptura.unblock();
            });
            modalCaptura.on("hidden.bs.modal", function () {
                DefaultMovPoliza();
            });
            modalAyuda.on('shown.bs.modal', function () {
                $(this).find('.modal-dialog').css({ width: 'auto', height: 'auto', 'max-height': '100%' });
            });
            modalCuenta.on('shown.bs.modal', function () {
                tablaCuentas.columns.adjust();
            });
            modalCuenta.on("hidden.bs.modal", function () {
                modalCuenta.unblock();
            });
            modalPago.on('shown.bs.modal', function () {
                $(this).find('.modal-dialog').css({ width: 'auto', height: 'auto', 'max-height': '100%' });
                tablaProductiva.columns.adjust();
                setDllComplementaria();
                modalPago.unblock();
            });
            modalPago.on("hidden.bs.modal", function () {
                DefaultPago();
            });
            radioBtn.on('click', function () {
                aClick(this);
            });
            radCC.on('click', function () {
                RangoCC(this);
            });
            radResumido.on('click', function () {
                TipoReporte(this);
            });
            radPorHoja.on('click', function () {
                TipoFirma(this);
            });
            radFirma.on('click', function () {
                AFirmar(this);
            });
            radIntereses.on('click', function () {
                AIntereses(this);
            });
        }
        function initForm() {
            dtIni.MonthPicker();
            dtFin.MonthPicker();
            txtRepIPeriodo.MonthPicker();
            txtRepFPeriodo.MonthPicker();
            $(".month-year-input").MonthPicker({
                Button: false,
                SelectedMonth: 0,
                i18n: mpDicEsp
            });
            dpCapFecha.datepicker({
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999);
                    }, 0);
                }
            }).datepicker("setDate", new Date());
            dtComplementaria.datepicker({
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999);
                    }, 0);
                }
            }).datepicker("setDate", new Date());
            selCapTp.fillCombo(getComboTipoPoliza, null, false, null);
            selTp.fillCombo(getComboTipoPoliza, null, false, null); selTp.val("04");
            selRepITipoPoliza.fillCombo(getComboTipoPoliza, null, false, null);
            selRepFTipoPoliza.fillCombo(getComboTipoPoliza, null, false, null);
            // selRepICC.fillCombo(FillComboCC, null, false, null);
            // selRepFCC.fillCombo(FillComboCC, null, false, null);
            fillItemsParaCombos();
            initMovPoliza();
            setEmpresas();
        }
        async function fillItemsParaCombos() {
            try {
                responseCC = await ejectFetchJson(getComboCentroCostos);
                if (responseCC.success) {
                    itemsCC = responseCC.items;
                }
                responseAC = await ejectFetchJson(getComboAreaCuenta);
                if (responseAC.success) {
                    itemsAC = responseAC.items;
                }
                responseTM = await ejectFetchJson(getCboTipoMovimiento);
                if (responseTM.success) {
                    itemsTM = responseTM.items;
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        async function setEmpresas() {
            try {
                response = await ejectFetchJson(getEmpresa);
                empresa = response;
                tablaMovPoliza.columns(10).visible(empresa !== 3);
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        function AbreModalReporte() {
            DefaultReporte();
            modalReporte.modal("show");
        }
        function DefaultReporte() {
            setRadioValue("radCC", true);
            setRadioValue("radResumido", true);
            setRadioValue("radPorHoja", false);
            setRadioValue("radFirma", false);
            setRadioValue("radEstatus", "V");
            radPorHoja.attr("disabled", true);
            radFirma.attr("disabled", true);
            txtRepFirma1.prop("disabled", true);
            txtRepFirma2.prop("disabled", true);
            selRepICC.val("001");
            selRepFCC.val("C72");
            txtRepIPoliza.val(1);
            txtRepFPoliza.val(50016);
            selRepITipoPoliza.val("01");
            selRepFTipoPoliza.val("2K");
            let fecha = new Date();
            txtRepIPeriodo.datepicker("setDate", fecha);
            txtRepFPeriodo.datepicker("setDate", fecha);
        }
        function DefaultPago() {
            cboBancos.val("").removeClass("errorClass");
            setRadioValue("radIntereses", true);
        }
        function buscaPoliza() {
            getMovPoliza({
                FECHA: dpCapFecha.val(),
                ESTATUS: txtCapStatus.val(),
                GENERADA: txtCapGenerada.val(),
                CARGO: txtCapCargo.val(),
                ABONO: txtCapAbono.val(),
                DIFERENCIA: txtCapDif.val(),
                POLIZA: txtCapPoliza.val(),
                TIPOPOLIZA: selCapTp.find("option:selected").text()
            });
        }
        function createCboTipoMovimiento(valor, iSistema, isInterface) {
            let sel = $('<div><select>')
                , lstTM = itemsTM.filter(tm => tm.Prefijo.split("-")[1] === iSistema).map(op => ({
                    Text: op.Text,
                    Value: op.Value,
                    Prefijo: op.Prefijo.split("-")[0]
                }));
            sel.find('select').addClass('form-control mov diff').width(65).fillComboItems(lstTM, null, valor);
            isInterface ? sel.find('select').removeClass('hidden') : sel.find('select').addClass('hidden');
            return sel;
        }
        function createCboCC(valor) {
            let sel = $('<div><select>');
            sel.find('select').addClass('form-control cc').width(75).fillComboItems(itemsCC, null, valor);
            return sel;
        }
        function createCboAC(ac, cc) {
            let sel = $('<div><select>')
                , lstAC = itemsAC.filter(ac => ac.Prefijo === cc).map(op => ({
                    Text: op.Text,
                    Value: op.Value,
                    Prefijo: op.Prefijo
                }));
            sel.find('select').addClass('form-control ac').width(75).fillComboItems(lstAC, "0-0", ac);
            return sel;
        }
        function DefaultMovPoliza() {
            getMovPoliza({
                FECHA: new Date(),
                ESTATUS: "C",
                GENERADA: "C",
                CARGO: "$0.00",
                ABONO: "$0.00",
                DIFERENCIA: "$0.00",
                CONCEPTO: "",
                POLIZA: "0",
                TIPOPOLIZA: "INGRESOS",
            });
            btnIva.data("aplicado", 1);
            btnIva.prop("disabled", false);
            btnAddMov.prop("disabled", false);
            diffCambiaria = 0;
            ivaCargo = 0;
            ivaAbono = 0;
            cargo = 0;
            abono = 0;
            iva = 0;
        }
        function getObjMovPoliza(row) {
            let monto = row.find('.monto').val()
                , Proveedor = row.find('.prov').val()
                , TipoMovimiento = row.find('.tm').val();
            return {
                No: +(row.find('.no').val()),
                D: +(row.find('.d').val()),
                Cuenta: +(row.find('.cuenta').val()),
                SCta: +(row.find('.scta').val()),
                SSCta: +(row.find('.sscta').val()),
                Mov: row.find('.mov').val(),
                Proveedor: Proveedor === undefined ? 0 : Proveedor,
                Referencia: row.find('.ref').val(),
                cc: row.find('.cc option:selected').text(),
                oc: row.find('.oc').val(),
                ac: row.find('.ac').val(),
                Concepto: row.find('.conpto').val(),
                TipoMovimiento: TipoMovimiento === undefined ? 0 : TipoMovimiento,
                monto: monto === undefined ? 0 : unmaskDinero(monto)
            };
        }
        function setTipoMovimiento(row) {
            let naturaleza = row.find('.mov').find(':selected').data();
            if (naturaleza.prefijo != undefined) {
                row.find('.tm').val(naturaleza.prefijo);
            }
        }
        function unmaskDinero(dinero) {
            return Number(dinero.replace(/[^0-9\.]+/g, ""));
        }
        function maskDinero(numero) {
            return "$" + parseFloat(numero).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        }
        function toDateFromJson(src) {
            return new Date(parseInt(src.substr(6)));
        }
        function limitText(limitField, limitNum) {
            if (limitField.value.length > limitNum)
                limitField.value = limitField.value.substring(0, limitNum);
        }
        function setCC(selCapCC) {
            if (selCapCC.find("option:selected").text() == "--Seleccione--")
                txtCapCC.val("");
            else
                txtCapCC.val(selCapCC.find("option:selected").text());
        }
        async function setPeriodo() {
            let fecha = dpCapFecha.val().split("/");
            txtCapPeriodo.val(fecha[1] + "/" + fecha[2]);
            await getDolarDelDia(dpCapFecha.val());
            txtDolar.val(dolar);
            if (dolar == 0) {
                txtDolar.addClass("tomato");
            }
            else {
                txtDolar.removeClass("tomato");
            }
            setDiferencia();
        }
        async function setDllComplementaria() {
            await getDolarDelDia(dtComplementaria.val());
            txtDllComplementaria.val(dolar);
            if (dolar == 0) {
                txtDllComplementaria.addClass("tomato");
            }
            else {
                txtDllComplementaria.removeClass("tomato");
            }
        }
        function abreCaptura() {
            DefaultMovPoliza();
            setConceptoPoliza();
            modalCaptura.modal('show');
        }
        function Hakai() {
            tablaMovPoliza.row('.selected').remove().draw(false);
            if (tablaMovPoliza.data().count() == 0) {
                AddMovimiento();
            }
            setDiferencia();
        }
        function validarGuardar() {
            let bandera = true,
                diff = unmaskDinero(txtCapDif.val());
            if ((diff <= -1 || diff >= 1) && (unmaskDinero(txtCapAbono.val()) == 0 || unmaskDinero(txtCapCargo.val()) == 0)) {
                bandera = false;
            }
            if (msjError.text().length > 0) {
                bandera = false;
            }
            bandera ? btnGuardar.removeClass("disabled") : btnGuardar.addClass("disabled");
        }
        function aClick(esto) {
            let sel = $(esto).data('title');
            let tog = $(esto).data('toggle');
            $(`#${tog}`).prop('value', sel);
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
        }
        function setRadioValue(tog, sel) {
            $(`#${tog}`).prop('value', sel);
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
        }
        function getRadioValue(tog) {
            return $(`a.active[data-toggle="${tog}"]`).data('title');
        }
        function RangoCC(esto) {
            let sel = !$(esto).data('title');
            selRepICC.attr("disabled", sel);
            selRepFCC.attr("disabled", sel);
        }
        function TipoReporte(esto) {
            let sel = $(esto).data('title');
            radPorHoja.attr("disabled", sel);
            if (getRadioValue("radPorHoja")) {
                radFirma.attr("disabled", sel);
                if (getRadioValue("radFirma")) {
                    txtRepFirma1.prop("disabled", sel);
                    txtRepFirma2.prop("disabled", sel);
                }
            }
            if (sel) {
                txtRepFirma1.removeClass("errorClass");
                txtRepFirma2.removeClass("errorClass");
            }
        }
        function TipoFirma(esto) {
            let sel = $(esto).data('title');
            radFirma.attr("disabled", !sel);
            var banFirma = !(sel && getRadioValue("radFirma"));
            txtRepFirma1.prop("disabled", banFirma);
            txtRepFirma2.prop("disabled", banFirma);
            if (!sel) {
                txtRepFirma1.removeClass("errorClass");
                txtRepFirma2.removeClass("errorClass");
            }
        }
        function AFirmar(esto) {
            let sel = !$(esto).data('title');
            txtRepFirma1.prop("disabled", sel);
            txtRepFirma2.prop("disabled", sel);
            txtRepFirma1.removeClass("errorClass");
            txtRepFirma2.removeClass("errorClass");
        }
        function AIntereses(esto) {
            let sel = $(esto).data('title');
        }
        function validaFirma() {
            if (getRadioValue("radFirma"))
                validarCampo($(this));
        }
        function getObjPoliza() {
            return {
                objPol: {
                    id: 0,
                    poliza: txtCapPoliza.val(),
                    tipoPoliza: selCapTp.val(),
                    fecha: dpCapFecha.val(),
                    estatus: txtCapStatus.val(),
                    generada: txtCapGenerada.val(),
                    cargo: unmaskDinero(txtCapCargo.val()),
                    abono: unmaskDinero(txtCapAbono.val()),
                    diferencia: unmaskDinero(txtCapDif.val()),
                    concepto: txtCapConcepto.val(),
                },
                lstMov: getLstMovPol()
            };
        }
        function getLstMovPol() {
            let lst = [];
            tblMovPolizas.find("tbody tr").each(function (idx, row) {
                let areaCuenta = $(this).find('.ac option:selected').val()
                    , area = areaCuenta == null || areaCuenta === "-" ? 0 : +areaCuenta.split("-")[0]
                    , cuenta = areaCuenta == null || areaCuenta === "-" ? 0 : +areaCuenta.split("-")[1]
                lst.push({
                    id: 0,
                    idPoliza: 0,
                    linea: $(this).find('.no').val(),
                    cta: $(this).find('.cuenta').val(),
                    scta: $(this).find('.scta').val(),
                    sscta: $(this).find('.sscta').val(),
                    digito: $(this).find('.d').val(),
                    movimiento: $(this).find('.mov').val(),
                    numProverdor: $(this).find('.prov').val(),
                    referencia: $(this).find('.ref').val(),
                    orderCompraCliente: $(this).find('.oc').val(),
                    concepto: $(this).find('.conpto').val(),
                    tipoMovimiento: $(this).find('.tm').val(),
                    Monto: unmaskDinero($(this).find('.monto').val()),
                    tipoMoneda: 1,
                    cc: $(this).find('.cc option:selected').text(),
                    area: area,
                    cuenta: cuenta
                });
            });
            return lst;
        }
        function getLstCtaFactura() {
            let l = [];
            tablaProductiva.rows().iterator('row', function (context, index) {
                let node = $(this.row(index).node())
                    , data = this.row(index).data()
                    , areaCuenta = data.area_cuenta
                    , area = areaCuenta == null || areaCuenta == "-" ? 0 : +areaCuenta.split("-")[0]
                    , cuenta = areaCuenta == null || areaCuenta == "-" ? 0 : +areaCuenta.split("-")[1];
                if (node.find("a.active").attr("disabled") != "disabled" && node.find("a.active").data().title)
                    l.push({
                        cta: cboBancos.val(),
                        scta: cboBancos.find(":selected").data().cta,
                        sscta: this.row(index).data().idPrincipal,
                        digito: this.row(index).data().id,
                        numpro: node.find("td:first").text(),
                        referencia: node.find("td").eq(4).text(),
                        cc: node.find("td").eq(2).text(),
                        area: area,
                        cuenta_oc: cuenta,
                        orden_compra: data.orden_compra,
                        monto: unmaskDinero(node.find("td").eq(7).text()),
                        concepto: `P${getRadioValue("radIntereses") ? "D" : "F"}-${txtCapPoliza.val()} ${getRadioValue("radIntereses") ? `LIQ.FACTORAJE ${cboBancos.find(":selected").text()}` : `${node.find("td").eq(1).text()}`}`
                    });
            });
            return l;
        }
        function AddMovimiento() {
            let objPrev = getObjMovPoliza(tblMovPolizas.find('tr:last'));
            tablaMovPoliza.row.add({
                No: objPrev.No + 1,
                Cuenta: 0,
                SCta: 0,
                SSCta: 0,
                D: 0,
                Mov: objPrev.Mov,
                Proveedor: objPrev.Proveedor,
                Referencia: objPrev.Referencia,
                cc: objPrev.cc,
                oc: objPrev.oc,
                ac: objPrev.ac,
                isOc: objPrev.oc.length > 0,
                Concepto: objPrev.Concepto,
                TipoMovimiento: objPrev.TipoMovimiento,
                Monto: '$0.00'
            }).draw();
        }
        function verReporte() {
            var ban = true;
            if (!getRadioValue("radResumido") && getRadioValue("radPorHoja") && getRadioValue("radFirma")) {
                let f1 = validarCampo(txtRepFirma1);
                let f2 = validarCampo(txtRepFirma2);
                if (!(f1 && f2)) ban = false;
            }
            if (ban) {
                modalReporte.block({ message: mensajes.PROCESANDO });
                var path = "/Reportes/Vista.aspx?idReporte=64"
                    + "&isCC=" + getRadioValue("radCC")
                    + "&isResumen=" + getRadioValue("radResumido")
                    + "&isPorHoja=" + getRadioValue("radPorHoja")
                    + "&isFirma=" + getRadioValue("radFirma")
                    + "&Estatus=" + getRadioValue("radEstatus")
                    + "&icc=" + selRepICC.val()
                    + "&fcc=" + selRepFCC.val()
                    + "&iPol=" + txtRepIPoliza.val()
                    + "&fPol=" + txtRepFPoliza.val()
                    + "&iPer=" + txtRepIPeriodo.val()
                    + "&fPer=" + txtRepFPeriodo.val()
                    + "&iTp=" + selRepITipoPoliza.val()
                    + "&fTp=" + selRepFTipoPoliza.val()
                    + "&firma1=" + txtRepFirma1.val()
                    + "&firma2=" + txtRepFirma2.val();
                report.attr("src", path);
                document.getElementById('report').onload = function () {
                    modalReporte.unblock();
                    openCRModal();
                };
            }
        }
        async function getDolarDelDia(fecha) {
            try {
                response = await ejectFetchJson(DolarDelDia, { fecha: fecha });
                if (response.success) {
                    dolar = response.dll;
                }
            }
            catch (o_O) {
                dolar = txtDolar.val();
            }
        }
        async function getiDescipcion(rowEvent) {
            try {
                var response = await postGetiDescipcion(rowEvent);
                txtCapCuenta.val(response.cuenta.descripcion);
                txtCapTm.val(response.desc);
                txtCapIDesc.val(response.idesc.descripcion);
                txtCapITipo.val(response.idesc.tipo);
                msjError.text(response.cuenta.lblError);
                return response;
            } catch (e) { }
        }
        function postGetiDescipcion(rowEvent) {
            setCC(rowEvent.find('.cc'));
            let obj = getObjMovPoliza(rowEvent);
            return $.post("/Administrativo/Poliza/getiDescipcion", {
                cta: obj.Cuenta,
                scta: obj.SCta,
                sscta: obj.SSCta,
                tp: selCapTp.val(),
                itm: obj.Mov,
                numprov: obj.Proveedor,
            }, function (response) { }, 'json');
        }
        function getMovPolDescipcion(rowEvent) {
            setCC(rowEvent.find('.cc'));
            let obj = getObjMovPoliza(rowEvent);
            let movDesc = postMovPolDescipcion(obj)
                .then(function (response) {
                    if (response.idesc.tipo == "") {
                        rowEvent.find('.mov').addClass("hidden");
                        rowEvent.find('.tm').prop({ disabled: false });
                    }
                    else {
                        let selMovValue = rowEvent.find('.mov').val();
                        rowEvent.find('td:eq(5)').html("<div>");
                        rowEvent.find('td:eq(5) div').append(createCboTipoMovimiento(selMovValue, response.idesc.tipo, response.idesc.tipo != "").html());
                        rowEvent.find('.mov').removeClass("hidden");
                        rowEvent.find('.tm').prop({ disabled: true });
                    }
                    response.cuenta.requiere_oc == "S" ? rowEvent.find('.oc').removeClass("hidden") : rowEvent.find('.oc').addClass("hidden");
                    try {
                        if (response.reff.factura > 0) {
                            rowEvent.find('.cc').val(response.reff.cc);
                            rowEvent.find('.oc').val(response.reff.referenciaoc);
                            rowEvent.find('.conpto').val(response.reff.concepto);
                            if (obj.monto > response.reff.total) {
                                AlertaGeneral("Aviso", "El valor de " + maskDinero(obj.monto) + " del monto exceden al total de " + maskDinero(response.reff.total) + " de la orden de compra ");
                                rowEvent.find('.monto').val("$0.00");
                            }
                        }
                    } catch (error) {
                        msjError.text(error);
                    }
                    txtCapCuenta.val(response.cuenta.descripcion);
                    txtCapTm.val(response.desc);
                    txtCapIDesc.val(response.idesc.descripcion);
                    txtCapITipo.val(response.idesc.tipo);
                    msjError.text(response.cuenta.lblError);
                })
                .done(function (response) {
                    validarGuardar();
                })
                .catch(function (o_O) { })
            AddIva();
        }
        function postMovPolDescipcion(obj) {
            return $.post("/Administrativo/Poliza/getMovPolDescipcion", {
                cta: obj.Cuenta,
                scta: obj.SCta,
                sscta: obj.SSCta,
                tp: selCapTp.val(),
                itm: obj.Mov,
                numprov: obj.Proveedor,
                referencia: obj.Referencia,
                cc: obj.cc,
                oc: obj.oc == "" ? 0 : obj.oc
            }, function (response) { }, 'json');
        }
        function setConceptoPoliza() {
            var TipoPoliza = $("#selCapTp option:selected").text();
            txtCapConcepto.val("Póliza de " + TipoPoliza);
            $.ajax({
                datatype: "json",
                type: "POST",
                async: false,
                url: '/Administrativo/Poliza/getNumPoliza',
                data: {
                    tp: selCapTp.val(),
                    fecha: dpCapFecha.val()
                },
                success: function (response) { txtCapPoliza.val(response.numPol); },
                error: function () {
                }
            });
        }
        function selecionarTodo() {
            tablaProductiva.rows().iterator('row', function (context, index) {
                var node = $(this.row(index).node());
                setRadioValue(node.find("a").data().toggle, true);
                setTotalSelecionado();
            });
        }
        function quitSelAplicados() {
            tablaAplicado.rows().iterator('row', function (context, index) {
                var node = $(this.row(index).node());
                setRadioValue(node.find("a").data().toggle, false);
            });
        }
        function quitSelCadena() {
            tablaProductiva.rows().iterator('row', function (context, index) {
                var node = $(this.row(index).node());
                setRadioValue(node.find("a").data().toggle, false);
            });
            txtSelcTotal.val(maskDinero(0));
        }
        function setTotalSelecionado() {
            let suma = 0;
            tablaProductiva.rows().iterator('row', function (context, index) {
                var node = $(this.row(index).node());
                if (node.find("a.active").attr("disabled") != "disabled" && getRadioValue(node.find("a").data().toggle))
                    suma += unmaskDinero(node.find("td").eq(7).text());
            });
            txtSelcTotal.val(maskDinero(suma));
        }
        function setDiferencia() {
            getDolarDelDia(dpCapFecha.val());
            diffCambiaria = 0,
                diferencia = 0,
                ivaAbono = 0,
                ivaCargo = 0,
                cargo = 0,
                abono = 0,
                diff = 0,
                iva = 0,
                isDiario2 = false,
                dllHoy = dolar;
            var arrCompl = getLstCtacompl();
            tablaMovPoliza.rows().iterator('row', function (context, index) {
                var node = $(this.row(index).node());
                var obj = getObjMovPoliza(node),
                    inputMonto = node.find('.monto'),
                    dllMonto = node.next('tr').find('.monto');
                res = arrCompl.filter(compl => compl.No == index + 1);
                response = res[0].response;
                try {
                    var objSig = getObjMovPoliza(node.next('tr'));
                } catch (err) { }
                if (response.objDll.moneda == 1) {
                    if ((obj.Cuenta == 5901 || obj.Cuenta == 4901) && btnDiffCambiaria.data("aplicado") == 3 && selCapTp.val() == "04") {
                        diff = cargo + abono;
                        inputMonto.val(maskDinero(diff * -1));
                        if (diff > 0) {
                            node.find('.cuenta').val(4901);
                            node.find('.tm').val("2");
                            cargo -= diff;
                        }
                        if (diff < 0) {
                            node.find('.cuenta').val(5901);
                            node.find('.tm').val("1");
                            cargo -= diff;
                        }
                    }
                    else if (obj.Cuenta == 5901 && selCapTp.val() == "03") {
                        isDiario2 = true;
                    }
                    else if (obj.Cuenta == 1147 && btnCargarIva.data("aplicado") == 3) {
                        let subCargo = cargo / 1.16;
                        ivaCargo = subCargo * 0.16;
                        cargo += ivaCargo;
                        inputMonto.val(maskDinero(ivaCargo)).prop("disabled", true);
                    }
                    else if (obj.Cuenta == 1146 && btnAbonarIva.data("aplicado") == 3) {
                        let subAbono = abono / 1.16;
                        ivaAbono = subAbono * 0.16;
                        cargo -= ivaAbono;
                        inputMonto.val(maskDinero(ivaAbono)).prop("disabled", true);
                    }
                    else {
                        obj.monto *= obj.TipoMovimiento == "2" || obj.TipoMovimiento == "3" ? -1 : 1;
                        inputMonto.val(maskDinero(obj.monto));
                        switch (obj.TipoMovimiento) {
                            case "1": { cargo += obj.monto; break; }
                            case "2": { abono += obj.monto; break; }
                            case "3": { cargo -= obj.monto; break; }
                            case "4": { abono -= obj.monto; break; }
                            default: break;
                        }
                    }
                } else if (response.objDll.moneda == 2 && (+(obj.Proveedor) == 0 || +(obj.Proveedor) > 8999) && dllHoy != 0) {
                    let enPesos = response.dllReg == -1 || isDiario2 ? dllHoy : response.dllReg;
                    let subDll = obj.monto * enPesos,
                        enDolares = subDll - obj.monto;
                    obj.monto *= obj.TipoMovimiento == "2" || obj.TipoMovimiento == "3" ? -1 : 1;
                    inputMonto.val(maskDinero(obj.monto));
                    switch (obj.TipoMovimiento) {
                        case "1": { cargo += obj.monto; break; }
                        case "2": { abono += obj.monto; break; }
                        case "3": { cargo -= obj.monto; break; }
                        case "4": { abono -= obj.monto; break; }
                        default: break;
                    }
                    if (isNaN(objSig.Cuenta)) {
                        tablaMovPoliza.row.add({
                            No: obj.No + 1,
                            Cuenta: response.objDll.ctacom,
                            SCta: response.objDll.sctacom,
                            SSCta: response.objDll.ssctacom,
                            D: response.objDll.digitocom,
                            Mov: 0,
                            Proveedor: obj.Proveedor,
                            Referencia: obj.Referencia,
                            cc: obj.cc,
                            oc: obj.oc,
                            ac: obj.ac,
                            isOc: obj.oc.length > 0,
                            Concepto: obj.Concepto,
                            TipoMovimiento: obj.TipoMovimiento,
                            Monto: maskDinero(obj.monto)
                        }).draw();
                    } else {
                        dllMonto.val(maskDinero(enDolares));
                        dllMonto.prop("disabled", true);
                    }

                }
            });
            cargo = 0;
            abono = 0;
            diff = 0;
            tablaMovPoliza.rows().iterator('row', function (context, index) {
                let node = $(this.row(index).node()),
                    tipo = node.find('.tm').val(),
                    monto = unmaskDinero(node.find('.monto').val());
                switch (tipo) {
                    case "1": { cargo += monto; break; }
                    case "2": { abono += monto; break; }
                    case "3": { cargo -= monto; break; }
                    case "4": { abono -= monto; break; }
                    default: break;
                }
            });
            diferencia = cargo - abono;
            txtCapCargo.val(maskDinero(cargo));
            txtCapAbono.val(maskDinero(-abono));
            txtCapDif.val(maskDinero(diferencia));
            validarGuardar();
        }
        function getLstCtacompl() {
            let arrCompl = [];
            tablaMovPoliza.rows().iterator('row', async function (context, index) {
                var node = $(this.row(index).node());
                var obj = getObjMovPoliza(node);
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/Administrativo/Poliza/getCtacompl',
                    async: false,
                    data: {
                        cta: obj.Cuenta,
                        scta: obj.SCta,
                        sscta: obj.SSCta,
                        iSistema: txtCapITipo.val(),
                        numprov: obj.Proveedor,
                        referencia: obj.Referencia,
                        tp: selCapTp.val(),
                        tm: obj.TipoMovimiento,
                        cc: node.find('.cc').val(),
                        monto: obj.monto
                    },
                    success: function (response) {
                        arrCompl.push({ No: obj.No, response: response });
                    },
                    error: function (response) {
                        var a = "";
                    }
                });
            });
            return arrCompl;
        }
        function Guardar() {
            modalCaptura.block({ message: mensajes.GUARDANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Administrativo/Poliza/Guardar',
                data: { o: getObjPoliza() },
                success: function (response) {
                    if (response.success) {
                        AlertaGeneral("Aviso", "Póliza guardada con éxito");
                        getAplicados();
                    }
                },
                error: function (response) { AlertaGeneral("Aviso", "Verifique la información de la póliza"); },
                complete: function () { modalCaptura.unblock(); }
            });
        }
        function setAplicado() {
            setRadioValue("radIntereses", true);
            $("#cboBancos optgroup").prop('disabled', false);
            $(`#cboBancos optgroup[label="${selAplMoneda.val() == "2" ? "Pesos" : "Dolares"}"]`).prop('disabled', true);
            lblMoneda.val(selAplMoneda.find(`:selected`).text());
            lblMoneda.data().moneda = selAplMoneda.val();
            let l = getLstAplicados();
            if (l.length == 0)
                AlertaGeneral("Aviso", "Seleccione al menos una cadena aplicada");
            else
                getProductiva(l);
        }
        function getLstAplicados() {
            let lstid = [];
            tablaAplicado.rows().iterator('row', function (context, index) {
                var node = $(this.row(index).node());
                if (node.find("a.active").data().title == true)
                    lstid.push({
                        id: node.find("div").data().id,
                        numProveedor: node.find(":eq(0)").text(),
                        fechaVencimiento: node.find(":eq(4)").text()
                    });
            });
            return lstid;
        }
        function getCtaCadena() {
            let lst = getLstCtaFactura();

            //#region Validaciones
            if (lblMoneda.data().moneda == 2 && txtDllComplementaria.val() == 0) {
                AlertaGeneral("Aviso", "No hay precio de dólar en la fecha.");
                return;
            }

            if (lst.length == 0) {
                AlertaGeneral("Aviso", "No ah seleccionado facturas.");
                selCapTp.val(selCapTp.val()).change();
                return;
            }

            if (lst[0].cta == 0) {
                AlertaGeneral("Aviso", `${cboBancos.find(':selected').text()} no puede generar cuentas de ${$('.radioBtn a.active[data-toggle="radIntereses"]').text()}`);
                return;
            }

            if (cboBancos.val() == "") {
                validarCampo(cboBancos);
                return;
            }
            //#endregion

            let isIntereses = getRadioValue("radIntereses");

            selCapTp.val(isIntereses ? "03" : "04");
            dpCapFecha.datepicker("setDate", dtComplementaria.val()).change();

            setConceptoPoliza();

            dpCapFecha.change();
            txtCapGenerada.val("C");

            // modalPago.block({ message: mensajes.PROCESANDO });

            axios.post('/Administrativo/Poliza/GetCtaCadenaNuevo', { f: lst, isIntereses, dllFecha: dpCapFecha.val() })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        tablaMovPoliza.clear().draw();
                        tablaMovPoliza.rows.add(response.data.lstCta).draw(false);

                        let isIntereses = !getRadioValue("radIntereses");

                        btnAbonarIva.data("aplicado", isIntereses ? 3 : 1);
                        btnCargarIva.data("aplicado", isIntereses ? 3 : 1);
                        btnAbonarIva.prop("disabled", isIntereses);
                        btnCargarIva.prop("disabled", isIntereses);
                        btnDiffCambiaria.data("aplicado", 3);
                        btnDiffCambiaria.prop("disabled", true);

                        modalPago.modal("hide");
                        modalCaptura.modal("show");
                        setDiferencia();
                        modalPago.unblock();
                        modalCaptura.unblock();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                        modalPago.unblock();
                        modalCaptura.unblock();
                    }
                }).catch(error => {
                    AlertaGeneral(`Alerta`, error.message);
                    modalPago.unblock();
                    modalCaptura.unblock();
                });
        }
        function getCtaIva() {
            return $.post("/Administrativo/Poliza/getCtaIva", {
                iSistema: txtCapITipo.val()
            }, function (response) { }, 'json');
        }
        async function AddIva() {
            try {
                var response = await getCtaIva();
                let objPrev = getObjMovPoliza(tblMovPolizas.find('tr:last')),
                    objCta;
                if (btnCargarIva.data("aplicado") == 2) {
                    objCta = {
                        No: objPrev.No,
                        Cuenta: response.objIva.cta_iva_actual,
                        SCta: response.objIva.scta_iva_actual,
                        SSCta: response.objIva.sscta_iva_actual,
                        D: 0,
                        Mov: 0,
                        Proveedor: 0,
                        Referencia: objPrev.Referencia,
                        cc: objPrev.cc,
                        oc: objPrev.oc,
                        ac: objPrev.ac,
                        isOc: objPrev.oc.length > 0,
                        Concepto: objPrev.Concepto,
                        TipoMovimiento: '01',
                        Monto: maskDinero(ivaCargo)
                    };
                }
                if (btnAbonarIva.data("aplicado") == 2) {
                    objCta = {
                        No: objPrev.No,
                        Cuenta: response.objIva.cta_iva,
                        SCta: response.objIva.scta_iva,
                        SSCta: response.objIva.sscta_iva,
                        D: 0,
                        Mov: 0,
                        Proveedor: 0,
                        Referencia: objPrev.Referencia,
                        cc: objPrev.cc,
                        oc: objPrev.oc,
                        ac: objPrev.ac,
                        isOc: objPrev.oc.length > 0,
                        Concepto: objPrev.Concepto,
                        TipoMovimiento: '02',
                        Monto: maskDinero(ivaAbono)
                    };
                }
                if (btnDiffCambiaria.data("aplicado") == 2) {
                    if (diffCambiaria > 0) {
                        objCta = {
                            No: objPrev.No,
                            Cuenta: response.objDiffCambiaria.cta_dif_cambiaria,
                            SCta: response.objDiffCambiaria.scta_dif_cambiaria,
                            SSCta: response.objDiffCambiaria.sscta_dif_cambiaria,
                            D: response.objDiffCambiaria.digito_dif_cambiaria,
                            Mov: 0,
                            Proveedor: 0,
                            Referencia: objPrev.Referencia,
                            cc: objPrev.cc,
                            oc: objPrev.oc,
                            ac: objPrev.ac,
                            isOc: objPrev.oc.length > 0,
                            Concepto: objPrev.Concepto,
                            TipoMovimiento: '02',
                            Monto: maskDinero(diffCambiaria)
                        };
                    }
                    if (diffCambiaria < 0) {
                        objCta = {
                            No: objPrev.No,
                            Cuenta: response.objDiffCambiaria.cta_perdida,
                            SCta: response.objDiffCambiaria.scta_perdida,
                            SSCta: response.objDiffCambiaria.sscta_perdida,
                            D: response.objDiffCambiaria.digito_perdida,
                            Mov: 0,
                            Proveedor: 0,
                            Referencia: objPrev.Referencia,
                            cc: objPrev.cc,
                            oc: objPrev.oc,
                            ac: objPrev.ac,
                            isOc: objPrev.oc.length > 0,
                            Concepto: objPrev.Concepto,
                            TipoMovimiento: '02',
                            Monto: maskDinero(diffCambiaria)
                        };
                    }
                }
                if (objCta != undefined) {
                    tablaMovPoliza.row.add(objCta).draw();
                    tblMovPolizas.find('tr:last').find("input").prop("readOnly", true);
                    tblMovPolizas.find('tr:last').find("select").prop("disabled", true);
                    btnAddMov.prop("disabled", true);
                }
                $(this).data("aplicado", 3);
                $(this).prop("disabled", true);
                setDiferencia();
            } catch (e) { }
        }
        function reporteCadena(idRegistros) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Administrativo/Reportes/SetDataPrint',
                type: 'POST',
                dataType: 'json',
                data: { id: idRegistros },
                success: function (response) {
                    report.attr("src", "/Reportes/Vista.aspx?idReporte=24");
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        async function getMovPoliza(obj) {
            try {
                modalCaptura.block({ message: mensajes.PROCESANDO });
                tablaMovPoliza.clear().draw();
                dpCapFecha.datepicker("setDate", obj.FECHA).change();
                txtCapStatus.val(obj.ESTATUS.substring(0, 1));
                txtCapGenerada.val(obj.GENERADA.substring(0, 1));
                txtCapCargo.val(obj.CARGO);
                txtCapAbono.val(obj.ABONO);
                txtCapDif.val(obj.DIFERENCIA);
                txtCapPoliza.val(obj.POLIZA);
                let tp = $('#selTp option').filter(function () { return $(this).html() == obj.TIPOPOLIZA; }).val();
                selCapTp.val(tp);
                response = await ejectFetchJson("/Administrativo/Poliza/getMovPoliza", {
                    fecha: dpCapFecha.val(),
                    poliza: obj.POLIZA,
                    tp: tp
                });
                if (response.success) {
                    tablaMovPoliza.rows.add(response.data).draw();
                    modalCaptura.unblock();
                }
                return response;
            } catch (o_O) { }
        }
        function getPoliza() {
            tablaPoliza = tblPolizas.DataTable({
                destroy: true,
                ajax: {
                    url: '/Administrativo/Poliza/getPoliza',
                    async: false,
                    data: {
                        perIni: dtIni.MonthPicker('GetSelectedDate').toLocaleDateString(),
                        perFin: dtFin.MonthPicker('GetSelectedDate').toLocaleDateString(),
                        poliza: numPoliza.val() == "" ? 0 : numPoliza.val(),
                        tp: selTp.val()
                    },
                },
                language: dtDicEsp,
                columns: [
                    { data: 'POLIZA' },
                    { data: 'TIPOPOLIZA' },
                    { data: 'FECHA' },
                    { data: 'CARGO', "className": "text-right" },
                    { data: 'ABONO', "className": "text-right" },
                    { data: 'DIFERENCIA', "className": "text-right" },
                    { data: 'ESTATUS' },
                    { data: 'GENERADA' },
                    { data: 'CONCEPTO' },
                    { sortable: false, data: null, "defaultContent": "<button class='btn btn-success'><span class='glyphicon glyphicon-list-alt'></span> Detalles</button>" }
                ]
            }).on('click', 'button', function () {
                modalCaptura.modal('show');
                getMovPoliza(tablaPoliza.row($(this).parents('tr')).data());
            });
        }
        function initMovPoliza() {
            tablaMovPoliza = tblMovPolizas.DataTable({
                destroy: true,
                searching: false,
                paging: false,
                iDisplayLength: -1,
                info: false,
                ordering: false,
                scrollY: true,
                deferRender: true,
                language: dtDicEsp,
                columns: [
                    { data: 'No', width: '5%', render: data => `<input type="text" class="form-control no" value ="${data}" readonly />` },
                    { data: 'Cuenta', width: '60px', render: data => `<input type="text" class="form-control cta cuenta" value ="${data}" />` },
                    { data: 'SCta', width: '60px', render: data => `<input type="text" class="form-control cta scta" value ="${data}"/>` },
                    { data: 'SSCta', width: '60px', render: data => `<input type="text" class="form-control cta sscta" value ="${data}"/>` },
                    { data: 'D', width: '35px', render: data => `<input type="text" class="form-control d" value ="${data}" readonly/>` },
                    {
                        data: 'Mov', render: function (data, type, row) {
                            let html = `<select class="form-control mov diff ${!row.isInterface ? 'hidden' : ''}">`;
                            let lstTM = itemsTM.filter(tm => tm.Prefijo.split("-")[1] === row.iSistema).map(op => ({
                                Text: op.Text,
                                Value: op.Value,
                                Prefijo: op.Prefijo.split("-")[0]
                            }));

                            html += `<option value="">--Seleccione--</option>`;

                            lstTM.forEach(function (element) {
                                html += `<option value="${element.Value}" Prefijo="${element.Prefijo}" ${element.Value == data ? 'selected' : ''}>${element.Text}</option>`;
                            });

                            html += '</select>';

                            // sel.addClass('form-control mov diff').width(65).fillComboItems(lstTM, null, data);
                            // !row.isInterface ? sel.removeClass('hidden') : sel.addClass('hidden');
                            return html;
                        }
                    },
                    { data: 'Proveedor', width: '75px', render: data => `<input type="text" class="form-control prov f1" value ="${data}"/>` },
                    { data: 'Referencia', width: '75px', render: data => `<input type="text" class="form-control ref f1" value ="${data}"/>` },
                    {
                        data: 'cc', render: function (data, type, row, meta) {
                            let html = `<select class="form-control cc">`;

                            html += `<option value="">--Seleccione--</option>`;

                            itemsCC.forEach(function (element) {
                                html += `<option value="${element.Value}" Prefijo="${element.Prefijo}" ${element.Value == data ? 'selected' : ''}>${element.Text}</option>`;
                            });

                            html += '</select>';

                            return html;

                            // let sel = $('<select></select>');
                            // sel.addClass('form-control cc').width(75).fillComboItems(itemsCC, null, data);
                            // return sel.html();
                        }
                    },
                    { data: 'oc', render: (data, type, row) => `<input type="text" class="form-control oc ${row.isOc ? "" : " hidden"}" value ="${data}"/>` },
                    {
                        data: 'ac', render: function (data, type, row, meta) {
                            let html = `<select class="form-control ac">`;
                            let lstAC = itemsAC.filter(ac => ac.Prefijo === row.cc).map(op => ({
                                Text: op.Text,
                                Value: op.Value,
                                Prefijo: op.Prefijo
                            }));

                            html += `<option value="0-0">0-0</option>`;

                            lstAC.forEach(function (element) {
                                html += `<option value="${element.Value}" Prefijo="${element.Prefijo}" ${element.Value == data ? 'selected' : ''}>${element.Text}</option>`;
                            });

                            html += '</select>';

                            return html;



                            // let sel = $('<select></select>');

                            // sel.addClass('form-control ac').width(75).fillComboItems(lstAC, "0-0", data);
                            // return sel.html();
                        }
                    },
                    { data: 'Concepto', render: data => `<input type="text" class="form-control conpto" value ="${data}"/>` },
                    { data: 'TipoMovimiento', render: (data, type, row) => `<select type="text" class="form-control tm diff" ${row.isInterface ? 'disabled' : ''}><option value="1" ${data == 1 ? "selected" : ""}>1 Cargos</option><option value="2" ${data == 2 ? "selected" : ""}>2 Abonos</option><option value="3" ${data == 3 ? "selected" : ""}>3 Cargos Rojos</option><option value="4" ${data == 4 ? "selected" : ""}>4 Abonos Rojos</option></select>` },
                    { data: 'Monto', render: data => `<input type="text" class="form-control text-right monto diff" value ="${data}"/>` }
                ],
                initComplete: function (settings, json) {
                    tblMovPolizas.on('keydown', '.cta', function (event) {
                        limitText(event.currentTarget, 4);
                    });
                    tblMovPolizas.on('keydown', '.ref', function (event) {
                        limitText(event.currentTarget, 6);
                    });
                    tblMovPolizas.on('keydown', '.prov', function (event) {
                        limitText(event.currentTarget, 4);
                    });
                    tblMovPolizas.on('keydown', '.oc', function (event) {
                        limitText(event.currentTarget, 9);
                    });
                    tblMovPolizas.on('keydown', '.f1', function (e) {
                        let keyCode = e.keyCode || e.which;
                        if (keyCode == 112 && txtCapITipo.val() == "P") {
                            modalAyuda.modal("show");
                            return false;
                        }
                    });
                    tblMovPolizas.on('keydown', '.cta ', function (e) {
                        let keyCode = e.keyCode || e.which;
                        if (keyCode == 112) {
                            $(this).parents('tr').addClass('selected');
                            modalCaptura.block({ message: mensajes.PROCESANDO });
                            modalCuenta.modal("show");
                            return false;
                        }
                    });
                    tblMovPolizas.on('click', 'tr', function () {
                        getiDescipcion($(this));
                    });
                    tblMovPolizas.on('change', 'tr', function () {
                        getMovPolDescipcion($(this));
                    });
                    tblMovPolizas.on('change', '.mov', function () {
                        setTipoMovimiento($(this).closest('tr'));
                    });
                    tblMovPolizas.on('change', '.diff', function () {
                        AddIva();
                    });
                    tblMovPolizas.on('change', '.cc', function () {
                        setCC($(this));
                        if (empresa !== 3) {
                            let cc = this.value
                                , ac = "0-0"
                                , tdAc = $(this).closest('tr').find('.ac').closest('td');
                            tdAc.html(createCboAC(ac, cc).html());
                        }
                    });
                    $('#tblMovPolizas tbody').on('click', 'tr', function () {
                        if ($(this).hasClass('selected'))
                            $(this).removeClass('selected');
                        else {
                            tblMovPolizas.find('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                        }
                    });
                    modalCaptura.unblock();
                }
            });
        }
        function getProveedor() {
            tablaProveedor = tblProveedor.DataTable({
                destroy: true,
                ajax: { url: '/Administrativo/Poliza/getProveedor' },
                language: dtDicEsp,
                columns: [
                    { data: 'numpro' },
                    { data: 'nomcorto', className: "cortar", render: (data, type, row) => `<div data-toggle="tooltip" title="${row.nombre}">${data}</div>` },
                    { data: 'moneda', render: data => data == "1" ? "MX" : "DLL" }
                ],
                initComplete: function (settings, json) {
                    tblProveedor.find('tbody').on('click', 'tr', function () {
                        if ($(this).hasClass('selected')) {
                            $(this).removeClass('selected');
                        }
                        else {
                            tablaProveedor.$('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                            getFactura(tablaProveedor.row($(this)).data().numpro);
                        }
                    });
                }
            });
        }
        function getFactura(numpro) {
            tablaFactura = tblFactura.DataTable({
                destroy: true,
                ajax: {
                    url: '/Administrativo/Poliza/getFactura',
                    data: { numPro: numpro }
                },
                language: dtDicEsp,
                columns: [
                    { data: 'factura', render: data => `<button class='btn btn-success'><span class='glyphicon glyphicon-list-alt'></span> Factura ${data}</button>` },
                    { data: 'fecha', render: data => toDateFromJson(data).toLocaleDateString() },
                    { data: 'fechavenc', render: data => toDateFromJson(data).toLocaleDateString() },
                    { data: 'cc' },
                    { data: 'referenciaoc' },
                    { data: 'total', className: "text-right", render: data => maskDinero(data) },
                    { data: 'concepto', className: "cortar" },
                    { data: 'tmDesc' }
                ]
            }).on('click', 'button', function () {
                let objProveedor = tablaProveedor.row(".selected").data();
                let objFactura = tablaFactura.row($(this).parents('tr')).data();
                let MovSel = tblMovPolizas.find(".selected");
                MovSel.find(".prov").val(objProveedor.numpro);
                MovSel.find(".ref").val(objFactura.factura);
                MovSel.find(".cc").find('option[name="' + objFactura.cc + '"]').attr('selected', true).change();
                MovSel.find(".oc").val(objFactura.referenciaoc);
                MovSel.find(".conpto").val(objFactura.concepto);
                modalAyuda.modal("hide");
            });
        }
        function getCadena() {
            tablaCadena = tblCadena.DataTable({
                destroy: true,
                ajax: {
                    url: '/Administrativo/Poliza/getCadena',
                    data: { moneda: selCadMoneda.val() }
                },
                language: dtDicEsp,
                columns: [
                    { data: 'numProveedor' },
                    { data: 'proveedor' },
                    { data: 'saldoFactura', "className": "text-right" },
                    { data: 'fechaS' },
                    { data: 'fechaVencimientoS' },
                    { data: 'id', render: data => `<button class='btn btn-primary print' data-id=${data}><span class='glyphicon glyphicon-print'></span> Imprimir</button>` },
                    { data: 'id', render: data => `<button class='btn btn-success pagar' data-id=${data}><span class='glyphicon glyphicon-check'></span> facturas</button>` },
                ],
                initComplete: function (settings, json) {
                    tablaCadena.on('click', '.print', function () {
                        let objCadena = tablaCadena.row($(this).parents('tr')).data();
                        reporteCadena(objCadena.id);
                    });
                    tablaCadena.on('click', '.pagar', function () {
                        let objCadena = tablaCadena.row($(this).parents('tr')).data();
                        setRadioValue("radIntereses", false);
                        let l = [];
                        l.push({
                            id: objCadena.id,
                            numProveedor: objCadena.numProveedor,
                            fechaVencimiento: objCadena.fechaVencimientoS
                        });
                        $("#cboBancos optgroup").prop('disabled', false);
                        $(`#cboBancos optgroup[label="${selCadMoneda.val() == "2" ? "Pesos" : "Dolares"}"]`).prop('disabled', true);
                        lblMoneda.val(selCadMoneda.find(`:selected`).text());
                        lblMoneda.data().moneda = selCadMoneda.val();
                        getProductiva(l);
                    });
                    this.api().columns().every(function () {
                        var column = this;
                        if (column.index() < 5) {
                            var select = $('<select class="form-control"><option value="">N/A</option></select>')
                                .appendTo($(column.header()).empty())
                                .on('change', function () {
                                    var val = $.fn.dataTable.util.escapeRegex(
                                        $(this).val()
                                    );
                                    column
                                        .search(val ? '^' + val + '$' : '', true, false)
                                        .draw();
                                });
                            column.data().unique().sort().each(function (d, j) {
                                select.append('<option value="' + d + '">' + d + '</option>')
                            });
                        }
                    });
                }
            });
        }
        function getAplicados() {
            tablaAplicado = tblAplicado.DataTable({
                destroy: true,
                ajax: {
                    url: '/Administrativo/Poliza/getCadenaAplicada',
                    data: { moneda: selAplMoneda.val() }
                },
                language: dtDicEsp,
                columns: [
                    { data: 'numProveedor' },
                    { data: 'proveedor' },
                    { data: 'saldoFactura', "className": "text-right" },
                    { data: 'fechaS' },
                    { data: 'fechaVencimientoS' },
                    {
                        data: 'id',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<button><span></span> Imprimir</button>");
                            $(td).find("button").addClass('btn btn-primary print');
                            $(td).find("span").addClass('glyphicon glyphicon-print');
                        }
                    },
                    {
                        data: 'id',
                        width: "100px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(`<div data-id='${cellData}'><a data-toggle='radDiaria${cellData}' data-title=${true}><i></i></a><a data-toggle='radDiaria${cellData}' data-title=${false}><i></i></a></div>`);
                            $(td).find("div").addClass('radioBtn btn-group');
                            $(td).find("a").addClass('btn btn-primary');
                            $($(td).find("a")[0]).addClass('notActive');
                            $($(td).find("i")[0]).addClass('fa fa-check');
                            $($(td).find("a")[1]).addClass('active');
                            $($(td).find("i")[1]).addClass('fa fa-times');
                        }
                    },
                    {
                        data: 'id',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(`<button><i></i> ${rowData.isAnticipo ? "Anticipo" : "Factoraje"}</button>`);
                            $(td).find("button").addClass('btn btn-success factoraje');
                            $(td).find("i").addClass('fa fa-table');
                            $(td).find("button").prop("disabled", rowData.isAnticipo)
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tablaAplicado.on('click', '.radioBtn a', function () {
                        let ban = true;
                        if (!getRadioValue("radIntereses")) {
                            let provActivos = tablaAplicado.rows(`[data-title='${true}'].active`).count();
                            ban = provActivos > 1;
                            if (!ban) AlertaGeneral("Aviso", "Solo se puede seleccionar 1 factura para generar pólizas de factorajes")
                        }
                        if (ban) {
                            let sel = $(this).data('title');
                            let tog = $(this).data('toggle');
                            $('#' + tog).prop('value', sel);
                            $(`a[data-toggle="${tog}"]`).not(`a[data-title="${sel}"]`).removeClass('active').addClass('notActive');
                            $(`a[data-toggle="${tog}"]a[data-title="${sel}"]`).removeClass('notActive').addClass('active');
                        }
                    });
                    tablaAplicado.on('click', '.print', function () {
                        let objCadena = tablaAplicado.row($(this).parents('tr')).data();
                        reporteCadena(objCadena.id);
                    });
                    tablaAplicado.on('click', '.factoraje', function () {
                        let objCadena = tablaAplicado.row($(this).parents('tr')).data();
                        setRadioValue("radIntereses", false);
                        let l = [];
                        l.push({
                            id: objCadena.id,
                            numProveedor: objCadena.numProveedor,
                            fechaVencimiento: objCadena.fechaVencimientoS
                        });
                        $("#cboBancos optgroup").prop('disabled', false);
                        $(`#cboBancos optgroup[label="${selAplMoneda.val() == "2" ? "Pesos" : "Dolares"}"]`).prop('disabled', true);
                        lblMoneda.val(selAplMoneda.find(`:selected`).text());
                        lblMoneda.data().moneda = selAplMoneda.val();
                        getProductiva(l);
                    });
                    this.api().columns().every(function () {
                        var column = this;
                        if (column.index() < 5) {
                            var select = $('<select class="form-control"><option value="">N/A</option></select>')
                                .appendTo($(column.header()).empty())
                                .on('change', function () {
                                    var val = $.fn.dataTable.util.escapeRegex(
                                        $(this).val()
                                    );
                                    column
                                        .search(val ? '^' + val + '$' : '', true, false)
                                        .draw();
                                });
                            column.data().unique().sort().each(function (d, j) {
                                select.append('<option value="' + d + '">' + d + '</option>')
                            });
                        }
                    });
                }
            });
        }
        function getProductiva(lstid) {
            tablaProductiva = tblProductiva.DataTable({
                destroy: true,
                searching: false,
                paging: false,
                iDisplayLength: -1,
                info: false,
                scrollY: "380px",
                scrollCollapse: true,
                ajax: {
                    datatype: "json",
                    type: "POST",
                    url: '/Administrativo/Poliza/getProductiva',
                    data: { lstid: lstid }
                },
                language: dtDicEsp,
                columns: [
                    { data: 'numProveedor' },
                    { data: 'proveedor' },
                    { data: 'centro_costos' },
                    { data: 'nombCC' },
                    { data: 'factura' },
                    { data: 'fecha', render: data => toDateFromJson(data).toLocaleDateString() },
                    { data: 'fechaVencimiento', render: data => toDateFromJson(data).toLocaleDateString() },
                    { data: 'saldoFactura', className: "text-right", render: data => maskDinero(data) },
                    {
                        data: 'id', width: '70px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(`<div data-idPrincipal='${rowData.idPrincipal} data-id='${cellData}'><a data-toggle='radPago${cellData}' data-title=${true}><i></i></a><a data-toggle='radPago${cellData}' data-title=${false}><i></i></a></div>`);
                            $(td).find("div").addClass('radioBtn btn-group');
                            $(td).find("a").addClass('btn btn-primary');
                            $($(td).find("a")[0]).addClass('active');
                            $($(td).find("i")[0]).addClass('fa fa-check');
                            $($(td).find("a")[1]).addClass('notActive');
                            $($(td).find("i")[1]).addClass('fa fa-times');
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    setTotalSelecionado(),
                        modalPago.modal("show");
                    $('.radioBtn a').on('click', function () {
                        let sel = $(this).data('title'),
                            tog = $(this).data('toggle');
                        $(`#${tog}`).prop('value', sel);
                        $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
                        $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
                        setTotalSelecionado();
                    });
                }
            });
        }
        function getCuenta() {
            tablaCuentas = tblCuenta.DataTable({
                destroy: true,
                searching: true,
                paging: false,
                iDisplayLength: -1,
                info: false,
                scrollY: "380px",
                scrollCollapse: true,
                ordering: false,
                ajax: {
                    url: '/Administrativo/Poliza/getCuenta'
                },
                language: dtDicEsp,
                columns: [
                    { data: 'cta' },
                    { data: 'scta' },
                    { data: 'sscta' },
                    { data: 'digito' },
                    { data: 'descripcion' },
                    { data: null, defaultContent: "<button class='btn btn-success'><span class='glyphicon glyphicon-check'></span></button>" }
                ],
                initComplete: function (settings, json) {
                    $("#tblCuenta_filter").addClass("hidden");
                    tablaCuentas.on('click', 'button', function () {
                        var objMov = $(tablaMovPoliza.row(".selected").node());
                        let objCta = tablaCuentas.row($(this).parents('tr')).data();
                        objMov.find(".cuenta").val(objCta.cta);
                        objMov.find(".scta").val(objCta.scta);
                        objMov.find(".sscta").val(objCta.sscta);
                        txtCapCuenta.val(objCta.descripcion);
                        modalCuenta.modal("hide");
                    });
                    this.api().columns().every(function () {
                        var column = this;
                        if (column.index() < 3) {
                            var select = $('<select class="form-control"><option value="">N/A</option></select>')
                                .appendTo($(column.header()).empty())
                                .on('change', function () {
                                    var val = $.fn.dataTable.util.escapeRegex(
                                        $(this).val()
                                    );
                                    column
                                        .search(val ? '^' + val + '$' : '', true, false)
                                        .draw();
                                });
                            column.data().unique().sort().each(function (d, j) {
                                select.append('<option value="' + d + '">' + d + '</option>')
                            });
                        }
                    });
                }
            });
        }
        init();
    }
    $(document).ready(function () {
        Administrativo.Contabilidad.Poliza.Captura = new Captura();
    }).ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();