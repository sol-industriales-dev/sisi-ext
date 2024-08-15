(() => {
    $.namespace('SO.Medicos');

    //#region Constants

    const tblS_SaludOcupacionalMedicos = $("#tblS_SaludOcupacionalMedicos");
    const btnCEMedicosModal = $("#btnCEMedicosModal");
    const btnCEMedicos = $("#btnCEMedicos");
    const lblTitleCEMedico = $("#lblTitleCEMedico");
    const titleBtnCEMedico = $('#titleBtnCEMedico');
    const mdlCEMedicos = $('#mdlCEMedicos');
    const txtNombre = $('#txtNombre');
    const txtCedulaProfesional = $('#txtCedulaProfesional');
    const txtPuesto = $('#txtPuesto');
    const txtEmpresa = $('#txtEmpresa');
    const cboUsuarioSIGOPLAN = $('#cboUsuarioSIGOPLAN');

    let dtMedicos;

    //#endregion

    Medicos = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {

            //#region INIT DT

            initTblMedicos();

            //#endregion

            fncGetMedicos();

            //#region EVENTOS

            cboUsuarioSIGOPLAN.fillCombo("FillCboUsuariosMedicos", {}, false);
            cboUsuarioSIGOPLAN.select2({ width: "100%" });

            btnCEMedicosModal.on("click", function (e) {
                lblTitleCEMedico.html("NUEVO REGISTRO");
                titleBtnCEMedico.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                btnCEMedicos.attr("data-id", 0);
                fncLimpiarModalCEMedicos();
                fncBorderDefault();
                mdlCEMedicos.modal("show");
            });

            btnCEMedicos.on("click", function (e) {
                fncCrearEditarMedico();
            });

            //#endregion
        }
    }

    //#region CRUD MEDICOS

    function initTblMedicos() {
        dtMedicos = tblS_SaludOcupacionalMedicos.DataTable({
            language: dtDicEsp,
            destroy: false,
            paging: false,
            ordering: false,
            searching: false,
            bFilter: false,
            info: false,
            columns: [
                //render: function (data, type, row) { }
                { data: 'nombre', title: 'Nombre'},
                { data: 'puesto', title: 'Puesto'},
                { data: 'cedulaProfesional', title: 'No. de Cedula Profesional'},
                { data: 'empresa', title: 'Empresa' },
                {
                    render: function (data, type, row, meta) {
                        let btnEditar = `<button class="btn btn-xs btn-warning editarMedico" 
                                            title="Editar Medico."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                        let btnEliminar = `<button class="btn btn-xs btn-danger eliminarMedico" title="Eliminar Medico"><i class="fas fa-trash"></i></button>`;
                        return btnEditar + btnEliminar;
                    },
                },
                { data: 'id', title: 'id', visible: false},
            ],
            columnDefs: [
                { className: 'dt-center','targets': '_all'}
            ],
            initComplete: function (settings, json) {
                tblS_SaludOcupacionalMedicos.on("click",".editarMedico",function () {
                    let rowData = dtMedicos.row($(this).closest("tr")).data();
                    fncLimpiarModalCEMedicos();
                    fncBorderDefault();
                    lblTitleCEMedico.html("ACTUALIZAR REGISTRO");
                    txtNombre.val(rowData.nombre);
                    txtCedulaProfesional.val(rowData.cedulaProfesional);
                    txtPuesto.val(rowData.puesto);
                    txtEmpresa.val(rowData.empresa);
                    cboUsuarioSIGOPLAN.val(rowData.idUsuarioSIGOPLAN);
                    cboUsuarioSIGOPLAN.trigger("change");
                    //txtNumAutorizacion.val(rowData.numAutorizacion);
                    titleBtnCEMedico.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                    btnCEMedicos.attr("data-id", rowData.id);
                    mdlCEMedicos.modal("show");
                });
                tblS_SaludOcupacionalMedicos.on("click",".eliminarMedico",function () {
                    let rowData = dtMedicos.row($(this).closest("tr")).data();
                    Alert2AccionConfirmar(
                        "¡Cuidado!",
                        "¿Desea eliminar el registro seleccionado?",
                        "Confirmar",
                        "Cancelar",
                        () => fncEliminarMedico(rowData.id)
                    );
                });
            },
        });
    }


    function fncGetMedicos() {
        axios.post("GetMedicos").then(response => {
            let { success, items, message } = response.data;
            if (success) {
                
                dtMedicos.clear();
                dtMedicos.rows.add(items);
                dtMedicos.draw();
                
            }
        }).catch(error => Alert2Error(error.message));
    }


    function fncCrearEditarMedico() {
        let obj = fncObjCEMedico();
        if (obj != "") {
            axios.post("CrearEditarMedicos", obj).then((response) => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetMedicos();
                    mdlCEMedicos.modal("hide");
                    Alert2Exito(message);
                } else {
                    Alert2Error(message);
                }
            }).catch((error) => Alert2Error(error.message));
        }
    }

    function fncObjCEMedico() {
        fncBorderDefault();
        let strMensajeError = "";
        if (txtNombre.val() == "") { txtNombre.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
        if (txtCedulaProfesional.val() == "") { txtCedulaProfesional.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
        if (txtPuesto.val() == "") { txtPuesto.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
        if (txtEmpresa.val() == "") { txtEmpresa.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }

        if (strMensajeError != "") {
            Alert2Warning(strMensajeError);
            return "";
        } else {
            let obj = new Object();
            obj = {
                id: btnCEMedicos.attr("data-id"),
                nombre: txtNombre.val(),
                cedulaProfesional: txtCedulaProfesional.val(),
                puesto: txtPuesto.val(),
                empresa: txtEmpresa.val(),
                idUsuarioSIGOPLAN: cboUsuarioSIGOPLAN.val()
            };
            return obj;
        }
    }

    function fncEliminarMedico(idMedico) {
        if (idMedico > 0) {
            let obj = new Object();
            obj = {
                _idMedico: idMedico,
            };
            axios.post("EliminarMedico", obj).then((response) => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetMedicos();
                    Alert2Exito(message);
                } else {
                    Alert2Error(message);
                }
            }).catch((error) => Alert2Error(error.message));
        } else {
            Alert2Error("Ocurrió un error al eliminar al Medico.");
        }
    }

    //#endregion

    //#region FUNCIONES GENERALES
    function fncLimpiarModalCEMedicos() {
        $('input[type="text"]').val("");

        //#region MEDICOS
        txtEmpresa[0].selectedIndex = 0;
        txtEmpresa.trigger("change");

        cboUsuarioSIGOPLAN[0].selectedIndex = 0;
        cboUsuarioSIGOPLAN.trigger("change");
        //#endregion
    }

    function fncBorderDefault() {
        //#region MEDICOS
        txtNombre.css("border", "1px solid #CCC");
        txtCedulaProfesional.css("border", "1px solid #CCC");
        txtPuesto.css("border", "1px solid #CCC");
        txtEmpresa.css("border", "1px solid #CCC");
        //#endregion
    }
    //#endregion

    $(document).ready(() => {
        SO.Medicos = new Medicos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();