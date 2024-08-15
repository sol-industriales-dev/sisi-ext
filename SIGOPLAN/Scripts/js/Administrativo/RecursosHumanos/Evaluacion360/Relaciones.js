(() => {
    $.namespace('CH.Relaciones')

    //#region CONST FILTROS
    const cboFiltroPeriodo = $('#cboFiltroPeriodo')
    const btnFiltroBuscar = $("#btnFiltroBuscar")
    const btnFiltroNuevo = $("#btnFiltroNuevo")
    //#endregion

    //#region CONST MENU
    const menuPersonal = $('#menuPersonal')
    const menuConductas = $("#menuConductas")
    const menuCuestionarios = $("#menuCuestionarios")
    const menuPeriodos = $("#menuPeriodos")
    const menuCriterios = $("#menuCriterios")
    const menuRelaciones = $('#menuRelaciones')
    const menuEvaluaciones = $('#menuEvaluaciones')
    const menuReporte360 = $('#menuReporte360')
    const menuAvances = $('#menuAvances')
    //#endregion

    //#region CONST DATATABLE RELACIONES
    const tblRH_Eval360_Relaciones = $('#tblRH_Eval360_Relaciones')
    let dtRelacion
    //#endregion

    //#region CONST CREAR/EDITAR RELACION
    const mdlCERelacion = $("#mdlCERelacion")
    const cboCEEvaluado = $("#cboCEEvaluado")
    const cboCECuestionario = $("#cboCECuestionario")
    const btnCERelacion = $("#btnCERelacion")

    const edicionEvaluado_txtCEEvaluado = $("#edicionEvaluado_txtCEEvaluado")
    const edicionEvaluado_cboCEEvaluador = $("#edicionEvaluado_cboCEEvaluador")
    const edicionEvaluado_cboCETipoRelacion = $("#edicionEvaluado_cboCETipoRelacion")
    const edicionEvaluado_cboCECuestionario = $("#edicionEvaluado_cboCECuestionario")
    const edicionEvaluado_btnCERelacion = $("#edicionEvaluado_btnCERelacion")
    const mdlListadoEvaluadoresRelEvaluado = $("#mdlListadoEvaluadoresRelEvaluado");
    const tblListadoEvaluadoresRelEvaluador = $("#tblListadoEvaluadoresRelEvaluador")
    let dtEvaluadoresRelEvaluado;
    //#endregion

    Relaciones = function () {
        (function init() {
            fncListeners()
        })()

        function fncListeners() {
            //#region INIT DATATABLE
            fncGetNivelAcceso()
            initTblRelaciones()
            initTblEvaluadoresRelEvaluado()
            //#endregion

            //#region FUNCIONES FILTROS
            $(".select2").select2()
            cboFiltroPeriodo.fillCombo('FillCboPeriodos', null, false, null)

            btnFiltroBuscar.click(function () {
                let mensajeError = ""
                if (cboFiltroPeriodo.val() <= 0) { $("#select2-cboFiltroPeriodo-container").css('border', '2px solid red'); mensajeError = 'Es necesario seleccionar un periodo.'; } else { fncDefaultCtrls("select2-cboFiltroPeriodo-container") }
                if (mensajeError != "") {
                    Alert2Warning(mensajeError)
                } else {
                    fncGetRelaciones();
                }
            })

            btnFiltroNuevo.click(function () {
                let mensajeError = ""
                if (cboFiltroPeriodo.val() <= 0) { $("#select2-cboFiltroPeriodo-container").css('border', '2px solid red'); mensajeError = 'Es necesario seleccionar un periodo.'; } else { fncDefaultCtrls("select2-cboFiltroPeriodo-container") }
                if (mensajeError != "") {
                    Alert2Warning(mensajeError)
                } else {
                    cboCECuestionario.fillCombo('FillCboCuestionarios', null, false, null)
                    cboCEEvaluado.fillCombo('FillCboPersonalRelRelacionDisponibles', { idPeriodo: cboFiltroPeriodo.val() }, false, null)
                    btnCERelacion.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                    $("#mdlCERelacion .modal-title").text("NUEVO REGISTRO");
                    mdlCERelacion.modal("show")
                }
            })
            //#endregion

            //#region FUNCIONES CREAR/EDITAR RELACIÓN
            btnCERelacion.click(function () {
                fncCERelacion()
            })

            edicionEvaluado_btnCERelacion.click(function () {
                fncCE_edicionEvaluado()
            })
            //#endregion

            //#region FUNCIONES MENU
            menuPersonal.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Personal'
            })

            menuConductas.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Conductas'
            })

            menuCuestionarios.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Cuestionarios'
            })

            menuPeriodos.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Periodos'
            })

            menuCriterios.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Criterios'
            })

            menuRelaciones.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Relaciones'
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
        }

        //#region FUNCIONES RELACIONES
        function initTblRelaciones() {
            dtRelacion = tblRH_Eval360_Relaciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreEvaluado', title: 'Nombre del evaluado' },
                    { data: 'nombreAutoevaluacion', title: 'Autoevaluación' },
                    {
                        title: 'Pares',
                        render: function (data, type, row) {
                            let btnNuevo = `<button class='btn btn-xs btn-primary btnCEUsuarioPar'><i class="fas fa-plus"></i></button><br>`;
                            let lstEvaluadores = `${row.lstEvaluadores_PARES}`;

                            if (row.lstEvaluadores_PARES != null) {
                                return btnNuevo + lstEvaluadores;
                            } else {
                                return btnNuevo;
                            }
                        }
                    },
                    {
                        title: 'Clientes internos',
                        render: function (data, type, row) {
                            let btnNuevo = `<button class='btn btn-xs btn-primary btnCEUsuarioClienteInterno'><i class="fas fa-plus"></i></button><br>`;
                            let lstEvaluadores = `${row.lstEvaluadores_CLIENTES_INTERNOS}`;

                            if (row.lstEvaluadores_CLIENTES_INTERNOS != null) {
                                return btnNuevo + lstEvaluadores;
                            } else {
                                return btnNuevo;
                            }
                        }
                    },
                    {
                        title: 'Colaboradores',
                        render: function (data, type, row) {
                            let btnNuevo = `<button class='btn btn-xs btn-primary btnCEUsuarioColaborador'><i class="fas fa-plus"></i></button><br>`;
                            let lstEvaluadores = `${row.lstEvaluadores_COLABORADORES}`;

                            if (row.lstEvaluadores_COLABORADORES != null) {
                                return btnNuevo + lstEvaluadores;
                            } else {
                                return btnNuevo;
                            }
                        }
                    },
                    {
                        title: 'Jefe',
                        render: function (data, type, row) {
                            let btnNuevo = `<button class='btn btn-xs btn-primary btnCEUsuarioJefe'><i class="fas fa-plus"></i></button><br>`;
                            let lstEvaluadores = `${row.lstEvaluadores_JEFE}`;

                            if (row.lstEvaluadores_JEFE != null) {
                                return btnNuevo + lstEvaluadores;
                            } else {
                                return btnNuevo;
                            }
                        }
                    },
                    {
                        render: function (data, type, row) {
                            return `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblRH_Eval360_Relaciones.on('click', '.btnCEUsuarioPar', function () {
                        let rowData = dtRelacion.row($(this).closest('tr')).data()

                        // SE INDICA QUIEN ES EL EVALUADO Y QUE TIPO DE RELACIÓN ESTAN CONSULTANDO/CREANDO
                        let tipoRelacion = 2 // PAR
                        edicionEvaluado_btnCERelacion.data().id = rowData.id
                        edicionEvaluado_btnCERelacion.data().idPersonalEvaluado = rowData.idPersonalEvaluado
                        edicionEvaluado_btnCERelacion.data().tipoRelacion = tipoRelacion;
                        edicionEvaluado_txtCEEvaluado.val(rowData.nombreEvaluado)
                        edicionEvaluado_cboCETipoRelacion.fillCombo('FillCboTipoRelacionEvaluado', null, false, null)
                        edicionEvaluado_cboCETipoRelacion.val(tipoRelacion)
                        edicionEvaluado_cboCETipoRelacion.trigger("change")
                        edicionEvaluado_cboCECuestionario.fillCombo('FillCboCuestionarios', null, false, null)

                        fncGetListadoEvaluadoresRelEvaluador(rowData.idPersonalEvaluado, tipoRelacion)
                        $("#mdlListadoEvaluadoresRelEvaluado .modal-title").text("PERSONAL TIPO PAR");
                        edicionEvaluado_btnCERelacion.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                        mdlListadoEvaluadoresRelEvaluado.modal("show")
                    })

                    tblRH_Eval360_Relaciones.on('click', '.btnCEUsuarioClienteInterno', function () {
                        let rowData = dtRelacion.row($(this).closest('tr')).data()

                        // SE INDICA QUIEN ES EL EVALUADO Y QUE TIPO DE RELACIÓN ESTAN CONSULTANDO/CREANDO
                        let tipoRelacion = 3 // CLIENTE INTERNO
                        edicionEvaluado_btnCERelacion.data().id = rowData.id
                        edicionEvaluado_btnCERelacion.data().idPersonalEvaluado = rowData.idPersonalEvaluado
                        edicionEvaluado_btnCERelacion.data().tipoRelacion = tipoRelacion;
                        edicionEvaluado_txtCEEvaluado.val(rowData.nombreEvaluado)
                        edicionEvaluado_cboCETipoRelacion.fillCombo('FillCboTipoRelacionEvaluado', null, false, null)
                        edicionEvaluado_cboCETipoRelacion.val(tipoRelacion)
                        edicionEvaluado_cboCETipoRelacion.trigger("change")
                        edicionEvaluado_cboCECuestionario.fillCombo('FillCboCuestionarios', null, false, null)

                        fncGetListadoEvaluadoresRelEvaluador(rowData.idPersonalEvaluado, tipoRelacion)
                        $("#mdlListadoEvaluadoresRelEvaluado .modal-title").text("PERSONAL TIPO CLIENTE INTERNO");
                        edicionEvaluado_btnCERelacion.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                        mdlListadoEvaluadoresRelEvaluado.modal("show")
                    })

                    tblRH_Eval360_Relaciones.on('click', '.btnCEUsuarioColaborador', function () {
                        let rowData = dtRelacion.row($(this).closest('tr')).data()

                        // SE INDICA QUIEN ES EL EVALUADO Y QUE TIPO DE RELACIÓN ESTAN CONSULTANDO/CREANDO
                        let tipoRelacion = 4 // COLABORADOR
                        edicionEvaluado_btnCERelacion.data().id = rowData.id
                        edicionEvaluado_btnCERelacion.data().idPersonalEvaluado = rowData.idPersonalEvaluado
                        edicionEvaluado_btnCERelacion.data().tipoRelacion = tipoRelacion;
                        edicionEvaluado_txtCEEvaluado.val(rowData.nombreEvaluado)
                        edicionEvaluado_cboCETipoRelacion.fillCombo('FillCboTipoRelacionEvaluado', null, false, null)
                        edicionEvaluado_cboCETipoRelacion.val(tipoRelacion)
                        edicionEvaluado_cboCETipoRelacion.trigger("change")
                        edicionEvaluado_cboCECuestionario.fillCombo('FillCboCuestionarios', null, false, null)

                        fncGetListadoEvaluadoresRelEvaluador(rowData.idPersonalEvaluado, tipoRelacion)
                        $("#mdlListadoEvaluadoresRelEvaluado .modal-title").text("PERSONAL TIPO COLABORADOR");
                        edicionEvaluado_btnCERelacion.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                        mdlListadoEvaluadoresRelEvaluado.modal("show")
                    })

                    tblRH_Eval360_Relaciones.on('click', '.btnCEUsuarioJefe', function () {
                        let rowData = dtRelacion.row($(this).closest('tr')).data()

                        // SE INDICA QUIEN ES EL EVALUADO Y QUE TIPO DE RELACIÓN ESTAN CONSULTANDO/CREANDO
                        let tipoRelacion = 5 // JEFE
                        edicionEvaluado_btnCERelacion.data().id = rowData.id
                        edicionEvaluado_btnCERelacion.data().idPersonalEvaluado = rowData.idPersonalEvaluado
                        edicionEvaluado_btnCERelacion.data().tipoRelacion = tipoRelacion;
                        edicionEvaluado_txtCEEvaluado.val(rowData.nombreEvaluado)
                        edicionEvaluado_cboCETipoRelacion.fillCombo('FillCboTipoRelacionEvaluado', null, false, null)
                        edicionEvaluado_cboCETipoRelacion.val(tipoRelacion)
                        edicionEvaluado_cboCETipoRelacion.trigger("change")
                        edicionEvaluado_cboCECuestionario.fillCombo('FillCboCuestionarios', null, false, null)

                        fncGetListadoEvaluadoresRelEvaluador(rowData.idPersonalEvaluado, tipoRelacion)
                        $("#mdlListadoEvaluadoresRelEvaluado .modal-title").text("PERSONAL TIPO JEFE");
                        edicionEvaluado_btnCERelacion.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                        mdlListadoEvaluadoresRelEvaluado.modal("show")
                    })

                    tblRH_Eval360_Relaciones.on('click', '.eliminarRegistro', function () {
                        let rowData = dtRelacion.row($(this).closest('tr')).data()
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?<br>Nota: Se eliminara el detalle de la relación.', 'Confirmar', 'Cancelar', () => fncEliminarRelacion(rowData.id))
                    })
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    {
                        targets: 0,
                        checkboxes: {
                            selectRow: true
                        }
                    },
                    // { className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '5%', targets: [6] }
                ],
                select: {
                    style: 'multi'
                }
            })
        }

        function fncGetRelaciones() {
            if (cboFiltroPeriodo.val() > 0) {
                let obj = new Object()
                obj.idPeriodo = cboFiltroPeriodo.val()
                axios.post('GetRelaciones', obj).then(response => {
                    let { success, items, message } = response.data
                    if (success) {
                        //#region FILL DATATABLE
                        dtRelacion.clear()
                        dtRelacion.rows.add(response.data.lstDTO)
                        dtRelacion.draw()
                        //#endregion
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message))
            } else {
                Alert2Warning("Es necesario seleccionar un periodo.")
            }
        }

        function fncCERelacion() {
            let objCERelacionOBJ = fncCERelacionOBJ();
            if (objCERelacionOBJ != "") {
                axios.post('CERelacion', objCERelacionOBJ).then(response => {
                    let { success, items, message } = response.data
                    if (success) {
                        fncGetRelaciones()
                        Alert2Exito(message)
                        mdlCERelacion.modal("hide")
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message))
            }
        }

        function fncCERelacionOBJ() {
            let mensajeError = "";
            if (cboFiltroPeriodo.val() <= 0) { $("#select2-cboFiltroPeriodo-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboFiltroPeriodo-container") }
            if (cboCEEvaluado.val() <= 0) { $("#select2-cboCEEvaluado-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboCEEvaluado-container") }

            if (mensajeError != "") {
                Alert2Error(mensajeError)
                return ""
            } else {
                let obj = new Object()
                obj.id = btnCERelacion.data().id
                obj.idPeriodo = cboFiltroPeriodo.val()
                obj.idPersonalEvaluado = cboCEEvaluado.val()
                obj.idCuestionario = cboCECuestionario.val()
                return obj
            }
        }

        function fncEliminarRelacion(id) {
            if (id > 0) {
                let obj = new Object()
                obj.id = id
                axios.post('EliminarRelacion', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetRelaciones()
                        Alert2Exito(message)
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el registro.")
            }
        }
        //#endregion

        //#region FUNCIONES RELACIONES DETALLE
        function initTblEvaluadoresRelEvaluado() {
            dtEvaluadoresRelEvaluado = tblListadoEvaluadoresRelEvaluador.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreCompleto', title: 'Evaluador' },
                    { data: 'nombreCuestionario', title: 'Cuestionario' },
                    {
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblListadoEvaluadoresRelEvaluador.on('click', '.eliminarRegistro', function () {
                        let rowData = dtEvaluadoresRelEvaluado.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarEvaluadorRelEvaluado(rowData.id, rowData.idPeriodo, rowData.idPersonalEvaluado, rowData.idPersonalEvaluador));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ]
            });
        }

        function fncGetListadoEvaluadoresRelEvaluador(idPersonalEvaluado, tipoRelacion) {
            if (idPersonalEvaluado > 0 && tipoRelacion > 0 && cboFiltroPeriodo.val() > 0) {
                let obj = new Object();
                obj.idPersonalEvaluado = idPersonalEvaluado;
                obj.tipoRelacion = tipoRelacion;
                obj.idPeriodo = cboFiltroPeriodo.val()
                axios.post('GetListadoEvaluadoresRelEvaluador', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtEvaluadoresRelEvaluado.clear();
                        dtEvaluadoresRelEvaluado.rows.add(response.data.lstEvaluadores);
                        dtEvaluadoresRelEvaluado.draw();

                        fncGetRelaciones()

                        let lstEvaluadores_ID = new Array();
                        tblRH_Eval360_Relaciones.DataTable().rows().eq(0).each(function (index) {
                            let row = tblRH_Eval360_Relaciones.DataTable().row(index)
                            let data = row.data()

                            if (data.id > 0) {
                                lstEvaluadores_ID.push(data.id)
                            }
                        });

                        edicionEvaluado_cboCEEvaluador.fillCombo('FillCboEvaluadores', { lstEvaluadores_ID: lstEvaluadores_ID }, false, null)
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (cboFiltroPeriodo.val() <= 0) {
                    Alert2Error("Es necesario seleccionar un periodo.")
                } else {
                    Alert2Error("Ocurrió un error al obtener la información.")
                }
            }
        }

        function fncCE_edicionEvaluado() {
            let obj = fncCERelacionDetOBJ()
            if (obj != "") {
                axios.post('CE_edicionEvaluado', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetListadoEvaluadoresRelEvaluador(edicionEvaluado_btnCERelacion.data().idPersonalEvaluado, edicionEvaluado_btnCERelacion.data().tipoRelacion)
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al registrar al evaluador.")
            }
        }

        function fncCERelacionDetOBJ() {
            let mensajeError = "";
            if (edicionEvaluado_cboCEEvaluador.val() <= 0) { $("#select2-edicionEvaluado_cboCEEvaluador-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-edicionEvaluado_cboCEEvaluador-container") }
            if (edicionEvaluado_cboCECuestionario.val() <= 0) { $("#select2-edicionEvaluado_cboCECuestionario-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-edicionEvaluado_cboCECuestionario-container") }

            if (mensajeError != "") {
                Alert2Error(mensajeError)
                return ""
            } else {
                let obj = new Object()
                obj.id = 0
                obj.idRelacion = edicionEvaluado_btnCERelacion.data().id
                obj.idPeriodo = cboFiltroPeriodo.val()
                obj.idPersonalEvaluador = edicionEvaluado_cboCEEvaluador.val()
                obj.tipoRelacion = edicionEvaluado_cboCETipoRelacion.val()
                obj.idCuestionario = edicionEvaluado_cboCECuestionario.val()
                return obj
            }
        }

        function fncEliminarEvaluadorRelEvaluado(id, idPeriodo, idPersonalEvaluado, idPersonalEvaluador) {
            if (id > 0 && idPeriodo > 0 && idPersonalEvaluado > 0 && idPersonalEvaluador > 0) {
                let obj = new Object()
                obj.id = id
                obj.idPeriodo = idPeriodo
                obj.idPersonalEvaluado = idPersonalEvaluado
                obj.idPersonalEvaluador = idPersonalEvaluador
                axios.post('EliminarEvaluadorRelEvaluado', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetListadoEvaluadoresRelEvaluador(edicionEvaluado_btnCERelacion.data().idPersonalEvaluado, edicionEvaluado_btnCERelacion.data().tipoRelacion)
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let mensajeError = "Ocurrió un error al eliminar el registro.";
                if (id <= 0) { Alert2Warning(mensajeError) }
                if (idPeriodo <= 0) { Alert2Warning(mensajeError) }
                if (idPersonalEvaluado <= 0) { Alert2Warning(mensajeError) }
                if (idPersonalEvaluador <= 0) { Alert2Warning(mensajeError) }
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncDefaultCtrls(obj) {
            $(`#${obj}`).css("border", "1px solid #CCC")
        }

        function fncLimpiarMdlCEPersonal() {
            //#region LIMPIAR MODAL CREAR/EDITAR PERSONAL
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
        CH.Relaciones = new Relaciones()
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }) })
        .ajaxStop(() => { $.unblockUI() })
})()