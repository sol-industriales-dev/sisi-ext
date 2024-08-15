(() => {
    $.namespace('CH.Vacaciones');

    //#region CRUD CONSTS

    const mdlCalendario = $('#mdlCalendario');
    const tblRH_Vacaciones_Vacaciones = $('#tblRH_Vacaciones_Vacaciones');
    const cboFiltroEstado = $('#cboFiltroEstado');
    const btnFiltroNuevo = $('#btnFiltroNuevo');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const cboVacacionPeriodo = $('#cboVacacionPeriodo');
    const cboVacacionCC = $('#cboVacacionCC');
    const dateFiltroIni = $('#dateFiltroIni');
    const dateFiltroFin = $('#dateFiltroFin');
    const cboFiltroEstatusEmpleado = $('#cboFiltroEstatusEmpleado');

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
    // const cboCEVacacionPeriodo = $('#cboCEVacacionPeriodo');
    const txtCEVacacionClaveResponsablePagadas = $('#txtCEVacacionClaveResponsablePagadas');
    const txtCEVacacionNombreResponsablePagadas = $('#txtCEVacacionNombreResponsablePagadas');
    const chkTipoVacaciones = $('#chkTipoVacaciones');
    const groupCEVacacionClaveResponsablePagadas = $('#groupCEVacacionClaveResponsablePagadas');
    const groupCEVacacionNombreResponsablePagadas = $('#groupCEVacacionNombreResponsablePagadas');
    const tooltipCEVacaciones = $('#tooltipCEVacaciones');
    const txtCEVacacionNombreJefeInmediato = $('#txtCEVacacionNombreJefeInmediato');
    const txtCEVacacionNumJefeInmediato = $('#txtCEVacacionNumJefeInmediato');
    const txtCEVacacionFechaIngreso = $('#txtCEVacacionFechaIngreso');
    const txtCEVacacionCC = $('#txtCEVacacionCC');
    const txtCEVacacionPuesto = $('#txtCEVacacionPuesto');
    const txtCEVacacionClaveResponsablePagadas2 = $('#txtCEVacacionClaveResponsablePagadas2');
    const txtCEVacacionNombreResponsablePagadas2 = $('#txtCEVacacionNombreResponsablePagadas2');
    const txtCEVacacionJustific = $('#txtCEVacacionJustific');
    const divChkTipoVacaciones = $('#divChkTipoVacaciones');
    const divContainerCalendario = $('#divContainerCalendario');
    const divContainerPagadas = $('#divContainerPagadas');
    const numCEVacacionNumDiasPagados = $('#numCEVacacionNumDiasPagados');
    //#endregion

    //#region CALENDARIO

    const calendar = $("#calendar");
    const btnCalendarioGuardar = $('#btnCalendarioGuardar');
    const btnTxtCalendario = $('#btnTxtCalendario');
    const txtCalendarioNumDias = $('#txtCalendarioNumDias');

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

    //#region CONST FECHAS MES
    const mdlFechasMes = $('#mdlFechasMes');
    const labelNumDias = $('#labelNumDias');
    const tblFechasMes = $('#tblFechasMes');

    let dtFechasMes;
    //#endregion

    let changeStatus = null;
    let esUsuarioReg = false;
    let cveUsuarioReg = 0;

    //ES CONSULTA
    const inputEsConsulta = $('#inputEsConsulta');
    let _esConsulta = inputEsConsulta.val();

    //ES SUPERADMIN
    let _esAdmin = false;

    Vacaciones = function () {
        (function init() {

            initCalendar();
            fncSetPeriodos(null, true);
            initTblRH_Vacaciones_Vacaciones();
            initTblFirmas();
            fncGetVacaciones();
            fncListeners();
        })();

        function fncListeners() {

            // cboCEVacacionPeriodo.fillCombo('/Administrativo/Vacaciones/FillComboPeriodos', {}, false);
            cboVacacionPeriodo.fillCombo('/Administrativo/Vacaciones/FillComboPeriodos', {}, false);
            cboVacacionCC.fillCombo('/Administrativo/Vacaciones/FillComboCC', {}, false);
            txtCEVacacionNombreJefeInmediato.fillCombo("FillCboUsuarios", {}, false);
            txtCEVacacionNombreResponsablePagadas.fillCombo("FillCboUsuarios", {}, false);
            txtCEVacacionNombreResponsablePagadas2.fillCombo("FillCboUsuarios", {}, false);

            $(".select2").select2({ width: "100%" });


            btnFiltroBuscar.on("click", function () {
                fncGetVacaciones();
            });

            btnFiltroNuevo.on("click", function () {
                fncEmptyFields();
                fncDefaultBorder();
                mdlVacacion.modal("show");
                titleCEVacacion.text("Crear Vacaciones");
                btnTxtCEVacacion.text("Guardar");
                btnTxtCEVacacion.prop("disabled", true);
                btnCEVacacionActualizar.attr("data-id", 0);
                tooltipCEVacaciones.attr("Title", "Crear");

                txtCEVacacionNombreEmp.attr("disabled", false);
                txtCEVacacionNombreEmp.focus();

                notClickablePer = false;
                notClickableEmp = false;

                chkTipoVacaciones.bootstrapToggle('off');
                btnCEVacacionActualizar.show();

                if ($.fn.DataTable.isDataTable('#tblFechasMes')) {
                    dtFechasMes.clear();
                    dtFechasMes.draw();
                    // fncInitChartIncaps();

                }

                if (esUsuarioReg) {

                    if (_esConsulta != "1") {
                        txtCEVacacionNombreEmp.prop("readonly", true);

                    }

                    divChkTipoVacaciones.hide();

                    fncGetFechas(cveUsuarioReg);
                    txtCEVacacionNombreResponsable.fillCombo('/Administrativo/Vacaciones/FillComboAutorizantes', { clave_empleado: cveUsuarioReg }, false);
                    fncGetDatosPersonal(cveUsuarioReg, txtCEVacacionNombreEmp.val(), false, false);
                    txtCEVacacionNombreEmp.trigger("change");

                } else {
                    fncGetFechas(0);

                }
            });

            btnCEVacacionActualizar.on("click", function () {
                fncCrearEditarVacaciones(false);
            });

            // btnCalendarioGuardar.on("click", function(){
            //     fncCEFechas();
            // });

            // txtCEVacacionClaveEmp.on("change", function () {


            //     let events = $('#calendar').fullCalendar('clientEvents');
            //     events.forEach(e => {
            //         if (e.title != "Rango") {
            //             $('#calendar').fullCalendar('removeEvents', [e._id]);
            //         }
            //     })

            //     fncGetDatosPersonal(txtCEVacacionClaveEmp.val(), txtCEVacacionNombreEmp.val(), false, false);
            //     fncGetFechas(txtCEVacacionClaveEmp.val());

            // });

            // txtCEVacacionNombreResponsable.getAutocomplete(funGetGerente, null, '/Administrativo/BajasPersonalEntrevista/getEmpleadosGeneral');
            txtCEVacacionNombreEmp.getAutocomplete(funGetEmpleado, null, '/Administrativo/FormatoCambio/getCatEmpleadosReclutamientos');

            txtCEVacacionNombreEmp.on("change", function () {
                // fncGetResponsable(txtCEVacacionClaveEmp.val());

                // fncGetDatosPersonal(txtCEVacacionClaveEmp.val(), txtCEVacacionNombreEmp.val(), false, false);

            });

            txtCEVacacionNombreResponsable.on("change", function () {
                if ($(this).val() != "") {
                    txtCEVacacionClaveResponsable.val($(this).val());
                } else {
                    txtCEVacacionClaveResponsable.val("");
                }
            });

            // txtCEVacacionClaveResponsable.on("change", function () {
            //     fncGetDatosPersonal(txtCEVacacionClaveResponsable.val(), txtCEVacacionNombreResponsable.val(), true, false);
            // });

            // txtCEVacacionClaveResponsablePagadas.on("change", function () {
            //     fncGetDatosPersonal(txtCEVacacionClaveResponsablePagadas.val(), txtCEVacacionClaveResponsablePagadas.val(), true, true);

            // });

            // cboCEVacacionPeriodo.on("change", function () {

            //     if (cboCEVacacionPeriodo.val() != "") {
            //         fncGetPeriodos(cboCEVacacionPeriodo.val());

            //     } else {
            //         $('#calendar').fullCalendar('removeEvents');

            //     }
            //     btnTxtCEVacacion.prop("disabled", false);

            // });

            chkTipoVacaciones.on("change", function () {
                if (chkTipoVacaciones.prop("checked")) {
                    //AUTH
                    groupCEVacacionClaveResponsablePagadas.show();
                    groupCEVacacionNombreResponsablePagadas.show();

                    //CALENDARIO|INPUT
                    divContainerCalendario.hide();
                    divContainerPagadas.show();

                } else {
                    //AUTH
                    groupCEVacacionClaveResponsablePagadas.hide();
                    groupCEVacacionNombreResponsablePagadas.hide();

                    //CALENDARIO|INPUT
                    divContainerCalendario.show();
                    divContainerPagadas.hide();
                }
            });

            labelNumDias.on("click", function () {
                mdlFechasMes.modal("show");
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
                        data: 'esPagadas', title: 'PAGADAS',
                        render: function (data, type, row) {
                            return data ? "PAGADAS" : "NO PAGADAS";
                        }
                    },
                    {
                        render: function (data, type, row) {
                            let btnDownload = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Descargar Reporte"><button class="btn btn-primary reporteVacacion btn-xs"><i class="fas fa-file-download"></i></button>&nbsp;</span>`;
                            let btnActualizar = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Actualizar"><button class="btn btn-warning actualizarVacacion btn-xs"><i class="far fa-edit"></i></button>&nbsp;</span>`;
                            let btnEliminar = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Eliminar"><button class="btn btn-danger eliminarVacacion btn-xs esConsulta"><i class="far fa-trash-alt"></i></button>&nbsp;</span>`;
                            let btnFirmas = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Ver autorizantes"><button class="btn btn-primary verFirmas btn-xs"><i class="fas fa-signature"></i></button>&nbsp;</span>`;
                            let btnJustificacion = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Ver justificacion"><button class="btn btn-primary verJustificacion btn-xs"><i class="far fa-comments"></i></button>&nbsp;</span>`;

                            if (cveUsuarioReg > 0 && row.claveEmpleado == cveUsuarioReg) {
                                btnEliminar = btnEliminar.replace("esConsulta", "");
                            }

                            let btns = btnJustificacion + btnFirmas + btnDownload + btnActualizar + btnEliminar;

                            if (!_esAdmin) {
                                if (row.estado == 2 || row.estado == 1) {
                                    btns = btnJustificacion + btnFirmas + btnDownload + btnActualizar;
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
                        // txtCEVacacionNombreResponsable.fillCombo('/Administrativo/Vacaciones/FillComboAutorizantes', { clave_empleado: rowData.claveEmpleado }, false);

                        btnCEVacacionActualizar.attr("data-id", rowData.id);
                        btnCEVacacionActualizar.attr("data-notificada", rowData.notificada);
                        tooltipCEVacaciones.attr("Title", "Actualizar");
                        mdlVacacion.modal("show");
                        titleCEVacacion.text("Actualizar Vacaciones");
                        btnTxtCEVacacion.text("Actualizar");
                        txtCEVacacionNombreEmp.val(rowData.nombreEmpleado);
                        txtCEVacacionClaveEmp.val(rowData.claveEmpleado);
                        txtCEVacacionNombreResponsable.val(rowData.claveResponsable);
                        txtCEVacacionNombreResponsable.trigger("change");
                        txtCEVacacionClaveResponsable.val(rowData.claveResponsable);
                        // cboCEVacacionPeriodo.val(rowData.idPeriodo);
                        // cboCEVacacionPeriodo.change();
                        txtCEVacacionJustific.val(rowData.justificacion);
                        txtCEVacacionNombreJefeInmediato.val(rowData.idJefeInmediato);
                        txtCEVacacionNombreJefeInmediato.trigger("change");
                        numCEVacacionNumDiasPagados.val(rowData.numDiasPagados * -1);

                        if (rowData.esPagadas) {
                            chkTipoVacaciones.bootstrapToggle('on');

                        } else {
                            chkTipoVacaciones.bootstrapToggle('off');

                        }

                        txtCEVacacionCC.val(rowData.ccDesc);

                        fncSetAutorizantes(rowData.lstAutorizantes, rowData.claveEmpleado);
                        fncGetDatosPersonal(rowData.claveEmpleado, rowData.nombreEmpleado, false, false, true);

                        fncGetFechas(rowData.claveEmpleado);
                        notClickablePer = true;
                        notClickableEmp = true;

                        if (esUsuarioReg) {
                            txtCEVacacionNombreEmp.prop("readonly", true);
                            divChkTipoVacaciones.hide();
                        }

                        if (rowData.estado == 1 || rowData.estado == 2) {
                            btnCEVacacionActualizar.hide();
                        } else {
                            btnCEVacacionActualizar.show();

                        }

                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.eliminarVacacion', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar la vacacion seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarVacacion(rowData.id));
                    });
                    tblRH_Vacaciones_Vacaciones.on('click', '.notificarVacacion', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('', '¿Desea notificar al repsonsable?', 'Confirmar', 'Cancelar', () => fncSetNotificada(rowData.id));
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
                        // fncGetReporte(rowData.id, rowData.tipoVacaciones);
                        mdlJustific.modal("show");
                        txtVacacionJustific.val(rowData.justificacion);
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

        function initTblFechasMes(columns) {
            dtFechasMes = tblFechasMes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: columns,
                initComplete: function (settings, json) {
                    tblFechasMes.on('click', '.classBtn', function () {
                        let rowData = dtFechasMes.row($(this).closest('tr')).data();
                    });
                    tblFechasMes.on('click', '.classBtn', function () {
                        let rowData = dtFechasMes.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
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

                        if (!response.data.esAdmnCH) {
                            divChkTipoVacaciones.hide();
                        } else {
                            _esAdmin = true;
                        }

                        if (response.data.claveEmpleado != "" && response.data.claveEmpleado != null) {
                            esUsuarioReg = true;
                            cveUsuarioReg = response.data.claveEmpleado;

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

            let listaFechas = fncGetFechasCalendiario();

            if (objVacacion != "" && objVacacion.lstAutorizantes != "" && (!objVacacion.esPagadas ? (listaFechas.length > 0) : true)) {
                axios.post("CrearEditarVacaciones", objVacacion).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        if (!objVacacion.esPagadas) {
                            fncCEFechas(items.id, esSobreEscribir);

                        }

                        fncGetVacaciones();
                        mdlVacacion.modal("hide");
                    } else {
                        Alert2Warning(message);

                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Favor de llenar todos los campos y capturar las vacaciones");
            }
        }

        function fncGetAutorizantes() {
            let notificantes = [];

            //POR SI LAS PULGAS
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

            if (chkTipoVacaciones.prop("checked")) { //PAGADAS
                if (txtCEVacacionNombreResponsablePagadas.val() != "" && txtCEVacacionNombreResponsablePagadas.val() != "--Seleccione--") {
                    notificantes.push({
                        idUsuario: txtCEVacacionNombreResponsablePagadas.val(),
                        orden: 2
                    });

                } else {
                    Alert2Warning("Seleccione un Responsable (1)");
                    return "";
                }

                if (txtCEVacacionNombreResponsablePagadas2.val() != "" && txtCEVacacionNombreResponsablePagadas2.val() != "--Seleccione--") {
                    notificantes.push({
                        idUsuario: txtCEVacacionNombreResponsablePagadas2.val(),
                        orden: 3
                    });

                } else {
                    Alert2Warning("Seleccione un Responsable (1)");
                    return "";
                }

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
                    case 2:
                        txtCEVacacionNombreResponsablePagadas.val(item.idUsuario);
                        txtCEVacacionNombreResponsablePagadas.trigger("change");

                        break;
                    case 3:
                        txtCEVacacionNombreResponsablePagadas2.val(item.idUsuario);
                        txtCEVacacionNombreResponsablePagadas2.trigger("change");

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
                    Alert2Exito("Vacacion Eliminada.");
                    fncGetVacaciones();

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetFechasCalendiario() {
            let events = $('#calendar').fullCalendar('clientEvents');
            let listaFechas = [];
            let dias = 0;

            if (events.length > 0) {

                for (let i = 0; i < events.length; i++) {

                    if (events[i].classNames != undefined && events[i].classNames[0] == "Vacaciones") {
                        // if (events[i].title == "Vacaciones") {
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
            return listaFechas;
        }

        function fncCEFechas(idReg, esSobreEscribir) {
            let listaFechas = fncGetFechasCalendiario();
            let esEditar = false;

            if (btnCEVacacionActualizar.attr("data-id") > 0) {
                esEditar = true;
            }

            axios.post("CrearEditarFechas", { idVacacion: idReg, lstFechas: listaFechas, diasPermitidos: txtCalendarioNumDias.text(), tipoPermiso: 5, esSobreEscribir: esSobreEscribir, esEditar: esEditar }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Vacaciones Guardadas.");
                } else {
                    if (response.data.esRecursar) {
                        Alert2AccionConfirmar('¡Cuidado!', message, 'Confirmar', 'Cancelar', () => fncCEFechas(idReg, true));

                    } else {
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

        function fncGetDatosPersonal(claveUsuario, nombreUsuario, esGerente, esGerentePagadas, esActualizar = false) {
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
                        let dateAntiguedad = moment(response.data.objDatosPersona.fechaAntiguedad);

                        txtCEVacacionClaveEmp.val(claveUsuario);
                        txtCEVacacionNombreEmp.val(response.data.objDatosPersona.nombreCompleto);
                        txtCEVacacionFechaIngreso.val(dateAntiguedad.format("DD/MM/YYYY"));
                        txtCEVacacionPuesto.val(response.data.objDatosPersona.nombrePuesto);

                        let dateAniversario = moment(new Date(moment().year(), dateAntiguedad.month(), dateAntiguedad.date()))

                        if (response.data.esAdmnCH) {
                            fncSetPeriodos(dateAniversario, true);

                        } else {
                            fncSetPeriodos(dateAniversario, false);

                        }

                        if (!esActualizar) {
                            txtCEVacacionCC.val(response.data.objDatosPersona.cc);

                        }
                    }
                    // txtCEVacacionNombreEmp.trigger("change");

                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetFechas(idEmpleado) {
            // fncGetNumDias();

            let obj = {
                idReg: btnCEVacacionActualizar.attr("data-id"),
                tipoPermiso: 7,
                clave_empleado: idEmpleado,
                // esGestion: true
            }

            axios.post("GetFechas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    let fechaAniversario = moment(response.data.fechaAniversario);

                    notClickableEmp = true;
                    if (btnCEVacacionActualizar.attr("data-id") > 0) {
                        fncFillCallendar(items, response.data.fechasTotalesSinActual, idEmpleado);

                    } else {
                        fncFillCallendar([], items, idEmpleado);
                        // fncEmptyCallendar();
                    }

                    if (response.data.fechasTotales != null && response.data.fechasTotales != undefined) {
                        let numDiasRestarActual = 0;
                        for (const item of response.data.fechasTotales) {
                            var dateItem = moment(item.fecha);

                            if (dateItem._d >= fechaAniversario) {
                                numDiasRestarActual++;
                            }
                        }
                        txtCalendarioNumDias.text((response.data.diasIniciales - numDiasRestarActual));

                    } else {
                        let numDiasRestarActual = 0;
                        for (const item of items) {
                            var dateItem = moment(item.fecha);

                            if (dateItem._d >= fechaAniversario) {
                                numDiasRestarActual++;
                            }
                        }
                        txtCalendarioNumDias.text((response.data.diasIniciales - numDiasRestarActual));

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

        function fncSetPeriodos(fechaAniversario, esAdmn) {
            if (esAdmn) {
                // let events = $('#calendar').fullCalendar('clientEvents');

                // events.forEach(e => {
                //     if ((e.classNames != undefined && e.classNames[0] != "VacacionesCapturadas")) {
                //         $('#calendar').fullCalendar('removeEvents', [e._id]);
                //     }
                // });

                // $('#calendar').fullCalendar('gotoDate', moment());
            } else {
                let fechaInicioPer = moment().add(-3, 'd');
                let today = moment();
                let fechaFinPer = moment().add(3, 'M');

                if (fechaAniversario != undefined && fechaAniversario != null) {

                    if (fechaAniversario._d > today._d) {
                        fechaFinPer = fechaAniversario;
                    } else {
                        if (fechaAniversario._d > fechaInicioPer._d) {
                            fechaInicioPer = fechaAniversario;

                        }
                    }
                }

                let events = $('#calendar').fullCalendar('clientEvents');

                // events.forEach(e => {
                //     if (e.title != "Vacaciones" || (e.classNames != undefined && e.classNames[0] != "VacacionesCapturadas")) {
                //         $('#calendar').fullCalendar('removeEvents', [e._id]);
                //     }
                // });
                $('#calendar').fullCalendar('addEventSource', [{
                    title: 'Rango',
                    start: fechaInicioPer.format("YYYY-MM-DD"),
                    end: fechaFinPer.format("YYYY-MM-DD"),
                    color: '#BEBEBE',
                    rendering: "inverse-background"
                }]);
                // $('#calendar').fullCalendar('gotoDate', fechaInicioPer);
            }
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
                    // txtCEVacacionClaveResponsable.val(items.clave_responsable);
                    // txtCEVacacionNombreResponsable.val(response.data.nombre)
                    // txtCEVacacionClaveResponsable.trigger("change");
                    // fncGetDatosPersonal(txtCEVacacionClaveResponsablePagadas.val(), txtCEVacacionClaveResponsablePagadas.val(), true, false);
                    fncGetFechas(txtCEVacacionClaveEmp.val());

                } else {
                    Alert2Error("Este empleado no tiene reponsable asignado")
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#endregion

        //#region FNC GRALES

        function funGetGerente(event, ui) {
            // txtCEVacacionClaveResponsable.val(ui.item.id);
            // txtCEVacacionNombreResponsable.val(ui.item.value);
            // //fncGetFechas(txtCEVacacionClaveEmp.val());
            //fncGetResponsable(txtCEVacacionClaveEmp.val());
        }

        function funGetEmpleado(event, ui) {
            txtCEVacacionClaveEmp.val(ui.item.id);
            txtCEVacacionNombreEmp.val(ui.item.value);

            // fncGetResponsable(txtCEVacacionClaveEmp.val()); // REMPLAZADO POR AUTORIZANTES DE FACULTAMIENTOS
            fncGetFechas(txtCEVacacionClaveEmp.val());

            if (txtCEVacacionClaveEmp.val() != "") {
                txtCEVacacionNombreResponsable.fillCombo('/Administrativo/Vacaciones/FillComboAutorizantes', { clave_empleado: txtCEVacacionClaveEmp.val() }, false);
                fncGetDatosPersonal(txtCEVacacionClaveEmp.val(), txtCEVacacionNombreEmp.val(), false, false);

                if (_esConsulta == 1) {
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
                    tipoVacaciones: 7,
                    fechaFiltroInicio: dateIni.format("DD/MM/YYYY"),
                    fechaFiltroFin: dateFin.format("DD/MM/YYYY"),
                    // estatusEmpleado: cboFiltroEstatusEmpleado.val(),
                }
            } else {
                objFiltro = {
                    estado: cboFiltroEstado.val(),
                    idPeriodo: 0,
                    ccEmpleado: cboVacacionCC.val(),
                    tipoVacaciones: 7,
                    fechaFiltroInicio: null,
                    fechaFiltroFin: null,
                    // estatusEmpleado: cboFiltroEstatusEmpleado.val(),
                }
            }

            return objFiltro;
        }

        function fncVacacionParametros() {
            fncDefaultBorder();

            let numCalendarioNumDias = Number(txtCalendarioNumDias.text());

            let strMessage = "";

            // if (txtCEVacacionClaveEmp.val() == "") { txtCEVacacionClaveEmp.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (txtCEVacacionNombreEmp.val() == "") { txtCEVacacionNombreEmp.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            // if (txtCEVacacionClaveResponsable.val() == "") { txtCEVacacionClaveResponsable.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (txtCEVacacionNombreResponsable.val() == "") { txtCEVacacionNombreResponsable.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            if (txtCEVacacionNombreJefeInmediato.val() == "") { $("#select2-txtCEVacacionNombreJefeInmediato-container").css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            // if (cboCEDatosEmpleadoLugarNac.val() == "" || cboCEDatosEmpleadoLugarNac.val() == null) { $("#select2-cboCEDatosEmpleadoLugarNac-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            if (chkTipoVacaciones.prop("checked")) { //PAGADAS
                if (txtCEVacacionNombreResponsablePagadas.val() == "") { txtCEVacacionNombreResponsablePagadas.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
                if (txtCEVacacionNombreResponsablePagadas2.val() == "") { txtCEVacacionNombreResponsablePagadas2.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
                if (numCEVacacionNumDiasPagados.val() == "" || numCEVacacionNumDiasPagados.val() == 0) { numCEVacacionNumDiasPagados.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
                if (numCalendarioNumDias == 0) { numCEVacacionNumDiasPagados.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
                if (numCalendarioNumDias < numCEVacacionNumDiasPagados.val()) { numCEVacacionNumDiasPagados.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }

            }

            if (strMessage != "") {
                Alert2Warning(strMessage);
                return "";
            } else {
                let obj = new Object();

                obj =
                {
                    id: btnCEVacacionActualizar.attr("data-id"),
                    estado: changeStatus ?? 3,
                    nombreEmpleado: txtCEVacacionNombreEmp.val(),
                    claveEmpleado: txtCEVacacionClaveEmp.val(),
                    // idPeriodo: cboCEVacacionPeriodo.val(),
                    // fechaInicial: '01/01/2022',
                    // fechaFinal: '01/01/2022',
                    // nombreResponsable: $('select[id="txtCEVacacionNombreResponsable"] option:selected').text(),
                    // claveResponsable: txtCEVacacionClaveResponsable.val(),
                    // nombreResponsablePagadas: txtCEVacacionNombreResponsablePagadas.val(),
                    // claveResponsablePagadas: txtCEVacacionClaveResponsablePagadas.val(),
                    tipoVacaciones: 7,
                    esPagadas: chkTipoVacaciones.prop("checked"),
                    justificacion: txtCEVacacionJustific.val(),
                    idJefeInmediato: txtCEVacacionNombreJefeInmediato.val(),
                    nombreJefeInmediato: $("#txtCEVacacionNombreJefeInmediato option:selected").text(),
                    numDiasPagados: numCEVacacionNumDiasPagados.val(),
                }
                return obj;
            }
        }

        function fncDefaultBorder() {
            // txtCEVacacionNombreResponsable.css("border", "1px solid #CCC");
            txtCEVacacionClaveEmp.css("border", "1px solid #CCC");
            txtCEVacacionNombreResponsable.css("border", "1px solid #CCC");
            $("#select2-txtCEVacacionNombreJefeInmediato-container").css('border', '1px solid #CCC');

            txtCEVacacionClaveResponsable.css("border", "1px solid #CCC");
            txtCEVacacionNombreEmp.css("border", "1px solid #CCC");
            numCEVacacionNumDiasPagados.css("border", "1px solid #CCC");
        }

        function fncEmptyFields() {
            txtCEVacacionNombreEmp.val("");
            txtCEVacacionClaveEmp.val("");
            txtCEVacacionClaveResponsable.val("");
            txtCEVacacionClaveResponsable.trigger("change");
            txtCalendarioNumDias.text("");
            numCEVacacionNumDiasPagados.val(0);

            txtCEVacacionCC.val("");
            txtCEVacacionFechaIngreso.val("");
            txtCEVacacionNombreJefeInmediato.val("");
            txtCEVacacionNombreJefeInmediato.trigger("change");
            txtCEVacacionNumJefeInmediato.val("");
            txtCEVacacionPuesto.val("");

            txtCEVacacionClaveResponsablePagadas.val("");
            txtCEVacacionNombreResponsablePagadas.val("");
            txtCEVacacionNombreResponsablePagadas.trigger("change");

            txtCEVacacionClaveResponsablePagadas2.val("");
            txtCEVacacionNombreResponsablePagadas2.val("");
            txtCEVacacionNombreResponsablePagadas2.trigger("change");

            let events = $('#calendar').fullCalendar('clientEvents');

            txtCEVacacionJustific.val("");

            if (events.lenght != 0) {
                // cboCEVacacionPeriodo[0].selectedIndex = 0;
                // cboCEVacacionPeriodo.trigger("change");
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
                            //txtCalendarioNumDias.text(fncGetNumDias(btnCalendarioGuardar.attr("data-id")));
                            Alert2AccionConfirmar('¡Cuidado!', '¿Desea quitar TODOS las vacaciones capturadas?', 'Confirmar', 'Cancelar',
                                () => {
                                    // fncGetNumDias();
                                    fncGetFechas(txtCEVacacionClaveEmp.val());
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
                    day: 'Dia',
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

                                txtCalendarioNumDias.text(Number(txtCalendarioNumDias.text()) + range);
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

                                    // ------------------------------------------------------- DESCOMENTAR CUANDO LOS USUARIOS CONOSCAN DE LAS FECHAS LIMITES DE CAPTURA DE CH (2meses) ( ͡° ͜ʖ ͡°)-----------------------
                                    // rangeStartDate = events[i].start.clone();
                                    // dateMinus2Month = events[i].start.clone();
                                    // dateMinus2Month.add(-2, "month");

                                    // if (moment()._d < dateMinus2Month._d) {
                                    //     addToday = false;

                                    // } else if (moment()._d >= events[i].start._d) {
                                    //     addToday = false;

                                    // }
                                    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                                    if (events[i].classNames == undefined || events[i].classNames[0] != "VacacionesCapturadas") {
                                        // LOS EVENTOS (DIAS DE VACACIONES) NECESITAN ESTAR DENTRO DEL RANGO DEL PERIODO DE LAS VACACIONES              
                                        if (addToday) {
                                            if (startStr._d < events[i].start._d) {
                                                addRange = false;

                                            } else if (endStr._d > events[i].end._d) {
                                                addRange = false;

                                            }
                                        }
                                    } else {
                                        if (startStr.format("YYYY-MM-DD") == events[i].start.format("YYYY-MM-DD")) {
                                            addVac = false;
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

        function fncFillCallendar(lstFechas, lstFechasCapturadas, idEmpleado) {

            //#region LIMPIAR
            let events = $('#calendar').fullCalendar('clientEvents');
            events.forEach(e => {
                if (e.title == "Vacaciones" || (e.classNames != undefined && e.classNames[0] == "VacacionesCapturadas")) {
                    $('#calendar').fullCalendar('removeEvents', [e._id]);
                }
            });
            //#endregion

            //#region OTRAS VACACIONES
            if (lstFechasCapturadas.length > 0) {

                lstFechasCapturadas.forEach(item => {
                    let date = moment(item.fecha);
                    $('#calendar').fullCalendar('addEventSource', [{
                        title: 'Vacaciones',
                        start: date,
                        color: '#BEBEBE',
                        classNames: ['VacacionesCapturadas']
                    }]);
                });

                $('#calendar').fullCalendar('gotoDate', moment(lstFechasCapturadas[0].fecha));
            }
            //#endregion

            //#region FECHAS DE LA VACACION A EDITAR
            if (lstFechas.length > 0) {

                lstFechas.forEach(item => {
                    //txtCalendarioNumDias.text(Number(txtCalendarioNumDias.text())-1);
                    let date = moment(item.fecha);
                    $('#calendar').fullCalendar('addEventSource', [{
                        title: 'Vacaciones',
                        start: date,
                        classNames: ['Vacaciones']
                    }]);
                });

                $('#calendar').fullCalendar('gotoDate', moment(lstFechas[0].fecha));
            }
            //#endregion

            //#region LLENAR TABLA DE RESUMEN POR MES
            let fechasTotales = lstFechas;
            fechasTotales.push(...lstFechasCapturadas);

            let freqsFechasMeses = {};
            let freqsFechasAños = [];
            let freqsFechasAñosCols = [];

            for (element of fechasTotales) {

                let dateFecha = moment(element.fecha);
                let dateFechaStr = dateFecha.format("MM/YYYY");

                if (freqsFechasAños.indexOf(dateFecha.year()) == -1) {
                    freqsFechasAñosCols.push({ data: dateFecha.year(), title: dateFecha.year() });
                    freqsFechasAños.push(dateFecha.year());
                }

                if (freqsFechasMeses[dateFechaStr]) {
                    freqsFechasMeses[dateFechaStr] += 1;
                } else {
                    freqsFechasMeses[dateFechaStr] = 1;
                }

                // if (typeof element === "string") {
                //     let newItem = element.toLowerCase();
                //     if (freqsFechas[newItem]) {
                //         freqsFechas[newItem] += 1;
                //     } else {
                //         freqsFechas[newItem] = 1;
                //     }
                // } else {

                // }

            };

            let rowsFechas = [];
            let meses = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];

            for (const key in freqsFechasMeses) {

                let año = key.substring(3, 7);
                let mes = meses[Number(key.substring(0, 2)) - 1];

                let tableRow = {};
                tableRow["order"] = Number(key.substring(0, 2));
                for (const item of freqsFechasAños) {
                    if (año == item) {
                        tableRow[item] = (mes + ": " + freqsFechasMeses[key]);
                    } else {
                        tableRow[item] = (mes + ": " + "0");
                    }
                }
                rowsFechas.push(tableRow);

            }

            rowsFechas = rowsFechas.sort(function (a, b) {
                a = (a.order);
                b = (b.order);

                if (a < b) return -1;
                if (a > b) return 1;

                return 0;
            });

            // console.log(Array.prototype.map.call(freqsFechasMeses, (key, value) => {
            //     return value;
            //     // for (const item of freqsFechasAños) {
            //     //     if (key.year() == item) {
            //     //         return ({ [item]: 0, })

            //     //     } else {
            //     //         return ({ [item]: value, })

            //     //     }
            //     // }
            // }));

            if (freqsFechasAñosCols.length > 0) {
                if ($.fn.DataTable.isDataTable('#tblFechasMes')) {
                    dtFechasMes.clear().destroy();
                    tblFechasMes.empty();

                    // dtIncapacidades = null;

                }
                initTblFechasMes(freqsFechasAñosCols);
                dtFechasMes.clear();
                dtFechasMes.rows.add(rowsFechas);
                dtFechasMes.draw();
            } else {
                if ($.fn.DataTable.isDataTable('#tblFechasMes')) {
                    dtFechasMes.clear();
                    dtFechasMes.draw();
                }
            }

            //#endregion
        }

        function fncEmptyCallendar() {
            let events = $('#calendar').fullCalendar('clientEvents');
            events.forEach(e => {
                if (e.title == "Vacaciones") {
                    $('#calendar').fullCalendar('removeEvents', [e._id]);
                }
            });
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Vacaciones = new Vacaciones();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();