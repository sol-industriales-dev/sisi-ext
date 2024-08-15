(() => {
    $.namespace('Administrativo.Requerimientos.RelacionEmpleadoAreaCC');

    RelacionEmpleadoAreaCC = function () {
        //#region Selectores
        const tablaRelacion = $('#tablaRelacion');
        const botonAgregar = $('#botonAgregar');
        const modalRelacion = $('#modalRelacion');
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const inputNombre = $('#inputNombre');
        const inputApellidoPaterno = $('#inputApellidoPaterno');
        const inputApellidoMaterno = $('#inputApellidoMaterno');
        const selectArea = $('#selectArea');
        const selectCentroCosto = $('#selectCentroCosto');
        const botonGuardar = $('#botonGuardar');
        const chkEsContratista = $('#chkEsContratista');
        const lblContratistas = $('#lblContratistas');
        //#endregion

        let dtRelacion;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            $('.select2').select2();
            initTablaRelacion();
            agregarListeners();
            cargarRelaciones();
            fncValidarAccesoContratista();

            selectArea.fillCombo('/Administrativo/Requerimientos/GetAreaCombo', null, false, null);
            selectCentroCosto.fillComboSeguridad(false);

            chkEsContratista.change(function (e) {
                if (inputClaveEmpleado.val() != "") { inputClaveEmpleado.trigger("change"); }
            });
        })();

        function fncValidarAccesoContratista() {
            axios.post("/Administrativo/IndicadoresSeguridad/ValidarAccesoContratista").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    botonGuardar.css("display", "none");
                    chkEsContratista.css("display", "none");
                    lblContratistas.css("display", "none");
                    botonGuardar.attr("data-esContratista", true);
                } else {
                    botonGuardar.attr("data-esContratista", false);
                }
            }).catch(error => Alert2Error(error.message));
        }

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

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;

                        let esContratista = rowData.esContratista;
                        if (esContratista) {
                            chkEsContratista.prop("checked", true);
                        } else {
                            chkEsContratista.prop("checked", false);
                        }

                        modalRelacion.modal('show');
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
                    { data: 'nombreCompleto', title: 'Empleado' },
                    { data: 'areaDesc', title: 'Área' },
                    { data: 'ccDesc', title: 'Centro de Costo' },
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
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarRelaciones() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GetRelacionesEmpleadoAreaCC')
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
            inputClaveEmpleado.click(function (e) {
                $(this).select();
            });
            botonAgregar.click(() => {
                limpiarModal();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                modalRelacion.modal('show');
            });
            botonGuardar.click(guardarRelacion);
            inputClaveEmpleado.change(cargarEmpleadoPorClave);
        }

        function limpiarModal() {
            inputClaveEmpleado.val('');
            inputNombre.val('');
            inputApellidoPaterno.val('');
            inputApellidoMaterno.val('');
            selectArea.val('');
            selectCentroCosto.val('');
            selectCentroCosto.select2().trigger('change');
        }

        function llenarCamposModal(data) {
            inputClaveEmpleado.val(data.empleado);
            inputNombre.val(data.nombre);
            inputApellidoPaterno.val(data.apellidoPaterno);
            inputApellidoMaterno.val(data.apellidoMaterno);
            selectArea.val(data.area);
            selectCentroCosto.val(data.cc);
            selectCentroCosto.select2().trigger('change');
        }

        function guardarRelacion() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevaRelacion();
                    break;
                case ESTATUS.EDITAR:
                    editarRelacion();
                    break;
            }
        }

        function nuevaRelacion() {
            let relacion = getInformacionRelacion();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GuardarNuevaRelacion', { relacion: relacion, esContratista: chkEsContratista.prop("checked") })
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
            let relacion = getInformacionRelacion();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/EditarRelacion', { relacion: relacion, esContratista: chkEsContratista.prop("checked") })
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

        function eliminarRelacion(id) {
            let relacion = { id };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/EliminarRelacion', { relacion })
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

        function getInformacionRelacion() {
            let idEmpresa = $(selectCentroCosto).getEmpresa();
            let strAgrupacion = $(selectCentroCosto).getAgrupador();
            let idAgrupacion;
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }

            return {
                id: botonGuardar.data().id,
                empleado: +(inputClaveEmpleado.val()),
                area: +(selectArea.val()),
                cc: selectCentroCosto.val(),
                idEmpresa: idEmpresa,
                idAgrupacion: idAgrupacion,
                estatus: true
            };
        }

        function cargarEmpleadoPorClave() {
            let claveEmpleado = +(inputClaveEmpleado.val());

            if (claveEmpleado > 0) {

                let attrEsContratista = botonGuardar.attr("data-esContratista");
                // let idEmpresa = selectCCRegistro.val() != "" ? selectCCRegistro.val() : 0;
                let idEmpresa = 0;
                let esContratista = false;
                if (attrEsContratista == "true") {
                    esContratista = true;
                } else {
                    if (chkEsContratista.prop('checked')) {
                        esContratista = true;
                    } else {
                        esContratista = false;
                    }
                }

                // if (esContratista == true && idEmpresa == 0) {
                //     Alert2Warning("Es necesario seleccionar una empresa");
                // } else {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Administrativo/Evaluacion/GetEmpleadoPorClave', { claveEmpleado: claveEmpleado, esContratista: esContratista, idEmpresaContratista: idEmpresa })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            inputNombre.val(response.data.nombre);
                            inputApellidoPaterno.val(response.data.apellidoPaterno);
                            inputApellidoMaterno.val(response.data.apellidoMaterno);
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    });
                // }
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }

    $(document).ready(() => Administrativo.Requerimientos.RelacionEmpleadoAreaCC = new RelacionEmpleadoAreaCC())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();