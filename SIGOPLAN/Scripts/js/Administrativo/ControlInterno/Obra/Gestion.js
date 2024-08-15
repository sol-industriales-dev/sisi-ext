(() => {
    $.namespace('Administrativo.ControlInterno.Obra.Gestion');
    Gestion = function () {
        let todos = "TODOS", lstTipo = [];
        //#region Elementos HTML
        const cboCC = $('#cboCC');
        const cboAC = $('#cboAC');
        const btnEnk = $('#btnEnk');
        const cboAuth = $('#cboAuth');
        const cboTipo = $('#cboTipo');
        const btnVobo = $('#btnVobo');
        const btnAuth = $('#btnAuth');
        const tblObra = $('#tblObra');
        const botonera = $('.botonera');
        const btnBuscar = $('#btnBuscar');
        const mdlFormObra = $('#mdlFormObra');
        //#endregion
        //#region urls
        const getCboObra = new URL(window.location.origin + '/Administrativo/Obra/getCboObra');
        const getTblObras = new URL(window.location.origin + '/Administrativo/Obra/getTblObras');
        //#endregion
        let init = () => {
            initForm()
            btnEnk.click(setGestionCatalogo);
            btnAuth.click(setGestionCatalogo);
            btnVobo.click(setGestionCatalogo);
            btnBuscar.click(setTblObras);
            mdlFormObra.on('show.bs.modal', function (e) {
                setFormDefault();
            });
        };
        //#region http
        async function setCboObra() {
            try {
                response = await ejectFetchJson(getCboObra);
                let { success, optionCC, optionAC, optionTipo, optionAuth } = response;
                lstTipo = optionTipo;
                cboTipo.fillComboItems(optionTipo, undefined);
                cboTipo.val([1]);
                convertToMultiselect('#cboTipo');
                cboAuth.fillComboItems(optionAuth, undefined);
                cboAuth.val([0, 1, 2, 3]);
                convertToMultiselect('#cboAuth');
                cboCC.fillComboItems(optionCC, todos);
                cboCC.find(`option[value=""]`).prop("value", todos);
                cboAC.fillComboItems(optionAC, todos);
                cboAC.find(`option[value=""]`).prop("value", todos);
                $(".select2").select2();
                setTblObras();
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        setTblObras = async () => {
            try {
                let busq = getBusqForm();
                dtObra.clear().draw();
                botonera.addClass("hidden");
                response = await ejectFetchJson(getTblObras, busq);
                let { success, lst } = response;
                if (success) {
                    setBotonera(lst);
                    dtObra.rows.add(lst).draw();
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        //#endregion
        //#region funciones eventos
        function setGestionCatalogo() {
            let lst = [];
            dtObra.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let row = $(this.node()),
                    data = this.data();
                if (data.puedeDarVobo || data.puedeDarAuth || data.puedeDarEnkoltrol) {
                    data.row = row;
                    lst.push(data);
                }
            });
            getCatalogoData(lst);
        }
        //#endregion
        //#region cargar/asignar elementos
        function getBusqForm() {
            return {
                lstTipo: cboTipo.val(),
                lstAuth: cboAuth.val(),
                lstCC: cboCC.val(),
                lstAC: cboAC.val()
            };
        }
        function initForm() {
            initDataTblObra();
            setCboObra();
        }
        function setBotonera(lst) {
            let puedeDarVobo = puedeDarAuth = puedeDarEnkoltrol = false;
            lst.forEach(catalogo => {
                if (catalogo.puedeDarVobo) {
                    puedeDarVobo = true;
                }
                if (catalogo.puedeDarAuth) {
                    puedeDarAuth = true;
                }
                if (catalogo.puedeDarEnkoltrol) {
                    puedeDarEnkoltrol = true;
                }
            });
            if (puedeDarVobo) {
                btnVobo.removeClass("hidden");
            }
            if (puedeDarAuth) {
                btnAuth.removeClass("hidden");
            }
            if (puedeDarEnkoltrol) {
                btnEnk.removeClass("hidden");
            }
        }
        function setTipoDescripcion(tipo) {
            let descripcion = lstTipo.find(option => option.Value == tipo).Text,
                ico = $("<i>");
            switch (tipo) {
                case 1:
                    ico.addClass("fas fa-monument text-primary");
                    break;
                case 2:
                    ico.addClass("fas fa-tractor text-warning");
                    break;
                case 3:
                    ico.addClass("fas fa-clipboard text-default");
                    break;
                case 4:
                    ico.addClass("fas fa-money-check text-success");
                    break;
                default:
                    break;
            }
            return `${ico[0].outerHTML} ${descripcion}`;
        }
        //#endregion
        //#region init
        function initDataTblObra() {
            dtObra = tblObra.DataTable({
                destroy: true
                , language: dtDicEsp
                , columns: [
                    { data: 'clave', title: 'Clave', width: '5%' }
                    , { data: 'descripcion', title: 'Descripcion' }
                    , { data: 'tipo', title: "Tipo", width: '18%', render: (data, type, row, meta) => setTipoDescripcion(data) }
                    , { data: 'fechaArranque', title: 'Arranque', width: '10%', render: (data, type, row, meta) => $.toDate(data) }
                    , { data: 'authEstado', title: 'Estado', width: '10%', render: (data, type, row, meta) => setAuthEstado(row) }
                    , { data: 'exiteEnkontrol', width: '17%', render: (data, type, row, meta) => setAuthEnkontrol(row) }
                    , {
                        data: 'id', width: '5%', createdCell: (td, data, row, meta) => {
                            let btn = $("<button>", {
                                class: "btn btn-success editar"
                            }),
                                ico = $("<i>", {
                                    class: "fas fa-edit"
                                });
                            btn.append(ico);
                            $(td).html(btn);
                        }
                    }
                ]
                , initComplete: function (settings, json) {
                    tblObra.on('click', '.editar', function (event) {
                        let row = $(this).closest("td"),
                            data = dtObra.row(row).data();
                        setFormDesdeClave(data);
                    });
                }
            });
        }
        //#endregion
        init();
    }
    $(document).ready(() => {
        Administrativo.ControlInterno.Obra.Gestion = new Gestion();
    })
        .ajaxStart(() => {
            $.blockUI({
                baseZ: 1000,
                message: 'Procesando...',
            });
        })
        .ajaxStop(() => { $.unblockUI(); });
})();