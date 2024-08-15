(() => {
    $.namespace('CapturaInformePreliminar.Seguridad');
    Seguridad = function () {

        //#region VARIABLES
        const hoy = new Date();
        const tblInformes = $("#tblInformes");
        let dtTblInformes;
        let folio = 0;
        let esEdit = false;
        let informe_id = 0;
        let dtTablaEvidencias;
        let dtTablaEvidenciasRIA;

        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const selectTipoAccidente = $('#selectTipoAccidente');
        const selectSupervisor = $('#selectSupervisor');
        const selectDepartamento = $('#selectDepartamento');
        const selectEstatus = $('#selectEstatus');
        const checkboxExterno = $('#checkboxExterno');
        const tablaEvidencias = $('#tablaEvidencias');
        const tablaEvidenciasRIA = $('#tablaEvidenciasRIA');
        const botonAgregarEvidencia = $('#botonAgregarEvidencia');
        const botonGuardarEvidencias = $('#botonGuardarEvidencias');

        // Reporte
        const report = $("#report");
        const inputArchivoInforme = $('#inputArchivoInforme');
        //#endregion

        //#region PETICIONES
        const getInformacion = (listaDivisiones, listaLineasNegocio, idAgrupacion, idEmpresa, fechaInicio, fechaFin, tipoAccidente, supervisor, departamento, estatus) => {
            return $.post('/Administrativo/IndicadoresSeguridad/GetInformesPreliminares', {
                listaDivisiones, listaLineasNegocio, idAgrupacion, idEmpresa, fechaInicio, fechaFin, tipoAccidente, supervisor, departamento, estatus
            })
        };
        const getInformeById = (id) => { return $.post('/Administrativo/IndicadoresSeguridad/GetInformePreliminarByID', { id }) }
        const getInfoEmpleado = (claveEmpleado) => { return $.post('/Administrativo/IndicadoresSeguridad/GetInfoEmpleado', { claveEmpleado }) };
        const getFolio = (cc) => { return $.post('/Administrativo/IndicadoresSeguridad/GetFolio', { cc }) };
        const guardarRegistro = (informe) => { return $.post('/Administrativo/IndicadoresSeguridad/GuardarInforme', { informe }) };
        const actualizarRegistro = (informe) => { return $.post('/Administrativo/IndicadoresSeguridad/UpdateInforme', { informe }) };
        const enviarCorreos = (informe_id, usuarios) => { return $.post('/Administrativo/IndicadoresSeguridad/EnviarCorreo', { informe_id, usuarios }) };
        //#endregion

        // Función init autoejecutable.
        (function init() {
            setDatosInicio();
            initTable();

            initTablaEvidencias();
            initTablaEvidenciasRIA();

            $('#btnGuardarRegistro').click(guardar)
            $('#btnBuscar').click(() => { buscar(); getDatosGeneralesIncidentes(); })
            $('#btnRegistrar').click(setModalAgregar)
            $('#btnSendMail').click(fnEnviarCorreos);

            checkboxExterno.click(alternarPersonaExterna)

            $('#modalRegistro').on('hide.bs.modal', () => {
                checkboxExterno[0].checked = false;
                checkboxExterno.attr('disabled', false);
                $('#claveEmpleado').attr('disabled', false);
                $('#nombreEmpleado').attr('disabled', true);
                $('#claveSupervisor').attr('disabled', false);
                $("#txtFechaIngreso").attr('disabled', false);
                $('#btnGuardarRegistro').prop('disabled', false);
                $('#selectContratista').val('').change();
                $('#selectContratista').prop('disabled', false);
                $('#divContratista').hide();
                $('#divSubclasificacion').hide();
                $('#divInfoExtraAccidente').hide();
                $('#divParteCuerpo').hide();
                $("input[name=aplicaRIA]").prop('disabled', false);
                clearInfoEmpleado();
                esEdit = false;

                $("#selectClasificacionIncidente").val('').change();
                $("#selectSubclasificacionIncidente").val('').change();
                $("#selectTipoContacto").val('').change();
                $("#selectParteCuerpo").val('').change();
                $("#selectAgenteImplicado").val('').change();
                $('#selectRiesgo').val('').change();
                $('#selectCCRegistro').val('').change();
                $('#selectDepartamentoEmpleado').val('').change();
                $('#selectProcedimientosViolados').val('').change();
            });

            botonAgregarEvidencia.click(() => {

                const filas = dtTablaEvidencias.rows().count();
                if (filas >= 5) {
                    return;
                }

                dtTablaEvidencias.row.add({
                    nombre: "",
                    fecha: "",
                    id: 0,
                    tieneEvidencia: false,
                    puedeEliminar: true
                }).draw();
            })

            botonGuardarEvidencias.click(guardarEvidencias);

            inputArchivoInforme.change(cargarArchivoInforme);

            $('#selectClasificacionIncidente').change(e => {
                const select = $(e.currentTarget);
                const tipoAccidenteID = select.val();

                switch (+tipoAccidenteID) {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 8:
                        $('#divParteCuerpo').show(500);
                        $('#divInfoExtraAccidente').show(500);
                        $('#divSubclasificacion').hide(500);
                        break;
                    case 5:
                        $('#divSubclasificacion').show(500);
                        $('#divParteCuerpo').hide(500);
                        $('#divInfoExtraAccidente').show(500);
                        break;
                    default:
                        $('#divSubclasificacion').hide(500);
                        $('#divInfoExtraAccidente').hide(500);
                        $('#divParteCuerpo').hide(500);
                        break;
                }
            });

            $('#divSubclasificacion').hide();
            $('#divInfoExtraAccidente').hide();
            $('#divParteCuerpo').hide();
        })();

        $('#selectDivision').on('change', function () {
            $("#selectCCFiltros").fillComboSeguridadDivisionLineaNegocio(false, false, $('#selectDivision').val(), $('#selectLineaNegocio').val());
        });

        $('#selectLineaNegocio').on('change', function () {
            $("#selectCCFiltros").fillComboSeguridadDivisionLineaNegocio(false, false, $('#selectDivision').val(), $('#selectLineaNegocio').val());
        });

        //#region CARGAS INICIALES
        function setDatosInicio() {
            $('#selectDivision').fillCombo('/Administrativo/Requerimientos/GetDivisionesCombo', null, false, 'Todos');
            convertToMultiselect('#selectDivision');
            $('#selectLineaNegocio').fillCombo('/Administrativo/Requerimientos/GetLineaNegocioCombo', { division: 0 }, false, 'Todos');
            convertToMultiselect('#selectLineaNegocio');

            var date = new Date();
            date.setMonth(date.getMonth(), 1);
            inputFechaInicio.datepicker().datepicker("setDate", date);
            inputFechaFin.datepicker().datepicker("setDate", new Date());
            selectTipoAccidente.fillCombo('GetTiposAccidentesList', null, false);
            selectSupervisor.fillCombo('LlenarComboSupervisorIncidente', null, false);
            selectEstatus.fillCombo('LlenarComboEstatusIncidente', null, false);
            selectDepartamento.fillCombo('LlenarComboDepartamentoIncidente', null, false);
            $('#selectCCRegistro').fillComboSeguridad(false);
            $("#selectCCFiltros").fillComboSeguridadDivisionLineaNegocio(false, false, $('#selectDivision').val(), $('#selectLineaNegocio').val()); // $('#selectCCFiltros').fillComboSeguridad(false);
            $('#selectClasificacionIncidente').fillCombo('GetTiposAccidentesList', null, false);
            $('#selectSubclasificacionIncidente').fillCombo('GetSubclasificacionesAccidente', null, false);
            $('#selectRiesgo').fillCombo('GetEvaluacionesRiesgo', null, false);
            $('#selectContratista').fillCombo('GetSubcontratistas', null, false);
            $('#selectProcedimientosViolados').fillCombo('getTipoProcedimientosVioladosList', null, false);
            $("#selectDepartamentoEmpleado").fillCombo('GetDepartamentosList', null, false);

            $("#selectTipoContacto").fillCombo('GetTiposContactoList', null, false);
            $("#selectParteCuerpo").fillCombo('GetPartesCuerposList', null, false);
            $('#selectAgenteImplicado').fillCombo('GetAgentesImplicadosList', null, false);

            $('#txtFechaInforme').datepicker("setDate", hoy)
                .datepicker({
                    dateFormat: "dd/mm/yy",
                    maxDate: hoy,
                    showAnim: "slide"
                });

            $('#txtFechaIngreso').datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: hoy,
                showAnim: "slide"
            });

            esEdit = false;

            $('#divContratista').hide();
            getDatosGeneralesIncidentes();
        }

        function getDatosGeneralesIncidentes() {
            let agrupacionID = 0;
            let empresa = 0;

            // if ($('#selectCCFiltros').val() != '') {
            //     agrupacionID = +$('#selectCCFiltros').val();
            //     empresa = +$('#selectCCFiltros').find('option:selected').attr('empresa');
            // }

            axios.post('/Administrativo/IndicadoresSeguridad/GetDatosGeneralesIncidentes', {
                agrupacionID, empresa, fechaInicio: inputFechaInicio.val(), fechaFin: inputFechaFin.val()
            }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    $('#spanTotalAccidentes').html(response.data.totalAccidentes);
                    $('#spanTotalAccidentesInvestigables').html(response.data.totalAccidentesInvestigables);
                    $('#spanPorcentajeAccidentesInvestigablesCompletos').html(response.data.porcentajeAccidentesInvestigablesCompletos);
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function esExtensionInvalida(nombreArchivo) {
            const extensionesNoAceptadas = ["exe", "app", "vb", "scr", "vbe", "vbs"];
            const extension = nombreArchivo.split('.').pop();
            const extensionNoEsValida = extensionesNoAceptadas.filter(x => x === extension);
            return extensionNoEsValida.length > 0;
        }

        function cargarArchivoInforme() {

            const inputArchivo = inputArchivoInforme[0];

            if (inputArchivo.length == 0) {
                return;
            }

            const archivo = inputArchivo.files[0];

            if (esExtensionInvalida(archivo.name)) {
                AlertaGeneral(`Aviso`, `El archivo tiene una extensión inválida.`);
                return;
            }

            const informeID = inputArchivoInforme.data().informeID;
            const esRIA = inputArchivoInforme.data().esRIA;

            const data = new FormData();
            data.append("archivo", archivo);
            data.append("informeID", informeID);
            data.append("esRIA", esRIA);

            $.blockUI({ message: 'Subiendo archivo...' });
            $.ajax({
                url: '/Administrativo/IndicadoresSeguridad/SubirReporteIncidente',
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
                        inputArchivoInforme.val('');
                        AlertaGeneral(`Éxito`, `Archivo cargado correctamente.`);
                        $('#btnBuscar').click();
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

        function initTable() {
            dtTblInformes = $("#tblInformes").DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                aaSorting: [[3, 'desc'], [0, 'desc']],
                rowId: 'id',
                scrollY: "500px",
                scrollCollapse: true,
                searching: false,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excel',
                        exportOptions: {
                            columns: [0, 1, 14, 9, 10, 2, 11, 12, 13, 15] //Your Colume value those you want
                        },
                        text: 'Exportar Excel',
                        filename: 'Gestión de Incidentes',
                        customize: function (xlsx) {
                            var sheet = xlsx.xl.worksheets['sheet1.xml'];
                            var col = $('col', sheet);
                            $(col[0]).attr('width', 5);
                            $(col[1]).attr('width', 50);
                            $(col[2]).attr('width', 12);
                            $(col[3]).attr('width', 20);
                            $(col[4]).attr('width', 15);
                            $(col[5]).attr('width', 40);
                            $(col[6]).attr('width', 55);
                            $(col[7]).attr('width', 55);
                            $(col[8]).attr('width', 10);
                            $(col[9]).attr('width', 55);
                        }

                    },
                ],
                columns: [
                    { data: 'folio', title: 'Folio' },
                    { data: 'proyecto', title: 'Proyecto' },
                    { data: 'empleado', title: 'Empleado' },
                    {
                        data: 'fechaIncidente', title: 'Fecha Incidente',
                        render: (data, type, row) => `<span>${data}</span>
                        <span title="${row.tipoIncidenteDesc}" class="abrevacion label label-primary pull-right">${row.abreviacionTipoIncidente}</span>`
                    },
                    { data: 'estatusAvanceDesc', title: 'Estatus' },
                    {
                        title: "Acciones", sortable: false,
                        render: (data, type, row) =>
                            `<button title="Editar" class="btn-editar-informacion btn btn-sm btn-warning" style="margin-top:5px;" type="button">
                                    <i class="fas fa-pencil-alt"></i>
                            </button>
                            &nbsp;<button title="Enviar accidente por correo" class="btn-mandar-correo btn btn-sm btn-info" style="margin-top:5px;" type="button">
                                <i class="fas fa-envelope"></i>
                            </button>
                            &nbsp;<button title="Evidencias" class="evidencias btn btn-sm btn-primary" style="margin-top:5px;" type="button">
                                <i class="fas fa-camera"></i>
                            </button>
                            &nbsp;<button title="Evidencias RIA" class="evidencias-ria btn btn-sm btn-default" style="margin-top:5px;" type="button">
                                <i class="fas fa-camera"></i>
                            </button>`
                    },
                    {
                        title: "Preliminar", sortable: false,
                        render: (data, type, row) => {
                            let html = ``;

                            if (row.tienePreliminar) {
                                html += `
                                <button title="Descargar Informe Preliminar" class="btn btn-sm btn-primary descargarPreliminar" style="margin-top:5px;" type="button">
                                    <i class="fas fa-file-download"></i>
                                </button>`;
                            } else {
                                html += `
                                <button title="Generar Formato Informe Preliminar" class="btn btn-sm btn-primary generarPreliminar" style="margin-top:5px;" type="button">
                                    <i class="fas fa-print"></i>
                                </button>
                                &nbsp;<button title="Subir Informe Preliminar" class="btn btn-sm btn-primary subirPreliminar" style="margin-top:5px;" type="button">
                                    <i class="fas fa-file-upload"></i>
                                </button>`;
                            }

                            return html;
                        }
                    },
                    {
                        title: "RIA", sortable: false,
                        render: function (data, type, row) {
                            if (row.aplicaRIA == 0) {
                                return `<span class="label label-default"> N / A </span>`;
                            }

                            let html = ``;

                            if (row.terminado) {
                                html +=
                                    `<button title="Ver RIA" class="verFormularioRIA btn btn-sm btn-primary" style="margin-top:5px;" type="button">
                                        <i class="fas fa-eye"></i>
                                    </button>`;

                                if (row.tieneRIA) {
                                    html += `&nbsp;<button title="Descargar RIA" class="btn btn-sm btn-primary descargarRIA" style="margin-top:5px;" type="button"><i class="fas fa-file-download"></i></button>`;
                                } else {
                                    if (row.estatusAvance == 3) {
                                        html += `
                                        &nbsp;<button title="Generar Formato RIA" class="btn btn-sm btn-primary generarRIA" style="margin-top:5px;" type="button">
                                            <i class="fas fa-print"></i>
                                        </button>
                                        &nbsp;<button title="Subir RIA" class="btn btn-sm btn-primary subirRIA" style="margin-top:5px;" type="button">
                                            <i class="fas fa-file-upload"></i>
                                        </button>`;
                                    }
                                }
                            } else {
                                if (row.tienePreliminar) {
                                    html +=
                                        `<button title="Generar RIA" class="formularioRIA btn btn-sm btn-primary" style="margin-top:5px;" type="button">
                                        <i class="fas fa-file-signature"></i>
                                    </button>`;
                                } else {
                                    return `<span class="label label-default"> PENDIENTE </span>`;
                                }
                            }

                            return html;
                        }
                    },
                    {
                        data: 'puedeEliminar',
                        title: "", sortable: false,
                        render: (data, type, row) => data ?
                            `<button title="Eliminar" class="btn-eliminar-informacion btn btn-sm btn-danger" style="margin-top:5px;" type="button">
                                    <i class="fas fa-trash"></i>
                            </button>` : ''
                    },
                    { data: 'tipoIncidenteDesc', title: 'Tipo Incidente', visible: false },
                    { data: 'claveEmpleado', title: 'Cve. Empleado', visible: false },
                    { data: 'descripcionIncidente', title: 'Descripción Incidente', visible: false },
                    { data: 'accionInmediata', title: 'Acción Inmediata', visible: false },
                    {
                        title: 'Riesgo', visible: false,
                        render: function (data, type, row) {

                            let html = ``;

                            switch (row.riesgo) {
                                case 1:
                                    html = "Leve";
                                    break;
                                case 2:
                                    html = "Tolerable";
                                    break;
                                case 3:
                                case 4:
                                    html = "Moderado";
                                    break;
                                case 6:
                                    html = "Crítico";
                                    break;
                                case 9:
                                    html = "Intolerable";
                                    break;
                                default:
                                    html = "N/A";
                                    break;
                            }
                            return html;
                        }
                    },
                    { data: 'fechaIncidente', title: 'Fecha', visible: false },
                    {
                        title: 'Acciones preventivas y correctivas', visible: false,
                        render: function (data, type, row) {

                            var data = "";
                            if (row.MedidasControl.length > 0) {
                                for (var i = 0; i < row.MedidasControl.length; i++) data += (row.MedidasControl[i] + '\r\n');
                            }

                            return data;
                        }
                    },
                ],
                drawCallback: function (settings, json) {


                    $("#tblInformes").find('.btn-editar-informacion').off().click(editarInforme);
                    $("#tblInformes").find('.btn-eliminar-informacion').off().click(e => {
                        const rowData = dtTblInformes.row($(e.currentTarget).closest('tr')).data();

                        AlertaAceptarRechazarNormal(
                            'Confirmar eliminación',
                            `¿Está seguro de eliminar el folio ${rowData.folio} de ${rowData.proyecto}?`,
                            () => eliminarIncidente(rowData.id))
                    });

                    $("#tblInformes").find('.generarPreliminar').off().click(function () {
                        const rowData = dtTblInformes.row($(this).closest('tr')).data();
                        verReporte(rowData.id, true);
                    });

                    $("#tblInformes").find('.subirPreliminar').off().click(function () {
                        const rowData = dtTblInformes.row($(this).closest('tr')).data();

                        inputArchivoInforme.data().informeID = rowData.id;
                        inputArchivoInforme.data().esRIA = false;
                        inputArchivoInforme.click();
                    });

                    $("#tblInformes").find('.descargarPreliminar').off().click(function () {
                        const rowData = dtTblInformes.row($(this).closest('tr')).data();
                        location.href = `DescargarReporte?informeID=${rowData.id}&esRIA=false`;
                    });

                    $("#tblInformes").find('.formularioRIA').off().click(function () {
                        const rowData = dtTblInformes.row($(this).closest('tr')).data();

                        const getUrl = window.location;
                        const baseUrl = getUrl.protocol + "//" + getUrl.host;
                        const urlPlantilla = baseUrl + `/Administrativo/IndicadoresSeguridad/CapturaIncidente?informe=${rowData.id}`;

                        window.open(urlPlantilla, '_blank');
                    });

                    $("#tblInformes").find('.generarRIA').off().click(function () {
                        const rowData = dtTblInformes.row($(this).closest('tr')).data();
                        verReporte(rowData.id, false);
                    });

                    $("#tblInformes").find('.subirRIA').off().click(function () {
                        const rowData = dtTblInformes.row($(this).closest('tr')).data();

                        inputArchivoInforme.data().informeID = rowData.id;
                        inputArchivoInforme.data().esRIA = true;
                        inputArchivoInforme.click();
                    });

                    $("#tblInformes").find('.descargarRIA').off().click(function () {
                        const rowData = dtTblInformes.row($(this).closest('tr')).data();
                        location.href = `DescargarReporte?informeID=${rowData.id}&esRIA=true`;
                    });

                    $("#tblInformes").find('.btn-mandar-correo').off().click(function () {
                        const rowData = dtTblInformes.row($(this).closest('tr')).data();
                        informe_id = rowData.id;
                        fnOpenEnviarCorreo(rowData.idEmpresa, rowData.idAgrupacion);
                    });

                    $("#tblInformes").find('.verFormularioRIA').off().click(function () {
                        const rowData = dtTblInformes.row($(this).closest('tr')).data();

                        const getUrl = window.location;
                        const baseUrl = getUrl.protocol + "//" + getUrl.host;
                        const urlPlantilla = baseUrl + `/Administrativo/IndicadoresSeguridad/CapturaIncidente?informe=${rowData.id}&completo=1`;

                        window.open(urlPlantilla, '_blank');
                    });

                    $("#tblInformes").find('.evidencias').off().click(function () {
                        const rowData = dtTblInformes.row($(this).closest('tr')).data();
                        obtenerEvidenciasInforme(rowData);
                    });

                    $("#tblInformes").find('.evidencias-ria').off().click(function () {
                        const rowData = dtTblInformes.row($(this).closest('tr')).data();
                        obtenerEvidenciasRIA(rowData);
                    });
                }
            });
        }

        function verReporte(informeID, esPreliminar) {
            $.blockUI({ message: 'Generando reporte...' });
            var path = `/Reportes/Vista.aspx?idReporte=${esPreliminar ? 194 : 175}&informeID=${informeID}`;
            report.attr("src", path);
            report[0].onload = () => {
                $.unblockUI();
                openCRModal();
            };
        }

        function obtenerEvidenciasInforme(informe) {
            if (informe == null) {
                return;
            }

            dtTablaEvidencias.clear().draw();
            botonGuardarEvidencias.data().informeID = informe.id;

            $.get('/Administrativo/IndicadoresSeguridad/ObtenerEvidenciasInforme', { informeID: informe.id })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.

                        dtTablaEvidencias.rows.add(response.items).draw();
                        $('#inputInformacionEvidencia').val(`${informe.proyecto.trim()} - ${informe.folio} - ${informe.fechaIncidente}`);
                        $('#modalEvidencias').modal('show');
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

        function obtenerEvidenciasRIA(informe) {
            if (informe == null) {
                return;
            }

            dtTablaEvidenciasRIA.clear().draw();
            // botonGuardarEvidencias.data().informeID = informe.id;

            $.get('/Administrativo/IndicadoresSeguridad/ObtenerEvidenciasRIA', { informeID: informe.id })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.

                        dtTablaEvidenciasRIA.rows.add(response.items).draw();
                        $('#inputInformacionEvidenciaRIA').val(`${informe.proyecto.trim()} - ${informe.folio} - ${informe.fechaIncidente}`);
                        $('#modalEvidenciasRIA').modal('show');
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, response.message);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function mostrarEvidencia(evidenciaID) {

            if (evidenciaID == null || evidenciaID == 0) {
                return;
            }

            $.post('CargarDatosEvidencia', { evidenciaID })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        $('#myModal').data().ruta = null;
                        $('#myModal').modal('show');
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

        function mostrarEvidenciaRIA(evidenciaID) {
            if (evidenciaID == null || evidenciaID == 0) {
                return;
            }

            $.post('CargarDatosEvidenciaRIA', { evidenciaID })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        $('#myModal').data().ruta = null;
                        $('#myModal').modal('show');
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

        function initTablaEvidencias() {
            dtTablaEvidencias = tablaEvidencias.DataTable({
                language: dtDicEsp,
                paging: false,
                searching: false,
                order: [[2, "desc"]],
                info: false,
                columns: [
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'fecha', title: 'Fecha' },
                    {
                        data: 'tieneEvidencia', title: 'Evidencia', render: (data, type, row) =>
                            row.tieneEvidencia ?
                                `<button title="Descargar evidencia" class="btn btn-primary descargarEvidencia"><i class="fas fa-arrow-alt-circle-down"></i></button>
                                <button title="Ver evidencia" class="btn btn-primary verEvidencia"><i class="fas fa-eye"></i></button>` :
                                `<input type="file" accept="application/pdf, image/*" class="form-control inputEvidencia"></input>`
                    },
                    {
                        data: 'id', title: 'Eliminar', render: (data, type, row) => row.puedeEliminar ?
                            `<button class="btn btn-danger botonEliminarEvidencia"><i class="fas fa-trash"></i></button>` : ''
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {

                    tablaEvidencias.find('button.descargarEvidencia').click(function () {
                        const evidencia = dtTablaEvidencias.row($(this).parents('tr')).data();
                        location.href = `DescargarEvidenciaInforme?evidenciaID=${evidencia.id}`;
                    });

                    tablaEvidencias.find('button.verEvidencia').click(function () {
                        const evidencia = dtTablaEvidencias.row($(this).parents('tr')).data();
                        mostrarEvidencia(evidencia.id);
                    });

                    tablaEvidencias.find('button.botonEliminarEvidencia').off().click(function () {

                        const evidencia = dtTablaEvidencias.row($(this).parents('tr')).data();

                        if (evidencia.tieneEvidencia) {
                            AlertaAceptarRechazarNormal(
                                'Confirmar eliminación',
                                '¿Está seguro de eliminar esta evidencia?',
                                () => eliminarEvidencia(evidencia.id)
                            );
                        } else {
                            dtTablaEvidencias.row($(this).closest('tr')).remove().draw();
                        }
                    });
                }
            });
        }

        function initTablaEvidenciasRIA() {
            dtTablaEvidenciasRIA = tablaEvidenciasRIA.DataTable({
                language: dtDicEsp,
                paging: false,
                searching: false,
                order: [[2, "desc"]],
                info: false,
                columns: [
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'fecha', title: 'Fecha' },
                    {
                        data: 'tieneEvidencia', title: 'Evidencia', render: (data, type, row) =>
                            row.tieneEvidencia ?
                                `<button title="Descargar evidencia" class="btn btn-primary descargarEvidencia"><i class="fas fa-arrow-alt-circle-down"></i></button>
                                <button title="Ver evidencia" class="btn btn-primary verEvidencia"><i class="fas fa-eye"></i></button>` :
                                `<input type="file" accept="application/pdf, image/*" class="form-control inputEvidencia"></input>`
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {
                    tablaEvidenciasRIA.find('button.descargarEvidencia').click(function () {
                        const evidencia = dtTablaEvidenciasRIA.row($(this).parents('tr')).data();
                        location.href = `DescargarEvidenciaRIA?evidenciaID=${evidencia.id}`;
                    });

                    tablaEvidenciasRIA.find('button.verEvidencia').click(function () {
                        const evidencia = dtTablaEvidenciasRIA.row($(this).parents('tr')).data();
                        mostrarEvidenciaRIA(evidencia.id);
                    });

                    tablaEvidenciasRIA.find('button.botonEliminarEvidencia').off().click(function () {
                        const evidencia = dtTablaEvidenciasRIA.row($(this).parents('tr')).data();

                        if (evidencia.tieneEvidencia) {
                            AlertaAceptarRechazarNormal(
                                'Confirmar eliminación',
                                '¿Está seguro de eliminar esta evidencia?',
                                () => eliminarEvidencia(evidencia.id)
                            );
                        } else {
                            dtTablaEvidenciasRIA.row($(this).closest('tr')).remove().draw();
                        }
                    });
                }
            });
        }

        function eliminarEvidencia(evidenciaID) {
            $.post('/Administrativo/IndicadoresSeguridad/EliminarEvidencia', { evidenciaID })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Evidencia eliminada correctamente.`);
                        $('#modalEvidencias').modal('hide');
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

        function guardarEvidencias() {

            let data = new FormData();

            const informeID = botonGuardarEvidencias.data().informeID;

            data.append('informeID', informeID);

            let numEvidencias = 0;

            tablaEvidencias.find('input.inputEvidencia').toArray().forEach(x => {
                const file = x.files[0];
                data.append('evidencias', file);
                numEvidencias++;
            });

            if (numEvidencias == 0) {
                AlertaGeneral(`Aviso`, `No hay nada que guardar.`);
                return;
            } else if (tablaEvidencias.find('input.inputEvidencia').toArray().some(x => x.files.length == 0)) {
                AlertaGeneral(`Aviso`, `Faltan evidencias por cargar.`);
                return;
            }

            $.ajax({
                url: 'GuardarEvidencias',
                data,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
            }).then(response => {
                if (response.success) {
                    // Operación exitosa.
                    $('#modalEvidencias').modal('hide');
                    AlertaGeneral(`Aviso`, `Evidencias cargadas correctamente`);
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

        function editarInforme() {
            const rowData = dtTblInformes.row($(this).closest('tr')).data();
            const getUrl = window.location;
            const baseUrl = getUrl.protocol + "//" + getUrl.host;
            const urlPlantilla = baseUrl + `/Administrativo/IndicadoresSeguridad/CapturaIncidente?informe=${rowData.id}&informeEditar=1`;

            window.open(urlPlantilla, '_blank');

            // getInformeById(rowData.id).done(function (response) {
            //     if (response.success) {
            //         const data = response.informacion;

            //         const fechaInforme = new Date(moment(data.fechaInforme, "DD-MM-YYYY").format());
            //         const fechaIngresoEmpleado = new Date(moment(data.fechaIngresoEmpleado, "DD-MM-YYYY").format());
            //         const fechaIncidente = new Date(moment(data.fechaIncidenteComplete))

            //         esEdit = true;

            //         $("#btnGuardarRegistro").val(rowData["id"]);
            //         $("#selectCCRegistro").val(data.cc).change();
            //         $("#selectCCRegistro").attr('disabled', true);

            //         $('#claveEmpleado').val(data.claveEmpleado);
            //         $("#nombreEmpleado").val(rowData.empleado);
            //         checkboxExterno[0].checked = data.esExterno;
            //         checkboxExterno.attr('disabled', true);

            //         $("#claveEmpleadoInformo").val(data.personaInformo).blur();

            //         $("#claveEmpleado").attr('disabled', true)
            //         $("#claveEmpleadoInformo").attr('disabled', true)

            //         $("#nombreEmpleado").attr('disabled', true)
            //         $("#nombreEmpleadoInformo").attr('disabled', true)

            //         $("#txtFechaInforme").datepicker("setDate", fechaInforme);
            //         $('#txtFechaIncidente').data("DateTimePicker").date(fechaIncidente);

            //         $("#txtFechaIngreso").attr('disabled', true);
            //         $("#txtFechaIngreso").datepicker("setDate", fechaIngresoEmpleado);

            //         $('#txtPuestoEmpleado').val(data.puestoEmpleado)
            //         $("#txtPuestoEmpleado").attr('disabled', true)

            //         if (data.departamento_id > 0) {
            //             $('#selectDepartamentoEmpleado').val(data.departamento_id).change();
            //         }

            //         $('#claveSupervisor').attr('disabled', true).val(data.claveSupervisor);
            //         $("#txtSupervisorEmpleado").attr('disabled', true).val(data.supervisorEmpleado);

            //         if (data.esExterno) {
            //             $('#selectContratista').val(data.claveContratista).change();
            //             $('#divContratista').show();
            //         } else {
            //             $('#selectContratista').val('').change();
            //         }

            //         $("#selectContratista").attr('disabled', true);

            //         $('#txtTipoLesion').val(data.tipoLesion)

            //         $('#txtDescripcionIncidente').val(data.descripcionIncidente)
            //         $('#txtAccionInmediata').val(data.accionInmediata);

            //         $("#selectClasificacionIncidente").val(data.tipoAccidente_id).change();
            //         switch (+data.tipoAccidente_id) {
            //             case 5:

            //                 $('#divSubclasificacion').show();
            //                 if (data.subclasificacionID > 0) {
            //                     $("#selectSubclasificacionIncidente").val(data.subclasificacionID).change()
            //                 }

            //                 $('#divInfoExtraAccidente').show();
            //                 if (data.tipoContacto_id > 0) {
            //                     $("#selectTipoContacto").val(data.tipoContacto_id).change()
            //                 }
            //                 if (data.agenteImplicado_id > 0) {
            //                     $("#selectAgenteImplicado").val(data.agenteImplicado_id).change()
            //                 }

            //                 break;
            //             case 1:
            //             case 2:
            //             case 3:
            //             case 4:
            //             case 8:
            //                 $('#divInfoExtraAccidente').show();
            //                 $('#divParteCuerpo').show();
            //                 if (data.tipoContacto_id > 0) {
            //                     $("#selectTipoContacto").val(data.tipoContacto_id).change()
            //                 }
            //                 if (data.parteCuerpo_id > 0) {
            //                     $("#selectParteCuerpo").val(data.parteCuerpo_id).change()
            //                 }
            //                 if (data.agenteImplicado_id > 0) {
            //                     $("#selectAgenteImplicado").val(data.agenteImplicado_id).change()
            //                 }
            //                 break;
            //             default:
            //                 $('#divSubclasificacion').hide();
            //                 $('#divInfoExtraAccidente').hide();
            //                 $('#divParteCuerpo').hide();
            //                 break;
            //         }

            //         $('#selectRiesgo').val(data.riesgo).change()
            //         $("#selectProcedimientosViolados").val(data.procedimientosViolados).change();

            //         $("input[name=aplicaRIA][value='" + data.aplicaRIA + "']").prop('checked', true);

            //         if (data.terminado) {
            //             $("input[name=aplicaRIA]").prop('disabled', true);
            //             $('#btnGuardarRegistro').prop('disabled', true);
            //         }

            //         $('#modalRegistro').modal('show');
            //     }
            // })
        }

        function eliminarIncidente(id) {

            if (id == null || id <= 0) {
                return;
            }

            $.post('/Administrativo/IndicadoresSeguridad/EliminarIncidente', { id })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Incidente eliminado correctamente.`);
                        $('#btnBuscar').click();
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

        function setModalAgregar() {
            const getUrl = window.location;
            const baseUrl = getUrl.protocol + "//" + getUrl.host;
            const urlPlantilla = baseUrl + `/Administrativo/IndicadoresSeguridad/CapturaIncidente?informe=0&informeEditar=0`;

            window.open(urlPlantilla, '_blank');

            // $("#txtFechaInforme")
            //     .datepicker({
            //         dateFormat: "dd/mm/yy",
            //         maxDate: hoy,
            //         showAnim: "slide"
            //     }).datepicker("setDate", hoy);

            // $("#txtPuestoEmpleado").val("");
            // $("#txtFechaIngreso").val("");
            // $("#selectDepartamentoEmpleado").val("");
            // $("#txtSupervisorEmpleado").val("");
            // $("#txtFechaIncidente").val("");
            // $("#txtTipoLesion").val("");
            // $("#txtDescripcionIncidente").val("");
            // $("#txtAccionInmediata").val("");
            // $("#claveEmpleado").val("");
            // $("#claveSupervisor").val("");
            // $("#nombreEmpleado").val("");
            // $("#claveEmpleadoInformo").val("");
            // $("#nombreEmpleadoInformo").val("");
            // $('#selectCCRegistro').val('');

            // $("#claveEmpleado").attr('disabled', false)
            // $("#selectCCRegistro").attr('disabled', false)
            // $("#txtPuestoEmpleado").attr('disabled', false)
            // $("#selectDepartamentoEmpleado").attr('disabled', false)
            // $("#claveSupervisor").attr('disabled', false)
            // $("#txtSupervisorEmpleado").attr('disabled', true)
            // $("#claveEmpleadoInformo").attr('disabled', false)

            // $("input[name=aplicaRIA][value=1]").prop('checked', true).change()

            // $('#modalRegistro').modal({
            //     backdrop: 'static',
            //     keyboard: false
            // });

            // $("#selectCCRegistro").focus();
        }

        function clearInfoEmpleado() {
            $('#txtPuestoEmpleado').val('')
            $('#txtFechaIngreso').val('')
            $('#selectDepartamentoEmpleado').val('')
            // $('#claveSupervisor').val('')
            // $('#txtSupervisorEmpleado').val('')
            $('#nombreEmpleado').val('')
        }

        //#endregion

        //#region METODOS GENERALES
        function alternarPersonaExterna() {
            const checked = this.checked;

            const inputClaveEmpleado = $('#claveEmpleado');
            const inputNombreEmpleado = $('#nombreEmpleado');

            const inputClaveSupervisor = $('#claveSupervisor');
            const inputNombreSupervisor = $('#txtSupervisorEmpleado');

            inputClaveEmpleado.attr('disabled', checked);
            inputNombreEmpleado.attr('disabled', !checked);

            inputClaveSupervisor.attr('disabled', checked);
            inputNombreSupervisor.attr('disabled', !checked);

            if (checked) {
                $('#divContratista').show(500);
                inputClaveEmpleado.val('');
                inputClaveSupervisor.val('');
            } else {
                $('#divContratista').hide(500);
                inputNombreEmpleado.val('');
                inputNombreSupervisor.val('');
                $('#selectContratista').val('').change();
            }
        }

        function fnOpenEnviarCorreo(idEmpresa, idAgrupacion) {
            $('#slMCorreos').fillCombo('GetUsuariosCCSigoPlan', { idEmpresa, idAgrupacion }, false, "Todos");
            convertToMultiselect("#slMCorreos");
            $('#modalCorreos').modal("show");
        }

        function fnEnviarCorreos() {
            $("#modalCorreos").modal('hide');

            enviarCorreos(informe_id, getValoresMultiples("#slMCorreos")).done(function (response) {
                if (response.success) {
                    AlertaGeneral("Confirmación", "Correos enviados correctamente");
                    informe_id = 0
                } else {
                    AlertaGeneral("Alerta", "¡Ocurrio un problema al enviar a los siguientes usuarios!<br/>" + response.obj);
                }
            })
        }

        function buscar() {
            let idEmpresa = $('#selectCCFiltros').val() != '' ? $(selectCCFiltros).getEmpresa() : 0;
            let strAgrupacion = $('#selectCCFiltros').val() != '' ? $(selectCCFiltros).getAgrupador() : 0;

            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }

            let listaDivisiones = $('#selectDivision').val();
            let listaLineasNegocio = $('#selectLineaNegocio').val();

            getInformacion(listaDivisiones, listaLineasNegocio, idAgrupacion, idEmpresa, inputFechaInicio.val(), inputFechaFin.val(), selectTipoAccidente.val(), selectSupervisor.val(), selectDepartamento.val(), selectEstatus.val()).done(function (response) {
                dtTblInformes.clear();
                if (response.success) {
                    dtTblInformes.rows.add(response.items);
                }
                dtTblInformes.draw();
            });
        }

        function validarGuardar() {
            let countInvalidos = 0

            if ($("#selectCCRegistro").val() == "") {
                countInvalidos++;
            }

            $('input.validar').each(function () {
                if ($(this).val() == "") {
                    countInvalidos++;
                }
            });

            if ($("#selectDepartamentoEmpleado").val().trim() == "") {
                countInvalidos++;
            }

            if ($("#txtTipoLesion").val().trim() == "" || $("#txtDescripcionIncidente").val().trim() == "" || $("#txtAccionInmediata").val().trim() == "") {
                countInvalidos++;
            }

            const esExterno = $('#checkboxExterno')[0].checked;

            if (esExterno && ($('#nombreEmpleado').val().trim() == "" || $('#txtSupervisorEmpleado').val().trim() == "")) {
                countInvalidos++;
            } else if (esExterno == false &&
                ($('#claveEmpleado').val().trim() == "" ||
                    $('#nombreEmpleado').val().trim() == "" ||
                    $('#claveSupervisor').val().trim() == "" ||
                    $('#txtSupervisorEmpleado').val().trim() == "")) {
                countInvalidos++;
            }

            const tipoAccidenteID = $("#selectClasificacionIncidente").val();

            if (tipoAccidenteID == "" || $("#selectRiesgo").val() == "") {
                countInvalidos++;
            } else if (tipoAccidenteID == 5 && ($("#selectSubclasificacionIncidente").val() == "" || $("#selectTipoContacto").val() == "" || $("#selectAgenteImplicado").val() == "")) {
                countInvalidos++;
            } else if (tipoAccidenteID == 1 ||
                tipoAccidenteID == 2 ||
                tipoAccidenteID == 3 ||
                tipoAccidenteID == 4 ||
                tipoAccidenteID == 8) {
                if ($("#selectTipoContacto").val() == "" || $("#selectParteCuerpo").val() == "" || $("#selectAgenteImplicado").val() == "") {
                    countInvalidos++;
                }
            }

            return countInvalidos;
        }

        function getObjRegistro() {
            let nombreExterno = null;
            let claveContratista = 0;


            const esExterno = $('#checkboxExterno')[0].checked;

            if (esExterno) {
                nombreExterno = $('#nombreEmpleado').val();

                if ($('#selectContratista').val() != "") {
                    claveContratista = $('#selectContratista').val();
                }
            }

            const tipoAccidenteID = $('#selectClasificacionIncidente').val();

            const registroInformacion = {
                folio,
                claveEmpleado: $('#claveEmpleado').val(),
                personaInformo: $("#claveEmpleadoInformo").val(),
                cc: '',
                idAgrupacion: selectCCRegistro.getAgrupador(),
                idEmpresa: selectCCRegistro.getEmpresa(),
                fechaInforme: $('#txtFechaInforme').val(),
                fechaIncidente: $('#txtFechaIncidente').val(),
                fechaIngresoEmpleado: $('#txtFechaIngreso').val(),
                puestoEmpleado: $('#txtPuestoEmpleado').val(),
                departamentoEmpleado: $('#selectDepartamentoEmpleado option:selected').text(),
                departamento_id: $('#selectDepartamentoEmpleado').val(),
                claveSupervisor: $('#claveSupervisor').val(),
                supervisorEmpleado: $('#txtSupervisorEmpleado').val(),
                tipoLesion: $('#txtTipoLesion').val(),
                descripcionIncidente: $('#txtDescripcionIncidente').val().trim(),
                accionInmediata: $('#txtAccionInmediata').val().trim(),
                aplicaRIA: $('input[name=aplicaRIA]:checked').val() == "1" ? true : false,
                tipoAccidente_id: tipoAccidenteID,
                subclasificacionID: $('#selectClasificacionIncidente').val() == 5 ? $('#selectSubclasificacionIncidente').val() : 0,
                procedimientosViolados: $("#selectProcedimientosViolados").val(),
                riesgo: $('#selectRiesgo').val(),
                esExterno,
                nombreExterno,
                claveContratista,
                procedimientosViolados: $("#selectProcedimientosViolados").val()
                    .filter(value => value != "" && value > 0)
                    .map(value => ({ id: value }))
            }

            if (tipoAccidenteID == 1 ||
                tipoAccidenteID == 2 ||
                tipoAccidenteID == 3 ||
                tipoAccidenteID == 4 ||
                tipoAccidenteID == 8) {
                registroInformacion.tipoContacto_id = $("#selectTipoContacto").val();
                registroInformacion.parteCuerpo_id = $("#selectParteCuerpo").val();
                registroInformacion.agenteImplicado_id = $("#selectAgenteImplicado").val();
            } else if (tipoAccidenteID == 5) {
                registroInformacion.tipoContacto_id = $("#selectTipoContacto").val();
                registroInformacion.agenteImplicado_id = $("#selectAgenteImplicado").val();
            }

            return registroInformacion;
        }

        function getObjRegistroEdit() {
            let nombreExterno = null
                , claveContratista = 0
                , id = $("#btnGuardarRegistro").val();

            const esExterno = $('#checkboxExterno')[0].checked;

            if (esExterno) {
                nombreExterno = $('#nombreEmpleado').val();

                if ($('#selectContratista').val() != "") {
                    claveContratista = $('#selectContratista').val();
                }
            }

            const tipoAccidenteID = $('#selectClasificacionIncidente').val();

            const registroInformacion = {
                id: id,
                departamentoEmpleado: $('#selectDepartamentoEmpleado option:selected').text(),
                departamento_id: $('#selectDepartamentoEmpleado').val(),
                fechaInforme: $('#txtFechaInforme').val(),
                fechaIncidente: $('#txtFechaIncidente').val(),
                fechaIngresoEmpleado: $('#txtFechaIngreso').val(),
                tipoLesion: $('#txtTipoLesion').val(),
                descripcionIncidente: $('#txtDescripcionIncidente').val().trim(),
                accionInmediata: $('#txtAccionInmediata').val().trim(),
                aplicaRIA: $('input[name=aplicaRIA]:checked').val() == "1",
                tipoAccidente_id: tipoAccidenteID,
                subclasificacionID: tipoAccidenteID == 5 ? $('#selectSubclasificacionIncidente').val() : 0,
                riesgo: $('#selectRiesgo').val(),
                esExterno,
                nombreExterno,
                claveContratista,
                procedimientosViolados: $("#selectProcedimientosViolados").val()
                    .filter(value => value != "" && value > 0)
                    .map(value => ({ id: value }))
            }

            if (tipoAccidenteID == 1 ||
                tipoAccidenteID == 2 ||
                tipoAccidenteID == 3 ||
                tipoAccidenteID == 4 ||
                tipoAccidenteID == 8) {
                registroInformacion.tipoContacto_id = $("#selectTipoContacto").val();
                registroInformacion.parteCuerpo_id = $("#selectParteCuerpo").val();
                registroInformacion.agenteImplicado_id = $("#selectAgenteImplicado").val();
            } else if (tipoAccidenteID == 5) {
                registroInformacion.tipoContacto_id = $("#selectTipoContacto").val();
                registroInformacion.agenteImplicado_id = $("#selectAgenteImplicado").val();
            }

            return registroInformacion
        }

        function guardar() {
            if (validarGuardar() > 0) {
                AlertaGeneral('Aviso', 'Debe ingresar todos los datos')
            } else {
                if (esEdit) {
                    actualizarRegistro(getObjRegistroEdit()).done(function (response) {
                        $('#modalRegistro').modal('hide');
                        if (response.success) {
                            AlertaGeneral('Aviso', 'Incidente actualizado correctamente.')
                            // tblInformes.ajax.reload(null, false);
                            setDatosInicio();
                            $('#btnBuscar').click();
                            esEdit = false;
                            folio = 0
                        } else {
                            AlertaGeneral('Aviso', response.error)
                        }
                    })
                } else {
                    guardarRegistro(getObjRegistro()).done(function (response) {
                        $('#modalRegistro').modal('hide');
                        if (response.success) {
                            AlertaGeneral('Éxito', 'Incidente registrado correctamente.')
                            // tblInformes.ajax.reload(null, false);
                            setDatosInicio();
                            $('#btnBuscar').click();
                            folio = 0;
                        } else {
                            AlertaGeneral('Error', response.error);
                        }
                    })
                }
            }
        }

        //#endregion

        //#region EVENT'S CHANGE, CLICKS
        $('.select2').select2();

        $('.claveEmpleado').on('blur', function () {

            if ($(this).val() == null || $(this).val() == "") {
                return;
            }

            getInfoEmpleado($(this).val()).done(function (response) {
                if (response.success) {
                    const fechaIngreso = moment(response.empleadoInfo.antiguedadEmpleadoStr).format("DD/MM/YYYY");
                    $('#txtFechaIngreso').val(fechaIngreso)
                    $('#txtPuestoEmpleado').val(response.empleadoInfo.puestoEmpleado)
                    $('#nombreEmpleado').val(response.empleadoInfo.nombreEmpleado)
                } else {
                    clearInfoEmpleado()
                }
            })
        })

        $('#claveSupervisor').on('blur', function () {

            if ($(this).val() == null || $(this).val() == "") {
                return;
            }

            getInfoEmpleado($(this).val()).done(function (response) {
                if (response.success) {
                    $('#txtSupervisorEmpleado').val(response.empleadoInfo.nombreEmpleado)
                } else {
                    $('#txtSupervisorEmpleado').val('');
                }
            })
        })

        $('.claveEmpleadoInformo').on('blur', function () {
            getInfoEmpleado($(this).val()).done(function (response) {
                if (response.success) {
                    $('#nombreEmpleadoInformo').val(response.empleadoInfo.nombreEmpleado)
                } else {
                    $('#nombreEmpleadoInformo').val('')
                }

            })
        })

        $('#txtFechaIncidente')
            .datetimepicker({
                format: 'DD/MM/YYYY h:mm a',
                maxDate: hoy
            });

        $('#selectCCRegistro').change(function () {

            if ($(this).val() == "") {
                return;
            };

            getFolio($(this).val()).done(function (response) {
                if (response.success) {
                    folio = +response.folio;
                } else {
                    AlertaGeneral(`Error`, `${response.error}`);
                    $('#modalRegistro').modal('hide');
                }
            })
        })

        //#endregion

    }
    $(document).ready(() => CapturaInformePreliminar.Seguridad = new Seguridad())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();