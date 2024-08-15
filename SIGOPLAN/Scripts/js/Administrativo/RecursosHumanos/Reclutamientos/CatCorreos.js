(() => {
    $.namespace('CH.CatCorreos');

    //#region CONST CORREO
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroNuevoCorreo = $('#btnFiltroNuevoCorreo');
    const tblRH_REC_CatCorreos = $('#tblRH_REC_CatCorreos');
    const mdlCrearEditarCorreo = $('#mdlCrearEditarCorreo');
    const spanTitleCECorreo = $('#spanTitleCECorreo');
    const txtCECorreoNombre = $('#txtCECorreoNombre');
    const txtCECorreo = $('#txtCECorreo');
    const btnCECorreo = $('#btnCECorreo');
    const spanBtnCECorreo = $('#spanBtnCECorreo');
    let dtCorreos;
    //#endregion

    CatCorreos = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            initTblCorreos();
            //#endregion

            //#region EVENTOS CORREOS
            fncGetCorreos();

            btnFiltroBuscar.on("click", function () {
                fncGetCorreos();
            });

            btnFiltroNuevoCorreo.on("click", function () {
                spanTitleCECorreo.html("Guardar");
                spanBtnCECorreo.html("Guardar");
                btnCECorreo.attr("data-id", 0);
                fncLimpiarCtrlsMdl();
                mdlCrearEditarCorreo.modal("show");
            });

            btnCECorreo.on("click", function () {
                fncCrearEditarCorreo();
            });
            //#endregion
        }

        //#region CRUD CORREOS
        function initTblCorreos() {
            dtCorreos = tblRH_REC_CatCorreos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'correo', title: 'Correo' },
                    {
                        render: function (data, type, row) {
                            let btnActualizar = `<button class="btn btn-warning actualizarCorreo btn-xs"><i class="far fa-edit"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-danger eliminarCorreo btn-xs"><i class="far fa-trash-alt"></i></button>`;
                            return btnActualizar + btnEliminar;
                        }
                    },
                    { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_CatCorreos.on('click','.actualizarCorreo', function () {
                        let rowData = dtCorreos.row($(this).closest('tr')).data();
                        txtCECorreoNombre.val(rowData.nombre);
                        txtCECorreo.val(rowData.correo);
                        spanTitleCECorreo.html("Actualizar");
                        spanBtnCECorreo.html("Actualizar");
                        btnCECorreo.attr("data-id", rowData.id);
                        mdlCrearEditarCorreo.modal("show");
                    });
                    tblRH_REC_CatCorreos.on('click','.eliminarCorreo', function () {
                        let rowData = dtCorreos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncEliminarCorreo(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center','targets': '_all'}
                ],
            });
        }

        function fncGetCorreos() {
            axios.post("GetCorreos").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtCorreos.clear();
                    dtCorreos.rows.add(response.data.lstCatCorreos);
                    dtCorreos.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarCorreo() {
            let obj = fncObjCECorreo();
            if (obj != "") {
                axios.post("CrearEditarCorreo", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        if (btnCECorreo.attr("data-id") > 0) {
                            Alert2Exito("Se ha actualizado con éxito el correo.");
                        } else {
                            Alert2Exito("Se ha registrado con éxito el correo.");
                        }
                        fncGetCorreos();
                        mdlCrearEditarCorreo.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCECorreo() {
            fncBorderDefault();
            let strMensajeError = "";

            if (txtCECorreo.val() == "") { txtCECorreo.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCECorreo.attr("data-id"),
                    nombre: txtCECorreoNombre.val(),
                    correo: txtCECorreo.val()
                }
                return obj;
            }
        }

        function fncLimpiarCtrlsMdl() {
            $('input[type="text"]').val('');
        }


        function fncBorderDefault() {
            txtCECorreo.css('border', '1px solid #CCC');
        }

        function fncEliminarCorreo(idCorreo) {
            let obj = new Object();
            obj = {
                idCorreo: idCorreo
            }
            axios.post("EliminarCorreo", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha eliminado con éxito el registro.");
                    fncGetCorreos();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.CatCorreos = new CatCorreos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();