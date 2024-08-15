(() => {
    $.namespace('CH.RelPuestoFases');

    //#region CONST
    const titulo = $('#titulo');
    const selectAgrupacion = $('#selectAgrupacion');
    const btnFiltroBuscar = $("#btnFiltroBuscar");
    const btnFiltroNuevoConcepto = $("#btnFiltroNuevoConcepto");
    const tblAF_CtrlPptalOfCe_Conceptos = $('#tblAF_CtrlPptalOfCe_Conceptos');
    const cboFiltroAnio = $('#cboFiltroAnio');
    const cboFiltroCC = $('#cboFiltroCC');
    let dtConceptos;
    //#endregion

    //#region CONST CREAR/EDITAR CONCEPTO
    const mdlCEConcepto = $("#mdlCEConcepto");
    const lblTitleCEConcepto = $("#lblTitleCEConcepto");
    const txtConcepto = $("#txtConcepto");
    const selectAgrupacionModal = $('#selectAgrupacionModal');
    const inputInsumo = $('#inputInsumo');
    const inputInsumoDescripcion = $('#inputInsumoDescripcion');
    const inputCTA = $('#inputCTA');
    const inputSCTA = $('#inputSCTA');
    const inputSSCTA = $('#inputSSCTA');
    const inputCuentaDescripcion = $('#inputCuentaDescripcion');
    const btnCEConcepto = $("#btnCEConcepto");
    const lblTitleBtnCEConcepto = $("#lblTitleBtnCEConcepto");
    const cboAnio = $('#cboAnio');
    const cboCC = $('#cboCC');
    const cboConcepto = $('#cboConcepto');
    //#endregion

    RelPuestoFases = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region DATATABLE INIT
            initTblConceptos();
            //#endregion

            //#region EVENTOS CONCEPTOS
            $(".select2").select2();

            btnFiltroNuevoConcepto.on("click", function () {
                let agrupacion = +selectAgrupacion.val();
                let anio = +cboFiltroAnio.val();
                let idCC = +cboFiltroCC.val();

                if (anio == 0) {
                    Alert2Warning('Es necesario seleccionar un año.');
                    return;
                }

                if (idCC == 0) {
                    Alert2Warning('Es necesario seleccionar un CC.');
                    return;
                }

                if (agrupacion == 0) {
                    Alert2Warning("Es necesario seleccionar una agrupación.");
                    return;
                }

                fncLimpiarMdlCEConceptos();
                fncTitleMdlCEConceptos(true);
                mdlCEConcepto.modal("show");
                cboAnio.val(cboFiltroAnio.val());
                cboAnio.trigger("change");
                cboCC.val(cboFiltroCC.val());
                cboCC.trigger("change");
            });

            btnCEConcepto.on("click", function () {
                fncCEConcepto();
            });

            btnFiltroBuscar.on("click", function () {
                fncGetConceptos();
            });
            //#endregion

            selectAgrupacion.on('change', function () {
                let agrupacion = +selectAgrupacion.val();

                if (agrupacion > 0) {
                    let nombreAgrupacion = selectAgrupacion.find('option:selected').text();

                    titulo.text(`CONCEPTOS - ${nombreAgrupacion}`);
                } else {
                    titulo.text('CONCEPTOS');
                }

                selectAgrupacionModal.val(agrupacion);
            });

            $('#inputInsumo, #inputInsumoDescripcion, #inputCTA, #inputSCTA, #inputSSCTA, #inputCuentaDescripcion').on('focus', function () {
                $(this).select();
            });

            inputInsumo.change(getInsumoDescripcion);

            $('#inputCTA, #inputSCTA, #inputSSCTA').on('change', getCuentaDescripcion);

            inputInsumoDescripcion.getAutocompleteValid(setInsumoAuto, validarInsumoAuto, null, '/Administrativo/CtrlPptalOficinasCentrales/GetInsumosAutocomplete');
            inputCuentaDescripcion.getAutocompleteValid(setCuentaAuto, validarCuentaAuto, null, '/Administrativo/CtrlPptalOficinasCentrales/GetCuentasAutocomplete');

            cboFiltroAnio.fillCombo("FillAnios", {}, false);
            cboFiltroAnio.select2();
            cboFiltroAnio.select2({ width: "100%" });

            cboFiltroCC.select2();
            cboFiltroCC.select2({ width: "100%" });
            cboFiltroAnio.on("change", function () {
                if ($(this).val() > 0) {
                    cboFiltroCC.fillCombo('FillUsuarioRelCC', { anio: $(this).val() }, false);
                }
            });

            cboAnio.fillCombo("FillAnios", {}, false);
            cboAnio.select2();
            cboAnio.select2({ width: "100%" });

            cboCC.fillCombo('FillCCAutorizantes', null, false, null);
            cboCC.select2();
            cboCC.select2({ width: "100%" });

            cboConcepto.fillCombo("FillConceptosRelCtasInsumos", {}, false);
            cboConcepto.select2();
            cboConcepto.select2({ width: "100%" });

            cboConcepto.on("change", function () {
                if ($(this).val() > 0) {
                    fncGetCtaInsumoConcepto();
                }
            });

            cboFiltroAnio.on("change", function(){
                if(cboFiltroAnio.val() > 0 && cboFiltroCC.val() > 0){
                    fncGetAgrupaciones();
                            
                }
            });

            cboFiltroCC.on("change", function(){
                if(cboFiltroAnio.val() > 0 && cboFiltroCC.val() > 0){
                    fncGetAgrupaciones();
                            
                }
            });
        }

        //#region CRUD CONCEPTOS
        function initTblConceptos() {
            dtConceptos = tblAF_CtrlPptalOfCe_Conceptos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'concepto', title: 'CONCEPTO' },
                    { data: 'insumo', title: 'INSUMO' },
                    { data: 'cuentaDescripcion', title: 'CUENTA', visible: false },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class="btn btn-xs btn-warning editarRegistro" title="Editar registro."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-xs btn-danger eliminarRegistro" title="Eliminar registro."><i class="fas fa-trash"></i></button>`;
                            return btnEditar + btnEliminar;
                        },
                    },
                    { data: 'id', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblAF_CtrlPptalOfCe_Conceptos.on('click', '.editarRegistro', function () {
                        let rowData = dtConceptos.row($(this).closest('tr')).data();
                        
                        fncGetDatosActualizarConcepto(rowData.id);
                        cboAnio.val(rowData.anio);
                        cboAnio.trigger("change");
                        cboCC.val(rowData.idCC);
                        cboCC.trigger("change");
                    });
                    tblAF_CtrlPptalOfCe_Conceptos.on('click', '.eliminarRegistro', function () {
                        let rowData = dtConceptos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarConcepto(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetConceptos() {
            let idAgrupacion = +selectAgrupacion.val();

            if (idAgrupacion == 0) {
                Alert2Warning('Debe seleccionar una agrupacion.');
                return;
            }   

            axios.post("GetConceptos", { idAgrupacion }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtConceptos.clear();
                    dtConceptos.rows.add(response.data.lstConceptos);
                    dtConceptos.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEConcepto() {
            let obj = fncObjCEConceptos();
            if (obj != "") {
                axios.post("CrearEditarConcepto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetConceptos();
                        mdlCEConcepto.modal("hide");
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCEConceptos() {
            fncBorderDefault();
            let strMensajeError = "";
            // if (txtConcepto.val() == "") { txtConcepto.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboConcepto.val() == '') { strMensajeError = 'Es necesario seleccionar un concepto.'; }
            if (selectAgrupacionModal.val() == '') { strMensajeError = '<br>Es necesario seleccionar una agrupación.'; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEConcepto.attr("data-id"),
                    idConcepto: cboConcepto.val(),
                    concepto: txtConcepto.val(),
                    idAgrupacion: selectAgrupacionModal.val(),
                    insumo: inputInsumo.val(),
                    insumoDescripcion: inputInsumoDescripcion.val(),
                    cta: inputCTA.val(),
                    scta: inputSCTA.val(),
                    sscta: inputSSCTA.val(),
                    cuentaDescripcion: inputCuentaDescripcion.val()
                };
                return obj;
            }
        }

        function fncEliminarConcepto(idConcepto) {
            if (idConcepto > 0) {
                let obj = new Object();
                obj = {
                    idConcepto: idConcepto
                }
                axios.post("EliminarConcepto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetConceptos();
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el registro seleccionado.");
            }
        }

        function fncGetDatosActualizarConcepto(idConcepto) {
            if (idConcepto > 0) {
                let obj = new Object();
                obj = {
                    idConcepto: idConcepto
                }
                axios.post("GetDatosActualizarConcepto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        let objetoRespuesta = response.data.objConcepto;

                        btnCEConcepto.attr("data-id", idConcepto);
                        txtConcepto.val(objetoRespuesta.concepto);
                        cboConcepto.val(objetoRespuesta.idConcepto);
                        selectAgrupacionModal.val(objetoRespuesta.idAgrupacion);
                        inputInsumo.val(objetoRespuesta.insumo);
                        inputInsumoDescripcion.val(objetoRespuesta.insumoDescripcion);
                        inputCTA.val(objetoRespuesta.cta);
                        inputSCTA.val(objetoRespuesta.scta);
                        inputSSCTA.val(objetoRespuesta.sscta);
                        inputCuentaDescripcion.val(objetoRespuesta.cuentaDescripcion);
                        fncTitleMdlCEConceptos(false);
                        mdlCEConcepto.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener la información.")
            }
        }

        function fncTitleMdlCEConceptos(esCrear) {
            let nombreAgrupacion = selectAgrupacion.find('option:selected').text();

            if (esCrear) {
                lblTitleCEConcepto.html(`Nuevo concepto - ${nombreAgrupacion}`);
                lblTitleBtnCEConcepto.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCEConcepto.attr("data-id", 0);
            } else {
                lblTitleCEConcepto.html(`Actualizar concepto - ${nombreAgrupacion}`);
                lblTitleBtnCEConcepto.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
            }

            selectAgrupacionModal.attr('disabled', esCrear);
        }

        function fncLimpiarMdlCEConceptos() {
            $('input').val("");

            cboConcepto[0].selectedIndex = 0;
            cboConcepto.trigger("change");

            // selectAgrupacionModal[0].selectedIndex = 0;
            // selectAgrupacionModal.trigger("change");
        }

        function fncBorderDefault() {
            txtConcepto.css("border", "1px solid #CCC");
        }

        function getInsumoDescripcion() {
            let insumo = inputInsumo.val();

            if (insumo.length == 7) {
                axios.post('GetInsumoDescripcion', { insumo })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            inputInsumoDescripcion.val(response.data.data.insumoDescripcion);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function getCuentaDescripcion() {
            let cta = inputCTA.val();
            let scta = inputSCTA.val();
            let sscta = inputSSCTA.val();

            if (cta != '' && !isNaN(cta) && scta != '' && !isNaN(scta) && sscta != '' && !isNaN(sscta)) {
                axios.post('GetCuentaDescripcion', { cta, scta, sscta })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            inputCuentaDescripcion.val(response.data.data.cuentaDescripcion);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function setInsumoAuto(event, ui) {
            inputInsumo.val(ui.item.id);
        }

        function validarInsumoAuto(event, ui) {
            if (ui.item == null) {
                // inputInsumo.val('');
                // inputInsumoDescripcion.val('');
            }
        }

        function setCuentaAuto(event, ui) {
            let arrayCuentas = ui.item.id.split('-');

            inputCTA.val(arrayCuentas[0]);
            inputSCTA.val(arrayCuentas[1]);
            inputSSCTA.val(arrayCuentas[2]);
        }

        function validarCuentaAuto(event, ui) {
            if (ui.item == null) {
                // inputCTA.val('');
                // inputSCTA.val('');
                // inputSSCTA.val('');
                // inputCuentaDescripcion.val('');
            }
        }

        function fncGetCtaInsumoConcepto() {
            if (cboConcepto.val() > 0) {
                let obj = new Object();
                obj = {
                    idConcepto: cboConcepto.val()
                }
                axios.post("GetCtaInsumoConcepto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        console.log(response.data.objConcepto);
                        inputCTA.val(response.data.objConcepto.cta);
                        inputSCTA.val(response.data.objConcepto.scta);
                        inputSSCTA.val(response.data.objConcepto.sscta);
                        inputInsumo.val(response.data.objConcepto.insumo);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }
        //#endregion
    
        //#region FNC GRALES
        function fncGetAgrupaciones(){
            let obj = {
                anio: cboFiltroAnio.val(),
                idCC: cboFiltroCC.val()
            }
            
            axios.post('GetAgrupaciones', obj)
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        selectAgrupacion.fillCombo({ items: response.data.dataCombo }, null, false, null);
                        selectAgrupacionModal.fillCombo({ items: response.data.dataCombo }, null, false, null);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        //#endregion
    
    }

    $(document).ready(() => {
        CH.RelPuestoFases = new RelPuestoFases();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();