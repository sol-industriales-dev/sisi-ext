(() => {
    $.namespace('ADMIN_FINANZAS.TrackingVentas');

    TrackingVentas = function () {

        //#region CONST FILTROS
        const cboFiltro_TipoFiltro = $("#cboFiltro_TipoFiltro");
        const cboFiltro_Busqueda = $("#cboFiltro_Busqueda");
        const btnFiltro_Buscar = $("#btnFiltro_Buscar");
        const btnFiltro_Nuevo = $('#btnFiltro_Nuevo');

        const TipoFiltrosEnum = { CLIENTE: 1, DIVISION: 2, RESPONSABLE: 3, PRIORIDAD: 4 }
        //#endregion

        //#region CONST LISTADO TRACKING VENTAS
        let dtTrackingVentas;
        const tblTrackingVentas = $('#tblTrackingVentas');
        //#endregion

        //#region CONST CREAR/EDITAR PROYECTO
        const mdlCE_Proyecto = $("#mdlCE_Proyecto");
        const txtCE_Proyecto_NombreProyecto = $("#txtCE_Proyecto_NombreProyecto");
        const cboCE_Proyecto_Cliente = $("#cboCE_Proyecto_Cliente");
        const cboCE_Proyecto_Prioridad = $("#cboCE_Proyecto_Prioridad");
        const cboCE_Proyecto_Division = $("#cboCE_Proyecto_Division");
        const cboCE_Proyecto_Pais = $('#cboCE_Proyecto_Pais');
        const cboCE_Proyecto_Estado = $('#cboCE_Proyecto_Estado');
        const cboCE_Proyecto_Municipio = $('#cboCE_Proyecto_Municipio');
        const txtCE_Proyecto_Importe = $("#txtCE_Proyecto_Importe");
        const txtCE_Proyecto_FechaInicio = $("#txtCE_Proyecto_FechaInicio");
        const cboCE_Proyecto_Estatus = $("#cboCE_Proyecto_Estatus");
        const cboCE_Proyecto_Escenario = $("#cboCE_Proyecto_Escenario");
        const cboCE_Proyecto_Responsable = $("#cboCE_Proyecto_Responsable");
        const txtCE_Proyecto_DescripcionObra = $('#txtCE_Proyecto_DescripcionObra');
        const txtCE_Proyecto_Cliente = $("#txtCE_Proyecto_Cliente");
        const txtCE_Proyecto_Contacto = $("#txtCE_Proyecto_Contacto");
        const div_cboCE_Proyecto_Cliente = $("#div_cboCE_Proyecto_Cliente");
        const div_txtCE_Proyecto_Cliente = $("#div_txtCE_Proyecto_Cliente");
        const div_txtCE_Proyecto_Contacto = $("#div_txtCE_Proyecto_Contacto");
        const btnCE_Proyecto = $("#btnCE_Proyecto");
        const cboCE_Proyecto_Riesgo = $('#cboCE_Proyecto_Riesgo');

        const PrioridadEnum = { Prospeccion: 1, LaborDeVenta: 2, Cotizacion: 3, Negociacion: 4, Cierre: 5 }
        const EscenariosEnum = { A: 1, B: 2, C: 3, D: 4, E: 5, T: 6 };
        const EstatusEnum = { Ganado: 12, Perdido: 13 };
        //#endregion

        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblTrackingVentas();
            fncGetTrackingVentas();
            cboFiltro_TipoFiltro.fillCombo('FillCboTipoFiltros', null, false, null);
            $(".select2").select2();
            fncIndicarMenuSeleccion();
            //#endregion

            //#region FILTROS
            btnFiltro_Buscar.click(function () {
                fncGetTrackingVentas();
            });

            btnFiltro_Nuevo.click(function () {
                fncFillCbos();
                btnCE_Proyecto.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCE_Proyecto.data().id = 0;
                fncLimpiarMdlCE_Proyecto();
                cboCE_Proyecto_Cliente.data().mostrar = true;
                cboCE_Proyecto_Cliente.fillCombo('FillCboClientes', null, false, null);
                mdlCE_Proyecto.modal("show");
            });

            cboFiltro_TipoFiltro.change(function () {
                if ($(this).val() > 0) {
                    cboFiltro_Busqueda.fillCombo('FillCboTipoBusqueda', { tipoFiltroEnum: $(this).val() }, false, null);
                } else {
                    $("#cboFiltro_Busqueda option").remove();
                }
            });

            cboCE_Proyecto_Pais.change(function () {
                if ($(this).val() > 0) {
                    cboCE_Proyecto_Estado.fillCombo('FillCboEstados', { FK_Pais: $(this).val() }, false, null);
                }
            });

            cboCE_Proyecto_Estado.change(function () {
                if ($(this).val() > 0) {
                    cboCE_Proyecto_Municipio.fillCombo('FillCboMunicipios', { FK_Estado: $(this).val() }, false, null);
                }
            });

            $(".fncChangeCliente").click(function () {
                fncChangeCliente();
            });
            //#endregion

            //#region CREAR/EDITAR PROYECTO
            btnCE_Proyecto.click(function () {
                if ($(this).data().id <= 0) {
                    fncCrearProyecto();
                } else {
                    fncActualizarProyecto();
                }
            });

            cboCE_Proyecto_Prioridad.change(function () {
                if ($(this).val() > 0) {
                    cboCE_Proyecto_Estatus.fillCombo('FillCboPrioridadesEstatus', { FK_Prioridad: $(this).val() }, false, null);
                }
            });

            cboCE_Proyecto_Estatus.change(function () {
                if ($(this).val() > 0) {
                    fncSetEscenario(cboCE_Proyecto_Prioridad.val());
                }
            });
            //#endregion
        }

        //#region TRACKING VENTAS
        function initTblTrackingVentas() {
            dtTrackingVentas = tblTrackingVentas.DataTable({
                language: dtDicEsp,
                paging: true,
                ordering: true,
                searching: true,
                scrollX: true,
                columns: [
                    { data: 'division', title: 'División' },
                    { data: 'esc', title: 'Esc' },
                    { data: 'prioridad', title: 'Prioridad' },
                    { data: 'nombreCliente', title: 'Cliente' },
                    { data: 'nombreProyecto', title: 'Proyecto' },
                    { data: 'ubicacion', title: 'Ubicación' },
                    { data: 'estatusActual', title: 'Estatus actual' },
                    { data: 'proximaAccion', title: 'Proxima<br>acción' },
                    { data: 'strFechaProximaAccion', title: 'Fecha compromiso' },
                    { data: 'nombreResponsableProyecto', title: 'Responsable' },
                    { data: 'nombreResponsableAccion', title: 'Responsable<br>acción' },
                    { data: 'riesgo', title: 'Riesgo del<br>proyecto' },
                    { data: 'estatus', title: 'Estatus' },
                    { data: 'porcCumplimiento', title: '% Cumplimiento' },
                    { data: 'go', title: 'GO' },
                    { data: 'get', title: 'GET' },
                    {
                        title: 'Acciones',
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return `${btnEditar} ${btnEliminar}`
                        },
                    },
                ],
                initComplete: function (settings, json) {
                    tblTrackingVentas.on('click', '.editarRegistro', function () {
                        let rowData = dtTrackingVentas.row($(this).closest('tr')).data();
                        fncFillCbos();
                        fncGetDatosActualizarProyecto(rowData.id);
                    });

                    tblTrackingVentas.on('click', '.eliminarRegistro', function () {
                        let rowData = dtTrackingVentas.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el proyecto seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarProyecto(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetTrackingVentas() {
            //#region GET PARAMETROS BUSQUEDA
            let objParamsDTO = {};
            switch (+cboFiltro_TipoFiltro.val()) {
                case TipoFiltrosEnum.CLIENTE:
                    objParamsDTO.FK_Cliente = cboFiltro_Busqueda.val();
                    break;
                case TipoFiltrosEnum.DIVISION:
                    objParamsDTO.FK_Division = cboFiltro_Busqueda.val();
                    break;
                case TipoFiltrosEnum.RESPONSABLE:
                    objParamsDTO.FK_UsuarioResponsable = cboFiltro_Busqueda.val();
                    break;
                case TipoFiltrosEnum.PRIORIDAD:
                    objParamsDTO.FK_Prioridad = cboFiltro_Busqueda.val();
                    break;
                default:
                    break;
            }
            //#endregion
            axios.post('GetTrackingVentas', objParamsDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtTrackingVentas.clear();
                    dtTrackingVentas.rows.add(items);
                    dtTrackingVentas.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarProyecto(idProyecto) {
            if (idProyecto > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idProyecto;
                axios.post('EliminarProyecto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetTrackingVentas();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el proyecto.");
            }
        }

        function fncGetDatosActualizarProyecto(idProyecto) {
            if (idProyecto > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idProyecto;
                axios.post('GetDatosActualizarProyecto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnCE_Proyecto.data().id = idProyecto;
                        txtCE_Proyecto_NombreProyecto.val(items.nombreProyecto);
                        cboCE_Proyecto_Cliente.fillCombo('FillCboClientes', null, false, null);
                        cboCE_Proyecto_Cliente.val(items.FK_Cliente);
                        cboCE_Proyecto_Cliente.trigger("change");
                        cboCE_Proyecto_Prioridad.val(items.FK_Prioridad);
                        cboCE_Proyecto_Prioridad.trigger("change");
                        cboCE_Proyecto_Division.val(items.FK_Division);
                        cboCE_Proyecto_Division.trigger("change");
                        cboCE_Proyecto_Estatus.val(items.FK_Estatus);
                        cboCE_Proyecto_Estatus.trigger("change");
                        txtCE_Proyecto_Importe.val(maskNumero2DCompras(items.importeCotizadoAprox));
                        txtCE_Proyecto_FechaInicio.val(moment(items.fechaInicio).format('YYYY-MM-DD'));
                        cboCE_Proyecto_Escenario.val(items.FK_Escenario);
                        cboCE_Proyecto_Escenario.trigger("change");
                        cboCE_Proyecto_Pais.val(items.FK_Pais);
                        cboCE_Proyecto_Pais.trigger("change");
                        cboCE_Proyecto_Estado.val(items.FK_Estado);
                        cboCE_Proyecto_Estado.trigger("change");
                        cboCE_Proyecto_Municipio.val(items.FK_Municipio)
                        cboCE_Proyecto_Municipio.trigger("change");
                        cboCE_Proyecto_Responsable.val(items.FK_UsuarioResponsable);
                        cboCE_Proyecto_Responsable.trigger("change");
                        cboCE_Proyecto_Riesgo.val(items.FK_Riesgo)
                        cboCE_Proyecto_Riesgo.trigger("change");
                        txtCE_Proyecto_DescripcionObra.val(items.descripcionObra);
                        btnCE_Proyecto.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
                        mdlCE_Proyecto.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncChangeCliente() {
            if (cboCE_Proyecto_Cliente.data().mostrar) {
                div_cboCE_Proyecto_Cliente.css("display", "inline");
                div_txtCE_Proyecto_Cliente.css("display", "none");
                div_txtCE_Proyecto_Contacto.css("display", "none");
                cboCE_Proyecto_Cliente.data().mostrar = false;
                cboCE_Proyecto_Cliente.data().estaOculto = false;
            } else if (!cboCE_Proyecto_Cliente.data().mostrar) {
                div_cboCE_Proyecto_Cliente.css("display", "none");
                div_txtCE_Proyecto_Cliente.css("display", "inline");
                div_txtCE_Proyecto_Contacto.css("display", "inline");
                cboCE_Proyecto_Cliente.data().mostrar = true;
                cboCE_Proyecto_Cliente.data().estaOculto = true;
            }
        }

        function fncLimpiarMdlCE_Proyecto() {
            $("input[type='text']").val("");
            cboCE_Proyecto_Cliente[0].selectedIndex = 0;
            cboCE_Proyecto_Cliente.trigger("change");
            cboCE_Proyecto_Prioridad[0].selectedIndex = 0;
            cboCE_Proyecto_Prioridad.trigger("change");
            cboCE_Proyecto_Division[0].selectedIndex = 0;
            cboCE_Proyecto_Division.trigger("change");
            cboCE_Proyecto_Estatus[0].selectedIndex = 0;
            cboCE_Proyecto_Estatus.trigger("change");
            cboCE_Proyecto_Escenario[0].selectedIndex = 0;
            cboCE_Proyecto_Escenario.trigger("change");
            cboCE_Proyecto_Pais[0].selectedIndex = 0;
            cboCE_Proyecto_Pais.trigger("change");
            cboCE_Proyecto_Estado[0].selectedIndex = 0;
            cboCE_Proyecto_Estado.trigger("change");
            cboCE_Proyecto_Municipio[0].selectedIndex = 0;
            cboCE_Proyecto_Municipio.trigger("change");
            cboCE_Proyecto_Responsable[0].selectedIndex = 0;
            cboCE_Proyecto_Responsable.trigger("change");
            txtCE_Proyecto_DescripcionObra.val("");
        }
        //#endregion

        //#region PROYECTOS
        function fncCrearProyecto() {
            let objParamsDTO = fncCEOBJProyecto();
            if (objParamsDTO != "") {
                axios.post('CrearProyecto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetTrackingVentas();
                        mdlCE_Proyecto.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncActualizarProyecto() {
            let objParamsDTO = fncCEOBJProyecto();
            if (objParamsDTO != "") {
                axios.post('ActualizarProyecto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetTrackingVentas();
                        mdlCE_Proyecto.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEOBJProyecto() {
            fncDefaultCtrls("txtCE_Proyecto_NombreProyecto", false);
            fncDefaultCtrls("cboCE_Proyecto_Cliente", true);
            fncDefaultCtrls("txtCE_Proyecto_Cliente", false);
            fncDefaultCtrls("cboCE_Proyecto_Prioridad", true);
            fncDefaultCtrls("cboCE_Proyecto_Division", true);
            fncDefaultCtrls("cboCE_Proyecto_Estatus", true);
            fncDefaultCtrls("txtCE_Proyecto_Importe", false);
            fncDefaultCtrls("txtCE_Proyecto_FechaInicio", false);
            fncDefaultCtrls("cboCE_Proyecto_Escenario", true);
            fncDefaultCtrls("cboCE_Proyecto_Pais", true);
            fncDefaultCtrls("cboCE_Proyecto_Estado", true);
            fncDefaultCtrls("cboCE_Proyecto_Municipio", true);
            fncDefaultCtrls("cboCE_Proyecto_Responsable", true);
            fncDefaultCtrls("txtCE_Proyecto_Contacto", false);
            fncDefaultCtrls("cboCE_Proyecto_Riesgo", false);
            fncDefaultCtrls("txtCE_Proyecto_DescripcionObra", false);

            if (txtCE_Proyecto_NombreProyecto.val() == "") { fncValidacionCtrl("txtCE_Proyecto_NombreProyecto", false, "Es necesario indicar el nombre del proyecto."); return ""; }

            if (!cboCE_Proyecto_Cliente.data().estaOculto) {
                if (cboCE_Proyecto_Cliente.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Cliente", true, "Es necesario seleccionar un cliente."); return ""; }
            } else if (cboCE_Proyecto_Cliente.data().estaOculto) {
                if (txtCE_Proyecto_Cliente.val() == "") { fncValidacionCtrl("txtCE_Proyecto_Cliente", false, "Es necesario indicar el nombre del cliente nuevo."); return ""; }
                if (txtCE_Proyecto_Contacto.val() == "") { fncValidacionCtrl("txtCE_Proyecto_Contacto", false, "Es necesario indicar el nombre del contacto nuevo."); return ""; }
            }

            if (cboCE_Proyecto_Prioridad.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Prioridad", true, "Es necesario seleccionar la prioridad."); return ""; }
            if (cboCE_Proyecto_Division.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Division", true, "Es necesario seleccionar la división."); return ""; }
            if (cboCE_Proyecto_Estatus.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Estatus", true, "Es necesario seleccionar el estatus."); return ""; }
            if (txtCE_Proyecto_Importe.val() == "") { fncValidacionCtrl("txtCE_Proyecto_Importe", false, "Es necesario indicar el importe."); return ""; }
            if (txtCE_Proyecto_FechaInicio.val() == "") { fncValidacionCtrl("txtCE_Proyecto_FechaInicio", false, "Es necesario indicar la fecha de inicio."); return ""; }
            if (cboCE_Proyecto_Escenario.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Escenario", true, "Es necesario seleccionar el escenario."); return ""; }
            if (cboCE_Proyecto_Pais.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Pais", true, "Es necesario seleccionar el país."); return ""; }
            if (cboCE_Proyecto_Estado.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Estado", true, "Es necesario seleccionar el estado."); return ""; }
            if (cboCE_Proyecto_Municipio.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Municipio", true, "Es necesario seleccionar el municipio."); return ""; }
            if (cboCE_Proyecto_Responsable.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Responsable", true, "Es necesario seleccionar al responsable."); return ""; }
            if (cboCE_Proyecto_Riesgo.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Riesgo", true, "Es necesario seleccionar un riesgo."); return ""; }
            if (txtCE_Proyecto_DescripcionObra.val() == "") { fncValidacionCtrl("txtCE_Proyecto_DescripcionObra", false, "Es necesario indicar la descripción de la obra."); return ""; }

            let objParamsDTO = {};
            objParamsDTO.id = btnCE_Proyecto.data().id;
            objParamsDTO.nombreProyecto = txtCE_Proyecto_NombreProyecto.val();
            if (cboCE_Proyecto_Cliente.val() > 0) {
                objParamsDTO.FK_Cliente = cboCE_Proyecto_Cliente.val();
            } else {
                objParamsDTO.nombreCliente = txtCE_Proyecto_Cliente.val();
                objParamsDTO.nombreContacto = txtCE_Proyecto_Contacto.val();
                objParamsDTO.esCrearClienteDesdeProyectos = true;
            }
            objParamsDTO.FK_Prioridad = cboCE_Proyecto_Prioridad.val();
            objParamsDTO.FK_Division = cboCE_Proyecto_Division.val();
            objParamsDTO.FK_Municipio = cboCE_Proyecto_Municipio.val();
            objParamsDTO.importeCotizadoAprox = unmaskNumero(txtCE_Proyecto_Importe.val());
            objParamsDTO.fechaInicio = txtCE_Proyecto_FechaInicio.val();
            objParamsDTO.FK_Estatus = cboCE_Proyecto_Estatus.val();
            objParamsDTO.FK_Escenario = cboCE_Proyecto_Escenario.val();
            objParamsDTO.FK_UsuarioResponsable = cboCE_Proyecto_Responsable.val();
            objParamsDTO.FK_Riesgo = cboCE_Proyecto_Riesgo.val();
            objParamsDTO.descripcionObra = txtCE_Proyecto_DescripcionObra.val();
            return objParamsDTO;
        }

        function fncFillCbos() {
            cboCE_Proyecto_Prioridad.fillCombo('FillCboPrioridades', null, false, null);
            cboCE_Proyecto_Division.fillCombo('FillCboDivisiones', null, false, null);
            cboCE_Proyecto_Pais.fillCombo('FillCboPaises', null, false, null);
            cboCE_Proyecto_Escenario.fillCombo('FillCboEscenarios', null, false, null);
            cboCE_Proyecto_Responsable.fillCombo('FillCboResponsables', null, false, null);
            cboCE_Proyecto_Riesgo.fillCombo('FillCboRiesgos', null, false, null);
            cboFiltro_TipoFiltro.fillCombo('FillCboTipoFiltros', null, false, null);
            $(".select2").select2();
        }

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
        //#endregion

        //#region GENERALES
        function fncIndicarMenuSeleccion() {
            const variables = fncGetParamsURL(window.location.href);
            if (variables != undefined) {
                $("#btnMenu_TrackingVentas").removeClass("btn-success");
                $("#btnMenu_TrackingVentas").addClass("btn-primary");
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
        ADMIN_FINANZAS.TrackingVentas = new TrackingVentas();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();