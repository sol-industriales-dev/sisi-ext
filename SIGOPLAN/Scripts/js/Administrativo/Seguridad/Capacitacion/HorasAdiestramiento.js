(() => {
    $.namespace('Capacitacion.HorasAdiestramiento');
    HorasAdiestramiento = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const inputMesInicial = $('#inputMesInicial');
        const inputMesFinal = $('#inputMesFinal');
        const selectEquipoFiltro = $('#selectEquipoFiltro');
        const selectEquipoFiltroGrafica = $('#selectEquipoFiltroGrafica');
        const selectActividadFiltro = $('#selectActividadFiltro');
        const botonBuscar = $('#botonBuscar');
        const btnNuevoColaborador = $('#btnNuevoColaborador');
        const botonNuevaCaptura = $('#botonNuevaCaptura');
        const modalNuevaCaptura = $('#modalNuevaCaptura');
        const selectCentroCostoNuevo = $('#selectCentroCostoNuevo');
        const selectEquipoNuevo = $('#selectEquipoNuevo');
        const selectEquipoAdiestramiento = $('#selectEquipoAdiestramiento');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaTermino = $('#inputFechaTermino');
        const inputClaveColaborador = $('#inputClaveColaborador');
        const selectInteresadoNuevo = $('#selectInteresadoNuevo');
        const inputNombreColaborador = $('#inputNombreColaborador');
        const inputClaveAdiestrador = $('#inputClaveAdiestrador');
        const inputNombreAdiestrador = $('#inputNombreAdiestrador');
        const inputNombreInstructor = $('#inputNombreInstructor');
        const inputNombreSeguridad = $('#inputNombreSeguridad');
        const inputNombreRecursosHumanos = $('#inputNombreRecursosHumanos');
        const inputNombreSobrestante = $('#inputNombreSobrestante');
        const inputNombreGerenteObra = $('#inputNombreGerenteObra');
        const botonGuardarNuevaCaptura = $('#botonGuardarNuevaCaptura');
        const modalHorasAdiestramiento = $('#modalHorasAdiestramiento');
        const modalHorasAdiestramientoActividad = $('#modalHorasAdiestramientoActividad');
        const tablaPendientes = $('#tablaPendientes');
        const tablaLiberados = $('#tablaLiberados');
        const tablaHorasAdiestramiento = $('#tablaHorasAdiestramiento');
        const tablaHorasAdiestramientoActividad = $('#tablaHorasAdiestramientoActividad');
        const botonGuardarHorasAdiestramiento = $('#botonGuardarHorasAdiestramiento');
        const botonGuardarHorasAdiestramientoActividad = $('#botonGuardarHorasAdiestramientoActividad');
        const botonAgregarRenglonHoras = $('#botonAgregarRenglonHoras');
        const botonAgregarRenglonHorasActividad = $('#botonAgregarRenglonHorasActividad');
        const labelHorasAcumuladas = $('#labelHorasAcumuladas');
        const labelHorasAcumuladasActividad = $('#labelHorasAcumuladasActividad');
        const labelHorasPendientes = $('#labelHorasPendientes');
        const labelHorasPendientesActividad = $('#labelHorasPendientesActividad');
        const botonGuardarLiberados = $('#botonGuardarLiberados');
        const btnRegresar = $('#btnRegresar');
        const report = $("#report");
        const botonExcelActividades = $('#botonExcelActividades');
        const botonExcelHoras = $('#botonExcelHoras');
        const selectTipo = $('#selectTipo');
        const selectActividad = $('#selectActividad');
        const inputPrivilegio = $('#inputPrivilegio');
        const botonLiberarAdministrador = $('#botonLiberarAdministrador');
        //#endregion

        let dtPendientes;
        let dtLiberados;
        let dtHorasAdiestramiento;
        let dtActividadAdiestramiento;
        let tblDetalleHorasHombre;
        let tblOTsPersonal;

        let imgGrafica;
        let imgGrafica2;

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioMes = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        //#endregion

        const ESTATUS = { NUEVO: 0, EDITAR: 1 };
        _privilegioUsuario = +inputPrivilegio.val();

        (function init() {
            // revisarPrivilegio();
            $('.select2').select2();

            selectCentroCostoNuevo.select2({ dropdownParent: $(modalNuevaCaptura) });
            selectEquipoNuevo.select2({ dropdownParent: $(modalNuevaCaptura) });
            selectEquipoAdiestramiento.select2({ dropdownParent: $(modalNuevaCaptura) });
            selectInteresadoNuevo.select2({ dropdownParent: $(modalNuevaCaptura) });

            initBotones();
            initCombo();
            initTablaPendientes();
            initTablaLiberados();
            initTablaHorasAdiestramiento();
            initTablaActividadAdiestramiento();

            initMonthPicker(inputMesInicial);
            initMonthPicker(inputMesFinal);
            inputFechaInicio.datepicker({ dateFormat, showAnim, beforeShow: function (input, inst) { inst.dpDiv.removeClass('month_year_datepicker'); } });
            inputFechaTermino.datepicker({ dateFormat, showAnim, beforeShow: function (input, inst) { inst.dpDiv.removeClass('month_year_datepicker'); } });

            inputNombreInstructor.getAutocompleteValid(setDatosInstructor, verificarInstructor, { porClave: false }, 'GetEmpleadoEnKontrolAutocomplete');
            inputNombreSeguridad.getAutocompleteValid(setDatosSeguridad, verificarSeguridad, { porClave: false }, 'GetEmpleadoEnKontrolAutocomplete');
            inputNombreRecursosHumanos.getAutocompleteValid(setDatosRecursosHumanos, verificarRecursosHumanos, { porClave: false }, 'GetEmpleadoEnKontrolAutocomplete');
            inputNombreSobrestante.getAutocompleteValid(setDatosSobrestante, verificarSobrestante, { porClave: false }, 'GetEmpleadoEnKontrolAutocomplete');
            inputNombreGerenteObra.getAutocompleteValid(setDatosGerenteObra, verificarGerenteObra, { porClave: false }, 'GetEmpleadoEnKontrolAutocomplete');
        })();

        selectTipo.on('change', function () {
            let tipo = +selectTipo.val();

            selectEquipoNuevo.val('');
            selectEquipoNuevo.select2().change();
            selectEquipoAdiestramiento.val('');
            selectEquipoAdiestramiento.select2().change();
            selectActividad.val('');

            switch (tipo) {
                case 1:
                    selectEquipoNuevo.attr('disabled', false);
                    selectEquipoAdiestramiento.attr('disabled', false);
                    selectActividad.attr('disabled', true);
                    break;
                case 2:
                    selectEquipoNuevo.attr('disabled', true);
                    selectEquipoAdiestramiento.attr('disabled', true);
                    selectActividad.attr('disabled', false);
                    break;
                default:
                    selectEquipoNuevo.attr('disabled', true);
                    selectEquipoAdiestramiento.attr('disabled', true);
                    selectActividad.attr('disabled', true);
                    break;
            }
        });

        inputClaveColaborador.on('change', function () {
            let claveEmpleado = +inputClaveColaborador.val();

            axios.get('GetEmpleadoPorClave', { params: { claveEmpleado } })
                .then(response => {
                    let { success, data, message } = response.data;

                    if (success) {
                        inputNombreColaborador.val(data.nombre);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        });

        inputClaveAdiestrador.on('change', function () {
            let claveEmpleado = +inputClaveAdiestrador.val();

            axios.get('GetEmpleadoPorClave', { params: { claveEmpleado } })
                .then(response => {
                    let { success, data, message } = response.data;

                    if (success) {
                        inputNombreAdiestrador.val(data.nombre);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        });

        botonAgregarRenglonHoras.on('click', function () {
            // let datos = dtHorasAdiestramiento.rows().data();

            // datos.push({
            //     fecha: '',
            //     horas: ''
            // });

            // dtHorasAdiestramiento.clear();
            // dtHorasAdiestramiento.rows.add(datos).draw();
            dtHorasAdiestramiento.row.add({ fecha: '', horas: '' }).draw(false);
        });

        botonAgregarRenglonHorasActividad.on('click', function () {
            dtActividadAdiestramiento.row.add({ fecha: '', actividadId: '' }).draw(false);
        });

        function initBotones() {
            botonNuevaCaptura.click(function () {
                limpiarModalCaptura();
                botonGuardarNuevaCaptura.data().estatus = ESTATUS.NUEVO;
                botonGuardarNuevaCaptura.data().id = 0;
                modalNuevaCaptura.modal('show');
            });
            botonBuscar.click(cargarColaboradoresCapacitacion);
            botonGuardarNuevaCaptura.click(guardarCaptura);
            botonGuardarHorasAdiestramiento.click(guardarHorasAdiestramiento);
            botonGuardarHorasAdiestramientoActividad.click(guardarActividadAdiestramiento);
            botonGuardarLiberados.click(guardarLiberados);
            botonLiberarAdministrador.click(guardarLiberacionAdministrador);
            btnRegresar.click(regresar);
            botonExcelActividades.click(descargarExcelActividades);
            botonExcelHoras.click(descargarExcelHoras);
        }

        function descargarExcelActividades() {
            location.href = `DescargarExcelAdiestramientoActividades`;
        }

        function descargarExcelHoras() {
            location.href = `DescargarExcelAdiestramientoHoras`;
        }

        function regresar() {
            $('#divOTModalDetalle').addClass('hide');
            $('#divPersonalModalDetalle').removeClass('hide');
        }

        function initCombo() {
            axios.get('ObtenerComboCCAmbasEmpresas').then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    selectCentroCosto.append('<option value="Todos">Todos</option>');
                    selectCentroCostoNuevo.append('<option value="">--Seleccione--</option>');

                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}">`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                        });

                        groupOption += `</optgroup>`;

                        selectCentroCosto.append(groupOption);
                        selectCentroCostoNuevo.append(groupOption);
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }

                convertToMultiselect('#selectCentroCosto');
            }).catch(error => AlertaGeneral(`Alerta`, error.message));

            selectEquipoNuevo.fillCombo('GetEquiposCombo', null, false, null, () => {
                selectEquipoNuevo.find('option').each(function (value, index, array) {
                    // $(this).clone().appendTo(selectEquipoFiltro);
                });
            });
            selectEquipoFiltro.fillCombo('GetEquipoAdiestramientoCombo', null, false, null);
            selectEquipoFiltroGrafica.fillCombo('GetEquipoAdiestramientoCombo', null, false, 'Todos');
            convertToMultiselect('#selectEquipoFiltroGrafica');
            selectEquipoAdiestramiento.fillCombo('GetEquipoAdiestramientoCombo', null, false, null, () => {
                // selectEquipoAdiestramiento.find('option').each(function (value, index, array) {
                //     $(this).clone().appendTo(selectEquipoFiltro);
                // });
            });
            selectInteresadoNuevo.fillComboBox('GetInteresados', null, '-- Seleccionar --', null);
            selectActividadFiltro.fillComboBox('GetActividades', null, null, null);

            axios.get('GetActividades').then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    selectActividad.append('<option value="">--Seleccione--</option>');

                    items.forEach(x => {
                        selectActividad.append(`<option value="${x.valor}">${x.texto}</option>`);
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarCaptura() {
            let estatus = botonGuardarNuevaCaptura.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    guardarNuevaCaptura();
                    break;
                case ESTATUS.EDITAR:
                    editarCaptura();
                    break;
            }
        }

        function initTablaPendientes() {
            dtPendientes = tablaPendientes.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                searching: false,
                paging: false,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaPendientes.on('click', '.botonHorasAdiestramiento', function () {
                        let rowData = dtPendientes.row($(this).closest('tr')).data();

                        cargarModalHorasAdiestramiento(rowData.id);

                        botonGuardarHorasAdiestramiento.data('colaboradorCapacitacionID', rowData.id);
                    });

                    tablaPendientes.on('click', '.botonHorasAdiestramientoActividad', function () {
                        let rowData = dtPendientes.row($(this).closest('tr')).data();

                        cargarModalHorasAdiestramientoActividad(rowData.id);

                        botonGuardarHorasAdiestramientoActividad.data('colaboradorCapacitacionID', rowData.id);
                    });

                    tablaPendientes.on('click', '.botonReporteColaborador', function () {
                        let rowData = tablaPendientes.DataTable().row($(this).closest('tr')).data();

                        imprimirColaborador(rowData.id);
                    });

                    tablaPendientes.on('click', '.botonMtto', function () {
                        let rowData = tablaPendientes.DataTable().row($(this).closest('tr')).data();

                        loadDataDetalleMtto(rowData.id);
                    });

                    tablaPendientes.on('click', '.botonEditarColaborador', function () {
                        let rowData = dtPendientes.row($(this).closest('tr')).data();

                        limpiarModalCaptura();
                        llenarModalCaptura(rowData);
                        botonGuardarNuevaCaptura.data().estatus = ESTATUS.EDITAR;
                        botonGuardarNuevaCaptura.data().id = rowData.id;
                        modalNuevaCaptura.modal('show');
                    });

                    tablaPendientes.on('click', '.botonEliminarColaborador', function () {
                        let rowData = dtPendientes.row($(this).closest('tr')).data();

                        Alert2AccionConfirmar('Atención', '¿Desea eliminar el registro del colaborador?', 'Confirmar', 'Cancelar', () => {
                            axios.post('EliminarColaboradorCapacitacion', { colaboradorCapacitacion: { id: rowData.id } }).then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    AlertaGeneral(`Alerta`, `Se ha eliminado la información.`);
                                    cargarColaboradoresCapacitacion();
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));
                        });
                    });
                },
                createdRow: function (row, rowData) {
                    if (rowData.estatus_empleado != 'A') {
                        $(row).addClass('renglonEmpleadoBaja');
                    }
                },
                columns: [
                    { data: 'colaborador', title: 'CLAVE' },
                    { data: 'colaboradorDesc', title: 'NOMBRE' },
                    {
                        title: 'EQUIPO/ACTIVIDAD', render: function (data, type, row, meta) {
                            if (row.tipo == 1) {
                                return row.equipoDesc;
                            } else if (row.tipo == 2) {
                                return row.actividadDesc;
                            }
                        }
                    },
                    {
                        title: 'ADIESTRAMIENTO/ACTIVIDAD', render: function (data, type, row, meta) {
                            if (row.tipo == 1) {
                                return row.equipoAdiestramientoDesc;
                            } else if (row.tipo == 2) {
                                return row.actividadDesc;
                            }
                        }
                    },
                    { data: 'fechaInicioString', title: 'INICIO' },
                    { data: 'fechaTerminoString', title: 'TERMINO' },
                    { data: 'adiestradorDesc', title: 'CAPACITADOR' },
                    {
                        title: 'HRS/ACTIVIDADES ACUMULADAS', render: function (data, type, row, meta) {
                            if (row.tipo == 1) {
                                return row.horas;
                            } else if (row.tipo == 2) {
                                return row.actividades;
                            }
                        }
                    },
                    {
                        title: 'HRS/ACTIVIDADES EN ADIESTRAMIENTO', render: function (data, type, row, meta) {
                            if (row.tipo == 2) {
                                return row.estatus_empleado == 'A' ? `<button class="btn btn-xs btn-warning botonHorasAdiestramientoActividad"><i class="fa fa-eye"></i></button>` : ``;
                            } else {
                                return row.estatus_empleado == 'A' ? `<button class="btn btn-xs btn-warning botonHorasAdiestramiento"><i class="fa fa-eye"></i></button>` : ``;
                            }
                        }
                    },
                    {
                        title: 'HRS MTTO', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-primary botonMtto"><i class="fa fa-file"></i></button>`;
                        }
                    },
                    {
                        title: 'CONTROL', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-primary botonReporteColaborador"><i class="fa fa-file"></i></button>`;
                        }
                    },
                    {
                        title: _privilegioUsuario == 1 ? 'Editar' : '', render: function (data, type, row, meta) {
                            return _privilegioUsuario == 1 ? `
                                <div>
                                    <button class="btn btn-xs btn-warning botonEditarColaborador"><i class="fa fa-pencil-alt"></i></button>
                                    <button class="btn btn-xs btn-danger botonEliminarColaborador"><i class="fa fa-times"></i></button>
                                </div>
                            ` : ``;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '50%', targets: [1, 6] }
                ]
            });

            if (_privilegioUsuario != 1) {
                dtPendientes.column(2).visible(false);
                dtPendientes.column(11).visible(false);
            }
        }

        function initTablaLiberados() {
            dtLiberados = tablaLiberados.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                searching: false,
                paging: false,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaLiberados.on('click', '.botonVerSoporteAdiestramiento', function () {
                        let rowData = dtLiberados.row($(this).closest('tr')).data();

                        mostrarArchivoSoporteAdiestramiento(rowData.id);
                    });

                    tablaLiberados.on('click', '.botonDescargarSoporteAdiestramiento', function () {
                        let rowData = dtLiberados.row($(this).closest('tr')).data();

                        descargarArchivoSoporteAdiestramiento(rowData.id);
                    });

                    tablaLiberados.on('click', '.botonDescargaGlobal', function () {
                        let rowData = dtLiberados.row($(this).closest('tr')).data();
                        descargarGlobal(rowData.id, rowData.colaborador);
                    });

                    tablaLiberados.on('change', 'input[type=file]', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtLiberados.row(row).data();
                        let inputSoporteAdiestramiento = $(row).find(`.inputSoporteAdiestramiento_${rowData.id}`);
                        let botonSoporteAdiestramiento = $(row).find(`#botonSoporteAdiestramiento_${rowData.id}`);
                        let iconoBoton = botonSoporteAdiestramiento.find('i');
                        let labelArchivo = $(row).find(`#labelArchivoSoporteAdiestramiento_${rowData.id}`);

                        if (inputSoporteAdiestramiento[0].files.length > 0) {
                            let textoLabel = inputSoporteAdiestramiento[0].files[0].name;

                            if (textoLabel.length > 35) {
                                textoLabel = textoLabel.substr(0, 31) + '...';
                            }

                            labelArchivo.text(textoLabel);
                            botonSoporteAdiestramiento.addClass('btn-success');
                            botonSoporteAdiestramiento.removeClass('btn-default');
                            iconoBoton.addClass('fa-check');
                            iconoBoton.removeClass('fa-upload');
                        } else {
                            labelArchivo.text('');
                            botonSoporteAdiestramiento.addClass('btn-default');
                            botonSoporteAdiestramiento.removeClass('btn-success');
                            iconoBoton.addClass('fa-upload');
                            iconoBoton.removeClass('fa-check');
                        }
                    });
                },
                createdRow: function (row, rowData) {
                    if (rowData.estatus_empleado != 'A') {
                        $(row).addClass('renglonEmpleadoBaja');
                    } else if (rowData.rutaSoporteAdiestramiento == null) {
                        $(row).addClass('renglonSoportePendiente');
                    }
                },
                columns: [
                    { data: 'colaborador', title: 'CLAVE' },
                    { data: 'colaboradorDesc', title: 'NOMBRE' },
                    { data: 'equipoAdiestramientoDesc', title: 'ADIESTRAMIENTO' },
                    { data: 'fechaInicioString', title: 'INICIO' },
                    { data: 'fechaTerminoString', title: 'TERMINO' },
                    { data: 'adiestradorDesc', title: 'CAPACITADOR' },
                    {
                        title: 'LIBERADO', render: function (data, type, row, meta) {
                            return row.puedeEvaluar ? `
                                <div>
                                    <input type="checkbox" id="checkBoxLiberado_${meta.row}" class="regular-checkbox" ${row.liberado ? `checked` : ``} ${_privilegioUsuario == 1 ? '' : (row.rutaSoporteAdiestramiento != null || _privilegioUsuario == 2 ? 'disabled' : '')}>
                                    <label for="checkBoxLiberado_${meta.row}"></label>
                                </div>
                            ` : ``;
                        }
                    },
                    {
                        title: 'SOPORTES ADIESTRAMIENTO', render: function (data, type, row, meta) {
                            return `
                                <div>
                                    ${row.rutaSoporteAdiestramiento != null ? '<button class="btn btn-xs btn-default botonVerSoporteAdiestramiento"><i class="fa fa-eye"></i></button>' : ''}
                                    ${row.rutaSoporteAdiestramiento != null ? '<button class="btn btn-xs btn-default botonDescargarSoporteAdiestramiento"><i class="fa fa-download"></i></button>' : ''}
                                    ${row.rutaSoporteAdiestramiento != null ? '<button class="btn btn-xs btn-default botonDescargaGlobal">Descarga global</button>' : ''}
                                    ${row.rutaSoporteAdiestramiento == null && _privilegioUsuario != 2 ? `
                                        <div class="text-center">
                                            <label id="botonSoporteAdiestramiento_${row.id}" for="inputSoporteAdiestramiento_${row.id}" class="btn btn-xs btn-default"><i class="fa fa-upload"></i></label>
                                            <label id="labelArchivoSoporteAdiestramiento_${row.id}" class="labelArchivo"></label>
                                            <input id="inputSoporteAdiestramiento_${row.id}" type="file" class="inputSoporteAdiestramiento_${row.id}" accept="application/pdf, image/*">
                                        </div>
                                    ` : ''}
                                </div>
                            `;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaHorasAdiestramiento() {
            dtHorasAdiestramiento = tablaHorasAdiestramiento.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                searching: false,
                paging: false,
                dom: 't',
                initComplete: function (settings, json) {
                    // tablaHorasAdiestramiento.on('change', 'input', function () {
                    //     let celdaSeleccionada = $(':focus');

                    //     tablaHorasAdiestramiento.find('tbody tr').each(function (index, row) {
                    //         let rowData = tablaHorasAdiestramiento.DataTable().row(row).data();
                    //         let fecha = $(row).find('.inputFechaAdiestramiento').val();
                    //         let horas = $(row).find('.inputHorasAdiestramiento').val();

                    //         rowData.fecha = fecha;
                    //         rowData.horas = horas;

                    //         tablaHorasAdiestramiento.DataTable().row(row).data(rowData).draw();
                    //     });

                    //     $(celdaSeleccionada).find('input').focus();
                    // });

                    tablaHorasAdiestramiento.on('click', '.botonQuitarRenglonHoras', function () {
                        dtHorasAdiestramiento.row($(this).closest('tr')).remove().draw();
                    });
                },
                createdRow: function (row, rowData) {
                    let inputFechaAdiestramiento = $(row).find('.inputFechaAdiestramiento');

                    inputFechaAdiestramiento.datepicker({
                        dateFormat,
                        maxDate: fechaActual,
                        showAnim,
                        beforeShow: function (input, inst) { inst.dpDiv.removeClass('month_year_datepicker'); }
                    });
                },
                columns: [
                    {
                        data: 'fecha', title: 'FECHA', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center inputFechaAdiestramiento">`;
                        }
                    },
                    {
                        data: 'horas', title: 'HORAS', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center inputHorasAdiestramiento">`;
                        }
                    },
                    {
                        render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-danger botonQuitarRenglonHoras"><i class="fa fa-times"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaActividadAdiestramiento() {
            dtActividadAdiestramiento = tablaHorasAdiestramientoActividad.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                searching: false,
                paging: false,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaHorasAdiestramientoActividad.on('click', '.botonQuitarRenglonHoras', function () {
                        dtActividadAdiestramiento.row($(this).closest('tr')).remove().draw();
                    });
                },
                createdRow: function (row, rowData) {
                    let inputFechaAdiestramiento = $(row).find('.inputFechaAdiestramiento');

                    inputFechaAdiestramiento.datepicker({
                        dateFormat,
                        maxDate: fechaActual,
                        showAnim,
                        beforeShow: function (input, inst) { inst.dpDiv.removeClass('month_year_datepicker'); }
                    });

                    // let cboAct = $(row).find('.selectActividadAdiestramiento');

                    // selectActividadFiltro.find('option').each(function (value, index, array) {
                    //     $(this).clone().appendTo(cboAct);
                    // });
                },
                columns: [
                    {
                        data: 'fecha', title: 'FECHA', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center inputFechaAdiestramiento">`;
                        }
                    },
                    {
                        data: 'cantidad', title: 'CANT. ACTIVIDADES', render: function (data, type, row, meta) {
                            return `<input type="number" placeholder="#" class="form-control text-center inputCantidadActividades">`;
                        }
                    },
                    {
                        render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-danger botonQuitarRenglonHoras"><i class="fa fa-times"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function mostrarArchivoSoporteAdiestramiento(id) {
            if (id > 0) {
                $.post('CargarDatosArchivoSoporteAdiestramiento', { id })
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

        function descargarArchivoSoporteAdiestramiento(id) {
            if (id > 0) {
                location.href = `DescargarArchivoSoporteAdiestramiento?id=${id}`;
            }
        }

        function descargarGlobal(id, clave_empleado) {
            let listaCC = getValoresMultiples('#selectCentroCosto');
            let mesInicial = inputMesInicial.val();
            let listaStringMesInicial = mesInicial.split('/');
            let fechaInicial = '01' + '/' + listaStringMesInicial[0] + '/' + listaStringMesInicial[1];
            let mesFinal = inputMesFinal.val();
            let listaStringMesFinal = mesFinal.split('/');
            let fechaFinal = '01' + '/' + listaStringMesFinal[0] + '/' + listaStringMesFinal[1];
            let equipos = selectEquipoFiltro.val();
            let actividades = selectActividadFiltro.val()

            axios.post('CargarHorasAdiestramientoColaborador', { listaCC, fechaInicial, fechaFinal, equipos, actividades, colaborador: clave_empleado })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        initGraficaAdiestramientoColaboradorActividades(response.data.adiestramientoColaboradorActividades);
                        // initGraficaPuestosDesarrollados(response.data.puestosDesarrollados);
                        initGraficaAdiestramientoColaboradorHoras(response.data.adiestramientoColaboradorHoras);

                        axios.get('GetMtto', { params: { colaboradorCapacitacionId: id } }).then(response => {
                            let { success, datos, message, dataset } = response.data;

                            if (success) {
                                SetDataInTablesDetalleMtto(dataset);
                                setGraficaMtto(dataset);
                                initGraficaMTTO(response.data.data);

                                setTimeout(function () {
                                    axios.get('GetMttoDetalle', { params: { clave_empleado: clave_empleado } }).then(response => {
                                        let { success, datos, message, dataResult } = response.data;

                                        if (success) {
                                            setTablaOT(dataResult);

                                            // fncGenerarImgChart($('#graficaAdiestramientoColaboradorActividades').highcharts(), 1);
                                            // fncGenerarImgChart($('#graficaAdiestramientoColaboradorActividades').highcharts(), 1);
                                            // fncGenerarImgChart($('#graficaAdiestramientoColaboradorActividades').highcharts(), 1);
                                            // fncGenerarImgChart($('#graficaMTTO').highcharts(), 2);

                                            axios.post('cargarImgTemp1', { gfx1: imgGrafica, gfx2: imgGrafica2 }).then(response => {
                                                let { success, items, message } = response.data;
                                                if (success) {
                                                    location.href = `descargarGlobal?id=${id}`;
                                                } else {
                                                    Alert2Error('Alerta');
                                                }
                                            }).catch(error => Alert2Error(error.message));
                                        } else {
                                            AlertaGeneral('Alerta', message);
                                        }
                                    }).catch(error => AlertaGeneral('Alerta', error.message));
                                }, 5000);
                            } else {
                                AlertaGeneral('Alerta', message);
                            }
                        }).catch(error => AlertaGeneral('Alerta', error.message));
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        EXPORT_WIDTH = 1000;
        function fncGenerarImgChart(chart, num) {
            var render_width = EXPORT_WIDTH;
            var render_height = render_width * chart.chartHeight / chart.chartWidth

            var svg = chart.getSVG({
                exporting: {
                    sourceWidth: chart.chartWidth,
                    sourceHeight: chart.chartHeight
                }
            });

            var canvas = document.createElement('canvas');
            canvas.height = render_height;
            canvas.width = render_width;

            var image = new Image;

            image.onload = function () {
                canvas.getContext('2d').drawImage(this, 0, 0, render_width, render_height);
                var data = canvas.toDataURL("image/png")
                // download(data, filename + '.png', num);

                document.body.appendChild(document.createElement('a'));

                if (num == 1) {
                    imgGrafica = data;
                } else {
                    imgGrafica2 = data;
                }
            };

            image.src = 'data:image/svg+xml;base64,' + window.btoa(unescape(encodeURIComponent(svg)));
        }

        function download(data, filename, num) {
            var a = document.createElement('a');
            // a.download = filename;
            // a.href = data
            document.body.appendChild(a);
            // a.click();
            // a.remove();
            if (num == 1) {
                imgGrafica = data;
            } else {
                imgGrafica2 = data;
            }
            // console.log(data);
        }

        function imprimirColaborador(colaboradorCapacitacionID) {
            if (colaboradorCapacitacionID == 0) {
                AlertaGeneral(`Alerta`, `No se ha seleccionado un colaborador.`);
                return;
            }

            $.blockUI({ message: 'Generando imprimible...' });
            var path = `/Reportes/Vista.aspx?idReporte=213&colaboradorCapacitacionID=${colaboradorCapacitacionID}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function cargarColaboradoresCapacitacion() {
            let listaCC = getValoresMultiples('#selectCentroCosto');
            let mesInicial = inputMesInicial.val();
            let listaStringMesInicial = mesInicial.split('/');
            let fechaInicial = '01' + '/' + listaStringMesInicial[0] + '/' + listaStringMesInicial[1];
            let mesFinal = inputMesFinal.val();
            let listaStringMesFinal = mesFinal.split('/');
            let fechaFinal = '01' + '/' + listaStringMesFinal[0] + '/' + listaStringMesFinal[1];
            let equipos = selectEquipoFiltro.val();
            let equiposGrafica = selectEquipoFiltroGrafica.val();
            let actividades = selectActividadFiltro.val()

            axios.post('CargarHorasAdiestramiento', { listaCC, fechaInicial, fechaFinal, equipos, equiposGrafica, actividades })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        $('#tituloCumplimientoGeneral').text(response.data.cumplimientoGeneral + '% cumplimiento');
                        AddRows(tablaPendientes, response.data.pendientes);
                        AddRows(tablaLiberados, response.data.liberados);
                        initGraficaColaboradoresAdiestramiento(response.data.dashboard);
                        initGraficaColaboradoresAdiestramientoLiberados(response.data.dashboardLiberados);
                        initGraficaAdiestramientoColaboradorActividades(response.data.adiestramientoColaboradorActividades);
                        initGraficaPuestosDesarrollados(response.data.puestosDesarrollados);
                        initGraficaAdiestramientoColaboradorHoras(response.data.adiestramientoColaboradorHoras);
                        initGraficaDesarrollosOperacionEquipos(response.data.desarrollosOperacionEquipos);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initGraficaColaboradoresAdiestramiento(datos) {
            Highcharts.chart('graficaColaboradoresAdiestramiento', {
                chart: { plotBackgroundColor: null, plotBorderWidth: null, pltoShadow: false, type: 'pie' },
                title: { text: 'COLABORADORES EN ADIESTRAMIENTO' },
                tooltip: {
                    pointFormat: '{series.nombre}: <b>{point.y}</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            formatter: function () {
                                return this.point.name + ': ' + this.point.y;
                            }
                            // enabled: false
                        },
                        showInLegend: true,
                        colors: ['rgb(255, 118, 58)', 'rgb(0, 176, 80)']
                    }
                },
                series: [{
                    name: '',
                    colorByPoint: true,
                    data: datos
                }],
                credits: {
                    enabled: false
                }
            });
        }

        function initGraficaColaboradoresAdiestramientoLiberados(datos) {
            Highcharts.chart('graficaColaboradoresAdiestramientoLiberados', {
                chart: { plotBackgroundColor: null, plotBorderWidth: null, pltoShadow: false, type: 'pie' },
                title: { text: 'LIBERADOS' },
                tooltip: {
                    pointFormat: '{series.nombre}: <b>{point.y}</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            formatter: function () {
                                return this.point.name + ': ' + this.point.y;
                            }
                            // enabled: false
                        },
                        showInLegend: true,
                        colors: ['green', 'red']
                    }
                },
                series: [{
                    name: '',
                    colorByPoint: true,
                    data: datos
                }],
                credits: {
                    enabled: false
                }
            });
        }

        function initGraficaAdiestramientoColaboradorActividades(datos) {
            Highcharts.chart('graficaAdiestramientoColaboradorActividades', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'Adiestramiento Colaborador Actividades' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 25,
                    title: { text: '' },
                    labels: { format: '{value}' }
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,
                        zones: [
                            { value: 11, color: 'rgb(192, 0, 0)' },
                            { value: 20, color: 'rgb(255, 192, 0)' },
                            { color: 'rgb(0, 176, 80)' }
                        ]
                    },
                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1 }
                ],
                credits: { enabled: false },
                legend: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");

            fncGenerarImgChart($('#graficaAdiestramientoColaboradorActividades').highcharts(), 1);
            fncGenerarImgChart($('#graficaAdiestramientoColaboradorActividades').highcharts(), 1);
            fncGenerarImgChart($('#graficaAdiestramientoColaboradorActividades').highcharts(), 1);
        }

        function initGraficaPuestosDesarrollados(datos) {
            Highcharts.chart('graficaPuestosDesarrollados', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'Puestos Desarrollados' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 25,
                    title: { text: '' },
                    labels: { format: '{value}' }
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,
                        // zones: [
                        //     { value: 11, color: 'rgb(192, 0, 0)' },
                        //     { value: 20, color: 'rgb(255, 192, 0)' },
                        //     { color: 'rgb(0, 176, 80)' }
                        // ]
                    },
                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1, color: 'orange' }
                ],
                credits: { enabled: false },
                legend: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");

            // fncGenerarImgChart($('#graficaPuestosDesarrollados').highcharts(), 1);
            // fncGenerarImgChart($('#graficaPuestosDesarrollados').highcharts(), 1);
            // fncGenerarImgChart($('#graficaPuestosDesarrollados').highcharts(), 1);
        }

        function initGraficaAdiestramientoColaboradorHoras(datos) {
            Highcharts.chart('graficaAdiestramientoColaboradorHoras', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'Adiestramiento Colaborador Horas' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 250,
                    title: { text: '' },
                    labels: { format: '{value}' }
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,
                        zones: [
                            { value: 84, color: 'rgb(192, 0, 0)' },
                            { value: 168, color: 'rgb(255, 192, 0)' },
                            { color: 'rgb(0, 176, 80)' }
                        ]
                    },
                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1 }
                ],
                credits: { enabled: false },
                legend: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function initGraficaDesarrollosOperacionEquipos(datos) {
            Highcharts.chart('graficaDesarrollosOperacionEquipos', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'Desarrollos en operación de equipos' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 250,
                    title: { text: '' },
                    labels: { format: '{value}' }
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,
                        // zones: [
                        //     { value: 84, color: 'rgb(192, 0, 0)' },
                        //     { value: 168, color: 'rgb(255, 192, 0)' },
                        //     { color: 'rgb(0, 176, 80)' }
                        // ]
                    },
                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1, color: 'orange' }
                ],
                credits: { enabled: false },
                legend: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function initGraficaMTTO(datos) {
            Highcharts.chart('graficaMTTO', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'MTTO' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
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
                    { name: datos.serie1Descripcion, data: datos.serie1, color: 'rgb(0, 0, 255)' },
                    { name: datos.serie2Descripcion, data: datos.serie2, color: 'rgb(255, 0, 0)' },
                    { name: datos.serie3Descripcion, data: datos.serie3, color: 'rgb(0, 128, 0)' }
                ],
                credits: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");

            fncGenerarImgChart($('#graficaMTTO').highcharts(), 2);
        }

        function guardarNuevaCaptura() {
            if (validarCamposNuevaCaptura()) {
                AlertaGeneral(`Alerta`, `Debe capturar todos los campos.`);
                return;
            }

            let colaboradorCapacitacion = {
                cc: selectCentroCostoNuevo.val(),
                area: 0, //Este valor se asigna en el back-end
                empresa: +selectCentroCostoNuevo.find('option:selected').attr('empresa'),
                fechaInicio: inputFechaInicio.val(),
                fechaTermino: inputFechaTermino.val(),
                colaborador: inputClaveColaborador.val(),
                adiestrador: inputClaveAdiestrador.val(),
                instructor: inputNombreInstructor.data().clave_empleado,
                seguridad: inputNombreSeguridad.data().clave_empleado,
                recursosHumanos: inputNombreRecursosHumanos.data().clave_empleado,
                sobrestante: inputNombreSobrestante.data().clave_empleado,
                gerenteObra: inputNombreGerenteObra.data().clave_empleado,
                tipo: +selectTipo.val(),
                equipo: +selectEquipoNuevo.val(),
                equipoAdiestramiento_id: +selectEquipoAdiestramiento.val(),
                actividad_id: +selectActividad.val(),
            };

            axios.post('GuardarNuevoColaboradorCapacitacion', { colaboradorCapacitacion, interesados: selectInteresadoNuevo.val() })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        modalNuevaCaptura.modal('hide');

                        imprimirColaborador(response.data.colaboradorCapacitacionID);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarCaptura() {
            if (selectEquipoNuevo.val() == '' && selectActividad.val() == '' && selectEquipoAdiestramiento.val() == '') {
                AlertaGeneral(`Alerta`, `Debe capturar todos los campos.`);
                return;
            }

            let colaboradorCapacitacion = {
                id: botonGuardarNuevaCaptura.data().id,
                // cc: selectCentroCostoNuevo.val(),
                // area: 0, //Este valor se asigna en el back-end
                // empresa: +selectCentroCostoNuevo.find('option:selected').attr('empresa'),
                // fechaInicio: inputFechaInicio.val(),
                // fechaTermino: inputFechaTermino.val(),
                // colaborador: inputClaveColaborador.val(),
                adiestrador: inputClaveAdiestrador.val(),
                // instructor: inputNombreInstructor.data().clave_empleado,
                // seguridad: inputNombreSeguridad.data().clave_empleado,
                // recursosHumanos: inputNombreRecursosHumanos.data().clave_empleado,
                // sobrestante: inputNombreSobrestante.data().clave_empleado,
                // gerenteObra: inputNombreGerenteObra.data().clave_empleado,
                tipo: +selectTipo.val(),
                equipo: +selectEquipoNuevo.val(),
                equipoAdiestramiento_id: +selectEquipoAdiestramiento.val(),
                actividad_id: +selectActividad.val(),
            };

            axios.post('EditarColaboradorCapacitacion', { colaboradorCapacitacion }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                    modalNuevaCaptura.modal('hide');
                    cargarColaboradoresCapacitacion();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarHorasAdiestramiento() {
            let listaControlHoras = [];

            tablaHorasAdiestramiento.find('tbody tr').each(function (index, row) {
                let fecha = $(row).find('.inputFechaAdiestramiento').val();
                let horas = $(row).find('.inputHorasAdiestramiento').val();

                if (fecha != '' && +horas > 0) {
                    listaControlHoras.push({
                        fecha,
                        horas,
                        colaboradorCapacitacionID: botonGuardarHorasAdiestramiento.data().colaboradorCapacitacionID
                    });
                }
            });

            axios.post('GuardarNuevoControlHoras', { listaControlHoras })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        modalHorasAdiestramiento.modal('hide');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarActividadAdiestramiento() {
            let listaControlActividad = [];

            tablaHorasAdiestramientoActividad.find('tbody tr').each(function (index, row) {
                let fecha = $(row).find('.inputFechaAdiestramiento').val();
                let cantidad = +$(row).find('.inputCantidadActividades').val();

                if (fecha != '' && cantidad > 0) {
                    listaControlActividad.push({
                        fecha,
                        cantidad,
                        colaboradorCapacitacionID: botonGuardarHorasAdiestramientoActividad.data().colaboradorCapacitacionID
                    });
                }
            });

            axios.post('GuardarNuevoControlActividad', { listaControlActividad }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                    modalHorasAdiestramientoActividad.modal('hide');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarLiberados() {
            let captura = getInformacionLiberados();

            $.ajax({
                url: 'GuardarLiberados',
                data: captura,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                dtLiberados.clear().draw();

                if (response.success) {
                    AlertaGeneral(`Éxito`, `Se ha guardado la información.`);
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                dtLiberados.clear().draw();
            }
            );
        }

        function guardarLiberacionAdministrador() {
            let captura = [];

            tablaLiberados.find('tbody tr').each(function (index, row) {
                let rowData = dtLiberados.row(row).data();
                let liberado = $(row).find(`.regular-checkbox`).prop('checked');

                if (rowData.rutaSoporteAdiestramiento != null && liberado) {
                    captura.push({
                        id: rowData.id,
                        liberado: true
                    });
                }
            });

            if (captura.length > 0) {
                axios.post('GuardarLiberacionAdministrador', { captura }).then(response => {
                    let { success, data, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        botonBuscar.click();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                Alert2Warning('Debe seleccionar un registro para liberar');
            }
        }

        function getInformacionLiberados() {
            const data = new FormData();
            let captura = [];

            tablaLiberados.find('tbody tr').each(function (index, row) {
                let rowData = dtLiberados.row(row).data();

                if (rowData.rutaSoporteAdiestramiento == null) {
                    let inputSoporteAdiestramiento = $(row).find(`.inputSoporteAdiestramiento_${rowData.id}`);
                    let archivo = inputSoporteAdiestramiento[0].files.length > 0 ? inputSoporteAdiestramiento[0].files[0] : null;

                    if (archivo != null) {
                        let liberado = $(row).find(`.regular-checkbox`).prop('checked');

                        captura.push({
                            id: rowData.id,
                            liberado: liberado
                        });

                        data.append('archivos', archivo);
                    }
                }
            });

            data.append('captura', JSON.stringify(captura));

            return data;
        }

        function cargarModalHorasAdiestramiento(colaboradorCapacitacionID) {
            axios.get('GetInfoColaboradorCapacitacion', { params: { colaboradorCapacitacionID } })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        limpiarModalHorasAdiestramiento();
                        modalHorasAdiestramiento.modal('show');

                        labelHorasAcumuladas.text(response.data.horasAcumuladas);
                        labelHorasPendientes.text(response.data.horasPendientes);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarModalHorasAdiestramientoActividad(colaboradorCapacitacionID) {
            axios.get('GetInfoColaboradorCapacitacionActividad', { params: { colaboradorCapacitacionID } })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        limpiarModalHorasAdiestramiento();
                        modalHorasAdiestramientoActividad.modal('show');

                        labelHorasAcumuladasActividad.text(response.data.actividadAcumuladas);
                        labelHorasPendientesActividad.text(response.data.actividadPendientes);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function setDatosInstructor(e, ui) {
            inputNombreInstructor.data('clave_empleado', ui.item.id);
            inputNombreInstructor.val(ui.item.nombreEmpleado);
        }

        function verificarInstructor(e, ui) {
            if (ui.item == null) {
                inputNombreInstructor.data('clave_empleado', 0);
                inputNombreInstructor.val('');
            }
        }

        function setDatosSeguridad(e, ui) {
            inputNombreSeguridad.data('clave_empleado', ui.item.id);
            inputNombreSeguridad.val(ui.item.nombreEmpleado);
        }

        function verificarSeguridad(e, ui) {
            if (ui.item == null) {
                inputNombreSeguridad.data('clave_empleado', 0);
                inputNombreSeguridad.val('');
            }
        }

        function setDatosRecursosHumanos(e, ui) {
            inputNombreRecursosHumanos.data('clave_empleado', ui.item.id);
            inputNombreRecursosHumanos.val(ui.item.nombreEmpleado);
        }

        function verificarRecursosHumanos(e, ui) {
            if (ui.item == null) {
                inputNombreRecursosHumanos.data('clave_empleado', 0);
                inputNombreRecursosHumanos.val('');
            }
        }

        function setDatosSobrestante(e, ui) {
            inputNombreSobrestante.data('clave_empleado', ui.item.id);
            inputNombreSobrestante.val(ui.item.nombreEmpleado);
        }

        function verificarSobrestante(e, ui) {
            if (ui.item == null) {
                inputNombreSobrestante.data('clave_empleado', 0);
                inputNombreSobrestante.val('');
            }
        }

        function setDatosGerenteObra(e, ui) {
            inputNombreGerenteObra.data('clave_empleado', ui.item.id);
            inputNombreGerenteObra.val(ui.item.nombreEmpleado);
        }

        function verificarGerenteObra(e, ui) {
            if (ui.item == null) {
                inputNombreGerenteObra.data('clave_empleado', 0);
                inputNombreGerenteObra.val('');
            }
        }

        function limpiarModalCaptura() {
            selectCentroCostoNuevo.val('');
            selectCentroCostoNuevo.select2().change();
            selectTipo.val('');
            selectTipo.change();
            selectEquipoNuevo.val('');
            selectEquipoNuevo.select2().change();
            selectEquipoAdiestramiento.val('');
            selectEquipoAdiestramiento.select2().change();
            inputFechaInicio.val('');
            inputFechaTermino.val('');
            inputClaveColaborador.val('');
            inputNombreColaborador.val('');
            inputClaveAdiestrador.val('');
            inputNombreAdiestrador.val('');
            inputNombreInstructor.data('clave_empleado', 0);
            inputNombreInstructor.val('');
            inputNombreSeguridad.data('clave_empleado', 0);
            inputNombreSeguridad.val('');
            inputNombreRecursosHumanos.data('clave_empleado', 0);
            inputNombreRecursosHumanos.val('');
            inputNombreSobrestante.data('clave_empleado', 0);
            inputNombreSobrestante.val('');
            inputNombreGerenteObra.data('clave_empleado', 0);
            inputNombreGerenteObra.val('');
        }

        function llenarModalCaptura(rowData) {
            selectCentroCostoNuevo.val(rowData.cc);
            selectCentroCostoNuevo.select2().change();
            selectTipo.val(rowData.tipo);

            switch (rowData.tipo) {
                case 1:
                    selectEquipoNuevo.attr('disabled', false);
                    selectEquipoAdiestramiento.attr('disabled', false);
                    selectActividad.attr('disabled', true);
                    break;
                case 2:
                    selectEquipoNuevo.attr('disabled', true);
                    selectEquipoAdiestramiento.attr('disabled', true);
                    selectActividad.attr('disabled', false);
                    break;
                default:
                    selectEquipoNuevo.attr('disabled', true);
                    selectEquipoAdiestramiento.attr('disabled', true);
                    selectActividad.attr('disabled', true);
                    break;
            }

            selectEquipoNuevo.val(rowData.equipo);
            selectEquipoNuevo.select2().change();
            selectEquipoAdiestramiento.val(rowData.equipoAdiestramiento_id);
            selectEquipoAdiestramiento.select2().change();
            selectActividad.val(rowData.actividad_id);
            inputFechaInicio.val(rowData.fechaInicioString);
            inputFechaTermino.val(rowData.fechaTerminoString);
            inputClaveColaborador.val(rowData.colaborador);
            inputNombreColaborador.val(rowData.colaboradorDesc);
            inputClaveAdiestrador.val(rowData.adiestrador);
            inputNombreAdiestrador.val(rowData.adiestradorDesc);
            inputNombreInstructor.data('clave_empleado', rowData.instructor);
            inputNombreInstructor.val(rowData.nombreInstructor);
            inputNombreSeguridad.data('clave_empleado', rowData.seguridad);
            inputNombreSeguridad.val(rowData.nombreSeguridad);
            inputNombreRecursosHumanos.data('clave_empleado', rowData.recursosHumanos);
            inputNombreRecursosHumanos.val(rowData.nombreRecursosHumanos);
            inputNombreSobrestante.data('clave_empleado', rowData.sobrestante);
            inputNombreSobrestante.val(rowData.nombreSobrestante);
            inputNombreGerenteObra.data('clave_empleado', rowData.gerenteObra);
            inputNombreGerenteObra.val(rowData.nombreGerenteObra);
        }

        function limpiarModalHorasAdiestramiento() {
            dtHorasAdiestramiento.clear().draw();
            labelHorasAcumuladas.text('');
            labelHorasPendientes.text('');

            dtActividadAdiestramiento.clear().draw();
            labelHorasAcumuladasActividad.text('');
            labelHorasPendientesActividad.text('');
        }

        function validarCamposNuevaCaptura() {
            let campoInvalido = false;

            if (selectCentroCostoNuevo.val() == '') {
                campoInvalido = true;
            }

            if (selectEquipoNuevo.val() == '' && selectActividad.val() == '' && selectEquipoAdiestramiento.val() == '') {
                campoInvalido = true;
            }

            if (inputFechaInicio.val() == '') {
                campoInvalido = true;
            }

            if (inputFechaTermino.val() == '') {
                campoInvalido = true;
            }

            if (inputClaveColaborador.val() == '') {
                campoInvalido = true;
            }

            if (inputClaveAdiestrador.val() == '') {
                campoInvalido = true;
            }

            if (inputClaveAdiestrador.val() == '') {
                campoInvalido = true;
            }

            if (+inputNombreInstructor.data().clave_empleado == 0 || isNaN(inputNombreInstructor.data().clave_empleado)) {
                campoInvalido = true;
            }

            if (+inputNombreSeguridad.data().clave_empleado == 0 || isNaN(inputNombreSeguridad.data().clave_empleado)) {
                campoInvalido = true;
            }

            if (+inputNombreRecursosHumanos.data().clave_empleado == 0 || isNaN(inputNombreRecursosHumanos.data().clave_empleado)) {
                campoInvalido = true;
            }

            if (+inputNombreSobrestante.data().clave_empleado == 0 || isNaN(inputNombreSobrestante.data().clave_empleado)) {
                campoInvalido = true;
            }

            if (+inputNombreGerenteObra.data().clave_empleado == 0 || isNaN(inputNombreGerenteObra.data().clave_empleado)) {
                campoInvalido = true;
            }

            return campoInvalido;
        }

        function initMonthPicker(input) {
            $(input).datepicker({
                dateFormat: "mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                maxDate: fechaActual,
                showAnim: showAnim,
                closeText: "Aceptar",
                onClose: function (dateText, inst) {
                    function isDonePressed() {
                        return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                    }

                    if (isDonePressed()) {
                        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                        $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');

                        $('.date-picker').focusout()//Added to remove focus from datepicker input box on selecting date
                    }
                },
                beforeShow: function (input, inst) {
                    inst.dpDiv.addClass('month_year_datepicker')

                    if ((datestr = $(this).val()).length > 0) {
                        year = datestr.substring(datestr.length - 4, datestr.length);
                        month = datestr.substring(0, 2);
                        $(this).datepicker('option', 'defaultDate', new Date(year, month - 1, 1));
                        $(this).datepicker('setDate', new Date(year, month - 1, 1));
                        $(".ui-datepicker-calendar").hide();
                    }
                }
            }).datepicker("setDate", fechaActual);
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        function revisarPrivilegio() {
            axios.get('privilegioCapacitacion')
                .then(response => {
                    if (response.data == 0) {
                        AlertaGeneral(`Alerta`, `No tiene permisos para visualizar este módulo.`);
                    } else {
                        _privilegioUsuario = response.data;

                        if (response.data == 2) {
                            botonGuardarLiberados.attr('disabled', true);
                            botonNuevaCaptura.attr('disabled', true);
                        }
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function SetDataInTablesDetalleMtto(dataSet) {
            tblDetalleHorasHombre = $("#tblDetalleHorasHombre").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: true,
                "bFilter": true,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [

                    {
                        data: "personalNombre"
                    },
                    {
                        data: "hrasPreventivo"
                    },
                    {
                        data: "hrasPredictivo"
                    },
                    {
                        data: "hrasCorrectivo"
                    },
                    {
                        data: "cantidadOT"

                    }
                    , {
                        data: "promedioHrasOT"

                    },
                    {
                        data: "btnDetalle",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append('<button type="button" class="btn btn-default btn-block btn-sm verOTEmpleado">Detalle</button>');
                        }
                    },
                ],
                "paging": true,
                "info": false,
                initComplete: function (settings, json) {
                    $("#tblDetalleHorasHombre").on('click', '.verOTEmpleado', function () {
                        // onclick="verOTEmpleado(' + rowData.personalID + ',\'' + rowData.personalNombre + '\')"
                        let rowData = tblDetalleHorasHombre.row($(this).closest('tr')).data();
                        verOTEmpleado(rowData.personalID, rowData.personalNombre);
                    });
                }
            });
        }

        myChart2 = null;

        function setGraficaMtto(dt) {
            if (myChart2 != null) {
                myChart2.destroy();
            }

            var dtPersonal = new Array();
            var dtPredictivo = new Array();
            var dtCorrectivo = new Array();
            var dtPreventivo = new Array();
            for (var i = 0; i < dt.length; i++) {
                dtPersonal.push(dt[i].personalNombre);
                dtCorrectivo.push(dt[i].hrasCorrectivo);
                dtPredictivo.push(dt[i].hrasPredictivo);
                dtPreventivo.push(dt[i].hrasPreventivo);

            }

            var ctx = document.getElementById("LineWithLine1").getContext("2d");

            var data = {
                labels: dtPersonal,
                datasets: [{
                    label: "Predictivo",
                    backgroundColor: "blue",
                    data: dtPredictivo
                }, {
                    label: "Correctivo",
                    backgroundColor: "red",
                    data: dtCorrectivo
                }, {
                    label: "Preventivo",
                    backgroundColor: "green",
                    data: dtPreventivo
                }]
            };

            myChart2 = new Chart(ctx, {
                type: 'bar',
                data: data,
                options: {
                    barValueSpacing: 20,
                    scales: {
                        yAxes: [{
                            ticks: {
                                min: 0,
                            }
                        }]
                    }
                }
            });
        }

        function loadDataDetalleMtto(colaboradorCapacitacionId) {
            axios.get('GetMtto', { params: { colaboradorCapacitacionId } }).then(response => {
                let { success, datos, message, dataset } = response.data;

                $("#divOTModalDetalle").addClass('hide');
                $("#divPersonalModalDetalle").removeClass('hide');

                $("#modalDetalleHorasHombre").modal('show');
                // $("#modalDetalleHorasHombre").block({ message: null });

                $('#modalDetalleHorasHombre').on('shown.bs.modal', function () {
                    // $("#modalDetalleHorasHombre").unblock();

                    if (success) {
                        $("#titleModalDetalle").text(dataset.puestoDesc);
                        SetDataInTablesDetalleMtto(dataset);
                        setGraficaMtto(dataset);
                        initGraficaMTTO(response.data.data);
                    } else {
                        AlertaGeneral('Alerta', message);
                    }
                });
            }).catch(error => AlertaGeneral('Alerta', error.message));
        }

        function verOTEmpleado(personalID, nombrePersona) {
            axios.get('GetMttoDetalle', { params: { clave_empleado: personalID } }).then(response => {
                let { success, datos, message, dataResult } = response.data;

                if (success) {
                    $("#divPersonalModalDetalle").addClass('hide');
                    $("#divOTModalDetalle").removeClass('hide');

                    setTablaOT(dataResult);

                    $("#txtOTDetalle").text(nombrePersona);
                } else {
                    AlertaGeneral('Alerta', message);
                }
            }).catch(error => AlertaGeneral('Alerta', error.message));
        }

        function setTablaOT(dataSet) {
            tblOTsPersonal = $("#tblOTsPersonal").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: true,
                "bFilter": true,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [

                    {
                        data: "folio"
                    },
                    {
                        data: "economico"
                    },
                    {
                        data: "motivoParo"
                    },
                    {
                        data: "inicioParo"
                    },
                    {
                        data: "finParo"
                    }
                ],
                "paging": true,
                "info": false

            });
        }
    }
    $(document).ready(() => Capacitacion.HorasAdiestramiento = new HorasAdiestramiento())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();