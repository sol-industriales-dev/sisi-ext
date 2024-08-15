function showDiv(divId, divId2, element) {
    document.getElementById(divId).style.display = element.value == "nunca" ? 'none' : 'block';
    document.getElementById(divId2).style.display = element.value == "otro" ? 'block' : 'none';
}


(function () {
    $.namespace('OTROS_SERVICIOS.Calendario');

    //#region CONST CREAR/EDITAR ADMIN SALAS
    const btnEliminar = $("#btnEliminar");
    const mdlCEAdminSalas = $("#mdlCEAdminSalas");
    const txtCE_AdminSalasAsunto = $('#asunto');
    const txtCE_AdminSalasFechaInicio = $('#fechaInicio');
    const txtCE_AdminSalasFechaFin = $('#fechaFin');
    const txtCE_AdminSalasRepeticion = $('#repeticion');
    const txtCE_AdminSalasFechaFinRepeticion = $('#fechaFinRepeticion');
    const txtCE_AdminSalasComentarios = $("#comentarios")
    const btnCEAdminSalas = $("#btnCEAdminSalas");
    const cboCE_CatEdificios = $("#cboCE_CatEdificios");
    const cboSelectEdificios = $("#selectEdificio");
    const cboCE_CatSalas = $("#cboCE_CatSalas");
    //#endregion

    var salas = new Array();
    var reuniones = new Array();

    //#region CALEDARIO
    Calendario = function () {

        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            fncFillCboCatEdificios();
            $("#selectEdificio").change(function () {
                if ($(this).val() != "") {
                    fncGetAdminSalas();
                    $("#calendar").fullCalendar("destroy");
                    fncFillSalas();
                    cboCE_CatEdificios.val($(this).val())
                    cboCE_CatEdificios.trigger("change")
                    fncFillCboCatSalas();
                }
                else {
                    $("#calendar").fullCalendar("destroy");
                    $("#externalTitle").html("Seleccione un edificio");
                }
            });

            cboCE_CatEdificios.change(function () {
                $(this).val(cboSelectEdificios.val())
            });

            $(".btn").mouseup(function () {
                $(this).blur();
            });

            //#region DATETIME PICKERS

            var checkPastTime = function (inputDateTime) {
                if (typeof (inputDateTime) != "undefined" && inputDateTime !== null) {
                    var current = new Date();

                    //check past year and month
                    if (inputDateTime.getFullYear() < current.getFullYear()) {
                        txtCE_AdminSalasFechaInicio.datetimepicker('reset');
                        txtCE_AdminSalasFechaFin.datetimepicker('reset');

                    } else if ((inputDateTime.getFullYear() == current.getFullYear()) && (inputDateTime.getMonth() < current.getMonth())) {
                        txtCE_AdminSalasFechaInicio.datetimepicker('reset');
                        txtCE_AdminSalasFechaFin.datetimepicker('reset');
                    }
                    // check input date equal to todate date
                    if (inputDateTime.getDate() == current.getDate()) {
                        if ((inputDateTime.getHours() < current.getHours()) && (inputDateTime.getMinutes() < current.getMinutes())) {
                            txtCE_AdminSalasFechaInicio.datetimepicker('reset');
                            txtCE_AdminSalasFechaFin.datetimepicker('reset');
                        }
                        this.setOptions({
                            minTime: `${txtCE_AdminSalasFechaInicio.datetimepicker("getValue").getHours()}:
                                ${txtCE_AdminSalasFechaInicio.datetimepicker("getValue").getMinutes()}` //here pass current time
                        });
                    } else {
                        this.setOptions({
                            minTime: "07:00"
                        });
                    }
                }
            };

            var currentYear = new Date();
            txtCE_AdminSalasFechaInicio.datetimepicker({
                // format: 'Y-m-d H:i',
                minDate: 0,
                yearStart: currentYear.getFullYear(),
                onChangeDateTime: checkPastTime,
                onShow: checkPastTime,
                maxTime: "19:00",
                dayOfWeekStart: 1,
                step: 5,
                // maxDate: jQuery(txtCE_AdminSalasFechaFin).val() ? jQuery(txtCE_AdminSalasFechaFin).val() : false
            });

            $.datetimepicker.setLocale('es');

            txtCE_AdminSalasFechaFin.datetimepicker(
                {
                    yearStart: currentYear.getFullYear(),
                    onChangeDateTime: checkPastTime,
                    maxTime: "19:00",
                    dayOfWeekStart: 1,
                    step: 5,
                    onShow:
                        function (ct) {

                            // checkPastTime,
                            // this.setOptions({
                            //     minDate: jQuery(txtCE_AdminSalasFechaInicio).val()
                            // });
                            this.setOptions({
                                minDate: jQuery(txtCE_AdminSalasFechaInicio).val() ? jQuery(txtCE_AdminSalasFechaInicio).val() : false
                            });
                            // this.setOptions({
                            //     checkPastTime,
                            //     minTime: `${txtCE_AdminSalasFechaInicio.datetimepicker("getValue").getHours()}:
                            //         ${txtCE_AdminSalasFechaInicio.datetimepicker("getValue").getMinutes()}`
                            // });
                            // this.setOptions({
                            //     value: jQuery(txtCE_AdminSalasFechaInicio).val()
                            // });
                        },
                }
            );

            txtCE_AdminSalasFechaFinRepeticion.datetimepicker(
                {
                    format: "Y/m/d",
                    dayOfWeekStart: 1,
                    timepicker: false,
                    onShow: function (ct) {
                        this.setOptions({ minDate: jQuery(txtCE_AdminSalasFechaInicio).val() ? jQuery(txtCE_AdminSalasFechaInicio).val() : false })
                    },
                }
            );
            //#endregion

            // #region FUNCIONES CREAR / EDITAR ADMIN SALAS
            btnCEAdminSalas.click(function () {
                fncGetAdminSalas();
                fncCEAdminSalas();
            });

            function fncCEAdminSalas() {
                let obj = fncCEOBJAdminSalas();
                if (obj != "") {
                    axios.post('CESalaJuntas', obj).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            $("#calendar").fullCalendar("destroy");
                            //#region RESET CALENDARIO
                            fncGetAdminSalas();
                            $("#btnCerrar").trigger("click");
                            $('#calendar').fullCalendar('destroy');
                            fncFillSalas();
                            $("#calendar").fullCalendar('removeEvents');
                            fncGetAdminSalas();

                            $("#calendar").fullCalendar('addEventSource', reuniones);
                            $("#calendar").fullCalendar('render');
                            $("#calendar").fullCalendar('rerenderEvents');
                            //#endregion



                            Alert2Exito(message);
                        } else {
                            Alert2Warning(message);
                        }
                    }).catch(error => Alert2Error(error.message));
                }
            }

            function fncCEOBJAdminSalas() {
                let mensajeError = ""
                // if (cboCE_CatEdificios.val() < 0) { $("#select2-cboCE_CatEdificio-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboCE_CatEdificio-container") }
                // if (cboCE_CatSalas.val() < 0) { $("#select2-cboCE_CatSalas-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboCE_CatSalas-container") }
                // if (txtCE_AdminSalasAsunto.val() == "") { txtCE_AdminSalasAsunto.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_AdminSalasAsunto") }
                // if (txtCE_AdminSalasFechaInicio.val() == "") { txtCE_AdminSalasFechaInicio.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_AdminSalasFechaInicio") }
                // if (txtCE_AdminSalasFechaFin.val() == "") { txtCE_AdminSalasFechaFin.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_AdminSalasFechaFin") }
                // if (txtCE_AdminSalasRepeticion.val() == "") { txtCE_AdminSalasRepeticion.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_AdminSalasRepeticion") }
                // if (txtCE_AdminSalasFechaFinRepeticion.val() == "") { txtCE_AdminSalasFechaFinRepeticion.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_AdminSalasFechaFinRepeticion") }


                if (mensajeError != "") {
                    Alert2Warning(mensajeError)
                    return ""
                } else {
                    let obj = new Object();
                    obj.id = btnCEAdminSalas.data().id;
                    obj.FK_Sala = cboCE_CatSalas.val();
                    obj.asunto = txtCE_AdminSalasAsunto.val();
                    obj.fechaInicio = txtCE_AdminSalasFechaInicio.val();
                    obj.fechaFin = txtCE_AdminSalasFechaFin.val();
                    obj.repeticion = txtCE_AdminSalasRepeticion.val();
                    obj.fechaFinRepeticion = txtCE_AdminSalasFechaFinRepeticion.val();
                    obj.comentarios = txtCE_AdminSalasComentarios.val();
                    obj.diasRepeticion = getDiasRepeticion();
                    return obj;
                }
            }
            // #endregion

            //#region FUNCIONES ELIMINAR ADMIN SALAS
            function fncEliminarSalaJuntas() {
                if (btnEliminar.data().id > 0) {
                    let obj = {}
                    obj.id = btnEliminar.data().id
                    axios.post('EliminarSalaJuntas', obj).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            Alert2Exito(message);
                            $("#btnCerrar").trigger("click");
                            $('#calendar').fullCalendar("removeEvents", obj.id);
                        } else {
                            Alert2Warning(message);
                        }
                    }).catch(error => Alert2Error(error.message));
                } else {
                    Alert2Warning("Ocurrió un error al eliminar el registro.")
                }
            }
            btnEliminar.click(function () {
                Alert2AccionConfirmar('Confirmación', 'Desea eliminar "' + document.getElementById("asunto").value + '" ?', 'Confirmar', 'Cancelar', () => fncEliminarSalaJuntas());
            });
            //#endregion

            //#region FUNCIONES GET SALAS Y REUNIONES

            function fncFillSalas() {
                let obj = {}
                obj.FK_Edificio = $("#selectEdificio").val();
                axios.post('FillSalas', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        salas = Array();
                        for (let index = 0; index < items.length; index++) {
                            var x = {
                                id: items[index].Value,
                                title: items[index].Text,
                                prefijo: items[index].Prefijo
                            };
                            salas.push(x);
                        }
                        initCalendar();

                    } else {
                        Alert2Warning(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }

            function fncGetAdminSalas() {
                let obj = {}
                obj.FK_Edificio = cboSelectEdificios.val();
                axios.post('GetSalaJuntas', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        reuniones.length = 0;
                        fncFillCboCatSalas();
                        for (let index = 0; index < items.length; index++) {

                            //#region ESTRUCTURAS REPETICIONES

                            // DÍAS SEMANA: D0-L1-M2-X3-J4-V5-S6

                            switch (items[index].repeticion) {
                                case "nunca":     //  EVENTO SENCILLO
                                    var x = {
                                        color: "#E45932",
                                        id: items[index].id,
                                        title: items[index].asunto,
                                        resourceId: items[index].FK_Sala,
                                        start: moment(items[index].fechaInicio).format('HH:mm'),
                                        end: moment(items[index].fechaFin).format('HH:mm'),
                                        // finRep: moment(items[index].fechaFinRepeticion).format('YYYY/MM/DD'),
                                        dow: [moment(items[index].fechaInicio).day()],
                                        ranges: [{
                                            start: moment(items[index].fechaInicio).startOf("day").format('YYYY-MM-DD HH:mm:ss'),
                                            end: moment(items[index].fechaInicio).endOf('day').format('YYYY-MM-DD HH:mm:ss'),
                                            finRep: null,
                                        }],
                                        comentarios: items[index].comentarios,
                                        tipoRep: items[index].repeticion,
                                    };
                                    break;
                                case "habiles":   //  ENTRE SEMANA
                                    var x = {
                                        color: '#C56A1D',
                                        id: items[index].id,
                                        title: items[index].asunto,
                                        resourceId: items[index].FK_Sala,
                                        start: moment(items[index].fechaInicio).format('HH:mm'),
                                        end: moment(items[index].fechaFin).format('HH:mm'),
                                        finRep: moment(items[index].fechaFinRepeticion).format('YYYY/MM/DD'),
                                        dow: [1, 2, 3, 4, 5],
                                        ranges: [{
                                            start: moment(items[index].fechaInicio).startOf("day").format('YYYY-MM-DD HH:mm:ss'),
                                            end: moment(items[index].fechaFinRepeticion).endOf('day').format('YYYY-MM-DD HH:mm:ss'),
                                        }],
                                        comentarios: items[index].comentarios,
                                        tipoRep: items[index].repeticion,
                                    };
                                    break;
                                case "diario":    //  TODOS LOS DÍAS
                                    var x = {
                                        color: '#FA9E64',
                                        id: items[index].id,
                                        title: items[index].asunto,
                                        resourceId: items[index].FK_Sala,
                                        start: moment(items[index].fechaInicio).format('HH:mm'),
                                        end: moment(items[index].fechaFin).format('HH:mm'),
                                        finRep: moment(items[index].fechaFinRepeticion).format('YYYY/MM/DD'),
                                        // dow: [0, 1, 2, 3, 4, 5, 6], FUNCIONA SIN ESTO
                                        ranges: [{
                                            start: moment(items[index].fechaInicio).startOf("day").format('YYYY-MM-DD HH:mm:ss'),
                                            end: moment(items[index].fechaFinRepeticion).endOf('day').format('YYYY-MM-DD HH:mm:ss'),
                                        }],
                                        comentarios: items[index].comentarios,
                                        tipoRep: items[index].repeticion,
                                    };
                                    break;
                                case "semanal":   //  SEMANAL
                                    var x = {
                                        color: '#98261E',
                                        id: items[index].id,
                                        title: items[index].asunto,
                                        resourceId: items[index].FK_Sala,
                                        start: moment(items[index].fechaInicio).format('HH:mm'),
                                        end: moment(items[index].fechaFin).format('HH:mm'),
                                        finRep: moment(items[index].fechaFinRepeticion).format('YYYY/MM/DD'),
                                        dow: [moment(items[index].fechaInicio).day()],
                                        ranges: [{
                                            start: moment(items[index].fechaInicio).startOf("day").format('YYYY-MM-DD HH:mm:ss'),
                                            end: moment(items[index].fechaFinRepeticion).endOf('day').format('YYYY-MM-DD HH:mm:ss'),
                                        }],
                                        comentarios: items[index].comentarios,
                                        tipoRep: items[index].repeticion,
                                    };
                                    break;
                                case "mensual":   //  MENSUAL      
                                    var m = monthsDiff(
                                        moment(items[index].fechaInicio).format('YYYY-MM-DD HH:mm:ss'),
                                        moment(items[index].fechaFinRepeticion).format('YYYY-MM-DD HH:mm:ss')
                                    )
                                    var rangosM = new Array;
                                    for (let x = 0; x <= m; x++) {
                                        let r = {
                                            start: moment(items[index].fechaInicio).startOf("day").add(x, "month"),
                                            end: moment(items[index].fechaInicio).endOf('day').add(x, "month"),
                                        };
                                        rangosM.push(r);
                                    }

                                    var x = {
                                        color: '#895739',
                                        id: items[index].id,
                                        title: items[index].asunto,
                                        resourceId: items[index].FK_Sala,
                                        start: moment(items[index].fechaInicio).format('HH:mm'),
                                        end: moment(items[index].fechaFin).format('HH:mm'),
                                        finRep: moment(items[index].fechaFinRepeticion).format('YYYY/MM/DD'),
                                        ranges: rangosM,
                                        comentarios: items[index].comentarios,
                                        tipoRep: items[index].repeticion,
                                    };
                                    break;
                                case "anual":     //  ANUAL
                                    var y = yearsDiff(
                                        moment(items[index].fechaInicio).format('YYYY-MM-DD HH:mm:ss'),
                                        moment(items[index].fechaFinRepeticion).format('YYYY-MM-DD HH:mm:ss')
                                    );
                                    var rangosY = new Array;
                                    for (let x = 0; x <= y; x++) {
                                        let r = {
                                            start: moment(items[index].fechaInicio).startOf("day").add(x, "years"),
                                            end: moment(items[index].fechaInicio).endOf('day').add(x, "years"),
                                        };
                                        rangosY.push(r);
                                    }

                                    var x = {
                                        // color: 'gray',
                                        id: items[index].id,
                                        title: items[index].asunto,
                                        resourceId: items[index].FK_Sala,
                                        start: moment(items[index].fechaInicio).format('HH:mm'),
                                        end: moment(items[index].fechaFin).format('HH:mm'),
                                        finRep: moment(items[index].fechaFinRepeticion).format('YYYY/MM/DD'),
                                        ranges: rangosY,
                                        comentarios: items[index].comentarios,
                                        tipoRep: items[index].repeticion,
                                    };
                                    break;
                                case "otro":      //  PERSONALIZADA (SEMANAL)
                                    var x = {
                                        color: '#F3A93E',
                                        id: items[index].id,
                                        title: items[index].asunto,
                                        resourceId: items[index].FK_Sala,
                                        start: moment(items[index].fechaInicio).format('HH:mm'),
                                        end: moment(items[index].fechaFin).format('HH:mm'),
                                        finRep: moment(items[index].fechaFinRepeticion).format('YYYY/MM/DD'),
                                        dow: JSON.parse("[" + items[index].diasRepeticion + "]"),  // SELECCIONADOS DE CHECKBOX
                                        ranges: [{
                                            start: moment(items[index].fechaInicio).startOf("day").format('YYYY-MM-DD HH:mm:ss'),
                                            end: moment(items[index].fechaFinRepeticion).endOf('day').format('YYYY-MM-DD HH:mm:ss'),
                                        }],
                                        comentarios: items[index].comentarios,
                                        tipoRep: items[index].repeticion,
                                    };
                                    break;
                            }
                            //#endregion

                            reuniones.push(x);
                        }
                    } else {
                        Alert2Warning(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
            //#endregion

            //#region FILL CBOs
            function fncFillCboCatEdificios() {
                cboCE_CatEdificios.fillCombo('FillCboCatEdificios', { FK_AdminSalas: btnCEAdminSalas.data().id }, false, null);

                cboSelectEdificios.fillCombo('FillCboCatEdificios', { FK_AdminSalas: btnCEAdminSalas.data().id }, false, null);
            }
            function fncFillCboCatSalas() {
                cboCE_CatSalas.fillCombo('FillCboCatSalas', { FK_Edificio: $("#selectEdificio").val() }, false, null);
            }
            //#endregion
        }

        //#region FUNCIONES GET DIFERENCIA MESES Y AÑOS
        function monthsDiff(date1, date2) {

            var date1 = new Date(date1);
            var date2 = new Date(date2);

            var months = (
                date2.getDate() - date1.getDate()) / 30 +
                date2.getMonth() - date1.getMonth() +
                (12 * (date2.getFullYear() - date1.getFullYear())
                );

            return Math.floor(months)
        }

        function yearsDiff(date1, date2) {

            var date1 = new Date(date1);
            var date2 = new Date(date2);

            var years = moment(date2).diff(date1, 'years');
            return years;
        }
        //#endregion

        //#region GET DIAS REPETICION     

        function getDiasRepeticion() {
            let dias = [];
            let checkboxes = document.querySelectorAll("input[type='checkbox']:checked");
            for (let i = 0; i < checkboxes.length; i++) {
                dias.push(checkboxes[i].value);
            }
            return dias.toString();
        }

        //#endregion

        //#region HEADER 
        $('.fc-prevYear-button').click(function () {
            if ($("#selectEdificio").val() != "") {
                $('#calendar').fullCalendar('prevYear');
            }
            else {
                Alert2Warning("Seleccione un edificio.");
            }
        });
        $('.fc-prev-button').click(function () {
            if ($("#selectEdificio").val() != "") {
                $('#calendar').fullCalendar('prev');
            }
            else {
                Alert2Warning("Seleccione un edificio.");
            }
        });
        $('.fc-next-button').click(function () {
            if ($("#selectEdificio").val() != "") {
                $('#calendar').fullCalendar('next');
            }
            else {
                Alert2Warning("Seleccione un edificio.");
            }
        });
        $('.fc-nextYear-button').click(function () {
            if ($("#selectEdificio").val() != "") {
                $('#calendar').fullCalendar('nextYear');
            }
            else {
                Alert2Warning("Seleccione un edificio.");
            }
        });
        $('.fc-today-button').click(function () {
            if ($("#selectEdificio").val() != "") {
                $('#calendar').fullCalendar('today');
            }
            else {
                Alert2Warning("Seleccione un edificio.");
            }
        });
        $('.fc-dayView-button').click(function () {
            if ($("#selectEdificio").val() != "") {
                $('#calendar').fullCalendar('changeView', 'agendaDay');
            }
            else {
                Alert2Warning("Seleccione un edificio.");
            }
        });
        $('.fc-weekView-button').click(function () {
            if ($("#selectEdificio").val() != "") {
                $('#calendar').fullCalendar('changeView', 'agendaWeek');
            }
            else {
                Alert2Warning("Seleccione un edificio.");
            }
        });
        $('.fc-monthView-button').click(function () {
            if ($("#selectEdificio").val() != "") {
                $('#calendar').fullCalendar('changeView', 'month');
            }
            else {
                Alert2Warning("Seleccione un edificio.");
            }
        });
        $('.fc-new-button').click(function () {
            if ($("#selectEdificio").val() != "") {
                btnCEAdminSalas.data().id = 0;
                //reset modal
                $('.modal').on('hidden.bs.modal', function () {
                    $(this).find('form')[0].reset();
                    $('select').reset();
                });
                $('input[type=checkbox]').attr('checked', false);
                document.getElementById("btnEliminar").style.display = 'none';
                $("#btnCEAdminSalas").html('Guardar');
                document.getElementById("hidden-div").style.display = 'none';
                document.getElementById("diasRepeticion").style.display = 'none';
                document.getElementById("comentarios").value = null;
                document.getElementById("fechaInicio").value = moment().format('YYYY/MM/DD HH:mm')
                document.getElementById("fechaFin").value = moment().add(1, "hour").format('YYYY/MM/DD HH:mm');
                document.getElementById("fechaFinRepeticion").value = moment().add(1, "month").format('YYYY/MM/DD');
                document.getElementById("btn_modal").click();
                cboCE_CatEdificios.val($("#selectEdificio").val())
                cboCE_CatEdificios.trigger("change")
                fncFillCboCatSalas();
            }
            else {
                Alert2Warning("Seleccione un edificio.");
            }
        });
        //#endregion


        function initCalendar() {
            $("#calendar").fullCalendar({
                //#region CONFIG BASICA CALENDARIO
                schedulerLicenseKey: "CC-Attribution-NonCommercial-NoDerivatives",
                selectable: true,
                selectHelper: true,
                minTime: "07:00",
                maxTime: "19:00",
                navLinks: "true",
                navLinkDayClick: function (date, jsEvent) {
                    $('#calendar').fullCalendar('changeView', 'agendaDay', date);
                },
                buttonText: {
                    today: 'Hoy',
                    month: 'Mes',
                    week: 'Semana',
                    day: 'Día'
                },
                allDaySlot: false,
                firstDay: 1,
                eventLimit: true,
                dayMaxEvents: true,
                nowIndicator: "true",
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
                header: false,
                showNonCurrentDates: false,
                fixedWeekCount: false,
                defaultView: "agendaDay",
                views: {
                    day: {
                        displayEventTime: true,
                        displayEventEnd: true,
                        timeFormat: "hh:mm a",
                        titleFormat: "dddd, DD MMMM YYYY",
                    },
                    week: {
                        groupByDateAndResource: true,
                        titleFormat: "D MMMM, YYYY",
                        columnFormat: "ddd D",
                        timeFormat: "hh:mm a",
                    },
                    month: {
                        eventLimit: 3,
                    },
                },
                contentHeight: 'auto',
                eventLimitText: "más",
                viewRender: function (view) {
                    var title = view.title;
                    $("#externalTitle").html(title);
                },

                //#region RESTRICCIONES P CREAR O ARRASTRAR EVENTOS
                businessHours: {
                    start: '07:00',
                    end: '19:00',
                    dow: [0, 1, 2, 3, 4, 5, 6]
                },
                selectConstraint: {
                    start: moment(),
                    end: moment().add(5, "years")
                },
                eventConstraint: "businessHours",
                eventDrop: false,
                eventResize: false,
                eventOverlap: false, // when load event
                selectOverlap: false, // when create event from calendar
                //#endregion

                //#endregion

                eventRender: function (event, element, view, el) {
                    return (event.ranges.filter(function (range) {
                        return (event.start.isBefore(range.end) &&
                            event.end.isAfter(range.start));
                    }).length) > 0;
                },

                // validRange: {
                //     start: moment().startOf("day").subtract(2, "days"),
                // },

                //#region MOUSEOVER EVENT + RESOURCES
                eventMouseover: function (calEvent, jsEvent, resource) {
                    var tooltip = '<div class="tooltipevent mayus">'
                        + calEvent.start.format("ddd D MMMM") + "<br>"
                        + "<b>" + calEvent.start.format("hh:mm A") + " - " + calEvent.end.format("hh:mm A") + "</b>"
                        + "<br><b>" + calEvent.title
                        + "</b><br>" + $("#calendar").fullCalendar("getEventResource", calEvent).title
                        + '</div>';
                    $("body").append(tooltip);
                    $(this).mouseover(function (e) {
                        $(this).css('z-index', 10000);
                        $('.tooltipevent').fadeIn('500');
                        $('.tooltipevent').fadeTo('10', 1.9);
                    })
                        .mousemove(function (e) {
                            $('.tooltipevent').css('top', e.pageY + 10);
                            $('.tooltipevent').css('left', e.pageX + 20);
                        });
                },
                eventMouseout: function (calEvent, jsEvent) {
                    $(this).css('z-index', 8);
                    $('.tooltipevent').remove();
                },
                resourceRender: function (resourceObj, $th) {
                    $th.popover({
                        title: resourceObj.title,
                        content: 'Capacidad: ' + resourceObj.prefijo,
                        trigger: 'hover',
                        placement: 'bottom',
                        container: 'body'
                    })
                },
                //#endregion

                //#region CONFIG ROTAR EVENTOS
                eventAfterAllRender: function (view) {
                    if (view['name'] != "dayGridMonth") {
                        $('.fc-event .fc-content').each(function () {
                            var e = $(this);
                            if (e.width() < 60) {
                                var p = e.parent();
                                var h = p.height();
                                var w = p.width();
                                e.css({
                                    'float': 'left',
                                    'transform': 'rotate(90deg)',
                                    'transform-origin': 'left top 0',
                                    'width': h + 'px',
                                    'margin-left': w + 'px'
                                });
                                e.find('.fc-time').css({
                                    // 'display': 'inline-block',
                                    'margin-right': '5px'
                                });
                                e.find('.fc-title').css('display', 'inline-block');
                            }
                        });
                    }
                },
                //#endregion

                resources: salas,
                events: reuniones,

                eventClick:
                    function click_evento(event) {
                        var reunion = reuniones.find(x => x.id === event.id);
                        $('input[type=checkbox]').attr('checked', false);
                        document.getElementById("btnEliminar").style.display = 'inline';
                        $("#btnCEAdminSalas").html('Actualizar');
                        cboCE_CatSalas.val(event.resourceId);
                        cboCE_CatSalas.trigger("change");
                        btnEliminar.data().id = event.id;
                        btnCEAdminSalas.data().id = event.id;
                        cboCE_CatEdificios.val($("#selectEdificio").val())
                        cboCE_CatEdificios.trigger("change")
                        document.getElementById("asunto").value = event.title;
                        document.getElementById("comentarios").value = event.comentarios;
                        document.getElementById("repeticion").value = event.tipoRep;
                        document.getElementById("fechaInicio").value = event.start.format('YYYY/MM/DD HH:mm');
                        document.getElementById("fechaFin").value = event.end.format('YYYY/MM/DD HH:mm');

                        //#region FILL INPUT FECHA FIN REPETICION
                        if (event.finRep != null) {
                            document.getElementById("fechaFinRepeticion").value = event.finRep;
                        }
                        else {
                            document.getElementById("fechaFinRepeticion").value = event.start.add(1, "month").format("YYYY/MM/DD");
                        }
                        //#endregion

                        //#region FILL CHECKBOXES DIAS REPETICION
                        switch (document.getElementById("repeticion").value) {
                            case "nunca":
                                document.getElementById("hidden-div").style.display = 'none';
                                document.getElementById("diasRepeticion").style.display = 'none';
                                break;
                            case "otro":
                                document.getElementById("hidden-div").style.display = 'block';
                                document.getElementById("diasRepeticion").style.display = 'block';

                                for (let i = 0; i < 7; i++) {
                                    if ((reunion.dow).includes(i)) {
                                        $("input:checkbox[value='" + i + "']").attr("checked", true);
                                    }
                                }
                                break;
                            default:
                                document.getElementById("hidden-div").style.display = 'block';
                                document.getElementById("diasRepeticion").style.display = 'none';
                                break;
                        }
                        //#endregion

                        document.getElementById("btn_modal").click();
                    },

                select: //selectevent 
                    function (start, end, jsEvent, view, resource, revertFunc, resourceObj) {
                        btnCEAdminSalas.data().id = 0;
                        $('.modal').on('hidden.bs.modal', function () {
                            $(this).find('form')[0].reset();
                            $('select').reset();
                        });
                        document.getElementById("comentarios").value = null;
                        cboCE_CatSalas.val(resource.id);
                        cboCE_CatSalas.trigger("change");
                        cboCE_CatEdificios.val($("#selectEdificio").val())
                        cboCE_CatEdificios.trigger("change")
                        document.getElementById("btnEliminar").style.display = 'none';
                        $("#btnCEAdminSalas").html('Guardar');
                        document.getElementById("hidden-div").style.display = 'none';
                        document.getElementById("diasRepeticion").style.display = 'none';
                        document.getElementById("fechaInicio").value = start.format('YYYY/MM/DD HH:mm');
                        document.getElementById("fechaFin").value = end.format('YYYY/MM/DD HH:mm');
                        document.getElementById("fechaFinRepeticion").value = end.add(1, "month").format('YYYY/MM/DD');
                        document.getElementById("btn_modal").click();
                    },
            });
        }
        //#endregion
    }

    $(document).ready(() => { OTROS_SERVICIOS.Calendario = new Calendario(); })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();