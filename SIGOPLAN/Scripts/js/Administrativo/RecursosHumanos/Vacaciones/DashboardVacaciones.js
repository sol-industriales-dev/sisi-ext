(() => {
    $.namespace('CH.DashboardVacaciones');

    //#region CONSTS FILTRO
    const cboVacacionCC = $('#cboVacacionCC');
    const cboFiltroTipoPermiso = $('#cboFiltroTipoPermiso');
    const dateFiltroIni = $('#dateFiltroIni');
    const dateFiltroFin = $('#dateFiltroFin');

    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const tblReporte = $('#tblReporte');
    let dtReporte;
    //#endregion

    //#region CONSTS CALENDARIO

    let calendar;
    //#endregion

    DashboardVacaciones = function () {
        (function init() {
            fncListeners();

        })();

        function fncListeners() {
            initCalenndario();

            cboVacacionCC.fillCombo('/Administrativo/Reclutamientos/FillComboCCUnique', {}, false, 'Todos');
            convertToMultiselect('#cboVacacionCC');

            btnFiltroBuscar.on("click", function () {
                if (getValoresMultiples('#cboVacacionCC').length > 0 && cboFiltroTipoPermiso.val() != null && dateFiltroIni.val() != "" && dateFiltroFin.val() != "") {

                    let momDateIni = moment(dateFiltroIni.val());
                    let momDateFin = moment(dateFiltroFin.val());

                    if (momDateIni._d < momDateFin._d) {
                        GetVacaciones(momDateIni, momDateFin);

                    }
                    else {
                        Alert2Warning("Las fechas ingresadas son invalidas")
                    }
                } else {
                    Alert2Warning("Favor de capturar los filtros")

                }
            });
        }

        //#region TBL
        function initTblReporte(columnas, data) {

            let columns = columnas.map(x => {
                return {
                    data: x.Item1,
                    title: x.Item2,
                    render: function (data, type, row, meta) {
                        return data.cantidad;
                    }
                }
            });

            var columnasFinal = [{ data: 'ccDesc', title: 'CC', width: '30px' },];

            columnasFinal.push(...columns);
            // columnasFinal.push({
            //     render: function (data, type, row) {
            //         return `<button class="btn btn-warning actualizarExpediente btn-xs" ><i class="far fa-edit"></i></button>&nbsp`;
            //     }
            // });

            dtReporte = tblReporte.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: true,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                columns: columnasFinal,
                dom: 'Bfrtip',
                initComplete: function (settings, json) {
                    tblReporte.on('click', '.classBtn', function () {
                        let rowData = dtReporte.row($(this).closest('tr')).data();
                    });
                    tblReporte.on('click', '.classBtn', function () {
                        let rowData = dtReporte.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        customize: function (xlsx) {
                            var sheet = xlsx.xl.worksheets['sheet1.xml'];

                            // Loop over the cells in column `C`
                            // $('row c', sheet).each(function () {
                            // $('row c[r^="C"]', sheet).each(function () {
                            //     // Get the value
                            //     if ($('is t', this).text() == 'New York') {
                            //         $(this).attr('s', '20');
                            //     }
                            // });
                        },
                        exportOptions: {
                            // format: {
                            //     body: function (data, row, column, node) {
                            //         // Strip $ from salary column to make it numeric
                            //         console.log(data)
                            //         return column > 1 ?
                            //             ($(data).hasClass("btn-success") ? "SI" : "NO") :
                            //             data;
                            //     }
                            // },
                            columns: ':visible'
                            // columns: [1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61]
                        },


                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });

            dtReporte.clear();
            dtReporte.rows.add(data);
            dtReporte.draw();
        }
        //#endregion

        //#region CALENDARIO
        function initCalenndario() {
            calendar = new Calendar('#calendar', {
                language: 'es',
                style: 'custom',
                mouseOnDay: function (e) {
                    if (e.events.length > 0) {
                        var content = '';

                        let cantidad = isNaN(e.events[0].cantidad) ? 0 : e.events[0].cantidad

                        if (cantidad > 0) {
                            content += '<div class="event-tooltip-content">'
                                + '<div class="event-name"><b>' + cantidad + '</b> Empleado(s)</div>'
                                + '</div>';
                        }

                        $(e.element).popover({
                            trigger: 'manual',
                            container: 'body',
                            html: true,
                            content: content
                        });

                        $(e.element).popover('show');
                    }
                },
                mouseOutDay: function (e) {
                    if (e.events.length > 0) {
                        $(e.element).popover('hide');
                    }
                },
                customDataSourceRenderer: function (element, date, event) {
                    // This will override the background-color to the event's color
                    for (var item of event) {
                        switch (true) {
                            case item.cantidad > 0 && item.cantidad < 25:
                                $(element).css('background-color', "#ffd280");

                                break;

                            case item.cantidad >= 25 && item.cantidad < 50:
                                $(element).css('background-color', "#ffae1a");

                                break;

                            case item.cantidad >= 50:
                                $(element).css('background-color', "#b37300");

                                break;

                            default:
                                break;
                        }

                    }
                },
            });

        }
        //#endregion

        //#region BACK

        function GetVacaciones(fecha1, fecha2) {

            let objFiltro = {
                // cc: cboVacacionCC.val(),
                lstFiltroCC: getValoresMultiples('#cboVacacionCC'),
                tipoVacaciones: cboFiltroTipoPermiso.val(),
                fechaInicial: fecha1.format("DD/MM/YYYY"),
                fechaFinal: fecha2.format("DD/MM/YYYY"),
            }

            axios.post("GetDashboardVacaciones", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    calendar.setDataSource([]);
                    var dataSource = calendar.getDataSource();
                    let idEvent = 0;

                    if (response.data.rows.length > 0) {
                        if ($.fn.DataTable.isDataTable('#tblReporte')) {
                            dtReporte.clear().destroy();
                            tblReporte.empty();

                            // dtReporte = null;

                        }
                        initTblReporte(response.data.cols, response.data.rows);
                    } else {
                        if ($.fn.DataTable.isDataTable('#tblReporte')) {
                            dtReporte.clear();
                            dtReporte.draw();
                        }
                    }

                    for (const item of items) {
                        idEvent++;
                        var event = {
                            id: idEvent,
                            name: item.cantidad,
                            // location: $('#event-modal input[name="event-location"]').val(),
                            startDate: moment(item.fecha),
                            endDate: moment(item.fecha),
                            cantidad: item.cantidad
                        }

                        dataSource.push(event);
                    }

                    calendar.setDataSource(dataSource);
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#endregion
    }

    $(document).ready(() => {
        CH.DashboardVacaciones = new DashboardVacaciones();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();    