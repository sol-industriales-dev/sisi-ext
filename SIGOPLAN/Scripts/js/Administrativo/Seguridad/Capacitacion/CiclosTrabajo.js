(() => {
    $.namespace('Administrativo.Seguridad.CiclosTrabajo');
    CiclosTrabajo = function () {
        //#region Selectores
        const inputPrivilegio = $('#inputPrivilegio');

        //#region Ciclos de Trabajo
        const selectCentroCosto = $('#selectCentroCosto');
        const selectCicloTrabajo = $('#selectCicloTrabajo');
        const selectArea = $('#selectArea');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const botonBuscar = $('#botonBuscar');
        const botonNuevoRegistro = $('#botonNuevoRegistro');
        const tablaCiclosRegistro = $('#tablaCiclosRegistro');
        const modalRegistroCiclo = $('#modalRegistroCiclo');
        const checkboxTodosCiclos = $('#checkboxTodosCiclos');

        const botonCiclosRequeridos = $('#botonCiclosRequeridos');
        const modalCiclosRequeridos = $('#modalCiclosRequeridos');
        const tablaCiclosRequeridos = $('#tablaCiclosRequeridos');
        const botonGuardarCiclosRequeridos = $('#botonGuardarCiclosRequeridos');

        //#region Modal Registro Ciclo
        const selectCentroCostoRegistroCiclo = $('#selectCentroCostoRegistroCiclo');
        const inputFechaRegistroCiclo = $('#inputFechaRegistroCiclo');
        const selectAreaRegistroCiclo = $('#selectAreaRegistroCiclo');
        const selectCicloRegistroCiclo = $('#selectCicloRegistroCiclo');
        const inputRevisorRegistroCiclo = $('#inputRevisorRegistroCiclo');
        const inputColaboradorRegistroCiclo = $('#inputColaboradorRegistroCiclo');
        const selectTipoCicloRegistroCiclo = $('#selectTipoCicloRegistroCiclo');
        const inputCalificacionRegistroCiclo = $('#inputCalificacionRegistroCiclo');
        const inputEconomicoRegistroCiclo = $('#inputEconomicoRegistroCiclo');
        const checkBoxAcreditoCiclo = $('#checkBoxAcreditoCiclo');
        const checkBoxRequiereRetroalimentacion = $('#checkBoxRequiereRetroalimentacion');
        const botonAccionRegistroCiclo = $('#botonAccionRegistroCiclo');
        const modalAccionRequerida = $('#modalAccionRequerida');
        const textAreaAccionRequerida = $('#textAreaAccionRequerida');
        const selectMetodoAccionRequerida = $('#selectMetodoAccionRequerida');
        const selectCapacitacionAccionRequerida = $('#selectCapacitacionAccionRequerida');
        const selectCapacitacionInteresados = $('#selectCapacitacionInteresados');
        const selectCapacitacionAreaSeguimiento = $('#selectCapacitacionAreaSeguimiento');
        const botonCancelarAccionRequerida = $('#botonCancelarAccionRequerida');
        const botonGuardarAccionRequerida = $('#botonGuardarAccionRequerida');
        const tablaRevisiones = $('#tablaRevisiones');
        const inputObservacionesRevisorRegistroCiclo = $('#inputObservacionesRevisorRegistroCiclo');
        const inputAccionesTomadasRegistroCiclo = $('#inputAccionesTomadasRegistroCiclo');
        const inputObservacionesLiderRegistroCiclo = $('#inputObservacionesLiderRegistroCiclo');
        const botonAgregarPropuesta = $('#botonAgregarPropuesta');
        const tablaPropuestasMejora = $('#tablaPropuestasMejora');
        const botonGuardarRegistroCiclo = $('#botonGuardarRegistroCiclo');


        const btnNuevoRegistro = $('#btnNuevoRegistro');
        const mdlRegistro = $('#mdlRegistro');
        const tblCiclosTrabajo = $('#tblCiclosTrabajo');
        let dtCiclo;
        //#endregion
        //#endregion

        //#region Seguimiento Acciones
        const selectCentroCostoSeguimiento = $('#selectCentroCostoSeguimiento');
        const selectSeguimiento = $('#selectSeguimiento');
        const inputFechaInicioSeguimiento = $('#inputFechaInicioSeguimiento');
        const inputFechaFinSeguimiento = $('#inputFechaFinSeguimiento');
        const botonBuscarSeguimiento = $('#botonBuscarSeguimiento');
        const divTablaSeguimientoAcciones = $('#divTablaSeguimientoAcciones');
        const botonExcelAcciones = $('#botonExcelAcciones');
        const inputTotalAcciones = $('#inputTotalAcciones');
        const inputSolventadasAcciones = $('#inputSolventadasAcciones');
        const inputProcesoAcciones = $('#inputProcesoAcciones');
        const inputPorcentajeAcciones = $('#inputPorcentajeAcciones');
        const tablaSeguimientoAcciones = $('#tablaSeguimientoAcciones');
        const botonGuardarSeguimientoAcciones = $('#botonGuardarSeguimientoAcciones');
        const divTablaSeguimientoPropuestas = $('#divTablaSeguimientoPropuestas');
        const inputTotalPropuestas = $('#inputTotalPropuestas');
        const inputSolventadasPropuestas = $('#inputSolventadasPropuestas');
        const inputProcesoPropuestas = $('#inputProcesoPropuestas');
        const inputPorcentajePropuestas = $('#inputPorcentajePropuestas');
        const tablaSeguimientoPropuestas = $('#tablaSeguimientoPropuestas');
        const botonGuardarSeguimientoPropuestas = $('#botonGuardarSeguimientoPropuestas');
        const botonConsultarIndicadoresAccionesCicloTrabajo = $('#botonConsultarIndicadoresAccionesCicloTrabajo');
        const botonConsultarPorcentajeAccionesSolventadasArea = $('#botonConsultarPorcentajeAccionesSolventadasArea');
        const botonConsultarTotalAccionesSolventadasArea = $('#botonConsultarTotalAccionesSolventadasArea');
        const botonConsultarIndicadoresAccionesCicloTrabajoArea = $('#botonConsultarIndicadoresAccionesCicloTrabajoArea');
        const botonConsultarHistoricoAccionesSolventadasArea = $('#botonConsultarHistoricoAccionesSolventadasArea');
        const panelIndicadoresAccionesCicloTrabajo = $('#panelIndicadoresAccionesCicloTrabajo');
        const panelPorcentajeAccionesSolventadasArea = $('#panelPorcentajeAccionesSolventadasArea');
        const panelTotalAccionesSolventadasArea = $('#panelTotalAccionesSolventadasArea');
        const panelIndicadoresAccionesCicloTrabajoArea = $('#panelIndicadoresAccionesCicloTrabajoArea');
        const panelHistoricoAccionesSolventadasArea = $('#panelHistoricoAccionesSolventadasArea');
        const inputSeguimientoDetalleTotalAcciones = $('#inputSeguimientoDetalleTotalAcciones');
        const inputSeguimientoDetalleAvance = $('#inputSeguimientoDetalleAvance');
        const inputSeguimientoDetalleSolventadas = $('#inputSeguimientoDetalleSolventadas');
        const inputSeguimientoDetalleProceso = $('#inputSeguimientoDetalleProceso');
        const tablaAreasAcciones = $('#tablaAreasAcciones');
        const selectAreaSeguimiento = $('#selectAreaSeguimiento');
        const tablaAreasAccionesDetalle = $('#tablaAreasAccionesDetalle');
        //#endregion

        //#region Dashboard
        const selectCentroCostoDashboard = $('#selectCentroCostoDashboard');
        const selectAreaDashboard = $('#selectAreaDashboard');
        const inputFechaInicioDashboard = $('#inputFechaInicioDashboard');
        const inputFechaFinDashboard = $('#inputFechaFinDashboard');
        const botonBuscarDashboard = $('#botonBuscarDashboard');
        const graficaIndicadores = $('#graficaIndicadores');
        const h1CiclosRealizados = $('#h1CiclosRealizados');
        const tablaInspeccionesCumplidas = $('#tablaInspeccionesCumplidas');
        const tablaInspeccionesRealizarNuevamente = $('#tablaInspeccionesRealizarNuevamente');
        const tablaDetallesCiclos = $('#tablaDetallesCiclos');
        const tablaDetallesCiclosRealizados = $('#tablaDetallesCiclosRealizados');
        const graficaDetalles = $('#graficaDetalles');
        const tablaDetallesAnual = $('#tablaDetallesAnual');
        const tablaDetallesColaboradores = $('#tablaDetallesColaboradores');
        const tablaPorcentajes = $('#tablaPorcentajes');
        const tdCiclosPeriodicos = $('#tdCiclosPeriodicos');
        const tdPromedioGeneral = $('#tdPromedioGeneral');
        const tdCiclosLiberacion = $('#tdCiclosLiberacion');
        const tablaDashboardCiclos = $('#tablaDashboardCiclos');
        const tablaAspectosColaboladores = $('#tablaAspectosColaboladores');
        //#endregion

        //#region Crear
        const inputTitulo = $('#inputTitulo');
        const inputDescripcion = $('#inputDescripcion');
        const divRadioTipoCiclo = $('#divRadioTipoCiclo');
        const radioPeriodico = $('#radioPeriodico');
        const radioLiberacion = $('#radioLiberacion');
        const selectDepartamentoCrear = $('#selectDepartamentoCrear');
        const inputCriterio = $('#inputCriterio');
        const selectPonderacion = $('#selectPonderacion');
        const botonAgregarCriterio = $('#botonAgregarCriterio');
        const tablaCriterios = $('#tablaCriterios');
        const botonGuardarCiclo = $('#botonGuardarCiclo');
        //#endregion
        //#endregion

        let dtCiclosTrabajo;
        let dtCriterios;
        let dtRevisiones;
        let dtPropuestasMejora;
        let dtSeguimientoAcciones;
        let dtSeguimientoPropuestas;
        let dtInspeccionesCumplidas;
        let dtInspeccionesRealizarNuevamente;
        let dtDetallesCiclos;
        let dtDetallesCiclosRealizados;
        let dtDetallesAnual;
        let dtDetallesColaboradores;
        let dtAreasAcciones;
        let dtAreasAccionesDetalle;
        let dtDashboardCiclos;
        let dtAspectosColaborades;
        let dtCiclosRequeridos;

        const SECCIONES = {
            INDICADORES_ACCIONES_CICLO_TRABAJO: 1,
            PORCENTAJE_ACCIONES_SOLVENTADAS_AREA: 2,
            TOTAL_ACCIONES_SOLVENTADAS_AREA: 3,
            INDICADORES_ACCIONES_CICLO_TRABAJO_AREA: 4,
            HISTORICO_ACCIONES_SOLVENTADAS_AREA: 5
        };

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioMes = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        //#endregion

        _privilegioUsuario = +inputPrivilegio.val();

        (function init() {
            // revisarPrivilegio();
            seleccionarPrimerTab();
            $('.select2').select2({ language: { noResults: () => { return "No hay resultados" }, searching: () => { return "Buscando..." } } });
            initTablaCiclosRegistro();
            initTablaCriterios();
            initTablaRevisiones();
            initTablaPropuestasMejora();
            initTablaSeguimientoAcciones();
            initTablaSeguimientoPropuestas();
            initTablaInspeccionesCumplidas();
            initTablaAspectosColaboladores();
            initTablaInspeccionesRealizarNuevamente();
            initTablaDetallesCiclos();
            initTablaDetallesCiclosRealizados();
            initTablaDetallesAnual();
            initTablaDetallesColaboradores();
            initTablaAreasAcciones();
            initTablaAreasAccionesDetalle();
            initTblDashboardCiclos();
            inittblCicloTrabajos();
            initTablaCiclosRequeridos();

            fncTablaCiclosTrabajo();

            setCombos();

            inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioMes);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
            inputFechaRegistroCiclo.datepicker({ dateFormat, maxDate: fechaActual, showAnim });
            inputFechaInicioSeguimiento.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioMes);
            inputFechaFinSeguimiento.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
            inputFechaInicioDashboard.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioMes);
            inputFechaFinDashboard.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);

            inputRevisorRegistroCiclo.getAutocompleteValid(setDatosRevisor, verificarRevisor, { porClave: false }, 'GetUsuariosAutocomplete');
            inputColaboradorRegistroCiclo.getAutocompleteValid(setDatosColaborador, verificarColaborador, { porClave: false }, 'GetEmpleadoEnKontrolAutocomplete');

            botonGuardarCiclo.click(function () {
                if (botonGuardarCiclo.attr("data-id") == 0) {
                    guardarCiclo();
                }
                else {
                    fncEditarCicloTrabajos();
                }
            });

            botonNuevoRegistro.click(() => { limpiarModalRegistroCicloTrabajo(); modalRegistroCiclo.modal('show'); mostrarControlesModalRegistroCicloTrabajo(); });
            botonAccionRegistroCiclo.click(() => {
                modalAccionRequerida.modal('show');
                modalAccionRequerida.css('z-index', 1501);
                $('.modal-backdrop:eq(1)').css('z-index', 1500);
            });
            botonCancelarAccionRequerida.click(limpiarAccionRequerida);
            botonGuardarRegistroCiclo.click(guardarRegistroCiclo);
            botonBuscar.click(buscarRegistrosCiclo);
            botonBuscarSeguimiento.click(buscarSeguimiento);
            botonGuardarSeguimientoAcciones.click(guardarSeguimientoAcciones);
            botonGuardarSeguimientoPropuestas.click(guardarSeguimientoPropuestas);
            botonBuscarDashboard.click(cargarInformacionDashboard);

            selectCentroCosto.change(cargarAreasCC);
            selectCentroCostoRegistroCiclo.change(cargarAreasCCRegistroCiclo);
            selectCentroCostoDashboard.change(cargarAreasCCDashboard);

            botonConsultarIndicadoresAccionesCicloTrabajo.click(() => { cargarDatosSeccion(SECCIONES.INDICADORES_ACCIONES_CICLO_TRABAJO) });
            botonConsultarPorcentajeAccionesSolventadasArea.click(() => { cargarDatosSeccion(SECCIONES.PORCENTAJE_ACCIONES_SOLVENTADAS_AREA) });
            botonConsultarTotalAccionesSolventadasArea.click(() => { cargarDatosSeccion(SECCIONES.TOTAL_ACCIONES_SOLVENTADAS_AREA) });
            botonConsultarIndicadoresAccionesCicloTrabajoArea.click(() => { cargarDatosSeccion(SECCIONES.INDICADORES_ACCIONES_CICLO_TRABAJO_AREA) });
            botonConsultarHistoricoAccionesSolventadasArea.click(() => { cargarDatosSeccion(SECCIONES.HISTORICO_ACCIONES_SOLVENTADAS_AREA) });

            botonExcelAcciones.click(() => {
                if (dtSeguimientoAcciones.data().toArray().length == 0) {
                    Alert2Warning('No hay datos en la tabla.');
                    return;
                }

                location.href = `DescargarExcelSeguimientoAcciones`;
            });

            botonCiclosRequeridos.click(() => {
                modalCiclosRequeridos.modal('show');
                cargarCiclosRequeridos();
            });
            botonGuardarCiclosRequeridos.click(guardarCiclosRequeridos);
        })();

        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });
        //#region Luis Olivarria

        btnNuevoRegistro.click(function () {
            $("#title-modal").text("Registro Ciclos de Trabajo");
            fncLimpiarFormulario();

            botonGuardarCiclo.attr("data-id", 0);
            inputTitulo.attr("disabled", false);
            mdlRegistro.modal("show");
        });

        function fncLimpiarFormulario() {
            inputTitulo.val("");
            inputDescripcion.val("");
            radioPeriodico.prop("checked", false);
            radioLiberacion.prop("checked", false);
            inputCriterio.val("");

            lstCicloTrabajoCriterio = [];
            dtCriterios.clear().draw();
            dtCriterios.rows.add(lstCicloTrabajoCriterio).draw();

        }

        function limpiar() {
            selectDepartamentoCrear.val('');
            selectDepartamentoCrear.multiselect('deselectAll', false);
            selectDepartamentoCrear.multiselect('refresh');
        }

        function llenarModal(datos) {


            selectDepartamentoCrear.find('option').each((index, element) => {
                let cc = $(element).attr('cc');
                let empresa = $(element).attr('empresa');
                let departamento = $(element).attr('departamento');

                if (datos.lista.some(x => x.cc == cc && x.empresa == empresa && x.area == departamento)) {
                    selectDepartamentoCrear.multiselect('select', $(element).val(), true);
                }
            });
            selectDepartamentoCrear.multiselect('refresh');


        }

        function inittblCicloTrabajos() {
            dtCiclo = tblCiclosTrabajo.DataTable({
                language: dtDicEsp,
                destroy: false,
                ordering: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: "id", title: "id", visible: false },
                    { data: "titulo", title: "Titulo" },
                    { data: "tipoCicloDesc", title: "Tipo de ciclo" },
                    { data: "descripcion", title: "descripcion", visible: false },
                    { data: "criterio", title: "criterio", visible: false },
                    { data: "ponderacion", title: "ponderacion", visible: false },
                    { data: "area", title: "area", visible: false },
                    {
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return `
                                <button title="Editar" class="btn-editar btn btn-sm btn-warning actualizarCicloTrabajo" value="${row.id}">
                                    <i class="fas fa-pencil-alt"></i>
                                </button>
                                &nbsp;
                                <button title="Eliminar" class="btn-eliminar btn btn-sm btn-danger eliminarCicloTrabajo" value="${row.id}">
                                    <i class="fas fa-trash"></i>
                                </button>`;
                        },
                        title: "Acciones"
                    },
                ],
                initComplete: function (settings, json, datos) {
                    tblCiclosTrabajo.on('click', '.actualizarCicloTrabajo', function () {
                        initTablaCriterios();
                        let rowData = dtCiclo.row($(this).closest("tr")).data();
                        let id = parseFloat(rowData.id);

                        botonGuardarCiclo.attr("data-id", rowData.id);
                        inputDescripcion.val(rowData.descripcion);
                        inputTitulo.val(rowData.titulo);
                        // inputTitulo.attr("disabled", true);
                        if (rowData.tipoCiclo == 1) {
                            radioPeriodico.prop("checked", true);
                        }
                        else {
                            radioLiberacion.prop("checked", true);

                        }
                        $("#title-modal").text("");
                        $("#title-modal").text("EDITAR CICLO");
                        axios.get('getListaDepartamientos', { params: { listaAutorizacionID: id } })
                            .then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    limpiar();
                                    llenarModal(datos);
                                    mdlRegistro.modal("show");
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));

                        fncTablaCiclosCriterioTrabajo(id);

                    });
                    tblCiclosTrabajo.on('click', '.eliminarCicloTrabajo', function () {
                        let rowData = dtCiclo.row($(this).closest("tr")).data();
                        let id = parseFloat(rowData.id);
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCicloTrabajo(id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncTablaCiclosTrabajo() {
            axios.post("GetTablaCicloTrabajo").then(response => {
                let { success, items, message } = response.data;
                console.log(response);
                if (success) {
                    dtCiclo.clear().draw();
                    dtCiclo.rows.add(response.data.lstCicloTrabajo).draw();
                }
                else {
                    Alert2Error(message)
                }

            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarCicloTrabajo(id) {
            console.log(id)
            axios.post('EliminarCicloTrabajo', { id: id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha eliminado con éxito.");
                    fncTablaCiclosTrabajo();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function Objeto() {

            let objFrente = new Object();
            let id = botonGuardarCiclo.attr("data-id");
            let descripcion = inputDescripcion.val();
            let ponderacionn = selectPonderacion.val();
            let tipoCiclo;

            if (radioPeriodico.prop("checked") == true) {
                tipoCiclo = 1;
                console.log(tipoCiclo);
            }
            else {
                tipoCiclo = 2;
                console.log(tipoCiclo);
            }

            if (descripcion == "")
                Alert2Warning("Es necesario indicar nombre del frente.");

            objFrente = {
                id: id,
                titulo: inputTitulo.val(),
                descripcion: descripcion,
                tipoCiclo: tipoCiclo,
                ponderacionn: ponderacionn
            };
            return objFrente;
        }

        function fncCamposVacios() {
            let vacio = false;
            inputDescripcion.val() == "" ? vacio = true : vacio = false;
            return vacio;
        }
        function fncEditarCicloTrabajos() {
            let parametros = Objeto();
            let AreaCuenta = getValoresMultiples('#selectDepartamentoCrear');
            let lstAreass = [];

            getValoresMultiplesCustomDepartamento('#selectDepartamentoCrear').forEach(x => {
                lstAreass.push({
                    cc: x.cc,
                    area: +x.departamento,
                    empresa: +x.empresa,
                });
            });

            let criterio = [];

            tablaCriterios.find('tbody tr').each(function (index, row) {
                let rowData = dtCriterios.row(row).data();

                criterio.push({
                    id: rowData.id,
                    descripcion: rowData.descripcion,
                    ponderacion: rowData.ponderacion,
                    aspectoEvaluado: +$(row).find('.selectAspectoEvaluado').val()
                });
            });

            if (fncCamposVacios()) {
                Alert2Warning("Es necesario ingresar los datos faltantes");
            } else {
                axios.post('EditarCicloTrabajo', { parametros: parametros, AreaCuenta: AreaCuenta, criterio: criterio, lstAreass: lstAreass }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        mdlRegistro.modal('hide');
                        Alert2Exito("Se ha modificado con éxito.");
                        fncTablaCiclosTrabajo();
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncTablaCiclosCriterioTrabajo(id) {
            axios.post("GetTablaCriterioTrabajo", { id: id }).then(response => {
                let { success, items, message } = response.data;
                console.log(response);
                if (success) {
                    dtCriterios.clear().draw();
                    dtCriterios.rows.add(response.data.lstCicloTrabajoCriterio).draw();
                }
                else {
                    Alert2Error(message)
                }

            }).catch(error => Alert2Error(error.message));
        }

        function cargarCiclosRequeridos() {
            axios.post('CargarCiclosRequeridos').then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    AddRows(tablaCiclosRequeridos, response.data.data);
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarCiclosRequeridos() {
            let listaCiclosRequeridos = [];

            tablaCiclosRequeridos.find('tbody tr').each(function (index, row) {
                let rowData = dtCiclosRequeridos.row(row).data();

                listaCiclosRequeridos.push({
                    cc: rowData.cc,
                    empresa: rowData.empresa,
                    cantidad: +$(row).find('.inputCiclosRequeridos').val()
                });
            });

            axios.post('GuardarCiclosRequeridos', { listaCiclosRequeridos }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información.');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        //#endregion

        function setCombos() {
            axios.get('ObtenerComboCCAmbasEmpresas').then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    selectCentroCosto.append('<option value="Todos">Todos</option>');
                    selectCentroCostoRegistroCiclo.append('<option value="">--Seleccione--</option>');
                    selectCentroCostoSeguimiento.append('<option value="Todos">Todos</option>');
                    selectCentroCostoDashboard.append('<option value="Todos">Todos</option>');

                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}">`;

                        x.options.forEach(y => {
                            let empresaNumero = 0;

                            switch (x.label) {
                                case 'CONSTRUPLAN':
                                    empresaNumero = 1;
                                    break;
                                case 'ARRENDADORA':
                                    empresaNumero = 2;
                                    break;
                                case 'PERÚ':
                                    empresaNumero = 6;
                                    break;
                                default:
                                    empresaNumero = 0;
                                    break;
                            }

                            groupOption += `<option value="${y.Value}" empresa="${empresaNumero}">${y.Text}</option>`;
                        });

                        groupOption += `</optgroup>`;

                        selectCentroCosto.append(groupOption);
                        selectCentroCostoRegistroCiclo.append(groupOption);
                        selectCentroCostoSeguimiento.append(groupOption);
                        selectCentroCostoDashboard.append(groupOption);
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }

                convertToMultiselect('#selectCentroCosto');
                convertToMultiselect('#selectCentroCostoSeguimiento');
                convertToMultiselect('#selectCentroCostoDashboard');
            }).catch(error => AlertaGeneral(`Alerta`, error.message));

            selectCicloTrabajo.fillCombo('GetCiclosTrabajoCombo', null, false, '');
            selectCicloTrabajo.find('option[value=""]').remove();
            selectCicloTrabajo.select2(); // convertToMultiselect('#selectCicloTrabajo');
            selectCicloRegistroCiclo.fillCombo('GetCiclosTrabajoCombo', null, false, null);

            axios.get('GetDepartamentosCombo').then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    // selectArea.append('<option value="Todos">Todos</option>');
                    selectDepartamentoCrear.append('<option value="Todos">Todos</option>');
                    // selectAreaRegistroCiclo.append('<option value="">--Seleccione--</option>');
                    // selectAreaDashboard.append('<option value="Todos">Todos</option>');

                    let valorContador = 0;
                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}">`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${valorContador}" empresa="${y.Prefijo == 'CONSTRUPLAN' ? 1 : y.Prefijo == 'ARRENDADORA' ? 2 : 0}" cc="${y.Id}" departamento="${y.Value}">${y.Text}</option>`;
                            valorContador++;
                        });

                        groupOption += `</optgroup>`;

                        // selectArea.append(groupOption);
                        selectDepartamentoCrear.append(groupOption);
                        // selectAreaRegistroCiclo.append(groupOption);
                        // selectAreaDashboard.append(groupOption);
                    });

                    convertToMultiselect('#selectArea');
                    convertToMultiselect('#selectDepartamentoCrear');
                    convertToMultiselect('#selectAreaDashboard');
                    convertToMultiselect('#selectAreaRegistroCiclo');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));

            selectCapacitacionAccionRequerida.fillCombo('ObtenerComboCursos', null, false, null);

            selectCapacitacionAreaSeguimiento.select2({ dropdownParent: $(modalAccionRequerida) });
            selectCapacitacionInteresados.select2({ dropdownParent: $(modalAccionRequerida) });

            selectCapacitacionAreaSeguimiento.fillComboBox('GetAreaSeguimiento', null, null, null);
            selectCapacitacionInteresados.fillComboBox('GetInteresados', null, null, null);

            axios.get('GetAreaSeguimiento').then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    selectAreaSeguimiento.append('<option value="Todos">Todos</option>');

                    items.forEach(x => {
                        selectAreaSeguimiento.append(`<option value="${x.valor}" >${x.texto}</option>`);
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }

                convertToMultiselect('#selectAreaSeguimiento');
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function seleccionarPrimerTab() {
            $('#ulTabs').find('li').first().addClass('active');
            $('#divTabs').find('div').first().addClass('in active');
        }

        function revisarPrivilegio() {
            axios.get('privilegioCapacitacion')
                .then(response => {
                    if (response.data == 0) {
                        AlertaGeneral(`Alerta`, `No tiene permisos para visualizar este módulo.`);
                    } else {
                        _privilegioUsuario = response.data;

                        if (response.data != 1 && response.data != 4) {
                            botonGuardarSeguimientoAcciones.attr('disabled', true);
                            botonGuardarSeguimientoPropuestas.attr('disabled', true);
                        }
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        //#region Ciclos de Trabajo
        selectCicloRegistroCiclo.on('change', function () {
            let cicloID = +selectCicloRegistroCiclo.val();

            inputCalificacionRegistroCiclo.val('');
            checkBoxAcreditoCiclo.prop('checked', false);
            checkBoxRequiereRetroalimentacion.prop('checked', false);

            if (cicloID > 0) {
                axios.get('GetCicloByID', { params: { cicloID } })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            selectTipoCicloRegistroCiclo.val(datos.tipoCiclo);
                            AddRows(tablaRevisiones, datos.listaCriterios);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                limpiarTabPuntosRevisionCiclo();
            }
        });

        botonAgregarPropuesta.on('click', function () {
            let datos = dtPropuestasMejora.rows().data();

            datos.push({
                descripcion: '',
                // proceso: false,
                // solventada: false
            });

            dtPropuestasMejora.clear();
            dtPropuestasMejora.rows.add(datos).draw();
        });

        checkboxTodosCiclos.on('click', function () {
            if (checkboxTodosCiclos.is(':checked')) {
                selectCicloTrabajo.find('option').prop("selected", true);
                selectCicloTrabajo.trigger("change");
            } else {
                selectCicloTrabajo.find('option').prop("selected", false);
                selectCicloTrabajo.trigger("change");
            }
        });

        function initTablaCiclosRegistro() {
            dtCiclosTrabajo = tablaCiclosRegistro.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaCiclosRegistro.on('click', '.botonVerDetalles', function () {
                        let rowData = dtCiclosTrabajo.row($(this).closest('tr')).data();

                        cargarRegistroCicloTrabajo(rowData.id);
                    });

                    tablaCiclosRegistro.on('click', '.botonEliminarRegistro', function () {
                        let rowData = dtCiclosTrabajo.row($(this).closest('tr')).data();

                        Alert2AccionConfirmar('Atención', '¿Desea eliminar el registro del ciclo de trabajo?', 'Confirmar', 'Cancelar', () => {
                            axios.post('EliminarRegistroCicloTrabajo', { ciclo: { id: rowData.id } }).then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    Alert2Exito('Se ha eliminado la información.');
                                    buscarRegistrosCiclo();
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));
                        });
                    });
                },
                columns: [
                    { data: 'cicloDesc', title: 'Ciclos', sortable: false },
                    { data: 'colaboradorDesc', title: 'Ciclo aplicado al colaborador', sortable: false },
                    { data: 'areaDesc', title: 'Áreas', sortable: false },
                    { data: 'fechaString', title: 'Fecha', sortable: false },
                    {
                        title: '', render: function (data, type, row, meta) {
                            return `
                                <div>
                                    <button class="btn btn-xs btn-primary botonVerDetalles"><i class="fa fa-eye"></i></button>
                                    ${_privilegioUsuario == 1 ? `
                                    <button class="btn btn-xs btn-danger botonEliminarRegistro"><i class="fa fa-times"></i></button>
                                    ` : ``}
                                </div>
                            `;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '60%', targets: [0] },
                    { width: '10%', targets: [4] }
                ]
            });
        }

        function initTablaRevisiones() {
            dtRevisiones = tablaRevisiones.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaRevisiones.on('change', '.regular-checkbox', function () {
                        calcularCalificacion();
                    });
                },
                columns: [
                    {
                        title: '#', render: function (data, type, row, meta) {
                            return meta.row + 1;
                        }
                    },
                    { data: 'descripcion', title: 'Revisiones' },
                    { data: 'ponderacionDesc', title: 'Ponderación' },
                    {
                        title: 'SÍ', render: function (data, type, row, meta) {
                            return `
                                <div>
                                    <input type="radio" id="radioAcreditaSI_${meta.row}" name="radioAcredita_${meta.row}" class="regular-checkbox" value="1">
                                    <label for="radioAcreditaSI_${meta.row}"></label>
                                </div>`;
                            // checkbox.style.height = '25px';
                        }
                    },
                    {
                        title: 'NO', render: function (data, type, row, meta) {
                            return `
                                <div>
                                    <input type="radio" id="radioAcreditaNO_${meta.row}" name="radioAcredita_${meta.row}" class="regular-checkbox" value="2">
                                    <label for="radioAcreditaNO_${meta.row}"></label>
                                </div>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '90%', targets: [1] }
                ]
            });

            // dtRevisiones.on('order.dt search.dt', function () {
            //     dtRevisiones.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            //         cell.innerHTML = i + 1;
            //     });
            // }).draw();
        }

        function initTablaPropuestasMejora() {
            dtPropuestasMejora = tablaPropuestasMejora.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaPropuestasMejora.on('change', '.inputPropuestaDescripcion', function () {
                        tablaPropuestasMejora.find('tbody tr').each(function (index, row) {
                            let rowData = tablaPropuestasMejora.DataTable().row(row).data();

                            let descripcion = $(row).find('.inputPropuestaDescripcion').val();

                            rowData.descripcion = descripcion;

                            tablaPropuestasMejora.DataTable().row(row).data(rowData).draw();
                        });
                    });

                    tablaPropuestasMejora.on('click', '.botonQuitarPropuesta', function () {
                        dtPropuestasMejora.row($(this).closest('tr')).remove().draw();
                    });

                    // tablaPropuestasMejora.on('click', '.radioPropuesta', function () {
                    //     let row = $(this).closest('tr');
                    //     let rowData = tablaPropuestasMejora.DataTable().row(row).data();
                    //     let checked = $(this).prop('checked');

                    //     if (+$(this).val() == 1) {
                    //         rowData.proceso = checked;
                    //         rowData.solventada = !checked;
                    //     } else if (+$(this).val() == 2) {
                    //         rowData.proceso = !checked;
                    //         rowData.solventada = checked;
                    //     }

                    //     tablaPropuestasMejora.DataTable().row(row).data(rowData).draw();
                    // });
                },
                columns: [
                    {
                        title: '#', render: function (data, type, row, meta) {
                            return ''; // return meta.row + 1;
                        }
                    },
                    {
                        data: 'descripcion', title: 'Propuestas del Personal Operativo', render: function (data, type, row, meta) {
                            return `<input class="form-control inputPropuestaDescripcion" value="${data}">`;
                        }
                    },
                    // {
                    //     data: 'proceso', title: 'Proceso', render: function (data, type, row, meta) {
                    //         return `
                    //             <div>
                    //                 <input type="radio" id="radioPropuestaProceso_${meta.row}" name="radioPropuesta_${meta.row}" class="regular-checkbox radioPropuesta" value="1" ${data ? 'checked' : ''}>
                    //                 <label for="radioPropuestaProceso_${meta.row}"></label>
                    //             </div>`;
                    //     }
                    // },
                    // {
                    //     data: 'solventada', title: 'Solventada', render: function (data, type, row, meta) {
                    //         return `
                    //             <div>
                    //                 <input type="radio" id="radioPropuestaSolventada_${meta.row}" name="radioPropuesta_${meta.row}" class="regular-checkbox radioPropuesta" value="2" ${data ? 'checked' : ''}>
                    //                 <label for="radioPropuestaSolventada_${meta.row}"></label>
                    //             </div>`;
                    //     }
                    // },
                    {
                        render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-danger botonQuitarPropuesta"><i class="fa fa-times"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '90%', targets: [1] }
                ]
            });

            dtPropuestasMejora.on('order.dt search.dt', function () {
                dtPropuestasMejora.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        }

        function initTablaCiclosRequeridos() {
            dtCiclosRequeridos = tablaCiclosRequeridos.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaCiclosRequeridos.on('focus', '.inputCiclosRequeridos', function () {
                        $(this).select();
                    });
                },
                columns: [
                    { data: 'ccDesc', title: 'Centro de Costo' },
                    { data: 'empresaDesc', title: 'Empresa' },
                    {
                        title: '# Ciclos Requeridos', render: function (data, type, row, meta) {
                            return `<input type="number" class="form-control text-center inputCiclosRequeridos" value="${row.cantidad}">`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function calcularCalificacion() {
            let flagRevisionCritica = false;
            let contadorRevisionMedia = 0;
            let contadorAcreditado = 0;
            let contadorRenglonesPendientes = 0;
            let cantidadTotalRevisiones = tablaRevisiones.find('tbody tr').length;

            tablaRevisiones.find('tbody tr').each(function (idx, row) {
                if ($(row).find('input[type=radio]:checked').length > 0) {
                    let rowData = dtRevisiones.row(row).data();
                    let radioSeleccionado = +$(row).find('input[type=radio]:checked').val();

                    if (radioSeleccionado == 1) {
                        contadorAcreditado++;
                    } else {
                        if (rowData.ponderacion == 2) {
                            contadorRevisionMedia++;
                        } else if (rowData.ponderacion == 3) {
                            flagRevisionCritica = true;
                        }
                    }
                } else {
                    contadorRenglonesPendientes++;
                }
            });

            inputCalificacionRegistroCiclo.val(maskNumero2D((contadorAcreditado * 100) / cantidadTotalRevisiones));

            if (contadorAcreditado == cantidadTotalRevisiones) {
                checkBoxAcreditoCiclo.prop('checked', true);
                checkBoxRequiereRetroalimentacion.prop('checked', false);
            } else {
                if (flagRevisionCritica) {
                    checkBoxAcreditoCiclo.prop('checked', false);
                    checkBoxRequiereRetroalimentacion.prop('checked', true);
                } else {
                    if (contadorRevisionMedia >= 3) {
                        checkBoxAcreditoCiclo.prop('checked', false);
                        checkBoxRequiereRetroalimentacion.prop('checked', false); //No se ha especificado que se coloque este campo como true en esta situación.
                    } else {
                        if (contadorRenglonesPendientes == 0) {
                            checkBoxAcreditoCiclo.prop('checked', true);
                            checkBoxRequiereRetroalimentacion.prop('checked', false);
                        } else {
                            checkBoxAcreditoCiclo.prop('checked', false);
                            checkBoxRequiereRetroalimentacion.prop('checked', false);
                        }
                    }
                }
            }

            let calificacion = +inputCalificacionRegistroCiclo.val();

            if (calificacion >= 85) {
                checkBoxAcreditoCiclo.prop('checked', true);
            } else {
                checkBoxAcreditoCiclo.prop('checked', false);
            }
        }

        function setDatosRevisor(e, ui) {
            inputRevisorRegistroCiclo.data().revisorID = ui.item.id;
            inputRevisorRegistroCiclo.val(ui.item.nombre);
        }

        function verificarRevisor(e, ui) {
            if (ui.item == null) {
                inputRevisorRegistroCiclo.val('');
                inputRevisorRegistroCiclo.data().revisorID = null;
            }
        }

        function setDatosColaborador(e, ui) {
            inputColaboradorRegistroCiclo.data().colaboradorClaveEmpleado = ui.item.id;
            inputColaboradorRegistroCiclo.val(ui.item.nombre);
        }

        function verificarColaborador(e, ui) {
            if (ui.item == null) {
                inputColaboradorRegistroCiclo.val('');
                inputColaboradorRegistroCiclo.data().colaboradorClaveEmpleado = null;
            }
        }

        function limpiarModalRegistroCicloTrabajo() {
            //#region Tab Puntos de Revisión
            selectCentroCostoRegistroCiclo.val('');
            selectCentroCostoRegistroCiclo.select2().change();
            inputFechaRegistroCiclo.val('');
            selectAreaRegistroCiclo.empty();
            selectAreaRegistroCiclo.multiselect('rebuild');
            selectCicloRegistroCiclo.val('');
            selectCicloRegistroCiclo.select2().change();
            inputRevisorRegistroCiclo.val('');
            inputRevisorRegistroCiclo.data().revisorID = null;
            inputColaboradorRegistroCiclo.val('');
            inputColaboradorRegistroCiclo.data().colaboradorClaveEmpleado = null;
            selectTipoCicloRegistroCiclo.val('');
            inputCalificacionRegistroCiclo.val('');
            inputEconomicoRegistroCiclo.val('');
            checkBoxAcreditoCiclo.prop('checked', false);
            checkBoxRequiereRetroalimentacion.prop('checked', false);
            dtRevisiones.clear().draw();
            inputObservacionesRevisorRegistroCiclo.val('');
            inputAccionesTomadasRegistroCiclo.val('');
            inputObservacionesLiderRegistroCiclo.val('');
            //#endregion

            //#region Tab Propuestas de Mejora
            dtPropuestasMejora.clear().draw();
            //#endregion
        }

        function limpiarTabPuntosRevision() {
            selectCentroCostoRegistroCiclo.val('');
            selectCentroCostoRegistroCiclo.select2().change();
            inputFechaRegistroCiclo.val('');
            selectAreaRegistroCiclo.val('');
            selectCicloRegistroCiclo.val('');
            selectCicloRegistroCiclo.select2().change();
            inputRevisorRegistroCiclo.val('');
            inputRevisorRegistroCiclo.data().revisorID = null;
            inputColaboradorRegistroCiclo.val('');
            inputColaboradorRegistroCiclo.data().colaboradorClaveEmpleado = null;
            selectTipoCicloRegistroCiclo.val('');
            inputCalificacionRegistroCiclo.val('');
            inputEconomicoRegistroCiclo.val('');
            checkBoxAcreditoCiclo.prop('checked', false);
            checkBoxRequiereRetroalimentacion.prop('checked', false);
            dtRevisiones.clear().draw();
            inputObservacionesRevisorRegistroCiclo.val('');
            inputAccionesTomadasRegistroCiclo.val('');
            inputObservacionesLiderRegistroCiclo.val('');
        }

        function limpiarTabPuntosRevisionCiclo() {
            selectTipoCicloRegistroCiclo.val('');
            dtRevisiones.clear().draw();
        }

        function limpiarAccionRequerida() {
            textAreaAccionRequerida.val('');
            selectMetodoAccionRequerida.val('');
            selectCapacitacionAccionRequerida.val('');
            selectCapacitacionAccionRequerida.select2().change();
            selectCapacitacionAreaSeguimiento.val('');
            selectCapacitacionAreaSeguimiento.select2().change();
            selectCapacitacionInteresados.val('');
            selectCapacitacionInteresados.select2().change();
        }

        function guardarRegistroCiclo() {
            if (validarCamposRegistroCiclo()) {
                AlertaGeneral(`Alerta`, `Debe capturar todos los campos.`);
                return;
            }

            let listaRevisiones = [];
            let listaPropuestas = [];

            tablaRevisiones.find('tbody tr').each(function (idx, row) {
                let rowData = dtRevisiones.row(row).data();

                if (rowData != undefined) {
                    listaRevisiones.push({
                        criterioID: rowData.id,
                        acredito: $(row).find(`input[name=radioAcredita_${idx}]:checked`).val() == '1' ? true : false
                    });
                }
            });

            tablaPropuestasMejora.find('tbody tr').each(function (idx, row) {
                let rowData = dtPropuestasMejora.row(row).data();

                if (rowData != undefined) {
                    listaPropuestas.push({
                        propuesta: $(row).find('.inputPropuestaDescripcion').val(),
                        // proceso: $(row).find(`#radioPropuestaProceso_${idx}`).prop('checked'),
                        // solventada: $(row).find(`#radioPropuestaSolventada_${idx}`).prop('checked')
                    });
                }
            });

            let registroCiclo = {
                cc: selectCentroCostoRegistroCiclo.val(),
                fecha: inputFechaRegistroCiclo.val(),
                // area: +selectAreaRegistroCiclo.val(),
                empresa: +$(selectAreaRegistroCiclo.find('option:selected')[0]).attr('empresa'),
                cicloID: +selectCicloRegistroCiclo.val(),
                revisor: inputRevisorRegistroCiclo.data().revisorID,
                colaborador: inputColaboradorRegistroCiclo.data().colaboradorClaveEmpleado,
                calificacion: inputCalificacionRegistroCiclo.val(),
                economico: inputEconomicoRegistroCiclo.val(),
                acredito: checkBoxAcreditoCiclo.prop('checked'),
                retroalimentacion: checkBoxRequiereRetroalimentacion.prop('checked'),
                accionRequerida: textAreaAccionRequerida.val(),
                metodo: +selectMetodoAccionRequerida.val(),
                cursoID: +selectCapacitacionAccionRequerida.val(),
                observacionesRevisor: inputObservacionesRevisorRegistroCiclo.val(),
                accionesTomadas: inputAccionesTomadasRegistroCiclo.val(),
                observacionesLider: inputObservacionesLiderRegistroCiclo.val()
            };

            let listaAreas = [];

            getValoresMultiplesCustom('#selectAreaRegistroCiclo').forEach(x => {
                listaAreas.push({
                    cc: x.cc,
                    area: +x.departamento,
                    empresa: +x.empresa
                });
            });

            axios.post('GuardarRegistroCiclo', { registroCiclo, listaRevisiones, listaPropuestas, listaAreas, areasSeguimiento: selectCapacitacionAreaSeguimiento.val(), interesados: selectCapacitacionInteresados.val() })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        modalRegistroCiclo.modal('hide');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function validarCamposRegistroCiclo() {
            let campoInvalido = false;

            if (selectCentroCostoRegistroCiclo.val() == '') {
                campoInvalido = true;
            }

            if (inputFechaRegistroCiclo.val() == '') {
                campoInvalido = true;
            }

            if (selectAreaRegistroCiclo.val() == '') {
                campoInvalido = true;
            }

            if (selectCicloRegistroCiclo.val() == '') {
                campoInvalido = true;
            }

            if (inputRevisorRegistroCiclo.data().revisorID == null) {
                campoInvalido = true;
            }

            if (inputColaboradorRegistroCiclo.data().colaboradorClaveEmpleado == null) {
                campoInvalido = true;
            }

            if (inputCalificacionRegistroCiclo.val() == '') {
                campoInvalido = true;
            }

            if (inputEconomicoRegistroCiclo.val() == '') {
                campoInvalido = true;
            }

            if (checkBoxRequiereRetroalimentacion.prop('checked')) {
                if (textAreaAccionRequerida.val() == '' || selectMetodoAccionRequerida.val() == '' || selectCapacitacionAccionRequerida.val() == '') {
                    campoInvalido = true;
                }

                selectCapacitacionAreaSeguimiento.val().forEach(function (value, index, array) {
                    if (value == '4' && !selectCapacitacionAccionRequerida.val()) {
                        campoInvalido = true;
                    }
                });
            }

            tablaRevisiones.find('tbody tr').each(function (idx, row) {
                let rowData = dtRevisiones.row(row).data();

                if (rowData != undefined) {
                    let checkBoxSeleccionado = $(row).find(`input[name=radioAcredita_${idx}]:checked`);

                    if (checkBoxSeleccionado.length == 0) {
                        campoInvalido = true;
                    }
                }
            });

            tablaPropuestasMejora.find('tbody tr').each(function (idx, row) {
                let rowData = dtPropuestasMejora.row(row).data();

                if (rowData != undefined) {
                    let propuesta = $(row).find('.inputPropuestaDescripcion').val();
                    // let proceso = $(row).find(`#radioPropuestaProceso_${idx}`).prop('checked');
                    // let solventada = $(row).find(`#radioPropuestaSolventada_${idx}`).prop('checked');

                    // if (propuesta == '' || (!proceso && !solventada)) {
                    //     campoInvalido = true;
                    // }
                }
            });

            return campoInvalido;
        }

        function buscarRegistrosCiclo() {
            let listaCC = getValoresMultiples('#selectCentroCosto');
            let listaCiclos = getValoresMultiples('#selectCicloTrabajo');
            let listaAreas = [];
            let fechaInicio = inputFechaInicio.val();
            let fechaFin = inputFechaFin.val();

            getValoresMultiplesCustom('#selectArea').forEach(x => {
                listaAreas.push({
                    cc: x.cc,
                    departamento: +x.departamento,
                    empresa: +x.empresa
                });
            });

            axios.post('GetRegistrosCiclos', { filtros: { listaCC, listaCiclos, listaAreas, fechaInicio, fechaFin } })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaCiclosRegistro, datos);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarRegistroCicloTrabajo(id) {
            axios.post('GetRegistroCicloTrabajoByID', { id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        limpiarModalRegistroCicloTrabajo();
                        ocultarControlesModalRegistroCicloTrabajo();
                        modalRegistroCiclo.modal('show');
                        llenarModalRegistroCicloTrabajo(response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function llenarModalRegistroCicloTrabajo(data) {
            //#region Tab Puntos de Revisión
            selectCentroCostoRegistroCiclo.val(data.cc).trigger('change.select2');

            //#region Evento Change de selectCentroCostoRegistroCiclo
            let listaCentrosCosto = [];

            getValoresMultiplesCustom('#selectCentroCostoRegistroCiclo').forEach(x => {
                listaCentrosCosto.push({
                    cc: x.value,
                    empresa: +(x.empresa)
                });
            });

            if (listaCentrosCosto.filter((x) => x.cc != '').length == 0) {
                selectAreaRegistroCiclo.empty();
            }

            let ccsCplan = listaCentrosCosto.filter((x) => { return x.empresa == 1 || x.empresa == 6; }).map(function (x) { return x.cc; });
            let ccsArr = listaCentrosCosto.filter((x) => { return x.empresa == 2; }).map(function (x) { return x.cc; });

            $.post('/Administrativo/Capacitacion/ObtenerAreasPorCC', { ccsCplan, ccsArr })
                .then(response => {
                    selectAreaRegistroCiclo.empty();
                    if (response.success) {
                        // Operación exitosa.
                        const todosOption = `<option value="Todos">Todos</option>`;
                        selectAreaRegistroCiclo.append(todosOption);

                        response.items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                let flagSeleccionado = false;

                                if (data.listaAreas.some((x) => x.cc == y.Id && x.area == y.Value && x.empresa == y.Prefijo)) {
                                    flagSeleccionado = true;
                                }

                                groupOption += `<option value="${y.Value}" cc="${y.Id}" empresa="${y.Prefijo}" ${flagSeleccionado ? 'selected' : ''}>${y.Text}</option>`;
                            });
                            selectAreaRegistroCiclo.append(groupOption);
                        });

                        convertToMultiselect('#selectAreaRegistroCiclo');
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            //#endregion

            inputFechaRegistroCiclo.val(data.fechaString);
            selectCicloRegistroCiclo.val(data.cicloID).trigger('change.select2');

            AddRows(tablaRevisiones, data.listaRevisiones);

            tablaRevisiones.find('tbody tr').each(function (index, row) {
                let rowData = dtRevisiones.row(row).data();

                if (rowData.acredito) {
                    $(row).find('input[value="1"]').prop('checked', true);
                } else {
                    $(row).find('input[value="2"]').prop('checked', true);
                }
            });

            inputRevisorRegistroCiclo.val(data.revisorDesc);
            inputColaboradorRegistroCiclo.val(data.colaboradorDesc);
            selectTipoCicloRegistroCiclo.val(data.tipoCiclo);
            inputCalificacionRegistroCiclo.val(data.calificacion);
            inputEconomicoRegistroCiclo.val(data.economico);
            checkBoxAcreditoCiclo.prop('checked', data.acredito);
            checkBoxRequiereRetroalimentacion.prop('checked', data.retroalimentacion);
            inputObservacionesRevisorRegistroCiclo.val(data.observacionesRevisor);
            inputAccionesTomadasRegistroCiclo.val(data.accionesTomadas);
            inputObservacionesLiderRegistroCiclo.val(data.observacionesLider);
            //#endregion

            //#region Tab Propuestas de Mejora
            AddRows(tablaPropuestasMejora, data.listaPropuestas);
            tablaPropuestasMejora.find('tbody .botonQuitarPropuesta').css('display', 'none');
            //#endregion

            //#region Modal Acciones
            textAreaAccionRequerida.val(data.accionRequerida);
            selectMetodoAccionRequerida.val(data.metodo);
            selectCapacitacionAccionRequerida.val(data.cursoID).trigger('change.select2');
            //#endregion
        }

        function ocultarControlesModalRegistroCicloTrabajo() {
            botonGuardarRegistroCiclo.css('display', 'none');
            botonGuardarAccionRequerida.css('display', 'none');
            botonAgregarPropuesta.css('display', 'none');
        }

        function mostrarControlesModalRegistroCicloTrabajo() {
            botonGuardarRegistroCiclo.css('display', 'inline-block');
            botonGuardarAccionRequerida.css('display', 'inline-block');
            botonAgregarPropuesta.css('display', 'inline-block');
        }
        //#endregion

        //#region Seguimiento Acciones
        selectSeguimiento.on('change', function () {
            let seguimiento = +selectSeguimiento.val();

            switch (seguimiento) {
                case 1:
                    divTablaSeguimientoAcciones.show(500);
                    divTablaSeguimientoPropuestas.hide();
                    break;
                case 2:
                    divTablaSeguimientoAcciones.hide();
                    divTablaSeguimientoPropuestas.show(500);
                    break;
                default:
                    divTablaSeguimientoAcciones.hide(500);
                    divTablaSeguimientoPropuestas.hide(500);
                    break;
            }

            limpiarSeguimientoAccionesPropuestas();
        });

        function initTablaSeguimientoAcciones() {
            dtSeguimientoAcciones = tablaSeguimientoAcciones.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                scrollY: '45vh',
                scrollX: 'auto',
                scrollCollapse: true,
                createdRow: function (row, rowData) {
                    if (rowData.evaluador > 0) {
                        $(row).addClass('seguimientoEvaluado');
                    }
                },
                drawCallback: function (settings) {
                    $.fn.dataTable.tables({ api: true }).columns.adjust();
                },
                initComplete: function (settings, json) {
                    tablaSeguimientoAcciones.on('click', '.botonVerEvidenciaAccion', function () {
                        let rowData = dtSeguimientoAcciones.row($(this).closest('tr')).data();

                        mostrarArchivoEvidenciaAccion(rowData.id);
                    });

                    tablaSeguimientoAcciones.on('click', '.botonDescargarEvidenciaAccion', function () {
                        let rowData = dtSeguimientoAcciones.row($(this).closest('tr')).data();

                        descargarArchivoEvidenciaAccion(rowData.id);
                    });

                    tablaSeguimientoAcciones.on('change', 'input[type=file]', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtSeguimientoAcciones.row(row).data();
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
                            botonEvidencia.addClass('btn-success');
                            botonEvidencia.removeClass('btn-default');
                            iconoBoton.addClass('fa-check');
                            iconoBoton.removeClass('fa-upload');
                        } else {
                            labelArchivo.text('');
                            botonEvidencia.addClass('btn-default');
                            botonEvidencia.removeClass('btn-success');
                            iconoBoton.addClass('fa-upload');
                            iconoBoton.removeClass('fa-check');
                        }
                    });

                    tablaSeguimientoAcciones.on('click', '.radioBtn a', function () {
                        let rowData = dtSeguimientoAcciones.row($(this).closest('tr')).data();
                        let div = $(this).closest('div');
                        let seleccion = $(this).attr('aprobada');

                        div.find(`a[data-toggle="radioAprobado${rowData.id}"]`).not(`[aprobada="${seleccion}"]`).removeClass('active').addClass('notActive');
                        div.find(`a[data-toggle="radioAprobado${rowData.id}"][aprobada="${seleccion}"]`).removeClass('notActive').addClass('active');
                    });
                },
                columns: [
                    { data: 'fechaString', title: 'Fecha del Ciclo' },
                    { data: 'accion', title: 'Acciones' },
                    { data: 'areaSeguimientoString', title: 'Área Responsable del Seguimiento' },
                    { data: 'interesadosString', title: 'Responsables del seguimiento' },
                    {
                        title: 'Evidencia', render: function (data, type, row, meta) {
                            return `
                                <div>
                                    ${row.rutaEvidencia != null ? '<button class="btn btn-xs btn-default botonVerEvidenciaAccion"><i class="fa fa-eye"></i></button>' : ''}
                                    ${row.rutaEvidencia != null ? '<button class="btn btn-xs btn-default botonDescargarEvidenciaAccion"><i class="fa fa-download"></i></button>' : ''}
                                    ${row.rutaEvidencia == null ? `
                                        <div class="text-center">
                                            <label id="botonEvidencia_${row.id}" for="inputEvidencia_${row.id}" class="btn btn-xs btn-default"><i class="fa fa-upload"></i></label>
                                            <label id="labelArchivoEvidencia_${row.id}" class="labelArchivo"></label>
                                            <input id="inputEvidencia_${row.id}" type="file" class="inputEvidencia_${row.id}" accept="application/pdf, image/*">
                                        </div>
                                    ` : ''}
                                </div>
                            `;
                        }
                    },
                    {
                        title: 'Aprobó', render: function (data, type, row, meta) {
                            return row.rutaEvidencia != null ? `
                                <div class="radioBtn btn-group" style="min-width: 140px;">
                                    <a class="btn btn-success ${row.evaluador > 0 && row.aprobo ? 'active' : 'notActive'}" data-toggle="radioAprobado${row.id}" aprobada="1" ${row.evaluador > 0 || (_privilegioUsuario != 1 && _privilegioUsuario != 4) ? 'disabled' : ''}><i class="fa fa-check"></i>&nbsp;SÍ</a>
                                    <a class="btn btn-danger ${row.evaluador > 0 && !row.aprobo ? 'active' : 'notActive'}" data-toggle="radioAprobado${row.id}" aprobada="0" ${row.evaluador > 0 || (_privilegioUsuario != 1 && _privilegioUsuario != 4) ? 'disabled' : ''}><i class="fa fa-times"></i>&nbsp;NO</a>
                                    <a class="btn btn-primary ${row.evaluador == 0 ? 'active' : 'notActive'}" data-toggle="radioAprobado${row.id}" aprobada="2" ${row.evaluador > 0 || (_privilegioUsuario != 1 && _privilegioUsuario != 4) ? 'disabled' : ''}><i class="fa fa-square"></i>&nbsp;En espera</a>
                                </div>
                            ` : ``;
                        }
                    },
                    {
                        title: 'Comentarios', render: function (data, type, row, meta) {
                            if (row.evaluador > 0) {
                                return row.comentariosEvaluador;
                            } else {
                                return row.rutaEvidencia != null ? `<input class="form-control inputComentariosEvaluador" value="${row.comentariosEvaluador}"  title="${row.comentariosEvaluador}" ${row.evaluador > 0 || (_privilegioUsuario != 1 && _privilegioUsuario != 4) ? 'disabled' : ''}>` : ``;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center dt-nowrap", targets: [0, 2, 3, 4, 5] }
                ]
            });
        }

        function initTablaSeguimientoPropuestas() {
            dtSeguimientoPropuestas = tablaSeguimientoPropuestas.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                createdRow: function (row, rowData) {
                    if (rowData.evaluador > 0) {
                        $(row).addClass('seguimientoEvaluado');
                    }
                },
                initComplete: function (settings, json) {
                    tablaSeguimientoPropuestas.on('click', '.botonVerEvidenciaPropuesta', function () {
                        let rowData = dtSeguimientoPropuestas.row($(this).closest('tr')).data();

                        mostrarArchivoEvidenciaPropuesta(rowData.id);
                    });

                    tablaSeguimientoPropuestas.on('click', '.botonDescargarEvidenciaPropuesta', function () {
                        let rowData = dtSeguimientoPropuestas.row($(this).closest('tr')).data();

                        descargarArchivoEvidenciaPropuesta(rowData.id);
                    });

                    tablaSeguimientoPropuestas.on('change', 'input[type=file]', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtSeguimientoPropuestas.row(row).data();
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
                            botonEvidencia.addClass('btn-success');
                            botonEvidencia.removeClass('btn-default');
                            iconoBoton.addClass('fa-check');
                            iconoBoton.removeClass('fa-upload');
                        } else {
                            labelArchivo.text('');
                            botonEvidencia.addClass('btn-default');
                            botonEvidencia.removeClass('btn-success');
                            iconoBoton.addClass('fa-upload');
                            iconoBoton.removeClass('fa-check');
                        }
                    });

                    tablaSeguimientoPropuestas.on('click', '.radioBtn a', function () {
                        let rowData = dtSeguimientoPropuestas.row($(this).closest('tr')).data();
                        let div = $(this).closest('div');
                        let seleccion = $(this).attr('aprobada');

                        div.find(`a[data-toggle="radioAprobado${rowData.id}"]`).not(`[aprobada="${seleccion}"]`).removeClass('active').addClass('notActive');
                        div.find(`a[data-toggle="radioAprobado${rowData.id}"][aprobada="${seleccion}"]`).removeClass('notActive').addClass('active');
                    });
                },
                columns: [
                    { data: 'propuesta', title: 'Propuesta' },
                    {
                        title: 'Evidencia', render: function (data, type, row, meta) {
                            return `
                                <div>
                                    ${row.rutaEvidencia != null ? '<button class="btn btn-xs btn-default botonVerEvidenciaPropuesta"><i class="fa fa-eye"></i></button>' : ''}
                                    ${row.rutaEvidencia != null ? '<button class="btn btn-xs btn-default botonDescargarEvidenciaPropuesta"><i class="fa fa-download"></i></button>' : ''}
                                    ${row.rutaEvidencia == null ? `
                                        <div class="text-center">
                                            <label id="botonEvidencia_${row.id}" for="inputEvidencia_${row.id}" class="btn btn-xs btn-default"><i class="fa fa-upload"></i></label>
                                            <label id="labelArchivoEvidencia_${row.id}" class="labelArchivo"></label>
                                            <input id="inputEvidencia_${row.id}" type="file" class="inputEvidencia_${row.id}" accept="application/pdf, image/*">
                                        </div>
                                    ` : ''}
                                </div>
                            `;
                        }
                    },
                    {
                        title: 'Solventada', render: function (data, type, row, meta) {
                            // return row.rutaEvidencia != null ? `
                            //     <div>
                            //         <input type="checkbox" id="checkBoxPropuesta_${meta.row}" class="regular-checkbox" ${row.solventada ? `checked` : ``} ${row.evaluador > 0 || (_privilegioUsuario != 1 && _privilegioUsuario != 4) ? 'disabled' : ''}>
                            //         <label for="checkBoxPropuesta_${meta.row}"></label>
                            //     </div>
                            // ` : ``;

                            return row.rutaEvidencia != null ? `
                            <div class="radioBtn btn-group">
                                <a class="btn btn-success ${row.evaluador > 0 && row.solventada ? 'active' : 'notActive'}" data-toggle="radioAprobado${row.id}" aprobada="1" ${row.evaluador > 0 || (_privilegioUsuario != 1 && _privilegioUsuario != 4) ? 'disabled' : ''}><i class="fa fa-check"></i>&nbsp;SÍ</a>
                                <a class="btn btn-danger ${row.evaluador > 0 && !row.solventada ? 'active' : 'notActive'}" data-toggle="radioAprobado${row.id}" aprobada="0" ${row.evaluador > 0 || (_privilegioUsuario != 1 && _privilegioUsuario != 4) ? 'disabled' : ''}><i class="fa fa-times"></i>&nbsp;NO</a>
                                <a class="btn btn-primary ${row.evaluador == 0 ? 'active' : 'notActive'}" data-toggle="radioAprobado${row.id}" aprobada="2" ${row.evaluador > 0 || (_privilegioUsuario != 1 && _privilegioUsuario != 4) ? 'disabled' : ''}><i class="fa fa-square"></i>&nbsp;En espera</a>
                            </div>
                            ` : ``;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '10%', targets: [2] }
                ]
            });
        }

        function initTablaAreasAcciones() {
            dtAreasAcciones = tablaAreasAcciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'areaDesc', title: 'Áreas' },
                    { data: 'accionesRegistradas', title: 'Acciones Registradas' },
                    { data: 'proceso', title: 'En Proceso' },
                    { data: 'solventadas', title: 'Solventadas' },
                    {
                        data: 'porcentajeSolventadas', title: '% Acciones Solventadas', render: function (data, type, row, meta) {
                            return data + '%';
                        }
                    }
                ],
                initComplete: function (settings, json) {

                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle', 'targets': '_all' }
                ],
            });
        }

        function initTablaAreasAccionesDetalle() {
            dtAreasAccionesDetalle = tablaAreasAccionesDetalle.DataTable({
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
                    { data: 'cicloDesc', title: 'Ciclo' },
                    { data: 'accionDesc', title: 'Acción' },
                    { data: 'areaDesc', title: 'Área' },
                    { data: 'responsableNombre', title: 'Responsable' },
                    { data: 'fechaDeteccionString', title: 'Fecha Detección' },
                    { data: 'tiempoTranscurrido', title: 'Tiempo Transcurrido' }
                ],
                initComplete: function (settings, json) {

                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle', 'targets': '_all' }
                ],
            });
        }

        function mostrarArchivoEvidenciaAccion(id) {
            if (id > 0) {
                $.post('CargarDatosArchivoEvidenciaSeguimientoAcciones', { id })
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

        function descargarArchivoEvidenciaAccion(id) {
            if (id > 0) {
                location.href = `DescargarArchivoEvidenciaAccion?id=${id}`;
            }
        }

        function mostrarArchivoEvidenciaPropuesta(id) {
            if (id > 0) {
                $.post('CargarDatosArchivoEvidenciaSeguimientoPropuestas', { id })
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

        function descargarArchivoEvidenciaPropuesta(id) {
            if (id > 0) {
                location.href = `DescargarArchivoEvidenciaPropuesta?id=${id}`;
            }
        }

        function limpiarSeguimientoAccionesPropuestas() {
            inputTotalAcciones.val('');
            inputSolventadasAcciones.val('');
            inputProcesoAcciones.val('');
            inputPorcentajeAcciones.val('');
            dtSeguimientoAcciones.clear().draw();

            inputTotalPropuestas.val('');
            inputSolventadasPropuestas.val('');
            inputProcesoPropuestas.val('');
            inputPorcentajePropuestas.val('');
            dtSeguimientoPropuestas.clear().draw();
        }

        function buscarSeguimiento() {
            if (selectSeguimiento.val() == '') {
                AlertaGeneral(`Alerta`, `Debes seleccionar un tipo de seguimiento.`);
                return;
            }

            let listaCC = getValoresMultiples('#selectCentroCostoSeguimiento');
            let tipoSeguimiento = +selectSeguimiento.val();
            let fechaInicio = inputFechaInicioSeguimiento.val();
            let fechaFin = inputFechaFinSeguimiento.val();

            axios.post('GetListaSeguimiento', { listaCC, tipoSeguimiento, fechaInicio, fechaFin }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    if (tipoSeguimiento == 1) {
                        inputTotalAcciones.val(datos.totalAcciones);
                        inputSolventadasAcciones.val(datos.solventadas);
                        inputProcesoAcciones.val(datos.proceso);
                        inputPorcentajeAcciones.val(datos.porcentaje);
                        AddRows(tablaSeguimientoAcciones, datos.listaSeguimiento);
                    } else if (tipoSeguimiento == 2) {
                        inputTotalPropuestas.val(datos.totalPropuestas);
                        inputSolventadasPropuestas.val(datos.solventadas);
                        inputProcesoPropuestas.val(datos.proceso);
                        inputPorcentajePropuestas.val(datos.porcentaje);
                        AddRows(tablaSeguimientoPropuestas, datos.listaSeguimiento);
                    }
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarSeguimientoAcciones() {
            let captura = getInformacionSeguimientoAcciones();

            $.ajax({
                url: 'GuardarSeguimientoAcciones',
                data: captura,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                dtSeguimientoAcciones.clear().draw();

                if (response.success) {
                    AlertaGeneral(`Éxito`, `Se ha guardado la información.`);
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                dtSeguimientoAcciones.clear().draw();
            }
            );
        }

        function guardarSeguimientoPropuestas() {
            let captura = getInformacionSeguimientoPropuestas();

            $.ajax({
                url: 'GuardarSeguimientoPropuestas',
                data: captura,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                dtSeguimientoPropuestas.clear().draw();

                if (response.success) {
                    AlertaGeneral(`Éxito`, `Se ha guardado la información.`);
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                dtSeguimientoPropuestas.clear().draw();
            }
            );
        }

        function getInformacionSeguimientoAcciones() {
            const data = new FormData();
            let capturaEvidencias = [];
            let capturaEvaluaciones = [];

            tablaSeguimientoAcciones.find('tbody tr').each(function (index, row) {
                let rowData = dtSeguimientoAcciones.row(row).data();

                if (rowData.evaluador == 0) {
                    if (rowData.rutaEvidencia == null) { //Se está capturando la evidencia.
                        let inputEvidencia = $(row).find(`.inputEvidencia_${rowData.id}`);
                        let evidencia = inputEvidencia[0].files.length > 0 ? inputEvidencia[0].files[0] : null;

                        if (evidencia != null) {
                            capturaEvidencias.push({
                                id: rowData.id
                            });

                            data.append('evidencias', evidencia);
                        }
                    } else { //Se está capturando la evaluación de la evidencia (evaluador, aprobó y comentarios)
                        // let aprobo = $(row).find(`.regular-checkbox`).prop('checked');
                        let aprobo = +($(row).find(`.radioBtn a.active[data-toggle=radioAprobado${rowData.id}]`).attr('aprobada'));
                        if (aprobo < 2) {
                            let comentariosEvaluador = $(row).find('.inputComentariosEvaluador').val();

                            capturaEvaluaciones.push({
                                id: rowData.id,
                                aprobo: aprobo == 1,
                                comentariosEvaluador: comentariosEvaluador
                            });
                        }
                    }
                }
            });

            data.append('capturaEvidencias', JSON.stringify(capturaEvidencias));
            data.append('capturaEvaluaciones', JSON.stringify(capturaEvaluaciones));

            return data;
        }

        function getInformacionSeguimientoPropuestas() {
            const data = new FormData();
            let capturaEvidencias = [];
            let capturaEvaluaciones = [];

            tablaSeguimientoPropuestas.find('tbody tr').each(function (index, row) {
                let rowData = dtSeguimientoPropuestas.row(row).data();

                if (rowData.evaluador == 0) {
                    if (rowData.rutaEvidencia == null) { //Se está capturando la evidencia.
                        let inputEvidencia = $(row).find(`.inputEvidencia_${rowData.id}`);
                        let evidencia = inputEvidencia[0].files.length > 0 ? inputEvidencia[0].files[0] : null;

                        if (evidencia != null) {
                            capturaEvidencias.push({
                                id: rowData.id
                            });

                            data.append('evidencias', evidencia);
                        }
                    } else { //Se está capturando la evaluación de la evidencia (aprobó)
                        // let solventada = $(row).find(`.regular-checkbox`).prop('checked');
                        let solventada = +($(row).find(`.radioBtn a.active[data-toggle=radioAprobado${rowData.id}]`).attr('aprobada'));
                        if (solventada < 2) {
                            capturaEvaluaciones.push({
                                id: rowData.id,
                                solventada: solventada == 1
                            });
                        }
                    }
                }
            });

            data.append('capturaEvidencias', JSON.stringify(capturaEvidencias));
            data.append('capturaEvaluaciones', JSON.stringify(capturaEvaluaciones));

            return data;
        }

        function cargarDatosSeccion(seccion) {

            let listaCC = getValoresMultiples('#selectCentroCostoSeguimiento');
            let tipoSeguimiento = 1;
            let fechaInicio = inputFechaInicioSeguimiento.val();
            let fechaFin = inputFechaFinSeguimiento.val();
            let listaAreasSeguimientoHistorial = selectAreaSeguimiento.val().map((x) => { return +x; });

            axios.post('/Administrativo/Capacitacion/CargarDatosSeccionSeguimientoAcciones', { listaCC, tipoSeguimiento, fechaInicio, fechaFin, listaAreasSeguimientoHistorial, seccion }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    mostrarDatosSeccion(seccion, response.data);
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function mostrarDatosSeccion(seccion, response) {
            switch (seccion) {
                case SECCIONES.INDICADORES_ACCIONES_CICLO_TRABAJO:
                    inputSeguimientoDetalleTotalAcciones.val(response.totalAcciones);
                    inputSeguimientoDetalleAvance.val(response.avance);
                    inputSeguimientoDetalleSolventadas.val(response.solventadas);
                    inputSeguimientoDetalleProceso.val(response.proceso);
                    AddRows(tablaAreasAcciones, response.listaAreasAcciones);
                    panelIndicadoresAccionesCicloTrabajo.addClass('show');
                    break;
                case SECCIONES.PORCENTAJE_ACCIONES_SOLVENTADAS_AREA:
                    initChartPorcentajeAccionesSolventadasArea(response.data);
                    panelPorcentajeAccionesSolventadasArea.addClass('show');
                    break;
                case SECCIONES.TOTAL_ACCIONES_SOLVENTADAS_AREA:
                    initChartTotalAccionesSolventadasArea(response.data);
                    panelTotalAccionesSolventadasArea.addClass('show');
                    break;
                case SECCIONES.INDICADORES_ACCIONES_CICLO_TRABAJO_AREA:
                    AddRows(tablaAreasAccionesDetalle, response.data);
                    panelIndicadoresAccionesCicloTrabajoArea.addClass('show');
                    $.fn.dataTable.tables({ api: true }).columns.adjust();
                    break;
                case SECCIONES.HISTORICO_ACCIONES_SOLVENTADAS_AREA:
                    initChartHistoricoAccionesSolventadasArea(response.data);
                    panelHistoricoAccionesSolventadasArea.addClass('show');
                    break;
            }
        }

        function initChartPorcentajeAccionesSolventadasArea(datos) {

            let serie90 = [];

            for (let i = 0; i < datos.serie1.length; i++) {
                serie90.push(90);
            }

            Highcharts.chart('chartPorcentajeAccionesSolventadasArea', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'Porcentaje Acciones Solventadas Área' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: { text: '' },
                    labels: {
                        format: '{value}',
                    }
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,
                        zones: [
                            { value: 60, color: 'rgb(192, 0, 0)' },
                            { value: 86, color: 'rgb(255, 192, 0)' },
                            { color: 'rgb(0, 176, 80)' }
                        ],
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                if (this.y == 0) {
                                    return `El área no cuenta<br>con acciones<br>detectadas en el<br>periodo consultado`;
                                } else {
                                    return this.y;
                                }
                            }
                        },
                    },
                    series: {
                        dataLabels: {
                            // enabled: true
                        }
                    }
                },
                series: [
                    { name: '% solventadas', data: datos.serie1, dataLabels: { enabled: true }, color: 'rgb(0, 176, 80)' },
                    { name: '% Desempeño', data: serie90, color: '#DA6A1A', type: 'spline', dataLabels: { enabled: false }, enableMouseTracking: false, tooltip: { enabled: false }, },
                    // dashStyle: 'Dash',
                ],
                credits: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function initChartTotalAccionesSolventadasArea(datos) {
            Highcharts.chart('chartTotalAccionesSolventadasArea', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'Total Acciones Solventadas Área' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
                    allowDecimals: false,
                    labels: { format: '{value}' }
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    },
                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1, color: 'rgb(64, 64, 64)' },
                    { name: datos.serie2Descripcion, data: datos.serie2, color: 'rgb(237, 125, 49)' },
                    { name: datos.serie3Descripcion, data: datos.serie3, color: 'rgb(191, 191, 191)' },
                ],
                credits: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function initChartHistoricoAccionesSolventadasArea(datos) {
            Highcharts.chart('chartHistoricoAccionesSolventadasArea', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'Histórico Acciones Solventadas Área' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
                    allowDecimals: false,
                    labels: { format: '{value}' }
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    },
                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1, color: 'rgb(237, 125, 49)' },
                    { name: datos.serie2Descripcion, data: datos.serie2, color: 'rgb(191, 191, 191)' }
                ],
                credits: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }
        //#endregion

        //#region Dashboard
        function cargarInformacionDashboard() {
            let listaCC = getValoresMultiples('#selectCentroCostoDashboard');
            let fechaInicio = inputFechaInicioDashboard.val();
            let fechaFin = inputFechaFinDashboard.val();
            let listaAreas = [];

            getValoresMultiplesCustom('#selectAreaDashboard').forEach(x => {
                listaAreas.push({
                    cc: x.cc,
                    area: +x.departamento,
                    empresa: +x.empresa
                });
            });

            axios.post('CargarDashboardCiclos', { listaCC, listaAreas, fechaInicio, fechaFin })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        initGraficaIndicadores(response.data.graficaIndicadores);
                        h1CiclosRealizados.text('CICLOS REALIZADOS ' + response.data.ciclosRealizados);
                        AddRows(tablaDashboardCiclos, response.data.rowIndicadoresCiclos);
                        AddRows(tablaInspeccionesCumplidas, response.data.listaInspeccionesCumplidas);
                        AddRows(tablaAspectosColaboladores, response.data.listaAspectosCumplidas);
                        AddRows(tablaInspeccionesRealizarNuevamente, response.data.listaInspeccionesRealizarNuevamente);

                        AddRows(tablaDetallesCiclos, response.data.listaDetallesCiclos);
                        AddRows(tablaDetallesCiclosRealizados, response.data.listaDetallesCiclosRealizados);
                        AddRows(tablaDetallesAnual, response.data.listaDetallesAnual);
                        AddRows(tablaDetallesColaboradores, response.data.listaDetallesColaboradores);
                        initGraficaDetalles(response.data.graficaDetalles);

                        tdCiclosPeriodicos.text(response.data.promedioCiclosPeriodicos + '%');
                        tdCiclosLiberacion.text(response.data.promedioCiclosLiberacion + '%');
                        tdPromedioGeneral.text(response.data.promedioGeneral + '%');

                        $('#tabIndicador').removeClass('hide');
                        $('#tabDetalles').removeClass('hide');

                        $.fn.dataTable.tables({ api: true }).columns.adjust();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initGraficaIndicadores(datos) {
            Highcharts.chart('graficaIndicadores', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'Gráfica Indicadores' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
                    labels: { format: '{value}' }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: `
                        <tr>
                            <td style="color:{series.color};padding:0">{series.name}: </td>
                            <td><b>{point.y}</b></td>
                        </tr>`,
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1, color: 'green' },
                    { name: datos.serie2Descripcion, data: datos.serie2, color: 'red' }
                ],
                credits: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function initGraficaDetalles(datos) {
            Highcharts.chart('graficaDetalles', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'Gráfica Detalles' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
                    labels: { format: '{value}' }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: `
                        <tr>
                            <td style="color:{series.color};padding:0">{series.name}: </td>
                            <td><b>{point.y}</b></td>
                        </tr>`,
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1, color: 'green' },
                    { name: datos.serie2Descripcion, data: datos.serie2, color: 'red' }
                ],
                credits: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function initTablaInspeccionesCumplidas() {
            dtInspeccionesCumplidas = tablaInspeccionesCumplidas.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                order: [[1, "asc"]],
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'totalCiclos', title: 'Total Ciclos' },
                    {
                        title: 'Meses', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return row.mesDesc;
                            } else {
                                return row.mes;
                            }
                        }
                    },
                    {
                        title: 'Críticos', render: function (data, type, row, meta) {
                            return row.criticos > 0 ? `
                                <div class="progress" style="margin-bottom: 0px !important;">
                                    <div class="progress-bar progress-bar-primary" role="progressbar" aria-valuenow="${row.criticos}" aria-valuemin="0" aria-valuemax="100" style="width: ${row.criticos}%;">
                                        ${row.criticos}%
                                    </div>
                                </div>
                            ` : `0%`;
                        }
                    },
                    {
                        title: 'Medios', render: function (data, type, row, meta) {
                            return row.medios > 0 ? `
                                <div class="progress" style="margin-bottom: 0px !important;">
                                    <div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="${row.medios}" aria-valuemin="0" aria-valuemax="100" style="width: ${row.medios}%;">
                                        ${row.medios}%
                                    </div>
                                </div>
                            ` : `0%`;
                        }
                    },
                    {
                        title: 'Bajos', render: function (data, type, row, meta) {
                            return row.bajos > 0 ? `
                                <div class="progress" style="margin-bottom: 0px !important;">
                                    <div class="progress-bar progress-bar-warning" role="progressbar" aria-valuenow="${row.bajos}" aria-valuemin="0" aria-valuemax="100" style="width: ${row.bajos}%;">
                                        ${row.bajos}%
                                    </div>
                                </div>
                            ` : `0%`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaAspectosColaboladores() {
            dtAspectosColaborades = tablaAspectosColaboladores.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                order: [[1, "asc"]],
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'totalCiclos', title: 'Total Ciclos' },
                    {
                        title: 'Meses', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return row.mesDesc;
                            } else {
                                return row.mes;
                            }
                        }
                    },
                    {
                        title: 'Técnica operacional', render: function (data, type, row, meta) {
                            return row.tecnicaOperacional > 0 ? `
                                <div class="progress" style="margin-bottom: 0px !important;">
                                    <div class="progress-bar progress-bar-primary" role="progressbar" aria-valuenow="${row.tecnicaOperacional}" aria-valuemin="0" aria-valuemax="100" style="width: ${row.tecnicaOperacional}%;">
                                        ${row.tecnicaOperacional}%
                                    </div>
                                </div>
                            ` : `0%`;
                        }
                    },
                    {
                        title: 'Comportamiento seguro', render: function (data, type, row, meta) {
                            return row.comportamientoSeguro > 0 ? `
                                <div class="progress" style="margin-bottom: 0px !important;">
                                    <div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="${row.comportamientoSeguro}" aria-valuemin="0" aria-valuemax="100" style="width: ${row.comportamientoSeguro}%;">
                                        ${row.comportamientoSeguro}%
                                    </div>
                                </div>
                            ` : `0%`;
                        }
                    },
                    {
                        title: 'Mantenimiento Técnico', render: function (data, type, row, meta) {
                            return row.mantenimientoTecnico > 0 ? `
                                <div class="progress" style="margin-bottom: 0px !important;">
                                    <div class="progress-bar progress-bar-warning" role="progressbar" aria-valuenow="${row.mantenimientoTecnico}" aria-valuemin="0" aria-valuemax="100" style="width: ${row.mantenimientoTecnico}%;">
                                        ${row.mantenimientoTecnico}%
                                    </div>
                                </div>
                            ` : `0%`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaInspeccionesRealizarNuevamente() {
            dtInspeccionesRealizarNuevamente = tablaInspeccionesRealizarNuevamente.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                scrollY: '45vh',
                scrollCollapse: true,
                // scrollX: 'auto',
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'criterio', title: '' },
                    { data: 'inspeccion', title: 'Ciclo' },//ciclo
                    { data: 'cantidad', title: '#' },
                    { data: 'enero', title: 'Enero' },
                    { data: 'febrero', title: 'Febrero' },
                    { data: 'marzo', title: 'Marzo' },
                    { data: 'abril', title: 'Abril' },
                    { data: 'mayo', title: 'Mayo' },
                    { data: 'junio', title: 'Junio' },
                    { data: 'julio', title: 'Julio' },
                    { data: 'agosto', title: 'Agosto' },
                    { data: 'septiembre', title: 'Septiembre' },
                    { data: 'octubre', title: 'Octubre' },
                    { data: 'noviembre', title: 'Noviembre' },
                    { data: 'diciembre', title: 'Diciembre' }
                ],
                columnDefs: [
                    {
                        className: "dt-center", "targets": "_all",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData[3] > 0) {
                                $(td).css('color', '#F8CBAD');
                            }
                        }
                    },
                ],
                "createdRow": function (row, data, index) {
                    if (data["enero"] > 0) {
                        $('td', row).eq(3).css('background-color', '#F8CBAD');
                    }
                    if (data["febrero"] > 0) {
                        $('td', row).eq(4).css('background-color', '#F8CBAD');
                    }
                    if (data["marzo"] > 0) {
                        $('td', row).eq(5).css('background-color', '#F8CBAD');
                    }
                    if (data["abril"] > 0) {
                        $('td', row).eq(6).css('background-color', '#F8CBAD');
                    }
                    if (data["mayo"] > 0) {
                        $('td', row).eq(7).css('background-color', '#F8CBAD');
                    }
                    if (data["junio"] > 0) {
                        $('td', row).eq(8).css('background-color', '#F8CBAD');
                    }
                    if (data["julio"] > 0) {
                        $('td', row).eq(9).css('background-color', '#F8CBAD');
                    }
                    if (data["agosto"] > 0) {
                        $('td', row).eq(10).css('background-color', '#F8CBAD');
                    }
                    if (data["septiembre"] > 0) {
                        $('td', row).eq(11).css('background-color', '#F8CBAD');
                    }
                    if (data["octubre"] > 0) {
                        $('td', row).eq(12).css('background-color', '#F8CBAD');
                    }
                    if (data["noviembre"] > 0) {
                        $('td', row).eq(13).css('background-color', '#F8CBAD');
                    }
                    if (data["diciembre"] > 0) {
                        $('td', row).eq(14).css('background-color', '#F8CBAD');
                    }
                },
            });
        }

        function initTablaDetallesCiclos() {
            dtDetallesCiclos = tablaDetallesCiclos.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'ciclosAcreditados', title: 'Ciclos Acreditados' },
                    { data: 'ciclosNoAcreditados', title: 'Ciclos NO Acreditados' },
                    { data: 'retroalimentaciones', title: 'Retroalimentaciones' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaDetallesCiclosRealizados() {
            dtDetallesCiclosRealizados = tablaDetallesCiclosRealizados.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'ciclosLiberacion', title: 'Liberacion' },
                    { data: 'ciclosPeriodicos', title: 'Periodicos' },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaDetallesAnual() {
            dtDetallesAnual = tablaDetallesAnual.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                scrollX: 'auto',
                scrollY: '500px',
                scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'cicloDesc', title: '' },
                    { data: 'enero', title: 'Enero' },
                    { data: 'febrero', title: 'Febrero' },
                    { data: 'marzo', title: 'Marzo' },
                    { data: 'abril', title: 'Abril' },
                    { data: 'mayo', title: 'Mayo' },
                    { data: 'junio', title: 'Junio' },
                    { data: 'julio', title: 'Julio' },
                    { data: 'agosto', title: 'Agosto' },
                    { data: 'septiembre', title: 'Septiembre' },
                    { data: 'octubre', title: 'Octubre' },
                    { data: 'noviembre', title: 'Noviembre' },
                    { data: 'diciembre', title: 'Diciembre' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                "createdRow": function (row, data, index) {
                    if (data["enero"] > 0) {
                        $('td', row).eq(1).css('background-color', '#F8CBAD');
                    }
                    if (data["febrero"] > 0) {
                        $('td', row).eq(2).css('background-color', '#F8CBAD');
                    }
                    if (data["marzo"] > 0) {
                        $('td', row).eq(3).css('background-color', '#F8CBAD');
                    }
                    if (data["abril"] > 0) {
                        $('td', row).eq(4).css('background-color', '#F8CBAD');
                    }
                    if (data["mayo"] > 0) {
                        $('td', row).eq(5).css('background-color', '#F8CBAD');
                    }
                    if (data["junio"] > 0) {
                        $('td', row).eq(6).css('background-color', '#F8CBAD');
                    }
                    if (data["julio"] > 0) {
                        $('td', row).eq(7).css('background-color', '#F8CBAD');
                    }
                    if (data["agosto"] > 0) {
                        $('td', row).eq(8).css('background-color', '#F8CBAD');
                    }
                    if (data["septiembre"] > 0) {
                        $('td', row).eq(9).css('background-color', '#F8CBAD');
                    }
                    if (data["octubre"] > 0) {
                        $('td', row).eq(10).css('background-color', '#F8CBAD');
                    }
                    if (data["noviembre"] > 0) {
                        $('td', row).eq(11).css('background-color', '#F8CBAD');
                    }
                    if (data["diciembre"] > 0) {
                        $('td', row).eq(12).css('background-color', '#F8CBAD');
                    }
                },
            });
        }

        function initTablaDetallesColaboradores() {
            dtDetallesColaboradores = tablaDetallesColaboradores.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                createdRow: function (row, rowData) {
                    if (rowData.ciclos > 0) {
                        $(row).addClass('renglonColaboradorConCiclos');
                    }
                },
                columns: [
                    { data: 'colaborador', title: 'Colaboradores Operativos' },
                    { data: 'ciclos', title: 'Ciclos' },
                    { data: 'calificacion', title: 'Calificación' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTblDashboardCiclos() {
            dtDashboardCiclos = tablaDashboardCiclos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'ciclosRealizados', title: 'Ciclos Realizados' },
                    { data: 'ciclosRequeridos', title: 'Ciclos Requeridos' },
                    { data: 'porcCumplimiento', title: '% de Cumplimiento', render: function (data, type, row) { return (data.toFixed(2) + " %") } },
                ],
                initComplete: function (settings, json) {
                    tablaDashboardCiclos.on('click', '.classBtn', function () {
                        let rowData = dtDashboardCiclos.row($(this).closest('tr')).data();
                    });
                    tablaDashboardCiclos.on('click', '.classBtn', function () {
                        let rowData = dtDashboardCiclos.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }
        //#endregion

        //#region Crear
        function initTablaCriterios() {
            dtCriterios = tablaCriterios.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaCriterios.on('click', '.botonQuitarCriterio', function () {
                        let row = $(this).closest('tr');

                        dtCriterios.row(row).remove().draw();

                        let cuerpo = tablaCriterios.find('tbody');

                        if (cuerpo.find("tr").length == 0) {
                            dtCriterios.draw();
                        } else {
                            tablaCriterios.find('tbody tr').each(function (idx, row) {
                                let rowData = dtCriterios.row(row).data();

                                if (rowData != undefined) {
                                    rowData.id = rowData.id;
                                    rowData.descripcion = rowData.descripcion;
                                    rowData.ponderacion = rowData.ponderacion;

                                    dtCriterios.row(row).data(rowData).draw();
                                }
                            });
                        }
                    });
                },
                createdRow: function (row, rowData) {
                    if (rowData.aspectoEvaluado > 0) {
                        $(row).find('.selectAspectoEvaluado').val(rowData.aspectoEvaluado);
                    }
                },
                columns: [
                    { data: 'descripcion', title: 'Criterio', sortable: false },
                    {
                        data: 'ponderacion', title: 'Ponderación', sortable: false, render: function (data, type, row, meta) {
                            switch (data) {
                                case 1:
                                    return 'Bajo';
                                case 2:
                                    return 'Medio';
                                case 3:
                                    return 'Crítico';
                                default:
                                    return 'No Asignado';
                            }
                        }
                    },
                    {
                        title: 'Aspecto Evaluado', render: function (data, type, row, meta) {
                            return `
                                <select class="form-control selectAspectoEvaluado">
                                    <option value="">--Seleccione--</option>
                                    <option value="1">Comportamiento Seguro</option>
                                    <option value="2">Técnica Operacional</option>
                                    <option value="3">Mantenimiento Técnico</option>
                                </select>
                            `;
                        }
                    },
                    {
                        title: 'Quitar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-danger botonQuitarCriterio"><i class="fa fa-times"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function guardarCiclo() {
            if (validarCamposCrearCiclo() > 0) {
                AlertaGeneral(`Alerta`, `Debe capturar todos los campos.`);
                return;
            }

            let ciclo = {
                titulo: inputTitulo.val(),
                descripcion: inputDescripcion.val(),
                tipoCiclo: +divRadioTipoCiclo.find('input[name=tipoCiclo]:checked').val(),
                fechaCiclo: null,
                fechaCreacion: null,
                division: 0,
                estatus: true
            }

            let listaAreas = [];

            getValoresMultiplesCustomDepartamento('#selectDepartamentoCrear').forEach(x => {
                listaAreas.push({
                    id: 0,
                    cc: x.cc,
                    area: +x.departamento,
                    empresa: +x.empresa,
                    cicloTrabajoID: 0,
                    estatus: true
                });
            });

            let listaCriterios = [];

            tablaCriterios.find('tbody tr').each(function (index, row) {
                let rowData = dtCriterios.row(row).data();

                listaCriterios.push({
                    id: 0,
                    descripcion: rowData.descripcion,
                    ponderacion: rowData.ponderacion,
                    aspectoEvaluado: +$(row).find('.selectAspectoEvaluado').val(),
                    cicloTrabajoID: 0,
                    estatus: true
                });
            });

            axios.post('GuardarNuevoCiclo', { ciclo, listaAreas, listaCriterios })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AlertaGeneral(`Alerta`, `Se guardó la información.`);
                        fncTablaCiclosTrabajo();
                        mdlRegistro.modal("hide");
                        limpiarTabCrear();

                        //Se vuelve a cargar el combo de ciclos en el tab de ciclos de trabajo.
                        selectCicloTrabajo.fillCombo('GetCiclosTrabajoCombo', null, false, '');
                        selectCicloTrabajo.find('option[value=""]').remove();
                        selectCicloTrabajo.select2(); // convertToMultiselect('#selectCicloTrabajo');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function validarCamposCrearCiclo() {
            let contadorCampoInvalido = 0;

            if (inputTitulo.val().trim().length == 0) {
                contadorCampoInvalido++;
            }

            if (inputDescripcion.val().trim().length == 0) {
                contadorCampoInvalido++;
            }

            if (!radioPeriodico.prop('checked') && !radioLiberacion.prop('checked')) {
                contadorCampoInvalido++;
            }

            if (getValoresMultiples('#selectDepartamentoCrear').length == 0) {
                contadorCampoInvalido++;
            }

            if (tablaCriterios.find('tbody tr').legnth == 0) {
                contadorCampoInvalido++;
            }

            return contadorCampoInvalido;
        }

        botonAgregarCriterio.on('click', function () {
            if (+selectPonderacion.val() > 0) {
                let datos = dtCriterios.rows().data();

                $.each(datos, function (idx, data) {
                    let row = tablaCriterios.find('tbody tr').eq(idx);
                    let rowData = dtCriterios.row(row).data();

                    if (rowData != undefined) {
                        data.descripcion = rowData.descripcion;
                        data.ponderacion = rowData.ponderacion;
                    }
                });

                datos.push({
                    descripcion: inputCriterio.val(),
                    ponderacion: +selectPonderacion.val()
                });

                dtCriterios.clear();
                dtCriterios.rows.add(datos).draw();

                inputCriterio.val('');
                selectPonderacion.val('');
            } else {
                AlertaGeneral(`Alerta`, `Debe seleccionar la ponderación.`);
            }
        });

        function limpiarTabCrear() {
            inputTitulo.val('');
            inputDescripcion.val('');
            radioPeriodico.prop('checked', false);
            radioLiberacion.prop('checked', false);
            selectDepartamentoCrear.val('');
            selectDepartamentoCrear.multiselect('deselectAll', false);
            inputCriterio.val('');
            selectPonderacion.val('');
            dtCriterios.clear().draw();
        }

        function getValoresMultiplesCustom(selector) {
            var _tempObj = $(selector + ' option:selected').map(function (a, item) {
                return { value: item.value, empresa: $(item).attr('empresa'), departamento: item.value, cc: $(item).attr('cc') };
            });
            var _tempArrObj = new Array();
            $.each(_tempObj, function (i, e) {
                _tempArrObj.push(e);
            });
            return _tempArrObj;
        }

        function getValoresMultiplesCustomDepartamento(selector) {
            var _tempObj = $(selector + ' option:selected').map(function (a, item) {
                return { value: item.value, empresa: $(item).attr('empresa'), departamento: $(item).attr('departamento'), cc: $(item).attr('cc') };
            });
            var _tempArrObj = new Array();
            $.each(_tempObj, function (i, e) {
                _tempArrObj.push(e);
            });
            return _tempArrObj;
        }
        //#endregion

        function cargarAreasCC() {
            let listaCentrosCosto = [];

            getValoresMultiplesCustom('#selectCentroCosto').forEach(x => {
                listaCentrosCosto.push({
                    cc: x.value,
                    empresa: +(x.empresa)
                });
            });

            if (listaCentrosCosto.length == 0) {
                selectArea.empty();
                convertToMultiselect('#selectArea');
                return;
            }

            let ccsCplan = listaCentrosCosto.filter((x) => { return x.empresa == 1 || x.empresa == 6; }).map(function (x) { return x.cc; });
            let ccsArr = listaCentrosCosto.filter((x) => { return x.empresa == 2; }).map(function (x) { return x.cc; });

            $.post('/Administrativo/Capacitacion/ObtenerAreasPorCC', { ccsCplan, ccsArr })
                .then(response => {
                    selectArea.empty();
                    if (response.success) {
                        // Operación exitosa.
                        // const todosOption = `<option value="Todos">Todos</option>`;
                        const option = `<option value="Todos">Todos</option>`;
                        selectArea.append(option);

                        response.items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" cc="${y.Id}" empresa="${y.Prefijo}">${y.Text}</option>`;
                            });
                            selectArea.append(groupOption);
                        });

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }

                    convertToMultiselect('#selectArea');
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cargarAreasCCRegistroCiclo() {
            let listaCentrosCosto = [];

            getValoresMultiplesCustom('#selectCentroCostoRegistroCiclo').forEach(x => {
                listaCentrosCosto.push({
                    cc: x.value,
                    empresa: +(x.empresa)
                });
            });

            if (listaCentrosCosto.filter((x) => x.cc != '').length == 0) {
                selectAreaRegistroCiclo.empty();
                return;
            }

            let ccsCplan = listaCentrosCosto.filter((x) => { return x.empresa == 1 || x.empresa == 6; }).map(function (x) { return x.cc; });
            let ccsArr = listaCentrosCosto.filter((x) => { return x.empresa == 2; }).map(function (x) { return x.cc; });

            $.post('/Administrativo/Capacitacion/ObtenerAreasPorCC', { ccsCplan, ccsArr })
                .then(response => {
                    selectAreaRegistroCiclo.empty();
                    if (response.success) {
                        // Operación exitosa.
                        const todosOption = `<option value="Todos">Todos</option>`;
                        selectAreaRegistroCiclo.append(todosOption);

                        response.items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" cc="${y.Id}" empresa="${y.Prefijo}">${y.Text}</option>`;
                            });
                            selectAreaRegistroCiclo.append(groupOption);
                        });

                        convertToMultiselect('#selectAreaRegistroCiclo');
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

        function cargarAreasCCDashboard() {
            let listaCentrosCosto = [];

            getValoresMultiplesCustom('#selectCentroCostoDashboard').forEach(x => {
                listaCentrosCosto.push({
                    cc: x.value,
                    empresa: +(x.empresa)
                });
            });

            if (listaCentrosCosto.length == 0) {
                selectAreaDashboard.empty();
                convertToMultiselect('#selectAreaDashboard');
                return;
            }

            let ccsCplan = listaCentrosCosto.filter((x) => { return (x.empresa == 1 || x.empresa == 6); }).map(function (x) { return x.cc; });
            let ccsArr = listaCentrosCosto.filter((x) => { return x.empresa == 2; }).map(function (x) { return x.cc; });

            $.post('/Administrativo/Capacitacion/ObtenerAreasPorCC', { ccsCplan, ccsArr })
                .then(response => {
                    selectAreaDashboard.empty();
                    if (response.success) {
                        // Operación exitosa.
                        // const todosOption = `<option value="Todos">Todos</option>`;
                        const option = `<option value="Todos">Todos</option>`;
                        selectAreaDashboard.append(option);

                        response.items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" cc="${y.Id}" empresa="${y.Prefijo}">${y.Text}</option>`;
                            });
                            selectAreaDashboard.append(groupOption);
                        });

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }

                    convertToMultiselect('#selectAreaDashboard');
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Seguridad.CiclosTrabajo = new CiclosTrabajo())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...', baseZ: 9000 }))
        .ajaxStop($.unblockUI);
})();