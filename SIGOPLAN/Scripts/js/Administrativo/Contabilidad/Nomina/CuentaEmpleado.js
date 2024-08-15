(() => {
    $.namespace('Administrativo.Contabilidad.CuentaEmpleado');
     CuentaEmpleado = function () {

        let _cc;
        let _tipoCuenta;
        let _renglonModificando;

        //#region selectores
        const panelGeneral = $('#panelGeneral');
        const cboxTipoCuentaFiltro = $('#cboxTipoCuentaFiltro');
        const cboxCCFiltro = $('#cboxCCFiltro');
        const btnAplicarFiltro = $('#btnAplicarFiltro');
        const btnAgregarNuevo = $('#btnAgregarNuevo');
        const tblCuentaEmpleado = $('#tblCuentaEmpleado');
        const btnValidarCuentaEmpleado = $('#btnValidarCuentaEmpleado');

        const modalAgregarEmpleado = $('#modalAgregarEmpleado');
        const inputNumero = $('#inputNumero');
        const inputNombre = $('#inputNombre');
        const inputApellidoPaterno = $('#inputApellidoPaterno');
        const inputApellidoMaterno = $('#inputApellidoMaterno');
        const cboxTipoCuenta = $('#cboxTipoCuenta');
        const cboxCuenta = $('#cboxCuenta');
        const cboxCC = $('#cboxCC');
        const btnGuardarEmpleado = $('#btnGuardarEmpleado');
        //#endregion

        //#region eventos
        btnAplicarFiltro.on('click', function() {
            $('#chkALLTblCuentaEmpleado').prop('checked', false);

            _tipoCuenta = cboxTipoCuentaFiltro.val();
            _cc = cboxCCFiltro.val() ?? null;

            catalogoCuentaEmpleado(_tipoCuenta, _cc);
        });

        btnAgregarNuevo.on('click', function() {
            limpiarRegistro();
            modalAgregarEmpleado.modal('show');
        });

        btnGuardarEmpleado.on('click', function() {
            const idEmpleado = $(this).data('id');

            if (validarCampos()) {
                if (idEmpleado > 0) {
                    modificarEmpleado(crearObjetoEmpleado(idEmpleado)).done(function(response) {
                        if (response && response.success) {
                            tblCuentaEmpleado.DataTable().row(_renglonModificando).data(response.items);

                            limpiarRegistro();
                            modalAgregarEmpleado.modal('hide');
                            swal('Confirmación', 'Se modificó correctamente el empleado', 'success');
                        }
                    });
                }
                else {
                    registrarEmpleado(crearObjetoEmpleado(0));
                }
            }
            else {
                swal('Alerta!', 'Favor de llenar todos los campos', 'warning');
            }
        });

        btnValidarCuentaEmpleado.on('click', function() {
            let ids = new Array();

            tblCuentaEmpleado.DataTable().rows().every(function(rowIdx, tableLoop, rowLoop) {
                const check = $(this.node()).find('.checkValidadoEmpleado').prop('checked');
                if (this.data().validada != check) {
                    this.data().validada = check;

                    const objCuentaEmpleado = {
                        id: this.data().id,
                        check: check
                    }

                    ids.push(objCuentaEmpleado);
                }
            });

            validarCuentaEmpleado(ids);
        });
        //#endregion

        function selectAutocomplete(event, ui) {
            $(this).text(ui.item.label);
            $(this).attr('data-index', ui.item.id);
        }

        function limpiarRegistro() {
            inputNumero.val('');
            inputNombre.val('');
            inputApellidoPaterno.val('');
            inputApellidoMaterno.val('');
            cboxTipoCuenta.val('');
            cboxTipoCuenta.trigger('change');
            cboxCuenta.val('');
            cboxCuenta.text('');
            cboxCuenta.attr('data-index', '');
            cboxCuenta.trigger('change');
            cboxCC.val('');
            cboxCC.trigger('change');

            btnGuardarEmpleado.removeData();
        }

        function llenarRegistro(empleado) {
            btnGuardarEmpleado.data('id', empleado.id);

            inputNumero.val(empleado.numero);
            inputNombre.val(empleado.nombre);
            inputApellidoPaterno.val(empleado.apellidoPaterno);
            inputApellidoMaterno.val(empleado.apellidoMaterno);
            cboxTipoCuenta.val(empleado.tipoCuentaId);
            cboxTipoCuenta.trigger('change');
            cboxCuenta.text(empleado.cuentaDescripcion);
            cboxCuenta.attr('data-index', empleado.cuenta);
            cboxCuenta.val(empleado.cuentaDescripcion);
            cboxCC.val(empleado.cc);
            cboxCC.trigger('change');
        }

        function crearObjetoEmpleado(id) {
            const cuenta = cboxCuenta.attr('data-index').split('-');

            const objEmpleado = {
                id: id,
                numero: inputNumero.val().trim(),
                nombre: inputNombre.val().trim().toUpperCase(),
                apellidoPaterno: inputApellidoPaterno.val().trim().toUpperCase(),
                apellidoMaterno: inputApellidoMaterno.val().trim().toUpperCase(),
                cuentaId: cboxTipoCuenta.val(),
                cta: cuenta[0],
                scta: cuenta[1],
                sscta: cuenta[2],
                digito: cuenta[3],
                cc: cboxCC.val(),
                ccDescripcion: cboxCC.find('option:selected').data('prefijo')
            }

            return objEmpleado;
        }

        function validarCampos() {
            let validado = true;

            if (!inputNumero.val()) { validado = false; }
            if (!inputNombre.val()) { validado = false; }
            if (!inputApellidoPaterno.val()) { validado = false; }
            if (!cboxTipoCuenta.val()) { validado = false; }
            if (!cboxCuenta.attr('data-index')) { validado = false; }
            if (!cboxCC.val()) { validado = false; }

            return validado;
        }

        //#region tablas
        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function addRow(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.row.add(datos).draw();
        }

        function initTbls() {
            tblCuentaEmpleado.DataTable({
                order: [[0, 'asc']],
                ordering: true,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: false,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],

                columns: [
                    { data: 'numero', title: 'Clave empleado', width: '50px' },
                    { data: 'nombreCompleto', title: 'Nombre' },
                    { data: 'tipoCuentaDescripcion', title: 'Tipo cuenta' },
                    { data: 'cuenta', title: 'Cuenta', width: '250px' },
                    { data: 'ccDescripcion', title: 'CC' },
                    { data: 'validada', title: 'Validar<br /><input type="checkbox" id="chkALLTblCuentaEmpleado" />' },
                    { data: null, title: 'Opciones' }
                ],

                columnDefs: [
                    {
                        targets: [0, 2, 3, 4, 5],
                        className: 'text-center'
                    },
                    {
                        targets: [5],
                        render: function(data, type, row) {
                            checkValidadoEmpleado = `<input type="checkbox" class="checkValidadoEmpleado" ${data ? 'checked' : ''} />`;

                            return checkValidadoEmpleado;
                        }
                    },
                    {
                        targets: [6],
                        render: function(data, type, row) {
                            const btnModificar = '<button class="btn btn-info btnModificarEmpleado" title="Modificar relación"><i class="far fa-edit"></i></button>';
                            const btnEliminar = '<button class="btn btn-danger btnEliminarEmpleado" title="Eliminar relación"><i class="far fa-trash-alt"></i></button>';

                            return btnModificar + ' ' + btnEliminar;
                        }
                    }
                ],
                
                initComplete: function(settings, json) {
                    tblCuentaEmpleado.on('click', '.btnEliminarEmpleado', function() {
                        const row = $(this).closest('tr');
                        const rowData = tblCuentaEmpleado.DataTable().row($(this).closest('tr')).data();

                        swal({
                            title: 'Alerta!',
                            text: 'Se eliminara la relación cuenta usuario, ¿Desea continua?',
                            icon: 'warning',
                            buttons: true,
                            buttons: ['Cancelar', 'Eliminar']
                        })
                        .then((aceptar) => {
                            if (aceptar) {
                                eliminarEmpleado(rowData.id).done(function(response) {
                                    if (response && response.success) {
                                        tblCuentaEmpleado.DataTable().row(row).remove().draw();
                                        swal('Confirmación', 'Se eliminó correctamente el empleado', 'success');
                                    }
                                });
                            }
                        });
                    });

                    tblCuentaEmpleado.on('click', '.btnModificarEmpleado', function() {
                        _renglonModificando = $(this).closest('tr');
                        const rowData = tblCuentaEmpleado.DataTable().row($(this).closest('tr')).data();

                        limpiarRegistro();
                        llenarRegistro(rowData);
                        modalAgregarEmpleado.modal('show');
                    });

                    $(document).on('click', '#chkALLTblCuentaEmpleado', function(event) {
                        const estatusCheck = $(this).prop('checked');

                        tblCuentaEmpleado.DataTable().rows({ page: 'current' }).every(function(rowIdx, tableLoop, rowLoop){
                            $(this.node()).find('.checkValidadoEmpleado').prop('checked', estatusCheck);
                        });
                    });
                },

                createdRow: function(row, data, dataIndex) {},
                drawCallback: function(settings) {},
                headerCallback: function(thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },

                footerCallback: function(tfoot, data, start, end, display) {}
            });
        }
        //#endregion

        //#region servidor
        function llenarCombos() {
            cboxTipoCuentaFiltro.fillCombo('/Nomina/GetTipoCuenta', null, false, null);

            cboxCCFiltro.fillCombo('/Nomina/GetCCs', null, false, null, () => {
                cboxCCFiltro.select2();
            })

            cboxTipoCuentaFiltro.find('option').each(function(value, index, array){
                $(this).clone().appendTo(cboxTipoCuenta);
            });

            cboxCuenta.getAutocomplete(selectAutocomplete, null, '/Nomina/GetCuenta');

            cboxCCFiltro.find('option').each(function(value, index, array){
                $(this).clone().appendTo(cboxCC);
            });
            cboxCC.select2({dropdownParent: $(modalAgregarEmpleado)});
        }

        function catalogoCuentaEmpleado(tipoCuentaId, cc) {
            $.get('/Nomina/CatalogoCuentaEmpleado',
            {
                tipoCuentaId,
                cc
            }).then(response => {
                if (response.success) {
                    addRows(tblCuentaEmpleado, response.items);
                } else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }

        function registrarEmpleado(empleado) {
            $.post('/Nomina/RegistrarEmpleado',
            {
                empleado
            }).then(response => {
                if (response.success) {
                    limpiarRegistro();

                    const cuentaEmpleado = response.items;

                    if (cuentaEmpleado.cc == _cc || _cc == undefined || _cc == null || _cc == "") {
                        if (cuentaEmpleado.tipoCuentaId == _tipoCuenta || _tipoCuenta == undefined || _tipoCuenta == null || _tipoCuenta == "") {
                            addRow(tblCuentaEmpleado, cuentaEmpleado);
                        }
                    }

                    swal('Confirmación', 'Se registró correctamente el empleado', 'success');
                } else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }

        function eliminarEmpleado(id) {
            return $.post('/Nomina/EliminarEmpleado',
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

        function modificarEmpleado(empleado){
            return $.post('/Nomina/ModificarEmpleado',
            {
                empleado
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

        function validarCuentaEmpleado(ids) {
            $.post('/Nomina/ValidarCuentaEmpleado',
            {
                ids
            }).then(response => {
                if (response.success) {
                    swal('Confirmación', 'Se guardó correctamente la validación', 'success');
                } else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }
        //#endregion

        (function init() {
            initTbls();
            llenarCombos();
        })();
    }
    $(document).ready(() => Administrativo.Contabilidad.CuentaEmpleado = new CuentaEmpleado())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();