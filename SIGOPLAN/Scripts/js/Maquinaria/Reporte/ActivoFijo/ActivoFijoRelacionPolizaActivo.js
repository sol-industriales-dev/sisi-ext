(() => {
    $.namespace('ActivoFijo.RelacionPolizaActivo');
    RelacionPolizaActivo = function () {
        const cboFiltroAnio = $('#cboFiltroAnio');
        const cboFiltroCC = $('#cboFiltroCC');
        const cboFiltroCuenta = $('#cboFiltroCuenta');
        const btnFiltrar = $('#btnFiltrar');
        const btnNuevo = $('#btnNuevo');
        const tblRelacionActivos = $('#tblRelacionActivos');

        const modalRelacionActivo = $('#modalRelacionActivo');
        const modalRelacionActivoTitulo = $('#modalRelacionActivoTitulo');
        const cboTipoActivo = $('#cboTipoActivo');
        const divEconomico = $('#divEconomico');
        const cboNumEconomico = $('#cboNumEconomico');
        const txtConcepto = $('#txtConcepto');
        const txtFechaMovimiento = $('#txtFechaMovimiento');
        const txtPorcentajeDep = $('#txtPorcentajeDep');
        const txtMesesDep = $('#txtMesesDep');
        const cboCC = $('#cboCC');
        const cboTipoPoliza = $('#cboTipoPoliza');
        const txtPoliza = $('#txtPoliza');
        const txtLinea = $('#txtLinea');
        const txtCuenta = $('#txtCuenta');
        const txtMonto = $('#txtMonto');
        const btnGuardar = $('#btnGuardar');

        btnFiltrar.on('click', function () {
            getActivos(cboFiltroAnio.val(), cboFiltroCC.val(), cboFiltroCuenta.val()).done(function (response) {
                if (response && response.success) {
                    AddRows(tblRelacionActivos, response.items);
                }
            });
        });

        btnNuevo.on('click', function () {
            limpiarTodosLosCampos();

            modalRelacionActivo.modal('show');
        });

        cboTipoActivo.on('change', function () {
            if (cboTipoActivo.val() == 'true') {
                divEconomico.show();
            } else {
                divEconomico.hide();
            }
        });

        txtMonto.on('change', function () {
            txtMonto.val(maskNumero(unmaskNumero($(this).val())));
        });

        btnGuardar.on('click', function () {
            if (validarCampos()) {
                guardarRelacionActivo(crearObjeto()).done(function (response) {
                    if (response && response.success) {
                        limpiarCampos();
                        swal('Confirmación!', 'Se guardó correctamente!', 'success');
                    }
                });
            } else {
                swal('Alerta!', 'Favor de llenar todos los campos obligatorios', 'warning');
            }
        });

        (function init() {
            initFechas();
            llenarFiltros();
            initTblRelacionActivos();
        })();

        function initFechas() {
            txtFechaMovimiento.datepicker().datepicker('setDate', new Date());
        }

        function llenarFiltros() {
            cboFiltroAnio.fillComboBox('GetAnios', null, '-- Seleccionar --', () => {
                cboFiltroAnio.select2();
            });

            cboFiltroCC.fillComboBox('GetCCs', null, '-- Seleccionar --', () => {
                cboFiltroCC.select2();
            });

            cboFiltroCuenta.fillComboBox('GetCuentasPeru', null, '-- Seleccionar --', () => {
                cboFiltroCuenta.select2();
            });

            cboNumEconomico.fillComboBox('GetEconomicosPeru', null, '-- Seleccionar --', () => {
                cboNumEconomico.select2({ width: '100%', dropdownParent: $(modalRelacionActivo) });
            });

            cboCC.fillComboBox('GetCCs', null, '-- Seleccionar --', () => {
                cboCC.select2({ width: '100%', dropdownParent: $(modalRelacionActivo) });
            });
        }

        function initTblRelacionActivos() {
            tblRelacionActivos.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                columns: [
                    {
                        data: null,
                        title: 'OPCIONES',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            let btnEliminar = '<button class="btn btn-danger btnEliminar btn-xs" title="Eliminar"><i class="fas fa-trash-alt"></i> Eliminar</button>';
                            //let btnActualizar = '<button class="btn btn-warning btnActualizar btn-xs" title="Actualizar"><i class="fas fa-pencil-alt"></i> Actualizar</button>';

                            //return btnEliminar + ' ' + btnActualizar;
                            return btnEliminar;
                        }
                    },
                    {
                        data: 'tipoActivo',
                        title: 'TIPO ACTIVO',
                        className: 'dt-center'
                    },
                    {
                        data: 'nombreActivo',
                        title: 'ACTIVO',
                        className: 'dt-body-left dt-head-center'
                    },
                    {
                        data: 'fechaInicioDep',
                        title: 'INICIO DEP',
                        className: 'dt-center'
                    },
                    {
                        data: 'porcentajeDep',
                        title: '% DEP',
                        className: 'dt-center'
                    },
                    {
                        data: 'mesesDep',
                        title: 'MESES DEP',
                        className: 'dt-center'
                    },
                    {
                        data: 'cc',
                        title: 'CC',
                        className: 'dt-head-center dt-body-left'
                    },
                    {
                        data: 'poliza',
                        title: 'POLIZA',
                        className: 'dt-center'
                    },
                    {
                        data: 'cuenta',
                        title: 'CUENTA',
                        className: 'dt-center'
                    },
                    {
                        data: 'monto',
                        title: 'MONTO',
                        className: 'dt-head-center dt-body-right',
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblRelacionActivos.on('click', '.btnEliminar', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblRelacionActivos.DataTable().row(row).data();

                        swal({
                            title: 'Alerta!',
                            text: `Se eliminara el registro ${rowData.nombreActivo}. ¿Desea continuar?`,
                            icon: 'warning',
                            buttons: true,
                            dangerMode: true,
                            buttons: ['Cerrar', 'Eliminar']
                        })
                            .then((aceptar) => {
                                if (aceptar) {
                                    eliminarRelacionActivo(rowData.id).done(function (response) {
                                        if (response && response.success) {
                                            tblRelacionActivos.DataTable().row(row).remove().draw();

                                            swal('Confirmación', 'Se eliminó el registro correctamente', 'success');
                                        }
                                    });
                                }
                            });
                    });
                },
                columnDefs: [
                ]
            });
        }

        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function validarCampos() {
            let validacion = true;

            if (cboTipoActivo.val() == 'true') {
                if (!cboNumEconomico.val()) {
                    validacion = false;
                }
            }

            if (!txtConcepto.val()) {
                validacion = false;
            }

            if (!txtFechaMovimiento.val()) {
                validacion = false;
            }

            if (!txtPorcentajeDep.val()) {
                validacion = false;
            }

            if (!txtMesesDep.val()) {
                validacion = false;
            }

            if (!cboCC.val()) {
                validacion = false;
            }

            if (!cboTipoPoliza.val()) {
                validacion = false;
            }

            if (!txtPoliza.val()) {
                validacion = false;
            }

            if (!txtLinea.val()) {
                validacion = false;
            }

            if (!txtCuenta.val()) {
                validacion = false;
            }

            if (!txtMonto.val()) {
                validacion = false;
            }

            return validacion;
        }

        function limpiarCampos() {
            txtPoliza.val('');
            txtLinea.val('');
            txtConcepto.val('');
            txtMonto.val('');
        }

        function limpiarTodosLosCampos() {
            txtPorcentajeDep.val('');
            txtMesesDep.val('');
            txtPoliza.val('');
            txtLinea.val('');
            txtCuenta.val('');
            txtConcepto.val('');
            txtMonto.val('');
        }

        function crearObjeto() {
            let obj = {
                esMaquina: cboTipoActivo.val() == 'true',
                idActivo: cboTipoActivo.val() == 'true' ? cboNumEconomico.val() : 0,
                fechaMovimiento: txtFechaMovimiento.val(),
                fechaInicioDep: txtFechaMovimiento.val(),
                porcentajeDep: txtPorcentajeDep.val(),
                mesesDep: txtMesesDep.val(),
                cc: cboCC.val(),
                polizaPoliza: txtPoliza.val(),
                tpPoliza: cboTipoPoliza.val(),
                lineaPoliza: txtLinea.val(),
                ctaPoliza: txtCuenta.val(),
                conceptoPoliza: txtConcepto.val(),
                montoPoliza: unmaskNumero(txtMonto.val()),
                fechaPoliza: txtFechaMovimiento.val(),
                concepto: txtConcepto.val()
            }

            return obj;
        }

        function eliminarRelacionActivo(id) {
            return $.post('EliminarRelacionActivo',
                {
                    id
                }).then(response => {
                    if (response.success) {
                        return response;
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                });
        }

        function getRelacionActivo(id) {
            return $.get('getRelacionActivo',
                {
                    id
                }).then(response => {
                    if (response.success) {
                        return response;
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                });
        }

        function guardarRelacionActivo(obj) {
            return $.post('GuardarRelacionActivo', {
                obj
            }).then(response => {
                if (response.success) {
                    return response;
                } else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }

        function getActivos(anio, cc, cuenta) {
            return $.get('GetActivos',
                {
                    anio,
                    cc,
                    cuenta
                }).then(response => {
                    if (response.success) {
                        return response;
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                });
        }
    }
    $(document).ready(() => ActivoFijo.RelacionPolizaActivo = new RelacionPolizaActivo())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();