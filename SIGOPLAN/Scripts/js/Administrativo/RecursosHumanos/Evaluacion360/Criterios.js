(() => {
    $.namespace('CH.Evaluacion360');

    //#region CONST FILTROS
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroNuevo = $('#btnFiltroNuevo');
    //#endregion

    //#region CONST PLANTILLAS
    const tblRH_Eval360_CatPlantillas = $('#tblRH_Eval360_CatPlantillas');
    let dtPlantillas;
    const modalPlantilla = $('#modalPlantilla');
    const inputPlantilla = $('#inputPlantilla');
    const inputDescripcionPlantilla = $('#inputDescripcionPlantilla');
    const btnGuardarPlantilla = $('#btnGuardarPlantilla');
    //#endregion

    //#region CONST CRITERIOS
    const mdlCECriterio = $('#mdlCECriterio');
    const tblRH_Eval360_CatCriterios = $('#tblRH_Eval360_CatCriterios');
    let dtCriterios;
    const modalCriterio = $('#modalCriterio');
    const Plantilla = $('#Plantilla');
    const inputLimiteInferior = $("#inputLimiteInferior");
    const inputLimiteSuperior = $("#inputLimiteSuperior");
    const inputEtiqueta = $("#inputEtiqueta");
    const inputDescripcion = $("#inputDescripcion");
    const inputColor = $("#inputColor");
    const cboCuestionarios = $('#cboCuestionarios');
    const inputIdPlantilla = $('#inputIdPlantilla');
    const btnAgregarCriterio = $('#btnAgregarCriterio');
    const btnGuardarCriterio = $('#btnGuardarCriterio');
    const btnNuevoCriterio = $('#btnNuevoCriterio');
    const btnCancelarCECriterio = $('#btnCancelarCECriterio');
    //#endregion

    //#region CONST MENU
    const menuPersonal = $('#menuPersonal');
    const menuConductas = $("#menuConductas");
    const menuCuestionarios = $("#menuCuestionarios");
    const menuPeriodos = $("#menuPeriodos");
    const menuCriterios = $("#menuCriterios");
    const menuRelaciones = $('#menuRelaciones');
    const menuEvaluaciones = $('#menuEvaluaciones')
    const menuReporte360 = $('#menuReporte360')
    const menuAvances = $('#menuAvances')
    //#endregion

    Evaluacion360 = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLE
            fncGetNivelAcceso()
            initTablaCriterios();
            initTablaPlantillas();
            fncGetPlantillas();
            //#endregion

            //#region FUNCIONES MENU
            menuPersonal.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Personal';
            })

            menuConductas.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Conductas';
            })

            menuCuestionarios.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Cuestionarios';
            })

            menuPeriodos.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Periodos';
            })

            menuCriterios.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Criterios';
            })

            menuRelaciones.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Relaciones';
            })

            menuEvaluaciones.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Evaluaciones';
            })

            menuReporte360.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Reporte360';
            })

            menuAvances.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Avances';
            })
            //#endregion

            //#region FUNCIONES FILTROS
            $(".select2").select2();

            btnFiltroBuscar.click(function () {
                fncGetPlantillas();
            })

            btnFiltroNuevo.click(() => {
                fncLimpiarModal();
                btnGuardarPlantilla.data().id = 0;
                btnGuardarPlantilla.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                $("#modalPlantilla .modal-title").text("NUEVO REGISTRO");
                modalPlantilla.modal('show');
            })
            //#endregion

            //#region FUNCIONES PLANTILLA
            btnGuardarPlantilla.click(function () {
                fncCEPlantilla();
            })
            //#endregion

            //#region FUNCIONES CRITERIOS
            btnNuevoCriterio.click(function () {
                btnGuardarCriterio.data().id = 0;
                btnGuardarCriterio.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                cboCuestionarios.fillCombo('/Administrativo/Evaluacion360/FillCboCuestionarios', { est: true }, false, null);
            })

            btnCancelarCECriterio.click(function () {
                btnNuevoCriterio.trigger("click");
            })

            btnGuardarCriterio.click(function () {
                fncCECriterio();
            })
            //#endregion

            //#region FUNCIONES GENERALES
            // ATTR AUTO-COMPLETE FORMULARIO
            $("input[type='text']").attr("autocomplete", "off");
            //#endregion
        }

        //#region FUNCIONES PLANTILLAS
        function initTablaPlantillas() {
            dtPlantillas = tblRH_Eval360_CatPlantillas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'plantilla', title: 'Plantilla' },
                    { data: 'descripcion', title: 'Descripción' },
                    {
                        title: 'Criterio',
                        render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-default btn-ModalCriterio" title="Listado de criterios."><i class="fas fa-plus"></i></button>`;
                        }
                    },
                    {
                        title: 'Opciones',
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning btn-editarPlantilla' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger btn-eliminarPlantilla' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEditar + btnEliminar;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblRH_Eval360_CatPlantillas.on('click', '.btn-ModalCriterio', function () {
                        let rowData = dtPlantillas.row($(this).closest('tr')).data();
                        Plantilla.val(rowData.plantilla);
                        fncGetCriterios(rowData.id);
                    });

                    tblRH_Eval360_CatPlantillas.on('click', '.btn-editarPlantilla', function () {
                        let rowData = dtPlantillas.row($(this).closest('tr')).data();
                        fncGetDatosActualizarPlantilla(rowData.id);
                    });

                    tblRH_Eval360_CatPlantillas.on('click', '.btn-eliminarPlantilla', function () {
                        let rowData = dtPlantillas.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarPlantilla(rowData.id));
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '5%', targets: [2, 3] }
                ]
            });
        }

        function fncGetDatosActualizarPlantilla(id) {
            if (id > 0) {
                let obj = new Object();
                obj.id = id;
                axios.post('GetDatosActualizarPlantilla', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        inputPlantilla.val(response.data.objPlantilla.plantilla);
                        inputDescripcionPlantilla.val(response.data.objPlantilla.descripcion);
                        btnGuardarPlantilla.data().id = id;
                        $("#modalPlantilla .modal-title").text("ACTUALIZAR REGISTRO");
                        btnGuardarPlantilla.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                        modalPlantilla.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener la información de la plantilla.");
            }
        }

        function fncGetPlantillas() {
            axios.post('GetPlantillas').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtPlantillas.clear();
                    dtPlantillas.rows.add(response.data.lstPlantillas);
                    dtPlantillas.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEPlantilla() {
            let obj = fncCEPersonalOBJ();
            if (obj != "") {
                axios.post("CEPlantilla", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetPlantillas();
                        Alert2Exito(message);
                        modalPlantilla.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEPersonalOBJ() {
            let mensajeError = "";
            if (inputPlantilla.val() == "") { inputPlantilla.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputPlantilla") }
            if (inputDescripcionPlantilla.val() == "") { inputDescripcionPlantilla.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputDescripcionPlantilla") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return "";
            } else {
                let obj = new Object();
                obj.id = btnGuardarPlantilla.data().id;
                obj.plantilla = inputPlantilla.val();
                obj.descripcion = inputDescripcionPlantilla.val();
                return obj;
            }
        }

        function fncEliminarPlantilla(id) {
            if (id > 0) {
                let objPlantilla = new Object();
                objPlantilla.idPlantilla = id;
                axios.post("EliminarPlantilla", objPlantilla).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetPlantillas();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al eliminar el registro.");
            }
        }
        //#endregion

        //#region FUNCIONES CRITERIOS
        function initTablaCriterios() {
            dtCriterios = tblRH_Eval360_CatCriterios.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'limInferior', title: 'Límite inferior' },
                    { data: 'limSuperior', title: 'Límite superior' },
                    { data: 'etiqueta', title: 'Etiqueta' },
                    { data: 'nombreCuestionario', title: 'Cuestionario' },
                    { data: 'descripcionEtiqueta', title: 'Descripción' },
                    {
                        data: 'color', title: 'Color',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let color = cellData;
                            $(td).css('background-color', color);
                            $(td).css('color', color);
                        }
                    },
                    {
                        title: 'Opciones',
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning btn-editar' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger btn-eliminar' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEditar + btnEliminar;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblRH_Eval360_CatCriterios.on('click', '.btn-editar', function () {
                        let rowData = dtCriterios.row($(this).closest('tr')).data();
                        fncGetDatosActualizarCriterio(rowData.id);
                    });

                    tblRH_Eval360_CatCriterios.on('click', '.btn-eliminar', function () {
                        let rowData = dtCriterios.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCriterio(rowData.id));
                    });

                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: '5%', targets: [0, 1, 5] }
                ]
            });
        }

        function fncGetDatosActualizarCriterio(id) {
            if (id > 0) {
                let obj = new Object();
                obj.id = id;
                axios.post('GetDatosActualizarCriterio', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnNuevoCriterio.trigger("click")
                        inputLimiteInferior.val(response.data.objCriterio.limInferior);
                        inputLimiteSuperior.val(response.data.objCriterio.limSuperior);
                        inputEtiqueta.val(response.data.objCriterio.etiqueta);
                        inputDescripcion.val(response.data.objCriterio.descripcionEtiqueta);
                        inputColor.val(response.data.objCriterio.color);
                        cboCuestionarios.val(response.data.objCriterio.idCuestionario);
                        cboCuestionarios.trigger("change");
                        btnGuardarCriterio.data().id = id;
                        btnGuardarCriterio.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener la información del criterio.");
            }
        }

        function fncGetCriterios(idPlantilla) {
            if (idPlantilla > 0) {
                let obj = new Object();
                obj.idPlantilla = idPlantilla;
                axios.post('GetCriterios', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        dtCriterios.clear();
                        dtCriterios.rows.add(response.data.lstCriterios);
                        dtCriterios.draw();
                        btnGuardarCriterio.data().idPlantilla = idPlantilla;
                        modalCriterio.modal("show");
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener los criterios.")
            }
        }

        function fncCECriterio() {
            let obj = fncCECriterioOBJ();
            if (obj != "") {
                axios.post("CECriterio", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncLimpiarModal();
                        btnCancelarCECriterio.trigger("click");
                        fncGetCriterios(btnGuardarCriterio.data().idPlantilla);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCECriterioOBJ() {
            let mensajeError = "";
            if (inputLimiteInferior.val() == "") { inputLimiteInferior.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputLimiteInferior") }
            if (inputLimiteSuperior.val() == "") { inputLimiteSuperior.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputLimiteSuperior") }
            if (inputEtiqueta.val() == "") { inputEtiqueta.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputEtiqueta") }
            if (inputDescripcion.val() == "") { inputDescripcion.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputDescripcion") }
            if (cboCuestionarios.val() <= 0) { $("#select2-cboCuestionarios-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("select2-cboCuestionarios-container") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return "";
            } else {
                let obj = new Object();
                obj.id = btnGuardarCriterio.data().id;
                obj.limInferior = inputLimiteInferior.val();
                obj.limSuperior = inputLimiteSuperior.val();
                obj.etiqueta = inputEtiqueta.val();
                obj.descripcionEtiqueta = inputDescripcion.val();
                obj.color = inputColor.val();
                obj.idCuestionario = cboCuestionarios.val();
                obj.idPlantilla = btnGuardarCriterio.data().idPlantilla;
                return obj;
            }
        }

        function fncEliminarCriterio(id) {
            if (id > 0) {
                let objCriterio = new Object();
                objCriterio.idCriterio = id;
                axios.post("EliminarCriterio", objCriterio).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetCriterios(btnGuardarCriterio.data().idPlantilla);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al eliminar el registro.");
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncLimpiarModal() {
            $("input[type='text']").val("");
            inputDescripcionPlantilla.val("");
            inputDescripcion.val("");
            cboCuestionarios[0].selectedIndex = 0;
            cboCuestionarios.trigger("change");
        }

        function fncDefaultCtrls(obj) {
            $(`#${obj}`).css("border", "1px solid #CCC");
        }

        function fncGetNivelAcceso() {
            axios.post('GetNivelAcceso').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (response.data.nivelAcceso == 1) {
                        menuPersonal.css("display", "block")
                        menuConductas.css("display", "block")
                        menuCuestionarios.css("display", "block")
                        menuPeriodos.css("display", "block")
                        menuCriterios.css("display", "block")
                        menuRelaciones.css("display", "block")
                        menuEvaluaciones.css("display", "block")
                        menuReporte360.css("display", "block")
                        menuAvances.css("display", "block")
                    } else if (response.data.nivelAcceso == 0) {
                        menuEvaluaciones.trigger("click")
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }
    $(document).ready(() => {
        CH.Evaluacion360 = new Evaluacion360();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();