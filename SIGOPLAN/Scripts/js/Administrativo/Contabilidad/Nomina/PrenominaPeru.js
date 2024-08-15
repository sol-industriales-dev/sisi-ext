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
        let _DISPLAY_NONE = "NONE";

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

            cboTipoNomina.select2();

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
                tipoPeriodo = dataPeriodo[2];
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

            botonCorreoDespacho.click(function () {
                Alert2AccionConfirmar('Recibo', '¿Desea enviar la boleta de nomina a los colaboradores?', 'Confirmar', 'Cancelar', () => fncGenerarReciboNomina(0));
            });

            cboTipoNomina.change(function (e) {
                comboNomina.fillComboGroup(GetCbotPeriodoNomina, { tipoNomina: cboTipoNomina.val() }, false, undefined, function () {
                    comboNomina.change();
                });
            });
        }

        function fncEnviarCorreo() {
            axios.post('EnviarCorreo', parametros).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    // dt.clear();
                    // dt.rows.add(response.data.lst);
                    // dt.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
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
                fixedColumns: { leftColumns: 5 },
                drawCallback: function (settings) {
                    let tabIndex = 1;
                    tablaPrenomina.find('input.captura').blur(function (e) {
                        var fila = ($(this).closest("tr")[0].rowIndex) - 1;
                        var columna = $(this).closest("td")[0].cellIndex;

                        switch (columna) {
                            case 5: dtTablaPrenomina.row(fila).data().horas_extra_60 = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 6: dtTablaPrenomina.row(fila).data().horas_extra_100 = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 7: dtTablaPrenomina.row(fila).data().horas_nocturnas = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 8: dtTablaPrenomina.row(fila).data().descuento_medico = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 9: dtTablaPrenomina.row(fila).data().feriados = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 10: dtTablaPrenomina.row(fila).data().subsidios = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;

                            case 11: dtTablaPrenomina.row(fila).data().buc = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 12: dtTablaPrenomina.row(fila).data().bono_altitud = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 13: dtTablaPrenomina.row(fila).data().indemnizacion = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 14: dtTablaPrenomina.row(fila).data().dominical = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;

                            case 15: dtTablaPrenomina.row(fila).data().bonificacion_extraordinaria = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 16: dtTablaPrenomina.row(fila).data().bonificacion_alta_especial = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 17: dtTablaPrenomina.row(fila).data().vacaciones_truncas = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 18: dtTablaPrenomina.row(fila).data().asignacion_escolar = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 19: dtTablaPrenomina.row(fila).data().bono_por_altura = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 20: dtTablaPrenomina.row(fila).data().devolucion_5ta = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 21: dtTablaPrenomina.row(fila).data().gratificacion_proporcional = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 22: dtTablaPrenomina.row(fila).data().adelanto_quincena = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 23: dtTablaPrenomina.row(fila).data().adelanto_gratificacion_semestre = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 24: dtTablaPrenomina.row(fila).data().bono_transporte = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 25: dtTablaPrenomina.row(fila).data().totalIngresos = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 26: dtTablaPrenomina.row(fila).data().AFP_obligatoria = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 27: dtTablaPrenomina.row(fila).data().AFP_voluntaria = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 28: dtTablaPrenomina.row(fila).data().AFP_comision = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 29: dtTablaPrenomina.row(fila).data().AFP_prima = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 30: dtTablaPrenomina.row(fila).data().conafovicer = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 31: dtTablaPrenomina.row(fila).data().essalud_vida = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 32: dtTablaPrenomina.row(fila).data().onp = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 33: dtTablaPrenomina.row(fila).data().renta_5ta = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 34: dtTablaPrenomina.row(fila).data().essalud_aportes = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 35: dtTablaPrenomina.row(fila).data().AFP_aportes = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
                            case 36: dtTablaPrenomina.row(fila).data().totalEgresos = Number($(this).val().replace(/[^0-9.-]+/g, "")); break;
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
                    tablaPrenomina.find('button.reporte').click(function (e) {
                        let _prenominaID = $(this).attr("data-prenominaID");
                        let _clave_empleado = $(this).attr("data-clave_empleado");
                        Alert2AccionConfirmar('Recibo', '¿Desea enviar la boleta de nomina a los colaboradores?', 'Confirmar', 'Cancelar', () => fncGenerarReciboNomina(_clave_empleado));
                    });
                },
                columns:
                    [
                        { data: 'id', title: 'id', visible: false },
                        { data: 'prenominaID', title: 'nominaID', visible: false },
                        {
                            data: 'clave_empleado',
                            title: 'Clave<br>Empl.',
                            render: function (data, type, row) {
                                return '<button class="btn btn-primary reporte" data-prenominaID="' + row.prenominaID + '" data-clave_empleado="' + row.clave_empleado + '">' + data + '</button>'
                            }
                        },
                        { data: 'nombre_empleado', title: 'Nombre Empleado' },
                        { data: 'puesto', title: 'Puesto' },
                        {
                            data: 'basico',
                            title: 'Básico',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' basico captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' disabled>' },
                        },
                        // PERCEPCIONES
                        {
                            data: 'jornada_semanal',
                            title: 'Jornada<br>Semanal',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' jornada_semanal captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' disabled>' },
                        },
                        {
                            data: 'horas_extra_60',
                            title: 'Horas<br>Extra<br>60%',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' horas_extra_60 captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'horas_extra_100',
                            title: 'Horas<br>Extra<br>100%',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' horas_extra_100 captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'horas_nocturnas',
                            title: 'Horas<br>Noct.',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' horas_nocturnas captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'descuento_medico',
                            title: 'Descuento<br>Médico',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' descuento_medico captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'feriados',
                            title: 'Feriados',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' feriados captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'subsidios',
                            title: 'Subsidios',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' subsidios captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'buc',
                            title: 'BUC',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' buc captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'bono_altitud',
                            title: 'Bono<br>Altitud',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' bono_altitud captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'indemnizacion',
                            title: 'Indemnización',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' indemnizacion captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'dominical',
                            title: 'Dominical',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' dominical captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'bonificacion_extraordinaria',
                            title: 'Bonifica.<br>Extra.',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' bonificacion_extraordinaria captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'bonificacion_alta_especial',
                            title: 'Bonifica.<br>Alta<br>Especial',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' bonificacion_alta_especial captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'vacaciones_truncas',
                            title: 'Vacaciones<br>Truncas',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' vacaciones_truncas captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'asignacion_escolar',
                            title: 'Asignación<br>Escolar',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' asignacion_escolar captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'bono_por_altura',
                            title: 'Bono<br>por<br>Altura',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' bono_por_altura captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'devolucion_5ta',
                            title: 'Devolución<br>5ta',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' devolucion_5ta captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'gratificacion_proporcional',
                            title: 'Gratificación<br>Proporcional',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' gratificacion_proporcional captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'adelanto_quincena',
                            title: 'Adelanto<br>Quincena',
                            visible: +cboTipoNomina.val() != 27 ? true : false,
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' adelanto_quincena captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'adelanto_gratificacion_semestre',
                            title: 'Adelanto<br>Gratificacion<br>Semestre',
                            visible: +cboTipoNomina.val() != 27 ? true : false,
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' adelanto_gratificacion_semestre captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'bono_transporte',
                            title: 'Bono<br>Transporte',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' bono_transporte captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'totalIngresos',
                            title: 'Total<br>ingresos',
                            render: function (data, type, row) { return '<input disabled="disabled" class="' + (data == 0 ? '' : 'aplica') + ' totalIngresos captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        // END: PERCEPCIONES
                        // DEDUCCIONES
                        {
                            data: 'AFP_obligatoria',
                            title: 'AFP<br>Obligatoria',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' AFP_obligatoria captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'AFP_voluntaria',
                            title: 'AFP<br>Voluntaria',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' AFP_voluntaria captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'AFP_comision',
                            title: 'AFP<br>Comisión',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' AFP_comision captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'AFP_prima',
                            title: 'AFP<br>Prima',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' AFP_prima captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'conafovicer',
                            title: 'Conafovicer',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' conafovicer captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'essalud_vida',
                            title: 'esSalud<br>Vida',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' essalud_vida captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'onp',
                            title: 'ONP',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' onp captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'renta_5ta',
                            title: 'Renta<br>5ta',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' renta_5ta captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'essalud_aportes',
                            title: 'esSalud<br>Aportes',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' essalud_aportes captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'AFP_aportes',
                            title: 'AFP<br>Aportes',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' AFP_aportes captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        {
                            data: 'totalEgresos',
                            title: 'Total<br>egresos',
                            render: function (data, type, row) { return '<input disabled="disabled" class="' + (data == 0 ? '' : 'aplica') + ' totalEgresos captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' ' + (validada ? 'disabled' : '') + '>' },
                        },
                        // END: DEDUCCIONES
                        {
                            data: 'total_pagar',
                            title: 'Total<br>Pagar',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' total_pagar captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' disabled>' },
                        },
                        {
                            data: 'total_deposito',
                            title: 'Total<br>Depósito',
                            render: function (data, type, row) { return '<input  class="' + (data == 0 ? '' : 'aplica') + ' total_deposito captura moneda text-right" value=' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' disabled>' },
                        }
                    ]
                ,
                columnDefs: [
                    { className: "first-col", targets: [2] },
                    { className: "second-col", targets: [3] },
                ]
            });

        }

        function fncGenerarReciboNomina(clave_empleado) {
            let objParamsDTO = fncGetObjGenerarReciboNomina(clave_empleado);
            axios.post('GenerarReciboNomina', objParamsDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito(message);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetObjGenerarReciboNomina(clave_empleado) {
            let objParamsDTO = {};
            objParamsDTO.prenominaID = botonReporte.attr("prenominaID");
            objParamsDTO.clave_empleado = clave_empleado;
            objParamsDTO.tipoNomina = cboTipoNomina.val();
            objParamsDTO.cc = comboCC.val();
            objParamsDTO.periodo = comboNomina.val();
            return objParamsDTO;
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

                if (+cboTipoNomina.val() != 27) {
                    tablaPrenomina.DataTable().column(24).visible(true);
                    tablaPrenomina.DataTable().column(25).visible(true);
                } else {
                    tablaPrenomina.DataTable().column(24).visible(false);
                    tablaPrenomina.DataTable().column(25).visible(false);
                }

                const dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
                tipoPeriodo = dataPeriodo[2];
                periodo = comboNomina.val();
                anio = dataPeriodo[3];
                cc = comboCC.val()
                $.blockUI({ message: 'Procesando...' });
                $.get('/Nomina/CargarPrenominaPeru', { CC: cc, periodo: periodo, tipoNomina: tipoPeriodo, anio: anio })
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
            return $.post('/Nomina/GuardarPrenominaPeru', { prenominaID: prenominaID, detalles: detalles, autorizantes: autorizantes, tipoNomina: tipoPeriodo, periodo: periodo, anio: anio, CC: cc })
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
            return $.post('/Nomina/ValidarPrenominaPeru', { prenominaID: prenominaID, detalles: detalles, autorizantes: autorizantes })
                .then(
                    function (response) {
                        if (response.success) {
                            CargarPrenomina();
                            Alert2Exito("Se ha validado correctamente la prenomina");
                            botonReporte.css("display", "none");
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
                            botonReporte.css("display", "none");
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