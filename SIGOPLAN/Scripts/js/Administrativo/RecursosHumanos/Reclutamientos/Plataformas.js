(() => {
    $.namespace('CH.Plataformas');

    //#region CONST PLATAFORMAS
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnShowMdlCEPlataforma = $('#btnShowMdlCEPlataforma');
    const tblRH_REC_CatPlataformas = $('#tblRH_REC_CatPlataformas');
    let dtPlataformas;
    const mdlCEPlataformas = $('#mdlCEPlataformas');
    const lblTitleCEPlataforma = $('#lblTitleCEPlataforma');
    const lblTitleBtnCEPlataforma = $('#lblTitleBtnCEPlataforma');
    const txtCEPlataforma = $('#txtCEPlataforma');
    const btnCEPlataforma = $('#btnCEPlataforma');
    //#endregion

    Plataformas = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLE
            initTblPlataformas();
            //#endregion

            //#region EVENTOS PLATAFORMAS
            fncGetPlataformas();

            btnFiltroBuscar.on("click", function () {
                fncGetPlataformas();
            });

            btnShowMdlCEPlataforma.on("click", function () {
                fncLimpiarMdlCEPlataformas();
                lblTitleCEPlataforma.html("Guardar plataforma");
                lblTitleBtnCEPlataforma.html("Guardar");
                btnCEPlataforma.attr("data-id", 0);
                txtCEPlataforma.val("");
                mdlCEPlataformas.modal("show");
            });

            btnCEPlataforma.on("click", function () {
                fncCEPlataforma();
            });
            //#endregion
        }
        
        //#region CRUD PLATAFORMAS
        function initTblPlataformas() {
            dtPlataformas = tblRH_REC_CatPlataformas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'plataforma', title: 'PLATAFORMA' },
                    {
                        render: function (data, type, row) {
                            let btnActualizar = `<button class="btn btn-warning actualizarPlataforma btn-xs"><i class="far fa-edit"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-danger eliminarPlataforma btn-xs"><i class="far fa-trash-alt"></i></button>`;
                            return btnActualizar + btnEliminar;
                        }
                    },
                    { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_CatPlataformas.on('click','.actualizarPlataforma', function () {
                        let rowData = dtPlataformas.row($(this).closest('tr')).data();
                        fncLimpiarMdlCEPlataformas();
                        txtCEPlataforma.val(rowData.plataforma);
                        btnCEPlataforma.attr("data-id", rowData.id);
                        lblTitleCEPlataforma.html("Actualizar plataforma");
                        lblTitleBtnCEPlataforma.html("Actualizar");
                        mdlCEPlataformas.modal("show");
                    });

                    tblRH_REC_CatPlataformas.on('click','.eliminarPlataforma', function () {
                        let rowData = dtPlataformas.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncEliminarPlataforma(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center','targets': '_all'}
                ],
            });
        }

        function fncGetPlataformas() {
            axios.post("GetPlataformas").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtPlataformas.clear();
                    dtPlataformas.rows.add(response.data.lstPlataformas);
                    dtPlataformas.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEPlataforma() {
            let obj = fncObjCEPlataforma();
            if (obj != "") {
                axios.post("CrearEditarPlataforma", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito("Se ha registrado con éxito la plataforma.");
                        fncGetPlataformas();
                        mdlCEPlataformas.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCEPlataforma() {
            fncBorderDefault();

            let strMensajeError = ""; 
            if (txtCEPlataforma.val() == "") { txtCEPlataforma.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEPlataforma.attr("data-id"),
                    plataforma: txtCEPlataforma.val(),
                    esActualizar: btnCEPlataforma.attr("data-id") > 0 ? true : false
                }
                return obj;
            }
        }

        function fncEliminarPlataforma(idPlataforma) {
            let obj = new Object();
            obj = {
                idPlataforma: idPlataforma
            }
            axios.post("EliminarPlataforma", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Éxito al eliminar el registro.");
                    fncGetPlataformas();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncLimpiarMdlCEPlataformas() {
            txtCEPlataforma.val("");
            btnCEPlataforma.attr("data-id", 0);
        }

        function fncBorderDefault() {
            //#region DATOS EMPLEADO
            txtCEPlataforma.css('border', '1px solid #CCC');
            //#endregion
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Plataformas = new Plataformas();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();