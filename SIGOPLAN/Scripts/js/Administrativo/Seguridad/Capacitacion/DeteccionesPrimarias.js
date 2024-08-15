(() => {
    $.namespace('Administrativo.Seguridad.DeteccionesPrimarias');
    DeteccionesPrimarias = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const selectArea = $('#selectArea');
        const inputFecha = $('#inputFecha');
        const botonNuevoRegistro = $('#botonNuevoRegistro');
        const botonBuscarDetecciones = $('#botonBuscarDetecciones');
        const tablaDeteccionesPrimarias = $('#tablaDeteccionesPrimarias');
        const modalNuevaDeteccionPrimaria = $('#modalNuevaDeteccionPrimaria');
        const selectCentroCostoNuevo = $('#selectCentroCostoNuevo');
        const selectAreaNuevo = $('#selectAreaNuevo');
        const inputFechaNuevo = $('#inputFechaNuevo');
        const botonAgregarNecesidad = $('#botonAgregarNecesidad');
        const tablaNecesidadesDetectadas = $('#tablaNecesidadesDetectadas');
        const botonGuardarDeteccionPrimaria = $('#botonGuardarDeteccionPrimaria');
        const report = $("#report");
        const tblCandidatos = $('#tblCandidatos')
        let dtCandidatos;
        //#endregion

        let dtDeteccionesPrimarias;
        let dtNecesidadesDetectadas;

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        //#endregion

        _privilegioUsuario = 0;

        (function init() {
            revisarPrivilegio();
            $('.select2').select2({ language: { noResults: () => { return "No hay resultados" }, searching: () => { return "Buscando..." } } });

            initTablaDeteccionesPrimarias();
            initTablaNecesidadesDetectadas();
            initTblCandidatos()

            inputFecha.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
            inputFechaNuevo.datepicker({ dateFormat, maxDate: fechaActual, showAnim });

            setCombos();

            botonNuevoRegistro.click(() => {
                limpiarModalDeteccionPrimaria();
                modalNuevaDeteccionPrimaria.modal('show');
            });

            botonBuscarDetecciones.click(buscarDetecciones);
            botonGuardarDeteccionPrimaria.click(guardarDeteccionPrimaria);

            selectCentroCosto.change(cargarAreasCC);
            selectCentroCostoNuevo.change(cargarAreasCCNuevo);
        })();

        botonAgregarNecesidad.on('click', function () {
            let datos = dtNecesidadesDetectadas.rows().data();

            datos.push({
                id: 0,
                metodo: '',
                detecciones: '',
                accionesCursoID: '',
                observaciones: ''
            });

            dtNecesidadesDetectadas.clear();
            dtNecesidadesDetectadas.rows.add(datos).draw();
        });

        function setCombos() {
            axios.get('ObtenerComboCCAmbasEmpresas').then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    selectCentroCosto.append('<option value="Todos">Todos</option>');
                    selectCentroCostoNuevo.append('<option value="">--Seleccione--</option>');

                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}">`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : x.label == 'PERÚ' ? 6 : 0}">${y.Text}</option>`;
                        });

                        groupOption += `</optgroup>`;

                        selectCentroCosto.append(groupOption);
                        selectCentroCostoNuevo.append(groupOption);
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }

                convertToMultiselect('#selectCentroCosto');
                convertToMultiselect('#selectAreaNuevo');
            }).catch(error => AlertaGeneral(`Alerta`, error.message));

            // axios.get('GetDepartamentosCombo').then(response => {
            //     let { success, items, message } = response.data;

            //     if (success) {
            //         selectArea.append('<option value="Todos">Todos</option>');
            //         selectAreaNuevo.append('<option value="">--Seleccione--</option>');

            //         items.forEach(x => {
            //             let groupOption = `<optgroup label="${x.label}">`;

            //             x.options.forEach(y => {
            //                 groupOption += `<option value="${y.Value}" empresa="${y.Prefijo == 'CONSTRUPLAN' ? 1 : y.Prefijo == 'ARRENDADORA' ? 2 : 0}" cc="${y.Id}">${y.Text}</option>`;
            //             });

            //             groupOption += `</optgroup>`;

            //             selectArea.append(groupOption);
            //             selectAreaNuevo.append(groupOption);
            //         });

            convertToMultiselect('#selectArea');
            //     } else {
            //         AlertaGeneral(`Alerta`, message);
            //     }
            // }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initTablaDeteccionesPrimarias() {
            dtDeteccionesPrimarias = tablaDeteccionesPrimarias.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaDeteccionesPrimarias.on('click', '.botonConsultarFormato', function () {
                        let deteccionPrimariaID = +$(this).attr('deteccionprimaria');

                        imprimirDeteccionPrimaria(deteccionPrimariaID);
                    });
                },
                columns: [
                    { data: 'areaDesc', title: 'ÁREAS' },
                    { data: 'fechaString', title: 'FECHA' },
                    {
                        title: 'CONSULTAR FORMATO', render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-default botonConsultarFormato" deteccionprimaria="${row.id}"><i class="fa fa-file"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaNecesidadesDetectadas() {
            dtNecesidadesDetectadas = tablaNecesidadesDetectadas.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaNecesidadesDetectadas.on('change', '.selectMetodo, .inputDetecciones, .selectAccionesCursos, .inputObservaciones', function () {
                        tablaNecesidadesDetectadas.find('tbody tr').each(function (index, row) {
                            let rowData = tablaNecesidadesDetectadas.DataTable().row(row).data();

                            let metodo = $(row).find('.selectMetodo').val();
                            let detecciones = $(row).find('.inputDetecciones').val();
                            let accionesCursoID = $(row).find('.selectAccionesCursos').val();
                            let observaciones = $(row).find('.inputObservaciones').val();

                            rowData.metodo = metodo;
                            rowData.detecciones = detecciones;
                            rowData.accionesCursoID = accionesCursoID;
                            rowData.observaciones = observaciones;

                            tablaNecesidadesDetectadas.DataTable().row(row).data(rowData).draw();

                            inicializarComboCursos(row, rowData);
                        });
                    });

                    tablaNecesidadesDetectadas.on('click', '.botonQuitarNecesidad', function () {
                        dtNecesidadesDetectadas.row($(this).closest('tr')).remove().draw();
                    });
                },
                createdRow: function (row, rowData) {
                    $(row).find('.selectMetodo').val(rowData.metodo);

                    inicializarComboCursos(row, rowData);
                },
                columns: [
                    {
                        title: 'Método', render: function (data, type, row, meta) {
                            return `
                                <select class="form-control selectMetodo">
                                    <option value="" ${row.metodo == '' ? 'selected' : ''}>--Seleccione--</option>
                                    <option value="1" ${row.metodo == 1 ? 'selected' : ''}>Capacitación</option>
                                    <option value="2" ${row.metodo == 2 ? 'selected' : ''}>Adiestramiento</option>
                                    <option value="3" ${row.metodo == 3 ? 'selected' : ''}>Monitoreo</option>
                                    <option value="4" ${row.metodo == 4 ? 'selected' : ''}>Otros</option>
                                </select>`;
                        }
                    },
                    {
                        title: 'Detecciones', render: function (data, type, row, meta) {
                            return `<input class="form-control inputDetecciones" value="${row.detecciones}">`;
                        }
                    },
                    {
                        title: 'Acciones a tomar', render: function (data, type, row, meta) {
                            return `<select class="form-control selectAccionesCursos"></select>`;
                        }
                    },
                    {
                        title: 'Observaciones', render: function (data, type, row, meta) {
                            return `<input class="form-control inputObservaciones" value="${row.observaciones}">`;
                        }
                    },
                    {
                        title: '', render: function (data, type, row, meta) {
                            return `<button class="btn btn-danger botonQuitarNecesidad"><i class="fa fa-times"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function inicializarComboCursos(row, rowData) {
            let selectAccionesCursos = $(row).find('.selectAccionesCursos');

            selectAccionesCursos.fillCombo('ObtenerComboCursos', null, false, null);
            selectAccionesCursos.val(rowData.accionesCursoID);
        }

        function limpiarModalDeteccionPrimaria() {
            selectCentroCostoNuevo.val('');
            // selectCentroCostoNuevo.select2().change();
            selectAreaNuevo.empty();
            selectAreaNuevo.multiselect('rebuild');
            inputFechaNuevo.val('');

            dtNecesidadesDetectadas.clear().draw();
        }

        function buscarDetecciones() {
            let listaCC = getValoresMultiples('#selectCentroCosto');
            let listaAreas = [];
            let fecha = inputFecha.val();

            getValoresMultiplesCustom('#selectArea').forEach(x => {
                listaAreas.push({
                    cc: x.cc,
                    area: +x.departamento,
                    empresa: +x.empresa
                });
            });

            axios.post('GetRegistrosDeteccionesPrimarias', { listaCC, listaAreas, fecha })
                .then(response => {
                    let { success, datos, message } = response.data;
                    if (success) {
                        AddRows(tablaDeteccionesPrimarias, datos);
                        dtCandidatos.clear();
                        dtCandidatos.rows.add(response.data.lstCandidatos);
                        dtCandidatos.draw();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarDeteccionPrimaria() {
            let deteccionPrimaria = {
                cc: selectCentroCostoNuevo.val(),
                area: 0,
                empresa: 0,
                fecha: inputFechaNuevo.val()
            };

            let listaNecesidades = [];

            tablaNecesidadesDetectadas.find('tbody tr').each(function (index, row) {
                listaNecesidades.push({
                    metodo: +$(row).find('.selectMetodo').val(),
                    detecciones: $(row).find('.inputDetecciones').val(),
                    accionesCursoID: $(row).find('.selectAccionesCursos').val(),
                    observaciones: $(row).find('.inputObservaciones').val()
                });
            });

            let listaAreas = [];

            getValoresMultiplesCustom('#selectAreaNuevo').forEach(x => {
                listaAreas.push({
                    cc: x.cc,
                    area: +x.departamento,
                    empresa: +x.empresa
                });
            });

            axios.post('GuardarDeteccionPrimaria', { deteccionPrimaria, listaNecesidades, listaAreas })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalNuevaDeteccionPrimaria.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function imprimirDeteccionPrimaria(deteccionPrimariaID) {
            if (deteccionPrimariaID == 0) {
                AlertaGeneral(`Alerta`, `No se ha seleccionado una detección primaria.`);
                return;
            }

            $.blockUI({ message: 'Generando imprimible...' });
            var path = `/Reportes/Vista.aspx?idReporte=211&deteccionPrimariaID=${deteccionPrimariaID}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
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

        function revisarPrivilegio() {
            axios.get('privilegioCapacitacion')
                .then(response => {
                    if (response.data == 0) {
                        AlertaGeneral(`Alerta`, `No tiene permisos para visualizar este módulo.`);
                    } else {
                        _privilegioUsuario = response.data;

                        if (response.data == 2) {
                            botonNuevoRegistro.attr('disabled', true);
                        }
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initTblCandidatos() {
            dtCandidatos = tblCandidatos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreCompleto', title: 'Candidato' },
                    { data: 'puestoDesc', title: 'puesto' },
                    { data: 'calificacion', title: 'Calificación' },
                    { data: 'comentario', title: 'Observación' }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

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

        function cargarAreasCCNuevo() {
            let listaCentrosCosto = [];

            getValoresMultiplesCustom('#selectCentroCostoNuevo').forEach(x => {
                listaCentrosCosto.push({
                    cc: x.value,
                    empresa: +(x.empresa)
                });
            });

            if (listaCentrosCosto.filter((x) => x.cc != '').length == 0) {
                selectAreaNuevo.empty();
                return;
            }

            let ccsCplan = listaCentrosCosto.filter((x) => { return x.empresa == 1 || x.empresa == 6; }).map(function (x) { return x.cc; });
            let ccsArr = listaCentrosCosto.filter((x) => { return x.empresa == 2; }).map(function (x) { return x.cc; });

            $.post('/Administrativo/Capacitacion/ObtenerAreasPorCC', { ccsCplan, ccsArr })
                .then(response => {
                    selectAreaNuevo.empty();
                    if (response.success) {
                        const option = `<option value="Todos">Todos</option>`;
                        selectAreaNuevo.append(option);

                        response.items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" cc="${y.Id}" empresa="${y.Prefijo}">${y.Text}</option>`;
                            });
                            selectAreaNuevo.append(groupOption);
                        });

                        convertToMultiselect('#selectAreaNuevo');
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
    $(document).ready(() => Administrativo.Seguridad.DeteccionesPrimarias = new DeteccionesPrimarias())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();