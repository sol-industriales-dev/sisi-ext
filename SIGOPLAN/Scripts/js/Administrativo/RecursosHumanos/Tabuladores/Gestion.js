(() => {
    $.namespace('CH.RelPuestoFases');

    //#region CONST FILTROS
    const cboFiltroCC = $("#cboFiltroCC")
    const cboFiltroVistaAutorizacion = $("#cboFiltroVistaAutorizacion")
    const btnFiltroBuscar = $("#btnFiltroBuscar")
    //#endregion

    //#region CONST AUTORIZACION
    let dtRegistros
    const tblRegistros = $('#tblRegistros')
    //#endregion

    RelPuestoFases = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            //#endregion
        }

        //#region FUNCIONES AUTORIZANTES
        function initTblRegistros() {
            dtRegistros = tblRegistros.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return `btnEditar btnEliminar`
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblRegistros.on('click', '.editarRegistro', function () {
                        let rowData = dtRegistros.row($(this).closest('tr')).data();
                        fncGetDatosActualizarCaptura(rowData.id);
                    });
                    tblRegistros.on('click', '.eliminarRegistro', function () {
                        let rowData = dtRegistros.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCaptura(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', 'targets': '_all' },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncGetAccesosMenu() {
            axios.post('GetAccesosMenu').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    for (let i = 0; i <= 9; i++) {
                        switch (response.data.lstAccesosDTO[i]) {
                            case 0:
                                $("#menuLineaNegocios").css("display", "inline");
                                break;
                            case 1:
                                $("#menuPuestos").css("display", "inline");
                                break;
                            case 2:
                                $("#menuTabuladores").css("display", "inline");
                                break;
                            case 3:
                                $("#menuPlantillasPersonal").css("display", "inline");
                                break;
                            case 4:
                                $("#menuGestionTabuladores").css("display", "inline");
                                break;
                            case 5:
                                $("#menuGestionPlantillasPersonal").css("display", "inline");
                                break;
                            case 6:
                                $("#menuModificacion").css("display", "inline");
                                break;
                            case 7:
                                $("#menuGestionModificacion").css("display", "inline");
                                break;
                            case 8:
                                $("#menuReportes").css("display", "inline");
                                break;
                            case 9:
                                $("#menuGestionReportes").css("display", "inline");
                                break;
                        }
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.RelPuestoFases = new RelPuestoFases();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();