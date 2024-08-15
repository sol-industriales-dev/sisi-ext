(() => {
    $.namespace('ADMIN_FINANZAS.UsuariosCRM');

    UsuariosCRM = function () {

        //#region CONST FILTROS
        const btnFiltro_Buscar = $('#btnFiltro_Buscar');
        const btnFiltro_Nuevo = $('#btnFiltro_Nuevo');
        //#endregion

        //#region CONST USUARIOS CRM
        let dtUsuariosCRM;
        const tblUsuariosCRM = $('#tblUsuariosCRM');
        //#endregion

        //#region CONST CREAR/EDITAR USUARIO CRM
        const mdlCrear_UsuarioCRM = $("#mdlCrear_UsuarioCRM");
        const cboCE_UsuarioCRM = $("#cboCE_UsuarioCRM");
        const cboCE_Menu = $("#cboCE_Menu");
        const lblCantPuestosSeleccionados = $('#lblCantPuestosSeleccionados');
        const btnCE_UsuarioCRM = $("#btnCE_UsuarioCRM");
        //#endregion

        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblUsuariosCRM();
            fncGetUsuariosCRM();
            cboCE_UsuarioCRM.fillCombo('FillCboUsuarios', null, false, null);
            cboCE_Menu.fillCombo('FillCboMenus', null, false, 'Todos');
            $(".select2").select2();
            fncIndicarMenuSeleccion();
            //#endregion

            //#region FILTROS USUARIOS CRM
            btnFiltro_Buscar.click(function () {
                fncGetUsuariosCRM();
            });

            btnFiltro_Nuevo.click(function () {
                fncLimpiarMdlCE_Usuario();
                btnCE_UsuarioCRM.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCE_UsuarioCRM.data().id = 0;
                mdlCrear_UsuarioCRM.modal("show");
            });
            //#endregion

            //#region CREAR/EDITAR USUARIOS CRM
            btnCE_UsuarioCRM.click(function () {
                if ($(this).data().id <= 0) {
                    fncCrearUsuarioCRM();
                } else {
                    fncActualizarUsuarioCRM();
                }
            });

            cboCE_Menu.change(function () {
                lblCantPuestosSeleccionados.html(`| seleccionado: ${$("#cboCE_Menu :selected").length}`);
            });
            //#endregion
        }

        //#region USUARIOS CRM
        function initTblUsuariosCRM() {
            dtUsuariosCRM = tblUsuariosCRM.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreUsuario', title: 'Usuario' },
                    { data: 'htmlMenus', title: 'Menus' },
                    {
                        title: 'Acciones',
                        render: (data, type, row) => {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return `${btnEditar} ${btnEliminar}`
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblUsuariosCRM.on('click', '.editarRegistro', function () {
                        let rowData = dtUsuariosCRM.row($(this).closest('tr')).data();
                        fncGetDatosActualizarUsuarioCRM(rowData.id);
                    });

                    tblUsuariosCRM.on('click', '.eliminarRegistro', function () {
                        let rowData = dtUsuariosCRM.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarUsuarioCRM(rowData.FK_Usuario));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetUsuariosCRM() {
            axios.post('GetUsuariosCRM').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtUsuariosCRM.clear();
                    dtUsuariosCRM.rows.add(items);
                    dtUsuariosCRM.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearUsuarioCRM() {
            let objParamsDTO = fncCEOBJUsuarioCRM();
            if (objParamsDTO != "") {
                axios.post('CrearUsuarioCRM', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetUsuariosCRM();
                        mdlCrear_UsuarioCRM.modal("hide");
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEOBJUsuarioCRM() {
            fncDefaultCtrls("cboCE_UsuarioCRM", true);
            fncDefaultCtrls("cboCE_Menu", true);

            if (cboCE_UsuarioCRM.val() <= 0) { fncValidacionCtrl("cboCE_UsuarioCRM", true, "Es necesario seleccionar un usuario."); return ""; }
            if (cboCE_Menu.val() <= 0) { fncValidacionCtrl("cboCE_Menu", true, "Es necesario seleccionar al menos un menú."); return ""; }

            let objParamsDTO = {};
            objParamsDTO.id = btnCE_UsuarioCRM.data().id;
            objParamsDTO.FK_Usuario = cboCE_UsuarioCRM.val();
            objParamsDTO.lstFK_Menu = cboCE_Menu.val();
            return objParamsDTO;
        }

        function fncEliminarUsuarioCRM(FK_Usuario) {
            if (FK_Usuario > 0) {
                let objParamsDTO = {};
                objParamsDTO.FK_Usuario = FK_Usuario;
                axios.post('EliminarUsuarioCRM', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetUsuariosCRM();
                        mdlCrear_UsuarioCRM.modal("hide");
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el usuario.");
            }
        }

        function fncLimpiarMdlCE_Usuario() {
            cboCE_UsuarioCRM.val(null).trigger("change");
            cboCE_Menu.val(null).trigger('change');
            cboCE_Menu.find('option').each(function () {
                if ($(this).val() === "Todos") {
                    $(this).remove();
                }
            });
        }

        function fncIndicarMenuSeleccion() {
            const variables = fncGetParamsURL(window.location.href);
            if (variables != undefined) {
                $("#btnMenu_Usuarios").removeClass("btn-success");
                $("#btnMenu_Usuarios").addClass("btn-primary");
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
        ADMIN_FINANZAS.UsuariosCRM = new UsuariosCRM();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();