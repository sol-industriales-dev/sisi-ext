(() => {
    $.namespace('EvaluacionSubcontratista.Catalogos');
    Catalogos = function () {
        //#region Selectores
        //#region Plantilla
        const selectPlantilla = $('#selectPlantilla');
        const selectContratos = $('#selectContratos');
        const botonBuscarPlantillasCreadas = $('#botonBuscarPlantillasCreadas');
        const botonNuevaPlantilla = $('#botonNuevaPlantilla');
        const tablaPlantillasCreadas = $('#tablaPlantillasCreadas');
        const modalPlantilla = $('#modalPlantilla');
        const inputNombrePlantilla = $('#inputNombrePlantilla');
        const selectContratosPlantilla = $('#selectContratosPlantilla');
        const inputColaboradorPlantilla = $('#inputColaboradorPlantilla');
        const inputFechaPlantilla = $('#inputFechaPlantilla');
        const checkBase = $('#checkBase');
        const selectElementoEvaluarPlantilla = $('#selectElementoEvaluarPlantilla');
        const botonNuevoElementoPlantilla = $('#botonNuevoElementoPlantilla');
        const botonNuevoRequerimientoPlantilla = $('#botonNuevoRequerimientoPlantilla');
        const tablaElementosPlantilla = $('#tablaElementosPlantilla');
        const botonGuardarPlantilla = $('#botonGuardarPlantilla');
        const modalNuevoElemento = $('#modalNuevoElemento');
        const inputNuevoElementoClave = $('#inputNuevoElementoClave');
        const inputNuevoElementoNombre = $('#inputNuevoElementoNombre');
        const inputMensajeElementoPlantilla = $('#inputMensajeElementoPlantilla');
        const botonGuardarNuevoElemento = $('#botonGuardarNuevoElemento');
        const inputNuevoElementoMensaje = $('#inputNuevoElementoMensaje');
        const botonAgregarElemento = $('#botonAgregarElemento');
        const inputPonderacionElementoPlantilla = $('#inputPonderacionElementoPlantilla');
        const inputNuevoRequerimientoDescripcion = $('#inputNuevoRequerimientoDescripcion');
        const botonGuardarNuevoRequerimiento = $('#botonGuardarNuevoRequerimiento');
        const modalNuevoRequerimiento = $('#modalNuevoRequerimiento');
        const selectRequerimientoPlantilla = $('#selectRequerimientoPlantilla');
        //#endregion

        //#region Evaluadores
        const selectProyectoEvaluadores = $('#selectProyectoEvaluadores');
        const selectElemento = $('#selectElemento');
        const botonBuscarEvaluadores = $('#botonBuscarEvaluadores');
        const botonNuevoEvaluador = $('#botonNuevoEvaluador');
        const tablaEvaluadores = $('#tablaEvaluadores');
        const modalNuevoEvaluador = $('#modalNuevoEvaluador');
        const inputEvaluador = $('#inputEvaluador');
        const selectProyectoNuevoEvaluador = $('#selectProyectoNuevoEvaluador');
        const selectElementoNuevoEvaluador = $('#selectElementoNuevoEvaluador');
        const botonGuardarNuevoEvaluador = $('#botonGuardarNuevoEvaluador');
        //#endregion

        //#region Facultamiento
        const selectProyectoFacultamientos = $('#selectProyectoFacultamientos');
        const selectTipoFacultamiento = $('#selectTipoFacultamiento');
        const botonBuscarFacultamientos = $('#botonBuscarFacultamientos');
        const botonNuevoFacultamiento = $('#botonNuevoFacultamiento');
        const tablaFacultamientos = $('#tablaFacultamientos');
        const modalNuevoFacultamiento = $('#modalNuevoFacultamiento');
        const inputColaboradorFacultamiento = $('#inputColaboradorFacultamiento');
        const selectProyectoNuevoFacultamiento = $('#selectProyectoNuevoFacultamiento');
        const selectPrivilegioNuevoFacultamiento = $('#selectPrivilegioNuevoFacultamiento');
        const botonGuardarNuevoFacultamiento = $('#botonGuardarNuevoFacultamiento');
        const mdlListadoCCRelUsuario = $('#mdlListadoCCRelUsuario')
        const tblListadoCCRelUsuario = $('#tblListadoCCRelUsuario')
        //#endregion

        //#region Firma Subcontratista
        const selectSubcontratista = $('#selectSubcontratista');
        const botonBuscarFirmas = $('#botonBuscarFirmas');
        const botonNuevaFirma = $('#botonNuevaFirma');
        const tablaFirmas = $('#tablaFirmas');
        const modalNuevaFirma = $('#modalNuevaFirma');
        const selectSubcontratistaNuevo = $('#selectSubcontratistaNuevo');
        const inputNombreFirma = $('#inputNombreFirma');
        const inputCorreoFirma = $('#inputCorreoFirma');
        const selectContratoFirma = $('#selectContratoFirma');
        const botonGuardarNuevaFirma = $('#botonGuardarNuevaFirma');
        //#endregion

        //#region Especialidad
        const tablaSubcontratistasEspecialidades = $('#tablaSubcontratistasEspecialidades');
        const selectProyectoEspecialidad = $('#selectProyectoEspecialidad');
        const botonBuscarSubcontratistasEspecialidad = $('#botonBuscarSubcontratistasEspecialidad');
        //#endregion
        //#endregion

        //#region Firma Gerente
        const selectCentroCosto = $('#selectCentroCosto');
        const botonBuscarFirmasGerente = $('#botonBuscarFirmasGerente');
        const botonNuevaFirmaGerente = $('#botonNuevaFirmaGerente');
        const tablaFirmasGerente = $('#tablaFirmasGerente');
        const modalNuevaFirmaGerente = $('#modalNuevaFirmaGerente');
        const inputUsuarioGerente = $('#inputUsuarioGerente');
        const selectCentroCostoNuevaFirmaGerente = $('#selectCentroCostoNuevaFirmaGerente');
        const botonGuardarNuevaFirmaGerente = $('#botonGuardarNuevaFirmaGerente');
        //#endregion

        let dtPlantillasCreadas;
        let dtElementosPlantilla;
        let dtEvaluadores;
        let dtFacultamientos;
        let dtFirmas;
        let dtSubcontratistasEspecialidades;
        let dtFirmasGerente;

        let _facultamientoUsuario = 0;

        const ESTATUS = {
            NUEVO: 0,
            EDITAR: 1
        };
        const FACULTAMIENTOS = {
            NO_ASIGNADO: 0,
            ADMINISTRADOR_PMO: 1,
            ADMINISTRADOR: 2,
            EVALUADOR: 3,
            CONSULTA: 4
        };

        (function init() {
            $('.select2').select2();

            getFacultamientoUsuario();

            //#region Plantilla
            initTablaPlantillasCreadas();
            initTablaElementosPlantilla();
            initTblListadoCCRelUsuario();

            botonBuscarPlantillasCreadas.click(cargarPlantillasCreadas);
            botonNuevaPlantilla.click(() => {
                limpiarModalPlantilla();
                mostrarBotonesModal();
                botonNuevaPlantilla.data().estatus = ESTATUS.NUEVO;
                botonNuevaPlantilla.data().id = 0;
                modalPlantilla.modal('show');
            });
            botonNuevoElementoPlantilla.click(() => {
                inputNuevoElementoClave.val('');
                inputNuevoElementoNombre.val('');
                inputNuevoElementoMensaje.val('');

                modalNuevoElemento.modal('show');
                modalNuevoElemento.css('z-index', 1501);
                $('.modal-backdrop:eq(1)').css('z-index', 1500);
            });
            botonNuevoRequerimientoPlantilla.click(() => {
                let elemento_id = +selectElementoEvaluarPlantilla.val();

                if (elemento_id > 0) {
                    inputNuevoRequerimientoDescripcion.val('');

                    botonGuardarNuevoRequerimiento.data().elemento_id = elemento_id;

                    modalNuevoRequerimiento.modal('show');
                    modalNuevoRequerimiento.css('z-index', 1501);
                    $('.modal-backdrop:eq(1)').css('z-index', 1500);
                } else {
                    Alert2Warning('Debe seleccionar un elemento.');
                }
            });
            botonGuardarNuevoElemento.click(guardarNuevoElemento);
            botonGuardarNuevoRequerimiento.click(guardarNuevoRequerimiento);
            botonAgregarElemento.click(agregarElementoPlantilla);
            botonGuardarPlantilla.click(guardarPlantilla);

            inputFechaPlantilla.datepicker({ format: 'dd/mm/yy', showAnim: 'slide' });
            inputColaboradorPlantilla.getAutocompleteValid(setDatosColaborador, verificarColaborador, { porClave: false }, 'GetUsuariosAutocomplete');

            selectPlantilla.fillCombo('FillComboPlantillas', null, false, null);
            selectContratos.fillCombo('FillComboContratos', null, false, null);
            selectContratosPlantilla.fillCombo('FillComboContratos', null, false, null);
            selectContratosPlantilla.find('option[value=""]').remove();
            selectElementoEvaluarPlantilla.fillCombo('FillComboElementos', null, false, null);
            //#endregion

            //#region Evaluadores
            initTablaEvaluadores();

            selectProyectoEvaluadores.fillCombo('FillComboProyectos', null, false, null);
            selectElemento.fillCombo('FillComboElementos', null, false, null);
            selectProyectoNuevoEvaluador.fillCombo('FillComboProyectos', null, false, null);
            selectProyectoNuevoEvaluador.find('option[value=""]').remove();
            selectElementoNuevoEvaluador.fillCombo('FillComboElementos', null, false, null);
            selectElementoNuevoEvaluador.find('option[value=""]').remove();

            inputEvaluador.getAutocompleteValid(setDatosEvaluador, verificarEvaluador, { porClave: false }, 'GetUsuariosAutocomplete');

            botonNuevoEvaluador.click(() => {
                inputEvaluador.val('');
                inputEvaluador.data().usuario_id = null;
                selectProyectoNuevoEvaluador.val('');
                selectProyectoNuevoEvaluador.select2().change();
                selectElementoNuevoEvaluador.val('');
                selectElementoNuevoEvaluador.select2().change();

                botonNuevoEvaluador.data().estatus = ESTATUS.NUEVO;
                botonNuevoEvaluador.data().evaluador_id = 0;

                modalNuevoEvaluador.modal('show');
            });
            botonGuardarNuevoEvaluador.click(guardarEvaluador);
            botonBuscarEvaluadores.click(cargarEvaluadores);
            //#endregion

            //#region Facultamiento
            initTablaFacultamientos();

            selectProyectoFacultamientos.fillCombo('FillComboProyectos', null, false, null);
            selectTipoFacultamiento.fillCombo('GetTipoFacultamientoCombo', null, false, null);
            selectProyectoNuevoFacultamiento.fillCombo('FillComboProyectos', null, false, null);
            selectProyectoNuevoFacultamiento.find('option[value=""]').remove();
            selectPrivilegioNuevoFacultamiento.fillCombo('GetTipoFacultamientoCombo', null, false, null);

            inputColaboradorFacultamiento.getAutocompleteValid(setDatosColaboradorFacultamiento, verificarColaboradorFacultamiento, { porClave: false }, 'GetUsuariosAutocomplete');

            botonNuevoFacultamiento.click(() => {
                inputColaboradorFacultamiento.val('');
                inputColaboradorFacultamiento.data().usuario_id = null;
                selectProyectoNuevoFacultamiento.val('');
                selectProyectoNuevoFacultamiento.select2().change();
                selectPrivilegioNuevoFacultamiento.val('');

                botonNuevoFacultamiento.data().estatus = ESTATUS.NUEVO;
                botonNuevoFacultamiento.data().facultamiento_id = 0;

                modalNuevoFacultamiento.modal('show');
            });
            botonGuardarNuevoFacultamiento.click(guardarFacultamiento);
            botonBuscarFacultamientos.click(cargarFacultamientos);
            //#endregion

            //#region Firma Subcontratista
            initTablaFirmas();

            selectSubcontratista.fillCombo('FillComboSubcontratistas', null, false, null);
            selectSubcontratistaNuevo.fillCombo('FillComboSubcontratistas', null, false, null);
            selectContratoFirma.fillCombo('FillComboContratos', null, false, null);
            selectContratoFirma.find('option[value=""]').remove();

            botonNuevaFirma.click(() => {
                selectSubcontratistaNuevo.val('');
                selectSubcontratistaNuevo.select2().change();
                inputNombreFirma.val('');
                inputCorreoFirma.val('');
                selectContratoFirma.fillCombo('FillComboContratos', null, false, null);
                selectContratoFirma.find('option[value=""]').remove();

                botonNuevaFirma.data().estatus = ESTATUS.NUEVO;
                botonNuevaFirma.data().firma_id = 0;

                modalNuevaFirma.modal('show');
            });
            botonGuardarNuevaFirma.click(guardarFirma);
            botonBuscarFirmas.click(cargarFirmas);
            //#endregion

            //#region Especialidad
            initTablaSubcontratistasEspecialidades();
            cargarSubcontratistasEspecialidades();

            selectProyectoEspecialidad.fillCombo('FillComboProyectos', null, false, null);
            botonBuscarSubcontratistasEspecialidad.click(cargarSubcontratistasEspecialidades);
            //#endregion

            //#region Firma Gerente
            initTablaFirmasGerente();

            inputUsuarioGerente.getAutocompleteValid(setDatosGerente, verificarGerente, { porClave: false }, 'GetUsuariosAutocomplete');
            selectCentroCosto.fillCombo('FillComboProyectos', null, false, null);
            selectCentroCostoNuevaFirmaGerente.fillCombo('FillComboProyectos', null, false, null);

            botonNuevaFirmaGerente.click(() => {
                selectCentroCostoNuevaFirmaGerente.val('');
                selectCentroCostoNuevaFirmaGerente.select2().change();

                inputUsuarioGerente.val('');
                inputUsuarioGerente.data().usuario_id = null;

                botonNuevaFirmaGerente.data().estatus = ESTATUS.NUEVO;
                botonNuevaFirmaGerente.data().firma_id = 0;

                modalNuevaFirmaGerente.modal('show');
            });
            botonGuardarNuevaFirmaGerente.click(guardarFirmaGerente);
            botonBuscarFirmasGerente.click(cargarFirmasGerente);
            //#endregion
        })();

        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function getFacultamientoUsuario() {
            axios.post('GetFacultamientoUsuario').then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    _facultamientoUsuario = response.data.data;

                    botonNuevaPlantilla.css('display', _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO ? 'inline-block' : 'none');
                    botonNuevoRequerimientoPlantilla.css('display', _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO ? 'inline-block' : 'none');
                    botonNuevoEvaluador.css('display', _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO || _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR ? 'inline-block' : 'none');
                    botonNuevoFacultamiento.css('display', _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO || _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR ? 'inline-block' : 'none');
                    botonNuevaFirma.css('display', _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO ? 'inline-block' : 'none');
                    botonNuevaFirmaGerente.css('display', _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO ? 'inline-block' : 'none');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        //#region Plantilla
        $('.checkTipoPlantilla').on('click', function () { $('.checkTipoPlantilla').not(`#${$(this).attr('id')}`).prop('checked', false); });
        $('.checkElementoCritico').on('click', function () { $('.checkElementoCritico').not(`#${$(this).attr('id')}`).prop('checked', false); });
        $('.checkTipoRequerimiento').on('click', function () { $('.checkTipoRequerimiento').not(`#${$(this).attr('id')}`).prop('checked', false); });

        selectElementoEvaluarPlantilla.on('change', function () {
            let elemento_id = +selectElementoEvaluarPlantilla.val();
            selectRequerimientoPlantilla.fillCombo('FillComboRequerimientos', { elemento_id: +selectElementoEvaluarPlantilla.val() }, false, null);
            selectRequerimientoPlantilla.select2().change();

            if (elemento_id > 0) {
                inputMensajeElementoPlantilla.val($(this).find('option:selected').attr('data-prefijo'));

                axios.post('GetRequerimientosElemento', { elemento_id }).then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        if (response.data.data.length > 0) {
                            $(`.checkElementoCritico[valor="${+response.data.data[0].critico}"]`).prop('checked', true);
                            $(`.checkElementoCritico[valor="${+!response.data.data[0].critico}"]`).prop('checked', false);
                            inputPonderacionElementoPlantilla.val(response.data.data[0].ponderacion);

                            // let datosAgregar = [];
                            // let datosTabla = dtElementosPlantilla.rows().data().toArray();

                            // response.data.data.forEach(x => {
                            //     if (!datosTabla.some(function (y) { return x.requerimiento_id == y.requerimiento_id && x.elemento_id == y.elemento_id })) {
                            //         datosAgregar.push(x);
                            //     }
                            // });

                            // dtElementosPlantilla.rows.add(datosAgregar).draw(false);
                        }
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                inputMensajeElementoPlantilla.val('');
                // dtElementosPlantilla.clear().draw();
            }
        });

        function initTablaPlantillasCreadas() {
            dtPlantillasCreadas = tablaPlantillasCreadas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombre', title: 'Plantilla' },
                    {
                        data: 'contratos', title: 'Contratos',
                        render: (data, type, row, meta) => {
                            let html = ``;

                            if (data != '') {
                                data.split(',').forEach(element => {
                                    html += `<span class='btn btn-info btn-xs displayCC' style="margin-right: 5px;"><i class='fab fa-creative-commons-nd'>&nbsp;${element}</i></span>`;
                                });
                            }

                            return html;
                        }
                    },
                    { data: 'tipoDesc', title: 'Tipo' },
                    {
                        title: 'Editar', render: function (data, type, row, meta) {
                            return `
                                ${row.plantillaBase && _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO ? `<button class="btn btn-primary btn-xs CopiarPlantilla" title="Copiar plantilla." data-id="${row.id}"><i class="fas fa-copy"></i></button>` : ``}
                                <button class="btn botonNaranja btn-xs VisualizarPlantilla" title="Visualizar plantilla." data-id="${row.id}"><i class="fas fa-eye"></i></button>
                                ${!row.plantillaBase && _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO ? `<button class="btn btn-warning btn-xs EditarPlantilla" title="Actualizar plantilla." data-id="${row.id}"><i class="fas fa-pencil-alt"></i></button>` : ``}
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO ? `<button class="btn btn-danger btn-xs EliminarPlantilla" title="Eliminar plantilla." data-id="${row.id}"><i class="fa fa-trash"></i></button>` : ``}
                            `;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaPlantillasCreadas.on('click', '.CopiarPlantilla', function () {
                        let rowData = dtPlantillasCreadas.row($(this).closest("tr")).data();
                        Alert2AccionConfirmar('Atención', `¿Desea crear una copia de la plantilla "${rowData.nombre}"?`, 'Confirmar', 'Cancelar', () => {
                            copiarPlantillaBase(rowData.id);
                        });
                    });

                    tablaPlantillasCreadas.on('click', '.VisualizarPlantilla', function () {
                        let rowData = dtPlantillasCreadas.row($(this).closest("tr")).data();

                        cargarPlantilla(rowData.id, false);
                    });

                    tablaPlantillasCreadas.on('click', '.EditarPlantilla', function () {
                        let rowData = dtPlantillasCreadas.row($(this).closest("tr")).data();

                        botonNuevaPlantilla.data().estatus = ESTATUS.EDITAR;
                        botonNuevaPlantilla.data().id = rowData.id;

                        cargarPlantilla(rowData.id, true);
                    });

                    tablaPlantillasCreadas.on('click', '.EliminarPlantilla', function () {
                        let rowData = dtPlantillasCreadas.row($(this).closest("tr")).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar la plantilla?', 'Confirmar', 'Cancelar', () => {
                            eliminarPlantilla(rowData.id);
                        });
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function initTablaElementosPlantilla() {
            dtElementosPlantilla = tablaElementosPlantilla.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'elementoDesc', title: 'Elemento' },
                    { data: 'mensaje', title: 'Mensaje del elemento' },
                    { data: 'criticoDesc', title: 'Crítico' },
                    { data: 'requerimientoDesc', title: 'Descripción del requerimiento' },
                    { data: 'tipoRequerimientoDesc', title: 'Único o recurrente' },
                    {
                        title: 'Quitar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-danger botonQuitarElemento"><i class="fa fa-times"></i></button>`
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaElementosPlantilla.on('click', '.botonQuitarElemento', function () {
                        dtElementosPlantilla.row($(this).closest("tr")).remove().draw();

                        if (tablaElementosPlantilla.find('tbody tr').length == 0) {
                            tblPartidas.DataTable().draw();
                        }
                    });
                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function agregarElementoPlantilla() {
            let datos = dtElementosPlantilla.rows().data();
            let requerimiento_id = +selectRequerimientoPlantilla.val();

            if (requerimiento_id > 0) {
                datos.push({
                    relPE_id: 0,
                    relPER_id: 0,
                    requerimiento_id: requerimiento_id,
                    elemento_id: selectElementoEvaluarPlantilla.val(),
                    elementoDesc: selectElementoEvaluarPlantilla.find('option:selected').text(),
                    mensaje: inputMensajeElementoPlantilla.val(),
                    critico: +$('.checkElementoCritico:checked').attr('valor'),
                    criticoDesc: $('.checkElementoCritico:checked').next().next().text(),
                    ponderacion: inputPonderacionElementoPlantilla.val(),
                    requerimientoDesc: selectRequerimientoPlantilla.find('option:selected').text(),
                    tipoRequerimiento: +$('.checkTipoRequerimiento:checked').attr('valor'),
                    tipoRequerimientoDesc: $('.checkTipoRequerimiento:checked').next().next().text()
                });

                dtElementosPlantilla.clear();
                dtElementosPlantilla.rows.add(datos).draw();

                limpiarSeccionRequerimiento();
            } else {
                Alert2Warning('Debe seleccionar un requerimiento.');
            }
        }

        function copiarPlantillaBase(plantilla_id) {
            axios.post('CopiarPlantillaBase', { plantilla_id }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información');
                    selectPlantilla.fillCombo('FillComboPlantillas', null, false, null);
                    botonBuscarPlantillasCreadas.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarPlantilla(plantilla_id, mostrar) {
            axios.post('GetPlantilla', { plantilla_id }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    limpiarModalPlantilla();
                    llenarModalPlantilla(response.data.data);

                    if (mostrar) {
                        mostrarBotonesModal();
                    } else {
                        ocultarBotonesModal();
                    }
                    modalPlantilla.modal('show');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarPlantilla(plantilla_id) {
            axios.post('EliminarPlantilla', { plantilla_id }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha eliminado la información');
                    selectPlantilla.fillCombo('FillComboPlantillas', null, false, null);
                    botonBuscarPlantillasCreadas.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function limpiarSeccionElemento() {
            selectElementoEvaluarPlantilla.val('');
            selectElementoEvaluarPlantilla.select2().change();
            inputMensajeElementoPlantilla.val('');
            $('.checkElementoCritico').prop('checked', false);
            inputPonderacionElementoPlantilla.val('');
            selectRequerimientoPlantilla.val('');
            selectRequerimientoPlantilla.select2().change();
            $('.checkTipoRequerimiento').prop('checked', false);
        }

        function limpiarSeccionRequerimiento() {
            selectRequerimientoPlantilla.val('');
            selectRequerimientoPlantilla.select2().change();
            $('.checkTipoRequerimiento').prop('checked', false);
        }

        function cargarPlantillasCreadas() {
            axios.post('GetPlantillasCreadas', { plantilla_id: +selectPlantilla.val(), contrato_id: +selectContratos.val() })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaPlantillasCreadas, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function llenarModalPlantilla(data) {
            inputNombrePlantilla.val(data.nombre);
            $(`.checkTipoPlantilla[valor="${data.tipo}"]`).prop('checked', true);
            selectContratosPlantilla.val(data.contratos);
            selectContratosPlantilla.select2().change();
            inputColaboradorPlantilla.data().usuario_id = data.colaborador_id;
            inputColaboradorPlantilla.val(data.colaboradorNombre);
            inputFechaPlantilla.val(data.fechaString);
            checkBase.prop('checked', data.plantillaBase);

            AddRows(tablaElementosPlantilla, data.requerimientos);
        }

        function limpiarModalPlantilla() {
            inputNombrePlantilla.val('');
            $('.checkTipoPlantilla').prop('checked', false);
            selectContratosPlantilla.val('');
            selectContratosPlantilla.select2().change();
            inputColaboradorPlantilla.val('');
            inputColaboradorPlantilla.data().usuario_id = null;
            inputFechaPlantilla.val('');
            checkBase.prop('checked', false);

            limpiarSeccionElemento();
            dtElementosPlantilla.clear().draw();
        }

        function setDatosColaborador(e, ui) {
            inputColaboradorPlantilla.data().usuario_id = ui.item.id;
            inputColaboradorPlantilla.val(ui.item.nombre);
        }

        function verificarColaborador(e, ui) {
            if (ui.item == null) {
                inputColaboradorPlantilla.val('');
                inputColaboradorPlantilla.data().usuario_id = null;
            }
        }

        function guardarNuevoElemento() {
            let elemento = {
                clave: inputNuevoElementoClave.val(),
                descripcion: inputNuevoElementoNombre.val(),
                mensaje: inputNuevoElementoMensaje.val()
            };

            axios.post('GuardarNuevoElemento', { elemento })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        modalNuevoElemento.modal('hide');
                        selectElementoEvaluarPlantilla.fillCombo('FillComboElementos', null, false, null);
                        selectElemento.fillCombo('FillComboElementos', null, false, null);
                        selectElementoNuevoEvaluador.fillCombo('FillComboElementos', null, false, null);
                        selectElementoNuevoEvaluador.find('option[value=""]').remove();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarNuevoRequerimiento() {
            let requerimiento = {
                descripcion: inputNuevoRequerimientoDescripcion.val(),
                elemento_id: botonGuardarNuevoRequerimiento.data().elemento_id
            };

            axios.post('GuardarNuevoRequerimiento', { requerimiento })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        modalNuevoRequerimiento.modal('hide');
                        selectRequerimientoPlantilla.fillCombo('FillComboRequerimientos', { elemento_id: +selectElementoEvaluarPlantilla.val() }, false, null);
                        selectRequerimientoPlantilla.select2().change();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarPlantilla() {
            let estatus = botonNuevaPlantilla.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    guardarNuevaPlantilla();
                    break;
                case ESTATUS.EDITAR:
                    editarPlantilla();
                    break;
            }
        }

        function guardarNuevaPlantilla() {
            let plantilla = {
                nombre: inputNombrePlantilla.val(),
                colaborador_id: inputColaboradorPlantilla.data().usuario_id,
                fecha: inputFechaPlantilla.val(),
                tipo: +$('.checkTipoPlantilla:checked').attr('valor'),
                plantillaBase: checkBase.prop('checked')
            };

            let contratos = selectContratosPlantilla.val();
            let requerimientos = [];

            tablaElementosPlantilla.find('tbody tr').each(function (index, row) {
                let rowData = dtElementosPlantilla.row(row).data();

                requerimientos.push({
                    relPE_id: 0,
                    relPER_id: 0,
                    requerimiento_id: rowData.requerimiento_id,
                    elemento_id: rowData.elemento_id,
                    elementoDesc: rowData.elementoDesc,
                    mensaje: rowData.mensaje,
                    critico: rowData.critico,
                    criticoDesc: rowData.criticoDesc,
                    ponderacion: rowData.ponderacion,
                    requerimientoDesc: rowData.requerimientoDesc,
                    tipoRequerimiento: rowData.tipoRequerimiento,
                    tipoRequerimientoDesc: rowData.tipoRequerimientoDesc
                });
            });

            axios.post('GuardarNuevaPlantilla', { plantilla, contratos, requerimientos }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información');
                    modalPlantilla.modal('hide');
                    selectPlantilla.fillCombo('FillComboPlantillas', null, false, null);
                    botonBuscarPlantillasCreadas.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarPlantilla() {
            let plantilla = {
                id: botonNuevaPlantilla.data().id,
                nombre: inputNombrePlantilla.val(),
                colaborador_id: inputColaboradorPlantilla.data().usuario_id,
                fecha: inputFechaPlantilla.val(),
                tipo: +$('.checkTipoPlantilla:checked').attr('valor'),
                plantillaBase: checkBase.prop('checked')
            };

            let contratos = selectContratosPlantilla.val();
            let requerimientos = [];

            tablaElementosPlantilla.find('tbody tr').each(function (index, row) {
                let rowData = dtElementosPlantilla.row(row).data();

                requerimientos.push({
                    relPE_id: rowData.relPE_id,
                    relPER_id: rowData.relPER_id,
                    requerimiento_id: rowData.requerimiento_id,
                    elemento_id: rowData.elemento_id,
                    elementoDesc: rowData.elementoDesc,
                    mensaje: rowData.mensaje,
                    critico: rowData.critico,
                    criticoDesc: rowData.criticoDesc,
                    ponderacion: rowData.ponderacion,
                    requerimientoDesc: rowData.requerimientoDesc,
                    tipoRequerimiento: rowData.tipoRequerimiento,
                    tipoRequerimientoDesc: rowData.tipoRequerimientoDesc
                });
            });

            axios.post('EditarPlantilla', { plantilla, contratos, requerimientos }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información');
                    modalPlantilla.modal('hide');
                    selectPlantilla.fillCombo('FillComboPlantillas', null, false, null);
                    botonBuscarPlantillasCreadas.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function mostrarBotonesModal() {
            botonNuevoElementoPlantilla.css('display', 'inline-block');
            botonAgregarElemento.css('display', 'inline-block');
            botonGuardarPlantilla.css('display', 'inline-block');
            $('.botonQuitarElemento').attr('disabled', false);
            dtElementosPlantilla.column(5).visible(true);
        }

        function ocultarBotonesModal() {
            botonNuevoElementoPlantilla.css('display', 'none');
            botonAgregarElemento.css('display', 'none');
            botonGuardarPlantilla.css('display', 'none');
            dtElementosPlantilla.column(5).visible(false);
        }
        //#endregion

        //#region Evaluadores
        function initTablaEvaluadores() {
            dtEvaluadores = tablaEvaluadores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'evaluadorNombre', title: 'Evaluador' },
                    {
                        data: 'proyectos', title: 'CC', render: function (data, type, row, meta) {
                            let html = ``;

                            if (data != '') {
                                data.split(',').forEach(element => {
                                    html += `<span class='btn btn-info btn-xs displayCC' style="margin-right: 5px;"><i class='fab fa-creative-commons-nd'>&nbsp;${element}</i></span>`;
                                });
                            }

                            return html;
                        }
                    },
                    {
                        data: 'elementos', title: 'Elementos', render: function (data, type, row, meta) {
                            let html = ``;

                            if (data != '') {
                                data.split(',').forEach(element => {
                                    html += `<span class='btn btn-info btn-xs displayCC' style="margin-right: 5px;"><i class='fab fa-creative-commons-nd'>&nbsp;${element}</i></span>`;
                                });
                            }

                            return html;
                        }
                    },
                    {
                        title: 'Editar', render: function (data, type, row, meta) {
                            return `
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO || _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR ? '<button class="btn btn-xs btn-warning botonEditarEvaluador"><i class="fa fa-edit"></i></button>' : ''}
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO || _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR ? '<button class="btn btn-xs btn-danger botonEliminarEvaluador"><i class="fa fa-times"></i></button>' : ''}
                            `;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaEvaluadores.on('click', '.botonEditarEvaluador', function () {
                        let rowData = dtEvaluadores.row($(this).closest("tr")).data();

                        botonNuevoEvaluador.data().estatus = ESTATUS.EDITAR;
                        botonNuevoEvaluador.data().evaluador_id = rowData.evaluador_id;

                        cargarEvaluador(rowData.evaluador_id);
                    });

                    tablaEvaluadores.on('click', '.botonEliminarEvaluador', function () {
                        let rowData = dtEvaluadores.row($(this).closest("tr")).data();

                        Alert2AccionConfirmar('Atención', `¿Desea eliminar el registro del evaluador "${rowData.evaluadorNombre}"?`, 'Confirmar', 'Cancelar', () => {
                            eliminarEvaluador(rowData.evaluador_id);
                        });
                    });
                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function setDatosEvaluador(e, ui) {
            inputEvaluador.data().usuario_id = ui.item.id;
            inputEvaluador.val(ui.item.nombre);
        }

        function verificarEvaluador(e, ui) {
            if (ui.item == null) {
                inputEvaluador.val('');
                inputEvaluador.data().usuario_id = null;
            }
        }

        function guardarEvaluador() {
            let estatus = botonNuevoEvaluador.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    guardarNuevoEvaluador();
                    break;
                case ESTATUS.EDITAR:
                    editarEvaluador();
                    break;
            }
        }

        function guardarNuevoEvaluador() {
            let evaluador = {
                usuario_id: inputEvaluador.data().usuario_id
            };

            let proyectos = [];
            selectProyectoNuevoEvaluador.val().forEach(element => { proyectos.push({ cc: element }) });
            let elementos = [];
            selectElementoNuevoEvaluador.val().forEach(element => { elementos.push({ elemento_id: +element }) });

            axios.post('GuardarNuevoEvaluador', { evaluador, proyectos, elementos }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información');
                    modalNuevoEvaluador.modal('hide');
                    botonBuscarEvaluadores.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarEvaluador() {
            let evaluador = {
                id: botonNuevoEvaluador.data().evaluador_id,
                usuario_id: inputEvaluador.data().usuario_id
            };

            let proyectos = [];
            selectProyectoNuevoEvaluador.val().forEach(element => { proyectos.push({ cc: element }) });
            let elementos = [];
            selectElementoNuevoEvaluador.val().forEach(element => { elementos.push({ elemento_id: +element }) });

            axios.post('EditarEvaluador', { evaluador, proyectos, elementos }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información');
                    modalNuevoEvaluador.modal('hide');
                    botonBuscarEvaluadores.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarEvaluadores() {
            axios.post('GetEvaluadores', { cc: selectProyectoEvaluadores.val(), elemento: +selectElemento.val() })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaEvaluadores, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarEvaluador(evaluador_id) {
            axios.post('GetEvaluador', { evaluador_id }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    inputEvaluador.val(response.data.data.evaluadorNombre);
                    inputEvaluador.data().usuario_id = response.data.data.usuario_id;
                    selectProyectoNuevoEvaluador.val(response.data.data.proyectos);
                    selectProyectoNuevoEvaluador.select2().change();
                    selectElementoNuevoEvaluador.val(response.data.data.elementos);
                    selectElementoNuevoEvaluador.select2().change();

                    modalNuevoEvaluador.modal('show');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarEvaluador(evaluador_id) {
            axios.post('EliminarEvaluador', { evaluador_id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado la información');
                        botonBuscarEvaluadores.click();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        //#endregion

        //#region Facultamiento
        function initTablaFacultamientos() {
            dtFacultamientos = tablaFacultamientos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'tipoDesc', title: 'Tipo de Usuario' },
                    { data: 'facultamientoNombre', title: 'Nombre Completo' },
                    {
                        data: 'proyectos', title: 'CC',
                        render: function (data, type, row, meta) {
                            html = ``;
                            if (row.mostrarModalCC) {
                                html = `<button class="btn btn-primary listadoCC"><i class="fas fa-list"></i></button>`;
                            } else if (data != '') {
                                data.split(',').forEach(element => {
                                    html += `<span class='btn btn-info btn-xs displayCC' style="margin-right: 5px;"><i class='fab fa-creative-commons-nd'>&nbsp;${element}</i></span>`;
                                });
                            }
                            return html;
                        }
                    },
                    {
                        title: 'Editar', render: function (data, type, row, meta) {
                            return `
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO || _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR ? '<button class="btn btn-xs btn-warning botonEditarFacultamiento"><i class="fa fa-edit"></i></button>' : ''}
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO || _facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR ? '<button class="btn btn-xs btn-danger botonEliminarFacultamiento"><i class="fa fa-times"></i></button>' : ''}
                            `;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaFacultamientos.on('click', '.botonEditarFacultamiento', function () {
                        let rowData = dtFacultamientos.row($(this).closest("tr")).data();

                        botonNuevoFacultamiento.data().estatus = ESTATUS.EDITAR;
                        botonNuevoFacultamiento.data().facultamiento_id = rowData.facultamiento_id;

                        cargarFacultamiento(rowData.facultamiento_id);
                    });

                    tablaFacultamientos.on('click', '.botonEliminarFacultamiento', function () {
                        let rowData = dtFacultamientos.row($(this).closest("tr")).data();

                        Alert2AccionConfirmar('Atención', `¿Desea eliminar el registro del usuario "${rowData.facultamientoNombre}"?`, 'Confirmar', 'Cancelar', () => {
                            eliminarFacultamiento(rowData.facultamiento_id);
                        });
                    });

                    tablaFacultamientos.on("click", ".listadoCC", function () {
                        let rowData = dtFacultamientos.row($(this).closest("tr")).data();
                        fncGetListadoCCRelUsuarioFacultamientos(rowData.facultamiento_id);
                    })
                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function initTblListadoCCRelUsuario() {
            dtListadoCCRelUsuario = tblListadoCCRelUsuario.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'cc', title: 'CC' }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetListadoCCRelUsuarioFacultamientos(facultamiento_id) {
            if (facultamiento_id > 0) {
                let obj = {};
                obj.facultamiento_id = facultamiento_id;
                axios.post('GetListadoCCRelUsuarioFacultamientos', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtListadoCCRelUsuario.clear();
                        dtListadoCCRelUsuario.rows.add(response.data.lstUsuarioRelCC);
                        dtListadoCCRelUsuario.draw();
                        mdlListadoCCRelUsuario.modal("show");
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener el listado de CC.");
            }
        }

        function setDatosColaboradorFacultamiento(e, ui) {
            inputColaboradorFacultamiento.data().usuario_id = ui.item.id;
            inputColaboradorFacultamiento.val(ui.item.nombre);
        }

        function verificarColaboradorFacultamiento(e, ui) {
            if (ui.item == null) {
                inputColaboradorFacultamiento.val('');
                inputColaboradorFacultamiento.data().usuario_id = null;
            }
        }

        function guardarFacultamiento() {
            let estatus = botonNuevoFacultamiento.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    guardarNuevoFacultamiento();
                    break;
                case ESTATUS.EDITAR:
                    editarFacultamiento();
                    break;
            }
        }

        function guardarNuevoFacultamiento() {
            let facultamiento = {
                usuario_id: inputColaboradorFacultamiento.data().usuario_id,
                tipo: +selectPrivilegioNuevoFacultamiento.val()
            };

            let proyectos = [];
            selectProyectoNuevoFacultamiento.val().forEach(element => { proyectos.push({ cc: element }) });

            axios.post('GuardarNuevoFacultamiento', { facultamiento, proyectos }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información');
                    modalNuevoFacultamiento.modal('hide');
                    botonBuscarFacultamientos.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarFacultamiento() {
            let facultamiento = {
                id: botonNuevoFacultamiento.data().facultamiento_id,
                usuario_id: inputColaboradorFacultamiento.data().usuario_id,
                tipo: +selectPrivilegioNuevoFacultamiento.val()
            };

            let proyectos = [];
            selectProyectoNuevoFacultamiento.val().forEach(element => { proyectos.push({ cc: element }) });

            axios.post('EditarFacultamiento', { facultamiento, proyectos }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información');
                    modalNuevoFacultamiento.modal('hide');
                    botonBuscarFacultamientos.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarFacultamientos() {
            axios.post('GetFacultamientos', { cc: selectProyectoFacultamientos.val(), tipo: +selectTipoFacultamiento.val() })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaFacultamientos, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarFacultamiento(facultamiento_id) {
            axios.post('GetFacultamiento', { facultamiento_id }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    inputColaboradorFacultamiento.val(response.data.data.facultamientoNombre);
                    inputColaboradorFacultamiento.data().usuario_id = response.data.data.usuario_id;
                    selectProyectoNuevoFacultamiento.val(response.data.data.proyectos);
                    selectProyectoNuevoFacultamiento.select2().change();
                    selectPrivilegioNuevoFacultamiento.val(response.data.data.tipo);

                    modalNuevoFacultamiento.modal('show');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarFacultamiento(facultamiento_id) {
            axios.post('EliminarFacultamiento', { facultamiento_id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado la información');
                        botonBuscarFacultamientos.click();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        //#endregion

        //#region Firma Subcontratista
        function initTablaFirmas() {
            dtFirmas = tablaFirmas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'subcontratistaDesc', title: 'Subcontratista' },
                    {
                        data: 'contratos', title: 'Contratos', render: function (data, type, row, meta) {
                            let html = ``;

                            if (data != '') {
                                data.split(',').forEach(element => {
                                    html += `<span class='btn btn-info btn-xs displayCC' style="margin-right: 5px;"><i class='fab fa-creative-commons-nd'>&nbsp;${element}</i></span>`;
                                });
                            }

                            return html;
                        }
                    },
                    { data: 'nombre', title: 'Firmante' },
                    { data: 'correo', title: 'Correo' },
                    {
                        title: 'Editar', render: function (data, type, row, meta) {
                            return `
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO ? '<button class="btn btn-xs btn-success botonEnviarCorreo"><i class="fa fa-paper-plane"></i></button>' : ''}
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO ? '<button class="btn btn-xs btn-warning botonEditarFirma"><i class="fa fa-edit"></i></button>' : ''}
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO ? '<button class="btn btn-xs btn-danger botonEliminarFirma"><i class="fa fa-times"></i></button>' : ''}
                            `;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaFirmas.on('click', '.botonEnviarCorreo', function () {
                        let rowData = dtFirmas.row($(this).closest("tr")).data();

                        Alert2AccionConfirmar('Atención', `¿Desea enviar el correo de notificación para el firmante "${rowData.nombre}"?`, 'Confirmar', 'Cancelar', () => {
                            enviarCorreoNotificacion(rowData.firma_id);
                        });
                    });

                    tablaFirmas.on('click', '.botonEditarFirma', function () {
                        let rowData = dtFirmas.row($(this).closest("tr")).data();

                        botonNuevaFirma.data().estatus = ESTATUS.EDITAR;
                        botonNuevaFirma.data().firma_id = rowData.firma_id;

                        cargarFirma(rowData.firma_id);
                    });

                    tablaFirmas.on('click', '.botonEliminarFirma', function () {
                        let rowData = dtFirmas.row($(this).closest("tr")).data();

                        Alert2AccionConfirmar('Atención', `¿Desea eliminar el registro del firmante "${rowData.nombre}"?`, 'Confirmar', 'Cancelar', () => {
                            eliminarFirma(rowData.firma_id);
                        });
                    });
                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function guardarFirma() {
            let estatus = botonNuevaFirma.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    guardarNuevaFirma();
                    break;
                case ESTATUS.EDITAR:
                    editarFirma();
                    break;
            }
        }

        function guardarNuevaFirma() {
            let firma = {
                subcontratista_id: +selectSubcontratistaNuevo.val(),
                nombre: inputNombreFirma.val(),
                correo: inputCorreoFirma.val()
            };

            let contratos = [];
            selectContratoFirma.val().forEach(element => { contratos.push({ contrato_id: element }) });

            axios.post('GuardarNuevaFirmaSubcontratista', { firma, contratos }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información');
                    modalNuevaFirma.modal('hide');
                    botonBuscarFirmas.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarFirma() {
            let firma = {
                id: botonNuevaFirma.data().firma_id,
                subcontratista_id: +selectSubcontratistaNuevo.val(),
                nombre: inputNombreFirma.val(),
                correo: inputCorreoFirma.val()
            };

            let contratos = [];
            selectContratoFirma.val().forEach(element => { contratos.push({ contrato_id: element }) });

            axios.post('EditarFirmaSubcontratista', { firma, contratos }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información');
                    modalNuevaFirma.modal('hide');
                    botonBuscarFirmas.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarFirmas() {
            axios.post('GetFirmaSubcontratistas', { subcontratista_id: +selectSubcontratista.val() })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaFirmas, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarFirma(firma_id) {
            axios.post('GetFirmaSubcontratista', { firma_id }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    selectSubcontratistaNuevo.val(response.data.data.subcontratista_id);
                    selectSubcontratistaNuevo.select2().change();
                    selectContratoFirma.val(response.data.data.contratos);
                    selectContratoFirma.select2().change();
                    inputNombreFirma.val(response.data.data.nombre);
                    inputCorreoFirma.val(response.data.data.correo);

                    modalNuevaFirma.modal('show');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarFirma(firma_id) {
            axios.post('EliminarFirmaSubcontratista', { firma_id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado la información');
                        botonBuscarFirmas.click();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function enviarCorreoNotificacion(firma_id) {
            axios.post('EnviarCorreoNotificacionFirma', { firma_id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha enviado el correo.');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        //#endregion

        //#region Especialidad
        function initTablaSubcontratistasEspecialidades() {
            dtSubcontratistasEspecialidades = tablaSubcontratistasEspecialidades.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                scrollY: '500px',
                // scrollCollapse: true,
                columns: [
                    { data: 'nombre', title: 'Subcontratista' },
                    {
                        title: 'Editar', render: function (data, type, row, meta) {
                            return `<select id="selectSubcontratista_${row.id}" class="form-control selectEspecialidad" multiple></select>`;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaSubcontratistasEspecialidades.on('change', '.selectEspecialidad', function () {
                        if (_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO) {
                            let row = $(this).closest("tr");
                            let rowData = dtSubcontratistasEspecialidades.row(row).data();

                            let especialidades = [];

                            $(row).find('.selectEspecialidad').val().forEach(element => {
                                especialidades.push({
                                    especialidad_id: +element, subcontratista_id: rowData.id
                                })
                            });

                            axios.post('GuardarEspecialidadesSubcontratista', { subcontratista_id: rowData.id, especialidades }).then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {

                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));
                        }
                    });
                },
                drawCallback: function () {
                    axios.post('FillComboEspecialidades').then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            tablaSubcontratistasEspecialidades.find('tbody tr').each(function (idx, row) {
                                let rowData = dtSubcontratistasEspecialidades.row(row).data();

                                if (rowData != undefined) {
                                    let select = $(row).find('.selectEspecialidad');

                                    $(select).fillCombo({ items: response.data.items }, null, false, 'Todos');

                                    if (rowData.especialidades != null && rowData.especialidades.length > 0) {
                                        $(select).val(rowData.especialidades);
                                    }

                                    convertToMultiselect('#selectSubcontratista_' + rowData.id);
                                }
                            });
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function cargarSubcontratistasEspecialidades() {
            axios.post('GetSubcontratistasEspecialidad', { cc: selectProyectoEspecialidad.val() }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    AddRows(tablaSubcontratistasEspecialidades, response.data.data);
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        //#endregion

        //#region Firma Gerente
        function initTablaFirmasGerente() {
            dtFirmasGerente = tablaFirmasGerente.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombre', title: 'Gerente' },
                    { data: 'ccDesc', title: 'Centro de Costo' },
                    {
                        title: 'Editar', render: function (data, type, row, meta) {
                            return `
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO ? '<button class="btn btn-xs btn-warning botonEditarFirma"><i class="fa fa-edit"></i></button>' : ''}
                                ${_facultamientoUsuario == FACULTAMIENTOS.ADMINISTRADOR_PMO ? '<button class="btn btn-xs btn-danger botonEliminarFirma"><i class="fa fa-times"></i></button>' : ''}
                            `;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaFirmasGerente.on('click', '.botonEditarFirma', function () {
                        let rowData = dtFirmasGerente.row($(this).closest("tr")).data();

                        botonNuevaFirmaGerente.data().estatus = ESTATUS.EDITAR;
                        botonNuevaFirmaGerente.data().firma_id = rowData.firma_id;

                        cargarFirmaGerente(rowData.firma_id);
                    });

                    tablaFirmasGerente.on('click', '.botonEliminarFirma', function () {
                        let rowData = dtFirmasGerente.row($(this).closest("tr")).data();

                        Alert2AccionConfirmar('Atención', `¿Desea eliminar el registro del firmante "${rowData.nombre}"?`, 'Confirmar', 'Cancelar', () => {
                            eliminarFirmaGerente(rowData.firma_id);
                        });
                    });
                },
                columnDefs: [
                    { className: 'dt-center dt-vertical-align-middle', 'targets': '_all' },
                    { className: 'dt-body-center', "targets": "_all" }
                ],
            });
        }

        function guardarFirmaGerente() {
            let estatus = botonNuevaFirmaGerente.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    guardarNuevaFirmaGerente();
                    break;
                case ESTATUS.EDITAR:
                    editarFirmaGerente();
                    break;
            }
        }

        function guardarNuevaFirmaGerente() {
            let firma = {
                usuario_id: inputUsuarioGerente.data().usuario_id,
                cc: selectCentroCostoNuevaFirmaGerente.val()
            };

            axios.post('GuardarNuevaFirmaGerente', { firma }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información');
                    modalNuevaFirmaGerente.modal('hide');
                    botonBuscarFirmasGerente.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarFirmaGerente() {
            let firma = {
                id: botonNuevaFirmaGerente.data().firma_id,
                usuario_id: inputUsuarioGerente.data().usuario_id,
                cc: selectCentroCostoNuevaFirmaGerente.val()
            };

            axios.post('EditarFirmaGerente', { firma }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información');
                    modalNuevaFirmaGerente.modal('hide');
                    botonBuscarFirmasGerente.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarFirmasGerente() {
            axios.post('GetFirmaGerentes', { cc: selectCentroCosto.val() }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    AddRows(tablaFirmasGerente, response.data.data);
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarFirmaGerente(firma_id) {
            axios.post('GetFirmaGerente', { firma_id }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    selectCentroCostoNuevaFirmaGerente.val(response.data.data.cc);
                    selectCentroCostoNuevaFirmaGerente.select2().change();
                    inputUsuarioGerente.data().usuario_id = response.data.data.usuario_id;
                    inputUsuarioGerente.val(response.data.data.nombre);

                    modalNuevaFirmaGerente.modal('show');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarFirmaGerente(firma_id) {
            axios.post('EliminarFirmaGerente', { firma_id }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha eliminado la información');
                    botonBuscarFirmasGerente.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function setDatosGerente(e, ui) {
            inputUsuarioGerente.data().usuario_id = ui.item.id;
            inputUsuarioGerente.val(ui.item.nombre);
        }

        function verificarGerente(e, ui) {
            if (ui.item == null) {
                inputUsuarioGerente.val('');
                inputUsuarioGerente.data().usuario_id = null;
            }
        }
        //#endregion

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => EvaluacionSubcontratista.Catalogos = new Catalogos())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();