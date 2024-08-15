(() => {
    $.namespace('CtrlPptalOfCE.EstimadoRealMensual');

    //#region CONST FILTROS
    const cboFiltroAnio = $('#cboFiltroAnio');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    //#endregion

    //#region CONST CE ESTIMADO REAL MENSUAL
    let dtEstimadoRealMensual;
    const tblEstimadoRealMensual = $('#tblEstimadoRealMensual');
    const txtCE_Enero = $("#txtCE_Enero");
    const txtCE_Febrero = $("#txtCE_Febrero");
    const txtCE_Marzo = $("#txtCE_Marzo");
    const txtCE_Abril = $("#txtCE_Abril");
    const txtCE_Mayo = $("#txtCE_Mayo");
    const txtCE_Junio = $("#txtCE_Junio");
    const txtCE_Julio = $("#txtCE_Julio");
    const txtCE_Agosto = $("#txtCE_Agosto");
    const txtCE_Septiembre = $("#txtCE_Septiembre");
    const txtCE_Octubre = $("#txtCE_Octubre");
    const txtCE_Noviembre = $("#txtCE_Noviembre");
    const txtCE_Diciembre = $("#txtCE_Diciembre");
    const btnCEEstimadoRealMensual = $('#btnCEEstimadoRealMensual');
    const mdlCEEstimadoRealMensual = $('#mdlCEEstimadoRealMensual');
    //#endregion

    EstimadoRealMensual = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblEstimadoRealMensual();

            $(".select2").select2();
            //#endregion

            //#region FILTROS
            btnFiltroBuscar.click(function () {
                fncGetListadoEstimadoRealMensual();
            })
            //#endregion

            //#region CE ESTIMADO REAL MENSUAL
            $(".tipoMoneda").change(function () {
                let Enero = unmaskNumero6DCompras(txtCE_Enero.val());
                txtCE_Enero.val(maskNumero2DCompras(Enero));

                let Febrero = unmaskNumero6DCompras(txtCE_Febrero.val());
                txtCE_Febrero.val(maskNumero2DCompras(Febrero));

                let Marzo = unmaskNumero6DCompras(txtCE_Marzo.val());
                txtCE_Marzo.val(maskNumero2DCompras(Marzo));

                let Abril = unmaskNumero6DCompras(txtCE_Abril.val());
                txtCE_Abril.val(maskNumero2DCompras(Abril));

                let Mayo = unmaskNumero6DCompras(txtCE_Mayo.val());
                txtCE_Mayo.val(maskNumero2DCompras(Mayo));

                let Junio = unmaskNumero6DCompras(txtCE_Junio.val());
                txtCE_Junio.val(maskNumero2DCompras(Junio));

                let Julio = unmaskNumero6DCompras(txtCE_Julio.val());
                txtCE_Julio.val(maskNumero2DCompras(Julio));

                let Agosto = unmaskNumero6DCompras(txtCE_Agosto.val());
                txtCE_Agosto.val(maskNumero2DCompras(Agosto));

                let Septiembre = unmaskNumero6DCompras(txtCE_Septiembre.val());
                txtCE_Septiembre.val(maskNumero2DCompras(Septiembre));

                let Octubre = unmaskNumero6DCompras(txtCE_Octubre.val());
                txtCE_Octubre.val(maskNumero2DCompras(Octubre));

                let Noviembre = unmaskNumero6DCompras(txtCE_Noviembre.val());
                txtCE_Noviembre.val(maskNumero2DCompras(Noviembre));

                let Diciembre = unmaskNumero6DCompras(txtCE_Diciembre.val());
                txtCE_Diciembre.val(maskNumero2DCompras(Diciembre));
            });

            btnCEEstimadoRealMensual.click(function () {
                fncCEEstimadoRealMensual();
            });
            //#endregion
        }

        //#region CE ESTIMADO REAL MENSUAL
        function initTblEstimadoRealMensual() {
            dtEstimadoRealMensual = tblEstimadoRealMensual.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'anio', title: 'Año' },
                    { title: 'Enero', render: (data, type, row, meta) => { return maskNumero2DCompras(row.estimadoReal_Enero); } },
                    { title: 'Febrero', render: (data, type, row, meta) => { return maskNumero2DCompras(row.estimadoReal_Febrero); } },
                    { title: 'Marzo', render: (data, type, row, meta) => { return maskNumero2DCompras(row.estimadoReal_Marzo); } },
                    { title: 'Abril', render: (data, type, row, meta) => { return maskNumero2DCompras(row.estimadoReal_Abril); } },
                    { title: 'Mayo', render: (data, type, row, meta) => { return maskNumero2DCompras(row.estimadoReal_Mayo); } },
                    { title: 'Junio', render: (data, type, row, meta) => { return maskNumero2DCompras(row.estimadoReal_Junio); } },
                    { title: 'Julio', render: (data, type, row, meta) => { return maskNumero2DCompras(row.estimadoReal_Julio); } },
                    { title: 'Agosto', render: (data, type, row, meta) => { return maskNumero2DCompras(row.estimadoReal_Agosto); } },
                    { title: 'Septiembre', render: (data, type, row, meta) => { return maskNumero2DCompras(row.estimadoReal_Septiembre); } },
                    { title: 'Octubre', render: (data, type, row, meta) => { return maskNumero2DCompras(row.estimadoReal_Octubre); } },
                    { title: 'Noviembre', render: (data, type, row, meta) => { return maskNumero2DCompras(row.estimadoReal_Noviembre); } },
                    { title: 'Diciembre', render: (data, type, row, meta) => { return maskNumero2DCompras(row.estimadoReal_Diciembre); } },
                    {
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>`;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblEstimadoRealMensual.on('click', '.editarRegistro', function () {
                        let rowData = dtEstimadoRealMensual.row($(this).closest('tr')).data();
                        fncGetDatosActualizarEstimadoRealMensual(rowData.id);
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetListadoEstimadoRealMensual() {
            if (cboFiltroAnio.val() > 0) {
                let obj = {};
                obj.anio = cboFiltroAnio.val();
                axios.post('GetListadoEstimadoRealMensual', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtEstimadoRealMensual.clear();
                        dtEstimadoRealMensual.rows.add(response.data.lstEstimadoRealMensualDTO);
                        dtEstimadoRealMensual.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Es necesario seleccionar un año.");
            }
        }

        function fncCEEstimadoRealMensual() {
            let obj = fncCEObjEstimadoRealMensual();
            if (obj != null) {
                axios.post('CEEstimadoRealMensual', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetListadoEstimadoRealMensual();
                        mdlCEEstimadoRealMensual.modal("hide");
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {

            }
        }

        function fncCEObjEstimadoRealMensual() {
            fncDefaultCtrls("txtCE_Enero");
            fncDefaultCtrls("txtCE_Febrero");
            fncDefaultCtrls("txtCE_Marzo");
            fncDefaultCtrls("txtCE_Abril");
            fncDefaultCtrls("txtCE_Mayo");
            fncDefaultCtrls("txtCE_Junio");
            fncDefaultCtrls("txtCE_Julio");
            fncDefaultCtrls("txtCE_Agosto");
            fncDefaultCtrls("txtCE_Septiembre");
            fncDefaultCtrls("txtCE_Octubre");
            fncDefaultCtrls("txtCE_Noviembre");
            fncDefaultCtrls("txtCE_Diciembre");

            if (btnCEEstimadoRealMensual.data().id <= 0) { Alert2Error("Ocurrió un error al actualizar la información."); return null; }
            if (unmaskNumero6DCompras(txtCE_Enero.val()) < 0) { fncValidacionCtrl("txtCE_Enero", false, "Favor de ingresar solamente número positivos en enero."); return null; }
            if (unmaskNumero6DCompras(txtCE_Febrero.val()) < 0) { fncValidacionCtrl("txtCE_Febrero", false, "Favor de ingresar solamente número positivos en febrero."); return null; }
            if (unmaskNumero6DCompras(txtCE_Marzo.val()) < 0) { fncValidacionCtrl("txtCE_Marzo", false, "Favor de ingresar solamente número positivos en marzo."); return null; }
            if (unmaskNumero6DCompras(txtCE_Abril.val()) < 0) { fncValidacionCtrl("txtCE_Abril", false, "Favor de ingresar solamente número positivos en abril."); return null; }
            if (unmaskNumero6DCompras(txtCE_Mayo.val()) < 0) { fncValidacionCtrl("txtCE_Mayo", false, "Favor de ingresar solamente número positivos en mayo."); return null; }
            if (unmaskNumero6DCompras(txtCE_Junio.val()) < 0) { fncValidacionCtrl("txtCE_Junio", false, "Favor de ingresar solamente número positivos en junio."); return null; }
            if (unmaskNumero6DCompras(txtCE_Julio.val()) < 0) { fncValidacionCtrl("txtCE_Julio", false, "Favor de ingresar solamente número positivos en julio."); return null; }
            if (unmaskNumero6DCompras(txtCE_Agosto.val()) < 0) { fncValidacionCtrl("txtCE_Agosto", false, "Favor de ingresar solamente número positivos en agosto."); return null; }
            if (unmaskNumero6DCompras(txtCE_Septiembre.val()) < 0) { fncValidacionCtrl("txtCE_Septiembre", false, "Favor de ingresar solamente número positivos en septiembre."); return null; }
            if (unmaskNumero6DCompras(txtCE_Octubre.val()) < 0) { fncValidacionCtrl("txtCE_Octubre", false, "Favor de ingresar solamente número positivos en octubre."); return null; }
            if (unmaskNumero6DCompras(txtCE_Noviembre.val()) < 0) { fncValidacionCtrl("txtCE_Noviembre", false, "Favor de ingresar solamente número positivos en noviembre."); return null; }
            if (unmaskNumero6DCompras(txtCE_Diciembre.val()) < 0) { fncValidacionCtrl("txtCE_Diciembre", false, "Favor de ingresar solamente número positivos en diciembre."); return null; }

            let obj = {};
            obj.id = btnCEEstimadoRealMensual.data().id;
            obj.anio = cboFiltroAnio.val();
            obj.estimadoReal_Enero = unmaskNumero6DCompras(txtCE_Enero.val());
            obj.estimadoReal_Febrero = unmaskNumero6DCompras(txtCE_Febrero.val());
            obj.estimadoReal_Marzo = unmaskNumero6DCompras(txtCE_Marzo.val());
            obj.estimadoReal_Abril = unmaskNumero6DCompras(txtCE_Abril.val());
            obj.estimadoReal_Mayo = unmaskNumero6DCompras(txtCE_Mayo.val());
            obj.estimadoReal_Junio = unmaskNumero6DCompras(txtCE_Junio.val());
            obj.estimadoReal_Julio = unmaskNumero6DCompras(txtCE_Julio.val());
            obj.estimadoReal_Agosto = unmaskNumero6DCompras(txtCE_Agosto.val());
            obj.estimadoReal_Septiembre = unmaskNumero6DCompras(txtCE_Septiembre.val());
            obj.estimadoReal_Octubre = unmaskNumero6DCompras(txtCE_Octubre.val());
            obj.estimadoReal_Noviembre = unmaskNumero6DCompras(txtCE_Noviembre.val());
            obj.estimadoReal_Diciembre = unmaskNumero6DCompras(txtCE_Diciembre.val());
            return obj;
        }

        function fncGetDatosActualizarEstimadoRealMensual(id) {
            if (id > 0) {
                let obj = {};
                obj.id = id;
                axios.post('GetDatosActualizarEstimadoRealMensual', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncLimpiarMdlCEEstimadoRealMensual();
                        txtCE_Enero.val(maskNumero2DCompras(response.data.objEstimadoRealMensualDTO.estimadoReal_Enero));
                        txtCE_Febrero.val(maskNumero2DCompras(response.data.objEstimadoRealMensualDTO.estimadoReal_Febrero));
                        txtCE_Marzo.val(maskNumero2DCompras(response.data.objEstimadoRealMensualDTO.estimadoReal_Marzo));
                        txtCE_Abril.val(maskNumero2DCompras(response.data.objEstimadoRealMensualDTO.estimadoReal_Abril));
                        txtCE_Mayo.val(maskNumero2DCompras(response.data.objEstimadoRealMensualDTO.estimadoReal_Mayo));
                        txtCE_Junio.val(maskNumero2DCompras(response.data.objEstimadoRealMensualDTO.estimadoReal_Junio));
                        txtCE_Julio.val(maskNumero2DCompras(response.data.objEstimadoRealMensualDTO.estimadoReal_Julio));
                        txtCE_Agosto.val(maskNumero2DCompras(response.data.objEstimadoRealMensualDTO.estimadoReal_Agosto));
                        txtCE_Septiembre.val(maskNumero2DCompras(response.data.objEstimadoRealMensualDTO.estimadoReal_Septiembre));
                        txtCE_Octubre.val(maskNumero2DCompras(response.data.objEstimadoRealMensualDTO.estimadoReal_Octubre));
                        txtCE_Noviembre.val(maskNumero2DCompras(response.data.objEstimadoRealMensualDTO.estimadoReal_Noviembre));
                        txtCE_Diciembre.val(maskNumero2DCompras(response.data.objEstimadoRealMensualDTO.estimadoReal_Diciembre));
                        btnCEEstimadoRealMensual.data().id = response.data.objEstimadoRealMensualDTO.id;
                        mdlCEEstimadoRealMensual.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener la información del registro.");
            }
        }
    }

    function fncLimpiarMdlCEEstimadoRealMensual() {
        $('input[type="text"]').val("");
    }
    //#endregion

    $(document).ready(() => {
        CtrlPptalOfCE.EstimadoRealMensual = new EstimadoRealMensual();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();