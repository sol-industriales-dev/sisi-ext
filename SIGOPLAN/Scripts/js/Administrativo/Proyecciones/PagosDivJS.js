
(function () {

    $.namespace('administrativo.proyecciones.pagosdiv');

    pagosdiv = function () {
        btnPagosDivGuardar = $("#btnPagosDivGuardar"),
        inputCalculos = $(".calculo"),
        btnCargarInfo = $("#btnCargarInfo"),
        calculoPs = $(".calculo"),
        ModalUploadFile = $("#ModalUploadFile"),
        btnUploadInfo = $("#btnUploadInfo"),
        divPagosDiversosPrincipal = $(".divPagosDiversosPrincipal"),
        CustomDecimalEvent = $(".CustomDecimalEvent");

        function init() {
            btnUploadInfo.click(OpenLoadFile);
            btnUploadInfo.hide();
            setMeses(GetPeriodoMeses($("#cboPeriodo"), $("#tbMesesInicio")));
            btnCargarInfo.click(function () {
                var mes = GetPeriodoMeses($("#cboPeriodo"), $("#tbMesesInicio"));
                idTituloContainer.text('Pagos Diversos ' + mes[0]);
                clickBuscar();
                setMeses(GetPeriodoMeses($("#cboPeriodo"), $("#tbMesesInicio")));
            });
            $.each(calculoPs, function (i, e) {
                if ($(this).hasClass("esPr")) {
                    $(e).DecimalFixPr(2);
                }
                else if ($(this).hasClass("2dec")) {
                    $(e).DecimalFixNS(2);
                }
                else {
                    $(e).DecimalFixNS(0);
                }
            });

            clickBuscar();
            btnPagosDivGuardar.click(guardar);
            //calculoPs.change(calculos);
            divPagosDiversosPrincipal.on({
                change: calculos
            }, ".CustomDecimalEvent");
            //CustomDecimalEvent.change(calculos);
        }

        function OpenLoadFile() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Proyecciones/setTipoDocumento',
                type: 'POST',
                dataType: 'json',
                data: { id: 2, mes: tbMesesInicio.val(), anio: cboPeriodo.val(), idGlobal: 0 },
                success: function (response) {
                    ModalUploadFile.modal('show');
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function clickBuscar() {
            var objFiltro = getFGB();

            $.ajax({
                url: '/Proyecciones/GetFillTablePagosDiv',
                type: 'POST',
                dataType: 'json',
                data: { objFiltro: objFiltro },
                success: function (response) {
                    if (response.success === true) {
                        $(".rowDesgloseData").remove();
                        btnPagosDivGuardar.show();
                        var obj = response.obj;
                        setPlainObject(obj);
                    } else {
                        $(".rowDesgloseData").remove();
                        btnPagosDivGuardar.hide();
                        $.each(calculoPs, function (i, e) {
                            if ($(this).hasClass("esPr")) {
                                $(e).setVal(0);
                            }
                            else {
                                $(e).setVal(0);
                            }
                        });
                        if (response.loadFile === true) {

                            btnUploadInfo.click();
                        }

                    }
                    $.unblockUI();
                },
                error: function (response) {
                    $(".rowDesgloseData").remove();
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function guardar() {
            beforeSaveOrUpdate();
        }

        function beforeSaveOrUpdate() {

            if (valid()) {
                calculos();
                saveOrUpdate(getPlainObject());
            }
        }

        function getPlainObject(data) {
            var obj = {};
            var ln1 = {};
            ln1.Mes1 = $(".ln1Mes1").getVal(2);
            ln1.Mes2 = $(".ln1Mes2").getVal(2);
            ln1.Mes3 = $(".ln1Mes3").getVal(2);
            ln1.Mes4 = $(".ln1Mes4").getVal(2);
            ln1.Mes5 = $(".ln1Mes5").getVal(2);
            ln1.Mes6 = $(".ln1Mes6").getVal(2);
            ln1.Mes7 = $(".ln1Mes7").getVal(2);
            ln1.Mes8 = $(".ln1Mes8").getVal(2);
            ln1.Mes9 = $(".ln1Mes9").getVal(2);
            ln1.Mes10 = $(".ln1Mes10").getVal(2);
            ln1.Mes11 = $(".ln1Mes11").getVal(2);
            ln1.Mes12 = $(".ln1Mes12").getVal(2);
            ln1.MesT = $(".ln1MesT").getVal(2);

            var ln2 = {};
            ln2.Mes1 = $(".ln2Mes1").getVal(2);
            ln2.Mes2 = $(".ln2Mes2").getVal(2);
            ln2.Mes3 = $(".ln2Mes3").getVal(2);
            ln2.Mes4 = $(".ln2Mes4").getVal(2);
            ln2.Mes5 = $(".ln2Mes5").getVal(2);
            ln2.Mes6 = $(".ln2Mes6").getVal(2);
            ln2.Mes7 = $(".ln2Mes7").getVal(2);
            ln2.Mes8 = $(".ln2Mes8").getVal(2);
            ln2.Mes9 = $(".ln2Mes9").getVal(2);
            ln2.Mes10 = $(".ln2Mes10").getVal(2);
            ln2.Mes11 = $(".ln2Mes11").getVal(2);
            ln2.Mes12 = $(".ln2Mes12").getVal(2);
            ln2.MesT = $(".ln2MesT").getVal(2);

            var ln3 = {};
            ln3.Mes1 = $(".ln3Mes1").getVal(2);
            ln3.Mes2 = $(".ln3Mes2").getVal(2);
            ln3.Mes3 = $(".ln3Mes3").getVal(2);
            ln3.Mes4 = $(".ln3Mes4").getVal(2);
            ln3.Mes5 = $(".ln3Mes5").getVal(2);
            ln3.Mes6 = $(".ln3Mes6").getVal(2);
            ln3.Mes7 = $(".ln3Mes7").getVal(2);
            ln3.Mes8 = $(".ln3Mes8").getVal(2);
            ln3.Mes9 = $(".ln3Mes9").getVal(2);
            ln3.Mes10 = $(".ln3Mes10").getVal(2);
            ln3.Mes11 = $(".ln3Mes11").getVal(2);
            ln3.Mes12 = $(".ln3Mes12").getVal(2);
            ln3.MesT = $(".ln3MesT").getVal(2);

            var ln4 = {};
            ln4.Mes1 = $(".ln4Mes1").getVal(2);
            ln4.Mes2 = $(".ln4Mes2").getVal(2);
            ln4.Mes3 = $(".ln4Mes3").getVal(2);
            ln4.Mes4 = $(".ln4Mes4").getVal(2);
            ln4.Mes5 = $(".ln4Mes5").getVal(2);
            ln4.Mes6 = $(".ln4Mes6").getVal(2);
            ln4.Mes7 = $(".ln4Mes7").getVal(2);
            ln4.Mes8 = $(".ln4Mes8").getVal(2);
            ln4.Mes9 = $(".ln4Mes9").getVal(2);
            ln4.Mes10 = $(".ln4Mes10").getVal(2);
            ln4.Mes11 = $(".ln4Mes11").getVal(2);
            ln4.Mes12 = $(".ln4Mes12").getVal(2);
            ln4.MesT = $(".ln4MesT").getVal(2);

            var ln5 = {};
            ln5.Mes1 = $(".ln5Mes1").getVal(2);
            ln5.Mes2 = $(".ln5Mes2").getVal(2);
            ln5.Mes3 = $(".ln5Mes3").getVal(2);
            ln5.Mes4 = $(".ln5Mes4").getVal(2);
            ln5.Mes5 = $(".ln5Mes5").getVal(2);
            ln5.Mes6 = $(".ln5Mes6").getVal(2);
            ln5.Mes7 = $(".ln5Mes7").getVal(2);
            ln5.Mes8 = $(".ln5Mes8").getVal(2);
            ln5.Mes9 = $(".ln5Mes9").getVal(2);
            ln5.Mes10 = $(".ln5Mes10").getVal(2);
            ln5.Mes11 = $(".ln5Mes11").getVal(2);
            ln5.Mes12 = $(".ln5Mes12").getVal(2);
            ln5.MesT = $(".ln5MesT").getVal(2);

            var ln6 = {};
            ln6.Mes1 = $(".ln6Mes1").getVal(2);
            ln6.Mes2 = $(".ln6Mes2").getVal(2);
            ln6.Mes3 = $(".ln6Mes3").getVal(2);
            ln6.Mes4 = $(".ln6Mes4").getVal(2);
            ln6.Mes5 = $(".ln6Mes5").getVal(2);
            ln6.Mes6 = $(".ln6Mes6").getVal(2);
            ln6.Mes7 = $(".ln6Mes7").getVal(2);
            ln6.Mes8 = $(".ln6Mes8").getVal(2);
            ln6.Mes9 = $(".ln6Mes9").getVal(2);
            ln6.Mes10 = $(".ln6Mes10").getVal(2);
            ln6.Mes11 = $(".ln6Mes11").getVal(2);
            ln6.Mes12 = $(".ln6Mes12").getVal(2);
            ln6.MesT = $(".ln6MesT").getVal(2);

            var ln7 = {};
            ln7.Mes1 = $(".ln7Mes1").getVal(2);
            ln7.Mes2 = $(".ln7Mes2").getVal(2);
            ln7.Mes3 = $(".ln7Mes3").getVal(2);
            ln7.Mes4 = $(".ln7Mes4").getVal(2);
            ln7.Mes5 = $(".ln7Mes5").getVal(2);
            ln7.Mes6 = $(".ln7Mes6").getVal(2);
            ln7.Mes7 = $(".ln7Mes7").getVal(2);
            ln7.Mes8 = $(".ln7Mes8").getVal(2);
            ln7.Mes9 = $(".ln7Mes9").getVal(2);
            ln7.Mes10 = $(".ln7Mes10").getVal(2);
            ln7.Mes11 = $(".ln7Mes11").getVal(2);
            ln7.Mes12 = $(".ln7Mes12").getVal(2);
            ln7.MesT = 0;

            var ln8 = {};
            ln8.Mes1 = $(".ln8Mes1").getVal(2);
            ln8.Mes2 = $(".ln8Mes2").getVal(2);
            ln8.Mes3 = $(".ln8Mes3").getVal(2);
            ln8.Mes4 = $(".ln8Mes4").getVal(2);
            ln8.Mes5 = $(".ln8Mes5").getVal(2);
            ln8.Mes6 = $(".ln8Mes6").getVal(2);
            ln8.Mes7 = $(".ln8Mes7").getVal(2);
            ln8.Mes8 = $(".ln8Mes8").getVal(2);
            ln8.Mes9 = $(".ln8Mes9").getVal(2);
            ln8.Mes10 = $(".ln8Mes10").getVal(2);
            ln8.Mes11 = $(".ln8Mes11").getVal(2);
            ln8.Mes12 = $(".ln8Mes12").getVal(2);
            ln8.MesT = $(".ln8MesT").getVal(2);

            var ln9 = {};
            ln9.Mes1 = $(".ln9Mes1").getVal(2);
            ln9.Mes2 = $(".ln9Mes2").getVal(2);
            ln9.Mes3 = $(".ln9Mes3").getVal(2);
            ln9.Mes4 = $(".ln9Mes4").getVal(2);
            ln9.Mes5 = $(".ln9Mes5").getVal(2);
            ln9.Mes6 = $(".ln9Mes6").getVal(2);
            ln9.Mes7 = $(".ln9Mes7").getVal(2);
            ln9.Mes8 = $(".ln9Mes8").getVal(2);
            ln9.Mes9 = $(".ln9Mes9").getVal(2);
            ln9.Mes10 = $(".ln9Mes10").getVal(2);
            ln9.Mes11 = $(".ln9Mes11").getVal(2);
            ln9.Mes12 = $(".ln9Mes12").getVal(2);
            ln9.MesT = $(".ln9MesT").getVal(2);

            var ln10 = {};
            ln10.Mes1 = $(".ln10Mes1").getVal(2);
            ln10.Mes2 = $(".ln10Mes2").getVal(2);
            ln10.Mes3 = $(".ln10Mes3").getVal(2);
            ln10.Mes4 = $(".ln10Mes4").getVal(2);
            ln10.Mes5 = $(".ln10Mes5").getVal(2);
            ln10.Mes6 = $(".ln10Mes6").getVal(2);
            ln10.Mes7 = $(".ln10Mes7").getVal(2);
            ln10.Mes8 = $(".ln10Mes8").getVal(2);
            ln10.Mes9 = $(".ln10Mes9").getVal(2);
            ln10.Mes10 = $(".ln10Mes10").getVal(2);
            ln10.Mes11 = $(".ln10Mes11").getVal(2);
            ln10.Mes12 = $(".ln10Mes12").getVal(2);
            ln10.MesT = $(".ln10MesT").getVal(2);

            var ln11 = {};
            ln11.Mes1 = $(".ln11Mes1").getVal(2);
            ln11.Mes2 = $(".ln11Mes2").getVal(2);
            ln11.Mes3 = $(".ln11Mes3").getVal(2);
            ln11.Mes4 = $(".ln11Mes4").getVal(2);
            ln11.Mes5 = $(".ln11Mes5").getVal(2);
            ln11.Mes6 = $(".ln11Mes6").getVal(2);
            ln11.Mes7 = $(".ln11Mes7").getVal(2);
            ln11.Mes8 = $(".ln11Mes8").getVal(2);
            ln11.Mes9 = $(".ln11Mes9").getVal(2);
            ln11.Mes10 = $(".ln11Mes10").getVal(2);
            ln11.Mes11 = $(".ln11Mes11").getVal(2);
            ln11.Mes12 = $(".ln11Mes12").getVal(2);
            ln11.MesT = $(".ln11MesT").getVal(2);

            var ln12 = {};
            ln12.Mes1 = $(".ln12Mes1").getVal(2);
            ln12.Mes2 = $(".ln12Mes2").getVal(2);
            ln12.Mes3 = $(".ln12Mes3").getVal(2);
            ln12.Mes4 = $(".ln12Mes4").getVal(2);
            ln12.Mes5 = $(".ln12Mes5").getVal(2);
            ln12.Mes6 = $(".ln12Mes6").getVal(2);
            ln12.Mes7 = $(".ln12Mes7").getVal(2);
            ln12.Mes8 = $(".ln12Mes8").getVal(2);
            ln12.Mes9 = $(".ln12Mes9").getVal(2);
            ln12.Mes10 = $(".ln12Mes10").getVal(2);
            ln12.Mes11 = $(".ln12Mes11").getVal(2);
            ln12.Mes12 = $(".ln12Mes12").getVal(2);
            ln12.MesT = $(".ln12MesT").getVal(2);

            var ln13 = {};
            ln13.Mes1 = $(".ln13Mes1").getVal(2);
            ln13.Mes2 = $(".ln13Mes2").getVal(2);
            ln13.Mes3 = $(".ln13Mes3").getVal(2);
            ln13.Mes4 = $(".ln13Mes4").getVal(2);
            ln13.Mes5 = $(".ln13Mes5").getVal(2);
            ln13.Mes6 = $(".ln13Mes6").getVal(2);
            ln13.Mes7 = $(".ln13Mes7").getVal(2);
            ln13.Mes8 = $(".ln13Mes8").getVal(2);
            ln13.Mes9 = $(".ln13Mes9").getVal(2);
            ln13.Mes10 = $(".ln13Mes10").getVal(2);
            ln13.Mes11 = $(".ln13Mes11").getVal(2);
            ln13.Mes12 = $(".ln13Mes12").getVal(2);
            ln13.MesT = $(".ln13MesT").getVal(2);

            var ln14 = {};
            ln14.Mes1 = $(".ln14Mes1").getVal(2);
            ln14.Mes2 = $(".ln14Mes2").getVal(2);
            ln14.Mes3 = $(".ln14Mes3").getVal(2);
            ln14.Mes4 = $(".ln14Mes4").getVal(2);
            ln14.Mes5 = $(".ln14Mes5").getVal(2);
            ln14.Mes6 = $(".ln14Mes6").getVal(2);
            ln14.Mes7 = $(".ln14Mes7").getVal(2);
            ln14.Mes8 = $(".ln14Mes8").getVal(2);
            ln14.Mes9 = $(".ln14Mes9").getVal(2);
            ln14.Mes10 = $(".ln14Mes10").getVal(2);
            ln14.Mes11 = $(".ln14Mes11").getVal(2);
            ln14.Mes12 = $(".ln14Mes12").getVal(2);
            ln14.MesT = $(".ln14MesT").getVal(2);

            var ln15 = {};
            ln15.Mes1 = $(".ln15Mes1").getVal(2);
            ln15.Mes2 = $(".ln15Mes2").getVal(2);
            ln15.Mes3 = $(".ln15Mes3").getVal(2);
            ln15.Mes4 = $(".ln15Mes4").getVal(2);
            ln15.Mes5 = $(".ln15Mes5").getVal(2);
            ln15.Mes6 = $(".ln15Mes6").getVal(2);
            ln15.Mes7 = $(".ln15Mes7").getVal(2);
            ln15.Mes8 = $(".ln15Mes8").getVal(2);
            ln15.Mes9 = $(".ln15Mes9").getVal(2);
            ln15.Mes10 = $(".ln15Mes10").getVal(2);
            ln15.Mes11 = $(".ln15Mes11").getVal(2);
            ln15.Mes12 = $(".ln15Mes12").getVal(2);
            ln15.MesT = 0;

            var ln16 = {};
            ln16.Mes1 = $(".ln16Mes1").getVal(2);
            ln16.Mes2 = $(".ln16Mes2").getVal(2);
            ln16.Mes3 = $(".ln16Mes3").getVal(2);
            ln16.Mes4 = $(".ln16Mes4").getVal(2);
            ln16.Mes5 = $(".ln16Mes5").getVal(2);
            ln16.Mes6 = $(".ln16Mes6").getVal(2);
            ln16.Mes7 = $(".ln16Mes7").getVal(2);
            ln16.Mes8 = $(".ln16Mes8").getVal(2);
            ln16.Mes9 = $(".ln16Mes9").getVal(2);
            ln16.Mes10 = $(".ln16Mes10").getVal(2);
            ln16.Mes11 = $(".ln16Mes11").getVal(2);
            ln16.Mes12 = $(".ln16Mes12").getVal(2);
            ln16.MesT = $(".ln16MesT").getVal(2);

            var ln17 = {};
            ln17.Mes1 = $(".ln17Mes1").getVal(2);
            ln17.Mes2 = $(".ln17Mes2").getVal(2);
            ln17.Mes3 = $(".ln17Mes3").getVal(2);
            ln17.Mes4 = $(".ln17Mes4").getVal(2);
            ln17.Mes5 = $(".ln17Mes5").getVal(2);
            ln17.Mes6 = $(".ln17Mes6").getVal(2);
            ln17.Mes7 = $(".ln17Mes7").getVal(2);
            ln17.Mes8 = $(".ln17Mes8").getVal(2);
            ln17.Mes9 = $(".ln17Mes9").getVal(2);
            ln17.Mes10 = $(".ln17Mes10").getVal(2);
            ln17.Mes11 = $(".ln17Mes11").getVal(2);
            ln17.Mes12 = $(".ln17Mes12").getVal(2);
            ln17.MesT = $(".ln17MesT").getVal(2);

            var ln18 = {};
            ln18.Mes1 = $(".ln18Mes1").getVal(2);
            ln18.Mes2 = $(".ln18Mes2").getVal(2);
            ln18.Mes3 = $(".ln18Mes3").getVal(2);
            ln18.Mes4 = $(".ln18Mes4").getVal(2);
            ln18.Mes5 = $(".ln18Mes5").getVal(2);
            ln18.Mes6 = $(".ln18Mes6").getVal(2);
            ln18.Mes7 = $(".ln18Mes7").getVal(2);
            ln18.Mes8 = $(".ln18Mes8").getVal(2);
            ln18.Mes9 = $(".ln18Mes9").getVal(2);
            ln18.Mes10 = $(".ln18Mes10").getVal(2);
            ln18.Mes11 = $(".ln18Mes11").getVal(2);
            ln18.Mes12 = $(".ln18Mes12").getVal(2);
            ln18.MesT = $(".ln18MesT").getVal(2);

            var ln19 = {};
            ln19.Mes1 = $(".ln19Mes1").getVal(2);
            ln19.Mes2 = $(".ln19Mes2").getVal(2);
            ln19.Mes3 = $(".ln19Mes3").getVal(2);
            ln19.Mes4 = $(".ln19Mes4").getVal(2);
            ln19.Mes5 = $(".ln19Mes5").getVal(2);
            ln19.Mes6 = $(".ln19Mes6").getVal(2);
            ln19.Mes7 = $(".ln19Mes7").getVal(2);
            ln19.Mes8 = $(".ln19Mes8").getVal(2);
            ln19.Mes9 = $(".ln19Mes9").getVal(2);
            ln19.Mes10 = $(".ln19Mes10").getVal(2);
            ln19.Mes11 = $(".ln19Mes11").getVal(2);
            ln19.Mes12 = $(".ln19Mes12").getVal(2);
            ln19.MesT = $(".ln19MesT").getVal(2);

            var ln20 = {};
            ln20.Mes1 = $(".ln20Mes1").getVal(2);
            ln20.Mes2 = $(".ln20Mes2").getVal(2);
            ln20.Mes3 = $(".ln20Mes3").getVal(2);
            ln20.Mes4 = $(".ln20Mes4").getVal(2);
            ln20.Mes5 = $(".ln20Mes5").getVal(2);
            ln20.Mes6 = $(".ln20Mes6").getVal(2);
            ln20.Mes7 = $(".ln20Mes7").getVal(2);
            ln20.Mes8 = $(".ln20Mes8").getVal(2);
            ln20.Mes9 = $(".ln20Mes9").getVal(2);
            ln20.Mes10 = $(".ln20Mes10").getVal(2);
            ln20.Mes11 = $(".ln20Mes11").getVal(2);
            ln20.Mes12 = $(".ln20Mes12").getVal(2);
            ln20.MesT = $(".ln20MesT").getVal(2);

            var ln21 = {};
            ln21.Mes1 = $(".ln21Mes1").getVal(2);
            ln21.Mes2 = $(".ln21Mes2").getVal(2);
            ln21.Mes3 = $(".ln21Mes3").getVal(2);
            ln21.Mes4 = $(".ln21Mes4").getVal(2);
            ln21.Mes5 = $(".ln21Mes5").getVal(2);
            ln21.Mes6 = $(".ln21Mes6").getVal(2);
            ln21.Mes7 = $(".ln21Mes7").getVal(2);
            ln21.Mes8 = $(".ln21Mes8").getVal(2);
            ln21.Mes9 = $(".ln21Mes9").getVal(2);
            ln21.Mes10 = $(".ln21Mes10").getVal(2);
            ln21.Mes11 = $(".ln21Mes11").getVal(2);
            ln21.Mes12 = $(".ln21Mes12").getVal(2);
            ln21.MesT = $(".ln21MesT").getVal(2);

            var ln22 = {};
            ln22.Mes1 = $(".ln22Mes1").getVal(2);
            ln22.Mes2 = $(".ln22Mes2").getVal(2);
            ln22.Mes3 = $(".ln22Mes3").getVal(2);
            ln22.Mes4 = $(".ln22Mes4").getVal(2);
            ln22.Mes5 = $(".ln22Mes5").getVal(2);
            ln22.Mes6 = $(".ln22Mes6").getVal(2);
            ln22.Mes7 = $(".ln22Mes7").getVal(2);
            ln22.Mes8 = $(".ln22Mes8").getVal(2);
            ln22.Mes9 = $(".ln22Mes9").getVal(2);
            ln22.Mes10 = $(".ln22Mes10").getVal(2);
            ln22.Mes11 = $(".ln22Mes11").getVal(2);
            ln22.Mes12 = $(".ln22Mes12").getVal(2);
            ln22.MesT = $(".ln22MesT").getVal(2);

            var ln23 = {};
            ln23.Mes1 = $(".ln23Mes1").getVal(2);
            ln23.Mes2 = $(".ln23Mes2").getVal(2);
            ln23.Mes3 = $(".ln23Mes3").getVal(2);
            ln23.Mes4 = $(".ln23Mes4").getVal(2);
            ln23.Mes5 = $(".ln23Mes5").getVal(2);
            ln23.Mes6 = $(".ln23Mes6").getVal(2);
            ln23.Mes7 = $(".ln23Mes7").getVal(2);
            ln23.Mes8 = $(".ln23Mes8").getVal(2);
            ln23.Mes9 = $(".ln23Mes9").getVal(2);
            ln23.Mes10 = $(".ln23Mes10").getVal(2);
            ln23.Mes11 = $(".ln23Mes11").getVal(2);
            ln23.Mes12 = $(".ln23Mes12").getVal(2);
            ln23.MesT = $(".ln23MesT").getVal(2);

            var ln24 = {};
            ln24.Mes1 = $(".ln24Mes1").getVal(2);
            ln24.Mes2 = $(".ln24Mes2").getVal(2);
            ln24.Mes3 = $(".ln24Mes3").getVal(2);
            ln24.Mes4 = $(".ln24Mes4").getVal(2);
            ln24.Mes5 = $(".ln24Mes5").getVal(2);
            ln24.Mes6 = $(".ln24Mes6").getVal(2);
            ln24.Mes7 = $(".ln24Mes7").getVal(2);
            ln24.Mes8 = $(".ln24Mes8").getVal(2);
            ln24.Mes9 = $(".ln24Mes9").getVal(2);
            ln24.Mes10 = $(".ln24Mes10").getVal(2);
            ln24.Mes11 = $(".ln24Mes11").getVal(2);
            ln24.Mes12 = $(".ln24Mes12").getVal(2);
            ln24.MesT = $(".ln24MesT").getVal(2);

            var ln25 = {};
            ln25.Mes1 = $(".ln25Mes1").getVal(2);
            ln25.Mes2 = $(".ln25Mes2").getVal(2);
            ln25.Mes3 = $(".ln25Mes3").getVal(2);
            ln25.Mes4 = $(".ln25Mes4").getVal(2);
            ln25.Mes5 = $(".ln25Mes5").getVal(2);
            ln25.Mes6 = $(".ln25Mes6").getVal(2);
            ln25.Mes7 = $(".ln25Mes7").getVal(2);
            ln25.Mes8 = $(".ln25Mes8").getVal(2);
            ln25.Mes9 = $(".ln25Mes9").getVal(2);
            ln25.Mes10 = $(".ln25Mes10").getVal(2);
            ln25.Mes11 = $(".ln25Mes11").getVal(2);
            ln25.Mes12 = $(".ln25Mes12").getVal(2);
            ln25.MesT = $(".ln25MesT").getVal(2);

            var ln26 = {};
            ln26.Mes1 = $(".ln26Mes1").getVal(2);
            ln26.Mes2 = $(".ln26Mes2").getVal(2);
            ln26.Mes3 = $(".ln26Mes3").getVal(2);
            ln26.Mes4 = $(".ln26Mes4").getVal(2);
            ln26.Mes5 = $(".ln26Mes5").getVal(2);
            ln26.Mes6 = $(".ln26Mes6").getVal(2);
            ln26.Mes7 = $(".ln26Mes7").getVal(2);
            ln26.Mes8 = $(".ln26Mes8").getVal(2);
            ln26.Mes9 = $(".ln26Mes9").getVal(2);
            ln26.Mes10 = $(".ln26Mes10").getVal(2);
            ln26.Mes11 = $(".ln26Mes11").getVal(2);
            ln26.Mes12 = $(".ln26Mes12").getVal(2);
            ln26.MesT = $(".ln26MesT").getVal(2);

            var ln27 = {};
            ln27.Mes1 = $(".ln27Mes1").getVal(2);
            ln27.Mes2 = $(".ln27Mes2").getVal(2);
            ln27.Mes3 = $(".ln27Mes3").getVal(2);
            ln27.Mes4 = $(".ln27Mes4").getVal(2);
            ln27.Mes5 = $(".ln27Mes5").getVal(2);
            ln27.Mes6 = $(".ln27Mes6").getVal(2);
            ln27.Mes7 = $(".ln27Mes7").getVal(2);
            ln27.Mes8 = $(".ln27Mes8").getVal(2);
            ln27.Mes9 = $(".ln27Mes9").getVal(2);
            ln27.Mes10 = $(".ln27Mes10").getVal(2);
            ln27.Mes11 = $(".ln27Mes11").getVal(2);
            ln27.Mes12 = $(".ln27Mes12").getVal(2);
            ln27.MesT = $(".ln27MesT").getVal(2);

            var ln28 = {};
            ln28.Mes1 = $(".ln28Mes1").getVal(2);
            ln28.Mes2 = $(".ln28Mes2").getVal(2);
            ln28.Mes3 = $(".ln28Mes3").getVal(2);
            ln28.Mes4 = $(".ln28Mes4").getVal(2);
            ln28.Mes5 = $(".ln28Mes5").getVal(2);
            ln28.Mes6 = $(".ln28Mes6").getVal(2);
            ln28.Mes7 = $(".ln28Mes7").getVal(2);
            ln28.Mes8 = $(".ln28Mes8").getVal(2);
            ln28.Mes9 = $(".ln28Mes9").getVal(2);
            ln28.Mes10 = $(".ln28Mes10").getVal(2);
            ln28.Mes11 = $(".ln28Mes11").getVal(2);
            ln28.Mes12 = $(".ln28Mes12").getVal(2);
            ln28.MesT = $(".ln28MesT").getVal(2);

            var ln29 = {};
            ln29.Mes1 = $(".ln29Mes1").getVal(2);
            ln29.Mes2 = $(".ln29Mes2").getVal(2);
            ln29.Mes3 = $(".ln29Mes3").getVal(2);
            ln29.Mes4 = $(".ln29Mes4").getVal(2);
            ln29.Mes5 = $(".ln29Mes5").getVal(2);
            ln29.Mes6 = $(".ln29Mes6").getVal(2);
            ln29.Mes7 = $(".ln29Mes7").getVal(2);
            ln29.Mes8 = $(".ln29Mes8").getVal(2);
            ln29.Mes9 = $(".ln29Mes9").getVal(2);
            ln29.Mes10 = $(".ln29Mes10").getVal(2);
            ln29.Mes11 = $(".ln29Mes11").getVal(2);
            ln29.Mes12 = $(".ln29Mes12").getVal(2);
            ln29.MesT = $(".ln29MesT").getVal(2);



            obj.PagExt = ln1;
            obj.PorcSaldoAmortAbono = ln2;
            obj.CtoAplicarCXC = ln3;
            obj.ImporteAmortAbono = ln4;
            obj.PorcSaldoAmortCompania = ln5;
            obj.ImporteAmortCompania = ln6;
            obj.Saldo = $(".tdSaldo").getVal(2);
            obj.PuntosAdic = $(".tdPuntosAdic").getVal(2);
            obj.TasaAnual = ln7;
            obj.PagoCapital = $(".tdPagoCapital").getVal(2);
            obj.SaldoAmortCompania = ln8;
            obj.AmortCapitalCompania = ln9;
            obj.AmortVencidasCapitalCompania = ln10;
            obj.InteresesGenerados = ln11;
            obj.PorcSaldoAmortAcreedores = ln12;
            obj.ImporteAmortAcreedores = ln13;
            obj.GastosDiferidos = ln14;
            obj.SaldoFlujo = ln15;
            var rowsPagosVarios = $('.tbodyDesglose tr.rowDesgloseData');


            obj.DesgloseVariosPagos = new Array();
            $.each(rowsPagosVarios, function (i, e) {
                var o = {};
                o.Concepto = $($(e).find(".lnConcepto")).html();
                o.Mes1 = $($(e).find(".lnMes1")).getVal(2);
                o.Mes2 = $($(e).find(".lnMes2")).getVal(2);
                o.Mes3 = $($(e).find(".lnMes3")).getVal(2);
                o.Mes4 = $($(e).find(".lnMes4")).getVal(2);
                o.Mes5 = $($(e).find(".lnMes5")).getVal(2);
                o.Mes6 = $($(e).find(".lnMes6")).getVal(2);
                o.Mes7 = $($(e).find(".lnMes7")).getVal(2);
                o.Mes8 = $($(e).find(".lnMes8")).getVal(2);
                o.Mes9 = $($(e).find(".lnMes9")).getVal(2);
                o.Mes10 = $($(e).find(".lnMes10")).getVal(2);
                o.Mes11 = $($(e).find(".lnMes11")).getVal(2);
                o.Mes12 = $($(e).find(".lnMes12")).getVal(2);
                o.MesT = $($(e).find(".lnMesT")).getVal(2);
                obj.DesgloseVariosPagos.push(o);
            });

            obj.Reserva_CBA = ln26;
            obj.Reserva_BA = ln27;
            obj.Reserva_A = ln28;

            obj.TotalConceptos = ln29;


            return obj;
        }
        function setPlainObject(obj) {
            var ln1 = obj.PagExt;
            var ln2 = obj.PorcSaldoAmortAbono;
            var ln3 = obj.CtoAplicarCXC;
            //var ln4Base = obj.BaseImporteAmortAbono;
            var ln4 = obj.ImporteAmortAbono;
            var ln5 = obj.PorcSaldoAmortCompania;
            //var ln6Base = obj.BaseImporteAmortCompania;
            var ln6 = obj.ImporteAmortCompania;
            var tdSaldo = obj.Saldo;
            var tdPuntoAdic = obj.PuntosAdic;
            //var ln7Base = obj.BaseTasaAnual;
            var ln7 = obj.TasaAnual;
            var tdPagoCapital = obj.PagoCapital;
            var ln8 = obj.SaldoAmortCompania;
            var ln9 = obj.AmortCapitalCompania;
            var ln10 = obj.AmortVencidasCapitalCompania;
            var ln11 = obj.InteresesGenerados;
            var ln12 = obj.PorcSaldoAmortAcreedores;
            //var ln13Base = obj.BaseImporteAmortAcreedores;
            var ln13 = obj.ImporteAmortAcreedores;
            var ln14 = obj.GastosDiferidos;
            var ln15 = obj.SaldoFlujo;

            var DesglosePagos = obj.DesgloseVariosPagos;

            var ln26 = obj.Reserva_CBA;
            var ln27 = obj.Reserva_BA;
            var ln28 = obj.Reserva_A;

            var ln29 = obj.TotalConceptos;


            $(".ln1Mes1").setVal(ln1.Mes1);
            $(".ln1Mes2").setVal(ln1.Mes2);
            $(".ln1Mes3").setVal(ln1.Mes3);
            $(".ln1Mes4").setVal(ln1.Mes4);
            $(".ln1Mes5").setVal(ln1.Mes5);
            $(".ln1Mes6").setVal(ln1.Mes6);
            $(".ln1Mes7").setVal(ln1.Mes7);
            $(".ln1Mes8").setVal(ln1.Mes8);
            $(".ln1Mes9").setVal(ln1.Mes9);
            $(".ln1Mes10").setVal(ln1.Mes10);
            $(".ln1Mes11").setVal(ln1.Mes11);
            $(".ln1Mes12").setVal(ln1.Mes12);
            $(".ln1MesT").setVal(ln1.MesT);

            $(".ln2Mes1").setVal(ln2.Mes1);
            $(".ln2Mes2").setVal(ln2.Mes2);
            $(".ln2Mes3").setVal(ln2.Mes3);
            $(".ln2Mes4").setVal(ln2.Mes4);
            $(".ln2Mes5").setVal(ln2.Mes5);
            $(".ln2Mes6").setVal(ln2.Mes6);
            $(".ln2Mes7").setVal(ln2.Mes7);
            $(".ln2Mes8").setVal(ln2.Mes8);
            $(".ln2Mes9").setVal(ln2.Mes9);
            $(".ln2Mes10").setVal(ln2.Mes10);
            $(".ln2Mes11").setVal(ln2.Mes11);
            $(".ln2Mes12").setVal(ln2.Mes12);
            $(".ln2MesT").setVal(ln2.MesT);

            $(".ln3Mes1").setVal(ln3.Mes1);
            $(".ln3Mes2").setVal(ln3.Mes2);
            $(".ln3Mes3").setVal(ln3.Mes3);
            $(".ln3Mes4").setVal(ln3.Mes4);
            $(".ln3Mes5").setVal(ln3.Mes5);
            $(".ln3Mes6").setVal(ln3.Mes6);
            $(".ln3Mes7").setVal(ln3.Mes7);
            $(".ln3Mes8").setVal(ln3.Mes8);
            $(".ln3Mes9").setVal(ln3.Mes9);
            $(".ln3Mes10").setVal(ln3.Mes10);
            $(".ln3Mes11").setVal(ln3.Mes11);
            $(".ln3Mes12").setVal(ln3.Mes12);
            $(".ln3MesT").setVal(ln3.MesT);


            //
            $(".ln4Mes1").setVal(ln4.Mes1);
            $(".ln4Mes2").setVal(ln4.Mes2);
            $(".ln4Mes3").setVal(ln4.Mes3);
            $(".ln4Mes4").setVal(ln4.Mes4);
            $(".ln4Mes5").setVal(ln4.Mes5);
            $(".ln4Mes6").setVal(ln4.Mes6);
            $(".ln4Mes7").setVal(ln4.Mes7);
            $(".ln4Mes8").setVal(ln4.Mes8);
            $(".ln4Mes9").setVal(ln4.Mes9);
            $(".ln4Mes10").setVal(ln4.Mes10);
            $(".ln4Mes11").setVal(ln4.Mes11);
            $(".ln4Mes12").setVal(ln4.Mes12);
            $(".ln4MesT").setVal(ln4.MesT);


            $(".ln5Mes1").setVal(ln5.Mes1);
            $(".ln5Mes2").setVal(ln5.Mes2);
            $(".ln5Mes3").setVal(ln5.Mes3);
            $(".ln5Mes4").setVal(ln5.Mes4);
            $(".ln5Mes5").setVal(ln5.Mes5);
            $(".ln5Mes6").setVal(ln5.Mes6);
            $(".ln5Mes7").setVal(ln5.Mes7);
            $(".ln5Mes8").setVal(ln5.Mes8);
            $(".ln5Mes9").setVal(ln5.Mes9);
            $(".ln5Mes10").setVal(ln5.Mes10);
            $(".ln5Mes11").setVal(ln5.Mes11);
            $(".ln5Mes12").setVal(ln5.Mes12);
            $(".ln5MesT").setVal(ln5.MesT);

            //
            $(".ln6Mes1").setVal(ln6.Mes1);
            $(".ln6Mes2").setVal(ln6.Mes2);
            $(".ln6Mes3").setVal(ln6.Mes3);
            $(".ln6Mes4").setVal(ln6.Mes4);
            $(".ln6Mes5").setVal(ln6.Mes5);
            $(".ln6Mes6").setVal(ln6.Mes6);
            $(".ln6Mes7").setVal(ln6.Mes7);
            $(".ln6Mes8").setVal(ln6.Mes8);
            $(".ln6Mes9").setVal(ln6.Mes9);
            $(".ln6Mes10").setVal(ln6.Mes10);
            $(".ln6Mes11").setVal(ln6.Mes11);
            $(".ln6Mes12").setVal(ln6.Mes12);
            $(".ln6MesT").setVal(ln6.MesT);


            //
            $(".ln7Mes1").setVal(ln7.Mes1);
            $(".ln7Mes2").setVal(ln7.Mes2);
            $(".ln7Mes3").setVal(ln7.Mes3);
            $(".ln7Mes4").setVal(ln7.Mes4);
            $(".ln7Mes5").setVal(ln7.Mes5);
            $(".ln7Mes6").setVal(ln7.Mes6);
            $(".ln7Mes7").setVal(ln7.Mes7);
            $(".ln7Mes8").setVal(ln7.Mes8);
            $(".ln7Mes9").setVal(ln7.Mes9);
            $(".ln7Mes10").setVal(ln7.Mes10);
            $(".ln7Mes11").setVal(ln7.Mes11);
            $(".ln7Mes12").setVal(ln7.Mes12);
            //$(".ln7MesT").setVal(ln7.MesT);

            $(".ln8Mes1").setVal(ln8.Mes1);
            $(".ln8Mes2").setVal(ln8.Mes2);
            $(".ln8Mes3").setVal(ln8.Mes3);
            $(".ln8Mes4").setVal(ln8.Mes4);
            $(".ln8Mes5").setVal(ln8.Mes5);
            $(".ln8Mes6").setVal(ln8.Mes6);
            $(".ln8Mes7").setVal(ln8.Mes7);
            $(".ln8Mes8").setVal(ln8.Mes8);
            $(".ln8Mes9").setVal(ln8.Mes9);
            $(".ln8Mes10").setVal(ln8.Mes10);
            $(".ln8Mes11").setVal(ln8.Mes11);
            $(".ln8Mes12").setVal(ln8.Mes12);
            $(".ln8MesT").setVal(ln8.MesT);

            $(".ln9Mes1").setVal(ln9.Mes1);
            $(".ln9Mes2").setVal(ln9.Mes2);
            $(".ln9Mes3").setVal(ln9.Mes3);
            $(".ln9Mes4").setVal(ln9.Mes4);
            $(".ln9Mes5").setVal(ln9.Mes5);
            $(".ln9Mes6").setVal(ln9.Mes6);
            $(".ln9Mes7").setVal(ln9.Mes7);
            $(".ln9Mes8").setVal(ln9.Mes8);
            $(".ln9Mes9").setVal(ln9.Mes9);
            $(".ln9Mes10").setVal(ln9.Mes10);
            $(".ln9Mes11").setVal(ln9.Mes11);
            $(".ln9Mes12").setVal(ln9.Mes12);
            $(".ln9MesT").setVal(ln9.MesT);

            $(".ln10Mes1").setVal(ln10.Mes1);
            $(".ln10Mes2").setVal(ln10.Mes2);
            $(".ln10Mes3").setVal(ln10.Mes3);
            $(".ln10Mes4").setVal(ln10.Mes4);
            $(".ln10Mes5").setVal(ln10.Mes5);
            $(".ln10Mes6").setVal(ln10.Mes6);
            $(".ln10Mes7").setVal(ln10.Mes7);
            $(".ln10Mes8").setVal(ln10.Mes8);
            $(".ln10Mes9").setVal(ln10.Mes9);
            $(".ln10Mes10").setVal(ln10.Mes10);
            $(".ln10Mes11").setVal(ln10.Mes11);
            $(".ln10Mes12").setVal(ln10.Mes12);
            $(".ln10MesT").setVal(ln10.MesT);

            $(".ln11Mes1").setVal(ln11.Mes1);
            $(".ln11Mes2").setVal(ln11.Mes2);
            $(".ln11Mes3").setVal(ln11.Mes3);
            $(".ln11Mes4").setVal(ln11.Mes4);
            $(".ln11Mes5").setVal(ln11.Mes5);
            $(".ln11Mes6").setVal(ln11.Mes6);
            $(".ln11Mes7").setVal(ln11.Mes7);
            $(".ln11Mes8").setVal(ln11.Mes8);
            $(".ln11Mes9").setVal(ln11.Mes9);
            $(".ln11Mes10").setVal(ln11.Mes10);
            $(".ln11Mes11").setVal(ln11.Mes11);
            $(".ln11Mes12").setVal(ln11.Mes12);
            $(".ln11MesT").setVal(ln11.MesT);

            $(".ln12Mes1").setVal(ln12.Mes1);
            $(".ln12Mes2").setVal(ln12.Mes2);
            $(".ln12Mes3").setVal(ln12.Mes3);
            $(".ln12Mes4").setVal(ln12.Mes4);
            $(".ln12Mes5").setVal(ln12.Mes5);
            $(".ln12Mes6").setVal(ln12.Mes6);
            $(".ln12Mes7").setVal(ln12.Mes7);
            $(".ln12Mes8").setVal(ln12.Mes8);
            $(".ln12Mes9").setVal(ln12.Mes9);
            $(".ln12Mes10").setVal(ln12.Mes10);
            $(".ln12Mes11").setVal(ln12.Mes11);
            $(".ln12Mes12").setVal(ln12.Mes12);
            $(".ln12MesT").setVal(ln12.MesT);
          
            $(".ln13Mes1").setVal(ln13.Mes1);
            $(".ln13Mes2").setVal(ln13.Mes2);
            $(".ln13Mes3").setVal(ln13.Mes3);
            $(".ln13Mes4").setVal(ln13.Mes4);
            $(".ln13Mes5").setVal(ln13.Mes5);
            $(".ln13Mes6").setVal(ln13.Mes6);
            $(".ln13Mes7").setVal(ln13.Mes7);
            $(".ln13Mes8").setVal(ln13.Mes8);
            $(".ln13Mes9").setVal(ln13.Mes9);
            $(".ln13Mes10").setVal(ln13.Mes10);
            $(".ln13Mes11").setVal(ln13.Mes11);
            $(".ln13Mes12").setVal(ln13.Mes12);
            $(".ln13MesT").setVal(ln13.MesT);

            $(".ln14Mes1").setVal(ln14.Mes1);
            $(".ln14Mes2").setVal(ln14.Mes2);
            $(".ln14Mes3").setVal(ln14.Mes3);
            $(".ln14Mes4").setVal(ln14.Mes4);
            $(".ln14Mes5").setVal(ln14.Mes5);
            $(".ln14Mes6").setVal(ln14.Mes6);
            $(".ln14Mes7").setVal(ln14.Mes7);
            $(".ln14Mes8").setVal(ln14.Mes8);
            $(".ln14Mes9").setVal(ln14.Mes9);
            $(".ln14Mes10").setVal(ln14.Mes10);
            $(".ln14Mes11").setVal(ln14.Mes11);
            $(".ln14Mes12").setVal(ln14.Mes12);
            $(".ln14MesT").setVal(ln14.MesT);

            $(".ln15Mes1").setVal(ln15.Mes1);
            $(".ln15Mes2").setVal(ln15.Mes2);
            $(".ln15Mes3").setVal(ln15.Mes3);
            $(".ln15Mes4").setVal(ln15.Mes4);
            $(".ln15Mes5").setVal(ln15.Mes5);
            $(".ln15Mes6").setVal(ln15.Mes6);
            $(".ln15Mes7").setVal(ln15.Mes7);
            $(".ln15Mes8").setVal(ln15.Mes8);
            $(".ln15Mes9").setVal(ln15.Mes9);
            $(".ln15Mes10").setVal(ln15.Mes10);
            $(".ln15Mes11").setVal(ln15.Mes11);
            $(".ln15Mes12").setVal(ln15.Mes12);
            //$(".ln15MesT").setVal(ln15.MesT);
            var trs = '';

            $.each(DesglosePagos, function (i, e) {
                var mesT = e.Mes1 + e.Mes2 + e.Mes3 + e.Mes4 + e.Mes5 + e.Mes6 + e.Mes7 + e.Mes8 + e.Mes9 + e.Mes10 + e.Mes11 + e.Mes12;
                trs += '<tr class="rowDesgloseData">' +
                           '<td colspan="2" class="lnConcepto">' + e.Concepto + '</td>' +
                                                   '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMesT calculo rDesglose dt-body-right setTamañoInput" value="' + mesT + '" disabled>' +
                               '</div>' +
                           '</td>' +
                           '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMes1 calculo rDesglose dt-body-right setTamañoInput" value="' + e.Mes1 + '">' +
                               '</div>' +
                           '</td>' +
                           '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMes2 calculo rDesglose dt-body-right setTamañoInput" value="' + e.Mes2 + '">' +
                               '</div>' +
                           '</td>' +
                           '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMes3 calculo rDesglose dt-body-right setTamañoInput" value="' + e.Mes3 + '">' +
                               '</div>' +
                           '</td>' +
                           '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMes4 calculo rDesglose dt-body-right setTamañoInput" value="' + e.Mes4 + '">' +
                               '</div>' +
                           '</td>' +
                           '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMes5 calculo rDesglose dt-body-right setTamañoInput" value="' + e.Mes5 + '">' +
                               '</div>' +
                           '</td>' +
                           '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMes6 calculo rDesglose dt-body-right setTamañoInput" value="' + e.Mes6 + '">' +
                               '</div>' +
                           '</td>' +
                           '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMes7 calculo rDesglose dt-body-right setTamañoInput" value="' + e.Mes7 + '">' +
                               '</div>' +
                           '</td>' +
                           '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMes8 calculo rDesglose dt-body-right setTamañoInput" value="' + e.Mes8 + '">' +
                               '</div>' +
                           '</td>' +
                           '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMes9 calculo rDesglose dt-body-right setTamañoInput" value="' + e.Mes9 + '">' +
                               '</div>' +
                           '</td>' +
                           '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMes10 calculo rDesglose dt-body-right setTamañoInput" value="' + e.Mes10 + '">' +
                               '</div>' +
                           '</td>' +
                           '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMes11 calculo rDesglose dt-body-right setTamañoInput" value="' + e.Mes11 + '">' +
                               '</div>' +
                           '</td>' +
                           '<td>' +
                               '<div class="input-group">' +
                                   '<input type="text" class="form-control lnMes12 calculo rDesglose dt-body-right setTamañoInput" value="' + e.Mes12 + '">' +
                               '</div>' +
                           '</td>' +

                       '</tr>';
            });
            $('.tbodyDesglose tr.rowDesglose').before(trs);
            var rDesglose = $(".rDesglose");
            $.each(rDesglose, function (i, e) {
                if ($(this).hasClass("esPr")) {
                    $(e).DecimalFixPr(2);
                }
                else {
                    $(e).DecimalFixNS(0);
                }
            });
            $(".ln26Mes1").setVal(ln26.Mes1);
            $(".ln26Mes2").setVal(ln26.Mes2);
            $(".ln26Mes3").setVal(ln26.Mes3);
            $(".ln26Mes4").setVal(ln26.Mes4);
            $(".ln26Mes5").setVal(ln26.Mes5);
            $(".ln26Mes6").setVal(ln26.Mes6);
            $(".ln26Mes7").setVal(ln26.Mes7);
            $(".ln26Mes8").setVal(ln26.Mes8);
            $(".ln26Mes9").setVal(ln26.Mes9);
            $(".ln26Mes10").setVal(ln26.Mes10);
            $(".ln26Mes11").setVal(ln26.Mes11);
            $(".ln26Mes12").setVal(ln26.Mes12);
            $(".ln26MesT").setVal(ln26.MesT);

            $(".ln27Mes1").setVal(ln27.MesT);
            $(".ln27Mes2").setVal(ln27.Mes1);
            $(".ln27Mes3").setVal(ln27.Mes2);
            $(".ln27Mes4").setVal(ln27.Mes3);
            $(".ln27Mes5").setVal(ln27.Mes4);
            $(".ln27Mes6").setVal(ln27.Mes5);
            $(".ln27Mes7").setVal(ln27.Mes6);
            $(".ln27Mes8").setVal(ln27.Mes7);
            $(".ln27Mes9").setVal(ln27.Mes8);
            $(".ln27Mes10").setVal(ln27.Mes9);
            $(".ln27Mes11").setVal(ln27.Mes10);
            $(".ln27Mes12").setVal(ln27.Mes11);
            $(".ln27MesT").setVal(ln27.Mes12);
            

            $(".ln28Mes1").setVal(ln28.Mes1);
            $(".ln28Mes2").setVal(ln28.Mes2);
            $(".ln28Mes3").setVal(ln28.Mes3);
            $(".ln28Mes4").setVal(ln28.Mes4);
            $(".ln28Mes5").setVal(ln28.Mes5);
            $(".ln28Mes6").setVal(ln28.Mes6);
            $(".ln28Mes7").setVal(ln28.Mes7);
            $(".ln28Mes8").setVal(ln28.Mes8);
            $(".ln28Mes9").setVal(ln28.Mes9);
            $(".ln28Mes10").setVal(ln28.Mes10);
            $(".ln28Mes11").setVal(ln28.Mes11);
            $(".ln28Mes12").setVal(ln28.Mes12);
            $(".ln28MesT").setVal(ln28.MesT);

            $(".ln29Mes2").setVal(ln29.Mes1);
            $(".ln29Mes3").setVal(ln29.Mes2);
            $(".ln29Mes4").setVal(ln29.Mes3);
            $(".ln29Mes5").setVal(ln29.Mes4);
            $(".ln29Mes6").setVal(ln29.Mes5);
            $(".ln29Mes7").setVal(ln29.Mes6);
            $(".ln29Mes8").setVal(ln29.Mes7);
            $(".ln29Mes9").setVal(ln29.Mes8);
            $(".ln29Mes10").setVal(ln29.Mes9);
            $(".ln29Mes11").setVal(ln29.Mes10);
            $(".ln29Mes12").setVal(ln29.Mes11);
            $(".ln29MesT").setVal(ln29.Mes12);
            $(".ln29Mes1").setVal(ln29.MesT);

            $(".lnMesT").trigger('change');
            calculos();
        }
        function calculos() {
            var col = $($(this)[0]).data("col");
            var objFiltro = getFGB();
            //var objFiltro = {
            //    escenario: 1,
            //    divisor: 1000,
            //    mes: 6,
            //    anio: 2017,
            //    inclCM: 0,
            //    reduccion: 0
            //};

            var o = getPlainObject();
            var ln2 = o.PorcSaldoAmortAbono;
            var ln4 = o.ImporteAmortAbono;
            var ln12 = o.PorcSaldoAmortAcreedores;
            var ln13 = o.ImporteAmortAcreedores;
            //Importe a amortizar abonos
            $.ajax({
                url: '/Proyecciones/getPDLN4',
                type: 'POST',
                dataType: 'json',
                data: { ln2: ln2, ln4: ln4, objFiltro: objFiltro, col: col },
                success: function (response) {
                    var r = response.r;
                    $(".ln2MesT").setVal(ln2.Mes1 + ln2.Mes2 + ln2.Mes3 + ln2.Mes4 + ln2.Mes5 + ln2.Mes6 + ln2.Mes7 + ln2.Mes8 + ln2.Mes9 + ln2.Mes10 + ln2.Mes11 + ln2.Mes12);
                    $(".ln4Mes1").setVal(r.Mes1);
                    $(".ln4Mes2").setVal(r.Mes2);
                    $(".ln4Mes3").setVal(r.Mes3);
                    $(".ln4Mes4").setVal(r.Mes4);
                    $(".ln4Mes5").setVal(r.Mes5);
                    $(".ln4Mes6").setVal(r.Mes6);
                    $(".ln4Mes7").setVal(r.Mes7);
                    $(".ln4Mes8").setVal(r.Mes8);
                    $(".ln4Mes9").setVal(r.Mes9);
                    $(".ln4Mes10").setVal(r.Mes10);
                    $(".ln4Mes11").setVal(r.Mes11);
                    $(".ln4Mes12").setVal(r.Mes12);
                    $(".ln4MesT").setVal(r.MesT);
                }
            });

            //Saldo amortizar compañias

            var ln5Mes1 = $(".ln5Mes1").getVal(2);
            var ln6Mes1Base = Number($(".ln6Mes1").data("valor"));
            $(".ln6Mes1").setVal((ln6Mes1Base * (ln5Mes1 / 100)));

            var ln5Mes2 = $(".ln5Mes2").getVal(2);
            var ln6Mes2Base = Number($(".ln6Mes2").data("valor"));
            $(".ln6Mes2").setVal((ln6Mes2Base * (ln5Mes2 / 100)));

            var ln5Mes3 = $(".ln5Mes3").getVal(2);
            var ln6Mes3Base = Number($(".ln6Mes3").data("valor"));
            $(".ln6Mes3").setVal((ln6Mes3Base * (ln5Mes3 / 100)));

            var ln5Mes4 = $(".ln5Mes4").getVal(2);
            var ln6Mes4Base = Number($(".ln6Mes4").data("valor"));
            $(".ln6Mes4").setVal((ln6Mes4Base * (ln5Mes4 / 100)));

            var ln5Mes5 = $(".ln5Mes5").getVal(2);
            var ln6Mes5Base = Number($(".ln6Mes5").data("valor"));
            $(".ln6Mes5").setVal((ln6Mes5Base * (ln5Mes5 / 100)));

            var ln5Mes6 = $(".ln5Mes6").getVal(2);
            var ln6Mes6Base = Number($(".ln6Mes6").data("valor"));
            $(".ln6Mes6").setVal((ln6Mes6Base * (ln5Mes6 / 100)));

            var ln5Mes7 = $(".ln5Mes7").getVal(2);
            var ln6Mes7Base = Number($(".ln6Mes7").data("valor"));
            $(".ln6Mes7").setVal((ln6Mes7Base * (ln5Mes7 / 100)));

            var ln5Mes8 = $(".ln5Mes8").getVal(2);
            var ln6Mes8Base = Number($(".ln6Mes8").data("valor"));
            $(".ln6Mes8").setVal((ln6Mes8Base * (ln5Mes8 / 100)));

            var ln5Mes9 = $(".ln5Mes9").getVal(2);
            var ln6Mes9Base = Number($(".ln6Mes9").data("valor"));
            $(".ln6Mes9").setVal((ln6Mes9Base * (ln5Mes9 / 100)));

            var ln5Mes10 = $(".ln5Mes10").getVal(2);
            var ln6Mes10Base = Number($(".ln6Mes10").data("valor"));
            $(".ln6Mes10").setVal((ln6Mes10Base * (ln5Mes10 / 100)));

            var ln5Mes11 = $(".ln5Mes11").getVal(2);
            var ln6Mes11Base = Number($(".ln6Mes11").data("valor"));
            $(".ln6Mes11").setVal((ln6Mes11Base * (ln5Mes11 / 100)));

            var ln5Mes12 = $(".ln5Mes12").getVal(2);
            var ln6Mes12Base = Number($(".ln6Mes12").data("valor"));
            $(".ln6Mes12").setVal((ln6Mes12Base * (ln5Mes12 / 100)));

            $(".ln5MesT").setVal(ln5Mes1 + ln5Mes2 + ln5Mes3 + ln5Mes4 + ln5Mes5 + ln5Mes6 + ln5Mes7 + ln5Mes8 + ln5Mes9 + ln5Mes10 + ln5Mes11 + ln5Mes12);
            $(".ln6MesT").setVal($(".ln6Mes1").getVal(2) + $(".ln6Mes2").getVal(2) + $(".ln6Mes3").getVal(2) + $(".ln6Mes4").getVal(2) + $(".ln6Mes5").getVal(2) + $(".ln6Mes6").getVal(2) + $(".ln6Mes7").getVal(2) + $(".ln6Mes8").getVal(2) + $(".ln6Mes9").getVal(2) + $(".ln6Mes10").getVal(2) + $(".ln6Mes11").getVal(2) + $(".ln6Mes12").getVal(2));

            //Tasa Anual

            var lnPuntosAdic = $(".tdPuntosAdic").getVal(2);
            $.ajax({
                url: '/Proyecciones/getPDLN7',
                type: 'POST',
                dataType: 'json',
                data: { valor: o.PuntosAdic, objFiltro: objFiltro, col: col },
                success: function (response) {
                    var r = response.r;
                    $(".ln7Mes1").setVal(r.Mes1);
                    $(".ln7Mes2").setVal(r.Mes2);
                    $(".ln7Mes3").setVal(r.Mes3);
                    $(".ln7Mes4").setVal(r.Mes4);
                    $(".ln7Mes5").setVal(r.Mes5);
                    $(".ln7Mes6").setVal(r.Mes6);
                    $(".ln7Mes7").setVal(r.Mes7);
                    $(".ln7Mes8").setVal(r.Mes8);
                    $(".ln7Mes9").setVal(r.Mes9);
                    $(".ln7Mes10").setVal(r.Mes10);
                    $(".ln7Mes11").setVal(r.Mes11);
                    $(".ln7Mes12").setVal(r.Mes12);
                }
            });

            //Importe a amortizar acreedores
            $.ajax({
                url: '/Proyecciones/getPDLN13',
                type: 'POST',
                dataType: 'json',
                data: { ln12: ln12, ln13: ln13, objFiltro: objFiltro, col: col },
                success: function (response) {
                    var r = response.r;
                    $(".ln12MesT").setVal(ln12.Mes1 + ln12.Mes2 + ln12.Mes3 + ln12.Mes4 + ln12.Mes5 + ln12.Mes6 + ln12.Mes7 + ln12.Mes8 + ln12.Mes9 + ln12.Mes10 + ln12.Mes11 + ln12.Mes12);
                    $(".ln13Mes1").setVal(r.Mes1);
                    $(".ln13Mes2").setVal(r.Mes2);
                    $(".ln13Mes3").setVal(r.Mes3);
                    $(".ln13Mes4").setVal(r.Mes4);
                    $(".ln13Mes5").setVal(r.Mes5);
                    $(".ln13Mes6").setVal(r.Mes6);
                    $(".ln13Mes7").setVal(r.Mes7);
                    $(".ln13Mes8").setVal(r.Mes8);
                    $(".ln13Mes9").setVal(r.Mes9);
                    $(".ln13Mes10").setVal(r.Mes10);
                    $(".ln13Mes11").setVal(r.Mes11);
                    $(".ln13Mes12").setVal(r.Mes12);
                    $(".ln13MesT").setVal(r.MesT);
                }
            });


            fnReloadCustomDecialEvent();

        }
        function valid() {
            var state = true;
            //if (!txtModaldescripcion.valid()) { state = false; }
            return state;
        }

        function saveOrUpdate(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            var objFiltro = getFGB();

            $.ajax({
                url: '/Proyecciones/GuardarPagosDiv',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ objFiltro: objFiltro, obj: obj }),
                success: function (response) {
                    if (response.success === true) {
                        AlertaGeneral("Confirmación", "¡Datos guardados correctamente!");
                    } else {
                        AlertaGeneral("Alerta", "¡Ocurrio un problema, porfavor intente de nuevo!");
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function resetFiltros() {

        }
        function setMeses(lista) {
            $.each(lista, function (i, e) {
                $(".thMes" + (i + 1)).html(e);
            });
        }
        init();

    };

    $(document).ready(function () {
        administrativo.proyecciones.pagosdiv = new pagosdiv();
    });
})();


