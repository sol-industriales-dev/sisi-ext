(function () {

    $.namespace('administrativo.proyecciones.cobrosdiv');

    cobrosdiv = function () {
        btnCobrosDivGuardar = $("#btnCobrosDivGuardar"),
        inputCalculos = $(".calculo"),
        filtroGeneral = $(".filtroGeneral"),
        txtLn1_Mes1 = $("#txtLn1_Mes1"),
        txtLn1_Mes2 = $("#txtLn1_Mes2"),
        txtLn1_Mes3 = $("#txtLn1_Mes3"),
        txtLn1_Mes4 = $("#txtLn1_Mes4"),
        txtLn1_Mes5 = $("#txtLn1_Mes5"),
        txtLn1_Mes6 = $("#txtLn1_Mes6"),
        txtLn1_Mes7 = $("#txtLn1_Mes7"),
        txtLn1_Mes8 = $("#txtLn1_Mes8"),
        txtLn1_Mes9 = $("#txtLn1_Mes9"),
        txtLn1_Mes10 = $("#txtLn1_Mes10"),
        txtLn1_Mes11 = $("#txtLn1_Mes11"),
        txtLn1_Mes12 = $("#txtLn1_Mes12"),
        txtLn1_MesT = $("#txtLn1_MesT"),
        txtLn2_Mes1 = $("#txtLn2_Mes1"),
        txtLn2_Mes2 = $("#txtLn2_Mes2"),
        txtLn2_Mes3 = $("#txtLn2_Mes3"),
        txtLn2_Mes4 = $("#txtLn2_Mes4"),
        txtLn2_Mes5 = $("#txtLn2_Mes5"),
        txtLn2_Mes6 = $("#txtLn2_Mes6"),
        txtLn2_Mes7 = $("#txtLn2_Mes7"),
        txtLn2_Mes8 = $("#txtLn2_Mes8"),
        txtLn2_Mes9 = $("#txtLn2_Mes9"),
        txtLn2_Mes10 = $("#txtLn2_Mes10"),
        txtLn2_Mes11 = $("#txtLn2_Mes11"),
        txtLn2_Mes12 = $("#txtLn2_Mes12"),
        txtLn2_MesT = $("#txtLn2_MesT"),
        txtLn3_Mes1 = $("#txtLn3_Mes1"),
        txtLn3_Mes2 = $("#txtLn3_Mes2"),
        txtLn3_Mes3 = $("#txtLn3_Mes3"),
        txtLn3_Mes4 = $("#txtLn3_Mes4"),
        txtLn3_Mes5 = $("#txtLn3_Mes5"),
        txtLn3_Mes6 = $("#txtLn3_Mes6"),
        txtLn3_Mes7 = $("#txtLn3_Mes7"),
        txtLn3_Mes8 = $("#txtLn3_Mes8"),
        txtLn3_Mes9 = $("#txtLn3_Mes9"),
        txtLn3_Mes10 = $("#txtLn3_Mes10"),
        txtLn3_Mes11 = $("#txtLn3_Mes11"),
        txtLn3_Mes12 = $("#txtLn3_Mes12"),
        txtLn3_MesT = $("#txtLn3_MesT"),
        txtLn4_Mes1 = $("#txtLn4_Mes1"),
        txtLn4_Mes2 = $("#txtLn4_Mes2"),
        txtLn4_Mes3 = $("#txtLn4_Mes3"),
        txtLn4_Mes4 = $("#txtLn4_Mes4"),
        txtLn4_Mes5 = $("#txtLn4_Mes5"),
        txtLn4_Mes6 = $("#txtLn4_Mes6"),
        txtLn4_Mes7 = $("#txtLn4_Mes7"),
        txtLn4_Mes8 = $("#txtLn4_Mes8"),
        txtLn4_Mes9 = $("#txtLn4_Mes9"),
        txtLn4_Mes10 = $("#txtLn4_Mes10"),
        txtLn4_Mes11 = $("#txtLn4_Mes11"),
        txtLn4_Mes12 = $("#txtLn4_Mes12"),
        txtLn4_MesT = $("#txtLn4_MesT"),
        txtLn5_Mes2 = $("#txtLn5_Mes2"),
        txtLn5_Mes3 = $("#txtLn5_Mes3"),
        txtLn5_Mes4 = $("#txtLn5_Mes4"),
        txtLn5_Mes5 = $("#txtLn5_Mes5"),
        txtLn5_Mes6 = $("#txtLn5_Mes6"),
        txtLn5_Mes7 = $("#txtLn5_Mes7"),
        txtLn5_Mes8 = $("#txtLn5_Mes8"),
        txtLn5_Mes9 = $("#txtLn5_Mes9"),
        txtLn5_Mes10 = $("#txtLn5_Mes10"),
        txtLn5_Mes11 = $("#txtLn5_Mes11"),
        txtLn5_Mes12 = $("#txtLn5_Mes12"),
        txtLn5_MesT = $("#txtLn5_MesT"),
        txtLn6_Mes1 = $("#txtLn6_Mes1"),
        txtLn6_Mes2 = $("#txtLn6_Mes2"),
        txtLn6_Mes3 = $("#txtLn6_Mes3"),
        txtLn6_Mes4 = $("#txtLn6_Mes4"),
        txtLn6_Mes5 = $("#txtLn6_Mes5"),
        txtLn6_Mes6 = $("#txtLn6_Mes6"),
        txtLn6_Mes7 = $("#txtLn6_Mes7"),
        txtLn6_Mes8 = $("#txtLn6_Mes8"),
        txtLn6_Mes9 = $("#txtLn6_Mes9"),
        txtLn6_Mes10 = $("#txtLn6_Mes10"),
        txtLn6_Mes11 = $("#txtLn6_Mes11"),
        txtLn6_Mes12 = $("#txtLn6_Mes12"),
        txtLn6_MesT = $("#txtLn6_MesT"),
        clsPorcentaje = $(".clsPorcentaje"),
        clsPesos = $(".clsPesos");
        function init() {
            setMeses(GetPeriodoMeses($("#cboPeriodo"), $("#tbMesesInicio")));
            $.each(clsPorcentaje, function (i, e) {
                $(e).DecimalFixNS(2);
            });
            $.each(clsPesos, function (i, e) {
                $(e).DecimalFixPs(0);
            });

            clickBuscar();
            $("#btnCargarInfo").unbind("click");
            $("#btnCargarInfo").click(function () {
                var mes = GetPeriodoMeses($("#cboPeriodo"), $("#tbMesesInicio"));
                idTituloContainer.text('Ingresos Diversos ' + mes[0]);
                clickBuscar();
                setMeses(GetPeriodoMeses($("#cboPeriodo"), $("#tbMesesInicio")));
            });

            btnCobrosDivGuardar.click(guardar);
            inputCalculos.change(calculos);
            
        }

        function clickBuscar() {
            var objFiltro = getFGB();
            $.ajax({
                url: '/Proyecciones/GetFillTableCobrosDiv',
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
                saveOrUpdate(getPlainObject());
            }
        }

        function getPlainObject() {
            var obj = {};
            var Ln1 = {};
            Ln1.Mes1 = txtLn1_Mes1.getVal(2);
            Ln1.Mes2 = txtLn1_Mes2.getVal(2);
            Ln1.Mes3 = txtLn1_Mes3.getVal(2);
            Ln1.Mes4 = txtLn1_Mes4.getVal(2);
            Ln1.Mes5 = txtLn1_Mes5.getVal(2);
            Ln1.Mes6 = txtLn1_Mes6.getVal(2);
            Ln1.Mes7 = txtLn1_Mes7.getVal(2);
            Ln1.Mes8 = txtLn1_Mes8.getVal(2);
            Ln1.Mes9 = txtLn1_Mes9.getVal(2);
            Ln1.Mes10 = txtLn1_Mes10.getVal(2);
            Ln1.Mes11 = txtLn1_Mes11.getVal(2);
            Ln1.Mes12 = txtLn1_Mes12.getVal(2);
            Ln1.MesT = txtLn1_MesT.getVal(2);

            var Ln2 = {};
            Ln2.Mes1 = txtLn2_Mes1.getVal(0);
            Ln2.Mes2 = txtLn2_Mes2.getVal(0);
            Ln2.Mes3 = txtLn2_Mes3.getVal(0);
            Ln2.Mes4 = txtLn2_Mes4.getVal(0);
            Ln2.Mes5 = txtLn2_Mes5.getVal(0);
            Ln2.Mes6 = txtLn2_Mes6.getVal(0);
            Ln2.Mes7 = txtLn2_Mes7.getVal(0);
            Ln2.Mes8 = txtLn2_Mes8.getVal(0);
            Ln2.Mes9 = txtLn2_Mes9.getVal(0);
            Ln2.Mes10 = txtLn2_Mes10.getVal(0);
            Ln2.Mes11 = txtLn2_Mes11.getVal(0);
            Ln2.Mes12 = txtLn2_Mes12.getVal(0);
            Ln2.MesT = txtLn2_MesT.getVal(0);

            var Ln3 = {};
            Ln3.Mes1 = txtLn3_Mes1.getVal(0);
            Ln3.Mes2 = txtLn3_Mes2.getVal(0);
            Ln3.Mes3 = txtLn3_Mes3.getVal(0);
            Ln3.Mes4 = txtLn3_Mes4.getVal(0);
            Ln3.Mes5 = txtLn3_Mes5.getVal(0);
            Ln3.Mes6 = txtLn3_Mes6.getVal(0);
            Ln3.Mes7 = txtLn3_Mes7.getVal(0);
            Ln3.Mes8 = txtLn3_Mes8.getVal(0);
            Ln3.Mes9 = txtLn3_Mes9.getVal(0);
            Ln3.Mes10 = txtLn3_Mes10.getVal(0);
            Ln3.Mes11 = txtLn3_Mes11.getVal(0);
            Ln3.Mes12 = txtLn3_Mes12.getVal(0);
            Ln3.MesT = txtLn3_MesT.getVal(0);

            var Ln4 = {};
            Ln4.Mes1 = txtLn4_Mes1.getVal(2);
            Ln4.Mes2 = txtLn4_Mes2.getVal(2);
            Ln4.Mes3 = txtLn4_Mes3.getVal(2);
            Ln4.Mes4 = txtLn4_Mes4.getVal(2);
            Ln4.Mes5 = txtLn4_Mes5.getVal(2);
            Ln4.Mes6 = txtLn4_Mes6.getVal(2);
            Ln4.Mes7 = txtLn4_Mes7.getVal(2);
            Ln4.Mes8 = txtLn4_Mes8.getVal(2);
            Ln4.Mes9 = txtLn4_Mes9.getVal(2);
            Ln4.Mes10 = txtLn4_Mes10.getVal(2);
            Ln4.Mes11 = txtLn4_Mes11.getVal(2);
            Ln4.Mes12 = txtLn4_Mes12.getVal(2);
            Ln4.MesT = txtLn4_MesT.getVal(2);

            var Ln5 = {};
            Ln5.Mes1 = 0;
            Ln5.Mes2 = txtLn5_Mes2.getVal(0);
            Ln5.Mes3 = txtLn5_Mes3.getVal(0);
            Ln5.Mes4 = txtLn5_Mes4.getVal(0);
            Ln5.Mes5 = txtLn5_Mes5.getVal(0);
            Ln5.Mes6 = txtLn5_Mes6.getVal(0);
            Ln5.Mes7 = txtLn5_Mes7.getVal(0);
            Ln5.Mes8 = txtLn5_Mes8.getVal(0);
            Ln5.Mes9 = txtLn5_Mes9.getVal(0);
            Ln5.Mes10 = txtLn5_Mes10.getVal(0);
            Ln5.Mes11 = txtLn5_Mes11.getVal(0);
            Ln5.Mes12 = txtLn5_Mes12.getVal(0);
            Ln5.MesT = txtLn5_MesT.getVal(0);

            var Ln6 = {};
            Ln6.Mes1 = txtLn6_Mes1.getVal(2);
            Ln6.Mes2 = txtLn6_Mes2.getVal(2);
            Ln6.Mes3 = txtLn6_Mes3.getVal(2);
            Ln6.Mes4 = txtLn6_Mes4.getVal(2);
            Ln6.Mes5 = txtLn6_Mes5.getVal(2);
            Ln6.Mes6 = txtLn6_Mes6.getVal(2);
            Ln6.Mes7 = txtLn6_Mes7.getVal(2);
            Ln6.Mes8 = txtLn6_Mes8.getVal(2);
            Ln6.Mes9 = txtLn6_Mes9.getVal(2);
            Ln6.Mes10 = txtLn6_Mes10.getVal(2);
            Ln6.Mes11 = txtLn6_Mes11.getVal(2);
            Ln6.Mes12 = txtLn6_Mes12.getVal(2);
            Ln6.MesT = txtLn6_MesT.getVal(2);

            obj.ln1CliPorcentajeSaldoAmortizar=Ln1;
            obj.ln2ImporteAmortizar1=Ln2;
            obj.ln3ImporteAmortizar2=Ln3;
            obj.ln4CxCPorcentajeSaldoAmortizar=Ln4;
            obj.ln5CxCImporteAmortizar=Ln5;
            obj.ln6AporteCapital = Ln6;
         
     
            return obj; 
        }
        function setPlainObject(obj) {
            var Ln1 = {};
            var Ln2 = {};
            var Ln3 = {};
            var Ln4 = {};
            var Ln5 = {};
            var Ln6 = {};
            Ln1 = obj.ln1CliPorcentajeSaldoAmortizar;
            Ln2 = obj.ln2ImporteAmortizar1;
            Ln3 = obj.ln3ImporteAmortizar2;
            Ln4 = obj.ln4CxCPorcentajeSaldoAmortizar;
            Ln5 = obj.ln5CxCImporteAmortizar;
            Ln6 = obj.ln6AporteCapital;

            txtLn1_Mes1.val(Ln1.Mes1);
            txtLn1_Mes2.val(Ln1.Mes2);
            txtLn1_Mes3.val(Ln1.Mes3);
            txtLn1_Mes4.val(Ln1.Mes4);
            txtLn1_Mes5.val(Ln1.Mes5);
            txtLn1_Mes6.val(Ln1.Mes6);
            txtLn1_Mes7.val(Ln1.Mes7);
            txtLn1_Mes8.val(Ln1.Mes8);
            txtLn1_Mes9.val(Ln1.Mes9);
            txtLn1_Mes10.val(Ln1.Mes10);
            txtLn1_Mes11.val(Ln1.Mes11);
            txtLn1_Mes12.val(Ln1.Mes12);
            txtLn1_MesT.val(Ln1.MesT);

            txtLn2_Mes1.html(Ln2.Mes1);
            txtLn2_Mes2.html(Ln2.Mes2);
            txtLn2_Mes3.html(Ln2.Mes3);
            txtLn2_Mes4.html(Ln2.Mes4);
            txtLn2_Mes5.html(Ln2.Mes5);
            txtLn2_Mes6.html(Ln2.Mes6);
            txtLn2_Mes7.html(Ln2.Mes7);
            txtLn2_Mes8.html(Ln2.Mes8);
            txtLn2_Mes9.html(Ln2.Mes9);
            txtLn2_Mes10.html(Ln2.Mes10);
            txtLn2_Mes11.html(Ln2.Mes11);
            txtLn2_Mes12.html(Ln2.Mes12);
            txtLn2_MesT.html(Ln2.MesT);

            txtLn3_Mes1.html(Ln3.Mes1);
            txtLn3_Mes2.html(Ln3.Mes2);
            txtLn3_Mes3.html(Ln3.Mes3);
            txtLn3_Mes4.html(Ln3.Mes4);
            txtLn3_Mes5.html(Ln3.Mes5);
            txtLn3_Mes6.html(Ln3.Mes6);
            txtLn3_Mes7.html(Ln3.Mes7);
            txtLn3_Mes8.html(Ln3.Mes8);
            txtLn3_Mes9.html(Ln3.Mes9);
            txtLn3_Mes10.html(Ln3.Mes10);
            txtLn3_Mes11.html(Ln3.Mes11);
            txtLn3_Mes12.html(Ln3.Mes12);
            txtLn3_MesT.html(Ln3.MesT);

            txtLn4_Mes1.val(Ln4.Mes1);
            txtLn4_Mes2.val(Ln4.Mes2);
            txtLn4_Mes3.val(Ln4.Mes3);
            txtLn4_Mes4.val(Ln4.Mes4);
            txtLn4_Mes5.val(Ln4.Mes5);
            txtLn4_Mes6.val(Ln4.Mes6);
            txtLn4_Mes7.val(Ln4.Mes7);
            txtLn4_Mes8.val(Ln4.Mes8);
            txtLn4_Mes9.val(Ln4.Mes9);
            txtLn4_Mes10.val(Ln4.Mes10);
            txtLn4_Mes11.val(Ln4.Mes11);
            txtLn4_Mes12.val(Ln4.Mes12);
            txtLn4_MesT.val(Ln4.MesT);

            txtLn5_Mes2.html(Ln5.Mes2);
            txtLn5_Mes3.html(Ln5.Mes3);
            txtLn5_Mes4.html(Ln5.Mes4);
            txtLn5_Mes5.html(Ln5.Mes5);
            txtLn5_Mes6.html(Ln5.Mes6);
            txtLn5_Mes7.html(Ln5.Mes7);
            txtLn5_Mes8.html(Ln5.Mes8);
            txtLn5_Mes9.html(Ln5.Mes9);
            txtLn5_Mes10.html(Ln5.Mes10);
            txtLn5_Mes11.html(Ln5.Mes11);
            txtLn5_Mes12.html(Ln5.Mes12);
            txtLn5_MesT.html(Ln5.MesT);

            txtLn6_Mes1.val(Ln6.Mes1);
            txtLn6_Mes2.val(Ln6.Mes2);
            txtLn6_Mes3.val(Ln6.Mes3);
            txtLn6_Mes4.val(Ln6.Mes4);
            txtLn6_Mes5.val(Ln6.Mes5);
            txtLn6_Mes6.val(Ln6.Mes6);
            txtLn6_Mes7.val(Ln6.Mes7);
            txtLn6_Mes8.val(Ln6.Mes8);
            txtLn6_Mes9.val(Ln6.Mes9);
            txtLn6_Mes10.val(Ln6.Mes10);
            txtLn6_Mes11.val(Ln6.Mes11);
            txtLn6_Mes12.val(Ln6.Mes12);
            txtLn6_MesT.val(Ln6.MesT);
           
            calculos();         
        }
        function calculos() {
            var obj = getPlainObject();
            
            var Ln1 = obj.ln1CliPorcentajeSaldoAmortizar;
            var Ln2 = obj.ln2ImporteAmortizar1;
            var Ln3 = obj.ln3ImporteAmortizar2;
            var Ln4 = obj.ln4CxCPorcentajeSaldoAmortizar;
            var Ln5 = obj.ln5CxCImporteAmortizar;
            var Ln6 = obj.ln6AporteCapital;
            txtLn1_MesT.setVal(Ln1.Mes1+Ln1.Mes2+Ln1.Mes3+Ln1.Mes4+Ln1.Mes5+Ln1.Mes6+Ln1.Mes7+Ln1.Mes8+Ln1.Mes9+Ln1.Mes10+Ln1.Mes11+Ln1.Mes12);
            txtLn3_Mes1.setVal(Ln2.Mes1 + ((Ln1.Mes1 / 100) * Ln2.Mes1))
            txtLn3_Mes2.setVal(Ln2.Mes2 + ((Ln1.Mes2 / 100) * Ln2.Mes2))
            txtLn3_Mes3.setVal(Ln2.Mes3 + ((Ln1.Mes3 / 100) * Ln2.Mes3))
            txtLn3_Mes4.setVal(Ln2.Mes4 + ((Ln1.Mes4 / 100) * Ln2.Mes4))
            txtLn3_Mes5.setVal(Ln2.Mes5 + ((Ln1.Mes5 / 100) * Ln2.Mes5))
            txtLn3_Mes6.setVal(Ln2.Mes6 + ((Ln1.Mes6 / 100) * Ln2.Mes6))
            txtLn3_Mes7.setVal(Ln2.Mes7 + ((Ln1.Mes7 / 100) * Ln2.Mes7))
            txtLn3_Mes8.setVal(Ln2.Mes8 + ((Ln1.Mes8 / 100) * Ln2.Mes8))
            txtLn3_Mes9.setVal(Ln2.Mes9 + ((Ln1.Mes9 / 100) * Ln2.Mes9))
            txtLn3_Mes10.setVal(Ln2.Mes10 + ((Ln1.Mes10 / 100) * Ln2.Mes10))
            txtLn3_Mes11.setVal(Ln2.Mes11 + ((Ln1.Mes11 / 100) * Ln2.Mes11))
            txtLn3_Mes12.setVal(Ln2.Mes12 + ((Ln1.Mes12 / 100) * Ln2.Mes12))
            txtLn3_MesT.setVal(Ln2.MesT + ((Ln1.MesT / 100) * Ln2.MesT));

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
                url: '/Proyecciones/GuardarCobrosDiv',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ objFiltro: objFiltro, obj: obj }),
                success: function (response) {
                    if (response.success === true) {
                        AlertaGeneral("Confirmación","¡Datos guardados correctamente!");
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
                $(".thMes" + (i+1)).html(e);
            });
        }
        init();

    };

    $(document).ready(function () {
        administrativo.proyecciones.cobrosdiv = new cobrosdiv();
    });
})();


