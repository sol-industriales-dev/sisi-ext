(() => {
    $.namespace('CH.AplicarIncidencias');

    //#region CRUD CONSTS

    const mdlCalendario = $('#mdlCalendario');
    const tblRH_Vacaciones_Vacaciones = $('#tblRH_Vacaciones_Vacaciones');
    const cboFiltroEstado = $('#cboFiltroEstado');
    const btnFiltroNuevo = $('#btnFiltroNuevo');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroGuardar = $('#btnFiltroGuardar');
    const cboVacacionPeriodo = $('#cboVacacionPeriodo');
    const cboVacacionCC = $('#cboVacacionCC');
    const cboTipo = $('#cboTipo');

    let dtRH_Vacaciones_Vacaciones;

    let notClickablePer = false;
    let notClickableEmp = false;
    //#endregion

    //#region MODAL CE VACACION CONSTS

    const mdlVacacion = $('#mdlVacacion');
    const titleCEVacacion = $('#titleCEVacacion');
    const txtCEVacacionClaveEmp = $('#txtCEVacacionClaveEmp');
    const txtCEVacacionNombreEmp = $('#txtCEVacacionNombreEmp');
    const txtCEVacacionClaveResponsable = $('#txtCEVacacionClaveResponsable');
    const txtCEVacacionNombreResponsable = $('#txtCEVacacionNombreResponsable');
    const btnCEVacacionActualizar = $('#btnCEVacacionActualizar');
    const btnTxtCEVacacion = $('#btnTxtCEVacacion');
    const cboCEVacacionPeriodo = $('#cboCEVacacionPeriodo');
    const txtCEVacacionClaveResponsablePagadas = $('#txtCEVacacionClaveResponsablePagadas');
    const txtCEVacacionNombreResponsablePagadas = $('#txtCEVacacionNombreResponsablePagadas');
    const chkTipoVacaciones = $('#chkTipoVacaciones');
    const groupCEVacacionClaveResponsablePagadas = $('#groupCEVacacionClaveResponsablePagadas');
    const groupCEVacacionNombreResponsablePagadas = $('#groupCEVacacionNombreResponsablePagadas');
    const tooltipCEVacaciones = $('#tooltipCEVacaciones');

    //#endregion

    //#region CALENDARIO

    const calendar = $("#calendar");
    const btnCalendarioGuardar = $('#btnCalendarioGuardar');
    const btnTxtCalendario = $('#btnTxtCalendario');
    const txtCalendarioNumDias = $('#txtCalendarioNumDias');

    //#endregion

    let changeStatus = null;

    AplicarIncidencias = function () {
        (function init() {

            initCalendar();
            initTblRH_Vacaciones_Vacaciones();
            fncGetVacaciones();
            fncListeners();


        })();

        function fncListeners() {

            cboCEVacacionPeriodo.fillCombo('/Administrativo/Vacaciones/FillComboPeriodos', {}, false);
            cboVacacionPeriodo.fillCombo('/Administrativo/Vacaciones/FillComboPeriodos', {}, false);
            cboVacacionCC.fillCombo('/Administrativo/Vacaciones/FillComboCC', {}, false);

            btnFiltroBuscar.on("click", function () {
                fncGetVacaciones();
            });

            btnFiltroGuardar.on("click", function () {
                fncGuardarIncidencias();
            });

            btnFiltroNuevo.on("click", function () {
                fncEmptyFields();
                mdlVacacion.modal("show");
                titleCEVacacion.text("Crear Vacacion");
                btnTxtCEVacacion.text("Guardar");
                btnTxtCEVacacion.prop("disabled", true);
                btnCEVacacionActualizar.attr("data-id", 0);
                tooltipCEVacaciones.attr("Title", "Crear");
                notClickablePer = false;
                notClickableEmp = false;


            });

            btnCEVacacionActualizar.on("click", function () {
                fncCrearEditarVacaciones();
            });

            // btnCalendarioGuardar.on("click", function(){
            //     fncCEFechas();
            // });

            txtCEVacacionClaveEmp.on("change", function () {


                let events = $('#calendar').fullCalendar('clientEvents');
                events.forEach(e => {
                    if (e.title != "Rango") {
                        $('#calendar').fullCalendar('removeEvents', [e._id]);
                    }
                })

                fncGetDatosPersonal(txtCEVacacionClaveEmp.val(), txtCEVacacionNombreEmp.val(), false, false);
                fncGetFechas(txtCEVacacionClaveEmp.val());
                fncGetResponsable(txtCEVacacionClaveEmp.val());

            });

            txtCEVacacionNombreResponsable.getAutocomplete(funGetGerente, null, '/Administrativo/FormatoCambio/getCatEmpleadosReclutamientos');
            txtCEVacacionNombreEmp.getAutocomplete(funGetEmpleado, null, '/Administrativo/FormatoCambio/getCatEmpleadosReclutamientos');

            txtCEVacacionNombreEmp.on("change", function () {

                fncGetDatosPersonal(txtCEVacacionClaveEmp.val(), txtCEVacacionNombreEmp.val(), false, false);

            });

            txtCEVacacionClaveResponsable.on("change", function () {
                fncGetDatosPersonal(txtCEVacacionClaveResponsable.val(), txtCEVacacionNombreResponsable.val(), true, false);
            });

            txtCEVacacionClaveResponsablePagadas.on("change", function () {
                fncGetDatosPersonal(txtCEVacacionClaveResponsablePagadas.val(), txtCEVacacionClaveResponsablePagadas.val(), true, true);

            });

            cboCEVacacionPeriodo.on("change", function () {

                if (cboCEVacacionPeriodo.val() != "") {
                    fncGetPeriodos(cboCEVacacionPeriodo.val());

                } else {
                    $('#calendar').fullCalendar('removeEvents');

                }
                btnTxtCEVacacion.prop("disabled", false);

            });

            chkTipoVacaciones.on("change", function () {
                if (chkTipoVacaciones.prop("checked")) {
                    groupCEVacacionClaveResponsablePagadas.show();
                    groupCEVacacionNombreResponsablePagadas.show();
                } else {
                    groupCEVacacionClaveResponsablePagadas.hide();
                    groupCEVacacionNombreResponsablePagadas.hide();
                }
            })

        }

        //#region TBL

        function initTblRH_Vacaciones_Vacaciones() {
            dtRH_Vacaciones_Vacaciones = tblRH_Vacaciones_Vacaciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreEmpleado', title: 'Empleado', visible: false },
                    { data: 'ccDescripcion', title: 'Centro de Costos' },
                    { data: 'nombreResponsable', title: 'Responsable' },
                    { 
                        title: 'Tipo Incidencia', 
                        render: function (data, type, row, meta) {
                            switch (row.tipoVacaciones) {
                                case 3:
                                    return `<span>PERMISO SIN GOCE</span>`;
                                    break;
                                case 4:
                                    return `<span>PERMISO CON GOCE</span>`;
                                    break;
                                case 5:
                                    return `<span>VACACIONES</span>`;
                                    break;
                                default:
                                    return `<span>N/A</span>`;
                                    break;
                            }
                        } 
                    },
                    {
                        title: 'Fecha',
                        render: function (data, type, row) { return `${moment(row.fecha).format("DD/MM/YYYY")}`; }
                    },
                    {
                        title: 'Aplicar',
                        render: function (data, type, row) { return row.aplica ? '<input type="checkbox" class="checkAplica" data-id="' + row.id + '">' : ''; }
                    },

                ],
                drawCallback: function ( settings ) {
                    var api = this.api();
                    var rows = api.rows( {page:'current'} ).nodes();
                    var last=null;
 
                    api.column(0, { page:'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before('<tr class="grupoEmpleado"><td colspan="6">' + group + '</td></tr>'); 
                            last = group;
                        }
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { "width": "9%", "targets": 5 },
                    { "width": "20%", "targets": 4 },
                    
                ],
                order: [[ 0, 'asc' ]],
            });
        }

        //#endregion

        //#region BACK END

        function fncGetVacaciones() {
            let objFiltro = fncGetFiltros();
            axios.post("GetVacacionesIncidencias", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    dtRH_Vacaciones_Vacaciones.clear();
                    dtRH_Vacaciones_Vacaciones.rows.add(items);
                    dtRH_Vacaciones_Vacaciones.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarVacaciones() {
            let objVacacion = fncVacacionParametros();

            if (objVacacion != "") {
                axios.post("CrearEditarVacaciones", objVacacion).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncCEFechas(items.id);
                        fncGetVacaciones();

                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncEliminarVacacion(idReg) {
            axios.post("EliminarVacacion", { id: idReg }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Vacacion Eliminada.");
                    fncGetVacaciones();

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEFechas(idReg) {
            let events = $('#calendar').fullCalendar('clientEvents');
            let listaFechas = [];
            let dias = 0;

            if (events.length > 0) {

                for (let i = 0; i < events.length; i++) {

                    if (events[i].title == "Vacaciones") {
                        let btmDate = events[i].start.clone();

                        let range = 0;

                        if (events[i].end != null) {
                            if (events[i].end._isValid) {

                                let topDate = events[i].end.clone();

                                range = topDate.diff(btmDate, "days");
                            } else {
                                range = 1;
                            }
                        } else {
                            range = 1;
                        }


                        if (range == 1) {
                            if (events[i].end != null) {
                                listaFechas.push(new Date(btmDate.add(1, "days")));
                            } else {
                                listaFechas.push(new Date(btmDate));

                            }

                        } else {
                            btmDate.add(1, 'days');
                            for (let j = 0; j < range; j++) {

                                listaFechas.push(new Date(btmDate));
                                btmDate.add(1, 'days');
                            }

                        }
                    }

                }
            }

            axios.post("CrearEditarFechas", { idVacacion: idReg, lstFechas: listaFechas, diasPermitidos: txtCalendarioNumDias.text(), tipoPermiso: 5 }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Vacaciones Guardadas.");
                } else {
                    Alert2Error(message)
                }
            }).catch(error => Alert2Error(error.message));

        }

        function fncGetDatosPersonal(claveUsuario, nombreUsuario, esGerente, esGerentePagadas) {
            let obj = new Object();
            obj = {
                claveEmpleado: claveUsuario,
                nombre: nombreUsuario
            }
            axios.post("GetDatosPersona", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (esGerente) {
                        if (esGerentePagadas) {
                            txtCEVacacionNombreResponsablePagadas.val(response.data.objDatosPersona.nombreCompleto);

                        } else {
                            txtCEVacacionNombreResponsable.val(response.data.objDatosPersona.nombreCompleto);

                        }
                    } else {
                        txtCEVacacionNombreEmp.val(response.data.objDatosPersona.nombreCompleto);
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetFechas(idEmpleado) {
            fncGetNumDias();

            let obj = {
                idReg: idEmpleado,
                tipoPermiso: 7
            }

            axios.post("GetFechas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    fncFillCallendar(items, idEmpleado);

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetNumDias() {
            let numDias = 0;

            obj = {
                id: txtCEVacacionClaveEmp.val(),
            }

            axios.post("GetHistorialDias", obj).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    data.forEach(e => {

                        numDias += e.dias;

                    });


                    notClickableEmp = true;
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetPeriodos(idReg) {

            let obj = {
                id: idReg
            }

            axios.post("GetPeriodos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    notClickablePer = true;

                    if (items.length == 1) {
                        let events = $('#calendar').fullCalendar('clientEvents');

                        events.forEach(e => {
                            if (e.title != "Vacaciones") {
                                $('#calendar').fullCalendar('removeEvents', [e._id]);
                            }
                        });
                        $('#calendar').fullCalendar('addEventSource', [{
                            title: 'Rango',
                            start: moment(items[0].fechaInicio).format("YYYY-MM-DD"),
                            end: moment(items[0].fechaFinal).add(1, 'days').format("YYYY-MM-DD"),
                            color: '#BEBEBE',
                            rendering: "inverse-background"
                        }]);
                        $('#calendar').fullCalendar('gotoDate', moment(items[0].fechaInicio));
                    } else {
                        let events = $('#calendar').fullCalendar('clientEvents');
                        events.forEach(e => {
                            if (e.title != "Rango") {
                                $('#calendar').fullCalendar('removeEvents', [e._id]);
                            }
                        });

                    }

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncSetNotificada(idReg) {
            let obj = {
                id: idReg
            }

            axios.post("SetNotificada", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetVacaciones();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetReporte(idReg, tipoVacaciones) {
            var path = `/Reportes/Vista.aspx?idReporte=250&idVac=${idReg}&tipoVacaciones=${tipoVacaciones}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function fncGetResponsable(idEmpleado) {
            let obj = {
                clvEmpleado: idEmpleado
            }

            axios.post("GetResponsable", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    txtCEVacacionClaveResponsable.val(items.clave_responsable);
                    txtCEVacacionClaveResponsable.trigger("change");

                } else {
                    Alert2Error("Este empleado no tiene reponsable asignado")
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#endregion

        //#region FNC GRALES

        function funGetGerente(event, ui) {
            txtCEVacacionClaveResponsable.val(ui.item.id);
            txtCEVacacionNombreResponsable.val(ui.item.value);
            //fncGetFechas(txtCEVacacionClaveEmp.val());
            //fncGetResponsable(txtCEVacacionClaveEmp.val());
        }

        function funGetEmpleado(event, ui) {
            txtCEVacacionClaveEmp.val(ui.item.id);
            txtCEVacacionNombreEmp.val(ui.item.value);

            txtCEVacacionClaveEmp.trigger("change");

        }

        function fncGetFiltros() {
            let objFiltro = {
                estado: cboFiltroEstado.val(),
                idPeriodo: cboVacacionPeriodo.val(),
                ccEmpleado: cboVacacionCC.val(),
                tipoVacaciones: cboTipo.val(),
            }
            return objFiltro;
        }

        function fncVacacionParametros() {
            fncDefaultBorder();

            let strMessage = "";

            if (txtCEVacacionClaveEmp.val() == "") { txtCEVacacionClaveEmp.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (txtCEVacacionNombreEmp.val() == "") { txtCEVacacionNombreEmp.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (txtCEVacacionClaveResponsable.val() == "") { txtCEVacacionClaveResponsable.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (txtCEVacacionNombreResponsable.val() == "") { txtCEVacacionNombreResponsable.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (chkTipoVacaciones.prop("checked")) {
                if (txtCEVacacionClaveResponsablePagadas.val() == "") { txtCEVacacionClaveResponsablePagadas.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
                if (txtCEVacacionNombreResponsablePagadas.val() == "") { txtCEVacacionNombreResponsablePagadas.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            }

            if (strMessage != "") {
                Alert2Warning(strMessage);
                return "";
            } else {
                let obj = new Object();

                obj =
                {

                    id: btnCEVacacionActualizar.attr("data-id"),
                    estado: cboFiltroEstado.val(),
                    nombreEmpleado: txtCEVacacionNombreEmp.val(),
                    claveEmpleado: txtCEVacacionClaveEmp.val(),
                    idPeriodo: cboCEVacacionPeriodo.val(),
                    fechaInicial: '01/01/2022',
                    fechaFinal: '01/01/2022',
                    nombreResponsable: txtCEVacacionNombreResponsable.val(),
                    claveResponsable: txtCEVacacionClaveResponsable.val(),
                    nombreResponsablePagadas: txtCEVacacionNombreResponsablePagadas.val(),
                    claveResponsablePagadas: txtCEVacacionClaveResponsablePagadas.val(),
                    notificada: btnCEVacacionActualizar.attr("data-notificada"),
                    tipoVacaciones: 7,
                }
                return obj;
            }
        }

        function fncDefaultBorder() {
            txtCEVacacionNombreResponsable.css("border", "1px solid #CCC");
            txtCEVacacionClaveEmp.css("border", "1px solid #CCC");
            txtCEVacacionNombreResponsable.css("border", "1px solid #CCC");
            txtCEVacacionClaveResponsable.css("border", "1px solid #CCC");

        }

        function fncEmptyFields() {
            txtCEVacacionNombreEmp.val("");
            txtCEVacacionClaveEmp.val("");
            txtCEVacacionClaveResponsable.val("");
            txtCEVacacionNombreResponsable.val("");
            txtCEVacacionClaveResponsablePagadas.val("");
            txtCEVacacionNombreResponsablePagadas.val("");
            txtCalendarioNumDias.text("");

            let events = $('#calendar').fullCalendar('clientEvents');

            if (events.lenght != 0) {
                cboCEVacacionPeriodo[0].selectedIndex = 0;
                cboCEVacacionPeriodo.trigger("change");
            } else {
                $('#calendar').fullCalendar('removeEvents');
            }

        }

        //#endregion

        //#region CALENDARIO

        function initCalendar() {

            calendar.fullCalendar({
                height: 450,
                defaultView: 'month',
                customButtons: {
                    btnCalendarioLimpiar: {
                        text: 'Limpiar',
                        click: function () {
                            let events = $('#calendar').fullCalendar('clientEvents');
                            fncGetNumDias();
                            //txtCalendarioNumDias.text(fncGetNumDias(btnCalendarioGuardar.attr("data-id")));
                            Alert2AccionConfirmar('¡Cuidado!', '¿Desea quitar TODOS las vacaciones capturadas?', 'Confirmar', 'Cancelar',
                                () =>
                                    events.forEach(e => {
                                        if (e.title != "Rango") {
                                            $('#calendar').fullCalendar('removeEvents', [e._id]);
                                        }
                                    })
                            );

                        }
                    }
                },
                header: {
                    left: 'prev,next today miboton, btnCalendarioLimpiar',
                    center: 'title',
                    //right: 'month,agendaWeek,agendaDay,listWeek'
                    //right: 'listYear,month,agendaWeek,agendaDay'
                    right: 'listYear,month,agendaWeek'
                },
                timeFormat: 'H(:mm)',
                eventLimit: true,
                timezone: 'local',
                buttonText: {
                    today: 'Hoy',
                    month: 'Mes',
                    week: 'Semana',
                    day: 'Dia',
                    list: 'Agenda'
                },
                //contentHeight: false,
                locate: 'ISO',
                //defaultView: 'month',
                displayEventTime: true,//muestra fecha
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
                //ddefaultDate: '2018-03-12',
                //defaultDate: Moment,
                //navLinks: true, // can click day/week names to navigate views
                editable: true,
                selectable: true,
                eventStartEditable: false,


                //eventLimit: true, // for all non-agenda views
                //views: {
                //    agenda: {
                //        eventLimit: 6 // adjust to 6 only for agendaWeek/agendaDay
                //    },
                events: [
                ],
                eventClick: function (calEvent) {
                    let range = 0;
                    if (calEvent.end != null) {
                        range = calEvent.end.diff(calEvent.start, "days");
                    } else {
                        range = 1;
                    }

                    Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el rango seleccionado?', 'Confirmar', 'Cancelar',
                        function () {

                            txtCalendarioNumDias.text(Number(txtCalendarioNumDias.text()) + range);
                            $('#calendar').fullCalendar('removeEvents', [calEvent._id]);
                        }

                    );

                },
                // //editable: true,
                // eventDrop: function (event, delta, revertFunc) {
                //     alert(event.title + " was dropped on " + event.start.format());
                //     if (!confirm(" ¿Se procede a relizar Cambio De Fecha?" + event.start.format())) {
                //         revertFunc();
                //     } else {
                //         ModificacionFecha(event.idMantenimiento, event.start.format(), event.end)
                //     }
                // },
                // eventResize: function (event, delta, revertFunc) {
                //     ModificacionHorarioServicio(event.idMantenimiento, event.end._d, event.start._d);
                // },
                eventRender: function (event, element) {
                    element.find('.fc-time').html("");
                    element.find('.fc-list-item-time').html("");
                },
                // dayClick: function(info) {
                //     alert();
                // }
                select: function (startStr, endStr) {

                    let notClickableDays = true;
                    if (txtCalendarioNumDias.text() < 1) {
                        notClickableDays = false;
                    }

                    if (notClickablePer && notClickableEmp && notClickableDays) {
                        let errorMsg = "";
                        let addVac = true;
                        let addRange = true;
                        let addToday = true;
                        let addDays = true;
                        let dateMinus2Month;
                        let rangeStartDate;
                        let events = $('#calendar').fullCalendar('clientEvents');

                        if (endStr._isValid) {
                            range = endStr.diff(startStr, "days");
                        } else {
                            range = 1;
                        }

                        if (Number(txtCalendarioNumDias.text()) - range < 0) {
                            addDays = false;
                        }

                        if (events.length > 0) {
                            for (let i = 0; i < events.length; i++) {
                                // CONSTRAINTS PARA LA ASIGNACION DE VACACIONES
                                // ** 1er "IF" SOLO UN EVENTO POR DIA ** 
                                // ** 2do 3ero 4to 5to "IF" SE UTILIZAN PARA CUALQUIER SOBREPOSICION DE LOS EVENTOS CAPTURADOS COMO RANGOS** 

                                if (events[i].title == "Vacaciones") {

                                    if (events[i].end != null) {
                                        if (startStr._d == events[i].start._d && endStr._d == events[i].end._d) {
                                            addVac = false;
                                        } else if (startStr._d >= events[i].start._d && endStr._d <= events[i].end._d) {
                                            addVac = false;

                                        } else if (startStr._d < events[i].start._d && endStr._d > events[i].end._d) {
                                            addVac = false;

                                        } else if (startStr._d < events[i].start._d && endStr._d > events[i].start._d) {
                                            addVac = false;

                                        } else if (startStr._d < events[i].end._d && endStr._d > events[i].end._d) {
                                            addVac = false;

                                        }
                                    } else {
                                        if (range != 1) {
                                            if (startStr.format("YYYY-MM-DD") <= events[i].start.format("YYYY-MM-DD") && endStr.format("YYYY-MM-DD") >= events[i].start.format("YYYY-MM-DD")) {
                                                addVac = false;
                                            }
                                        } else {
                                            if (startStr.format("YYYY-MM-DD") == events[i].start.format("YYYY-MM-DD")) {
                                                addVac = false;
                                            }
                                        }
                                    }
                                } else {
                                    //Checar si el dia en el que se esta llenando la vacacion esta dentro del rango permitido del periodo (2 meses antes y antes de que empiezen)

                                    rangeStartDate = events[i].start.clone();
                                    dateMinus2Month = events[i].start.clone();
                                    dateMinus2Month.add(-2, "month");

                                    if (moment()._d < dateMinus2Month._d) {
                                        addToday = false;

                                    } else if (moment()._d >= events[i].start._d) {
                                        addToday = false;

                                    }

                                    // LOS EVENTOS (DIAS DE VACACIONES) NECESITAN ESTAR DENTRO DEL RANGO DEL PERIODO DE LAS VACACIONES              
                                    if (addToday) {
                                        if (startStr._d < events[i].start._d) {
                                            addRange = false;

                                        } else if (endStr._d > events[i].end._d) {
                                            addRange = false;

                                        }
                                    }
                                }
                            }
                        }

                        if (addVac && addRange && addToday && addDays) {
                            txtCalendarioNumDias.text(Number(txtCalendarioNumDias.text()) - range);

                            $('#calendar').fullCalendar('addEventSource', [{
                                title: 'Vacaciones',
                                start: startStr,
                                end: endStr,

                            }]);

                        }
                        if (!addDays) {
                            Alert2Warning("Numero de día(s) mayor a los disponibles");
                        } else {
                            if (!addToday) {
                                Alert2Warning(`Periodo disponible de ${dateMinus2Month.format("DD/MM/YYYY")} al ${rangeStartDate.format("DD/MM/YYYY")}`);
                            }
                            if (!addRange) {
                                Alert2Warning("Vacaciones fuera de rango del periodo asignado");
                            }
                            if (!addVac) {
                                Alert2Warning("Día(s) Invalido(s)");
                            }
                        }
                    }
                }
            });
        }

        function fncFillCallendar(lstFechas, idEmpleado) {

            let events = $('#calendar').fullCalendar('clientEvents');
            events.forEach(e => {
                if (e.title == "Vacaciones") {
                    $('#calendar').fullCalendar('removeEvents', [e._id]);
                }
            });

            if (lstFechas.length > 0) {

                lstFechas.forEach(item => {
                    //txtCalendarioNumDias.text(Number(txtCalendarioNumDias.text())-1);
                    let date = moment(item.fecha);
                    $('#calendar').fullCalendar('addEventSource', [{
                        title: 'Vacaciones',
                        start: date,
                    }]);
                });
            }

            let obj = {
                cc: null,
                claveEmpleado: idEmpleado
            }

            axios.post("GetResponsables", obj).then(response => {
                let { success, data, message } = response.data;
                if (success) {
                    txtCalendarioNumDias.text(data[0].diasPendientes);
                }
            }).catch(error => { });
        }

        function fncGuardarIncidencias()
        {
            var fechasIDs = $(".checkAplica").map(function() {
                return $(this).data("id");
            }).get();

            axios.post("GuardarVacacionesIncidencias", { fechasIDs: fechasIDs }).then(response => {
                let { success, items, message } = response.data;
                if (success) {                    

                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }


    $(document).ready(() => {
        CH.AplicarIncidencias = new AplicarIncidencias();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();
