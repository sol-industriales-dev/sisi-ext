(() => {
    $.namespace('CH.Pendientes');

    //#region CRUD CONSTS

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
	
	//#endregion

    let changeStatus = null;

	Pendientes = function () {
		(function init() {
            
			initTblRH_Vacaciones_Vacaciones();
            fncGetVacaciones();
			fncListeners();
            initCalendar();
		})();

        btnFiltroBuscar.on("click", function(){
            fncGetVacaciones();
        });

		function fncListeners() {
			
            cboVacacionPeriodo.fillCombo('/Administrativo/Vacaciones/FillComboPeriodos', {}, false);

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
                    //render: function (data, type, row) { }
                    { data: 'nombreEmpleado', title: 'Empleado' },
                    { title: 'Estado' , 
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
                    { data: 'nombreResponsable', title: 'Responsable' },
                    { title: 'Fecha(s) de Ausencias', 
                        render: function (data, type, row) {
                            return `${moment(row.fechaInicial).format("DD/MM/YYYY")} → ${moment(row.fechaFinal).format("DD/MM/YYYY")}`;
                        }
                    },
                    { title: '' , 
                        render: function (data, type, row, meta) {
                            let range = moment(row.fechaInicial).diff(moment(), "days");
                            return range<7? `<b><span style="color:orange">!Antencion¡</span> Quedan <span style="color:red">${range+1}</span> días restantes</b>` :`Quedan ${range+1} días restantes`;
                        }
                    },
                    { 
                        title: '',
                        render: function (data, type, row) {
                            return `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Ver Vacaciones"><button class="btn btn-primary setVacaciones btn-xs" style="width:100%"><i class="far fa-calendar-alt"></i> VER</button></span>`;
                        }
                    },
                    // {

                    //     render: function (data, type, row) {
                    //         let btnAutorizar = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Autorizar"><button class="btn btn-success autorizarVacacion btn-xs" ${row.estado == 3? "" : "disabled"}><span class="glyphicon glyphicon-ok"></i></button>&nbsp;</span>`;
                    //         let btnRechazar = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Rechazar"><button class="btn btn-danger rechazarVacacion btn-xs" ${row.estado == 3 ? "" : "disabled"}><span class="glyphicon glyphicon-remove"></i></button></span>`;
                    //         let btns = btnAutorizar + btnRechazar;
                    //         return btns;
                    //     }
                    // },
                    { data: 'id', title: 'id', visible: false},
                ],
                initComplete: function (settings, json) {
                    tblRH_Vacaciones_Vacaciones.on('click','.autorizarVacacion', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();

                        Alert2AccionConfirmar('¡Cuidado!','¿Desea autorizar la vacacion seleccionada?','Confirmar','Cancelar', () => fncAutorizarVacacion(rowData.id, 1));
                        
                    });
                    tblRH_Vacaciones_Vacaciones.on('click','.rechazarVacacion', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        
                        Alert2AccionConfirmar('¡Cuidado!','¿Desea rechazar la vacacion seleccionada?','Confirmar','Cancelar', () => fncAutorizarVacacion(rowData.id, 2));
                    });
                    tblRH_Vacaciones_Vacaciones.on('click','.setVacaciones', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        btnCalendarioGuardar.attr("data-id",rowData.id)
                        mdlCalendario.modal("show");

                        $('#calendar').fullCalendar('removeEvents');
                        $('#calendar').fullCalendar('addEventSource', [{
                            title: 'Rango',
                            start: moment(rowData.fechaInicial).format("YYYY-MM-DD"),
                            end: moment(rowData.fechaFinal).add(1, 'days').format("YYYY-MM-DD"),
                            color: '#BEBEBE',
                            rendering: "inverse-background"
                        }]);

                        $('#calendar').fullCalendar('gotoDate',moment(rowData.fechaInicial));
                        fncGetFechas(rowData.claveEmpleado);

                    });
                },
                columnDefs: [
                    { className: 'dt-center','targets': '_all'},
                    { "width": "20%", "targets": 4 },
                    { "width": "6%", "targets": 5 },
                ],
            });
        }

        //#endregion

        //#region BACK END

        function fncGetVacaciones() {
            let objFiltro = fncGetFiltros();
            axios.post("GetVacacionesPendientes", objFiltro).then(response => {
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

        function fncAutorizarVacacion(idVac, estadoNum){
            obj = {
                id: idVac,
                estado: estadoNum
            }

            axios.post("AutorizarVacacion", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    switch (estadoNum) {
                        case 1:
                            Alert2Exito("Vacacion Autorizada");
                            break;
                        case 2:
                            Alert2Exito("Vacacion Rechazada");
                            break;
                    }
                    
                    fncGetVacaciones();
                }
            }).catch(error => Alert2Error(error.message));
        }                          

        function fncGetFechas(idEmpleado){
            fncGetNumDias(idEmpleado);

            let obj = {
                idReg: idEmpleado,
            }

            axios.post("GetFechas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    fncFillCallendar(items,idEmpleado);

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetNumDias(idReg){
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

        //#endregion

        //#region FNC GRALES

        function fncGetFiltros() {
            let objFiltro = {
                estado: cboFiltroEstado.val(),
                idPeriodo: cboVacacionPeriodo.val()
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
                select: function (startStr,endStr) {
                    
                }

            });

        }

        function fncFillCallendar(lstFechas, idEmpleado){
            
            if (lstFechas.length>0) {

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
            }).catch(error => Alert2Error(error.message));
            
        }


        //#endregion


	}

    $(document).ready(() => {
        CH.Pendientes = new Pendientes();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();