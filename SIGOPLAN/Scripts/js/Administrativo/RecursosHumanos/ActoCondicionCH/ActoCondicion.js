(() => {
    $.namespace('Administrativo.ActoCondicion');

    ActoCondicion = function () {
        //#region CONST ARCHIVOS ADJUNTOS
        const seccionAdjuntarArchivos = $('#seccionAdjuntarArchivos')
        const btnGuardarArchivoAdjunto = $("#btnGuardarArchivoAdjunto")
        const tablaArchivosAdjuntos = $("#tablaArchivosAdjuntos")
        let dtArchivoAdjunto;
        //#endregion

        //#region CONST GENERAR REPORTE
        const mdlReportes = $('#mdlReportes')
        const cboTipoReporte = $('#cboTipoReporte')
        const btnGenerarReporte = $('#btnGenerarReporte')

        // AMONESTACIÓN
        const divAmonestacion = $('#divAmonestacion')
        const txtCiudad = $("#txtCiudad")
        const txtContenido = $("#txtContenido")
        // END: AMONESTACIÓN

        // SUSPENSIÓN
        const divSuspension = $("#divSuspension")
        const txtSuspension_Ciudad = $("#txtSuspension_Ciudad")
        const txtSuspension_Contenido = $("#txtSuspension_Contenido")
        // END: SUSPENSIÓN

        // CARTA DE RESPONSABILIDAD
        const divCartaResponsabilidad = $("#divCartaResponsabilidad")
        const txtCartaResponsabilidad_Ciudad = $("#txtCartaResponsabilidad_Ciudad")
        const txtCartaResponsabilidad_Contenido = $("#txtCartaResponsabilidad_Contenido")
        // END: CARTA DE RESPONSABILIDAD

        // ACTA ADMINISTRATIVA
        const divActaAdministrativa = $("#divActaAdministrativa")
        const txtActaAdministrtiva_Ciudad = $("#txtActaAdministrtiva_Ciudad")
        const txtActaAdministrtiva_Articulos = $('#txtActaAdministrtiva_Articulos')
        // END: ACTA ADMINISTRATIVA
        //#endregion

        // FILTROS
        const comboCC = $('#comboCC');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const comboSupervisor = $('#comboSupervisor');
        const comboDepartamento = $('#comboDepartamento');
        const comboFiltroSubclasificacionDepartamento = $('#comboFiltroSubclasificacionDepartamento');
        const comboEstatus = $('#comboEstatus');
        const comboFiltroClasificacion = $('#comboFiltroClasificacion');
        const botonBuscar = $('#botonBuscar');

        const report = $('#report');

        const botonDescargarReporteExcel = $('#botonDescargarReporteExcel');

        // Tabla Acto / Condición.
        const botonAgregar = $('#botonAgregar');
        const tablaRiesgos = $("#tablaRiesgos");
        let dtTablaRiesgos;

        // Modal Crear / Editar.
        const modalActoCondicion = $('#modalActoCondicion');
        const divRadioOpciones = $('#divRadioOpciones');
        const botonGuardar = $('#botonGuardar');

        // Cargar zip
        const botonCargaZip = $('#botonCargaZip');
        const mdlCargarZip = $('#mdlCargarZip');
        const inputArchivoZip = $('#inputArchivoZip');
        const btnDescargarFormato = $('#btnDescargarFormato');
        const btnGuardarZip = $('#btnGuardarZip');

        // Acto.
        const divActo = $('#divActo');
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const botonVerHistorial = $('#botonVerHistorial');
        const inputNombreEmpleado = $('#inputNombreEmpleado');
        const divRadioTipoActo = $('#divRadioTipoActo');
        const divFechaIngreso = $('#divFechaIngreso');
        const txtClasificacionGeneral = $('#txtClasificacionGeneral')
        const inputFechaIngreso = $('#inputFechaIngreso');
        const comboAccion = $('#comboAccion');
        const inputPuestoEmpleado = $('#inputPuestoEmpleado');
        const divContratista = $('#divContratista');
        const comboContratista = $('#comboContratista');
        const inputTipoInfraccion = $('#inputTipoInfraccion');
        const btnTipoInfraccion = $('#btnTipoInfraccion');
        const inputTipoInfraccionDescripcion = $('#inputTipoInfraccionDescripcion');
        const inputNivelInfraccion = $('#inputNivelInfraccion');
        const inputNumeroFalta = $('#inputNumeroFalta');
        const inputNivelInfraccionAcumulado = $('#inputNivelInfraccionAcumulado');
        const divContactoPersonal = $('#divContactoPersonal');
        const causas = $('#causas');
        const acciones = $('#acciones');
        const inputCompromiso = $('#inputCompromiso');
        const inputResponsableCC = $('#inputResponsableCC');

        const modalCargarActa = $('#modalCargarActa');
        const inputActa = $('#inputActa');
        const btnGuardarActa = $('#btnGuardarActa');

        // Condición.
        const divCondicion = $('#divCondicion');
        const divImagenAntes = $('#divImagenAntes');
        const inputImagenAntes = $('#inputImagenAntes');
        const divDescargarImagenAntes = $('#divDescargarImagenAntes');
        const botonDescargarImagenAntes = $('#botonDescargarImagenAntes');
        const botonVerImagenAntes = $('#botonVerImagenAntes');
        const divImagenDespues = $('#divImagenDespues');
        const inputImagenDespues = $('#inputImagenDespues');
        const divDescargarImagenDespues = $('#divDescargarImagenDespues');
        const botonDescargarImagenDespues = $('#botonDescargarImagenDespues');
        const botonVerImagenDespues = $('#botonVerImagenDespues');
        const inputFechaResolucion = $('#inputFechaResolucion');
        const comboNivelPrioridad = $('#comboNivelPrioridad');
        const divAccionCorrectiva = $('#divAccionCorrectiva');
        const inputAccionCorrectiva = $('#inputAccionCorrectiva');
        const comboClasificacionGeneral = $('#comboClasificacionGeneral');

        // Campos en común.
        const comboCCSuceso = $('#comboCCSuceso');
        const comboDepartamentoSuceso = $('#comboDepartamentoSuceso');
        const inputFechaSuceso = $('#inputFechaSuceso');
        const inputClaveSupervisor = $('#inputClaveSupervisor');
        const inputNombreSupervisor = $('#inputNombreSupervisor');
        const inputClaveEmpleadoInformo = $('#inputClaveEmpleadoInformo');
        const inputNombreEmpleadoInformo = $('#inputNombreEmpleadoInformo');
        const comboSubclasificacionDepartamento = $('#comboSubclasificacionDepartamento');
        const botonDescargarEvidencia = $('#botonDescargarEvidencia');
        const divDescargarImagenEvidencia = $('#divDescargarImagenEvidencia');
        const botonVerImagenEvidencia = $('#botonVerImagenEvidencia');
        const inputFechaInforme = $('#inputFechaInforme');
        const inputDescripion = $('#inputDescripion');
        const comboClasificacion = $('#comboClasificacion');
        const comboProcedimientoViolado = $('#comboProcedimientoViolado');
        const chkEsContratista = $('#chkEsContratista');
        const lblContratista = $('#lblContratista');
        const divFirmas = $('#divFirmas');
        const btnFirmaEmpleado = $('#btnFirmaEmpleado');
        const btnFirmaSupervisor = $('#btnFirmaSupervisor');
        const btnFirmaSST = $('#btnFirmaSST');

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);

        // Clasificaciones.
        let clasificacionesActo;
        let clasificacionesCondicion;

        //Firma
        let _cFirmaFull = document.getElementById('canvasFirmaFull');
        var signaturePadFull = new SignaturePad(_cFirmaFull, {
            backgroundColor: 'rgba(255, 255, 255, 0)',
            penColor: 'black'
        });
        const divFirmaFull = $('#divFirmaFull');
        const claveDelQueFirma = $('#claveDelQueFirma');
        const nombreDelQueFirma = $('#nombreDelQueFirma');
        const btnGuardarFirma = $('#btnGuardarFirma');
        const btnCancelarFirma = $('#btnCancelarFirma');

        // Estatus.
        const ESTATUS = {
            NUEVO: 0,
            COMPLETO: 1,
            PENDIENTE_IMAGEN_DESPUES: 2,
            EN_PROCESO: 2,
            VENCIDO: 3
        };

        // Tipo de riesgo. 
        const TIPO_RIESGO = {
            ACTO: "1",
            CONDICION: "2"
        };

        // Tipo de riesgo. 
        const TIPO_ACTO = {
            INSEGURO: "1",
            SEGURO: "2"
        };

        // Tipo de riesgo. 
        const TIPO_ARCHIVO = {
            EVIDENCIA: 1,
            IMAGEN_ANTES: 2,
            IMAGEN_DESPUES: 3
        };

        const TIPO_FIRMA = {
            EMPLEADO: 1,
            SUPERVISOR: 2,
            SST: 3
        };

        (function init() {
            // Lógica de inicialización.
            QuitarAlerta();
            llenarCombos();
            initDatepickers();
            agregarListeners();
            initTablaRiesgos();
            ocultarElementosDefault();
            fncValidarAccesoContratista();
            initCausas();
            initAccion();
            initSignaturePad();
            initBotonesFirma();
            inittablaArchivosAdjuntos();

            inputNombreSupervisor.getAutocompleteParams(selectAutocompleteEmpleado, null, '/Administrativo/ActoCondicionCH/GetInfoEmpleadoInternoContratista', getParametrosAutocompleteEmpleadoInterno);
            inputNombreEmpleadoInformo.getAutocompleteParams(selectAutocompleteEmpleado, null, '/Administrativo/ActoCondicionCH/GetInfoEmpleadoInternoContratista', getParametrosAutocompleteEmpleadoInterno);
            inputNombreEmpleado.getAutocompleteParams(selectAutocompleteEmpleado, null, '/Administrativo/ActoCondicionCH/GetInfoEmpleadoInternoContratista', getParametrosAutocompleteEmpleado);

            botonGuardar.data().tipoRiesgo = TIPO_RIESGO.ACTO;
            botonGuardar.data().sucesoID = null;

            chkEsContratista.change(function (e) {
                if (inputClaveEmpleado.val() != "") { inputClaveEmpleado.trigger("change"); }
                if (inputClaveSupervisor.val() != "") { inputClaveSupervisor.trigger("change"); }
                if (inputClaveEmpleadoInformo.val() != "") { inputClaveEmpleadoInformo.trigger("change"); }
            });

            inputClaveEmpleado.click(function (e) {
                $(this).select();
            });

            inputClaveEmpleado.change(function (e) {
                fncGetSiguienteInfraccion()
            })

            comboProcedimientoViolado.change(function () {
                fncGetSiguienteInfraccion()
            })

            comboDepartamento.change(function (e) {
                if ($(this).val() == "Todos") {
                    comboFiltroSubclasificacionDepartamento.empty();
                } else {
                    comboFiltroSubclasificacionDepartamento.fillCombo('/Administrativo/ActoCondicionCH/FillCboSubclasificacionesDepartamentos', { idDepartamento: $(this).val() }, false, 'Todos');
                    // comboFiltroSubclasificacionDepartamento.select2();
                }
            });

            comboDepartamentoSuceso.change(function (e) {
                comboSubclasificacionDepartamento.fillCombo('/Administrativo/ActoCondicionCH/FillCboSubclasificacionesDepartamentos', { idDepartamento: $(this).val() }, false, 'Todos');
                let opcionTodos = comboSubclasificacionDepartamento.find('option[value="Todos"]');
                opcionTodos.attr('value', '0');
                opcionTodos.text('N/A');
                // comboSubclasificacionDepartamento.select2();
            });

            comboCCSuceso.change(function () {
                if ($(this).val() > 0) {
                    comboDepartamentoSuceso.fillCombo("/Administrativo/ActoCondicionCH/FillCboDepartamentos", { cc: $(this).val() }, false, '--Seleccione--');
                }
            })

            comboCC.change(function () {
                if ($(this).val() > 0) {
                    comboDepartamento.fillCombo("/Administrativo/ActoCondicionCH/FillCboDepartamentos", { cc: $(this).val() }, false, '--Seleccione--');
                }
            })

            comboClasificacion.change(function () {
                if ($(this).val() > 0) {
                    comboProcedimientoViolado.fillCombo('/Administrativo/ActoCondicionCH/FillCboProcedimientos', { FK_Clasificacion: $(this).val() }, false, 'Seleccione');
                }
            })

            btnGuardarArchivoAdjunto.click(function () {
                fncGuardarArchivoAdjunto()
            })

            btnGenerarReporte.click(function () {
                fncGenerarReporte()
            })

            cboTipoReporte.fillCombo('/Administrativo/ActoCondicionCH/FillCboTipoReportes', null, false, 'Seleccione');
            $(".select2").select2()

            divAmonestacion.css("display", "none")
            divSuspension.css("display", "none")
            divCartaResponsabilidad.css("display", "none")
            divActaAdministrativa.css("display", "none")

            cboTipoReporte.change(function () {
                let tipoReporte = $(this).val()

                switch (tipoReporte) {
                    case "1": // AMONESTACIÓN
                        divAmonestacion.css("display", "block")
                        divSuspension.css("display", "none")
                        divCartaResponsabilidad.css("display", "none")
                        divActaAdministrativa.css("display", "none")
                        break
                    case "2": // SUSPENSIÓN
                        divAmonestacion.css("display", "none")
                        divSuspension.css("display", "block")
                        divCartaResponsabilidad.css("display", "none")
                        divActaAdministrativa.css("display", "none")
                        let contenidoSuspension = "Por este conducto se hace de su conocimiento que con efectos a partir de la fecha ___________ " +
                            "se le suspende en su trabajo por un término de __ días, sin goce de salario, debiendo de presentarse a cumplir " +
                            "con sus obligaciones laborales en la fecha _________________. " +
                            "Lo anterior con fundamento en lo dispuesto en el Contrato Individual de Trabajo, al haber incurrido en los hechos siguientes: _________________";
                        txtSuspension_Contenido.val(contenidoSuspension)
                        break
                    case "3": // CARTA DE RESPONSABILIDAD
                        divAmonestacion.css("display", "none")
                        divSuspension.css("display", "none")
                        divCartaResponsabilidad.css("display", "block")
                        divActaAdministrativa.css("display", "none")
                        fncGetPuestoEmpleado()
                        break;
                    case "4": // ACTA ADMINISTRATIVA
                        divAmonestacion.css("display", "none")
                        divSuspension.css("display", "none")
                        divCartaResponsabilidad.css("display", "none")
                        divActaAdministrativa.css("display", "block")
                        fncGetContenidoActaAdministrativa()
                        break;
                    default:
                        divAmonestacion.css("display", "none")
                        divSuspension.css("display", "none")
                        divCartaResponsabilidad.css("display", "none")
                        divActaAdministrativa.css("display", "none")
                        break
                }
            });
        })();

        function fncGetPermisos() {
            axios.post('GetPermisos').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (response.data.puedeCambiarAccion) {
                        comboAccion.attr("disabled", false);
                    } else {
                        comboAccion.attr("disabled", true);
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetContenidoActaAdministrativa() {
            let obj = new Object()
            obj.idActo = btnGuardarArchivoAdjunto.data().idActo
            axios.post('GetContenidoActaAdministrativa', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    txtActaAdministrtiva_Articulos.val(response.data.objArticulos)
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetSiguienteInfraccion() {
            if (inputClaveEmpleado.val() != "" && comboProcedimientoViolado.val() > 0) {
                let obj = new Object()
                obj.claveEmpleado = inputClaveEmpleado.val()
                obj.procedimientoID = comboProcedimientoViolado.val()
                axios.post('GetSiguienteInfraccion', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        txtClasificacionGeneral.val(response.data.infraccion)
                        comboAccion.val(response.data.idInfraccion)
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetPuestoEmpleado() {
            if (btnGuardarArchivoAdjunto.data().idActo > 0) {
                let obj = new Object()
                obj.idActo = btnGuardarArchivoAdjunto.data().idActo
                axios.post('GetPuestoEmpleado', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        txtCartaResponsabilidad_Contenido.val(response.data.parrafoUno)
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGenerarReporte() {
            if (cboTipoReporte.val() > 0) {
                let idTipoReporte = cboTipoReporte.val()
                switch (idTipoReporte) {
                    case "1":
                        //#region AMONESTACIÓN
                        if (btnGuardarArchivoAdjunto.data().idActo > 0 && txtCiudad.val() != "" && txtContenido.val() != "") {
                            let idActo = btnGuardarArchivoAdjunto.data().idActo
                            let fechaReporte = txtCiudad.val()
                            let parrafoUno = txtContenido.val()
                            var path = `/Reportes/Vista.aspx?idReporte=275&idTipoReporte=${idTipoReporte}&idActo=${idActo}&fechaReporte=${fechaReporte}&parrafoUno=${parrafoUno}`;
                            $("#report").attr("src", path);
                            document.getElementById('report').onload = function () {
                                $.unblockUI();
                                openCRModal();
                            };
                        }
                        //#endregion
                        break;
                    case "2":
                        //#region SUSPENSIÓN
                        if (btnGuardarArchivoAdjunto.data().idActo > 0 && txtSuspension_Ciudad.val() != "" && txtSuspension_Contenido.val() != "") {
                            let idActo = btnGuardarArchivoAdjunto.data().idActo
                            let fechaReporte = txtSuspension_Ciudad.val()
                            let parrafoUno = txtSuspension_Contenido.val()
                            var path = `/Reportes/Vista.aspx?idReporte=276&idTipoReporte=${idTipoReporte}&idActo=${idActo}&fechaReporte=${fechaReporte}&parrafoUno=${parrafoUno}`;
                            $("#report").attr("src", path);
                            document.getElementById('report').onload = function () {
                                $.unblockUI();
                                openCRModal();
                            };
                        }
                        //#region SUSPENSIÓN
                        break;
                    case "3":
                        //#region CARTA DE RESPONSABILIDAD
                        if (btnGuardarArchivoAdjunto.data().idActo > 0 && txtCartaResponsabilidad_Ciudad.val() != "" && txtCartaResponsabilidad_Contenido.val() != "") {
                            let idActo = btnGuardarArchivoAdjunto.data().idActo
                            let fechaReporte = txtCartaResponsabilidad_Ciudad.val()
                            let parrafoUno = txtCartaResponsabilidad_Contenido.val()
                            var path = `/Reportes/Vista.aspx?idReporte=277&idTipoReporte=${idTipoReporte}&idActo=${idActo}&fechaReporte=${fechaReporte}&parrafoUno=${parrafoUno}`;
                            $("#report").attr("src", path);
                            document.getElementById('report').onload = function () {
                                $.unblockUI();
                                openCRModal();
                            };
                        }
                        //#endregion
                        break;
                    case "4":
                        //#region ACTA ADMINISTRATIVA
                        if (btnGuardarArchivoAdjunto.data().idActo > 0 && txtActaAdministrtiva_Ciudad.val() != "" && txtActaAdministrtiva_Articulos.val() != "") {
                            let idActo = btnGuardarArchivoAdjunto.data().idActo
                            let ciudad = txtActaAdministrtiva_Ciudad.val()
                            let articulos = txtActaAdministrtiva_Articulos.val()
                            descargarActa(idActo, ciudad, articulos)
                        }
                        //#endregion
                        break;
                }
            }
        }

        function inittablaArchivosAdjuntos() {
            dtArchivoAdjunto = tablaArchivosAdjuntos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreArchivo', title: 'Archivo' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>&nbsp;`;
                            let btnDescargar = `<button class='btn btn-xs btn-primary visualizarArchivo' title='Visualizar archivo.'><i class="fas fa-file-download"></i></button>`;
                            return btnEliminar + btnDescargar;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tablaArchivosAdjuntos.on('click', '.eliminarRegistro', function () {
                        let rowData = dtArchivoAdjunto.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarArchivoAdjunto(rowData.id));
                    });

                    tablaArchivosAdjuntos.on('click', '.visualizarArchivo', function () {
                        let rowData = dtArchivoAdjunto.row($(this).closest('tr')).data();
                        fncVisualizarArchivoAdjunto(rowData.id)
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function fncVisualizarArchivoAdjunto(idArchivo) {
            if (idArchivo > 0) {
                let obj = new Object();
                obj.idArchivo = idArchivo
                axios.post("VisualizarArchivoAdjunto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        $('#myModal').data().ruta = null;
                        $('#myModal').modal('show');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncEliminarArchivoAdjunto(idArchivo) {
            if (idArchivo > 0) {
                let obj = {};
                obj.idArchivo = idArchivo;
                axios.post('EliminarArchivoAdjunto', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetArchivosAdjuntos()
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        const fncGuardarArchivoAdjunto = function () {
            var data = fncGetEvidenciaParaGuardar();
            let obj = new Object();
            obj.idActo = btnGuardarArchivoAdjunto.data().idActo

            axios.post('GuardarArchivoAdjunto', data, { params: idActo = obj }, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                let { success, datos, message } = response.data;
                if (success) {
                    Alert2Exito(message)
                    fncGetArchivosAdjuntos()
                } else {
                    Alert2Error(message)
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function fncGetEvidenciaParaGuardar() {
            let data = new FormData();
            data.append("id", $("#rowDataId").val());
            $.each(document.getElementById("txtArchivoAdjunto").files, function (i, file) {
                data.append("lstArchivos", file);
            });
            // console.log(data);
            return data;
        }

        function fncGetArchivosAdjuntos() {
            if (btnGuardarArchivoAdjunto.data().idActo > 0) {
                let obj = {};
                obj.idActo = btnGuardarArchivoAdjunto.data().idActo
                axios.post('GetArchivosAdjuntos', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtArchivoAdjunto.clear();
                        dtArchivoAdjunto.rows.add(response.data.lstArchivos);
                        dtArchivoAdjunto.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function getParametrosAutocompleteEmpleado() {
            let idEmpresa = comboCCSuceso.val() != '' ? +comboCCSuceso.find('option:selected').attr('empresa') : 0;
            let esContratista = false;

            if (botonGuardar.attr("data-esContratista") == "true") {
                esContratista = true;
            } else {
                if (chkEsContratista.prop('checked')) {
                    esContratista = true;
                } else {
                    esContratista = false;
                }
            }

            return { esContratista: false, idEmpresaContratista: 0 };
        }

        function getParametrosAutocompleteEmpleadoInterno() {
            let esContratista = false;

            if (botonGuardar.attr("data-esContratista") == "true") {
                esContratista = true;
            } else {
                if (chkEsContratista.prop('checked')) {
                    esContratista = true;
                } else {
                    esContratista = false;
                }
            }

            return { esContratista, idEmpresaContratista: 0 };
        }

        // Métodos.
        function QuitarAlerta() {
            const parametrosUrl = new URLSearchParams(window.location.search);

            const clean_uri = location.protocol + "//" + location.host + location.pathname;
            window.history.replaceState({}, document.title, clean_uri);
        }

        function ocultarElementosDefault() {
            divContratista.hide();
            divActo.show();
            divCondicion.hide();
            divImagenAntes.show();
            divImagenDespues.show();
            divDescargarImagenAntes.hide();
            divDescargarImagenDespues.hide();
            divDescargarImagenEvidencia.hide();
        }

        function initDatepickers() {
            inputFechaInicio.datepicker({ dateFormat, showAnim }).datepicker("setDate", fechaInicioAnio);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
            inputFechaIngreso.datepicker({ dateFormat, maxDate: fechaActual, showAnim });
            inputFechaSuceso.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
            inputFechaInforme.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
        }

        function llenarCombos() {
            comboCC.fillComboBox('/Administrativo/Reclutamientos/GetCCs', null, 'Seleccione');
            // comboCC.select2();
            comboAccion.fillCombo('/Administrativo/ActoCondicionCH/ObtenerAcciones', null, false, 'Seleccione');
            comboCCSuceso.fillComboBox('/Administrativo/Reclutamientos/GetCCs', null, 'Seleccione');
            // comboCCSuceso.select2({ dropdownParent: $(modalActoCondicion) });
            comboSupervisor.fillCombo('/Administrativo/ActoCondicionCH/ObtenerSupervisores', null, false, 'Todos');
            // comboSupervisor.select2();
            comboDepartamento.fillCombo('/Administrativo/ActoCondicionCH/ObtenerDepartamentos', null, false, 'Todos');
            // comboDepartamento.select2();
            comboDepartamentoSuceso.fillCombo('/Administrativo/ActoCondicionCH/ObtenerDepartamentos', null, false, 'Seleccione');
            // comboDepartamentoSuceso.select2({ dropdownParent: $(modalActoCondicion) });
            comboEstatus.fillCombo('/Administrativo/ActoCondicionCH/ObtenerEstatusActoCondicion', null, false, 'Todos');
            comboContratista.fillCombo('/Administrativo/IndicadoresSeguridad/GetSubcontratistas', null, false, 'Seleccione');
            // comboProcedimientoViolado.select2({ dropdownParent: $(modalActoCondicion) });
            comboNivelPrioridad.fillCombo('/Administrativo/ActoCondicionCH/ObtenerPrioridades', null, false, null);
            comboClasificacionGeneral.fillCombo('/Administrativo/ActoCondicionCH/ObtenerClasificacionesGenerales', null, false, 'Seleccione');
            // comboClasificacionGeneral.select2({ dropdownParent: $(modalActoCondicion) });
            comboFiltroClasificacion.fillCombo('/Administrativo/ActoCondicionCH/ObtenerClasificacionesGenerales', null, false, 'Todos');
            // comboFiltroClasificacion.select2();

            cargarClasificaciones();
        }


        function cargarClasificaciones() {
            $.get('/Administrativo/ActoCondicionCH/ObtenerClasificaciones')
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        clasificacionesActo = response.clasificacionesActo;
                        clasificacionesCondicion = response.clasificacionesCondicion;

                        comboClasificacion.append(`<option selected value='Seleccione'>Seleccione</option>`);
                        comboClasificacion.append(clasificacionesActo.map(item => `<option value=${item.Value}>${item.Text}</option>`).join(''));
                        // comboClasificacion.select2({ dropdownParent: $(modalActoCondicion) });
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        function limpiarCamposModal() {

            // Opciones
            divRadioOpciones.find('a').attr('disabled', false);
            divRadioOpciones.find('a').removeClass('active');
            divRadioOpciones.find('a').first().addClass('active');

            divRadioTipoActo.find('a').attr('disabled', false);
            divRadioTipoActo.find('a').removeClass('active');
            divRadioTipoActo.find('a').first().addClass('active');

            botonGuardar.data().tipoRiesgo = TIPO_RIESGO.ACTO;

            // Acto.
            inputClaveEmpleado.val('').attr('disabled', false);
            inputNombreEmpleado.val('').attr('disabled', false);
            inputFechaIngreso.val('').attr('disabled', true);
            comboAccion.val('Seleccione').change();
            inputPuestoEmpleado.val('').attr('disabled', true);
            comboContratista.attr('disabled', false);
            comboContratista.val('Seleccione').change();
            divRadioTipoActo.show();
            divContactoPersonal.hide();

            // Condición.
            inputImagenAntes.val('');
            inputImagenDespues.val('');
            inputFechaResolucion.val('');
            divAccionCorrectiva.hide();
            inputImagenDespues.prop('disabled', false);
            inputAccionCorrectiva.val('');

            // Campos en común.
            comboCCSuceso.attr('disabled', false);
            comboCCSuceso.val('').change();
            comboDepartamentoSuceso.attr('disabled', false);
            comboDepartamentoSuceso.val('Seleccione').change();
            inputFechaSuceso.attr('disabled', false).datepicker("setDate", fechaActual);
            inputClaveSupervisor.val('').attr('disabled', false);
            inputNombreSupervisor.val('').attr('disabled', false);
            inputClaveEmpleadoInformo.val('').attr('disabled', false);
            inputNombreEmpleadoInformo.val('').attr('disabled', false);
            comboSubclasificacionDepartamento.val('').attr('disabled', false);
            divDescargarImagenEvidencia.hide();
            inputFechaInforme.datepicker("setDate", fechaActual);
            inputDescripion.val('');
            inputDescripion.prop('disabled', false);
            comboClasificacion.val('Seleccione').change();
            comboClasificacion.prop('disabled', false);
            comboProcedimientoViolado.val('Seleccione').change();
            comboProcedimientoViolado.prop('disabled', false);
            botonGuardar.data().estatus = null;
            botonGuardar.data().sucesoID = null;
            botonGuardar.show();
            chkEsContratista.prop('disabled', false);
            comboNivelPrioridad.val('')
            comboNivelPrioridad.prop('disabled', true);
            comboClasificacionGeneral.val('Seleccione').change();
            comboClasificacionGeneral.prop('disabled', false);

            ocultarElementosDefault();

            limpiarCamposInfraccion();

            //Firma
            divFirmas.hide();
            initBotonesFirma();
            reiniciarBtnsFirma();
        }

        function alternarPersonaExterna() {
            const checked = !chkEsContratista.prop('checked');

            // inputClaveEmpleado.attr('disabled', checked);
            // inputFechaIngreso.attr('disabled', !checked);
            // inputPuestoEmpleado.attr('disabled', !checked);

            inputClaveEmpleado.val('');
            inputNombreEmpleado.val('');
            inputPuestoEmpleado.val('');
            inputFechaIngreso.val('');

            if (checked) {
                divFechaIngreso.hide(500);
                divContratista.show(500);
                // inputClaveEmpleado.val('');
            } else {
                divFechaIngreso.show(500);
                divContratista.hide(500);
                comboContratista.val('Seleccione').change();
            }
        }

        function getInfoEmpleado(clave, esContratista, idEmpresa) {
            if (typeof idEmpresa === 'string' || idEmpresa instanceof String) {
                idEmpresa = idEmpresa.replace('a_', '');
            }

            return $.post('InfEmpleado', { claveEmpleado: clave, esContratista: false, idEmpresaContratista: 0 }) //OMAR
        }

        function cargarInfoEmpleado() {
            if (inputTipoInfraccion.val()) {
                obtenerInformacionInfraccion();
            }

            const claveEmpleado = $(this).val();
            if (claveEmpleado == null || claveEmpleado == "") {
                clearInfoEmpleado();
                return;
            }

            let attrEsContratista = botonGuardar.attr("data-esContratista");
            let idEmpresa = (comboCCSuceso.val() != "" && comboCCSuceso.val() != null) ? comboCCSuceso.val() : 0;
            let esContratista = false;
            if (attrEsContratista == "true") {
                esContratista = true;
            } else {
                if (chkEsContratista.prop('checked')) {
                    esContratista = true;
                } else {
                    esContratista = false;
                }
            }
            if (chkEsContratista.attr("esOculto") == 1 && comboCCSuceso.val() == "") {
                Alert2Warning("Es necesario seleccionar una empresa");
            } else {
                let obj = new Object()
                obj.claveEmpleado = claveEmpleado
                axios.post("InfEmpleado", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        inputFechaIngreso.val(moment(response.data.empleado.antiguedadEmpleado).format("DD/MM/YYYY"));
                        inputPuestoEmpleado.val(response.data.empleado.puestoEmpleado);
                        inputNombreEmpleado.val(response.data.empleado.nombreEmpleado);
                        inputResponsableCC.val(response.data.empleado.responsableCC);
                        inputResponsableCC.data("FK_ClaveEmpleadoResponsableCC", response.data.empleado.FK_responsableCC);
                        comboCCSuceso.val(response.data.empleado.ccID);
                        comboCCSuceso.trigger("change");
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncValidarAccesoContratista() {
            axios.post("../IndicadoresSeguridad/ValidarAccesoContratista").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    botonGuardar.css("display", "none");
                    chkEsContratista.css("display", "none");
                    $("#divChkEsContratista").css("display", "none");
                    lblContratista.css("display", "none");
                    botonGuardar.attr("data-esContratista", true);
                    botonVerHistorial.css("display", "none");
                    botonGuardar.attr("data-esInterno", false);
                    chkEsContratista.attr("esOculto", "1");
                } else {
                    botonGuardar.attr("data-esContratista", false);
                    botonGuardar.attr("data-esInterno", true);
                    chkEsContratista.attr("esOculto", "0");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function clearInfoEmpleado() {
            inputClaveEmpleado.val('');
            inputNombreEmpleado.val('');
            inputFechaIngreso.val('');
            inputPuestoEmpleado.val('');
        }

        function cargarInfoSupervisor(clave) {
            const claveSupervisor = clave;

            if (claveSupervisor == null || claveSupervisor == "") {
                inputNombreSupervisor.val('');
                return;
            }

            let attrEsContratista = botonGuardar.attr("data-esContratista");
            let idEmpresa = 0;
            let esContratista = false;
            if (attrEsContratista == "true") {
                esContratista = true;
            } else {
                if (chkEsContratista.prop('checked')) {
                    esContratista = true;
                } else {
                    esContratista = false;
                }
            }

            let obj = new Object()
            obj.claveEmpleado = claveSupervisor
            obj.esConstratista = false
            obj.idEmpresa = 0
            axios.post("InfEmpleado", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    console.log(response.data.empleado);
                    inputNombreSupervisor.val(response.data.empleado.nombreEmpleado);
                    inputNombreSupervisor.focus();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function cargarInfoEmpleadoInformo() {

            const claveInformo = $(this).val();

            if (claveInformo == null || claveInformo == "") {
                inputNombreEmpleadoInformo.val('');
                return;
            }

            let attrEsContratista = botonGuardar.attr("data-esContratista");
            let idEmpresa = 0;
            let esContratista = false;
            if (attrEsContratista == "true") {
                esContratista = true;
            } else {
                if (chkEsContratista.prop('checked')) {
                    esContratista = true;
                } else {
                    esContratista = false;
                }
            }

            let obj = new Object()
            obj.claveEmpleado = claveInformo
            obj.esConstratista = false
            obj.idEmpresa = 0
            axios.post("InfEmpleado", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    inputNombreEmpleadoInformo.val(response.data.empleado.nombreEmpleado);
                    inputNombreEmpleadoInformo.focus();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function agregarListeners() {

            botonBuscar.click(cargarActosCondiciones);

            botonAgregar.click(() => {
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                seccionAdjuntarArchivos.css("display", "none")
                modalActoCondicion.modal('show');
                fncGetPermisos();
            });

            divRadioOpciones.find('a').click(alternarCamposSuceso);
            divRadioTipoActo.find('a').click(alternarTipoActo);

            inputClaveEmpleado.on('change', cargarInfoEmpleado);

            botonVerHistorial.click(verHistorialEmpleado);

            $('#divChkEsContratista').click(alternarPersonaExterna); // checkboxExterno.click(alternarPersonaExterna);

            inputClaveSupervisor.on('change', function (event) {
                cargarInfoSupervisor($(this).val());
            });

            inputClaveSupervisor.on('keydown', function (event) {
                if (event.which == 9) {
                    event.preventDefault();
                    $(this).trigger('change');
                }
            });

            inputClaveEmpleadoInformo.on('change', cargarInfoEmpleadoInformo);

            inputClaveEmpleadoInformo.on('keydown', function (event) {
                if (event.which == 9) {
                    event.preventDefault();
                    $(this).trigger('change');
                }
            });

            modalActoCondicion.on('hide.bs.modal', limpiarCamposModal);

            botonGuardar.click(guardar);

            botonDescargarEvidencia.click(() => descargarArchivo(TIPO_ARCHIVO.EVIDENCIA));
            botonDescargarImagenAntes.click(() => descargarArchivo(TIPO_ARCHIVO.IMAGEN_ANTES));
            botonDescargarImagenDespues.click(() => descargarArchivo(TIPO_ARCHIVO.IMAGEN_DESPUES));
            botonVerImagenAntes.click(() => mostrarArchivo(TIPO_ARCHIVO.IMAGEN_ANTES));
            botonVerImagenDespues.click(() => mostrarArchivo(TIPO_ARCHIVO.IMAGEN_DESPUES));
            botonVerImagenEvidencia.click(() => mostrarArchivo(TIPO_ARCHIVO.EVIDENCIA));

            chkEsContratista.bootstrapToggle();

            btnTipoInfraccion.click(obtenerInformacionInfraccion);
            inputFechaSuceso.change(obtenerInformacionInfraccion);

            inputImagenDespues.change(() => {
                if (!inputImagenDespues[0].files.length == 0) {
                    divAccionCorrectiva.show();
                } else {
                    divAccionCorrectiva.hide();
                }
            });

            btnFirmaEmpleado.on('click', function () {
                mostrarCampoParaFirmar($(this));
            });

            btnFirmaSupervisor.on('click', function () {
                mostrarCampoParaFirmar($(this));
            });

            btnFirmaSST.on('click', function () {
                mostrarCampoParaFirmar($(this));
            });

            btnCancelarFirma.on('click', function () {
                modalActoCondicion.show();
                divFirmaFull.hide();
            })

            btnGuardarFirma.on('click', function () {
                if (signaturePadFull.isEmpty()) {
                    alert('firmale!');
                } else {
                    let dataURL = signaturePadFull.toDataURL('image/png');

                    const parametros = {
                        idActoCondicion: btnGuardarFirma.data('id'),
                        tipoRiesgo: botonGuardar.data().tipoRiesgo,
                        tipoFirma: btnGuardarFirma.data('tipo'),
                        imagen: dataURL,
                        claveEmpleadoSST: claveDelQueFirma.val(),
                        nombreEmpleadoSST: nombreDelQueFirma.val()
                    }

                    let cantidadFirmas = 0;
                    if (btnFirmaEmpleado.prop('disabled')) { cantidadFirmas++; }
                    if (btnFirmaSupervisor.prop('disabled')) { cantidadFirmas++; }
                    if (btnFirmaSST.prop('disabled')) { cantidadFirmas++; }

                    if (parametros.tipoRiesgo == TIPO_RIESGO.ACTO && cantidadFirmas == 2) {
                        axios.post('/Administrativo/ActoCondicionCH/ObtenerReporteActoCondicion',
                            {
                                id: parametros.idActoCondicion,
                                tipo: parametros.tipoRiesgo
                            }).then(response => {
                                let { success } = response.data;
                                if (success) {
                                    var path = `/Reportes/Vista.aspx?idReporte=231&&idActo=${parametros.idActoCondicion}`;
                                    report.attr('src', path);
                                    document.getElementById('report').onload = function () {
                                        $.post('/Administrativo/ActoCondicionCH/GuardarFirma',
                                            {
                                                data: parametros
                                            }).then(response => {
                                                if (response.success) {
                                                    modalActoCondicion.show();
                                                    divFirmaFull.hide();
                                                    deshabilitarBtn(btnGuardarFirma.data('tipo'));
                                                    AlertaGeneral('Correcto', 'La firma se guardó correctamente');
                                                    if (response.firmado) {
                                                        cargarActosCondiciones();
                                                    }
                                                } else {
                                                    AlertaGeneral('Alerta', response.message);
                                                }
                                            }, error => {
                                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                                            });
                                    }
                                }
                            });
                    } else if (parametros.tipoRiesgo == TIPO_RIESGO.CONDICION && cantidadFirmas == 1) {
                        axios.post('/Administrativo/ActoCondicionCH/ObtenerReporteActoCondicion',
                            {
                                id: parametros.idActoCondicion,
                                tipo: parametros.tipoRiesgo
                            }).then(response => {
                                let { success } = response.data;
                                if (success) {
                                    var path = `/Reportes/Vista.aspx?idReporte=231`;
                                    report.attr('src', path);
                                    document.getElementById('report').onload = function () {
                                        $.post('/Administrativo/ActoCondicionCH/GuardarFirma',
                                            {
                                                data: parametros
                                            }).then(response => {
                                                if (response.success) {
                                                    modalActoCondicion.show();
                                                    divFirmaFull.hide();
                                                    deshabilitarBtn(btnGuardarFirma.data('tipo'));
                                                    AlertaGeneral('Correcto', 'La firma se guardó correctamente');
                                                    if (response.firmado) {
                                                        cargarActosCondiciones();
                                                    }
                                                } else {
                                                    AlertaGeneral('Alerta', response.message);
                                                }
                                            }, error => {
                                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                                            });
                                    }
                                }
                            });
                    } else {
                        $.post('/Administrativo/ActoCondicionCH/GuardarFirma',
                            {
                                data: parametros
                            }).then(response => {
                                if (response.success) {
                                    modalActoCondicion.show();
                                    divFirmaFull.hide();
                                    deshabilitarBtn(btnGuardarFirma.data('tipo'));
                                    AlertaGeneral('Correcto', 'La firma se guardó correctamente');
                                    if (response.firmado) {
                                        cargarActosCondiciones();
                                    }
                                } else {
                                    AlertaGeneral('Alerta', response.message);
                                }
                            }, error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            });
                    }
                }
            });

            claveDelQueFirma.on('change', buscarEmpleadoSST);

            // Cargar zip
            botonCargaZip.on("click", function () {
                mdlCargarZip.modal("show");
            });

            btnGuardarZip.on('click', function () {
                if (validarArchivoCargarZIP()) {
                    cargarZip();
                }
            });

            btnGuardarActa.on('click', function () {
                cargarActa($(this).data('id'));
            })

            btnDescargarFormato.on('click', function () {
                const path = '/Administrativo/ActoCondicionCH/DescargarFormato'
                location.href = path;
            });

            botonDescargarReporteExcel.on('click', function () {
                descargarReporteExcel();
            });
        }

        function obtenerInformacionInfraccion() {
            const numeroInfraccion = inputTipoInfraccion.val();
            const claveEmpleado = inputClaveEmpleado.val();
            const fechaSuceso = moment(inputFechaSuceso.val(), 'DD/MM/YYYY').toISOString(true);
            const esConstratista = chkEsContratista.prop('checked');

            limpiarCamposInfraccion();

            if (numeroInfraccion && moment(fechaSuceso).isValid() && claveEmpleado) {
                if (!esConstratista) {
                    axios.get('/Administrativo/ActoCondicionCH/ObtenerInformacionInfraccion', { params: { numeroInfraccion, claveEmpleado, fechaSuceso } }).then(response => {
                        let { success, items, message } = response.data;

                        if (success) {
                            inputTipoInfraccion.val(numeroInfraccion);
                            inputTipoInfraccionDescripcion.val(items.descripcion);
                            inputNivelInfraccion.val(items.nivelInfraccion);
                            inputNumeroFalta.val(items.numeroFalta);
                            inputNivelInfraccionAcumulado.val(items.nivelInfraccionAcumulado);

                            switch (items.nivelInfraccionAcumulado) {
                                case 1:
                                    // divContactoPersonal.show(); // OMAR
                                    break;
                                case 4:
                                    divContactoPersonal.hide();
                                    inputCompromiso.val('');
                                    break;
                                default:
                                    divContactoPersonal.hide();
                                    break;
                            }

                            // comboAccion.val(items.nivelInfraccionAcumulado);
                            // comboAccion.trigger('change');
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
                } else {
                    axios.post('/Administrativo/ActoCondicionCH/ObtenerInformacionInfraccionContratista', { numeroInfraccion, claveEmpleado, fechaSuceso })
                        .then(response => {
                            let { success, data, message } = response.data;

                            if (success) {
                                inputTipoInfraccion.val(numeroInfraccion);
                                inputTipoInfraccionDescripcion.val(data.descripcion);
                                inputNivelInfraccion.val(data.nivelInfraccion);
                                inputNumeroFalta.val(data.numeroFalta);
                                inputNivelInfraccionAcumulado.val(data.nivelInfraccionAcumulado);

                                switch (data.nivelInfraccionAcumulado) {
                                    case 1:
                                        // divContactoPersonal.show(); // OMAR
                                        break;
                                    case 4:
                                        divContactoPersonal.hide();
                                        inputCompromiso.val('');
                                        break;
                                    default:
                                        divContactoPersonal.hide();
                                        break;
                                }

                                comboAccion.val(data.nivelInfraccionAcumulado);
                                comboAccion.trigger('change');
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));
                }
            } else {
                AlertaGeneral('Alerta', 'Primero debe ingresar una clave de empleado valida y fecha de acto/condición y el # de tipo de infracción');
            }
        }

        function initCausas() {
            $.get('/Administrativo/ActoCondicionCH/ObtenerAccionReaccion',
                {
                    tipo: 1
                }).then(response => {
                    if (response.success) {
                        response.items.forEach(element => {
                            let causa = '<div class="col-md-4 col-sm-4 col-xs-12"><div class="checkbox"><label class="checkboxMovil">' +
                                '<input type="checkbox" id="causa_' + element.id + '" /> ' + element.descripcion +
                                '</label></div></div>';
                            causas.append(causa);
                        });
                    } else {
                        AlertaGeneral('Alerta', response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        function initAccion() {
            $.get('/Administrativo/ActoCondicionCH/ObtenerAccionReaccion',
                {
                    tipo: 2
                }).then(response => {
                    if (response.success) {
                        response.items.forEach(element => {
                            let accion = '<div class="col-md-4 col-sm-4 col-xs-12"><div class="checkbox"><label class="checkboxMovil"">' +
                                '<input type="checkbox" id="accion_' + element.id + '" /> ' + element.descripcion +
                                '</label></div></div>';
                            acciones.append(accion);
                        });
                    } else {
                        AlertaGeneral('Alerta', response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        function limpiarCamposInfraccion() {
            inputTipoInfraccion.val('');
            inputNivelInfraccion.val('');
            inputTipoInfraccionDescripcion.val('');
            inputNumeroFalta.val('');
            inputNivelInfraccionAcumulado.val('');
            inputCompromiso.val('');

            inputTipoInfraccion.prop('disabled', false);
            btnTipoInfraccion.prop('disabled', false);
            inputCompromiso.prop('disabled', false);

            divContactoPersonal.find('input').each(function (element, index) {
                $(this).prop('checked', false);
                $(this).prop('disabled', false);
            });
        }

        function verHistorialEmpleado() {

            const getUrl = window.location;
            const baseUrl = getUrl.protocol + "//" + getUrl.host;

            let urlHistorial = "";
            const claveEmpleado = inputClaveEmpleado.val();

            if (claveEmpleado != "") {
                urlHistorial = baseUrl + `/Administrativo/ActoCondicionCH/Historial?claveEmpleado=${claveEmpleado}`;
            } else {
                urlHistorial = baseUrl + `/Administrativo/ActoCondicionCH/Historial`;
            }

            window.open(urlHistorial, '_blank');
        }

        function descargarArchivo(tipoArchivo) {
            const tipoRiesgo = botonGuardar.data().tipoRiesgo;
            const sucesoID = botonGuardar.data().sucesoID;

            if (tipoRiesgo == null || sucesoID == null || tipoArchivo == null) {
                return;
            }

            location.href = `/Administrativo/ActoCondicionCH/DescargarArchivo?sucesoID=${sucesoID}&tipoRiesgo=${tipoRiesgo}&tipoArchivo=${tipoArchivo}`;
        }

        function mostrarArchivo(tipoArchivo) {
            const tipoRiesgo = botonGuardar.data().tipoRiesgo;
            const sucesoID = botonGuardar.data().sucesoID;

            if (tipoRiesgo == null || sucesoID == null || tipoArchivo == null) {
                return;
            }

            $.post('/Administrativo/ActoCondicionCH/CargarDatosArchivo', { sucesoID, tipoRiesgo, tipoArchivo })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        $('#myModal').data().ruta = null;
                        $('#myModal').modal('show');
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function camposInvalidos() {
            let camposInvalidos = 0;
            const estatus = botonGuardar.data().estatus;
            const tipoRiesgo = botonGuardar.data().tipoRiesgo;
            const tipoActo = divRadioTipoActo.find(`a.active`).attr('option');

            // Campos comunes
            $('.validar').each(function () {
                if ($(this).val().trim() == "") {
                    camposInvalidos++;
                }
            });

            $('.validarSelect').each(function () {
                if ($(this).val().trim() == "Seleccione") {
                    camposInvalidos++;
                }
            });

            // Acto
            if (tipoRiesgo == TIPO_RIESGO.ACTO) {

                if (tipoActo == TIPO_ACTO.INSEGURO) {
                    if (!inputCompromiso.val()) {
                        camposInvalidos++;
                    }
                }

                $('.validarActo').each(function () {
                    if ($(this).val().trim() == "" && comboAccion.val() != 4 && tipoActo != TIPO_ACTO.SEGURO) {
                        camposInvalidos++;
                    }
                });

                $('.validarActoSelect').each(function () {
                    if ($(this).val().trim() == "Seleccione") {
                        camposInvalidos++;
                    }
                });
            }
            // Condicion
            else {

                if (estatus == ESTATUS.NUEVO) {

                    if (inputImagenAntes[0].files.length == 0) {
                        camposInvalidos++;
                    }

                    if (!inputImagenDespues[0].files.length == 0) {
                        if (!inputAccionCorrectiva.val()) {
                            camposInvalidos++;
                        }
                    }

                } else if (estatus == ESTATUS.PENDIENTE_IMAGEN_DESPUES) {

                    if (inputImagenDespues[0].files.length == 0) {
                        camposInvalidos++;
                    }

                    if (!inputAccionCorrectiva.val()) {
                        camposInvalidos++;
                    }
                }

                $('.validarCondicionSelect').each(function () {
                    if (!$(this).val()) {
                        camposInvalidos++;
                    }
                });
            }

            return camposInvalidos > 0;
        }

        function guardar() { //TODO
            if (camposInvalidos()) {
                AlertaGeneral(`Aviso`, `Favor de llenar todos los campos.`);
                return;
            }

            const tipoRiesgo = botonGuardar.data().tipoRiesgo;

            switch (tipoRiesgo) {
                case TIPO_RIESGO.ACTO:
                    guardarActo();
                    break;

                case TIPO_RIESGO.CONDICION:
                    guardarCondicion();
                    break;

                default:
                    AlertaGeneral(`Aviso`, `Valor indefinido.`);
                    return;
            }
        }

        function guardarActo() {
            const acto = obtenerCamposActo();

            if (acto != "") {
                $.ajax({
                    url: '/Administrativo/ActoCondicionCH/GuardarActo',
                    data: acto,
                    async: false,
                    cache: false,
                    contentType: false,
                    processData: false,
                    method: 'POST'
                })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            if (response.message) {
                                Alert2Exito('Alerta', `Acto guardado correctamente. ${response.message}`);
                            } else {
                                Alert2Exito(`Éxito`, `Acto guardado correctamente.`);
                            }
                            botonBuscar.click();
                            comboSupervisor.fillCombo('/Administrativo/ActoCondicionCH/ObtenerSupervisores', null, false, 'Todos');
                            $('.select2').select2();
                            modalActoCondicion.modal('hide');
                        } else {
                            // Operación no completada.
                            Alert2Error(`No se guardó la información`, response.message);
                        }
                    }, error => {
                        // Error al lanzar la petición.
                        Alert2Error(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }

        }

        function obtenerCamposActo() {

            const claveContratista = comboContratista.val() == 'Seleccione' ? null : comboContratista.val();
            const tipoActo = divRadioTipoActo.find(`a.active`).attr('option');

            const data = new FormData();

            let idEmpresa = $(comboCCSuceso).getEmpresa();
            let strAgrupacion = $(comboCCSuceso).getAgrupador();
            let idAgrupacion;
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }

            data.append("id", (botonGuardar.data().sucesoID || 0));
            data.append("claveEmpleado", inputClaveEmpleado.val());
            data.append("nombre", inputNombreEmpleado.val());
            data.append("puesto", inputPuestoEmpleado.val());
            data.append("fechaIngreso", inputFechaIngreso.val());
            data.append("accionID", comboAccion.val());
            data.append("tipoActo", tipoActo);
            data.append("esExterno", false); // data.append("esExterno", checkboxExterno[0].checked);
            data.append("claveContratista", claveContratista);
            data.append("claveInformo", inputClaveEmpleadoInformo.val());
            data.append("nombreInformo", inputNombreEmpleadoInformo.val());
            data.append("cc", comboCCSuceso.val());
            data.append("idEmpresa", idEmpresa);
            data.append("idAgrupacion", idAgrupacion);
            data.append("descripcion", inputDescripion.val());
            data.append("clasificacionID", comboClasificacion.val());
            data.append("procedimientoID", comboProcedimientoViolado.val());
            data.append("fechaSuceso", inputFechaSuceso.val());
            data.append("claveSupervisor", inputClaveSupervisor.val());
            data.append("nombreSupervisor", inputNombreSupervisor.val());
            data.append("departamentoID", comboDepartamentoSuceso.val());
            data.append("subclasificacionDepID", comboSubclasificacionDepartamento.val());
            data.append("estatus", botonGuardar.data().estatus);
            data.append("numeroInfraccion", inputTipoInfraccion.val());
            data.append("nivelInfraccion", inputNivelInfraccion.val());
            data.append("nivelInfraccionAcumulado", inputNivelInfraccionAcumulado.val());
            data.append("numeroFalta", inputNumeroFalta.val());
            data.append("compromiso", inputCompromiso.val());
            data.append("clasificacionGeneralID", comboClasificacionGeneral.val());
            data.append("hallazgo", inputTipoInfraccionDescripcion.val());
            data.append("FK_ClaveEmpleadoResponsableCC", inputResponsableCC.data().FK_ClaveEmpleadoResponsableCC);

            let causasList = new Array();
            let accionesList = new Array();

            causas.find('input').each(function () {
                let causa = {
                    id: +$(this).prop('id').split('_')[1],
                    check: $(this).prop('checked')
                }

                causasList.push(causa);
            });

            acciones.find('input').each(function () {
                let accion = {
                    id: +$(this).prop('id').split('_')[1],
                    check: $(this).prop('checked')
                }

                accionesList.push(accion);
            });

            data.append("causas", JSON.stringify(causasList));
            data.append("acciones", JSON.stringify(accionesList));

            //#region CAMPOS OBLIGATORIOS
            fncBorderDefault();
            let strMensajeError = "";
            let esActualizar = botonGuardar.data().sucesoID;
            if (esActualizar > 0) {
                if (comboSubclasificacionDepartamento.val() == "") {
                    $("#select2-comboSubclasificacionDepartamento-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios.";
                }
            }
            //#endregion

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                return data;
            }
        }

        function fncBorderDefault() {
            $("#select2-comboSubclasificacionDepartamento-container").css('border', '1px solid #CCC');
        }

        function guardarCondicion() {
            const condicion = obtenerCamposCondicion();

            $.ajax({
                url: '/Administrativo/ActoCondicionCH/GuardarCondicion',
                data: condicion,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Condición guardada correctamente.`);
                        botonBuscar.click();
                        comboSupervisor.fillCombo('/Administrativo/ActoCondicionCH/ObtenerSupervisores', null, false, 'Todos');
                        $('.select2').select2();
                        modalActoCondicion.modal('hide');
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`No se guardó la información`, response.message);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function obtenerCamposCondicion() {

            const data = new FormData();

            const archivoAntes = inputImagenAntes[0].files.length > 0 ? inputImagenAntes[0].files[0] : null;
            const archivoDespues = inputImagenDespues[0].files.length > 0 ? inputImagenDespues[0].files[0] : null;

            let idEmpresa = $(comboCCSuceso).getEmpresa();
            let strAgrupacion = $(comboCCSuceso).getAgrupador();
            let idAgrupacion;
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }

            data.append("id", (botonGuardar.data().sucesoID || 0));
            data.append("imagenAntes", archivoAntes);
            data.append("imagenDespues", archivoDespues);
            data.append("claveInformo", inputClaveEmpleadoInformo.val());
            data.append("nombreInformo", inputNombreEmpleadoInformo.val());
            data.append("cc", "");
            data.append("idEmpresa", idEmpresa);
            data.append("idAgrupacion", idAgrupacion);
            data.append("descripcion", inputDescripion.val());
            data.append("clasificacionID", comboClasificacion.val());
            data.append("procedimientoID", comboProcedimientoViolado.val());
            data.append("fechaSuceso", inputFechaSuceso.val());
            data.append("claveSupervisor", inputClaveSupervisor.val());
            data.append("nombreSupervisor", inputNombreSupervisor.val());
            data.append("departamentoID", comboDepartamentoSuceso.val());
            data.append("subclasificacionDepID", comboSubclasificacionDepartamento.val());
            data.append("estatus", botonGuardar.data().estatus);
            data.append("nivelPrioridad", comboNivelPrioridad.val());
            data.append("accionCorrectiva", inputAccionCorrectiva.val());
            data.append("clasificacionGeneralID", comboClasificacionGeneral.val());

            //#region CAMPOS OBLIGATORIOS
            fncBorderDefault();
            let strMensajeError = "";
            let esActualizar = botonGuardar.data().sucesoID;
            if (esActualizar > 0) {
                if (comboSubclasificacionDepartamento.val() == "") {
                    $("#select2-comboSubclasificacionDepartamento-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios.";
                }
            }
            //#endregion

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                return data;
            }
        }

        function alternarCamposSuceso(e) {
            const option = $(e.currentTarget).addClass('active').attr('option');
            divRadioOpciones.find(`a[option!="${option}"]`).removeClass('active');
            botonGuardar.data().tipoRiesgo = option;
            alternarDivs(option);
        }

        function alternarTipoActo(e) {
            const tipoActoAnterior = divRadioTipoActo.find(`a.active`).attr('option');

            const option = $(e.currentTarget).addClass('active').attr('option');
            divRadioTipoActo.find(`a[option!="${option}"]`).removeClass('active');

            const tipoActo = divRadioTipoActo.find(`a.active`).attr('option');
            if (tipoActo != tipoActoAnterior) {
                limpiarCamposInfraccion();

                inputTipoInfraccion.prop('disabled', tipoActo != 1);
                btnTipoInfraccion.prop('disabled', tipoActo != 1);
                divContactoPersonal.hide();
            }

            if (tipoActo == TIPO_ACTO.SEGURO) {
                // comboAccion.val('1');
            } else {
                // comboAccion.val('Seleccione');
            }
        }

        function alternarDivs(option) {
            comboClasificacion.empty();
            comboClasificacion.append(`<option selected value='Seleccione'>Seleccione</option>`);

            if (option === TIPO_RIESGO.ACTO) {
                divActo.show(500);
                divRadioTipoActo.show(500);
                comboClasificacion.append(clasificacionesActo.map(item => `<option value=${item.Value}>${item.Text}</option>`).join(''));
                comboNivelPrioridad.prop('disabled', true);
                comboNivelPrioridad.val('');
                divCondicion.hide(500);
            } else if (option === TIPO_RIESGO.CONDICION) {
                divCondicion.show(500);
                divRadioTipoActo.hide(500);
                comboClasificacion.append(clasificacionesCondicion.map(item => `<option value=${item.Value}>${item.Text}</option>`).join(''));
                comboNivelPrioridad.prop('disabled', false);
                divActo.hide(500);
            }

            botonGuardar.data().tipoRiesgo = option;
        }

        function cargarActosCondiciones() {
            const filtro = obtenerFiltroBusqueda();
            if (filtro != "") {
                $.post('/Administrativo/ActoCondicionCH/CargarActosCondiciones', { filtro })
                    .then(response => {
                        dtTablaRiesgos.clear();
                        if (response.success) {
                            // Operación exitosa.
                            dtTablaRiesgos.rows.add(response.items);
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                        dtTablaRiesgos.draw();
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        }

        function obtenerFiltroBusqueda() {

            const claveSupervisor = comboSupervisor.val();
            const departamentoID = comboDepartamento.val();
            const subclasificacionDepID = comboFiltroSubclasificacionDepartamento.val();
            const estatus = comboEstatus.val();
            const clasificacion = comboFiltroClasificacion.val();

            let idEmpresa = $(comboCC).getEmpresa();
            let strAgrupacion = $(comboCC).getAgrupador();
            let idAgrupacion;
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }

            $("#select2-comboCC-container").css("border", "1px solid #CCC");
            let strMensajeError = "";
            if (idAgrupacion == "") { comboCC.css("border", "2px solid red"); strMensajeError = "Es necesario seleccionar un Centro de Costo."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                return {
                    cc: strAgrupacion,
                    idEmpresa: idEmpresa,
                    idAgrupacion: idAgrupacion,
                    fechaInicial: inputFechaInicio.val(),
                    fechaFinal: inputFechaFin.val(),
                    claveSupervisor: claveSupervisor == "Todos" ? 0 : claveSupervisor,
                    departamentoID: departamentoID == "Todos" ? 0 : departamentoID,
                    subclasificacionDepID: subclasificacionDepID == "Todos" ? 0 : subclasificacionDepID,
                    estatus: estatus == "Todos" ? -1 : estatus,
                    clasificacionID: clasificacion == "Todos" ? -1 : clasificacion
                };
            }
        }

        function initTablaRiesgos() {
            dtTablaRiesgos = tablaRiesgos.DataTable({
                paging: false,
                language: dtDicEsp,
                order: [[4, "desc"]],
                searching: false,
                scrollY: '45vh',
                scrollX: true,
                scrollCollapse: true,
                columns: [
                    { data: 'folio', title: 'Folio' },
                    { data: 'claveEmpleado', title: 'Clave empleado' },
                    { data: 'proyecto', title: 'Proyecto' },
                    { data: 'fechaSuceso', title: 'Fecha' },
                    {
                        title: 'Acciones',
                        render: (data, type, row, meta) => {
                            let btns = '';
                            const btnEditar = `<button title="Editar" class="btn-editar btn btn-warning"><i class="fas fa-pencil-alt"></i></button>`;
                            const btnEliminar = `<button title="Eliminar" class="btn-eliminar btn btn-danger"><i class="fas fa-trash"></i></button>`;
                            const btnFirmas = `<button title="Firmar" class="btn-firmar btn btn-info"><i class="fas fa-file-signature"></i></button>`;
                            const btnReporte = `<button title="Reporte" class="btn-reporte btn btn-primary"><i class="fas fa-file-pdf"></i></button>`;
                            const btnDescargarActa = `<button title="Descargar acta" class="btn-descarga-acta btn btn-default"><i class="fas fa-file-download"></i></button>`;
                            const btnCargarActa = `<button title="Cargar acta" class="btn-modal-acta btn btn-success"><i class="fas fa-file-upload"></i></button>`;
                            const btnGenerarReporte = `<button title="Generar reporte" class="btn btn-primary mdlGenerarReporte"><i class="fas fa-file-alt"></i></button>`;

                            if (!row.firmado) {
                                btns += btnFirmas;
                            }

                            if (row.puedeEliminar) {
                                btns += ` ${btnEditar} ${btnEliminar}`;
                            }

                            if (row.firmado) {
                                btns += `${btnReporte}`;
                            }

                            return `${btns} ${btnDescargarActa} ${btnGenerarReporte}`;
                        }
                    },
                ],
                columnDefs: [
                    { width: '5%', targets: [0] }
                ],
                drawCallback: function (settings, json) {
                    tablaRiesgos.find('.btn-firmar').off().click(e => {
                        const rowData = dtTablaRiesgos.row($(e.currentTarget).closest('tr')).data();
                        seccionAdjuntarArchivos.css("display", "block")
                        btnGuardarArchivoAdjunto.data().idActo = rowData.id
                        fncGetArchivosAdjuntos()
                        editar(true, $(e.currentTarget));
                    });

                    tablaRiesgos.find('.btn-editar').off().click(e => {
                        editar(false, $(e.currentTarget));
                    });

                    tablaRiesgos.find('.btn-eliminar').off().click(e => {
                        const rowData = dtTablaRiesgos.row($(e.currentTarget).closest('tr')).data();
                        const { tipoRiesgo, id } = rowData;
                        AlertaAceptarRechazarNormal(
                            'Confirmar eliminación',
                            `¿Está seguro de eliminar el folio ${rowData.folio} de ${rowData.proyecto}?`,
                            () => eliminar(tipoRiesgo, id))
                    });

                    tablaRiesgos.find('.btn-reporte').off().click(e => {
                        const rowData = dtTablaRiesgos.row($(e.currentTarget).closest('tr')).data();

                        obtenerReporte(rowData.id, rowData.tipoRiesgo);
                    });

                    tablaRiesgos.find('.btn-descarga-acta').off().click(e => {
                        const rowData = dtTablaRiesgos.row($(e.currentTarget).closest('tr')).data();
                        mdlReportes.modal("show")
                        btnGuardarArchivoAdjunto.data().idActo = rowData.id
                        cboTipoReporte.val(4)
                        cboTipoReporte.trigger("change")
                        // Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => descargarActa(rowData.id))
                    });

                    tablaRiesgos.find('.btn-modal-acta').off().click(e => {
                        const rowData = dtTablaRiesgos.row($(e.currentTarget).closest('tr')).data();

                        btnGuardarActa.removeData();
                        btnGuardarActa.data('id', rowData.id);

                        modalCargarActa.modal('show');
                    });

                    tablaRiesgos.find(".mdlGenerarReporte").off().click(e => {
                        const rowData = dtTablaRiesgos.row($(e.currentTarget).closest('tr')).data();
                        btnGuardarArchivoAdjunto.data().idActo = rowData.id
                        mdlReportes.modal("show");
                    });
                },
                createdRow: function (row, data, dataIndex) {
                    if (data.estatus == ESTATUS.VENCIDO)
                        $(row).addClass('actoCondicionVencido');
                }
            });
        }


        function obtenerReporte(id, tipoRiesgo) {
            axios.post('/Administrativo/ActoCondicionCH/ObtenerReporteActoCondicion',
                {
                    id,
                    tipo: tipoRiesgo
                }).then(response => {
                    let { success, items } = response.data;
                    if (success) {
                        $.blockUI({ message: 'Generando imprimible...' });
                        var path = `/Reportes/Vista.aspx?idReporte=227`;
                        report.attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            openCRModal();
                        };
                    }
                });
        }

        function eliminar(tipoRiesgo, id) {

            $.post('/Administrativo/ActoCondicionCH/EliminarActoCondicion', { tipoRiesgo, id })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Elemento eliminado correctamente.`);
                        botonBuscar.click();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cargarCamposComunes(item) {
            divRadioOpciones.find('a').attr('disabled', true);
            divRadioOpciones.find('a').removeClass('active');
            divRadioOpciones.find(`a[option=${item.tipoRiesgo}]`).first().addClass('active');

            comboCCSuceso.attr('disabled', true).val(item.cc).change();
            comboDepartamentoSuceso.attr('disabled', true).val(item.departamentoID).change();
            comboDepartamentoSuceso.trigger("change");
            comboSubclasificacionDepartamento.attr('disabled', true).val(item.subclasificacionDepID).change();
            inputFechaSuceso.attr('disabled', true).val(item.fechaSuceso);
            inputClaveSupervisor.attr('disabled', true).val(item.claveSupervisor);
            inputNombreSupervisor.val(item.nombreSupervisor);
            inputNombreSupervisor.attr('disabled', true);
            inputFechaInforme.val(item.fechaCreacion);
            inputClaveEmpleadoInformo.attr('disabled', true).val(item.claveInformo);
            inputNombreEmpleadoInformo.attr('disabled', true);
            inputNombreEmpleadoInformo.val(item.nombreInformo);
            inputNombreEmpleado.attr('disabled', true);
            comboClasificacionGeneral.val(item.clasificacionGeneralID).change();

            if (item.tieneEvidencia) {
                divDescargarImagenEvidencia.show();
            }

            inputDescripion.val(item.descripcion);
            comboClasificacion.val(item.clasificacionID).change();
            comboProcedimientoViolado.val(item.procedimientoID).change();

            botonGuardar.data().estatus = item.estatus;
            botonGuardar.data().sucesoID = item.id;

            chkEsContratista.prop('disabled', true);
        }

        function camposFirma(data) {
            botonGuardar.hide();

            if (data.tipoRiesgo == TIPO_RIESGO.CONDICION) {
                btnFirmaEmpleado.hide();
            } else {
                btnFirmaEmpleado.show();

                if (data.firmadoPorEmpleado) {
                    deshabilitarBtn(TIPO_FIRMA.EMPLEADO);
                }
            }

            if (data.firmadoPorSupervisor) {
                deshabilitarBtn(TIPO_FIRMA.SUPERVISOR);
            }

            if (data.firmadoPorSST) {
                deshabilitarBtn(TIPO_FIRMA.SST);
            }

            inputDescripion.prop('disabled', true);
            comboClasificacion.prop('disabled', true);
            comboProcedimientoViolado.prop('disabled', true);
            comboClasificacionGeneral.prop('disabled', true);
            divFirmas.show();
            divFirmas.hide()
        }

        function cargarCamposActo(item) {

            inputClaveEmpleado.attr('disabled', true).val(item.claveEmpleado);
            inputNombreEmpleado.val(item.nombre);
            inputNombreEmpleado.attr('disabled', true);
            inputFechaIngreso.val(item.fechaIngreso);
            comboAccion.val(item.accionID).change();
            inputPuestoEmpleado.val(item.puesto);

            inputTipoInfraccion.val(item.numeroInfraccion == 0 ? '' : item.numeroInfraccion);
            inputTipoInfraccion.prop('disabled', true);
            btnTipoInfraccion.prop('disabled', true);
            inputTipoInfraccionDescripcion.val(item.descripcionInfraccion);
            inputNivelInfraccion.val(item.nivelInfraccion == 0 ? '' : item.nivelInfraccion);
            inputNivelInfraccionAcumulado.val(item.nivelInfraccionAcumulado == 0 ? '' : item.nivelInfraccionAcumulado);
            inputNumeroFalta.val(item.numeroFalta == 0 ? '' : item.numeroFalta);
            inputCompromiso.val(item.compromiso ?? '');
            inputCompromiso.prop('disabled', true);

            // divContactoPersonal.show(); // OMAR
            divContactoPersonal.find('input').each(function (element, index) {
                $(this).prop('disabled', true);
            });

            if (item.causas != null && item.causas.length > 0) {
                item.causas.forEach(element => {
                    $('#causa_' + element.id).prop('checked', element.check);
                });

                item.acciones.forEach(element => {
                    $('#accion_' + element.id).prop('checked', element.check);
                });
            }

            divRadioTipoActo.find('a').attr('disabled', true).removeClass('active');
            divRadioTipoActo.find(`a[option=${item.tipoActo.toString()}]`).first().addClass('active');

            // if (item.esExterno) {
            //     comboContratista.attr('disabled', true).val(item.claveContratista).change();
            //     divContratista.show();
            // } else {
            //     comboContratista.val('').change();
            // }

            divRadioTipoActo.find('.btnGroup2').find('a').prop('disabled', true);
        }

        function cargarCamposCondicion(item) {

            divImagenAntes.hide();
            divDescargarImagenAntes.show();

            comboNivelPrioridad.val(item.nivelPrioridad);

            if (item.tieneImagenDespues) {
                divImagenDespues.hide();
                divDescargarImagenDespues.show();
                inputFechaResolucion.val(item.fechaResolucion);
                inputAccionCorrectiva.prop('disabled', false);
                inputAccionCorrectiva.val(item.accionCorrectiva);
            }
        }

        function editar(agregarFirmas, target) {

            const rowData = dtTablaRiesgos.row($(target).closest('tr')).data();
            const { tipoRiesgo, id } = rowData;

            $.get('/Administrativo/ActoCondicionCH/ObtenerActoCondicion', { tipoRiesgo, id })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.

                        modalActoCondicion.modal('show');
                        const item = response.items;
                        item.tipoRiesgo = item.tipoRiesgo.toString();

                        alternarDivs(item.tipoRiesgo);
                        cargarCamposComunes(item);

                        switch (item.tipoRiesgo) {
                            case TIPO_RIESGO.ACTO:
                                cargarCamposActo(item);
                                break;

                            case TIPO_RIESGO.CONDICION:
                                cargarCamposCondicion(item);
                                break;

                            default:
                                AlertaGeneral(`Error`, `Tipo de riesgo no definido.`);
                                return;
                        }

                        if (agregarFirmas) {
                            camposFirma(item);
                            btnGuardarFirma.data('id', rowData.id);
                        }

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        //#region firmas
        function initSignaturePad() {
            function resizeCanvasFull() {
                _cFirmaFull.width = divFirmaFull.width();
                _cFirmaFull.height = divFirmaFull.height();
                signaturePadFull.clear();
            }

            window.onresize = resizeCanvasFull;
            resizeCanvasFull();
        }

        function initBotonesFirma() {
            btnFirmaEmpleado.data('tipoFirma', TIPO_FIRMA.EMPLEADO);
            btnFirmaSupervisor.data('tipoFirma', TIPO_FIRMA.SUPERVISOR);
            btnFirmaSST.data('tipoFirma', TIPO_FIRMA.SST);

            btnFirmaEmpleado.text('EMPLEADO');
            btnFirmaSupervisor.text('SUPERVISOR');
            btnFirmaSST.text('SST');
        }

        function mostrarCampoParaFirmar(btn) {
            reiniciarCamposParaFirmar();

            const tipoFirma = btn.data('tipoFirma');

            btnGuardarFirma.data('tipo', tipoFirma);

            switch (tipoFirma) {
                case TIPO_FIRMA.EMPLEADO:
                    claveDelQueFirma.val(inputClaveEmpleado.val());
                    nombreDelQueFirma.val(inputNombreEmpleado.val());
                    break;
                case TIPO_FIRMA.SUPERVISOR:
                    claveDelQueFirma.val(inputClaveSupervisor.val());
                    nombreDelQueFirma.val(inputNombreSupervisor.val());
                    break;
                case TIPO_FIRMA.SST:
                    modalActoCondicion.hide();
                    claveDelQueFirma.prop('disabled', false);
                    btnGuardarFirma.prop('disabled', true);
                    break;
            }

            divFirmaFull.show();
            _cFirmaFull.width = divFirmaFull.width();
            _cFirmaFull.height = document.body.clientHeight;
            signaturePadFull.clear();
        }

        function reiniciarCamposParaFirmar() {
            btnGuardarFirma.data('tipo', '');
            btnGuardarFirma.prop('disabled', false);

            claveDelQueFirma.val('');
            claveDelQueFirma.prop('disabled', true);
            nombreDelQueFirma.val('');
            claveDelQueFirma.prop('disabled', true);
        }

        function reiniciarBtnsFirma() {
            btnFirmaEmpleado.prop('disabled', false);
            btnFirmaSupervisor.prop('disabled', false);
            btnFirmaSST.prop('disabled', false);
        }

        function deshabilitarBtn(tipoFirma) {
            switch (tipoFirma) {
                case TIPO_FIRMA.EMPLEADO:
                    btnFirmaEmpleado.prop('disabled', true);
                    btnFirmaEmpleado.html('<i class="far fa-check-circle"></i> EMPLEADO');
                    break;
                case TIPO_FIRMA.SUPERVISOR:
                    btnFirmaSupervisor.prop('disabled', true);
                    btnFirmaSupervisor.html('<i class="far fa-check-circle"></i> SUPERVISOR');
                    break;
                case TIPO_FIRMA.SST:
                    btnFirmaSST.prop('disabled', true);
                    btnFirmaSST.html('<i class="far fa-check-circle"></i> SST');
                    break;
            }
        }

        function buscarEmpleadoSST() {
            const claveEmpleado = $(this).val();

            if (claveEmpleado) {
                let attrEsContratista = botonGuardar.attr("data-esContratista");
                let idEmpresa = 0;
                let esContratista = false;
                if (attrEsContratista == "true") {
                    esContratista = true;
                } else {
                    if (chkEsContratista.prop('checked')) {
                        esContratista = true;
                    } else {
                        esContratista = false;
                    }
                }

                let obj = new Object()
                obj.claveEmpleado = claveEmpleado
                axios.post("InfEmpleado", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        nombreDelQueFirma.val(response.data.empleado.nombreEmpleado);
                        btnGuardarFirma.prop('disabled', false);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                nombreDelQueFirma.val('');
            }
        }
        //#endregion

        //#region Cargar Zip
        function cargarZip() {
            const data = new FormData();
            data.append('archivoComprimido', inputArchivoZip.get(0).files[0]);

            var xhr = new XMLHttpRequest();
            xhr.onloadstart = () => {
                $.blockUI({
                    message: 'Procesando...',
                    baseZ: 2000
                });
            }
            xhr.onload = () => {
                if (xhr.status == 200) {
                    const respuesta = JSON.parse(xhr.response);
                    if (respuesta.success) {
                        AlertaGeneral('Carga correcta', 'El archivo fue cargado correctamente');
                        cargarActosCondiciones();
                        mdlCargarZip.modal('hide');
                        inputArchivoZip.val('');
                    } else {
                        AlertaGeneral('Alerta', respuesta.message);
                    }
                } else {
                    AlertaGeneral('Alerta', `Ocurrió un error al lanzar la petición al servidor. ${xhr.status} ${xhr.statusText}`);
                }
            }
            xhr.onerror = () => {
                AlertaGeneral('Alerta', `Ocurrió un error al lanzar la petición al servidor.`);
            }
            xhr.addEventListener('loadend', () => {
                $.unblockUI();
            });
            xhr.open('POST', '/Administrativo/ActoCondicionCH/CargarComprimido');
            xhr.send(data);
        }

        function validarArchivoCargarZIP() {
            const cantidadArchivos = inputArchivoZip.get(0).files.length;

            if (cantidadArchivos == 1) {
                const extensionValida = [".zip"];

                if (extensionValida.includes(inputArchivoZip.get(0).value.substring(inputArchivoZip.get(0).value.lastIndexOf('.')))) {
                    return true;
                } else {
                    AlertaGeneral('Alerta', 'Debe se cargar un archivo de tipo zip (.zip)');
                    return false;
                }
            } else if (cantidadArchivos > 1) {
                AlertaGeneral(`Alerta`, `Debe de seleccionar solamente un archivo.`);
                return false;
            } else {
                AlertaGeneral('Alerta', 'Debe se seleccionar un archivo');
                return false;
            }
        }
        //#endregion

        function descargarActa(idActo, ciudad, articulos) {
            const path = `/Reportes/Vista.aspx?esDescargaVisor=true&esVisor=true&idReporte=230&esCH=1&idActo=${idActo}&ciudad=${ciudad}&articulos=${articulos}`
            // const path = '' +
            //     '' +
            //     '' +
            //     '' + '' +
            //     '' + id
            location.href = path;
        }

        function cargarActa(id) {
            const data = new FormData();
            data.append('acta', inputActa.get(0).files[0]);
            data.append('id', id);

            var xhr = new XMLHttpRequest();
            xhr.onloadstart = () => {
                $.bloackUI({
                    message: 'Procesando...',
                    baseZ: 2000
                });
            }
            xhr.onload = () => {
                if (xhr.status == 200) {
                    const respuesta = JSON.parse(xhr.response);
                    if (respuesta.success) {
                        modalCargarActa.modal('hide');
                        AlertaGeneral('Confirmación', 'Se guardó correctamente el acta');
                        cargarActosCondiciones();
                    } else {
                        AlertaGeneral('Alerta', respuesta.message);
                    }
                } else {
                    AlertaGeneral('Alerta', `Ocurrió un error al lanzar la petición al servidor. ${xhr.status} ${xhr.statusText}`);
                }
            }
            xhr.onerror = () => {
                AlertaGeneral('Alerta', `Ocurrió un error al lanzar la petición al servidor.`);
            }
            xhr.addEventListener('loadend', () => {
                $.unblockUI();
            });
            xhr.open('POST', '/Administrativo/ActoCondicionCH/CargarActa');
            xhr.send(data);
        }

        //#region autocomplete
        inputNombreSupervisor.getAutocompleteParams(selectAutocompleteEmpleado, null, '/Administrativo/ActoCondicionCH/GetInfoEmpleadoInternoContratista', getParametrosAutocompleteEmpleadoInterno);
        inputNombreEmpleadoInformo.getAutocompleteParams(selectAutocompleteEmpleado, null, '/Administrativo/ActoCondicionCH/GetInfoEmpleadoInternoContratista', getParametrosAutocompleteEmpleadoInterno);
        inputNombreEmpleado.getAutocompleteParams(selectAutocompleteEmpleado, null, '/Administrativo/ActoCondicionCH/GetInfoEmpleadoInternoContratista', getParametrosAutocompleteEmpleado);

        modalActoCondicion.on("autocompleteselect", '.nombreEmpleado', function (event, ui) {
            event.preventDefault();

            let soloNombre = ui.item.label.replace(' [CONSTRUPLAN]', '');
            soloNombre = soloNombre.replace(' [ARRENDADORA]', '');

            $(this).text(soloNombre);
            $(this).val(soloNombre);
        });

        modalActoCondicion.on("autocompletechange", '.nombreEmpleado', function (event, ui) {
            // if (!ui.item) {
            //     const id = $(this).attr('id');

            //     let limpiar = true;

            //     switch (id) {
            //         case 'inputNombreSupervisor':
            //             inputClaveSupervisor.val('');
            //             break;
            //         case 'inputNombreEmpleadoInformo':
            //             inputClaveEmpleadoInformo.val('');
            //             break;
            //     }

            //     if (limpiar) {
            //         $(this).val('');
            //         $(this).text('');
            //         $(this).attr('data-index', '');
            //     }
            // }
        });

        function selectAutocompleteEmpleado(event, ui) {
            $(this).text('');
            $(this).attr('data-index', '');
            $(this).removeData();

            const claveEmpleado = ui.item.id.split('-')[0];
            const puestoEmpleado = ui.item.id.split('-')[1];
            const fechaAlta = ui.item.id.split('-')[2];

            const id = $(this).attr('id');

            switch (id) {
                case 'inputNombreSupervisor':
                    inputClaveSupervisor.val(claveEmpleado);
                    break;
                case 'inputNombreEmpleadoInformo':
                    inputClaveEmpleadoInformo.val(claveEmpleado);
                    break;
                case 'inputNombreEmpleado':
                    inputClaveEmpleado.val(claveEmpleado);
                    inputClaveEmpleado.change();
                    break;
            }

            $(this).text(ui.item.label);
            $(this).attr('data-index', ui.item.id);
        }
        //#endregion

        //#region Descargar reporte excel
        function descargarReporteExcel() {
            const filtro = JSON.stringify(obtenerFiltroBusqueda());

            const data = new FormData();
            data.append('filtro', filtro);

            var xhr = new XMLHttpRequest();
            xhr.responseType = 'blob';

            xhr.onloadstart = () => {
                $.blockUI({
                    message: 'Procesando...',
                    baseZ: 2000
                });
            }

            xhr.onload = () => {
                if (xhr.status == 200) {
                    var blob = xhr.response;

                    if (blob.size > 0) {
                        var fileName = xhr.getResponseHeader('content-disposition');
                        var link = document.createElement('a');
                        link.href = window.URL.createObjectURL(blob);
                        link.download = fileName.split('filename=')[1];
                        link.click();
                    } else {
                        AlertaGeneral('Alerta', 'Ocurrió un error al generar el reporte');
                    }
                } else {
                    AlertaGeneral('Alerta', `Ocurrió un error al lanzar la petición al servidor. ${xhr.status} ${xhr.statusText}`);
                }
            }

            xhr.onerror = () => {
                AlertaGeneral('Alerta', `Ocurrió un error al lanzar la petición al servidor.`);
            }

            xhr.addEventListener('loadend', () => {
                $.unblockUI();
            });

            xhr.open('POST', '/Administrativo/ActoCondicionCH/DescargarReporteExcel');
            xhr.send(data);
        }
        //#endregion
    }

    $(document).ready(() => Administrativo.ActoCondicion = new ActoCondicion())
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); });
})();