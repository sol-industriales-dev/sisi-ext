(function () {

    $.namespace('Administrativo.Contabilidad.Prenomina');

    Prenomina = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Autorizacion de Solicitudes Reemplazo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        const comboNomina = $("#comboNomina");
        const comboCC = $("#comboCC");
        const botonGuardar = $("#botonGuardar");
        const botonBuscar = $("#botonBuscar");
        const botonReporte = $("#botonReporte");
        const botonValidar = $("#botonValidar");
        const botonDesValidar = $("#botonDesValidar");
        const inputNumEmpleados = $("#inputNumEmpleados");

        const tablaPrenomina = $("#tablaPrenomina");
        let dtTablaPrenomina;

        const tablaAutorizantes = $("#tablaAutorizantes");
        let dtTablaAutorizantes;

        const modalAgregarAutorizante = $("#modalAgregarAutorizante");
        const comboAutorizante = $("#comboAutorizante");
        const inputPuestoAutorizante = $("#inputPuestoAutorizante");
        const botonGuardarAutorizante = $("#botonGuardarAutorizante");

        const botonSolicitudCheque = $('#botonSolicitudCheque');
        const modalSolicitudCheque = $('#modalSolicitudCheque');
        const selectBanco = $('#selectBanco');
        const botonGenerarSolicitudCheque = $('#botonGenerarSolicitudCheque');
        const botonGenerarCedulaCostos = $('#botonGenerarCedulaCostos');
        const botonGenerarPolizaOCSI = $('#botonGenerarPolizaOCSI');
        const botonCorreoDespacho = $('#botonCorreoDespacho');
        const botonCorreoSolicitud = $('#botonCorreoSolicitud');
        const report = $("#report");

        const GetCbotPeriodoNomina = originURL('/Administrativo/Nomina/GetCbotPeriodoNomina');
        const FillCboCC = originURL('/Administrativo/Nomina/GetCCsIncidencias');
        const FillCboBancos = originURL('/Administrativo/Nomina/FillCboBancos');
        const FillCboEmpresas = originURL('/Administrativo/Nomina/FillCboEmpresasNomina');
        const GetUsuariosAutorizantes = originURL('/Administrativo/Nomina/GetUsuariosAutorizantes');
        //const GuardarPrenomina = originURL('/Administrativo/Nomina/GuardarPrenomina');

        var startDate, endDate;
        //var fechaActual = "";
        //const txtFecha = $('#txtFecha');
        const cboTipoNomina = $('#cboTipoNomina');

        let tipoPeriodo = -1;
        let periodo = -1;
        let anio = -1;
        let cc = '-1'
        let validada = false;
        let empresa = 1;
        let tipoNomina = 1;

        function init() {
            GetEmpresaActual();
            AgregarListener();

            LlenarCombos();
            botonSolicitudCheque.click(() => { modalSolicitudCheque.modal('show'); });
            botonGenerarSolicitudCheque.click(imprimirSolicitudCheque);
            botonGenerarCedulaCostos.click(imprimirCedulaCostos);
            botonGenerarPolizaOCSI.click(imprimirPolizaOCSI);
        }

        function AgregarListener() {
            //#region SE OBTIENE LA FECHA ACTUAL
            //let today = new Date();
            //let MM = today.getMonth() + 1;
            //let dd = today.getDate();
            //let yyyy = today.getFullYear();
            //dd = dd >= 1 && dd <= 9 ? `0${dd}` : dd;
            //MM = MM >= 1 && MM <= 9 ? `0${MM}` : MM;
            //fechaActual = `${dd}/${MM}/${yyyy}`;
            //#endregion

            //cboTipoNomina.select2();

            //txtFecha.on("mouseover", function () {
            //    if (cboTipoNomina.val() <= 0) {
            //        Alert2Warning("Es necesario seleccionar el tipo de nómina.");
            //    } else {
            //        // $(".ui-datepicker-calendar tbody tr:eq(0) td:eq(0)").css("background-color", "#da6a1a");
            //        // $('#datePicker').fadeIn('fast');
            //        let mes = $(".ui-datepicker-month").text();
            //        let anio = $(".ui-datepicker-year").text();
            //    }
            //});

            //txtFecha.on("click", function () {
            //    let mes = $(".ui-datepicker-month").text();
            //    let anio = $(".ui-datepicker-year").text();
            //    fncGetEstatusPeriodo(cboTipoNomina.val(), anio, mes);
            //});

            //$(".ui-datepicker-next ui-corner-all").on("click", function () {
            //    let mes = $(".ui-datepicker-month").text();
            //    let anio = $(".ui-datepicker-year").text();
            //    console.log(`${mes} - ${anio}`);
            //});

            //txtFecha.datepicker({
            //    firstDay: 3,
            //    showOtherMonths: true,
            //    selectOtherMonths: true,
            //    onSelect: function (dateText, inst) {
            //        setSemanaSelecionada();
            //    },
            //    beforeShowDay: function (date) {
            //        var cssClass = '';
            //        if (date >= startDate && date <= endDate)
            //            cssClass = 'ui-datepicker-current-day';
            //        return [true, cssClass];
            //    },
            //    onChangeMonthYear: function (year, month, inst) {
            //        selectCurrentWeek();
            //    }
            //}).datepicker("setDate", fechaActual);

            botonBuscar.click(CargarPrenomina);
            botonGuardar.click(GuardarPrenomina);
            botonReporte.click(DescargarReporte);
            botonValidar.click(ConfirmarValidacion);
            botonDesValidar.click(ConfirmarEliminarValidacion);
            comboAutorizante.change(function (e) {
                inputPuestoAutorizante.val($("#comboAutorizante option:selected").attr("data-prefijo"));
            });
            botonGuardarAutorizante.click(GuardarAutorizante);
            comboNomina.change(function (e) {
                const dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
                //tipoPeriodo = dataPeriodo[2];
                tipoPeriodo = cboTipoNomina.val();
                periodo = comboNomina.val();
                anio = dataPeriodo[3];
                comboCC.fillComboGroupSelectable(FillCboCC, { periodo: periodo, tipoNomina: tipoPeriodo, anio: anio }, false, "--Seleccione--");
                selectBanco.fillCombo(FillCboBancos, { tipoNomina: tipoPeriodo, periodo: periodo }, false, "--Seleccione--");
                if (cboTipoNomina.val() == 10) botonGenerarPolizaOCSI.prop("disabled", true);
                else botonGenerarPolizaOCSI.prop("disabled", false);
            });
            comboCC.change(function (e) {
                $("#select2-comboCC-container").removeClass('validada');
                $("#select2-comboCC-container").removeClass('no-validada');
                $("#select2-comboCC-container").addClass($("#comboCC option:selected")[0].className);
                $(".select2-selection--single").removeClass('validada');
                $(".select2-selection--single").removeClass('no-validada');
                $(".select2-selection--single").addClass($("#comboCC option:selected")[0].className);
            });

            botonCorreoDespacho.on('click', function () {
                const nominaId = $(this).data().nominaId;

                swal({
                    title: 'Alerta!',
                    text: 'Se enviará un correo con los concentrados de prenóminas para el periodo seleccionado, además de las solicitudes de cheque correspondientes. ¿Desea continuar?',
                    icon: 'warning',
                    buttons: true,
                    dangerMode: true,
                    buttons: ['Cancelar', 'Enviar']
                })
                    .then((aceptar) => {
                        if (aceptar) {
                            solicitarCorreoDespacho();
                        }
                    });
            });
            botonCorreoSolicitud.on('click', function () {
                const nominaId = $(this).data().nominaId;

                swal({
                    title: 'Alerta!',
                    text: 'Se enviará un correo con los concentrados de solicitudes de cheque correspondientes. ¿Desea continuar?',
                    icon: 'warning',
                    buttons: true,
                    dangerMode: true,
                    buttons: ['Cancelar', 'Enviar']
                })
                    .then((aceptar) => {
                        if (aceptar) {
                            solicitarCorreoSolicitudes();
                        }
                    });
            });
            cboTipoNomina.change(function (e) {
                comboNomina.fillComboGroup(GetCbotPeriodoNomina, { tipoNomina: cboTipoNomina.val() }, false, undefined, function () {
                    //comboNomina.prop("selectedIndex", 1);
                    comboNomina.change();
                });
            });
        }

        let arrPeriodos = new Array();
        function fncGetEstatusPeriodo(tipo_nomina, anio) {
            if (cboTipoNomina.val() >= 0) {
                let obj = new Object();
                obj.tipo_nomina = tipo_nomina;
                obj.anio = anio;
                axios.post("GetEstatusPeriodo", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        let obj = new Object();
                        arrPeriodos = new Array();
                        response.data.lstPeriodos.forEach(element => {
                            obj = new Object();
                            obj.anio = element.anio;
                            obj.periodo = element.periodo;
                            obj.fecha_inicial = element.fecha_inicial;
                            obj.fecha_final = element.fecha_final;
                            arrPeriodos.push(obj);
                        });
                        console.log(arrPeriodos);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario seleccionar el tipo de nómina.");
            }
        }

        function Inicializadores() {
            InitTablaPrenomina();
            InitTablaAutorizantes();
            comboCC.select2({
                templateResult: function (data, container) {
                    if (data.element) {
                        $(container).addClass($(data.element).attr("class"));
                    }
                    return data.text;
                }
            });
            comboAutorizante.select2();
        }

        function LlenarCombos() {
            comboNomina.fillComboGroup(GetCbotPeriodoNomina, { tipoNomina: cboTipoNomina.val() }, false, undefined, function () {
                //comboNomina.prop("selectedIndex", 1);
                comboNomina.change();
            });
            comboAutorizante.fillCombo(GetUsuariosAutorizantes);
        }

        function InitTablaPrenomina() {
            dtTablaPrenomina = tablaPrenomina.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                ordering: false,
                fixedHeader: true,
                drawCallback: function (settings) {
                    let tabIndex = 1;
                    tablaPrenomina.find('input.captura').blur(function (e) {
                        var fila = ($(this).closest("tr")[0].rowIndex) - 1;
                        var columna = $(this).closest("td")[0].cellIndex;

                        if (tipoNomina == 10) {
                            switch (columna) {
                                case 5: dtTablaPrenomina.row(fila).data().dias = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                case 8: dtTablaPrenomina.row(fila).data().diaFestivo = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                case 10: dtTablaPrenomina.row(fila).data().pensionAlimenticia = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            }
                        }
                        else {

                            if (empresa == 6) {
                                //#region PERU
                                switch (columna) {
                                    case 3:
                                        dtTablaPrenomina.row(fila).data().dias = Number($(this).val().replace(/[^0-9.-]+/g, ""));
                                        //dtTablaPrenomina.row(fila).data().fondoAhorroNomina = ((dtTablaPrenomina.row(fila).data().nominaBase / (tipoPeriodo == "1" ? 7 : 15)) * Number($(this).val().replace(/[^0-9.-]+/g, ""))) * 0.025
                                        //dtTablaPrenomina.row(fila).data().fondoAhorroNomina = ((dtTablaPrenomina.row(fila).data().complementoNomina / (tipoPeriodo == "1" ? 7 : 15)) * Number($(this).val().replace(/[^0-9.-]+/g, ""))) * 0.025
                                        break;

                                    case 4: dtTablaPrenomina.row(fila).data().onp = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 5: dtTablaPrenomina.row(fila).data().afp = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 6: dtTablaPrenomina.row(fila).data().afpSeguros = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 7: dtTablaPrenomina.row(fila).data().afpComision = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 8: dtTablaPrenomina.row(fila).data().sta = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 9: dtTablaPrenomina.row(fila).data().esSalud = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;

                                    case 10: dtTablaPrenomina.row(fila).data().bonoProduccion = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 11: dtTablaPrenomina.row(fila).data().otros = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 12: dtTablaPrenomina.row(fila).data().primaVacacional = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;

                                    case 13: dtTablaPrenomina.row(fila).data().hrExtra = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 14: dtTablaPrenomina.row(fila).data().diaHrExtra = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 15: dtTablaPrenomina.row(fila).data().asigFamiliar = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 16: dtTablaPrenomina.row(fila).data().descuento = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 17: dtTablaPrenomina.row(fila).data().adelantoQuincena = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                }
                                //#region PERU
                            }
                            else if (empresa == 3) {
                                switch (columna) {
                                    //#region COLOMBIA
                                    case 3: dtTablaPrenomina.row(fila).data().dias = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;

                                    case 4: dtTablaPrenomina.row(fila).data().diasVacaciones = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 5: dtTablaPrenomina.row(fila).data().primaVacacional = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 6: dtTablaPrenomina.row(fila).data().diasIncapacidad = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 7: dtTablaPrenomina.row(fila).data().importeIncapacidad = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 8: dtTablaPrenomina.row(fila).data().licenciaLuto = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 9: dtTablaPrenomina.row(fila).data().prestamo = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 10: dtTablaPrenomina.row(fila).data().pensionAlimenticia = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 11: dtTablaPrenomina.row(fila).data().onp = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 12: dtTablaPrenomina.row(fila).data().bonoRecreacion = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;

                                    case 13: dtTablaPrenomina.row(fila).data().esSalud = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 14: dtTablaPrenomina.row(fila).data().otros = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 15: dtTablaPrenomina.row(fila).data().transporte = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;

                                    case 16: dtTablaPrenomina.row(fila).data().fsp = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 17: dtTablaPrenomina.row(fila).data().afp = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 18: dtTablaPrenomina.row(fila).data().retencion = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 19: dtTablaPrenomina.row(fila).data().descuento = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 20: dtTablaPrenomina.row(fila).data().hrExtra = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 21: dtTablaPrenomina.row(fila).data().diaHrExtra = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;

                                    case 22: dtTablaPrenomina.row(fila).data().cesantia = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 23: dtTablaPrenomina.row(fila).data().interesCesantia = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 24: dtTablaPrenomina.row(fila).data().retroactivo = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 25: dtTablaPrenomina.row(fila).data().prima = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 26: dtTablaPrenomina.row(fila).data().hrExtraDiurnasDominicales = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 27: dtTablaPrenomina.row(fila).data().hrNocturnas = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    //#endregion
                                }
                            }
                            else {
                                switch (columna) {
                                    case 4:
                                        dtTablaPrenomina.row(fila).data().dias = Number($(this).val().replace(/[^0-9.-]+/g, ""));
                                        //dtTablaPrenomina.row(fila).data().fondoAhorroNomina = ((dtTablaPrenomina.row(fila).data().nominaBase / (tipoPeriodo == "1" ? 7 : 15)) * Number($(this).val().replace(/[^0-9.-]+/g, ""))) * 0.025
                                        //dtTablaPrenomina.row(fila).data().fondoAhorroNomina = ((dtTablaPrenomina.row(fila).data().complementoNomina / (tipoPeriodo == "1" ? 7 : 15)) * Number($(this).val().replace(/[^0-9.-]+/g, ""))) * 0.025
                                        break;
                                    case 5:
                                        dtTablaPrenomina.row(fila).data().diasVacaciones = Number($(this).val().replace(/[^0-9.-]+/g, ""));
                                        //dtTablaPrenomina.row(fila).data().fondoAhorroNomina = ((dtTablaPrenomina.row(fila).data().nominaBase / (tipoPeriodo == "1" ? 7 : 15)) * Number($(this).val().replace(/[^0-9.-]+/g, ""))) * 0.025
                                        //dtTablaPrenomina.row(fila).data().fondoAhorroNomina = ((dtTablaPrenomina.row(fila).data().complementoNomina / (tipoPeriodo == "1" ? 7 : 15)) * Number($(this).val().replace(/[^0-9.-]+/g, ""))) * 0.025
                                        break;
                                    case 6: dtTablaPrenomina.row(fila).data().descuento = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 7: dtTablaPrenomina.row(fila).data().apoyoColectivo = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 8: dtTablaPrenomina.row(fila).data().prestamo = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 9: dtTablaPrenomina.row(fila).data().axa = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    //case 10: dtTablaPrenomina.row(fila).data().descuentoFamsa = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 10: dtTablaPrenomina.row(fila).data().pensionAlimenticia = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 11: dtTablaPrenomina.row(fila).data().fonacot = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 12: dtTablaPrenomina.row(fila).data().infonavit = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 13: dtTablaPrenomina.row(fila).data().fondoAhorroNomina = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;

                                    case 14: dtTablaPrenomina.row(fila).data().fondoAhorroComplemento = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 15: dtTablaPrenomina.row(fila).data().bonoProduccion = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 16: dtTablaPrenomina.row(fila).data().otros = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 17: dtTablaPrenomina.row(fila).data().primaVacacional = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;

                                    case 18: dtTablaPrenomina.row(fila).data().hrExtra = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 19: dtTablaPrenomina.row(fila).data().diaHrExtra = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                    case 20: dtTablaPrenomina.row(fila).data().diaFestivo = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                                }
                            }
                        }
                    });
                    tablaPrenomina.find('input.captura').focus(function (e) {
                        $(this).select();
                    });
                    tablaPrenomina.find('input.moneda').blur(function (e) {
                        if ($(this).val() == '') $(this).val("0.00");
                        else $(this).val(parseFloat($(this).val()).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                        if ($(this).val() != "0.00") $(this).css("background-color", "#B4C6E7");
                        else $(this).css("background-color", "-internal-light-dark(rgb(255, 255, 255), rgb(59, 59, 59));");
                    });
                    tablaPrenomina.find('input.moneda').focus(function (e) {
                        $(this).val(Number($(this).val().replace(/[^0-9.-]+/g, "")));
                    });

                    if (tipoNomina == 10) {
                        tablaPrenomina.find('input.dias').each(function () { $(this).attr("tabindex", tabIndex++); });
                        tablaPrenomina.find('input.diaFestivo').each(function () { $(this).attr("tabindex", tabIndex++); });
                        tablaPrenomina.find('input.pensionAlimenticia').each(function () { $(this).attr("tabindex", tabIndex++); });
                    }
                    else {
                        if (empresa == 6) {
                            //#region PERU
                            tablaPrenomina.find('input.dias').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.onp').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.afp').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.afpSeguros').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.afpComision').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.sta').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.esSalud').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.bonoProduccion').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.otros').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.primaVacacional').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.hrExtra').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.diaHrExtra').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.asigFamiliar').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.descuento').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.adelantoQuincena').each(function () { $(this).attr("tabindex", tabIndex++); });
                            //#endregion
                        }
                        else {
                            tablaPrenomina.find('input.dias').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.diasVacaciones').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.descuento').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.apoyoColectivo').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.licenciaLuto').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.prestamo').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.axa').each(function () { $(this).attr("tabindex", tabIndex++); });
                            //tablaPrenomina.find('input.descuentoFamsa').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.pensionAlimenticia').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.fonacot').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.infonavit').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.fondoAhorroNomina').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.fondoAhorroComplemento').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.bonoProduccion').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.otros').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.primaVacacional').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.hrExtra').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.diaHrExtra').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.diaFestivo').each(function () { $(this).attr("tabindex", tabIndex++); });

                            tablaPrenomina.find('input.cesantia').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.interesCesantia').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.retroactivo').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.prima').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.hrExtraDiurnasDominicales').each(function () { $(this).attr("tabindex", tabIndex++); });
                            tablaPrenomina.find('input.hrNocturnas').each(function () { $(this).attr("tabindex", tabIndex++); });
                        }
                    }
                },
                columns:
                    tipoNomina == 10 ? (
                        [
                            { data: 'id', title: 'id', visible: false },
                            { data: 'prenominaID', title: 'nominaID', visible: false },
                            { data: 'orden', title: 'Orden', visible: false },
                            { data: 'empleadoCve', title: 'Clave<br>Empl.' },
                            { data: 'empleadoNombre', title: 'Nombre Empleado' },
                            { data: 'puesto', title: 'Puesto' },
                            {
                                data: 'observaciones',
                                title: 'Fecha Alta',
                            },
                            {
                                data: 'sueldoSemanal',
                                title: 'Sueldo Periodo',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' sueldo captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                // render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }
                            },
                            {
                                data: 'dias',
                                title: 'Días<br>Aguinaldo',
                                //render: function (data, type, row) { return '<input type="number" oninput="this.value = Math.floor(this.value);" class="dias captura text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }, 
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' dias captura text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                                //visible: false  
                            },
                            {
                                data: 'nominaBase',
                                title: 'Base Nomina',
                                render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") },
                                visible: false
                            },
                            {
                                data: 'diasVacaciones',
                                title: 'Días Aguinaldo Vacaciones',
                                //render: function (data, type, row) { return '<input type="number" oninput="this.value = Math.floor(this.value);" class="dias captura text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }, 
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' dias captura text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },
                            {
                                data: 'nominaBaseVacaciones',
                                title: 'Base Nomina Vacaciones',
                                render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") },
                                visible: false
                            },
                            {
                                data: 'descuento',
                                title: 'Descuentos',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' descuento captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },
                            {
                                data: 'apoyoColectivo',
                                title: 'Apoyo Colectivo',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' apoyoColectivo captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },
                            {
                                data: 'prestamo',
                                title: 'Préstamo',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' prestamo captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },
                            {
                                data: 'axa',
                                title: 'Axa',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' axa captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },
                            {
                                data: 'descuentoFamsa',
                                title: 'Famsa',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' descuentoFamsa captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },

                            {
                                data: 'fonacot',
                                title: 'Fonacot',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' fonacot captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },
                            {
                                data: 'infonavit',
                                title: 'Infonavit',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' infonavit captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },
                            { data: 'sindicato', title: 'Sindicato', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            {
                                data: 'fondoAhorroNomina',
                                title: '2.5% Fondo Ahorro',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' fondoAhorroNomina captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },
                            {
                                data: 'diaExtraValor',
                                title: 'Dias<br>Disfrutados',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' diaExtraValor captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : 'disabled') + '>' },
                                visible: false
                            },
                            {
                                data: 'totalNomina',
                                title: 'Total<br>Aguinaldo',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' totalNomina captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : 'disabled') + '>' },
                            },
                            { data: 'complementoNomina', title: 'Comple-mento', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            {
                                data: 'fondoAhorroComplemento',
                                title: '2.5% Fondo Ahorro',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' fondoAhorroComplemento captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },//21
                            { data: 'onp', title: 'ONP', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            { data: 'afp', title: 'AFP', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            { data: 'afpSeguros', title: 'AFP<brSeguros', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            { data: 'afpComision', title: 'AFP<brComisión', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            { data: 'sta', title: '5TA', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            { data: 'esSalud', title: 'Aportación<br>esSalud', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            //{ data: 'fondoAhorroComplemento', title: '2% Fondo Ahorro', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false  },
                            { data: 'bonoZona', title: 'Bono Zona', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            {
                                data: 'bonoProduccion',
                                title: 'Bono de Producción',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' bonoProduccion captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },
                            {
                                data: 'otros',
                                title: 'Otros',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' otros captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },
                            {
                                data: 'diaHrExtra',
                                title: 'Años<br>Laborados',
                                render: function (data, type, row) { return '<input type="number" oninput="this.value = Math.floor(this.value);" class="diaHrExtra captura text-right" value=' + parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : 'disabled') + '>' },
                                //visible: false
                            },
                            {
                                data: 'diaFestivo',
                                title: 'Días<br>Vacaciones',
                                render: function (data, type, row) { return '<input type="number" oninput="this.value = Math.floor(this.value);" class="diaFestivo captura text-right" value=' + parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                            },
                            {
                                data: 'primaVacacional',
                                title: 'Prima<br>Vacacional',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' primaVacacional captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : 'disabled') + '>' }
                            },//31
                            {
                                data: 'pensionAlimenticia',
                                title: 'Pensión',
                                render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' pensionAlimenticia captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                            },
                            { data: 'primaDominical', title: 'Prima Dominical', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            {
                                data: 'hrExtra',
                                title: 'Hrs.<br>Extras<br>Diurnas',
                                render: function (data, type, row) { return '<input type="number" class="hrExtra captura text-right" value=' + parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                                visible: false
                            },
                            { data: 'hrExtraValor', title: 'Valor Horas Extra', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            { data: 'importeExtra', title: 'Importe de Horas Extra', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },


                            { data: 'importeDiaExtra', title: 'Importe de Días Extra', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },

                            { data: 'diaFestivoValor', title: 'Valor Día Extra', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            { data: 'importeDiaFestivo', title: 'Importe de Días Extra', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            { data: 'totalComplemento', title: 'Total de Complemento', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            //39
                            { data: 'asigFamiliar', title: 'Asig.<br>Familiar', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            { data: 'totalPagar', title: 'Total a Pagar', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            { data: 'porcentajeTotalPagar', title: '%', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            {
                                data: 'totalRealPagar',
                                title: 'Total<br>A Pagar',
                                render: function (data, type, row) { return '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }
                            },
                            { data: 'valesDespensa', title: 'Vales<br>Despensa', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            { data: 'totalDeposito', title: 'Total<br>Depósito', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                            { data: 'cuenta', title: 'Cuenta', visible: false },
                            { data: 'clabeInterbancaria', title: 'Clabe<br>Interbancaria', visible: false },
                            { data: 'banco', title: 'Banco', visible: false }
                        ]
                    ) :
                    [
                        { data: 'id', title: 'id', visible: false },
                        { data: 'fechaAntiguedad', title: 'Fecha<br>Alta' },
                        { data: 'prenominaID', title: 'nominaID', visible: false },
                        { data: 'orden', title: 'Orden', visible: false },
                        { data: 'empleadoCve', title: 'Clave<br>Empl.' },
                        { data: 'empleadoNombre', title: 'Nombre<br>Empleado' },
                        { data: 'puesto', title: 'Puesto', visible: false },
                        {
                            data: 'sueldoSemanal',
                            title: 'Sueldo Periodo',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' sueldo captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                            // render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }
                        },
                        {
                            data: 'dias',
                            title: 'Días   ',
                            //render: function (data, type, row) { return '<input type="number" oninput="this.value = Math.floor(this.value);" class="dias captura text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }, 
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' dias captura text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                            //visible: false  
                        },
                        {
                            data: 'nominaBase',
                            title: 'Base Nomina',
                            render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") },
                            visible: false
                        },
                        {
                            data: 'diasVacaciones',
                            title: 'Días<br>Vac',
                            //render: function (data, type, row) { return '<input type="number" oninput="this.value = Math.floor(this.value);" class="dias captura text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }, 
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' diasVacaciones captura text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                            //visible: false  
                        },
                        {
                            data: 'nominaBaseVacaciones',
                            title: 'Base Nomina Vacaciones',
                            render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") },
                            visible: false
                        },
                        {
                            data: 'descuento',
                            title: 'Descuentos',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' descuento captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                        },
                        {
                            data: 'apoyoColectivo',
                            title: 'Apoyo Colectivo',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' apoyoColectivo captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                        },
                        {
                            data: 'prestamo',
                            title: 'Préstamo',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' prestamo captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                        },
                        {
                            data: 'axa',
                            title: 'Otras percepciones',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' axa captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                        },
                        {
                            data: 'descuentoFamsa',
                            title: 'Famsa',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' descuentoFamsa captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                            visible: false
                        },
                        {
                            data: 'pensionAlimenticia',
                            title: 'Pensión',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' pensionAlimenticia captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                        },
                        {
                            data: 'fonacot',
                            title: 'Fonacot',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' fonacot captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },

                        },
                        {
                            data: 'infonavit',
                            title: 'Infonavit',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' infonavit captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                        },
                        { data: 'sindicato', title: 'Sindicato', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        {
                            data: 'fondoAhorroNomina',
                            title: '2.5% Fondo Ahorro',
                            visible: false,
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' fondoAhorroNomina captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                        },
                        //{ data: 'fondoAhorroNomina', title: '2% Fondo Ahorro', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false  },
                        { data: 'totalNomina', title: 'Total Base Nómina', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'complementoNomina', title: 'Comple-mento', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        {
                            data: 'fondoAhorroComplemento',
                            title: '2.5% Fondo Ahorro',
                            visible: false,
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' fondoAhorroComplemento captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                        },//21
                        { data: 'onp', title: 'ONP', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'afp', title: 'AFP', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'afpSeguros', title: 'AFP<brSeguros', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'afpComision', title: 'AFP<brComisión', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'sta', title: '5TA', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'esSalud', title: 'Aportación<br>esSalud', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        //{ data: 'fondoAhorroComplemento', title: '2% Fondo Ahorro', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false  },
                        { data: 'bonoZona', title: 'Bono Zona', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        {
                            data: 'bonoProduccion',
                            title: 'Bono de Producción',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' bonoProduccion captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                        },
                        {
                            data: 'otros',
                            title: 'Otros',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' otros captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                        },
                        {
                            data: 'primaVacacional',
                            title: 'Prima<br>Vacacional',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' primaVacacional captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' }
                        },//31
                        { data: 'primaDominical', title: 'Prima Dominical', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        {
                            data: 'hrExtra',
                            title: 'Horas Extra',
                            render: function (data, type, row) { return '<input type="number" class="hrExtra captura text-right" value=' + parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        { data: 'hrExtraValor', title: 'Valor Horas Extra', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'importeExtra', title: 'Importe de Horas Extra', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        {
                            data: 'diaHrExtra',
                            title: 'Días<br>Extra',
                            render: function (data, type, row) { return '<input type="number" oninput="this.value = Math.floor(this.value);" class="diaHrExtra captura text-right" value=' + parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        { data: 'diaExtraValor', title: 'Valor Día Extra', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'importeDiaExtra', title: 'Importe de Días Extra', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        {
                            data: 'diaFestivo',
                            title: 'Día<br>Festivo',
                            render: function (data, type, row) { return '<input type="number" oninput="this.value = Math.floor(this.value);" class="diaFestivo captura text-right" value=' + parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        { data: 'diaFestivoValor', title: 'Valor Día Extra', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'importeDiaFestivo', title: 'Importe de Días Extra', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'totalComplemento', title: 'Total de Complemento', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        //39
                        { data: 'asigFamiliar', title: 'Asig.<br>Familiar', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'totalPagar', title: 'Total a Pagar', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'porcentajeTotalPagar', title: '%', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        {
                            data: 'totalRealPagar',
                            title: 'Total<br>A Pagar',
                            render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") },
                            visible: false
                        },
                        { data: 'valesDespensa', title: 'Vales<br>Despensa', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'totalDeposito', title: 'Total<br>Depósito', render: function (data, type, row) { return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") }, visible: false },
                        { data: 'cuenta', title: 'Cuenta', visible: false },
                        { data: 'clabeInterbancaria', title: 'Clabe<br>Interbancaria', visible: false },
                        { data: 'banco', title: 'Banco', visible: false },
                        { data: 'observaciones', title: 'Observaciones', visible: false },

                    ],
                columnDefs: [
                    { className: "dt-center", targets: [0, 1, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42] },
                    { className: "dt-center", targets: [2, 3, 4, 5, 42, 43, 44, 45, 46, 47, 48] },
                    { width: '80px', targets: [0, 1, 6, 8, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 33, 34, 36, 37, 38, 39] },
                    { width: '30px', targets: [3, 7, 9, 32, 35] },
                    { width: '100px', targets: [2] },
                ]
            });

        }

        function InitTablaAutorizantes() {
            dtTablaAutorizantes = tablaAutorizantes.DataTable({
                rowReorder: {
                    dataSrc: 'orden'
                },
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 'Btip',
                order: [[0, "asc"]],
                //ordering: false,
                drawCallback: function (settings) {
                    tablaAutorizantes.find("th").each(function (e) {
                        $(this).unbind();
                    });
                    tablaAutorizantes.find("span.DataTables_sort_icon").css("display", "none");
                    tablaAutorizantes.find('input.esObra').bootstrapToggle();
                    tablaAutorizantes.find('input.esObra').change(function (e) { dtTablaAutorizantes.cell($(this).parents('tr'), 3).data($(this).prop("checked")).draw(); });
                },
                columns: [
                    { data: 'orden', title: 'Orden', className: 'reorder' },
                    { data: 'aprobadorNombre', title: 'Nombre' },
                    { data: 'aprobadorPuesto', title: 'Puesto' },
                    {
                        data: 'esObra',
                        title: 'Autorizante Obra',
                        render: function (data, type, row) {
                            html = '<input class="esObra" type="checkbox" ' + (data ? 'checked' : '') + ' data-toggle="toggle" data-on="Sí" data-off="No">';
                            return html;
                        }
                    },
                    {
                        title: '',
                        render: function (data, type, row) {
                            html = '<button class="btn btn-danger aplicaAutorizacion"> <i class="fas fa-times"></i> </button>';
                            return html;
                        }
                    },

                    { data: 'aprobadorClave', title: 'claveEmpleado', visible: false },


                ],
                columnDefs: [
                    { className: "dt-center", "targets": [1, 2, 3, 4, 5] },
                    { width: '20px', targets: [0, 3, 4, 5] },
                ],
                buttons: [
                    {
                        text: 'Agregar Autorizante',
                        action: function (e, dt, node, config) {
                            var prenominaID = botonReporte.attr("prenominaID");
                            if (prenominaID != null && !validada) AgregarAutorizante();
                        }
                    }
                ]
            });

        }

        function fnSelRevisa(event, ui) {
            $(this).data("id", ui.item.id);
            $(this).data("nombre", ui.item.value);
        }

        function fnSelNull(event, ui) {
            if (ui.item === null && $(this).val() != '') {
                $(this).val("");
                $(this).data("id", "");
                $(this).data("nombre", "");
                AlertaGeneral("Alerta", "Solo puede seleccionar un usuario de la lista, si no aparece en la lista de autocompletado favor de solicitar al personal de TI");
            }
        }

        function GetEmpresaActual() {
            $.blockUI({ message: 'Procesando...' });
            $.get('/Nomina/GetEmpresaActual', {})
                .then(
                    function (response) {
                        $.unblockUI();
                        empresa = response.empresa.Data;
                        Inicializadores();
                        //switch (empresa) {
                        //    case 1:
                        //        var optionesSolicitudCheque = [
                        //            { text: 'BANAMEX', value: 1 },
                        //            { text: 'SANTANDER', value: 2 },
                        //            { text: 'EICI', value: 3 }
                        //        ];
                        //        $.each(optionesSolicitudCheque, function (i, el) { selectBanco.append(new Option(el.text, el.value)); });
                        //        break;
                        //    default:
                        //        var optionesSolicitudCheque = [
                        //            { text: 'SANTANDER', value: 2 }
                        //        ];
                        //        $.each(optionesSolicitudCheque, function (i, el) { selectBanco.append(new Option(el.text, el.value)); });
                        //        break;
                        //}
                    },
                    function (error) {
                        $.unblockUI();
                    }
                );
        }

        function CargarPrenomina() {
            if (comboNomina.val() && comboCC.val()) {
                const dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
                //tipoPeriodo = dataPeriodo[2];
                tipoPeriodo = cboTipoNomina.val();
                periodo = comboNomina.val();
                anio = dataPeriodo[3];
                cc = comboCC.val()
                $.blockUI({ message: 'Procesando...' });
                $.get('/Nomina/CargarPrenomina', { CC: cc, periodo: periodo, tipoNomina: tipoPeriodo, anio: anio })
                    .then(
                        function (response) {
                            $.unblockUI();
                            if (response.success) {
                                tipoNomina = tipoPeriodo;
                                dtTablaPrenomina.clear();
                                dtTablaPrenomina.destroy();
                                InitTablaPrenomina();
                                validada = response.validada;
                                AddRows(tablaPrenomina, response.detalles);
                                AddRows(tablaAutorizantes, response.autorizantes);
                                botonReporte.attr("prenominaID", response.prenominaID);
                                botonValidar.attr("prenominaID", response.prenominaID);
                                botonDesValidar.attr("prenominaID", response.prenominaID);
                                botonReporte.css("display", "block");
                                inputNumEmpleados.val(response.detalles.length.toString());
                                if (validada) {
                                    botonGuardar.css("display", "none");
                                    botonValidar.css("display", "none");
                                    botonDesValidar.css("display", "inline-block");
                                    dtTablaAutorizantes.column(3).visible(false);
                                    dtTablaAutorizantes.column(4).visible(false);
                                }
                                else {
                                    botonGuardar.css("display", "block");
                                    botonDesValidar.css("display", "none");
                                    if (response.prenominaID > 0) {
                                        botonValidar.css("display", "block");
                                    }
                                    else {
                                        botonValidar.css("display", "none");
                                    }
                                    dtTablaAutorizantes.column(3).visible(true);
                                    dtTablaAutorizantes.column(4).visible(true);
                                }
                                tablaAutorizantes.find('button.aplicaAutorizacion').click(function (e) {
                                    dtTablaAutorizantes.row($(this).parents('tr')).remove().draw();
                                    var numRows = dtTablaAutorizantes.data().count();
                                    for (let i = 0; i < numRows; i++) { dtTablaAutorizantes.cell(i, 0).data((i + 1)).draw(); }
                                });
                            } else {
                                swal('Alerta!', response.message, 'warning');
                            }
                        },
                        function (error) {
                            swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                            $.unblockUI();
                        }
                    );
            }
            else {
                swal('Alerta!', 'Debe seleccionar todos los filtros', 'warning');
                $.unblockUI();
            }
        }

        function GuardarPrenomina() {
            var prenominaID = botonReporte.attr("prenominaID");
            var detalles = dtTablaPrenomina.data().toArray();
            var autorizantes = dtTablaAutorizantes.data().toArray();
            $.blockUI({ message: 'Procesando...' });
            return $.post('/Nomina/GuardarPrenomina', { prenominaID: prenominaID, detalles: detalles, autorizantes: autorizantes, tipoNomina: tipoPeriodo, periodo: periodo, anio: anio, CC: cc })
                .then(
                    function (response) {
                        if (response.success) {
                            Alert2Exito("Se han guardado los cambios correctamente");
                            CargarPrenomina();
                            botonReporte.attr("prenominaID", response.prenominaID);
                            botonValidar.attr("prenominaID", response.prenominaID);
                            botonReporte.css("display", "block");
                            botonValidar.css("display", "block");
                            botonGuardar.css("display", "block");
                            CargarPrenomina();
                            $.unblockUI();
                        }
                        else {
                            swal('Alerta!', response.message, 'warning');
                            $.unblockUI();
                        }
                    },
                    function (error) {
                        swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                        $.unblockUI();
                    });
        }

        function DescargarReporte() {
            var prenominaID = botonReporte.attr("prenominaID");
            if (prenominaID > 0) {
                $(this).download = '/Nomina/CrearExcelPrenomina?prenominaID=' + prenominaID;
                $(this).href = '/Nomina/CrearExcelPrenomina?prenominaID=' + prenominaID;
                location.href = '/Nomina/CrearExcelPrenomina?prenominaID=' + prenominaID;
            }
            else {
                swal('Alerta!', "No se ha cargado prenomina", 'warning');
            }
        }

        function ConfirmarValidacion() {
            Alert2AccionConfirmar("Confirmación", "Esta a punto de validar la prenomina y sus autorizantes, ¿desea cotinuar?", "Confirmar", "Cancelar", () => ValidarReporte());
        }

        function ConfirmarEliminarValidacion() {
            Alert2AccionConfirmar("Confirmación", "Esta a punto de eliminar la validación de la prenomina y sus autorizantes, ¿desea cotinuar?", "Confirmar", "Cancelar", () => EliminarValidacion());
        }

        function ValidarReporte() {
            var prenominaID = botonValidar.attr("prenominaID");
            var detalles = dtTablaPrenomina.data().toArray();
            var autorizantes = dtTablaAutorizantes.data().toArray();
            $.blockUI({ message: 'Procesando...' });
            return $.post('/Nomina/ValidarPrenomina', { prenominaID: prenominaID, detalles: detalles, autorizantes: autorizantes })
                .then(
                    function (response) {
                        if (response.success) {
                            CargarPrenomina();
                            Alert2Exito("Se ha validado correctamente la prenomina");
                            botonReporte.css("display", "block");
                            botonValidar.css("display", "none");
                            botonGuardar.css("display", "none");
                            $.unblockUI();
                        }
                        else {
                            swal('Alerta!', response.message, 'warning');
                            $.unblockUI();
                        }
                    },
                    function (error) {
                        swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                        $.unblockUI();
                    });
        }

        function EliminarValidacion() {
            var prenominaID = botonDesValidar.attr("prenominaID");
            $.blockUI({ message: 'Procesando...' });
            return $.post('/Nomina/DesValidarPrenomina', { prenominaID: prenominaID })
                .then(
                    function (response) {
                        if (response.success) {
                            CargarPrenomina();
                            Alert2Exito("Se ha eliminado la validación correctamente la prenomina");
                            botonReporte.css("display", "block");
                            botonValidar.css("display", "block");
                            botonDesValidar.css("display", "none");
                            $.unblockUI();
                        }
                        else {
                            swal('Alerta!', response.message, 'warning');
                            $.unblockUI();
                        }
                    },
                    function (error) {
                        swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                        $.unblockUI();
                    });
        }

        function AgregarAutorizante() {
            comboAutorizante.val('').change();
            modalAgregarAutorizante.modal("show");
        }

        function GuardarAutorizante() {
            if (comboAutorizante.val()) {
                var auxOrden = dtTablaAutorizantes.data().count() + 1;
                dtTablaAutorizantes.row.add({
                    "aprobadorClave": +comboAutorizante.val(),
                    "aprobadorNombre": $("#comboAutorizante option:selected").text(),
                    "aprobadorPuesto": $("#comboAutorizante option:selected").attr("data-prefijo"),
                    "autorizando": false,
                    "comentario": null,
                    "estatus": 0,
                    "fecha": null,
                    "firma": null,
                    "id": 0,
                    "orden": auxOrden,
                    "prenomina": null,
                    "prenominaID": 0,
                    "tipo": null,
                    "esObra": false,
                }).draw(false);
                modalAgregarAutorizante.modal("hide");

                tablaAutorizantes.find('input.esObra').bootstrapToggle();
                tablaAutorizantes.find('button.aplicaAutorizacion').click(function (e) {
                    dtTablaAutorizantes.row($(this).parents('tr')).remove().draw();
                    var numRows = dtTablaAutorizantes.data().count();
                    for (let i = 0; i < numRows; i++) { dtTablaAutorizantes.cell(i, 0).data((i + 1)).draw(); }
                });
                tablaAutorizantes.find('input.esObra').change(function (e) {
                    dtTablaAutorizantes.cell($(this).parents('tr'), 3).data($(this).prop("checked")).draw();
                });
            }
            else swal('Alerta!', 'Seleccione autorizante', 'warning');
        }

        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function imprimirSolicitudCheque() {
            let periodo = comboNomina.val();
            let dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
            let tipoNomina = dataPeriodo[2];
            let year = dataPeriodo[3];
            let banco = selectBanco.val();

            if (periodo <= 0 || isNaN(periodo)) {
                AlertaGeneral(`Alerta`, `Debe seleccionar un periodo.`);
                return;
            }

            $.blockUI({ message: 'Generando solicitud de cheque...' });
            var path = `/Reportes/Vista.aspx?idReporte=248&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=${banco}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
                modalSolicitudCheque.modal('hide');
            };
        }

        function imprimirCedulaCostos() {
            let periodo = comboNomina.val();
            let dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
            let tipoNomina = dataPeriodo[2];
            let year = dataPeriodo[3];
            let banco = selectBanco.val();

            if (periodo <= 0 || isNaN(periodo)) {
                AlertaGeneral(`Alerta`, `Debe seleccionar un periodo.`);
                return;
            }

            $.blockUI({ message: 'Generando solicitud de cheque...' });
            var path = `/Reportes/Vista.aspx?idReporte=259&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=${banco}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
                modalSolicitudCheque.modal('hide');
            };
        }

        function imprimirPolizaOCSI() {
            let periodo = comboNomina.val();
            let dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
            let tipoNomina = dataPeriodo[2];
            let year = dataPeriodo[3];
            let banco = selectBanco.val();

            if (periodo <= 0 || isNaN(periodo)) {
                AlertaGeneral(`Alerta`, `Debe seleccionar un periodo.`);
                return;
            }

            $.blockUI({ message: 'Generando solicitud de cheque...' });
            var path = `/Reportes/Vista.aspx?idReporte=261&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=${banco}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
                modalSolicitudCheque.modal('hide');
            };
        }

        function solicitarCorreoDespacho() {
            let periodo = comboNomina.val();
            let dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
            let tipoNomina = dataPeriodo[2];
            let year = dataPeriodo[3];

            if (periodo <= 0 || isNaN(periodo)) {
                AlertaGeneral(`Alerta`, `Debe seleccionar un periodo.`);
                return;
            }
            $.blockUI({ message: 'Enviando Correo...' });
            var path = `/Reportes/Vista.aspx?idReporte=248&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=1&inMemory=1`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                path = `/Reportes/Vista.aspx?idReporte=252&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=2&inMemory=1`;
                report.attr("src", path);
                document.getElementById('report').onload = function () {
                    path = `/Reportes/Vista.aspx?idReporte=253&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=3&inMemory=1`;
                    report.attr("src", path);
                    document.getElementById('report').onload = function () {
                        path = `/Reportes/Vista.aspx?idReporte=262&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=4&inMemory=1`;
                        report.attr("src", path);
                        document.getElementById('report').onload = function () {
                            path = `/Reportes/Vista.aspx?idReporte=259&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=1&inMemory=1`;
                            report.attr("src", path);
                            document.getElementById('report').onload = function () {
                                path = `/Reportes/Vista.aspx?idReporte=260&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=2&inMemory=1`;
                                report.attr("src", path);
                                document.getElementById('report').onload = function () {
                                    path = `/Reportes/Vista.aspx?idReporte=263&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=3&inMemory=1`;
                                    report.attr("src", path);
                                    document.getElementById('report').onload = function () {
                                        path = `/Reportes/Vista.aspx?idReporte=264&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=4&inMemory=1`;
                                        report.attr("src", path);
                                        document.getElementById('report').onload = function () {
                                            $.ajax({
                                                datatype: "json",
                                                type: "POST",
                                                url: '/Nomina/EnviarCorreoDespacho',
                                                data: { periodo: periodo, tipoNomina: tipoPeriodo, anio: year },
                                                success: function (response) {
                                                    $.unblockUI();
                                                    if (response.success) {
                                                        Alert2Exito("Se ha enviado el correo con éxito");
                                                    } else {
                                                        swal('Alerta!', response.message, 'warning');
                                                    }
                                                },
                                                error: function () {
                                                    $.unblockUI();
                                                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        function solicitarCorreoSolicitudes() {
            let periodo = comboNomina.val();
            let dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
            let tipoNomina = dataPeriodo[2];
            let year = dataPeriodo[3];

            if (periodo <= 0 || isNaN(periodo)) {
                AlertaGeneral(`Alerta`, `Debe seleccionar un periodo.`);
                return;
            }
            $.blockUI({ message: 'Enviando Correo...' });
            var path = `/Reportes/Vista.aspx?idReporte=248&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=1&inMemory=1`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                path = `/Reportes/Vista.aspx?idReporte=252&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=2&inMemory=1`;
                report.attr("src", path);
                document.getElementById('report').onload = function () {
                    path = `/Reportes/Vista.aspx?idReporte=253&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=3&inMemory=1`;
                    report.attr("src", path);
                    document.getElementById('report').onload = function () {
                        path = `/Reportes/Vista.aspx?idReporte=262&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=4&inMemory=1`;
                        report.attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.ajax({
                                datatype: "json",
                                type: "POST",
                                url: '/Nomina/EnviarCorreoSolicitudes',
                                data: { periodo: periodo, tipoNomina: tipoPeriodo, anio: year },
                                success: function (response) {
                                    $.unblockUI();
                                    if (response.success) {
                                        Alert2Exito("Se ha enviado el correo con éxito");
                                    } else {
                                        swal('Alerta!', response.message, 'warning');
                                    }
                                },
                                error: function () {
                                    $.unblockUI();
                                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                                }
                            });
                        }
                    }
                }
            }
        }

        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                txtFecha.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }
        function setSemanaSelecionada() {
            date = txtFecha.datepicker('getDate');
            prevDom = date.getDate() - (date.getDay() + 14) % 7;
            startDate = new Date(date.getFullYear(), date.getMonth(), prevDom);
            endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 6);
            noSemana = endDate.noSemana();
            txtFecha.val(`Semana ${noSemana} - ${startDate.toLocaleDateString()} - ${endDate.toLocaleDateString()}`);
            selectCurrentWeek();
        }

        init();
    };

    $(document).ready(function () {

        Administrativo.Contabilidad.Prenomina = new Prenomina();
    });
})();