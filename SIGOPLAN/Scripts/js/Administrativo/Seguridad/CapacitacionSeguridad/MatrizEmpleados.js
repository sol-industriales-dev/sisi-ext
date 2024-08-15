(() => {
    $.namespace('Adminstrativo.Seguridad.CapacitacionSeguridad.MatrizEmpleados');

    MatrizEmpleados = function () {

        // Variables.

        //// Filtros
        const comboCplan = $('#comboCplan');
        const comboArr = $('#comboArr');
        const comboPuesto = $('#comboPuesto');
        const botonBuscar = $('#botonBuscar');
        const idEmpresa = $('#idEmpresa');
        // Modal cursos
        const modalCursos = $('#modalCursos');
        const inputEmpleado = $('#inputEmpleado');
        const inputPuesto = $('#inputPuesto');
        const inputCC = $('#inputCC');
        const inputFechaIngreso = $('#inputFechaIngreso');
        const spanPorcentaje = $('#spanPorcentaje');
        const botonLicencia = $('#botonLicencia');
        const botonExpediente = $('#botonExpediente');
        const tablaClasificaciones = $('#tablaClasificaciones');
        let dtTablaClasificaciones;
        const tablaCursos = $('#tablaCursos');
        let dtTablaCursos;

        // Modal extracurriculares
        const modalExtracurriculares = $('#modalExtracurriculares');

        const inputExtracurricularEmpleado = $('#inputExtracurricularEmpleado');
        const botonAgregarExtracurricular = $('#botonAgregarExtracurricular');
        const divCamposExtracurricular = $('#divCamposExtracurricular');
        const inputNombreExtracurricular = $('#inputNombreExtracurricular');
        const inputDuracionExtracurricular = $('#inputDuracionExtracurricular');
        const inputFechaExtracurricularInicio = $('#inputFechaExtracurricularInicio');
        const inputFechaExtracurricularFin = $('#inputFechaExtracurricularFin');
        const inputEvidenciaExtracurricular = $('#inputEvidenciaExtracurricular');
        const botonGuardarExtracurricular = $('#botonGuardarExtracurricular');

        const tablaExtracurriculares = $('#tablaExtracurriculares');
        let dtTablaExtracurriculares;

        // Tabla empleados
        const tablaEmpleados = $('#tablaEmpleados');
        let dtTablaEmpleados;

        // Reporte
        const report = $("#report");

        const esCreador = $('#inputEsCreador').val() == "True";

        (function init() {
            // Lógica de inicialización.

            llenarCombos();

            initTablaEmpleados();

            agregarListeners();

            initTablaCursos();
            initTablaExtracurriculares();
            initTablaClasificaciones();

            divCamposExtracurricular.hide();

            initDatepickersExtracurricular();

        })();

        // Métodos.
        function initDatepickersExtracurricular() {

            var fechaActual = new Date();

            inputFechaExtracurricularInicio.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": fechaActual
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", fechaActual);

            inputFechaExtracurricularFin.datepicker({
                "dateFormat": "dd/mm/yy",
            }).datepicker("option", "showAnim", "slide")
            //     .datepicker("setDate", fechaActual);
            // inputFechaExtracurricularFin.val('');
        }

        function agregarListeners() {
            botonBuscar.click(obtenerEmpleados);
            botonExpediente.click(() => verReporte(false));
            botonLicencia.click(() => verReporte(true));
            botonAgregarExtracurricular.click(() => divCamposExtracurricular.show(1000));
            botonGuardarExtracurricular.click(guardarExtracurricular);
        }

        function llenarCombos() {

            comboPuesto.fillCombo('/Administrativo/CapacitacionSeguridad/GetPuestos', null, false, 'Todos');
            comboCplan.fillCombo('/Administrativo/CapacitacionSeguridad/ObtenerComboCCEnKontrol', { empresa: 1 }, false, 'Todos');
            comboArr.fillCombo('/Administrativo/CapacitacionSeguridad/ObtenerComboCCEnKontrol', { empresa: 2 }, false, 'Todos');
            convertToMultiselect('#comboPuesto');
            convertToMultiselect('#comboCplan');
            convertToMultiselect('#comboArr');
        }

        function obtenerEmpleados() {
            const ccsCplan = getValoresMultiples('#comboCplan');
            const ccsArr = getValoresMultiples('#comboArr');
            const puestos = getValoresMultiples('#comboPuesto');

            if (ccsCplan.length == 0 && ccsArr.length == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar un centro de costo de por lo menos una empresa.`);
                return;
            }

            $.blockUI({ message: 'Cargando empleados...' });
            $.post('/Administrativo/CapacitacionSeguridad/ObtenerEmpleados', { ccsCplan, ccsArr, puestos })
                .always($.unblockUI)
                .then(response => {
                    if (response) {
                        // Operación exitosa.
                        dtTablaEmpleados.clear().rows.add(response).draw();

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function initTablaEmpleados() {

            dtTablaEmpleados = tablaEmpleados.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: true,
                order: [[4, "desc"]],
                columns: [
                    { data: 'claveEmpleado', title: 'No. Empleado' },
                    { data: 'nombreEmpleado', title: 'Nombre' },
                    { data: 'puestoEmpleado', title: 'Puesto' },
                    { data: 'cc', title: 'CC' },
                    {
                        data: 'id', title: 'Detalles', render: (data, type, row) =>
                            '<button class="btn btn-primary verCursos"><i class="fas fa-info-circle"></i></button>'
                    },
                    {
                        data: 'id', title: 'Extracurricular', render: (data, type, row) =>
                            '<button class="btn btn-warning extracurricular"><i class="fas fa-trophy"></i></button>'
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: "25%", "targets": [1, 2, 3] },
                ],
                drawCallback: function (settings) {

                },
                initComplete: function (settings, json) {
                    tablaEmpleados.on('click', '.verCursos', function () {
                        const empleado = dtTablaEmpleados.row($(this).parents('tr')).data();
                        if (empleado && empleado.claveEmpleado) {
                            cargarCursosEmpleado(empleado);
                        }
                    });
                    // tablaEmpleados.find('button.verCursos').click(function () {

                    // });

                    tablaEmpleados.on('click', '.extracurricular', function () {
                        const empleado = dtTablaEmpleados.row($(this).parents('tr')).data();
                        if (empleado && empleado.claveEmpleado) {
                            modalExtracurriculares.modal('show');
                            inputExtracurricularEmpleado.val(empleado.nombreEmpleado);
                            botonGuardarExtracurricular.data().claveEmpleado = empleado.claveEmpleado;
                            cargarExtracurricularesEmpleado(empleado.claveEmpleado);
                        }
                    })
                    // tablaEmpleados.find('button.extracurricular').click(function () {
                    // });
                }
            });
        }

        function initTablaCursos() {

            dtTablaCursos = tablaCursos.DataTable({
                language: dtDicEsp,
                // paging: false,
                searching: false,
                order: [[5, "desc"]],
                info: false,
                columns: [
                    {
                        data: 'nombre', title: 'Curso', render: (data, type, row) =>
                            `<p>${row.claveCurso}</p>
                            <p>${row.nombre}</p>`
                    },
                    { data: 'clasificacion', title: 'Clasificación' },
                    { data: 'instructor', title: 'Instructor' },
                    { data: 'lugar', title: 'Lugar' },
                    { data: 'fechaVigencia', title: 'Fecha expiración' },
                    {
                        data: 'id', title: 'Formato asistencia', render: (data, type, row) =>
                            `<button class="btn btn-primary botonDescargarFormato"><i class="fas fa-arrow-alt-circle-down"></i> Descargar</button>`
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {

                    tablaCursos.find('button.botonDescargarFormato').click(function () {
                        const controlAsistencia = dtTablaCursos.row($(this).parents('tr')).data();
                        descargarControlAsistencia(controlAsistencia.id);
                    });

                }
            });
        }

        function initTablaExtracurriculares() {

            dtTablaExtracurriculares = tablaExtracurriculares.DataTable({
                language: dtDicEsp,
                paging: false,
                searching: false,
                order: [[4, "asc"]],
                info: false,
                columns: [
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'duracion', title: 'Duración' },
                    { data: 'fecha', title: 'Fecha' },
                    { data: 'fechaExpiracion', title: 'Vigente hasta' },
                    {
                        data: 'id', title: 'Evidencia', render: (data, type, row) =>
                            row.tieneEvidencia ?
                                `<button class="btn btn-primary botonDescargarEvidencia"><i class="fas fa-arrow-alt-circle-down"></i> Descargar</button>` : 'N/A'
                    },
                    {
                        data: 'id', title: 'Eliminar', render: (data, type, row) => esCreador ? `<button class="btn btn-danger botonEliminarEvidencia"><i class="fas fa-trash"></i></button>` : ''
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {

                    tablaExtracurriculares.find('button.botonDescargarEvidencia').click(function () {
                        const extracurricular = dtTablaExtracurriculares.row($(this).parents('tr')).data();
                        descargarEvidenciaExtracurricular(extracurricular.id);
                    });

                    tablaExtracurriculares.find('button.botonEliminarEvidencia').click(function () {
                        const extracurricular = dtTablaExtracurriculares.row($(this).parents('tr')).data();
                        const claveEmpleado = botonGuardarExtracurricular.data().claveEmpleado;
                        AlertaAceptarRechazarNormal(
                            'Confirmar eliminación',
                            '¿Está seguro de eliminar esta información extracurricular?',
                            () => eliminarEvidenciaExtracurricular(extracurricular.id, claveEmpleado))
                    });

                }
            });
        }

        function eliminarEvidenciaExtracurricular(extracurricularID, claveEmpleado) {
            $.post('/Administrativo/CapacitacionSeguridad/EliminarEvidenciaExtracurricular', { extracurricularID })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Extracurricular eliminada correctamente.`);
                        cargarExtracurricularesEmpleado(claveEmpleado);
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

        function descargarEvidenciaExtracurricular(extracurricularID) {
            location.href = `DescargarEvidenciaExtracurricular?extracurricularID=${extracurricularID}`;
        }

        function descargarControlAsistencia(controlAsistenciaID) {
            location.href = `DescargarListaControlAsistencia?controlAsistenciaID=${controlAsistenciaID}`;
        }

        function initTablaClasificaciones() {

            dtTablaClasificaciones = tablaClasificaciones.DataTable({
                language: dtDicEsp,
                paging: false,
                searching: false,
                info: false,
                columns: [
                    { data: 'clasificacionDesc', title: 'Clasificacion' },
                    { data: 'cursosAplican', title: '# aplican' },
                    { data: 'cursosVigentes', title: '# completos' },
                    { data: 'porcentajeCapacitacion', title: 'Porcentaje' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarCursosEmpleado(empleado) {
            const { claveEmpleado, puestoID, empresa } = empleado;
            $.blockUI({ message: 'Consultado información sobre el empleado...' });
            $.get('/Administrativo/CapacitacionSeguridad/ObtenerCursosEmpleado', { claveEmpleado, puestoID })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.

                        dtTablaCursos.clear().rows.add(response.items).draw();
                        dtTablaClasificaciones.clear().rows.add(response.listaPorcentajes).draw();

                        inputEmpleado.val(empleado.nombreEmpleado);
                        inputPuesto.val(empleado.puestoEmpleado);
                        inputCC.val(empleado.cc);
                        inputFechaIngreso.val(formatearFecha(empleado.fechaAlta));
                        spanPorcentaje.html(response.porcentajeGeneral);

                        if (response.tieneExpediente) {
                            botonExpediente.show();
                            botonLicencia.show();
                            botonExpediente.data().claveEmpleado = claveEmpleado;
                            botonExpediente.data().empresa = empresa;
                        } else {
                            botonExpediente.hide();
                            botonLicencia.hide();
                            botonExpediente.data().claveEmpleado = null;
                            botonExpediente.data().empresa = null;
                        }

                        modalCursos.modal('show');
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

        function cargarExtracurricularesEmpleado(claveEmpleado) {
            $.blockUI({ message: 'Consultado información extracurricular sobre el empleado...' });
            $.get('/Administrativo/CapacitacionSeguridad/ObtenerExtracurricularesEmpleado', { claveEmpleado })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        dtTablaExtracurriculares.clear().rows.add(response.items).draw();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        dtTablaExtracurriculares.clear().draw();
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    dtTablaExtracurriculares.clear().draw();
                }
                );
        }

        function guardarExtracurricular() {

            const claveEmpleado = botonGuardarExtracurricular.data().claveEmpleado;
            const nombre = inputNombreExtracurricular.val().trim();
            const duracion = inputDuracionExtracurricular.val().trim();
            const fecha = inputFechaExtracurricularInicio.val().trim();

            if (claveEmpleado == null || claveEmpleado == "" || nombre == "" || duracion == "" || fecha == "") {
                AlertaGeneral(`Aviso`, `Debe llenar los campos marcado con *`);
                return;
            }

            const fechaFin = inputFechaExtracurricularFin.val().trim();
            const evidencia = inputEvidenciaExtracurricular[0].files[0];

            const data = new FormData();
            data.append("claveEmpleado", claveEmpleado);
            data.append("nombre", nombre);
            data.append("duracion", duracion);
            data.append("fecha", fecha);
            data.append("fechaFin", fechaFin);
            data.append("evidencia", evidencia);

            $.blockUI({ message: 'Subiendo evidencia...' });
            $.ajax({
                url: '/Administrativo/CapacitacionSeguridad/SubirEvidenciaExtracurricular',
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
                        AlertaGeneral(`Éxito`, `Extracurricular creada correctamente.`);
                        divCamposExtracurricular.hide(500);
                        limpiarCamposDivExtracurricular();
                        cargarExtracurricularesEmpleado(claveEmpleado);
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

        function limpiarCamposDivExtracurricular() {
            inputNombreExtracurricular.val('');
            inputDuracionExtracurricular.val('');
            inputFechaExtracurricularInicio.val('');
            inputFechaExtracurricularFin.val('');
            inputEvidenciaExtracurricular.val('');
        }

        function formatearFecha(fecha) {
            try {
                fecha = fecha.split(' ')[0];
                const datos = fecha.split('/');
                const temp = datos[1];
                datos[1] = datos[0];
                datos[0] = temp;
                return datos.join('/');
            } catch (error) {
                return fecha;
            }
        }

        function descargarExpediente() {
            const claveEmpleado = botonExpediente.data().claveEmpleado;
            if (claveEmpleado && claveEmpleado > 0) {
                location.href = `DescargarExpedienteEmpleado?claveEmpleado=${claveEmpleado}`;
            }
        }

        function verReporte(esReporte) {
            const claveEmpleado = botonExpediente.data().claveEmpleado;
            const empresa = botonExpediente.data().empresa;
            $.blockUI({ message: 'Generando licencia...' });
            var path = `/Reportes/Vista.aspx?idReporte=171&claveEmpleado=${claveEmpleado}&empresa=${empresa}&isCRModal=${true}&inMemory=${1}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                if (esReporte) {
                    openCRModal();
                } else {
                    descargarExpediente();
                }
            };
        }


    }
    $(() => Adminstrativo.Seguridad.CapacitacionSeguridad.MatrizEmpleados = new MatrizEmpleados());
})();