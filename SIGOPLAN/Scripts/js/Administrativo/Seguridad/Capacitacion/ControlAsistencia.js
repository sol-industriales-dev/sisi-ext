(() => {
    $.namespace('Adminstrativo.Seguridad.Capacitacion.ControlAsistencia');

    ControlAsistencia = function () {

        // Variables.
        const inputEmpresaActual = $('#inputEmpresaActual');

        //// Url autocompletes
        const getCursosAutocompleteURL = '/Administrativo/Capacitacion/GetCursosAutocomplete';
        const getUsuariosAutocompleteURL = '/Administrativo/Capacitacion/GetUsuariosAutocomplete';
        const getGetLugarCursoAutocompleteURL = '/Administrativo/Capacitacion/GetLugarCursoAutocomplete';
        const getEmpleadoEnKontrolAutocompleteURL = '/Administrativo/Capacitacion/GetEmpleadoEnKontrolAutocomplete';

        // Filtros
        const comboCC = $('#comboCC');
        const comboEstado = $('#comboEstado');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const botonBuscar = $('#botonBuscar');

        // Tabla control asistencia
        const botonNuevo = $('#botonNuevo');
        const tablaControlAsistencia = $('#tablaControlAsistencia');
        let dtTablaControlAsistencia;
        const inputArchivoControlAsistencia = $('#inputArchivoControlAsistencia');

        // Carga masiva
        const botonCargaMasiva = $('#botonCargaMasiva');
        const modalCargaMasiva = $('#modalCargaMasiva');
        const botonGuardarCargaMasiva = $('#botonGuardarCargaMasiva');

        //Migrar Empleado
        const botonMigrarEmpleado = $('#botonMigrarEmpleado');
        const modalMigrarEmpleado = $('#modalMigrarEmpleado');
        const botonGuardarMigracionEmpleado = $('#botonGuardarMigracionEmpleado');
        const inputClaveEmpleadoMigracion = $('#inputClaveEmpleadoMigracion');
        const inputNombreEmpleadoMigracion = $('#inputNombreEmpleadoMigracion');
        const selectCentroCostoMigracion = $('#selectCentroCostoMigracion');

        // Modal alta lista asistencia
        const modalAltaListaAsistencia = $('#modalAltaListaAsistencia');
        const comboAltaCC = $('#comboAltaCC');
        const inputFechaCurso = $('#inputFechaCurso');
        const inputDuracionCurso = $('#inputDuracionCurso');
        const inputClaveCurso = $('#inputClaveCurso');
        const inputNombreCurso = $('#inputNombreCurso');
        const spanClasificacionCurso = $('#spanClasificacionCurso');
        const inputClaveInstructor = $('#inputClaveInstructor');
        const inputNombreInstructor = $('#inputNombreInstructor');

        const checkboxValidacion = $('#checkboxValidacion');
        const checkboxExterno = $('#checkboxExterno');
        const divExterno = $('#divExterno');
        const inputInstructorExterno = $('#inputInstructorExterno');
        const inputEmpresaExterna = $('#inputEmpresaExterna');

        const inputLugarCurso = $('#inputLugarCurso');
        const inputHorarioCurso = $('#inputHorarioCurso');
        const botonCrearListaAsistencia = $('#botonCrearListaAsistencia');

        //// Lista de asistentes
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const inputNombreEmpleado = $('#inputNombreEmpleado');
        const botonAgregarAsistente = $('#botonAgregarAsistente');
        const tablaAsistentes = $('#tablaAsistentes');
        let dtTablaAsistentes;

        // Modal detalles control asistencia
        const modalDetallesControlAsistencia = $('#modalDetallesControlAsistencia');
        const inputDetalleCC = $('#inputDetalleCC');
        const inputDetalleFechaCurso = $('#inputDetalleFechaCurso');
        const inputDetalleDuracionCurso = $('#inputDetalleDuracionCurso');
        const inputDetalleClaveCurso = $('#inputDetalleClaveCurso');
        const inputDetalleNombreCurso = $('#inputDetalleNombreCurso');
        const spanDetalleClasificacionCurso = $('#spanDetalleClasificacionCurso');
        const inputDetalleEmpresaExterna = $('#inputDetalleEmpresaExterna');
        const inputDetalleNombreInstructor = $('#inputDetalleNombreInstructor');
        const inputDetalleLugarCurso = $('#inputDetalleLugarCurso');
        const inputDetalleHorarioCurso = $('#inputDetalleHorarioCurso');
        const tablaAsistentesDetalles = $('#tablaAsistentesDetalles');
        let dtTablaAsistentesDetalles;

        // Modal gestión de exámenes
        const modalExamenes = $('#modalExamenes');
        const inputExamenCC = $('#inputExamenCC');
        const inputExamenFechaCurso = $('#inputExamenFechaCurso');
        const inputExamenNombreInstructor = $('#inputExamenNombreInstructor');
        const inputExamenNombreCurso = $('#inputExamenNombreCurso');
        const inputJefeDepartamento = $('#inputJefeDepartamento');
        const inputCoordinadorCSH = $('#inputCoordinadorCSH');
        const inputSecretarioCSH = $('#inputSecretarioCSH');
        const inputGerenteProyecto = $('#inputGerenteProyecto');
        const inputRFC = $('#inputRFC');
        const selectRazonSocial = $('#selectRazonSocial');
        const botonGuardarExamenes = $('#botonGuardarExamenes');
        const tablaExamenes = $('#tablaExamenes');
        let dtTablaExamenes;

        // Modal DC-3
        const modalDC3 = $('#modalDC3');
        const inputExamenCCDC3 = $('#inputExamenCCDC3');
        const inputExamenFechaCursoDC3 = $('#inputExamenFechaCursoDC3');
        const inputExamenNombreInstructorDC3 = $('#inputExamenNombreInstructorDC3');
        const inputExamenNombreCursoDC3 = $('#inputExamenNombreCursoDC3');
        const inputJefeDepartamentoDC3 = $('#inputJefeDepartamentoDC3');
        const inputCoordinadorCSHDC3 = $('#inputCoordinadorCSHDC3');
        const inputSecretarioCSHDC3 = $('#inputSecretarioCSHDC3');
        const inputGerenteProyectoDC3 = $('#inputGerenteProyectoDC3');
        const inputRFCDC3 = $('#inputRFCDC3');
        const selectRazonSocialDC3 = $('#selectRazonSocialDC3');
        const botonGuardarDC3 = $('#botonGuardarDC3');
        const tablaDC3 = $('#tablaDC3');
        let dtTablaDC3;

        const puedeEliminar = $('#inputPuedeEliminarControlAsistencia').val() == "True";

        _moduloCapacitacionOperativa = false;

        // Reporte
        const report = $("#report");

        // Lógica de inicialización.
        (function init() {

            initDatepickers();

            llenarCombos();

            inicializarDatatables();

            agergarListeners();

            checarModuloCapacitacionOperativa();

            divExterno.hide();
        })();

        // Métodos.
        function inicializarDatatables() {

            initTablaControlAsistencia();

            initTablaListaAsistentes();

            initTablaListaAsistentesDetalles();

            initTablaDC3();
        }

        function initDatepickers() {

            const fechaActual = new Date();

            inputFechaInicio.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": fechaActual
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", -7);

            inputFechaFin.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": fechaActual
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", fechaActual);

            inputFechaCurso.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": fechaActual
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", fechaActual);
        }

        function agergarListeners() {

            botonBuscar.click(cargarListaControlAsistencia);

            botonNuevo.click(() => modalAltaListaAsistencia.modal('show'));

            checkboxExterno.click(alternarCursoExterno);

            inputClaveCurso.getAutocompleteValid(setDatosCurso, verificarCurso, { porClave: true }, getCursosAutocompleteURL);
            inputNombreCurso.getAutocompleteValid(setDatosCurso, verificarCurso, { porClave: false }, getCursosAutocompleteURL);

            inputClaveInstructor.getAutocompleteValid(setDatosInstructor, verificarInstructor, { porClave: true }, getUsuariosAutocompleteURL);
            inputNombreInstructor.getAutocompleteValid(setDatosInstructor, verificarInstructor, { porClave: false }, getUsuariosAutocompleteURL);

            inputClaveEmpleado.getAutocompleteValid(setDatosEmpleado, verificarEmpleado, { porClave: true }, getEmpleadoEnKontrolAutocompleteURL);
            inputNombreEmpleado.getAutocompleteValid(setDatosEmpleado, verificarEmpleado, { porClave: false }, getEmpleadoEnKontrolAutocompleteURL);

            inputClaveEmpleadoMigracion.getAutocompleteValid(setDatosEmpleadoMigracion, verificarEmpleadoMigracion, { porClave: true }, getEmpleadoEnKontrolAutocompleteURL);
            inputNombreEmpleadoMigracion.getAutocompleteValid(setDatosEmpleadoMigracion, verificarEmpleadoMigracion, { porClave: false }, getEmpleadoEnKontrolAutocompleteURL);

            inputLugarCurso.getAutocomplete(setLugarCurso, null, getGetLugarCursoAutocompleteURL);

            inputJefeDepartamento.getAutocompleteValid(setDatosUsuario, verificarDatosUsuario, { porClave: false }, getUsuariosAutocompleteURL);
            inputCoordinadorCSH.getAutocompleteValid(setDatosUsuario, verificarDatosUsuario, { porClave: false }, getUsuariosAutocompleteURL);
            inputSecretarioCSH.getAutocompleteValid(setDatosUsuario, verificarDatosUsuario, { porClave: false }, getUsuariosAutocompleteURL);
            inputGerenteProyecto.getAutocompleteValid(setDatosUsuario, verificarDatosUsuario, { porClave: false }, getUsuariosAutocompleteURL);

            modalAltaListaAsistencia.find(`button.mostrarInfoCurso`).unbind().click(mostrarInfoCurso);
            modalDetallesControlAsistencia.find(`button.mostrarDetalleInfoCurso`).unbind().click(mostrarDetalleInfoCurso);

            modalAltaListaAsistencia.on('hide.bs.modal', limpiarCamposModalAlta);
            modalExamenes.on('hide.bs.modal', limpiarCamposModalExamenes);

            botonAgregarAsistente.click(agregarAsistente);

            botonCrearListaAsistencia.click(crearListaAsistencia);

            inputArchivoControlAsistencia.change(cargarArchivoControlAsistencia);

            botonGuardarExamenes.click(guardarExamenesAsistentes);
            botonGuardarDC3.click(guardarArchivosDC3)

            botonCargaMasiva.click(() => { modalCargaMasiva.modal('show') });
            botonMigrarEmpleado.click(() => {
                limpiarModalMigracion();
                modalMigrarEmpleado.modal('show');
            });
            botonGuardarCargaMasiva.click(guardarCargaMasiva);
            botonGuardarMigracionEmpleado.click(guardarMigracionEmpleado);
        }

        function alternarCursoExterno() {
            const checked = this.checked;
            inputClaveInstructor.attr('disabled', checked);
            inputClaveInstructor.val('');
            inputNombreInstructor.attr('disabled', checked);
            inputNombreInstructor.val('');
            if (checked) {
                divExterno.show(500);
            } else {
                divExterno.hide(500);
                inputInstructorExterno.val('');
                inputEmpresaExterna.val('');
            }
        }

        function checarModuloCapacitacionOperativa() {
            axios.get('checarModuloCapacitacionOperativa')
                .then(response => {
                    _moduloCapacitacionOperativa = response.data == 'True';

                    //Ocultar Autorizantes
                    $('#divAutorizantes').css('display', response.data == 'True' ? 'none' : 'block');

                    //Inicializar la tabla de exámenes para ocultar la columna de examen de diagnóstico.
                    initTablaExamenes();
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarExamenesAsistentes() {

            const esCursoGeneral = modalExamenes.data().clasificacion == 3 || modalExamenes.data().clasificacion == 4;

            if (esCursoGeneral) {
                guardarEvaluacionesAsistentes();
            } else {

                const examenesValidos = validarExamenesCargados();

                if (examenesValidos == false) {
                    AlertaGeneral(`Aviso`, `Faltan exámenes por cargar.`);
                    return;
                }

                let todoEnOrden = true;

                const asistentes = tablaExamenes.find('tbody tr[role="row"] td .inputExamenArchvo').closest('tr').toArray();

                $.blockUI({ message: 'Subiendo exámenes...' });

                for (let index = 0; index < asistentes.length; index++) {

                    const row = $(asistentes[index]);

                    const rowData = dtTablaExamenes.row(row).data();

                    const examenDiagnostico = !_moduloCapacitacionOperativa ? row.find('input.inputExamenDiagnostico')[0].files[0] : null;
                    const examenFinal = row.find('input.inputExamenFinal')[0].files[0];
                    const aprobado = row.find('input.inputAproboExamen')[0].checked;
                    const calificacion = +row.find('.inputCalificacion').val()

                    const data = new FormData();
                    data.append("examenDiagnostico", examenDiagnostico);
                    data.append("examenFinal", examenFinal);
                    data.append("id", rowData.id);
                    data.append("claveEmpleado", rowData.claveEmpleado);
                    data.append("aprobado", aprobado);
                    data.append("calificacion", calificacion);

                    $.ajax({
                        url: '/Administrativo/Capacitacion/CargarExamenesAsistente',
                        data,
                        async: false,
                        cache: false,
                        contentType: false,
                        processData: false,
                        method: 'POST',
                        success: response => {
                            if (response.success == false) {
                                todoEnOrden = false;
                            }
                        },
                        error: () => todoEnOrden = false
                    });

                    if (todoEnOrden == false) {
                        break;
                    }
                }

                $.unblockUI();

                if (todoEnOrden == false) {
                    AlertaGeneral(`Error`, `Ocurrió un error al guardar los exámenes de los asistentes.`);
                    return;
                }

                const jefeID = inputJefeDepartamento.data().usuarioID != null ? inputJefeDepartamento.data().usuarioID : 0;
                const coordinadorID = inputCoordinadorCSH.data().usuarioID != null ? inputCoordinadorCSH.data().usuarioID : 0;
                const secretarioID = inputSecretarioCSH.data().usuarioID != null ? inputSecretarioCSH.data().usuarioID : 0;
                const gerenteID = inputGerenteProyecto.data().usuarioID != null ? inputGerenteProyecto.data().usuarioID : 0;
                const rfc = inputRFC.val();
                const razonSocial = selectRazonSocial.val();

                $.blockUI({ message: 'Guardando exámenes...' });
                $.post('/Administrativo/Capacitacion/GuardarExamenesAsistentes', { jefeID, coordinadorID, secretarioID, gerenteID, rfc, razonSocial })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            modalExamenes.modal('hide');
                            if (response.aplicaAutorizacion || response.aplicaAutorizacionPorPuesto) {
                                generarReporte(response.controlAsistenciaID);
                            } else {
                                AlertaGeneral(`Éxito`, `Evaluación guardada correctamente.`);
                                cargarListaControlAsistencia();
                            }
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

        }

        function guardarArchivosDC3() {
            const asistentes = tablaDC3.find('tbody tr[role="row"]').toArray();
            var flagGuardadoCorrecto = false;

            $.blockUI({ message: 'Guardando Archivos...' });

            for (let index = 0; index < asistentes.length; index++) {
                const row = $(asistentes[index]);
                const rowData = dtTablaDC3.row(row).data();
                const inputFile = row.find('input.inputDC3')[0];

                if (inputFile != undefined) {
                    const archivoDC3 = row.find('input.inputDC3')[0].files[0];

                    if (archivoDC3 != undefined) {
                        const data = new FormData();

                        data.append("archivoDC3", archivoDC3);
                        data.append("controlAsistenciaDetalleID", rowData.id);

                        $.ajax({
                            url: '/Administrativo/Capacitacion/GuardarArchivosDC3',
                            data,
                            async: false,
                            cache: false,
                            contentType: false,
                            processData: false,
                            method: 'POST',
                            success: response => {
                                flagGuardadoCorrecto = response.success;
                            },
                            error: (response) => {
                                AlertaGeneral(`Alerta`, `Error en la petición. ${response.message}`);
                            }
                        });
                    }
                }
            }

            $.unblockUI();

            if (flagGuardadoCorrecto) {
                modalDC3.modal('hide');
                AlertaGeneral(`Alerta`, `Se han guardado correctamente los archivos.`);
            }
        }

        function validarExamenesCargados() {
            const examenes = tablaExamenes.find('tbody tr[role="row"] input.inputExamenArchvo');
            return examenes.length == examenes.toArray().filter(x => $(x)[0].files.length > 0).length;
        }

        function guardarEvaluacionesAsistentes() {

            const listaAsistentes = tablaExamenes.find('tbody tr[role="row"]')
                .toArray().map(row => {

                    row = $(row);

                    return {
                        id: dtTablaExamenes.row(row).data().id,
                        aprobado: row.find('input.inputAproboExamen')[0].checked,
                        calificacion: +row.find('.inputCalificacion').val()
                    };
                });

            debugger;

            $.blockUI({ message: 'Guardando evaluaciones...' });
            $.post('/Administrativo/Capacitacion/GuardarEvaluacionAsistentes', { listaAsistentes })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Evaluación guardada correctamente.`);
                        modalExamenes.modal('hide');
                        cargarListaControlAsistencia();
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

        function limpiarCamposModalAlta() {

            comboAltaCC.prop("selectedIndex", 0);
            inputFechaCurso.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": new Date()
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", new Date());
            inputDuracionCurso.val('');
            inputClaveCurso.val('');
            inputClaveCurso.data().cursoID = null;
            inputNombreCurso.val('');
            inputClaveInstructor.val('');
            inputClaveInstructor.data().instructorID = null;
            inputNombreInstructor.val('');
            inputLugarCurso.val('');
            inputHorarioCurso.val('');
            dtTablaAsistentes.clear().draw();
            establecerInformacionLabelClaisificacion('', '');

            modalAltaListaAsistencia.data().objetivo = null;
            modalAltaListaAsistencia.data().temasPrincipales = null;
            modalAltaListaAsistencia.data().referenciasNormativas = null;
            modalAltaListaAsistencia.data().nota = null;

            checkboxValidacion[0].checked = false;
            checkboxExterno[0].checked = false;
            inputInstructorExterno.val('');
            inputEmpresaExterna.val('');
            divExterno.hide();
            inputClaveInstructor.attr('disabled', false);
            inputNombreInstructor.attr('disabled', false);
        }

        function limpiarCamposModalExamenes() {
            inputJefeDepartamento.val('');
            inputCoordinadorCSH.val('');
            inputSecretarioCSH.val('');
            inputGerenteProyecto.val('');
            selectRazonSocial.val('');
            inputRFC.val('');
        }

        function crearListaAsistencia() {
            const controlAsistencia = obtenerDatosListaAsistencia();

            if (controlAsistencia === false) {
                AlertaGeneral(`Aviso`, `Insuficientes datos para poder dar de alta un control de asistencia.`);
                return;
            }

            modalAltaListaAsistencia.modal('hide');

            $.blockUI({ message: 'Creando lista de asistencia...' });
            $.post('/Administrativo/Capacitacion/CrearControlAsistencia', { controlAsistencia })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Control de asistencia creado.`);
                        cargarListaControlAsistencia();
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

        function generarReporte(controlAsistenciaID) {
            // CRModal = Crystal Reports Modal.
            $.blockUI({ message: 'Generando reporte...' });
            var path = `/Reportes/Vista.aspx?idReporte=170&controlAsistenciaID=${controlAsistenciaID}&inMemory=${1}&isCRModal=${false}`;
            report.attr("src", path);
            report[0].onload = () => enviarCorreoAut(controlAsistenciaID);
        }

        function enviarCorreoAut(controlAsistenciaID) {
            $.post('/Administrativo/Capacitacion/EnviarCorreoAutorizacion', { controlAsistenciaID })
                .always(() => {
                    $.unblockUI();
                    cargarListaControlAsistencia();
                })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Evaluación guardada correctamente.`);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `La evaluación fue correctamente pero ocurrió un error al enviar el correo de notificación.`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function obtenerDatosListaAsistencia() {
            const cursoID = inputClaveCurso.data().cursoID;
            const instructorID = inputClaveInstructor.data().instructorID;
            const fechaCapacitacion = inputFechaCurso.val();
            const cc = comboAltaCC.val();
            const empresa = +comboAltaCC.find('option:selected').attr('empresa');
            const lugar = inputLugarCurso.val();
            const horario = inputHorarioCurso.val();
            const estatus = 1;
            const activo = true;
            const asistentes = obtenerDatosAsistentes();

            let instructorExterno = "";
            let empresaExterna = "";

            const validacion = checkboxValidacion[0].checked;
            const esExterno = checkboxExterno[0].checked;

            if (esExterno) {
                instructorExterno = inputInstructorExterno.val().trim().toUpperCase();
                empresaExterna = inputEmpresaExterna.val().trim().toUpperCase();

                if (instructorExterno == null || instructorExterno.trim().length == 0 ||
                    empresaExterna == null || empresaExterna.trim().length == 0) {
                    return false;
                }
            }

            // Se validan los datos.
            if (
                cursoID == null ||
                (esExterno ? false : instructorID == null) ||
                fechaCapacitacion == null || fechaCapacitacion == "" ||
                cc == "-1" ||
                lugar == "" ||
                horario == "" ||
                asistentes.length == 0
            ) {
                return false;
            }

            return {
                cursoID,
                instructorID,
                fechaCapacitacion,
                cc,
                empresa,
                lugar,
                horario,
                estatus,
                activo,
                asistentes,
                validacion,
                esExterno,
                instructorExterno,
                empresaExterna
            };
        }

        function obtenerDatosAsistentes() {
            const asistentes = [];

            dtTablaAsistentes.rows().data().toArray().forEach(asistente => {
                const { claveEmpleado, puesto, ccID
                    // , aplicaAutorizacion 
                } = asistente;
                asistentes.push({
                    claveEmpleado,
                    puesto,
                    cc: ccID,
                    estatus: 1,
                    // estatusAutorizacion: aplicaAutorizacion ? 2 : 1
                });
            });
            return asistentes;
        }

        function agregarAsistente() {

            if (inputNombreEmpleado.val() == "") {
                return;
            }

            let inputData = inputNombreEmpleado.data().uiAutocomplete;

            if (inputData == null) {
                return;
            }

            let empleadoData = inputData.selectedItem;

            if (empleadoData == null) {
                empleadoData = inputClaveEmpleado.data().uiAutocomplete.selectedItem;
                if (empleadoData == null) {
                    return;
                }
            }

            const { id, nombreEmpleado, cc, ccID, puestoEmpleado, } = empleadoData;

            if (tablaAsistentes.find('tbody tr').toArray().some(x => $(x).find('p').text() == id)) {
                return;
            }

            añadirRowAsistente(id, nombreEmpleado, puestoEmpleado, `${ccID} - ${cc}`, false, ccID);
            inputNombreEmpleado.val('');
            inputClaveEmpleado.val('');
            inputNombreEmpleado.focus();
        }

        function añadirRowAsistente(claveEmpleado, nombre, puesto, ccDesc
            // , aplicaAutorizacion
            , ccID) {
            dtTablaAsistentes.row.add({
                claveEmpleado,
                nombre,
                puesto,
                ccDesc,
                // aplicaAutorizacion,
                ccID
            }).draw();
        }

        function mostrarInfoCurso(e) {

            const buttonInfo = $(e.currentTarget).attr('info');

            let titulo = "";
            let texto = "";

            switch (buttonInfo) {
                case 'objetivo':
                    titulo = "Objetivos";
                    texto = modalAltaListaAsistencia.data().objetivo;
                    break;
                case 'tema':
                    titulo = "Temas Principales";
                    texto = modalAltaListaAsistencia.data().temasPrincipales;
                    break;
                case 'referencia':
                    titulo = "Referencias Normativas";
                    texto = modalAltaListaAsistencia.data().referenciasNormativas;
                    break;
                case 'nota':
                    titulo = "Notas";
                    texto = modalAltaListaAsistencia.data().nota;
                    break;
                default:
                    titulo = "Error";
                    texto = "Indefinido";
                    break;
            }

            texto = texto.split("\n").map(line => `<p>${line}</p>`).join(`</p>`);
            AlertaGeneral(`${titulo}`, `${texto}`);
        }

        function mostrarDetalleInfoCurso(e) {

            const buttonInfo = $(e.currentTarget).attr('info');

            let titulo = "";
            let texto = "";

            const infoCurso = modalDetallesControlAsistencia.data();

            switch (buttonInfo) {
                case 'objetivo':
                    titulo = "Objetivos";
                    texto = infoCurso.objetivos;
                    break;
                case 'tema':
                    titulo = "Temas Principales";
                    texto = infoCurso.temasPrincipales;
                    break;
                case 'referencia':
                    titulo = "Referencias Normativas";
                    texto = infoCurso.referenciasNormativas;
                    break;
                case 'nota':
                    titulo = "Notas";
                    texto = infoCurso.notas;
                    break;
                default:
                    titulo = "Error";
                    texto = "Indefinido";
                    break;
            }

            texto = texto.split("\n").map(line => `<p>${line}</p>`).join(`</p>`);
            AlertaGeneral(`${titulo}`, `${texto}`);
        }

        function setDatosCurso(e, ui) {
            inputNombreCurso.val(ui.item.nombre);
            inputClaveCurso.val(ui.item.claveCurso);
            inputClaveCurso.data().cursoID = ui.item.id;
            inputDuracionCurso.val(obtenerTextoHoras(ui.item.duracion));

            modalAltaListaAsistencia.data().objetivo = ui.item.objetivo;
            modalAltaListaAsistencia.data().temasPrincipales = ui.item.temasPrincipales;
            modalAltaListaAsistencia.data().referenciasNormativas = ui.item.referenciasNormativas;
            modalAltaListaAsistencia.data().nota = ui.item.nota;

            establecerInformacionLabelClaisificacion(ui.item.claseSpan, ui.item.clasificacion)
        }

        function establecerInformacionLabelClaisificacion(claseSpan, clasificacion) {
            spanClasificacionCurso.removeClass('label-danger').removeClass('label-warning').removeClass('label-default');
            spanClasificacionCurso.addClass(claseSpan);
            spanClasificacionCurso.html(clasificacion);
        }

        function establecerInformacionLabelDetalleClaisificacion(claseSpan, clasificacion) {
            spanDetalleClasificacionCurso.removeClass('label-danger').removeClass('label-warning').removeClass('label-default');
            spanDetalleClasificacionCurso.addClass(claseSpan);
            spanDetalleClasificacionCurso.html(clasificacion);
        }

        function verificarCurso(e, ui) {
            if (ui.item == null) {
                inputNombreCurso.val('');
                inputClaveCurso.val('');
                inputClaveCurso.data().cursoID = null;
                inputDuracionCurso.val('');
                establecerInformacionLabelClaisificacion('', '');
            }
        }

        function setDatosInstructor(e, ui) {
            inputClaveInstructor.val(ui.item.claveEmpleado);
            inputClaveInstructor.data().instructorID = ui.item.id;
            inputNombreInstructor.val(ui.item.nombre);
        }

        function verificarInstructor(e, ui) {
            if (ui.item == null) {
                inputClaveInstructor.val('');
                inputClaveInstructor.data().instructorID = null;
                inputNombreInstructor.val('')
            }
        }

        function setDatosUsuario(e, ui) {
            $(this).val(ui.item.nombre);
            $(this).data().usuarioID = ui.item.id;
        }

        function verificarDatosUsuario(e, ui) {
            if (ui.item == null) {
                $(this).val('');
                $(this).data().usuarioID = null;
            }
        }

        function setDatosEmpleado(e, ui) {
            inputClaveEmpleado.val(ui.item.id);
            inputNombreEmpleado.val(ui.item.nombreEmpleado);
        }

        function verificarEmpleado(e, ui) {
            if (ui.item == null) {
                inputClaveEmpleado.val('');
                inputNombreEmpleado.val('');
            }
        }

        function setDatosEmpleadoMigracion(e, ui) {
            inputClaveEmpleadoMigracion.val(ui.item.id);
            inputNombreEmpleadoMigracion.val(ui.item.nombreEmpleado);
        }

        function verificarEmpleadoMigracion(e, ui) {
            if (ui.item == null) {
                inputClaveEmpleadoMigracion.val('');
                inputNombreEmpleadoMigracion.val('');
            }
        }

        function limpiarModalMigracion() {
            inputClaveEmpleadoMigracion.val('');
            inputNombreEmpleadoMigracion.val('');
            selectCentroCostoMigracion.val('');
        }

        function setLugarCurso(e, ui) {
            inputLugarCurso.val(ui.item.value);
        }

        function obtenerTextoHoras(duracion) {
            return duracion == 1 ? `${duracion} hora` : `${duracion} horas`;
        }

        function cargarListaControlAsistencia() {

            const cc = comboCC.val();
            const estado = comboEstado.val();
            const fechaInicio = inputFechaInicio.val();
            const fechaFin = inputFechaFin.val();

            $.blockUI({ message: 'Cargando listado de controles de asistencia...' });
            $.get('/Administrativo/Capacitacion/ObtenerListaControlesAsistencia', { cc, estado, fechaInicio, fechaFin })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        if (response.items && response.items.length > 0) {
                            dtTablaControlAsistencia.clear().rows.add(response.items).draw();
                        } else {
                            dtTablaControlAsistencia.clear().draw();
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

        function llenarCombos() {
            selectRazonSocial.fillCombo('/Administrativo/Capacitacion/GetRazonSocialCombo', null, false, null);

            axios.get('ObtenerComboCCAmbasEmpresas').then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    comboCC.append(`<option value="Todos">Todos</option>`);
                    comboAltaCC.append(`<option value="-1">Seleccionar</option>`);

                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}">`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                        });

                        groupOption += `</optgroup>`;

                        comboCC.append(groupOption);

                        let groupOption2 = `<optgroup label="${x.label}">`;

                        x.options.forEach(y => {
                            groupOption2 += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                        });

                        groupOption2 += `</optgroup>`;

                        comboAltaCC.append(groupOption2);
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }

                convertToMultiselect('#selectCentroCosto');
            }).catch(error => AlertaGeneral(`Alerta`, error.message));

            axios.post('ObtenerComboCCMigracion').then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    selectCentroCostoMigracion.append(`<option value="">--Seleccione--</option>`);

                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}">`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                        });

                        groupOption += `</optgroup>`;

                        selectCentroCostoMigracion.append(groupOption);
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));

            $.get('/Administrativo/Capacitacion/ObtenerEstatusControlAsistencia').always($.unblockUI).then(response => {
                if (response) {
                    // Operación exitosa.
                    comboEstado.append(`<option value=0>Todos</option>`);
                    comboEstado.append(response.map(item => `<option value=${item.Value} >${item.Text}</option>`).join(''));
                } else {
                    // Operación no completada.
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación.`);
                }
            }, () => AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor.`));
        }

        function esExtensionInvalida(nombreArchivo) {
            const extensionesNoAceptadas = ["exe", "app", "vb", "scr", "vbe", "vbs"];
            const extension = nombreArchivo.split('.').pop();
            const extensionNoEsValida = extensionesNoAceptadas.filter(x => x === extension);
            return extensionNoEsValida.length > 0;
        }

        function cargarArchivoControlAsistencia() {

            const inputArchivo = inputArchivoControlAsistencia[0];

            if (inputArchivo.length == 0) {
                return;
            }

            const archivo = inputArchivo.files[0];

            if (esExtensionInvalida(archivo.name)) {
                AlertaGeneral(`Aviso`, `El archivo tiene una extensión inválida.`);
                return;
            }

            const controlAsistenciaID = inputArchivoControlAsistencia.data().controlAsistenciaID;

            const data = new FormData();
            data.append("archivo", archivo);
            data.append("controlAsistenciaID", controlAsistenciaID);

            $.blockUI({ message: 'Subiendo archivo...' });
            $.ajax({
                url: '/Administrativo/Capacitacion/SubirArchivoControlAsistencia',
                data,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
            })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        inputArchivoControlAsistencia.val('');
                        AlertaGeneral(`Éxito`, `Archivo cargado correctamente.`);
                        cargarListaControlAsistencia();
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

        function initTablaControlAsistencia() {

            dtTablaControlAsistencia = tablaControlAsistencia.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: true,
                columns: [
                    { data: 'fecha', title: 'Fecha' },
                    { data: 'curso', title: 'Curso' },
                    { data: 'instructor', title: 'Instructor' },
                    { data: 'lugar', title: 'Lugar' },
                    { data: 'estatusDesc', title: 'Estatus' },
                    { data: 'cc', title: 'Centro de Costos' },
                    {
                        data: 'id', title: 'Lista de Asistencia', render: (data, type, row) => {
                            if (row.estatus == 1) {
                                return `
                                <button class="btn btn-primary reporteControlAsistencia"><i class="fas fa-print"></i> Imprimir</button>
                                <button class="btn btn-primary botonCargarLista"><i class="fas fa-arrow-alt-circle-up"></i> Cargar</button>
                                `;
                            } else {
                                return '<button class="btn btn-primary botonDescargarLista"><i class="fas fa-arrow-alt-circle-down"></i> Descargar</button>'
                            }
                        }
                    },
                    {
                        data: 'id', title: 'Detalles', render: (data, type, row) =>
                            '<button class="btn btn-primary verLista"><i class="fas fa-info-circle"></i></button>'
                    },
                    {
                        data: 'id', title: inputEmpresaActual.val() != 6 ? 'DC-3' : 'Certificado de Trabajo', render: (data, type, row) =>
                            (row.estatus == 1 ? '' : '<button class="btn btn-primary botonDC3"><i class="fas fa-list-ol"></i></button>')
                    },
                    {
                        data: 'id', title: 'Evaluaciones', render: (data, type, row) =>
                            (row.estatus == 1 ? '' : '<button class="btn btn-primary botonExamenes"><i class="fas fa-list-ol"></i></button>')
                    },
                    {
                        data: 'id', title: '', render: (data, type, row) =>
                            puedeEliminar ? '<button class="btn btn-danger btn-sm botonEliminar"><i class="fas fa-trash"></i></button>' : ''
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                createdRow: function (row, rowData) {
                    if (rowData.migrado) {
                        $(row).find('td').css('background-color', '#eec298');
                    }
                },
                drawCallback: function (settings) {

                    tablaControlAsistencia.find('button.botonCargarLista').click(function () {
                        const controlAsistencia = dtTablaControlAsistencia.row($(this).parents('tr')).data();

                        inputArchivoControlAsistencia.data().controlAsistenciaID = controlAsistencia.id;
                        inputArchivoControlAsistencia.click();
                    });

                    tablaControlAsistencia.find('button.botonDescargarLista').click(function () {
                        const controlAsistencia = dtTablaControlAsistencia.row($(this).parents('tr')).data();
                        descargarControlAsistencia(controlAsistencia.id);
                    });

                    tablaControlAsistencia.find('button.verLista').click(function () {
                        const controlAsistencia = dtTablaControlAsistencia.row($(this).parents('tr')).data();
                        const controlAsistenciaID = controlAsistencia.id;
                        cargarDatosControlAsistencia(controlAsistenciaID);
                    });

                    tablaControlAsistencia.find('button.botonDC3').off().click(function () {
                        const controlAsistencia = dtTablaControlAsistencia.row($(this).parents('tr')).data();
                        if (controlAsistencia) {
                            cargarAsistentesDC3(controlAsistencia);
                        }
                        modalDC3.data().id = controlAsistencia.id
                    });

                    tablaControlAsistencia.find('button.botonExamenes').click(function () {
                        const controlAsistencia = dtTablaControlAsistencia.row($(this).parents('tr')).data();
                        if (controlAsistencia) {
                            controlAsistencia.estatus == 2 ? botonGuardarExamenes.show() : botonGuardarExamenes.hide();
                            cargarAsistentes(controlAsistencia);
                        }
                    });

                    tablaControlAsistencia.find('button.reporteControlAsistencia').click(function () {
                        const controlAsistencia = dtTablaControlAsistencia.row($(this).parents('tr')).data();
                        verReporte(controlAsistencia.id);
                    });

                    tablaControlAsistencia.on('click', '.botonEliminar', function () {
                        const controlAsistencia = dtTablaControlAsistencia.row($(this).parents('tr')).data();

                        if (controlAsistencia == null || controlAsistencia.id <= 0) {
                            return;
                        }

                        AlertaAceptarRechazarNormal(
                            'Confirmar eliminación',
                            `¿Está seguro de eliminar este control de asistencia? Eliminarlo provocará que toda la información vinculada también se elimine.`,
                            () => eliminarControlAsistencia(controlAsistencia.id))
                    });
                }
            });
        }

        function eliminarControlAsistencia(controlAsistenciaID) {

            $.post('/Administrativo/Capacitacion/EliminarControlAsistencia', { controlAsistenciaID })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Control de asistencia eliminado correctamente.`);
                        cargarListaControlAsistencia();
                        esEdit = false;
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`));

        }

        function verReporte(controlAsistenciaID) {
            $.blockUI({ message: 'Generando reporte...' });
            var path = `/Reportes/Vista.aspx?idReporte=168&controlAsistenciaID=${controlAsistenciaID}&isCRModal=${true}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function mostrarModalExamenes(asistentes, controlAsistencia) {

            inputExamenCC.val(controlAsistencia.cc);
            inputExamenFechaCurso.val(controlAsistencia.fecha);
            inputExamenNombreInstructor.val(controlAsistencia.instructor);
            inputExamenNombreCurso.val(controlAsistencia.curso);

            inputJefeDepartamento.val('');
            inputCoordinadorCSH.val('');
            inputSecretarioCSH.val('');
            inputGerenteProyecto.val('');

            dtTablaExamenes.clear().draw();
            dtTablaExamenes.rows.add(asistentes).draw();

            modalExamenes.data().clasificacion = controlAsistencia.clasificacion;


            if (controlAsistencia.clasificacion == 3 || controlAsistencia.clasificacion == 4) {
                tablaExamenes.find('tbody tr[role="row"] input.inputExamenArchvo').attr('disabled', true);
            }

            if (controlAsistencia.esExterno) {
                tablaExamenes.find('tbody tr[role="row"] button.botonDescargarExamen').attr('disabled', true);
            }

            // Agregar el onclick event a la columna de aprobación.
            tablaExamenes.find('thead th').last().unbind().click(() => {

                asistentes = tablaExamenes.find('tbody tr[role="row"] input.inputAproboExamen')
                    .toArray()
                    .forEach(input => input.checked = true);

            });

            modalExamenes.modal('show');
        }

        function mostrarModalDC3(asistentes, controlAsistencia) {
            inputExamenCCDC3.val(controlAsistencia.cc);
            inputExamenFechaCursoDC3.val(controlAsistencia.fecha);
            inputExamenNombreInstructorDC3.val(controlAsistencia.instructor);
            inputExamenNombreCursoDC3.val(controlAsistencia.curso);

            inputJefeDepartamentoDC3.val('');
            inputCoordinadorCSHDC3.val('');
            inputSecretarioCSHDC3.val('');
            inputGerenteProyectoDC3.val('');

            dtTablaDC3.clear().draw();
            dtTablaDC3.rows.add(asistentes).draw();

            modalDC3.data().clasificacion = controlAsistencia.clasificacion;

            // Agregar el onclick event a la columna de aprobación.
            tablaDC3.find('thead th').last().unbind().click(() => {
                asistentes = tablaDC3.find('tbody tr[role="row"] input.inputAproboDC3').toArray().forEach(input => input.checked = true);
            });

            modalDC3.modal('show');
        }

        function cargarAsistentes(controlAsistencia) {
            $.blockUI({ message: 'Cargando asistentes...' });
            $.get('/Administrativo/Capacitacion/CargarAsistentesCapacitacion', { controlAsistenciaID: controlAsistencia.id })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        mostrarModalExamenes(response.items, controlAsistencia);

                        if (response.listaAutorizantes.length > 0) {
                            colocarAutorizantesPorCC(response.listaAutorizantes);
                        }
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

        function cargarAsistentesDC3(controlAsistencia) {
            $.blockUI({ message: 'Cargando asistentes...' });
            $.get('/Administrativo/Capacitacion/CargarAsistentesCapacitacion', { controlAsistenciaID: controlAsistencia.id })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        mostrarModalDC3(response.items, controlAsistencia);

                        // if (response.listaAutorizantes.length > 0) {
                        //     colocarAutorizantesPorCC(response.listaAutorizantes);
                        // }
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

        function descargarControlAsistencia(controlAsistenciaID) {
            location.href = `DescargarListaControlAsistencia?controlAsistenciaID=${controlAsistenciaID}`;
        }

        function mostrarControlAsistencia(controlAsistencia) {
            inputDetalleCC.val(controlAsistencia.ccDesc);
            inputDetalleFechaCurso.val(controlAsistencia.fechaCapacitacion);
            inputDetalleDuracionCurso.val(controlAsistencia.duracion);

            inputDetalleClaveCurso.val(controlAsistencia.claveCurso);
            inputDetalleNombreCurso.val(controlAsistencia.nombreCurso);

            establecerInformacionLabelDetalleClaisificacion(controlAsistencia.claseSpan, controlAsistencia.clasificacion);

            inputDetalleEmpresaExterna.val(controlAsistencia.empresaExterna);
            inputDetalleNombreInstructor.val(controlAsistencia.nombreInstructor);

            inputDetalleLugarCurso.val(controlAsistencia.lugar);
            inputDetalleHorarioCurso.val(controlAsistencia.horario);

            modalDetallesControlAsistencia.data().objetivos = controlAsistencia.objetivos;
            modalDetallesControlAsistencia.data().temasPrincipales = controlAsistencia.temasPrincipales;
            modalDetallesControlAsistencia.data().referenciasNormativas = controlAsistencia.referenciasNormativas;
            modalDetallesControlAsistencia.data().notas = controlAsistencia.notas;

            dtTablaAsistentesDetalles.clear().draw();
            dtTablaAsistentesDetalles.rows.add(controlAsistencia.asistentes).draw();

            modalDetallesControlAsistencia.modal('show');
        }

        function cargarDatosControlAsistencia(controlAsistenciaID) {
            $.blockUI({ message: 'Cargando datos...' });
            $.get('/Administrativo/Capacitacion/CargarDatosControlAsistencia', { controlAsistenciaID })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        mostrarControlAsistencia(response.controlAsistencia);
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

        function initTablaListaAsistentes() {
            dtTablaAsistentes = tablaAsistentes.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                order: [[4, "asc"]],
                columns: [
                    { data: 'claveEmpleado', title: '# Empleado', render: (data, type, row) => `<p>${row.claveEmpleado}</p>` },
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'puesto', title: 'Puesto' },
                    { data: 'ccDesc', title: 'Centro de Costo' },
                    // {
                    //     data: 'aplicaAutorizacion', title: 'Aplica autorización', render: (data, type, row) =>
                    //         `<input type="checkbox" class="form-control aplicaAut" ${row.aplicaAutorizacion ? 'checked' : ''} ></input>`
                    // },
                    {
                        data: 'ccID', title: 'Eliminar', render: (data, type, row) =>
                            `<button class="btn btn-danger botonEliminarAsistente"><i class="fas fa-trash-alt"></i></button>`
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {

                    tablaAsistentes.find('.botonEliminarAsistente').unbind().click(function () {
                        dtTablaAsistentes.row($(this).closest('tr')).remove().draw();
                    });

                    // tablaAsistentes.find('input.aplicaAut').unbind().click(function () {
                    //     const thisRow = $(this).closest('tr');
                    //     const rowData = dtTablaAsistentes.row(thisRow).data();
                    //     rowData.aplicaAutorizacion = this.checked;
                    //     dtTablaAsistentes.row(thisRow).data(rowData).draw();
                    // });

                    // tablaAsistentes.find('thead th').last().prev().unbind().click(() => {
                    //     tablaAsistentes.find('tbody tr[role="row"] input.aplicaAut')
                    //         .toArray()
                    //         .forEach(input => {
                    //             const thisRow = $(input).closest('tr');
                    //             const rowData = dtTablaAsistentes.row(thisRow).data();
                    //             rowData.aplicaAutorizacion = true;
                    //             dtTablaAsistentes.row(thisRow).data(rowData).draw();
                    //         });
                    // });
                }
            });
        }

        function initTablaListaAsistentesDetalles() {
            dtTablaAsistentesDetalles = tablaAsistentesDetalles.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                // order: [[4, "asc"]],
                columns: [
                    { data: 'claveEmpleado', title: '# Empleado' },
                    { data: 'nombreEmpleado', title: 'Nombre' },
                    { data: 'puesto', title: 'Puesto' },
                    { data: 'ccDesc', title: 'Centro de Costo' },
                    // {
                    //     data: 'aplicaAutorizacion', title: 'Aplica autorización', render: (data, type, row) =>
                    //         `<input type="checkbox" disabled class="form-control" ${row.aplicaAutorizacion ? 'checked' : ''} ></input>`
                    // },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaExamenes() {
            //#region Columnas
            let columnas = [
                {
                    data: 'claveEmpleado', title: 'Empleado', render: (data, type, row) =>
                        `<p>${row.claveEmpleado} - ${row.nombreEmpleado}</p>`
                },
                { data: 'estatusDesc', title: 'Estatus' },
                { data: 'estatusAutorizacionDesc', title: 'Estatus Autorización' },
                {
                    data: 'tipoExamen', title: 'Examen Base', render: (data, type, row) => {
                        if (row.clasificacion != 3 && row.clasificacion != 4) {
                            return `
                            <p>${row.tipoExamen}</p>
                            <button tipoExamen=3 class="btn btn-primary botonDescargarExamen"><i class="fas fa-arrow-alt-circle-down"></i> Descargar</button>
                            `;
                        } else {
                            return `<p>N/A</p>`;
                        }
                    }
                }
            ];

            if (!_moduloCapacitacionOperativa) {
                columnas.push(
                    {
                        data: 'id', title: 'Examen Diagnóstico', render: (data, type, row) => {
                            if (row.estatus == 1) {
                                return `<input class=" form-control inputExamenArchvo inputExamenDiagnostico"  type="file" />`;
                            } else if (row.clasificacion != 3 && row.clasificacion != 4) {
                                return '<button tipoExamen=1 class="btn btn-primary botonDescargarExamen"><i class="fas fa-arrow-alt-circle-down"></i> Descargar</button>';
                            } else {
                                return `<p>N/A</p>`;
                            }
                        }
                    }
                );
            }

            columnas.push(
                {
                    data: 'id', title: 'Examen Final', render: (data, type, row) => {
                        if (row.estatus == 1) {
                            return `<input class=" form-control inputExamenArchvo inputExamenFinal"  type="file" />`;
                        } else if (row.clasificacion != 3 && row.clasificacion != 4) {
                            return '<button tipoExamen=2 class="btn btn-primary botonDescargarExamen"><i class="fas fa-arrow-alt-circle-down"></i> Descargar</button>';
                        }
                        else {
                            return `<p>N/A</p>`;
                        }
                    }
                },
                {
                    data: 'id', title: 'Aprobó (85%-100%)', render: (data, type, row) =>
                        `<input type="checkbox" class="form-control inputAproboExamen" ${row.estatus == 1 ? '' : 'disabled'} ${row.estatus == 2 ? 'checked' : ''} ></input>`
                },
                {
                    data: 'id', title: 'Calificación (0-100)', render: (data, type, row) =>
                        `<input type="number" class="form-control text-center inputCalificacion" ${row.estatus == 1 ? '' : `disabled value="${row.calificacion}"`}></input>`
                }
            );
            //#endregion

            dtTablaExamenes = tablaExamenes.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                // order: [[4, "asc"]],
                columns: columnas,
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {

                    tablaExamenes.find('input.inputExamenArchvo').unbind().change(function () {
                        this.files.length > 0 ? $(this).addClass('archivoCargado') : $(this).removeClass('archivoCargado');
                    });

                    tablaExamenes.find('button.botonDescargarExamen').unbind().click(function () {
                        const boton = $(this);
                        const tipoExamen = boton.attr('tipoExamen');
                        const controlAsistenciaDetalle = dtTablaExamenes.row(boton.parents('tr')).data();

                        descargarExamenAsistente(controlAsistenciaDetalle.id, tipoExamen);

                    });

                }
            });
        }

        function initTablaDC3() {
            dtTablaDC3 = tablaDC3.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                columns: [
                    {
                        data: 'claveEmpleado', title: 'Empleado',
                        render: (data, type, row) =>
                            `<p>${row.claveEmpleado} - ${row.nombreEmpleado}</p>`
                    },
                    { data: 'estatusDesc', title: 'Estatus' },
                    {
                        data: 'id', title: inputEmpresaActual.val() != 6 ? 'Archivo DC-3' : 'Archivo Certificado de Trabajo',
                        render: (data, type, row) => {
                            if (inputEmpresaActual.val() == 6) {
                                return `<button btn="btn btn-default" class="descargarCertificadoTrabajo"><i class="fas fa-file-pdf"></i></button>`
                            } else {
                                if (row.rutaDC3 == null) {
                                    return `<input class=" form-control inputDC3Archivo inputDC3" type="file" />`;
                                } else {
                                    return '<button class="btn btn-primary botonDescargarDC3"><i class="fas fa-arrow-alt-circle-down"></i> Descargar</button>';
                                }
                            }
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaDC3.on('click', '.descargarCertificadoTrabajo', function () {
                        let rowData = dtTablaDC3.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea visualizar el certificado de trabajo?', 'Confirmar', 'Cancelar', () => fncVisualizarCertificadoTrabajo());
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {
                    tablaDC3.find('input.inputDC3Archivo').unbind().change(function () {
                        this.files.length > 0 ? $(this).addClass('archivoCargado') : $(this).removeClass('archivoCargado');
                    });

                    tablaDC3.find('button.botonDescargarDC3').unbind().click(function () {
                        const boton = $(this);
                        const controlAsistenciaDetalle = dtTablaDC3.row(boton.parents('tr')).data();

                        descargarDC3(controlAsistenciaDetalle.id);
                    });
                }
            });
        }

        function fncVisualizarCertificadoTrabajo() {
            var path = `/Reportes/Vista.aspx?idReporte=286&id=${modalDC3.data().id}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function descargarExamenAsistente(controlAsistenciaDetalleID, tipoExamen) {
            location.href = `DescargarExamenAsistente?controlAsistenciaDetalleID=${controlAsistenciaDetalleID}&tipoExamen=${tipoExamen}`;
        }

        function descargarDC3(controlAsistenciaDetalleID) {
            location.href = `DescargarDC3?controlAsistenciaDetalleID=${controlAsistenciaDetalleID}`;
        }

        function colocarAutorizantesPorCC(listaAutorizantes) {
            listaAutorizantes.forEach(function (element) {
                switch (element.tipoPuesto) {
                    case 1:
                        inputJefeDepartamento.val(element.nombre);
                        inputJefeDepartamento.data().usuarioID = element.usuarioID;
                        break;
                    case 2:
                        inputCoordinadorCSH.val(element.nombre);
                        inputCoordinadorCSH.data().usuarioID = element.usuarioID;
                        break;
                    case 3:
                        inputSecretarioCSH.val(element.nombre);
                        inputSecretarioCSH.data().usuarioID = element.usuarioID;
                        break;
                    case 4:
                        inputGerenteProyecto.val(element.nombre);
                        inputGerenteProyecto.data().usuarioID = element.usuarioID;
                        break;
                    default:
                        AlertaGeneral(`Alerta`, `Tipo de puesto para autorizante inválido.`);
                        break;
                }
            });
        }

        selectRazonSocial.on('change', function () {
            let razonSocial = $(this).val();

            if (razonSocial != '') {
                let rfc = $(this).find('option:selected').attr('data-prefijo');

                inputRFC.val(rfc);
            } else {
                inputRFC.val('');
            }
        });

        function guardarCargaMasiva() {
            var request = new XMLHttpRequest();

            request.open("POST", "/Administrativo/Capacitacion/GuardarCargaMasivaControlAsistencia");
            request.send(formDataCargaMasiva());

            request.onload = function (response) {
                if (request.status == 200) {
                    let respuesta = JSON.parse(request.response);

                    modalCargaMasiva.modal('hide');
                    $('#inputFile').val('');

                    if (respuesta.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                    } else {
                        AlertaGeneral(`Alerta`, respuesta.message);
                    }

                } else {
                    AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                }
            };
        }

        function guardarMigracionEmpleado() {
            if (inputClaveEmpleadoMigracion.val() == '') {
                Alert2Warning('Debe capturar un empleado.');
                return;
            }

            if (selectCentroCostoMigracion.val() == '') {
                Alert2Warning('Debe seleccionar un centro de costo.');
                return;
            }

            axios.post('/Administrativo/Capacitacion/GuardarMigracionEmpleado', {
                claveEmpleado: +inputClaveEmpleadoMigracion.val(),
                cc: selectCentroCostoMigracion.val(),
                empresa: +selectCentroCostoMigracion.find('option:selected').attr('empresa')
            }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    modalMigrarEmpleado.modal('hide');
                    Alert2Exito('Se ha guardado la información.');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function formDataCargaMasiva() {
            let formData = new FormData();

            $.each(document.getElementById("inputFile").files, function (i, file) {
                formData.append("files[]", file);
            });

            return formData;
        }
    }

    $(() => Adminstrativo.Seguridad.Capacitacion.ControlAsistencia = new ControlAsistencia());
})();