(() => {
    $.namespace('Administrativo.FlujoEfectivoArrendadora._catConceptos');
    _catConceptos = function () {
        const selcatTm = $('#selcatTm');
        const txtCarDesc = $('#txtCarDesc');
        const selCatPadre = $('#selCatPadre');
        const chbCatActivo = $('#chbCatActivo');
        const btnCatLimpiar = $('#btnCatLimpiar');
        const btnCatGuardar = $('#btnCatGuardar');
        const tblCatConcepto = $('#tblCatConcepto');
        const inputGroupBtn = $('.input-group-btn');
        const getCatConcepto = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getCatConcepto');
        const guardarConcepto = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/guardarConcepto');
        const getComboTipoMovimiento = new URL(window.location.origin + '/Administrativo/Poliza/getComboTipoMovimiento');
        const getCboPadreConcepto = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getCboPadreConcepto');
        let init = () => {
            initForm();
            initDataTblCatConcepto();
            setCatConcepto();
            esPadre();
            selCatPadre.change(esPadre);
            btnCatLimpiar.click(setDefault);
            btnCatGuardar.click(setGuardado);
            txtCarDesc.change(validaConcepto);
            inputGroupBtn.click(chngSetAllSelOpt);
        }
        async function setCatConcepto() {
            try {
                dtCatConcepto.clear().draw();
                response = await ejectFetchJson(getCatConcepto);
                if (response.success) {
                    dtCatConcepto.rows.add(response.lst).draw();
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        async function setGuardado() {
            try {
                var obj = getForm();
                if (obj.Concepto.trim().length > 0) {
                    response = await ejectFetchJson(guardarConcepto, { obj, tm: selcatTm.val() });
                    if (response.success) {
                        setCatConcepto();
                        AlertaGeneral("Aviso", "Concepto guardado con éxito.");
                        initForm();
                    }
                }
                else {
                    txtCarDesc.addClass('errorClass');
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        function chngSetAllSelOpt() {
            let estodo = !this.value,
                select = $(this).next().find("select");
            this.value = estodo;
            limpiarMultiselect(select);
            if (estodo) {
                let lstValor = $(`#${select.attr("id")}`).find(`option`).toArray().map(option => option.value);
                select.val(lstValor);
                convertToMultiselect(select);
            }
        }
        function initDataTblCatConcepto() {
            dtCatConcepto = tblCatConcepto.DataTable({
                destroy: true,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: true,
                language: dtDicEsp,
                ordening: [],
                columns: [
                    { data: 'padre', visible: false }
                    , { data: 'Concepto' }
                    , { data: 'tm', render: data => data.map(tm => tm.Text.replace(/\d+/g, '')).join(",") }
                    , {
                        data: 'esActivo', createdCell: (td, data) => {
                            $(td).html(data ? "Activo" : "No activo");
                            $(td).addClass(data ? `success` : 'danger');
                        }
                    }
                ]
                , drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;
                    api.column(api.data().padre, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group warning"><td colspan="3">' + group + '</td></tr>'
                            );
                            last = group;
                        }
                    })
                }
                , initComplete: function (settings, json) {
                    tblCatConcepto.on('click', 'tr:not(.group)', function (event) {
                        let data = dtCatConcepto.row(this).data();
                        setForm(data);
                    });
                }
            });
        }
        function validaConcepto() {
            validarCampo(txtCarDesc);
        }
        function esPadre() {
            let valor = selCatPadre.val()
                , esConcepto = valor === "Nuevo";
            selcatTm.find(`option`).prop("disabled", esConcepto);
        }
        function setDefault() {
            setForm({
                id: 0
                , Concepto: ""
                , idpadre: "Nuevo"
                , esActivo: true
                , tm: []
            });
        }
        function setForm({ id, Concepto, idPadre, tm, esActivo }) {
            btnCatGuardar.data().id = id;
            txtCarDesc.val(Concepto);
            selCatPadre.val(idPadre);
            chbCatActivo[0].checked = esActivo;
            selcatTm.val(tm.map(mov => mov.Value));
            convertToMultiselect(selcatTm);
        }
        function getForm() {
            return {
                id: btnCatGuardar.data().id
                , Concepto: txtCarDesc.val()
                , idPadre: selCatPadre.val()
                , esActivo: chbCatActivo[0].checked
            };
        }
        function initForm() {
            selCatPadre.fillCombo(getCboPadreConcepto, null, false, "Nuevo");
            selcatTm.fillCombo(getComboTipoMovimiento, { iSistema: "B" }, true, null);
            convertToMultiselect(selcatTm);
            setDefault();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.FlujoEfectivoArrendadora._catConceptos = new _catConceptos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();