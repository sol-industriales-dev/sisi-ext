(() => {
    $.namespace('Administrativo.Contabilidad.Reportes._numeroNafin');
    _numeroNafin = function () {
        const btnNafinNuevo = $('#btnNafinNuevo');
        const selNafinMoneda = $('#selNafinMoneda');
        const btnNafinGuardar = $('#btnNafinGuardar');
        const btnNafinElimina = $('#btnNafinElimina');
        const tblNafin = $('#tblNafin');
        let init = () => {
            initForm();
            selNafinMoneda.change(setLstNafin);
            btnNafinNuevo.click(setNuevoNafin);
            btnNafinElimina.click(setMensajeEliminarNafin);
            btnNafinGuardar.click(setMensajeGuardarNafin);
        }
        const getLstNafin = () => $.post('/Administrativo/Reportes/GetLstNafin', { moneda: selNafinMoneda.val() });
        const EliminarLstNafin = () => $.post('/Administrativo/Reportes/EliminarLstNafin', { prov: getObjNafinData() });
        const GuardarLstProvNafin = () => $.post('/Administrativo/Reportes/GuardarLstProvNafin', { lst: getLstNafinData() });
        function setLstNafin() {
            getLstNafin().then(response => {
                dtNafin.clear().rows.add(response.lstNafin).draw();
                deshabilitarProvSeleccionados();
            }).catch(o_O => { });
        }
        function setNuevoNafin() {
            dtNafin.row.add({
                NumProveedor: 0,
                NumNafin: ""
            }).draw();
            deshabilitarProvSeleccionados();
        }
        function setMensajeGuardarNafin() {
            AlertaAceptarRechazar("Aviso", `Se guardaran los proveedores. ¿Desea continuar?`, GuardarLstProvNafin, null)
                .then(btnAceptar => {
                    GuardarLstProvNafin().then(response => {
                        setLstNafin();
                        AlertaGeneral("Aviso", "Proveedores guardados con éxito.");
                    });
                })
                .catch(btnCancelar => { });
        }
        function setMensajeEliminarNafin() {
            let prov = tblNafin.find(`.selected .prov option:selected`).text();
            if (prov === "--Seleccione--")
                prov = "";
            AlertaAceptarRechazar("Aviso", `Se eliminará el proveedor ${prov}. ¿Desea continuar?`, EliminarLstNafin, null)
                .then(btnAceptar => {
                    EliminarLstNafin();
                    dtNafin.row('.selected').remove().draw(false);
                    AlertaGeneral("Aviso", "Proveedor eliminado con éxito.");
                })
                .catch(btnCancelar => { });
        }
        function deshabilitarProvSeleccionados() {
            let provDes = [];
            //#region obtiene proveedores       
            dtNafin.rows().iterator('row', function (context, index) {
                let row = $(this.row(index).node()),
                    prov = row.find('.prov').val();
                if (prov === null) {
                    row.find('.prov option').prop('disabled', false);
                    prov = row.find('.prov').val();
                }
                if (+(prov) > 0) {
                    provDes.push(prov);
                }
            });
            //#endregion
            //#region deshabilita prov
            dtNafin.rows().iterator('row', function (context, index) {
                let row = $(this.row(index).node());
                var sel = row.find('.prov');
                let opHab = sel.find(`option`);
                opHab.prop('disabled', false);
                provDes.forEach(provDes => {
                    let opDes = sel.find(`option[value="${provDes}"]`);
                    opDes.prop('disabled', true);
                });
            });
            //#endregion
        }
        function getLstNafinData() {
            let lst = [],
                moneda = selNafinMoneda.val();
            dtNafin.rows().iterator('row', function (context, index) {
                let row = $(this.row(index).node()),
                    nafin = row.find('.nafin').val(),
                    prov = row.find('.prov').val();
                if (prov === null) {
                    row.find('.prov option').prop('disabled', false);
                    prov = row.find('.prov').val();
                }
                if (+(prov) > 0 && nafin.length > 0) {
                    lst.push({
                        TipoMoneda: moneda,
                        NumProveedor: prov,
                        NumNafin: nafin
                    });
                }
            });
            return lst;
        }
        function getObjNafinData() {
            let row = tblNafin.find(`.selected`),
                nafin = row.find(`.nafin`).val(),
                prov = row.find(`.prov`).val();
            if (prov === null) {
                row.find('.prov option').prop('disabled', false);
                prov = row.find('.prov').val();
            }
            let obj = {
                TipoMoneda: selNafinMoneda.val(),
                NumProveedor: prov,
                NumNafin: nafin
            };
            return obj;
        }
        function initForm() {
            selNafinMoneda.fillCombo('/Administrativo/Reportes/FillComboTipoMoneda', null, false, null);
            selNafinMoneda.val(1);
            initDataTblNafin();
            setLstNafin();
        }
        function initDataTblNafin() {
            dtNafin = tblNafin.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    {
                        data: 'NumProveedor', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<select>`);
                            $(td).find(`select`).addClass(`form-control prov`);
                            $(td).find(`select`).fillCombo('/Administrativo/Reportes/FillComboProvMoneda', { moneda: selNafinMoneda.val() }, false, null);
                            if (data > 0) {
                                $(td).find(`select > option[value="${data}"]`).prop(`selected`, true);
                                $(td).find(`select`).prop(`disabled`, true);
                            }
                        }
                    },
                    {
                        data: 'NumNafin', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control nafin`);
                            $(td).find(`input`).val(data);
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblNafin.on("change", ".prov", function (event) {
                        deshabilitarProvSeleccionados();
                    });
                    tblNafin.find('tbody').on('click', 'tr', function () {
                        if ($(this).hasClass('selected')) {
                            $(this).removeClass('selected');
                        }
                        else {
                            tblNafin.find('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                        }
                    });
                }
            });
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Reportes._numeroNafin = new _numeroNafin();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();