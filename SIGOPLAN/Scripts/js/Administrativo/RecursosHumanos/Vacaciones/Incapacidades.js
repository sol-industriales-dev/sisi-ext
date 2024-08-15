(() => {
    $.namespace('CH.Incapacidades');

    //#region CONST FILT
    const cboFiltroCC = $('#cboFiltroCC');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    // const btnFiltroExportar = $('#btnFiltroExportar');
    const txtFiltroFechaInicio = $('#txtFiltroFechaInicio');
    const txtFiltroFechaTerminacion = $('#txtFiltroFechaTerminacion');
    const cboFiltroEstatus = $('#cboFiltroEstatus');
    const btnFiltroNuevo = $('#btnFiltroNuevo');
    const tblIncapacidades = $('#tblIncapacidades');
    const txtFiltroClaveEmpleado = $('#txtFiltroClaveEmpleado');
    const txtFiltroNombreEmpleado = $('#txtFiltroNombreEmpleado');
    let dtIncapacidades;
    //#endregion

    //#region CONST CE INCAP
    const mdlCEIncap = $('#mdlCEIncap');
    const txtCEIncapFechaCaptura = $('#txtCEIncapFechaCaptura');
    const txtCEIncapEstatus = $('#txtCEIncapEstatus');
    const txtCEIncapClaveEmpleado = $('#txtCEIncapClaveEmpleado');
    const txtCEIncapNombreEmpleado = $('#txtCEIncapNombreEmpleado');
    const txtCEIncapCCEmpleado = $('#txtCEIncapCCEmpleado');
    const txtCEIncapPuestoEmpleado = $('#txtCEIncapPuestoEmpleado');
    const txtCEIncapNSSEmpleado = $('#txtCEIncapNSSEmpleado');
    const cboCEIncapTipo = $('#cboCEIncapTipo');
    const txtCEIncapFechaInicio = $('#txtCEIncapFechaInicio');
    const txtCEIncapTotalDias = $('#txtCEIncapTotalDias');
    const cboCEIncapTipo2 = $('#cboCEIncapTipo2');
    const txtCEIncapFechaTerminacion = $('#txtCEIncapFechaTerminacion');
    const txtCEIncapCodigo = $('#txtCEIncapCodigo');
    const txtCEIncapMotivoIncap = $('#txtCEIncapMotivoIncap');

    const inputCEIncapCargarArchivo = $('#inputCEIncapCargarArchivo');
    const spanCEIncapBtnArchivoTitle = $('#spanCEIncapBtnArchivoTitle');
    const spanCEIncapBtnNotificarTitle = $('#spanCEIncapBtnNotificarTitle');
    const divCEIncapHistorial = $('#divCEIncapHistorial');
    const spanCEIncapTitulo = $('#spanCEIncapTitulo');
    const btnCEIncapHistorial = $('#btnCEIncapHistorial');
    const btnCEIncapCargarArchivo = $('#btnCEIncapCargarArchivo');
    const btnCEIncapNotificar = $('#btnCEIncapNotificar');
    const tblHistorialIncaps = $('#tblHistorialIncaps');
    const btnCEIncap = $('#btnCEIncap');

    let dtHistorialIncaps;
    //#endregion

    //#region MDL CAROUSEL
    const mdlCarousel = $('#mdlCarousel');
    const carouselNotis = $('#carouselNotis');
    const carouselNotisInner = $('#carouselNotisInner');
    var btnNotiCapturar;
    //#endregion

    let tipoIncaps = [];
    tipoIncaps[0] = "Probable riesgo de trabajo";
    tipoIncaps[1] = "Riesgo de trabajo";
    tipoIncaps[2] = "Enfermedad general";
    tipoIncaps[3] = "Maternidad";
    tipoIncaps[4] = "ST7";
    tipoIncaps[5] = "ST4";
    tipoIncaps[6] = "ST2";
    tipoIncaps[7] = "ST2 Calificada";

    let tipoIncaps2 = [];
    tipoIncaps2[0] = "Probable riesgo de trabajo";
    tipoIncaps2[1] = "Riesgo de trabajo";

    Incapacidades = function () {
        (function init() {
            fncListeners();
            initTblIncapacidades();
            initTblHistorialIncaps();
            fncGetIncaps();
            fncGetIncapcidadesVencer();

            console.log(carouselNotisInner.html());

        })();

        function fncListeners() {
            //#region FILL COMBO
            // cboFiltroCC.fillCombo("FillComboCC", {}, false, null);
            cboFiltroCC.fillCombo("FillComboCC", {}, false, 'Todos');
            convertToMultiselect('#cboFiltroCC');
            //#endregion

            //#region LISTENERS
            btnFiltroBuscar.on("click", function () {
                fncGetIncaps();
            });

            btnFiltroNuevo.on("click", function () {
                btnCEIncap.text("Crear");
                spanCEIncapTitulo.text("CAPTURA");
                mdlCEIncap.modal("show");
                btnCEIncap.data("id", 0);
                btnCEIncap.data("clave", 0);

                fncLimpiarModalCEIncap();
                fncDefaultBorders();

                txtCEIncapFechaCaptura.val(moment().format("YYYY-MM-DD"));

                txtCEIncapEstatus.bootstrapToggle('on');

                btnCEIncapCargarArchivo.css("display", "none");
                btnCEIncapNotificar.attr("disabled", true);

                //COLOR DEFAULT PARA LOS BOTONES
                if (btnCEIncapCargarArchivo.hasClass("btn-success")) {

                    btnCEIncapCargarArchivo.removeClass("btn-success");
                    btnCEIncapCargarArchivo.addClass("btn-primary");
                    spanCEIncapBtnArchivoTitle.text("CARGAR ");
                }

                if (btnCEIncapNotificar.hasClass("btn-success")) {

                    btnCEIncapNotificar.removeClass("btn-success");
                    btnCEIncapNotificar.addClass("btn-warning");

                }
            });

            btnCEIncap.on("click", function () {
                fncCrearEditarIncap();
            });

            btnCEIncapHistorial.on("click", function () {
                if (!divCEIncapHistorial.hasClass("in") && btnCEIncap.data("clave") != 0) {
                    fncGetHistorialIncaps();
                }
            });

            btnCEIncapCargarArchivo.on("click", function () {
                // fncCargarEvidencia();
            });

            btnCEIncapNotificar.on("click", function () {
                fncNotificarIncap();
            });

            inputCEIncapCargarArchivo.on("change", function () {
                console.log(inputCEIncapCargarArchivo.val());
                console.log(inputCEIncapCargarArchivo[0].files.length);
                if ($(this).val() != "") {
                    fncCargarEvidencia();
                }
            });

            //BTN ARCHIVO
            $(document).on('change', ':file', function () {
                var input = $(this),
                    numFiles = input.get(0).files ? input.get(0).files.length : 1,
                    label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                input.trigger('fileselect', [numFiles, label]);

            });

            $(document).ready(function () {
                $(':file').on('fileselect', function (event, numFiles, label) {

                    var input = $(this).parents('.input-group').find(':text'),
                        log = numFiles > 1 ? numFiles + ' files selected' : label;

                    // if (input.length) {
                    //     input.val(log);
                    // } else {
                    //     if (log) alert(log);
                    // }
                });
            });


            //#endregion

            //#region FNC GRALES
            txtCEIncapNombreEmpleado.getAutocomplete(funGetEmpleado, null, '/Administrativo/FormatoCambio/getCatEmpleadosReclutamientos');

            txtCEIncapFechaInicio.on("change", function () {
                fncGetFechaTerminacion();
            });

            txtCEIncapTotalDias.on("change", function () {
                fncGetFechaTerminacion();
            });
            //#endregion
        }

        //#region CRUD INCAP
        function initTblIncapacidades() {
            dtIncapacidades = tblIncapacidades.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                    }
                ],
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'nombreUsuarioCapturo', title: 'CAPTURO' },
                    { data: 'ccDesc', title: 'CC' },
                    { data: 'nombre_corto', title: 'REG. PATRONAL' },
                    { data: 'clave_empleado', title: 'NUM. EMPLEADO' },
                    { data: 'nombreCompleto', title: 'NOMBRE' },
                    { data: 'nss', title: 'NSS' },
                    { data: 'codigoIncap', title: 'CODIGO' },
                    { data: 'descIncap', title: 'Remo de Seguro' },
                    { data: 'descIncap2', title: 'Tipo Incapacidad' },
                    {
                        data: 'fechaInicio', title: 'FECHA INICIO',
                        // render: function (data, type, row) {
                        //     return moment(data).format("DD/MM/YYYY");
                        // }
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'fechaTerminacion', title: 'FECHA TERMINACION',
                        // render: function (data, type, row) {
                        //     return moment(data).format("DD/MM/YYYY");
                        // }
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'descEstatus', title: 'ESTATUS'
                    },
                    {
                        title: 'EVIDENCIA',
                        render: (data, type, row, meta) => {
                            if (row.evidenciaIncap != null) {
                                return `<button title="Imprimir evidencia" class="btn btn-primary btn-xs imprimirEvidencia"><i class="fas fa-print"></i></button>`;
                            } else {
                                return `<button title="Imprimir evidencia" class="btn btn-primary btn-xs imprimirEvidencia" disabled><i class="fas fa-print"></i></button>`;
                            }
                        }
                    },
                    {
                        title: 'HISTORIAL',
                        render: (data, type, row, meta) => {
                            return `<button title="" class="btn btn-primary btn-xs imprimirReporte"><i class="fas fa-print"></i></button>`;
                        }
                    },
                    {
                        render: (data, type, row, meta) => {
                            return `
                                <button title='Actualizar Incapacidad.' class="btn btn-warning actualizarIncap btn-xs"><i class="far fa-edit"></i></button>&nbsp;
                                <button title='Actualizar Incapacidad.' class="btn btn-danger eliminarIncap btn-xs"><i class="far fa-trash-alt"></i></button>&nbsp;
                            `;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblIncapacidades.on('click', '.actualizarIncap', function () {
                        let rowData = dtIncapacidades.row($(this).closest('tr')).data();

                        fncLimpiarModalCEIncap();
                        fncDefaultBorders();

                        btnCEIncap.data("id", rowData.id);
                        btnCEIncap.data("clave", rowData.clave_empleado);
                        txtCEIncapCCEmpleado.data("cc", rowData.cc);

                        btnCEIncap.text("Actualizar");
                        spanCEIncapTitulo.text("ACTUALIZACIÓN");

                        txtCEIncapFechaCaptura.val(moment(rowData.fechaCreacion).format("YYYY-MM-DD"));
                        // txtCEIncapEstatus.val(rowData.estatus);
                        if (rowData.estatus == 1) {
                            txtCEIncapEstatus.bootstrapToggle('on');
                        } else {
                            txtCEIncapEstatus.bootstrapToggle('off');

                        }
                        txtCEIncapClaveEmpleado.val(rowData.clave_empleado);
                        txtCEIncapNombreEmpleado.val(rowData.nombreCompleto);
                        txtCEIncapCCEmpleado.val(rowData.ccDescripcion);
                        txtCEIncapPuestoEmpleado.val(rowData.puestoDesc);//PENDING
                        txtCEIncapNSSEmpleado.val(rowData.nss);
                        cboCEIncapTipo.val(rowData.tipoIncap);
                        cboCEIncapTipo.trigger("change");
                        cboCEIncapTipo2.val(rowData.tipoIncap2);
                        cboCEIncapTipo2.trigger("change");
                        txtCEIncapFechaInicio.val(moment(rowData.fechaInicio).format("YYYY-MM-DD"));
                        txtCEIncapTotalDias.val(rowData.totalDias);
                        txtCEIncapFechaTerminacion.val(moment(rowData.fechaTerminacion).format("YYYY-MM-DD"));
                        txtCEIncapCodigo.val(rowData.codigoIncap);
                        txtCEIncapMotivoIncap.text(rowData.motivoIncap);

                        // btnCEIncapCargarArchivo.attr("disabled", false);
                        btnCEIncapCargarArchivo.css("display", "block");

                        btnCEIncapNotificar.attr("disabled", false);

                        //CHECAR SI LA INCAP TIENE EVIDENCIA CARGADA
                        if (rowData.evidenciaIncap != null) {
                            if (btnCEIncapCargarArchivo.hasClass("btn-primary")) {

                                btnCEIncapCargarArchivo.removeClass("btn-primary");
                                btnCEIncapCargarArchivo.addClass("btn-success");
                                spanCEIncapBtnArchivoTitle.text("ACTUALIZAR ");
                            }


                        } else {
                            if (btnCEIncapCargarArchivo.hasClass("btn-success")) {

                                btnCEIncapCargarArchivo.removeClass("btn-success");
                                btnCEIncapCargarArchivo.addClass("btn-primary");
                                spanCEIncapBtnArchivoTitle.text("CARGAR ");
                            }
                        }

                        //CHECAR SI LA INCAP ESTA NOTIFICADA
                        if (rowData.esNotificada) {
                            if (btnCEIncapNotificar.hasClass("btn-warning")) {

                                btnCEIncapNotificar.removeClass("btn-warning");
                                btnCEIncapNotificar.addClass("btn-success");

                            }

                        } else {
                            if (btnCEIncapNotificar.hasClass("btn-success")) {

                                btnCEIncapNotificar.removeClass("btn-success");
                                btnCEIncapNotificar.addClass("btn-warning");

                            }
                        }

                        mdlCEIncap.modal("show");
                    });
                    tblIncapacidades.on('click', '.eliminarIncap', function () {
                        let rowData = dtIncapacidades.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarIncap(rowData.id));
                    });
                    tblIncapacidades.on('click', '.imprimirEvidencia', function () {
                        let rowData = dtIncapacidades.row($(this).closest('tr')).data();
                        // Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarIncap(rowData.id));
                        location.href = `/Administrativo/Vacaciones/DescargarArchivo?archivoCargado_id=` + rowData.evidenciaIncap;
                    });
                    tblIncapacidades.on('click', '.imprimirReporte', function () {
                        let rowData = dtIncapacidades.row($(this).closest('tr')).data();
                        // Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarIncap(rowData.id));
                        fncImprimirReporte(rowData.clave_empleado);
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { "width": "20%", "targets": [0, 1, 4] },
                    { "width": "5%", "targets": 14 },
                ],
            });
        }

        function fncGetIncaps() {
            let obj = fncGetObjFilt();

            if (obj != "") {
                axios.post("GetIncapacidades", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //CODE...
                        dtIncapacidades.clear();
                        dtIncapacidades.rows.add(items);
                        dtIncapacidades.draw();
                    }
                }).catch(error => Alert2Error(error.message));
            }

        }

        function fncGetObjFilt() {
            let strMensajeError = "";

            // if (txtCEDatosEmpleadoNombre.val() == "") { txtCEDatosEmpleadoNombre.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtFiltroFechaInicio.val() == "" && txtFiltroFechaTerminacion.val() != "") {
                strMensajeError = "Ingrese una fecha de inicio de liberacion";
            }

            if (txtFiltroFechaTerminacion.val() == "" && txtFiltroFechaInicio.val() != "") {
                strMensajeError = "Ingrese una fecha de fin de liberacion";
            }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    // cc: cboFiltroCC.val(),
                    estatus: cboFiltroEstatus.val(),
                    ccs: getValoresMultiples('#cboFiltroCC'),
                    fechaInicio: txtFiltroFechaInicio.val(),
                    fechaTerminacion: txtFiltroFechaTerminacion.val(),
                    claveEmpleado: txtFiltroClaveEmpleado.val(),
                    nombreEmpleado: txtFiltroNombreEmpleado.val(),
                }
                return obj;
            }

        }

        function fncCrearEditarIncap() {
            let obj = fncGetObjCEIncap();

            if (obj != "") {
                axios.post("CrearEditarIncapacidades", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //CODE...
                        fncGetIncaps();
                        mdlCEIncap.modal("hide");

                        if (obj.id > 0) {
                            Alert2Exito("Registro actualizado con exito");

                        } else {
                            Alert2Exito("Registro creado con exito");

                        }
                    } else {
                        Alert2Warning(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }

        }

        function fncGetObjCEIncap() {
            let strMensajeError = "";

            if (txtCEIncapNombreEmpleado.val() == "") { txtCEIncapNombreEmpleado.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboCEIncapTipo.val() == "") { cboCEIncapTipo.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCEIncapFechaInicio.val() == "") { txtCEIncapFechaInicio.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCEIncapTotalDias.val() == "") { txtCEIncapTotalDias.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboCEIncapTipo2.val() == "") { cboCEIncapTipo2.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCEIncapFechaTerminacion.val() == "") { txtCEIncapFechaTerminacion.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCEIncapCodigo.val() == "") { txtCEIncapCodigo.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCEIncapMotivoIncap.val() == "") { txtCEIncapMotivoIncap.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEIncap.data("id"),
                    estatus: txtCEIncapEstatus.prop("checked") ? 1 : 2,
                    cc: txtCEIncapCCEmpleado.data("cc"),
                    clave_empleado: txtCEIncapClaveEmpleado.val(),
                    codigoIncap: txtCEIncapCodigo.val(),
                    tipoIncap: cboCEIncapTipo.val(),
                    tipoIncap2: cboCEIncapTipo2.val(),
                    totalDias: txtCEIncapTotalDias.val(),
                    fechaInicio: txtCEIncapFechaInicio.val(),
                    fechaTerminacion: txtCEIncapFechaTerminacion.val(),
                    motivoIncap: txtCEIncapMotivoIncap.val(),

                }
                return obj;
            }
        }

        function fncEliminarIncap(id_incap) {
            axios.post("DeleteIncapacidades", { id_incap }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Registro eliminado con exito");
                    fncGetIncaps();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblHistorialIncaps() {
            dtHistorialIncaps = tblHistorialIncaps.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    // {
                    //     data: 'tipoIncap', title: 'TIPO',
                    //     render: function (data, type, row) {
                    //         return tipoIncaps[data];
                    //     }
                    // },
                    { data: 'descIncap', title: 'descIncap' },
                    { data: 'codigoIncap', title: 'CODIGO' },
                    { data: 'totalDias', title: 'DÍAS' },
                    {
                        data: 'fechaInicio', title: 'FECHA INICIO',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    {
                        data: 'fechaTerminacion', title: 'FECHA TERMINACION',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'ccDesc', title: 'CC' },
                    { data: 'nombre_corto', title: 'REG. PATRONAL' },
                    {
                        render: (data, type, row, meta) => {
                            if (row.evidenciaIncap != null) {
                                return `<button title="Imprimir aviso de baja" class="btn btn-primary btn-xs imprimirEvidencia"><i class="fas fa-print"></i></button>`;
                            } else {
                                return `<button title="Imprimir aviso de baja" class="btn btn-primary btn-xs imprimirEvidencia" disabled><i class="fas fa-print"></i></button>`;
                            }
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblHistorialIncaps.on('click', '.classBtn', function () {
                        let rowData = dtHistorialIncaps.row($(this).closest('tr')).data();
                    });
                    tblHistorialIncaps.on('click', '.classBtn', function () {
                        let rowData = dtHistorialIncaps.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                    tblHistorialIncaps.on('click', '.imprimirEvidencia', function () {
                        let rowData = dtHistorialIncaps.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                        location.href = `/Administrativo/Vacaciones/DescargarArchivo?archivoCargado_id=` + rowData.evidenciaIncap;
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetHistorialIncaps() {
            axios.post("GetHistorialIncapacidades", { clave_empleado: btnCEIncap.data("clave") }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtHistorialIncaps.clear();
                    dtHistorialIncaps.rows.add(items);
                    dtHistorialIncaps.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region FUNC GRALES

        function fncLimpiarModalCEIncap() {

            txtCEIncapFechaCaptura.val("");
            txtCEIncapEstatus.val("");
            txtCEIncapClaveEmpleado.val("");
            txtCEIncapNombreEmpleado.val("");
            txtCEIncapCCEmpleado.val("");
            txtCEIncapPuestoEmpleado.val("");
            txtCEIncapNSSEmpleado.val("");
            cboCEIncapTipo.val("");
            cboCEIncapTipo.trigger("change");
            cboCEIncapTipo2.val("");
            cboCEIncapTipo2.trigger("change");
            txtCEIncapFechaInicio.val("");
            txtCEIncapTotalDias.val("");
            txtCEIncapFechaTerminacion.val("");
            txtCEIncapCodigo.val("");
            txtCEIncapMotivoIncap.text("");

            dtHistorialIncaps.clear();
            dtHistorialIncaps.draw();
        }

        function fncDefaultBorders() {

            // $("#select2-cboCandidatosAprobados-container").css('border', '1px solid #CCC');
            // txtCEDatosEmpleadoNombre.css('border', '1px solid #CCC');

            txtCEIncapNombreEmpleado.css('border', '1px solid #CCC');
            $("#select2-cboCEIncapTipo-container").css('border', '1px solid #CCC');
            txtCEIncapFechaInicio.css('border', '1px solid #CCC');
            txtCEIncapTotalDias.css('border', '1px solid #CCC');
            $("#select2-cboCEIncapTipo2-container").css('border', '1px solid #CCC');
            txtCEIncapFechaTerminacion.css('border', '1px solid #CCC');
            txtCEIncapCodigo.css('border', '1px solid #CCC');
            txtCEIncapMotivoIncap.css('border', '1px solid #CCC');
        }

        function funGetEmpleado(event, ui) {
            txtCEIncapClaveEmpleado.val(ui.item.id);
            txtCEIncapNombreEmpleado.val(ui.item.value);


            if (txtCEIncapNombreEmpleado.val() != "") {
                fncGetDatosEmpleado();
            }
        }

        function fncGetDatosEmpleado() {
            axios.post("GetDatosPersona", { claveEmpleado: txtCEIncapClaveEmpleado.val(), nombre: txtCEIncapNombreEmpleado.val() }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    if (txtCEIncapNombreEmpleado.val() == "") {
                        txtCEIncapNombreEmpleado.val(response.data.objDatosPersona.nombreCompleto);
                    }

                    txtCEIncapCCEmpleado.val(response.data.objDatosPersona.cc);
                    txtCEIncapCCEmpleado.data("cc", response.data.objDatosPersona.numCC);
                    txtCEIncapPuestoEmpleado.val(response.data.objDatosPersona.nombrePuesto);//PENDING
                    txtCEIncapNSSEmpleado.val(response.data.objDatosPersona.nss);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetFechaTerminacion() {
            if (txtCEIncapFechaInicio.val() != "" && txtCEIncapTotalDias.val() != "") {
                txtCEIncapFechaTerminacion.val(moment(txtCEIncapFechaInicio.val()).add((txtCEIncapTotalDias.val() == 0 ? 1 : txtCEIncapTotalDias.val()) - 1, "days").format("YYYY-MM-DD"));
            }
        }

        function fncCargarEvidencia() {
            const data = new FormData();

            data.append('id_incap', btnCEIncap.data("id"));
            data.append('tipoArchivo', 1); // TIPO INCAPACIDAD
            data.append('archivo', inputCEIncapCargarArchivo[0].files[0]);

            if (inputCEIncapCargarArchivo[0].files.length > 0) {
                axios.post("GuardarArchivo", data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito("Evidencia guardad con exito");
                        fncGetIncaps();

                        if (btnCEIncapCargarArchivo.hasClass("btn-primary")) {

                            btnCEIncapCargarArchivo.removeClass("btn-primary");
                            btnCEIncapCargarArchivo.addClass("btn-success");
                            spanCEIncapBtnArchivoTitle.text("ACTUALIZAR ");
                        }
                    }
                }).catch(error => Alert2Error(error.message));
            }

        }

        function fncNotificarIncap() {
            axios.post("NotificarIncapacidades", { id_incapacidad: btnCEIncap.data("id") }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Incapacidad notificada con exito");
                    fncGetIncaps();

                    if (btnCEIncapNotificar.hasClass("btn-warning")) {

                        btnCEIncapNotificar.removeClass("btn-warning");
                        btnCEIncapNotificar.addClass("btn-success");

                    }
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncImprimirReporte(clave_empleado) {
            // console.log('imprimir')
            var path = `/Reportes/Vista.aspx?idReporte=273&claveEmpleado=${clave_empleado}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function fncGetIncapcidadesVencer() {
            axios.post("GetIncapacidadesVencer").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    console.log(items);
                    console.log(response.data.lstVencidos);
                    console.log(response.data.esOfi);

                    let first = false;

                    items.forEach(e => {
                        carouselNotisInner.html(carouselNotisInner.html() +
                            `
                            <div class="item ${first ? "" : "active"}" style="height: 300px;">
                                <div class="row" style="text-align: center;">
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-8">
                                        <h3 id="carouselNombreEmp${e.claveEmpleado}" > [${e.clave_empleado}] ${e.nombreCompleto}</h3>
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                </div>
                                <div class="row" style="text-align: center;">
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-4">
                                        <h3 id="carouselClaveEmp">Fecha Inicio:</h3>
                                        <h3 id="carouselClaveEmp">${moment(e.fechaInicio).format("DD/MM/YYYY")}</h3>
                                    </div>
                                    <div class="col-sm-4">
                                        <h3 id="carouselNombreEmp">Fecha Terminacion:</h3>
                                        <h3 id="carouselNombreEmp">${moment(e.fechaTerminacion).format("DD/MM/YYYY")}</h3>
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                </div>
                            ${e.esVencida ?
                                `<div class="carousel-caption">
                                    <button id="btnNotiCapturar" class="btn btn-primary btnNotiCapturar" data-clave="${e.clave_empleado}" >Capturar</button>
                                </div>` :
                                `
                                <div class="carousel-caption">
                                    <h3 style="color: black;">${e.diasVencer} dias para vencer</h3>
                                </div>
                                `
                            }
                                
                            </div>
                        `
                        );
                        first = true;
                    });

                    $(".btnNotiCapturar").on("click", function () {
                        // console.log($(this).data());
                        mdlCarousel.modal("hide");
                        mdlCEIncap.modal("show");

                        spanCEIncapTitulo.text("CAPTURA");

                        txtCEIncapClaveEmpleado.val($(this).data("clave"));
                        fncGetDatosEmpleado();
                    });

                    if (items.length > 0) {
                        mdlCarousel.modal("show");
                        carouselNotis.carousel({
                            interval: false,
                        });
                    }


                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Incapacidades = new Incapacidades();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();