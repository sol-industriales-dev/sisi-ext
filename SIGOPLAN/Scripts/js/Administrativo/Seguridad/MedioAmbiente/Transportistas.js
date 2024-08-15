(() => {
    $.namespace("CH.Transportistas");

    //#region CONST
    const btnCETransportistaModal = $("#btnCETransportistaModal");
    const tblS_MedioAmbienteTransportistas = $("#tblS_MedioAmbienteTransportistas");
    const mdlCETransportista = $("#mdlCETransportista");
    const lblTitleCETransportista = $("#lblTitleCETransportista");
    const txtRazonSocial = $("#txtRazonSocial");
    const txtNumAutorizacion = $("#txtNumAutorizacion");
    const cboClasificacion = $("#cboClasificacion");
    const btnCETransportista = $("#btnCETransportista");
    const titleBtnCETransportista = $("#titleBtnCETransportista");
    const btnCETransportistaClasificacionesModal = $("#btnCETransportistaClasificacionesModal");
    let dtTransportistas;
    const mdlCEClasificacion = $("#mdlCEClasificacion");
    const lblTitleCEClasificacion = $("#lblTitleCEClasificacion");
    const txtClasificacion = $("#txtClasificacion");
    const txtDescripcion = $("#txtDescripcion");
    const btnCEClasificacion = $("#btnCEClasificacion");
    const titleBtnCEClasificacion = $("#titleBtnCEClasificacion");
    const mdlListadoCEClasificaciones = $("#mdlListadoCEClasificaciones");
    const tblS_MedioAmbienteClasificacionesTransportistas = $("#tblS_MedioAmbienteClasificacionesTransportistas");
    const btnCEClasificacionesModal = $("#btnCEClasificacionesModal");
    let dtClasificaciones;
    //#endregion

    Transportistas = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            initTblTransportistas();
            initTblClasificaciones();
            //#endregion

            //#region EVENTS TRANSPORTISTAS
            fncGetTransportistas();

            btnCETransportistaModal.on("click", function (e) {
                lblTitleCETransportista.html("NUEVO REGISTRO");
                titleBtnCETransportista.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                btnCETransportista.attr("data-id", 0);
                fncLimpiarModalCETransportistas();
                fncBorderDefault();
                fncFillCboClasificacionesTransportistas();
                mdlCETransportista.modal("show");
            });

            btnCETransportista.on("click", function (e) {
                fncCrearEditarTransportista();
            });
            //#endregion

            //#region EVENTS CLASIFICACIONES
            btnCETransportistaClasificacionesModal.on("click", function (e) {
                fncGetClasificaciones();
                mdlListadoCEClasificaciones.modal("show");
            });

            btnCEClasificacionesModal.on("click", function (e) {
                fncLimpiarModalCETransportistas();
                fncBorderDefault();
                lblTitleCEClasificacion.html("NUEVO REGISTRO");
                titleBtnCEClasificacion.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                btnCEClasificacion.attr("data-id", 0);
                mdlCEClasificacion.modal("show");
            });

            btnCEClasificacion.on("click", function (e) {
                fncCrearEditarClasificacionTransportista();
            });
            //#endregion
        }

        //#region CRUD TRANSPORTISTAS
        function initTblTransportistas() {
            dtTransportistas = tblS_MedioAmbienteTransportistas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: "razonSocial", title: "Razón social" },
                    { data: "clasificacion", title: "Clasificación" },
                    { data: "numAutorizacion", title: "Número autorización" },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class="btn btn-xs btn-warning editarTransportista" 
                                                title="Editar transportista."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-xs btn-danger eliminarTransportista" title="Eliminar transportista"><i class="fas fa-trash"></i></button>`;
                            return btnEditar + btnEliminar;
                        },
                    },
                    { data: "idClasificacion", visible: false },
                    { data: "id", visible: false },
                ],
                initComplete: function (settings, json) {
                    tblS_MedioAmbienteTransportistas.on("click", ".editarTransportista", function () {
                        let rowData = dtTransportistas.row($(this).closest("tr")).data();
                        fncLimpiarModalCETransportistas();
                        fncBorderDefault();
                        lblTitleCETransportista.html("ACTUALIZAR REGISTRO");
                        txtRazonSocial.val(rowData.razonSocial);
                        cboClasificacion.val(rowData.idClasificacion);
                        cboClasificacion.trigger("change");
                        txtNumAutorizacion.val(rowData.numAutorizacion);
                        titleBtnCETransportista.html(
                            `<i class="fas fa-save"></i>&nbsp;Actualizar`
                        );
                        btnCETransportista.attr("data-id", rowData.id);
                        mdlCETransportista.modal("show");
                    });
                    tblS_MedioAmbienteTransportistas.on("click", ".eliminarTransportista", function () {
                        let rowData = dtTransportistas.row($(this).closest("tr")).data();
                        Alert2AccionConfirmar(
                            "¡Cuidado!",
                            "¿Desea eliminar el registro seleccionado?",
                            "Confirmar",
                            "Cancelar",
                            () => fncEliminarTransportista(rowData.id)
                        );
                    });
                },
                columnDefs: [
                    { className: "dt-center", targets: "_all" },
                    { width: '10%', targets: [1] }
                ],
            });
        }

        function fncGetTransportistas() {
            axios.post("GetTransportistas").then((response) => {
                let { success, items, message } = response.data;
                if (success) {
                    // #region FILL DATATABLE
                    dtTransportistas.clear();
                    dtTransportistas.rows.add(response.data.data);
                    dtTransportistas.draw();
                    // #endregion
                } else {
                    Alert2Error(message);
                }
            }).catch((error) => Alert2Error(error.message));
        }

        function fncCrearEditarTransportista() {
            let obj = fncObjCETransportista();
            if (obj != "") {
                axios.post("CrearEditarTransportista", obj).then((response) => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetTransportistas();
                        mdlCETransportista.modal("hide");
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch((error) => Alert2Error(error.message));
            }
        }

        function fncObjCETransportista() {
            fncBorderDefault();
            let strMensajeError = "";
            if (txtRazonSocial.val() == "") { txtRazonSocial.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtNumAutorizacion.val() == "") { txtNumAutorizacion.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboClasificacion.val() == "") { $("#select2-cboClasificacion-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            console.log(strMensajeError);
            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCETransportista.attr("data-id"),
                    razonSocial: txtRazonSocial.val(),
                    numAutorizacion: txtNumAutorizacion.val(),
                    idClasificacion: cboClasificacion.val(),
                };
                return obj;
            }
        }

        function fncEliminarTransportista(idTransportista) {
            if (idTransportista > 0) {
                let obj = new Object();
                obj = {
                    _idTransportista: idTransportista,
                };
                axios.post("EliminarTransportista", obj).then((response) => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetTransportistas();
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch((error) => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar al transportista.");
            }
        }

        function fncFillCboClasificacionesTransportistas() {
            cboClasificacion.fillCombo("FillCboClasificacionesTransportistas", {}, false);
            cboClasificacion.select2({ width: "100%" });
        }
        //#endregion

        //#region CRUD CLASIFICACIONES
        function initTblClasificaciones() {
            dtClasificaciones = tblS_MedioAmbienteClasificacionesTransportistas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: "clasificacion", title: "Clasificación" },
                    { data: "descripcion", title: "Descripción" },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class="btn btn-xs btn-warning editarClasificacion" 
                                            title="Editar clasificación."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-xs btn-danger eliminarClasificacion" title="Eliminar clasificacion"><i class="fas fa-trash"></i></button>`;
                            return btnEditar + btnEliminar;
                        },
                    },
                    { data: "id", visible: false },
                ],
                initComplete: function (settings, json) {
                    tblS_MedioAmbienteClasificacionesTransportistas.on("click", ".editarClasificacion", function () {
                        let rowData = dtClasificaciones
                            .row($(this).closest("tr"))
                            .data();
                        fncLimpiarModalCETransportistas();
                        fncBorderDefault();
                        lblTitleCEClasificacion.html("ACTUALIZAR REGISTRO");
                        titleBtnCEClasificacion.html(
                            `<i class="fas fa-save"></i>&nbsp;Actualizar`
                        );
                        btnCEClasificacion.attr("data-id", rowData.id);
                        fncBorderDefault();
                        fncLimpiarModalCETransportistas();
                        txtClasificacion.val(rowData.clasificacion);
                        txtDescripcion.val(rowData.descripcion);
                        mdlCEClasificacion.modal("show");
                    });
                    tblS_MedioAmbienteClasificacionesTransportistas.on("click", ".eliminarClasificacion", function () {
                        let rowData = dtClasificaciones
                            .row($(this).closest("tr"))
                            .data();
                        Alert2AccionConfirmar(
                            "¡Cuidado!",
                            "¿Desea eliminar el registro seleccionado?",
                            "Confirmar",
                            "Cancelar",
                            () => fncEliminarClasificacion(rowData.id)
                        );
                    });
                },
                columnDefs: [{ className: "dt-center", targets: "_all" }],
            });
        }

        function fncGetClasificaciones() {
            axios.post("GetClasificacionesTransportistas").then((response) => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtClasificaciones.clear();
                    dtClasificaciones.rows.add(response.data.data);
                    dtClasificaciones.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch((error) => Alert2Error(error.message));
        }

        function fncCrearEditarClasificacionTransportista() {
            let obj = fncObjCEClasificacion();
            if (obj != "") {
                axios.post("CrearEditarClasificacionTransportista", obj).then((response) => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetClasificaciones();
                        mdlCEClasificacion.modal("hide");
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch((error) => Alert2Error(error.message));
            }
        }

        function fncObjCEClasificacion() {
            fncBorderDefault();
            let strMensajeError = "";
            if (txtClasificacion.val() == "") { txtClasificacion.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEClasificacion.attr("data-id"),
                    clasificacion: txtClasificacion.val(),
                    descripcion: txtDescripcion.val(),
                };
                return obj;
            }
        }

        function fncEliminarClasificacion(idClasificacion) {
            if (idClasificacion > 0) {
                let obj = new Object();
                obj = {
                    _idClasificacionTransportista: idClasificacion,
                };
                axios.post("EliminarClasificacionTransportista", obj).then((response) => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetClasificaciones();
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch((error) => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar la clasificación.");
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncLimpiarModalCETransportistas() {
            $('input[type="text"]').val("");

            //#region TRANSPORTISTAS
            cboClasificacion[0].selectedIndex = 0;
            cboClasificacion.trigger("change");
            //#endregion

            //#region CLASIFICACIONES
            txtDescripcion.val("");
            //#endregion
        }

        function fncBorderDefault() {
            //#region TRANSPORTISTAS
            txtRazonSocial.css("border", "1px solid #CCC");
            txtNumAutorizacion.css("border", "1px solid #CCC");
            $("#select2-cboClasificacion-container").css("border", "1px solid #CCC");
            //#endregion

            //#region CLASIFICACIÓN
            txtClasificacion.css("border", "1px solid #CCC");
            txtDescripcion.css("border", "1px solid #CCC");
            //#endregion
        }
        //#endregion
    };

    $(document).ready(() => { CH.Transportistas = new Transportistas(); })
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();