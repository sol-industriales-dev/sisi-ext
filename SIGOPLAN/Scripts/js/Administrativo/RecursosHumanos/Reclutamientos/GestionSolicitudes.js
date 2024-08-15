(() => {
    $.namespace('CH.GestionReclutamientos');

    //#region CONST FILTROS SOLICITUDES
    const cboFiltroCC = $('#cboFiltroCC');
    const cboFiltroPuesto = $('#cboFiltroPuesto');
    const chkFiltroAutorizada = $('#chkFiltroAutorizada');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroLimpiar = $('#btnFiltroLimpiar');
    const tblRH_REC_GestionSolicitudes = $('#tblRH_REC_GestionSolicitudes');
    let dtGestionSolicitudes;
    //#endregion

    //#region CONST MODAL DETALLES SOLICITUD
    const mdlCrearEditarSolicitud = $('#mdlCrearEditarSolicitud');
    const chkCrearEditarPuesto = $('#chkCrearEditarPuesto');
    const cboCrearEditarCC = $('#cboCrearEditarCC');
    const cboCrearEditarPuesto = $('#cboCrearEditarPuesto');
    const txtCrearEditarPuesto = $('#txtCrearEditarPuesto');
    const cboCrearEditarMotivo = $('#cboCrearEditarMotivo');
    const cboCrearEditarSexo = $('#cboCrearEditarSexo');
    const txtCrearEditarInicioEdad = $('#txtCrearEditarInicioEdad');
    const txtCrearEditarFinEdad = $('#txtCrearEditarFinEdad');
    const cboCrearEditarEscolaridad = $('#cboCrearEditarEscolaridad');
    const cboCrearEditarPais = $('#cboCrearEditarPais');
    const cboCrearEditarEstado = $('#cboCrearEditarEstado');
    const cboCrearEditarMunicipio = $('#cboCrearEditarMunicipio');
    const txtCrearEditarAniosExp = $('#txtCrearEditarAniosExp');
    const txtCrearEditarCantVacantes = $('#txtCrearEditarCantVacantes');
    const txtCrearEditarConocimientosGen = $('#txtCrearEditarConocimientosGen');
    const txtCrearEditarExpEspecializada = $('#txtCrearEditarExpEspecializada');
    const divCboCrearEditarPuesto = $('#divCboCrearEditarPuesto');
    const divTxtCrearEditarPuesto = $('#divTxtCrearEditarPuesto');
    //#endregion

    //#region CONST MOTIVO RECHAZO SOLICITUD
    const mdlMotivoRechazo = $('#mdlMotivoRechazo');
    const txtMotivoRechazoSolicitud = $('#txtMotivoRechazoSolicitud');
    //#endregion

    GestionReclutamientos = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            initTblGestionSolicitudes();
            fncFillFiltroCbos();
            fncDeshabilitarCtrlsMdl();
            fncFillCbosMdl();
            fncCtrlsPuestos();

            fncGetGestionSolicitudes();

            btnFiltroBuscar.on("click", function () {
                fncGetGestionSolicitudes();
            });

            btnFiltroLimpiar.on("click", function () {
                cboFiltroCC[0].selectedIndex = 0;
                cboFiltroCC.trigger("change");
                cboFiltroPuesto[0].selectedIndex = 0;
                cboFiltroPuesto.trigger("change");
            });

            cboFiltroCC.select2({ width: 'resolve' });
            cboFiltroPuesto.select2({ width: 'resolve' });
        }

        function fncCtrlsPuestos() {
            if (chkCrearEditarPuesto.prop("checked")) {
                divCboCrearEditarPuesto.hide();
                divTxtCrearEditarPuesto.show();
            } else {
                divCboCrearEditarPuesto.show();
                divTxtCrearEditarPuesto.hide();
            }
        }

        function fncFillFiltroCbos() {
            cboFiltroCC.fillCombo("/Reclutamientos/FillFiltroCboCC", {}, false);
            cboFiltroPuesto.fillCombo("/Reclutamientos/FillFiltroCboPuestos", {}, false);
        }

        function fncFillCbosMdl() {
            cboCrearEditarCC.fillCombo("/Reclutamientos/FillCboCC", {}, false);
            cboCrearEditarMotivo.fillCombo("/Reclutamientos/FillCboMotivos", {}, false);
            cboCrearEditarEscolaridad.fillCombo("/Reclutamientos/FillCboEscolaridades", {}, false);

            cboCrearEditarPais.fillCombo("/Reclutamientos/FillCboPaises", {}, false);

            cboCrearEditarPais.on("change", function () {
                if ($(this).val() != "") {
                    cboCrearEditarEstado.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: $(this).val() }, false);
                }
            });
            cboCrearEditarEstado.on("change", function () {
                if ($(this).val() != "") {
                    cboCrearEditarMunicipio.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: cboCrearEditarPais.val(), _claveEstado: $(this).val() }, false);
                }
            });
        }

        function initTblGestionSolicitudes() {
            dtGestionSolicitudes = tblRH_REC_GestionSolicitudes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                scrollX: false,
                columns: [
                    { title: 'Folio', data: 'id' },
                    {
                        title: 'Vacantes',
                        render: function (data, type, row) {
                            return `${row.cantVacantesCubiertas} de ${row.cantVacantes}`;
                        }
                    },
                    { data: 'cantVacantesPendientes', title: 'Pendientes' },
                    { data: 'ccDescripcion', title: 'CC' },
                    { data: 'puesto', title: 'Puesto' },
                    {
                        title: 'Rango edad',
                        render: function (data, type, row) {
                            return row.rangoInicioEdad + ' - ' + row.rangoFinEdad;
                        }
                    },
                    {
                        title: 'Residencia',
                        render: function (data, type, row) {
                            return row.ciudad + ', ' + row.estado + ', ' + row.pais
                        }
                    },
                    {
                        title: 'Opciones',
                        render: function (data, type, row) {
                            let personalExistente = row.personalExistente;
                            let personalRequerido = row.personalRequerido;
                            let cantVacantes = row.cantVacantes;
                            let vacantesDisponibles = parseFloat(personalExistente) - parseFloat(personalRequerido);
                            if (parseFloat(vacantesDisponibles) != 0) {
                                vacantesDisponibles -= cantVacantes;
                            }

                            let btnVerDetalles = `<button class="btn btn-primary verDetalles btn-xs" title="DETALLES"><i class="fas fa-list"></i></button>&nbsp;`;
                            let btnRechazar = `<button class="btn btn-danger rechazarSolicitud btn-xs" title="RECHAZAR"><i class="far fa-trash-alt"></i></button>&nbsp;`;
                            let btnMotivoRechazo = `<button class="btn btn-primary motivoRechazo btn-xs" title="COMENTARIO RECHAZO"><i class="fas fa-ban"></i></button>&nbsp;`;
                            let btnCrearPuesto = `<button class="btn btn-success crearPuesto btn-xs" cantVacantes="${vacantesDisponibles}" title="ADITIVA"><i class="fas fa-user-plus"></i></button>&nbsp;`;

                            let btns = "";
                            if (row.esAutorizada) {
                                if (row.esPuestoNuevo || vacantesDisponibles <= 0) {
                                    btns = btnVerDetalles + btnRechazar + btnCrearPuesto;
                                } else {
                                    btns = btnVerDetalles + btnRechazar;
                                }
                            } else {
                                btns = btnMotivoRechazo + btnVerDetalles;
                            }
                            return btns;
                        }
                    },
                    { data: 'id', visible: false },
                    { data: 'idSolicitud', visible: false },
                    { data: 'cc', visible: false },
                    { data: 'idPuesto', visible: false },
                    { data: 'puesto', visible: false },
                    { data: 'motivo', visible: false },
                    { data: 'sexo', visible: false },
                    { data: 'rangoInicioEdad', visible: false },
                    { data: 'rangoFinEdad', visible: false },
                    { data: 'escolaridad', visible: false },
                    { data: 'pais', visible: false },
                    { data: 'estado', visible: false },
                    { data: 'ciudad', visible: false },
                    { data: 'aniosExp', visible: false },
                    { data: 'conocimientoGen', visible: false },
                    { data: 'expEspecializada', visible: false },
                    { data: 'cantVacantes', visible: false },
                    { data: 'esAutorizada', visible: false },
                    { data: 'strAutorizada', visible: false },
                    { data: 'motivoRechazo', visible: false },
                    { data: 'esPuestoNuevo', visible: false },
                    { data: 'personalExistente', visible: false },
                    { data: 'personalRequerido', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_GestionSolicitudes.on("click", ".crearPuesto", function () {
                        let rowData = dtGestionSolicitudes.row($(this).closest('tr')).data();
                        document.location.href =
                            `/Administrativo/AditivaPersonal/AltaAditiva?cc=${rowData.cc}&&puesto=${rowData.puesto}&&cantVacantes=${rowData.cantVacantes}&&idSolicitud=${rowData.idSolicitud}&&esPuestoNuevo=${rowData.esPuestoNuevo}&&idPuesto=${rowData.idPuesto}&&personalExistente=${rowData.personalExistente}`;
                    });

                    tblRH_REC_GestionSolicitudes.on('click', '.verDetalles', function () {
                        let rowData = dtGestionSolicitudes.row($(this).closest('tr')).data();
                        chkCrearEditarPuesto.attr("disabled", false);
                        chkCrearEditarPuesto.prop("checked", rowData.esPuestoNuevo);
                        chkCrearEditarPuesto.trigger("change");
                        chkCrearEditarPuesto.attr("disabled", true);
                        cboCrearEditarCC.val(rowData.cc);
                        cboCrearEditarCC.trigger("change");
                        cboCrearEditarPuesto.val(rowData.idPuesto);
                        txtCrearEditarPuesto.val(rowData.puesto);
                        cboCrearEditarPuesto.trigger("change");
                        cboCrearEditarMotivo.val(rowData.idMotivo);
                        cboCrearEditarMotivo.trigger("change");
                        cboCrearEditarSexo.val(rowData.sexo);
                        cboCrearEditarSexo.trigger("change");
                        txtCrearEditarInicioEdad.val(rowData.rangoInicioEdad);
                        txtCrearEditarFinEdad.val(rowData.rangoFinEdad);
                        cboCrearEditarEscolaridad.val(rowData.idEscolaridad);
                        cboCrearEditarEscolaridad.trigger("change");
                        cboCrearEditarPais.val(rowData.clave_pais_nac);
                        cboCrearEditarPais.trigger("change");
                        cboCrearEditarEstado.val(rowData.clave_estado_nac);
                        cboCrearEditarEstado.trigger("change");
                        cboCrearEditarMunicipio.val(rowData.clave_ciudad_nac);
                        cboCrearEditarMunicipio.trigger("change");
                        txtCrearEditarAniosExp.val(rowData.aniosExp);
                        txtCrearEditarConocimientosGen.val(rowData.conocimientoGen);
                        txtCrearEditarExpEspecializada.val(rowData.expEspecializada);
                        txtCrearEditarCantVacantes.val(rowData.cantVacantes);
                        mdlCrearEditarSolicitud.modal("show");
                    });

                    tblRH_REC_GestionSolicitudes.on('click', '.rechazarSolicitud', function () {
                        let rowData = dtGestionSolicitudes.row($(this).closest('tr')).data();
                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "Rechazar solicitud",
                            input: 'textarea',
                            width: '50%',
                            showCancelButton: true,
                            html: "<h3>¿Desea rechazar la solicitud seleccionada?<br>Indicar el motivo:</h3>",
                            confirmButtonText: "Rechazar",
                            confirmButtonColor: "#d9534f",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#5c636a",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                fncRechazarSolicitud(rowData.idSolicitud, $('.swal2-textarea').val());
                            }
                        });
                    });

                    tblRH_REC_GestionSolicitudes.on('click', '.motivoRechazo', function () {
                        let rowData = dtGestionSolicitudes.row($(this).closest('tr')).data();
                        txtMotivoRechazoSolicitud.val(rowData.motivoRechazo);
                        mdlMotivoRechazo.modal("show");
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': [0, 1, 2, 3, 4, 5, 6, 7] },
                    { "width": "5%", "targets": 0 },
                    { "width": "5%", "targets": 1 },
                    { "width": "5%", "targets": 2 },
                    { "width": "25%", "targets": 3 },
                    { "width": "25%", "targets": 4 },
                    { "width": "5%", "targets": 5 },
                    { "width": "20%", "targets": 6 },
                    { "width": "10%", "targets": 7 }
                ],
            });
        }

        function fncDeshabilitarCtrlsMdl() {
            // chkCrearEditarPuesto.attr("disabled", true);
            cboCrearEditarCC.attr("disabled", true);
            cboCrearEditarPuesto.attr("disabled", true);
            txtCrearEditarPuesto.attr("disabled", true);
            cboCrearEditarMotivo.attr("disabled", true);
            cboCrearEditarSexo.attr("disabled", true);
            txtCrearEditarInicioEdad.attr("disabled", true);
            txtCrearEditarFinEdad.attr("disabled", true);
            cboCrearEditarEscolaridad.attr("disabled", true);
            cboCrearEditarPais.attr("disabled", true);
            cboCrearEditarEstado.attr("disabled", true);
            cboCrearEditarMunicipio.attr("disabled", true);
            txtCrearEditarAniosExp.attr("disabled", true);
            txtCrearEditarConocimientosGen.attr("disabled", true);
            txtCrearEditarExpEspecializada.attr("disabled", true);
            txtCrearEditarCantVacantes.attr("disabled", true);
        }

        function fncRechazarSolicitud(idSolicitud, motivoRechazo) {
            if (motivoRechazo != "") {
                let objSolicitud = fncObjMotivoRechivo(idSolicitud, motivoRechazo);
                axios.post("RechazarSolicitud", objSolicitud).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetGestionSolicitudes();
                        Alert2Exito("Éxito al rechazar la solicitud.");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario indicar el motivo.");
            }
        }

        function fncObjMotivoRechivo(idSolicitud, motivoRechazo) {
            let objSolicitud = new Object();
            objSolicitud = {
                idSolicitud: idSolicitud,
                motivoRechazo: motivoRechazo != "" ? motivoRechazo : Alert2Warning()
            }
            return objSolicitud;
        }

        function fncGetGestionSolicitudes() {
            let objFiltro = fncGetFiltros();
            axios.post("GetGestionSolicitudes", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtGestionSolicitudes.clear();
                    dtGestionSolicitudes.rows.add(response.data.lstGestionSolicitudes);
                    dtGestionSolicitudes.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetFiltros() {
            //#region SE CREA EL OBJETO PARA LA BUSQUEDA CON FILTRO
            let strFiltro = $('select[id="cboFiltroPuesto"] option:selected').text();
            let objFiltro = new Object();
            objFiltro = {
                cc: cboFiltroCC.val(),
                idPuesto: cboFiltroPuesto.val(),
                puesto: strFiltro == "--Seleccione--" ? "" : strFiltro,
                esAutorizada: chkFiltroAutorizada.prop("checked")
            };
            return objFiltro;
            //#endregion
        }
    }

    $(document).ready(() => {
        CH.GestionReclutamientos = new GestionReclutamientos();
    })

        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();