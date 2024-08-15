(() => {
    $.namespace('CH.Staffing');

    //#region CONSTS
    const cboFiltroCC = $('#cboFiltroCC');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroExportar = $('#btnFiltroExportar');
    const tblStaffing = $('#tblStaffing');
    const btnFiltroExportarCR = $('#btnFiltroExportarCR');

    let dtStaffing;
    //#endregion

    Staffing = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            initTblStaffing();

            // cboFiltroCC.fillCombo('/Reclutamientos/FillCboCC', {}, false);
            cboFiltroCC.fillComboBox('/Reclutamientos/GetCCs', null, '-- Seleccionar --', () => {
                cboFiltroCC.select2();
            });

            // fncGetPuestosCategorias();

            btnFiltroBuscar.on("click", function () {
                fncGetPuestosCategorias();

            });

            btnFiltroExportar.on("click", function () {
                fncCrearReporte();
            });

            btnFiltroExportarCR.on("click", function () {
                getReporte();
            });
        }

        function initTblStaffing() {
            dtStaffing = tblStaffing.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: true,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'id_puesto', title: 'ID' },
                    { data: 'puesto', title: 'PUESTO' },
                    { data: 'cantidad', title: 'PERSONAL EN PLANTILLA' },
                    { data: 'altas', title: 'PERSONAL EXISTENTE' },
                    { data: 'totalXContratar', title: 'totalXContratar' },

                ],
                initComplete: function (settings, json) {
                    tblStaffing.on('click', '.classBtn', function () {
                        let rowData = dtStaffing.row($(this).closest('tr')).data();
                    });
                    tblStaffing.on('click', '.classBtn', function () {
                        let rowData = dtStaffing.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                drawCallback: function (settings) {
                    // tblStaffing.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                    //     if (colIdx > 1) {
                    //         let total = 0;

                    //         for (let x = 0; x < this.data().length; x++) {
                    //             total += this.data()[x];
                    //         }

                    //         this.column(colIdx).visible(total != 0);
                    //     }
                    // });

                    let total = 0;
                    let invisibles = 0;

                    tblStaffing.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                        if (colIdx > 1) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            if (this.column(colIdx).visible()) {
                                $(this.footer()).html(total);
                            }
                            else {
                                invisibles++;
                            }
                            total = 0;
                        }
                    });
                },
            });
        }

        function fncGetPuestosCategorias() {
            let obj = new Object();
            obj = {
                _cc: cboFiltroCC.val(),
                _strPuesto: $('select[id="cboFiltroPuesto"] option:selected').text(),
            }
            axios.post("GetPuestosCategoriasRelPuesto", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtStaffing.clear();
                    dtStaffing.rows.add(response.data.lstPuestosRelPuesto);
                    dtStaffing.draw();
                    // lstPuestos = response.data.lstPuestosRelPuesto;
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearReporte(params) {
            axios.post("creaVariableDeSesion", { cc: cboFiltroCC.val() }).then(response => {
                location.href = '/Administrativo/ReportesRH/crearReporte';
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                }
            }).catch(error => Alert2Error(error.message));
        }

        function getReporte() {

            // $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = idReporte;

            var path = "/Reportes/Vista.aspx?idReporte=258&cc=" + cboFiltroCC.val();
            ireport = $("#report");
            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();
                openCRModal();

            };
        }
    }

    $(document).ready(() => {
        CH.Staffing = new Staffing();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();