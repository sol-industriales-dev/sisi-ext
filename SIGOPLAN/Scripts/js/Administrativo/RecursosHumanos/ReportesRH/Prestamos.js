(() => {
    $.namespace('recursoshumanos.reportesrh.Prestamos');

    Prestamos = function () {


        const idEmpleado = $('#idEmpleado');
        const tipoDePrestamo = $('#tipoDePrestamo');
        const statusPrestamo = $('#statusPrestamo');
        const ccEmpleado = $('#ccEmpleado');


        //#region CONST CREAR ARCHIVOS ADJUNTOS
        const mdlArchivos = $("#mdlArchivos")
        const txtArchivoAdjunto = $("#txtArchivoAdjunto")
        const tablaArchivosAdjuntos = $("#tablaArchivosAdjuntos")
        const btnGuardarArchivoAdjunto = $("#btnGuardarArchivoAdjunto")
        //#endregion

        //#region CONSTS
        const idCapitalHumano = $('#idCapitalHumano')
        const divAutorizantes = $('#divAutorizantes')
        const cboCC = $('#cboCC');
        const cboCC2 = $('#cboCC2');
        const cboCC3 = $('#cboCC3');
        const tblPrestamos = $('#tblPrestamos');
        const tblPrestamosGestion = $('#tblPrestamosGestion');
        const tblConstancias = $('#tblConstancias');
        const tblConfiguracionPrestamos = $('#tblConfiguracionPrestamos');
        const btnBuscar = $('#btnBuscar');
        const CC = $('#CC');
        const puesto = $('#puesto');
        const inputClaveEmp = $('#inputClaveEmp');
        const inputPuesto = $('#inputPuesto');
        const inputCC = $('#inputCC');
        const inputTrabajador = $('#inputTrabajador');
        const inputFechaIngreso = $('#inputFechaIngreso');
        const inputNomina = $('#inputNomina');
        const inputSueldoBase = $('#inputSueldoBase');
        const inputComplemento = $('#inputComplemento');
        const inputTotalN = $('#inputTotalN');
        const inputTotalM = $('#inputTotalM');
        const inputOtrosDesc = $('#inputOtrosDesc');
        const inputCantMax = $('#inputCantMax');
        const inputCantSoli = $('#inputCantSoli');
        const inputCantDescontar = $('#inputCantDescontar');
        const inputEmpresa = $('#inputEmpresa');
        const tipoPago = $('#tipoPago');
        const tipoPrestamo = $('#tipoPrestamo');
        const tipoPuesto = $('#tipoPuesto');
        const tipoSolicitud = $('#tipoSolicitud');
        const formaPago = $('#formaPago');
        const MotivoPres = $('#MotivoPres');
        const inputJustificacion = $('#inputJustificacion');
        const ModalPrestamo = $('#ModalPrestamo');
        const ModalConsultaPrestamo = $('#ModalConsultaPrestamo');
        const btnPrestamo = $("#btnPrestamo");
        const btnBuscarGestion = $("#btnBuscarGestion");
        const btnBuscarPrestamo = $("#btnBuscarPrestamo");
        const filtroTipoPrestamo = $("#filtroTipoPrestamo");
        const filtroStatus = $("#filtroStatus");
        const filtroMonto = $("#filtroMonto");
        const filtroFechaFin = $("#filtroFechaFin");
        const filtroFechaInicio = $("#filtroFechaInicio");
        const btnAutorizar = $('#btnAutorizar');
        const btnRechazar = $('#btnRechazar');
        const btnGuardar = $('#btnGuardar');
        const btnGuardarConfiguracion = $('#btnGuardarConfiguracion');
        const inputBono = $('#inputBono');
        const btnSubirEvidencia = $('#btnSubirEvidencia');
        const mdlComentario = $('#mdlComentario')
        const txtComentario = $('#txtComentario')
        const inputFolio = $('#inputFolio');
        const inputFechaCaptura = $('#inputFechaCaptura');

        const _ESTATUS = {
            AUTORIZADA: 'A',
            PENDIENTE: 'P',
            CANCELADA: 'C'
        }
        const _ESTATUSCONF = {
            ACTIVO: 'A',
            DESACTIVADO: 'D'

        }

        const idResponsableCC = $('#idResponsableCC');
        const idDirectorLineaN = $('#idDirectorLineaN');
        const idGerenteOdirector = $('#idGerenteOdirector');
        const idDirectorGeneral = $('#idDirectorGeneral');
        const divDirectorGeneral = $('#divDirectorGeneral')

        let dtPrestamos;
        let dtPrestamosGestion;
        let dtConstancias;
        let dtConfiguracionPrestamos;
        //#endregion

        //#region CONST LISTADO AUTORIZANTES
        const mdlListadoAutorizantes = $('#mdlListadoAutorizantes')
        const tblListadoAutorizantes = $('#tblListadoAutorizantes')
        let dtListadoAutorizantes;
        //#endregion

        (function init() {
            fncListeners();

            // $(".select2").select2()
        })();

        $('#filtroFechaInicio').datepicker({
            format: 'DD/MM/YYYY',
        });

        $('#filtroFechaFin').datepicker({
            format: 'DD/MM/YYYY',
        });

        $('#FechaInicioPeriodo').datepicker({
            format: 'DD/MM/YYYY',
        });

        $('#FechaFinPeriodo').datepicker({
            format: 'DD/MM/YYYY',
        });

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            if (variables.cargarModal == 1) {
                fncGetPrestamosFiltroGestion(ccEmpleado.val(), tipoDePrestamo.val(), statusPrestamo.val());
                var clean_uri = location.protocol + "//" + location.host + location.pathname;
                window.history.replaceState({}, document.title, clean_uri);
            }
        }

        function getUrlParams(url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            return params;
        }

        function fncListeners() {
            initTblEmpleados();
            initTblPrestamos();
            initTblPrestamosGestion();
            initTblConfiguracion();
            initTblListadoAutorizantes()
            inittablaArchivosAdjuntos()
            GetConfiguracionPrestamos();
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");
            cboCC2.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC2");
            cboCC3.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC3");
            obtenerUrlPArams();
            divAutorizantes.css("display", "none")
            tipoPrestamo.change(function () {
                // idResponsableCC.fillCombo('/Administrativo/ReportesRH/FillComboAutorizantesPrestamos', { clave_empleado: $(this).data().clave_empleado, tipoPrestamo: tipoPrestamo.val() }, false, null);
                // idDirectorLineaN.fillCombo('/Administrativo/ReportesRH/FillComboAutorizantesPrestamos', { clave_empleado: $(this).data().clave_empleado, tipoPrestamo: tipoPrestamo.val() }, false, null);
                // idGerenteOdirector.fillCombo('/Administrativo/ReportesRH/FillComboAutorizantesPrestamos', { clave_empleado: $(this).data().clave_empleado, tipoPrestamo: tipoPrestamo.val() }, false, null);
                // idDirectorGeneral.fillCombo('/Administrativo/ReportesRH/FillComboAutorizantesPrestamos', { clave_empleado: $(this).data().clave_empleado, tipoPrestamo: tipoPrestamo.val() }, false, null);
                //idCapitalHumano.fillCombo('/Administrativo/ReportesRH/FillCboCapitalHumano', null, false, null);

                fncGetFacultamientos();

                switch ($(this).val()) {
                    case "SINDICATO":
                        divDirectorGeneral.css("display", "none")
                        divAutorizantes.css("display", "inline")
                        break;
                    case "MayorIgualA10":
                        divDirectorGeneral.css("display", "inline")
                        divAutorizantes.css("display", "inline")
                        break;
                    case "MenorA10":
                        divDirectorGeneral.css("display", "none")
                        divAutorizantes.css("display", "inline")
                        break;
                    default:
                        divAutorizantes.css("display", "none")
                        break;
                }
            })

            btnBuscar.click(function () {
                GetEmpleadosPrestamos();
                fncValidarRangoFechas();
            });

            btnBuscarGestion.click(function () {
                fncGetPrestamosFiltroGestion();
            });

            btnBuscarPrestamo.click(function () {
                // if ($('#filtroFechaInicio').val() == "") {
                //     Alert2Error('Campo de Fecha de inicio requerido');
                //     return;
                // }
                // if ($('#filtroFechaFin').val() == "") {
                //     Alert2Error('Campo de Fecha de final requerido');
                //     return;
                // }
                fncGetPrestamosFiltro();
            });

            btnGuardarArchivoAdjunto.click(function () {
                fncGuardarArchivoAdjunto()
            })
            btnSubirEvidencia.click(function () {
                $("#mdlArchivosCaptura").modal('show');
            })

            btnPrestamo.click(function () {
                //Validation/         
                if ($('#inputCantSoli').val() == "") {
                    Alert2Error('Cantidad a solicitar Requerido');
                    return;
                }

                if ($('#inputJustificacion').val() == "") {
                    Alert2Error('Justificacion Requerida');
                    return;
                }

                if ($('#MotivoPres').val() == "") {
                    Alert2Error('Motivo del prestamo requerido');
                    return;
                }

                if ($('#tipoSolicitud').val() == "") {
                    Alert2Error('Tipo de solicitud de prestamo requerida');
                    return;
                }

                if ($("#tipoPago").val() == "") {
                    Alert2Error('Plazo de pago necesaria');
                    return;
                }

                if (tipoPuesto.val() == "NO SINDICALIZADO" && MotivoPres.val() == "5") {
                    Alert2Warning("El empleado no es sindicalizado, favor de elegir otro motivo de prestamo.");
                    return "";
                }

                // if (tipoPuesto.val() == "SINDICALIZADO" && MotivoPres.val() == "1" && MotivoPres.val() == "2" && MotivoPres.val() == "3" && MotivoPres.val() == "4") {
                //     Alert2Warning("El empleado es sindicalizado, favor de elegir otro motivo de prestamo.");
                //     return "";
                // }

                var prestamo = {
                    cc: $('#CC').val(),
                    clave_empleado: $('#inputClaveEmp').val(),
                    puesto: $('#puesto').val(),
                    fecha_alta: $('#inputFechaIngreso').val(),
                    tipoNomina: $('#inputNomina').val(),
                    sueldo_base: unmaskNumero($('#inputSueldoBase').val()),
                    complemento: unmaskNumero($('#inputComplemento').val()),
                    totalN: unmaskNumero($('#inputTotalN').val()),
                    totalM: unmaskNumero($('#inputTotalM').val()),
                    otrosDescuento: unmaskNumero($('#inputOtrosDesc').val()),
                    cantidadMax: unmaskNumero($('#inputCantMax').val()),
                    cantidadSoli: unmaskNumero($('#inputCantSoli').val()),
                    cantidadDescontar: unmaskNumero($('#inputCantDescontar').val()),
                    formaPago: $('#tipoPago').val(),
                    motivoPrestamo: $('#MotivoPres').val(),
                    justificacion: $('#inputJustificacion').val(),
                    cantidadLetra: numeroALetras(+unmaskNumero(inputCantSoli.val()), { plural: "PESOS", singular: "PESO", centPlural: "CENTAVOS", centSingular: "CENTAVO" }),
                    tipoPrestamo: $('#tipoPrestamo').val(),
                    tipoPuesto: $('#tipoPuesto').val(),
                    tipoSolicitud: $('#tipoSolicitud').val(),
                    empresa: $('#inputEmpresa').val(),
                    idResponsableCC: $('#idResponsableCC').val(),
                    idDirectorLineaN: $('#idDirectorLineaN').val(),
                    idGerenteOdirector: $('#idGerenteOdirector').val(),
                    idDirectorGeneral: $('#idDirectorGeneral').val(),
                    idCapitalHumano: $("#idCapitalHumano").val(),
                }

                fncGuardarEditarPrestamos(prestamo);
            });

            btnAutorizar.on('click', function () {
                autorizarRechazarPrestamo('A');
            });

            btnRechazar.on('click', function () {
                autorizarRechazarPrestamo('C');
            });

            // btnActivarPeriodo.on('click', function () {
            //     ActivarDesactivarPeriodo('A');
            // });

            // btnDesactivarPeriodo.on('click', function () {
            //     ActivarDesactivarPeriodo('D');
            // });
            btnGuardarConfiguracion.on('click', function () {
                var configuracion = {
                    descripcionFinPeriodo: $('#inputDescripcionFinPeriodo').val(),
                    fechaInicioPeriodo: $('#FechaInicioPeriodo').val(),
                    fechaFinPeriodo: $('#FechaFinPeriodo').val(),

                }

                fncGuardarConfiguracionPrestamo(configuracion);

            });

            MotivoPres.on("change", function (event, esEditar) {
                if (!esEditar) {

                    if ($(this).val() == "5") {
                        tipoSolicitud.val("CHEQUE FAUSTINO RAYGOZA TIRADO");
                        tipoSolicitud.trigger("change");
                        tipoSolicitud.prop("disabled", true);
                        $("#tipoSolicitud option[value='CHEQUE FAUSTINO RAYGOZA TIRADO'").css("display", "initial");


                        tipoPrestamo.val("MenorA10");
                        tipoPrestamo.trigger("change");
                        tipoPrestamo.prop("disabled", true);

                    } else {
                        if ($(this).val() == "1" || $(this).val() == "2" || $(this).val() == "3" || $(this).val() == "4") {
                            tipoSolicitud.val("");
                            tipoSolicitud.trigger("change");
                            $("#tipoSolicitud option[value='CHEQUE FAUSTINO RAYGOZA TIRADO'").css("display", "none");

                        }

                        tipoSolicitud.prop("disabled", false);
                        tipoPrestamo.prop("disabled", false);

                    }
                    fncGetFacultamientos();

                }

            });
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

        function fncGetFacultamientos() {
            let strTipoPrestamo = "";

            if (MotivoPres.val() == 5) {
                strTipoPrestamo = "SINDICATO";
            } else {
                strTipoPrestamo = tipoPrestamo.val();
            }

            axios.post("FillComboAutorizantesPrestamos", { clave_empleado: tipoPrestamo.data().clave_empleado, tipoPrestamo: strTipoPrestamo }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    idResponsableCC.empty();
                    idDirectorLineaN.empty();
                    idGerenteOdirector.empty();
                    idCapitalHumano.empty();
                    idDirectorGeneral.empty();

                    // idResponsableCC.fillCombo('/Administrativo/ReportesRH/FillComboAutorizantesPrestamos', { clave_empleado: $(this).data().clave_empleado, tipoPrestamo: tipoPrestamo.val() }, false, null);
                    // idDirectorLineaN.fillCombo('/Administrativo/ReportesRH/FillComboAutorizantesPrestamos', { clave_empleado: $(this).data().clave_empleado, tipoPrestamo: tipoPrestamo.val() }, false, null);
                    // idGerenteOdirector.fillCombo('/Administrativo/ReportesRH/FillComboAutorizantesPrestamos', { clave_empleado: $(this).data().clave_empleado, tipoPrestamo: tipoPrestamo.val() }, false, null);
                    // idDirectorGeneral.fillCombo('/Administrativo/ReportesRH/FillComboAutorizantesPrestamos', { clave_empleado: $(this).data().clave_empleado, tipoPrestamo: tipoPrestamo.val() }, false, null);
                    idResponsableCC.parent().css("display", "none");
                    idGerenteOdirector.parent().css("display", "none");
                    idCapitalHumano.parent().css("display", "none");
                    idDirectorLineaN.parent().css("display", "none");
                    idDirectorGeneral.parent().css("display", "none");

                    for (const item of items) {

                        //#region NUEVOS FACULTAMIENTOS
                        switch (item.Prefijo) {
                            case "454":
                            case "449":
                            case "445":
                                var newOption = new Option(item.Text, item.Value, false, false);
                                idResponsableCC.append(newOption);
                                idResponsableCC.parent().css("display", "block");

                                break;

                            case "446":
                            case "450":
                            case "455":
                                var newOption = new Option(item.Text, item.Value, false, false);
                                idCapitalHumano.append(newOption);
                                idCapitalHumano.parent().css("display", "block");

                                break;
                            case "569":
                            case "448":
                            case "544":
                            case "447":
                            case "451":
                                var newOption = new Option(item.Text, item.Value, false, false);
                                idGerenteOdirector.append(newOption);
                                idGerenteOdirector.parent().css("display", "block");

                                break;
                            case "2672":
                            case "453":
                            case "543":
                            case "593":
                                var newOption = new Option(item.Text, item.Value, false, false);
                                idDirectorLineaN.append(newOption);
                                idDirectorLineaN.parent().css("display", "block");

                                break;
                            case "592":
                                var newOption = new Option(item.Text, item.Value, false, false);
                                idDirectorGeneral.append(newOption);
                                idDirectorGeneral.parent().css("display", "block");

                                break;
                            default:
                                break;
                        }

                        //#endregion

                    }
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblPrestamos() {
            dtPrestamos = tblPrestamos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'folio', title: 'Folio',
                        render: (data, type, row, meta) => {
                            return (row.cc == null ? "" : row.cc) + "-" + (row.clave_empleado == null ? "" : row.clave_empleado) + "-" + (row.consecutivo == null ? "" : row.consecutivo.toString().padStart(3, '0'))
                        }
                    },
                    { data: 'nombreUsuarioCreacion', title: 'Capturo' },
                    {
                        data: 'fecha_creacion', title: 'Fecha Captura', render: (data, type, row, meta) => {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    { data: 'clave_empleado', title: 'Clave empleado' },
                    { data: 'nombreCompleto', title: 'Trabajador' },
                    { data: 'DescripcionPuesto', title: 'Puesto' },
                    { data: 'ccDescripcion', title: 'CC' },
                    {
                        data: 'cantidadSoli', title: 'Cantidad solicitada',
                        render: (data, type, row, meta) => {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'otrosDescuento', title: 'Descuentos',
                        render: (data, type, row, meta) => {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'montoDescontados', title: 'Cant. Descontada',
                        render: (data, type, row, meta) => {
                            return maskNumero2DCompras(data);
                        }
                    },
                    { data: 'descripcionTipoPrestamo', title: 'Tipo prestamo' },
                    {
                        data: 'estatus', title: 'Estatus', className: 'dt-center',
                        render: function (data, type, row) {
                            if (data == _ESTATUS.AUTORIZADA) {
                                return 'Autorizado';
                            }
                            if (data == _ESTATUS.PENDIENTE) {
                                return 'Pendiente';
                            }
                            if (data == _ESTATUS.CANCELADA) {
                                return 'Rechazado';
                            }
                        }
                    },
                    {
                        title: 'Opciones',
                        render: (data, type, row, meta) => {
                            let btns = "";
                            let btnPrestamo = `<button class='btn btn-xs btn-warning imprimirPrestamos' title='Imprimir prestamo.'><i class='fas fa-print'></i></button>`
                            let btnPagare = `<button class='btn btn-xs btn-warning imprimirPagare' title='Imprimir pagaré.'><i class='fas fa-print'></i></button>`
                            let btnArchivos = `<button class='btn btn-xs btn-primary archivosAdjuntos' title='Listado de archivos adjuntos.'><i class="fas fa-file-pdf"></i></button>`
                            let btnNotificarGestion = `<button class='btn btn-xs btn-primary notificarPrestamo' title='Notificar para gestión de firmas.'><i class="far fa-envelope"></i></button>`
                            let btnListadoAutorizantes = '<button class="btn btn-primary btn-xs btnListadoAutorizantes" title="Listado de autorizantes."><i class="fas fa-user-lock"></i></button>'

                            btns = `${btnPrestamo} ${btnPagare} ${btnArchivos} ${btnListadoAutorizantes}`
                            if (row.puedeAutorizarRechazar) {
                                btns += ` ${btnNotificarGestion}`
                            } else if (row.esSindicalizado) { btns += ` ${btnNotificarGestion}` }
                            return btns
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblPrestamos.on('click', '.imprimirPagare', function () {
                        let rowData = dtPrestamos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('Imprimir', '¿Desea generar el imprimible pagaré?', 'Confirmar', 'Cancelar', () => getReportePagare(rowData.clave_empleado));
                    });

                    tblPrestamos.on('click', '.imprimirPrestamos', function () {
                        let rowData = dtPrestamos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('Imprimir', '¿Desea generar el imprimible del prestamo?', 'Confirmar', 'Cancelar', () => getReportePrestamo(rowData.clave_empleado));
                    });

                    tblPrestamos.on('click', '.archivosAdjuntos', function () {
                        let rowData = dtPrestamos.row($(this).closest('tr')).data();
                        btnGuardarArchivoAdjunto.data().FK_Prestamo = rowData.id
                        fncGetArchivosAdjuntos();
                    });

                    tblPrestamos.on('click', '.notificarPrestamo', function () {
                        let rowData = dtPrestamos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('Notificar', '¿Desea notificar el prestamo para la gestión de firmas?', 'Confirmar', 'Cancelar', () => fncNotificarPrestamo(rowData.id));
                    });

                    tblPrestamos.on("click", ".btnListadoAutorizantes", function () {
                        let rowData = dtPrestamos.row($(this).closest('tr')).data();
                        fncGetListadoAutorizantes(rowData.id)
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '10%', targets: [0] },
                    { width: '5%', targets: [3] },
                    { width: '15%', targets: [4] },
                    { width: '8%', targets: [7, 8, 9] },
                    { width: '10%', targets: [10] },
                ],
            });
        }

        function fncNotificarPrestamo(FK_Prestamo) {
            if (FK_Prestamo > 0) {
                let obj = new Object()
                obj.FK_Prestamo = FK_Prestamo
                axios.post('NotificarPrestamo', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al notificar el prestamo.")
            }
        }

        function initTblPrestamosGestion() {
            dtPrestamosGestion = tblPrestamosGestion.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'folio', title: 'Folio',
                        render: (data, type, row, meta) => {
                            return (row.cc == null ? "" : row.cc) + "-" + (row.clave_empleado == null ? "" : row.clave_empleado) + "-" + (row.consecutivo == null ? "" : row.consecutivo.toString().padStart(3, '0'))
                        }
                    },
                    {
                        data: 'fecha_creacion', title: 'Fecha Creacion', render: (data, type, row, meta) => {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    { data: 'clave_empleado', title: 'Clave empleado' },
                    { data: 'nombreCompleto', title: 'Trabajador' },
                    { data: 'DescripcionPuesto', title: 'Puesto' },
                    { data: 'ccDescripcion', title: 'CC' },
                    {
                        data: 'estatus', title: 'Estatus', className: 'dt-center',
                        render: function (data, type, row) {
                            if (data == _ESTATUS.AUTORIZADA) {
                                return 'Autorizada';
                            }
                            if (data == _ESTATUS.PENDIENTE) {
                                return 'Pendiente (' + 'Faltan por cargar ' + row.cantidadArchivos + ' archivos.)';
                            }
                            if (data == _ESTATUS.CANCELADA) {
                                return 'Rechazada';
                            }
                        }
                    },
                    {
                        data: null, title: 'Opciones', className: 'dt-center',
                        render: function (data, type, row) {
                            let btns = "";
                            let btnAutorizar = '<button class="btn btn-success btnAutorizar btn-xs" title="Autorizar"><i class="fas fa-check"></i></button>';
                            let btnRechazar = '<button class="btn btn-danger btnRechazar btn-xs" title="Cancelar"><i class="fas fa-ban"></i></button>';
                            let btnConsultar = '<button class="btn btn-primary btnConsultar btn-xs" title="Consultar"><i class="fas fa-eye"></i></button>';
                            let btnEliminarPrestamo = '<button class="btn btn-danger botonEliminarPrestamo btn-xs" title="Eliminar"><i class="fas fa-trash"></i></button>';
                            let btnListadoAutorizantes = '<button class="btn btn-primary btn-xs btnListadoAutorizantes" title="Listado de autorizantes."><i class="fas fa-user-lock"></i></button>'
                            let btnArchivos = `<button class='btn btn-xs btn-primary archivosAdjuntos' title='Listado de archivos adjuntos.'><i class="fas fa-file-pdf"></i></button>`
                            let btnComentario = `<button class='btn btn-xs btn-primary verComentario' title='Ver comentario rechazo.'><i class="far fa-comments"></i></button>`

                            if (row.estatus == _ESTATUS.PENDIENTE) {
                                btns = `${btnConsultar} ${btnEliminarPrestamo} ${btnListadoAutorizantes} ${btnArchivos} `
                            } else {
                                btns = `${btnConsultar} ${btnEliminarPrestamo} ${btnListadoAutorizantes} ${btnArchivos} `

                                if (row.estatus == _ESTATUS.CANCELADA) {
                                    btns += btnComentario;
                                }
                            }

                            if (row.puedeAutorizarRechazar) {
                                btns += `${btnAutorizar} ${btnRechazar}`
                            }

                            return btns;
                            // PUEDE AUTORIZAR/RECHAZAR
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblPrestamosGestion.on('click', '.btnAutorizar', function () {
                        let rowData = dtPrestamosGestion.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('Autorizar prestamo', '¿Desea autorizar el prestamo seleccionado?', 'Confirmar', 'Cancelar', () => fncAutorizarRechazarPrestamo(rowData.id, true));
                    });

                    tblPrestamosGestion.on('click', '.btnRechazar', function () {
                        let rowData = dtPrestamosGestion.row($(this).closest('tr')).data();
                        // Alert2AccionConfirmar('Rechazar prestamo', '¿Desea rechazar el prestamo seleccionado?', 'Confirmar', 'Cancelar', () => fncAutorizarRechazarPrestamo(rowData.id, false));

                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "Rechazar reporte",
                            input: 'textarea',
                            width: '50%',
                            showCancelButton: true,
                            html: "<h3>¿Desea rechazar el reporte seleccionado?<br>Indicar el motivo:</h3>",
                            confirmButtonText: "Aceptar",
                            confirmButtonColor: "#5cb85c",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#5c636a",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                fncAutorizarRechazarPrestamo(rowData.id, false, $('.swal2-textarea').val());
                            }
                        });
                    });

                    tblPrestamosGestion.on('click', '.btnConsultar', function () {
                        let rowData = dtPrestamosGestion.row($(this).closest('tr')).data();
                        fncGetConsultaPrestamos(rowData.clave_empleado);
                        ModalConsultaPrestamo.modal("show");
                        btnAutorizar.hide();
                        btnRechazar.hide();
                        btnGuardar.hide();
                    });

                    tblPrestamosGestion.on('click', '.botonEliminarPrestamo', function () {
                        let rowData = dtPrestamosGestion.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => eliminarPrestamo(rowData.id));
                    });

                    tblPrestamosGestion.on("click", ".btnListadoAutorizantes", function () {
                        let rowData = dtPrestamosGestion.row($(this).closest('tr')).data();
                        fncGetListadoAutorizantes(rowData.id)
                    });

                    tblPrestamosGestion.on("click", ".archivosAdjuntos", function () {
                        let rowData = dtPrestamosGestion.row($(this).closest('tr')).data();
                        fncGetArchivosAdjuntosByID(rowData.id);

                    });

                    tblPrestamosGestion.on("click", ".verComentario", function () {
                        let rowData = dtPrestamosGestion.row($(this).closest('tr')).data();
                        mdlComentario.modal("show");
                        txtComentario.val(rowData.comentarioRechazo);

                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '5%', targets: [2] },
                    { width: '15%', targets: [7] },
                    { width: '10%', targets: [6] },
                ],
            });
        }

        function fncAutorizarRechazarPrestamo(FK_Prestamo, esAutorizar, comentarioRechazo) {
            if (FK_Prestamo > 0) {
                let obj = new Object();
                obj.FK_Prestamo = FK_Prestamo;
                obj.esAutorizar = esAutorizar;
                obj.comentarioRechazo = comentarioRechazo;
                axios.post('AutorizarRechazarPrestamo', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetPrestamosFiltroGestion()
                        Alert2Exito(message)
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (esAutorizar) {
                    Alert2Error("Ocurrió un error al autorizar el prestamo.")
                } else {
                    Alert2Error("Ocurrió un error al rechazar el prestamo.")
                }
            }
        }

        function fncGetListadoAutorizantes(idPrestamo) {
            if (idPrestamo > 0) {
                let obj = new Object()
                obj.idPrestamo = idPrestamo
                axios.post('GetListadoAutorizantes', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtListadoAutorizantes.clear();
                        dtListadoAutorizantes.rows.add(response.data.lstAutorizantes);
                        dtListadoAutorizantes.draw();
                        mdlListadoAutorizantes.modal("show")
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function initTblListadoAutorizantes() {
            dtListadoAutorizantes = tblListadoAutorizantes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'tipoPuesto', title: 'Autorizante' },
                    { data: 'nombreCompleto', title: 'Nombre' },
                    { data: 'estatus', title: 'Estatus prestamo' }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTblEmpleados() { // OMAR
            dtConstancias = tblConstancias.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'clave_empleado', title: 'Clave empleado' },
                    {
                        title: 'Nombre del trabajador',
                        render: (data, type, row, meta) => {
                            return "" + row.ape_paterno + " " + row.ape_materno + " " + row.nombre + "";
                        }
                    },
                    { data: 'descripcion', title: 'Puesto' },
                    {
                        data: 'propiedad', title: 'Opciones',
                        render: (data, type, row, meta) => {
                            return `<button class='btn btn-xs btn-warning solicitarPrestamo' title='Solicitar Prestamo.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblConstancias.on('click', '.solicitarPrestamo', function () {
                        let rowData = dtConstancias.row($(this).closest('tr')).data();
                        fncGetDatosEmpleadosPrestamos(rowData.clave_empleado);
                        fncValidarRangoFechas();
                        tipoPrestamo.data().clave_empleado = rowData.clave_empleado;
                        // idResponsableCC.fillCombo('/Administrativo/ReportesRH/FillComboAutorizantesPrestamos', { clave_empleado: rowData.clave_empleado, tipoPrestamo: tipoPrestamo.val() }, false, null);
                        // idDirectorLineaN.fillCombo('/Administrativo/ReportesRH/FillComboAutorizantesPrestamos', { clave_empleado: rowData.clave_empleado, tipoPrestamo: tipoPrestamo.val() }, false, null);
                        // idGerenteOdirector.fillCombo('/Administrativo/ReportesRH/FillComboAutorizantesPrestamos', { clave_empleado: rowData.clave_empleado, tipoPrestamo: tipoPrestamo.val() }, false, null);
                        // idDirectorGeneral.fillCombo('/Administrativo/ReportesRH/FillComboAutorizantesPrestamos', { clave_empleado: rowData.clave_empleado, tipoPrestamo: tipoPrestamo.val() }, false, null);
                        ModalPrestamo.modal('show');

                        MotivoPres.val("");
                        MotivoPres.trigger("change");
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '5%', targets: [0, 3] },
                    { width: '50%', targets: [1, 2] }
                ],
            });
        }
        function initTblConfiguracion() {
            dtConfiguracionPrestamos = tblConfiguracionPrestamos.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                columns: [
                    { data: 'id', title: 'ID', visible: false },
                    { data: 'descripcionFinPeriodo', title: 'Mensaje' },
                    {
                        data: 'fechaInicioPeriodo', title: 'Fecha inicio',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    {
                        data: 'fechaFinPeriodo', title: 'Fecha fin',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    {
                        data: 'estatus', title: 'Estatus',
                        render: function (data, type, row) {
                            if (data == _ESTATUSCONF.ACTIVO) {
                                return 'ACTIVO';
                            }
                            if (data == _ESTATUSCONF.DESACTIVADO) {
                                return 'DESACTIVADO'
                            }
                        },
                    },
                    {
                        data: null, title: 'Opciones', className: 'dt-center',
                        render: function (data, type, row) {
                            let btnActivarPeriodo = '<button class="btn btn-success btnActivarPeriodo btn-xs" title="Activar"><i class="fas fa-power-off"></i></button>';
                            let btnDesactivarPeriodo = '<button class="btn btn-danger btnDesactivarPeriodo btn-xs" title="Desactivar"><i class="fas fa-power-off"></i></button>';
                            let btnEliminarPeriodo = '<button class="btn btn-danger btnEliminarPeriodo btn-xs" title="Eliminar"><i class="fas fa-trash"></i></button>';

                            if (row.estatus == _ESTATUSCONF.ACTIVO) {
                                return btnDesactivarPeriodo;
                            } else {
                                return `${btnActivarPeriodo} ${btnEliminarPeriodo}`
                            }
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblConfiguracionPrestamos.on('click', '.btnActivarPeriodo ', function () {
                        var estatus = "A";
                        let rowData = dtConfiguracionPrestamos.row($(this).closest('tr')).data();
                        ActivarDesactivarPeriodo(rowData.id, estatus);


                    });
                    tblConfiguracionPrestamos.on('click', '.btnDesactivarPeriodo ', function () {
                        var estatus = "D";
                        let rowData = dtConfiguracionPrestamos.row($(this).closest('tr')).data();
                        ActivarDesactivarPeriodo(rowData.id, estatus);

                    });
                    tblConfiguracionPrestamos.on('click', '.btnEliminarPeriodo ', function () {
                        let rowData = dtConfiguracionPrestamos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => eliminarConfiguracion(rowData.id));
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
        function GetEmpleadosPrestamos() {
            axios.post('GetEmpleadosPrestamos', { cc: getValoresMultiples("#cboCC") }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtConstancias.clear();
                    dtConstancias.rows.add(items);
                    dtConstancias.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetPrestamosFiltro() {
            let obj = new Object()
            obj.lstCC = getValoresMultiples("#cboCC2")
            obj.estatus = filtroStatus.val()
            obj.tipoPrestamo = filtroTipoPrestamo.val()
            obj.cantidad = filtroMonto.val()
            obj.fechaInicio = filtroFechaInicio.val()
            obj.fechaFin = filtroFechaFin.val()
            axios.post("GetPrestamosFiltro", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtPrestamos.clear();
                    dtPrestamos.rows.add(items);
                    dtPrestamos.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetPrestamosFiltroGestion() {
            axios.post("GetPrestamosFiltroGestion", { cc3: getValoresMultiples("#cboCC3"), estatus: filtroStatus.val(), tipoPrestamo: filtroTipoPrestamo.val() }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncValidarRangoFechas();
                    dtPrestamosGestion.clear();
                    dtPrestamosGestion.rows.add(items);
                    dtPrestamosGestion.draw();

                    tblPrestamosGestion.DataTable().search(idEmpleado.val()).draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        // function fncGuardarEditarPrestamos(prestamo) {
        //     $.ajax({
        //         type: "POST",
        //         url: '/Administrativo/ReportesRH/GuardarEditarPrestamos',
        //         data: prestamo,
        //         success: function (data) {
        //             $('#ModalPrestamo').modal('hide');
        //             limpiarModal();
        //             Alert2Exito("Se ha registrado con éxito.");
        //             // alert("Se envio Notificacion correctamente");
        //             // getReportePrestamo(inputClaveEmp.val());

        //         },
        //         error: function () {
        //             // Alert2Error('Failed');

        //         }
        //     })
        //         .catch(error => Alert2Error(error.message));
        // }
        function fncGuardarEditarPrestamos(prestamo) {
            axios.post("GuardarEditarPrestamos", { data: prestamo }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    fncGuardarArchivoAdjuntoEnCaptura();
                    $('#ModalPrestamo').modal('hide');
                    limpiarModal();
                    Alert2Exito("Se ha registrado con éxito.");
                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        function GetConfiguracionPrestamos() {
            axios.post('GetConfiguracionPrestamos').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtConfiguracionPrestamos.clear();
                    dtConfiguracionPrestamos.rows.add(items);
                    dtConfiguracionPrestamos.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }
        function fncGuardarConfiguracionPrestamo(configuracionPrestamo) {
            $.ajax({
                type: "POST",
                url: '/Administrativo/ReportesRH/GuardarConfiguracionPrestamo',
                data: configuracionPrestamo,
                success: function (data) {

                    Alert2Exito("Se ha registrado con éxito.");
                    GetConfiguracionPrestamos();

                },
                error: function () {
                    Alert2Error('Failed');
                }
            })
                .catch(error => Alert2Error(error.message));
        }
        function eliminarPrestamo(prestamo_id) {
            axios.post('EliminarPrestamo', { prestamo_id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado la información.');
                        fncGetPrestamosFiltroGestion();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        function autorizarRechazarPrestamo(estatus) {

            axios.post('AutorizarRechazarPrestamo', { estatus })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        ModalConsultaPrestamo.modal('hide');
                        Alert2Exito('Se ha guardado la información.');
                        btnBuscarGestion.click();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        function ActivarDesactivarPeriodo(id, estatus) {

            axios.post('ActivarDesactivarPeriodo', { id, estatus })
                .then(response => {
                    let { success, datos, message } = response.data
                    if (success) {

                        Alert2Exito('Se ha guardado la información.');
                        GetConfiguracionPrestamos();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        function getReportePrestamo(idEmpleado) {

            // $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = idReporte;

            var path = "/Reportes/Vista.aspx?idReporte=265&idEmpleado=" + idEmpleado;
            ireport = $("#report");
            ireport.attr("src", path);

            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        function getReportePagare(idEmpleado) {

            // $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = idReporte;

            var path = "/Reportes/Vista.aspx?idReporte=268&idEmpleado=" + idEmpleado;
            ireport = $("#report");
            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();
                openCRModal();
            };
        }
        function fncGetDatosEmpleadosPrestamos(clave_empleado) {
            axios.post("GetSolicitudPrestamos", { clave_empleado }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    tipoPrestamo[0].selectedIndex = 0
                    tipoPrestamo.trigger("change")

                    inputFolio.val("");
                    inputFechaCaptura.val(moment().format("DD/MM/YYYY"));

                    CC.val(items.cc);
                    inputCC.val(items.ccDescripcion);
                    inputClaveEmp.val(items.clave_empleado);
                    inputTrabajador.val(items.nombreCompleto);
                    puesto.val(items.puesto);
                    inputPuesto.val(items.nombrePuesto);
                    inputEmpresa.val(items.empresa);
                    inputFechaIngreso.val(items.fecha_alta);
                    inputNomina.val(items.tipoNomina);
                    inputSueldoBase.val(maskNumero(items.sueldo_base));
                    inputComplemento.val(maskNumero(items.complemento));
                    tipoPrestamo.val(items.tipoPrestamo);
                    inputBono.val(maskNumero(items.bono_zona));

                    if (items.sindicato == "S") {
                        tipoPuesto.val("SINDICALIZADO");
                    } else {
                        tipoPuesto.val("NO SINDICALIZADO");
                    }

                    tipoPuesto.trigger("change")

                    tipoSolicitud.val(items.tipoSolicitud);
                    inputTotalN.val(maskNumero((Number(items.sueldo_base) + Number(items.complemento) + Number(items.bono_zona)).toFixed(2)));

                    if (items.tipoNomina == "SEMANAL") {
                        inputTotalM.val(maskNumero((((Number(items.sueldo_base) + Number(items.complemento) + Number(items.bono_zona)) / 7) * 30.4).toFixed(2)));
                        tipoPago.val("24 Semanas");

                    } else {
                        inputTotalM.val(maskNumero(((Number(items.sueldo_base) + Number(items.complemento) + Number(items.bono_zona)) * 2).toFixed(2)));
                        tipoPago.val("12 Quincenas");

                    }
                    inputCantMax.val(maskNumero((Number(unmaskNumero(inputTotalM.val())) * 1.5).toFixed(2)));
                    inputOtrosDesc.on('keyup', function () {
                        inputCantMax.val(maskNumero((Number(unmaskNumero(inputTotalM.val())) * 1.5) - Number(unmaskNumero(inputOtrosDesc.val()))));
                    });

                    inputOtrosDesc.on("change", function () {
                        inputOtrosDesc.val(maskNumero(unmaskNumero(inputOtrosDesc.val())));

                    });

                    inputCantSoli.on('change', function () {

                        if (Number($(this).val() ?? 0) > Number(unmaskNumero(inputCantMax.val()) ?? 0)) {
                            $(this).val(0);
                            Alert2Warning("Ingrese un monto a solicitar menor al maximo permitido");
                        }

                        if (unmaskNumero($(this).val()) >= 10000) {
                            tipoPrestamo.val("MayorIgualA10");
                            tipoPrestamo.trigger("change");
                        } else {
                            tipoPrestamo.val("MenorA10");
                            tipoPrestamo.trigger("change");
                        }

                        if (tipoPago.val() == "12 Quincenas") {
                            inputCantDescontar.val(maskNumero((unmaskNumero(inputCantSoli.val()) / 12).toFixed(2)))
                        }
                        else {
                            inputCantDescontar.val(maskNumero((unmaskNumero(inputCantSoli.val()) / 24).toFixed(2)))
                        }

                        $(this).val(maskNumero(unmaskNumero($(this).val())));
                    });

                    tipoPago.on('click', function () {
                        if (tipoPago.val() == "12 Quincenas") {
                            inputCantDescontar.val(maskNumero((unmaskNumero(inputCantSoli.val()) / 12).toFixed(2)))
                        }
                        else {
                            inputCantDescontar.val(maskNumero((unmaskNumero(inputCantSoli.val()) / 24).toFixed(2)))
                        }
                    });

                    // fncGetResponsableCC(responsableCC);
                    btnPrestamo.prop("disabled", false);

                } else {

                    //#region LIMPIAR MODAL
                    inputFolio.val("");
                    inputFechaCaptura.val("");
                    CC.val("");
                    inputCC.val("");
                    inputClaveEmp.val(0);
                    inputTrabajador.val("");
                    puesto.val("");
                    inputPuesto.val("");
                    inputEmpresa.val("");
                    inputFechaIngreso.val("");
                    inputNomina.val("");
                    inputSueldoBase.val(0);
                    inputComplemento.val(0);
                    tipoPrestamo.val("");
                    inputBono.val(0);

                    tipoPuesto.val("");
                    tipoPuesto.trigger("change")

                    tipoSolicitud.val("");
                    inputTotalN.val(0);

                    tipoPago.val("");
                    inputTotalM.val(0);
                    inputCantMax.val(0);


                    //#endregion

                    btnPrestamo.prop("disabled", true);
                    Alert2Warning(message)
                }
            }).catch(error => Alert2Error(error.message));
        }
        function fncGetConsultaPrestamos(clave_empleado) {
            axios.post("GetConsultaPrestamos", { clave_empleado }).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    inputFolio.val((items.cc == null ? "" : items.cc) + "-" + (items.clave_empleado == null ? "" : items.clave_empleado) + "-" + (items.consecutivo == null ? "" : items.consecutivo.toString().padStart(3, '0')));
                    inputFechaCaptura.val(moment(items.fecha_creacion).format("DD/MM/YYYY"));
                    CC.val(items.cc);
                    inputCC.val(items.ccDescripcion);
                    inputClaveEmp.val(items.clave_empleado);
                    inputTrabajador.val(items.nombreCompleto);
                    puesto.val(items.puesto);
                    inputPuesto.val(items.nombrePuesto);
                    inputEmpresa.val(items.empresa);
                    inputFechaIngreso.val(items.fecha_alta);
                    inputNomina.val(items.tipoNomina);
                    inputSueldoBase.val(maskNumero(items.sueldo_base));
                    inputComplemento.val(maskNumero(items.complemento));
                    tipoPrestamo.val(items.tipoPrestamo);
                    tipoPuesto.val(items.tipoPuesto);
                    tipoSolicitud.val(items.tipoSolicitud);
                    inputEmpresa.val(items.empresa);
                    inputTotalN.val(maskNumero(items.totalN));
                    inputTotalM.val(maskNumero(items.totalM));
                    tipoPago.val(items.tipoPago);
                    tipoPrestamo.val(items.tipoPrestamo);
                    tipoPuesto.val(items.tipoPuesto);
                    formaPago.val(items.formaPago);
                    MotivoPres.val(items.motivoPrestamo);
                    MotivoPres.trigger("change", ['esEditar']);
                    inputCantSoli.val(maskNumero(items.cantidadSoli));
                    inputJustificacion.val(items.justificacion);
                    inputOtrosDesc.val(maskNumero(items.otrosDescuento));
                    inputCantDescontar.val(maskNumero(items.cantidadDescontar.toFixed(2)));
                    inputCantMax.val(maskNumero(items.cantidadMax));
                    idResponsableCC.val(items.nombreResponsableCC);
                    idDirectorLineaN.val(items.nombreDirectorLineaN);
                    idGerenteOdirector.val(items.nombreGerenteOdirector);
                    idDirectorGeneral.val(items.nombreDirectorGeneral);
                    idCapitalHumano.val(items.nombreCapitalHumano)

                    if (items.nombreResponsableCC == null) {
                        idResponsableCC.parent().css("display", "none");
                        // idCapitalHumano.parent().css("display", "none");
                    } else {
                        idResponsableCC.parent().css("display", "initial");
                    }

                    if (items.nombreCapitalHumano == null) {
                        idCapitalHumano.parent().css("display", "none");
                        // idCapitalHumano.parent().css("display", "none");
                    } else {
                        idCapitalHumano.parent().css("display", "initial");
                    }

                    if (items.nombreDirectorLineaN == null) {
                        idDirectorLineaN.parent().css("display", "none");

                    } else {
                        idDirectorLineaN.parent().css("display", "initial");
                    }

                    if (items.nombreGerenteOdirector == null) {
                        idGerenteOdirector.parent().css("display", "none");

                    } else {
                        idGerenteOdirector.parent().css("display", "initial");
                    }

                    if (items.nombreDirectorGeneral == null) {
                        idDirectorGeneral.parent().css("display", "none");

                    } else {
                        idDirectorGeneral.parent().css("display", "initial");
                    }

                }
            }).catch(error => Alert2Error(error.message));
        }
        function eliminarConfiguracion(id) {
            axios.post('EliminarConfiguracion', { id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado la información.');
                        GetConfiguracionPrestamos();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        function fncValidarRangoFechas() {
            axios.post('GetFechasPeriodos').then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    if (items) {
                        Alert2Error(response.data.lstPeriodos[0].descripcionFinPeriodo + ' ' + moment(response.data.lstPeriodos[0].fechaInicioPeriodo).format('DD/MM/YYYY') + ' al ' + moment(response.data.lstPeriodos[0].fechaFinPeriodo).format('DD/MM/YYYY') + '.');
                        btnPrestamo.hide();
                        ModalPrestamo.modal('hide');
                    } else {
                        btnPrestamo.show();
                    }


                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));


        }
        function limpiarModal() {
            $('#inputOtrosDesc').val(""),
                $('#inputCantSoli').val(""),
                $('#inputCantLetra').val(""),
                $('#inputFechaReq').val(""),
                $('#chkMotivoPres').val(""),
                $('#inputJustificacion').val(""),
                $('#inputPendientes').val("")
        }
        //#region validacion de no letras
        $(function () {
            $(".validar").keydown(function (event) {
                //alert(event.keyCode);
                if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && event.keyCode !== 190 && event.keyCode !== 110 && event.keyCode !== 8 && event.keyCode !== 9) {
                    return false;
                }
            });
        });

        //#region CRUD ARCHIVOS
        function inittablaArchivosAdjuntos() {
            dtArchivoAdjunto = tablaArchivosAdjuntos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreArchivo', title: 'Archivo' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>&nbsp;`;
                            let btnDescargar = `<button class='btn btn-xs btn-primary visualizarArchivo' title='Visualizar archivo.'><i class="fas fa-file-download"></i></button>`;
                            return btnEliminar + btnDescargar;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tablaArchivosAdjuntos.on('click', '.eliminarRegistro', function () {
                        let rowData = dtArchivoAdjunto.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarArchivoAdjunto(rowData.id));
                    });

                    tablaArchivosAdjuntos.on('click', '.visualizarArchivo', function () {
                        let rowData = dtArchivoAdjunto.row($(this).closest('tr')).data();
                        fncVisualizarArchivoAdjunto(rowData.id)
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

        function fncVisualizarArchivoAdjunto(idArchivo) {
            if (idArchivo > 0) {
                let obj = new Object();
                obj.idArchivo = idArchivo
                axios.post("VisualizarArchivoAdjunto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        $('#myModal').data().ruta = null;
                        $('#myModal').modal('show');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncEliminarArchivoAdjunto(idArchivo) {
            if (idArchivo > 0) {
                let obj = {};
                obj.idArchivo = idArchivo;
                axios.post('EliminarArchivoAdjunto', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetArchivosAdjuntos()
                        fncGetPrestamosFiltro()
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        const fncGuardarArchivoAdjunto = function () {
            var data = fncGetEvidenciaParaGuardar();
            let obj = new Object();
            obj.FK_Prestamo = btnGuardarArchivoAdjunto.data().FK_Prestamo
            axios.post('GuardarArchivoAdjunto', data, { params: FK_Prestamo = obj }, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                let { success, datos, message } = response.data;
                if (success) {
                    Alert2Exito(message)
                    fncGetArchivosAdjuntos()
                    fncGetPrestamosFiltro()
                } else {
                    Alert2Error(message)
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        const fncGuardarArchivoAdjuntoEnCaptura = function () {
            var data = fncGetEvidenciaParaGuardarEnCaptura();
            axios.post('GuardarArchivoAdjuntoEnCaptura', data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                let { success, datos, message } = response.data;
                if (success) {
                    Alert2Exito(message)
                } else {
                    Alert2Error(message)
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function fncGetEvidenciaParaGuardar() {
            let data = new FormData();
            data.append("id", $("#rowDataId").val());
            $.each(document.getElementById("txtArchivoAdjunto").files, function (i, file) {
                data.append("lstArchivos", file);
            });
            return data;
        }
        function fncGetEvidenciaParaGuardarEnCaptura() {
            let data = new FormData();
            $.each(document.getElementById("txtArchivoAdjuntoIne").files, function (i, file) {
                data.append("lstArchivos", file);
            });
            $.each(document.getElementById("txtArchivoAdjuntoSoporte").files, function (i, file) {
                data.append("lstArchivos", file);
            });
            $.each(document.getElementById("txtArchivoAdjuntoPagare").files, function (i, file) {
                data.append("lstArchivos", file);
            });
            return data;
        }

        function fncGetArchivosAdjuntos() {
            if (btnGuardarArchivoAdjunto.data().FK_Prestamo > 0) {
                let obj = {};
                obj.FK_Prestamo = btnGuardarArchivoAdjunto.data().FK_Prestamo
                axios.post('GetArchivosAdjuntos', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtArchivoAdjunto.clear();
                        dtArchivoAdjunto.rows.add(response.data.lstArchivos);
                        dtArchivoAdjunto.draw();
                        $("#mdlArchivos").modal('show')
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetArchivosAdjuntosByID(idPrestamo) {
            btnGuardarArchivoAdjunto.data().FK_Prestamo = idPrestamo
            if (idPrestamo > 0) {
                let obj = {};
                obj.FK_Prestamo = idPrestamo
                axios.post('GetArchivosAdjuntos', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtArchivoAdjunto.clear();
                        dtArchivoAdjunto.rows.add(response.data.lstArchivos);
                        dtArchivoAdjunto.draw();
                        $("#mdlArchivos").modal('show')
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }
        //#endregion
    }
    $(document).ready(() => {
        recursoshumanos.reportesrh.Prestamos = new Prestamos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();