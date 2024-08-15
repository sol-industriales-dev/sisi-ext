(() => {
    $.namespace('Administrativo.SistemaContable.iTipoMovimiento._RelacioniTm');
    _RelacioniTm = function () {
        let dtItm, busq, catISistema = [], catiTm = [], optPal = [], optSec = [];
        const tblItm = $('#tblItm');
        const btnBuscar = $('#btnBuscar');
        const btnGuardar = $('#btnGuardar');
        const cboEmpresaPal = $('#cboEmpresaPal');
        const cboEmpresaSec = $('#cboEmpresaSec');
        const GuardarRelitm = originURL('/Administrativo/iTipoMovimiento/GuardarRelitm');
        const getLstEmpresas = originURL('/Administrador/Usuarios/getLstEmpresasActivas');
        const ReliTmEmpresas = originURL('/Administrativo/iTipoMovimiento/ReliTmEmpresas');
        const ItemsiTmPorEmpresa = originURL('/Administrativo/iTipoMovimiento/ItemsiTmPorEmpresa');
        const setRelitmSession = originURL('/Administrativo/iTipoMovimiento/setRelitmSession');
        (() => {
            initForm();
            btnBuscar.click(setiTipoMovimiento);
            btnGuardar.click(setGuardarRelitm);
        })();
        function initForm() {
            axios.get(getLstEmpresas).then(response => {
                let { items, success } = response.data;
                if (success) {
                    cboEmpresaPal.fillComboItems(items, undefined, 3);
                    cboEmpresaSec.fillComboItems(items, undefined, 1);
                }
                setCboiTmEmpresas();
            }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }
        function setCboiTmEmpresas() {
            axios.get(ItemsiTmPorEmpresa)
                .then(response => {
                    let { success, optISistema, optiTm } = response.data;
                    if (success) {
                        catISistema = optISistema;
                        catiTm = optiTm;
                        getBusqForm();
                        initDataTblItm();
                        setiTipoMovimiento();
                    }
                }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }
        function setiTipoMovimiento() {
            dtItm.clear().draw();
            getBusqForm();
            axios.post(ReliTmEmpresas, busq)
                .then(response => {
                    let { success, lst } = response.data;
                    setOptionsEmpresas();
                    $(dtItm.column(0).header()).text(cboEmpresaPal.find(":selected").text());
                    $(dtItm.column(1).header()).text(cboEmpresaSec.find(":selected").text());
                    if (success) {
                        dtItm.rows.add(lst).draw();
                    }
                    if (lst.length == 0) {
                        setNuevo();
                    }
                }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }
        function setGuardarRelitm() {
            try {
                relitm = getTblitmData();
                $.LoadInMemoryThenSave(setRelitmSession.href, GuardarRelitm, relitm, null, 5, thenGuardarRelitm);
            } catch (o_O) {
                AlertaGeneral('Aviso', o_O.message);
            }
        }
        function thenGuardarRelitm() {
            AlertaGeneral("Aviso", "Tipo movimientos conciliados con éxito.");
            setiTipoMovimiento();
        }
        function getTblitmData() {
            let lst = [];
            dtItm.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let row = $(this.node()),
                    data = this.data();
                lst.push({
                    Id: data === undefined ? 0 : data.Id,
                    PalEmpresa: busq.palEmpresa,
                    PalSistema: row.find(".palSistema option:selected").val(),
                    PaliTm: +row.find(".palitm option:selected").val(),
                    SecEmpresa: busq.secEmpresa,
                    SecSistema: row.find(".secSistema option:selected").val(),
                    SeciTm: +row.find(".secitm option:selected").val(),
                });
            });
            return lst;
        }
        function getBusqForm() {
            busq = {
                palEmpresa: +cboEmpresaPal.val(),
                secEmpresa: +cboEmpresaSec.val()
            };
        }
        function setOptionsEmpresas() {
            optPal = catiTm.filter(opt => opt.Empresa == busq.palEmpresa);
            optSec = catiTm.filter(opt => opt.Empresa == busq.secEmpresa);
        }
        function setNuevo() {
            let primeriSistema = catISistema[0].Value,
                lstPalSel = [],
                lstSecSel = [];
            dtItm.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let row = $(this.node());
                lstPalSel.push(row.find(`.palitm`).val());
                lstSecSel.push(row.find(`.secitm`).val());
            });
            let primerPalItm = optPal.find(opt => opt.Prefijo === primeriSistema && !lstPalSel.includes(opt.Value)).Value,
                primerSecItm = optPal.find(opt => opt.Prefijo === primeriSistema && !lstSecSel.includes(opt.Value)).Value;
            dtItm.row.add({
                Id: 0,
                PalSistema: primeriSistema,
                PaliTm: primerPalItm,
                SecSistema: primeriSistema,
                SeciTm: primerSecItm,
            }).draw();
        }
        function createBtnTblitm() {
            btnNuevo = $("<button>", {
                id: "btnNuevo",
                type: "button",
                class: "btn btn-success",
                text: "Agregar Movimiento"
            });
            let ico = $("<i>", { class: "fa fa-plus" });
            btnNuevo.click(setNuevo);
            btnNuevo.prepend(ico);
            $("div.toolbar").html(btnNuevo);
        }
        function initDataTblItm() {
            dtItm = tblItm.DataTable({
                destroy: true,
                language: dtDicEsp,
                dom: 'f<"toolbar">rtip',
                paging: false,
                columns: [
                    { data: 'PaliTm', title: 'iTM Principal ', createdCell: (td, cellData, rowData, row, col) => $(td).html(setCboiTmPrincipal(rowData)) },
                    { data: 'SeciTm', title: 'iTM Secundario', createdCell: (td, cellData, rowData, row, col) => $(td).html(setCboiTmSecundario(rowData)) },
                ],
                initComplete: function (settings, json) {
                    createBtnTblitm();
                    tblItm.on('click', '.palitm, .secitm', function () {
                        DeshabilitarOpcionesSeleccionadas();
                    });
                    tblItm.on('change', '.palSistema', function () {
                        let row = $(this).closest("tr"),
                            itm = row.find(".palitm").val(),
                            cbos = setCboiTmPrincipal({ PalSistema: this.value, PaliTm: itm });
                        row[0].innerHTML = cbos;
                    });
                    tblItm.on('change', '.palSistema', function () {
                        let row = $(this).closest("tr"),
                            itm = row.find(".secitm").val(),
                            cbos = setCboiTmSecundario({ PalSistema: this.value, PaliTm: itm });
                        row[0].innerHTML = cbos;
                    });
                }
            });
        }
        function setCboiTmPrincipal({ PalSistema, PaliTm }) {
            let options = optPal.filter(opt => opt.Prefijo === PalSistema.trim());
            return setCboiTm({ iSistema: PalSistema.trim(), itm: PaliTm }, options, "pal");
        }
        function setCboiTmSecundario({ SecSistema, SeciTm }) {
            let options = optSec.filter(opt => opt.Prefijo === SecSistema.trim());
            return setCboiTm({ iSistema: SecSistema.trim(), itm: SeciTm }, options, "sec");
        }
        function setCboiTm({ iSistema, itm }, options, subfijo) {
            let div = $("<div>", { class: "input-group" }),
                cboSistema = $(`<select>`, { class: `form-control ${subfijo}Sistema`, disabled: catISistema.length == 1 }),
                cboItm = $(`<select>`, { class: `form-control ${subfijo}itm` });
            cboSistema.fillComboItems(catISistema, undefined, iSistema);
            cboItm.fillComboItems(options, undefined, itm);
            div.append(cboSistema);
            div.append($("<span>", { class: "input-group-addon", style: "padding:0px;" }));
            div.append(cboItm);
            return div;
        }
        function DeshabilitarOpcionesSeleccionadas() {
            let optDeshabilitada = [];
            tblItm.find(`select option`).prop('disabled', false);
            dtItm.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let row = $(this.node());
                optDeshabilitada.push({
                    PaliTm: row.find(`.palitm`).val(),
                    SeciTm: row.find(`.secitm`).val()
                });
            });
            let optPal = optDeshabilitada.map(opt => `.palitm option[value='${opt.PaliTm}']`).join(),
                optSec = optDeshabilitada.map(opt => `.secitm option[value='${opt.SeciTm}']`).join();
            tblItm.find(`${optPal}`).prop('disabled', true);
            tblItm.find(`${optSec}`).prop('disabled', true);
        }
    }
    $(document).ready(() => Administrativo.SistemaContable.iTipoMovimiento._RelacioniTm = new _RelacioniTm());
})();