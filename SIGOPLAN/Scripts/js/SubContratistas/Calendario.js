(() => {
    $.namespace('Subcontratistas.Calendario');
    //#region CONST
    const cboFiltroCC = $('#cboFiltroCC');
    const cboFiltroSubcontratista = $('#cboFiltroSubcontratista');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroEditarFechas = $('#btnFiltroEditarFechas');
    const mdlActualizarFechaEvaluacion = $('#mdlActualizarFechaEvaluacion');
    const btnCEActualizarFechaEvaluacion = $('#btnCEActualizarFechaEvaluacion');

    const txtCEFechaEvaluacion = $('#txtCEFechaEvaluacion');
    const txtCENombreEvaluacion = $('#txtCENombreEvaluacion');
    const lstProximosEventos = $('#lstProximosEventos');
    // const btnFiltroImprimir = $('#btnFiltroImprimir');
    const btnFiltroExportar = $('#btnFiltroExportar');
    const btnFiltroLimpiar = $('#btnFiltroLimpiar');

    let calendar;
    let imgCalendario;
    //#endregion

    let ccEvaluacion = "";

    Calendario = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initCalendar();
            llenarCombos();
            // cargarProximosEventos();

            btnFiltroBuscar.on("click", function () {
                if (getValoresMultiples("#cboFiltroSubcontratista").length >= 1) {
                    getEventosCalendario();
                } else {
                    Alert2Warning("Seleccione almenos un subcontratista");
                }
            });

            cboFiltroSubcontratista.on("change", function () {
                calendar.setDataSource([]);
                ccEvaluacion = "";

                if (getValoresMultiples("#cboFiltroSubcontratista").length == 1) {
                    btnFiltroExportar.prop("disabled", false);
                } else {
                    btnFiltroExportar.prop("disabled", true);
                }
            });

            cboFiltroCC.on("change", function () {
                calendar.setDataSource([]);
            });

            btnFiltroExportar.on("click", function () {
                fncGenerarReporteCrystalReport($("#calendar"), 'calendario');
            });

            btnFiltroLimpiar.on("click", function () {
                // $('#cboFiltroCC').multiSelect('deselect_all');
                cboFiltroCC.val('');
                cboFiltroCC.trigger("change");
            });
        }

        function llenarCombos() {
            cboFiltroCC.fillCombo('FillComboProyectosUnicos', null, false, 'Todos');
            convertToMultiselect("#cboFiltroCC");
            cboFiltroSubcontratista.fillCombo('FillComboSubcontratistasSorted', null, false, "Todos");
            convertToMultiselect("#cboFiltroSubcontratista");
        }

        function getEventosCalendario() {
            axios.post('llenarCalendarioEvaluaciones', { lstFiltroCC: getValoresMultiples("#cboFiltroCC"), lstFiltroSubC: getValoresMultiples("#cboFiltroSubcontratista") }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    calendar.setDataSource([]);

                    if (items.length > 0) {

                        let regionEventLog = $('#events-log');
                        regionEventLog.html("<h4>Próximos Eventos</h4>");

                        for (var a of items) {
                            ccEvaluacion = items.centroCosto;

                            if (moment(a.fecha).format("YYYY-MM-DD") > moment().format("YYYY-MM-DD")) {
                                let logElt = [];
                                logElt = document.createElement('div');
                                logElt.textContent = moment(a.fecha).format("DD/MM/YYYY") + " " + a.SubContratista;
                                document.querySelector('#events-log').appendChild(logElt);
                            }

                            let colorEvent = colorearFecha(a.fecha);

                            var event = {
                                id: 0,
                                name: a.SubContratista,
                                // location: $('#event-modal input[name="event-location"]').val(),
                                startDate: moment(a.fecha)._d,
                                endDate: moment(a.fecha)._d,
                                color: colorearFecha(a.fecha),
                                details: a.SubContratista,
                                esTooltip: true
                            }

                            var event2 = {
                                id: 0,
                                name: a.SubContratista,
                                // location: $('#event-modal input[name="event-location"]').val(),
                                startDate: moment(a.fecha).add(1, "d"),
                                endDate: moment(a.fecha).add(1, "d"),
                                color: colorEvent,
                                details: a.SubContratista,
                                esTooltip: false
                            }

                            var event3 = {
                                id: 0,
                                name: a.SubContratista,
                                // location: $('#event-modal input[name="event-location"]').val(),
                                startDate: moment(a.fecha).add(2, "d"),
                                endDate: moment(a.fecha).add(2, "d"),
                                color: colorEvent,
                                details: a.SubContratista,
                                esTooltip: false
                            }

                            var dataSource = calendar.getDataSource();

                            if (event.id) {
                                for (var i in dataSource) {
                                    if (dataSource[i].id == event.id) {
                                        dataSource[i].name = event.name;
                                        dataSource[i].location = event.location;
                                        dataSource[i].startDate = event.startDate;
                                        dataSource[i].endDate = event.endDate;
                                    }
                                }
                            }
                            else {
                                var newId = 0;

                                for (var i in dataSource) {
                                    if (dataSource[i].id > newId) {
                                        newId = dataSource[i].id;
                                    }
                                }

                                newId++;
                                event.id = newId;

                                event2.id = newId + 1;
                                event3.id = newId + 1;

                                dataSource.push(event);

                                if (getValoresMultiples("#cboFiltroSubcontratista").length == 1) {
                                    dataSource.push(event2);
                                    dataSource.push(event3);
                                }
                            }
                            calendar.setDataSource(dataSource);
                            // $('#event-modal').modal('hide');
                        }
                    }
                } else {
                    calendar.setDataSource([]);
                }

            }).catch(error => Alert2Error(error.message));;
        }

        function shadeHexColor(color, percent) {
            var f = parseInt(color.slice(1), 16), t = percent < 0 ? 0 : 255, p = percent < 0 ? percent * -1 : percent, R = f >> 16, G = f >> 8 & 0x00FF, B = f & 0x0000FF;
            return "#" + (0x1000000 + (Math.round((t - R) * p) + R) * 0x10000 + (Math.round((t - G) * p) + G) * 0x100 + (Math.round((t - B) * p) + B)).toString(16).slice(1);
        }

        function colorearFecha(items) {
            var colorEvento = "";

            if (getValoresMultiples("#cboFiltroSubcontratista").length != 1) {
                colorEvento = "#E9960D" // Amarillo;

            } else {
                if (getValoresMultiples("#cboFiltroCC").length == 1) {
                    colorEvento = "#E9960D" // Amarillo;

                } else {
                    if (moment(items).format('YYYY-MM-DD') >= moment().format('YYYY-MM-DD')) {
                        colorEvento = '#154A9C'; //azul
                    } else {
                        colorEvento = '#37A82A'; // verde
                    }
                }

            }
            return colorEvento;
        }

        function initCalendar() {
            let tooltip = null;

            calendar = new Calendar('#calendar', {
                language: 'es',
                style: 'custom',
                enableContextMenu: true,
                // customDayRenderer: function (element, date) {
                //     if (date.endDate==1) {
                //         $(element).css('background-color', '#37A82A');
                //     }
                // },
                customDataSourceRenderer: function (element, date, event) {
                    // This will override the background-color to the event's color
                    for (var item of event) {
                        if (item.esTooltip) {
                            $(element).css('background-color', item.color);
                        } else {
                            $(element).css('background-color', shadeHexColor(item.color, .5));

                        }
                    }

                },

                mouseOnDay: function (e) {
                    if (e.events.length > 0) {
                        var content = '';

                        // console.log(e.events)

                        for (var i in e.events) {
                            if (e.events[i].esTooltip) {
                                content += '<div class="event-tooltip-content">'
                                    + '<div class="event-details"><i class="fas fa-square-full" style="color:' + e.events[i].color + '"></i>&nbsp;&nbsp;' + e.events[i].details + '</div>'
                                    + '</div>';
                            }

                        }

                        if (tooltip !== null) {
                            tooltip.destroy();
                            tooltip = null;
                        }

                        tooltip = tippy(e.element, {
                            placement: 'right',
                            content: content,
                            animateFill: false,
                            animation: 'shift-away',
                            arrow: true
                        });
                        tooltip.show();
                    }
                },
                mouseOutDay: function () {
                    if (tooltip !== null) {
                        tooltip.destroy();
                        tooltip = null;
                    }
                }
            });


        }

        //#region CREACIÓN DE REPORTE CON GRAFICA TOMADA COMO IMAGEN

        function filter(node) {
            return (node.tagName !== 'i');
        }

        EXPORT_WIDTH = 1000;
        function fncGenerarReporteCrystalReport(chart, filename) {
            var render_width = EXPORT_WIDTH;
            var render_height = render_width * chart.chartHeight / chart.chartWidth

            // var svg = chart.getSVG({
            //     exporting: {
            //         sourceWidth: chart.chartWidth,
            //         sourceHeight: chart.chartHeight
            //     }
            // });
            let domImgSrc;

            var node = document.getElementById('calendar');

            domtoimage
                .toPng(node)
                .then(function (dataUrl) {
                    var img = new Image();
                    domImgSrc = dataUrl;
                    img.src = dataUrl;
                    // document.body.appendChild(img);
                    // console.log(imgCalendario);

                    img.onload = function () {
                        imgCalendario = dataUrl;
                        fncSaveImgSession();
                        // Alert2AccionConfirmar('', '¿Desea generar el reporte?', 'Confirmar', 'Cancelar', () => fncSaveImgSession());
                    };
                })
                .catch(function (error) {
                    console.error("oops, something went wrong!", error);
                });
        }

        function download(data, filename) {
            var a = document.createElement('a');
            document.body.appendChild(a);
            imgCalendario = data;
        }

        function fncSaveImgSession() {
            axios.post("SaveImgSession", { img: imgCalendario }).then(response => {
                let { success, items, message } = response.data;
                if (success) {


                    const report = $("#report");
                    report.attr("src", `/Reportes/Vista.aspx?idReporte=279&cc=${$("#cboFiltroCC option:selected").text()}&nombreSubC=${$("#cboFiltroSubcontratista option:selected").text()}`);
                    document.getElementById('report').onload = function () {
                        openCRModal();
                        $.unblockUI();
                    };
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

    }
    $(document).ready(() => {
        Subcontratistas.Calendario = new Calendario();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();