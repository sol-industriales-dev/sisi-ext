(() => {
    $.namespace('Adminstrativo.Seguridad.CapacitacionSeguridad.AutorizacionCapacitacion');

    AutorizacionCapacitacion = function () {

        // Variables.

        //// Filtros
        const comboCC = $('#comboCC');
        const comboCurso = $('#comboCurso');
        const comboEstatusAutorizacion = $('#comboEstatusAutorizacion');
        const botonBuscar = $('#botonBuscar');
        const botonImprimir = $('#botonImprimir');

        // Tabla capacitaciones
        const tablaCapacitaciones = $('#tablaCapacitaciones');
        let dtTablaCapacitaciones;


        // Autorizar / Rechazar
        const divCapacitaciones = $('#divCapacitaciones');
        const fieldsetReporte = $("#fieldsetReporte");
        const fieldsetAutorizacion = $("#fieldsetAutorizacion");
        const divAutorizantes = $('#divAutorizantes');
        const legendHome = $("#legendHome");
        const report = $("#report");
        const reportAutorizantesIndividual = $('#reportAutorizantesIndividual');
        const textAreaRechazo = $('#textAreaRechazo');
        const modalAutorizar = $('#modalAutorizar');
        const modalRechazar = $('#modalRechazar');
        const modalAutorizarbtnRechazar = $('#modalAutorizarbtnRechazar');
        const modalAutorizarbtnAutorizar = $('#modalAutorizarbtnAutorizar');

        (function init() {
            // Lógica de inicialización.
            $('.select2').select2();

            llenarCombos();

            initTablaCapacitaciones();

            agregarListeners();
        })();

        comboCurso.on('change', function () {
            obtenerAutorizaciones();
        });

        // Métodos.
        function agregarListeners() {
            botonBuscar.click(obtenerAutorizaciones);
            botonImprimir.click(imprimirReporteGeneral);
            legendHome.click(ocultarPaneles);
            modalAutorizarbtnRechazar.unbind().click(rechazarControlAsistencia);
            modalAutorizarbtnAutorizar.unbind().click(autorizarControlAsistencia);
            modalRechazar.on("hide.bs.modal", () => $(textAreaRechazo).val(""));
        }

        function llenarCombos() {

            $.blockUI({ message: 'Cargando...' });
            $.get('/Administrativo/CapacitacionSeguridad/ObtenerComboCC')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        comboCC.append(`<option value="Todos">Todos</option>`);
                        comboCC.append(response.items.map(item => `<option value=${item.Value} >${item.Text}</option>`).join(''));

                        $.blockUI({ message: 'Cargando...' });
                        $.get('/Administrativo/CapacitacionSeguridad/ObtenerComboEstatusAutorizacionCapacitacion')
                            .always($.unblockUI)
                            .then(response => {
                                if (response.success) {
                                    // Operación exitosa.
                                    comboEstatusAutorizacion.append(response.items.map(item => `<option value=${item.Value} >${item.Text}</option>`).join(''));
                                    comboEstatusAutorizacion.append(`<option value=0>Todos</option>`);

                                    obtenerAutorizaciones();
                                } else {
                                    // Operación no completada.
                                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                                }
                            }, error => {
                                // Error al lanzar la petición.
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            }
                            );

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );

            comboCurso.fillCombo('/Administrativo/CapacitacionSeguridad/ObtenerComboCursos', null, false, null);
        }

        function obtenerAutorizaciones() {
            const cc = comboCC.val();
            const curso = +(comboCurso.val());
            const estatus = comboEstatusAutorizacion.val();

            $.blockUI({ message: 'Cargando...' });
            $.get('/Administrativo/CapacitacionSeguridad/ObtenerAutorizaciones', { cc, curso, estatus })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        dtTablaCapacitaciones.clear().rows.add(response.items).draw();
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

        function imprimirReporteGeneral() {
            let curso = +(comboCurso.val());

            if (curso == 0) {
                AlertaGeneral(`Alerta`, `Seleccione un curso.`);
                return;
            }

            $.blockUI({ message: 'Cargando Reporte...' });
            report.attr("src", `/Reportes/Vista.aspx?idReporte=191`);
            document.getElementById('report').onload = function () {
                openCRModal();
                $.unblockUI();
            };
        }

        function obtenerListaAutorizantes(capacitacionID) {

            $.blockUI({ message: 'Cargando datos...' });
            $.get('/Administrativo/CapacitacionSeguridad/ObtenerAutorizantes', { capacitacionID })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        verReporte(capacitacionID, true);
                        establecerPaneles(response.items, capacitacionID);
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

        function ocultarPaneles() {

            fieldsetAutorizacion.hide(500);

            fieldsetAutorizacion.data().controlAsistenciaID = null;
            fieldsetReporte.hide(500);
            divAutorizantes.empty();

            divCapacitaciones.prop('hidden', false);
            divCapacitaciones.show(500);
        }

        function establecerPaneles(listaAutorizantes, controlAsistenciaID) {

            divCapacitaciones.hide(500);
            fieldsetReporte.show(500);
            fieldsetAutorizacion.show(500);
            fieldsetAutorizacion.data().controlAsistenciaID = controlAsistenciaID;
            divAutorizantes.empty();
            divAutorizantes.html("");
            listaAutorizantes.forEach((autorizante, index) => {
                const divPanel = $(`
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="panel panel-default text-center">
                            <div class="panel-heading"><label>${autorizante.nombre}</label></div>
                            <div class="panel-body">
                                <p>${autorizante.puestoDesc}</p>
                                <button ${autorizante.puedeAutorizar ? "id='botonAutorizar'" : ''} class='${autorizante.puedeAutorizar ? '' : 'hidden'} btn btn-success btnPanel'><i class='fa fa-check'></i> Autorizar</button>
                                <button ${autorizante.puedeAutorizar ? "id='botonRechazar'" : ''} class='${autorizante.puedeAutorizar ? '' : 'hidden'} btn btn-danger btnPanel'><i class='fa fa-ban'></i> Rechazar</button>
                            </div>
                            <div class="panel-footer ${autorizante.estatus == 2 ? "panelAutorizado" : "panelPendiente"}">
                                <p>${autorizante.estatus == 2 ? 'Autorizado' : autorizante.estatus == 3 ? "Rechazado" : "Pendiente"}</p>
                                <p>${autorizante.firma ? autorizante.firma : "S/F"}</p>
                            </div>
                        </div>
                    </div>
                </div>
                `);

                divPanel.data().usuarioID = autorizante.usuarioID;

                divAutorizantes.append(divPanel);
            });

            $('#botonAutorizar').unbind().click(() => modalAutorizar.modal('show'));

            $('#botonRechazar').unbind().click(() => {
                modalRechazar.modal('show');
                textAreaRechazo.change(() =>
                    textAreaRechazo.val(sanitizeString(textAreaRechazo.val()))
                );
            });
        }

        function autorizarControlAsistencia() {

            const controlAsistenciaID = fieldsetAutorizacion.data().controlAsistenciaID;

            if (controlAsistenciaID && controlAsistenciaID > 0) {

                modalAutorizar.modal('hide');

                $.blockUI({ message: 'Autorizando...' });
                $.post('/Administrativo/CapacitacionSeguridad/AutorizarControlAsistencia', { controlAsistenciaID })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            if (response.autCompleta) {
                                verReporte(controlAsistenciaID, false, true, false);
                            } else {
                                verReporte(controlAsistenciaID, false, false, false);
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

        function sanitizeString(str) {
            str = str.replace(/[^a-z0-9áéíóúñü \.,]/gim, "");
            return str.trim();
        }

        function rechazarControlAsistencia() {

            const controlAsistenciaID = fieldsetAutorizacion.data().controlAsistenciaID;

            const comentario = textAreaRechazo.val().trim();

            if (comentario == null || comentario.trim().length <= 19) {
                AlertaGeneral("Aviso", "El mensaje de rechazo debe tener un mínimo de 20 caracteres.");
                return;
            }

            modalRechazar.modal('hide');
            $.blockUI({ message: 'Rechazando paquete...' });
            $.post('/Administrativo/CapacitacionSeguridad/RechazarControlAsistencia', { controlAsistenciaID, comentario })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        verReporte(controlAsistenciaID, false, false, true);
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

        function initTablaCapacitaciones() {

            dtTablaCapacitaciones = tablaCapacitaciones.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: true,
                columns: [
                    { data: 'fechaCapacitacion', title: 'Fecha' },
                    { data: 'nombreCurso', title: 'Curso' },
                    { data: 'instructor', title: 'Instructor' },
                    { data: 'ccDesc', title: 'Centro de Costos' },
                    { data: 'estatusDesc', title: 'Estatus' },
                    {
                        data: 'id', title: 'Gestionar', render: (data, type, row) => {
                            if (row.estatus != 2) {
                                return '<button class="btn btn-primary gestionar"><i class="fas fa-eye"></i></button>';
                            } else {
                                return `
                                    <button title="Gestionar autorización" class="btn btn-primary gestionar">
                                        <i class="fas fa-eye"></i>
                                    </button>
                                    <button title="Descargar formato de autorización" class="btn btn-primary descargar">
                                        <i class="fas fa-arrow-alt-circle-down"></i>
                                    </button>`;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: "20%", "targets": [1, 2] },
                ],
                drawCallback: function (settings) {

                    tablaCapacitaciones.find('button.gestionar').click(function () {
                        const controlAsistencia = dtTablaCapacitaciones.row($(this).parents('tr')).data();
                        if (controlAsistencia && controlAsistencia.id && controlAsistencia.id > 0) {
                            obtenerListaAutorizantes(controlAsistencia.id);
                        } else {
                            AlertaGeneral(`Error`, `Ocurrió un error al cargar los datos de autorización.`);
                        }
                    });

                    tablaCapacitaciones.find('button.descargar').click(function () {
                        const controlAsistencia = dtTablaCapacitaciones.row($(this).parents('tr')).data();
                        descargarFormatoAutorizacion(controlAsistencia.id);
                    });
                }
            });
        }

        function descargarFormatoAutorizacion(controlAsistenciaID) {
            $.blockUI({ message: 'Cargando reporte...' });
            var path = `/Reportes/Vista.aspx?idReporte=170&controlAsistenciaID=${controlAsistenciaID}&inMemory=${1}&isCRModal=${false}`;
            reportAutorizantesIndividual.attr("src", path);
            reportAutorizantesIndividual[0].onload = () => {
                $.unblockUI();
                location.href = `DescargarFormatoAutorizacion?controlAsistenciaID=${controlAsistenciaID}`;
            }
        }

        function verReporte(controlAsistenciaID, esReporte, autCompleta, esRechazo) {
            // CRModal = Crystal Reports Modal.
            $.blockUI({ message: 'Cargando reporte...' });
            var path = `/Reportes/Vista.aspx?idReporte=170&controlAsistenciaID=${controlAsistenciaID}&inMemory=${1}&isCRModal=${false}`;
            reportAutorizantesIndividual.attr("src", path);
            reportAutorizantesIndividual[0].onload = () => {

                if (esReporte) {
                    $.unblockUI();
                } else {
                    if (esRechazo) {
                        enviarCorreoRechazo(controlAsistenciaID);
                    } else if (autCompleta) {
                        enviarCorreoAutCompleta(controlAsistenciaID);
                    } else {
                        enviarCorreoAut(controlAsistenciaID);
                    }
                }

            }
        }

        function enviarCorreoRechazo(controlAsistenciaID) {
            $.post('/Administrativo/CapacitacionSeguridad/EnviarCorreoRechazo', { controlAsistenciaID })
                .always(() => {
                    $.unblockUI();
                    ocultarPaneles();
                    obtenerAutorizaciones();
                })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Capacitación rechazada correctamente.`);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `La capacitación fue rechazada correctamente pero ocurrió un error al enviar los correos de notificación.`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function enviarCorreoAut(controlAsistenciaID) {
            $.post('/Administrativo/CapacitacionSeguridad/EnviarCorreoAutorizacion', { controlAsistenciaID })
                .always(() => {
                    $.unblockUI();
                    ocultarPaneles();
                    obtenerAutorizaciones();
                })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Capacitación autorizada correctamente.`);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `La capacitación fue autorizada correctamente pero ocurrió un error al enviar el correo de notificación.`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function enviarCorreoAutCompleta(controlAsistenciaID) {
            $.post('/Administrativo/CapacitacionSeguridad/EnviarCorreoAutorizacionCompleta', { controlAsistenciaID })
                .always(() => {
                    $.unblockUI();
                    ocultarPaneles();
                    obtenerAutorizaciones();
                })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Capacitación completada correctamente.`);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `La capacitación fue completada correctamente pero ocurrió un error al enviar los correos de notificación.`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }
    }

    $(() => Adminstrativo.Seguridad.CapacitacionSeguridad.AutorizacionCapacitacion = new AutorizacionCapacitacion());
})();