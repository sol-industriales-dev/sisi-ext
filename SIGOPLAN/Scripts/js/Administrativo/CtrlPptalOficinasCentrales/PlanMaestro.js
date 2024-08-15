(() => {
    $.namespace('CtrlPptalOfCE.PlanMaestro');

    //#region CONST FILTROS
    const cboFiltroAnio = $("#cboFiltroAnio");
    const cboFiltroCC = $("#cboFiltroCC");
    const btnFiltroBuscar = $("#btnFiltroBuscar");
    const btnFiltroNuevo = $("#btnFiltroNuevo");
    const tblAF_CtrlPptalOfCe_RN_PlanMaestro = $("#tblAF_CtrlPptalOfCe_RN_PlanMaestro");
    let dtPlanMaestro;
    //#endregion

    //#region CONST CREAR/EDITAR PLAN MAESTRO
    const mdlCE_PM = $('#mdlCE_PM');
    const lblTitleCE_PM = $('#lblTitleCE_PM');
    const cboCE_PM_Anio = $('#cboCE_PM_Anio');
    const cboCE_PM_CC = $('#cboCE_PM_CC');
    const txtCE_PM_MisionArea = $('#txtCE_PM_MisionArea');
    const txtCE_PM_ObjetivoEspecificoMedible = $('#txtCE_PM_ObjetivoEspecificoMedible');
    const txtCE_PM_Meta = $('#txtCE_PM_Meta');
    const btnCE_PM = $('#btnCE_PM');
    const lblTitleBtnCE_PM = $('#lblTitleBtnCE_PM');
    const spanNuevaSeccionMedicionIndicador = $('#spanNuevaSeccionMedicionIndicador');
    const btnCE_PM_Mediciones_EliminarIndicadorFijo = $('#btnCE_PM_Mediciones_EliminarIndicadorFijo');
    //#endregion

    //#region CONST CREAR/EDITAR MEDICIONES/INDICADORES
    const btnCE_PM_Mediciones_NuevoIndicador = $('#btnCE_PM_Mediciones_NuevoIndicador');
    const mdlCEMedicionIndicador = $("#mdlCEMedicionIndicador");
    const lblTitleCEMedicionIndicador = $("#lblTitleCEMedicionIndicador");
    const txtCE_Indicador = $("#txtCE_Indicador");
    const txtCE_FuenteDatos = $("#txtCE_FuenteDatos");
    const cboCE_UsuarioResponsable = $("#cboCE_UsuarioResponsable");
    const txtCE_Meta = $('#txtCE_Meta');
    const btnCE_MedicionIndicador = $("#btnCE_MedicionIndicador");
    const lblTitleBtnCE_MedicionIndicador = $("#lblTitleBtnCE_MedicionIndicador");
    //#endregion

    //#region CONST CREAR/EDITAR AGRUPACIONES/CONCEPTOS
    const spanNuevaSeccionAgrupaciones = $('#spanNuevaSeccionAgrupaciones');
    const mdlCE_PM_RN_Agrupacion = $("#mdlCE_PM_RN_Agrupacion");
    const lblTitleCE_PM_RN_Agrupacion = $("#lblTitleCE_PM_RN_Agrupacion");
    const cboCE_PM_RN_Anio = $("#cboCE_PM_RN_Anio");
    const cboCE_PM_RN_CC = $("#cboCE_PM_RN_CC");
    const txtCE_PM_RN_Agrupacion = $("#txtCE_PM_RN_Agrupacion");
    const btnCE_PM_RN_Agrupacion = $("#btnCE_PM_RN_Agrupacion");
    const lblTitleBtnCE_PM_RN_Agrupacion = $("#lblTitleBtnCE_PM_RN_Agrupacion");
    const btnCE_PM_RN_NuevaAgrupacion = $('#btnCE_PM_RN_NuevaAgrupacion');
    const btnCE_PM_RN_NuevoConcepto = $('#btnCE_PM_RN_NuevoConcepto');
    const mdlCE_PM_RN_Concepto = $("#mdlCE_PM_RN_Concepto");
    const lblTitleCE_PM_RN_Concepto = $("#lblTitleCE_PM_RN_Concepto");
    const cboCE_PM_RN_ConceptoAnio = $("#cboCE_PM_RN_ConceptoAnio");
    const cboCE_PM_RN_ConceptoCC = $("#cboCE_PM_RN_ConceptoCC");
    const cboCE_PM_RN_ConceptoAgrupacion = $("#cboCE_PM_RN_ConceptoAgrupacion");
    const txtCE_PM_RN_ConceptoConcepto = $("#txtCE_PM_RN_ConceptoConcepto");
    const txtCE_PM_RN_ConceptoCantidad = $("#txtCE_PM_RN_ConceptoCantidad");
    const btnCE_PM_RN_Concepto = $("#btnCE_PM_RN_Concepto");
    const lblTitleBtnCE_PM_RN_Concepto = $("#lblTitleBtnCE_PM_RN_Concepto");
    //#endregion

    PlanMaestro = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            initTblPlanMaestro();
            //#endregion

            //#region EVENTOS PLAN MAESTRO
            txtCE_PM_RN_ConceptoCantidad.on("click", function () {
                $(this).select();
            });

            btnFiltroNuevo.on("click", function () {
                fncBorderDefaultPlanMaestro();
                fncLimpiarMdlCEPlanMaestro();
                fncTitleMdlCEPlanMaestro(true);
                btnCE_PM_Mediciones_EliminarIndicadorFijo.css("display", "inline");

                //#region VALIDACIÓN: ES NECESARIO SELECCIONAR UN AÑO Y UN CC EN LOS FILTROS.
                $('#select2-cboFiltroAnio-container').css('border', '1px solid #CCC');
                $('#select2-cboFiltroCC-container').css('border', '1px solid #CCC');
                if (cboFiltroAnio.val() <= 0 || cboFiltroCC.val() <= 0) {
                    let strMensajeError = "";
                    if (cboFiltroAnio.val() <= 0) {
                        strMensajeError += "Es necesario seleccionar un año.";
                        $('#select2-cboFiltroAnio-container').css('border', '2px solid red');
                    }

                    if (cboFiltroCC.val() <= 0) {
                        strMensajeError += "<br>Es necesario seleccionar un cc.";
                        $('#select2-cboFiltroCC-container').css('border', '2px solid red');
                    }

                    Alert2Warning(strMensajeError);
                } else {
                    cboCE_PM_Anio.val(cboFiltroAnio.val());
                    cboCE_PM_Anio.trigger("change");
                    cboCE_PM_CC.val(cboFiltroCC.val());
                    cboCE_PM_CC.trigger("change");
                    btnCE_PM.attr("data-id", 0);
                    btnCE_PM_Mediciones_NuevoIndicador.attr("data-id", 0);
                    btnCE_PM_Mediciones_NuevoIndicador.trigger("click");
                    fncGetCantAgrupacionesConceptos();
                    mdlCE_PM.modal("show");
                }
                //#endregion
            });

            btnCE_PM.on('click', function () {
                fncCEPlanMaestro();
            });

            btnFiltroBuscar.on('click', function () {
                //#region VALIDACIÓN: ES NECESARIO SELECCIONAR UN AÑO Y UN CC EN LOS FILTROS.
                $('#select2-cboFiltroAnio-container').css('border', '1px solid #CCC');
                $('#select2-cboFiltroCC-container').css('border', '1px solid #CCC');
                if (cboFiltroAnio.val() <= 0 || cboFiltroCC.val() <= 0) {
                    let strMensajeError = "";
                    if (cboFiltroAnio.val() <= 0) {
                        strMensajeError += "Es necesario seleccionar un año.";
                        $('#select2-cboFiltroAnio-container').css('border', '2px solid red');
                    }

                    if (cboFiltroCC.val() <= 0) {
                        strMensajeError += "<br>Es necesario seleccionar un cc.";
                        $('#select2-cboFiltroCC-container').css('border', '2px solid red');
                    }

                    Alert2Warning(strMensajeError);
                } else {
                    fncGetPlanMaestro();
                }
                //#endregion
            });
            //#endregion

            //#region FILL COMBOS PLAN MAESTRO
            cboFiltroAnio.fillCombo('FillAnios', {}, false);
            cboCE_PM_Anio.fillCombo('FillAnios', {}, false);

            cboFiltroAnio.on("change", function () {
                if ($(this).val() > 0) {
                    cboFiltroCC.fillCombo('FillUsuarioRelCC', { anio: $(this).val() }, false);
                    cboCE_PM_CC.fillCombo('FillUsuarioRelCC', { anio: $(this).val() }, false);
                }
            });

            //#endregion

            //#region EVENTOS MEDICIONES/INDICADORES
            cboCE_UsuarioResponsable.fillCombo('FillUsuarios', {}, false);
            btnCE_PM_Mediciones_NuevoIndicador.on("click", function () {
                if (btnCE_PM.attr("data-id") > 0) {
                    fncLimpiarMdlCEMedicionIndicador();
                    fncTitleMdlCEMedicionIndicador(true);
                    btnCE_MedicionIndicador.attr("data-id", btnCE_PM.attr("data-id"));
                    mdlCEMedicionIndicador.modal("show");
                } else {
                    fncAsignarClasesCrearEditarConceptos();
                    fncCrearSeccionMedicionIndicador(true, false, null);
                }
            });

            btnCE_PM_Mediciones_EliminarIndicadorFijo.on("click", function () {
                fncCrearSeccionMedicionIndicador(false, false, null);
            });

            btnCE_MedicionIndicador.on("click", function () {
                fncCrearEditarMedicionIndicador();
            });
            //#endregion

            //#region EVENTOS AGRUPACIONES
            btnCE_PM_RN_NuevaAgrupacion.on("click", function () {
                fncBorderDefaultAgrupacion();
                fncLimpiarMdlCEAgrupacion();
                fncTitleMdlCEAgrupacion(true);

                cboCE_PM_RN_Anio.val(cboFiltroAnio.val());
                cboCE_PM_RN_Anio.trigger("change");
                cboCE_PM_RN_CC.val(cboFiltroCC.val());
                cboCE_PM_RN_CC.trigger("change");
                mdlCE_PM_RN_Agrupacion.modal("show");
            });

            btnCE_PM_RN_Agrupacion.on('click', function () {
                fncCE_PM_RN_Agrupacion();
            });

            cboCE_PM_RN_Anio.fillCombo('FillAnios', {}, false);
            cboCE_PM_RN_Anio.on("change", function () {
                if ($(this).val() > 0) {
                    cboCE_PM_RN_CC.fillCombo('FillUsuarioRelCC', { anio: $(this).val() }, false);
                }
            });
            //#endregion

            //#region EVENTOS CONCEPTOS
            btnCE_PM_RN_NuevoConcepto.on("click", function () {
                fncBorderDefaultConcepto();
                fncLimpiarMdlCEConcepto();
                fncTitleMdlCEConcepto(true);

                cboCE_PM_RN_ConceptoAnio.fillCombo('FillAnios', {}, false);
                cboCE_PM_RN_ConceptoCC.fillCombo('FillUsuarioRelCC', { anio: cboFiltroAnio.val() }, false);
                cboCE_PM_RN_ConceptoAgrupacion.fillCombo('FillRNAgrupaciones', { anio: cboFiltroAnio.val(), idCC: cboFiltroCC.val() }, false);

                cboCE_PM_RN_ConceptoAnio.val(cboFiltroAnio.val());
                cboCE_PM_RN_ConceptoAnio.trigger("change");
                cboCE_PM_RN_ConceptoCC.val(cboFiltroCC.val());
                cboCE_PM_RN_ConceptoCC.trigger("change");
                mdlCE_PM_RN_Concepto.modal("show");
            });

            btnCE_PM_RN_Concepto.on("click", function () {
                fncCE_PM_RN_Concepto();
            });
            //#endregion

            $('.select2').select2();
            $(".select2").select2({ width: '100%' });
        }

        //#region CRUD PLAN MAESTRO
        function initTblPlanMaestro() {
            dtPlanMaestro = tblAF_CtrlPptalOfCe_RN_PlanMaestro.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'anio', title: 'AÑO' },
                    { data: 'cc', title: 'CC' },
                    { data: 'meta', title: 'META' },
                    {
                        render: function (data, type, row, meta) {
                            let btnReporte, btnEditar, btnEliminar;

                            btnReporte = `<button  class='btn btn-xs btn-primary getReporte' title='Descargar reporte.'><i class="fas fa-file"></i></button>&nbsp;`;
                            console.log(row.autorizado);
                            if (!row.notificado) {
                                btnEditar = `<button class='btn btn-xs btn-warning editarRegistroPM' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                                btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistroPM' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                                btnFiltroNuevo.attr('disabled', false);
                            } else {
                                btnEditar = `<button class='btn btn-xs btn-warning editarRegistroPM' disabled="disabled" title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                                btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistroPM' disabled="disabled" title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                                btnFiltroNuevo.attr('disabled', true);
                            }
                            return btnReporte + btnEditar + btnEliminar;
                        },
                    },
                    { data: 'autorizado', title: 'autorizado', visible: false },
                    { data: 'id', title: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblAF_CtrlPptalOfCe_RN_PlanMaestro.on('click', '.editarRegistroPM', function () {
                        let rowData = dtPlanMaestro.row($(this).closest('tr')).data();
                        fncGetCantAgrupacionesConceptos();
                        fncBorderDefaultPlanMaestro();
                        fncGetDatosActualizarPlanMaestro(rowData.id);
                    });
                    tblAF_CtrlPptalOfCe_RN_PlanMaestro.on('click', '.eliminarRegistroPM', function () {
                        let rowData = dtPlanMaestro.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarPlanMaestro(rowData.id));
                    });
                    tblAF_CtrlPptalOfCe_RN_PlanMaestro.on('click', '.getReporte', function () {
                        let rowData = dtPlanMaestro.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('', '¿Desea descargar el reporte del plan maestro seleccionado?', 'Confirmar', 'Cancelar', () => fncGetReporte(rowData.id, rowData.anio, rowData.idCC));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', targets: '_all' },
                    { className: 'dt-body-center', targets: "_all" },
                    { className: 'dt-head-center', targets: "_all" },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function fncGetPlanMaestro() {
            if (cboFiltroAnio.val() > 0 && cboFiltroCC.val() > 0) {
                let obj = new Object();
                obj = {
                    anio: cboFiltroAnio.val(),
                    idCC: cboFiltroCC.val()
                }
                axios.post('GetPlanMaestro', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtPlanMaestro.clear();
                        dtPlanMaestro.rows.add(response.data.lstPlanMaestro);
                        dtPlanMaestro.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = '';
                cboFiltroAnio.val() <= 0 ? strMensajeError += 'Es necesario indicar un año.' : '';
                cboFiltroCC.val() <= 0 ? strMensajeError += '<br>Es necesario indicar un CC.' : '';
                Alert2Warning(strMensajeError);
            }
        }

        function fncCEPlanMaestro() {
            let obj = fncObjCEPlanMaestro();
            if (obj != '') {
                axios.post('CrearEditarPlanMaestro', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetPlanMaestro();
                        mdlCE_PM.modal('hide');
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCEPlanMaestro() {
            fncBorderDefaultPlanMaestro();
            let strMensajeError = '';
            if (txtCE_PM_MisionArea.val() == '') { txtCE_PM_MisionArea.css('border', '2px solid red'); strMensajeError = 'Es necesario llenar los campos obligatorios.'; }
            if (txtCE_PM_ObjetivoEspecificoMedible.val() == '') { txtCE_PM_ObjetivoEspecificoMedible.css('border', '2px solid red'); strMensajeError = 'Es necesario llenar los campos obligatorios.'; }
            if (txtCE_PM_Meta.val() == '') { txtCE_PM_Meta.css('border', '2px solid red'); strMensajeError = 'Es necesario llenar los campos obligatorios.'; }

            if (strMensajeError != '') {
                Alert2Warning(strMensajeError);
                return '';
            } else {

                //#region MEDICIONES Y/O INDICADORES
                let cantSecciones = btnCE_PM_Mediciones_NuevoIndicador.attr("data-id");
                cantSecciones++;
                let arrMedicionesIndicadores = new Array();
                for (let i = 0; i < cantSecciones; i++) {
                    if ($(`#txtCE_PM_Mediciones_Indicador${i}`).val() != undefined) {
                        let obj2 = new Object();
                        obj2 = {
                            id: $(`#txtCE_PM_Mediciones_Indicador${i}`).attr("data-id") != undefined ? $(`#txtCE_PM_Mediciones_Indicador${i}`).attr("data-id") : 0,
                            indicador: $(`#txtCE_PM_Mediciones_Indicador${i}`).val(),
                            fuenteDatos: $(`#txtCE_PM_Mediciones_FuenteDatos${i}`).val(),
                            idUsuarioResponsable: $(`#cboCE_PM_Mediciones_Responsable${i}`).val(),
                            meta: $(`#txtCE_PM_Mediciones_Meta${i}`).val()
                        }
                        arrMedicionesIndicadores.push(obj2);
                    }
                }
                //#endregion

                let obj = new Object();
                obj = {
                    id: btnCE_PM.attr('data-id'),
                    anio: cboCE_PM_Anio.val(),
                    idCC: cboCE_PM_CC.val(),
                    misionArea: txtCE_PM_MisionArea.val(),
                    objEspecificoMedible: txtCE_PM_ObjetivoEspecificoMedible.val(),
                    meta: txtCE_PM_Meta.val(),
                    lstMedicionesIndicadores: arrMedicionesIndicadores
                };
                return obj;
            }
        }

        function fncEliminarPlanMaestro(idPlanMaestro) {
            if (idPlanMaestro > 0) {
                let obj = new Object();
                obj = {
                    idPlanMaestro: idPlanMaestro
                }
                axios.post('EliminarPlanMaestro', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetPlanMaestro();
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error('Ocurrió un error al eliminar el registro seleccionado.');
            }
        }

        function fncGetDatosActualizarPlanMaestro(idPlanMaestro) {
            if (idPlanMaestro > 0) {
                let obj = new Object();
                obj = {
                    idPlanMaestro: idPlanMaestro
                }
                axios.post('GetDatosActualizarPlanMaestro', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnCE_PM.attr('data-id', idPlanMaestro);
                        cboCE_PM_Anio.val(response.data.objPlanMaestro.anio);
                        cboCE_PM_Anio.trigger('change');
                        cboCE_PM_CC.val(response.data.objPlanMaestro.idCC);
                        cboCE_PM_CC.trigger('change');
                        txtCE_PM_MisionArea.val(response.data.objPlanMaestro.misionArea);
                        txtCE_PM_ObjetivoEspecificoMedible.val(response.data.objPlanMaestro.objEspecificoMedible);
                        txtCE_PM_Meta.val(response.data.objPlanMaestro.meta);
                        btnCE_PM_Mediciones_EliminarIndicadorFijo.css("display", "none");

                        //#region SE CREA SECCIONES DE MEDICIONES/INDICADORES
                        btnCE_PM_Mediciones_NuevoIndicador.attr("data-id", response.data.lstIndicadores.length);
                        fncCrearSeccionMedicionIndicador(true, true, response.data.lstIndicadores);
                        //#endregion

                        fncTitleMdlCEPlanMaestro(false);
                        fncAsignarClasesCrearEditarConceptos();
                        mdlCE_PM.modal('show');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error('Ocurrió un error al obtener la información.')
            }
        }

        function fncTitleMdlCEPlanMaestro(esCrear) {
            if (esCrear) {
                lblTitleCE_PM.html(`NUEVO REGISTRO`);
                lblTitleBtnCE_PM.html(`<i class='fas fa-save'></i>&nbsp;Guardar`);
                btnCE_PM.attr('data-id', 0);
            } else {
                lblTitleCE_PM.html(`ACTUALIZAR REGISTRO`);
                lblTitleBtnCE_PM.html(`<i class='fas fa-save'></i>&nbsp;Actualizar`);
            }
        }

        function fncLimpiarMdlCEPlanMaestro() {
            $('input[type="text"]').val('');

            cboCE_PM_Anio[0].selectedIndex = 0;
            cboCE_PM_Anio.trigger('change');

            cboCE_PM_CC[0].selectedIndex = 0;
            cboCE_PM_CC.trigger('change');

            txtCE_PM_MisionArea.val("");
            txtCE_PM_ObjetivoEspecificoMedible.val("");
        }

        function fncBorderDefaultPlanMaestro() {
            $('#select2-cboCEAnio-container').css('border', '1px solid #CCC');
            $('#select2-cboCECC-container').css('border', '1px solid #CCC');
            txtCE_PM_MisionArea.css('border', '1px solid #CCC');
            txtCE_PM_ObjetivoEspecificoMedible.css('border', '1px solid #CCC');
            txtCE_PM_Meta.css('border', '1px solid #CCC');
        }
        //#endregion

        //#region CRUD MEDICIONES/INDICADORES
        function fncCrearSeccionMedicionIndicador(esNuevo, esActualizar, lstIndicadores) {
            let cantSecciones = btnCE_PM_Mediciones_NuevoIndicador.attr("data-id");
            if (esNuevo && !esActualizar) {
                cantSecciones++;
            }
            else if (!esNuevo && !esActualizar) {
                cantSecciones--;
            }

            if (cantSecciones > 0) {
                btnCE_PM_Mediciones_NuevoIndicador.attr("data-id", cantSecciones);

                let seccionAnterior = "";
                let contador = 0;
                for (let i = 0; i < cantSecciones; i++) {
                    contador++;
                    seccionAnterior += `
                        <!-- MEDICIONES Y/O INDICADORES -->
                        <div class='row' id="mdlMedicionIndicador${contador}">
                            <div class='col-sm-12'>
                                <div class='panel panel-default panel-principal'>
                                    <div class='panel-heading text-center'><h4>MEDICIONES Y/O INDICADORES ${contador}</h4></div>
                                    <div class='panel-body'>
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="pull-right" style="margin-bottom: 0.5%;">
                                                    <button type='button' class='btn btn-danger btnCE_PM_Mediciones_EliminarIndicador' idDiv="${contador}" id='btnCE_PM_Mediciones_EliminarIndicador${contador}'>
                                                        <i class="fas fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='row'>
                                            <div class='col-lg-6'>
                                                <label>Indicador</label>
                                                <input type="text" id="txtCE_PM_Mediciones_Indicador${contador}" placeholder="INGRESAR INDICADOR." class="form-control" autocomplete="off" />
                                            </div>
                                            <div class='col-lg-6'>
                                                <label>Fuente de datos</label>
                                                <input type="text" id="txtCE_PM_Mediciones_FuenteDatos${contador}" placeholder="INGRESAR FUENTE DE DATOS." class="form-control" autocomplete="off" />
                                            </div>
                                        </div>
                                        <div class='row'>
                                            <div class='col-lg-6'>
                                                <label>Responsable</label>
                                                <select id="cboCE_PM_Mediciones_Responsable${contador}" class="form-control medicionesResponsables"></select>
                                            </div>
                                            <div class='col-lg-6'>
                                                <label>Meta</label>
                                                <input type="text" id="txtCE_PM_Mediciones_Meta${contador}" placeholder="INGRESAR META." class="form-control" autocomplete="off" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- END: MEDICIONES Y/O INDICADORES -->`;
                }

                let arrObj = new Array();
                if (cantSecciones > 1) {
                    for (let i = 0; i < cantSecciones; i++) {
                        let obj = new Object();
                        obj = {
                            indicador: $(`#txtCE_PM_Mediciones_Indicador${i}`).val(),
                            fuenteDatos: $(`#txtCE_PM_Mediciones_FuenteDatos${i}`).val(),
                            idUsuarioResponsable: $(`#cboCE_PM_Mediciones_Responsable${i}`).val(),
                            meta: $(`#txtCE_PM_Mediciones_Meta${i}`).val()
                        }
                        arrObj.push(obj);
                    }
                }
                spanNuevaSeccionMedicionIndicador.html(seccionAnterior);
                $(`.medicionesResponsables`).fillCombo('FillUsuarios', {}, false);
                if (cantSecciones > 1) {
                    for (let i = 0; i < cantSecciones; i++) {
                        $(`#txtCE_PM_Mediciones_Indicador${i}`).val(arrObj[i].indicador);
                        $(`#txtCE_PM_Mediciones_FuenteDatos${i}`).val(arrObj[i].fuenteDatos);
                        $(`#cboCE_PM_Mediciones_Responsable${i}`).val(arrObj[i].idUsuarioResponsable);
                        $(`#cboCE_PM_Mediciones_Responsable${i}`).trigger("change");
                        $(`#txtCE_PM_Mediciones_Meta${i}`).val(arrObj[i].meta);
                    }
                }
                $(`.medicionesResponsables`).select2();
                $(`.medicionesResponsables`).select2({ width: '100%' });
            }

            if (lstIndicadores != null) {
                if (cantSecciones > 0) {
                    let contador = 0;
                    for (let i = 0; i < lstIndicadores.length; i++) {
                        contador++
                        $(`#btnCE_PM_Mediciones_EliminarIndicador${contador}`).attr("data-id", lstIndicadores[i].id);
                        $(`#txtCE_PM_Mediciones_Indicador${contador}`).attr("data-id", lstIndicadores[i].id);
                        $(`#txtCE_PM_Mediciones_Indicador${contador}`).val(lstIndicadores[i].indicador);
                        $(`#txtCE_PM_Mediciones_FuenteDatos${contador}`).val(lstIndicadores[i].fuenteDatos);
                        $(`#cboCE_PM_Mediciones_Responsable${contador}`).val(lstIndicadores[i].idUsuarioResponsable);
                        $(`#cboCE_PM_Mediciones_Responsable${contador}`).trigger("change");
                        $(`#txtCE_PM_Mediciones_Meta${contador}`).val(lstIndicadores[i].meta);
                    }
                }
            }
            fncAsignarClasesCrearEditarConceptos();
        }

        function fncEliminarMedicionIndicador(idMedicionIndicador, idDiv) {
            if (idMedicionIndicador > 0) {
                let obj = new Object();
                obj = {
                    idMedicionIndicador: idMedicionIndicador
                }
                axios.post("EliminarMedicionIndicador", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        $(`#mdlMedicionIndicador${idDiv}`).remove();
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                $(`#mdlMedicionIndicador${idDiv}`).remove();
            }
        }

        function fncTitleMdlCEMedicionIndicador(esCrear) {
            if (esCrear) {
                lblTitleCEMedicionIndicador.html(`NUEVO REGISTRO`);
                lblTitleBtnCE_MedicionIndicador.html(`<i class='fas fa-save'></i>&nbsp;Guardar`);
                btnCE_MedicionIndicador.attr('data-id', 0);
            } else {
                lblTitleBtnCE_MedicionIndicador.html(`ACTUALIZAR REGISTRO`);
                lblTitleCEMedicionIndicador.html(`<i class='fas fa-save'></i>&nbsp;Actualizar`);
            }
        }

        function fncCrearEditarMedicionIndicador() {
            let obj = fncObjCEMedicionIndicador();
            if (obj != "") {
                axios.post("CrearEditarMedicionIndicador", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(response.data.message);
                        fncGetDatosActualizarPlanMaestro(btnCE_MedicionIndicador.attr("data-id"))
                        mdlCEMedicionIndicador.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCEMedicionIndicador() {
            fncBorderDefaultMedicionIndicador();
            let strMensajeError = '';
            if (txtCE_Indicador.val() == '') { txtCE_Indicador.css('border', '2px solid red'); strMensajeError = 'Es necesario llenar los campos obligatorios.'; }
            if (txtCE_FuenteDatos.val() == '') { txtCE_FuenteDatos.css('border', '2px solid red'); strMensajeError = 'Es necesario llenar los campos obligatorios.'; }
            if (cboCE_UsuarioResponsable.val() == '') { $('#select2-cboCE_UsuarioResponsable-container').css('border', '2px solid red'); strMensajeError = 'Es necesario llenar los campos obligatorios.'; }
            if (txtCE_Meta.val() == '') { txtCE_Meta.css('border', '2px solid red'); strMensajeError = 'Es necesario llenar los campos obligatorios.'; }

            if (strMensajeError != '') {
                Alert2Warning(strMensajeError);
                return '';
            } else {
                let obj = new Object();
                obj = {
                    idPlanMaestro: btnCE_MedicionIndicador.attr("data-id"),
                    indicador: txtCE_Indicador.val(),
                    fuenteDatos: txtCE_FuenteDatos.val(),
                    idUsuarioResponsable: cboCE_UsuarioResponsable.val(),
                    meta: txtCE_Meta.val()
                };
                return obj;
            }
        }

        function fncBorderDefaultMedicionIndicador() {
            txtCE_Indicador.css('border', '1px solid #CCC');
            txtCE_FuenteDatos.css('border', '1px solid #CCC');
            $('#select2-cboCE_UsuarioResponsable-container').css('border', '1px solid #CCC');
            txtCE_Meta.css('border', '1px solid #CCC');
        }

        function fncLimpiarMdlCEMedicionIndicador() {
            txtCE_Indicador.val("");
            txtCE_FuenteDatos.val("");
            cboCE_UsuarioResponsable[0].selectedIndex = 0;
            cboCE_UsuarioResponsable.trigger('change');
            txtCE_Meta.val("");
        }
        //#endregion

        //#region CRUD AGRUPACIONES
        function fncGetCantAgrupacionesConceptos() {
            if (cboFiltroAnio.val() > 0 && cboFiltroCC.val() > 0) {
                let obj = new Object();
                obj = {
                    anio: cboFiltroAnio.val(),
                    idCC: cboFiltroCC.val()
                }
                axios.post("GetCantAgrupacionesConceptos", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncCrearSeccionAgrupaciones(response.data.lstAgrupaciones, response.data.lstConceptos, response.data.lstCantidades);
                        fncAsignarClasesCrearEditarConceptos();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = '';
                cboFiltroAnio.val() <= 0 ? strMensajeError += 'Es necesario indicar un año.' : '';
                cboFiltroCC.val() <= 0 ? strMensajeError += '<br>Es necesario indicar un CC.' : '';
                Alert2Warning(strMensajeError);
            }
        }

        function fncAsignarClasesCrearEditarConceptos() {
            $(".eliminarRegistro").on("click", function () {
                let idConcepto = $(this).attr("data-id");
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarConcepto(idConcepto));
            });

            $(".editarRegistro").on("click", function () {
                let idConcepto = $(this).attr("data-id");
                fncGetDatosActualizarConcepto(idConcepto);
                mdlCE_PM_RN_Concepto.modal("show");
            });

            $(".btnCE_PM_Mediciones_EliminarIndicador").on("click", function () {
                let idMedicionIndicador = $(this).attr("data-id");
                let idDiv = $(this).attr("idDiv");
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarMedicionIndicador(idMedicionIndicador, idDiv));
            });
        }

        function fncEliminarConcepto(idConcepto) {
            if (idConcepto > 0) {
                let obj = new Object();
                obj = {
                    idConcepto: idConcepto
                }
                axios.post('EliminarRNConcepto', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetCantAgrupacionesConceptos();
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error('Ocurrió un error al eliminar el registro seleccionado.');
            }
        }

        function fncCrearSeccionAgrupaciones(lstAgrupaciones, lstConceptos, lstCantidades) {
            let cantAgrupaciones = lstAgrupaciones.length;
            if (cantAgrupaciones > 0) {

                let inicioRow = "";
                let finRow = "";
                let row = "";
                let plantilla = "";

                inicioRow = `
                    <!-- ROW -->
                    <div class="row">
                        <div class="col-lg-12">
                            <div class='row'>`;

                finRow = `
                            </div>
                        </div>
                    </div>
                    <!-- END: ROW -->`;

                plantilla = inicioRow;

                let splitAgrupacionConcepto = "";
                let lstArregloConceptos = new Array();
                lstConceptos.forEach(element2 => {
                    splitAgrupacionConcepto = element2.split("|");
                    let obj = new Object();
                    obj = {
                        idAgrupacion: splitAgrupacionConcepto[0],
                        concepto: splitAgrupacionConcepto[1],
                        idConcepto: splitAgrupacionConcepto[2]
                    }
                    lstArregloConceptos.push(obj);
                });

                let splitAgrupacionCantidad = "";
                let lstArregloCantidades = new Array();
                lstCantidades.forEach(element2 => {
                    splitAgrupacionCantidad = element2.split("|");
                    let obj = new Object();
                    obj = {
                        idAgrupacion: splitAgrupacionCantidad[0],
                        cantidad: maskNumero_NoDecimal(splitAgrupacionCantidad[1])
                    }
                    lstArregloCantidades.push(obj);
                });

                lstAgrupaciones.forEach(element => {
                    let agrupacion = "";
                    agrupacion = element.split("|");

                    //#region SE OBTIENE LOS CONCEPTOS DE LA AGRUPACIÓN A CARGAR
                    let input = "";
                    let arrConceptos = new Array();
                    lstArregloConceptos.forEach(element2 => {
                        if (agrupacion[0] == element2.idAgrupacion) {
                            arrConceptos.push(`<div class="row">
                                                <div class="col-lg-5">
                                                    <input type="text" class="form-control marginBottom" value="${element2.concepto}" disabled="disabled" />
                                                </div>`);
                        }
                    });
                    //#endregion

                    //#region SE OBTIENE LOS CONCEPTOS DE LA AGRUPACIÓN A CARGAR
                    let arrCantidades = new Array();
                    lstArregloCantidades.forEach(element2 => {
                        if (agrupacion[0] == element2.idAgrupacion) {
                            arrCantidades.push(`<div class="col-lg-3">
                                                    <input type="text" class="form-control marginBottom" value="${element2.cantidad}" disabled="disabled" />
                                                </div>`);
                        }
                    });
                    //#endregion

                    //#region SE ASIGNA ID CONCEPTO A LOS BOTONES EDITAR/ELIMINAR DEL CONCEPTO POR ROW
                    let arrBotonesEditarEliminar = new Array();
                    lstArregloConceptos.forEach(element2 => {
                        if (agrupacion[0] == element2.idAgrupacion) {
                            arrBotonesEditarEliminar.push(`<div class="col-lg-4">
                                                                <button class='btn btn-warning editarRegistro' data-id="${element2.idConcepto}" title='Editar registro.'>
                                                                    <i class='fas fa-pencil-alt'></i></button>&nbsp;
                                                                <button style="display:none;" class='btn btn-danger eliminarRegistro' data-id="${element2.idConcepto}" title='Eliminar registro.'>
                                                                <i class='fas fa-trash'></i></button>
                                                            </div>
                                                        </div>`);
                        }
                    });
                    //#endregion

                    let inputs = "";
                    for (let i = 0; i < arrConceptos.length; i++) {
                        inputs += arrConceptos[i] + arrCantidades[i] + arrBotonesEditarEliminar[i];
                    }

                    let esMatch = "red";
                    if (agrupacion[3]) {
                        esMatch = "green";
                    }
                    row = `
                    <!-- ${agrupacion[1]} -->
                    <div class='col-sm-12'>
                        <div class='panel panel-default panel-principal'>
                            <div class='panel-heading text-center'>
                                <div class="row">
                                    <div class="col-lg-4"></div>
                                    <div class="col-lg-4">
                                        <h4>${agrupacion[1]}</h4>
                                    </div>
                                    <div class="col-lg-2"></div>
                                    <div class="col-lg-2">
                                        <h4 style="color: ${esMatch}; font-weight: bold; background-color: white;">${agrupacion[2]}</h4>
                                    </div>
                                </div>
                            </div>
                            <div class='panel-body'>
                                ${inputs}
                            </div>
                        </div>
                    </div>
                    <!-- END: ${agrupacion[1]} -->`;

                    plantilla += row;
                });
                plantilla += finRow;
                spanNuevaSeccionAgrupaciones.html(plantilla);
            }
        }

        function fncCE_PM_RN_Agrupacion() {
            let obj = fncObjCE_PM_RN_Agrupacion();
            if (obj != '') {
                axios.post('CrearEditarRNAgrupacion', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        mdlCE_PM_RN_Agrupacion.modal('hide');
                        fncGetCantAgrupacionesConceptos();
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCE_PM_RN_Agrupacion() {
            fncBorderDefaultAgrupacion();
            let strMensajeError = '';
            if (txtCE_PM_RN_Agrupacion.val() == '') { txtCE_PM_RN_Agrupacion.css('border', '2px solid red'); strMensajeError = 'Es necesario llenar los campos obligatorios.'; }

            if (strMensajeError != '') {
                Alert2Warning(strMensajeError);
                return '';
            } else {
                let obj = new Object();
                obj = {
                    id: btnCE_PM_RN_Agrupacion.attr('data-id'),
                    anio: cboCE_PM_RN_Anio.val(),
                    idCC: cboCE_PM_RN_CC.val(),
                    agrupacion: txtCE_PM_RN_Agrupacion.val()
                };
                return obj;
            }
        }

        function fncTitleMdlCEAgrupacion(esCrear) {
            if (esCrear) {
                lblTitleCE_PM_RN_Agrupacion.html(`NUEVO REGISTRO`);
                lblTitleBtnCE_PM_RN_Agrupacion.html(`<i class='fas fa-save'></i>&nbsp;Guardar`);
                btnCE_PM_RN_Agrupacion.attr('data-id', 0);
            } else {
                lblTitleBtnCE_PM_RN_Agrupacion.html(`ACTUALIZAR REGISTRO`);
                lblTitleCE_PM_RN_Agrupacion.html(`<i class='fas fa-save'></i>&nbsp;Actualizar`);
            }
        }

        function fncLimpiarMdlCEAgrupacion() {
            cboCE_PM_RN_Anio[0].selectedIndex = 0;
            cboCE_PM_RN_Anio.trigger('change');

            cboCE_PM_RN_CC[0].selectedIndex = 0;
            cboCE_PM_RN_CC.trigger('change');

            txtCE_PM_RN_Agrupacion.val("");
        }

        function fncBorderDefaultAgrupacion() {
            $('#select2-cboCE_PM_RN_Anio-container').css('border', '1px solid #CCC');
            $('#select2-cboCE_PM_RN_CC-container').css('border', '1px solid #CCC');
            txtCE_PM_RN_Agrupacion.css('border', '1px solid #CCC');
        }
        //#endregion

        //#region CRUD CONCEPTOS
        function fncTitleMdlCEConcepto(esCrear) {
            if (esCrear) {
                lblTitleCE_PM_RN_Concepto.html(`NUEVO REGISTRO`);
                lblTitleBtnCE_PM_RN_Concepto.html(`<i class='fas fa-save'></i>&nbsp;Guardar`);
                btnCE_PM_RN_Concepto.attr('data-id', 0);
            } else {
                lblTitleCE_PM_RN_Concepto.html(`ACTUALIZAR REGISTRO`);
                lblTitleBtnCE_PM_RN_Concepto.html(`<i class='fas fa-save'></i>&nbsp;Actualizar`);
            }
        }

        function fncLimpiarMdlCEConcepto() {
            cboCE_PM_RN_ConceptoAnio[0].selectedIndex = 0;
            cboCE_PM_RN_ConceptoAnio.trigger('change');

            cboCE_PM_RN_ConceptoCC[0].selectedIndex = 0;
            cboCE_PM_RN_ConceptoCC.trigger('change');

            txtCE_PM_RN_ConceptoConcepto.val("");
            txtCE_PM_RN_ConceptoCantidad.val("");
        }

        function fncBorderDefaultConcepto() {
            $('#select2-cboCEConceptoAnio-container').css('border', '1px solid #CCC');
            $('#select2-cboCEConceptoCC-container').css('border', '1px solid #CCC');
            txtCE_PM_RN_ConceptoConcepto.css('border', '1px solid #CCC');
        }

        function fncCE_PM_RN_Concepto() {
            let obj = fncObjCE_PM_RN_Concepto();
            if (obj != '') {
                axios.post('CrearEditarRNConcepto', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetCantAgrupacionesConceptos();
                        mdlCE_PM_RN_Concepto.modal('hide');
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCE_PM_RN_Concepto() {
            fncBorderDefaultConcepto();
            let strMensajeError = '';
            if (txtCE_PM_RN_ConceptoConcepto.val() == '') { txtCE_PM_RN_ConceptoConcepto.css('border', '2px solid red'); strMensajeError = 'Es necesario llenar los campos obligatorios.'; }

            if (strMensajeError != '') {
                Alert2Warning(strMensajeError);
                return '';
            } else {
                let obj = new Object();
                obj = {
                    id: btnCE_PM_RN_Concepto.attr('data-id'),
                    idRNAgrupacion: cboCE_PM_RN_ConceptoAgrupacion.val(),
                    concepto: txtCE_PM_RN_ConceptoConcepto.val(),
                    cantidad: unmaskNumero(txtCE_PM_RN_ConceptoCantidad.val())
                };
                return obj;
            }
        }

        function fncGetDatosActualizarConcepto(idConcepto) {
            if (idConcepto > 0) {
                let obj = new Object();
                obj = {
                    idConcepto: idConcepto
                }
                axios.post('GetDatosActualizarRNConcepto', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        cboCE_PM_RN_ConceptoAnio.fillCombo('FillAnios', {}, false);
                        cboCE_PM_RN_ConceptoCC.fillCombo('FillUsuarioRelCC', { anio: cboFiltroAnio.val() }, false);
                        cboCE_PM_RN_ConceptoAgrupacion.fillCombo('FillAgrupaciones', { anio: cboFiltroAnio.val(), idCC: cboFiltroCC.val() }, false);

                        btnCE_PM_RN_Concepto.attr('data-id', idConcepto);
                        cboCE_PM_RN_ConceptoAnio.val(response.data.objConcepto.anio);
                        cboCE_PM_RN_ConceptoAnio.trigger('change');
                        cboCE_PM_RN_ConceptoCC.val(response.data.objConcepto.idCC);
                        cboCE_PM_RN_ConceptoCC.trigger('change');
                        cboCE_PM_RN_ConceptoAgrupacion.val(response.data.objConcepto.idAgrupacion);
                        cboCE_PM_RN_ConceptoAgrupacion.trigger("change");
                        txtCE_PM_RN_ConceptoConcepto.val(response.data.objConcepto.concepto);
                        txtCE_PM_RN_ConceptoCantidad.val(maskNumero_NoDecimal(response.data.objConcepto.cantPpto));
                        fncTitleMdlCEConcepto(false);
                        mdlCE_PM_RN_Concepto.modal('show');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error('Ocurrió un error al obtener la información.')
            }
        }
        //#endregion

        //#region REPORTE
        function fncGetReporte(idReg, anioVal, idCCVal) {
            var path = `/Reportes/Vista.aspx?idReporte=251&idReg=${idReg}&year=${anioVal}&cc=${idCCVal}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        //#endregion
    }

    $(document).ready(() => {
        CtrlPptalOfCE.PlanMaestro = new PlanMaestro();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();