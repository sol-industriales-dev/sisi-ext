
(function () {

    $.namespace('administracion.Proyecciones.CapCifrasPrincipales');

    CapCifrasPrincipales = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Captura de Cifras Principales',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        tbMesesInicio = $("#tbMesesInicio"),
        cboPeriodo = $("#cboPeriodo");

        var idData = 0;
        var escenariosEstatico = [];
        var escenariosaGuardar = "";
        var escenariosaBorrar = "";
        var _messageBoxYesNoResult = new $.Deferred();
        var flag1 = false,
        tbAnioAnteriorVentaProyectada = $("#tbAnioAnteriorVentaProyectada"),
        tbProyeccionMesVentaProyectada = $("#tbProyeccionMesVentaProyectada"),
        tbProyectadoAnioVentaProyectada = $("#tbProyectadoAnioVentaProyectada"),
        tbAnioAnteriorVentaReal = $("#tbAnioAnteriorVentaReal"),
        tbProyeccionesMesVentaReal = $("#tbProyeccionesMesVentaReal"),
        tbProyectadoAnioMesVentaReal = $("#tbProyectadoAnioMesVentaReal"),
        tbAnioAnteriorUtilidadPlaneada = $("#tbAnioAnteriorUtilidadPlaneada"),
        tbProyecionesMesUtilidadPlaneada = $("#tbProyecionesMesUtilidadPlaneada"),
        tbProyecionesAnioUtilidadPlaneada = $("#tbProyecionesAnioUtilidadPlaneada"),
        tbAnioAnteriorUtilidadReal = $("#tbAnioAnteriorUtilidadReal"),
        tbProyecionesMesUtilidadReal = $("#tbProyecionesMesUtilidadReal"),
        tbProyecionesAnioUtilidadReal = $("#tbProyecionesAnioUtilidadReal"),

        cboEscenario = $("#cboEscenario"),
        cboConfiguraciones = $("#cboConfiguraciones"),
        btnGuardar = $("#btnGuardar");

        function init() {            
            cboEscenario.fillCombo('/proyecciones/fillCboEscenarios', { tipo: 1 }, true);
            CargarConfiguraciones()
            convertToMultiselect(cboEscenario);
            cboEscenario.parent().find("button").css("color", '#555');
            tbAnioAnteriorVentaProyectada.DecimalFixPs(0);
            tbProyeccionMesVentaProyectada.DecimalFixPs(0);
            tbProyectadoAnioVentaProyectada.DecimalFixPs(0);
            tbAnioAnteriorVentaReal.DecimalFixPs(0);
            tbProyeccionesMesVentaReal.DecimalFixPs(0);
            tbProyectadoAnioMesVentaReal.DecimalFixPs(0);
            tbAnioAnteriorUtilidadPlaneada.DecimalFixPs(0);
            tbProyecionesMesUtilidadPlaneada.DecimalFixPs(0);
            tbProyecionesAnioUtilidadPlaneada.DecimalFixPs(0);
            tbAnioAnteriorUtilidadReal.DecimalFixPs(0);
            tbProyecionesMesUtilidadReal.DecimalFixPs(0);
            tbProyecionesAnioUtilidadReal.DecimalFixPs(0);

            tbMesesInicio.change(CargarConfiguraciones);
            tbMesesInicio.change(LoadInfo);
            tbMesesInicio.change(cargarBlock);            
            cboPeriodo.change(CargarConfiguraciones);
            cboPeriodo.change(LoadInfo);
            cboPeriodo.change(cargarBlock);
            cboConfiguraciones.change(LoadInfo);
            cboConfiguraciones.change(cargarBlock);
            btnGuardar.click(GuardarInformacion);
                       
            LoadInfo();

            //tbAnioAnteriorVentaProyectada.change(setComas);

            tbAnioAnteriorVentaProyectada.change(setColor);
            tbProyeccionMesVentaProyectada.change(setColor);
            tbProyectadoAnioVentaProyectada.change(setColor);
            tbAnioAnteriorVentaReal.change(setColor);
            tbProyeccionesMesVentaReal.change(setColor);
            tbProyectadoAnioMesVentaReal.change(setColor);
            tbAnioAnteriorUtilidadPlaneada.change(setColor);
            tbProyecionesMesUtilidadPlaneada.change(setColor);
            tbProyecionesAnioUtilidadPlaneada.change(setColor);
            tbAnioAnteriorUtilidadReal.change(setColor);
            tbProyecionesMesUtilidadReal.change(setColor);
            tbProyecionesAnioUtilidadReal.change(setColor);
            cboEscenario.change(setColorEscenario);     
        }

        function cargarBlock() {
            if (cboConfiguraciones.val() != null && cboConfiguraciones.val() != '0') {
                if (flag1 == false) {                    
                    $('#rowDatos').collapse("show")
                    btnGuardar.css("visibility", "visible");
                    flag1 = true;
                }                
            }
            else {
                if (flag1 == true) {                    
                    $('#rowDatos').collapse("hide")
                    btnGuardar.css("visibility", "collapse");
                    flag1 = false;
                }
            }
        }

        function CargarConfiguraciones() {
            cboConfiguraciones.fillCombo('/proyecciones/fillCboEscenariosConfiguraciones', { mes: tbMesesInicio.val(), anio: cboPeriodo.val() }, true);            
            escenariosEstatico = [];
            var aux = "";
            for (var i = 0; i < $("#cboConfiguraciones option").length - 2; i++)
            {
                aux += $("#cboConfiguraciones option").eq(i + 2).val() + ",";
            }
            escenariosEstatico = aux.split(",");
            $(cboConfiguraciones).html($(cboConfiguraciones).children('option').sort(function (x, y) {
                return $(x).text().toUpperCase() < $(y).text().toUpperCase() ? -1 : 1;
            }));
            cboConfiguraciones.val('0');
            $("#cboConfiguraciones option[value='Nuevo']").detach().insertBefore($("#cboConfiguraciones option:first"));
            $("#cboConfiguraciones option[value='0']").detach().insertBefore($("#cboConfiguraciones option:first"));
        }

        function setColor() { $(this).css("background-color", '#fdfd96'); }

        function setColorEscenario() {
            $(this).parent().find("button").css("background-color", '#fdfd96');
            $(this).parent().find("ul").css("background-color", '#fdfd96');
        }

        function redondear(valor) {
            valor = $(this).val();
            var sinComas = removeCommas(valor);
            var redondeado = Math.round(sinComas);
            var conCommas = addCommas(redondeado.toFixed(2));
            return conCommas;
        }

        function LoadInfo() {
            limpiar();
            $.blockUI({ message: mensajes.PROCESANDO });            
            $.ajax({
                url: '/proyecciones/LoadInfoCifrasPples',
                type: 'POST',
                dataType: 'json',
                data: { mes: tbMesesInicio.val(), anio: cboPeriodo.val(), escenarios: cboConfiguraciones.val() },
                success: function (response) {                    
                    $.unblockUI();
                    var DataResult = response.DataResult;
                    if (DataResult != null) {
                        idData = DataResult.id;
                        tbAnioAnteriorVentaProyectada.setVal(DataResult.VentaProyectadaAnioAnterior);
                        tbProyeccionMesVentaProyectada.setVal(DataResult.VentaProyectadaMesActual);
                        tbProyectadoAnioVentaProyectada.setVal(DataResult.VentaProyectadaAlAnio);
                        tbAnioAnteriorVentaReal.setVal(DataResult.VentaRealAnioAnterior);
                        tbProyeccionesMesVentaReal.setVal(DataResult.VentaRealMesActual);
                        tbProyectadoAnioMesVentaReal.setVal(DataResult.VentaRealProyectdaAlAnio);
                        tbAnioAnteriorUtilidadPlaneada.setVal(DataResult.UtilidadPlaneadaAnioAnterior);
                        tbProyecionesMesUtilidadPlaneada.setVal(DataResult.UtilidadPlaneadaMesActual);
                        tbProyecionesAnioUtilidadPlaneada.setVal(DataResult.UtilidadPlaneadaAnioActual);
                        tbAnioAnteriorUtilidadReal.setVal(DataResult.UtilidadRealAnioAnterior);
                        tbProyecionesMesUtilidadReal.setVal(DataResult.UtilidadRealMesActual);
                        tbProyecionesAnioUtilidadReal.setVal(DataResult.UtilidadRealAnioActual);
                        if (DataResult.escenarios != null) { cboEscenario.val(DataResult.escenarios.split(',')); }
                        else { cboEscenario.val(''); }
                        cboEscenario.multiselect("refresh");                                                
                        AlterarColor('#cffcd6');
                    }
                    else {
                        AlterarColor('#fff');
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GetInfoAguardar() {
            return {
                id: idData,
                MesInicio: tbMesesInicio.val(),
                ejercicioAnio: cboPeriodo.val(),

                VentaProyectadaAnioAnterior: tbAnioAnteriorVentaProyectada.getVal(0),
                VentaProyectadaMesActual: tbProyeccionMesVentaProyectada.getVal(0),
                VentaProyectadaAlAnio: tbProyectadoAnioVentaProyectada.getVal(0),

                VentaRealAnioAnterior: tbAnioAnteriorVentaReal.getVal(0),
                VentaRealMesActual: tbProyeccionesMesVentaReal.getVal(0),
                VentaRealProyectdaAlAnio: tbProyectadoAnioMesVentaReal.getVal(0),

                UtilidadPlaneadaAnioAnterior: tbAnioAnteriorUtilidadPlaneada.getVal(0),
                UtilidadPlaneadaMesActual: tbProyecionesMesUtilidadPlaneada.getVal(0),
                UtilidadPlaneadaAnioActual: tbProyecionesAnioUtilidadPlaneada.getVal(0),
                UtilidadRealAnioAnterior: tbAnioAnteriorUtilidadReal.getVal(0),
                UtilidadRealMesActual: tbProyecionesMesUtilidadReal.getVal(0),
                UtilidadRealAnioActual: tbProyecionesAnioUtilidadReal.getVal(0),
                estatus: 0,
                escenarios: escenariosaGuardar
            }
        }

        function limpiar() {
            idData = 0;
            tbAnioAnteriorVentaProyectada.setVal(0);
            tbProyeccionMesVentaProyectada.setVal(0);
            tbProyectadoAnioVentaProyectada.setVal(0);
            tbAnioAnteriorVentaReal.setVal(0);
            tbProyeccionesMesVentaReal.setVal(0);
            tbProyectadoAnioMesVentaReal.setVal(0);
            tbAnioAnteriorUtilidadPlaneada.setVal(0);
            tbProyecionesMesUtilidadPlaneada.setVal(0);
            tbProyecionesAnioUtilidadPlaneada.setVal(0);
            tbAnioAnteriorUtilidadReal.setVal(0);
            tbProyecionesMesUtilidadReal.setVal(0);
            tbProyecionesAnioUtilidadReal.setVal(0);
            cboEscenario.val(''); 
            cboEscenario.multiselect("refresh");
        }

        function GuardarInformacion() {
            if (cboEscenario.val() != "") {
                if (validarCampos()) {
                    escenariosaGuardar = "";
                    escenariosaBorrar = "";
                    var aux = [];

                    aux = cboEscenario.val().toString().split(",");
                    for (var i = 0; i < aux.length; i++) {
                        escenariosaGuardar += aux[i] + ',';
                        if (escenariosEstatico.indexOf(aux[i]) > -1) { escenariosaBorrar += aux[i] + ','; }
                    }
                    escenariosaGuardar = escenariosaGuardar.substring(0, escenariosaGuardar.length - 1);

                    if (escenariosaGuardar != "") {
                        if ($("#cboConfiguraciones option[value='" + escenariosaGuardar + "']").length > 0) {
                            ConfirmacionGeneral2("Confirmar", "Ya existe un registro para [" + $("#cboConfiguraciones option[value='" + escenariosaGuardar + "']").text() + "], ¿Desea reemplazarlo?");
                        }
                        else {
                            escenariosaBorrar = escenariosaBorrar.substring(0, escenariosaBorrar.length - 1)
                            BorrarEscenarios(tbMesesInicio.val(), cboPeriodo.val(), escenariosaBorrar);
                            idData = 0;
                            GuardarCaptura();
                        }
                    }
                }
                else
                {
                    AlertaGeneral("Alerta", "Uno o varios campos son inválidos");
                }               
            }
            else
            {
                AlertaGeneral("Alerta", "Seleccione al menos un Escenario");
                cboEscenario.parent().find("button").css("background-color", "#ffbaba");
            }
        }

        function GuardarCaptura()
        {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/GuardarCapCifrasPrincipales',
                type: 'POST',
                dataType: 'json',
                data: { obj: GetInfoAguardar() },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        AlertaGeneral("Confirmacion", "El registro ha sido ingresado correctamente");
                        AlterarColor('#cffcd6');
                        CargarConfiguraciones();
                        cboConfiguraciones.val(escenariosaGuardar);
                    }
                    else {
                        AlertaGeneral("Alerta", "Ocurrió un error en el guardado de la información");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function AlterarColor(color) {
            tbAnioAnteriorVentaProyectada.css("background-color", color);
            tbProyeccionMesVentaProyectada.css("background-color", color);
            tbProyectadoAnioVentaProyectada.css("background-color", color);
            tbAnioAnteriorVentaReal.css("background-color", color);
            tbProyeccionesMesVentaReal.css("background-color", color);
            tbProyectadoAnioMesVentaReal.css("background-color", color);
            tbAnioAnteriorUtilidadPlaneada.css("background-color", color);
            tbProyecionesMesUtilidadPlaneada.css("background-color", color);
            tbProyecionesAnioUtilidadPlaneada.css("background-color", color);
            tbAnioAnteriorUtilidadReal.css("background-color", color);
            tbProyecionesMesUtilidadReal.css("background-color", color);
            tbProyecionesAnioUtilidadReal.css("background-color", color);
            cboEscenario.parent().find("button").css("background-color", color);
            cboEscenario.parent().find("ul").css("background-color", color);
        }

        function BorrarEscenarios(mes, anio, escenarios)
        {
            $.ajax({
                url: '/proyecciones/BorrarEscenariosCapCifrasPrincipales',
                type: 'POST',
                dataType: 'json',
                data: { mes: mes, anio: anio, escenarios: escenarios },
                success: function (response) {
                    if (response.success) { }
                    else { AlertaGeneral("Alerta", "Ocurrió un error en la eliminación"); }
                },
                error: function (response) { AlertaGeneral("Alerta", response.message); }
            });
        }

        function ConfirmacionGeneral2(titulo, mensaje) {
            if (mensaje == null) {
                mensaje = "Error en el resultado de la peticion favor de intentar de nuevo";
            }
            $("#dialogalertaGeneral").removeClass('hide');
            $("#txtComentarioAlerta").html(mensaje);
            var opt = {
                title: titulo,
                autoOpen: false,
                draggable: false,
                resizable: false,
                modal: true,
                maxWidth: 600,
                minWidth: 400,
                position: {
                    my: "center",
                    at: "center",
                    within: $(".RenderBody")
                },
                buttons: [
                {
                    text: "Aceptar",
                    id: "btnok",
                    click: function () {
                        $("#dialogalertaGeneral").addClass('hide');
                        $(this).dialog("close");
                        ActualizarCapCifrasPrincipales();
                    }
                },
                {
                    text: "Cancelar",
                    id: "btncancel",
                    click: function () {
                        $("#dialogalertaGeneral").addClass('hide');
                        $(this).dialog("close");
                    }
                }
                ]

            };
            var theDialog = $("#dialogalertaGeneral").dialog(opt);
            theDialog.dialog("open");
        }



        function ActualizarCapCifrasPrincipales() {
            $.ajax({
                url: '/proyecciones/LoadInfoCifrasPples',
                type: 'POST',
                dataType: 'json',
                data: { mes: tbMesesInicio.val(), anio: cboPeriodo.val(), escenarios: escenariosaGuardar },
                success: function (response) {
                    var DataResult = response.DataResult;
                    if (DataResult != null) {
                        idData = DataResult.id;
                        GuardarCaptura();
                    }
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function validarCampos()
        {
            var aux = true;
            if (parseFloat(tbAnioAnteriorVentaProyectada.getVal(0)) < 0) { tbAnioAnteriorVentaProyectada.css("background-color", "#ffb3b3"); aux = false;}
            if (parseFloat(tbProyeccionMesVentaProyectada.getVal(0)) < 0) { tbProyeccionMesVentaProyectada.css("background-color", "#ffb3b3");  aux = false;}
            if (parseFloat(tbProyectadoAnioVentaProyectada.getVal(0)) < 0) { tbProyectadoAnioVentaProyectada.css("background-color", "#ffb3b3");  aux = false;}
            if (parseFloat(tbAnioAnteriorVentaReal.getVal(0)) < 0) { tbAnioAnteriorVentaReal.css("background-color", "#ffb3b3"); aux = false;}
            if (parseFloat(tbProyeccionesMesVentaReal.getVal(0)) < 0) { tbProyeccionesMesVentaReal.css("background-color", "#ffb3b3");  aux = false;}
            if (parseFloat(tbProyectadoAnioMesVentaReal.getVal(0)) < 0) { tbProyectadoAnioMesVentaReal.css("background-color", "#ffb3b3"); aux = false; }
            if (parseFloat(tbAnioAnteriorUtilidadPlaneada.getVal(0)) < 0) { tbAnioAnteriorUtilidadPlaneada.css("background-color", "#ffb3b3"); aux = false; }
            if (parseFloat(tbProyecionesMesUtilidadPlaneada.getVal(0)) < 0) { tbProyecionesMesUtilidadPlaneada.css("background-color", "#ffb3b3"); aux = false; }
            if (parseFloat(tbProyecionesAnioUtilidadPlaneada.getVal(0)) < 0) { tbProyecionesAnioUtilidadPlaneada.css("background-color", "#ffb3b3"); aux = false; }
            if (parseFloat(tbAnioAnteriorUtilidadReal.getVal(0)) < 0) { tbAnioAnteriorUtilidadReal.css("background-color", "#ffb3b3");aux = false; }
            if (parseFloat(tbProyecionesMesUtilidadReal.getVal(0)) < 0) { tbProyecionesMesUtilidadReal.css("background-color", "#ffb3b3");aux = false; }
            if (parseFloat(tbProyecionesAnioUtilidadReal.getVal(0)) < 0) { tbProyecionesAnioUtilidadReal.css("background-color", "#ffb3b3");aux = false; }
            return aux;
        }

        init();
    };
    $(document).ready(function () {
        administracion.Proyecciones.CapCifrasPrincipales = new CapCifrasPrincipales();
    });
})();

