(() => {
    $.namespace('recursoshumanos.reportesrh.Expediciones');

    //#region CONSTS
    const tblExpediciones = $('#tblExpediciones');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const cboFiltroCC = $('#cboFiltroCC');
    const cboFiltroTipoFormato = $('#cboFiltroTipoFormato');
    const txtFiltroClaveEmpleado = $('#txtFiltroClaveEmpleado');
    const txtFiltroNombreEmpleado = $('#txtFiltroNombreEmpleado');
    //#endregion

    let formatosEnum = [];
    formatosEnum[0] = "FONACOT";
    formatosEnum[1] = "GUARDERIA";
    formatosEnum[2] = "LABORAL";
    formatosEnum[3] = "LACTANCIA";
    formatosEnum[4] = "LIBERACION";
    formatosEnum[5] = "PAGARE";
    formatosEnum[6] = "PRESTAMOS";

    Expediciones = function () {
        (function init() {
            fncListeners();
            initTblExpediciones();
            fncGetExpediciones();

        })();

        function fncListeners() {
            cboFiltroCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, null);

            btnFiltroBuscar.on("click", function () {
                fncGetExpediciones();
            });
        }

        //#region MAIN
        function initTblExpediciones() {
            dtExpediciones = tblExpediciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'cveEmpleado', title: 'NUM. EMPLEADO' },
                    { data: 'nombreEmpleado', title: 'EMPLEADO' },
                    { data: 'ccDesc', title: 'CC' },
                    {
                        data: 'idReporte', title: 'TIPO REPORTE',
                        render: function (data, type, row) {
                            return formatosEnum[data ?? 0];
                        }
                    },
                    { data: 'firmaElect', title: 'FIRMA' },
                    {
                        data: 'nombreExpidio', title: 'NOMBRE EXP.',
                        render: function (data, type, row) {
                            return (data ?? "").toUpperCase();
                        }
                    },
                    {
                        data: 'fechaCreacion', title: 'FECHA EXP.', render: (data, type, row, meta) => {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    {
                        title: 'ARCHIVO', render: (data, type, row, meta) => {
                            return `<button class='btn btn-xs btn-primary descargarArchive' title='Descargar.' ><i class='fas fa-print'></i></button>`;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblExpediciones.on('click', '.classBtn', function () {
                        let rowData = dtExpediciones.row($(this).closest('tr')).data();
                    });
                    tblExpediciones.on('click', '.descargarArchive', function () {
                        let rowData = dtExpediciones.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('Descargar', '¿Desea descargar el archivo seleccionado?', 'Confirmar', 'Cancelar', () => {
                            location.href = `/Administrativo/ReportesRH/GetArchivoExpedicion?file_id=${rowData.idArchivoExp}`;
                        });
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetExpediciones() {
            let obj = {
                cc: cboFiltroCC.val(),
                tipoReporte: cboFiltroTipoFormato.val(),
                claveEmpleado: txtFiltroClaveEmpleado.val(),
                nombreEmpleado: txtFiltroNombreEmpleado.val(),
            }
            axios.post("GetExpediciones", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtExpediciones.clear();
                    dtExpediciones.rows.add(items);
                    dtExpediciones.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        recursoshumanos.reportesrh.Expediciones = new Expediciones();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();