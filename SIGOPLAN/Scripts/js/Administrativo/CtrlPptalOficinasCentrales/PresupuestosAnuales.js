(() => {
    $.namespace('CH.PresupuestosAnuales');

    PresupuestosAnuales = function () {
        //#region Selectores
        const tablaPresupuestos = $('#tablaPresupuestos');
        const botonAgregar = $('#botonAgregar');
        const modalPresupuesto = $('#modalPresupuesto');
        const selectCentroCosto = $('#selectCentroCosto');
        const inputAnio = $('#inputAnio');
        const inputNombrePresupuesto = $('#inputNombrePresupuesto');
        const botonGuardar = $('#botonGuardar');
        //#endregion

        //#region CONST PPTO INICIAL
        const btnBuscar = $('#btnBuscar');
        const mdlCEPptoInicial = $("#mdlCEPptoInicial");
        const lblTitleCEPptoInicial = $("#lblTitleCEPptoInicial");
        const txt_pptoInicial_anio = $("#txt_pptoInicial_anio");
        const txt_pptoInicial_nombrePpto = $("#txt_pptoInicial_nombrePpto");
        const txt_pptoInicial_fechaInicio = $("#txt_pptoInicial_fechaInicio");
        const txt_pptoInicial_fechaFin = $("#txt_pptoInicial_fechaFin");
        const txt_pptoInicial_fechaInicioLimite = $("#txt_pptoInicial_fechaInicioLimite");
        const txt_pptoInicial_fechaFinLimite = $("#txt_pptoInicial_fechaFinLimite");
        const btnCEPptoInicial = $("#btnCEPptoInicial");
        const lblTitleBtnCEPptoInicial = $("#lblTitleBtnCEPptoInicial");
        const tblAF_CtrlPptalOfCe_PptoInicial = $('#tblAF_CtrlPptalOfCe_PptoInicial');  
        let dtPptoInicial;
        //#endregion

        //#region CONST CC REL PPTO
        const mdlCECCRelPpto = $('#mdlCECCRelPpto');
        const tblCCRelPpto = $('#tblCCRelPpto');
        let dtCCRelPpto;
        //#endregion

        //#region CONST AGREGAR CC AL PPTO
        const mdlAgregarCC = $("#mdlAgregarCC");
        const cboCC = $("#cboCC");
        const btnAgregarCC = $("#btnAgregarCC");
        //#endregion

        let dtPresupuestos;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            agregarListeners();
        })();

        function agregarListeners() {
            // botonAgregar.click(() => { //AGREGAR PPTO INICIAL CON DET
            //     fncLimpiarModal(); 
            //     botonGuardar.data().estatus = ESTATUS.NUEVO;
            //     botonGuardar.data().id = 0;
            //     modalPresupuesto.modal('show');
            // });
            // botonGuardar.click(guardarPresupuesto);

            // cargarPresupuestos(); //CARGA DETALLE PPTO INICIAL CON CC

            //#region INIT DATATABLES
            initTablaPresupuestos();
            initTblPptoInicial();
            initTblCCRelPpto();
            //#endregion

            // selectCentroCosto.fillCombo('FillUsuarioRelCC', null, false, null);

            //#region EVENTOS CREAR/EDITAR PPTO INICIAL
            fncGetPptosIniciales();
            //#endregion

            botonAgregar.on("click", function() {
                fncBorderDefault();
                fncLimpiarModal();
                fncTitleMdlCEPptoInicial(true);
                mdlCEPptoInicial.modal("show");
            });
            
            btnCEPptoInicial.on("click", function () {
                fncCrearEditarPptoInicial();
            });

            btnBuscar.on("click", function() {
                fncGetPptosIniciales();
            });

            btnAgregarCC.on("click", function () {
                fncAgregarCC();
            });

            $('.select2').select2();
            $(".select2").select2({ width: '100%' });
        }

        modalPresupuesto.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function initTablaPresupuestos() {
            dtPresupuestos = tablaPresupuestos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaPresupuestos.on('click', '.btn-editar', function () {
                        let rowData = dtPresupuestos.row($(this).closest('tr')).data();

                        fncLimpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        modalPresupuesto.modal('show');
                    });

                    tablaPresupuestos.on('click', '.btn-eliminar', function () {
                        let rowData = dtPresupuestos.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el presupuesto anual "${rowData.nombrePresupuesto}"?`,
                            () => eliminarPresupuesto(rowData.id))
                    });
                },
                drawCallback: function () {
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                },
                columns: [
                    { data: 'ccDesc', title: 'Centro de Costo' },
                    { data: 'anio', title: 'Año' },
                    { data: 'nombrePresupuesto', title: 'Nombre del Presupuesto' },
                    {
                        render: function (data, type, row, meta) {
                            return `
                            <button title="Editar" class="btn-editar btn btn-xs btn-warning">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            &nbsp;
                            <button title="Eliminar" class="btn-eliminar btn btn-xs btn-danger">
                                <i class="fas fa-trash"></i>
                            </button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarPresupuestos() {
            axios.post('GetPresupuestosAnuales')
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaPresupuestos, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function fncLimpiarModal() {
            selectCentroCosto.val('');
            selectCentroCosto.select2().change();
            inputAnio.val('');
            inputNombrePresupuesto.val('');
            txt_pptoInicial_anio.val("");
            txt_pptoInicial_nombrePpto.val("");
            btnCEPptoInicial.attr("data-id", 0);
            cboCC[0].selectedIndex = 0;
            $('#select2-cboCC-container').css('border', '1px solid #CCC');
            cboCC.trigger("change");
            txt_pptoInicial_fechaInicio.val("");
            txt_pptoInicial_fechaFin.val("");
            txt_pptoInicial_fechaInicioLimite.val("");
            txt_pptoInicial_fechaFinLimite.val("");
        }

        function llenarCamposModal(data) {
            selectCentroCosto.val(data.cc);
            selectCentroCosto.select2().change();
            inputAnio.val(data.anio);
            inputNombrePresupuesto.val(data.nombrePresupuesto);
        }

        function guardarPresupuesto() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevoPresupuesto();
                    break;
                case ESTATUS.EDITAR:
                    editarPresupuesto();
                    break;
            }
        }

        function nuevoPresupuesto() {
            let presupuesto = getInformacionPresupuesto();

            axios.post('GuardarNuevoPresupuestoAnual', presupuesto)
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalPresupuesto.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarPresupuestos();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarPresupuesto() {
            let presupuesto = getInformacionPresupuesto();

            axios.post('EditarPresupuestoAnual', { presupuesto })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        modalPresupuesto.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarPresupuestos();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarPresupuesto(id) {
            let presupuesto = { id };

            axios.post('EliminarPresupuestoAnual', { presupuesto })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarPresupuestos();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function getInformacionPresupuesto() {
            return {
                id: botonGuardar.data().id,
                cc: selectCentroCosto.val(),
                anio: inputAnio.val(),
                nombrePresupuesto: inputNombrePresupuesto.val(),
                estatus: true
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        //#region CRUD PPTO INICIAL
        function initTblPptoInicial() {
            dtPptoInicial = tblAF_CtrlPptalOfCe_PptoInicial.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'anio', title: 'AÑO' },
                    { data: 'nombrePresupuesto', title: 'NOMBRE PPTO' },
                    { 
                        data: 'fechaInicio', title: 'FECHA INICIO',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { 
                        data: 'fechaFin', title: 'FECHA FIN',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { 
                        data: 'fechaInicioLimite', title: 'FECHA INICIO LIMITE',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { 
                        data: 'fechaFinLimite', title: 'FECHA FIN LIMITE',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class="btn btn-xs btn-warning editarRegistro" title="Editar registro."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-xs btn-danger eliminarRegistro" title="Eliminar registro."><i class="fas fa-trash"></i></button>&nbsp;`;
                            let btnCCRelPpto = `<button class="btn btn-xs btn-primary ccRelPpto" title="CC relacionados al Ppto."><i class="fas fa-list"></i></button>&nbsp;`;
                            let btnCC = `<button class="btn btn-xs btn-success cc" title="Administrar CC."><i class="fas fa-plus"></i></button>&nbsp;`;
                            return btnEditar + btnEliminar + btnCCRelPpto + btnCC;
                        },
                    },
                    { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblAF_CtrlPptalOfCe_PptoInicial.on('click','.editarRegistro', function () {
                        let rowData = dtPptoInicial.row($(this).closest('tr')).data();
                        txt_pptoInicial_anio.val(rowData.anio);
                        txt_pptoInicial_nombrePpto.val(rowData.nombrePresupuesto);
                        txt_pptoInicial_fechaInicio.val(moment(rowData.fechaInicio).format("YYYY-MM-DD"));
                        txt_pptoInicial_fechaFin.val(moment(rowData.fechaFin).format("YYYY-MM-DD"));
                        txt_pptoInicial_fechaInicioLimite.val(moment(rowData.fechaInicioLiminite).format("YYYY-MM-DD"));
                        txt_pptoInicial_fechaFinLimite.val(moment(rowData.fechaFinLimite).format("YYYY-MM-DD"));
                        btnCEPptoInicial.attr("data-id", rowData.id);
                        fncTitleMdlCEPptoInicial(false);
                        mdlCEPptoInicial.modal("show");
                    });

                    tblAF_CtrlPptalOfCe_PptoInicial.on('click','.eliminarRegistro', function () {
                        let rowData = dtPptoInicial.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncEliminarPptoInicial(rowData.id));
                    });

                    tblAF_CtrlPptalOfCe_PptoInicial.on("click", ".ccRelPpto", function () {
                        let rowData = dtPptoInicial.row($(this).closest("tr")).data();
                        fncGetCCRelPpto(rowData.id);
                    });
                    
                    tblAF_CtrlPptalOfCe_PptoInicial.on("click", ".cc", function () {
                        let rowData = dtPptoInicial.row($(this).closest("tr")).data();
                        fncLimpiarModal();
                        cboCC.fillCombo("FillCboCCFaltantes", { idPptoInicial: rowData.id }, false);
                        mdlAgregarCC.attr("data-id", rowData.id);
                        mdlAgregarCC.modal("show");
                    });
                },
                columnDefs: [
                    { className: 'dt-center','targets': '_all'},
                    { className: 'dt-body-center', "targets": "_all" },
                    { width: "10%", "targets": 6 }
                ],
            });
        }

        function fncGetPptosIniciales() {
            axios.post("GetPptosIniciales").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtPptoInicial.clear();
                    dtPptoInicial.rows.add(response.data.lstPptosIniciales);
                    dtPptoInicial.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarPptoInicial(idPptoInicial) {
            let obj = new Object();
            obj = {
                idPptoInicial: idPptoInicial
            }
            axios.post("EliminarPptoInicial", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetPptosIniciales();
                    Alert2Exito(response.data.message);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarPptoInicial() {
            let obj = fncObjCEPptoInicial();
            if (obj != "") {
                axios.post("CrearEditarPptoInicial", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetPptosIniciales();
                        Alert2Exito(response.data.message);
                        mdlCEPptoInicial.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCEPptoInicial() {
            fncBorderDefault();
            let strMensajeError = "";
            if (txt_pptoInicial_anio.val() == "") { txt_pptoInicial_anio.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEPptoInicial.attr("data-id"),
                    anio: txt_pptoInicial_anio.val(),
                    nombrePresupuesto: txt_pptoInicial_nombrePpto.val(),
                    fechaInicio: txt_pptoInicial_fechaInicio.val(),
                    fechaFin: txt_pptoInicial_fechaFin.val(),
                    fechaInicioLimite: txt_pptoInicial_fechaInicioLimite.val(),
                    fechaFinLimite: txt_pptoInicial_fechaFinLimite.val()
                };
                return obj;
            }
        }

        function fncBorderDefault() {
            txt_pptoInicial_anio.css("border", "1px solid #CCC");
        }

        function fncTitleMdlCEPptoInicial(esCrear) {
            if (esCrear) {
                lblTitleCEPptoInicial.html(`NUEVO REGISTRO`);
                lblTitleBtnCEPptoInicial.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCEPptoInicial.attr("data-id", 0);
            } else {
                lblTitleCEPptoInicial.html(`ACTUALIZAR REGISTRO`);
                lblTitleBtnCEPptoInicial.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
            }
        }

        function initTblCCRelPpto() {
            dtCCRelPpto = tblCCRelPpto.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'cc', title: 'CC' },
                    { 
                        data: 'descripcionCC', title: 'DESCRIPCIÓN', visible: false,
                        render: function (data, type, row) {
                            return `- ${data}`;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center','targets': '_all'},
                    { className: 'dt-body-center', targets: "_all" },
                    // { className: 'dt-body-left', targets: [1] }
                ],
            });
        }

        function fncGetCCRelPpto(idPptoInicial) {
            let obj = new Object();
            obj = {
                idPptoInicial: idPptoInicial
            }
            axios.post("GetCCRelPpto", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtCCRelPpto.clear();
                    dtCCRelPpto.rows.add(response.data.lstCCRelPpto);
                    dtCCRelPpto.draw();

                    mdlCECCRelPpto.modal("show");
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAgregarCC() {
            if (cboCC.find('option:selected').text() != "--Seleccione--") {
                let obj = new Object();
                obj = {
                    idPptoInicial: mdlAgregarCC.attr("data-id"),
                    idCC: cboCC.val()
                }
                axios.post("AgregarCC", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetPptosIniciales();
                        Alert2Exito(response.data.message);
                        mdlAgregarCC.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                $('#select2-cboCC-container').css('border', '2px solid red');
                Alert2Warning("Es necesario indicar un CC.");
            }
        }
        //#endregion
    }

    $(document).ready(() => CH.PresupuestosAnuales = new PresupuestosAnuales())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();