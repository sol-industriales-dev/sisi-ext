(() => {
    $.namespace('CtrlPptalOfCE.Capturas');

    //#region CONST AGRUPACIONES
    const cboFiltroAnio = $("#cboFiltroAnio");
    const cboFiltroCC = $("#cboFiltroCC");
    const btnFiltroBuscar = $("#btnFiltroBuscar");
    const btnFiltroNuevo = $("#btnFiltroNuevo");
    const tblAF_CtrlPptalOfCe_RN_CatAgrupaciones = $("#tblAF_CtrlPptalOfCe_RN_CatAgrupaciones");
    const mdlCEAgrupacion = $("#mdlCEAgrupacion");
    const lblTitleCEAgrupacion = $("#lblTitleCEAgrupacion");
    const cboCEAnio = $("#cboCEAnio");
    const cboCECC = $("#cboCECC");
    const txtCEAgrupacion = $("#txtCEAgrupacion");
    const btnCEAgrupacion = $("#btnCEAgrupacion");
    const lblTitleBtnCEAgrupacion = $("#lblTitleBtnCEAgrupacion");
    let dtAgrupaciones;
    //#endregion

    //#region CONST CONCEPTOS
    const mdlListadoConceptos = $('#mdlListadoConceptos');
    const lblTitleListadoConceptos = $("#lblTitleListadoConceptos");
    const tblAF_CtrlPptalOfCe_RN_CatConceptos = $("#tblAF_CtrlPptalOfCe_RN_CatConceptos");
    const mdlCEConcepto = $("#mdlCEConcepto");
    const lblTitleCEConcepto = $("#lblTitleCEConcepto");
    const cboCEConceptoAnio = $("#cboCEConceptoAnio");
    const cboCEConceptoCC = $("#cboCEConceptoCC");
    const txtCEConceptoConcepto = $("#txtCEConceptoConcepto");
    const txtCEConceptoCantidad = $("#txtCEConceptoCantidad");
    const btnCEConcepto = $("#btnCEConcepto");
    const lblTitleBtnCEConcepto = $("#lblTitleBtnCEConcepto");
    const btnFiltroNuevoConcepto = $('#btnFiltroNuevoConcepto');
    const cboCEConceptoAgrupacion = $('#cboCEConceptoAgrupacion');
    let dtConceptos;
    //#endregion

    Capturas = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            initTablaAgrupaciones();
            initTablaConceptos();
            //#endregion

            //#region EVENTOS AGRUPACIONES
            btnFiltroNuevo.on("click", function () {
                fncBorderDefaultAgrupacion();
                fncLimpiarMdlCEAgrupacion();
                fncTitleMdlCEAgrupacion(true);

                //#region VALIDACIÓN: ES NECESARIO SELECCIONAR UN AÑO Y UN CC EN LOS FILTROS.
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
                    $('#select2-cboFiltroAnio-container').css('border', '1px solid #CCC');
                    $('#select2-cboFiltroCC-container').css('border', '1px solid #CCC');
                    cboCEAnio.val(cboFiltroAnio.val());
                    cboCEAnio.trigger("change");
                    cboCECC.val(cboFiltroCC.val());
                    cboCECC.trigger("change");
                    mdlCEAgrupacion.modal("show");
                }
                //#endregion
            });

            btnCEAgrupacion.on('click', function () {
                fncCEAgrupacion();
            });

            btnFiltroBuscar.on('click', function () {
                fncGetAgrupaciones();
            });
            //#endregion

            //#region FILL COMBOS AGRUPACIONES
            cboFiltroAnio.fillCombo('FillAnios', {}, false);
            cboCEAnio.fillCombo('FillAnios', {}, false);

            cboFiltroAnio.on("change", function () {
                if ($(this).val() > 0) {
                    cboFiltroCC.fillCombo('FillUsuarioRelCC', { anio: $(this).val() }, false);
                    cboCECC.fillCombo('FillUsuarioRelCC', { anio: $(this).val() }, false);
                }
            });
            
            //#endregion

            //#region EVENTOS CONCEPTOS
            btnFiltroNuevoConcepto.on("click", function () {
                fncBorderDefaultConcepto();
                fncLimpiarMdlCEConcepto();
                fncTitleMdlCEConcepto(true);

                //#region VALIDACIÓN: ES NECESARIO SELECCIONAR UN AÑO Y UN CC EN LOS FILTROS.
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
                    $('#select2-cboFiltroAnio-container').css('border', '1px solid #CCC');
                    $('#select2-cboFiltroCC-container').css('border', '1px solid #CCC');
                    cboCEConceptoAnio.val(cboFiltroAnio.val());
                    cboCEConceptoAnio.trigger("change");
                    cboCEConceptoCC.val(cboFiltroCC.val());
                    cboCEConceptoCC.trigger("change");
                    mdlCEConcepto.modal("show");
                }
                //#endregion
            });

            btnCEConcepto.on('click', function () {
                fncCEConcepto();
            });

            //#region FILL COMBOS AGRUPACIONES
            cboCEConceptoAnio.fillCombo('FillAnios', {}, false);
            cboCEConceptoCC.fillCombo('FillUsuarioRelCC', {}, false);
            //#endregion
            //#endregion

            $('.select2').select2();
            $(".select2").select2({ width: '100%' });
        }

        //#region CRUD AGRUPACIONES
        function initTablaAgrupaciones() {
            dtAgrupaciones = tblAF_CtrlPptalOfCe_RN_CatAgrupaciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'agrupacion', title: 'AGRUPACIÓN' },
                    { 
                        title: "Conceptos",
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-primary verConceptos' title='Conceptos.'><i class="fas fa-list-ul"></i></button>&nbsp;`;
                        },
                    },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEditar + btnEliminar;
                        },
                    },
                    { data: 'id', title: 'id', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblAF_CtrlPptalOfCe_RN_CatAgrupaciones.on('click', '.editarRegistro', function () {
                        let rowData = dtAgrupaciones.row($(this).closest('tr')).data();
                        fncBorderDefaultAgrupacion();
                        fncGetDatosActualizarAgrupacion(rowData.id);
                    });

                    tblAF_CtrlPptalOfCe_RN_CatAgrupaciones.on('click', '.eliminarRegistro', function () {
                        let rowData = dtAgrupaciones.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarAgrupacion(rowData.id));
                    });

                    tblAF_CtrlPptalOfCe_RN_CatAgrupaciones.on('click', '.verConceptos', function () {
                        let rowData = dtAgrupaciones.row($(this).closest('tr')).data();
                        btnFiltroBuscar.attr("data-id", rowData.id);
                        cboCEConceptoAgrupacion.fillCombo('FillRNAgrupaciones', { anio: cboFiltroAnio.val(), idCC: cboFiltroCC.val() }, false);
                        cboCEConceptoAgrupacion.val(rowData.id);
                        cboCEConceptoAgrupacion.trigger("change");
                        // let nombreAgrupacion = selectAgrupacion.find('option:selected').text();
                        lblTitleListadoConceptos.html(`CONCEPTOS - ${rowData.agrupacion}`);
                        fncGetConceptos();
                        mdlListadoConceptos.modal("show");
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    // { className: 'dt-body-center', targets: [0] },
                    // { className: 'dt-body-right', targets: [0] },
                    // { width: '5%', targets: [0] }
                ],
            });
        }

        function fncGetAgrupaciones() {
            if (cboFiltroAnio.val() > 0 && cboFiltroCC.val() > 0) {
                let obj = new Object();
                obj = {
                    anio: cboFiltroAnio.val(),
                    idCC: cboFiltroCC.val()
                }
                axios.post('GetRNAgrupaciones', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtAgrupaciones.clear();
                        dtAgrupaciones.rows.add(response.data.lstAgrupaciones);
                        dtAgrupaciones.draw();
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

        function fncCEAgrupacion() {
            let obj = fncObjCEAgrupacion();
            if (obj != '') {
                axios.post('CrearEditarRNAgrupacion', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetAgrupaciones();
                        mdlCEAgrupacion.modal('hide');
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCEAgrupacion() {
            fncBorderDefaultAgrupacion();
            let strMensajeError = '';
            if (txtCEAgrupacion.val() == '') { txtCEAgrupacion.css('border', '2px solid red'); strMensajeError = 'Es necesario llenar los campos obligatorios.'; }

            if (strMensajeError != '') {
                Alert2Warning(strMensajeError);
                return '';
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEAgrupacion.attr('data-id'),
                    anio: cboCEAnio.val(),
                    idCC: cboCECC.val(),
                    agrupacion: txtCEAgrupacion.val()
                };
                return obj;
            }
        }

        function fncEliminarAgrupacion(idAgrupacion) {
            if (idAgrupacion > 0) {
                let obj = new Object();
                obj = {
                    idAgrupacion: idAgrupacion
                }
                axios.post('EliminarRNAgrupacion', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetAgrupaciones();
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error('Ocurrió un error al eliminar el registro seleccionado.');
            }
        }

        function fncGetDatosActualizarAgrupacion(idAgrupacion) {
            if (idAgrupacion > 0) {
                let obj = new Object();
                obj = {
                    idAgrupacion: idAgrupacion
                }
                axios.post('GetDatosActualizarRNAgrupacion', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnCEAgrupacion.attr('data-id', idAgrupacion);
                        cboCEAnio.val(response.data.objAgrupacion.anio);
                        cboCEAnio.trigger('change');
                        cboCECC.val(response.data.objAgrupacion.idCC);
                        cboCECC.trigger('change');
                        txtCEAgrupacion.val(response.data.objAgrupacion.agrupacion);
                        fncTitleMdlCEAgrupacion(false);
                        mdlCEAgrupacion.modal('show');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error('Ocurrió un error al obtener la información.')
            }
        }

        function fncTitleMdlCEAgrupacion(esCrear) {
            if (esCrear) {
                lblTitleCEAgrupacion.html(`NUEVO REGISTRO`);
                lblTitleBtnCEAgrupacion.html(`<i class='fas fa-save'></i>&nbsp;Guardar`);
                btnCEAgrupacion.attr('data-id', 0);
            } else {
                lblTitleCEAgrupacion.html(`ACTUALIZAR REGISTRO`);
                lblTitleBtnCEAgrupacion.html(`<i class='fas fa-save'></i>&nbsp;Actualizar`);
            }
        }

        function fncLimpiarMdlCEAgrupacion() {
            $('input[type="text"]').val('');

            cboCEAnio[0].selectedIndex = 0;
            cboCEAnio.trigger('change');

            cboCECC[0].selectedIndex = 0;
            cboCECC.trigger('change');
        }

        function fncBorderDefaultAgrupacion() {
            $('#select2-cboCEAnio-container').css('border', '1px solid #CCC');
            $('#select2-cboCECC-container').css('border', '1px solid #CCC');
            txtCEAgrupacion.css('border', '1px solid #CCC');
        }
        //#endregion

        //#region CONCEPTOS
        function initTablaConceptos() {
            dtConceptos = tblAF_CtrlPptalOfCe_RN_CatConceptos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'concepto', title: 'CONCEPTO' },
                    { 
                        data: 'cantidad', title: 'CANTIDAD',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEditar + btnEliminar;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblAF_CtrlPptalOfCe_RN_CatConceptos.on('click','.editarRegistro', function () {
                        let rowData = dtConceptos.row($(this).closest('tr')).data();
                        fncGetDatosActualizarConcepto(rowData.id);
                    });

                    tblAF_CtrlPptalOfCe_RN_CatConceptos.on('click','.eliminarRegistro', function () {
                        let rowData = dtConceptos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncEliminarConcepto(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center','targets': [0,2] },
                    //{ className: 'dt-body-center', targets: [0] },
                    { className: 'dt-body-right', targets: [1] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }
        
        function fncGetConceptos() {
            if (true) {
                let obj = new Object();
                obj = {
                    idRNAgrupacion: btnFiltroBuscar.attr("data-id")
                }
                axios.post('GetRNConceptos', obj).then(response => {
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
            } else {
                let strMensajeError = '';
                idAgrupacion <= 0 ? strMensajeError = 'Ocurrió un error al consultar los conceptos.' : '';
                Alert2Warning(strMensajeError);
            }
        }

        function fncCEConcepto() {
            let obj = fncObjCEConcepto();
            if (obj != '') {
                axios.post('CrearEditarRNConcepto', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetConceptos();
                        mdlCEConcepto.modal('hide');
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCEConcepto() {
            fncBorderDefaultConcepto();
            let strMensajeError = '';
            if (txtCEConceptoConcepto.val() == '') { txtCEConceptoConcepto.css('border', '2px solid red'); strMensajeError = 'Es necesario llenar los campos obligatorios.'; }
            // if (txtCEConceptoCantidad.val() == '') { txtCEConceptoCantidad.css('border', '2px solid red'); strMensajeError = 'Es necesario llenar los campos obligatorios.'; }

            if (strMensajeError != '') {
                Alert2Warning(strMensajeError);
                return '';
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEConcepto.attr('data-id'),
                    idRNAgrupacion: btnFiltroBuscar.attr("data-id"),
                    concepto: txtCEConceptoConcepto.val(),
                    cantidad: txtCEConceptoCantidad.val()
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
                axios.post('EliminarRNConcepto', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetConceptos();
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error('Ocurrió un error al eliminar el registro seleccionado.');
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
                        btnCEConcepto.attr('data-id', idConcepto);
                        cboCEConceptoAnio.val(response.data.objConcepto.anio);
                        cboCEConceptoAnio.trigger('change');
                        cboCEConceptoCC.val(response.data.objConcepto.idCC);
                        cboCEConceptoCC.trigger('change');
                        txtCEConceptoConcepto.val(response.data.objConcepto.concepto);
                        txtCEConceptoCantidad.val(response.data.objConcepto.cantidad);
                        fncTitleMdlCEConcepto(false);
                        mdlCEConcepto.modal('show');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error('Ocurrió un error al obtener la información.')
            }
        }

        function fncTitleMdlCEConcepto(esCrear) {
            if (esCrear) {
                lblTitleCEConcepto.html(`NUEVO REGISTRO`);
                lblTitleBtnCEConcepto.html(`<i class='fas fa-save'></i>&nbsp;Guardar`);
                btnCEConcepto.attr('data-id', 0);
            } else {
                lblTitleCEConcepto.html(`ACTUALIZAR REGISTRO`);
                lblTitleBtnCEConcepto.html(`<i class='fas fa-save'></i>&nbsp;Actualizar`);
            }
        }

        function fncLimpiarMdlCEConcepto() {
            $('input[type="text"]').val('');

            cboCEConceptoAnio[0].selectedIndex = 0;
            cboCEConceptoAnio.trigger('change');

            cboCEConceptoCC[0].selectedIndex = 0;
            cboCEConceptoCC.trigger('change');
        }

        function fncBorderDefaultConcepto() {
            $('#select2-cboCEConceptoAnio-container').css('border', '1px solid #CCC');
            $('#select2-cboCEConceptoCC-container').css('border', '1px solid #CCC');
            txtCEConceptoConcepto.css('border', '1px solid #CCC');
            // txtCEConceptoCantidad.css('border', '1px solid #CCC');
        }
        //#endregion
    }

    $(document).ready(() => {
        CtrlPptalOfCE.Capturas = new Capturas();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();