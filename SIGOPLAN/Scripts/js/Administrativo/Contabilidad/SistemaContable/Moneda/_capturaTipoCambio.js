(() => {
    $.namespace('Administrativo._capturaTipoCambio');
    let MonedaPrincipal = 0, id = 0;
    const txtTcFecha = $('#txtTcFecha');
    const cboTcMoneda = $('#cboTcMoneda');
    const btnTcLimpiar = $('#btnTcLimpiar');
    const btnTcGuardar = $('#btnTcGuardar');
    const tblTcHistorial = $('#tblTcHistorial');
    const txtTcTipoCambio = $('#txtTcTipoCambio');
    const GetComboMonedas = originURL('/Administrativo/TipoCambio/GetComboMonedas');
    const GuardarTipoCambio = originURL('/Administrativo/TipoCambio/GuardarTipoCambio');
    const ValidaTipoCambioDelDia = originURL('/Administrativo/TipoCambio/ValidaTipoCambioDelDia');
    _capturaTipoCambio = function () {
        (() => {
            initForm();
            btnTcLimpiar.click(LimpiarFormulario);
            btnTcGuardar.click(setGuardarTipoCambio);
            txtTcTipoCambio.change(changeTipoCambio);
        })();
        //#region http
        function VerificaMonedaDia() {
            dtTcHistorial.clear().draw();
            axiosClean.get(ValidaTipoCambioDelDia)
                .then(response => {
                    let { success, tipoCambio, HistoricoTipoCambio } = response.data;
                    dtTcHistorial.rows.add(HistoricoTipoCambio).draw();
                    if (success) {
                        MonedaPrincipal = tipoCambio.Divisa.Moneda;
                        setFormulario(tipoCambio);
                        $("#mdlCapturaTipoCambio").modal("show");
                    }
                }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }
        function setGuardarTipoCambio() {
            let tipoCambio = getFormulario();
            if (!esTipoCambioValido(tipoCambio)) {
                return;
            }
            axios.post(GuardarTipoCambio, { tipoCambio: tipoCambio })
                .then(response => {
                    let { success, message } = response.data;
                    if (success) {
                        NotificacionGeneral("Aviso", `El tipo cambio fue guardado con éxito.`);
                        LimpiarFormulario();
                        VerificaMonedaDia();
                        $("#mdlCapturaTipoCambio").modal('hide');
                    } else {
                        AlertaGeneral('Aviso', message)
                    }
                }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }
        function initForm() {
            initDataTblTcHistorial();
            txtTcTipoCambio.val(0);
            txtTcFecha.datepicker({
                maxDate: new Date(),
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999);
                    }, 0);
                }
            }).datepicker("setDate", new Date());
            //axios.get(GetComboMonedas)
            //    .then(response => {
            //        let { success, items } = response.data;
            //        if (success) {
            //            cboTcMoneda.fillComboItems(items, undefined, "MEX");
            //            VerificaMonedaDia();
            //        }
            //    }).catch(o_O => AlertaGeneral(o_O.message));
        }
        //#endregion
        //#region formularios
        function changeTipoCambio() {
            let tipoCambio = unmaskNumero(txtTcTipoCambio.val());
            if (tipoCambio < 0) {
                tipoCambio *= -1;
            }
            txtTcTipoCambio.val(tipoCambio);
        }
        function setFormulario(tipoCambio) {
            id = tipoCambio.Id;
            cboTcMoneda.val(tipoCambio.Divisa.Moneda);
            txtTcFecha.val($.toDate(tipoCambio.Fecha));
            txtTcTipoCambio.val(tipoCambio.TipoCambio);
        }
        function LimpiarFormulario() {
            tblTcHistorial.find('tr.selected').removeClass('selected');
            setFormulario({
                Id: 0,
                Fecha: new Date(),
                TipoCambio: 0,
                Divisa: {
                    Moneda: MonedaPrincipal
                }
            });
        }
        function getFormulario() {
            let monedaSeleccionada = getMonedaSeleccionada();
            return {
                Id: id,
                Fecha: txtTcFecha.val().toDate(),
                TipoCambio: unmaskNumero(txtTcTipoCambio.val()),
                Moneda: monedaSeleccionada.Clave,
                Divisa: monedaSeleccionada
            };
        }
        function esTipoCambioValido(tipoCambio) {
            let mensaje = "";
            if (tipoCambio.TipoCambio <= 0) {
                mensaje += "El tipo de cambio no es correcto. ";
            }
            if (tipoCambio.Fecha > new Date) {
                mensaje += "Seleccione una fecha valida. ";
            }
            if (tipoCambio.Divisa.Clave <= 0) {
                mensaje += "Seleccione una moneda correcta. ";
            }
            let tieneError = mensaje.length > 0;
            if (tieneError) {
                AlertaGeneral("Aviso", mensaje);
            }
            return !tieneError;
        }
        function getMonedaSeleccionada() {
            return JSON.parse(cboTcMoneda.find(`:selected`).data().prefijo);
        }
        function initDataTblTcHistorial() {
            dtTcHistorial = tblTcHistorial.DataTable({
                destroy: true
                , language: dtDicEsp
                , ordering: false
                , columns: [
                    { data: 'Divisa.Moneda', title: 'Moneda', render: (data, type, row, meta) => `${row.Divisa.Denominacion} ${data}` },
                    { data: 'Fecha', title: 'Fecha', render: data => $.toDate(data) },
                    { data: 'TipoCambio', title: "Tipo Cambio", sClass: "text-right", render: data => maskNumero(data) },
                    { data: 'Empleado_modifica_nombre', visible: false, title: "Empleado Modifica" },
                    { data: 'FechaModifica', title: 'Modificación', render: data => $.toDate(data) },
                ]
                , initComplete: function (settings, json) {
                    tblTcHistorial.find('tbody').on('click', 'tr', function () {
                        let tr = $(this);
                        if (tr.hasClass('selected'))
                            tr.removeClass('selected');
                        else {
                            tblTcHistorial.find('tr.selected').removeClass('selected');
                            tr.addClass('selected');
                        }
                        let tipoCambio = dtTcHistorial.row(tr).data();
                        setFormulario(tipoCambio);
                    });
                }
            });
        }
        //#endregion
    }
    $(document).ready(() => {
        Administrativo._capturaTipoCambio = new _capturaTipoCambio();
    });
})();