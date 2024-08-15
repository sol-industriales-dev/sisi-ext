(() => {
    $.namespace('CH.Evaluacion360');

    //#region CONST FILTROS
    const btnFiltroBuscar = $('#btnFiltroBuscar')
    //#endregion

    //#region CONST GRUPOS
    const tblRH_Eval360_CatGrupos = $('#tblRH_Eval360_CatGrupos');
    let dtGrupos;
    const modalGrupo = $('#modalGrupo');
    const inputDescripcionGrupo = $('#inputDescripcionGrupo');
    const btnModalGrupo = $('#btnModalGrupo');
    const btnGuardarGrupo = $('#btnGuardarGrupo');
    const cboFiltroGrupos = $('#cboFiltroGrupos');
    const btnCancelarCEGrupo = $('#btnCancelarCEGrupo');
    const btnNuevoGrupo = $('#btnNuevoGrupo');
    //#endregion

    //#region CONST COMPETENCIAS
    const tblRH_Eval360_CatCompetencias = $('#tblRH_Eval360_CatCompetencias');
    let dtCompetencias;
    const modalCompetencia = $('#modalCompetencia');
    const cboGrupoCompetencia = $('#cboGrupoCompetencia');
    const cboCompetencias = $('#cboCompetencias');
    const inputDescripcionCompetencia = $('#inputDescripcionCompetencia');
    const inputDefinicionCompetencia = $('#inputDefinicionCompetencia');
    const btnModalCompetencia = $('#btnModalCompetencia');
    const btnGuardarCompetencia = $('#btnGuardarCompetencia');
    const btnCancelarCECompetencia = $('#btnCancelarCECompetencia');
    const cboFiltroGruposCECompetencia = $('#cboFiltroGruposCECompetencia');
    const btnFiltroBuscarCECompetencia = $('#btnFiltroBuscarCECompetencia');
    //#endregion

    //#region CONST CONDUCTAS
    const tblRH_Eval360_CatConductas = $('#tblRH_Eval360_CatConductas');
    let dtConductas;
    const modalConducta = $('#modalConducta');
    const cboGrupoConducta = $('#cboGrupoConducta');
    const cboCompetenciaGrupo = $('#cboCompetenciaGrupo');
    const inputDefinicionCompetenciaGrupo = $('#inputDefinicionCompetenciaGrupo');
    const inputDescripcionConducta = $('#inputDescripcionConducta');
    const btnModalConducta = $('#btnModalConducta');
    const btnGuardarConducta = $('#btnGuardarConducta');
    const btnNuevaCompetencia = $('#btnNuevaCompetencia');
    //#endregion

    //#region CONST MENU
    const menuPersonal = $("#menuPersonal");
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
            //#region INIT DATATABLES
            fncGetNivelAcceso()
            initTablaGrupos()
            initTablaCompetencias()
            initTablaConductas()
            fncGetConductas()
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

            //#region FUNCIONES GRUPOS
            btnCancelarCEGrupo.click(function () {
                btnNuevoGrupo.trigger("click");
            })

            btnNuevoGrupo.click(function () {
                btnGuardarGrupo.data().id = 0;
                fncLimpiarModal();
                btnGuardarGrupo.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
            })

            btnGuardarGrupo.click(function () {
                fncCEGrupo();
            })

            btnModalGrupo.click(() => {
                fncGetGrupos();
                btnGuardarGrupo.data().id = 0;
                modalGrupo.modal('show');
            })

            modalGrupo.on('shown.bs.modal', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            })
            //#endregion

            //#region FUNCIONES FILTROS
            $(".select2").select2();
            cboFiltroGrupos.fillCombo('/Administrativo/Evaluacion360/FillCboGrupos', { est: true }, false, null);

            cboFiltroGrupos.change(function () {
                if ($(this).val() > 0) {
                    cboCompetencias.fillCombo('/Administrativo/Evaluacion360/FillCboCompetencias', { idGrupo: $(this).val() }, false, null);
                }
            })

            btnFiltroBuscar.click(function () {
                fncGetConductas()
            })
            //#endregion

            //#region FUNCIONES COMPETENCIAS
            btnCancelarCECompetencia.click(function () {
                btnNuevaCompetencia.trigger("click");
            })

            btnNuevaCompetencia.click(function () {
                btnGuardarCompetencia.data().id = 0;
                fncLimpiarModal();
                btnGuardarCompetencia.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
            })

            btnModalCompetencia.click(() => {
                fncGetCompetencias();
                cboFiltroGruposCECompetencia.fillCombo('/Administrativo/Evaluacion360/FillCboGrupos', { est: true }, false, null);
                cboGrupoCompetencia.fillCombo('/Administrativo/Evaluacion360/FillCboGrupos', { est: true }, false, null);
                modalCompetencia.modal('show');
            })

            modalCompetencia.on('shown.bs.modal', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            })

            btnGuardarCompetencia.click(function () {
                fncCECompetencia();
            })

            btnFiltroBuscarCECompetencia.click(function () {
                fncGetCompetencias();
            })
            //#endregion

            //#region FUNCIONES CONDUCTAS
            btnModalConducta.click(() => {
                cboGrupoConducta.fillCombo('/Administrativo/Evaluacion360/FillCboGrupos', null, false, null);
                btnGuardarConducta.data().id = 0;
                btnGuardarConducta.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                fncLimpiarModal();
                modalConducta.modal('show');
            })

            cboCompetenciaGrupo.change(function () {
                let option = $(this).find(`option[value="${$(this).val()}"]`);
                let prefijo = option.attr("data-prefijo");
                inputDefinicionCompetenciaGrupo.val(prefijo);
            })

            btnGuardarConducta.click(function () {
                fncCEConducta();
            })

            modalConducta.on('shown.bs.modal', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            });

            cboGrupoConducta.change(function () {
                if ($(this).val() > 0) {
                    cboCompetenciaGrupo.fillCombo('/Administrativo/Evaluacion360/FillCboCompetencias', { idGrupo: $(this).val() }, false, null);
                }
            })
            //#endregion
        }

        //#region FUNCIONES GRUPOS
        function initTablaGrupos() {
            dtGrupos = tblRH_Eval360_CatGrupos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreGrupo', title: 'Grupo' },
                    {
                        title: 'Opciones',
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning btn-editarGrupo' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger btn-eliminarGrupo' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEditar + btnEliminar;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblRH_Eval360_CatGrupos.on('click', '.btn-editarGrupo', function () {
                        let rowData = dtGrupos.row($(this).closest('tr')).data();
                        fncGetDatosActualizarGrupo(rowData.id);
                    });

                    tblRH_Eval360_CatGrupos.on('click', '.btn-eliminarGrupo', function () {
                        let rowData = dtGrupos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarGrupo(rowData.id));
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function fncGetDatosActualizarGrupo(id) {
            if (id > 0) {
                let obj = new Object();
                obj.id = id;
                axios.post('GetDatosActualizarGrupo', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnNuevoGrupo.trigger("click");
                        inputDescripcionGrupo.val(response.data.objGrupo.nombreGrupo);
                        btnGuardarGrupo.data().id = id;
                        btnGuardarGrupo.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener la información del grupo.");
            }
        }

        function fncGetGrupos() {
            axios.post('GetGrupos').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtGrupos.clear();
                    dtGrupos.rows.add(response.data.lstGrupos);
                    dtGrupos.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEGrupo() {
            let objCEGrupo = fncCEGrupoOBJ();
            if (objCEGrupo != "") {
                axios.post("CEGrupo", objCEGrupo).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        cboFiltroGrupos.fillCombo('/Administrativo/Evaluacion360/FillCboGrupos', { est: true }, false, null);
                        btnNuevoGrupo.trigger("click");
                        fncGetGrupos();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEGrupoOBJ() {
            let mensajeError = "";
            if (inputDescripcionGrupo.val() == "") { inputDescripcionGrupo.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputDescripcionGrupo") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return "";
            } else {
                let obj = new Object();
                obj.id = btnGuardarGrupo.data().id;
                obj.nombreGrupo = inputDescripcionGrupo.val();
                return obj;
            }
        }

        function fncEliminarGrupo(idGrupo) {
            if (idGrupo >= 0) {
                let objGrupo = new Object();
                objGrupo.idGrupo = idGrupo;
                axios.post("EliminarGrupo", objGrupo).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        cboFiltroGrupos.fillCombo('/Administrativo/Evaluacion360/FillCboGrupos', { est: true }, false, null);
                        fncGetGrupos();
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
            inputDefinicionCompetencia.val("");
            cboGrupoConducta[0].selectedIndex = 0;
            cboGrupoConducta.trigger("change");
            cboCompetenciaGrupo[0].selectedIndex = 0;
            cboCompetenciaGrupo.trigger("change");
            inputDefinicionCompetenciaGrupo.val("");
            inputDescripcionConducta.val("");
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

        //#region FUNCIONES COMPETENCIAS
        function initTablaCompetencias() {
            dtCompetencias = tblRH_Eval360_CatCompetencias.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreCompetencia', title: 'Competencia' },
                    { data: 'definicion', title: 'Definición' },
                    {
                        title: 'Opciones',
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class="btn-editarCompetencia btn btn-xs btn-warning" title="Editar registro."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn-eliminarCompetencia btn btn-xs btn-danger" title="Eliminar registro."><i class="fas fa-trash"></i></button>`;
                            return btnEditar + btnEliminar;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblRH_Eval360_CatCompetencias.on('click', '.btn-editarCompetencia', function () {
                        let rowData = dtCompetencias.row($(this).closest('tr')).data();
                        fncGetDatosActualizarCompetencia(rowData.id);
                    });

                    tblRH_Eval360_CatCompetencias.on('click', '.btn-eliminarCompetencia', function () {
                        let rowData = dtCompetencias.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCompetencia(rowData.id));
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function fncGetDatosActualizarCompetencia(id) {
            if (id > 0) {
                let obj = new Object();
                obj.id = id;
                axios.post('GetDatosActualizarCompetencia', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnNuevaCompetencia.trigger("click");
                        cboGrupoCompetencia.val(response.data.objCompetencia.idGrupo);
                        cboGrupoCompetencia.trigger("change");
                        inputDescripcionCompetencia.val(response.data.objCompetencia.nombreCompetencia);
                        inputDefinicionCompetencia.val(response.data.objCompetencia.definicion);
                        btnGuardarCompetencia.data().id = id;
                        btnGuardarCompetencia.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener la información de la competencia.");
            }
        }

        function fncGetCompetencias() {
            if (cboFiltroGruposCECompetencia.val() > 0) {
                let obj = new Object();
                obj.idGrupo = cboFiltroGruposCECompetencia.val();
                axios.post('GetCompetencias', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        dtCompetencias.clear();
                        dtCompetencias.rows.add(response.data.lstCompetencias);
                        dtCompetencias.draw();

                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCECompetencia() {
            let objCECompetencia = fncCECompetenciaOBJ();
            if (objCECompetencia != "") {
                axios.post("CECompetencia", objCECompetencia).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        btnCancelarCECompetencia.trigger("click");
                        fncGetCompetencias();
                        fncLimpiarModal();

                        let idGrupo = cboFiltroGrupos.val()
                        cboFiltroGrupos[0].selectedIndex = 0
                        cboFiltroGrupos.trigger("change")
                        cboFiltroGrupos.val(idGrupo)
                        cboFiltroGrupos.trigger("change")
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCECompetenciaOBJ() {
            let mensajeError = "";
            if (cboGrupoCompetencia.val() <= 0) { $("#select2-cboGrupoCompetencia-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("select2-cboGrupoCompetencia-container") }
            if (inputDescripcionCompetencia.val() == "") { inputDescripcionCompetencia.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputDescripcionCompetencia") }
            if (inputDefinicionCompetencia.val() == "") { inputDefinicionCompetencia.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputDefinicionCompetencia") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return "";
            } else {
                let obj = new Object();
                obj.id = btnGuardarCompetencia.data().id;
                obj.idGrupo = cboGrupoCompetencia.val();
                obj.nombreCompetencia = inputDescripcionCompetencia.val();
                obj.definicion = inputDefinicionCompetencia.val();
                return obj;
            }
        }

        function fncEliminarCompetencia(id) {
            if (id >= 0) {
                let objCompetencia = new Object();
                objCompetencia.idCompetencia = id;
                axios.post("EliminarCompetencia", objCompetencia).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetCompetencias();

                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al eliminar el registro.");
            }
        }
        //#endregion

        //#region FUNCIONES CONDUCTAS
        function initTablaConductas() {
            dtConductas = tblRH_Eval360_CatConductas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'descripcionConducta', title: 'Descripción de conducta observable' },
                    {
                        title: 'Opciones',
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn-editarConducta btn btn-xs btn-warning' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn-eliminarConducta btn btn-xs btn-danger' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEditar + btnEliminar;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblRH_Eval360_CatConductas.on('click', '.btn-editarConducta', function () {
                        let rowData = dtConductas.row($(this).closest('tr')).data();
                        fncGetDatosActualizarConducta(rowData.id);
                    });

                    tblRH_Eval360_CatConductas.on('click', '.btn-eliminarConducta', function () {
                        let rowData = dtConductas.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarConducta(rowData.id));
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '5%', targets: [1] }

                ]
            });
        }

        function fncGetDatosActualizarConducta(id) {
            if (id > 0) {
                let obj = new Object();
                obj.id = id;
                axios.post('GetDatosActualizarConducta', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncLimpiarModal();
                        cboGrupoConducta.fillCombo('/Administrativo/Evaluacion360/FillCboGrupos', null, false, null);
                        cboGrupoConducta.val(response.data.objConducta.idGrupo);
                        cboGrupoConducta.trigger("change");
                        cboCompetenciaGrupo.val(response.data.objConducta.idCompetencia);
                        cboCompetenciaGrupo.trigger("change");
                        inputDescripcionConducta.val(response.data.objConducta.definicion);
                        inputDescripcionConducta.val(response.data.objConducta.descripcionConducta);
                        btnGuardarConducta.data().id = id;
                        btnGuardarConducta.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                        modalConducta.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener la información de la conducta.");
            }
        }

        function fncGetConductas() {
            let obj = new Object()
            obj.idGrupo = cboFiltroGrupos.val()
            obj.idCompetencia = cboCompetencias.val()
            axios.post('GetConductas', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtConductas.clear();
                    dtConductas.rows.add(response.data.lstConductas);
                    dtConductas.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEConducta() {
            let obj = fncCEConductaOBJ();
            if (obj != "") {
                axios.post("CEConducta", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetConductas();
                        modalConducta.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEConductaOBJ() {
            let mensajeError = "";
            if (cboGrupoConducta.val() <= 0) { $("#select2-cboGrupoConducta-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("select2-cboGrupoConducta-container") }
            if (cboCompetenciaGrupo.val() <= 0) { $("#select2-cboCompetenciaGrupo-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("select2-cboCompetenciaGrupo-container") }
            if (inputDefinicionCompetenciaGrupo.val() == "") { inputDefinicionCompetenciaGrupo.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputDefinicionCompetenciaGrupo") }
            if (inputDescripcionConducta.val() == "") { inputDescripcionConducta.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputDescripcionConducta") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return "";
            } else {
                let obj = new Object();
                obj.id = btnGuardarConducta.data().id;
                obj.descripcionConducta = inputDescripcionConducta.val();
                obj.idCompetencia = cboCompetenciaGrupo.val();
                return obj;
            }
        }

        function fncEliminarConducta(id) {
            if (id >= 0) {
                let objConducta = new Object();
                objConducta.idConducta = id;
                axios.post("EliminarConducta", objConducta).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetConductas();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al eliminar el registro.");
            }
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Evaluacion360 = new Evaluacion360();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();