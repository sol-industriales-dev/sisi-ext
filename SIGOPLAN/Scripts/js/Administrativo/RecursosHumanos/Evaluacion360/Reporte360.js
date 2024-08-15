(() => {
    $.namespace('CH.Evaluacion360');

    //#region CONST FILTROS
    const cboFiltroPeriodo = $('#cboFiltroPeriodo')
    const btnFiltroBuscar = $('#btnFiltroBuscar')
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

    //#region CONST REPORTE 360
    const tblReporte360 = $('#tblReporte360')
    let dtReporte360;
    let imgGrafica;
    //#endregion

    //#region CONST ESTATUS CUESTIONARIO EVALUADORES
    const mdlListadoEstatusCuestionarioEvaluadores = $('#mdlListadoEstatusCuestionarioEvaluadores')
    const tblEstatusCuestionarioEvaluadores = $('#tblEstatusCuestionarioEvaluadores')
    let dtEstatusCuestionarioEvaluador;
    //#endregion

    Evaluacion360 = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            fncGetNivelAcceso()
            initTblReporte360()
            fncGetEstatusEvaluados()
            initTblEstatusCuestionarioEvaluadores()
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

            //#region FUNCIONES FILTROS
            btnFiltroBuscar.click(function () {
                fncGetEstatusEvaluados()
            })

            $(".select2").select2()
            cboFiltroPeriodo.fillCombo('FillCboPeriodos', null, false, null)
            //#endregion
        }

        //#region FUNCIONES REPORTE 360
        function initTblReporte360() {
            dtReporte360 = tblReporte360.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreCompleto', title: 'Nombre del evaluado' },
                    { data: 'estatusCuestionario', title: 'Estatus cuestionario' },
                    {
                        title: 'Autoevaluación',
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-primary mdlListadoAutoevaluacion'><i class="fas fa-list-ul"></i></button>`;
                        },
                    },
                    {
                        title: 'Jefes',
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-primary mdlListadoJefes'><i class="fas fa-list-ul"></i></button>`;
                        },
                    },
                    {
                        title: 'Clientes Internos',
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-primary mdlListadoClientesInternos'><i class="fas fa-list-ul"></i></button>`;
                        },
                    },
                    {
                        title: 'Colaboradores',
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-primary mdlListadoColaboradores'><i class="fas fa-list-ul"></i></button>`;
                        },
                    },
                    {
                        title: 'Pares',
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-primary mdlListadoPares'><i class="fas fa-list-ul"></i></button>`;
                        },
                    },
                    {
                        title: 'Opciones',
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-danger generarReporte'><i class="fas fa-file-pdf"></i></button>`;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblReporte360.on('click', '.mdlListadoAutoevaluacion', function () {
                        let rowData = dtReporte360.row($(this).closest('tr')).data();
                        let tipoRelacion = 1
                        fncGetEstatusCuestionariosEvaluadores(rowData.id, tipoRelacion);
                    });

                    tblReporte360.on('click', '.mdlListadoJefes', function () {
                        let rowData = dtReporte360.row($(this).closest('tr')).data();
                        let tipoRelacion = 5
                        fncGetEstatusCuestionariosEvaluadores(rowData.id, tipoRelacion);
                    });

                    tblReporte360.on('click', '.mdlListadoClientesInternos', function () {
                        let rowData = dtReporte360.row($(this).closest('tr')).data();
                        let tipoRelacion = 3
                        fncGetEstatusCuestionariosEvaluadores(rowData.id, tipoRelacion);
                    });

                    tblReporte360.on('click', '.mdlListadoColaboradores', function () {
                        let rowData = dtReporte360.row($(this).closest('tr')).data();
                        let tipoRelacion = 4
                        fncGetEstatusCuestionariosEvaluadores(rowData.id, tipoRelacion);
                    });

                    tblReporte360.on('click', '.mdlListadoPares', function () {
                        let rowData = dtReporte360.row($(this).closest('tr')).data();
                        let tipoRelacion = 2
                        fncGetEstatusCuestionariosEvaluadores(rowData.id, tipoRelacion);
                    });

                    tblReporte360.on('click', '.generarReporte', function () {
                        let rowData = dtReporte360.row($(this).closest('tr')).data();
                        fncGetCompetenciasRelEvaluado(rowData.id, rowData.idPersonalEvaluado)
                        Alert2AccionConfirmar('Reporte 360', '¿Desea generar el reporte 360?', 'Confirmar', 'Cancelar',
                            () => fncGenerarReporte360(rowData.id, rowData.idPersonalEvaluado))
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

        function fncGenerarReporte360(idRelacion, idPersonalEvaluado) {
            if (idRelacion > 0 && idPersonalEvaluado > 0) {
                let obj = new Object()
                obj.idRelacion = idRelacion
                obj.idPersonalEvaluado = idPersonalEvaluado
                axios.post("GenerarReporte360", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        var path = `/Reportes/Vista.aspx?idReporte=274&idRelacion=${idRelacion}&idPersonalEvaluado=${idPersonalEvaluado}`;
                        $("#report").attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            openCRModal();
                        };
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al generar el reporte 360.")
            }
        }

        function fncGetEstatusEvaluados() {
            let obj = new Object()
            obj.idPeriodo = cboFiltroPeriodo.val()
            axios.post('GetEstatusEvaluados', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtReporte360.clear();
                    dtReporte360.rows.add(response.data.lstReporteDTO);
                    dtReporte360.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCargarGraficaBarrasPorCompetencias(datos) {
            //#region SE GENERA GRAFICA DE BARRA EN BASE A LA COMPETENCIAS DEL EVALUADO
            let arrConductas = [];
            datos.lstConductas.forEach(element => {
                arrConductas.push(element);
            });

            let arrPromedios = [];
            datos.lstPromedios.forEach(element => {
                arrPromedios.push(element);
            });

            Highcharts.chart('graficaBarrasPorCompetencia', {
                chart: {
                    type: 'bar',
                    width: 1000,
                },
                title: {
                    text: null
                },
                subtitle: {
                },
                xAxis: {
                    categories: arrConductas,
                    title: {
                        text: null
                    },
                    labels: {
                        style: {
                            color: 'black',
                            fontSize: '15px'
                        }
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        align: 'high'
                    },
                    labels: {
                        overflow: 'justify'
                    }
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    fontSize: 20,
                    pointWidth: 20,
                    showInLegend: false,
                    name: "",
                    data: arrPromedios
                }]
            });
            //#endregion

            fncGenerarReporteCrystalReport($('#graficaBarrasPorCompetencia').highcharts(), 'chart');
            fncGenerarReporteCrystalReport($('#graficaBarrasPorCompetencia').highcharts(), 'chart');
            fncGenerarReporteCrystalReport($('#graficaBarrasPorCompetencia').highcharts(), 'chart');
        }

        function fncGetCompetenciasRelEvaluado(idRelacion, idPersonalEvaluado) {
            if (idRelacion > 0 && idPersonalEvaluado > 0) {
                let obj = new Object();
                obj.idRelacion = idRelacion;
                obj.idPersonalEvaluado = idPersonalEvaluado
                axios.post('GetCompetenciasRelEvaluado', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncCargarGraficaBarrasPorCompetencias(response.data.objGraficaDTO);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al pre-generar el reporte 360.");
            }
        }
        //#endregion

        //#region FUNCIONES ESTATUS CUESTIONARIOS EVALUADORES
        function initTblEstatusCuestionarioEvaluadores() {
            dtEstatusCuestionarioEvaluador = tblEstatusCuestionarioEvaluadores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreCompleto', title: 'Nombre del evaluador' },
                    { data: 'estatusCuestionario', title: 'Estatus cuestionario' }
                ],
                initComplete: function (settings, json) {
                    tblEstatusCuestionarioEvaluadores.on('click', '.editarRegistro', function () {
                        let rowData = dtEstatusCuestionarioEvaluador.row($(this).closest('tr')).data();
                        fncGetDatosActualizarCaptura(rowData.id);
                    });
                    tblEstatusCuestionarioEvaluadores.on('click', '.eliminarRegistro', function () {
                        let rowData = dtEstatusCuestionarioEvaluador.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCaptura(rowData.id));
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

        function fncGetEstatusCuestionariosEvaluadores(idRelacion, tipoRelacion) {
            if (idRelacion > 0 && tipoRelacion > 0) {
                let obj = new Object();
                obj.idRelacion = idRelacion
                obj.tipoRelacion = tipoRelacion
                axios.post('GetEstatusCuestionariosEvaluadores', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtEstatusCuestionarioEvaluador.clear();
                        dtEstatusCuestionarioEvaluador.rows.add(response.data.lstReporte360DTO);
                        dtEstatusCuestionarioEvaluador.draw();

                        mdlListadoEstatusCuestionarioEvaluadores.modal("show")
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener la información.")
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
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

        //#region CREACIÓN DE REPORTE CON GRAFICA TOMADA COMO IMAGEN
        EXPORT_WIDTH = 1000;
        function fncGenerarReporteCrystalReport(chart, filename, numGrafica) {
            var render_width = EXPORT_WIDTH;
            var render_height = render_width * chart.chartHeight / chart.chartWidth

            var svg = chart.getSVG({
                exporting: {
                    sourceWidth: chart.chartWidth,
                    sourceHeight: chart.chartHeight
                }
            });

            var canvas = document.createElement('canvas');
            canvas.height = render_height;
            canvas.width = render_width;

            var image = new Image;
            image.onload = function () {
                canvas.getContext('2d').drawImage(this, 0, 0, render_width, render_height);
                var data = canvas.toDataURL("image/png")
                download(data, filename + '.png');
            };
            // image.src = 'data:image/svg+xml;base64,' + window.btoa(svg);

            var imgsrc = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(svg)));
            // var img = new Image(1, 1); // width, height values are optional params 
            image.src = imgsrc;
        }

        function download(data, filename, numGrafica) {
            var a = document.createElement('a');
            document.body.appendChild(a);
            let obj = new Object();
            obj.grafica = data;
            axios.post("CargarVariableSesion", obj).then(response => {
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