(() => {
    $.namespace('Subcontratistas.UsuariosRelSubcontratistas');

    //#region CONST
    const cboFiltroSubcontratistas = $('#cboFiltroSubcontratistas');
    const btnFiltroBuscar = $("#btnFiltroBuscar");
    const btnFiltroNuevo = $("#btnFiltroNuevo");
    const tblCO_ADP_UsuariosFirmantesRelSubcontratistas = $("#tblCO_ADP_UsuariosFirmantesRelSubcontratistas");
    const mdlCEUsuarioRelSubcontratista = $("#mdlCEUsuarioRelSubcontratista");
    const lblTitleCEUsuarioRelSubcontratista = $("#lblTitleCEUsuarioRelSubcontratista");
    const cboCESubcontratista = $('#cboCESubcontratista');
    const txtCENombreCompleto = $("#txtCENombreCompleto");
    const txtCECorreo = $("#txtCECorreo");
    const btnCEUsuarioRelSubcontratista = $("#btnCEUsuarioRelSubcontratista");
    const cboCEContrato = $('#cboCEContrato');
    const lblTitleBtnCEUsuarioRelSubcontratista = $("#lblTitleBtnCEUsuarioRelSubcontratista");
    let dtUsuarioRelSubcontratista;
    //#endregion

    UsuariosRelSubcontratistas = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            initTblUsuariosRelSubcontratistas();
            //#endregion

            //#region EVENTOS
            // fncGetUsuariosRelSubcontratistas();

            cboFiltroSubcontratistas.fillCombo("FillCboSubcontratistas", {}, false);
            cboCESubcontratista.fillCombo("FillCboSubcontratistas", {}, false);
            $(".select2").select2();

            btnFiltroBuscar.on("click", function () {
                if (cboFiltroSubcontratistas.val() > 0) {
                    fncGetUsuariosRelSubcontratistas();
                } else {
                    Alert2Warning("Es necesario seleccionar un subcontratista.");
                }
            });

            btnFiltroNuevo.on("click", function () {
                fncLimpiarMdlCEUsuarioRelSubcontratista();
                lblTitleCEUsuarioRelSubcontratista.html("NUEVO REGISTRO");
                btnCEUsuarioRelSubcontratista.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                $("#divContratos").css("display", "none");
                mdlCEUsuarioRelSubcontratista.modal("show");
            });

            btnCEUsuarioRelSubcontratista.on("click", function () {
                fncCEUsuarioRelSubcontratista();
            });

            cboCESubcontratista.on("change", function () {
                if ($(this).val() > 0) {
                    $("#divContratos").css("display", "inline");
                    cboCEContrato.select2({ closeOnSelect: false });
                    cboCEContrato.fillCombo("FillCboContratosRelSubcontratistas", { idSubcontratista: $(this).val() }, false);
                    cboCEContrato.find("option").get(0).remove();
                    $("#spanCboCEContrato").trigger("click");
                    $("#spanCboCEContrato").trigger("click");
                }
            });

            $("#spanCboCEContrato").click(function (e) {
                cboCEContrato.next(".select2-container").css("display", "block");
                cboCEContrato.siblings("span").find(".select2-selection__rendered")[0].click();
            });

            cboCEContrato.on('select2:close', function (e) {
                cboCEContrato.next(".select2-container").css("display", "none");
                var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
                if (seleccionados.length == 0) $("#spanCboCEContrato").text("TODOS");
                else {
                    if (seleccionados.length == 1) $("#spanCboCEContrato").text($(seleccionados[0]).text().slice(1));
                    else $("#spanCboCEContrato").text(seleccionados.length.toString() + " Seleccionados");
                }
            });

            cboCEContrato.on("select2:unselect", function (evt) {
                if (!evt.params.originalEvent) { return; }
                evt.params.originalEvent.stopPropagation();
            });
            //#endregion
        }

        //#region CRUD USUARIOS REL SUBCONTRATISTAS
        function initTblUsuariosRelSubcontratistas() {
            dtUsuariosRelSubcontratistas = tblCO_ADP_UsuariosFirmantesRelSubcontratistas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'subcontratista', title: 'SUBCONTRATISTA' },
                    {
                        data: 'idContrato', title: 'CONTRATO',
                        render: function (data, type, row) {
                            let contratos = data;
                            let arrContratos = new Array();
                            arrContratos = contratos.split(",");
                            var contratosMap = arrContratos.map(function (num) {
                                return num;
                            });
                            let html = "";
                            contratosMap.forEach(element => {
                                html += `<span class='btn-primary'>&nbsp;&nbsp;<i class='fab fa-creative-commons-nd'></i>${element}</span><br>`;
                            });
                            return html;
                        }
                    },
                    { data: 'nombreFirmante', title: 'FIRMANTE' },
                    { data: 'correo', title: 'CORREO' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>&nbsp;`;
                            let btnHistorial = `<button class='btn btn-xs btn-primary usuarioHistorial' title='Mandar al historial.'><i class="fas fa-book"></i></button>`;
                            return btnEditar + btnEliminar + btnHistorial;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblCO_ADP_UsuariosFirmantesRelSubcontratistas.on('click', '.editarRegistro', function () {
                        let rowData = dtUsuariosRelSubcontratistas.row($(this).closest('tr')).data();
                        fncLimpiarMdlCEUsuarioRelSubcontratista();
                        lblTitleCEUsuarioRelSubcontratista.html("ACTUALIZAR REGISTRO");
                        btnCEUsuarioRelSubcontratista.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                        fncGetDatosActualizarUsuarioRelSubcontratista(rowData.id);
                    });

                    tblCO_ADP_UsuariosFirmantesRelSubcontratistas.on('click', '.eliminarRegistro', function () {
                        let rowData = dtUsuariosRelSubcontratistas.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarUsuarioRelSubcontratista(rowData.id));
                    });

                    tblCO_ADP_UsuariosFirmantesRelSubcontratistas.on('click', '.usuarioHistorial', function () {
                        let rowData = dtUsuariosRelSubcontratistas.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea mandar el usuario al historial?', 'Confirmar', 'Cancelar', () => fncMandarUsuarioComoHistorial(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '15%', targets: [2] },
                    { width: '10%', targets: [5] }
                ],
            });
        }

        function fncGetUsuariosRelSubcontratistas() {
            let obj = new Object();
            obj.idUsuarioSubcontratista = cboFiltroSubcontratistas.val();
            axios.post("GetUsuariosRelSubcontratistas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtUsuariosRelSubcontratistas.clear();
                    dtUsuariosRelSubcontratistas.rows.add(response.data.lstUsuariosRelSubcontratistas);
                    dtUsuariosRelSubcontratistas.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEUsuarioRelSubcontratista() {
            let obj = fncOBJCEUsuarioRelSubcontratista();
            if (obj != "") {
                axios.post("CEUsuarioRelSubcontratista", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetUsuariosRelSubcontratistas();
                        mdlCEUsuarioRelSubcontratista.modal("hide");
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncOBJCEUsuarioRelSubcontratista() {
            fncBorderDefault();
            let strMensajeError = "";
            if (cboCESubcontratista.val() == "") { $("#select2-cboCESubcontratista-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            // if (cboCEContrato.val() == "") { $("#select2-cboCEContrato-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCENombreCompleto.val() == "") { txtCENombreCompleto.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCECorreo.val() == "") { txtCECorreo.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let arrContratos = new Array();

                if (cboCEContrato.val() == "") {
                    $("#cboCEContrato > option").prop("selected", "selected");
                }

                cboCEContrato.val().forEach(element => {
                    if (element > 0) {
                        arrContratos.push(element);
                    }
                });

                let obj = new Object();
                obj = {
                    id: btnCEUsuarioRelSubcontratista.attr("data-id"),
                    lstContratos: cboCEContrato.val(),
                    idUsuarioSubcontratista: cboCESubcontratista.val(),
                    nombreFirmante: txtCENombreCompleto.val(),
                    correo: txtCECorreo.val()
                };
                return obj;
            }
        }

        function fncEliminarUsuarioRelSubcontratista(idUsuarioRelSubcontratista) {
            if (idUsuarioRelSubcontratista <= 0) {
                Alert2Warning("Ocurrió un error al eliminar el registro seleccionado.");
            } else {
                let obj = new Object();
                obj.id = idUsuarioRelSubcontratista;
                axios.post("EliminarUsuarioRelSubcontratista", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetUsuariosRelSubcontratistas();
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncMandarUsuarioComoHistorial(idUsuarioRelSubcontratista) {
            if (idUsuarioRelSubcontratista <= 0) {
                Alert2Warning("Ocurrió un error al mandar el usuario al historial.");
            } else {
                let obj = new Object();
                obj.id = idUsuarioRelSubcontratista;
                axios.post("MandarUsuarioComoHistorial", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetUsuariosRelSubcontratistas();
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetDatosActualizarUsuarioRelSubcontratista(idUsuarioRelSubcontratista) {
            if (parseFloat(idUsuarioRelSubcontratista) <= 0) {
                Alert2Warning("Ocurrió un error al obtener la información del registro seleccionado.");
            } else {
                let obj = new Object();
                obj.id = parseFloat(idUsuarioRelSubcontratista);
                axios.post("GetDatosActualizarUsuarioRelSubcontratista", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region SE OBTIENE LA INFORMACIÓN DEL USUARIO A ACTUALIZAR
                        cboCESubcontratista.val(response.data.objUsuarioRelSubcontratista.idUsuarioSubcontratista);
                        cboCESubcontratista.trigger("change");

                        let contratos = response.data.objUsuarioRelSubcontratista.idContrato;
                        let arrContratos = new Array();
                        arrContratos = contratos.split(",");
                        var contratosMap = arrContratos.map(function (num) {
                            return num;
                        });
                        cboCEContrato.val(contratosMap);
                        cboCEContrato.trigger("change");
                        $("#spanCboCEContrato").trigger("click");
                        $("#spanCboCEContrato").trigger("click");

                        txtCENombreCompleto.val(response.data.objUsuarioRelSubcontratista.nombreFirmante);
                        txtCECorreo.val(response.data.objUsuarioRelSubcontratista.correo);
                        btnCEUsuarioRelSubcontratista.attr("data-id", parseFloat(response.data.objUsuarioRelSubcontratista.id));
                        mdlCEUsuarioRelSubcontratista.modal("show");
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncBorderDefault() {
            $("#select2-cboCESubcontratista-container").css("border", "1px solid #CCC");
            txtCENombreCompleto.css("border", "1px solid #CCC");
            txtCECorreo.css("border", "1px solid #CCC");
        }

        function fncLimpiarMdlCEUsuarioRelSubcontratista() {
            $("input[type='text']").val("");
            cboCEContrato[0].selectedIndex = 0;
            cboCEContrato.trigger("change");
            cboCESubcontratista[0].selectedIndex = 0;
            cboCESubcontratista.trigger("change");
            btnCEUsuarioRelSubcontratista.attr("data-id", 0);
        }
        //#endregion
    }

    $(document).ready(() => {
        Subcontratistas.UsuariosRelSubcontratistas = new UsuariosRelSubcontratistas();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();