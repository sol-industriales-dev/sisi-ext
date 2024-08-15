(() => {
    $.namespace('facturacion.prefacturacion.Insumos');

    //#region CONSTS
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroNuevo = $('#btnFiltroNuevo');
    const tblInsumos = $('#tblInsumos');

    let dtInsumos;
    let dtInsumosEK;
    //#endregion

    //#region MDL CONSTS
    const tblInsumosEK = $('#tblInsumosEK');
    const spanCEInsumoTitulo = $('#spanCEInsumoTitulo');
    const mdlCEInsumo = $('#mdlCEInsumo');
    const btnCEInsumo = $('#btnCEInsumo');
    const cboCEInsumos = $('#cboCEInsumos');

    const txtCEInsumoClave = $('#txtCEInsumoClave');
    const txtCEInsumoDesc = $('#txtCEInsumoDesc');
    const txtCEInsumoUnidad = $('#txtCEInsumoUnidad');
    //#endregion

    Insumos = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            initTblInsumos();
            initTblInsumosEK();
            fncGetInsumosSAT();

            cboCEInsumos.select2({
                placeholder: "Agrege insumo",
            });

            cboCEInsumos.fillCombo('/Facturacion/Prefacturacion/FIllComboInsumos', null, false, null);

            btnFiltroNuevo.on("click", function () {
                spanCEInsumoTitulo.text("CREAR");
                mdlCEInsumo.modal("show");
                btnCEInsumo.data('insumo', 0);

                btnCEInsumo.text("Añadir");

                txtCEInsumoClave.val("");
                txtCEInsumoDesc.val("");
                txtCEInsumoUnidad.val("");
                cboCEInsumos.val(null).trigger('change');
            });

            btnCEInsumo.on("click", function () {
                fncCEInsumo(btnCEInsumo.data('insumo'));
            });
        }

        //#region BACK
        function initTblInsumos() {
            dtInsumos = tblInsumos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'clave', title: 'INSUMO EK' },
                    { data: 'descripcion', title: 'DESCRIPCION' },
                    { data: 'unidad', title: 'UNIDAD' },
                    {
                        render: function (data, type, row) {
                            return `
                            <button title="Actualizar Insumo" class="btn btn-sm btn-warning actualizarInsumo btn-xs"><i class="far fa-edit"></i></button>
                            <button title="Eliminar Insumo" class="btn btn-sm btn-danger eliminarInsumo btn-xs"> <i class="far fa-trash-alt"></i></button>
                            `;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblInsumos.on('click', '.actualizarInsumo', function () {
                        let rowData = dtInsumos.row($(this).closest('tr')).data();
                        mdlCEInsumo.modal("show");
                        spanCEInsumoTitulo.text("EDITAR");
                        btnCEInsumo.data('insumo', rowData.id);

                        txtCEInsumoClave.val(rowData.clave);
                        txtCEInsumoDesc.val(rowData.descripcion);
                        txtCEInsumoUnidad.val(rowData.unidad);

                        cboCEInsumos.val(null).trigger('change');

                        btnCEInsumo.text("Editar");

                        fncGetInsumoEK(rowData.clave);
                    });
                    tblInsumos.on('click', '.eliminarInsumo', function () {
                        let rowData = dtInsumos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarInsumo(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTblInsumosEK() {
            dtInsumosEK = tblInsumosEK.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'insumo', title: 'INSUMO EK' },
                    { data: 'descripcion', title: 'DESCRIPCION' },
                    {
                        render: function (data, type, row) {
                            return `<button title="Eliminar Insumo" class="btn btn-sm btn-danger eliminarInsumoRel btn-xs"> <i class="far fa-trash-alt"></i></button>`
                        }
                    }

                ],
                initComplete: function (settings, json) {
                    tblInsumosEK.on('click', '.classBtn', function () {
                        let rowData = dtInsumosEK.row($(this).closest('tr')).data();
                    });
                    tblInsumosEK.on('click', '.eliminarInsumoRel', function () {
                        let rowData = dtInsumosEK.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarInsumoRel(rowData.insumo));

                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetInsumosSAT() {
            let obj = {
                clave: 0,
                descripcion: "",
                esActivo: true,
            };

            axios.post("GetInsumosSAT", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    dtInsumos.clear();
                    dtInsumos.rows.add(items);
                    dtInsumos.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetInsumoEK(idInsumo) {
            let obj = {
                idInsumoSAT: idInsumo
            }

            axios.post("GetInsumosEK", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    dtInsumosEK.clear();
                    dtInsumosEK.rows.add(items);
                    dtInsumosEK.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEInsumo(idInsumo) {
            let objInsumo = {
                id: idInsumo,
                clave: txtCEInsumoClave.val(),
                descripcion: txtCEInsumoDesc.val(),
                unidad: txtCEInsumoUnidad.val(),
            }

            let obj = {
                objInsumo: objInsumo,
                lstRel: cboCEInsumos.val()
            }

            axios.post("CrearEditarInsumos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    mdlCEInsumo.modal("hide");
                    if (idInsumo > 0) {
                        Alert2Exito("Insumo actualizado con exito");

                    } else {
                        Alert2Exito("Insumo registrado con exito");

                    }

                    fncGetInsumosSAT();
                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarInsumo(idInsumo) {

            axios.post("EliminarInsumo", { idInsumo: idInsumo }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Warning("Insumo eliminado con exito");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarInsumoRel(idInsumoEK) {
            let obj = {
                idInsumoSAT: txtCEInsumoClave.val(),
                idInsumoEK: idInsumoEK,
            }

            axios.post("EliminarRelInsumo", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    fncGetInsumoEK(txtCEInsumoClave.val());
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        facturacion.prefacturacion.Insumos = new Insumos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();