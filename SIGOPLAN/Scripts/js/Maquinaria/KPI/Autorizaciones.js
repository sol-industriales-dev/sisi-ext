(() => {
    $.namespace('Maquinaria.KPI.Autorizacion');
    Autorizacion = function () {
        let busq, dtAuth, EnTurno, colExcel = [];
        var formatosID = 0;
        const report = $('#report');
        const mdlPanelAuth = $('#mdlPanelAuth');
        const divAuthPanel = $('#divAuthPanel');
        const lblAuthMessage = $('#lblAuthMessage');
        const divAutorizantes = $('#divAutorizantes');
        const cboAC = $('#cboAC');
        const tblAuth = $('#tblAuth');
        const cboEstado = $('#cboEstado');
        const btnBuscar = $('#btnBuscar');
        const cboPeriodo = $('#cboPeriodo');
        const AuthCargar = originURL('/KPI/AuthCargar');
        const AuthAprueba = originURL('/KPI/AuthAprueba');
        const AuthRechaza = originURL('/KPI/AuthRechaza');
        const CargarCaptura = originURL('/KPI/CargarCaptura');
        const CargarAutorizantes = originURL('/KPI/CargarAutorizantes');
        const CargarComboxAutorizantes = originURL('/KPI/CargarComboxAutorizantes');
        const CargarPendientes = originURL('/KPI/CargarPendientes');
        // const graficaDisponibilidadUtilizacion_modeloDiario = $('#graficaDisponibilidadUtilizacion_modeloDiario');
        // const graficaDisponibilidadUtilizacion_modeloSemanal = $('#graficaDisponibilidadUtilizacion_modeloSemanal');
        // const graficaDisponibilidadUtilizacion_modeloMensual = $('#graficaDisponibilidadUtilizacion_modeloMensual');

        //VARIABLES PARA IMAGENES
        let imgGraficaDiario;
        let imgGraficaSemanal;
        let imgGraficaMensual;

        let fechaAuthReporte = null;
        let areaCuentaAuthReporte = null;

        let countGraficas = 0;

        let usuarioAutorizando = 0;
        objPanelAuth = {
            idPanelReporte: 11,
            urlAuth: AuthAprueba,
            urlRech: AuthRechaza,
            urlLstAuth: AuthCargar,
            callbackAuth: null,
            callbackRech: null
        };
        (() => {
            InitForm();
            GeneraArregloColumnas();
            setIframeResolution();
            initDataTblAuth();
            btnBuscar.click(setAutorizantes);

        })();

        function loadPendientes() {
            dtAuth.clear().draw();
            GetBusqForm();
            axios.post(CargarPendientes, { busq })
                .then(response => {
                    let { success, lst } = response.data;
                    if (success) {
                        dtAuth.rows.add(lst).draw();
                    }
                }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }

        //#region Solicitudes
        function setAutorizantes() {
            dtAuth.clear().draw();
            GetBusqForm();
            axios.post(CargarAutorizantes, { busq })
                .then(response => {
                    let { success, lst } = response.data;
                    if (success) {
                        dtAuth.rows.add(lst).draw();
                    }
                }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }
        //#endregion
        //#region Formulario
        function InitForm() {
            axios.get('/Usuarios/GetUsuarioActivo').then(response => {
                usuarioAutorizando = response.data.usuarioActivo;
            });

            axios.get(CargarComboxAutorizantes)
                .then(response => {
                    let { success, itemsAuth, itemsPeriodo, itemsAreaCuenta } = response.data;
                    if (success) {
                        cboEstado.fillComboItems(itemsAuth, undefined);
                        cboPeriodo.fillComboItems(itemsPeriodo, "TODOS");
                        cboAC.fillComboItems(itemsAreaCuenta, "TODOS");
                        CargarPanelDeseAlerta();
                    }
                }).catch(o_O => AlertaGeneral('Aviso', o_O.message))
                .then(function (response) {
                    loadPendientes();
                });
        }

        function CargarPanelDeseAlerta() {
            let url_string = window.location.href,
                url = new URL(url_string),
                idAuth = +url.searchParams.get("idAuth");
            if (idAuth > 0) {
                setPanelAutorizantes({ id: idAuth });
            }
        }
        function GetBusqForm() {
            let periodo = GetPeriodoSeleccionado();
            busq = {
                ac: cboAC.val(),
                estatus: +cboEstado.val(),
                año: periodo.año,
                semana: periodo.semana,
                min: periodo.min,
                max: periodo.max
            }
        }
        function GetPeriodoSeleccionado() {
            if (cboPeriodo.val() != 'TODOS') {
                let prefijo = cboPeriodo.find("option:selected").data() == null ? '' : cboPeriodo.find("option:selected").data().prefijo,
                    periodo = JSON.parse(prefijo);
                periodo.max = new Date(periodo.max);
                periodo.min = new Date(periodo.min);
                return periodo;
            }
            else {
                var periodo = {};
                periodo.año = 0;
                periodo.semana = 0;
                periodo.max = new Date();
                periodo.min = new Date();
                return periodo;
            }
        }
        function initDataTblAuth() {
            dtAuth = tblAuth.DataTable({
                destroy: true,
                language: dtDicEsp,
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Reporte de Autorizaciones KPI Homologado", "<center><h3>Reporte de Autorizaciones KPI Homologado</h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true
                    }
                ],
                columns: [
                    { data: 'Folio', title: 'Folio' },
                    { data: 'Descripcion', title: 'Area-Cuenta' },
                    { data: 'Periodo', title: 'Periodo' },
                    { data: 'Estatus', title: 'Estatus' },
                    {
                        data: 'Id', title: 'Reporte', createdCell: function (td, data, rowData, row, col) {
                            let btn = $(`<button>`, {
                                text: 'Reporte',
                                class: 'btn btn-default text-center reporte',
                            });
                            $(td).html(btn);
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblAuth.on('click', '.reporte', function (event) {
                        let row = $(this).closest("td"),
                            auth = dtAuth.row(row).data();
                        setPanelAutorizantes({ id: auth.Id, periodo: auth.Periodo + "         Obra:" + auth.Descripcion + "        Folio:" + auth.Folio, areaCuenta: auth.AC, descAreaCuenta: auth.Descripcion, fechaInicio: auth.Periodo });
                    });
                }
            });
        }
        //#endregion
        //#region Autorizantes
        function setIframeResolution() {
            let height = screen.height;
            if (height > 769) {
                report.css("height", "66.3em");
            } else {
                report.css("height", "43em");
            }
        }
        function setPanelAutorizantes(objLstAuth) {
            lblAuthMessage.text("");
            report.contents().find("body").html("<center><h3 style='color:white;font-weight:bold;'><br/><br/>Cargando información...</h3></center>");
            report.contents().find('body').css('backgroundColor', 'rgb(142, 142, 142)');
            axios.post(objPanelAuth.urlLstAuth, objLstAuth)
                .then(response => {
                    let { success } = response.data;
                    if (success) {
                        createPanelAuth(response.data, objLstAuth.fechaInicio, objLstAuth.areaCuenta, objLstAuth.descAreaCuenta);
                        formatosID = objLstAuth.id;
                        setPanelReporte(objLstAuth);
                        mdlPanelAuth.modal(`show`);
                    }
                }).catch(o_O => console.log(o_O.message));
        }/*

        setPanelAutorizantes = (objLstAuth) => {
      }*/

        function setPanelReporte(auth) {
            axios.get(CargarCaptura.href, { params: { idCaptura: auth.id } })
                .then(response => {
                    if (response.data.success) {
                        report.attr("src", `/Reportes/Vista.aspx?idReporte=206&inMemory=1&pModulo=2&pFechaPeriodo=${auth.periodo}`);
                        document.getElementById('report').onload = () => {
                        }
                    }
                }).catch(o_O => AlertaGeneral(o_O.message));
        }

        function setAuth() {
            axios.post(objPanelAuth.urlAuth, EnTurno)
                .then(response => {
                    if (response.data.success) {
                        report.attr("src", `/Reportes/Vista.aspx?idReporte=206&inMemory=1&pModulo=2&pFechaPeriodo=${EnTurno.Periodo}`);
                        document.getElementById('report').onload = () => {
                            if (objPanelAuth.callbackAuth) {
                                objPanelAuth.callbackAuth(response.data);
                            }
                            mdlPanelAuth.modal(`hide`);
                            AlertaGeneral("Aviso", "Autorización firmada con éxito");

                            $('#divGraficaDiario').hide();
                            $('#divGraficaSemanal').hide();
                            $('#divGraficaMensual').hide();
                        }
                    } else {
                        AlertaGeneral("Aviso", response.data.message);
                    }
                }).catch(o_O => console.log(o_O.message));
        }

        function setRech() {
            AlertaAceptarRechazar("Aviso", `<p>¿Cúal es el motivo de rechazo?</p><textarea rows="4" cols="70" class="form-control comentarioRechazo"></textarea>`, objPanelAuth.urlRech, null)
                .then(btn => {
                    EnTurno.comentario = $(".comentarioRechazo").val();
                    axios.post(objPanelAuth.urlRech, EnTurno).then(response => {
                        if (response.data.success) {
                            report.attr("src", `/Reportes/Vista.aspx?idReporte=${objPanelAuth.idPanelReporte}&fId=${formatosID}&inMemory=1&pModulo=2&pFechaPeriodo=1`);
                            document.getElementById('report').onload = () => {
                                if (objPanelAuth.callbackRech) {
                                    objPanelAuth.callbackRech(response.data);
                                }
                                mdlPanelAuth.modal(`hide`);
                                AlertaGeneral("Aviso", "Autorización rechazada con éxito");
                            }
                        } else {
                            AlertaGeneral("Aviso", response.data.message);
                        }

                    }).catch(o_O => console.log(o_O.message));
                })
        }

        function createPanelAuth({ autorizantes, message }, fechaInicio, areaCuenta, descAreaCuenta) {
            EnTurno = null;
            divAutorizantes.html(``);
            //FECHA A AUTORIZAR
            fechaAuthReporte = fechaInicio;
            areaCuentaAuthReporte = areaCuenta;

            autorizantes.forEach(auth => {
                let panel = $("<div>");
                let encabezado = $("<div>");
                let cuerpo = $(`<div>`);
                let pie = $(`<div>`);
                let color = ColorDesdeEstado(auth);
                panel.data(auth);
                panel.addClass("panel panel-default text-center");
                encabezado.addClass(`panel-heading ${color}`);
                cuerpo.addClass(`panel-body`);
                pie.addClass(`panel-footer ${auth.clase} ${color}`);
                encabezado.text(auth.nombre);
                cuerpo.text(auth.descripcion);
                pie.text(auth.clase);
                if (auth.authEstado === 3) {
                    if (usuarioAutorizando == auth.idAuth) {
                        auth.descAreaCuenta = descAreaCuenta;
                        auth.fechaInicio = fechaInicio;
                        auth.areaCuenta = areaCuenta;
                        EnTurno = auth;
                        let btnAuth = $(`<button id="btnAutorizarKPI">`);
                        let btnRech = $(`<button id="btnRechazarKPI">`);
                        btnAuth.addClass(`btn btn-success btn-xs pull-right btnAuth`);
                        btnRech.addClass(`btn btn-danger btn-xs pull-left btnRech`);
                        btnAuth.html(`<i class="fa fa-check"></i>`);
                        btnRech.html(`<i class="fas fa-times"></i>`);
                        pie.text(`Autorice`);
                        btnAuth.click(setAuth);
                        // btnAuth.click(fncLoadGraficas);
                        btnRech.click(setRech);
                        pie.append(btnAuth);
                        pie.append(btnRech);
                    }
                }
                panel.append(encabezado);
                panel.append(cuerpo);
                panel.append(pie);
                divAutorizantes.append(panel);
            });
            if (message !== null) {
                lblAuthMessage.text(message);
            }
        }

        function ColorDesdeEstado({ authEstado }) {
            switch (authEstado) {
                case 0:
                    return "Espera";
                case 1:
                    return "Autorizado";
                case 2:
                    return "Rechazado";
                case 3:
                    return "AutorizanteEnTurno";
                default:
                    return "";
            }
        }
        function InyectarPanel({ lst, maxCol, maxRow }) {
            let tbl = $(`<table>`, {
                class: "tblPanel table"
            }),
                rowSpanciones = [],
                colSpanciones = [];
            for (let iRow = 1; iRow < +maxRow + 1; iRow++) {
                let tr = $(`<tr>`);
                tr.attr('data-row', iRow);
                for (let iCol = 0; iCol < colExcel.length; iCol++) {
                    const cCol = colExcel[iCol];
                    let celda = lst.find(data => data.col === cCol && data.row === iRow.toString());
                    if (celda === undefined) {
                        celda = CeldaDefault(iRow, cCol);
                    }
                    let td = $(`<td>`, {
                        text: celda.valor,
                        class: celda.clase,
                        style: `background-color: rgba(${celda.color.red}, ${celda.color.green}, ${celda.color.blue}, ${celda.color.aplha})`
                    });
                    td.attr('data-col', cCol);

                    tr.append(td);
                    if (cCol === maxCol) {
                        break;
                    }
                }
                tbl.append(tr);
            }
            report.empty();
            report.append(tbl);
        }
        function GeneraArregloColumnas() {
            var i1, i2, i3;
            for (i1 = 0; i1 < 26; i1++) {
                colExcel.push(String.fromCharCode(65 + i1));
            }
            for (i1 = 0; i1 < 26; i1++) {
                for (i2 = 0; i2 < 26; i2++) {
                    colExcel.push(String.fromCharCode(65 + i1) + String.fromCharCode(65 + i2));
                }
            }
            for (i1 = 0; i1 < 26; i1++) {
                for (i2 = 0; i2 < 26; i2++) {
                    for (let i3 = 0; i3 < 26; i3++) {
                        colExcel.push(String.fromCharCode(65 + i1) + String.fromCharCode(65 + i2) + String.fromCharCode(65 + i3));
                    }
                }
            }
        }

        function CeldaDefault(row, col) {
            let celda = {
                valor: "",
                row: row,
                col: col,
                colSpan: 1,
                rowSpan: 1,
                clase: "",
                color: {
                    aplha: 0,
                    red: 0,
                    blue: 0,
                    green: 0
                }
            };
            return celda;
        }
        //#endregion

        //#region GRAFICAS O METODO DEFAULT
        function fncLoadGraficas() {
            countGraficas = 0;

            if (EnTurno.orden == 4) {
                if (fechaAuthReporte != null) {
                    filtro = {
                        areaCuenta: areaCuentaAuthReporte,
                        fechaInicio: fechaAuthReporte,
                    }

                    axios.post("GetInfoGraficasPDF", filtro).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {

                            EnTurno.descPeriodoDia = response.data.descPeriodoDia;
                            EnTurno.descPeriodoSemana = response.data.descPeriodoSemana;
                            EnTurno.descPeriodoMes = response.data.descPeriodoMes;

                            fncCargarGraficaDisponibilidadUtilizacion_modeloDiario(response.data.gpx_disVsUti_modeloDiario);
                            fncCargarGraficaDisponibilidadUtilizacion_modeloSemanal(response.data.gpx_disVsUti_modeloSemanal);
                            fncCargarGraficaDisponibilidadUtilizacion_modeloMensual(response.data.gpx_disVsUti_modeloMensual);

                            $('#divGraficaDiario').show();
                            $('#divGraficaSemanal').show();
                            $('#divGraficaMensual').show();
                            $('#graficaDisponibilidadUtilizacion_modeloDiario').highcharts().reflow();
                            $('#graficaDisponibilidadUtilizacion_modeloSemanal').highcharts().reflow();
                            $('#graficaDisponibilidadUtilizacion_modeloMensual').highcharts().reflow();

                        }
                    }).catch(error => Alert2Error(error.message));
                }
            } else {
                countGraficas = 3;
                setAuth(countGraficas);
            }

        }

        function fncCargarGraficaDisponibilidadUtilizacion_modeloDiario(datos) {
            Highcharts.chart('graficaDisponibilidadUtilizacion_modeloDiario', {
                chart: {
                    type: 'column',
                    backgroundColor: '#FFFFFF',
                },
                title: {
                    text: null,
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {

                    column: {
                        dataLabels: {
                            enabled: true,
                            format: '{y} %'
                        },
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1

                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            }, function (chart) {
                console.log("inside callback START");
                if (!chart.userOptions.chart.forExport) {
                    fncGenerarReporteCrystalReport($('#graficaDisponibilidadUtilizacion_modeloDiario').highcharts(), 1, setAuth);
                }
                console.log("inside callback END");
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaDisponibilidadUtilizacion_modeloSemanal(datos) {
            Highcharts.chart('graficaDisponibilidadUtilizacion_modeloSemanal', {
                chart: {
                    type: 'column',
                    backgroundColor: '#FFFFFF',

                },
                title: {
                    text: null,
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        dataLabels: {
                            enabled: true,
                            format: '{y} %'
                        },
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1

                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            }, function (chart) {
                console.log("inside callback START");
                if (!chart.userOptions.chart.forExport) {
                    fncGenerarReporteCrystalReport($('#graficaDisponibilidadUtilizacion_modeloSemanal').highcharts(), 2, setAuth);
                }
                console.log("inside callback END");
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncCargarGraficaDisponibilidadUtilizacion_modeloMensual(datos) {
            Highcharts.chart('graficaDisponibilidadUtilizacion_modeloMensual', {
                chart: {
                    type: 'column',
                    backgroundColor: '#FFFFFF',

                },
                title: {
                    text: null,
                },
                // subtitle: {
                //     text: 'Source: WorldClimate.com'
                // },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        format: '{value}%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        dataLabels: {
                            enabled: true,
                            format: '{y} %'
                        },
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [{
                    name: datos.serie1Descripcion,
                    data: datos.serie1

                }, {
                    name: datos.serie2Descripcion,
                    data: datos.serie2
                }],
                credits: {
                    enabled: false
                }
            }, function (chart) {
                console.log("inside callback START");
                if (!chart.userOptions.chart.forExport) {
                    fncGenerarReporteCrystalReport($('#graficaDisponibilidadUtilizacion_modeloMensual').highcharts(), 3, setAuth);
                }
                console.log("inside callback END");
            });
            $('.highcharts-title').css("display", "none");
        }

        function fncGenerarReporteCrystalReport(chart, numGrafica, setAuthCallback) {

            EXPORT_WIDTH = 1000;
            if (chart == undefined) {
                return "";
            }

            var render_width = EXPORT_WIDTH;
            var render_height = render_width * chart.chartHeight / chart.chartWidth

            var svg = chart.getSVG({
                exporting: {
                    sourceWidth: chart.chartWidth,
                    sourceHeight: chart.chartHeight
                }
            });

            // svg.style('fill', 'white'); 

            var canvas = document.createElement('canvas');
            canvas.height = render_height;
            canvas.width = render_width;

            var image = new Image;
            image.onload = function () {
                canvas.getContext('2d').drawImage(this, 0, 0, render_width, render_height);
                var data = canvas.toDataURL("image/png");

                switch (numGrafica) {
                    case 1:
                        imgGraficaDiario = data;
                        break;
                    case 2:
                        imgGraficaSemanal = data;
                        break;
                    case 3:
                        imgGraficaMensual = data;
                        break;
                    default:
                        break;
                }
                countGraficas++;

                setAuthCallback(countGraficas);
                // download(data, filename + '.png', numGrafica);
            };
            // image.src = 'data:image/svg+xml;base64,' + window.btoa(svg);

            var imgsrc = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(svg)));
            // // var img = new Image(1, 1); // width, height values are optional params 
            image.src = imgsrc;

        }
        //#endregion

    }
    $(document).ready(() => {
        Maquinaria.KPI.Autorizacion = new Autorizacion();
    });
})();