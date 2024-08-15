$(function () {
    $.namespace('recursoshumanos.finiquito.captFiniquito');
    captFiniquito = function () {
        _Eliminar = 0;
        mensajes = {
            PROCESANDO: 'Procesando...'
        };

        /******* Variables divInfoUsuario **********/
        txtBonoNominaChange = $("#txtBonoNominaChange"),
            lblBonoMensualChange = $("#lblBonoMensualChange"),
            lblIgBonoNomina = $("#lblIgBonoNomina"),
            lblIgBonoMensual = $("#lblIgBonoMensual"),

            ireport = $("#report");
        txtIgNumeroEmpleado = $("#txtIgNumeroEmpleado");

        lblIgNombre = $("#lblIgNombre");
        lblIgIngreso = $("#lblIgIngreso");
        inputFechaEgreso = $("#inputFechaEgreso");
        lblIgTiempo = $("#lblIgTiempo");
        lblIgPuesto = $("#lblIgPuesto");
        lblIgNomina = $("#lblIgNomina");
        lblIgCentro = $("#lblIgCentro");
        lblIgJefe = $("#lblIgJefe");
        lblIgRegistro = $("#lblIgRegistro");
        lblIgSueldoNomina = $("#lblIgSueldoNomina");
        lblIgSueldoMensual = $("#lblIgSueldoMensual");
        lblIgComplementoNomina = $("#lblIgComplementoNomina");
        lblIgComplementoMensual = $("#lblIgComplementoMensual");
        lblIgTotalNomina = $("#lblIgTotalNomina");
        lblIgTotalMensual = $("#lblIgTotalMensual");
        lblTblIgNomina = $("#lblTblIgNomina");

        comboFormulo = $("#comboFormulo");
        comboFormuloPuesto = $("#comboFormuloPuesto");
        comboVobo = $("#comboVobo");
        comboVoboPuesto = $("#comboVoboPuesto");
        comboAutorizo = $("#comboAutorizo");
        comboAutorizoPuesto = $("#comboAutorizoPuesto");

        btnAddRenglon = $("#btnAddRenglon");
        btnMinusRenglon = $("#btnMinusRenglon");

        tblData = $("#tblData");

        inputTotalFiniquito = $("#inputTotalFiniquito");

        btnGuardarFiniquito = $("#btnGuardarFiniquito");

        $('#tblData tbody').on({
            click: function () {
                $(this).closest('tr').remove();
            }
        }, ".btn-eliminar");

        $("#tblData tbody").on({
            change: function () {
                fnResultados();
            }
        }, ".resultado");

        $("#tblData tbody").on({
            change: function () {
                if (
                    //$(this).val() == 2 &&
                    $(this).find('option:selected').text().includes("VACACIONES")) {
                    var row = $(this).closest('tr');

                    ////
                    //var conceptoInfo = row.find('.conceptoInfo');
                    //var conceptoInfoDiv = row.find('.conceptoInfo').closest('div');
                    //conceptoInfo.remove();
                    //var html = "";
                    //html += '<select class="form-control conceptoInfo" style="box-shadow: none;"></select>';
                    //$(html).appendTo(conceptoInfoDiv);

                    //$(".conceptoInfo:last").fillCombo('/Finiquito/FillComboVacacionesPeriodos', { ingreso: lblIgIngreso.val(), egreso: inputFechaEgreso.val(), anios: unmaskNumero(lblIgTiempo.val()) }, false, null);
                    ////

                    var detalle = row.find('.conceptoDetalle').closest('div');
                    detalle.remove();

                    var resultadoInput = row.find('.resultado').closest('div');
                    resultadoInput.remove();

                    var html = "";
                    html += '       <div class="col-lg-12 text-right vacInput">';
                    html += '           <input type="text" id="" class="form-control text-center inputTablaFiniquito vacFijo" value="' + fnSwitchVacaciones(unmaskNumero(lblIgTiempo.val())) + '" /> x';
                    html += '           <input type="text" id="" class="form-control text-center inputTablaFiniquito vacDiasTrans" value="" /> =';
                    html += '           <input type="text" id="" class="form-control text-center inputTablaFiniquito vacRes1" value="0.00" /> x';
                    html += '           <input type="text" id="" class="form-control text-right inputTablaFiniquito aguiSalDiario dinero" value="$0.00" /> =';
                    html += '       </div>';
                    $(html).appendTo(row.find('td:eq(1)'));

                    var html2 = "";
                    html2 += '       <div class="col-lg-12">';
                    html2 += '          <input type="text" class="form-control text-right resultado dinero" value="$0.00" style="background-color: white; border-radius: 0px;" />';
                    html2 += '       </div>';
                    $(html2).appendTo(row.find('td:eq(2)'));

                    row.find('.aguiSalDiario').val(maskNumero(unmaskNumero(lblIgSueldoMensual.val()) / 30.4));
                } else {
                    var row = $(this).closest('tr');
                    var detalle = row.find('.vacInput');
                    detalle.remove();

                    var resultadoInput = row.find('.resultado').closest('div');
                    resultadoInput.remove();

                    ////
                    //var info = row.find('.conceptoInfo');
                    //if (!info.length) {
                    //    var html = "";
                    //    html += '<input type="text" class="form-control text-left conceptoInfo" id="" style="box-shadow: none;" />';
                    //    $(html).appendTo(row.find('td:eq(0) div:eq(1)'));
                    //}
                    ////

                    var detalle2 = row.find('.conceptoDetalle');
                    if (!detalle2.length) {
                        var html = "";
                        html += '       <div class="col-lg-12">';
                        html += '           <input type="text" class="form-control text-right conceptoDetalle" id="" style="box-shadow: none;" />';
                        html += '       </div>';
                        $(html).appendTo(row.find('td:eq(1)'));
                    }

                    var html2 = "";
                    html2 += '       <div class="col-lg-12">';
                    html2 += '          <input type="text" class="form-control text-right resultado dinero" value="$0.00" style="box-shadow: none; border-radius: 0px;" />';
                    html2 += '       </div>';
                    $(html2).appendTo(row.find('td:eq(2)'));
                }

                if (
                    //$(this).val() == 3 &&
                    $(this).find('option:selected').text().includes("PRIMA VACACIONAL")) {
                    var row = $(this).closest('tr');
                    var detalleInput = row.find('.conceptoDetalle');
                    var resultado = row.find('.resultado');
                    detalleInput.val("25%");

                    if ($(".comboConcepto option:selected[value='2']").length) {
                        var res = 0;
                        $(".comboConcepto option:selected[value='2']").each(function () {
                            var row = $(this).closest('tr');
                            var resTemp = unmaskNumero(row.find('.resultado').val());
                            res += resTemp;
                        });

                        var final = res * 0.25;

                        resultado.val(maskNumero(final));

                        fnResultados();
                    }
                } else {
                    var row = $(this).closest('tr');
                    var detalleInput = row.find('.conceptoDetalle');
                    detalleInput.val("");

                    fnResultados();
                }

                if (
                    //$(this).val() == 5 &&
                    $(this).find('option:selected').text().includes("PRIMA DE ANTIGÜEDAD")) {
                    var row = $(this).closest('tr');
                    var detalleInput = row.find('.conceptoDetalle');
                    var resultado = row.find('.resultado');

                    //var tiempo = unmaskNumero(lblIgTiempo.val());
                    //var salarioMinimo = 88.36;

                    //var primaAnti = (tiempo * (salarioMinimo * 2))

                    var ingreso = lblIgIngreso.val();
                    var egreso = inputFechaEgreso.val();

                    $.ajax({
                        url: '/Finiquito/PrimaAntiguedad',
                        type: 'POST',
                        data: { ingreso: ingreso, egreso: egreso },
                        success: function (response) {
                            resultado.val(maskNumero(response.data));
                            resultado.change();
                        }
                    });
                }
            }
        }, ".comboConcepto");

        $("#tblData tbody").on({
            change: function () {
                var row = $(this).closest('tr');

                var check = row.find('.comboConcepto');

                if (check.length) {
                    if ($(this)[0] == row.find('.vacRes1')[0]) {
                        var fijo = row.find('.vacFijo');
                        var vacDiasTrans = row.find('.vacDiasTrans');
                        var res1 = row.find('.vacRes1');
                        var aguiSalDiario = row.find('.aguiSalDiario');
                        var resultado = row.find('.resultado');

                        var fijoVal = unmaskNumero(fijo.val());
                        var vacDiasTransVal = unmaskNumero(vacDiasTrans.val());
                        var res1Val = unmaskNumero(res1.val());
                        var aguiSalDiarioVal = unmaskNumero(aguiSalDiario.val());

                        if (!isNaN(fijoVal) && !isNaN(vacDiasTransVal) && !isNaN(res1Val) && !isNaN(aguiSalDiarioVal)) {
                            if (vacDiasTrans.val() == "" || isNaN(vacDiasTrans.val())) {
                                vacDiasTrans.val(365);
                            }

                            fijo.val((res1.val() / vacDiasTrans.val()).toFixed(2));
                            resultado.val(maskNumero((res1.val() * unmaskNumero(aguiSalDiario.val())).toFixed(2)));

                            fnResultados();
                        }
                    } else {
                        var fijo = row.find('.vacFijo');
                        var vacDiasTrans = row.find('.vacDiasTrans');
                        var res1 = row.find('.vacRes1');
                        var aguiSalDiario = row.find('.aguiSalDiario');
                        var resultado = row.find('.resultado');

                        var fijoVal = unmaskNumero(fijo.val());
                        var vacDiasTransVal = unmaskNumero(vacDiasTrans.val());
                        var res1Val = unmaskNumero(res1.val());
                        var aguiSalDiarioVal = unmaskNumero(aguiSalDiario.val());

                        if (!isNaN(fijoVal) && !isNaN(vacDiasTransVal) && !isNaN(res1Val) && !isNaN(aguiSalDiarioVal)) {
                            res1.val(unmaskNumero((fijo.val() * vacDiasTrans.val()).toFixed(2)));
                            resultado.val(maskNumero((res1.val() * unmaskNumero(aguiSalDiario.val())).toFixed(2)));

                            fnResultados();
                        }
                    }
                } else {
                    if ($(this)[0] == row.find('#aguiRes1')[0]) {
                        var fijo = row.find('#aguiFijo');
                        var aguiDiasTrans = row.find('#aguiDiasTrans');
                        var res1 = row.find('#aguiRes1');
                        var aguiSalDiario = row.find('#aguiSalDiario');
                        var resultado = row.find('#aguiRes2');

                        var fijoVal = unmaskNumero(fijo.val());
                        var aguiDiasTransVal = unmaskNumero(aguiDiasTrans.val());
                        var res1Val = unmaskNumero(res1.val());
                        var aguiSalDiarioVal = unmaskNumero(aguiSalDiario.val());

                        if (!isNaN(fijoVal) && !isNaN(aguiDiasTransVal) && !isNaN(res1Val) && !isNaN(aguiSalDiarioVal)) {
                            if (aguiDiasTrans.val() == "" || isNaN(aguiDiasTrans.val())) {
                                aguiDiasTrans.val(365);
                            }

                            fijo.val((res1.val() / aguiDiasTrans.val()).toFixed(2));
                            resultado.val(maskNumero((res1.val() * unmaskNumero(aguiSalDiario.val())).toFixed(2)));

                            fnResultados();
                        }
                    } else {
                        var fijo = row.find('#aguiFijo');
                        var aguiDiasTrans = row.find('#aguiDiasTrans');
                        var res1 = row.find('#aguiRes1');
                        var aguiSalDiario = row.find('#aguiSalDiario');
                        var resultado = row.find('#aguiRes2');

                        var fijoVal = unmaskNumero(fijo.val());
                        var aguiDiasTransVal = unmaskNumero(aguiDiasTrans.val());
                        var res1Val = unmaskNumero(res1.val());
                        var aguiSalDiarioVal = unmaskNumero(aguiSalDiario.val());

                        if (!isNaN(fijoVal) && !isNaN(aguiDiasTransVal) && !isNaN(res1Val) && !isNaN(aguiSalDiarioVal)) {
                            res1.val(unmaskNumero((fijo.val() * aguiDiasTrans.val()).toFixed(2)));
                            resultado.val(maskNumero((res1.val() * unmaskNumero(aguiSalDiario.val())).toFixed(2)));

                            fnResultados();
                        }
                    }
                }
            }
        }, ".inputTablaFiniquito");

        $("#tblData tbody").on({
            change: function () {
                var row = $(this).closest('tr');
                var diasTrans = row.find('.vacDiasTrans');
                var res1 = row.find('.vacRes1');
                var aguiSalDiario = row.find('.aguiSalDiario');
                var resultado = row.find('.resultado');

                var value = unmaskNumero($(this).val());
                // add only if the value is number
                if ((!isNaN(value) && value.length != 0) && (!isNaN(diasTrans.val()) && diasTrans.val().length != 0)) {
                    res1.val(unmaskNumero((value * diasTrans.val()).toFixed(2)));
                    resultado.val(maskNumero((res1.val() * unmaskNumero(aguiSalDiario.val())).toFixed(2)));

                    fnResultados();
                }
            }
        }, ".vacFijo");

        $("#tblData tbody").on({
            change: function () {
                var row = $(this).closest('tr');
                var fijo = row.find('.vacFijo');
                var res1 = row.find('.vacRes1');
                var aguiSalDiario = row.find('.aguiSalDiario');
                var resultado = row.find('.resultado');

                var value = unmaskNumero($(this).val());
                // add only if the value is number
                if ((!isNaN(value) && value.length != 0) && (!isNaN(fijo.val()) && fijo.val().length != 0)) {
                    res1.val(unmaskNumero((fijo.val() * value).toFixed(2)));
                    resultado.val(maskNumero((res1.val() * unmaskNumero(aguiSalDiario.val())).toFixed(2)));

                    fnResultados();
                }
            }
        }, ".vacDiasTrans");

        $("#divInfoUsuario").on({
            change: function () {
                var ingreso = lblIgIngreso.val();
                var egreso = inputFechaEgreso.val();

                $.ajax({
                    url: '/Finiquito/CheckDatesDiferencia',
                    type: 'POST',
                    data: { ingreso: ingreso, egreso: egreso },
                    success: function (response) {
                        var diferencia = response.success;

                        if (diferencia == true) {
                            //btnGuardarFiniquito.attr("disabled", false);

                            $.ajax({
                                url: '/Finiquito/EgresoChange',
                                type: 'POST',
                                data: { ingreso: ingreso, egreso: egreso },
                                success: function (response) {
                                    lblIgTiempo.val(response.anios.toFixed(2));
                                    $("#aguiDiasTrans").val(response.diasTrans);
                                    $("#aguiDiasTrans").change();
                                }
                            });

                            var resultado = $(".comboConcepto option:selected[value='5']").closest('tr').find('.resultado')
                            $.ajax({
                                url: '/Finiquito/PrimaAntiguedad',
                                type: 'POST',
                                data: { ingreso: ingreso, egreso: egreso },
                                success: function (response) {
                                    resultado.val(maskNumero(response.data));
                                    resultado.change();
                                }
                            });
                        } else {
                            //btnGuardarFiniquito.attr("disabled", true);

                            AlertaGeneral("Alerta", "La fecha de egreso es igual o menor a la fecha de ingreso. Se asignará el valor inicial para el empleado especificado.");

                            txtIgNumeroEmpleado.change();

                            //$.ajax({
                            //    url: '/Finiquito/EgresoChange',
                            //    type: 'POST',
                            //    data: { ingreso: ingreso, egreso: egreso },
                            //    success: function (response) {
                            //        lblIgTiempo.val(response.anios.toFixed(2));
                            //        $("#aguiDiasTrans").val(response.diasTrans);
                            //        $("#aguiDiasTrans").change();
                            //    }
                            //});

                            //var resultado = $(".comboConcepto option:selected[value='5']").closest('tr').find('.resultado')
                            //$.ajax({
                            //    url: '/Finiquito/PrimaAntiguedad',
                            //    type: 'POST',
                            //    data: { ingreso: ingreso, egreso: egreso },
                            //    success: function (response) {
                            //        resultado.val(maskNumero(response.data));
                            //        resultado.change();
                            //    }
                            //});
                        }
                    }
                });
            }
        }, "#inputFechaEgreso");

        $("#tblData tbody").on({
            change: function () {
                var valor = $(this).val();
                if (!isNaN(valor)) {
                    $(this).val(maskNumero(valor));
                }
            }
        }, ".dinero");

        $("#divAutorizaciones").on({
            change: function () {
                if (comboVobo.val() == "") {
                    comboVobo.data("claveEmpleado", null);
                    comboVoboPuesto.val("");
                } else {
                    $.ajax({
                        datatype: "json",
                        url: '/Administrativo/Finiquito/getEmpleado',
                        data: { id: comboVobo.data("claveEmpleado") },
                        success: function (response) {
                            var data = response.items;

                            var nombre = data.nombre + " " + data.ape_paterno + " " + data.ape_materno;

                            if (nombre != comboVobo.val()) {
                                comboVobo.data("claveEmpleado", null);
                                comboVoboPuesto.val("");
                            }
                        },
                        error: function () {
                            $.unblockUI();
                        }
                    });
                }
            }
        }, "#comboVobo");

        $("#divAutorizaciones").on({
            change: function () {
                if (comboAutorizo.val() == "") {
                    comboAutorizo.data("claveEmpleado", null);
                    comboAutorizoPuesto.val("");
                } else {
                    $.ajax({
                        datatype: "json",
                        url: '/Administrativo/Finiquito/getEmpleado',
                        data: { id: comboAutorizo.data("claveEmpleado") },
                        success: function (response) {
                            var data = response.items;

                            var nombre = data.nombre + " " + data.ape_paterno + " " + data.ape_materno;

                            if (nombre != comboAutorizo.val()) {
                                comboAutorizo.data("claveEmpleado", null);
                                comboAutorizoPuesto.val("");
                            }
                        },
                        error: function () {
                            $.unblockUI();
                        }
                    });
                }
            }
        }, "#comboAutorizo");

        function init() {
            txtIgNumeroEmpleado.change(selectEmpleado);

            btnAddRenglon.click(AgregarRenglon);
            btnMinusRenglon.click(RemueveRenglon);

            btnGuardarFiniquito.click(fnGuardarFiniquito);

            inputFechaEgreso.datepicker({ dateFormat: 'd/m/yy' });

            setAutoComplete();

            AgregarRenglones();
        }

        function fnSwitchVacaciones(anios) {
            var resultado = 0;

            if (anios < 1) {
                resultado = 0.016;
            }

            if (anios == 1) {
                resultado = 0.016;
            }

            if (anios == 2) {
                resultado = 0.021;
            }

            if (anios == 3) {
                resultado = 0.027;
            }

            if (anios == 4) {
                resultado = 0.032;
            }

            if (anios >= 5 && anios < 10) {
                resultado = 0.038;
            }

            if (anios >= 10 && anios < 15) {
                resultado = 0.043;
            }

            if (anios >= 15 && anios < 20) {
                resultado = 0.049;
            }

            if (anios >= 20 && anios <= 24) {
                resultado = 0.054;
            }

            if (anios > 24) {
                resultado = 0.054;
            }

            return resultado;
        }

        function fnGuardarFiniquito() {
            //if (inputFechaEgreso.val() != "") {
            if (txtIgNumeroEmpleado.val() != "" && lblIgNombre.val() != "" && !isNaN(unmaskNumero(txtIgNumeroEmpleado.val()))) {
                var objGeneral = {
                    id: 0,
                    claveEmpleado: unmaskNumero(txtIgNumeroEmpleado.val()),
                    formuloID: comboFormulo.data("claveEmpleado"),
                    formuloNombre: comboFormulo.val(),
                    voboID: comboVobo.data("claveEmpleado"),
                    voboNombre: comboVobo.val(),
                    autorizoID: comboAutorizo.data("claveEmpleado"),
                    autorizoNombre: comboAutorizo.val(),
                    total: unmaskNumero(inputTotalFiniquito.val())
                };

                var arr = new Array();

                var flagConceptoOTROS = false;

                $("#tblData tbody tr").each(function (index) {
                    var row = $(this);
                    var objDetalle = {};

                    if (index == 0) {
                        objDetalle = {
                            id: 0,
                            conceptoID: 1,
                            conceptoInfo: "",
                            operacion1: unmaskNumero(row.find('#aguiFijo').val()),
                            operacion2: unmaskNumero(row.find('#aguiDiasTrans').val()),
                            operacion3: unmaskNumero(row.find('#aguiRes1').val()),
                            operacion4: unmaskNumero(row.find('#aguiSalDiario').val()),
                            conceptoDetalle: "",
                            resultado: unmaskNumero(row.find('.resultado').val())
                        };
                    } else {
                        //conceptoTipo = row.find('.comboConcepto option:selected').data("prefijo");

                        if (row.find('.comboConcepto option:selected').val() == 2) {
                            objDetalle = {
                                id: 0,
                                conceptoID: unmaskNumero(row.find('.comboConcepto option:selected').val()),
                                conceptoInfo: row.find('.conceptoInfo').val(),
                                operacion1: unmaskNumero(row.find('.vacFijo').val()),
                                operacion2: unmaskNumero(row.find('.vacDiasTrans').val()),
                                operacion3: unmaskNumero(row.find('.vacRes1').val()),
                                operacion4: unmaskNumero(row.find('.aguiSalDiario').val()),
                                conceptoDetalle: "",
                                resultado: unmaskNumero(row.find('.resultado').val())
                            };
                        } else {
                            if (row.find('.comboConcepto option:selected').val() == 9 && row.find('.conceptoInfo').val() == "") {
                                flagConceptoOTROS = true;
                            }
                            objDetalle = {
                                id: 0,
                                conceptoID: unmaskNumero(row.find('.comboConcepto option:selected').val()),
                                conceptoInfo: row.find('.conceptoInfo').val(),
                                operacion1: 0,
                                operacion2: 0,
                                operacion3: 0,
                                operacion4: 0,
                                conceptoDetalle: row.find('.conceptoDetalle').val(),
                                resultado: unmaskNumero(row.find('.resultado').val())
                            };
                        }
                    }

                    arr.push(objDetalle);
                });
                if (flagConceptoOTROS == false) {
                    if ((comboVobo.val() != "" && comboVobo.data("claveEmpleado")) && (comboAutorizo.val() != "" && comboAutorizo.data("claveEmpleado"))) {
                        $.ajax({
                            url: '/Finiquito/GuardarFiniquito',
                            type: 'POST',
                            data: { general: objGeneral, detalle: arr, fechaBaja: inputFechaEgreso.val() },
                            success: function (response) {
                                var data = response.data;

                                if (data != null) {
                                    AlertaGeneral("Finiquito Capturado", "Se ha guardado la información del finiquito.");
                                    fnClearTable();

                                    verReporte(data.id, data.claveEmpleado, data.voboID);
                                } else {
                                    AlertaGeneral("Alerta", "Ya existe un finiquito capturado para este empleado.");
                                    fnClearTable();
                                }
                            }
                        });
                    } else {
                        AlertaGeneral("Alerta", "Seleccione los usuarios para el VoBo y la autorización del finiquito.")
                    }
                } else {
                    AlertaGeneral("Alerta", 'Agregue una descripción a los conceptos de tipo "OTROS".');
                }
            } else {
                AlertaGeneral("Alerta", "Error al capturar la información general del empleado.");
            }
            //} else {
            //    AlertaGeneral("Alerta", "No se puede guardar el finiquito. El empleado no ha egresado.");
            //}
        }

        function verReporte(idFiniquito, claveEmpleado, usuarioID) {

            //$.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = "77";

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&fId=" + idFiniquito + "&inMemory=1";

            ireport.attr("src", path);

            document.getElementById('report').onload = function () {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/Finiquito/EnviarCorreos',
                    data: { empleadoClave: claveEmpleado, usuarioID: usuarioID },
                    success: function (response) {

                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
                //$.unblockUI();
                //openCRModal();
            };
        }

        function AgregarRenglon() {
            var html = "";
            html += '<tr class="renglon">';
            html += '   <td>';
            html += '       <div class="col-lg-6">';
            html += '           <select class="form-control comboConcepto" class="form-control" style="box-shadow: none;"></select>';
            html += '       </div>';
            html += '       <div class="col-lg-6" style="padding-left: 0px;">';
            html += '           <input type="text" class="form-control text-left conceptoInfo" id="" style="box-shadow: none;" />';
            html += '       </div>';
            html += '   </td>';
            html += '   <td>';
            html += '       <div class="col-lg-12">';
            html += '           <input type="text" class="form-control text-right conceptoDetalle" id="" style="box-shadow: none;" />';
            html += '       </div>';
            html += '   </td>';
            html += '   <td>';
            html += '       <div class="col-lg-12">';
            html += '           <input type="text" class="form-control text-right resultado dinero" value="$0.00" style="box-shadow: none; border-radius: 0px;" />';
            html += '       </div>';
            html += '   </td>';
            html += '</tr>';
            $(html).appendTo($("#tblData tbody"));

            $(".comboConcepto:last").fillCombo('/Finiquito/FillComboConcepto', null, false, null);
        }

        function AgregarRenglones() {
            var html = "";

            html += '<tr>';
            html += '   <td>';
            html += '       <div class="col-lg-12">';
            html += '           <input type="text" class="form-control col-lg-3" id="" value="AGUINALDO" style="border: none; box-shadow: none; background-color: white;" readonly />';
            html += '       </div>';
            html += '   </td>';
            html += '   <td>';
            html += '       <div class="col-lg-12 text-right">';
            html += '           <input type="text" id="aguiFijo" class="form-control text-center inputTablaFiniquito" value="0.00" /> x';
            html += '           <input type="text" id="aguiDiasTrans" class="form-control text-center inputTablaFiniquito" value="0" /> =';
            html += '           <input type="text" id="aguiRes1" class="form-control text-center inputTablaFiniquito" value="0.00" /> x';
            html += '           <input type="text" id="aguiSalDiario" class="form-control text-right inputTablaFiniquito aguiSalDiario dinero" value="$0.00" /> =';
            html += '       </div>';
            html += '   </td>';
            html += '   <td>';
            html += '       <div class="col-lg-12">';
            html += '           <input type="text" id="aguiRes2" class="form-control text-right resultado dinero" value="$0.00" style="background-color: white; border-radius: 0px;" />';
            html += '       </div>';
            html += '   </td>';
            html += '</tr>';

            $(html).appendTo($("#tblData tbody"));
        }

        function RemueveRenglon() {
            $('#tblData tbody .renglon:last').remove();

            fnResultados();
        }

        function selectEmpleado() {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Administrativo/Finiquito/getEmpleado',
                data: { id: txtIgNumeroEmpleado.val() },
                async: false,
                success: function (response) {
                    if (response.success) {
                        viewDatosEmpleados(response.items, response.diasTrans);
                    }
                    else {
                        AlertaGeneral('Alerta', 'No se encontró el número de usuario');

                        fnClearTable();

                        //$("#divDetalleFiniquito").addClass("disabledbutton");
                        //$("#divAutorizaciones").addClass("disabledbutton");
                        //$("#divAcciones").addClass("disabledbutton");
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function viewDatosEmpleados(objEmpleado, diasTrans) {
            var ingresoEmpleado = new Date(objEmpleado.fechaAlta.match(/\d+/)[0] * 1);
            var egresoEmpleado = (objEmpleado.fechaBaja != null) ? new Date(objEmpleado.fechaBaja.match(/\d+/)[0] * 1) : "";

            //LLena Tabla de info general
            lblIgNombre.val(objEmpleado.nombre + " " + objEmpleado.ape_paterno + " " + objEmpleado.ape_materno);
            lblIgIngreso.val(ingresoEmpleado.toLocaleDateString());
            inputFechaEgreso.val((egresoEmpleado != "") ? egresoEmpleado.toLocaleDateString() : "");

            if (egresoEmpleado != "") {
                lblIgTiempo.val((fnRestaFechasDias(ingresoEmpleado, egresoEmpleado) / 365).toFixed(2));
            } else {
                var hoy = new Date();
                lblIgTiempo.val((fnRestaFechasDias(ingresoEmpleado, hoy) / 365).toFixed(2));
            }

            lblIgPuesto.val(objEmpleado.puesto);
            lblIgCentro.val(objEmpleado.cc);

            // Calculo de sueldo tabla informativa
            lblIgSueldoNomina.text('$ ' + objEmpleado.salarioBase);
            lblIgComplementoNomina.text('$ ' + (objEmpleado.complemento));
            lblIgBonoNomina.text('$ ' + objEmpleado.bono);

            if (objEmpleado.tipoNominaID == 1) {
                var bonoMensual = (objEmpleado.bono / 7) * 30.4;
                var sueldoMensual = (objEmpleado.salarioBase / 7) * 30.4;
                var complementoMensual = (objEmpleado.complemento / 7) * 30.4;

                lblIgBonoMensual.text('$ ' + bonoMensual.toFixed(2));

                //lblIgSueldoMensual.val('$ ' + (sueldoMensual.toFixed(2)));
                lblIgSueldoMensual.val(maskNumero(unmaskNumero(sueldoMensual.toString()) + unmaskNumero(complementoMensual.toString()) + unmaskNumero(bonoMensual.toString())));

                lblIgComplementoMensual.text('$ ' + ((complementoMensual.toFixed(2))));
                lblIgTotalNomina.text('$ ' + ((objEmpleado.salarioBase + objEmpleado.complemento + objEmpleado.bono).toFixed(2)));
                lblIgTotalMensual.val('$ ' + ((sueldoMensual + complementoMensual + bonoMensual).toFixed(2)));

            }
            else if (objEmpleado.tipoNominaID == 4) {

                var sueldoMensual = objEmpleado.salarioBase * 2;
                var complementoMensual = objEmpleado.complemento * 2;
                var bonoMensual = objEmpleado.bono * 2;

                //lblIgSueldoMensual.val('$ ' + (sueldoMensual.toFixed(2)));
                lblIgSueldoMensual.val(maskNumero(unmaskNumero(sueldoMensual.toString()) + unmaskNumero(complementoMensual.toString()) + unmaskNumero(bonoMensual.toString())));

                lblIgComplementoMensual.text('$ ' + (complementoMensual.toFixed(2)));
                lblIgBonoMensual.text('$ ' + bonoMensual.toFixed(2));
                lblIgTotalNomina.text('$ ' + ((objEmpleado.salarioBase + objEmpleado.complemento).toFixed(2)));
                lblIgTotalMensual.text('$ ' + ((sueldoMensual + complementoMensual + bonoMensual).toFixed(2)));
            }

            $("#aguiFijo").val(0.04);
            $("#aguiDiasTrans").val(diasTrans);
            $("#aguiRes1").val((unmaskNumero($("#aguiFijo").val()) * unmaskNumero($("#aguiDiasTrans").val())).toFixed(2));
            $(".aguiSalDiario").val(maskNumero(unmaskNumero(lblIgSueldoMensual.val()) / 30.4));
            $("#aguiRes2").val(maskNumero(unmaskNumero($("#aguiRes1").val()) * unmaskNumero($("#aguiSalDiario").val())));

            fnResultados();

            //if (objEmpleado.fechaBaja == null) {
            //    fnClearTable();
            //}

            //if (objEmpleado.fechaBaja != null) {
            //    $("#divDetalleFiniquito").removeClass("disabledbutton");
            //    $("#divAutorizaciones").removeClass("disabledbutton");
            //    $("#divAcciones").removeClass("disabledbutton");
            //} else {
            //    $("#divDetalleFiniquito").addClass("disabledbutton");
            //    $("#divAutorizaciones").addClass("disabledbutton");
            //    $("#divAcciones").addClass("disabledbutton");
            //}
        }

        function fnRestaFechasDias(a, b) {
            var _MS_PER_DAY = 1000 * 60 * 60 * 24;

            // a and b are javascript Date objects
            function dateDiffInDays(a, b) {
                // Discard the time and time-zone information.
                var utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
                var utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());

                return Math.floor((utc2 - utc1) / _MS_PER_DAY);
            }

            var difference = dateDiffInDays(a, b);

            return difference;
        }

        function fnResultados() {
            var sum = 0;
            $(".resultado").each(function (index) {
                var row = $(this).closest('tr');

                var conceptoTipo = "";

                if (index == 0) {
                    conceptoTipo = "True";
                } else {
                    conceptoTipo = row.find('.comboConcepto option:selected').data("prefijo");
                }

                var value = unmaskNumero($(this).val());
                // add only if the value is number
                if (!isNaN(value) && value.length != 0) {
                    if (conceptoTipo == "True") {
                        sum += value;
                    } else {
                        sum -= value;
                    }
                }
            });
            inputTotalFiniquito.val(maskNumero(sum));
        }

        function fnClearTable() {
            $("#aguiFijo").val(0.00);
            $("#aguiDiasTrans").val(0);
            $("#aguiRes1").val(0.00);
            $("#aguiSalDiario").val("$0.00");
            $("#aguiRes2").val("$0.00");

            $('#tblData tbody .renglon').remove();

            inputTotalFiniquito.val("$0.00");
        }

        function setAutoComplete() {
            lblIgNombre.getAutocomplete(setIdEmpleado, null, '/Finiquito/getEmpleados');

            comboFormulo.getAutocomplete(null, null, '/Administrativo/FormatoCambio/getEmpleados');
            $.ajax({
                url: '/Finiquito/getUsuarioNombre',
                success: function (response) {
                    var data = response.data;
                    comboFormulo.data("claveEmpleado", data.userID);
                    comboFormulo.val(data.user);
                    comboFormuloPuesto.val(data.puesto);
                }
            });

            comboVobo.getAutocomplete(fnAsignClaveVobo, null, '/Administrativo/FormatoCambio/getEmpleados');
            //comboVobo.fillCombo('/Finiquito/FillComboEmpleados', null, false, null);

            comboAutorizo.getAutocomplete(fnAsignClaveAutorizo, null, '/Administrativo/FormatoCambio/getEmpleados');

            //comboVobo.val("SYLVIA LILIANA MADRID GARCIA");
            //comboAutorizo.val("ARNULFO ISLAS ENRIQUEZ");
        }

        function fnAsignClaveVobo(event, ui) {
            $.ajax({
                datatype: "json",
                url: '/Administrativo/Finiquito/getEmpleado',
                data: { id: ui.item.id },
                success: function (response) {
                    var data = response.items;
                    comboVobo.data("claveEmpleado", data.claveEmpleado);
                    comboVoboPuesto.val(data.puesto);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function fnAsignClaveAutorizo(event, ui) {
            $.ajax({
                datatype: "json",
                url: '/Administrativo/Finiquito/getEmpleado',
                data: { id: ui.item.id },
                success: function (response) {
                    var data = response.items;
                    comboAutorizo.data("claveEmpleado", data.claveEmpleado);
                    comboAutorizoPuesto.val(data.puesto);
                },
                error: function () {
                    $.unblockUI();
                }
            });

            //comboAutorizoPuesto.val(ui.item.id);
        }

        function setIdEmpleado(event, ui) {
            txtIgNumeroEmpleado.val(ui.item.id);
            txtIgNumeroEmpleado.trigger('change');
        }

        init();
    }

    $(document).ready(function () {
        recursoshumanos.finiquito.captFiniquito = new captFiniquito();
    });
});