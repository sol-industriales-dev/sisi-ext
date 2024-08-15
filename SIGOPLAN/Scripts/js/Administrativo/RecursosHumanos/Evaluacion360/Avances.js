(() => {
    $.namespace('CH.Evaluacion360');

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

    //#region CONST FILTROS
    const cboFiltroPeriodo = $('#cboFiltroPeriodo')
    const btnFiltroBuscar = $('#btnFiltroBuscar')
    const btnFiltroEnviarCorreo = $('#btnFiltroEnviarCorreo')
    //#endregion

    //#region CONST DATATABLE
    const tblEstatusEvaluadores = $('#tblEstatusEvaluadores')
    let dtEstatusEvaluadores;
    //#endregion

    Evaluacion360 = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLE
            fncGetNivelAcceso()
            initTblEstatusEvaluadores()
            initGraficaEstatusCuestionarios()
            //#endregion

            //#region FUNCIONES FILTROS
            $(".select2").select2();
            cboFiltroPeriodo.fillCombo('FillCboPeriodos', null, false, null)

            btnFiltroBuscar.click(function () {
                fncGetEstatusEvaluadores()
            })

            btnFiltroEnviarCorreo.click(function () {
                //#region SE OBTIENE LA CLAVE DEL EMPLEADO SELECCIONADO
                let strMensaje = "";
                let arrRowsChecked = [];
                let rowsChecked = tblEstatusEvaluadores.DataTable().column(0).checkboxes.selected();
                $.each(rowsChecked, function (index, idPersonalEvaluador) {
                    arrRowsChecked.push(idPersonalEvaluador);
                    strMensaje == "" ? "¿Desea notificar a los empleados seleccionados?" : "¿Desea notificar al empleado seleccionado?";
                });
                //#endregion

                Alert2AccionConfirmar('¡Cuidado!', "Desea notificar al empleado seleccionado", 'Confirmar', 'Cancelar', () => fncEnviarCorreo(arrRowsChecked))
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
        }

        //#region FUNCIONES ESTATUS EVALUADORES
        function initTblEstatusEvaluadores() {
            dtEstatusEvaluadores = tblEstatusEvaluadores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'idPersonalEvaluador' },
                    { data: 'numRegistro', title: '#' },
                    { data: 'nombreEvaluador', title: 'Nombre del evaluador' },
                    { data: 'nombreEvaluado', title: 'Nombre del evaluado' },
                    { data: 'descripcionTipoRelacion', title: 'Relación' },
                    { data: 'nombreCuestionario', title: 'Nombre cuestionario' },
                    { data: 'estatusAvance', title: 'Avance' }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    {
                        targets: 0,
                        checkboxes: {
                            selectRow: true
                        }
                    },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
                select: {
                    style: 'multi'
                }
            });
        }

        function fncGetEstatusEvaluadores() {
            fncDefaultCtrls("select2-cboFiltroPeriodo-container")
            if (cboFiltroPeriodo.val() > 0) {
                let obj = new Object()
                obj.idPeriodo = cboFiltroPeriodo.val()
                axios.post('GetEstatusEvaluadores', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtEstatusEvaluadores.clear()
                        dtEstatusEvaluadores.rows.add(response.data.lstEvaluadoresDTO)
                        dtEstatusEvaluadores.draw()

                        initGraficaEstatusCuestionarios(response.data.objEstatusGrafica)
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (cboFiltroPeriodo.val() <= 0) {
                    $("#select2-cboFiltroPeriodo-container").css('border', '2px solid red');
                    Alert2Warning("Es necesario seleccionar un periodo.")
                }
            }
        }

        function initGraficaEstatusCuestionarios(datos) {
            if (datos != null && datos != undefined) {
                let arr = new Array();

                // NO INICIADAS
                let obj = new Object();
                obj.name = "No iniciadas"
                obj.y = datos.cantNoIniciadas
                obj.color = "red"
                arr.push(obj)

                // EN PROCESO
                obj = new Object();
                obj.name = "En proceso"
                obj.y = datos.cantEnProceso
                obj.color = "orange"
                arr.push(obj)

                // CONTESTADAS
                obj = new Object();
                obj.name = "Contestadas"
                obj.y = datos.cantContestadas
                obj.color = "green"
                arr.push(obj)

                console.log(arr);

                Highcharts.chart('graficaEstatusCuestionarios', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'Avances por concepto'
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                connectorColor: 'silver'
                            }
                        }
                    },
                    series: [{
                        name: 'Avance',
                        data: arr
                    }]
                });
            }
        }

        function fncEnviarCorreo(lstPersonalEvaluadorID) {
            if (lstPersonalEvaluadorID != null && cboFiltroPeriodo.val() > 0) {
                let obj = new Object();
                obj.lstPersonalEvaluadorID = lstPersonalEvaluadorID
                obj.idPeriodo = cboFiltroPeriodo.val()
                axios.post('EnviarCorreo', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (cboFiltroPeriodo.val() <= 0) {
                    Alert2Warning("Es necesario seleccionar un periodo.")
                } else {
                    Alert2Error("Ocurrió un error al enviar el correo.");
                }
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
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