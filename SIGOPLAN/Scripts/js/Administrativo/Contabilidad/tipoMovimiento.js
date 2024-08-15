(() => {
    $.namespace('Movimiento.tipoMovimiento');
    tipoMovimiento = function () {

        const cboCC = $("#cboCC");
        const cboTM = $("#cboTM");
        const tbltipoMovimientos = $("#tbltipoMovimientos");
        const botonBuscar = $("#botonBuscar");
        const botonGuardar = $("#botonGuardar");


        (function init() {
            fncListeners();
            // dpBanMovMax.datepicker().datepicker('setDate', hoy);
            // dpBanMovMin.datepicker().datepicker('setDate', new Date(hoy.getFullYear(), hoy.getMonth(), 1));
        })();

        function fncListeners() {
            initTbltipoMovimientos();
            CargarCombos();
        }

        function CargarCombos() {
            cboCC.fillCombo('/Administrativo/ConciliacionCC/FillCCPrincipal', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");
            cboTM.fillCombo('/Administrativo/Poliza/getComboTipoMovimiento', null, true, null);
            convertToMultiselect("#cboTM");
        }

        function initTbltipoMovimientos() {
            dttipoMovimientos = tbltipoMovimientos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEditar + btnEliminar;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tbltipoMovimientos.on('click', '.editarRegistro', function () {
                        let rowData = dttipoMovimientos.row($(this).closest('tr')).data();
                        fncGetDatosActualizarCaptura(rowData.id);
                    });
                    tbltipoMovimientos.on('click', '.eliminarRegistro', function () {
                        let rowData = dttipoMovimientos.row($(this).closest('tr')).data();
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



























    }

    $(document).ready(() => {
        Movimiento.tipoMovimiento = new tipoMovimiento();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();