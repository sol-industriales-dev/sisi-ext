(() => {
    $.namespace('CH.Vacaciones');

    //#region CRUD CONSTS
    const cboVacacionCC = $('#cboVacacionCC');

    const mdlCalendario = $('#mdlCalendario');
    const tblRH_Vacaciones_Vacaciones = $('#tblRH_Vacaciones_Vacaciones');
    const cboFiltroEstado = $('#cboFiltroEstado');
    const cboVacacionPeriodo = $('#cboVacacionPeriodo');
    const btnFiltroBuscar = $('#btnFiltroBuscar');

    let dtRH_Vacaciones_Vacaciones;
    //#endregion

    //#region CALENDARIO
    const calendar = $("#calendar");
    const btnCalendarioGuardar = $('#btnCalendarioGuardar');
    const btnTxtCalendario = $('#btnTxtCalendario');
    const txtCalendarioNumDias = $('#txtCalendarioNumDias');
    const divCECalendario = $('#divCECalendario');
    const divCELactancia = $('#divCELactancia');
    const dateCELactanciaInicio = $('#dateCELactanciaInicio');
    const dateCELactanciaFin = $('#dateCELactanciaFin');
    //#endregion

    //#region CONST FIRMAS
    let dtFirmas;
    const tblFirmas = $('#tblFirmas');
    const mdlFirmas = $('#mdlFirmas');
    let estatusEnum = ["-", "AUTORIZADO", "-", "-"];
    //#endregion

    //#region CONST MDL COMENTARIO
    const mdlComentario = $('#mdlComentario');
    const txtMdlComentario = $('#txtMdlComentario');
    //#endregion

    let changeStatus = null;

    Vacaciones = function () {
        (function init() {

            initTblRH_Vacaciones_Vacaciones();
            initTblFirmas();
            fncGetVacaciones();
            fncListeners();
            initCalendar();
        })();

        btnFiltroBuscar.on("click", function () {
            fncGetVacaciones();
        });

        function fncListeners() {

            cboVacacionPeriodo.fillCombo('/Administrativo/Vacaciones/FillComboPeriodos', {}, false);

            dateCELactanciaInicio.on("change", function () {
                console.log((dateCELactanciaInicio.val()));
                let currentDate = dateCELactanciaInicio.val().split('/');
                dateCELactanciaFin.val(moment(`${currentDate[2]}-${currentDate[1]}-${currentDate[0]}`).add(3, "months").format("DD/MM/YYYY"));
            });

            cboVacacionCC.fillCombo('/Administrativo/Vacaciones/FillComboCC', {}, false);

        }

        //#region TBL

        function initTblRH_Vacaciones_Vacaciones() {
            dtRH_Vacaciones_Vacaciones = tblRH_Vacaciones_Vacaciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: false,
                searching: false,
                bFilter: true,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    {
                        data: 'folio', title: "Folio",
                        render: function (data, type, row) {
                            return (row.cc == null ? "" : row.cc) + "-" + (row.claveEmpleado == null ? "" : row.claveEmpleado) + "-" + (row.consecutivo == null ? "" : row.consecutivo.toString().padStart(3, '0'))

                        }
                    },
                    { data: 'nombreEmpleado', title: 'Empleado' },
                    { data: 'ccDesc', title: 'CC' },
                    {
                        title: 'Estado',
                        render: function (data, type, row, meta) {
                            switch (row.estado) {
                                case 1:
                                    return `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Autorizadas"><button class="btn btn-xs btn-success"  disabled><i class="fas fa-check"></i></button></span>`;
                                case 2:
                                    return `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Rechazadas"><button class="btn btn-xs btn-danger" disabled><i class="fa fa-times"></i></button></span>`;
                                case 3:
                                    return `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Pendientes"><button class="inputs pointer btn btn-xs btn-warning" disabled><i class="fas fa-hourglass-half"></i></button></span>`;
                            }
                        }
                    },
                    {
                        title: 'Tipo Ausencia',
                        render: function (data, type, row) {
                            let vacacionesDesc = "";
                            switch (row.tipoVacaciones) {
                                case 0:
                                    vacacionesDesc = "Permiso por paternidad"
                                    break;
                                case 1:
                                    vacacionesDesc = "Permiso por matrimonio"

                                    break;
                                case 2:
                                    vacacionesDesc = "Permiso sindical"

                                    break;
                                case 3:
                                    vacacionesDesc = "Permiso por deceso familiar"

                                    break;
                                case 4:
                                    vacacionesDesc = "Permiso por lactancia"

                                    break;
                                case 5:
                                    vacacionesDesc = "Permiso medico"

                                    break;
                                case 6:
                                    vacacionesDesc = ""

                                    break;
                                case 7:
                                    vacacionesDesc = "Vacaciones"

                                    break;
                                case 8:
                                    vacacionesDesc = "Permiso SIN goce de sueldo"

                                    break;
                                case 9:
                                    vacacionesDesc = "Comision de trabajo"

                                    break;
                                case 10:
                                    vacacionesDesc = "Home office"

                                    break;
                                case 11:
                                    vacacionesDesc = "Tiempo por tiempo"

                                    break;
                                case 13:
                                    vacacionesDesc = "Suspención"

                                    break;
                                default:
                                    break;
                            }

                            return vacacionesDesc;
                        }
                    },
                    {
                        title: '',
                        render: function (data, type, row) {
                            return `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Ver Vacaciones"><button class="btn btn-primary setVacaciones btn-xs" style="width:100%"><i class="far fa-calendar-alt"></i> Ver Fechas</button></span>`;
                        }
                    },
                    {

                        render: function (data, type, row) {

                            let btnDownload = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Descargar Reporte"><button class="btn btn-primary reporteVacacion btn-xs"><i class="fas fa-file-download"></i></button>&nbsp;</span>`;
                            let btnMotivoRechazo = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Ver motivo de rechazo"><button class="btn btn-primary verComentario btn-xs" style="width:100%"><i class="far fa-comments"></i> Comentario</button></span>`
                            let btnAutorizar = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Autorizar"><button class="btn btn-success autorizarVacacion btn-xs" ${row.estado == 3 ? "" : "disabled"}><span class="glyphicon glyphicon-ok"></i></button>&nbsp;</span>`;
                            let btnRechazar = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Rechazar"><button class="btn btn-danger rechazarVacacion btn-xs" ${row.estado == 3 ? "" : "disabled"}><span class="glyphicon glyphicon-remove"></i></button></span>`;
                            let btnFirmas = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Ver autorizantes"><button class="btn btn-primary verFirmas btn-xs"><i class="fas fa-signature"></i></button>&nbsp;</span>`;
                            let btnArchivoActa = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Descargar Archivo Acta"><button class="btn btn-default botonDescargarArchivoActa btn-xs"><i class="fas fa-file-download"></i></button>&nbsp;</span>`

                            let btns = btnFirmas + btnDownload + (row.rutaArchivoActa != null ? btnArchivoActa : '');

                            if (row.esFirmar) {
                                btns += btnAutorizar + btnRechazar;
                            }
                            if (row.estado == 2) {
                                btns = btnMotivoRechazo + btnDownload
                            }
                            return btns;
                        }
                    },
                    { data: 'id', title: 'id', visible: false },
                    { data: 'comentarioRechazada', title: 'comentarioRechazada', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblRH_Vacaciones_Vacaciones.on('click', '.autorizarVacacion', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();

                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea autorizar' + (rowData.tipoVacaciones == 7 ? " las vacaciones seleccionadas" : " el control de ausencias seleccionado") + '?', 'Confirmar', 'Cancelar', () => fncAutorizarVacacion(rowData.id, 1, "", rowData.tipoVacaciones));

                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.rechazarVacacion', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();

                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "Rechazar " + (rowData.tipoVacaciones == 7 ? "vacaciones" : "control de ausencias"),
                            input: 'textarea',
                            width: '50%',
                            showCancelButton: true,
                            html: "<h3>¿Desea rechazar" + (rowData.tipoVacaciones == 7 ? " las vacaciones seleccionadas" : " el control de ausencias seleccionado") + " ?<br>Indicar el motivo:</h3> ",
                            confirmButtonText: "Rechazar",
                            confirmButtonColor: "#d9534f",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#5c636a",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                fncAutorizarVacacion(rowData.id, 2, $('.swal2-textarea').val(), rowData.tipoVacaciones);
                            }
                        });
                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.setVacaciones', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        btnCalendarioGuardar.attr("data-id", rowData.id)
                        mdlCalendario.modal("show");

                        $('#calendar').fullCalendar('removeEvents');

                        // if (rowData.tipoVacaciones == 7) {
                        //     $('#calendar').fullCalendar('addEventSource', [{
                        //         title: 'Rango',
                        //         start: moment(rowData.fechaInicial).format("YYYY-MM-DD"),
                        //         end: moment(rowData.fechaFinal).add(1, 'days').format("YYYY-MM-DD"),
                        //         color: "#BEBEBE",
                        //         rendering: "inverse-background",

                        //     }]);
                        //     $('#calendar').fullCalendar('gotoDate', moment(rowData.fechaInicial));


                        // }
                        // if (rowData.tipoVacaciones == 4) {
                        //     divCELactancia.css("display", "initial");
                        //     divCECalendario.css("display", "none");
                        // } else {
                        //     divCELactancia.css("display", "none");
                        //     divCECalendario.css("display", "initial");
                        // }

                        fncGetFechas(rowData.id, rowData.tipoVacaciones, rowData.claveEmpleado);

                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.verComentario', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();

                        txtMdlComentario.text(rowData.comentarioRechazada);
                        mdlComentario.modal("show");

                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.verFirmas', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();

                        dtFirmas.clear();
                        dtFirmas.rows.add(rowData.lstAutorizantes);
                        dtFirmas.draw();

                        mdlFirmas.modal("show");

                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.reporteVacacion', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        fncGetReporte(rowData.id, rowData.tipoVacaciones);
                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.botonDescargarArchivoActa', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();

                        location.href = `DescargarArchivoActa?id=${rowData.id}`;
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { "width": "30%", "targets": 2 },
                    { "width": "3%", "targets": 3 },
                    { "width": "15%", "targets": 4 },
                    { "width": "6%", "targets": 5 },
                    { "width": "12%", "targets": 6 },
                ],
            });
        }

        function initTblFirmas() {
            dtFirmas = tblFirmas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreCompleto', title: 'USUARIO' },
                    {
                        data: 'estatus', title: 'ESTATUS',
                        render: function (data, type, row) {
                            return estatusEnum[data];
                        }
                    },
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }
        //#endregion

        //#region BACK END

        function fncGetVacaciones() {
            let objFiltro = fncGetFiltros();
            axios.post("GetVacacionesGestion", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtRH_Vacaciones_Vacaciones.clear();
                    dtRH_Vacaciones_Vacaciones.rows.add(items);
                    dtRH_Vacaciones_Vacaciones.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetDatosPersonal(claveUsuario, nombreUsuario, esGerente) {
            let obj = new Object();
            obj = {
                claveEmpleado: claveUsuario,
                nombre: nombreUsuario
            }
            axios.post("GetDatosPersona", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (esGerente) {
                        txtCEVacacionNombreResponsable.val(response.data.objDatosPersona.nombreCompleto);
                    } else {
                        txtCEVacacionNombreEmp.val(response.data.objDatosPersona.nombreCompleto);
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAutorizarVacacion(idVac, estadoNum, mensaje, tipoVacaciones) {
            obj = {
                id: idVac,
                estado: estadoNum,
                msg: mensaje
            }

            axios.post("AutorizarVacacion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    switch (estadoNum) {
                        case 1:
                            Alert2Exito((tipoVacaciones == 7 ? "Vacaciones autorizadas" : "Control de ausencias autorizadas"));
                            break;
                        case 2:
                            Alert2Exito((tipoVacaciones == 7 ? "Vacaciones rechazadas" : "Control de ausencias rechazadas"));
                            break;
                    }

                    fncGetVacaciones();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetFechas(idVacaciones, tipoVacs, idEmpleado) {
            fncGetNumDias(idEmpleado);

            let obj = {
                idReg: idVacaciones,
                tipoPermiso: tipoVacs,
                esGestion: true,
            }

            axios.post("GetFechas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (tipoVacs == 4) {
                        if (items.length > 0) {
                            dateCELactanciaInicio.val(moment(items[0].fecha).format("DD/MM/YYYY"));
                            dateCELactanciaInicio.change();
                        } else {
                            dateCELactanciaInicio.val(moment().format("DD/MM/YYYY"));
                            dateCELactanciaInicio.change();
                        }
                    } else {
                        fncFillCallendar(items, null, tipoVacs);

                    }
                    //fncFillCallendar(items, idEmpleado, tipoVacs);

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetNumDias(idReg) {
            let numDias = 0;

            obj = {
                id: idReg,
            }

            axios.post("GetHistorialDias", obj).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    data.forEach(e => {

                        numDias = e.dias;

                    });
                    txtCalendarioNumDias.text(numDias);

                }
            }).catch(error => Alert2Error(error.message));


        }

        function fncGetReporte(idReg, tipoVacaciones) {
            var path = `/Reportes/Vista.aspx?idReporte=250&idVac=${idReg}&tipoVacaciones=${tipoVacaciones}&esGestion=true`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        //#endregion

        //#region FNC GRALES

        function fncGetFiltros() {
            let objFiltro = {
                estado: cboFiltroEstado.val(),
                idPeriodo: cboVacacionPeriodo.val(),
                cc: cboVacacionCC.val()
            }
            return objFiltro;
        }

        //#endregion

        //#region CALENDARIO

        function initCalendar() {

            calendar.fullCalendar({
                height: 450,
                defaultView: 'month',
                customButtons: {

                },
                header: {
                    left: 'prev,next today miboton,',
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

                }

            });

        }

        function fncFillCallendar(lstFechas, idEmpleado, tipoVacs) {

            if (lstFechas.length > 0) {

                $('#calendar').fullCalendar('gotoDate', moment(lstFechas[0].fecha));

                let vacacionesDesc = "";
                switch (tipoVacs) {
                    case 0:
                        vacacionesDesc = "Permiso por paternidad"
                        break;
                    case 1:
                        vacacionesDesc = "Permiso por matrimonio"

                        break;
                    case 2:
                        vacacionesDesc = "Permiso sindical"

                        break;
                    case 3:
                        vacacionesDesc = "Permiso por deceso familiar"

                        break;
                    case 4:
                        vacacionesDesc = "Permiso por lactancia"

                        break;
                    case 5:
                        vacacionesDesc = "Permiso medico"

                        break;
                    case 6:
                        vacacionesDesc = ""

                        break;
                    case 7:
                        vacacionesDesc = "Vacaciones"

                        break;
                    case 8:
                        vacacionesDesc = "Permiso SIN goce de sueldo"

                        break;
                    case 9:
                        vacacionesDesc = "Comision de trabajo"

                        break;
                    case 10:
                        vacacionesDesc = "Home office"

                        break;
                    case 11:
                        vacacionesDesc = "Tiempo por tiempo"

                        break;
                    case 13:
                        vacacionesDesc = "Suspención"

                        break;
                    default:
                        break;
                }


                lstFechas.forEach(item => {
                    //txtCalendarioNumDias.text(Number(txtCalendarioNumDias.text())-1);
                    let date = moment(item.fecha);
                    $('#calendar').fullCalendar('addEventSource', [{
                        title: vacacionesDesc,
                        start: date,
                    }]);
                });
            }
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Vacaciones = new Vacaciones();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();