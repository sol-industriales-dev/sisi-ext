(() => {
    $.namespace('Administrativo.CincoS');

    //#region CONSTS FILTRO
    const cboFiltroCC = $('#cboFiltroCC');
    const cboFiltroAño = $('#cboFiltroAño');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroExportar = $('#btnFiltroExportar');
    //#endregion

    //#region CONSTS 
    const calendar = $('#calendar');

    let calendarHidden;
    let imgCalendario;

    //#endregion

    //#region ENUM
    const consultaCCEnum = {
        Todos: 0,
        TodosLosActivos: 1,
        TodosConCheckListCreado: 2,
        LosDelCheckList: 3
    };

    //#endregion

    CincoS = function () {
        (function init() {
            fncListeners();

            cboFiltroCC.fillCombo('GetCCs', { consulta: consultaCCEnum.TodosConCheckListCreado, checkListId: null }, false, null, () => {
                cboFiltroCC.attr('multiple', 'multiple');
                cboFiltroCC.select2();
                cboFiltroCC.find('option:eq(0)').remove();
                cboFiltroCC.trigger("change");
            });
            calendarHidden = new Calendar('#calendarHidden', {
                language: 'es',
                style: 'custom',
                customDataSourceRenderer: function (element, date, event) {
                    // This will override the background-color to the event's color
                    for (var item of event) {
                        if (item.esTooltip) {
                            $(element).css('background-color', item.color);
                        } else {
                            // console.log($(element).css('background-color'));
                            if ($(element).css('background-color') === "rgb(237, 125, 49)") {
                                //SKIP
                            } else {
                                $(element).css('background-color', shadeHexColor(item.color, .5));

                            }

                        }
                    }

                },
            });

            calendar.fullCalendar({
                // put your options and callbacks here
                height: 620,
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
            });

        })();

        function fncListeners() {
            btnFiltroBuscar.on("click", function () {
                fncGetCalendarios();
            });

            btnFiltroExportar.on("click", function () {
                fncGenerarReporteCrystalReport();
            });

        }

        //#region BACK

        function fncGetCalendarios() {
            axios.post("GetCalendarios", { ccsFiltro: getValoresMultiples("#cboFiltroCC"), añoFiltro: cboFiltroAño.val() }).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    calendar.fullCalendar('removeEvents');
                    calendarHidden.setDataSource([]);

                    // let events = $('#calendar').fullCalendar('clientEvents');

                    let idEvent = 0;
                    for (const item of items) {
                        for (const itemFecha of item.fechas) {
                            let fecha = itemFecha.Value.split("/");
                            let fechaFormat = `${fecha[2]}-${fecha[1]}-${fecha[0]}`;

                            let dateAnteayer = moment(fechaFormat).add(-2, 'days');
                            let dateAyer = moment(fechaFormat).add(-1, 'days');

                            calendar.fullCalendar('addEventSource', [
                                {
                                    title: item.nombre,
                                    start: dateAnteayer,
                                    allDay: true,
                                    color: '#d3d3d3',
                                    // rendering: "background"
                                },
                                {
                                    title: item.nombre,
                                    start: dateAyer,
                                    allDay: true,
                                    color: '#d3d3d3',
                                    // rendering: "background"
                                },
                                {
                                    title: item.nombre,
                                    start: fechaFormat,
                                    color: '#808080',
                                    allDay: true
                                },
                            ]);

                            idEvent++;
                            var event1 = {
                                id: idEvent,
                                name: item.nombre,
                                // location: $('#event-modal input[name="event-location"]').val(),
                                startDate: moment(dateAnteayer),
                                endDate: moment(dateAnteayer),
                                color: "#ed7d31",
                                esTooltip: false
                            }

                            idEvent++;
                            var event2 = {
                                id: idEvent,
                                name: item.nombre,
                                // location: $('#event-modal input[name="event-location"]').val(),
                                startDate: moment(dateAyer),
                                endDate: moment(dateAyer),
                                color: "#ed7d31",
                                esTooltip: false
                            }

                            idEvent++;
                            var event3 = {
                                id: idEvent,
                                name: item.nombre,
                                // location: $('#event-modal input[name="event-location"]').val(),
                                startDate: moment(fechaFormat),
                                endDate: moment(fechaFormat),
                                color: "#ed7d31",
                                esTooltip: true
                            }

                            var dataSource = calendarHidden.getDataSource();
                            dataSource.push(event3);
                            dataSource.push(event1);
                            dataSource.push(event2);
                            calendarHidden.setDataSource(dataSource);
                        }
                    }

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGenerarReporteCrystalReport(chart, filename) {
            // var svg = chart.getSVG({
            //     exporting: {
            //         sourceWidth: chart.chartWidth,
            //         sourceHeight: chart.chartHeight
            //     }
            // });
            let domImgSrc;

            var node = document.getElementById('calendarHidden');
            // node.style.display = "block";

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
                        // node.style.display = "none";

                    };
                })
                .catch(function (error) {
                    console.error("oops, something went wrong!", error);
                });
        }

        function fncSaveImgSession() {
            let ccsReporte = "";

            let ccsValues = getValoresMultiples("#cboFiltroCC");
            let ccsDesc = $("#cboFiltroCC option:selected").text().split("    ");
            let ccsSplit = [];

            for (const item of ccsDesc) {
                if (ccsSplit.length > 0) {
                    ccsSplit.push(', ' + item.trim());
                } else {
                    ccsSplit.push(item.trim());
                }
                // if (item.includes(" ")) {
                //     ccsSplit.concat(item.trim(" "));
                // } else {
                //     ccsSplit.push(item);
                // }
            }

            let i = 0;
            for (const item of ccsSplit) {
                ccsReporte += `[${ccsValues[i]}] ${item}`;
            }

            axios.post("SaveImgSessionCalendario", { img: imgCalendario }).then(response => {
                let { success, items, message } = response.data;
                if (success) {


                    const report = $("#report");
                    report.attr("src", `/Reportes/Vista.aspx?idReporte=284&ccs=${ccsReporte}`);
                    document.getElementById('report').onload = function () {
                        openCRModal();
                        $.unblockUI();
                    };
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#endregion

        //#region GRALES
        function shadeHexColor(color, percent) {
            var f = parseInt(color.slice(1), 16), t = percent < 0 ? 0 : 255, p = percent < 0 ? percent * -1 : percent, R = f >> 16, G = f >> 8 & 0x00FF, B = f & 0x0000FF;
            return "#" + (0x1000000 + (Math.round((t - R) * p) + R) * 0x10000 + (Math.round((t - G) * p) + G) * 0x100 + (Math.round((t - B) * p) + B)).toString(16).slice(1);
        }
        //#endregion
    }

    $(document).ready(() => {
        Administrativo.CincoS = new CincoS();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();