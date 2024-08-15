(() => {
    $.namespace('EvaluacionSubcontratistas.AdministracionEvaluacion');
    AdministracionEvaluacion = function () {
        //#region Selectores
        const selectProyecto = $('#selectProyecto');
        const selectSubcontratista = $('#selectSubcontratista');
        const selectEstatus = $('#selectEstatus');
        const botonBuscar = $('#botonBuscar');
        const inputPendientesAsignar = $('#inputPendientesAsignar');
        const inputAsignadas = $('#inputAsignadas');
        const inputAutorizadas = $('#inputAutorizadas');
        const inputCumplimiento = $('#inputCumplimiento');
        const tablaEvaluaciones = $('#tablaEvaluaciones');
        const modalAsignacion = $('#modalAsignacion');
        const inputFechaInicial = $('#inputFechaInicial');
        const inputFechaFinal = $('#inputFechaFinal');
        const inputEspecialidad = $('#inputEspecialidad');
        const inputEstado = $('#inputEstado');
        const inputMunicipio = $('#inputMunicipio');
        const inputEvaluacionInicial = $('#inputEvaluacionInicial');
        const inputEvaluacionFinal = $('#inputEvaluacionFinal');
        const botonGuardarAsignacion = $('#botonGuardarAsignacion');
        const divEvaluaciones = $('#divEvaluaciones');
        const modalActualizarEvaluacion = $('#modalActualizarEvaluacion');
        const inputFechaEvaluacionActualizar = $('#inputFechaEvaluacionActualizar');
        const inputSolicitante = $('#inputSolicitante');
        const textareaMotivo = $('#textareaMotivo');
        const botonNotificarActualizacionEvaluacion = $('#botonNotificarActualizacionEvaluacion');
        const modalSoporte = $('#modalSoporte');
        const selectElemento = $('#selectElemento');
        const divRequerimientos = $('#divRequerimientos');
        const botonGuardarSoporte = $('#botonGuardarSoporte');
        const modalRetroalimentacion = $('#modalRetroalimentacion');
        const labelRequerimientoRetroalimentacion = $('#labelRequerimientoRetroalimentacion');
        const textareaComentario = $('#textareaComentario');
        const textareaPlanAccion = $('#textareaPlanAccion');
        const inputResponsable = $('#inputResponsable');
        const inputFechaCompromiso = $('#inputFechaCompromiso');
        const inputCalificacion = $('#inputCalificacion');
        const botonGuardarRetroalimentacion = $('#botonGuardarRetroalimentacion');
        const botonEstrellaRetroalimentacion = $('#botonEstrellaRetroalimentacion');
        const inputCalificacionGlobal = $('#inputCalificacionGlobal');
        const divGeneral = $('#divGeneral');
        const divCompromisos = $('#divCompromisos');
        const tablaCompromisos = $('#tablaCompromisos');
        const celdaTotalRequerimientosEvaluados = $('#celdaTotalRequerimientosEvaluados');
        const celdaRequerimientosAprobados = $('#celdaRequerimientosAprobados');
        const celdaRequerimientosNoAprobados = $('#celdaRequerimientosNoAprobados');
        const celdaTotalCompromisos = $('#celdaTotalCompromisos');
        const celdaPromedioCumplimiento = $('#celdaPromedioCumplimiento');
        const modalSeguimientoFirmas = $('#modalSeguimientoFirmas');
        const tituloModalSeguimientoFirmas = $('#tituloModalSeguimientoFirmas');
        const tablaFirmas = $('#tablaFirmas');
        const modalAutorizarCambioEvaluacion = $('#modalAutorizarCambioEvaluacion');
        const inputFechaEvaluacionAutorizar = $('#inputFechaEvaluacionAutorizar');
        const inputSolicitanteAutorizar = $('#inputSolicitanteAutorizar');
        const textareaMotivoAutorizar = $('#textareaMotivoAutorizar');
        const botonRechazarCambioEvaluacion = $('#botonRechazarCambioEvaluacion');
        const botonAutorizarCambioEvaluacion = $('#botonAutorizarCambioEvaluacion');
        const textareaComentarioAutorizante = $('#textareaComentarioAutorizante');
        const modalGraficas = $('#modalGraficas');
        //#endregion

        let dtEvaluaciones;
        let dtCompromisos;
        let dtFirmas;

        let _facultamientoUsuario = 0;

        const FACULTAMIENTOS = {
            NO_ASIGNADO: 0,
            ADMINISTRADOR_PMO: 1,
            ADMINISTRADOR: 2,
            EVALUADOR: 3,
            CONSULTA: 4
        };

        (function init() {
            $('.select2').select2();

            getFacultamientoUsuario();

            initTablaEvaluaciones();
            initTablaCompromisos();
            initTablaFirmas();

            selectProyecto.fillCombo('FillComboProyectos', null, false, null);
            selectSubcontratista.fillCombo('FillComboSubcontratistas', null, false, null);

            inputFechaInicial.datepicker({ format: 'dd/mm/yy', showAnim: 'slide' });
            inputFechaFinal.datepicker({ format: 'dd/mm/yy', showAnim: 'slide' });
            inputFechaEvaluacionActualizar.datepicker({ format: 'dd/mm/yy', showAnim: 'slide' });
            inputFechaCompromiso.datepicker({ format: 'dd/mm/yy', showAnim: 'slide' });

            inputSolicitante.getAutocompleteValid(setDatosSolicitante, verificarSolicitante, { porClave: false }, 'GetUsuariosAutocomplete');

            botonBuscar.click(cargarEvaluacionesSubcontratistas);
            botonGuardarAsignacion.click(guardarAsignacion);
            botonGuardarSoporte.click(guardarSoporte);
            botonGuardarRetroalimentacion.click(guardarRetroalimentacionEvaluador);
            botonNotificarActualizacionEvaluacion.click(guardarCambioEvaluacion);
            botonAutorizarCambioEvaluacion.click(function () { guardarAutorizacionCambioEvaluacion($(this).data().cambioEvaluacion_id, 2) });
            botonRechazarCambioEvaluacion.click(function () { guardarAutorizacionCambioEvaluacion($(this).data().cambioEvaluacion_id, 3) });
        })();

        $('#inputFechaInicial, #inputFechaFinal').on('change', function () {
            let fechaInicial = inputFechaInicial.val();
            let fechaFinal = inputFechaFinal.val();

            if (fechaInicial != '' && fechaFinal != '') {
                let diferencia = Math.floor(moment(fechaFinal, 'DD/MM/YYYY').diff(moment(fechaInicial, 'DD/MM/YYYY'), 'months', true));
                let evaluacionInicial = moment(moment(fechaInicial, 'DD/MM/YYYY').add(14, 'days')).format('DD/MM/YYYY');
                let evaluacionFinal = moment(moment(fechaFinal, 'DD/MM/YYYY').add(-15, 'days')).format('DD/MM/YYYY');
                let mesEvaluacionFinal = +moment(evaluacionFinal, 'DD/MM/YYYY').format('M');

                inputEvaluacionInicial.val(evaluacionInicial);
                inputEvaluacionFinal.val(evaluacionFinal);

                divEvaluaciones.empty();

                if (diferencia >= 2) {
                    let evaluacionPeriodica = moment(moment(evaluacionInicial, 'DD/MM/YYYY').add(15, 'days')).format('DD/MM/YYYY');
                    let mesEvaluacionPeriodica = +moment(evaluacionPeriodica, 'DD/MM/YYYY').format('M');

                    if (mesEvaluacionPeriodica < mesEvaluacionFinal) { //Se agregan las evaluaciones de meses anteriores al último.
                        agregarCampoFechaEvaluacion(evaluacionPeriodica, false, 0);
                    }

                    for (let i = 0; i < (diferencia - 1); i++) {
                        evaluacionPeriodica = moment(moment(evaluacionPeriodica, 'DD/MM/YYYY').add(30, 'days')).format('DD/MM/YYYY');
                        mesEvaluacionPeriodica = +moment(evaluacionPeriodica, 'DD/MM/YYYY').format('M');

                        if (mesEvaluacionPeriodica < mesEvaluacionFinal) { //Se agregan las evaluaciones de meses anteriores al último.
                            agregarCampoFechaEvaluacion(evaluacionPeriodica, false, 0);
                        }
                    }
                }
            }
        });

        selectElemento.on('change', function () {
            let elemento_id = +selectElemento.val();

            divRequerimientos.find('fieldset').css('display', 'none');

            if (elemento_id > 0) {
                divRequerimientos.find(`#fieldset_elemento_${elemento_id}`).show('slow');
            }
        });

        inputCalificacion.on('change', function () {
            calcularPonderacionRetroalimentacion();
        });

        modalSeguimientoFirmas.on('shown.bs.modal', function () {
            dtFirmas.columns.adjust().draw();
        });

        function getFacultamientoUsuario() {
            axios.post('GetFacultamientoUsuario').then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    _facultamientoUsuario = response.data.data;


                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function agregarCampoFechaEvaluacion(evaluacionPeriodica, editar, evaluacion_id) {
            divEvaluaciones.append(`
                <div class="divEvaluacion col-sm-3" style="display: none;">
                    <div class="input-group mrgTop">
                        <input class="form-control text-center inputFechaEvaluacion" value="${evaluacionPeriodica}" disabled>
                        <button class="btn btn-default botonActualizarEvaluacion" ${!editar ? 'disabled' : ''} evaluacion-id="${evaluacion_id}"><i class="fa fa-pen"></i></button>
                    </div>
                </div>
            `);

            //Se bindea el evento click al elemento recién creado.
            $('.botonActualizarEvaluacion').on('click', function () {
                inputFechaEvaluacionActualizar.val('');
                inputSolicitante.val('');
                inputSolicitante.data().usuario_id = null;
                textareaMotivo.val('');

                botonNotificarActualizacionEvaluacion.data().evaluacion_id = +$(this).attr('evaluacion-id');

                inputFechaEvaluacionActualizar.val($(this).closest('.input-group').find('.inputFechaEvaluacion').val());

                modalActualizarEvaluacion.modal('show');
                modalActualizarEvaluacion.css('z-index', 1501);
                $('.modal-backdrop:eq(1)').css('z-index', 1500);
            });

            $('#divEvaluaciones .divEvaluacion:last').show('slow');
        }

        function initTablaEvaluaciones() {
            dtEvaluaciones = tablaEvaluaciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    { data: 'numeroContrato', title: 'CONTRATO' },
                    { data: 'ccDesc', title: 'CC' },
                    { data: 'subcontratistaDesc', title: 'SUBCONTRATISTA' },
                    { data: 'periodoEvaluable', title: 'PERIODO EVALUABLE' },
                    { data: 'fechaEvaluacionString', title: 'FECHA DE EVALUACIÓN' },
                    { data: 'cargaSoportes', title: 'CARGA SOPORTES' },
                    {
                        title: 'EVALUACIÓN', render: function (data, type, row, meta) {
                            return `
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO || _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR ? '<button class="btn btn-xs btn-default botonAsignacionEvaluacion"><i class="fa fa-save"></i></button>' : ''}
                            `;
                        }
                    },
                    {
                        title: 'CAMBIO EVALUACIÓN', render: function (data, type, row, meta) {
                            return row.cambioEvaluacion_id > 0 ? `
                                <button class="btn btn-xs btn-danger botonAutorizarCambio" style="padding: 1px 9px;"><i class="fa fa-exclamation"></i></button>
                            ` : ``;
                        }
                    },
                    {
                        title: 'SEGUIMIENTO', render: function (data, type, row, meta) {
                            return `
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO || _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR ? '<button class="btn btn-xs btn-warning botonEditarEvaluacion"><i class="fa fa-save"></i></button>' : ''}
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO || _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR || _facultamientoUsuario == FACULTAMIENTOS.EVALUADOR ? '<button class="btn btn-xs btn-primary botonProcesoEvaluacion"><i class="fa fa-edit"></i></button>' : ''}
                                <button class="btn btn-xs btn-danger botonReporte"><i class="fa fa-file"></i></button>
                                ${(_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO || _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR) && row.flagGestionFirmas ? '<button class="btn btn-xs btn-success botonGestionFirmas"><i class="fa fa-check"></i></button>' : ''}
                            `;
                        }, width: '10%'
                    },
                    {
                        title: 'GRÁFICA', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-primary botonGrafica"><i class="fa fa-chart-bar"></i></button>`;
                        }
                    },
                    { data: 'estatusFirmas', title: 'ESTATUS DE FIRMAS' },
                    {
                        title: 'REPORTE', render: function (data, type, row, meta) {
                            return `
                                <button class="btn btn-xs btn-danger botonReporte"><i class="fa fa-file"></i></button>
                                <button class="btn btn-xs btn-primary botonSeguimientoFirmas"><i class="fa fa-list"></i></button>
                            `;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaEvaluaciones.on('click', '.botonAsignacionEvaluacion', function () {
                        let rowData = dtEvaluaciones.row($(this).closest("tr")).data();

                        botonGuardarAsignacion.show();
                        botonGuardarAsignacion.data().contrato_id = rowData.contrato_id;
                        botonGuardarAsignacion.data().asignacion_id = rowData.asignacion_id;

                        inputFechaInicial.val('');
                        inputFechaFinal.val('');
                        inputEspecialidad.val('');
                        inputEstado.val('');
                        inputMunicipio.val('');
                        inputEvaluacionInicial.val('');
                        inputEvaluacionFinal.val('');

                        divEvaluaciones.empty();

                        inputFechaInicial.attr('disabled', false);
                        inputFechaFinal.attr('disabled', false);

                        axios.post('GetContratoInformacion', { contrato_id: rowData.contrato_id }).then(response => {
                            let { success, datos, message } = response.data;

                            if (success) {
                                inputFechaInicial.val(response.data.data.fechaInicialString);
                                inputFechaFinal.val(response.data.data.fechaFinalString);
                                inputEspecialidad.val(response.data.data.especialidad);
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));

                        modalAsignacion.modal('show');
                    });

                    tablaEvaluaciones.on('click', '.botonEditarEvaluacion', function () {
                        let rowData = dtEvaluaciones.row($(this).closest("tr")).data();

                        botonGuardarAsignacion.hide();
                        botonNotificarActualizacionEvaluacion.data().contrato_id = rowData.contrato_id;
                        botonNotificarActualizacionEvaluacion.data().asignacion_id = rowData.asignacion_id;

                        inputFechaInicial.val('');
                        inputFechaFinal.val('');
                        inputEspecialidad.val('');
                        inputEstado.val('');
                        inputMunicipio.val('');
                        inputEvaluacionInicial.val('');
                        inputEvaluacionFinal.val('');

                        divEvaluaciones.empty();

                        inputFechaInicial.attr('disabled', true);
                        inputFechaFinal.attr('disabled', true);

                        axios.post('GetContratoInformacion', { contrato_id: rowData.contrato_id }).then(response => {
                            let { success, datos, message } = response.data;

                            if (success) {
                                inputEspecialidad.val(response.data.data.especialidad);
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));

                        axios.post('GetAsignacionContrato', { asignacion_id: rowData.asignacion_id }).then(response => {
                            let { success, datos, message } = response.data;

                            if (success) {
                                inputFechaInicial.val(response.data.data.fechaInicialString);
                                inputFechaFinal.val(response.data.data.fechaFinalString);
                                inputEvaluacionInicial.val(response.data.data.evaluaciones.filter(x => x.tipo == 1)[0].fechaString);
                                inputEvaluacionFinal.val(response.data.data.evaluaciones.filter(x => x.tipo == 3)[0].fechaString);

                                let listaEvaluacionesPeriodicas = response.data.data.evaluaciones.filter(x => x.tipo == 2);

                                listaEvaluacionesPeriodicas.forEach(x => {
                                    agregarCampoFechaEvaluacion(x.fechaString, true, x.id);
                                });

                                modalAsignacion.modal('show');
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    });

                    tablaEvaluaciones.on('click', '.botonProcesoEvaluacion', function () {
                        let rowData = dtEvaluaciones.row($(this).closest('tr')).data();

                        botonGuardarRetroalimentacion.data({
                            contrato_id: rowData.contrato_id,
                            evaluacion_id: rowData.evaluacion_id,
                            subcontratista_id: rowData.subcontratista_id
                        });

                        axios.post('GetElementosEvaluacion', { contrato_id: rowData.contrato_id, evaluacion_id: rowData.evaluacion_id }).then(response => {
                            let { success, datos, message } = response.data;

                            if (success) {
                                divRequerimientos.empty();
                                cargarElementosEvaluacion(response.data.data, response.data.combo);

                                botonGuardarSoporte.data().subcontratista_id = rowData.subcontratista_id;

                                modalSoporte.modal('show');
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    });

                    tablaEvaluaciones.on('click', '.botonGestionFirmas', function () {
                        let rowData = dtEvaluaciones.row($(this).closest('tr')).data();

                        Alert2AccionConfirmar('Atención', '¿Desea enviar la evaluación a gestión de firmas? Se notificará al firmante.', 'Confirmar', 'Cancelar', () => {
                            enviarGestionFirmas(rowData.evaluacion_id, rowData.contrato_id, rowData.subcontratista_id)
                        });
                    });

                    tablaEvaluaciones.on('click', '.botonSeguimientoFirmas', function () {
                        let rowData = dtEvaluaciones.row($(this).closest('tr')).data();

                        axios.post('GetSeguimientoFirmas', { evaluacion_id: rowData.evaluacion_id, contrato_id: rowData.contrato_id }).then(response => {
                            let { success, datos, message } = response.data;

                            if (success) {
                                AddRows(tablaFirmas, response.data.data);
                                tituloModalSeguimientoFirmas.text('Seguimiento de Firmas - ' + rowData.numeroContrato);
                                modalSeguimientoFirmas.modal('show');
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    });

                    tablaEvaluaciones.on('click', '.botonAutorizarCambio', function () {
                        let rowData = dtEvaluaciones.row($(this).closest("tr")).data();

                        axios.post('GetCambioEvaluacion', { cambioEvaluacion_id: rowData.cambioEvaluacion_id }).then(response => {
                            let { success, datos, message } = response.data;

                            if (success) {
                                inputFechaEvaluacionAutorizar.val(response.data.data.fechaNuevaString);
                                inputSolicitanteAutorizar.val(response.data.data.usuarioSolicitanteNombre);
                                textareaMotivoAutorizar.val(response.data.data.motivoCambio);

                                botonAutorizarCambioEvaluacion.data().cambioEvaluacion_id = rowData.cambioEvaluacion_id;
                                botonRechazarCambioEvaluacion.data().cambioEvaluacion_id = rowData.cambioEvaluacion_id;

                                modalAutorizarCambioEvaluacion.modal('show');
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    });

                    tablaEvaluaciones.on('click', '.botonGrafica', function () {
                        let rowData = dtEvaluaciones.row($(this).closest('tr')).data();

                        axios.post('CargarGraficasSubcontratista', { cc: rowData.cc, subcontratista_id: rowData.subcontratista_id, contrato_id: rowData.contrato_id, estatus: rowData.estatusEvaluacion })
                            .then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    initChartCumplimientoCargaSoportes(response.data.chartCumplimientoCargaSoportes);
                                    initChartHistoricoCargaSoportes(response.data.chartHistoricoCargaSoportes);
                                    modalGraficas.modal('show');
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    });

                    tablaEvaluaciones.on('click', '.botonReporte', function () {
                        let rowData = dtEvaluaciones.row($(this).closest('tr')).data();

                        $("#report").attr("src", `/Reportes/Vista.aspx?idReporte=249&evaluacion_id=${rowData.evaluacion_id}`);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            openCRModal();
                        };
                    });
                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function initTablaCompromisos() {
            dtCompromisos = tablaCompromisos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    { data: 'ccDesc', title: 'CC' },
                    { data: 'subcontratistaDesc', title: 'SUBCONTRATISTA' },
                    { data: 'fechaEvaluacionString', title: 'FECHA DE EVALUACIÓN' },
                    { data: 'comentarioEvaluacion', title: 'COMENTARIO' },
                    { data: 'planAccion', title: 'PLAN DE ACCIÓN' },
                    { data: 'responsable', title: 'RESPONSABLE' },
                    { data: 'fechaCompromisoString', title: 'FECHA COMPROMISO' },
                    {
                        data: 'calificacion', title: 'CALIFICACIÓN COMPROMISO', render: function (data, type, row, meta) {
                            return row.estatusEvidencia > 1 ? data : '';
                        }
                    },
                    {
                        title: 'EVALUAR', render: function (data, type, row, meta) {
                            return `
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO || _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR || _facultamientoUsuario == FACULTAMIENTOS.EVALUADOR ? '<button class="btn btn-xs btn-primary botonProcesoEvaluacion"><i class="fa fa-edit"></i></button>' : ''}
                            `;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaCompromisos.on('click', '.botonProcesoEvaluacion', function () {
                        let rowData = dtCompromisos.row($(this).closest('tr')).data();

                        botonGuardarRetroalimentacion.data({
                            contrato_id: rowData.contrato_id,
                            evaluacion_id: rowData.evaluacion_id,
                            subcontratista_id: rowData.subcontratista_id
                        });

                        axios.post('GetElementosEvaluacion', { contrato_id: rowData.contrato_id, evaluacion_id: rowData.evaluacion_id }).then(response => {
                            let { success, datos, message } = response.data;

                            if (success) {
                                divRequerimientos.empty();
                                cargarElementosEvaluacion(response.data.data, response.data.combo);

                                botonGuardarSoporte.data().subcontratista_id = rowData.subcontratista_id;

                                modalSoporte.modal('show');
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    });
                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function initTablaFirmas() {
            dtFirmas = tablaFirmas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    { data: 'tipoDesc', title: 'TIPO FIRMA' },
                    { data: 'nombre', title: 'NOMBRE' },
                    { data: 'fechaAutorizacionString', title: 'FECHA AUTORIZACIÓN' },
                    { data: 'estadoFirmaDesc', title: 'ESTADO DE FIRMA' },
                    {
                        title: 'AUTORIZAR', render: function (data, type, row, meta) {
                            return row.flagPuedeFirmar ? `
                                <div class="divArchivo" style="display: inline-block;">
                                    <label for="inputFile_${row.firma_id}" class="btn btn-xs btn-default iconoInputFile">
                                        <i class="fa fa-file"></i>
                                        <i class="fa fa-check" style="display: none;"></i>
                                    </label>
                                    <input id="inputFile_${row.firma_id}" type="file" class="inputArchivoFirma" style="display:none;">
                                </div>
                                <button class="btn btn-xs btn-primary botonFirmar"><i class="fa fa-arrow-right"></i></button>
                            ` : ``;
                        }
                    },
                    {
                        title: 'RECHAZAR', render: function (data, type, row, meta) {
                            return row.flagPuedeRechazar ? '<button class="btn btn-xs btn-danger botonRechazar"><i class="fa fa-times"></i></button>' : '';
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaFirmas.on('click', '.botonFirmar', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtFirmas.row(row).data();
                        let archivoFirma = $(row).find('.inputArchivoFirma')[0].files[0];

                        if (archivoFirma == undefined) {
                            Alert2Warning('Debe capturar un archivo de firma para autorizar la evaluación.');
                            return;
                        }

                        Alert2AccionConfirmar('Atención', '¿Desea autorizar la evaluación?', 'Confirmar', 'Cancelar', () => {
                            autorizarEvaluacion(rowData.firma_id, archivoFirma);
                        });
                    });

                    tablaFirmas.on('input', '.inputArchivoFirma', function () {
                        let labelInput = $(this).closest('.divArchivo').find('.iconoInputFile');
                        let iconoFile = labelInput.find('i.fa-file');
                        let iconoCheck = labelInput.find('i.fa-check');
                        let archivoCargado = document.getElementById($(this).attr('id')).files[0];

                        if (archivoCargado != undefined) {
                            iconoFile.css('display', 'none');
                            iconoCheck.css('display', 'inline-block');
                            labelInput.addClass('btn-success').removeClass('btn-default');
                        } else {
                            iconoFile.css('display', 'inline-block');
                            iconoCheck.css('display', 'none');
                            labelInput.addClass('btn-default').removeClass('btn-success');
                        }
                    });

                    tablaFirmas.on('click', '.botonRechazar', function () {
                        let rowData = dtFirmas.row($(this).closest('tr')).data();

                        Alert2AccionConfirmar('Atención', '¿Desea rechazar la autorización del subcontratista?', 'Confirmar', 'Cancelar', () => {
                            rechazarEvaluacion(rowData.firma_id);
                        });
                    });
                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function cargarEvaluacionesSubcontratistas() {
            inputPendientesAsignar.val('');
            inputAsignadas.val('');
            inputAutorizadas.val('');
            inputCumplimiento.val('');

            let estatus = +selectEstatus.val();

            if (estatus == 0) {
                Alert2Warning('Debe seleccionar un estatus.');
                return;
            }

            axios.post('CargarEvaluacionesSubcontratistas', { cc: selectProyecto.val(), subcontratista_id: +selectSubcontratista.val(), estatus }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    celdaTotalRequerimientosEvaluados.text('');
                    celdaRequerimientosAprobados.text('');
                    celdaRequerimientosNoAprobados.text('');
                    celdaTotalCompromisos.text('');
                    celdaPromedioCumplimiento.text('');

                    switch (estatus) {
                        case 1: //PENDIENTES POR ASIGNAR
                            AddRows(tablaEvaluaciones, response.data.data);

                            inputPendientesAsignar.val(response.data.pendientesAsignar);
                            inputAsignadas.val(response.data.asignadas);
                            inputAutorizadas.val(response.data.autorizadas);
                            inputCumplimiento.val(response.data.cumplimiento);

                            divGeneral.show('slow', () => { $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust(); });
                            divCompromisos.hide();
                            dtEvaluaciones.column(4).visible(false);
                            dtEvaluaciones.column(5).visible(false);
                            dtEvaluaciones.column(6).visible(true);
                            dtEvaluaciones.column(7).visible(false);
                            dtEvaluaciones.column(8).visible(false);
                            dtEvaluaciones.column(9).visible(false);
                            dtEvaluaciones.column(10).visible(false);
                            dtEvaluaciones.column(11).visible(false);
                            break;
                        case 2: //EVALUACIÓN ASIGNADA
                            AddRows(tablaEvaluaciones, response.data.data);

                            inputPendientesAsignar.val(response.data.pendientesAsignar);
                            inputAsignadas.val(response.data.asignadas);
                            inputAutorizadas.val(response.data.autorizadas);
                            inputCumplimiento.val(response.data.cumplimiento);

                            divGeneral.show('slow', () => { $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust(); });
                            divCompromisos.hide();
                            dtEvaluaciones.column(4).visible(true);
                            dtEvaluaciones.column(5).visible(true);
                            dtEvaluaciones.column(6).visible(false);
                            dtEvaluaciones.column(7).visible(true);
                            dtEvaluaciones.column(8).visible(true);
                            dtEvaluaciones.column(9).visible(true);
                            dtEvaluaciones.column(10).visible(false);
                            dtEvaluaciones.column(11).visible(false);
                            break;
                        case 3: //ESTATUS DE EVALUACIÓN
                            AddRows(tablaEvaluaciones, response.data.data);

                            inputPendientesAsignar.val(response.data.pendientesAsignar);
                            inputAsignadas.val(response.data.asignadas);
                            inputAutorizadas.val(response.data.autorizadas);
                            inputCumplimiento.val(response.data.cumplimiento);

                            divGeneral.show('slow', () => { $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust(); });
                            divCompromisos.hide();
                            dtEvaluaciones.column(4).visible(true);
                            dtEvaluaciones.column(5).visible(false);
                            dtEvaluaciones.column(6).visible(false);
                            dtEvaluaciones.column(7).visible(false);
                            dtEvaluaciones.column(8).visible(false);
                            dtEvaluaciones.column(9).visible(false);
                            dtEvaluaciones.column(10).visible(true);
                            dtEvaluaciones.column(11).visible(true);
                            break;
                        case 4: //COMPROMISOS
                            AddRows(tablaCompromisos, response.data.data);

                            celdaTotalRequerimientosEvaluados.text(response.data.totalRequerimientosEvaluados);
                            celdaRequerimientosAprobados.text(response.data.requerimientosAprobados);
                            celdaRequerimientosNoAprobados.text(response.data.requerimientosNoAprobados);
                            celdaTotalCompromisos.text(response.data.totalCompromisos);
                            celdaPromedioCumplimiento.text(response.data.promedioCumplimiento);

                            divCompromisos.show('slow', () => { $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust(); });
                            divGeneral.hide();
                            break;
                        default:
                            Alert2Warning('Estatus no controlado para las columnas.');
                            break;
                    }
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarAsignacion() {
            let asignacion = {
                contrato_id: botonGuardarAsignacion.data().contrato_id,
                fechaInicial: inputFechaInicial.val(),
                fechaFinal: inputFechaFinal.val()
            };
            let evaluaciones = [];

            evaluaciones.push({ fecha: inputEvaluacionInicial.val(), tipo: 1 });

            divEvaluaciones.find('.inputFechaEvaluacion').each(function (index, element) {
                evaluaciones.push({ fecha: $(element).val(), tipo: 2 });
            });

            evaluaciones.push({ fecha: inputEvaluacionFinal.val(), tipo: 3 });

            axios.post('GuardarAsignacionEvaluacion', { asignacion, evaluaciones })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información');
                        modalAsignacion.modal('hide');
                        botonBuscar.click();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function setDatosSolicitante(e, ui) {
            inputSolicitante.data().usuario_id = ui.item.id;
            inputSolicitante.val(ui.item.nombre);
        }

        function verificarSolicitante(e, ui) {
            if (ui.item == null) {
                inputSolicitante.val('');
                inputSolicitante.data().usuario_id = null;
            }
        }

        function cargarElementosEvaluacion(data, combo) {
            selectElemento.fillCombo({ items: combo }, null, false, null);
            let listaPromediosElementos = [];

            data.forEach(elemento => {
                divRequerimientos.append(`
                    <fieldset id="fieldset_elemento_${elemento.elemento_id}" class="fieldset-custm mrgTop" style="display: none;">
                        <legend id="legendElemento" class="legend-custm">${elemento.descripcion}</legend>
                    </fieldset>
                `);

                let ultimoFieldsetElemento = divRequerimientos.find('fieldset:last');
                let listaCalificaciones = [];

                elemento.requerimientos.forEach(requerimiento => {
                    listaCalificaciones.push(requerimiento.calificacionUltimaEvidencia);

                    //#region Concatenar string HTML
                    let estrellasPonderacionString = getPonderacionString(requerimiento.ponderacionUltimaEvidencia);

                    let ultimaEvidenciaInicial = null;

                    if (requerimiento.evidencias.filter((x) => x.tipo == 1).length > 0) {
                        ultimaEvidenciaInicial = requerimiento.evidencias.filter((x) => x.tipo == 1)[requerimiento.evidencias.filter((x) => x.tipo == 1).length - 1];
                    }

                    let stringArchivoEvidencia = `
                        <div class="divArchivo" style="display: inline-block;">
                            <label for="inputFileInicial_${requerimiento.evaluacion_id}_${requerimiento.requerimiento_id}" class="inputs pointer btn iconoInputFile" style="margin-top: 10px; border-radius: 20px;" ${ultimaEvidenciaInicial == null ? 'disabled' : ''}>
                                <i class="fa fa-file-alt fa-6x"></i>
                                <i class="fa fa-check fa-6x" style="display: none;"></i>
                            </label>
                            <input id="inputFileInicial_${requerimiento.evaluacion_id}_${requerimiento.requerimiento_id}" class="inputArchivoRequerimiento" evidencia-id="${ultimaEvidenciaInicial != null ? ultimaEvidenciaInicial.evidencia_id : 0}" evaluacion-id="${requerimiento.evaluacion_id}" requerimiento-id="${requerimiento.requerimiento_id}" style="display:none;">
                            <label class="label" style="display: block; color: #333;">${ultimaEvidenciaInicial == null ? 'Ningún Archivo Cargado' : '&nbsp;'}</label>
                        </div>
                    `;

                    let ultimaEvidenciaCompromiso = null;

                    if (requerimiento.evidencias.filter((x) => x.tipo == 2).length > 0) {
                        ultimaEvidenciaCompromiso = requerimiento.evidencias.filter((x) => x.tipo == 2)[requerimiento.evidencias.filter((x) => x.tipo == 2).length - 1];
                    }

                    let stringArchivoEvidenciaCompromiso = `
                        <div class="divArchivo" style="display: ${requerimiento.estatusUltimaEvidenciaInicial == 3 ? 'inline-block;' : 'none;'}">
                            <label for="inputFileCompromiso_${requerimiento.evaluacion_id}_${requerimiento.requerimiento_id}" class="inputs pointer btn iconoInputFile" style="margin-top: 10px; border-radius: 20px;" ${ultimaEvidenciaCompromiso == null ? 'disabled' : ''}>
                                <i class="fa fa-file-alt fa-6x"></i>
                                <i class="fa fa-check fa-6x" style="display: none;"></i>
                            </label>
                            <input id="inputFileCompromiso_${requerimiento.evaluacion_id}_${requerimiento.requerimiento_id}" class="inputArchivoRequerimiento" evidencia-id="${ultimaEvidenciaCompromiso != null ? ultimaEvidenciaCompromiso.evidencia_id : 0}" evaluacion-id="${requerimiento.evaluacion_id}" requerimiento-id="${requerimiento.requerimiento_id}" style="display:none;">
                            <label class="label" style="display: block; color: #333;">${ultimaEvidenciaCompromiso == null ? 'COMPROMISO' : '&nbsp;'}</label>
                        </div>
                    `;

                    ultimoFieldsetElemento.append(`
                        <div class="col-sm-6">
                            <div class="panel-group">
                                <div class="panel panel-principal${requerimiento.estatusUltimaEvidencia == 3 ? '-rojo' : ''}">
                                    <div class="panel-heading panel-naranja">
                                        <div class="panel-title" style="font-size: 15px; overflow: hidden;">${requerimiento.descripcion}</div>
                                    </div>
                                    <div class="panel-collapse collapse in" aria-expanded="true" style="">
                                        <div class="panel-body p-primary">
                                            <div class="row text-center">
                                                ${stringArchivoEvidencia}
                                                ${stringArchivoEvidenciaCompromiso}
                                            </div>
                                        </div>
                                        <button id="botonEvaluacion_${requerimiento.evaluacion_id}_${requerimiento.requerimiento_id}" class="btn ${requerimiento.estatusUltimaEvidencia == 3 ? 'botonRojo' : 'botonNaranja'} botonEvaluacion" style="width: 100%; text-align: left;">
                                            <i class="fa fa-list"></i>&nbsp;Evaluación
                                            ${requerimiento.calificacionUltimaEvidencia > 0 ? `
                                            <div class="pull-right" style="background-color: #fff; border-radius: 4px;">
                                                &nbsp;
                                                <label style="color: #333;">${requerimiento.calificacionUltimaEvidencia}</label>&nbsp;${estrellasPonderacionString}
                                            </div>
                                            ` : ``}
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `);
                    //#endregion

                    //#region Data Botón Evaluación
                    let dataBotonEvaluacion = {
                        evaluacion_id: requerimiento.evaluacion_id,
                        requerimiento_id: requerimiento.requerimiento_id,
                        descripcion: requerimiento.descripcion
                    };

                    if (requerimiento.evidencias.length > 0) {
                        let ultimaEvidencia = requerimiento.evidencias[requerimiento.evidencias.length - 1];

                        dataBotonEvaluacion.evidencia_id = ultimaEvidencia.evidencia_id;
                        dataBotonEvaluacion.comentarioEvaluacion = ultimaEvidencia.comentarioEvaluacion;
                        dataBotonEvaluacion.calificacion = ultimaEvidencia.calificacion;
                        dataBotonEvaluacion.ponderacion = ultimaEvidencia.ponderacion;
                        dataBotonEvaluacion.planAccion = ultimaEvidencia.planAccion;
                        dataBotonEvaluacion.responsable = ultimaEvidencia.responsable;
                        dataBotonEvaluacion.fechaCompromiso = ultimaEvidencia.fechaCompromisoString;
                    } else {
                        dataBotonEvaluacion.evidencia_id = 0;
                        dataBotonEvaluacion.comentarioEvaluacion = '';
                        dataBotonEvaluacion.calificacion = '';
                        dataBotonEvaluacion.ponderacion = '';
                        dataBotonEvaluacion.planAccion = '';
                        dataBotonEvaluacion.responsable = '';
                        dataBotonEvaluacion.fechaCompromiso = '';
                    }

                    $(`#botonEvaluacion_${requerimiento.evaluacion_id}_${requerimiento.requerimiento_id}`).data(dataBotonEvaluacion);
                    //#endregion

                    //#region Bindear Eventos
                    $('input.inputArchivoRequerimiento').on('input', function () {
                        let labelInput = $(this).closest('.divArchivo').find('.iconoInputFile');
                        let labelTexto = $(this).closest('.divArchivo').find('.label');
                        let iconoFile = labelInput.find('i.fa-file-alt');
                        let iconoCheck = labelInput.find('i.fa-check');
                        let archivoCargado = document.getElementById($(this).attr('id')).files[0];

                        if (archivoCargado != undefined) {
                            iconoFile.css('display', 'none');
                            iconoCheck.css('display', 'inline-block');
                            labelTexto.text('Archivo Cargado'); // labelTexto.text(archivoCargado.name);
                        } else {
                            iconoFile.css('display', 'inline-block');
                            iconoCheck.css('display', 'none');
                            labelTexto.text('Ningún Archivo Cargado');
                        }
                    });

                    $('.botonEvaluacion').on('click', function () {
                        let databoton = $(this).data();
                        let evidencia_id = databoton.evidencia_id;
                        let evaluacion_id = databoton.evaluacion_id;
                        let requerimiento_id = databoton.requerimiento_id;
                        let descripcion = databoton.descripcion;

                        textareaComentario.val(databoton.comentarioEvaluacion);
                        textareaPlanAccion.val(databoton.planAccion);
                        inputResponsable.val(databoton.responsable);
                        inputFechaCompromiso.val(databoton.fechaCompromiso);
                        inputCalificacion.val(databoton.calificacion > 0 ? databoton.calificacion : '');

                        calcularPonderacionRetroalimentacion();

                        labelRequerimientoRetroalimentacion.text(descripcion);

                        botonGuardarRetroalimentacion.data({
                            evidencia_id, evaluacion_id, requerimiento_id
                        });

                        modalRetroalimentacion.modal('show');
                        modalRetroalimentacion.css('z-index', 1501);
                        $('.modal-backdrop:eq(1)').css('z-index', 1500);
                    });

                    $('label.inputs').click(function () {
                        let evidencia_id = +$(this).parent().find('input.inputArchivoRequerimiento').attr('evidencia-id')
                        let link = document.createElement("button");
                        link.download = 'GetArchivoEvidencia?id=' + evidencia_id;
                        link.href = 'GetArchivoEvidencia?id=' + evidencia_id;
                        link.click();
                        location.href = 'GetArchivoEvidencia?id=' + evidencia_id;
                    });

                    $('label.inputs').click(function (e) {
                        e.preventDefault();
                    });
                    //#endregion
                });

                let promedioCalificaciones = +((listaCalificaciones.reduce((a, b) => a + b) / listaCalificaciones.length).toFixed(2));

                listaPromediosElementos.push(promedioCalificaciones);

                ultimoFieldsetElemento.append(`
                    <div class="col-sm-12">
                        <div class="input-group col-sm-6">
                            <span class="input-group-addon">Promedio Evaluación</span>
                            <input class="form-control text-center" value="${promedioCalificaciones}" disabled>
                        </div>
                    </div>
                `);
            });

            let promedioGlobal = +((listaPromediosElementos.reduce((a, b) => a + b) / listaPromediosElementos.length).toFixed(2));

            inputCalificacionGlobal.text('Calificación Global ' + promedioGlobal);
        }

        function calcularPonderacionRetroalimentacion() {
            botonEstrellaRetroalimentacion.empty();

            let calificacion = +inputCalificacion.val();

            if (calificacion >= 0 && calificacion <= 25) {
                botonEstrellaRetroalimentacion.append(`
                    Ponderación&nbsp;&nbsp;&nbsp;
                    <i class="fa fa-star" style="color: #FA0101;"></i>
                `);
            } else if (calificacion >= 26 && calificacion <= 50) {
                botonEstrellaRetroalimentacion.append(`
                    Ponderación&nbsp;&nbsp;&nbsp;
                    <i class="fa fa-star" style="color: #FA8001;"></i>
                    <i class="fa fa-star" style="color: #FA8001;"></i>
                `);
            } else if (calificacion >= 51 && calificacion <= 75) {
                botonEstrellaRetroalimentacion.append(`
                    Ponderación&nbsp;&nbsp;&nbsp;
                    <i class="fa fa-star" style="color: #FAFF01;"></i>
                    <i class="fa fa-star" style="color: #FAFF01;"></i>
                    <i class="fa fa-star" style="color: #FAFF01;"></i>
                `);
            } else if (calificacion >= 76 && calificacion <= 90) {
                botonEstrellaRetroalimentacion.append(`
                    Ponderación&nbsp;&nbsp;&nbsp;
                    <i class="fa fa-star" style="color: #018001;"></i>
                    <i class="fa fa-star" style="color: #018001;"></i>
                    <i class="fa fa-star" style="color: #018001;"></i>
                    <i class="fa fa-star" style="color: #018001;"></i>
                `);
            } else if (calificacion >= 91 && calificacion <= 100) {
                botonEstrellaRetroalimentacion.append(`
                    Ponderación&nbsp;&nbsp;&nbsp;
                    <i class="fa fa-star" style="color: #0180FF;"></i>
                    <i class="fa fa-star" style="color: #0180FF;"></i>
                    <i class="fa fa-star" style="color: #0180FF;"></i>
                    <i class="fa fa-star" style="color: #0180FF;"></i>
                    <i class="fa fa-star" style="color: #0180FF;"></i>
                `);
            } else {
                botonEstrellaRetroalimentacion.append(`Ponderación&nbsp;&nbsp;&nbsp;`);
            }
        }

        function getPonderacionString(ponderacion) {
            let color = '';

            switch (ponderacion) {
                case 1:
                    color = '#FA0101';
                    break;
                case 2:
                    color = '#FA8001';
                    break;
                case 3:
                    color = '#FAFF01';
                    break;
                case 4:
                    color = '#018001';
                    break;
                case 5:
                    color = '#0180FF';
                    break;
                default:
                    color = 'white';
                    break;
            }

            let estrellasPonderacionString = ``;

            for (let i = 0; i < ponderacion; i++) {
                estrellasPonderacionString += `<i class="fa fa-star" style="color: ${color};"></i>&nbsp;`;
            }

            return estrellasPonderacionString;
        }

        function guardarSoporte() {
            let listaInputFile = $('.inputArchivoRequerimiento');
            let listaArchivosCargados = [];
            let listaEvidencias = [];
            const data = new FormData();

            listaInputFile.toArray().forEach(input => {
                if ($(input)[0].files.length > 0) {
                    let evaluacion_id = +$(input).attr('evaluacion-id');
                    let requerimiento_id = +$(input).attr('requerimiento-id');

                    listaEvidencias.push({ evaluacion_id, requerimiento_id });

                    data.append('archivos', $(input)[0].files[0]);
                    listaArchivosCargados.push($(input)[0].files[0]);
                }
            });

            if (listaInputFile.length > listaArchivosCargados.length) {
                Alert2Warning('Debe capturar todos los archivos para los elementos y sus requerimientos.');
                return;
            }

            data.append('listaEvidencias', JSON.stringify(listaEvidencias));
            data.append('subcontratista_id', botonGuardarSoporte.data().subcontratista_id);

            axios.post('GuardarSoporteEvaluacion', data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    modalSoporte.modal('hide');
                    Alert2Exito('Se ha guardado la información.');
                    botonBuscar.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarRetroalimentacionEvaluador() {
            let evidencia = {
                id: botonGuardarRetroalimentacion.data().evidencia_id,
                comentarioEvaluacion: textareaComentario.val(),
                ponderacion: botonEstrellaRetroalimentacion.find('i').length,
                calificacion: +inputCalificacion.val()
            };

            axios.post('GuardarRetroalimentacionEvaluador', { evidencia }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información.');
                    modalRetroalimentacion.modal('hide');
                    // modalSoporte.modal('hide');

                    axios.post('GetElementosEvaluacion', { contrato_id: botonGuardarRetroalimentacion.data().contrato_id, evaluacion_id: botonGuardarRetroalimentacion.data().evaluacion_id }).then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            divRequerimientos.empty();
                            cargarElementosEvaluacion(response.data.data, response.data.combo);

                            botonGuardarSoporte.data().subcontratista_id = botonGuardarRetroalimentacion.data().subcontratista_id;

                            modalSoporte.modal('show');
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function enviarGestionFirmas(evaluacion_id, contrato_id, subcontratista_id) {
            axios.post('EnviarGestionFirmas', { evaluacion_id, contrato_id, subcontratista_id }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha enviado la evaluación a gestión de firmas.');
                    botonBuscar.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function autorizarEvaluacion(firma_id, archivoFirma) {
            const data = new FormData();

            data.append('archivoFirma', archivoFirma);
            data.append('firma_id', firma_id);

            axios.post('AutorizarEvaluacion', data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    modalSeguimientoFirmas.modal('hide');
                    Alert2Exito('Se ha guardado la información.');
                    botonBuscar.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function rechazarEvaluacion(firma_id) {
            axios.post('RechazarEvaluacion', { firma_id }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    modalSeguimientoFirmas.modal('hide');
                    Alert2Exito('Se ha guardado la información.');
                    botonBuscar.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarCambioEvaluacion() {
            let cambio = {
                evaluacion_id: botonNotificarActualizacionEvaluacion.data().evaluacion_id,
                fechaNueva: inputFechaEvaluacionActualizar.val(),
                usuarioSolicitante_id: inputSolicitante.data().usuario_id,
                motivoCambio: textareaMotivo.val(),
            };

            axios.post('GuardarCambioEvaluacion', { cambio }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha notificado a los administradores PMO.');
                    modalActualizarEvaluacion.modal('hide');
                    modalAsignacion.modal('hide');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarAutorizacionCambioEvaluacion(cambioEvaluacion_id, estatus) {
            let cambio = {
                id: cambioEvaluacion_id,
                estatus,
                comentarioAutorizante: textareaComentarioAutorizante.val()
            };

            axios.post('GuardarAutorizacionCambioEvaluacion', { cambio }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información.');
                    modalAutorizarCambioEvaluacion.modal('hide');
                    botonBuscar.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initChartCumplimientoCargaSoportes(datos) {
            Highcharts.chart('chartCumplimientoCargaSoportes', {
                chart: {
                    zoomType: 'xy'
                },
                lang: highChartsDicEsp,
                title: { text: 'Cumplimiento Carga Soportes' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: [
                    { // Primary yAxis
                        labels: { format: '{value}' },
                        title: { text: '' },
                        allowDecimals: false,
                        min: 0,
                        max: datos.serie3[0] //El valor óptimo es el valor máximo
                    },
                    { // Secondary yAxis
                        labels: { format: '{value}' },
                        title: { text: '' },
                        allowDecimals: false,
                        opposite: true,
                        min: 0,
                        max: datos.serie3[0] //El valor óptimo es el valor máximo
                    }
                ],
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, type: 'column', yAxis: 1, data: datos.serie1, color: 'green' },
                    { name: datos.serie2Descripcion, type: 'column', yAxis: 1, data: datos.serie2, color: 'red' },
                    { name: datos.serie3Descripcion, type: 'line', data: datos.serie3, color: 'orange' }
                ],
                credits: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function initChartHistoricoCargaSoportes(datos) {
            let serie1Date = datos.serie1String.map(function (x) {
                return x != '' ? Date.UTC(
                    moment(x, 'DD/MM/YYYY').format('YYYY'),
                    moment(x, 'DD/MM/YYYY').format('M'),
                    moment(x, 'DD/MM/YYYY').format('D'),
                    0, 0, 0) : null;
            });
            let serie2Date = datos.serie2String.map(function (x) {
                return x != '' ? Date.UTC(
                    moment(x, 'DD/MM/YYYY').format('YYYY'),
                    moment(x, 'DD/MM/YYYY').format('M'),
                    moment(x, 'DD/MM/YYYY').format('D'),
                    0, 0, 0) : null;
            });

            Highcharts.chart('chartHistoricoCargaSoportes', {
                chart: {
                    zoomType: 'xy'
                },
                lang: highChartsDicEsp,
                title: { text: 'Historico Carga Soportes' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: [
                    { // Primary yAxis
                        // labels: { format: '{value}' },
                        title: { text: '' },
                        type: 'datetime',
                        labels: {
                            formatter: function () {
                                return Highcharts.dateFormat('%d/%m/%Y', this.value);
                            }
                        }
                    }
                ],
                tooltip: {
                    pointFormatter: function () {
                        let point = this;
                        let series = point.series;
                        let formatoFecha = moment(new Date(point.y)).format('DD/MM/YYYY');

                        return `${series.name}: <b>${formatoFecha}</b>`
                    }
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, type: 'line', data: serie1Date, color: 'orange' },
                    { name: datos.serie2Descripcion, type: 'line', data: serie2Date, color: 'black' }
                ],
                credits: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => EvaluacionSubcontratistas.AdministracionEvaluacion = new AdministracionEvaluacion())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();








(() => {
    $.namespace('subContratistas.EvaluacionSubcontratista');

    //#region CONST
    const cboProyecto = $('#cboProyecto');
    const cboSubContratista = $('#cboSubContratista');
    const contenido = $('#contenido');
    const contenido23 = $('#contenido23');
    const tblAutorizacion = $('#tblAutorizacion');
    let dtAutorizacion;
    const btnBuscar = $('#btnBuscar');
    const mdlComentario = $('#mdlComentario');
    const btnGuardar = $('#btnGuardar');

    const inpComentario = $('#inpComentario');
    const inpEvaluacion = $('#inpEvaluacion');
    const inpPlanDeAccion = $('#inpPlanDeAccion');
    const inpResponsable = $('#inpResponsable');
    const inpFechaCompromiso = $('#inpFechaCompromiso');

    const mdlPreguntarPrimero = $('#mdlPreguntarPrimero');
    const containerPreguntarPrimero = $('#containerPreguntarPrimero');
    const btnGuardarPreguntarPrimero = $('#btnGuardarPreguntarPrimero');
    const cboEstatus = $('#cboEstatus');
    const mdlFormulario = $('#mdlFormulario');
    const txtNombreEvaluacion = $('#txtNombreEvaluacion');

    const inpCorreo = $('#inpCorreo');

    const btnGuardarModal = $('#btnGuardarModal');
    const txtUsuario = $('#txtUsuario');
    const txtFechaPeriodo = $('#txtFechaPeriodo');
    const txtTituloClick = $('#txtTituloClick');
    const mdlGraficasPorSubcontratista = $('#mdlGraficasPorSubcontratista');
    const mdlPlantillas = $('#mdlPlantillas');
    var TipoUSuario = 0;
    let datosAGuardar;
    let dtPromedio;
    let dtAsginacion;
    var srowData = {};
    let Estatus = [
        { val: 0, text: 'PENDIENTES POR ASIGNAR' },
        { val: 2, text: 'EVALUACIÓN ASIGNADA' },
        { val: 3, text: 'AUTORIZADAS' },
        { val: 4, text: 'HISTORIAL' }
    ]
    const divicionesxPlantilla = $('#divicionesxPlantilla');
    const contenedorEstatus = $('#contenedorEstatus');
    let tipoUsuario = 0;
    //#endregion

    EvaluacionSubcontratista = function () {
        let init = () => {
            inpEvaluacion.css('font-weight', 'bold');
            fillCombos();
            fncButtons();
            cargarcboEstatus();
            cargarTipoUsuario();

            $(`#ContenidoDiviciones`).remove();
            $(`#ConenidoArchivos`).remove();

            $("body").on("click", ".inpFillMunicipios", function () {
                if ($(this).val() > 0) {
                    $("#inpMunicipio").fillCombo("FillCboMunicipios", { idEstado: $(this).val() }, false);
                }
            });

            $("#cboProyecto").on("change", function () {
                fncFocus("cboProyecto");
            });

            $("#cboSubContratista").on("change", function () {
                fncFocus("cboSubContratista");
            });

            $("#cboEstatus").on("change", function () {
                fncFocus("cboEstatus");
            });
        }
        init();
    }

    function fncFocus(obj) {
        if (obj != "") {
            setTimeout(() => $(`#${obj}`).focus(), 50);
        }
    }

    function getParametrosGrafica() {
        let parametros = {

        };
        return parametros;
    }
    function ObtenerGraficaDeEvaluacionPorCentroDeCosto(params) {
        let parametros = getParametrosGrafica();
        axios.post('ObtenerGraficaDeEvaluacionPorCentroDeCosto', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {

                }
            });
    }
    function cargarcboEstatus() {
        let elemento = ``;
        Estatus.forEach(x => {
            elemento = `<option value="${x.val}">${x.text}</option>`;
            cboEstatus.append(elemento);
        });
    }
    function cargarTipoUsuario() {
        axios.post('cargarTipoUsuarios').then(response => {
            let { success, items } = response.data
            if (success) {
                // console.log(items);
                if (items == 10) {
                    TipoUSuario = items;
                    contenedorEstatus.css('display', 'none');
                    cboEstatus.val(2);
                    cboEstatus.trigger('change');
                } else {
                    TipoUSuario = items;
                    contenedorEstatus.css('display', 'block');
                    cboEstatus.val(0);
                    cboEstatus.trigger('change');
                }
            }
        }).catch(error => Alert2Error(error.message));
    }
    function fillCombos() {
        cboProyecto.fillCombo('getProyecto', false, null);
        // cboSubContratista.fillCombo('getSubContratistas?AreaCuenta=' + cboProyecto.val(), false, null);
    }
    function fncButtons() {
        btnBuscar.click(function () {
            $(`#ContenidoDiviciones`).remove();
            $(`#ConenidoArchivos`).remove();
            CargarConfiguracionVista();
        })
        btnGuardar.click(function () {
            GuardarEvaluacion($(this), btnGuardar);
        })
        cboProyecto.change(function (e) {
            // cboSubContratista.fillCombo('getSubContratistas?AreaCuenta=' + cboProyecto.val(), null, false, null);
        })
        btnGuardarPreguntarPrimero.click(function () {
            fncGuardarPreguntarPrimero();
        })
        inpEvaluacion.change(function () {
            coloresIo();
        })
        cboEstatus.change(function () {
            $(`#ContenidoDiviciones`).remove();
            $(`#ConenidoArchivos`).remove();

            CargarConfiguracionVista();
        })
    }
    function coloresIo() {
        // if (inpEvaluacion.val() == 25) {
        //     inpEvaluacion.css('background-color','#FA0101');
        //     inpEvaluacion.css('color','#fff');
        // }else if(inpEvaluacion.val() == 50){
        //     inpEvaluacion.css('background-color','#FA8001');
        //     inpEvaluacion.css('color','#fff');
        // }else if(inpEvaluacion.val() == 70){
        //     inpEvaluacion.css('background-color','#FAFF01');
        //     inpEvaluacion.css('color','#000');
        // }else if(inpEvaluacion.val() == 90){
        //     inpEvaluacion.css('background-color','#018001');
        //     inpEvaluacion.css('color','#fff');
        // }else if(inpEvaluacion.val() == 100){
        //     inpEvaluacion.css('background-color','#0180FF');
        //     inpEvaluacion.css('color','#fff');
        // }
    }
    function CargarConfiguracionVista() {
        axios.post('obtenerDivicionesEvaluador', {})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    fncGenerandoDivicion(items);
                }
            });
    }
    function fncGenerandoDivicion(lstDatos) {
        let html = `   <div class="col-lg-12 col-md-12 col-sm-12" id='ContenidoDiviciones'>
                            <div class=" row">
                                <div class="col-lg-12 col-md-12 col-sm-12" style="padding: 0;display:none;">
                                    <fieldset class="fieldset-custm" >
                                        <legend class="legend-custm">Menu : </legend>`;
        for (let index = 0; index < lstDatos.length; index++) {
            html += `
                                    <div class='col-md-3'>     
                                        <button class="btn btn-block btn-social btn-primary-menu boton-menu" id="${lstDatos[index].idbutton}" data-id="${lstDatos[index].id}" title="${lstDatos[index].toltips}">
                                            <i class="fa fa-clipboard-check icono-menu"></i> ${lstDatos[index].descripcion}
                                        </button>
                                    </div>    
                                        `;
        }

        html += ` 
                            </fieldset >
                            
                            </div>
                        </div>
                    </div>`;
        html += `<div class="col-lg-12 col-md-12 col-sm-12" id='ConenidoArchivos'>
                    <div class='row'>
                    `;
        for (let index = 0; index < lstDatos.length; index++) {
            if (lstDatos[index].descripcion != "Inicio") {
                html += `<div id="${lstDatos[index].idsection}"  class="col-md-12" style='padding: 0;'>
                            <!--<fieldset class="fieldset-custm">-->
                                <!--<legend class="legend-custm">${lstDatos[index].descripcion}<span id="lblErrorProyecto"></span></legend>-->
                                <div id="${lstDatos[index].idsection + lstDatos[index].id}" ></div>
                            <!--</fieldset>-->
                        </div>`;
            } else {
                html += `<div id="${lstDatos[index].idsection}"  class="col-md-12" style='padding: 0;'>
                            <div id="${lstDatos[index].idsection + lstDatos[index].id}" ></div>
                        </div>`;
            }
        }
        html += `
                    </div>
                
                </div>`;
        contenido.append(html);
        // FUNCIOM CLICK

        for (let index = 0; index < lstDatos.length; index++) {
            $(`#${lstDatos[index].idsection}`).css('display', 'none');
            $(`#${lstDatos[index].idbutton}`).click(function () {
                console.log("eee");
                $('#contenidoPromedio').remove();
                $('#contenidoPromedio2').remove();
                for (let i = 0; i < lstDatos.length; i++) {
                    if (lstDatos[index].idbutton == lstDatos[i].idbutton) {
                        $(`#${lstDatos[i].idsection}`).css('display', 'block');
                        btnBuscar.attr('data-id', lstDatos[i].id);
                        obtenerInputs(lstDatos[index].id, lstDatos[index].idsection + lstDatos[index].id);
                    } else {
                        $(`#${lstDatos[i].idsection}`).css('display', 'none');
                    }

                }
            })
        }
        //AQUI ANDO
        if (lstDatos.length != 0) {
            if (TipoUSuario == 10) {
                $(`#${lstDatos[0].idsection}`).css('display', 'block');
                btnBuscar.attr('data-id', lstDatos[0].id);
                obtenerInputs(lstDatos[0].id, lstDatos[0].idsection + lstDatos[0].id);
                $('#btnEvaluacion2').css('display', 'none');
            } else {
                $(`#${lstDatos[0].idsection}`).css('display', 'block');
                btnBuscar.attr('data-id', lstDatos[0].id);
                obtenerInputs(lstDatos[0].id, lstDatos[0].idsection + lstDatos[0].id);
            }
        }

    }
    function obtenerInputs(id, idsection) {
        axios.post('obtenerRequerimientos', { idDiv: id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    generarInputs(items, idsection);
                }
            });
    }
    function generarInputs(lstDatos, idsection) {
        $(`#${idsection}`).find('div').remove();
        if (idsection == 'sectionDiv22') {
            let html = `

                <div class='row'>
                    <div class="col-lg-12">
                        <table id='tblPromedios' class='table table-hover table-bordered table-striped compact'></table>
                    </div>
                </div><br>  

                <!-- GRAFICAS -->
                <!--<div class="row margin-top">
                    <div class="col-xs-6 col-md-6">
                        <div class="panel-group">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h4 class="panel-title text-center">
                                        <a data-toggle="collapse" href="#panel2">
                                            GRÁFICA DE EVALUACIONES PENDIENTES REALIZADAS POR CC
                                        </a>
                                    </h4>
                                </div>
                                <div id="panel2" class="panel-collapse collapse in">
                                    <div class="panel-body">
                                        <div class="col-lg-12">
                                            <div style="margin-left: auto; margin-right: auto">
                                                <figure class="highcharts-figure">
                                                    <div id="gpxporCentrosDeCosto" style="margin-left:auto; margin-right:auto;"></div>
                                                </figure>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>-->
                
                    <!--<div class="col-xs-6 col-md-6">
                        <div class="panel-group">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h4 class="panel-title text-center">
                                        <a data-toggle="collapse" href="#panel1">
                                            GRÁFICA DE SUBCONTRATISTAS POR INDICADORES
                                        </a>
                                    </h4>
                                </div>
                                <div id="panel1" class="panel-collapse collapse in">
                                    <div class="panel-body">
                                        <div class="col-lg-12">
                                            <div style="margin-left: auto; margin-right: auto">
                                            <figure class="highcharts-figure">
                                                <div id="GraficaReportesBarras" style="margin-left:auto; margin-right:auto;"></div>
                                            </figure>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>-->
                <!-- END: GRAFICAS -->`;

            $(`#${idsection}`).append(html);
            obtenerGraficaPastel();
            initDataTblPromedio();
            obtenerPromediosxSubcontratista();

        } else {
            let html = `<div class='row'>
                            <div class="col-lg-12">
                                <table id='tblAsignacionEvaluacion' class='table table-hover table-bordered table-striped compact' width='100%'>
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th colspan="3">EVALUACIONES</th>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th id='txtTexto'></th>
                                    </tr>
                                </thead>
                                </table>
                            </div>
                        </div>`;
            $(`#${idsection}`).append(html);

            inittblAsignacionEvaluacion();
            ObtenerContratistas();
        }
    }
    function GenerarInputModal(lstDatos, idsection, rowData, dn, btnEvaluacion) {
        $(`#${idsection}`).find('div').remove();
        // CargarArchivosXSubcontratista(lstDatos);
        if (lstDatos.length != 0) {
            let html = ` <div class='row'>`;
            html += `
                        <div class='container-fluid'>
                            <div class='row' style='overflow-y: scroll;'>
                                <div class='col-md-12'>
                             
                                    `;
            for (let index = 0; index < lstDatos.length; index++) {
                html += `
                                    <div class="col-md-6">
                                        <div class="panel-group">
                                            <div class="panel panel-primary">
                                                <div class="panel-heading" style="padding: 5px;">
                                                    <div class="panel-title" style="font-size: 11px;">
                                                        ${lstDatos[index].texto.substr(0, 80)}
                                                    </div>
                                                </div>
                                                <div class="panel-collapse collapse in" aria-expanded="true">
                                                    <div class="panel-body p-primary" style="padding: 5px;">
                                                        <div class="row text-center">
                                                            <label for="${lstDatos[index].inputFile}" id="${lstDatos[index].inputFile}"  class="inputs pointer btn" style="margin-top: 10px; border-radius: 20px;">
                                                                <i class="fa fa-file-alt fa-3x"></i>
                                                            </label>
                                                            <br>
                                                            <label class="label labelNombre" id="${lstDatos[index].lblInput}" class="botonDocumento" data-idevaluacion='${lstDatos[index].id}' style='color:black;'> Ningún Archivo Seleccionado</label>
                                                        </div>
                                                    </div>
                                                </div>
                                                    <button id="btnbutton${lstDatos[index].id}" data-id='${lstDatos[index].id}' class="btn btn-primary" style='width:100%;'><i class="fa fa-list"></i> Evaluacion</button>
                                            
                                            </div>
                                        </div>
                                    </div>
                                        `;

            }
            html += `
                                    </div>
                                </div>
                            </div>
                            `;

            html += `
                        </div>
                        <div class='row'>
                        <div class="col-xs-12 col-md-6" id='contenidoPromedio'>
                            <div class="input-group">
                                <span class="input-group-addon">Promedio evaluación: </span>
                                <input id="inpPromedio" class="form-control" disabled='true'/>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-6" id='contenidoPromedio2' style='    text-align-last: end;'>
                           <button id="btnEvaluado" class="btn btn-primary"><i class="far fa-envelope"></i>&nbsp;Notificar subcontratista</button>
                        </div>
                        </div>
                        `;

            $(`#${idsection}`).append(html);
        }
        for (let index = 0; index < lstDatos.length; index++) {

            $(`#${lstDatos[index].inputFile}`).change(function () {
                $(`#${lstDatos[index].lblInput}`).text($(this)[0].files[0].name);
            });

            $(`#btnbutton${lstDatos[index].id}`).click(function () {
                btnGuardar.removeData('idBtnRequerimiento');
                mdlComentario.modal('show');
                txtTituloClick.text(`${lstDatos[index].texto}`);
                btnBuscar.attr('data-id', $(`#btnbutton${lstDatos[index].id}`).attr('data-id'));
                btnGuardar.attr('data-idevaluacion', $(`#${lstDatos[index].lblInput}`).attr('data-idevaluacion'))
                btnGuardar.attr('data-idBtnRequerimiento', $(this).attr('id'));
                obtenerEvaluacionxReq();
            });

            $(`#${lstDatos[index].inputFile}`).click(function () {
                DescargarArchivos($(`#${lstDatos[index].lblInput}`).attr('data-idevaluacion'), +txtFechaPeriodo.attr("data-id"));
            });
        }

        cargandoModal(lstDatos, idsection, rowData);

        $('#btnEvaluado').attr('data-idevaluacion', rowData.id);
        $('#btnEvaluado').removeData('idbtnevaluacion');
        $('#btnEvaluado').attr('data-idbtnevaluacion', btnEvaluacion.attr('id'));

        $('#btnEvaluado').click(function () {
            Evaluado(lstDatos);
        });
    }

    function obtenerFuncion() {
        $('input.botonDocumento').change(function () {
            $(this).attr('archivosCambiados', 1);

            if ($(this)[0].files.length > 0) {
                visualizarArchivoCargado(this);

                if ($(this)[0].files.length > 1) {
                    $(this).closest('.panel-body').find('.labelNombre').text($(this)[0].files.length + ' archivos cargados.');
                } else {
                    $(this).closest('.panel-body').find('.labelNombre').text($(this)[0].files[0].name);
                }
            } else {
                visualizarArchivoNoCargado(this);

                $(this).closest('.panel-body').find('.labelNombre').text('Ningún Archivo Seleccionado');
            }
        });
    }
    function visualizarArchivoCargado(elemento) {
        $(elemento).closest('.panel-body').removeClass('p-primary');
        $(elemento).closest('.panel-body').addClass('p-success');
        $(elemento).closest('.panel-body').find('i').removeClass('fa-file-alt');
        $(elemento).closest('.panel-body').find('i').addClass('fa-check');

        $(elemento).closest('.panel').find('.panel-title').find('.botonVerArchivos').remove();
    }
    function visualizarArchivoNoCargado(elemento) {
        $(elemento).closest('.panel-body').addClass('p-primary');
        $(elemento).closest('.panel-body').removeClass('p-success');
        $(elemento).closest('.panel-body').find('i').addClass('fa-file-alt');
        $(elemento).closest('.panel-body').find('i').removeClass('fa-check');

        $(elemento).closest('.panel').find('.panel-title').find('.botonVerArchivos').remove();
    }
    function visualizarArchivoGuardado(elemento, archivos, tipoAnexo) {
        $(elemento).closest('.panel-body').removeClass('p-primary');
        $(elemento).closest('.panel-body').addClass('p-success');
        $(elemento).closest('.panel-body').find('i').removeClass('fa-file-alt');
        $(elemento).closest('.panel-body').find('i').addClass('fa-check');

        if (archivos.length > 1) {
            $(elemento).closest('.panel-body').find('.labelNombre').text(archivos);
        } else {
            let rutaSeccionada = archivos[0].rutaArchivo.split('\\');

            $(elemento).closest('.panel-body').find('.labelNombre').text(rutaSeccionada[rutaSeccionada.length - 1]);
        }

        // $(elemento).closest('.panel').find('.panel-title').append(`
        //     <button class="btn btn-xs btn-warning botonVerArchivos" tipoanexo="${tipoAnexo}"><i class="fa fa-align-justify"></i></button>
        // `);
    }
    function getparamsEvaluar(lstDatos) {
        let params = []
        for (let i = 0; i < lstDatos.length; i++) {
            let item = {
                id: $(`#${lstDatos[i].lblInput}`).attr('data-idevaluacion'),
                nombreSubcontratista: txtFechaPeriodo.attr("data-nombresubcontratista"),
                cc: txtFechaPeriodo.attr("data-cc"),
                strFechaInicial: txtFechaPeriodo.attr("data-fechainicial"),
                strFechaFinal: txtFechaPeriodo.attr("data-fechafinal")
            };
            params.push(item)
        }
        return params;
    }
    function Evaluado(lstDatos) {
        let parametros = getparamsEvaluar(lstDatos);

        let textoElemento = $(`#${$('#btnEvaluado').attr('data-idbtnevaluacion')}`).text().trim();

        swal({
            title: 'Alerta',
            text: `Se notificara al firmante que el elemento: ${textoElemento} fue evaluado. ¿Desea continuar?`,
            icon: 'warning',
            buttons: true,
            dangerMode: true,
            buttons: ['Cancelar', 'Notificar']
        }).then((aceptar) => {
            if (aceptar) {
                axios.post('EvaluarDetalle', { id: $('#btnEvaluado').attr('data-idevaluacion'), parametros: parametros }).catch(o_O => AlertaGeneral(o_O.message)).then(response => {
                    let { success, items } = response.data;
                    if (success) {
                        if (items.status == 2) {
                            let btnElemento = $(`#${$('#btnEvaluado').attr('data-idbtnevaluacion')}`);
                            if (!btnElemento.hasClass('p-success')) {
                                btnElemento.addClass('p-success');
                            }
                        }
                        Alert2Exito('Se ha notificado con éxito.');
                    }
                });
            }
        });
    }
    function DescargarArchivos(idDet, idEvaluacion) {
        if (idEvaluacion > 0) {
            location.href = `DescargarArchivo?p_idDet=${idDet}|${idEvaluacion}`;
        }
    }
    function getParameters(rowData) {
        let item = {
            cc: rowData.cc,
            idSubContratista: rowData.idSubContratista,
            tipoEvaluacion: btnBuscar.attr('data-idTipoEvaluacion'),
        }
        return item;
    }
    function CargarArchivosXSubcontratista(lstDatos, rowData) {
        let parametros = getParameters(rowData);
        axios.post('CargarArchivosXSubcontratista', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    obtenerPromegioEvaluacion(rowData)

                    for (let i = 0; i < items.length; i++) {
                        $(`#${lstDatos[i].lblInput}`).text(items[i].rutaArchivo);
                        items[i].rutaArchivo != "Ningún archivo seleccionado" ? visualizarArchivoGuardado($(`#${lstDatos[i].lblInput}`), items[i].rutaArchivo, i) : visualizarArchivoNoCargado($(`#${lstDatos[i].lblInput}`));
                        $(`#${lstDatos[i].lblInput}`).attr('data-idEvaluacion', items[i].id);

                        let btnElemento = $(`#${items[i].btnElemento}`);
                        if (items[i].estaEvaluado) {
                            // if (!btnElemento.hasClass('p-success')) {
                            //     btnElemento.addClass('p-success');
                            // }

                            let tieneSuccess = $(`#btnbutton${lstDatos[i].id}`).hasClass('p-success');
                            if (!tieneSuccess) {
                                $(`#btnbutton${lstDatos[i].id}`).addClass('p-success');
                            }
                        } else {
                            btnElemento.removeClass('p-success');
                            btnElemento.addClass('p-primary');
                        }
                    }
                }
            });
    }
    function obtenerValuesInputs(id) {
        axios.post('obtenerRequerimientos', { idDiv: id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    fncGuardar(items);
                }
            });
    }
    function CrearFormData(lstDatos) {
        let formData = new FormData();
        let idRow = 0;
        for (let index = 0; index < lstDatos.length; index++) {
            idRow++;
            formData.append("parametros[" + index + "][idEvaluacion]", $(`#${lstDatos[index].lblInput}`).attr('data-idEvaluacion'));
            formData.append("parametros[" + index + "][idRow]", idRow);
            formData.append("parametros[" + index + "][tipoEvaluacion]", btnBuscar.attr('data-id'));
            if (document.getElementById(`${lstDatos[index].inputFile}`).files[0] != null) {
                formData.append("parametros[" + index + "][Archivo]", document.getElementById(`${lstDatos[index].inputFile}`).files[0].name);
                formData.append("Archivo", document.getElementById(`${lstDatos[index].inputFile}`).files[0]);
            }

        }

        return formData;
    }
    function fncGuardar(lstDatos) {
        let parametros = CrearFormData(lstDatos);
        axios.post('addEditSubContratista', parametros, { headers: { 'Content-Type': 'multipart/form-data' } })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
            });
    }
    function AddRows(tbl, lst) {
        dtGestionCambios = tbl.DataTable();
        dtGestionCambios.clear().draw();
        dtGestionCambios.rows.add(lst).draw(false);
    }
    function parametrosGuardar() {
        let item = {
            id: btnGuardar.attr('data-idevaluacion'),
            Comentario: inpComentario.val(),
            Calificacion: obtenerCalificacion($('.starrr').attr('data-calificacion')),
            strFechaInicial: txtFechaPeriodo.attr("data-fechaInicial"),
            strFechaFinal: txtFechaPeriodo.attr("data-fechaFinal"),
            nombreSubcontratista: txtFechaPeriodo.attr("data-nombreSubcontratista"),
            cc: txtFechaPeriodo.attr("data-CC")
        };
        return item;
    }
    function obtenerCalificacion(item) {
        let numero = 0;
        if (item == 1) {
            return numero = 25;
        }
        if (item == 2) {
            return numero = 50;
        }
        if (item == 3) {
            return numero = 70;
        }
        if (item == 4) {
            return numero = 90;
        }
        if (item == 5) {
            return numero = 100;
        }
    }
    function obtenerEvaluacionxReq() {
        let parametros = parametrosGuardar()
        axios.post('obtenerEvaluacionxReq', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items != null) {
                        let item = items;
                        inpComentario.val(item.Comentario);
                        inpEvaluacion.val(item.Calificacion);
                        inpPlanDeAccion.val(item.planesDeAccion);
                        inpPlanDeAccion.attr('disabled', 'true');
                        inpResponsable.val(item.responsable);
                        inpResponsable.attr('disabled', 'true');
                        inpFechaCompromiso.val(item.fechaCompromiso);
                        inpFechaCompromiso.attr('disabled', 'true');

                        //#region ESTRELLAS
                        axios.post('getEstrellas', {})
                            .catch(o_O => AlertaGeneral(o_O.message))
                            .then(response => {
                                let { success, items } = response.data;
                                if (success) {
                                    let estrellas = items;

                                    $('.starrr').starrr({
                                        rating: 0,
                                        change: function (e, value) {


                                            var id = $(e.currentTarget).data("id");
                                            $(e.currentTarget).attr("data-calificacion", value);
                                            if (value <= 2) {
                                                $('[data-id="txtRespuesta' + id + '"]').show();
                                            }
                                            if (value > 2) {
                                                $('[data-id="txtRespuesta' + id + '"]').hide();

                                            }
                                            $(e.currentTarget).find('label').text('');
                                            let etiqueta = $(e.currentTarget).find('label');
                                            etiqueta.text('');
                                            if (value > 0) {
                                                $.each(estrellas, function (index, est) {
                                                    if (est.estrellas == value) {
                                                        etiqueta.text(est.descripcion);
                                                    }
                                                });
                                            } else {
                                                etiqueta.text('');
                                            }
                                        }
                                    });
                                    $('.starrr').find('label').remove();
                                    var etiqueta = document.createElement('label');
                                    etiqueta.style.marginLeft = '10px';
                                    $('.starrr:last').append(etiqueta);
                                    let estrella = $('.starrr').find('a')

                                    $(estrella[0]).removeClass();
                                    $(estrella[1]).removeClass();
                                    $(estrella[2]).removeClass();
                                    $(estrella[3]).removeClass();
                                    $(estrella[4]).removeClass();
                                    $(estrella[0]).addClass('far fa-star default');
                                    $(estrella[1]).addClass('far fa-star default');
                                    $(estrella[2]).addClass('far fa-star default');
                                    $(estrella[3]).addClass('far fa-star default');
                                    $(estrella[4]).addClass('far fa-star default');

                                    switch (item.Calificacion) {
                                        case 100:
                                            $('.starrr').attr('data-calificacion', 5)

                                            $.each(estrellas, function (index, est) {
                                                if (est.estrellas == 5) {
                                                    $(estrella[0]).removeClass();
                                                    $(estrella[1]).removeClass();
                                                    $(estrella[2]).removeClass();
                                                    $(estrella[3]).removeClass();
                                                    $(estrella[4]).removeClass();
                                                    $(estrella[0]).addClass('fas fa-star star-azul');
                                                    $(estrella[1]).addClass('fas fa-star star-azul');
                                                    $(estrella[2]).addClass('fas fa-star star-azul');
                                                    $(estrella[3]).addClass('fas fa-star star-azul');
                                                    $(estrella[4]).addClass('fas fa-star star-azul');
                                                    $('.starrr').find('label').text(est.descripcion);

                                                }
                                            });
                                            break;
                                        case 90:
                                            $('.starrr').attr('data-calificacion', 4)
                                            $.each(estrellas, function (index, est) {
                                                if (est.estrellas == 4) {
                                                    $(estrella[0]).removeClass();
                                                    $(estrella[1]).removeClass();
                                                    $(estrella[2]).removeClass();
                                                    $(estrella[3]).removeClass();
                                                    $(estrella[0]).addClass('fas fa-star star-verde');
                                                    $(estrella[1]).addClass('fas fa-star star-verde');
                                                    $(estrella[2]).addClass('fas fa-star star-verde');
                                                    $(estrella[3]).addClass('fas fa-star star-verde');
                                                    $(estrella[4]).removeClass();
                                                    $(estrella[4]).addClass('far fa-star default');
                                                    $('.starrr').find('label').text(est.descripcion);
                                                }
                                            });
                                            break;
                                        case 70:
                                            $('.starrr').attr('data-calificacion', 3)
                                            $.each(estrellas, function (index, est) {
                                                if (est.estrellas == 3) {
                                                    $(estrella[0]).removeClass();
                                                    $(estrella[1]).removeClass();
                                                    $(estrella[2]).removeClass();
                                                    $(estrella[0]).addClass('fas fa-star star-amarillo');
                                                    $(estrella[1]).addClass('fas fa-star star-amarillo');
                                                    $(estrella[2]).addClass('fas fa-star star-amarillo');
                                                    $(estrella[3]).removeClass();
                                                    $(estrella[4]).removeClass();
                                                    $(estrella[3]).addClass('far fa-star default');
                                                    $(estrella[4]).addClass('far fa-star default');

                                                    $('.starrr').find('label').text(est.descripcion);
                                                }
                                            });
                                            break;
                                        case 50:
                                            $('.starrr').attr('data-calificacion', 2)
                                            $.each(estrellas, function (index, est) {
                                                if (est.estrellas == 2) {
                                                    $(estrella[0]).removeClass();
                                                    $(estrella[1]).removeClass();
                                                    $(estrella[0]).addClass('fas fa-star star-naranja');
                                                    $(estrella[1]).addClass('fas fa-star star-naranja');
                                                    $(estrella[2]).removeClass();
                                                    $(estrella[3]).removeClass();
                                                    $(estrella[4]).removeClass();
                                                    $(estrella[2]).addClass('far fa-star default');
                                                    $(estrella[3]).addClass('far fa-star default');
                                                    $(estrella[4]).addClass('far fa-star default');

                                                    $('.starrr').find('label').text(est.descripcion);
                                                }
                                            });
                                            break;
                                        case 25:
                                            $('.starrr').attr('data-calificacion', 1)
                                            $.each(estrellas, function (index, est) {
                                                if (est.estrellas == 1) {
                                                    $(estrella[0]).removeClass();
                                                    $(estrella[0]).addClass('fas fa-star star-rojo');
                                                    $(estrella[1]).removeClass();
                                                    $(estrella[2]).removeClass();
                                                    $(estrella[3]).removeClass();
                                                    $(estrella[4]).removeClass();
                                                    $(estrella[1]).addClass('far fa-star default');
                                                    $(estrella[2]).addClass('far fa-star default');
                                                    $(estrella[3]).addClass('far fa-star default');
                                                    $(estrella[4]).addClass('far fa-star default');

                                                    $('.starrr').find('label').text(est.descripcion);
                                                }
                                            });
                                            break;
                                    }
                                }
                            });
                        //#endregion
                    }
                } else {
                    inpComentario.val("");
                    inpEvaluacion.val("");
                    inpPlanDeAccion.val("");
                    inpPlanDeAccion.attr('disabled', 'true');
                    inpResponsable.val("");
                    inpResponsable.attr('disabled', 'true');
                    inpFechaCompromiso.val("");
                    inpFechaCompromiso.attr('disabled', 'true');
                }
            });
    }
    function GuardarEvaluacion(btnRequerimiento) {
        let parametros = parametrosGuardar()
        axios.post('GuardarEvaluacion', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    var btnReq = btnGuardar.attr('data-idbtnrequerimiento');
                    // $('#' + btnReq).addClass('p-success');
                    if (items.status == 1) {
                        let botonVerdeRequerimiento = $(`#${btnRequerimiento.data('idbtnrequerimiento')}`);
                        // $(`btnbutton${btnRequerimiento.data('idbtnrequerimiento')}`).css("background-color", "green");
                        // if (!botonVerdeRequerimiento.hasClass('btn-success')) {
                        //     botonVerdeRequerimiento.addClass('btn-success');
                        // }
                        console.log(parametros);

                        if (items.elementoVerde) {
                            // botonVerdeRequerimiento.addClass("btn-success");
                            // let btnElemento = $(`#ContenidoDivicionesModal`).find(`button[data-id=${items.idElemento}]`);
                            $(`#btnEvaluacion${items.idElemento}`).css("background-color", "green");
                            // btnElemento.css("background-color", "green");
                        }

                        Alert2Exito(items.mensaje);
                        $('#mdlComentario').modal('hide');
                        obtenerPromegioEvaluacion(srowData);
                    } else if (items.status == 2) {
                        AlertaGeneral('Hubo un error', items.mensaje);
                    } else {
                        AlertaGeneral('Alerta', items.mensaje);
                    }
                }
            });
    }

    function ponerVerdeRequerimiento(seccion) {

    }

    function parametrosBusquedaPromedio(rowData) {
        let item = {
            // cc: rowData.descripcioncc,
            tipoEvaluacion: btnBuscar.attr('data-idTipoEvaluacion'),
            tipoEvaluacionDet: btnGuardar.attr("data-idevaluacion"),
            // idSubContratista: rowData.idSubContratista,
            id: txtFechaPeriodo.attr("data-id")
        };
        return item;
    }
    function obtenerPromegioEvaluacion(rowData) {
        let parametros = parametrosBusquedaPromedio(rowData);
        axios.post('obtenerPromegioEvaluacion', { objDTO: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    srowData = rowData;
                    $('#inpPromedio').val('')
                    $('#inpPromedio').val(items.promedio.toFixed(2))

                    $("#inpCalificacionGlobal").val("");
                    $("#inpCalificacionGlobal").val(items.promedioGlobal.toFixed(2));
                }
            });
    }
    function obtenerGraficaPastel() {
        let parametros = parametrosGuardar();


        axios.post('ObtenerGraficaDeBarras', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                // CrearGraficasPastel('GraficaReportesPastel', response.data, null, null);
                CrearGraficasPastel('GraficaReportesBarras', response.data, null, null);

            });
        axios.post('ObtenerGraficaDeEvaluacionPorCentroDeCosto', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                // CrearGraficasPastel('GraficaReportesPastel', response.data, null, null);
                gxpGraficaTotal('gpxporCentrosDeCosto', response.data, null, null);

            });
    }
    function obtenerGraficaBarras() {

    }
    function CrearGraficasPastel(gpxGraficaPastel, datos, callback, items) {
        Highcharts.chart(gpxGraficaPastel, {
            chart: {
                type: 'column'
            },
            title: {
                text: ''
            },
            accessibility: {
                announceNewData: {
                    enabled: true
                }
            },
            xAxis: {
                type: 'category'
            },
            yAxis: {
                title: {
                    text: 'Total'
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        // format: '{point.y:.1f}'
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
            },
            series: datos.gpxGraficaDeBarras,
            credits: { enabled: false }
        });
    }
    function gxpGraficaTotal(grafica, datos, callback, items) {
        Highcharts.chart(grafica, {
            chart: {
                type: 'column'
            },
            title: {
                text: ''
            },
            accessibility: {
                announceNewData: {
                    enabled: true
                }
            },
            xAxis: {
                type: 'category'
            },
            yAxis: {
                min: 0,
                max: datos.numMaximo,
                title: {
                    text: ''
                },
                labels: {
                    format: '{value}'
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        format: '{point.y %}'
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b><br/>'
            },
            series: datos.gpxGrafica,
            credits: { enabled: false }
        });
    }
    function initDataTblPromedio() {
        dtnombre = $('#tblPromedios').DataTable({
            destroy: true,
            paging: true,
            ordering: true,
            searching: true,
            bFilter: true,
            info: false,
            language: dtDicEsp,
            columns: [
                { data: 'cc', title: 'CC' },
                { data: 'responsable', title: 'SUBCONTRATISTA' },
                {
                    data: 'promedio', title: 'EVALUACIÓN',
                    render: (data, type, row, meta) => {
                        let html = `<center>${Math.round(data)}</center>`;
                        return html;
                    }
                },
                {
                    data: 'Indicador', title: 'INDICADOR', render: (data, type, row, meta) => {
                        let html = ``;
                        if (data == 1) {
                            html += `<center><i class="fas fa-star" style='color: #FA0101;'></i></center>`;
                        }
                        if (data == 2) {
                            html += `<center><i class="fas fa-star" style='color: #FA8001;'></i><i class="fas fa-star" style='color: #FA8001;'></i></center>`;
                        }
                        if (data == 3) {
                            html += `<center><i class="fas fa-star" style='color: #FAFF01;'></i><i class="fas fa-star" style='color: #FAFF01;'></i><i class="fas fa-star" style='color: #FAFF01;'></i></center>`;
                        }
                        if (data == 4) {
                            html += `<center><i class="fas fa-star" style='color: #018001;'></i><i class="fas fa-star" style='color: #018001;'></i><i class="fas fa-star" style='color: #018001;'></i><i class="fas fa-star" style='color: #018001;'></i></center>`;
                        }
                        if (data == 5) {
                            html += `<center><i class="fas fa-star" style='color: #0180FF;'></i><i class="fas fa-star" style='color: #0180FF;'></i><i class="fas fa-star" style='color: #0180FF;'></i><i class="fas fa-star" style='color: #0180FF;'></i><i class="fas fa-star" style='color: #0180FF;'></i></center>`;
                        }
                        return html;
                    }
                },
                {
                    render: (data, type, row, meta) => {
                        let html = ``;
                        html += `<button class='btn btn-primary btn-xs btnGrafica'><i class="fas fa-chart-bar"></i></button>`
                        return html;
                    }
                },
            ], initComplete: function (settings, json) {
                $('#tblPromedios').on("click", ".btnGrafica", function () {
                    let rowData = dtnombre.row($(this).closest('tr')).data();
                    //INIT COMPLETE
                    mdlGraficasPorSubcontratista.modal('show');
                    ObtenerGraficaDeEvaluacionPorDivisionElemento(rowData.id);
                    // ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(rowData.centroCostos,rowData.idSubContratista);
                    ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(rowData.idContrato);
                })
            },
            columnDefs: [
                { className: 'dt-center', 'targets': '_all' },
                //{ className: 'dt-body-center', targets: [0] },
                //{ className: 'dt-body-right', targets: [0] },
                //{ width: '5%', targets: [0] }
            ],
        });
    }
    function obtenerPromediosxSubcontratista() {
        let parametros = parametrosGuardar();
        axios.post('obtenerPromediosxSubcontratista', parametros)
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    AddRows($('#tblPromedios'), items);
                }
            });
    }
    function inittblAsignacionEvaluacion() {
        dtAsginacion = $('#tblAsignacionEvaluacion').DataTable({ //omaromar
            destroy: true,
            paging: true,
            ordering: false,
            searching: false,
            bFilter: true,
            info: false,
            scrollY: true,
            scrollX: true,
            language: dtDicEsp,
            columns: [
                {
                    data: 'id', title: 'GRÁFICAS', visible: false,
                    render: (data, type, row, meta) => {
                        return `<button class='btn btn-xs btn-primary btnGrafica' title='Ver contenido de gráficas.'><i class="fas fa-chart-bar"></i></button>`;
                    }
                },
                { data: 'id', title: 'id', visible: false },
                { data: 'numeroContrato', title: 'NO. CONTRATO' },
                { data: 'descripcioncc', title: 'CC' },
                { data: 'nombre', title: 'SUBCONTRATISTA' },
                //#region VERSIÓN ADAN 
                { data: 'direccion', title: 'DIRECCIÓN', visible: false },
                {
                    // data: 'evaluacionAnteriorid', title: 'ANTERIOR',
                    data: "fechaInicial", title: 'PERIODO EVALUABLE',
                    render: (data, type, row, meta) => {
                        //#region VERSIÓN ADAN
                        // let html = ``;
                        // if (row.status == 3) {
                        //     html = moment(row.fechaCreacion).format("YYYY-MM-D");
                        // } else {
                        //     if (data != 0) {
                        //         html = `<button class="btn btn-xs btn-danger descargarEvaluacionActualPdfAnterior"  title='Descargar Evaluacion Actual en PDF'> <i class="fas fa-file-pdf"></i></button>`;
                        //     } else {
                        //         html = ``;
                        //     }
                        // }
                        // return html;
                        //#endregion

                        let fechaInicial = moment(row.fechaInicial).format("DD/MM/YYYY");
                        let fechaFinal = moment(row.fechaFinal).format("DD/MM/YYYY");
                        if (fechaInicial == '01/01/0001') {
                            return "";
                        } else {
                            return `${fechaInicial} - ${fechaFinal}`;
                        }
                    }
                },
                {
                    data: 'evaluacionActual', title: 'ACTUAL', visible: false,
                    render: (data, type, row, meta) => {
                        let html = ``;
                        if (data != 0) {
                            html = `<button class="btn btn-xs btn-danger descargarEvaluacionActualPdf"  title='Descargar Evaluacion Actual en PDF'> <i class="fas fa-file-pdf"></i></button>`;
                        } else {
                            html = ``;
                        }
                        return html;
                    }
                },
                {
                    data: 'estatusAutorizacion', title: ' ',
                    render: (data, type, row, meta) => {
                        let html = ``;
                        if (data == 0) {
                            if (row.tipoUsuarioID == 1 || row.tipoUsuarioID == 2) {
                                if (row.statusVobo == false) {
                                    html = `<button class="btn btn-xs btn-primary GuardarAsignacion" title='Realizar guardado de la asginacion'><i class="fa fa-save"></i></button>`;
                                }
                                else {
                                    html = `<button class="btn btn-xs btn-primary verPlantilla" title='Revisar plantilla para evaluacion'><i class="far fa-file"></i></i></button>
                                        <button class="btn btn-xs btn-success AutorizarAsignacion" title='Asignar Evaluacion A SubContratista'><i class="fa fa-check"></i></button>`;
                                }
                            }
                        }
                        else if (data == 2) {
                            let fechaInicial = moment(row.fechaInicial).format("DD/MM/YYYY");
                            if (fechaInicial != "01/01/2000") {
                                if (row.evaluacionActiva) {
                                    if (row.tipoUsuarioID == 1 || row.tipoUsuarioID == 2) {
                                        html += `<button class="btn btn-xs btn-primary Evaluar" title='Realizar Evaluacion'><i class="far fa-edit"></i></button>&nbsp;`;
                                        if (row.tipoUsuario != 10) {
                                            html += `
                                        <button class="btn btn-xs btn-danger descargarEvaluacionActualPdf" title='Descargar Evaluacion Actual en PDF'><i class="fas fa-file-pdf"></i></button>&nbsp;
                                        <button class="btn btn-xs btn-success autorizarEvaluacion" title='Autorizar evaluacion '><i class="fa fa-check"></i></button>`;
                                        }
                                    }
                                }
                            } else {
                                html = "PENDIENTE: INDICAR PERIODO EVALUABLE EN CALENDARIO.";
                            }
                        } else {
                            html = `<button class="btn btn-xs btn-danger descargarEvaluacionActualPdf" title='Descargar Evaluacion Actual en PDF'><i class="fas fa-file-pdf"></i></button>`;
                        }
                        return html;
                    }
                },
                //#endregion
            ],
            initComplete: function (settings, json) {
                $('#tblAsignacionEvaluacion').on("click", ".verPlantilla", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    //INIT COMPLETE
                    mdlPlantillas.modal('show');
                    imprimirDivciones(rowData);
                });

                $('#tblAsignacionEvaluacion').on("click", ".btnGrafica", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    //INIT COMPLETE
                    mdlGraficasPorSubcontratista.modal('show');
                    ObtenerGraficaDeEvaluacionPorDivisionElemento(rowData.id);
                    ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(/*rowData.centroCostos,rowData.idSubContratista*/rowData.idContrato);
                });

                $('#tblAsignacionEvaluacion').on("click", ".Evaluar", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    mdlFormulario.modal('show');
                    let fechaInicial = moment(rowData.fechaInicial).format("DD/MM/YYYY");
                    let fechaFinal = moment(rowData.fechaFinal).format("DD/MM/YYYY");
                    inpCorreo.attr('data-idcorreo', rowData.emails);
                    txtUsuario.text(`Subcontratista a evaluar: ${rowData.nombre}`);
                    txtNombreEvaluacion.text(`Evaluación: ${rowData.nombreEvaluacion}`);

                    txtFechaPeriodo.text(`${fechaInicial} - ${fechaFinal}`);
                    txtFechaPeriodo.attr("data-fechaInicial", moment(rowData.fechaInicial).format("DD-MM-YYYY"));
                    txtFechaPeriodo.attr("data-fechaFinal", moment(rowData.fechaFinal).format("DD-MM-YYYY"));
                    txtFechaPeriodo.attr("data-nombreSubcontratista", rowData.nombre);
                    txtFechaPeriodo.attr("data-CC", rowData.descripcioncc);
                    txtFechaPeriodo.attr("data-id", rowData.id);
                    //moment(row.fechaCreacion).format("YYYY-MM-D");
                    $(`#ContenidoDivicionesModal`).remove();
                    $(`#ConenidoArchivosModal`).remove();
                    CargarConfiguracionVistaModal(rowData);
                    CargarColorVerdeDeNotificado(rowData);
                });

                $('#tblAsignacionEvaluacion').on("click", ".GuardarAsignacion", function () {
                    let strMensaje = "Asignar evaluación";
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    Swal.fire({
                        title: strMensaje,
                        icon: "warning",
                        type: 'question',
                        width: '35%',
                        html: `<br>
                                <fieldset class='fieldset-custm'>
                                    <legend class='legend-custm'><h5>Fecha de ejecución</h5></legend>
                                    <div class="row">
                                        <div class="col-lg-6">
                                            <label>Fecha inicial:</label>
                                            <input id="datepickerInicial" readonly class="form-control" style='background:white;'>
                                        </div>
                                        <div class="col-lg-6">
                                            <label>Fecha final:</label>
                                            <input id="datepickerFinal" readonly class="form-control" style='background:white;'>
                                        </div>
                                        <hr>
                                    </div>
                                </fieldset>

                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>Nombre de evaluación:</label>
                                        <input type="text" id="inppNombreEvaluacion" class="form-control" style='background:white;'>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>Servicio contratado:</label>
                                        <input type="text" id="inpServicioContratado" class="form-control" style='background:white;'>
                                    </div>
                                </div>
                                
                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>Estado:</label>
                                        <select id="inpEstado" readonly class="form-control inpFillMunicipios select2" style='background:white;'></select>
                                    </div>
                                </div>
                                
                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>Municipio:</label>
                                        <select id="inpMunicipio" readonly class="form-control select2" style='background:white;'></select>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>Cant. evaluaciones:</label>
                                        <input id="inpCantEvaluaciones" class="form-control" style='background:white;'>
                                    </div>
                                </div>
                                
                                <!--<label> Frecuencia de evaluación: </label><input type="number" min = "1" max = "50" id="inFreqNum" class="form-control" style='background:white;'>-->`,
                        customClass: 'swal2-overflow',
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCancelButton: true,
                        input: "text",
                        didOpen: function () {
                            $("#inpEstado").fillCombo("FillCboEstados", {}, false);

                            $('#datepickerInicial').datepicker({
                                dateFormat: 'yy/mm/dd'
                            });
                            $('#datepickerInicial').change(function () {

                                if ($('#datepickerInicial').val() == "") {
                                    $('.swal2-input').val('');
                                } else {
                                    $('.swal2-input').val('fechaInicial');
                                }

                                if ($('#datepickerFinal').val() == "") {
                                    $('.swal2-input').val('');
                                } else {
                                    $('.swal2-input').val('fechaInicial');
                                }
                            });

                            $('#datepickerFinal').datepicker({
                                dateFormat: 'yy/mm/dd'
                            });
                            $('#datepickerFinal').change(function () {
                                if ($('#datepickerFinal').val() == "") {
                                    $('.swal2-input').val('');
                                } else {
                                    $('.swal2-input').val('fechaInicial');
                                }
                                if ($('#datepickerInicial').val() == "") {
                                    $('.swal2-input').val('');
                                } else {
                                    $('.swal2-input').val('fechaInicial');
                                }
                            });
                        },
                        inputValidator: nombre => {

                            if (!nombre) {
                                return "Por favor seleccionar una fecha";
                            } else {
                                return undefined;
                            }
                        },
                    }).then(function (result) {
                        if (result.isConfirmed == true) {
                            GuardarAsignacion(rowData,
                                $('#datepickerInicial').val(),
                                $('#datepickerFinal').val(),
                                $('#inFreqNum').val(),
                                $('#inppNombreEvaluacion').val(),
                                $('#inpServicioContratado').val(),
                                rowData.idPlantilla,
                                $('#inpEstado').val(),
                                $('#inpMunicipio').val(),
                                $("#inpCantEvaluaciones").val());
                        } else {
                        }
                    });
                    $('.swal2-input').css('display', 'none');
                });

                $('#tblAsignacionEvaluacion').on("click", ".autorizarEvaluacion", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    // let strMensaje = "Correo a Notificar :  " + rowData.emails + " ¿Desea dar Vobo la evaluacion para mandar a Gestion Firmas?";

                    strMensaje = "¿Desea enviar la evaluación a gestión de firmas? Se notificara al firmante.";
                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "¡Cuidado!",
                        width: '35%',
                        showCancelButton: true,
                        html: `<h3>${strMensaje}</h3>`,
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.value) {
                            AutorizarEvaluacion(rowData);
                        }
                    });
                });

                $('#tblAsignacionEvaluacion').on("click", ".AutorizarAsignacion", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    // let strMensaje = "Correo a notificar:  " + rowData.emails + "¿Desea dar Vobo 1 la evaluación?";
                    let strMensaje = "Se notificara al firmante sobre la asignación de la evaluación.";
                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "¡Cuidado!",
                        width: '35%',
                        showCancelButton: true,
                        html: `<h3>${strMensaje}</h3>`,
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.value) {
                            AutorizarAsignacion(rowData);
                        }
                    });
                });

                // $('#tblAsignacionEvaluacion').on("click", ".preguntarPrimero", function () {
                //     let rowData = dtAsginacion.row($(this).closest('tr')).data();
                //     abrirModalPreguntarPrimero(rowData);
                // });
                $('#tblAsignacionEvaluacion').on("click", ".descargarEvaluacionActualPdfAnterior", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    var path = `/Reportes/Vista.aspx?idReporte=249&idAsignacion=${rowData.evaluacionAnteriorid}`;
                    $("#report").attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };
                });

                $('#tblAsignacionEvaluacion').on("click", ".descargarEvaluacionActual", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    axios.post('creaExcelito', { idAsignacion: rowData.idAsignacion })
                        .catch(o_O => AlertaGeneral(o_O.message))
                        .then(response => {
                            let { success, items } = response.data;
                            if (success) {
                                location.href = `realizarExcel`;
                            }
                        });
                });

                $('#tblAsignacionEvaluacion').on("click", ".descargarEvaluacionActualPdf", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    Alert2AccionConfirmar('¡Cuidado!', '¿Desea descargar la evaluación en PDF?', 'Confirmar', 'Cancelar', () => fncDescargarEvaluacionPDF(rowData.idAsignacion, rowData.descripcioncc));
                });
            },
            columnDefs: [
                { className: 'dt-center', 'targets': '_all' },
                //{ className: 'dt-body-center', targets: [0] },
                //{ className: 'dt-body-right', targets: [0] },
                //{ width: '5%', targets: [0] }
            ], drawCallback: function (settings) {
                $("#tblAsignacionEvaluacion").DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                    if (colIdx == 6) {
                        let existeFechaInicial = 0;

                        for (let x = 0; x < this.data().length; x++) {
                            let fechaInicial = moment(this.data()[x]).format("DD/MM/YYYY");
                            if (fechaInicial != '01/01/2000') {
                                existeFechaInicial = 1
                            }
                        }

                        this.column(colIdx).visible(existeFechaInicial != 0);
                        this.column(colIdx++).visible(existeFechaInicial != 0);
                    }
                });
            },
        });
        // $('.dataTables_scrollBody').css('height', '330px')
    }

    function fncDescargarEvaluacionPDF(idAsignacion, descripcioncc) {
        var path = `/Reportes/Vista.aspx?idReporte=249&idAsignacion=${idAsignacion}&descripcioncc=${descripcioncc}`;
        $("#report").attr("src", path);
        document.getElementById('report').onload = function () {
            $.unblockUI();
            openCRModal();
        };
    }
    function CargarColorVerdeDeNotificado2(idPlantilla, idAsignacion) {
        axios.post('cambiarDeColor', { idPlantilla: idPlantilla, idAsignacion: idAsignacion }).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                items.forEach(element => {
                    $('#' + element.idButton).addClass(element.classe);
                });
            }
        }).catch(error => Alert2Error(error.message));
    }
    function CargarColorVerdeDeNotificado(rowData) {
        axios.post('cambiarDeColor', { idPlantilla: rowData.idPlantilla, idAsignacion: rowData.idAsignacion }).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                items.forEach(element => {
                    $('#' + element.idButton).addClass(element.classe);
                });
            }
        }).catch(error => Alert2Error(error.message));
    }
    function imprimirDivciones(rowData) {
        axios.post('obtenerDivicionesEvaluadorArchivos', { idPlantilla: rowData.idPlantilla, idAsignacion: rowData.idAsignacion })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    $('#divPantalla').remove();
                    let html = '<div id="divPantalla">';
                    html += `<label>Elementos a evaluar:</label><br>`;
                    items.forEach(element => {
                        html += `<div class='row'>
                                    <div class='col-md-4'>     
                                        <button style="margin-top: 3px;" class="btn btn-block btn-social btn-primary-menu boton-menu" id="${element.idbutton}" data-id="${element.id}" title="${element.toltips}">
                                            <i class="fa fa-clipboard-check icono-menu"></i> ${element.descripcion}
                                        </button>
                                    </div> 
                                </div>`;
                    });
                    html += '</div>'
                    divicionesxPlantilla.append(html);
                }
            });
    }
    function ObtenerContratistas() {

        let objProyecto = cboProyecto.find(`option[value="${cboProyecto.val()}"]`);
        let prefijo = objProyecto.attr("data-prefijo");

        axios.post('obtenerContratistasConContrato', { //omaromar
            AreaCuenta: cboProyecto.val(),
            subcontratista: cboSubContratista.val() == "" ? 0 : cboSubContratista.val(),
            Estatus: cboEstatus.val(),
            cc: prefijo
        }).catch(o_O => AlertaGeneral(o_O.message)).then(response => {
            let { success, items } = response.data;
            if (success) {
                AddRows($('#tblAsignacionEvaluacion'), items);
                if (cboEstatus.val() === '0') {
                    $('#txtTexto').text('PERIODO EVALUACIÓN')
                    var table = $('#tblAsignacionEvaluacion').DataTable();
                    table.column(0).visible(false);
                } else if (cboEstatus.val() === '2') {
                    // $('#txtTexto').text('Acciones')
                    var table = $('#tblAsignacionEvaluacion').DataTable();
                    table.column(0).visible(true);
                }
                $('#tblAsignacionEvaluacion').css('font-size', '12px');

                fncVerificarTipoUsuario();
                if (tipoUsuario == 4) {
                    $(".GuardarAsignacion").attr("disabled", true);
                    $(".AutorizarAsignacion").attr("disabled", true);
                    $(".Evaluar").attr("disabled", true);
                    $(".autorizarEvaluacion").attr("disabled", true);
                } else {
                    $(".GuardarAsignacion").attr("disabled", false);
                    $(".AutorizarAsignacion").attr("disabled", false);
                    $(".Evaluar").attr("disabled", false);
                    $(".autorizarEvaluacion").attr("disabled", false);
                }
            }
        });
    }

    function getParametersSubContrat(rowData, fechaInicial, fechaFinal, freqNum, NombreEvaluacion, inpServicioContratado, idPlantilla, inpEstado, inpMunicipio, inpCantEvaluaciones) {
        let strMensajeError = "";

        if (NombreEvaluacion == "") { strMensajeError += "Es necesario indicar nombre evaluación."; }
        if (inpServicioContratado == "") { strMensajeError += "<br>Es necesario indicar el servicio contratado."; }
        if (fechaInicial == "") { strMensajeError += "<br>Es necesario indicar fecha inicial."; }
        if (fechaFinal == "") { strMensajeError += "<br>Es necesario indicar fecha final."; }
        if (inpEstado == "") { strMensajeError += "<br>Es necesario indicar el estado."; }
        if (inpMunicipio == "") { strMensajeError += "<br>Es necesario indicar el municipio."; }
        if (inpCantEvaluaciones == "") { strMensajeError += "<br>Es necesario indicar la cantidad de evaluaciones."; }

        if (fechaInicial > fechaFinal) {
            if (strMensajeError != "") {
                strMensajeError += "<br>La fecha final debe ser mayor a la fecha inicial.";
            } else {
                strMensajeError += "La fecha final debe ser mayor a la fecha inicial.";
            }
        }

        if (!strMensajeError != "") {
            let item = {
                cc: rowData.cc,
                idSubContratista: rowData.idSubContratista,
                idAsignacion: rowData.idAsignacion,
                lstRelacion: getParametersPreguntar(),
                fechaInicial: fechaInicial,
                fechaFinal: fechaFinal,
                freqEval: Number(freqNum),
                NombreEvaluacion: NombreEvaluacion,
                inpServicioContratado, inpServicioContratado,
                idPlantilla: idPlantilla,
                idContrato: rowData.idContrato,
                idEstado: inpEstado,
                idMunicipio: inpMunicipio,
                cantEvaluaciones: inpCantEvaluaciones
            };
            return item;
        } else {
            Alert2Warning(strMensajeError);
            return "";
        }
    }
    function GuardarAsignacion(rowData, fechaInicial, fechaFinal, freqNum, NombreEvaluacion, inpServicioContratado, idPlantilla, inpEstado, inpMunicipio, inpCantEvaluaciones) {
        let parametros = getParametersSubContrat(rowData, fechaInicial, fechaFinal, freqNum, NombreEvaluacion, inpServicioContratado, idPlantilla, inpEstado, inpMunicipio, inpCantEvaluaciones);
        if (parametros != "") {
            axios.post('GuardarAsignacion', { parametros: parametros }).catch(o_O => AlertaGeneral(o_O.message)).then(response => {
                let { success, items } = response.data;
                if (success) {
                    ObtenerContratistas();
                    mdlPreguntarPrimero.modal('hide');
                }
            });
        }
    }
    function getAutorizarEvaluacion(rowData) {
        let item = {
            id: rowData.idAsignacion,
            correo: rowData.emails
        };
        return item;
    }
    function AutorizarEvaluacion(rowData) {
        let parametros = getAutorizarEvaluacion(rowData);
        axios.post('AutorizarEvaluacion', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    ObtenerContratistas();
                } else {
                    Alert2Error(message);
                }
            });
    }

    function AutorizarAsignacion(rowData) {
        let parametros = getAutorizarEvaluacion(rowData);
        axios.post('AutorizarAsignacion', { objDTO: parametros }).catch(o_O => AlertaGeneral(o_O.message)).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                ObtenerContratistas();
            } else {
                Alert2Error(message);
            }
        });
    }

    function abrirModalPreguntarPrimero(rowData) {
        mdlPreguntarPrimero.modal('show');
        axios.post('obtenerTodosLosElementosConSuRequerimiento')
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    ImprimirTodoPreguntarPrimero(items, rowData);
                }
            });
    }
    function ImprimirTodoPreguntarPrimero(items, rowData) {
        datosAGuardar = {};
        datosAGuardar = rowData;
        containerPreguntarPrimero.find('div').remove();
        let html = '<div id="ContenidoPreguntaPrim">';
        items.forEach(x => {
            if (x.Aparece == true) {
                html += x.descripcion + `<br>`
                x.lstRequerimientos.forEach(y => {
                    html += `&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<input data-idreq='${y.id}' type='checkbox'/>&nbsp&nbsp` + y.texto + `<br>`
                });
            }
        });
        html += '</div>'
        containerPreguntarPrimero.append(html);
    }
    function getParametersPreguntar() {
        let paramet = [];
        let input = $('#ContenidoPreguntaPrim').find('input');
        for (let index = 0; index < input.length; index++) {
            let items = {
                idSubContratista: datosAGuardar.id,
                Preguntar: $(input[index]).prop('checked'),
                idReq: $(input[index]).attr('data-idreq'),
            }
            paramet.push(items);
        }
        return paramet;
    }
    function fncGuardarPreguntarPrimero() {
        GuardarAsignacion(datosAGuardar);
    }
    function cargandoModal(lstDatos, idsection, rowData) {
        CargarArchivosXSubcontratista(lstDatos, rowData)
        obtenerFuncion();
    }
    function CargarConfiguracionVistaModal(rowData) {
        axios.post('obtenerDivicionesEvaluadorArchivos', { idPlantilla: rowData.idPlantilla, idAsignacion: rowData.id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    obtenerVisibles(items, rowData, rowData.id)
                }
            });
    }
    function obtenerVisibles(items2, rowData, idAsignacion) {
        let dn = [];
        axios.post('obtenerElementosEvaluar', { idPlantilla: rowData.idPlantilla, idAsignacion: idAsignacion })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items.items != "" && items.items != undefined) {
                        let item = items.items.split(',');
                        item.forEach(x => {
                            if (x != "") {
                                dn.push(x);
                            }
                        });

                    }
                    fncGenerandoDivicionVistaModal(items2, rowData, dn, response.data.items.elementoVerde);
                }
            });
    }
    function fncGenerandoDivicionVistaModal(lstDatos, rowData, dn, elementoVerde) {
        let elementoInicial = null;

        let html = `<div class="col-lg-2 col-md-2 col-sm-2" style="margin-top:15px;" id='ContenidoDivicionesModal'>
                        <div class=" row">
                            <div class="col-lg-12 col-md-12 col-sm-12" style="padding: 0;">
                                <fieldset class="fieldset-custm">
                                <legend class="legend-custm">Elementos:</legend>`;
        for (let index = 0; index < lstDatos.length; index++) {
            for (let v = 0; v < dn.length; v++) {
                if (lstDatos[index].id == dn[v]) {
                    if (elementoInicial == null) {
                        elementoInicial = lstDatos[index].idbutton;
                    }
                    // console.log(elementoVerde);
                    // if (!elementoVerde) {
                    // html += `<button class="btn btn-block btn-social btn-primary-menu boton-menu" id="${lstDatos[index].idbutton}" 
                    html += `<button class="btn btn-block btnElementos" id="${lstDatos[index].idbutton}" 
                                    data-id="${lstDatos[index].id}" title="${lstDatos[index].toltips}">
                                    <i class="fa fa-clipboard-check icono-menu"></i> ${lstDatos[index].descripcion}
                                </button>`;

                    $(`${lstDatos[index].idbutton}`).trigger("click");
                    // } else {
                    //     html += `< button class="btn btn-block btn-social btn-success-menu btn-success-menu boton-menu" id = "${lstDatos[index].idbutton}"
                    //                 data-id="${lstDatos[index].id}" title="${lstDatos[index].toltips}" style="background-color: green;">
                    //                 <i class="fa fa-clipboard-check icono-menu"></i> ${lstDatos[index].descripcion}
                    //             </>`;
                    // }
                }
            }
        }

        html += ` </fieldset >
                    <fieldset class="fieldset-custm">
                    <legend class="legend-custm">Indicadores: </legend>

                    <button class="btn btn-block boton-estatus "> 0 - 25 Pesimo<br>
                        <i class="fa fa-star color-pesimo"></i>
                    </button>

                    <button class="btn btn-block boton-estatus"> 26 - 50 Malo<br>
                        <i class="fa fa-star color-malo"></i>
                        <i class="fa fa-star color-malo"></i>
                    </button>
                    <button class="btn btn-block boton-estatus"> 51 - 75 Regular<br>
                        <i class="fa fa-star color-regular"></i>
                        <i class="fa fa-star color-regular"></i>
                        <i class="fa fa-star color-regular"></i>
                    </button>
                    <button class="btn btn-block boton-estatus"> 76 - 90 Aceptable<br>
                        <i class="fa fa-star color-aceptable"></i>
                        <i class="fa fa-star color-aceptable"></i>
                        <i class="fa fa-star color-aceptable"></i>
                        <i class="fa fa-star color-aceptable"></i>
                    </button>
                    <button class="btn btn-block boton-estatus"> 91 - 100 Excediendo las expectativas<br>
                        <i class="fa fa-star color-excediendo"></i>
                        <i class="fa fa-star color-excediendo"></i>
                        <i class="fa fa-star color-excediendo"></i>
                        <i class="fa fa-star color-excediendo"></i>
                        <i class="fa fa-star color-excediendo"></i>
                    </button>
                </fieldset>
            </div>
        </div>
    </div>`;
        html += `<div class="col-lg-10 col-md-10 col-sm-10" id='ConenidoArchivosModal'>`;
        for (let index = 0; index < lstDatos.length; index++) {
            for (let v = 0; v < dn.length; v++) {
                if (lstDatos[index].id == dn[v]) {
                    html += `<div id="${lstDatos[index].idsection}"  class="col-md-12">
                                <fieldset class="fieldset-custm">
                                    <legend class="legend-custm">${lstDatos[index].descripcion} : <span id="lblErrorProyecto"></span></legend>
                                    <div id="${lstDatos[index].idsection + lstDatos[index].id}" ></div>
                                </fieldset>
                            </div>`;
                }
            }
        }
        html += `<div class="row"><br>
                    <div class='col-md-6 col-lg-6'>
                        <div class='input-group'>
                            <span class='input-group-addon'>Calificación global</span>
                            <input type='text' id='inpCalificacionGlobal' class='form-control'>
                        </div>
                    </div>
                </div>
            </div>`;
        contenido23.append(html);

        let lstARr = [];
        for (let index = 0; index < lstDatos.length; index++) {
            for (let v = 0; v < dn.length; v++) {
                if (lstDatos[index].id == dn[v]) {
                    lstARr.push(lstDatos[index])
                }
            }
        }
        $('#inpCalificacionGlobal').val(rowData.promedio);
        //FUNCION   
        for (let index = 0; index < lstARr.length; index++) {
            $(`#${lstARr[index].idsection}`).css('display', 'none');
            $(`#${lstARr[index].idbutton}`).click(function () {
                $('#contenidoPromedio').remove();
                $('#contenidoPromedio2').remove();
                CargarColorVerdeDeNotificado(rowData);
                for (let i = 0; i < lstARr.length; i++) {
                    if (lstARr[index].idbutton == lstARr[i].idbutton) {
                        $(`#${lstARr[i].idsection}`).css('display', 'block');
                        btnBuscar.attr('data-idTipoEvaluacion', lstARr[i].id);
                        ObtenerReqModal(lstARr[index].id, lstARr[index].idsection + lstARr[index].id, rowData, dn, $(this));
                    } else {
                        $(`#${lstARr[i].idsection}`).css('display', 'none');
                    }

                }
            })
        }
        if (lstARr.length != 0) {
            $(`#${lstARr[0].idsection}`).css('display', 'block');
            btnBuscar.attr('data-id', lstARr[0].id);
            btnBuscar.attr('data-idTipoEvaluacion', lstARr[0].id);
            ObtenerReqModal(lstARr[0].id, lstARr[0].idsection + lstARr[0].id, rowData, dn, $(`#${elementoInicial}`));
        }

        for (let index = 0; index < lstDatos.length; index++) {
            let obj = new Object();
            obj.idDiv = lstDatos[index].id;
            obj.idEvaluacion = txtFechaPeriodo.attr("data-id");
            axios.post("VerificarElementoTerminado", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    $(`#${lstDatos[index].idbutton}`).css("background-color", "green");
                } else {
                    $(`#${lstDatos[index].idbutton}`).css("background-color", "#337ab7");
                }
            }).catch(error => Alert2Error(error.message));
        }
    }

    function ObtenerReqModal(id, idsection, rowData, dn, btnEvaluacion) {
        axios.post('obtenerRequerimientos', { idDiv: id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {

                    GenerarInputModal(items, idsection, rowData, dn, btnEvaluacion);
                }
            });
    }
    function ObtenerGraficaDeEvaluacionPorDivisionElemento(id) {
        let parametros = {
            id: id
        };
        axios.post('ObtenerGraficaDeEvaluacionPorDivisionElemento', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    gpxGraficaSubxCa('gpxGraficaSubxCalificacion', response.data, null, null);
                }
            });
    }
    function gpxGraficaSubxCa(gpxGraficaSubxCalificacion, datos, callback, items) {
        Highcharts.chart(gpxGraficaSubxCalificacion, {
            chart: {
                type: 'column'
            },
            title: {
                text: ''
            },
            accessibility: {
                announceNewData: {
                    enabled: true
                }
            },
            xAxis: {
                type: 'category'
            },
            yAxis: {
                title: {
                    text: 'Total'
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.2f}'
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
            },
            series: datos.gpxGrafica,
            credits: { enabled: false }
        });
    }
    function ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(/*cc,idSubContratista*/idContrato) {
        let parametros = {
            // cc:cc,
            // idSubContratista:idSubContratista,
            idContrato: idContrato
        }
        axios.post('ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    gpxObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones('gpxSubPorCalificacionMensual', response.data, null, null);
                }
            });
    }
    function gpxObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(gpxSubPorCalificacionMensual, datos, callback, items) {
        let categorias = [];
        datos.gpxGrafica[0].data.forEach(x => {
            categorias.push(x.name)
        });

        Highcharts.chart(gpxSubPorCalificacionMensual, {

            title: {
                text: ''
            },
            accessibility: {
                announceNewData: {
                    enabled: true
                }
            },
            xAxis: {
                categories: categorias,
                type: 'Meses'
            },
            yAxis: {
                min: 0,
                max: 100,
                title: {
                    text: 'Promedio de evaluaciones'
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.2f}'
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
            },
            series: datos.gpxGrafica,
            credits: { enabled: false }
        });
    }

    function fncVerificarTipoUsuario() {
        axios.post("VerificarTipoUsuario").then(response => {
            let { success, items, message } = response.data;
            if (success) {
                tipoUsuario = response.data.objTipoUsuario.tipo;
            } else {
                Alert2Error(message);
            }
        }).catch(error => Alert2Error(error.message));
    }

    $(document).ready(() => {
        subContratistas.EvaluacionSubcontratista = new EvaluacionSubcontratista();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();