(() => {
    $.namespace('Administrativo.Contabilidad.SistemaContable._capturaRelObras');
    _capturaRelObras = function () {
        let cboObras, optPal, busq;
        const tblObras = $('#tblObras');
        const btnBuscar = $('#btnBuscar');
        const btnGuardar = $('#btnGuardar');
        const cboEmpresaPal = $('#cboEmpresaPal');
        const cboEmpresaSec = $('#cboEmpresaSec');
        const DeleteObra = originURL('/Administrativo/RelObra/DeleteObra');
        const GuardarRelObras = originURL('/Administrativo/RelObra/GuardarRelObras');
        const RelObrasEmpresa = originURL('/Administrativo/RelObra/RelObrasEmpresa');
        const ObrasDescripcion = originURL('/Administrativo/RelObra/ObrasDescripcion');
        const getLstEmpresas = originURL('/Administrador/Usuarios/getLstEmpresasActivas');
        const setRelObrasSession = originURL('/Administrativo/RelObra/setRelObrasSession');
        const ItemsObraPorEmpresa = originURL('/Administrativo/RelObra/ItemsObraPorEmpresa');
        (() => {
            initForm();
            btnBuscar.click(SetRelObrasEmpresas);
            btnGuardar.click(setGuardarRelObras);
        })();
        function initForm() {
            axios.get(getLstEmpresas).then(response => {
                let { items, success } = response.data;
                if (success) {
                    cboEmpresaPal.fillComboItems(items, undefined, 3);
                    cboEmpresaSec.fillComboItems(items, undefined, 1);
                    getBusqForm();
                }
                SetItems();
            }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }
        function SetItems() {
            axios.get(ItemsObraPorEmpresa)
                .then(response => {
                    let { success, items } = response.data;
                    if (success) {
                        cboObras = items;
                        initDataTblObras();
                        SetRelObrasEmpresas();
                    }
                }).catch(o_O => AlertaGeneral(o_O.message));
        }
        function SetRelObrasEmpresas() {
            getBusqForm();
            if (busq.palEmpresa === busq.secEmpresa) {
                return;
            }
            dtObras.clear().draw();
            axios.post(RelObrasEmpresa, { busq })
                .then(response => {
                    let { success, lst } = response.data;
                    PreparaOptionsPrincipal();
                    $(dtObras.column(0).header()).text(cboEmpresaPal.find(":selected").text());
                    $(dtObras.column(1).header()).text(cboEmpresaSec.find(":selected").text());
                    if (success) {
                        dtObras.rows.add(lst).draw();
                    }
                    if (lst.length == 0) {
                        setNuevo();
                    }
                }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }
        function setGuardarRelObras() {
            try {
                relObras = getTblObrasData();
                let response = $.LoadInMemoryThenSave(setRelObrasSession.href, GuardarRelObras, relObras, null, 5, thenGuardarRelObras);
            } catch (o_O) {
                AlertaGeneral('Aviso', o_O.message);
            }
        }
        function thenGuardarRelObras() {
            SetRelObrasEmpresas();
            AlertaGeneral("Aviso", "Obras conciliadas con éxito.");
        }
        function setDeleteObra(_this) {
            let descPal = _this.closest("tr").find(".inpPal").val(),
                descSec = _this.parent().parent().text(),
                obraPal = ObraDesdeCadena(descPal),
                obraSec = ObraDesdeCadena(descSec),
                obra = {
                    PalEmpresa: busq.palEmpresa,
                    PalObra: obraPal.cc,
                    SecEmpresa: busq.secEmpresa,
                    SecObra: obraSec.cc,
                };
            _this.parent().parent().remove();
            axiosClean.post(DeleteObra, obra);
        }
        function getBusqForm() {
            busq = {
                palEmpresa: +cboEmpresaPal.val(),
                secEmpresa: +cboEmpresaSec.val()
            };
        }
        function ObraDesdeCadena(cadena) {
            let arrCadena = cadena.split("-");
            return {
                cc: arrCadena[0],
                descripcion: arrCadena[1].replace(/^\s+|\s+$/g, '')
            }
        }
        function setNuevo() {
            let lstPalSel = tblObras.find(`tr td .cboPal option:selected`).map((i, opt) => opt.value).toArray(),
                { Value, Text } = cboObras.find(option => option.Empresa === busq.palEmpresa && !lstPalSel.includes(option.Value));
            dtObras.row.add({
                PalEmpresa: busq.palEmpresa,
                PalObra: Value,
                PalDescripcion: Text,
                lstSec: []
            }).draw();
        }
        function PreparaOptionsPrincipal() {
            optPal = cboObras.filter(option => option.empresa === busq.palEmpresa);
        }
        function initDataTblObras() {
            dtObras = tblObras.DataTable({
                destroy: true
                , language: dtDicEsp
                , dom: 'f<"toolbar">rtip'
                , columns: [
                    { data: 'PalObra', width: "30%", title: 'Obra Principal', createdCell: (td, data, rowData, row, col) => $(td).html(setInputObraPrincipal(rowData)) },
                    { data: 'lstSec', title: 'Obra Secundaria', createdCell: (td, data, rowData, row, col) => $(td).html(setInputObraSecundaria(data)) },
                ]
                , initComplete: function (settings, json) {
                    createBtnTblObras();
                    tblObras.on('click', '.obraDelete', function (event) {
                        setDeleteObra($(this));
                    });
                }
            });
        }

        function createBtnTblObras() {
            btnNuevo = $("<button>", {
                id: "btnNuevo",
                type: "button",
                class: "btn btn-success",
                text: "Agregar Obra"
            });
            let ico = $("<i>", { class: "fa fa-plus" });
            btnNuevo.click(setNuevo);
            btnNuevo.prepend(ico);
            $("div.toolbar").html(btnNuevo);
        }
        //#region Autocumplete
        function setInputObraPrincipal({ lstSec, PalObra, PalDescripcion }) {
            let input = $("<input>", {
                type: "text",
                class: "form-control inpPal",
                width: "100%",
                disabled: lstSec.length > 0
            });
            input.getAutocompleteValid(ObraPalSelecionada, ObraPalCambio, { empresa: busq.palEmpresa }, ObrasDescripcion.href);
            input.val(`${PalObra}-${PalDescripcion}`);
            return input;
        }
        function setInputObraSecundaria(lstSec) {
            let lstValores = lstSec.map(obra => `${obra.SecObra}-${obra.SecDescripcion}`),
                input = $("<textarea>", {
                    type: "text",
                    class: "form-control inpSec",
                    width: "100%"
                }),
                div = $(`<div>`, {
                    class: "form-control divSec",
                    width: "100%"
                });
            setDescObraSec(lstValores, div);
            input.getAutocompleteValid(ObrSecSelecionada, ObraSecCambio, { empresa: busq.secEmpresa }, ObrasDescripcion.href);
            div.append(input);
            return div;
        }
        function setDescObraSec(lstSec, div) {
            lstSec.forEach(obra => {
                div.append(`<div class="diObraContainer2">
                <div class="divObraContainer">
                    <span class="obraFill">&nbsp;</span>
                    <span class="obraComponent">${obra}</span>
                    <button type="button" class="obraDelete">&nbsp;X</button>
                </div>`);
            });
        }
        function ObraPalSelecionada(e, ui) { }
        function ObrSecSelecionada(e, ui) {
            let row = $(this).closest('tr'),
                div = row.find(".divSec");
            setDescObraSec([ui.item.value], div);
            e.target.value = "";
        }
        function ObraPalCambio(e, ui) {
            if ((ui.item === null && $(this).val() != '')) {
                let row = $(this).closest('tr');
            }
        }
        function ObraSecCambio(e, ui) {
            e.target.value = "";
        }
        //#endregion
        function getTblObrasData() {
            let lst = [];
            dtObras.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let row = $(this.node())
                    , data = this.data()
                    , inpPal = row.find('.inpPal').val()
                    , obraPal = ObraDesdeCadena(inpPal)
                    , optSec = row.find('.obraComponent');
                optSec.each((i, optSec) => {
                    let obraSec = ObraDesdeCadena(optSec.textContent)
                    dataSec = data.lstSec.find(sec => obraSec.SecObra == sec.cc);
                    lst.push({
                        Id: dataSec == undefined ? 0 : dataSec.Id,
                        PalEmpresa: busq.palEmpresa,
                        PalObra: obraPal.cc,
                        SecEmpresa: busq.secEmpresa,
                        SecObra: obraSec.cc
                    })
                });
            });
            return lst;
        }
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.SistemaContable._capturaRelObras = new _capturaRelObras();
    });
})();