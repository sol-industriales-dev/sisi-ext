(() => {
$.namespace('Administracion.Contabilidad.Propuesta.ResumenProveedor');
ResumenProveedor = function (){
        const btnEnvioCorreos = $("#btnEnvioCorreos");
        const selPropCC = $('#selPropCC');
        const txtPropFac = $('#txtPropFac');
        const txtPropFOC = $('#txtPropFOC');
        const txtProvMin = $('#txtProvMin');
        const txtProvMax = $('#txtProvMax');
        const txtPropFSol = $('#txtPropFSol');
        const txtPropFPag = $('#txtPropFPag');
        const txtPropFMon = $('#txtPropFMon');
        const dpPropCorte = $('#dpPropCorte');
        const txtPropHSel = $('#txtPropHSel');
        const txtPropHSelDll = $('#txtPropHSelDll');
        const txtPropFReci = $('#txtPropFReci');
        const btnProvLimit = $('#btnProvLimit');
        const btnPropBuscar = $('#btnPropBuscar');
        const txtPropHTotal = $('#txtPropHTotal');
        const txtPropHTotalDll = $('#txtPropHTotalDll');
        const btnPropGuardar = $('#btnPropGuardar');
        const inputGroupBtn = $('.input-group-btn');
        const tblPropFacturas = $('#tblPropFacturas');
        const selPropTipoProceso = $('#selPropTipoProceso');
        const chkManual = $("#chkManual");
        let lstAutorizado = [], itemsGiro = [], dtPropFacturas ,dlldia = 1;
        let init = () => {
            initForm();
            setLstFacturasProv();
            //btnProvLimit.click(setDataLimit);
            btnPropGuardar.click(saveSplitted);
            //inputGroupBtn.click(chngSetAllSelOpt);
            //btnPropBuscar.click(setLstFacturasProv);
            //inputGroupBtn.each((i ,btn) => $(btn).click() );
            //btnEnvioCorreos.click(enviarCorreos);
            //setItemsGiro();
            //btnPropBuscar.click();
        }
        const getDolarDelDia = new URL(window.location.origin + '/Administrativo/Poliza/getDolarDelDia');
        const FillComboGiro = new URL(window.location.origin + '/Administrativo/Reportes/FillComboGiro');
        const guardarGastosProv = new URL(window.location.origin + '/Administrativo/Propuesta/guardarGastosProv_AF');
        const getLstFacturasProv = new URL(window.location.origin + '/Administrativo/Propuesta/getLstFacturasProv_activofijo');
        const getLimitNoProveedores = new URL(window.location.origin + '/Administrativo/Propuesta/getLimitNoProveedores');

        function saveSplitted(){
            let obj = getLstTblGastos();
            //var obj = {};
            //var lst = [];
            //obj.valid= true;
            //var temp = [{"loop":7,"success":false,"dataRequest":{"lst":[{"id":0,"numpro":9000,"proveedor":"EMPRESAS MATCO SA DE CV","referenciaoc":"307","cc":"CY7","centroCostos":"CY7","tm":7,"tmDesc":"7  REFACCIONES","vence":"04/09/2021","factura":"279614","saldo":20.45,"monto_plan":20.45,"concepto":"HOUSING AS","moneda":"DLL","autorizado":"A","tipocambio":null,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"estatus":"A"},{"id":0,"numpro":9000,"proveedor":"EMPRESAS MATCO SA DE CV","referenciaoc":"368","cc":"CYF","centroCostos":"CYF","tm":7,"tmDesc":"7  REFACCIONES","vence":"04/09/2021","factura":"279615","saldo":148.93,"monto_plan":148.93,"concepto":"DAMPER AS. -","moneda":"DLL","autorizado":"A","tipocambio":null,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"estatus":"A"},{"id":0,"numpro":9000,"proveedor":"EMPRESAS MATCO SA DE CV","referenciaoc":"456","cc":"AIK","centroCostos":"AIK","tm":7,"tmDesc":"7  REFACCIONES","vence":"04/09/2021","factura":"279616","saldo":317.03,"monto_plan":317.03,"concepto":"RECEIVER","moneda":"DLL","autorizado":"A","tipocambio":null,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"estatus":"A"},{"id":0,"numpro":9000,"proveedor":"EMPRESAS MATCO SA DE CV","referenciaoc":"573","cc":"AI4","centroCostos":"AI4","tm":7,"tmDesc":"7  REFACCIONES","vence":"04/09/2021","factura":"279617","saldo":957.7,"monto_plan":957.7,"concepto":"GUIA","moneda":"DLL","autorizado":"A","tipocambio":null,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"estatus":"A"},{"id":0,"numpro":9000,"proveedor":"EMPRESAS MATCO SA DE CV","referenciaoc":"117","cc":"A83","centroCostos":"A83","tm":7,"tmDesc":"7  REFACCIONES","vence":"04/09/2021","factura":"279618","saldo":1784.71,"monto_plan":1784.71,"concepto":"PIN AS","moneda":"DLL","autorizado":"A","tipocambio":null,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"estatus":"A"},{"id":0,"numpro":9000,"proveedor":"EMPRESAS MATCO SA DE CV","referenciaoc":"159","cc":"A75","centroCostos":"A75","tm":7,"tmDesc":"7  REFACCIONES","vence":"04/09/2021","factura":"279619","saldo":1981.19,"monto_plan":1981.19,"concepto":"GUIA","moneda":"DLL","autorizado":"A","tipocambio":null,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"estatus":"A"},{"id":0,"numpro":9000,"proveedor":"EMPRESAS MATCO SA DE CV","referenciaoc":"22","cc":"A87","centroCostos":"A87","tm":7,"tmDesc":"7  REFACCIONES","vence":"04/09/2021","factura":"279620","saldo":6446.21,"monto_plan":6446.21,"concepto":"ACCUMULATOR","moneda":"DLL","autorizado":"A","tipocambio":null,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"estatus":"A"},{"id":0,"numpro":9000,"proveedor":"EMPRESAS MATCO SA DE CV","referenciaoc":"273","cc":"CYM","centroCostos":"CYM","tm":7,"tmDesc":"7  REFACCIONES","vence":"04/09/2021","factura":"279621","saldo":1211.1,"monto_plan":1211.1,"concepto":"PIN","moneda":"DLL","autorizado":"A","tipocambio":null,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"estatus":"A"},{"id":0,"numpro":9000,"proveedor":"EMPRESAS MATCO SA DE CV","referenciaoc":"275","cc":"A48","centroCostos":"A48","tm":7,"tmDesc":"7  REFACCIONES","vence":"04/09/2021","factura":"279622","saldo":992.93,"monto_plan":992.93,"concepto":"ALTERNATOR G *** CARGO REMAN  427.98","moneda":"DLL","autorizado":"A","tipocambio":null,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"estatus":"A"},{"id":0,"numpro":9000,"proveedor":"EMPRESAS MATCO SA DE CV","referenciaoc":"276","cc":"259","centroCostos":"259","tm":7,"tmDesc":"7  REFACCIONES","vence":"04/09/2021","factura":"279623","saldo":100.64,"monto_plan":100.64,"concepto":"CINTA C/SO","moneda":"DLL","autorizado":"A","tipocambio":null,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"estatus":"A"}],"manual":false}}];
            //$.each(temp,function(i,e){
            //    $.each(e.dataRequest.lst,function(i2,e2){
            //        lst.push(e2);
            //    });
                
            //});
            //obj.data = lst;
            if(obj.valid)
            {
                if(obj.data.length > 0){
                    var scheme = {lst : new Array(),manual:chkManual.is(":checked")};
                    $.sm_SplittedSave(guardarGastosProv,obj.data,scheme,10,setLstFacturasProv);
                }
                else{
                    AlertaGeneral("Alerta","¡Debe seleccionar almenos un registro para continuar!");
                }
            }
            else{
                AlertaGeneral("Alerta","¡Faltaron datos por indicar los cuales fueron marcados de color rojo!");
            }
        }
        function enviarCorreos() {
            $.ajax({
                url: '/Administrativo/Reportes/enviarCorreosGerardoPropuesta',
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        AlertaGeneral('Confirmación', 'Se Enviaron los correos');
                    } else {
                        AlertaGeneral('Alerta', 'No hay factura existente');
                    }
                },
                error: function (response) {
                   // AlertaGeneral("Alerta", response.message);
                }
            });
        }
        
        async function guardarLstGastos() {
            try {
                let lst = getLstTblGastos();

                response = await ejectFetchJson(guardarGastosProv, lst);
                if (response.success) {
                    setLstFacturasProv();
                    AlertaGeneral("Aviso", "Datos guardados correctamente.");
                    
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        async function setLstFacturasProv() {
            try {
                //setDolarDelDia();
                dtPropFacturas.clear().draw();
                txtPropHSel.text(maskNumero(0));
                txtPropHTotal.text(maskNumero(0));
                txtPropHSelDll.text(maskNumero(0));
                txtPropHTotalDll.text(maskNumero(0));
                let busq =  getForm()
                ,response = await ejectFetchJson(getLstFacturasProv, busq);
             if (response.success) {
                    dtPropFacturas.rows.add(response.lst).draw();
                    txtPropHTotal.text(maskNumero(response.total));
                    txtPropHTotalDll.text(maskNumero(response.totaldll));
                    setSaldosTotales();
             }   
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message);}
        }
        async function setDolarDelDia(){
            try {
                response = await ejectFetchJson(getDolarDelDia, { fecha: dpPropCorte.val()});
                if (response.success) {
                    dlldia = response.dll;
                }
            } catch (o_O) { dlldia = 1; }
        }
        async function getLimitProv() {
            try {
                response = await ejectFetchJson(getLimitNoProveedores);
                if (response.success) {
                    txtProvMin.val(response.limit[0]);
                    txtProvMin.data().prov = response.limit[0];
                    txtProvMax.val(response.limit[1]);
                    txtProvMax.data().prov = response.limit[1];
                }   
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message);}
        }
        function setDataLimit() {
            txtProvMin.val(txtProvMin.data().prov);
            txtProvMax.val(txtProvMax.data().prov);
        }
        async function setItemsGiro (){
            response = await ejectFetchJson(FillComboGiro);
            if (response.success) {
                itemsGiro = response.items;
            }
        }
        function getLstTblGastos() {
            var obj = {};
            var lst = [];
            var valid = true;
            dtPropFacturas.rows().every(function (i) {
                let node = $(this.node())
                   ,autorizado = " "
                   ,idGiro = 0
                   ,cbo = node.find('td:eq(8) select')
                   ,chk = node.find('td:eq(9) input');
                autorizado = cbo.val();
                autorizado = autorizado === null ? " " : autorizado;
                if (chk.is(":checked") ) {
                    let data = this.data();
                    data.estatus = autorizado;
                    lst.push(data);
                }


            });
            obj.data = lst;
            obj.valid = valid;
            return obj;
        }
        function getForm(){
            return {
                fechaCorte: dpPropCorte.val()
               ,tipoProceso: selPropTipoProceso.val()
               ,min: txtProvMin.val()
               ,max: txtProvMax.val()
               ,lstCc: selPropCC.val()
               ,tipo: chkManual.is(":checked")
            };
        }
        function setFooter({cc ,referenciaoc ,saldo ,moneda}) {
            txtPropFOC.val(`${cc} - ${referenciaoc}`);
            txtPropFSol.val(maskNumero(saldo));
            txtPropFac.val(maskNumero(saldo));
            txtPropFReci.val(maskNumero(saldo));
            txtPropFPag.val(maskNumero(0));
            txtPropFMon.val(moneda);
        }
        function setSaldosTotales() {
            let sumaSaldo = 0;
            let sumaSaldoDll = 0;
            dtPropFacturas.rows().every(function (i) {
                let node = $(this.node())
                   ,data = this.data()
                   ,autorizado = " "
                   ,idGiro = 0
                   ,cbo = node.find('td:eq(9) select')
                   ,esBloq = cbo.prop('disabled');
                cbo.prop('disabled', false);
                autorizado = cbo.val();
                autorizado = autorizado === null ? " " : autorizado;
                cbo.prop('disabled', esBloq);
                //idGiro += +(node.find('td:eq(9) select').val())
                if ((autorizado === "A" || autorizado === "E")/* && idGiro > 0*/) {
                    let tc = data.tipocambio === null ? dlldia : data.tipocambio;
                    if(data.moneda=='MN'){
                        sumaSaldo += (unmaskNumero(node.find('.inputAPagar').val()));
                    }
                    else{
                        sumaSaldoDll += (unmaskNumero(node.find('.inputAPagar').val()) * tc);
                    }
                }
            });
            txtPropHSel.text(maskNumero(sumaSaldo));
            txtPropHSelDll.text(maskNumero(sumaSaldoDll));
        }
        function setSaldoProv({numpro}){
            let lstProv = dtPropFacturas.data().filter(prov => numpro === prov.numpro).toArray()
               ,sumGroup = lstProv.filter((prov ,a ,b) => {
                return (prov.autorizado === "A" || prov.autorizado === "E");
            }).reduce((sumGroup, current) => sumGroup + (current.monto_plan), 0);
            tblPropFacturas.find(`tr.prov${numpro} td:eq(3)`).text(maskNumero(sumGroup));
        }
        function initDataTblPropFacturas() {
            dtPropFacturas = tblPropFacturas.DataTable({
                 paging: false
                ,destroy: true
                ,ordering:false
                ,language: dtDicEsp
                ,"sScrollX": "100%"
                ,"sScrollXInner": "100%"
                ,"bScrollCollapse": true
                ,scrollY: '65vh'
                ,scrollCollapse: true
                ,"bLengthChange": false
                ,"searching": false
                ,"bFilter": true
                ,"bInfo": true
                ,"bAutoWidth": false
                ,"createdRow": function(row, data, dataIndex) {
                    if (data.bloqueado) {
                      $(row).css('background-color', '#e8403d');
                    }
                  }
                ,drawCallback: function (settings) {
                    var api = this.api(),
                        rows = api.rows({ page: 'current' }).nodes(),
                        head = null;
                        api.column({ page: 'current' }).data().each((group, i, dtable) => {
                            const dataBefore = dtable.data()[i-1];
                            const data = dtable.data()[i];
                            if(data.activo_fijo){
                                $(rows).eq(i).addClass('activo_fijo');
                            }
                            if(i>0){
                                if (dataBefore.numpro !== data.numpro) {
                                    let lstProv = dtable.data().filter(prov => dataBefore.numpro === prov.numpro);
                                    let suma = lstProv.reduce((suma, current) => suma + current.saldo, 0);
                                    let sumProv = lstProv.filter(prov => {
                                        return (prov.autorizado === "A" || prov.autorizado === "E");
                                    }).reduce((suma, current) => suma + current.saldo, 0);
                                    $(rows).eq(i).before(`<tr class="prov${dataBefore.numpro}">
                                                            <td colspan="7" class="fondoTotal">Cuenta Bancaria:<span style="float:right">Saldo del proveedor</span></td>
                                                            <td class="text-right fondoTotal">${maskNumero(suma)}</td>
                                                            <td class="text-right fondoTotal fondoAPagar">${maskNumero(suma)}</td>
                                                            <td class="text-right fondoTotal">${maskNumero(sumProv)}</td>
                                                            <td></td>
                                                        </tr>`);
                                }
                                if(i==dtable.length-1){
                                    let lstProv = dtable.data().filter(prov => data.numpro === prov.numpro);
                                    let suma = lstProv.reduce((suma, current) => suma + current.saldo, 0);
                                    let sumProv = lstProv.filter(prov => {
                                        return (prov.autorizado === "A" || prov.autorizado === "E");
                                    }).reduce((suma, current) => suma + current.saldo, 0);
                                    $(rows).eq(i).after(`<tr class="prov${data.numpro} fondoTotal">
                                                            <td colspan="7">Cuenta Bancaria:<span style="float:right">Saldo del proveedor</span></td>
                                                            <td class="text-right fondoTotal">${maskNumero(suma)}</td>
                                                            <td class="text-right fondoTotal fondoAPagar">${maskNumero(suma)}</td>
                                                            <td class="text-right fondoTotal">${maskNumero(sumProv)}</td>
                                                            <td></td>
                                                        </tr>`);
                                }
                            }
                        });
                }
                ,columns: [
                    { data: 'proveedor',createdCell: (td, data, rowData) => $(td).html(`${rowData.numpro} - ${data}`)},
                    { data: 'factura',width: "50px" },
                    { data: 'vence'},
                    { data: 'tmDesc'},
                    { data: 'centroCostos',createdCell: (td, data, rowData) => $(td).html(`${rowData.cc} - ${data}`)},
                    { data: 'referenciaoc'},
                    { data: 'concepto'},
                    { data: 'monto_plan' ,class:'text-right' ,createdCell: (td, data, rowData) => $(td).html(`${maskNumero(data)} ${rowData.moneda}`)},
                    //{ data: 'monto_plan', class: 'text-right' },
                    { data: 'autorizado' ,width: "150px",createdCell: (td, data, rowData) => {
                            if (!rowData.bloqueado) {
                                let cbo = $(`<div class='input-group'>
                                            <select class='form-control auth'></select>
                                            <span class='input-group-btn'>
                                                <button type='button' class='btn btn-default' style='display:none;'> B</button>
                                            </span>
                                        </div>`);
                                cbo.find('select').addClass("form-control");
                                cbo.find('select').fillComboItems(lstAutorizado, true, data);
                                cbo.find('select').prop("disabled", true);
                                $(td).html(cbo);
                            } else {
                                let text = rowData.descripcionBloqueo;
                                $(td).html(text);
                            }
                            
                        }
                    },
                    { 
                        data: 'factura' ,createdCell: (td, data) => { 
                            let cbo = $(`<input type='checkbox'/>`);
                            cbo.addClass("form-control autorizar");
                            $(td).html(cbo);
                        }
                    },
                ],
                columnDefs: [
                    {
                        targets: [8],
                        render: function (data, type, row) {
                            let input = '<input type="text" class="form-control inputAPagar" value="' + maskNumero(data) + '"' + (row.bloqueado ? 'disabled' : '') + ' />';
                            return input;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblPropFacturas.on('change', '.giro', function (event) {
                        setSaldosTotales();
                    });
                    tblPropFacturas.on('change', '.auth', function (event) {
                        let data = dtPropFacturas.row($(this).closest('tr')).data()
                           ,auth = this.value;
                        if (auth === 'B') {
                            $(this).prop('disabled', true);
                        }
                        data.autorizado = auth;
                        setSaldoProv(data);
                        setSaldosTotales();
                        if (auth != 'B') {
                            $(this).parent().parent().parent().find(".giro").val("");
                        }
                        
                    });
                    tblPropFacturas.on('click', '.btn', function (event) {
                        let select = $(this).closest(`div`).find("select")
                           ,esDisabled = select.prop('disabled');
                        select.prop('disabled', !esDisabled);
                        select.val(" ");
                        setSaldoProv(dtPropFacturas.row($(this).closest('tr')).data());
                        setSaldosTotales();
                    });
                    tblPropFacturas.find('tbody').on('click', 'tr', function () {
                        let data = dtPropFacturas.row(this).data();
                        if (data !== undefined) {
                            setFooter(data);   
                        }
                    });

                    tblPropFacturas.find('tbody').on('change', '.inputAPagar', function(event) {
                        const datos = tblPropFacturas.DataTable().row($(this).closest('tr')).data();
                        datos.monto_plan = unmaskNumero($(this).val());
                        
                        const valorAnterior = unmaskNumero(event.currentTarget.defaultValue);
                        event.currentTarget.defaultValue = unmaskNumero($(this).val());

                        $(this).val(maskNumero(unmaskNumero($(this).val())));

                        const tdTotalAPagar = unmaskNumero(tblPropFacturas.find('.prov' + datos.numpro).find('td.fondoAPagar').text());
                        tblPropFacturas.find('.prov' + datos.numpro).find('td.fondoAPagar').html(maskNumero(tdTotalAPagar - valorAnterior + datos.monto_plan));

                        if (datos.autorizado == 'A' || datos.autorizado == 'E') {
                            const tdTotalAPagar = unmaskNumero(tblPropFacturas.find('.prov' + datos.numpro).find('td').eq(3).text());
                            tblPropFacturas.find('.prov' + datos.numpro).find('td').eq(3).html(maskNumero(tdTotalAPagar - valorAnterior + datos.monto_plan));
                        }

                        setSaldosTotales();

                        if (datos.moneda == 'MN') {
                            txtPropHTotal.text(maskNumero(unmaskNumero(txtPropHTotal.text()) - valorAnterior + datos.monto_plan));
                        } else {
                            txtPropHTotalDll.text(maskNumero(unmaskNumero(txtPropHTotalDll.val()) - valorAnterior + datos.monto_plan));
                        }
                    });
                }
            });
        }
        function chngSetAllSelOpt() {
            let estodo = !this.value,
                select = $(this).next().find("select[multiple]");
            this.value = estodo;
            limpiarMultiselect(select);
            if (estodo) {
                let lstValor = $(`#${select.attr("id")}`).find(`option`).toArray().map(option => option.value);
                select.val(lstValor);
                convertToMultiselect(select);
            }
        }
        function initForm() {
            //getLimitProv();
            lstAutorizado.push({Text: "Pendiente", Value:" "});
            lstAutorizado.push({Text: "A. Cheque", Value:"A"});
            lstAutorizado.push({Text: "Eletrónico", Value:"E"});
            lstAutorizado.push({Text: "Bloqueado", Value:"B"});
            //dpPropCorte.datepicker();
            //if(_gpEmpresa!=2){
            //    selPropCC.fillCombo('/Administrativo/Poliza/getCC', null, true, null);
            //    convertToMultiselect(selPropCC);
            //}
            //else{
            //    convertToMultiselect(selPropCC);
            //    $(".selPropCC").hide();
            //}
            
            //selPropTipoProceso.fillCombo('/Administrativo/Propuesta/cboTipoOperacion', null, true, null);
            
            //inputGroupBtn.val(false);
            initDataTblPropFacturas();
            setFooter({cc: "" ,referenciaoc:"" ,saldo:0 ,moneda: ""});
            txtPropHTotal.text(maskNumero(0));
            txtPropHSel.text(maskNumero(0));
            txtPropHTotalDll.text(maskNumero(0));
            txtPropHSelDll.text(maskNumero(0));
        }
        init();
    }
        $(document).ready(() => {
            Administracion.Contabilidad.Propuesta.ResumenProveedor = new ResumenProveedor();
        });
})();