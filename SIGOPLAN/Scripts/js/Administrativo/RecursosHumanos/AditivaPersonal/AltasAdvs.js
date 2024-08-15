$(function () {
    $.namespace('recursoshumanos.AditivaPersonal.AltasAdvs');

    AltasAdtvs = function () {
        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        btnAutorizaCck1 = $("#btnAutorizaCck1");
        btnAutorizaCck2 = $("#btnAutorizaCck2");
        btnAutorizaCck3 = $("#btnAutorizaCck3");
        btnSolicitarAp = $("#btnSolicitarAp");
        fechaIni = $("#fechaIni");
        selAutSolicita = $("#selAutSolicita");
        selAutVoBo = $("#selAutVoBo");
        selAutoriza1 = $("#selAutoriza1");
        selAutoriza12 = $("#selAutoriza12");
        selAutoriza2 = $("#selAutoriza2");
        selAutoriza22 = $("#selAutoriza22");
        selAutoriza3 = $("#selAutoriza3");
        selAutoriza32 = $("#selAutoriza32");
        txtCondicionInicial = $("#txtCondicionInicial");
        txtCondicionActual = $("#txtCondicionActual");
        txtOtraJustificacion = $("#txtOtraJustificacion");
        fuEvidencia = $("#fuEvidencia");
        var flagglobalpuesto = false;
        var banderaCat = false;
        var idlocal;
        var renglon;
        var combo;
        var categoria;
        var alta;
        var cantidad;
        var faltante;
        var lugplantilla;
        var personalCc;
        var aditiva;
        var deductiva;
        var Bandera = false;
        var argAutorizaciones = [];
        var objAutorizacion = {
            Clave_Aprobador: 0,
            Nombre_Aprobador: "",
            PuestoAprobador: "",
            Autorizando: false,
            Orden: 0,
            tipoAutoriza: 0
        }
        var totalPuestos = 0;
        var id_renglon = 0;
        var puesto = "";
        cboPuesto = $("#cboPuesto");
        fechaIni = $("#fechaIni");
        btnAgregar = $("#addPlantilla");
        btnPuesto1 = $("#addPuesto");
        btnSalir = $("#btnModalAceptarAditiva");
        btnEliminar = $("#btndrop");
        tblPlantilla = $("#tblPlantilla");
        cboCC = $("#cboCC");
        var plantilla = "<tr class='rowPuesto' >"
            + "<td id='Puesto'>"
            //+ "<select id='cboPuesto' style='width:100%' class='form-control select2'>"
            + "<select id='cboPuesto' class='form-control'>"
            + "<option>--Seleccionar--</option>"
            + "</select>"
            + "</td>"
            + "<td class='categorias'>"
            + 'N/A'
            + "</td>"
            + "<td class='PersonalNecesario'>"
            + '0'
            + "</td>"
            + "<td class='PersonalExistente'>"
            + '0'
            + "</td>"
            + "<td class='PersonalFaltante'>"
            + '0'
            + "</td>"
            + "<td class='lugplantilla'>"
            + '0'
            + "</td>"
            + "<td class='personalcC'>"
            + '0'
            + "</td>"
            + "<td class='aditiva'>"
            + '0'
            + "</td>"
            + "<td  class='deductiva'>"
            + '0'
            + "</td>"
            + "<td  class='justificacion'><textarea class='motivo' disabled style='width:100%'></textarea></td>"
            + "<td><button type='button' style='margin-top:15px; margin-left:8px;' id='btndrop' > <span class='glyphicon glyphicon-trash'></span></button></td>"

            + "</tr>";


        var plantillaPuesto = "<tr class='rowPuesto' >"
            + "<td id='NewPuesto'>"
            // + "<input class='form-control' id='txtpuesto' type='text'>"
            //+ "<select id='cboNuevoPuesto' style='width:100%' class='form-control select2'>"
            + "<select id='cboNuevoPuesto' class='form-control'>"
            + "<option>--Seleccionar--</option>"
            + "</select>"
            + "</td>"
            + "<td class='categorias'>"
            + " <div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + 'N/A' + "</div>"
            + "</td>"
            + "<td>"
            + " <div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + '0' + "</div>"
            + "</td>"
            + "<td>"
            + " <div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + '0' + "</div>"
            + "</td>"
            + "<td>"
            + " <div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + '0' + "</div>"
            + "</td>"
            + "<td>"
            + "<div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + '0' + "</div>"
            + "</td>"
            + "<td class='personalcCPuesto'>"
            + "<input  disabled type='number' style='border:1px solid black; background-color:#C0C0C0' name='numericInput' size='2' min='0' max='200' value='1'>"
            + "</td>"
            + "<td class='aditiva'>"
            + "<input id='adInp' type='number'style='border:1px solid black' name='numericInput' size='2' min='0' max='200' value='" + 1 + "'>"
            + "</td>"
            + "<td>"
            + "<div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + '0' + "</div>"
            + "</td>"
            + "<td  class='justificacionPuesto'><textarea class='motivo'  style='width:100%'></textarea></td>"
            + "<td><button type='button' style='margin-top:15px; margin-left:8px;' id='btndrop'> <span class='glyphicon glyphicon-trash'></span></button></td>"
            + "</tr>";
        var iDFormatoAditiva;
        ireport = $("#report")



        function init() {

            //#region SE OBTIENE LOS PARAMETROS DEL URL
            // const variables = getUrlParams(window.location.href);
            // if (variables && variables.cc && variables.puesto && variables.cantVacantes && variables.idSolicitud && variables.esPuestoNuevo && variables.personalExistente) {
            //     strPuesto = variables.puesto;
            //     idPuesto = variables.idPuesto;
            //     cantVacantes = variables.cantVacantes;
            //     strPersonalExistente = variables.personalExistente;
            //     idSolicitud = variables.idSolicitud;
            //     esPuestoNuevo = variables.esPuestoNuevo;

            //     var clean_uri = location.protocol + "//" + location.host + location.pathname;
            //     window.history.replaceState({}, document.title, clean_uri);
            // }
            //#endregion

            //selAutVoBo.val('MANUEL DE JESUS CRUZ GARCIA').prop('disabled', true);



            // selAutoriza3.val("JOSE MANUEL GAYTAN LIZAMA");
            // selAutoriza3.data("id", 4);
            // selAutoriza3.data("nombre", "JOSE MANUEL GAYTAN LIZAMA");
            // objAutorizacion = {
            //     Clave_Aprobador: 4,
            //     Nombre_Aprobador: 'JOSE MANUEL GAYTAN LIZAMA',
            //     PuestoAprobador: "Alta Dirección 1",
            //     Responsable: "Autorización 3",
            //     Orden: 6,
            //     tipoAutoriza: false
            // };
            // argAutorizaciones.push(objAutorizacion);

           /* selAutVoBo.val("MANUEL DE JESUS CRUZ GARCIA");
            selAutVoBo.data("id", 1041);
            selAutVoBo.data("nombre", "MANUEL DE JESUS CRUZ GARCIA");
            objAutorizacion = {
                Clave_Aprobador: 1041,
                Nombre_Aprobador: 'MANUEL DE JESUS CRUZ GARCIA',
                PuestoAprobador: "Capital Humano",
                Responsable: "VoBo",
                Orden: 9,
                tipoAutoriza: false
            };*/
            argAutorizaciones.push(objAutorizacion);

            setAutorizacion();

            btnAutorizaCck1.prop('checked', true);
            btnAutorizaCck2.prop('checked', true);
            btnAutorizaCck3.prop('checked', true);
            //cboCC.select2();
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");

            const params = new Proxy(new URLSearchParams(window.location.search), {
                get: (searchParams, prop) => searchParams.get(prop),
            });

            if (params.cc != null) {
                cboCC.val(params.cc);
                cboCC.trigger("change");

                if (params.esPuestoNuevo) {
                    Llenado();

                    puestoCbo = $('tr[id-renglon="' + 0 + '"]').find('td select');

                    puestoCbo.val(params.idPuesto);
                    puestoCbo.trigger("change");

                } else {
                    LlenadoPuesto();

                    puestoCbo = $('tr[id-renglon="' + 0 + '"]').find('td select');

                    puestoCbo.val(params.idPuesto);
                    puestoCbo.trigger("change");

                }
            }
            console.log(params.cc);

            //#region GESTIÓN DE SOLICITUDES
            // cboCC.val(variables.cc);
            // cboCC.trigger("change");
            //#endregion

            var hoy = new Date();
            var dd = hoy.getDate();
            var mm = hoy.getMonth() + 1;
            var yyyy = hoy.getFullYear();
            fechaIni.datepicker().datepicker("setDate", dd + "/" + mm + "/" + yyyy);
            $(fechaIni).attr('disabled', true);
            btnAgregar.click(Llenado);
            btnPuesto1.click(LlenadoPuesto);

            //#region GESTIÓN DE SOLICITUDES
            // if (variables.idSolicitud >= 0 && variables.esPuestoNuevo == "true") {
            //     btnPuesto1.click(fncNuevoPuesto());
            // } else if (variables.idSolicitud > 0 && variables.esPuestoNuevo == "false") {
            //     btnAgregar.click(fncAgregarPuesto());
            // }
            //#endregion

            $("#cboCC").focus(function () {
                if ($('#tablaPadre >tbody >tr').length > 0) {
                    $('#cboCC').attr('disabled', 'true');
                    ConfirmacionModal("Alerta", "Se perdera la informacion Actual ¿Desea Continuar?");
                }
            });
            $(document).on('click', '.close', function () {
                $("#cboCC").attr("disabled", false);
            });
            var numInput = document.querySelector('input');
            numInput.addEventListener('input', function () {
                var num = this.value.match(/^\d+$/);
                if (num === null) {
                    this.value = "";
                }
            }, false)
            $(document).on('keyup', '#adInp', function () {
                AditivaValor = (this.value);
                idActual = $(this).closest('tr').attr('id-renglon');
                var Data = DataTotalExixtente(idActual);
                if (AditivaValor != "") {
                    var iNumAditiva = parseInt(AditivaValor);
                    resultado = (iNumAditiva + parseInt(Data));
                    personalCc = $('tr[id-renglon="' + idActual + '"]').find("td").eq(6);
                    personalCc.empty();
                    personalCc.append(" <input disabled type='number'style='border:1px solid black; background-color:#C0C0C0' name='numericInput' size='2' min='0' max='200' value='" + resultado + "'>");
                    resultado = 0;
                    if (iNumAditiva > 0) {
                        deductiva = $('tr[id-renglon="' + idActual + '"]').find("td.deductiva input").attr("disabled", "true");
                    } else {
                        deductiva = $('tr[id-renglon="' + idActual + '"]').find("td.deductiva input").prop("disabled", false);
                    }

                } else {
                    deductiva = $('tr[id-renglon="' + idActual + '"]').find("td.deductiva input").prop("disabled", false);
                }
            });
            $(document).on('keyup', '#adDed', function () {
                idActual = $(this).closest('tr').attr('id-renglon');
                var iNumAditiva = $('tr[id-renglon="' + idActual + '"]').find("td.aditiva input").val();
                var Data = DataTotalExixtente(idActual);
                var iNumDeductiva = parseInt($('tr[id-renglon="' + idActual + '"]').find("td.deductiva input").val());
                resultado = (parseInt(Data) - iNumDeductiva);
                if (iNumAditiva == 0) {
                    deductiva = $('tr[id-renglon="' + idActual + '"]').find("td.deductiva input").prop("disabled", false);
                    personalCc = $('tr[id-renglon="' + idActual + '"]').find("td").eq(6);
                    personalCc.empty();
                    personalCc.append(" <input disabled type='number'style='border:1px solid black; background-color:#C0C0C0' name='numericInput' size='2' min='0' max='200' value='" + resultado + "'>");
                    resultado = 0;
                    if (iNumDeductiva > 0) {
                        aditiva = $('tr[id-renglon="' + idActual + '"]').find("td.aditiva input").attr("disabled", "true");
                    } else {
                        aditiva = $('tr[id-renglon="' + idActual + '"]').find("td.aditiva input").prop("disabled", false);
                    }
                }
            });
            $(document).on('blur', '#adDed', function () {
                idActual = $(this).closest('tr').attr('id-renglon');
                DeductivaValor = (this.value);
                var Data = DataTotalExixtente(idActual);
                if (DeductivaValor == "") {
                    deductiva = $('tr[id-renglon="' + idActual + '"]').find("td").eq(8);
                    deductiva.empty();
                    deductiva.append(" <input id='adDed' type='number'style='border:1px solid black' name='numericInput' size='2' min='0' max='200' value='0'  oninput=\"validity.valid||(value='');\">");
                    personalCc = $('tr[id-renglon="' + idActual + '"]').find("td").eq(6);
                    personalCc.empty();
                    personalCc.append(" <input disabled type='number'style='border:1px solid black; background-color:#C0C0C0' name='numericInput' size='2' min='0' max='200' value='" + Data + "'>");
                }
            });
            $(document).on('blur', '#adInp', function () {
                idActual = $(this).closest('tr').attr('id-renglon');
                AditivaValor = (this.value);
                var Data = DataTotalExixtente(idActual);
                if (AditivaValor == "") {
                    aditiva = $('tr[id-renglon="' + idActual + '"]').find("td").eq(7);
                    aditiva.empty();
                    aditiva.append(" <input id='adInp' type='number'style='border:1px solid black' name='numericInput' size='2' min='0' max='200' value='0' oninput=\"validity.valid||(value='');\">");
                    personalCc = $('tr[id-renglon="' + idActual + '"]').find("td").eq(6);
                    personalCc.empty();
                    personalCc.append(" <input disabled type='number'style='border:1px solid black; background-color:#C0C0C0' name='numericInput' size='2' min='0' max='200' value='" + Data + "'>");
                }
            });
            setAutoComplete();
            selAutoriza1.change();
            btnSolicitarAp.click(carga);
            idAditivaDeductivaUpdate();
            iDFormatoAditiva = $.get("id");
            if (iDFormatoAditiva > 0) {
                selectAditivaEditar(iDFormatoAditiva);
                $("#cboCC").attr("disabled", "true");
            }
            banderaCat = true;
            banderapuestoigual = false;
            $(document).on('keyup', '#txtpuesto', function () {
                var puestoActual = $(this).closest('tr').find('td#NewPuesto input').val();
                var idpuestoActual = $(this).closest('tr').attr('id-renglon');
                var longitud = $("#tablaPadre >tbody >tr").length
                for (var i = 0; i <= longitud; i++) {
                    renglon = $("#tablaPadre >tbody >tr")[i]
                    puesto = $(renglon).find('td#NewPuesto input').val();
                    if (puestoActual == puesto && idpuestoActual != i) {
                        banderapuestoigual = true;
                        $(this).closest('tr').find('td#NewPuesto input').attr('style', 'color:red');
                    } else if (banderapuestoigual == false) {
                        $(this).closest('tr').find('td#NewPuesto input').attr('style', 'color:black');
                    }
                }
                flagglobalpuesto = banderapuestoigual;
                banderapuestoigual = false;
            });
        }

        //#region GESTIÓN DE SOLICITUDES
        // function fncNuevoPuesto() {
        //     btnPuesto1.trigger("click");
        // }
        // function fncAgregarPuesto() {
        //     btnAgregar.trigger("click");
        // }
        // function fncKeyUpTxtAditiva() {
        //     $("#adInp").trigger("keyup");
        //     $(".txtpersonalcCPuesto").val(cantVacantes);
        // }
        // function fncKeyUpTxtAditivaPuestoExistente() {
        //     $("#adInp").trigger("keyup");
        //     console.log("111231123");
        // }
        //#endregion

        function setAutorizacion() {
            objAutorizacion = {
                Clave_Aprobador: 4,
                Nombre_Aprobador: 'JOSE MANUEL GAYTAN LIZAMA',
                PuestoAprobador: "Responsable del Centro de Costos",
                Responsable: "Solicitante",
                Autorizando: true,
                Orden: 1,
                tipoAutoriza: false
            };
            argAutorizaciones.push(objAutorizacion);
        }


        function focusFunction() {
            document.getElementById("myInput").style.background = "yellow";
        }
        function blurFunction() {
            document.getElementById("myInput").style.background = "red";
        }
        var campo = null;
        function valida(val) {
            if (campo != null) {
                if (campo != val) {
                    alert('Ambos campos deben ser iguales!');
                    campo = null;
                    return false;
                } else { return true; }
            } else {
                campo = val;
                return;
            }
        }
        function idAditivaDeductivaUpdate() {
            (function ($) {
                $.get = function (key) {
                    key = key.replace(/[\[]/, '\\[');
                    key = key.replace(/[\]]/, '\\]');
                    var pattern = "[\\?&]" + key + "=([^&#]*)";
                    var regex = new RegExp(pattern);
                    var url = unescape(window.location.href);
                    var results = regex.exec(url);
                    if (results === null) {
                        return null;
                    } else {
                        return results[1];
                    }
                }
            })(jQuery);
        }
        function selectAditivaEditar(id) {
            var objEditar;
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Administrativo/AditivaPersonal/getAditivadeductivaEditar',
                data: { id: id },
                async: false,
                success: function (response) {
                    if (response.success) {
                        $("#cboCC option:selected").text(response.AditivaDeductiva[0].cC);
                        $("#cboCC option:selected").val(response.AditivaDeductiva[0].cCid);
                        FormateoFechaEdicion(response.AditivaDeductiva[0].fecha_Alta);
                        selectAprobaciones(response.AditivaDeductiva[0].id);
                        arrPuestonuevoEdit = [];
                        arrPuestoEdit = [];
                        response.Detalle.forEach(function (valorDet, indiceDet, arrayDet) {
                            if (valorDet.nuevo == true) {
                                arrPuestonuevoEdit.push(valorDet);
                            } else {
                                arrPuestoEdit.push(valorDet);
                            }
                        });
                        response.Detalle = [];
                        if (arrPuestoEdit.length > 0) {
                            DibujadoCeldasEdicion(arrPuestoEdit);
                        }
                        if (arrPuestonuevoEdit.length > 0) {
                            arrPuestonuevoEdit.forEach(function (valorDet, indiceDet, arrayDet) {
                                var longitud = $("#tablaPadre >tbody >tr").length
                                $("#tblPlantilla").append(plantillaPuesto);
                                renglon = $("#tablaPadre >tbody >tr")[longitud]
                                $(renglon).find('td#NewPuesto input').val(valorDet.puesto);
                                $(renglon).find('td.categorias').empty();
                                $(renglon).find('td.categorias').append(" <div class='pn'  id-detalle='" + valorDet.id + "' style='border:1px solid black'>N/A</div>");
                                $(renglon).find("td.personalcCPuesto").empty();
                                $(renglon).find("td.personalcCPuesto").append(" <input disabled type='number'style='border:1px solid black; background-color:#C0C0C0' name='numericInput' size='2' min='0' max='200' value='" + valorDet.numPersTotal + "'>");
                                $(renglon).find('td.aditiva input').val(valorDet.aditiva);
                                if (valorDet.justificacion == null) {
                                    valorDet.justificacion = "";
                                }
                                $(renglon).find('td.justificacionPuesto textarea').val(valorDet.justificacion);
                                banderaPuesto = true;
                                OrderId(longitud)
                            });
                        }
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        var indiPuesto = "";
        var Puestotmp = "";
        var BanderaEdit = false;
        var arrayEditPuesto = [];
        var arrayIDdetalle = [];
        function DibujadoCeldasEdicion(Detalle) {
            BanderaEdit = true;
            Detalle.forEach(function (valorDet, indiceDet, arrayDet) {
                var puestoEdit = valorDet.puesto;
                objidDetalle = { id_detalle: valorDet.id, puesto: puestoEdit };
                arrayIDdetalle.push(objidDetalle);
                arrayEditPuesto.push(puestoEdit);
            });
            arrayEditPuesto = eliminateDuplicates(arrayEditPuesto);
            arrayEditPuesto.forEach(function (valorPue, indicePue, arrayPue) {
                indiPuesto = indicePue;
                Puestotmp = valorPue;
                Llenado();
            });
            var longarrayEditPuesto = arrayEditPuesto.length;
            var longDetalle = Detalle.length;
            if (longarrayEditPuesto > longDetalle) {
                arrayEditPuesto.forEach(function (valorPueAux, indicePueAux, arrayPueAux) {
                    Detalle.forEach(function (valorAux, indiceDetAux, arrayDetAux) {
                        if (valorAux.puesto.trim() == valorPueAux.trim()) {
                            Detalle.splice(indiceDetAux, 1)
                        }
                    });
                });
            }
            Detalle.forEach(function (valorAux, indiceDetAux, arrayDetAux) {
                var longitud = $("#tablaPadre >tbody >tr").length
                for (var i = 0; i < longitud; i++) {
                    renglon = $("#tablaPadre >tbody >tr")[i]
                    var Puesto = $('tr[id-renglon="' + i + '"]').find('td option:selected').text();
                    if (Puesto == valorAux.puesto) {
                        $('tr[id-renglon="' + i + '"]').find('td.aditiva').empty();
                        $('tr[id-renglon="' + i + '"]').find('td.aditiva').append("<input id='adInp' type='number'style='border:1px solid black' name='numericInput' size='2' min='0' max='200' value='" + valorAux.aditiva + "' oninput=\"validity.valid||(value='');\">");
                        $('tr[id-renglon="' + i + '"]').find('td.personalcC').empty();
                        $('tr[id-renglon="' + i + '"]').find('td.personalcC').append(" <input disabled type='number'style='border:1px solid black; background-color:#C0C0C0' name='numericInput' size='2' min='0' max='200' value='" + valorAux.numPersTotal + "'>");
                        $('tr[id-renglon="' + i + '"]').find('td.deductiva').empty();
                        $('tr[id-renglon="' + i + '"]').find('td.deductiva').append("<input id='adDed' type='number'style='border:1px solid black' name='numericInput' size='2' min='0' max='200' value='" + valorAux.deductiva + "' oninput=\"validity.valid||(value='');\">");
                        if (valorAux.justificacion == null) {
                            valorAux.justificacion = "";
                        }
                        $('tr[id-renglon="' + i + '"]').find('td.justificacion').empty();
                        $('tr[id-renglon="' + i + '"]').find('td.justificacion').append("<textarea class='motivo'   style='width:100%'>" + valorAux.justificacion + "</textarea></td>");
                        OrderId(longitud, true);
                        if (valorAux.aditiva > 0) {
                            $('tr[id-renglon="' + i + '"]').find('td.deductiva #adDed').prop('disabled', 'true');
                        } else if (valorAux.deductiva > 0) {
                            $('tr[id-renglon="' + i + '"]').find('td.aditiva #adInp').prop('disabled', 'true');
                        }
                    }
                }
            });
            BanderaEdit = false;
        }
        function eliminateDuplicates(arrayEditPuesto) {
            var i,
                len = arrayEditPuesto.length,
                out = [],
                obj = {};

            for (i = 0; i < len; i++) {
                obj[arrayEditPuesto[i]] = 0;
            }
            for (i in obj) {
                out.push(i);
            }
            return out;
        }
        function FormateoFechaEdicion(Fecha) {
            var dateString = Fecha.substr(6);
            var currentTime = new Date(parseInt(dateString));
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var date = day + "/" + month + "/" + year;
            fechaIni.val(date);
        }
        function DataTotalExixtente(idActual) {
            var intHijExistente = 0;
            personalExistente = $('tr[id-renglon="' + idActual + '"]').find('td.PersonalNecesario div.pn');
            var lonpersonalExistente = personalExistente.length;
            for (var c = 0; c < lonpersonalExistente; c++) {
                hijpersonalExistente = personalExistente[c];
                intHijo = parseInt($(hijpersonalExistente).html());
                intHijExistente = (intHijExistente + intHijo);
            }
            return intHijExistente;
        }
        var BanderaPlantilla = false;
        function Llenado() {
            BanderaPlantilla = true;
            if ("Todos" == $("#cboCC option:selected").text()) {
                AlertaGeneral('Alerta', 'Favor de Seleccionar Un Centro De Costos');
            } else {
                CargaPuestos();
                $(combo).change(function () {
                    //if (true) {
                    //    eliminadolista($(this).closest('tr').find('td.categorias div.pn'), idEliminadoEdit);
                    //}
                    var trid = $(this).closest('tr').attr('id-renglon');
                    Categorizar(trid);
                    var puestoValue = $(this).closest('tr').find('select').val();
                })
                if (BanderaEdit == true) {
                    var valorPuesto = $('tr[id-renglon="' + indiPuesto + '"] option').filter(function () { return $(this).html() == Puestotmp; }).val()
                    $('tr[id-renglon="' + indiPuesto + '"]').find('td select').val(valorPuesto)
                    Categorizar(indiPuesto);
                }
                // $("#cboPuesto").val(idPuesto);
                // $("#cboPuesto").trigger("change");

                // $("#adInp").val(parseFloat(cantVacantes));
                // let adInpCantVacantes = $("#adInp").val();
                // if (adInpCantVacantes > 0) {
                //     $("#adInp").trigger("keyup");
                // } else {
                //     $("#adInp").keyup();
                // }
            }
        }

        var banderaPuesto = false;
        let origCat = " <div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + 'N/A' + "</div>";
        let origCant = " <div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + '0' + "</div>";
        let origAlta = " <div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + '0' + "</div>";
        let origFalt = " <div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + '0' + "</div>";
        let origLugPlant = "<div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + '0' + "</div>";
        let origPerso = "<input  disabled type='number' style='border:1px solid black; background-color:#C0C0C0' name='numericInput' size='2' min='0' max='200' value='1'>";
        let origAdit = "<input id='adInp' type='number'style='border:1px solid black' name='numericInput' size='2' min='0' max='200' value='" + 1 + "'>";
        let origDeduc = "<div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + '0' + "</div>";
        let origJust = "<textarea class='motivo'  style='width:100%'></textarea>";
        function LlenadoPuesto() {
            BanderaPlantilla = true;
            if ("Todos" == $("#cboCC option:selected").text()) {
                AlertaGeneral('Alerta', 'Favor de Seleccionar Un Centro De Costos');
            } else {

                CargaAllPuestos();

                $(combo).change(function () {
                    var trid = $(this).closest('tr').attr('id-renglon');

                    // console.log(combo);

                    let option = ($('tr[id-renglon="' + trid + '"]').find('td option:selected'));
                    if (option.data("cc") != "" && option.data("cc") != null && option.data("cc") != "N") {
                        if (true) {
                            eliminadolista($(this).closest('tr').find('td.categorias div.pn'), idEliminadoEdit);
                        }
                        // CategorizarNuevoPuesto(trid); 
                        var puestoValue = $(this).closest('tr').find('select').val();
                    } else {
                        categoria = $('tr[id-renglon="' + trid + '"]').find("td").eq(1);
                        categoria.html(origCat);
                        cantidad = $('tr[id-renglon="' + trid + '"]').find("td").eq(2);
                        cantidad.html(origCant);
                        alta = $('tr[id-renglon="' + trid + '"]').find("td").eq(3);
                        alta.html(origAlta);
                        faltante = $('tr[id-renglon="' + trid + '"]').find("td").eq(4);
                        faltante.html(origFalt);
                        lugplantilla = $('tr[id-renglon="' + trid + '"]').find("td").eq(5);
                        lugplantilla.html(origLugPlant);
                        personalCc = $('tr[id-renglon="' + trid + '"]').find("td").eq(6);
                        personalCc.html(origPerso);
                        aditiva = $('tr[id-renglon="' + trid + '"]').find("td").eq(7);
                        aditiva.html(origAdit);
                        deductiva = $('tr[id-renglon="' + trid + '"]').find("td").eq(8);
                        deductiva.html(origDeduc);
                        justificacion = $('tr[id-renglon="' + trid + '"]').find("td").eq(9);
                        justificacion.html(origJust);
                    }


                });
                // if (BanderaEdit == true) {
                //     var valorPuesto = $('tr[id-renglon="' + indiPuesto + '"] option').filter(function () { return $(this).html() == Puestotmp; }).val()
                //     $('tr[id-renglon="' + indiPuesto + '"]').find('td select').val(valorPuesto)
                //     CategorizarNuevoPuesto(indiPuesto);
                // }

                //#region VERSION ORIG
                // $("#tblPlantilla").append(plantillaPuesto);
                // var longitud = $("#tablaPadre >tbody >tr").length
                // OrderId(longitud);
                //#endregion

                // $("#txtpuesto").val(strPuesto);
                // $("#adInp").val(cantVacantes);
                // $("#adInp").keyup(fncKeyUpTxtAditiva());
            }
        }
        function deshabilitado() {
            $(this).closest('tr').remove();
            $(btnPuesto).prop('disabled', false);
            $(btnAgregar).prop('disabled', false);
            banderaPuesto = false;
        };
        var idEliminadoEdit;
        $('#tablaPadre').on('focus', 'tr td #btndrop', function (evt) {
            idEliminadoEdit = $(this).closest('tr').attr('id-renglon');
            EliminadoModal("Alerta", "Se perdera la informacion Relacionada Al puesto ¿Desea Continuar?");
        });
        $(document).on('click', "#btnModalAceptarEliminar", function () {
            $('#btndrop').click();
        });
        $('#tablaPadre').on('click', 'tr td #btndrop', function (evt) {
            eliminadolista($(this).closest('tr').find('td.categorias div.pn'), idEliminadoEdit);
            $('tr[id-renglon="' + idEliminadoEdit + '"]').remove();
            var longitud = $("#tablaPadre >tbody >tr").length
            Bandera = true;
            if (longitud != undefined) {
                OrderId(longitud, Bandera);
            }
        });
        var idActualdetModal;
        var iddetModal;
        $(document).on('focus', '#cboPuesto', function () {
            idActualdetModal = $(this).closest('tr').attr('id-renglon');
            iddetModal = $('tr[id-renglon="' + idActualdetModal + '"]').find('td.categorias div.pn').attr('id-detalle');
            if (iddetModal != undefined) {
                var a = $('tr[id-renglon="' + idActualdetModal + '"]').find('td#Puesto select#cboPuesto').attr('disabled', true);
                DetalleModal("Alerta", "Se perdera la informacion Del puesto Actual ¿Desea Cambiar Puesto?");
            }
            var puestotxt = $(this).find("option:selected").text();
        });
        $(document).on('click', "#btnModalAceptarDetModal", function () {
            idActual = $('tr[id-renglon="' + idActualdetModal + '"]').attr('id-renglon');
            eliminadolista($('tr[id-renglon="' + idActualdetModal + '"]').find('td.categorias div.pn'), idActual);
            iddetModal = $('tr[id-renglon="' + idActualdetModal + '"]').find('td.categorias div.pn').removeAttr('id-detalle');
            var b = $('tr[id-renglon="' + idActual + '"]').find('td#Puesto select#cboPuesto').attr('disabled', false);

        });
        var arraylstElimina = [];
        var objEliminaId;
        function eliminadolista(objeto, idrenglon) {
            var longitud = $(objeto).length
            var arrayEnvioObj = [];
            idDet = $('tr[id-renglon="' + idrenglon + '"]').find('td.categorias div.pn');
            var lonDEt = idDet.length;
            for (var cid = 0; cid < lonDEt; cid++) {
                hijoidDet = idDet[cid];
                objEliminaId = { id: $(hijoidDet).attr('id-detalle') }
                if (objEliminaId.id != undefined) {
                    arraylstElimina.push(objEliminaId);
                }
            }
        }
        $(document).on('click', "#btnModalAceptarAditiva", function () {
            combo = undefined;
            $("#tablaPadre  >tbody >tr").remove();
            $('#cboCC').removeAttr('disabled');
        });
        $(document).on('click', "#btnCancelarAditiva", function () {
            $('#cboCC').removeAttr('disabled');
        });
        function CargaPuestos() {
            cC = $("#cboCC option:selected").val();
            $.ajax({
                url: '/Administrativo/AditivaPersonal/getPuestos',
                type: 'POST',
                async: false,
                dataType: 'json',
                data: { cC: cC },
                success: function (data) {
                    rrecorrer(data.length);
                    $.each(data, function (index, item) {
                        $(combo).append($('<option>', {
                            value: item.puesto,
                            text: item.descripcion,
                            data: { cc: item.cc }

                        }));
                    })
                    InhabilitarPuestosAgregados(data)
                }
            });
        }

        function CargaAllPuestos() {
            cC = $("#cboCC option:selected").val();
            $.ajax({
                url: '/Administrativo/AditivaPersonal/getAllPuestos',
                type: 'POST',
                async: false,
                dataType: 'json',
                data: { cC: cC },
                success: function (data) {
                    rrecorrerNuevoPuesto(data.length);
                    $.each(data, function (index, item) {
                        $(combo).append($('<option>', {
                            value: item.puesto,
                            text: item.descripcion,
                            data: { cc: item.cc }
                        }));
                    })
                    //InhabilitarPuestosAgregados(data)
                }
            });
        }

        function InhabilitarPuestosAgregados(data) {
            var longitud = $("#tablaPadre >tbody >tr").length
            for (var i = 0; i <= longitud; i++) {
                renglon = $("#tablaPadre >tbody >tr")[i]
                var Puesto = $('tr[id-renglon="' + i + '"]').find('td select').val();
                if (Puesto == undefined) {
                    Puesto = $('tr[id-renglon="' + i + '"]').find('option[disabled]:selected').val();
                }
                $.each(data, function (index, item) {
                    if (Puesto == item.puesto) {
                        $("#cboPuesto option[value='" + Puesto + "']").attr("disabled", "disabled");
                    };
                })
            };
        }
        function Categorizar(trid) {
            combo = $('tr[id-renglon="' + trid + '"]').find('td option:selected');
            categoria = $('tr[id-renglon="' + trid + '"]').find("td").eq(1);
            cantidad = $('tr[id-renglon="' + trid + '"]').find("td").eq(2);
            alta = $('tr[id-renglon="' + trid + '"]').find("td").eq(3);
            faltante = $('tr[id-renglon="' + trid + '"]').find("td").eq(4);
            lugplantilla = $('tr[id-renglon="' + trid + '"]').find("td").eq(5);
            personalCc = $('tr[id-renglon="' + trid + '"]').find("td").eq(6);
            aditiva = $('tr[id-renglon="' + trid + '"]').find("td").eq(7);
            deductiva = $('tr[id-renglon="' + trid + '"]').find("td").eq(8);
            justificacion = $('tr[id-renglon="' + trid + '"]').find("td").eq(9);
            if (combo == undefined) {
                cC = $("#cboCC option:selected").val();
            }
            $.ajax({
                url: '/Administrativo/AditivaPersonal/getCategorias',
                type: 'POST',
                dataType: 'json',
                async: false,
                data: { Puesto: combo.text().trim(), cC: cC },
                success: function (response) {
                    ModificarCategoria(response);
                },
                error: function (mensaje) {
                    alert(mensaje);
                }
            });
        };

        function CategorizarNuevoPuesto(trid) {
            combo = $('tr[id-renglon="' + trid + '"]').find('td option:selected');
            categoria = $('tr[id-renglon="' + trid + '"]').find("td").eq(1);
            cantidad = $('tr[id-renglon="' + trid + '"]').find("td").eq(2);
            alta = $('tr[id-renglon="' + trid + '"]').find("td").eq(3);
            faltante = $('tr[id-renglon="' + trid + '"]').find("td").eq(4);
            lugplantilla = $('tr[id-renglon="' + trid + '"]').find("td").eq(5);
            personalCc = $('tr[id-renglon="' + trid + '"]').find("td").eq(6);
            aditiva = $('tr[id-renglon="' + trid + '"]').find("td").eq(7);
            deductiva = $('tr[id-renglon="' + trid + '"]').find("td").eq(8);
            justificacion = $('tr[id-renglon="' + trid + '"]').find("td").eq(9);
            if (combo == undefined) {
                cC = $("#cboCC option:selected").val();
            }
            $.ajax({
                url: '/Administrativo/AditivaPersonal/getCategorias',
                type: 'POST',
                dataType: 'json',
                async: false,
                data: { Puesto: combo.text().trim(), cC: $("#cboCC option:selected").val() },
                success: function (response) {
                    ModificarCategoria(response);
                },
                error: function (mensaje) {
                    alert(mensaje);
                }
            });
        };

        var SumaCantidad = 0;
        var SumaNecesario = 0;
        var baderaIdDetalle = false;
        function ModificarCategoria(data) {
            var valEdit = $('tr').find("td.categorias div").attr('id-detalle');
            if (iDFormatoAditiva != 0) {
                borrado();
                $.each(data, function (index, item) {
                    var PersonalFaltanteCal = calculo(item.altas, item.cantidad);
                    $(combo).append($('<option>', {
                        value: item.puesto,
                        text: item.descripcion
                    }));
                    //ajuste para la categoria 18/12/17 por cada categoria agregar el id primero
                    if (arrayIDdetalle.length == 0) {
                        categoria.append(" <div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + item.categoria + "</div>");
                    } else if (banderaCat == true) {
                        categoria.append(" <div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + item.categoria + "</div>");
                    }
                    if (arrayIDdetalle.length != undefined) {
                        $.each(arrayIDdetalle, function (indexDetalle, itemDetalle) {
                            var textopuesto = $('tr[id-renglon="' + indiPuesto + '"]').find('td option:selected').text();
                            if (itemDetalle != undefined) {
                                if (textopuesto == itemDetalle.puesto && baderaIdDetalle == false) {
                                    categoria.append(" <div class='pn'; background-color:#C0C0C0' id-detalle='" + itemDetalle.id_detalle + "' style='border:1px solid black'>" + item.categoria + "</div>");
                                    arrayIDdetalle.splice(indexDetalle, 1);
                                    baderaIdDetalle = true;
                                }
                            }
                        });
                        baderaIdDetalle = false;
                    }
                    alta.append(" <div class='pn'  style='border:1px solid black; background-color:#C0C0C0'>" + item.altas + "</div><div class='pn' style='border:1px solid black; background-color:#C0C0C0'>" + item.categoria + "</div>");
                    cantidad.append(" <div class='pn' style='border:1px solid black; background-color:#C0C0C0'>" + item.cantidad + "</div>");

                    if (PersonalFaltanteCal < 0) {
                        faltante.append(" <div class='pn' style='border:1px dashed  black ; background-color:#C0C0C0'>" + (PersonalFaltanteCal) + "</div>");
                    } else {
                        faltante.append(" <div class='pn' style='border:1px solid black; background-color:#C0C0C0'>" + (PersonalFaltanteCal) + "</div>");
                    }
                    //aditiva.append(" <input type='number'style='border:1px solid black' name='numericInput' size='1' min='0' max='200' value='0'> </br>")
                    //deductiva.append(" <input type='number'style='border:1px solid black' name='numericInput' size='1' min='0' max='200' value='0'> </br>")
                    //44justificacion.append("<textarea class='motivo'  style='width:100%'></textarea>")
                    SumaNecesario = (SumaNecesario + item.cantidad);
                    SumaCantidad = (SumaCantidad + item.cantidad);
                    //SumaCantidad = (SumaCantidad + item.altas);
                    if (data.length == (index + 1)) {
                        ultimoRenglon(SumaNecesario, SumaCantidad);
                        SumaCantidad = 0;
                        SumaNecesario = 0;
                    }

                });
                //}
            }


        };
        var ultimoRenglon = function (SumNecesario, SumaCantidad) {
            personalCc.append(" <input disabled type='number'style='border:1px solid black; background-color:#C0C0C0' name='numericInput' size='2' min='0' max='200' value='" + SumaCantidad + "'>")
            lugplantilla.append("<div class='pn' style='border:1px solid black; background-color:#C0C0C0'>" + SumNecesario + "</div>");
            var ejemplo4 = 'I\'m giving you an example';
            aditiva.append("<input id='adInp' type='number'style='border:1px solid black; ' name='numericInput' size='2' min='0' max='200' value='0' oninput=\"validity.valid||(value='');\">");
            deductiva.append("<input id='adDed' type='number'style='border:1px solid black;' name='numericInput' size='2' min='0' max='200' value='0' oninput=\"validity.valid||(value='');\">");
            justificacion.append("<textarea class='motivo'  style='width:100%;'></textarea></td>")
        }
        var borrado = function () {
            categoria.empty()
            alta.empty();
            cantidad.empty();
            cantidad.empty();
            faltante.empty();
            lugplantilla.empty();
            personalCc.empty();
            aditiva.empty();
            deductiva.empty();
            justificacion.empty();
        }
        var calculo = function (cantidad, altas) {
            if (cantidad < altas) {
                PersonalFaltante = (altas - cantidad);
            } else {
                PersonalFaltante = (altas - cantidad);
            }
            return PersonalFaltante;
        }

        function rrecorrer(cC) {
            var longitud = $("#tablaPadre >tbody >tr").length
            if (cC >= longitud + 1) {
                $("#tblPlantilla").append(plantilla);
                //$(".select2").select2({ width: 'resolve' });
            }
            OrderId(longitud);
        };

        function rrecorrerNuevoPuesto(cC) {
            var longitud = $("#tablaPadre >tbody >tr").length
            if (cC >= longitud + 1) {
                $("#tblPlantilla").append(plantillaPuesto);
                //$(".select2").select2({ width: 'resolve' });

            }
            OrderId(longitud);
        };

        function OrderId(longitud) {
            for (var i = 0; i <= longitud; i++) {
                renglon = $("#tablaPadre >tbody >tr")[i]
                $(renglon).attr("id-renglon", i);
                if ((longitud) == i) {
                    combo = $('tr[id-renglon="' + i + '"]').find('td select');
                }
            };
        };



        //#region GESTIÓN DE SOLICITUDES
        // var strPuesto;
        // var idPuesto
        // var cantVacantes;
        // var idSolicitud;
        // var esPuestoNuevo;
        // var strPersonalExistente;
        //#endregion

        var id_AditivaDeductiva;
        var categoria;
        var personalNecesario;
        var personalExistente;
        var personalFaltante;
        var lugaresPlantilla;
        var numPersTotal;
        var aditiva;
        var deductiva;
        var justificacion;
        var ArregloAditivaDeductiva = [];
        function carga() {
            var longitud = $("#tablaPadre >tbody >tr").length
            var arrayEnvioObj = [];
            for (var j = 0; j < longitud; j++) {
                renglon = $("#tablaPadre >tbody >tr")[j];
                var tipoPuesto = $(renglon).find('td').attr('id');

                if (tipoPuesto == "Puesto") {
                    arrCategorias = [];
                    categorias = $('tr[id-renglon="' + j + '"]').find('td.categorias div.pn');
                    var lonCategorias = categorias.length;
                    for (var c = 0; c < lonCategorias; c++) {
                        hijoCategoria = categorias[c];
                        arrCategorias.push($(hijoCategoria).html());
                    }
                    if (arrayIDdetalle != undefined) {
                        aarrId = [];
                        idDet = $('tr[id-renglon="' + j + '"]').find('td.categorias div.pn');
                        var lonDEt = idDet.length;
                        for (var cid = 0; cid < lonCategorias; cid++) {
                            hijoidDet = idDet[cid];
                            aarrId.push($(hijoidDet).attr('id-detalle'));
                        }
                    }
                    arrpersonalNecesario = [];
                    personalNecesario = $('tr[id-renglon="' + j + '"]').find('td.PersonalNecesario div.pn');
                    var lonpersonalNecesario = personalNecesario.length;
                    for (var c = 0; c < lonpersonalNecesario; c++) {
                        hijopersonalNecesario = personalNecesario[c];
                        arrpersonalNecesario.push($(hijopersonalNecesario).html());
                    }
                    arrpersonalExistente = [];
                    personalExistente = $('tr[id-renglon="' + j + '"]').find('td.PersonalExistente div.pn');
                    var lonpersonalExistente = personalExistente.length;
                    for (var c = 0; c < lonpersonalExistente; c++) {
                        hijpersonalExistente = personalExistente[c];
                        arrpersonalExistente.push($(hijpersonalExistente).html());
                    }
                    arrpersonalFaltante = [];
                    personalFaltante = $('tr[id-renglon="' + j + '"]').find('td.PersonalFaltante div.pn');
                    var lonpersonalFaltante = personalFaltante.length;
                    for (var c = 0; c < lonpersonalFaltante; c++) {
                        hijopersonalFaltante = personalFaltante[c];
                        arrpersonalFaltante.push($(hijopersonalFaltante).html());
                    }
                    var objcategoria;
                    var objpersonalnecesario;
                    var objpersonalfaltante;
                    var objpersonalexistente;
                    var obidDet;
                    var longrrecorrido = arrCategorias.length;
                    for (var i = 0; i < longrrecorrido; i++) {
                        arrCategorias.forEach(function (valor, indice, array) {
                            for (var i = 0; i == indice; i++) {
                                objcategoria = valor;
                                arrCategorias.splice(indice, 1);
                            }
                        });
                        arrpersonalNecesario.forEach(function (valor, indice, array) {
                            for (var i = 0; i == indice; i++) {
                                objpersonalnecesario = valor;
                                arrpersonalNecesario.splice(indice, 1);
                            }
                        });
                        arrpersonalFaltante.forEach(function (valor, indice, array) {
                            for (var i = 0; i == indice; i++) {
                                objpersonalfaltante = valor;
                                arrpersonalFaltante.splice(indice, 1);
                            }
                        });
                        arrpersonalExistente.forEach(function (valor, indice, array) {
                            for (var i = 0; i == indice; i++) {
                                objpersonalexistente = valor;
                                arrpersonalExistente.splice(indice, 1);
                            }
                        });
                        if (arrayIDdetalle != undefined) {
                            aarrId.forEach(function (valor, indice, array) {
                                for (var i = 0; i == indice; i++) {
                                    obidDet = valor;
                                    aarrId.splice(indice, 1);
                                }
                            });
                        }
                        if (i == 0) {
                            lugaresPlantilla = $('tr[id-renglon="' + j + '"]').find('td.lugplantilla').text().trim();
                            aditiva = $('tr[id-renglon="' + j + '"]').find("td.aditiva input").val();
                            deductiva = $('tr[id-renglon="' + j + '"]').find("td.deductiva input").val();
                            numPersTotal = $('tr[id-renglon="' + j + '"]').find('td.personalcC input').val();
                            if (numPersTotal == undefined) {
                                numPersTotal = $('tr[id-renglon="' + j + '"]').find('td.personalcC').text()
                            }
                            puesto = $('tr[id-renglon="' + j + '"]').find('td option:selected').text();
                            justificacion = $('tr[id-renglon="' + j + '"]').find('td.justificacion textarea').val();
                        }
                        iDFormatoAditiva > 0 ? idlocal = iDFormatoAditiva : idlocal = 0;
                        obidDet > 0 ? idtmpDet = obidDet : idtmpDet = 0;
                        objAditivaDeductiva = {
                            id: idtmpDet,
                            id_AditivaDeductiva: idlocal,
                            puesto: puesto,
                            categoria: objcategoria,
                            personalNecesario: objpersonalnecesario,
                            personalFaltante: objpersonalfaltante,
                            personalExistente: objpersonalexistente,
                            lugaresPlantilla: lugaresPlantilla,
                            aditiva: aditiva,
                            deductiva: deductiva,
                            numPersTotal: numPersTotal,
                            justificacion: justificacion,
                            nuevo: false,
                            idPuesto: $('tr[id-renglon="' + j + '"]').find('td option:selected').val(),

                        };
                        arrayEnvioObj.push(objAditivaDeductiva);
                    }
                } else if (tipoPuesto == "NewPuesto") {
                    iDFormatoAditiva > 0 ? idlocal = iDFormatoAditiva : idlocal = 0;
                    var iddet = $(renglon).find("td.categorias div").attr('id-detalle');
                    iddet > 0 ? id = iddet : id = 0;
                    objAditivaDeductiva = {
                        id: id,
                        id_AditivaDeductiva: idlocal,
                        puesto: $('tr[id-renglon="' + j + '"]').find('td option:selected').text(),
                        categoria: "N/A",
                        personalNecesario: 0,
                        personalFaltante: 0,
                        personalExistente: 0,
                        lugaresPlantilla: 0,
                        aditiva: $(renglon).find("td.aditiva input").val(),
                        deductiva: 0,
                        numPersTotal: $(renglon).find("td.personalcCPuesto input").val(),
                        justificacion: $(renglon).find('td.justificacionPuesto textarea').val(),
                        nuevo: true,
                        idPuesto: $('tr[id-renglon="' + j + '"]').find('td option:selected').val(),
                    };
                    arrayEnvioObj.push(objAditivaDeductiva);
                }
            }
            SolicitarAprobacion(arrayEnvioObj);
        }
        var banderavalAddv = false;
        function SolicitarAprobacion(arrayEnvioObj) {
            validacionGralEnvio();
            if (banderavalAddv == false) {
                if (iDFormatoAditiva > 0) {
                    idlocal = iDFormatoAditiva;
                } else {
                    idlocal = 0;
                }
                btnSolicitarAp.prop("disabled", true);
                objtblRhAdi = {
                    cCid: $("#cboCC option:selected").val(),
                    fecha_Alta: fechaIni.val(),
                    cC: $("#cboCC option:selected").text(),
                    id: idlocal,
                    condicionInicial: $("#txtCondicionInicial").val(),
                    condicionActual: $("#txtCondicionActual").val(),
                    soporte: $("#txtOtraJustificacion").val()
                };
                revisarAmbos();
                revisarExisteAutorizadores();
                $.blockUI({ message: mensajes.Enviando });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/Administrativo/AditivaPersonal/getEnvioDetalle',
                    data: { arrayEnvioObj: arrayEnvioObj, lstAutorizacion: argAutorizaciones, objAditivaDeduvtiva: objtblRhAdi, arraylstElimina: arraylstElimina },
                    success: function (response) {
                        arraylstElimina = [];
                        var idFormatoAditivaDeductiva = response.idFormatoAditiva;
                        var usuarioEnvia = response.usuarioEnvia;
                        var idReporte = "62";
                        var folio = response.objFormatoAditivaDTO.objAditivaDeductiva.folio;
                        var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&fId=" + idFormatoAditivaDeductiva + "&inMemory=1";
                        ireport.attr("src", path);
                        var tipo = objAditivaDeductiva.id == 0 ? "nuevo" : "cambio";
                        var file = document.getElementById("fuEvidencia").files[0];
                        if (file != undefined) {
                            subirEvidencia(idFormatoAditivaDeductiva, file)
                        }
                        document.getElementById('report').onload = function () {
                            $.ajax({
                                datatype: "json",
                                type: "POST",
                                //url: '/Administrativo/AditivaPersonal/enviarCorreos',
                                //data: { usuariorecibe: usuarioEnvia, formatoID: idFormatoAditivaDeductiva, tipo: tipo },
                                url: '/Administrativo/AditivaPersonal/EnviarCorreoFormato',
                                data: { plantillaID: idFormatoAditivaDeductiva, autorizacion: 0, estatus: 1 },
                                success: function (response) {
                                    $.unblockUI();

                                    ConfirmacionGeneralFC("Confirmación", "Se a guardado el folio " + folio, "bg-green");
                                    setTimeout(function () { location.reload(); }, 1500);

                                },
                                error: function () {
                                    $.unblockUI();
                                }
                            });
                        };
                    },
                    error: function () {
                        btnSolicitarAp.prop("disabled", false);
                        $.unblockUI();
                    }
                });
            }
            banderavalAddv = false;
        }
        function subirEvidencia(solicitudID, file) {
            var formData = new FormData();

            formData.append("fuEvidencia", file);
            formData.append("solicitudID", solicitudID);

            $.ajax({
                type: "POST",
                url: "/Administrativo/AditivaPersonal/SubirEvidenciaSolicitud",
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    fuEvidencia.val("");
                },
                error: function (error) {
                }
            });
        }
        function validacionGralEnvio() {
            var longitud = $("#tablaPadre >tbody >tr").length
            if (longitud > 0) {
                for (var i = 0; i < longitud && banderavalAddv == false; i++) {
                    renglon = $("#tablaPadre >tbody >tr")[i];
                    var tipoPuesto = $(renglon).find('td').attr('id');
                    if (tipoPuesto == "Puesto") {
                        var txtcbopuesto = $(renglon).find('td option:selected').text();
                        if (txtcbopuesto == "--Seleccionar--") {
                            banderavalAddv = true;
                            AlertaGeneral("Alerta", "Falta Indicar Puesto, un valor es del tipo --Seleccionar--");
                        }
                        var aditiva = $(renglon).find("td.aditiva input").val();
                        var deductiva = $(renglon).find("td.deductiva input").val();
                        //if (aditiva == 0 && deductiva == 0) { 7/2/18/ 12:14pm
                        //    banderavalAddv = true;
                        //    AlertaGeneral("Alerta", "Falta Indicar valor para Aditiva-Deductiva de Personal");
                        //}
                    }
                    else if (tipoPuesto == "NewPuesto") {
                        var puestoNew = $(renglon).find('td#NewPuesto input').val();
                        var aditivapuestoNew = $(renglon).find("td.aditiva input").val();
                        if ("" == puestoNew) {
                            AlertaGeneral('Alerta', 'Favor de indicar el nuevo puesto');
                            banderavalAddv = true;
                        }// else if (aditivapuestoNew == 0) {
                        //    AlertaGeneral('Alerta', 'Favor de indicar la aditiva del nuevo puesto');
                        //    banderavalAddv = true;
                        //}
                    }
                    if ("--Seleccionar--" == $("#cboPuesto option:selected").text()) {
                        AlertaGeneral("Alerta", "Debe Seleccionar Un puesto")
                        banderavalAddv = true;
                    }
                    else if (selAutSolicita.val() === "" || selAutVoBo.val() === "") {
                        AlertaGeneral("Alerta", "Debe seleccionar un Solicitante y VoBo para poder guardar")
                        banderavalAddv = true;
                    } else if (flagglobalpuesto == true) {
                        AlertaGeneral("Alerta", "No se permiten Nombre de puestos Repetidos")
                        banderavalAddv = true;
                    }
                }
            } else {
                AlertaGeneral("Alerta", "Debe tener informacion en la plantilla")
                banderavalAddv = true;
            }
        }
        function revisarExisteAutorizadores() {
            if (selAutoriza1.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Gerente/SubDirector/Director de Área 1"; },
                    true);
            }
            if (selAutoriza12.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Gerente/SubDirector/Director de Área 2"; },
                    true);
            }
            if (selAutoriza2.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Director de Línea de Negocios 1"; },
                    true);
            }
            if (selAutoriza22.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Director de Línea de Negocios 2"; },
                    true);
            }
            if (selAutoriza3.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Alta Dirección 1"; },
                    true);
            }
            if (selAutoriza32.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Alta Dirección 2"; },
                    true);
            }
            if (selAutVoBo.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Capital Humano"; },
                    true);
            }
        }
        function revisarAmbos() {
            for (var i = 0; i < argAutorizaciones.length; i++) {
                if (argAutorizaciones[i].Orden == 2 || argAutorizaciones[i].Orden == 3) {
                    if (!btnAutorizaCck1.is(':checked')) {
                        argAutorizaciones[i].Orden = 2
                        argAutorizaciones[i].tipoAutoriza = true;
                    }
                }
                if (argAutorizaciones[i].Orden == 4 || argAutorizaciones[i].Orden == 5) {
                    if (!btnAutorizaCck2.is(':checked')) {
                        argAutorizaciones[i].Orden = 4
                        argAutorizaciones[i].tipoAutoriza = true;
                    }
                }
                if (argAutorizaciones[i].Orden == 7 || argAutorizaciones[i].Orden == 8) {
                    if (!btnAutorizaCck3.is(':checked')) {
                        argAutorizaciones[i].Orden = 7
                        argAutorizaciones[i].tipoAutoriza = true;
                    }
                }
            }
        }
        function setAutoComplete() {
            selAutSolicita.getAutocomplete(funAutSolicita, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutVoBo.getAutocomplete(funAutVoBo, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza1.getAutocomplete(funAutoriza1, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza12.getAutocomplete(funAutoriza12, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza2.getAutocomplete(funAutoriza2, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza22.getAutocomplete(funAutoriza22, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza3.getAutocomplete(funAutoriza3, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza32.getAutocomplete(funAutoriza32, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
        }
        function ConfirmacionGeneralFC(titulo, mensaje, color) {
            if (!$("#dialogalertaGeneral").is(':visible')) {
                var html = '<div id="dialogalertaGeneral" class="modal fade" role="dialog">' +
                    '<div class="modal-dialog">' +
                    '<div class="modal-content">' +
                    '<div class="modal-header text-center modal-lg">' +
                    '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
                    '&times;</button>' +
                    '<h4  class="modal-title">' + titulo + '</h4>' +
                    '</div>' +
                    '<div class="modal-body">' +
                    '<div class="container">' +
                    '<div class="row">' +
                    '<div class="col-lg-12">' +
                    '<h3> <span class="glyphicon glyphicon-ok-circle ' + color + '" aria-hidden="true" style="font-size:40px;"></span> <label style="position: fixed;">' + mensaje + '</label></h3>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div class="modal-footer">' +
                    '<a id="btndialogalertaGeneral" href="/Administrativo/AditivaPersonal/index" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
                    '</div>' +
                    '</div>' +
                    '</div></div>';

                var _this = $(html);
                _this.modal("show");
            }
        }
        function selectAprobaciones(id) {
            argAutorizaciones = [];
            selAutSolicita.val("");
            selAutVoBo.val("");
            selAutoriza1.val("");
            selAutoriza12.val("");
            selAutoriza2.val("");
            selAutoriza22.val("");
            selAutoriza3.val("");
            selAutoriza32.val("");
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '/Administrativo/AditivaPersonal/GetAutorizacion',
                data: { id: id },
                async: false,
                success: function (response) {

                    var i = 0;
                    for (i; i <= response.items.length - 1; i++) {

                        switch (response.items[i].responsable) {
                            case 'Solicitante':
                                selAutSolicita.val(response.items[i].nombre_Aprobador);
                                objAutorizacion = {
                                    id: response.items[i].id,
                                    Clave_Aprobador: response.items[i].clave_Aprobador,
                                    Nombre_Aprobador: response.items[i].nombre_Aprobador,
                                    PuestoAprobador: response.items[i].puestoAprobador,
                                    Responsable: response.items[i].responsable,
                                    Autorizando: response.items[i].autorizando,
                                    Orden: response.items[i].orden

                                }
                                argAutorizaciones.push(objAutorizacion);
                                break;

                            case 'VoBo':
                                selAutVoBo.val(response.items[i].nombre_Aprobador);
                                objAutorizacion = {
                                    id: response.items[i].id,
                                    Clave_Aprobador: response.items[i].clave_Aprobador,
                                    Nombre_Aprobador: response.items[i].nombre_Aprobador,
                                    PuestoAprobador: response.items[i].puestoAprobador,
                                    Responsable: response.items[i].responsable,
                                    Autorizando: response.items[i].autorizando,
                                    Orden: response.items[i].orden
                                }
                                argAutorizaciones.push(objAutorizacion);
                                break;
                            case 'Autorización 1':
                                if (selAutoriza1.val().length == 0) {
                                    selAutoriza1.val(response.items[i].nombre_Aprobador);
                                }
                                else { selAutoriza12.val(response.items[i].nombre_Aprobador); }
                                objAutorizacion = {
                                    id: response.items[i].id,
                                    Clave_Aprobador: response.items[i].clave_Aprobador,
                                    Nombre_Aprobador: response.items[i].nombre_Aprobador,
                                    PuestoAprobador: response.items[i].puestoAprobador,
                                    Responsable: response.items[i].responsable,
                                    Autorizando: response.items[i].autorizando,
                                    Orden: response.items[i].orden
                                }
                                argAutorizaciones.push(objAutorizacion);
                                break;
                            case 'Autorización 2':
                                if (selAutoriza2.val().length == 0) {
                                    selAutoriza2.val(response.items[i].nombre_Aprobador);
                                }
                                else { selAutoriza22.val(response.items[i].nombre_Aprobador); }

                                objAutorizacion = {
                                    id: response.items[i].id,
                                    Clave_Aprobador: response.items[i].clave_Aprobador,
                                    Nombre_Aprobador: response.items[i].nombre_Aprobador,
                                    PuestoAprobador: response.items[i].puestoAprobador,
                                    Responsable: response.items[i].responsable,
                                    Autorizando: response.items[i].autorizando,
                                    Orden: response.items[i].orden
                                }
                                argAutorizaciones.push(objAutorizacion);
                                break;
                            case 'Autorización 3':
                                if (selAutoriza3.val().length == 0) {
                                    selAutoriza3.val(response.items[i].nombre_Aprobador);
                                }
                                else { selAutoriza32.val(response.items[i].nombre_Aprobador); }
                                objAutorizacion = {
                                    id: response.items[i].id,
                                    Clave_Aprobador: response.items[i].clave_Aprobador,
                                    Nombre_Aprobador: response.items[i].nombre_Aprobador,
                                    PuestoAprobador: response.items[i].puestoAprobador,
                                    Responsable: response.items[i].responsable,
                                    Autorizando: response.items[i].autorizando,
                                    Orden: response.items[i].orden
                                }
                                argAutorizaciones.push(objAutorizacion);
                                break;

                        }
                    }
                },
                error: function (response) {
                }
            });
        }
        function funAutSolicita(event, ui) {
            selAutSolicita.text(ui.item.value);
            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.Responsable != "Solicitante"; },
                true);
            if (a.length > 0) {
                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].Responsable == "Solicitante") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Responsable del Centro de Costos",
                    Responsable: "Solicitante",
                    Autorizando: true,
                    Orden: 1,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }
        }
        function funAutVoBo(event, ui) {
            selAutVoBo.text(ui.item.value);
            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.Responsable != "VoBo"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].Responsable == "VoBo") {
                        if (ui.item.id == 1041) argAutorizaciones[i].Orden = 9;
                        else argAutorizaciones[i].Orden = 2;
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Capital Humano",
                    Responsable: "VoBo",
                    Orden: 2,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }

        }
        function funAutoriza1(event, ui) {
            selAutoriza1.text(ui.item.value);
            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Gerente/SubDirector/Director de Área 1"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Gerente/SubDirector/Director de Área 1") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Gerente/SubDirector/Director de Área 1",
                    Responsable: "Autorización 1",
                    Orden: 3,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }

        }
        function funAutoriza12(event, ui) {
            selAutoriza12.text(ui.item.value);

            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Gerente/SubDirector/Director de Área 2"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Gerente/SubDirector/Director de Área 2") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Gerente/SubDirector/Director de Área 2",
                    Responsable: "Autorización 1",
                    Orden: 4,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }
        }
        function funAutoriza2(event, ui) {
            selAutoriza2.text(ui.item.value);
            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Director de Línea de Negocios 1"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Director de Línea de Negocios 1") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Director de Línea de Negocios 1",
                    Responsable: "Autorización 2",
                    Orden: 5,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }
        }
        function funAutoriza22(event, ui) {
            selAutoriza22.text(ui.item.value);
            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Director de Línea de Negocios 2"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Director de Línea de Negocios 2") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Director de Línea de Negocios 2",
                    Responsable: "Autorización 2",
                    Orden: 6,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }
        }
        function funAutoriza3(event, ui) {
            selAutoriza3.text(ui.item.value);
            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Alta Dirección 1"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Alta Dirección 1") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Alta Dirección 1",
                    Responsable: "Autorización 3",
                    Orden: 7,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }
        }
        function funAutoriza32(event, ui) {
            selAutoriza32.text(ui.item.value);
            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Alta Dirección 2"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Alta Dirección 2") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Alta Dirección 2",
                    Responsable: "Autorización 3",
                    Orden: 8,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }
        }
        function ConfirmacionModal(titulo, mensaje) {
            if (!$("#modalConfirmacionModal").is(':visible')) {
                var html = '<div id="modalConfirmacionModal" class="modal fade" role="dialog" data-backdrop="static">' +
                    '<div class="modal-dialog modal-dialog-fix modal-md" >' +
                    '<div class="modal-content">' +
                    '<div class="modal-header text-center modal-bg">' +
                    '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
                    '&times;</button>' +
                    '<h4  class="modal-title">' + titulo + '</h4>' +
                    '</div>' +
                    '<div class="modal-body ajustar-texto">' +
                    '<h5 id="pMessage">' +
                    '</h5>' +
                    '<div class="row">' +
                    '<div id="icon" class="col-md-2">' +
                    '<span class="glyphicon glyphicon-warning-sign alert-warning-span" style="font-size:40px;" aria-hidden="true"></span>' +
                    '</div>' +
                    '<div class="col-md-10">' +
                    '<h3>  ' + mensaje + '</h3>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div class="modal-footer">' +
                    '<a data-dismiss="modal" id="btnModalAceptarAditiva" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
                    '<a data-dismiss="modal" id="btnCancelarAditiva" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-remove"></span> Cancelar</a>' +
                    '</div>' +
                    '</div>' +
                    '</div></div>';

                var _this = $(html);
                _this.modal("show");
            }
        }
        function DetalleModal(titulo, mensaje) {
            if (!$("#modalEliminacion").is(':visible')) {
                var html = '<div id="modalEliminar" class="modal fade" role="dialog" data-backdrop="static">' +
                    '<div class="modal-dialog modal-dialog-fix modal-md" >' +
                    '<div class="modal-content">' +
                    '<div class="modal-header text-center modal-bg">' +
                    '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
                    '&times;</button>' +
                    '<h4  class="modal-title">' + titulo + '</h4>' +
                    '</div>' +
                    '<div class="modal-body ajustar-texto">' +
                    '<h5 id="pMessage">' +
                    '</h5>' +
                    '<div class="row">' +
                    '<div id="icon" class="col-md-2">' +
                    '<span class="glyphicon glyphicon-warning-sign alert-warning-span" style="font-size:40px;" aria-hidden="true"></span>' +
                    '</div>' +
                    '<div class="col-md-10">' +
                    '<h3>  ' + mensaje + '</h3>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div class="modal-footer">' +
                    '<a data-dismiss="modal" id="btnModalAceptarDetModal" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
                    '<a data-dismiss="modal" id="btnCancelarDetModal" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-remove"></span> Cancelar</a>' +
                    '</div>' +
                    '</div>' +
                    '</div></div>';

                var _this = $(html);
                _this.modal("show");
            }
            //    $("#dialogalertaGeneral").trigger("click");

        }
        function EliminadoModal(titulo, mensaje) {
            if (!$("#modalEliminacion").is(':visible')) {
                var html = '<div id="modalEliminar" class="modal fade" role="dialog" data-backdrop="static">' +
                    '<div class="modal-dialog modal-dialog-fix modal-md" >' +
                    '<div class="modal-content">' +
                    '<div class="modal-header text-center modal-bg">' +
                    '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
                    '&times;</button>' +
                    '<h4  class="modal-title">' + titulo + '</h4>' +
                    '</div>' +
                    '<div class="modal-body ajustar-texto">' +
                    '<h5 id="pMessage">' +
                    '</h5>' +
                    '<div class="row">' +
                    '<div id="icon" class="col-md-2">' +
                    '<span class="glyphicon glyphicon-warning-sign alert-warning-span" style="font-size:40px;" aria-hidden="true"></span>' +
                    '</div>' +
                    '<div class="col-md-10">' +
                    '<h3>  ' + mensaje + '</h3>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div class="modal-footer">' +
                    '<a data-dismiss="modal" id="btnModalAceptarEliminar" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
                    '<a data-dismiss="modal" id="btnCancelarEliminar" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-remove"></span> Cancelar</a>' +
                    '</div>' +
                    '</div>' +
                    '</div></div>';

                var _this = $(html);
                _this.modal("show");
            }
            //    $("#dialogalertaGeneral").trigger("click");

        }

        //#region GESTIÓN DE SOLICITUDES
        // const getUrlParams = function (url) {
        //     let params = {};
        //     let parser = document.createElement('a');
        //     parser.href = url;
        //     let query = parser.search.substring(1);
        //     let vars = query.split('&');
        //     for (let i = 0; i < vars.length; i++) {
        //         let pair = vars[i].split('=');
        //         params[pair[0]] = decodeURIComponent(pair[1]);
        //     }
        //     return params;
        // };
        //#endregion

        init();
    }
    $(document).ready(function () {
        recursoshumanos.RecursosHumanos = new AltasAdtvs();
    });
});


