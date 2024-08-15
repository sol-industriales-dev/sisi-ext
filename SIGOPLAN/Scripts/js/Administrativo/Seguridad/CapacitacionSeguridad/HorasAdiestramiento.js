(() => {
    $.namespace('Capacitacion.HorasAdiestramiento');
    HorasAdiestramiento = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const inputMesInicial = $('#inputMesInicial');
        const inputMesFinal = $('#inputMesFinal');
        const botonBuscar = $('#botonBuscar');
        const btnNuevoColaborador = $('#btnNuevoColaborador');
        const botonNuevaCaptura = $('#botonNuevaCaptura');
        const modalNuevaCaptura = $('#modalNuevaCaptura');
        const selectCentroCostoNuevo = $('#selectCentroCostoNuevo');
        const selectEquipoNuevo = $('#selectEquipoNuevo');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaTermino = $('#inputFechaTermino');
        const inputClaveColaborador = $('#inputClaveColaborador');
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
        const tablaPendientes = $('#tablaPendientes');
        const tablaLiberados = $('#tablaLiberados');
        const tablaHorasAdiestramiento = $('#tablaHorasAdiestramiento');
        const botonGuardarHorasAdiestramiento = $('#botonGuardarHorasAdiestramiento');
        const botonAgregarRenglonHoras = $('#botonAgregarRenglonHoras');
        const labelHorasAcumuladas = $('#labelHorasAcumuladas');
        const labelHorasPendientes = $('#labelHorasPendientes');
        const botonGuardarLiberados = $('#botonGuardarLiberados');
        const report = $("#report");
        //#endregion

        let dtPendientes;
        let dtLiberados;
        let dtHorasAdiestramiento;

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioMes = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        //#endregion

        _privilegioUsuario = 0;

        (function init() {
            revisarPrivilegio();
            $('.select2').select2();

            initBotones();
            initCombo();
            initTablaPendientes();
            initTablaLiberados();
            initTablaHorasAdiestramiento();

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

        function initBotones() {
            botonNuevaCaptura.click(function () {
                limpiarModalCaptura();
                modalNuevaCaptura.modal('show');
            });
            botonBuscar.click(cargarColaboradoresCapacitacion);
            botonGuardarNuevaCaptura.click(guardarNuevaCaptura);
            botonGuardarHorasAdiestramiento.click(guardarHorasAdiestramiento);
            botonGuardarLiberados.click(guardarLiberados);
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

            selectEquipoNuevo.fillCombo('GetEquiposCombo', null, false, null);
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

                    tablaPendientes.on('click', '.botonReporteColaborador', function () {
                        let rowData = tablaPendientes.DataTable().row($(this).closest('tr')).data();

                        imprimirColaborador(rowData.id);
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
                    { data: 'equipoDesc', title: 'EQUIPO' },
                    { data: 'fechaInicioString', title: 'INICIO' },
                    { data: 'fechaTerminoString', title: 'TERMINO' },
                    { data: 'adiestradorDesc', title: 'CAPACITADOR' },
                    {
                        title: 'HRS ADIESTRAMIENTO', render: function (data, type, row, meta) {
                            return row.estatus_empleado == 'A' ? `<button class="btn btn-sm btn-warning botonHorasAdiestramiento"><i class="fa fa-eye"></i></button>` : ``;
                        }
                    },
                    {
                        title: '', render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-primary botonReporteColaborador"><i class="fa fa-file"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
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
                    { data: 'equipoDesc', title: 'EQUIPO' },
                    { data: 'fechaInicioString', title: 'INICIO' },
                    { data: 'fechaTerminoString', title: 'TERMINO' },
                    { data: 'adiestradorDesc', title: 'CAPACITADOR' },
                    {
                        title: 'LIBERADO', render: function (data, type, row, meta) {
                            return row.puedeEvaluar ? `
                                <div>
                                    <input type="checkbox" id="checkBoxLiberado_${meta.row}" class="regular-checkbox" ${row.liberado ? `checked` : ``} ${row.rutaSoporteAdiestramiento != null || _privilegioUsuario == 2 ? 'disabled' : ''}>
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
                                    ${row.rutaSoporteAdiestramiento == null ? `
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

            axios.post('CargarHorasAdiestramiento', { listaCC, fechaInicial, fechaFinal })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaPendientes, response.data.pendientes);
                        AddRows(tablaLiberados, response.data.liberados);
                        initGraficaColaboradoresAdiestramiento(response.data.dashboard);
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

        function guardarNuevaCaptura() {
            if (validarCamposNuevaCaptura()) {
                AlertaGeneral(`Alerta`, `Debe capturar todos los campos.`);
                return;
            }

            let colaboradorCapacitacion = {
                cc: selectCentroCostoNuevo.val(),
                area: 0, //Este valor se asigna en el back-end
                empresa: +selectCentroCostoNuevo.find('option:selected').attr('empresa'),
                equipo: +selectEquipoNuevo.val(),
                fechaInicio: inputFechaInicio.val(),
                fechaTermino: inputFechaTermino.val(),
                colaborador: inputClaveColaborador.val(),
                adiestrador: inputClaveAdiestrador.val(),
                instructor: inputNombreInstructor.data().clave_empleado,
                seguridad: inputNombreSeguridad.data().clave_empleado,
                recursosHumanos: inputNombreRecursosHumanos.data().clave_empleado,
                sobrestante: inputNombreSobrestante.data().clave_empleado,
                gerenteObra: inputNombreGerenteObra.data().clave_empleado
            };

            axios.post('GuardarNuevoColaboradorCapacitacion', { colaboradorCapacitacion })
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
            selectEquipoNuevo.val('');
            selectEquipoNuevo.select2().change();
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

        function limpiarModalHorasAdiestramiento() {
            dtHorasAdiestramiento.clear().draw();
            labelHorasAcumuladas.text('');
            labelHorasPendientes.text('');
        }

        function validarCamposNuevaCaptura() {
            let campoInvalido = false;

            if (selectCentroCostoNuevo.val() == '') {
                campoInvalido = true;
            }

            if (selectEquipoNuevo.val() == '') {
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
    }
    $(document).ready(() => Capacitacion.HorasAdiestramiento = new HorasAdiestramiento())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();