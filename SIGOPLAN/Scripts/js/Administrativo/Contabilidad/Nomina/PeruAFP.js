(() => {
    $.namespace('Nomina.PeruAFP');

    PeruAFP = function () {

        //#region CONST FILTROS
        const btnFiltro_Buscar = $('#btnFiltro_Buscar');
        const btnFiltro_CE = $('#btnFiltro_CE');
        //#endregion

        //#region CONST CATALOGO AFP
        let dtAFP;
        const tblAFP = $('#tblAFP');
        const mdlCrearEditarRegistroAFP = $("#mdlCrearEditarRegistroAFP");
        const cboCE_Anio = $("#cboCE_Anio");
        const cboCE_NumMes = $("#cboCE_NumMes");
        const cboCE_AFP = $('#cboCE_AFP');
        const txtCE_ComisionSobreFlujo = $("#txtCE_ComisionSobreFlujo");
        const txtCE_ComisionAnualSobreSaldo = $("#txtCE_ComisionAnualSobreSaldo");
        const txtCE_PrimaDeSeguros = $("#txtCE_PrimaDeSeguros");
        const txtCE_AporteObligatorioFondoPensiones = $("#txtCE_AporteObligatorioFondoPensiones");
        const txtCE_RenumeracionMaximaAsegurable = $("#txtCE_RenumeracionMaximaAsegurable");
        const btnCE_RegistroAFP = $("#btnCE_RegistroAFP");
        //#endregion

        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblAFP();
            $(".select2").select2({ width: "100%" });
            cboCE_AFP.fillCombo('FillCboCatAFP', null, false, null);
            fncGetRegistrosPeruAFP();
            //#endregion

            //#region FILTROS
            btnFiltro_Buscar.click(function () {
                fncGetRegistrosPeruAFP();
            });

            btnFiltro_CE.click(function () {
                btnCE_RegistroAFP.data().id = 0;
                btnCE_RegistroAFP.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                fncLimpiarMdlCrearEditar();
                fncSetAnioMes();
                mdlCrearEditarRegistroAFP.modal("show");
            })
            //#endregion

            //#region CATALOGO AFP
            btnCE_RegistroAFP.click(function () {
                fncCrearEditarRegistroPeruAFP();
            })
            //#endregion
        }

        //#region FUNCIONES CATALOGO AFP
        function initTblAFP() {
            dtAFP = tblAFP.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'anio', title: 'Año' },
                    { data: 'mes', title: 'Mes' },
                    { data: 'afp', title: 'AFP' },
                    { data: 'comisionSobreFlujo', title: 'Comisión sobre flujo<br>(% Renumeración Bruta Mensual)' },
                    { data: 'comisionAnualSobreSaldo', title: 'Comisión anual sobre saldo' },
                    { data: 'primaDeSeguros', title: 'Prima de seguros (%)<br>(% Renumeración Bruta Mensual)' },
                    { data: 'aporteObligatorioFondoPensiones', title: 'Aporte obligatorio al fondo de pensiones<br>(% Renumeración Bruta Mensual)' },
                    { data: 'remuneracionMaximaAsegurable', title: 'Renumeración máxima asegurable' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return `${btnEditar} ${btnEliminar}`
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblAFP.on('click', '.editarRegistro', function () {
                        let rowData = dtAFP.row($(this).closest('tr')).data();
                        fncLimpiarMdlCrearEditar();
                        fncGetRegistroActualizarPeruAFP(rowData.id);
                    });

                    tblAFP.on('click', '.eliminarRegistro', function () {
                        let rowData = dtAFP.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarRegistroPeruAFP(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: '5%', targets: [8] }
                ],
            });
        }

        function fncGetRegistrosPeruAFP() {
            axios.post('GetRegistrosPeruAFP').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtAFP.clear();
                    dtAFP.rows.add(items);
                    dtAFP.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarRegistroPeruAFP() {
            let objParamsDTO = fncGetOBJCrearEditarRegistroPeruAFP();
            if (objParamsDTO != "") {
                axios.post('CrearEditarRegistroPeruAFP', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetRegistrosPeruAFP();
                        Alert2Exito(message);
                        mdlCrearEditarRegistroAFP.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetOBJCrearEditarRegistroPeruAFP() {
            if (cboCE_Anio.val() <= 0) { fncValidacionCtrl("cboCE_Anio", true, "Es necesario indicar el año."); return ""; } else { fncDefaultCtrls("cboCE_Anio", true); }
            if (cboCE_NumMes.val() <= 0) { fncValidacionCtrl("cboCE_NumMes", true, "Es necesario indicar el mes."); return ""; } else { fncDefaultCtrls("cboCE_NumMes", true); }
            if (cboCE_AFP.val() <= 0) { fncValidacionCtrl("cboCE_AFP", true, "Es necesario indicar el mes."); return ""; } else { fncDefaultCtrls("cboCE_AFP", true); }
            if (txtCE_ComisionSobreFlujo.val() == "") { fncValidacionCtrl("txtCE_ComisionSobreFlujo", false, "Es necesario indicar 'Comisión sobre flujo'."); return ""; } else { fncDefaultCtrls("txtCE_ComisionSobreFlujo", false); }
            if (txtCE_ComisionAnualSobreSaldo.val() == "") { fncValidacionCtrl("txtCE_ComisionAnualSobreSaldo", false, "Es necesario indicar 'Comisión anual sobre saldo'."); return ""; } else { fncDefaultCtrls("txtCE_ComisionAnualSobreSaldo", false); }
            if (txtCE_PrimaDeSeguros.val() == "") { fncValidacionCtrl("txtCE_PrimaDeSeguros", false, "Es necesario indicar 'Prima de seguros'."); return ""; } else { fncDefaultCtrls("txtCE_PrimaDeSeguros", false); }
            if (txtCE_AporteObligatorioFondoPensiones.val() == "") { fncValidacionCtrl("txtCE_AporteObligatorioFondoPensiones", false, "Es necesario indicar 'Aporte obligatorio al fondo de pensiones'."); return ""; } else { fncDefaultCtrls("txtCE_AporteObligatorioFondoPensiones", false); }
            if (txtCE_RenumeracionMaximaAsegurable.val() == "") { fncValidacionCtrl("txtCE_RenumeracionMaximaAsegurable", false, "Es necesario indicar 'Renumeración máxima asegurable'."); return ""; } else { fncDefaultCtrls("txtCE_RenumeracionMaximaAsegurable", false); }

            let objParamsDTO = {};
            objParamsDTO.id = btnCE_RegistroAFP.data().id;
            objParamsDTO.anio = cboCE_Anio.val();
            objParamsDTO.numMes = cboCE_NumMes.val();
            objParamsDTO.FK_AFP = cboCE_AFP.val();
            objParamsDTO.comisionSobreFlujo = txtCE_ComisionSobreFlujo.val();
            objParamsDTO.comisionAnualSobreSaldo = txtCE_ComisionAnualSobreSaldo.val();
            objParamsDTO.primaDeSeguros = txtCE_PrimaDeSeguros.val();
            objParamsDTO.aporteObligatorioFondoPensiones = txtCE_AporteObligatorioFondoPensiones.val();
            objParamsDTO.remuneracionMaximaAsegurable = txtCE_RenumeracionMaximaAsegurable.val();
            return objParamsDTO;
        }

        function fncEliminarRegistroPeruAFP(id) {
            axios.post('EliminarRegistroPeruAFP', { id: id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito(message);
                    fncGetRegistrosPeruAFP();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetRegistroActualizarPeruAFP(id) {
            axios.post('GetRegistroActualizarPeruAFP', { id: id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    cboCE_Anio.val(items.anio)
                    cboCE_Anio.trigger("change");
                    cboCE_NumMes.val(items.numMes)
                    cboCE_NumMes.trigger("change");
                    cboCE_AFP.val(items.FK_AFP)
                    cboCE_AFP.trigger("change");
                    txtCE_ComisionSobreFlujo.val(items.comisionSobreFlujo)
                    txtCE_ComisionAnualSobreSaldo.val(items.comisionAnualSobreSaldo)
                    txtCE_PrimaDeSeguros.val(items.primaDeSeguros)
                    txtCE_AporteObligatorioFondoPensiones.val(items.aporteObligatorioFondoPensiones)
                    txtCE_RenumeracionMaximaAsegurable.val(items.remuneracionMaximaAsegurable)
                    btnCE_RegistroAFP.data().id = id;
                    btnCE_RegistroAFP.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
                    mdlCrearEditarRegistroAFP.modal("show");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncLimpiarMdlCrearEditar() {
            cboCE_Anio[0].selectedIndex = 0;
            cboCE_Anio.trigger("change");
            cboCE_NumMes[0].selectedIndex = 0;
            cboCE_NumMes.trigger("change");
            cboCE_AFP[0].selectedIndex = 0;
            cboCE_AFP.trigger("change");
            txtCE_ComisionSobreFlujo.val("");
            txtCE_ComisionAnualSobreSaldo.val("");
            txtCE_PrimaDeSeguros.val("");
            txtCE_AporteObligatorioFondoPensiones.val("");
            txtCE_RenumeracionMaximaAsegurable.val("");
        }

        function fncSetAnioMes() {
            axios.post('SetAnioMes').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    cboCE_Anio.val(response.data.anio);
                    cboCE_Anio.trigger("change");
                    cboCE_NumMes.val(response.data.mes);
                    cboCE_NumMes.trigger("change");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        Nomina.PeruAFP = new PeruAFP();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();