(() => {
    $.namespace('CH.Saldos');

    //#region FILTRO CONSTS
    const txtClaveEmpleado = $('#txtClaveEmpleado');
    const txtNombreEmpleado = $('#txtNombreEmpleado');
    const cboVacacionCC = $('#cboVacacionCC');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const tblSaldos = $('#tblSaldos');
    const cboFiltroEstatusEmpleado = $('#cboFiltroEstatusEmpleado');
    let dtSaldos;
    //#endregion

    //#region MODAL CONSTS
    const mdlLstSaldos = $('#mdlLstSaldos');
    const tblSaldosDet = $('#tblSaldosDet');
    let dtSaldosDet;

    const btnNewSaldo = $('#btnNewSaldo');
    //#endregion

    //#region MODAL CE CONSTS
    const mdlCESaldo = $('#mdlCESaldo');
    const txtCESaldoAdd = $('#txtCESaldoAdd');
    const txtCESaldoActual = $('#txtCESaldoActual');
    const btnCESaldo = $('#btnCESaldo');
    //#endregion

    //#region MODAL SLADOS ANUAL
    const mdlCESaldoAnual = $('#mdlCESaldoAnual');
    const tblSaldosAnual = $('#tblSaldosAnual');
    const spantMdlSaldosAnualesFechaAntigue = $('#spantMdlSaldosAnualesFechaAntigue');

    let dtSaldosAnual
    //#endregion

    Saldos = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //URL
            obtenerUrlPArams();

            //#region INITS
            initTblSaldos();
            initTblSaldosDet();
            initTblSaldosAnual();
            //#endregion

            //#region FILL COMBOS
            cboVacacionCC.fillCombo('/Administrativo/Vacaciones/FillComboCC', {}, false);

            //#endregion

            btnFiltroBuscar.on("click", function () {
                GetSaldos();
                // if () {

                // } else {
                //     Alert2Warning("Favor de ingresar el nombre de un empleado o un CC");
                // }
            });

            btnNewSaldo.on("click", function () {
                txtCESaldoAdd.val("");
                mdlCESaldo.modal("show");
            });

            btnCESaldo.on("click", function () {
                fncCESaldo();

            })

            // txtCESaldoAdd.on("change", function () {
            //     if ($(this).val() > 0) {
            //         let newVal = txtCESaldoActual.val() + $(this).val();
            //         txtCESaldoActual.val()
            //     }
            // });
        }

        function initTblSaldos() {
            dtSaldos = tblSaldos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                dom: 'Bfrtip',
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'clave_empleado', },
                    { data: 'nombre_completo', },
                    {
                        data: 'estatusEmpleado',
                        render: function (data, type, row) {
                            return data == "A" ? "Activo" : "Baja";
                        }
                    },
                    { data: 'cc', },
                    {
                        data: 'fecha_alta',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY")
                        }
                    },
                    { data: 'años_servicio', },
                    { data: 'dias_ganadosActual', },
                    { data: 'dias_difrutadosActual', },
                    { data: 'dias_pendientesGozarActual', },
                    {
                        data: 'dias_proporcionalProximo',
                        render: function (data, type, row) {
                            return (Number(data).toFixed(2));
                        }
                    },
                    {
                        data: 'dias_totalPendientesGozarProximo',
                        render: function (data, type, row) {
                            return (Number(data).toFixed(2));
                        }
                    },
                    {
                        data: 'salario_diario',
                        render: function (data, type, row) {
                            return maskNumero(Number(data).toFixed(2));
                        }
                    },
                    {
                        data: 'vacaciones',
                        render: function (data, type, row) {
                            return maskNumero(Number(data).toFixed(2));
                        }
                    },
                    {
                        data: 'prima_vacacionalProporcional',
                        render: function (data, type, row) {
                            return maskNumero(Number(data).toFixed(2));
                        }
                    },
                    {
                        data: 'prima_vacacional',
                        render: function (data, type, row) {
                            return maskNumero(Number(data).toFixed(2));
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblSaldos.on('click', '.detalle', function () {
                        let rowData = dtSaldos.row($(this).closest('tr')).data();

                        GetSaldosDet(rowData.clave_empleado);
                        btnCESaldo.data("claveempleado", rowData.clave_empleado);

                        txtCESaldoActual.val(rowData.saldo_actual);

                        mdlLstSaldos.modal("show");
                    });
                    tblSaldos.on('click', '.detalleAnual', function () {
                        let rowData = dtSaldos.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                        mdlCESaldoAnual.modal("show");

                        spantMdlSaldosAnualesFechaAntigue.text(moment(rowData.fechaAntiguedad).format("DD/MM/YYYY"));

                        dtSaldosAnual.clear();
                        dtSaldosAnual.rows.add(rowData.lstSaldosAnuales);
                        dtSaldosAnual.draw();

                    });
                },
                columnDefs: [
                    { width: '20%', targets: 1 },
                    { width: '20%', targets: 3 },
                    { width: '5%', targets: 5 },
                    { width: '5%', targets: 6 },
                    { width: '5%', targets: 7 },
                    { width: '5%', targets: 8 },
                    { width: '5%', targets: 9 },
                    { width: '5%', targets: 10 },
                    { className: 'dt-center', 'targets': '_all' },
                    // {
                    //     targets: [11, 12],
                    //     createdCell: function (td, cellData, rowData, row, col) {
                    //         if (rowData.dias_periodoActual === 0) {
                    //             $(td).css('color', 'red');
                    //             // $('td', row).eq(11).css('color', 'red');
                    //         }
                    //     }
                    // }
                ],
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            columns: ':visible'
                            // columns: [1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61]
                        }

                    }
                ],
                createdRow: function (row, data, start, end, display) {
                    totalVac = dtSaldos
                        .column(12)
                        .data()
                        .reduce(function (a, b) {
                            return Number((a)) + Number((b));
                        }, 0);
                    $(dtSaldos.column(12).footer()).html(maskNumero(totalVac));

                    totalPrimaVac = dtSaldos
                        .column(13)
                        .data()
                        .reduce(function (a, b) {
                            return Number((a)) + Number((b));
                        }, 0);
                    $(dtSaldos.column(13).footer()).html(maskNumero(totalPrimaVac));

                    totalPrimaVac = dtSaldos
                        .column(14)
                        .data()
                        .reduce(function (a, b) {
                            return Number((a)) + Number((b));
                        }, 0);
                    $(dtSaldos.column(14).footer()).html(maskNumero(totalPrimaVac));
                },
            });
        }

        function initTblSaldosDet() {
            dtSaldosDet = tblSaldosDet.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'num_dias', title: 'DIAS ADICIONALES' },
                    {
                        data: 'fechaCreacion', title: 'FECHA CAPTURA',
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    {
                        render: (data, type, row, meta) => {
                            return `<button class='btn btn-xs btn-danger delRow' title='Eliminar.'><i class="fas fa-trash"></i></button>`;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblSaldosDet.on('click', '.classBtn', function () {
                        let rowData = dtSaldosDet.row($(this).closest('tr')).data();
                    });
                    tblSaldosDet.on('click', '.delRow', function () {
                        let rowData = dtSaldosDet.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncDeleteSaldoDet(rowData.id, rowData.clave_empleado));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTblSaldosAnual() {
            dtSaldosAnual = tblSaldosAnual.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'descRango', title: 'Rango' },
                    { data: 'saldo_inicial', title: 'Dias Iniciales' },
                    { data: 'saldo_adicional', title: 'Dias Adicionales' },
                    { data: 'dias_gozados', title: 'Dias gozados' },
                    { data: 'saldo_actual', title: 'Saldo' },
                ],
                initComplete: function (settings, json) {
                    tblSaldosAnual.on('click', '.classBtn', function () {
                        let rowData = dtSaldosAnual.row($(this).closest('tr')).data();
                    });
                    tblSaldosAnual.on('click', '.classBtn', function () {
                        let rowData = dtSaldosAnual.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function GetSaldos() {
            let objFiltro = {
                cc: cboVacacionCC.val(),
                clave_empleado: txtClaveEmpleado.val(),
                nombre_empleado: txtNombreEmpleado.val(),
                estatusEmpleado: cboFiltroEstatusEmpleado.val(),
            }

            axios.post("GetSaldos", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    dtSaldos.clear();
                    dtSaldos.rows.add(items);
                    dtSaldos.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function GetSaldosDet(clave_empleado) {
            axios.post("GetSaldosDet", { clave_empleado }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtSaldosDet.clear();
                    dtSaldosDet.rows.add(items);
                    dtSaldosDet.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncDeleteSaldoDet(id, clave_empleado) {
            axios.post("DeleteSaldoDet", { id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Registro eliminado con exito");
                    GetSaldosDet(clave_empleado);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCESaldo() {
            let objFiltro = {
                clave_empleado: btnCESaldo.data("claveempleado"),
                num_dias: txtCESaldoAdd.val()
            }

            axios.post("CrearEditarSaldo", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Saldo capturado con exito");
                    GetSaldosDet(objFiltro.clave_empleado);
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#region URL

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            if (variables.cargarModal == 1) {

                var clean_uri = location.protocol + "//" + location.host + location.pathname;
                window.history.replaceState({}, document.title, clean_uri);
            }
        }

        function getUrlParams(url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            return params;
        }

        //#endregion
    }

    $(document).ready(() => {
        CH.Saldos = new Saldos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();