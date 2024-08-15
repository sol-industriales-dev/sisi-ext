(() => {
    $.namespace('recursoshumanos.reportesrh.Constancias');

    Constancias = function () {

        //#region CONSTS Reportes
        const cboCC = $('#cboCC');
        const tblConstancias = $('#tblConstancias');
        const ModalGuarderia = $('#ModalGuarderia');
        const ModalLactancia = $('#ModalLactancia');
        const checkActivo = $('#checkActivo');
        const btnBuscar = $('#btnBuscar');

        var estatus = "B";

        let dtConstancias;

        //#region CONSTS Guarderia          
        const report = $("#report");
        const btnGuarderia = $("#btnGuarderia");
        const descripcionCCGuard = $("#descripcionCCGuard");
        const nombreCompletoGuard = $("#nombreCompletoGuard");
        const nombrePuestoGuard = $("#nombrePuestoGuard");
        const divHijosEmpleados = $('#divHijosEmpleados');

        const btnCerrarLactancia = $("#btnCerrarLactancia");
        const btnCerrarGuarderia = $("#btnCerrarGuarderia");
        const btnCerrarLaboral = $("#btnCerrarLaboral");

        const horaEntradaD = $('#horaEntradaD');
        const horaSalidaD = $('#horaSalidaD');
        const chkDescanso = $('#chkDescanso');
        //#endregion    

        //#region CONSTS Laboral  
        const ModalLaboral = $('#ModalLaboral');
        const btnLaboral = $("#btnLaboral");
        const nombreCompletoLab = $('#nombreCompletoLab');
        const nombrePuestoLab = $('#nombrePuestoLab');
        const numeroPatronal = $('#numeroPatronal');
        const nombreRegPatronal = $('#nombreRegPatronal');
        const numeroSeguroLab = $('#numeroSeguroLab');
        const rfcLab = $('#rfcLab');
        const curpLab = $('#curpLab');
        const proyectoCCLab = $('#proyectoCCLab');
        const sueldoBaseLab = $('#sueldoBaseLab');
        const complementoLab = $('#complementoLab');
        const mensualNetoLab = $('#mensualNetoLab');
        const tipoNominaLab = $('#tipoNominaLab');
        const fechaAltaLab = $('#fechaAltaLab');
        const fechaBajaLab = $('#fechaBajaLab');
        const contratableLab = $('#contratableLab');

        let valBaseNeto = 0;


        //#endregion 

        //#region CONSTS Lactancia
        const btnLactancia = $("#btnLactancia");
        const inputPuestoLact = $('#inputPuestoLact');
        const inputNombreLact = $('#inputNombreLact');
        const inputSexo = $('#inputSexo');
        const cboMotivoLact = $('#cboMotivoLact');
        const inputFechaInicioPermiso = $('#inputFechaInicioPermiso');
        const inputFechaFinPermiso = $('#inputFechaFinPermiso');
        const spanTipoHorario = $('#spanTipoHorario');
        //#endregion

        //#region CONSTS Fonacot

        const nombreCompleto = $('#nombreCompleto');
        const nombrePuesto = $('#nombrePuesto');
        const regPatron = $('#regPatron');
        const nombrePatron = $('#nombrePatron');
        const imss = $('#imss');
        const rfc = $('#rfc');
        const curp = $('#curp');
        const sueldoBase = $('#sueldoBase');
        const complemento = $('#complemento');
        const mensualNeto = $('#mensualNeto');
        const tipoNomina = $('#tipoNomina');
        const fechaIngreso = $('#fechaIngreso');
        const ccDescripcionFonacot = $('#ccDescripcionFonacot');

        //#endregion

        $('#inputFechaInicioPermiso').datepicker({
            format: 'DD/MM/YYYY',
        });

        $('#inputFechaFinPermiso').datepicker({
            format: 'DD/MM/YYYY',
        });


        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            chkDescanso;
            initTblConstancias();
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");

            cboCC.on('change', function () {
                fncGetConsultaCC();
            });
            btnBuscar.click(function () {
                fncGetConsultaCC();
            });
            btnCerrarLactancia.click(function () {
                limpiarModal();
                ModalLactancia.modal("hide");
            });
            btnCerrarLaboral.click(function () {
                limpiarModal();
                ModalLaboral.modal("hide");
            });
            btnCerrarGuarderia.click(function () {
                limpiarModal();
                $("select[type='text']").val("");
                divHijosEmpleados.html(`<div class="col-md-4">
                                         <label class="">Nombre del Hijo(a):</label>
                                          <select type="text" id="nombreHijo" class="form-control"></select>
                                        </div>`);
                ModalGuarderia.modal("hide");
            });
            btnLaboral.click(function () {
                //Validation/         

                if ($('#contratableLab').val() == "N") {
                    Alert2Error('No se puede imprimir el formato, El usuario no es contratable.');
                    return;
                }
                var laboral = {
                    tituloLab: $('#inputTituloLaboral').val(),
                    nombreCompletoLab: $('#nombreCompletoLab').val(),
                    nombrePuestoLab: $('#nombrePuestoLab').val(),
                    numeroPatronal: $('#numeroPatronal').val(),
                    nombreRegPatronal: $('#nombreRegPatronal').val(),
                    numeroSeguroLab: $('#numeroSeguroLab').val(),
                    rfcLab: $('#rfcLab').val(),
                    curpLab: $('#curpLab').val(),
                    proyectoCCLab: $('#proyectoCCLab').val(),
                    motivoLab: $('#motivoLab').val(),
                    sueldoBaseLab: $('#sueldoBaseLab').val(),
                    complementoLab: $('#complementoLab').val(),
                    mensualNetoLab: $('#mensualNetoLab').val(),
                    tipoNominaLab: $('#tipoNominaLab').val(),
                    fechaAltaLab: $('#fechaAltaLab').val(),
                    fechaBajaLab: $('#fechaBajaLab').val(),
                    mensualNetoLab: mensualNetoLab.val(),
                    baseNeto: valBaseNeto,
                    status: estatus,

                    valorLetraLab: numeroALetras(+mensualNetoLab.val(), { plural: "PESOS", singular: "PESO", centPlural: "CENTAVOS", centSingular: "CENTAVO" }),
                    valorLetraBase: numeroALetras(+valBaseNeto, { plural: "PESOS", singular: "PESO", centPlural: "CENTAVOS", centSingular: "CENTAVO" }),

                    mostrarSueldoLab: $('#mostrarSueldoLab').val(),
                }

                let obj = {
                    cveEmpleado: btnLaboral.data("clave_empleado"),
                    idReporte: btnLaboral.data("id_reporte"),
                }

                axios.post("GuardarExpediciones", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //CODE...
                        axios.post('SetInformacionLaboral', { laboral })
                            .then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    console.log(btnLaboral.data("clave_empleado"));
                                    report.attr("src", `/Reportes/Vista.aspx?idReporte=${266}&inMemory=1&idEmpleado=${btnLaboral.data("clave_empleado")}&idExp=${items.id}`);
                                    report.on('load', function () {
                                        $.unblockUI();
                                        openCRModal();
                                        limpiarModal();
                                        ModalLaboral.modal("hide");
                                    });
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    }
                }).catch(error => Alert2Error(error.message));


            });
            btnGuarderia.click(function () {
                //Validation/         
                if ($('#inputGuarderia').val() == "") {
                    Alert2Error('Campo de Guarderia Requerido');
                    return;
                }
                if ($('#inputDirector').val() == "") {
                    Alert2Error('Nombre del Director Requerido');
                    return;
                }
                if ($('#nombreHijo').val() == "") {
                    Alert2Error('Nombre del hijo Requerido');
                    return;
                }
                if ($('#inputVacaciones').val() == "") {
                    Alert2Error('Campo de vacaciones Requerido');
                    return;
                }
                if ($('#horaEntrada').val() == "") {
                    Alert2Error('Campo de hora de entrada necesario');
                    return;
                }
                if ($('#horaSalida').val() == "") {
                    Alert2Error('Campo de hora de salida necesario');
                    return;
                }
                if ($('#horaComida').val() == "") {
                    Alert2Error('Campo de hora de comida necesario');
                    return;
                }
                if ($('#horaEntradaS').val() == "") {
                    Alert2Error('Campo de hora de entrada sabatino necesario');
                    return;
                }
                if ($('#horaSalidaS').val() == "") {
                    Alert2Error('Campo de hora de salida sabatino necesario');
                    return;
                }

                if (!chkDescanso.prop("checked")) {
                    if (horaEntradaD.val() == "") {
                        Alert2Error('Campo de hora de entrada de Domingo necesario');
                        return;
                    }

                    if (horaSalidaD.val() == "") {
                        Alert2Error('Campo de hora de salida de Domingo necesario');
                        return;
                    }
                }

                var guarderia = {
                    nombreCompletoGuard: $('#nombreCompletoGuard').val(),
                    nombrePuestoGuard: $('#nombrePuestoGuard').val(),
                    regPatron: $('#regPatron').val(),
                    nombrePatron: $('#nombrePatron').val(),
                    imss: $('#imss').val(),
                    rfc: $('#rfc').val(),
                    curp: $('#curp').val(),
                    descripcionCCGuard: $('#descripcionCCGuard').val(),
                    fechaIngreso: $('#fechaIngreso').val(),
                    guarderia: $('#inputGuarderia').val(),
                    director: $('#inputDirector').val(),
                    nombreHijo: $('#nombreHijo').val(),
                    horaEntrada: $('#horaEntrada').val(),
                    horaSalida: $('#horaSalida').val(),
                    horaComida: $('#horaComida').val(),
                    horaEntradaS: $('#horaEntradaS').val(),
                    horaSalidaS: $('#horaSalidaS').val(),
                    horaEntradaD: horaEntradaD.val(),
                    horaSalidaD: horaSalidaD.val(),
                    diasTrab: $('#diasTrab').val(),
                    diasDesc: $('#diasDesc').val(),
                    vacaciones: $('#inputVacaciones').val(),

                }

                let obj = {
                    cveEmpleado: btnLaboral.data("clave_empleado"),
                    idReporte: btnLaboral.data("id_reporte"),
                }

                axios.post("GuardarExpediciones", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //CODE...
                        axios.post('SetInformacionGuarderia', { guarderia })
                            .then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    report.attr("src", `/Reportes/Vista.aspx?idReporte=${271}&idEmpleado=${btnLaboral.data("clave_empleado")}&inMemory=1&idExp=${items.id}&esdomDescanso=${chkDescanso.prop("checked")}`);
                                    report.on('load', function () {
                                        $.unblockUI();
                                        openCRModal();
                                        ModalGuarderia.modal("hide");
                                        limpiarModal();

                                    });
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    }
                }).catch(error => Alert2Error(error.message));

                // axios.post('SetInformacionGuarderia', { guarderia })
                //     .then(response => {
                //         let { success, datos, message } = response.data;

                //         if (success) {
                //             report.attr("src", `/Reportes/Vista.aspx?idReporte=${271}&idEmpleado=${btnGuarderia.data().clave_empleado}`);
                //             report.on('load', function () {
                //                 $.unblockUI();
                //                 openCRModal();
                //             });
                //         } else {
                //             AlertaGeneral(`Alerta`, message);
                //         }
                //     }).catch(error => AlertaGeneral(`Alerta`, error.message));
            });
            btnLactancia.click(function () {
                //Validation/         
                if ($('#inputFechaInicioPermiso').val() == "") {
                    Alert2Error('Fecha de Inicio Requerido');
                    return;
                }
                if ($('#inputFechaFinPermiso').val() == "") {
                    Alert2Error('Fecha Final Requerido');
                    return;
                }

                if (cboMotivoLact.val() == "lactancia" && $('#inputSexo').val() == "M") {
                    Alert2Error('Solo sexo Femenino');
                    return;
                }

                if (cboMotivoLact.val() == "lactancia" && $('#inputFechaInicioInca').val() == "N/A") {
                    Alert2Error('Solo personas con Incapacidad');
                    return;
                }
                if (cboMotivoLact.val() == "lactancia" && $('#inputFechaFinInca').val() == "N/A") {
                    Alert2Error('Solo personas con Incapacidad');
                    return;
                }

                if (cboMotivoLact.val() == "N/A") {
                    Alert2Error('Seleccione un motivo');
                    return;

                }

                var lactancia = {
                    nombreCompletoLact: $('#inputNombreLact').val(),
                    nombrePuestoLact: $('#inputPuestoLact').val(),
                    fechaInicioInca: $('#inputFechaInicioInca').val(),
                    fechaFinInca: $('#inputFechaFinInca').val(),
                    fechaInicioPermiso: $('#inputFechaInicioPermiso').val(),
                    fechaFinPermiso: $('#inputFechaFinPermiso').val(),


                    entradaLunes: $('#inputEntradaLunes').val(),
                    salidaLunes: $('#inputSalidaLunes').val(),
                    comidaLunes: $('#inputComidaLunes').val(),

                    entradaMartes: $('#inputEntradaMartes').val(),
                    salidaMartes: $('#inputSalidaMartes').val(),
                    comidaMartes: $('#inputComidaMartes').val(),

                    entradaMiercoles: $('#inputEntradaMiercoles').val(),
                    salidaMiercoles: $('#inputSalidaMiercoles').val(),
                    comidaMiercoles: $('#inputComidaMiercoles').val(),

                    entradaJueves: $('#inputEntradaJueves').val(),
                    salidaJueves: $('#inputSalidaJueves').val(),
                    comidaJueves: $('#inputComidaJueves').val(),

                    entradaViernes: $('#inputEntradaViernes').val(),
                    salidaViernes: $('#inputSalidaViernes').val(),
                    comidaViernes: $('#inputComidaViernes').val(),

                    entradaSabado: $('#inputEntradaSabado').val(),
                    salidaSabado: $('#inputSalidaSabado').val(),
                    comidaSabado: $('#inputComidaSabado').val(),

                    entradaDomingo: $('#inputEntradaDomingo').val(),
                    salidaDomingo: $('#inputSalidaDomingo').val(),
                    comidaDomingo: $('#inputComidaDomingo').val(),


                    entradaLunesL: $('#inputEntradaLunesL').val(),
                    salidaLunesL: $('#inputSalidaLunesL').val(),
                    comidaLunesL: $('#inputComidaLunesL').val(),

                    entradaMartesL: $('#inputEntradaMartesL').val(),
                    salidaMartesL: $('#inputSalidaMartesL').val(),
                    comidaMartesL: $('#inputComidaMartesL').val(),

                    entradaMiercolesL: $('#inputEntradaMiercolesL').val(),
                    salidaMiercolesL: $('#inputSalidaMiercolesL').val(),
                    comidaMiercolesL: $('#inputComidaMiercolesL').val(),

                    entradaJuevesL: $('#inputEntradaJuevesL').val(),
                    salidaJuevesL: $('#inputSalidaJuevesL').val(),
                    comidaJuevesL: $('#inputComidaJuevesL').val(),

                    entradaViernesL: $('#inputEntradaViernesL').val(),
                    salidaViernesL: $('#inputSalidaViernesL').val(),
                    comidaViernesL: $('#inputComidaViernesL').val(),

                    entradaSabadoL: $('#inputEntradaSabadoL').val(),
                    salidaSabadoL: $('#inputSalidaSabadoL').val(),
                    comidaSabadoL: $('#inputComidaSabadoL').val(),

                    entradaDomingoL: $('#inputEntradaDomingoL').val(),
                    salidaDomingoL: $('#inputSalidaDomingoL').val(),
                    comidaDomingoL: $('#inputComidaDomingoL').val(),

                }

                let obj = {
                    cveEmpleado: btnLaboral.data("clave_empleado"),
                    idReporte: btnLaboral.data("id_reporte"),
                }


                axios.post("GuardarExpediciones", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //CODE...
                        axios.post('SetInformacionLactancia', { lactancia })
                            .then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    report.attr("src", `/Reportes/Vista.aspx?idReporte=${272}&idEmpleado=${btnLaboral.data("clave_empleado")}&inMemory=1&idExp=${items.id}&tipoHorario=${cboMotivoLact.val()}`);
                                    report.on('load', function () {
                                        $.unblockUI();
                                        openCRModal();
                                        ModalLactancia.modal("hide");
                                        limpiarModal();
                                    });
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    }
                }).catch(error => Alert2Error(error.message));

                // axios.post('SetInformacionLactancia', { lactancia })
                //     .then(response => {
                //         let { success, datos, message } = response.data;

                //         if (success) {
                //             report.attr("src", `/Reportes/Vista.aspx?idReporte=${272}&idEmpleado=${btnLactancia.data().clave_empleado}`);
                //             report.on('load', function () {
                //                 $.unblockUI();
                //                 openCRModal();
                //             });
                //         } else {
                //             AlertaGeneral(`Alerta`, message);
                //         }
                //     }).catch(error => AlertaGeneral(`Alerta`, error.message));
            });
            checkActivo.change(function () {
                if (document.getElementById('checkActivo').checked) {
                    estatus = "A";
                    dtConstancias.column(3).visible(true);
                    dtConstancias.column(4).visible(false);
                    dtConstancias.column(5).visible(true);
                    dtConstancias.column(6).visible(true);
                    dtConstancias.column(7).visible(true);
                    // dtConstancias.column(8).visible(true);
                    fncGetConsultaCC();
                } else {
                    estatus = "B";
                    dtConstancias.column(3).visible(true);
                    dtConstancias.column(4).visible(true);
                    dtConstancias.column(5).visible(false);
                    dtConstancias.column(6).visible(false);
                    dtConstancias.column(7).visible(false);
                    // dtConstancias.column(8).visible(false);

                    fncGetConsultaCC();
                }
            });

            cboMotivoLact.on("change", function () {
                if ($(this).val() == "lactancia") {
                    spanTipoHorario.text("Lactancia");

                } else if ($(this).val() == "especial") {
                    spanTipoHorario.text("Especial");

                } else {
                    spanTipoHorario.text("N/A");

                }
            });

            chkDescanso.on("change", function () {
                if ($(this).prop("checked")) {
                    horaEntradaD.prop("disabled", true);
                    horaSalidaD.prop("disabled", true);
                } else {
                    horaEntradaD.prop("disabled", false);
                    horaSalidaD.prop("disabled", false);
                }
            })
        }
        var numeroALetras = (function () {

            function Unidades(num) {

                switch (num) {
                    case 1:
                        return 'UN';
                    case 2:
                        return 'DOS';
                    case 3:
                        return 'TRES';
                    case 4:
                        return 'CUATRO';
                    case 5:
                        return 'CINCO';
                    case 6:
                        return 'SEIS';
                    case 7:
                        return 'SIETE';
                    case 8:
                        return 'OCHO';
                    case 9:
                        return 'NUEVE';
                }

                return '';
            } //Unidades()

            function Decenas(num) {

                let decena = Math.floor(num / 10);
                let unidad = num - (decena * 10);

                switch (decena) {
                    case 1:
                        switch (unidad) {
                            case 0:
                                return 'DIEZ';
                            case 1:
                                return 'ONCE';
                            case 2:
                                return 'DOCE';
                            case 3:
                                return 'TRECE';
                            case 4:
                                return 'CATORCE';
                            case 5:
                                return 'QUINCE';
                            default:
                                return 'DIECI' + Unidades(unidad);
                        }
                    case 2:
                        switch (unidad) {
                            case 0:
                                return 'VEINTE';
                            default:
                                return 'VEINTI' + Unidades(unidad);
                        }
                    case 3:
                        return DecenasY('TREINTA', unidad);
                    case 4:
                        return DecenasY('CUARENTA', unidad);
                    case 5:
                        return DecenasY('CINCUENTA', unidad);
                    case 6:
                        return DecenasY('SESENTA', unidad);
                    case 7:
                        return DecenasY('SETENTA', unidad);
                    case 8:
                        return DecenasY('OCHENTA', unidad);
                    case 9:
                        return DecenasY('NOVENTA', unidad);
                    case 0:
                        return Unidades(unidad);
                }
            } //Unidades()

            function DecenasY(strSin, numUnidades) {
                if (numUnidades > 0)
                    return strSin + ' Y ' + Unidades(numUnidades)

                return strSin;
            } //DecenasY()

            function Centenas(num) {
                let centenas = Math.floor(num / 100);
                let decenas = num - (centenas * 100);

                switch (centenas) {
                    case 1:
                        if (decenas > 0)
                            return 'CIENTO ' + Decenas(decenas);
                        return 'CIEN';
                    case 2:
                        return 'DOSCIENTOS ' + Decenas(decenas);
                    case 3:
                        return 'TRESCIENTOS ' + Decenas(decenas);
                    case 4:
                        return 'CUATROCIENTOS ' + Decenas(decenas);
                    case 5:
                        return 'QUINIENTOS ' + Decenas(decenas);
                    case 6:
                        return 'SEISCIENTOS ' + Decenas(decenas);
                    case 7:
                        return 'SETECIENTOS ' + Decenas(decenas);
                    case 8:
                        return 'OCHOCIENTOS ' + Decenas(decenas);
                    case 9:
                        return 'NOVECIENTOS ' + Decenas(decenas);
                }

                return Decenas(decenas);
            } //Centenas()

            function Seccion(num, divisor, strSingular, strPlural) {
                let cientos = Math.floor(num / divisor)
                let resto = num - (cientos * divisor)

                let letras = '';

                if (cientos > 0)
                    if (cientos > 1)
                        letras = Centenas(cientos) + ' ' + strPlural;
                    else
                        letras = strSingular;

                if (resto > 0)
                    letras += '';

                return letras;
            } //Seccion()

            function Miles(num) {
                let divisor = 1000;
                let cientos = Math.floor(num / divisor)
                let resto = num - (cientos * divisor)

                let strMiles = Seccion(num, divisor, 'UN MIL', 'MIL');
                let strCentenas = Centenas(resto);

                if (strMiles == '')
                    return strCentenas;

                return strMiles + ' ' + strCentenas;
            } //Miles()

            function Millones(num) {
                let divisor = 1000000;
                let cientos = Math.floor(num / divisor)
                let resto = num - (cientos * divisor)

                let strMillones = Seccion(num, divisor, 'UN MILLON DE', 'MILLONES DE');
                let strMiles = Miles(resto);

                if (strMillones == '')
                    return strMiles;

                return strMillones + ' ' + strMiles;
            } //Millones()

            return function NumeroALetras(num, currency) {
                currency = currency || {};
                let data = {
                    numero: num,
                    enteros: Math.floor(num),
                    centavos: (((Math.round(num * 100)) - (Math.floor(num) * 100))),
                    letrasCentavos: '',
                    letrasMonedaPlural: currency.plural || 'PESOS', //'PESOS', 'Dólares', 'Bolívares', 'etcs'
                    letrasMonedaSingular: currency.singular || 'PESO', //'PESO', 'Dólar', 'Bolivar', 'etc'
                    letrasMonedaCentavoPlural: currency.centPlural || 'PESOS',
                    letrasMonedaCentavoSingular: currency.centSingular || 'PESO'
                };

                if (data.centavos > 0) {
                    data.letrasCentavos = 'CON ' + (function () {
                        if (data.centavos == 1)
                            return Millones(data.centavos) + ' ' + data.letrasMonedaCentavoSingular;
                        else
                            return Millones(data.centavos) + ' ' + data.letrasMonedaCentavoPlural;
                    })();
                };

                if (data.enteros == 0)
                    return 'CERO ' + data.letrasMonedaPlural + ' ' + data.letrasCentavos;
                if (data.enteros == 1)
                    return Millones(data.enteros) + ' ' + data.letrasMonedaSingular + ' ' + data.letrasCentavos;
                else
                    return Millones(data.enteros) + ' ' + data.letrasMonedaPlural + ' ' + data.letrasCentavos;
            };

        })();
        function initTblConstancias() {
            dtConstancias = tblConstancias.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [

                    { data: 'clave_empleado', title: 'CLAVE_EMPLEADO' },
                    { data: 'nombreCompleto', title: 'NOMBRE COMPLETO' },
                    { data: 'nombrePuesto', title: 'PUESTO' },
                    // { data: 'contratable', title: 'contratable' },
                    //Activos 
                    {
                        data: 'propiedad', title: 'CONSTANCIA LABORAL',
                        render: (data, type, row, meta) => {
                            return `<button class='btn btn-xs btn-warning imprimirLaboral' title='Imprimir.'><i class='fas fa-print'></i></button>&nbsp;`
                        }
                    },
                    {
                        data: 'propiedad', title: 'CARTA DE LIBERACION',
                        render: (data, type, row, meta) => {
                            return `<button class='btn btn-xs btn-warning imprimirLiberacion' title='Imprimir.'><i class='fas fa-print'></i></button>&nbsp;`
                        }
                    },
                    {
                        data: 'propiedad', title: 'CONSTANCIA FONACOT',
                        render: (data, type, row, meta) => {
                            return `<button class='btn btn-xs btn-warning imprimirFonacot' title='Imprimir.'><i class='fas fa-print'></i></button>&nbsp;`
                        }
                    },
                    {
                        data: 'propiedad', title: 'CONSTANCIA GUARDERIA',
                        render: (data, type, row, meta) => {
                            return `<button class='btn btn-xs btn-warning imprimirGuarderia' title='Imprimir.'><i class='fas fa-print'></i></button>&nbsp;`
                        }
                    },
                    {
                        data: 'propiedad', title: 'PERMISO DE HORARIO',
                        render: (data, type, row, meta) => {
                            return `<button class='btn btn-xs btn-warning imprimirLactancia' title='Imprimir.'><i class='fas fa-print'></i></button>&nbsp;`
                        }
                    },

                ],
                initComplete: function (settings, json) {
                    tblConstancias.on('click', '.imprimirLaboral', function () {
                        let rowData = dtConstancias.row($(this).closest('tr')).data();
                        fncGetDatosEmpleadosLaboral(rowData.clave_empleado);
                        btnLaboral.data("clave_empleado", rowData.clave_empleado);
                        btnLaboral.data("id_reporte", 2);

                        ModalLaboral.modal("show");
                    });
                    tblConstancias.on('click', '.imprimirLiberacion', function () {
                        let rowData = dtConstancias.row($(this).closest('tr')).data();
                        if (rowData.contratable == "N") {
                            Alert2Error('No se puede imprimir el formato, El usuario no es contratable.');
                            return;

                        }
                        getReporteLiberacion(rowData.clave_empleado, 4);
                        btnLaboral.data("clave_empleado", rowData.clave_empleado);
                    });

                    tblConstancias.on('click', '.imprimirFonacot', function () {
                        let rowData = dtConstancias.row($(this).closest('tr')).data();
                        fncGetDatosEmpleadosFonacot(rowData.clave_empleado, 0);
                        btnLaboral.data("clave_empleado", rowData.clave_empleado);
                    });
                    tblConstancias.on('click', '.imprimirGuarderia', function () {
                        let rowData = dtConstancias.row($(this).closest('tr')).data();
                        fncGetDatosHijosGuarderia(rowData.clave_empleado);
                        fncGetDatosEmpleadosGuarderia(rowData.clave_empleado);
                        chkDescanso.bootstrapToggle('on');
                        horaEntradaD.val("");
                        horaSalidaD.val("");

                        ModalGuarderia.modal("show");
                        btnLaboral.data("clave_empleado", rowData.clave_empleado);
                        btnLaboral.data("id_reporte", 1);
                    });
                    tblConstancias.on('click', '.imprimirLactancia', function () {
                        let rowData = dtConstancias.row($(this).closest('tr')).data();
                        fncGetDatosEmpleadosLactancia(rowData.clave_empleado, 3);
                        ModalLactancia.modal("show");
                        btnLaboral.data("clave_empleado", rowData.clave_empleado);
                        btnLaboral.data("id_reporte", 3);
                    });

                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }
        function fncGetConsultaCC() {
            axios.post('GetConsultaCC', { cc: getValoresMultiples("#cboCC"), estatus }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtConstancias.clear();
                    dtConstancias.rows.add(items);
                    dtConstancias.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }
        //region GetReportes

        function getReporteLiberacion(idEmpleado, idFormato) {

            // $.blockUI({ message: mensajes.PROCESANDO });
            let obj = {
                cveEmpleado: idEmpleado,
                idReporte: idFormato,
            }

            axios.post("GuardarExpediciones", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    var idReporte = idReporte;

                    var path = "/Reportes/Vista.aspx?idReporte=267&idEmpleado=" + idEmpleado + `&inMemory=1&idExp=${items.id}`;
                    ireport = $("#report");
                    ireport.attr("src", path);

                    document.getElementById('report').onload = function () {

                        $.unblockUI();
                        openCRModal();


                    };
                }
            }).catch(error => Alert2Error(error.message));


        }
        //endregion

        function fncGetDatosEmpleadosLactancia(clave_empleado) {
            axios.post("GetSolicitudLactancia", { clave_empleado }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    var fechanca = null;
                    inputNombreLact.val(items.nombreCompletoLact);
                    inputPuestoLact.val(items.nombrePuestoLact);
                    inputSexo.val(items.sexo);
                    if (items.fechaInicioInca == fechanca) {
                        $('#inputFechaInicioInca').val("N/A");
                        $('#inputFechaFinInca').val("N/A");

                    } else {
                        $('#inputFechaInicioInca').val(moment(items.fechaInicioInca).format("DD/MM/YYYY"));
                        $('#inputFechaFinInca').val(moment(items.fechaFinInca).format("DD/MM/YYYY"));

                        let fechaPermiso = moment(items.fechaFinInca).add(1, "d");
                        inputFechaInicioPermiso.val(fechaPermiso.format("DD/MM/YYYY"));
                        inputFechaFinPermiso.val(fechaPermiso.add(6, "M").format("DD/MM/YYYY"));

                    }
                }
            }).catch(error => Alert2Error(error.message));

        }
        function fncGetDatosEmpleadosFonacot(clave_empleado, idFormato) {

            axios.post("GetSolicitudFonacot", { clave_empleado }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    nombreCompleto.val(items.nombreCompleto);
                    nombrePuesto.val(items.nombrePuesto);
                    ccDescripcionFonacot.val(items.ccDescripcionFonacot)
                    regPatron.val(items.regPatron);
                    nombrePatron.val(items.nombrePatron);
                    imss.val(items.imss);
                    rfc.val(items.rfc);
                    curp.val(items.curp);
                    sueldoBase.val(items.sueldoBase);
                    complemento.val(items.complemento);
                    tipoNomina.val(items.tipoNomina);
                    if (items.tipoNomina == "SEMANAL") {
                        mensualNeto.val(((Number(items.sueldoBase) + Number(items.complemento)) / 7) * 30.4);
                    } else {
                        mensualNeto.val((Number(items.sueldoBase) + Number(items.complemento)) * 2);
                    }
                    fechaIngreso.val(items.fechaIngreso);

                }
                fncGetReporteFonacot(clave_empleado, idFormato);
            }).catch(error => Alert2Error(error.message));




        }
        function fncGetReporteFonacot(clave_empleado, idFormato) {
            var fonacot = {
                nombreCompleto: $('#nombreCompleto').val(),
                nombrePuesto: $('#nombrePuesto').val(),
                ccDescripcionFonacot: $('#ccDescripcionFonacot').val(),
                regPatron: $('#regPatron').val(),
                nombrePatron: $('#nombrePatron').val(),
                imss: $('#imss').val(),
                rfc: $('#rfc').val(),
                curp: $('#curp').val(),
                sueldoBase: $('#sueldoBase').val(),
                complemento: $('#complemento').val(),
                tipoNomina: $('#tipoNomina').val(),
                fechaIngreso: $('#fechaIngreso').val(),
                mensualNeto: $('#mensualNeto').val(),
                valorLetra: numeroALetras(+mensualNeto.val(), { plural: "PESOS", singular: "PESO", centPlural: "CENTAVOS", centSingular: "CENTAVO" }),

            }

            let obj = {
                cveEmpleado: clave_empleado,
                idReporte: idFormato,
            }


            axios.post("GuardarExpediciones", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    axios.post('SetInformacionFonacot', { fonacot })
                        .then(response => {
                            let { success, datos, message } = response.data;

                            if (success) {
                                report.attr("src", `/Reportes/Vista.aspx?idReporte=${270}&idEmpleado=${clave_empleado}&inMemory=1&idExp=${items.id}`);
                                report.on('load', function () {
                                    $.unblockUI();
                                    openCRModal();
                                });
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));
                }
            }).catch(error => Alert2Error(error.message));

            // axios.post('SetInformacionFonacot', { fonacot })
            //     .then(response => {
            //         let { success, datos, message } = response.data;

            //         if (success) {
            //             report.attr("src", `/Reportes/Vista.aspx?idReporte=${270}&idEmpleado=${tblConstancias.data().clave_empleado}`);
            //             report.on('load', function () {
            //                 $.unblockUI();
            //                 openCRModal();
            //             });
            //         } else {
            //             AlertaGeneral(`Alerta`, message);
            //         }
            //     }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        function fncGetDatosEmpleadosGuarderia(clave_empleado) {
            axios.post("GetSolicitudGuarderia", { clave_empleado }).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    nombreCompletoGuard.val(items.nombreCompletoGuard),
                        nombrePuestoGuard.val(items.nombrePuestoGuard),
                        regPatron.val(items.regPatron),
                        nombrePatron.val(items.nombrePatron),
                        imss.val(items.imss),
                        rfc.val(items.rfc),
                        curp.val(items.curp),
                        fechaIngreso.val(items.fechaIngreso),
                        descripcionCCGuard.val(items.descripcionCCGuard)
                }
            }).catch(error => Alert2Error(error.message));

        }
        //region GetComboHijos
        function fncGetDatosHijosGuarderia(clave_empleado) {
            axios.post("GetHijos", { clave_empleado }).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    let datos
                    for (var i = 0; i < items.length; i++) {
                        const hijos = items[i];
                        datos += "<option value='" + hijos.nombreCompleto + "'>" + hijos.nombreCompleto + "</option>";
                    }
                    $('#nombreHijo').append(datos);

                }
            }).catch(error => Alert2Error(error.message));

        }
        function fncGetDatosEmpleadosLaboral(clave_empleado) {
            axios.post("GetSolicitudLaboral", { clave_empleado }).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    nombreCompletoLab.val(items.nombreCompletoLab),
                        nombrePuestoLab.val(items.nombrePuestoLab),
                        numeroPatronal.val(items.numeroPatronal),
                        nombreRegPatronal.val(items.nombreRegPatronal),
                        proyectoCCLab.val(items.proyectoCCLab),
                        numeroSeguroLab.val(items.numeroSeguroLab),
                        rfcLab.val(items.rfcLab),
                        curpLab.val(items.curpLab),
                        fechaAltaLab.val(items.fechaAltaLab),
                        fechaBajaLab.val(items.fechaBajaLab),
                        tipoNominaLab.val(items.tipoNominaLab),
                        sueldoBaseLab.val(items.sueldoBaseLab),
                        complementoLab.val(items.complementoLab),
                        contratableLab.val(items.contratable)



                    if (items.tipoNominaLab == "SEMANAL") {
                        mensualNetoLab.val(((Number(items.sueldoBaseLab) + Number(items.complementoLab)) / 7) * 30.4);
                        baseNvalBaseNetoeto = (Number(items.sueldoBaseLab) / 7) * 30.4;
                        valBaseNeto = (Number(items.sueldoBaseLab) / 7) * 30.4;
                    } else {
                        mensualNetoLab.val((Number(items.sueldoBaseLab) + Number(items.complementoLab)) * 2);
                        valBaseNeto = (Number(items.sueldoBaseLab) * 2);

                    }
                }
            }).catch(error => Alert2Error(error.message));

        }

        function limpiarModal() {
            $("input[type='text']").val("");


            $('#inputTituloLaboral').val(""),
                $('#inputNombreLact').val(""),
                $('#inputPuestoLact').val(""),
                $('#inputFechaInicioInca').val(""),
                $('#inputFechaFinInca').val(""),
                $('#inputFechaInicioPermiso').val(""),
                $('#inputFechaFinPermiso').val("")
        }
        $(function () {
            $(".validar").keydown(function (event) {
                //alert(event.keyCode);
                if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && event.keyCode !== 190 && event.keyCode !== 110 && event.keyCode !== 8 && event.keyCode !== 9) {
                    return false;
                }
            });
        });
    }
    $(document).ready(() => {
        recursoshumanos.reportesrh.Constancias = new Constancias();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();