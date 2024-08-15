(() => {
    $.namespace('CH.Reclutamientos');

    //#region CONST FILTROS SOLICITUDES
    const cboFiltroCC = $('#cboFiltroCC');
    const cboFiltroPuesto = $('#cboFiltroPuesto');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroLimpiar = $('#btnFiltroLimpiar');
    const btnFiltroSolicitudesConclusas = $('#btnFiltroSolicitudesConclusas');
    const btnFiltroNuevaSolicitud = $('#btnFiltroNuevaSolicitud');
    const mdlCrearEditarSolicitud = $('#mdlCrearEditarSolicitud');
    const tblRH_REC_Solicitudes = $('#tblRH_REC_Solicitudes');
    let dtSolicitudes;
    //#endregion

    //#region CONST CREAR/EDITAR SOLICITUDES
    const chkCrearEditarPuesto = $('#chkCrearEditarPuesto');
    const chkCrearEditarGeneral = $('#chkCrearEditarGeneral');
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
    const btnCrearEditarSolicitud = $('#btnCrearEditarSolicitud');
    const spanCrearEditarSolicitud = $('#spanCrearEditarSolicitud');
    const spanTitleCrearEditarSolicitud = $('#spanTitleCrearEditarSolicitud');
    const divCboCrearEditarPuesto = $('#divCboCrearEditarPuesto');
    const divTxtCrearEditarPuesto = $('#divTxtCrearEditarPuesto');
    const cboCrearEditarDepartamento = $('#cboCrearEditarDepartamento');
    const lblPuestosCategorias = $('#lblPuestosCategorias');
    const txtCrearEditarCategoriasPuesto = $('#txtCrearEditarCategoriasPuesto');
    const txtCrearEditarSolicitados = $('#txtCrearEditarSolicitados');
    const txtCrearEditarFaltante = $('#txtCrearEditarFaltante');
    const divCrearEditarCaja = $('#divCrearEditarCaja');
    const btnCrearPuesto = $('#btnCrearPuesto');
    const divCrearEditarEsDisp = $('#divCrearEditarEsDisp');
    const divCrearEditarNoEsDisp = $('#divCrearEditarNoEsDisp');
    const txtFechaCreacionSolicitud = $('#txtFechaCreacionSolicitud');
    const txtFechaAltaUltimaVacante = $('#txtFechaAltaUltimaVacante');
    //#endregion

    Reclutamientos = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region EVENTOS SOLICITUDES

            initTblSolicitudes();
            fncCtrlsPuestos();
            fncGetSolicitudes(false);

            //#region ABRIR MODAL CREAR/EDITAR SOLICITUD
            btnFiltroNuevaSolicitud.on("click", function () {
                // cboCrearEditarPuesto.attr("disabled", true);
                divCrearEditarCaja.css("border-color", "#ffffff");
                txtCrearEditarSolicitados.text("0");
                txtCrearEditarFaltante.text("0");
                btnCrearPuesto.hide();
                divCrearEditarCaja.show();
                divCrearEditarNoEsDisp.hide();
                fncHabilitarDeshabilitarCtrlsMdl(false);
                fncBorderDefault();

                fncLimpiarCtrlsMdl();
                btnCrearEditarSolicitud.attr("data-id", 0);
                spanCrearEditarSolicitud.html("Guardar");
                spanTitleCrearEditarSolicitud.html("SOLICITUD DE VACANTE");
                mdlCrearEditarSolicitud.modal("show");
                txtFechaCreacionSolicitud.val(moment(Date.now()).format("DD/MM/YYYY"));
                fncCtrlsPuestos();

                fncFocus("txtFechaCreacionSolicitud");
            });
            //#endregion

            //#region FILL COMBOS
            fncFillFiltroCbos();
            cboCrearEditarCC.fillCombo("/Reclutamientos/FillCboCC", {}, false);
            cboCrearEditarMotivo.fillCombo("/Reclutamientos/FillCboMotivos", {}, false);
            cboCrearEditarEscolaridad.fillCombo("/Reclutamientos/FillCboEscolaridades", {}, false);

            cboCrearEditarPais.fillCombo("/Reclutamientos/FillCboPaises", {}, false);
            cboCrearEditarPais.on("change", function () {
                if ($(this).val() != "") {
                    cboCrearEditarEstado.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: $(this).val() }, false);
                }
                fncFocus("cboCrearEditarPais");
            });
            cboCrearEditarEstado.on("change", function () {
                if ($(cboCrearEditarPais).val() != "" && $(this).val() != "") {
                    cboCrearEditarMunicipio.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: cboCrearEditarPais.val(), _claveEstado: $(this).val() }, false);
                }
                fncFocus("cboCrearEditarEstado");
            });
            cboCrearEditarCC.on("change", function () {
                cboCrearEditarDepartamento.fillCombo("/Reclutamientos/FillCboDepartamentos", { cc: cboCrearEditarCC.val() }, false);
            });

            cboCrearEditarCC.on("change", function () {
                if (cboCrearEditarCC.val()) {
                    cboCrearEditarPuesto.fillCombo("/Reclutamientos/FillCboPuestosSolicitudes", { cc: cboCrearEditarCC.val() }, false);
                    cboCrearEditarPuesto.attr("disabled", false);
                }
                fncFocus("cboCrearEditarCC");
            });

            cboCrearEditarPuesto.on("change", function () {
                fncGetPuestosCategorias();
                if ($(this).val() != "--Seleccione--" && $(this).val() != "") {
                    if (cboCrearEditarCC.val() != "--Seleccione--" && cboCrearEditarCC.val() != "") {
                        fncGetPlantillas();
                    }
                }
                fncFocus("cboCrearEditarPuesto");
            });

            txtCrearEditarCantVacantes.on("keyup", function () {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }

            });

            cboCrearEditarMotivo.on("change", function () {
                fncFocus("cboCrearEditarMotivo");
            });

            cboCrearEditarSexo.on("change", function () {
                fncFocus("cboCrearEditarSexo");
            });

            cboCrearEditarEscolaridad.on("change", function () {
                fncFocus("cboCrearEditarEscolaridad");
            });

            cboCrearEditarMunicipio.on("change", function () {
                fncFocus("cboCrearEditarMunicipio");
            });
            //#endregion

            //#region CONVERT TO SELECT2
            cboFiltroCC.select2({ width: 'resolve' });
            cboFiltroPuesto.select2({ width: 'resolve' });
            cboCrearEditarCC.select2({ width: 'resolve' });
            cboCrearEditarPuesto.select2({ width: 'resolve' });
            cboCrearEditarMotivo.select2({ width: 'resolve' });
            cboCrearEditarSexo.select2({ width: 'resolve' });
            cboCrearEditarEscolaridad.select2({ width: 'resolve' });
            cboCrearEditarPais.select2({ width: 'resolve' });
            cboCrearEditarEstado.select2({ width: 'resolve' });
            cboCrearEditarMunicipio.select2({ width: 'resolve' });
            cboCrearEditarDepartamento.select2({ width: 'resolve' });
            //#endregion

            //#region SE OBTIENE LAS SOLICITUDES EN BASE A LOS FILTROS SELECCIONADOS
            btnFiltroBuscar.on("click", function () {
                fncGetSolicitudes(false);
            });
            //#endregion

            //#region SE LIMPIAN LOS FILTROS
            btnFiltroLimpiar.on("click", function () {
                cboFiltroCC[0].selectedIndex = 0;
                cboFiltroCC.trigger("change");
                cboFiltroPuesto[0].selectedIndex = 0;
                cboFiltroPuesto.trigger("change");
            });
            //#endregion

            //#region SE CREA/ACTUALIZA LA SOLICITUD
            btnCrearEditarSolicitud.on("click", function () {
                fncCrearEditarSolicitud();
            });
            //#endregion

            //#region SE CAMBIA DE SELECT A INPUT Y VICEVERSA EN BASE AL CHECKBOX SUPERIOR IZQUIERDO DEL MODAL CREAR/EDITAR SOLICITUD
            chkCrearEditarPuesto.on("change", function () {
                fncCtrlsPuestos();
            });
            //#endregion

            //#region SE OBTIENE LISTADO DE SOLICITUDES CONCLUSAS
            btnFiltroSolicitudesConclusas.on("click", function () {
                fncGetSolicitudes(true);
            });
            //#endregion

            //#region FUNCIONES GENERALES
            function fncFocus(obj) {
                if (obj != "") {
                    setTimeout(() => $(`#${obj}`).focus(), 50);
                }
            }
            //#endregion
            //#endregion

            btnCrearPuesto.on("click", function () {
                document.location.href =
                    `/Administrativo/AditivaPersonal/AltaAditiva?cc=${cboCrearEditarCC.val()}&&puesto=${cboCrearEditarPuesto.val()}&&cantVacantes=${0}&&idSolicitud=${0}&&esPuestoNuevo=${!chkCrearEditarPuesto.prop("checked")}&&idPuesto=${cboCrearEditarPuesto.val()}&&personalExistente=${0}`;
            })
        }

        function fncFillFiltroCbos() {
            cboFiltroCC.fillCombo("/Reclutamientos/FillFiltroCboCC", {}, false);
            cboFiltroPuesto.fillCombo("/Reclutamientos/FillFiltroCboPuestos", {}, false);
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

        //#region CRUD SOLICITUDES
        function initTblSolicitudes() {
            dtSolicitudes = tblRH_REC_Solicitudes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    {
                        title: 'Folio',
                        data: 'id'
                    },
                    {
                        title: 'Vacantes',
                        render: function (data, type, row) {
                            return `${row.cantVacantesCubiertas} de ${row.cantVacantes}`;
                        }
                    },
                    { data: 'ccDescripcion', title: 'CC' },
                    { data: 'puesto', title: 'Puesto' },
                    {
                        title: 'Rango edad', visible: true,
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
                        title: 'Fecha de captura',
                        render: function (data, type, row) {
                            return moment(row.fechaCreacion).format('DD/MM/YYYY');
                            // return new Date(parseInt(row.fechaCreacion.replace(/[^0-9|\-|\+]+/g, ''))).toLocaleDateString();
                        }
                    },
                    {
                        title: 'Opciones',
                        render: function (data, type, row) {
                            let btnVerDetalles = `<button class="btn btn-primary verDetalles btn-xs" title="VER DETALLES"><i class="fas fa-list"></i></button>&nbsp;`;
                            let btnActualizar = `<button class="btn btn-warning actualizarSolicitud btn-xs" title="EDITAR"><i class="far fa-edit"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-danger eliminarSolicitud btn-xs" title="ELIMINAR"><i class="far fa-trash-alt"></i></button>`;

                            if (row.fechaAltaUltimaVacante == null) {
                                return btnVerDetalles + btnActualizar + btnEliminar;
                            } else {
                                return btnVerDetalles;
                            }
                        }
                    },
                    { data: 'id', visible: false },
                    { data: 'cc', visible: false },
                    { data: 'idPuesto', visible: false },
                    { data: 'motivo', visible: false },
                    { data: 'sexo', visible: false },
                    { data: 'rangoInicioEdad', visible: false },
                    { data: 'rangoFinEdad', visible: false },
                    { data: 'escolaridad', visible: false },
                    { data: 'pais', visible: false },
                    { data: 'estado', visible: false },
                    { data: 'ciudad', visible: false },
                    { data: 'clave_pais_nac', visible: false },
                    { data: 'clave_estado_nac', visible: false },
                    { data: 'clave_ciudad_nac', visible: false },
                    { data: 'aniosExp', visible: false },
                    { data: 'conocimientoGen', visible: false },
                    { data: 'expEspecializada', visible: false },
                    { data: 'cantVacantes', visible: false },
                    { data: 'cantVacantesCubiertas', visible: false },
                    { data: 'esPuestoNuevo', visible: false },
                    { data: 'esGeneral', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_Solicitudes.on('click', '.verDetalles', function () {
                        let rowData = dtSolicitudes.row($(this).closest('tr')).data();
                        cboCrearEditarCC.val(rowData.cc);
                        cboCrearEditarCC.trigger("change");
                        cboCrearEditarPuesto.val(rowData.idPuesto);
                        cboCrearEditarPuesto.trigger("change");
                        txtCrearEditarPuesto.val(rowData.puesto);
                        chkCrearEditarGeneral.prop("checked", rowData.esGeneral);
                        chkCrearEditarGeneral.trigger("change");
                        //chkCrearEditarGeneral.prop('disabled', true);
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
                        chkCrearEditarPuesto.prop("checked", rowData.esPuestoNuevo);
                        // cboCrearEditarDepartamento.val(rowData.clave_depto);
                        // cboCrearEditarDepartamento.trigger("change");
                        btnCrearEditarSolicitud.attr("data-id", rowData.id);
                        fncHabilitarDeshabilitarCtrlsMdl(true);
                        spanTitleCrearEditarSolicitud.html("SOLICITUD DE VACANTE");
                        txtFechaCreacionSolicitud.val(moment(rowData.fechaCreacion).format("DD/MM/YYYY"));
                        if (rowData.solicitudesConclusas) {
                            txtFechaAltaUltimaVacante.val(moment(rowData.fechaAltaUltimaVacante).format("DD/MM/YYYY"));
                        }
                        mdlCrearEditarSolicitud.modal("show");
                    });

                    tblRH_REC_Solicitudes.on('click', '.actualizarSolicitud', function () {
                        let rowData = dtSolicitudes.row($(this).closest('tr')).data();
                        chkCrearEditarPuesto.prop("checked", rowData.esPuestoNuevo);
                        chkCrearEditarPuesto.trigger("change");
                        chkCrearEditarGeneral.prop("checked", rowData.esGeneral);
                        chkCrearEditarGeneral.trigger("change");
                        //chkCrearEditarGeneral.prop('disabled', false);
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
                        // cboCrearEditarDepartamento.val(rowData.clave_depto);
                        cboCrearEditarDepartamento.trigger("change");
                        btnCrearEditarSolicitud.attr("data-id", rowData.id);
                        fncHabilitarDeshabilitarCtrlsMdl(false);
                        spanCrearEditarSolicitud.html("Actualizar");
                        spanTitleCrearEditarSolicitud.html("SOLICITUD DE VACANTE");
                        mdlCrearEditarSolicitud.modal("show");
                        fncBorderDefault();
                    });

                    tblRH_REC_Solicitudes.on('click', '.eliminarSolicitud', function () {
                        let rowData = dtSolicitudes.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarSolicitud(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { "width": "2%", "targets": 0 },
                    { "width": "3%", "targets": 1 },
                    { "width": "25%", "targets": 2 },
                    { "width": "25%", "targets": 3 },
                    { "width": "5%", "targets": 4 },
                    { "width": "20%", "targets": 5 },
                    { "width": "10%", "targets": 6 },
                    { "width": "10%", "targets": 7 }
                ],
            });
        }

        function fncLimpiarCtrlsMdl() {
            cboCrearEditarCC[0].selectedIndex = 0;
            cboCrearEditarCC.trigger("change");
            cboCrearEditarPuesto[0].selectedIndex = 0;
            cboCrearEditarPuesto.trigger("change");
            cboCrearEditarMotivo[0].selectedIndex = 0;
            cboCrearEditarMotivo.trigger("change");
            cboCrearEditarSexo[0].selectedIndex = 0;
            cboCrearEditarSexo.trigger("change");
            cboCrearEditarEscolaridad[0].selectedIndex = 0;
            cboCrearEditarEscolaridad.trigger("change");
            cboCrearEditarPais[0].selectedIndex = 0;
            cboCrearEditarPais.trigger("change");
            cboCrearEditarEstado[0].selectedIndex = 0;
            cboCrearEditarEstado.trigger("change");
            cboCrearEditarMunicipio[0].selectedIndex = 0;
            cboCrearEditarMunicipio.trigger("change");

            txtCrearEditarConocimientosGen.val("");
            txtCrearEditarExpEspecializada.val("");

            btnCrearEditarSolicitud.css("display", "inline");

            $('input[type="text"]').val('');
        }

        function fncHabilitarDeshabilitarCtrlsMdl(esDeshabilitar) {
            if (!esDeshabilitar) {
                chkCrearEditarGeneral.attr("disabled", false);
                chkCrearEditarGeneral.closest('div').attr('disabled', false);
                cboCrearEditarCC.attr("disabled", false);
                txtCrearEditarPuesto.attr("disabled", false);
                cboCrearEditarMotivo.attr("disabled", false);
                cboCrearEditarSexo.attr("disabled", false);
                txtCrearEditarInicioEdad.attr("disabled", false);
                txtCrearEditarFinEdad.attr("disabled", false);
                cboCrearEditarEscolaridad.attr("disabled", false);
                cboCrearEditarPais.attr("disabled", false);
                cboCrearEditarEstado.attr("disabled", false);
                cboCrearEditarMunicipio.attr("disabled", false);
                txtCrearEditarAniosExp.attr("disabled", false);
                txtCrearEditarConocimientosGen.attr("disabled", false);
                txtCrearEditarExpEspecializada.attr("disabled", false);
                txtCrearEditarCantVacantes.attr("disabled", false);
                btnCrearEditarSolicitud.css("display", "inline");
                cboCrearEditarDepartamento.attr('disabled', false);
            } else {
                chkCrearEditarGeneral.attr("disabled", true);
                chkCrearEditarGeneral.closest('div').attr('disabled', true);
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
                btnCrearEditarSolicitud.css("display", "none");
                cboCrearEditarDepartamento.attr('disabled', true);
            }
        }

        function fncGetSolicitudes(solicitudesConclusas) {
            let objFiltro = fncGetFiltros(solicitudesConclusas);
            axios.post("GetSolicitudes", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region SE LLENA LA TABLA DE SOLICITUDES
                    dtSolicitudes.clear();
                    dtSolicitudes.rows.add(response.data.lstSolicitudes);
                    dtSolicitudes.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetFiltros(solicitudesConclusas) {
            //#region SE CREA EL OBJETO PARA LA BUSQUEDA CON FILTRO
            let strFiltro = $('select[id="cboFiltroPuesto"] option:selected').text();
            let objFiltro = new Object();
            objFiltro = {
                cc: cboFiltroCC.val(),
                idPuesto: cboFiltroPuesto.val(),
                puesto: strFiltro == "--Seleccione--" ? "" : strFiltro,
                solicitudesConclusas: solicitudesConclusas
            };
            return objFiltro;
            //#endregion
        }

        function fncCrearEditarSolicitud() {
            let objSolicitud = fncGetDataCrearEditarSolicitud();
            if (objSolicitud != "") {
                axios.post("CrearEditarSolicitud", objSolicitud).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetSolicitudes(false);
                        mdlCrearEditarSolicitud.modal("hide");
                        let strMensaje = btnCrearEditarSolicitud.attr("data-id") > 0 ? "Se ha actualiza con éxito la solicitud." : "Se ha registrado con éxito la solicitud.";
                        fncFillFiltroCbos();
                        Alert2Exito(strMensaje);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetDataCrearEditarSolicitud() {
            //#region SE CREA EL OBJETO PARA CREAR/EDITAR LA SOLICITUD
            fncBorderDefault();
            let strMensajeError = "";

            if (chkCrearEditarPuesto.prop("checked")) {
                if (txtCrearEditarPuesto.val() == "") { txtCrearEditarPuesto.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            } else {
                if (cboCrearEditarPuesto.val() == "") { $("#select2-cboCrearEditarPuesto-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            }
            if (cboCrearEditarCC.val() == "") { $("#select2-cboCrearEditarCC-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboCrearEditarDepartamento.val() == "") { $("#select2-cboCrearEditarDepartamento-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboCrearEditarMotivo.val() == "") { $("#select2-cboCrearEditarMotivo-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboCrearEditarEscolaridad.val() == "") { $("#select2-cboCrearEditarEscolaridad-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboCrearEditarPais.val() == "") { $("#select2-cboCrearEditarPais-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboCrearEditarEstado.val() == "") { $("#select2-cboCrearEditarEstado-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboCrearEditarMunicipio.val() == "") { $("#select2-cboCrearEditarMunicipio-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCrearEditarCantVacantes.val() == "") { txtCrearEditarCantVacantes.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            txtCrearEditarCantVacantes.trigger("keyup");


            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let objSolicitud = new Object();
                objSolicitud = {
                    id: btnCrearEditarSolicitud.attr("data-id"),
                    cc: cboCrearEditarCC.val(),
                    idPuesto: cboCrearEditarPuesto.val(),
                    puesto: txtCrearEditarPuesto.val(),
                    // clave_depto: cboCrearEditarDepartamento.val(),
                    esGeneral: chkCrearEditarGeneral.prop("checked"),
                    idMotivo: cboCrearEditarMotivo.val(),
                    sexo: cboCrearEditarSexo.val(),
                    rangoInicioEdad: txtCrearEditarInicioEdad.val(),
                    rangoFinEdad: txtCrearEditarFinEdad.val(),
                    idEscolaridad: cboCrearEditarEscolaridad.val(),
                    clave_pais_nac: cboCrearEditarPais.val(),
                    clave_estado_nac: cboCrearEditarEstado.val(),
                    clave_ciudad_nac: cboCrearEditarMunicipio.val(),
                    aniosExp: txtCrearEditarAniosExp.val(),
                    conocimientoGen: txtCrearEditarConocimientosGen.val(),
                    expEspecializada: txtCrearEditarExpEspecializada.val(),
                    cantVacantes: txtCrearEditarCantVacantes.val(),
                    esPuestoNuevo: chkCrearEditarPuesto.prop("checked")
                }

                if (txtCrearEditarPuesto.val() == "") {
                    objSolicitud.puesto = cboCrearEditarPuesto[0][cboCrearEditarPuesto[0].selectedIndex].text;
                }

                return objSolicitud;
            }
            //#endregion
        }

        function fncBorderDefault() {
            txtCrearEditarPuesto.css('border', '1px solid #CCC');
            $("#select2-cboCrearEditarPuesto-container").css('border', '1px solid #CCC');
            $("#select2-cboCrearEditarCC-container").css('border', '1px solid #CCC');
            $("#select2-cboCrearEditarDepartamento-container").css('border', '1px solid #CCC');
            $("#select2-cboCrearEditarMotivo-container").css('border', '1px solid #CCC');
            $("#select2-cboCrearEditarSexo-container").css('border', '1px solid #CCC');
            $("#select2-cboCrearEditarEscolaridad-container").css('border', '1px solid #CCC');
            $("#select2-cboCrearEditarPais-container").css('border', '1px solid #CCC');
            $("#select2-cboCrearEditarEstado-container").css('border', '1px solid #CCC');
            $("#select2-cboCrearEditarMunicipio-container").css('border', '1px solid #CCC');
            txtCrearEditarCantVacantes.css('border', '1px solid #CCC');
        }

        function fncEliminarSolicitud(idSolicitud) {
            //#region SE ELIMINA LA SOLICITUD
            let objSolicitud = new Object();
            objSolicitud = {
                idSolicitud: idSolicitud
            }
            axios.post("EliminarSolicitud", objSolicitud).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetSolicitudes(false);
                    fncFillFiltroCbos();
                    Alert2Exito("Se ha eliminado con éxito el registro.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
            //#endregion
        }

        function fncGetPuestosCategorias() {
            let obj = new Object();
            obj = {
                _cc: cboCrearEditarCC.val(),
                _puesto: $('select[id="cboCrearEditarPuesto"] option:selected').text()
            }
            axios.post("GetCategoriasRelPuesto", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let strCategorias = "";
                    let input = ``;
                    for (let i = 0; i < response.data.lstCategoriasRelPuesto.length; i++) {
                        strCategorias += " " + response.data.lstCategoriasRelPuesto[i] + " ";
                        input += `<div class="col-lg-1"><input type="text" class="form-control" disabled="disabled" value="${response.data.lstCategoriasRelPuesto[i]}"></div>`;
                    }

                    // if (response.data.lstCategoriasRelPuesto.length > 0) {
                    //     lblPuestosCategorias.html(`Categorías: ${strCategorias}`);
                    //     txtCrearEditarCategoriasPuesto.val(strCategorias);
                    // } else {
                    //     lblPuestosCategorias.html("");
                    //     txtCrearEditarCategoriasPuesto.val("");
                    // }
                    lblPuestosCategorias.html(input);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetPlantillas() {
            axios.post("GetPlantilla", { cc: cboCrearEditarCC.val(), puesto: cboCrearEditarPuesto.val() }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    console.log(items);
                    if (response.data.success) {
                        if (response.data.items.length > 0) {
                            divCrearEditarCaja.show();
                            divCrearEditarNoEsDisp.hide();
                            if (response.data.items[0].faltantes >= 1) {
                                divCrearEditarCaja.css("border-color", "#5cb85c");
                                btnCrearPuesto.hide();

                            } else {
                                divCrearEditarCaja.css("border-color", "#d43f3a");
                                btnCrearPuesto.show();
                            }
                            txtCrearEditarSolicitados.text(response.data.items[0].solicitados);
                            txtCrearEditarFaltante.text(response.data.items[0].faltantes);
                        } else {
                            btnCrearPuesto.show();
                            divCrearEditarCaja.hide();
                            divCrearEditarNoEsDisp.show();
                        }
                    }

                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Reclutamientos = new Reclutamientos();
    })

        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();