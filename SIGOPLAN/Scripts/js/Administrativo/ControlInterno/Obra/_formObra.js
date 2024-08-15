(() => {
    $.namespace('Administrativo.ControlInterno.Obra._formObra');
    _formObra = function () {
        let catalogo = {}, lstAuthEstado = [];
        //#region elementos HTML
        const tblFormAuth = $('#tblFormAuth');
        const cboFormTipo = $('#cboFormTipo');
        const txtFormClave = $('#txtFormClave');
        const btnFormGuardar = $('#btnFormGuardar');
        const divFormEstatus = $('#divFormEstatus');
        const txtFormArranque = $('#txtFormArranque');
        const divMotivoRechazo = $('#divMotivoRechazo');
        const divFormEnkontrol = $('#divFormEnkontrol');
        const txtFormDescripcion = $('#txtFormDescripcion');
        const lblFormMotivoRechazo = $('#lblFormMotivoRechazo');
        const btnMotivoRechazoGuardar = $('#btnMotivoRechazoGuardar');
        //#endregion
        //#region urls
        const GuardarObra = new URL(window.location.origin + '/Administrativo/Obra/GuardarObra');
        const getFormDesdeClave = new URL(window.location.origin + '/Administrativo/Obra/getFormDesdeClave');
        const getCboTipoCatalogo = new URL(window.location.origin + '/Administrativo/Obra/getCboTipoCatalogo');
        const getAutocompleteClave = new URL(window.location.origin + '/Administrativo/Obra/getAutocompleteClave');
        //#endregion
        let init = () => {
            initForm();
            txtFormClave.getAutocompleteValid(setClave, verificarClave, null, getAutocompleteClave);
            btnFormGuardar.click(getFormCatalogo);
            txtFormClave.change(changeClave);
            btnMotivoRechazoGuardar.click(getFormMotivoCierre);
        }
        //#region http
        async function initForm() {
            txtFormArranque.datepicker({
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 9999);
                    }, 0);
                }
            }).datepicker('setDate', new Date());
            initDataTblFormAuth();
            response = await ejectFetchJson(getCboTipoCatalogo);
            let { success, items, itemsAuthEstado } = response;
            if (success) {
                cboFormTipo.fillComboItems(items, undefined, items[0].Value);
                lstAuthEstado = itemsAuthEstado;
            }
            setFormDefault();
        }
        async function setGuardarObra(lst) {
            try {
                response = await ejectFetchJson(GuardarObra, lst);
                let { success, message } = response;
                if (success) {
                    setTblObras();
                    NotificacionGeneral("Aviso", `El catálogo se guardo con éxito.`);
                    if (lst.contains(cat => cat.row == undefined)) {
                        setFormDesdeClave(catalogo);
                    }
                }
                else {
                    NotificacionGeneral("Aviso", message);
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        setFormDesdeClave = async ({ clave }) => {
            try {
                response = await ejectFetchJson(getFormDesdeClave, { clave });
                let { success, catalogoVM } = response;
                if (success) {
                    $("#mdlFormObra").modal('show');
                    catalogo = catalogoVM;
                    catalogo.fechaArranque = $.toDate(catalogo.fechaArranque);
                    setForm();
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        //#endregion
        //#region funciones eventos
        function changeClave() {
            txtFormClave.val(txtFormClave.val().toUpperCase());
        }
        function setClave(e, ul) {
            if (ul.item === null) {
                setFormDefault();
            } else {
                catalogo = ul.item;
                catalogo.fechaArranque = $.toDate(catalogo.fechaArranque);
                setForm();
            }
        }
        function verificarClave(e, ul) {
            if (ul.item == null) {
                let clave = txtFormClave.val();
                setFormDefault();
                txtFormClave.val(clave);
            }
            else {
                catalogo = ul.item;
            }
        }
        //#endregion
        //#region cargar/asigna elementos
        function getFormCatalogo() {
            getCatalogoData([catalogo]);
        }
        getCatalogoData = lst => {
            lst.forEach(cat => {
                if (cat.row === undefined) {
                    cat.clave = txtFormClave.val();
                    cat.descripcion = txtFormDescripcion.val();
                    cat.tipo = +cboFormTipo.val();
                    cat.fechaArranque = txtFormArranque.val();
                    cat.lstAuth = dtFormAuth.rows().data().toArray();
                    if (cat.puedeDarVobo) {
                        let voboEstado = +divFormEstatus.find(".darVobo").val()
                        cat.lstAuth.find(a => a.orden == 1).authEstado = voboEstado;
                        cat.lstAuth.find(a => a.orden == 2).authEstado = 3
                    }
                    if (cat.puedeDarAuth) {
                        let authEstado = +divFormEstatus.find(".darAuth").val();
                        cat.lstAuth.find(a => a.orden == 2).authEstado = authEstado;
                    }
                    if (cat.puedeDarEnkoltrol) {
                        cat.exiteEnkontrol = divFormEnkontrol.find(".darEnk").prop("checked");
                    }
                } else {
                    if (cat.puedeDarVobo) {
                        let voboEstado = +cat.row.find(".darVobo").val()
                        cat.lstAuth.find(a => a.orden == 1).authEstado = voboEstado;
                        cat.lstAuth.find(a => a.orden == 2).authEstado = 3
                    }
                    if (cat.puedeDarAuth) {
                        let authEstado = +cat.row.find(".darAuth").val();
                        cat.lstAuth.find(a => a.orden == 2).authEstado = authEstado;
                    }
                    if (cat.puedeDarEnkoltrol) {
                        cat.exiteEnkontrol = cat.row.find(".darEnk").prop("checked");
                    }
                }
                switch (true) {
                    case cat.lstAuth.every(auth => auth.authEstado === 1):
                        cat.authEstado = 1;
                        break;
                    case cat.lstAuth.contains(auth => auth.authEstado === 2):
                        cat.authEstado = 2;
                        break;
                    default:
                        cat.authEstado = 0;
                        break;
                }
            });
            if (lst.contains(cat => cat.authEstado == 2)) {
                setMdlRechazo(lst);
            } else {
                setGuardarObra(lst);
            }
        }
        getFormMotivoCierre = () => {
            let lst = [];
            divMotivoRechazo.find(".motivoRechazo").each((index, input) => {
                let cat = $(input).data();
                cat.lstAuth.find(auth => auth.authEstado === 2).motivoRechazo = input.value;
                lst.push(cat);
            });
            setGuardarObra(lst);
        }
        function setForm() {
            let { clave, descripcion, tipo, fechaArranque, lstAuth, authEstado, exiteEnkontrol } = catalogo,
                rechazo = lstAuth.find(auth => auth.authEstado === 2);
            //#region formulario
            txtFormClave.val(clave);
            txtFormDescripcion.val(descripcion);
            txtFormArranque.val(fechaArranque);
            cboFormTipo.val(tipo);
            //#endregion
            //#region estado
            divFormEstatus.html(setAuthEstado(catalogo));
            divFormEnkontrol.html(setAuthEnkontrol(catalogo));
            //#endregion
            //#region Autorizacion
            dtFormAuth.clear().draw();
            dtFormAuth.rows.add(lstAuth).draw();
            lblFormMotivoRechazo.text(rechazo !== undefined ? rechazo.motivoRechazo : "");
            //#endregion
        }
        setFormDefault = () => {
            catalogo = {
                id: 0,
                idUsuarioRegistro: 0,
                clave: "",
                descripcion: "",
                tipo: 0,
                fechaArranque: new Date().toLocaleDateString(),
                authEstado: 0,
                exiteEnkontrol: false,
                esActivo: true,
                puedeDarVobo: false,
                puedeDarAuth: false,
                puedeDarEnkoltrol: false,
                lstAuth: [{
                    id: 0,
                    idCatalogo: 0,
                    idUsuario: 4,
                    orden: 1,
                    nombre: "JOSE MANUEL GAYTAN LIZAMA",
                    motivoRechazo: "",
                    authEstado: 3,
                },
                {
                    id: 0,
                    idCatalogo: 0,
                    idUsuario: 1073,
                    orden: 2,
                    nombre: "EDGAR ARTURO SANCHEZ ORDUNO",
                    motivoRechazo: "",
                    authEstado: 0,
                }]
            };
            setForm();
        }
        setAuthEstado = ({ authEstado, puedeDarVobo, puedeDarAuth }) => {
            if (lstAuthEstado.length == 0) {
                lstAuthEstado = $("#cboAuth option").map((index, opt) => ({
                    Value: +opt.value,
                    Text: opt.text,
                    Prefijo: ''
                })).toArray();
            }
            if (puedeDarVobo || puedeDarAuth) {
                let optVobo = lstAuthEstado.filter(opt => opt.Value != 0);
                cboVobo = $("<select>", {
                    class: `form-control ${puedeDarVobo ? "darVobo" : ""} ${puedeDarAuth ? "darAuth" : ""}`
                });
                cboVobo.fillComboItems(optVobo, undefined, 3);
                return cboVobo[0].outerHTML;
            } else {
                let auth = lstAuthEstado.find(auth => auth.Value === authEstado),
                    ico = $("<i>");
                switch (authEstado) {
                    case 0:
                        ico.addClass("fas fa-stopwatch text-warning");
                        break;
                    case 1:
                        ico.addClass("fas fa-check text-success");
                        break;
                    case 2:
                        ico.addClass("fas fa-ban text-danger");
                        break;
                    case 3:
                        ico.addClass("fas fa-mitten text-primary");
                        break;
                    default:
                        break;
                }
                return `${ico[0].outerHTML} ${auth.Text}`;
            }
        }
        setAuthEnkontrol = ({ exiteEnkontrol, puedeDarEnkoltrol }) => {
            if (puedeDarEnkoltrol) {
                let lblEnk = $("<label>", {
                    text: "Dar alta enkontrol "
                }),
                    cbEnk = $("<input>", {
                        type: "checkbox",
                        class: "darEnk"
                    });
                lblEnk.append(cbEnk);
                return lblEnk[0].outerHTML;
            } else if (exiteEnkontrol) {
                return `<i class="fas fa-check text-success"></i> Existe en enkontrol`;
            } else {
                return `<i class="fas fa-ban text-danger"></i> No hay registro en enkotrol`;
            }
        }
        setMdlRechazo = lstCatalogo => {
            divMotivoRechazo.empty();
            lstCatalogo.filter(cat => cat.authEstado == 2).forEach(cat => {
                let fieldset = $("<fieldset>"),
                    legend = $("<legend>", {
                        class: "legend-custm",
                        text: `${cat.clave} ${cat.descripcion}`
                    }),
                    textarea = $("<textarea>", {
                        class: "form-control motivoRechazo",
                        value: cat.motivoRechazo,
                        placeholder: `Escriba la razón de rechazo para ${cat.clave} ${cat.descripcion}`
                    });
                textarea.data(cat);
                fieldset.append(legend);
                fieldset.append(textarea);
                divMotivoRechazo.append(fieldset);
            });
            $('#mdlFormMotivoRehazo').modal('show');
        }
        //#endregion
        //#region init
        function initDataTblFormAuth() {
            dtFormAuth = tblFormAuth.DataTable({
                destroy: true
                , ordering: false
                , paging: false
                , ordering: false
                , searching: false
                , bFilter: true
                , info: false
                , language: dtDicEsp
                , columns: [
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'authEstado', title: 'Estado', render: (data, type, row, meta) => setAuthEstado(row) },
                ]
                , initComplete: function (settings, json) {
                }
            });
        }
        //#endregion
        init();
    }
    $(document).ready(() => {
        Administrativo.ControlInterno.Obra._formObra = new _formObra();
    })
        .ajaxStart(() => {
            $.blockUI({
                baseZ: 1000,
                message: 'Procesando...',
            });
        })
        .ajaxStop(() => { $.unblockUI(); });
})();