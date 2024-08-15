(function () {
    $.namespace('administrativo.proyecciones.Premisas');

    Premisas = function () {
        var idGlobalRegistro = 0;
        mensajes = {
            NOMBRE: 'Proyecciones Financieras',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },
        tblPremisas = $("#tblPremisas"),
        lblEncabezadoPremisas = $("#lblEncabezadoPremisas"),
        btnGuardarPremisas = $("#btnGuardarPremisas"),
        tbCCPActual = $("tbCCPActual"),
        //Filtros
        tbMesesInicio = $("#tbMesesInicio"),
        cboPeriodo = $("#cboPeriodo")



        function init() {
            clearTabla();
            LoadTableP();
            btnGuardarPremisas.click(GuardarPremisas);
            $("#btnCargarInfo").unbind("click");
            $("#btnCargarInfo").click(function () {
                var mes = GetPeriodoMeses();
                idTituloContainer.text('Premisas macro económicas ' + mes[0]);
                LoadTableP();
            });

            $(".CustomDecimalEvent").change(function () {
                calculos();
            });
        }

        function LoadTableP() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var objFiltro = getFGB();

            $.ajax({
                url: '/proyecciones/GetFillTableP',
                type: 'POST',
                dataType: 'json',
                data: { objFiltro: objFiltro },
                success: function (response) {
                    if (response.success === true) {
                        clearTabla2();
                        idGlobalRegistro = response.id;
                        fillTblPremisas(response.TablaPremisas);
                        calculos();
                        fnReloadCustomDecialEvent();
                    }
                    else {
                        clearTabla2();
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    clearTabla2();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GuardarPremisas() {
            var objFiltro = getFGB();
            var objPremisa =  {
                id: 0,
                CadenaJson: "",
                Estatus: true,
                Mes: objFiltro.mes,
                Anio: objFiltro.anio
            }
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/GuardarPremisas',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj:objPremisa, data: getPremisasObj() }),
                success: function (response) {
                    ConfirmacionGeneral('Confirmacion', 'El registro se guardo Correctamente');
                    LoadTableP();
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function getPremisasObj() {

            var obj = {};
            var Ln1 = {
                Mes1: $("#tbCCPActual").getVal(2),
                Mes2: 0,
                Mes3: 0,
                Mes4: 0,
                Mes5: 0,
                Mes6: 0,
                Mes7: 0,
                Mes8: 0,
                Mes9: 0,
                Mes10: 0,
                Mes11: 0,
                Mes12: 0,
                MesT: 0
            }
            var Ln2 = {
                Mes1: $("#tbCCP1").getVal(2),
                Mes2: $("#tbCCP2").getVal(2),
                Mes3: $("#tbCCP3").getVal(2),
                Mes4: $("#tbCCP4").getVal(2),
                Mes5: $("#tbCCP5").getVal(2),
                Mes6: $("#tbCCP6").getVal(2),
                Mes7: $("#tbCCP7").getVal(2),
                Mes8: $("#tbCCP8").getVal(2),
                Mes9: $("#tbCCP9").getVal(2),
                Mes10: $("#tbCCP10").getVal(2),
                Mes11: $("#tbCCP11").getVal(2),
                Mes12: $("#tbCCP12").getVal(2),
                MesT: 0
            }
            var Ln3 = {
                Mes1: $(".CCPProyectado1").getVal(2),
                Mes2: $(".CCPProyectado2").getVal(2),
                Mes3: $(".CCPProyectado3").getVal(2),
                Mes4: $(".CCPProyectado4").getVal(2),
                Mes5: $(".CCPProyectado5").getVal(2),
                Mes6: $(".CCPProyectado6").getVal(2),
                Mes7: $(".CCPProyectado7").getVal(2),
                Mes8: $(".CCPProyectado8").getVal(2),
                Mes9: $(".CCPProyectado9").getVal(2),
                Mes10: $(".CCPProyectado10").getVal(2),
                Mes11: $(".CCPProyectado11").getVal(2),
                Mes12: $(".CCPProyectado12").getVal(2),
                MesT: 0
            }
            var Ln4 = {
                Mes1: $("#tbLiborActual").getVal(2),
                Mes2: 0,
                Mes3: 0,
                Mes4: 0,
                Mes5: 0,
                Mes6: 0,
                Mes7: 0,
                Mes8: 0,
                Mes9: 0,
                Mes10: 0,
                Mes11: 0,
                Mes12: 0,
                MesT: 0
            }
            var Ln5 = {
                Mes1: $("#tbLibor1").getVal(2),
                Mes2: $("#tbLibor2").getVal(2),
                Mes3: $("#tbLibor3").getVal(2),
                Mes4: $("#tbLibor4").getVal(2),
                Mes5: $("#tbLibor5").getVal(2),
                Mes6: $("#tbLibor6").getVal(2),
                Mes7: $("#tbLibor7").getVal(2),
                Mes8: $("#tbLibor8").getVal(2),
                Mes9: $("#tbLibor9").getVal(2),
                Mes10: $("#tbLibor10").getVal(2),
                Mes11: $("#tbLibor11").getVal(2),
                Mes12: $("#tbLibor12").getVal(2),
                MesT: 0
            }
            var Ln6 = {
                Mes1: $(".LiborProyectado1").getVal(2),
                Mes2: $(".LiborProyectado2").getVal(2),
                Mes3: $(".LiborProyectado3").getVal(2),
                Mes4: $(".LiborProyectado4").getVal(2),
                Mes5: $(".LiborProyectado5").getVal(2),
                Mes6: $(".LiborProyectado6").getVal(2),
                Mes7: $(".LiborProyectado7").getVal(2),
                Mes8: $(".LiborProyectado8").getVal(2),
                Mes9: $(".LiborProyectado9").getVal(2),
                Mes10: $(".LiborProyectado10").getVal(2),
                Mes11: $(".LiborProyectado11").getVal(2),
                Mes12: $(".LiborProyectado12").getVal(2),
                MesT: 0
            }
            var Ln7 = {
                Mes1: $("#tbDolarActual").getVal(2),
                Mes2: 0,
                Mes3: 0,
                Mes4: 0,
                Mes5: 0,
                Mes6: 0,
                Mes7: 0,
                Mes8: 0,
                Mes9: 0,
                Mes10: 0,
                Mes11: 0,
                Mes12: 0,
                MesT: 0
            }
            var Ln8 = {
                Mes1: $("#tbDolar1").getVal(2),
                Mes2: $("#tbDolar2").getVal(2),
                Mes3: $("#tbDolar3").getVal(2),
                Mes4: $("#tbDolar4").getVal(2),
                Mes5: $("#tbDolar5").getVal(2),
                Mes6: $("#tbDolar6").getVal(2),
                Mes7: $("#tbDolar7").getVal(2),
                Mes8: $("#tbDolar8").getVal(2),
                Mes9: $("#tbDolar9").getVal(2),
                Mes10: $("#tbDolar10").getVal(2),
                Mes11: $("#tbDolar11").getVal(2),
                Mes12: $("#tbDolar12").getVal(2),
                MesT: 0
            }
            var Ln9 = {
                Mes1: $(".DolarProyectado1").getVal(2),
                Mes2: $(".DolarProyectado2").getVal(2),
                Mes3: $(".DolarProyectado3").getVal(2),
                Mes4: $(".DolarProyectado4").getVal(2),
                Mes5: $(".DolarProyectado5").getVal(2),
                Mes6: $(".DolarProyectado6").getVal(2),
                Mes7: $(".DolarProyectado7").getVal(2),
                Mes8: $(".DolarProyectado8").getVal(2),
                Mes9: $(".DolarProyectado9").getVal(2),
                Mes10: $(".DolarProyectado10").getVal(2),
                Mes11: $(".DolarProyectado11").getVal(2),
                Mes12: $(".DolarProyectado12").getVal(2),
                MesT: 0
            }
            var Ln10 = {
                Mes1: $("#tbInflaccion1").getVal(2),
                Mes2: $("#tbInflaccion2").getVal(2),
                Mes3: $("#tbInflaccion3").getVal(2),
                Mes4: $("#tbInflaccion4").getVal(2),
                Mes5: $("#tbInflaccion5").getVal(2),
                Mes6: $("#tbInflaccion6").getVal(2),
                Mes7: $("#tbInflaccion7").getVal(2),
                Mes8: $("#tbInflaccion8").getVal(2),
                Mes9: $("#tbInflaccion9").getVal(2),
                Mes10: $("#tbInflaccion10").getVal(2),
                Mes11: $("#tbInflaccion11").getVal(2),
                Mes12: $("#tbInflaccion12").getVal(2),
                MesT: 0
            }
            var Ln11 = {
                Mes1: $(".InflaccionProyectado1").getVal(2),
                Mes2: $(".InflaccionProyectado2").getVal(2),
                Mes3: $(".InflaccionProyectado3").getVal(2),
                Mes4: $(".InflaccionProyectado4").getVal(2),
                Mes5: $(".InflaccionProyectado5").getVal(2),
                Mes6: $(".InflaccionProyectado6").getVal(2),
                Mes7: $(".InflaccionProyectado7").getVal(2),
                Mes8: $(".InflaccionProyectado8").getVal(2),
                Mes9: $(".InflaccionProyectado9").getVal(2),
                Mes10: $(".InflaccionProyectado10").getVal(2),
                Mes11: $(".InflaccionProyectado11").getVal(2),
                Mes12: $(".InflaccionProyectado12").getVal(2),
                MesT: 0
            }
            var Ln12 = {
                Mes1: $("#txtP1").getVal(0),
                Mes2: $("#txtP2").getVal(0),
                Mes3: $("#txtAguinaldo").getVal(2),
                Mes4: 0,
                Mes5: 0,
                Mes6: 0,
                Mes7: 0,
                Mes8: 0,
                Mes9: 0,
                Mes10: 0,
                Mes11: 0,
                Mes12: 0,
                MesT: 0
            }
            obj.Ln1 = Ln1;
            obj.Ln2 = Ln2;
            obj.Ln3 = Ln3;
            obj.Ln4 = Ln4;
            obj.Ln5 = Ln5;
            obj.Ln6 = Ln6;
            obj.Ln7 = Ln7;
            obj.Ln8 = Ln8;
            obj.Ln9 = Ln9;
            obj.Ln10 = Ln10;
            obj.Ln11 = Ln11;
            obj.Ln12 = Ln12;

            return obj;
        }

        function fillTblPremisas(dataSet) {
            
            var Meses = GetPeriodoMeses();

            $("#Mes1").setVal(Meses[0]);
            $("#Mes2").setVal(Meses[1]);
            $("#Mes3").setVal(Meses[2]);
            $("#Mes4").setVal(Meses[3]);
            $("#Mes5").setVal(Meses[4]);
            $("#Mes6").setVal(Meses[5]);
            $("#Mes7").setVal(Meses[6]);
            $("#Mes8").setVal(Meses[7]);
            $("#Mes9").setVal(Meses[8]);
            $("#Mes10").setVal(Meses[9]);
            $("#Mes11").setVal(Meses[10]);
            $("#Mes12").setVal(Meses[11]);

            if (dataSet != undefined) {
                var Ln1 = dataSet.Ln1;
                var Ln2 = dataSet.Ln2;
                var Ln3 = dataSet.Ln3;
                var Ln4 = dataSet.Ln4;
                var Ln5 = dataSet.Ln5;
                var Ln6 = dataSet.Ln6;
                var Ln7 = dataSet.Ln7;
                var Ln8 = dataSet.Ln8;
                var Ln9 = dataSet.Ln9;
                var Ln10 = dataSet.Ln10;
                var Ln11 = dataSet.Ln11;
                var Ln12 = dataSet.Ln12;

                $("#tbCCPActual").setVal(Ln1.Mes1);

                $("#tbCCP1").setVal(Ln2.Mes1);
                $("#tbCCP2").setVal(Ln2.Mes2);
                $("#tbCCP3").setVal(Ln2.Mes3);
                $("#tbCCP4").setVal(Ln2.Mes4);
                $("#tbCCP5").setVal(Ln2.Mes5);
                $("#tbCCP6").setVal(Ln2.Mes6);
                $("#tbCCP7").setVal(Ln2.Mes7);
                $("#tbCCP8").setVal(Ln2.Mes8);
                $("#tbCCP9").setVal(Ln2.Mes9);
                $("#tbCCP10").setVal(Ln2.Mes10);
                $("#tbCCP11").setVal(Ln2.Mes11);
                $("#tbCCP12").setVal(Ln2.Mes12);

                $(".CCPProyectado1").setVal(Ln3.Mes1);
                $(".CCPProyectado2").setVal(Ln3.Mes2);
                $(".CCPProyectado3").setVal(Ln3.Mes3);
                $(".CCPProyectado4").setVal(Ln3.Mes4);
                $(".CCPProyectado5").setVal(Ln3.Mes5);
                $(".CCPProyectado6").setVal(Ln3.Mes6);
                $(".CCPProyectado7").setVal(Ln3.Mes7);
                $(".CCPProyectado8").setVal(Ln3.Mes8);
                $(".CCPProyectado9").setVal(Ln3.Mes9);
                $(".CCPProyectado10").setVal(Ln3.Mes10);
                $(".CCPProyectado11").setVal(Ln3.Mes11);
                $(".CCPProyectado12").setVal(Ln3.Mes12);

                $("#tbLiborActual").setVal(Ln4.Mes1);

                $("#tbLibor1").setVal(Ln5.Mes1);
                $("#tbLibor2").setVal(Ln5.Mes2);
                $("#tbLibor3").setVal(Ln5.Mes3);
                $("#tbLibor4").setVal(Ln5.Mes4);
                $("#tbLibor5").setVal(Ln5.Mes5);
                $("#tbLibor6").setVal(Ln5.Mes6);
                $("#tbLibor7").setVal(Ln5.Mes7);
                $("#tbLibor8").setVal(Ln5.Mes8);
                $("#tbLibor9").setVal(Ln5.Mes9);
                $("#tbLibor10").setVal(Ln5.Mes10);
                $("#tbLibor11").setVal(Ln5.Mes11);
                $("#tbLibor12").setVal(Ln5.Mes12);

                $(".LiborProyectado1").setVal(Ln6.Mes1);
                $(".LiborProyectado2").setVal(Ln6.Mes2);
                $(".LiborProyectado3").setVal(Ln6.Mes3);
                $(".LiborProyectado4").setVal(Ln6.Mes4);
                $(".LiborProyectado5").setVal(Ln6.Mes5);
                $(".LiborProyectado6").setVal(Ln6.Mes6);
                $(".LiborProyectado7").setVal(Ln6.Mes7);
                $(".LiborProyectado8").setVal(Ln6.Mes8);
                $(".LiborProyectado9").setVal(Ln6.Mes9);
                $(".LiborProyectado10").setVal(Ln6.Mes10);
                $(".LiborProyectado11").setVal(Ln6.Mes11);
                $(".LiborProyectado12").setVal(Ln6.Mes12);

                $("#tbDolarActual").setVal(Ln7.Mes1);

                $("#tbDolar1").setVal(Ln8.Mes1);
                $("#tbDolar2").setVal(Ln8.Mes2);
                $("#tbDolar3").setVal(Ln8.Mes3);
                $("#tbDolar4").setVal(Ln8.Mes4);
                $("#tbDolar5").setVal(Ln8.Mes5);
                $("#tbDolar6").setVal(Ln8.Mes6);
                $("#tbDolar7").setVal(Ln8.Mes7);
                $("#tbDolar8").setVal(Ln8.Mes8);
                $("#tbDolar9").setVal(Ln8.Mes9);
                $("#tbDolar10").setVal(Ln8.Mes10);
                $("#tbDolar11").setVal(Ln8.Mes11);
                $("#tbDolar12").setVal(Ln8.Mes12);

                $(".DolarProyectado1").setVal(Ln9.Mes1);
                $(".DolarProyectado2").setVal(Ln9.Mes2);
                $(".DolarProyectado3").setVal(Ln9.Mes3);
                $(".DolarProyectado4").setVal(Ln9.Mes4);
                $(".DolarProyectado5").setVal(Ln9.Mes5);
                $(".DolarProyectado6").setVal(Ln9.Mes6);
                $(".DolarProyectado7").setVal(Ln9.Mes7);
                $(".DolarProyectado8").setVal(Ln9.Mes8);
                $(".DolarProyectado9").setVal(Ln9.Mes9);
                $(".DolarProyectado10").setVal(Ln9.Mes10);
                $(".DolarProyectado11").setVal(Ln9.Mes11);
                $(".DolarProyectado12").setVal(Ln9.Mes12);

                $("#tbInflaccion1").setVal(Ln10.Mes1);
                $("#tbInflaccion2").setVal(Ln10.Mes2);
                $("#tbInflaccion3").setVal(Ln10.Mes3);
                $("#tbInflaccion4").setVal(Ln10.Mes4);
                $("#tbInflaccion5").setVal(Ln10.Mes5);
                $("#tbInflaccion6").setVal(Ln10.Mes6);
                $("#tbInflaccion7").setVal(Ln10.Mes7);
                $("#tbInflaccion8").setVal(Ln10.Mes8);
                $("#tbInflaccion9").setVal(Ln10.Mes9);
                $("#tbInflaccion10").setVal(Ln10.Mes10);
                $("#tbInflaccion11").setVal(Ln10.Mes11);
                $("#tbInflaccion12").setVal(Ln10.Mes12);

                $(".InflaccionProyectado1").setVal(Ln11.Mes1);
                $(".InflaccionProyectado2").setVal(Ln11.Mes2);
                $(".InflaccionProyectado3").setVal(Ln11.Mes3);
                $(".InflaccionProyectado4").setVal(Ln11.Mes4);
                $(".InflaccionProyectado5").setVal(Ln11.Mes5);
                $(".InflaccionProyectado6").setVal(Ln11.Mes6);
                $(".InflaccionProyectado7").setVal(Ln11.Mes7);
                $(".InflaccionProyectado8").setVal(Ln11.Mes8);
                $(".InflaccionProyectado9").setVal(Ln11.Mes9);
                $(".InflaccionProyectado10").setVal(Ln11.Mes10);
                $(".InflaccionProyectado11").setVal(Ln11.Mes11);
                $(".InflaccionProyectado12").setVal(Ln11.Mes12);


                $("#txtP1").setVal(Ln12.Mes1);
                $("#txtP2").setVal(Ln12.Mes2);
                $("#txtAguinaldo").setVal(Ln12.Mes3);
            }

        }

        function clearTabla() {
            $("#tbCCPActual").DecimalFixNS(2);

            $("#tbCCP1").DecimalFixNS(2);
            $("#tbCCP2").DecimalFixNS(2);
            $("#tbCCP3").DecimalFixNS(2);
            $("#tbCCP4").DecimalFixNS(2);
            $("#tbCCP5").DecimalFixNS(2);
            $("#tbCCP6").DecimalFixNS(2);
            $("#tbCCP7").DecimalFixNS(2);
            $("#tbCCP8").DecimalFixNS(2);
            $("#tbCCP9").DecimalFixNS(2);
            $("#tbCCP10").DecimalFixNS(2);
            $("#tbCCP11").DecimalFixNS(2);
            $("#tbCCP12").DecimalFixNS(2);

            $(".CCPProyectado1").DecimalFixNS(2);
            $(".CCPProyectado2").DecimalFixNS(2);
            $(".CCPProyectado3").DecimalFixNS(2);
            $(".CCPProyectado4").DecimalFixNS(2);
            $(".CCPProyectado5").DecimalFixNS(2);
            $(".CCPProyectado6").DecimalFixNS(2);
            $(".CCPProyectado7").DecimalFixNS(2);
            $(".CCPProyectado8").DecimalFixNS(2);
            $(".CCPProyectado9").DecimalFixNS(2);
            $(".CCPProyectado10").DecimalFixNS(2);
            $(".CCPProyectado11").DecimalFixNS(2);
            $(".CCPProyectado12").DecimalFixNS(2);

            $("#tbLiborActual").DecimalFixNS(2);

            $("#tbLibor1").DecimalFixNS(3);
            $("#tbLibor2").DecimalFixNS(3);
            $("#tbLibor3").DecimalFixNS(3);
            $("#tbLibor4").DecimalFixNS(3);
            $("#tbLibor5").DecimalFixNS(3);
            $("#tbLibor6").DecimalFixNS(3);
            $("#tbLibor7").DecimalFixNS(3);
            $("#tbLibor8").DecimalFixNS(3);
            $("#tbLibor9").DecimalFixNS(3);
            $("#tbLibor10").DecimalFixNS(3);
            $("#tbLibor11").DecimalFixNS(3);
            $("#tbLibor12").DecimalFixNS(3);

            $(".LiborProyectado1").DecimalFixNS(2);
            $(".LiborProyectado2").DecimalFixNS(2);
            $(".LiborProyectado3").DecimalFixNS(2);
            $(".LiborProyectado4").DecimalFixNS(2);
            $(".LiborProyectado5").DecimalFixNS(2);
            $(".LiborProyectado6").DecimalFixNS(2);
            $(".LiborProyectado7").DecimalFixNS(2);
            $(".LiborProyectado8").DecimalFixNS(2);
            $(".LiborProyectado9").DecimalFixNS(2);
            $(".LiborProyectado10").DecimalFixNS(2);
            $(".LiborProyectado11").DecimalFixNS(2);
            $(".LiborProyectado12").DecimalFixNS(2);

            $("#tbDolarActual").DecimalFixNS(2);

            $("#tbDolar1").DecimalFixNS(4);
            $("#tbDolar2").DecimalFixNS(4);
            $("#tbDolar3").DecimalFixNS(4);
            $("#tbDolar4").DecimalFixNS(4);
            $("#tbDolar5").DecimalFixNS(4);
            $("#tbDolar6").DecimalFixNS(4);
            $("#tbDolar7").DecimalFixNS(4);
            $("#tbDolar8").DecimalFixNS(4);
            $("#tbDolar9").DecimalFixNS(4);
            $("#tbDolar10").DecimalFixNS(4);
            $("#tbDolar11").DecimalFixNS(4);
            $("#tbDolar12").DecimalFixNS(4);

            $(".DolarProyectado1").DecimalFixNS(4);
            $(".DolarProyectado2").DecimalFixNS(4);
            $(".DolarProyectado3").DecimalFixNS(4);
            $(".DolarProyectado4").DecimalFixNS(4);
            $(".DolarProyectado5").DecimalFixNS(4);
            $(".DolarProyectado6").DecimalFixNS(4);
            $(".DolarProyectado7").DecimalFixNS(4);
            $(".DolarProyectado8").DecimalFixNS(4);
            $(".DolarProyectado9").DecimalFixNS(4);
            $(".DolarProyectado10").DecimalFixNS(4);
            $(".DolarProyectado11").DecimalFixNS(4);
            $(".DolarProyectado12").DecimalFixNS(4);

            $("#tbInflaccion1").DecimalFixNS(2);
            $("#tbInflaccion2").DecimalFixNS(2);
            $("#tbInflaccion3").DecimalFixNS(2);
            $("#tbInflaccion4").DecimalFixNS(2);
            $("#tbInflaccion5").DecimalFixNS(2);
            $("#tbInflaccion6").DecimalFixNS(2);
            $("#tbInflaccion7").DecimalFixNS(2);
            $("#tbInflaccion8").DecimalFixNS(2);
            $("#tbInflaccion9").DecimalFixNS(2);
            $("#tbInflaccion10").DecimalFixNS(2);
            $("#tbInflaccion11").DecimalFixNS(2);
            $("#tbInflaccion12").DecimalFixNS(2);
            $("#tbInflaccionTotal").DecimalFixNS(2);

            $("#tbInflaccion1").DecimalFixPr(2);
            $("#tbInflaccion2").DecimalFixPr(2);
            $("#tbInflaccion3").DecimalFixPr(2);
            $("#tbInflaccion4").DecimalFixPr(2);
            $("#tbInflaccion5").DecimalFixPr(2);
            $("#tbInflaccion6").DecimalFixPr(2);
            $("#tbInflaccion7").DecimalFixPr(2);
            $("#tbInflaccion8").DecimalFixPr(2);
            $("#tbInflaccion9").DecimalFixPr(2);
            $("#tbInflaccion10").DecimalFixPr(2);
            $("#tbInflaccion11").DecimalFixPr(2);
            $("#tbInflaccion12").DecimalFixPr(2);

            $(".InflaccionProyectado1").DecimalFixPr(2);
            $(".InflaccionProyectado2").DecimalFixPr(2);
            $(".InflaccionProyectado3").DecimalFixPr(2);
            $(".InflaccionProyectado4").DecimalFixPr(2);
            $(".InflaccionProyectado5").DecimalFixPr(2);
            $(".InflaccionProyectado6").DecimalFixPr(2);
            $(".InflaccionProyectado7").DecimalFixPr(2);
            $(".InflaccionProyectado8").DecimalFixPr(2);
            $(".InflaccionProyectado9").DecimalFixPr(2);
            $(".InflaccionProyectado10").DecimalFixPr(2);
            $(".InflaccionProyectado11").DecimalFixPr(2);
            $(".InflaccionProyectado12").DecimalFixPr(2);

            $("#txtP1").DecimalFixNS(2);
            $("#txtP2").DecimalFixNS(2);
            $("#txtAguinaldo").DecimalFixNS(2);
        }
        function clearTabla2() {
            $("#tbCCPActual").setVal(0);

            $("#tbCCP1").setVal(0);
            $("#tbCCP2").setVal(0);
            $("#tbCCP3").setVal(0);
            $("#tbCCP4").setVal(0);
            $("#tbCCP5").setVal(0);
            $("#tbCCP6").setVal(0);
            $("#tbCCP7").setVal(0);
            $("#tbCCP8").setVal(0);
            $("#tbCCP9").setVal(0);
            $("#tbCCP10").setVal(0);
            $("#tbCCP11").setVal(0);
            $("#tbCCP12").setVal(0);

            $(".CCPProyectado1").setVal(0);
            $(".CCPProyectado2").setVal(0);
            $(".CCPProyectado3").setVal(0);
            $(".CCPProyectado4").setVal(0);
            $(".CCPProyectado5").setVal(0);
            $(".CCPProyectado6").setVal(0);
            $(".CCPProyectado7").setVal(0);
            $(".CCPProyectado8").setVal(0);
            $(".CCPProyectado9").setVal(0);
            $(".CCPProyectado10").setVal(0);
            $(".CCPProyectado11").setVal(0);
            $(".CCPProyectado12").setVal(0);

            $("#tbLiborActual").setVal(0);

            $("#tbLibor1").setVal(0);
            $("#tbLibor2").setVal(0);
            $("#tbLibor3").setVal(0);
            $("#tbLibor4").setVal(0);
            $("#tbLibor5").setVal(0);
            $("#tbLibor6").setVal(0);
            $("#tbLibor7").setVal(0);
            $("#tbLibor8").setVal(0);
            $("#tbLibor9").setVal(0);
            $("#tbLibor10").setVal(0);
            $("#tbLibor11").setVal(0);
            $("#tbLibor12").setVal(0);

            $(".LiborProyectado1").setVal(0);
            $(".LiborProyectado2").setVal(0);
            $(".LiborProyectado3").setVal(0);
            $(".LiborProyectado4").setVal(0);
            $(".LiborProyectado5").setVal(0);
            $(".LiborProyectado6").setVal(0);
            $(".LiborProyectado7").setVal(0);
            $(".LiborProyectado8").setVal(0);
            $(".LiborProyectado9").setVal(0);
            $(".LiborProyectado10").setVal(0);
            $(".LiborProyectado11").setVal(0);
            $(".LiborProyectado12").setVal(0);

            $("#tbDolarActual").setVal(0);

            $("#tbDolar1").setVal(0);
            $("#tbDolar2").setVal(0);
            $("#tbDolar3").setVal(0);
            $("#tbDolar4").setVal(0);
            $("#tbDolar5").setVal(0);
            $("#tbDolar6").setVal(0);
            $("#tbDolar7").setVal(0);
            $("#tbDolar8").setVal(0);
            $("#tbDolar9").setVal(0);
            $("#tbDolar10").setVal(0);
            $("#tbDolar11").setVal(0);
            $("#tbDolar12").setVal(0);

            $(".DolarProyectado1").setVal(0);
            $(".DolarProyectado2").setVal(0);
            $(".DolarProyectado3").setVal(0);
            $(".DolarProyectado4").setVal(0);
            $(".DolarProyectado5").setVal(0);
            $(".DolarProyectado6").setVal(0);
            $(".DolarProyectado7").setVal(0);
            $(".DolarProyectado8").setVal(0);
            $(".DolarProyectado9").setVal(0);
            $(".DolarProyectado10").setVal(0);
            $(".DolarProyectado11").setVal(0);
            $(".DolarProyectado12").setVal(0);

            $("#tbInflaccion1").setVal(0);
            $("#tbInflaccion2").setVal(0);
            $("#tbInflaccion3").setVal(0);
            $("#tbInflaccion4").setVal(0);
            $("#tbInflaccion5").setVal(0);
            $("#tbInflaccion6").setVal(0);
            $("#tbInflaccion7").setVal(0);
            $("#tbInflaccion8").setVal(0);
            $("#tbInflaccion9").setVal(0);
            $("#tbInflaccion10").setVal(0);
            $("#tbInflaccion11").setVal(0);
            $("#tbInflaccion12").setVal(0);
            $("#tbInflaccionTotal").setVal(0);

            $("#tbInflaccion1").setVal(0);
            $("#tbInflaccion2").setVal(0);
            $("#tbInflaccion3").setVal(0);
            $("#tbInflaccion4").setVal(0);
            $("#tbInflaccion5").setVal(0);
            $("#tbInflaccion6").setVal(0);
            $("#tbInflaccion7").setVal(0);
            $("#tbInflaccion8").setVal(0);
            $("#tbInflaccion9").setVal(0);
            $("#tbInflaccion10").setVal(0);
            $("#tbInflaccion11").setVal(0);
            $("#tbInflaccion12").setVal(0);

            $(".InflaccionProyectado1").setVal(0);
            $(".InflaccionProyectado2").setVal(0);
            $(".InflaccionProyectado3").setVal(0);
            $(".InflaccionProyectado4").setVal(0);
            $(".InflaccionProyectado5").setVal(0);
            $(".InflaccionProyectado6").setVal(0);
            $(".InflaccionProyectado7").setVal(0);
            $(".InflaccionProyectado8").setVal(0);
            $(".InflaccionProyectado9").setVal(0);
            $(".InflaccionProyectado10").setVal(0);
            $(".InflaccionProyectado11").setVal(0);
            $(".InflaccionProyectado12").setVal(0);

            $("#txtP1").setVal(0);
            $("#txtP2").setVal(0);
            $("#txtAguinaldo").setVal(0);
        }
        function calculos() {
            $(".CCPProyectado1").setVal($("#tbCCPActual").getVal(2) * $("#tbCCP1").getVal(2));
            $(".CCPProyectado2").setVal($(".CCPProyectado1").getVal(2) * $("#tbCCP2").getVal(2));
            $(".CCPProyectado3").setVal($(".CCPProyectado2").getVal(2) * $("#tbCCP3").getVal(2));
            $(".CCPProyectado4").setVal($(".CCPProyectado3").getVal(2) * $("#tbCCP4").getVal(2));
            $(".CCPProyectado5").setVal($(".CCPProyectado4").getVal(2) * $("#tbCCP5").getVal(2));
            $(".CCPProyectado6").setVal($(".CCPProyectado5").getVal(2) * $("#tbCCP6").getVal(2));
            $(".CCPProyectado7").setVal($(".CCPProyectado6").getVal(2) * $("#tbCCP7").getVal(2));
            $(".CCPProyectado8").setVal($(".CCPProyectado7").getVal(2) * $("#tbCCP8").getVal(2));
            $(".CCPProyectado9").setVal($(".CCPProyectado8").getVal(2) * $("#tbCCP9").getVal(2));
            $(".CCPProyectado10").setVal($(".CCPProyectado9").getVal(2) * $("#tbCCP10").getVal(2));
            $(".CCPProyectado11").setVal($(".CCPProyectado10").getVal(2) * $("#tbCCP11").getVal(2));
            $(".CCPProyectado12").setVal($(".CCPProyectado11").getVal(2) * $("#tbCCP12").getVal(2));

            $(".LiborProyectado1").setVal($("#tbLiborActual").getVal(2) * $("#tbLibor1").getVal(3));
            $(".LiborProyectado2").setVal($(".LiborProyectado1").getVal(2) * $("#tbLibor2").getVal(3));
            $(".LiborProyectado3").setVal($(".LiborProyectado2").getVal(2) * $("#tbLibor3").getVal(3));
            $(".LiborProyectado4").setVal($(".LiborProyectado3").getVal(2) * $("#tbLibor4").getVal(3));
            $(".LiborProyectado5").setVal($(".LiborProyectado4").getVal(2) * $("#tbLibor5").getVal(3));
            $(".LiborProyectado6").setVal($(".LiborProyectado5").getVal(2) * $("#tbLibor6").getVal(3));
            $(".LiborProyectado7").setVal($(".LiborProyectado6").getVal(2) * $("#tbLibor7").getVal(3));
            $(".LiborProyectado8").setVal($(".LiborProyectado7").getVal(2) * $("#tbLibor8").getVal(3));
            $(".LiborProyectado9").setVal($(".LiborProyectado8").getVal(2) * $("#tbLibor9").getVal(3));
            $(".LiborProyectado10").setVal($(".LiborProyectado9").getVal(2) * $("#tbLibor10").getVal(3));
            $(".LiborProyectado11").setVal($(".LiborProyectado10").getVal(2) * $("#tbLibor11").getVal(3));
            $(".LiborProyectado12").setVal($(".LiborProyectado11").getVal(2) * $("#tbLibor12").getVal(3));

            $(".DolarProyectado1").setVal($("#tbDolarActual").getVal(2) * $("#tbDolar1").getVal(4));
            $(".DolarProyectado2").setVal($("#tbDolarActual").getVal(2) * ($("#tbDolar2").getVal(4) * 2) + $("#tbDolarActual").getVal(2));
            $(".DolarProyectado3").setVal($("#tbDolarActual").getVal(2) * ($("#tbDolar3").getVal(4) * 2) + $("#tbDolarActual").getVal(2));
            $(".DolarProyectado4").setVal($("#tbDolarActual").getVal(2) * ($("#tbDolar4").getVal(4) * 2) + $("#tbDolarActual").getVal(2));
            $(".DolarProyectado5").setVal($("#tbDolarActual").getVal(2) * ($("#tbDolar5").getVal(4) * 2) + $("#tbDolarActual").getVal(2));
            $(".DolarProyectado6").setVal($("#tbDolarActual").getVal(2) * ($("#tbDolar6").getVal(4) * 2) + $("#tbDolarActual").getVal(2));
            $(".DolarProyectado7").setVal($("#tbDolarActual").getVal(2) * ($("#tbDolar7").getVal(4) * 2) + $("#tbDolarActual").getVal(2));
            $(".DolarProyectado8").setVal($("#tbDolarActual").getVal(2) * ($("#tbDolar8").getVal(4) * 2) + $("#tbDolarActual").getVal(2));
            $(".DolarProyectado9").setVal($("#tbDolarActual").getVal(2) * ($("#tbDolar9").getVal(4) * 2) + $("#tbDolarActual").getVal(2));
            $(".DolarProyectado10").setVal($("#tbDolarActual").getVal(2) * ($("#tbDolar10").getVal(4) * 2) + $("#tbDolarActual").getVal(2));
            $(".DolarProyectado11").setVal($("#tbDolarActual").getVal(2) * ($("#tbDolar11").getVal(4) * 2) + $("#tbDolarActual").getVal(2));
            $(".DolarProyectado12").setVal($("#tbDolarActual").getVal(2) * ($("#tbDolar12").getVal(4) * 2) + $("#tbDolarActual").getVal(2));

            $("#tbInflaccionTotal").setVal($("#tbInflaccion1").getVal(2) + $("#tbInflaccion2").getVal(2) + $("#tbInflaccion3").getVal(2) + $("#tbInflaccion4").getVal(2) + $("#tbInflaccion5").getVal(2) + $("#tbInflaccion6").getVal(2) + $("#tbInflaccion7").getVal(2) + $("#tbInflaccion8").getVal(2) + $("#tbInflaccion9").getVal(2) + $("#tbInflaccion10").getVal(2) + $("#tbInflaccion11").getVal(2) + $("#tbInflaccion12").getVal(2));

            $(".InflaccionProyectado1").setVal($("#tbInflaccion1").getVal(2));
            $(".InflaccionProyectado2").setVal($(".InflaccionProyectado1").getVal(2) + $("#tbInflaccion2").getVal(2));
            $(".InflaccionProyectado3").setVal($(".InflaccionProyectado2").getVal(2) + $("#tbInflaccion3").getVal(2));
            $(".InflaccionProyectado4").setVal($(".InflaccionProyectado3").getVal(2) + $("#tbInflaccion4").getVal(2));
            $(".InflaccionProyectado5").setVal($(".InflaccionProyectado4").getVal(2) + $("#tbInflaccion5").getVal(2));
            $(".InflaccionProyectado6").setVal($(".InflaccionProyectado5").getVal(2) + $("#tbInflaccion6").getVal(2));
            $(".InflaccionProyectado7").setVal($(".InflaccionProyectado6").getVal(2) + $("#tbInflaccion7").getVal(2));
            $(".InflaccionProyectado8").setVal($(".InflaccionProyectado7").getVal(2) + $("#tbInflaccion8").getVal(2));
            $(".InflaccionProyectado9").setVal($(".InflaccionProyectado8").getVal(2) + $("#tbInflaccion9").getVal(2));
            $(".InflaccionProyectado10").setVal($(".InflaccionProyectado9").getVal(2) + $("#tbInflaccion10").getVal(2));
            $(".InflaccionProyectado11").setVal($(".InflaccionProyectado10").getVal(2) + $("#tbInflaccion11").getVal(2));
            $(".InflaccionProyectado12").setVal($(".InflaccionProyectado11").getVal(2) + $("#tbInflaccion12").getVal(2));

        }

        function GetPeriodoMeses() {
            var periodo = cboPeriodo.val();
            var MesInicio = tbMesesInicio.val();
            var months = ["ENE", "FEB", "MAR", "ABR", "MAY", "JUN",
                          "JUL", "AGO", "SEP", "OCT", "NOV", "DIC"];
            var tituloMeses = [];

            var count = 0;
            for (var i = MesInicio; i < 12; i++) {
                count++;
                tituloMeses.push(months[i] + " " + periodo);
            }

            for (var i = 0 ; i < MesInicio; i++) {
                tituloMeses.push(months[i] + " " + (Number(periodo) + 1));
            }
            return tituloMeses;

        }
        function formatNumber(valor) {
            return +((valor).replace(/[^0-9\.]/g, ''))
        }
        function formatCurrency(total) {
            var neg = false;
            if (total < 0) {
                neg = true;
                total = Math.abs(total);
            }
            return (neg ? "-$" : '$') + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
        }
        function getValueHtml(cadena) {
            switch (cadena.children().prop("tagName")) {
                case "INPUT":
                    return cadena.children().val();
                    break;
                case "LABEL":
                    return $(cadena).text();
                    break;
                default: return "";
                    break;
            }

        }


        init();
    };

    $(document).ready(function () {
        administrativo.proyecciones.Premisas = new Premisas();
    });
})();