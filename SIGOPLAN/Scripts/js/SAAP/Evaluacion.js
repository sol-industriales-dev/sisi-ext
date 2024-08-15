(() => {
    $.namespace('SAAP.Evaluacion');
    Evaluacion = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const selectAreas = $('#selectAreas');
        const selectEstatus = $('#selectEstatus');
        const botonBuscar = $('#botonBuscar');
        const tablaCapturas = $('#tablaCapturas');
        const botonGuardar = $('#botonGuardar');
        const botonCancelar = $('#botonCancelar');
        const modalEvidencias = $('#modalEvidencias');
        const tablaEvidencias = $('#tablaEvidencias');
        const botonQuitarTerminacion = $('#botonQuitarTerminacion');
        //#endregion

        let dtCapturas;
        let dtEvidencias;

        _agrupacion_id = 0;
        _actividad_id = 0;

        (function init() {
            initTablaCapturas();
            initTablaEvidencias();

            selectCentroCosto.fillCombo('/SAAP/SAAP/GetAgrupacionCombo', null, false, null);
            selectAreas.fillCombo("/SAAP/SAAP/GetAreaCombo", null, false, '--Todos--');
            $('.select2').select2();

            botonGuardar.click(guardarEvaluacion);
            botonBuscar.click(cargarEvidencias);
            botonQuitarTerminacion.click(() => { $('.radio').prop('checked', false); });
        })();

        modalEvidencias.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function initTablaCapturas() {
            dtCapturas = tablaCapturas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaCapturas.on('click', '.botonEvidencias', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();

                        _agrupacion_id = rowData.agrupacion_id;
                        _actividad_id = rowData.actividad_id;

                        axios.post('GetEvidenciasActividad', { agrupacion_id: rowData.agrupacion_id, area: rowData.area, actividad_id: rowData.id })
                            .then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    _progresoEstimadoActual = response.data.data.map((x) => x.progresoEstimado).at(-1);
                                    _progresoRealActual = response.data.data.map((x) => x.progreso).at(-1);

                                    let ocultarBotones =
                                        (response.data.data.filter((x) => x.usuarioEvaluador_id == 0).length == 0) ||
                                        (response.data.data.filter((x) => x.terminacion).length > 0);

                                    botonGuardar.css('display', ocultarBotones ? 'none' : 'inline-block');
                                    botonCancelar.css('display', ocultarBotones ? 'none' : 'inline-block');
                                    botonQuitarTerminacion.css('display', ocultarBotones ? 'none' : 'inline-block');

                                    AddRows(tablaEvidencias, response.data.data);
                                    modalEvidencias.modal('show');
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'agrupacionDesc', title: 'Proyecto' },
                    { data: 'areaDesc', title: 'Área' },
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'clasificacionDesc', title: 'Clasificación' },
                    {
                        title: 'Estatus', render: function (data, type, row, meta) {
                            let estatus = '';

                            if (row.evidencias.every((x) => x.usuarioEvaluador_id > 0) || row.evidencias.some((x) => x.terminacion)) {
                                estatus = 'EVALUADO';
                            } else {
                                estatus = 'PENDIENTE';
                            }

                            return estatus;
                        }
                    },
                    {
                        data: 'progresoEstimado', title: 'Autoevaluación', 
                        render: function (data, type, row, meta) {
                            return `
                                <div class="progress" style="margin-bottom: 0px;">
                                    <div class="progress-bar" role="progressbar" aria-valuenow="${data}" aria-valuemin="0" aria-valuemax="100" style="width: ${data}%;">
                                        <span>${data}%</span>
                                    </div>
                                </div>
                            `;
                        }
                    },
                    {
                        data: 'progresoReal', title: 'Progreso Real', render: function (data, type, row, meta) {
                            return `
                                <div class="progress" style="margin-bottom: 0px;">
                                    <div class="progress-bar" role="progressbar" aria-valuenow="${data}" aria-valuemin="0" aria-valuemax="100" style="width: ${data}%;">
                                        <span>${data}%</span>
                                    </div>
                                </div>
                            `;
                        }
                    },
                    {
                        title: 'Evidencias', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-primary botonEvidencias"><i class="fa fa-align-justify"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaEvidencias() {
            dtEvidencias = tablaEvidencias.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaEvidencias.on('click', '.btn-descargar-evidencia', function () {
                        let rowData = dtEvidencias.row($(this).closest('tr')).data();

                        descargarArchivoEvidencia(rowData.id);
                    });

                    tablaEvidencias.on('click', '.radio', function () {
                        let row = $(this).closest('tr');

                        if ($(this).prop('checked')) {
                            $(row).find('.inputProgreso').val(100);
                        }
                    });

                    tablaEvidencias.on('change', '.inputProgreso', function () {
                        let row = $(this).closest('tr');

                        if ($(this).val() == 100) {
                            botonQuitarTerminacion.click();
                            $(row).find('.radio').prop('checked', true);
                        } else {
                            botonQuitarTerminacion.click();
                        }
                    });
                },
                createdRow: function (row, rowData) {
                    if (rowData.terminacion) {
                        $(row).find('.radio').prop('checked', true);
                    }
                },
                columns: [
                    { data: 'consecutivo', title: '#' },
                    {
                        title: 'Evidencia', render: function (data, type, row, meta) {
                            return row.rutaEvidencia != '' ? '<button class="btn-descargar-evidencia btn btn-sm btn-primary"><i class="fas fa-file-download"></i></button>' : '';
                        }
                    },
                    { data: 'fechaCreacionString', title: 'Fecha Captura' },
                    {
                        data: 'progresoEstimado', title: 'Progreso Estimado', render: function (data, type, row, meta) {
                            return `
                                <div class="progress" style="margin-bottom: 0px;">
                                    <div class="progress-bar" role="progressbar" aria-valuenow="${data}" aria-valuemin="0" aria-valuemax="100" style="width: ${data}%;">
                                        <span>${data}%</span>
                                    </div>
                                </div>`;
                            // return data + '%';
                        }
                    },
                    { data: 'comentariosCaptura', title: 'Comentarios' },
                    {
                        data: 'progreso', title: 'Progreso Real', render: function (data, type, row, meta) {
                            if (row.usuarioEvaluador_id == 0) {
                                if (!row.evaluacionCompletada) {
                                    return `<input class="form-control text-center inputProgreso" type="number">`;
                                } else {
                                    return '';
                                }
                            } else {
                                return `
                                <div class="progress" style="margin-bottom: 0px;">
                                    <div class="progress-bar" role="progressbar" aria-valuenow="${data}" aria-valuemin="0" aria-valuemax="100" style="width: ${data}%;">
                                        <span>${data}%</span>
                                    </div>
                                </div>`;
                                // return data + '%';
                            }
                        }
                    },
                    { data: 'fechaEvaluacionString', title: 'Fecha Evaluación' },
                    {
                        data: 'comentariosEvaluador', title: 'Comentarios Evaluador', render: function (data, type, row, meta) {
                            if (row.usuarioEvaluador_id == 0) {
                                if (!row.evaluacionCompletada) {
                                    return `<input class="form-control inputComentariosEvaluador">`;
                                } else {
                                    return '';
                                }
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        title: 'Terminación', render: function (data, type, row, meta) {
                            if (!row.evaluacionCompletada || row.terminacion) {
                                return `<input class="form-control radio" type="radio" name="radioTerminacion" value="${row.id}">`;
                            } else {
                                return '';
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarEvidencias() {
            let estatus = +(selectEstatus.val());
            let filtroArea = selectAreas.val() == '--Todos--' ? 0 : selectAreas.val();

            axios.post('/SAAP/SAAP/GetActividadesEvaluacion', { agrupacion_id: +selectCentroCosto.getAgrupador(), estatus, filtroArea: filtroArea })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaCapturas, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarEvaluacion() {
            let evaluaciones = [];
            let flagTerminacionInvalida = false;

            tablaEvidencias.find('tbody tr').each(function (index, row) {
                let rowData = dtEvidencias.row(row).data();

                if (rowData.usuarioEvaluador_id == 0) {
                    let progreso = +$(row).find('.inputProgreso').val();
                    let comentariosEvaluador = $(row).find('.inputComentariosEvaluador').val();
                    let terminacion = $(row).find('.radio').prop('checked');

                    if (terminacion && progreso < 100) {
                        flagTerminacionInvalida = true;
                    }

                    if (progreso > 0) {
                        evaluaciones.push({
                            id: rowData.id,
                            consecutivo: rowData.consecutivo,
                            progreso,
                            comentariosEvaluador,
                            terminacion
                        });
                    }
                }
            });

            if (flagTerminacionInvalida) {
                Alert2Warning('El progreso debe ser igual a 100 para dar terminación a la evaluación.');
                return;
            }

            if (evaluaciones.length > 0) {
                axios.post('/SAAP/SAAP/GuardarEvaluaciones', { evaluaciones })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            modalEvidencias.modal('hide');
                            cargarEvidencias();
                            AlertaGeneral(`Éxito`, `Se ha guardado la información.`);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                AlertaGeneral(`Alerta`, `No se han evaluado evidencias.`);
            }
        }

        function descargarArchivoEvidencia(evidencia_id) {
            if (evidencia_id > 0) {
                location.href = `/SAAP/SAAP/DescargarArchivoEvidencia?evidencia_id=${evidencia_id}`;
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => SAAP.Evaluacion = new Evaluacion())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();