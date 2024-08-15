(() => {
    $.namespace('CH.RelFases');
    const tblRH_REC_FasesUsuarios = $('#tblRH_REC_FasesUsuarios');

    //#region CONSTS
    const cboFiltroFase = $('#cboFiltroFase');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroNuevoUser = $('#btnFiltroNuevoUser');
    let dtRH_REC_FasesUsuarios;
    //#endregion

    //#region 
    const mdlUser = $('#mdlUser');
    const txtUserNombre = $('#txtUserNombre');
    const txtUserNumero = $('#txtUserNumero');
    const cboUserFase = $('#cboUserFase');
    const btnUserAñadir = $('#btnUserAñadir');
    //#endregion

    RelFases = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            initTblRH_REC_FasesUsuarios();

            cboFiltroFase.fillCombo('FillComboRelFases', null, null);
            cboUserFase.fillCombo('FillComboRelFases', null, null);

            btnFiltroBuscar.on("click", function () {
                if (cboFiltroFase.val() != null && cboFiltroFase.val() != "") {
                    fncGetFasesUsuarios(cboFiltroFase.val());
                }
            });

            btnFiltroNuevoUser.on("click", function () {
                fncLimpiarMdlUser();
                mdlUser.modal("show");

                if (cboFiltroFase.val() != null && cboFiltroFase.val() != "") {
                    cboUserFase.val(cboFiltroFase.val());
                }
            });

            btnUserAñadir.on("click", function () {
                fncAddFasesUsuarios(txtUserNombre.data("id"), cboUserFase.val());
            });

            txtUserNombre.getAutocomplete(funGetUser, null, '/Administrativo/FormatoCambio/getCatEmpleadosReclutamientos');
        }

        function funGetUser(event, ui) {
            txtUserNombre.data("id", ui.item.id);
            txtUserNombre.val(ui.item.value);
            //fncGetFechas(txtCEVacacionClaveEmp.val());
            //fncGetResponsable(txtCEVacacionClaveEmp.val());
        }

        function initTblRH_REC_FasesUsuarios() {
            dtRH_REC_FasesUsuarios = tblRH_REC_FasesUsuarios.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'faseDesc', title: 'faseDesc' },
                    { data: 'nombreUsuario', title: 'nombreUsuario' },
                    {
                        render: function (data, type, row) {
                            return `<button title="Eliminar relacion usuario" class="btn btn-xs btn-danger eliminarUsr"><i class="fa fa-times"></i></i></button>`
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_FasesUsuarios.on('click', '.classBtn', function () {
                        let rowData = dtRH_REC_FasesUsuarios.row($(this).closest('tr')).data();
                    });
                    tblRH_REC_FasesUsuarios.on('click', '.eliminarUsr', function () {
                        let rowData = dtRH_REC_FasesUsuarios.row($(this).closest('tr')).data();

                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncDeleteFasesUsuarios(rowData.idUsuario, rowData.idFase));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetFasesUsuarios(idFase) {
            axios.post("GetFasesUsuarios", { idFase }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtRH_REC_FasesUsuarios.clear();
                    dtRH_REC_FasesUsuarios.rows.add(items);
                    dtRH_REC_FasesUsuarios.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAddFasesUsuarios(idUsuario, idFase) {
            axios.post("AddFasesUsuarios", { idUsuario, idFase }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    fncGetFasesUsuarios(cboFiltroFase.val());
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncDeleteFasesUsuarios(idUsuario, idFase) {
            axios.post("DeleteFasesUsuarios", { idUsuario, idFase }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    fncGetFasesUsuarios(cboFiltroFase.val());

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncLimpiarMdlUser() {
            cboUserFase[0].selectedIndex = 0;
            cboUserFase.trigger("change");

            txtUserNombre.val("");
            txtUserNombre.removeData(); // QUITAR EL ID GUARDADO POSTERIORMENTE
        }
    }

    $(document).ready(() => {
        CH.RelFases = new RelFases();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();