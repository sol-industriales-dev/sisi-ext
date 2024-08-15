(() => {
    $.namespace('Administrativo.Evaluacion.Captura');
    Captura = function () {
        //#region Selectores
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const selectEvaluador = $('#selectEvaluador');
        const selectEstatus = $('#selectEstatus');
        const botonBuscar = $('#botonBuscar');
        const botonAgregar = $('#botonAgregar');
        const tablaCapturas = $('#tablaCapturas');
        const modalCaptura = $('#modalCaptura');
        const botonGuardar = $('#botonGuardar');
        const selectActividad = $('#selectActividad');
        const divEvidencia = $('#divEvidencia');
        const inputEvidencia = $('#inputEvidencia');
        const divDescargarEvidencia = $('#divDescargarEvidencia');
        const botonDescargarEvidencia = $('#botonDescargarEvidencia');
        const botonVerEvidencia = $('#botonVerEvidencia');
        const inputComentariosEmpleado = $('#inputComentariosEmpleado');
        const inputFechaActividad = $('#inputFechaActividad');
        //#endregion

        let dtCapturas;
        const ESTATUS = {
            NUEVO: 0,
            EDITAR: 1
        };

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);

        (function init() {
            $.fn.dataTable.moment('DD/MM/YYYY');

            agregarListeners();
            initTablaCapturas();

            inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioAnio);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
            inputFechaActividad.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);

            selectEvaluador.fillCombo('/Administrativo/Evaluacion/GetEvaluadoresCombo', null, false);
            selectActividad.fillCombo('/Administrativo/Evaluacion/GetActividadesCapturaCombo', null, false);

            cargarCapturas();
        })();

        function agregarListeners() {
            botonAgregar.click(() => {
                limpiarModal();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                modalCaptura.modal('show');
            });
            botonGuardar.click(guardarCaptura);
            botonBuscar.click(cargarCapturas);
        }

        function limpiarModal() {
            selectActividad.val('');
            inputEvidencia.val('');
            inputComentariosEmpleado.val('');
        }

        function guardarCaptura() {
            const captura = getInformacionCaptura();

            if (captura.get('evidencia') == 'null') {
                AlertaGeneral(`Alerta`, `Debe anexar evidencia para guardar la captura.`);
                return;
            }

            // let fechaActividad = moment(captura.get('fechaActividad'), "DD/MM/YYYY").toDate();
            // let fechaActividadMaxima = addDays(fechaActividad, 5);

            // if (fechaActual > fechaActividadMaxima) {
            //     AlertaGeneral(`Alerta`,
            //         `La fecha límite para la captura de la actividad es "${$.datepicker.formatDate('dd/mm/yy', fechaActividadMaxima)}".`);
            //     return;
            // }

            modalCaptura.modal('hide');
            limpiarModal();

            $.ajax({
                url: '/Administrativo/Evaluacion/GuardarCaptura',
                data: captura,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                modalCaptura.modal('hide');
                cargarCapturas();

                if (response.success) {
                    AlertaGeneral(`Éxito`, `Se ha guardado la información.`);
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                cargarCapturas();
            }
            );
        }

        function getInformacionCaptura() {
            const data = new FormData();
            const evidencia = inputEvidencia[0].files.length > 0 ? inputEvidencia[0].files[0] : null;

            // data.append('empleadoID', );
            data.append('actividadID', selectActividad.val());
            // data.append('rutaEvidencia', );
            data.append('comentariosEmpleado', inputComentariosEmpleado.val());
            data.append('fechaActividad', inputFechaActividad.val());
            // data.append('fechaCaptura', );
            // data.append('ponderacionActual', );
            // data.append('periodicidadActual', );
            data.append('aplica', true);
            // data.append('evaluadorID', );
            // data.append('comentariosEvaluador', );
            // data.append('aprobado', );
            // data.append('fechaEvaluacion', );
            // data.append('estatus', );
            data.append('evidencia', evidencia);

            return data;
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

                        mostrarArchivo(rowData.id);
                    });

                    tablaCapturas.on('click', '.btn-descargar-evidencia', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();

                        descargarArchivo(rowData.id);
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
                    { data: 'actividadDesc', title: 'Actividad' },
                    // { data: 'comentariosEmpleado', title: 'Comentarios' },
                    { data: 'fechaActividadDesc', title: 'Fecha Actividad' },
                    { data: 'fechaCapturaDesc', title: 'Fecha Captura' },
                    // { data: 'ponderacionActual', title: 'Ponderación' },
                    { data: 'periodicidadActualDesc', title: 'Periodicidad' },
                    { data: 'evaluadorDesc', title: 'Evaluador' },
                    { data: 'comentariosEvaluador', title: 'Comentarios Evaluación' },
                    {
                        title: 'Aprobado', render: function (data, type, row, meta) {
                            if (row.evaluadorID > 0) {
                                return (row.aprobado ? 'SÍ' : 'NO');
                            } else {
                                return '';
                            }
                        }
                    },
                    { data: 'fechaEvaluacionDesc', title: 'Fecha Evaluación' },
                    {
                        title: 'Evidencia', render: function (data, type, row, meta) {
                            return `<button class="btn-ver-evidencia btn btn-sm btn-primary"><i class="fas fa-eye"></i></button>
                            &nbsp; <button class="btn-descargar-evidencia btn btn-sm btn-primary"><i class="fas fa-file-download"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarCapturas() {
            let fechaInicio = inputFechaInicio.val();
            let fechaFin = inputFechaFin.val();
            let evaluadorID = +(selectEvaluador.val());
            let estatus = +(selectEstatus.val());

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/GetCapturasEmpleado', { fechaInicio, fechaFin, evaluadorID, estatus })
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

        function mostrarArchivo(capturaID) {
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

        function descargarArchivo(capturaID) {
            location.href = `/Administrativo/Evaluacion/DescargarArchivoEvidencia?capturaID=${capturaID}`;
        }

        function addDays(date, days) {
            var result = new Date(date);
            result.setDate(result.getDate() + days);
            return result;
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Evaluacion.Captura = new Captura())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();