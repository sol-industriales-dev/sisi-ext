(() => {
    $.namespace('CH.Retardos');

    //#region CRUD CONSTS

    const mdlCalendario = $('#mdlCalendario');
    const tblRH_Vacaciones_Vacaciones = $('#tblRH_Vacaciones_Vacaciones');
    const cboFiltroEstado = $('#cboFiltroEstado');
    const btnFiltroNuevo = $('#btnFiltroNuevo');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const cboVacacionPeriodo = $('#cboVacacionPeriodo');
    const cboVacacionCC = $('#cboVacacionCC');
    const cboTipoPermiso = $('#cboTipoPermiso');
    const cboTipoVacaciones = $('#cboTipoVacaciones');
    const divCboPermisosingoce = $('#divCboPermisosingoce');
    const divCECboPermisosingoce = $('#divCECboPermisosingoce');
    // const cboCETipoVacaciones = $('#cboCETipoVacaciones');
    const dateFiltroIni = $('#dateFiltroIni');
    const dateFiltroFin = $('#dateFiltroFin');

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
    const txtCEVacacionCC = $('#txtCEVacacionCC');
    const txtCEVacacionFechaIngreso = $('#txtCEVacacionFechaIngreso');
    const txtCEVacacionPuesto = $('#txtCEVacacionPuesto');
    const txtCEVacacionNumJefeInmediato = $('#txtCEVacacionNumJefeInmediato');
    const txtCEVacacionNombreJefeInmediato = $('#txtCEVacacionNombreJefeInmediato');
    const txtCEVacacionJustific = $('#txtCEVacacionJustific');
    const divCEVacacionesNombreMedico = $('#divCEVacacionesNombreMedico');
    const divCapitalHumano = $('#divCapitalHumano');
    const txtCEVacacionNombreMedico = $('#txtCEVacacionNombreMedico');
    const selectCapitalHumano = $('#selectCapitalHumano');
    const inputArchivoActa = $('#inputArchivoActa');
    const divArchivoActa = $('#divArchivoActa');
    const botonArchivoActa = $('#botonArchivoActa');
    const labelArchivoActa = $('#labelArchivoActa');
    const cboCETipoRetardo = $('#cboCETipoRetardo');
    const cboCERetardoMotivos = $('#cboCERetardoMotivos');
    const dateCEDiaTomado = $('#dateCEDiaTomado');
    const cboCEHorario = $('#cboCEHorario');
    const txtCEHorasRequeridas = $('#txtCEHorasRequeridas');
    const txtCEMinutosRequeridos = $('#txtCEMinutosRequeridos');
    const txtCEHorarioLower = $('#txtCEHorarioLower');
    const txtCEHorarioUpper = $('#txtCEHorarioUpper');
    const divHorarioIncidencia = $('#divHorarioIncidencia');
    const divHorarioPermiso = $('#divHorarioPermiso');
    //#endregion

    //#region CALENDARIO

    const calendar = $("#calendar");
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
    const mdlComentario = $('#mdlComentario');
    const txtComentario = $('#txtComentario');
    let estatusEnum = ["-", "AUTORIZADO", "-", "-"];
    //#endregion

    //#region CONST JUSTIFICACION
    const mdlJustific = $('#mdlJustific');
    const txtVacacionJustific = $('#txtVacacionJustific');
    //#endregion

    let changeStatus = null;
    let esUsuarioReg = false;
    let cveUsuarioReg = 0;

    //ES CONSULTA
    const inputEsConsulta = $('#inputEsConsulta');
    let _esConsulta = inputEsConsulta.val();
    let _esAdmn = false;

    Retardos = function () {
        (function init() {

            initCalendar();
            // fncSetPeriodos(false);
            initTblRH_Vacaciones_Vacaciones();
            initTblFirmas();
            fncGetVacaciones();
            fncListeners();

        })();

        function fncListeners() {

            // cboCEVacacionPeriodo.fillCombo('/Administrativo/Vacaciones/FillComboPeriodos', {}, false);
            // cboVacacionPeriodo.fillCombo('/Administrativo/Vacaciones/FillComboPeriodos', {}, false);
            cboVacacionCC.fillCombo('/Administrativo/Vacaciones/FillComboCC', {}, false);

            txtCEVacacionNombreJefeInmediato.fillCombo("FillCboUsuarios", {}, false);
            txtCEVacacionNombreMedico.fillCombo("FillCboUsuarios", {}, false, null, () => {
                txtCEVacacionNombreMedico.val(3357); //PABLO CLAMONT
                txtCEVacacionNombreMedico.trigger("change");
            });

            $(".select2").select2({ width: "100%" });

            dateCELactanciaInicio.datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                dateFormat: "dd/mm/yy",
                yearRange: '2018:c',
                // onClose: function (dateText, inst) {
                //     $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
                // }
            });

            dateCELactanciaFin.datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                dateFormat: "dd/mm/yy",
                yearRange: '2018:c',
                // onClose: function (dateText, inst) {
                //     $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
                // }
            });

            btnFiltroBuscar.on("click", function () {
                fncGetVacaciones();
            });

            btnFiltroNuevo.on("click", function () {
                fncEmptyFields();
                mdlVacacion.modal("show");
                titleCEVacacion.text("Crear Justificacion ");
                btnTxtCEVacacion.text("Guardar");
                btnTxtCEVacacion.prop("disabled", true);
                btnCEVacacionActualizar.attr("data-id", 0);
                tooltipCEVacaciones.attr("Title", "Crear");
                notClickablePer = false;
                notClickableEmp = false;
                // cboCETipoPermiso.attr("disabled", false);
                // cboCETipoVacaciones.attr("disabled", false);

                // txtCEVacacionClaveEmp.attr("disabled", false);
                txtCEVacacionNombreEmp.attr("disabled", false);
                txtCEVacacionNombreEmp.focus();
                divArchivoActa.css('display', 'initial');

                // cboCETipoVacaciones[0].selectedIndex = 0;
                // cboCETipoVacaciones.trigger("change");

                // cboCETipoPermiso.show();

                let events = $('#calendar').fullCalendar('clientEvents');
                events.forEach(e => {
                    if (e.title != "Rango") {
                        $('#calendar').fullCalendar('removeEvents', [e._id]);
                    }
                });

                btnCEVacacionActualizar.show();
                divCEVacacionesNombreMedico.css("display", "none");

                if (esUsuarioReg) {

                    if (_esConsulta != "1") {
                        txtCEVacacionNombreEmp.prop("readonly", true);

                    }

                    // fncGetFechas(txtCEVacacionClaveEmp.val(), cboCETipoPermiso.val());
                    txtCEVacacionNombreResponsable.fillCombo('/Administrativo/Vacaciones/FillComboAutorizantes', { clave_empleado: cveUsuarioReg }, false);
                    fncGetDatosPersonal(cveUsuarioReg, txtCEVacacionNombreEmp.val(), false, false);
                    txtCEVacacionNombreEmp.trigger("change");
                }
            });

            btnCEVacacionActualizar.on("click", function () {
                fncCrearEditarVacaciones(false);
            });

            txtCEVacacionNombreEmp.getAutocomplete(funGetEmpleado, null, '/Administrativo/FormatoCambio/getCatEmpleadosReclutamientos');

            txtCEVacacionClaveResponsable.on("change", function () {
                fncGetDatosPersonal(txtCEVacacionClaveResponsable.val(), txtCEVacacionNombreResponsable.val(), true, false);
            });

            txtCEVacacionClaveResponsablePagadas.on("change", function () {
                fncGetDatosPersonal(txtCEVacacionClaveResponsablePagadas.val(), txtCEVacacionClaveResponsablePagadas.val(), true, true);

            });

            txtCEVacacionNombreResponsable.on("change", function () {
                if ($(this).val() != "") {
                    txtCEVacacionClaveResponsable.val($(this).val());
                } else {
                    txtCEVacacionClaveResponsable.val("");
                }
            });

            chkTipoVacaciones.on("change", function () {
                if (chkTipoVacaciones.prop("checked")) {
                    groupCEVacacionClaveResponsablePagadas.show();
                    groupCEVacacionNombreResponsablePagadas.show();
                } else {
                    groupCEVacacionClaveResponsablePagadas.hide();
                    groupCEVacacionNombreResponsablePagadas.hide();
                }
            });

            inputArchivoActa.on('change', function () {
                let iconoBoton = botonArchivoActa.find('i');

                if (inputArchivoActa[0].files.length > 0) {
                    let textoLabel = inputArchivoActa[0].files[0].name;

                    labelArchivoActa.text(textoLabel);
                    botonArchivoActa.addClass('btn-success');
                    botonArchivoActa.removeClass('btn-default');
                    iconoBoton.addClass('fa-check');
                    iconoBoton.removeClass('fa-upload');
                } else {
                    labelArchivoActa.text('');
                    botonArchivoActa.addClass('btn-default');
                    botonArchivoActa.removeClass('btn-success');
                    iconoBoton.addClass('fa-upload');
                    iconoBoton.removeClass('fa-check');
                }
            });

            dateCELactanciaInicio.on("change", function () {
                console.log((dateCELactanciaInicio.val()));
                let currentDate = dateCELactanciaInicio.val().split('/');
                dateCELactanciaFin.val(moment(`${currentDate[2]}-${currentDate[1]}-${currentDate[0]}`).add(3, "months").format("DD/MM/YYYY"));
            });

            cboTipoVacaciones.on("change", function () {
                if (cboTipoVacaciones.val() == '0') {
                    divCboPermisosingoce.show();
                } else {
                    divCboPermisosingoce.hide();
                }
            });

            // cboCETipoVacaciones.on("change", function () {
            //     // if (cboCETipoVacaciones.val() == '0') {
            //     //     divCECboPermisosingoce.show();
            //     // } else {
            //     //     divCECboPermisosingoce.hide();
            //     //     txtCalendarioNumDias.text("∞").trigger("change");
            //     // }

            //     let events = $('#calendar').fullCalendar('clientEvents');
            //     events.forEach(e => {
            //         if (e.classNames[0] != "Rango") {
            //             $('#calendar').fullCalendar('removeEvents', [e._id]);
            //         }
            //     });
            // });

            txtCalendarioNumDias.on('change', function () {
                if (isNaN(($(this).text()))) {
                    $(this).text('∞');
                }
            });

            txtCEVacacionNombreJefeInmediato.on("change", function () {
                if ($(this).val() != "") {
                    txtCEVacacionNumJefeInmediato.val($(this).val());
                } else {
                    txtCEVacacionNumJefeInmediato.val("");
                }
            });

            cboCETipoRetardo.on("change", function (event, esEditar) {
                if (!esEditar) {
                    if ($(this).val() != "") {
                        if ($(this).val() == "0") {
                            divHorarioIncidencia.show();
                            divHorarioPermiso.hide();
                        } else {
                            divHorarioPermiso.show();
                            divHorarioIncidencia.hide();
                        }
                        cboCERetardoMotivos.fillCombo("FillComboMotivosByTipo", { tipoRetardo: $(this).val() }, false, null, null);
                    }
                    let events = $('#calendar').fullCalendar('clientEvents');
                    events.forEach(e => {
                        if (e.title != "Rango") {
                            $('#calendar').fullCalendar('removeEvents', [e._id]);
                        }

                    });

                    if ($(this).val() != "") {

                        notClickablePer = true;
                        notClickableEmp = true;
                        txtCalendarioNumDias.text(1);
                    } else {
                        notClickablePer = false;
                        notClickableEmp = false;
                        txtCalendarioNumDias.text(0);
                    }
                } else {

                    if ($(this).val() != "") {
                        if ($(this).val() == "0") {
                            divHorarioIncidencia.show();
                            divHorarioPermiso.hide();
                        } else {
                            divHorarioPermiso.show();
                            divHorarioIncidencia.hide();
                        }
                    }
                    if ($(this).val() != "") {

                        notClickablePer = true;
                        notClickableEmp = true;
                        txtCalendarioNumDias.text(1);
                    } else {
                        notClickablePer = false;
                        notClickableEmp = false;
                        txtCalendarioNumDias.text(0);
                    }
                }
            });

            cboCERetardoMotivos.on("change", function (event, esEditar) {

            });

        }

        //#region TBL
        let _motivoForCallback = 0;
        function initTblRH_Vacaciones_Vacaciones() {
            dtRH_Vacaciones_Vacaciones = tblRH_Vacaciones_Vacaciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    {
                        data: 'folio', title: "Folio",
                        render: function (data, type, row) {
                            return (row.cc == null ? "" : row.cc) + "-" + (row.claveEmpleado == null ? "" : row.claveEmpleado) + "-" + (row.consecutivo == null ? "" : row.consecutivo.toString().padStart(3, '0'))

                        }
                    },
                    { data: 'claveEmpleado', title: 'No.' },
                    { data: 'nombreEmpleado', title: 'Empleado' },
                    { data: 'ccDesc', title: 'CC' },
                    {
                        title: 'Estado',
                        render: function (data, type, row, meta) {
                            switch (row.estado) {
                                case 1:
                                    return `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Autorizadas"><button class="btn btn-xs btn-success"  disabled><i class="fa fa-check"></i></button></span>`;
                                case 2:
                                    return `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Rechazadas"><button class="btn btn-xs btn-danger" disabled><i class="fa fa-times"></i></button></span>`;
                                case 3:
                                    return `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Pendientes"><button class="inputs pointer btn btn-xs btn-warning" disabled><i class="fas fa-hourglass-half"></i></button></span>`;
                            }
                        }
                    },
                    {
                        title: 'Tipo',
                        render: function (data, type, row) {
                            let vacacionesDesc = "";
                            switch (row.tipoRetardo) {
                                case 0:
                                    vacacionesDesc = "Justificacion de incidecia mayor"
                                    break;
                                case 1:
                                    vacacionesDesc = "Permiso de salida durante la jornada laboral"
                                    break;
                            }

                            return vacacionesDesc;
                        }
                    },
                    {
                        render: function (data, type, row) {
                            // let bthNotificar = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Notificar"><button class="btn ${row.notificada ? "btn-success" : "btn-warning"} notificarVacacion btn-xs"  ${row.notificada ? "disabled" : ""}><i class="fas fa-envelope"></i></button>&nbsp;</span>`;
                            // let btnDownload = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Descargar Reporte"><button class="btn btn-primary reporteVacacion btn-xs"><i class="fas fa-file-download"></i></button>&nbsp;</span>`;
                            let btnDownload = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Descargar Reporte"><button class="btn btn-primary reporteVacacion btn-xs"><i class="fas fa-file-download"></i></button>&nbsp;</span>`;
                            let btnActualizar = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Actualizar"><button class="btn btn-warning actualizarVacacion btn-xs"><i class="far fa-edit"></i></button>&nbsp;</span>`;
                            let btnEliminar = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Eliminar"><button class="btn btn-danger eliminarVacacion btn-xs esConsulta"><i class="far fa-trash-alt"></i></button>&nbsp;</span>`;
                            let btnFirmas = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Ver autorizantes"><button class="btn btn-primary verFirmas btn-xs"><i class="fas fa-signature"></i></button>&nbsp;</span>`;
                            let btnJustificacion = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Ver justificacion"><button class="btn btn-primary verJustificacion btn-xs"><i class="far fa-comments"></i></button>&nbsp;</span>`;
                            let btnArchivoActa = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Descargar Archivo Acta"><button class="btn btn-default botonDescargarArchivoActa btn-xs"><i class="fas fa-file-download"></i></button>&nbsp;</span>`

                            if (cveUsuarioReg > 0 && row.claveEmpleado == cveUsuarioReg) {
                                btnEliminar = btnEliminar.replace("esConsulta", "");
                            }

                            let btns = btnJustificacion + btnFirmas + btnDownload + (row.rutaArchivoActa != null ? btnArchivoActa : '') + btnActualizar + btnEliminar;

                            if (!_esAdmn) {
                                if (row.estado == 2 || row.estado == 1) {
                                    btns = btnJustificacion + btnFirmas + btnDownload + (row.rutaArchivoActa != null ? btnArchivoActa : '');
                                }
                            }

                            return btns;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblRH_Vacaciones_Vacaciones.on('click', '.actualizarVacacion', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        fncDefaultBorder();

                        inputArchivoActa.val('');
                        inputArchivoActa.change();

                        divCapitalHumano.css('display', 'none');

                        selectCapitalHumano.val('');

                        btnCEVacacionActualizar.attr("data-id", rowData.id);
                        btnCEVacacionActualizar.attr("data-notificada", rowData.notificada);
                        tooltipCEVacaciones.attr("Title", "Actualizar");
                        mdlVacacion.modal("show");
                        titleCEVacacion.text("Actualizar Justificacion");
                        btnTxtCEVacacion.text("Actualizar");
                        txtCEVacacionNombreEmp.val(rowData.nombreEmpleado);
                        txtCEVacacionClaveEmp.val(rowData.claveEmpleado);
                        txtCEVacacionNombreResponsable.val(rowData.claveResponsable);
                        txtCEVacacionNombreResponsable.trigger("change");
                        txtCEVacacionClaveResponsable.val(rowData.claveResponsable);
                        // cboCETipoPermiso.attr("disabled", true);
                        // cboCETipoVacaciones.attr("disabled", true);
                        // txtCEVacacionClaveEmp.attr("disabled", true);
                        txtCEVacacionNombreEmp.attr("disabled", true);
                        txtCEVacacionJustific.val(rowData.justificacion);

                        // cboCETipoPermiso.val(rowData.tipoVacaciones);
                        // cboCETipoPermiso.change();

                        txtCEVacacionCC.val(rowData.ccDesc);

                        txtCEVacacionNombreJefeInmediato.val(rowData.idJefeInmediato);
                        txtCEVacacionNombreJefeInmediato.trigger("change");

                        fncSetAutorizantes(rowData.lstAutorizantes, rowData.claveEmpleado);

                        fncGetDatosPersonal(rowData.claveEmpleado, rowData.nombreEmpleado, false, false, true);

                        cboCETipoRetardo.val(rowData.tipoRetardo);
                        cboCETipoRetardo.trigger("change", ["esEditar"]);
                        rowData.tipoRetardo == 0 ? cboCEHorario.val(rowData.horario) : cboCEHorario.val("");
                        cboCEHorario.trigger("change");
                        rowData.tipoRetardo == 1 ? txtCEHorarioLower.val(`${rowData.horarioLower.Hours.toString().padStart(2, '0')}:${rowData.horarioLower.Minutes.toString().padStart(2, '0')}`) : txtCEHorarioLower.val("");
                        rowData.tipoRetardo == 1 ? txtCEHorarioUpper.val(`${rowData.horarioUpper.Hours.toString().padStart(2, '0')}:${rowData.horarioUpper.Minutes.toString().padStart(2, '0')}`) : txtCEHorarioUpper.val("");
                        txtCEHorasRequeridas.val(rowData.tiempoRequeridoHrs);
                        txtCEMinutosRequeridos.val(rowData.tiempoRequeridoMin);
                        dateCEDiaTomado.val(moment(rowData.diaTomado).format("YYYY-MM-DD"));

                        fncSetPeriodos(false);
                        _motivoForCallback = rowData.motivoJustificacion;
                        cboCERetardoMotivos.fillCombo("FillComboMotivosByTipo", { tipoRetardo: cboCETipoRetardo.val() }, false, null, () => {

                            $("#cboCERetardoMotivos").val(_motivoForCallback);
                            $("#cboCERetardoMotivos").trigger("change", ["esEditar"]);

                            let events = $('#calendar').fullCalendar('clientEvents');

                            events.forEach(e => {
                                $('#calendar').fullCalendar('removeEvents', [e._id]);
                            });

                            if (!_esAdmn) {

                                let bottomLimit = null;
                                let topLimit = null;

                                if (cboCETipoRetardo.val() == 0) {
                                    bottomLimit = moment().add(-3, 'd');
                                    topLimit = moment().add(2, 'M');
                                } else {
                                    bottomLimit = moment().add(-3, 'd');
                                    topLimit = moment().add(2, 'M');
                                }

                                $('#calendar').fullCalendar('addEventSource', [{
                                    title: 'Rango',
                                    start: bottomLimit.format("YYYY-MM-DD"),
                                    end: topLimit.format("YYYY-MM-DD"),
                                    color: '#BEBEBE',
                                    rendering: "inverse-background",
                                    classNames: ['Rango']
                                }]);
                            }

                            let date = moment(rowData.diaTomado);

                            $('#calendar').fullCalendar('addEventSource', [{
                                title: $("#cboCETipoRetardo option:selected").text().split(' ')[0],
                                start: date,
                                classNames: ['Vacaciones']
                            }]);

                            $('#calendar').fullCalendar('gotoDate', moment(date));

                        });

                        notClickablePer = true;
                        notClickableEmp = true;

                        txtCalendarioNumDias.text(0);

                        if (esUsuarioReg) {
                            txtCEVacacionNombreEmp.prop("readonly", true);

                        }

                        if (rowData.estado == 1 || rowData.estado == 2) {
                            btnCEVacacionActualizar.hide();
                        } else {
                            btnCEVacacionActualizar.show();

                        }

                        divArchivoActa.css('display', 'none');
                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.eliminarVacacion', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar las ausencias seleccionadas?', 'Confirmar', 'Cancelar', () => fncEliminarVacacion(rowData.id));
                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.notificarVacacion', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('', '¿Desea notificar al responsable?', 'Confirmar', 'Cancelar', () => fncSetNotificada(rowData.id));
                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.reporteVacacion', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        fncGetReporte(rowData.id);
                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.verFirmas', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        // fncGetReporte(rowData.id, rowData.tipoVacaciones);

                        dtFirmas.clear();
                        dtFirmas.rows.add(rowData.lstAutorizantes);
                        dtFirmas.draw();

                        mdlFirmas.modal("show");
                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.verJustificacion', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();

                        mdlJustific.modal("show");
                        txtVacacionJustific.val(rowData.justificacion);
                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.botonDescargarArchivoActa', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();

                        location.href = `DescargarArchivoRetardo?id=${rowData.id}`;
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { "width": "10%", "targets": 6 },
                    { "width": "20%", "targets": 5 },
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
            if (objFiltro != "") {
                axios.post("GetRetardos", objFiltro).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {

                        if (response.data.esAdmnCH) {
                            fncSetPeriodos(true);
                            _esAdmn = true;
                        } else {
                            fncSetPeriodos(false);
                            _esAdmn = false;

                        }

                        if (response.data.claveEmpleado != "" && response.data.claveEmpleado != null) {
                            esUsuarioReg = true;
                            cveUsuarioReg = response.data.claveEmpleado;

                            //#region REMOVER PARA USUARIO SIN ACCESO A LA VISTA
                            // $('#cboCETipoPermiso option[value=10]').remove();
                            // $('#cboCETipoPermiso option[value=11]').remove();
                            //#endregion
                        }

                        dtRH_Vacaciones_Vacaciones.clear();
                        dtRH_Vacaciones_Vacaciones.rows.add(items);
                        dtRH_Vacaciones_Vacaciones.draw();

                        if (response.data.esRegCH == null) {
                            // $('#cboCETipoPermiso option[value=13]').remove();

                        }
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCrearEditarVacaciones(esSobreEscribir) {
            let objVacacion = fncVacacionParametros();
            objVacacion.lstAutorizantes = fncGetAutorizantes();

            let listaFechas = fncGetFechasCalendiario(objVacacion.tipoVacaciones);
            objVacacion.diaTomado = listaFechas[0];

            //Si el tipo de permiso es por defunción, matrimonio o paternidad se debe capturar el campo de capital humano.
            if (inputArchivoActa.val() == '' && objVacacion.id == 0) {
                Alert2Warning('Debe anexar un archivo de justificante.');
                return;
            }

            if (objVacacion != "" && objVacacion.lstAutorizantes != "" && listaFechas.length > 0) {
                axios.post("CrearEditarRetardo", objVacacion).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        if (objVacacion.id == 0) {
                            guardarArchivoActa(response.data.idRetardo);
                            Alert2Exito("Registo guardado.");
                        } else {

                            Alert2Exito("Registo Actualizado.");
                        }
                        fncGetVacaciones();
                    } else {
                        Alert2Warning("Ocurrio algo mal con el guardado del registro, favor de contactarse con el departamento de TI");
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Favor de llenar todos los campos")
            }

            // if (objVacacion != "" && objVacacion.lstAutorizantes != "" && listaFechas.length > 0) {
            //     axios.post("CrearEditarRetardos", objVacacion).then(response => {
            //         let { success, items, message } = response.data;
            //         if (success) {
            //             // fncCEFechas(items.id, items.tipoVacaciones, esSobreEscribir);
            //             fncGetVacaciones();
            //             mdlVacacion.modal("hide");
            //         } else {
            //             Alert2Error(message);
            //         }
            //     }).catch(error => Alert2Error(error.message));
            // } else {
            //     Alert2Warning("Favor de capturar las ausencias");
            // }
        }

        function fncGetAutorizantes() {
            let notificantes = [];

            // if (txtCEVacacionNombreJefeInmediato.val() != "" && txtCEVacacionNombreJefeInmediato.val() != "--Seleccione--") {
            //     notificantes.push({
            //         idUsuario: txtCEVacacionNombreJefeInmediato.val(),
            //         orden: 0
            //     });

            // } else {
            //     Alert2Warning("Seleccione un Jefe Inmediado");
            //     return "";
            // }
            notificantes.push(({
              //  idUsuario: 1019, // DIANA ALVAREZ
                orden: 5
            }));

            if (txtCEVacacionNombreResponsable.val() != "" && txtCEVacacionNombreResponsable.val() != "--Seleccione--") {
                notificantes.push({
                    idUsuario: txtCEVacacionNombreResponsable.val(),
                    orden: 1
                });

            } else {
                Alert2Warning("Seleccione un Responsable de CC");
                return "";
            }

            return notificantes;
        }
        let idUsuarioResponsable = 0;
        function fncSetAutorizantes(lstAutorizantes, claveEmpleado) {
            for (const item of lstAutorizantes) {
                switch (item.orden) {
                    case 0:
                        txtCEVacacionNombreJefeInmediato.val(item.idUsuario);
                        txtCEVacacionNombreJefeInmediato.trigger("change");
                        break;
                    case 1:
                        idUsuarioResponsable = item.idUsuario;
                        txtCEVacacionNombreResponsable.fillCombo('/Administrativo/Vacaciones/FillComboAutorizantes', { clave_empleado: claveEmpleado }, false, null, () => {
                            txtCEVacacionNombreResponsable.val(idUsuarioResponsable);
                            // txtCEVacacionClaveResponsable.trigger("change");
                        });
                        break;
                    // case 5:
                    //     selectCapitalHumano.val(item.idUsuario);
                    //     selectCapitalHumano.change();
                    //     break;
                    default:
                        break;
                }
            }
        }

        function fncEliminarVacacion(idReg) {
            axios.post("RemoveRetardo", { idRetardo: idReg }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Ausencias Eliminadas.");
                    fncGetVacaciones();

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetFechasCalendiario(tipoVacaciones) {
            let events = $('#calendar').fullCalendar('clientEvents');
            let listaFechas = [];
            let dias = 0;

            if (tipoVacaciones == 4) {
                listaFechas.push(dateCELactanciaInicio.val());
                listaFechas.push(dateCELactanciaFin.val());
            } else {
                if (events.length > 0) {

                    for (let i = 0; i < events.length; i++) {

                        if (events[i].classNames[0] == "Vacaciones") {
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
            }

            return listaFechas;

        }

        function guardarArchivoActa(idRetardo) {
            const data = new FormData();

            data.append('archivoActa', inputArchivoActa[0].files[0]);
            data.append('idRetardo', idRetardo);

            $.ajax({
                url: 'GuardarArchivoRetardo',
                data,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                if (response.success) {
                    Alert2Exito("Ausencias Guardadas.");
                    fncGetVacaciones();
                    mdlVacacion.modal("hide");
                } else {
                    //Función para eliminar registro principal y detalle.
                    // axios.post("EliminarVacacion", { id: vacacion_id }).then(response => {
                    //     let { success, items, message } = response.data;
                    //     if (success) {
                    //         fncGetVacaciones();
                    //     }
                    // }).catch(error => Alert2Error(error.message));
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        function fncGetDatosPersonal(claveUsuario, nombreUsuario, esGerente, esGerentePagadas, esActualizar = false) {
            let obj = new Object();
            obj = {
                claveEmpleado: claveUsuario,
                nombre: nombreUsuario
            }
            axios.post("GetDatosPersona", obj).then(response => {
                let { success, items, message } = response.data;
                if (response.data.objDatosPersona != null) {
                    if (success) {
                        if (esGerente) {
                            if (esGerentePagadas) {
                                txtCEVacacionNombreResponsablePagadas.val(response.data.objDatosPersona.nombreCompleto);
                            } else {
                                txtCEVacacionNombreResponsable.val(response.data.objDatosPersona.nombreCompleto);
                            }
                        } else {
                            txtCEVacacionClaveEmp.val(claveUsuario);
                            txtCEVacacionNombreEmp.val(response.data.objDatosPersona.nombreCompleto);
                            txtCEVacacionFechaIngreso.val(moment(response.data.objDatosPersona.fechaAlta).format("DD/MM/YYYY"));
                            txtCEVacacionPuesto.val(response.data.objDatosPersona.nombrePuesto);

                            if (!esActualizar) {
                                txtCEVacacionCC.val(response.data.objDatosPersona.cc);

                            }
                        }

                    }
                    else {
                        Alert2Error(message);
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
            var path = `/Reportes/Vista.aspx?idReporte=295&idVac=${idReg}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        //#endregion

        //#region FNC GRALES

        function funGetEmpleado(event, ui) {
            txtCEVacacionClaveEmp.val(ui.item.id);
            txtCEVacacionNombreEmp.val(ui.item.value);

            if (txtCEVacacionClaveEmp.val() != "") {
                txtCEVacacionNombreResponsable.fillCombo('/Administrativo/Vacaciones/FillComboAutorizantes', { clave_empleado: txtCEVacacionClaveEmp.val() }, false);
                fncGetDatosPersonal(txtCEVacacionClaveEmp.val(), txtCEVacacionNombreEmp.val(), false, false);

                if (_esConsulta == "1") {
                    if (txtCEVacacionClaveEmp.val() == cveUsuarioReg) {
                        btnCEVacacionActualizar.show();

                    } else {
                        btnCEVacacionActualizar.hide();

                    }
                }
            }

            txtCEVacacionNombreEmp.trigger("change");

        }

        function fncGetFiltros() {
            let dateIni = moment(dateFiltroIni.val());
            let dateFin = moment(dateFiltroFin.val());
            let objFiltro = {};

            if (dateFiltroIni.val() != "" && dateFiltroFin.val() != "") {
                if (dateIni._d > dateFin._d) {
                    Alert2Warning("Ingrese una fecha valida");
                    return "";
                }

                objFiltro = {
                    estado: cboFiltroEstado.val(),
                    idPeriodo: null,
                    ccEmpleado: cboVacacionCC.val(),
                    tipoRetardo: cboTipoPermiso.val(),
                    fechaFiltroInicio: dateIni.format("DD/MM/YYYY"),
                    fechaFiltroFin: dateFin.format("DD/MM/YYYY"),
                }
            } else {
                objFiltro = {
                    estado: cboFiltroEstado.val(),
                    idPeriodo: null,
                    ccEmpleado: cboVacacionCC.val(),
                    tipoRetardo: cboTipoPermiso.val(),
                    fechaFiltroInicio: null,
                    fechaFiltroFin: null,
                }
            }

            return objFiltro;
        }

        function fncVacacionParametros() {
            fncDefaultBorder();
            let strMessage = "";
            if (txtCEVacacionClaveEmp.val() == "") { txtCEVacacionClaveEmp.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (txtCEVacacionNombreEmp.val() == "") { txtCEVacacionNombreEmp.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            // if (txtCEVacacionClaveResponsable.val() == "") { txtCEVacacionClaveResponsable.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (txtCEVacacionNombreResponsable.val() == "") { txtCEVacacionNombreResponsable.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (txtCEVacacionNombreJefeInmediato.val() == "") { $("#select2-txtCEVacacionNombreJefeInmediato-container").css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            // if (cboCETipoPermiso.val() == "") { cboCETipoPermiso.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (cboCERetardoMotivos.val() == "") { cboCERetardoMotivos.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (cboCETipoRetardo.val() == "") { cboCETipoRetardo.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (dateCEDiaTomado.val() == "") { dateCEDiaTomado.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (cboCETipoRetardo.val() != "") {
                if (cboCETipoRetardo.val() == "0") {
                    if (cboCEHorario.val() == "") { cboCEHorario.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }

                } else {
                    if (txtCEHorarioLower.val() == "") { txtCEHorarioLower.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
                    if (txtCEHorarioUpper.val() == "") { txtCEHorarioUpper.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }

                }
            }

            if (txtCEHorasRequeridas.val() == "") { txtCEHorasRequeridas.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (txtCEMinutosRequeridos.val() == "") { txtCEMinutosRequeridos.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }

            if (strMessage != "") {
                Alert2Warning(strMessage);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEVacacionActualizar.attr("data-id"),
                    estado: changeStatus ?? 3,
                    nombreEmpleado: txtCEVacacionNombreEmp.val(),
                    claveEmpleado: txtCEVacacionClaveEmp.val(),
                    tipoRetardo: cboCETipoRetardo.val(),
                    motivoJustificacion: cboCERetardoMotivos.val(),
                    justificacion: txtCEVacacionJustific.val(),
                    horario: cboCETipoRetardo.val() == "0" ? cboCEHorario.val() : null,
                    horarioLower: cboCETipoRetardo.val() == "1" ? txtCEHorarioLower.val() : 0,
                    horarioUpper: cboCETipoRetardo.val() == "1" ? txtCEHorarioUpper.val() : 0,
                    tiempoRequeridoHrs: txtCEHorasRequeridas.val(),
                    tiempoRequeridoMin: txtCEMinutosRequeridos.val(),
                    idJefeInmediato: txtCEVacacionNombreJefeInmediato.val(),
                    nombreJefeInmediato: $("#txtCEVacacionNombreJefeInmediato option:selected").text(),
                }

                return obj;
            }
        }

        function fncDefaultBorder() {
            txtCEVacacionClaveEmp.css("border", "1px solid #CCC");
            $("#select2-txtCEVacacionNombreJefeInmediato-container").css('border', '1px solid #CCC');
            txtCEVacacionNombreResponsable.css("border", "1px solid #CCC");
            txtCEVacacionClaveResponsable.css("border", "1px solid #CCC");
            txtCEVacacionNombreEmp.css("border", "1px solid #CCC");

            cboCERetardoMotivos.css("border", "1px solid #CCC");
            cboCETipoRetardo.css("border", "1px solid #CCC");
            dateCEDiaTomado.css("border", "1px solid #CCC");
            cboCEHorario.css("border", "1px solid #CCC");
            txtCEHorarioLower.css("border", "1px solid #CCC");
            txtCEHorarioUpper.css("border", "1px solid #CCC");
            txtCEHorasRequeridas.css("border", "1px solid #CCC");
            txtCEMinutosRequeridos.css("border", "1px solid #CCC");
            // cboCETipoPermiso.css("border", "1px solid #CCC");

        }

        function fncEmptyFields() {
            txtCEVacacionNombreEmp.val("");
            txtCEVacacionClaveEmp.val("");
            txtCEVacacionClaveResponsable.val("");
            txtCEVacacionNombreResponsable.val("");
            txtCEVacacionClaveResponsablePagadas.val("");
            txtCEVacacionNombreResponsablePagadas.val("");
            txtCalendarioNumDias.text(0).trigger("change");
            dateCELactanciaInicio.val(moment().format("DD/MM/YYYY"));
            dateCELactanciaInicio.trigger("change");
            // cboCETipoPermiso.val("");
            // cboCETipoPermiso.trigger("change");
            txtCEVacacionCC.val("");
            txtCEVacacionFechaIngreso.val("");
            txtCEVacacionNombreJefeInmediato.val("");
            txtCEVacacionNombreJefeInmediato.trigger("change");
            txtCEVacacionNumJefeInmediato.val("");
            txtCEVacacionPuesto.val("");
            txtCEVacacionJustific.val("");

            cboCERetardoMotivos.val("");;
            cboCERetardoMotivos.trigger("change");
            cboCETipoRetardo.val("");
            cboCETipoRetardo.trigger("change");
            dateCEDiaTomado.val("");
            cboCEHorario.val("");
            cboCEHorario.trigger("change");
            txtCEHorarioLower.val("");
            txtCEHorarioUpper.val("");
            txtCEHorasRequeridas.val("");
            txtCEMinutosRequeridos.val("");

            let events = $('#calendar').fullCalendar('clientEvents');

            if (events.length != 0) {
                // cboCEVacacionPeriodo[0].selectedIndex = 0;
                // cboCEVacacionPeriodo.trigger("change");
            } else {
                $('#calendar').fullCalendar('removeEvents');
            }

            inputArchivoActa.val('');
            inputArchivoActa.change();

        }

        function fncSetPeriodos(esAdmn) {
            if (esAdmn) {
                let events = $('#calendar').fullCalendar('clientEvents');

                events.forEach(e => {
                    $('#calendar').fullCalendar('removeEvents', [e._id]);
                });

                $('#calendar').fullCalendar('gotoDate', moment());

            } else {
                let anteantier = moment().add(-3, 'd');
                let tresDias = moment().add(3, 'd');

                let events = $('#calendar').fullCalendar('clientEvents');

                events.forEach(e => {
                    if (e.title != "Vacaciones" || (e.classNames != undefined && e.classNames[0] != "VacacionesCapturadas")) {
                        $('#calendar').fullCalendar('removeEvents', [e._id]);
                    }
                });

                $('#calendar').fullCalendar('addEventSource', [{
                    title: 'Rango',
                    start: anteantier.format("YYYY-MM-DD"),
                    end: tresDias.format("YYYY-MM-DD"),
                    color: '#BEBEBE',
                    rendering: "inverse-background",
                    classNames: ['Rango']
                }]);

                $('#calendar').fullCalendar('gotoDate', anteantier);
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
                            // if (txtCEVacacionClaveEmp.val() != "") {
                            //     fncGetDiasDispPermisos();

                            // }
                            //fncGetNumDias();
                            //txtCalendarioNumDias.text(fncGetNumDias(btnCalendarioGuardar.attr("data-id")));
                            Alert2AccionConfirmar('¡Cuidado!', '¿Desea quitar TODOS los permisos capturados?', 'Confirmar', 'Cancelar',
                                () => {
                                    // fncGetFechas(txtCEVacacionClaveEmp.val(), cboCETipoPermiso.val());
                                    events.forEach(e => {
                                        if (e.title != "Rango") {
                                            $('#calendar').fullCalendar('removeEvents', [e._id]);
                                        }
                                    })
                                }

                            );

                        }
                    }
                },
                header: {
                    left: 'prev,next today miboton, btnCalendarioLimpiar',
                    center: 'title',
                    //right: 'month,agendaWeek,agendaDay,listWeek'
                    //right: 'listYear,month,agendaWeek,agendaDay'
                    right: 'month'
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
                editable: false,
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
                    if (calEvent.classNames == undefined || calEvent.classNames[0] != "VacacionesCapturadas") {
                        if (calEvent.end != null) {
                            range = calEvent.end.diff(calEvent.start, "days");
                        } else {
                            range = 1;
                        }

                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el rango seleccionado?', 'Confirmar', 'Cancelar',
                            function () {

                                txtCalendarioNumDias.text(Number(txtCalendarioNumDias.text()) + range).trigger("change");
                                $('#calendar').fullCalendar('removeEvents', [calEvent._id]);
                            }

                        );
                    }
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
                    console.log(notClickablePer)
                    console.log(notClickableEmp)
                    console.log(notClickableDays)
                    if (notClickableEmp && notClickableDays) {
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


                                if (events[i].title != "Rango") {
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
                                    if (addToday) {
                                        if (startStr._d < events[i].start._d) {
                                            addRange = false;

                                        } else if (endStr._d > events[i].end._d) {
                                            addRange = false;

                                        }
                                    }
                                    // if (events[i].classNames == undefined || events[i].classNames[0] != "VacacionesCapturadas") {
                                    //     // LOS EVENTOS (DIAS DE VACACIONES) NECESITAN ESTAR DENTRO DEL RANGO DEL PERIODO DE LAS VACACIONES              

                                    // } else {
                                    //     if (startStr.format("YYYY-MM-DD") == events[i].start.format("YYYY-MM-DD")) {
                                    //         addVac = false;
                                    //     }
                                    // }
                                }
                            }
                        }

                        if (addVac && addRange && addToday && addDays) {
                            txtCalendarioNumDias.text(Number(txtCalendarioNumDias.text()) - range).trigger("change");
                            // let eventNum = cboCETipoVacaciones.val();
                            // let eventName = $(`#cboCETipoVacaciones option[value='${eventNum}']`).text();
                            // // if (eventName == '' || eventName == undefined) {
                            // //     if (eventNum == 0) {
                            // //         eventNum = cboCETipoPermiso.val();
                            // //         eventName = $(`#cboCETipoPermiso option[value='${eventNum}']`).text();
                            // //     }
                            // // }

                            // if (eventNum == 0) {

                            // }

                            let eventName = $(`#cboCETipoRetardo option:selected`).text().split(' ')[0];

                            $('#calendar').fullCalendar('addEventSource', [{
                                title: eventName,
                                start: startStr,
                                end: endStr,
                                classNames: ['Vacaciones']
                            }]);

                            dateCEDiaTomado.val(startStr.format("YYYY-MM-DD"));

                        }
                        if (!addDays) {
                            Alert2Warning("Numero de día(s) mayor a los disponibles");
                        } else {
                            if (!addToday) {
                                Alert2Warning(`Periodo disponible de ${dateMinus2Month.format("DD/MM/YYYY")} al ${rangeStartDate.format("DD/MM/YYYY")}`);
                            }
                            if (!addRange) {
                                Alert2Warning("Controles de ausencia fuera de rango del periodo asignado");
                            }
                            if (!addVac) {
                                Alert2Warning("Día(s) Invalido(s)");
                            }
                        }
                    }
                }

            });

        }

        function fncFillCallendar(lstFechas, lstFechasCapturadas, idEmpleado, tipoVacacion) {

            let vacacionesDesc = "";
            switch (tipoVacacion) {
                case "0":
                    vacacionesDesc = "Permiso por paternidad (PGS)"
                    break;
                case "1":
                    vacacionesDesc = "Permiso por matrimonio (PGS)"

                    break;
                case "2":
                    vacacionesDesc = "Permiso sindical (PGS)"

                    break;
                case "3":
                    vacacionesDesc = "Permiso por deceso familiar (PGS)"

                    break;
                case "4":
                    vacacionesDesc = "Permiso por lactancia (PGS)"

                    break;
                case "5":
                    vacacionesDesc = "Permiso medico (PGS)"

                    break;
                case "6":
                    vacacionesDesc = ""

                    break;
                case "7":
                    vacacionesDesc = "Vacaciones (PGS)"

                    break;
                case "8":
                    vacacionesDesc = "Permiso SIN goce de sueldo (PS)"

                    break;
                case "9":
                    vacacionesDesc = "Comision de trabajo (CT)"

                    break;
                case "10":
                    vacacionesDesc = "Home office (PGS)"

                    break;
                case "11":
                    vacacionesDesc = "Tiempo por tiempo (PGS)"

                    break;
                case "13":
                    vacacionesDesc = "Suspención (SUSP)"

                    break;
                default:
                    break;
            }

            let events = $('#calendar').fullCalendar('clientEvents');
            events.forEach(e => {
                if (e.title == "Vacaciones" || (e.classNames != undefined && e.classNames[0] == "VacacionesCapturadas")) {
                    $('#calendar').fullCalendar('removeEvents', [e._id]);
                }

            });

            //OTRAS VACACIONES
            if (lstFechasCapturadas.length > 0) {

                lstFechasCapturadas.forEach(item => {
                    let date = moment(item.fecha);
                    $('#calendar').fullCalendar('addEventSource', [{
                        title: vacacionesDesc,
                        start: date,
                        color: '#BEBEBE',
                        classNames: ['VacacionesCapturadas']
                    }]);
                });
                $('#calendar').fullCalendar('gotoDate', moment(lstFechasCapturadas[0].fecha));

            }

            if (lstFechas.length > 0) {
                lstFechas.forEach(item => {
                    //txtCalendarioNumDias.text(Number(txtCalendarioNumDias.text())-1);
                    let date = moment(item.fecha);
                    // let eventName = $(`#cboCETipoPermiso option[value='${tipoVacacion}']`).text();
                    // if (eventName == '' || eventName == undefined) {
                    //     eventName = $(`#cboCETipoVacaciones option[value='${tipoVacacion}']`).text();
                    // }
                    $('#calendar').fullCalendar('addEventSource', [{
                        title: vacacionesDesc,
                        start: date,
                        classNames: ['Vacaciones']
                    }]);
                });
                $('#calendar').fullCalendar('gotoDate', moment(lstFechas[0].fecha));

            }

            // let obj = {
            //     cc: null,
            //     claveEmpleado: idEmpleado
            // }

            // axios.post("GetResponsables", obj).then(response => {
            //     let { success, data, message } = response.data;
            //     if (success) {
            //         txtCalendarioNumDias.text(data[0].diasPendientes);
            //     }
            // }).catch(error => {});
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Retardos = new Retardos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();