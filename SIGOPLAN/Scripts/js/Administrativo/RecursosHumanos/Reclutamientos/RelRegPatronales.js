(() => {
    $.namespace('CH.RelRegPatronales');

    //#region CONSTS
    const tblRH_EK_Registros_Patronales = $('#tblRH_EK_Registros_Patronales');
    const btnFiltroNewRegPat = $('#btnFiltroNewRegPat');
    let dtRH_EK_Registros_Patronales;
    //#endregion

    //#region MODAL CC CONSTS
    const mdlCC = $('#mdlCC');
    const tblCCs = $('#tblCCs');
    const spanCCRegPatTitle = $('#spanCCRegPatTitle');
    const btnCCNuevo = $('#btnCCNuevo');
    let dtCCs;
    //#endregion

    //#region MODAL CC ADD CONSTS
    const mdlAddCC = $('#mdlAddCC');
    const spanAddCCRegPatTitle = $('#spanAddCCRegPatTitle');
    const cboAddCC = $('#cboAddCC');
    const btnAddCCAñadir = $('#btnAddCCAñadir');
    //#endregion

    //#region CE RegPat
    const mdlCERegPat = $('#mdlCERegPat');
    const txtCERegPatDescripcion = $('#txtCERegPatDescripcion');
    const txtCERegPatNomCorto = $('#txtCERegPatNomCorto');
    const txtCERegPatDireccion = $('#txtCERegPatDireccion');
    const txtCERegPatColonia = $('#txtCERegPatColonia');
    const txtCERegPatLocalidad = $('#txtCERegPatLocalidad');
    const cboCERegPatEstado = $('#cboCERegPatEstado');
    const cboCERegPatCuidad = $('#cboCERegPatCuidad');
    const txtCERegPatCodigo = $('#txtCERegPatCodigo');
    const txtCERegPatGiro = $('#txtCERegPatGiro');
    const btnCERegPatAñadir = $('#btnCERegPatAñadir');
    const txtCERegPatNumero = $('#txtCERegPatNumero');
    const txtCERegPatRFCEmpresa = $('#txtCERegPatRFCEmpresa');
    const txtCERegPatClave = $('#txtCERegPatClave');
    const txtCERegPatRepresentante = $('#txtCERegPatRepresentante');
    const txtCERegPatRFCRepresentate = $('#txtCERegPatRFCRepresentate');
    const divArchivoActa = $('#divArchivoActa');
    const botonArchivoActa = $('#botonArchivoActa');
    const labelArchivoActa = $('#labelArchivoActa');
    const inputArchivoActa = $('#inputArchivoActa');
    //#endregion

    RelRegPatronales = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            initTblRH_EK_Registros_Patronales();
            initTblCCs();

            fncGetRelRegPatronales();

            cboAddCC.fillComboBox('FillCboCCRegistrosPatronales', null, null);
            cboCERegPatCuidad.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: 0, _claveEstado: 0 }, false);
            cboCERegPatEstado.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: 1 }, false);
            // cboCEDatosEmpleadoLugarNac.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: 0, _claveEstado: 0 }, false);

            cboCERegPatEstado.on("change", function () {
                if ($(this).val() != "") {
                    cboCERegPatCuidad.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: 1, _claveEstado: $(this).val() }, false);
                }
            });

            btnCCNuevo.on("click", function () {
                mdlAddCC.modal("show");
                spanAddCCRegPatTitle.text(btnCCNuevo.data("descregpat") + " - " + btnCCNuevo.data("regpat"));
            });

            btnAddCCAñadir.on("click", function () {
                fncAddCCRegPatronal(btnCCNuevo.data("clave_reg_pat"), cboAddCC.val());
            });

            btnFiltroNewRegPat.on("click", function () {
                btnCERegPatAñadir.text("Crear");
                mdlCERegPat.modal("show");
                btnCERegPatAñadir.data("clave", 0);

                inputArchivoActa.val("");
                inputArchivoActa.trigger("change");

                fncDefaultBorder();
                fncDefaultValue();
                fncGetUltimoIdRegPat();
            });

            btnCERegPatAñadir.on("click", function () {
                fncCERegPat($(this).data("clave"));

            });

            inputArchivoActa.on('change', function (event, noLimpiarActualizar) {
                let iconoBoton = botonArchivoActa.find('i');

                if (inputArchivoActa[0].files.length > 0) {
                    let textoLabel = inputArchivoActa[0].files[0].name;

                    labelArchivoActa.text(textoLabel);
                    botonArchivoActa.addClass('btn-success');
                    botonArchivoActa.removeClass('btn-default');
                    iconoBoton.addClass('fa-check');
                    iconoBoton.removeClass('fa-upload');
                } else {
                    if (!noLimpiarActualizar) {
                        labelArchivoActa.text('');
                        botonArchivoActa.addClass('btn-default');
                        botonArchivoActa.removeClass('btn-success');
                        iconoBoton.addClass('fa-upload');
                        iconoBoton.removeClass('fa-check');
                    } else {
                        botonArchivoActa.addClass('btn-success');
                        botonArchivoActa.removeClass('btn-default');
                        iconoBoton.addClass('fa-check');
                        iconoBoton.removeClass('fa-upload');
                    }
                }
            });

            // const txtCERegPatDescripcion = $('#txtCERegPatDescripcion');
            // const txtCERegPatNomCorto = $('#txtCERegPatNomCorto');
            // const txtCERegPatDireccion = $('#txtCERegPatDireccion');
            // const txtCERegPatColonia = $('#txtCERegPatColonia');
            // const txtCERegPatLocalidad = $('#txtCERegPatLocalidad');
            // const cboCERegPatEstado = $('#cboCERegPatEstado');
            // const cboCERegPatCuidad = $('#cboCERegPatCuidad');
            // const txtCERegPatCodigo = $('#txtCERegPatCodigo');
            // const txtCERegPatGiro = $('#txtCERegPatGiro');
            // const txtCERegPatRFCEmpresa = $('#txtCERegPatRFCEmpresa');
            // const txtCERegPatClave = $('#txtCERegPatClave');
            // const txtCERegPatRepresentante = $('#txtCERegPatRepresentante');
            // const txtCERegPatRFCRepresentate = $('#txtCERegPatRFCRepresentate');

            txtCERegPatDescripcion.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCERegPatNomCorto.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCERegPatDireccion.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCERegPatColonia.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCERegPatLocalidad.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            cboCERegPatEstado.on("change", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            cboCERegPatCuidad.on("change", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCERegPatCodigo.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCERegPatGiro.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCERegPatRFCEmpresa.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCERegPatClave.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCERegPatRepresentante.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCERegPatRFCRepresentate.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });

        }

        function initTblRH_EK_Registros_Patronales() {
            dtRH_EK_Registros_Patronales = tblRH_EK_Registros_Patronales.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: true,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'clave_reg_pat', title: 'NUM' },
                    { data: 'desc_reg_pat', title: 'RAZON SOCIAL' },
                    { data: 'giro', title: 'GIRO' },
                    { data: 'registro_patronal', title: 'CLAVE' },
                    { data: 'nombre_representante', title: 'REPRESENTANTE' },
                    { data: 'nombre_corto', title: 'NOMBRE CORTO' },
                    {
                        data: 'rutaArchivo', title: 'ARCHIVO',
                        render: function (data, type, row) {
                            if (row.rutaArchivo == "" || row.rutaArchivo == null) {
                                return `<button title='Archivo pendiente de adjuntar.' class="btn btn-warning btn-sm"><i class="fa fa-clock"></i></button>`;

                            } else {
                                return `
                                    <button title='Descargar archivo adjunto.' class="btn btn-success descargarArchivo btn-sm"><i class="fa fa-check"></i></button>
                                `;
                            }
                        }
                    },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-sm btn-primary verCC">CCs</button>
                            <button title='Actualizar Registro Patronal.' class="btn btn-warning actualizarRegPat btn-sm"><i class="far fa-edit"></i></button>
                            `;

                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblRH_EK_Registros_Patronales.on('click', '.actualizarRegPat', function () {
                        let rowData = dtRH_EK_Registros_Patronales.row($(this).closest('tr')).data();
                        btnCERegPatAñadir.data("clave", rowData.clave_reg_pat);
                        btnCERegPatAñadir.text("Actualizar");

                        if (rowData.rutaArchivo == "" || rowData.rutaArchivo == null) {
                            inputArchivoActa.val("");
                            inputArchivoActa.trigger("change");

                        } else {
                            labelArchivoActa.text(rowData.nombreArchivo);
                            inputArchivoActa.val("");
                            inputArchivoActa.trigger("change", ["noLimpiarActualizar"]);
                        }

                        txtCERegPatNumero.val(rowData.clave_reg_pat);
                        txtCERegPatDescripcion.val(rowData.desc_reg_pat);
                        txtCERegPatNomCorto.val(rowData.nombre_corto);
                        txtCERegPatDireccion.val(rowData.direccion);
                        txtCERegPatColonia.val(rowData.colonia);
                        txtCERegPatLocalidad.val(rowData.localidad);
                        cboCERegPatEstado.val(rowData.clave_estado);
                        cboCERegPatEstado.trigger("change");
                        cboCERegPatCuidad.val(rowData.clave_cuidad);
                        cboCERegPatCuidad.trigger("change");
                        txtCERegPatCodigo.val(rowData.codigo_postal);
                        txtCERegPatGiro.val(rowData.giro);
                        txtCERegPatRFCEmpresa.val(rowData.rfc_cia);
                        txtCERegPatClave.val(rowData.registro_patronal);
                        txtCERegPatRepresentante.val(rowData.nombre_representante);
                        txtCERegPatRFCRepresentate.val(rowData.rfc_representante);

                        mdlCERegPat.modal("show");
                    });
                    tblRH_EK_Registros_Patronales.on('click', '.deleteRegPat', function () {
                        let rowData = dtRH_EK_Registros_Patronales.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncDeleteRegPat(rowData.id));
                    });
                    tblRH_EK_Registros_Patronales.on('click', '.verCC', function () {
                        let rowData = dtRH_EK_Registros_Patronales.row($(this).closest('tr')).data();

                        btnCCNuevo.data("regpat", rowData.registro_patronal);
                        btnCCNuevo.data("descregpat", rowData.desc_reg_pat);
                        btnCCNuevo.data("clave_reg_pat", rowData.clave_reg_pat);

                        fncGetRelRegPatCC(rowData.clave_reg_pat);
                    });
                    tblRH_EK_Registros_Patronales.on('click', '.descargarArchivo', function () {
                        let rowData = dtRH_EK_Registros_Patronales.row($(this).closest('tr')).data();

                        location.href = `DescargarArchivoRegPat?id=${rowData.clave_reg_pat}`;
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: '10%', targets: 7 }
                ],
            });
        }

        function initTblCCs() {
            dtCCs = tblCCs.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'cc', title: 'cc' },
                    { data: 'descripcion', title: 'descripcion' },
                    {
                        render: function (data, type, row) {
                            return `<button title="Eliminar CC" class="btn btn-xs btn-danger eliminarCC"><i class="fa fa-times"></i></i></button>`;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblCCs.on('click', '.classBtn', function () {
                        let rowData = dtCCs.row($(this).closest('tr')).data();
                    });
                    tblCCs.on('click', '.eliminarCC', function () {
                        let rowData = dtCCs.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el CC seleccionado?', 'Confirmar', 'Cancelar', () => fncDeleteCCRegPatronal(btnCCNuevo.data("clave_reg_pat"), rowData.cc));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetRelRegPatronales() {
            let obj = {};
            axios.post("GetRelRegPatronales", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtRH_EK_Registros_Patronales.clear();
                    dtRH_EK_Registros_Patronales.rows.add(items);
                    dtRH_EK_Registros_Patronales.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetRelRegPatCC(clave_reg_pat) {
            axios.post("GetRelRegPatCC", { clave_reg_pat }).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    cboAddCC.fillComboBox('FillCboCCRegistrosPatronales', { clave_reg_pat }, null);

                    mdlCC.modal("show");
                    spanCCRegPatTitle.text(btnCCNuevo.data("descregpat") + " - " + btnCCNuevo.data("regpat"));
                    dtCCs.clear();
                    dtCCs.rows.add(items);
                    dtCCs.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAddCCRegPatronal(clave_reg_pat, cc) {
            axios.post("AddCCRegPatronal", { clave_reg_pat, cc }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se el CC correctamente");
                    mdlAddCC.modal("hide");
                    fncGetRelRegPatCC(clave_reg_pat);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncDeleteCCRegPatronal(clave_reg_pat, cc) {
            axios.post("DeleteCCRegPatronal", { clave_reg_pat, cc }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se elimino el CC correctamente");
                    fncGetRelRegPatCC(clave_reg_pat);
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#region CRUD REG PAT
        function fncCERegPat(claveRegPat) {
            obj = fncGetObj(claveRegPat);
            if (obj != "") {
                axios.post("CrearEditarRegistroPatronal", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        if (claveRegPat != 0) {
                            Alert2Exito("Registro patronal actualizado");

                        } else {
                            Alert2Exito("Registro patronal creado");
                        }

                        mdlCERegPat.modal("hide");
                        fncGetRelRegPatronales();
                    } else {
                        Alert2Warning(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetObj(claveRegPat) {
            //if (txtCEDatosEmpleadoNombre.val() == "") { txtCEDatosEmpleadoNombre.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            //if (cboCEDatosEmpleadoPaisNac.val() == "" || cboCEDatosEmpleadoPaisNac.val() == null) { $("#select2-cboCEDatosEmpleadoPaisNac-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            let strMensajeError = "";

            if (txtCERegPatDescripcion.val() == "") { txtCERegPatDescripcion.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCERegPatNomCorto.val() == "") { txtCERegPatNomCorto.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCERegPatDireccion.val() == "") { txtCERegPatDireccion.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCERegPatColonia.val() == "") { txtCERegPatColonia.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCERegPatCodigo.val() == "") { txtCERegPatCodigo.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCERegPatLocalidad.val() == "") { txtCERegPatLocalidad.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboCERegPatEstado.val() == "" || cboCERegPatEstado.val() == null) { $("#cboCERegPatEstado").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCERegPatCuidad.val() == "" || cboCERegPatCuidad.val() == null) { $("#cboCERegPatCuidad").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCERegPatGiro.val() == "") { txtCERegPatGiro.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCERegPatRFCEmpresa.val() == "") { txtCERegPatRFCEmpresa.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCERegPatClave.val() == "") { txtCERegPatClave.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCERegPatRepresentante.val() == "") { txtCERegPatRepresentante.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCERegPatRFCRepresentate.val() == "") { txtCERegPatRFCRepresentate.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                fncDefaultBorder();
                let obj = {
                    clave_reg_pat: claveRegPat,
                    desc_reg_pat: txtCERegPatDescripcion.val(),
                    nombre_corto: txtCERegPatNomCorto.val(),
                    direccion: txtCERegPatDireccion.val(),
                    colonia: txtCERegPatColonia.val(),
                    localidad: txtCERegPatLocalidad.val(),
                    clave_cuidad: cboCERegPatCuidad.val(),
                    clave_estado: cboCERegPatEstado.val(),
                    codigo_postal: txtCERegPatCodigo.val(),
                    giro: txtCERegPatGiro.val(),
                    rfc_cia: txtCERegPatRFCEmpresa.val(),
                    registro_patronal: txtCERegPatClave.val(),
                    nombre_representante: txtCERegPatRepresentante.val(),
                    rfc_representante: txtCERegPatRFCRepresentate.val(),
                }

                let formData = new FormData();
                formData.set('archivoAdjunto', inputArchivoActa[0].files[0]);
                formData.set('objCERegPat', JSON.stringify(obj));
                return formData;
            }
        }

        function fncDefaultBorder() {
            // $("#select2-cboCandidatosAprobados-container").css('border', '1px solid #CCC');
            // txtCEDatosEmpleadoNombre.css('border', '1px solid #CCC');

            txtCERegPatDescripcion.css('border', '1px solid #CCC');
            txtCERegPatNomCorto.css('border', '1px solid #CCC');
            txtCERegPatDireccion.css('border', '1px solid #CCC');
            txtCERegPatColonia.css('border', '1px solid #CCC');
            txtCERegPatCodigo.css('border', '1px solid #CCC');
            txtCERegPatLocalidad.css('border', '1px solid #CCC');
            txtCERegPatGiro.css('border', '1px solid #CCC');
            txtCERegPatRFCEmpresa.css('border', '1px solid #CCC');
            txtCERegPatClave.css('border', '1px solid #CCC');
            txtCERegPatRepresentante.css('border', '1px solid #CCC');
            txtCERegPatRFCRepresentate.css('border', '1px solid #CCC');
            $("#cboCERegPatEstado").css('border', '1px solid #CCC');
            $("#cboCERegPatCuidad").css('border', '1px solid #CCC');
        }

        function fncDefaultValue() {
            txtCERegPatDescripcion.val("");
            txtCERegPatNomCorto.val("");
            txtCERegPatDireccion.val("");
            txtCERegPatColonia.val("");
            txtCERegPatLocalidad.val("");
            cboCERegPatCuidad.val("");
            cboCERegPatEstado.val("");
            txtCERegPatCodigo.val("");
            txtCERegPatGiro.val("");
            txtCERegPatRFCEmpresa.val("");
            txtCERegPatClave.val("");
            txtCERegPatRepresentante.val("");
            txtCERegPatRFCRepresentate.val("");
        }

        function fncDeleteRegPat(idRegPat) {
            axios.post("DeleteRegistroPatronal", { idRegPat }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Registro eliminado");
                    fncGetRelRegPatronales();
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        function fncGetUltimoIdRegPat() {
            axios.post("GetUltimoIdRegPat").then(response => {
                txtCERegPatNumero.val(response.data);
            }).catch(error => Alert2Error(error.message));
        }
    }

    $(document).ready(() => {
        CH.RelRegPatronales = new RelRegPatronales();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();