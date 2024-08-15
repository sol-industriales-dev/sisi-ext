(() => {
    $.namespace('Administrativo.SistemaContable.Cuenta._conciliacion');
    _conciliacion = function () {
        //#region formularios
        let catCta = [], optPal = [], optSec = [], busq, btnNuevo, dtCuentas;
        const btnBuscar = $('#btnBuscar');
        const btnGuardar = $('#btnGuardar');
        const tblCuentas = $('#tblCuentas');
        const cboEmpresaPal = $('#cboEmpresaPal');
        const cboEmpresaSec = $('#cboEmpresaSec');
        const idEmpresa = $('#idEmpresa');
        //#endregion
        //#region url
        const DeleteCuenta = originURL('/Administrativo/Cuenta/DeleteCuenta');
        const getCuentaDesc = originURL('/Administrativo/Cuenta/getCuentaDesc');
        const getRelCuentas = originURL('/Administrativo/Cuenta/getRelCuentas');
        const GuardarRelCuentas = originURL('/Administrativo/Cuenta/GuardarRelCuentas');
        const getLstEmpresas = originURL('/Administrador/Usuarios/getLstEmpresasActivas');
        const setRelCuentasSession = originURL('/Administrativo/Cuenta/setRelCuentasSession');
        const getCboCuentasEmpresas = originURL('/Administrativo/Cuenta/getCboCuentasEmpresas');
        //#endregion
        //#region init
        let init = () => {
            initForm();
            btnBuscar.click(setRelAsignadaCuentas);
            btnGuardar.click(setGuardarRelCuentas);
        }
        function initForm() {
            axios.get(getLstEmpresas).then(response => {
                let { items, success } = response.data;
                if (success) {
                    if (idEmpresa.val() == 3) {
                        cboEmpresaPal.fillComboItems(items, undefined, 3);
                    } else if (idEmpresa.val() == 6) {
                        cboEmpresaPal.fillComboItems(items, undefined, 6);
                    }
                    // cboEmpresaPal.fillComboItems(items, undefined, 3);
                    cboEmpresaSec.fillComboItems(items, undefined, 1);
                }
                setCboCuentasEmpresas();
            }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }
        //#endregion
        //#region http
        //function setCboCuentasEmpresas() {
        //    axios.get(getCboCuentasEmpresas).then(response => {
        //        let { success, items } = response.data;
        //        if (success) {
        //            catCta = items;
        //            getBusqForm();
        //            setOptionsEmpresas();
        //            initDataTblCuentas();
        //            setRelAsignadaCuentas();
        //        }
        //    }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        //}

        function setCboCuentasEmpresas() {
            $.post(getCboCuentasEmpresas, { })
            .then(function(response) {                    
                if (response.success) {
                    catCta = response.items;
                    getBusqForm();
                    setOptionsEmpresas();
                    initDataTblCuentas();
                    setRelAsignadaCuentas();                      
                } 
            }, function(error) {
                // Error al lanzar la petición.
                AlertaGeneral("Operación fallida", "Ocurrió un error al lanzar la petición al servidor: Error " + error.status + " - " + error.statusText + ".");
            });
        }
        function setRelAsignadaCuentas() {
            getBusqForm();
            if (busq.palEmpresa === busq.secEmpresa) {
                return;
            }
            // dtCuentas.clear().draw();
            axios.post(getRelCuentas, busq)
                .then(response => {
                    let { success, lst } = response.data;
                    setOptionsEmpresas();
                    $(dtCuentas.column(0).header()).text(cboEmpresaPal.find(":selected").text());
                    $(dtCuentas.column(1).header()).text(cboEmpresaSec.find(":selected").text());
                    if (success) {
                        dtCuentas.rows.add(lst).draw();
                    }
                    if (lst.length == 0) {
                        setNuevo();
                    }
                }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }
        function setGuardarRelCuentas() {
            try {
                relCtas = getTblCuentasData();
                let response = $.LoadInMemoryThenSave(setRelCuentasSession.href, GuardarRelCuentas, relCtas, null, 5, thenGuardarRelCuentas);
            } catch (o_O) {
                AlertaGeneral('Aviso', o_O.message);
            }
        }
        function thenGuardarRelCuentas() {
            setRelAsignadaCuentas();
            AlertaGeneral("Aviso", "Cuentas conciliadas con éxito.");
        }
        function setDeleteCta(_this) {
            let descPal = _this.closest("tr").find(".cboPal").val(),
                descSec = _this.parent().parent().text(),
                ctaPal = CuentaDesdeCadena(descPal),
                ctaSec = CuentaDesdeCadena(descSec),
                cuenta = {
                    palEmpresa: busq.palEmpresa,
                    palCta: ctaPal.cta,
                    palScta: ctaPal.scta,
                    palSsta: ctaPal.sscta,
                    secEmpresa: busq.secEmpresa,
                    secCta: ctaSec.cta,
                    secScta: ctaSec.scta,
                    secSscta: ctaSec.sscta
                };
            _this.parent().parent().remove();
            axiosClean.post(DeleteCuenta, cuenta);
        }
        //#endregion
        //#region form functions
        function getBusqForm() {
            busq = {
                palEmpresa: +cboEmpresaPal.val(),
                secEmpresa: +cboEmpresaSec.val()
            };
        }
        function setOptionsEmpresas() {
            optPal = catCta.filter(option => option.empresa === busq.palEmpresa);
            let lstSec = catCta.filter(option => option.empresa === busq.secEmpresa);
            optSec = [];
            lstSec.forEach(grupo => {
                optSec.push(grupo);
                grupo.options.forEach(option => optSec.push(option));
            });
        }
        function initDataTblCuentas() {
            dtCuentas = tblCuentas.DataTable({
                destroy: true
                , language: dtDicEsp
                , dom: 'f<"toolbar">rtip'
                , columns: [
                    { data: 'palCta', width: "30%", title: 'Cuentas Principales', createdCell: (td, data, rowData, row, col) => $(td).html(setCboCuentasPrimarias(rowData)) },
                    { data: 'lstSec', title: 'Cuentas Secundarias', createdCell: (td, data, type, row, meta) => $(td).html(setCboCuentasSecundarias(data)) },
                ]
                , initComplete: function (settings, json) {
                    createBtnTblCuentas();
                    tblCuentas.on('change', '.cboPal', function (event) {

                    });
                    tblCuentas.on('click', '.ctaDelete', function (event) {
                        setDeleteCta($(this));
                    });
                }
            });
        }
        function createBtnTblCuentas() {
            btnNuevo = $("<button>", {
                id: "btnNuevo",
                type: "button",
                class: "btn btn-success",
                text: "Agregar Cuenta"
            });
            let ico = $("<i>", { class: "fa fa-plus" });
            btnNuevo.click(setNuevo);
            btnNuevo.prepend(ico);
            $("div.toolbar").html(btnNuevo);
        }
        function setCboCuentasPrimarias(cuenta) {
            let input = $("<input>", {
                type: "text",
                class: "form-control cboPal",
                width: "100%",
                disabled: cuenta.lstSec.length > 0
            });
            input.getAutocompleteValid(CuenaSelecionada, CuentaCambio, { empresa: busq.palEmpresa }, getCuentaDesc.href);
            input.val(`${cuenta.palCta}-${cuenta.palScta}-${cuenta.palSscta} ${cuenta.palDescripcion}`);
            return input;
        }
        function setCboCuentasSecundarias(lstSec) {
            let lstValores = lstSec.map(cta => `${cta.secCta}-${cta.secScta}-${cta.secSscta} ${cta.secDescripcion}`),
                input = $("<textarea>", {
                    type: "text",
                    class: "form-control cboSec",
                    width: "100%"
                }),
                div = $(`<div>`, {
                    class: "form-control divSec",
                    width: "100%"
                });
            setDescCuentsaSec(lstValores, div);
            input.getAutocompleteValid(CuenaSecSelecionada, CuentaCambioSec, { empresa: busq.secEmpresa }, getCuentaDesc.href);
            div.append(input);
            return div;
        }
        function setDescCuentsaSec(lstSec, div) {
            lstSec.forEach(cuenta => {
                div.append(`<div class="divCtaContainer2">
                <div class="divCtaContainer">
                    <span class="ctaFill">&nbsp;</span>
                    <span class="ctaComponent">${cuenta}</span>
                    <button type="button" class="ctaDelete">&nbsp;X</button>
                </div>`);
            });
        }
        function CuenaSelecionada(e, ui) {

        }
        function CuenaSecSelecionada(e, ui) {
            let row = $(this).closest('tr'),
                div = row.find(".divSec");
            setDescCuentsaSec([ui.item.value], div);
            e.target.value = "";
        }
        function CuentaCambio(e, ui) {
            if ((ui.item === null && $(this).val() != '')) {
                let row = $(this).closest('tr');
            }
        }
        function CuentaCambioSec(e, ui) {
            e.target.value = "";
        }
        function CuentaDesdeCadena(cadena) {
            let arrCadena = cadena.split("-"),
                ssCtaDescripcion = arrCadena[2].split(" "),
                cta = arrCadena[0].replace(/^\s+|\s+$/g, ''),
                scta = arrCadena[1].replace(/^\s+|\s+$/g, ''),
                sscta = ssCtaDescripcion[0].replace(/^\s+|\s+$/g, ''),
                descripcion = ssCtaDescripcion.filter(str => !ssCtaDescripcion[0].includes(str)).join(" ");
            return {
                cta: cta,
                scta: scta,
                sscta: sscta,
                descripcion: descripcion,
                text: `${cta}-${scta}-${sscta} ${descripcion}`
            }
        }
        function setNuevo() {
            let lstPalSel = tblCuentas.find(`tr td .cboPal option:selected`).map((i, opt) => opt.value).toArray(),
                primerCta = catCta.find(cta => cta.empresa === busq.palEmpresa && !lstPalSel.includes(cta.Value)).Text,
                cuentaNueva = CuentaDesdeCadena(primerCta);
            dtCuentas.row.add({
                palEmpresa: busq.palEmpresa,
                palCta: cuentaNueva.cta,
                palScta: cuentaNueva.scta,
                palSscta: cuentaNueva.sscta,
                palDescripcion: cuentaNueva.descripcion,
                lstSec: []
            }).draw();
        }
        function getTblCuentasData() {
            let lst = [];
            dtCuentas.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let row = $(this.node())
                    , data = this.data()
                    , cboPal = row.find(".cboPal").val()
                    , ctaPal = CuentaDesdeCadena(cboPal)
                    , optSecundarios = row.find(".ctaComponent");
                optSecundarios.each((i, optSec) => {
                    let ctaSec = CuentaDesdeCadena(optSec.textContent),
                        dataSec = data.lstSec.find(sec => ctaSec.text === `${sec.secCta}-${sec.secScta}-${sec.secSscta}`);
                    lst.push({
                        id: dataSec === undefined ? 0 : dataSec.id,
                        palEmpresa: busq.palEmpresa,
                        palCta: ctaPal.cta,
                        palScta: ctaPal.scta,
                        palSscta: ctaPal.sscta,
                        palDescripcion: ctaPal.descripcion,
                        secEmpresa: busq.secEmpresa,
                        secCta: ctaSec.cta,
                        secScta: ctaSec.scta,
                        secSscta: ctaSec.sscta,
                        secDescripcion: ctaSec.descripcion
                    });
                });
            });
            return lst;
        }
        //#endregion
        init();
    }
    $(document).ready(() => {
        Administrativo.SistemaContable.Cuenta._conciliacion = new _conciliacion();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();