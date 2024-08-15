(function () {
    $.namespace('Enkontrol.Compras.Requisicion.origenStock');
    origenStock = function () {
        const selCCOS = $("#selCCOS");
        const selAlmacenOS = $("#selAlmacenOS");
        const btnAddRenlonOS = $("#btnAddRenlonOS");
        const tblInsumosOS = $("#tblInsumosOS");
        const inputCantidadTotalOS = $("#inputCantidadTotalOS");

        function init() {
            initForm();
            btnAddRenlonOS.click(AddNewRenglon);
        }

        function initForm() {
            selCCOS.fillCombo('/Enkontrol/Requisicion/FillComboCcAsigReq', null, false, null);
            selAlmacenOS.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, null);
        }

        function _renglonNuevo(partida, nuevo, cancelado) {
            return $.post('/Enkontrol/Requisicion/_renglonNuevo', { partida, nuevo, cancelado });
        }


        function AddNewRenglon() {
            let cuerpo = tblInsumosOS.find('tbody');
            if (cuerpo.find("tr td").prop("colspan") == 11)
                cuerpo.empty();
            if (validaCamposRenglones()) {
                let partida = ++cuerpo.find("tr").length;
                _renglonNuevo(partida, true, false).done(function (_renglonNuevo) {
                    let row = initRenglonInsumo($(_renglonNuevo), partida);

                    cuerpo.append(row);

                    //txtDescPartida.prop("disabled", false);
                    //btnRemRenlon.prop("disabled", false);
                });
            }

            getCantidadTotal();
        }

        function validaCamposRenglones() {
            let ban = true;

            tblInsumosOS.find("tbody tr").each(function (idx, row) {
                let $row = $(row);

                if ($row.find(".insumo").val().length != 7) {
                    ban = false;
                }

                if ($row.data().isAreaCueta) {
                    if ($row.find(".insumo").val() == "000-000") {
                        ban = false;
                    }
                }

                if (+($row.find(".cantidad").val()) == 0) {
                    ban = false;
                }
            });

            return ban;
        }

        function initRenglonInsumo(row, partida) {
            row.find('.insumo').getAutocomplete(setInsumoDesc, { cc: selCCOS.val() }, '/Enkontrol/Requisicion/getInsumos');
            row.find('.insumoDesc').getAutocomplete(setInsumoBusqPorDesc, { cc: selCCOS.val() }, '/Enkontrol/Requisicion/getInsumosDesc');

            //row.find('.areaCuenta').fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: selCCOS.val() }, false, "000-000");
            //row.find('.fechaReq').datepicker().datepicker("setDate", new Date().addDays(selTipoReq.find(":selected").data().prefijo).toLocaleDateString());
            row.find(".cantidad").val(0).change();
            row.find(".porComprar").val(0).change();
            row.find(".exceso").val(0).change();
            row.find(".existencia").val(0).change();


            row.data({
                id: 0,
                idReq: 0,
                partida: partida,
                DescPartida: "",
                exceso: 0,
                isAreaCueta: false
            });

            return row;
        }

        function getCantidadTotal() {
            let cantidadTotal = 0;
            let inputs = tblInsumosOS.find('tbody tr .cantidad');

            inputs.each(function (index, elemento) {
                cantidadTotal += parseFloat(elemento.value)
            });

            inputCantidadTotalOS.val(cantidadTotal);
        }

        function setInsumoDesc(e, ui) {
            let exceso = ui.item.exceso,
                isAreaCueta = ui.item.isAreaCueta,
                row = $(this).closest('tr'),
                valor = row.find(".cantidad").val();
            row.find('.insumoDesc').val(ui.item.id);
            row.find('.unidad').text(ui.item.unidad);
            row.find(".porComprar").val(valor).change();
            row.find('.exceso').val(0).change();
            row.find('.existencia').text(getExistencia(ui.item.value, 400, selAlmacenOS.val() != '' ? selAlmacenOS.val() : 0));
            row.find('.existenciaBoton').removeClass('hidden');
            row.data({
                exceso: 0,
                isAreaCueta: isAreaCueta,
                insumo: ui.item.value
            });

            if (isAreaCueta)
                if (row.find(".areaCuenta").val() == "000-000")
                    row.find(".areaCuenta").val(row.find(".areaCuenta option:eq(1)").val());

            if (ui.item.cancelado == 'A') {
                row.find('.btn-estatus-activo').css('display', 'inline-block');
                row.find('.btn-estatus-inactivo').css('display', 'none');
                row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
            } else {
                row.find('.btn-estatus-activo').css('display', 'none');
                row.find('.btn-estatus-inactivo').css('display', 'inline-block');
                row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
            }

            function setInsumoBusqPorDesc(e, ui) {
                let exceso = ui.item.exceso,
                    isAreaCueta = ui.item.isAreaCueta,
                    row = $(this).closest('tr'),
                    valor = row.find(".cantidad").val();
                row.find('.insumo').val(ui.item.id);
                row.find('.insumoDesc').val(ui.item.value);
                row.find('.unidad').text(ui.item.unidad);
                row.find('.existencia').text(getExistencia(ui.item.id, 400, selAlmacenOS.val() != '' ? selAlmacenOS.val() : 0));
                row.find('.existenciaBoton').removeClass('hidden');
                row.data({
                    exceso: 0,
                    isAreaCueta: isAreaCueta,
                    insumo: ui.item.id
                });

                row.find(".porComprar").val(valor).change();
                row.find('.exceso').val(0).change();

                if (isAreaCueta)
                    if (row.find(".areaCuenta").val() == "000-000")
                        row.find(".areaCuenta").val(row.find(".areaCuenta option:eq(1)").val());

                if (ui.item.cancelado == 'A') {
                    row.find('.btn-estatus-activo').css('display', 'inline-block');
                    row.find('.btn-estatus-inactivo').css('display', 'none');
                    row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
                } else {
                    row.find('.btn-estatus-activo').css('display', 'none');
                    row.find('.btn-estatus-inactivo').css('display', 'inline-block');
                    row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
                }
            }
        }

        function setInsumoBusqPorDesc(e, ui) {
            let exceso = ui.item.exceso,
                isAreaCueta = ui.item.isAreaCueta,
                row = $(this).closest('tr'),
                valor = row.find(".cantidad").val();
            row.find('.insumo').val(ui.item.id);
            row.find('.insumoDesc').val(ui.item.value);
            row.find('.unidad').text(ui.item.unidad);
            row.find('.existencia').text(getExistencia(ui.item.id, 400, selAlmacenOS.val() != '' ? selAlmacenOS.val() : 0));
            row.find('.existenciaBoton').removeClass('hidden');
            row.data({
                exceso: 0,
                isAreaCueta: isAreaCueta,
                insumo: ui.item.id
            });

            row.find(".porComprar").val(valor).change();
            row.find('.exceso').val(0).change();

            if (isAreaCueta)
                if (row.find(".areaCuenta").val() == "000-000")
                    row.find(".areaCuenta").val(row.find(".areaCuenta option:eq(1)").val());

            if (ui.item.cancelado == 'A') {
                row.find('.btn-estatus-activo').css('display', 'inline-block');
                row.find('.btn-estatus-inactivo').css('display', 'none');
                row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
            } else {
                row.find('.btn-estatus-activo').css('display', 'none');
                row.find('.btn-estatus-inactivo').css('display', 'inline-block');
                row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
            }
        }

        function getExistencia(insumo, cc, almacen) {
            let existencia = 0;
            $.ajax({
                url: '/Enkontrol/Requisicion/GetExistenciaInsumo',
                datatype: "json",
                type: "GET",
                async: false,
                data: {
                    insumo: insumo,
                    cc: cc,
                    almacen: almacen
                },
                success: function (response) {
                    if (response.success) {
                        existencia = response.existencia;
                    }
                }
            });

            return existencia;
        }

    init();
}
$(document).ready(function () {
    Enkontrol.Compras.Requisicion.origenStock = new origenStock();
});
})();