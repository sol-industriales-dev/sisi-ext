(() => {
    $.namespace('Administrativo.SeguimientoCompromisos.Captura');
    Captura = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const botonBuscar = $('#botonBuscar');
        const botonGuardar = $('#botonGuardar');
        const tablaCapturas = $('#tablaCapturas');
        const modalEvidencias = $('#modalEvidencias');
        const tablaEvidencias = $('#tablaEvidencias');
        const inputProgresoEstimado = $('#inputProgresoEstimado');
        const inputComentarios = $('#inputComentarios');
        //#endregion

        let dtCapturas;
        let dtEvidencias;

        _agrupacion_id = 0;
        _actividad_id = 0;

        (function init() {
            agregarListeners();
            initTablaCapturas();
            initTablaEvidencias();
            $('.select2').select2();

            selectCentroCosto.fillCombo('/Administrativo/SeguimientoCompromisos/GetAgrupacionCombo', null, false, null);
        })();

        modalEvidencias.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        $('input[type=file]').on('change', function () {
            let inputEvidencia = $(`.inputEvidencia`);
            let botonEvidencia = $(`#botonEvidencia`);
            let iconoBoton = botonEvidencia.find('i');
            let labelArchivo = $(`#labelArchivoEvidencia`);

            if (inputEvidencia[0].files.length > 0) {
                let textoLabel = inputEvidencia[0].files[0].name;

                if (textoLabel.length > 35) {
                    textoLabel = textoLabel.substr(0, 31) + '...';
                }

                labelArchivo.text(textoLabel);
                botonEvidencia.addClass('custom-file-upload-subido');
                botonEvidencia.removeClass('custom-file-upload');
                iconoBoton.addClass('fa-check');
                iconoBoton.removeClass('fa-file-upload');
            } else {
                labelArchivo.text('');
                botonEvidencia.addClass('custom-file-upload');
                botonEvidencia.removeClass('custom-file-upload-subido');
                iconoBoton.addClass('fa-file-upload');
                iconoBoton.removeClass('fa-check');
            }
        });

        function agregarListeners() {
            botonBuscar.click(cargarAsignacionCaptura);
            botonGuardar.click(guardarEvidencia);
        }

        function guardarEvidencia() {
            let progresoEstimado = inputProgresoEstimado.val();
            let comentarios = inputComentarios.val();

            //#region Validaciones
            if (progresoEstimado == '' || isNaN(progresoEstimado) || (+progresoEstimado < 1) || (+progresoEstimado > 100)) {
                Alert2Warning('Debe capturar un número válido para el progreso estimado.');
                return;
            }

            if (_progresoRealActual > 0) {
                if (+progresoEstimado <= _progresoRealActual) {
                    Alert2Warning('Debe capturar un avance mayor al progreso real.');
                    return;
                }
            } else {
                if (+progresoEstimado <= _progresoEstimadoActual) {
                    Alert2Warning('Debe capturar un avance mayor al progreso estimado.');
                    return;
                }
            }

            if (comentarios == '') {
                Alert2Warning('Debe capturar un comentario para la captura.');
                return;
            }
            //#endregion

            const data = new FormData();
            let inputEvidencia = $(`.inputEvidencia`);
            let evidencia = inputEvidencia[0].files.length > 0 ? inputEvidencia[0].files[0] : null;

            if (evidencia != null) {
                data.append('evidencias', evidencia);
            } else {
                data.append('evidencias', null);
            }

            data.append('captura', JSON.stringify({
                agrupacion_id: _agrupacion_id,
                actividad_id: _actividad_id,
                progresoEstimado: progresoEstimado,
                comentariosCaptura: comentarios
            }));

            $.ajax({
                url: '/Administrativo/SeguimientoCompromisos/GuardarEvidencia',
                data: data,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                dtCapturas.clear().draw();

                if (response.success) {
                    AlertaGeneral(`Éxito`, `Se ha guardado la información.`);
                    $(`.inputEvidencia`).val('');
                    modalEvidencias.modal('hide');
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                dtCapturas.clear().draw();
            }
            );
        }

        function initTablaCapturas() {
            dtCapturas = tablaCapturas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaCapturas.on('click', '.botonEvidencias', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();

                        _agrupacion_id = rowData.agrupacion_id;
                        _actividad_id = rowData.actividad_id;

                        axios.post('GetEvidenciasActividad', { agrupacion_id: rowData.agrupacion_id, area: rowData.area, actividad_id: rowData.actividad_id })
                            .then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    _progresoEstimadoActual = response.data.data.map((x) => x.progresoEstimado).at(-1);
                                    _progresoRealActual = response.data.data.map((x) => x.progreso).at(-1);

                                    limpiarModal();
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
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'areaDesc', title: 'Área' },
                    {
                        title: 'Estatus', render: function (data, type, row, meta) {
                            let estatus = '';

                            if (row.evidencias.length > 0) {
                                let ultimoProgresoEstimado = Math.max(...row.evidencias.map((x) => x.progresoEstimado));
                                let ultimoProgresoReal = Math.max(...row.evidencias.map((x) => x.progreso));

                                if (ultimoProgresoReal > 0) {
                                    if (ultimoProgresoReal == 100) {
                                        estatus = 'EVALUADO';
                                    } else {
                                        estatus = 'PENDIENTE';
                                    }
                                } else {
                                    if (ultimoProgresoEstimado == 100) {
                                        estatus = 'COMPLETO';
                                    } else {
                                        estatus = 'PENDIENTE';
                                    }
                                }
                            } else {
                                estatus = 'PENDIENTE'
                            }

                            return estatus;
                        }
                    },
                    { data: 'fechaCompromisoString', title: 'Fecha Compromiso' },
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
                        title: 'Evidencia', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-primary botonEvidencias"><i class="fa fa-align-justify"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function limpiarModal() {
            $(`.inputEvidencia`).val('');
            $('input[type=file]').change();
            inputProgresoEstimado.val('');
            inputComentarios.val('');
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

                        descargarArchivo(rowData.id);
                    });

                    tablaEvidencias.on('change', 'input[type=file]', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtCapturas.row(row).data();
                        let inputEvidencia = $(row).find(`.inputEvidencia_${rowData.id}`);
                        let botonEvidencia = $(row).find(`#botonEvidencia_${rowData.id}`);
                        let iconoBoton = botonEvidencia.find('i');
                        let labelArchivo = $(row).find(`#labelArchivoEvidencia_${rowData.id}`);

                        if (inputEvidencia[0].files.length > 0) {
                            let textoLabel = inputEvidencia[0].files[0].name;

                            if (textoLabel.length > 35) {
                                textoLabel = textoLabel.substr(0, 31) + '...';
                            }

                            labelArchivo.text(textoLabel);
                            botonEvidencia.addClass('custom-file-upload-subido');
                            botonEvidencia.removeClass('custom-file-upload');
                            iconoBoton.addClass('fa-check');
                            iconoBoton.removeClass('fa-file-upload');
                        } else {
                            labelArchivo.text('');
                            botonEvidencia.addClass('custom-file-upload');
                            botonEvidencia.removeClass('custom-file-upload-subido');
                            iconoBoton.addClass('fa-file-upload');
                            iconoBoton.removeClass('fa-check');
                        }
                    });
                },
                createdRow: function (row, rowData) {

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
                            return `
                                <div class="progress" style="margin-bottom: 0px;">
                                    <div class="progress-bar" role="progressbar" aria-valuenow="${data}" aria-valuemin="0" aria-valuemax="100" style="width: ${data}%;">
                                        <span>${data}%</span>
                                    </div>
                                </div>`;
                            // return data + '%';
                        }
                    },
                    { data: 'fechaEvaluacionString', title: 'Fecha Evaluación' },
                    { data: 'comentariosEvaluador', title: 'Comentarios Evaluador' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarAsignacionCaptura() {
            axios.post('/Administrativo/SeguimientoCompromisos/GetAsignacionActividadesCaptura', { agrupacion_id: +selectCentroCosto.getAgrupador() })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaCapturas, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function descargarArchivo(evidencia_id) {
            location.href = `/Administrativo/SeguimientoCompromisos/DescargarArchivoEvidencia?evidencia_id=${evidencia_id}`;
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.SeguimientoCompromisos.Captura = new Captura())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...', baseZ: 2000 }))
        .ajaxStop($.unblockUI);
})();