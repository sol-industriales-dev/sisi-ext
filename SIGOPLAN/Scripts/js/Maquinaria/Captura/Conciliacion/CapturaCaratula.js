(function () {
    $.namespace('SIGOPLAN.Controllers.Maquinaria.Caratula');
    Caratula = function () {
        urlCboCC = '/Conciliacion/getCboCC';
        selCapCC = $("#selCapCC");
        radioBtn = $('.radioBtn a');
        tblPrecio = $("#tblPrecio");
        btnAddRow = $("#btnAddRow");
        btnDelRow = $("#btnDelRow");
        btnGuardar = $("#btnGuardar");
        tblIncluye = $("#tblIncluye");
        tblNoIncluye = $("#tblNoIncluye");
        dtVencimiento = $('#dtVencimiento');
        tblConsideracion = $(".tblConsideracion");
        var longConsideracion = 0;
        function init() {
            dtVencimiento.prop('disabled', true);
            selCapCC.change(setLstPrecios);
            btnAddRow.click(addNuevoRenglon);
            btnDelRow.click(remSelRow);
            btnGuardar.click(guardar);
            radioBtn.click(aClick);
            initForm();
        }
        function getLstPrecios(ccID, cc, moneda) {
            return $.post("/Conciliacion/getLstPrecios", { ccID: ccID, cc: cc });
        }
        function getConsideraciones() {
            return $.post("/Conciliacion/getConsideraciones");
        }
        function _nuevoRowPrecio() {
            return $.post("/Conciliacion/_nuevoRowPrecio");
        }
        function GuardarCaratula(enc, lst, lstCon) {
            return $.post("/Conciliacion/GuardarCaratula", { enc: enc, lst: lst, lstCon: lstCon });
        }
        function guardar() {
            if (vaildaGuardar) {
                GuardarCaratula({
                    ccID: selCapCC.val(),
                    moneda: getRadioValue("radMoneda"),
                    fechaVigencia: dtVencimiento.val()
                },
                    getTblPrecio(),
                    getTblConsideracion())
                    .done(function (response) {
                        if (response.success)
                            AlertaGeneral("Aviso", "Caratula guardada correctamente.");
                    });
            }
        }
        function getTblPrecio() {
            var lst = [];
            tblPrecio.find("tbody tr").each(function (idx, row) {
                let data = $(row).data();
                for (const [i, v] of data.idModelo.entries()) {
                    lst.push({
                        idGrupo: data.idGrupo,
                        idModelo: v,
                        unidad: data.unidad,
                        costo: data.costo,
                        cargoFijo: data.cargoFijo,
                        cOverhaul: data.cOverhaul,
                        cMttoCorrectivo: data.cMttoCorrectivo,
                        cCombustible: data.cCombustible,
                        cAceites: data.cAceites,
                        cFiltros: data.cFiltros,
                        cAnsul: data.cAnsul,
                        cCarrileria: data.cCarrileria,
                        cLlantas: data.cLlantas,
                        cHerramientasDesgaste: data.cHerramientasDesgaste,
                        cCargoOperador: data.cCargoOperador,
                        cPersonalMtto: data.cPersonalMtto
                    });
                }
            });
            return lst;
        }
        function getTblConsideracion() {
            var lst = [];
            for (let i = 0; i < longConsideracion; i++) {
                lst.push({
                    ConsideracionCostoHora: i + 1,
                    isActivo: getRadioValue(`${'radCon' + i}`)
                });
            }
            return lst;
        }
        function vaildaGuardar() {
            let ban = selCapCC.val() != "";
            tblPrecio.find("tbody tr").each(function (idx, row) {
                let data = $(row).data();
                if (data.idGrupo == 0)
                    ban = false;
                if (data.idModelo == 0)
                    ban = false;
            });
            return ban;
        }
        function setLstPrecios() {
            let cc = selCapCC.val(),
                isVacio = cc == "";
            dtPercio.clear().draw();
            if (isVacio)
                $('.radioBtn > a').addClass("disabled");
            else {
                getLstPrecios(
                    cc,
                    selCapCC.find(':selected').data().prefijo
                )
                    .done(function (response) {
                        if (response.success) {
                            // AddRows(tblPrecio, response.lstPrecio);
                            dtPercio.rows.add(response.lstPrecio).draw();
                            dtPercio.columns.adjust();
                            $('.radioBtn > a').removeClass("disabled");
                            setConsideracionesEstatus(response.lstConci);
                            dtVencimiento.val(response.Vencimiento);
                        }
                    });
            }
        }
        function setConsideracionesEstatus(lst) {
            if (lst.length == 0)
                for (let i = 0; i < longConsideracion; i++)
                    setRadioValue(`${'radCon' + i}`, true);
            else
                for (let i in lst)
                    if (lst.hasOwnProperty(i))
                        setRadioValue(`${'radCon' + +(+(i) + 1)}`, lst[i].isActivo);
        }
        function setConsideraciones() {
            dtConsidaracion.clear().draw();
            getConsideraciones()
                .done(function (response) {
                    if (response.success) {
                        longConsideracion = response.lstIncluye.length + response.lstNoIncluye.length;
                        AddRows(tblIncluye, response.lstIncluye);
                        AddRows(tblNoIncluye, response.lstNoIncluye);
                    }
                });
        }
        function addNuevoRenglon() {
            if (selCapCC.val() != "")
                _nuevoRowPrecio()
                    .done(function (row) {
                        if (tblPrecio.find('tbody').text() == "Ningún dato disponible en esta tabla")
                            tblPrecio.find('tbody').empty();
                        let $row = initRowPrecio($(row));
                        tblPrecio.find('tbody').append($row);
                    });
        }
        function remSelRow() {
            tblPrecio.find("tr.active").remove();
        }
        function initRowPrecio(row) {
            row.find('.grupo').fillCombo('/Conciliacion/getCboGrupo', null, false, null);
            row.find(".unidad").fillCombo('/Conciliacion/getCboUnidad', null, false, null);

            return row;
        }
        function initForm() {
            selCapCC.fillCombo(urlCboCC, null, false, null);
            dtVencimiento.datepicker().datepicker("setDate", new Date());
            initTblPrecio();
            initTblsConsideracio();
            setConsideraciones();
        }
        function initTblPrecio() {
            dtPercio = tblPrecio.DataTable({
                info: false,
                paging: false,
                searching: false,
                language: dtDicEsp,
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    $(nRow).data({
                        idGrupo: aData.idGrupo,
                        idModelo: aData.idModelo,
                        unidad: aData.unidad,
                        costo: aData.costo,
                        cargoFijo: aData.cargoFijo,
                        cOverhaul: aData.cOverhaul,
                        cMttoCorrectivo: aData.cMttoCorrectivo,
                        cCombustible: aData.cCombustible,
                        cAceites: aData.cAceites,
                        cFiltros: aData.cFiltros,
                        cAnsul: aData.cAnsul,
                        cCarrileria: aData.cCarrileria,
                        cLlantas: aData.cLlantas,
                        cHerramientasDesgaste: aData.cHerramientasDesgaste,
                        cCargoOperador: aData.cCargoOperador,
                        cPersonalMtto: aData.cPersonalMtto

                    });
                },
                columns: [
                    {
                        data: 'idGrupo', sortable: false, 
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<select></select>");
                            $(td).find("select").addClass('form-control grupo');
                            $(td).find("select").fillCombo('/Conciliacion/getCboGrupo', null, false, null);
                            $(td).find("select").val(cellData);
                        }
                    },
                    {
                        data: 'idModelo', sortable: false, 
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<select></select>");
                            $(td).find("select").addClass('form-control modelo');
                            $(td).find("select").prop("multiple", "multiple");
                            $(td).find("select").fillCombo('/Conciliacion/getCboModelo', { idGrupo: rowData.idGrupo }, true, null);
                            $(td).find("select").val(cellData);
                            convertToMultiselect($(td).find(".modelo"));
                            $(td).find("select");
                        }
                    },
                    {
                        data: 'unidad', sortable: false, 
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<select></select>");
                            $(td).find("select").addClass('form-control unidad');
                            $(td).find("select").fillCombo('/Conciliacion/getCboUnidad', null, false, null);
                            $(td).find("select").val(cellData);
                        }
                    },
                    {
                        data: 'costo', sortable: false, 
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input></input>");
                            $(td).find("input").addClass('form-control text-right costo').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'cargoFijo', sortable: false, 
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input data-tipo='1'></input>");
                            $(td).find("input").addClass('form-control text-right setPrecio').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'cOverhaul', sortable: false, 
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input data-tipo='2'></input>");
                            $(td).find("input").addClass('form-control text-right setPrecio').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'cMttoCorrectivo', sortable: false, 
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input data-tipo='3'></input>");
                            $(td).find("input").addClass('form-control text-right setPrecio').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'cCombustible', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input data-tipo='4'></input>");
                            $(td).find("input").addClass('form-control text-right setPrecio').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'cAceites', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input data-tipo='5'></input>");
                            $(td).find("input").addClass('form-control text-right setPrecio').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'cFiltros', sortable: false, 
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input data-tipo='6'></input>");
                            $(td).find("input").addClass('form-control text-right setPrecio').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'cAnsul', sortable: false, 
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input data-tipo='7'></input>");
                            $(td).find("input").addClass('form-control text-right setPrecio').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'cCarrileria', sortable: false, 
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input data-tipo='8'></input>");
                            $(td).find("input").addClass('form-control text-right setPrecio').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'cLlantas', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input data-tipo='9'></input>");
                            $(td).find("input").addClass('form-control text-right setPrecio').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'cHerramientasDesgaste', sortable: false, 
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input data-tipo='10'></input>");
                            $(td).find("input").addClass('form-control text-right setPrecio').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'cCargoOperador', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input data-tipo='11'></input>");
                            $(td).find("input").addClass('form-control text-right setPrecio').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'cPersonalMtto', sortable: false, 
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input data-tipo='12'></input>");
                            $(td).find("input").addClass('form-control text-right setPrecio').val(maskDinero(cellData));
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblPrecio.on('change', '.setPrecio', function () {
                        let row = $(this).closest('tr'),
                            costo = unmaskDinero(this.value);
                        this.value = maskDinero(costo);

                        switch ($(this).attr('data-tipo')) {
                            case "1":
                                row.data().cargoFijo = costo;
                                break;
                            case "2":
                                row.data().cOverhaul = costo;
                                break;
                            case "3":
                                row.data().cMttoCorrectivo = costo;
                                break;
                            case "4":
                                row.data().cCombustible = costo;
                                break;
                            case "5":
                                row.data().cAceites = costo;
                                break;
                            case "6": row.data().cFiltros = costo;
                                break;
                            case "7": row.data().cAnsul = costo;
                                break;
                            case "8": row.data().cLlantas = costo;
                                break;
                            case "9": row.data().cCarrileria = costo;
                                break;
                            case "10": row.data().cHerramientasDesgaste = costo;
                                break;
                            case "11": row.data().cCargoOperador = costo;
                                break;
                            case "12": row.data().cPersonalMtto = costo;
                                break;
                            default:
                        }
                    });
                    tblPrecio.on('change', '.costo', function () {
                        let row = $(this).closest('tr'),
                            costo = unmaskDinero(this.value);
                        this.value = maskDinero(costo);
                        row.data().costo = costo;
                    });
                    tblPrecio.on('change', '.grupo', function () {
                        let row = $(this).closest('tr'),
                            sel = row.find("select.modelo");
                        row.data().idGrupo = this.value;
                        sel.prop("multiple", "multiple");
                        sel.fillCombo('/Conciliacion/getCboModelo', { idGrupo: this.value }, true, null);
                        convertToMultiselect(sel);
                    });
                    tblPrecio.on('change', '.modelo', function () {
                        let row = $(this).closest('tr'),
                            val = getValoresMultiples(`#tblPrecio > tbody > tr:eq(${row[0].rowIndex - 1}) > td:eq(1) .modelo`);
                        row.data().idModelo = val;
                    });
                    tblPrecio.on('change', '.unidad', function () {
                        let row = $(this).closest('tr');
                        row.data().unidad = this.value;
                    });
                    tblPrecio.on('click', 'tbody tr', function () {
                        var selected = $(this).hasClass("active");
                        tblPrecio.find("tr").removeClass("active");
                        if (!selected)
                            $(this).not("th").addClass("active");
                    });
                }
            });
        }
        function initTblsConsideracio() {
            dtConsidaracion = tblConsideracion.DataTable({
                info: false,
                paging: false,
                searching: false,
                language: dtDicEsp,
                iDisplayLength: -1,
                columns: [
                    {
                        data: 'id', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(`<div data-id='${cellData}'><a data-toggle='radCon${cellData}' data-title=${true}><i></i></a><a data-toggle='radCon${cellData}' data-title=${false}><i></i></a></div>`);
                            $(td).find("div").addClass('radioBtn btn-group');
                            $(td).find("a").addClass('btn btn-primary disabled');
                            $($(td).find("a")[0]).addClass(`active`);
                            $($(td).find("i")[0]).addClass('fa fa-check');
                            $($(td).find("a")[1]).addClass(`notActive`);
                            $($(td).find("i")[1]).addClass('fa fa-times');
                        }
                    },
                    {
                        data: 'id', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html((`${"0" + +(row + 1)}`).slice(-2)).addClass('text-right');
                        }
                    },
                    { data: 'descripcion', sortable: false }
                ],
                initComplete: function (settings, json) {
                    tblConsideracion.on('click', '.radioBtn a', function () {
                        let sel = $(this).data('title'),
                            tog = $(this).data('toggle');
                        $('#' + tog).prop('value', sel);
                        $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
                        $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');
                    });
                }
            });
        }
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear();
            for (let i in lst)
                if (lst.hasOwnProperty(i))
                    AddRow(dt, lst[i]);
        }
        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }
        function unmaskDinero(dinero) {
            return Number(dinero.replace(/[^0-9\.]+/g, ""));
        }
        function maskDinero(numero) {
            return `${parseFloat(numero).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")}`;
        }
        String.prototype.parseDate = function () {
            return new Date(parseInt(this.replace('/Date(', '')));
        }
        Date.prototype.parseDate = function () {
            return this;
        }
        //#region aRadio
        function setRadioValue(tog, sel) {
            $(`#${tog}`).prop('value', sel);
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
        }
        function aClick() {
            let sel = $(this).data('title');
            let tog = $(this).data('toggle');
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
        }
        function getRadioValue(tog) {
            return $(`a.active[data-toggle="${tog}"]`).data('title');
        }
        //#endregion
        init();
    }
    $(document).ready(function () {
        SIGOPLAN.Controllers.Maquinaria.Caratula = new Caratula();
    })
        .ajaxStart(function () {
            $.blockUI({
                message: 'Procesando...'
            });
        })
        .ajaxStop(function () {
            $.unblockUI();
        });
})();