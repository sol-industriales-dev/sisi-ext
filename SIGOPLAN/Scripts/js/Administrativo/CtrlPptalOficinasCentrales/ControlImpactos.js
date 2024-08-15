(() => {
    $.namespace('CtrlPptalOfCE.ControlImpactos');

    //#region CONST
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroNuevoControlImpactos = $('#btnFiltroNuevoControlImpactos');
    const tblAF_CtrlPptalOfCe_ControlImpactos = $('#tblAF_CtrlPptalOfCe_ControlImpactos');
    let dtControlImpactos;
    //#endregion

    //#region CONST MODAL CREAR/EDITAR CONTROL IMPACTOS
    const lblTitleCEControlImpacto = $('#lblTitleCEControlImpacto');
    const mdlCEControlImpacto = $('#mdlCEControlImpacto');
    const txtNoCuenta = $('#txtNoCuenta');
    const txtDescripcion = $("#txtDescripcion");
    const txtTotal12Meses = $("#txtTotal12Meses");
    const txtPromMes = $("#txtPromMes");
    const cboEstrategia = $("#cboEstrategia");
    const cboResponsableLiderCuenta = $('#cboResponsableLiderCuenta');
    const txtPorcAhorroEsperado = $("#txtPorcAhorroEsperado");
    const txtAhorroEsperadoAnual = $("#txtAhorroEsperadoAnual");
    const txtPosiblesAcciones = $("#txtPosiblesAcciones");
    const txtEstatus = $("#txtEstatus");
    const txtAccionesEstablecidas = $("#txtAccionesEstablecidas");
    const txtFechaImplementacion = $("#txtFechaImplementacion");
    const txtPendientes = $("#txtPendientes");
    const btnCEControlImpacto = $('#btnCEControlImpacto');
    const lblTitleBtnCEControlImpacto = $('#lblTitleBtnCEControlImpacto');
    //#endregion

    ControlImpactos = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            initTblControlImpactos();
            //#endregion

            //#region EVENTOS CONTROL IMPACTOS
            fncGetControlImpactos();

            btnFiltroNuevoControlImpactos.on("click", function () {
                fncLimpiarMdlCEControlImpacto();
                fncTitleMdlCEControlImpacto(true);
                mdlCEControlImpacto.modal("show");
            });

            btnCEControlImpacto.on("click", function () {
                fncCEControlImpacto();
            });

            btnFiltroBuscar.on("click", function () {
                fncGetControlImpactos();
            });
            //#endregion

            //#region FILL COMBOS
            $(".select2").select2();
            cboEstrategia.fillCombo("FillEstrategias", {}, false);
            cboEstrategia.select2({ width: "100%" });

            cboResponsableLiderCuenta.fillCombo("FillResponsablesCuentasLider", {}, false);
            cboResponsableLiderCuenta.select2({ width: "100%" });
            //#endregion
        }

        //#region CRUD CONTROL IMPACTOS
        function initTblControlImpactos() {
            dtControlImpactos = tblAF_CtrlPptalOfCe_ControlImpactos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'consecutivo', title: '#' },
                    { data: 'noCuenta', title: 'N° Cuenta' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'total12meses', title: 'Total 12 meses $MXN' },
                    { data: 'promedioMes', title: 'Promedio mes' },
                    { data: 'idEstrategia', title: 'Estrategia' },
                    { data: 'responsableCuentaLider', title: 'Responsable (Líder cuenta)' },
                    { data: 'porcAhorroEsperado', title: '% Ahorro esperado' },
                    { data: 'ahorroEsperadoAnual', title: 'Ahorro esperado anual' },
                    { data: 'posiblesAcciones', title: 'Posibles acciones' },
                    { data: 'estatus', title: 'Status' },
                    { data: 'accionesEstablecidas', title: 'Acciones establecidas' },
                    { 
                        data: 'fechaImplementacion', title: 'Fecha implementación',
                        render: function (data, type, row) {
                            if (data != null) {
                                return moment(data).format("DD/MM/YYYY");
                            } else {
                                return "";
                            }
                        }
                    },
                    { data: 'pendientes', title: 'Pendientes' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class="btn btn-warning editarRegistro" title="Editar registro."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-danger eliminarRegistro" title="Eliminar registro."><i class="fas fa-trash"></i></button>`;
                            return btnEditar + btnEliminar;
                        },
                    },
                    { data: 'id', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblAF_CtrlPptalOfCe_ControlImpactos.on('click','.editarRegistro', function () {
                        let rowData = dtControlImpactos.row($(this).closest('tr')).data();
                        fncGetDatosActualizarControlImpacto(rowData.id);
                    });
                    tblAF_CtrlPptalOfCe_ControlImpactos.on('click','.eliminarRegistro', function () {
                        let rowData = dtControlImpactos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncEliminarControlImpacto(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center','targets': '_all'}
                ],
            });
        }

        function fncGetControlImpactos() {
            axios.post("GetControlImpactos").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtControlImpactos.clear();
                    dtControlImpactos.rows.add(response.data.lstControlImpactos);
                    dtControlImpactos.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEControlImpacto() {
            let obj = fncObjCEControlImpacto();
            if (obj != "") {
                axios.post("CrearEditarControlImpacto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetControlImpactos();
                        mdlCEControlImpacto.modal("hide");
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCEControlImpacto() {
            fncBorderDefault();
            let strMensajeError = "";
            if (txtNoCuenta.val() == "") { txtNoCuenta.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboResponsableLiderCuenta.val() == "") { $("#select2-cboResponsableLiderCuenta-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEControlImpacto.attr("data-id"),
                    noCuenta: txtNoCuenta.val(),
                    descripcion: txtDescripcion.val(),
                    total12meses: txtTotal12Meses.val(),
                    promedioMes: txtPromMes.val(),
                    idEstrategia: cboEstrategia.val(),
                    idResponsableLiderCuenta: cboResponsableLiderCuenta.val(),
                    porcAhorroEsperado: txtPorcAhorroEsperado.val(),
                    ahorroEsperadoAnual: txtAhorroEsperadoAnual.val(),
                    posiblesAcciones: txtPosiblesAcciones.val(),
                    estatus: txtEstatus.val(),
                    accionesEstablecidas: txtAccionesEstablecidas.val(),
                    fechaImplementacion: txtFechaImplementacion.val(),
                    pendientes: txtPendientes.val()
                };
                return obj;
            }
        }

        function fncEliminarControlImpacto(idControlImpacto) {
            if (idControlImpacto > 0) {
                let obj = new Object();
                obj = {
                    idControlImpacto: idControlImpacto
                }
                axios.post("EliminarControlImpacto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetControlImpactos();
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el registro seleccionado.");
            }
        }

        function fncGetDatosActualizarControlImpacto(idControlImpacto) {
            if (idControlImpacto > 0) {
                let obj = new Object();
                obj = {
                    idControlImpacto: idControlImpacto
                }
                axios.post("GetDatosActualizarControlImpacto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnCEControlImpacto.attr("data-id", idControlImpacto);
                        txtNoCuenta.val(response.data.objControlImpacto.noCuenta);
                        txtDescripcion.val(response.data.objControlImpacto.descripcion);
                        txtTotal12Meses.val(response.data.objControlImpacto.total12meses);
                        txtPromMes.val(response.data.objControlImpacto.promedioMes);
                        cboEstrategia.val(response.data.objControlImpacto.idEstrategia);
                        cboEstrategia.trigger("change");
                        cboResponsableLiderCuenta.val(response.data.objControlImpacto.idResponsableLiderCuenta);
                        cboResponsableLiderCuenta.trigger("change");
                        txtPorcAhorroEsperado.val(response.data.objControlImpacto.porcAhorroEsperado);
                        txtAhorroEsperadoAnual.val(response.data.objControlImpacto.ahorroEsperadoAnual);
                        txtPosiblesAcciones.val(response.data.objControlImpacto.posiblesAcciones);
                        txtEstatus.val(response.data.objControlImpacto.estatus);
                        txtAccionesEstablecidas.val(response.data.objControlImpacto.accionesEstablecidas);
                        txtFechaImplementacion.val(moment(response.data.objControlImpacto.fechaImplementacion).format("YYYY-MM-DD"));
                        txtPendientes.val(response.data.objControlImpacto.pendientes);
                        fncTitleMdlCEControlImpacto(false);
                        mdlCEControlImpacto.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener la información.")
            }
        }

        function fncTitleMdlCEControlImpacto(esCrear) {
            if (esCrear) {
                lblTitleCEControlImpacto.html("Nuevo registro");
                lblTitleBtnCEControlImpacto.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCEControlImpacto.attr("data-id", 0);
            } else {
                lblTitleCEControlImpacto.html("Actualizar registro");
                lblTitleBtnCEControlImpacto.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncLimpiarMdlCEControlImpacto() {
            $('input[type="text"]').val("");

            cboResponsableLiderCuenta[0].selectedIndex = 0;
            cboResponsableLiderCuenta.trigger("change");
            txtDescripcion.val("");
            txtPendientes.val("");
        }

        function fncBorderDefault() {
            txtNoCuenta.css("border", "1px solid #CCC");
            $("#select2-cboResponsableLiderCuenta-container").css("border", "1px solid #CCC");
        }
        //#endregion
    }

    $(document).ready(() => {
        CtrlPptalOfCE.ControlImpactos = new ControlImpactos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();