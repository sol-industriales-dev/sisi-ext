(() => {
    $.namespace('CH.Permisos');

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
    const cboCETipoPermiso = $('#cboCETipoPermiso');
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
    let dtFirmas
    const tblFirmas = $('#tblFirmas')
    const mdlFirmas = $('#mdlFirmas')
    const mdlComentario = $('#mdlComentario')
    const txtComentario = $('#txtComentario')
    let estatusEnum = ["-", "AUTORIZADO", "-", "-"]
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

    //ES ADMIN
    let _esAdmin = false;

    Permisos = function () {
        (function init() {

            initCalendar();
            fncSetPeriodos(false);
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
                titleCEVacacion.text("Crear Ausencias");
                btnTxtCEVacacion.text("Guardar");
                btnTxtCEVacacion.prop("disabled", true);
                btnCEVacacionActualizar.attr("data-id", 0);
                tooltipCEVacaciones.attr("Title", "Crear");
                notClickablePer = false;
                notClickableEmp = false;
                cboCETipoPermiso.attr("disabled", false);
                // cboCETipoVacaciones.attr("disabled", false);

                // txtCEVacacionClaveEmp.attr("disabled", false);
                txtCEVacacionNombreEmp.attr("disabled", false);
                txtCEVacacionNombreEmp.focus();

                // cboCETipoVacaciones[0].selectedIndex = 0;
                // cboCETipoVacaciones.trigger("change");

                cboCETipoPermiso.show();

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

            // txtCEVacacionClaveEmp.on("change", function () {
            //     let events = $('#calendar').fullCalendar('clientEvents');
            //     events.forEach(e => {
            //         if (e.classNames[0] != "Rango") {
            //             $('#calendar').fullCalendar('removeEvents', [e._id]);
            //         }
            //     })

            //     fncGetDatosPersonal(txtCEVacacionClaveEmp.val(), txtCEVacacionNombreEmp.val(), false, false);
            //     fncGetFechas(txtCEVacacionClaveEmp.val(), cboCETipoPermiso.val());
            //     // fncGetResponsable(txtCEVacacionClaveEmp.val());
            // });

            cboCETipoPermiso.select2({ width: "100%" });

            // txtCEVacacionNombreResponsable.getAutocomplete(funGetGerente, null, '/Administrativo/BajasPersonalEntrevista/getEmpleadosGeneral');
            txtCEVacacionNombreEmp.getAutocomplete(funGetEmpleado, null, '/Administrativo/FormatoCambio/getCatEmpleadosReclutamientos');

            // txtCEVacacionNombreEmp.on("change", function () {
            //     if ($(this).val() != "" && cboCETipoPermiso.val() > 0) {
            //         cboCETipoPermiso.trigger("change");
            //     }
            //     //fncGetDatosPersonal(txtCEVacacionClaveEmp.val(),txtCEVacacionNombreEmp.val(),false,false);
            // });

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

            // cboCEVacacionPeriodo.on("change", function(){

            //     if(cboCEVacacionPeriodo.val()!=""){
            //         fncGetPeriodos(cboCEVacacionPeriodo.val());

            //     }else{
            //         $('#calendar').fullCalendar('removeEvents');

            //     }
            //     btnTxtCEVacacion.prop("disabled", false);

            // });

            chkTipoVacaciones.on("change", function () {
                if (chkTipoVacaciones.prop("checked")) {
                    groupCEVacacionClaveResponsablePagadas.show();
                    groupCEVacacionNombreResponsablePagadas.show();
                } else {
                    groupCEVacacionClaveResponsablePagadas.hide();
                    groupCEVacacionNombreResponsablePagadas.hide();
                }
            });

            cboCETipoPermiso.on("change", function () {
                if ($(this).val() >= 0 && txtCEVacacionNombreEmp.val() != "") {
                    if (cboCETipoPermiso.val() == "4") {
                        divCELactancia.css("display", "initial");
                        divCECalendario.css("display", "none");
                    } else {
                        if ($(this).val() == "5") {
                            divCEVacacionesNombreMedico.css("display", "initial")
                        } else {
                            divCEVacacionesNombreMedico.css("display", "none")
                        }

                        //Si el tipo de permiso es por defunción, matrimonio o paternidad se muestra el div de capital humano.
                        if (['0', '1', '3', '5'].includes($(this).val())) {
                            if ($(this).val() == "5") {
                                divCapitalHumano.css('display', 'none');
                                divArchivoActa.css('display', 'initial');
                            } else {
                                divCapitalHumano.css('display', 'initial');
                                divArchivoActa.css('display', 'initial');
                            }

                        } else {
                            divCapitalHumano.css('display', 'none');
                            divArchivoActa.css('display', 'none');
                        }

                        divCELactancia.css("display", "none");
                        divCECalendario.css("display", "initial");
                        if (txtCEVacacionClaveEmp.val() != "" && cboCETipoPermiso.val()) {
                            fncGetFechas(txtCEVacacionClaveEmp.val(), cboCETipoPermiso.val());
                        }
                    }
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
        }

        //#region TBL

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
                        render: function (data, type, row) {
                            // let bthNotificar = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Notificar"><button class="btn ${row.notificada ? "btn-success" : "btn-warning"} notificarVacacion btn-xs"  ${row.notificada ? "disabled" : ""}><i class="fas fa-envelope"></i></button>&nbsp;</span>`;
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

                            if (!_esAdmin) {
                                if (row.estado == 1 || row.estado == 2) {
                                    btns = btnJustificacion + btnFirmas + btnDownload + (row.rutaArchivoActa != null ? btnArchivoActa : '') + btnActualizar;
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
                        titleCEVacacion.text("Actualizar Permiso");
                        btnTxtCEVacacion.text("Actualizar");
                        txtCEVacacionNombreEmp.val(rowData.nombreEmpleado);
                        txtCEVacacionClaveEmp.val(rowData.claveEmpleado);
                        txtCEVacacionNombreResponsable.val(rowData.claveResponsable);
                        txtCEVacacionNombreResponsable.trigger("change");
                        txtCEVacacionClaveResponsable.val(rowData.claveResponsable);
                        cboCETipoPermiso.attr("disabled", true);
                        // cboCETipoVacaciones.attr("disabled", true);
                        // txtCEVacacionClaveEmp.attr("disabled", true);
                        txtCEVacacionNombreEmp.attr("disabled", true);
                        txtCEVacacionJustific.val(rowData.justificacion);

                        cboCETipoPermiso.val(rowData.tipoVacaciones);
                        cboCETipoPermiso.change();

                        txtCEVacacionCC.val(rowData.ccDesc);

                        txtCEVacacionNombreJefeInmediato.val(rowData.idJefeInmediato);
                        txtCEVacacionNombreJefeInmediato.trigger("change");

                        fncSetAutorizantes(rowData.lstAutorizantes, rowData.claveEmpleado);

                        fncGetDatosPersonal(rowData.claveEmpleado, rowData.nombreEmpleado, false, false, true);

                        // if (rowData.tipoVacaciones != 9 && rowData.tipoVacaciones != 8) {

                        //     // cboCETipoPermiso.trigger("change", ['noConsultar']);
                        //     cboCETipoVacaciones.val(0);
                        //     cboCETipoVacaciones.change();
                        // } else {
                        //     cboCETipoVacaciones.val(rowData.tipoVacaciones);
                        //     cboCETipoVacaciones.change();
                        // }

                        // cboCEVacacionPeriodo.val(rowData.idPeriodo);
                        // cboCEVacacionPeriodo.change();
                        // fncGetFechas(rowData.claveEmpleado, rowData.tipoVacaciones);
                        // fncGetDiasDispPermisos();
                        notClickablePer = true;
                        notClickableEmp = true;

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
                        fncGetReporte(rowData.id, rowData.tipoVacaciones);
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

                        location.href = `DescargarArchivoActa?id=${rowData.id}`;
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
                axios.post("GetVacaciones", objFiltro).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {

                        if (response.data.esAdmnCH) {
                            fncSetPeriodos(true);
                            _esAdmin = true;
                        }

                        if (response.data.claveEmpleado != "" && response.data.claveEmpleado != null) {
                            esUsuarioReg = true;
                            cveUsuarioReg = response.data.claveEmpleado;

                            //#region REMOVER PARA USUARIO SIN ACCESO A LA VISTA
                            $('#cboCETipoPermiso option[value=10]').remove();
                            $('#cboCETipoPermiso option[value=11]').remove();
                            //#endregion
                        }

                        if (response.data.esRegCH == null) {
                            $('#cboCETipoPermiso option[value=13]').remove();

                        }

                        dtRH_Vacaciones_Vacaciones.clear();
                        dtRH_Vacaciones_Vacaciones.rows.add(items);
                        dtRH_Vacaciones_Vacaciones.draw();

                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCrearEditarVacaciones(esSobreEscribir) {
            let objVacacion = fncVacacionParametros();
            objVacacion.lstAutorizantes = fncGetAutorizantes();

            let listaFechas = fncGetFechasCalendiario(objVacacion.tipoVacaciones);

            //Si el tipo de permiso es por defunción, matrimonio o paternidad se debe capturar el campo de capital humano.
            if (objVacacion.id == 0) {
                if (objVacacion.tipoVacaciones == "5" && inputArchivoActa.val() == '') {

                    Alert2Warning('Debe anexar evidencia para el permiso medico.');
                    return;
                } else {
                    if (['0', '1', '3'].includes(objVacacion.tipoVacaciones) && (objVacacion.capitalHumano == 0 || inputArchivoActa.val() == '')) {
                        Alert2Warning('Debe capturar el campo de "capital humano" y anexar un archivo de acta para los permisos de paternidad, matrimonio o fallecimiento.');

                        return;
                    }
                }

            }

            if (objVacacion != "" && objVacacion.lstAutorizantes != "" && listaFechas.length > 0) {
                axios.post("CrearEditarVacaciones", objVacacion).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncCEFechas(items.id, items.tipoVacaciones, esSobreEscribir);
                        fncGetVacaciones();
                        mdlVacacion.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Favor de capturar todos los campos");
            }
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

            if (txtCEVacacionNombreResponsable.val() != "" && txtCEVacacionNombreResponsable.val() != "--Seleccione--") {
                notificantes.push({
                    idUsuario: txtCEVacacionNombreResponsable.val(),
                    orden: 1
                });

            } else {
                Alert2Warning("Seleccione un Responsable de CC");
                return "";
            }

            if (cboCETipoPermiso.val() == 5) {
                if (txtCEVacacionNombreMedico.val() != "" && txtCEVacacionNombreMedico.val() != "--Seleccione--") {
                    notificantes.unshift({
                        idUsuario: txtCEVacacionNombreMedico.val(),
                        orden: 4
                    });

                } else {
                    Alert2Warning("Seleccione un Medico");
                    return "";
                }
            }

            if (['0', '1', '3'].includes(cboCETipoPermiso.val())) {
                notificantes.unshift({
                    idUsuario: selectCapitalHumano.val(),
                    orden: 5
                });
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
                    case 5:
                        selectCapitalHumano.val(item.idUsuario);
                        selectCapitalHumano.change();
                        break;
                    default:
                        break;
                }
            }
        }

        function fncEliminarVacacion(idReg) {
            axios.post("EliminarVacacion", { id: idReg }).then(response => {
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

        function fncCEFechas(idReg, tipoVacaciones, esSobreEscribir) {
            let listaFechas = fncGetFechasCalendiario(tipoVacaciones);
            let esEditar = false;

            if (btnCEVacacionActualizar.attr("data-id") > 0) {
                esEditar = true;
            }

            let tipoIncidencia = 4;

            if (cboCETipoPermiso.val() == '8') {
                tipoIncidencia = 3;

            } else if (cboCETipoPermiso.val() == '9') {
                tipoIncidencia = 12;

            } else if (cboCETipoPermiso.val() == '13') {
                tipoIncidencia = 6;

            }

            axios.post("CrearEditarFechas", { idVacacion: idReg, lstFechas: listaFechas, diasPermitidos: txtCalendarioNumDias.text(), tipoPermiso: tipoIncidencia, esSobreEscribir: esSobreEscribir, esEditar: esEditar }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (!esEditar && inputArchivoActa[0].files.length > 0) {
                        guardarArchivoActa(idReg); // Alert2Exito("Ausencias Guardadas.");
                    } else {
                        Alert2Exito("Ausencias Guardadas.");
                    }
                } else {
                    if (response.data.esRecursar) {
                        Alert2AccionConfirmar('¡Cuidado!', message, 'Confirmar', 'Cancelar', () => fncCEFechas(idReg, tipoVacaciones, true));
                    }
                    else {
                        if (!esEditar) {
                            axios.post("EliminarVacacion", { id: idReg }).then(response => {
                                let { success, items, message } = response.data;
                                if (success) {
                                    fncGetVacaciones();
                                }
                            }).catch(error => Alert2Error(error.message));
                        }
                        Alert2Warning(message);
                    }
                }
            }).catch(error => Alert2Error(error.message));
        }

        function guardarArchivoActa(vacacion_id) {
            const data = new FormData();

            data.append('archivoActa', inputArchivoActa[0].files[0]);
            data.append('vacacion_id', vacacion_id);

            $.ajax({
                url: 'GuardarArchivoActa',
                data,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                if (response.success) {
                    Alert2Exito("Ausencias Guardadas.");
                } else {
                    //Función para eliminar registro principal y detalle.
                    axios.post("EliminarVacacion", { id: vacacion_id }).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            fncGetVacaciones();
                        }
                    }).catch(error => Alert2Error(error.message));
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

        function fncGetFechas(idEmpleado, tipoVacaciones) {
            // fncGetNumDias();

            let obj = {
                idReg: btnCEVacacionActualizar.attr("data-id"),
                tipoPermiso: tipoVacaciones,
                clave_empleado: idEmpleado
            }

            axios.post("GetFechas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (tipoVacaciones == 4) {
                        if (items.length > 0) {
                            dateCELactanciaInicio.val(moment(items[0].fecha).format("DD/MM/YYYY"));
                            dateCELactanciaInicio.change();
                        } else {
                            dateCELactanciaInicio.val(moment().format("DD/MM/YYYY"));
                            dateCELactanciaInicio.change();
                        }
                    } else {
                        if (btnCEVacacionActualizar.attr("data-id") > 0) {
                            fncFillCallendar(items, response.data.fechasTotalesSinActual, idEmpleado, tipoVacaciones);
                        } else {
                            fncFillCallendar([], items, idEmpleado, tipoVacaciones);

                        }

                        let numDiasDisponibles = 0;

                        //#region ASIGNAR DIAS DISPONIBLES POR PERMISO

                        if (!_esAdmin) {
                            switch (tipoVacaciones) {
                                case '0':
                                    numDiasDisponibles = 5
                                    txtCalendarioNumDias.text(numDiasDisponibles).trigger("change");

                                    break;
                                case '1':
                                    numDiasDisponibles = 3
                                    txtCalendarioNumDias.text(numDiasDisponibles).trigger("change");

                                    break;
                                case '2':
                                    //∞
                                    numDiasDisponibles = 999999999999999
                                    txtCalendarioNumDias.text("∞").trigger("change");

                                    break;
                                case '3':
                                    numDiasDisponibles = 3
                                    txtCalendarioNumDias.text(numDiasDisponibles).trigger("change");

                                    break;
                                case '4':
                                    txtCalendarioNumDias.text("MESES").trigger("change");

                                    break;
                                case '5':
                                    numDiasDisponibles = 1
                                    txtCalendarioNumDias.text(numDiasDisponibles).trigger("change");

                                    break;
                                case '6':
                                    numDiasDisponibles = 1
                                    txtCalendarioNumDias.text(numDiasDisponibles).trigger("change");
                                    break;
                                default:
                                    txtCalendarioNumDias.text("∞").trigger("change");
                                    break;
                            }

                            notClickablePer = true;
                            notClickableEmp = true;

                            if (txtCalendarioNumDias.text() != "∞") {
                                if (tipoVacaciones != "5") {

                                    txtCalendarioNumDias.text(Number(txtCalendarioNumDias.text()) - items.length)
                                }
                            }
                        } else {
                            numDiasDisponibles = 999999999999999
                            txtCalendarioNumDias.text("∞").trigger("change");

                            notClickablePer = true;
                            notClickableEmp = true;
                        }
                        //#endregion
                    }

                }
            }).catch(error => Alert2Error(error.message));

        }

        // function fncGetNumDias() {
        //     let numDias = 0;

        //     obj = {
        //         id: txtCEVacacionClaveEmp.val(),
        //     }

        //     axios.post("GetHistorialDias", obj).then(response => {
        //         let { success, data, message } = response.data;

        //         if (success) {
        //             data.forEach(e => {

        //                 numDias += e.dias;

        //             });


        //             notClickableEmp = true;
        //         }
        //     }).catch(error => Alert2Error(error.message));
        // }

        // function fncGetPeriodos(idReg) {

        //     let obj = {
        //         id: idReg
        //     }

        //     axios.post("GetPeriodos",obj).then(response => {
        //         let { success, items, message } = response.data;
        //         if (success) {
        //             notClickablePer = true;

        //             if (items.length == 1 ) {
        //                 let events = $('#calendar').fullCalendar('clientEvents');

        //                 events.forEach(e => {
        //                     if (e.classNames[0] != "Vacaciones") {
        //                         $('#calendar').fullCalendar('removeEvents', [e._id]);
        //                     }
        //                 });
        //                 $('#calendar').fullCalendar('addEventSource', [{
        //                     title: 'Rango',
        //                     start: moment(items[0].fechaInicio).format("YYYY-MM-DD"),
        //                     end: moment(items[0].fechaFinal).add(1, 'days').format("YYYY-MM-DD"),
        //                     color: '#BEBEBE',
        //                     rendering: "inverse-background"
        //                 }]);
        //                 $('#calendar').fullCalendar('gotoDate',moment(items[0].fechaInicio));
        //             }else{
        //                 let events = $('#calendar').fullCalendar('clientEvents');
        //                 events.forEach(e => {
        //                     if (e.classNames[0] != "Rango") {
        //                         $('#calendar').fullCalendar('removeEvents', [e._id]);
        //                     }
        //                 });

        //             }

        //         }
        //     }).catch(error => Alert2Error(error.message));
        // }

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
                    // Alert2Error("Este empleado no tiene reponsable asignado")
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetDiasDispPermisos() {
            if (btnCEVacacionActualizar.attr("data-id") == 0) {
                if (+txtCEVacacionClaveEmp.val() > 0 && +cboCETipoPermiso.val() >= 0) {
                    let tipoVac = cboCETipoPermiso.val();

                    axios.post("GetDiasDispPermisos", { cveUsuario: txtCEVacacionClaveEmp.val(), tipoPermiso: tipoVac }).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            let numDiasTomados = items.length + response.data.lstFechasCapturadas.length;
                            let numDiasDisponibles = 0;
                            switch (tipoVac) {
                                case '0':
                                    numDiasDisponibles = 5
                                    txtCalendarioNumDias.text(numDiasDisponibles).trigger("change");

                                    break;
                                case '1':
                                    numDiasDisponibles = 3
                                    // numDiasDisponibles -= numDiasTomados
                                    txtCalendarioNumDias.text(numDiasDisponibles).trigger("change");

                                    break;
                                case '2':
                                    //∞
                                    numDiasDisponibles = 999999999999999
                                    txtCalendarioNumDias.text("∞").trigger("change");

                                    break;
                                case '3':
                                    numDiasDisponibles = 3
                                    // numDiasDisponibles -= numDiasTomados
                                    txtCalendarioNumDias.text(numDiasDisponibles).trigger("change");

                                    break;
                                case '4':
                                    txtCalendarioNumDias.text("MESES").trigger("change");

                                    break;
                                case '5':
                                    numDiasDisponibles = 1
                                    // numDiasDisponibles -= numDiasTomados
                                    // fncVerificarAntiguedadEmpleado(numDiasDisponibles);

                                    break;
                                case '6':
                                    numDiasDisponibles = 1
                                    // numDiasDisponibles -= numDiasTomados
                                    txtCalendarioNumDias.text(numDiasDisponibles).trigger("change");
                                    break;
                                default:
                                    txtCalendarioNumDias.text("∞").trigger("change");
                                    break;
                            }

                            notClickablePer = true;
                            notClickableEmp = true;

                            // if (txtCalendarioNumDias.text() != "∞") {
                            //     txtCalendarioNumDias.text(Number(txtCalendarioNumDias.text()) - response.data.lstFechasCapturadas.length)

                            // }
                        } else {
                            notClickablePer = false;
                            notClickableEmp = false;
                        }
                    }).catch(error => Alert2Error(error.message));
                }
            } else {

            }
        }

        function fncVerificarAntiguedadEmpleado(numDiasDisponibles) {
            // SE VERIFICA SI EL EMPLEADO YA CUENTA CON 1 AÑO DE ANTIGUEDAD.
            if (+txtCEVacacionClaveEmp.val() >= 0) {
                let obj = new Object();
                obj.claveEmpleado = +txtCEVacacionClaveEmp.val();
                axios.post('VerificarAntiguedadEmpleado', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        let tieneVacaciones = response.data.tieneVacaciones;
                        if (tieneVacaciones) {
                            // txtCalendarioNumDias.text(1).trigger("change");
                            txtCalendarioNumDias.text(numDiasDisponibles).trigger("change");
                        } else {
                            Alert2Error("El empleado no tiene la antiguedad sufuciente");
                            txtCalendarioNumDias.text(0).trigger("change");
                        }

                    } else {
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario indicar la clave del empleado.");
            }
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

            // fncGetResponsable(txtCEVacacionClaveEmp.val());
            if (cboCETipoPermiso.val() != "") {
                fncGetFechas(txtCEVacacionClaveEmp.val(), cboCETipoPermiso.val());
            }

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

            if (cboCETipoPermiso.val() == "5") {
                divCEVacacionesNombreMedico.css("display", "initial")
            } else {
                divCEVacacionesNombreMedico.css("display", "none")
            }

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
                    tipoVacaciones: cboTipoPermiso.val(),
                    fechaFiltroInicio: dateIni.format("DD/MM/YYYY"),
                    fechaFiltroFin: dateFin.format("DD/MM/YYYY"),
                }
            } else {
                objFiltro = {
                    estado: cboFiltroEstado.val(),
                    idPeriodo: null,
                    ccEmpleado: cboVacacionCC.val(),
                    tipoVacaciones: cboTipoPermiso.val(),
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
            if (cboCETipoPermiso.val() == "") { cboCETipoPermiso.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }


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
                    tipoVacaciones: cboCETipoPermiso.val(),
                    esPagadas: false,
                    justificacion: txtCEVacacionJustific.val(),
                    idJefeInmediato: txtCEVacacionNombreJefeInmediato.val(),
                    nombreJefeInmediato: $("#txtCEVacacionNombreJefeInmediato option:selected").text(),
                    capitalHumano: +selectCapitalHumano.val()
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
            cboCETipoPermiso.css("border", "1px solid #CCC");

        }

        function fncEmptyFields() {
            txtCEVacacionNombreEmp.val("");
            txtCEVacacionClaveEmp.val("");
            txtCEVacacionClaveResponsable.val("");
            txtCEVacacionNombreResponsable.val("");
            txtCEVacacionClaveResponsablePagadas.val("");
            txtCEVacacionNombreResponsablePagadas.val("");
            txtCalendarioNumDias.text("∞").trigger("change");
            dateCELactanciaInicio.val(moment().format("DD/MM/YYYY"));
            dateCELactanciaInicio.trigger("change");
            cboCETipoPermiso.val("");
            cboCETipoPermiso.trigger("change");
            txtCEVacacionCC.val("");
            txtCEVacacionFechaIngreso.val("");
            txtCEVacacionNombreJefeInmediato.val("");
            txtCEVacacionNombreJefeInmediato.trigger("change");
            txtCEVacacionNumJefeInmediato.val("");
            txtCEVacacionPuesto.val("");
            txtCEVacacionJustific.val("");

            let events = $('#calendar').fullCalendar('clientEvents');

            if (events.length != 0) {
                // cboCEVacacionPeriodo[0].selectedIndex = 0;
                // cboCEVacacionPeriodo.trigger("change");
            } else {
                $('#calendar').fullCalendar('removeEvents');
            }

            divCapitalHumano.css('display', 'none');
            divArchivoActa.css('display', 'none');

            inputArchivoActa.val('');
            inputArchivoActa.change();

            selectCapitalHumano.val('');
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
                let todayNext2Months = moment().add(2, 'M');

                let events = $('#calendar').fullCalendar('clientEvents');

                events.forEach(e => {
                    if (e.title != "Vacaciones" || (e.classNames != undefined && e.classNames[0] != "VacacionesCapturadas")) {
                        $('#calendar').fullCalendar('removeEvents', [e._id]);
                    }
                });

                $('#calendar').fullCalendar('addEventSource', [{
                    title: 'Rango',
                    start: anteantier.format("YYYY-MM-DD"),
                    end: todayNext2Months.format("YYYY-MM-DD"),
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
                                    fncGetFechas(txtCEVacacionClaveEmp.val(), cboCETipoPermiso.val());
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

                            let eventNum = cboCETipoPermiso.val();
                            let eventName = $(`#cboCETipoPermiso option[value='${eventNum}']`).text();

                            $('#calendar').fullCalendar('addEventSource', [{
                                title: eventName,
                                start: startStr,
                                end: endStr,
                                classNames: ['Vacaciones']

                            }]);

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
                if (e.title == "Vacaciones" || (e.classNames != undefined && (e.classNames[0] == "VacacionesCapturadas" || e.classNames[0] == "Vacaciones"))) {
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
        CH.Permisos = new Permisos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();