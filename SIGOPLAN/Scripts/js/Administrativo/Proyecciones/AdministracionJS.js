(function () {

    $.namespace('administrativo.proyecciones.administracion');

    administracion = function () {
        btnAdministracionGuardar = $("#btnAdministracionGuardar"),
        inputCalculos = $(".calculo"),
        txtMesesAbarcados = $("#txtMesesAbarcados"),
        txtGAMAT = $("#txtGAMAT"),
        txtGAMAOBR = $("#txtGAMAOBR"),
        txtGASEGEN = $("#txtGASEGEN"),
        txtGACONDGEN = $("#txtGACONDGEN"),
        txtGASUBCONT = $("#txtGASUBCONT"),
        txtGARENTMAQ = $("#txtGARENTMAQ"),
        txtGAFLETE = $("#txtGAFLETE"),
        tdGAD = $("#tdGAD"),
        txtGAMATP = $("#txtGAMATP"),
        txtGAMAOBRP = $("#txtGAMAOBRP"),
        txtGASEGENP = $("#txtGASEGENP"),
        txtGACONDGENP = $("#txtGACONDGENP"),
        txtGASUBCONTP = $("#txtGASUBCONTP"),
        txtGARENTMAQP = $("#txtGARENTMAQP"),
        txtGAFLETEP = $("#txtGAFLETEP"),
        tdGAP = $("#tdGAP"),
        txtGAMATT = $("#txtGAMATT"),
        txtGAMAOBRT = $("#txtGAMAOBRT"),
        txtGASEGENT = $("#txtGASEGENT"),
        txtGACONDGENT = $("#txtGACONDGENT"),
        txtGASUBCONTT = $("#txtGASUBCONTT"),
        txtGARENTMAQT = $("#txtGARENTMAQT"),
        txtGAFLETET = $("#txtGAFLETET"),
        tdGAT = $("#tdGAT"),

        txtGMMAT = $("#txtGMMAT"),
        txtGMMAOBR = $("#txtGMMAOBR"),
        txtGMSEGEN = $("#txtGMSEGEN"),
        txtGMCONDGEN = $("#txtGMCONDGEN"),
        txtGMSUBCONT = $("#txtGMSUBCONT"),
        txtGMRENTMAQ = $("#txtGMRENTMAQ"),
        txtGMFLETE = $("#txtGMFLETE"),
        tdGMD = $("#tdGMD"),
        txtGMMATP = $("#txtGMMATP"),
        txtGMMAOBRP = $("#txtGMMAOBRP"),
        txtGMSEGENP = $("#txtGMSEGENP"),
        txtGMCONDGENP = $("#txtGMCONDGENP"),
        txtGMSUBCONTP = $("#txtGMSUBCONTP"),
        txtGMRENTMAQP = $("#txtGMRENTMAQP"),
        txtGMFLETEP = $("#txtGMFLETEP"),
        tdGMP = $("#tdGMP"),
        txtGMMATT = $("#txtGMMATT"),
        txtGMMAOBRT = $("#txtGMMAOBRT"),
        txtGMSEGENT = $("#txtGMSEGENT"),
        txtGMCONDGENT = $("#txtGMCONDGENT"),
        txtGMSUBCONTT = $("#txtGMSUBCONTT"),
        txtGMRENTMAQT = $("#txtGMRENTMAQT"),
        txtGMFLETET = $("#txtGMFLETET"),
        tdGMT = $("#tdGMT"),

        txtGTMAT = $("#txtGTMAT"),
        txtGTMAOBR = $("#txtGTMAOBR"),
        txtGTSEGEN = $("#txtGTSEGEN"),
        txtGTCONDGEN = $("#txtGTCONDGEN"),
        txtGTSUBCONT = $("#txtGTSUBCONT"),
        txtGTRENTMAQ = $("#txtGTRENTMAQ"),
        txtGTFLETE = $("#txtGTFLETE"),
        tdGTD = $("#tdGTD"),

        txtGPMAT = $("#txtGPMAT"),
        txtGPMAOBR = $("#txtGPMAOBR"),
        txtGPSEGEN = $("#txtGPSEGEN"),
        txtGPCONDGEN = $("#txtGPCONDGEN"),
        txtGPSUBCONT = $("#txtGPSUBCONT"),
        txtGPRENTMAQ = $("#txtGPRENTMAQ"),
        txtGPFLETE = $("#txtGPFLETE"),
        tdGPD = $("#tdGPD"),

        txtDeprec = $("#txtDeprec"),

        tdPMRGAD = $("#tdPMRGAD"),
        tdPMRGMD = $("#tdPMRGMD"),
        tdPMRTD = $("#tdPMRTD"),

        tdPMRGAP = $("#tdPMRGAP"),
        tdPMRGMP = $("#tdPMRGMP"),
        tdPMRTP = $("#tdPMRTP"),

        txtSG_1 = $("#txtSG_1"),
        txtSG_2 = $("#txtSG_2"),
        txtSG_3 = $("#txtSG_3"),
        txtSG_4 = $("#txtSG_4"),
        txtSG_5 = $("#txtSG_5"),
        txtSG_6 = $("#txtSG_6"),
        txtSG_7 = $("#txtSG_7"),
        txtSG_8 = $("#txtSG_8"),
        txtSG_9 = $("#txtSG_9"),
        txtSG_10 = $("#txtSG_10"),
        txtSG_11 = $("#txtSG_11"),
        txtSG_12 = $("#txtSG_12"),
        txtSG_T = $("#txtSG_T"),
        tdSG_1 = $("#tdSG_1"),
        tdSG_2 = $("#tdSG_2"),
        tdSG_3 = $("#tdSG_3"),
        tdSG_4 = $("#tdSG_4"),
        tdSG_5 = $("#tdSG_5"),
        tdSG_6 = $("#tdSG_6"),
        tdSG_7 = $("#tdSG_7"),
        tdSG_8 = $("#tdSG_8"),
        tdSG_9 = $("#tdSG_9"),
        tdSG_10 = $("#tdSG_10"),
        tdSG_11 = $("#tdSG_11"),
        tdSG_12 = $("#tdSG_12"),
        tdSG_T = $("#tdSG_T"),

        txtAD01_1 = $("#txtAD01_1"),
        txtAD01_2 = $("#txtAD01_2"),
        txtAD01_3 = $("#txtAD01_3"),
        txtAD01_4 = $("#txtAD01_4"),
        txtAD01_5 = $("#txtAD01_5"),
        txtAD01_6 = $("#txtAD01_6"),
        txtAD01_7 = $("#txtAD01_7"),
        txtAD01_8 = $("#txtAD01_8"),
        txtAD01_9 = $("#txtAD01_9"),
        txtAD01_10 = $("#txtAD01_10"),
        txtAD01_11 = $("#txtAD01_11"),
        txtAD01_12 = $("#txtAD01_12"),
        txtAD01_T = $("#txtAD01_T"),

        txtAD02_1 = $("#txtAD02_1"),
        txtAD02_2 = $("#txtAD02_2"),
        txtAD02_3 = $("#txtAD02_3"),
        txtAD02_4 = $("#txtAD02_4"),
        txtAD02_5 = $("#txtAD02_5"),
        txtAD02_6 = $("#txtAD02_6"),
        txtAD02_7 = $("#txtAD02_7"),
        txtAD02_8 = $("#txtAD02_8"),
        txtAD02_9 = $("#txtAD02_9"),
        txtAD02_10 = $("#txtAD02_10"),
        txtAD02_11 = $("#txtAD02_11"),
        txtAD02_12 = $("#txtAD02_12"),
        txtAD02_T = $("#txtAD02_T"),

        txtAD03_1 = $("#txtAD03_1"),
        txtAD03_2 = $("#txtAD03_2"),
        txtAD03_3 = $("#txtAD03_3"),
        txtAD03_4 = $("#txtAD03_4"),
        txtAD03_5 = $("#txtAD03_5"),
        txtAD03_6 = $("#txtAD03_6"),
        txtAD03_7 = $("#txtAD03_7"),
        txtAD03_8 = $("#txtAD03_8"),
        txtAD03_9 = $("#txtAD03_9"),
        txtAD03_10 = $("#txtAD03_10"),
        txtAD03_11 = $("#txtAD03_11"),
        txtAD03_12 = $("#txtAD03_12"),
        txtAD03_T = $("#txtAD03_T"),

        tdADT1 = $("#tdADT1"),
        tdADT2 = $("#tdADT2"),
        tdADT3 = $("#tdADT3"),
        tdADT4 = $("#tdADT4"),
        tdADT5 = $("#tdADT5"),
        tdADT6 = $("#tdADT6"),
        tdADT7 = $("#tdADT7"),
        tdADT8 = $("#tdADT8"),
        tdADT9 = $("#tdADT9"),
        tdADT10 = $("#tdADT10"),
        tdADT11 = $("#tdADT11"),
        tdADT12 = $("#tdADT12"),
        btnCargarInfo = $("#btnCargarInfo"),
        filtroGeneral = $(".filtroGeneral");
        function init() {
            setMeses(GetPeriodoMeses($("#cboPeriodo"), $("#tbMesesInicio")));
            //filtroGeneral.change(function () {
            //   // clickBuscar();
            //    setMeses(GetPeriodoMeses($("#cboPeriodo"), $("#tbMesesInicio")));
            //});
            btnCargarInfo.click(BuscarInfoData);
            txtMesesAbarcados.DecimalFixNS(0, 1);
            //GASTOS ADMON
            txtGAMAT.DecimalFixNS(2);
            txtGAMAOBR.DecimalFixNS(2);
            txtGASEGEN.DecimalFixNS(2);
            txtGACONDGEN.DecimalFixNS(2);
            txtGASUBCONT.DecimalFixNS(2);
            txtGARENTMAQ.DecimalFixNS(2);
            txtGAFLETE.DecimalFixNS(2);
            tdGAD.DecimalFixPs(2);

            txtGAMATP.DecimalFixNS(2);
            txtGAMAOBRP.DecimalFixNS(2);
            txtGASEGENP.DecimalFixNS(2);
            txtGACONDGENP.DecimalFixNS(2);
            txtGASUBCONTP.DecimalFixNS(2);
            txtGARENTMAQP.DecimalFixNS(2);
            txtGAFLETEP.DecimalFixNS(2);
            tdGAP.DecimalFixPr(2);

            txtGAMATT.DecimalFixPs(2);
            txtGAMAOBRT.DecimalFixPs(2);
            txtGASEGENT.DecimalFixPs(2);
            txtGACONDGENT.DecimalFixPs(2);
            txtGASUBCONTT.DecimalFixPs(2);
            txtGARENTMAQT.DecimalFixPs(2);
            txtGAFLETET.DecimalFixPs(2);
            tdGAT.DecimalFixPs(2);

            //GASTOS MAQ
            txtGMMAT.DecimalFixNS(2);
            txtGMMAOBR.DecimalFixNS(2);
            txtGMSEGEN.DecimalFixNS(2);
            txtGMCONDGEN.DecimalFixNS(2);
            txtGMSUBCONT.DecimalFixNS(2);
            txtGMRENTMAQ.DecimalFixNS(2);
            txtGMFLETE.DecimalFixNS(2);
            tdGMD.DecimalFixPs(2);

            txtGMMATP.DecimalFixNS(2);
            txtGMMAOBRP.DecimalFixNS(2);
            txtGMSEGENP.DecimalFixNS(2);
            txtGMCONDGENP.DecimalFixNS(2);
            txtGMSUBCONTP.DecimalFixNS(2);
            txtGMRENTMAQP.DecimalFixNS(2);
            txtGMFLETEP.DecimalFixNS(2);
            tdGMP.DecimalFixPr(2);

            txtGMMATT.DecimalFixPs(2);
            txtGMMAOBRT.DecimalFixPs(2);
            txtGMSEGENT.DecimalFixPs(2);
            txtGMCONDGENT.DecimalFixPs(2);
            txtGMSUBCONTT.DecimalFixPs(2);
            txtGMRENTMAQT.DecimalFixPs(2);
            txtGMFLETET.DecimalFixPs(2);
            tdGMT.DecimalFixPs(2);

            //TOTAL
            txtGTMAT.DecimalFixPs(2);
            txtGTMAOBR.DecimalFixPs(2);
            txtGTSEGEN.DecimalFixPs(2);
            txtGTCONDGEN.DecimalFixPs(2);
            txtGTSUBCONT.DecimalFixPs(2);
            txtGTRENTMAQ.DecimalFixPs(2);
            txtGTFLETE.DecimalFixPs(2);
            tdGTD.DecimalFixPs(2);

            //% POND
            txtGPMAT.DecimalFixPr(2);
            txtGPMAOBR.DecimalFixPr(2);
            txtGPSEGEN.DecimalFixPr(2);
            txtGPCONDGEN.DecimalFixPr(2);
            txtGPSUBCONT.DecimalFixPr(2);
            txtGPRENTMAQ.DecimalFixPr(2);
            txtGPFLETE.DecimalFixPr(2);
            tdGPD.DecimalFixPr(2);

            txtDeprec.DecimalFixNS(2);

            tdPMRGAD.DecimalFixPs(2);
            tdPMRGMD.DecimalFixPs(2);
            tdPMRTD.DecimalFixPs(2);

            tdPMRGAP.DecimalFixPr(2);
            tdPMRGMP.DecimalFixPr(2);
            tdPMRTP.DecimalFixPr(2);

            //Valorizacion
            txtSG_1.DecimalFixNS(2);
            txtSG_2.DecimalFixNS(2);
            txtSG_3.DecimalFixNS(2);
            txtSG_4.DecimalFixNS(2);
            txtSG_5.DecimalFixNS(2);
            txtSG_6.DecimalFixNS(2);
            txtSG_7.DecimalFixNS(2);
            txtSG_8.DecimalFixNS(2);
            txtSG_9.DecimalFixNS(2);
            txtSG_10.DecimalFixNS(2);
            txtSG_11.DecimalFixNS(2);
            txtSG_12.DecimalFixNS(2);
            txtSG_T.DecimalFixNS(2);
            tdSG_1.DecimalFixNS(2);
            tdSG_2.DecimalFixNS(2);
            tdSG_3.DecimalFixNS(2);
            tdSG_4.DecimalFixNS(2);
            tdSG_5.DecimalFixNS(2);
            tdSG_6.DecimalFixNS(2);
            tdSG_7.DecimalFixNS(2);
            tdSG_8.DecimalFixNS(2);
            tdSG_9.DecimalFixNS(2);
            tdSG_10.DecimalFixNS(2);
            tdSG_11.DecimalFixNS(2);
            tdSG_12.DecimalFixNS(2);
            tdSG_T.DecimalFixNS(2);

            txtAD01_1.DecimalFixNS(2);
            txtAD01_2.DecimalFixNS(2);
            txtAD01_3.DecimalFixNS(2);
            txtAD01_4.DecimalFixNS(2);
            txtAD01_5.DecimalFixNS(2);
            txtAD01_6.DecimalFixNS(2);
            txtAD01_7.DecimalFixNS(2);
            txtAD01_8.DecimalFixNS(2);
            txtAD01_9.DecimalFixNS(2);
            txtAD01_10.DecimalFixNS(2);
            txtAD01_11.DecimalFixNS(2);
            txtAD01_12.DecimalFixNS(2);
            txtAD01_T.DecimalFixPs(2);

            txtAD02_1.DecimalFixNS(2);
            txtAD02_2.DecimalFixNS(2);
            txtAD02_3.DecimalFixNS(2);
            txtAD02_4.DecimalFixNS(2);
            txtAD02_5.DecimalFixNS(2);
            txtAD02_6.DecimalFixNS(2);
            txtAD02_7.DecimalFixNS(2);
            txtAD02_8.DecimalFixNS(2);
            txtAD02_9.DecimalFixNS(2);
            txtAD02_10.DecimalFixNS(2);
            txtAD02_11.DecimalFixNS(2);
            txtAD02_12.DecimalFixNS(2);
            txtAD02_T.DecimalFixPs(2);

            txtAD03_1.DecimalFixNS(2);
            txtAD03_2.DecimalFixNS(2);
            txtAD03_3.DecimalFixNS(2);
            txtAD03_4.DecimalFixNS(2);
            txtAD03_5.DecimalFixNS(2);
            txtAD03_6.DecimalFixNS(2);
            txtAD03_7.DecimalFixNS(2);
            txtAD03_8.DecimalFixNS(2);
            txtAD03_9.DecimalFixNS(2);
            txtAD03_10.DecimalFixNS(2);
            txtAD03_11.DecimalFixNS(2);
            txtAD03_12.DecimalFixNS(2);
            txtAD03_T.DecimalFixPs(2);

            tdADT1.DecimalFixPs(2);
            tdADT2.DecimalFixPs(2);
            tdADT3.DecimalFixPs(2);
            tdADT4.DecimalFixPs(2);
            tdADT5.DecimalFixPs(2);
            tdADT6.DecimalFixPs(2);
            tdADT7.DecimalFixPs(2);
            tdADT8.DecimalFixPs(2);
            tdADT9.DecimalFixPs(2);
            tdADT10.DecimalFixPs(2);
            tdADT11.DecimalFixPs(2);
            tdADT12.DecimalFixPs(2);

            clickBuscar();
            btnAdministracionGuardar.click(guardar);
            inputCalculos.change(calculos);
        }

        function BuscarInfoData() {
            var mes = GetPeriodoMeses($("#cboPeriodo"), $("#tbMesesInicio"));
            idTituloContainer.text('DETALLE DE GASTOS DE ADMINISTRACION Y VENTAS ' + mes[0]);
            clickBuscar();
            setMeses(GetPeriodoMeses($("#cboPeriodo"), $("#tbMesesInicio")));
        }
        function clickBuscar() {
            var objFiltro = getFGB();
            $.ajax({
                url: '/Proyecciones/GetFillTableAdministracion',
                type: 'POST',
                dataType: 'json',
                data: { objFiltro: objFiltro },
                success: function (response) {
                    if (response.success === true) {
                        var obj = response.obj;
                        setPlainObject(obj);
                    } else {

                    }
                    $.unblockUI();
                },
                error: function (response) {
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
                if (tdADT12.getVal(2) > 0) {
                    saveOrUpdate(getPlainObject());
                }
                else {
                    AlertaGeneral("Alerta", "¡Debe capturar información antes de continuar!");
                }
            }
        }

        function getPlainObject() {
            var obj = {};
            obj.periodo = $("#txtPeriodoReal").getVal(2);
            obj.mesesAbarcados = $("#txtMesesAbarcados").getVal(2);

            obj.gaMateriales = $("#txtGAMAT").getVal(2);
            obj.gaMaterialesPor = $("#txtGAMATP").getVal(2);
            obj.gaManoObra = $("#txtGAMAOBR").getVal(2);
            obj.gaManoObraPor = $("#txtGAMAOBRP").getVal(2);
            obj.gaServicioGeneral = $("#txtGASEGEN").getVal(2);
            obj.gaServicioGeneralPor = $("#txtGASEGENP").getVal(2);
            obj.gaCondicionGeneral = $("#txtGACONDGEN").getVal(2);
            obj.gaCondicionGeneralPor = $("#txtGACONDGENP").getVal(2);
            obj.gaSubcontratos = $("#txtGASUBCONT").getVal(2);
            obj.gaSubcontratosPor = $("#txtGASUBCONTP").getVal(2);
            obj.gaRentaMaq = $("#txtGARENTMAQ").getVal(2);
            obj.gaRentaMaqPor = $("#txtGARENTMAQP").getVal(2);
            obj.gaFletes = $("#txtGAFLETE").getVal(2);
            obj.gaFletesPor = $("#txtGAFLETEP").getVal(2);

            obj.gmMateriales = $("#txtGMMAT").getVal(2);
            obj.gmMaterialesPor = $("#txtGMMATP").getVal(2);
            obj.gmManoObra = $("#txtGMMAOBR").getVal(2);
            obj.gmManoObraPor = $("#txtGMMAOBRP").getVal(2);
            obj.gmServicioGeneral = $("#txtGMSEGEN").getVal(2);
            obj.gmServicioGeneralPor = $("#txtGMSEGENP").getVal(2);
            obj.gmCondicionGeneral = $("#txtGMCONDGEN").getVal(2);
            obj.gmCondicionGeneralPor = $("#txtGMCONDGENP").getVal(2);
            obj.gmSubcontratos = $("#txtGMSUBCONT").getVal(2);
            obj.gmSubcontratosPor = $("#txtGMSUBCONTP").getVal(2);
            obj.gmRentaMaq = $("#txtGMRENTMAQ").getVal(2);
            obj.gmRentaMaqPor = $("#txtGMRENTMAQP").getVal(2);
            obj.gmFletes = $("#txtGMFLETE").getVal(2);
            obj.gmFletesPor = $("#txtGMFLETEP").getVal(2);

            obj.deprec = $("#txtDeprec").getVal(2);

            obj.sgMes1 = $("#txtSG_1").getVal(2);
            obj.sgMes2 = $("#txtSG_2").getVal(2);
            obj.sgMes3 = $("#txtSG_3").getVal(2);
            obj.sgMes4 = $("#txtSG_4").getVal(2);
            obj.sgMes5 = $("#txtSG_5").getVal(2);
            obj.sgMes6 = $("#txtSG_6").getVal(2);
            obj.sgMes7 = $("#txtSG_7").getVal(2);
            obj.sgMes8 = $("#txtSG_8").getVal(2);
            obj.sgMes9 = $("#txtSG_9").getVal(2);
            obj.sgMes10 = $("#txtSG_10").getVal(2);
            obj.sgMes11 = $("#txtSG_11").getVal(2);
            obj.sgMes12 = $("#txtSG_12").getVal(2);

            obj.ad01_Mes1 = $("#txtAD01_1").getVal(2);
            obj.ad01_Mes2 = $("#txtAD01_2").getVal(2);
            obj.ad01_Mes3 = $("#txtAD01_3").getVal(2);
            obj.ad01_Mes4 = $("#txtAD01_4").getVal(2);
            obj.ad01_Mes5 = $("#txtAD01_5").getVal(2);
            obj.ad01_Mes6 = $("#txtAD01_6").getVal(2);
            obj.ad01_Mes7 = $("#txtAD01_7").getVal(2);
            obj.ad01_Mes8 = $("#txtAD01_8").getVal(2);
            obj.ad01_Mes9 = $("#txtAD01_9").getVal(2);
            obj.ad01_Mes10 = $("#txtAD01_10").getVal(2);
            obj.ad01_Mes11 = $("#txtAD01_11").getVal(2);
            obj.ad01_Mes12 = $("#txtAD01_12").getVal(2);

            obj.ad02_Mes1 = $("#txtAD02_1").getVal(2);
            obj.ad02_Mes2 = $("#txtAD02_2").getVal(2);
            obj.ad02_Mes3 = $("#txtAD02_3").getVal(2);
            obj.ad02_Mes4 = $("#txtAD02_4").getVal(2);
            obj.ad02_Mes5 = $("#txtAD02_5").getVal(2);
            obj.ad02_Mes6 = $("#txtAD02_6").getVal(2);
            obj.ad02_Mes7 = $("#txtAD02_7").getVal(2);
            obj.ad02_Mes8 = $("#txtAD02_8").getVal(2);
            obj.ad02_Mes9 = $("#txtAD02_9").getVal(2);
            obj.ad02_Mes10 = $("#txtAD02_10").getVal(2);
            obj.ad02_Mes11 = $("#txtAD02_11").getVal(2);
            obj.ad02_Mes12 = $("#txtAD02_12").getVal(2);

            obj.ad03_Mes1 = $("#txtAD03_1").getVal(2);
            obj.ad03_Mes2 = $("#txtAD03_2").getVal(2);
            obj.ad03_Mes3 = $("#txtAD03_3").getVal(2);
            obj.ad03_Mes4 = $("#txtAD03_4").getVal(2);
            obj.ad03_Mes5 = $("#txtAD03_5").getVal(2);
            obj.ad03_Mes6 = $("#txtAD03_6").getVal(2);
            obj.ad03_Mes7 = $("#txtAD03_7").getVal(2);
            obj.ad03_Mes8 = $("#txtAD03_8").getVal(2);
            obj.ad03_Mes9 = $("#txtAD03_9").getVal(2);
            obj.ad03_Mes10 = $("#txtAD03_10").getVal(2);
            obj.ad03_Mes11 = $("#txtAD03_11").getVal(2);
            obj.ad03_Mes12 = $("#txtAD03_12").getVal(2);

            obj.PMRGAP = $("#tdPMRGAP").getVal(2);

            obj.ADT1 = $("#tdADT1").getVal(2);
            obj.ADT2 = $("#tdADT2").getVal(2);
            obj.ADT3 = $("#tdADT3").getVal(2);
            obj.ADT4 = $("#tdADT4").getVal(2);
            obj.ADT5 = $("#tdADT5").getVal(2);
            obj.ADT6 = $("#tdADT6").getVal(2);
            obj.ADT7 = $("#tdADT7").getVal(2);
            obj.ADT8 = $("#tdADT8").getVal(2);
            obj.ADT9 = $("#tdADT9").getVal(2);
            obj.ADT10 = $("#tdADT10").getVal(2);
            obj.ADT11 = $("#tdADT11").getVal(2);
            obj.ADT12 = $("#tdADT12").getVal(2);

            return obj;
        }
        function setPlainObject(obj) {
            $("#txtPeriodoReal").val(obj.periodo);
            $("#txtMesesAbarcados").val(obj.mesesAbarcados);

            $("#txtGAMAT").val(obj.gaMateriales);
            $("#txtGAMATP").val(obj.gaMaterialesPor);
            $("#txtGAMAOBR").val(obj.gaManoObra);
            $("#txtGAMAOBRP").val(obj.gaManoObraPor);
            $("#txtGASEGEN").val(obj.gaServicioGeneral);
            $("#txtGASEGENP").val(obj.gaServicioGeneralPor);
            $("#txtGACONDGEN").val(obj.gaCondicionGeneral);
            $("#txtGACONDGENP").val(obj.gaCondicionGeneralPor);
            $("#txtGASUBCONT").val(obj.gaSubcontratos);
            $("#txtGASUBCONTP").val(obj.gaSubcontratosPor);
            $("#txtGARENTMAQ").val(obj.gaRentaMaq);
            $("#txtGARENTMAQP").val(obj.gaRentaMaqPor);
            $("#txtGAFLETE").val(obj.gaFletes);
            $("#txtGAFLETEP").val(obj.gaFletesPor);

            $("#txtGMMAT").val(obj.gmMateriales);
            $("#txtGMMATP").val(obj.gmMaterialesPor);
            $("#txtGMMAOBR").val(obj.gmManoObra);
            $("#txtGMMAOBRP").val(obj.gmManoObraPor);
            $("#txtGMSEGEN").val(obj.gmServicioGeneral);
            $("#txtGMSEGENP").val(obj.gmServicioGeneralPor);
            $("#txtGMCONDGEN").val(obj.gmCondicionGeneral);
            $("#txtGMCONDGENP").val(obj.gmCondicionGeneralPor);
            $("#txtGMSUBCONT").val(obj.gmSubcontratos);
            $("#txtGMSUBCONTP").val(obj.gmSubcontratosPor);
            $("#txtGMRENTMAQ").val(obj.gmRentaMaq);
            $("#txtGMRENTMAQP").val(obj.gmRentaMaqPor);
            $("#txtGMFLETE").val(obj.gmFletes);
            $("#txtGMFLETEP").val(obj.gmFletesPor);

            $("#txtDeprec").val(obj.deprec);

            $("#txtSG_1").val(obj.sgMes1);
            $("#txtSG_2").val(obj.sgMes2);
            $("#txtSG_3").val(obj.sgMes3);
            $("#txtSG_4").val(obj.sgMes4);
            $("#txtSG_5").val(obj.sgMes5);
            $("#txtSG_6").val(obj.sgMes6);
            $("#txtSG_7").val(obj.sgMes7);
            $("#txtSG_8").val(obj.sgMes8);
            $("#txtSG_9").val(obj.sgMes9);
            $("#txtSG_10").val(obj.sgMes10);
            $("#txtSG_11").val(obj.sgMes11);
            $("#txtSG_12").val(obj.sgMes12);

            $("#txtAD01_1").val(obj.ad01_Mes1);
            $("#txtAD01_2").val(obj.ad01_Mes2);
            $("#txtAD01_3").val(obj.ad01_Mes3);
            $("#txtAD01_4").val(obj.ad01_Mes4);
            $("#txtAD01_5").val(obj.ad01_Mes5);
            $("#txtAD01_6").val(obj.ad01_Mes6);
            $("#txtAD01_7").val(obj.ad01_Mes7);
            $("#txtAD01_8").val(obj.ad01_Mes8);
            $("#txtAD01_9").val(obj.ad01_Mes9);
            $("#txtAD01_10").val(obj.ad01_Mes10);
            $("#txtAD01_11").val(obj.ad01_Mes11);
            $("#txtAD01_12").val(obj.ad01_Mes12);

            $("#txtAD02_1").val(obj.ad02_Mes1);
            $("#txtAD02_2").val(obj.ad02_Mes2);
            $("#txtAD02_3").val(obj.ad02_Mes3);
            $("#txtAD02_4").val(obj.ad02_Mes4);
            $("#txtAD02_5").val(obj.ad02_Mes5);
            $("#txtAD02_6").val(obj.ad02_Mes6);
            $("#txtAD02_7").val(obj.ad02_Mes7);
            $("#txtAD02_8").val(obj.ad02_Mes8);
            $("#txtAD02_9").val(obj.ad02_Mes9);
            $("#txtAD02_10").val(obj.ad02_Mes10);
            $("#txtAD02_11").val(obj.ad02_Mes11);
            $("#txtAD02_12").val(obj.ad02_Mes12);

            $("#txtAD03_1").val(obj.ad03_Mes1);
            $("#txtAD03_2").val(obj.ad03_Mes2);
            $("#txtAD03_3").val(obj.ad03_Mes3);
            $("#txtAD03_4").val(obj.ad03_Mes4);
            $("#txtAD03_5").val(obj.ad03_Mes5);
            $("#txtAD03_6").val(obj.ad03_Mes6);
            $("#txtAD03_7").val(obj.ad03_Mes7);
            $("#txtAD03_8").val(obj.ad03_Mes8);
            $("#txtAD03_9").val(obj.ad03_Mes9);
            $("#txtAD03_10").val(obj.ad03_Mes10);
            $("#txtAD03_11").val(obj.ad03_Mes11);
            $("#txtAD03_12").val(obj.ad03_Mes12);
            calculos();
        }
        function calculos() {
            var o = getPlainObject();
            txtGAMATT.val(Math.round(Number(o.gaMateriales) + (Number(o.gaMateriales) * (Number(o.gaMaterialesPor) / 100))));
            txtGAMAOBRT.val(Math.round(Number(o.gaManoObra) + (Number(o.gaManoObra) * (Number(o.gaManoObraPor) / 100))));
            txtGASEGENT.val(Math.round(Number(o.gaServicioGeneral) + (Number(o.gaServicioGeneral) * (Number(o.gaServicioGeneralPor) / 100))));
            txtGACONDGENT.val(Math.round(Number(o.gaCondicionGeneral) + (Number(o.gaCondicionGeneral) * (Number(o.gaCondicionGeneralPor) / 100))));
            txtGASUBCONTT.val(Math.round(Number(o.gaSubcontratos) + (Number(o.gaSubcontratos) * (Number(o.gaSubcontratosPor) / 100))));
            txtGARENTMAQT.val(Math.round(Number(o.gaRentaMaq) + (Number(o.gaRentaMaq) * (Number(o.gaRentaMaqPor) / 100))));
            txtGAFLETET.val(Math.round(Number(o.gaFletes) + (Number(o.gaFletes) * (Number(o.gaFletesPor) / 100))));

            txtGMMATT.val(Math.round(Number(o.gmMateriales) + (Number(o.gmMateriales) * (Number(o.gmMaterialesPor) / 100))));
            txtGMMAOBRT.val(Math.round(Number(o.gmManoObra) + (Number(o.gmManoObra) * (Number(o.gmManoObraPor) / 100))));
            txtGMSEGENT.val(Math.round(Number(o.gmServicioGeneral) + (Number(o.gmServicioGeneral) * (Number(o.gmServicioGeneralPor) / 100))));
            txtGMCONDGENT.val(Math.round(Number(o.gmCondicionGeneral) + (Number(o.gmCondicionGeneral) * (Number(o.gmCondicionGeneralPor) / 100))));
            txtGMSUBCONTT.val(Math.round(Number(o.gmSubcontratos) + (Number(o.gmSubcontratos) * (Number(o.gmSubcontratosPor) / 100))));
            txtGMRENTMAQT.val(Math.round(Number(o.gmRentaMaq) + (Number(o.gmRentaMaq) * (Number(o.gmRentaMaqPor) / 100))));
            txtGMFLETET.val(Math.round(Number(o.gmFletes) + (Number(o.gmFletes) * (Number(o.gmFletesPor) / 100))));

            txtGTMAT.val(Number(txtGAMATT.getVal(2)) + Number(txtGMMATT.getVal(2)));
            txtGTMAOBR.val(Number(txtGAMAOBRT.getVal(2)) + Number(txtGMMAOBRT.getVal(2)));
            txtGTSEGEN.val(Number(txtGASEGENT.getVal(2)) + Number(txtGMSEGENT.getVal(2)));
            txtGTCONDGEN.val(Number(txtGACONDGENT.getVal(2)) + Number(txtGMCONDGENT.getVal(2)));
            txtGTSUBCONT.val(Number(txtGASUBCONTT.getVal(2)) + Number(txtGMSUBCONTT.getVal(2)));
            txtGTRENTMAQ.val(Number(txtGARENTMAQT.getVal(2)) + Number(txtGMRENTMAQT.getVal(2)));
            txtGTFLETE.val(Number(txtGAFLETET.getVal(2)) + Number(txtGMFLETET.getVal(2)));


            var GAT = Math.round(Number(o.gaMateriales) + Number(o.gaManoObra) + Number(o.gaServicioGeneral) + Number(o.gaCondicionGeneral) + Number(o.gaSubcontratos) + Number(o.gaRentaMaq) + Number(o.gaFletes));
            var GAP = Math.round(Number(o.gaMaterialesPor) + Number(o.gaManoObraPor) + Number(o.gaServicioGeneralPor) + Number(o.gaCondicionGeneralPor) + Number(o.gaSubcontratosPor) + Number(o.gaRentaMaqPor) + Number(o.gaFletesPor));
            var GATT = Math.round(Number(txtGAMATT.getVal(2)) + Number(txtGAMAOBRT.getVal(2)) + Number(txtGASEGENT.getVal(2)) + Number(txtGACONDGENT.getVal(2)) + Number(txtGASUBCONTT.getVal(2)) + Number(txtGARENTMAQT.getVal(2)) + Number(txtGAFLETET.getVal(2)));
            var GMT = Math.round(Number(o.gmMateriales) + Number(o.gmManoObra) + Number(o.gmServicioGeneral) + Number(o.gmCondicionGeneral) + Number(o.gmSubcontratos) + Number(o.gmRentaMaq) + Number(o.gmFletes));
            var GMP = Math.round(Number(o.gmMaterialesPor) + Number(o.gmManoObraPor) + Number(o.gmServicioGeneralPor) + Number(o.gmCondicionGeneralPor) + Number(o.gmSubcontratosPor) + Number(o.gmRentaMaqPor) + Number(o.gmFletesPor));
            var GMTT = Math.round(Number(txtGMMATT.getVal(2)) + Number(txtGMMAOBRT.getVal(2)) + Number(txtGMSEGENT.getVal(2)) + Number(txtGMCONDGENT.getVal(2)) + Number(txtGMSUBCONTT.getVal(2)) + Number(txtGMRENTMAQT.getVal(2)) + Number(txtGMFLETET.getVal(2)));
            var GTTotal = Math.round(Number(txtGTMAT.getVal(2)) + Number(txtGTMAOBR.getVal(2)) + Number(txtGTSEGEN.getVal(2)) + Number(txtGTCONDGEN.getVal(2)) + Number(txtGTSUBCONT.getVal(2)) + Number(txtGTRENTMAQ.getVal(2)) + Number(txtGTFLETE.getVal(2)));
            var GPTotal = 100;
            tdGAD.html(GAT);
            tdGAP.html(GAP);
            tdGAT.html(GATT);
            tdGMD.html(GMT);
            tdGMP.html(GMP);
            tdGMT.html(GMTT);
            tdGTD.html(GTTotal);
            tdGPD.html(GPTotal);

            txtGPMAT.val(Number((txtGTMAT.getVal(2) / GTTotal)) * 100);
            txtGPMAOBR.val(Number((txtGTMAOBR.getVal(2) / GTTotal)) * 100);
            txtGPSEGEN.val(Number((txtGTSEGEN.getVal(2) / GTTotal)) * 100);
            txtGPCONDGEN.val(Number((txtGTCONDGEN.getVal(2) / GTTotal)) * 100);
            txtGPSUBCONT.val(Number((txtGTSUBCONT.getVal(2) / GTTotal)) * 100);
            txtGPRENTMAQ.val(Number((txtGTRENTMAQ.getVal(2) / GTTotal)) * 100);
            txtGPFLETE.val(Number((txtGTFLETE.getVal(2) / GTTotal)) * 100);

            tdPMRGAD.html(tdGAT.getVal(2));
            tdPMRGMD.html(GMTT / txtMesesAbarcados.getVal(0));
            tdPMRTD.html(tdPMRGAD.getVal(2) + tdPMRGMD.getVal(2));

            tdPMRGAP.html((tdPMRGAD.getVal(2) / tdPMRTD.getVal(2)) * 100);
            tdPMRGMP.html((tdPMRGMD.getVal(2) / tdPMRTD.getVal(2)) * 100);
            //tdPMRTP.html(tdPMRGAP.getVal(2)+tdPMRGMP.getVal(2));

            //Valorizacion
            tdSG_1.html(tdPMRTD.getVal(2) + (tdPMRTD.getVal(2) * (txtSG_1.getVal(2) / 100)));
            tdSG_2.html(tdSG_1.getVal(2) + (tdSG_1.getVal(2) * (txtSG_2.getVal(2) / 100)));
            tdSG_3.html(tdSG_2.getVal(2) + (tdSG_2.getVal(2) * (txtSG_3.getVal(2) / 100)));
            tdSG_4.html(tdSG_3.getVal(2) + (tdSG_3.getVal(2) * (txtSG_4.getVal(2) / 100)));
            tdSG_5.html(tdSG_4.getVal(2) + (tdSG_4.getVal(2) * (txtSG_5.getVal(2) / 100)));
            tdSG_6.html(tdSG_5.getVal(2) + (tdSG_5.getVal(2) * (txtSG_6.getVal(2) / 100)));
            tdSG_7.html(tdSG_6.getVal(2) + (tdSG_6.getVal(2) * (txtSG_7.getVal(2) / 100)));
            tdSG_8.html(tdSG_7.getVal(2) + (tdSG_7.getVal(2) * (txtSG_8.getVal(2) / 100)));
            tdSG_9.html(tdSG_8.getVal(2) + (tdSG_8.getVal(2) * (txtSG_9.getVal(2) / 100)));
            tdSG_10.html(tdSG_9.getVal(2) + (tdSG_9.getVal(2) * (txtSG_10.getVal(2) / 100)));
            tdSG_11.html(tdSG_10.getVal(2) + (tdSG_10.getVal(2) * (txtSG_11.getVal(2) / 100)));
            tdSG_12.html(tdSG_11.getVal(2) + (tdSG_11.getVal(2) * (txtSG_12.getVal(2) / 100)));
            txtSG_T.html(txtSG_1.getVal(2) + txtSG_2.getVal(2) + txtSG_3.getVal(2) + txtSG_4.getVal(2) + txtSG_5.getVal(2) + txtSG_6.getVal(2) + txtSG_7.getVal(2) + txtSG_8.getVal(2) + txtSG_9.getVal(2) + txtSG_10.getVal(2) + txtSG_11.getVal(2) + txtSG_12.getVal(12));
            tdSG_T.html(tdSG_1.getVal(2) + tdSG_2.getVal(2) + tdSG_3.getVal(2) + tdSG_4.getVal(2) + tdSG_5.getVal(2) + tdSG_6.getVal(2) + tdSG_7.getVal(2) + tdSG_8.getVal(2) + tdSG_9.getVal(2) + tdSG_10.getVal(2) + tdSG_11.getVal(2) + tdSG_12.getVal(12));

            txtAD01_T.val(Math.round(txtAD01_1.getVal(2) + txtAD01_2.getVal(2) + txtAD01_3.getVal(2) + txtAD01_4.getVal(2) + txtAD01_5.getVal(2) + txtAD01_6.getVal(2) + txtAD01_7.getVal(2) + txtAD01_8.getVal(2) + txtAD01_9.getVal(2) + txtAD01_10.getVal(2) + txtAD01_11.getVal(2) + txtAD01_12.getVal(12)));
            txtAD02_T.val(Math.round(txtAD02_1.getVal(2) + txtAD02_2.getVal(2) + txtAD02_3.getVal(2) + txtAD02_4.getVal(2) + txtAD02_5.getVal(2) + txtAD02_6.getVal(2) + txtAD02_7.getVal(2) + txtAD02_8.getVal(2) + txtAD02_9.getVal(2) + txtAD02_10.getVal(2) + txtAD02_11.getVal(2) + txtAD02_12.getVal(12)));
            txtAD03_T.val(Math.round(txtAD03_1.getVal(2) + txtAD03_2.getVal(2) + txtAD03_3.getVal(2) + txtAD03_4.getVal(2) + txtAD03_5.getVal(2) + txtAD03_6.getVal(2) + txtAD03_7.getVal(2) + txtAD03_8.getVal(2) + txtAD03_9.getVal(2) + txtAD03_10.getVal(2) + txtAD03_11.getVal(2) + txtAD03_12.getVal(12)));

            tdADT1.html(Math.round(tdSG_1.getVal(2) + txtAD01_1.getVal(2) + txtAD02_1.getVal(2) + txtAD03_1.getVal(2)));
            tdADT2.html(Math.round(tdSG_2.getVal(2) + txtAD01_2.getVal(2) + txtAD02_2.getVal(2) + txtAD03_2.getVal(2)));
            tdADT3.html(Math.round(tdSG_3.getVal(2) + txtAD01_3.getVal(2) + txtAD02_3.getVal(2) + txtAD03_3.getVal(2)));
            tdADT4.html(Math.round(tdSG_4.getVal(2) + txtAD01_4.getVal(2) + txtAD02_4.getVal(2) + txtAD03_4.getVal(2)));
            tdADT5.html(Math.round(tdSG_5.getVal(2) + txtAD01_5.getVal(2) + txtAD02_5.getVal(2) + txtAD03_5.getVal(2)));
            tdADT6.html(Math.round(tdSG_6.getVal(2) + txtAD01_6.getVal(2) + txtAD02_6.getVal(2) + txtAD03_6.getVal(2)));
            tdADT7.html(Math.round(tdSG_7.getVal(2) + txtAD01_7.getVal(2) + txtAD02_7.getVal(2) + txtAD03_7.getVal(2)));
            tdADT8.html(Math.round(tdSG_8.getVal(2) + txtAD01_8.getVal(2) + txtAD02_8.getVal(2) + txtAD03_8.getVal(2)));
            tdADT9.html(Math.round(tdSG_9.getVal(2) + txtAD01_9.getVal(2) + txtAD02_9.getVal(2) + txtAD03_9.getVal(2)));
            tdADT10.html(Math.round(tdSG_10.getVal(2) + txtAD01_10.getVal(2) + txtAD02_10.getVal(2) + txtAD03_10.getVal(2)));
            tdADT11.html(Math.round(tdSG_11.getVal(2) + txtAD01_11.getVal(2) + txtAD02_11.getVal(2) + txtAD03_11.getVal(2)));
            tdADT12.html(Math.round(tdSG_12.getVal(2) + txtAD01_12.getVal(2) + txtAD02_12.getVal(2) + txtAD03_12.getVal(2)));

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
                url: '/Proyecciones/GuardarAdministracion',
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

        $(document).on('change', '.calculo ', function () {
            var valor = redondear($(this).val());
            $(this).val(valor);
        });

        function redondear(valor) {
            var sinComas = removeCommas(valor);
            var redondeado = Math.round(sinComas);
            var conCommas = addCommas(redondeado.toFixed(2));
            return conCommas;
        }

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

        init();

    };

    $(document).ready(function () {
        administrativo.proyecciones.administracion = new administracion();
    });
})();


