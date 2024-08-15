(() => {
    $.namespace('Administrativo.Nomina.Descuentos');
    Descuentos = function () {
        //#region Selectores
        const tablaDescuentos = $('#tablaDescuentos');
        const botonExcel = $('#botonExcel');
        const botonAgregar = $('#botonAgregar');
        const modalDescuento = $('#modalDescuento');
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const inputNombreEmpleado = $('#inputNombreEmpleado');
        const selectTipoDescuento = $('#selectTipoDescuento');
        const inputAnio = $('#inputAnio');
        const selectTipoNomina = $('#selectTipoNomina');
        const selectPeriodoInicial = $('#selectPeriodoInicial');
        const selectPeriodoFinal = $('#selectPeriodoFinal');
        const inputMonto = $('#inputMonto');
        const botonGuardar = $('#botonGuardar');
        const modalCargarExcel = $('#modalCargarExcel');
        const inputFileExcel = $('#inputFileExcel');
        const botonGuardarExcel = $('#botonGuardarExcel');
        //#endregion

        let dtDescuentos;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            initTablaDescuentos();
            agregarListeners();
            cargarDescuentos();

            selectPeriodoInicial.fillComboGroup('/Administrativo/Nomina/GetCbotPeriodoNomina', { tipoNomina: selectTipoNomina.val() }, false, undefined, null);
            selectPeriodoFinal.fillComboGroup('/Administrativo/Nomina/GetCbotPeriodoNomina', { tipoNomina: selectTipoNomina.val() }, false, undefined, null);
            selectTipoDescuento.fillCombo('/Administrativo/Nomina/GetTipoDescuentoPrenominaEnumCombo', null, false, null);
            selectTipoNomina.fillCombo('/Administrativo/Nomina/GetTipoNominaPropuestaEnumCombo', null, false, null);
        })();

        modalDescuento.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        inputMonto.on('focus', function () {
            $(this).select();
        });

        inputMonto.on('change', function () {
            $(this).val(maskNumero6DCompras($(this).val()));
        });

        inputClaveEmpleado.on('change', function () {
            let clave_empleado = +$(this).val();

            if (clave_empleado > 0) {
                axios.post('/Administrativo/Nomina/ObtenerEmpleadoPorClave', { clave_empleado })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            let empleado = response.data.data;

                            if (empleado != null) {
                                inputNombreEmpleado.val(`${empleado.nombre} ${empleado.ape_paterno} ${empleado.ape_materno}`);
                            } else {
                                Alert2Info('No se encuentra la información del empleado.');
                            }
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        });

        selectTipoNomina.on('change', function () {
            selectPeriodoInicial.fillComboGroup('/Administrativo/Nomina/GetCbotPeriodoNomina', { tipoNomina: selectTipoNomina.val() }, false, undefined, null);
            selectPeriodoFinal.fillComboGroup('/Administrativo/Nomina/GetCbotPeriodoNomina', { tipoNomina: selectTipoNomina.val() }, false, undefined, null);
        });

        botonGuardarExcel.on('click', function () {
            try {
                var request = new XMLHttpRequest();

                $.blockUI({ message: 'Procesando...', baseZ: 2000 });

                request.open("POST", "/Administrativo/Nomina/CargarExcelDescuentos");
                request.send(formData());

                request.onload = function (response) {
                    if (request.status == 200) {
                        $.unblockUI();

                        let respuesta = JSON.parse(request.response);

                        if (respuesta.success) {
                            modalCargarExcel.modal('hide');
                            cargarDescuentos();
                            Alert2Exito('Se ha guardado la información.');
                        } else {
                            AlertaGeneral(`Alerta`, `No se ha guardado la información. ${respuesta.message}`);
                        }
                    } else {
                        $.unblockUI();
                        AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                    }
                };
            } catch {
                $.unblockUI();
            }
        });

        function formData() {
            let formData = new FormData();

            $.each(document.getElementById("inputFileExcel").files, function (i, file) {
                formData.append("files[]", file);
            });

            return formData;
        }

        function initTablaDescuentos() {
            dtDescuentos = tablaDescuentos.DataTable({
                retrieve: true,
                // paging: false,
                // searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaDescuentos.on('click', '.btn-editar', function () {
                        let rowData = dtDescuentos.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        modalDescuento.modal('show');
                    });

                    tablaDescuentos.on('click', '.btn-eliminar', function () {
                        let rowData = dtDescuentos.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el descuento del empleado "${rowData.nombreEmpleado}"?`,
                            () => eliminarDescuento(rowData.id))
                    });
                },
                columns: [
                    { data: 'empleadoCve', title: '# Empleado' },
                    { data: 'nombreEmpleado', title: 'Nombre' },
                    { data: 'tipoDescuentoDesc', title: 'Tipo Descuento' },
                    { data: 'anio', title: 'Año' },
                    { data: 'tipoNominaDesc', title: 'Tipo Nómina' },
                    { data: 'periodoInicial', title: 'Periodo Inicial' },
                    { data: 'periodoFinal', title: 'Periodo Final' },
                    {
                        data: 'monto', title: 'Monto', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return maskNumero6DCompras(data);
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        render: function (data, type, row, meta) {
                            return `
                            <button title="Editar" class="btn-editar btn btn-xs btn-warning">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            &nbsp;
                            <button title="Eliminar" class="btn-eliminar btn btn-xs btn-danger">
                                <i class="fas fa-trash"></i>
                            </button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '10%', targets: [8] }
                ]
            });
        }

        function cargarDescuentos() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Nomina/GetDescuentos')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaDescuentos, response.data);
                        $.fn.dataTable.tables({ api: true }).columns.adjust();
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
                modalDescuento.modal('show');
            });
            botonGuardar.click(guardarDescuento);
            botonExcel.click(() => { modalCargarExcel.modal('show') });
        }

        function limpiarModal() {
            inputClaveEmpleado.val('');
            inputNombreEmpleado.val('');
            selectTipoDescuento.val('');
            inputAnio.val('');
            selectTipoNomina.val('');
            selectPeriodoInicial.val('');
            selectPeriodoFinal.val('');
            inputMonto.val('');
        }

        function llenarCamposModal(data) {
            inputClaveEmpleado.val(data.empleadoCve);
            inputNombreEmpleado.val(data.nombreEmpleado);
            selectTipoDescuento.val(data.tipoDescuento);
            inputAnio.val(data.anio);
            selectTipoNomina.val(data.tipoNomina);
            selectTipoNomina.change();

            switch (data.tipoNomina) {
                case 1:
                    selectPeriodoInicial.find(`optgroup:eq(0) option[value="${data.periodoInicial}"]`).prop('selected', true);
                    selectPeriodoFinal.find(`optgroup:eq(0) option[value="${data.periodoFinal}"]`).prop('selected', true);
                    break;
                case 4:
                    selectPeriodoInicial.find(`optgroup:eq(1) option[value="${data.periodoInicial}"]`).prop('selected', true);
                    selectPeriodoFinal.find(`optgroup:eq(1) option[value="${data.periodoFinal}"]`).prop('selected', true);
                    break;
            }

            inputMonto.val(maskNumero6DCompras(data.monto));
        }

        function guardarDescuento() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevaDescuento();
                    break;
                case ESTATUS.EDITAR:
                    editarDescuento();
                    break;
            }
        }

        function nuevaDescuento() {
            let descuento = getInformacionDescuento();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Nomina/GuardarNuevoDescuento', { descuento })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalDescuento.modal('hide');
                        Alert2Exito('Se ha guardado la información.');
                        cargarDescuentos();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function editarDescuento() {
            let descuento = getInformacionDescuento();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Nomina/EditarDescuento', { descuento })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalDescuento.modal('hide');
                        Alert2Exito('Se ha guardado la información.');
                        cargarDescuentos();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function eliminarDescuento(id) {
            let descuento = { id };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Nomina/EliminarDescuento', { descuento })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        Alert2Exito('Se ha quitado la información.');
                        cargarDescuentos();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getInformacionDescuento() {
            return {
                id: botonGuardar.data().id,
                empleadoCve: inputClaveEmpleado.val(),
                tipoDescuento: selectTipoDescuento.val(),
                anio: inputAnio.val(),
                tipoNomina: selectTipoNomina.val(),
                periodoInicial: selectPeriodoInicial.val(),
                periodoFinal: selectPeriodoFinal.val(),
                monto: unmaskNumero6DCompras(inputMonto.val()),
                estatus: true
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Nomina.Descuentos = new Descuentos())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();