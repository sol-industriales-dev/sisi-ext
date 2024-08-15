(() => {
    $.namespace('ADMIN_FINANZAS.ProspectosClientes');

    //#region CONST FILTROS
    const cboFiltro_Division = $("#cboFiltro_Division");
    const cboFiltro_Cliente = $("#cboFiltro_Cliente");
    const cboFiltro_Historial = $('#cboFiltro_Historial');
    const btnFiltro_Buscar = $("#btnFiltro_Buscar");
    const btnFiltro_Nuevo = $("#btnFiltro_Nuevo");
    const btnFiltro_ProspectoProyecto = $('#btnFiltro_ProspectoProyecto');
    const HistorialEnum = { ACTIVO: 1, HISTORIAL: 2 };
    //#endregion

    //#region LISTADO PROSPECTOS CLIENTES
    let dtProspectosClientes;
    const tblProspectosClientes = $('#tblProspectosClientes');
    //#endregion

    //#region CREAR/EDITAR PROSPECTO CLIENTE
    const mdlCE_ProspectoCliente = $("#mdlCE_ProspectoCliente");
    const cboCE_Prospecto_Division = $("#cboCE_Prospecto_Division");
    const txtCE_Prospecto_NombreProyecto = $('#txtCE_Prospecto_NombreProyecto');
    const cboCE_Prospecto_Cliente = $("#cboCE_Prospecto_Cliente");
    const cboCE_Prospecto_Pais = $("#cboCE_Prospecto_Pais");
    const cboCE_Prospecto_Estado = $("#cboCE_Prospecto_Estado");
    const cboCE_Prospecto_Municipio = $("#cboCE_Prospecto_Municipio");
    const txtCE_Prospecto_MontoInversion = $("#txtCE_Prospecto_MontoInversion");
    const txtCE_Prospecto_FechaInicio = $("#txtCE_Prospecto_FechaInicio");
    const cboCE_Prospecto_Canal = $("#cboCE_Prospecto_Canal");
    const btnCE_ProspectoCliente = $("#btnCE_ProspectoCliente");
    let arrProspectos = [];
    //#endregion

    //#region CREAR/EDITAR PROYECTO
    const mdlCE_Proyecto = $("#mdlCE_Proyecto");
    const cboCE_Proyecto_Prioridad = $("#cboCE_Proyecto_Prioridad");
    const cboCE_Proyecto_Estatus = $("#cboCE_Proyecto_Estatus");
    const cboCE_Proyecto_Escenario = $("#cboCE_Proyecto_Escenario");
    const btnCE_Proyecto = $("#btnCE_Proyecto");
    const EscenariosEnum = { A: 1, B: 2, C: 3, D: 4, E: 5, T: 6 };
    //#endregion

    ProspectosClientes = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblProspectosClientes();
            fncFillCbos();
            $(".select2").select2();
            fncIndicarMenuSeleccion();
            //#endregion

            //#region FILTROS
            btnFiltro_Buscar.click(function () {
                fncGetProspectosClientes();
            });

            btnFiltro_Nuevo.click(function () {
                btnCE_ProspectoCliente.data().id = 0;
                btnCE_ProspectoCliente.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                fncLimpiarMdlCE_ProspectoCliente();
                mdlCE_ProspectoCliente.modal("show");
            });

            btnFiltro_ProspectoProyecto.click(function () {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea enviar a los prospectos seleccionados a proyectos?', 'Confirmar', 'Cancelar', () => fncEnviarProspectosProyecto());
            });
            //#endregion

            //#region CREAR/EDITAR PROSPECTO CLIENTE
            cboCE_Prospecto_Pais.change(function () {
                if ($(this).val() > 0) {
                    cboCE_Prospecto_Estado.fillCombo('FillCboEstados', { FK_Pais: $(this).val() }, false, null);
                }
            });

            cboCE_Prospecto_Estado.change(function () {
                if ($(this).val() > 0) {
                    cboCE_Prospecto_Municipio.fillCombo('FillCboMunicipios', { FK_Estado: $(this).val() }, false, null);
                }
            });

            btnCE_ProspectoCliente.click(function () {
                if ($(this).data().id <= 0) {
                    fncCrearProspectoCliente();
                } else {
                    fncActualizarProspectoCliente();
                }
            });
            //#endregion

            //#region PROYECTO
            cboCE_Proyecto_Prioridad.change(function () {
                if ($(this).val() > 0) {
                    cboCE_Proyecto_Estatus.fillCombo('FillCboPrioridadesEstatus', { FK_Prioridad: $(this).val() }, false, null);
                    fncSetEscenario($(this).val());
                }
            });

            btnCE_Proyecto.click(function () {
                let obj = {};
                obj.id = $(this).data().id;
                obj.FK_Prioridad = cboCE_Proyecto_Prioridad.val();
                obj.FK_Estatus = cboCE_Proyecto_Estatus.val();
                obj.FK_Escenario = cboCE_Proyecto_Escenario.val();
                arrProspectos.push(obj);
                mdlCE_Proyecto.modal("hide");
            });
            //#endregion
        }

        //#region PROSPECTOS CLIENTES
        function initTblProspectosClientes() {
            dtProspectosClientes = tblProspectosClientes.DataTable({
                language: dtDicEsp,
                paging: true,
                ordering: true,
                searching: true,
                columns: [
                    {
                        render: (data, type, row, meta) => {
                            return `<input type="checkbox" class="chkProspecto">`
                        }
                    },
                    { data: 'division', title: 'División' },
                    { data: 'nombreProyecto', title: 'Proyecto' },
                    { data: 'nombreCliente', title: 'Cliente' },
                    { data: 'tipoCliente', title: 'Tipo cliente' },
                    { data: 'ubicacion', title: 'Ubicación' },
                    { data: 'strImporteCotizadoAprox', title: 'Monto inversión' },
                    {
                        title: "Fecha inicio",
                        render: (data, type, row, meta) => {
                            return moment(data).format('DD/MM/YYYY')
                        }
                    },
                    { data: 'canal', title: 'Canal' },
                    {
                        title: 'Acciones',
                        render: function (data, type, row, meta) {
                            let botones = "";
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            let btnEnviarHistorial = `<button class='btn btn-xs btn-primary enviarHistorial' title='Enviar prospecto a historial.'><i class="fas fa-archive"></i></button>`
                            let btnActivarProspecto = `<button class='btn btn-xs btn-primary activarProspecto' title='Activar prospecto.'><i class="fas fa-power-off"></i></button>`

                            botones = `${btnEditar} ${btnEliminar}`;
                            if (cboFiltro_Historial.val() == HistorialEnum.ACTIVO) {
                                botones += ` ${btnEnviarHistorial}`;
                            }
                            else if (cboFiltro_Historial.val() == HistorialEnum.HISTORIAL) {
                                botones += ` ${btnActivarProspecto}`;
                            }

                            return botones;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblProspectosClientes.on('click', '.editarRegistro', function () {
                        let rowData = dtProspectosClientes.row($(this).closest('tr')).data();
                        fncGetDatosActualizarProspectoCliente(rowData.id);
                    });

                    tblProspectosClientes.on('click', '.eliminarRegistro', function () {
                        let rowData = dtProspectosClientes.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarProspectoCliente(rowData.id));
                    });

                    tblProspectosClientes.on('click', '.chkProspecto', function () {
                        let rowData = dtProspectosClientes.row($(this).closest('tr')).data();
                        fncLimpiarMdlCE_Proyecto();
                        btnCE_Proyecto.data().id = rowData.id;
                        if (rowData.esSeleccionado == 0) {
                            rowData.esSeleccionado = 1;
                        } else {
                            rowData.esSeleccionado = 0;

                            arrTemp = [];
                            arrProspectos.forEach(element => {
                                if (element.id != rowData.id) {
                                    let obj = {};
                                    obj.id = element.id;
                                    obj.FK_Prioridad = element.FK_Prioridad;
                                    obj.FK_Estatus = element.FK_Estatus;
                                    obj.FK_Escenario = element.FK_Escenario;
                                    arrTemp.push(obj);
                                }
                            });

                            arrProspectos = []
                            arrTemp.forEach(element => {
                                let obj = {};
                                obj.id = element.id;
                                obj.FK_Prioridad = element.FK_Prioridad;
                                obj.FK_Estatus = element.FK_Estatus;
                                obj.FK_Escenario = element.FK_Escenario;
                                arrProspectos.push(obj);
                            });
                        }

                        if (rowData.esSeleccionado == 1) {
                            mdlCE_Proyecto.modal("show");
                        }
                    });

                    tblProspectosClientes.on('click', '.enviarHistorial', function () {
                        let rowData = dtProspectosClientes.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea enviar al prospecto a historial?', 'Confirmar', 'Cancelar', () => fncEnviarProspectoHistorial(rowData.id));
                    });

                    tblProspectosClientes.on('click', '.activarProspecto', function () {
                        let rowData = dtProspectosClientes.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea activar al prospecto?', 'Confirmar', 'Cancelar', () => fncActivarProspecto(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetProspectosClientes() {
            fncDefaultCtrls("cboFiltro_Historial", true);
            if (cboFiltro_Historial.val() > 0) {
                let objParamsDTO = {};
                objParamsDTO.FK_Division = cboFiltro_Division.val();
                objParamsDTO.FK_Cliente = cboFiltro_Cliente.val();
                objParamsDTO.FK_EstatusHistorial = cboFiltro_Historial.val();
                axios.post('GetProspectosClientes', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtProspectosClientes.clear();
                        dtProspectosClientes.rows.add(items);
                        dtProspectosClientes.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
            else {
                if (cboFiltro_Historial.val() <= 0) { fncValidacionCtrl("cboFiltro_Historial", true, "Es necesario seleccionar el estatus."); }
            }
        }

        function fncCrearProspectoCliente() {
            let objParamsDTO = fncCEOBJProspectoCliente();
            if (objParamsDTO != "") {
                axios.post('CrearProspectoCliente', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetProspectosClientes();
                        fncFillCbos();
                        mdlCE_ProspectoCliente.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncActualizarProspectoCliente() {
            let objParamsDTO = fncCEOBJProspectoCliente();
            if (objParamsDTO != "") {
                axios.post('ActualizarProspectoCliente', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetProspectosClientes();
                        fncFillCbos();
                        mdlCE_ProspectoCliente.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncEliminarProspectoCliente(idProspectoCliente) {
            if (idProspectoCliente > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idProspectoCliente;
                axios.post('EliminarProspectoCliente', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetProspectosClientes();
                        fncFillCbos();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el prospecto.");
            }
        }

        function fncGetDatosActualizarProspectoCliente(idProspectoCliente) {
            if (idProspectoCliente > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idProspectoCliente;
                axios.post('GetDatosActualizarProspectoCliente', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        cboCE_Prospecto_Division.val(items.FK_Division);
                        cboCE_Prospecto_Division.trigger("change");
                        txtCE_Prospecto_NombreProyecto.val(items.nombreProyecto);
                        cboCE_Prospecto_Cliente.val(items.FK_Cliente);
                        cboCE_Prospecto_Cliente.trigger("change");
                        cboCE_Prospecto_Pais.val(items.FK_Pais);
                        cboCE_Prospecto_Pais.trigger("change");
                        cboCE_Prospecto_Estado.val(items.FK_Estado);
                        cboCE_Prospecto_Estado.trigger("change");
                        cboCE_Prospecto_Municipio.val(items.FK_Municipio);
                        cboCE_Prospecto_Municipio.trigger("change");
                        txtCE_Prospecto_MontoInversion.val(items.importeCotizadoAprox);
                        txtCE_Prospecto_FechaInicio.val(moment(items.fechaInicio).format('YYYY-MM-DD'));
                        cboCE_Prospecto_Canal.val(items.FK_Canal);
                        cboCE_Prospecto_Canal.trigger("change");
                        btnCE_ProspectoCliente.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
                        btnCE_ProspectoCliente.data().id = idProspectoCliente;
                        mdlCE_ProspectoCliente.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEOBJProspectoCliente() {
            fncDefaultCtrls("cboCE_Prospecto_Division", true);
            fncDefaultCtrls("txtCE_Prospecto_NombreProyecto", false);
            fncDefaultCtrls("cboCE_Prospecto_Cliente", true);
            fncDefaultCtrls("cboCE_Prospecto_Pais", true);
            fncDefaultCtrls("cboCE_Prospecto_Estado", true);
            fncDefaultCtrls("cboCE_Prospecto_Municipio", true);
            fncDefaultCtrls("txtCE_Prospecto_MontoInversion", false);
            fncDefaultCtrls("txtCE_Prospecto_FechaInicio", false);
            fncDefaultCtrls("cboCE_Prospecto_Canal", true);

            if (cboCE_Prospecto_Division.val() <= 0) { fncValidacionCtrl("cboCE_Prospecto_Division", true, "Es necesario seleccionar una división."); return ""; }
            if (txtCE_Prospecto_NombreProyecto.val() == "") { fncValidacionCtrl("txtCE_Prospecto_NombreProyecto", false, "Es necesario indicar el nombre del proyecto."); return ""; }
            if (cboCE_Prospecto_Cliente.val() <= 0) { fncValidacionCtrl("cboCE_Prospecto_Cliente", true, "Es necesario seleccionar un cliente."); return ""; }
            if (cboCE_Prospecto_Pais.val() <= 0) { fncValidacionCtrl("cboCE_Prospecto_Pais", true, "Es necesario seleccionar un país."); return ""; }
            if (cboCE_Prospecto_Estado.val() <= 0) { fncValidacionCtrl("cboCE_Prospecto_Estado", true, "Es necesario seleccionar un estado."); return ""; }
            if (cboCE_Prospecto_Municipio.val() <= 0) { fncValidacionCtrl("cboCE_Prospecto_Municipio", true, "Es necesario seleccionar un municipio."); return ""; }
            if (txtCE_Prospecto_MontoInversion.val() == "") { fncValidacionCtrl("txtCE_Prospecto_MontoInversion", false, "Es necesario indicar el monto de inversión."); return ""; }
            if (txtCE_Prospecto_MontoInversion.val() == 0) { fncValidacionCtrl("txtCE_Prospecto_MontoInversion", false, "El necesario que el monto sea mayor a 0."); return ""; }
            if (txtCE_Prospecto_MontoInversion.val() <= -1) { fncValidacionCtrl("txtCE_Prospecto_MontoInversion", false, "Es necesario indicar el importe con valor positivo."); return ""; }
            if (txtCE_Prospecto_FechaInicio.val() == "") { fncValidacionCtrl("txtCE_Prospecto_FechaInicio", false, "Es necesario indicar la fecha de inicio."); return ""; }
            if (cboCE_Prospecto_Canal.val() <= 0) { fncValidacionCtrl("cboCE_Prospecto_Canal", true, "Es necesario seleccionar un canal."); return ""; }

            let objParamsDTO = {};
            objParamsDTO.id = btnCE_ProspectoCliente.data().id;
            objParamsDTO.nombreProyecto = txtCE_Prospecto_NombreProyecto.val();
            objParamsDTO.FK_Division = cboCE_Prospecto_Division.val();
            objParamsDTO.FK_Cliente = cboCE_Prospecto_Cliente.val();
            objParamsDTO.FK_Municipio = cboCE_Prospecto_Municipio.val();
            objParamsDTO.importeCotizadoAprox = txtCE_Prospecto_MontoInversion.val();
            objParamsDTO.fechaInicio = txtCE_Prospecto_FechaInicio.val();
            objParamsDTO.FK_Canal = cboCE_Prospecto_Canal.val();
            return objParamsDTO;
        }

        function fncLimpiarMdlCE_ProspectoCliente() {
            $("input[type='text']").val("");
            cboCE_Prospecto_Division[0].selectedIndex = 0;
            cboCE_Prospecto_Division.trigger("change");
            cboCE_Prospecto_Cliente[0].selectedIndex = 0;
            cboCE_Prospecto_Cliente.trigger("change");
            cboCE_Prospecto_Pais[0].selectedIndex = 0;
            cboCE_Prospecto_Pais.trigger("change");
            cboCE_Prospecto_Estado[0].selectedIndex = 0;
            cboCE_Prospecto_Estado.trigger("change");
            cboCE_Prospecto_Municipio[0].selectedIndex = 0;
            cboCE_Prospecto_Municipio.trigger("change");
            cboCE_Prospecto_Canal[0].selectedIndex = 0;
            cboCE_Prospecto_Canal.trigger("change");
        }

        function fncFillCbos() {
            cboFiltro_Division.fillCombo('FillCboFiltro_Prospectos_Divisiones', null, false, null);
            cboFiltro_Cliente.fillCombo('FillCboFiltro_Prospectos_Clientes', null, false, null);
            cboFiltro_Historial.fillCombo('FillCboHistorial', null, false, null);
            cboFiltro_Historial[0].selectedIndex = 1;
            cboFiltro_Historial.trigger("change");
            fncGetProspectosClientes();

            cboCE_Proyecto_Escenario.fillCombo('FillCboEscenarios', null, false, null);
            cboCE_Prospecto_Division.fillCombo('FillCboDivisiones', null, false, null);
            cboCE_Prospecto_Cliente.fillCombo('FillCboClientes', null, false, null);
            cboCE_Prospecto_Pais.fillCombo('FillCboPaises', null, false, null);
            cboCE_Prospecto_Canal.fillCombo('FillCboCanales', null, false, null);
            cboCE_Proyecto_Prioridad.fillCombo("FillCboPrioridades", null, false, null);
        }

        function fncEnviarProspectoHistorial(idProspecto) {
            if (idProspecto > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idProspecto;
                axios.post('EnviarProspectoHistorial', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetProspectosClientes();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al enviar el prospecto a historial.");
            }
        }

        function fncActivarProspecto(idProspecto) {
            if (idProspecto > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idProspecto;
                axios.post('ActivarProspecto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetProspectosClientes();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al activar el prospecto.");
            }
        }

        function fncEnviarProspectosProyecto() {
            let objParamsDTO = {}
            objParamsDTO.lstProspectosDTO = arrProspectos;
            axios.post('EnviarProspectosProyecto', objParamsDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetProspectosClientes();
                    Alert2Exito(message);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region PROYECTOS
        function fncSetEscenario(idPrioridad) {
            switch (idPrioridad) {
                case "1":
                    cboCE_Proyecto_Escenario.val(EscenariosEnum.E);
                    cboCE_Proyecto_Escenario.change();
                    break;
                case "2":
                    cboCE_Proyecto_Escenario.val(EscenariosEnum.D);
                    cboCE_Proyecto_Escenario.change();
                    break;
                case "3":
                    cboCE_Proyecto_Escenario.val(EscenariosEnum.C);
                    cboCE_Proyecto_Escenario.change();
                    break;
                case "4":
                    cboCE_Proyecto_Escenario.val(EscenariosEnum.B);
                    cboCE_Proyecto_Escenario.change();
                    break;
                case "5":
                    {
                        if (cboCE_Proyecto_Estatus.val() > 0) {
                            if (cboCE_Proyecto_Estatus.val() == EstatusEnum.Ganado) {
                                cboCE_Proyecto_Escenario.val(EscenariosEnum.A);
                            } else if (cboCE_Proyecto_Estatus.val() == EstatusEnum.Perdido) {
                                cboCE_Proyecto_Escenario.val(EscenariosEnum.T);
                            }
                            cboCE_Proyecto_Escenario.change();
                        }
                        break;
                    }
            }
        }

        function fncLimpiarMdlCE_Proyecto() {
            cboCE_Proyecto_Prioridad[0].selectedIndex = 0;
            cboCE_Proyecto_Prioridad.trigger("change");
            cboCE_Proyecto_Estatus[0].selectedIndex = 0;
            cboCE_Proyecto_Estatus.trigger("change");
            cboCE_Proyecto_Escenario[0].selectedIndex = 0;
            cboCE_Proyecto_Escenario.trigger("change");
        }
        //#endregion

        //#region GENERALES
        function fncIndicarMenuSeleccion() {
            const variables = fncGetParamsURL(window.location.href);
            if (variables != undefined) {
                $("#btnMenu_ProspectosClientes").removeClass("btn-success");
                $("#btnMenu_ProspectosClientes").addClass("btn-primary");
            }
        }

        function fncGetParamsURL(url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');
            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }
            return params;
        }
        //#endregion
    }

    $(document).ready(() => {
        ADMIN_FINANZAS.ProspectosClientes = new ProspectosClientes();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();