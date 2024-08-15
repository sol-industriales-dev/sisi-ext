(() => {
    $.namespace('CH.Evaluacion360');

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

    //#region CONST FILTROS CATALOGO CUESTIONARIOS
    const btnFiltroBuscar = $("#btnFiltroBuscar");
    const btnFiltroNuevoCuestionario = $("#btnFiltroNuevoCuestionario");
    //#endregion

    //#region CONST CATALOGO CUESTIONARIOS
    const mdlCECuestionario = $("#mdlCECuestionario");
    const txtNombreCuestionario = $("#txtNombreCuestionario");
    const btnCECuestionario = $("#btnCECuestionario");
    const tblRH_Eval360_Cuestionarios = $('#tblRH_Eval360_Cuestionarios');
    let dtCuestionario;
    //#endregion

    //#region CONST FILTROS CONDUCTAS REL CUESTIONARIOS
    const cboFiltroConductaRelCuestionario_cboGrupo = $("#cboFiltroConductaRelCuestionario_cboGrupo");
    const cboFiltroConductaRelCuestionario_Competencia = $("#cboFiltroConductaRelCuestionario_Competencia");
    const btnFiltroConductaRelCuestionario_Buscar = $('#btnFiltroConductaRelCuestionario_Buscar');
    //#endregion

    //#region CONST CATALOGO CONDUCTAS REL CUESTIONARIOS
    const tblRH_Eval360_CuestionariosDet = $("#tblRH_Eval360_CuestionariosDet");
    let dtConductasRelCuestionario;
    //#endregion

    //#region CONST FILTROS CONDUCTAS
    const cboFiltroConductas_cboGrupo = $("#cboFiltroConductas_cboGrupo");
    const cboFiltroConductas_cboCompetencia = $("#cboFiltroConductas_cboCompetencia");
    const btnFiltroConductas_Guardar = $('#btnFiltroConductas_Guardar');
    const tblRH_Eval360_CatConductas = $("#tblRH_Eval360_CatConductas");
    const btnFiltroConductaNoRelCuestionario_Buscar = $('#btnFiltroConductaNoRelCuestionario_Buscar')
    let arrConductas_ID = new Array();
    let dtConductas;
    //#endregion

    Evaluacion360 = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            fncGetNivelAcceso()
            initTblCuestionarios()
            initTblConductasRelCuesiontario()
            initTblConductas()
            fncGetCuestionarios()
            $(".select2").select2()
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

            //#region FUNCIONES FILTROS CUESTIONARIOS
            btnFiltroBuscar.click(function () {
                fncGetCuestionarios();
            })

            btnFiltroNuevoCuestionario.click(function () {
                fncLimpiarModal();
                $("#mdlCECuestionario .modal-title").text("NUEVO REGISTRO");
                btnCECuestionario.data().id = 0;
                btnCECuestionario.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                mdlCECuestionario.modal('show');
            })

            btnCECuestionario.click(function () {
                fncCECuestionario();
            })
            //#endregion

            //#region FUNCIONES FILTROS CONDUCTAS REL CUESTIONARIOS
            btnFiltroConductaRelCuestionario_Buscar.click(function () {
                fncGetConductasRelCuestionario($(this).data().id);
            })

            cboFiltroConductaRelCuestionario_cboGrupo.change(function () {
                if ($(this).val() > 0) {
                    cboFiltroConductaRelCuestionario_Competencia.fillCombo('/Administrativo/Evaluacion360/FillCboCompetencias', { est: true, idGrupo: $(this).val() }, false, null);
                }
            })
            //#endregion

            //#region FUNCIONES FILTROS CONDUCTAS
            cboFiltroConductas_cboGrupo.change(function () {
                if ($(this).val() > 0) {
                    cboFiltroConductas_cboCompetencia.fillCombo('/Administrativo/Evaluacion360/FillCboCompetencias', { est: true, idGrupo: $(this).val() }, false, null);
                } else {
                    cboFiltroConductas_cboCompetencia[0].selectedIndex = 0
                    cboFiltroConductas_cboCompetencia.trigger("change")
                }
            })

            btnFiltroConductas_Guardar.click(function () {
                arrConductas_ID = new Array();
                $(":checkbox:checked").map(function () {
                    arrConductas_ID.push(this.id);
                }).get();
                fncCrearConductaRelCuestionario();
            })

            btnFiltroConductaNoRelCuestionario_Buscar.click(function () {
                fncGetConductasDisponibles()
            })
            //#endregion
        }

        //#region FUNCIONES CUESTIONARIOS
        function initTblCuestionarios() {
            dtCuestionario = tblRH_Eval360_Cuestionarios.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreCuestionario', title: 'Nombre del cuestionario' },
                    {
                        title: "Conductas",
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-primary mdlConductasRelCuestionario' title='Conductas relacionadas al cuestionario.'><i class="fas fa-align-justify"></i></button>`;
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
                    tblRH_Eval360_Cuestionarios.on('click', '.mdlConductasRelCuestionario', function () {
                        let rowData = dtCuestionario.row($(this).closest('tr')).data();
                        fncGetConductasRelCuestionario(rowData.id);
                    });

                    tblRH_Eval360_Cuestionarios.on('click', '.editarRegistro', function () {
                        let rowData = dtCuestionario.row($(this).closest('tr')).data();
                        fncGetDatosActualizarCuestionario(rowData.id);
                    });

                    tblRH_Eval360_Cuestionarios.on('click', '.eliminarRegistro', function () {
                        let rowData = dtCuestionario.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCuestionario(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '5%', targets: [1, 2] }
                ],
            });
        }

        function fncGetCuestionarios() {
            axios.post('GetCuestionarios').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtCuestionario.clear();
                    dtCuestionario.rows.add(response.data.lstCuestionarios);
                    dtCuestionario.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCECuestionario() {
            let obj = fncCECuestionarioOBJ();
            if (obj != "") {
                axios.post('CECuestionario', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetCuestionarios();
                        Alert2Exito(message);
                        mdlCECuestionario.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCECuestionarioOBJ() {
            let mensajeError = "";
            if (txtNombreCuestionario.val() == "") { txtNombreCuestionario.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("txtNombreCuestionario") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return "";
            } else {
                let obj = new Object();
                obj.id = btnCECuestionario.data().id;
                obj.nombreCuestionario = txtNombreCuestionario.val();
                return obj;
            }
        }

        function fncGetDatosActualizarCuestionario(id) {
            if (id > 0) {
                let obj = new Object();
                obj.id = id;
                axios.post('GetDatosActualizarCuestionario', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        txtNombreCuestionario.val(response.data.objCuestionario.nombreCuestionario);
                        $("#mdlCECuestionario .modal-title").text("ACTUALIZAR REGISTRO");
                        btnCECuestionario.data().id = id;
                        btnCECuestionario.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                        mdlCECuestionario.modal('show');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener la información del cuestionario.");
            }
        }

        function fncEliminarCuestionario(id) {
            if (id > 0) {
                let obj = new Object();
                obj.idCuestionario = id;
                axios.post('EliminarCuestionario', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetCuestionarios();
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al eliminar el registro.");
            }
        }
        //#endregion

        //#region FUNCIONES CONDUCTAS REL CUESTIONARIOS
        function initTblConductasRelCuesiontario() {
            dtConductasRelCuestionario = tblRH_Eval360_CuestionariosDet.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreCuestionario', title: 'Cuestionario' },
                    { data: 'nombreGrupo', title: 'Grupo' },
                    { data: 'nombreCompetencia', title: 'Competencia' },
                    { data: 'descripcionConducta', title: 'Conducta' },
                    {
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblRH_Eval360_CuestionariosDet.on('click', '.eliminarRegistro', function () {
                        let rowData = dtConductasRelCuestionario.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarConductaRelCuestionario(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '5%', targets: [4] },
                    { width: '20%', targets: [0] }
                ],
            });
        }

        function fncGetConductasRelCuestionario(idCuestionario) {
            if (idCuestionario > 0) {
                let obj = new Object();
                obj.idCuestionario = idCuestionario;
                obj.idGrupo = cboFiltroConductaRelCuestionario_cboGrupo.val();
                obj.idCompetencia = cboFiltroConductaRelCuestionario_Competencia.val();
                axios.post('GetConductasRelCuestionario', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnFiltroConductaRelCuestionario_Buscar.data().id = idCuestionario;
                        dtConductasRelCuestionario.clear();
                        dtConductasRelCuestionario.rows.add(response.data.lstConductasRelCuestionario);
                        dtConductasRelCuestionario.draw();

                        cboFiltroConductaRelCuestionario_cboGrupo.fillCombo('/Administrativo/Evaluacion360/FillCboGrupos', { est: true }, false, null);
                        cboFiltroConductas_cboGrupo.fillCombo('/Administrativo/Evaluacion360/FillCboGrupos', { est: true }, false, null);
                        fncGetConductasDisponibles(idCuestionario);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCrearConductaRelCuestionario() {
            let idCuestionario = btnFiltroConductaRelCuestionario_Buscar.data().id;
            if (arrConductas_ID.length > 0 && btnFiltroConductaRelCuestionario_Buscar.data().id > 0) {
                axios.post('CrearConductaRelCuestionario', { idCuestionario: idCuestionario, lstConductasID: arrConductas_ID }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetConductasRelCuestionario(idCuestionario);
                        fncGetConductasDisponibles(idCuestionario);
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (arrConductas_ID.length <= 0) { Alert2Warning("Es necesario seleccionar al menos una conduta.") }
                if (btnFiltroConductaRelCuestionario_Buscar.data().id <= 0) {
                    let mensajeError = arrConductas_ID.length == 1 ? "Ocurrió un error al guardar las conductas seleccionadas" : "Ocurrió un error al guardar la conduta seleccionada.";
                    Alert2Warning(mensajeError);
                }
            }
        }

        function fncEliminarConductaRelCuestionario(idConductaRelCuestonario) {
            let idCuestionario = btnFiltroConductaRelCuestionario_Buscar.data().id;
            if (idConductaRelCuestonario > 0 && idCuestionario > 0) {
                let obj = new Object();
                obj.idConductaRelCuestonario = idConductaRelCuestonario;
                axios.post('EliminarConductaRelCuestionario', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetConductasRelCuestionario(btnFiltroConductaRelCuestionario_Buscar.data().id);
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (idConductaRelCuestonario <= 0) { Alert2Warning("Ocurrió un error al eliminar el registro seleccionado.") }
                if (btnFiltroConductaRelCuestionario_Buscar.data().id <= 0) { Alert2Warning("Ocurrió un error al eliminar el registro seleccionado.") }
            }
        }
        //#endregion

        //#region FUNCIONES CONDUCTAS
        function initTblConductas() {
            dtConductas = tblRH_Eval360_CatConductas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'descripcionConducta', title: 'Descripción de conducta' },
                    {
                        title: 'Seleccionar',
                        render: function (data, type, row, meta) {
                            return `<input type='checkbox' id="${row.id}" class="chkConducta" />`;
                        },
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function fncGetConductasDisponibles(idCuestionario) {
            if (btnFiltroConductaRelCuestionario_Buscar.data().id > 0) {
                let obj = new Object();
                obj.idCuestionario = btnFiltroConductaRelCuestionario_Buscar.data().id
                obj.idGrupo = cboFiltroConductas_cboGrupo.val()
                obj.idCompetencia = cboFiltroConductas_cboCompetencia.val()
                axios.post('GetConductasDisponibles', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtConductas.clear();
                        dtConductas.rows.add(response.data.lstConductasDisponibles);
                        dtConductas.draw();
                        // #endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener el listado de conductas disponibles.");
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncLimpiarModal() {
            $("input[type='text']").val("");
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