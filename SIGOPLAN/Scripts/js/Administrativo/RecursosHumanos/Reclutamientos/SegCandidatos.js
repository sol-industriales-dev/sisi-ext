(() => {
    $.namespace('CH.SegCandidatos');

    //#region CONST FILTRO
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroRegistrarEmpleado = $('#btnFiltroRegistrarEmpleado');
    const txtFiltroEstatus = $('#txtFiltroEstatus');
    const cboFiltroCC = $('#cboFiltroCC');
    //#endregion

    //#region CONST SEGUIMIENTO CANDIDATOS
    const divFases = $('#divFases');
    const divSeguimientoCandidatos = $('#divSeguimientoCandidatos');
    const tblRH_REC_SegCandidatos = $('#tblRH_REC_SegCandidatos');
    let dtSegCandidatos;
    //#endregion

    //#region CONST FASES
    const divSetFases = $('#divSetFases');
    const btnRegresarSegCandidatos = $('#btnRegresarSegCandidatos');
    const tblRH_REC_Actividades = $('#tblRH_REC_Actividades');
    let dtActividades;
    let contadorFasesContruccion = 0;
    let contadorFases = 0;
    //#endregion

    //#region CONST ENTREVISTA INICIAL
    const mdlCrearEditarEntrevistaInicial = $('#mdlCrearEditarEntrevistaInicial');
    const txtCEEntrevistaInicialNombreCompleto = $('#txtCEEntrevistaInicialNombreCompleto');
    const cboCEEntrevistaInicialEscolaridad = $('#cboCEEntrevistaInicialEscolaridad');
    const txtCEEntrevistaInicialEdad = $('#txtCEEntrevistaInicialEdad');
    const cboCEEntrevistaInicialEstadoCivil = $('#cboCEEntrevistaInicialEstadoCivil');
    const txtCEEntrevistaInicialLugarNacimiento = $('#txtCEEntrevistaInicialLugarNacimiento');
    const txtCEEntrevistaInicialExpectativaSalarial = $('#txtCEEntrevistaInicialExpectativaSalarial');
    const txtCEEntrevistaInicialPuestoSolicitado = $('#txtCEEntrevistaInicialPuestoSolicitado');
    const txtCEEntrevistaInicialExperienciaLaboral = $('#txtCEEntrevistaInicialExperienciaLaboral');
    const txtCEEntrevistaInicialSectorCiudad = $('#txtCEEntrevistaInicialSectorCiudad');
    const txtCEEntrevistaInicialTiempoEnLaCiudad = $('#txtCEEntrevistaInicialTiempoEnLaCiudad');
    const chkCEEntrevistaInicialEntrevistasAnteriores = $('#chkCEEntrevistaInicialEntrevistasAnteriores');
    const cboCEEntrevistaInicialPlataforma = $('#cboCEEntrevistaInicialPlataforma');
    const cboCEEntrevistaInicialDocumentacion = $('#cboCEEntrevistaInicialDocumentacion');
    const chkCEEntrevistaInicialFamiliarEnLaEmpresa = $('#chkCEEntrevistaInicialFamiliarEnLaEmpresa');
    const txtCEEntrevistaInicialTelefono = $('#txtCEEntrevistaInicialTelefono');
    const txtCEEntrevistaInicialFamilia = $('#txtCEEntrevistaInicialFamilia');
    const txtCEEntrevistaInicialEmpleos = $('#txtCEEntrevistaInicialEmpleos');
    const txtCEEntrevistaInicialCaracteristicasPersonalesCandidato = $('#txtCEEntrevistaInicialCaracteristicasPersonalesCandidato');
    const txtCEEntrevistaInicialComentariosEntrevistador = $('#txtCEEntrevistaInicialComentariosEntrevistador');
    const txtCEEntrevistaInicialFechaEntrevista = $('#txtCEEntrevistaInicialFechaEntrevista');
    const txtCEEntrevistaInicialFamiliaEnLaEmpresa = $('#txtCEEntrevistaInicialFamiliaEnLaEmpresa');
    const divFamiliaEnLaEmpresa = $('#divFamiliaEnLaEmpresa');
    const chkCEEntrevistaInicialDisposicionHorario = $('#chkCEEntrevistaInicialDisposicionHorario');
    const chkCEEntrevistaInicialAvanza = $('#chkCEEntrevistaInicialAvanza');
    const cboCEEntrevistaInicialEntrevisto = $('#cboCEEntrevistaInicialEntrevisto');
    const txtCEEntrevistaInicialResultado = $('#txtCEEntrevistaInicialResultado');
    const btnCEEntrevistaInicial = $('#btnCEEntrevistaInicial');
    const spanTitleBtnCEEntrevistaInicial = $('#spanTitleBtnCEEntrevistaInicial');
    const chkCEEntrevistaInicialBrutoNeto = $('#chkCEEntrevistaInicialBrutoNeto');
    // const txtCEEntrevistaInicialDisposicionHorario = $('#txtCEEntrevistaInicialDisposicionHorario');
    const divCEEntrevistaInicialAvanza = $('#divCEEntrevistaInicialAvanza');
    const txtCEEntrevistaInicialComentariosAvanza = $('#txtCEEntrevistaInicialComentariosAvanza');
    //#endregion


    const mdlVerObservacion = $('#mdlVerObservacion');
    const btnCEObservacionActividad = $('#btnCEObservacionActividad');
    const txtCEObservacionActividad = $('#txtCEObservacionActividad');
    const mdlVerEvidencias = $('#mdlVerEvidencias');
    const btnCrearArchivoActividad = $('#btnCrearArchivoActividad');
    const mdlVerCalificacion = $('#mdlVerCalificacion');
    const tblRH_REC_Evidencias = $('#tblRH_REC_Evidencias');

    //#region MDL SEND NOTIFICANTE
    const mdlSendNotificante = $('#mdlSendNotificante');
    const btnSendEnviar = $('#btnSendEnviar');
    const txtSendClave = $('#txtSendClave');
    const txtSendNombre = $('#txtSendNombre');
    //#endregion

    //#region MODAL EXAMEN MÉDICO
    const modalExamenMedico = $('#modalExamenMedico');
    const botonGuardarExamenMedico = $('#botonGuardarExamenMedico');
    const inputNombre = $('#inputNombre');
    const inputEdad = $('#inputEdad');
    const inputTelefono = $('#inputTelefono');
    const inputDireccion = $('#inputDireccion');
    const selectEscolaridad = $('#selectEscolaridad');
    const inputOtraEscolaridad = $('#inputOtraEscolaridad');
    const selectEstadoCivil = $('#selectEstadoCivil');
    const inputOtroEstadoCivil = $('#inputOtroEstadoCivil');
    const inputHijos = $('#inputHijos');
    const selectPuestoAnterior = $('#selectPuestoAnterior');
    const selectPuestoOcupar = $('#selectPuestoOcupar');
    const selectAlcoholismo = $('#selectAlcoholismo');
    const inputTipoSanguineo = $('#inputTipoSanguineo');
    const inputObservacionesAlcoholismo = $('#inputObservacionesAlcoholismo');
    const selectTabaquismo = $('#selectTabaquismo');
    const inputObservacionesTabaquismo = $('#inputObservacionesTabaquismo');
    const selectToxicomania = $('#selectToxicomania');
    const inputObservacionesToxicomania = $('#inputObservacionesToxicomania');
    const selectLentes = $('#selectLentes');
    const inputObservacionesLentes = $('#inputObservacionesLentes');
    const inputVisual = $('#inputVisual');
    const inputAuditiva = $('#inputAuditiva');
    const inputTA = $('#inputTA');
    const inputPulso = $('#inputPulso');
    const inputMarchaPunta = $('#inputMarchaPunta');
    const inputTalon = $('#inputTalon');
    const inputRomberg = $('#inputRomberg');
    const inputArcosFlexion = $('#inputArcosFlexion');
    const inputAntecedentesFamiliares = $('#inputAntecedentesFamiliares');
    const inputHeredoFamiliar = $('#inputHeredoFamiliar');
    const selectAntecedentes = $('#selectAntecedentes');
    const inputTratamiento = $('#inputTratamiento');
    const inputRayosX = $('#inputRayosX');
    const inputMenarca = $('#inputMenarca');
    const inputVSA = $('#inputVSA');
    const inputNumeroGestas = $('#inputNumeroGestas');
    const inputRitmo = $('#inputRitmo');
    const inputMPF = $('#inputMPF');
    const inputPIE = $('#inputPIE');
    const inputOPI = $('#inputOPI');
    const inputMET = $('#inputMET');
    const inputCOC = $('#inputCOC');
    const inputAMP = $('#inputAMP');
    const inputTHC = $('#inputTHC');
    const textareaObservacionesGenerales = $('#textareaObservacionesGenerales');
    //#endregion

    let primeraCargaPagina = true;

    SegCandidatos = function () {
        (function init() {
            fncListeners();

            $('.select2').select2();
            convertToMultiselect('#selectAntecedentes');
            selectPuestoAnterior.fillCombo('FillCboPuestos', null, false, null);
            selectPuestoOcupar.fillCombo('FillCboPuestos', null, false, null);
        })();

        function fncListeners() {
            //#region INIT TABLAS
            initTblSegCandidatos();
            initTblActividadesFase();
            initTblEvidencias();
            //#endregion

            //#region SE OCULTA LAS FASES DEL CANDIDATO
            fncMostrarFases(false);
            //#endregion

            cboFiltroCC.fillCombo('FillComboCCUnique', null, false, 'Todos');
            convertToMultiselect('#cboFiltroCC');

            //#region EVENTS FASES
            btnRegresarSegCandidatos.on("click", function () {
                // Alert2AccionConfirmar('', '¿Desea regresar al seguimiento de candidatos?', 'Confirmar', 'Cancelar', () => fncGetSegCandidatos());
                fncMostrarFases(false);
                fncGetSegCandidatos();
            });

            $(document).on('click', '.btnFaseCandidato', function () {
                let idFase = $(this).data().id;
                $('.btnFaseCandidato').removeClass("active");
                $('.btnFaseCandidato').addClass("done");
                $(this).removeClass("done");
                $(this).addClass("active");
                fncGetActividadesFase(idFase);

                if (!primeraCargaPagina) {
                    $(`.lblFaseCandidato`).css("text-decoration", "none");
                    $(`#txt${idFase}`).css("text-decoration", "underline");
                }
            });

            btnCEObservacionActividad.on("click", function () {
                // btnCrearArchivoActividad.attr("data-idCandidato", btnRegresarSegCandidatos.attr("data-idCandidato"));
                // btnCrearArchivoActividad.attr("data-idFase", rowData.idFase);
                // btnCrearArchivoActividad.attr("data-idActividad", rowData.id);
                fncCEObservacionActividad(btnCrearArchivoActividad.attr("data-idCandidato"),
                    btnCrearArchivoActividad.attr("data-idFase"),
                    btnCrearArchivoActividad.attr("data-idActividad"),
                    txtCEObservacionActividad.val());
            });

            btnCrearArchivoActividad.on("click", function () {
                fncCrearArchivoActividad();
            });

            $(document).on("keyup", ".inputCalificacionActividad", function (e) {
                // let valCalActividad = row.find(`.inputCalificacionActividad`).val();
                // aceptaSoloNumero2D(valCalActividad, e);
                // console.log(valCalActividad);
            });

            txtSendNombre.getAutocomplete(funGetEmpleado, null, 'getCatUsuariosGeneral');

            btnSendEnviar.on("click", function () {
                btnSendEnviar.data("idNotific",);

                fncNoficarActividad(btnRegresarSegCandidatos.attr("data-idCandidato"), btnCrearArchivoActividad.attr("data-idActividad"), 0, txtSendClave.val())
                fncSetNotiEstatusActividad(btnRegresarSegCandidatos.attr("data-idCandidato"), btnCrearArchivoActividad.attr("data-idFase"), btnCrearArchivoActividad.attr("data-idActividad"));

                // if (txtSendNombre.val() != null) {
                //     fncActividadAprobada(
                //         btnCrearArchivoActividad.attr("data-idCandidato"),
                //         btnCrearArchivoActividad.attr("data-idFase"),
                //         btnCrearArchivoActividad.attr("data-idActividad"),
                //         btnCrearArchivoActividad.attr("data-Cali"),
                //         btnCrearArchivoActividad.attr("data-Coment"),
                //         txtSendClave.val()
                //     );
                //     // txtSendClave.val(0);
                // } else {
                //     Alert2Warning("Ingrese el nombre de un usuario");
                // }
            });

            $('.radioBtn a').on('click', function () {
                let seleccion = $(this).attr('apto');

                $(`a[data-toggle="radioApto"]`).not(`[apto="${seleccion}"]`).removeClass('active').addClass('notActive');
                $(`a[data-toggle="radioApto"][apto="${seleccion}"]`).removeClass('notActive').addClass('active');
            });
            botonGuardarExamenMedico.click(guardarExamenMedico);
            //#endregion

            //#region EVENTS SEGUIMIENTO CANDIDATOS
            //fncGetSegCandidatos();
            btnFiltroBuscar.on("click", function () {
                fncGetSegCandidatos();
            });

            btnFiltroRegistrarEmpleado.on("click", function () {
                document.location.href = `/Administrativo/Reclutamientos/AltaEmpleados?empleadosAdmn=0&cargarModal=${1}`;
            });
            //#endregion

            //#region ENTREVISTA
            cboCEEntrevistaInicialEscolaridad.fillCombo("/Reclutamientos/FillCboEscolaridades", {}, false, null);
            // cboCEEntrevistaInicialEscolaridad.select2({ width: "100%" });

            cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("--Seleccione--").text("--Seleccione--"));
            cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("Casado").text("Casado"));
            cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("Divorciado").text("Divorciado"));
            cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("Soltero").text("Soltero"));
            cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("Union Libre").text("Unión libre"));
            cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("Viudo").text("Viudo"));
            // cboCEEntrevistaInicialEstadoCivil.select2({ width: "100%" });

            cboCEEntrevistaInicialPlataforma.fillCombo("/Reclutamientos/FillCboPlataformas", {}, false);
            // cboCEEntrevistaInicialPlataforma.select2({ width: "100%" });

            cboCEEntrevistaInicialEntrevisto.fillCombo("/Reclutamientos/FillCboUsuarios", {}, false);
            // cboCEEntrevistaInicialEntrevisto.select2({ width: "100%" });
            //#endregion
        }

        function fncMostrarFases(display) {
            esDisplay = display ? "block" : "none";
            divFases.css("display", esDisplay);

            esDisplay = display ? "none" : "block";
            if (display) {
                divSeguimientoCandidatos.css("display", "none");
            } else {
                divSeguimientoCandidatos.css("display", "block");
            }
        }

        //#region PASOS/FASES
        function initTblActividadesFase() {
            dtActividades = tblRH_REC_Actividades.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'esObligatoria', title: 'Actividad<br>obligatoria',
                        render: function (data, type, row) {
                            if (data) {
                                return "SI";
                            } else {
                                return "NO";
                            }
                        }
                    },
                    { data: 'tituloActividad', title: 'Actividad' },
                    { data: 'descActividad', title: 'Descripción' },
                    {
                        data: 'esArchivos', title: 'Requiere<br>archivos',
                        render: function (data, type, row) {
                            if (data) {
                                return "SI";
                            } else {
                                return "NO";
                            }
                        }
                    },
                    {
                        title: 'Resultado',
                        render: function (data, type, row) {

                            let input = `<div class="input-group">
                                    <input type="text" value="${row.calificacion}" onClick="this.select();" class="inputCalificacionActividad" ${!row.esCalificacion ? 'disabled' : ''}>
                                    <span class="input-group-btn">
                                        <button class="btn btn-primary btn-xs calificacionActividad" type="button" title="Registrar calificación" ${!row.esCalificacion ? 'disabled' : ''}><i class="fas fa-save"></i></button>
                                    </span>
                                </div>`;
                            //#region COMBO EVUALIACION MEDICO
                            if (row.id == 73) {

                                input = `<div class="input-group">
                                    <select class="form-control inputCalificacionActividad">
                                        <option value="" ${row.calificacion == null ? "selected" : ""}>--Seleccionar--</option>
                                        <option value="1" ${row.calificacion == 1 ? "selected" : ""}>APTO</option>
                                        <option value="2" ${row.calificacion == 2 ? "selected" : ""}>NO APTO</option>
                                        <option value="3" ${row.calificacion == 3 ? "selected" : ""}>MEDICAMENTE CONDICIONADO</option>
                                    </select>
                                    <span class="input-group-btn">
                                        <button class="btn btn-primary btn-sm calificacionActividad" type="button" title="Registrar calificación" ${!row.esCalificacion ? 'disabled' : ''}><i class="fas fa-save"></i></button>
                                    </span>
                                </div>`;
                                if (row.esValidar) {
                                    if (row.id == row.actividadValidar) {
                                        input = `<div class="input-group">
                                            <select class="form-control inputCalificacionActividad">
                                                <option value="" ${row.calificacion == null ? "selected" : ""}>--Seleccionar--</option>
                                                <option value="1" ${row.calificacion == 1 ? "selected" : ""}>APTO</option>
                                                <option value="2" ${row.calificacion == 2 ? "selected" : ""}>NO APTO</option>
                                                <option value="3" ${row.calificacion == 3 ? "selected" : ""}>MEDICAMENTE CONDICIONADO</option>
                                            </select>
                                            <span class="input-group-btn">
                                                <button class="btn btn-primary btn-xs calificacionActividad" type="button" title="Registrar calificación" ${!row.esCalificacion ? 'disabled' : ''}><i class="fas fa-save"></i></button>
                                            </span>
                                        </div>`;
                                    } else {
                                        input = `<div class="input-group ">
                                            <select class="form-control inputCalificacionActividad" disabled>
                                                <option value="" ${row.calificacion == null ? "selected" : ""}>--Seleccionar--</option>
                                                <option value="1" ${row.calificacion == 1 ? "selected" : ""}>APTO</option>
                                                <option value="2" ${row.calificacion == 2 ? "selected" : ""}>NO APTO</option>
                                                <option value="3" ${row.calificacion == 3 ? "selected" : ""}>MEDICAMENTE CONDICIONADO</option>
                                            </select>
                                            <span class="input-group-btn">
                                                <button class="btn btn-primary btn-xs calificacionActividad" type="button" title="Registrar calificación" disabled><i class="fas fa-save"></i></button>
                                            </span>
                                        </div>`;
                                    }
                                } else {
                                    input = `<div class="input-group ">
                                        <select class="form-control inputCalificacionActividad" disabled>
                                        <option value="" ${row.calificacion == null ? "selected" : ""}>--Seleccionar--</option>
                                            <option value="1" ${row.calificacion == 1 ? "selected" : ""}>APTO</option>
                                            <option value="2" ${row.calificacion == 2 ? "selected" : ""}>NO APTO</option>
                                            <option value="3" ${row.calificacion == 3 ? "selected" : ""}>MEDICAMENTE CONDICIONADO</option>
                                        </select>
                                        <span class="input-group-btn">
                                            <button class="btn btn-primary btn-xs calificacionActividad" type="button" title="Registrar calificación" disabled><i class="fas fa-save"></i></button>
                                        </span>
                                    </div>`;
                                }
                                return input;
                            }
                            //#endregion

                            if (row.esValidar) {
                                if (row.id == row.actividadValidar) {
                                    input = `<div class="input-group">
                                        <input type="text" value="${row.calificacion}" onClick="this.select();" class="inputCalificacionActividad" ${!row.esCalificacion ? 'disabled' : ''}>
                                        <span class="input-group-btn">
                                            <button class="btn btn-primary btn-xs calificacionActividad" type="button" title="Registrar calificación" ${!row.esCalificacion ? 'disabled' : ''}><i class="fas fa-save"></i></button>
                                        </span>
                                    </div>`;
                                } else {
                                    input = `<div class="input-group">
                                        <input type="text" value="${row.calificacion}" onClick="this.select();" class="inputCalificacionActividad" disabled>
                                        <span class="input-group-btn">
                                            <button class="btn btn-primary btn-xs calificacionActividad" type="button" title="Registrar calificación" disabled><i class="fas fa-save"></i></button>
                                        </span>
                                    </div>`;
                                }
                            } else {
                                input = `<div class="input-group">
                                    <input type="text" value="${row.calificacion}" onClick="this.select();" class="inputCalificacionActividad" disabled>
                                    <span class="input-group-btn">
                                        <button class="btn btn-primary btn-xs calificacionActividad" type="button" title="Registrar calificación" disabled><i class="fas fa-save"></i></button>
                                    </span>
                                </div>`;
                            }


                            return input;

                            // if (parseFloat(row.calificacion) > 0) {
                            //     return `<input type="text" value="${row.calificacion}" class="form-control calificacionActividad">`;
                            // } else {
                            //     return `<input type="text" class="form-control calificacionActividad">`;
                            // }
                        }
                    },
                    {
                        title: 'Observación',
                        render: function (data, type, row) {
                            if (row.esValidar) {
                                if (row.id == row.actividadValidar) {

                                    return `<button class="btn ${row.comentario != null && row.comentario != "" ? "btn-success" : "btn-primary"} btn-xs verObservacion">Observación</button>`;

                                } else {
                                    return `<button class="btn ${row.comentario != null && row.comentario != "" ? "btn-success" : "btn-primary"} btn-xs verObservacion" disabled>Observación</button>`;

                                }
                            }
                            return `<button class="btn ${row.comentario != null && row.comentario != "" ? "btn-success" : "btn-primary"} btn-xs verObservacion">Observación</button>`;
                        }
                    },
                    {
                        title: 'Evidencias',
                        render: function (data, type, row) {
                            if (row.esArchivos) {

                                if (row.id == 71) {
                                    return `<button class="btn btn-success btn-xs verEvidencias">Evidencias</button>`;
                                }

                                if (row.esValidar) {
                                    if (row.id == row.actividadValidar) {

                                        if (row.numEvidencia > 0) {

                                            return `<button class="btn btn-success btn-xs verEvidencias">Evidencias</button>`;
                                        } else {

                                            return `<button class="btn btn-primary btn-xs verEvidencias">Evidencias</button>`;
                                        }
                                    } else {
                                        if (row.numEvidencia > 0) {

                                            return `<button class="btn btn-success btn-xs verEvidencias" disabled>Evidencias</button>`;
                                        } else {

                                            return `<button class="btn btn-primary btn-xs verEvidencias" disabled>Evidencias</button>`;
                                        }
                                    }
                                }

                                if (row.numEvidencia > 0) {
                                    return `<button class="btn btn-success btn-xs verEvidencias">Evidencias</button>`;
                                } else {

                                    return `<button class="btn btn-primary btn-xs verEvidencias">Evidencias</button>`;
                                }

                            } else {
                                return `<button class="btn btn-primary btn-xs verEvidencias" disabled>Evidencias</button>`;

                            }
                        }
                    },
                    {
                        data: 'estatus', title: 'Estatus',
                        render: (data, type, row, meta) => {
                            if (data == "PENDIENTE") {
                                return data;
                            } else {
                                return data + "<br>" + row.firma;
                            }
                        }
                    },
                    {
                        render: function (data, type, row) {
                            let btn = "";
                            btn += `<div class="btn-group" role="group" aria-label="Basic example">
                                    <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto"><i class="fas fa-check"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto"><i class="fas fa-times"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica" ><i class="fas fa-user-slash"></i></button>
                                    <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente"><i class="fas fa-stop"></i></button>
                                </div>`;

                            if (row.esTodasActividades) {
                                if (row.id == 73 || row.id == 74 || row.id == 76) {
                                    return `<div class="btn-group" role="group" aria-label="Basic example">
                                <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto" ><i class="fas fa-check"></i></button>
                                <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto" ><i class="fas fa-times"></i></button>
                                <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica" ><i class="fas fa-user-slash"></i></button>
                                <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente" ><i class="fas fa-stop" ></i></button>
                            </div>
                            <button class="btn btn-${row.esNotificada ? "success" : "warning"} notificarActividad btn-xs" title="Notificar"><i class="fas fa-envelope"></i></button>`;
                                } else {
                                    if (row.id == 75) {
                                        return `<div class="btn-group" role="group" aria-label="Basic example">
                                <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto"><i class="fas fa-check"></i></button>
                                <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto"><i class="fas fa-times"></i></button>
                                <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica"><i class="fas fa-user-slash"></i></button>
                                <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente"><i class="fas fa-stop"></i></button>
                            </div>`;
                                    } else {
                                        if (row.idFase == 1018 && row.esObligatoria) {
                                            return `<div class="btn-group" role="group" aria-label="Basic example">
                                <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto"><i class="fas fa-check"></i></button>
                                <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto"><i class="fas fa-times"></i></button>
                                                    
                                <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente"><i class="fas fa-stop"></i></button>
                            </div>`;
                                        }
                                    }
                                }
                            }

                            if (row.esValidar) {
                                if (row.id == row.actividadValidar) {
                                    if (row.id == 73 || row.id == 74 || row.id == 76) {
                                        if (row.id != 73) {
                                            return `<div class="btn-group" role="group" aria-label="Basic example">
                                    <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto"><i class="fas fa-check"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto"><i class="fas fa-times"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica"><i class="fas fa-user-slash"></i></button>
                                    <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente"><i class="fas fa-stop"></i></button>
                                </div>
                                <button class="btn btn-${row.esNotificada ? "success" : "warning"} notificarActividad btn-xs" title="Notificar"><i class="fas fa-envelope"></i></button>`;
                                        }
                                        btn = `<div class="btn-group" role="group" aria-label="Basic example">
                                <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto"><i class="fas fa-check"></i></button>
                                <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto"><i class="fas fa-times"></i></button>
                                <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica" ><i class="fas fa-user-slash"></i></button>
                                <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente"><i class="fas fa-stop"></i></button>
                            </div>
                            <button type="button" class="btn btn-default btn-xs botonCapturaExamenMedico paddingValidar" title="Examen Médico"><i class="fas fa-align-justify"></i></button>
                            <button class="btn btn-${row.esNotificada ? "success" : "warning"} notificarActividad btn-xs" title="Notificar"><i class="fas fa-envelope"></i></button>`;
                                    } else {
                                        if (row.id == 75) {
                                            return `<div class="btn-group" role="group" aria-label="Basic example">
                                    <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto"><i class="fas fa-check"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto"><i class="fas fa-times"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica"><i class="fas fa-user-slash"></i></button>
                                    <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente"><i class="fas fa-stop"></i></button>
                                </div>`;
                                        }
                                        else {
                                            if (row.idFase == 1018 && row.esObligatoria) {
                                                return `<div class="btn-group" role="group" aria-label="Basic example">
                                    <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto"><i class="fas fa-check"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto"><i class="fas fa-times"></i></button>
                                                        
                                    <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente"><i class="fas fa-stop"></i></button>
                                </div>`;
                                            }
                                        }
                                    }
                                } else {
                                    btn = `<div class="btn-group" role="group" aria-label="Basic example">
                            <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto" disabled><i class="fas fa-check"></i></button>
                            <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto" disabled><i class="fas fa-times"></i></button>
                            <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica" ><i class="fas fa-user-slash"></i></button>
                            <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente" disabled><i class="fas fa-stop"></i></button>
                        </div>`;
                                }
                            } else {
                                if (row.esOfi) {
                                    if (row.id == 73 || row.id == 74 || row.id == 76) {
                                        if (row.id == 74) {
                                            return `<div class="btn-group" role="group" aria-label="Basic example">
                                                <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto" disabled><i class="fas fa-check"></i></button>
                                                <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto" disabled><i class="fas fa-times"></i></button>
                                                <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica" disabled><i class="fas fa-user-slash"></i></button>
                                                <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente" disabled><i class="fas fa-stop" ></i></button>
                                            </div>
                                            <button class="btn btn-${row.esNotificada ? "success" : "warning"} notificarActividad btn-xs" title="Notificar"><i class="fas fa-envelope"></i></button>`;
                                        } else {
                                            return `<div class="btn-group" role="group" aria-label="Basic example">
                                                <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto" disabled><i class="fas fa-check"></i></button>
                                                <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto" disabled><i class="fas fa-times"></i></button>
                                                <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica" ><i class="fas fa-user-slash"></i></button>
                                                <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente" ><i class="fas fa-stop" ></i></button>
                                            </div>
                                            <button class="btn btn-${row.esNotificada ? "success" : "warning"} notificarActividad btn-xs" title="Notificar"><i class="fas fa-envelope"></i></button>`;
                                        }

                                    } else {
                                        if (row.id == 75) {
                                            return `<div class="btn-group" role="group" aria-label="Basic example">
                                    <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto"><i class="fas fa-check"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto"><i class="fas fa-times"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica"><i class="fas fa-user-slash"></i></button>
                                    <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente"><i class="fas fa-stop"></i></button>
                                </div>`;
                                        } else {
                                            if (row.idFase == 1018 && row.esObligatoria) {
                                                return `<div class="btn-group" role="group" aria-label="Basic example">
                                    <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto"><i class="fas fa-check"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto"><i class="fas fa-times"></i></button>
                                                        
                                    <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente"><i class="fas fa-stop"></i></button>
                                </div>`;
                                            }
                                        }
                                    }
                                } else {
                                    if (row.id == 73 || row.id == 74 || row.id == 76) {
                                        if (row.id != 73) {
                                            return `<div class="btn-group" role="group" aria-label="Basic example">
                                    <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto" disabled><i class="fas fa-check"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto" disabled><i class="fas fa-times"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica" disabled><i class="fas fa-user-slash"></i></button>
                                    <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente" disabled><i class="fas fa-stop" ></i></button>
                                </div>
                                <button class="btn btn-${row.esNotificada ? "success" : "warning"} notificarActividad btn-xs" title="Notificar" disabled><i class="fas fa-envelope"></i></button>`;
                                        }
                                        btn = `<div class="btn-group" role="group" aria-label="Basic example">
                                <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto" disabled><i class="fas fa-check"></i></button>
                                <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto" disabled><i class="fas fa-times"></i></button>
                                <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica" ><i class="fas fa-user-slash"></i></button>
                                <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente" disabled><i class="fas fa-stop"></i></button>
                            </div>
                            <button class="btn btn-${row.esNotificada ? "success" : "warning"} notificarActividad btn-xs" title="Notificar" disabled><i class="fas fa-envelope"></i></button>`;
                                    } else {
                                        if (row.id == 75) {
                                            return `<div class="btn-group" role="group" aria-label="Basic example">
                                    <button type="button" class="btn btn-xs btnValidado borderColorValidar paddingValidar notActive" title="Apto" disabled><i class="fas fa-check"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAprobado borderColorRechazar paddingValidar notActive" title="No Apto" disabled><i class="fas fa-times"></i></button>
                                    <button type="button" class="btn btn-xs btnNoAplica borderColorRechazar paddingValidar notActive" title="No Aplica" disabled><i class="fas fa-user-slash"></i></button>
                                    <button type="button" class="btn btn-primary btn-xs btnPendiente paddingValidar" title="Pendiente" disabled><i class="fas fa-stop" ></i></button>
                                </div>`;
                                        }
                                    }
                                }

                            }

                            return btn;
                        }
                    },
                    { data: 'id', visible: false },
                    { data: 'idFase', visible: false },
                    { data: 'esGeneral', visible: false },
                    { data: 'calificacion', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_Actividades.on('click', '.btnValidado', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtActividades.row(row).data();
                        let calificacionActividad = row.find(`.inputCalificacionActividad`).val();

                        btnCrearArchivoActividad.attr("data-idCandidato", btnRegresarSegCandidatos.attr("data-idCandidato"));
                        btnCrearArchivoActividad.attr("data-idFase", rowData.idFase);
                        btnCrearArchivoActividad.attr("data-idActividad", rowData.id);
                        btnCrearArchivoActividad.attr("data-Cali", calificacionActividad);
                        btnCrearArchivoActividad.attr("data-Coment", rowData.comentario);

                        //ENTREVISTA INICIAL; ENTREVISTA JEFE INMEDIATO
                        if (rowData.id == 71 || rowData.id == 76) {

                            Alert2AccionConfirmar('Validar actividad', '¿Desea indicar que se valido correctamente esta actividad?', 'Confirmar', 'Cancelar',
                                () => fncActividadAprobada(btnRegresarSegCandidatos.attr("data-idCandidato"), rowData.idFase, rowData.id, calificacionActividad, rowData.comentario, null));

                        } else {
                            if (rowData.numEvidencia > 0) {
                                Alert2AccionConfirmar('Validar actividad', '¿Desea indicar que se valido correctamente esta actividad?', 'Confirmar', 'Cancelar',
                                    () => fncActividadAprobada(btnRegresarSegCandidatos.attr("data-idCandidato"), rowData.idFase, rowData.id, calificacionActividad, rowData.comentario, null));
                            } else {
                                Alert2Warning("Evidencia obligatoria para la actividad: " + rowData.tituloActividad);
                            }
                        }



                    });

                    tblRH_REC_Actividades.on('click', '.btnNoAplica', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtActividades.row(row).data();
                        let calificacionActividad = row.find(`.calificacionActividad`).val();

                        if (rowData.esObligatoria) {
                            Alert2Warning("Esta actividad es obligatoria y no puede evaluarse como No Aplica");
                        } else {
                            btnCrearArchivoActividad.attr("data-idCandidato", btnRegresarSegCandidatos.attr("data-idCandidato"));
                            btnCrearArchivoActividad.attr("data-idFase", rowData.idFase);
                            btnCrearArchivoActividad.attr("data-idActividad", rowData.id);
                            btnCrearArchivoActividad.attr("data-Cali", calificacionActividad);
                            btnCrearArchivoActividad.attr("data-Coment", rowData.comentario);

                            Alert2AccionConfirmar('Validar actividad', '¿Desea indicar que esta actividad no aplica para el candidato?', 'Confirmar', 'Cancelar',
                                () => fncActividadNoAplica(btnRegresarSegCandidatos.attr("data-idCandidato"), rowData.idFase, rowData.id, calificacionActividad, rowData.comentario, null));
                        }
                    });

                    tblRH_REC_Actividades.on('click', '.btnNoAprobado', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtActividades.row(row).data();
                        let calificacionActividad = row.find(`.calificacionActividad`).val();

                        Alert2AccionConfirmar('Validar actividad', '¿Desea indicar que no se valido correctamente esta actividad?', 'Confirmar', 'Cancelar',
                            () => fncActividadNoAprobada(btnRegresarSegCandidatos.attr("data-idCandidato"), rowData.idFase, rowData.id, calificacionActividad));
                    });

                    tblRH_REC_Actividades.on("click", ".btnPendiente", function () {
                        let row = $(this).closest('tr');
                        let rowData = dtActividades.row(row).data();
                        let calificacionActividad = row.find(`.calificacionActividad`).val();

                        Alert2AccionConfirmar('Validar actividad', '¿Desea indicar que esta pendiente esta actividad?', 'Confirmar', 'Cancelar',
                            () => fncActividadPendiente(btnRegresarSegCandidatos.attr("data-idCandidato"), rowData.idFase, rowData.id, calificacionActividad));
                    })

                    tblRH_REC_Actividades.on("click", ".verObservacion", function () {
                        let row = $(this).closest('tr');
                        let rowData = dtActividades.row(row).data();

                        btnCrearArchivoActividad.attr("data-idCandidato", btnRegresarSegCandidatos.attr("data-idCandidato"));
                        btnCrearArchivoActividad.attr("data-idFase", rowData.idFase);
                        btnCrearArchivoActividad.attr("data-idActividad", rowData.id);

                        fncGetObservacionActividad(btnRegresarSegCandidatos.attr("data-idCandidato"), rowData.idFase, rowData.id);
                        mdlVerObservacion.modal("show");
                    });

                    tblRH_REC_Actividades.on("click", ".verEvidencias", function () {
                        let row = $(this).closest('tr');
                        let rowData = dtActividades.row(row).data();

                        btnCrearArchivoActividad.attr("data-idCandidato", btnRegresarSegCandidatos.attr("data-idCandidato"));
                        btnCrearArchivoActividad.attr("data-idFase", rowData.idFase);
                        btnCrearArchivoActividad.attr("data-idActividad", rowData.id);

                        if (rowData.id == 71) {
                            mdlCrearEditarEntrevistaInicial.modal("show");
                            fncGetEntrevistaInicial(btnRegresarSegCandidatos.attr("data-idCandidato"));
                        } else {
                            fncGetArchivosActividadesRelFase();
                            mdlVerEvidencias.modal("show");
                        }
                    });

                    tblRH_REC_Actividades.on("click", ".btnCalificar", function () {
                        mdlVerCalificacion.modal("show");
                    });

                    tblRH_REC_Actividades.on("click", ".calificacionActividad", function () {
                        let row = $(this).closest('tr');
                        let rowData = dtActividades.row(row).data();
                        let valCalActividad = row.find(`.inputCalificacionActividad`).val();

                        if (valCalActividad != "") {
                            Alert2AccionConfirmar('Validar actividad', '¿Desea registrar la calificación?', 'Confirmar', 'Cancelar',
                                () => fncCECalificacionActividad(btnRegresarSegCandidatos.attr("data-idCandidato"), rowData.idFase, rowData.id, valCalActividad));
                        } else {
                            Alert2Warning("Capture un resultado");
                        }
                    });

                    tblRH_REC_Actividades.on("click", ".notificarActividad", function () {
                        let row = $(this).closest('tr');
                        let rowData = dtActividades.row(row).data();
                        let calificacionActividad = row.find(`.calificacionActividad`).val();

                        txtSendNombre.val("");
                        txtSendClave.val("");

                        btnCrearArchivoActividad.attr("data-idCandidato", btnRegresarSegCandidatos.attr("data-idCandidato"));
                        btnCrearArchivoActividad.attr("data-idFase", rowData.idFase);
                        btnCrearArchivoActividad.attr("data-idActividad", rowData.id);
                        btnCrearArchivoActividad.attr("data-Cali", calificacionActividad);
                        btnCrearArchivoActividad.attr("data-Coment", rowData.comentario);

                        if (rowData.id == 76) {

                            mdlSendNotificante.modal("show");
                        } else {
                            Alert2AccionConfirmar('Notificar', '¿Desea notificar la actividad?', 'Confirmar', 'Cancelar', () => {
                                fncNoficarActividad(btnRegresarSegCandidatos.attr("data-idCandidato"), btnCrearArchivoActividad.attr("data-idActividad"), 0, 0);
                                fncSetNotiEstatusActividad(btnRegresarSegCandidatos.attr("data-idCandidato"), btnCrearArchivoActividad.attr("data-idFase"), btnCrearArchivoActividad.attr("data-idActividad"));
                            });
                        }



                        // if (rowData.id == 76) {
                        //     mdlSendNotificante.modal("show");
                        // } else {
                        //     Alert2AccionConfirmar('NOTIFICAR', '¿DESEA MANDAR CORREO DE NOTIFICACIÓN?', 'Confirmar', 'Cancelar',
                        //         () => { fncNoficarActividad(btnRegresarSegCandidatos.attr("data-idCandidato"), rowData.id, 0) });
                        // }

                    });

                    tblRH_REC_Actividades.on('click', '.botonCapturaExamenMedico', function () {
                        limpiarModalExamenMedico();
                        modalExamenMedico.modal('show');
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': "_all" },
                    { "width": "5%", "targets": 0 },
                    { "width": "8%", "targets": 1 },
                    { "width": "5%", "targets": 3 },
                    { "width": "8%", "targets": 4 },
                    { "width": "5%", "targets": 5 },
                    { "width": "8%", "targets": 6 },
                    { "width": "8%", "targets": 7 }
                ],
            });
        }

        function limpiarModalExamenMedico() {
            $('input').not(':checkbox').not('#inputNombre').not('#inputEdad').not('#inputTelefono').val(''); //Selector de jQuery que excluye los inputs de tipo checkbox para que no cause problemas con el Bootstrap Multiselect.
            $('select').val('');
            $('textarea').val('');
            selectAntecedentes.multiselect('deselectAll', false).multiselect('refresh');
            $(`a[data-toggle="radioApto"]`).removeClass('active').addClass('notActive');
            $('.select2').select2().change();
        }

        function guardarExamenMedico() {
            let examenMedico = {
                id: 0,
                nombre: inputNombre.val(),
                edad: +inputEdad.val(),
                telefono: inputTelefono.val(),
                direccion: inputDireccion.val(),
                escolaridad: +selectEscolaridad.val(),
                otraEscolaridad: inputOtraEscolaridad.val(),
                estadoCivil: +selectEstadoCivil.val(),
                otroEstadoCivil: inputOtroEstadoCivil.val(),
                hijos: +inputHijos.val(),
                puestoAnterior: +selectPuestoAnterior.val(),
                puestoAnteriorDesc: +selectPuestoAnterior.val() > 0 ? selectPuestoAnterior.find('option:selected').text() : '',
                puestoOcupar: +selectPuestoOcupar.val(),
                puestoOcuparDesc: +selectPuestoOcupar.val() > 0 ? selectPuestoOcupar.find('option:selected').text() : '',
                alcoholismo: +selectAlcoholismo.val() == 1,
                observacionesAlcoholismo: inputObservacionesAlcoholismo.val(),
                tabaquismo: +selectTabaquismo.val() == 1,
                observacionesTabaquismo: inputObservacionesTabaquismo.val(),
                toxicomania: +selectToxicomania.val() == 1,
                observacionesToxicomania: inputObservacionesToxicomania.val(),
                lentes: +selectLentes.val() == 1,
                observacionesLentes: inputObservacionesLentes.val(),
                tipoSanguineo: inputTipoSanguineo.val(),
                visual: inputVisual.val(),
                auditiva: inputAuditiva.val(),
                TA: inputTA.val(),
                pulso: inputPulso.val(),
                marchaPunta: inputMarchaPunta.val(),
                talon: inputTalon.val(),
                romberg: inputRomberg.val(),
                arcosFlexion: inputArcosFlexion.val(),
                antecedentesFamiliares: inputAntecedentesFamiliares.val(),
                heredoFamiliar: inputHeredoFamiliar.val(),
                tratamiento: inputTratamiento.val(),
                rayosX: inputRayosX.val(),
                menarca: inputMenarca.val(),
                VSA: inputVSA.val(),
                numeroGestas: inputNumeroGestas.val(),
                ritmo: inputRitmo.val(),
                MPF: inputMPF.val(),
                PIE: inputPIE.val(),
                OPI: inputOPI.val(),
                MET: inputMET.val(),
                COC: inputCOC.val(),
                AMP: inputAMP.val(),
                THC: inputTHC.val(),
                personaApta: +($(`.radioBtn a.active[data-toggle=radioApto]`).attr('apto')) == 1,
                observacionesGenerales: textareaObservacionesGenerales.val(),
            };
            let listaAntecedentes = [];

            selectAntecedentes.val().forEach(x => {
                listaAntecedentes.push({
                    id: 0,
                    examenMedico_id: 0,
                    antecedente: +x,
                    registroActivo: true
                });
            });

            let idCandidato = +btnRegresarSegCandidatos.attr('data-idcandidato');

            axios.post(`/Reportes/Vista.aspx?idReporte=${269}&idCandidato=${idCandidato}&inMemory=1`, { examenMedico, listaAntecedentes }).then(response => {
                if (response.status == 200) {
                    Alert2Exito('Se ha guardado la información.');
                    modalExamenMedico.modal('hide');
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function fncActividadAprobada(idCandidato, idFase, idActividad, calificacion, comentario, idNoti) {
            let obj = new Object();
            obj = {
                idCandidato: idCandidato,
                idFase: idFase,
                idActividad: idActividad,
                calificacion: calificacion,
                esAprobada: 1,
                esOmitida: 0,

            }

            axios.post("CrearEditarSegCandidatos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetActividadesFase(idFase);

                    if (idActividad == 73 || idActividad == 74 || idActividad == 76) {

                        fncNoficarActividad(idCandidato, idActividad, 1, 0);
                    }

                    if (comentario == null || comentario == "") {
                        Alert2Exito("Se ha registrado con éxito la actividad sin comentarios.");

                    } else {
                        Alert2Exito("Se ha registrado con éxito la actividad.");

                    }
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncActividadNoAplica(idCandidato, idFase, idActividad, calificacion, comentario, idNoti) {
            let obj = new Object();
            obj = {
                idCandidato: idCandidato,
                idFase: idFase,
                idActividad: idActividad,
                calificacion: calificacion,
                esAprobada: 3,
                esOmitida: 0,

            }

            axios.post("CrearEditarSegCandidatos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetActividadesFase(idFase);
                    Alert2Exito("Se ha registrado con éxito la actividad.");

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCECalificacionActividad(idCandidato, idFase, idActividad, calificacion) {
            let obj = new Object();
            obj = {
                idCandidato: idCandidato,
                idFase: idFase,
                idActividad: idActividad,
                calificacion: Number(calificacion)
            }
            console.log(calificacion);
            axios.post("CrearEditarCalificacionActividad", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha registrado con éxito la calificación.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEObservacionActividad(idCandidato, idFase, idActividad, strObservacion) {
            let obj = new Object();
            obj = {
                idCandidato: idCandidato,
                idFase: idFase,
                idActividad: idActividad,
                comentario: strObservacion
            }
            axios.post("CrearEditarComentarioActividad", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha registrado con éxito la observación.");
                    txtCEObservacionActividad.val("");
                    mdlVerObservacion.modal("hide");
                } else {
                    Alert2Error(message);
                }
                fncGetActividadesFase(idFase);
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetObservacionActividad(idCandidato, idFase, idActividad) {
            let obj = new Object();
            obj = {
                idCandidato: idCandidato,
                idFase: idFase,
                idActividad: idActividad
            }
            axios.post("GetObservacionActividad", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    txtCEObservacionActividad.val(response.data.strObservacion);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearArchivoActividad() {
            let objSegCandidatoDTO = fncObjArchivos();
            axios.post("CrearArchivoActividad", objSegCandidatoDTO, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetArchivosActividadesRelFase();
                        $("#txtCrearArchivo").val("");
                        Alert2Exito("Éxito al registrar el archivo.");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
        }

        function fncObjArchivos() {
            let formData = new FormData();
            // const archivo = $("#txtCrearArchivo").get(0).files[0];

            $.each(document.getElementById("txtCrearArchivo").files, function (i, file) {
                formData.append("lstFiles", file);
            });

            let objSegCandidatoDTO = new Object();
            objSegCandidatoDTO = {
                idCandidato: btnCrearArchivoActividad.attr("data-idCandidato"),
                idFase: btnCrearArchivoActividad.attr("data-idFase"),
                idActividad: btnCrearArchivoActividad.attr("data-idActividad")
            }
            // formData.set('objFile', archivo);
            formData.set('objSegCandidatoDTO', JSON.stringify(objSegCandidatoDTO));

            return formData;
        }

        function fncGetArchivosActividadesRelFase() {
            let obj = new Object();
            obj = {
                idCandidato: btnCrearArchivoActividad.attr("data-idCandidato"),
                idFase: btnCrearArchivoActividad.attr("data-idFase"),
                idActividad: btnCrearArchivoActividad.attr("data-idActividad")
            }
            axios.post("GetArchivosActividadesRelFase", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtEvidencias.clear();
                    dtEvidencias.rows.add(response.data.lstArchivos);
                    dtEvidencias.draw();
                    //#endregion

                    // btnCrearArchivoActividad.attr('disabled', response.data.lstArchivos.length > 0);
                    // $('#txtCrearArchivo').attr('disabled', response.data.lstArchivos.length > 0);
                    fncGetActividadesFase(btnCrearArchivoActividad.attr("data-idFase"));
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncActividadNoAprobada(idCandidato, idFase, idActividad, calificacion) {
            let obj = new Object();
            obj = {
                idCandidato: idCandidato,
                idFase: idFase,
                idActividad: idActividad,
                calificacion: calificacion,
                esAprobada: 0,
                esOmitida: 0
            }
            axios.post("CrearEditarSegCandidatos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetActividadesFase(idFase);
                    Alert2Exito("Se ha registrado con éxito la actividad.");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncActividadPendiente(idCandidato, idFase, idActividad, calificacion) {
            let obj = new Object();
            obj = {
                idCandidato: idCandidato,
                idFase: idFase,
                idActividad: idActividad,
                calificacion: calificacion,
                esAprobada: 2,
                esOmitida: 0
            }
            axios.post("CrearEditarSegCandidatos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetActividadesFase(idFase);
                    Alert2Exito("Se ha registrado con éxito la actividad.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetActividadesFase(idFase) {
            let obj = new Object();
            obj = {
                idFase: idFase,
                idCandidato: btnRegresarSegCandidatos.attr("data-idCandidato"),
                clave_empleado: btnRegresarSegCandidatos.attr("data-claveCandidato"),
                idPuesto: btnRegresarSegCandidatos.attr("data-puesto"),

            }

            axios.post("GetActividades", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE

                    $(`#${idFase}`).css("background-color", response.data.lstActividades[0].colorFase);
                    $(`#${idFase}`).css("color", response.data.lstActividades[0].color);

                    if (!primeraCargaPagina) {
                        if (idFase == 1020) {
                            dtActividades.clear();
                            dtActividades.draw();
                        } else {
                            dtActividades.clear();
                            dtActividades.rows.add(response.data.lstActividades);
                            dtActividades.draw();
                            dtActividades.column(4).visible(!response.data.lstActividades.every((x) => !x.esCalificacion));
                        }


                        if (idFase == 1020) {
                            if (response.data.progresoVal < 100) {
                                Alert2Warning("El candidato tiene actividades obligatorias por validar.");
                            } else {
                                if (response.data.esAlta) {
                                    if (response.data.esReingreso) {
                                        Alert2AccionConfirmar('', '¿Desea proceder con el reingreso de datos de empleado de este candidato?', 'Confirmar', 'Cancelar',
                                            () => fncAltaEmpleado(btnRegresarSegCandidatos.attr("data-idCandidato")));
                                    } else {
                                        Alert2AccionConfirmar('', '¿Desea proceder con alta de datos de empleado de este candidato?', 'Confirmar', 'Cancelar',
                                            () => fncAltaEmpleado(btnRegresarSegCandidatos.attr("data-idCandidato")));
                                    }
                                } else {
                                    Alert2AccionConfirmar('', '¿Desea proceder con alta de datos de empleado de este candidato?', 'Confirmar', 'Cancelar',
                                        () => fncAltaEmpleado(btnRegresarSegCandidatos.attr("data-idCandidato")));
                                }
                            }
                        }
                    }

                    contadorFases++
                    if (contadorFases == contadorFasesContruccion) {
                        primeraCargaPagina = false;
                    }
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));


        }

        function fncConstruccionFases(cantFases, lstFasesID, nombreCandidato, lstFasesStr, puesto, esSeguimientoCancelado) {
            fncMostrarFases(false);

            let sortedLstFasesID = lstFasesID.sort();

            $("#lblNombreCandidato").html(`Candidato: ${nombreCandidato}`);
            $("#lblNombrePuesto").html(`Puesto: ${puesto}`);
            $("#lblEstatusSeguimiento").html(`Estatus seguimiento:&nbsp;${esSeguimientoCancelado}</b>`);
            console.log(esSeguimientoCancelado);
            if (esSeguimientoCancelado != "ACTIVO") {
                $("#lblEstatusSeguimiento").css("color", "red");
            } else {
                $("#lblEstatusSeguimiento").css("color", "#333");
            }

            let contador = 0;
            let header = `<div class="stepwizard"><div class="stepwizard-row setup-panel">`;
            let body = ``;
            let footer = `</div></div>`;
            for (let i = 1; i < parseFloat(cantFases) + parseFloat(1); i++) {

                let nombreFase = "";

                switch (sortedLstFasesID[contador]) {
                    case 1016:
                        nombreFase = "CAPITAL HUMANO";
                        break;
                    case 1017:
                        nombreFase = "EVALUACION";
                        break;
                    case 1018:
                        nombreFase = "DOCUMENTACION";
                        break;
                    case 1019:
                        nombreFase = "DOCUMENTACION COMPLEMENTARIA";
                        break;
                    case 1020:
                        nombreFase = "CONTRATACION";
                        break;
                    default:
                        nombreFase = "default";
                        break;

                }
                body += `<div class="stepwizard-step">
                    <a type="button" class="btn btn-circle btn-default btn-success btnFaseCandidato inactive done" id="${sortedLstFasesID[contador]}" data-id="${sortedLstFasesID[contador]}" disabled>${i}</a>
                    <p class="lblFaseCandidato" id="txt${sortedLstFasesID[contador]}">${nombreFase}</p>
                </div>`;
                contador++;
                contadorFasesContruccion++;
            }

            let div = header + body + footer;
            divSetFases.html(div);
        }

        function fncSetNotiEstatusActividad(idCandidato, idFase, idActividad) {
            let obj = {
                idCandidato: idCandidato,
                idFase: idFase,
                idActividad: idActividad,
            }

            axios.post("SetNotiEstatusActividad", obj).then(response => {
                let { success, items, message } = response.data;
                if (response.data) {
                    Alert2Exito("Actividad Notificada");
                    fncGetActividadesFase(idFase);
                } else {
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region CRUD SEGUIMIENTO CANDIDATOS
        function initTblSegCandidatos() {
            dtSegCandidatos = tblRH_REC_SegCandidatos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreCandidato', title: 'Candidato' },
                    { data: 'puesto', title: 'Puesto' },
                    { data: 'correo', title: 'Correo', visible: false },
                    { data: 'celular', title: 'Celular' },
                    {
                        title: 'Seguimiento',
                        render: function (data, type, row) {
                            let txtProgressBar = "";
                            let porcent = row.progresoSeguimiento.toFixed(2);
                            if (row.progresoSeguimiento <= 20) {
                                txtProgressBar +=
                                    `<div class="progress">
                            <div class="progress-bar" style="width: 30%; background-color: #008000 !important;" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100">${porcent}%</div>
                        </div>`;
                            } else {
                                txtProgressBar +=
                                    `<div class="progress">
                            <div class="progress-bar" style="width: ${porcent}%; background-color: #008000 !important;" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100">${porcent}%</div>
                        </div>`;
                            }

                            return txtProgressBar;
                        }
                    },
                    {
                        visible: true,
                        render: function (data, type, row) {
                            let btnVerSegCandidato = `<button class="btn btn-primary btn-xs verSeguimientoCandidato" title="Ver seguimiento del candidato">
                                                <i class="fas fa-sign-in-alt"></i></button>&nbsp;`;
                            let btnAltaEmpleado = `<button class="btn btn-success btn-xs btnAltaEmpleado" title="Dar de alta al empleado."><i class="fas fa-user-plus"></i></button>`;
                            let porcent = row.progresoSeguimiento;
                            // return btnVerSegCandidato + btnAltaEmpleado;


                            // MOSTRAR BOTON DE ALTA SOLO SI TIENE EL 100 % DE LOS DOCUS
                            if (porcent < 100) {
                                return btnVerSegCandidato;

                            } else {
                                if (row.estatus == 2) {
                                    return btnVerSegCandidato;

                                } else {

                                    return btnVerSegCandidato + btnAltaEmpleado;

                                }

                            }

                        }
                    },
                    { data: 'id', visible: false },
                    { data: 'cantFases', visible: false },
                    { data: 'lstFasesID', visible: false },
                    { data: 'lstFasesStr', visible: false },
                    { data: 'progresoSeguimiento', visible: false },
                    { data: 'esSeguimientoCancelado', visible: false },
                    { data: 'idPuesto', title: 'idPuesto', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_SegCandidatos.on('click', '.verSeguimientoCandidato', function () {
                        contadorFases = 0;
                        contadorFasesContruccion = 0;
                        primeraCargaPagina = true;
                        let rowData = dtSegCandidatos.row($(this).closest('tr')).data();
                        btnRegresarSegCandidatos.attr("data-idCandidato", rowData.id);
                        btnRegresarSegCandidatos.attr("data-claveCandidato", rowData.clave_empleado);
                        btnRegresarSegCandidatos.attr("data-puesto", rowData.idPuesto);
                        fncVisualizarSegCandidato(rowData.id, rowData.cantFases, rowData.lstFasesID, rowData.nombreCandidato, rowData.lstFasesStr, rowData.puesto, rowData.esSeguimientoCancelado);
                        fncGetFasesAutorizadas(rowData.idPuesto);
                        dtActividades.clear();
                        dtActividades.draw();

                        inputNombre.val(rowData.nombreCandidato);
                        inputEdad.val(rowData.edad);
                        inputTelefono.val(rowData.telefono);
                        $(".btnFaseCandidato").trigger("click");
                    });

                    tblRH_REC_SegCandidatos.on('click', '.btnAltaEmpleado', function () {
                        let rowData = dtSegCandidatos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('', '¿Desea proceder con alta de datos de empleado de este candidato?', 'Confirmar', 'Cancelar',
                            () => fncAltaEmpleado(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncVisualizarSegCandidato(id, cantFases, lstFasesID, nombreCandidato, lstFasesStr, puesto, esSeguimientoCancelado) {
            //#region SE CONSTRUYE LOS PASOS EN BASE AL CANDIDATO SELECCIONADO
            fncConstruccionFases(cantFases, lstFasesID, nombreCandidato, lstFasesStr, puesto, esSeguimientoCancelado);
            //#endregion

            //#region SE VISUALIZA LA LINEA DE FASES DEL CANDIDATO SELECCIONADO
            fncMostrarFases(true);
            //#endregion
        }

        function fncAltaEmpleado(id) {
            document.location.href = `/Administrativo/Reclutamientos/AltaEmpleados?empleadosAdmn=0&cargarModal=${1}&candidatoID=${id}`;
        }

        function fncGetSegCandidatos() {
            axios.post("GetSegCandidatos", { estatus: txtFiltroEstatus.val(), lstFiltroCC: getValoresMultiples("#cboFiltroCC") }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtSegCandidatos.clear();
                    dtSegCandidatos.rows.add(response.data.lstSegCandidatos);
                    dtSegCandidatos.draw();
                    //#endregion

                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        function initTblEvidencias() {
            dtEvidencias = tblRH_REC_Evidencias.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreArchivo', title: 'Archivo' },
                    { data: 'descripcion', title: 'Descripción', visible: false },
                    {
                        render: function (data, type, row) {
                            return `
                    <button class="btn btn-warning descargarEvidencia btn-xs"><i class="fas fa-print"></i></button>
                    <button class="btn btn-danger eliminarEvidencia btn-xs"><i class="far fa-trash-alt"></i></button>
                    `;
                        }
                    },
                    { data: 'id', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_Evidencias.on('click', '.eliminarEvidencia ', function () {
                        let rowData = dtEvidencias.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarArchivoActividad(rowData.id));
                    });
                    tblRH_REC_Evidencias.on('click', '.descargarEvidencia ', function () {
                        let rowData = dtEvidencias.row($(this).closest('tr')).data();
                        location.href = `/Administrativo/Reclutamientos/DescargarArchivoEvidencia?file_id=${rowData.id}`;
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncEliminarArchivoActividad(id) {
            let obj = new Object();
            obj = {
                idArchivo: id
            }
            axios.post("EliminarArchivoActividad", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetArchivosActividadesRelFase();
                    Alert2Exito("Éxito al eliminar el archivo.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetFasesAutorizadas(idPuesto) {
            axios.post("GetFasesAutorizadas", { idPuesto: idPuesto }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    // $('.btnFaseCandidato').attr("disabled","none");
                    console.log(items)
                    items.forEach(e => {
                        $("a[data-id='" + e.id + "']").attr('disabled', false);
                    });

                }
            }).catch(error => Alert2Error(error.message));
        }

        //#region FNC GRALES

        function fncNoficarActividad(idCandidato, idActividad, estatus, idNotificante) {
            axios.post("NotificarActividad", { idCandidato, idActividad, estatus, idNotificante }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    // Alert2Exito("Notificacion enviada con exito");
                    // fncGetActividadesFase(idFase);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function funGetEmpleado(event, ui) {
            txtSendClave.val(ui.item.id);
            txtSendNombre.val(ui.item.value);
        }
        //#endregion

        //#region ENTREVISTA INICIAL
        function fncGetEntrevistaInicial(idCandidato) {
            let obj = new Object();
            obj = {
                idCandidato: idCandidato
            }
            axios.post("GetEntrevistaInicial", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncLimpiarCtrlsMdl();

                    if (response.data.objEntrevistaInicial != null) {

                        txtCEEntrevistaInicialNombreCompleto.val(response.data.objEntrevistaInicial.apePaterno + " " + response.data.objEntrevistaInicial.apeMaterno + " " +
                            response.data.objEntrevistaInicial.nombre);
                        txtCEEntrevistaInicialEdad.val(response.data.objEntrevistaInicial.edad);
                        txtCEEntrevistaInicialLugarNacimiento.val(response.data.objEntrevistaInicial.lugarNacimiento);
                        txtCEEntrevistaInicialPuestoSolicitado.val(response.data.objEntrevistaInicial.puestoSolicitadoDesc);

                        //#region SE LLENA LOS CONTROLES CON LOS DATOS PREVIAMENTE REGISTRADOS
                        cboCEEntrevistaInicialEscolaridad.val(response.data.objEntrevistaInicial.idEscolaridad);
                        cboCEEntrevistaInicialEscolaridad.trigger("change");
                        cboCEEntrevistaInicialEstadoCivil.val(response.data.objEntrevistaInicial.estadoCivil);
                        cboCEEntrevistaInicialEstadoCivil.trigger("change");
                        txtCEEntrevistaInicialExpectativaSalarial.val(maskNumero(response.data.objEntrevistaInicial.expectativaSalarial));
                        txtCEEntrevistaInicialExperienciaLaboral.val(response.data.objEntrevistaInicial.expLaboral);
                        txtCEEntrevistaInicialSectorCiudad.val(response.data.objEntrevistaInicial.sectorCiudad);
                        txtCEEntrevistaInicialTiempoEnLaCiudad.val(response.data.objEntrevistaInicial.tiempoEnLaCiudad);
                        chkCEEntrevistaInicialEntrevistasAnteriores.prop("checked", response.data.objEntrevistaInicial.entrevistasAnteriores);
                        chkCEEntrevistaInicialEntrevistasAnteriores.trigger('change');
                        cboCEEntrevistaInicialPlataforma.val(response.data.objEntrevistaInicial.idPlataforma);
                        cboCEEntrevistaInicialPlataforma.trigger("change");
                        cboCEEntrevistaInicialDocumentacion.val(response.data.objEntrevistaInicial.documentacion);
                        cboCEEntrevistaInicialDocumentacion.trigger('change');
                        chkCEEntrevistaInicialFamiliarEnLaEmpresa.prop("checked", response.data.objEntrevistaInicial.familiarEnEmpresa);
                        chkCEEntrevistaInicialFamiliarEnLaEmpresa.trigger('change');
                        txtCEEntrevistaInicialFamiliaEnLaEmpresa.val(response.data.objEntrevistaInicial.familiaEnLaEmpresa);
                        txtCEEntrevistaInicialTelefono.val(response.data.objEntrevistaInicial.telefono);
                        txtCEEntrevistaInicialFamilia.val(response.data.objEntrevistaInicial.familia);
                        txtCEEntrevistaInicialEmpleos.val(response.data.objEntrevistaInicial.empleos);
                        txtCEEntrevistaInicialCaracteristicasPersonalesCandidato.val(response.data.objEntrevistaInicial.caracteristicasCandidato);
                        txtCEEntrevistaInicialComentariosEntrevistador.val(response.data.objEntrevistaInicial.comentarioEntrevistador);
                        txtCEEntrevistaInicialFechaEntrevista.val(moment(response.data.objEntrevistaInicial.fechaEntrevista).format("YYYY-MM-DD"));
                        // chkCEEntrevistaInicialDisposicionHorario.prop("checked", response.data.objEntrevistaInicial.disposicionHorario);
                        // chkCEEntrevistaInicialDisposicionHorario.trigger('change');
                        // txtCEEntrevistaInicialDisposicionHorario.val(response.data.objEntrevistaInicial.disposicionHorario);
                        chkCEEntrevistaInicialDisposicionHorario.prop("checked", response.data.objEntrevistaInicial.disposicionHorario.toUpperCase() == "SI" ? true : false);
                        chkCEEntrevistaInicialDisposicionHorario.trigger('change');

                        chkCEEntrevistaInicialAvanza.prop("checked", response.data.objEntrevistaInicial.avanza);
                        chkCEEntrevistaInicialAvanza.trigger('change');
                        cboCEEntrevistaInicialEntrevisto.val(response.data.objEntrevistaInicial.idUsuarioEntrevisto);
                        cboCEEntrevistaInicialEntrevisto.trigger("change");
                        txtCEEntrevistaInicialResultado.val(response.data.objEntrevistaInicial.resultado);
                        txtCEEntrevistaInicialNombreCompleto.attr("data-id", response.data.objEntrevistaInicial.id);

                        chkCEEntrevistaInicialBrutoNeto.val(response.data.objEntrevistaInicial.tipoSalario);
                        chkCEEntrevistaInicialBrutoNeto.trigger("change");
                        txtCEEntrevistaInicialComentariosAvanza.text(response.data.objEntrevistaInicial.comentariosAvanza);

                        btnCEEntrevistaInicial.html("Actualizar");
                        //#endregion
                    } else {
                        txtCEEntrevistaInicialNombreCompleto.attr("data-id", 0);
                        btnCEEntrevistaInicial.html("Guardar");
                        chkCEEntrevistaInicialBrutoNeto.prop("checked", true);
                        chkCEEntrevistaInicialBrutoNeto.trigger("change");
                        chkCEEntrevistaInicialAvanza.prop("checked", false);
                        chkCEEntrevistaInicialAvanza.trigger("change");
                        fncGetUsuarioEntrevistaActual();
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetUsuarioEntrevistaActual() {
            axios.post("GetUsuarioEntrevistaActual").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    cboCEEntrevistaInicialEntrevisto.val(response.data.idUsuario);
                    cboCEEntrevistaInicialEntrevisto.trigger("change");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncLimpiarCtrlsMdl() {
            //#region ENTREVISTA INICIAL
            cboCEEntrevistaInicialEscolaridad[0].selectedIndex = 0;
            cboCEEntrevistaInicialEscolaridad.trigger("change");

            cboCEEntrevistaInicialEstadoCivil[0].selectedIndex = 0;
            cboCEEntrevistaInicialEstadoCivil.trigger("change");

            txtCEEntrevistaInicialExperienciaLaboral.val("");
            chkCEEntrevistaInicialEntrevistasAnteriores.prop("checked", false);
            chkCEEntrevistaInicialFamiliarEnLaEmpresa.prop("checked", false);
            txtCEEntrevistaInicialFamilia.val("");
            txtCEEntrevistaInicialEmpleos.val("");
            txtCEEntrevistaInicialCaracteristicasPersonalesCandidato.val("");
            txtCEEntrevistaInicialComentariosEntrevistador.val("");
            // chkCEEntrevistaInicialDisposicionHorario.prop("checked", true);
            chkCEEntrevistaInicialAvanza.prop("checked", true);
            cboCEEntrevistaInicialEntrevisto[0].selectedIndex = 0;
            cboCEEntrevistaInicialEntrevisto.trigger("change");
            txtCEEntrevistaInicialFamiliaEnLaEmpresa.val("");
            //#endregion
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.SegCandidatos = new SegCandidatos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();