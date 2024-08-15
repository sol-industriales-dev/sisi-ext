(() => {
    $.namespace('Administrativo.Evaluacion.Evaluacion');
    Evaluacion = function () {
        //#region Selectores
        const selectEmpleado = $('#selectEmpleado');
        const selectEstatus = $('#selectEstatus');
        const botonBuscar = $('#botonBuscar');
        const tablaCapturas = $('#tablaCapturas');
        const modalEvaluar = $('#modalEvaluar');
        const botonDescargarEvidencia = $('#botonDescargarEvidencia');
        const botonVerEvidencia = $('#botonVerEvidencia');
        const inputComentariosEmpleado = $('#inputComentariosEmpleado');
        const inputComentariosEvaluador = $('#inputComentariosEvaluador');
        const botonGuardar = $('#botonGuardar');
        const radioBtn = $('.radioBtn a');
        //#endregion

        let dtCapturas;

        (function init() {
            $.fn.dataTable.moment('DD/MM/YYYY');
            agregarListeners();
            initTablaCapturas();

            selectEmpleado.fillCombo('/Administrativo/Evaluacion/GetEmpleadosCombo', null, false);

            cargarCapturasEvaluador();
        })();

        function agregarListeners() {
            botonGuardar.click(guardarEvaluacion);
            botonBuscar.click(cargarCapturasEvaluador);
            botonDescargarEvidencia.click(descargarArchivoEvidencia);
            botonVerEvidencia.click(mostrarArchivoEvidencia);
            radioBtn.click(clickRadios);
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
                    tablaCapturas.on('click', '.btn-evaluar', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();

                        //Si ya se evaluó se toma el valor correspondiente, si todavía no se evalúa se toma true por default.
                        let valorAprobacion = rowData.evaluadorID > 0 ? (rowData.aprobado ? 'true' : 'false') : 'true';

                        botonGuardar.data().capturaID = rowData.id;
                        inputComentariosEmpleado.val(rowData.comentariosEmpleado);
                        inputComentariosEvaluador.val('');
                        $(`a[data-toggle="radioAprobado"]`).not(`[aprobado="${valorAprobacion}"]`).removeClass('active').addClass('notActive');
                        $(`a[data-toggle="radioAprobado"][aprobado="${valorAprobacion}"]`).removeClass('notActive').addClass('active');
                        botonGuardar.css('display', 'inline-block');
                        modalEvaluar.modal('show');
                    });

                    tablaCapturas.on('click', '.btn-consultar', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();

                        //Si ya se evaluó se toma el valor correspondiente, si todavía no se evalúa se toma true por default.
                        let valorAprobacion = rowData.evaluadorID > 0 ? (rowData.aprobado ? 'true' : 'false') : 'true';

                        botonGuardar.data().capturaID = rowData.id;
                        inputComentariosEmpleado.val(rowData.comentariosEmpleado);
                        inputComentariosEvaluador.val(rowData.comentariosEvaluador);
                        $(`a[data-toggle="radioAprobado"]`).not(`[aprobado="${valorAprobacion}"]`).removeClass('active').addClass('notActive');
                        $(`a[data-toggle="radioAprobado"][aprobado="${valorAprobacion}"]`).removeClass('notActive').addClass('active');
                        botonGuardar.css('display', 'none');
                        modalEvaluar.modal('show');
                    });
                },
                createdRow: function (row, rowData) {
                    if (rowData.evaluadorID > 0) {
                        if (rowData.aprobado) {
                            $(row).addClass('renglonAprobado');
                        } else {
                            $(row).addClass('renglonNoAprobado');
                        }
                    }
                },
                columns: [
                    { data: 'empleadoDesc', title: 'Empleado' },
                    { data: 'actividadDesc', title: 'Actividad' },
                    { data: 'comentariosEmpleado', title: 'Comentarios' },
                    { data: 'fechaCapturaDesc', title: 'Fecha Captura' },
                    { data: 'ponderacionActual', title: 'Ponderación' },
                    { data: 'periodicidadActualDesc', title: 'Periodicidad' },
                    // {
                    //     title: 'Evidencia', render: function (data, type, row, meta) {
                    //         return `<button class="btn-ver-evidencia btn btn-sm btn-primary"><i class="fas fa-eye"></i></button>
                    //         &nbsp; <button class="btn-descargar-evidencia btn btn-sm btn-primary"><i class="fas fa-file-download"></i></button>`;
                    //     }
                    // },
                    {
                        title: 'Evaluar', render: function (data, type, row, meta) {
                            if (row.evaluadorID == 0) {
                                return `<button class="btn-evaluar btn btn-xs btn-primary"><i class="fas fa-edit"></i></button>`;
                            } else {
                                return '<button class="btn-consultar btn btn-xs btn-default"><i class="fas fa-eye"></i></button>';
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarCapturasEvaluador() {
            let empleadoID = +(selectEmpleado.val());
            let estatus = +(selectEstatus.val());

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/GetCapturasEvaluador', { empleadoID, estatus })
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

            modalEvaluar.modal('hide');
            limpiarModal();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/GuardarEvaluacion', { evaluacion })
                .always($.unblockUI)
                .then(response => {
                    modalEvaluar.modal('hide');
                    cargarCapturasEvaluador();

                    if (response.success) {
                        AlertaGeneral(`Éxito`, `Se ha guardado la información.`);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    cargarCapturasEvaluador();
                }
                );
        }

        function limpiarModal() {
            botonGuardar.data().id = 0;
            $(`a[data-toggle="radioAprobado"]`).not(`[aprobado="true"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="radioAprobado"][aprobado="true"]`).removeClass('notActive').addClass('active');
            inputComentariosEmpleado.val('');
            inputComentariosEvaluador.val('');
        }

        function getInformacionEvaluacion() {
            return {
                id: botonGuardar.data().capturaID,
                aprobado: $(`a.active[data-toggle=radioAprobado]`).attr('aprobado') == 'true',
                comentariosEvaluador: inputComentariosEvaluador.val()
            };
        }

        function mostrarArchivoEvidencia() {
            let capturaID = botonGuardar.data().capturaID;

            if (capturaID > 0) {
                $.post('/Administrativo/Evaluacion/CargarDatosArchivoEvidencia', { capturaID })
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

        function descargarArchivoEvidencia() {
            let capturaID = botonGuardar.data().capturaID;

            if (capturaID > 0) {
                location.href = `/Administrativo/Evaluacion/DescargarArchivoEvidencia?capturaID=${capturaID}`;
            }
        }

        function clickRadios() {
            let seleccion = $(this).attr('aprobado');

            $(`a[data-toggle="radioAprobado"]`).not(`[aprobado="${seleccion}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="radioAprobado"][aprobado="${seleccion}"]`).removeClass('notActive').addClass('active');
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Evaluacion.Evaluacion = new Evaluacion())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();