(() => {
    $.namespace('CH.CatContratos');


    let dtContratos;
    const tblContratos = $('#tblContratos');
    const btnFiltroNuevoUser = $('#btnFiltroNuevoUser');
    const btnDuracionAñadir = $('#btnDuracionAñadir');
    const mdlDuracion = $('#mdlDuracion');

    //CONSTS MDL
    const txtDuracionNombre = $('#txtDuracionNombre');
    const cboDuracionPeriodo = $('#cboDuracionPeriodo');
    const txtDuracionCantidad = $('#txtDuracionCantidad');

    CatContratos = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            initTblContratos();
            fncGetDuracionContratos();

            btnFiltroNuevoUser.on("click", function () {
                txtDuracionCantidad.val("");
                txtDuracionNombre.val("");
                mdlDuracion.modal("show");
            });

            btnDuracionAñadir.on("click", function () {
                if (txtDuracionNombre.val() == "") {
                    Alert2Warning("Ingrese un nombre");
                    return "";
                }

                if (cboDuracionPeriodo.val() != 3) {
                    if (txtDuracionCantidad.val() == "") {
                        Alert2Warning("Ingrese una cantidad");
                        return "";
                    }
                }

                fncAddDuracionContrato();
            });
        }

        function initTblContratos() {
            dtContratos = tblContratos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'nombre', title: 'nombre' },
                    {
                        title: '',
                        render: function (data, type, row) {
                            return `
                                <button class="btn btn-danger eliminarDuracion btn-xs" title="Eliminar fase."><i class="far fa-trash-alt"></i></button>
                            `;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblContratos.on('click', '.classBtn', function () {
                        let rowData = dtContratos.row($(this).closest('tr')).data();
                    });
                    tblContratos.on('click', '.eliminarDuracion', function () {
                        let rowData = dtContratos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncDeleteDuracionContrato(rowData.clave_duracion));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetDuracionContratos() {
            axios.post("GetDuracionContratos").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtContratos.clear();
                    dtContratos.rows.add(items);
                    dtContratos.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncDeleteDuracionContrato(id_duracion) {
            axios.post("DeleteDuracionContrato", { id_duracion }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    fncGetDuracionContratos();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAddDuracionContrato() {
            let obj;

            switch (cboDuracionPeriodo.val()) {
                case "0":
                    obj = {
                        duracion_desc: txtDuracionNombre.val(),
                        dias: txtDuracionCantidad.val(),
                        meses: null,
                        años: null,
                        idef: false,
                    }
                    break;
                case "1":
                    obj = {
                        duracion_desc: txtDuracionNombre.val(),
                        dias: null,
                        meses: txtDuracionCantidad.val(),
                        años: null,
                        indef: false,
                    }
                    break;
                case "2":
                    obj = {
                        duracion_desc: txtDuracionNombre.val(),
                        dias: null,
                        meses: null,
                        años: txtDuracionCantidad.val(),
                        indef: false,
                    }
                    break;
                case "3":
                    obj = {
                        duracion_desc: txtDuracionNombre.val(),
                        dias: null,
                        meses: null,
                        años: null,
                        indef: true,
                    }
                    break;

                default:
                    break;
            }

            axios.post("AddDuracionContrato", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    fncGetDuracionContratos();
                }
            }).catch(error => Alert2Error(error.message));
        }
    }

    $(document).ready(() => {
        CH.CatContratos = new CatContratos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();