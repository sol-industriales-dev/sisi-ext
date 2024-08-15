(() => {
    $.namespace('OTROS_SERVICIOS.Facultamientos');

    Facultamientos = function () {

        //#region CONST FILTROS
        const btnFiltroBuscar = $('#btnFiltroBuscar');
        const btnFiltroNuevo = $('#btnFiltroNuevo');
        //#endregion

        //#region CONST CREAR/EDITAR FACULTAMIENTOS
        let dtFacultamientos;
        const tblFacultamientos = $('#tblFacultamientos');
        const mdlCEFacultamiento = $("#mdlCEFacultamiento");
        const cboCE_Usuario = $("#cboCE_Usuario");
        const cboCE_Facultamiento = $("#cboCE_Facultamiento");
        const btnCE_Facultamiento = $("#btnCE_Facultamiento");
        //#endregion

        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            initTblFacultamientos();
            fncGetFacultamientos();
            $(".select2").select2();

            //#region FILTROS
            btnFiltroBuscar.click(function () {
                fncGetFacultamientos();
            });

            btnFiltroNuevo.click(function () {
                fncLimpiarModalCEFacultamiento();
                btnCE_Facultamiento.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCE_Facultamiento.data().id = 0;
                cboCE_Usuario.attr("disabled", false);
                mdlCEFacultamiento.modal("show");
            });
            //#endregion

            //#region CREAR/EDITAR FACULTAMIENTO
            cboCE_Usuario.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Facultamiento.fillCombo('FillCboTipoFacultamientos', null, false, null);

            btnCE_Facultamiento.click(function () {
                if ($(this).data().id <= 0) {
                    fncCrearFacultamiento();
                } else {
                    fncActualizarFacultamiento();
                }
            });
            //#endregion
        }

        function initTblFacultamientos() {
            dtFacultamientos = tblFacultamientos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreCompleto', title: 'Usuario' },
                    { data: 'facultamiento', title: 'Facultamiento' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return `${btnEditar} ${btnEliminar}`
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblFacultamientos.on('click', '.editarRegistro', function () {
                        let rowData = dtFacultamientos.row($(this).closest('tr')).data();
                        fncGetDatosActualizarFacultamiento(rowData.id);
                    });

                    tblFacultamientos.on('click', '.eliminarRegistro', function () {
                        let rowData = dtFacultamientos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarFacultamiento(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetFacultamientos() {
            axios.post('GetFacultamientos').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtFacultamientos.clear();
                    dtFacultamientos.rows.add(items);
                    dtFacultamientos.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearFacultamiento() {
            let objParamsDTO = fncOBJCEFacultamiento();
            if (objParamsDTO != "") {
                axios.post('CrearFacultamiento', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetFacultamientos();
                        mdlCEFacultamiento.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncActualizarFacultamiento() {
            let objParamsDTO = fncOBJCEFacultamiento();
            if (objParamsDTO != "") {
                axios.post('ActualizarFacultamiento', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetFacultamientos();
                        mdlCEFacultamiento.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncOBJCEFacultamiento() {
            let error = false;

            fncDefaultCtrls("cboCE_Usuario", true);
            fncDefaultCtrls("cboCE_Facultamiento", true);

            if (cboCE_Usuario.val() <= 0) { fncValidacionCtrl("cboCE_Usuario", true, "Es necesario seleccionar un usuario."); return ""; }
            if (cboCE_Facultamiento.val() <= 0) { fncValidacionCtrl("cboCE_Facultamiento", true, "Es necesario seleccionar un facultamiento."); return ""; }

            let objParamsDTO = {};
            objParamsDTO.id = btnCE_Facultamiento.data().id;
            objParamsDTO.FK_Usuario = cboCE_Usuario.val();
            objParamsDTO.tipoFacultamiento = cboCE_Facultamiento.val();
            return objParamsDTO;
        }

        function fncGetDatosActualizarFacultamiento(id) {
            if (id > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = id;
                axios.post('GetDatosActualizarFacultamiento', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnCE_Facultamiento.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
                        btnCE_Facultamiento.data().id = id;
                        cboCE_Usuario.val(items.FK_Usuario);
                        cboCE_Usuario.trigger("change");
                        cboCE_Usuario.attr("disabled", true);
                        cboCE_Facultamiento.val(items.tipoFacultamiento);
                        cboCE_Facultamiento.trigger("change");
                        mdlCEFacultamiento.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener la información del facultamiento a actualizar.");
            }
        }

        function fncEliminarFacultamiento(id) {
            if (id > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = id;
                axios.post('EliminarFacultamiento', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetFacultamientos();
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el facultamiento.");
            }
        }

        function fncLimpiarModalCEFacultamiento() {
            cboCE_Usuario[0].selectedIndex = 0;
            cboCE_Usuario.trigger("change");
            cboCE_Facultamiento[0].selectedIndex = 0;
            cboCE_Facultamiento.trigger("change");
        }
    }

    $(document).ready(() => {
        OTROS_SERVICIOS.Facultamientos = new Facultamientos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();