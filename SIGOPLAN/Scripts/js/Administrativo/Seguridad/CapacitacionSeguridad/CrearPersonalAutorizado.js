(() => {
    $.namespace('administrativo.seguridad.CrearPersonalAutorizado');
    CrearPersonalAutorizado = function () {
        //#region Selectores
        const selectCursoAutorizado = $('#selectCursoAutorizado');
        const selectDepartamento = $('#selectDepartamento');
        const botonBuscar = $('#botonBuscar');
        const botonRegistrar = $('#botonRegistrar');
        const tablaListasAutorizacion = $('#tablaListasAutorizacion');
        const modalRegistro = $('#modalRegistro');
        const inputClaveListaAutorizacion = $('#inputClaveListaAutorizacion');
        const selectCurso = $('#selectCurso');
        const selectDepartamentoCaptura = $('#selectDepartamentoCaptura');
        const textAreaObjetivo = $('#textAreaObjetivo');
        const textAreaReferenciaNormativa = $('#textAreaReferenciaNormativa');
        const textAreaNotaCurso = $('#textAreaNotaCurso');
        const inputRevision = $('#inputRevision');
        const botonAgregarRenglon = $('#botonAgregarRenglon');
        const tablaRFC = $('#tablaRFC');
        const botonGuardar = $('#botonGuardar');
        //#endregion

        let dtListaAutorizacion;
        let dtRFC;
        const ESTATUS = {
            NUEVO: 0,
            EDITAR: 1
        };

        (function init() {
            $('.select2').select2();
            //$('.select2Multi').select2();

            initTablaListasAutorizacion();
            initTablaRFC();
            setDatosInicio();
            agregarListeners();
        })();

        $(document).ready(function() {
            selectDepartamentoCaptura.select2();          
            selectDepartamentoCaptura.trigger("change");
        });   

        botonAgregarRenglon.on('click', function () {
            let datos = dtRFC.rows().data();

            $.each(datos, function (idx, data) {
                let row = tablaRFC.find('tbody tr').eq(idx);

                data.rfc = $(row).find('.inputRFC').val();
                data.razonSocial = $(row).find('.inputRazonSocial').val();
            });

            datos.push({
                rfc: '',
                razonSocial: ''
            });

            dtRFC.clear();
            dtRFC.rows.add(datos).draw();
        });

        selectCurso.on('change', function () {
            let cursoID = selectCurso.val();

            if (cursoID == '') {
                textAreaNotaCurso.val('');
                textAreaObjetivo.val('');
                textAreaReferenciaNormativa.val('');
            } else {
                axios.get('GetCursoById', { params: { id: cursoID } })
                    .then(response => {
                        let { success, informacion, message } = response.data;

                        if (success) {
                            textAreaNotaCurso.val(informacion.nota);
                            textAreaObjetivo.val(informacion.objetivo);
                            textAreaReferenciaNormativa.val(informacion.referenciasNormativas);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        });

        function initTablaListasAutorizacion() {
            dtListaAutorizacion = tablaListasAutorizacion.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaListasAutorizacion.on('click', '.btn-editar', function () {
                        let rowData = dtListaAutorizacion.row($(this).closest('tr')).data();

                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;

                        axios.get('GetListaAutorizacionByID', { params: { listaAutorizacionID: rowData.id } })
                            .then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    limpiarModal();
                                    llenarModal(datos);
                                    modalRegistro.modal('show');
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    });

                    tablaListasAutorizacion.on('click', '.btn-eliminar', function () {
                        let rowData = dtListaAutorizacion.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación',
                            `¿Está seguro de eliminar la lista de autorización "${rowData.claveLista}"? ATENCIÓN: Se perderá toda la información relacionada.`,
                            () => eliminarListaAutorizacion(rowData.id))
                    });
                },
                columns: [
                    { data: 'claveLista', title: 'Clave' },
                    {
                        title: 'Curso', render: function (data, type, row, meta) {
                            return row.claveCurso + ' ' + row.cursoNombre;
                        }
                    },
                    { data: 'ccDesc', title: 'Centro de Costo' },
                    { data: 'departamentoDesc', title: 'Departamento' },
                    {
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return `
                                <button title="Editar" class="btn-editar btn btn-sm btn-warning" value="${row.id}">
                                    <i class="fas fa-pencil-alt"></i>
                                </button>
                                &nbsp;
                                <button title="Eliminar" class="btn-eliminar btn btn-sm btn-danger" value="${row.id}">
                                    <i class="fas fa-trash"></i>
                                </button>`;
                        },
                        title: "Acciones"
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaRFC() {
            dtRFC = tablaRFC.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                searching: false,
                bInfo: false,
                ordering: false,
                initComplete: function (settings, json) {
                    tablaRFC.on('click', '.botonQuitarRFC', function () {
                        let row = $(this).closest('tr');

                        dtRFC.row(row).remove().draw();

                        let cuerpo = tablaRFC.find('tbody');

                        if (cuerpo.find("tr").length == 0) {
                            dtRFC.draw();
                        } else {
                            tablaRFC.find('tbody tr').each(function (idx, row) {
                                let rowData = dtRFC.row(row).data();

                                if (rowData != undefined) {
                                    rowData.rfc = $(row).find('.inputRFC').val();
                                    rowData.razonSocial = $(row).find('.inputRazonSocial').val();

                                    dtRFC.row(row).data(rowData).draw();
                                }
                            });
                        }
                    });
                },
                columns: [
                    {
                        title: 'RFC', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center inputRFC" value="${row.rfc}">`;
                        }
                    },
                    {
                        title: 'Razón Social', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center inputRazonSocial" value="${row.razonSocial}">`;
                        }
                    },
                    {
                        title: 'Quitar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-danger btn-sm botonQuitarRFC"><i class="fa fa-times"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function setDatosInicio() {
            selectCursoAutorizado.fillCombo('ObtenerComboCursos', null, false, 'Todos');
            convertToMultiselect('#selectCursoAutorizado');

            axios.get('GetDepartamentosCombo')
                .then(response => {
                    let { success, items, message } = response.data;

                    if (success) {
                        selectDepartamento.append('<option value="Todos">Todos</option>');
                        selectDepartamentoCaptura.append('<option value="Todos">Todos</option>');

                        let valorContador = 0;
                        items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}">`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${valorContador}" empresa="${y.Prefijo == 'CONSTRUPLAN' ? 1 : y.Prefijo == 'ARRENDADORA' ? 2 : 0}" cc="${y.Id}" departamento="${y.Value}">${y.Text}</option>`;
                                valorContador++;
                            });
                            groupOption += `</optgroup>`;
                            selectDepartamento.append(groupOption);
                            selectDepartamentoCaptura.append(groupOption);
                        });

                        convertToMultiselect('#selectDepartamento');
                        //convertToMultiselect('#selectDepartamentoCaptura');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));

            selectCurso.fillCombo('ObtenerComboCursos', null, false, null);
        }

        function agregarListeners() {
            botonBuscar.click(buscarListasAutorizacion)
            botonRegistrar.click(() => {
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                limpiarModal();
                modalRegistro.modal('show');
            });
            botonGuardar.click(guardarListaAutorizacion)
        }

        function limpiarModal() {
            inputClaveListaAutorizacion.val('');
            selectCurso.val('');
            selectCurso.select2().change();
            inputRevision.val('');
            textAreaNotaCurso.val('');
            textAreaObjetivo.val('');
            textAreaReferenciaNormativa.val('');
            selectDepartamentoCaptura.val('');
            selectDepartamentoCaptura.multiselect('deselectAll', false);
            selectDepartamentoCaptura.multiselect('refresh');

            dtRFC.clear().draw();
        }

        function llenarModal(datos) {
            inputClaveListaAutorizacion.val(datos.claveLista);
            inputRevision.val(datos.revision);
            selectCurso.val(datos.cursoID);
            selectCurso.select2().trigger('change');

            //#region Campo Departamento
            selectDepartamentoCaptura.find('option').each((index, element) => {
                let cc = $(element).attr('cc');
                let empresa = $(element).attr('empresa');
                let departamento = $(element).attr('departamento');

                if (datos.listaCC.some(x => x.cc == cc && x.empresa == empresa && x.departamento == departamento)) {
                    selectDepartamentoCaptura.multiselect('select', $(element).val(), true);
                }
            });

            selectDepartamentoCaptura.multiselect('refresh');
            //#endregion

            AddRows(tablaRFC, datos.listaRFC);
        }

        function buscarListasAutorizacion() {
            let listaCursos = getValoresMultiples('#selectCursoAutorizado');
            let listaCC = [];

            getValoresMultiplesCustom('#selectDepartamento').forEach(x => {
                listaCC.push({
                    cc: x.cc,
                    departamento: +x.departamento,
                    empresa: +x.empresa
                });
            });

            axios.post('GetListasAutorizacion', { listaCursos, listaCC })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaListasAutorizacion, datos);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function validarGuardarListaAutorizacion() {
            let countInvalidos = 0

            if (inputClaveListaAutorizacion.val().trim() == '') {
                countInvalidos++;
            }

            if (selectCurso.val() == '') {
                countInvalidos++;
            }

            if (inputRevision.val() == '') {
                countInvalidos++;
            }

            if (selectDepartamentoCaptura.val() == '') {
                countInvalidos++;
            }

            return countInvalidos;
        }

        function guardarListaAutorizacion() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevaListaAutorizacion();
                    break;
                case ESTATUS.EDITAR:
                    editarListaAutorizacion();
                    break;
            }
        }

        function nuevaListaAutorizacion() {
            if (validarGuardarListaAutorizacion() > 0) {
                AlertaGeneral('Aviso', 'Debe ingresar todos los datos')
            } else {
                let datos = getDatosListaAutorizacion();

                axios.post('GuardarListaAutorizacion', {
                    listaAutorizacion: datos.listaAutorizacion, listaRFC: datos.listaRFC, listaCentrosCosto: datos.listaCentrosCosto
                }).then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AlertaGeneral(`Alerta`, `Se guardó la información.`);
                        modalRegistro.modal('hide');
                        buscarListasAutorizacion();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function editarListaAutorizacion() {
            if (validarGuardarListaAutorizacion() > 0) {
                AlertaGeneral('Aviso', 'Debe ingresar todos los datos')
            } else {
                let datos = getDatosListaAutorizacion();

                axios.post('EditarListaAutorizacion', {
                    listaAutorizacion: datos.listaAutorizacion, listaRFC: datos.listaRFC, listaCentrosCosto: datos.listaCentrosCosto
                }).then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AlertaGeneral(`Alerta`, `Se guardó la información.`);
                        modalRegistro.modal('hide');
                        buscarListasAutorizacion();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function eliminarListaAutorizacion(listaAutorizacionID) {
            axios.post('EliminarListaAutorizacion', { listaAutorizacionID })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AlertaGeneral(`Alerta`, `Se eliminó la información.`);
                        buscarListasAutorizacion();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function getDatosListaAutorizacion() {
            let listaAutorizacion = {
                id: botonGuardar.data().id,
                claveLista: inputClaveListaAutorizacion.val(),
                cursoID: selectCurso.val(),
                revision: inputRevision.val(),
                jefeDepartamento: 0,
                gerenteProyecto: 0,
                coordinadorCSH: 0,
                secretarioCSH: 0,
                fechaCreacion: null,
                division: 0,
                estatus: true
            };

            let listaCentrosCosto = [];

            getValoresMultiplesCustom('#selectDepartamentoCaptura').forEach(x => {
                listaCentrosCosto.push({
                    id: 0,
                    cc: x.cc,
                    departamento: +x.departamento,
                    empresa: +x.empresa,
                    listaAutorizacionID: listaAutorizacion.id,
                    estatus: true
                });
            });

            let listaRFC = [];

            tablaRFC.find('tbody tr').each(function (index, row) {
                listaRFC.push({
                    id: 0,
                    rfc: $(row).find('.inputRFC').val().trim(),
                    razonSocial: $(row).find('.inputRazonSocial').val().trim(),
                    listaAutorizacionID: 0, //El valor se asigna en el back-end.
                    estatus: true
                });
            });

            return { listaAutorizacion, listaRFC, listaCentrosCosto };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        function getValoresMultiplesCustom(selector) {
            var _tempObj = $(selector + ' option:selected').map(function (a, item) {
                return { value: item.value, empresa: $(item).attr('empresa'), departamento: $(item).attr('departamento'), cc: $(item).attr('cc') };
            });
            var _tempArrObj = new Array();
            $.each(_tempObj, function (i, e) {
                _tempArrObj.push(e);
            });
            return _tempArrObj;
        }
    }
    $(document).ready(() => administrativo.seguridad.CrearPersonalAutorizado = new CrearPersonalAutorizado())
})();