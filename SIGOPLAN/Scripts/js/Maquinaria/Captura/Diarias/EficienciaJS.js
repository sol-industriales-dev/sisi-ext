(function () {
    $.namespace('maquinaria.capturas.diarias');

    Eficiencia = function () {
        mensajes = { PROCESANDO: 'Procesando...' };
        txtId = $("#txtId");
        dtFecha = $("#dtFecha");
        cboTipoEquipo = $("#cboTipoEquipo");
        cboEquipo = $("#cboEquipo");
        cboGrupo = $("#cboGrupo");
        cboCC = $("#cboCC");
        txtTurno = $("#txtTurno");
        txtHrsInicial = $("#txtHrsInicial");
        txtHrsFinal = $("#txtHrsFinal");
        txtHrsTrabajada = $("#txtHrsTrabajada");
        txtFallaTrenRodaje = $("#txtFallaTrenRodaje");
        txtFallaElectrica = $("#txtFallaElectrica");
        txtFallaHidraulica = $("#txtFallaHidraulica");
        txtFallaOtros = $("#txtFallaOtros");
        txtFallaOperacion = $("#txtFallaOperacion");
        txtFaltaOperador = $("#txtFaltaOperador");
        txtTramoFMat = $("#txtTramoFMat");
        txtTramoFDat = $("#txtTramoFDat");
        txtTramoFAvan = $("#txtTramoFAvan");
        txtTramoIClie = $("#txtTramoIClie");
        txtHrsTotal = $("#txtHrsTotal");
        txtHrsTotalReparacion = $("#txtHrsTotalReparacion");
        txtParo = $("#txtParo");
        txtSemana = $("#txtSemana");
        txtHrsBase = $("#txtHrsBase");
        txtHrsDiferencia = $("#txtHrsDiferencia");
        txtEconomico = $("#txtEconomico");
        totalReparacion = $('.totalReparacion');
        btnGuardaEficiencia = $("#btnGuardaEficiencia");
        txtComentario = $("#txtComentario");
        divCaptura = $("#divCaptura");
        divTabla = $("#divTabla");
        dtFechaFiltro = $("#dtFechaFiltro");
        cboCCFiltro = $("#cboCCFiltro");
        btnNuevaEficiencia = $("#btnNuevaEficiencia");
        btnCancelarEficiencia = $("#btnCancelarEficiencia");
        btnBuscarEficiencia = $("#btnBuscarEficiencia");
        tblEficiencia = $("#tblEficiencia");
        btnReporteObra = $("#btnReporteObra");
        btnReporteGeneral = $("#btnReporteGeneral");
        ireport = $("#report");
        txtSemanaFiltro = $("#txtSemanaFiltro");
        dtFechaFinFiltro = $("#dtFechaFinFiltro");
        divBtnObra = $("#divBtnObra");
        divBtnGeneral = $("#divBtnGeneral");

        function init() {
            getDataPiker();
            getComboBox();
            cboTipoEquipo.change(FillCboGrupo);
            cboGrupo.change(FillCboModelo);
            cboEquipo.change(validarEquipo);
            cboCCFiltro.change(muestraBoton);
            totalReparacion.change();
            sumReparacion();
            btnGuardaEficiencia.click(GuardaEficiencia);
            btnNuevaEficiencia.click(MostrarCaptura);
            btnCancelarEficiencia.click(MostrarTabla);
            btnBuscarEficiencia.click(getTablaEficiencia);
            btnReporteObra.click(ReporteObra);
            btnReporteGeneral.click(ReporteObra);
        }

        function getDataPiker() {
            dtFecha.datepicker({
                showWeek: true,
                firstDay: 2,
                onSelect: function (value, date) {
                    txtSemana.val(getSemana(date));
                }
            });
            dtFechaFiltro.datepicker({
                showWeek: true,
                firstDay: 2
            });
            dtFechaFinFiltro.datepicker({
                showWeek: true,
                firstDay: 2
            });
            dtFecha.datepicker().datepicker("setDate", new Date());
            dtFechaFiltro.datepicker().datepicker("setDate", new Date());
            dtFechaFinFiltro.datepicker().datepicker("setDate", new Date());
        }

        function getSemana(date) {
            var week = $.datepicker.iso8601Week(
            new Date(date.selectedYear,
                    date.selectedMonth,
                    date.selectedDay));
            return week;
        }

        function getComboBox() {
            cboGrupo.attr('disabled', true);
            cboEquipo.attr('disabled', true);
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', null, false, null);
            cboCCFiltro.fillCombo('/Administrativo/ReportesRH/FillComboCC', null, false, null);
            cboTipoEquipo.fillCombo('/RepGastosMaquinaria/fillCboTipoMaquina', { estatus: true });
        }
        function FillCboGrupo() {
            if (cboTipoEquipo.val() != "") {
                cboGrupo.fillCombo('/RepGastosMaquinaria/fillCboGrupoMaquina', { idTipo: cboTipoEquipo.val() });
                cboGrupo.attr('disabled', false);
                cboEquipo.clearCombo();
                cboEquipo.attr('disabled', true);
                txtHrsInicial.clearCombo();
                txtHrsInicial.attr('disabled', true);
                txtHrsFinal.clearCombo();
                txtHrsFinal.attr('disabled', true);
                txtTurno.clearCombo();
                txtTurno.attr('disabled', true);
            }
            else {
                cboGrupo.clearCombo();
                cboGrupo.attr('disabled', true);
                cboEquipo.clearCombo();
                cboEquipo.attr('disabled', true);
                txtHrsInicial.clearCombo();
                txtHrsInicial.attr('disabled', true);
                txtHrsFinal.clearCombo();
                txtHrsFinal.attr('disabled', true);
                txtTurno.clearCombo();
                txtTurno.attr('disabled', true);
            }
        }
        function FillCboModelo() {
            if (cboGrupo.val() != "") {
                cboEquipo.fillCombo('/RepGastosMaquinaria/fillCboMaquinarias', { idGrupo: cboGrupo.val(), idTipo: cboTipoEquipo.val() });
                cboEquipo.attr('disabled', false);
                txtHrsInicial.clearCombo();
                txtHrsInicial.attr('disabled', true);
                txtHrsFinal.clearCombo();
                txtHrsFinal.attr('disabled', true);
                txtTurno.clearCombo();
                txtTurno.attr('disabled', true);
            }
            else {
                cboEquipo.clearCombo();
                cboEquipo.attr('disabled', true);
                txtHrsInicial.clearCombo();
                txtHrsInicial.attr('disabled', true);
                txtHrsFinal.clearCombo();
                txtHrsFinal.attr('disabled', true);
                txtTurno.clearCombo();
                txtTurno.attr('disabled', true);
            }
        }
        function validarEquipo() {
            txtHrsInicial.fillCombo('/Eficiencia/getcboHorometro', { Econ: $("#cboEquipo option:selected").text(), Fecha: dtFecha.val(), cc: cboCC.val() });
            txtHrsInicial.attr('disabled', false);
            txtHrsFinal.fillCombo('/Eficiencia/getcboHorometro', { Econ: $("#cboEquipo option:selected").text(), Fecha: dtFecha.val(), cc: cboCC.val() });
            txtHrsFinal.attr('disabled', false);
            txtTurno.fillCombo('/Eficiencia/getcboTurno', { Econ: $("#cboEquipo option:selected").text(), Fecha: dtFecha.val(), cc: cboCC.val() });
            txtTurno.attr('disabled', false);
        }

        function muestraBoton() {
            if (cboCCFiltro.val() == "") {
                divBtnObra.addClass("hidden");
                divBtnGeneral.removeClass("hidden");
            } else {
                divBtnObra.removeClass("hidden");
                divBtnGeneral.addClass("hidden");
            }
        }

        function sumHorasTrabajadas() {
            var diferencia;
            diferencia = parseFloat(txtHrsFinal.val()) - parseFloat(txtHrsInicial.val());
            txtHrsTrabajada.val(diferencia);
        }

        $('.horasTrabajadas').on('input', function () {
            sumHorasTrabajadas();
        });

        function sumReparacion() {
            var suma = 0;
            suma += parseFloat(txtFallaTrenRodaje.val());
            suma += parseFloat(txtFallaElectrica.val());
            suma += parseFloat(txtFallaHidraulica.val());
            suma += parseFloat(txtFallaOtros.val());
            suma += parseFloat(txtFallaOperacion.val());
            txtHrsTotalReparacion.val(suma);
        }
        $('.totalReparacion').on('input', function () {
            sumReparacion();
        });
        function sumParo() {
            var suma = 0;
            suma += parseFloat(txtTramoFMat.val());
            suma += parseFloat(txtTramoFDat.val());
            suma += parseFloat(txtTramoFAvan.val());
            suma += parseFloat(txtTramoIClie.val());
            txtParo.val(suma);
        }
        $('.paro').on('input', function () {
            sumParo();
        });

        function difHoras() {
            var diferencia;
            diferencia = parseFloat($(txtHrsTrabajada).val()) - parseFloat($(txtHrsBase).val());
            txtHrsDiferencia.val(diferencia);
        }
        $('.diferencia').on('input', function () {
            difHoras();
        });
        function sumTotal() {
            sumReparacion();
            sumParo();
            var suma = 0;
            suma += parseFloat(txtHrsTotalReparacion.val());
            suma += parseFloat(txtParo.val());
            suma += parseFloat(txtFaltaOperador.val());
            txtHrsTotal.val(suma);
        }
        $('.total').on('input', function () {
            sumTotal();
        });
        function GuardaEficiencia() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Eficiencia/GuardaEficiencia',
                type: "POST",
                datatype: "json",
                data: getObjEficiencia(),
                success: function (response) {

                    $.unblockUI();
                    ConfirmacionGeneral("Confirmación", "¡Registro guardado correctamente!");

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function getObjEficiencia() {
            var objEficiencia = {
                id: txtId.val(),
                Fecha: dtFecha.val(),
                IdEquipo: cboEquipo.val(),
                IdGrupo: cboGrupo.val(),
                IdObra: cboCC.val(),
                IdTipoEquipo: cboTipoEquipo.val(),
                Turno: txtTurno.val(),
                HrsInicial: txtHrsInicial.val(),
                HrsFinal: txtHrsFinal.val(),
                HrsTrabajada: txtHrsTrabajada.val(),
                FallaTrenRodaje: txtFallaTrenRodaje.val(),
                FallaElectrica: txtFallaElectrica.val(),
                FallaHidraulica: txtFallaHidraulica.val(),
                FallaOtros: txtFallaOtros.val(),
                FallaOperacion: txtFallaOperacion.val(),
                FaltaOperador: txtFaltaOperador.val(),
                TramoFMat: txtTramoFMat.val(),
                TramoFDat: txtTramoFDat.val(),
                TramoFAvan: txtTramoFAvan.val(),
                TramoIClie: txtTramoIClie.val(),
                HrsTotal: txtHrsTotal.val(),
                HrsTotalReparacion: txtHrsTotalReparacion.val(),
                Paro: txtParo.val(),
                Semana: txtSemana.val(),
                HrsBase: txtHrsBase.val(),
                HrsDiferencia: txtHrsDiferencia.val(),
                Comentarios: txtComentario.val(),
                Economico: ''
            };
            if ($("#cboEquipo option:selected").text() == "OTRO") {
                objEficiencia.Economico = txtEconomico.val();
            }
            else {
                objEficiencia.Economico = $("#cboEquipo option:selected").text();
            }
            return objEficiencia;
        }
        function MostrarCaptura() {
            divCaptura.removeAttr("hidden");
            divTabla.prop("hidden", "hidden");
        }
        function MostrarTabla() {
            divTabla.removeAttr("hidden");
            divCaptura.prop("hidden", "hidden");
        }
        function getTablaEficiencia() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Eficiencia/getTablaEficiencia",
                type: "POST",
                datatype: "json",
                data: { FechaInicioFiltro: dtFechaFiltro.val(), FechaUltimoFiltro: dtFechaFinFiltro.val(), cc: cboCCFiltro.val() },
                success: function (response) {
                    tblEficiencia.bootgrid("clear");
                    tblEficiencia.bootgrid("append", response);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function ReporteObra() {
            var btnValue = $(this).val();
            if (validacion(btnValue)) {
                $.blockUI({ message: mensajes.PROCESANDO });
                var idReporte = btnValue;
                var CC = cboCCFiltro.val();
                var fechaInicio = dtFechaFiltro.val();
                var fechaFin = dtFechaFinFiltro.val();
                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&CC=" + CC + "&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin;
                ireport.attr("src", path);
                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
            }
        }

        function validacion(btnValue) {
            var valida = true;
            switch (btnValue) {
                case 37:
                    if ((cboCCFiltro.val() == '' || cboCCFiltro.val() == null) && (txtSemanaFiltro.val() == '' || txtSemanaFiltro.val() == null))
                        valida = false;
                    break;
                case 38:
                    if (txtSemanaFiltro.val() == '' || txtSemanaFiltro.val() == null)
                        valida = false;
                    break;
                default:
                    break;
            }
            return valida;
        }
        init();
    }
    $(document).ready(function () {
        maquinaria.capturas.diarias = new Eficiencia();
    });
})();
function getWeek(date) {
    $.datepicker.iso8601Week(date);
};