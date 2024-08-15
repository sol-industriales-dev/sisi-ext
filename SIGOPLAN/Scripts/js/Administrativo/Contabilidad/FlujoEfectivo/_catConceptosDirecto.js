(() => {
$.namespace('Administrativo.FlujoEfectivo._catConceptosDirecto');
    _catConceptosDirecto = function (){
        const selDirTm = $('#selDirTm');
        const selDirCpto = $('#selDirCpto');
        const txtDirDesc = $('#txtDirDesc');
        const selDirPadre = $('#selDirPadre');
        const chbDirActivo = $('#chbDirActivo');
        const btnDirGuardar = $('#btnDirGuardar');
        const btnDirLimpiar = $('#btnDirLimpiar');
        const tblDirConcepto = $('#tblDirConcepto');
        const inputGroupBtn = $('.input-group-btn');
        const getCatConceptoDir = new URL(window.location.origin + '/Administrativo/FlujoEfectivo/getCatConceptoDir');
        const guardarConceptoDir = new URL(window.location.origin + '/Administrativo/FlujoEfectivo/guardarConceptoDir');
        const getComboTipoMovimiento = new URL(window.location.origin + '/Administrativo/Poliza/getComboTipoMovimiento');
        const getCboPadreConceptoDir = new URL(window.location.origin + '/Administrativo/FlujoEfectivo/getCboPadreConceptoDir');
        const getCboOperadorConcepto = new URL(window.location.origin + '/Administrativo/FlujoEfectivo/getCboOperadorConcepto');
        let init = () => {
            initForm();
            initDataTblDirConcepto();
            setCatConcepto();
            esPadre();
            selDirPadre.change(esPadre);
            btnDirLimpiar.click(setDefault);
            btnDirGuardar.click(setGuardado);
            txtDirDesc.change(validaConcepto);
            inputGroupBtn.click(chngSetAllSelOpt);
        }
        async function setCatConcepto(){
            try {
                dtDirConcepto.clear().draw();
                response = await ejectFetchJson(getCatConceptoDir);
                if (response.success) {
                    dtDirConcepto.rows.add(response.lst).draw();
                }
            } catch (o_O) { AlertaGeneral("Aviso",o_O.message);}
        }
        async function setGuardado(){
            try {
                var obj = getForm();
                if (obj.Concepto.trim().length > 0) {
                    response = await ejectFetchJson(guardarConceptoDir, {obj , tm: selDirTm.val()});
                    if (response.success) {
                        setCatConcepto();
                        AlertaGeneral("Aviso", "Concepto guardado con éxito.");
                        initForm();
                    }   
                }
                else {
                    txtDirDesc.addClass('errorClass');
                }
            } catch (o_O) { AlertaGeneral("Aviso",o_O.message);}
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
        function initDataTblDirConcepto() {
            dtDirConcepto = tblDirConcepto.DataTable({
                destroy: true,
                paging: false,
                ordering:false,
                searching: false,
                bFilter: true,
                language: dtDicEsp,
                ordening: [],
                columns: [
                    { data: 'padre' ,visible: false}
                   ,{ data: 'Concepto' , render: (data, type, row, meta) => `(${row.operador}) ${data}`}
                   ,{ data: 'tm' ,render: data => data.map(tm => tm.Text.replace(/\d+/g,'')).join(",") }
                   ,{
                       data: 'esActivo' ,createdCell: (td, data) => {
                           $(td).html(data ? "Activo" : "No activo");
                           $(td).addClass(data ? `success` : 'danger');
                       }
                   }
                ]
                ,drawCallback: function ( settings ) {
                    var api = this.api();
                    var rows = api.rows( {page:'current'} ).nodes();
                    var last = null;         
                    api.column(api.data().padre, {page:'current'} ).data().each( function ( group, i ) {
                        if ( last !== group ) {
                            $(rows).eq( i ).before(`<tr class="group warning"><td colspan="3">(=) ${group}</td></tr>`);         
                            last = group;
                        }
                    })
                }
                ,initComplete: function (settings, json) {
                    tblDirConcepto.on('click', 'tr:not(.group)', function (event) {
                        let data = dtDirConcepto.row(this).data();
                        setForm(data);
                    });
                }
            });
        }
        function validaConcepto() {
            validarCampo(txtDirDesc);
        }
        function esPadre() {
            let valor = selDirPadre.val()
               ,esConcepto = valor === "Nuevo";
            selDirTm.find(`option`).prop("disabled", esConcepto);
            selDirCpto.prop("disabled", esConcepto);
            selDirCpto.val(esConcepto ? "=" : "-");
        }
        function setDefault() {
            setForm({
                id: 0
               ,Concepto: ""
               ,idpadre: "Nuevo"
               ,operador: "="
               ,esActivo: true
               ,tm: []
            });
        }
        function setForm({id ,Concepto ,idPadre ,tm ,esActivo, operador}) {
            btnDirGuardar.data().id = id;
            txtDirDesc.val(Concepto);
            selDirPadre.val(idPadre);
            selDirCpto.val(operador);
            chbDirActivo[0].checked = esActivo;
            selDirTm.val(tm.map(mov => mov.Value));
            convertToMultiselect(selDirTm);
        }
        function getForm() {
            return {
                id: btnDirGuardar.data().id
               ,Concepto: txtDirDesc.val()
               ,idPadre: selDirPadre.val()
               ,esActivo: chbDirActivo[0].checked
               ,operador: selDirCpto.val()
            };
        }
        function initForm () {
            selDirCpto.fillCombo(getCboOperadorConcepto, null, true, null);
            selDirPadre.fillCombo(getCboPadreConceptoDir, null, false, "Nuevo");
            selDirTm.fillCombo(getComboTipoMovimiento, {iSistema: "B"}, true, null);
            convertToMultiselect(selDirTm);
            setDefault();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.FlujoEfectivo._catConceptosDirecto = new _catConceptosDirecto();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();