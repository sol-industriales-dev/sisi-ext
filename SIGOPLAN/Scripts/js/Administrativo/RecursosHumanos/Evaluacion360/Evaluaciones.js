(() => {
    $.namespace('CH.Evaluacion360');

    //#region CONST FILTROS
    const cboFiltroCC = $("#cboFiltroCC")
    const btnFiltroBuscar = $("#btnFiltroBuscar")
    //#endregion

    //#region CONST MENU
    const menuPersonal = $("#menuPersonal")
    const menuConductas = $("#menuConductas")
    const menuCuestionarios = $("#menuCuestionarios")
    const menuPeriodos = $("#menuPeriodos")
    const menuCriterios = $("#menuCriterios")
    const menuRelaciones = $("#menuRelaciones")
    const menuEvaluaciones = $("#menuEvaluaciones")
    const menuReporte360 = $('#menuReporte360')
    const menuAvances = $('#menuAvances')
    //#endregion

    //#region CONST EVALUACIONES
    const divListadoEvaluaciones = $('#divListadoEvaluaciones')
    const btnFiltroListaEvaluados = $('#btnFiltroListaEvaluados')
    const evaluacion_lblPersonalEvaluado = $('#evaluacion_lblPersonalEvaluado')
    const evaluacion_lblTipoRelacion = $('#evaluacion_lblTipoRelacion')
    const evaluacion_lblCompetencia = $('#evaluacion_lblCompetencia')
    const evaluacion_lblConducta = $('#evaluacion_lblConducta')
    const txtComentarioEvaluacion = $('#txtComentarioEvaluacion')
    const divNombreCriterio = $("#divNombreCriterio")
    const divRadioCriterio = $("#divRadioCriterio")
    const evaluacion_btnRegresar = $("#evaluacion_btnRegresar")
    const evaluacion_btnSiguiente = $("#evaluacion_btnSiguiente")
    const tblRH_Eval360_EvaluacionesEvaluador = $('#tblRH_Eval360_EvaluacionesEvaluador')
    let dtEvaluacion
    //#endregion

    Evaluacion360 = function () {
        (function init() {
            fncListeners()
        })()

        function fncListeners() {
            //#region INIT DATATABLE
            fncGetNivelAcceso()
            fncMostrarListadoEvaluaciones()
            initTblEvaluaciones()
            fncGetEvaluaciones()
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
            btnFiltroBuscar.click(function () {
                fncGetEvaluaciones()
            })

            btnFiltroListaEvaluados.click(function () {
                fncMostrarListadoEvaluaciones()
            })
            //#endregion

            //#region FUNCIONES EVALUACIONES
            evaluacion_btnSiguiente.click(function () {
                fncGuardarRespuestaConducta()
            })
            //#endregion
        }

        //#region FUNCIONES EVALUACIONES
        function initTblEvaluaciones() {
            dtEvaluacion = tblRH_Eval360_EvaluacionesEvaluador.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreCompleto', title: 'Nombre del evaluado' },
                    { data: 'relacion', title: 'Rol de evaluación' },
                    {
                        title: 'Fecha límite para la evaluación',
                        render: function (data, type, row, meta) {
                            return moment(row.fechaLimiteEvaluacion).format("DD/MM/YYYY");
                        },
                    },
                    { data: 'nombrePeriodo', title: 'Periodo', visible: false },
                    { data: 'avance', title: 'Avance' },
                    {
                        title: 'Evaluar',
                        render: function (data, type, row, meta) {
                            if (row.cuestionarioTerminado) {
                                return `<button class='btn btn-xs btn-primary cuestionarioTerminado' title='El cuestionario ya se encuentra finalizado al 100%.'><i class="fas fa-list-ol"></i></button>`
                            } else {
                                return `<button class='btn btn-xs btn-primary comenzarEvaluacion' title='Iniciar evaluación.'><i class="fas fa-list-ol"></i></button>`
                            }
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblRH_Eval360_EvaluacionesEvaluador.on('click', '.comenzarEvaluacion', function () {
                        let rowData = dtEvaluacion.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea comenzar la evaluación?', 'Confirmar', 'Cancelar', () => fncComenzarEvaluacion(
                            rowData.idPersonalEvaluado, rowData.idPersonalEvaluador, rowData.idCuestionario, rowData.nombreCompleto, rowData.id, rowData.relacion
                        ));
                    });

                    tblRH_Eval360_EvaluacionesEvaluador.on('click', '.cuestionarioTerminado', function () {
                        Alert2Warning("El cuestionario ya se encuentra finalizado al 100%");
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function fncGetEvaluaciones() {
            axios.post('GetEvaluaciones').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtEvaluacion.clear();
                    dtEvaluacion.rows.add(response.data.lstEvaluacionesEvaluador);
                    dtEvaluacion.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncComenzarEvaluacion(idPersonalEvaluado, idPersonalEvaluador, idCuestionario, nombreCompleto, idEvaluacion, relacion) {
            if (idPersonalEvaluado > 0 && idPersonalEvaluador > 0 && idCuestionario > 0 && nombreCompleto != "" && idEvaluacion > 0 && relacion != "") {
                fncOcultarListadoEvaluaciones()
                let obj = new Object()
                obj.idEvaluacion = idEvaluacion
                axios.post('GetEvaluacionEvaluadoRelEvaluador', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        txtComentarioEvaluacion.val("")
                        evaluacion_lblPersonalEvaluado.html(nombreCompleto)
                        evaluacion_lblTipoRelacion.html(relacion)
                        evaluacion_lblCompetencia.html(response.data.objConducta.nombreCompetencia)
                        evaluacion_lblConducta.html(response.data.objConducta.descripcionConducta)
                        divNombreCriterio.html(response.data.objNombreCriterios)
                        divRadioCriterio.html(response.data.objRadioCriterios)
                        evaluacion_btnRegresar.attr("disabled", "disabled")
                        evaluacion_btnSiguiente.data().idCuestionario = idCuestionario

                        evaluacion_btnSiguiente.html(response.data.btnSiguienteFinalizar)
                        if (response.data.btnSiguienteFinalizar == "Finalizar") {
                            evaluacion_btnSiguiente.attr("class", "btn btn-success")
                        } else {
                            evaluacion_btnSiguiente.data().idConductaSiguiente = response.data.objConducta.idConductaSiguiente
                            evaluacion_btnSiguiente.attr("class", "btn btn-primary")
                        }

                        if (response.data.mostrarComentario) {
                            txtComentarioEvaluacion.css("display", "block")
                        } else {
                            txtComentarioEvaluacion.css("display", "none")
                        }

                        evaluacion_btnSiguiente.data().idEvaluacion = idEvaluacion
                        evaluacion_btnSiguiente.data().idConducta = response.data.objConducta.idConducta
                        evaluacion_btnSiguiente.data().nombreCompleto = nombreCompleto
                    } else {
                        Alert2Error(message)
                        fncMostrarListadoEvaluaciones()
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("No se encontró ninguna conducta en el cuestionario.")
            }
        }

        function fncGuardarRespuestaConducta() {
            let respuestaConducta = $("input[type=radio][name=name]:checked").val()
            if (respuestaConducta > 0 && respuestaConducta != undefined) {
                let obj = new Object()
                obj.idEvaluacion = evaluacion_btnSiguiente.data().idEvaluacion
                obj.idConducta = evaluacion_btnSiguiente.data().idConducta
                obj.idCriterio = respuestaConducta
                obj.idConductaSiguiente = evaluacion_btnSiguiente.data().idConductaSiguiente
                obj.idCuestionario = evaluacion_btnSiguiente.data().idCuestionario
                obj.comentario = txtComentarioEvaluacion.val()
                axios.post('GuardarRespuestaConducta', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        if (response.data.objConducta != undefined) {
                            evaluacion_lblCompetencia.html(response.data.objConducta.nombreCompetencia)
                            evaluacion_lblConducta.html(response.data.objConducta.descripcionConducta)
                            divNombreCriterio.html(response.data.objNombreCriterios)
                            divRadioCriterio.html(response.data.objRadioCriterios)
                            evaluacion_btnRegresar.attr("disabled", false)
                            evaluacion_btnSiguiente.html(response.data.btnSiguienteFinalizar)
                            evaluacion_btnSiguiente.data().idConducta = response.data.objConducta.idConducta

                            if (response.data.btnSiguienteFinalizar == "Finalizar") {
                                evaluacion_btnSiguiente.attr("class", "btn btn-success")
                            } else {
                                evaluacion_btnSiguiente.data().idConductaSiguiente = response.data.objConducta.idConductaSiguiente
                                evaluacion_btnSiguiente.attr("class", "btn btn-primary")
                            }

                            if (response.data.mostrarComentario) {
                                txtComentarioEvaluacion.css("display", "block")
                            } else {
                                txtComentarioEvaluacion.css("display", "none")
                            }
                        }

                        if (message != null) { Alert2Exito(message) }
                        if (response.data.cuestionarioTerminado) {
                            btnFiltroListaEvaluados.trigger("click")
                        }
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario seleccionar una opción.")
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncMostrarListadoEvaluaciones() {
            fncGetEvaluaciones()
            divListadoEvaluaciones.css("display", "block")
            $(".divEvaluacionEnProceso").css("display", "none")
        }

        function fncOcultarListadoEvaluaciones() {
            divListadoEvaluaciones.css("display", "none")
            $(".divEvaluacionEnProceso").css("display", "block")
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
                        menuEvaluaciones.css("display", "block")
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