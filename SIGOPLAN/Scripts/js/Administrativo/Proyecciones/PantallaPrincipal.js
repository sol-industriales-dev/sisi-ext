
(function () {

    $.namespace('Administrativo.Proyecciones.PantallaPrincipal');

    PantallaPrincipal = function () {

        mensajes = {
            NOMBRE: 'Proyecciones Financieras',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },

        tbModalDescEscenario = $("#tbModalDescEscenario"),
        tbModalMargenInicial = $("#tbModalMargenInicial"),
        tbModalMargenFinal = $("#tbModalMargenFinal"),

        btnGuardarNuevoEscenario = $("#btnGuardarNuevoEscenario"),
        cboEscenariosPrincipales = $("#cboEscenariosPrincipales"),
        chkEscenarioPadre = $("#chkEscenarioPadre"),
        btnAddEscenario = $("#btnAddEscenario"),
        modalNewEscenario = $("#modalNewEscenario"),
        ireport = $("#report"),
        btnMenuPrincipal = $("#btnMenuPrincipal"),
        cboEscenario = $("#cboEscenario"),
        tbDivisor = $("#tbDivisor"),
        tbMesesInicio = $("#tbMesesInicio"),
        cboPeriodo = $("#cboPeriodo"),

        idTituloContainer = $("#idTituloContainer"),
        btnHome = $("#btnHome"),
        btnCifrasPrincipales = $("#btnCifrasPrincipales"),
        btnEdoResultado = $("#btnEdoResultado"),
        btnFlujo = $("#btnFlujo"),
        btnBalance = $("#btnBalance"),
        btnObras = $("#btnObras"),
        btnMaquinaria = $("#btnMaquinaria"),
        btnGastosAdmon = $("#btnGastosAdmon"),
        btnActivoFijo = $("#btnActivoFijo"),
        btnPagosDiversos = $("#btnPagosDiversos"),
        btnCobroDiversos = $("#btnCobroDiversos"),
        btnSaldosIniciales = $("#btnSaldosIniciales"),

        btnPremisas = $("#btnPremisas");

        function init() {
            //     
            var d = new Date();
            var n = d.getMonth();

            tbMesesInicio.val(n);
            btnHome.click(loadVista);
            btnCifrasPrincipales.click(VerReporte);
            btnEdoResultado.click(VerReporte);
            btnFlujo.click(VerReporte);
            btnBalance.click(VerReporte);
            btnObras.click(loadVista);
            btnMaquinaria.click(loadVista);
            btnGastosAdmon.click(loadVista);
            btnActivoFijo.click(loadVista);
            btnPagosDiversos.click(loadVista);
            btnCobroDiversos.click(loadVista);
            btnSaldosIniciales.click(loadVista);
            btnPremisas.click(loadVista);
            cboPeriodo.val(_GCurrentYear);
            btnHome.click();
            btnAddEscenario.click(OpenModalEscenarios);
            cboEscenariosPrincipales.prop('disabled', true);
            chkEscenarioPadre.change(VerEscenarios);
            btnGuardarNuevoEscenario.click(guardarNuevoEscenario);
        }

        function guardarNuevoEscenario() {
            $.ajax({
                url: '/Proyecciones/guardarEscenario',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: GetData() }),
                success: function (response) {
                    fnLimpiarDataEscenario();
                    AlertaGeneral("Confirmación", "Fue Agregado un nuevo Escenario");
                },
                error: function (response) {
                    AlertaGeneral("Error", response.message);
                }
            });
        }

        function fnLimpiarDataEscenario() {
            cboEscenariosPrincipales.val('');
            tbModalDescEscenario.val('');
            tbModalMargenInicial.val('');
            tbModalMargenFinal.val('');

        }

        function GetData() {
            return {
                id: 0,
                PadreID: cboEscenariosPrincipales.val(),
                descripcion: tbModalDescEscenario.val(),
                pInicial: tbModalMargenInicial.val(),
                pFinal: tbModalMargenFinal.val(),
                estatus: 1
            }
        }

        function VerEscenarios() {
            if (chkEscenarioPadre.is(':checked')) {
                cboEscenariosPrincipales.prop('disabled', false);
                cboEscenariosPrincipales.fillCombo('/Proyecciones/fillCboEscenariosPadre', null, false);
            }
            else {
                cboEscenariosPrincipales.prop('disabled', true);

                cboEscenariosPrincipales.clearCombo();
                cboEscenariosPrincipales.val('');
            }
        }

        function OpenModalEscenarios() {
            fnLimpiarDataEscenario();
            $("#modalNewEscenario").modal('show');
        }

        function LoadDisabled() {
            tbDivisor.prop("disabled", true);
            tbMesesInicio.prop("disabled", true);
            cboPeriodo.prop("disabled", true);

            cboEscenario.prop("disabled", true);
            $("#tbEscenarioPorciento").prop('disabled', true);


        }

        function LoadCifrasPrincipales() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var d = new Date();
            idTituloContainer.css('font-size', '13px');
            idTituloContainer.text('PROYECCIONES DE ESCENARIOS FINANCIEROS ( EJERCICIO INICIAL  ' + (1900 + d.getYear()) + ' )');

            var liga = '/Proyecciones/CifrasPrincipales';
            if (liga != "") {
                $('#idVistaParcial').html('');
                $('#idVistaParcial').empty();
                $.unblockUI();
                //$('#idVistaParcial').load(liga, function () {
                //    $.unblockUI();
                //    tbDivisor.prop("disabled", false);
                //    tbMesesInicio.prop("disabled", false);
                //    cboPeriodo.prop("disabled", false);
                //    cboEscenario.prop("disabled", false);
                //    $("#tbEscenarioPorciento").prop('disabled', false);
                //});
            }
        }

        function loadVista() {
            //$.blockUI({ message: mensajes.PROCESANDO });
            var btnValue = $(this).val();
            var liga = getLiga($(this).val());
            if (liga != "") {
                $('#idVistaParcial').empty();
                $('#idVistaParcial').load(liga, function () {
                    $.unblockUI();
                });

            }
        }

        function RemoveEventHandle() {
            $("#tbDivisor").unbind();
            $("#tbMesesInicio").unbind();
            $("#cboEscenario").unbind();
            $("#cboPeriodo").unbind();
            $("#btnCargarInfo").unbind('click');
            $('.editRow').unbind();

        }

        function getLiga(liga) {
            LoadDisabled();
            RemoveEventHandle();
            idTituloContainer.css("font-size", "");
            var mes = GetPeriodoMeses();
            switch (liga) {
                case "":
                    var d = new Date();
                    idTituloContainer.text('PROYECCIONES DE ESCENARIOS FINANCIEROS ( EJERCICIO INICIAL  ' + (1900 + d.getYear()) + ' )');
                    idTituloContainer.css('font-size', '13px');
                    tbDivisor.prop("disabled", false);
                    tbMesesInicio.prop("disabled", false);
                    cboPeriodo.prop("disabled", false);
                    cboEscenario.prop("disabled", false);
                    $("#tbEscenarioPorcieKnto").prop('disabled', false);
                    return '/Proyecciones/CifrasPrincipales';
                case "5":
                    btnMenuPrincipal.trigger('click');
                    tbMesesInicio.prop("disabled", false);
                    cboPeriodo.prop("disabled", false);
                    idTituloContainer.text('Captura de Obra ' + mes[0]);
                    return '/Proyecciones/CapturaDeObras';
                case "6":
                    tbMesesInicio.prop("disabled", false);
                    cboPeriodo.prop("disabled", false);
                    idTituloContainer.text('CxC ' + mes[0]);
                    return '/Proyecciones/CxC';
                case "7":
                    tbMesesInicio.prop("disabled", false);
                    cboPeriodo.prop("disabled", false);
                    $("#tbEscenarioPorciento").prop('disabled', false);
                    idTituloContainer.text('DETALLE DE GASTOS DE ADMINISTRACION Y VENTAS ' + mes[0]);
                    return '/Proyecciones/GastosAdministacionyVentas';
                case "8":
                    tbMesesInicio.prop("disabled", false);
                    cboPeriodo.prop("disabled", false);
                    idTituloContainer.text('Proyección de adquisición de activo fijo ' + mes[0]);
                    return '/Proyecciones/ActivoFijo';
                case "9":
                    tbMesesInicio.prop("disabled", false);
                    cboPeriodo.prop("disabled", false);
                    idTituloContainer.text('Pagos Diversos ' + mes[0]);
                    return '/Proyecciones/PagosDiversos';
                case "10":
                    tbMesesInicio.prop("disabled", false);
                    cboPeriodo.prop("disabled", false);
                    idTituloContainer.text('Ingresos Diversos ' + mes[0]);
                    return '/Proyecciones/IngresosDiversos';
                case "11":
                    tbMesesInicio.prop("disabled", false);
                    cboPeriodo.prop("disabled", false);
                    idTituloContainer.text('Estado De Posición Financiera (Captura De Datos Balance Inicial) ' + mes[0]);
                    return '/Proyecciones/EstadoPosicionFinanciera';
                case "12":
                    tbMesesInicio.prop("disabled", false);
                    cboPeriodo.prop("disabled", false);
                    idTituloContainer.text('Premisas macro económicas ' + mes[0]);
                    return '/Proyecciones/Premisas';
                default:
                    return '';
            }
        }

        function GetPeriodoMeses(tipo) {
            var periodo = cboPeriodo.val();
            var MesInicio = tbMesesInicio.val();
            var months = ["ENE", "FEB", "MAR", "ABR", "MAY", "JUN",
                          "JUL", "AGO", "SEP", "OCT", "NOV", "DIC"];
            var tituloMeses = [];
            var ListoMonthsID = [];
            var count = 0;
            for (var i = MesInicio; i < 12; i++) {
                count++;
                //   $("#lblFecha" + count).text(months[i] + " " + periodo);
                tituloMeses.push(months[i] + " " + periodo);
                ListoMonthsID.push(i);
            }
            for (var i = 0 ; i < MesInicio; i++) {
                //  $("#lblFecha" + count).text(months[i] + " " + periodo);
                tituloMeses.push(months[i] + " " + (Number(periodo) + 1));
                ListoMonthsID.push(i);
            }
            if (tipo == 2) {
                return ListoMonthsID;
            }
            else {
                return tituloMeses;
            }
        }

        function redondear(valor) {
            var sinComas = removeCommas(valor);
            var redondeado = Math.round(sinComas);
            var conCommas = addCommas(redondeado.toFixed(2));
            return conCommas;
        }

        function removeCommas(str) {

            while (str.search(",") >= 0) {
                str = (str + "").replace(',', '');
            }
            return str;
        };

        function addCommas(nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }

        function VerReporte() {

            var btnValue = $(this).val();
            $.blockUI({ message: mensajes.PROCESANDO });
            var idReporte = btnValue;
            var Escenario = cboEscenario.val();
            var Divisor = tbDivisor.val();
            var meses = tbMesesInicio.val();
            var anio = cboPeriodo.val();
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&Escenario=" + Escenario + "&Divisor=" + Divisor + "&meses=" + meses + "&anio=" + anio;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        init();
    };

    $(document).ready(function () {

        Administrativo.Proyecciones.PantallaPrincipal = new PantallaPrincipal();
    });
})();

