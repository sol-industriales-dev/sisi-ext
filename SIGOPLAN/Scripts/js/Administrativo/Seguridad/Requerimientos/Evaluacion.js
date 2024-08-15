(() => {
    $.namespace('Administrativo.Requerimientos.Evaluacion');
    Evaluacion = function () {
        //#region Selectores
        const selectClasificacion = $('#selectClasificacion');
        const selectCentroCosto = $('#selectCentroCosto');
        const selectRequerimiento = $('#selectRequerimiento');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const selectEstatus = $('#selectEstatus');
        const botonBuscar = $('#botonBuscar');
        const tablaCapturas = $('#tablaCapturas');
        const botonGuardar = $('#botonGuardar');
        //#endregion

        let dtCapturas;

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioMesAnterior = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        fechaInicioMesAnterior.setMonth(fechaInicioMesAnterior.getMonth() - 1);
        const fechaFinMesAnterior = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        fechaFinMesAnterior.setDate(fechaFinMesAnterior.getDate() - 1);

        (function init() {
            agregarListeners();
            initTablaCapturas();
            $('.select2').select2();

            selectClasificacion.fillCombo('/Administrativo/Requerimientos/GetClasificacionCombo', null, false, null);
            selectCentroCosto.fillComboSeguridad(false);
            selectRequerimiento.fillCombo('/Administrativo/Requerimientos/GetRequerimientosCombo', null, false, null);

            inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioMesAnterior);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaFinMesAnterior);
        })();

        function agregarListeners() {
            botonGuardar.click(guardarEvaluacion);
            botonBuscar.click(cargarEvidencias);
        }

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
                    tablaCapturas.on('click', '.btn-ver-evidencia', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();

                        mostrarArchivoEvidencia(rowData.id);
                    });

                    tablaCapturas.on('click', '.btn-descargar-evidencia', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();

                        descargarArchivoEvidencia(rowData.id);
                    });

                    tablaCapturas.on('click', '.radioBtn a', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        let div = $(this).closest('div');
                        let seleccion = $(this).attr('aprobado');

                        div.find(`a[data-toggle="radioAprobado${rowData.id}"]`).not(`[aprobado="${seleccion}"]`).removeClass('active').addClass('notActive');
                        div.find(`a[data-toggle="radioAprobado${rowData.id}"][aprobado="${seleccion}"]`).removeClass('notActive').addClass('active');
                    });
                },
                createdRow: function (row, rowData) {
                    if (rowData.usuarioEvaluadorID > 0) {
                        if (rowData.aprobado) {
                            $(row).addClass('renglonAprobado');
                        } else {
                            $(row).addClass('renglonNoAprobado');
                        }
                    }
                },
                columns: [
                    { data: 'proyecto', title: 'Proyecto' },
                    { data: 'requerimientoDesc', title: 'Requerimiento' },
                    { data: 'puntoDesc', title: 'Punto' },
                    { data: 'codigo', title: 'Código' },
                    { data: 'fechaCapturaString', title: 'Fecha Captura' },
                    {
                        title: 'Evidencia', render: function (data, type, row, meta) {
                            return `<button class="btn-ver-evidencia btn btn-sm btn-primary"><i class="fas fa-eye"></i></button>
                            <button class="btn-descargar-evidencia btn btn-sm btn-primary" style="margin-left: 5px;"><i class="fas fa-file-download"></i></button>`;
                        }
                    },
                    {
                        title: 'Porcentaje', render: function (data, type, row, meta) {
                            return row.porcentaje + '%';
                        }
                    },
                    { data: 'usuarioEvaluadorDesc', title: 'Evaluador' },
                    {
                        title: 'Evaluar', render: function (data, type, row, meta) {
                            let html = '';

                            if (row.usuarioEvaluadorID == 0) {
                                html = `
                                <div class="radioBtn btn-group">
                                    <a class="btn btn-success notActive" data-toggle="radioAprobado${row.id}" aprobado="1"><i class="fa fa-check"></i>&nbsp;SÍ</a>
                                    <a class="btn btn-danger notActive" data-toggle="radioAprobado${row.id}" aprobado="2"><i class="fa fa-times"></i>&nbsp;NO</a>
                                    <a class="btn btn-primary active" data-toggle="radioAprobado${row.id}" aprobado="0"><i class="fa fa-square"></i>&nbsp;NO EVALUADO</a>
                                </div>`;
                            }

                            return html;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    // { width: '5%', targets: [4, 6] },
                    { width: '10%', targets: [3, 5] },
                    { width: '16%', targets: [8] }
                ]
            });
        }

        function cargarEvidencias() {
            let idEmpresa = $(selectCentroCosto).getEmpresa();
            let strAgrupacion = $(selectCentroCosto).getAgrupador();
            let idAgrupacion;
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }

            let clasificacion = +(selectClasificacion.val());
            // let idEmpresa = idEmpresa;
            // let idAgrupacion = idAgrupacion;
            let requerimientoID = +(selectRequerimiento.val());
            let fechaInicio = inputFechaInicio.val();
            let fechaFin = inputFechaFin.val();
            let estatus = +(selectEstatus.val());

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GetEvidenciasEvaluacion', { clasificacion, idEmpresa, idAgrupacion, requerimientoID, fechaInicio, fechaFin, estatus })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaCapturas, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function guardarEvaluacion() {
            const evaluacion = getInformacionEvaluacion();

            if (evaluacion.length > 0) {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Administrativo/Requerimientos/GuardarEvaluacion', { evaluacion })
                    .always($.unblockUI)
                    .then(response => {
                        cargarEvidencias();

                        if (response.success) {
                            AlertaGeneral(`Éxito`, `Se ha guardado la información.`);
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        cargarEvidencias();
                    }
                    );
            } else {
                AlertaGeneral(`Alerta`, `No se han evaluado evidencias.`);
            }
        }

        function getInformacionEvaluacion() {
            let evaluacion = [];

            tablaCapturas.find('tbody tr').each(function (index, row) {
                let rowData = dtCapturas.row(row).data();
                let aprobado = +($(row).find(`.radioBtn a.active[data-toggle=radioAprobado${rowData.id}]`).attr('aprobado'));

                if (aprobado > 0) {
                    evaluacion.push({
                        id: rowData.id,
                        aprobado: aprobado == 1,
                        calificacion: 0,
                        estatus: true
                    });
                }
            });

            return evaluacion;
        }

        function mostrarArchivoEvidencia(evidenciaID) {
            if (evidenciaID > 0) {
                $.post('/Administrativo/Requerimientos/CargarDatosArchivoEvidencia', { evidenciaID })
                    .then(response => {
                        if (response.success) {
                            $('#myModal').data().ruta = null;
                            $('#myModal').modal('show');
                        } else {
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        }

        function descargarArchivoEvidencia(evidenciaID) {
            if (evidenciaID > 0) {
                location.href = `/Administrativo/Requerimientos/DescargarArchivoEvidencia?evidenciaID=${evidenciaID}`;
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Requerimientos.Evaluacion = new Evaluacion())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();