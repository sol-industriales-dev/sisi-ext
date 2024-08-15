(() => {
$.namespace('Administrativo.Banco._divBancoMov');
    _divBancoMov = function (){
        const tblBancMov = $('#tblBancMov');
        const dpBanMovMin = $('#dpBanMovMin');
        const dpBanMovMax = $('#dpBanMovMax');
        const selBanMovCc = $('#selBanMovCc');
        const selBanMovTm = $('#selBanMovTm');
        const selBanMovTp = $('#selBanMovTp');
        const btnBanMovXlm = $('#btnBanMovXlm');
        const selBanMovCta = $('#selBanMovCta');
        const btnBanMovBusq = $('#btnBanMovBusq');
        const btnBanMovRepo = $('#btnBanMovRepo');
        const inputGroupBtn = $('.input-group-btn');
        const selBanMovFormato = $('#selBanMovFormato');
        let init = () => {
            initForm();
            selBanMovFormato.change(setFormatTable);
            btnBanMovBusq.click(setLstBanMov);
            inputGroupBtn.click(chngSetAllSelOpt);
            btnBanMovXlm.click(exportXmlBanMov);
            inputGroupBtn.each((i ,btn) => {
                $(btn).click();
            });
            setLstBanMov();
        }
        const exportBanMov = '/Administrativo/Banco/exportBanMov';
        const getLstBancoMov = new URL(window.location.origin + '/Administrativo/Banco/getLstBancoMov');
        exportXmlBanMov = () => exportUrlToFile(exportBanMov);
        async function setLstBanMov() {
            try {
                dtBancMov.clear();
                let busq = getBanMovForm();
                response = await ejectFetchJson(getLstBancoMov, busq);
                if (response.success) {
                    setFormatTable();
                    dtBancMov.rows.add(response.lst).draw();
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message)}
        }
        function getBanMovForm(){
            return {
                min: dpBanMovMin.val()
               ,max: dpBanMovMax.val()
               ,lstCc: selBanMovCc.val()
               ,lstCta: selBanMovCta.val()
               ,lstTm: selBanMovTm.val()
               ,lstTp: selBanMovTp.val()
               ,formato: +(selBanMovFormato.val())
            };
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
        function initDataTblBancMov() {
            dtBancMov = tblBancMov.DataTable({
                order: []
               ,destroy: true
               ,ordering:false
               ,language: dtDicEsp
               ,drawCallback: function (setting) {
                   switch (selBanMovFormato.val()) {
                       case "1": break;
                       case "2": break;
                       case "3": headResumen(this); break;
                       case "4": break;
                       default:
                           break;
                   }
               }
               ,columns: [
                    { data: 'banDesc'}
                   ,{ data: 'ctaDesc'}
                   ,{ data: 'fecha_mov' ,width:'1%' ,createdCell: (td, data) => { $(td).html($.toDate(data)) } }
                   ,{ data: 'numero' ,width:'1%' ,class:'text-right' ,createdCell: (td, data) => { $(td).html(("000000"+data).slice(-6)) }}
                   ,{ data: 'tmDesc'}
                   ,{ data: 'ccDesc' ,width:'18%' ,createdCell: (td, data, rowData) => { $(td).html(`${rowData.cc} - ${data}`) } }
                   ,{ data: 'descripcion'}
                   ,{ data: 'cargo' ,width:'1%' ,class:'text-right' ,createdCell: (td, data) => { $(td).html(maskNumero(data)) }}
                   ,{ data: 'abono' ,width:'1%' ,class:'text-right' ,createdCell: (td, data) => { $(td).html(maskNumero(data)) }}
                   ,{ data: 'stDesc' ,width:'7%'}
                   ,{ data: 'ipoliza',width:'1%' ,class:'text-right'}
                   ,{ data: 'tpDesc' ,createdCell: (td, data, rowData) => { $(td).html(`${rowData.itp} - ${data}`) } }
                   ,{ data: 'ilinea' ,width:'1%' ,class:'text-right'}
               ]
            });
        }
        function headResumen(setting) {
            let api = setting.api(),
                rows = api.rows({ page: 'current' }).nodes(),
                last = null;
            api.column({ page: 'current' }).data().each(function (group, i, dtable) {
                const data = dtable.data()[i];
                if (last !== data.ctaDesc) {
                    $(rows).eq(i).before(`<tr>
                                            <td colspan="3">${data.ctaDesc}</td>
                                            <td>${data.cargo}</td>
                                            <td>${data.abono}</td>
                                        </tr>`);
                    last = data.ctaDesc;
                }
            });
        }
        function setFormatTable() {
            let lstCol = [];
            dtBancMov.columns().visible(true);
            switch (selBanMovFormato.val()) {
                case "1": lstCol.push(0); break;
                case "2": lstCol.push(4 ,5); break;
                case "3": lstCol.push(2 ,3 ,5 ,6 ,9 ,10 ,11 ,12); break;
                case "4": lstCol.push(0 ,1 ,2 ,3 ,6 ,9 ,10 ,11 ,12); break;
                default: break;
            }
            lstCol.forEach(col => dtBancMov.columns(col).visible(false));
            dtBancMov.draw();
        }

        function initForm() {
            let hoy = new Date();
            dpBanMovMax.datepicker().datepicker('setDate', hoy);
            dpBanMovMin.datepicker().datepicker('setDate', new Date(hoy.getFullYear(), hoy.getMonth(), 1));
            selBanMovCc.fillCombo('/Administrativo/ReportesRH/FillComboCC', null, true, null);
            selBanMovTm.fillCombo('/Administrativo/Poliza/getComboTipoMovimiento', {iSistema: "B"}, true, null);
            selBanMovTp.fillCombo('/Administrativo/Poliza/getComboTipoPoliza', null, true, null);
            selBanMovCta.fillCombo('/Administrativo/CcDivision/cboCuentaBanco', null, true, null);
            selBanMovFormato.fillCombo('/Administrativo/Banco/cboFormatoBanMov', null, true, null);
            convertToMultiselect(selBanMovCc);
            convertToMultiselect(selBanMovCta);
            convertToMultiselect(selBanMovTp);
            convertToMultiselect(selBanMovTm);
            initDataTblBancMov();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Banco._divBancoMov = new _divBancoMov();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();