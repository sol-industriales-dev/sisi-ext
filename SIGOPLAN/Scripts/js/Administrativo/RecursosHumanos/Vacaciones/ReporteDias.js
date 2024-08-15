(() => {
    $.namespace('CH.Vacaciones');

    //#region CONSTS FILTRO
    const cboVacacionCC = $('#cboVacacionCC');
    const cboFiltroTipoPermiso = $('#cboFiltroTipoPermiso');
    const dateFiltroIni = $('#dateFiltroIni');
    const dateFiltroFin = $('#dateFiltroFin');

    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroExportar = $('#btnFiltroExportar');
    const tblReporte = $('#tblReporte');
    let dtReporte;
    //#endregion

    Vacaciones = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {

            cboVacacionCC.fillCombo('/Administrativo/Vacaciones/FillComboCC', {}, false);

            btnFiltroBuscar.on("click", function () {
                if (cboVacacionCC.val() != null && cboFiltroTipoPermiso.val() != null && dateFiltroIni.val() != "" && dateFiltroFin.val() != "") {

                    let momDateIni = moment(dateFiltroIni.val());
                    let momDateFin = moment(dateFiltroFin.val());

                    if (momDateIni._d < momDateFin._d) {
                        fncGetVacaciones(momDateIni, momDateFin);

                    }
                    else {
                        Alert2Warning("Las fechas ingresadas son invalidas")
                    }
                } else {
                    Alert2Warning("Favor de capturar los filtros")

                }

            });

            btnFiltroExportar.on("click", function () {
                fncGetExcel();
            })
        }

        //#region TBL

        function initTblReporte(columnas, data) {

            let columns = columnas.map(x => {
                return {
                    data: x.Item1,
                    title: x.Item2,
                    render: function (data, type, row, meta) {
                        if (data.estatus) {
                            return `<button class="btn btn-xs btn-success botonRedondo"><i class="fa fa-check"></i></button>`;

                        } else {
                            return `<button class="btn btn-xs btn-default botonRedondo" disabled><i class="fa fa-times"></i></button>`;

                        }
                    }
                }
            });

            var columnasFinal = [{ data: 'claveEmpleado', title: 'Cve', width: '30px' }, { data: 'empleadoDesc', title: 'Empleado', width: '200px' }];

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
                            format: {
                                body: function (data, row, column, node) {
                                    // Strip $ from salary column to make it numeric
                                    console.log(data)
                                    return column > 1 ?
                                        ($(data).hasClass("btn-success") ? "SI" : "NO") :
                                        data;
                                }
                            },
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

        //#region BACK

        function fncGetVacaciones(fecha1, fecha2) {
            let objFiltro = {
                cc: cboVacacionCC.val(),
                tipoVacaciones: cboFiltroTipoPermiso.val(),
                fechaInicial: fecha1.format("DD/MM/YYYY"),
                fechaFinal: fecha2.format("DD/MM/YYYY"),
            }

            axios.post("GetVacacionesReporte", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
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

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetExcel() {

            let momDateIni = moment(dateFiltroIni.val());
            let momDateFin = moment(dateFiltroFin.val());

            let objFiltro = {
                cc: cboVacacionCC.val(),
                tipoVacaciones: cboFiltroTipoPermiso.val(),
                fechaInicial: momDateIni.format("DD/MM/YYYY"),
                fechaFinal: momDateFin.format("DD/MM/YYYY"),
            }

            axios.post("SetFiltrosExcel", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    location.href = 'GetExcelReporte';
                }
            }).catch(error => Alert2Error(error.message));

        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Vacaciones = new Vacaciones();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();