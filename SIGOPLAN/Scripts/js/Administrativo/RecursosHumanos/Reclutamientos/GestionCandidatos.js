(() => {
    $.namespace('CH.GestionCandidatos');

    //#region CONST FILTROS
    const cboFiltroCC = $('#cboFiltroCC');
    const cboFiltroPuesto = $('#cboFiltroPuesto');
    const cboFiltroEstatusCandidato = $('#cboFiltroEstatusCandidato');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroLimpiar = $('#btnFiltroLimpiar');
    const btnFiltroNuevoCandidato = $('#btnFiltroNuevoCandidato');
    const tblRH_REC_GestionCandidatos = $('#tblRH_REC_GestionCandidatos');
    let dtGestionCandidatos;
    //#endregion

    //#region CONST CREAR/EDITAR CANDIDATO
    const mdlCrearEditarCandidato = $('#mdlCrearEditarCandidato');
    const spanTitleCrearEditarCandidato = $('#spanTitleCrearEditarCandidato');
    const txtCrearEditarNombre = $('#txtCrearEditarNombre');
    const txtCrearEditarApePaterno = $('#txtCrearEditarApePaterno');
    const txtCrearEditarApeMaterno = $('#txtCrearEditarApeMaterno');
    const txtCrearEditarCorreo = $('#txtCrearEditarCorreo');
    const txtCrearEditarTelefono = $('#txtCrearEditarTelefono');
    const txtCrearEditarCelular = $('#txtCrearEditarCelular');
    const cboCrearEditarPuesto = $('#cboCrearEditarPuesto');
    const btnCrearEditarCandidato = $('#btnCrearEditarCandidato');
    const spanCrearEditarCandidato = $('#spanCrearEditarCandidato');
    const txtCrearArchivo = $('#txtCrearArchivo');
    const txtCrearEditarNSS = $('#txtCrearEditarNSS');
    const cboCEDatosEmpleadoPaisNac = $('#cboCEDatosEmpleadoPaisNac');
    const cboCEDatosEmpleadoEstadoNac = $('#cboCEDatosEmpleadoEstadoNac');
    const cboCEDatosEmpleadoLugarNac = $('#cboCEDatosEmpleadoLugarNac');
    const txtCrearEditarFechaNacimiento = $('#txtCrearEditarFechaNacimiento');
    const txtCrearEditarEdad = $('#txtCrearEditarEdad');
    const txtCrearEditarAltura = $('#txtCrearEditarAltura');
    const txtCrearEditarPeso = $('#txtCrearEditarPeso');
    const txtCrearEditarNotasReclutador = $('#txtCrearEditarNotasReclutador');
    const cboCrearEditarSexo = $('#cboCrearEditarSexo');
    const chkReingreso = $('#chkReingreso');
    const divCrearEditarReingreso = $('#divCrearEditarReingreso');
    const txtCrearEditarReClave = $('#txtCrearEditarReClave');
    const txtCrearEditarReNombre = $('#txtCrearEditarReNombre');
    const divCrearEditarToggle = $('#divCrearEditarToggle');
    const cboCEDatosEmpleadoDepartamentoNac = $('#cboCEDatosEmpleadoDepartamentoNac');
    const divCeDatosEmpleadosDepartamentoNac = $('#divCeDatosEmpleadosDepartamentoNac');
    const txtCrearEditarCUSPP = $('#txtCrearEditarCUSPP');
    //#endregion

    //#region CONST ARCHIVOS
    const mdlLstArchivos = $('#mdlLstArchivos');
    const tblRH_REC_Archivos = $('#tblRH_REC_Archivos');
    let dtArchivos;
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
    const txtEmpresa = $('#txtEmpresa');
    //#endregion


    let _empresaActual = +txtEmpresa.val();

    GestionCandidatos = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT TABLAS
            initTblGestionCandidatos();
            initTblArchivos();
            //#endregion

            //#region EVENTOS GESTIÓN DE CANDIDATOS
            fncGetCandidatos();

            btnFiltroBuscar.on("click", function () {
                fncGetCandidatos();
            });

            btnCrearEditarCandidato.on("click", function () {
                fncCrearEditarCandidato();
            });

            btnFiltroNuevoCandidato.on("click", function () {
                spanTitleCrearEditarCandidato.html("Guardar candidato");
                btnCrearEditarCandidato.attr("data-id", 0);
                spanCrearEditarCandidato.html("Guardar");
                fncLimpiarCtrlsMdl();
                mdlCrearEditarCandidato.modal("show");
                fncFocus("txtCrearEditarNombre");

                // chkReingreso.prop("checked", false);
                chkReingreso.bootstrapToggle('enable');
                chkReingreso.bootstrapToggle('off');
                // chkReingreso.trigger("change");
                txtCrearEditarReNombre.attr("disabled", false);
                txtCrearEditarReNombre.css('border', '1px solid #CCC');

            });

            cboFiltroCC.fillCombo("/Reclutamientos/FillFiltroCboCC", {}, false);
            cboFiltroPuesto.fillCombo("/Reclutamientos/FillFiltroCboPuestosGestion", {}, false);
            cboCrearEditarPuesto.fillCombo("/Reclutamientos/FillFiltroCboPuestosGestion", {}, false);
            cboCrearEditarPuesto.on("change", function () {
                fncFocus("cboCrearEditarPuesto");
            });

            cboCEDatosEmpleadoPaisNac.fillCombo("/Reclutamientos/FillCboPaises", {}, false);
            cboCEDatosEmpleadoDepartamentoNac.fillCombo("/Reclutamientos/FillComboGeoDepartamentos", {}, false);
            cboCEDatosEmpleadoEstadoNac.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: 0 }, false);
            cboCEDatosEmpleadoLugarNac.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: 0, _claveEstado: 0 }, false);

            cboCEDatosEmpleadoPaisNac.on("change", function () {
                if ($(this).val() != "") {
                    cboCEDatosEmpleadoEstadoNac.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: parseFloat($(this).val()) }, false);
                }

                if ($(this).val() == 7 && txtEmpresa.val() == 6) {
                    divCeDatosEmpleadosDepartamentoNac.show();
                } else {
                    divCeDatosEmpleadosDepartamentoNac.hide();

                }

                $("#select2-cboCEDatosEmpleadoPaisNac-container").css('border', '1px solid #CCC');
                fncFocus("cboCEDatosEmpleadoPaisNac");
            });

            cboCEDatosEmpleadoEstadoNac.on("change", function () {
                if ($(this).val() != "") {
                    cboCEDatosEmpleadoLugarNac.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: cboCEDatosEmpleadoPaisNac.val(), _claveEstado: $(this).val() }, false);
                }

                $("#select2-cboCEDatosEmpleadoEstadoNac-container").css('border', '1px solid #CCC');
                fncFocus("cboCEDatosEmpleadoEstadoNac");
            });

            cboCEDatosEmpleadoDepartamentoNac.on("change", function () {
                if ($(this).val() != "" && txtEmpresa.val() == 6) {
                    cboCEDatosEmpleadoEstadoNac.fillCombo("/Reclutamientos/FillCboEstadosPERU", { claveDepartamento: parseFloat($(this).val()) }, false);
                }

                fncFocus("cboCEDatosEmpleadoDepartamentoNacIndex");

            });

            cboCEDatosEmpleadoLugarNac.on("change", function () {
                $("#select2-cboCEDatosEmpleadoLugarNac-container").css('border', '1px solid #CCC');
                fncFocus("cboCEDatosEmpleadoLugarNac");
            });

            cboCEDatosEmpleadoPaisNac.select2({ width: "100%" });
            cboCEDatosEmpleadoEstadoNac.select2({ width: "100%" });
            cboCEDatosEmpleadoLugarNac.select2({ width: "100%" });

            cboFiltroCC.select2({ width: 'resolve' });
            cboFiltroPuesto.select2({ width: 'resolve' });
            cboCrearEditarPuesto.select2({ width: '100%' });

            cboFiltroEstatusCandidato.select2({ width: '100%' });

            btnFiltroLimpiar.on("click", function () {
                cboFiltroCC[0].selectedIndex = 0;
                cboFiltroCC.trigger("change");
                cboFiltroPuesto[0].selectedIndex = 0;
                cboFiltroPuesto.trigger("change");
            });

            txtCrearEditarFechaNacimiento.change(calcularEdad);

            cboCrearEditarSexo.on("change", function () {
                fncFocus("cboCrearEditarSexo");
            });
            //#endregion

            //#region EVENTOS ENTREVISTA INICIAL
            btnCEEntrevistaInicial.on("click", function () {
                fncCrearEditarEntrevistaInicial(btnCEEntrevistaInicial.attr("data-idCandidato"));

            });

            chkCEEntrevistaInicialFamiliarEnLaEmpresa.on("change", function () {
                if ($(this).prop("checked")) {
                    divFamiliaEnLaEmpresa.css("display", "block");
                    // divFamiliaEnLaEmpresa.css("width", "100% !important");
                } else {
                    divFamiliaEnLaEmpresa.css("display", "none");
                }
            });

            //#region FILL COMBOS
            cboCEEntrevistaInicialEscolaridad.fillCombo("/Reclutamientos/FillCboEscolaridades", {}, false);
            cboCEEntrevistaInicialEscolaridad.select2({ width: "100%" });

            cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("--Seleccione--").text("--Seleccione--"));
            cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("Casado").text("Casado"));
            cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("Divorciado").text("Divorciado"));
            cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("Soltero").text("Soltero"));

            if (_empresaActual == 6) {
                cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("Union Libre").text("Conviviente"));
            } else {
                cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("Union Libre").text("Unión libre"));
            }

            cboCEEntrevistaInicialEstadoCivil.append($("<option />").val("Viudo").text("Viudo"));
            cboCEEntrevistaInicialEstadoCivil.select2({ width: "100%" });

            cboCEEntrevistaInicialPlataforma.fillCombo("/Reclutamientos/FillCboPlataformas", {}, false);
            cboCEEntrevistaInicialPlataforma.select2({ width: "100%" });

            cboCEEntrevistaInicialEntrevisto.fillCombo("/Reclutamientos/FillCboUsuarios", {}, false);
            cboCEEntrevistaInicialEntrevisto.select2({ width: "100%" });

            cboCEEntrevistaInicialDocumentacion.select2({ width: "100%" })
            //#endregion

            //#endregion

            txtCEEntrevistaInicialFechaEntrevista.val(moment().format("YYYY-MM-DD"));

            chkCEEntrevistaInicialAvanza.on("change", function () {
                if (!$(this).prop("checked")) {
                    divCEEntrevistaInicialAvanza.show();
                } else {
                    divCEEntrevistaInicialAvanza.hide();
                }
            });

            txtCEEntrevistaInicialExpectativaSalarial.on("change", function () {
                $(this).val(maskNumero($(this).val()));
            });

            chkReingreso.on("change", function () {
                if ($(this).prop("checked")) {
                    divCrearEditarReingreso.css("display", "initial");
                } else {
                    divCrearEditarReingreso.css("display", "none");
                    txtCrearEditarReClave.val("");
                    txtCrearEditarReNombre.val("");
                }
                // chkReingreso.attr("disabled", false);
                // txtCrearEditarReNombre.attr("disabled", false);
            });

            txtCrearEditarReClave.on("change", function () {
                if ($(this).val() != "") {
                    fncGetInfoReingreso();

                }
            });

            txtCrearEditarReNombre.getAutocomplete(funGetEmpleado, null, '/Reclutamientos/getAutoCompleteCandidatos');

            txtCrearEditarReNombre.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
        }

        //#region CRUD GESTIÓN CANDIDATOS
        function initTblGestionCandidatos() {
            dtGestionCandidatos = tblRH_REC_GestionCandidatos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                scrollX: false,
                columns: [
                    { data: 'nombreCompleto', title: 'Nombre' },
                    { data: 'puestoDesc', title: 'Vacante', width: '300px' },
                    { data: 'correo', title: 'Correo' },
                    { data: 'celular', title: 'Celular' },
                    { data: 'estatus', title: 'Estatus' },
                    {
                        title: 'Fecha alta',
                        render: function (data, type, row) {
                            return new Date(parseInt(row.fechaCreacion.replace(/[^0-9|\-|\+]+/g, ''))).toLocaleDateString();
                        }
                    },
                    {
                        title: 'CV',
                        render: function (data, type, row) {
                            if (row.tieneArchivo) {
                                return `<button class="btn btn-primary btn-xs botonDescargarCV" title="Descargar CV."><i class="fas fa-print"></i></button>`;
                            } else {
                                return ``;
                            }
                        }
                    },
                    {
                        render: function (data, type, row) {
                            var entrevista = row.tieneEntrevista ? 'btn-success' : 'btn-primary';
                            // return `
                            //     <button title='Entrevista inicial' class="btn ${entrevista} entrevistaInicial btn-xs"><i class="far fa-comments"></i></button>
                            //     <button title='Eliminar documento' class="btn btn-default lstArchivos btn-xs"><i class="far fa-folder-open"></i></button>
                            //     <button title='Actualizar candidato' class="btn btn-warning actualizarCandidato btn-xs"><i class="far fa-edit"></i></button>
                            //     <button title='Eliminar candidato' class="btn btn-danger eliminarCandidato btn-xs"><i class="far fa-trash-alt"></i></button>
                            // `;

                            return `
                                <button title='Entrevista inicial' class="btn ${entrevista} entrevistaInicial btn-xs"><i class="far fa-comments"></i></button>
                                <button title='Actualizar candidato' class="btn btn-warning actualizarCandidato btn-xs"><i class="far fa-edit"></i></button>
                                <button title='Eliminar candidato' class="btn btn-danger eliminarCandidato btn-xs"><i class="far fa-trash-alt"></i></button>
                            `;
                        }
                    },
                    { data: 'telefono', title: 'Teléfono', visible: false },
                    { data: 'nombre', visible: false },
                    { data: 'apePaterno', visible: false },
                    { data: 'apeMaterno', visible: false },
                    { data: 'idGestionSolicitud', visible: false },
                    { data: 'id', visible: false },
                    { data: 'fechaNacimiento', visible: false },
                    { data: 'altura', visible: false },
                    { data: 'peso', visible: false },
                    { data: 'edad', visible: false },
                    { data: 'idPuesto', visible: false },
                    { data: 'puestoDesc', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_GestionCandidatos.on('click', '.lstArchivos', function () {
                        let rowData = dtGestionCandidatos.row($(this).closest('tr')).data();
                        fncGetArchivosCandidato(rowData.id);
                        mdlLstArchivos.modal("show");
                    });
                    tblRH_REC_GestionCandidatos.on('click', '.actualizarCandidato', function () {
                        let rowData = dtGestionCandidatos.row($(this).closest('tr')).data();
                        txtCrearEditarNombre.val(rowData.nombre);
                        txtCrearEditarApePaterno.val(rowData.apePaterno);
                        txtCrearEditarApeMaterno.val(rowData.apeMaterno);
                        txtCrearEditarCorreo.val(rowData.correo);
                        txtCrearEditarTelefono.val(rowData.telefono);
                        txtCrearEditarCelular.val(rowData.celular);
                        cboCrearEditarPuesto.val('');
                        cboCrearEditarPuesto.trigger('change');
                        cboCrearEditarPuesto.find('[data-prefijo=' + rowData.idGestionSolicitud + ']').prop('selected', true).trigger('change');
                        txtCrearEditarFechaNacimiento.val(moment(rowData.fechaNacimiento).format("YYYY-MM-DD"));
                        txtCrearEditarFechaNacimiento.change();
                        txtCrearEditarAltura.val(rowData.altura);
                        txtCrearEditarPeso.val(rowData.peso);
                        btnCrearEditarCandidato.attr("data-id", rowData.id);
                        spanCrearEditarCandidato.html("Actualizar");
                        spanTitleCrearEditarCandidato.html("Actualizar candidato");
                        cboCEDatosEmpleadoPaisNac.val(rowData.pais);
                        cboCEDatosEmpleadoPaisNac.trigger("change");
                        cboCEDatosEmpleadoDepartamentoNac.val(rowData.PERU_departamento);
                        cboCEDatosEmpleadoDepartamentoNac.change();
                        cboCEDatosEmpleadoEstadoNac.val(rowData.estado);
                        cboCEDatosEmpleadoEstadoNac.trigger("change");
                        cboCEDatosEmpleadoLugarNac.val(rowData.municipio);
                        cboCEDatosEmpleadoLugarNac.trigger("change");
                        txtCrearEditarNotasReclutador.val(rowData.notasReclutador);
                        mdlCrearEditarCandidato.modal("show");
                        cboCrearEditarSexo.val(rowData.sexo ?? "");
                        cboCrearEditarSexo.trigger("change");
                        txtCrearEditarCUSPP.val(rowData.cuspp);

                        chkReingreso.bootstrapToggle('enable');
                        if (rowData.esReingreso) {
                            // chkReingreso.prop("checked", true);
                            chkReingreso.bootstrapToggle('on');
                            // chkReingreso.trigger("change");

                            txtCrearEditarReNombre.attr("disabled", true);
                            txtCrearEditarReClave.attr("disabled", true);

                            txtCrearEditarReNombre.val(rowData.nombre + " " + rowData.apePaterno + " " + rowData.apeMaterno);
                            txtCrearEditarReClave.val(rowData.clave_empleado);

                        } else {
                            chkReingreso.bootstrapToggle('off')
                            // chkReingreso.trigger("change");


                            txtCrearEditarReClave.val("");
                            txtCrearEditarReNombre.val("");

                        }
                        // chkReingreso.attr("disabled", true);
                        chkReingreso.bootstrapToggle('disable');


                    });
                    tblRH_REC_GestionCandidatos.on('click', '.eliminarCandidato', function () {
                        let rowData = dtGestionCandidatos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCandidato(rowData.id));
                    });
                    tblRH_REC_GestionCandidatos.on("click", ".entrevistaInicial", function () {
                        fncLimpiarCtrlsMdl();
                        fncDefaultBorderEntrevista();
                        let rowData = dtGestionCandidatos.row($(this).closest('tr')).data();

                        btnCEEntrevistaInicial.attr("data-idCandidato", rowData.id);

                        txtCEEntrevistaInicialNombreCompleto.val(rowData.nombreCompleto);
                        btnCEEntrevistaInicial.attr("puesto-id", rowData.idPuesto);
                        txtCEEntrevistaInicialEdad.val(rowData.edad);
                        txtCEEntrevistaInicialLugarNacimiento.val(rowData.municipioDesc);
                        txtCEEntrevistaInicialPuestoSolicitado.val(rowData.puestoDesc);
                        txtCEEntrevistaInicialPuestoSolicitado.attr("puesto-id", rowData.puesto);

                        txtCrearEditarTelefono
                        txtCrearEditarCelular

                        if (rowData.celular != "") {
                            txtCEEntrevistaInicialTelefono.val(rowData.celular);

                        } else {
                            txtCEEntrevistaInicialTelefono.val(rowData.telefono);

                        }


                        // puestoSolicitado: btnCEEntrevistaInicial.attr("puesto-id"),

                        btnCEEntrevistaInicial.html("Actualizar");
                        fncGetEntrevistaInicial(rowData.id);
                        mdlCrearEditarEntrevistaInicial.modal("show");
                    });
                    tblRH_REC_GestionCandidatos.on('click', '.botonDescargarCV', function () {
                        let rowData = dtGestionCandidatos.row($(this).closest('tr')).data();

                        location.href = `/Administrativo/Reclutamientos/DescargarArchivoCV?candidatoID=${rowData.id}`;
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { "width": "2%", "targets": 2 },
                    { "width": "2%", "targets": 3 },
                    { "width": "6%", "targets": 4 },
                    { "width": "6%", "targets": 5 },
                    { "width": "3%", "targets": 6 },
                    { "width": "13%", "targets": 7 }
                ],
            });
        }

        function fncGetCandidatos() {
            let objFiltro = fncObjFiltro();
            axios.post("GetCandidatos", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtGestionCandidatos.clear();
                    dtGestionCandidatos.rows.add(response.data.lstCandidatos);
                    dtGestionCandidatos.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncObjFiltro() {
            let strFiltro = $('select[id="cboFiltroPuesto"] option:selected').text();
            let objFiltro = new Object();
            objFiltro = {
                cc: cboFiltroCC.val(),
                idPuesto: cboFiltroPuesto.val(),
                puesto: strFiltro == "--Seleccione--" ? "" : strFiltro,
                idEstatus: cboFiltroEstatusCandidato.val(),
                idGestionSolicitud: cboFiltroPuesto.find('option:selected').data('prefijo')
            };
            return objFiltro;
        }

        function fncEliminarCandidato(idCandidato) {
            let objCandidato = new Object();
            objCandidato = {
                idCandidato: idCandidato
            };
            axios.post("EliminarCandidato", objCandidato).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetCandidatos();
                    Alert2Exito("Se ha eliminado con éxito el registro");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarCandidato() {
            let objCandidato = fncObjCandidato();
            if (objCandidato != null) {
                axios.post("CrearEditarCandidato", objCandidato, { headers: { 'Content-Type': 'multipart/form-data' } })
                    .then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            fncGetCandidatos();
                            mdlCrearEditarCandidato.modal("hide");
                            let strMensaje = btnCrearEditarCandidato.attr("data-id") > 0 ?
                                "Se ha actualizado con éxito al candidato." :
                                "Se ha registrado con éxito al candidato";
                            Alert2Exito(strMensaje);
                        } else {
                            if (response.data.esReingreso != undefined) {
                                if (response.data.esReingreso) {
                                    chkReingreso.bootstrapToggle("on");
                                } else {
                                    chkReingreso.bootstrapToggle("off");

                                }
                            }

                            if (response.data.esReingresoClave != undefined) {
                                txtCrearEditarReNombre.css('border', '2px solid red');
                            }
                            Alert2Error(`No se guardó la información. ${response.data.message}`);
                        }
                    }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCandidato() {

            if (cboCrearEditarPuesto.val() == "") {
                Alert2Warning('Seleccione un puesto.');
                return null;
            }

            var puestoDescripcion = cboCrearEditarPuesto.find('option:selected').text();
            var puestoPalabras = puestoDescripcion.split(' ');
            puestoDescripcion = '';
            for (let index = 2; index < puestoPalabras.length; index++) {
                puestoDescripcion += puestoPalabras[index] + ' ';
            }
            puestoDescripcion = puestoDescripcion.trim();

            if (txtEmpresa.val() == 6) {
                if (txtCrearEditarCelular.val() != "" || txtCrearEditarCelular.val() != null) {
                    if (txtCrearEditarCelular.val().length != 9) {
                        Alert2Warning('El campo de celular debe contener 9 digitos.');
                        return null;
                    }
                }

                if (txtCrearEditarTelefono.val() != "" || txtCrearEditarTelefono.val() != null) {
                    if (txtCrearEditarTelefono.val().length != 9) {
                        Alert2Warning('El campo de telefono debe contener 9 digitos.');
                        return null;
                    }
                }
            } else {
                if (txtCrearEditarCelular.val() != "" || txtCrearEditarCelular.val() != null) {
                    if (txtCrearEditarCelular.val().length != 10) {
                        Alert2Warning('El campo de celular debe contener 10 digitos.');
                        return null;
                    }
                }

                if (txtCrearEditarTelefono.val() != "" || txtCrearEditarTelefono.val() != null) {
                    if (txtCrearEditarTelefono.val().length != 10) {
                        Alert2Warning('El campo de telefono debe contener 10 digitos.');
                        return null;
                    }
                }

            }

            const archivoContrato = txtCrearArchivo.get(0).files[0];
            let objCandidato = new Object();
            objCandidato = {
                nombre: txtCrearEditarNombre.val(),
                apePaterno: txtCrearEditarApePaterno.val(),
                apeMaterno: txtCrearEditarApeMaterno.val(),
                correo: txtCrearEditarCorreo.val(),
                telefono: txtCrearEditarTelefono.val(),
                celular: txtCrearEditarCelular.val(),
                idGestionSolicitud: cboCrearEditarPuesto.find('option:selected').data('prefijo'),
                idPuesto: cboCrearEditarPuesto.val(),
                puestoDesc: puestoDescripcion,
                nss: txtCrearEditarNSS.val(),
                pais: cboCEDatosEmpleadoPaisNac.val(),
                paisDesc: cboCEDatosEmpleadoPaisNac.find('option:selected').text(),
                estado: cboCEDatosEmpleadoEstadoNac.val(),
                estadoDesc: cboCEDatosEmpleadoEstadoNac.find('option:selected').text(),
                municipio: cboCEDatosEmpleadoLugarNac.val(),
                municipioDesc: cboCEDatosEmpleadoLugarNac.find('option:selected').text(),
                fechaNacimiento: txtCrearEditarFechaNacimiento.val(),
                altura: txtCrearEditarAltura.val(),
                peso: txtCrearEditarPeso.val(),
                id: btnCrearEditarCandidato.attr("data-id"),
                notasReclutador: txtCrearEditarNotasReclutador.val(),
                sexo: cboCrearEditarSexo.val(),
                clave_empleado: txtCrearEditarReClave.val(),
                esCandiReingreso: chkReingreso.prop("checked"),
                cuspp: txtCrearEditarCUSPP.val(),
                PERU_departamento: cboCEDatosEmpleadoDepartamentoNac.val(),
                PERU_descDepartamento: cboCEDatosEmpleadoDepartamentoNac.find('option:selected').text(),
            }
            let formData = new FormData();
            formData.set('objFile', archivoContrato);
            formData.set('objCandidato', JSON.stringify(objCandidato));

            return formData;
        }

        function fncLimpiarCtrlsMdl() {
            //$('input[type="text"]').val('');
            $("input[type='text']").val("");
            $("input[type='number']").val("");
            $("input[type='date']").val("");
            $("textarea").val("");

            txtCrearEditarCorreo.val("@");
            cboCrearEditarPuesto[0].selectedIndex = 0;
            cboCrearEditarPuesto.trigger("change");

            txtCrearEditarTelefono.val(null);
            txtCrearEditarCelular.val(null);

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

            cboCrearEditarSexo[0].selectedIndex = 0;
            cboCrearEditarSexo.trigger("change");
            cboCEDatosEmpleadoPaisNac[0].selectedIndex = 0;
            cboCEDatosEmpleadoPaisNac.trigger("change");
            cboCEDatosEmpleadoEstadoNac[0].selectedIndex = 0;
            cboCEDatosEmpleadoEstadoNac.trigger("change");
            cboCEDatosEmpleadoLugarNac[0].selectedIndex = 0;
            cboCEDatosEmpleadoLugarNac.trigger("change");

            txtCrearEditarFechaNacimiento.val('');
            txtCrearEditarNotasReclutador.val('');

            txtCrearArchivo.val("");

            txtCEEntrevistaInicialFechaEntrevista.val(moment().format("YYYY-MM-DD"));

        }

        function calcularEdad() {
            var today = new Date();
            var birthDate = new Date(txtCrearEditarFechaNacimiento.val());
            var age = today.getFullYear() - birthDate.getFullYear();
            var m = today.getMonth() - birthDate.getMonth();
            if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
                age--;
            }

            txtCrearEditarEdad.val(age);
        }
        //#endregion

        //#region CRUD ARCHIVOS CANDIDATO
        function initTblArchivos() {
            dtArchivos = tblRH_REC_Archivos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: "nombreArchivo", title: 'Archivo' },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-danger eliminarArchivo btn-xs"><i class="far fa-trash-alt"></i></button>`;
                        }
                    },
                    { data: 'id', visible: false },
                    { data: 'idCandidato', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_Archivos.on('click', '.eliminarArchivo', function () {
                        let rowData = dtArchivos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarArchivoCandidato(rowData.idCandidato, rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetArchivosCandidato(idCandidato) {
            let obj = new Object();
            obj = {
                idCandidato: idCandidato
            }
            axios.post("GetArchivosCandidato", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtArchivos.clear();
                    dtArchivos.rows.add(response.data.lstArchivos);
                    dtArchivos.draw();
                    //#endregion
                } else {
                    //Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarArchivoCandidato(idCandidato, idArchivo) {
            let objEliminar = new Object();
            objEliminar = {
                idArchivo: idArchivo
            }
            axios.post("EliminarArchivoCandidato", objEliminar).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetArchivosCandidato(idCandidato);
                    Alert2Exito("Se ha eliminado con éxito el registro.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region CRUD ENTREVISTA INICIAL
        function fncGetEntrevistaInicial(idCandidato) {
            let obj = new Object();
            obj = {
                idCandidato: idCandidato
            }
            axios.post("GetEntrevistaInicial", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (response.data.objEntrevistaInicial.id > 0) {
                        //fncLimpiarCtrlsMdl();
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

                        chkCEEntrevistaInicialDisposicionHorario.prop("checked", (response.data.objEntrevistaInicial.disposicionHorario != null ? response.data.objEntrevistaInicial.disposicionHorario.toUpperCase() : "NO") == "SI" ? true : false);
                        chkCEEntrevistaInicialDisposicionHorario.trigger('change');
                        // txtCEEntrevistaInicialDisposicionHorario.val(response.data.objEntrevistaInicial.disposicionHorario);
                        chkCEEntrevistaInicialAvanza.prop("checked", response.data.objEntrevistaInicial.avanza);
                        chkCEEntrevistaInicialAvanza.trigger('change');
                        cboCEEntrevistaInicialEntrevisto.val(response.data.objEntrevistaInicial.idUsuarioEntrevisto);
                        cboCEEntrevistaInicialEntrevisto.trigger("change");
                        txtCEEntrevistaInicialResultado.val(response.data.objEntrevistaInicial.resultado);
                        txtCEEntrevistaInicialNombreCompleto.attr("data-id", response.data.objEntrevistaInicial.id);

                        chkCEEntrevistaInicialBrutoNeto.val(response.data.objEntrevistaInicial.tipoSalario);
                        chkCEEntrevistaInicialBrutoNeto.trigger("change");
                        txtCEEntrevistaInicialComentariosAvanza.val(response.data.objEntrevistaInicial.comentariosAvanza);

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

        function fncCrearEditarEntrevistaInicial(idObj) {
            fncDefaultBorderEntrevista();

            let obj = fncObjCEEntrevistaInicial(Number(idObj));
            if (obj != "") {
                axios.post("CrearEditarEntrevistaInicial", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //fncGetEntrevistaInicial(obj.id)
                        Alert2Exito(response.data.strMensaje);
                        mdlCrearEditarEntrevistaInicial.modal("hide");
                        // $("#mdlCrearEditarEntrevistaInicial .close").click();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }

        }

        function fncObjCEEntrevistaInicial() {
            let strMensajeError = "";

            if (txtCEEntrevistaInicialEdad.val() == "") { txtCEEntrevistaInicialEdad.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEEntrevistaInicialEscolaridad.val() == "") { $("#select2-cboCEEntrevistaInicialEscolaridad-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEEntrevistaInicialEstadoCivil.val() == "--Seleccione-") { $("#select2-cboCEEntrevistaInicialEstadoCivil-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEEntrevistaInicialExpectativaSalarial.val() == "") { txtCEEntrevistaInicialExpectativaSalarial.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEEntrevistaInicialExperienciaLaboral.val() == "") { txtCEEntrevistaInicialExperienciaLaboral.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEEntrevistaInicialSectorCiudad.val() == "") { txtCEEntrevistaInicialSectorCiudad.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEEntrevistaInicialTiempoEnLaCiudad.val() == "") { txtCEEntrevistaInicialTiempoEnLaCiudad.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEEntrevistaInicialPlataforma.val() == "") { $("#select2-cboCEEntrevistaInicialPlataforma-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEEntrevistaInicialFamilia.val() == "") { txtCEEntrevistaInicialFamilia.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEEntrevistaInicialEmpleos.val() == "") { txtCEEntrevistaInicialEmpleos.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEEntrevistaInicialCaracteristicasPersonalesCandidato.val() == "") { txtCEEntrevistaInicialCaracteristicasPersonalesCandidato.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEEntrevistaInicialComentariosEntrevistador.val() == "") { txtCEEntrevistaInicialComentariosEntrevistador.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            // if (txtCEEntrevistaInicialDisposicionHorario.val() == "") { txtCEEntrevistaInicialDisposicionHorario.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEEntrevistaInicialEntrevisto.val() == "") { $("#select2-cboCEEntrevistaInicialEntrevisto-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            if (strMensajeError == "") {
                let obj = new Object();
                obj = {
                    id: txtCEEntrevistaInicialNombreCompleto.attr("data-id"),
                    idCandidato: btnCEEntrevistaInicial.attr("data-idCandidato"),
                    idEscolaridad: cboCEEntrevistaInicialEscolaridad.val(),
                    estadoCivil: cboCEEntrevistaInicialEstadoCivil.val(),
                    lugarNacimiento: txtCEEntrevistaInicialLugarNacimiento.val(),
                    expectativaSalarial: unmaskNumero(txtCEEntrevistaInicialExpectativaSalarial.val()),
                    puestoSolicitado: btnCEEntrevistaInicial.attr("puesto-id"),
                    expLaboral: txtCEEntrevistaInicialExperienciaLaboral.val(),
                    sectorCiudad: txtCEEntrevistaInicialSectorCiudad.val(),
                    tiempoEnLaCiudad: txtCEEntrevistaInicialTiempoEnLaCiudad.val(),
                    entrevistasAnteriores: chkCEEntrevistaInicialEntrevistasAnteriores.prop("checked"),
                    idPlataforma: cboCEEntrevistaInicialPlataforma.val(),
                    documentacion: cboCEEntrevistaInicialDocumentacion.val(),
                    familiarEnEmpresa: chkCEEntrevistaInicialFamiliarEnLaEmpresa.prop("checked"),
                    telefono: txtCEEntrevistaInicialTelefono.val(),
                    familia: txtCEEntrevistaInicialFamilia.val(),
                    empleos: txtCEEntrevistaInicialEmpleos.val(),
                    caracteristicasCandidato: txtCEEntrevistaInicialCaracteristicasPersonalesCandidato.val(),
                    comentarioEntrevistador: txtCEEntrevistaInicialComentariosEntrevistador.val(),
                    fechaEntrevista: txtCEEntrevistaInicialFechaEntrevista.val(),
                    disposicionHorario: chkCEEntrevistaInicialDisposicionHorario.prop("checked") ? "SI" : "NO",
                    // disposicionHorario: txtCEEntrevistaInicialDisposicionHorario.val(),
                    avanza: chkCEEntrevistaInicialAvanza.prop("checked"),
                    idUsuarioEntrevisto: cboCEEntrevistaInicialEntrevisto.val(),
                    resultado: txtCEEntrevistaInicialResultado.val(),
                    tipoSalario: chkCEEntrevistaInicialBrutoNeto.prop("checked"),
                    comentariosAvanza: txtCEEntrevistaInicialComentariosAvanza.val(),
                    familiaEnLaEmpresa: chkCEEntrevistaInicialFamiliarEnLaEmpresa.prop("checked") ? txtCEEntrevistaInicialFamiliaEnLaEmpresa.val() : ""
                }
                return obj;
            } else {
                Alert2Warning(strMensajeError);
                return "";
            }
        }

        function fncDefaultBorderEntrevista() {
            // $("#select2-cboCandidatosAprobados-container").css('border', '1px solid #CCC');
            // txtCEDatosEmpleadoNombre.css('border', '1px solid #CCC');
            txtCEEntrevistaInicialEdad.css('border', '1px solid #CCC');
            $("#select2-cboCEEntrevistaInicialEscolaridad-container").css('border', '1px solid #CCC');
            $("#select2-cboCEEntrevistaInicialEstadoCivil-container").css('border', '1px solid #CCC');
            txtCEEntrevistaInicialExpectativaSalarial.css('border', '1px solid #CCC');
            txtCEEntrevistaInicialExperienciaLaboral.css('border', '1px solid #CCC');
            txtCEEntrevistaInicialSectorCiudad.css('border', '1px solid #CCC');
            txtCEEntrevistaInicialTiempoEnLaCiudad.css('border', '1px solid #CCC');
            $("#select2-cboCEEntrevistaInicialPlataforma-container").css('border', '1px solid #CCC');
            txtCEEntrevistaInicialFamilia.css('border', '1px solid #CCC');
            txtCEEntrevistaInicialEmpleos.css('border', '1px solid #CCC');
            txtCEEntrevistaInicialCaracteristicasPersonalesCandidato.css('border', '1px solid #CCC');
            txtCEEntrevistaInicialComentariosEntrevistador.css('border', '1px solid #CCC');
            // txtCEEntrevistaInicialDisposicionHorario.css('border', '1px solid #CCC');
            $("#select2-cboCEEntrevistaInicialEntrevisto-container").css('border', '1px solid #CCC');
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncFocus(obj) {
            if (obj != "") {
                setTimeout(() => $(`#${obj}`).focus(), 50);
            }
        }

        function funGetEmpleado(event, ui) {
            txtCrearEditarReClave.val(ui.item.id);
            txtCrearEditarReNombre.val(ui.item.value);

            txtCrearEditarReClave.trigger("change");
        }

        function fncGetInfoReingreso() {
            axios.post("GetDatosActualizarEmpleado", { claveEmpleado: txtCrearEditarReClave.val(), esReingresoEmpleado: chkReingreso.prop("checked") }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncLimpiarCtrlsMdl();

                    txtCrearEditarReClave.val(response.data.lstDatos[0].clave_empleado);
                    txtCrearEditarReNombre.val(response.data.lstDatos[0].nombre + " " + response.data.lstDatos[0].ape_paterno + " " + response.data.lstDatos[0].ape_materno);

                    txtCrearEditarNombre.val(response.data.lstDatos[0].nombre);
                    txtCrearEditarApePaterno.val(response.data.lstDatos[0].ape_paterno);
                    txtCrearEditarApeMaterno.val(response.data.lstDatos[0].ape_materno);
                    txtCrearEditarCorreo.val(response.data.lstGenerales[0].email);
                    txtCrearEditarTelefono.val(response.data.lstGenerales[0].tel_casa);
                    txtCrearEditarCelular.val(response.data.lstGenerales[0].tel_cel);
                    txtCrearEditarFechaNacimiento.val(moment(response.data.lstDatos[0].fecha_nacString).format('YYYY-MM-DD'));
                    txtCrearEditarEdad.val(response.data.lstDatos[0].edad);
                    cboCrearEditarSexo.val(response.data.lstDatos[0].sexo);
                    cboCrearEditarSexo.trigger("change");
                    cboCEDatosEmpleadoPaisNac.val(response.data.lstDatos[0].clave_pais_nac);
                    cboCEDatosEmpleadoPaisNac.trigger("change");
                    if (_empresaActual == 6) {

                        txtCrearEditarCUSPP.val(response.data.lstDatos[0].cuspp);
                        cboCEDatosEmpleadoDepartamentoNac.val(response.data.lstDatos[0].clave_departamento_nac_PERU);
                        cboCEDatosEmpleadoDepartamentoNac.trigger("change");
                    }

                    cboCEDatosEmpleadoEstadoNac.val(response.data.lstDatos[0].clave_estado_nac);
                    cboCEDatosEmpleadoEstadoNac.trigger("change");
                    cboCEDatosEmpleadoLugarNac.val(response.data.lstDatos[0].clave_ciudad_nac);
                    cboCEDatosEmpleadoLugarNac.trigger("change");
                } else {
                    fncLimpiarCtrlsMdl();
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.GestionCandidatos = new GestionCandidatos();
    })

        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();