(() => {
    $.namespace('EvaluacionSubcontratista.Gestion');
    Gestion = function () {
        //#region Selectores
        //#region Filtro
        const comboBoxProyecto = $('#comboBoxProyecto');
        const comboBoxSubcontratista = $('#comboBoxSubcontratista');
        const botonBuscar = $('#botonBuscar');
        //#endregion

        //#region Tabla evaluaciones
        const tablaEvaluaciones = $('#tablaEvaluaciones');
        //#endregion

        //#region modal seguimiento de firmas
        const modalSeguimientoFirmas = $('#modalSeguimientoFirmas');
        const modalSeguimientoFirmasTitulo = $('#modalSeguimientoFirmasTitulo');
        const tablaSeguimientoFirmas = $('#tablaSeguimientoFirmas');
        //#endregion

        //#region firma
        const seccionParaFirmar = $('#seccionParaFirmar');
        const inputNombreFirmante = $('#inputNombreFirmante');
        const botonCancelarFirma = $('#botonCancelarFirma');
        const botonGuardarFirma = $('#botonGuardarFirma');

        let canvasFirma = document.getElementById('canvasFirma');
        var signaturePad = new SignaturePad(canvasFirma, {
            backgroundColor: 'rgba(255, 255, 255, 0)',
            penColor: 'black'
        });
        //#endregion
        //#endregion

        (function init() {
            initComboBoxProyecto();
            initComboBoxSubcontratista();
            initTablaEvaluaciones();
            initTablaSeguimientoFirmas();
            initSignaturePad();


        })();

        //#region Filtro tabla evaluaciones
        //#region Funciones
        function initComboBoxProyecto() {
            comboBoxProyecto.fillComboBox('ObtenerProyectosParaFiltro', null, '-- Seleccionar --');
            comboBoxProyecto.select2();
        }

        function initComboBoxSubcontratista() {
            comboBoxSubcontratista.fillComboBox(null, null, '-- Seleccionar --');
            comboBoxSubcontratista.select2();
        }

        function cargarComboBoxSubcontratista(proyecto) {
            comboBoxSubcontratista.fillComboBox('ObtenerSubcontratistasParaFiltro', { proyecto }, '-- Seleccionar --');
        }

        function obtenerEvaluacionesSubcontratistas(proyecto, subcontratistaId) {
            $.get('ObtenerEvaluacionesSubcontratistas',
                {
                    proyecto,
                    subcontratistaId
                }).then(response => {
                    if (response.success) {
                        console.log(response.items)
                        addRows(tablaEvaluaciones, response.items);
                    } else {
                        swal('Aviso', response.message, 'warning');
                    }
                }, error => {
                    swal('Aviso', `Ocurrió un error al lanzar la petición al servidor:\n${error.status} - ${error.statusText}`, 'error');
                });
        }
        //#endregion

        //#region Eventos
        comboBoxProyecto.on('change', function () {
            const proyectoSeleccionado = $(this).val();

            if (proyectoSeleccionado) {
                cargarComboBoxSubcontratista(proyectoSeleccionado);
            } else {
                initComboBoxSubcontratista();
            }
        });

        botonBuscar.on('click', function () {
            let proyecto = null;
            let subcontratistaId = null;

            if (comboBoxProyecto.val()) {
                proyecto = comboBoxProyecto.val();
            }

            if (comboBoxSubcontratista.val()) {
                subcontratistaId = comboBoxSubcontratista.val();
            }

            obtenerEvaluacionesSubcontratistas(proyecto, subcontratistaId);
        });
        //#endregion
        //#endregion

        //#region modal seguimiento de firmas
        //#region Funciones
        function obtenerEstatusFirmantes(evaluacionId) {
            return $.get('ObtenerEstatusFirmantes', { evaluacionId }).then(response => {
                if (response.success) {
                    return response;
                } else {
                    swal('Aviso', response.message, 'warning');
                }
            }, error => {
                swal('Aviso', `Ocurrió un error al lanzar la petición al servidor:\n${error.status} - ${error.statusText}`, 'error');
            });
        }
        //#endregion

        //#region eventos
        modalSeguimientoFirmas.on('shown.bs.modal', function () {
            tablaSeguimientoFirmas.DataTable().columns.adjust().draw();
        });
        //#endregion
        //#endregion

        //#region firma
        //#region Funciones
        function obtenerFirmante(evaluacionId) {
            return $.get('ObtenerFirmante', { evaluacionId }).then(response => {
                if (response.success) {
                    return response;
                } else {
                    swal('Aviso', response.message, 'warning');
                }
            }, error => {
                swal('Aviso', `Ocurrió un error al lanzar la petición al servidor:\n${error.status} - ${error.statusText}`, 'error');
            });
        }

        function guardarFirma(informacionFirmaDigitalDTO) {
            return $.post('GuardarFirma', { firma: informacionFirmaDigitalDTO }).then(response => {
                if (response.success) {
                    return response;
                } else {
                    swal('Aviso', response.message, 'warning');
                }
            }, error => {
                swal('Aviso', `Ocurrió un error al lanzar la petición al servidor:\n${error.status} - ${error.statusText}`, 'error');
            });
        }

        function initSignaturePad() {
            function resizeCanvas() {
                canvasFirma.width = seccionParaFirmar.width();
                canvasFirma.height = seccionParaFirmar.height();
                signaturePad.clear();
            }

            window.onresize = resizeCanvas();
            resizeCanvas();
        }

        function llenarCamposParaFirmar(datos) {
            limpiarCamposParaFirmar();

            inputNombreFirmante.val(datos.puestoDelFirmante + ' ' + datos.nombreCompletoFirmante);
            botonGuardarFirma.data('evaluacion-id', datos.evaluacionId);
            botonGuardarFirma.data('firmante-id', datos.firmanteId);
        }

        function mostrarCampoParaFirmar() {
            seccionParaFirmar.show();

            canvasFirma.width = seccionParaFirmar.width();
            canvasFirma.height = document.body.clientHeight;
            signaturePad.clear();
        }

        function limpiarCamposParaFirmar() {
            inputNombreFirmante.val('');
            botonGuardarFirma.removeData('evaluacion-id');
            botonGuardarFirma.removeData('firmante-id');
        }
        //#endregion

        //#region eventos
        botonCancelarFirma.on('click', function () {
            seccionParaFirmar.hide();
            limpiarCamposParaFirmar();
        });

        botonGuardarFirma.on('click', function () {
            if (signaturePad.isEmpty()) {
                swal('Aviso', 'Favor de firmar', 'warning');
            } else {
                let firmaBase64 = signaturePad.toDataURL('image/png');

                const informacionFirma = {
                    evaluacionId: botonGuardarFirma.data('evaluacion-id'),
                    firmanteId: botonGuardarFirma.data('firmante-id'),
                    firmaDigitalBase64: firmaBase64
                }

                guardarFirma(informacionFirma).done(function (response) {
                    if (response && response.success) {
                        botonCancelarFirma.trigger('click');

                        swal('Confirmación', 'La firma se guardó correctamente', 'success');
                    }
                });
            }
        });
        //#endregion
        //#endregion

        //#region Tablas
        function initTablaEvaluaciones() {
            tablaEvaluaciones.DataTable({
                order: [[0, 'asc']],
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[10, 25, 50, -1], [10, 25, 50, 'Todos']],
                dom: 'frtip',
                columns: [
                    { data: 'numeroContrato', title: 'CONTRATO', className: 'dt-center' },
                    { data: 'nombreSubcontratista', title: 'SUBCONTRATISTA', className: 'dt-head-center' },
                    { data: 'nombreProyecto', title: 'PROYECTO', className: 'dt-head-center' },
                    { data: 'nombreEvaluacion', title: 'NOMBRE EVALUACIÓN', className: 'dt-head-center' },
                    {
                        data: null, title: 'ESTATUS FIRMAS', className: 'dt-center',
                        render: function (data, type, row) {
                            return row.firmado ? 'FIRMADA' : 'PENDIENTE';
                        }
                    },
                    {
                        data: null, title: 'OPCIONES', className: 'dt-head-center',
                        render: function (data, type, row) {
                            let opcionFirma = `
                                <button class="btn btn-danger descargarEvaluacionActualPdf"  title='Descargar Evaluacion Actual en PDF'> <i class="fas fa-file-pdf"></i></button>
                                <button class="btn btn-primary dtBtnSeguimientoFirma" title="Consultar estatus firmas"><i class="fas fa-list-ol"></i></button>`;
                            let opcionConsulta = `<button class="btn btn-success dtBtnFirmar" title="Firmar"><i class="fas fa-file-signature"></i></button>`;

                            if (row.firmado || !row.elUsuarioPuedeFirmar) {
                                return opcionFirma;
                            } else {
                                return opcionFirma + ' ' + opcionConsulta;
                            }
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaEvaluaciones.on('click', '.dtBtnSeguimientoFirma', function () {
                        const rowData = tablaEvaluaciones.DataTable().row($(this).closest('tr')).data();

                        modalSeguimientoFirmasTitulo.text(`SEGUIMIENTO DE FIRMAS - ${rowData.numeroContrato}`);

                        obtenerEstatusFirmantes(rowData.evaluacionId).done(function (response) {
                            if (response && response.success) {
                                addRows(tablaSeguimientoFirmas, response.items);
                                modalSeguimientoFirmas.modal('show');
                            }
                        })
                    });

                    tablaEvaluaciones.on('click', '.dtBtnFirmar', function () {
                        const rowData = tablaEvaluaciones.DataTable().row($(this).closest('tr')).data();

                        obtenerFirmante(rowData.evaluacionId).done(function (response) {
                            if (response && response.success) {
                                llenarCamposParaFirmar(response.items);
                                mostrarCampoParaFirmar();
                            }
                        });
                    });

                    tablaEvaluaciones.on("click", ".descargarEvaluacionActual", function () {
                        let rowData = tablaEvaluaciones.DataTable().row($(this).closest('tr')).data();
                        axios.post('creaExcelito', { idAsignacion: rowData.evaluacionId })
                            .catch(o_O => AlertaGeneral(o_O.message))
                            .then(response => {
                                let { success, items } = response.data;
                                if (success) {
                                    location.href = `realizarExcel`;
                                }
                            });
                    });

                    tablaEvaluaciones.on("click", ".descargarEvaluacionActualPdf", function () {
                        let rowData = tablaEvaluaciones.DataTable().row($(this).closest('tr')).data();
                        var path = `/Reportes/Vista.aspx?idReporte=249&idAsignacion=${rowData.evaluacionId}`;
                        $("#report").attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            openCRModal();
                        };
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: '5%', targets: [0] }
                ],
            });
        }

        function initTablaSeguimientoFirmas() {
            tablaSeguimientoFirmas.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[10, 25, 50, -1], [10, 25, 50, 'Todos']],
                dom: 'frtip',

                columns: [
                    {
                        data: 'puesto',
                        title: 'PUESTO',
                        className: 'dt-head-center'
                    },
                    {
                        data: 'nombreCompleto',
                        title: 'NOMBRE',
                        className: 'dt-head-center'
                    },
                    {
                        data: 'fechaAutorizacion',
                        title: 'FECHA AUTORIZACIÓN',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            if (data != null) {
                                return moment(data).format('DD/MM/YYYY');
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'estatusFirma',
                        title: 'ESTADO DE FIRMA',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            return data ? 'Firmada' : 'Pendiente';
                        }
                    }
                ]
            });
        }

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }
        //#endregion
    }

    $(document).ready(() => EvaluacionSubcontratista.Gestion = new Gestion())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();