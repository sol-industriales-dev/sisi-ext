(() => {
    $.namespace('RecursosHumanos.Reclutamientos');
    Reclutamientos = function () {
        //#region Selectores
        const cboFiltroCC = $('#cboFiltroCC');
        const cboFiltroEstatus = $('#cboFiltroEstatus');
        const btnFiltrar = $('#btnFiltrar');
        const btnAutorizarMasivo = $('#btnAutorizarMasivo')
        const btnNuevo = $('#btnNuevo');
        const tblRequisiciones = $('#tblRequisiciones');

        const modalRequisicion = $('#modalRequisicion');
        const modalRequisicionTitulo = $('#modalRequisicionTitulo');
        const inputId = $('#inputId');
        const inputEstatus = $('#inputEstatus');
        const cboCC = $('#cboCC');
        const inputPlantilla = $('#inputPlantilla');
        const btnPlantilla = $('#btnPlantilla');
        const inputVacantes = $('#inputVacantes');
        const inputPuesto = $('#inputPuesto');
        const inputPuestoDescripcion = $('#inputPuestoDescripcion');
        const inputJefe = $('#inputJefe');
        const inputSolicita = $('#inputSolicita');
        const inputAutoriza = $('#inputAutoriza');
        const cboTipoContrato = $('#cboTipoContrato');
        const inputFechaContratacion = $('#inputFechaContratacion');
        const cboRazonSolicitud = $('#cboRazonSolicitud');
        const inputFechaVigencia = $('#inputFechaVigencia');
        const btnAutorizar = $('#btnAutorizar');
        const btnRechazar = $('#btnRechazar');
        const btnGuardar = $('#btnGuardar');
        const inputFechaBaja = $('#inputFechaBaja');
        const txtEmpleadoRemplazar = $('#txtEmpleadoRemplazar');
        const txtNumEmpleadoRemplazar = $('#txtNumEmpleadoRemplazar');
        const divRemplazoFecha = $('#divRemplazoFecha');
        const divRemplazoEmpleado = $('#divRemplazoEmpleado');

        const modalPlantilla = $('#modalPlantilla');
        const ccPlantilla = $('#ccPlantilla');
        const tblPlantilla = $('#tblPlantilla');
        const divComentarioRechazo = $('#divComentarioRechazo');
        const textAreaComentarioRechazo = $('#textAreaComentarioRechazo');
        const cboCategoria = $('#cboCategoria');
        const inputSueldoTipoNomina = $('#inputSueldoTipoNomina');
        const inputSueldoBase = $('#inputSueldoBase');
        const inputSueldoComplemento = $('#inputSueldoComplemento');
        const inputSueldoTotalNominal = $('#inputSueldoTotalNominal');
        //#endregion

        const _ESTATUS = {
            AUTORIZADA: 'A',
            PENDIENTE: 'P',
            CANCELADA: 'C'
        }

        const _MODAL_REQUI = {
            NUEVA: 1,
            CONSULTA: 2,
            AUTORIZACION: 3
        }

        //#region Eventos
        btnFiltrar.on('click', function () {
            getRequisiciones();
        });

        btnNuevo.on('click', function () {
            prepararRequisicion(_MODAL_REQUI.NUEVA, null);
            divRemplazoEmpleado.hide();
            divRemplazoFecha.hide();
        });

        btnPlantilla.on('click', function () {
            if (cboCC.val()) {
                getPlantilla().done(function (response) {
                    if (response && response.success) {
                        addRows(tblPlantilla, response.items);
                        modalPlantilla.modal('show');
                    }
                });
            } else {
                swal('Alerta!', 'Debe seleccionar un CC', 'warning');
            }
        });

        inputVacantes.on('change', function () {
            if ($(this).data('cantidad_maxima')) {
                if ($(this).val() > 0 && $(this).val() <= $(this).data('cantidad_maxima')) {

                } else {
                    $(this).val('');
                    swal('Alerta!', 'Favor de agregar un número del 1 al ' + $(this).data('cantidad_maxima'), 'warning');
                }
            } else {
                $(this).val('');
            }
        });

        modalPlantilla.on('shown.bs.modal', function () {
            tblPlantilla.DataTable().columns.adjust();
        });

        cboCC.on('change', function () {
            // inputJefe.fillComboBox('GetJefeInmediato', { cc: cboCC.val() }, '-- Seleccionar --', () => {
            //     inputAutoriza.find('option').remove();
            //     inputJefe.find('option').each(function (value, index, array) {
            //         $(this).clone().appendTo(inputAutoriza);
            //     });
            // });

            inputJefe.fillComboBox('GetJefeInmediato', { cc: cboCC.val() }, '-- Seleccionar --');
            inputAutoriza.fillComboBox('GetAutoriza', { cc: cboCC.val() }, '-- Seleccionar --');
        });

        btnAutorizarMasivo.click(function () {
            //#region SE OBTIENE LA CLAVE DEL EMPLEADO SELECCIONADO
            let arrRowsChecked = []
            let rowsChecked = tblRequisiciones.DataTable().column(0).checkboxes.selected()
            $.each(rowsChecked, function (i, claveEmpleado) {
                arrRowsChecked.push(claveEmpleado)
            })

            if (arrRowsChecked.length == 0) {
                Alert2Warning("Es necesario seleccionar al menos un empleado.")
            } else if (arrRowsChecked.length == 1) {
                Alert2AccionConfirmar('Autorización', "¿Desea autorizar la requisición seleccionada?", 'Confirmar', 'Cancelar', () => autorizarRechazarRequisicion('A', arrRowsChecked))
            } else if (arrRowsChecked.length > 1) {
                Alert2AccionConfirmar('Autorización', "¿Desea autorizar las requisiciones seleccionadas?", 'Confirmar', 'Cancelar', () => autorizarRechazarRequisicion('A', arrRowsChecked))
            }
            //#endregion
        })

        btnAutorizar.on('click', function () {
            //#region SE OBTIENE LA CLAVE DEL EMPLEADO SELECCIONADO
            let arrRowsChecked = []
            let rowsChecked = tblRequisiciones.DataTable().column(0).checkboxes.selected()
            $.each(rowsChecked, function (i, claveEmpleado) {
                arrRowsChecked.push(claveEmpleado)
            })

            if (btnAutorizar.data().idSigoplan > 0) {
                arrRowsChecked.push(btnAutorizar.data().idSigoplan)
            }

            if (arrRowsChecked.length == 0) {
                Alert2Warning("Es necesario seleccionar al menos un empleado.")
            } else if (arrRowsChecked.length == 1) {
                Alert2AccionConfirmar('Autorización', "¿Desea autorizar la requisición seleccionada?", 'Confirmar', 'Cancelar', () => autorizarRechazarRequisicion('A', arrRowsChecked))
            } else if (arrRowsChecked.length > 1) {
                Alert2AccionConfirmar('Autorización', "¿Desea autorizar las requisiciones seleccionadas?", 'Confirmar', 'Cancelar', () => autorizarRechazarRequisicion('A', arrRowsChecked))
            }
            //#endregion
        });

        btnRechazar.on('click', function () {
            //#region SE OBTIENE LA CLAVE DEL EMPLEADO SELECCIONADO
            let arrRowsChecked = []
            let rowsChecked = tblRequisiciones.DataTable().column(0).checkboxes.selected()
            $.each(rowsChecked, function (i, claveEmpleado) {
                arrRowsChecked.push(claveEmpleado)
            })

            if (btnRechazar.data().idSigoplan > 0) {
                arrRowsChecked.push(btnRechazar.data().idSigoplan)
            }

            if (arrRowsChecked.length == 0) {
                Alert2Warning("Es necesario seleccionar al menos un empleado.")
            } else if (arrRowsChecked.length == 1) {
                Alert2AccionConfirmar('Autorización', "¿Desea autorizar la requisición seleccionada?", 'Confirmar', 'Cancelar', () => autorizarRechazarRequisicion('C', arrRowsChecked))
            } else if (arrRowsChecked.length > 1) {
                Alert2AccionConfirmar('Autorización', "¿Desea autorizar las requisiciones seleccionadas?", 'Confirmar', 'Cancelar', () => autorizarRechazarRequisicion('C', arrRowsChecked))
            }
            //#endregion
        });

        btnGuardar.on('click', function () {
            if (validarCampos()) {
                let requisicion = {
                    cc: cboCC.val(),
                    puesto: +inputPuesto.val(),
                    cantidad_solicitada: +inputVacantes.val(),
                    // solicitante: +inputSolicita.attr('data-index'),
                    solicitante: inputSolicita.val(),
                    // autoriza: +inputAutoriza.attr('data-index'),
                    autoriza: inputAutoriza.val(),
                    // jefe_inmediato: +inputJefe.attr('data-index'),
                    jefe_inmediato: inputJefe.val(),
                    razon_solicitud: +cboRazonSolicitud.val(),
                    fecha_contratacion: inputFechaContratacion.val(),
                    tipo_contrato: cboTipoContrato.val(),
                    id_plantilla: +inputPlantilla.val(),
                    fecha_vigencia: inputFechaVigencia.val(),
                    fecha_baja: inputFechaBaja.val(),
                    sustituye: txtNumEmpleadoRemplazar.val(),
                    idTabuladorDet: cboCategoria.val() != "" ? cboCategoria.val() : 0,
                };

                axios.post('GuardarRequisicion', { requisicion })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            modalRequisicion.modal('hide');

                            if (response.data.correoEnviado) {
                                Alert2Exito('Se ha guardado la información.');
                            } else {
                                Alert2Info('Se ha guardado la información. ATENCIÓN: No se pudo enviar el correo al autorizante porque no se encuentra la información de su usuario en SIGOPLAN.');
                            }
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        });

        cboRazonSolicitud.on("change", function () {
            if ($(this).val() == "2") {
                divRemplazoEmpleado.show();
                divRemplazoFecha.show();
            } else {
                divRemplazoEmpleado.hide();
                divRemplazoFecha.hide();
            }
        });

        cboCategoria.on('change', function () {
            if ($(this).val()) {
                getTabuladorCategoria().done(function (response) {
                    if (response && response.success) {
                        inputSueldoBase.val(maskNumero(response.items.sueldoBase, 2));
                        inputSueldoComplemento.val(maskNumero(response.items.sueldoComplemento, 2));
                        inputSueldoTotalNominal.val(maskNumero(response.items.totalNominal, 2));
                        inputSueldoTipoNomina.val(response.items.tipoNomina);
                    } else {
                        limpiarCamposSueldo();
                    }
                });
                // getPlantilla().done(function (response) {
                //     if (response && response.success) {
                //         addRows(tblPlantilla, response.items);
                //         modalPlantilla.modal('show');
                //     }
                // });
            } else {
                limpiarCamposSueldo();
            }
        });
        //#endregion

        (function init() {
            getRequisiciones();
            cboFiltroCC.fillComboBox('GetCCs', null, '-- Seleccionar --', () => {
                cboFiltroCC.select2();

                cboFiltroCC.find('option').each(function (value, index, array) {
                    $(this).clone().appendTo(cboCC);
                });
                cboCC.select2({ width: '100%', dropdownParent: $(modalRequisicion) });
            });
            cboTipoContrato.fillComboBox('GetTipoContrato', null, '-- Seleccionar --', null);
            cboRazonSolicitud.fillComboBox('GetRazonSolicitud', null, '-- Seleccionar --', null);
            inputFechaContratacion.datepicker().datepicker('setDate', new Date());
            inputFechaBaja.datepicker().datepicker('setDate', new Date());
            inputFechaVigencia.datepicker().datepicker('setDate', new Date());

            // inputJefe.getAutocomplete(selectAutocompleteJefe, null, 'GetAutocompleteJefe');
            inputJefe.select2({ width: '100%', dropdownParent: $(modalRequisicion) });
            inputAutoriza.select2({ width: '100%', dropdownParent: $(modalRequisicion) });
            // inputSolicita.getAutocomplete(selectAutocompleteSolicita, null, 'GetAutocompleteSolicita');
            inputSolicita.fillComboBox('GetSolicita', null, null, () => {
                // if (inputSolicita.find('option').length > 0) {
                //     inputSolicita.find('option').attr('selected', 'selected').change();
                // }
            });

            initTblRequisicion();
            initTblPlantilla();

            txtEmpleadoRemplazar.getAutocomplete(funGetEmpleado, null, '/Administrativo/FormatoCambio/getCatEmpleadosReclutamientos');

        })();

        function funGetEmpleado(event, ui) {
            txtNumEmpleadoRemplazar.val(ui.item.id);
            txtEmpleadoRemplazar.val(ui.item.value);
        }

        function prepararRequisicion(estatusPreparacionModal, datos) {
            inputFechaVigencia.attr("disabled", false);
            switch (estatusPreparacionModal) {
                case _MODAL_REQUI.NUEVA:
                    limpiarCamposRequisicion();
                    desbloquearCamposRequisicion();

                    inputSolicita.fillComboBox('GetSolicita', null, null, () => {
                        // if (inputSolicita.find('option').length > 0) {
                        //     inputSolicita.find('option').attr('selected', 'selected').change();
                        // }
                    });

                    getIdRequisicionDisponible().done(function (response) {
                        if (response && response.success) {
                            modalRequisicionTitulo.text('CREAR NUEVA REQUISICIÓN');
                            inputEstatus.val('PENDIENTE');
                            inputId.val(response.items);
                            btnAutorizar.hide();
                            btnRechazar.hide();
                            btnGuardar.show();
                            divComentarioRechazo.hide();
                            modalRequisicion.modal('show');
                        }
                    });

                    fncGetFechaVigencia7DiasNaturales().done(function (response) {
                        if (response && response.success) {
                            inputFechaVigencia.val(moment(response.items).format("DD/MM/YYYY"));
                            inputFechaVigencia.attr("disabled", true);
                        }
                    })
                    break;
                case _MODAL_REQUI.CONSULTA:
                    break;
                case _MODAL_REQUI.AUTORIZACION:
                    break;
            }
        }

        function limpiarCamposSueldo() {
            inputSueldoBase.val('');
            inputSueldoComplemento.val('');
            inputSueldoTotalNominal.val('');
            inputSueldoTipoNomina.val('');
        }

        function limpiarCamposRequisicion() {
            inputId.val('');
            inputEstatus.val('');
            cboCC.val('').change();
            inputPlantilla.val('');
            inputVacantes.val('');
            inputVacantes.removeData();
            inputPuesto.val('');
            inputPuestoDescripcion.val('');
            inputJefe.val('');
            // inputJefe.removeAttr('data-index');
            // inputSolicita.val('');
            // inputSolicita.removeAttr('data-index');
            inputAutoriza.val('');
            // inputAutoriza.removeAttr('data-index');
            cboTipoContrato.val('').change();
            inputFechaContratacion.val(moment(Date.now()).format("DD/MM/YYYY"));
            cboRazonSolicitud.val('').change();
            inputFechaVigencia.val('');
            cboCategoria.val("").change();
            limpiarCamposSueldo();
        }

        function validarCampos() {
            if (!cboCC.val()) { swal('Alerta!', 'Favor de seleccionar un CC', 'warning'); return false; }
            if (!inputPlantilla.val()) { swal('Alerta!', 'Favor de ingresar una plantilla', 'warning'); return false; }
            if (!inputVacantes.val()) { swal('Alerta!', 'Favor de ingresar la cantidad de vacantes a cubrir', 'warning'); return false; }
            if (!inputPuesto.val()) { swal('Alerta!', 'Favor de ingresar un puesto', 'warning'); return false; }
            if (!cboCategoria.val()) { swal('Alerta!', 'Favor de ingresar una categoria del puesto', 'warning'); return false; }
            // if (!inputJefe.attr('data-index')) { swal('Alerta!', 'Favor de ingresar un jefe inmediato', 'warning'); return false; }
            if (!inputJefe.val()) { swal('Alerta!', 'Favor de ingresar un jefe inmediato', 'warning'); return false; }
            // if (!inputSolicita.attr('data-index')) { swal('Alerta!', 'Favor de ingresar un solicitante', 'warning'); return false; }
            if (!inputSolicita.val() || inputSolicita.val() == 0) { swal('Alerta!', 'Favor de ingresar un solicitante'); }
            // if (!inputAutoriza.attr('data-index')) { swal('Alerta!', 'Favor de ingresar un autorizante', 'warning'); return false; }
            if (!inputAutoriza.val()) { swal('Alerta!', 'Favor de ingresar un autorizante', 'warning'); return false; }
            if (!cboTipoContrato.val()) { swal('Alerta!', 'Favor de ingresar un tipo de contrato', 'warning'); return false; }
            if (!inputFechaContratacion.val()) { swal('Alerta!', 'Favor de ingresar una fecha de contratación', 'warning'); return false; }
            if (!inputFechaVigencia.val()) { swal('Alerta!', 'Favor de ingresar una fecha de vigencia', 'warning'); return false; }
            if (!cboRazonSolicitud.val()) { swal('Alerta!', 'Favor de ingresar una razón de la solicitud', 'warning'); return false; }
            return true;
        }

        // function selectAutocompleteJefe(event, ui) {
        //     $(this).text(ui.item.label);
        //     $(this).attr('data-index', ui.item.id);
        // }

        // function selectAutocompleteSolicita(event, ui) {
        //     $(this).text(ui.item.label);
        //     $(this).attr('data-index', ui.item.id);
        // }

        // function selectAutocompleteAutoriza(event, ui) {
        //     $(this).text(ui.item.label);
        //     $(this).attr('data-index', ui.item.id);
        // }

        //#region Server
        function getRequisiciones() {
            let ccs = cboFiltroCC.val();
            let estatus = null;

            if (cboFiltroEstatus.val()) {
                estatus = cboFiltroEstatus.val();
            }

            $.post('GetRequisiciones',
                {
                    ccs: ccs,
                    estatus: estatus
                }).then(response => {
                    if (response.success) {
                        addRows(tblRequisiciones, response.items);
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function getIdRequisicionDisponible() {
            return $.get('GetIdRequisicionDisponible').then(response => {
                if (response.success) {
                    return response;
                } else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
            });
        }

        function getPlantilla() {
            return $.get('GetPlantilla',
                {
                    cc: cboCC.val()
                }).then(response => {
                    if (response.success) {
                        return response;
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function fncGetFechaVigencia7DiasNaturales() {
            return $.get("GetFechaVigencia7DiasNaturales").then(response => {
                if (response.success) {
                    return response;
                } else {
                    swal("¡Alerta!", response.message, "warning");
                }
            }, error => {
                swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
            });
        }

        function getTabuladorCategoria() {
            return $.get('GetTabuladorCategoria',
                {
                    categoriaID: cboCategoria.val()
                }).then(response => {
                    if (response.success) {
                        return response;
                    } else {
                        swal('¡Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }
        //#endregion

        function autorizarRechazarRequisicion(estatus, arrRowsChecked) {
            if (arrRowsChecked.length > 0) {
                let obj = {}
                obj.lstID_Requisiciones = arrRowsChecked
                obj.estatus = estatus
                obj.comentarioRechazo = textAreaComentarioRechazo.val()
                axios.post('AutorizarRechazarRequisicion', obj).then(response => {
                    let { success, datos, message } = response.data;
                    if (success) {
                        btnAutorizar.data().idSigoplan = 0
                        btnRechazar.data().idSigoplan = 0
                        Alert2Exito(message)
                        modalRequisicion.modal('hide')
                        btnFiltrar.click()
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(message))
            } else {
                Alert2Warning("Es necesario seleccionar al menos un empleado.")
            }
        }

        //#region Tablas
        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function initTblRequisicion() {
            tblRequisiciones.DataTable({
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
                        data: 'idSigoplan', title: 'ID', className: 'dt-head-center dt-body-right'
                        // render: function (data, type, row) {
                        //     if (row.esAutorizante && row.estatus == _ESTATUS.PENDIENTE) {
                        //         return `<input type="checkbox">`
                        //     } else {
                        //         btnAutorizarMasivo.css("display", "none")
                        //         return ``
                        //     }
                        // }
                    },
                    {
                        data: null,
                        title: 'OPCIONES',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            let btnAutorizar = '<button class="btn btn-success btnAutorizar btn-xs" title="Autorizar"><i class="fas fa-user-check"></i></button>';
                            let btnCancelar = '<button class="btn btn-warning btnCancelar btn-xs" title="Cancelar"><i class="fas fa-user-times"></i></button>';
                            let btnConsultar = '<button class="btn btn-info btnCconsultar btn-xs" title="Consultar"><i class="fas fa-eye"></i></button>';

                            if (row.esAutorizante && row.estatus == _ESTATUS.PENDIENTE) {
                                return btnAutorizar + ' ' + btnCancelar + ' ' + btnConsultar;
                            } else {
                                return btnConsultar;
                            }
                        }
                    },
                    {
                        data: 'id',
                        title: 'ID',
                        className: 'dt-head-center dt-body-center'
                    },
                    {
                        data: 'ccDescripcion',
                        title: 'CENTRO DE COSTOS',
                        className: 'dt-head-center dt-body-left'
                    },
                    {
                        data: 'puestoDescripcion',
                        title: 'PUESTO',
                        className: 'dt-head-center dt-body-left'
                    },
                    {
                        data: 'categoriaDescripcion',
                        title: 'CATEGORÍA',
                        className: 'dt-head-center dt-body-center'
                    },
                    {
                        data: 'cantidad_solicitada',
                        title: 'CANTIDAD SOLICITADA',
                        className: 'dt-center'
                    },
                    {
                        data: 'fecha_contratacion',
                        title: 'FECHA CONTRATACIÓN',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        data: 'nombreJefeInmediato',
                        title: 'NOMBRE JEFE INMEDIATO',
                        className: 'dt-head-center dt-body-left'
                    },
                    {
                        data: 'nombreAutoriza',
                        title: 'NOMBRE AUTORIZA',
                        className: 'dt-head-center dt-body-left'
                    },
                    {
                        data: 'estatus',
                        title: 'ESTATUS',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            if (data == _ESTATUS.AUTORIZADA) {
                                return 'AUTORIZADA';
                            }

                            if (data == _ESTATUS.PENDIENTE) {
                                return 'PENDIENTE'
                            }

                            if (data == _ESTATUS.CANCELADA) {
                                return 'CANCELADA';
                            }
                        }
                    },
                    {
                        className: 'dt-center',
                        render: function (data, type, row, meta) {
                            if (row.puedeEliminar) {
                                return `<button class="btn btn-xs btn-danger botonEliminarRequisicion"><i class="fa fa-times"></i></button>`;
                            } else {
                                return ``;
                            }
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblRequisiciones.on('click', '.btnAutorizar', function () {
                        let rowData = tblRequisiciones.DataTable().row($(this).closest('tr')).data();

                        btnAutorizar.show();
                        btnRechazar.hide();
                        btnGuardar.hide();
                        btnAutorizar.data().idSigoplan = rowData.idSigoplan

                        cargarInformacionRequisicion(rowData.id, rowData.cc, false, false);
                    });

                    tblRequisiciones.on('click', '.btnCancelar', function () {
                        let rowData = tblRequisiciones.DataTable().row($(this).closest('tr')).data();

                        btnAutorizar.hide();
                        btnRechazar.show();
                        btnGuardar.hide();
                        btnRechazar.data().idSigoplan = rowData.idSigoplan

                        cargarInformacionRequisicion(rowData.id, rowData.cc, true, true);
                    });

                    tblRequisiciones.on('click', '.btnCconsultar', function () {
                        let rowData = tblRequisiciones.DataTable().row($(this).closest('tr')).data();

                        btnAutorizar.hide();
                        btnRechazar.hide();
                        btnGuardar.hide();

                        cargarInformacionRequisicion(rowData.id, rowData.cc, true, false);
                    });

                    tblRequisiciones.on('click', '.botonEliminarRequisicion', function () {
                        let rowData = tblRequisiciones.DataTable().row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar la requisicion con ID "${rowData.id}"?`,
                            () => eliminarRequisicion(rowData.id))
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { className: 'dt-body-center', 'targets': '_all' },
                    // { className: 'dt-body-right', targets: [0] },
                    { width: '3%', targets: [0, 10] },
                    { width: '8%', targets: [6, 7] },
                    {
                        targets: 0,
                        checkboxes: {
                            selectRow: true
                        }
                    }
                ],
                select: {
                    style: 'multi'
                }
            });
        }

        function eliminarRequisicion(requisicion_id) {
            axios.post('EliminarRequisicion', { requisicion_id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado la información.');
                        getRequisiciones();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarInformacionRequisicion(requisicion_id, cc, mostrarComentarioRechazo, rechazo) {
            limpiarCamposRequisicion();
            bloquearCamposRequisicion();

            if (mostrarComentarioRechazo) {
                if (rechazo) {
                    textAreaComentarioRechazo.attr('disabled', false);
                }

                divComentarioRechazo.show();
            } else {
                divComentarioRechazo.hide();
            }

            axios.post('GetInformacionRequisicionConsulta', { requisicion_id, cc })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        llenarInformacionRequisicion(response.data.data);
                        modalRequisicion.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function bloquearCamposRequisicion() {
            cboCC.attr('disabled', true);
            btnPlantilla.attr('disabled', true);
            inputVacantes.attr('disabled', true);
            inputJefe.attr('disabled', true);
            inputSolicita.attr('disabled', true);
            inputAutoriza.attr('disabled', true);
            cboTipoContrato.attr('disabled', true);
            inputFechaContratacion.attr('disabled', true);
            cboRazonSolicitud.attr('disabled', true);
            inputFechaVigencia.attr('disabled', true);
            textAreaComentarioRechazo.attr('disabled', true);
        }

        function desbloquearCamposRequisicion() {
            cboCC.attr('disabled', false);
            btnPlantilla.attr('disabled', false);
            // inputVacantes.attr('disabled', false);
            inputJefe.attr('disabled', false);
            // inputSolicita.attr('disabled', false);
            inputAutoriza.attr('disabled', false);
            cboTipoContrato.attr('disabled', false);
            // inputFechaContratacion.attr('disabled', false);
            cboRazonSolicitud.attr('disabled', false);
            inputFechaVigencia.attr('disabled', false);
            textAreaComentarioRechazo.attr('disabled', false);
        }

        function llenarInformacionRequisicion(data) {
            inputId.val(data.id);

            let estatusRequisicion = '';

            switch (data.estatus) {
                case 'A':
                    estatusRequisicion = 'AUTORIZADA';
                    break;
                case 'P':
                    estatusRequisicion = 'PENDIENTE';
                    break;
                case 'C':
                    estatusRequisicion = 'RECHAZADA';
                    break;
            }

            inputEstatus.val(estatusRequisicion);
            cboCC.val(data.cc);
            cboCC.change();
            inputPlantilla.val(data.id_plantilla);
            inputVacantes.val(data.cantidad_solicitada > 0 ? data.cantidad_solicitada : 1);
            inputPuesto.val(data.puesto);
            inputPuestoDescripcion.val(data.puestoDesc);

            inputSolicita.empty();
            inputSolicita.append(`<option value="${data.solicitante}">${data.solicitanteDesc}</option>`);
            inputSolicita.val(data.solicitante);

            // inputJefe.fillComboBox('GetJefeInmediato', { cc: cboCC.val() }, '-- Seleccionar --', () => {
            //     inputAutoriza.find('option').remove();
            //     inputJefe.find('option').each(function (value, index, array) {
            //         $(this).clone().appendTo(inputAutoriza);
            //     });

            //     inputJefe.val(data.jefe_inmediato);
            //     inputAutoriza.val(data.autoriza);
            // });
            inputJefe.fillComboBox('GetJefeInmediato', { cc: cboCC.val() }, '-- Seleccionar --', () => {
                inputJefe.val(data.jefe_inmediato);

                inputAutoriza.fillComboBox('GetAutoriza', { cc: cboCC.val() }, '-- Seleccionar --', () => {
                    inputAutoriza.val(data.autoriza);
                });
            });

            cboTipoContrato.val(data.tipo_contrato);
            inputFechaContratacion.val(data.fecha_contratacionString);
            cboRazonSolicitud.val(data.razon_solicitud);
            inputFechaVigencia.val(data.fecha_vigenciaString);
            textAreaComentarioRechazo.val(data.comentarioRechazo);

            cboCategoria.fillComboBox('GetCategoriasPuesto', { idPuesto: data.puesto, cc: data.cc }, '-- Seleccionar --', () => {
                cboCategoria.val(data.idTabuladorDet);
                cboCategoria.change();
            });
        }

        function initTblPlantilla() {
            tblPlantilla.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
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
                            return '<button class="btn btn-success btn-xs btnAgregarPlantilla" title="Usar puesto"><i class="fas fa-check"></i></button>'
                        }
                    },
                    {
                        data: 'puestoDescripcion',
                        title: 'PUESTO',
                        className: 'dt-head-center dt-body-left'
                    },
                    {
                        data: 'solicitados',
                        title: 'SOLICITADOS',
                        className: 'dt-center'
                    },
                    {
                        data: 'faltantes',
                        title: 'FALTANTES',
                        className: 'dt-center'
                    }
                ],

                initComplete: function (settings, json) {
                    tblPlantilla.on('click', '.btnAgregarPlantilla', function () {
                        let rowData = tblPlantilla.DataTable().row($(this).closest('tr')).data();

                        inputPlantilla.val(rowData.id_plantilla);
                        inputVacantes.data('cantidad_maxima', (rowData.faltantes > 0 ? rowData.faltantes : 1));
                        inputVacantes.val('');
                        inputPuesto.val(rowData.puesto);
                        inputPuestoDescripcion.val(rowData.puestoDescripcion);
                        inputVacantes.val(1);
                        inputVacantes.trigger('change');
                        modalPlantilla.modal('hide');

                        $("#cboCategoria option").remove();
                        cboCategoria.fillComboBox('GetCategoriasPuesto', {
                            idPuesto: rowData.puesto,
                            cc: cboCC.val()
                        }, '-- Seleccionar --', null);
                    });
                }
            });
        }
        //#endregion
    }
    $(document).ready(() => RecursosHumanos.Reclutamientos = new Reclutamientos())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();