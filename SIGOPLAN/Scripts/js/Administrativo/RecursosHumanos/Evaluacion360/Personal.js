(() => {
    $.namespace('CH.Evaluacion360');

    //#region CONST FILTROS
    const cboFiltroCC = $('#cboFiltroCC')
    const cboFiltroTipoUsuario = $('#cboFiltroTipoUsuario')
    const btnFiltroBuscar = $('#btnFiltroBuscar')
    const btnFiltroNuevo = $('#btnFiltroNuevo')
    const btnFiltroAsignarPeriodo = $('#btnFiltroAsignarPeriodo')
    //#endregion

    //#region CONST PERIODO
    const mdlAsignarPeriodo = $('#mdlAsignarPeriodo')
    const cboAsignarPeriodo = $("#cboAsignarPeriodo")
    const btnAsignarPeriodo = $("#btnAsignarPeriodo")
    const cboAsignarCuestionario = $('#cboAsignarCuestionario')
    //#endregion

    //#region CONST DATATABLE PERSONAL
    const tblRH_Eval360_CatPersonal = $('#tblRH_Eval360_CatPersonal');
    let dtPersonal;
    //#endregion

    //#region CONST CREAR/EDITAR PERSONAL
    const mdlCEPersonal = $('#mdlCEPersonal');
    const cboCENombre = $("#cboCENombre");
    const txtCEDepartamento = $("#txtCEDepartamento");
    const txtCECorreo = $("#txtCECorreo");
    const txtCEPuesto = $("#txtCEPuesto");
    const cboCETipoUsuario = $("#cboCETipoUsuario");
    const txtCETelefono = $("#txtCETelefono");
    const btnCEPersonal = $('#btnCEPersonal');
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
            initTblPersonal()
            fncGetCatalogoPersonal()
            //#endregion

            //#region FUNCIONES FILTROS
            $(".select2").select2();
            cboFiltroCC.fillCombo('FillCboCC', null, false, null)
            cboFiltroTipoUsuario.fillCombo("FillCboTipoUsuarios", null, false, null)

            btnFiltroBuscar.click(function () {
                fncGetCatalogoPersonal();
            })

            btnFiltroNuevo.click(function () {
                btnCEPersonal.data().id = 0;
                btnCEPersonal.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                $("#mdlCEPersonal .modal-title").text("NUEVO REGISTRO");
                txtCEDepartamento.attr("disabled", "disabled");
                txtCEPuesto.attr("disabled", "disabled");
                txtCECorreo.attr("disabled", "disabled");
                fncLimpiarMdlCEPersonal();
                mdlCEPersonal.modal("show");
            })

            btnFiltroAsignarPeriodo.click(function () {
                //#region SE OBTIENE ID DEL PERSONAL
                let arrRowsChecked = [];
                let rowsChecked = tblRH_Eval360_CatPersonal.DataTable().column(0).checkboxes.selected();
                $.each(rowsChecked, function (index, id) {
                    arrRowsChecked.push(id);
                });
                //#endregion

                cboAsignarPeriodo[0].selectedIndex = 0
                cboAsignarPeriodo.trigger("change")
                cboAsignarCuestionario[0].selectedIndex = 0
                cboAsignarCuestionario.trigger("change")
                mdlAsignarPeriodo.data().lstPersonalID = null;
                mdlAsignarPeriodo.data().lstPersonalID = arrRowsChecked;
                mdlAsignarPeriodo.modal("show")
            })
            //#endregion

            //#region FUNCIONES CREAR/EDITAR PERSONAL

            // ATTR AUTO-COMPLETE FORMULARIO
            $("input[type='text']").attr("autocomplete", "off");

            // FILL COMBOS
            cboCENombre.fillCombo('FillCboUsuarios', null, false, null)
            // txtCEDepartamento.fillCombo('FillCboCC', null, false, null)
            // txtCEPuesto.fillCombo('FillCboPuestos', null, false, null)
            cboCETipoUsuario.fillCombo("FillCboTipoUsuarios", null, false, null)

            cboCENombre.change(function () {
                //#region SE OBTIENE LA INFORMACIÓN DEL USUARIO SELECCIONADO.
                if (cboCENombre.val() > 0) {
                    let option = cboCENombre.find(`option[value="${cboCENombre.val()}"]`);
                    let idEmpresa = option.attr("data-prefijo");
                    fncGetInformacionUsuario(cboCENombre.val(), idEmpresa);
                }
                //#endregion
            })

            btnCEPersonal.click(function () {
                fncCECatalogoPersonal();
            })
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

            //#region FUNCIONES ASIGNAR PERIODOS
            $(".select2").select2()
            cboAsignarPeriodo.fillCombo('FillCboPeriodos', null, false, null)

            cboAsignarCuestionario.fillCombo('FillCboCuestionarios', null, false, null)

            btnAsignarPeriodo.click(function () {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea crear las relaciones en base al personal, periodo y cuestionario seleccionado?', 'Confirmar', 'Cancelar', () => fncCrearRelacion());
            })
            //#endregion
        }

        //#region FUNCIONES PERSONAL
        function initTblPersonal() {
            dtPersonal = tblRH_Eval360_CatPersonal.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id' },
                    { data: 'nombreCompleto', title: 'Nombre' },
                    { data: 'descripcionCC', title: 'Departamento' },
                    { data: 'correo', title: 'Correo' },
                    { data: 'tipoUsuario', title: 'Tipo' },
                    {
                        title: 'Opciones',
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEditar + btnEliminar;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblRH_Eval360_CatPersonal.on('click', '.editarRegistro', function () {
                        let rowData = dtPersonal.row($(this).closest('tr')).data();
                        fncGetDatosActualizarPersonal(rowData.id);
                    });
                    tblRH_Eval360_CatPersonal.on('click', '.eliminarRegistro', function () {
                        let rowData = dtPersonal.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarPersonal(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: '8%', targets: [4] },
                    { width: '5%', targets: [0, 5] },
                    {
                        targets: 0,
                        checkboxes: {
                            selectRow: true
                        }
                    }
                ],
                select: {
                    style: 'multi'
                }
            });
        }

        function fncGetDatosActualizarPersonal(id) {
            if (id > 0) {
                let obj = new Object();
                obj.id = id;
                axios.post('GetDatosActualizarPersonal', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncDefaultCtrls("select2-cboCENombre-container");
                        fncDefaultCtrls("select2-cboCETipoUsuario-container");
                        fncDefaultCtrls("txtCETelefono");
                        $("#mdlCEPersonal .modal-title").text("ACTUALIZAR REGISTRO");
                        cboCENombre.val(response.data.objPersonal.idUsuario);
                        cboCENombre.trigger("change");
                        cboCETipoUsuario.val(response.data.objPersonal.idTipoUsuario);
                        cboCETipoUsuario.trigger("change");
                        txtCETelefono.val(response.data.objPersonal.telefono);
                        btnCEPersonal.data().id = id;
                        btnCEPersonal.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                        mdlCEPersonal.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener la información del colaborador.");
            }
        }

        function fncGetCatalogoPersonal() {
            let obj = new Object()
            obj.cc = cboFiltroCC.val()
            obj.idTipoUsuario = cboFiltroTipoUsuario.val()
            axios.post('GetCatalogoPersonal', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtPersonal.clear();
                    dtPersonal.rows.add(response.data.lstPersonal);
                    dtPersonal.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCECatalogoPersonal() {
            let objCEPersonal = fncCEPersonalOBJ();
            if (objCEPersonal != "") {
                axios.post('CECatalogoPersonal', objCEPersonal).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        mdlCEPersonal.modal("hide");
                        Alert2Exito(message);
                        fncGetCatalogoPersonal();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEPersonalOBJ() {
            let mensajeError = "";
            if (cboCENombre.val() <= 0) { $("#select2-cboCENombre-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("select2-cboCENombre-container") }
            if (cboCETipoUsuario.val() <= 0) { $("#select2-cboCETipoUsuario-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("select2-cboCETipoUsuario-container") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return "";
            } else {
                let option = cboCENombre.find(`option[value="${cboCENombre.val()}"]`);
                let idEmpresa = option.attr("data-prefijo");

                let obj = new Object();
                obj.id = btnCEPersonal.data().id;
                obj.idUsuario = cboCENombre.val();
                obj.idEmpresa = idEmpresa;
                obj.idTipoUsuario = cboCETipoUsuario.val();
                obj.telefono = txtCETelefono.val();
                return obj;
            }
        }

        function fncGetInformacionUsuario(idUsuario, idEmpresa) {
            if (idUsuario > 0 && idEmpresa > 0) {
                let obj = new Object();
                obj.idUsuario = idUsuario
                obj.idEmpresa = idEmpresa
                axios.post('GetInformacionUsuario', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        txtCEDepartamento.val(response.data.objInfoColaborador.cc);
                        // txtCEDepartamento.trigger("change");
                        txtCECorreo.val(response.data.objInfoColaborador.correo);
                        txtCEPuesto.val(response.data.objInfoColaborador.descripcionPuesto);
                        // txtCEPuesto.trigger("change");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener la información del colaborador.");
            }
        }

        function fncEliminarPersonal(id) {
            if (id > 0) {
                let obj = new Object();
                obj.idCatalogoPersonal = id;
                axios.post('EliminarCatalogoPersonal', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetCatalogoPersonal();
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

        //#region FUNCIONES ASIGNAR PERIODO
        function fncCrearRelacion() {
            let objCERelacionOBJ = fncCERelacionOBJ();
            if (objCERelacionOBJ != "") {
                axios.post('CERelacion', objCERelacionOBJ).then(response => {
                    let { success, items, message } = response.data
                    if (success) {
                        fncGetCatalogoPersonal()
                        Alert2Exito(message)
                        mdlAsignarPeriodo.modal("hide")
                        mdlAsignarPeriodo.data().lstPersonalID = null
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message))
            }
        }

        function fncCERelacionOBJ() {
            let mensajeError = "";
            if (cboAsignarPeriodo.val() <= 0) { $("#select2-cboAsignarPeriodo-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboAsignarPeriodo-container") }
            if (cboAsignarCuestionario.val() <= 0) { $("#select2-cboAsignarCuestionario-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboAsignarCuestionario-container") }
            if (mdlAsignarPeriodo.data().lstPersonalID == "") { mensajeError += "<br>Es necesario seleccionar al menos un personal." }

            if (mensajeError != "") {
                Alert2Error(mensajeError)
                return ""
            } else {
                let obj = new Object()
                obj.idPeriodo = cboAsignarPeriodo.val()
                obj.idCuestionario = cboAsignarCuestionario.val()
                obj.lstPersonalID = mdlAsignarPeriodo.data().lstPersonalID
                return obj
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncDefaultCtrls(obj) {
            $(`#${obj}`).css("border", "1px solid #CCC");
        }

        function fncLimpiarMdlCEPersonal() {
            //#region LIMPIAR MODAL CREAR/EDITAR PERSONAL
            cboCENombre[0].selectedIndex = 0;
            cboCENombre.trigger("change");
            txtCEDepartamento.val("");
            txtCEPuesto.val("");
            txtCECorreo.val("");
            cboCETipoUsuario[0].selectedIndex = 0;
            cboCETipoUsuario.trigger("change");
            txtCETelefono.val("");
            btnCEPersonal.data().id = 0;
            //#endregion
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