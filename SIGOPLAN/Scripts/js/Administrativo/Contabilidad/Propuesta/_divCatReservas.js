(() => {
$.namespace('Administrativo.Contabilidad.Propuesta._divCatReservas');
    _divCatReservas = function (){
        const txtCatResHex = $('#txtCatResHex');
        const selCatResTipo = $('#selCatResTipo');
        const inputGroupBtn = $('.input-group-btn');
        const tblCatReservas = $('#tblCatReservas');
        const txtCatResColor = $('#txtCatResColor');
        const selCatResAutCc = $('#selCatResAutCc');
        const selCatResAutTp = $('#selCatResAutTp');
        const selCatResAutTm = $('#selCatResAutTm');
        const selCatResSelCc = $('#selCatResSelCc');
        const selCatResSelTp = $('#selCatResSelTp');
        const selCatResSelTm = $('#selCatResSelTm');
        const chbCatResEstado = $('#chbCatResEstado');
        const btnCatResGuardar = $('#btnCatResGuardar');
        const btnCatResLimpiar = $('#btnCatResLimpiar');
        const catResPorcentaje = $('.catResPorcentaje');
        const chbCatResAutomatico = $('#chbCatResAutomatico');
        const txtCatResDescripcion = $('#txtCatResDescripcion');
        const selCatResSelTipoCalc = $('#selCatResSelTipoCalc');
        const selCatResAutTipoCalc = $('#selCatResAutTipoCalc');
        const chbCatResSeleccionado = $('#chbCatResSeleccionado');
        const txtCatResSelProcentaje = $('#txtCatResSelProcentaje');
        const txtCatResAutProcentaje = $('#txtCatResAutProcentaje');
        let init = () => {
            initForm();
            txtCatResHex.change(cngTxtCatResHex);
            inputGroupBtn.click(chngSetAllSelOpt);
            txtCatResColor.change(chngTxtResColor);
            btnCatResGuardar.click(clckGuardarCatRes);
            btnCatResLimpiar.click(clkBtnCatResLimpiar);
            catResPorcentaje.change(chngTxtCatResProcentaje);
        }
        const guardarCatReserva = new URL(window.location.origin + '/administrativo/propuesta/guardarCatReserva');
        const getLstCatReserva = new URL(window.location.origin + '/administrativo/propuesta/getLstCatReserva');
        async function clckGuardarCatRes() {
            try {
                let obj = getForm();
                    if (esFormValido(obj)) {
                    response = await ejectFetchJson(guardarCatReserva, obj);
                    if (response.success) {
                        AlertaGeneral("Aviso", "Reserva guardada con éxito.");
                        setTblCatReserva();
                    }   
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        async function setTblCatReserva() {
            try {
                dtCatReservas.clear().draw();
                response = await ejectFetchJson(getLstCatReserva);
                if (response.success) {
                    dtCatReservas.rows.add(response.lst).draw();
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        function chngTxtCatResProcentaje() {
            let txtPorcen = $(this)
               ,por = +(txtPorcen.val());
            if (por <= 0) {
                por == 100;
            }
            txtPorcen.val(maskNumero2D(por));
        }
        function chngTxtResColor() {
            txtCatResHex.val(txtCatResColor.val());
        }
        function cngTxtCatResHex() {
            txtCatResColor.val(txtCatResHex.val());
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
        function clkBtnCatResLimpiar() {

            setForm({catReserva:{
                id: 0
               ,descripcion: ""
               ,tipoReservaSaldoGlobal: 0
               ,esActivo: false
               ,esSeleccionado: false
               ,esAutomatico: false
               ,hexColor: "#219cd8"
               ,esPrincipal: false
            }
            ,lstCc:[] 
            ,lstTp:[] 
            ,lstTm:[] 
            ,lstCalc:[]});
        }
        function getForm(){
            let id = txtCatResDescripcion.data().id
               ,{lstCc ,lstTm ,lstTp ,lstCalc} = [];
            lstCc = selCatResSelCc.val().map(x => ({cc: x ,idCatReserva: id ,idTipoProrrateo: 1}));
            lstCc = lstCc.concat(...(selCatResAutCc.val().map(x => ({cc: x ,idCatReserva: id ,idTipoProrrateo: 2}))));
            lstTm = selCatResSelTm.val().map(x => ({tm: x ,idCatReserva: id, idTipoProrrateo: 1 ,generado:"B"}));
            lstTm = lstTm.concat(...(selCatResAutTm.val().map(x => ({tm: x ,idCatReserva: id, idTipoProrrateo: 2,generado:"B"}))));
            lstTp = selCatResSelTp.val().map(x => ({tp: x ,idCatReserva: id, idTipoProrrateo: 1}));
            lstTp = lstTp.concat(...(selCatResAutTp.val().map(x => ({tp: x ,idCatReserva: id, idTipoProrrateo: 2}))));
            lstCalc = [
                {idTipoCalculo: selCatResSelTipoCalc.val() ,porcentaje: txtCatResSelProcentaje.val() ,idTipoProrrateo:1}
               ,{idTipoCalculo: selCatResAutTipoCalc.val() ,porcentaje: txtCatResAutProcentaje.val() ,idTipoProrrateo:2}
            ];
            return {
                 catReserva:{
                     id: id
                    ,descripcion: txtCatResDescripcion.val()
                    ,tipoReservaSaldoGlobal: +(selCatResTipo.val())
                    ,esSeleccionado: chbCatResSeleccionado.prop("checked")
                    ,esAutomatico: chbCatResAutomatico.prop("checked")
                    ,esActivo: chbCatResEstado.prop("checked")
                    ,hexColor: txtCatResHex.val()
                    ,esPrincipal: chbCatResEstado.prop("disabled")
                }
                ,lstCc
                ,lstTm
                ,lstTp
                ,lstCalc
            };
        }
        function setForm(data) {
            let {id, descripcion ,tipoReservaSaldoGlobal ,esActivo ,esSeleccionado ,esAutomatico ,hexColor ,esPrincipal} = data.catReserva
                ,{lstCc ,lstTm ,lstTp ,lstCalc} = data
                ,relCondiciones = [
                    {lst: lstCc.filter(cc => cc.idTipoProrrateo === 1).map(x => x.cc) ,sel:selCatResSelCc}
                   ,{lst: lstTm.filter(tm => tm.idTipoProrrateo === 1).map(x => x.tm) ,sel:selCatResSelTm}
                   ,{lst: lstTp.filter(tm => tm.idTipoProrrateo === 1).map(x => x.tp) ,sel:selCatResSelTp}
                   ,{lst: lstCc.filter(cc => cc.idTipoProrrateo === 2).map(x => x.cc) ,sel:selCatResAutCc}
                   ,{lst: lstTm.filter(cc => cc.idTipoProrrateo === 2).map(x => x.tm) ,sel:selCatResAutTm}
                   ,{lst: lstTp.filter(cc => cc.idTipoProrrateo === 2).map(x => x.tp) ,sel:selCatResAutTp}
                ]
                ,objSel = lstCalc.find(cal => cal.idTipoProrrateo === 1)
                ,objAut = lstCalc.find(cal => cal.idTipoProrrateo === 2);
            txtCatResDescripcion.data().id = id;
            txtCatResHex.val(hexColor);
            txtCatResColor.val(hexColor);
            txtCatResDescripcion.val(descripcion);
            selCatResTipo.val(tipoReservaSaldoGlobal);
            chbCatResEstado.prop("checked", esActivo);
            chbCatResEstado.prop("disabled", esPrincipal);
            chbCatResSeleccionado.prop("checked", esSeleccionado);
            chbCatResAutomatico.prop("checked", esAutomatico);
            if (objSel === undefined) {
                selCatResSelTipoCalc.val("");
                txtCatResSelProcentaje.val(maskNumero2D(100));
            } else{
                selCatResSelTipoCalc.val(objSel.idTipoCalculo);
                txtCatResSelProcentaje.val(maskNumero2D(objSel.porcentaje));
            }
            if (objAut === undefined) {
                selCatResAutTipoCalc.val("");
            txtCatResAutProcentaje.val(maskNumero2D(100));
            }else {
                selCatResAutTipoCalc.val(objAut.idTipoCalculo);
                txtCatResAutProcentaje.val(maskNumero2D(objAut.porcentaje));
            }
            relCondiciones.forEach(response => {
                limpiarMultiselect(response.sel);
                if (response.lst.length > 0) {
                    response.sel.val(response.lst);
                    convertToMultiselect(response.sel);
                }
            });
        }
        function esFormValido({catReserva ,lstCc ,lstTm ,lstTp ,lstCalc}){
            let tieneDescripcion = catReserva.descripcion.length > 0
               ,esCondicion = false
               ,esCc = false
               ,esTm = false
               ,esTp = false
               ,esCalc = false;
            if (!catReserva.esAutomatico && !catReserva.esProrrateo) {
                esCondicion = true;
            } else if (catReserva.esAutomatico || catReserva.esProrrateo) {
                esCc = lstCc.length > 0;
                esTm = lstTm.length > 0;
                esTp = lstTp.length > 0;
                esCalc = lstCalc.length > 0;
                esCondicion = esCc && esTm && esTp && esCalc;
            }
            let esvalido = tieneDescripcion && esCondicion;;
            if (!esvalido) {
                let txt = `${tieneDescripcion?'':'No tiene descripción. '}
                            No seleccionó 
                           ${esCc?'':'centro de costos. '}
                           ${esTp?'':'tipo de póliza. '}
                           ${esTm?'':'tipo de movimiento. '}
                           ${esCalc?'':'tipo de cálculo. '}`;
                AlertaGeneral("Aviso",txt);
            }
            return esvalido;
        }
        function initDataTblCatReservas() {
            dtCatReservas = tblCatReservas.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    { data: 'catReserva.descripcion'},
                    { data: 'catReserva.esSeleccionado', createdCell: (td, data) => { $(td).html(data ? "Sí" : "No"); } },
                    { data: 'catReserva.esAutomatico', createdCell: (td, data) => { $(td).html(data ? "Sí" : "No"); } },
                    { data: 'catReserva.hexColor' ,sClass:'text-right' ,createdCell: (td ,data) => { $(td).css(`background-color`, `$${data}`); } },
                    { data: 'catReserva.tipoReservaSaldoGlobal', createdCell: (td, data) => { $(td).html(selCatResTipo.find(`option[value='${data}']`).text()); } },
                    { data: 'catReserva.esActivo', createdCell: (td, data) => { $(td).html(data ? "Activo" : "No Activo"); } },
                ]
                ,initComplete: function (settings, json) {
                    tblCatReservas.find('tbody').on('click', 'tr', function () {
                        let data = dtCatReservas.row(this).data();
                        if (data !== undefined) {
                            setForm(data);   
                        }
                    });
                }
            });
        }
        function initForm() {
            selCatResAutCc.fillCombo('/Administrativo/Propuesta/getCC', null, true, null);
            selCatResSelCc.fillCombo('/Administrativo/Propuesta/getCC', null, true, null);
            selCatResAutTp.fillCombo('/Administrativo/Poliza/getComboTipoPoliza', null, true, null);
            selCatResSelTp.fillCombo('/Administrativo/Poliza/getComboTipoPoliza', null, true, null);
            selCatResSelTipoCalc.fillCombo('/Administrativo/Propuesta/cboCatCalculoRes', null, false, null);
            selCatResAutTipoCalc.fillCombo('/Administrativo/Propuesta/cboCatCalculoRes', null, false, null);
            selCatResTipo.fillCombo('/Administrativo/Propuesta/cboCatResTipoSaldoGlobal', null, false, null);
            selCatResSelTm.fillCombo('/Administrativo/Poliza/getComboTipoMovimiento', {iSistema: "B"}, true, null);
            selCatResAutTm.fillCombo('/Administrativo/Poliza/getComboTipoMovimiento', {iSistema: "B"}, false, null);
            convertToMultiselect(selCatResSelCc);
            convertToMultiselect(selCatResSelTp);
            convertToMultiselect(selCatResSelTm);
            convertToMultiselect(selCatResAutCc);
            convertToMultiselect(selCatResAutTp);
            convertToMultiselect(selCatResAutTm);
            initDataTblCatReservas();
            setTblCatReserva();
            inputGroupBtn.val(false);
            clkBtnCatResLimpiar();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta._divCatReservas = new _divCatReservas();
    })
    .ajaxStart(() => { $.blockUI({ 
        message: 'Procesando...' 
       ,baseZ: 100000,
    }); })
    .ajaxStop(() => { $.unblockUI(); });
})();