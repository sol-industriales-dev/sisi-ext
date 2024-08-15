(() => {
    $.namespace('ADMIN_FINANZAS.Canales');

    Canales = function () {

        //#region CONST FILTROS
        const btnFiltro_Buscar = $("#btnFiltro_Buscar");
        const btnFiltro_Nuevo = $("#btnFiltro_Nuevo");
        //#endregion

        //#region LISTADO CANALES
        let dtCanales;
        const tblCanales = $('#tblCanales');
        //#endregion

        //#region CREAR/EDITAR CANALES
        const mdlCE_Canal = $("#mdlCE_Canal");
        const txtCE_Canal = $("#txtCE_Canal");
        const btnCE_Canal = $("#btnCE_Canal");
        //#endregion

        //#region CONST FILTROS CANALES DIVISIONES
        const btnFiltro_CanalesDivisiones_Nuevo = $('#btnFiltro_CanalesDivisiones_Nuevo');
        //#endregion

        //#region CONST LISTADO CANALES DIVISIONES
        let dtCanalesDivisiones;
        const mdlListado_CanalesDivisiones = $('#mdlListado_CanalesDivisiones');
        const tblCanalesDivisiones = $('#tblCanalesDivisiones');
        //#endregion

        //#region CONST FILTROS CREAR CANALES DIVISIONES
        const mdlCE_CanalesDivisiones = $("#mdlCE_CanalesDivisiones");
        const cboCE_CanalesDivisiones_Canal = $("#cboCE_CanalesDivisiones_Canal");
        const cboCE_CanalesDivisiones_Division = $("#cboCE_CanalesDivisiones_Division");
        const btnCE_CanalesDivisiones_CanalDivision = $("#btnCE_CanalesDivisiones_CanalDivision");
        //#endregion

        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            // fncVerificarAccesosMenu();
            initTblCanales();
            initTblCanalesDivisiones();
            fncGetCanales();
            fncIndicarMenuSeleccion();
            //#endregion

            //#region FILTROS
            btnFiltro_Buscar.click(function () {
                fncGetCanales();
            });

            btnFiltro_Nuevo.click(function () {
                btnCE_Canal.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCE_Canal.data().id = 0;
                fncLimpiarMdlCE_Canal();
                mdlCE_Canal.modal("show");
            });
            //#endregion

            //#region CANALES
            btnCE_Canal.click(function () {
                if ($(this).data().id <= 0) {
                    fncCrearCanal();
                } else {
                    fncActualizarCanal();
                }
            });
            //#endregion

            //#region FILTROS CANALES DIVISIONES
            btnFiltro_CanalesDivisiones_Nuevo.click(function () {
                fncLimpiarMdlCE_CanalDivision();
                fncFillCbos();
                cboCE_CanalesDivisiones_Canal.attr("disabled", false);
                cboCE_CanalesDivisiones_Canal.val(btnCE_CanalesDivisiones_CanalDivision.data().FK_Canal);
                cboCE_CanalesDivisiones_Canal.trigger("change");
                cboCE_CanalesDivisiones_Canal.attr("disabled", true);
                mdlCE_CanalesDivisiones.modal("show");
            });
            //#endregion

            //#region CANALES DIVISIONES
            btnCE_CanalesDivisiones_CanalDivision.click(function () {
                fncCrearCanalDivision();
            });
            //#endregion
        }

        //#region CANALES
        function initTblCanales() {
            dtCanales = tblCanales.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'canal', title: 'Canal' },
                    { data: 'htmlDivisiones', title: 'Divisiones' },
                    {
                        render: function (data, type, row, meta) {
                            let btnListadoDivisiones = `<button class='btn btn-xs btn-primary listadoCanalesDivisiones' title='Listado divisiones.'><i class='fas fa-list'></i></button>`;
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return `${btnListadoDivisiones} ${btnEditar} ${btnEliminar}`;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblCanales.on('click', '.editarRegistro', function () {
                        let rowData = dtCanales.row($(this).closest('tr')).data();
                        fncGetDatosActualizarCanal(rowData.id);
                    });

                    tblCanales.on('click', '.eliminarRegistro', function () {
                        let rowData = dtCanales.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCanal(rowData.id));
                    });

                    tblCanales.on('click', '.listadoCanalesDivisiones', function () {
                        let rowData = dtCanales.row($(this).closest('tr')).data();
                        btnCE_CanalesDivisiones_CanalDivision.data().FK_Canal = rowData.id;
                        fncGetCanalesDivisiones();
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetCanales() {
            axios.post('GetCanales').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtCanales.clear();
                    dtCanales.rows.add(items);
                    dtCanales.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearCanal() {
            let objParamsDTO = fncCEOBJCanal();
            if (objParamsDTO != "") {
                axios.post('CrearCanal', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetCanales();
                        mdlCE_Canal.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncActualizarCanal() {
            let objParamsDTO = fncCEOBJCanal();
            if (objParamsDTO != "") {
                axios.post('ActualizarCanal', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetCanales();
                        mdlCE_Canal.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEOBJCanal() {
            fncDefaultCtrls("txtCE_Canal", false);

            if ($.trim(txtCE_Canal.val()) == "") { fncValidacionCtrl("txtCE_Canal", false, "Es necesario indicar el canal."); return ""; }

            let objParamsDTO = {};
            objParamsDTO.id = btnCE_Canal.data().id;
            objParamsDTO.canal = txtCE_Canal.val();
            return objParamsDTO;
        }

        function fncEliminarCanal(idCanal) {
            if (idCanal > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idCanal;
                axios.post('EliminarCanal', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetCanales();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el canal.");
            }
        }

        function fncGetDatosActualizarCanal(idCanal) {
            if (idCanal > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idCanal;
                axios.post('GetDatosActualizarCanal', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncLimpiarMdlCE_Canal();
                        txtCE_Canal.val(items.canal);
                        btnCE_Canal.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                        btnCE_Canal.data().id = idCanal;
                        mdlCE_Canal.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncLimpiarMdlCE_Canal() {
            $("input[type='text']").val("");
        }
        //#endregion

        //#region CANALES DIVISIONES
        function initTblCanalesDivisiones() {
            dtCanalesDivisiones = tblCanalesDivisiones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'canal', title: 'Canal' },
                    { data: 'division', title: 'Division' },
                    {
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblCanalesDivisiones.on('click', '.eliminarRegistro', function () {
                        let rowData = dtCanalesDivisiones.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCanalDivision(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetCanalesDivisiones() {
            if (btnCE_CanalesDivisiones_CanalDivision.data().FK_Canal > 0) {
                let objParamsDTO = {};
                objParamsDTO.FK_Canal = btnCE_CanalesDivisiones_CanalDivision.data().FK_Canal;
                axios.post('GetCanalesDivisiones', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtCanalesDivisiones.clear();
                        dtCanalesDivisiones.rows.add(items);
                        dtCanalesDivisiones.draw();
                        mdlListado_CanalesDivisiones.modal("show");
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener el listado de divisiones del canal.");
            }
        }

        function fncCrearCanalDivision() {
            let objParamsDTO = fncCEOBJCanalDivision();
            if (objParamsDTO != "") {
                axios.post('CrearCanalDivision', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetCanales();
                        fncGetCanalesDivisiones();
                        mdlCE_CanalesDivisiones.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEOBJCanalDivision() {
            fncDefaultCtrls("cboCE_CanalesDivisiones_Canal", true);
            fncDefaultCtrls("cboCE_CanalesDivisiones_Division", true);

            if (cboCE_CanalesDivisiones_Canal.val() <= 0) { fncValidacionCtrl("cboCE_CanalesDivisiones_Canal", true, "Ocurrió un error al registrar la división en el canal."); return ""; }
            if (cboCE_CanalesDivisiones_Division.val() <= 0) { fncValidacionCtrl("cboCE_CanalesDivisiones_Division", true, "Es necesario seleccionar una división."); return ""; }

            let objParamsDTO = {};
            objParamsDTO.id = btnCE_CanalesDivisiones_CanalDivision.data().id;
            objParamsDTO.FK_Canal = cboCE_CanalesDivisiones_Canal.val();
            objParamsDTO.FK_Division = cboCE_CanalesDivisiones_Division.val();
            return objParamsDTO;
        }

        function fncEliminarCanalDivision(idCanalDivision) {
            if (idCanalDivision > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idCanalDivision;
                axios.post('EliminarCanalDivision', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetCanalesDivisiones();
                        fncGetCanales();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar la división del canal.");
            }
        }

        function fncLimpiarMdlCE_CanalDivision() {
            cboCE_CanalesDivisiones_Canal[0].selectedIndex = 0;
            cboCE_CanalesDivisiones_Canal.trigger("change");
            cboCE_CanalesDivisiones_Division[0].selectedIndex = 0;
            cboCE_CanalesDivisiones_Division.trigger("change");
        }

        function fncFillCbos() {
            cboCE_CanalesDivisiones_Canal.fillCombo('FillCboCanales', null, false, null);
            cboCE_CanalesDivisiones_Division.fillCombo('FillCboDivisiones', null, false, null);
            $(".select2").select2();
        }
        //#endregion

        //#region GENERALES
        function fncIndicarMenuSeleccion() {
            const variables = fncGetParamsURL(window.location.href);
            if (variables != undefined) {
                $("#btnMenu_Canales").removeClass("btn-success");
                $("#btnMenu_Canales").addClass("btn-primary");
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
        ADMIN_FINANZAS.Canales = new Canales();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();