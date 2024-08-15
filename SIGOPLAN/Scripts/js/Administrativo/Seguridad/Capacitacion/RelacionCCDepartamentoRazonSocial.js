(() => {
    $.namespace('Administrativo.Capacitacion.RelacionCCDepartamentoRazonSocial');
    RelacionCCDepartamentoRazonSocial = function () {
        //#region Selectores
        const tablaRelacion = $('#tablaRelacion');
        const botonAgregar = $('#botonAgregar');
        const modalRelacion = $('#modalRelacion');
        const selectDepartamentoConstruplan = $('#selectDepartamentoConstruplan');
        const selectDepartamentoArrendadora = $('#selectDepartamentoArrendadora');
        const selectRazonSocial = $('#selectRazonSocial');
        const botonGuardar = $('#botonGuardar');
        const modalRelacionEditar = $('#modalRelacionEditar');
        const selectDepartamentoEditar = $('#selectDepartamentoEditar');
        const selectRazonSocialEditar = $('#selectRazonSocialEditar');
        const botonGuardarEditar = $('#botonGuardarEditar');
        //#endregion

        let dtRelacion;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            $('.select2').select2();
            initTablaRelacion();
            agregarListeners();
            cargarRelaciones();

            selectRazonSocial.fillCombo('/Administrativo/Capacitacion/GetRazonSocialCombo', null, false, null);
            llenarComboDepartamentos();
            selectRazonSocialEditar.fillCombo('/Administrativo/Capacitacion/GetRazonSocialCombo', null, false, null);
            llenarComboDepartamentosEditar();
        })();

        function initTablaRelacion() {
            dtRelacion = tablaRelacion.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '50vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaRelacion.on('click', '.btn-editar', function () {
                        let rowData = dtRelacion.row($(this).closest('tr')).data();

                        limpiarModalEditar();
                        llenarCamposModalEditar(rowData);
                        botonGuardarEditar.data().estatus = ESTATUS.EDITAR;
                        botonGuardarEditar.data().id = rowData.id;
                        modalRelacionEditar.modal('show');
                    });

                    tablaRelacion.on('click', '.btn-eliminar', function () {
                        let rowData = dtRelacion.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el registro?`,
                            () => eliminarRelacion(rowData.id))
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'empresaDesc', title: 'Empresa' },
                    { data: 'ccDesc', title: 'Centro de Costo' },
                    { data: 'departamentoDesc', title: 'Departamento' },
                    { data: 'razonSocialDesc', title: 'Razón Social' },
                    {
                        render: function (data, type, row, meta) {
                            return `
                            <button title="Editar" class="btn-editar btn btn-warning">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            &nbsp;
                            <button title="Eliminar" class="btn-eliminar btn btn-danger">
                                <i class="fas fa-trash"></i>
                            </button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '10%', targets: 4 }
                ]
            });
        }

        function cargarRelaciones() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Capacitacion/GetRelacionesCCDepartamentoRazonSocial')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaRelacion, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function agregarListeners() {
            botonAgregar.click(() => {
                limpiarModal();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                modalRelacion.modal('show');
            });
            botonGuardar.click(nuevaRelacion);
            botonGuardarEditar.click(editarRelacion);
        }

        function limpiarModal() {
            llenarComboDepartamentos();
            selectRazonSocial.fillCombo('/Administrativo/Capacitacion/GetRazonSocialCombo', null, false, null);
        }

        function limpiarModalEditar() {
            llenarComboDepartamentosEditar();
            selectRazonSocialEditar.fillCombo('/Administrativo/Capacitacion/GetRazonSocialCombo', null, false, null);
        }

        function llenarCamposModalEditar(data) {
            selectDepartamentoEditar.find(`option[value="${data.departamento}"][empresa="${data.empresa}"]`).attr('selected', true);
            selectRazonSocialEditar.val(data.razonSocialID);
        }

        function nuevaRelacion() {
            let relaciones = getInformacionRelaciones();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Capacitacion/GuardarNuevaRelacionCCDepartamentoRazonSocial', { relaciones })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalRelacion.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarRelaciones();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function editarRelacion() {
            let relacion = getInformacionRelacionEditar();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Capacitacion/EditarRelacionCCDepartamentoRazonSocial', { relacion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalRelacionEditar.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarRelaciones();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function eliminarRelacion(id) {
            let relacion = { id };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Capacitacion/EliminarRelacionCCDepartamentoRazonSocial', { relacion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarRelaciones();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getInformacionRelaciones() {
            let relaciones = [];
            let departamentosConstruplan = getValoresMultiples('#selectDepartamentoConstruplan');
            let departamentosArrendadora = getValoresMultiples('#selectDepartamentoArrendadora');

            departamentosConstruplan.forEach(departamento => {
                relaciones.push({
                    id: botonGuardar.data().id,
                    departamento: departamento,
                    razonSocialID: selectRazonSocial.val(),
                    empresa: 1,
                    estatus: true
                });
            });

            departamentosArrendadora.forEach(departamento => {
                relaciones.push({
                    id: botonGuardar.data().id,
                    departamento: departamento,
                    razonSocialID: selectRazonSocial.val(),
                    empresa: 2,
                    estatus: true
                });
            });

            return relaciones;
        }

        function getInformacionRelacionEditar() {
            let empresaSeleccionada = +(selectDepartamentoEditar.find('option:selected').attr('empresa'));

            return {
                id: botonGuardarEditar.data().id,
                departamento: selectDepartamentoEditar.val(),
                razonSocialID: selectRazonSocialEditar.val(),
                empresa: empresaSeleccionada,
                estatus: true
            };
        }

        function llenarComboDepartamentos() {
            selectDepartamentoConstruplan.empty();
            selectDepartamentoArrendadora.empty();
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Capacitacion/GetDepartamentosCombo')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        //#region Construplan
                        let itemsConstruplan = response.items.filter(function (x) { return x.Prefijo == "CONSTRUPLAN" });
                        selectDepartamentoConstruplan.append('<option value="Todos">Todos</option>');

                        itemsConstruplan.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" empresa="${y.Prefijo == 'CONSTRUPLAN' ? 1 : y.Prefijo == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                            });
                            selectDepartamentoConstruplan.append(groupOption);
                        });

                        convertToMultiselect('#selectDepartamentoConstruplan');
                        //#endregion

                        //#region Arrendadora
                        let itemsArrendadora = response.items.filter(function (x) { return x.Prefijo == "ARRENDADORA" });
                        selectDepartamentoArrendadora.append('<option value="Todos">Todos</option>');

                        itemsArrendadora.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" empresa="${y.Prefijo == 'CONSTRUPLAN' ? 1 : y.Prefijo == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                            });
                            selectDepartamentoArrendadora.append(groupOption);
                        });

                        convertToMultiselect('#selectDepartamentoArrendadora');
                        //#endregion
                    } else {
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function llenarComboDepartamentosEditar() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Capacitacion/GetDepartamentosCombo')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        selectDepartamentoEditar.append('<option value="Todos">Todos</option>');

                        response.items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" empresa="${y.Prefijo == 'CONSTRUPLAN' ? 1 : y.Prefijo == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                            });
                            selectDepartamentoEditar.append(groupOption);
                        });
                    } else {
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
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
    $(document).ready(() => Administrativo.Capacitacion.RelacionCCDepartamentoRazonSocial = new RelacionCCDepartamentoRazonSocial())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();